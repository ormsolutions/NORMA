using System;
using System.Collections.Generic;
using Northface.Tools.ORM.Framework;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class ORMMetaModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return MultiColumnExternalConstraint.FixupListener;
				yield return SingleColumnExternalConstraint.FixupListener;
				yield return NamedElementDictionary.GetFixupListener((int)ORMDeserializationFixupPhase.AddImplicitElements);
				yield return ModelError.FixupListener;
				yield return ReferenceMode.FixupListener;
				yield return ORMModel.DataTypesFixupListener;
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
	}
}
