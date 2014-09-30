//#pragma once
//class Commands
//{
//public:
//	Commands(void);
//	~Commands(void);
//};
//
// Commands.h : header file
//

#if !defined(AFX_COMMANDS_H__03B8616A_1896_11D1_BA18_080036230602__INCLUDED_)
#define AFX_COMMANDS_H__03B8616A_1896_11D1_BA18_080036230602__INCLUDED_

#define C_RELEASE(x) if( NULL != x ) { x->Release(); x = NULL; }

#include "stdafx.h"
#include "OverlayAddIn.h"

using namespace SolidEdgeFramework;
//using namespace SolidEdgeFrameworkSupport;
//using namespace SolidEdgePart;
//using namespace SolidEdgeGeometry;
//using namespace SolidEdgeDraft;
//using namespace SolidEdgeAssembly;

// This class is used to handle generic application events and command events
// fired specifically to this addin. Trivial implementations of the application
// events are provided. To handle a specific app event, replace the implementation
// provided with your own.

class CTIMBOView;

// Use the following typedef when creating an instance of CTIMBOView COM object.
typedef CComObject<CTIMBOView> CTIMBOViewObj;

typedef CTypedPtrMap<CMapPtrToPtr,LPDISPATCH,CComObject<CTIMBOView>*> CMapSEViewDispatchToMyView;

class CCommands : 
	public CComObjectRoot,
	public CComCoClass<CCommands, &CLSID_Commands>
{
protected:
  // The one and only Solid Edge application object! Use this object to communicate with the solid
  // edge application, e.g., to obtain your favorite Solid Edge interface (IDispatch)

  ApplicationPtr m_pApplication;
  ISEAddInPtr    m_pSEAddIn;

  CMapSEViewDispatchToMyView m_pViews;

public:
	CCommands();
	~CCommands();
  
  HRESULT SetApplicationObject(IDispatch* pApplicationDispatch, BOOL bEvents = TRUE);
	ApplicationPtr GetApplicationPtr() { return m_pApplication; }
	HRESULT UnadviseFromEvents();

  HRESULT SetAddInObject(AddIn* pSolidEdgeAddIn, BOOL bEvents = TRUE);
	ISEAddInPtr GetAddIn() { return m_pSEAddIn; }

  HRESULT CreateCTIMBOView(LPDISPATCH, BOOL bWithEvents = TRUE, CTIMBOViewObj** ppCTIMBOView = NULL);
  HRESULT DestroyCTIMBOView(LPDISPATCH);
  HRESULT DestroyAllCTIMBOViews( void );
  CTIMBOViewObj* GetView(LPDISPATCH);

	BEGIN_COM_MAP(CCommands)
	END_COM_MAP()
	DECLARE_NOT_AGGREGATABLE(CCommands)

protected:
	
  // This class template is used as the base class for the
	// event handler objects which are declared below.

  template <class IEvents, const IID* piidEvents, const GUID* plibid,
		class XEvents, const CLSID* pClsidEvents>
	class XEventHandler :
		public CComDualImpl<IEvents, piidEvents, plibid>,
		public CComObjectRoot,
		public CComCoClass<XEvents, pClsidEvents>
	{
	public:
		BEGIN_COM_MAP(XEvents)
			COM_INTERFACE_ENTRY_IID(*piidEvents, IEvents)
		END_COM_MAP()
		DECLARE_NOT_AGGREGATABLE(XEvents)
		HRESULT Connect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED(hr = AtlAdvise(pUnk, this, *piidEvents, &m_dwAdvise))); return hr; }
		HRESULT Disconnect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED(hr = AtlUnadvise(pUnk, *piidEvents, m_dwAdvise))); return hr; }

    // Store a back pointer to the embedding command object (makes life easy).
    // Do not delete! Its merely a back pointer to the object that controls the
    // lifetime of your event handlers. Use this pointer in the event handling methods 
    // based on this object template to communicate to the embedding class (CCommands).

    CCommands* m_pCommands;

	protected:
		DWORD m_dwAdvise;
	};

	// This object handles events fired by the Application object
	class XApplicationEvents : public XEventHandler<ISEApplicationEvents, 
		&__uuidof(ISEApplicationEvents), &LIBID_OverlayAddIn, 
		XApplicationEvents, &CLSID_TIMBOApplicationEvents>
	{
	public:
		// ISEApplicationEvents methods
		STDMETHOD (raw_AfterActiveDocumentChange)( IDispatch * theDocument );
		STDMETHOD (raw_AfterCommandRun)( long theCommandID );
		STDMETHOD (raw_AfterDocumentOpen)( IDispatch * theDocument );
		STDMETHOD (raw_AfterDocumentPrint)( IDispatch * theDocument,  long hDC, double *ModelToDC, long *Rect );
		STDMETHOD (raw_AfterDocumentSave)( IDispatch * theDocument );
		STDMETHOD (raw_AfterEnvironmentActivate)( IDispatch * theEnvironment );
		STDMETHOD (raw_AfterNewDocumentOpen)( IDispatch * theDocument );
		STDMETHOD (raw_AfterNewWindow)( IDispatch * theWindow );
		STDMETHOD (raw_AfterWindowActivate)( IDispatch * theWindow );
		STDMETHOD (raw_BeforeCommandRun)( long theCommandID );
		STDMETHOD (raw_BeforeDocumentClose)( IDispatch * theDocument );
		STDMETHOD (raw_BeforeDocumentPrint)( IDispatch * theDocument,  long hDC, double *ModelToDC, long *Rect );
		STDMETHOD (raw_BeforeEnvironmentDeactivate)( IDispatch * theEnvironment );
		STDMETHOD (raw_BeforeWindowDeactivate)( IDispatch * theWindow );
		STDMETHOD (raw_BeforeQuit)();
		STDMETHOD (raw_BeforeDocumentSave)( IDispatch * theDocument );
	};
	typedef CComObject<XApplicationEvents> XApplicationEventsObj;
	XApplicationEventsObj* m_pApplicationEventsObj;


     // This object handles command events fired by Solid Edge to the addin object
	class XAddInEvents : public XEventHandler<ISEAddInEvents, 
		&__uuidof(ISEAddInEvents), &LIBID_OverlayAddIn, 
		XAddInEvents, &CLSID_TIMBOAddInEvents>
	{
	public:
		// ISEAddInEvents methods
    STDMETHOD (raw_OnCommand)( long nCmdID );
		STDMETHOD (raw_OnCommandHelp)( long hFrameWnd, long uHelpCommand, long nCmdID );
		STDMETHOD (raw_OnCommandUpdateUI)( long nCmdID, long *lCmdFlags, BSTR *MenuItemText,
                                   long *nIDBitmap );
	};
	typedef CComObject<XAddInEvents> XAddInEventsObj;
	XAddInEventsObj* m_pAddInEventsObj;

  public:

  friend class XApplicationEvents;
  friend class XAddInEvents;
};
// Use the following typedef when creating an instance of CCommands.
typedef CComObject<CCommands> CCommandsObj;

