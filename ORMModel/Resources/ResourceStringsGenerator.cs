
// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Core
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
				return ResourceStrings.GetString(ResourceManagers.ShapeModel, "ORM DesignerToolboxTab");
			}
		}
		/// <summary>The accessible name for a diagram</summary>
		public static string ORMDiagramAccessibleName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ShapeModel, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMDiagram.DisplayName");
			}
		}
		/// <summary>The display name used for a simple FactType</summary>
		public static string FactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.DisplayName");
			}
		}
		/// <summary>The display name used for a SubtypeFact</summary>
		public static string SubtypeFact
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.DisplayName");
			}
		}
		/// <summary>The display name used for a disjunctive mandatory constraint</summary>
		public static string DisjunctiveMandatoryConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.MandatoryConstraint.DisplayName");
			}
		}
		/// <summary>The display name used for a ReadingType</summary>
		public static string ReadingType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.Reading.DisplayName");
			}
		}
		/// <summary>The name displayed as the class name for all <see cref="ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingType"/> instances</summary>
		public static string ElementGroupingTypeDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingType.DisplayName");
			}
		}
		/// <summary>The display name used for an ObjectType when IsValueType is true</summary>
		public static string ValueType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueType");
			}
		}
		/// <summary>The display name used for an ObjectType when IsValueType is false</summary>
		public static string EntityType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.EntityType");
			}
		}
		/// <summary>The description for the Name property of a constraint.</summary>
		public static string ConstraintNameDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Constraint.Name.Description");
			}
		}
		/// <summary>Description for <see cref="P:ObjectModel.ORMModel.ModelErrorDisplayFilterDisplay"/></summary>
		public static string ModelModelErrorDisplayFilterDisplayDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Model.ModelErrorDisplayFilterDisplay.Description");
			}
		}
		/// <summary>Display name for <see cref="P:ObjectModel.ORMModel.ModelErrorDisplayFilterDisplay"/></summary>
		public static string ModelModelErrorDisplayFilterDisplayDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Model.ModelErrorDisplayFilterDisplay.DisplayName");
			}
		}
		/// <summary>The description for the Name property of an ORMModel.</summary>
		public static string ModelNameDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Model.Name.Description");
			}
		}
		/// <summary>Description for <see cref="ObjectModel.ObjectType.DataTypeDisplay"/></summary>
		public static string ObjectTypeDataTypeDisplayDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ObjectType.DataTypeDisplay.Description");
			}
		}
		/// <summary>Display name for <see cref="ObjectModel.ObjectType.DataTypeDisplay"/></summary>
		public static string ObjectTypeDataTypeDisplayDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ObjectType.DataTypeDisplay.DisplayName");
			}
		}
		/// <summary>The description for the Name property of an ObjectType.</summary>
		public static string ObjectTypeNameDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ObjectType.Name.Description");
			}
		}
		/// <summary>Description for <see cref="ObjectModel.ReferenceMode.KindDisplay"/></summary>
		public static string ReferenceModeKindDisplayDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ReferenceMode.KindDisplay.Description");
			}
		}
		/// <summary>Display name for <see cref="ObjectModel.ReferenceMode.KindDisplay"/></summary>
		public static string ReferenceModeKindDisplayDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ReferenceMode.KindDisplay.DisplayName");
			}
		}
		/// <summary>Description for <see cref="ObjectModel.Role.RolePlayerDisplay"/></summary>
		public static string RoleRolePlayerDisplayDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Role.RolePlayerDisplay.Description");
			}
		}
		/// <summary>Display name for <see cref="ObjectModel.Role.RolePlayerDisplay"/></summary>
		public static string RoleRolePlayerDisplayDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Role.RolePlayerDisplay.DisplayName");
			}
		}
		/// <summary>The description for the Name property of a SubtypeFact.</summary>
		public static string SubtypeFactNameDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.SubtypeFact.Name.Description");
			}
		}
		/// <summary>The display name used for an objectified (nested) FactType</summary>
		public static string ObjectifiedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ObjectifiedFactType");
			}
		}
		/// <summary>The display name used for an internal uniqueness constraint</summary>
		public static string InternalUniquenessConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.InternalUniquenessConstraint");
			}
		}
		/// <summary>The display name used for an external uniqueness constraint</summary>
		public static string ExternalUniquenessConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.UniquenessConstraint");
			}
		}
		/// <summary>The display name used for an implied mandatory constraint</summary>
		public static string ImpliedMandatoryConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ImpliedMandatoryConstraint");
			}
		}
		/// <summary>The display name used for a simple mandatory constraint</summary>
		public static string SimpleMandatoryConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.SimpleMandatoryConstraint");
			}
		}
		/// <summary>The name displayed to represent null in the role player picker</summary>
		public static string RolePlayerPickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Editors.RolePlayerPicker.NullItemText");
			}
		}
		/// <summary>The name displayed to represent null in the nested fact type picker</summary>
		public static string NestedFactTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Editors.NestedFactTypePicker.NullItemText");
			}
		}
		/// <summary>The name displayed to represent null in the nesting type picker</summary>
		public static string NestingTypePickerNullItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.Editors.NestingTypePicker.NullItemText");
			}
		}
		/// <summary>The base name used to create a name for a new EntityType. This is a format string</summary>
		public static string EntityTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.EntityType.DefaultNamePattern");
			}
		}
		/// <summary>An alternate description for the DataType.Length property. The default description describes a variable length.</summary>
		public static string DataTypeExactLengthDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.Length.ExactLengthDescription");
			}
		}
		/// <summary>An alternate display name for the DataType.Length property.</summary>
		public static string DataTypePrecision
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.Precision");
			}
		}
		/// <summary>An alternate description for the DataType.Length property.</summary>
		public static string DataTypePrecisionDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.Precision.Description");
			}
		}
		/// <summary>The descriptive text for a PortableDataType of Unspecified.</summary>
		public static string PortableDataTypeUnspecified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Unspecified.Text");
			}
		}
		/// <summary>A fixed length text data type</summary>
		public static string PortableDataTypeTextFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Text.FixedLength.Text");
			}
		}
		/// <summary>A variable length text data type</summary>
		public static string PortableDataTypeTextVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Text.VariableLength.Text");
			}
		}
		/// <summary>A large length text data type</summary>
		public static string PortableDataTypeTextLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Text.LargeLength.Text");
			}
		}
		/// <summary>A signed integer numeric data type</summary>
		public static string PortableDataTypeNumericSignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.SignedInteger.Text");
			}
		}
		/// <summary>A signed large integer numeric data type</summary>
		public static string PortableDataTypeNumericSignedLargeInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.SignedLargeInteger.Text");
			}
		}
		/// <summary>A signed small integer numeric data type</summary>
		public static string PortableDataTypeNumericSignedSmallInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.SignedSmallInteger.Text");
			}
		}
		/// <summary>An unsigned integer numeric data type</summary>
		public static string PortableDataTypeNumericUnsignedInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.UnsignedInteger.Text");
			}
		}
		/// <summary>An unsigned large integer numeric data type</summary>
		public static string PortableDataTypeNumericUnsignedLargeInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.UnsignedLargeInteger.Text");
			}
		}
		/// <summary>An unsigned small integer numeric data type</summary>
		public static string PortableDataTypeNumericUnsignedSmallInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.UnsignedSmallInteger.Text");
			}
		}
		/// <summary>An unsigned tiny integer numeric data type</summary>
		public static string PortableDataTypeNumericUnsignedTinyInteger
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.UnsignedTinyInteger.Text");
			}
		}
		/// <summary>An auto counter numeric data type</summary>
		public static string PortableDataTypeNumericAutoCounter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.AutoCounter.Text");
			}
		}
		/// <summary>A custom precision floating point numeric data type</summary>
		public static string PortableDataTypeNumericFloatingPoint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.FloatingPoint.Text");
			}
		}
		/// <summary>A single precision floating point numeric data type</summary>
		public static string PortableDataTypeNumericSinglePrecisionFloatingPoint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.SinglePrecisionFloatingPoint.Text");
			}
		}
		/// <summary>A double precision floating point numeric data type</summary>
		public static string PortableDataTypeNumericDoublePrecisionFloatingPoint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.DoublePrecisionFloatingPoint.Text");
			}
		}
		/// <summary>A decimal numeric data type</summary>
		public static string PortableDataTypeNumericDecimal
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.Decimal.Text");
			}
		}
		/// <summary>A money numeric data type</summary>
		public static string PortableDataTypeNumericMoney
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Numeric.Money.Text");
			}
		}
		/// <summary>A fixed length raw data data type</summary>
		public static string PortableDataTypeRawDataFixedLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.RawData.FixedLength.Text");
			}
		}
		/// <summary>A variable length raw data data type</summary>
		public static string PortableDataTypeRawDataVariableLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.RawData.VariableLength.Text");
			}
		}
		/// <summary>A large length raw data data type</summary>
		public static string PortableDataTypeRawDataLargeLength
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.RawData.LargeLength.Text");
			}
		}
		/// <summary>A picture raw data data type</summary>
		public static string PortableDataTypeRawDataPicture
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.RawData.Picture.Text");
			}
		}
		/// <summary>An OLE object raw data data type</summary>
		public static string PortableDataTypeRawDataOleObject
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.RawData.OleObject.Text");
			}
		}
		/// <summary>An auto timestamp temporal data type</summary>
		public static string PortableDataTypeTemporalAutoTimestamp
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Temporal.AutoTimestamp.Text");
			}
		}
		/// <summary>A time temporal data type</summary>
		public static string PortableDataTypeTemporalTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Temporal.Time.Text");
			}
		}
		/// <summary>A date temporal data type</summary>
		public static string PortableDataTypeTemporalDate
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Temporal.Date.Text");
			}
		}
		/// <summary>A date and time temporal data type</summary>
		public static string PortableDataTypeTemporalDateAndTime
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Temporal.DateAndTime.Text");
			}
		}
		/// <summary>A true or false logical data type</summary>
		public static string PortableDataTypeLogicalTrueOrFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Logical.TrueOrFalse.Text");
			}
		}
		/// <summary>A yes or no logical data type</summary>
		public static string PortableDataTypeLogicalYesOrNo
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Logical.YesOrNo.Text");
			}
		}
		/// <summary>A row id data type (can not be classified in any of the groups above)</summary>
		public static string PortableDataTypeOtherRowId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Other.RowId.Text");
			}
		}
		/// <summary>An object id data type (can not be classified in any of the groups above)</summary>
		public static string PortableDataTypeOtherObjectId
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.DataType.PortableDataType.Other.ObjectId.Text");
			}
		}
		/// <summary>Used to automatically turn a value type into an entity type with a reference mode when IsValueType is set to false and the ValueType has downstream value roles. Replacement field {0} is the name of the exisiting type and field {1} is used to insert a number to ensure that the name is unique. {0}Values{1} is the default format.</summary>
		public static string ValueTypeAutoCreateReferenceModeNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueType.AutoCreateReferenceModeNamePattern");
			}
		}
		/// <summary>The base name used to create a name for a new ValueType. This is a format string with {0} being the placeholder for the number placement.</summary>
		public static string ValueTypeDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueType.DefaultNamePattern");
			}
		}
		/// <summary>The format string for the default reading of a subtype. The {0} replacement field is used for the subtype element name; {1} for the supertype.</summary>
		public static string SubtypeFactDefaultReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.DefaultReadingText");
			}
		}
		/// <summary>The format string for the generated name of a subtype relationship. The {0} replacement field is used for the subtype element name; {1} for the supertype.</summary>
		public static string SubtypeFactElementNameFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.ElementNameFormat");
			}
		}
		/// <summary>The inverse instance reading for the predicate created by creating a sub type relationship.</summary>
		public static string SubtypeFactPredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SubtypeFact.PredicateInverseReading");
			}
		}
		/// <summary>The instance reading for the forward predicate created by creating a subtype relationship.</summary>
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
		/// <summary>The reading for the predicate created implicitly via objectification.There is no attempt made to keep the predicate readings unique in ring situations.We allow the model error to populate instead of generating an articial unique reading.</summary>
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
		/// <summary>The name given to the transaction used when adding an internal uniqueness constraint to correct the FactTypeRequiresInternalUniquenessConstraint error.</summary>
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
		/// <summary>The name given to the transaction used when unobjectifying a fact type.</summary>
		public static string UnobjectifyFactTypeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "UnobjectifyFactType.TransactionName");
			}
		}
		/// <summary>The transaction name used by ExclusiveOrCoupler command. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ExclusiveOrCouplerTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExclusiveOrCoupler.TransactionName");
			}
		}
		/// <summary>The transaction name used by ExclusiveOrDecoupler command. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ExclusiveOrDecouplerTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExclusiveOrDecoupler.TransactionName");
			}
		}
		/// <summary>The name given to the transaction used when a culture change modifies displayed diagram elements.</summary>
		public static string CultureChangedTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "CultureChanged.TransactionName");
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
		public static string ElementGroupingPropertyCategory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping/TypeCompliance.Category");
			}
		}
		/// <summary>The base name used to create a name for a new ElementGrouping. This is a format string with {0} being the placeholder for the number placement.</summary>
		public static string ElementGroupingDefaultNamePattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGrouping.DefaultNamePattern");
			}
		}
		/// <summary>The transaction name used when adding an element to a group.</summary>
		public static string ElementGroupingAddElementTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGrouping.AddElement.TransactionName");
			}
		}
		/// <summary>The transaction name used when adding a group.</summary>
		public static string ElementGroupingAddGroupTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGrouping.AddGroup.TransactionName");
			}
		}
		/// <summary>The transaction name used when removing an explicitly excluded element from a group.</summary>
		public static string ElementGroupingRemoveElementExclusionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGrouping.RemoveElementExclusion.TransactionName");
			}
		}
		/// <summary>Description shown in the properties window for the 'GroupTypes' property of an ElementGrouping.</summary>
		public static string ElementGroupingTypesPropertyDescriptorDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGroupingTypes.PropertyDescriptor.Description");
			}
		}
		/// <summary>Display name shown in the properties window for the 'GroupTypes' property of an ElementGrouping.</summary>
		public static string ElementGroupingTypesPropertyDescriptorDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGroupingTypes.PropertyDescriptor.DisplayName");
			}
		}
		/// <summary>The transaction name used when modifying the set of types associated with a group</summary>
		public static string ElementGroupingTypesPropertyDescriptorTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ElementGroupingTypes.PropertyDescriptor.TransactionName");
			}
		}
		/// <summary>The text shown to explain how to hook up an external constraint to its associated roles.</summary>
		public static string ExternalConstraintConnectActionSetComparisonConstraintInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.SetComparisonConstraint.Instructions");
			}
		}
		/// <summary>The text shown to explain how to hook up an external constraint to its associated roles.</summary>
		public static string ExternalConstraintConnectActionSetConstraintInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ExternalConstraintConnectAction.SetConstraint.Instructions");
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
		/// <summary>The format to be used in frequency constraints for a minimum value (eg: Minimum of {0}).</summary>
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
		/// <summary>The format to be used in frequency constraints to represent a range of values (eg: Between {0} and {1}).</summary>
		public static string FrequencyConstraintBetweenFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FrequencyConstraint.Between.FormatString");
			}
		}
		/// <summary>The text shown to explain how to hook up an internal uniqueness constraint to its associated roles.</summary>
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
		/// <summary>The transaction name used by the modelnote connect action. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ModelNoteConnectActionTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ModelNoteConnectAction.TransactionName");
			}
		}
		/// <summary>The transaction name used by the model error display filter dialog when a filter is changed. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string ModelErrorDisplayFilterChangeTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ModelErrorDisplayFilterChange.TransactionName");
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
		/// <summary>The transaction name used for moving a role order in a fact type shape. The text appears in the undo dropdown in the VS IDE.</summary>
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
		/// <summary>The transaction name used for moving a role sequence up in a multi column external uniqueness constraint. The text appears in the undo dropdown in the VS IDE.</summary>
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
		/// <summary>The window title for the ORM model browser window</summary>
		public static string ModelBrowserWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ORMModelBrowser.WindowTitle");
			}
		}
		/// <summary>Exception message when a ModelErrorDisplayFilterAttribute is initialized with a type that does not derive from ModelErrorCategory.</summary>
		public static string ModelExceptionModelErrorCategoryInvalid
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ModelErrorCategory.TypeNotDerivedFromModelErrorCategory");
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
		/// <summary>Exception message when an attempt is made to objectify a fact type that is implied by another objectification.</summary>
		public static string ModelExceptionObjectificationImpliedFactObjectified
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedFactTypesCannotBeObjectified");
			}
		}
		/// <summary>Exception message when an attempt is made to change an explicit Objectification to implied when it does not match the implied objectification pattern or is not independent (if possible) or plays a Role in a non-implied FactType.</summary>
		public static string ModelExceptionObjectificationImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact");
			}
		}
		/// <summary>Exception message when an attempt is made to operate on an implied objectification by a method that only accepts explicit objectifications.</summary>
		public static string ModelExceptionObjectificationImpliedObjectificationNotAllowed
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Objectification.ImpliedObjectificationNotAllowed");
			}
		}
		/// <summary>Exception message when an attempt is made to modify an EntityTypeInstance implied by a FactTypeInstance and an internal preferred identifier on the objectified FactType.</summary>
		public static string ModelExceptionObjectificationInstanceDirectModificationOfImpliedEntityTypeInstance
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectificationInstance.DirectModificationOfImpliedEntityTypeInstance");
			}
		}
		/// <summary>Exception message when an attempt is made to create an ObjectificationInstance will partially constructed endpoint instances.</summary>
		public static string ModelExceptionObjectificationInstanceIncompleteRolePlayers
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectificationInstance.IncompleteRolePlayers");
			}
		}
		/// <summary>Exception message output when an attempt is made to set the IsPrimaryForReadingOrder or IsPrimaryForFactType properties of a Reading to false.</summary>
		public static string ModelExceptionReadingIsPrimaryToFalse
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Reading.IsPrimary.ReadOnlyWhenFalse");
			}
		}
		/// <summary>The error message that is returned when attempting to add a new reading to a fact type and the text is not valid. Validate reading text must have the number of placeholders equal to the number of roles in the FactType. The replacement fields must be ordered and identified using String.Format replacement syntax. For example: "{0} has {1}".</summary>
		public static string ModelExceptionFactAddReadingInvalidReadingText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Fact.AddReading.InvalidReadingText");
			}
		}
		/// <summary>Model validation error text for an aggregate calculation with no aggregation context. {0}=error display context</summary>
		public static string ModelErrorCalculatedPathValueRequiresAggregationContext
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.CalculatedPathValue.CalculatedPathValueRequiresAggregationContext.Text");
			}
		}
		/// <summary>Model validation error text for a path calculation without no specified function. {0}=error display context</summary>
		public static string ModelErrorCalculatedPathValueRequiresFunction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.CalculatedPathValue.CalculatedPathValueRequiresFunction.Text");
			}
		}
		/// <summary>Model validation error text for a path calculation with no binding for some parameter. {0}=error display context,{1}=parameter name</summary>
		public static string ModelErrorCalculatedPathValueParameterBinding
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.CalculatedPathValue.CalculatedPathValueParameterBinding.Text");
			}
		}
		/// <summary>Model validation error text for a path calculation with a result that is not being used. {0}=error display context</summary>
		public static string ModelErrorCalculatedPathValueMustBeConsumed
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.CalculatedPathValue.CalculatedPathValueMustBeConsumed.Text");
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
		/// <summary>Model validation error text when a single-column equality constraint is put on a set of mandatory roles.</summary>
		public static string ModelErrorConstraintImplicationEqualityMandatory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.EqualityImpliedByMandatoryError.Text");
			}
		}
		/// <summary>Model validation error text when a single-column subset constraint targets a mandatory role.</summary>
		public static string ModelErrorConstraintImplicationSubsetMandatory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.SubsetImpliedByMandatoryError.Text");
			}
		}
		/// <summary>Model validation error text for a fact type derivation with no path projection. {0}=error display context</summary>
		public static string ModelErrorFactTypeDerivationRuleProjectionRequired
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactTypeDerivationRule.FactTypeDerivationRequiresProjection.Text");
			}
		}
		/// <summary>Model validation error text for a fact type derivation projection that does not project all fact type roles. {0}=error display context</summary>
		public static string ModelErrorFactTypeDerivationRulePartialProjection
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactTypeDerivationRule.PartialFactTypeDerivationProjection.Text");
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
		/// <summary>Model validation error text when too few role sequences are specified for a constraint. This is a common condition when constraints are being created.</summary>
		public static string ModelErrorConstraintHasTooFewRoleSequencesText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.TooFewRoleSequences.Text");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with the derivation rule of a FactType. Automatically capitalized on replacement. {0}=FactType {1}=Model.</summary>
		public static string ModelErrorDisplayContextFactTypeDerivationRule
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.FactTypeDerivationRule");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with a Role in a FactType. Automatically capitalized on replacement. {0}=model {1}=facttype {2}=role number.</summary>
		public static string ModelErrorDisplayContextFactTypeRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.FactTypeRole");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with an ObjectType. Automatically capitalized on replacement. {0}=ObjectType {1}=Model.</summary>
		public static string ModelErrorDisplayContextObjectType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.ObjectType");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with a sequence in a set comparison constraint. Automatically capitalized on replacement. {0}=Constraint {1}=Model {2}=sequence number.</summary>
		public static string ModelErrorDisplayContextSetComparisonConstraintSequence
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.SetComparisonConstraintSequence");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with a sequence in a set comparison constraint join path. Automatically capitalized on replacement. {0}=Constraint {1}=Model {2}=sequence number.</summary>
		public static string ModelErrorDisplayContextSetComparisonConstraintSequenceJoinPath
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.SetComparisonConstraintSequenceJoinPath");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with a SetConstraint. Automatically capitalized on replacement. {0}=SetConstraint {1}=Model.</summary>
		public static string ModelErrorDisplayContextSetConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.SetConstraint");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with a SetConstraint join path. Automatically capitalized on replacement. {0}=SetConstraint {1}=Model.</summary>
		public static string ModelErrorDisplayContextSetConstraintJoinPath
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.SetConstraintJoinPath");
			}
		}
		/// <summary>Dynamic text replacement for an error associated with the derivation rule of a subtype. Automatically capitalized on replacement. {0}=ObjectType {1}=Model.</summary>
		public static string ModelErrorDisplayContextSubtypeDerivationRule
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.DisplayContext.SubtypeDerivationRule");
			}
		}
		/// <summary>Model validation error text used when a frequency constraint is places on the same roles as a uniqueness constraint. {0}=constraint name {1}=model name</summary>
		public static string ModelErrorFrequencyConstraintViolatedByUniquenessConstraintText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintViolatedByUniquenessConstraintError.Text");
			}
		}
		/// <summary>Model validation error text when too many role sequences are specified for a constraint. This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
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
		/// <summary>Model validation error text for a constraint join path with no path projection. {0}=error display context</summary>
		public static string ModelErrorJoinPathProjectionRequired
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ConstraintRoleSequenceJoinPath.ConstraintRoleSequenceJoinPathRequiresProjection.Text");
			}
		}
		/// <summary>Model validation error text for a constraint join path projection that does not project all constraint roles. {0}=error display context</summary>
		public static string ModelErrorJoinPathPartialProjection
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ConstraintRoleSequenceJoinPath.PartialConstraintRoleSequenceJoinPathProjection.Text");
			}
		}
		/// <summary>Model validation error text used when multiple constraints with the same name are loaded into a model. Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateConstraintNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateConstraintNames.Text");
			}
		}
		/// <summary>Model validation error text used when multiple functions with the same name are loaded into a model.Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateFunctionNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateFunctionNames.Text");
			}
		}
		/// <summary>Model validation error text used when multiple object types with the same name are loaded into a model.Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateObjectTypeNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateObjectTypeNames.Text");
			}
		}
		/// <summary>Model validation error text used when multiple recognized phrases with the same name are loaded into a model.Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateRecognizedPhraseNames
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.DuplicateRecognizedPhraseNames.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a uniqueness constraint and its role is named.  Field 0 is the name of the object type, field 1 is the string representation of the instance, field 2 is the model name, field 3 is the role name.</summary>
		public static string ModelErrorModelHasPopulationUniquenessErrorWithNamedRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationUniquenessError.Role.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a uniqueness constraint and its role is unnamed.  Field 0 is the name of the object type, field 1 is the string representation of the instance, field 2 is the model name, field 3 is the facttype name.</summary>
		public static string ModelErrorModelHasPopulationUniquenessErrorWithUnnamedRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationUniquenessError.FactType.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a mandatory constraint. Field 0 is the name of the role player, field 1 is the derived name of the instance, field 2 is the model name, field 3 is the name of the first facttype name, field 4 is a placeholder for additional fact types.</summary>
		public static string ModelErrorModelHasPopulationMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationMandatoryError.Text");
			}
		}
		/// <summary>Model validation error shown when a population violates a mandatory constraint (additional fact types). Field 0 is the name of the fact type, field 1 is the following fact type (replaced by yet another trailing fact type, if available).</summary>
		public static string ModelErrorModelHasPopulationMandatoryErrorAdditionalFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Model.PopulationMandatoryError.AdditionalFactTypeText");
			}
		}
		/// <summary>Model validation error text for explicit object unifiers relating multiple disconnected pathed roles and path roots. {0}=error display context</summary>
		public static string ModelErrorRolePathObjectUnifierIncompatibleCorrelation
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.ObjectUnifierRequiresCompatibleObjectTypes.Text");
			}
		}
		/// <summary>Model validation error text for a join to a pathed role with a role player that is not compatible with the join source. {0}=error display context</summary>
		public static string ModelErrorRolePathIncompatibleJoin
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.JoinedPathRoleRequiresCompatibleRolePlayer.Text");
			}
		}
		/// <summary>Model validation error text for a same fact type role specified without a corresponding start or join role. {0}=error display context</summary>
		public static string ModelErrorRolePathSameFactTypeRoleFollowsJoin
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.SameFactTypeRoleFollowsJoin.Text");
			}
		}
		/// <summary>Model validation error text for a pathed role that is not marked as a start role or is marked as a start role and should not be. {0}=error display context</summary>
		public static string ModelErrorRolePathStartRoleFollowsRootObjectType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.StartRoleFollowsRootObjectType.Text");
			}
		}
		/// <summary>Model validation error text for an outer join to a mandatory role. {0}=error display context</summary>
		public static string ModelErrorRolePathMandatoryOuterJoin
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.OuterJoinRequiresOptionalRole.Text");
			}
		}
		/// <summary>Model validation error text for a role path with no root object type. {0}=error display context</summary>
		public static string ModelErrorRolePathRequiresRootObjectType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RolePath.RequiresRootObjectType.Text");
			}
		}
		/// <summary>ValueConstraintValueTypeDetachedError text with owner place holder. The resulting sentence will be capitalized automatically. {0}=owner information</summary>
		public static string ModelErrorValueConstraintValueTypeDetachedError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueConstraint.ValueTypeDetachedError.Message");
			}
		}
		/// <summary>ValueRangeOverlapError text with owner place holder. The resulting sentence will be capitablized automatically. {0}=owner information</summary>
		public static string ModelErrorValueConstraintValueRangeOverlapError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueConstraint.ValueRangeOverlapError.Message");
			}
		}
		/// <summary>MaxValueMismatchError text with owner place holder. The resulting sentence will be capitalized automatically. {0}=owner information</summary>
		public static string ModelErrorValueRangeMaxValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MaxValueMismatchError.Message");
			}
		}
		/// <summary>MinValueMismatchError text with owner place holder. The resulting sentence will be capitalized automatically. {0}=owner information</summary>
		public static string ModelErrorValueRangeMinValueMismatchError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueRange.MinValueMismatchError.Message");
			}
		}
		/// <summary>Category name to display for uncategorized model errors.</summary>
		public static string ModelErrorUncategorized
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.UncategorizedModelError.CategoryDisplayName");
			}
		}
		/// <summary>Text to display for the property for the ModelErrorDisplayFilter when it is filtered.</summary>
		public static string ModelErrorDisplayFilteredText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelErrorDisplayFilter.ToStringOverrideText");
			}
		}
		/// <summary>Exception message when an attempt is made to modify assign a PathConstant as the input for a bag parameter.</summary>
		public static string ModelExceptionPathConstantInconsistentWithBagInput
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.CalculatedPathValueInputBindsToPathConstant.PathConstantInconsistentWithBagInput");
			}
		}
		/// <summary>Exception message when an attempt is made to modify the CalculatedValue of a CalculatedPathValueIsCalculatedWithFunction relationship.</summary>
		public static string ModelExceptionEnforceInitialCalculatedValue
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.CalculatedPathValueIsCalculatedWithFunction.EnforceInitialCalculatedValue");
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
		/// <summary>Exception message when an attempt is made to modify a role player associated with an implicit boolean value.</summary>
		public static string ModelExceptionFactTypeEnforceNoImplicitBooleanValueTypeRolePlayerChange
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactType.EnforceNoImplicitBooleanValueTypeRolePlayerChange");
			}
		}
		/// <summary>Exception message when an attempt is made to move a role to a different FactType.</summary>
		public static string ModelExceptionFactTypeEnforceNoRoleMigration
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactType.EnforceNoRoleMigration");
			}
		}
		/// <summary>Exception message when an attempt is made to set the IsIndependent property on an object type that plays mandatory roles that are not part of its preferred identification scheme.</summary>
		public static string ModelExceptionObjectTypeEnforceIsIndependentPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectType.EnforceIsIndependentPattern");
			}
		}
		/// <summary>Exception message when an attempt is made to change the role players of an ObjectTypeHasObjectTypeInstance relationship.</summary>
		public static string ModelExceptionObjectTypeInstanceEnforceFixedRolePlayers
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ObjectTypeInstance.EnforceFixedRolePlayers");
			}
		}
		/// <summary>Exception message when an attempt is made to set both a primary identifier and a value type on the same object type.</summary>
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
		/// <summary>Exception message when an attempt is made to set the IsMandatory property on an unattached role. IsMandatory creates an internal constraint, which is owned by an FactType, so cannot be realized if the parent FactType is unknown.</summary>
		public static string ModelExceptionIsMandatoryRequiresAttachedFactType
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Role.IsMandatoryRequiresAttachedFactType");
			}
		}
		/// <summary>Exception message when an attempt is made to modify the Role of a RoleInstance.</summary>
		public static string ModelExceptionRoleInstanceEnforceInitialRole
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.RoleInstance.EnforceInitialRole");
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
		public static string ModelExceptionConstraintEnforceNoForeignFactTypes
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Constraint.EnforceNoForeignFactTypes");
			}
		}
		/// <summary>Exception message when an attempt is made to attached more than one fact type to an internal constraint.</summary>
		public static string ModelExceptionConstraintEnforceSingleFactTypeForInternalConstraint
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.Constraint.EnforceSingleFactTypeForInternalConstraint");
			}
		}
		/// <summary>Exception message when an attempt is made to set the IsAutomatic property to true for a ConstraintRoleSequenceJoinPath when no automatic join path can be determined.</summary>
		public static string ModelExceptionConstraintRoleSequenceAutomaticJoinPathNotAvailable
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ConstraintRoleSequence.AutomaticJoinPathNotAvailable");
			}
		}
		/// <summary>Exception message when an attempt is made to directly modify role players on the ConstraintRoleSequenceHasRole relationship.</summary>
		public static string ModelExceptionConstraintRoleSequenceHasRoleEnforceNoRolePlayerChange
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ConstraintRoleSequenceHasRole.EnforceNoRolePlayerChange");
			}
		}
		/// <summary>Exception message when an attempt is made to form an ExclusiveOrConstraintCoupler relationship between incompatible MandatoryConstraint and ExclusionConstraint elements.</summary>
		public static string ModelExceptionExclusiveOrConstraintCouplerInconsistentConstraints
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ExclusiveOrConstraintCoupler.InconsistentConstraintRolesOrModality");
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
		/// <summary>The description for the default verbalization report snippets.</summary>
		public static string VerbalizationReportSnippetsDefaultDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Verbalization.ReportSnippetsDefaultDescription");
			}
		}
		/// <summary>The description for the snippets associated with the verbalization reports.</summary>
		public static string VerbalizationReportSnippetsTypeDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Verbalization.ReportSnippetsTypeDescription");
			}
		}
		/// <summary>Exception message when an attempt is made to add a FactTypeInstance to an implied population.</summary>
		public static string ModelExceptionFactTypeInstanceDirectionPopulationOfImpliedInstances
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactTypeInstance.DirectionPopulationOfImpliedInstances");
			}
		}
		/// <summary>Exception message when an attempt is made to modify either role player on a FactTypeInstanceHasRoleInstance relationship.</summary>
		public static string ModelExceptionFactTypeInstanceEnforceFixedRoleInstance
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactTypeInstance.EnforceFixedRoleInstance");
			}
		}
		/// <summary>Exception message when an attempt is made to add a FactTypeRoleInstance with a duplicate Role to the FactTypeRoleInstance collection.</summary>
		public static string ModelExceptionFactTypeInstanceEnforceRoleUniqueness
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.FactTypeInstance.EnforceRoleUniqueness");
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
		/// <summary>Exception message when an attempt is made to modify either role player on a EntityTypeInstanceHasRoleInstance relationship.</summary>
		public static string ModelExceptionEntityTypeInstanceEnforceFixedRoleInstance
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeInstance.EnforceFixedRoleInstance");
			}
		}
		/// <summary>Exception message when an attempt is made to add an EntityTypeRoleInstance with a duplicate Role to the EntityTypeRoleInstance collection.</summary>
		public static string ModelExceptionEntityTypeInstanceEnforceRoleUniqueness
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeInstance.EnforceRoleUniqueness");
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
		/// <summary>Exception message when an attempt is made to attach an EntityTypeSubtypeInstance to an EntityTypeSubtype that is associated with another EntityTypeSubtypeInstance on the same subtype.</summary>
		public static string ModelExceptionEntityTypeSubtypeInstanceDuplicateSupertypeInstance
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeSubtypeInstance.DuplicateSupertypeInstance");
			}
		}
		/// <summary>Exception message when an attempt is made to modify the EntityTypeSubtypeInstance role of the EntityTypeSubtypeInstanceHasSupertypeInstance relationship</summary>
		public static string ModelExceptionEntityTypeSubtypeInstanceEnforceInitialSubtypeInstance
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.EntityTypeSubtypeInstance.EnforceInitialSubtypeInstance");
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
		/// <summary>This text appears in the edit menu when an element reference in a group is selected.</summary>
		public static string CommandDeleteFromGroupText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteFromGroup.Text");
			}
		}
		/// <summary>This text appears in the edit menu when a group element is selected.</summary>
		public static string CommandDeleteGroupText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.DeleteGroup.Text");
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
		/// <summary>This text appears in the edit menu when reference to model notes are selected in the diagram.</summary>
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
		/// <summary>This text appears on the move role left/right when the fact type is binary.</summary>
		public static string CommandSwapRoleOrderText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Command.SwapRoleOrder.Text");
			}
		}
		/// <summary>The window title for the ORM diagram spy window</summary>
		public static string DiagramSpyWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DiagramSpy.WindowTitle");
			}
		}
		/// <summary>The header text shown on the diagram spy watermark when a diagram is not selected</summary>
		public static string DiagramSpyDiagramListStart
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DiagramSpy.DiagramListStart");
			}
		}
		/// <summary>The text shown on the diagram spy watermark when no diagrams are available</summary>
		public static string DiagramSpyNoSelection
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "DiagramSpy.NoSelection");
			}
		}
		/// <summary>Text used to label the readings header. Visible when extension property descriptors are defined for variables.</summary>
		public static string ModelReadingEditorColumnHeaderReadings
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.ColumnHeader.Readings");
			}
		}
		/// <summary>Text used to describe the transaction created when deleting an existing reading.</summary>
		public static string ModelReadingEditorDeleteReadingTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.DeleteReadingTransactionText");
			}
		}
		/// <summary>Text to display in the reading editor when the role has no roleplayer to substitute into the text.</summary>
		public static string ModelReadingEditorMissingRolePlayerText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MissingRolePlayerText");
			}
		}
		/// <summary>Text to place in the new item entry of the readings list in the reading editor tool window</summary>
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
		/// <summary>Text used to label the transaction created when a reading order is moved through the reading editor.</summary>
		public static string ModelReadingEditorMoveReadingOrder
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MoveReadingOrder");
			}
		}
		/// <summary>Text used to label the transaction created when a reading is moved through the reading editor.</summary>
		public static string ModelReadingEditorMoveReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReadingEditor.MoveReading");
			}
		}
		/// <summary>Text used to label the transaction created when the primary reading is changed through the reading editor.</summary>
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
		/// <summary>Text to place in an empty description editor tool window.</summary>
		public static string ModelDescriptionWindowEmptyDisplayText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelDescriptionWindow.EmptyDisplayText");
			}
		}
		/// <summary>Text to place in the title bar of the description editor tool window.</summary>
		public static string ModelDescriptionWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelDescriptionWindow.WindowTitle");
			}
		}
		/// <summary>Text to place in an empty notes editor tool window.</summary>
		public static string ModelNotesWindowEmptyDisplayText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelNotesWindow.EmptyDisplayText");
			}
		}
		/// <summary>Text to place in the title bar of the notes editor tool window.</summary>
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
		/// <summary>Text to place in the reading editor when the document view currently has a selection with no associated reading.</summary>
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
		/// <summary>Text used to describe the transaction when editing an instance.  {0}=ObjectType or FactType name, {1}=instance name</summary>
		public static string ModelSamplePopulationEditorEditInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.EditInstanceTransactionText");
			}
		}
		/// <summary>Text used to describe the editor exception when an objectification identifier is chosen that is already associated with another FactType instance.</summary>
		public static string ModelSamplePopulationEditorObjectifyingIdentifierAlreadyUsedExceptionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.ObjectifyingIdentifierAlreadyUsedExceptionText");
			}
		}
		/// <summary>Text used to describe the editor exception when an objectification identifier is chosen that conflicts with existing FactType instance relationships.</summary>
		public static string ModelSamplePopulationEditorObjectifyingIdentifierRelationshipConflictsExceptionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.ObjectifyingIdentifierRelationshipConflictsExceptionText");
			}
		}
		/// <summary>Text used to describe the editor exception when an implied element cannot be deleted because it would simply reappear.</summary>
		public static string ModelSamplePopulationEditorRefuseDeleteRoleInstanceExceptionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RefuseDeleteRoleInstanceExceptionText");
			}
		}
		/// <summary>Text used to describe the transaction when an identifier is attached to an objectified FactType instance. {0}=type name, {1}=identifier instance name, {2}=fact instance name.</summary>
		public static string ModelSamplePopulationEditorRelateObjectifiedInstanceIdentifierTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RelateObjectifiedInstanceIdentifierTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when removing a fact instance. {0}=instance name.</summary>
		public static string ModelSamplePopulationEditorRemoveFactInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RemoveFactInstanceTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when a reference an object instance is removed. {0}=type name {1}=instance name.</summary>
		public static string ModelSamplePopulationEditorRemoveObjectInstanceReferenceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RemoveObjectInstanceReferenceTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when removing an object instance. {0}=type name {1}=instance name.</summary>
		public static string ModelSamplePopulationEditorRemoveObjectInstanceTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.RemoveObjectInstanceTransactionText");
			}
		}
		/// <summary>Text used to describe the transaction when an identifier is separated from an objectified FactType instance. {0}=type name, {1}=identifier instance name, {2}=fact instance name.</summary>
		public static string ModelSamplePopulationEditorSeparateObjectifiedInstanceIdentifierTransactionText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.SeparateObjectifiedInstanceIdentifierTransactionText");
			}
		}
		/// <summary>Text to place in an empty sample population window.</summary>
		public static string ModelSamplePopulationEditorEmptyDisplayText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelSamplePopulationEditor.EmptyDisplayText");
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
		/// <summary>Column header for the first 'Abbreviation' column in the name generator abbreviations dialog</summary>
		public static string NameGeneratorAbbreviationsEditorAbbreviationColumnHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.AbbreviationColumnHeader");
			}
		}
		/// <summary>Column header for the first 'Element' column in the name generator abbreviations dialog</summary>
		public static string NameGeneratorAbbreviationsEditorElementColumnHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.ElementColumnHeader");
			}
		}
		/// <summary>Text displayed for new item dropdown in name generator abbreviations dialog.</summary>
		public static string NameGeneratorAbbreviationsEditorNewItemText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.NewItemText");
			}
		}
		/// <summary>Format string for the title of the name generator abbreviations dialog. Replacement fields: 0=NameGenerator name.</summary>
		public static string NameGeneratorAbbreviationsEditorTitleFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.TitleFormatString");
			}
		}
		/// <summary>Format string for the title of the name generator abbreviations dialog. Replacement fields: 0=NameGenerator name, 1=NameUsage name.</summary>
		public static string NameGeneratorAbbreviationsEditorTitleFormatStringWithNameUsage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.TitleFormatStringWithNameUsage");
			}
		}
		/// <summary>Transaction name when the name generator abbreviations dialog is committed. Displays in the Visual Studio undo and redo dropdowns.</summary>
		public static string NameGeneratorAbbreviationsEditorTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.Editor.TransactionName");
			}
		}
		/// <summary>Description shown in the properties window for the 'Abbreviations' property of a NameGenerator.</summary>
		public static string NameGeneratorAbbreviationsPropertyDescriptorDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.PropertyDescriptor.Description");
			}
		}
		/// <summary>Display name shown in the properties window for the custom 'Abbreviations' property of a NameGenerator.</summary>
		public static string NameGeneratorAbbreviationsPropertyDescriptorDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "NameGeneratorAbbreviations.PropertyDescriptor.DisplayName");
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
		/// <summary>The extra information that appears in the about box of Visual Studio</summary>
		public static string PackageProductDetails
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Package.ProductDetails");
			}
		}
		/// <summary>Text that names the reference mode in the displayed format string.</summary>
		public static string ModelReferenceModeEditorReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeName");
			}
		}
		/// <summary>Text that names the Entity Type in the displayed format string.</summary>
		public static string ModelReferenceModeEditorEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EntityTypeName");
			}
		}
		/// <summary>Text used to name the Reference Mode Kind column</summary>
		public static string ModelReferenceModeEditorReferenceModeKindHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ReferenceModeKindHeader");
			}
		}
		/// <summary>Text used to name the transaction the changes the name of a custom reference mode</summary>
		public static string ModelReferenceModeEditorChangeNameTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeNameTransaction");
			}
		}
		/// <summary>Text used to name the Custom Reference Modes column</summary>
		public static string ModelReferenceModeEditorCustomReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.CustomReferenceModesHeader");
			}
		}
		/// <summary>Text used to name the Intrinsic Reference Modes column</summary>
		public static string ModelReferenceModeEditorIntrinsicReferenceModesHeader
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.IntrinsicReferenceModesHeader");
			}
		}
		/// <summary>Abbreviated form of the entity type name</summary>
		public static string ModelReferenceModeEditorAbbreviatedEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedEntityTypeName");
			}
		}
		/// <summary>Abbreviated form of the reference mode name</summary>
		public static string ModelReferenceModeEditorAbbreviatedReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AbbreviatedReferenceModeName");
			}
		}
		/// <summary>Text used to name the transaction that adds a custom reference mode</summary>
		public static string ModelReferenceModeEditorAddCustomReferenceModeTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddCustomReferenceModeTransaction");
			}
		}
		/// <summary>Text to display the add a new row to the custom reference modes branch</summary>
		public static string ModelReferenceModeEditorAddNewRowText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.AddNewRowText");
			}
		}
		/// <summary>Text used to name the transaction that changes the format string.</summary>
		public static string ModelReferenceModeEditorChangeFormatStringTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeFormatStringTransaction");
			}
		}
		/// <summary>The text that displays the title of the editor window.</summary>
		public static string ModelReferenceModeEditorEditorWindowTitle
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.EditorWindowTitle");
			}
		}
		/// <summary>Exception message when the unique format string rule is violated for reference modes</summary>
		public static string ModelExceptionReferenceModeEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceUniqueFormatString");
			}
		}
		/// <summary>Exception message when the unique reference mode name rule is violated</summary>
		public static string ModelExceptionReferenceModeEnforceUniqueModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceUniqueModeName");
			}
		}
		/// <summary>Exception message when the unique format string rule is violated for reference mode kinds</summary>
		public static string ModelExceptionReferenceModeKindEnforceUniqueFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceModeKind.EnforceUniqueFormatString");
			}
		}
		/// <summary>Exception message when the pattern for a popular reference mode kind is violated.</summary>
		public static string ModelExceptionReferenceModeKindEnforcePopularPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceModeKind.EnforcePopularPattern");
			}
		}
		/// <summary>Exception message when the pattern for a unit based reference mode kind is violated.</summary>
		public static string ModelExceptionReferenceModeKindEnforceUnitBasedPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceModeKind.EnforceUnitBasedPattern");
			}
		}
		/// <summary>Exception message when the trivial general reference mode format string pattern is violated</summary>
		public static string ModelExceptionReferenceModeEnforceGeneralPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceGeneralPattern");
			}
		}
		/// <summary>Exception message when the popular reference mode format string pattern is violated</summary>
		public static string ModelExceptionReferenceModeEnforcePopularPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforcePopularPattern");
			}
		}
		/// <summary>Exception message when the unit based reference mode format string pattern is violated</summary>
		public static string ModelExceptionReferenceModeEnforceUnitBasedPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.EnforceUnitBasedPattern");
			}
		}
		/// <summary>Exception messege when atttempt is made to change the kind of an intrinsic reference mode</summary>
		public static string ModelExceptionReferenceModeIntrinsicRefModesDontChange
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.IntrinsicRefModesDontChange");
			}
		}
		/// <summary>Exception message when attempt is made to remove reference mode kind.</summary>
		public static string ModelExceptionReferenceModeReferenceModesKindNotEmpty
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.ReferenceMode.ReferenceModesKindNotEmpty");
			}
		}
		/// <summary>Text used to name the transaction that changes the reference mode Kind.</summary>
		public static string ModelReferenceModeEditorChangeReferenceModeKindTransaction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.ChangeReferenceModeKindTransaction");
			}
		}
		/// <summary>The reading for the predicate created by setting the ref mode.</summary>
		public static string ReferenceModePredicateReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateReading");
			}
		}
		/// <summary>The inverse reading for the predicate created by setting the ref mode.</summary>
		public static string ReferenceModePredicateInverseReading
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ReferenceMode.PredicateInverseReading");
			}
		}
		/// <summary>Column header text for the Format String column.</summary>
		public static string ModelReferenceModeEditorFormatStringColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.FormatStringColumn");
			}
		}
		/// <summary>Column header text for the Kind column.</summary>
		public static string ModelReferenceModeEditorKindColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.KindColumn");
			}
		}
		/// <summary>Column header text for the name column</summary>
		public static string ModelReferenceModeEditorNameColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelReferenceModeEditor.NameColumn");
			}
		}
		/// <summary>The format string for a missing role player in the FactEditor. The format string should not use parentheses, which are parsed by the FactEditor as reference modes.</summary>
		public static string FactEditorMissingRolePlayerFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FactEditor.MissingRolePlayerFormatString");
			}
		}
		/// <summary>The format string for a role player with a () qualifier in the FactEditor. The qualifier represents either a value type or a reference mode. Languages which should not rely on capitalization to delimit ObjectType names should put square braces around this format string.</summary>
		public static string FactEditorQualifiedRolePlayerFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FactEditor.QualifiedRolePlayerFormatString");
			}
		}
		/// <summary>The format string for a role player with no () qualifier in the FactEditor. Languages which should not rely on capitalization to delimit ObjectType names should put square braces around this format string.</summary>
		public static string FactEditorUnqualifiedRolePlayerFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "FactEditor.UnqualifiedRolePlayerFormatString");
			}
		}
		/// <summary>The caption of the Fact Editor Tool Window.</summary>
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
		/// <summary>Exception message when an attempt is made to change the SubtypeFact.ProvidesPreferredIdentifier property to false.</summary>
		public static string ModelExceptionSubtypeFactProvidesPreferredIdentifierInvalid
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeFact.ProvidesPreferredIdentifierInvalid");
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
		/// <summary>Exception message when an attempt is made to constraint a SubtypeMetaRole with a constraint other than the implicit uniqueness and mandatory constraints.</summary>
		public static string ModelExceptionSubtypeMetaRoleOnlyAllowsImplicitConstraints
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SubtypeMetaRole.OnlyAllowsImplicitConstraints");
			}
		}
		/// <summary>Exception message when an attempt is made to mix Roles and SupertypeMetaRoles in the same DisjunctiveMandatory constraint.</summary>
		public static string ModelExceptionSupertypeMetaRoleDisjunctiveMandatoryMustContainOnlySupertypeMetaRoles
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SupertypeMetaRole.DisjunctiveMandatoryMustContainOnlySupertypeMetaRoles");
			}
		}
		/// <summary>Exception message when an attempt is made to mix Roles and SupertypeMetaRoles in the same Exclusion constraint or to constraint SubtypeFacts with a multi-column Exclusion constraint.</summary>
		public static string ModelExceptionSupertypeMetaRoleExclusionMustBeSingleColumnAndContainOnlySupertypeMetaRoles
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SupertypeMetaRole.ExclusionMustBeSingleColumnAndContainOnlySupertypeMetaRoles");
			}
		}
		/// <summary>Exception message when an attempt is made to constraint a SupertypeMetaRole with a constraint other than the implicit uniqueness constraint, or disjunctive mandatory or exclusion constraints.</summary>
		public static string ModelExceptionSupertypeMetaRoleOnlyAllowsImplicitDisjunctiveMandatoryAndExclusionConstraints
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelException.SupertypeMetaRole.OnlyAllowsImplicitDisjunctiveMandatoryAndExclusionConstraints");
			}
		}
		/// <summary>The format string for the reference mode display text in the reference mode picker</summary>
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
		/// <summary>Text displayed in the text of the ReadingRequiresUserModificationError. {0}=fact name,{1}=model name,{2}=reading text</summary>
		public static string ModelErrorReadingRequiresUserModificationMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Reading.RequiresUserModification.Message");
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
		/// <summary>Model validation error text used when multiple groups with the same name are loaded into a model. Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorElementGroupingDuplicateNameError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ElementGrouping.DuplicateNameError.Message");
			}
		}
		/// <summary>Model validation error text used when an element is explicitly included by one grouptype and explicitly excluded by another. Field 0 is the group name, field 1 is the element name.</summary>
		public static string ModelErrorElementGroupingMembershipContradictionError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ElementGrouping.MembershipContradictionError.Message");
			}
		}
		/// <summary>Text displayed in the text of the FactTypeRequiresInternalUniquenessContraintError</summary>
		public static string ModelErrorFactTypeRequiresInternalUniquenessConstraintMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresInternalUniquenessConstraint.Message");
			}
		}
		/// <summary>Text displayed in the text of the FactTypeRequiresReadingError</summary>
		public static string ModelErrorFactTypeRequiresReadingMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.RequiresReading.Message");
			}
		}
		/// <summary>Text displayed for an unspecified data type</summary>
		public static string ModelErrorValueTypeDataTypeNotSpecifiedMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ValueType.DataTypeNotSpecified.Message");
			}
		}
		/// <summary>Text displayed when an objectified FactType instance is not associated with an external identifying instance. {0}=instance, {1}=entitytype, {2}=model.</summary>
		public static string ModelErrorEntityTypeInstanceObjectifiedInstanceRequiredMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.EntityTypeInstance.ObjectifiedInstanceRequired.Message");
			}
		}
		/// <summary>Text displayed when there aren't enough entity type role instances to completely fill a single entity population row. {0}=entitytype, {1}=instance, {2}=model.</summary>
		public static string ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.EntityTypeInstance.TooFewEntityTypeRoleInstances.Message");
			}
		}
		/// <summary>Text displayed when an external identifier is needed for a FactTypeInstance. {0}=instance {1}=model.</summary>
		public static string ModelErrorFactTypeInstanceObjectifyingInstanceRequiredMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactTypeInstance.ObjectifyingInstanceRequired.Message");
			}
		}
		/// <summary>Text displayed when there aren't enough fact type role instances to completely fill a single fact type population row. {0}=facttype {1}=model.</summary>
		public static string ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactTypeInstance.TooFewFactTypeRoleInstances.Message");
			}
		}
		/// <summary>Text displayed when the data type of the sample data doesn't match it's set data type. {0}=value {1}=valuetype {2}=model.</summary>
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
		/// <summary>The format string for a selected role when a role name is specified. {0}=ObjectType Name, {1}=Role Name</summary>
		public static string RoleSurveyNameFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Role.SurveyNameFormat");
			}
		}
		/// <summary>The name for a role name in place of an ObjectType name when no role player is specified.</summary>
		public static string RoleSurveyNameMissingRolePlayer
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "Role.SurveyNameMissingRolePlayer");
			}
		}
		/// <summary>Closing delimiter to contain one or more value ranges.</summary>
		public static string ValueConstraintDefinitionContainerCloseDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.DefinitionContainer.CloseDelimiter");
			}
		}
		/// <summary>Opening delimiter to contain one or more value ranges.</summary>
		public static string ValueConstraintDefinitionContainerOpenDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.DefinitionContainer.OpenDelimiter");
			}
		}
		/// <summary>One character with possible spaces for the closed lower bound of a value range.</summary>
		public static string ValueConstraintClosedInclusionLowerDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.ClosedInclusion.LowerDelimiter");
			}
		}
		/// <summary>One character with possible spaces for the closed upper bound of a value range.</summary>
		public static string ValueConstraintClosedInclusionUpperDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.ClosedInclusion.UpperDelimiter");
			}
		}
		/// <summary>One character with possible spaces for the open lower bound of a value range.</summary>
		public static string ValueConstraintOpenInclusionLowerDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.OpenInclusion.LowerDelimiter");
			}
		}
		/// <summary>One character with possible spaces for the open upper bound of a value range.</summary>
		public static string ValueConstraintOpenInclusionUpperDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.OpenInclusion.UpperDelimiter");
			}
		}
		/// <summary>The string delimiter character. Used to open and close strings. Doubling this character in a string results in the string in the final result.</summary>
		public static string ValueConstraintStringDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.StringDelimiter");
			}
		}
		/// <summary>String used to delimit the min- and max-values of a value range.</summary>
		public static string ValueConstraintValueDelimiter
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.ValueDelimiter");
			}
		}
		/// <summary>A regex pattern used for extracting parts of a single value range.
		/// This string contains a number of other resource values, escaped for use in a regex expression. The format string and replacements are:
		/// (?n)\s*((?&lt;MinClosedInclusion>{2})|(?&lt;MinOpenInclusion>{4}))?\s*(?&lt;MinValue>(((?!{1})[^{0}])+|({0}.*?{0})+)*)\s*(?&lt;RangeDelimiter>{1})?\s*(?&lt;MaxValue>(((?!({3}|{5}))[^{0}])+|({0}.*?{0})+)*)\s*((?&lt;MaxClosedInclusion>{3})|(?&lt;MaxOpenInclusion>{5}))?\s*
		/// {0}=ObjectModel.ValueConstraint.StringDelimiter
		/// {1}=ObjectModel.ValueConstraint.ValueDelimiter
		/// The remaining replacements are trimmed for leading/trailing whitespace
		/// {2}=ObjectModel.ValueConstraint.ClosedInclusion.LowerDelimiter
		/// {3}=ObjectModel.ValueConstraint.ClosedInclusion.UpperDelimiter
		/// {4}=ObjectModel.ValueConstraint.OpenInclusion.LowerDelimiter
		/// {5}=ObjectModel.ValueConstraint.OpenInclusion.UpperDelimiter</summary>
		public static string ValueConstraintValueRangeRegexPattern
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ObjectModel.ValueConstraint.ValueRangeRegexPattern");
			}
		}
		/// <summary>Model validation error text used when multiple recognized phrases with the same name are loaded into a model.Field 0 is the model name, field 1 is the element name.This is an uncommon condition that should only occur with a hand edit to a model file.</summary>
		public static string ModelErrorModelHasDuplicateRecognizedPhrases
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.RecognizedPhrase.DuplicateNameError.Message");
			}
		}
		/// <summary>The string used to display that a subtype object is derived.</summary>
		public static string ObjectTypeShapeDerivedSubtypeFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectTypeShape.DerivedSubtypeFormatString");
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
		/// <summary>The string used to display an objectified type that is a derived subtype.</summary>
		public static string ObjectifiedFactTypeNameShapeDerivedSubtypeFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.DerivedSubtypeFormatString");
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
		/// <summary>The string used to display an objectified type name for an derived subtype with a reference mode.</summary>
		public static string ObjectifiedFactTypeNameShapeRefModeDerivedSubtypeFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.RefModeDerivedSubtypeFormatString");
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
		/// <summary>The string used to display an objectified type name.</summary>
		public static string ObjectifiedFactTypeNameShapeStandardFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ObjectifiedFactTypeNameShape.StandardFormatString");
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
		/// <summary>The character to use as the object placeholder in a ReadingShape.</summary>
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
		/// <summary>The string used to display a reading with a non-primary order when the role is attached and the role player is duplicated in the reading. Replacement fields: {0}=role player name, {1}=displayed role index.</summary>
		public static string ReadingShapeAttachedDuplicateRoleDisplay
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ReadingShape.AttachedDuplicateRoleDisplay");
			}
		}
		/// <summary>The string used to display a reading with a non-primary order when the role is not attached. Replement fields: {0}=displayed role index</summary>
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
		/// <summary>The roles in a set constraint are in different fact types and a join path has not been provided. Replacement field {0} is context owner name.</summary>
		public static string ModelErrorConstraintRoleSequenceJoinPathRequiredError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ConstraintRoleSequence.JoinPathRequiredError.Text");
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
		/// <summary>Model validation error text displayed when a role does not have a specified role player. {0} is the (1-based) role number, {1} is the name of the fact, and {2} is the name of the model.</summary>
		public static string ModelErrorRolePlayerRequiredError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Role.RolePlayerRequired.Message");
			}
		}
		/// <summary>Text displayed in the text of the CompatibleSupertypesError. {0}=ObjectType name {1}=model name.</summary>
		public static string ModelErrorObjectTypeCompatibleSupertypesError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ObjectType.CompatibleSupertypesError.Message");
			}
		}
		/// <summary>Text displayed in the text of the PreferredIdentifierRequiresMandatoryError. {0}=ObjectType name {1}=model name</summary>
		public static string ModelErrorObjectTypePreferredIdentifierRequiresMandatoryError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.ObjectType.PreferredIdentifierRequiresMandatoryError.Message");
			}
		}
		/// <summary>An entity type must have a preferred reference scheme. Replacement field {0} is the entity type name and {1} is the model name.</summary>
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
		/// <summary>The frequency constraint minimum must be less than or equal to the maximum. {0}=constraint name {1}=model name</summary>
		public static string ModelErrorFrequencyConstraintMinMaxError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintMinMaxError.Text");
			}
		}
		/// <summary>The frequency constraint has minimum and maximum values of exactly one, should be represented as a uniqueness constraint. {0}=constraint name {1}=model name</summary>
		public static string ModelErrorFrequencyConstraintExactlyOneError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintExactlyOneError.Text");
			}
		}
		/// <summary>A frequency constraint with a minimum of 1 and an unbounded maximum is always true and should not be specified. {0}=constraint name {1}=model name</summary>
		public static string ModelErrorFrequencyConstraintNonRestrictiveRangeError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.Constraint.FrequencyConstraintNonRestrictiveRangeError.Text");
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
		/// <summary>Text used in the ImpliedInternalUniquenessConstraintError' {0}=fact type name {2}=model name</summary>
		public static string ModelErrorImpliedInternalUniquenessConstraintError
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ModelError.FactType.ImpliedInternalUniquenessConstraintError.Text");
			}
		}
		/// <summary>The message displayed if a merge operation is attempted but not all extensions are enabled.</summary>
		public static string CopyMergeExtensionRequiredMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.CopyMergeExtensionRequired.Message");
			}
		}
		/// <summary>The join separator to combine multiple missing extension names in the MessageBox.CopyMergeExtensionRequired.Message.</summary>
		public static string CopyMergeExtensionRequiredMessageJoinSeparator
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.CopyMergeExtensionRequired.Message.JoinSeparator");
			}
		}
		/// <summary>The message displayed if a selected item cannot be located in the model browser.</summary>
		public static string ElementNotInModelBrowserMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.ElementNotInModelBrowser.Message");
			}
		}
		/// <summary>The message for the auto-fix implied internal uniqueness constraint message box.</summary>
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
		/// <summary>The message shown when a file is upgraded to a new format. Replacement field 0 is the file name.</summary>
		public static string FileFormatUpgradeMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.FileFormatUpgrade.Message");
			}
		}
		/// <summary>The message shown when extensions are automatically removed from a file. Replacements: {0}=file name, {1}=list of unrecognized extensions.</summary>
		public static string UnrecognizedExtensionsStrippedMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.UnrecognizedExtensionsStripped.Message");
			}
		}
		/// <summary>The header for the message displayed if a set of extensions fails to correctly reload.</summary>
		public static string RevertExtensionsMessage
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "MessageBox.RevertExtensions.Message");
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
		/// <summary>Returned as the text for an unlimited display bound on the MaximumDisplayedValues and MaximumDisplayedColumns properties.</summary>
		public static string ValueConstraintShapeUnlimitedDisplayBoundText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "ValueConstraintShape.UnlimitedDisplayBound.Text");
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
		/// <summary>The context menu command text and transaction name for reordering diagram.</summary>
		public static string DiagramCommandReorderPages
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Diagram, "Diagram.Command.ReorderPages");
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
		/// <summary>Text used to display instructions when a load fails due to schema validation issues</summary>
		public static string SchemaValidationFailureInstructions
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "SchemaValidationFailure.Instructions");
			}
		}
		/// <summary>Error message for when a user attempts to modify the IsImplicitBooleanValueType property on a ValueType</summary>
		public static string ImplicitBooleanValueTypeRestriction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImplicitBooleanValueType.Restriction");
			}
		}
		/// <summary>Error message for when a user attempts to modify properties on an Implicit Boolean ValueType</summary>
		public static string ImplicitBooleanValueTypePropertyRestriction
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImplicitBooleanValueType.PropertyRestriction");
			}
		}
		/// <summary>The format string for naming an implicit boolean value type when there are no readings on the fact type.</summary>
		public static string ImplicitBooleanValueTypeNoReadingFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "ImplicitBooleanValueType.NoReadingFormatString");
			}
		}
		/// <summary>Text used in the dropdown editor and type converter for the frequency constraint max value. Correspond to the 0 value in the object model.</summary>
		public static string FrequencyConstraintUnboundedMaxValueText
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "FrequencyConstraint.UnboundedMaxValueText");
			}
		}
		public static string RoleProxyTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.ObjectModel, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.DisplayName");
			}
		}
		/// <summary>The name of the transaction that converts a frequency constraint to a uniqueness.</summary>
		public static string ConvertFrequencyToUniquenessTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "FrequencyConstraint.ConvertToUniquenessConstraint.TransactionName");
			}
		}
		/// <summary>Description for target HtmlReport customizations. Displays in the verbalization customizations dropdown in the options page.</summary>
		public static string VerbalizationTargetHtmlReportDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "VerbalizationTarget.HtmlReport.DisplayName");
			}
		}
		/// <summary>Description for target HtmlReport command name. Displays in the GenerateReport submenu on the context menu.</summary>
		public static string VerbalizationTargetHtmlReportCommandName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "VerbalizationTarget.HtmlReport.CommandName");
			}
		}
		/// <summary>Description for target VerbalizationBrowser customizations. Displays in the verbalization customizations dropdown in the options page.</summary>
		public static string VerbalizationTargetVerbalizationBrowserDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.Model, "VerbalizationTarget.VerbalizationBrowser.DisplayName");
			}
		}
	}
	#endregion // ResourceStrings class
}
