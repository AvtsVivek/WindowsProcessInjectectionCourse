

Adjusting Process Token Privileges
As some of you know, Windows NT is a far more secure OS than Windows 9x, both
from a User and Developer standpoint. For a developer, this aspect of NT brings a new challenge:
Getting programs to work both under 9x and NT comfortably.

Under NT, many of the Win32 API functions require that the process has a
certain level of privilege to execute, which is a good thing, and a great advance on security,
compared to 9x. But this also involves more work for the programmer, because some times he's
responsible for getting the program to work for user's which do not have Administrator privileges.
Don't believe that just because you're not doing low level stuff, or changing user, rights or whatever,
you won't need to know this, because you will. A specific example, the SetSystemTime()
call: It's simple, just changes the system time, but requires that the process has SE_SYSTEMTIME_NAME
privilege on it's tokens, which is not enabled by default.

But, What's an access token? A token is:

A group of security attributes permanently attached to a process when a user logs on to the
operating system. An access token contains privileges and security identifiers for a user,
global group, or local group. The privileges regulate the use of some system services and
the security identifiers regulate access to objects that are protected by access-control
lists (ACLs). There are two kinds of access token: primary and impersonation.

When adjusting a process access tokens, you have to be careful to leave the
tokens in the state they were in. Don't assume that you can just change them and leave it like
that (unless you are enabling the SE_SHUTDOWN_NAME privilege, in which case it doesn't matter),
you should always return them to the state you found them. Following this simple rule
ensures that your program won't cause security troubles later. 

Now, let's go a little deeper. What's the whole purpose of
tokens? Tokens are created by the Local Security Authority (lsass.exe), and
allow the system to keep track of some information related to the process. The
most important information a process token holds is the SID of the user account
the process is running under. It also carries the list of SID's for groups the
user is member of, and, obviously, which privileges the user has been granted.
All this allows the system to easily determine if the process should be granted
access to a protected resource. Another important thing the token carries is a
default DACL (Discretionary Access Control List) that is used to assign
default security settings to objects (i.e. files) created by the process in
behalf of the user. Keep in mind that this DACL is only used if the process
doesn't explicitly supply a security descriptor when the object is created.

One thing I haven't mentioned so far is that threads can also
have tokens associated, which you can get at via the OpenThreadToken() api.
However, you'll soon notice that most of the time calling this api will fail.
Why? Because usually a thread won't have a token attached, in which case
the system will use the token of the process the thread is running on. When a
thread does have a token attached to it, it's because it is impersonating
another user, in which case it is said that the thread runs under a different
security context. Is important to notice that a thread can also impersonate the
same user account the process is running under, via the ImpersonateSelf() api.

There are some basic steps to
adjusting the tokens, so let's review them, and later I'll present an example.


Call OpenProcessToken() with at least the TOKEN_ADJUST_PRIVILEGE and
TOKEN_QUERY flags.
Use LookupPrivilegeValue() to get the LUID (Locally Unique Identifier) of
the privilege you want to adjust.
Call AdjustTokenPrivileges() to adjust the tokens.
Do whatever calls you need to do to accomplish your task.
Call AdjustTokenPrivileges() again to set the old privileges back and
leave the tokens as found.
Close the Token handle.
Let's see an example: Here we enable the SE_SYSTEMTIME_NAME privilege
to be able of setting the system time:

```cpp

HANDLE      hToken;     /* process token */
TOKEN_PRIVILEGES tp;    /* token provileges */
TOKEN_PRIVILEGES oldtp;    /* old token privileges */
DWORD    dwSize = sizeof (TOKEN_PRIVILEGES);
LUID     luid;          

/* now, set the SE_SYSTEMTIME_NAME privilege to our current
*  process, so we can call SetSystemTime()
*/

if (!OpenProcessToken (GetCurrentProcess(), 
    TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken))
{

   printf ("OpenProcessToken() failed with code %d\n", GetLastError());

   return 1;

}

if (!LookupPrivilegeValue (NULL, SE_SYSTEMTIME_NAME, &luid))
{

   printf ("LookupPrivilege() failed with code %d\n", GetLastError());

   CloseHandle (hToken);

   return 1;

}


ZeroMemory (&tp, sizeof (tp));

tp.PrivilegeCount = 1;

tp.Privileges[0].Luid = luid;

tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;


/* Adjust Token privileges */
if (!AdjustTokenPrivileges (hToken, FALSE, &tp, sizeof(TOKEN_PRIVILEGES),
         &oldtp, &dwSize))
{

   printf ("AdjustTokenPrivileges() failed with code %d\n", GetLastError());

   CloseHandle (hToken);

   return 1;

}


/* Set time */
if (!SetSystemTime (&stCurrentTime))
{

   printf ("SetSystemTime() failed with code %d\n", GetLastError());

   CloseHandle (hToken);

   return 1;
}


/* disable SE_SYSTEMTIME_NAME again */

AdjustTokenPrivileges (hToken, FALSE, &oldtp, dwSize, NULL, NULL);

if (GetLastError() != ERROR_SUCCESS)
{

   printf ("AdjustTokenPrivileges() failed with code %d\n", GetLastError());

   CloseHandle (hToken);

   return 1;
}
CloseHandle (hToken);
```

As you can see from the example, it's not really a hard thing to do, but
requires several non trivial calls just to do a simple task.

Finally, let's consider some linguistic issues. What exactly the
difference between a privilege and a right is, that's something
most people don't clearly understand. The problem is terminology. The original
Windows NT docs referred to privileges as Advanced User Rights, which
doesn't help much, either.

One way to clarify things is by understanding that permissions
(a.k.a. rights) are always associated to a particular object. You have
the permission to open a file, to read from it, etc. There are a set of generic
rights, but every object type supplies it's own rights that only make sense for
it. The system knows what rights a user have to access an object by looking at
the ACL (Access Control List) associated with the object. Usually, ACL's
are saved along with the object, so they are persistent if the object itself is.
For example, a file's ACL is saved along with it on disk.

 Privileges, on the other hand, are associated with
particular actions on the system, and are granted to users, not objects.
Privileges allow a user to override permissions, and this is why you have to be
careful when granting them. A user with SE_RESTORE_NAME privilege could easily
overwrite almost any file on the system..

