#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
using Microsoft.VisualStudio.Modeling;
#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// The Custom Reference Modes branch;
	/// </summary>
	public class CustomReferenceModesBranch : IBranch, IMultiColumnBranch
	{
		#region Locals
		private enum Columns
		{
			Name = 0,
			ReferenceModeKind = 1,
			FormatString = 2
		}

		private List<CustomReferenceMode> myCustomReferenceModesList = new List<CustomReferenceMode>();
		private static int myNumCols = Enum.GetValues(typeof(Columns)).Length;
		private Columns[] myEditable = new Columns[] { Columns.FormatString, Columns.Name, Columns.ReferenceModeKind };
		private Store myStore;
		private ORMModel myModel;
		private bool myIDidIt = false;

		#endregion // Locals
		#region Methods
		/// <summary>
		/// Sets the custom refernce modes.
		/// </summary>
		/// <param name="model"></param>
		public void SetModel(ORMModel model)
		{
			if (!object.ReferenceEquals(model, myModel))
			{
				Store newStore = (model == null) ? null : model.Store;
				if (myStore != null && !object.ReferenceEquals(myStore, newStore) && !myStore.Disposed)
				{
					this.RemoveStoreEvents(myStore);
				}
				if (newStore != null && !object.ReferenceEquals(newStore, myStore))
				{
					this.AddStoreEvents(newStore);

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
			if (row < myCustomReferenceModesList.Count && col < myNumCols)
			{
				string changeNameTransaction = ResourceStrings.ModelReferenceModeEditorChangeNameTransaction;
				using (Transaction t = myStore.TransactionManager.BeginTransaction(changeNameTransaction))
				{
					myCustomReferenceModesList[row].Remove();
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}

		/// <summary>
		/// Replaces the {0} and {1} with entityTypeName and referenceModeName
		/// </summary>
		/// <param name="uglyFormatString"></param>
		/// <returns></returns>
		private string PrettyFormatString(string uglyFormatString)
		{
			string entityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorEntityTypeName + "}";
			string referenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorReferenceModeName + "}";
			return uglyFormatString.Replace("{0}", entityTypeName).Replace("{1}", referenceModeName);
		}

		/// <summary>
		/// Replaces the {0} and {1} with entityTypeName and referenceModeName
		/// </summary>
		/// <param name="prettyFormatString"></param>
		/// <returns></returns>
		private string UglyFormatString(string prettyFormatString)
		{
			string entityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorEntityTypeName + "}";
			string referenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorReferenceModeName + "}";

			return prettyFormatString.Replace(entityTypeName, "{0}").Replace(referenceModeName, "{1}");
		}
		#endregion //Methods

		#region EventHandling
		/// <summary>
		/// An IMS event to track the shape element added to the associated
		/// diagram during this connect action.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReferenceModeKindChangeEvent(object sender, ElementAttributeChangedEventArgs e)
		{
				ReferenceModeKind referenceModeKind = e.ModelElement as ReferenceModeKind;

				if (referenceModeKind != null && ! referenceModeKind.IsRemoved && referenceModeKind.Model == this.myModel)
				{
					foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
					{
						if (refMode is CustomReferenceMode)
						{
							if (myModify != null)
							{
								int row = this.FindReferenceMode((CustomReferenceMode)refMode);
								myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, (int)Columns.FormatString, 1)));
							}
						}
					}
				}
		}
		private void CustomReferenceModeChangeEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			CustomReferenceMode customReferenceMode = e.ModelElement as CustomReferenceMode;

			if (customReferenceMode != null && !customReferenceMode.IsRemoved && customReferenceMode.Model == this.myModel)
			{
				if (myModify != null)
				{
					Guid attributeId = e.MetaAttribute.Id;
					int column = -1;
					if (attributeId == CustomReferenceMode.CustomFormatStringMetaAttributeGuid)
					{
						column = (int)Columns.FormatString;
					}
					else if (attributeId == CustomReferenceMode.NameMetaAttributeGuid)
					{
						column = (int)Columns.Name;
					}
					// The reference mode kind column keys off the relationship between
					// a reference mode and its kind. The change may also fire here if the
					// ReferenceMode.KindDisplay is used to change it, but we ignore the
					// attribute change in favor of the backing object.
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
			CustomReferenceMode customReferenceMode = link.ReferenceModeCollection as CustomReferenceMode;

			if (customReferenceMode != null && !customReferenceMode.IsRemoved && link.Model == this.myModel)
			{
				if (myModify != null)
				{
					int index = myCustomReferenceModesList.BinarySearch(customReferenceMode);

					int insertAt = 0;
					insertAt = (index < 0) ? ~index : index;

					myCustomReferenceModesList.Insert(insertAt, customReferenceMode);

					if (myModify != null)
					{
						myModify(this, BranchModificationEventArgs.InsertItems(this, myCustomReferenceModesList.Count - 1, 1));
						if (insertAt != index && myIDidIt)
						{
							myIDidIt = false;
							myModify(this, BranchModificationEventArgs.MoveItem(this, myCustomReferenceModesList.Count - 1, insertAt));
						}
					}
				}

			}
		}

		private void CustomReferenceModeRemoveEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelHasReferenceMode link = e.ModelElement as ModelHasReferenceMode;
			CustomReferenceMode customReferenceMode = link.ReferenceModeCollection as CustomReferenceMode;

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
			ReferenceModeHasReferenceModeKind link = e.ElementLink as ReferenceModeHasReferenceModeKind;
			if (link != null)
			{
				ReferenceModeKind referenceModeKind = link.Kind;
				if (referenceModeKind.Model == this.myModel && !link.IsRemoved)
				{
					foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
					{
						if (refMode is CustomReferenceMode)
						{
							if (myModify != null)
							{
								int row = this.FindReferenceMode((CustomReferenceMode)refMode);
								myModify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, row, -1, 1)));
							}
						}
					}
				}
			}
		}

		private int FindReferenceMode(CustomReferenceMode custRefMode)
		{
			for(int i = 0; i < myCustomReferenceModesList.Count; i++)				
			{
				CustomReferenceMode mode = myCustomReferenceModesList[i];
				if (mode == custRefMode)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Add events to the store during connect action
		/// activation. The default implementation watches for
		/// new external constraints added to the diagram.
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void AddStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ReferenceModeKind.MetaClassGuid);
			eventManager.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ReferenceModeKindChangeEvent));

			classInfo = dataDirectory.FindMetaClass(CustomReferenceMode.MetaClassGuid);
			eventManager.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(CustomReferenceModeChangeEvent));

			classInfo = dataDirectory.FindMetaRelationship(ModelHasReferenceMode.MetaRelationshipGuid);
			eventManager.ElementAdded.Add(classInfo, new ElementAddedEventHandler(CustomReferenceModeAddEvent));
			eventManager.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(CustomReferenceModeRemoveEvent));

			classInfo = dataDirectory.FindMetaRelationship(ReferenceModeHasReferenceModeKind.MetaRelationshipGuid);
			eventManager.RolePlayerChanged.Add(classInfo, new RolePlayerChangedEventHandler(ReferenceModeHasKindChangeEvent));
		}
		/// <summary>
		/// Removed any events added during the AddStoreEvents methods
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void RemoveStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ReferenceModeKind.MetaClassGuid);
			eventManager.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ReferenceModeKindChangeEvent));

			classInfo = dataDirectory.FindMetaClass(CustomReferenceMode.MetaClassGuid);
			eventManager.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(CustomReferenceModeChangeEvent));

			classInfo = dataDirectory.FindMetaRelationship(ModelHasReferenceMode.MetaRelationshipGuid);
			eventManager.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(CustomReferenceModeAddEvent));
			eventManager.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(CustomReferenceModeRemoveEvent));

			classInfo = dataDirectory.FindMetaRelationship(ReferenceModeHasReferenceModeKind.MetaRelationshipGuid);
			eventManager.RolePlayerChanged.Remove(classInfo, new RolePlayerChangedEventHandler(ReferenceModeHasKindChangeEvent));
		}

		#endregion // EventHandling

		#region IMultiColumnBranch Members
		int IMultiColumnBranch.ColumnCount
		{
			get { return myNumCols; }
		}


		SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
		{
			return SubItemCellStyles.Simple;
		}

		int IMultiColumnBranch.GetJaggedColumnCount(int row)
		{
			if (row == myCustomReferenceModesList.Count)
			{
				return 1;
			}
			else
			{
				return myNumCols;
			}
		}

		#endregion

		#region IBranch Members
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			if (column == (int)Columns.ReferenceModeKind)
			{
				ModelElement element = myCustomReferenceModesList[row];
				MetaAttributeInfo metaAttr = element.Store.MetaDataDirectory.FindMetaAttribute(ReferenceMode.KindDisplayMetaAttributeGuid);
				ElementPropertyDescriptor descriptor = element.CreatePropertyDescriptor(metaAttr, element);

				TypeEditorHost hostControl = TypeEditorHost.Create(descriptor, element, TypeEditorHostEditControlStyle.TransparentEditRegion);
				hostControl.Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.SizeToText;
				// UNDONE: Deferring ImmediateSelection from the control sends in the
				// ImmediateSelection value twice to activationStyle. Use the following
				// settings when/if this is fixed.
				//TypeEditorHost hostControl = TypeEditorHost.Create(descriptor, element, TypeEditorHostEditControlStyle.TransparentEditRegion);
				//hostControl.Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText | VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.ForwardKeyEvents;
				return new VirtualTreeLabelEditData(hostControl);
			}
			// UNDONE: Deferring ImmediateSelection from the control sends in the
				// ImmediateSelection value twice to activationStyle
				//else if (0 != (activationStyle | VirtualTreeLabelEditActivationStyles.ImmediateSelection))
				//{
				//	return VirtualTreeLabelEditData.DeferActivation;
				//}
			else if (row == myCustomReferenceModesList.Count)
			{
				VirtualTreeLabelEditData newRow = VirtualTreeLabelEditData.Default;
				newRow.AlternateText = "";
				return newRow;
			}
			else
			{
				
				return (IsColEditable(column)) ? VirtualTreeLabelEditData.Default : VirtualTreeLabelEditData.Invalid;
			}
		}

		private bool IsColEditable(int col)
		{
			foreach (Columns i in myEditable)
			{
				if ((int)i == col)
				{
					return true;
				}
			}
			return false;
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
								myCustomReferenceModesList[row].Remove();
							}
							if (t.HasPendingChanges)
							{
								t.Commit();
							}
						}
						break;
					case Columns.FormatString:
						string entityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorEntityTypeName + "}";
						string referenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorReferenceModeName + "}";
						string abbreviatedEntityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorAbbreviatedEntityTypeName + "}";
						string abbreviatedReferenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorAbbreviatedReferenceModeName + "}";


						newText = newText.Replace(abbreviatedReferenceModeName, referenceModeName).Replace(abbreviatedEntityTypeName, entityTypeName);
						if ((newText.IndexOf(referenceModeName) == -1 ||
							 newText.IndexOf(referenceModeName) != newText.LastIndexOf(referenceModeName) ||
							 newText.IndexOf(entityTypeName) != newText.LastIndexOf(entityTypeName)) &&
							(newText.Length >0))
						{
							return LabelEditResult.CancelEdit;
						}

						string changeFormatStringTransaction = ResourceStrings.ModelReferenceModeEditorChangeFormatStringTransaction;
						using (Transaction  t = myStore.TransactionManager.BeginTransaction(changeFormatStringTransaction))
						{
							myCustomReferenceModesList[row].CustomFormatString = this.UglyFormatString(newText);
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
					CustomReferenceMode newCustomReferenceMode = CustomReferenceMode.CreateAndInitializeCustomReferenceMode(this.myStore, new AttributeAssignment[] { new AttributeAssignment(CustomReferenceMode.NameMetaAttributeGuid, newText, this.myStore) });
					newCustomReferenceMode.Model = this.myModel;
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
			// UNDONE: Deferring ImmediateSelection from the control sends in the
			// ImmediateSelection value twice to activationStyle. Would like to
			// add BranchFeatures.ImmediateSelectionLabelEdits when/if this is fixed.
			get { return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.JaggedColumns | BranchFeatures.InsertsAndDeletes; }
		}


		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
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
			return new VirtualTreeDisplayData(-1);
		}

		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return null;
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
							return PrettyFormatString(myCustomReferenceModesList[row].FormatString);
						case Columns.ReferenceModeKind:
							if (myCustomReferenceModesList[row].Kind != null)
							{
								return myCustomReferenceModesList[row].Kind.ReferenceModeType.ToString();
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

		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return null;
		}

		bool IBranch.IsExpandable(int row, int column)
		{
			return false;
		}

		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			LocateObjectData empty;
			return empty;
		}

		private BranchModificationEventHandler myModify;
		event BranchModificationEventHandler IBranch.OnBranchModification
		{
			add { myModify += value; }
			remove { myModify -= value; }
		}


		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
		}

		void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{

		}

		void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{

		}

		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return VirtualTreeStartDragData.Empty;
		}

		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return StateRefreshChanges.None;
		}
		int IBranch.UpdateCounter
		{
			get { return 0; }
		}

		int IBranch.VisibleItemCount
		{
			get { return myCustomReferenceModesList.Count + 1; }
		}
		#endregion //IBranch Members
	}
}