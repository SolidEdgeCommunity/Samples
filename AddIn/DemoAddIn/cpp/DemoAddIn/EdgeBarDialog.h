#pragma once
#include "afxcmn.h"


// CEdgeBarDialog dialog

class CEdgeBarDialog : public CDialogEx
{
	DECLARE_DYNAMIC(CEdgeBarDialog)

public:
	CEdgeBarDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~CEdgeBarDialog();

	// Dialog Data
	enum { IDD = IDD_EDGEBARDIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	CListCtrl m_listView;
};
