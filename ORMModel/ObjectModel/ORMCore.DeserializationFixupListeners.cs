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

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ORMCoreDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return SetComparisonConstraint.FixupListener;
				yield return SetConstraint.FixupListener;
				yield return NamedElementDictionary.GetFixupListener((int)ORMDeserializationFixupPhase.ValidateElementNames);
				yield return SubtypeFact.FixupListener;
				yield return FactType.NameFixupListener;
				yield return ExclusiveOrConstraintCoupler.FixupListener;
				yield return Objectification.FixupListener;
				yield return Objectification.ImpliedFixupListener;
				yield return ReferenceMode.FixupListener;
				yield return ORMModel.DataTypesFixupListener;
				yield return ObjectType.IsIndependentFixupListener;
				yield return ModelError.FixupListener;
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
