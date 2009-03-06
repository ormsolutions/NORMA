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
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private abstract class BranchBase : IBranch
		{
			protected BranchBase()
			{
			}

			public virtual VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}

			public virtual LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}

			public virtual BranchFeatures Features
			{
				get { return BranchFeatures.None; }
			}

			public virtual VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}

			public virtual VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}

			public virtual object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return null;
			}

			public virtual string GetText(int row, int column)
			{
				return null;
			}

			public virtual string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.Default)
				{
					return null;
				}
				return "";
			}

			public virtual bool IsExpandable(int row, int column)
			{
				return false;
			}

			public virtual LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}

			private BranchModificationEventHandler _onBranchModification;
			public virtual event BranchModificationEventHandler OnBranchModification
			{
				add
				{
					this._onBranchModification = (BranchModificationEventHandler)Delegate.Combine(this._onBranchModification, value);
				}
				remove
				{
					this._onBranchModification = (BranchModificationEventHandler)Delegate.Remove(this._onBranchModification, value);
				}
			}

			public virtual void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}

			public virtual void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}

			public virtual void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}

			public virtual VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}

			public virtual StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}

			public virtual StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}

			public virtual int UpdateCounter
			{
				get { return 0; }
			}

			public virtual int VisibleItemCount
			{
				get { return 0; }
			}
		}
	}
}
