// ReportVariables.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

SolidEdgeConstants::ObjectType GetObjectType(IDispatchPtr pDispatch);

int _tmain(int argc, _TCHAR* argv[])
{
	HRESULT hr = S_OK;

	// Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SolidEdgeFramework::ApplicationPtr pApplication = NULL;
		SolidEdgeFramework::SolidEdgeDocumentPtr pDocument = NULL;
		SolidEdgeFramework::VariablesPtr pVariables = NULL;
		SolidEdgeFramework::VariableListPtr pVariableList = NULL;
		SolidEdgeFramework::variablePtr pVariable = NULL;
		SolidEdgeFrameworkSupport::DimensionPtr pDimension = NULL;

		// Attempt to connect to a running instance of Solid Edge.
		IfFailGo(pApplication.GetActiveObject(L"SolidEdge.Application"));

		// Get a reference to the active document.
		pDocument = pApplication->ActiveDocument;

		if (pDocument != NULL)
		{
			// Get a reference to the Variables.
			pVariables = pDocument->Variables;

			// Get a reference to the VariableList.
			pVariableList = pVariables->Query(L"*", (LONG)SolidEdgeConstants::seVariableNameByBoth, (LONG)SolidEdgeConstants::SeVariableVarTypeBoth);

			// Process variables.
			for (LONG i = 1; i <= pVariableList->Count; i++)
			{
				IDispatchPtr pVariableListItem = pVariableList->Item(i);

				// Use helper method to get the object type.
				SolidEdgeConstants::ObjectType objectType = GetObjectType(pVariableListItem);

				// Process the specific variable item type.
				switch (objectType)
				{
				case SolidEdgeConstants::ObjectType::igDimension:
					// Get a reference to the dimension.
					pDimension = pVariableListItem;
					wprintf(L"Dimension: '%s' = '%f'\n", pDimension->DisplayName.GetBSTR(), pDimension->Value);
					break;
				case SolidEdgeConstants::ObjectType::igVariable:
					// Get a reference to the variable.
					pVariable = pVariableList->Item(i);
					wprintf(L"Variable: '%s' = '%f'\n", pVariable->DisplayName.GetBSTR(), pVariable->Value);
					break;
				}
			}
		}
		else
		{
			wprintf(L"No active document.\n");
		}
	}

Error:

	// Uninitialize COM.
	::CoUninitialize();

	return 0;
}

SolidEdgeConstants::ObjectType GetObjectType(IDispatchPtr pDispatch)
{
	HRESULT hr = S_OK;

	if (pDispatch != NULL)
	{
		DISPID rgDispId = 0;
		OLECHAR *Names[1] = { L"Type" };
		VARIANT varResult;
		VariantInit(&varResult);
		V_VT(&varResult) = VT_I4;
		DISPPARAMS disp = { NULL, NULL, 0, 0 };

		// Get the DISPID of the 'Type' property.
		IfFailGo(pDispatch->GetIDsOfNames(IID_NULL, Names, 1, LOCALE_USER_DEFAULT, &rgDispId));

		// Invoke the 'Type' property.
		IfFailGo(pDispatch->Invoke(rgDispId, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_PROPERTYGET, &disp, &varResult, NULL, NULL));

		return (SolidEdgeConstants::ObjectType)(V_I4(&varResult));		
	}

Error:
	return (SolidEdgeConstants::ObjectType)0;
}