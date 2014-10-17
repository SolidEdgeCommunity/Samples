// AddInDemo.h : main header file for the AddInDemo DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include "AddInDemo_i.h"


// CAddInDemoApp
// See AddInDemo.cpp for the implementation of this class
//

class CAddInDemoApp : public CWinApp
{
public:
	CAddInDemoApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
