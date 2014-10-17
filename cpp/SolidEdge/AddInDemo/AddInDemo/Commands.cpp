#include "stdafx.h"
//#include "Commands.h"
//
//ISEAddInExPtr CCommands::m_pSEAddIn = NULL;
//ApplicationPtr CCommands::m_pApplication = NULL;;
//
//AddInPtr GetAddInPtr() { return CCommands::GetAddIn(); }
//ApplicationPtr GetApplicationPtr() { return CCommands::GetApplicationPtr(); }
//
//CCommands::CCommands(void)
//{
//	m_pApplication = NULL;
//
//	m_pSEAddIn = NULL;
//  
//	m_pApplicationEventsObj = NULL;
//
//	m_pAddInEventsObj = NULL;
//	m_pAddInEventsExObj = NULL;
//
//	m_pAddInEdgeBarEventsObj = NULL;
//}
//
//
//CCommands::~CCommands(void)
//{
//	m_pApplication = NULL;
//
//	m_pSEAddIn = NULL;
//
//	//DestroyAllADDINDocuments();
//}
//
//HRESULT CCommands::SetApplicationObject( LPDISPATCH pApplicationDispatch,
//                                         BOOL bWithEvents )
//{
//	ASSERT( pApplicationDispatch );
//
//	HRESULT hr = NOERROR;
//
//	m_pApplication = pApplicationDispatch;
//
//	if( bWithEvents )
//	{
//		// Create Application event handlers
//		XApplicationEventsObj::CreateInstance(&m_pApplicationEventsObj);
//		if(m_pApplicationEventsObj)
//		{
//			m_pApplicationEventsObj->AddRef();
//			hr = m_pApplicationEventsObj->Connect(m_pApplication);
//			m_pApplicationEventsObj->m_pCommands = this;
//		}
//		else
//		{
//			hr = E_OUTOFMEMORY;
//		}
//	}
//
//	return hr;
//}
//
//HRESULT CCommands::UnadviseFromEvents()
//{
//	HRESULT hr = NOERROR;
//
//	if( NULL != m_pAddInEventsObj )
//	{
//		hr = m_pAddInEventsObj->Disconnect(m_pSEAddIn);
//		m_pAddInEventsObj->Release();
//		m_pAddInEventsObj = NULL;
//	}
//
//	if( NULL != m_pAddInEventsExObj )
//	{
//		hr = m_pAddInEventsExObj->Disconnect(m_pSEAddIn);
//		m_pAddInEventsExObj->Release();
//		m_pAddInEventsExObj = NULL;
//	}
//
//	if( NULL != m_pApplicationEventsObj )
//	{
//		hr = m_pApplicationEventsObj->Disconnect(m_pApplication);
//		m_pApplicationEventsObj->Release();
//		m_pApplicationEventsObj = NULL;
//	}
//	
//	if( NULL != m_pAddInEdgeBarEventsObj )
//	{
//		hr = m_pAddInEdgeBarEventsObj->Disconnect(m_pSEAddIn);
//		m_pAddInEdgeBarEventsObj->Release();
//		m_pAddInEdgeBarEventsObj = NULL;
//	}
//
//	return hr;
//}
//
//HRESULT CCommands::SetAddInObject( AddIn* pSolidEdgeAddIn,
//                                   BOOL bWithEvents )
//{
//	// This function assumes pSolidEdgeAddIn has already been AddRef'd
//	//  for us, which containing class did in its QueryInterface call
//	//  just before it called us.
//
//	ASSERT( pSolidEdgeAddIn );
//
//	HRESULT hr = NOERROR;
//
//	m_pSEAddIn = pSolidEdgeAddIn;
//
//	if( bWithEvents )
//	{
//		XAddInEventsExObj::CreateInstance(&m_pAddInEventsExObj);
//		if( m_pAddInEventsExObj )
//		{
//			m_pAddInEventsExObj->AddRef();
//			hr = m_pAddInEventsExObj->Connect(m_pSEAddIn->GetAddInEvents());
//			m_pAddInEventsExObj->m_pCommands = this;
//		}
//		else
//		{
//			hr = E_OUTOFMEMORY;
//		}
//
//		// I could be running a version of Edge that doesn't support the newer addin events. So I'll create the original
//		// events object just in case. But I have another reason to create both. The only code difference between the
//		// event sets is that the new one has the on-line help event for a command. So the "Ex" event object will simply
//		// forward the calls for all the other events to this object.
//		XAddInEventsObj::CreateInstance(&m_pAddInEventsObj);
//		if( m_pAddInEventsObj )
//		{
//			m_pAddInEventsObj->AddRef();
//			hr = m_pAddInEventsObj->Connect(m_pSEAddIn->GetAddInEvents());
//			m_pAddInEventsObj->m_pCommands = this;
//		}
//		else
//		{
//			hr = E_OUTOFMEMORY;
//		}
//
//		XAddInEdgeBarEventsObj::CreateInstance(&m_pAddInEdgeBarEventsObj);
//		if( m_pAddInEdgeBarEventsObj )
//		{
//			m_pAddInEdgeBarEventsObj->AddRef();
//			hr = m_pAddInEdgeBarEventsObj->Connect(m_pSEAddIn->GetAddInEdgeBarEvents());
//			if( SUCCEEDED(hr) )
//			{
//				m_pAddInEdgeBarEventsObj->m_pCommands = this;
//			}
//			else
//			{
//				// I assume this version of edge did not support the event set. Edge bar
//				// page will be added the old way.
//				m_pAddInEdgeBarEventsObj->Release();
//				m_pAddInEdgeBarEventsObj = NULL;
//			}
//		}
//		else
//		{
//			hr = E_OUTOFMEMORY;
//		}
//	}
//
//	return hr;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterActiveDocumentChange( LPDISPATCH theDocument)
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterDocumentOpen( LPDISPATCH theDocument )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterDocumentPrint( LPDISPATCH theDocument,
//                                                               long hDC,
//                                                               double *ModelToDC,
//                                                               long *Rect )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterDocumentSave( LPDISPATCH theDocument )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterEnvironmentActivate( LPDISPATCH theEnvironment )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterNewDocumentOpen( LPDISPATCH theDocument )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterNewWindow( LPDISPATCH theWindow )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterWindowActivate( LPDISPATCH theWindow )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeCommandRun( long lCommandID )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_AfterCommandRun( long lCommandID )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentClose( LPDISPATCH theDocument )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentPrint( LPDISPATCH theDocument,
//                                                                long hDC,
//                                                                double *ModelToDC,
//                                                                long *Rect )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeEnvironmentDeactivate( LPDISPATCH theEnvironment )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeWindowDeactivate( LPDISPATCH theWindow )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeQuit( void )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XApplicationEvents::raw_BeforeDocumentSave( LPDISPATCH theDocument )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//
///////////////////////////////////////////////////////////////////////////////
//// AddIn event handler.
//
//// AddIn events: This is the only sink that exists for add-in events.
//// Note for the following functions: 
////       nCmdID is the identifier the addin passed to Solid Edge in either the SetAddInInfo
////       or AddCommand call. It is NOT the runtime command identifier returned by Solid Edge
////       from those calls and which is used as the parameter in the WM_COMMAND message
////       posted to the app by Windows whenever a button or menu item is selected.
//
//SAFEARRAY *pRefKey1 = 0;
//
//void SetRefKey( SAFEARRAY * pRefKey ) { pRefKey1 = pRefKey; }
//
//static int s_RadioState = 0;
//static int s_CheckState = 0;
//
//HRESULT CCommands::XAddInEventsEx::raw_OnCommand( long nCmdID )
//{
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEventsEx::raw_OnCommandHelp( long hFrameWnd,
//													  long uHelpCommand,
//													  long nCmdID )
//{
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEventsEx::raw_OnCommandUpdateUI( long nCmdID,
//                                                          long *lCmdFlags,
//                                                          BSTR *MenuItemText,
//                                                          long *nIDBitmap )
//{
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEventsEx::raw_OnCommandOnLineHelp( long uHelpCommand,
//															long nCmdID,
//															BSTR* HelpURL )
//{
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEvents::raw_OnCommand( long nCmdID )
//{
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEvents::raw_OnCommandHelp( long hFrameWnd,
//												   long uHelpCommand,
//												   long nCmdID )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEvents::raw_OnCommandUpdateUI( long nCmdID,
//                                                        long *lCmdFlags,
//                                                        BSTR *MenuItemText,
//                                                        long *nIDBitmap )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEdgeBarEvents::raw_AddPage( LPDISPATCH pSEDocumentDispatch )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEdgeBarEvents::raw_RemovePage( LPDISPATCH pSEDocumentDispatch )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}
//
//HRESULT CCommands::XAddInEdgeBarEvents::raw_IsPageDisplayable ( IDispatch * pSEDocumentDispatch,
//																BSTR EnvironmentCatID,
//																VARIANT_BOOL * vbIsPageDisplayable )
//{
//	AFX_MANAGE_STATE(AfxGetStaticModuleState());
//	return S_OK;
//}