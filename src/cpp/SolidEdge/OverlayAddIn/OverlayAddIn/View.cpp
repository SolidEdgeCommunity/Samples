#include "stdafx.h"
#include "Commands.h"
#include "resource.h"
#include "igl.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

static void DrawCubeIGL(LPGL pIGL, float fSize);

CTIMBOView::CTIMBOView()
{
	m_pView = NULL;

	m_pViewEventsObj = NULL;
	m_bViewEvents = FALSE;

	m_phDCDisplayEventsObj = NULL;
	m_bhDCDisplayEvents = FALSE;

	m_pGLDisplayEventsObj = NULL;
	m_bGLDisplayEvents = FALSE;
}

CTIMBOView::~CTIMBOView()
{
	HRESULT hr = UnadviseFromEvents();
	C_RELEASE( m_pViewEventsObj );
	C_RELEASE( m_phDCDisplayEventsObj );
	C_RELEASE( m_pGLDisplayEventsObj );
}

HRESULT CTIMBOView::SetViewObject(IDispatch* pViewDispatch, BOOL bAdviseEvents )
{
	ASSERT( pViewDispatch );

	HRESULT hr = NOERROR;

	m_pView = pViewDispatch;

	if( bAdviseEvents )
	{
		// Create view event handler
		hr = AdviseEvents();
	}

	return hr;
}

