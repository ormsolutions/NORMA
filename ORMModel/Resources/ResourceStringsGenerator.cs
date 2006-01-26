namespace Neumont.Tools.ORM
{
	#region ResourceStrings class
	/// <summary>
	/// A helper class to insulate the rest of the code from direct resource manipulation.
	/// </summary>
	internal partial class ResourceStrings
	{
		/// <summary>
		/// The tab name for default toolbox
		/// </summary>
		public static string ToolboxDefaultTabName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ShapeModel, "Toolbox.DefaultTabName");
			}
		}
		/// <summary>
		/// The display name used for an ObjectType when IsValueType is false
		/// </summary>
		public static string ValueType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueType");
			}
		}
		/// <summary>
		/// The display name used for an ObjectType when IsValueType is true
		/// </summary>
		public static string EntityType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.EntityType");
			}
		}
		/// <summary>
		/// The display name used for a simple FactType
		/// </summary>
		public static string FactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.FactType");
			}
		}
		/// <summary>
		/// The display name used for a SubtypeFact
		/// </summary>
		public static string SubtypeFact
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.SubtypeFact");
			}
		}
		/// <summary>
		/// The display name used for an objectified (nested) FactType
		/// </summary>
		public static string ObjectifiedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ObjectifiedFactType");
			}
		}
		/// <summary>
		/// The display name used for a ReadingType
		/// </summary>
		public static string ReadingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Reading");
			}
		}
		/// <summary>
		/// The name displayed to represent null in the role player picker
		/// </summary>
		public static string RolePlayerPickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.RolePlayerPicker.NullItemText");
			}
		}
		/// <summary>
		/// The name displayed to represent null in the nested fact type picker
		/// </summary>
		public static string NestedFactTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker.NullItemText");
			}
		}
		/// <summary>
		/// The name displayed to represent null in the nesting type picker
		/// </summary>
		public static string NestingTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker.NullItemText");
			}
		}
		/// <summary>
		/// The base name used to create a name for a new EntityType. This is a format string
		/// </summary>
		public static string EntityTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.EntityType.DefaultNamePattern");
			}
		}
		/// <summary>
		/// The descriptive text for a PortableDataType of Unspecified.
		/// </summary>
		public static string PortableDataTypeUnspecified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Unspecified.Text");
			}
		}
		/// <summary>
		/// A fixed length text data type
		/// </summary>
		public static string PortableDataTypeTextFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.FixedLength.Text");
			}
		}
		/// <summary>
		/// A variable length text data type
		/// </summary>
		public static string PortableDataTypeTextVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.VariableLength.Text");
			}
		}
		/// <summary>
		/// A large length text data type
		/// </summary>
		public static string PortableDataTypeTextLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.LargeLength.Text");
			}
		}
		/// <summary>
		/// A signed integer numeric data type
		/// </summary>
		public static string PortableDataTypeNumericSignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.SignedInteger.Text");
			}
		}
		/// <summary>
		/// An unsigned integer numeric data type
		/// </summary>
		public static string PortableDataTypeNumericUnsignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.UnsignedInteger.Text");
			}
		}
		/// <summary>
		/// An auto counter numeric data type
		/// </summary>
		public static string PortableDataTypeNumericAutoCounter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.AutoCounter.Text");
			}
		}
		/// <summary>
		/// A floating point numeric data type
		/// </summary>
		public static string PortableDataTypeNumericFloatingPoint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.FloatingPoint.Text");
			}
		}
		/// <summary>
		/// A decimal numeric data type
		/// </summary>
		public static string PortableDataTypeNumericDecimal
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.Decimal.Text");
			}
		}
		/// <summary>
		/// A money numeric data type
		/// </summary>
		public static string PortableDataTypeNumericMoney
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.Money.Text");
			}
		}
		/// <summary>
		/// A fixed length raw data data type
		/// </summary>
		public static string PortableDataTypeRawDataFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.FixedLength.Text");
			}
		}
		/// <summary>
		/// A variable length raw data data type
		/// </summary>
		public static string PortableDataTypeRawDataVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.VariableLength.Text");
			}
		}
		/// <summary>
		/// A large length raw data data type
		/// </summary>
		public static string PortableDataTypeRawDataLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.LargeLength.Text");
			}
		}
		/// <summary>
		/// A picture raw data data type
		/// </summary>
		public static string PortableDataTypeRawDataPicture
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.Picture.Text");
			}
		}
		/// <summary>
		/// An OLE object raw data data type
		/// </summary>
		public static string PortableDataTypeRawDataOleObject
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.OleObject.Text");
			}
		}
		/// <summary>
		/// An auto timestamp temporal data type
		/// </summary>
		public static string PortableDataTypeTemporalAutoTimestamp
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.AutoTimestamp.Text");
			}
		}
		/// <summary>
		/// An time temporal data type
		/// </summary>
		public static string PortableDataTypeTemporalTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.Time.Text");
			}
		}
		/// <summary>
		/// An date temporal data type
		/// </summary>
		public static string PortableDataTypeTemporalDate
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.Date.Text");
			}
		}
		/// <summary>
		/// An date and time temporal data type
		/// </summary>
		public static string PortableDataTypeTemporalDateAndTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.DateAndTime.Text");
			}
		}
		/// <summary>
		/// A true or false logical data type
		/// </summary>
		public static string PortableDataTypeLogicalTrueOrFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Logical.TrueOrFalse.Text");
			}
		}
		/// <summary>
		/// A yes or no logical data type
		/// </summary>
		public static string PortableDataTypeLogicalYesOrNo
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Logical.YesOrNo.Text");
			}
		}
		/// <summary>
		/// A row id data type (can not be classified in any of the groups above)
		/// </summary>
		public static string PortableDataTypeOtherRowId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Other.RowId.Text");
			}
		}
		/// <summary>
		/// An object id data type (can not be classified in any of the groups above)
		/// </summary>
		public static string PortableDataTypeOtherObjectId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Other.ObjectId.Text");
			}
		}
		/// <summary>
		/// The base name used to create a name for a new ValueType. This is a format string with {0} being the placeholder for the number placement.
		/// </summary>
		public static string ValueTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueType.DefaultNamePattern");
			}
		}
		/// <summary>
		/// The base name used to create a name for a new FactType. This is a format string with {0} being the placeholder for the number placement.
		/// </summary>
		public static string FactTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.FactType.DefaultNamePattern");
			}
		}
		/// <summary>
		/// The base name used to create a name for a new SubtypeFact. This is a format string with {0} being the placeholder for the number placement.
		/// </summary>
		public static string SubtypeFactDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.DefaultNamePattern");
			}
		}
		/// <summary>
		/// The inverse reading for the predicate created by creating a sub type relationship.
		/// </summary>
		public static string SubtypeFactPredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.PredicateInverseReading");
			}
		}
		/// <summary>
		/// The reading for the forward predicate created by creating a subtype relationship.
		/// </summary>
		public static string SubtypeFactPredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.PredicateReading");
			}
		}
		/// <summary>
		/// The inverse reading for the predicate created implicitly via objectification. There is no attempt made to keep the predicate readings unique in ring situations.We allow the model error to populate instead of generating an articial unique reading.
		/// </summary>
		public static string ImpliedFactTypePredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImpliedFactType.PredicateInverseReading");
			}
		}
		/// <summary>
		/// The reading for the predicate created implicitly via objectification. There is no attempt made to keep the predicate readings unique in ring situations.We allow the model error to populate instead of generating an articial unique reading.
		/// </summary>
		public static string ImpliedFactTypePredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImpliedFactType.PredicateReading");
			}
		}
		/// <summary>
		/// String for generating a component name for a subtype. The {0} replacement field is used for the subtype component name; {1} for the supertype.
		/// </summary>
		public static string SubtypeFactComponentNameFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.ComponentNameFormat");
			}
		}
		/// <summary>
		/// The instructions shown when creating an external constraint
		/// </summary>
		public static string ExternalConstraintConnectActionInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.Instructions");
			}
		}
		/// <summary>
		/// The transaction name used by the external constraint connect action. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string ExternalConstraintConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.TransactionName");
			}
		}
		/// <summary>
		/// The string used to represent a greater than symbol on a frequency constraint.
		/// </summary>
		public static string FrequencyConstraintMinimumFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.Minimum.FormatString");
			}
		}
		/// <summary>
		/// The string used to represent 'between' on a frequency constraint (eg: 'Between 1 and 5' would be represented as '1[this string]5'.
		/// </summary>
		public static string FrequencyConstraintBetweenFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.Between.FormatString");
			}
		}
		/// <summary>
		/// The instructions shown when creating an internal uniqueness constraint
		/// </summary>
		public static string InternalUniquenessConstraintConnectActionInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InternalUniquenessConstraintConnectAction.Instructions");
			}
		}
		/// <summary>
		/// The transaction name used by the internal uniqueness constraint connect action. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string InternalUniquenessConstraintConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InternalUniquenessConstraintConnectAction.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used by the InsertRoleBefore/InsertRoleAfter commands. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string InsertRoleTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InsertRole.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used by the role connect action. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string RoleConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "RoleConnectAction.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used by the subtype connect action. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string SubtypeConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SubtypeConnectAction.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used when an options page change modifies diagram layout and connections. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string OptionsPageChangeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "OptionsPageChange.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used for deleting a role sequence from a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string DeleteRoleSequenceTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DeleteRoleSequence.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used for moving a role sequence down in a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string MoveRoleSequenceDownTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MoveRoleSequenceDown.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used for moving a role sequence up in a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE. 
		/// </summary>
		public static string MoveRoleSequenceUpTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MoveRoleSequenceUp.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used for changes made in response to committing a modified line in the fact editor. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string InterpretFactEditorLineTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InterpretFactEditorLine.TransactionName");
			}
		}
		/// <summary>
		/// The window title for the model browser tool window
		/// </summary>
		public static string ModelBrowserWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ORMModelBrowser.WindowTitle");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to create an objectification relationship when the Model is not specified for either the NestingType or the NestedFactType.
		/// </summary>
		public static string ModelExceptionObjectificationRequiresModel
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ModelNotSpecified");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to modify roles, constraints, and role players on elements implied by an Objectification relationship.
		/// </summary>
		public static string ModelExceptionObjectificationImpliedElementModified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.DirectModificationOfImpliedElement");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to objectify a fact that is implied by another objectification.
		/// </summary>
		public static string ModelExceptionObjectificationImpliedFactObjectified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedFactTypesCannotBeObjectified");
			}
		}
		/// <summary>
		/// The error message to return when an attempt is made to change the IsPrimary property of a reading to false.
		/// </summary>
		public static string ModelExceptionReadingIsPrimaryToFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Reading.IsPrimary.ReadOnlyWhenFalse");
			}
		}
		/// <summary>
		/// The error message that is returned when attempting to add a new reading to a fact and the text is not valid.  It needs to have a number of place holders equal to the number of roles in the fact and they need to have their positions identified by number using the replacement syntax of String.Format. For example: "{0} has {1}"
		/// </summary>
		public static string ModelExceptionFactAddReadingInvalidReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Fact.AddReading.InvalidReadingText");
			}
		}
		/// <summary>
		/// Model validation error shown when too few role sequences are defined for a constraint. This is a frequent occurrence as external constraints are easily created in this state.
		/// </summary>
		public static string ModelErrorConstraintHasTooFewRoleSequencesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.TooFewRoleSequences.Text");
			}
		}
		/// <summary>
		/// Model validation error text used when a frequency constraint contradicts an internal uniqueness constraint
		/// </summary>
		public static string FrequencyConstraintContradictsInternalUniquenessConstraintText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintContradictsInternalUniquenessConstraintError.Text");
			}
		}
		/// <summary>
		/// Model validation error shown when too many role sequences are defined for a constraint. This is an infrequent occurrence which should not be attainable via the UI
		/// </summary>
		public static string ModelErrorConstraintHasTooManyRoleSequencesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.TooManyRoleSequences.Text");
			}
		}
		/// <summary>
		/// Model validation error text when role sequences in a multi column constraint have different role counts (arity).
		/// </summary>
		public static string ModelErrorConstraintExternalConstraintArityMismatch
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ExternalConstraintArityMismatch.Text");
			}
		}
		/// <summary>
		/// Model validation error shown when multiple constraints have the same name. This is an uncommon condition that should only occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateConstraintNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateConstraintNames.Text");
			}
		}
		/// <summary>
		/// Model validation error shown when multiple fact types have the same name. This is an uncommon condition that should only occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateFactTypeNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateFactTypeNames.Text");
			}
		}
		/// <summary>
		/// Model validation error shown when multiple object types have the same name. This is an uncommon condition that should only occur with a hand edit to a model file.
		/// </summary>
		public static string ModelErrorModelHasDuplicateObjectTypeNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateObjectTypeNames.Text");
			}
		}
		/// <summary>
		/// Role data type does not match max inclusion
		/// </summary>
		public static string ModelErrorRoleValueRangeMaxValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MaxValueMismatchError.Message2");
			}
		}
		/// <summary>
		/// Role data type does not match min inclusion
		/// </summary>
		public static string ModelErrorRoleValueRangeMinValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MinValueMismatchError.Message2");
			}
		}
		/// <summary>
		/// Data type does not match max inclusion
		/// </summary>
		public static string ModelErrorValueRangeMaxValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MaxValueMismatchError.Message");
			}
		}
		/// <summary>
		/// Data type does not match min inclusion
		/// </summary>
		public static string ModelErrorValueRangeMinValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MinValueMismatchError.Message");
			}
		}
		/// <summary>
		/// Exception message when a name change in the editor attempts to introduce a duplicate name into the model.
		/// </summary>
		public static string ModelExceptionNameAlreadyUsedByModel
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Model.DuplicateName.Text");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to make an object type both a value type and an objectified fact type.
		/// </summary>
		public static string ModelExceptionEnforceValueTypeNotNestingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectType.EnforceValueTypeNotNestingType");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to use the same type as both a role player and the nesting type of a fact type.
		/// </summary>
		public static string ModelExceptionEnforceRolePlayerNotNestingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactType.EnforceRolePlayerNotNestingType");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set\ both a primary identifier and a value type or a nested fact type on the same object type.
		/// </summary>
		public static string ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectType.EnforcePreferredIdentifierForUnobjectifiedEntityType");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set the IsMandatory property on an unattached role. IsMandatory creates an internal constraint, which is owned by an FactType, so cannot be realized if the parent fact is unknown.
		/// </summary>
		public static string ModelExceptionIsMandatoryRequiresAttachedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Role.IsMandatoryRequiresAttachedFactType");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to create a preferred identifier relationship with an incompatible constraint type.
		/// </summary>
		public static string ModelExceptionPreferredIdentifierMustBeUniquenessConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Constraint.PreferredIdentifierMustBeUniquenessConstraint");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set an external uniqueness constraint as a preferred identifier when the preconditions are not met.
		/// </summary>
		public static string ModelExceptionInvalidExternalPreferredIdentifierPreConditions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ExternalUniquenessConstraint.InvalidPreferredIdentifierPreConditions");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to set an internal uniqueness constraint as a preferred identifier when the preconditions are not met.
		/// </summary>
		public static string ModelExceptionInvalidInternalPreferredIdentifierPreConditions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.InternalUniquenessConstraint.InvalidPreferredIdentifierPreConditions");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to add roles from different fact types to a role sequence owned by an internal constraint.
		/// </summary>
		public static string ModelExceptionInternalConstraintInconsistentRoleOwners
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.InternalConstraint.InconsistentRoleOwners");
			}
		}
		/// <summary>
		/// This text appears in the edit menu when fact types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteFactTypeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteFactType.Text");
			}
		}
		/// <summary>
		/// This text appears in the edit menu when object types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteObjectTypeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteObjectType.Text");
			}
		}
		/// <summary>
		/// This text appears in the edit menu when constraint types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteConstraintText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteConstraint.Text");
			}
		}
		/// <summary>
		/// This text appears in the edit menu when multiple elements of different types are selected in the diagram.
		/// </summary>
		public static string CommandDeleteMultipleText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteMultiple.Text");
			}
		}
		/// <summary>
		/// This text appears in the edit menu when roles are selected in the diagram.
		/// </summary>
		public static string CommandDeleteRoleText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteRole.Text");
			}
		}
		/// <summary>
		/// Text that appears in the headers of the reading editor tool window for the reading text Text that appears in the headers of the reading editor tool window for the reading text
		/// </summary>
		public static string ModelReadingEditorListColumnHeaderReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ListColumnHeader.ReadingText");
			}
		}
		/// <summary>
		/// Text that appears in the header of the IsPrimary column in the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorListColumnHeaderIsPrimary
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ListColumnHeader.IsPrimary");
			}
		}
		/// <summary>
		/// The text to place in the node of the tree in the readings editor that shows all readings for the fact.
		/// </summary>
		public static string ModelReadingEditorAllReadingsNodeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.AllReadingsNodeName");
			}
		}
		/// <summary>
		/// The text to display in the readings editor when a role has no roleplayer.
		/// </summary>
		public static string ModelReadingEditorMissingRolePlayerText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MissingRolePlayerText");
			}
		}
		/// <summary>
		/// The text that will be used for the new reading item in the readings list of the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorNewItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.NewItemText");
			}
		}
		/// <summary>
		/// Text used to describe the transaction created when creating a new reading.
		/// </summary>
		public static string ModelReadingEditorNewReadingTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.NewReadingTransactionText");
			}
		}
		/// <summary>
		/// Text used to label the transaction created when changing reading text through the editor
		/// </summary>
		public static string ModelReadingEditorChangeReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ChangeReadingText");
			}
		}
		/// <summary>
		/// ToolTip text that appears on the reading list for the IsPrimary status column.
		/// </summary>
		public static string ModelReadingEditorIsPrimaryToolTip
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.IsPrimaryToolTip");
			}
		}
		/// <summary>
		/// Text used to label the transaction created when which reading is primary is changed in the editor.
		/// </summary>
		public static string ModelReadingEditorChangePrimaryReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ChangePrimaryReadingText");
			}
		}
		/// <summary>
		/// Text to place in the title of the reading editor tool window.
		/// </summary>
		public static string ModelReadingEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.WindowTitle");
			}
		}
		/// <summary>
		/// Text that will display in the middle of the tool window when  the document view currently has an unsuppoted collection.
		/// </summary>
		public static string ModelReadingEditorUnsupportedSelectionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.UnsupportedSelectionText");
			}
		}
		/// <summary>
		/// The official name of the package used in the About dialog
		/// </summary>
		public static string PackageOfficialName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Package.OfficialName");
			}
		}
		/// <summary>
		/// The description of the package used in the About dialog
		/// </summary>
		public static string PackageProductDetails
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Package.ProductDetails");
			}
		}
		/// <summary>
		/// returns the Text that names the reference mode.
		/// </summary>
		public static string ModelReferenceModeEditorReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeName");
			}
		}
		/// <summary>
		/// returns the Text that names the Entity Type.
		/// </summary>
		public static string ModelReferenceModeEditorEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EntityTypeName");
			}
		}
		/// <summary>
		/// returns the Text used to name the Reference Mode Kind column
		/// </summary>
		public static string ModelReferenceModeEditorReferenceModeKindHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeKindHeader");
			}
		}
		/// <summary>
		/// returns the Text used to name the transaction the changes the name of a custom reference mode
		/// </summary>
		public static string ModelReferenceModeEditorChangeNameTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeNameTransaction");
			}
		}
		/// <summary>
		/// returns the Text used to name the Custom Reference Modes column
		/// </summary>
		public static string ModelReferenceModeEditorCustomReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.CustomReferenceModesHeader");
			}
		}
		/// <summary>
		/// returns the Text used to name the Intrinsic Reference Modes column
		/// </summary>
		public static string ModelReferenceModeEditorIntrinsicReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.IntrinsicReferenceModesHeader");
			}
		}
		/// <summary>
		/// returns the abbreviated form of the entity type name
		/// </summary>
		public static string ModelReferenceModeEditorAbbreviatedEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedEntityTypeName");
			}
		}
		/// <summary>
		/// return the abbreviated form of the reference mode name
		/// </summary>
		public static string ModelReferenceModeEditorAbbreviatedReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedReferenceModeName");
			}
		}
		/// <summary>
		/// returns the text used to name the transaction that adds a custom reference mode
		/// </summary>
		public static string ModelReferenceModeEditorAddCustomReferenceModeTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddCustomReferenceModeTransaction");
			}
		}
		/// <summary>
		/// returns the text to display the add a new row to the custom reference modes branch
		/// </summary>
		public static string ModelReferenceModeEditorAddNewRowText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddNewRowText");
			}
		}
		/// <summary>
		/// returns the text used to name the transaction that changes the format string. 
		/// </summary>
		public static string ModelReferenceModeEditorChangeFormatStringTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeFormatStringTransaction");
			}
		}
		/// <summary>
		/// returns The text that displays the title of the editor window.
		/// </summary>
		public static string ModelReferenceModeEditorEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EditorWindowTitle");
			}
		}
		/// <summary>
		/// returns Exception message when the unique format string rule is violated for reference modes
		/// </summary>
		public static string ModelExceptionReferenceModeEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceUniqueFormatString");
			}
		}
		/// <summary>
		/// returns Exception message when the unique format string rule is violated for reference mode kinds
		/// </summary>
		public static string ModelExceptionReferenceModeKindEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceModeKind.EnforceUniqueFormatString");
			}
		}
		/// <summary>
		/// returns Exception messege when atttempt is made to change the kind of an intrinsic reference mode
		/// </summary>
		public static string ModelExceptionReferenceModeIntrinsicRefModesDontChange
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.IntrinsicRefModesDontChange");
			}
		}
		/// <summary>
		/// returns Exception messege when attempt is made to remove reference mode kind.
		/// </summary>
		public static string ModelExceptionReferenceModeReferenceModesKindNotEmpty
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.ReferenceModesKindNotEmpty");
			}
		}
		/// <summary>
		/// returns Text used to name the transaction that changes the referencemode Kind.
		/// </summary>
		public static string ModelReferenceModeEditorChangeReferenceModeKindTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeReferenceModeKindTransaction");
			}
		}
		/// <summary>
		/// returns The reading for the predicate created by setting the ref mode.
		/// </summary>
		public static string ReferenceModePredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateReading");
			}
		}
		/// <summary>
		/// returns The inverse reading for the predicate created by setting the ref mode.
		/// </summary>
		public static string ReferenceModePredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateInverseReading");
			}
		}
		/// <summary>
		/// returns The Column header text for the Format String column.
		/// </summary>
		public static string ModelReferenceModeEditorFormatStringColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.FormatStringColumn");
			}
		}
		/// <summary>
		/// returns The Column header text for the Kind column.
		/// </summary>
		public static string ModelReferenceModeEditorKindColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.KindColumn");
			}
		}
		/// <summary>
		/// returns The Column header text for the name column.
		/// </summary>
		public static string ModelReferenceModeEditorNameColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.NameColumn");
			}
		}
		/// <summary>
		/// The caption of the Fact Editor Tool Window
		/// </summary>
		public static string FactEditorToolWindowCaption
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FactEditorToolWindow.Caption");
			}
		}
		/// <summary>
		/// Exception message output when multiple reference modes are found with the same name
		/// </summary>
		public static string ModelExceptionReferenceModeAmbiguousName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.AmbiguousName");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to modify the roles collection or internal constraints of a SubtypeFact.
		/// </summary>
		public static string ModelExceptionSubtypeConstraintAndRolePatternFixed
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.ConstraintAndRolePatternFixed");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to nest a SubtypeFact.
		/// </summary>
		public static string ModelExceptionSubtypeFactNotNested
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.NotNested");
			}
		}
		/// <summary>
		/// Exception message when an attempt is made to add a subtype relationship where the subtype is a direct or indirect subtype of the supertype.
		/// </summary>
		public static string ModelExceptionSubtypeFactCycle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.Cycle");
			}
		}
		/// <summary>
		/// returns the format string for the display text for reference mode picker
		/// </summary>
		public static string ModelReferenceModePickerFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModePicker.FormatString");
			}
		}
		/// <summary>
		/// Text displayed in the text of the TooFewRolesError
		/// </summary>
		public static string ModelErrorReadingTooFewRolesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Reading.TooFewRoles.Message");
			}
		}
		/// <summary>
		/// Text displayed in the text of the TooManyRolesError
		/// </summary>
		public static string ModelErrorReadingTooManyRolesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Reading.TooManyRoles.Message");
			}
		}
		/// <summary>
		/// Text used to replace a role place holder when the role is deleted
		/// </summary>
		public static string ModelReadingRoleDeletedRoleText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Model.Reading.RoleDeletedText");
			}
		}
		/// <summary>
		/// Text displayed in the text of the FactTypeRequiresReadingError
		/// </summary>
		public static string ModelErrorFactTypeRequiresInternalUniquessConstraintMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresInternalUniquenessConstraint.Message");
			}
		}
		/// <summary>
		/// Text displayed in the text of the FactTypeRequiresInternalUniquessContraintError
		/// </summary>
		public static string ModelErrorFactTypeRequiresReadingMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresReading.Message");
			}
		}
		/// <summary>
		/// Text displayed for an unspecified data type.
		/// </summary>
		public static string ModelErrorValueTypeDataTypeNotSpecifiedMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueType.DataTypeNotSpecified.Message");
			}
		}
		/// <summary>
		/// The name given to the transaction used when adding an internal uniqueness constraint to correct the error of lacking one.
		/// </summary>
		public static string ModelErrorFactTypeRequiresIUCActivateTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresIUC.ActivateTransactionName");
			}
		}
		/// <summary>
		/// Pattern showing left- and right-string to use for containing a value range definition.
		/// </summary>
		public static string ValueRangeDefinitionDefinitionContainerPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.DefinitionContainerPattern");
			}
		}
		/// <summary>
		/// String used to delimit sets of value ranges in a definition.
		/// </summary>
		public static string ValueRangeDefinitionRangeDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.RangeDelimiter");
			}
		}
		/// <summary>
		/// String used to delimit the min- and max-values of a value range.
		/// </summary>
		public static string ValueRangeDefinitionValueDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.ValueDelimiter");
			}
		}
		/// <summary>
		/// Pattern showing left- and right-string to use for containing a value range as a string.
		/// </summary>
		public static string ValueRangeDefinitionStringContainerPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.StringContainerPattern");
			}
		}
		/// <summary>
		/// Pattern showing left- and right-string to use to indicate the min- and max-values are open (i.e. the value itself is not a member of the range).
		/// </summary>
		public static string ValueRangeDefinitionOpenInclusionContainer
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.OpenInclusionPattern");
			}
		}
		/// <summary>
		/// Pattern showing left- and right-string to use to indicate the min- and max-values are closed (i.e. the value itself is a member of the range).
		/// </summary>
		public static string ValueRangeDefinitionClosedInclusionContainer
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueRangeDefinition.ClosedInclusionPattern");
			}
		}
		/// <summary>
		/// The string used to display that an object is independent.
		/// </summary>
		public static string ObjectTypeShapeIsIndependentReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectTypeShape.IsIndependentReading");
			}
		}
		/// <summary>
		/// The string used to divide multiple readings shown in a ReadingShape.
		/// </summary>
		public static string ReadingShapeReadingSeparator
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.ReadingSeparator");
			}
		}
		/// <summary>
		/// The character to use as the object placeholder in a ReadingShape (an ellipsis).
		/// </summary>
		public static string ReadingShapeEllipsis
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.Ellipsis");
			}
		}
		/// <summary>
		/// The string to use to indicate that a reading goes in the opposite direction. The preferred string (per Terry Halpin) is "\u25C4 ", but the unicode character is not available in that Tahoma font.
		/// </summary>
		public static string ReadingShapeInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.InverseReading");
			}
		}
		/// <summary>
		/// The string used to display a reading with a non-primary order when the role is attached.
		/// </summary>
		public static string ReadingShapeAttachedRoleDisplay
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.AttachedRoleDisplay");
			}
		}
		/// <summary>
		/// The string used to display a reading with a non-primary order when the role is not attached.
		/// </summary>
		public static string ReadingShapeUnattachedRoleDisplay
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.UnattachedRoleDisplay");
			}
		}
		/// <summary>
		/// Text diplayed in the Model Error when the span of the internal constraint is less than the span of the Fact Type - 1
		/// </summary>
		public static string NMinusOneRuleInternalSpan
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.NMinusOneRule.Text");
			}
		}
		/// <summary>
		/// The role players in an external constraint must have compatible types. Replacement field {0} is the constraint name and {1} is the model name.
		/// </summary>
		public static string ModelErrorSingleColumnConstraintCompatibleRolePlayerTypeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.CompatibleRolePlayerTypeError.SingleColumn.Text");
			}
		}
		/// <summary>
		/// The role players in an external constraint column must have compatible types. Replacement field {0} is the constraint name, {1} is the model name, and {2} is the (1-based) column number.
		/// </summary>
		public static string ModelErrorMultiColumnConstraintCompatibleRolePlayerTypeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.CompatibleRolePlayerTypeError.MultiColumn.Text");
			}
		}
		/// <summary>
		/// Text displayed in the text of the RoleRequiresRolePlayerError. {0} is the (1-based) role number, {1} is the name of the fact, and {2} is the name of the model.
		/// </summary>
		public static string ModelErrorRolePlayerRequiredError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Role.RolePlayerRequired.Message");
			}
		}
		/// <summary>
		/// Constraint '{0}' is implied by mandatory constraints.
		/// </summary>
		public static string ModelErrorExternalEqualityIsImpliedByMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ExternalEqualityIsImpliedByMandatory.Text");
			}
		}
		/// <summary>
		/// An entity type must have a primary reference scheme. Replacement field {0} is the entity type name and {1} is the model name.
		/// </summary>
		public static string ModelErrorEntityTypeRequiresReferenceSchemeMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.EntityType.RequiresReferenceScheme.Message");
			}
		}
		/// <summary>
		/// The mandatory disjunctive constraint is implied by a simple mandatory constraint. Replacement field {0} is the constraint name and {1} is the model name.
		/// </summary>
		public static string SimpleMandatoryImpliesDisjunctiveMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.SimpleMandatoryImpliesDisjunctiveMandatoryError.Message");
			}
		}
		/// <summary>
		/// The ring constraint type must be specified. {0} is the constraint name and {1} is the model name.
		/// </summary>
		public static string RingConstraintTypeNotSpecifiedError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.RingConstraintTypeNotSpecifiedError.Message");
			}
		}
		/// <summary>
		/// Text displayed in the text of the FrequencyConstraintMinMaxError. {0} is the constraint name and {1} is the model name
		/// </summary>
		public static string ModelErrorFrequencyConstraintMinMaxError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintMinMaxError.Text");
			}
		}
		/// <summary>
		/// Text to place in the title of the verbalization tool window.
		/// </summary>
		public static string ModelVerbalizationWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelVerbalization.WindowTitle");
			}
		}
		/// <summary>
		/// Text displayed in the text of the ImpliedInternalUniquenessConstraintError. {0} is the fact type name
		/// </summary>
		public static string ModelErrorImpliedInternalUniquenessConstraintError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.ImpliedInternalUniquenessConstraintError.Text");
			}
		}
		/// <summary>
		/// The message of the auto-fix message box for implied internal constraints
		/// </summary>
		public static string ImpliedInternalConstraintFixMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MesageBox.ImpliedInternalUniquenessConstraint.Message");
			}
		}
		/// <summary>
		/// The name of the transaction that auto-fixes implied and duplicate internal constraints.
		/// </summary>
		public static string RemoveInternalConstraintsTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "FactType.RemoveInternalUniquenessConstraints.TransactionName");
			}
		}
		/// <summary>
		/// The transaction name used by AutoLayout command. The text appears in the undo dropdown in the VS IDE.
		/// </summary>
		public static string AutoLayoutTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "AutoLayout.TransactionName");
			}
		}
	}
	#endregion // ResourceStrings class
}
