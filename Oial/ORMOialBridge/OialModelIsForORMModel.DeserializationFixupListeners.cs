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
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.Collections;
using Neumont.Tools.ORMAbstraction;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	/// <summary>
	/// The public fixup phase for the ORM abstraction bridge model
	/// </summary>
	public enum ORMToORMAbstractionBridgeDeserializationFixupPhase
	{
		/// <summary>
		/// Create bridge for model if no bridge currently exists
		/// </summary>
		CreateImplicitElements = (int)ORMDeserializationFixupPhase.ValidateErrors + 10,
	}
	public partial class ORMToORMAbstractionBridgeDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return AbstractionModelIsForORMModel.FixupListener;
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
				return typeof(ORMToORMAbstractionBridgeDeserializationFixupPhase);
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
	partial class AbstractionModelIsForORMModel
	{
		#region Deserialization Fixup Classes
		/// <summary>
		/// A <see cref="IDeserializationFixupListener"/> for synchronizing the abstraction model on load
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ORMModelFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class ORMModelFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// ORMModelFixupListener constructor
			/// </summary>
			public ORMModelFixupListener()
				: base((int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements)
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
				AbstractionModel oil = AbstractionModelIsForORMModel.GetAbstractionModel(element);
				if (oil == null)
				{
					// UNDONE: DelayValidateModel currently deletes and recreates any existing
					// bridge relationship, so there is no point deleting it up front, we'll
					// just retrieve it later. Also note that DelayValidateModel does not call notifyAdded.
					DelayValidateModel(element);
					oil = AbstractionModelIsForORMModel.GetAbstractionModel(element);
					if (oil != null)
					{
						notifyAdded.ElementAdded(oil, true);
					}
				}
				else
				{
					AbstractionModelGenerationSetting generationSetting;
					bool regenerateForVersion = null == (generationSetting = GenerationSettingTargetsAbstractionModel.GetGenerationSetting(oil)) || generationSetting.AlgorithmVersion != CurrentAlgorithmVersion;
					bool excludedBridgedElement = false;
					ORMElementGateway.Initialize(
						element,
						regenerateForVersion ? (ORMElementGateway.NotifyORMElementExcluded)null :
						delegate(ORMModelElement modelElement)
						{
							if (excludedBridgedElement)
							{
								return;
							}
							ObjectType objectType;
							FactType factType;
							// Note that the types we're checking here are synchronized with the ORMElementGateway.ExclusionAdded method
							if (null != (objectType = modelElement as ObjectType))
							{
								if (null != ConceptTypeIsForObjectType.GetLinkToConceptType(objectType) ||
									null != InformationTypeFormatIsForValueType.GetLinkToInformationTypeFormat(objectType))
								{
									excludedBridgedElement = true;
								}
							}
							else if (null != (factType = modelElement as FactType))
							{
								if (null != FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType) ||
									ConceptTypeChildHasPathFactType.GetLinksToConceptTypeChild(factType).Count != 0)
								{
									excludedBridgedElement = true;
								}
							}
						});
					if (regenerateForVersion || excludedBridgedElement)
					{
						// Something is very wrong, regenerate (does not regenerate the excluded elements we already have)
						DelayValidateModel(element);
					}
				}
			}
		}
		#endregion // Deserialization Fixup Classes
	}
}
