#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	partial class MappingCustomizationModel
	{
		#region Static helper functions
		// Put this first so that MappingCustomizationModel.resx binds the resource name to the correct class
		/// <summary>
		/// Find or create the <see cref="MappingCustomizationModel"/> for the given <paramref name="store"/>
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		/// <param name="forceCreate">Set to <see langword="true"/> to force a new customization model to
		/// be created if one does not already exist.</param>
		public static MappingCustomizationModel GetMappingCustomizationModel(Store store, bool forceCreate)
		{
			MappingCustomizationModel model = null;
			foreach (MappingCustomizationModel findModel in store.ElementDirectory.FindElements<MappingCustomizationModel>())
			{
				model = findModel;
				break;
			}
			if (model == null && forceCreate)
			{
				model = new MappingCustomizationModel(store);
			}
			return model;
		}
		#endregion // Static helper functions
	}
	public partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : IModelingEventSubscriber
	{
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>
		/// </summary>
		protected void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if (0 != (reasons & EventSubscriberReasons.DocumentLoaded))
			{
				IPropertyProviderService propertyProvider = ((IFrameworkServices)Store).PropertyProviderService;
				propertyProvider.AddOrRemovePropertyProvider(typeof(FactType), AssimilationMapping.PopulateAssimilationMappingExtensionProperties, true, action);
				propertyProvider.AddOrRemovePropertyProvider(typeof(ObjectType), AssimilationMapping.PopulateObjectTypeAbsorptionExtensionProperties, false, action);
				propertyProvider.AddOrRemovePropertyProvider(typeof(ObjectType), ReferenceModeNaming.PopulateReferenceModeNamingExtensionProperties, false, action);
				propertyProvider.AddOrRemovePropertyProvider(typeof(ORMModel), ReferenceModeNaming.PopulateDefaultReferenceModeNamingExtensionPropertiesOnORMModel, false, action);
				propertyProvider.AddOrRemovePropertyProvider(typeof(RelationalNameGenerator), ReferenceModeNaming.PopulateDefaultReferenceModeNamingExtensionPropertiesOnColumnNameGenerator, false, action);

				if (0 != (reasons & EventSubscriberReasons.ModelStateEvents))
				{
					SchemaCustomization.ManageModelingEventHandlers(eventManager, this.Store, action);
				}
			}
		}
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			ManageModelingEventHandlers(eventManager, reasons, action);
		}
		#endregion // IModelingEventSubscriber Implementation
	}
}
