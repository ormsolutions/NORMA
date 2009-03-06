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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class ORMReferenceModeEditorToolWindow
	{
		partial class ReferenceModeViewForm
		{
			private sealed partial class ReferenceModeHeaderBranch : IBranch
			{
				/// <summary>
				/// The Custom Reference Modes branch;
				/// </summary>
				private sealed class CustomReferenceModesBranch : MultiColumnBaseBranch, IBranch, IMultiColumnBranch
				{
					#region Locals
					private List<CustomReferenceMode> myCustomReferenceModesList = new List<CustomReferenceMode>();
					private Store myStore;
					private ORMModel myModel;
					private bool myIDidIt;

					#endregion // Locals

					#region Methods
					/// <summary>
					/// Sets the custom refernce modes.
					/// </summary>
					/// <param name="model"></param>
					public void SetModel(ORMModel model)
					{
						if (model != myModel)
						{
							Store newStore = (model == null) ? null : model.Store;
							if (myStore != null && myStore != newStore && !myStore.Disposed)
							{
								ManageStoreEvents(myStore, EventHandlerAction.Remove);
							}
							if (newStore != null && newStore != myStore)
							{
								ManageStoreEvents(newStore, EventHandlerAction.Add);
							}
							this.myModel = model;
							this.myStore = newStore;
							int count = myCustomReferenceModesList.Count;
							this.myCustomReferenceModesList.Clear();
							if (myModify != null && count != 0)
							{
								myModify(this, BranchModificationEventArgs.DeleteItems(this, 0, count));
							}
							if (model != null)
							{
								foreach (ReferenceMode mode in model.ReferenceModeCollection)
								{
									CustomReferenceMode customMode = mode as CustomReferenceMode;
									if (customMode != null)
									{
										this.myCustomReferenceModesList.Add(customMode);
									}
								}
								myCustomReferenceModesList.Sort();
							}
							count = myCustomReferenceModesList.Count;
							if (myModify != null && count != 0)
							{
								myModify(this, BranchModificationEventArgs.InsertItems(this, -1, count));
							}
						}
					}

					/// <summary>
					/// Deletes a specific row
					/// </summary>
					/// <param name="row"></param>
					/// <param name="col"></param>
					public void Delete(int row, int col)
					{
						if (row < myCustomReferenceModesList.Count && col <= (int)Columns.Last)
						{
							string changeNameTransaction = ResourceStrings.ModelReferenceModeEditorChangeNameTransaction;
							using (Transaction t = myStore.TransactionManager.BeginTransaction(changeNameTransaction))
							{
								myCustomReferenceModesList[row].Delete();
								if (t.HasPendingChanges)
								{
									t.Commit();
								}
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
						if (myModify != null)
						{
							ReferenceModeKind referenceModeKind = e.ModelElement as ReferenceModeKind;

							if (referenceModeKind != null && !referenceModeKind.IsDeleted && referenceModeKind.Model == this.myModel)
							{
								foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
								{
									CustomReferenceMode custRefMode = refMode as CustomReferenceMode;
									if (custRefMode != null)
									{
										int row = this.FindReferenceMode(custRefMode);
										myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, (int)Columns.FormatString, 1)));
									}
								}
							}
						}
					}
					private void CustomReferenceModeChangeEvent(object sender, ElementPropertyChangedEventArgs e)
					{
						CustomReferenceMode customReferenceMode = e.ModelElement as CustomReferenceMode;

						if (customReferenceMode != null && !customReferenceMode.IsDeleted && customReferenceMode.Model == this.myModel)
						{
							if (myModify != null)
							{
								Guid attributeId = e.DomainProperty.Id;
								int column = -1;
								if (attributeId == CustomReferenceMode.CustomFormatStringDomainPropertyId)
								{
									column = (int)Columns.FormatString;
								}
								else if (attributeId == CustomReferenceMode.NameDomainPropertyId)
								{
									column = (int)Columns.Name;
								}
								// The reference mode kind column keys off the relationship between
								// a reference mode and its kind. The change may also fire here if the
								// ReferenceMode.KindDisplay is used to change it, but we ignore the
								// property change in favor of the backing object.
								if (column != -1)
								{
									int row = this.FindReferenceMode(customReferenceMode);
									myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, column, 1)));
								}
							}
						}
					}

					private void CustomReferenceModeAddEvent(object sender, ElementAddedEventArgs e)
					{
						ModelHasReferenceMode link = e.ModelElement as ModelHasReferenceMode;
						CustomReferenceMode customReferenceMode = link.ReferenceMode as CustomReferenceMode;

						if (customReferenceMode != null && !customReferenceMode.IsDeleted && link.Model == this.myModel)
						{
							int index = myCustomReferenceModesList.BinarySearch(customReferenceMode, NamedElementComparer<CustomReferenceMode>.CurrentCulture);

							int insertAt = 0;
							insertAt = (index < 0) ? ~index : index;

							myCustomReferenceModesList.Insert(insertAt, customReferenceMode);

							if (myModify != null)
							{
								if (myIDidIt)
								{
									myIDidIt = false;
									myModify(this, BranchModificationEventArgs.InsertItems(this, myCustomReferenceModesList.Count - 1, 1));
									if (insertAt != index)
									{
										myModify(this, BranchModificationEventArgs.MoveItem(this, myCustomReferenceModesList.Count - 1, insertAt));
									}
								}
								else
								{
									myModify(this, BranchModificationEventArgs.InsertItems(this, insertAt - 1, 1));
								}
							}
						}
					}

					private void CustomReferenceModeRemoveEvent(object sender, ElementDeletedEventArgs e)
					{
						ModelHasReferenceMode link = e.ModelElement as ModelHasReferenceMode;
						CustomReferenceMode customReferenceMode = link.ReferenceMode as CustomReferenceMode;

						if (customReferenceMode != null && link.Model == this.myModel)
						{
							int row = this.FindReferenceMode(customReferenceMode);
							if (row >= 0)
							{
								myCustomReferenceModesList.RemoveAt(row);
								if (myModify != null)
								{
									myModify(this, BranchModificationEventArgs.DeleteItems(this, row, 1));
								}
							}
						}
					}

					private void ReferenceModeHasKindChangeEvent(object sender, RolePlayerChangedEventArgs e)
					{
						if (myModify != null)
						{
							ReferenceModeHasReferenceModeKind link = e.ElementLink as ReferenceModeHasReferenceModeKind;
							if (link != null)
							{
								ReferenceModeKind referenceModeKind = link.Kind;
								if (referenceModeKind.Model == this.myModel && !link.IsDeleted)
								{
									foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
									{
										CustomReferenceMode custRefMode = refMode as CustomReferenceMode;
										if (custRefMode != null)
										{
											int row = this.FindReferenceMode(custRefMode);
											myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, -1, 1)));
										}
									}
								}
							}
						}
					}

					private int FindReferenceMode(CustomReferenceMode custRefMode)
					{
						return myCustomReferenceModesList.IndexOf(custRefMode);
					}

					/// <summary>
					/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> during activation and
					/// deactivation.
					/// </summary>
					/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
					/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
					private void ManageStoreEvents(Store store, EventHandlerAction action)
					{
						if (store == null || store.Disposed)
						{
							return; // bail out
						}
						DomainDataDirectory dataDirectory = store.DomainDataDirectory;
						ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);

						DomainClassInfo classInfo = dataDirectory.FindDomainClass(ReferenceModeKind.DomainClassId);
						eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindChangeEvent), action);

						classInfo = dataDirectory.FindDomainClass(CustomReferenceMode.DomainClassId);
						eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(CustomReferenceModeChangeEvent), action);

						classInfo = dataDirectory.FindDomainRelationship(ModelHasReferenceMode.DomainClassId);
						eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(CustomReferenceModeAddEvent), action);
						eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(CustomReferenceModeRemoveEvent), action);

						classInfo = dataDirectory.FindDomainRelationship(ReferenceModeHasReferenceModeKind.DomainClassId);
						eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ReferenceModeHasKindChangeEvent), action);
					}
					#endregion // EventHandling

					#region IMultiColumnBranch Implementation
					int IMultiColumnBranch.GetJaggedColumnCount(int row)
					{
						return (row == myCustomReferenceModesList.Count) ? 1 : ((int)Columns.Last + 1);
					}
					#endregion // IMultiColumnBranch Implementation

					#region IBranch Implementation
					VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
					{
						if (column == (int)Columns.ReferenceModeKind)
						{
							ModelElement element = myCustomReferenceModesList[row];
							if (element.Store == null)
							{
								// Teardown scenario
								return VirtualTreeLabelEditData.Invalid;
							}
							PropertyDescriptor descriptor = DomainTypeDescriptor.CreatePropertyDescriptor(element, ReferenceMode.KindDisplayDomainPropertyId);
							TypeEditorHost hostControl = OnScreenTypeEditorHost.Create(descriptor, element, TypeEditorHostEditControlStyle.TransparentEditRegion);
							hostControl.Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText | VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.ForwardKeyEvents;
							return new VirtualTreeLabelEditData(hostControl);
						}
						else if (0 != (activationStyle & VirtualTreeLabelEditActivationStyles.ImmediateSelection))
						{
							return VirtualTreeLabelEditData.DeferActivation;
						}
						else if (row == myCustomReferenceModesList.Count)
						{
							return new VirtualTreeLabelEditData("");
						}
						else if (column == (int)Columns.Name)
						{
							return VirtualTreeLabelEditData.Default;
						}
						else if (column == (int)Columns.FormatString)
						{
							CustomReferenceMode mode = myCustomReferenceModesList[row];
							if (mode.Kind.ReferenceModeType != ReferenceModeType.General || mode.CustomFormatString != "{1}")
							{
								return new VirtualTreeLabelEditData(PrettyFormatString(mode, true));
							}
						}
						return VirtualTreeLabelEditData.Invalid;
					}

					LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
					{
						newText = newText.Trim();
						if (row < myCustomReferenceModesList.Count)
						{
							switch ((Columns)column)
							{
								case Columns.Name:
									string changeNameTransaction = ResourceStrings.ModelReferenceModeEditorChangeNameTransaction;
									using (Transaction t = myStore.TransactionManager.BeginTransaction(changeNameTransaction))
									{
										if (newText.Length != 0)
										{
											myCustomReferenceModesList[row].Name = newText;
										}
										else
										{
											myCustomReferenceModesList[row].Delete();
										}
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
									break;
								case Columns.FormatString:
									using (Transaction t = myStore.TransactionManager.BeginTransaction(ResourceStrings.ModelReferenceModeEditorChangeFormatStringTransaction))
									{
										myCustomReferenceModesList[row].CustomFormatString = UglyFormatString(newText);
										if (t.HasPendingChanges)
										{
											t.Commit();
										}
									}
									break;
								case Columns.ReferenceModeKind:
									Debug.WriteLine("New text on Kind mode: " + newText);
									break;
							}
						}
						else
						{
							Transaction t = null;
							bool success = false;

							string addCustomReferenceModeTransaction = ResourceStrings.ModelReferenceModeEditorAddCustomReferenceModeTransaction;
							using (t = myStore.TransactionManager.BeginTransaction(addCustomReferenceModeTransaction))
							{
								new CustomReferenceMode(this.myStore, new PropertyAssignment(CustomReferenceMode.NameDomainPropertyId, newText)).Model = this.myModel;
								// Note that the Kind is automatically set to General on Commit

								if (t.HasPendingChanges)
								{
									try
									{
										myIDidIt = true;
										t.Commit();
										success = true;
									}
									finally
									{
										myIDidIt = false;
									}
								}
							}
							if (success)
							{
								// We want to activate the kind dropdown for the new row.
								// However, if we begin a label edit now before the current
								// one is finished, then the control gets really confused, so
								// we wait until the control is officially done--it will tell us
								// via an event--so we can open the new dropdown.
								VirtualTreeControl control = ORMDesignerPackage.ReferenceModeEditorWindow.TreeControl;
								control.LabelEditControlChanged += new EventHandler(DelayActivateKindDropdown);
							}
						}
						return LabelEditResult.AcceptEdit;
					}
					/// <summary>
					/// Select the kind column, activate the edit control, and
					/// open the dropdown. Implemented as a delayed event so we
					/// can call it indirectly from CommitLabelEdit.
					/// </summary>
					/// <param name="sender">VirtualTreeControl</param>
					/// <param name="e">EventArgs</param>
					private static void DelayActivateKindDropdown(object sender, EventArgs e)
					{
						VirtualTreeControl control = (VirtualTreeControl)sender;
						Debug.Assert(!control.InLabelEdit); // Fires after a CommitLabelEdit, the control will be closed

						// We only care on the first call
						control.LabelEditControlChanged -= new EventHandler(DelayActivateKindDropdown);

						// Select the correct column, launch the label edit control, then
						// activate it by opening the dropdown
						control.CurrentColumn = (int)Columns.ReferenceModeKind;
						control.BeginLabelEdit();
						if (control.InLabelEdit)
						{
							TypeEditorHost hostControl = control.LabelEditControl as TypeEditorHost;
							if (hostControl != null)
							{
								hostControl.OpenDropDown();
							}
						}
					}

					BranchFeatures IBranch.Features
					{
						get { return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.ImmediateSelectionLabelEdits | BranchFeatures.JaggedColumns | BranchFeatures.InsertsAndDeletes; }
					}


					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						if (row == myCustomReferenceModesList.Count && column == 0)
						{
							VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
							retVal.ForeColor = SystemColors.GrayText;
							return retVal;
						}
						if (row < myCustomReferenceModesList.Count && column == (int)Columns.FormatString)
						{
							if (myCustomReferenceModesList[row].CustomFormatString == null || myCustomReferenceModesList[row].CustomFormatString.Length == 0)
							{
								VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
								retVal.ForeColor = SystemColors.GrayText;
								return retVal;
							}
						}
						return VirtualTreeDisplayData.Empty;
					}
					string IBranch.GetText(int row, int column)
					{
						try
						{
							if (row < myCustomReferenceModesList.Count)
							{
								switch ((Columns)column)
								{
									case Columns.Name:
										return myCustomReferenceModesList[row].Name;
									case Columns.FormatString:
										return PrettyFormatString(myCustomReferenceModesList[row], false);
									case Columns.ReferenceModeKind:
										if (myCustomReferenceModesList[row].Kind != null)
										{
											return myCustomReferenceModesList[row].Kind.ToString();
										}
										else
										{
											return null;
										}
									default:
										return null;
								}
							}
							else
							{
								string addNewRowText = ResourceStrings.ModelReferenceModeEditorAddNewRowText;
								return addNewRowText;
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
						get { return myCustomReferenceModesList.Count + 1; }
					}
					#endregion // IBranch Implementation
				}
			}
		}
	}
}
