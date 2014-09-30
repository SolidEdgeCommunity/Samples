// MySolidEdgeAddIn.cpp : Implementation of CMySolidEdgeAddIn

// NOTE: See ReadMe.txt for important details.

#include "stdafx.h"
#include "MySolidEdgeAddIn.h"

// CMySolidEdgeAddIn

STDMETHODIMP CMySolidEdgeAddIn::raw_OnConnection( IDispatch *pAppDispatch, SolidEdgeFramework::SeConnectMode ConnectMode, SolidEdgeFramework::AddIn* pAddIn )
{
    return S_OK;
}

STDMETHODIMP CMySolidEdgeAddIn::raw_OnConnectToEnvironment( BSTR EnvironmentCatid, LPDISPATCH pEnvironment,
                                                           VARIANT_BOOL bFirstTime )
{
    return S_OK;
}

STDMETHODIMP CMySolidEdgeAddIn::raw_OnDisconnection( SolidEdgeFramework::SeDisconnectMode DisconnectMode )
{
    return S_OK;
}