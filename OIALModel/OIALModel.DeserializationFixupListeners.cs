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
using System.Collections.Generic;
using Neumont.Tools.ORM.Framework;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.Collections;

namespace Neumont.Tools.ORM.OIALModel
{
	public partial class OIALMetaModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new ORMModelFixupListener();
				yield return OIALModel.ORMModelHasObjectTypeFixupListener;
				yield return OIALModel.ORMModelHasFactTypeFixupListener;
				yield return OIALModel.ORMModelModelHasSetConstraintFixupListener;
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
		#region Deserialization Fixup Classes
		/// <summary>
		/// Fixup listener implementation. Adds implicit MyCustomExtensionElement objects to roles
		/// that don't have them when the file is deserialized. This allows extension elements to
		/// be added to existing files, as well as extensions with default values (which don't serialize
		/// because of the settings in the ExtensionDomainModel.SerializationExtensions.xml file) to
		/// be readded when the file loads.
		/// </summary>
		private sealed class ORMModelFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public ORMModelFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Make sure we have our tracker attached to all loaded models.
			/// </summary>
			/// <param name="element">An ORMModel element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ORMModel element, Store store, INotifyElementAdded notifyAdded)
			{
				OIALModel oil = (OIALModel)element.GetCounterpartRolePlayer(
					OIALModelHasORMModel.ORMModelMetaRoleGuid,
					OIALModelHasORMModel.OIALModelMetaRoleGuid,
					false);
				if (oil == null)
				{
					oil = OIALModel.CreateOIALModel(store);
					oil.ORMModel = element;
					notifyAdded.ElementAdded(oil, true);
				}
			}
		}
		#endregion // Deserialization Fixup Classes
	}
}
