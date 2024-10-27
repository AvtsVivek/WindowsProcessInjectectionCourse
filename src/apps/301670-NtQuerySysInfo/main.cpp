#include <Windows.h>
#pragma comment(lib, "ntdll")

#define STATUS_BUFFER_TOO_SMALL 0xC0000004
#include <iostream>


enum SYSTEM_INFORMATION_CLASS {
	SystemExtendedProcessInformation = 57
};

typedef struct _UNICODE_STRING {
	USHORT Length;
	USHORT MaximumLength;
	PWSTR  Buffer;
} UNICODE_STRING;

typedef struct _SYSTEM_PROCESS_INFORMATION {
	ULONG NextEntryOffset;
	ULONG NumberOfThreads;
	LARGE_INTEGER WorkingSetPrivateSize; // since VISTA
	ULONG HardFaultCount; // since WIN7
	ULONG NumberOfThreadsHighWatermark; // since WIN7
	ULONGLONG CycleTime; // since WIN7
	LARGE_INTEGER CreateTime;
	LARGE_INTEGER UserTime;
	LARGE_INTEGER KernelTime;
	UNICODE_STRING ImageName;
	int BasePriority;
	HANDLE UniqueProcessId;
	HANDLE InheritedFromUniqueProcessId;
	ULONG HandleCount;
	ULONG SessionId;
	ULONG_PTR UniqueProcessKey; // since VISTA (requires SystemExtendedProcessInformation)
	SIZE_T PeakVirtualSize;
	SIZE_T VirtualSize;
	ULONG PageFaultCount;
	SIZE_T PeakWorkingSetSize;
	SIZE_T WorkingSetSize;
	SIZE_T QuotaPeakPagedPoolUsage;
	SIZE_T QuotaPagedPoolUsage;
	SIZE_T QuotaPeakNonPagedPoolUsage;
	SIZE_T QuotaNonPagedPoolUsage;
	SIZE_T PagefileUsage;
	SIZE_T PeakPagefileUsage;
	SIZE_T PrivatePageCount;
	LARGE_INTEGER ReadOperationCount;
	LARGE_INTEGER WriteOperationCount;
	LARGE_INTEGER OtherOperationCount;
	LARGE_INTEGER ReadTransferCount;
	LARGE_INTEGER WriteTransferCount;
	LARGE_INTEGER OtherTransferCount;
} SYSTEM_PROCESS_INFORMATION;

// The following definition is obtained from the process hacker project
// https://github.com/winsiderss/phnt/blob/7675984a0f0d49f5be79cd43854fa06d57ddbb1e/ntexapi.h#L4597C1-L4597C25
// This function is very generic.

extern "C" NTSTATUS NTAPI NtQuerySystemInformation(
	_In_ SYSTEM_INFORMATION_CLASS SystemInformationClass,
	_Out_writes_bytes_opt_(SystemInformationLength) PVOID SystemInformation,
	_In_ ULONG SystemInformationLength,
	_Out_opt_ PULONG ReturnLength
);

int ProcEnumWithNtQuerySystem();

int main()
{
    std::cout << "Hello World!\n";
	return ProcEnumWithNtQuerySystem();
}

int ProcEnumWithNtQuerySystem() {
	// allocate large-enough buffer
	ULONG size = 1 << 18;
	void* buffer = nullptr;

	for (;;) {
		buffer = realloc(buffer, size);
		if (!buffer)
			return 1;

		ULONG needed;
		NTSTATUS status = NtQuerySystemInformation(SystemExtendedProcessInformation, buffer, size, &needed);
		if (status == 0)	// success
			break;

		if (status == STATUS_BUFFER_TOO_SMALL) {
			size = needed + (1 << 12);
			continue;
		}
		// some other error
		return status;
	}

	auto p = (SYSTEM_PROCESS_INFORMATION*)buffer;
	for (;;) {
		printf("PID: %6u PPID: %6u, Session: %3u, Threads: %3u %ws\n",
			HandleToULong(p->UniqueProcessId),
			HandleToULong(p->InheritedFromUniqueProcessId),
			p->SessionId, p->NumberOfThreads,
			p->ImageName.Buffer ? p->ImageName.Buffer : L"");

		if (p->NextEntryOffset == 0)	// enumeration end
			break;

		p = (SYSTEM_PROCESS_INFORMATION*)((BYTE*)p + p->NextEntryOffset);
	}
	free(buffer);

	return 0;
}

