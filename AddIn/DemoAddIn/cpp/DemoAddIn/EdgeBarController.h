#pragma once

#include "DemoAddIn_i.h"
#include "EdgeBarDocument.h"
#include "SolidEdgeCommunity.h"

// {AC9C8327-445B-4C98-81ED-D96E60A0F7E2}
DEFINE_GUID(CLSID_EdgeBarController, 0xac9c8327, 0x445b, 0x4c98, 0x81, 0xed, 0xd9, 0x6e, 0x60, 0xa0, 0xf7, 0xe2);

// Define a mapping from a Solid Edge document dispatch pointer to my add-in document.
typedef CTypedPtrMap<CMapPtrToPtr, SolidEdgeDocument*, CComObject<CEdgeBarDocument>*> CMapDocumentToEdgeBarPage;

// Local COM class that handles adding\removing EdgeBar pages.
class ATL_NO_VTABLE CEdgeBarController :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CEdgeBarController, &CLSID_EdgeBarController>,
	public ISEAddInEdgeBarEvents, public SolidEdgeCommunity::CEventSink<ISEAddInEdgeBarEvents>
{
public:
	CEdgeBarController() :
		CEventSink<ISEAddInEdgeBarEvents>(this)
	{
	}

	~CEdgeBarController()
	{
	}

	BEGIN_COM_MAP(CEdgeBarController)
		COM_INTERFACE_ENTRY(ISEAddInEdgeBarEvents)
	END_COM_MAP()

	DECLARE_NOT_AGGREGATABLE(CEdgeBarController)

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}

	void FinalRelease()
	{
	}


	HRESULT OnConnection(ISEAddInEx* pAddInEx);
	HRESULT OnDisconnection();

protected:
	CMapDocumentToEdgeBarPage m_pMap;
	ISEAddInExPtr m_pAddInEx;

	HRESULT AddEdgeBarPage(SolidEdgeDocument* pDocument);
	HRESULT RemoveEdgeBarPage(SolidEdgeDocument* pDocument);
	HRESULT RemoveAllEdgeBarPages();

public:
	// ISEAddInEdgeBarEvents methods
	STDMETHOD(raw_AddPage) (LPDISPATCH theDocument);
	STDMETHOD(raw_RemovePage) (LPDISPATCH theDocument);
	STDMETHOD(raw_IsPageDisplayable) (LPDISPATCH theDocument, BSTR EnvironmentCatID, VARIANT_BOOL * vbIsPageDisplayable);
};

typedef CComObject<CEdgeBarController> CEdgeBarControllerObj;