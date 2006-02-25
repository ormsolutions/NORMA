#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object Role Modeling Architect for Visual Studio                 *
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

#endregion

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window for editing refence modes.
	/// </summary>
	[Guid("c2415d9f-dba8-49bc-8bf2-008f24f10559")]
	[CLSCompliant(false)]
	public class ORMReferenceModeEditorToolWindow : ToolWindow
	{
		private ReferenceModeViewForm myForm = new ReferenceModeViewForm();

		#region construction
		/// <summary>
		/// Default constructor 
		/// </summary>
		/// <param name="serviceProvder">Service provider</param>
		public ORMReferenceModeEditorToolWindow(IServiceProvider serviceProvder) : base(serviceProvder)
		{
			IMonitorSelectionService service = serviceProvder.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
			if (service != null)
			{
				service.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(service_DocumentWindowChanged);
			}
			ORMDesignerDocView docView = service.CurrentDocumentView as ORMDesignerDocView;
			LoadWindow(docView);
		}
		#endregion
		#region Accessor properties
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
		#region ToolWindow overrides
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
		/// Returns the localized Tital for the too window
		/// </summary>
		/// <value></value>
		public override string WindowTitle
		{
			get 
			{
				string editorWindowTitle = ResourceStrings.ModelReferenceModeEditorEditorWindowTitle;
				return editorWindowTitle;
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
		#region nested class ReadingsViewForm
		private class ReferenceModeViewForm : Form
		{
			private CustomReferenceModeEditor myRefModeEditor;

			#region construction
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

			#region methods
			public void SetModel(ORMModel model)
			{
				myRefModeEditor.SetModel(model);
			}
			#endregion

			#region Accessor Properties
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
		#region Event handlers
		void service_DocumentWindowChanged(object sender, MonitorSelectionEventArgs e)
		{
			ORMDesignerDocView docView = (sender as IMonitorSelectionService).CurrentDocumentView as ORMDesignerDocView;
			LoadWindow(docView);

		}
		private void LoadWindow(ORMDesignerDocView docView)
		{
			ORMModel model = null;
			if (docView != null)
			{
				model = docView.CurrentDiagram.ModelElement as ORMModel;
			}
			myForm.SetModel(model);
		}
		#endregion // Event handlers
	}
}
