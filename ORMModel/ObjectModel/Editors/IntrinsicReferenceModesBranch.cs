#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM.ObjectModel;  
#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// The Intrinsic Reference Modes Branch on the tree
	/// </summary>
	public class IntrinsicReferenceModesBranch : IBranch, IMultiColumnBranch
	{
		#region Locals
		private enum Columns
		{
			Name = 0,
			ReferenceModeKind = 1,
			FormatString = 2
		}

		private List<IntrinsicReferenceMode> myIntrinsicReferenceModesList = new List<IntrinsicReferenceMode>();
		private static int myNumCols = Enum.GetValues(typeof(Columns)).Length;
		private Columns[] myEditable = new Columns[]{};
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
			if (model != null)
			{
				this.myModel = model;
				if (this.myStore != null && this.myStore != model.Store && !myStore.Disposed)
				{
					this.RemoveStoreEvents(model.Store);
				}
				if (this.myStore != model.Store)
				{
					this.AddStoreEvents(model.Store);

				}
				this.myStore = model.Store;
				int count = myIntrinsicReferenceModesList.Count;
				this.myIntrinsicReferenceModesList.Clear();
				if (myModify != null && count != 0)
				{
					myModify(this, BranchModificationEventArgs.DeleteItems(this, 0, count));
				}
				foreach (ReferenceMode mode in model.ReferenceModeCollection)
				{
					IntrinsicReferenceMode intMode = mode as IntrinsicReferenceMode;
					if (intMode != null)
					{
						this.myIntrinsicReferenceModesList.Add(intMode);
					}
				}
				myIntrinsicReferenceModesList.Sort();
				count = myIntrinsicReferenceModesList.Count;
				if (myModify != null && count != 0)
				{
					myModify(this, BranchModificationEventArgs.InsertItems(this, -1, count));
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

			if (referenceModeKind != null && referenceModeKind.Model == this.myModel)
			{
				foreach (ReferenceMode refMode in referenceModeKind.ReferenceModeCollection)
				{
					if (refMode is IntrinsicReferenceMode)
					{
						if (myModify != null)
						{
							int row = this.FindReferenceMode((IntrinsicReferenceMode)refMode);
							//This forces the whole control to redraw and pick up the refmode kind format string changes
							myModify(this, BranchModificationEventArgs.Redraw(false));
							myModify(this, BranchModificationEventArgs.Redraw(true));
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
		/// Add events to the store during connect action
		/// activation. The default implementation watches for
		/// new external constraints added to the diagram.
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void AddStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo referenceModeKindClassInfo = dataDirectory.FindMetaClass(ReferenceModeKind.MetaClassGuid);
			eventManager.ElementAttributeChanged.Add(referenceModeKindClassInfo, new ElementAttributeChangedEventHandler(ReferenceModeKindChangeEvent));
		}
		/// <summary>
		/// Removed any events added during the AddStoreEvents methods
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void RemoveStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo referenceModeKindClassInfo = dataDirectory.FindMetaClass(ReferenceModeKind.MetaClassGuid);
			eventManager.ElementAttributeChanged.Remove(referenceModeKindClassInfo, new ElementAttributeChangedEventHandler(ReferenceModeKindChangeEvent));
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
			if (row == myIntrinsicReferenceModesList.Count)
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
			return (IsColEditable(column)) ? VirtualTreeLabelEditData.Default : VirtualTreeLabelEditData.Invalid;
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
			return LabelEditResult.CancelEdit;			
		}
		BranchFeatures IBranch.Features
		{
			get { return BranchFeatures.DelayedLabelEdits | BranchFeatures.InsertsAndDeletes; }
		}


		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
		}

		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			if (row == myIntrinsicReferenceModesList.Count && column == 0)
			{
				VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
				retVal.ForeColor = SystemColors.GrayText;
				return retVal;
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
				if (row < myIntrinsicReferenceModesList.Count)
				{
					switch ((Columns)column)
					{
						case Columns.Name:
							return myIntrinsicReferenceModesList[row].Name;
						case Columns.FormatString:
							return  PrettyFormatString(myIntrinsicReferenceModesList[row].Kind.FormatString);
						case Columns.ReferenceModeKind:
							return myIntrinsicReferenceModesList[row].Kind.ReferenceModeType.ToString();
						default:
							return null;
					}
				}
				else
				{
					return null;
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
			get { return myIntrinsicReferenceModesList.Count; }
		}
		#endregion //IBranch Members
	}
}