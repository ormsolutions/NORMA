#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	public partial class ORMCoreDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				DomainModelInfo contextDomainModel = DomainModelInfo;
				yield return SetComparisonConstraint.FixupListener;
				yield return SetConstraint.FixupListener;
				yield return NamedElementDictionary.GetFixupListener((int)ORMDeserializationFixupPhase.ValidateElementNames, contextDomainModel);
				yield return SubtypeFact.FixupListener;
				yield return FactType.NameFixupListener;
				yield return FactType.UnaryFixupListener;
				yield return FactTypeDerivationExpression.FixupListener;
				yield return ExclusiveOrConstraintCoupler.FixupListener;
				yield return Objectification.FixupListener;
				yield return Objectification.ImpliedFixupListener;
				yield return ReferenceMode.FixupListener;
				yield return ORMModel.DataTypesFixupListener;
				yield return ObjectType.IsIndependentFixupListener;
				yield return ObjectType.PreferredIdentificationPathFixupListener;
				yield return ModelError.GetFixupListener((int)ORMDeserializationFixupPhase.ValidateErrors, contextDomainModel);
				yield return NameAlias.FixupListener;
				yield return NameGenerator.FixupListener;
				yield return ModelErrorDisplayFilter.FixupListener;
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
				return typeof(ORMDeserializationFixupPhase);
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
