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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	#region AvailableVerbalizationSnippetsPicker class
	/// <summary>
	/// An editor class to choose the current verbalization options
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class AvailableVerbalizationSnippetsPicker<T> : TreePicker<T>
		where T : AvailableVerbalizationSnippetsPicker<T>
	{
		#region Abstract properties
		/// <summary>
		/// The directory containing snippet XML files
		/// </summary>
		protected abstract string VerbalizationDirectory { get;}
		/// <summary>
		/// An enumerator of snippets providers
		/// </summary>
		protected abstract IEnumerable<IVerbalizationSnippetsProvider> SnippetsProviders { get;}
		/// <summary>
		/// An enumerator of target providers
		/// </summary>
		protected abstract IEnumerable<IVerbalizationTargetProvider> TargetProviders { get;}
		/// <summary>
		/// A format string indicating how language information should be combined
		/// with the description of a given snippet. The {0} replacement field gets
		/// the description, the {1} gets the field value. Return null or empty to
		/// not display any language information.
		/// </summary>
		protected abstract string LanguageFormatString { get;}
		#endregion // Abstract properties
		#region TreePicker overrides
		/// <summary>
		/// Get the ITree representing the current verbalization settings
		/// </summary>
		protected override ITree GetTree(ITypeDescriptorContext context, object value)
		{
			ITree tree = new VirtualTree();
			IBranch rootBranch = new ProviderBranch((string)value, TargetProviders, SnippetsProviders, VerbalizationDirectory, LanguageFormatString);
			tree.Root = rootBranch;
			if (rootBranch.VisibleItemCount == 1)
			{
				tree.ToggleExpansion(0, 0);
			}
			return tree;
		}
		/// <summary>
		/// Translate the tree settings to a useable value
		/// </summary>
		protected override object TranslateToValue(ITypeDescriptorContext context, object oldValue, ITree tree, int selectedRow, int selectedColumn)
		{
			return VerbalizationSnippetsIdentifier.SaveIdentifiers(((ProviderBranch)tree.Root).CurrentIdentifiers);
		}
		/// <summary>
		/// Leave the initial selection empty
		/// </summary>
		protected override void SelectInitialValue(object value, VirtualTreeControl control)
		{
			// Nothing to do, leave it blank
		}
		/// <summary>
		/// Selection does not affect the current value of the tree, ignore it
		/// </summary>
		protected override bool AlwaysTranslateToValue
		{
			get
			{
				return true;
			}
		}
		private static Size myLastControlSize = new Size(272, 128);
		/// <summary>
		/// Manage control size independently
		/// </summary>
		protected override Size LastControlSize
		{
			get { return myLastControlSize; }
			set { myLastControlSize = value; }
		}
		#endregion // TreePicker overrides
		#region ProviderBranch class
		/// <summary>
		/// A branch class to display snippet types
		/// </summary>
		private sealed class ProviderBranch : BaseBranch, IBranch
		{
			#region SnippetsType structure
			private struct SnippetsType
			{
				public string TypeDescription;
				public string EnumTypeName;
				public int FirstTargetedType;
				public int FirstExplicitlyTargetedType;
				public int LastTargetedType;
			}
			#endregion // SnippetsType structure
			#region TargetedSnippetsType structure
			private struct TargetedSnippetsType
			{
				public string Target;
				public int TypeIndex;
				public int FirstIdentifier;
				public int LastIdentifier;
				public int CurrentIdentifier;
				public void BindType(int typeIndex, string target)
				{
					TypeIndex = typeIndex;
					Target = target;
				}
			}
			#endregion // TargetedSnippetsType structure
			#region TargetKeyComparer class
			private sealed class TargetKeyComparer : IComparer<VerbalizationTargetData>
			{
				public static readonly IComparer<VerbalizationTargetData> Instance = new TargetKeyComparer();
				private TargetKeyComparer()
				{
				}
				int IComparer<VerbalizationTargetData>.Compare(VerbalizationTargetData x, VerbalizationTargetData y)
				{
					return string.CompareOrdinal(x.KeyName, y.KeyName);
				}
			}
			#endregion // TargetKeyComparer class
			#region Member Variables
			private List<SnippetsType> myTypes;
			private TargetedSnippetsType[] myTargetedTypes;
			private VerbalizationSnippetsIdentifier[] myIdentifiers;
			private string[] myItemStrings;
			private string myLanguageFormatString;
			private VerbalizationTargetData[] myVerbalizationTargets;
			#endregion // Member Variables
			#region Constructors
			public ProviderBranch(string currentSettings, IEnumerable<IVerbalizationTargetProvider> targetProviders, IEnumerable<IVerbalizationSnippetsProvider> snippetProviders, string verbalizationDirectory, string languageFormatString)
			{
				VerbalizationSnippetsIdentifier[] allIdentifiers = VerbalizationSnippetSetsManager.LoadAvailableSnippets(
					snippetProviders,
					verbalizationDirectory);
				VerbalizationSnippetsIdentifier[] currentIdentifiers = VerbalizationSnippetsIdentifier.ParseIdentifiers(currentSettings);

				if (languageFormatString != null)
				{
					if (languageFormatString.Length != 0)
					{
						myLanguageFormatString = languageFormatString;
					}
					else
					{
						languageFormatString = null;
					}
				}

				// Gather all targets
				List<VerbalizationTargetData> targetsList = new List<VerbalizationTargetData>();
				foreach (IVerbalizationTargetProvider provider in targetProviders)
				{
					VerbalizationTargetData[] currentData = provider.ProvideVerbalizationTargets();
					if (currentData != null)
					{
						for (int i = 0; i < currentData.Length; ++i)
						{
							targetsList.Add(currentData[i]);
							// Put it in a condition we can binary search it
							targetsList.Sort(TargetKeyComparer.Instance);
						}
					}
				}
				VerbalizationTargetData[] targets;
				myVerbalizationTargets = targets = targetsList.ToArray();

				// Make sure all identifiers can map to a known target
				int unknownTargetsCount = 0;
				for (int i = 0; i < allIdentifiers.Length; ++i)
				{
					string identifierTarget = allIdentifiers[i].Target;
					if (!string.IsNullOrEmpty(identifierTarget) &&
						0 > Array.BinarySearch<VerbalizationTargetData>(targets, new VerbalizationTargetData(identifierTarget, null), TargetKeyComparer.Instance))
					{
						++unknownTargetsCount;
						allIdentifiers[i] = default(VerbalizationSnippetsIdentifier);
					}
				}
				if (unknownTargetsCount != 0)
				{
					VerbalizationSnippetsIdentifier[] reducedIdentifiers = new VerbalizationSnippetsIdentifier[allIdentifiers.Length - unknownTargetsCount];
					int currentValidIdentifier = 0;
					for (int i = 0; i < allIdentifiers.Length; ++i)
					{
						VerbalizationSnippetsIdentifier identifier = allIdentifiers[i];
						if (!identifier.IsEmpty)
						{
							reducedIdentifiers[currentValidIdentifier] = identifier;
							++currentValidIdentifier;
						}
					}
					allIdentifiers = reducedIdentifiers;
				}

				// Gather all types
				List<SnippetsType> types = new List<SnippetsType>();
				SnippetsType currentType;
				foreach (IVerbalizationSnippetsProvider provider in snippetProviders)
				{
					VerbalizationSnippetsData[] currentData = provider.ProvideVerbalizationSnippets();
					if (currentData != null)
					{
						for (int i = 0; i < currentData.Length; ++i)
						{
							currentType = default(SnippetsType);
							currentType.TypeDescription = currentData[i].TypeDescription;
							currentType.EnumTypeName = currentData[i].EnumType.FullName;
							types.Add(currentType);
						}
					}
				}

				// Sort first by type description
				types.Sort(
					delegate(SnippetsType type1, SnippetsType type2)
					{
						return string.Compare(type1.TypeDescription, type2.TypeDescription, StringComparison.CurrentCultureIgnoreCase);
					});

				// Sort all identifiers. First by type description on previous sort, then
				// putting the default identifier first, then by explicit target types, then by the identifier description
				int typesCount = types.Count;
				Array.Sort<VerbalizationSnippetsIdentifier>(
					allIdentifiers,
					delegate(VerbalizationSnippetsIdentifier identifier1, VerbalizationSnippetsIdentifier identifier2)
					{
						int retVal = 0;
						string typeName1 = identifier1.EnumTypeName;
						string typeName2 = identifier2.EnumTypeName;
						if (typeName1 != typeName2)
						{
							int location1 = -1;
							for (int i = 0; i < typesCount; ++i)
							{
								if (types[i].EnumTypeName == typeName1)
								{
									location1 = i;
									break;
								}
							}
							int location2 = -1;
							for (int i = 0; i < typesCount; ++i)
							{
								if (types[i].EnumTypeName == typeName2)
								{
									location2 = i;
									break;
								}
							}
							retVal = location1.CompareTo(location2);
						}
						if (retVal == 0)
						{
							string target1 = identifier1.Target;
							string target2 = identifier2.Target;
							if (target1 != target2)
							{
								if (target1 == VerbalizationSnippetsIdentifier.DefaultTarget)
								{
									retVal = -1;
								}
								else if (target2 == VerbalizationSnippetsIdentifier.DefaultTarget)
								{
									retVal = 1;
								}
								else
								{
									retVal = string.Compare(GetVerbalizationTargetDisplayName(target1), GetVerbalizationTargetDisplayName(target2), StringComparison.CurrentCultureIgnoreCase);
								}
							}
						}
						if (retVal == 0)
						{
							bool isDefault1 = identifier1.IsDefaultIdentifier;
							bool isDefault2 = identifier2.IsDefaultIdentifier;
							if (isDefault1)
							{
								if (!isDefault2)
								{
									retVal = -1;
								}
							}
							else if (isDefault2)
							{
								retVal = 1;
							}
						}
						if (retVal == 0)
						{
							retVal = string.Compare(identifier1.Description, identifier2.Description, StringComparison.CurrentCultureIgnoreCase);
						}
						return retVal;
					});
				
				// Now get a count of all targeted types
				int allIdentifiersCount = allIdentifiers.Length;
				int targetedTypesCount = 0;
				string lastTarget = null; // Start with invalid target
				string lastEnumTypeName = null;
				for (int i = 0; i < allIdentifiersCount; ++i)
				{
					string currentTarget = allIdentifiers[i].Target;
					if (currentTarget != lastTarget)
					{
						lastEnumTypeName = allIdentifiers[i].EnumTypeName;
						lastTarget = currentTarget;
						++targetedTypesCount;
					}
					else if (lastEnumTypeName != null)
					{
						string currentEnumTypeName = allIdentifiers[i].EnumTypeName;
						if (currentEnumTypeName != lastEnumTypeName)
						{
							lastEnumTypeName = currentEnumTypeName;
							++targetedTypesCount;
						}
					}
				}

				// Allocate targeted types and bind targeted types to types
				TargetedSnippetsType[] targetedTypes = new TargetedSnippetsType[targetedTypesCount];
				lastTarget = null;
				int currentTypeIndex = -1;
				currentType = default(SnippetsType);
				TargetedSnippetsType currentTargetedType = default(TargetedSnippetsType);
				int currentTargetIndex = -1;
				lastEnumTypeName = null;
				for (int i = 0; i < allIdentifiersCount; ++i)
				{
					string currentTarget = allIdentifiers[i].Target;
					string currentEnumTypeName = allIdentifiers[i].EnumTypeName;
					bool enumNameChanged = lastEnumTypeName == null || currentEnumTypeName != lastEnumTypeName;
					if (enumNameChanged || currentTarget != lastTarget)
					{
						++currentTargetIndex;
						lastTarget = currentTarget;
						if (enumNameChanged)
						{
							lastEnumTypeName = currentEnumTypeName;
							if (currentTypeIndex != -1)
							{
								types[currentTypeIndex] = currentType;
							}
							currentType = types[++currentTypeIndex];
							currentType.FirstExplicitlyTargetedType = -1;
							currentType.FirstTargetedType = currentTargetIndex;
							currentType.LastTargetedType = currentTargetIndex;
						}
						else
						{
							currentType.LastTargetedType += 1;
						}
						targetedTypes[currentTargetIndex].BindType(currentTypeIndex, currentTarget);
					}
					if (!string.IsNullOrEmpty(currentTarget) && currentType.FirstExplicitlyTargetedType == -1)
					{
						currentType.FirstExplicitlyTargetedType = currentTargetIndex;
					}
				}
				if (currentTypeIndex != -1)
				{
					types[currentTypeIndex] = currentType;
				}

				// Now, associate indices in the sorted allIdentifiersList with each targeted type
				int nextIdentifier = 0;
				for (int i = 0; i < targetedTypesCount; ++i)
				{
					currentTargetedType = targetedTypes[i];
					string matchTypeName = types[currentTargetedType.TypeIndex].EnumTypeName;
					string matchTarget = currentTargetedType.Target;
					currentTargetedType.FirstIdentifier = nextIdentifier;
					currentTargetedType.LastIdentifier = nextIdentifier;
					currentTargetedType.CurrentIdentifier = nextIdentifier;
					Debug.Assert(allIdentifiers[nextIdentifier].IsDefaultIdentifier && allIdentifiers[nextIdentifier].EnumTypeName == matchTypeName, "No default snippets identifier for " + matchTypeName);
					++nextIdentifier;
					bool matchedCurrent = currentIdentifiers == null;
					for (; nextIdentifier < allIdentifiersCount; ++nextIdentifier)
					{
						if (allIdentifiers[nextIdentifier].Target != matchTarget ||
							allIdentifiers[nextIdentifier].EnumTypeName != matchTypeName)
						{
							break;
						}
						currentTargetedType.LastIdentifier = nextIdentifier;
						if (!matchedCurrent)
						{
							if (Array.IndexOf<VerbalizationSnippetsIdentifier>(currentIdentifiers, allIdentifiers[nextIdentifier]) >= 0)
							{
								currentTargetedType.CurrentIdentifier = nextIdentifier;
								matchedCurrent = true;
							}
						}
					}
					targetedTypes[i] = currentTargetedType;
				}
				myTargetedTypes = targetedTypes;
				myTypes = types;
				myIdentifiers = allIdentifiers;
				if (languageFormatString != null)
				{
					myItemStrings = new string[allIdentifiersCount];
				}
			}
			#endregion // Constructors
			#region ProviderBranch specific
			/// <summary>
			/// Get the display name for the verbalization target
			/// </summary>
			private string GetVerbalizationTargetDisplayName(string targetKey)
			{
				string retVal = null;
				VerbalizationTargetData[] targets = myVerbalizationTargets;
				int targetIndex = Array.BinarySearch<VerbalizationTargetData>(targets, new VerbalizationTargetData(targetKey, ""), TargetKeyComparer.Instance);
				if (targetIndex >= 0)
				{
					retVal = targets[targetIndex].DisplayName;
				}
				return retVal ?? targetKey;
			}
			/// <summary>
			/// An enumerable of current identifiers
			/// </summary>
			public IEnumerable<VerbalizationSnippetsIdentifier> CurrentIdentifiers
			{
				get
				{
					TargetedSnippetsType[] targetedTypes = myTargetedTypes;
					if (targetedTypes != null)
					{
						VerbalizationSnippetsIdentifier[] allIdentifiers = myIdentifiers;
						for (int i = 0; i < targetedTypes.Length; ++i)
						{
							yield return allIdentifiers[targetedTypes[i].CurrentIdentifier];
						}
					}
				}
			}
			#endregion // ProviderBranch specific
			#region IBranch implementation
			int IBranch.VisibleItemCount
			{
				get
				{
					return myTypes.Count;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.Expansions;
				}
			}
			string IBranch.GetText(int row, int column)
			{
				return myTypes[row].TypeDescription;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return true;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				Debug.Assert(style == ObjectStyle.ExpandedBranch);
				if (style != ObjectStyle.ExpandedBranch)
				{
					return null;
				}
				SnippetsType type = myTypes[row];
				int firstTargetedType = type.FirstTargetedType;
				int targetedTypesCount = type.LastTargetedType - firstTargetedType;
				if (targetedTypesCount != 0)
				{
					TargetedTypeData[] typeData = new TargetedTypeData[targetedTypesCount];
					for (int i = 0; i < targetedTypesCount; ++i)
					{
						typeData[i] = new TargetedTypeData(GetVerbalizationTargetDisplayName(myTargetedTypes[firstTargetedType + i + 1].Target), firstTargetedType + i + 1);
					}
					return new TypeBranchWithExplicitTargets(this, firstTargetedType, typeData);
				}
				else
				{
					return new TypeBranch(this, firstTargetedType);
				}
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				retVal.Bold = true;
				return retVal;
			}
			#endregion // IBranch implementation
			#region TypeBranch class
			private class TypeBranch : BaseBranch, IBranch
			{
				#region Member Variables
				private ProviderBranch myParentBranch;
				private int myTargetedTypeIndex;
				#endregion // Member Variables
				#region Constructors
				public TypeBranch(ProviderBranch parentBranch, int targetedTypeIndex)
				{
					myParentBranch = parentBranch;
					myTargetedTypeIndex = targetedTypeIndex;
				}
				#endregion // Constructors
				#region Accessor Functions
				protected ProviderBranch ParentBranch
				{
					get
					{
						return myParentBranch;
					}
				}
				#endregion // Accessor Functions
				#region IBranch implementation
				/// <summary>
				/// The item count for this branch. Implements IBranch.VisibleItemCount;
				/// </summary>
				protected int VisibleItemCount
				{
					get
					{
						TargetedSnippetsType type = myParentBranch.myTargetedTypes[myTargetedTypeIndex];
						return type.LastIdentifier - type.FirstIdentifier + 1;
					}
				}
				int IBranch.VisibleItemCount
				{
					get
					{
						return VisibleItemCount;
					}
				}
				/// <summary>
				/// The features enabled for this branch. Implements IBranch.Features
				/// </summary>
				protected static BranchFeatures Features
				{
					get
					{
						return BranchFeatures.StateChanges;
					}
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return Features;
					}
				}
				/// <summary>
				/// Implements IBranch.Text
				/// </summary>
				protected string GetText(int row, int column)
				{
					ProviderBranch parentBranch = myParentBranch;
					string[] itemStrings = parentBranch.myItemStrings;
					string retVal;
					if (itemStrings != null)
					{
						row += parentBranch.myTargetedTypes[myTargetedTypeIndex].FirstIdentifier;
						retVal = itemStrings[row];
						if (retVal == null)
						{
							VerbalizationSnippetsIdentifier id = parentBranch.myIdentifiers[row];
							if (id.IsDefaultIdentifier)
							{
								retVal = id.Description;
							}
							else
							{
								string languageName = id.LanguageId;
								try
								{
									languageName = CultureInfo.GetCultureInfoByIetfLanguageTag(id.LanguageId).DisplayName;
								}
								catch (ArgumentException)
								{
								}
								retVal = string.Format(CultureInfo.CurrentCulture, parentBranch.myLanguageFormatString, id.Description, languageName);
							}
							itemStrings[row] = retVal;
						}
					}
					else
					{
						retVal = parentBranch.myIdentifiers[parentBranch.myTargetedTypes[myTargetedTypeIndex].FirstIdentifier + row].Description;
					}
					return retVal;
				}
				string IBranch.GetText(int row, int column)
				{
					return GetText(row, column);
				}
				/// <summary>
				/// Implements IBranch.GetDisplayData
				/// </summary>
				protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					TargetedSnippetsType type = myParentBranch.myTargetedTypes[myTargetedTypeIndex];
					if (type.CurrentIdentifier == (type.FirstIdentifier + row))
					{
						retVal.Bold = true;
						retVal.StateImageIndex = (short)StandardCheckBoxImage.CheckedFlat;
					}
					else
					{
						retVal.StateImageIndex = (short)StandardCheckBoxImage.UncheckedFlat;
					}
					return retVal;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					return GetDisplayData(row, column, requiredData);
				}
				protected StateRefreshChanges ToggleState(int row, int column)
				{
					TargetedSnippetsType type = myParentBranch.myTargetedTypes[myTargetedTypeIndex];
					int identifierIndex = type.FirstIdentifier + row;
					if (type.CurrentIdentifier != identifierIndex)
					{
						type.CurrentIdentifier = identifierIndex;
						myParentBranch.myTargetedTypes[myTargetedTypeIndex] = type;
						return StateRefreshChanges.ParentsChildren;
					}
					return StateRefreshChanges.None;
				}
				StateRefreshChanges IBranch.ToggleState(int row, int column)
				{
					return ToggleState(row, column);
				}
				#endregion // IBranch implementation
			}
			#endregion // TypeBranch class
			#region TypeBranchWithExplicitTargets class
			/// <summary>
			/// A structure representing an explicitly targeted snippet expansion
			/// </summary>
			private struct TargetedTypeData
			{
				public TargetedTypeData(string targetDisplayName, int targetedTypeIndex)
				{
					TargetDisplayName = targetDisplayName;
					TargetedTypeIndex = targetedTypeIndex;
				}
				public string TargetDisplayName;
				public int TargetedTypeIndex;
			}
			/// <summary>
			/// A <see cref="TypeBranch"/> extended with othered targeted types
			/// </summary>
			private sealed class TypeBranchWithExplicitTargets : TypeBranch, IBranch
			{
				#region Member Variables
				private TargetedTypeData[] myExpansions;
				#endregion // Member Variables
				#region Constructors
				public TypeBranchWithExplicitTargets(ProviderBranch parentBranch, int typeIndex, TargetedTypeData[] expansions)
					: base(parentBranch, typeIndex)
				{
					Debug.Assert(expansions != null && expansions.Length != 0); // Create a TypeBranch directly if there are no expansions
					myExpansions = expansions;
				}
				#endregion // Constructors
				#region IBranch implementation
				int IBranch.VisibleItemCount
				{
					get
					{
						return base.VisibleItemCount + myExpansions.Length;
					}
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return Features | BranchFeatures.Expansions;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					int baseItemCount = base.VisibleItemCount;
					if (row < baseItemCount)
					{
						return base.GetText(row, column);
					}
					return myExpansions[row - baseItemCount].TargetDisplayName;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					if (row < base.VisibleItemCount)
					{
						return base.GetDisplayData(row, column, requiredData);
					}
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					retVal.Bold = true;
					return retVal;
				}
				bool IBranch.IsExpandable(int row, int column)
				{
					return row >= base.VisibleItemCount;
				}
				object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
				{
					if (style == ObjectStyle.ExpandedBranch)
					{
						int baseItemCount = base.VisibleItemCount;
						if (row >= baseItemCount)
						{
							return new TypeBranch(ParentBranch, myExpansions[row - baseItemCount].TargetedTypeIndex);
						}
					}
					return null;
				}
				StateRefreshChanges IBranch.ToggleState(int row, int column)
				{
					if (row < base.VisibleItemCount)
					{
						return base.ToggleState(row, column);
					}
					return StateRefreshChanges.None;
				}
				#endregion // IBranch implementation
			}
			#endregion // TypeBranchWithExplicitTargets class
		}
		#endregion // ProviderBranch class
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
	}
	#endregion // AvailableVerbalizationSnippetsPicker class
}
