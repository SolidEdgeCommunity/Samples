// MySolidEdgeAddIn.h : Declaration of the CMySolidEdgeAddIn

// NOTE: See ReadMe.txt for important details.

#pragma once
#include "resource.h"       // main symbols
#include "MinimalAddIn_i.h"


#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CMySolidEdgeAddIn

class ATL_NO_VTABLE CMySolidEdgeAddIn :
    public SolidEdgeFramework::ISolidEdgeAddIn,
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CMySolidEdgeAddIn, &CLSID_MySolidEdgeAddIn>
    //public IDispatchImpl<IMySolidEdgeAddIn, &IID_IMySolidEdgeAddIn, &LIBID_MinimalAddInLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
    CMySolidEdgeAddIn()
    {
    }

    DECLARE_REGISTRY_RESOURCEID(IDR_MYSOLIDEDGEADDIN)
    DECLARE_NOT_AGGREGATABLE(CMySolidEdgeAddIn)

    BEGIN_COM_MAP(CMySolidEdgeAddIn)
        COM_INTERFACE_ENTRY(SolidEdgeFramework::ISolidEdgeAddIn)
        //COM_INTERFACE_ENTRY(IDispatch)
    END_COM_MAP()

    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct()
    {
        return S_OK;
    }

    void FinalRelease()
    {
    }

public:
    // Occurs when Solid Edge connects to an add-in.
    STDMETHOD(raw_OnConnection)( THIS_ IDispatch* pAppDispatch, SolidEdgeFramework::SeConnectMode ConnectMode, SolidEdgeFramework::AddIn* pUnkAddIn );
    // Occurs when a Solid Edge environment and an add-in connect.
    STDMETHOD(raw_OnConnectToEnvironment)( BSTR EnvironmentCatid, LPDISPATCH pEnvironment, VARIANT_BOOL bFirstTime );
    // Occurs when Solid Edge and an add-in are disconnected.
    STDMETHOD(raw_OnDisconnection)( THIS_ SolidEdgeFramework::SeDisconnectMode DisconnectMode );


};

OBJECT_ENTRY_AUTO(__uuidof(MySolidEdgeAddIn), CMySolidEdgeAddIn)
