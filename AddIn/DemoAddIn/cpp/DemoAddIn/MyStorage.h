#pragma once

#include "stdafx.h"
#include "Resource.h"

typedef struct MY_STORAGE_INFO_
{
	DocumentStatus	lastSavedStatus;
	bool			randomFlag;
	DocumentTypeConstants documentType;
} MY_STORAGE_INFO, *PMY_STORAGE_INFO;

void SaveAddInStorageInfo(LPDISPATCH theDocument)
{
	SolidEdgeFramework::SolidEdgeDocumentPtr pDocument = NULL;
	IStoragePtr pStorage = NULL;
	IStreamPtr pStream = NULL;	

	pDocument = theDocument;

	MY_STORAGE_INFO storageInfo;
	memset(&storageInfo, 0, sizeof(storageInfo));
	storageInfo.lastSavedStatus = pDocument->Status;
	storageInfo.randomFlag = true;
	storageInfo.documentType = pDocument->Type;

	// Connect to the addin storage specifying the storage name.
	pStorage = pDocument->GetAddInsStorage(L"DemoAddIn", 0);

	// Create a new stream in the storage.
	pStorage->CreateStream(L"Info", STGM_DIRECT | STGM_CREATE | STGM_READWRITE | STGM_SHARE_EXCLUSIVE, 0, 0, &pStream);

	ULONG pcbWritten = 0;
	pStream->Write(&storageInfo, sizeof(MY_STORAGE_INFO), &pcbWritten);

	pStream->Commit(STGC_DEFAULT | STGC_OVERWRITE);

	pStream = NULL;
	pStorage = NULL;
	pDocument = NULL;
}

HRESULT ReadAddInStorageInfo(LPDISPATCH theDocument, MY_STORAGE_INFO* storageInfo)
{
	HRESULT hr = S_OK;
	SolidEdgeFramework::SolidEdgeDocumentPtr pDocument = NULL;
	IStoragePtr pStorage = NULL;
	IStreamPtr pStream = NULL;

	pDocument = theDocument;

	// Connect to the addin storage specifying the storage name.
	pStorage = pDocument->GetAddInsStorage(L"DemoAddIn", 0);

	// Attempt to open the stream.
	hr = pStorage->OpenStream(L"Info", NULL, STGM_DIRECT | STGM_READ | STGM_SHARE_EXCLUSIVE, 0, &pStream);

	if (hr == S_OK)
	{
		ULONG pcbRead = 0;
		hr = pStream->Read(storageInfo, sizeof(MY_STORAGE_INFO), &pcbRead);
	}

	pStream = NULL;
	pStorage = NULL;
	pDocument = NULL;

	return hr;
}