#include "stdafx.h"
#include "EdgeBarDocument.h"
#include "resource.h"

CEdgeBarDocument::CEdgeBarDocument()
{
	m_pDocument = NULL;
}


CEdgeBarDocument::~CEdgeBarDocument()
{
	m_pDocument = NULL;
}

HRESULT CEdgeBarDocument::CreateEdgeBarPage(ISolidEdgeBarEx* pEdgeBarEx, SolidEdgeDocument* pDocument)
{
	m_pDocument = pDocument;

	// Build path to .dll that contains the resources.
	HINSTANCE hInstance = AfxGetResourceHandle();
	TCHAR ResourceFilename[MAX_PATH];
	GetModuleFileName(hInstance, ResourceFilename, sizeof(ResourceFilename));

	CString strTooltip;
	strTooltip.LoadString(IDS_EDGEBAR_CAPTION);

	_bstr_t bstrTooltip = strTooltip;

	// Create a new EdgeBar page.
	HWND hWndEdgeBarPage = (HWND)pEdgeBarEx->AddPageEx(pDocument, ResourceFilename, IDB_EDGEBAR, bstrTooltip, 2);

	if (hWndEdgeBarPage)
	{
		// Attach a CWnd local variable to the new EdgeBar page.
		BOOL bAttached = m_hWndEdgeBarPage.Attach(hWndEdgeBarPage);

		if (bAttached)
		{
			// Create a new dialog and make it a child of the new EdgeBar page.
			m_pDialog = new CEdgeBarDialog();
			m_pDialog->Create(IDD_EDGEBARDIALOG, &m_hWndEdgeBarPage);
			m_pDialog->ShowWindow(SW_SHOW);
		}
	}

	return S_OK;
}

HRESULT CEdgeBarDocument::DeleteEdgeBarPage(ISolidEdgeBarEx* pEdgeBarEx, SolidEdgeDocument* pDocument)
{
	HRESULT hr = S_OK;

	// Detach the CWnd and obtain the HWND.
	HWND hWndEdgeBarPage = m_hWndEdgeBarPage.Detach();

	// Remove the EdgeBar page.
	hr = pEdgeBarEx->RemovePage(pDocument, (LONG)hWndEdgeBarPage, 0);

	// Cleanup dialog.
	delete m_pDialog;
	m_pDialog = NULL;

	return S_OK;
}