// Define a base class for an individual command. This class provides all the essential
// ingredients for writing a fully functional Solid Edge command. It pvovides default
// handling of all the various events which can be fired by the Solid Edge object 
// obtained by calling CreateCommand. When an event occurs, a virtual function on the base 
// class is called. ALL ONE HAS TO DO IS SUBCLASS FROM THIS OBJECT AND OVERRIDE ANY
// SPECIFIC EVENT THE SUBCLASS IS INTERESTED IN RECEIVING. Users of the command and mouse
// controls should feel quite familiar with this object. That's because the object returned
// from CreateCommand is essentially the command control and its mouse property is 
// essentially the mouse control.
//

// Note: I choose to class an individual command from IUnknown, which I then add to
//       the COM_MAP because I am using the "GetUnknown" function that the BEGIN_COM_MAP
//       macro declares for the CCommand object. The reason for doing this is that once
//       the CCommands object creates a CCommandObj (the templatized COM version of CCommand),
//       it addrefs the CCommandObj and then abandons the CCommand. From that point on, the
//       lifetime of CCommand is controlled by Solid Edge. Eventually, the XCommandEvents
//       object embedded in CCommands (you can see that object below) receives a Terminate
//       call from Solid Edge. At that point, the command should perform all clean up code
//       and stop execution. Once the CCommand object disconnects from the Solid Edge
//       event sinks, there should be no reference to Solid Edge held by the command, and
//       there is (and never really was) no other reference to the CCommand object. Hence,
//       the CCommand object must dereference itself so that its destructor will be called
//       and hence its allocated memory will be freed.
//
//       But theres a slight problem with that. There is a difference between a CCommandObj
//       object, and a CCommand object. The former is created by the use of the CComObject
//       ATL template, with CCommand as the template argument. That implicitly creates a
//       C++ COM object whose base class is CCommand object. Hence, member functions declared
//       on CCommand object cannot directly call IUnknown functions. They exist somewhere "up"
//       the class graph; I say "somewhere" because I was unable to figure out exactly how to
//       do a dynamic_cast (see C++ documenation) because the compiler complains that my object
//       was not polymorphic!
//
//       Apparently, ATL solves this "dilema" by defining a GetUnknown function in BEGIN_COM_MAP.
//       Sounds simple but there was a caveat. I must have at least one "_ATL_SIMPLEMAP_ENTRY" in
//       the COM map for GetUnknown to work ( asserts without one ). Hence, the reason for adding
//       IUnknown to the object and the map.
//
//       I could not find any documentation on GetUnknown, so I wrote a GetMyUnknown which calls
//       GetUnknown (see code below) to provide some degree of protection in case that ever changes.
//       
//       There is another way around this problem. I could simply write a SetMyUnknown function that
//       stores the IUnknown pointer in instance data. The CCommands object could call this function
//       on the CCommandObj after it creates it and AddRefs the pointer. Then GetMyUnknown could
//       simply return that pointer when it needs to do a Release. If there is ever a problem with
//       what I do now, this solution may be the next best thing. Of course you could make the CCommands
//       object a global, or store its pointer and some cookie which the CCommand object then uses
//       to ask the CCommands object dereference the CCommandObj. Confused? You should have been on
//       the front end of this as this project is my first use of ATL ...
//
//       Alternatively, one could use the fact that any event set that the object connects to results
//       in a reference on that object. Simply allow the disconnection process to delete the object.
//       I chose not to simply to strengthen my control over the lifetime of the object.

