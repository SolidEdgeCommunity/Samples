// ExtrudedSurface.cpp : Defines the entry point for the console application.
//

// This project is configured to include path "C:\Program Files\Solid Edge ST4\Program".
// If you are getting build errors, you may have to update the path to your Solid Edge installation.
// Project -> Properties -> Configuration Properties -> VC++ Directories -> Library Directories
// Specifically, the #import directives in stdafx.h have to be able to resolve the .tlb(s).

#include "stdafx.h"

VOID DemoExtrudedSurfaces(SolidEdgeFramework::ApplicationPtr pApplication);

int _tmain(int argc, _TCHAR* argv[])
{
    HRESULT hr = S_OK;

    // Initialize COM.
	::CoInitialize(NULL);

	// Encapsulate COM smart pointers in separate code block.
	{
		SolidEdgeFramework::ApplicationPtr pApplication = NULL;

		// Attempt to connect to a running instance of Solid Edge.
		hr = pApplication.GetActiveObject(L"SolidEdge.Application");

		if (hr == MK_E_UNAVAILABLE)
		{
			// Solid Edge is not running. Start a new instance.
			hr = pApplication.CreateInstance(L"SolidEdge.Application");

			// Show the main window.
			pApplication->Visible = VARIANT_TRUE;
		}

		if (hr == S_OK)
		{
			DemoExtrudedSurfaces(pApplication);

			// Switch to ISO view.
			pApplication->StartCommand((SolidEdgeFramework::SolidEdgeCommandConstants)SolidEdgeConstants::PartViewISOView);
		}
	}

    // Uninitialize COM.
    ::CoUninitialize();

    return 0;
}