HRESULT CTIMBOView::AdviseEvents()
{
	ASSERT( NULL != m_pView );

	HRESULT hr = NOERROR;

	if( NULL == m_pViewEventsObj )
	{
		XViewEventsObj::CreateInstance(&m_pViewEventsObj);
		if( m_pViewEventsObj )
		{
			m_pViewEventsObj->AddRef();
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	if( m_pViewEventsObj )
	{
		IUnknownPtr pUnkViewEvents = m_pView->GetViewEvents();

		if( pUnkViewEvents )
		{
			hr = m_pViewEventsObj->Connect(pUnkViewEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bViewEvents = TRUE;
			}
		}

		m_pViewEventsObj->m_pView = this;
	}

	if( NULL == m_phDCDisplayEventsObj )
	{
		XhDCDisplayEventsObj::CreateInstance(&m_phDCDisplayEventsObj);
		if( m_phDCDisplayEventsObj )
		{
			m_phDCDisplayEventsObj->AddRef();
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	if( m_phDCDisplayEventsObj )
	{
		IUnknownPtr pUnkDisplayEvents = m_pView->GetDisplayEvents();
		if( pUnkDisplayEvents )
		{
			hr = m_phDCDisplayEventsObj->Connect(pUnkDisplayEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bhDCDisplayEvents = TRUE;
			}
		}

		m_phDCDisplayEventsObj->m_pView = this;
	}


	if( NULL == m_pGLDisplayEventsObj )
	{
		XGLDisplayEventsObj::CreateInstance(&m_pGLDisplayEventsObj);
		if( m_pGLDisplayEventsObj )
		{
			m_pGLDisplayEventsObj->AddRef();
		}
		else
		{
			hr = E_OUTOFMEMORY;
		}
	}

	if( m_pGLDisplayEventsObj )
	{
		IUnknownPtr pUnkGLDisplayEvents = m_pView->GetGLDisplayEvents();
		if( pUnkGLDisplayEvents )
		{
			hr = m_pGLDisplayEventsObj->Connect(pUnkGLDisplayEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bGLDisplayEvents = TRUE;
			}
		}

		m_pGLDisplayEventsObj->m_pView = this;
	}

	return hr;
}

HRESULT CTIMBOView::UnadviseFromEvents()
{
	HRESULT hr = NOERROR;

	if( m_bViewEvents && NULL != m_pViewEventsObj )
	{
		IUnknownPtr pUnkViewEvents = m_pView->GetViewEvents();

		if( pUnkViewEvents )
		{
			hr = m_pViewEventsObj->Disconnect(pUnkViewEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bViewEvents = FALSE;
			}
		}
	}

	if( m_bhDCDisplayEvents && NULL != m_phDCDisplayEventsObj )
	{
		IUnknownPtr pUnkDisplayEvents = m_pView->GetDisplayEvents();
		if( pUnkDisplayEvents )
		{
			hr = m_phDCDisplayEventsObj->Disconnect(pUnkDisplayEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bhDCDisplayEvents = FALSE;
			}
		}
	}


	if( m_bGLDisplayEvents && NULL != m_pGLDisplayEventsObj )
	{
		IUnknownPtr pUnkGLDisplayEvents = m_pView->GetGLDisplayEvents();
		if( pUnkGLDisplayEvents )
		{
			hr = m_pGLDisplayEventsObj->Disconnect(pUnkGLDisplayEvents);
			if( SUCCEEDED( hr ) )
			{
				m_bGLDisplayEvents = FALSE;
			}
		}
	}
	return hr;
}

HRESULT CTIMBOView::XViewEvents::raw_Changed( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XViewEvents::raw_Destroyed( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	HRESULT hr = m_pView->GetCommandsObject()->DestroyCTIMBOView(m_pView->m_pView);

	return hr;
}

HRESULT CTIMBOView::XViewEvents::raw_StyleChanged( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XhDCDisplayEvents::raw_BeginDisplay( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XhDCDisplayEvents::raw_EndDisplay( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XhDCDisplayEvents::raw_BeginhDCMainDisplay( long hDC, double *ModelToDC, long *Rect )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XhDCDisplayEvents::raw_EndhDCMainDisplay( long hDC, double *ModelToDC, long *Rect)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}


HRESULT CTIMBOView::XGLDisplayEvents::raw_BeginDisplay( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XGLDisplayEvents::raw_EndDisplay( void )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

HRESULT CTIMBOView::XGLDisplayEvents::raw_BeginIGLMainDisplay( IUnknown* pGL )
{
	LPGL pIGL = NULL;
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	pGL->QueryInterface(IID_IGL, (LPVOID *)&pIGL);

	float fSize = 0.025f;
	double matrix0[16] = {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1};
	double matrix1[16] = {1,0,0,0,0,1,0,0,0,0,1,0,0,0,fSize,1};
	double matrix2[16] = {1,0,0,0,0,1,0,0,0,0,1,0,0,fSize,-fSize,1};
	double matrix3[16] = {1,0,0,0,0,1,0,0,0,0,1,0,fSize,-fSize,0,1};

	pIGL->glMatrixMode(GL_MODELVIEW);


	GLint mode;
	GLint depth;
	GLenum error;

	error = pIGL->glGetError();

	const GLubyte* vendor = pIGL->glGetString(GL_VENDOR);
	const GLubyte* renderer = pIGL->glGetString(GL_RENDERER);
	const GLubyte* version = pIGL->glGetString(GL_VERSION);
	const GLubyte* extensions = pIGL->glGetString(GL_EXTENSIONS);

	pIGL->glGetIntegerv(GL_MATRIX_MODE, &mode);

	pIGL->glGetIntegerv(GL_MODELVIEW_STACK_DEPTH,&depth);
	pIGL->glPushMatrix();
	pIGL->glGetIntegerv(GL_MODELVIEW_STACK_DEPTH,&depth);

	pIGL->glLoadMatrixd(matrix0);
	pIGL->glColor3f(1,0,0);
	DrawCubeIGL(pIGL,fSize/2.0f);

	pIGL->glPopMatrix();


	pIGL->glPushMatrix();

	pIGL->glMultMatrixd(matrix1);
	pIGL->glColor3f(0,1,0);
	DrawCubeIGL(pIGL,fSize/2.0f);

	pIGL->glMultMatrixd(matrix2);
	pIGL->glColor3f(0,0,1);
	DrawCubeIGL(pIGL,fSize/2.0f);

	pIGL->glMultMatrixd(matrix3);
	pIGL->glColor4f(1,1,0,.25f);
	DrawCubeIGL(pIGL, fSize/2.0f);

	pIGL->glPopMatrix();


	return S_OK;
}

HRESULT CTIMBOView::XGLDisplayEvents::raw_EndIGLMainDisplay( IUnknown* pGL )
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

static void DrawCubeIGL(LPGL pIGL, float fSize)
{
	float *p;
	float *n;

	float p0[12] = {0.0f,0.0f, 0.0f, 0.0f,fSize, 0.0f, fSize,0.0f, 0.0f, fSize,fSize, 0.0f};
	float p1[12] = {0.0f,0.0f,fSize, 0.0f,fSize,fSize, fSize,0.0f,fSize, fSize,fSize,fSize};

	float p2[12] = { 0.0f,0.0f,0.0f,  0.0f,0.0f,fSize,  0.0f,fSize,0.0f,  0.0f,fSize,fSize};
	float p3[12] = {fSize,0.0f,0.0f, fSize,0.0f,fSize, fSize,fSize,0.0f, fSize,fSize,fSize};

	float p4[12] = {0.0f, 0.0f,0.0f, 0.0f, 0.0f,fSize, fSize, 0.0f,0.0f, fSize, 0.0f,fSize};
	float p5[12] = {0.0f,fSize,0.0f, 0.0f,fSize,fSize, fSize,fSize,0.0f, fSize,fSize,fSize};

	float n0[21] = {0.0f,0.0f,-1.0f, 0.0f,0.0f, 1.0f,  0.0f,-1.0f,0.0f, 0.0f, 1.0f,0.0f,
		0.0f,0.0f, 1.0f, 0.0f,1.0f, 0.0f,  0.0f,-1.0f,0.0f};

	n = n0;
	p = p0;
	pIGL->glBegin(GL_TRIANGLES);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(FALSE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;

	p -= 6;
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(FALSE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();

	p = p1;
	pIGL->glBegin(GL_TRIANGLE_STRIP);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();

	p = p2;
	pIGL->glBegin(GL_TRIANGLE_STRIP);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();

	p = p3;
	pIGL->glBegin(GL_TRIANGLE_STRIP);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();

	p = p4;
	pIGL->glBegin(GL_TRIANGLE_STRIP);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();

	p = p5;
	pIGL->glBegin(GL_TRIANGLE_STRIP);
	pIGL->glNormal3fv(n); n+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEdgeFlag(TRUE);
	pIGL->glVertex3fv(p); p+=3;
	pIGL->glEnd();
}