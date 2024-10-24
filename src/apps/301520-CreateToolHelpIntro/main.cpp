// CreateToolHelpIntro.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <Windows.h>
#include <stdio.h>
#include <TlHelp32.h>

int Error(const char* text) {
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}

int ProcEnumWithToolhelp() {
	HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (hSnapshot == INVALID_HANDLE_VALUE)
		return Error("Failed to create snapshot");

	PROCESSENTRY32 pe;
	pe.dwSize = sizeof(pe);

	if (!Process32First(hSnapshot, &pe))
		return Error("Failed in Process32First");

	do {
		printf("PID:%6u (PPID:%6u) (Threads: %3u) (Priority: %2u): %ws\n",
			pe.th32ProcessID, pe.th32ParentProcessID,
			pe.cntThreads, pe.pcPriClassBase, pe.szExeFile);
	} while (Process32Next(hSnapshot, &pe));

	CloseHandle(hSnapshot);
	return 0;
}

int main()
{

	ProcEnumWithToolhelp();
	std::cout << "Hello World!\n";
}