VOID DemoExtrudedSurfaces(SolidEdgeFramework::ApplicationPtr pApplication)
{
	HRESULT hr = S_OK;
    SolidEdgeFramework::DocumentsPtr pDocuments = NULL;
    SolidEdgePart::PartDocumentPtr pPartDocument = NULL;
	SolidEdgePart::RefPlanesPtr pRefPlanes = NULL;
	SolidEdgePart::RefPlanePtr pRefPlane = NULL;
	SolidEdgePart::ProfileSetsPtr pProfileSets = NULL;
	SolidEdgePart::ProfileSetPtr pProfileSet = NULL;
	SolidEdgePart::ProfilesPtr pProfiles = NULL;
	SolidEdgePart::ProfilePtr pProfile = NULL;
	SolidEdgeFrameworkSupport::Circles2dPtr pCircles2d = NULL;
	SolidEdgeFrameworkSupport::Circle2dPtr pCircle2d = NULL;
	SolidEdgePart::ConstructionsPtr pConstructions = NULL;
	SolidEdgePart::ExtrudedSurfacesPtr pExtrudedSurfaces = NULL;
	SolidEdgePart::ExtrudedSurfacePtr pExtrudedSurface = NULL;
	SAFEARRAY *pProfileArray = NULL;

    // Get a reference to the Documents collection.
    pDocuments = pApplication->Documents;

    ::wprintf(L"Creating a new 'SolidEdge.PartDocument'.  No template specified.\n");
    pPartDocument = pDocuments->Add(L"SolidEdge.PartDocument");

    pApplication->DoIdle();

	// Get a reference to the RefPlanes collection.
	pRefPlanes = pPartDocument->RefPlanes;

	// Get a reference to the Front (xz) plane.
	pRefPlane = pRefPlanes->Item(3L);

	// Get a reference to the ProfileSets collection.
	pProfileSets = pPartDocument->ProfileSets;

	// Add a new ProfileSet.
	pProfileSet = pProfileSets->Add();

	// Get a reference to the Profiles collection.
	pProfiles = pProfileSet->Profiles;

	// Add a new Profile.
	pProfile = pProfiles->Add(pRefPlane);

	// Get a reference to the Circles2d collection.
	pCircles2d = pProfile->Circles2d;

	// Add a new Circle2d.
	pCircle2d = pCircles2d->AddByCenterRadius(
		0.04, // x
		0.05, // y
		0.02  // Radius
		); 

	// Close the profile and check state.
	if (pProfile->End(SolidEdgePart::igProfileClosed) != 0)
	{
		::wprintf(L"Profile validation failed.\n");
		goto Error;
	}

	// Get a reference to the Constructions collection.
	pConstructions = pPartDocument->Constructions;

	// Get a reference to the ExtrudedSurfaces collection.
	pExtrudedSurfaces = pConstructions->ExtrudedSurfaces;

	// Allocate profiles SAFEARRAY.
	pProfileArray = SafeArrayCreateVector(VT_DISPATCH, 0, pProfiles->Count);

	// Populate profiles SAFEARRAY.
	for (long index = 0; index < pProfiles->Count; index++)
	{
		IfFailGo(SafeArrayPutElement(pProfileArray, &index, pProfiles->Item(index + 1)));
	}

	// These parameter variables are declared because we have to pass them as pointers.
	SolidEdgePart::KeyPointExtentConstants KeyPointFlags1 = SolidEdgePart::igTangentNormal;
	SolidEdgePart::KeyPointExtentConstants KeyPointFlags2 = SolidEdgePart::igTangentNormal;

	// Add a new ExtrudedSurface.
	pExtrudedSurface = pExtrudedSurfaces->Add(
		pProfiles->Count, // NumberOfProfiles
		&pProfileArray, // ProfileArray
		SolidEdgePart::igFinite, // ExtentType1
		SolidEdgePart::igRight, // ExtentSide1
		0.0127, // FiniteDepth1
		NULL, // KeyPointOrTangentFace1
		&KeyPointFlags1, // KeyPointFlags1
		NULL, // FromFaceOrRefPlane
		SolidEdgePart::seOffsetNone, // FromFaceOffsetSide
		0, // FromFaceOffsetDistance
		SolidEdgePart::seTreatmentCrown, // TreatmentType1
		SolidEdgePart::seDraftInside, // TreatmentDraftSide1
		0.1, // TreatmentDraftAngle1
		SolidEdgePart::seTreatmentCrownByOffset, // TreatmentCrownType1
		SolidEdgePart::seTreatmentCrownSideInside, // TreatmentCrownSide1
		SolidEdgePart::seTreatmentCrownCurvatureInside, // TreatmentCrownCurvatureSide1
		0.003, // TreatmentCrownRadiusOrOffset1
		0, // TreatmentCrownTakeOffAngle1
		SolidEdgePart::igFinite, // ExtentType2
		SolidEdgePart::igLeft, // ExtentSide2
		0.0127, // FiniteDepth2
		NULL, // KeyPointOrTangentFace2
		&KeyPointFlags2, // KeyPointFlags2
		NULL, // ToFaceOrRefPlane
		SolidEdgePart::seOffsetNone, // ToFaceOffsetSide
		0, // ToFaceOffsetDistance
		SolidEdgePart::seTreatmentCrown, // TreatmentType2
		SolidEdgePart::seDraftNone, // TreatmentDraftSide2
		0, // TreatmentDraftAngle2
		SolidEdgePart::seTreatmentCrownByOffset, // TreatmentCrownType2
		SolidEdgePart::seTreatmentCrownSideInside, // TreatmentCrownSide2
		SolidEdgePart::seTreatmentCrownCurvatureInside, // TreatmentCrownCurvatureSide2
		0.003, // TreatmentCrownRadiusOrOffset2
		0, // TreatmentCrownTakeOffAngle2
		VARIANT_TRUE // WantEndCaps
		);

	SolidEdgePart::FeaturePropertyConstants ExtentType;
	SolidEdgePart::FeaturePropertyConstants ExtentSide;
	double FiniteDepth = 0.0;
	SolidEdgePart::KeyPointExtentConstants KeyPointFlags = SolidEdgePart::igTangentNormal;

	// Get extent information for the first direction.
	IfFailGo(pExtrudedSurface->GetDirection1Extent(&ExtentType, &ExtentSide, &FiniteDepth));

	// Modify parameters.
	FiniteDepth = 2.0;
	ExtentType = SolidEdgePart::igFinite;
	ExtentSide = SolidEdgePart::igRight;

	// Apply extent information for the first direction.
	pExtrudedSurface->ApplyDirection1Extent(
		ExtentType,
		ExtentSide,
		FiniteDepth,
		NULL,
		&KeyPointFlags);

Error:
	// Destroy SAFEARRAY.
	if (pProfileArray) SafeArrayDestroy(pProfileArray);

	// Release smart pointers.
    pExtrudedSurface = NULL;
    pExtrudedSurfaces = NULL;
	pConstructions = NULL;
	pCircle2d = NULL;
	pCircles2d = NULL;
	pProfile = NULL;
	pProfiles = NULL;
	pProfileSet = NULL;
	pProfileSets = NULL;
	pRefPlane = NULL;
	pRefPlanes = NULL;
	pPartDocument = NULL;
	pDocuments = NULL;
}

