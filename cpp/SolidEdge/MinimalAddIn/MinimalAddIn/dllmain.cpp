// dllmain.cpp : Implementation of DllMain.

#include "stdafx.h"
#include "resource.h"
#include "MinimalAddIn_i.h"
#include "dllmain.h"
//#include "xdlldata.h"

CMinimalAddInModule _AtlModule;

// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
    _AtlModule.SetResourceInstance(hInstance);
    return _AtlModule.DllMain(dwReason, lpReserved); 
}
