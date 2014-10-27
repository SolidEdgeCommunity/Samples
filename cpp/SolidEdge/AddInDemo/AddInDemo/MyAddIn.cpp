// MyAddIn.cpp : Implementation of CMyAddIn

#include "stdafx.h"
#include "MyAddIn.h"
#include "MyCommands.h"

// CMyAddIn
#pragma region ISolidEdgeAddIn

STDMETHODIMP CMyAddIn::raw_OnConnection(IDispatch *pAppDispatch, SolidEdgeFramework::SeConnectMode ConnectMode, SolidEdgeFramework::AddIn* pAddIn)
{
	AFX_MANAGE_STATE( AfxGetStaticModuleState() );

	HRESULT hr = S_OK;

	m_pApplication = pAppDispatch;
	m_pAddIn = pAddIn;

	// Prepending a "\n" to the description will give the addin it's own ribbon tab in ST & later.
	_bstr_t bstrDescription = L"\n" + m_pAddIn->Description;
	m_pAddIn->Description = bstrDescription;

	// If you make changes to your ribbon, you must change the GuiVersion.
	// This tells SE to purge any saved data about your ribbon and recreate it.
	// This will also cause the bFirstTime parameter of OnConnectToEnvironment to be TRUE.
	m_pAddIn->GuiVersion = 2;

	// Attach the event sinks
	CEventSink<ISEAddInEvents>::Advise(m_pAddIn);
	//CEventSink<ISEAddInEventsEx>::Advise(m_pAddIn); // Added in ST6
	CEventSink<ISEApplicationEvents>::Advise(m_pApplication);
	//CEventSink<ISEApplicationWindowEvents>::Advise( m_pApplication);
	//CEventSink<ISEFileUIEvents>::Advise(m_pApplication);
	//CEventSink<ISENewFileUIEvents>::Advise(m_pApplication);
	//CEventSink<ISEECEvents>::Advise(m_pApplication);
	//CEventSink<ISEShortCutMenuEvents>::Advise(m_pApplication);

	CMyViewOverlayObj::CreateInstance(&m_pMyViewOverlay);
	m_pMyViewOverlay->AddRef();

	return S_OK;
}

STDMETHODIMP CMyAddIn::raw_OnConnectToEnvironment(BSTR EnvironmentCatid, LPDISPATCH pEnvironment, VARIANT_BOOL bFirstTime)
{
	HRESULT hr = S_OK;

	// Convert the environment catid string to a GUID.
	GUID environmentGuid;
	CLSIDFromString(EnvironmentCatid, &environmentGuid);

	// Get a strongly typed Environment smart pointer.
	EnvironmentPtr environment = pEnvironment;

	CreateRibbon(environmentGuid, environment, bFirstTime);

	return S_OK;
}

STDMETHODIMP CMyAddIn::raw_OnDisconnection(SolidEdgeFramework::SeDisconnectMode DisconnectMode)
{
	SolidEdgeFramework::ViewPtr pView = NULL;

	if (m_pMyViewOverlay != NULL)
	{
		m_pMyViewOverlay->SetView(pView);
		m_pMyViewOverlay->Release();
		m_pMyViewOverlay = NULL;
	}

	// Detach the event sinks
	CEventSink<ISEAddInEvents>::Unadvise();
	//CEventSink<ISEAddInEventsEx>::Unadvise(); // Added in ST6
	CEventSink<ISEApplicationEvents>::Unadvise();
	//CEventSink<ISEApplicationWindowEvents>::Unadvise();
	//CEventSink<ISEFileUIEvents>::Unadvise();
	//CEventSink<ISENewFileUIEvents>::Unadvise();
	//CEventSink<ISEECEvents>::Unadvise();
	//CEventSink<ISEShortCutMenuEvents>::Unadvise();

	return S_OK;
}

#pragma endregion

#pragma region ISEAddInEvents

HRESULT CMyAddIn::raw_OnCommand(long nCmdID)
{
	switch (nCmdID)
	{
	case idSave:
		break;
	case idFolder:
		break;
	case idMonitor:
		break;
	case idBox:
		break;
	case idCamera:
		break;
	case idPhotograph:
		break;
	case idFavorites:
		break;
	case idPrinter:
		break;
	case idTools:
		break;
	case idCommandPrompt:
		break;
	case idNotepad:
		break;
	case idHelp:
		break;
	case idSearch:
		break;
	case idQuestion:
		break;
	case idCheckbox1:
		break;
	case idCheckbox2:
		break;
	case idCheckbox3:
		break;
	case idRadiobutton1:
		break;
	case idRadiobutton2:
		break;
	case idRadiobutton3:
		break;
	case idBoundingBox:
		break;
	case idBoxes:
		break;
	case idGdiPlus:
		break;
	}

	return S_OK;
}

