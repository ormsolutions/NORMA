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

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeModel : IORMModelEventSubscriber
	{
		#region IORMModelEventSubscriber Implementation
		/// <summary>
		/// Implements IORMModelEventSubscriber.AddPreLoadModelingEventHandlers
		/// </summary>
		protected static void AddPreLoadModelingEventHandlers()
		{
		}
		void IORMModelEventSubscriber.AddPreLoadModelingEventHandlers()
		{
			AddPreLoadModelingEventHandlers();
		}
		/// <summary>
		/// Implements IORMModelEventSubscriber.AddPostLoadModelingEventHandlers
		/// </summary>
		protected void AddPostLoadModelingEventHandlers()
		{
			Store store = Store;
			ORMBaseShape.AttachEventHandlers(store);
			ReadingShape.AttachEventHandlers(store);
			ExternalConstraintShape.AttachEventHandlers(store);
			RolePlayerLink.AttachEventHandlers(store);
			ObjectTypeShape.AttachEventHandlers(store);
			FactTypeShape.AttachEventHandlers(store);
			SubtypeLink.AttachEventHandlers(store);
		}
		void IORMModelEventSubscriber.AddPostLoadModelingEventHandlers()
		{
			AddPostLoadModelingEventHandlers();
		}
		/// <summary>
		/// Implements IORMModelEventSubscriber.RemoveModelingEventHandlers
		/// </summary>
		protected void RemoveModelingEventHandlers(bool preLoadAdded, bool postLoadAdded)
		{
			if (postLoadAdded)
			{
				Store store = Store;
				SubtypeLink.DetachEventHandlers(store);
				FactTypeShape.DetachEventHandlers(store);
				ObjectTypeShape.DetachEventHandlers(store);
				RolePlayerLink.DetachEventHandlers(store);
				ExternalConstraintShape.DetachEventHandlers(store);
				ReadingShape.DetachEventHandlers(store);
				ORMBaseShape.DetachEventHandlers(store);
			}
		}
		void IORMModelEventSubscriber.RemoveModelingEventHandlers(bool preLoadAdded, bool postLoadAdded)
		{
			RemoveModelingEventHandlers(preLoadAdded, postLoadAdded);
		}
		#endregion // IORMModelEventSubscriber Implementation
	}
}
