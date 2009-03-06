
// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region ResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class ResourceStrings
	{
		/// <summary>The format string to use when the assimilation absorption choice property is modified. Shown in the undo dropdown in VS.</summary>
		public static string AbsorptionChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AbsorptionChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the AbsorptionChoice property. Displays as a detailed description in the Properties Window.</summary>
		public static string AbsorptionChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AbsorptionChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the AbsorptionChoice property on a FactType. Displays as the name of a property in the Properties Window.</summary>
		public static string AbsorptionChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AbsorptionChoiceProperty.DisplayName");
			}
		}
		/// <summary>The exception message used when an attempt is made to partition a pattern that does not support partitioning.</summary>
		public static string AssimilationMappingInvalidPatternForPartitionException
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AssimilationMapping.InvalidPatternForPartitionException");
			}
		}
		/// <summary>The exception message used when an attempt is made to absorb an assimilation in an incorrect pattern.</summary>
		public static string AssimilationMappingInvalidSeparationPatternForAbsorbException
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AssimilationMapping.InvalidSeparationPatternForAbsorbException");
			}
		}
		/// <summary>The exception message used when an attempt is made to absorb a partitioned object type.</summary>
		public static string AssimilationMappingInvalidPartitionPatternForAbsorbException
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "AssimilationMapping.InvalidPartitionPatternForAbsorbException");
			}
		}
		/// <summary>The category for relational mapping customizations. Displays as a grouping category in the Properties Window.</summary>
		public static string MappingCustomizationPropertyCategory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "MappingCustomizationProperty.Category");
			}
		}
		/// <summary>The description for the AbsorptionChoice property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ObjectTypeAbsorptionChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ObjectTypeAbsorptionChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the AbsorptionChoice property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ObjectTypeAbsorptionChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ObjectTypeAbsorptionChoiceProperty.DisplayName");
			}
		}
		/// <summary>The name of the identifier column for a ValueType mapped to its own top-level table.</summary>
		public static string NameGenerationValueTypeValueColumn
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "NameGeneration.ValueTypeValueColumn");
			}
		}
		/// <summary>The displayed form of the ModelDefault value in the dropdown list when the ModelDefault is CustomFormat</summary>
		public static string ReferenceModeNamingCurrentModelDefaultCustomFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentModelDefault.CustomFormat");
			}
		}
		/// <summary>The displayed form of the ModelDefault value in the dropdown list when the ModelDefault is EntityTypeName</summary>
		public static string ReferenceModeNamingCurrentModelDefaultEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentModelDefault.EntityTypeName");
			}
		}
		/// <summary>The displayed form of the ModelDefault value in the dropdown list when the ModelDefault is ReferenceModeName</summary>
		public static string ReferenceModeNamingCurrentModelDefaultReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentModelDefault.ReferenceModeName");
			}
		}
		/// <summary>The displayed form of the ModelDefault value in the dropdown list when the ModelDefault is ValueTypeName</summary>
		public static string ReferenceModeNamingCurrentModelDefaultValueTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentModelDefault.ValueTypeName");
			}
		}
		/// <summary>The format string for the dropdown form of the current naming choice when CustomFormat is current. {0} is replaced with the customized name for the current selection.</summary>
		public static string ReferenceModeNamingCurrentFormatStringCustomFormat
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentFormatString.CustomFormat");
			}
		}
		/// <summary>The format string for the dropdown form of the current naming choice when EntityTypeName is current. {0} is replaced with the name of the EntityType for the current selection.</summary>
		public static string ReferenceModeNamingCurrentFormatStringEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentFormatString.EntityTypeName");
			}
		}
		/// <summary>The format string for the dropdown form of the current naming choice when ReferenceModeName is current. {0} is replaced with the name of the ReferenceMode for the current selection.</summary>
		public static string ReferenceModeNamingCurrentFormatStringReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentFormatString.ReferenceModeName");
			}
		}
		/// <summary>The format string for the dropdown form of the current naming choice when ValueTypeName is current. {0} is replaced with the name of the ValueType for the current selection.</summary>
		public static string ReferenceModeNamingCurrentFormatStringValueTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CurrentFormatString.ValueTypeName");
			}
		}
		/// <summary>The description for the ReferencedEntityTypeCustomFormat property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the ReferencedEntityTypeCustomFormat property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferencedEntityTypeCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.CustomFormatProperty.TransactionName");
			}
		}
		/// <summary>Value displayed for any modified state of the grouped reference mode customization options.</summary>
		public static string ReferenceModeNamingDefaultGroupDisplayValueCustom
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.DisplayValue.Custom");
			}
		}
		/// <summary>Value displayed for the default state of the  grouped reference mode customization options.</summary>
		public static string ReferenceModeNamingDefaultGroupDisplayValueDefault
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.DisplayValue.Default");
			}
		}
		/// <summary>The description for the GeneralReferenceModeNames property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupGeneralDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.General.Description");
			}
		}
		/// <summary>The display name for the GeneralBasedReferenceModeNames property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupGeneralDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.General.DisplayName");
			}
		}
		/// <summary>The description for the PopularReferenceModeNames property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupPopularDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.Popular.Description");
			}
		}
		/// <summary>The display name for the PopularReferenceModeNames property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupPopularDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.Popular.DisplayName");
			}
		}
		/// <summary>The description for the UnitBasedReferenceModeNames property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupUnitBasedDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.UnitBased.Description");
			}
		}
		/// <summary>The display name for the UnitBasedReferenceModeNames property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultGroupUnitBasedDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultGroup.UnitBased.DisplayName");
			}
		}
		/// <summary>The description for the ReferencedEntityTypeName property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.NamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the ReferencedEntityTypeName property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.NamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferencedEntityTypeName property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.NamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the PrimaryIdentifierCustomFormat property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the PrimaryIdentifierCustomFormat property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PrimaryIdentifierCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the PrimaryIdentifierName property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the PrimaryIdentifierName property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PrimaryIdentifierName property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingPrimaryIdentifierNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the ReferencedEntityTypeCustomFormat property on ReferenceModeType-scoped naming options on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the ReferencedEntityTypeCustomFormat property on ReferenceModeType-scoped naming options on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferencedEntityTypeCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the ReferencedEntityTypeName property on ReferenceModeType-scoped naming options on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the ReferencedEntityTypeName property on ReferenceModeType-scoped naming options on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferencedEntityTypeName property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingDefaultNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultNamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the PrimaryIdentifierCustomFormat property on ReferenceModeType-scoped naming options on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the PrimaryIdentifierCustomFormat property on ReferenceModeType-scoped naming options on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PrimaryIdentifierCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the PrimaryIdentifierName property on ReferenceModeType-scoped naming options on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the PrimaryIdentifierName property on ReferenceModeType-scoped naming options on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PrimaryIdentifierName property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The short form of the parseable replacement field for an EntityTypeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldShortFormEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementFieldShortForm.EntityTypeName");
			}
		}
		/// <summary>The short form of the parseable replacement field for a ReferenceModeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldShortFormReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementFieldShortForm.ReferenceModeName");
			}
		}
		/// <summary>The short form of the parseable replacement field for a ValueTypeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldShortFormValueTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementFieldShortForm.ValueTypeName");
			}
		}
		/// <summary>The replacement field displayed to the user for the EntityTypeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldEntityTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementField.EntityTypeName");
			}
		}
		/// <summary>The replacement field displayed to the user for the ReferenceModeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldReferenceModeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementField.ReferenceModeName");
			}
		}
		/// <summary>The replacement field displayed to the user for the ValueTypeName in the CustomFormat editor.</summary>
		public static string ReferenceModeNamingDisplayedReplacementFieldValueTypeName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DisplayedReplacementField.ValueTypeName");
			}
		}
		/// <summary>Custom formatting for a general reference mode defaults to {EntityType}{ReferenceMode}.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatGeneral
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.General");
			}
		}
		/// <summary>Custom formatting for a popular reference mode defaults to {EntityType}{ReferenceMode}.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatPopular
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.Popular");
			}
		}
		/// <summary>Custom formatting for a unit-based reference mode defaults to {ReferenceMode}{EntityType}.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatUnitBased
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.UnitBased");
			}
		}
		/// <summary>Custom formatting for a general reference mode used as a primary identifier defaults to {EntityType}{ReferenceMode}.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatGeneral
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.General");
			}
		}
		/// <summary>Custom formatting for a popular reference mode used as a primary identifier defaults to {EntityType}{ReferenceMode}.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPopular
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.Popular");
			}
		}
		/// <summary>Custom formatting for a unit-based reference mode used as a primary identifier defaults to {ReferenceMode}{EntityType}.</summary>
		public static string ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatUnitBased
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.UnitBased");
			}
		}
		/// <summary>The text for the exception thrown when an attempt is made to enter a default custom format with no replacement fields.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatInvalidDefaultCustomFormatException
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.InvalidDefaultCustomFormatException");
			}
		}
	}
	#endregion // ResourceStrings class
}