HRESULT CMyAddIn::raw_OnCommandHelp(long hFrameWnd, long uHelpCommand, long nCmdID)
{
	return S_OK;
}

HRESULT CMyAddIn::raw_OnCommandUpdateUI(long nCmdID, long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap)
{
	switch (nCmdID)
	{
	case idSave:
		break;
	case idFolder:
		break;
	case idMonitor:
		break;
	case idBox:
		break;
	case idCamera:
		break;
	case idPhotograph:
		break;
	case idFavorites:
		break;
	case idPrinter:
		break;
	case idTools:
		break;
	case idCommandPrompt:
		break;
	case idNotepad:
		break;
	case idHelp:
		break;
	case idSearch:
		break;
	case idQuestion:
		break;
	case idCheckbox1:
		// Demonstrate how to set the check state.
		*lCmdFlags |= SolidEdgeConstants::seCmdActive_Checked;
		break;
	case idCheckbox2:
		break;
	case idCheckbox3:
		break;
	case idRadiobutton1:
		break;
	case idRadiobutton2:
		// Demonstrate how to set the check state.
		*lCmdFlags |= SolidEdgeConstants::seCmdActive_Checked;
		break;
	case idRadiobutton3:
		break;
	case idBoundingBox:
		break;
	case idBoxes:
		break;
	case idGdiPlus:
		break;
	}

	return S_OK;
}

#pragma endregion

#pragma region ISEAddInEventsEx (Added in ST6)

//HRESULT CMyAddIn::raw_OnCommand(long nCmdID)
//{
//	return S_OK;
//}
//
//HRESULT CMyAddIn::raw_OnCommandHelp(long hFrameWnd, long uHelpCommand, long nCmdID)
//{
//	return S_OK;
//}
//
//HRESULT CMyAddIn::raw_OnCommandUpdateUI(long nCmdID, long *lCmdFlags, BSTR *MenuItemText, long *nIDBitmap)
//{
//	return S_OK;
//}
//
//HRESULT CMyAddIn::raw_OnCommandOnLineHelp(long uHelpCommand, long nCmdID, BSTR* HelpURL)
//{
//	return S_OK;
//}

#pragma endregion

#pragma region ISEApplicationEvents

