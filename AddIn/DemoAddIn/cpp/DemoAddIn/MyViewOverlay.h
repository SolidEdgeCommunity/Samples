#pragma once

#include "DemoAddIn_i.h"
#include "igl.h"

// {CC4DA15C-E266-49E4-9F59-ECD69F29B1B4}
DEFINE_GUID(CLSID_MyViewOverlay, 0xcc4da15c, 0xe266, 0x49e4, 0x9f, 0x59, 0xec, 0xd6, 0x9f, 0x29, 0xb1, 0xb4);

// {6FB89D98-B356-42C6-9AF7-8BF3422B43D8}
DEFINE_GUID(CLSID_MyViewOverlayWindowEvents, 0x6fb89d98, 0xb356, 0x42c6, 0x9a, 0xf7, 0x8b, 0xf3, 0x42, 0x2b, 0x43, 0xd8);

// {D514D12B-98C7-42A4-B051-52572AD4F319}
DEFINE_GUID(CLSID_MyViewOverlayGLDisplayEvents, 0xd514d12b, 0x98c7, 0x42a4, 0xb0, 0x51, 0x52, 0x57, 0x2a, 0xd4, 0xf3, 0x19);

// {52C523A5-3F9A-4F0F-B3F7-BD4CAC825FFA}
DEFINE_GUID(CLSID_MyViewOverlayDCDisplayEvents, 0x52c523a5, 0x3f9a, 0x4f0f, 0xb3, 0xf7, 0xbd, 0x4c, 0xac, 0x82, 0x5f, 0xfa);

class ATL_NO_VTABLE CMyViewOverlay :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMyViewOverlay, &CLSID_MyViewOverlay>
{
public:
	CMyViewOverlay()
	{
		m_bBoundingBox = false;
		m_bOpenGLBoxes = false;
	}

	~CMyViewOverlay()
	{
	}

	BEGIN_COM_MAP(CMyViewOverlay)
	END_COM_MAP()

	DECLARE_NOT_AGGREGATABLE(CMyViewOverlay)

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}

	HRESULT SetView(Window* pWindow);
	HRESULT SetView(View* pView);
	HRESULT ShowBoundingBox();
	HRESULT HideBoundingBox();
	HRESULT ShowCubes();
	HRESULT HideCubes();
	HRESULT UnadviseFromEvents();

private:
	ViewPtr m_pView;
	bool m_bBoundingBox;
	bool m_bOpenGLBoxes;

	void DrawOpenGlBoundingBox(LPGL pGL);
	void DrawOpenGlBoxes(LPGL pGL);
	void DrawOpenGlBoxes(LPGL pGL, float fSize);

protected:
	template <class IEvents, const IID* piidEvents, const GUID* plibid, class XEvents, const CLSID* pClsidEvents>
	class XEventHandler :
		public CComDualImpl<IEvents, piidEvents, plibid>,
		public CComObjectRoot,
		public CComCoClass<XEvents, pClsidEvents>
	{
	public:
		XEventHandler()
		{
			m_pMyViewOverlay = NULL;
		}
		BEGIN_COM_MAP(XEvents)
			COM_INTERFACE_ENTRY_IID(*piidEvents, IEvents)
		END_COM_MAP()

		DECLARE_NOT_AGGREGATABLE(XEvents)

		HRESULT Connect(IUnknown* pUnk)
		{
			return AtlAdvise(pUnk, this, *piidEvents, &m_dwAdvise); 
		}
		HRESULT Disconnect(IUnknown* pUnk)
		{
			return AtlUnadvise(pUnk, *piidEvents, m_dwAdvise);
		}

		CMyViewOverlay* m_pMyViewOverlay;

	protected:
		DWORD m_dwAdvise;
	};

	// This object handles view events fired by Solid Edge
	class XViewEvents :
		public XEventHandler<ISEViewEvents, &__uuidof(ISEViewEvents), &LIBID_DemoAddInLib, XViewEvents, &CLSID_MyViewOverlayWindowEvents>
	{
	public:
		// ISEViewEvents methods
		STDMETHOD (raw_Changed)       ( void );
		STDMETHOD (raw_Destroyed)     ( void );
		STDMETHOD (raw_StyleChanged)  ( void );
	};
	typedef CComObject<XViewEvents> XViewEventsObj;
	XViewEventsObj* m_pViewEventsObj;

	// This object handles display events fired by Solid Edge. The 
	// source of the events is a view.
	class XhDCDisplayEvents :
		public XEventHandler<ISEhDCDisplayEvents, &__uuidof(ISEhDCDisplayEvents), &LIBID_DemoAddInLib, XhDCDisplayEvents, &CLSID_MyViewOverlayDCDisplayEvents>
	{
	public:
		// ISEhDCDisplayEvents methods
		STDMETHOD (raw_BeginDisplay)        ( void );
		STDMETHOD (raw_EndDisplay)          ( void );
		STDMETHOD (raw_BeginhDCMainDisplay) ( long hDC, double *ModelToDC, long *Rect );
		STDMETHOD (raw_EndhDCMainDisplay)   ( long hDC, double *ModelToDC, long *Rect );
	};

	typedef CComObject<XhDCDisplayEvents> XhDCDisplayEventsObj;
	XhDCDisplayEventsObj* m_phDCDisplayEventsObj;


	// This object handles gl display events fired by Solid Edge. The 
	// source of the events is a view.
	class XGLDisplayEvents :
		public XEventHandler<ISEIGLDisplayEvents, &__uuidof(ISEIGLDisplayEvents), &LIBID_DemoAddInLib, XGLDisplayEvents, &CLSID_MyViewOverlayGLDisplayEvents>
	{
	public:
		// ISEIGLDisplayEvents methods
		STDMETHOD (raw_BeginDisplay)        ( void );
		STDMETHOD (raw_EndDisplay)          ( void );
		STDMETHOD (raw_BeginIGLMainDisplay) ( LPUNKNOWN pGL );
		STDMETHOD (raw_EndIGLMainDisplay)   ( LPUNKNOWN pGL );
	};

	typedef CComObject<XGLDisplayEvents> XGLDisplayEventsObj;
	XGLDisplayEventsObj* m_pGLDisplayEventsObj;
public:

	friend class XViewEvents;
	friend class XhDCDisplayEvents;
	friend class XGLDisplayEvents;

};

typedef CComObject<CMyViewOverlay> CMyViewOverlayObj;