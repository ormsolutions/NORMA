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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ORMShapeDomainModel : IModelingEventSubscriber
	{
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>.
		/// </summary>
		protected void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			// UNDONE: If we delay attach user interface events (possible in the future for
			// external model scenarios), then we need to check ModelStateEvents here and
			// be more precise in which events are attached that affect model state such as
			// calculated shape size. Currently, this is only called without 'UserInterfaceEvents'
			// for unit-testing.
			if ((EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.UserInterfaceEvents) == (reasons & (EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.UserInterfaceEvents)))
			{
				Store store = Store;
				ORMDiagram.ManageEventHandlers(store, eventManager, action);
				ORMBaseShape.ManageEventHandlers(store, eventManager, action);
				ReadingShape.ManageEventHandlers(store, eventManager, action);
				ExternalConstraintShape.ManageEventHandlers(store, eventManager, action);
				RolePlayerLink.ManageEventHandlers(store, eventManager, action);
				ObjectTypeShape.ManageEventHandlers(store, eventManager, action);
				ORMBaseBinaryLinkShape.ManageEventHandlers(store, eventManager, action);
				FactTypeShape.ManageEventHandlers(store, eventManager, action);
				SubtypeLink.ManageEventHandlers(store, eventManager, action);
			}

			if (0 != (reasons & EventSubscriberReasons.DocumentLoaded))
			{
				IORMToolServices services;
				IORMExtendableElementService extendableElementService;
				if (null != (services = Store as IORMToolServices) &&
					null != (extendableElementService = services.ExtendableElementService))
				{
					extendableElementService.RegisterExtensionRoles(new Guid[] { ORMDiagramHasExtensionElement.ExtensionDomainRoleId, ORMBaseShapeHasExtensionElement.ExtensionDomainRoleId });
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
