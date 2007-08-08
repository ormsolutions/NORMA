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
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new DisplayRolePlayersFixupListener();
				yield return new DisplayReadingsFixupListener();
				yield return new DisplayExternalConstraintLinksFixupListener();
				yield return new DisplaySubtypeLinkFixupListener();
				yield return new DisplayRoleValueConstraintFixupListener();
				yield return new DisplayValueTypeValueConstraintFixupListener();
				yield return new DisplayRoleNameFixupListener();
				yield return new DisplayModelNoteLinksFixupListener();
				yield return FactTypeShape.FixupListener;
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
			}
		}
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupPhaseType"/>
		/// The shape model uses the same fixup phases as the core domain model, so this returns null.
		/// </summary>
		protected static Type DeserializationFixupPhaseType
		{
			get
			{
				return null;
			}
		}
		Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return DeserializationFixupPhaseType;
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
	}
}
