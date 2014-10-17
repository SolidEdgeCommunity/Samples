// MyAddIn.h : Declaration of the CMyAddIn

#pragma once
#include "resource.h"       // main symbols
#include "AddInDemo_i.h"
//#include "commands.h"
#include "SolidEdgeCommunity.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CMyAddIn

// AddIn GUID Must be unique!
class __declspec(uuid("{EE3C24E4-BB07-4C3F-909C-031C84EC3F07}")) CMyAddIn;

class ATL_NO_VTABLE CMyAddIn :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMyAddIn, &__uuidof(CMyAddIn)>,
	public SolidEdgeFramework::ISolidEdgeAddIn,
	public ISEAddInEvents, public CEventSink<ISEAddInEvents>,
	//public ISEAddInEventsEx, public CEventSink<ISEAddInEventsEx>, // Added in ST6
	public ISEApplicationEvents, public CEventSink<ISEApplicationEvents>
	//public ISEApplicationWindowEvents, public CEventSink<ISEApplicationWindowEvents>,
	//public ISEFeatureLibraryEvents, public CEventSink<ISEFeatureLibraryEvents>,
	//public ISEFileUIEvents, public CEventSink<ISEFileUIEvents>,
	//public ISENewFileUIEvents, public CEventSink<ISENewFileUIEvents>,
	//public ISEECEvents, public CEventSink<ISEECEvents>,
	//public ISEShortCutMenuEvents, public CEventSink<ISEShortCutMenuEvents>
{
public:
	CMyAddIn() :
		CEventSink<ISEAddInEvents>(this),
		//CEventSink<ISEAddInEventsEx>(this),
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
		COM_INTERFACE_ENTRY(ISEAddInEvents)
		//COM_INTERFACE_ENTRY(ISEAddInEventsEx)
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
		return S_OK;
	}

	void FinalRelease()
	{
	}

protected:

public:

#pragma region ISolidEdgeAddIn

	STDMETHOD (raw_OnConnection) (LPDISPATCH Application, SolidEdgeFramework::SeConnectMode ConnectMode, SolidEdgeFramework::AddIn * AddInInstance);
	STDMETHOD (raw_OnConnectToEnvironment) (BSTR EnvCatID, LPDISPATCH pEnvironmentDispatch, VARIANT_BOOL bFirstTime);
	STDMETHOD (raw_OnDisconnection) (SolidEdgeFramework::SeDisconnectMode DisconnectMode);

#pragma endregion

#pragma region ISEAddInEvents

	STDMETHOD (raw_OnCommand) ( long nCmdID );
	STDMETHOD (raw_OnCommandHelp) ( long hFrameWnd, long uHelpCommand, long nCmdID );
	STDMETHOD (raw_OnCommandUpdateUI) ( long nCmdID, long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap );

#pragma endregion

#pragma region ISEAddInEventsEx

	//STDMETHOD (raw_OnCommand) ( long nCmdID );
	//STDMETHOD (raw_OnCommandHelp) ( long hFrameWnd, long uHelpCommand, long nCmdID );
	//STDMETHOD (raw_OnCommandUpdateUI) ( long nCmdID,  long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap );
	//STDMETHOD (raw_OnCommandOnLineHelp) ( long uHelpCommand, long nCmdID, BSTR* HelpURL );

#pragma endregion

