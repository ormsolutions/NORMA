// Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/

///////////////////////////////////////////////////////////////////////////////
// Menu IDs          (0x01xx)
// IMPORTANT: keep constants in sync with ORMModel\Shell\ORMCommandSet.cs
#define menuIdContextMenu				0x0100
#define menuIdVerbalizationToolBar		0x0101
#define menuIdErrorList					0x0102
#define menuIdNewReadingEditorContextMenu 0x0103

///////////////////////////////////////////////////////////////////////////////
// Menu Group IDs    (0x10xx)
#ifdef DEBUG
#define groupIdDebugCommands			0x0FFF
#endif
#define groupIdDeleteContext			0x1000
#define groupIdFactCommands				0x1001
#define groupIdRoleCommands				0x1002
#define groupIdModelCommands			0x1003
#define groupIdConstraintEditContext	0x1004
#define groupIdVerbalizationCommands	0x1005
#define groupIdORMToolWindows			0x1006
#define groupIdErrorList				0x1007
#define groupIdErrorListTest			0x1008
#define groupIdNewReadingEditorContextAddDelete		0x1009
#define groupIdNewReadingEditorContextReadingPromoteDemote 0x1010
#define groupIdNewReadingEditorContextOrderPromoteDemote		0x10011

///////////////////////////////////////////////////////////////////////////////
// Command IDs       (0x2xxx)
// IMPORTANT: keep constants in sync with ORMModel\Shell\ORMCommandSet.cs
#ifdef DEBUG
#define cmdIdDebugViewStore				0x28FF
#endif
#define cmdIdViewModelBrowser			0x2900
#define cmdIdViewReadingEditor			0x2901
#define cmdIdViewReferenceModeEditor	0x2902
#define cmdIdInsertRoleAfter			0x2903
#define cmdIdInsertRoleBefore			0x2904
#define cmdIdViewFactEditor				0x2905

// Menu commands associated with editing the roles in the external constraint sequences.
#define cmdIdActivateRoleSequence		0x2906
#define cmdIdDeleteRoleSequence			0x2907
#define cmdIdEditExternalConstraint		0x2908
#define cmdIdMoveRoleSequenceUp			0x2909
#define cmdIdMoveRoleSequenceDown		0x290A

#define cmdIdCopyImage					0x290B
#define cmdIdViewVerbalizationBrowser	0x290C
#define cmdIdShowPositiveVerbalization	0x290D
#define cmdIdShowNegativeVerbalization	0x290E
#define cmdIdAutoLayout					0x290F

#define cmdIdToggleSimpleMandatory		0x2910
#define cmdIdAddInternalUniqueness		0x2911
#define cmdIdExtensionManager			0x2912

#define cmdIdViewNotesWindow			0x2913

#define cmdIdDeleteAlternate			0x2914
#define cmdIdMoveRoleLeft				0x2915
#define cmdIdMoveRoleRight				0x2916

#define cmdIdViewNewReadingEditor			0x2917

#define cmdIdReadingEditorAddReading   0x2918
#define cmdIdReadingEditorAddReadingOrder 0x2919
#define cmdIdReadingEditorDeleteReading  0x2920
#define cmdIdReadingEditorPromoteReading	 0x2921
#define cmdIdReadingEditorDemoteReading 0x2922
#define cmdIdReadingEditorPromoteReadingOrder 0x2923
#define cmdIdReadingEditorDemoteReadingOrder 0x2924

#define cmdIdErrorList					0x2a00
#define cmdIdErrorListEnd				0x2aff
// Reserve 2a00 range for errors, start additional commands after
///////////////////////////////////////////////////////////////////////////////
// Bitmap IDs for BitmapResourceStrip.bmp
#define bmpIdShowNegativeVerbalization		1
#define bmpIdShowPositiveVerbalization		2

#define bmpIdToolWindowVerbalizationBrowser	1
#define bmpIdToolWindowReadingEditor		2
#define bmpIdToolWindowReferenceModeEditor	3
#define bmpIdToolWindowFactEditor			4
#define bmpIdToolWindowModelBrowser			5
#define bmpIdToolWindowNotesEditor			6
