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
using Microsoft.VisualStudio.Shell.Interop;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// This is the concrete tool window class that the Object Model Browser uses
	/// </summary>
	[Guid("DD2334C3-AFDB-4FC5-9E8A-17D19A8CC97A")]
	[CLSCompliant(false)]
	public partial class ORMBrowserToolWindow : ModelExplorerToolWindow, IORMSelectionContainer
	{
		private ORMDesignerCommands myVisibleCommands;
		private ORMDesignerCommands myCheckedCommands;
		private ORMDesignerCommands myCheckableCommands;
		private ORMDesignerCommands myEnabledCommands;
		private object myCommandSet;
		private IMonitorSelectionService myMonitorSelection;
				
		#region MenuService, MonitorSelectionService, and SelectedNode properties
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
		protected IMonitorSelectionService MonitorSelectionService
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
		#endregion //MenuService, MonitorSelectionService, and SelectedNode properties
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
			IMonitorSelectionService monitorService = this.MonitorSelectionService;
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
				commandText = ResourceStrings.CommandDeleteMultipleElementsText;
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
				browser.HideSelection = false;
				browser.ItemDrag += new ItemDragEventHandler(browser_ItemDrag);
			}
			void browser_ItemDrag(object sender, ItemDragEventArgs e)
			{
				object resolvedObject = EditorUtility.ResolveContextInstance(e.Item, false);
				ModelElement element;
				IDataObject elementData = new DataObject();
				if (null != (element = resolvedObject  as MultiColumnExternalConstraint))
				{
					elementData.SetData(typeof(MultiColumnExternalConstraint), element);
					ObjectModelBrowser.DoDragDrop(elementData, DragDropEffects.All);
				}
				else if (null != (element = resolvedObject as SingleColumnExternalConstraint))
				{
					elementData.SetData(typeof(SingleColumnExternalConstraint), element);
					ObjectModelBrowser.DoDragDrop(elementData, DragDropEffects.All);
				}
				else if ((element = resolvedObject as ObjectType) != null || (element  = resolvedObject as FactType)!= null)
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
					return ORMDesignerDocView.ORMDesignerCommandIds.ViewContextMenu; //.ViewBrowserContextMenu;
				}
			}
			#region MonitorSelectionService and CurrentWindow Properties
			/// <summary>
			/// gets the monitor selection service associated with this ORM Model Browser window
			/// </summary>
			private IMonitorSelectionService MonitorSelectionService
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
			protected override IElementVisitorFilter CreateElementVisitorFilter()
			{
				return new CustomVisitorFilter();
			}
			protected override IList FindRootElements(Store store)
			{
				return store.ElementDirectory.GetElements(ORMModel.MetaClassGuid);
			}
			#endregion
			#region CustomVisitorFilter
			private class CustomVisitorFilter : AggregateVisitorFilter
			{
				private static Dictionary<Guid, VisitorFilterResult> myFilterDictionary;
				public override VisitorFilterResult ShouldVisitRelationship(ElementWalker walker, ModelElement sourceElement, MetaRoleInfo sourceRoleInfo, MetaRelationshipInfo metaRelationshipInfo, ElementLink targetRelationship)
				{
					VisitorFilterResult result;
					Dictionary<Guid, VisitorFilterResult> dictionary = myFilterDictionary;
					if (dictionary == null)
					{
						dictionary = new Dictionary<Guid, VisitorFilterResult>();
						dictionary.Add(ModelHasDataType.MetaRelationshipGuid, VisitorFilterResult.Never);
						dictionary.Add(ModelHasError.MetaRelationshipGuid, VisitorFilterResult.Never);
						dictionary.Add(ModelHasReferenceMode.MetaRelationshipGuid, VisitorFilterResult.Never);
						dictionary.Add(ModelHasReferenceModeKind.MetaRelationshipGuid, VisitorFilterResult.Never);
						dictionary.Add(ORMModelElementHasExtensionElement.MetaRelationshipGuid, VisitorFilterResult.Never);
						dictionary.Add(ORMNamedElementHasExtensionElement.MetaRelationshipGuid, VisitorFilterResult.Never);
						myFilterDictionary = dictionary;
					}
					if (!dictionary.TryGetValue(metaRelationshipInfo.Id, out result))
					{
						result = base.ShouldVisitRelationship(walker, sourceElement, sourceRoleInfo, metaRelationshipInfo, targetRelationship);
					}
					return result;
				}
			}
			#endregion // CustomVisitorFilter
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
				return 4;
			}
		}

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
