using System;
using System.Diagnostics;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
using System.Resources;
using System.Windows.Forms;

namespace Northface.Tools.ORM
{
	/// <summary>
	/// A constant list of strings corresponding to resource identifiers
	/// in the resource files for all models. Any resource id referenced
	/// directly in non-spit code should be duplicated here.
	/// </summary>
	internal static class ResourceStrings
	{
		#region Supported Resource Managers
		/// <summary>
		/// Recognized resource managers
		/// </summary>
		private enum ResourceManagers
		{
			/// <summary>
			/// IMS-managed resource file for the core object model
			/// </summary>
			ObjectModel,
			/// <summary>
			/// IMS-managed resource file for the shape object model
			/// </summary>
			ShapeModel,
			/// <summary>
			/// Standalone resource file for the core model
			/// </summary>
			Model,
			/// <summary>
			/// Standalone resource file for the diagram
			/// </summary>
			Diagram,
		}
		#endregion // Supported Resource Managers
		#region Non-IMS ResourceManagers
		private static object myLockObject;
		private static object LockObject
		{
			get
			{
				if (myLockObject == null)
				{
					System.Threading.Interlocked.CompareExchange(ref myLockObject, new object(), null);
				}
				return myLockObject;
			}
		}
		private static ResourceManager myDiagramResourceManager;
		private static ResourceManager DiagramResourceManager
		{
			get
			{
				if (myDiagramResourceManager == null)
				{
					lock (LockObject)
					{
						if (myDiagramResourceManager == null)
						{
							myDiagramResourceManager = LoadResourceManagerForType(typeof(ORMDiagram));
						}
					}
				}
				return myDiagramResourceManager;
			}
		}
		private static ResourceManager LoadResourceManagerForType(Type type)
		{
			return new ResourceManager(type.FullName, type.Assembly);
		}

		private static ResourceManager myModelResourceManager;
		private static ResourceManager ModelResourceManager
		{
			get
			{
				if (myModelResourceManager == null)
				{
					lock (LockObject)
					{
						if (myModelResourceManager == null)
						{
							myModelResourceManager = LoadResourceManagerForType(typeof(ORMModel));
						}
					}
				}
				return myModelResourceManager;
			}
		}
		#endregion // Non-IMS ResourceManagers
		#region Helper functions
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			ResourceManager resMgr = null;
			switch (manager)
			{
				case ResourceManagers.ObjectModel:
					resMgr = ORMMetaModel.SingletonResourceManager;
					break;
				case ResourceManagers.ShapeModel:
					resMgr = ORMShapeModel.SingletonResourceManager;
					break;
				case ResourceManagers.Diagram:
					resMgr = DiagramResourceManager;
					break;
				case ResourceManagers.Model:
					resMgr = ModelResourceManager;
					break;
			}
			return resMgr;
		}

		private static string GetString(ResourceManagers manager, string resourceName)
		{
			ResourceManager resMgr = null;
			string retVal = null;
			resMgr = GetResourceManager(manager);
			if (resMgr != null)
			{
				retVal = resMgr.GetString(resourceName);
			}
			Debug.Assert(retVal != null && retVal.Length > 0, "Unrecognized resource string: " + resourceName);
			return (retVal != null) ? retVal : String.Empty;
		}

