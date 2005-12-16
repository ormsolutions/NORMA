using System;
using System.Collections.Generic;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new DisplayRolePlayersFixupListener();
				yield return new DisplayReadingsFixupListener();
				yield return new DisplayExternalConstraintLinksFixupListener();
				yield return new DisplaySubtypeLinkFixupListener();
				yield return new DisplayRoleValueRangeDefinitionFixupListener();
				yield return new DisplayValueTypeValueRangeDefinitionFixupListener();
				yield return new EliminateOrphanedShapesFixupListener();
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