class CCommand :
  IUnknown, 
	public CComObjectRoot,
	public CComCoClass<CCommand, &CLSID_TIMBOCommand>
{
protected:
	CCommands* m_pCommands;

  // The ever changing Solid Edge command object! This object is created by the commands object
  // whenever its OnCommand function is called. The CCommands object AddRefs it and then calls
  // SetCommandObject. CCommand must do the final Release of this interface in order to keep
  // from leaking (remember that after creation by CCommands, a CCommand is on its own.
  // Use it to connect up any sinks a particular command needs in order to function successfully 
  // such as the command or mouse event sink and to obtain the Command and Mouse interfaces if they 
  // are needed.

  ISECommandPtr  m_pSECommand;
  ISEMousePtr    m_pSEMouse;

public:
	CCommand();
	virtual ~CCommand();

  LPUNKNOWN GetMyUnknown() { return GetUnknown(); }

  void SetCommandsObject( CCommands* pCommands ) { ASSERT( pCommands ); m_pCommands = pCommands; }
  CCommands* GetCommandsObject() { ASSERT( m_pCommands ); return m_pCommands; }

  virtual HRESULT CreateCommand( SolidEdgeConstants::seCmdFlag CommandType );

  ISECommandPtr GetCommand() { return m_pSECommand; }
  
  // UnadviseFromCommandEvents unadvises from the command event sets (command, mouse, window, well, you get the idea).
  virtual void UnadviseFromCommandEvents();

  // ReleaseInterfaces releases the command, mouse and window interfaces
  virtual void ReleaseInterfaces();

	BEGIN_COM_MAP(CCommand)
		COM_INTERFACE_ENTRY(IUnknown)
	END_COM_MAP()
	DECLARE_NOT_AGGREGATABLE(CCommand)

protected:
  
  //  Here is a base class for the various sinks that can be obtained from the
  //  object Solid Edge returns in the CreateCommand (see Application object).

	template <class IEvents, const IID* piidEvents, const GUID* plibid,
		class XEvents, const CLSID* pClsidEvents>
	class XEventHandler :
		public CComDualImpl<IEvents, piidEvents, plibid>,
		public CComObjectRoot,
		public CComCoClass<XEvents, pClsidEvents>
	{
	public:
		BEGIN_COM_MAP(XEvents)
			COM_INTERFACE_ENTRY_IID(*piidEvents, IEvents)
		END_COM_MAP()
		DECLARE_NOT_AGGREGATABLE(XEvents)
		HRESULT Connect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED( hr = AtlAdvise(pUnk, this, *piidEvents, &m_dwAdvise))); return hr; }
		HRESULT Disconnect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED( hr = AtlUnadvise(pUnk, *piidEvents, m_dwAdvise))); return hr; }

    // Store a back pointer to the embedding command object (makes life easy).
    // Do not delete! Its merely a back pointer to the object that controls the
    // lifetime of your event handlers. Use this pointer in the event handling methods 
    // based on this object template to communicate with the embedding object (CCOmmand).
		CCommand* m_pCommand;
	protected:
		DWORD m_dwAdvise;
	};

	class XCommandEvents : public XEventHandler<ISECommandEvents, 
		&__uuidof(ISECommandEvents), &LIBID_OverlayAddIn, 
		XCommandEvents, &CLSID_CommandEvents>
	{
	public:
		// ISECommandEvents methods
    STDMETHOD(raw_Activate)();
    STDMETHOD(raw_Deactivate)();
    STDMETHOD(raw_Terminate)();
    STDMETHOD(raw_Idle)       (long lCount, VARIANT_BOOL* pbMore);
    STDMETHOD(raw_KeyDown)    (short* KeyCode, short Shift);
    STDMETHOD(raw_KeyPress)   (short* KeyAscii);
    STDMETHOD(raw_KeyUp)      (short* KeyCode, short Shift);
  };
	typedef CComObject<XCommandEvents> XCommandEventsObj;
	XCommandEventsObj* m_pCommandEventsObj;

	class XMouseEvents : public XEventHandler<ISEMouseEvents, 
		&__uuidof(ISEMouseEvents), &LIBID_OverlayAddIn, 
		XMouseEvents, &CLSID_MouseEvents>
	{
	public:
		// ISEMouseEvents methods
    STDMETHOD(raw_MouseDown)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch);
    STDMETHOD(raw_MouseUp)      (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch);
    STDMETHOD(raw_MouseMove)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch);
    STDMETHOD(raw_MouseClick)   (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch);
    STDMETHOD(raw_MouseDblClick)(short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch);
    STDMETHOD(raw_MouseDrag)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, short DragState, long lKeyPointType, LPDISPATCH pGraphicDispatch);
  };
	typedef CComObject<XMouseEvents> XMouseEventsObj;
	XMouseEventsObj* m_pMouseEventsObj;

	class XWindowEvents : public XEventHandler<ISECommandWindowEvents, 
		&__uuidof(ISECommandWindowEvents), &LIBID_OverlayAddIn, 
		XWindowEvents, &CLSID_CommandWindowEvents>
	{
	public:
		// ISEWindowEvents methods
    STDMETHOD(raw_WindowProc)(LPDISPATCH pUnkDoc, LPDISPATCH pUnkView, UINT nMsg, WPARAM wParam, LPARAM lParam, LRESULT *lResult);
  };
	typedef CComObject<XWindowEvents> XWindowEventsObj;
	XWindowEventsObj* m_pWindowEventsObj;

  // Member functions below are called by the embedded event handlers whenever Solid Edge fires
  // an event to one of the handlers. Override the functions below that your CCommand derived
  // class needs to respond to.
  STDMETHOD(Activate)  () {return S_OK;}
  STDMETHOD(Deactivate)() {return S_OK;}
  STDMETHOD(Terminate) () {return S_OK;}
  STDMETHOD(Idle)      ( long lCount, VARIANT_BOOL* pbMore ) { *pbMore = VARIANT_FALSE; return S_OK;};
  STDMETHOD(KeyDown)   ( short* KeyCode, short Shift ) {return S_OK;} 
  STDMETHOD(KeyPress)  ( short* KeyAscii) {return S_OK;}
  STDMETHOD(KeyUp)     ( short* KeyCode, short Shift) {return S_OK;}

  STDMETHOD(MouseDown)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}
  STDMETHOD(MouseUp)      (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}
  STDMETHOD(MouseMove)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}
  STDMETHOD(MouseClick)   (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}
  STDMETHOD(MouseDblClick)(short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}
  STDMETHOD(MouseDrag)    (short sButton, short sShift, double dX, double dY, double dZ, LPDISPATCH pWindowDispatch, short DragState, long lKeyPointType, LPDISPATCH pGraphicDispatch) {return S_OK;}

  STDMETHOD(WindowProc) (LPDISPATCH pUnkDoc, LPDISPATCH pUnkView, UINT nMsg, WPARAM wParam, LPARAM lParam, LRESULT *lResult) {return S_OK;}

  friend class XCommandEvents;
  friend class XMouseEvents;
  friend class XWindowEvents;

  public:
};

