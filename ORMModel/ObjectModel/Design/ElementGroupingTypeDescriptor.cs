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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Neumont.Tools.ORM.Shell;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ElementGrouping"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ElementGroupingTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : ElementGrouping
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ElementGroupingTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ElementGroupingTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Add custom property descriptors 
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection retVal = base.GetProperties(attributes);
			retVal.Add(GroupingTypesPropertyDescriptor.Instance);
			return retVal;
		}
		#endregion // Base overrides
		#region GroupingTypesPropertyDescriptor class
		/// <summary>
		/// A property descriptor to show types for this group
		/// </summary>
		private class GroupingTypesPropertyDescriptor : PropertyDescriptor
		{
			#region Public singleton
			public static readonly PropertyDescriptor Instance = new GroupingTypesPropertyDescriptor();
			#endregion // Public singleton
			#region Private constructor
			private GroupingTypesPropertyDescriptor()
				: base("GroupingTypesPropertyDescriptor", null)
			{

			}
			#endregion // Private constructor
			#region Base overrides
			public override bool CanResetValue(object component)
			{
				return false;
			}
			public override Type ComponentType
			{
				get
				{
					return typeof(ElementGrouping);
				}
			}
			public override object GetValue(object component)
			{
				return null;
			}
			public override bool IsReadOnly
			{
				get { return false; }
			}
			public override Type PropertyType
			{
				get { return typeof(object); }
			}
			public override void ResetValue(object component)
			{
				// Required override
			}
			public override void SetValue(object component, object value)
			{
				// Required override
			}
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			public override string Description
			{
				get
				{
					return ResourceStrings.ElementGroupingTypesPropertyDescriptorDescription;
				}
			}
			public override string DisplayName
			{
				get
				{
					return ResourceStrings.ElementGroupingTypesPropertyDescriptorDisplayName;
				}
			}
			public override object GetEditor(Type editorBaseType)
			{
				if (editorBaseType == typeof(UITypeEditor))
				{
					return new GroupingTypesEditor();
				}
				return base.GetEditor(editorBaseType);
			}
			#endregion // Base overrides
			#region GroupingTypesEditor class
			/// <summary>
			/// Retrieve a tree for the dropdown
			/// </summary>
			private class GroupingTypesEditor : TreePicker<GroupingTypesEditor>
			{
				#region TreePicker overrides
				static GroupingTypesEditor()
				{
					LastControlSizeStorage = new Size(240, 116);
				}
				protected override ITree GetTree(ITypeDescriptorContext context, object value)
				{
					ElementGrouping instance = (ElementGrouping)EditorUtility.ResolveContextInstance(context.Instance, false); // false indicates this should not be called in multiselect mode.
					ITree tree = new VirtualTree();
					tree.Root = new GroupingTypesBranch(instance);
					return tree;
				}
				/// <summary>
				/// Turn off the line display in the tree control
				/// </summary>
				/// <param name="treeControl"></param>
				protected override void SetTreeControlDisplayOptions(VirtualTreeControl treeControl)
				{
					treeControl.HasLines = false;
					treeControl.HasRootButtons = false;
				}
				/// <summary>
				/// Our state is checkbox based, not selection based
				/// </summary>
				protected override bool AlwaysTranslateToValue
				{
					get
					{
						return true;
					}
				}
				/// <summary>
				/// Apply the changes and return null
				/// </summary>
				protected override object TranslateToValue(ITypeDescriptorContext context, object oldValue, ITree tree, int selectedRow, int selectedColumn)
				{
					((GroupingTypesBranch)tree.Root).ApplyChanges();
					return null;
				}
				/// <summary>
				/// Leave the initial selection empty
				/// </summary>
				protected override void SelectInitialValue(object value, VirtualTreeControl control)
				{
					// Nothing to do, leave it blank
				}
				#endregion // TreePicker overrides
				#region GroupingTypesBranch class
				private class GroupingTypesBranch : IBranch
				{
					#region GroupingTypeData struct
					/// <summary>
					/// Specify if a type was initial selected and currently selected
					/// </summary>
					[Flags]
					private enum GroupingTypeSelectionState
					{
						/// <summary>
						/// The element was initially selected
						/// </summary>
						InitiallySelected = 1,
						/// <summary>
						/// The element is currently selected
						/// </summary>
						CurrentlySelected = 2,
					}
					private struct GroupingTypeData
					{
						public GroupingTypeData(ElementGroupingType groupingTypeInstance, DomainClassInfo groupingTypeClassInfo)
						{
							GroupingTypeInstance = groupingTypeInstance;
							GroupingTypeClassInfo = groupingTypeClassInfo;
							Type groupingTypeClass = groupingTypeClassInfo.ImplementationClass;
							DisplayName = DomainTypeDescriptor.GetDisplayName(groupingTypeClass);
							Description = DomainTypeDescriptor.GetDescription(groupingTypeClass);
							SelectionState = (groupingTypeInstance != null) ? (GroupingTypeSelectionState.InitiallySelected | GroupingTypeSelectionState.CurrentlySelected) : 0;
						}
						public ElementGroupingType GroupingTypeInstance;
						public DomainClassInfo GroupingTypeClassInfo;
						public string DisplayName;
						public string Description;
						public GroupingTypeSelectionState SelectionState;
					}
					#endregion // GroupingTypeData struct
					#region Member variables
					private ElementGrouping myContextGrouping;
					private GroupingTypeData[] myGroupTypeData;
					private SurveyGroupingTypeGlyph myGlyphProvider;
					#endregion // Member variables
					#region Constructor
					public GroupingTypesBranch(ElementGrouping grouping)
					{
						myContextGrouping = grouping;
						Store store = grouping.Store;
						ReadOnlyCollection<DomainClassInfo> groupTypes = store.DomainDataDirectory.GetDomainClass(ElementGroupingType.DomainClassId).AllDescendants;
						int groupTypeCount = groupTypes.Count;
						if (groupTypeCount != 0)
						{
							int concreteGroupTypeCount = 0;
							for (int i = 0; i < groupTypeCount; ++i)
							{
								if (!groupTypes[i].ImplementationClass.IsAbstract)
								{
									++concreteGroupTypeCount;
								}
							}
							if (concreteGroupTypeCount != 0)
							{
								LinkedElementCollection<ElementGroupingType> currentTypeInstances = grouping.GroupingTypeCollection;
								int currentTypeInstanceCount = currentTypeInstances.Count;
								GroupingTypeData[] typeData = new GroupingTypeData[concreteGroupTypeCount];
								int nextTypeData = 0;
								for (int i = 0; i < groupTypeCount; ++i)
								{
									DomainClassInfo groupType = groupTypes[i];
									if (!groupType.ImplementationClass.IsAbstract)
									{
										ElementGroupingType matchingTypeInstance = null;
										for (int j = 0; j < currentTypeInstanceCount; ++j)
										{
											ElementGroupingType testInstance = currentTypeInstances[j];
											if (testInstance.GetDomainClass() == groupType)
											{
												matchingTypeInstance = testInstance;
											}
										}
										typeData[nextTypeData] = new GroupingTypeData(matchingTypeInstance, groupType);
										++nextTypeData;
									}
								}
								if (concreteGroupTypeCount > 1)
								{
									Array.Sort<GroupingTypeData>(
										typeData,
										delegate(GroupingTypeData left, GroupingTypeData right)
										{
											return string.Compare(left.DisplayName, right.DisplayName, StringComparison.CurrentCultureIgnoreCase);
										});
								}
								myGroupTypeData = typeData;
								myGlyphProvider = SurveyGroupingTypeGlyph.GetStoreInstance(store);
							}
						}
						
					}
					#endregion // Constructor
					#region Public Methods
					/// <summary>
					/// Apply changes based on the selection state
					/// </summary>
					public void ApplyChanges()
					{
						GroupingTypeData[] types = myGroupTypeData;
						if (types != null)
						{
							int typeCount = types.Length;
							bool hasChanges = false;
							for (int i = 0; i < typeCount; ++i)
							{
								GroupingTypeSelectionState selectionState = types[i].SelectionState;
								if ((0 == (selectionState & GroupingTypeSelectionState.InitiallySelected)) != (0 == (selectionState & GroupingTypeSelectionState.CurrentlySelected)))
								{
									hasChanges = true;
									break;
								}
							}
							if (hasChanges)
							{
								ElementGrouping grouping = myContextGrouping;
								Store store = grouping.Store;
								using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ElementGroupingTypesPropertyDescriptorTransactionName))
								{
									for (int i = 0; i < typeCount; ++i)
									{
										GroupingTypeData type = types[i];
										GroupingTypeSelectionState selectionState = type.SelectionState;
										bool typeAdded;
										if ((typeAdded = (0 == (selectionState & GroupingTypeSelectionState.InitiallySelected))) != (0 == (selectionState & GroupingTypeSelectionState.CurrentlySelected)))
										{
											if (typeAdded)
											{
												((ElementGroupingType)store.ElementFactory.CreateElement(type.GroupingTypeClassInfo)).Grouping = grouping;
											}
											else
											{
												type.GroupingTypeInstance.Delete();
											}
										}
									}
									t.Commit();
								}
							}
						}
					}
					#endregion // Public Methods
					#region IBranch Implementation
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						SurveyGroupingTypeGlyph glyphProvider = myGlyphProvider;
						GroupingTypeData typeData = myGroupTypeData[row];
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						if (0 != (typeData.SelectionState & GroupingTypeSelectionState.CurrentlySelected))
						{
							retVal.Bold = true;
							retVal.StateImageIndex = (short)StandardCheckBoxImage.CheckedFlat;
						}
						else
						{
							retVal.StateImageIndex = (short)StandardCheckBoxImage.UncheckedFlat;
						}
						retVal.ImageList = glyphProvider.GroupingTypeImages;
						retVal.SelectedImage = retVal.Image = (short)glyphProvider.GetGroupingTypeIndex(typeData.GroupingTypeClassInfo);
						return retVal;
					}
					string IBranch.GetText(int row, int column)
					{
						return myGroupTypeData[row].DisplayName;
					}
					string IBranch.GetTipText(int row, int column, ToolTipType tipType)
					{
						switch (tipType)
						{
							case ToolTipType.Icon:
								return myGroupTypeData[row].Description;
							case ToolTipType.StateIcon:
								return "";
						}
						return null;
					}
					StateRefreshChanges IBranch.ToggleState(int row, int column)
					{
						GroupingTypeData typeData = myGroupTypeData[row];
						if (0 != (typeData.SelectionState & GroupingTypeSelectionState.CurrentlySelected))
						{
							typeData.SelectionState &= ~GroupingTypeSelectionState.CurrentlySelected;
						}
						else
						{
							typeData.SelectionState |= GroupingTypeSelectionState.CurrentlySelected;
						}
						myGroupTypeData[row] = typeData;
						return StateRefreshChanges.Current;
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							GroupingTypeData[] types = myGroupTypeData;
							return (types == null) ? 0 : types.Length;
						}
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
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.StateChanges;
						}
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
						add {  }
						remove {  }
					}
					void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
					{
					}
					void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
					{
					}
					void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
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
					#endregion // Unused methods
					#endregion // IBranch Implementation
				}
				#endregion // GroupingTypesBranch class
			}
			#endregion // GroupingTypesEditor class
		}
		#endregion // GroupingTypesPropertyDescriptor class
	}
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ElementGroupingType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ElementGroupingTypeTypeDescriptor<TModelElement> : ElementTypeDescriptor<TModelElement>
		where TModelElement : ElementGroupingType
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ElementGroupingTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ElementGroupingTypeTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Do not show embedded object properties
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}
		/// <summary>
		/// Do not show relationship properties
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return false;
		}
		/// <summary>
		/// Show the class name as the component name
		/// </summary>
		public override string GetComponentName()
		{
			return this.ModelElement.GetDomainClass().DisplayName;
		}
		/// <summary>
		/// Show the base class name as the class name
		/// </summary>
		public override string GetClassName()
		{
			// Note that we could look this up through the object model as well,
			// but the base type is not changing and it is just as easy to grab
			// the resource name directly.
			return ResourceStrings.ElementGroupingTypeDisplayName;
		}
		#endregion // Base overrides
	}
	/// <summary>
	/// Redirect <see cref="GroupingElementRelationship"/> properties to the
	/// target element. This is part of enabling a link element to appear
	/// mostly like a normal node, but let commands distinguish between
	/// the link element and the target element.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class GroupingElementRelationshipTypeDescriptionProvider : TypeDescriptionProvider
	{
		#region Base overrides
		/// <summary>
		/// Redirect the type descriptor to the <see cref="P:GroupingElementRelationship.Element"/>
		/// of the <see cref="GroupingElementRelationship"/> instance.
		/// </summary>
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			GroupingElementRelationship typedInstance;
			ModelElement element;
			if (null != (typedInstance = instance as GroupingElementRelationship) &&
				!(element = typedInstance.Element).IsDeleted)
			{
				return TypeDescriptor.GetProvider(element).GetTypeDescriptor(element.GetType(), element);
			}
			return base.GetTypeDescriptor(objectType, instance);
		}
		#endregion // Base overrides
	}
}
