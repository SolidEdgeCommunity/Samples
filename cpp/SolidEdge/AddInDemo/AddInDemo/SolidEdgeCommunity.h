#pragma once

#define IfFailGo(x) { hr=(x); if (FAILED(hr)) goto Error; }
#define IfFailGoCheck(x, p) { hr=(x); if (FAILED(hr)) goto Error; if(!p) {hr = E_FAIL; goto Error; } }

namespace SolidEdgeCommunity
{
	class CAddInRegistrar
	{
	public:
		CAddInRegistrar()
		{
		}

		~CAddInRegistrar()
		{
		}

		static HRESULT Register(CLSID clsid, LPCTSTR name, LPCTSTR summary, ULONG cEnvironments, CATID environments[]);
		static HRESULT Register(CLSID clsid, LCID lcid, LPCTSTR name, LPCTSTR summary, ULONG cEnvironments, CATID environments[]);
		static void Unregister(CLSID clsid);
	};

	template<typename TSink>
	class CEventSink
	{
	public:
		IUnknownPtr m_pUnkCP;   // reference to the connection point pointer
		TSink*      m_pSink;    // pointer to the sink interface object
		DWORD       m_dwCookie;

		CEventSink( TSink* pSink ) :
			m_pSink(pSink),
			m_dwCookie(0)
		{
		}

		~CEventSink()
		{
			if (m_dwCookie)
				Unadvise();
		}

		HRESULT Advise( IUnknown* pUnkCP )
		{
			HRESULT hr;
			m_pUnkCP = pUnkCP;
			hr = AtlAdvise( m_pUnkCP, m_pSink, __uuidof(TSink), &m_dwCookie );
			ATLASSERT(SUCCEEDED(hr));
			return hr;
		}

		HRESULT Unadvise(void)
		{
			HRESULT hr = S_OK;
			if (m_dwCookie)
			{
				hr = AtlUnadvise( m_pUnkCP, __uuidof(TSink), m_dwCookie );
				m_pUnkCP = NULL;
				ATLASSERT(SUCCEEDED(hr));
				m_dwCookie = 0;
			}
			return hr;
		}
	};

	template <class T, const CLSID* pclsid = &CLSID_NULL>
	class CSolidEdgeAddIn
	{
	public:
		CSolidEdgeAddIn()
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
			OLECHAR szGuid[GUID_STRING_LENGTH] = {0};
			::StringFromGUID2(*pclsid, szGuid, GUID_STRING_LENGTH );
			CString csGuid(szGuid);
			m_strCommandPrefix = csGuid.Mid(1, 8);

#pragma endregion
		}

		~CSolidEdgeAddIn()
		{
		}

	protected:
		SolidEdgeFramework::ApplicationPtr m_pApplication;
		SolidEdgeFramework::AddInPtr m_pAddIn;
		CString m_strResourceFilename;
		CString m_strCommandPrefix;
	};
}