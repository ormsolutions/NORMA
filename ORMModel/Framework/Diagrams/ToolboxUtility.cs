#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams
{
	/// <summary>
	/// Provides methods to support multiple <see cref="ShapeElement"/>s for the same <see cref="ModelElement"/>
	/// </summary>
	public static class ToolboxUtility
	{
		/// <summary>
		/// Activate a <see cref="MouseAction"/> for a given <see cref="DiagramClientView"/>
		/// </summary>
		/// <param name="action">The <see cref="MouseAction"/> to activate.</param>
		/// <param name="clientView">The current <see cref="DiagramClientView"/></param>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/></param>
		public static void ActivateMouseAction(MouseAction action, DiagramClientView clientView, IServiceProvider serviceProvider)
		{
			clientView.ActiveMouseAction = action;
#if !VISUALSTUDIO_10_0
			IVsUIShell shell;
			if (null != serviceProvider &&
				null != (shell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell))))
			{
				Guid windowGuid = StandardToolWindows.Toolbox;
				IVsWindowFrame frame;
				object frameMode;
				if (0 == shell.FindToolWindow((uint)(__VSFINDTOOLWIN.FTW_fFrameOnly), ref windowGuid, out frame) &&
					0 == frame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out frameMode))
				{
					switch ((VSFRAMEMODE)frameMode)
					{
						case VSFRAMEMODE.VSFM_FloatOnly:
						case VSFRAMEMODE.VSFM_Float:
							new ActionReactivator(action, clientView);
							break;
					}
				}
			}
#endif // !VISUALSTUDIO_10_0
		}
#if !VISUALSTUDIO_10_0
		/// <summary>
		/// Helper class to reactivate a mouse action if it is deactivated
		/// for a very short period of time, such as with a click
		/// </summary>
		private sealed class ActionReactivator
		{
			private MouseAction myAction;
			private DiagramClientView myClientView;
			private long myTicks;
			public ActionReactivator(MouseAction action, DiagramClientView clientView)
			{
				myAction = action;
				myClientView = clientView;
				clientView.LostFocus += new EventHandler(this.OnLostFocus);
			}
			private void OnLostFocus(object sender, EventArgs e)
			{
				Control control = (Control)sender;
				control.LostFocus -= this.OnLostFocus;
				myTicks = DateTime.Now.Ticks;
				control.GotFocus += this.OnGotFocus;
			}
			private void OnGotFocus(object sender, EventArgs e)
			{
				((Control)sender).GotFocus -= this.OnGotFocus;
				long ticks = DateTime.Now.Ticks;
				DiagramClientView clientView;
				MouseAction action;
				if (Math.Abs(myTicks - ticks) < 10000 &&
					!(clientView = myClientView).IsDisposed &&
					clientView.ActiveMouseAction == null &&
					!(action = myAction).IsActive)
				{
					clientView.ActiveMouseAction = myAction;
				}
			}
		}
