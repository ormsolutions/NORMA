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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class ORMReferenceModeEditorToolWindow
	{
		partial class ReferenceModeViewForm
		{
			#region BaseBranch class
			/// <summary>
			/// A helper class to provide a default IBranch implementation
			/// </summary>
			private abstract class BaseBranch : IBranch
			{
				#region IBranch Implementation
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
					get
					{
						return BranchFeatures.None;
					}
				}
				VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
				{
					return VirtualTreeAccessibilityData.Empty;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					return VirtualTreeDisplayData.Empty;
				}
				object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
				{
					Debug.Fail("Should override.");
					return null;
				}
				string IBranch.GetText(int row, int column)
				{
					Debug.Fail("Should override.");
					return null;
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
				event BranchModificationEventHandler IBranch.OnBranchModification
				{
					add { }
					remove { }
				}
				void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
				{
				}
				void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
				{
				}
				void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
				{
				}
				VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
				{
					return VirtualTreeStartDragData.Empty;
				}
				StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
				{
					return StateRefreshChanges.None;
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
					get
					{
						Debug.Fail("Should override");
						return 0;
					}
				}
				#endregion // IBranch Implementation
			}
			#endregion // BaseBranch class
			/// <summary>
			/// Represents the Reference Mode Header Branch
			/// </summary>
			private sealed partial class ReferenceModeHeaderBranch : BaseBranch, IBranch
			{
				#region Shared by all branches
				private enum Columns
				{
					Name = 0,
					ReferenceModeKind = 1,
					FormatString = 2,
					Last = FormatString,
				}
				/// <summary>
				/// Replaces the {0} and {1} with entityTypeName and referenceModeName
				/// </summary>
				/// <param name="mode">The <see cref="ReferenceMode"/> to format</param>
				/// <param name="forEditing">True to replace the reference mode name with the editable form of the name.
				/// Otherwise, use the reference mode name</param>
				private static string PrettyFormatString(ReferenceMode mode, bool forEditing)
				{
					return string.Format(mode.FormatString, ResourceStrings.ModelReferenceModeEditorEntityTypeName, forEditing ? ResourceStrings.ModelReferenceModeEditorReferenceModeName : mode.Name);
				}
				/// <summary>
				/// Replaces the {0} and {1} with entityTypeName and referenceModeName
				/// </summary>
				/// <param name="kind">The <see cref="ReferenceMode"/> to format</param>
				private static string PrettyFormatString(ReferenceModeKind kind)
				{
					return string.Format(kind.FormatString, ResourceStrings.ModelReferenceModeEditorEntityTypeName, ResourceStrings.ModelReferenceModeEditorReferenceModeName);
				}
				/// <summary>
				/// Replaces the {0} and {1} with entityTypeName and referenceModeName
				/// </summary>
				/// <param name="prettyFormatString"></param>
				/// <returns></returns>
				private static string UglyFormatString(string prettyFormatString)
				{
					return ReplaceInsensitive(ReplaceInsensitive(ReplaceInsensitive(ReplaceInsensitive(prettyFormatString,
						ResourceStrings.ModelReferenceModeEditorAbbreviatedEntityTypeName, "{0}"),
						ResourceStrings.ModelReferenceModeEditorAbbreviatedReferenceModeName, "{1}"),
						ResourceStrings.ModelReferenceModeEditorEntityTypeName, "{0}"),
						ResourceStrings.ModelReferenceModeEditorReferenceModeName, "{1}");
				}
				private static string ReplaceInsensitive(string value, string find, string replaceWith)
				{
					int index = value.IndexOf(find, StringComparison.CurrentCultureIgnoreCase);
					if (index == 0)
					{
						return replaceWith + value.Substring(find.Length);
					}
					else if (index > 0)
					{
						return value.Substring(0, index) + replaceWith + value.Substring(find.Length + index);
					}
					return value;
				}
				#region MultiColumnBaseBranch class
				private abstract class MultiColumnBaseBranch : BaseBranch, IMultiColumnBranch
				{
					#region IMultiColumnBranch Implementation
					int IMultiColumnBranch.ColumnCount
					{
						get { return (int)Columns.Last + 1; }
					}
					SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
					{
						return SubItemCellStyles.Simple;
					}

					int IMultiColumnBranch.GetJaggedColumnCount(int row)
					{
						return (int)Columns.Last + 1;
					}
					#endregion // IMultiColumnBaseBranch Implementation
				}
				#endregion // MultiColumnBaseBranch class
				#endregion // Shared by all branches
				private ReferenceModeKindsBranch myReferenceModeKindsBranch;
				private CustomReferenceModesBranch myCustomBranch;
				private IntrinsicReferenceModesBranch myIntrinsicBranch;

				private enum Headers
				{
					ReferenceModeKinds = 0,
					CustomReferenceModes = 1,
					IntrinsicReferenceModes = 2
				}

				private static readonly string myIntrinsicReferenceModesHeader = ResourceStrings.ModelReferenceModeEditorIntrinsicReferenceModesHeader;
				private static readonly string myCustomReferenceModesHeader = ResourceStrings.ModelReferenceModeEditorCustomReferenceModesHeader;
				private static readonly string myReferenceModeKindHeader = ResourceStrings.ModelReferenceModeEditorReferenceModeKindHeader;

				private static readonly string[] myHeaderNames = { myReferenceModeKindHeader, myCustomReferenceModesHeader, myIntrinsicReferenceModesHeader };

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
				#region IBranch Implementation
				BranchFeatures IBranch.Features
				{
					get { return BranchFeatures.Expansions; }
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

							case Headers.IntrinsicReferenceModes:
								return myIntrinsicBranch;
						}
					}
					return null;
				}
				string IBranch.GetText(int row, int column)
				{
					return myHeaderNames[row];
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
							Debug.Fail("Shouldn't be here");
							return new LocateObjectData();
					}
				}
				int IBranch.VisibleItemCount
				{
					get { return myHeaderNames.Length; }
				}
				#endregion // IBranch Implementation
			}
		}
	}
}
