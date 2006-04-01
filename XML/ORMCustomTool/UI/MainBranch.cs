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
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch : BranchBase
		{
			public MainBranch(ORMGeneratorSelectionControl parent)
			{
				this._parent = parent;
				SortedList<string, OutputFormatBranch> branches = this._branches = new SortedList<string, OutputFormatBranch>(StringComparer.OrdinalIgnoreCase);
				foreach (IORMGenerator ormGenerator in ORMCustomTool.ORMGenerators.Values)
				{
					string outputFormatName = ormGenerator.ProvidesOutputFormat;
					OutputFormatBranch outputFormatBranch;
					if (!branches.TryGetValue(outputFormatName, out outputFormatBranch))
					{
						outputFormatBranch = new OutputFormatBranch(this);
						branches.Add(outputFormatName, outputFormatBranch);
					}
					outputFormatBranch.ORMGenerators.Add(ormGenerator);
				}
			}

			private readonly ORMGeneratorSelectionControl _parent;
			private ORMGeneratorSelectionControl Parent
			{
				get
				{
					return this._parent;
				}
			}

			private readonly SortedList<string, OutputFormatBranch> _branches;
			public IDictionary<string, OutputFormatBranch> Branches
			{
				get
				{
					return this._branches;
				}
			}

			public override object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					System.Diagnostics.Debug.Assert(row >= 0 && row < this._branches.Count);
					return this._branches.Values[row];
				}
				return base.GetObject(row, column, style, ref options);
			}

			public override string GetText(int row, int column)
			{
				System.Diagnostics.Debug.Assert(row >= 0 && row < this._branches.Count);
				return this._branches.Keys[row];
			}
			public override string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (tipType == ToolTipType.StateIcon)
				{
					OutputFormatBranch currentBranch = _branches.Values[row];
					IORMGenerator useGenerator = currentBranch.SelectedORMGenerator;
					if (useGenerator == null)
					{
						useGenerator = currentBranch.ORMGenerators[0];
					}
					return useGenerator.DisplayDescription;
				}
				return base.GetTipText(row, column, tipType);
			}
			public override VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
				displayData.BackColor = System.Drawing.SystemColors.ControlLight;
				if (_branches.Values[row].SelectedORMGenerator != null)
				{
					displayData.StateImageIndex = (short)StandardCheckBoxImage.CheckedDisabled;
				}
				else
				{
					displayData.StateImageIndex = (short)StandardCheckBoxImage.Unchecked;
				}
				return displayData;
			}

			public override int VisibleItemCount
			{
				get
				{
					return this.Branches.Count;
				}
			}
			public override StateRefreshChanges ToggleState(int row, int column)
			{
				return ToggleOnRequiredBranches(_branches.Values[row]);
			}
			private StateRefreshChanges ToggleOnRequiredBranches(OutputFormatBranch formatBranch)
			{
				StateRefreshChanges retVal = StateRefreshChanges.None;
				if (formatBranch.SelectedORMGenerator == null)
				{
					retVal = StateRefreshChanges.Current | StateRefreshChanges.Children;
					IORMGenerator useGenerator = formatBranch.ORMGenerators[0];
					_parent.BuildItemsByGenerator[useGenerator.OfficialName] = useGenerator.AddGeneratedFileBuildItem(_parent._buildItemGroup, _parent._sourceFileName, null);
					formatBranch.SelectedORMGenerator = useGenerator;
					IList<string> requiredFormats = useGenerator.RequiresInputFormats;
					int requiredFormatsCount = requiredFormats.Count;
					for (int i = 0; i < requiredFormatsCount; ++i)
					{
						OutputFormatBranch requiredBranch;
						if (_branches.TryGetValue(requiredFormats[i], out requiredBranch))
						{
							if (StateRefreshChanges.None != ToggleOnRequiredBranches(requiredBranch))
							{
								retVal = StateRefreshChanges.Entire;
							}
						}
					}
				}
				return retVal;
			}
			public override BranchFeatures Features
			{
				get
				{
					return BranchFeatures.Expansions | BranchFeatures.StateChanges;
				}
			}

			public override bool IsExpandable(int row, int column)
			{
				return true;
			}
		}
	}
}
