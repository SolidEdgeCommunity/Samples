// DemoAddIn.h : main header file for the DemoAddIn DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include "DemoAddIn_i.h"


// CDemoAddInApp
// See DemoAddIn.cpp for the implementation of this class
//

class CDemoAddInApp : public CWinApp
{
public:
	CDemoAddInApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
