#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// Editor for ring constraint types enables simplified feedback for valid ring combinations.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class RingConstraintTypePicker : TreePicker<RingConstraintTypePicker>
	{
		#region TreePicker overrides
		static RingConstraintTypePicker()
		{
			LastControlSizeStorage = new Size(200, 220);
		}
		/// <summary>
		/// Get a checkbox-based tree with uncombined ring types
		/// </summary>
		protected sealed override ITree GetTree(ITypeDescriptorContext context, object value)
		{
			ITree tree = new VirtualTree();
			RingTypeBranch branch = new RingTypeBranch(EditorUtility.ResolveContextInstance(context.Instance, true) as ModelElement);
			tree.Root = branch;
			branch.Selection = (RingConstraintType)value;
			return tree;
		}
		/// <summary>
		/// Set control display options
		/// </summary>
		protected sealed override Control SetTreeControlDisplayOptions(VirtualTreeControl treeControl)
		{
			treeControl.HasRootLines = false;
			treeControl.HasLines = false;
			treeControl.ImageList = ResourceStrings.SurveyTreeImageList;
			return null;
		}
		/// <summary>
		/// Checkbox based UI, value not selection based
		/// </summary>
		protected sealed override bool AlwaysTranslateToValue
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Get the final value from the single selection values
		/// </summary>
		protected sealed override object TranslateToValue(ITypeDescriptorContext context, object oldValue, ITree tree, int selectedRow, int selectedColumn)
		{
			return ((RingTypeBranch)tree.Root).Selection;
		}
		/// <summary>
		/// Selects an initial single value
		/// </summary>
		protected override void SelectInitialValue(object value, VirtualTreeControl control)
		{
			int index = ((RingTypeBranch)control.Tree.Root).GetIndexOfSingleValue((RingConstraintType)value);
			if (index != -1)
			{
				control.CurrentIndex = index;
			}
		}
		#endregion // TreePicker overrides
		#region RingTypeBranch class
		private sealed class RingTypeBranch : IBranch
		{
			#region SingleRingTypeInfo struct
			private struct SingleRingTypeInfo
			{
				public readonly RingConstraintType NodeType;
				public readonly RingConstraintType[] IncompatibleWith;
				public readonly RingConstraintType[] IncompatibleWithCombination; // Theoretically [][], but doesn't actually happen
				public readonly RingConstraintType[] ImpliedBy;
				public readonly RingConstraintType[] ImpliedByCombination; // Theoretically [][], but doesn't actually happen
				public readonly RingConstraintType[] UsedInCombinationBy;
				public readonly string DisplayName;
				public readonly string Description;
				public readonly SurveyQuestionGlyph Glyph;
				public SingleRingTypeInfo(
					RingConstraintType nodeType,
					RingConstraintType[] incompatibleWith,
					RingConstraintType[] incompatibleWithCombination,
					RingConstraintType[] impliedBy,
					RingConstraintType[] impliedByCombination,
					RingConstraintType[] usedInCombinationBy,
					string displayName,
					string description,
					SurveyQuestionGlyph glyph)
				{
					NodeType = nodeType;
					IncompatibleWith = incompatibleWith;
					IncompatibleWithCombination = incompatibleWithCombination;
					ImpliedBy = impliedBy;
					ImpliedByCombination = impliedByCombination;
					UsedInCombinationBy = usedInCombinationBy;
					DisplayName = displayName;
					Description = description;
					Glyph = glyph;
				}
			}
			#endregion // SingleRingTypeInfo struct
			#region Member Variables
			private const int SingleRingTypeCount = 10;
			private static SingleRingTypeInfo[] mySingleNodes;
			private static int[] myEnumValueToPositionMap;
			private static int myImageBase = -1;
			private BitTracker mySelectionTracker;
			private int mySelectedCount;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a ring type branch with default settings
			/// </summary>
			/// <param name="element">An ring constraint element, although any element will do.
			/// Used to initialize glyph information only.</param>
			public RingTypeBranch(ModelElement element)
			{
				if (mySingleNodes == null)
				{
					string[] enumNames = Utility.GetLocalizedEnumNames(typeof(RingConstraintType), true);
					// List nodes by relative strength with negative ring types first, then positive ring types
					SingleRingTypeInfo[] singleNodes = new SingleRingTypeInfo[] {
						new SingleRingTypeInfo(
							RingConstraintType.Irreflexive,
							new RingConstraintType[]{RingConstraintType.Reflexive, RingConstraintType.Antisymmetric, RingConstraintType.PurelyReflexive}, // Antisymmetric and Irreflexive make Asymmetric, don't allow both to be selected
							new RingConstraintType[]{RingConstraintType.Symmetric, RingConstraintType.Transitive},
							new RingConstraintType[]{RingConstraintType.Asymmetric, RingConstraintType.Intransitive, RingConstraintType.StronglyIntransitive, RingConstraintType.Acyclic},
							null,
							null,
							enumNames[(int)RingConstraintType.Irreflexive],
							ResourceStrings.RingConstraintTypeDescriptionIrreflexive,
							SurveyQuestionGlyph.RingIrreflexive)
						, new SingleRingTypeInfo(
							RingConstraintType.Antisymmetric,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Intransitive, RingConstraintType.StronglyIntransitive, RingConstraintType.Symmetric, RingConstraintType.PurelyReflexive},
							null,
							new RingConstraintType[]{RingConstraintType.Asymmetric, RingConstraintType.Acyclic},
							null,
							null,
							enumNames[(int)RingConstraintType.Antisymmetric],
							ResourceStrings.RingConstraintTypeDescriptionAntisymmetric,
							SurveyQuestionGlyph.RingAntisymmetric)
						, new SingleRingTypeInfo(
							RingConstraintType.Asymmetric,
							new RingConstraintType[]{RingConstraintType.Antisymmetric, RingConstraintType.Irreflexive, RingConstraintType.Reflexive, RingConstraintType.Symmetric, RingConstraintType.PurelyReflexive},
							null,
							new RingConstraintType[]{RingConstraintType.Acyclic},
							null,
							null,
							enumNames[(int)RingConstraintType.Asymmetric],
							ResourceStrings.RingConstraintTypeDescriptionAsymmetric,
							SurveyQuestionGlyph.RingAsymmetric)
						, new SingleRingTypeInfo(
							RingConstraintType.Intransitive,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Antisymmetric, RingConstraintType.Transitive, RingConstraintType.Reflexive, RingConstraintType.PurelyReflexive},
							null,
							new RingConstraintType[]{RingConstraintType.StronglyIntransitive},
							null,
							null,
							enumNames[(int)RingConstraintType.Intransitive],
							ResourceStrings.RingConstraintTypeDescriptionIntransitive,
							SurveyQuestionGlyph.RingIntransitive)
						, new SingleRingTypeInfo(
							RingConstraintType.StronglyIntransitive,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Antisymmetric, RingConstraintType.Intransitive, RingConstraintType.Transitive, RingConstraintType.Reflexive, RingConstraintType.PurelyReflexive},
							null,
							null,
							null,
							null,
							enumNames[(int)RingConstraintType.StronglyIntransitive],
							ResourceStrings.RingConstraintTypeDescriptionStronglyIntransitive,
							SurveyQuestionGlyph.RingStronglyIntransitive)
						, new SingleRingTypeInfo(
							RingConstraintType.Acyclic,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Antisymmetric, RingConstraintType.Asymmetric, RingConstraintType.Reflexive, RingConstraintType.Symmetric, RingConstraintType.PurelyReflexive},
							null,
							null,
							null,
							null,
							enumNames[(int)RingConstraintType.Acyclic],
							ResourceStrings.RingConstraintTypeDescriptionAcyclic,
							SurveyQuestionGlyph.RingAcyclic)
						, new SingleRingTypeInfo(
							RingConstraintType.PurelyReflexive,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Antisymmetric, RingConstraintType.Asymmetric, RingConstraintType.Intransitive, RingConstraintType.StronglyIntransitive, RingConstraintType.Acyclic, RingConstraintType.Reflexive, RingConstraintType.Symmetric, RingConstraintType.Transitive},
							null,
							null,
							null,
							null,
							enumNames[(int)RingConstraintType.PurelyReflexive],
							ResourceStrings.RingConstraintTypeDescriptionPurelyReflexive,
							SurveyQuestionGlyph.RingPurelyReflexive)
						// Note that if any two of Reflexive/Symmetric/Transitive are selected then the
						// third remains bold (or implied if Symmetric/Transitive is selected). This is
						// slightly inconsistent with the other selections, but is reasonable from a
						// usability perspective as selecting the third positive ring type will simply
						// make reflexive implied, not turn it off.
						, new SingleRingTypeInfo(
							RingConstraintType.Reflexive,
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Asymmetric, RingConstraintType.Intransitive, RingConstraintType.StronglyIntransitive, RingConstraintType.Acyclic},
							null,
							new RingConstraintType[]{RingConstraintType.PurelyReflexive},
							new RingConstraintType[]{RingConstraintType.Symmetric, RingConstraintType.Transitive},
							null,
							enumNames[(int)RingConstraintType.Reflexive],
							ResourceStrings.RingConstraintTypeDescriptionReflexive,
							SurveyQuestionGlyph.RingReflexive)
						, new SingleRingTypeInfo(
							RingConstraintType.Symmetric,
							new RingConstraintType[]{RingConstraintType.Antisymmetric, RingConstraintType.Asymmetric, RingConstraintType.Acyclic},
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Transitive},
							new RingConstraintType[]{RingConstraintType.PurelyReflexive},
							null,
							new RingConstraintType[]{RingConstraintType.Reflexive, RingConstraintType.Irreflexive, RingConstraintType.Transitive},
							enumNames[(int)RingConstraintType.Symmetric],
							ResourceStrings.RingConstraintTypeDescriptionSymmetric,
							SurveyQuestionGlyph.RingSymmetric)
						, new SingleRingTypeInfo(
							RingConstraintType.Transitive,
							new RingConstraintType[]{RingConstraintType.Intransitive, RingConstraintType.StronglyIntransitive, RingConstraintType.PurelyReflexive},
							new RingConstraintType[]{RingConstraintType.Irreflexive, RingConstraintType.Symmetric},
							null,
							null,
							new RingConstraintType[]{RingConstraintType.Reflexive, RingConstraintType.Irreflexive, RingConstraintType.Symmetric},
							enumNames[(int)RingConstraintType.Transitive],
							ResourceStrings.RingConstraintTypeDescriptionTransitive,
							SurveyQuestionGlyph.RingTransitive)
					};
					int[] enumToPositionMap = new int[SingleRingTypeCount + 1]; // Note that undefined is at 0, so real values start at 1
					for (int i = 0; i < SingleRingTypeCount; ++i)
					{
						enumToPositionMap[(int)singleNodes[i].NodeType] = i;
					}
					mySingleNodes = singleNodes;
					myEnumValueToPositionMap = enumToPositionMap;
				}
				Store store;
				if (myImageBase == -1 &&
					element != null &&
					null != (store = Utility.ValidateStore(element.Store)))
				{
					// Find the base offset for the SurveyElementGlyph question in the image list.
					Type questionType = typeof(SurveyQuestionGlyph);
					foreach (ISurveyQuestionTypeInfo questionInfo in ((ISurveyQuestionProvider<Store>)store.GetDomainModel<ORMCoreDomainModel>()).GetSurveyQuestions(store, null))
					{
						if (questionInfo.QuestionType == questionType)
						{
							myImageBase = questionInfo.MapAnswerToImageIndex(0);
							break;
						}
					}
				}
				mySelectionTracker.Resize(SingleRingTypeCount);
				mySelectedCount = 0;
			}
			#endregion // Constructor
			#region Translation Methods
			/// <summary>
			/// Get the current index of a single-valued ring type. Returns -1 if the value is not singular.
			/// </summary>
			public int GetIndexOfSingleValue(RingConstraintType value)
			{
				switch (value)
				{
					//case RingConstraintType.Undefined:
					case RingConstraintType.Reflexive:
					case RingConstraintType.PurelyReflexive:
					case RingConstraintType.Irreflexive:
					case RingConstraintType.Symmetric:
					case RingConstraintType.Antisymmetric:
					case RingConstraintType.Asymmetric:
					case RingConstraintType.Transitive:
					case RingConstraintType.Intransitive:
					case RingConstraintType.StronglyIntransitive:
					case RingConstraintType.Acyclic:
						return myEnumValueToPositionMap[(int)value];
				}
				return -1;
			}
			/// <summary>
			/// Set the selection to match the given ring type
			/// </summary>
			public RingConstraintType Selection
			{
				get
				{
					RingConstraintType retVal = RingConstraintType.Undefined;
					if (mySelectedCount != 0)
					{
						BitTracker tracker = mySelectionTracker;
						int[] positionMap = myEnumValueToPositionMap;
						bool isReflexive = tracker[positionMap[(int)RingConstraintType.Reflexive]];
						bool isIrreflexive = tracker[positionMap[(int)RingConstraintType.Irreflexive]];
						bool isSymmetric = tracker[positionMap[(int)RingConstraintType.Symmetric]];
						bool isAntisymmetric = tracker[positionMap[(int)RingConstraintType.Antisymmetric]];
						bool isAsymmetric = tracker[positionMap[(int)RingConstraintType.Asymmetric]];
						bool isTransitive = tracker[positionMap[(int)RingConstraintType.Transitive]];
						bool isIntransitive = tracker[positionMap[(int)RingConstraintType.Intransitive]];
						bool isStronglyIntransitive = tracker[positionMap[(int)RingConstraintType.StronglyIntransitive]];
						bool isAcyclic = tracker[positionMap[(int)RingConstraintType.Acyclic]];
						bool isPurelyReflexive = tracker[positionMap[(int)RingConstraintType.PurelyReflexive]];
						if (isReflexive)
						{
							if (isTransitive)
							{
								retVal = isAntisymmetric ? RingConstraintType.ReflexiveTransitiveAntisymmetric : RingConstraintType.ReflexiveTransitive;
							}
							else if (isSymmetric)
							{
								retVal = RingConstraintType.ReflexiveSymmetric;
							}
							else if (isAntisymmetric)
							{
								retVal = RingConstraintType.ReflexiveAntisymmetric;
							}
							else
							{
								retVal = RingConstraintType.Reflexive;
							}
						}
						else if (isTransitive)
						{
							if (isAntisymmetric)
							{
								retVal = RingConstraintType.TransitiveAntisymmetric;
							}
							else if (isAsymmetric)
							{
								retVal = RingConstraintType.TransitiveAsymmetric;
							}
							else if (isIrreflexive)
							{
								retVal = RingConstraintType.TransitiveIrreflexive;
							}
							else if (isAcyclic)
							{
								retVal = RingConstraintType.AcyclicTransitive;
							}
							else if (isSymmetric)
							{
								retVal = RingConstraintType.SymmetricTransitive;
							}
							else
							{
								retVal = RingConstraintType.Transitive;
							}
						}
						else if (isSymmetric)
						{
							if (isIrreflexive)
							{
								retVal = RingConstraintType.SymmetricIrreflexive;
							}
							else if (isIntransitive)
							{
								retVal = RingConstraintType.SymmetricIntransitive;
							}
							else if (isStronglyIntransitive)
							{
								retVal = RingConstraintType.SymmetricStronglyIntransitive;
							}
							else
							{
								retVal = RingConstraintType.Symmetric;
							}
						}
						else if (isAsymmetric)
						{
							if (isIntransitive)
							{
								retVal = RingConstraintType.AsymmetricIntransitive;
							}
							else if (isStronglyIntransitive)
							{
								retVal = RingConstraintType.AsymmetricStronglyIntransitive;
							}
							else
							{
								retVal = RingConstraintType.Asymmetric;
							}
						}
						else if (isAcyclic)
						{
							if (isIntransitive)
							{
								retVal = RingConstraintType.AcyclicIntransitive;
							}
							else if (isStronglyIntransitive)
							{
								retVal = RingConstraintType.AcyclicStronglyIntransitive;
							}
							else
							{
								retVal = RingConstraintType.Acyclic;
							}
						}
						else if (isAntisymmetric)
						{
							retVal = RingConstraintType.Antisymmetric;
						}
						else if (isIntransitive)
						{
							retVal = RingConstraintType.Intransitive;
						}
						else if (isStronglyIntransitive)
						{
							retVal = RingConstraintType.StronglyIntransitive;
						}
						else if (isIrreflexive)
						{
							retVal = RingConstraintType.Irreflexive;
						}
						else if (isPurelyReflexive)
						{
							retVal = RingConstraintType.PurelyReflexive;
						}
					}
					return retVal;
				}
				set
				{
					RingConstraintType ringType1 = RingConstraintType.Undefined;
					RingConstraintType ringType2 = RingConstraintType.Undefined;
					RingConstraintType ringType3 = RingConstraintType.Undefined;
					switch (value)
					{
						//case RingConstraintType.Undefined:
						case RingConstraintType.Reflexive:
						case RingConstraintType.Irreflexive:
						case RingConstraintType.Symmetric:
						case RingConstraintType.Antisymmetric:
						case RingConstraintType.Asymmetric:
						case RingConstraintType.Transitive:
						case RingConstraintType.Intransitive:
						case RingConstraintType.StronglyIntransitive:
						case RingConstraintType.Acyclic:
						case RingConstraintType.PurelyReflexive:
							ringType1 = value;
							break;
						case RingConstraintType.AcyclicTransitive:
							ringType1 = RingConstraintType.Acyclic;
							ringType2 = RingConstraintType.Transitive;
							break;
						case RingConstraintType.AcyclicIntransitive:
							ringType1 = RingConstraintType.Acyclic;
							ringType2 = RingConstraintType.Intransitive;
							break;
						case RingConstraintType.AcyclicStronglyIntransitive:
							ringType1 = RingConstraintType.Acyclic;
							ringType2 = RingConstraintType.StronglyIntransitive;
							break;
						case RingConstraintType.ReflexiveSymmetric:
							ringType1 = RingConstraintType.Reflexive;
							ringType2 = RingConstraintType.Symmetric;
							break;
						case RingConstraintType.ReflexiveAntisymmetric:
							ringType1 = RingConstraintType.Reflexive;
							ringType2 = RingConstraintType.Antisymmetric;
							break;
						case RingConstraintType.ReflexiveTransitive:
							ringType1 = RingConstraintType.Reflexive;
							ringType2 = RingConstraintType.Transitive;
							break;
						case RingConstraintType.ReflexiveTransitiveAntisymmetric:
							ringType1 = RingConstraintType.Reflexive;
							ringType2 = RingConstraintType.Transitive;
							ringType3 = RingConstraintType.Antisymmetric;
							break;
						case RingConstraintType.SymmetricTransitive:
							ringType1 = RingConstraintType.Symmetric;
							ringType2 = RingConstraintType.Transitive;
							break;
						case RingConstraintType.SymmetricIrreflexive:
							ringType1 = RingConstraintType.Symmetric;
							ringType2 = RingConstraintType.Irreflexive;
							break;
						case RingConstraintType.SymmetricIntransitive:
							ringType1 = RingConstraintType.Symmetric;
							ringType2 = RingConstraintType.Intransitive;
							break;
						case RingConstraintType.SymmetricStronglyIntransitive:
							ringType1 = RingConstraintType.Symmetric;
							ringType2 = RingConstraintType.StronglyIntransitive;
							break;
						case RingConstraintType.AsymmetricIntransitive:
							ringType1 = RingConstraintType.Asymmetric;
							ringType2 = RingConstraintType.Intransitive;
							break;
						case RingConstraintType.AsymmetricStronglyIntransitive:
							ringType1 = RingConstraintType.Asymmetric;
							ringType2 = RingConstraintType.StronglyIntransitive;
							break;
						case RingConstraintType.TransitiveIrreflexive:
							ringType1 = RingConstraintType.Transitive;
							ringType2 = RingConstraintType.Irreflexive;
							break;
						case RingConstraintType.TransitiveAntisymmetric:
							ringType1 = RingConstraintType.Transitive;
							ringType2 = RingConstraintType.Antisymmetric;
							break;
						case RingConstraintType.TransitiveAsymmetric:
							ringType1 = RingConstraintType.Transitive;
							ringType2 = RingConstraintType.Asymmetric;
							break;
					}
					int[] positionMap = myEnumValueToPositionMap;
					int count = 0;
					if (ringType1 != RingConstraintType.Undefined)
					{
						mySelectionTracker[positionMap[(int)ringType1]] = true;
						++count;
					}
					if (ringType2 != RingConstraintType.Undefined)
					{
						mySelectionTracker[positionMap[(int)ringType2]] = true;
						++count;
					}
					if (ringType3 != RingConstraintType.Undefined)
					{
						mySelectionTracker[positionMap[(int)ringType3]] = true;
						++count;
					}
					mySelectedCount = count;
				}
			}
			#endregion // Translation Methods
			#region IBranch Implementation
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.StateChanges;
				}
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
				SingleRingTypeInfo typeNode = mySingleNodes[row];
				if (mySelectedCount != 0)
				{
					BitTracker selection = mySelectionTracker;
					bool isSelected = false;
					bool isImplied = false;
					bool isBold = false;
					if (selection[row])
					{
						// Currently on, nothing to check
						isBold = isSelected = true;
					}
					else
					{
						// Current state depends on state of other rows
						int[] positionMap = myEnumValueToPositionMap;
						RingConstraintType[] relatedTypes = typeNode.ImpliedBy;
						if (relatedTypes != null)
						{
							for (int i = 0; i < relatedTypes.Length; ++i)
							{
								if (selection[positionMap[(int)relatedTypes[i]]])
								{
									isImplied = true;
									break;
								}
							}
						}
						if (!isImplied &&
							null != (relatedTypes = typeNode.ImpliedByCombination))
						{
							isImplied = true;
							for (int i = 0; i < relatedTypes.Length; ++i)
							{
								if (!selection[positionMap[(int)relatedTypes[i]]])
								{
									isImplied = false;
									break;
								}
							}
						}
						if (!isImplied) // Implied items are never bold
						{
							isBold = true; // Assume true, determine otherwise
							relatedTypes = typeNode.IncompatibleWith;
							for (int i = 0; i < relatedTypes.Length; ++i)
							{
								if (selection[positionMap[(int)relatedTypes[i]]])
								{
									isBold = false;
									break;
								}
							}
							if (isBold &&
								null != (relatedTypes = typeNode.IncompatibleWithCombination))
							{
								isBold = false;
								for (int i = 0; i < relatedTypes.Length; ++i)
								{
									if (!selection[positionMap[(int)relatedTypes[i]]])
									{
										isBold = true;
										break;
									}
								}
							}
						}
					}
					displayData.Bold = isBold;
					if (isSelected)
					{
						displayData.StateImageIndex = (short)StandardCheckBoxImage.CheckedFlat;
					}
					else if (isImplied)
					{
						displayData.StateImageIndex = (short)StandardCheckBoxImage.IndeterminateFlat;
					}
					else
					{
						displayData.StateImageIndex = (short)StandardCheckBoxImage.UncheckedFlat;
					}
				}
				else
				{
					displayData.StateImageIndex = (short)StandardCheckBoxImage.UncheckedFlat;
				}
				int imageIndex = myImageBase;
				if (imageIndex != -1)
				{
					displayData.SelectedImage = displayData.Image = (short)(imageIndex + (int)typeNode.Glyph);
				}
				return displayData;
			}
			string IBranch.GetText(int row, int column)
			{
				return mySingleNodes[row].DisplayName;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				switch (tipType)
				{
					case ToolTipType.Icon:
						return mySingleNodes[row].Description;
					case ToolTipType.StateIcon:
						return "";
				}
				return null;
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return SingleRingTypeCount;
				}
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				BitTracker tracker = mySelectionTracker;
				if (tracker[row])
				{
					// Just turn it off, other states will display differently with the refresh
					mySelectionTracker[row] = false;
					--mySelectedCount;
				}
				else
				{
					// Turning this item on turns off implied and incompatible items. Find them and turn them off.
					int count = mySelectedCount;
					tracker[row] = true;
					++count;
					SingleRingTypeInfo info = mySingleNodes[row];
					int[] positionMap = myEnumValueToPositionMap;
					
					// Process single items
					RingConstraintType[] relatedTypes = info.ImpliedBy;
					bool checkedIncompatible = false;
					if (relatedTypes == null)
					{
						checkedIncompatible = true;
						relatedTypes = info.IncompatibleWith;
					}
					while (relatedTypes != null)
					{
						for (int i = 0; i < relatedTypes.Length; ++i)
						{
							int position = positionMap[(int)relatedTypes[i]];
							if (tracker[position])
							{
								tracker[position] = false;
								--count;
							}
						}
						if (checkedIncompatible)
						{
							break;
						}
						else
						{
							checkedIncompatible = true;
							relatedTypes = info.IncompatibleWith;
						}
					}

					// Turn off full combination items
					relatedTypes = info.ImpliedByCombination;
					checkedIncompatible = false;
					if (relatedTypes == null)
					{
						checkedIncompatible = true;
						relatedTypes = info.IncompatibleWithCombination;
					}
					while (relatedTypes != null)
					{
						int i = 0;
						int combinationCount = relatedTypes.Length;
						for (; i < combinationCount; ++i)
						{
							if (!tracker[positionMap[(int)relatedTypes[i]]])
							{
								break;
							}
						}
						if (i == combinationCount)
						{
							// Turn them all off
							for (i = 0; i < combinationCount; ++i)
							{
								tracker[positionMap[(int)relatedTypes[i]]] = false;
							}
							count -= combinationCount;
						}
						if (checkedIncompatible)
						{
							break;
						}
						else
						{
							checkedIncompatible = true;
							relatedTypes = info.IncompatibleWithCombination;
						}
					}

					// Check if this is part of a complete combination and
					// make sure the combined item is turned off
					RingConstraintType[] usedInCombination = info.UsedInCombinationBy;
					if (usedInCombination != null)
					{
						for (int j = 0; j < usedInCombination.Length; ++j)
						{
							int usedByPosition = positionMap[(int)usedInCombination[j]];
							if (tracker[usedByPosition])
							{
								SingleRingTypeInfo usedByInfo = mySingleNodes[usedByPosition];
								relatedTypes = usedByInfo.ImpliedByCombination;
								checkedIncompatible = false;
								if (relatedTypes == null)
								{
									checkedIncompatible = true;
									relatedTypes = usedByInfo.IncompatibleWithCombination;
								}
								while (relatedTypes != null)
								{
									int i = 0;
									int combinationCount = relatedTypes.Length;
									for (; i < combinationCount; ++i)
									{
										if (!tracker[positionMap[(int)relatedTypes[i]]])
										{
											break;
										}
									}
									if (i == combinationCount)
									{
										tracker[usedByPosition] = false;
										--count;
										break;
									}
									if (checkedIncompatible)
									{
										break;
									}
									else
									{
										checkedIncompatible = true;
										relatedTypes = usedByInfo.IncompatibleWithCombination;
									}
								}
							}
						}
					}
					mySelectionTracker = tracker;
					mySelectedCount = count;
				}
				return StateRefreshChanges.Entire;
			}
			#region Unused Methods
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
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
			int IBranch.UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			#endregion // Unused Methods
			#endregion // IBranch Implementation
		}
		#endregion // RingTypeBranch class
	}
}
