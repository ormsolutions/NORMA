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
using System.Drawing;
using System.Diagnostics;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch
		{
			public sealed class OutputFormatBranch : BranchBase
			{
				public OutputFormatBranch(MainBranch mainBranch)
				{
					this._mainBranch = mainBranch;
					// MainBranch populates this list for us...
					this._ormGenerators = new List<IORMGenerator>();
				}

				private IORMGenerator _selectedORMGenerator;
				private readonly List<IORMGenerator> _ormGenerators;
				private readonly MainBranch _mainBranch;
				private int _selectedUseCount;
				private OutputFormatBranch _nextModifier;
				
				public IORMGenerator SelectedORMGenerator
				{
					get
					{
						return this._selectedORMGenerator;
					}
					set
					{
						IORMGenerator oldGenerator = _selectedORMGenerator;
						if (oldGenerator == value)
						{
							return;
						}
						if (value != null)
						{
							_selectedUseCount = -1;
							// Note that we add new dependencies first so we don't toggle
							// a support file off, then back on.
							UpdateDependencyUseCounts(value, true);
						}
						else
						{
							_selectedUseCount = 0;
						}
						this._selectedORMGenerator = value;
						if (oldGenerator != null)
						{
							UpdateDependencyUseCounts(oldGenerator, false);
						}
					}
				}
				/// <summary>
				/// Manage a singly linked list of format modifiers.
				/// The modifier should always be added to the head node
				/// to maintain an order modifier priority.
				/// </summary>
				public OutputFormatBranch NextModifier
				{
					get
					{
						return _nextModifier;
					}
					set
					{
						Debug.Assert(value != null && value.IsModifier);
						OutputFormatBranch oldModifier = _nextModifier;
						if (oldModifier != null)
						{
							if (oldModifier._ormGenerators[0].FormatModifierPriority <= value._ormGenerators[0].FormatModifierPriority)
							{
								oldModifier.NextModifier = value;
							}
							else
							{
								_nextModifier = value;
								value.NextModifier = oldModifier;
							}
						}
						else
						{
							_nextModifier = value;
						}
					}
				}
				/// <summary>
				/// Get a space-separated list of the selected generator
				/// followed by the official names of all selected modifiers.
				/// </summary>
				public string SelectedGeneratorOfficialNames
				{
					get
					{
						string retVal = "";
						IORMGenerator selectedGenerator = _selectedORMGenerator;
						if (selectedGenerator != null)
						{
							retVal = selectedGenerator.OfficialName;
							OutputFormatBranch modifierBranch = _nextModifier;
							while (modifierBranch != null)
							{
								selectedGenerator = modifierBranch.SelectedORMGenerator;
								if (selectedGenerator != null)
								{
									retVal += " " + selectedGenerator.OfficialName;
								}
								modifierBranch = modifierBranch.NextModifier;
							}
						}
						return retVal;
					}
				}
				/// <summary>
				/// Does this branch represent a format modifier.
				/// A format modifier has exactly one generator.
				/// </summary>
				public bool IsModifier
				{
					get
					{
						List<IORMGenerator> generators = _ormGenerators;
						return generators.Count == 1 && generators[0].IsFormatModifier;
					}
				}
				/// <summary>
				/// Update dependency counts for branches of required formats
				/// </summary>
				/// <param name="generator">The generator to add or remove dependencies for</param>
				/// <param name="addOrRemove">true to add a dependency, false to remove</param>
				private void UpdateDependencyUseCounts(IORMGenerator generator, bool addOrRemove)
				{
					IList<string> requiredFormats = generator.RequiresInputFormats;
					int requiredFormatCount = requiredFormats.Count;
					IList<string> companionFormats = generator.RequiresCompanionFormats;
					int requiredCompanionCount = companionFormats.Count;
					int dependencyCount = requiredFormatCount + requiredCompanionCount;
					if (dependencyCount != 0)
					{
						string outputFormat = generator.ProvidesOutputFormat;
						SortedList<string, OutputFormatBranch> branchDictionary = _mainBranch._branches;
						for (int i = 0; i < dependencyCount; ++i)
						{
							OutputFormatBranch dependentUponBranch;
							bool isRequired = i < requiredFormatCount;
							if (branchDictionary.TryGetValue(isRequired ? requiredFormats[i] : companionFormats[i - requiredFormatCount], out dependentUponBranch))
							{
								int currentUseCount = dependentUponBranch._selectedUseCount;
								IORMGenerator selectedGenerator; 
								if (addOrRemove)
								{
									if (currentUseCount != -1 &&
										null != (selectedGenerator = dependentUponBranch._selectedORMGenerator) &&
										(isRequired || !selectedGenerator.RequiresCompanionFormats.Contains(outputFormat)))
									{
										dependentUponBranch._selectedUseCount = currentUseCount + 1;
									}
								}
								else
								{
									selectedGenerator = dependentUponBranch._selectedORMGenerator;
									bool autoRemove = (selectedGenerator != null) ? selectedGenerator.GeneratesSupportFile : false;
									if (currentUseCount > 0)
									{
										bool companionCycle;
										if (isRequired || !(companionCycle = selectedGenerator.RequiresCompanionFormats.Contains(outputFormat)))
										{
											dependentUponBranch._selectedUseCount = currentUseCount - 1;
											autoRemove = autoRemove && currentUseCount == 1;
										}
										else if (companionCycle)
										{
											autoRemove = autoRemove && currentUseCount == 1;
										}
									}
									else if (autoRemove)
									{
										if (currentUseCount == -1)
										{
											autoRemove = !dependentUponBranch.IsDependency;
										}
										else
										{
											autoRemove = false;
										}
									}
									else if (!isRequired &&
										selectedGenerator != null &&
										selectedGenerator.RequiresCompanionFormats.Contains(outputFormat))
									{
										autoRemove = (currentUseCount == -1) ? !dependentUponBranch.IsDependency : (currentUseCount == 0);
									}
									if (autoRemove)
									{
										_mainBranch.RemoveGenerator(dependentUponBranch);
									}
								}
							}
						}
					}
				}

				public IList<IORMGenerator> ORMGenerators
				{
					get
					{
						return this._ormGenerators;
					}
				}

				private MainBranch MainBranch
				{
					get
					{
						return this._mainBranch;
					}
				}
				/// <summary>
				/// Determine if the current branch is required by other branches
				/// </summary>
				public bool IsDependency
				{
					get
					{
						int useCount = _selectedUseCount;
						bool retVal = false;
						switch (useCount)
						{
							case 0:
								// We've already determined that this is not a dependency, or there is no generator
								break;
							case -1:
								// We don't know if this is a dependency or not
								{
									IORMGenerator selectedGenerator = _selectedORMGenerator;
									Debug.Assert(selectedGenerator != null, "_selectedUseCount should be zero if there is no selected generator");
									useCount = 0;
									if (selectedGenerator != null)
									{
										// Don't allow this to uncheck if another tool is using it
										IList<OutputFormatBranch> allBranches = _mainBranch._branches.Values;
										IList<OutputFormatBranch> modifierBranches = _mainBranch._modifiers;
										int branchCount = allBranches.Count;
										int allCount = branchCount + (modifierBranches != null ? modifierBranches.Count : 0);
										string outputFormat = selectedGenerator.ProvidesOutputFormat;
										int i = 0;
										for (; i < allCount; ++i)
										{
											bool isPrimaryBranch = i < branchCount;
											OutputFormatBranch currentBranch = isPrimaryBranch ? allBranches[i] : modifierBranches[i - branchCount];
											if (currentBranch != this)
											{
												IORMGenerator testGenerator = currentBranch.SelectedORMGenerator;
												if (testGenerator != null &&
													(testGenerator.RequiresInputFormats.Contains(outputFormat) ||
													(testGenerator.RequiresCompanionFormats.Contains(outputFormat) && !selectedGenerator.RequiresCompanionFormats.Contains(testGenerator.ProvidesOutputFormat))))
												{
													++useCount;
												}
											}
										}
									}
									_selectedUseCount = useCount;
									retVal = useCount > 0;
									break;
								}
							default:
								// We've already calculated that this is a dependency
								retVal = true;
								break;
						}
						return retVal;
					}
				}

				public override string GetText(int row, int column)
				{
					return this.ORMGenerators[row].DisplayName;
				}

				public override string GetTipText(int row, int column, ToolTipType tipType)
				{
					if (tipType == ToolTipType.StateIcon)
					{
						return _ormGenerators[row].DisplayDescription;
					}
					return base.GetTipText(row, column, tipType);
				}
				public override VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
					displayData.BackColor = SystemColors.ControlLight;
					bool currentGeneratorSelected = object.ReferenceEquals(_selectedORMGenerator, _ormGenerators[row]);
					if (currentGeneratorSelected)
					{
						displayData.Bold = true;
					}
					if (0 != (requiredData.Mask & VirtualTreeDisplayMasks.StateImage))
					{
						if (currentGeneratorSelected)
						{
							displayData.StateImageIndex = _mainBranch.CanRemoveGenerator(this) ? (short)StandardCheckBoxImage.Checked : (short)StandardCheckBoxImage.CheckedDisabled;
						}
						else
						{
							displayData.StateImageIndex = (short)StandardCheckBoxImage.Unchecked;
						}
					}
					return displayData;
				}
				public override StateRefreshChanges ToggleState(int row, int column)
				{
					StateRefreshChanges retVal = StateRefreshChanges.None;
					IORMGenerator selectedGenerator = _selectedORMGenerator;
					if (selectedGenerator == null)
					{
						retVal = _mainBranch.ToggleOnRequiredBranches(this, row);
					}
					else
					{
						IORMGenerator newGenerator = _ormGenerators[row];
						if (object.ReferenceEquals(newGenerator, selectedGenerator))
						{
							retVal = _mainBranch.ToggleOnRequiredBranches(this, row, true);
						}
						else
						{
							_mainBranch.RemoveGenerator(this);
							retVal = _mainBranch.ToggleOnRequiredBranches(this, row, false);
						}
					}
					if (retVal != StateRefreshChanges.None && retVal != StateRefreshChanges.Entire)
					{
						retVal = StateRefreshChanges.ParentsChildren;
					}
					return retVal;
				}
				public override int VisibleItemCount
				{
					get
					{
						return this.ORMGenerators.Count;
					}
				}
				public override BranchFeatures Features
				{
					get
					{
						return BranchFeatures.StateChanges;
					}
				}
			}
		}
	}
}
