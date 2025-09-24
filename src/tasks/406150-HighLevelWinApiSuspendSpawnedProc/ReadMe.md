# Notes.

1. This example is similar to earlier one.
2. The only difference is the following code.
   
```cs
foreach (ProcessThread thread in process.Threads)
{
    IntPtr pThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);

    if (pThread == IntPtr.Zero)
    {
        continue;
    }

    SuspendThread(pThread);

    CloseHandle(pThread);
}
```