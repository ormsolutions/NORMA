#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// Tool window to contain survey tree control
	/// </summary>
	[Guid("DD2334C3-AFDB-4FC5-9E8A-17D19A8CC97A")]
	[CLSCompliant(false)]
	public partial class ORMModelBrowserToolWindow : ORMToolWindow, IORMSelectionContainer, IProvideFrameVisibility
	{
		#region Member Variables
		private SurveyTreeContainer myTreeContainer;
		private object myCommandSet;

		// Cached command status
		private ORMDesignerCommands myVisibleCommands;
		private ORMDesignerCommands myCheckedCommands;
		private ORMDesignerCommands myCheckableCommands;
		private ORMDesignerCommands myEnabledCommands;
		private bool myStatusCacheValid;

		// Dynamic command set caches
		private ElementGrouping[] myIncludeInGroups;
		private ElementGrouping[] myDeleteFromGroups;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="serviceProvider"></param>
		public ORMModelBrowserToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#endregion // Constructor
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
					myCommandSet = new ORMModelBrowserCommandSet(ExternalServiceProvider, retVal);
				}
				return retVal;
			}
		}
		private object SelectedNode
		{
			get
			{
				VirtualTreeControl treeControl = myTreeContainer.TreeControl;
				int currentIndex = treeControl.CurrentIndex;
				if (currentIndex >= 0)
				{
					VirtualTreeItemInfo info = treeControl.Tree.GetItemInfo(currentIndex, 0, false);
					int options = 0;
					object trackingObject = info.Branch.GetObject(info.Row, 0, ObjectStyle.TrackingObject, ref options);

					// Resolve reference reasons
					ISurveyNodeReference reference = trackingObject as ISurveyNodeReference;
					if (reference != null)
					{
						switch (reference.SurveyNodeReferenceOptions & (SurveyNodeReferenceOptions.SelectReferenceReason | SurveyNodeReferenceOptions.SelectSelf))
						{
							case SurveyNodeReferenceOptions.SelectSelf:
								break;
							case SurveyNodeReferenceOptions.SelectReferenceReason:
								trackingObject = reference.SurveyNodeReferenceReason;
								break;
							default:
								trackingObject = reference.ReferencedElement;
								break;
						}
					}
					
					// There is no guarantee we'll get a ModelElement, but things work better if we can
					IRepresentModelElements proxy;
					ModelElement[] representedElements;
					if (trackingObject != null &&
						!(trackingObject is ModelElement) &&
						null != (proxy = trackingObject as IRepresentModelElements) &&
						null != (representedElements = proxy.GetRepresentedElements()) &&
						1 == representedElements.Length)
					{
						trackingObject = representedElements[0];
					}
					return trackingObject;
				}
				return null;
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
		protected static void OnStatusCommand(object sender, ORMDesignerCommands commandFlags, ORMModelBrowserToolWindow currentWindow)
		{
			MenuCommand command = sender as MenuCommand;
			Debug.Assert(command != null);
			if (currentWindow != null)
			{
				currentWindow.EnsureCommandStatusCache();
				bool isEnabled;
				command.Visible = 0 != (commandFlags & currentWindow.myVisibleCommands);
				command.Enabled = isEnabled = 0 != (commandFlags & currentWindow.myEnabledCommands);
				command.Checked = 0 != (commandFlags & currentWindow.myCheckedCommands);
				if (command.Visible)
				{
					if (0 != (commandFlags & (ORMDesignerCommands.Delete)))
					{
						currentWindow.SetDeleteCommandText((OleMenuCommand)command);
					}
					else if (0 != (commandFlags & (ORMDesignerCommands.DiagramList)))
					{
						OleMenuCommand cmd = command as OleMenuCommand;
						object selectedNode = currentWindow.SelectedNode;
						IElementReference elementReference;
						ModelElement element;
						string diagramName = null;
						if (null != (elementReference = selectedNode as IElementReference) ?
								null != (element = elementReference.ReferencedElement as ModelElement) :
								null != (element = selectedNode as ModelElement))
						{
							int diagramIndex = cmd.MatchedCommandId;
							ORMBaseShape.VisitAssociatedShapes(
								element,
								null,
								true,
								delegate(ShapeElement testShape)
								{
									if (diagramIndex == 0)
									{
										diagramName = testShape.Diagram.Name;
										return false;
									}
									--diagramIndex;
									return true;
								});
						}
						if (diagramName != null)
						{
							cmd.Enabled = true;
							cmd.Visible = true;
							cmd.Supported = true;
							cmd.Text = diagramName;
						}
						else
						{
							cmd.Supported = false;
						}
					}
					else if (0 != (commandFlags & ORMDesignerCommands.FreeFormCommandList))
					{
						bool haveStatus = false;
						object node = currentWindow.SelectedNode;
						Store store = null;
						ModelElement element;
						ModelingDocData docData;
						if (null != (element = node as ModelElement))
						{
							store = element.Store;
						}
						else if (null != (docData = currentWindow.CurrentDocument))
						{
							store = docData.Store;
						}
						if (store != null)
						{
							IFreeFormCommandProvider<Store> directCommandProvider = node as IFreeFormCommandProvider<Store>;
							IFreeFormCommandProviderService<Store>[] remoteCommandServices = ((IFrameworkServices)store).GetTypedDomainModelProviders<IFreeFormCommandProviderService<Store>>();
							if (directCommandProvider != null || remoteCommandServices != null)
							{
								int freeFormCommandIndex = ((OleMenuCommand)command).MatchedCommandId;
								IFreeFormCommandProvider<Store> resolvedCommandProvider = null;
								int commandCount;
								if (directCommandProvider != null)
								{
									commandCount = directCommandProvider.GetFreeFormCommandCount(store, directCommandProvider);
									if (freeFormCommandIndex < commandCount)
									{
										resolvedCommandProvider = directCommandProvider;
									}
									else
									{
										freeFormCommandIndex -= commandCount;
									}
								}
								if (resolvedCommandProvider == null && remoteCommandServices != null)
								{
									for (int i = 0; i < remoteCommandServices.Length; ++i)
									{
										IFreeFormCommandProvider<Store> remoteCommandProvider = remoteCommandServices[i].GetFreeFormCommandProvider(store, node);
										if (remoteCommandProvider != null)
										{
											commandCount = remoteCommandProvider.GetFreeFormCommandCount(store, node);
											if (freeFormCommandIndex < commandCount)
											{
												resolvedCommandProvider = remoteCommandProvider;
												break;
											}
											freeFormCommandIndex -= commandCount;
										}
									}
								}
								if (resolvedCommandProvider != null)
								{
									resolvedCommandProvider.OnFreeFormCommandStatus(store, node, command, freeFormCommandIndex);
									command.Supported = true; // Make sure this is turned on, or the dynamic menus do not work
									haveStatus = true;
								}
							}
						}
						if (!haveStatus)
						{
							command.Supported = false;
						}
					}
					else if (0 != (commandFlags & ORMDesignerCommands.IncludeInGroupList))
					{
						// UNDONE: ModelBrowserMultiselect There is a multi-select version of this in ORMDesignerCommandManager.
						// Share the implementations when the model browser is multiselect.
						if (isEnabled)
						{
							ElementGrouping[] cachedGroupings = currentWindow.myIncludeInGroups;
							if (cachedGroupings == null)
							{
								ModelElement element = EditorUtility.ResolveContextInstance(currentWindow.SelectedNode, false) as ModelElement;
								if (element == null)
								{
									cachedGroupings = new ElementGrouping[0]; // Set this even for zero to indicate that we tried to get it
								}
								else
								{
									// Get the full set of groups, determine which ones support removal for
									// all selected elements, and sort the results by group name.
									ReadOnlyCollection<ElementGroupingSet> groupingSet = element.Store.ElementDirectory.FindElements<ElementGroupingSet>();
									LinkedElementCollection<ElementGrouping> groupings = null;
									int groupingCount = (groupingSet.Count == 0) ? 0 : (groupings = groupingSet[0].GroupingCollection).Count;
									if (groupingCount == 0)
									{
										cachedGroupings = new ElementGrouping[0];
									}
									else
									{
										cachedGroupings = new ElementGrouping[groupingCount];

										// Keep any group that allows deletion for some element
										int allowedGroupingCount = 0;
										for (int i = 0; i < groupingCount; ++i)
										{
											ElementGrouping grouping = groupings[i];
											if (GroupingMembershipInclusion.AddAllowed == grouping.GetElementInclusion(element, null))
											{
												cachedGroupings[allowedGroupingCount] = grouping;
												++allowedGroupingCount;
											}
										}
										if (allowedGroupingCount < groupingCount)
										{
											groupingCount = allowedGroupingCount;
											Array.Resize<ElementGrouping>(ref cachedGroupings, groupingCount);
										}
										if (groupingCount > 1)
										{
											Array.Sort<ElementGrouping>(cachedGroupings, NamedElementComparer<ElementGrouping>.CurrentCulture);
										}
									}
								}
								currentWindow.myIncludeInGroups = cachedGroupings;
							}

							OleMenuCommand cmd = (OleMenuCommand)sender;
							int groupIndex = cmd.MatchedCommandId;
							if (groupIndex < cachedGroupings.Length)
							{
								cmd.Enabled = true;
								cmd.Visible = true;
								cmd.Supported = true;
								cmd.Text = "&" + cachedGroupings[groupIndex].Name;
							}
							else
							{
								cmd.Supported = false;
							}
						}
					}
					else if (0 != (commandFlags & ORMDesignerCommands.DeleteFromGroupList))
					{
						// UNDONE: ModelBrowserMultiselect There is a multi-select version of this in ORMDesignerCommandManager.
						// Share the implementations when the model browser is multiselect.
						if (isEnabled)
						{
							ElementGrouping[] cachedGroupings = currentWindow.myDeleteFromGroups;
							if (cachedGroupings == null)
							{
								ModelElement element = EditorUtility.ResolveContextInstance(currentWindow.SelectedNode, false) as ModelElement;
								if (element == null)
								{
									cachedGroupings = new ElementGrouping[0]; // Set this even for zero to indicate that we tried to get it
								}
								else
								{
									// Get the full set of groups, determine which ones support removal for
									// all selected elements, and sort the results by group name.
									ReadOnlyCollection<ElementGroupingSet> groupingSet = element.Store.ElementDirectory.FindElements<ElementGroupingSet>();
									LinkedElementCollection<ElementGrouping> groupings = null;
									int groupingCount = (groupingSet.Count == 0) ? 0 : (groupings = groupingSet[0].GroupingCollection).Count;
									if (groupingCount == 0)
									{
										cachedGroupings = new ElementGrouping[0];
									}
									else
									{
										cachedGroupings = new ElementGrouping[groupingCount];

										// Keep any group that allows deletion for this element
										int allowedGroupingCount = 0;
										for (int i = 0; i < groupingCount; ++i)
										{
											ElementGrouping grouping = groupings[i];
											switch (grouping.GetMembershipType(element))
											{
												case GroupingMembershipType.Inclusion:
												case GroupingMembershipType.Contradiction:
													cachedGroupings[allowedGroupingCount] = grouping;
													++allowedGroupingCount;
													break;
											}
										}
										if (allowedGroupingCount < groupingCount)
										{
											groupingCount = allowedGroupingCount;
											Array.Resize<ElementGrouping>(ref cachedGroupings, groupingCount);
										}
										if (groupingCount > 1)
										{
											Array.Sort<ElementGrouping>(cachedGroupings, NamedElementComparer<ElementGrouping>.CurrentCulture);
										}
									}
								}
								currentWindow.myDeleteFromGroups = cachedGroupings;
							}

							OleMenuCommand cmd = (OleMenuCommand)sender;
							int groupIndex = cmd.MatchedCommandId;
							if (groupIndex < cachedGroupings.Length)
							{
								cmd.Enabled = true;
								cmd.Visible = true;
								cmd.Supported = true;
								cmd.Text = "&" + cachedGroupings[groupIndex].Name;
							}
							else
							{
								cmd.Supported = false;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// the action to be taken when a delete command is issued on the ORM Model Browser window
		/// </summary>
		/// <param name="commandText">text of the current command</param>
		protected virtual void OnMenuDelete(String commandText)
		{
			object currentNode = SelectedNode;
			if (currentNode != null)
			{
				EnsureCommandStatusCache(); // Should do nothing because the status request comes first
				if (0 != (myEnabledCommands & (ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny)))//facts objects multi and single column external constraints
				{
					ContextElementParts currentParts = ContextElementParts.ResolveContextInstance(currentNode, false);
					ModelElement referenceElement = currentParts.ReferenceElement as ModelElement;
					ModelElement deleteTarget = referenceElement ?? currentParts.PrimaryElement as ModelElement;
					if (deleteTarget != null)
					{
						Store store = deleteTarget.Store;
						Debug.Assert(store != null);
						using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", "")))
						{
							if (!deleteTarget.IsDeleted)
							{
								ObjectType objectType;
								GroupingElementInclusion groupingInclusion;
								GroupingMembershipContradictionErrorIsForElement groupingContradictionLink;
								bool executeDelete = true;
								if (null != (objectType = deleteTarget as ObjectType))
								{
									Dictionary<object, object> contextinfo = t.TopLevelTransaction.Context.ContextInfo;
									LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(objectType);
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
								}
								else if (referenceElement != null)
								{
									if (null != (groupingInclusion = deleteTarget as GroupingElementInclusion))
									{
										ElementGrouping.RemoveElement(groupingInclusion);
										executeDelete = false;
									}
									else if (null != (groupingContradictionLink = deleteTarget as GroupingMembershipContradictionErrorIsForElement))
									{
										ElementGrouping.RemoveElement(groupingContradictionLink);
										executeDelete = false;
									}
								}
								if (executeDelete)
								{
									ICustomElementDeletion customDeletion = deleteTarget as ICustomElementDeletion;
									if (customDeletion != null)
									{
										customDeletion.DeleteCustomElement();
									}
									else
									{
										deleteTarget.Delete();
									}
								}
							}
							if (t.HasPendingChanges)
							{
								t.Commit();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Split an exclusive or constraint into its two parts
		/// </summary>
		protected virtual void OnMenuExclusiveOrDecoupler()
		{
			MandatoryConstraint constraint = SelectedNode as MandatoryConstraint;
			if (constraint != null)
			{
				Store store = constraint.Store;
				IORMToolServices toolServices = (IORMToolServices)store;
				toolServices.AutomatedElementFilter += AutomatedExclusionConstraintFilter;
				try
				{
					using (Transaction t = constraint.Store.TransactionManager.BeginTransaction(ResourceStrings.ExclusiveOrDecouplerTransactionName))
					{
						constraint.ExclusiveOrExclusionConstraint = null;
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
				}
				finally
				{
					toolServices.AutomatedElementFilter -= AutomatedExclusionConstraintFilter;
				}
			}
		}
		private AutomatedElementDirective AutomatedExclusionConstraintFilter(ModelElement element)
		{
			return element is ExclusionConstraint ? AutomatedElementDirective.NeverIgnore : AutomatedElementDirective.None;
		}
		/// <summary>
		/// Include a group exclusion element in the group
		/// </summary>
		protected virtual void OnMenuIncludeInGroup()
		{
			GroupingElementExclusion currentNode = SelectedNode as GroupingElementExclusion;
			if (currentNode != null && !currentNode.IsDeleted)
			{
				using (Transaction t = currentNode.Store.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingRemoveElementExclusionTransactionName))
				{
					// Rules will handle addition of either an inclusion or contradiction to the group
					currentNode.Delete();
					t.Commit();
				}
			}

		}
		/// <summary>
		/// Include a selected element in a new group
		/// </summary>
		protected virtual void OnMenuIncludeInNewGroup()
		{
			ModelElement currentNode = EditorUtility.ResolveContextInstance(SelectedNode, false) as ModelElement;
			if (currentNode != null &&
				!currentNode.IsDeleted)
			{
				ElementGrouping grouping;
				Store store = currentNode.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingAddGroupTransactionName))
				{
					grouping = new ElementGrouping(store);
					ReadOnlyCollection<ElementGroupingSet> groupingSets = store.ElementDirectory.FindElements<ElementGroupingSet>();
					grouping.GroupingSet = (groupingSets.Count == 0) ? new ElementGroupingSet(store) : groupingSets[0];
					new GroupingElementInclusion(grouping, currentNode);
					t.Commit();
				}
				((IORMToolServices)store).NavigateTo(grouping, NavigateToWindow.ModelBrowser);
			}

		}
		/// <summary>
		/// Remove selected item from the group at the specified index
		/// </summary>
		/// <param name="groupIndex">The offset of the group in the current set of current groups</param>
		public virtual void OnMenuIncludeInGroupList(int groupIndex)
		{
			ModelElement currentNode = EditorUtility.ResolveContextInstance(SelectedNode, false) as ModelElement;
			ElementGrouping[] eligibleGroups;
			if (currentNode != null &&
				!currentNode.IsDeleted &&
				null != (eligibleGroups = myIncludeInGroups) &&
				groupIndex < eligibleGroups.Length)
			{
				ElementGrouping grouping = eligibleGroups[groupIndex];
				IList<ElementGroupingType> groupingTypes = grouping.GroupingTypeCollection;
				using (Transaction t = currentNode.Store.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingAddElementTransactionName))
				{
					if (grouping.GetElementInclusion(currentNode, groupingTypes) == GroupingMembershipInclusion.AddAllowed)
					{
						GroupingElementExclusion exclusion = GroupingElementExclusion.GetLink(grouping, currentNode);
						if (exclusion != null)
						{
							// Delete the exclusion. A rule will automatically determine
							// if this turns into a new inclusion or a contradiction.
							exclusion.Delete();
						}
						else
						{
							new GroupingElementInclusion(grouping, currentNode);
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Remove selected item from the group at the specified index
		/// </summary>
		/// <param name="groupIndex">The offset of the group in the current set of current groups</param>
		public virtual void OnMenuDeleteFromGroupList(int groupIndex)
		{
			ModelElement currentNode = EditorUtility.ResolveContextInstance(SelectedNode, false) as ModelElement;
			ElementGrouping[] eligibleGroups;
			if (currentNode != null &&
				!currentNode.IsDeleted &&
				null != (eligibleGroups = myDeleteFromGroups) &&
				groupIndex < eligibleGroups.Length)
			{
				ElementGrouping grouping = eligibleGroups[groupIndex];
				using (Transaction t = currentNode.Store.TransactionManager.BeginTransaction(ResourceStrings.CommandDeleteFromGroupText.Replace("&", "")))
				{
					grouping.RemoveGroupedElement(currentNode, null);
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Place the element label editing mode
		/// </summary>
		protected virtual void OnMenuEditLabel()
		{
			// We always report an enabled status on this, so verify that it
			// is actually enabled before handling it.
			EnsureCommandStatusCache(); // Should do nothing because the status request comes first
			if (0 != (myEnabledCommands & ORMDesignerCommands.EditLabel))
			{
				myTreeContainer.TreeControl.BeginLabelEdit();
			}
		}
		/// <summary>
		/// Select the specified shape in the specified target window.
		/// The shape may be a diagram.
		/// </summary>
		protected virtual void OnMenuSelectShape(NavigateToWindow targetWindow)
		{
			ShapeElement shape;
			if (null != (shape = SelectedNode as ShapeElement))
			{
				((IORMToolServices)shape.Store).ActivateShape(shape, targetWindow);
			}
		}
		/// <summary>
		/// Activate the shape on the selected diagram
		/// </summary>
		/// <param name="diagramIndex">The index of the diagram in the diagram list</param>
		/// <param name="targetWindow">The type of window to navigate to select the diagram in</param>
		protected virtual void OnMenuDiagramList(int diagramIndex, NavigateToWindow targetWindow)
		{
			ModelElement element;
			IElementReference elementReference;
			object selectedNode = SelectedNode;
			if (null != (elementReference = selectedNode as IElementReference) ?
					null != (element = elementReference.ReferencedElement as ModelElement) :
					null != (element = selectedNode as ModelElement))
			{
				ORMBaseShape.VisitAssociatedShapes(
					element,
					null,
					true,
					delegate(ShapeElement shape)
					{
						if (diagramIndex == 0)
						{
							(shape.Store as IORMToolServices).ActivateShape(shape, targetWindow);
							return false;
						}
						--diagramIndex;
						return true;
					});
			}
		}
		/// <summary>
		/// Menu handler for executing a free-form command
		/// </summary>
		/// <param name="freeFormCommandIndex">The index of the freeform command</param>
		protected virtual void OnMenuFreeFormCommand(int freeFormCommandIndex)
		{
			object node = SelectedNode;
			Store store = null;
			ModelElement element;
			ModelingDocData docData;
			if (null != (element = SelectedNode as ModelElement))
			{
				store = element.Store;
			}
			else if (null != (docData = CurrentDocument))
			{
				store = docData.Store;
			}
			if (store != null)
			{
				IFreeFormCommandProvider<Store> directCommandProvider = node as IFreeFormCommandProvider<Store>;
				IFreeFormCommandProviderService<Store>[] remoteCommandServices = ((IFrameworkServices)store).GetTypedDomainModelProviders<IFreeFormCommandProviderService<Store>>();
				if (directCommandProvider != null || remoteCommandServices != null)
				{
					IFreeFormCommandProvider<Store> resolvedCommandProvider = null;
					int commandCount;
					if (directCommandProvider != null)
					{
						commandCount = directCommandProvider.GetFreeFormCommandCount(store, directCommandProvider);
						if (freeFormCommandIndex < commandCount)
						{
							resolvedCommandProvider = directCommandProvider;
						}
						else
						{
							freeFormCommandIndex -= commandCount;
						}
					}
					if (resolvedCommandProvider == null && remoteCommandServices != null)
					{
						for (int i = 0; i < remoteCommandServices.Length; ++i)
						{
							IFreeFormCommandProvider<Store> remoteCommandProvider = remoteCommandServices[i].GetFreeFormCommandProvider(store, element);
							if (remoteCommandProvider != null)
							{
								commandCount = remoteCommandProvider.GetFreeFormCommandCount(store, element);
								if (freeFormCommandIndex < commandCount)
								{
									resolvedCommandProvider = remoteCommandProvider;
									break;
								}
								freeFormCommandIndex -= commandCount;
							}
						}
					}
					if (resolvedCommandProvider != null)
					{
						resolvedCommandProvider.OnFreeFormCommandExecute(store, node, freeFormCommandIndex);
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
			base.OnSelectionChanged(e);
			UpdateCommandStatus();
		}
		/// <summary>
		/// Enable menu commands when the selection changes
		/// </summary>
		public void UpdateCommandStatus()
		{
			myStatusCacheValid = false;
			myIncludeInGroups = null;
			myDeleteFromGroups = null;
		}
		private void EnsureCommandStatusCache()
		{
			if (myStatusCacheValid)
			{
				return;
			}
			ORMDesignerCommands visibleCommands = ORMDesignerCommands.None;
			ORMDesignerCommands enabledCommands = ORMDesignerCommands.None;
			ORMDesignerCommands checkedCommands = ORMDesignerCommands.None;
			ORMDesignerCommands checkableCommands = ORMDesignerCommands.None;
			ORMDesignerCommands toleratedCommands = ORMDesignerCommands.None;
			ORMDesignerDocView currentDoc = CurrentDocumentView as ORMDesignerDocView;
			if (currentDoc != null)
			{
				object selectedNode = SelectedNode;
				if (selectedNode != null)
				{
					ContextElementParts selectedParts = ContextElementParts.ResolveContextInstance(selectedNode, false);
					ModelElement selectedElement = selectedParts.PrimaryElement as ModelElement;
					if (selectedElement != null)
					{
						((IORMDesignerView)currentDoc).CommandManager.SetCommandStatus(selectedParts, true, out visibleCommands, out enabledCommands, out checkableCommands, out checkedCommands, out toleratedCommands);
						// Add in label editing command
						ISurveyNode surveyNode = selectedElement as ISurveyNode;
						if ((surveyNode != null && surveyNode.IsSurveyNameEditable) || selectedElement is ISurveyNodeCustomEditor)
						{
							visibleCommands |= ORMDesignerCommands.EditLabel;
							enabledCommands |= ORMDesignerCommands.EditLabel;
						}
						if (selectedNode is ShapeElement)
						{
							visibleCommands |= ORMDesignerCommands.SelectInDocumentWindow | ORMDesignerCommands.SelectInDiagramSpy;
							enabledCommands |= ORMDesignerCommands.SelectInDocumentWindow | ORMDesignerCommands.SelectInDiagramSpy;
						}
						else
						{
							// These may be turned on by the current document, but we have our own handlers,
							// so we need the command flags off to avoid showing inappropriate commands.
							visibleCommands &= ~(ORMDesignerCommands.SelectInDocumentWindow | ORMDesignerCommands.SelectInDiagramSpy | ORMDesignerCommands.IncludeInNewGroup | ORMDesignerCommands.IncludeInGroupList | ORMDesignerCommands.DeleteFromGroupList);
							enabledCommands &= ~(ORMDesignerCommands.SelectInDocumentWindow | ORMDesignerCommands.SelectInDiagramSpy | ORMDesignerCommands.IncludeInNewGroup | ORMDesignerCommands.IncludeInGroupList | ORMDesignerCommands.DeleteFromGroupList);

							// Do later checking for the DiagramList command
							visibleCommands |= ORMDesignerCommands.DiagramList;
							enabledCommands |= ORMDesignerCommands.DiagramList;
						}
						// UNDONE: NestedGrouping
						if (selectedParts.ReferenceElement == null && !(selectedElement is ElementGrouping) && !(selectedElement is ElementGroupingType))
						{
							visibleCommands |= ORMDesignerCommands.IncludeInNewGroup | ORMDesignerCommands.IncludeInGroupList | ORMDesignerCommands.DeleteFromGroupList;
							enabledCommands |= ORMDesignerCommands.IncludeInNewGroup | ORMDesignerCommands.IncludeInGroupList | ORMDesignerCommands.DeleteFromGroupList;
						}
					}
					visibleCommands |= ORMDesignerCommands.FreeFormCommandList;
					enabledCommands |= ORMDesignerCommands.FreeFormCommandList;
				}
			}
			myVisibleCommands = visibleCommands;
			myEnabledCommands = enabledCommands;
			myCheckedCommands = checkedCommands & visibleCommands;
			myCheckableCommands = checkableCommands & visibleCommands & enabledCommands;
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
			EnsureCommandStatusCache();
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
				case ORMDesignerCommands.DeleteGroup:
					commandText = ResourceStrings.CommandDeleteGroupText;
					break;
				case ORMDesignerCommands.RemoveFromGroup:
					commandText = ResourceStrings.CommandDeleteFromGroupText;
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
		#region LoadWindow method

		/// <summary>
		/// Loads the SurveyTreeControl from the current document
		/// </summary>
		protected void LoadWindow()
		{
			SurveyTreeContainer treeContainer = myTreeContainer;
			if (treeContainer == null)
			{
				myTreeContainer = treeContainer = new SurveyTreeContainer();
				VirtualTreeControl treeControl = treeContainer.TreeControl;
				treeControl.SelectionChanged += new EventHandler(Tree_SelectionChanged);
				treeControl.ContextMenuInvoked += new ContextMenuEventHandler(Tree_ContextMenuInvoked);
				treeControl.LabelEditControlChanged += new EventHandler(Tree_LabelEditControlChanged);
				treeControl.DoubleClick += new DoubleClickEventHandler(Tree_DoubleClick);
				Guid commandSetId = typeof(ORMDesignerEditorFactory).GUID;
				Frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref commandSetId);
			}

			ORMDesignerDocData currentDocument = this.CurrentDocument;
			treeContainer.Tree = (currentDocument != null) ? currentDocument.SurveyTree : null;
		}
		private void Tree_DoubleClick(object sender, DoubleClickEventArgs e)
		{
			if (!e.Handled &&
				e.Button == MouseButtons.Left &&
				0 != (e.HitInfo.HitTarget & (VirtualTreeHitTargets.OnItem | VirtualTreeHitTargets.OnItemRight)))
			{
				VirtualTreeItemInfo itemInfo = e.ItemInfo;
				int options = 0;
				ISurveyNodeReference reference = itemInfo.Branch.GetObject(itemInfo.Row, 0, ObjectStyle.TrackingObject, ref options) as ISurveyNodeReference;
				object referencedElement;
				if (null != reference &&
					0 == (reference.SurveyNodeReferenceOptions & SurveyNodeReferenceOptions.BlockTargetNavigation) &&
					!((referencedElement = reference.ReferencedElement) is ISurveyFloatingNode))
				{
					if (((VirtualTreeControl)sender).SelectObject(null, referencedElement, (int)ObjectStyle.TrackingObject, 0))
					{
						e.Handled = true;
					}
				}
			}
		}
		private void Tree_LabelEditControlChanged(object sender, EventArgs e)
		{
			ActiveInPlaceEditWindow = myTreeContainer.TreeControl.LabelEditControl;
		}
		private void Tree_ContextMenuInvoked(object sender, ContextMenuEventArgs e)
		{
			IMenuCommandService menuCommandService = MenuService;
			if (menuCommandService != null)
			{
				menuCommandService.ShowContextMenu(ORMDesignerDocView.ORMDesignerCommandIds.ViewContextMenu, e.X, e.Y);
			}
		}
		private void Tree_SelectionChanged(object sender, EventArgs e)
		{
			object selectedObject = SelectedNode;
			SetSelectedComponents((selectedObject != null) ? new object[] { selectedObject } : null);
		}
		#endregion //LoadWindow method
		#region ORMToolWindow overrides
		/// <summary>
		/// Required override. Attach handlers for ElementEventsBegun and ElementEventsEnded.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			if (Utility.ValidateStore(store) == null)
			{
				return;
			}
			// Track Currently Executing Events
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent), action);
		}

		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			ITree tree = this.myTreeContainer.Tree;
			if (tree != null)
			{
				tree.DelayRedraw = true;
			}
		}

		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			ITree tree = this.myTreeContainer.Tree;
			if (tree != null)
			{
				tree.DelayRedraw = false;
			}
			if (((IORMToolServices)sender).ProcessingVisibleTransactionItemEvents)
			{
				// UNDONE: We could probably put this check around the full ElementEventsBegunEvent
				// and ElementEventsEndedEvent methods. However, the delayredraw is cheap and nothing
				// bad should happen here by not turning this on.
				UpdateCommandStatus();
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
				return PackageResources.ToolWindowIconIndex.ModelBrowser;
			}
		}
		/// <summary>
		/// retuns the SurveyTreeControl this window contains
		/// </summary>
		public override System.Windows.Forms.IWin32Window Window
		{
			get
			{
				if (myTreeContainer == null)
				{
					LoadWindow();
				}
				return myTreeContainer;
			}
		}
		#endregion //ORMToolWindow overrides
		#region IProvideFrameVisibility Implementation
		FrameVisibility IProvideFrameVisibility.CurrentFrameVisibility
		{
			get
			{
				return CurrentFrameVisibility;
			}
		}
		#endregion // IProvideFrameVisibility Implementation
	}
}