		private static object GetObject(ResourceManagers manager, string resourceName)
		{
			ResourceManager resMgr = null;
			object retVal = null;
			resMgr = GetResourceManager(manager);
			if (resMgr != null)
			{
				retVal = resMgr.GetObject(resourceName);
			}
			Debug.Assert(retVal != null, "Unrecognized resource string: " + resourceName);
			return retVal;
		}
		#endregion // Helper functions
		#region Public resource ids
		/// <summary>
		/// The identifier for the EntityType toolbox item
		/// </summary>
		public const string ToolboxEntityTypeItemId = "Toolbox.EntityType.Item.Id";
		/// <summary>
		/// The identifier for the ValueType toolbox item
		/// </summary>
		public const string ToolboxValueTypeItemId = "Toolbox.ValueType.Item.Id";
		/// <summary>
		/// The identifier for the UnaryFactType toolbox item
		/// </summary>
		public const string ToolboxUnaryFactTypeItemId = "Toolbox.UnaryFactType.Item.Id";
		/// <summary>
		/// The identifier for the BinaryFactType toolbox item
		/// </summary>
		public const string ToolboxBinaryFactTypeItemId = "Toolbox.BinaryFactType.Item.Id";
		/// <summary>
		/// The identifier for the TernaryFactType toolbox item
		/// </summary>
		public const string ToolboxTernaryFactTypeItemId = "Toolbox.TernaryFactType.Item.Id";
		/// <summary>
		/// The identifier for an ExternalUniquenessConstraint toolbox item
		/// </summary>
		public const string ToolboxExternalUniquenessConstraintItemId = "Toolbox.ExternalUniquenessConstraint.Item.Id";
		/// <summary>
		/// The identifier for an InternalUniquenessConstraint toolbox item
		/// </summary>
		public const string ToolboxInternalUniquenessConstraintItemId = "Toolbox.InternalUniquenessConstraint.Item.Id";
		/// <summary>
		/// The identifier for an ExclusionConstraint toolbox item
		/// </summary>
		public const string ToolboxExclusionConstraintItemId = "Toolbox.ExclusionConstraint.Item.Id";
		/// <summary>
		/// The identifier for an InclusiveOrConstraint toolbox item
		/// </summary>
		public const string ToolboxInclusiveOrConstraintItemId = "Toolbox.InclusiveOrConstraint.Item.Id";
		/// <summary>
		/// The identifier for an ExclusiveOrConstraint toolbox item
		/// </summary>
		public const string ToolboxExclusiveOrConstraintItemId = "Toolbox.ExclusiveOrConstraint.Item.Id";
		/// <summary>
		/// The identifier for the RoleConnector toolbox item
		/// </summary>
		public const string ToolboxRoleConnectorItemId = "Toolbox.RoleConnector.Item.Id";
		/// <summary>
		/// The identifier for an SubsetConstraint toolbox item
		/// </summary>
		public const string ToolboxSubsetConstraintItemId = "Toolbox.SubsetConstraint.Item.Id";
		/// <summary>
		/// The identifier for an EqualityConstraint toolbox item
		/// </summary>
		public const string ToolboxEqualityConstraintItemId = "Toolbox.EqualityConstraint.Item.Id";
		/// <summary>
		/// The identifier for an ExternalConstraintConnector toolbox item
		/// </summary>
		public const string ToolboxExternalConstraintConnectorItemId = "Toolbox.ExternalConstraintConnector.Item.Id";
		/// <summary>
		/// The identifier for an InternalUniquenessConstraintConnector toolbox item
		/// </summary>
		public const string ToolboxInternalUniquenessConstraintConnectorItemId = "Toolbox.InternalUniquenessConstraintConnector.Item.Id";
		/// <summary>
		/// Category name for options page
		/// </summary>
		public const string OptionsPageCategoryAppearanceId = "OptionsPage.Category.Appearance";
		/// <summary>
		/// Description of the object type shape
		/// </summary>
		public const string OptionsPagePropertyObjectTypeShapeDescriptionId = "OptionsPage.Property.ObjectTypeShape.Description";
		/// <summary>
		/// Display name of the object type shape
		/// </summary>
		public const string OptionsPagePropertyObjectTypeShapeDisplayNameId = "OptionsPage.Property.ObjectTypeShape.DisplayName";
		/// <summary>
		/// Description of the objectified fact shape
		/// </summary>
		public const string OptionsPagePropertyObjectifiedShapeDescriptionId = "OptionsPage.Property.ObjectifiedShape.Description";
		/// <summary>
		/// Display name of the objectified fact shape
		/// </summary>
		public const string OptionsPagePropertyObjectifiedShapeDisplayNameId = "OptionsPage.Property.ObjectifiedShape.DisplayName";
		/// <summary>
		/// Description of the Mandatory Dot placement
		/// </summary>
		public const string OptionsPagePropertyMandatoryDotDescriptionId = "OptionsPage.Property.MandatoryDot.Description";
		/// <summary>
		/// Display name of the Mandatory Dot placement
		/// </summary>
		public const string OptionsPagePropertyMandatoryDotDisplayNameId = "OptionsPage.Property.MandatoryDot.DisplayName";
		/// <summary>
		/// Description of the Role Name Display option
		/// </summary>
		public const string OptionsPagePropertyRoleNameDisplayDescriptionId = "OptionsPage.Property.RoleNameDisplay.Description";
		/// <summary>
		/// Display Name of the Role Name Display option
		/// </summary>
		public const string OptionsPagePropertyRoleNameDisplayDisplayNameId = "OptionsPage.Property.RoleNameDisplay.DisplayName";
		/// <summary>
		/// Display Name of the Object Name color
		/// </summary>
		public const string FactEditorColorsObjectNameId = "FactEditorColors.ObjectName";
		/// <summary>
		/// Display Name of the Reference Mode Name color
		/// </summary>
		public const string FactEditorColorsReferenceModeNameId = "FactEditorColors.ReferenceModeName";
		/// <summary>
		/// Display Name of the Predicate Text color
		/// </summary>
		public const string FactEditorColorsPredicateTextId = "FactEditorColors.PredicateText";
		/// <summary>
		/// Display Name of the Delimiter color
		/// </summary>
		public const string FactEditorColorsDelimiterId = "FactEditorColors.Delimiter";
		/// <summary>
		/// Display name for the ORM Designer fonts and colors category
		/// </summary>
		public const string FontsAndColorsCategoryNameId = "FontsAndColors.CategoryName";
		/// <summary>
		/// Display name for the color used to draw an ORM constraint
		/// </summary>
		public const string FontsAndColorsConstraintColorId = "FontsAndColors.ConstraintColor";
		/// <summary>
		/// Display name for the color used to draw the constraint box for role sequence editing
		/// </summary>
		public const string FontsAndColorsRolePickerColorId = "FontsAndColors.RolePickerColor";
		#endregion // Public resource ids
		#region Private resource ids
		private const string ToolboxDefaultTabName_Id = "Toolbox.DefaultTabName";
		private const string ValueType_Id = "Northface.Tools.ORM.ObjectModel.ValueType";
		private const string EntityType_Id = "Northface.Tools.ORM.ObjectModel.EntityType";
		private const string FactType_Id = "Northface.Tools.ORM.ObjectModel.FactType";
		private const string ReadingType_Id = "Northface.Tools.ORM.ObjectModel.Reading";
		private const string ObjectifiedFactType_Id = "Northface.Tools.ORM.ObjectModel.ObjectifiedFactType";
		private const string RolePlayerPickerNullItemText_Id = "Northface.Tools.ORM.ObjectModel.Editors.RolePlayerPicker.NullItemText";
		private const string NestedFactTypePickerNullItemText_Id = "Northface.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker.NullItemText";
		private const string NestingTypePickerNullItemText_Id = "Northface.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker.NullItemText";
		private const string EntityTypeDefaultNamePattern_Id = "Northface.Tools.ORM.ObjectModel.EntityType.DefaultNamePattern";
		private const string ValueTypeDefaultNamePattern_Id = "Northface.Tools.ORM.ObjectModel.ValueType.DefaultNamePattern";
		private const string FactTypeDefaultNamePattern_Id = "Northface.Tools.ORM.ObjectModel.FactType.DefaultNamePattern";
		private const string ExternalConstraintConnectActionInstructions_Id = "ExternalConstraintConnectAction.Instructions";
		private const string ExternalConstraintConnectActionTransactionName_Id = "ExternalConstraintConnectAction.TransactionName";
		private const string InsertRoleTransactionName_Id = "InsertRole.TransactionName";
		private const string InternalUniquenessConstraintConnectActionInstructions_Id = "InternalUniquenessConstraintConnectAction.Instructions";
		private const string InternalUniquenessConstraintConnectActionTransactionName_Id = "InternalUniquenessConstraintConnectAction.TransactionName";
		private const string RoleConnectActionTransactionName_Id = "RoleConnectAction.TransactionName";
		private const string OptionsPageChangeTransactionName_Id = "OptionsPageChange.TransactionName";
		private const string ModelBrowserWindowTitle_Id = "ORMModelBrowser.WindowTitle";
		private const string ModelExceptionReadingIsPrimaryToFalse_Id = "ModelException.Reading.IsPrimary.ReadOnlyWhenFalse";
		private const string ModelExceptionReadingTextChangeInvalid_Id = "ModelException.Reading.Text.InvalidText";
		private const string ModelExceptionFactAddReadingInvalidReadingText_Id = "ModelException.Fact.AddReading.InvalidReadingText";
		private const string ModelExceptionNameAlreadyUsedByModel_Id = "ModelException.Model.DuplicateName.Text";
		private const string ModelExceptionEnforceValueTypeNotNestingType_Id = "ModelException.ObjectType.EnforceValueTypeNotNestingType";
		private const string ModelExceptionEnforceRolePlayerNotNestingType_Id = "ModelException.FactType.EnforceRolePlayerNotNestingType";
		private const string ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType_Id = "ModelException.ObjectType.EnforcePreferredIdentifierForUnobjectifiedEntityType";
		private const string ModelExceptionIsMandatoryRequiresAttachedFactType_Id = "ModelException.Role.IsMandatoryRequiresAttachedFactType";
		private const string ModelExceptionPreferredIdentifierMustBeUniquenessConstraint_Id = "ModelException.Constraint.PreferredIdentifierMustBeUniquenessConstraint";
		private const string ModelExceptionInvalidInternalPreferredIdentifierPreConditions_Id = "ModelException.InternalUniquenessConstraint.InvalidPreferredIdentifierPreConditions";
		private const string ModelExceptionInternalConstraintInconsistentRoleOwners_Id = "ModelException.InternalConstraint.InconsistentRoleOwners";
		private const string CommandDeleteFactTypeText_Id = "Command.DeleteFactType.Text";
		private const string CommandDeleteObjectTypeText_Id = "Command.DeleteObjectType.Text";
		private const string CommandDeleteConstraintText_Id = "Command.DeleteConstraint.Text";
		private const string CommandDeleteRoleText_Id = "Command.DeleteRole.Text";
		private const string ModelErrorConstraintHasTooFewRoleSequencesText_Id = "ModelError.Constraint.TooFewRoleSequences.Text";
		private const string ModelErrorConstraintHasTooManyRoleSequencesText_Id = "ModelError.Constraint.TooManyRoleSequences.Text";
		private const string ModelErrorModelHasDuplicateConstraintNames_Id = "ModelError.Model.DuplicateConstraintNames.Text";
		private const string ModelErrorModelHasDuplicateFactTypeNames_Id = "ModelError.Model.DuplicateFactTypeNames.Text";
		private const string ModelErrorModelHasDuplicateObjectTypeNames_Id = "ModelError.Model.DuplicateObjectTypeNames.Text";
		private const string ModelReadingEditorListColumnHeaderReadingText_Id = "ModelReadingEditor.ListColumnHeader.ReadingText";
		private const string ModelReadingEditorListColumnHeaderIsPrimary_Id = "ModelReadingEditor.ListColumnHeader.IsPrimary";
		private const string ModelReadingEditorAllReadingsNodeName_Id = "ModelReadingEditor.AllReadingsNodeName";
		private const string ModelReadingEditorMissingRolePlayerText_Id = "ModelReadingEditor.MissingRolePlayerText";
		private const string ModelReadingEditorNewItemText_Id = "ModelReadingEditor.NewItemText";
		private const string ModelReadingEditorNewReadingTransactionText_Id = "ModelReadingEditor.NewReadingTransactionText";
		private const string ModelReadingEditorChangeReadingText_Id = "ModelReadingEditor.ChangeReadingText";
		private const string ModelReadingEditorIsPrimaryToolTip_Id = "ModelReadingEditor.IsPrimaryToolTip";
		private const string ModelReadingEditorChangePrimaryReadingText_Id = "ModelReadingEditor.ChangePrimaryReadingText";
		private const string ModelReadingEditorWindowTitle_Id = "ModelReadingEditor.WindowTitle";
		private const string ModelReadingEditorUnsupportedSelectionText_Id = "ModelReadingEditor.UnsupportedSelectionText";
		private const string PackageOfficialName_Id = "Package.OfficialName";
		private const string PackageProductDetails_Id = "Package.ProductDetails";
		private const string ModelErrorReadingTooFewRolesMessage_Id = "ModelError.Reading.TooFewRoles.Message";
		private const string ModelErrorReadingTooManyRolesMessage_Id = "ModelError.Reading.TooManyRoles.Message";
		private const string ModelReadingRoleDeletedRoleText_Id = "Model.Reading.RoleDeletedText";