HRESULT CMyAddIn::raw_AfterActiveDocumentChange( LPDISPATCH theDocument)
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterDocumentOpen( LPDISPATCH theDocument )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterDocumentPrint( LPDISPATCH theDocument,
										 long hDC,
										 double *ModelToDC,
										 long *Rect )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterDocumentSave( LPDISPATCH theDocument )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterEnvironmentActivate( LPDISPATCH theEnvironment )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterNewDocumentOpen( LPDISPATCH theDocument )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterNewWindow( LPDISPATCH theWindow )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterWindowActivate( LPDISPATCH theWindow )
{
	SolidEdgeFramework::WindowPtr pWindow = theWindow;

	if (pWindow	!= NULL)
	{
		m_pMyViewOverlay->SetView(pWindow);
	}

	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeCommandRun( long lCommandID )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterCommandRun( long lCommandID )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeDocumentClose( LPDISPATCH theDocument )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeDocumentPrint( LPDISPATCH theDocument,
										  long hDC,
										  double *ModelToDC,
										  long *Rect )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeEnvironmentDeactivate( LPDISPATCH theEnvironment )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeWindowDeactivate( LPDISPATCH theWindow )
{
	SolidEdgeFramework::ViewPtr pView = NULL;
	m_pMyViewOverlay->SetView(pView);

	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeQuit( void )
{
	return S_OK;
}

HRESULT CMyAddIn::raw_BeforeDocumentSave( LPDISPATCH theDocument )
{
	return S_OK;
}

#pragma endregion

#pragma region ISEApplicationWindowEvents

HRESULT CMyAddIn::raw_WindowProc(long hWnd, long nMsg, long wParam, long lParam)
{
	return S_OK;
}

#pragma endregion

#pragma region ISEFeatureLibraryEvents

HRESULT CMyAddIn::raw_AfterFeatureLibraryDocumentCreated(BSTR Name)
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterFeatureLibraryDocumentRenamed(BSTR NewName, BSTR OldName)
{
	return S_OK;
}

HRESULT CMyAddIn::raw_AfterFeatureLibraryDocumentDeleted(BSTR Name)
{
	return S_OK;
}

#pragma endregion

#pragma region ISEFileUIEvents

HRESULT CMyAddIn::raw_OnFileOpenUI(BSTR * Filename, BSTR * AppendToTitle)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_OnFileSaveAsUI(BSTR * Filename, BSTR * AppendToTitle)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_OnFileNewUI(BSTR * Filename, BSTR * AppendToTitle)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_OnFileSaveAsImageUI(BSTR * Filename, BSTR * AppendToTitle, long * Width, long * Height, enum SeImageQualityType * ImageQuality)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_OnPlacePartUI(BSTR * Filename, BSTR * AppendToTitle)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_OnCreateInPlacePartUI(BSTR * Filename, BSTR * AppendToTitle, BSTR * Template)
{
	return E_NOTIMPL;
}

#pragma endregion

#pragma region ISENewFileUIEvents

HRESULT CMyAddIn::raw_OnNewFileUI(enum DocumentTypeConstants DocumentType, BSTR * Filename, BSTR * AppendToTitle)
{
	return E_NOTIMPL;
}

#pragma endregion

#pragma region ISEECEvents

HRESULT CMyAddIn::raw_SEEC_IsPreCPDEventSupported(VARIANT_BOOL * pvbPreCPDEventSupported)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_SEEC_BeforeCPDDisplay(IDispatch * pCPDInitializer, enum eCPDMode eCPDMode)
{
	return E_NOTIMPL;
}

HRESULT CMyAddIn::raw_PDM_OnFileOpenUI(BSTR * bstrFilename)
{
	return E_NOTIMPL;
}

#pragma endregion

#pragma region ISEShortCutMenuEvents

HRESULT CMyAddIn::raw_BuildMenu(BSTR EnvCatID, enum ShortCutMenuContextConstants Context, IDispatch * pGraphicDispatch, SAFEARRAY * * MenuStrings, SAFEARRAY * * CommandIDs)
{
	return E_NOTIMPL;
}

#pragma endregion

void CMyAddIn::CreateRibbon(GUID environmentGuid, EnvironmentPtr pEnvironment, VARIANT_BOOL bFirstTime)
{
	HRESULT hr = S_OK;

	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	ISEAddInExPtr pAddInEx = m_pAddIn;
	//ISEAddInEx2Ptr pAddInEx2 = m_pAddIn;

	// Path to resources .dll.
	_bstr_t bstrResourceFilename(m_strResourceFilename);

	// Loop through MyEnvironments checking to see if we have configured commands for this environment.
	for (UINT i = 0; i < _countof(MyEnvironments); i++)
	{
		if (IsEqualGUID(MyEnvironments[i].environmentGuid, environmentGuid) == TRUE)
		{
			// Add the commands to the environment.
			for (UINT j = 0; j < MyEnvironments[i].nCommands; j++)
			{
				MY_COMMAND_INFO pCommandInfo = MyEnvironments[i].pCommands[j];
				long commandId = pCommandInfo.iCommand;

				// Load category from string resource.
				CString szCategory;
				ATLVERIFY(szCategory.LoadString(pCommandInfo.iCategory));

				_bstr_t bstrCategory(szCategory);

				// Load localized string resource.
				CString szLocalized;
				ATLVERIFY(szLocalized.LoadString(pCommandInfo.iString));

				// Prepend the non-localized prefix.
				CString szCommandString = m_strCommandPrefix;// csPrefix;// +szLocalized;
				szCommandString.AppendFormat(L"_Command%d", pCommandInfo.iCommand);
				szCommandString.Append(L"\n");
				szCommandString.Append(szLocalized);

				CComSafeArray<BSTR> saCmdStrings(1);
				CComSafeArray<long> saCmdIDs(1);

				// Populate arrays.
				hr = saCmdStrings.SetAt((long)0, szCommandString.AllocSysString());
				hr = saCmdIDs.SetAt( (long)0, pCommandInfo.iCommand );

				hr = pAddInEx->SetAddInInfoEx(
					bstrResourceFilename,			// ResourceFilename
					pEnvironment->CATID,			// EnvironmentCatID
					bstrCategory,					// CategoryName (Ribbon Tab Name)
					pCommandInfo.iImage,			// IDColorBitmapMedium
					-1,								// IDColorBitmapLarge
					-1,								// IDMonochromeBitmapMedium
					-1,								// IDMonochromeBitmapLarge
					1,								// NumberOfCommands
					saCmdStrings.GetSafeArrayPtr(),	// CommandNames
					saCmdIDs.GetSafeArrayPtr() );	// CommandIDs

				if (bFirstTime == VARIANT_TRUE)
				{
					CComBSTR bstrCommandBarName;

					bstrCommandBarName.AppendBSTR(bstrCategory);
					bstrCommandBarName.Append(L"\n");

					// Load group from string resource.
					CComBSTR bstrGroup;
					ATLVERIFY(bstrGroup.LoadString( pCommandInfo.iGroup));
					
					bstrCommandBarName.Append(bstrGroup.m_str);

					CommandBarButtonPtr pButton = pAddInEx->AddCommandBarButton(pEnvironment->CATID, bstrCommandBarName.m_str, pCommandInfo.iCommand); 

					if( pButton )
					{
						pButton->Style = pCommandInfo.buttonStyle;
					}
				}
			}

			break;
		}
	}
}