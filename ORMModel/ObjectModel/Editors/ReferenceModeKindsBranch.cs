#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Northface.Tools.ORM.ObjectModel; 
using Microsoft.VisualStudio.Modeling;
#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// Branch for all the differnt Reference mode kinds
	/// </summary>
	public class ReferenceModeKindsBranch : IBranch, IMultiColumnBranch
	{
		#region Locals
		private enum Columns
		{
			Empty = 0,
			ReferenceModeKind = 1,
			FormatString = 2
		}

		private System.Collections.Generic.List<ReferenceModeKind> myReferenceModeKindsList = new System.Collections.Generic.List<ReferenceModeKind>();
		private static int myNumCols = Enum.GetValues(typeof(Columns)).Length;
		private Columns[] myEditable = new Columns[] { Columns.FormatString };
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
			if (!object.ReferenceEquals(model, myModel))
			{
				Store newStore = (model == null) ? null : model.Store;
				if (myStore != null && !object.ReferenceEquals(myStore, newStore))
				{
					this.RemoveStoreEvents(myStore);
				}
				if (newStore != null && !object.ReferenceEquals(newStore, myStore))
				{
					this.AddStoreEvents(newStore);

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
				}
				count = myReferenceModeKindsList.Count;
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

			if (referenceModeKind != null && referenceModeKind.Model == this.myModel)
			{
				if (myModify != null)
				{
					int row = this.FindReferenceModeKind(referenceModeKind);
					//This forces the whole control to redraw and pick up the refmode kind format string changes
					myModify(this, BranchModificationEventArgs.Redraw(false));
					myModify(this, BranchModificationEventArgs.Redraw(true));
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
			if (row == myReferenceModeKindsList.Count) 
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
			return (IsColEditable(column))? VirtualTreeLabelEditData.Default : VirtualTreeLabelEditData.Invalid;
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
			Transaction t = null;
			try
			{
				switch ((Columns)column)
				{
					case Columns.Empty:
						return LabelEditResult.CancelEdit;
					case Columns.FormatString:
						string entityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorEntityTypeName + "}";
						string referenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorReferenceModeName + "}";
						string abbreviatedEntityTypeName = "{" + ResourceStrings.ModelReferenceModeEditorAbbreviatedEntityTypeName + "}";
						string abbreviatedReferenceModeName = "{" + ResourceStrings.ModelReferenceModeEditorAbbreviatedReferenceModeName + "}";


						newText = newText.Replace(abbreviatedReferenceModeName, referenceModeName).Replace(abbreviatedEntityTypeName, entityTypeName); 
						if (newText.IndexOf(referenceModeName) == -1 ||
							newText.IndexOf(referenceModeName) != newText.LastIndexOf(referenceModeName) ||
							newText.IndexOf(entityTypeName) != newText.LastIndexOf(entityTypeName) )
						{
							return LabelEditResult.CancelEdit;
						}

						string changeFormatStringTransaction = ResourceStrings.ModelReferenceModeEditorChangeFormatStringTransaction;
						using (t = myStore.TransactionManager.BeginTransaction(changeFormatStringTransaction))
						{
							myReferenceModeKindsList[row].FormatString = this.UglyFormatString(newText);
							if (t.HasPendingChanges)
							{
								t.Commit();
							}
						}
						break;
					case Columns.ReferenceModeKind:
						return LabelEditResult.CancelEdit;
				}
				return LabelEditResult.AcceptEdit;
			}
			catch
			{
				return LabelEditResult.CancelEdit;
			}
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
			if (row == myReferenceModeKindsList.Count && column == 0)
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
				switch ((Columns)column)
				{
					case Columns.Empty:
						return null;
					case Columns.FormatString:
						return PrettyFormatString(myReferenceModeKindsList[row].FormatString);
					case Columns.ReferenceModeKind:
						return myReferenceModeKindsList[row].ReferenceModeType.ToString();
					default :
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
			get { return myReferenceModeKindsList.Count; }
		}
		#endregion //IBranch Members
	}
}
