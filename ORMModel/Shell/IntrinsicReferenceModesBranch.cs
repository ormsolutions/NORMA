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

namespace Neumont.Tools.ORM.Shell
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
			if (model != myModel)
			{
				Store newStore = (model == null) ? null : model.Store;
				if (myStore != null && myStore != newStore && !myStore.Disposed)
				{
					this.RemoveStoreEvents(myStore);
				}
				if (newStore != null && newStore != myStore)
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
		/// Replacement string to prettify the {0} numeric placeholder fields in a format string
		/// </summary>
		private static readonly string EntityTypeNameReplacement = string.Concat("{", ResourceStrings.ModelReferenceModeEditorEntityTypeName, "}");
		/// <summary>
		/// Replacement string to prettify the {1} numeric placeholder fields in a format string
		/// </summary>
		private static readonly string ReferenceModeNameReplacement = string.Concat("{", ResourceStrings.ModelReferenceModeEditorReferenceModeName, "}");
		/// <summary>
		/// Replaces the {0} and {1} with entityTypeName and referenceModeName
		/// </summary>
		/// <param name="uglyFormatString"></param>
		/// <returns></returns>
		private static string PrettyFormatString(string uglyFormatString)
		{
			return uglyFormatString.Replace("{0}", EntityTypeNameReplacement).Replace("{1}", ReferenceModeNameReplacement);
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
		/// Add events to the store during connect action
		/// activation. The default implementation watches for
		/// new external constraints added to the diagram.
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void AddStoreEvents(Store store)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			DomainClassInfo referenceModeKindClassInfo = dataDirectory.FindDomainClass(ReferenceModeKind.DomainClassId);
			eventManager.ElementPropertyChanged.Add(referenceModeKindClassInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindChangeEvent));
		}
		/// <summary>
		/// Removed any events added during the AddStoreEvents methods
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void RemoveStoreEvents(Store store)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			DomainClassInfo referenceModeKindClassInfo = dataDirectory.FindDomainClass(ReferenceModeKind.DomainClassId);
			eventManager.ElementPropertyChanged.Remove(referenceModeKindClassInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindChangeEvent));
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
			return default(LocateObjectData);
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
		StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
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
