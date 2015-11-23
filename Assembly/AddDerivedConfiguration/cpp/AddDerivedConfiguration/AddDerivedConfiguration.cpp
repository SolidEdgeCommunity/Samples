// AddDerivedConfiguration.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
	HRESULT hr = S_OK;
	CString strConfigName;

	// Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SolidEdgeFramework::ApplicationPtr pApplication = NULL;
		SolidEdgeAssembly::AssemblyDocumentPtr pAssemblyDocument = NULL;
		SolidEdgeAssembly::ConfigurationsPtr pConfigurations = NULL;
		SolidEdgeAssembly::ConfigurationPtr pConfiguration = NULL;

		// Attempt to connect to a running instance of Solid Edge.
		IfFailGo(pApplication.GetActiveObject(L"SolidEdge.Application"));

		// Get a reference to the active assembly document.
		pAssemblyDocument = pApplication->ActiveDocument;

		if (pAssemblyDocument != NULL)
		{
			// Get a reference to the Configurations collection.
			pConfigurations = pAssemblyDocument->Configurations;

			LONG c = pConfigurations->Count;

			if (pConfigurations->Count > 0)
			{
				// Get a reference tot he Configurations collection.
				pConfiguration = pConfigurations->Item((LONG)1);

				// Configuration name has to be unique so for demonstration
				// purposes, use a random number.
				strConfigName.Format(L"%s %d", L"Configuration", rand());

				/* NOTE - Example is not working. Need help :-( */
				SAFEARRAY* sa;
				SAFEARRAYBOUND aDim;
				aDim.lLbound = 0;
				aDim.cElements = 1;

				sa = SafeArrayCreate(VT_BSTR, 1, &aDim);
				long index = 0;
				hr = SafeArrayPutElement(sa, &index, (void*)strConfigName.AllocSysString());

				VARIANT vConfigList;
				VariantInit(&vConfigList);
				vConfigList.vt = VT_ARRAY | VT_BSTR;
				vConfigList.parray = sa;

				pConfigurations->AddDerivedConfig(1, 0, 0, &vConfigList, NULL, NULL, strConfigName.AllocSysString());

				SafeArrayDestroy(sa);
			}
		}
		else
		{
			wprintf(L"No active document.\n");
		}
	}

Error:

	// Uninitialize COM.
	::CoUninitialize();

	return 0;
}

