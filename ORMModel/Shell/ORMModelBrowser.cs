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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Neumont.Tools.ORM.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

using PropertyChangedEventArgs = Microsoft.VisualStudio.Modeling.Diagrams.PropertyChangedEventArgs;

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
						if (!selectedType.IsDeleted)
						{
							Dictionary<object, object> contextinfo = t.TopLevelTransaction.Context.ContextInfo;
							LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(selectedType);
							foreach (PresentationElement o in presentationElements)
							{
								ObjectTypeShape objectShape;
								ObjectifiedFactTypeNameShape objectifiedShape;
								if ((null != (objectShape = o as ObjectTypeShape) && !objectShape.ExpandRefMode) ||
									(null != (objectifiedShape = o as ObjectifiedFactTypeNameShape) && !objectifiedShape.ExpandRefMode))
								{
									contextinfo[ObjectType.DeleteReferenceModeValueType] = null;
								}
							}
							presentationElements.Clear();
							selectedType.Delete();
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
			ORMDesignerCommands toleratedCommands = ORMDesignerCommands.None;
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
							currentDoc.SetCommandStatus(selectedType, null, true, out visibleCommands, out enabledCommands, out checkableCommands, out checkedCommands, out toleratedCommands);
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
				case ORMDesignerCommands.DeleteModelNote:
					commandText = ResourceStrings.CommandDeleteModelNoteText;
					break;
				case ORMDesignerCommands.DeleteModelNoteReference:
					commandText = ResourceStrings.CommandDeleteModelNoteReferenceText;
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
				if (null != (element = resolvedObject  as SetComparisonConstraint))
				{
					elementData.SetData(typeof(SetComparisonConstraint), element);
					ObjectModelBrowser.DoDragDrop(elementData, DragDropEffects.All);
				}
				else if (null != (element = resolvedObject as SetConstraint))
				{
					elementData.SetData(typeof(SetConstraint), element);
					ObjectModelBrowser.DoDragDrop(elementData, DragDropEffects.All);
				}
				else if ((element = resolvedObject as ObjectType) != null || (element  = resolvedObject as FactType)!= null)
				{
					ObjectModelBrowser.DoDragDrop(element, DragDropEffects.All);
				}
				else if (null != (element = resolvedObject as ModelNote))
				{
					elementData.SetData(typeof(ModelNote), element);
					ObjectModelBrowser.DoDragDrop(elementData, DragDropEffects.All);
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
				return store.ElementDirectory.FindElements<ORMModel>();
			}
			#endregion
			#region CustomVisitorFilter
			private sealed class CustomVisitorFilter : EmbeddingVisitorFilter
			{
				private static Dictionary<Guid, VisitorFilterResult> myFilterDictionary;
				public sealed override VisitorFilterResult ShouldVisitRelationship(ElementWalker walker, ModelElement sourceElement, DomainRoleInfo sourceRoleInfo, DomainRelationshipInfo domainRelationshipInfo, ElementLink targetRelationship)
				{
					VisitorFilterResult result;
					Dictionary<Guid, VisitorFilterResult> dictionary = myFilterDictionary;
					if (dictionary == null)
					{
						dictionary = new Dictionary<Guid, VisitorFilterResult>();
						dictionary.Add(ModelHasDataType.DomainClassId, VisitorFilterResult.Never);
						dictionary.Add(ModelHasError.DomainClassId, VisitorFilterResult.Never);
						dictionary.Add(ModelHasReferenceMode.DomainClassId, VisitorFilterResult.Never);
						dictionary.Add(ModelHasReferenceModeKind.DomainClassId, VisitorFilterResult.Never);
						dictionary.Add(ORMModelElementHasExtensionElement.DomainClassId, VisitorFilterResult.Never);
						myFilterDictionary = dictionary;
					}
					if (!dictionary.TryGetValue(domainRelationshipInfo.Id, out result))
					{
						result = base.ShouldVisitRelationship(walker, sourceElement, sourceRoleInfo, domainRelationshipInfo, targetRelationship);
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
		/// <summary>
		/// Hack override for the framework's incomplete implementation of OnDocumentWindowChanged
		/// </summary>
		protected override void OnDocumentWindowChanged(ModelingDocView oldView, ModelingDocView newView)
		{
			// UNDONE: MSBUG The frameworks implementation of OnDocumentWindowChanged is
			// garbage. Specifically, the ModelExplorerTreeContainer.ModelingDocData property
			// is not set if the newView is empty. This causes an immediate crash during document
			// reload (the new docdata has not changed, but the events are still attached to the
			// old store), and delayed crashes if the model browser is touched when the document
			// is closed.
			ModelExplorerTreeContainer container = ObjectModelBrowser;
			if (container != null)
			{
				ModelingDocData oldDocData = container.ModelingDocData;
				if (oldDocData != null)
				{
					DetachModelEvents(oldDocData.Store);
				}
				ModelingDocData newDocData = (newView != null) ? (newView.DocData as ModelingDocData) : null;
				myFactCollectionNode = null;
				if (newDocData != null)
				{
					container.ShowTree();
					AttachModelEvents(newDocData.Store);
				}
				else
				{
					container.HideTree();
				}
				container.ModelingDocData = newDocData;
			}
		}
		private void AttachModelEvents(Store store)
		{
			if (store == null || store.Disposed)
			{
				return;
			}
			store.EventManagerDirectory.ElementPropertyChanged.Add(FactType.NameChangedDomainPropertyId, new EventHandler<ElementPropertyChangedEventArgs>(FactTypeNameChanged));
		}
		private void DetachModelEvents(Store store)
		{
			if (store == null || store.Disposed)
			{
				return;
			}
			store.EventManagerDirectory.ElementPropertyChanged.Remove(FactType.NameChangedDomainPropertyId, new EventHandler<ElementPropertyChangedEventArgs>(FactTypeNameChanged));
		}
		private RoleGroupTreeNode myFactCollectionNode;
		/// <summary>
		/// Custom handling of fact type name changes. The default implementation
		/// naively assumes all name updates stem from changes to NamedElement.Name
		/// and does not give any way to directly attached other events to individual
		/// nodes, so we have to go hunting to find the facts.
		/// </summary>
		private void FactTypeNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			FactType fact = e.ModelElement as FactType;
			if (!fact.IsDeleted)
			{
				RoleGroupTreeNode factsNode = myFactCollectionNode;
				if (factsNode == null)
				{
					ModelExplorerTreeContainer treeContainer;
					TreeView treeControl;
					TreeNodeCollection rootNodes;
					ModelElementTreeNode modelNode;
					if (null != (treeContainer = ObjectModelBrowser) &&
						null != (treeControl = treeContainer.ObjectModelBrowser) &&
						0 != (rootNodes = treeControl.Nodes).Count &&
						null != (modelNode = rootNodes[0] as ModelElementTreeNode))
					{
						DomainRoleInfo factRoleInfo = modelNode.ModelElement.Store.DomainDataDirectory.FindDomainRole(ModelHasFactType.FactTypeDomainRoleId);
						TreeNode testNode = modelNode.FirstNode;
						while (testNode != null)
						{
							RoleGroupTreeNode testRoleNode = testNode as RoleGroupTreeNode;
							if (testRoleNode != null && testRoleNode.RoleInfo == factRoleInfo)
							{
								myFactCollectionNode = factsNode = testRoleNode;
								break;
							}
							testNode = testNode.NextNode;
						}
					}
				}
				if (factsNode != null)
				{
					foreach (TreeNode testNode in factsNode.Nodes)
					{
						ModelElementTreeNode elementNode = testNode as ModelElementTreeNode;
						if (elementNode != null && elementNode.ModelElement == fact)
						{
							elementNode.UpdateNodeText();
							break;
						}
					}
				}
			}
		}
		#endregion
	}
}
