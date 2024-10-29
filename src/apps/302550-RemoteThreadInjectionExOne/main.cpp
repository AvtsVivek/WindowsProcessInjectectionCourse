// CreateRemoteThread.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <Windows.h>
#include <stdio.h> // for printf.

int Error(const char *text)
{
	std::cout << "Here we go...!\n";
	printf("%s (%u)\n", text, GetLastError());
	return 1;
}

// We need to supply the dll path and the process as the command line args.
int main(int argc, const char *argv[])
{
	if (argc < 3)
	{
		printf("Usage: remotethread <pid> <dllpath>\n");
		return 0;
	}

	// First get the process id. The first arg should be the process id.
	int pid = atoi(argv[1]);

	// OpenProcess gets a handle to the target process. 
	// The first param is what kind of access we should have to the target process.
	// If the access is too little, then we may not be able to do anything with it.
	// If we are asking too much, then we may be able to the the handle in the fist place.
	// PROCESS_VM_WRITE is needed to make a call to the WriteProcessMemory function.
	// PROCESS_VM_OPERATION is needed because, we will be allocate some memory to in the target process to hold, the path to the dll. 
	// PROCESS_CREATE_THREAD is ndded, as we have to create a thread in the target process.
	HANDLE hProcess = OpenProcess(PROCESS_VM_WRITE | PROCESS_VM_OPERATION | PROCESS_CREATE_THREAD, FALSE, pid);
	
	// Creation may fail, as some processes may have higher level of previlages the attacker might have. 
	// If we fail, then we get back the null.

	if (!hProcess)
		return Error("Failed to open process");

	// We need to write the path to the dll into the target process.
	// To do that we need to first allocate some memory into the target process.
	// Thats because, when the load library function is going to be called,
	// it can only be called with a pointer to some buffer within the process
	// where a load library is being called from, that is the target process.
	// To do that, we will use VirtualAllocEx. 
	// This is an extended version of VirtualAlloc that can allocate memory in the current process.
	// Simple function such as malloc are ineffective here. 
	// The second parameter is nullptr. 
	// Using the second param, we can specify a preferred address. 
	// But we dont have to worry about it, anywhere its ok. So pass nullptr
	// 1 << 12, four kilo bytes, one page. These types of allocations work only in pages.
	// So even if we ask for a 10 bytes, we will anyway be allocated 1 page.
	// MEM_COMMIT | MEM_RESERVE, Reserve a region and commit immediately, the memory should be available, once I try to access it.
	// Then I have to provide a protection to these pages, that is read and write.
 
	void *buffer = VirtualAllocEx(hProcess, nullptr, 1 << 12, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

	// This can technically fail, but its very rare.
	if (!buffer)
		return Error("Failed to allocate memory");

	// Now we want to write to that memory in that target process, the path to the dll.
	// The second param is the path to the dll.
	// We want to write in the buffer. Thats the address in that target process.
	// Next the data. argv[2].
	// How much data we want to write? The length of the the path.

	BOOL writeSuccess = WriteProcessMemory(hProcess, buffer, argv[2], strlen(argv[2]), nullptr);

	// This call should not fail, but just in case.
	if (!writeSuccess)
	{
		return Error("Failed in WriteProcessMemory");
	}

	// THe last thing we need to do is to create the thread in the target process. 
	// Then we instruct the thread to load our dll.
	// CreateThread method creates thread in the current process. 
	// CreateRemoteThread creates in a remote process. 

	// This functioin is usually used by debuggers. When we try to break into a process, the debugger actually creates a remote thread
	// And then gives the break instruction.  

	// The first parameter is a handle to the target process. That handle must have process create thread access mask.
	// We have it, because, we asked for it when we were creating process handle. 
	// The second param is the security attributes, here we dont care about it.
	// The third parameter is the size of the stack for that new thread. We specify 0, meaning default. This does not matter much.
	// The fourth parameter is the most important. This is the start address of where the thread should start executing the code.
	// And the trick here for CreateRemoteThread is that a thread function accepts a pointer, and returns something. 
	// The load library function accepts a pointer as well and returns something. 
	// So we are going to instruct the thread to execute the load library function.   
	// The way to do that is to locate where the load library function is. 
	// We need to use the GetProcAddress function. The load library function is implimented in the module kernel32 module.
	// So simply put, we are creating a remote thread and asking it to execute LoadLibrary fucntion.
	// Now we want to create the thread in another process and execute the load library in another(target) process
	// But the loadlibrary functions address that is present in the attacker process is same as the load library address present in the target process. 
	// Thats because, kernal32 module is a common module for all of the processes in the windows machine.
	// So its is certain that where ever the load library funcitoin is in my(attacker) process, is in the same location of the target process.  
	// From the compiler's perpective, the type of the GetProcAddress function should be LPTHREAD_START_ROUTINE.
	// https://stackoverflow.com/a/19472879/1977871 
	// The next parameter is the parameter that we need to pass to the load library function. 
	// Load library needs a path to a dll. So buffer contains the path to the dll. 
	// The next params are some creation flags, which are not important for us. So pass 0.
	// The final parameter is the id of the new thread. We are not going to do anything with that id for this example.
	// So nullprt should be fine. 
	HANDLE hThread = CreateRemoteThread(hProcess, nullptr, 0, 
										(LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandle(L"kernel32"), "LoadLibraryA"),
										buffer, 0, nullptr);
	if (!hThread)
		return Error("Failed to create remote thread");

	printf("Remote thread created successfully!");

	WaitForSingleObject(hThread, 5000);
	VirtualFreeEx(hProcess, buffer, 0, MEM_RELEASE);
	CloseHandle(hProcess);

	return 0;
}
