//------------------------------------------------------------------------------
/// <copyright from='1997' to='2003' company='Microsoft Corporation'>           
/// Copyright (c) Microsoft Corporation. All Rights Reserved.                
/// Information Contained Herein is Proprietary and Confidential.            
/// </copyright>                                                                
//------------------------------------------------------------------------------

///////////////////////////////////////////////////////////////////////////////
// Menu IDs          (0x01xx)
// IMPORTANT: keep constants in sync with ORMModel\Shell\ORMCommandSet.cs
#define menuIdContextMenu				0x0100

///////////////////////////////////////////////////////////////////////////////
// Menu Group IDs    (0x10xx)
#ifdef DEBUG
#define groupIdDebugCommands			0x0FFF
#endif
#define groupIdDeleteContext			0x1000

///////////////////////////////////////////////////////////////////////////////
// Command IDs       (0x2xxx)
// IMPORTANT: keep constants in sync with ORMModel\Shell\ORMCommandSet.cs
#ifdef DEBUG
#define cmdIdDebugViewStore				0x28FF
#endif
#define cmdIdViewModelExplorer			0x2900
///////////////////////////////////////////////////////////////////////////////
// Bitmap IDs