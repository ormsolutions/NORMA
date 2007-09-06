
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

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
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
		/// <summary>The description for the ReferenceModeNamingCustomFormat property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the ReferenceModeNamingCustomFormat property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferenceModeNamingCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the ReferenceModeNaming property on an ObjectType. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the ReferenceModeNaming property on an ObjectType. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the ReferenceModeNaming property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingNamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the PopularReferenceModeNamingCustomFormat property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingPopularCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the PopularReferenceModeNamingCustomFormat property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingPopularCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PopularReferenceModeNamingCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingPopularCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the PopularReferenceModeNaming property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingPopularNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the PopularReferenceModeNaming property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingPopularNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the PopularReferenceModeNaming property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingPopularNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingPopularNamingChoiceProperty.TransactionName");
			}
		}
		/// <summary>The description for the UnitBasedReferenceModeNamingCustomFormat property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingUnitBasedCustomFormatPropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedCustomFormatProperty.Description");
			}
		}
		/// <summary>The display name for the UnitBasedReferenceModeNamingCustomFormat property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingUnitBasedCustomFormatPropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedCustomFormatProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the UnitBasedReferenceModeNamingCustomFormat property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingUnitBasedCustomFormatPropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedCustomFormatProperty.TransactionName");
			}
		}
		/// <summary>The description for the UnitBasedReferenceModeNaming property on an ORMModel. Displays as a detailed description in the Properties Window.</summary>
		public static string ReferenceModeNamingUnitBasedNamingChoicePropertyDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedNamingChoiceProperty.Description");
			}
		}
		/// <summary>The display name for the UnitBasedReferenceModeNaming property on an ORMModel. Displays as the name of a property in the Properties Window.</summary>
		public static string ReferenceModeNamingUnitBasedNamingChoicePropertyDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedNamingChoiceProperty.DisplayName");
			}
		}
		/// <summary>The format string to use when the UnitBasedReferenceModeNaming property is modified. Shown in the undo dropdown in VS.</summary>
		public static string ReferenceModeNamingUnitBasedNamingChoicePropertyTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNamingUnitBasedNamingChoiceProperty.TransactionName");
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
		/// <summary>Custom formatting for a popular reference mode defaults to {EntityType}{ReferenceMode}.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatPopular
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.Popular");
			}
		}
		/// <summary>Custom formatting for a unit-base reference mode defaults to {ReferenceMode}{EntityType}.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatUnitBased
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.UnitBased");
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
