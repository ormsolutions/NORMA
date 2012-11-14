#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using EnvDTE;
using System.IO;
#if VISUALSTUDIO_10_0
using Microsoft.Build.Construction;
#else
using Microsoft.Build.BuildEngine;
#endif

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch : BranchBase, IMultiColumnBranch
		{
			private readonly ORMGeneratorSelectionControl _parent;
			private readonly SortedList<string, OutputFormatBranch> _branches;
			private readonly List<OutputFormatBranch> _modifiers;
			private static class ColumnNumber
			{
				public const int GeneratedFormat = 0;
				public const int GeneratedFileName = 1;
			}
			private enum RowStyle
			{
				/// <summary>
				/// The row corresponds to a primary generator
				/// </summary>
				Generator,
				/// <summary>
				/// The row corresponds to a modifier
				/// </summary>
				Modifier,
			}
			private RowStyle TranslateRow(ref int row)
			{
				int branchCount = _branches.Count;
				if (row < branchCount)
				{
					return RowStyle.Generator;
				}
				row -= branchCount;
				return RowStyle.Modifier;
			}
			public MainBranch(ORMGeneratorSelectionControl parent)
			{
				SortedList<string, OutputFormatBranch> branches = new SortedList<string, OutputFormatBranch>(StringComparer.OrdinalIgnoreCase);
				List<OutputFormatBranch> modifiers = null;
				int modifierAsGeneratorCount = 0;
				foreach (IORMGenerator ormGenerator in ORMCustomTool.ORMGenerators.Values)
				{
					string outputFormatName = ormGenerator.ProvidesOutputFormat;
					OutputFormatBranch formatBranch;
					if (ormGenerator.IsFormatModifier)
					{
						// Track modifiers separately
						if (modifiers == null)
						{
							modifiers = new List<OutputFormatBranch>();
						}
						formatBranch = new OutputFormatBranch(this);
						formatBranch.ORMGenerators.Add(ormGenerator);
						OutputFormatBranch primaryFormatBranch;
						if (branches.TryGetValue(outputFormatName, out primaryFormatBranch))
						{
							if (primaryFormatBranch.IsModifier)
							{
								if (primaryFormatBranch.ORMGenerators[0].FormatModifierPriority <= ormGenerator.FormatModifierPriority)
								{
									primaryFormatBranch.NextModifier = formatBranch;
								}
								else
								{
									formatBranch.NextModifier = primaryFormatBranch;
									branches[outputFormatName] = formatBranch;
								}
							}
							else
							{
								primaryFormatBranch.NextModifier = formatBranch;
							}
						}
						else
						{
							// We don't have a branch yet for a primary generator, track
							// the modifier by adding it here. We will verify later that
							// all modifiers have a non-modifier primary generator.
							branches.Add(outputFormatName, formatBranch);
							++modifierAsGeneratorCount;
						}
						modifiers.Add(formatBranch);
					}
					else
					{
						if (branches.TryGetValue(outputFormatName, out formatBranch))
						{
							if (formatBranch.IsModifier)
							{
								OutputFormatBranch modifierBranch = formatBranch;
								formatBranch = new OutputFormatBranch(this);
								formatBranch.NextModifier = modifierBranch;
								branches[outputFormatName] = formatBranch;
								--modifierAsGeneratorCount;
							}
						}
						else
						{
							formatBranch = new OutputFormatBranch(this);
							branches.Add(outputFormatName, formatBranch);
						}
						formatBranch.ORMGenerators.Add(ormGenerator);
					}
				}
				if (modifierAsGeneratorCount != 0)
				{
					// A modifier with no associated generator does not
					// make sense, but is currently stored as a branch.
					// Pull it out of the list.
					for (int i = modifiers.Count - 1; i >= 0; --i)
					{
						OutputFormatBranch testBranch;
						string outputFormat = modifiers[i].ORMGenerators[0].ProvidesOutputFormat;
						if (!branches.TryGetValue(outputFormat, out testBranch) ||
							testBranch.IsModifier)
						{
							if (testBranch != null)
							{
								branches.Remove(outputFormat);
							}
							modifiers.RemoveAt(i);
						}
					}
				}
				_parent = parent;
				_branches = branches;
				int modifierCount;
				if (modifiers != null && 0 != (modifierCount = modifiers.Count))
				{
					if (modifierCount > 1)
					{
						modifiers.Sort(delegate(OutputFormatBranch left, OutputFormatBranch right)
						{
							return string.Compare(left.ORMGenerators[0].DisplayName, right.ORMGenerators[0].DisplayName);
						});
					}
					_modifiers = modifiers;
				}
			}

			private ORMGeneratorSelectionControl Parent
			{
				get
				{
					return this._parent;
				}
			}

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
					List<OutputFormatBranch> modifiers = _modifiers;
					if (modifiers != null)
					{
						foreach (OutputFormatBranch branch in _modifiers)
						{
							IORMGenerator selectedGenerator = branch.SelectedORMGenerator;
							if (selectedGenerator != null)
							{
								yield return selectedGenerator;
							}
						}
					}
				}
			}

			public bool IsPrimaryDisplayItem(int index)
			{
				switch (TranslateRow(ref index))
				{
					case RowStyle.Generator:
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
					default: // case RowStyle.Modifier:
						return false;
				}
			}

			public override object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					if (RowStyle.Generator == TranslateRow(ref row))
					{
						return this._branches.Values[row];
					}
				}
				return base.GetObject(row, column, style, ref options);
			}

			public override string GetText(int row, int column)
			{
				string retVal = null;
				switch (column)
				{
					case ColumnNumber.GeneratedFormat:
						switch (TranslateRow(ref row))
						{
							case RowStyle.Generator:
								retVal = _branches.Keys[row];
								break;
							case RowStyle.Modifier:
								retVal = _modifiers[row].ORMGenerators[0].DisplayName;
								break;
						}
						break;
					case ColumnNumber.GeneratedFileName:
						{
							IORMGenerator selectedORMGenerator = null;
							switch (TranslateRow(ref row))
							{
								case RowStyle.Generator:
									selectedORMGenerator = _branches.Values[row].SelectedORMGenerator;
									break;
								case RowStyle.Modifier:
									selectedORMGenerator = _modifiers[row].SelectedORMGenerator;
									if (selectedORMGenerator != null)
									{
										selectedORMGenerator = _branches[selectedORMGenerator.ProvidesOutputFormat].SelectedORMGenerator;
									}
									break;
							}
							if (selectedORMGenerator != null)
							{
								retVal = ORMCustomToolUtility.GetItemInclude(_parent.BuildItemsByGenerator[selectedORMGenerator.OfficialName]);
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
					OutputFormatBranch currentBranch = (TranslateRow(ref row) == RowStyle.Generator) ? _branches.Values[row] : _modifiers[row];
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
				bool isModifierRow = TranslateRow(ref row) == RowStyle.Modifier;
				IORMGenerator selectedGenerator = isModifierRow ? _modifiers[row].SelectedORMGenerator : _branches.Values[row].SelectedORMGenerator;
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
								displayData.StateImageIndex = (short)((isModifierRow || CanRemoveGenerator(row)) ? StandardCheckBoxImage.Checked : StandardCheckBoxImage.CheckedDisabled);
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
			/// <param name="generatorIndex">The row to test.</param>
			/// <returns>true if the generator can be removed without removing a format required by another generator</returns>
			private bool CanRemoveGenerator(int generatorIndex)
			{
				return CanRemoveGenerator(_branches.Values[generatorIndex]);
			}
			/// <summary>
			/// Test if a generator for an item can be removed
			/// </summary>
			/// <param name="branch">The branch to test.</param>
			/// <returns>true if the generator can be removed without removing a format required by another generator</returns>
			private bool CanRemoveGenerator(OutputFormatBranch branch)
			{
				return !branch.IsDependency;
			}
			public override int VisibleItemCount
			{
				get
				{
					List<OutputFormatBranch> modifiers = _modifiers;
					return _branches.Count + ((modifiers != null) ? modifiers.Count : 0);
				}
			}
			public override StateRefreshChanges ToggleState(int row, int column)
			{
				return ToggleOnRequiredBranches((TranslateRow(ref row) == RowStyle.Generator) ? _branches.Values[row] : _modifiers[row], 0);
			}
			private StateRefreshChanges ToggleOnRequiredBranches(OutputFormatBranch formatBranch, int branchGeneratorIndex)
			{
				return ToggleOnRequiredBranches(formatBranch, branchGeneratorIndex, true);
			}
			private StateRefreshChanges ToggleOnRequiredBranches(OutputFormatBranch formatBranch, int branchGeneratorIndex, bool testToggleOff)
			{
				StateRefreshChanges retVal = StateRefreshChanges.None;
				if (formatBranch.IsModifier)
				{
					if (formatBranch.SelectedORMGenerator == null)
					{
						// The build item is associated primarily with the primary generator,
						// not the modifier. We need to make sure that the primary generator
						// is turned on.
						IORMGenerator modifierGenerator = formatBranch.ORMGenerators[0];
						OutputFormatBranch primaryBranch = _branches[modifierGenerator.ProvidesOutputFormat];
						IORMGenerator primaryGenerator = primaryBranch.SelectedORMGenerator;
						if (primaryGenerator == null)
						{
							if (StateRefreshChanges.None != ToggleOnRequiredBranches(primaryBranch, 0, false))
							{
								retVal = StateRefreshChanges.Entire;
							}
							primaryGenerator = primaryBranch.SelectedORMGenerator;
							if (primaryGenerator == null)
							{
								return StateRefreshChanges.None;
							}
						}
						formatBranch.SelectedORMGenerator = modifierGenerator;
						SetItemMetaData(_parent.BuildItemsByGenerator[primaryGenerator.OfficialName], ITEMMETADATA_ORMGENERATOR, primaryBranch.SelectedGeneratorOfficialNames);
						retVal |= StateRefreshChanges.Children | StateRefreshChanges.Children;
					}
					else if (testToggleOff)
					{
						// Note that we can always remove a modifier, do not call CanRemoveGenerator
						RemoveGenerator(formatBranch);
						retVal |= StateRefreshChanges.Current | StateRefreshChanges.Children;
					}
				}
				else if (formatBranch.SelectedORMGenerator == null)
				{
#if VISUALSTUDIO_10_0
					string projectPath = Parent._project.FullPath;
#else
					string projectPath = Parent._project.FullFileName;
#endif
					EnvDTE.ProjectItem projectItem = Parent._projectItem;

					string sourceFileName = _parent._sourceFileName;
					string projectItemPath = (string)projectItem.Properties.Item("LocalPath").Value;
					string newItemDirectory = (new Uri(projectPath)).MakeRelativeUri(new Uri(projectItemPath)).ToString();
					newItemDirectory = Path.GetDirectoryName(newItemDirectory);

					retVal = StateRefreshChanges.Current | StateRefreshChanges.Children;
					IORMGenerator useGenerator = formatBranch.ORMGenerators[branchGeneratorIndex];
					string outputFileName = useGenerator.GetOutputFileDefaultName(sourceFileName);
					outputFileName = Path.Combine(newItemDirectory, outputFileName);
#if VISUALSTUDIO_10_0
					ProjectItemElement newBuildItem;
#else
					BuildItem newBuildItem;
#endif
					newBuildItem = useGenerator.AddGeneratedFileItem(_parent._itemGroup, sourceFileName, outputFileName); //string.Concat(newItemPath, Path.DirectorySeparatorChar, _parent._sourceFileName));
					_parent.BuildItemsByGenerator[useGenerator.OfficialName] = newBuildItem;
					_parent.RemoveRemovedItem(newBuildItem);
					formatBranch.SelectedORMGenerator = useGenerator;
					IList<string> requiredFormats = useGenerator.RequiresInputFormats;
					int requiredFormatCount = requiredFormats.Count;
					IList<string> companionFormats = useGenerator.RequiresCompanionFormats;
					int companionFormatCount = companionFormats.Count;
					int totalCount = requiredFormatCount + companionFormatCount;
					for (int i = 0; i < totalCount; ++i)
					{
						OutputFormatBranch requiredBranch;
						if (_branches.TryGetValue(i < requiredFormatCount ? requiredFormats[i] : companionFormats[i - requiredFormatCount], out requiredBranch))
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
			/// no checking here as to whether or not the generator is required
			/// in other places. Most uses will first call CanRemoveGenerator, but
			/// switching generators for a format will want to remove this blindly
			/// then toggle on the other format provider.
			/// </summary>
			/// <param name="formatBranch">The branch to remove from the generation process</param>
			private void RemoveGenerator(OutputFormatBranch formatBranch)
			{
				IORMGenerator removeGenerator = formatBranch.SelectedORMGenerator;
#if VISUALSTUDIO_10_0
				IDictionary<string, ProjectItemElement> buildItemsByGeneratorName
#else
				IDictionary<string, BuildItem> buildItemsByGeneratorName
#endif
				 = _parent.BuildItemsByGenerator;
				if (removeGenerator.IsFormatModifier)
				{
					OutputFormatBranch primaryBranch = _branches[removeGenerator.ProvidesOutputFormat];
					IORMGenerator primaryGenerator = primaryBranch.SelectedORMGenerator;
					if (primaryGenerator != null)
					{
#if VISUALSTUDIO_10_0
						ProjectItemElement updateBuildItem
#else
						BuildItem updateBuildItem
#endif
						 = buildItemsByGeneratorName[primaryGenerator.OfficialName];
						formatBranch.SelectedORMGenerator = null;
						SetItemMetaData(updateBuildItem, ITEMMETADATA_ORMGENERATOR, primaryBranch.SelectedGeneratorOfficialNames);
					}
				}
				else
				{
					string generatorKey = removeGenerator.OfficialName;
					formatBranch.SelectedORMGenerator = null;
#if VISUALSTUDIO_10_0
					ProjectItemElement removeBuildItem
#else
					BuildItem removeBuildItem
#endif
					 = buildItemsByGeneratorName[generatorKey];
					buildItemsByGeneratorName.Remove(generatorKey);
#if VISUALSTUDIO_10_0
					_parent._itemGroup.RemoveChild(removeBuildItem);
#else
					_parent._itemGroup.RemoveItem(removeBuildItem);
#endif
					_parent.AddRemovedItem(removeBuildItem);
				}
			}
#if VISUALSTUDIO_10_0
			private static void SetItemMetaData(ProjectItemElement buildItem, string metadataName, string metadataValue)
			{
				// ProjectItemElement.SetMetadata adds a new metadata element with the same name
				// as the previous one. There is no 'RemoveMetadata' that takes a string, so we go through
				// the entire metadata collection and clean it out.
				foreach (ProjectMetadataElement element in buildItem.Metadata)
				{
					if (element.Name == metadataName)
					{
						// The Metadata collection is a read-only snapshot, so deleting from it is safe
						// inside the iterator.
						buildItem.RemoveChild(element);
						// Do not break. This handles removing multiple metadata items with the
						// same name, which will clean up project files affected by this problem
						// in previous drops.
					}
				}
				buildItem.AddMetadata(metadataName, metadataValue);
			}
#else // VISUALSTUDIO_10_0
			private static void SetItemMetaData(BuildItem buildItem, string metadataName, string metadataValue)
			{
				buildItem.SetMetadata(metadataName, metadataValue);
			}
#endif // VISUALSTUDIO_10_0
			public override BranchFeatures Features
			{
				get
				{
					return BranchFeatures.Expansions | BranchFeatures.StateChanges;
				}
			}

			public override bool IsExpandable(int row, int column)
			{
				return TranslateRow(ref row) == RowStyle.Generator &&
					this._branches.Values[row].VisibleItemCount != 1;
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
