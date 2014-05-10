// dllmain.h : Declaration of module class.

// NOTE: See ReadMe.txt for important details.

class CMinimalAddInModule : public ATL::CAtlDllModuleT< CMinimalAddInModule >
{
public :
    DECLARE_LIBID(LIBID_MinimalAddInLib)
    DECLARE_REGISTRY_APPID_RESOURCEID(IDR_MINIMALADDIN, "{221A24FD-ABFA-4FF4-8AC7-E9BBA6C2C111}")

    CMinimalAddInModule()
    {
        m_hInstance = NULL;
    }

    inline HINSTANCE GetResourceInstance()
    {
        return m_hInstance;
    }

    inline void SetResourceInstance(HINSTANCE hInstance)
    {
        m_hInstance = hInstance;
    }

private:
    HINSTANCE m_hInstance;
};

extern class CMinimalAddInModule _AtlModule;
