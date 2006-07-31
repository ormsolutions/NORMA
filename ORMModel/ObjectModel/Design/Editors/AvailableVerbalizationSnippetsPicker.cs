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
using Neumont.Tools.Modeling.Design;

namespace Neumont.Tools.ORM.ObjectModel.Design
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
			IBranch rootBranch = new ProviderBranch((string)value, SnippetsProviders, VerbalizationDirectory, LanguageFormatString);
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
				public int FirstIdentifier;
				public int LastIdentifier;
				public int CurrentIdentifier;
			}
			#endregion // SnippetsType structure
			#region Member Variables
			private List<SnippetsType> myTypes;
			private VerbalizationSnippetsIdentifier[] myIdentifiers;
			private string[] myItemStrings;
			private string myLanguageFormatString;
			#endregion // Member Variables
			#region Constructors
			public ProviderBranch(string currentSettings, IEnumerable<IVerbalizationSnippetsProvider> providers, string verbalizationDirectory, string languageFormatString)
			{
				VerbalizationSnippetsIdentifier[] allIdentifiers = VerbalizationSnippetSetsManager.LoadAvailableSnippets(
					providers,
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

				// Gather all types
				List<SnippetsType> types = new List<SnippetsType>();
				foreach (IVerbalizationSnippetsProvider provider in providers)
				{
					VerbalizationSnippetsData[] currentData = provider.ProvideVerbalizationSnippets();
					int count = (currentData != null) ? currentData.Length : 0;
					for (int i = 0; i < count; ++i)
					{
						SnippetsType currentType = default(SnippetsType);
						currentType.TypeDescription = currentData[i].TypeDescription;
						currentType.EnumTypeName = currentData[i].EnumType.FullName;
						types.Add(currentType);
					}
				}

				// Sort first by type description
				types.Sort(
					delegate(SnippetsType type1, SnippetsType type2)
					{
						return string.Compare(type1.TypeDescription, type2.TypeDescription, StringComparison.CurrentCultureIgnoreCase);
					});

				// Sort all identifiers. First by type description on previous sort, then
				// putting the default identifier first, then by the identifier description
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

				// Now, associate indices in the sorted allIdentifiersList with each type
				int allIdentifiersCount = allIdentifiers.Length;
				int nextIdentifier = 0;
				for (int i = 0; i < typesCount; ++i)
				{
					SnippetsType currentType = types[i];
					string matchTypeName = currentType.EnumTypeName;
					currentType.FirstIdentifier = nextIdentifier;
					currentType.LastIdentifier = nextIdentifier;
					currentType.CurrentIdentifier = nextIdentifier;
					Debug.Assert(allIdentifiers[nextIdentifier].IsDefaultIdentifier && allIdentifiers[nextIdentifier].EnumTypeName == matchTypeName, "No default snippets identifier for " + matchTypeName);
					++nextIdentifier;
					bool matchedCurrent = currentIdentifiers == null;
					for (; nextIdentifier < allIdentifiersCount; ++nextIdentifier)
					{
						if (allIdentifiers[nextIdentifier].EnumTypeName != matchTypeName)
						{
							break;
						}
						currentType.LastIdentifier = nextIdentifier;
						if (!matchedCurrent)
						{
							if (Array.IndexOf<VerbalizationSnippetsIdentifier>(currentIdentifiers, allIdentifiers[nextIdentifier]) >= 0)
							{
								currentType.CurrentIdentifier = nextIdentifier;
								matchedCurrent = true;
							}
						}
					}
					types[i] = currentType;
				}
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
			/// An enumerable of current identifiers
			/// </summary>
			public IEnumerable<VerbalizationSnippetsIdentifier> CurrentIdentifiers
			{
				get
				{
					List<SnippetsType> types = myTypes;
					if (types != null)
					{
						VerbalizationSnippetsIdentifier[] allIdentifiers = myIdentifiers;
						int typesCount = types.Count;
						for (int i = 0; i < typesCount; ++i)
						{
							yield return allIdentifiers[types[i].CurrentIdentifier];
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
				return new Subbranch(this, row);
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				retVal.Bold = true;
				return retVal;
			}
			#endregion // IBranch implementation
			#region Subbranch class
			private sealed class Subbranch : BaseBranch, IBranch
			{
				#region Member Variables
				private ProviderBranch myParentBranch;
				private int myTypeIndex;
				#endregion // Member Variables
				#region Constructors
				public Subbranch(ProviderBranch parentBranch, int typeIndex)
				{
					myParentBranch = parentBranch;
					myTypeIndex = typeIndex;
				}
				#endregion // Constructors
				#region IBranch implementation
				int IBranch.VisibleItemCount
				{
					get
					{
						SnippetsType type = myParentBranch.myTypes[myTypeIndex];
						return type.LastIdentifier - type.FirstIdentifier + 1;
					}
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.StateChanges;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					ProviderBranch parentBranch = myParentBranch;
					string[] itemStrings = parentBranch.myItemStrings;
					string retVal;
					if (itemStrings != null)
					{
						retVal = itemStrings[row];
						if (retVal == null)
						{
							SnippetsType type = parentBranch.myTypes[myTypeIndex];
							VerbalizationSnippetsIdentifier id = parentBranch.myIdentifiers[type.FirstIdentifier + row];
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
						retVal = parentBranch.myIdentifiers[parentBranch.myTypes[myTypeIndex].FirstIdentifier + row].Description;
					}
					return retVal;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					SnippetsType type = myParentBranch.myTypes[myTypeIndex];
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
				StateRefreshChanges IBranch.ToggleState(int row, int column)
				{
					SnippetsType type = myParentBranch.myTypes[myTypeIndex];
					int identifierIndex = type.FirstIdentifier + row;
					if (type.CurrentIdentifier != identifierIndex)
					{
						type.CurrentIdentifier = identifierIndex;
						myParentBranch.myTypes[myTypeIndex] = type;
						return StateRefreshChanges.ParentsChildren;
					}
					return StateRefreshChanges.None;
				}
				#endregion // IBranch implementation
			}
			#endregion // Subbranch class
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
