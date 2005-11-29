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
