// AddSheet.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
	HRESULT hr = S_OK;

	// Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SolidEdgeFramework::ApplicationPtr pApplication = NULL;
		SolidEdgeDraft::DraftDocumentPtr pDraft = NULL;
		SolidEdgeDraft::SheetsPtr pSheets = NULL;
		SolidEdgeDraft::SheetPtr pSheet = NULL;

		// Attempt to connect to a running instance of Solid Edge.
		hr = pApplication.GetActiveObject(L"SolidEdge.Application");
		
		if (hr == S_OK)
		{
			try
			{
				// Get a reference to the active draft document.
				pDraft = pApplication->ActiveDocument;
			}
			catch (_com_error &e)
			{
				::wprintf(L"No active draft document.\n");
			}

			// Get a reference to the Sheets collection.
			pSheets = pDraft->Sheets;

			// Add a new sheet.
			pSheet = pSheets->Add();

			// Make the new sheet the active sheet.
			pSheet->Activate();
		}
		else
		{
			::wprintf(L"Solid Edge is not running.\n");
		}
	}

	// Uninitialize COM.
	::CoUninitialize();

	return 0;
}

