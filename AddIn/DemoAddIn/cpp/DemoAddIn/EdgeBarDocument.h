#pragma once

#include "resource.h"
#include "EdgeBarDialog.h"

// {703AADD7-4B5A-41E1-AD20-122FB3588C50}
DEFINE_GUID(CLSID_EdgeBarDocument, 0x703aadd7, 0x4b5a, 0x41e1, 0xad, 0x20, 0x12, 0x2f, 0xb3, 0x58, 0x8c, 0x50);

class CEdgeBarDocument :
	public CComObjectRoot,
	public CComCoClass<CEdgeBarDocument, &CLSID_EdgeBarDocument>
{
protected:
	SolidEdgeDocumentPtr m_pDocument;
	CWnd m_hWndEdgeBarPage;
	CEdgeBarDialog* m_pDialog;

public:
	CEdgeBarDocument();
	~CEdgeBarDocument();

	HRESULT CreateEdgeBarPage(ISolidEdgeBarEx* pEdgeBarEx, SolidEdgeDocument* pDocument);
	HRESULT DeleteEdgeBarPage(ISolidEdgeBarEx* pEdgeBarEx, SolidEdgeDocument* pDocument);

	BEGIN_COM_MAP(CEdgeBarDocument)
	END_COM_MAP()
	DECLARE_NOT_AGGREGATABLE(CEdgeBarDocument)
};

typedef CComObject<CEdgeBarDocument> CEdgeBarDocumentObj;