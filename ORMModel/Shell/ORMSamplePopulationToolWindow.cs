#region Common Public License Copyright Notice
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
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Design;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// ToolWindow for hosting the Sample Population Editor
	/// </summary>
	[Guid("051209C1-250B-45a7-B7B1-8AFB50BEC9B7")]
	[CLSCompliant(false)]
	public class ORMSamplePopulationToolWindow : ORMToolWindow
	{
		private SamplePopulationEditor myEditor;

		#region Construction
		/// <summary>
		/// Construct a sample population tool window
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMSamplePopulationToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		/// <summary>
		/// Initialize here after we have the frame so we can grab the toolbar host
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			IVsToolWindowToolbarHost host = ToolBarHost;
			Debug.Assert(host != null); // Should be set with HasToolbar true
			if (host != null)
			{
				CommandID command = ORMDesignerDocView.ORMDesignerCommandIds.ViewSamplePopulationEditor;
				Guid commandGuid = command.Guid;
				host.AddToolbar(VSTWT_LOCATION.VSTWT_LEFT, ref commandGuid, (uint)command.ID);
			}
		}
		/// <summary>
		/// Make sure the toolbar flag gets set
		/// </summary>
		protected override bool HasToolBar
		{
			get
			{
				return true;
			}
		}
		#endregion // Construction
		#region ToolWindow Overrides
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapResource
		{
			get
			{
				return 125;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return -1;
			}
		}
		/// <summary>
		/// Gets the user control hosted in the tool window
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				SamplePopulationEditor editor = myEditor;
				if (editor == null)
				{
					myEditor = editor = new SamplePopulationEditor();
					ORMDesignerDocData currentDoc = CurrentDocument;
					if (currentDoc != null)
					{
						AttachEventHandlers(currentDoc.Store);
						OnORMSelectionContainerChanged();
					}
				}
				return myEditor;
			}
		}
		/// <summary>
		/// Clean up any existing objects
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (myEditor != null)
				{
					myEditor.Dispose();
				}
			}
		}
		#endregion // ToolWindow Overrides
		#region Selection Monitoring
		/// <summary>
		/// Update our reading to reflect the current selection
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			if (myEditor == null)
			{
				return;
			}
			ICollection selectedObjects;
			if (CurrentORMSelectionContainer != null && (selectedObjects = base.GetSelectedComponents()).Count == 1)
			{
				foreach (object selectedObject in selectedObjects)
				{
					ObjectType testObjectType = EditorUtility.ResolveContextInstance(selectedObject, false) as ObjectType;
					if (testObjectType != null)
					{
						if (testObjectType.IsValueType)
						{
							SelectedValueType = testObjectType;
						}
						else if (testObjectType != null)
						{
							SelectedEntityType = testObjectType;
						}
						else
						{
							NullSelection();
						}
					}
					else
					{
						FactType testFactType = EditorUtility.ResolveContextFactType(selectedObject) as FactType;
						if (testFactType != null)
						{
							SelectedFactType = testFactType;
						}
						else
						{
							NullSelection();
						}
					}
				}
			}
			else
			{
				NullSelection();
			}
		}
		#endregion // Selection Monitoring
		#region Properties
		/// <summary>
		/// Controls which ValueType is being displayed by the tool window.
		/// </summary>
		public ObjectType SelectedValueType
		{
			get
			{
				return myEditor.SelectedValueType;
			}
			set
			{
				myEditor.SelectedValueType = value;
			}
		}

		/// <summary>
		/// Controls which EntityType is being displayed by the tool window.
		/// </summary>
		public ObjectType SelectedEntityType
		{
			get
			{
				return myEditor.SelectedEntityType;
			}
			set
			{
				myEditor.SelectedEntityType = value;
			}
		}

		/// <summary>
		/// Controls which FactType is being displayed by the tool window.
		/// </summary>
		public FactType SelectedFactType
		{
			get
			{
				return myEditor.SelectedFactType;
			}
			set
			{
				myEditor.SelectedFactType = value;
			}
		}

		/// <summary>
		/// Nulls all properties for the embedded editor to force a blank window
		/// </summary>
		private void NullSelection()
		{
			Debug.Assert(myEditor != null, "Don't call before editor is initialized");
			myEditor.NullSelection();
		}
		#endregion // Properties
		#region ORMToolWindow Implementation
		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelSamplePopulationEditorWindowTitle;
			}
		}
		/// <summary>
		/// Wires event handlers to the store.
		/// </summary>
		protected override void AttachEventHandlers(Store store)
		{
			SamplePopulationEditor editor = myEditor;
			if (editor != null)
			{
				editor.AttachEventHandlers(store);
			}
		}
		/// <summary>
		/// Unwires event handlers from the store.
		/// </summary>
		protected override void DetachEventHandlers(Store store)
		{
			SamplePopulationEditor editor = myEditor;
			if (editor != null)
			{
				editor.DetachEventHandlers(store);
			}
		}
		#endregion // ORMToolWindow Implementation
	}
}