		private const string ModelReferenceModeEditorAbbreviatedEntityTypeName_Id = "ModelReferenceModeEditor.AbbreviatedEntityTypeName";
		private const string ModelReferenceModeEditorAbbreviatedReferenceModeName_Id = "ModelReferenceModeEditor.AbbreviatedReferenceModeName";
		private const string ModelReferenceModeEditorAddCustomReferenceModeTransaction_Id = "ModelReferenceModeEditor.AddCustomReferenceModeTransaction";
		private const string ModelReferenceModeEditorAddNewRowText_Id = "ModelReferenceModeEditor.AddNewRowText";
		private const string ModelReferenceModeEditorChangeFormatStringTransaction_Id = "ModelReferenceModeEditor.ChangeFormatStringTransaction";
		private const string ModelReferenceModeEditorChangeNameTransaction_Id = "ModelReferenceModeEditor.ChangeNameTransaction";
		private const string ModelReferenceModeEditorCustomReferenceModesHeader_Id = "ModelReferenceModeEditor.CustomReferenceModesHeader";
		private const string ModelReferenceModeEditorEntityTypeName_Id = "ModelReferenceModeEditor.EntityTypeName";
		private const string ModelReferenceModeEditorIntrinsicReferenceModesHeader_Id = "ModelReferenceModeEditor.IntrinsicReferenceModesHeader";		                                                                                 
		private const string ModelReferenceModeEditorReferenceModeKindHeader_Id = "ModelReferenceModeEditor.ReferenceModeKindHeader";
		private const string ModelReferenceModeEditorReferenceModeName_Id = "ModelReferenceModeEditor.ReferenceModeName";
		private const string ModelReferenceModeEditorEditorWindowTitle_Id = "ModelReferenceModeEditor.EditorWindowTitle";
		private const string ModelExceptionReferenceModeEnforceUniqueFormatString_Id = "ModelException.ReferenceMode.EnforceUniqueFormatString";
		private const string ModelExceptionReferenceModeKindEnforceUniqueFormatString_Id = "ModelException.ReferenceModeKind.EnforceUniqueFormatString";
		private const string ModelExceptionReferenceModeIntrinsicRefModesDontChange_Id = "ModelException.ReferenceMode.IntrinsicRefModesDontChange";
		private const string ModelExceptionReferenceModeReferenceModesKindNotEmpty_Id = "ModelException.ReferenceMode.ReferenceModesKindNotEmpty";
		private const string ModelReferenceModeEditorChangeReferenceModeKindTransaction_Id = "ModelReferenceModeEditor.ChangeReferenceModeKindTransaction";
		private const string ReferenceModePredicateReading_Id = "ReferenceMode.PredicateReading";
		private const string ReferenceModePredicateInverseReading_Id = "ReferenceMode.PredicateInverseReading";
		private const string ModelReferenceModeEditorFormatStringColumn_Id = "ModelReferenceModeEditor.FormatStringColumn";
		private const string ModelReferenceModeEditorKindColumn_Id = "ModelReferenceModeEditor.KindColumn";
		private const string ModelReferenceModeEditorNameColumn_Id = "ModelReferenceModeEditor.NameColumn";
		private const string FactEditorToolWindowCaption_Id = "FactEditorToolWindow.Caption";
		private const string FactEditorIntellisenseImageList_Id = "FactEditor.Intellisense.ImageList";
		private const string ModelExceptionReferenceModeAmbiguousName_Id = "ModelException.ReferenceMode.AmbiguousName";
		private const string ModelReferenceModePickerFormatString_Id = "ModelReferenceModePicker.FormatString";
		#endregion // Private resource ids
		#region Public accessor properties
		/// <summary>
		/// The tab name for default toolbox
		/// </summary>
		public static string ToolboxDefaultTabName
		{
			get
			{
				return GetString(ResourceManagers.ShapeModel, ToolboxDefaultTabName_Id);
			}
		}
		/// <summary>
		/// The display name used for an ObjectType when IsValueType is false
		/// </summary>
		public static string ValueType
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, ValueType_Id);
			}
		}
		/// <summary>
		/// The display name used for an ObjectType when IsValueType is true
		/// </summary>
		public static string EntityType
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, EntityType_Id);
			}
		}
		/// <summary>
		/// The display name used for a simple FactType
		/// </summary>
		public static string FactType
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, FactType_Id);
			}
		}
		/// <summary>
		/// The display name used for an objectified (nested) FactType
		/// </summary>
		public static string ObjectifiedFactType
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, ObjectifiedFactType_Id);
			}
		}
		/// <summary>
		/// The display name used for a ReadingType
		/// </summary>
		public static string ReadingType
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, ReadingType_Id);
			}
		}
		/// <summary>
		/// The name displayed to represent null in the role player picker
		/// </summary>
		public static string RolePlayerPickerNullItemText
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, RolePlayerPickerNullItemText_Id);
			}
		}
		/// <summary>
		/// The name displayed to represent null in the nested fact type picker
		/// </summary>
		public static string NestedFactTypePickerNullItemText
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, NestedFactTypePickerNullItemText_Id);
			}
		}
		/// <summary>
		/// The name displayed to represent null in the nesting type picker
		/// </summary>
		public static string NestingTypePickerNullItemText
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, NestingTypePickerNullItemText_Id);
			}
		}
		/// <summary>
		/// The base name used to create a name for a new EntityType. This is a format string,
		/// with {0} being the placeholder for the number placement.
		/// </summary>
		public static string EntityTypeDefaultNamePattern
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, EntityTypeDefaultNamePattern_Id);
			}
		}
		/// <summary>
		/// The base name used to create a name for a new ValueType. This is a format string,
		/// with {0} being the placeholder for the number placement.
		/// </summary>
		public static string ValueTypeDefaultNamePattern
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, ValueTypeDefaultNamePattern_Id);
			}
		}
		/// <summary>
		/// The base name used to create a name for a new FactType. This is a format string,
		/// with {0} being the placeholder for the number placement.
		/// </summary>
		public static string FactTypeDefaultNamePattern
		{
			get
			{
				return GetString(ResourceManagers.ObjectModel, FactTypeDefaultNamePattern_Id);
			}
		}
		/// <summary>
		/// The instructions shown when creating an external constraint
		/// </summary>
		public static string ExternalConstraintConnectActionInstructions
		{
			get
			{
				return GetString(ResourceManagers.Diagram, ExternalConstraintConnectActionInstructions_Id);
			}
		}
		/// <summary>
		/// The transaction name used by the external constraint connect action.
		/// The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string ExternalConstraintConnectActionTransactionName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, ExternalConstraintConnectActionTransactionName_Id);
			}
		}
		/// <summary>
		/// The instructions shown when creating an internal uniqueness constraint
		/// </summary>
		public static string InternalUniquenessConstraintConnectActionInstructions
		{
			get
			{
				return GetString(ResourceManagers.Diagram, InternalUniquenessConstraintConnectActionInstructions_Id);
			}
		}
		/// <summary>
		/// The transaction name used by the internal uniqueness constraint connect action.
		/// The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string InternalUniquenessConstraintConnectActionTransactionName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, InternalUniquenessConstraintConnectActionTransactionName_Id);
			}
		}
		/// <summary>
		/// The transaction name used by the InsertRoleBefore/InsertRoleAfter commands.
		/// The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string InsertRoleTransactionName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, InsertRoleTransactionName_Id);
			}
		}
		/// <summary>
		/// The transaction name used by the role connect action.
		/// The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string RoleConnectActionTransactionName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, RoleConnectActionTransactionName_Id);
			}
		}
		/// <summary>
		/// The transaction name used when an options page change modifies
		/// diagram layout and connections. The text appears in the undo
		/// dropdown in the VS IDE.
		/// </summary>
		public static string OptionsPageChangeTransactionName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, OptionsPageChangeTransactionName_Id);
			}
		}
		/// <summary>
		/// The window title for the model browser tool window
		/// </summary>
		public static string ModelBrowserWindowTitle
		{
			get
			{
				return GetString(ResourceManagers.Diagram, ModelBrowserWindowTitle_Id);
			}
		}
		/// <summary>
		/// The category name to display on the options pages
		/// </summary>
		public static string GetOptionsPageString(string resourceName)
		{
			return GetString(ResourceManagers.Diagram, resourceName);
		}
		/// <summary>
		/// The localized string to display on Fonts and Colors setting
		/// for the ORM Designer in the options page.
		/// </summary>
		public static string GetColorNameString(string resourceName)
		{
			return GetString(ResourceManagers.Diagram, resourceName);
		}
		/// <summary>
		/// The error message to return when an attempt is made to change the IsPrimary property of a 
		/// reading to false.
		/// </summary>
		public static string ModelExceptionReadingIsPrimaryToFalse
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReadingIsPrimaryToFalse_Id);
			}
		}
		/// <summary>
		/// The error message that is returned when attempting to add a new reading to a fact
		/// and the text is not valid.
		/// 
		/// It needs to have a number of place holders equal to the number of roles in the fact
		/// and they need to have their positions identified by number using the replacement
		/// syntax of String.Format. For example: "{0} has {1}"
		/// </summary>
		public static string ModelExceptionFactAddReadingInvalidReadingText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionFactAddReadingInvalidReadingText_Id);
			}
		}
		/// <summary>
		/// Error message thrown when the Text of a reading is changed to something that is
		/// not valid for the current state of the Reading.
		/// </summary>
		public static string ModelExceptionReadingTextChangeInvalid
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReadingTextChangeInvalid_Id);
			}
		}
		/// <summary>
		/// Model validation error shown when too few role sequences are defined
		/// for a constraint. This is a frequent occurrence as external constraints
		/// are easily created in this state.
		/// </summary>
		public static string ModelErrorConstraintHasTooFewRoleSequencesText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorConstraintHasTooFewRoleSequencesText_Id);
			}
		}
		/// <summary>
		/// Model validation error shown when too many role sequences are defined
		/// for a constraint. This is an infrequent occurrence which should not
		/// be attainable via the UI, but should be possible with a hand edit
		/// of the model file.
		/// </summary>
		public static string ModelErrorConstraintHasTooManyRoleSequencesText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorConstraintHasTooManyRoleSequencesText_Id);
			}
		}
		/// <summary>
		/// Model validation error shown when multiple constraints have
		/// the same name. This is an uncommon condition that should only
		/// occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateConstraintNames
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorModelHasDuplicateConstraintNames_Id);
			}
		}
		/// <summary>
		/// Model validation error shown when multiple fact types have
		/// the same name. This is an uncommon condition that should only
		/// occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateFactTypeNames
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorModelHasDuplicateFactTypeNames_Id);
			}
		}
		/// <summary>
		/// Model validation error shown when multiple object types have
		/// the same name. This is an uncommon condition that should only
		/// occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateObjectTypeNames
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorModelHasDuplicateObjectTypeNames_Id);
			}
		}
		/// <summary>
		/// Exception message when a name change in the editor attempts to
		/// introduce a duplicate name into the model.
		/// </summary>
		public static string ModelExceptionNameAlreadyUsedByModel
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionNameAlreadyUsedByModel_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to make an
		/// object type both a value type and an objectified fact type.
		/// </summary>
		public static string ModelExceptionEnforceValueTypeNotNestingType
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionEnforceValueTypeNotNestingType_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to
		/// use the same type as both a role player and
		/// the nesting type of a fact type.
		/// </summary>
		public static string ModelExceptionEnforceRolePlayerNotNestingType
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionEnforceRolePlayerNotNestingType_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set\
		/// both a primary identifier and a value type or a
		/// nested fact type on the same object type.
		/// </summary>
		public static string ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made
		/// to set the IsMandatory property on a role
		/// of an unattached fact type. IsMandatory creates a constraint,
		/// which is owned by an ORMModel, so cannot be realized if the model is unknown.
		/// </summary>
		public static string ModelExceptionIsMandatoryRequiresAttachedFactType
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionIsMandatoryRequiresAttachedFactType_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made
		/// to create a preferred identifier relationship
		/// with an incompatible constraint type.
		/// </summary>
		public static string ModelExceptionPreferredIdentifierMustBeUniquenessConstraint
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionPreferredIdentifierMustBeUniquenessConstraint_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set an internal uniqueness
		/// constraint as a preferred identifier when the preconditions are not met.
		/// </summary>
		public static string ModelExceptionInvalidInternalPreferredIdentifierPreConditions
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionInvalidInternalPreferredIdentifierPreConditions_Id);
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to add
		/// roles from different fact types to a role sequence owned by an internal constraint.
		/// </summary>
		public static string ModelExceptionInternalConstraintInconsistentRoleOwners
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionInternalConstraintInconsistentRoleOwners_Id);
			}
		}
		/// <summary>
		/// This text appears in the edit menu when fact types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteFactTypeText
		{
			get
			{
				return GetString(ResourceManagers.Diagram, CommandDeleteFactTypeText_Id);
			}
		}
		/// <summary>
		/// This text appears in the edit menu when object types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteObjectTypeText
		{
			get
			{
				return GetString(ResourceManagers.Diagram, CommandDeleteObjectTypeText_Id);
			}
		}
		/// <summary>
		/// This text appears in the edit menu when constraint types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteConstraintText
		{
			get
			{
				return GetString(ResourceManagers.Diagram, CommandDeleteConstraintText_Id);
			}
		}
		/// <summary>
		/// This text appears in the edit menu when roles are selected in the diagram.
		/// </summary>
		public static string CommandDeleteRoleText
		{
			get
			{
				return GetString(ResourceManagers.Diagram, CommandDeleteRoleText_Id);
			}
		}
		/// <summary>
		/// Text that appears in the headers of the reading editor tool window for the reading text
		/// Text that appears in the headers of the reading editor tool window for the reading text
		/// </summary>
		public static string ModelReadingEditorListColumnHeaderReadingText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorListColumnHeaderReadingText_Id);
			}
		}
		/// <summary>
		/// Text that appears in the header of the IsPrimary column in the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorListColumnHeaderIsPrimary
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorListColumnHeaderIsPrimary_Id);
			}
		}
		/// <summary>
		/// The text to place in the node of the tree in the readings editor that shows all readings for the fact.
		/// </summary>
		public static string ModelReadingEditorAllReadingsNodeName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorAllReadingsNodeName_Id);
			}
		}
		/// <summary>
		/// The text to display in the readings editor when a role has no roleplayer.
		/// </summary>
		public static string ModelReadingEditorMissingRolePlayerText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorMissingRolePlayerText_Id);
			}
		}
		/// <summary>
		/// The text that will be used for the new reading item in the readings list of the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorNewItemText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorNewItemText_Id);
			}
		}
		/// <summary>
		/// Text used to describe the transaction created when creating a new reading.
		/// </summary>
		public static string ModelReadingEditorNewReadingTransactionText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorNewReadingTransactionText_Id);
			}
		}
		/// <summary>
		/// Text used to label the transaction created when changing reading text through the editor
		/// </summary>
		public static string ModelReadingEditorChangeReadingText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorChangeReadingText_Id);
			}
		}
		/// <summary>
		/// ToolTip text that appears on the reading list for the IsPrimary status column.
		/// </summary>
		public static string ModelReadingEditorIsPrimaryToolTip
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorIsPrimaryToolTip_Id);
			}
		}
		/// <summary>
		/// Text used to label the transaction created when which reading is primary is changed in the editor.
		/// </summary>
		public static string ModelReadingEditorChangePrimaryReadingText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorChangePrimaryReadingText_Id);
			}
		}
		/// <summary>
		/// Text to place in the title of the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorWindowTitle
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorWindowTitle_Id);
			}
		}
		/// <summary>
		/// Text that will display in the middle of the tool window when 
		/// the document view currently has an unsuppoted collection.
		/// </summary>
		public static string ModelReadingEditorUnsupportedSelectionText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingEditorUnsupportedSelectionText_Id);
			}
		}
		/// <summary>
		/// The official name of the package used in the About dialog
		/// </summary>
		/// <value></value>
		public static string PackageOfficialName
		{
			get
			{
				return GetString(ResourceManagers.Diagram, PackageOfficialName_Id);
			}
		}
		/// <summary>
		/// The description of the package used in the About dialog
		/// </summary>
		/// <value></value>
		public static string PackageProductDetails
		{
			get
			{
				return GetString(ResourceManagers.Diagram, PackageProductDetails_Id);
			}
		}

		/// <summary>
		/// returns the Text that names the reference mode.
		/// </summary>
		public static string ModelReferenceModeEditorReferenceModeName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorReferenceModeName_Id);
			}
		}
		
		/// <summary>
		/// returns the Text that names the Entity Type.
		/// </summary>
		public static string ModelReferenceModeEditorEntityTypeName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorEntityTypeName_Id);
			}
		}

		/// <summary>
		/// returns the Text used to name the Reference Mode Kind column
		/// </summary>
		public static string ModelReferenceModeEditorReferenceModeKindHeader
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorReferenceModeKindHeader_Id);
			}
		}

		/// <summary>
		/// returns the Text used to name the transaction the changes the name of a custom reference mode
		/// </summary>
		public static string ModelReferenceModeEditorChangeNameTransaction
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorChangeNameTransaction_Id);
			}
		}

		/// <summary>
		/// returns the Text used to name the Custom Reference Modes column
		/// </summary>
		public static string ModelReferenceModeEditorCustomReferenceModesHeader
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorCustomReferenceModesHeader_Id);
			}
		}

		/// <summary>
		/// returns the Text used to name the Intrinsic Reference Modes column
		/// </summary>
		public static string ModelReferenceModeEditorIntrinsicReferenceModesHeader
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorIntrinsicReferenceModesHeader_Id);
			}
		}

		/// <summary>
		/// returns the abbreviated form of the entity type name
		/// </summary>
		public static string ModelReferenceModeEditorAbbreviatedEntityTypeName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorAbbreviatedEntityTypeName_Id);
			}
		}

		/// <summary>
		/// return the abbreviated form of the reference mode name
		/// </summary>
		public static string ModelReferenceModeEditorAbbreviatedReferenceModeName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorAbbreviatedReferenceModeName_Id);
			}
		}
		
		/// <summary>
		/// returns the text used to name the transaction that adds a custom reference mode
		/// </summary>
		public static string ModelReferenceModeEditorAddCustomReferenceModeTransaction
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorAddCustomReferenceModeTransaction_Id);
			}
		}

		/// <summary>
		/// returns the text to display the add a new row to the custom reference modes branch
		/// </summary>
		public static string ModelReferenceModeEditorAddNewRowText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorAddNewRowText_Id);
			}
		}

		/// <summary>
		/// returns the text used to name the transaction that changes the format string. 
		/// </summary>
		public static string ModelReferenceModeEditorChangeFormatStringTransaction
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorChangeFormatStringTransaction_Id);
			}
		}

		/// <summary>
		/// returns The text that displays the title of the editor window.
		/// </summary>
		public static string ModelReferenceModeEditorEditorWindowTitle
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorEditorWindowTitle_Id);
			}
		}

		/// <summary>
		/// returns Exception message when the unique format string rule is violated for reference modes
		/// </summary>
		public static string ModelExceptionReferenceModeEnforceUniqueFormatString
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReferenceModeEnforceUniqueFormatString_Id);
			}
		}

		/// <summary>
		/// returns Exception message when the unique format string rule is violated for reference mode kinds
		/// </summary>
		public static string ModelExceptionReferenceModeKindEnforceUniqueFormatString
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReferenceModeKindEnforceUniqueFormatString_Id);
			}
		}

		/// <summary>
		/// returns Exception messege when atttempt is made to change the kind of an intrinsic reference mode
		/// </summary>
		public static string ModelExceptionReferenceModeIntrinsicRefModesDontChange
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReferenceModeIntrinsicRefModesDontChange_Id);
			}
		}

		/// <summary>
		/// returns Exception messege when attempt is made to remove reference mode kind.
		/// </summary>
		public static string ModelExceptionReferenceModeReferenceModesKindNotEmpty
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReferenceModeReferenceModesKindNotEmpty_Id);
			}
		}

		/// <summary>
		/// returns Text used to name the transaction that changes the referencemode Kind.
		/// </summary>
		public static string ModelReferenceModeEditorChangeReferenceModeKindTransaction
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorChangeReferenceModeKindTransaction_Id);
			}
		}

		/// <summary>
		/// returns The reading for the predicate created by setting the ref mode.
		/// </summary>
		public static string ReferenceModePredicateReading
		{
			get
			{
				return GetString(ResourceManagers.Model, ReferenceModePredicateReading_Id);
			}
		}

		/// <summary>
		/// returns The inverse reading for the predicate created by setting the ref mode.
		/// </summary>
		public static string ReferenceModePredicateInverseReading
		{
			get
			{
				return GetString(ResourceManagers.Model, ReferenceModePredicateInverseReading_Id);
			}
		}

		/// <summary>
		/// returns The Column header text for the Format String column.
		/// </summary>
		public static string ModelReferenceModeEditorFormatStringColumn
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorFormatStringColumn_Id);
			}
		}

		/// <summary>
		/// returns The Column header text for the Kind column.
		/// </summary>
		public static string ModelReferenceModeEditorKindColumn
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorKindColumn_Id);
			}
		}

		/// <summary>
		/// returns The Column header text for the name column.
		/// </summary>
		public static string ModelReferenceModeEditorNameColumn
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModeEditorNameColumn_Id);
			}
		}

		/// <summary>
		/// The caption of the Fact Editor Tool Window
		/// </summary>
		/// <value></value>
		public static string FactEditorToolWindowCaption
		{
			get
			{
				return GetString(ResourceManagers.Diagram, FactEditorToolWindowCaption_Id);
			}
		}

		/// <summary>
		/// The images for the Intellisense drop down
		/// </summary>
		public static ImageListStreamer FactEditorIntellisenseImageList
		{
			get
			{
				return GetObject(ResourceManagers.Diagram, FactEditorIntellisenseImageList_Id) as ImageListStreamer;
			}
		}

		/// <summary>
		/// Exception message output when multiple reference modes are found with the same name
		/// </summary>
		public static string ModelExceptionReferenceModeAmbiguousName
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelExceptionReferenceModeAmbiguousName_Id);
			}
		}

		/// <summary>
		/// returns the format string for the display text for reference mode picker
		/// </summary>
		public static string ModelReferenceModePickerFormatString
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReferenceModePickerFormatString_Id);
			}
		}

		/// <summary>
		/// Text displayed in the text of the TooFewRolesError
		/// </summary>
		public static string ModelErrorReadingTooFewRolesMessage
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorReadingTooFewRolesMessage_Id);
			}
		}

		/// <summary>
		/// Text displayed in the text of the TooManyRolesError
		/// </summary>
		public static string ModelErrorReadingTooManyRolesMessage
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelErrorReadingTooManyRolesMessage_Id);
			}
		}

		/// <summary>
		/// Text used to replace a role place holder when the role is deleted
		/// </summary>
		/// <value></value>
		public static string ModelReadingRoleDeletedRoleText
		{
			get
			{
				return GetString(ResourceManagers.Model, ModelReadingRoleDeletedRoleText_Id);
			}
		}
		#endregion // Public accessor properties
	}
}
