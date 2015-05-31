// MyAddIn.h : Declaration of the CMyAddIn

#pragma once
#include "resource.h"       // main symbols
#include "DemoAddIn_i.h"
#include "SolidEdgeCommunity.h"
#include "EdgeBarController.h"
#include "MyViewOverlay.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CMyAddIn

// AddIn GUID Must be unique!
// {3CE1FCC9-6345-4367-94D4-31C701B06AEC}
DEFINE_GUID(CLSID_MyAddIn, 0x3ce1fcc9, 0x6345, 0x4367, 0x94, 0xd4, 0x31, 0xc7, 0x1, 0xb0, 0x6a, 0xec);

class ATL_NO_VTABLE CMyAddIn :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMyAddIn, &CLSID_MyAddIn>,
	public ISolidEdgeAddIn,
	//public ISEAddInEvents, public SolidEdgeCommunity::CEventSink<ISEAddInEvents>,
	public ISEAddInEventsEx, public SolidEdgeCommunity::CEventSink<ISEAddInEventsEx>, // Added in ST6
	public ISEApplicationEvents, public SolidEdgeCommunity::CEventSink<ISEApplicationEvents>
	//public ISEApplicationWindowEvents, public CEventSink<ISEApplicationWindowEvents>,
	//public ISEFeatureLibraryEvents, public CEventSink<ISEFeatureLibraryEvents>,
	//public ISEFileUIEvents, public CEventSink<ISEFileUIEvents>,
	//public ISENewFileUIEvents, public CEventSink<ISENewFileUIEvents>,
	//public ISEECEvents, public CEventSink<ISEECEvents>,
	//public ISEShortCutMenuEvents, public CEventSink<ISEShortCutMenuEvents>
{
public:
	CMyAddIn() :
		//CEventSink<ISEAddInEvents>(this),
		CEventSink<ISEAddInEventsEx>(this),
		CEventSink<ISEApplicationEvents>(this)
		//CEventSink<ISEApplicationWindowEvents>(this),
		//CEventSink<ISEFeatureLibraryEvents>(this),
		//CEventSink<ISEFileUIEvents>(this),
		//CEventSink<ISENewFileUIEvents>(this),
		//CEventSink<ISEECEvents>(this),
		//CEventSink<ISEShortCutMenuEvents>(this)
	{
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_MYADDIN)

	BEGIN_COM_MAP(CMyAddIn)
		COM_INTERFACE_ENTRY(ISolidEdgeAddIn)
		//COM_INTERFACE_ENTRY(ISEAddInEvents)
		COM_INTERFACE_ENTRY(ISEAddInEventsEx)
		COM_INTERFACE_ENTRY(ISEApplicationEvents)
		//COM_INTERFACE_ENTRY(ISEApplicationWindowEvents)
		//COM_INTERFACE_ENTRY(ISEFeatureLibraryEvents)
		//COM_INTERFACE_ENTRY(ISEFileUIEvents)
		//COM_INTERFACE_ENTRY(ISENewFileUIEvents)
		//COM_INTERFACE_ENTRY(ISEECEvents)
		//COM_INTERFACE_ENTRY(ISEShortCutMenuEvents)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		AFX_MANAGE_STATE(AfxGetStaticModuleState());

#pragma region ResourceFilename

		// Build path to .dll that contains the resources.
		HINSTANCE hInstance = AfxGetResourceHandle();
		TCHAR ResourceFilename[MAX_PATH];
		GetModuleFileName(hInstance, ResourceFilename, sizeof(ResourceFilename));

		m_strResourceFilename = ResourceFilename;

#pragma endregion

#pragma region CommandPrefix

		// Build the command prefix for uniqueness.
		const int GUID_STRING_LENGTH = 40;
		OLECHAR szGuid[GUID_STRING_LENGTH] = { 0 };
		::StringFromGUID2(CLSID_MyAddIn, szGuid, GUID_STRING_LENGTH);
		CString csGuid(szGuid);
		m_strCommandPrefix = csGuid.Mid(1, 8);

#pragma endregion

		m_bBoundingBox = false;
		m_bOpenGLBoxes = false;

		return S_OK;
	}

	void FinalRelease()
	{
	}

private:
	ApplicationPtr m_pApplication;
	AddInPtr m_pAddIn;
	ISEAddInExPtr m_pSEAddInEx;
	CString m_strResourceFilename;
	CString m_strCommandPrefix;
	bool m_bBoundingBox;
	bool m_bOpenGLBoxes;

protected:

public:

	static void OnDllRegisterServer();
	static void OnDllUnregisterServer();

#pragma region ISolidEdgeAddIn

	STDMETHOD (raw_OnConnection) (LPDISPATCH Application, SeConnectMode ConnectMode, AddIn * AddInInstance);
	STDMETHOD (raw_OnConnectToEnvironment) (BSTR EnvCatID, LPDISPATCH pEnvironmentDispatch, VARIANT_BOOL bFirstTime);
	STDMETHOD (raw_OnDisconnection) (SeDisconnectMode DisconnectMode);

#pragma endregion

