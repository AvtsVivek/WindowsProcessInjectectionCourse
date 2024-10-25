
#include <Windows.h>
#include <stdio.h>
#include <string>
#include <TlHelp32.h>
#include <WtsApi32.h>
#include <Psapi.h>

#include <iostream>

//  Forward declarations:
int ProcEnumWithWTS();
std::wstring GetUserNameFromSid(PSID sid);
int Error(const char* text);

int main()
{
	ProcEnumWithWTS();
	return 0;
}

int ProcEnumWithWTS() {

	DWORD level = 1;
	PWTS_PROCESS_INFO_EX info;
	DWORD count;

	if (!WTSEnumerateProcessesEx(WTS_CURRENT_SERVER_HANDLE, &level,
		WTS_ANY_SESSION, (PWSTR*)&info, &count))
		return Error("Failed in calling WTSEnumerateProcessesEx");

	for (DWORD i = 0; i < count; i++)
	{
		PWTS_PROCESS_INFO_EX pinfo = info + i;
		printf("PID: %6u Session: %u (%ws) Username: %ws\n",
			pinfo->ProcessId, pinfo->SessionId, pinfo->pProcessName,
			GetUserNameFromSid(pinfo->pUserSid).c_str());
	}

	WTSFreeMemoryEx(WTSTypeProcessInfoLevel1, info, count);

	return 0;
}

std::wstring GetUserNameFromSid(PSID sid) {
	if (sid == nullptr)
		return L"";

	WCHAR name[32], domain[32];
	DWORD len = _countof(name);
	DWORD domainLen = _countof(domain);
	SID_NAME_USE use;
	if (!LookupAccountSid(nullptr, sid, name, &len, domain, &domainLen, &use))
		return L"";

	return std::wstring(domain) + L"\\" + name;
}

int Error(const char* text) {
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}



