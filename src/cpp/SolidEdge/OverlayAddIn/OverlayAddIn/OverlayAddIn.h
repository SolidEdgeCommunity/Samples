// OverlayAddIn.h : main header file for the OverlayAddIn DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include "OverlayAddIn_h.h"


// COverlayAddInApp
// See OverlayAddIn.cpp for the implementation of this class
//

class COverlayAddInApp : public CWinApp
{
public:
	COverlayAddInApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
