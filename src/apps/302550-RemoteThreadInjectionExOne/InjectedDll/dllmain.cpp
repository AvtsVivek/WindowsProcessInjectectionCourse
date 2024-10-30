// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

// This function is called in several cases, and one of the cases is when this dll is loaded into a process.

// 9. For our case, it is process attach, which is `DLL_PROCESS_ATTACH`, the first case.

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    {
        MessageBox(nullptr, L"Injected into another process", L"Injected", MB_OK);
    }
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
