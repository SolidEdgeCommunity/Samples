// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently

#pragma once

#define _ATL_APARTMENT_THREADED 
#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // Exclude rarely-used stuff from Windows headers
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // some CString constructors will be explicit

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxole.h>         // MFC OLE classes
#include <afxodlgs.h>       // MFC OLE dialog classes
#include <afxdisp.h>        // MFC Automation classes
#endif // _AFX_NO_OLE_SUPPORT

#ifndef _AFX_NO_DB_SUPPORT
#include <afxdb.h>                      // MFC ODBC database classes
#endif // _AFX_NO_DB_SUPPORT

#ifndef _AFX_NO_DAO_SUPPORT
#include <afxdao.h>                     // MFC DAO database classes
#endif // _AFX_NO_DAO_SUPPORT

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC support for Internet Explorer 4 Common Controls
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>                     // MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
#include <atlsafe.h>

#pragma region Solid Edge specific

#include <initguid.h> // Necessary for secatids.h.
#include "secatids.h" // C:\Program Files\Solid Edge ST7\SDK\Advanced\include (Extract from DVD)

#pragma region import type libraries via filename

// SolidEdgeConstants
#import "constant.tlb"

//SolidEdgeFramework
#import "framewrk.tlb" exclude ("UINT_PTR", "LONG_PTR") rename ("GetOpenFileName", "SEGetOpenFileName")

// SolidEdgeGeometry
#import "geometry.tlb"

// SolidEdgeFrameworkSupport
#import "fwksupp.tlb"

// SolidEdgePart
#import "part.tlb"

// SolidEdgeAssembly
#import "assembly.tlb"

// SolidEdgeDraft
#import "draft.tlb"

// SEInstallDataLib
#import "SEInstallData.dll"

#pragma endregion

#pragma region import type libraries via libid
// NOTE: This will compile but does not play nice with intellisense (VS2012).
//
//// SolidEdgeConstants
//#import "libid:C467A6F5-27ED-11D2-BE30-080036B4D502" version("1.0")
//
////SolidEdgeFramework
//#import "libid:8A7EFA3A-F000-11D1-BDFC-080036B4D502" version("1.0") exclude ("UINT_PTR", "LONG_PTR") rename ("GetOpenFileName", "SEGetOpenFileName")
//
//// SolidEdgeGeometry
//#import "libid:3E2B3BE1-F0B9-11D1-BDFD-080036B4D502" version("1.0")
//
//// SolidEdgeFrameworkSupport
//#import "libid:943AC5C6-F4DB-11D1-BE00-080036B4D502" version("1.0")
//
//// SolidEdgePart
//#import "libid:8A7EFA42-F000-11D1-BDFC-080036B4D502" version("1.0")
//
//// SolidEdgeAssembly
//#import "libid:3E2B3BD4-F0B9-11D1-BDFD-080036B4D502" version("1.0")
//
//// SolidEdgeDraft
//#import "libid:3E2B3BDC-F0B9-11D1-BDFD-080036B4D502" version("1.0")
//
//// SEInstallDataLib
//#import "libid:42E04299-18A0-11D5-BBB2-00C04F79BEA5" version("1.0")
//
//using namespace SolidEdgeFramework;

#pragma endregion

using namespace SolidEdgeFramework;

#pragma endregion