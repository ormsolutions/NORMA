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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.Shell
{
	partial class ORMReferenceModeEditorToolWindow
	{
		partial class ReferenceModeViewForm
		{
			private sealed partial class ReferenceModeHeaderBranch : IBranch
			{
				/// <summary>
				/// The Intrinsic Reference Modes Branch on the tree
				/// </summary>
				private sealed class IntrinsicReferenceModesBranch : MultiColumnBaseBranch, IBranch
				{
					#region Locals
					private List<IntrinsicReferenceMode> myIntrinsicReferenceModesList = new List<IntrinsicReferenceMode>();
					private Store myStore;
					private ORMModel myModel;
					#endregion //Locals
					#region Methods
					/// <summary>
					/// Sets the reference modes for the control
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
							int count = myIntrinsicReferenceModesList.Count;
							this.myIntrinsicReferenceModesList.Clear();
							if (myModify != null && count != 0)
							{
								myModify(this, BranchModificationEventArgs.DeleteItems(this, 0, count));
							}
							if (model != null)
							{
								foreach (ReferenceMode mode in model.ReferenceModeCollection)
								{
									IntrinsicReferenceMode intMode = mode as IntrinsicReferenceMode;
									if (intMode != null)
									{
										this.myIntrinsicReferenceModesList.Add(intMode);
									}
								}
								myIntrinsicReferenceModesList.Sort();
							}
							count = myIntrinsicReferenceModesList.Count;
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
						if (myModify != null)
						{
							ReferenceModeKind referenceModeKind = e.ModelElement as ReferenceModeKind;

							if (referenceModeKind != null && referenceModeKind.Model == this.myModel)
							{
								foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
								{
									IntrinsicReferenceMode intrinsicRefMode = refMode as IntrinsicReferenceMode;
									if (intrinsicRefMode != null)
									{
										int row = this.FindReferenceMode(intrinsicRefMode);
										myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, (int)Columns.FormatString, 1)));
									}
								}
							}
						}
					}

					private int FindReferenceMode(IntrinsicReferenceMode intRefMode)
					{
						for (int i = 0; i < myIntrinsicReferenceModesList.Count; i++)
						{
							IntrinsicReferenceMode mode = myIntrinsicReferenceModesList[i];
							if (mode == intRefMode)
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
						if (store == null || store.Disposed)
						{
							return; // bail out
						}
						ModelingEventManager.GetModelingEventManager(store).AddOrRemoveHandler(store.DomainDataDirectory.FindDomainClass(ReferenceModeKind.DomainClassId), new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindChangeEvent), action);
					}
					#endregion // EventHandling
					#region IBranch Implementation
					BranchFeatures IBranch.Features
					{
						get { return BranchFeatures.InsertsAndDeletes; }
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						if (row == myIntrinsicReferenceModesList.Count && column == 0)
						{
							VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
							retVal.ForeColor = SystemColors.GrayText;
							return retVal;
						}
						return VirtualTreeDisplayData.Empty;
					}
					string IBranch.GetText(int row, int column)
					{
						if (row < myIntrinsicReferenceModesList.Count)
						{
							switch ((Columns)column)
							{
								case Columns.Name:
									return myIntrinsicReferenceModesList[row].Name;
								case Columns.FormatString:
									return PrettyFormatString(myIntrinsicReferenceModesList[row], false);
								case Columns.ReferenceModeKind:
									return myIntrinsicReferenceModesList[row].Kind.ToString();
							}
						}
						return null;
					}
					private BranchModificationEventHandler myModify;
					event BranchModificationEventHandler IBranch.OnBranchModification
					{
						add { myModify += value; }
						remove { myModify -= value; }
					}
					int IBranch.VisibleItemCount
					{
						get { return myIntrinsicReferenceModesList.Count; }
					}
					#endregion //IBranch Implementation
				}
			}
		}
	}
}