// Use the following typedef when creating an instance of CCommand.
typedef CComObject<CCommand> CCommandObj;
typedef CComAggObject<CCommand> CAggCommandObj;

// Use this class to trap display and/or view events.
class CTIMBOView : 
	public CComObjectRoot,
	public CComCoClass<CTIMBOView, &CLSID_TIMBOView>
{
protected:

  ViewPtr m_pView;
  BOOL m_bViewEvents;
  BOOL m_bhDCDisplayEvents;
  BOOL m_bGLDisplayEvents;
  CCommands* m_pCommands; // back pointer, do not Release().

public:
	CTIMBOView();
	~CTIMBOView();
  
  // SetViewObject assumes pViewDisplatch has been AddRef'd.
  HRESULT SetViewObject(IDispatch* pViewDispatch, BOOL bEvents = TRUE);
	ViewPtr GetView() { return m_pView; }
	
  HRESULT UnadviseFromEvents();
  HRESULT AdviseEvents();

  void SetCommandsObject( CCommands* pCommands ) { ASSERT( pCommands ); m_pCommands = pCommands; }
  CCommands* GetCommandsObject() {   ASSERT( m_pCommands ); return m_pCommands; }
	
  BEGIN_COM_MAP(CTIMBOView)
	END_COM_MAP()
	DECLARE_NOT_AGGREGATABLE(CTIMBOView)

protected:
	
  // This class template is used as the base class for the
	// event handler object below.

  template <class IEvents, const IID* piidEvents, const GUID* plibid,
		class XEvents, const CLSID* pClsidEvents>
	class XEventHandler :
		public CComDualImpl<IEvents, piidEvents, plibid>,
		public CComObjectRoot,
		public CComCoClass<XEvents, pClsidEvents>
	{
	public:
    XEventHandler() { m_pView = NULL; }
		BEGIN_COM_MAP(XEvents)
			COM_INTERFACE_ENTRY_IID(*piidEvents, IEvents)
		END_COM_MAP()
		DECLARE_NOT_AGGREGATABLE(XEvents)
		HRESULT Connect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED(hr = AtlAdvise(pUnk, this, *piidEvents, &m_dwAdvise))); return hr; }
		HRESULT Disconnect(IUnknown* pUnk)
		{ HRESULT hr; VERIFY(SUCCEEDED(hr = AtlUnadvise(pUnk, *piidEvents, m_dwAdvise))); return hr; }

    // Store a back pointer to the embedding doc object (makes life easy).
    // Do not delete! Its merely a back pointer to the object that controls the
    // lifetime of your event handlers. Use this pointer in the event handling methods 
    // based on this object template to communicate to the embedding class.

    CTIMBOView* m_pView;

	protected:
		DWORD m_dwAdvise;
	};

     // This object handles view events fired by Solid Edge
	class XViewEvents : public XEventHandler<ISEViewEvents, 
		&__uuidof(ISEViewEvents), &LIBID_OverlayAddIn, 
		XViewEvents, &CLSID_TIMBOViewEvents>
	{
	public:
		// ISEViewEvents methods
    STDMETHOD (raw_Changed)( void );
    STDMETHOD (raw_Destroyed)( void );
    STDMETHOD (raw_StyleChanged)( void );
	};
	typedef CComObject<XViewEvents> XViewEventsObj;
	XViewEventsObj* m_pViewEventsObj;

     // This object handles display events fired by Solid Edge. The 
     // source of the events is a view.
	class XhDCDisplayEvents : public XEventHandler<ISEhDCDisplayEvents, 
		&__uuidof(ISEhDCDisplayEvents), &LIBID_OverlayAddIn, 
		XhDCDisplayEvents, &CLSID_TIMBOhDCDisplayEvents>
	{
	public:
		// ISEhDCDisplayEvents methods
    STDMETHOD (raw_BeginDisplay)( void );
    STDMETHOD (raw_EndDisplay)( void );
    STDMETHOD (raw_BeginhDCMainDisplay)( long hDC, double *ModelToDC, long *Rect );
    STDMETHOD (raw_EndhDCMainDisplay)( long hDC, double *ModelToDC, long *Rect);
	};
	typedef CComObject<XhDCDisplayEvents> XhDCDisplayEventsObj;
	XhDCDisplayEventsObj* m_phDCDisplayEventsObj;


     // This object handles gl display events fired by Solid Edge. The 
     // source of the events is a view.
	class XGLDisplayEvents : public XEventHandler<ISEIGLDisplayEvents, 
		&__uuidof(ISEIGLDisplayEvents), &LIBID_OverlayAddIn, 
		XGLDisplayEvents, &CLSID_TIMBOGLDisplayEvents>
	{
	public:
		// ISEIGLDisplayEvents methods
    STDMETHOD (raw_BeginDisplay)( void );
    STDMETHOD (raw_EndDisplay)( void );
    STDMETHOD (raw_BeginIGLMainDisplay)( IUnknown* pGL );
    STDMETHOD (raw_EndIGLMainDisplay)( IUnknown* pGL );
	};
	typedef CComObject<XGLDisplayEvents> XGLDisplayEventsObj;
	XGLDisplayEventsObj* m_pGLDisplayEventsObj;
  public:

  friend class XViewEvents;
  friend class XhDCDisplayEvents;
  friend class XGLDisplayEvents;
};
//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_COMMANDS_H__03B8616A_1896_11D1_BA18_080036230602__INCLUDED)