#pragma region ISEAddInEvents

	//STDMETHOD (raw_OnCommand) (long nCmdID);
	//STDMETHOD (raw_OnCommandHelp) (long hFrameWnd, long uHelpCommand, long nCmdID);
	//STDMETHOD (raw_OnCommandUpdateUI) (long nCmdID, long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap);

#pragma endregion

#pragma region ISEAddInEventsEx

	STDMETHOD (raw_OnCommand) (long nCmdID );
	STDMETHOD (raw_OnCommandHelp) (long hFrameWnd, long uHelpCommand, long nCmdID);
	STDMETHOD (raw_OnCommandUpdateUI) (long nCmdID,  long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap);
	STDMETHOD (raw_OnCommandOnLineHelp) (long uHelpCommand, long nCmdID, BSTR* HelpURL);

#pragma endregion

#pragma region ISEApplicationEvents

	STDMETHOD (raw_AfterActiveDocumentChange) (LPDISPATCH theDocument);
	STDMETHOD (raw_AfterCommandRun) (long theCommandID);
	STDMETHOD (raw_AfterDocumentOpen) (LPDISPATCH theDocument);
	STDMETHOD (raw_AfterDocumentPrint) (LPDISPATCH theDocument, long hDC, double *ModelToDC, long *Rect);
	STDMETHOD (raw_AfterDocumentSave) (LPDISPATCH theDocument);
	STDMETHOD (raw_AfterEnvironmentActivate) (LPDISPATCH theEnvironment);
	STDMETHOD (raw_AfterNewDocumentOpen) (LPDISPATCH theDocument);
	STDMETHOD (raw_AfterNewWindow) (LPDISPATCH theWindow);
	STDMETHOD (raw_AfterWindowActivate) (LPDISPATCH theWindow);
	STDMETHOD (raw_BeforeCommandRun) (long theCommandID);
	STDMETHOD (raw_BeforeDocumentClose) (LPDISPATCH theDocument);
	STDMETHOD (raw_BeforeDocumentPrint) (LPDISPATCH theDocument, long hDC, double *ModelToDC, long *Rect);
	STDMETHOD (raw_BeforeEnvironmentDeactivate) (LPDISPATCH theEnvironment);
	STDMETHOD (raw_BeforeWindowDeactivate) (LPDISPATCH theWindow);
	STDMETHOD (raw_BeforeQuit) (void);
	STDMETHOD (raw_BeforeDocumentSave) (LPDISPATCH theDocument);

#pragma endregion

#pragma region ISEApplicationWindowEvents

	STDMETHOD (raw_WindowProc) (long hWnd, long nMsg, long wParam, long lParam);

#pragma endregion

#pragma region ISEFeatureLibraryEvents

      STDMETHOD (raw_AfterFeatureLibraryDocumentCreated) (BSTR Name);
      STDMETHOD (raw_AfterFeatureLibraryDocumentRenamed) (BSTR NewName, BSTR OldName);
      STDMETHOD (raw_AfterFeatureLibraryDocumentDeleted) (BSTR Name);

#pragma endregion

#pragma region ISEFileUIEvents

      STDMETHOD (raw_OnFileOpenUI) (BSTR * Filename, BSTR * AppendToTitle);
      STDMETHOD (raw_OnFileSaveAsUI) (BSTR * Filename, BSTR * AppendToTitle);
      STDMETHOD (raw_OnFileNewUI) (BSTR * Filename, BSTR * AppendToTitle);
      STDMETHOD (raw_OnFileSaveAsImageUI) (BSTR * Filename, BSTR * AppendToTitle, long * Width, long * Height, enum SeImageQualityType * ImageQuality);
      STDMETHOD (raw_OnPlacePartUI) (BSTR * Filename, BSTR * AppendToTitle);
      STDMETHOD (raw_OnCreateInPlacePartUI) (BSTR * Filename, BSTR * AppendToTitle, BSTR * Template);

#pragma endregion

#pragma region ISENewFileUIEvents

      STDMETHOD (raw_OnNewFileUI) (enum DocumentTypeConstants DocumentType, BSTR * Filename, BSTR * AppendToTitle);

#pragma endregion

#pragma region ISEECEvents

      STDMETHOD (raw_SEEC_IsPreCPDEventSupported) (VARIANT_BOOL * pvbPreCPDEventSupported);
      STDMETHOD (raw_SEEC_BeforeCPDDisplay) (IDispatch * pCPDInitializer, enum eCPDMode eCPDMode);
      STDMETHOD (raw_PDM_OnFileOpenUI) (BSTR * bstrFilename);

#pragma endregion

#pragma region ISEShortCutMenuEvents

      STDMETHOD (raw_BuildMenu) (BSTR EnvCatID, enum ShortCutMenuContextConstants Context, IDispatch * pGraphicDispatch, SAFEARRAY * * MenuStrings, SAFEARRAY * * CommandIDs);

#pragma endregion
	  
private:

#pragma region CMyAddIn private methods

	HRESULT CreateRibbon(GUID environmentGuid, EnvironmentPtr pEnvironment, VARIANT_BOOL bFirstTime);

#pragma endregion

protected:
	CEdgeBarControllerObj* m_pEdgeBarController;
	CMyViewOverlayObj* m_pMyViewOverlay;
};

OBJECT_ENTRY_AUTO(CLSID_MyAddIn, CMyAddIn)

