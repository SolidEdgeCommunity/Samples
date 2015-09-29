// ReportTubes.cpp : Defines the entry point for the console application.
//

// C:\Program Files\Solid Edge ST7\Training\Try It\zone_try_it.asm was used to write this sample.

#include "stdafx.h"

void PrintDoubleArray(LPCTSTR prefix, LPSAFEARRAY parray);

int _tmain(int argc, _TCHAR* argv[])
{
	HRESULT hr = S_OK;

	// Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SolidEdgeFramework::ApplicationPtr pApplication = NULL;
		SolidEdgeAssembly::AssemblyDocumentPtr pAssemblyDocument = NULL;
		SolidEdgeAssembly::OccurrencesPtr pOccurrences = NULL;
		SolidEdgeAssembly::OccurrencePtr pOccurrence = NULL;
		SolidEdgeAssembly::TubePtr pTube = NULL;

		// Attempt to connect to a running instance of Solid Edge.
		hr = pApplication.GetActiveObject(L"SolidEdge.Application");

		if (FAILED(hr))
		{
			IfFailGo(pApplication.CreateInstance(L"SolidEdge.Application"));
			pApplication->Visible = VARIANT_TRUE;
		}

		try
		{
			// Get a reference to the active document.
			pAssemblyDocument = pApplication->ActiveDocument;
		}
		catch (_com_error& e)
		{
		}

		if (pAssemblyDocument != NULL)
		{
			// Get a reference to the Occurrences collection.
			pOccurrences = pAssemblyDocument->Occurrences;

			for (LONG i = 1; i <= pOccurrences->Count; i++)
			{
				pOccurrence = pOccurrences->Item(i);

				// Check to see if the occurrence is a tube.
				if (pOccurrence->IsTube() == VARIANT_TRUE)
				{
					wprintf(L"Occurrences[%d] is a tube.\n", pOccurrence->Index);
					wprintf(L"PartFileName: %s\n", pOccurrence->PartFileName.GetBSTR());

					pTube = pOccurrence->GetTube();

					_variant_t vtCutLength = 0.0;
					_variant_t vtNumOfBends = 0;
					_variant_t vtFeedLength;
					_variant_t vtRotationAngle;
					_variant_t vtBendRadius;
					_variant_t vtReverseBendOrder = 0;
					_variant_t vtBendAngle;

					vtFeedLength.vt = VT_ARRAY | VT_R8; // double array.
					vtRotationAngle.vt = VT_ARRAY | VT_R8; // double array.
					vtBendRadius.vt = VT_ARRAY | VT_R8; // double array.
					vtBendAngle.vt = VT_ARRAY | VT_R8; // double array.

					pTube->BendTable(&vtCutLength, &vtNumOfBends, &vtFeedLength, &vtRotationAngle, &vtBendRadius, &vtReverseBendOrder, vtMissing, &vtBendAngle);

					wprintf(L"BendTable information:\n");
					wprintf(L"CutLength: %f\n", vtCutLength.dblVal);
					wprintf(L"NumOfBends: %d\n", vtNumOfBends.lVal);
					PrintDoubleArray(L"FeedLength", vtFeedLength.parray);
					PrintDoubleArray(L"RotationAngle", vtRotationAngle.parray);
					PrintDoubleArray(L"BendRadius", vtBendRadius.parray);
					PrintDoubleArray(L"BendAngle", vtBendAngle.parray);
					wprintf(L"\n");
				}
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

void PrintDoubleArray(LPCTSTR prefix, LPSAFEARRAY parray)
{
	long lStartBound = 0;
	long lEndBound = 0;

	SafeArrayGetLBound(parray, 1, &lStartBound);
	SafeArrayGetUBound(parray, 1, &lEndBound);

	CString strFeedLength;

	for (long iIndex = lStartBound; iIndex <= lEndBound; iIndex++)
	{
		double feedLength = 0;
		SafeArrayGetElement(parray, &iIndex, &feedLength);
		strFeedLength.AppendFormat(L"%f", feedLength);

		if (iIndex < lEndBound)
		{
			strFeedLength.Append(L", ");
		}
	}

	wprintf(L"%s: %s\n", prefix, strFeedLength);
}