// BOM.cpp : Defines the entry point for the console application.
//

// This project is configured to include path "C:\Program Files\Solid Edge ST6\Program".
// If you are getting build errors, you may have to update the path to your Solid Edge installation.
// Project -> Properties -> Configuration Properties -> VC++ Directories -> Library Directories
// Specifically, the #import directives in stdafx.h have to be able to resolve the .tlb(s).

#include "stdafx.h"

VOID ProcessAssembly(LONG level, SolidEdgeAssembly::AssemblyDocumentPtr pAssemblyDocument);
VOID PrintBomItem(LONG level, SolidEdgeFramework::SolidEdgeDocumentPtr pDocument);

int _tmain(int argc, _TCHAR* argv[])
{
    HRESULT hr = S_OK;
    SolidEdgeFramework::ApplicationPtr pApplication = NULL;
    SolidEdgeFramework::DocumentsPtr pDocuments = NULL;
    SolidEdgeAssembly::AssemblyDocumentPtr pAssemblyDocument = NULL;

    // Initialize COM.
    ::CoInitialize(NULL);

    // Attempt to connect to a running instance of Solid Edge.
    hr = pApplication.GetActiveObject(L"SolidEdge.Application");

    if (hr == MK_E_UNAVAILABLE)
    {
        ::wprintf(L"Solid Edge is not running.\n");
        return 1;
    }

    // Get a reference to the Documents collection.
    pDocuments = pApplication->Documents;

    // Make sure a document is open and that it's an AssemblyDocument.
    if ((pDocuments->Count == 1) && (pApplication->ActiveDocumentType == SolidEdgeFramework::igAssemblyDocument))
    {
        // Get a reference to the AssemblyDocument.
        pAssemblyDocument = pApplication->ActiveDocument;

        // Start walking the assembly tree.
        ProcessAssembly(0, pAssemblyDocument);
    }

    pAssemblyDocument = NULL;
    pDocuments = NULL;
    pApplication = NULL;

    // Uninitialize COM.
    ::CoUninitialize();

    return 0;
}

VOID ProcessAssembly(LONG level, SolidEdgeAssembly::AssemblyDocumentPtr pAssemblyDocument)
{
    // Increment level (depth).
    level++;

    SolidEdgeAssembly::OccurrencesPtr pOccurrences = pAssemblyDocument->Occurrences;
    SolidEdgeAssembly::OccurrencePtr pOccurrence = NULL;
    SolidEdgeFramework::SolidEdgeDocumentPtr pSolidEdgeDocument = NULL;

    // Loop through the Occurrences.
    for (LONG i = 1; i <= pOccurrences->Count; i++)
    {
        pOccurrence = pOccurrences->Item(i);
        
        // Filter out certain occurrences.
        if (pOccurrence->IncludeInBom == VARIANT_FALSE) { continue; }
        if (pOccurrence->IsPatternItem == VARIANT_TRUE) { continue; }
        if (pOccurrence->OccurrenceDocument == NULL) { continue; }

        // Get a reference to the SolidEdgeDocument.
        pSolidEdgeDocument = pOccurrence->OccurrenceDocument;

        // Print the BomItem.
        PrintBomItem(level, pSolidEdgeDocument);

        if (pOccurrence->Subassembly == VARIANT_TRUE)
        {
            // Sub Assembly. Recurisve call to drill down.
            ProcessAssembly(level, pSolidEdgeDocument);
        }
    }

    pSolidEdgeDocument = NULL;
    pOccurrence = NULL;
    pOccurrences = NULL;
}

VOID PrintBomItem(LONG level, SolidEdgeFramework::SolidEdgeDocumentPtr pSolidEdgeDocument)
{
    SolidEdgeFramework::SummaryInfoPtr pSummaryInfo = pSolidEdgeDocument->SummaryInfo;

    ::wprintf(L"%li\t", level); // Level
    ::wprintf(L"%s\t", pSummaryInfo->DocumentNumber.GetBSTR()); // Document Number
    ::wprintf(L"%s\t", pSummaryInfo->RevisionNumber.GetBSTR()); // Revision
    ::wprintf(L"%s\t", pSummaryInfo->Title.GetBSTR()); // Title
    ::wprintf(L"%s\t", pSolidEdgeDocument->FullName.GetBSTR()); // FileName
    ::wprintf(L"\n"); // Newline

    pSummaryInfo = NULL;
}