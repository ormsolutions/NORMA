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

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Editors;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

#endregion

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window for editing refence modes.
	/// </summary>
	[Guid("c2415d9f-dba8-49bc-8bf2-008f24f10559")]
	[CLSCompliant(false)]
	public class ORMReferenceModeEditorToolWindow : ORMToolWindow
	{
		private ReferenceModeViewForm myForm = new ReferenceModeViewForm();

		#region Construction
		/// <summary>
		/// Default constructor 
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMReferenceModeEditorToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			LoadWindow();
		}
		#endregion
		#region TreeControl Property
		/// <summary>
		/// Get the tree control for this editor
		/// </summary>
		public Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl TreeControl
		{
			get
			{
				return myForm.TreeControl;
			}
		}
		#endregion // Accessor properties
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
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return 2;
			}
		}
		/// <summary>
		/// Provides and interface to expose Win32 handles
		/// </summary>
		/// <value></value>
		public override IWin32Window Window
		{
			get
			{
				return myForm;
			}
		}
		#endregion // ToolWindow overrides
		#region Nested Class ReadingsViewForm
		private class ReferenceModeViewForm : Form
		{
			private CustomReferenceModeEditor myRefModeEditor;

			#region Construction
			public ReferenceModeViewForm()
			{
				Initialize();
			}

			private void Initialize()
			{
				myRefModeEditor = new CustomReferenceModeEditor();
				this.Controls.Add(myRefModeEditor);
				myRefModeEditor.Dock = DockStyle.Fill;
			}
			#endregion

			#region SetModel
			public void SetModel(ORMModel model)
			{
				myRefModeEditor.SetModel(model);
			}
			#endregion

			#region TreeControl Property
			/// <summary>
			/// Get the tree control for this editor
			/// </summary>
			public Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl TreeControl
			{
				get
				{
					return myRefModeEditor.TreeControl;
				}
			}
			#endregion // Accessor Properties
		}
		#endregion
		#region LoadWindow Method
		private void LoadWindow()
		{
			ORMModel model = null;
			ORMDesignerDocData docData = myCurrentDocument;
			if (docData != null)
			{
				ORMDesignerDocView docView = docData.DocViews[0] as ORMDesignerDocView;
				if (docView != null)
				{
					Diagram diagram = docView.CurrentDiagram;
					if (diagram != null)
					{
						model = diagram.ModelElement as ORMModel;
					}
				}
				myForm.SetModel(model);
			}
		}
		#endregion // LoadWindow Method
		#region ORMToolWindow Implementation
		/// <summary>
		/// Returns the localized title of the Reference Mode Editor.
		/// </summary>
		public override string WindowTitle
		{
			get { return ResourceStrings.ModelReferenceModeEditorEditorWindowTitle; }
		}
		/// <summary>
		/// Handles the IMonitorSelectionService DocumentWindowChanged event.
		/// </summary>
		protected override void DocumentWindowChanged(object sender, MonitorSelectionEventArgs e)
		{
			base.DocumentWindowChanged(sender, e);
			LoadWindow();
		}
		/// <summary>
		/// NOT IMPLEMENTED: There are no event handlers to attach for this class.
		/// </summary>
		protected override void AttachEventHandlers(Store store) { }
		/// <summary>
		/// NOT IMPLEMENTED: There are no event handlers to detach for this class.
		/// </summary>
		protected override void DetachEventHandlers(Store store) { }
		#endregion
	}
}
