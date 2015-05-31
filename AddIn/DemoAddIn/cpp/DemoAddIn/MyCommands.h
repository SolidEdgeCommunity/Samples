#pragma once

#include "stdafx.h"
#include "Resource.h"

// Define the command IDs
enum MyCommandId
{
	idSave1,
	idSave2,
	idSave3,
	idFolder,
	idMonitor,
	idBox,
	idCamera,
	idPhotograph,
	idFavorites,
	idPrinter,
	idTools,
	idCommandPrompt,
	idNotepad,
	idHelp,
	idSearch,
	idQuestion,
	idCheckbox1,
	idCheckbox2,
	idCheckbox3,
	idRadiobutton1,
	idRadiobutton2,
	idRadiobutton3,
	idBoundingBox,
	idBoxes,
	idGdiPlus
};

typedef struct MY_COMMAND_INFO_
{
	UINT			iCategory;
	UINT			iGroup;
	UINT			iCommand;
	UINT			iString;
	UINT			iImage;
	SeButtonStyle	buttonStyle;
} MY_COMMAND_INFO, *PMY_COMMAND_INFO;

typedef struct MY_ENVIRONMENT_INFO_
{
	GUID				environmentGuid;
	MY_COMMAND_INFO*	pCommands;
	UINT				nCommands;
} MY_ENVIRONMENT_INFO, *PMY_ENVIRONMENT_INFO;

static const MY_COMMAND_INFO MyCommands3D[] =
{
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave1, IDS_CMD_SAVE1, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave2, IDS_CMD_SAVE2, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave3, IDS_CMD_SAVE3, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idFolder, IDS_CMD_FOLDER, IDB_FOLDER, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idMonitor, IDS_CMD_MONITOR, IDB_MONITOR, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idBox, IDS_CMD_BOX, IDB_BOX, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idCamera, IDS_CMD_CAMERA, IDB_CAMERA, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idPhotograph, IDS_CMD_PHOTOGRAPH, IDB_PHOTOGRAPH, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idFavorites, IDS_CMD_FAVORITES, IDB_FAVORITES, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idPrinter, IDS_CMD_PRINTER, IDB_PRINTER, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idTools, IDS_CMD_TOOLS, IDB_TOOLS, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idCommandPrompt, IDS_CMD_COMMAND_PROMPT, IDB_COMMAND_PROMPT, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idNotepad, IDS_CMD_NOTEPAD, IDB_NOTEPAD, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP4, idHelp, IDS_CMD_HELP, IDB_HELP, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP4, idSearch, IDS_CMD_SEARCH, IDB_SEARCH, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP5, idQuestion, IDS_CMD_QUESTION, IDB_QUESTION, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox1, IDS_CMD_CHECKBOX1, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox2, IDS_CMD_CHECKBOX2, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox3, IDS_CMD_CHECKBOX3, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton1, IDS_CMD_RADIOBUTTON_1, 0, seRadioButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton2, IDS_CMD_RADIOBUTTON_2, 0, seRadioButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton3, IDS_CMD_RADIOBUTTON_3, 0, seRadioButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP7, idBoundingBox, IDS_CMD_BOUNDING_BOX, IDB_BOUNDING_BOX, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP7, idBoxes, IDS_CMD_BOXES, IDB_BOXES, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP7, idGdiPlus, IDS_CMD_GDIPLUS, IDB_GDIPLUS, seButtonIconAndCaptionBelow }
};

static const MY_COMMAND_INFO MyCommands2D[] =
{
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave1, IDS_CMD_SAVE1, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave2, IDS_CMD_SAVE2, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idSave3, IDS_CMD_SAVE3, IDB_SAVE, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idFolder, IDS_CMD_FOLDER, IDB_FOLDER, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idMonitor, IDS_CMD_MONITOR, IDB_MONITOR, seButtonAutomatic },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP1, idBox, IDS_CMD_BOX, IDB_BOX, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idCamera, IDS_CMD_CAMERA, IDB_CAMERA, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idPhotograph, IDS_CMD_PHOTOGRAPH, IDB_PHOTOGRAPH, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idFavorites, IDS_CMD_FAVORITES, IDB_FAVORITES, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP2, idPrinter, IDS_CMD_PRINTER, IDB_PRINTER, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idTools, IDS_CMD_TOOLS, IDB_TOOLS, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idCommandPrompt, IDS_CMD_COMMAND_PROMPT, IDB_COMMAND_PROMPT, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP3, idNotepad, IDS_CMD_NOTEPAD, IDB_NOTEPAD, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP4, idHelp, IDS_CMD_HELP, IDB_HELP, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP4, idSearch, IDS_CMD_SEARCH, IDB_SEARCH, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP5, idQuestion, IDS_CMD_QUESTION, IDB_QUESTION, seButtonIconAndCaptionBelow },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox1, IDS_CMD_CHECKBOX1, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox2, IDS_CMD_CHECKBOX2, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idCheckbox3, IDS_CMD_CHECKBOX3, 0, seCheckButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton1, IDS_CMD_RADIOBUTTON_1, 0, seRadioButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton2, IDS_CMD_RADIOBUTTON_2, 0, seRadioButton },
	{ IDS_CMD_CATEGORY1, IDS_CMD_GROUP6, idRadiobutton3, IDS_CMD_RADIOBUTTON_3, 0, seRadioButton }
};


static const MY_ENVIRONMENT_INFO MyEnvironments[] =
{
	{ CATID_SEAssembly, (MY_COMMAND_INFO*)&MyCommands3D, _countof(MyCommands3D) },
	{ CATID_SEDraft, (MY_COMMAND_INFO*)&MyCommands2D, _countof(MyCommands2D) },
	{ CATID_SEPart, (MY_COMMAND_INFO*)&MyCommands3D, _countof(MyCommands3D) },
	{ CATID_SEDMPart, (MY_COMMAND_INFO*)&MyCommands3D, _countof(MyCommands3D) },
	{ CATID_SESheetMetal, (MY_COMMAND_INFO*)&MyCommands3D, _countof(MyCommands3D) },
	{ CATID_SEDMSheetMetal, (MY_COMMAND_INFO*)&MyCommands3D, _countof(MyCommands3D) }
};