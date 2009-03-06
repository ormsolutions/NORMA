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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.Build.BuildEngine;
using EnvDTE;
using System.IO;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch : BranchBase, IMultiColumnBranch
		{
			private static class ColumnNumber
			{
				public const int GeneratedFormat = 0;
				public const int GeneratedFileName = 1;
			}
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

			public IEnumerable<IORMGenerator> SelectedGenerators
			{
				get
				{
					foreach (OutputFormatBranch branch in _branches.Values)
					{
						IORMGenerator selectedGenerator = branch.SelectedORMGenerator;
						if (selectedGenerator != null)
						{
							yield return selectedGenerator;
						}
					}
				}
			}

			public bool IsPrimaryDisplayItem(int index)
			{
				OutputFormatBranch branch = _branches.Values[index];
				IList<IORMGenerator> generators = branch.ORMGenerators;
				int generatorCount = generators.Count;
				bool retVal = true;
				for (int i = 0; i < generatorCount; ++i)
				{
					if (generators[i].GeneratesSupportFile)
					{
						retVal = false;
						break;
					}
				}
				return retVal;
			}

			public override object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					return this._branches.Values[row];
				}
				return base.GetObject(row, column, style, ref options);
			}

			public override string GetText(int row, int column)
			{
				string retVal = null;
				switch (column)
				{
					case ColumnNumber.GeneratedFormat:
						retVal = _branches.Keys[row];
						break;
					case ColumnNumber.GeneratedFileName:
						{
							IORMGenerator selectedORMGenerator = this._branches.Values[row].SelectedORMGenerator;
							if (selectedORMGenerator != null)
							{
								retVal = _parent.BuildItemsByGenerator[selectedORMGenerator.OfficialName].FinalItemSpec;
							}
						}
						break;
				}
				return retVal;
			}
			public override string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (column == ColumnNumber.GeneratedFormat && tipType == ToolTipType.StateIcon)
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
				IORMGenerator selectedGenerator = _branches.Values[row].SelectedORMGenerator;
				switch (column)
				{
					case ColumnNumber.GeneratedFormat:
						if (selectedGenerator != null)
						{
							displayData.Bold = true;
						}
						if (0 != (requiredData.Mask & VirtualTreeDisplayMasks.StateImage))
						{
							if (selectedGenerator != null)
							{
								// Don't allow this to uncheck if another tool is using it
								displayData.StateImageIndex = (short)(CanRemoveGenerator(row) ? StandardCheckBoxImage.Checked : StandardCheckBoxImage.CheckedDisabled);
							}
							else
							{
								displayData.StateImageIndex = (short)StandardCheckBoxImage.Unchecked;
							}
						}
						break;
					case ColumnNumber.GeneratedFileName:
						displayData.GrayText = true;
						break;
				}
				return displayData;
			}
			/// <summary>
			/// Test if a generator for an item can be removed
			/// </summary>
			/// <param name="row">The row to test.</param>
			/// <returns>true if the generator can be removed without removing a prerequisite for another generator</returns>
			private bool CanRemoveGenerator(int row)
			{
				return CanRemoveGenerator(_branches.Values[row]);
			}
			/// <summary>
			/// Test if a generator for an item can be removed
			/// </summary>
			/// <param name="branch">The branch to test.</param>
			/// <returns>true if the generator can be removed without removing a prerequisite for another generator</returns>
			private bool CanRemoveGenerator(OutputFormatBranch branch)
			{
				return !branch.IsDependency;
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
				return ToggleOnRequiredBranches(_branches.Values[row], 0);
			}
			private StateRefreshChanges ToggleOnRequiredBranches(OutputFormatBranch formatBranch, int branchGeneratorIndex)
			{
				return ToggleOnRequiredBranches(formatBranch, branchGeneratorIndex, true);
			}
			private StateRefreshChanges ToggleOnRequiredBranches(OutputFormatBranch formatBranch, int branchGeneratorIndex, bool testToggleOff)
			{
				StateRefreshChanges retVal = StateRefreshChanges.None;
				if (formatBranch.SelectedORMGenerator == null)
				{
					Microsoft.Build.BuildEngine.Project project = Parent._project;
					EnvDTE.ProjectItem projectItem = Parent._projectItem;

					string sourceFileName = _parent._sourceFileName;
					string projectItemPath = (string)projectItem.Properties.Item("LocalPath").Value;
					string projectPath = project.FullFileName;
					string newItemDirectory = (new Uri(projectPath)).MakeRelativeUri(new Uri(projectItemPath)).ToString();
					newItemDirectory = Path.GetDirectoryName(newItemDirectory);

					retVal = StateRefreshChanges.Current | StateRefreshChanges.Children;
					IORMGenerator useGenerator = formatBranch.ORMGenerators[branchGeneratorIndex];
					string outputFileName = useGenerator.GetOutputFileDefaultName(sourceFileName);
					outputFileName = Path.Combine(newItemDirectory, outputFileName);
					BuildItem newBuildItem = useGenerator.AddGeneratedFileBuildItem(_parent._buildItemGroup, sourceFileName, outputFileName); //string.Concat(newItemPath, Path.DirectorySeparatorChar, _parent._sourceFileName));
					_parent.BuildItemsByGenerator[useGenerator.OfficialName] = newBuildItem;
					_parent.RemoveRemovedItem(newBuildItem);
					formatBranch.SelectedORMGenerator = useGenerator;
					IList<string> requiredFormats = useGenerator.RequiresInputFormats;
					int requiredFormatsCount = requiredFormats.Count;
					for (int i = 0; i < requiredFormatsCount; ++i)
					{
						OutputFormatBranch requiredBranch;
						if (_branches.TryGetValue(requiredFormats[i], out requiredBranch))
						{
							if (StateRefreshChanges.None != ToggleOnRequiredBranches(requiredBranch, 0, false))
							{
								retVal = StateRefreshChanges.Entire;
							}
						}
					}
				}
				else if (testToggleOff && CanRemoveGenerator(formatBranch))
				{
					RemoveGenerator(formatBranch);
					retVal = StateRefreshChanges.Current | StateRefreshChanges.Children;
				}
				return retVal;
			}
			/// <summary>
			/// Remove the generator for this branch. Note that there is
			/// not checking here as to whether or not the generator is required
			/// in other places. Most uses will first call CanRemoveGenerator, but
			/// switching generators for a format will want to remove this blindly
			/// then toggle on the other format provider.
			/// </summary>
			/// <param name="formatBranch">The branch to remove from the generation process</param>
			private void RemoveGenerator(OutputFormatBranch formatBranch)
			{
				IORMGenerator removeGenerator = formatBranch.SelectedORMGenerator;
				IDictionary<string, BuildItem> buildItemsByGeneratorName = _parent.BuildItemsByGenerator;
				string generatorKey = removeGenerator.OfficialName;
				formatBranch.SelectedORMGenerator = null;
				BuildItem removeBuildItem = buildItemsByGeneratorName[generatorKey];
				buildItemsByGeneratorName.Remove(generatorKey);
				_parent._buildItemGroup.RemoveItem(removeBuildItem);
				_parent.AddRemovedItem(removeBuildItem);
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
			#region IMultiColumnBranch Members
			int IMultiColumnBranch.ColumnCount
			{
				get { return 2; }
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return SubItemCellStyles.Simple;
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				Debug.Assert(false, "Should not be called, the JaggedColumns feature is not turned on");
				return 2;
			}

			#endregion // IMultiColumnBranch Members
		}
	}
}
