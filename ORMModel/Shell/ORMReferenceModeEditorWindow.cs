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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window for editing refence modes.
	/// </summary>
	[Guid("C2415D9F-DBA8-49BC-8BF2-008F24F10559")]
	[CLSCompliant(false)]
	public partial class ORMReferenceModeEditorToolWindow : ORMToolWindow
	{
		private ReferenceModeViewForm myForm;

		#region Construction
		/// <summary>
		/// Default constructor 
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMReferenceModeEditorToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#endregion
		#region TreeControl Property
		/// <summary>
		/// Get the tree control for this editor
		/// </summary>
		public VirtualTreeControl TreeControl
		{
			get
			{
				ReferenceModeViewForm form = myForm;
				return (form != null) ? form.TreeControl : null;
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
				return PackageResources.Id.ToolWindowIcons;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return PackageResources.ToolWindowIconIndex.ReferenceModeEditor;
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
				ReferenceModeViewForm form = myForm;
				if (form == null)
				{
					myForm = form = new ReferenceModeViewForm(this.ServiceProvider);
				}
				LoadWindow();
				return form;
			}
		}
		#endregion // ToolWindow overrides
		#region Nested Class ReadingsViewForm
		private sealed partial class ReferenceModeViewForm : ContainerControl
		{
			private CustomReferenceModeEditor myRefModeEditor;

			#region Construction
			public ReferenceModeViewForm(IServiceProvider serviceProvider)
			{
				Initialize(serviceProvider);
			}

			private void Initialize(IServiceProvider serviceProvider)
			{
				myRefModeEditor = new CustomReferenceModeEditor(serviceProvider);
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
			public VirtualTreeControl TreeControl
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
			ReferenceModeViewForm form = myForm;
			if (form == null)
			{
				return;
			}
			ORMDesignerDocData docData = CurrentDocument;
			Store store;
			ReadOnlyCollection<ORMModel> models;
			if (docData != null &&
				null != (store = docData.Store) &&
				0 != (models = store.ElementDirectory.FindElements<ORMModel>()).Count)
			{
				form.SetModel(models[0]);
			}
			else
			{
				form.SetModel(null);
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
		/// Update the current window when the document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
		{
			LoadWindow();
		}
		/// <summary>
		/// NOT IMPLEMENTED: There are no event handlers to attach for this class.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action) { }
		#endregion
	}
}
