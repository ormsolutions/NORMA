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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.Shell;
using Microsoft.VisualStudio.Modeling;
using EnvDTE;
#if VISUALSTUDIO_10_0
using Microsoft.Build.Construction;
using Microsoft.Win32;
#else
using Microsoft.Build.BuildEngine;
#endif

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	internal sealed partial class ORMGeneratorSelectionControl : Form
	{
		// TODO: Remove these once this is nested within ORMCustomTool.
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
		private const string ITEMMETADATA_GENERATOR = "Generator";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
		private const string ITEMMETADATA_ORMGENERATORTARGET_PREFIX = "ORMGeneratorTarget_"; // Suffix is index then _ then the target type, value is the name (not the id)

		/// <summary>
		/// A component class for <see cref="PseudoBuildItem"/> to represent a
		/// single build item. A PseudoBuildItem is a placeholder for a single
		/// output format, which can be comprised of multiple instances dependending
		/// on how the generator targets resolve.
		/// </summary>
		private class PseudoBuildInstance
		{
			public readonly string OriginalGeneratorNames;
			public readonly GeneratorTarget[] OriginalGeneratorTargets;

			/// <summary>
			/// Flag during final processing if the instance is removed.
			/// </summary>
			/// <remarks>clearing PseudoBuiltItem.CurrentGeneratorNames indicates
			/// that all instances for the build item are to be removed, so this is
			/// used to manage individual instances.</remarks>
			public bool IsRemoved;

			/// <summary>
			/// Create an instance corresponding to a single generated
			/// output to associated with a <see cref="PseudoBuildItem"/>
			/// </summary>
			/// <param name="originalGeneratorNames"></param>
			/// <param name="originalGeneratorTargets"></param>
			public PseudoBuildInstance(string originalGeneratorNames, GeneratorTarget[] originalGeneratorTargets)
			{
				OriginalGeneratorNames = originalGeneratorNames;
				OriginalGeneratorTargets = originalGeneratorTargets;
				IsRemoved = false;
			}
		}

		/// <summary>
		/// Create a structure to represent a virtual build item. The dialog
		/// is keyed off of generator types, which originally mapped to a single
		/// build item in the current group in this system. However, generator
		/// targets now allow multiple build items per group. We don't want to
		/// radically complicate the dialog by adding additional dimensions showing
		/// the different generator targets, so we pretend that all build items
		/// with the same generator are the same item, then expand this back out
		/// to real build items on commit.
		/// </summary>
		/// <remarks>We may split this up in the future with different generators
		/// and different format modifiers applying to different branches, but it is
		/// not worth the complication at this point.</remarks>
		private class PseudoBuildItem
		{
			public string DefaultGeneratedFileName;
			public string CurrentGeneratorNames;
			public List<PseudoBuildInstance> OriginalInstances;

			/// <summary>
			/// Create an initial instance of a pseudo build item for an existing item.
			/// </summary>
			public PseudoBuildItem(string currentGeneratorNames, string defaultGeneratedFileName)
			{
				CurrentGeneratorNames = currentGeneratorNames;
				DefaultGeneratedFileName = defaultGeneratedFileName;
			}

			/// <summary>
			/// Add a tracked target set found in the original items.
			/// </summary>
			public void AddOriginalInstance(string generatorNames, GeneratorTarget[] generatorTargets)
			{
				(OriginalInstances ?? (OriginalInstances = new List<PseudoBuildInstance>())).Add(new PseudoBuildInstance(generatorNames, generatorTargets));
			}
		}

		private readonly MainBranch _mainBranch;
		private readonly EnvDTE.ProjectItem _projectItem;
		private bool _savedChanges;
		private readonly string _sourceFileName;
		private readonly string _projectItemRelativePath;
		private readonly IServiceProvider _serviceProvider;
#if VISUALSTUDIO_10_0
		private readonly ProjectRootElement _project;
		private readonly ProjectItemGroupElement _originalItemGroup;
#else // VISUALSTUDIO_10_0
		private readonly Microsoft.Build.BuildEngine.Project _project;
		private readonly BuildItemGroup _originalItemGroup;
#endif // VISUALSTUDIO_10_0
		private readonly Dictionary<string, PseudoBuildItem> _pseudoItemsByOutputFormat;
		private ORMGeneratorSelectionControl()
		{
			this.InitializeComponent();
			textBox_ORMFileName.Left = label_GeneratedFilesFor.Right + textBox_ORMFileName.Margin.Left;
		}

		public ORMGeneratorSelectionControl(EnvDTE.ProjectItem projectItem, IServiceProvider serviceProvider)
			: this()
		{
			_projectItem = projectItem;
			_serviceProvider = serviceProvider;
#if VISUALSTUDIO_10_0
			ProjectRootElement project = ProjectRootElement.TryOpen(projectItem.ContainingProject.FullName);
			string projectFullPath = project.FullPath;
#else // VISUALSTUDIO_10_0
			Microsoft.Build.BuildEngine.Project project = Engine.GlobalEngine.GetLoadedProject(projectItem.ContainingProject.FullName);
			string projectFullPath = project.FullFileName;
#endif // VISUALSTUDIO_10_0
			_project = project;

			string projectItemRelativePath = (string)projectItem.Properties.Item("LocalPath").Value;
			projectItemRelativePath = (new Uri(projectFullPath)).MakeRelativeUri(new Uri(projectItemRelativePath)).ToString();
			_projectItemRelativePath = projectItemRelativePath;

#if VISUALSTUDIO_10_0
			ProjectItemGroupElement originalItemGroup = ORMCustomTool.GetItemGroup(project, projectItemRelativePath);
#else // VISUALSTUDIO_10_0
			BuildItemGroup originalItemGroup = ORMCustomTool.GetItemGroup(project, projectItemRelativePath);
#endif // VISUALSTUDIO_10_0
			_originalItemGroup = originalItemGroup;

			string sourceFileName = projectItem.Name;
			_sourceFileName = sourceFileName;
			this.textBox_ORMFileName.Text = sourceFileName;

			this.button_SaveChanges.Click += new EventHandler(this.SaveChanges);
			this.button_Cancel.Click += new EventHandler(this.Cancel);

			ITree tree = (ITree)(this.virtualTreeControl.MultiColumnTree = new MultiColumnTree(2));
			this.virtualTreeControl.SetColumnHeaders(new VirtualTreeColumnHeader[]
				{
					// TODO: Localize these.
					new VirtualTreeColumnHeader("Generated File Format", 0.30f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled),
					new VirtualTreeColumnHeader("Generated File Name", 1f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled)
				}, true);
			MainBranch mainBranch = this._mainBranch = new MainBranch(this
#if VISUALSTUDIO_15_0
				, serviceProvider
#endif
				);
			int totalCount = mainBranch.VisibleItemCount;
			int[] primaryIndices = new int[totalCount];
			for (int i = 0; i < totalCount; ++i)
			{
				if (mainBranch.IsPrimaryDisplayItem(i))
				{
					primaryIndices[i] = i - totalCount;
				}
				else
				{
					primaryIndices[i] = i + 1;
				}
			}
			Array.Sort<int>(primaryIndices);
			int lastPrimary = -1;
			for (int i = 0; i < totalCount; ++i)
			{
				int modifiedIndex = primaryIndices[i];
				if (modifiedIndex < 0)
				{
					primaryIndices[i] = modifiedIndex + totalCount;
				}
				else
				{
					if (lastPrimary == -1)
					{
						lastPrimary = i - 1;
					}
					primaryIndices[i] = modifiedIndex - 1;
				}
			}
			int modifierCount = totalCount - mainBranch.Branches.Count;
			tree.Root = (lastPrimary == -1) ? (IBranch)mainBranch : new BranchPartition(
				mainBranch,
				primaryIndices,
				new BranchPartitionSection(0, lastPrimary + 1),
				new BranchPartitionSection(totalCount - modifierCount, modifierCount, "Generated File Modifiers", false),
				new BranchPartitionSection(lastPrimary + 1, totalCount - lastPrimary - modifierCount - 1, "Intermediate and Secondary Files", true)); // UNDONE: Localize Header
			this.virtualTreeControl.ShowToolTips = true;
			this.virtualTreeControl.FullCellSelect = true;

			Dictionary<string, PseudoBuildItem> pseudoItemsByOutputFormat = new Dictionary<string, PseudoBuildItem>(StringComparer.OrdinalIgnoreCase);
			_pseudoItemsByOutputFormat = pseudoItemsByOutputFormat;
			IDictionary<string, IORMGenerator> generators = 
#if VISUALSTUDIO_15_0
				ORMCustomTool.GetORMGenerators(serviceProvider);
#else
				ORMCustomTool.ORMGenerators;
#endif
			if (originalItemGroup != null)
			{
#if VISUALSTUDIO_10_0
				foreach (ProjectItemElement buildItem in originalItemGroup.Items)
#else // VISUALSTUDIO_10_0
				foreach (BuildItem buildItem in originalItemGroup)
#endif // VISUALSTUDIO_10_0
				{
					// Do this very defensively so that the dialog can still be opened if a project is out
					// of step with the generators registered on a specific machine.
					string generatorNameData;
					string[] generatorNames; // The first string is the primary generator, others are the format modifiers
					int generatorNameCount;
					IORMGenerator primaryGenerator;
					MainBranch.OutputFormatBranch primaryFormatBranch;
					if (!string.IsNullOrEmpty(generatorNameData = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR)) &&
						!string.IsNullOrEmpty(generatorNameData = generatorNameData.Trim()) &&
						string.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), sourceFileName, StringComparison.OrdinalIgnoreCase) &&
						null != (generatorNames = generatorNameData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)) &&
						0 != (generatorNameCount = generatorNames.Length) &&
						// This assumes that each generator target of the same type has all of the same options.
						// This is currently the result of this dialog, and we're not considering hand edits
						// to the project file at this point.
						generators.TryGetValue(generatorNames[0], out primaryGenerator) &&
						mainBranch.Branches.TryGetValue(primaryGenerator.ProvidesOutputFormat, out primaryFormatBranch))
					{
						PseudoBuildItem pseudoItem;
						string outputFormat = primaryGenerator.ProvidesOutputFormat;

						if (!pseudoItemsByOutputFormat.TryGetValue(outputFormat, out pseudoItem))
						{
							// Note that we can't use the build item file name here as it might be decorated with
							// target names. Go back to the generator to get an undecorated default name.
							pseudoItem = new PseudoBuildItem(generatorNameData, primaryGenerator.GetOutputFileDefaultName(sourceFileName));
							pseudoItemsByOutputFormat.Add(outputFormat, pseudoItem);

							if (primaryFormatBranch.SelectedORMGenerator == null)
							{
								primaryFormatBranch.SelectedORMGenerator = primaryGenerator;
							}

							// Format modifiers are attached to the end of the list
							for (int i = 1; i < generatorNameCount; ++i)
							{
								MainBranch.OutputFormatBranch modifierBranch = primaryFormatBranch.NextModifier;
								string findName = generatorNames[i];
								while (modifierBranch != null)
								{
									IORMGenerator testGenerator = modifierBranch.ORMGenerators[0];
									if (testGenerator.OfficialName == findName)
									{
										modifierBranch.SelectedORMGenerator = testGenerator;
										break;
									}
									modifierBranch = modifierBranch.NextModifier;
								}
							}
						}

						pseudoItem.AddOriginalInstance(generatorNameData, ORMCustomToolUtility.GeneratorTargetsFromBuildItem(buildItem));
					}
				}
			}
		}

		private string SourceFileName
		{
			get
			{
				return this._sourceFileName;
			}
		}

		private IDictionary<string, PseudoBuildItem> PseudoItemsByOutputFormat
		{
			get
			{
				return this._pseudoItemsByOutputFormat;
			}
		}

		private void RemovePseudoItem(string outputFormat)
		{
			Dictionary<string, PseudoBuildItem> items = _pseudoItemsByOutputFormat;
			PseudoBuildItem pseudoItem;
			if (items.TryGetValue(outputFormat, out pseudoItem))
			{
				if (pseudoItem.OriginalInstances != null)
				{
					// Indicate an original item is deleted by clearing the generator names.
					pseudoItem.CurrentGeneratorNames = null;
				}
				else
				{
					// Added and removed in the same session, no reason to track.
					items.Remove(outputFormat);
				}
			}
		}

		/// <summary>
		/// Add a new item, or resurrect a deleted one.
		/// </summary>
		/// <param name="generator">The generator to add.</param>
		/// <param name="currentGeneratorNames">The current generator names. Note that if this is a
		/// toggle that adds a new generator for the same format, then the current names may
		/// include format modifiers that are not available from the generator itself.</param>
		private void AddPseudoItem(IORMGenerator generator, string currentGeneratorNames)
		{
			Dictionary<string, PseudoBuildItem> items = _pseudoItemsByOutputFormat;
			string outputFormat = generator.ProvidesOutputFormat;
			string generatedFileName = generator.GetOutputFileDefaultName(_sourceFileName);
			PseudoBuildItem pseudoItem;
			if (items.TryGetValue(outputFormat, out pseudoItem))
			{
				pseudoItem.CurrentGeneratorNames = currentGeneratorNames;
				pseudoItem.DefaultGeneratedFileName = generatedFileName;
			}
			else
			{
				pseudoItem = new PseudoBuildItem(null, generatedFileName);
				pseudoItem.CurrentGeneratorNames = currentGeneratorNames;
				items[outputFormat] = pseudoItem;
			}
		}

