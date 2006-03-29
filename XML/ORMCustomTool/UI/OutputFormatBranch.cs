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
using System.Windows.Forms;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch
		{
			public sealed class OutputFormatBranch : BranchBase, IMultiColumnBranch
			{
				private enum Column
				{
					GeneratorName = 0,
					GeneratedFileName = 1
				}

				public OutputFormatBranch(MainBranch mainBranch)
				{
					this._mainBranch = mainBranch;
					// MainBranch populates this list for us...
					this._ormGenerators = new List<IORMGenerator>();
				}

				private IORMGenerator _selectedORMGenerator;
				public IORMGenerator SelectedORMGenerator
				{
					get
					{
						return this._selectedORMGenerator;
					}
					set
					{
						this._selectedORMGenerator = value;
					}
				}

				private readonly List<IORMGenerator> _ormGenerators;
				public IList<IORMGenerator> ORMGenerators
				{
					get
					{
						return this._ormGenerators;
					}
				}

				private readonly MainBranch _mainBranch;
				private MainBranch MainBranch
				{
					get
					{
						return this._mainBranch;
					}
				}


				public override string GetText(int row, int column)
				{
					if (column == (int)Column.GeneratorName)
					{
						return this.ORMGenerators[row].DisplayName;
					}
					else if (column == (int)Column.GeneratedFileName)
					{
						IORMGenerator selectedORMGenerator = this.SelectedORMGenerator;
						if (selectedORMGenerator == null)
						{
							// TODO: Localize this.
							return "Add...";
						}
						if (selectedORMGenerator == this.ORMGenerators[row])
						{
							return this.MainBranch.Parent.BuildItemsByGenerator[selectedORMGenerator.OfficialName].FinalItemSpec;
						}
						else 
						{
							// TODO: Eventually we may want to allow the user to switch which generator is selected for an output format...
							return null;
						}
					}
					else
					{
						return base.GetText(row, column);
					}
				}
				public override VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					if (this.SelectedORMGenerator != null)
					{
						// If the user has already selected a generator for this output format, gray out this section...
						VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
						displayData.GrayText = true;
						return displayData;
					}
					else
					{
						return base.GetDisplayData(row, column, requiredData);
					}
				}

				public override string GetTipText(int row, int column, ToolTipType tipType)
				{
					IORMGenerator selectedORMGenerator;
					if (column == (int)Column.GeneratorName)
					{
						return this.ORMGenerators[row].DisplayDescription;
					}
					else if (column == (int)Column.GeneratedFileName && (selectedORMGenerator = this.SelectedORMGenerator) == this.ORMGenerators[row])
					{
						return this.MainBranch.Parent.BuildItemsByGenerator[selectedORMGenerator.OfficialName].FinalItemSpec;
					}
					else
					{
						return base.GetTipText(row, column, tipType);
					}
				}

				public override int VisibleItemCount
				{
					get
					{
						return this.ORMGenerators.Count;
					}
				}

				#region IMultiColumnBranch Members

				public int ColumnCount
				{
					get { return 2; }
				}

				public SubItemCellStyles ColumnStyles(int column)
				{
					return SubItemCellStyles.Simple;
				}

				public int GetJaggedColumnCount(int row)
				{
					if (row == this.ORMGenerators.Count)
					{
						return 1;
					}
					else
					{
						return this.ColumnCount;
					}
				}

				#endregion // IMultiColumnBranch Members
			}
		}
	}
}
