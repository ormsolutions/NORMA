#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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

using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace unibz.ORMInferenceEngine
{
	public enum ORMInferenceEngineDeserializationFixupPhase
	{
		CreateHierarchy = (int)ORMDeserializationFixupPhase.ValidateErrors + 10,
		CreateInferenceResult = (int)ORMDeserializationFixupPhase.ValidateErrors + 10,
		CreateUnsatisfiableDomain = (int)ORMDeserializationFixupPhase.ValidateErrors + 10
	}

	partial class ORMInferenceEngineDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				yield return Hierarchy.FixupListener;
				yield return InferenceResult.FixupListener;
				//				yield return ModelError.GetFixupListener((int)ORMDeserializationFixupPhase.ValidateErrors, contextDomainModel);
			}
		}
		Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ORMInferenceEngineDeserializationFixupPhase);
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
	}
}