#endif // !VISUALSTUDIO_10_0
	}
	#region ElementPrototypeToolboxAction class
	/// <summary>
	/// A replacement class for <see cref="ToolboxAction"/> that does not
	/// require a currently selected <see cref="ModelingToolboxItem"/> to
	/// function correctly. Use this in place of ToolboxAction and
	/// <see cref="ToolboxUtility.ActivateMouseAction"/> to activate a mouse
	/// action to enable toolbox actions to work with both a docked and
	/// floating toolbox window.
	/// </summary>
	/// <remarks>The justification for this class is that the click
	/// gesture with a selected toolbox item in a floating toolbox
	/// briefly defocuses the target window, which deactivates the
	/// mouse action and deselects the toolbox item. The mouse action
	/// can be reactivated</remarks>
	public class ElementPrototypeToolboxAction : SelectAction
	{
		private Cursor myCursor;
		private ElementGroupPrototype myPrototype;
		/// <summary>
		/// Create a new toolbox action. One instance of this class
		/// should be created per distinct toolbox item for each
		/// diagram instance.
		/// </summary>
		/// <param name="diagram">The current diagram.</param>
		/// <param name="prototype">The group prototype, retrieved from the
		/// <see cref="ModelingToolboxItem.Prototype"/> property.</param>
		public ElementPrototypeToolboxAction(Diagram diagram, ElementGroupPrototype prototype)
			: base(diagram)
		{
			myCursor = Cursors.No;
			myPrototype = prototype;
		}
		/// <summary>
		/// Always return the current cursor.
		/// </summary>
		public override Cursor GetCursor(Cursor currentCursor, DiagramClientView diagramClientView, PointD mousePosition)
		{
			return this.myCursor;
		}
		/// <summary>
		/// Handle the click by emulating a drag/drop operation
		/// </summary>
		protected override void OnClicked(MouseActionEventArgs e)
		{
			ElementGroupPrototype prototype;
			DiagramClientView clientView;
			Diagram diagram;
			DiagramView activeDiagramView;
			IToolboxService toolbox;
			if (null != (prototype = this.myPrototype) &&
				null != (clientView = e.DiagramClientView) &&
				null != (diagram = clientView.Diagram) &&
				null != (activeDiagramView = diagram.ActiveDiagramView) &&
				null != (toolbox = activeDiagramView.Toolbox))
			{
				IDataObject dataObject = new DataObject(typeof(ElementGroupPrototype).FullName, prototype);
				Point p = clientView.PointToScreen(clientView.WorldToDevice(e.CurrentMousePosition));
				clientView.OnDragDropCommon(new DragEventArgs(dataObject, 0, p.X, p.Y, DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll, DragDropEffects.None), true, base.MouseDownHitShape);
				toolbox.SelectedToolboxItemUsed();
			}
#if VISUALSTUDIO_10_0
			// Make sure we deactivate on click
			if (IsActive)
			{
				Cancel(e.DiagramClientView);
			}
#endif // VISUALSTUDIO_10_0
		}
		/// <summary>
		/// Deactivate the mouse action by deselecting the current toolbox item.
		/// </summary>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			base.OnMouseActionDeactivated(e);
			DiagramClientView clientView;
			Diagram diagram;
			DiagramView activeDiagramView;
			IToolboxService toolbox;
			if (null != (clientView = e.DiagramClientView) &&
				null != (diagram = clientView.Diagram) &&
				null != (activeDiagramView = diagram.ActiveDiagramView) &&
				null != (toolbox = activeDiagramView.Toolbox))
			{
				toolbox.SelectedToolboxItemUsed();
			}
		}
		/// <summary>
		/// Use the auto-generated cursor that looks like the currently
		/// selected toolbox item. Note that for reactivation cases,
		/// the reactivated mouse action (without the toolbox item) is
		/// only active for a very short time, so it doesn't matter
		/// that we can't get the toolbox cursort.
		/// </summary>
		protected override void OnMouseMove(DiagramMouseEventArgs e)
		{
			Cursor cursor = Cursors.No;
			ElementGroupPrototype prototype;
			DiagramClientView clientView;
			Diagram diagram;
			DiagramView activeDiagramView;
			IToolboxService toolbox;
			ShapeElement shape;
			if (null != (prototype = this.myPrototype) &&
				null != (clientView = e.DiagramClientView) &&
				null != (diagram = clientView.Diagram) &&
				null != (activeDiagramView = diagram.ActiveDiagramView) &&
				null != (toolbox = activeDiagramView.Toolbox) &&
				null != (shape = e.DiagramHitTestInfo.HitDiagramItem.Shape) &&
				diagram.ElementOperations.CanMergeElementGroupPrototype(shape, prototype))
			{
				toolbox.SetCursor();
				cursor = Cursor.Current;
			}
			this.myCursor = cursor;
		}
	}
	#endregion // ElementPrototypeToolboxAction class
}
