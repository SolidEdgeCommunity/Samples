//#include "stdafx.h"
//#include "Commands.h"
//
//
//Commands::Commands(void)
//{
//}
//
//
//Commands::~Commands(void)
//{
//}
// Commands.cpp : implementation file
//

#include "stdafx.h"
#include "Commands.h"
#include "resource.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CCommands

CCommands::CCommands()
{
	m_pApplication = NULL;

	m_pSEAddIn = NULL;

	m_pApplicationEventsObj = NULL;

	m_pAddInEventsObj = NULL;
}

CCommands::~CCommands()
{
	DestroyAllCTIMBOViews();
}

HRESULT CCommands::SetApplicationObject(IDispatch* pApplicationDispatch, BOOL bEvents)
{
	ASSERT( pApplicationDispatch );

	HRESULT hr = NOERROR;

	m_pApplication = pApplicationDispatch;

	if( bEvents )
	{
		// Create Application event handlers
		XApplicationEventsObj::CreateInstance(&m_pApplicationEventsObj);
		if(m_pApplicationEventsObj)
		{
			m_pApplicationEventsObj->AddRef();
			hr = m_pApplicationEventsObj->Connect(m_pApplication);
			m_pApplicationEventsObj->m_pCommands = this;
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	return hr;
}

HRESULT CCommands::UnadviseFromEvents()
{
	HRESULT hr = NOERROR;

	if( NULL != m_pApplicationEventsObj )
	{
		hr = m_pApplicationEventsObj->Disconnect(m_pApplication);
		m_pApplicationEventsObj->Release();
		m_pApplicationEventsObj = NULL;
	}

	if( NULL != m_pAddInEventsObj )
	{
		hr = m_pAddInEventsObj->Disconnect(m_pSEAddIn);
		m_pAddInEventsObj->Release();
		m_pAddInEventsObj = NULL;
	}

	return hr;
}

HRESULT CCommands::SetAddInObject(AddIn* pSolidEdgeAddIn, BOOL bEvents)
{
	// This function assumes pSolidEdgeAddIn has already been AddRef'd
	//  for us, which containing class did in its QueryInterface call
	//  just before it called us.

	ASSERT( pSolidEdgeAddIn );

	HRESULT hr = NOERROR;

	m_pSEAddIn = pSolidEdgeAddIn;

	if( bEvents )
	{
		XAddInEventsObj::CreateInstance(&m_pAddInEventsObj);
		if( m_pAddInEventsObj )
		{
			m_pAddInEventsObj->AddRef();
			hr = m_pAddInEventsObj->Connect(m_pSEAddIn->GetAddInEvents());
			m_pAddInEventsObj->m_pCommands = this;
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	return hr;
}

/////////////////////////////////////////////////////////////////////////////
// Application event handler.

// Application events: This is the only sink that exists for application events.

// TODO: Fill out the implementation for those events you wish handle
//  Use m_pCommands->GetApplicationPtr() to access the Solid
//  Edge Application object
HRESULT CCommands::XApplicationEvents::raw_AfterActiveDocumentChange( IDispatch *theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterDocumentOpen(IDispatch* theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterDocumentPrint(IDispatch* theDocument, long hDC, double *ModelToDC, long *Rect )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterDocumentSave(IDispatch* theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterEnvironmentActivate( IDispatch * theEnvironment )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterNewDocumentOpen(IDispatch* theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterNewWindow(IDispatch* theWindow)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterWindowActivate(IDispatch* theWindow)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	WindowPtr pWindow = theWindow;
	ViewPtr pView = pWindow->View;
	m_pCommands->CreateCTIMBOView( pView );

	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeCommandRun( long lCommandID )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_AfterCommandRun( long lCommandID )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentClose(IDispatch* theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentPrint(IDispatch* theDocument, long hDC, double *ModelToDC, long *Rect )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeEnvironmentDeactivate( IDispatch * theEnvironment )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeWindowDeactivate(IDispatch* theWindow)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeQuit()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentSave(IDispatch* theDocument)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

/////////////////////////////////////////////////////////////////////////////
// AddIn event handler.

// AddIn events: This is the only sink that exists for add-in events.
// Note for the following functions: 
//       nCmdID is the identifier the addin passed to Solid Edge in either the SetAddInInfo
//       or AddCommand call. It is NOT the runtime command identifier returned by Solid Edge
//       from those calls and which is used as the parameter in the WM_COMMAND message
//       posted to the app by Windows whenever a button or menu item is selected.

HRESULT CCommands::XAddInEvents::raw_OnCommand( long nCmdID )
{
	return S_OK;
}

static DWORD ConvertCommandIDToHelpID( long nCmdID )
{
	// TODO: This assumes the help context identifier for your command
	//       is the same as the command identifier handed to Solid Edge.
	//       Modify this code if this is not true in your case.
	return( (DWORD) nCmdID );
}

HRESULT CCommands::XAddInEvents::raw_OnCommandHelp( long hFrameWnd, long uHelpCommand, long nCmdID )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CCommands::XAddInEvents::raw_OnCommandUpdateUI( long nCmdID, long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Update the user interface for the command whose id is nCmdID. 
	// Note: Taking no action results in the command being enabled.
	return S_OK;
}

// Creates a CTIMBOView given the dispatch of a Solid Edge view. Connects to the Solid Edge 
// view's event set if bEvents is TRUE. If ppTIMBOView is not NULL, the created view
// will be AddRef'd and returned in ppTIMBOView. If a CTIMBOview corresponding to 
// pViewDispatch already exists, a new one will NOT be created. If such is the case, the existing 
// view will be AddRef'd and returned if ppTIMBOView is not NULL.
HRESULT CCommands::CreateCTIMBOView(LPDISPATCH pViewDispatch, BOOL bEvents, CTIMBOViewObj** ppCTIMBOView)
{
	HRESULT hr = NOERROR;

	CTIMBOViewObj* pView = NULL;

	BOOL bAlreadyThere = m_pViews.Lookup(pViewDispatch, pView);

	if( FALSE == bAlreadyThere )
	{
		CTIMBOViewObj::CreateInstance(&pView);
		if( pView )
		{
			pView->AddRef();
			pView->SetCommandsObject(this);
			pView->SetViewObject(pViewDispatch,bEvents);

			m_pViews.SetAt(pViewDispatch, pView);
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	if( ppCTIMBOView )
	{
		*ppCTIMBOView = pView;
		(*ppCTIMBOView)->AddRef();
	}

	return hr;
}

HRESULT CCommands::DestroyCTIMBOView(LPDISPATCH pViewDispatch)
{
	HRESULT hr = NOERROR;

	CTIMBOViewObj* pView = NULL;

	BOOL bViewExists = m_pViews.Lookup(pViewDispatch, pView);

	if( bViewExists )
	{  
		m_pViews.RemoveKey(pViewDispatch);

		C_RELEASE(pView);
	}
	else
	{
		hr = E_INVALIDARG;
	}

	return hr;
}

HRESULT CCommands::DestroyAllCTIMBOViews( void )
{
	HRESULT hr = NOERROR;

	POSITION pos = m_pViews.GetStartPosition();

	while( NULL != pos )
	{
		LPDISPATCH pViewDispatch = NULL;
		CTIMBOViewObj *pView = NULL;

		m_pViews.GetNextAssoc( pos, pViewDispatch, pView );

		// This should be the final Release. The View will disconnect
		// from any event set and release the dispatch pointer it
		// has stored.
		C_RELEASE(pView)
	}

	m_pViews.RemoveAll();

	return hr;
}

// Given the dispatch of a Solid Edge view, lookup, AddRef and return the CTIMBOView
// if one exists in the collection. Returns NULL if one does not exist.
CTIMBOViewObj* CCommands::GetView(LPDISPATCH pViewDispatch)
{
	CTIMBOViewObj* pView = NULL;

	BOOL bViewExists = m_pViews.Lookup(pViewDispatch, pView);

	if( NULL != pView )
	{
		pView->AddRef();
	}

	return pView;
}
