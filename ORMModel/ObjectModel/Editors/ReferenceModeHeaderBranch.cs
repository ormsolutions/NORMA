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
		/// <summary>
		/// Implements IBranch.BeginLabelEdit
		/// </summary>
		protected VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return VirtualTreeLabelEditData.Invalid;
		}
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return BeginLabelEdit(row, column, activationStyle);
		}
		/// <summary>
		/// Implements IBranch.CommitLabelEdit
		/// </summary>
		protected LabelEditResult CommitLabelEdit(int row, int column, string newText)
		{
			return LabelEditResult.CancelEdit;
		}
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return CommitLabelEdit(row, column, newText);
		}
		/// <summary>
		/// Implements IBranch.Features
		/// </summary>
		protected BranchFeatures Features
		{
			get { return BranchFeatures.Expansions | BranchFeatures.Realigns; }
		}
		BranchFeatures IBranch.Features
		{
			get { return Features; }
		}
		/// <summary>
		/// Implements IBranch.GetAccssibilityData
		/// </summary>
		protected VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
		}
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return GetAccessibilityData(row, column);
		}
		/// <summary>
		/// Implements IBranch.GetDisplayData
		/// </summary>
		protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			VirtualTreeDisplayData retVal = new VirtualTreeDisplayData();
			retVal.BackColor = SystemColors.ControlLight;
			return retVal;
		}
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return GetDisplayData(row, column, requiredData);
		}
		/// <summary>
		/// Implements IBranch.GetObject
		/// </summary>
		protected object GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			if (style == ObjectStyle.ExpandedBranch)
			{
				switch ((Headers)row)
				{
					case Headers.ReferenceModeKinds:
						return this.myReferenceModeKindsBranch;

					case Headers.CustomReferenceModes:
						return this.myCustomBranch;

					case Headers.IntrinsicReferenceModes:
						return myIntrinsicBranch;
				}
			}
			return null;
		}
		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return GetObject(row, column, style, ref options);
		}
		/// <summary>
		/// Implements IBranch.GetText
		/// </summary>
		protected string GetText(int row, int column)
		{
			return myHeaderNames[row];
		}
		string IBranch.GetText(int row, int column)
		{
			return GetText(row, column);
		}
		/// <summary>
		/// Implements IBranch.GetTipText
		/// </summary>
		protected string GetTipText(int row, int column, ToolTipType tipType)
		{
			return null;
		}
		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return GetTipText(row, column, tipType);
		}
		/// <summary>
		/// Implements IBranch.IsExpandable
		/// </summary>
		protected bool IsExpandable(int row, int column)
		{
			return true;
		}
		bool IBranch.IsExpandable(int row, int column)
		{
			return IsExpandable(row, column);
		}
		/// <summary>
		/// Implements IBranch.LocateObject
		/// </summary>
		protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
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
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			return LocateObject(obj, style, locateOptions);
		}
		event BranchModificationEventHandler IBranch.OnBranchModification
		{
			add { }
			remove { }
		}
		/// <summary>
		/// Implements IBranch.OnDragEvent
		/// </summary>
		protected void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
		}
		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
			OnDragEvent(sender, row, column, eventType, args);
		}
		/// <summary>
		/// Implements IBranch.OnGiveFeedback
		/// </summary>
		protected void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
		}
		void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
			OnGiveFeedback(args, row, column);
		}
		/// <summary>
		/// Implements IBranch.OnQueryContinueDrag
		/// </summary>
		protected void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
		}
		void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
			OnQueryContinueDrag(args, row, column);
		}
		/// <summary>
		/// Implements IBranch.OnStartDrag
		/// </summary>
		protected VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return VirtualTreeStartDragData.Empty;
		}
		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return OnStartDrag(sender, row, column, reason);
		}
		/// <summary>
		/// Implements IBranch.ToggleState
		/// </summary>
		protected StateRefreshChanges ToggleState(int row, int column)
		{
			return StateRefreshChanges.None;
		}
		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return ToggleState(row, column);
		}
		/// <summary>
		/// Implements IBranch.UpdateCounter
		/// </summary>
		protected int UpdateCounter
		{
			get
			{
				return 0;
			}
		}
		int IBranch.UpdateCounter
		{
			get
			{
				return UpdateCounter;
			}
		}
		/// <summary>
		/// Implements IBranch.VisibleItemCount
		/// </summary>
		protected int VisibleItemCount
		{
			get { return myHeaderNames.Length; }
		}
		int IBranch.VisibleItemCount
		{
			get { return VisibleItemCount; }
		}
		#endregion
	}
}
