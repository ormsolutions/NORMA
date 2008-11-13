#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.Modeling.Shell
{
	/// <summary>
	/// A <see cref="DiagramView"/> designed for hosting in a <see cref="ToolWindow"/>.
	/// Use in place of <see cref="VSDiagramView"/>, which is designed for hosting
	/// in a <see cref="DiagramDocView"/>
	/// </summary>
	[CLSCompliant(false)]
	public class ToolWindowDiagramView : DiagramView
	{
		#region DiagramViewSite class
		private sealed class DiagramViewSite : ISite, IServiceProvider
		{
			#region Fields and constructor
			private IServiceProvider myServiceProvider;
			public DiagramViewSite(IServiceProvider serviceProvider)
			{
				myServiceProvider = serviceProvider;
			}
			#endregion // Fields and constructor
			#region IServiceProvider implementation
			object IServiceProvider.GetService(Type serviceType)
			{
				return myServiceProvider.GetService(serviceType);
			}
			#endregion // IServiceProvider implementation
			#region ISite implementation
			IComponent ISite.Component
			{
				[DebuggerStepThrough]
				get
				{
					return null;
				}
			}
			IContainer ISite.Container
			{
				[DebuggerStepThrough]
				get
				{
					return null;
				}
			}
			bool ISite.DesignMode
			{
				[DebuggerStepThrough]
				get
				{
					return false;
				}
			}
			string ISite.Name
			{
				[DebuggerStepThrough]
				get
				{
					return string.Empty;
				}
				[DebuggerStepThrough]
				set
				{
				}
			}
			#endregion // ISite implementation
		}
		#endregion // DiagramViewSite class
		#region Member Variables
		private ToolWindow myHostWindow;
		private ISelectionService mySelectionService;
		#endregion // Member Variables
		#region Event Definitions
		/// <summary>
		/// The event that fires when the context menu is requested
		/// </summary>
		public event EventHandler<DiagramMouseEventArgs> ContextMenuRequestedEvent;
		#endregion // Event Definitions
		#region Constructor
		/// <summary>
		/// Create a new <see cref="ToolWindowDiagramView"/> for the specific <see cref="ToolWindow"/>
		/// </summary>
		/// <remarks>The control is returned sited to the <see cref="IServiceProvider"/> of the ToolWindow</remarks>
		public ToolWindowDiagramView(ToolWindow hostWindow)
		{
			myHostWindow = hostWindow;
			base.DiagramClientView.DiagramAssociated += new EventHandler(this.OnDiagramAssociated);
			base.DiagramClientView.DiagramDisassociating += new EventHandler(this.OnDiagramDisassociating);
			base.DiagramClientView.ContextMenuRequested += new ContextMenuRequestedEventHandler(this.OnClientViewContextMenuRequested);
			base.DiagramClientView.DragDrop += new DragEventHandler(this.OnClientViewDragDrop);
			base.DiagramClientView.DragLeave += new EventHandler(this.OnClientViewDragLeave);
			InPlaceTextEditor.Instance.BeginEdit += new DiagramItemEventHandler(this.OnBeginInPlaceEdit);
			InPlaceTextEditor.Instance.EndEdit += new DiagramItemEventHandler(this.OnEndInPlaceEdit);
			Site = new DiagramViewSite(hostWindow);
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Get the watermark font from the current service provider
		/// </summary>
		protected override void OnWatermarkCreated()
		{
			base.OnWatermarkCreated();
			DiagramWatermark watermark = base.Watermark;
			Font font = null;
			ISite site = this.Site;
			if (site != null)
			{
				IUIService service = (IUIService)site.GetService(typeof(IUIService));
				if (service != null)
				{
					font = service.Styles["DialogFont"] as Font;
				}
			}
			watermark.Font = font ?? SystemFonts.IconTitleFont;
			watermark.ForeColor = SystemColors.GrayText;
		}
		#endregion // Base overrides
		#region Helper properties
		/// <summary>
		/// Get the <see cref="ISelectionService"/>
		/// </summary>
		protected ISelectionService SelectionService
		{
			[DebuggerStepThrough]
			get
			{
				ISelectionService retVal = mySelectionService;
				if (retVal == null)
				{
					ISite site = this.Site;
					if (site != null)
					{
						mySelectionService = retVal = (ISelectionService)site.GetService(typeof(ISelectionService));
					}
				}
				return retVal;
			}
		}
		#endregion // Helper properties
		#region Event handlers
		private void OnShapeSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService service = SelectionService;
			if (service != null)
			{
				service.SetSelectedComponents(Selection.RepresentedElements);
			}
		}
		private void OnDiagramAssociated(object sender, EventArgs e)
		{
			Selection.ShapeSelectionChanged += new ShapeSelectionChangedEventHandler(OnShapeSelectionChanged);
		}
		private void OnDiagramDisassociating(object sender, EventArgs e)
		{
			Selection.ShapeSelectionChanged -= new ShapeSelectionChangedEventHandler(OnShapeSelectionChanged);
		}
		private void OnClientViewDragDrop(object sender, DragEventArgs e)
		{
			if (Diagram != null)
			{
				myHostWindow.Show();
				DiagramClientView.Focus();
			}
			IToolboxService toolboxService = Toolbox;
			if (toolboxService != null)
			{
				toolboxService.SelectedToolboxItemUsed();
			}
		}
		private void OnClientViewDragLeave(object sender, EventArgs e)
		{
			DiagramClientView clientView;
			if (Diagram != null &&
				(clientView = DiagramClientView).ClientRectangle.Contains(clientView.PointToClient(Control.MousePosition)))
			{
				myHostWindow.Show();
				clientView.Focus();
			}
		}
		private void OnClientViewContextMenuRequested(object sender, MouseEventArgs e)
		{
			EventHandler<DiagramMouseEventArgs> contextMenuRequest;
			if (null != (contextMenuRequest = ContextMenuRequestedEvent))
			{
				contextMenuRequest(this, new DiagramMouseEventArgs(e, DiagramClientView));
			}
		}
		private void OnBeginInPlaceEdit(object sender, DiagramItemEventArgs e)
		{
			InPlaceTextEditor editor = sender as InPlaceTextEditor;
			myHostWindow.ActiveInPlaceEditWindow = (editor != null) ? editor.EditorControl : null;
		}
		private void OnEndInPlaceEdit(object sender, DiagramItemEventArgs e)
		{
			myHostWindow.ActiveInPlaceEditWindow = null;
		}
		#endregion // Event handlers
	}
}
