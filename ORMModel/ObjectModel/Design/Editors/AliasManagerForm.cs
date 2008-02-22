using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling.Design;
using System.Collections;
using System.Drawing.Design;
using System.Globalization;
using Neumont.Tools.Modeling.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// Form for displaying alias elements in a <see cref="NameGenerator"/> context
	/// </summary>
	public partial class AliasManagerForm : Form
	{
		private static AliasManagerForm mySingleton;
		/// <summary>
		/// Display a dialog detailing name generations settings for a specific generator in the current store
		/// </summary>
		/// <param name="nameGenerator">The context <see cref="NameGenerator"/> element</param>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to display the dialog</param>
		public static void Show(NameGenerator nameGenerator, IServiceProvider serviceProvider)
		{
			IUIService uiService;
			if (null != serviceProvider &&
				null != (uiService = (IUIService)serviceProvider.GetService(typeof(IUIService))))
			{
				AliasManagerForm viewer = mySingleton;
				if (viewer == null)
				{
					viewer = new AliasManagerForm();
					viewer.virtualTreeControl.SetColumnHeaders(
						new VirtualTreeColumnHeader[]
						{
							new VirtualTreeColumnHeader(ResourceStrings.NameGeneratorAbbreviationsEditorElementColumnHeader),
							new VirtualTreeColumnHeader(ResourceStrings.NameGeneratorAbbreviationsEditorAbbreviationColumnHeader)
						},
						true);
					mySingleton = viewer;
				}
				VirtualTreeControl treeControl = viewer.virtualTreeControl;
				ITree tree = treeControl.Tree;
				AliasOwnerBranch rootBranch = new AliasOwnerBranch(nameGenerator, treeControl);
				tree.Root = rootBranch;
				if (tree.VisibleItemCount == 1)
				{
					tree.ToggleExpansion(0, 0);
				}
				DomainClassInfo usageClass = nameGenerator.NameUsageDomainClass;
				viewer.Text = usageClass != null ?
					string.Format(CultureInfo.InvariantCulture, ResourceStrings.NameGeneratorAbbreviationsEditorTitleFormatStringWithNameUsage, nameGenerator.GetDomainClass().DisplayName, usageClass.DisplayName) :
					string.Format(CultureInfo.InvariantCulture, ResourceStrings.NameGeneratorAbbreviationsEditorTitleFormatString, nameGenerator.GetDomainClass().DisplayName);
				bool processResults = uiService.ShowDialog(viewer) == DialogResult.OK;
				tree.Root = null;
				if (processResults)
				{
					using (Transaction t = nameGenerator.Store.TransactionManager.BeginTransaction(ResourceStrings.NameGeneratorAbbreviationsEditorTransactionName))
					{
						rootBranch.CommitChanges();
						if (t.HasPendingChanges)
						{
							t.Commit();
						}
					}
				}
			}
		}
		/// <summary>
		/// Create a new AliasManagerForm
		/// </summary>
		private AliasManagerForm()
		{
			InitializeComponent();
			virtualTreeControl.MultiColumnTree = new MultiColumnTree(2);
		}

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
		#endregion // BaseBranch Class
		 
		private sealed class AliasOwnerBranch : BaseBranch, IBranch
		{
			#region Member variables and constructor
			private struct DetailBranchInfo
			{
				/// <summary>
				/// Create new detail branch information for this
				/// owner role. The branch is delay created.
				/// </summary>
				public DetailBranchInfo(DomainRoleInfo roleInfo, ElementInstanceBranch branch)
				{
					DomainRole = roleInfo;
					Branch = branch;
				}
				public DomainRoleInfo DomainRole;
				public ElementInstanceBranch Branch;
			}
			private DetailBranchInfo[] myDetails;
			private NameGenerator myNameGenerator;
			private VirtualTreeControl myHostControl;
			public AliasOwnerBranch(NameGenerator nameGenerator, VirtualTreeControl hostControl)
			{
				Store store = nameGenerator.Store;
				DomainDataDirectory dataDir = store.DomainDataDirectory;
				ReadOnlyCollection<DomainClassInfo> relationships = dataDir.GetDomainRelationship(ElementHasAlias.DomainClassId).AllDescendants;
				int relCount = relationships.Count;
				DetailBranchInfo[] details = new DetailBranchInfo[relCount];
				for (int i = 0; i < relCount; ++i)
				{
					DomainRelationshipInfo relInfo = (DomainRelationshipInfo)relationships[i];
					ReadOnlyCollection<DomainRoleInfo> roles = relInfo.DomainRoles;
					DomainRoleInfo roleInfo = roles[0];
					if (!roleInfo.IsSource)
					{
						roleInfo = roles[1];
						Debug.Assert(roleInfo.IsSource);
					}
					details[i] = new DetailBranchInfo(roleInfo, null);
				}
				myHostControl = hostControl;
				myDetails = details;
				myNameGenerator = nameGenerator;
			}
			#endregion // Member variables and constructor
			#region Commit changes to the store
			/// <summary>
			/// Commit changes to an open transaction
			/// </summary>
			public void CommitChanges()
			{
				DetailBranchInfo[] details = myDetails;
				NameGenerator generator = myNameGenerator;
				for (int i = 0; i < details.Length; ++i)
				{
					ElementInstanceBranch instanceBranch = details[i].Branch;
					if (instanceBranch != null)
					{
						instanceBranch.CommitChanges(generator);
					}
				}
			}
			#endregion // Commit changes to the store
			#region IBranch Implementation
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.Expansions;
				}
			}

			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					DomainRoleInfo roleInfo = myDetails[row].DomainRole;
					ElementInstanceBranch branch = new ElementInstanceBranch(roleInfo, this);
					myDetails[row] = new DetailBranchInfo(roleInfo, branch);
					return branch;
				}
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				return myDetails[row].DomainRole.DomainRelationship.DisplayName;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return true;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				retVal.BackColor = SystemColors.ControlLight;
				return retVal;
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return myDetails.Length;
				}
			}
			#endregion // IBranch Implementation
			#region InstanceBranch class
			private sealed class ElementInstanceBranch : BaseBranch, IBranch, IMultiColumnBranch
			{
				#region Helper structures
				[Flags]
				private enum ItemFlags
				{
					/// <summary>
					/// The direct alias already exists in the store, modify the existing one
					/// </summary>
					ExistingAlias = 1,
					/// <summary>
					/// The alias associated with the main generator exists
					/// </summary>
					DirectAlias = 2,
					/// <summary>
					/// An alias associated with a base generator exists
					/// </summary>
					ExistingOnBase = 4,
					/// <summary>
					/// The item has been modified in the dialog
					/// </summary>
					ModifiedEntry = 8,
					/// <summary>
					/// The item should be treated as deleted
					/// </summary>
					DeletedEntry = 0x10,
				}
				private sealed class ItemInfo
				{
					/// <summary>
					/// Compare items
					/// </summary>
					public static IComparer<ItemInfo> Comparer = new ItemInfoComparer();
					private ItemFlags myFlags;
					private ModelElement myOwner;
					private NameAlias myAlias;
					private NameAlias myBaseAlias;
					private string myAliasName;
					private string myOwnerName;
					/// <summary>
					/// Create a new ItemInfo for an existing alias
					/// </summary>
					public ItemInfo(NameAlias alias, NameAlias baseAlias, ModelElement owner, DomainPropertyInfo ownerNameProperty)
					{
						myAlias = alias;
						myBaseAlias = baseAlias;
						ItemFlags flags = 0;
						if (alias != null)
						{
							flags |= ItemFlags.ExistingAlias | ItemFlags.DirectAlias;
							myAliasName = alias.Name;
							if (baseAlias != null)
							{
								flags |= ItemFlags.ExistingOnBase;
							}
						}
						else if (baseAlias == null)
						{
							// This is a new item
							myAliasName = "";
							flags |= ItemFlags.ModifiedEntry | ItemFlags.DirectAlias;
						}
						else
						{
							myAliasName = baseAlias.Name;
							flags |= ItemFlags.ExistingOnBase;
						}
						myFlags = flags;
						myOwner = owner;
						myOwnerName = (ownerNameProperty != null) ? (string)ownerNameProperty.GetValue(owner) : owner.ToString();
					}
					private ItemInfo()
					{
					}
					/// <summary>
					/// Create a dummy <see cref="ItemInfo"/> appropriate for use with the <see cref="Comparer"/>.
					/// The item must be passed to <see cref="UpdateForCompare"/> before it can be used.
					/// </summary>
					public static ItemInfo CreateForCompare()
					{
						return new ItemInfo();
					}
					/// <summary>
					/// Update the item created with <see cref="CreateForCompare"/>
					/// </summary>
					public static void UpdateForCompare(ItemInfo item, ModelElement owner, DomainPropertyInfo ownerNameProperty)
					{
						item.myOwner = owner;
						item.myOwnerName = (ownerNameProperty != null) ? (string)ownerNameProperty.GetValue(owner) : owner.ToString();
					}
					/// <summary>
					/// The current alias is on a base generator
					/// </summary>
					public bool IsBase
					{
						get
						{
							return 0 == (myFlags & ItemFlags.DirectAlias);
						}
					}
					/// <summary>
					/// The item alias is marked for deletion
					/// </summary>
					public bool IsDeleted
					{
						get
						{
							return 0 != (myFlags & ItemFlags.DeletedEntry);
						}
					}
					/// <summary>
					/// The current state is modified
					/// </summary>
					public bool IsModified
					{
						get
						{
							return 0 != (myFlags & ItemFlags.ModifiedEntry);
						}
					}
					/// <summary>
					/// An existing alias instance directly associated with the generator being edited
					/// </summary>
					public NameAlias Alias
					{
						get
						{
							return myAlias;
						}
					}
					/// <summary>
					/// An existing alias associated with the base of the generator being edited
					/// </summary>
					public NameAlias BaseAlias
					{
						get
						{
							return myBaseAlias;
						}
					}
					/// <summary>
					/// The owning element for this alias entry
					/// </summary>
					public ModelElement Owner
					{
						get
						{
							return myOwner;
						}
					}
					/// <summary>
					/// Get the current alias name
					/// </summary>
					public string AliasName
					{
						get
						{
							return myAliasName;
						}
						set
						{
							ItemFlags flags = myFlags;
							if (0 != (flags & ItemFlags.DirectAlias) && 0 == (flags & (ItemFlags.ExistingAlias | ItemFlags.ExistingOnBase)))
							{
								// New entry
								myAliasName = value;
							}
							else if ((ItemFlags.ExistingAlias | ItemFlags.ExistingOnBase) == (flags & (ItemFlags.ExistingAlias | ItemFlags.ExistingOnBase)))
							{
								// An existing alias and a base alias
								if (string.IsNullOrEmpty(value) || value == myBaseAlias.Name)
								{
									myAliasName = myBaseAlias.Name;
									flags |= ItemFlags.DeletedEntry | ItemFlags.ModifiedEntry;
									flags &= ~ItemFlags.DirectAlias;
								}
								else if (value == myAlias.Name)
								{
									myAliasName = value;
									flags &= ~(ItemFlags.DeletedEntry | ItemFlags.ModifiedEntry);
									flags |= ItemFlags.DirectAlias;
								}
								else
								{
									myAliasName = value;
									flags &= ~ItemFlags.DeletedEntry;
									flags |= ItemFlags.ModifiedEntry | ItemFlags.DirectAlias;
								}
							}
							else if (0 != (flags & ItemFlags.ExistingAlias))
							{
								// An existing item only
								myAliasName = value;
								if (value == myAlias.Name)
								{
									flags &= ~(ItemFlags.DeletedEntry | ItemFlags.ModifiedEntry);
								}
								else if (string.IsNullOrEmpty(value))
								{
									flags |= ItemFlags.DeletedEntry | ItemFlags.ModifiedEntry;
								}
								else
								{
									flags &= ~ItemFlags.DeletedEntry;
									flags |= ItemFlags.ModifiedEntry;
								}
							}
							else if (string.IsNullOrEmpty(value))
							{
								// A base alias only
								myAliasName = myBaseAlias.Name;
								flags &= ~(ItemFlags.ModifiedEntry | ItemFlags.DirectAlias);
							}
							else
							{
								// A base alias only
								myAliasName = value;
								flags |= ItemFlags.ModifiedEntry | ItemFlags.DirectAlias;
							}
							myFlags = flags;
						}
					}
					/// <summary>
					/// Get the cached owner name
					/// </summary>
					public string OwnerName
					{
						get
						{
							return myOwnerName;
						}
					}

					#region IComparable<ItemInfo> Implementation
					private class ItemInfoComparer : IComparer<ItemInfo>
					{
						#region IComparer<ItemInfo> Implementation
						int IComparer<ItemInfo>.Compare(ItemInfo x, ItemInfo y)
						{
							if (x.myOwner == y.myOwner)
							{
								return 0;
							}
							int retVal = x.myOwnerName.CompareTo(y.myOwnerName);
							return retVal == 0 ? x.myOwner.Id.CompareTo(y.myOwner.Id) : retVal;
						}
						#endregion // IComparer<ItemInfo> Implementation
					}
					#endregion // IComparable<ItemInfo> Implementation
				}
				/// <summary>
				/// Helper structure for branch construction
				/// </summary>
				private struct GeneratorClassAndAlias
				{
					public GeneratorClassAndAlias(DomainClassInfo generatorClass, NameAlias alias, DomainClassInfo baseGeneratorClass, NameAlias baseAlias)
					{
						GeneratorClass = generatorClass;
						Alias = alias;
						BaseGeneratorClass = baseGeneratorClass;
						BaseAlias = baseAlias;
					}
					public DomainClassInfo GeneratorClass;
					public NameAlias Alias;
					public DomainClassInfo BaseGeneratorClass;
					public NameAlias BaseAlias;
				}
				#endregion // Helper structures
				#region OtherOwnersPropertyDescriptor class
				/// <summary>
				/// A property descriptor to host inside the TypeEditorHost.
				/// Handles all in-place editing.
				/// </summary>
				private sealed class OtherOwnersPropertyDescriptor : PropertyDescriptor
				{
					public static readonly PropertyDescriptor Descriptor = new OtherOwnersPropertyDescriptor();
					#region InstanceDropDown class
					/// <summary>
					/// An ElementPicker list used to display available instance values
					/// </summary>
					private sealed class InstanceDropDown : ElementPicker<InstanceDropDown>
					{
						/// <summary>
						/// Return the string values for the contents of the dropdown list
						/// </summary>
						protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
						{
							ElementInstanceBranch branchContext = (ElementInstanceBranch)context.Instance;
							DomainClassInfo ownerClassInfo = branchContext.myOwnerRoleInfo.RolePlayer;
							ReadOnlyCollection<ModelElement> elements = branchContext.myStore.ElementDirectory.FindElements(ownerClassInfo, true);
							int elementCount = elements.Count;
							if (elementCount != 0)
							{
								List<ItemInfo> existingItems = branchContext.myItems;
								int existingCount = existingItems != null ? existingItems.Count : 0;
								int instanceCount = elementCount - existingCount;
								if (instanceCount != 0)
								{
									ModelElement[] instances = new ModelElement[instanceCount];
									DomainPropertyInfo nameProperty = ownerClassInfo.NameDomainProperty;
									if (existingCount == 0)
									{
										elements.CopyTo(instances, 0);
									}
									else
									{
										ItemInfo findItem = ItemInfo.CreateForCompare();
										int nextItem = -1;
										for (int i = 0; i < elementCount; ++i)
										{
											ModelElement testItem = elements[i];
											ItemInfo.UpdateForCompare(findItem, testItem, nameProperty);
											if (0 > existingItems.BinarySearch(findItem, ItemInfo.Comparer))
											{
												instances[++nextItem] = testItem;
											}
										}
									}
									if (instanceCount > 1)
									{
										Array.Sort<ModelElement>(
											instances,
											nameProperty != null ?
												(Comparison<ModelElement>)delegate(ModelElement x, ModelElement y)
												{
													return string.Compare((string)nameProperty.GetValue(x), (string)nameProperty.GetValue(y), StringComparison.CurrentCultureIgnoreCase);
												} :
												delegate(ModelElement x, ModelElement y)
												{
													return string.Compare(x.ToString(), y.ToString(), StringComparison.CurrentCultureIgnoreCase);
												});
									}
									return instances;
								}
							}
							return null;
						}
						/// <summary>
						/// Override to ensure that no initial item is selected.
						/// </summary>
						protected override object TranslateToDisplayObject(object initialObject, IList contentList)
						{
							return null;
						}
					}
					#endregion // InstanceDropDown class
					#region Constructor
					private OtherOwnersPropertyDescriptor()
						: base(" ", null)
					{
					}
					#endregion // Constructor
					#region Base Overrides
					public sealed override object GetEditor(Type editorBaseType)
					{
						return editorBaseType == typeof(UITypeEditor) ? new InstanceDropDown() : base.GetEditor(editorBaseType);
					}
					public sealed override object GetValue(object component)
					{
						return ResourceStrings.NameGeneratorAbbreviationsEditorNewItemText;
					}
					public sealed override void SetValue(object component, object value)
					{
						ElementInstanceBranch contextBranch = (ElementInstanceBranch)component;
						ItemInfo newItem = new ItemInfo(null, null, (ModelElement)value, contextBranch.myOwnerRoleInfo.RolePlayer.NameDomainProperty);
						List<ItemInfo> items = contextBranch.myItems;
						int itemCount = items != null ? items.Count : 0;
						BranchModificationEventHandler notify = contextBranch.myModify;
						if (items == null)
						{
							items = new List<ItemInfo>();
							items.Add(newItem);
							contextBranch.myItems = items;
						}
						else
						{
							int insertIndex = ~items.BinarySearch(newItem, ItemInfo.Comparer);
							if (insertIndex == itemCount)
							{
								items.Add(newItem);
							}
							else
							{
								items.Insert(insertIndex, newItem);
								notify(contextBranch, BranchModificationEventArgs.MoveItem(contextBranch, itemCount, insertIndex));
							}
						}
						notify(contextBranch, BranchModificationEventArgs.InsertItems(contextBranch, itemCount, 1));
						VirtualTreeControl control = contextBranch.myParentBranch.myHostControl;
						control.CurrentColumn = 1;
						control.BeginLabelEdit();
					}
					public sealed override Type ComponentType
					{
						get
						{
							return typeof(ElementInstanceBranch);
						}
					}
					public sealed override Type PropertyType
					{
						get
						{
							return typeof(ModelElement);
						}
					}
					public sealed override bool IsReadOnly
					{
						get
						{
							return false;
						}
					}
					public sealed override bool CanResetValue(object component)
					{
						return false;
					}
					public sealed override void ResetValue(object component)
					{
					}
					public sealed override bool ShouldSerializeValue(object component)
					{
						return true;
					}
					#endregion // Base Overrides
				}
				#endregion // OtherOwnersPropertyDescriptor class
				#region Member Variables
				private List<ItemInfo> myItems;
				private DomainRoleInfo myOwnerRoleInfo;
				private Store myStore;
				private AliasOwnerBranch myParentBranch;
				#endregion // Member Variables
				#region Constructor
				public ElementInstanceBranch(DomainRoleInfo ownerRoleInfo, AliasOwnerBranch parentBranch)
				{
					NameGenerator generator = parentBranch.myNameGenerator;
					myParentBranch = parentBranch;
					myStore = generator.Store;
					myOwnerRoleInfo = ownerRoleInfo;
					//Find all the aliases that are associated with that name consumer or any of it's base types
					Store store = generator.Store;
					Type contextNameUsageType = generator.NameUsageType;
					DomainClassInfo contextGeneratorClass = generator.GetDomainClass();
					Dictionary<ModelElement, GeneratorClassAndAlias> elementMap = null;
					foreach (ElementHasAlias link in store.ElementDirectory.FindElements(ownerRoleInfo.DomainRelationship, false))
					{
						NameAlias alias = link.Alias;
						// Extra care needs to be taken when usage is set to determine the
						// nearest base generator. The nearest generator type is chosen first,
						// followed by the best usage match. An exact usage match is preferred,
						// followed by the null usage match.
						Type currentNameUsageType = alias.NameUsageType;
						if ((contextNameUsageType == null) ? (currentNameUsageType == null) : (currentNameUsageType == null || currentNameUsageType == contextNameUsageType))
						{
							DomainClassInfo currentGeneratorClass = alias.NameConsumerDomainClass;
							if (currentGeneratorClass.IsDerivedFrom(NameGenerator.DomainClassId))
							{
								bool directGenerator = currentGeneratorClass == contextGeneratorClass && contextNameUsageType == currentNameUsageType;
								if (directGenerator ||
									contextGeneratorClass.IsDerivedFrom(currentGeneratorClass))
								{
									ModelElement owner = alias.Element;
									GeneratorClassAndAlias existingGeneratorClass;
									if (elementMap == null)
									{
										elementMap = new Dictionary<ModelElement, GeneratorClassAndAlias>();
										elementMap[owner] = directGenerator ?
											new GeneratorClassAndAlias(currentGeneratorClass, alias, null, null) :
											new GeneratorClassAndAlias(null, null, currentGeneratorClass, alias);
									}
									else if (elementMap.TryGetValue(owner, out existingGeneratorClass))
									{
										if (directGenerator)
										{
											elementMap[owner] = new GeneratorClassAndAlias(currentGeneratorClass, alias, existingGeneratorClass.BaseGeneratorClass, existingGeneratorClass.BaseAlias);
										}
										else
										{
											DomainClassInfo iterateClass = existingGeneratorClass.BaseGeneratorClass;
											if (iterateClass != null)
											{
												// If the existing generator is nearer the NameConsumer base type
												// than the current generator, then the current one wins
												int existingDepth = 0;
												int newDepth = 0;
												if (iterateClass != currentGeneratorClass)
												{
													do
													{
														++existingDepth;
														iterateClass = iterateClass.BaseDomainClass;
													} while (iterateClass != null);
													iterateClass = currentGeneratorClass;
													do
													{
														++newDepth;
														iterateClass = iterateClass.BaseDomainClass;
													} while (iterateClass != null);
													if (newDepth > existingDepth ||
														(currentGeneratorClass == existingGeneratorClass.BaseGeneratorClass && currentNameUsageType != null))
													{
														elementMap[owner] = new GeneratorClassAndAlias(existingGeneratorClass.GeneratorClass, existingGeneratorClass.Alias, currentGeneratorClass, alias);
													}
												}
											}
											else
											{
												elementMap[owner] = new GeneratorClassAndAlias(existingGeneratorClass.GeneratorClass, existingGeneratorClass.Alias, currentGeneratorClass, alias);
											}
										}
									}
									else
									{
										elementMap[owner] = directGenerator ?
											new GeneratorClassAndAlias(currentGeneratorClass, alias, null, null) :
											new GeneratorClassAndAlias(null, null, currentGeneratorClass, alias);
									}
								}
							}
						}
					}
					if (elementMap != null)
					{
						List<ItemInfo> items = new List<ItemInfo>();
						DomainPropertyInfo nameProperty = ownerRoleInfo.RolePlayer.NameDomainProperty;
						DomainClassInfo localGeneratorClass = generator.GetDomainClass();
						foreach (KeyValuePair<ModelElement, GeneratorClassAndAlias> pair in elementMap)
						{
							GeneratorClassAndAlias value = pair.Value;
							items.Add(new ItemInfo(value.Alias, value.BaseAlias, pair.Key, nameProperty));
						}
						items.Sort(ItemInfo.Comparer);
						myItems = items;
					}
				}
				#endregion // Constructor
				#region CommitChanges
				/// <summary>
				/// Commit changes to an open transaction
				/// </summary>
				/// <param name="generator">The context generator</param>
				public void CommitChanges(NameGenerator generator)
				{
					List<ItemInfo> items = myItems;
					if (items != null)
					{
						int itemCount = items.Count;
						PropertyAssignment consumerProperty = null;
						PropertyAssignment usageProperty = null;
						Store store = generator.Store;
						for (int i = 0; i < itemCount; ++i)
						{
							ItemInfo currentItem = items[i];
							if (currentItem.IsModified)
							{
								if (currentItem.IsDeleted)
								{
									currentItem.Alias.Delete();
								}
								else
								{
									string aliasName = currentItem.AliasName;
									if (!string.IsNullOrEmpty(aliasName))
									{
										NameAlias alias = currentItem.Alias;
										if (alias == null)
										{
											if (consumerProperty == null)
											{
												consumerProperty = new PropertyAssignment(NameAlias.NameConsumerDomainPropertyId, NameConsumer.TranslateToConsumerIdentifier(generator.GetDomainClass()));
												string nameUsage = NameUsage.TranslateToNameUsageIdentifier(generator.NameUsageDomainClass);
												if (!string.IsNullOrEmpty(nameUsage))
												{
													usageProperty = new PropertyAssignment(NameAlias.NameUsageDomainPropertyId, nameUsage);
												}
											}
											PropertyAssignment nameAssignment = new PropertyAssignment(NameAlias.NameDomainPropertyId, aliasName);
											alias = (usageProperty != null) ?
												new NameAlias(store, nameAssignment, consumerProperty, usageProperty) :
												new NameAlias(store, nameAssignment, consumerProperty);
											DomainRoleInfo roleInfo = myOwnerRoleInfo;
											store.GetDomainModel(roleInfo.DomainModel.Id).CreateElementLink(
												store.DefaultPartition,
												myOwnerRoleInfo.DomainRelationship.ImplementationClass,
												new RoleAssignment[]{
													new RoleAssignment(roleInfo.Id, currentItem.Owner),
													new RoleAssignment(roleInfo.OppositeDomainRole.Id, alias)},
												null);
										}
										else
										{
											alias.Name = aliasName;
										}
									}
								}
							}
						}
					}
				}
				#endregion // CommitChanges
				#region IBranch Implementation
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.JaggedColumns | BranchFeatures.ExplicitLabelEdits | BranchFeatures.DelayedLabelEdits | BranchFeatures.ImmediateSelectionLabelEdits | BranchFeatures.InsertsAndDeletes;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					if (row == ExistingItemCount)
					{
						return ResourceStrings.NameGeneratorAbbreviationsEditorNewItemText;
					}
					return (column == 0) ? myItems[row].OwnerName : myItems[row].AliasName;
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					if (row != ExistingItemCount)
					{
						ItemInfo currentItem = myItems[row];
						if ((column == 1 && currentItem.IsBase) ||
							(column == 0 && currentItem.IsDeleted && !currentItem.IsBase))
						{
							retVal.GrayText = true;
						}
					}
					return retVal;
				}
				VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					if (row == ExistingItemCount)
					{
						TypeEditorHost host = OnScreenTypeEditorHost.Create(
							OtherOwnersPropertyDescriptor.Descriptor,
							this,
							TypeEditorHostEditControlStyle.TransparentEditRegion);
						if (host != null)
						{
							(host as IVirtualTreeInPlaceControl).Flags = VirtualTreeInPlaceControlFlags.DisposeControl | VirtualTreeInPlaceControlFlags.SizeToText | VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.ForwardKeyEvents;
							VirtualTreeLabelEditData retVal = VirtualTreeLabelEditData.Default;
							retVal.CustomInPlaceEdit = host;
							return retVal;
						}
					}
					else if (column == 1)
					{
						if (0 != (activationStyle & (VirtualTreeLabelEditActivationStyles.ImmediateSelection | VirtualTreeLabelEditActivationStyles.ImmediateMouse)))
						{
							return VirtualTreeLabelEditData.DeferActivation;
						}
						VirtualTreeLabelEditData retVal = VirtualTreeLabelEditData.Default;
						IVirtualTreeInPlaceControl editControl = new VirtualTreeInPlaceEditControl();
						editControl.Flags &= ~VirtualTreeInPlaceControlFlags.SizeToText;
						retVal.CustomInPlaceEdit = editControl;
						if (myItems[row].IsBase)
						{
							retVal.AlternateText = "";
						}
						return retVal;
					}
					return VirtualTreeLabelEditData.Invalid;
				}
				LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
				{
					if (column == 1)
					{
						myItems[row].AliasName = newText;
						return LabelEditResult.AcceptEdit;
					}
					return LabelEditResult.CancelEdit;
				}
				private BranchModificationEventHandler myModify;
				event BranchModificationEventHandler IBranch.OnBranchModification
				{
					add { myModify += value; }
					remove { myModify -= value; }
				}
				private int ExistingItemCount
				{
					get
					{
						List<ItemInfo> items = myItems;
						return (items == null) ? 0 : items.Count;
					}
				}
				int IBranch.VisibleItemCount
				{
					get
					{
						return ExistingItemCount + 1;
					}
				}
				#endregion // IBranch Implementation
				#region IMultiColumnBranch Implementation
				int IMultiColumnBranch.ColumnCount
				{
					get
					{
						return 2;
					}
				}
				SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
				{
					return SubItemCellStyles.Simple;
				}
				int IMultiColumnBranch.GetJaggedColumnCount(int row)
				{
					return (row == ExistingItemCount) ? 1 : 2;
				}
				#endregion // IMultiColumnBranch Implementation
			}
			#endregion // InstanceBranch class
		}

		private void virtualTreeControl_LabelEditControlChanged(object sender, EventArgs e)
		{
			if (virtualTreeControl.LabelEditControl == null)
			{
				AcceptButton = btnOK;
				CancelButton = btnCancel;
			}
			else
			{
				AcceptButton = null;
				CancelButton = null;
			}
		}

		private void virtualTreeControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.F2 && virtualTreeControl.LabelEditControl == null)
			{
				virtualTreeControl.BeginLabelEdit();
			}
		}
	}
}