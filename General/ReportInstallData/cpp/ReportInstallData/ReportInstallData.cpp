// InstallData.cpp : Defines the entry point for the console application.
//

// This project is configured to include path "C:\Program Files\Solid Edge ST6\Program".
// If you are getting build errors, you may have to update the path to your Solid Edge installation.
// Project -> Properties -> Configuration Properties -> VC++ Directories -> Library Directories
// Specifically, the #import directives in stdafx.h have to be able to resolve the .tlb(s).

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
    HRESULT hr = S_OK;

    // Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SEInstallDataLib::ISEInstallDataPtr pInstallData = NULL;

		// Create a new instance of 'SolidEdge.InstallData'.
		hr = pInstallData.CreateInstance(L"SolidEdge.InstallData");

		// Solid Edge language.  i.e. 'English', 'German', etc.
		LONG lLanguageID = pInstallData->GetLanguageID();

		// Solid Edge version
		LONG lMajor = pInstallData->GetMajorVersion();
		LONG lMinor = pInstallData->GetMinorVersion();
		LONG lServicePack = pInstallData->GetServicePackVersion();
		LONG lBuild = pInstallData->GetBuildNumber();

		// Parasolid version
		LONG lParasolidMajor = pInstallData->GetParasolidMajorVersion();
		LONG lParasolidMinor = pInstallData->GetParasolidMinorVersion();

		// Solid Edge version string
		_bstr_t bstrVersion = pInstallData->GetVersion();

		// Get path to Solid Edge installation directory.  Typically, 'C:\Program Files\Solid Edge XXX'.
		_bstr_t bstrInstalledPtah = pInstallData->GetInstalledPath();

		// Output info to screen.
		::wprintf(L"LanguageID: %li\n", lLanguageID);
		::wprintf(L"Version: %li.%li.%li.%li\n", lMajor, lMinor, lServicePack, lBuild);
		::wprintf(L"VersionString: %s\n", bstrVersion.GetBSTR());
		::wprintf(L"ParasolidVersion: %li.%li\n", lParasolidMajor, lParasolidMinor);
		::wprintf(L"InstalledPath: %s\n", bstrInstalledPtah.GetBSTR());
	}

    // Uninitialize COM.
    ::CoUninitialize();

    return 0;
}

