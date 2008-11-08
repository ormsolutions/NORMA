#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Reflection;
using System.Reflection.Emit;

namespace Neumont.Tools.Modeling.Shell
{
	/// <summary>
	/// A version of <see cref="MultiColumnTree"/> with overrides
	/// for common bug fixes.
	/// </summary>
	public class StandardMultiColumnTree : MultiColumnTree, ITree
	{
		#region BlankExpansionData construction
		// BlankExpansionData constructors are internal, create a dynamic method to create one
		private delegate BlankExpansionData BlankExpansionDataConstructorDelegate(int topRow, int leftColumn, int bottomRow, int rightColumn, int anchorColumn);
		private static readonly BlankExpansionDataConstructorDelegate BlankExpansionDataConstructor = CreateBlankExpansionDataConstructorDelegate();
		private static BlankExpansionDataConstructorDelegate CreateBlankExpansionDataConstructorDelegate()
		{
			Type expansionDataType = typeof(BlankExpansionData);
			Type intType = typeof(int);
			Type[] ctorParamTypes = new Type[] { intType, intType, intType, intType, intType };
			DynamicMethod dynamicMethod = new DynamicMethod("BlankExpansionDataConstructor", expansionDataType, ctorParamTypes, expansionDataType, true);
			ILGenerator il = dynamicMethod.GetILGenerator(24); // Final size is 16, give it some room
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Ldarg_S, 4);
			il.Emit(OpCodes.Newobj, expansionDataType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, ctorParamTypes, null));
			il.Emit(OpCodes.Ret);

			return (BlankExpansionDataConstructorDelegate)dynamicMethod.CreateDelegate(typeof(BlankExpansionDataConstructorDelegate));
		}
		#endregion // BlankExpansionData construction
		#region Constructor
		/// <summary>
		/// Create a new multi column tree
		/// </summary>
		public StandardMultiColumnTree(int columns)
			: base(columns)
		{
		}
		#endregion // Constructor
		#region ITree Overrides
		/// <summary>
		/// Reimplementation of <see cref="ITree.GetNavigationTarget"/> to fix issues with the Down navigation direction
		/// </summary>
		protected new VirtualTreeCoordinate GetNavigationTarget(TreeNavigation direction, int sourceRow, int sourceColumn, ColumnPermutation columnPermutation)
		{
			// UNDONE: MSBUG GetNavigationTarget calls the internal method to get its blank expansion, not
			// the interface, so we have no way to override it. Duplicate the GetNavigationTarget fix here for
			// the 'Down' navigation target.
			switch (direction)
			{
				case TreeNavigation.Down:
					BlankExpansionData blankExpansion = GetBlankExpansion(sourceRow, sourceColumn, columnPermutation);
					if (blankExpansion.Anchor.IsValid)
					{
						int lastRow = ((ITree)this).VisibleItemCount - 1;
						while (blankExpansion.BottomRow < lastRow)
						{
							// Just return the next non-blank column
							int testRow = blankExpansion.BottomRow + 1;
							blankExpansion = GetBlankExpansion(testRow, sourceColumn, columnPermutation);
							if (!blankExpansion.Anchor.IsValid)
							{
								break;
							}
							int topRow = blankExpansion.TopRow;
							if (sourceColumn == blankExpansion.AnchorColumn && topRow >= testRow)
							{
								return new VirtualTreeCoordinate(topRow, (columnPermutation != null) ? columnPermutation.GetPermutedColumn(sourceColumn) : sourceColumn);
							}
						}
					}
					return VirtualTreeCoordinate.Invalid;
				case TreeNavigation.LeftColumn:
					if (sourceColumn == 0)
					{
						// UNDONE: MSBUG Navigating LeftColumn from column 0 is crashing
						return VirtualTreeCoordinate.Invalid;
					}
					break;
			}
			return base.GetNavigationTarget(direction, sourceRow, sourceColumn, columnPermutation);
		}
		VirtualTreeCoordinate ITree.GetNavigationTarget(TreeNavigation direction, int sourceRow, int sourceColumn, ColumnPermutation columnPermutation)
		{
			return GetNavigationTarget(direction, sourceRow, sourceColumn, columnPermutation);
		}
		/// <summary>
		/// Reimplementation of <see cref="ITree.GetBlankExpansion"/>
		/// </summary>
		protected new BlankExpansionData GetBlankExpansion(int row, int column, ColumnPermutation columnPermutation)
		{
			// UNDONE: MSBUG GetBlankExpansion is returning a blank cell for the top node in a complex subitem. This
			// causes the Down arrow to skip any expanded subitems, which is obviously not good. This does not appear to
			// happen in all cases, and may be related to the SubItemCellStyle.
			BlankExpansionData retVal = base.GetBlankExpansion(row, column, columnPermutation);
			int topRow;
			if (retVal.Height > 1 &&
				row == (topRow = retVal.TopRow))
			{
				// If the next cell down does not return the same anchor, then the
				// initial information is incorrect and we really have a single cell.
				if (base.GetBlankExpansion(topRow + 1, column, columnPermutation).Anchor != retVal.Anchor)
				{
					return BlankExpansionDataConstructor(topRow, retVal.LeftColumn, retVal.TopRow, retVal.RightColumn, retVal.AnchorColumn);
				}
			}
			return retVal;
		}
		BlankExpansionData ITree.GetBlankExpansion(int row, int column, ColumnPermutation columnPermutation)
		{
			return GetBlankExpansion(row, column, columnPermutation);
		}
		#endregion // ITree Overrides
	}
}