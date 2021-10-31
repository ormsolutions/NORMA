#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	partial class RelationalReferenceModeNaming
	{
		#region Base overrides
		/// <summary>
		/// Standard override
		/// </summary>
		public override ObjectType ResolvedObjectType
		{
			get
			{
				return ObjectType;
			}
			set
			{
				ObjectType = value;
			}
		}
		/// <summary>
		/// Attach this new instance to the <see cref="MappingCustomizationModel"/> that owns these settings.
		/// </summary>
		public override void AttachDynamicInstance(Type namingLinkType)
		{
			this.Model = MappingCustomizationModel.GetMappingCustomizationModel(Store, true);
		}
		/// <summary>
		/// Retrieve the default naming instance from the ORMModel
		/// </summary>
		/// <returns></returns>
		public override DefaultReferenceModeNaming ResolveDefaultReferenceModeNaming()
		{
			ObjectType objectType;
			IReferenceModePattern referenceModePattern;
			if (null != (objectType = ResolvedObjectType) &&
				null != (referenceModePattern = objectType.ReferenceModePattern))
			{
				ReferenceModeType matchType = referenceModePattern.ReferenceModeType;
				foreach (RelationalDefaultReferenceModeNaming defaultNaming in DefaultReferenceModeNamingCustomizesORMModel.GetDefaultReferenceModeNamingCollection(ResolvedObjectType.ResolvedModel))
				{
					if (defaultNaming.ReferenceModeTargetKind == matchType)
					{
						return defaultNaming;
					}
				}
			}
			return null;
		}
		#endregion // Base overrides
		#region Extension property provider callbacks
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="ORMModel"/>
		/// </summary>
		public static void PopulateDefaultReferenceModeNamingExtensionPropertiesOnORMModel(object extendableElement, PropertyDescriptorCollection properties)
		{
			ORMModel model;
			if (null != (model = extendableElement as ORMModel) &&
				!model.IsDeleted)
			{
				properties.Add(GeneralGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel>.Instance);
				properties.Add(PopularGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel >.Instance);
				properties.Add(UnitBasedGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel>.Instance);
			}
		}
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="RelationalNameGenerator"/>
		/// </summary>
		public static void PopulateDefaultReferenceModeNamingExtensionPropertiesOnColumnNameGenerator(object extendableElement, PropertyDescriptorCollection properties)
		{
			RelationalNameGenerator generator;
			if (null != (generator = extendableElement as RelationalNameGenerator) &&
				!generator.IsDeleted &&
				generator.NameUsageType == typeof(ColumnNameUsage))
			{
				properties.Add(GeneralGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel>.Instance);
				properties.Add(PopularGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel>.Instance);
				properties.Add(UnitBasedGroupingPropertyDescriptor<RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel, ORMModel>.Instance);
			}
		}
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="ObjectType"/>
		/// </summary>
		public static void PopulateReferenceModeNamingExtensionPropertiesOnObjectType(object extendableElement, PropertyDescriptorCollection properties)
		{
			ObjectType objectType;
			IReferenceModePattern referenceMode;
			if (null != (objectType = extendableElement as ObjectType) &&
				!objectType.IsDeleted &&
				null != (referenceMode = objectType.ReferenceModePattern))
			{
				bool extensionSpecific = HasMultipleObjectTypeExtensionSources(objectType.Store);
				ReferenceModeType referenceModeType = referenceMode.ReferenceModeType;
				properties.Add(extensionSpecific ?
					ReferenceToEntityTypeNamingChoicePropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.ExtensionSpecificInstance :
					ReferenceToEntityTypeNamingChoicePropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.Instance);
				ReferenceModeNaming naming = InstanceResolver<RelationalDefaultReferenceModeNaming>.ResolveReferenceModeNamingFromObjectType(null, objectType, false, typeof(ReferenceModeNamingCustomizesObjectType));
				DefaultReferenceModeNaming defaultNaming = null;

				ReferenceModeNamingChoice currentChoice = GetNamingChoice(naming, ReferenceModeNamingUse.ReferenceToEntityType);
				if (currentChoice == ReferenceModeNamingChoice.CustomFormat ||
					(currentChoice == ReferenceModeNamingChoice.ModelDefault && GetNamingChoiceFromDefault(defaultNaming = InstanceResolver<RelationalDefaultReferenceModeNaming>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultReferenceModeNamingCustomizesORMModel), typeof(ReferenceModeNamingCustomizesObjectType)), referenceModeType, ReferenceModeNamingUse.ReferenceToEntityType) == EffectiveReferenceModeNamingChoice.CustomFormat))
				{
					properties.Add(extensionSpecific ?
						ReferenceToEntityTypeCustomFormatPropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.ExtensionSpecificInstance :
						ReferenceToEntityTypeCustomFormatPropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.Instance);
				}
				properties.Add(extensionSpecific ?
					PrimaryIdentifierNamingChoicePropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.ExtensionSpecificInstance :
					PrimaryIdentifierNamingChoicePropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.Instance);

				currentChoice = GetNamingChoice(naming, ReferenceModeNamingUse.PrimaryIdentifier);
				if (currentChoice == ReferenceModeNamingChoice.CustomFormat ||
					(currentChoice == ReferenceModeNamingChoice.ModelDefault && GetNamingChoiceFromDefault(defaultNaming ?? (defaultNaming = InstanceResolver<RelationalDefaultReferenceModeNaming>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultReferenceModeNamingCustomizesORMModel), typeof(ReferenceModeNamingCustomizesObjectType))), referenceModeType, ReferenceModeNamingUse.PrimaryIdentifier) == EffectiveReferenceModeNamingChoice.CustomFormat))
				{
					properties.Add(extensionSpecific ?
						PrimaryIdentifierCustomFormatPropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.ExtensionSpecificInstance :
						PrimaryIdentifierCustomFormatPropertyDescriptor<RelationalReferenceModeNaming, ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>.Instance);
				}
			}
		}
		#endregion // Extension property provider callbacks
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DefaultReferenceModeNamingFixupListener();
			}
		}
		/// <summary>
		/// Validation DefaultReferenceModeNaming options
		/// </summary>
		private sealed class DefaultReferenceModeNamingFixupListener : DeserializationFixupListener<RelationalDefaultReferenceModeNaming>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public DefaultReferenceModeNamingFixupListener()
				: base((int)ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase.ValidateCustomizationOptions)
			{
			}
			/// <summary>
			/// Validate AssimilationMapping options
			/// </summary>
			protected sealed override void ProcessElement(RelationalDefaultReferenceModeNaming element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					if (string.IsNullOrEmpty(element.CustomFormat))
					{
						element.CustomFormat = GetDefaultCustomFormat<MappingCustomizationModel>(element.ReferenceModeTargetKind, ReferenceModeNamingUse.ReferenceToEntityType);
					}
					if (string.IsNullOrEmpty(element.PrimaryIdentifierCustomFormat))
					{
						element.PrimaryIdentifierCustomFormat = GetDefaultCustomFormat<MappingCustomizationModel>(element.ReferenceModeTargetKind, ReferenceModeNamingUse.PrimaryIdentifier);
					}
				}
			}
		}
		#endregion // Deserialization Fixup
	}
	[DefaultReferenceModeNamingOwner("GetRelationalDefaultReferenceModeNaming", "GetRelationalReferenceModeNamingFromObjectType", "GetRelationalDefaultReferenceModeNamingFromObjectType", "GetContextDefaultReferenceModeNaming", "GetContextReferenceModeNaming", "GetPropertyDescriptorSuffix")]
	partial class RelationalDefaultReferenceModeNaming
	{
		#region Owner resolution helpers
		private static DefaultReferenceModeNaming GetRelationalDefaultReferenceModeNaming(object component, ReferenceModeType referenceModeType, bool findNearest, Type ownerLinkType)
		{
			RelationalDefaultReferenceModeNaming retVal = null;
			ModelElement element;
			if (null != (element = EditorUtility.ResolveContextInstance(component, false) as ModelElement))
			{
				if (null == (retVal = element as RelationalDefaultReferenceModeNaming))
				{
					foreach (RelationalDefaultReferenceModeNaming defaultNaming in element.Store.ElementDirectory.FindElements<RelationalDefaultReferenceModeNaming>(false))
					{
						if (defaultNaming.ReferenceModeTargetKind == referenceModeType)
						{
							return defaultNaming;
						}
					}
				}
			}
			return retVal;
		} 
		private static ReferenceModeNaming GetRelationalReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, bool findNearest, Type namingLinkType)
		{
			return ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
		}
		private static DefaultReferenceModeNaming GetRelationalDefaultReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, Type ownerLinkType, Type namingLinkType)
		{
			DefaultReferenceModeNaming retVal = null;
			IReferenceModePattern referenceMode;
			if (objectType != null &&
				null != (referenceMode = objectType.ReferenceModePattern))
			{
				ReferenceModeType matchType = referenceMode.ReferenceModeType;
				foreach (RelationalDefaultReferenceModeNaming defaultNaming in objectType.Store.ElementDirectory.FindElements<RelationalDefaultReferenceModeNaming>(false))
				{
					if (defaultNaming.ReferenceModeTargetKind == matchType)
					{
						retVal = defaultNaming;
						break;
					}
				}
			}
			return retVal;
		}
		private static DefaultReferenceModeNaming GetContextDefaultReferenceModeNaming(object component, ReferenceModeType referenceModeType, Type defaultNamingLinkType)
		{
			// This implementation has only one level of defaults.
			return null;
		}
		private static ReferenceModeNaming GetContextReferenceModeNaming(ObjectType objectType, Type namingLinkType)
		{
			return null;
		}
		private static string GetPropertyDescriptorSuffix(Type defaultNamingLinkType, Type namingLinkType)
		{
			return "_Relational";
		}
		#endregion // Owner resolutions helpers
		#region Base overrides
		/// <summary>
		/// Standard override
		/// </summary>
		public override void AttachDynamicInstance(ModelElement contextElement, Type defaultNamingLinkType)
		{
			Store store = Store;
			this.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
			this.ORMModel = store.ElementDirectory.FindElements<ORMModel>(false)[0];
		}
		#endregion // Base overrides
	}
}
