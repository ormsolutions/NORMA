﻿
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
	}
	#endregion // ResourceStrings class
}
