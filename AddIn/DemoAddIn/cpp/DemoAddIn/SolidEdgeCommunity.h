#pragma once

#define IfFailGo(x) { hr=(x); if (FAILED(hr)) goto Error; }
#define IfFailGoCheck(x, p) { hr=(x); if (FAILED(hr)) goto Error; if(!p) {hr = E_FAIL; goto Error; } }

namespace SolidEdgeCommunity
{
	template<typename TSink>
	class CEventSink
	{
	public:
		IUnknownPtr m_pUnkCP;   // reference to the connection point pointer
		TSink*      m_pSink;    // pointer to the sink interface object
		DWORD       m_dwCookie;

		CEventSink(TSink* pSink) :
			m_pSink(pSink),
			m_dwCookie(0)
		{
		}

		~CEventSink()
		{
			if (m_dwCookie)
				Unadvise();
		}

		HRESULT Advise(IUnknown* pUnkCP)
		{
			HRESULT hr;
			m_pUnkCP = pUnkCP;
			hr = AtlAdvise(m_pUnkCP, m_pSink, __uuidof(TSink), &m_dwCookie);
			ATLASSERT(SUCCEEDED(hr));
			return hr;
		}

		HRESULT Unadvise(void)
		{
			HRESULT hr = S_OK;
			if (m_dwCookie)
			{
				hr = AtlUnadvise(m_pUnkCP, __uuidof(TSink), m_dwCookie);
				m_pUnkCP = NULL;
				ATLASSERT(SUCCEEDED(hr));
				m_dwCookie = 0;
			}
			return hr;
		}
	};

	HRESULT RegisterSolidEdgeAddIn(CLSID clsid, LPCTSTR name, LPCTSTR summary, ULONG cEnvironments, CATID environments[]);
	HRESULT RegisterSolidEdgeAddIn(CLSID clsid, LCID lcid, LPCTSTR name, LPCTSTR summary, ULONG cEnvironments, CATID environments[]);
	void UnRegisterSolidEdgeAddIn(CLSID clsid);
}