#pragma region ISEApplicationEvents

	STDMETHOD (raw_AfterActiveDocumentChange) ( LPDISPATCH theDocument );
	STDMETHOD (raw_AfterCommandRun) ( long theCommandID );
	STDMETHOD (raw_AfterDocumentOpen) ( LPDISPATCH theDocument );
	STDMETHOD (raw_AfterDocumentPrint) ( LPDISPATCH theDocument, long hDC, double *ModelToDC, long *Rect );
	STDMETHOD (raw_AfterDocumentSave) ( LPDISPATCH theDocument );
	STDMETHOD (raw_AfterEnvironmentActivate) ( LPDISPATCH theEnvironment );
	STDMETHOD (raw_AfterNewDocumentOpen) ( LPDISPATCH theDocument );
	STDMETHOD (raw_AfterNewWindow) ( LPDISPATCH theWindow );
	STDMETHOD (raw_AfterWindowActivate) ( LPDISPATCH theWindow );
	STDMETHOD (raw_BeforeCommandRun) ( long theCommandID );
	STDMETHOD (raw_BeforeDocumentClose) ( LPDISPATCH theDocument );
	STDMETHOD (raw_BeforeDocumentPrint) ( LPDISPATCH theDocument, long hDC, double *ModelToDC, long *Rect );
	STDMETHOD (raw_BeforeEnvironmentDeactivate) ( LPDISPATCH theEnvironment );
	STDMETHOD (raw_BeforeWindowDeactivate) ( LPDISPATCH theWindow );
	STDMETHOD (raw_BeforeQuit) ( void );
	STDMETHOD (raw_BeforeDocumentSave) ( LPDISPATCH theDocument );

#pragma endregion

#pragma region ISEApplicationWindowEvents

	STDMETHOD (raw_WindowProc) ( long hWnd, long nMsg, long wParam, long lParam );

#pragma endregion

#pragma region ISEFeatureLibraryEvents

      STDMETHOD (raw_AfterFeatureLibraryDocumentCreated) ( BSTR Name );
      STDMETHOD (raw_AfterFeatureLibraryDocumentRenamed) ( BSTR NewName, BSTR OldName );
      STDMETHOD (raw_AfterFeatureLibraryDocumentDeleted) ( BSTR Name );

#pragma endregion

#pragma region ISEFileUIEvents

      STDMETHOD (raw_OnFileOpenUI) ( BSTR * Filename, BSTR * AppendToTitle );
      STDMETHOD (raw_OnFileSaveAsUI) ( BSTR * Filename, BSTR * AppendToTitle );
      STDMETHOD (raw_OnFileNewUI) ( BSTR * Filename, BSTR * AppendToTitle );
      STDMETHOD (raw_OnFileSaveAsImageUI) ( BSTR * Filename, BSTR * AppendToTitle, long * Width, long * Height, enum SeImageQualityType * ImageQuality );
      STDMETHOD (raw_OnPlacePartUI) ( BSTR * Filename, BSTR * AppendToTitle );
      STDMETHOD (raw_OnCreateInPlacePartUI) ( BSTR * Filename, BSTR * AppendToTitle, BSTR * Template );

#pragma endregion

#pragma region ISENewFileUIEvents

      STDMETHOD (raw_OnNewFileUI) ( enum DocumentTypeConstants DocumentType, BSTR * Filename, BSTR * AppendToTitle );

#pragma endregion

#pragma region ISEECEvents

      STDMETHOD (raw_SEEC_IsPreCPDEventSupported) ( VARIANT_BOOL * pvbPreCPDEventSupported );
      STDMETHOD (raw_SEEC_BeforeCPDDisplay) ( IDispatch * pCPDInitializer, enum eCPDMode eCPDMode );
      STDMETHOD (raw_PDM_OnFileOpenUI) ( BSTR * bstrFilename );

#pragma endregion

#pragma region ISEShortCutMenuEvents

      STDMETHOD (raw_BuildMenu) ( BSTR EnvCatID, enum ShortCutMenuContextConstants Context, IDispatch * pGraphicDispatch, SAFEARRAY * * MenuStrings, SAFEARRAY * * CommandIDs );

#pragma endregion
	  
private:

#pragma region CMyAddIn private methods

	void CreateEnvironmentRibbon(GUID environmentGuid, EnvironmentPtr pEnvironment, VARIANT_BOOL bFirstTime);

#pragma endregion

private:
	SolidEdgeFramework::ApplicationPtr m_pApplication;
	SolidEdgeFramework::AddInPtr m_pAddIn;
	//LPCTSTR m_pResourceFilename;
};

OBJECT_ENTRY_AUTO(__uuidof(CMyAddIn), CMyAddIn)

