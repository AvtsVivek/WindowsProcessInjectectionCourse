// EnumProcExOne.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <Windows.h>
#include <Psapi.h>

#pragma comment(lib, "wtsapi32")
#pragma comment(lib, "ntdll")

int ProcEnumWithEnumProc();

int Error(const char* text);

int main()
{
	std::cout << "Hello World asdf!\n";
	return ProcEnumWithEnumProc();
}

int ProcEnumWithEnumProc() {
	DWORD size = 512 * sizeof(DWORD);
	DWORD* ids = nullptr;
	DWORD needed = 0;

	//DWORD pids[10000];
	//EnumProcesses(pids, sizeof(pids), &needed);

	for (;;) {
		ids = (DWORD*)realloc(ids, size);
		if (ids == nullptr)
			break;

		if (!EnumProcesses(ids, size, &needed)) {
			free(ids);
			return Error("Failed in EnumProcesses");
		}

		if (size >= needed)
			break;

		size = needed + sizeof(DWORD) * 16;
	}
	if (ids == nullptr)
		return Error("Out of memory");

	WCHAR name[MAX_PATH];
	for (DWORD i = 0; i < needed / sizeof(DWORD); i++) {
		DWORD id = ids[i];
		printf("PID: %6u", id);
		HANDLE hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE, id);
		if (hProcess) {
			DWORD size = _countof(name);
			if (QueryFullProcessImageName(hProcess, 0, name, &size)) {
				printf(" %ws", name);
			}
			CloseHandle(hProcess);
		}
		printf("\n");
	}

	free(ids);
	return 0;
}

int Error(const char* text) {
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}


