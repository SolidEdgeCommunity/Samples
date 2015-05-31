#include "StdAfx.h"
#include "SolidEdgeCommunity.h"

namespace SolidEdgeCommunity
{
	HRESULT RegisterSolidEdgeAddIn(CLSID clsid, LPCTSTR description, LPCTSTR summary, ULONG cEnvironments, CATID environments[])
	{
		LCID lcid = GetSystemDefaultLCID();
		return RegisterSolidEdgeAddIn(clsid, lcid, description, summary, cEnvironments, environments);
	}

	HRESULT RegisterSolidEdgeAddIn(CLSID clsid, LCID lcid, LPCTSTR name, LPCTSTR summary, ULONG cEnvironments, CATID environments[])
	{
		HRESULT hr = S_OK;

#pragma region "HKEY_CLASSES_ROOT\\CLSID\\{ADDIN_GUID}"

		LPOLESTR lpClsidString;
		StringFromCLSID(clsid, &lpClsidString);

		CString strKeyCLSID;
		strKeyCLSID.AppendFormat(L"CLSID\\%s", lpClsidString);

		CoTaskMemFree(lpClsidString);

		CString strValueName;
		strValueName.AppendFormat(L"%x", lcid);

		CRegKey key;

		// Create\Open the key.
		LONG result = key.Create(HKEY_CLASSES_ROOT, strKeyCLSID);

		if (result == ERROR_SUCCESS)
		{
			// Set the localized addin name.
			key.SetStringValue(strValueName, name);
			key.Close();
		}

#pragma endregion

#pragma region "HKEY_CLASSES_ROOT\\CLSID\\{ADDIN_GUID}\\Summary"

		CString strKeySummary;
		strKeySummary = strKeyCLSID;
		strKeySummary.Append(L"\\Summary");

		result = key.Create(HKEY_CLASSES_ROOT, strKeySummary);

		if (result == ERROR_SUCCESS)
		{
			key.SetStringValue(strValueName, summary);
			key.Close();
		}

#pragma endregion

#pragma region "HKEY_CLASSES_ROOT\\CLSID\\{ADDIN_GUID}\\Environment Categories"

		CString strKeyEnvironmentCategories;
		strKeyEnvironmentCategories = strKeyCLSID;
		strKeyEnvironmentCategories.Append(L"\\Environment Categories");

		result = key.Create(HKEY_CLASSES_ROOT, strKeyEnvironmentCategories);

		if (result == ERROR_SUCCESS)
		{
			for (ULONG i = 0; i < cEnvironments; i++)
			{
				LPOLESTR lpCatidString = NULL;
				StringFromCLSID(environments[i], &lpCatidString);

				CString strEnvCATID(lpCatidString);
				CRegKey keyEnvironment;
				keyEnvironment.Create(key, strEnvCATID);

				CoTaskMemFree(lpCatidString);
			}

			key.Close();
		}

#pragma endregion

#pragma region "HKEY_CLASSES_ROOT\\CLSID\\{ADDIN_GUID}\\Implemented Categories"

		ICatRegisterPtr pCatRegister;
		IfFailGoCheck(pCatRegister.CreateInstance(CLSID_StdComponentCategoriesMgr), pCatRegister);

		CATID implementedCategories[] = { CATID_SolidEdgeAddIn };
		IfFailGo(pCatRegister->RegisterClassImplCategories(clsid, _countof(implementedCategories), implementedCategories));

#pragma endregion

	Error:
		pCatRegister = NULL;
		return hr;
	}

	void UnRegisterSolidEdgeAddIn(CLSID clsid)
	{
		LPOLESTR lpClsidString;
		StringFromCLSID(clsid, &lpClsidString);

		CString strKeyCLSID = lpClsidString;

		CRegKey key;
		key.Open(HKEY_CLASSES_ROOT, L"CLSID");
		key.RecurseDeleteKey(strKeyCLSID);
		key.Close();

		CoTaskMemFree(lpClsidString);
	}
}