#if !VISUALSTUDIO_10_0
		private delegate void Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
#endif
		protected override void OnClosed(EventArgs e)
		{
			if (_savedChanges)
			{
				// Make sure the current document has the necessary
				// extensions loaded.
				// UNDONE: We should be able to do this with the document
				// closed or open as text as well via a registered service
				// on the ORMDesignerPackage, but this is sufficient for now.
				Dictionary<string, string> requiredExtensions = null;
				string[] loadedExtensions = null;
				foreach (IORMGenerator selectedGenerator in _mainBranch.SelectedGenerators)
				{
					foreach (string requiredExtension in selectedGenerator.GetRequiredExtensionsForInputFormat("ORM"))
					{
						if (loadedExtensions == null)
						{
							loadedExtensions = (new ORMExtensionManager(_projectItem)).GetLoadedExtensions(_serviceProvider);
						}
						if (Array.BinarySearch<string>(loadedExtensions, requiredExtension) < 0)
						{
							if (requiredExtensions == null)
							{
								requiredExtensions = new Dictionary<string, string>();
							}
							else if (requiredExtensions.ContainsKey(requiredExtension))
							{
								continue;
							}
							requiredExtensions.Add(requiredExtension, requiredExtension);
						}
					}
				}
				if (requiredExtensions != null)
				{
					_savedChanges = ORMExtensionManager.EnsureExtensions(_projectItem, _serviceProvider, requiredExtensions.Values);
				}
			}
			if (_savedChanges)
			{
#if VISUALSTUDIO_10_0
				ProjectItemGroupElement itemGroup = _originalItemGroup;
				ProjectRootElement project = _project;
#else // VISUALSTUDIO_10_0
				BuildItemGroup itemGroup = _originalItemGroup;
				Microsoft.Build.BuildEngine.Project project = _project;
#endif // VISUALSTUDIO_10_0
				EnvDTE.ProjectItem projectItem = _projectItem;
				string sourceFileName = _sourceFileName;
				Dictionary<string, PseudoBuildItem>pseudoItems = _pseudoItemsByOutputFormat;
				IDictionary<string, IORMGenerator> generators =
#if VISUALSTUDIO_15_0
					ORMCustomTool.GetORMGenerators(_serviceProvider);
#else
					ORMCustomTool.ORMGenerators;
#endif
				PseudoBuildItem pseudoItem;
				string generatorNameData; // The first string is the primary generator, others are the format modifiers, space delimited
				IVsShell shell;

				Dictionary<string, IORMGenerator> generatorsWithTargetsByOutputFormat = null;
				IDictionary<string, ORMCustomToolUtility.GeneratorTargetSet> targetSetsByFormatName = null;
				foreach (PseudoBuildItem testPseudoItem in pseudoItems.Values)
				{
					string primaryGeneratorName;
					IList<string> generatorTargets;
					IORMGenerator generator;
					if (!string.IsNullOrEmpty(generatorNameData = testPseudoItem.CurrentGeneratorNames) &&
						null != (primaryGeneratorName = ORMCustomToolUtility.GetPrimaryGeneratorName(generatorNameData)) &&
						generators.TryGetValue(primaryGeneratorName, out generator) &&
						null != (generatorTargets = generator.GeneratorTargetTypes) &&
						0 != generatorTargets.Count)
					{
						(generatorsWithTargetsByOutputFormat ?? (generatorsWithTargetsByOutputFormat = new Dictionary<string, IORMGenerator>(StringComparer.OrdinalIgnoreCase)))[generator.ProvidesOutputFormat] = generator;
					}
				}
				if (generatorsWithTargetsByOutputFormat != null)
				{
					IDictionary<string, GeneratorTarget[]> docTargets = null;
					EnvDTE.Document projectItemDocument = projectItem.Document;
					string itemPath;
					if (projectItemDocument != null)
					{
						using (Stream targetsStream = ORMCustomToolUtility.GetDocumentExtension<Stream>(projectItemDocument, "ORMGeneratorTargets", itemPath = projectItem.get_FileNames(0), _serviceProvider))
						{
							if (targetsStream != null)
							{
								targetsStream.Seek(0, SeekOrigin.Begin);
								docTargets = new BinaryFormatter().Deserialize(targetsStream) as IDictionary<string, GeneratorTarget[]>;
							}
						}
					}
					else if (null != (shell = _serviceProvider.GetService(typeof(SVsShell)) as IVsShell))
					{
						Guid pkgId = typeof(ORMDesignerPackage).GUID;
						IVsPackage package;
						if (0 != shell.IsPackageLoaded(ref pkgId, out package) || package == null)
						{
							shell.LoadPackage(ref pkgId, out package);
						}

						// Temporarily load the document so that the generator targets can be resolved.
						using (Store store = new ModelLoader(ORMDesignerPackage.ExtensionLoader, true).Load(projectItem.get_FileNames(0)))
						{
							docTargets = GeneratorTarget.ConsolidateGeneratorTargets(store as IFrameworkServices);
						}
					}

					// We have generators that care about targets, which means that ExpandGeneratorTargets will
					// product placeholder targets for these generators even if docTargets is currently null.
					// This allows the dialog to turn on a generator before the data (or even extension) to feed
					// it is available in the model and provides a smooth transition in and out of this placeholder
					// state. It is up to the individual generators to proceed without explicit target data or
					// to produce a message for the user with instructions on how to add the data to the model.
					Dictionary<string, string> generatorNamesByOutputFormat = new Dictionary<string, string>();
					foreach (KeyValuePair<string, PseudoBuildItem> pair in pseudoItems)
					{
						generatorNameData = pair.Value.CurrentGeneratorNames;
						if (!string.IsNullOrEmpty(generatorNameData))
						{
							generatorNamesByOutputFormat[pair.Key] = ORMCustomToolUtility.GetPrimaryGeneratorName(generatorNameData);
						}
					}
					targetSetsByFormatName = ORMCustomToolUtility.ExpandGeneratorTargets(generatorNamesByOutputFormat, docTargets
#if VISUALSTUDIO_15_0
						, _serviceProvider
#endif // VISUALSTUDIO_15_0
						);
				}

				Dictionary<string, BitTracker> processedGeneratorTargets = null;
				if (targetSetsByFormatName != null)
				{
					processedGeneratorTargets = new Dictionary<string, BitTracker>();
					foreach (KeyValuePair<string, ORMCustomToolUtility.GeneratorTargetSet> kvp in targetSetsByFormatName)
					{
						processedGeneratorTargets[kvp.Key] = new BitTracker(kvp.Value.Instances.Length);
					}
				}

				if (null != itemGroup)
				{
#if VISUALSTUDIO_10_0
					Dictionary<string, ProjectItemElement> removedItems = null;
					foreach (ProjectItemElement item in itemGroup.Items)
#else // VISUALSTUDIO_10_0
					Dictionary<string, BuildItem> removedItems = null;
					foreach (BuildItem item in itemGroup)
#endif // VISUALSTUDIO_10_0
					{
						string primaryGeneratorName;
						string outputFormat;
						IORMGenerator generator;
						if (null != (primaryGeneratorName = ORMCustomToolUtility.GetPrimaryGeneratorName(item.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR))) &&
							string.Equals(item.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), sourceFileName, StringComparison.OrdinalIgnoreCase) &&
							generators.TryGetValue(primaryGeneratorName, out generator) &&
							pseudoItems.TryGetValue(outputFormat = generator.ProvidesOutputFormat, out pseudoItem))
						{
							generatorNameData = pseudoItem.CurrentGeneratorNames;
							ORMCustomToolUtility.GeneratorTargetSet targetSet = null;
							BitTracker processedForFormat = default(BitTracker);
							if (targetSetsByFormatName != null)
							{
								if (targetSetsByFormatName.TryGetValue(outputFormat, out targetSet))
								{
									processedForFormat = processedGeneratorTargets[outputFormat];
								}
							}

							List<PseudoBuildInstance> originalInstances;
							bool removeInstance = false;
							if (string.IsNullOrEmpty(generatorNameData))
							{
								// The item is deleted, mark for removal
								removeInstance = true;
							}
							else if (null != (originalInstances = pseudoItem.OriginalInstances))
							{
								for (int i = 0, count = originalInstances.Count; i < count && !removeInstance; ++i)
								{
									PseudoBuildInstance instance = originalInstances[i];
									if (instance.IsRemoved)
									{
										continue;
									}

									GeneratorTarget[] targets = instance.OriginalGeneratorTargets;
									if (targetSet != null)
									{
										if (targets == null)
										{
											// Remove, if a target set is available then it must be used
											removeInstance = true;
										}
										else
										{
											int instanceIndex = targetSet.IndexOfInstance(targets, delegate (int ignoreInstance) { return processedForFormat[ignoreInstance]; });
											if (instanceIndex == -1)
											{
												removeInstance = true;
											}
											else if (!processedForFormat[instanceIndex])
											{
												if (instance.OriginalGeneratorNames != generatorNameData)
												{
													// This is a preexisting item, update its meta information
													ORMCustomToolUtility.SetItemMetaData(item, ITEMMETADATA_ORMGENERATOR, generatorNameData);
												}
												processedForFormat[instanceIndex] = true;
												processedGeneratorTargets[outputFormat] = processedForFormat;
												break;
											}
										}
									}
									else if (targets != null)
									{
										// Remove, formatter changed to one that does not use a generator target
										removeInstance = true;
									}
									else if (instance.OriginalGeneratorNames != generatorNameData)
									{
										// This is a preexisting item, update its meta information
										ORMCustomToolUtility.SetItemMetaData(item, ITEMMETADATA_ORMGENERATOR, generatorNameData);
									}

									if (removeInstance)
									{
										instance.IsRemoved = true;
									}
								}
							}

							if (removeInstance)
							{
								if (removedItems == null)
								{
#if VISUALSTUDIO_10_0
									removedItems = new Dictionary<string, ProjectItemElement>();
#else // VISUALSTUDIO_10_0
									removedItems = new Dictionary<string, BuildItem>();
#endif // VISUALSTUDIO_10_0
								}
								removedItems[ORMCustomToolUtility.GetItemInclude(item)] = item;
							}
						}
					}
					if (removedItems != null)
					{
						EnvDTE.ProjectItems subItems = projectItem.ProjectItems;
#if VISUALSTUDIO_10_0
						foreach (KeyValuePair<string, ProjectItemElement> removePair in removedItems)
						{
							ProjectItemElement removeItem = removePair.Value;
							ProjectElementContainer removeFrom;
							if (null != (removeFrom = removeItem.Parent))
							{
								removeFrom.RemoveChild(removeItem);
							}
#else // VISUALSTUDIO_10_0
						foreach (KeyValuePair<string, BuildItem> removePair in removedItems)
						{
							project.RemoveItem(removePair.Value);
#endif // VISUALSTUDIO_10_0
							try
							{
								EnvDTE.ProjectItem subItem = subItems.Item(removePair.Key);
								if (subItem != null)
								{
									subItem.Delete();
								}
							}
							catch (ArgumentException)
							{
								// Swallow
							}
						}
					}

#if !VISUALSTUDIO_10_0
					// Empty item groups remove themselves from the project, we'll need
					// to recreate below if the group is empty after the remove phase.
					if (itemGroup.Count == 0)
					{
						itemGroup = null;
					}
#endif
				}

				// Removes and changes are complete, proceed with adds for any new items
				string newItemDirectory = null;
				string projectPath = null;
				EnvDTE.ProjectItems projectItems = null;
				string tmpFile = null;
				// Adding a file to our special item group adds it to the build system. However,
				// it does not add it to the parallel project system, which is what displays in
				// the solution explorer. Therefore, we also explicitly add the item to the
				// project system as well. Unfortunately, this extra add automatically creates
				// a redundant item (usually in a new item group) for our adding item. Track anything
				// we add through the project system so that we can remove these redundant items from the
				// build system when we're done.
				Dictionary<string, string> sideEffectItemNames = null;
				try
				{
					Action<IORMGenerator, string, ORMCustomToolUtility.GeneratorTargetSet, GeneratorTarget[]> addProjectItem = delegate (IORMGenerator generator, string allGenerators, ORMCustomToolUtility.GeneratorTargetSet targetSet, GeneratorTarget[] targetInstance)
					{
						if (itemGroup == null)
						{
#if VISUALSTUDIO_10_0
							itemGroup = project.AddItemGroup();
#else
							itemGroup = project.AddNewItemGroup();
#endif
							itemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, _projectItemRelativePath, ITEMGROUP_CONDITIONEND);
						}
						if (newItemDirectory == null)
						{
							// Initialize general information
#if VISUALSTUDIO_10_0
							projectPath = project.FullPath;
#else
							projectPath = project.FullFileName;
#endif
							newItemDirectory = Path.GetDirectoryName(new Uri(projectPath).MakeRelativeUri(new Uri((string)projectItem.Properties.Item("LocalPath").Value)).ToString());
							projectItems = projectItem.ProjectItems;
						}

						string defaultFileName = generator.GetOutputFileDefaultName(sourceFileName);
						string fileName = targetInstance == null ? defaultFileName : ORMCustomToolUtility.GeneratorTargetSet.DecorateFileName(defaultFileName, targetInstance);
						string fileRelativePath = Path.Combine(newItemDirectory, fileName);
						string fileAbsolutePath = string.Concat(new FileInfo(projectPath).DirectoryName, Path.DirectorySeparatorChar, fileRelativePath);
#if VISUALSTUDIO_10_0
						ProjectItemElement newBuildItem;
#else
						BuildItem newBuildItem;
#endif
						newBuildItem = generator.AddGeneratedFileItem(itemGroup, sourceFileName, fileRelativePath);

						if (allGenerators != null)
						{
							ORMCustomToolUtility.SetItemMetaData(newBuildItem, ITEMMETADATA_ORMGENERATOR, allGenerators);
						}

						if (targetInstance != null)
						{
							ORMCustomToolUtility.SetGeneratorTargetMetadata(newBuildItem, targetInstance);
						}

						(sideEffectItemNames ?? (sideEffectItemNames = new Dictionary<string, string>()))[fileRelativePath] = null;
						if (File.Exists(fileAbsolutePath))
						{
							try
							{
								projectItems.AddFromFile(fileAbsolutePath);
							}
							catch (ArgumentException)
							{
								// Swallow
							}
						}
						else
						{
							if (tmpFile == null)
							{
								tmpFile = Path.GetTempFileName();
							}
							EnvDTE.ProjectItem newProjectItem = projectItems.AddFromTemplate(tmpFile, fileName);
							string customTool;
							if (!string.IsNullOrEmpty(customTool = newBuildItem.GetMetadata(ITEMMETADATA_GENERATOR)))
							{
								newProjectItem.Properties.Item("CustomTool").Value = customTool;
							}
						}
					};

					foreach (KeyValuePair<string, PseudoBuildItem> keyedPseudoItem in pseudoItems)
					{
						pseudoItem = keyedPseudoItem.Value;
						string allGenerators = pseudoItem.CurrentGeneratorNames;
						string primaryGenerator = ORMCustomToolUtility.GetPrimaryGeneratorName(allGenerators);
						if (allGenerators == primaryGenerator)
						{
							allGenerators = null;
						}

						IORMGenerator generator = null;
						string outputFormat = null;
						ORMCustomToolUtility.GeneratorTargetSet targetSet = null;
						if (null != primaryGenerator)
						{
							generator = generators[primaryGenerator];
							outputFormat = generator.ProvidesOutputFormat;
							if (targetSetsByFormatName != null)
							{
								targetSetsByFormatName.TryGetValue(outputFormat, out targetSet);
							}
						}

						if (targetSet != null)
						{
							// OriginalInstances were already updated in the remove loop and processed
							// instances were flagged. Find additional instances from the target set (created
							// just now from the current model), not from the pseudoItem (created from the project
							// files that possibly reflect a previous version of the model).
							GeneratorTarget[][] instances = targetSet.Instances;
							BitTracker processed = processedGeneratorTargets[outputFormat];

							for (int i = 0, count = instances.Length; i < count; ++i)
							{
								if (!processed[i])
								{
									addProjectItem(generator, allGenerators, targetSet, instances[i]);
								}
							}
						}
						else if (pseudoItem.OriginalInstances == null)
						{
							addProjectItem(generator, allGenerators, null, null);
						}
						else
						{
							// Make sure there was an original instance that did not have a target set.
							List<PseudoBuildInstance> originals = pseudoItem.OriginalInstances;
							int i = 0, count = originals.Count;
							for (; i < count; ++i)
							{
								if (originals[i].OriginalGeneratorTargets == null)
								{
									break;
								}
							}

							if (i == count)
							{
								addProjectItem(generator, allGenerators, null, null);
							}
						}
					}
				}
				finally
				{
					if (tmpFile != null)
					{
						File.Delete(tmpFile);
					}
				}

				if (sideEffectItemNames != null)
				{
					ORMCustomToolUtility.RemoveSideEffectItems(sideEffectItemNames, project, itemGroup);
				}

#if VISUALSTUDIO_10_0
				// Old group remove themselves when empty, but this is
				// not true in the new build system. Clean up as needed.
				if (itemGroup != null &&
					itemGroup.Items.Count == 0)
				{
					project.RemoveChild(itemGroup);
				}
#endif
				VSLangProj.VSProjectItem vsProjectItem = projectItem.Object as VSLangProj.VSProjectItem;
				if (vsProjectItem != null)
				{
					vsProjectItem.RunCustomTool();
				}
			}
			base.OnClosed(e);
		}

		private void SaveChanges(object sender, EventArgs e)
		{
			this._savedChanges = true;
			this.Close();
		}

		private void Cancel(object sender, EventArgs e)
		{
			this._savedChanges = false;
			this.Close();
		}
	}
}
