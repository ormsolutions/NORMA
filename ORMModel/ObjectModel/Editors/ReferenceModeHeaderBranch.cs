#region Using directives
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid; 
#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// Represents the Reference Mode Header Branch
	/// </summary>
	public class ReferenceModeHeaderBranch : IBranch 
	{
		private ReferenceModeKindsBranch myReferenceModeKindsBranch;
		private CustomReferenceModesBranch myCustomBranch;
		private IntrinsicReferenceModesBranch myIntrinsicBranch;

		private enum Headers
		{
			ReferenceModeKinds = 0,
			CustomReferenceModes = 1,
			IntrinsicReferenceModes = 2
		}

		private static string myIntrinsicReferenceModesHeader = ResourceStrings.ModelReferenceModeEditorIntrinsicReferenceModesHeader;
		private static string myCustomReferenceModesHeader = ResourceStrings.ModelReferenceModeEditorCustomReferenceModesHeader;
		private static string myReferenceModeKindHeader = ResourceStrings.ModelReferenceModeEditorReferenceModeKindHeader;

		private string[] myHeaderNames = { myReferenceModeKindHeader, myCustomReferenceModesHeader, myIntrinsicReferenceModesHeader }; 

		/// <summary>
		/// Default constructor
		/// </summary>
		public ReferenceModeHeaderBranch()
		{
			this.myReferenceModeKindsBranch = new ReferenceModeKindsBranch();
			myCustomBranch = new CustomReferenceModesBranch();
			myIntrinsicBranch = new IntrinsicReferenceModesBranch();			
		}

		/// <summary>
		/// Sets the reference modes for the class
		/// </summary>
		/// <param name="model"></param>
		public void SetModel(ORMModel model)
		{
			myCustomBranch.SetModel(model);
			myIntrinsicBranch.SetModel(model);
			myReferenceModeKindsBranch.SetModel(model);
		}
		#region IBranch Members
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return VirtualTreeLabelEditData.Invalid;
		}
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return LabelEditResult.CancelEdit;
		}
		BranchFeatures IBranch.Features
		{
			get { return BranchFeatures.Expansions | BranchFeatures.Realigns; }
		}
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
		}
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
			retVal.BackColor = SystemColors.ControlLight;
			return retVal;
		}

		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			if (style == ObjectStyle.ExpandedBranch)
			{
				switch ((Headers)row)
				{
					case Headers.ReferenceModeKinds:
						return this.myReferenceModeKindsBranch;

					case Headers.CustomReferenceModes:			
						return this.myCustomBranch;

					case Headers.IntrinsicReferenceModes :						
						return myIntrinsicBranch;
				}
			}
			return null;
		}
		string IBranch.GetText(int row, int column)
		{
			return myHeaderNames[row];
		}
		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return null;
		}
		bool IBranch.IsExpandable(int row, int column)
		{
			return true;
		}
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			switch (style)
			{
				case ObjectStyle.ExpandedBranch:
					if (obj is ReferenceModeKindsBranch)
					{
						return new LocateObjectData(0, 0, (int)BranchLocationAction.KeepBranch);
					}
					return new LocateObjectData(0, 0, (int)BranchLocationAction.DiscardBranch);
				default:
					Debug.Assert(false); // Shouldn't be here
					return new LocateObjectData();
			}
		}
		event BranchModificationEventHandler IBranch.OnBranchModification
		{
			add { }
			remove { }
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
			get
			{
				return 0;
			}
		}
		int IBranch.VisibleItemCount
		{
			get { return myHeaderNames.Length; }
		}
		#endregion
	}
}
