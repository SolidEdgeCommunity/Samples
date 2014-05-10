// MySolidEdgeAddIn.cpp : Implementation of CMySolidEdgeAddIn

#include "stdafx.h"
#include "MySolidEdgeAddIn.h"


// CMySolidEdgeAddIn

STDMETHODIMP CMySolidEdgeAddIn::raw_OnConnection( IDispatch *pAppDispatch, SolidEdgeFramework::SeConnectMode ConnectMode, SolidEdgeFramework::AddIn* pAddIn )
{
	AFX_MANAGE_STATE( AfxGetStaticModuleState() );

	HRESULT hr = S_OK;

	// Create commands
	CCommandsObj::CreateInstance(&m_pCommands);
	m_pCommands->AddRef();

	// The QueryInterface above AddRef'd the Application object.  It will
	//  be Release'd in CCommands' destructor.
	hr = m_pCommands->SetApplicationObject(pAppDispatch);

	// The QueryInterface above AddRef'd the AddIn object. It too will be released
	// in CCommands' destructor.
	hr = m_pCommands->SetAddInObject(pAddIn);

	if( SUCCEEDED(hr) )
	{
		//TODO: Set your gui version.

		// Set the gui version. If the version changed since the last time 
		// addin was connected, SE will purge any gui related to 
		// this it and force bFirstTime to VARIANT_TRUE when calling 
		// OnConnectToEnvironment.

		m_pCommands->GetAddIn()->put_GuiVersion( 1 );
	}

	return( hr );
}

STDMETHODIMP CMySolidEdgeAddIn::raw_OnConnectToEnvironment( BSTR EnvironmentCatid, LPDISPATCH pEnvironment,
                                                           VARIANT_BOOL bFirstTime )
{
    return S_OK;
}

STDMETHODIMP CMySolidEdgeAddIn::raw_OnDisconnection( SolidEdgeFramework::SeDisconnectMode DisconnectMode )
{
	AFX_MANAGE_STATE( AfxGetStaticModuleState() );

	if( m_pCommands )
	{
		m_pCommands->UnadviseFromEvents();
		m_pCommands->Release();
		m_pCommands = NULL;
	}

    return S_OK;
}