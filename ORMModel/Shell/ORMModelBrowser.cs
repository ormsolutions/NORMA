using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Editors;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
		/// <summary>
	/// This is the concrete tool window class that the Object Model Browser uses
	/// </summary>
	[Guid("DD2334C3-AFDB-4FC5-9E8A-17D19A8CC97A")]
	[CLSCompliant(false)]
	public partial class ORMBrowserToolWindow : ModelExplorerToolWindow
	{
		private ORMDesignerCommands myVisibleCommands;
		private ORMDesignerCommands myCheckedCommands;
		private ORMDesignerCommands myCheckableCommands;
		private ORMDesignerCommands myEnabledCommands;
		private object myCommandSet;
		private IMonitorSelectionService myMonitorSelection;
				
		#region MenuService, MonitorService, and SelectedNode properties
		private static bool myCommandsPopulated;
		/// <summary>
		/// returns the menu service and instantiates a new command set if none exists
		/// </summary>
		public override IMenuCommandService MenuService
		{
			get
			{
				IMenuCommandService retVal = base.MenuService;
				if (retVal != null && !myCommandsPopulated)
				{
					myCommandsPopulated = true;
					myCommandSet = new ORMModelBrowserCommandSet(myCtorServiceProvider, retVal);
				}
				return retVal;
			}
		}
		/// <summary>
		/// returns the monitor service
		/// </summary>
		protected IMonitorSelectionService MonitorService
		{
			get
			{
				IMonitorSelectionService monitorSelect = myMonitorSelection;
				if (monitorSelect == null)
				{
					myMonitorSelection = monitorSelect = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
				}
				return monitorSelect;
			}
		}
		/// <summary>
		/// returns the currently selected node in the tree view
		/// </summary>
		protected TreeNode SelectedNode
		{
			get 
			{
				return this.ObjectModelBrowser.ObjectModelBrowser.SelectedNode;
			}
		}
		#endregion //MenuService, MonitorService, and SelectedNode properties
		#region Command handling for window
		/// <summary>
		/// sets up commands that should be enabled in the ORM Model Browser window
		/// </summary>
		/// <param name="sender">Menu Command</param>
		/// <param name="commandFlags">commands that are a part of the menu command</param>
		/// <param name="currentWindow">the currently selected window</param>
		protected static void OnStatusCommand(Object sender, ORMDesignerCommands commandFlags, ORMBrowserToolWindow currentWindow)
		{
			MenuCommand command = sender as MenuCommand;
			Debug.Assert(command != null);
			if (currentWindow != null)
			{
				command.Visible = 0 != (commandFlags & currentWindow.myVisibleCommands);
				command.Enabled = 0 != (commandFlags & currentWindow.myEnabledCommands);
				command.Checked = 0 != (commandFlags & currentWindow.myCheckedCommands);
				if (0 != (commandFlags & (ORMDesignerCommands.Delete)))
				{
					currentWindow.SetDeleteCommandText((OleMenuCommand)command);
				}
			}
		}
		/// <summary>
		/// the action to be taken when a delete command is issued on the ORM Model Browser window
		/// </summary>
		/// <param name="commandText">text of the current command</param>
		protected virtual void OnMenuDelete(String commandText)
		{
			TreeNode currentNode = SelectedNode;
			if (currentNode != null)
			{
				ModelElement selectedType = EditorUtility.ResolveContextInstance(currentNode, false) as ModelElement;
				if (0 != (myEnabledCommands & ORMDesignerCommands.Delete))//facts objects multi and single column external constraints
				{
					Store store = selectedType.Store;
					Debug.Assert(store != null);
					using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", "")))
					{
						if (!selectedType.IsRemoved)
						{
							IDictionary contextinfo = t.TopLevelTransaction.Context.ContextInfo;
							foreach (object o in selectedType.PresentationRolePlayers)
							{
								ObjectTypeShape objectShape = o as ObjectTypeShape;
								if (null != objectShape && !objectShape.ExpandRefMode)
								{
									contextinfo[ObjectType.DeleteReferenceModeValueType] = null;
								}
							}
							selectedType.PresentationRolePlayers.Clear();
							selectedType.Remove();
						}
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
				}
			}
		}
		/// <summary>
		/// fires when ORM Browser Tool window has a selection change
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			ORMDesignerCommands visibleCommands = ORMDesignerCommands.None;
			ORMDesignerCommands enabledCommands = ORMDesignerCommands.None;
			ORMDesignerCommands checkedCommands = ORMDesignerCommands.None;
			ORMDesignerCommands checkableCommands = ORMDesignerCommands.None;
			IMonitorSelectionService monitorService = this.MonitorService;
			if (monitorService != null)
			{
				ORMDesignerDocView currentDoc = monitorService.CurrentDocumentView as ORMDesignerDocView;
				if (currentDoc != null)
				{
					TreeNode selectedNode = SelectedNode;
					if (selectedNode != null)
					{
						ModelElement selectedType = EditorUtility.ResolveContextInstance(selectedNode, false) as ModelElement;
						if (selectedType != null)
						{
							currentDoc.SetCommandStatus(selectedType, null, out visibleCommands, out enabledCommands, out checkableCommands, out checkedCommands);
						}
					}
				}
			}
			myVisibleCommands = visibleCommands;
			myEnabledCommands = enabledCommands;
			myCheckedCommands = checkedCommands & visibleCommands;
			myCheckableCommands = checkableCommands & visibleCommands & enabledCommands;
			base.OnSelectionChanged(e);
		}
		#region set command text 
		/// <summary>
		/// Set the menu's text for the delete command
		/// </summary>
		/// <param name="command">OleMenuCommand</param>
		protected virtual void SetDeleteCommandText(OleMenuCommand command)
		{
			Debug.Assert(command != null);
			string commandText;
			switch (myVisibleCommands & ORMDesignerCommands.Delete)
			{
				case ORMDesignerCommands.DeleteObjectType:
					commandText = ResourceStrings.CommandDeleteObjectTypeText;
					break;
				case ORMDesignerCommands.DeleteFactType:
					commandText = ResourceStrings.CommandDeleteFactTypeText;
					break;
				case ORMDesignerCommands.DeleteConstraint:
					commandText = ResourceStrings.CommandDeleteConstraintText;
					break;
				case ORMDesignerCommands.DeleteRole:
					commandText = ResourceStrings.CommandDeleteRoleText;
					break;
				default:
					commandText = null;
					break;
			}
			if (commandText == null && 0 != (myVisibleCommands & ORMDesignerCommands.DeleteAny))
			{
				commandText = ResourceStrings.CommandDeleteMultipleText;
			}
			// Setting command.Text to null will pick up
			// the default command text
			command.Text = commandText;
		}
		#endregion //set command text
		#endregion //Command handling for window
		#region TreeContainer class
		/// <summary>
		/// Summary description for ORMModelExplorerTreeContainer.
		/// </summary>
		private sealed class ORMModelExplorerTreeContainer : ModelExplorerTreeContainer
		{
			IMonitorSelectionService myMonitorSelection;
			protected override void OnCreateControl()
			{
				base.OnCreateControl();
				TreeView browser = ObjectModelBrowser;
				browser.ItemDrag += new ItemDragEventHandler(browser_ItemDrag);
			}
			void browser_ItemDrag(object sender, ItemDragEventArgs e)
			{
				object resolvedObject = EditorUtility.ResolveContextInstance(e.Item, false);
				ModelElement element = resolvedObject as ObjectType;
				if (element != null || (element  = resolvedObject as FactType)!= null)
				{
					ObjectModelBrowser.DoDragDrop(element, DragDropEffects.All);
				}
			}
			/// <summary>
			/// Use the standard command ORM command
			/// </summary>
			protected override CommandID ContextMenuCommandId
			{
				get
				{
					return ORMDesignerDocView.ORMDesignerCommandIds.ViewContextMenu;
				}
			}
			#region MonitorSelectionService and CurrentWindow Properties
			/// <summary>
			/// gets the monitor selection service associated with this ORM Model Browser window
			/// </summary>
			private IMonitorSelectionService MonitorService
			{
				get
				{
					IMonitorSelectionService monitorSelect = myMonitorSelection;
					if (monitorSelect == null)
					{
						myMonitorSelection = monitorSelect = (IMonitorSelectionService)ServiceProvider.GetService(typeof(IMonitorSelectionService));
					}
					return monitorSelect;
				}
			}
			#endregion //MonitorSelectionService and CurrentWindow Properties
			#region Constructor
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="serviceProvider">Service</param>
			public ORMModelExplorerTreeContainer(IServiceProvider serviceProvider) : base(serviceProvider)
			{
			}
			#endregion
			#region ModelExplorerTreeContainer Overrides
			/// <summary>
			/// Create IElementVisitor
			/// </summary>
			/// <returns>IElementVisitor</returns>
			protected override IElementVisitor CreateElementVisitor()
			{
				return new ExplorerElementVisitor(this.ObjectModelBrowser, ServiceProvider);
			}
			protected override IList FindRootElements(Store store)
			{
				return store.ElementDirectory.GetElements(ORMModel.MetaClassGuid);
			}
			#endregion
			#region CustomVisitor
			/*
			private class CustomVisitor : ExplorerElementVisitor
			{
				public CustomVisitor(TreeView treeView, IServiceProvider serviceProvider) : base(treeView, serviceProvider)
				{
				}
				protected override void PruneTree()
				{
					PruneTree(TreeView.Nodes);
				}
				private void PruneTree(TreeNodeCollection nodes)
				{
					IList<ModelElementTreeNode> deadNodes = null;
					foreach (TreeNode childNode in nodes)
					{
						ModelElementTreeNode node;
						if (null != (node = childNode as ModelElementTreeNode))
						{
							if (node.KeepNode)
							{
								node.KeepNode = false;
								PruneTree(node.Nodes);
							}
							else
							{
								if (deadNodes == null)
								{
									deadNodes = new List<ModelElementTreeNode>();
								}
								deadNodes.Add(node);
							}
						}
					}
					if (deadNodes != null)
					{
						foreach (TreeNode killNode in deadNodes)
						{
							killNode.Remove();
						}
					}
				}
			}
			*/
			#endregion // CustomVisitor
		}
		#endregion // TreeContainer class
		#region Construction/Destruction
		private IServiceProvider myCtorServiceProvider;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serviceProvider">Service Provider</param>
		public ORMBrowserToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			myCtorServiceProvider = serviceProvider; 
		}
		#endregion
		#region ModelExplorerToolWindow Overrides
		/// <summary>
		/// Retrieve the localized text for the model browser tool window
		/// </summary>
		public override String WindowTitle
		{
			get
			{
				return ResourceStrings.ModelBrowserWindowTitle;
			}
		}

		/// <summary>
		/// Create TreeContainer
		/// </summary>
		/// <returns>ModelExplorerTreeContainer</returns>
		protected override ModelExplorerTreeContainer CreateTreeContainer()
		{
			return new ORMModelExplorerTreeContainer(this);
		}
		#endregion
	}
}
