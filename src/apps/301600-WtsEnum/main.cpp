
#include <Windows.h>
#include <stdio.h>
#include <WtsApi32.h>
#include <iostream>

#pragma comment(lib, "wtsapi32.lib")

//  Forward declarations:
int ProcEnumWithWTS();
std::wstring GetUserNameFromSid(PSID sid);
int Error(const char *text);
bool EnableDebugPrivilege();

int main()
{
	///EnableDebugPrivilege();
	return ProcEnumWithWTS();
}

int ProcEnumWithWTS()
{
	DWORD level = 1;
	PWTS_PROCESS_INFO_EX info;
	// OR. You can use above or below, any of the two.
	// WTS_PROCESS_INFO_EX *info;

	// The following count indicates the number of process that we get back.
	DWORD count;

	// The first parameter is the server we wnat to use for the enumeration process.
	// WTS_CURRENT_SERVER_HANDLE has a value of 0, and this indicates this machine.
	// The next parameter level should be set to zero or 1.
	// 0 provides basic information and 1 provides extended info.
	// WTS_ANY_SESSION is the third parameter. This is the session.
	// Because we want to see all of the processes on the machine, we will use WTS_ANY_SESSION
	// The next param is a pointer to unicode string.
	BOOL success_result = WTSEnumerateProcessesEx(WTS_CURRENT_SERVER_HANDLE, &level,
												  WTS_ANY_SESSION, (PWSTR *)&info, &count);

	if (!success_result)
	{
		return Error("Failed in calling WTSEnumerateProcessesEx");
	}

	for (DWORD i = 0; i < count; i++)
	{
		PWTS_PROCESS_INFO_EX pinfo = info + i;
		printf("PID: %6u   Threads: %3u Session: %u (%ws) Username: %ws\n",
			   pinfo->ProcessId, pinfo->NumberOfThreads, pinfo->SessionId, pinfo->pProcessName,
			   GetUserNameFromSid(pinfo->pUserSid).c_str());
	}

	// WTSTypeProcessInfoLevel1
	WTSFreeMemoryEx(WTSTypeProcessInfoLevel1, info, count);

	return 0;
}

std::wstring GetUserNameFromSid(PSID sid)
{
	if (sid == nullptr)
		return L"";

	WCHAR name[32], domain[32];
	DWORD len = _countof(name);
	DWORD domainLen = _countof(domain);

	// The following is an enumeration that indicates what kind of sid is this.
	SID_NAME_USE use;
	// Given a sid, get back the user name.
	// nullptr here means local system, I dont want to look at other system.
	if (!LookupAccountSid(nullptr, sid, name, &len, domain, &domainLen, &use))
		return L"";

	return std::wstring(domain) + L"\\" + name;
}

int Error(const char *text)
{
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}

bool EnableDebugPrivilege()
{
	HANDLE hToken;
	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, &hToken))
		return false;

	TOKEN_PRIVILEGES tp;
	tp.PrivilegeCount = 1;
	tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	if (!LookupPrivilegeValue(nullptr, SE_DEBUG_NAME, &tp.Privileges[0].Luid))
		return false;

	BOOL success = AdjustTokenPrivileges(hToken, FALSE, &tp, sizeof(tp), nullptr, nullptr);
	CloseHandle(hToken);

	return success && GetLastError() == ERROR_SUCCESS;
}