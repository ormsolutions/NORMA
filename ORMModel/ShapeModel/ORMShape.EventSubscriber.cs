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
		/// Implements <see cref="IModelingEventSubscriber.ManagePreLoadModelingEventHandlers"/>.
		/// This implementation does nothing and does not need to be called.
		/// </summary>
		void IModelingEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
		}
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManagePostLoadModelingEventHandlers"/>.
		/// </summary>
		protected void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			Store store = Store;
			ORMBaseShape.ManageEventHandlers(store, eventManager, action);
			ReadingShape.ManageEventHandlers(store, eventManager, action);
			ExternalConstraintShape.ManageEventHandlers(store, eventManager, action);
			RolePlayerLink.ManageEventHandlers(store, eventManager, action);
			ObjectTypeShape.ManageEventHandlers(store, eventManager, action);
			FactTypeShape.ManageEventHandlers(store, eventManager, action);
			SubtypeLink.ManageEventHandlers(store, eventManager, action);
		}
		void IModelingEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			ManagePostLoadModelingEventHandlers(eventManager, action);
		}
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageSurveyQuestionModelingEventHandlers"/>.
		/// This implementation does nothing and does not need to be called.
		/// </summary>
		void IModelingEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			//currently unimplemented as the survey doesn't care about shape model changes
		}
		#endregion // IModelingEventSubscriber Implementation
	}
}
