#pragma once

template< typename TSink >
class CEventSink
{
public:
    IUnknownPtr m_pUnkCP;   // reference to the connection point pointer
    TSink*      m_pSink;    // pointer to the sink interface object
    DWORD       m_dwCookie;

    CEventSink( TSink* pSink ) :
		m_pSink(pSink),
		m_dwCookie(0)
    {
    }

    ~CEventSink()
    {
        if (m_dwCookie)
            Unadvise();
    }

    HRESULT Advise( IUnknown* pUnkCP )
    {
        HRESULT hr;
        m_pUnkCP = pUnkCP;
        hr = AtlAdvise( m_pUnkCP, m_pSink, __uuidof(TSink), &m_dwCookie );
        ATLASSERT(SUCCEEDED(hr));
        return hr;
    }

    HRESULT Unadvise(void)
    {
        HRESULT hr = S_OK;
        if (m_dwCookie)
        {
            hr = AtlUnadvise( m_pUnkCP, __uuidof(TSink), m_dwCookie );
            m_pUnkCP = NULL;
            ATLASSERT(SUCCEEDED(hr));
            m_dwCookie = 0;
        }
        return hr;
    }
};

//template <class T>
//class CRibbon
//{
//};
//
//class __declspec(uuid("{98A1341F-A428-4792-AF47-AE83D20C4981}")) CRibbonController;
//// Use the following typedef when creating an instance of CCommands.
//typedef CComObject<CRibbonController> CRibbonControllerObj;
//
//class CRibbonController
//    : public CComObjectRootEx<CComSingleThreadModel>
//    , public CComCoClass<CRibbonController, &__uuidof(CRibbonController)>
//    , public ISEAddInEvents,        public CEventSink<ISEAddInEvents>
//{
//public:
//    DECLARE_NO_REGISTRY()   // stop ATL automatically registering this class
//
//    // Only permit the existence of a single CRibbonController object
//    // (May not be needed?)
//    DECLARE_CLASSFACTORY_SINGLETON(CRibbonController)
//
//    BEGIN_COM_MAP(CRibbonController)
//        COM_INTERFACE_ENTRY(CRibbonController)
//        COM_INTERFACE_ENTRY(ISEAddInEvents)
//    END_COM_MAP()
//
//    CRibbonController(void)
//        : CEventSink<ISEAddInEvents>( this )
//    {
//        m_pAddIn = NULL;
//    }
//
//    ~CRibbonController(void)
//    {
//		m_pAddIn = NULL;
//    }
//
//	void Set(SolidEdgeFramework::AddInPtr pAddIn)
//	{
//		m_pAddIn = pAddIn;
//
//		if (m_pAddIn)
//		{
//			CEventSink<ISEAddInEvents>::Advise(m_pAddIn);
//		}
//		else
//		{
//			CEventSink<ISEAddInEvents>::Unadvise();
//		}
//	}
//
//	void UnadviseFromEvents()
//	{
//		CEventSink<ISEAddInEvents>::Unadvise();
//	}
//
//#pragma region ISEAddInEvents declaration
//    STDMETHOD(raw_OnCommand) ( /*[in]*/ long CommandID );
//
//    STDMETHOD(raw_OnCommandHelp) ( /*[in]*/ long hFrameWnd,
//                                   /*[in]*/ long HelpCommandID,
//                                   /*[in]*/ long CommandID );
//
//    STDMETHOD(raw_OnCommandUpdateUI) ( /*[in]*/ long CommandID,
//                                       /*[in,out]*/ long* pCommandFlags,
//                                       /*[out]*/ BSTR* pbstrMenuItemText,
//                                       /*[in,out]*/ long* pBitmapID );
//
//#pragma endregion
//
//private:
//	SolidEdgeFramework::AddInPtr m_pAddIn;
//};