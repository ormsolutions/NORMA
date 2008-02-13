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
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	/// <summary>
	/// The public fixup phase for the ORM abstraction bridge model
	/// </summary>
	public enum ConceptualDatabaseDeserializationFixupPhase
	{
		// Note that this value is ORMToORMAbstractionBridgeDeserializationFixupPhase.ValidateElements + 10
		/// <summary>
		/// Validate Constraints
		/// </summary>
		ValidateConstraints = (int)Neumont.Tools.ORM.ObjectModel.ORMDeserializationFixupPhase.ValidateErrors + 30,
	}
	public partial class ConceptualDatabaseDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return ReferenceConstraint.FixupListener;
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
		/// </summary>
		protected static Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ConceptualDatabaseDeserializationFixupPhase);
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
