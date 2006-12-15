using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.Modeling;
using System.Collections;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window to contain survey tree control
	/// </summary>
	[CLSCompliant(false)]
	public class NewORMModelBrowser : ORMToolWindow, IORMSelectionContainer
	{
		private SurveyTreeControl myTreeControl;
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="serviceProvider"></param>
		public NewORMModelBrowser(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#region LoadWindow method

		/// <summary>
		/// Loads the SurveyTreeControl from the current document
		/// </summary>
		protected void LoadWindow()
		{
			SurveyTreeControl treeControl = myTreeControl;
			if (treeControl == null)
			{
				myTreeControl = treeControl = new SurveyTreeControl();
			}
			
			ORMDesignerDocData currentDocument = this.CurrentDocument;
			treeControl.Tree = (currentDocument != null) ? currentDocument.SurveyTree : null;
			treeControl.TreeControl.SelectionChanged += new EventHandler(Tree_SelectionChanged);
		}

		private void Tree_SelectionChanged(object sender, EventArgs e)
		{
			ICollection newSelection = null;
			VirtualTreeControl treeControl = myTreeControl.TreeControl;
			int currentIndex = treeControl.CurrentIndex;
			if (currentIndex >= 0)
			{
				VirtualTreeItemInfo info = treeControl.Tree.GetItemInfo(currentIndex, 0, false);
				int options = 0;
				object trackingObject = info.Branch.GetObject(info.Row, 0, ObjectStyle.TrackingObject, ref options);
				if (trackingObject != null)
				{
					newSelection = new object[] { trackingObject };
				}
			}
			SetSelectedComponents(newSelection);
		}
		#endregion //LoadWindow method
		#region ORMToolWindow overrides
		///// <summary>
		///// currently unimplemented, all events handled by tree directly
		///// </summary>
		/// <summary>
		/// Attaches custom <see cref="EventHandler{TEventArgs}"/>s to the <see cref="Store"/>.  This method must be overridden.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected override void ManageEventHandlers(Microsoft.VisualStudio.Modeling.Store store, Neumont.Tools.Modeling.ModelingEventManager eventManager, Neumont.Tools.Modeling.EventHandlerAction action)
		{
			// Track Currently Executing Events
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent), action);
		}

		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			ITree tree = this.myTreeControl.Tree;
			if (tree != null)
			{
				tree.DelayRedraw = true;
			}
		}

		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			ITree tree = this.myTreeControl.Tree;
			if (tree != null)
			{
				tree.DelayRedraw = false;
			}
		}

		/// <summary>
		/// called when document current selected document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
		{
			base.OnCurrentDocumentChanged();
			LoadWindow();
		}
		/// <summary>
		/// returns string to be displayed as window title
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelBrowserWindowTitle;
			}
		}
		/// <summary>
		/// returns int value for window icon
		/// </summary>
		protected override int BitmapResource //TODO: find correct bitmap resource to use
		{
			get
			{
				return 125;
			}
		}
		/// <summary>
		/// returns index for window icon
		/// </summary>
		protected override int BitmapIndex //TODO: find correct bitmap index to use
		{
			get
			{
				return 4;
			}
		}
		/// <summary>
		/// retuns the SurveyTreeControl this window contains
		/// </summary>
		public override System.Windows.Forms.IWin32Window Window
		{
			get
			{
				if (myTreeControl == null)
				{
					LoadWindow();
				}
				return myTreeControl;
			}
		}
		#endregion //ORMToolWindow overrides
	}
}
