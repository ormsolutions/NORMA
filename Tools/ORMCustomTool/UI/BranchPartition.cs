using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		#region BranchPartitionSection structure
		/// <summary>
		/// A single partition section
		/// </summary>
		private struct BranchPartitionSection
		{
			private int myStart;
			private int myCount;
			private string myHeader;
			/// <summary>
			/// Create a new partition section
			/// </summary>
			/// <param name="start">The starting index in the inner branch</param>
			/// <param name="count">The number of items in the inner branch to include in this branch.</param>
			/// <param name="header">A header string for this group of items. It the header is not null, then this item will expand to a branch and have a length of 1.</param>
			public BranchPartitionSection(int start, int count, string header)
			{
				myStart = start;
				myCount = count;
				myHeader = header;
			}
			/// <summary>
			/// The starting index in the inner branch
			/// </summary>
			public int Start
			{
				get
				{
					return myStart;
				}
			}
			/// <summary>
			/// The number of items to use in this branch
			/// </summary>
			public int Count
			{
				get
				{
					return myCount;
				}
			}
			/// <summary>
			/// A header for this section of items
			/// </summary>
			public string Header
			{
				get
				{
					return myHeader;
				}
			}
			public int VisibleItemCount
			{
				get
				{
					int retVal = myCount;
					return (myHeader == null) ? retVal : ((retVal == 0) ? 0 : 1);
				}
			}
		}
		#endregion // BranchPartitionSection structure
		private sealed class BranchPartition : IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly IBranch myInnerBranch;
			private readonly IMultiColumnBranch myInnerMultiColumnBranch;
			private readonly int[] myIndexer;
			private readonly BranchPartitionSection[] mySections;
			private readonly int myItemCount;
			private readonly BranchFeatures myFeatures;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Partition a static IBranch instance
			/// </summary>
			/// <param name="innerBranch">The branch to partition</param>
			/// <param name="indexer">An array of indices to reorder the items in the branch (optional)</param>
			/// <param name="sections">One or more PartitionSection structures indicating how the branch should be split up. If indexer is specified, then the Start and Count properties of the partition should be relative to the indexer, not the starting branch.</param>
			public BranchPartition(IBranch innerBranch, int[] indexer, params BranchPartitionSection[] sections)
			{
				myInnerBranch = innerBranch;
				myIndexer = indexer;
				mySections = sections;
				int sectionCount = sections.Length;
				int totalCount = 0;
				myInnerMultiColumnBranch = innerBranch as IMultiColumnBranch;
				BranchFeatures requiredFeatures = 0;
				for (int i = 0; i < sectionCount; ++i)
				{
					BranchPartitionSection currentSection = sections[i];
					int visibleCount = sections[i].VisibleItemCount;
					if (visibleCount != 0)
					{
						if (currentSection.Header != null && requiredFeatures == 0)
						{
							requiredFeatures = BranchFeatures.Expansions | ((myInnerMultiColumnBranch != null) ? BranchFeatures.JaggedColumns : 0);
						}
						totalCount += visibleCount;
					}
				}
				myItemCount = totalCount;
				myFeatures = requiredFeatures;
			}
			#endregion // Constructors
			#region Index translation
			/// <summary>
			/// Translate an incoming row to a row on the inner branch
			/// </summary>
			/// <param name="row">The incoming row for this branch</param>
			/// <param name="section">The corresponding partion section</param>
			/// <returns>A translated row, or -1 for a section header.</returns>
			private int TranslateRow(int row, out BranchPartitionSection section)
			{
				BranchPartitionSection[] sections = mySections;
				int sectionCount = sections.Length;
				for (int i = 0; i < sectionCount; ++i)
				{
					BranchPartitionSection currentSection = sections[i];
					int visibleOnThisSection = currentSection.VisibleItemCount;
					if (row < visibleOnThisSection)
					{
						section = currentSection;
						if (currentSection.Header != null)
						{
							return -1;
						}
						row = currentSection.Start + row;
						int[] indexer = myIndexer;
						if (indexer != null)
						{
							row = indexer[row];
						}
						return row;
					}
					row -= visibleOnThisSection;
				}
				throw new ArgumentOutOfRangeException();
			}
			#endregion // Index translation
			#region IBranch Members
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return VirtualTreeLabelEditData.Invalid;
				}
				return myInnerBranch.BeginLabelEdit(row, column, activationStyle);
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return LabelEditResult.CancelEdit;
				}
				return myInnerBranch.CommitLabelEdit(row, column, newText);
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return myInnerBranch.Features | myFeatures;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return VirtualTreeAccessibilityData.Empty;
				}
				return myInnerBranch.GetAccessibilityData(row, column);
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
					displayData.GrayText = true;
					return displayData;
				}
				return myInnerBranch.GetDisplayData(row, column, requiredData);
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					if (style == ObjectStyle.ExpandedBranch)
					{
						options = 0;
						return new BranchPartition(myInnerBranch, myIndexer, new BranchPartitionSection(section.Start, section.Count, null));
					}
					return null;
				}
				return myInnerBranch.GetObject(row, column, style, ref options);
			}
			string IBranch.GetText(int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return section.Header;
				}
				return myInnerBranch.GetText(row, column);
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return null;
				}
				return myInnerBranch.GetTipText(row, column, tipType);
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return true;
				}
				return ((myInnerBranch.Features & BranchFeatures.Expansions) == 0) ? false : myInnerBranch.IsExpandable(row, column);
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				Debug.Assert(false);
				// UNDONE: This would require an UntranslateRow to do correctly, not worth it for now
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add
				{
					myInnerBranch.OnBranchModification += value;
				}
				remove
				{
					myInnerBranch.OnBranchModification -= value;
				}
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					args.Effect = DragDropEffects.None;
				}
				myInnerBranch.OnDragEvent(sender, row, column, eventType, args);
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					args.UseDefaultCursors = true;
				}
				myInnerBranch.OnGiveFeedback(args, row, column);
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					args.Action = DragAction.Cancel;
				}
				myInnerBranch.OnQueryContinueDrag(args, row, column);
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return VirtualTreeStartDragData.Empty;
				}
				return myInnerBranch.OnStartDrag(sender, row, column, reason);
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return StateRefreshChanges.None;
				}
				return myInnerBranch.SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return StateRefreshChanges.None;
				}
				return myInnerBranch.ToggleState(row, column);
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return myInnerBranch.UpdateCounter;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return myItemCount;
				}
			}
			#endregion // IBranch Implementation
			#region IMultiColumnBranch Implementation
			int IMultiColumnBranch.ColumnCount
			{
				get
				{
					IMultiColumnBranch inner = myInnerMultiColumnBranch;
					return (inner != null) ? inner.ColumnCount : 1;
				}
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				IMultiColumnBranch inner = myInnerMultiColumnBranch;
				return (inner != null) ? inner.ColumnStyles(column) : SubItemCellStyles.Simple;
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				IMultiColumnBranch inner = myInnerMultiColumnBranch;
				if (inner == null)
				{
					return 1;
				}
				BranchPartitionSection section;
				row = TranslateRow(row, out section);
				if (row == -1)
				{
					return 1;
				}
				return ((myInnerBranch.Features & BranchFeatures.JaggedColumns) == 0) ? inner.ColumnCount : inner.GetJaggedColumnCount(row);
			}
			#endregion // IMultiColumnBranch Implementation
		}
	}
}
