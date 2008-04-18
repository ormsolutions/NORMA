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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ShapeModel
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
				ORMBaseShape.ManageEventHandlers(store, eventManager, action);
				ReadingShape.ManageEventHandlers(store, eventManager, action);
				ExternalConstraintShape.ManageEventHandlers(store, eventManager, action);
				RolePlayerLink.ManageEventHandlers(store, eventManager, action);
				ObjectTypeShape.ManageEventHandlers(store, eventManager, action);
				ORMBaseBinaryLinkShape.ManageEventHandlers(store, eventManager, action);
				FactTypeShape.ManageEventHandlers(store, eventManager, action);
				SubtypeLink.ManageEventHandlers(store, eventManager, action);
			}
		}
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			ManageModelingEventHandlers(eventManager, reasons, action);
		}
		#endregion // IModelingEventSubscriber Implementation
	}
}
