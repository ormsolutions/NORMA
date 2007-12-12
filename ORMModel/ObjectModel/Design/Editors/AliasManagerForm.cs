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
					AliasOwnerBranch.InitializeHeaders(viewer.virtualTreeControl);
					mySingleton = viewer;
				}
				viewer.virtualTreeControl.Tree.Root = new AliasOwnerBranch(nameGenerator);
				uiService.ShowDialog(viewer);
				viewer.virtualTreeControl.Tree.Root = null;
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
			public AliasOwnerBranch(NameGenerator nameGenerator)
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
				myDetails = details;
				myNameGenerator = nameGenerator;
			}
			#endregion // Member variables and constructor
			#region Header Initialization
			public static void InitializeHeaders(VirtualTreeControl control)
			{
				control.SetColumnHeaders(new VirtualTreeColumnHeader[]
				{
					new VirtualTreeColumnHeader("Object"),
					new VirtualTreeColumnHeader("Alias")
					}, true);
			}
			#endregion // Header Initialization
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
					ElementInstanceBranch branch = new ElementInstanceBranch(roleInfo, myNameGenerator);
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
			//event BranchModificationEventHandler IBranch.OnBranchModification
			//{
			//    add { throw new Exception("The method or operation is not implemented."); }
			//    remove { throw new Exception("The method or operation is not implemented."); }
			//}
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
					/// The alias exists in the store
					/// </summary>
					ExistingAlias = 1,
					/// <summary>
					/// The alias is on base alias owner, display differently
					/// </summary>
					ExistingOnBase = 2,
					/// <summary>
					/// The item has been modified in the dialog
					/// </summary>
					ModifiedEntry = 4,
				}
				private struct ItemInfo
				{
					/// <summary>
					/// Create a new ItemInfo for an existing alias
					/// </summary>
					public ItemInfo(NameAlias alias, ModelElement owner, string ownerName, bool aliasOnBase)
					{
						AliasInstance = alias;
						OwnerInstance = owner;
						Flags = ItemFlags.ExistingAlias | (aliasOnBase ? ItemFlags.ExistingOnBase : 0);
						AliasName = alias.Name;
						OwnerName = ownerName;
					}
					public ItemFlags Flags;
					public ModelElement OwnerInstance;
					public NameAlias AliasInstance;
					public string AliasName;
					public string OwnerName;
				}
				/// <summary>
				/// Helper structure for branch construction
				/// </summary>
				private struct GeneratorClassAndAlias
				{
					public GeneratorClassAndAlias(DomainClassInfo generatorClass, NameAlias alias)
					{
						GeneratorClass = generatorClass;
						Alias = alias;
					}
					public DomainClassInfo GeneratorClass;
					public NameAlias Alias;
				}
				#endregion // Helper structures
				#region Member Variables
				private List<ItemInfo> myItems;
				#endregion // Member Variables
				#region Constructor
				public ElementInstanceBranch(DomainRoleInfo ownerRoleInfo, NameGenerator generator)
				{
					//Find all the aliases that are associated with that name consumer or any of it's base types
					Store store = generator.Store;
					Type filterNameUsageType = generator.NameUsageType;
					DomainClassInfo filterGeneratorClass = generator.GetDomainClass();
					Dictionary<ModelElement, GeneratorClassAndAlias> elementMap = null;
					foreach (ElementHasAlias link in store.ElementDirectory.FindElements(ownerRoleInfo.DomainRelationship, false))
					{
						NameAlias alias = link.Alias;
						if (alias.NameUsageType == filterNameUsageType)
						{
							DomainClassInfo currentGeneratorClass = alias.NameConsumerDomainClass;
							if (currentGeneratorClass.IsDerivedFrom(NameGenerator.DomainClassId))
							{
								if (currentGeneratorClass == filterGeneratorClass ||
									filterGeneratorClass.IsDerivedFrom(currentGeneratorClass))
								{
									ModelElement owner = alias.Element;
									GeneratorClassAndAlias existingGeneratorClass;
									if (elementMap == null)
									{
										elementMap = new Dictionary<ModelElement, GeneratorClassAndAlias>();
										elementMap[owner] = new GeneratorClassAndAlias(currentGeneratorClass, alias);
									}
									else if (elementMap.TryGetValue(owner, out existingGeneratorClass))
									{
										// If the existing generator is nearer the NameConsumer base type
										// than the current generator, then the current one wins
										int existingDepth = 0;
										int newDepth = 0;
										DomainClassInfo iterateClass = existingGeneratorClass.GeneratorClass;
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
											if (newDepth > existingDepth)
											{
												elementMap[owner] = new GeneratorClassAndAlias(currentGeneratorClass, alias);
											}
										}
									}
									else
									{
										elementMap[owner] = new GeneratorClassAndAlias(currentGeneratorClass, alias);
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
							ModelElement element = pair.Key;
							GeneratorClassAndAlias value = pair.Value;
							items.Add(new ItemInfo(value.Alias, pair.Key, (nameProperty != null) ? (string)nameProperty.GetValue(element) : element.ToString(), value.GeneratorClass != localGeneratorClass));
						}
						myItems = items;
					}
				}
				#endregion // Constructor
				#region IBranch Implementation
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.JaggedColumns;
					}
				}
				string IBranch.GetText(int row, int column)
				{
					if (row == ExistingItemCount)
					{
						return "PICK NEW ITEM";
					}
					return (column == 0) ? myItems[row].OwnerName : myItems[row].AliasName;
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
	}
}