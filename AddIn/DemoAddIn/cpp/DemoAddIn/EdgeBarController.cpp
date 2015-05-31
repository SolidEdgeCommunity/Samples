#include "stdafx.h"
#include "EdgeBarController.h"

HRESULT CEdgeBarController::OnConnection(ISEAddInEx* pAddInEx)
{
	HRESULT hr = S_OK;
	ApplicationPtr pApplication = NULL;
	SolidEdgeDocumentPtr pDocument = NULL;

	try
	{
		// Assign local variable.
		m_pAddInEx = pAddInEx;

		if (m_pAddInEx != NULL)
		{
			CEventSink<ISEAddInEdgeBarEvents>::Advise(m_pAddInEx->AddInEdgeBarEvents);

			pApplication = m_pAddInEx->Application;

			// ActiveDocument property will throw an exception if no document is open.
			try
			{
				pDocument = pApplication->ActiveDocument;

				if (pDocument != NULL)
				{
					AddEdgeBarPage(pDocument);
				}
			}
			catch (_com_error& e)
			{
			}
		}
	}
	catch (_com_error& e)
	{
		hr = e.Error();
	}

	pDocument = NULL;
	pApplication = NULL;

	return hr;
}

HRESULT CEdgeBarController::OnDisconnection()
{
	HRESULT hr = S_OK;

	try
	{
		CEventSink<ISEAddInEdgeBarEvents>::Unadvise();
		RemoveAllEdgeBarPages();
		m_pAddInEx = NULL;
	}
	catch (_com_error& e)
	{
		hr = e.Error();
	}

	return hr;
}

HRESULT CEdgeBarController::AddEdgeBarPage(SolidEdgeDocument* pDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	HRESULT hr = S_OK;
	CEdgeBarDocumentObj* pEdgeBarDocument = NULL;
	ISolidEdgeBarExPtr pEdgeBarEx = NULL;

	try
	{
		// Check our map to see if we've already created an EdgeBar page for the document.
		BOOL bFound = m_pMap.Lookup(pDocument, pEdgeBarDocument);

		if (bFound == FALSE)
		{
			// Get a pointer to the ISolidEdgeBarEx interface.
			pEdgeBarEx = m_pAddInEx;

			// Create an instance of local COM object CEdgeBarDocument.
			CEdgeBarDocumentObj::CreateInstance(&pEdgeBarDocument);

			// Manually AddRef() it.
			pEdgeBarDocument->AddRef();

			// EdgeBar page creation logic is contained in the CEdgeBarDocument.
			hr = pEdgeBarDocument->CreateEdgeBarPage(pEdgeBarEx, pDocument);

			if (SUCCEEDED(hr))
			{
				// Add it to our map so we can keep track of which documents we've added EdgeBar pages to.
				m_pMap.SetAt(pDocument, pEdgeBarDocument);
			}
		}
	}
	catch (_com_error& e)
	{
		hr = e.Error();
	}

	pEdgeBarEx = NULL;

	return hr;
}

HRESULT CEdgeBarController::RemoveEdgeBarPage(SolidEdgeDocument* pDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	HRESULT hr = S_OK;
	CEdgeBarDocumentObj* pEdgeBarDocument = NULL;
	ISolidEdgeBarExPtr pEdgeBarEx = NULL;

	try
	{
		// Check our map to see if we've created an EdgeBar page for the document.
		BOOL bFound = m_pMap.Lookup(pDocument, pEdgeBarDocument);

		if ((bFound == TRUE) && (pEdgeBarDocument != NULL))
		{
			// Remove the entry from the map.
			m_pMap.RemoveKey(pDocument);

			// Get a pointer to the ISolidEdgeBarEx interface.
			pEdgeBarEx = m_pAddInEx;

			// Give the CEdgeBarDocument the opportunity to clean up.
			hr = pEdgeBarDocument->DeleteEdgeBarPage(pEdgeBarEx, pDocument);

			// This is the final release.
			pEdgeBarDocument->Release();
		}
	}
	catch (_com_error& e)
	{
		hr = e.Error();
	}

	pEdgeBarEx = NULL;

	return hr;
}

HRESULT CEdgeBarController::RemoveAllEdgeBarPages()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	HRESULT hr = S_OK;
	CEdgeBarDocumentObj* pEdgeBarDocument = NULL;
	ISolidEdgeBarExPtr pEdgeBarEx = NULL;
	SolidEdgeDocument* pDocument = NULL;

	try
	{
		// Get a pointer to the ISolidEdgeBarEx interface.
		pEdgeBarEx = m_pAddInEx;

		POSITION position = m_pMap.GetStartPosition();

		while (position)
		{
			m_pMap.GetNextAssoc(position, pDocument, pEdgeBarDocument);

			if (pEdgeBarDocument != NULL)
			{
				// Give the CEdgeBarDocument the opportunity to clean up.
				hr = pEdgeBarDocument->DeleteEdgeBarPage(pEdgeBarEx, pDocument);

				// This is the final release.
				pEdgeBarDocument->Release();
			}
		}

		m_pMap.RemoveAll();
	}
	catch (_com_error& e)
	{
		hr = e.Error();
	}

	pEdgeBarEx = NULL;
	return S_OK;
}

#pragma region ISEAddInEdgeBarEvents implementation

HRESULT CEdgeBarController::raw_AddPage(LPDISPATCH theDocument)
{
	HRESULT hr = S_OK;
	SolidEdgeDocumentPtr pDocument = theDocument;

	if (pDocument != NULL)
	{
		hr = AddEdgeBarPage(pDocument);
	}
	else
	{
		hr = E_FAIL;
	}

	pDocument = NULL;
	return hr;
}

HRESULT CEdgeBarController::raw_RemovePage(LPDISPATCH theDocument)
{
	HRESULT hr = S_OK;
	SolidEdgeDocumentPtr pDocument = theDocument;

	if (pDocument != NULL)
	{
		hr = RemoveEdgeBarPage(pDocument);
	}
	else
	{
		hr = E_FAIL;
	}

	pDocument = NULL;
	return hr;
}

HRESULT CEdgeBarController::raw_IsPageDisplayable(LPDISPATCH theDocument, BSTR EnvironmentCatID, VARIANT_BOOL * vbIsPageDisplayable)
{
	*vbIsPageDisplayable = VARIANT_TRUE;
	return S_OK;
}

#pragma endregion