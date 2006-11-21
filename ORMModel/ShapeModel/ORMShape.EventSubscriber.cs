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
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeDomainModel : IORMModelEventSubscriber
	{
		#region IORMModelEventSubscriber Implementation
		/// <summary>
		/// Implements IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers
		/// </summary>
		protected static void ManagePreLoadModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
		}
		void IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
			ManagePreLoadModelingEventHandlers(eventManager, addHandlers);
		}
		/// <summary>
		/// Implements IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers
		/// </summary>
		protected void ManagePostLoadModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
			Store store = Store;
			ORMBaseShape.ManageEventHandlers(store, eventManager, addHandlers);
			ReadingShape.ManageEventHandlers(store, eventManager, addHandlers);
			ExternalConstraintShape.ManageEventHandlers(store, eventManager, addHandlers);
			RolePlayerLink.ManageEventHandlers(store, eventManager, addHandlers);
			ObjectTypeShape.ManageEventHandlers(store, eventManager, addHandlers);
			FactTypeShape.ManageEventHandlers(store, eventManager, addHandlers);
			SubtypeLink.ManageEventHandlers(store, eventManager, addHandlers);
		}
		void IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
			ManagePostLoadModelingEventHandlers(eventManager, addHandlers);
		}
		/// <summary>
		/// Implements IORMModelEvenSubscriber.ManageSurveyQuestionModelingEventHandlers
		/// </summary>
		protected static void ManageSurveyQuestionModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
			//currently unimplemented as the survey doesn't care about shape model changes
		}
		void IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers(SafeEventManager eventManager, bool addHandlers)
		{
			ManageSurveyQuestionModelingEventHandlers(eventManager, addHandlers);
		}
		#endregion // IORMModelEventSubscriber Implementation
	}
}
