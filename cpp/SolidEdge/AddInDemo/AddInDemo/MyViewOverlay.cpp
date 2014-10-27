#include "stdafx.h"
#include "MyViewOverlay.h"

#pragma region CMyViewOverlay

HRESULT CMyViewOverlay::SetView(SolidEdgeFramework::WindowPtr pWindow)
{
	SolidEdgeFramework::ViewPtr pView = NULL;

	if (pWindow != NULL)
	{
		pView = pWindow->View;
	}

	return SetView(pView);
}

HRESULT CMyViewOverlay::SetView(SolidEdgeFramework::ViewPtr pView)
{
	HRESULT hr = S_OK;

	hr = UnadviseFromEvents();

	m_pView = pView;

	if (m_pView != NULL)
	{
		XViewEventsObj::CreateInstance(&m_pViewEventsObj);
		m_pViewEventsObj->AddRef();
		hr = m_pViewEventsObj->Connect(m_pView->ViewEvents);
		m_pViewEventsObj->m_pMyViewOverlay = this;

		XhDCDisplayEventsObj::CreateInstance(&m_phDCDisplayEventsObj);
		m_phDCDisplayEventsObj->AddRef();
		hr = m_phDCDisplayEventsObj->Connect(m_pView->DisplayEvents);
		m_phDCDisplayEventsObj->m_pMyViewOverlay = this;

		XGLDisplayEventsObj::CreateInstance(&m_pGLDisplayEventsObj);
		m_pGLDisplayEventsObj->AddRef();
		hr = m_pGLDisplayEventsObj->Connect(m_pView->GLDisplayEvents);
		m_pGLDisplayEventsObj->m_pMyViewOverlay = this;
	}

	return hr;
}

HRESULT CMyViewOverlay::UnadviseFromEvents()
{
	HRESULT hr = S_OK;

	if (m_pViewEventsObj != NULL)
	{
		hr = m_pViewEventsObj->Disconnect(m_pView->ViewEvents);
		m_pViewEventsObj->Release();
		m_pViewEventsObj = NULL;
	}

	if (m_phDCDisplayEventsObj != NULL)
	{
		hr = m_phDCDisplayEventsObj->Disconnect(m_pView->DisplayEvents);
		m_phDCDisplayEventsObj->Release();
		m_phDCDisplayEventsObj = NULL;
	}

	if (m_pGLDisplayEventsObj != NULL)
	{
		hr = m_pGLDisplayEventsObj->Disconnect(m_pView->GLDisplayEvents);
		m_pGLDisplayEventsObj->Release();
		m_pGLDisplayEventsObj = NULL;
	}

	return S_OK;
}

#pragma endregion

#pragma region CMyViewOverlay::XViewEvents (ISEViewEvents)

HRESULT CMyViewOverlay::XViewEvents::raw_Changed()
{
	return S_OK;
}

HRESULT CMyViewOverlay::XViewEvents::raw_Destroyed()
{
	HRESULT hr = S_OK;

	SolidEdgeFramework::ViewPtr pView = NULL;
	hr = m_pMyViewOverlay->SetView(pView);

	return S_OK;
}

HRESULT CMyViewOverlay::XViewEvents::raw_StyleChanged()
{
	return S_OK;
}

#pragma endregion

#pragma region ISEhDCDisplayEvents

HRESULT CMyViewOverlay::XhDCDisplayEvents::raw_BeginDisplay()
{
	return S_OK;
}

HRESULT CMyViewOverlay::XhDCDisplayEvents::raw_EndDisplay()
{
	return S_OK;
}

HRESULT CMyViewOverlay::XhDCDisplayEvents::raw_BeginhDCMainDisplay(long hDC, double *ModelToDC, long *Rect)
{
	return S_OK;
}

HRESULT CMyViewOverlay::XhDCDisplayEvents::raw_EndhDCMainDisplay(long hDC, double *ModelToDC, long *Rect)
{
	return S_OK;
}

#pragma endregion

#pragma region ISEIGLDisplayEvents

HRESULT CMyViewOverlay::XGLDisplayEvents::raw_BeginDisplay()
{
	return S_OK;
}

HRESULT CMyViewOverlay::XGLDisplayEvents::raw_EndDisplay()
{
	return S_OK;
}

HRESULT CMyViewOverlay::XGLDisplayEvents::raw_BeginIGLMainDisplay(LPUNKNOWN pGL)
{
	IGLPtr pIGL = pGL;
	GLint iMatrixMode = 0;
	const GLubyte* pVersionString = NULL;

#pragma region IGL
	// Demo interacting with OpenGL via IGL interface.
	if (pIGL != NULL)
	{
		// Returns "UNSUPPORTED" on my machine.
		pVersionString = pIGL->glGetString(GL_VERSION);

		pIGL->glGetIntegerv(GL_MATRIX_MODE, &iMatrixMode);

		switch (iMatrixMode)
		{
		case GL_MODELVIEW:
			break;
		case GL_PROJECTION:
			break;
		case GL_TEXTURE:
			break;
		}
	}
#pragma endregion

	// Reset variables.
	iMatrixMode = 0;
	pVersionString = NULL;

#pragma region No IGL
	// Demo interacting with OpenGL directly. (Not recommended...)
	// Note this required modifying linker input to include OpenGL32.lib.

	// Returns "4.4.0 NVIDIA 344.48" on my machine.
	pVersionString = ::glGetString(GL_VERSION);

	::glGetIntegerv(GL_MATRIX_MODE, &iMatrixMode);

	switch (iMatrixMode)
	{
	case GL_MODELVIEW:
		break;
	case GL_PROJECTION:
		break;
	case GL_TEXTURE:
		break;
	}

#pragma endregion

	return S_OK;
}

HRESULT CMyViewOverlay::XGLDisplayEvents::raw_EndIGLMainDisplay(LPUNKNOWN pGL)
{
	IGLPtr pIGL = pGL;

	return S_OK;
}

#pragma endregion