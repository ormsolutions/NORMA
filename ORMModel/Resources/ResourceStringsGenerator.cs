
// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.ORM
{
	#region ResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class ResourceStrings
	{
		/// <summary>The tab name for default toolbox</summary>
		public static string ToolboxDefaultTabName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ShapeModel, "Toolbox.DefaultTabName");
			}
		}
		/// <summary>The display name used for an ObjectType when IsValueType is false</summary>
		public static string ValueType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueType");
			}
		}
		/// <summary>The display name used for an ObjectType when IsValueType is true</summary>
		public static string EntityType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.EntityType");
			}
		}
		/// <summary>The display name used for a simple FactType</summary>
		public static string FactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.FactType");
			}
		}
		/// <summary>The display name used for a SubtypeFact</summary>
		public static string SubtypeFact
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.SubtypeFact");
			}
		}
		/// <summary>The display name used for an objectified (nested) FactType</summary>
		public static string ObjectifiedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ObjectifiedFactType");
			}
		}
		/// <summary>The display name used for an internal uniqueness constraint</summary>
		public static string InternalUniquenessConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.InternalUniquenessConstraint");
			}
		}
		/// <summary>The display name used for an external uniqueness constraint</summary>
		public static string ExternalUniquenessConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint");
			}
		}
		/// <summary>The display name used for a disjunctive mandatory constraint</summary>
		public static string DisjunctiveMandatoryConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.MandatoryConstraint");
			}
		}
		/// <summary>The display name used for an exclusion constraint</summary>
		public static string ExclusionConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ExclusionConstraint");
			}
		}
		/// <summary>The display name used for a simple mandatory constraint</summary>
		public static string SimpleMandatoryConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.SimpleMandatoryConstraint");
			}
		}
		/// <summary>The display name used for a ReadingType</summary>
		public static string ReadingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Reading");
			}
		}
		/// <summary>The name displayed to represent null in the role player picker</summary>
		public static string RolePlayerPickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.RolePlayerPicker.NullItemText");
			}
		}
		/// <summary>The name displayed to represent null in the nested fact type picker</summary>
		public static string NestedFactTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker.NullItemText");
			}
		}
		/// <summary>The name displayed to represent null in the nesting type picker</summary>
		public static string NestingTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.Editors.NestingTypePicker.NullItemText");
			}
		}
		/// <summary>The base name used to create a name for a new EntityType. This is a format string</summary>
		public static string EntityTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.EntityType.DefaultNamePattern");
			}
		}
		/// <summary>The descriptive text for a PortableDataType of Unspecified.</summary>
		public static string PortableDataTypeUnspecified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Unspecified.Text");
			}
		}
		/// <summary>A fixed length text data type</summary>
		public static string PortableDataTypeTextFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.FixedLength.Text");
			}
		}
		/// <summary>A variable length text data type</summary>
		public static string PortableDataTypeTextVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.VariableLength.Text");
			}
		}
		/// <summary>A large length text data type</summary>
		public static string PortableDataTypeTextLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Text.LargeLength.Text");
			}
		}
		/// <summary>A signed integer numeric data type</summary>
		public static string PortableDataTypeNumericSignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.SignedInteger.Text");
			}
		}
		/// <summary>An unsigned integer numeric data type</summary>
		public static string PortableDataTypeNumericUnsignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.UnsignedInteger.Text");
			}
		}
		/// <summary>An auto counter numeric data type</summary>
		public static string PortableDataTypeNumericAutoCounter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.AutoCounter.Text");
			}
		}
		/// <summary>A floating point numeric data type</summary>
		public static string PortableDataTypeNumericFloatingPoint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.FloatingPoint.Text");
			}
		}
		/// <summary>A decimal numeric data type</summary>
		public static string PortableDataTypeNumericDecimal
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.Decimal.Text");
			}
		}
		/// <summary>A money numeric data type</summary>
		public static string PortableDataTypeNumericMoney
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Numeric.Money.Text");
			}
		}
		/// <summary>A fixed length raw data data type</summary>
		public static string PortableDataTypeRawDataFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.FixedLength.Text");
			}
		}
		/// <summary>A variable length raw data data type</summary>
		public static string PortableDataTypeRawDataVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.VariableLength.Text");
			}
		}
		/// <summary>A large length raw data data type</summary>
		public static string PortableDataTypeRawDataLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.LargeLength.Text");
			}
		}
		/// <summary>A picture raw data data type</summary>
		public static string PortableDataTypeRawDataPicture
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.Picture.Text");
			}
		}
		/// <summary>An OLE object raw data data type</summary>
		public static string PortableDataTypeRawDataOleObject
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.RawData.OleObject.Text");
			}
		}
		/// <summary>An auto timestamp temporal data type</summary>
		public static string PortableDataTypeTemporalAutoTimestamp
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.AutoTimestamp.Text");
			}
		}
		/// <summary>An time temporal data type</summary>
		public static string PortableDataTypeTemporalTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.Time.Text");
			}
		}
		/// <summary>An date temporal data type</summary>
		public static string PortableDataTypeTemporalDate
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.Date.Text");
			}
		}
		/// <summary>An date and time temporal data type</summary>
		public static string PortableDataTypeTemporalDateAndTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Temporal.DateAndTime.Text");
			}
		}
		/// <summary>A true or false logical data type</summary>
		public static string PortableDataTypeLogicalTrueOrFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Logical.TrueOrFalse.Text");
			}
		}
		/// <summary>A yes or no logical data type</summary>
		public static string PortableDataTypeLogicalYesOrNo
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Logical.YesOrNo.Text");
			}
		}
		/// <summary>A row id data type (can not be classified in any of the groups above)</summary>
		public static string PortableDataTypeOtherRowId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Other.RowId.Text");
			}
		}
		/// <summary>An object id data type (can not be classified in any of the groups above)</summary>
		public static string PortableDataTypeOtherObjectId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.DataType.PortableDataType.Other.ObjectId.Text");
			}
		}
		/// <summary>Used to automatically turn a value type into an entity type with a reference mode when IsValueType is set to false and the ValueType has downstream value roles. Replacement field {0} is the name of the exisiting type and field {1} is used to insert a number to ensure that the name is unique. {0}Values{1} is the default format.</summary>
		public static string ValueTypeAutoCreateReferenceModeNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueType.AutoCreateReferenceModeNamePattern");
			}
		}
		/// <summary>The base name used to create a name for a new ValueType. This is a format string with {0} being the placeholder for the number placement.</summary>
		public static string ValueTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueType.DefaultNamePattern");
			}
		}
		/// <summary>The base name used to create a name for a new FactType. This is a format string with {0} being the placeholder for the number placement.</summary>
		public static string FactTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.FactType.DefaultNamePattern");
			}
		}
		/// <summary>The base name used to create a name for a new SubtypeFact. This is a format string with {0} being the placeholder for the number placement.</summary>
		public static string SubtypeFactDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.DefaultNamePattern");
			}
		}
		/// <summary>The inverse reading for the predicate created by creating a sub type relationship.</summary>
		public static string SubtypeFactPredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.PredicateInverseReading");
			}
		}
		/// <summary>The reading for the forward predicate created by creating a subtype relationship.</summary>
		public static string SubtypeFactPredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.PredicateReading");
			}
		}
		/// <summary>The inverse reading for the predicate created implicitly via objectification. There is no attempt made to keep the predicate readings unique in ring situations.We allow the model error to populate instead of generating an articial unique reading.</summary>
		public static string ImpliedFactTypePredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImpliedFactType.PredicateInverseReading");
			}
		}
		/// <summary>The reading for the predicate created implicitly via objectification. There is no attempt made to keep the predicate readings unique in ring situations.We allow the model error to populate instead of generating an articial unique reading.</summary>
		public static string ImpliedFactTypePredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImpliedFactType.PredicateReading");
			}
		}
		/// <summary>String for generating a component name for a subtype. The {0} replacement field is used for the subtype component name; {1} for the supertype.</summary>
		public static string SubtypeFactComponentNameFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.ComponentNameFormat");
			}
		}
		/// <summary>The name given to the transaction used when adding an internal uniqueness constraint to correct the error of lacking one.</summary>
		public static string AddInternalConstraintTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "AddInternalUniquenessConstraint.TransactionName");
			}
		}
		/// <summary>The name given to the transaction used when objectifying a fact type.</summary>
		public static string ObjectifyFactTypeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifyFactType.TransactionName");
			}
		}
		/// <summary>The transaction name used by ExclusiveOrCoupler command.</summary>
		public static string ExclusiveOrCouplerTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExclusiveOrCoupler.TransactionName");
			}
		}
		/// <summary>The transaction name used by ExclusiveOrDecoupler command.</summary>
		public static string ExclusiveOrDecouplerTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExclusiveOrDecoupler.TransactionName");
			}
		}
		/// <summary>The name given to the transaction used when adding a shape element for an existing object.</summary>
		public static string DropShapeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DropShape.TransactionName");
			}
		}
		/// <summary>The instructions shown when creating an external constraint</summary>
		public static string ExternalConstraintConnectActionInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.Instructions");
			}
		}
		/// <summary>The transaction name used by the external constraint connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ExternalConstraintConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.TransactionName");
			}
		}
		/// <summary>The string used to represent a greater than symbol on a frequency constraint.</summary>
		public static string FrequencyConstraintMinimumFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.Minimum.FormatString");
			}
		}
		/// <summary>The format to be used in frequency constraints with a minimum value of 1 (eg: Maximum of {0}).</summary>
		public static string FrequencyConstraintMinimumOneFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.MinimumOne.FormatString");
			}
		}
		/// <summary>The string used to represent 'between' on a frequency constraint (eg: 'Between 1 and 5' would be represented as '1[this string]5'.</summary>
		public static string FrequencyConstraintBetweenFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.Between.FormatString");
			}
		}
		/// <summary>The instructions shown when creating an internal uniqueness constraint</summary>
		public static string InternalUniquenessConstraintConnectActionInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InternalUniquenessConstraintConnectAction.Instructions");
			}
		}
		/// <summary>The transaction name used by the internal uniqueness constraint connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string InternalUniquenessConstraintConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InternalUniquenessConstraintConnectAction.TransactionName");
			}
		}
		/// <summary>The transaction name used by the InsertRoleBefore/InsertRoleAfter commands. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string InsertRoleTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InsertRole.TransactionName");
			}
		}
		/// <summary>The transaction name used by the role connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string RoleConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "RoleConnectAction.TransactionName");
			}
		}
		/// <summary>The transaction name used by the subtype connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string SubtypeConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SubtypeConnectAction.TransactionName");
			}
		}
		/// <summary>The transaction name used by the model note connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ModelNoteConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ModelNoteConnectAction.TransactionName");
			}
		}
		/// <summary>The transaction name used when an options page change modifies diagram layout and connections. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string OptionsPageChangeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "OptionsPageChange.TransactionName");
			}
		}
		/// <summary>Value displayed for any modified state of the Custom Verbalization Snippets option.</summary>
		public static string OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueCustom
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "OptionsPage.Property.CustomVerbalizationSnippets.DisplayValue.Custom");
			}
		}
		/// <summary>Value displayed for the default state of the Custom Verbalization Snippets option.</summary>
		public static string OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueDefault
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "OptionsPage.Property.CustomVerbalizationSnippets.DisplayValue.Default");
			}
		}
		/// <summary>Language format string for the Custom Verbalization Snippets option dropdown.</summary>
		public static string OptionsPagePropertyCustomVerbalizationSnippetsDisplayValueLanguageFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "OptionsPage.Property.CustomVerbalizationSnippets.DisplayValue.LanguageFormat");
			}
		}
		/// <summary>The transaction name used for deleting a role sequence from a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string DeleteRoleSequenceTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DeleteRoleSequence.TransactionName");
			}
		}
		/// <summary>The transaction name used for reversing the role order in a fact type shape. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ReverseRoleOrderTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReverseRoleOrder.TransactionName");
			}
		}
		/// <summary>The transaction name used for moving a role order in a fact type. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string MoveRoleOrderTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MoveRoleOrder.TransactionName");
			}
		}
		/// <summary>The transaction name used for moving a role sequence down in a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string MoveRoleSequenceDownTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MoveRoleSequenceDown.TransactionName");
			}
		}
		/// <summary>The transaction name used for moving a role sequence up in a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE. </summary>
		public static string MoveRoleSequenceUpTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MoveRoleSequenceUp.TransactionName");
			}
		}
		/// <summary>The transaction name used for changes made in response to committing a modified line in the fact editor. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string InterpretFactEditorLineTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "InterpretFactEditorLine.TransactionName");
			}
		}
		/// <summary>The window title for the model browser tool window</summary>
		public static string ModelBrowserWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ORMModelBrowser.WindowTitle");
			}
		}
		/// <summary>Exception message when an attempt is made to modify roles, constraints, and role players on elements implied by an Objectification relationship.</summary>
		public static string ModelExceptionObjectificationImpliedElementModified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.DirectModificationOfImpliedElement");
			}
		}
		/// <summary>Exception message when an attempt is made to objectify a fact that is implied by another objectification.</summary>
		public static string ModelExceptionObjectificationImpliedFactObjectified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedFactTypesCannotBeObjectified");
			}
		}
		/// <summary>Exception message when an attempt is made to change an explicit Objectification to implied when it does not match the implied objectification pattern or is not independent or plays a Role in a non-implied FactType.</summary>
		public static string ModelExceptionObjectificationImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact");
			}
		}
		/// <summary>The error message to return when an attempt is made to change the IsPrimary property of a reading to false.</summary>
		public static string ModelExceptionReadingIsPrimaryToFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Reading.IsPrimary.ReadOnlyWhenFalse");
			}
		}
		/// <summary>The error message that is returned when attempting to add a new reading to a fact and the text is not valid.  It needs to have a number of place holders equal to the number of roles in the fact and they need to have their positions identified by number using the replacement syntax of String.Format. For example: "{0} has {1}"</summary>
		public static string ModelExceptionFactAddReadingInvalidReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Fact.AddReading.InvalidReadingText");
			}
		}
		/// <summary>Model validation error text when a constraint intersects a second constraint where the second constraints roles are a subset of the constraint roles.</summary>
		public static string ModelErrorConstraintImplication
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ImplicationError.Text");
			}
		}
		/// <summary>Model validation error text when an equality constraint or subset constraint is put on roles that are mandatory.</summary>
		public static string ModelErrorConstraintImplicationEqualityOrSubsetMandatory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.EqualityOrSubsetImpliedByMandatoryError.Text");
			}
		}
		/// <summary>Model validation error text when a mandatory constraint is put on the subset role of a subset constraint relationship.</summary>
		public static string ModelErrorNotWellModeledSubsetAndMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.NotWellModeledSubsetAndMandatoryError.Text");
			}
		}
		/// <summary>Model validation error text when a constraint intersects a second constraint where the second constraints roles are a subset of the constraint roles and are in a state of contradiction.</summary>
		public static string ModelErrorConstraintContradiction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ContradictionError.Text");
			}
		}
		/// <summary>Model validation error shown when too few role sequences are defined for a constraint. This is a frequent occurrence as external constraints are easily created in this state.</summary>
		public static string ModelErrorConstraintHasTooFewRoleSequencesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.TooFewRoleSequences.Text");
			}
		}
		/// <summary>Model validation error text used when a frequency constraint contradicts an internal uniqueness constraint</summary>
		public static string FrequencyConstraintContradictsInternalUniquenessConstraintText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintContradictsInternalUniquenessConstraintError.Text");
			}
		}
		/// <summary>Model validation error shown when too many role sequences are defined for a constraint. This is an infrequent occurrence which should not be attainable via the UI</summary>
		public static string ModelErrorConstraintHasTooManyRoleSequencesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.TooManyRoleSequences.Text");
			}
		}
		/// <summary>Model validation error text when role sequences in a multi column constraint have different role counts (arity).</summary>
		public static string ModelErrorConstraintExternalConstraintArityMismatch
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ExternalConstraintArityMismatch.Text");
			}
		}
		/// <summary>Model validation error shown when multiple constraints have the same name. This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateConstraintNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateConstraintNames.Text");
			}
		}
		/// <summary>Model validation error shown when multiple object types have the same name. This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateObjectTypeNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateObjectTypeNames.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a uniqueness constraint and the role is named.</summary>
		public static string ModelErrorModelHasPopulationUniquenessErrorWithNamedRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationUniquenessError.Role.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a uniqueness constraint and the role is unnamed.</summary>
		public static string ModelErrorModelHasPopulationUniquenessErrorWithUnnamedRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationUniquenessError.FactType.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a mandatory constraint.</summary>
		public static string ModelErrorModelHasPopulationMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationMandatoryError.Text");
			}
		}
		/// <summary>Role data type does not match max inclusion</summary>
		public static string ModelErrorRoleValueRangeMaxValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MaxValueMismatchError.Role.Message");
			}
		}
		/// <summary>Role data type does not match min inclusion</summary>
		public static string ModelErrorRoleValueRangeMinValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MinValueMismatchError.Role.Message");
			}
		}
		/// <summary>Data type does not match max inclusion</summary>
		public static string ModelErrorValueRangeMaxValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MaxValueMismatchError.ValueType.Message");
			}
		}
		/// <summary>Data type does not match min inclusion</summary>
		public static string ModelErrorValueRangeMinValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MinValueMismatchError.ValueType.Message");
			}
		}
		/// <summary>ValueRangeOverlapError text for a ValueTypeConstraint. Replace valuetype, model.</summary>
		public static string ModelErrorValueTypeValueRangeOverlapError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueConstraint.ValueRangeOverlapError.ValueType.Message");
			}
		}
		/// <summary>ValueRangeOverlapError text for a RoleValueConstraint. Replace model,fact,role number.</summary>
		public static string ModelErrorRoleValueRangeOverlapError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueConstraint.ValueRangeOverlapError.Role.Message");
			}
		}
		/// <summary>Exception message when a name change in the editor attempts to introduce a duplicate name into the model.</summary>
		public static string ModelExceptionNameAlreadyUsedByModel
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Model.DuplicateName.Text");
			}
		}
		/// <summary>Exception message when an attempt is made to make an object type both a value type and an objectified fact type.</summary>
		public static string ModelExceptionEnforceValueTypeNotNestingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectType.EnforceValueTypeNotNestingType");
			}
		}
		/// <summary>Exception message when an attempt is made to use the same type as both a role player and the nesting type of a fact type.</summary>
		public static string ModelExceptionEnforceRolePlayerNotNestingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactType.EnforceRolePlayerNotNestingType");
			}
		}
		/// <summary>Exception message when an attempt is made to set\ both a primary identifier and a value type on the same object type.</summary>
		public static string ModelExceptionEnforcePreferredIdentifierForEntityType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectType.EnforcePreferredIdentifierForEntityType");
			}
		}
		/// <summary>Exception message when an attempt is made to directly edit roles on an ExclusionConstraint that participates in an ExclusiveOrCoupler relationship.</summary>
		public static string ModelExceptionExclusiveOrConstraintCouplerDirectExclusionConstraintEdit
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ExclusiveOrConstraintCoupler.DirectExclusionConstraintEdit");
			}
		}
		/// <summary>Exception message when an attempt is made to set the IsMandatory property on an unattached role. IsMandatory creates an internal constraint, which is owned by an FactType, so cannot be realized if the parent fact is unknown.</summary>
		public static string ModelExceptionIsMandatoryRequiresAttachedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Role.IsMandatoryRequiresAttachedFactType");
			}
		}
		/// <summary>Exception message when an attempt is made to set a uniqueness constraint as a preferred identifier when the preconditions are not met.</summary>
		public static string ModelExceptionInvalidPreferredIdentifierPreConditions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.UniquenessConstraint.InvalidPreferredIdentifierPreConditions");
			}
		}
		/// <summary>Exception message when an attempt is made to attached facts from an external model to a constraint.</summary>
		public static string ModelExceptionConstraintEnforceNoForeignFacts
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Constraint.EnforceNoForeignFacts");
			}
		}
		/// <summary>Exception message when an attempt is made to attached more than one fact to an internal constraint.</summary>
		public static string ModelExceptionConstraintEnforceSingleFactForInternalConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Constraint.EnforceSingleFactForInternalConstraint");
			}
		}
		/// <summary>Exception message when an attempt is made to form and ExclusiveOrConstraintCoupler relationship between incompatible MandatoryConstraint and ExclusionConstraint elements.</summary>
		public static string ModelExceptionExclusiveOrConstraintCouplerInconsistentConstraintRoles
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ExclusiveOrConstraintCoupler.InconsistentConstraintRoles");
			}
		}
		/// <summary>The description for the default verbalization snippets for the core model.</summary>
		public static string CoreVerbalizationSnippetsDefaultDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Verbalization.SnippetsDefaultDescription");
			}
		}
		/// <summary>The description for the verbalization snippets associated with the core model.</summary>
		public static string CoreVerbalizationSnippetsTypeDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Verbalization.SnippetsTypeDescription");
			}
		}
		/// <summary>Exception message when an attempt is made to add roles from different fact types to a role sequence owned by a fact type instance.</summary>
		public static string ModelExceptionFactTypeInstanceInconsistentRoleOwners
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactTypeInstance.InconsistentRoleOwners");
			}
		}
		/// <summary>Exception message when an attempt is made to hook up role instances to an entity type where the roles are not in the entity type's preferred identifier collection.</summary>
		public static string ModelExceptionEntityTypeInstanceInvalidRolesPreferredIdentifier
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeInstance.InvalidRolesPreferredIdentifier");
			}
		}
		/// <summary>Exception message when an attempt is made to hook up a ValueType to an EntityTypeInstance.</summary>
		public static string ModelExceptionEntityTypeInstanceInvalidEntityTypeParent
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeInstance.InvalidEntityTypeParent");
			}
		}
		/// <summary>Exception message when an attempt is made to hook up an EntityType to a ValueTypeInstance.</summary>
		public static string ModelExceptionValueTypeInstanceInvalidValueTypeParent
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ValueTypeInstance.InvalidValueTypeParent");
			}
		}
		/// <summary>This text appears in the edit menu when fact types are selected in the diagram.</summary>
		public static string CommandDeleteFactTypeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteFactType.Text");
			}
		}
		/// <summary>This text appears in the edit menu when object types are selected in the diagram.</summary>
		public static string CommandDeleteObjectTypeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteObjectType.Text");
			}
		}
		/// <summary>This text appears in the edit menu when constraint types are selected in the diagram.</summary>
		public static string CommandDeleteConstraintText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteConstraint.Text");
			}
		}
		/// <summary>This text appears in the edit menu when multiple elements of different types are selected in the diagram.</summary>
		public static string CommandDeleteMultipleElementsText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteMultipleElements.Text");
			}
		}
		/// <summary>This text appears in the edit menu when multiple shapes of different types are selected in the diagram.</summary>
		public static string CommandDeleteMultipleShapesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteMultipleShapes.Text");
			}
		}
		/// <summary>This text appears in the edit menu when fact types are selected in the diagram.</summary>
		public static string CommandDeleteFactTypeShapeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteFactTypeShape.Text");
			}
		}
		/// <summary>This text appears in the edit menu when object types are selected in the diagram.</summary>
		public static string CommandDeleteObjectTypeShapeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteObjectTypeShape.Text");
			}
		}
		/// <summary>This text appears in the edit menu when constraint types are selected in the diagram.</summary>
		public static string CommandDeleteConstraintShapeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteConstraintShape.Text");
			}
		}
		/// <summary>This text appears in the edit menu when model notes are selected in the diagram.</summary>
		public static string CommandDeleteModelNoteText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteModelNote.Text");
			}
		}
		/// <summary>This text appears in the edit menu when references to model notes are selected in the diagram.</summary>
		public static string CommandDeleteModelNoteReferenceText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteModelNoteReference.Text");
			}
		}
		/// <summary>This text appears in the edit menu when model notes are selected in the diagram.</summary>
		public static string CommandDeleteModelNoteShapeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteModelNoteShape.Text");
			}
		}
		/// <summary>This text appears in the edit menu when multiple elements of different types are selected in the diagram.</summary>
		public static string CommandDeleteMultipleShapeText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteMultipleShape.Text");
			}
		}
		/// <summary>This text appears in the edit menu when roles are selected in the diagram.</summary>
		public static string CommandDeleteRoleText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteRole.Text");
			}
		}
		/// <summary>This text appears on the undo/redo menu when a reading is deleted.</summary>
		public static string CommandDeleteReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteReading.Text");
			}
		}
		/// <summary>This text appears on the undo/redo menu when a reading is edited.</summary>
		public static string CommandEditReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.EditReading.Text");
			}
		}
		/// <summary>This text appears on the move role left/right when the fact type is binary.</summary>
		public static string CommandSwapRoleOrderText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.SwapRoleOrder.Text");
			}
		}
		/// <summary>Text that appears in the headers of the reading editor tool window for the reading text Text that appears in the headers of the reading editor tool window for the reading text</summary>
		public static string ModelReadingEditorListColumnHeaderReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ListColumnHeader.ReadingText");
			}
		}
		/// <summary>Text that appears in the header of the IsPrimary column in the reading editor tool window.</summary>
		public static string ModelReadingEditorListColumnHeaderIsPrimary
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ListColumnHeader.IsPrimary");
			}
		}
		/// <summary>The text to place in the node of the tree in the readings editor that shows all readings for the fact.</summary>
		public static string ModelReadingEditorAllReadingsNodeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.AllReadingsNodeName");
			}
		}
		/// <summary>The text to display in the readings editor when a role has no roleplayer.</summary>
		public static string ModelReadingEditorMissingRolePlayerText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MissingRolePlayerText");
			}
		}
		/// <summary>The text that will be used for the new reading item in the readings list of the reading editor tool window.</summary>
		public static string ModelReadingEditorNewItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.NewItemText");
			}
		}
		/// <summary>Text used to describe the transaction created when creating a new reading.</summary>
		public static string ModelReadingEditorNewReadingTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.NewReadingTransactionText");
			}
		}
		/// <summary>Text used to label the transaction created when changing reading text through the editor</summary>
		public static string ModelReadingEditorChangeReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ChangeReadingText");
			}
		}
		/// <summary>Text used to label the transaction created when a reading order is moved through the reading editor</summary>
		public static string ModelReadingEditorMoveReadingOrder
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MoveReadingOrder");
			}
		}
		/// <summary>Text used to label the transaction created when a reading is moved through the reading editor</summary>
		public static string ModelReadingEditorMoveReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MoveReading");
			}
		}
		/// <summary>ToolTip text that appears on the reading list for the IsPrimary status column.</summary>
		public static string ModelReadingEditorIsPrimaryToolTip
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.IsPrimaryToolTip");
			}
		}
		/// <summary>Text used to label the transaction created when which reading is primary is changed in the editor.</summary>
		public static string ModelReadingEditorChangePrimaryReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ChangePrimaryReadingText");
			}
		}
		/// <summary>Text to place in the title of the reading editor tool window.</summary>
		public static string ModelReadingEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.WindowTitle");
			}
		}
		/// <summary>Text used as the header row for the implied readings in the reading editor for a role selection on an objectified fact type.</summary>
		public static string ModelReadingEditorImpliedFactTypeReadingsText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ImpliedFactTypeReadingsText");
			}
		}
		/// <summary>Text used as the header row for the primary readings in the reading editor for a role selection on an objectified fact type.</summary>
		public static string ModelReadingEditorPrimaryFactTypeReadingsText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.PrimaryFactTypeReadingsText");
			}
		}
		/// <summary>Text to place in the title bar of the notes tool window.</summary>
		public static string ModelNotesWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelNotesWindow.WindowTitle");
			}
		}
		/// <summary>Text to place in the title bar of the context tool window.</summary>
		public static string ModelContextWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelContextWindow.WindowTitle");
			}
		}
		/// <summary>Text that separates a root type's name from notes if multiple notes are displayed in the ORM notes window. Replacement field 0 is the element name, 1 is the note text.</summary>
		public static string ModelNotesWindowRootTypeNameNotesSeparatorFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelNotesWindow.RootTypeNameNotesSeparatorFormatString");
			}
		}
		/// <summary>Text that will display in the middle of the tool window when the document view currently has an unsuppoted collection.</summary>
		public static string ModelReadingEditorUnsupportedSelectionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.UnsupportedSelectionText");
			}
		}
		/// <summary>Text used to describe the transaction when creating an instance.  Replacement field 0 is the column name of the instance being created.</summary>
		public static string ModelSamplePopulationEditorNewInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.NewInstanceTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when editing an instance.  Replacement field 0 is the column name of the instance being edited.</summary>
		public static string ModelSamplePopulationEditorEditInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.EditInstanceTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when removing an instance.  Replacement field 0 is the column name of the instance being removed.</summary>
		public static string ModelSamplePopulationEditorRemoveInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RemoveInstanceTransactionText");
			}
		}
		/// <summary>Text to place in the title bar of the sample population window.</summary>
		public static string ModelSamplePopulationEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.WindowTitle");
			}
		}
		/// <summary>Text to place when selection is null in the sample population window.</summary>
		public static string ModelSamplePopulationEditorNullSelection
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.NullSelection");
			}
		}
		/// <summary>The official name of the package used in the About dialog</summary>
		public static string PackageOfficialName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Package.OfficialName");
			}
		}
		/// <summary>The description of the package used in the About dialog</summary>
		public static string PackageProductDetails
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Package.ProductDetails");
			}
		}
		/// <summary>returns the Text that names the reference mode.</summary>
		public static string ModelReferenceModeEditorReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeName");
			}
		}
		/// <summary>returns the Text that names the Entity Type.</summary>
		public static string ModelReferenceModeEditorEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EntityTypeName");
			}
		}
		/// <summary>returns the Text used to name the Reference Mode Kind column</summary>
		public static string ModelReferenceModeEditorReferenceModeKindHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeKindHeader");
			}
		}
		/// <summary>returns the Text used to name the transaction the changes the name of a custom reference mode</summary>
		public static string ModelReferenceModeEditorChangeNameTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeNameTransaction");
			}
		}
		/// <summary>returns the Text used to name the Custom Reference Modes column</summary>
		public static string ModelReferenceModeEditorCustomReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.CustomReferenceModesHeader");
			}
		}
		/// <summary>returns the Text used to name the Intrinsic Reference Modes column</summary>
		public static string ModelReferenceModeEditorIntrinsicReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.IntrinsicReferenceModesHeader");
			}
		}
		/// <summary>returns the abbreviated form of the entity type name</summary>
		public static string ModelReferenceModeEditorAbbreviatedEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedEntityTypeName");
			}
		}
		/// <summary>return the abbreviated form of the reference mode name</summary>
		public static string ModelReferenceModeEditorAbbreviatedReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedReferenceModeName");
			}
		}
		/// <summary>returns the text used to name the transaction that adds a custom reference mode</summary>
		public static string ModelReferenceModeEditorAddCustomReferenceModeTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddCustomReferenceModeTransaction");
			}
		}
		/// <summary>returns the text to display the add a new row to the custom reference modes branch</summary>
		public static string ModelReferenceModeEditorAddNewRowText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddNewRowText");
			}
		}
		/// <summary>returns the text used to name the transaction that changes the format string. </summary>
		public static string ModelReferenceModeEditorChangeFormatStringTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeFormatStringTransaction");
			}
		}
		/// <summary>returns The text that displays the title of the editor window.</summary>
		public static string ModelReferenceModeEditorEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EditorWindowTitle");
			}
		}
		/// <summary>returns Exception message when the unique format string rule is violated for reference modes</summary>
		public static string ModelExceptionReferenceModeEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceUniqueFormatString");
			}
		}
		/// <summary>returns Exception message when the unique format string rule is violated for reference mode kinds</summary>
		public static string ModelExceptionReferenceModeKindEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceModeKind.EnforceUniqueFormatString");
			}
		}
		/// <summary>returns Exception messege when atttempt is made to change the kind of an intrinsic reference mode</summary>
		public static string ModelExceptionReferenceModeIntrinsicRefModesDontChange
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.IntrinsicRefModesDontChange");
			}
		}
		/// <summary>returns Exception messege when attempt is made to remove reference mode kind.</summary>
		public static string ModelExceptionReferenceModeReferenceModesKindNotEmpty
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.ReferenceModesKindNotEmpty");
			}
		}
		/// <summary>returns Text used to name the transaction that changes the referencemode Kind.</summary>
		public static string ModelReferenceModeEditorChangeReferenceModeKindTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeReferenceModeKindTransaction");
			}
		}
		/// <summary>returns The reading for the predicate created by setting the ref mode.</summary>
		public static string ReferenceModePredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateReading");
			}
		}
		/// <summary>returns The inverse reading for the predicate created by setting the ref mode.</summary>
		public static string ReferenceModePredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateInverseReading");
			}
		}
		/// <summary>returns The Column header text for the Format String column.</summary>
		public static string ModelReferenceModeEditorFormatStringColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.FormatStringColumn");
			}
		}
		/// <summary>returns The Column header text for the Kind column.</summary>
		public static string ModelReferenceModeEditorKindColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.KindColumn");
			}
		}
		/// <summary>returns The Column header text for the name column.</summary>
		public static string ModelReferenceModeEditorNameColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.NameColumn");
			}
		}
		/// <summary>The caption of the Fact Editor Tool Window</summary>
		public static string FactEditorToolWindowCaption
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FactEditorToolWindow.Caption");
			}
		}
		/// <summary>Exception message output when multiple reference modes are found with the same name</summary>
		public static string ModelExceptionReferenceModeAmbiguousName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.AmbiguousName");
			}
		}
		/// <summary>Exception message when an attempt is made to modify the roles collection or internal constraints of a SubtypeFact.</summary>
		public static string ModelExceptionSubtypeConstraintAndRolePatternFixed
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.ConstraintAndRolePatternFixed");
			}
		}
		/// <summary>Exception message when an attempt is made to attach SubtypeMetaRole or SuperTypeMetaRole objects to a FactType that is not a SubtypeFact.</summary>
		public static string ModelExceptionSubtypeFactMustBeParentOfMetaRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.MetaRolesMustBeUsedOnSubtypeFact");
			}
		}
		/// <summary>Exception message when an attempt is made to mix EntityType and ValueType role players on a SubtypeFact.</summary>
		public static string ModelExceptionSubtypeRolePlayerTypesCannotBeMixed
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.RolePlayersTypesNotMixed");
			}
		}
		/// <summary>Exception message when an attempt is made to nest a SubtypeFact.</summary>
		public static string ModelExceptionSubtypeFactNotNested
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.NotNested");
			}
		}
		/// <summary>Exception message when an attempt is made to change the SubtypeFact.IsPrimary property to false.</summary>
		public static string ModelExceptionSubtypeFactPrimaryMustBeTrue
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.PrimaryMustBeTrue");
			}
		}
		/// <summary>Exception message when an attempt is made to add a subtype relationship where the subtype is a direct or indirect subtype of the supertype.</summary>
		public static string ModelExceptionSubtypeFactCycle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.Cycle");
			}
		}
		/// <summary>returns the format string for the display text for reference mode picker</summary>
		public static string ModelReferenceModePickerFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModePicker.FormatString");
			}
		}
		/// <summary>Text displayed in the text of the TooFewRolesError. {0}=fact name,{1}=model name,{2}=reading text</summary>
		public static string ModelErrorReadingTooFewRolesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Reading.TooFewRoles.Message");
			}
		}
		/// <summary>Text displayed in the text of the TooManyRolesError. {0}=fact name,{1}=model name,{2}=reading text</summary>
		public static string ModelErrorReadingTooManyRolesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Reading.TooManyRoles.Message");
			}
		}
		/// <summary>Text used to replace a role place holder when the role is deleted</summary>
		public static string ModelReadingRoleDeletedRoleText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Model.Reading.RoleDeletedText");
			}
		}
		/// <summary>Text displayed in the text of the FactTypeRequiresReadingError</summary>
		public static string ModelErrorFactTypeRequiresInternalUniquenessConstraintMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresInternalUniquenessConstraint.Message");
			}
		}
		/// <summary>Text displayed in the text of the FactTypeRequiresInternalUniquenessContraintError</summary>
		public static string ModelErrorFactTypeRequiresReadingMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresReading.Message");
			}
		}
		/// <summary>Text displayed for an unspecified data type.</summary>
		public static string ModelErrorValueTypeDataTypeNotSpecifiedMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueType.DataTypeNotSpecified.Message");
			}
		}
		/// <summary>Text displayed when there aren't enough entity type role instances to completely fill a single entity population row.</summary>
		public static string ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.EntityTypeInstance.TooFewEntityTypeRoleInstances.Message");
			}
		}
		/// <summary>Text displayed when there aren't enough fact type role instances to completely fill a single fact type population row.</summary>
		public static string ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactTypeInstance.TooFewFactTypeRoleInstances.Message");
			}
		}
		/// <summary>Text displayed when the data type of the sample data doesn't match it's set data type.</summary>
		public static string ModelErrorValueTypeInstanceCompatibleValueTypeInstanceValueMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueTypeInstance.CompatibleValueTypeInstanceValue.Message");
			}
		}
		/// <summary>Text used as the class name, displayed in the properties window.</summary>
		public static string ExclusiveOrConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ExclusiveOrConstraint.ClassName");
			}
		}
		/// <summary>Text used as the name for Name property of the coupled mandatory constraint, displayed in the properties window.</summary>
		public static string ExclusiveOrConstraintMandatoryConstraintNameDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ExclusiveOrConstraint.MandatoryConstraintName.DisplayName");
			}
		}
		/// <summary>Text used as the name for Name property of the coupled exclusion constraint, displayed in the properties window.</summary>
		public static string ExclusiveOrConstraintExclusionConstraintNameDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ExclusiveOrConstraint.ExclusionConstraintName.DisplayName");
			}
		}
		/// <summary>Pattern showing left- and right-string to use for containing a value range definition.</summary>
		public static string ValueConstraintDefinitionContainerPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DefinitionContainerPattern");
			}
		}
		/// <summary>String used to delimit sets of value ranges in a definition.</summary>
		public static string ValueConstraintRangeDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RangeDelimiter");
			}
		}
		/// <summary>String used to delimit the min- and max-values of a value range.</summary>
		public static string ValueConstraintValueDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueDelimiter");
			}
		}
		/// <summary>Pattern showing left- and right-string to use for containing a value range as a string.</summary>
		public static string ValueConstraintStringContainerPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.StringContainerPattern");
			}
		}
		/// <summary>Pattern showing left- and right-string to use to indicate the min- and max-values are open (i.e. the value itself is not a member of the range).</summary>
		public static string ValueConstraintOpenInclusionContainer
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.OpenInclusionPattern");
			}
		}
		/// <summary>Pattern showing left- and right-string to use to indicate the min- and max-values are closed (i.e. the value itself is a member of the range).</summary>
		public static string ValueConstraintClosedInclusionContainer
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ClosedInclusionPattern");
			}
		}
		/// <summary>The string used to display that an object is independent.</summary>
		public static string ObjectTypeShapeIndependentFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectTypeShape.IndependentFormatString");
			}
		}
		/// <summary>The string used to display a reference mode.</summary>
		public static string ObjectTypeShapeReferenceModeFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectTypeShape.ReferenceModeFormatString");
			}
		}
		/// <summary>The string used to display an objectified type name.</summary>
		public static string ObjectifiedFactTypeNameShapeStandardFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.StandardFormatString");
			}
		}
		/// <summary>The string used to display an objectified type name for an independent object.</summary>
		public static string ObjectifiedFactTypeNameShapeIndependentFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.IndependentFormatString");
			}
		}
		/// <summary>The string used to display an objectified type name for an independent object with a reference mode.</summary>
		public static string ObjectifiedFactTypeNameShapeRefModeIndependentFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.RefModeIndependentFormatString");
			}
		}
		/// <summary>The string used to display an objectified type name for an object with a reference mode.</summary>
		public static string ObjectifiedFactTypeNameShapeRefModeFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.RefModeFormatString");
			}
		}
		/// <summary>The string used to divide multiple readings shown in a ReadingShape.</summary>
		public static string ReadingShapeReadingSeparator
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.ReadingSeparator");
			}
		}
		/// <summary>The character to use as the object placeholder in a ReadingShape (an ellipsis).</summary>
		public static string ReadingShapeEllipsis
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.Ellipsis");
			}
		}
		/// <summary>The string used to display a reading with a non-primary order when the role is attached.</summary>
		public static string ReadingShapeAttachedRoleDisplay
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.AttachedRoleDisplay");
			}
		}
		/// <summary>The string used to display a reading with a non-primary order when the role is not attached.</summary>
		public static string ReadingShapeUnattachedRoleDisplay
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.UnattachedRoleDisplay");
			}
		}
		/// <summary>Text diplayed in the Model Error when the span of the internal constraint is less than the span of the Fact Type - 1. {0}=constraint name, {1}=fact name, {2}=model name, {3}=factarity-1</summary>
		public static string NMinusOneRuleInternalSpan
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.NMinusOneRule.Text");
			}
		}
		/// <summary>The role players in an external constraint must have compatible types. Replacement field {0} is the constraint name and {1} is the model name.</summary>
		public static string ModelErrorSetConstraintCompatibleRolePlayerTypeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.CompatibleRolePlayerTypeError.Set.Text");
			}
		}
		/// <summary>The role players in an external constraint column must have compatible types. Replacement field {0} is the constraint name, {1} is the model name, and {2} is the (1-based) column number.</summary>
		public static string ModelErrorSetComparisonConstraintCompatibleRolePlayerTypeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.CompatibleRolePlayerTypeError.SetComparison.Text");
			}
		}
		/// <summary>Text displayed in the text of the RoleRequiresRolePlayerError. {0} is the (1-based) role number, {1} is the name of the fact, and {2} is the name of the model.</summary>
		public static string ModelErrorRolePlayerRequiredError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Role.RolePlayerRequired.Message");
			}
		}
		/// <summary>Constraint '{0}' is implied by mandatory constraints.</summary>
		public static string ModelErrorExternalEqualityImpliedByMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.ExternalEqualityImpliedByMandatory.Text");
			}
		}
		/// <summary>Text displayed in the text of the ObjectTypeRequiresPrimarySupertypeError. {0} is the name of the object type and {1} is the name of the model.</summary>
		public static string ModelErrorObjectTypeRequiresPrimarySupertypeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ObjectType.ObjectTypeRequiresPrimarySupertypeError.Message");
			}
		}
		/// <summary>Text displayed in the text of the CompatibleSupertypesError. {0} is the name of the object type and {1} is the name of the model.</summary>
		public static string ModelErrorObjectTypeCompatibleSupertypesError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ObjectType.CompatibleSupertypesError.Message");
			}
		}
		/// <summary>Text displayed in the text of the PreferredIdentifierRequiresMandatoryError. {0} is the name of the object type and {1} is the name of the model.</summary>
		public static string ModelErrorObjectTypePreferredIdentifierRequiresMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ObjectType.PreferredIdentifierRequiresMandatoryError.Message");
			}
		}
		/// <summary>An entity type must have a primary reference scheme. Replacement field {0} is the entity type name and {1} is the model name.</summary>
		public static string ModelErrorEntityTypeRequiresReferenceSchemeMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.EntityType.RequiresReferenceScheme.Message");
			}
		}
		/// <summary>The ring constraint type must be specified. {0} is the constraint name and {1} is the model name.</summary>
		public static string RingConstraintTypeNotSpecifiedError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.RingConstraintTypeNotSpecifiedError.Message");
			}
		}
		/// <summary>Text displayed in the text of the FrequencyConstraintMinMaxError. {0} is the constraint name and {1} is the model name</summary>
		public static string ModelErrorFrequencyConstraintMinMaxError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintMinMaxError.Text");
			}
		}
		/// <summary>Text to place in the title of the verbalization tool window.</summary>
		public static string ModelVerbalizationWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelVerbalization.WindowTitle");
			}
		}
		/// <summary>Text displayed in the text of the ImpliedInternalUniquenessConstraintError. {0} is the fact type name</summary>
		public static string ModelErrorImpliedInternalUniquenessConstraintError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.ImpliedInternalUniquenessConstraintError.Text");
			}
		}
		/// <summary>The message of the auto-fix message box for implied internal constraints</summary>
		public static string ImpliedInternalConstraintFixMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.ImpliedInternalUniquenessConstraint.Message");
			}
		}
		/// <summary>The message for the prompt to delete an element from the model when the final shape representing it is delete. Replacement field 0 gets the class name and 1 the component name.</summary>
		public static string FinalShapeDeletionMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.FinalShapeDeletion.Message");
			}
		}
		/// <summary>The name of the transaction that auto-fixes implied and duplicate internal constraints.</summary>
		public static string RemoveImpliedInternalUniquenessConstraintsTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "FactType.RemoveImpliedInternalUniquenessConstraints.TransactionName");
			}
		}
		/// <summary>The transaction name used by shape alignment commands. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string AlignShapesTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "AlignShapes.TransactionName");
			}
		}
		/// <summary>The transaction name used by AutoLayout command. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string AutoLayoutTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "AutoLayout.TransactionName");
			}
		}
		/// <summary>Return the default format string for any link shape. Replacement field {0} is the accessible name for the from object, field {1} is the accessible name for the to object.</summary>
		public static string DefaultLinkShapeAccessibleValueFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ORMBaseBinaryLinkShape.Accessible.ValueFormat");
			}
		}
		/// <summary>Returned as the accessible description for a role player link</summary>
		public static string RolePlayerLinkAccessibleDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "RolePlayerLink.Accessible.Description");
			}
		}
		/// <summary>Returned as the accessible name for a role player link</summary>
		public static string RolePlayerLinkAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "RolePlayerLink.Accessible.Name");
			}
		}
		/// <summary>The end point of a role player link is described in terms of the fact name and the role name. Replacement field {0} is the fact name, {1} is the role name, and {2} is the role position.</summary>
		public static string RolePlayerLinkAccessibleFromValueFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "RolePlayerLink.Accessible.FromValueFormat");
			}
		}
		/// <summary>Returned as the accessible description for a subtype link</summary>
		public static string SubtypeLinkAccessibleDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SubtypeLink.Accessible.Description");
			}
		}
		/// <summary>Returned as the accessible name for a subtype link</summary>
		public static string SubtypeLinkAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SubtypeLink.Accessible.Name");
			}
		}
		/// <summary>Returned as the accessible description for an external constraint link</summary>
		public static string ExternalConstraintLinkAccessibleDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintLink.Accessible.Description");
			}
		}
		/// <summary>Returned as the accessible name for an external constraint link</summary>
		public static string ExternalConstraintLinkAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintLink.Accessible.Name");
			}
		}
		/// <summary>Returned as the accessible description for a model note link</summary>
		public static string ModelNoteLinkAccessibleDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ModelNoteLink.Accessible.Description");
			}
		}
		/// <summary>Returned as the accessible name for a model note link</summary>
		public static string ModelNoteLinkAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ModelNoteLink.Accessible.Name");
			}
		}
		/// <summary>Returned as the accessible description for a value range link</summary>
		public static string ValueRangeLinkAccessibleDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ValueRangeLink.Accessible.Description");
			}
		}
		/// <summary>Returned as the accessible name for a value range link</summary>
		public static string ValueRangeLinkAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ValueRangeLink.Accessible.Name");
			}
		}
		/// <summary>The title of the message shown when a registered extension fails to load.</summary>
		public static string ExtensionLoadFailureTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExtensionLoadFailure.Title");
			}
		}
		/// <summary>The format string for the content of the message shown when a registered extension fails to load.</summary>
		/// <remarks>{0} will be <see cref="System.Environment.NewLine"/>. {1} will be the registered name of the extension that failed to load. {2} will be the result of calling <see cref="System.Exception.ToString">ToString()</see> on the <see cref="System.Exception"/> that caused the load failure.</remarks>
		public static string ExtensionLoadFailureMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExtensionLoadFailure.Message");
			}
		}
		/// <summary>The default name for new diagrams and the context menu command text and transaction name for creating a new diagram.</summary>
		public static string DiagramCommandNewPage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Diagram.Command.NewPage");
			}
		}
		/// <summary>The context menu command text and transaction name for renaming a diagram.</summary>
		public static string DiagramCommandRenamePage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Diagram.Command.RenamePage");
			}
		}
		/// <summary>The context menu command text and transaction name for deleting diagram.</summary>
		public static string DiagramCommandDeletePage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Diagram.Command.DeletePage");
			}
		}
		/// <summary>The format string used to combine the sequence and column numbers in the role box of an active set (multicolumn) constraint.</summary>
		public static string SetConstraintStickyRoleFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SetConstraintStickyRole.DisplayFormatString");
			}
		}
		/// <summary>The format string used to combine the sequence and column numbers in the tooltip of the role box of an active set (multicolumn) constraint.</summary>
		public static string SetConstraintStickyRoleTooltipFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SetConstraintStickyRole.DisplayTooltipFormatString");
			}
		}
		/// <summary>The string to display when role sequences in the active set (multicolumn) constraint overlap on a single role.</summary>
		public static string SetConstraintStickyRoleOverlapping
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "SetConstraintStickyRole.DisplayOverlapping");
			}
		}
	}
	#endregion // ResourceStrings class
}
