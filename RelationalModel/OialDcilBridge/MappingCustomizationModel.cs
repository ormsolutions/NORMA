#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using Neumont.Tools.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	partial class MappingCustomizationModel
	{
		// Put this first so that MappingCustomizationModel.resx binds the resource name to the correct class
	}
	public partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : IORMModelEventSubscriber
	{
		#region IORMModelEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers"/>
		/// </summary>
		protected void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			IORMPropertyProviderService propertyProvider = ((IORMToolServices)Store).PropertyProviderService;
			propertyProvider.AddOrRemovePropertyProvider<FactType>(AssimilationMapping.PopulateAssimilationMappingExtensionProperties, true, action);
		}
		void IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			ManagePostLoadModelingEventHandlers(eventManager, action);
		}
		/// <summary>
		/// Implements <see cref="IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers"/>
		/// </summary>
		protected static void ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Nothing to do
		}
		void IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			ManagePreLoadModelingEventHandlers(eventManager, action);
		}
		/// <summary>
		/// Implements <see cref="IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers"/>
		/// </summary>
		protected static void ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Nothing to do
		}
		void IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			ManageSurveyQuestionModelingEventHandlers(eventManager, action);
		}
		#endregion // IORMModelEventSubscriber Implementation
	}
}
