// CreateToolHelpIntro.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <Windows.h>
#include <stdio.h>
#include <TlHelp32.h>

//  Forward declarations:
int ProcEnumWithToolhelp();

int main()
{
	ProcEnumWithToolhelp();
	std::cout << "Process Enumeration completed!\n";
}


int Error(const char* text) {
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}

int ProcEnumWithToolhelp() {

	// https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot
	HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (hSnapshot == INVALID_HANDLE_VALUE) // The value of INVALID_HANDLE_VALUE is -1 (cast to HANDLE).
		return Error("Failed to create snapshot");

	// This structure holds information after we start the enumeration process.
	// Press F12 to see the structure
	PROCESSENTRY32 pe;

	// The following is required, 
	// Set the size of the structure before using it.
	pe.dwSize = sizeof(pe);

	if (!Process32First(hSnapshot, &pe))
		return Error("Failed in Process32First");

	do {
		printf("PID:%6u (PPID:%6u) (Threads: %3u) (Priority: %2u): %ws\n",
			pe.th32ProcessID, pe.th32ParentProcessID,
			pe.cntThreads, pe.pcPriClassBase, pe.szExeFile);
	} while (Process32Next(hSnapshot, &pe));

	// This is not important because, once the process is exited, the kernel automatically 
	// closes all of the handles in the handle table of the process.
	// But this is a good habit to close it ourselves.
	CloseHandle(hSnapshot);
	return 0;
}




