#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class ORMReferenceModeEditorToolWindow
	{
		partial class ReferenceModeViewForm
		{
			private sealed partial class ReferenceModeHeaderBranch : IBranch
			{
				/// <summary>
				/// Branch for all the differnt Reference mode kinds
				/// </summary>
				private sealed class ReferenceModeKindsBranch : MultiColumnBaseBranch, IBranch
				{
					#region Locals
					private List<ReferenceModeKind> myReferenceModeKindsList = new List<ReferenceModeKind>();
					private ORMModel myModel;
					private Store myStore;
					#endregion //Locals

					#region Methods
					/// <summary>
					/// Sets the reference modes displayed on the tree
					/// </summary>
					/// <param name="model"></param>
					public void SetModel(ORMModel model)
					{
						if (model != myModel)
						{
							Store newStore = (model == null) ? null : model.Store;
							Store oldStore = myStore;
							if (oldStore != null && oldStore != newStore && !oldStore.Disposed)
							{
								ManageStoreEvents(oldStore, EventHandlerAction.Remove);
							}
							if (newStore != null && newStore != oldStore)
							{
								ManageStoreEvents(newStore, EventHandlerAction.Add);

							}
							this.myModel = model;
							this.myStore = newStore;
							int count = myReferenceModeKindsList.Count;
							this.myReferenceModeKindsList.Clear();
							if (myModify != null && count != 0)
							{
								myModify(this, BranchModificationEventArgs.DeleteItems(this, 0, count));
							}
							if (model != null)
							{
								foreach (ReferenceModeKind kind in model.ReferenceModeKindCollection)
								{
									this.myReferenceModeKindsList.Add(kind);
								}
								myReferenceModeKindsList.Sort();
							}
							count = myReferenceModeKindsList.Count;
							if (myModify != null && count != 0)
							{
								myModify(this, BranchModificationEventArgs.InsertItems(this, -1, count));
							}
						}
					}
					#endregion //Methods

					#region EventHandling
					/// <summary>
					/// An IMS event to track the shape element added to the associated
					/// diagram during this connect action.
					/// </summary>
					/// <param name="sender"></param>
					/// <param name="e"></param>
					private void ReferenceModeKindChangeEvent(object sender, ElementPropertyChangedEventArgs e)
					{
						ReferenceModeKind referenceModeKind = e.ModelElement as ReferenceModeKind;

						if (referenceModeKind != null && referenceModeKind.Model == this.myModel)
						{
							if (myModify != null)
							{
								int row = this.FindReferenceModeKind(referenceModeKind);
								myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, (int)Columns.FormatString, 1)));
							}
						}
					}

					private int FindReferenceModeKind(ReferenceModeKind referenceModeKind)
					{
						for (int i = 0; i < myReferenceModeKindsList.Count; i++)
						{
							ReferenceModeKind kind = myReferenceModeKindsList[i];
							if (kind == referenceModeKind)
							{
								return i;
							}
						}

						return -1;
					}
					/// <summary>
					/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> during activation and
					/// deactivation.
					/// </summary>
					/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
					/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
					private void ManageStoreEvents(Store store, EventHandlerAction action)
					{
						ModelingEventManager.GetModelingEventManager(store).AddOrRemoveHandler(store.DomainDataDirectory.FindDomainClass(ReferenceModeKind.DomainClassId), new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindChangeEvent), action);
					}
					#endregion // EventHandling
					#region IBranch Implementation

					VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
					{
						if (column == (int)Columns.FormatString)
						{
							ReferenceModeKind kind = myReferenceModeKindsList[row];
							if (kind.ReferenceModeType != ReferenceModeType.General || kind.FormatString != "{1}") // Allow a repair, we didn't used to enforce this
							{
								return VirtualTreeLabelEditData.Default;
							}
						}
						return VirtualTreeLabelEditData.Invalid;
					}
					private static AutomatedElementDirective FilterAllElements(ModelElement element)
					{
						// Ignore all elements
						return AutomatedElementDirective.Ignore;
					}
					LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
					{
						newText = newText.Trim();
						switch ((Columns)column)
						{
							case Columns.Name:
								return LabelEditResult.CancelEdit;
							case Columns.FormatString:
								if (newText.Length == 0)
								{
									return LabelEditResult.CancelEdit;
								}
								IORMToolServices toolServices = (IORMToolServices)myStore;
								toolServices.AutomatedElementFilter += FilterAllElements;
								try
								{
									using (Transaction t = myStore.TransactionManager.BeginTransaction(ResourceStrings.ModelReferenceModeEditorChangeFormatStringTransaction))
									{
										myReferenceModeKindsList[row].FormatString = UglyFormatString(newText);
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
								}
								finally
								{
									toolServices.AutomatedElementFilter -= FilterAllElements;
								}
								break;
							case Columns.ReferenceModeKind:
								return LabelEditResult.CancelEdit;
						}
						return LabelEditResult.AcceptEdit;
					}
					BranchFeatures IBranch.Features
					{
						get { return BranchFeatures.DelayedLabelEdits | BranchFeatures.InsertsAndDeletes; }
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						if (row == myReferenceModeKindsList.Count && column == 0)
						{
							VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
							retVal.ForeColor = SystemColors.GrayText;
							return retVal;
						}


						return VirtualTreeDisplayData.Empty;
					}
					string IBranch.GetText(int row, int column)
					{
						try
						{
							switch ((Columns)column)
							{
								case Columns.Name:
									return null;
								case Columns.FormatString:
									return PrettyFormatString(myReferenceModeKindsList[row]);
								case Columns.ReferenceModeKind:
									return myReferenceModeKindsList[row].ToString();
								default:
									return null;
							}
						}
						catch
						{
							return null;
						}
					}
					private BranchModificationEventHandler myModify;
					event BranchModificationEventHandler IBranch.OnBranchModification
					{
						add { myModify += value; }
						remove { myModify -= value; }
					}
					int IBranch.VisibleItemCount
					{
						get { return myReferenceModeKindsList.Count; }
					}
					#endregion //IBranch Implementation
				}
			}
		}
	}
}