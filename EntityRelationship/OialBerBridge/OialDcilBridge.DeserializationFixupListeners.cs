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

using sys = System;
using System.Collections.Generic;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Neumont.Tools.EntityRelationshipModels.Barker;
using ORMCore = Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORMAbstractionToBarkerERBridge
{
	#region GenerateBarkerERModelFixupListener class

	/// <summary>
	/// The public fixup phase for the ORM abstraction bridge model
	/// </summary>
	public enum ORMAbstractionToBarkerERBridgeDeserializationFixupPhase
	{
		/// <summary>
		/// Validate Customization Options
		/// </summary>
		ValidateCustomizationOptions = (int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements + 5,
		/// <summary>
		/// Validate bridge elements after all core ORM validation is complete
		/// </summary>
		ValidateElements = (int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements + 10,
	}
	public partial class ORMAbstractionToBarkerERBridgeDomainModel : IDeserializationFixupListenerProvider
	{
		#region Algorithm Version Constants
		/// <summary>
		/// The algorithm version written to the file for the core algorithm
		/// </summary>
		public const string CurrentCoreAlgorithmVersion = "1.001";
		/// <summary>
		/// The algorithm version written to the file for the name generation algorithm
		/// </summary>
		public const string CurrentNameAlgorithmVersion = "1.000";
		#endregion // Algorithm Version Constants
		#region IDeserializationFixupListenerProvider Members

		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new GenerateBarkerERModelFixupListener();
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
		protected static sys.Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ORMAbstractionToBarkerERBridgeDeserializationFixupPhase);
			}
		}
		sys.Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return DeserializationFixupPhaseType;
			}
		}

		#endregion
		#region GENERATE METHOD

		private static void FullyGenerateBarkerERModel(BarkerErModel barkerModel, AbstractionModel sourceModel, INotifyElementAdded notifyAdded)
		{
			LinkedElementCollection<EntityType> barkerEntities = barkerModel.EntityTypeCollection;
			LinkedElementCollection<ConceptType> conceptTypes = sourceModel.ConceptTypeCollection;
			Store store = barkerModel.Store;

			// Generate all Barker entities
			List<ConceptType> manyToMany = new List<ConceptType>();
			foreach (ConceptType conceptType in conceptTypes)
			{
				if (!IsSimpleManyToManyAssociation(conceptType))
				{
					EntityType entity = new EntityType(
						conceptType.Store,
						new PropertyAssignment[]{
							new PropertyAssignment(EntityType.NameDomainPropertyId, conceptType.Name)});
					new EntityTypeIsPrimarilyForConceptType(entity, conceptType);

					barkerEntities.Add(entity);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(entity, true);
					}
				}
				else
				{
					manyToMany.Add(conceptType);
				}
			}

			// For every concept type create all attributes that they represent, map uniquenesses that they participate in.
			int associationCounter = 0;
			foreach (ConceptType conceptType in conceptTypes)
			{
				if (!manyToMany.Contains(conceptType))
				{
					CreateAttributesAndBinaryRelationships(conceptType, notifyAdded, ref associationCounter);
				}
				else
				{
					CreateBinaryAssociation(conceptType, notifyAdded, ref associationCounter);
				}
			}

			// For each entity type in the Barker model generate relationships it plays and detemine which of its atrributes are mandatory and nullable.
			foreach (EntityType entity in barkerModel.EntityTypeCollection)
			{
				GenerateMandatoryConstraints(entity);
			}
		}
		/// <summary>
		/// Applies Nullable/nonNullable constraints on all attributes for a given entity type.
		/// </summary>
		/// <param name="entity">The <see cref="EntityType"/> to set initial constraints for</param>
		private static void GenerateMandatoryConstraints(EntityType entity)
		{
			foreach (Attribute attribute in entity.AttributeCollection)
			{
				CheckAttributeConstraint(attribute, entity);
			}
		}
		/// <summary>
		/// CheckAttributeConstraint looks at the the ConceptTypeChildPath for an attribute, setting the attribute to nullable when some point in the path is not mandatory. 
		/// </summary>
		private static void CheckAttributeConstraint(Attribute attribute, EntityType entity)
		{
			if (entity == null)
			{
				entity = attribute.EntityType;
			}
			attribute.IsMandatory = AllStepsMandatory(entity, AttributeHasConceptTypeChild.GetConceptTypeChildPath(attribute));
		}
		private static bool AllStepsMandatory(EntityType entity, IEnumerable<ConceptTypeChild> links)
		{
			bool allStepsMandatory = true;
			ConceptType lastTarget = null;
			bool firstPass = true;
			foreach (ConceptTypeChild child in links)
			{
				if (!child.IsMandatory)
				{
					ConceptTypeAssimilatesConceptType assimilation = child as ConceptTypeAssimilatesConceptType;
					if (assimilation != null)
					{
						// The IsMandatory property applies when stepping parent-to-target, However, stepping target-to-parent
						// is always considered mandatory. See if we're in this situation.
						if (firstPass)
						{
							lastTarget = EntityTypeIsPrimarilyForConceptType.GetConceptType(entity);

						}
						if (lastTarget != null &&
							lastTarget == assimilation.Target)
						{
							lastTarget = assimilation.Parent;
							firstPass = false;
							continue;
						}
					}
					allStepsMandatory = false;
					break;
				}
				lastTarget = child.Target as ConceptType;
				firstPass = false;
			}

			return allStepsMandatory;
		}
		private static bool IsSimpleManyToManyAssociation(ConceptType conceptType)
		{
			LinkedElementCollection<ConceptTypeChild> associationChildren = ConceptTypeHasChildAsPartOfAssociation.GetTargetCollection(conceptType);
			ConceptTypeChild child0;
			ConceptTypeChild child1;
			if (associationChildren.Count == 2 &&
				!((child0 = associationChildren[0]) is InformationType) &&
				!((child1 = associationChildren[1]) is InformationType))
			{
				ReadOnlyCollection<ConceptTypeChild> allChildren = ConceptTypeChild.GetLinksToTargetCollection(conceptType);
				if (allChildren.Count == 2)
				{
					return
						child0 == allChildren[0] && child1 == allChildren[1] ||
						child0 == allChildren[1] && child1 == allChildren[0];
				}
			}
			return false;
		}
		private static void CreateAttributesAndBinaryRelationships(ConceptType conceptType, INotifyElementAdded notifyAdded, ref int associationCounter)
		{
			List<Attribute> attributesForConceptType = new List<Attribute>();

			foreach (InformationType informationType in InformationType.GetLinksToInformationTypeFormatCollection(conceptType))
			{
				attributesForConceptType.Add(CreateAttributeForInformationType(informationType, new Stack<ConceptTypeChild>()));
			}
			foreach (ConceptTypeRelatesToConceptType conceptTypeRelation in ConceptTypeRelatesToConceptType.GetLinksToRelatedConceptTypeCollection(conceptType))
			{
				if (!CreateBinaryAssociation(conceptTypeRelation, conceptType, conceptTypeRelation.RelatedConceptType, notifyAdded, ref associationCounter))
				{
					//if binary association was not created - let's create an attribute for it
					attributesForConceptType.AddRange(GetAttributesForConceptTypeRelation(conceptTypeRelation, new Stack<ConceptTypeChild>()));
				}				
			}
			foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (!conceptTypeAssimilation.RefersToSubtype)
				{
					CreateBinaryAssociation(conceptTypeAssimilation, conceptType, conceptTypeAssimilation.AssimilatedConceptType, notifyAdded, ref associationCounter);
				}
			}
			

			EntityType conceptTypeEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(conceptType);
			if (conceptTypeEntity != null)
			{
				conceptTypeEntity.AttributeCollection.AddRange(attributesForConceptType);
				if (notifyAdded != null)
				{
					foreach (Attribute attr in attributesForConceptType)
					{
						notifyAdded.ElementAdded(attr, true);
					}
				}
			}
		}
		/// <summary>
		/// for regular relationships
		/// </summary>
		/// <param name="relation"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="notifyAdded"></param>
		/// <param name="associationCounter"></param>
		/// <returns></returns>
		private static bool CreateBinaryAssociation(ConceptTypeChild relation, ConceptType source, ConceptType target, INotifyElementAdded notifyAdded, ref int associationCounter)
		{
			if (BinaryAssociationHasConceptTypeChild.GetBinaryAssociation(relation).Count > 0)
			{
				//it has already been created
				return true;
			}
			else if (EntityTypeIsPrimarilyForConceptType.GetEntityType(target) != null)
			{
				#region create association
				BinaryAssociation b = new BinaryAssociation(relation.Store,
							new PropertyAssignment[] { new PropertyAssignment(BinaryAssociation.NumberDomainPropertyId, associationCounter++) });
				//new BinaryAssociationHasConceptTypeChild(b, relation);
				BinaryAssociationHasConceptTypeChild.GetConceptTypeChildPath(b).Add(relation);

				Role r1 = new Role(relation.Store,
							new PropertyAssignment[] { new PropertyAssignment(Role.PredicateTextDomainPropertyId, source.Name) });
		
				Role r2 = new Role(relation.Store,
							new PropertyAssignment[] { new PropertyAssignment(Role.PredicateTextDomainPropertyId, target.Name) });

				b.RoleCollection.Add(r1);
				b.RoleCollection.Add(r2);

				EntityType sourceEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(source);
				EntityType targetEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(target);
				sourceEntity.RoleCollection.Add(r1);
				targetEntity.RoleCollection.Add(r2);
				sourceEntity.BarkerErModel.BinaryAssociationCollection.Add(b); 
				#endregion

				//determine whether roles are mandatory or optional
				List<ConceptTypeChild> links = new List<ConceptTypeChild>(1);
				links.Add(relation);
				r1.IsMandatory = AllStepsMandatory(targetEntity, links);
				if (relation is ConceptTypeAssimilatesConceptType)
				{
					r2.IsMandatory = AllStepsMandatory(sourceEntity, links);
				}

				#region determine whether roles are multivalued or not - and possibly rename
				ORMCore.ObjectType sourceObjectType = ConceptTypeIsForObjectType.GetObjectType(source);
				ORMCore.ObjectType targetObjectType = ConceptTypeIsForObjectType.GetObjectType(target);
				ORMCore.UniquenessConstraint uSource = null, uTarget = null;
				foreach (ORMCore.FactType factType in ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(relation))
				{
					Debug.Assert(factType.RoleCollection.Count == 2, "Error when mapping to Barker ER; the fact type is not binary");
					foreach (ORMCore.RoleBase r in factType.RoleCollection)
					{
						//need to use RoleBase because we might run into RoleProxy
						ORMCore.Role role = r.Role;
						foreach (ORMCore.ConstraintRoleSequence constraintRoleSequence in role.ConstraintRoleSequenceCollection)
						{
							ORMCore.UniquenessConstraint uninquenessConstraint = constraintRoleSequence as ORMCore.UniquenessConstraint;
							if (uninquenessConstraint != null && //check that it's a uniqueness constraint
								uninquenessConstraint.Modality == ORMCore.ConstraintModality.Alethic && //check it's alethic
								uninquenessConstraint.IsInternal) //check it's internal
							{
								if (role.RolePlayer == sourceObjectType)
								{
									uSource = uninquenessConstraint;
								}
								if (role.RolePlayer == targetObjectType)
								{
									uTarget = uninquenessConstraint;
								}
							}
						}
					}
					//name the roles properly
					//TODO this is a hack; proper name generation is yet to be implemented
					foreach (ORMCore.ReadingOrder order in factType.ReadingOrderCollection)
					{
						string text = order.ReadingText;
						int first = text.IndexOf('}') + 1;
						text = text.Substring(first, text.LastIndexOf('{') - first);
						text = text.Trim();

						if (!string.IsNullOrEmpty(text) &&
							order.RoleCollection != null && order.RoleCollection.Count > 0 &&
							order.RoleCollection[0].Role != null)
						{
							ORMCore.ObjectType o = order.RoleCollection[0].Role.RolePlayer;
							if (o == sourceObjectType)
							{
								r1.PredicateText = text;
							}
							else if (o == targetObjectType)
							{
								r2.PredicateText = text;
							}
						}
					}
				}
				if (uSource != null && uSource == uTarget)
				{
					//it's many-to-many
					r1.IsMultiValued = true;
					r2.IsMultiValued = true;
				}
				else if (uSource == null || uTarget == null)
				{
					//it's one-to-many
					r1.IsMultiValued = uSource != null;
					r2.IsMultiValued = uTarget != null;
				}
				else if (uSource != null && uTarget != null)
				{
					//it's one-to-one
					r1.IsMultiValued = false;
					r2.IsMultiValued = false;
				}
				else
				{
					Debug.Fail("Found a fact type with no uniqueness constraints!");
				} 
				#endregion

				#region primary id?
				foreach (Uniqueness u in UniquenessIncludesConceptTypeChild.GetUniquenessCollection(relation))
				{
					if (u.IsPreferred)
					{
						r1.IsPrimaryIdComponent = true;
						break;
					}
				}
				#endregion

				//notify elements added
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(b, true);
					notifyAdded.ElementAdded(r1, true);
					notifyAdded.ElementAdded(r2, true);
				}

				return true;
			}
			else
			{
				//should not create binary association in this case
				return false;
			}
		}
		/// <summary>
		/// for many-to-many associations; it is assumed that IsSimpleManyToManyAssociation has been
		/// called on the passed-in concept type and returned true
		/// </summary>
		/// <param name="parentConceptType"></param>
		/// <param name="notifyAdded"></param>
		/// <param name="associationCounter"></param>
		private static void CreateBinaryAssociation(ConceptType parentConceptType, INotifyElementAdded notifyAdded, ref int associationCounter)
		{
			if (BinaryAssociationHasConceptType.GetBinaryAssociation(parentConceptType).Count == 0)
			{
				LinkedElementCollection<ConceptTypeChild> associationChildren =
					ConceptTypeHasChildAsPartOfAssociation.GetTargetCollection(parentConceptType);
				ConceptTypeChild relation1, relation2;
				ConceptType ct1, ct2;

				if (null != (relation1 = associationChildren[0]) &&
					null != (relation2 = associationChildren[1]) &&
					null != (ct1 = relation1.Target as ConceptType) &&
					null != (ct2 = relation2.Target as ConceptType))
				{
					// create association
					BinaryAssociation b = new BinaryAssociation(parentConceptType.Store,
								new PropertyAssignment[] { new PropertyAssignment(BinaryAssociation.NumberDomainPropertyId, associationCounter++) });
					//new BinaryAssociationHasConceptTypeChild(b, relation);
					BinaryAssociationHasConceptType.GetConceptType(b).Add(parentConceptType);

					Role r1 = new Role(parentConceptType.Store,
								new PropertyAssignment[] { new PropertyAssignment(Role.PredicateTextDomainPropertyId, ct1.Name) });

					Role r2 = new Role(parentConceptType.Store,
								new PropertyAssignment[] { new PropertyAssignment(Role.PredicateTextDomainPropertyId, ct2.Name) });

					b.RoleCollection.Add(r1);
					b.RoleCollection.Add(r2);

					EntityType sourceEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(ct1);
					EntityType targetEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(ct2);
					sourceEntity.RoleCollection.Add(r1);
					targetEntity.RoleCollection.Add(r2);
					sourceEntity.BarkerErModel.BinaryAssociationCollection.Add(b);

					//determine whether roles are mandatory or optional
					//TODO

					//set multi-values
					r1.IsMultiValued = true;
					r2.IsMultiValued = true;

					//notify elements added
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(b, true);
						notifyAdded.ElementAdded(r1, true);
						notifyAdded.ElementAdded(r2, true);
					}
				}
			}
		}
		private static Attribute CreateAttributeForInformationType(InformationType informationType, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			conceptTypeChildPath.Push(informationType);
			Attribute attribute = new Attribute(informationType.Store,
								new PropertyAssignment[]{
									new PropertyAssignment(Attribute.NameDomainPropertyId, informationType.Name)});
			foreach (Uniqueness u in UniquenessIncludesConceptTypeChild.GetUniquenessCollection(informationType))
			{
				if (u.IsPreferred)
				{
					attribute.IsPrimaryIdComponent = true;
					break;
				}
			}

			ConceptTypeChild[] conceptTypeChildPathReverse = conceptTypeChildPath.ToArray();
			sys.Array.Reverse(conceptTypeChildPathReverse);
			AttributeHasConceptTypeChild.GetConceptTypeChildPath(attribute).AddRange(conceptTypeChildPathReverse);
			conceptTypeChildPath.Pop();
			return attribute;
		}
		private static List<Attribute> GetAttributesForConceptTypeRelation(ConceptTypeRelatesToConceptType conceptTypeRelation, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			conceptTypeChildPath.Push(conceptTypeRelation);
			List<Attribute> attributes = GetPreferredIdentifierAttributesForConceptType(conceptTypeRelation.RelatedConceptType, conceptTypeChildPath);
			conceptTypeChildPath.Pop();
			return attributes;
		}
		private static List<Attribute> GetPreferredIdentifierAttributesForConceptType(ConceptType conceptType, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
			{
				if (uniqueness.IsPreferred)
				{
					LinkedElementCollection<ConceptTypeChild> uniquenessConceptTypeChildren = uniqueness.ConceptTypeChildCollection;
					List<Attribute> attributes = new List<Attribute>(uniquenessConceptTypeChildren.Count);

					foreach (ConceptTypeChild conceptTypeChild in uniquenessConceptTypeChildren)
					{
						InformationType informationType = conceptTypeChild as InformationType;
						if (informationType != null)
						{
							attributes.Add(CreateAttributeForInformationType(informationType, conceptTypeChildPath));
						}
						else
						{
							Debug.Assert(conceptTypeChild is ConceptTypeRelatesToConceptType, "Uniquenesses can't contain ConceptTypeAssimilations.");
							attributes.AddRange(GetAttributesForConceptTypeRelation((ConceptTypeRelatesToConceptType)conceptTypeChild, conceptTypeChildPath));
						}
					}

					return attributes;
				}
			}
			Debug.Fail("Couldn't find preferred identifier for concept type.");
			throw new sys.InvalidOperationException();
		}

		#endregion
		#region fixup listener that creates barker ER model
		private class GenerateBarkerERModelFixupListener : DeserializationFixupListener<AbstractionModel>
		{
			/// <summary>
			/// Create a new fixup listener
			/// </summary>
			public GenerateBarkerERModelFixupListener()
				: base((int)ORMAbstractionToBarkerERBridgeDeserializationFixupPhase.ValidateElements)
			{
			}
			/// <summary>
			/// Verify that an abstraction model has an appropriate Barker ER model and bridge
			/// </summary>
			protected override void ProcessElement(AbstractionModel element, Store store, INotifyElementAdded notifyAdded)
			{
				BarkerErModel barkerModel = BarkerErModelIsForAbstractionModel.GetBarkerErModel(element);
				if (barkerModel == null)
				{
					// Create the initial Barker ER model and notify
					barkerModel = new BarkerErModel(
						store,
						new PropertyAssignment[]{
						new PropertyAssignment(BarkerErModel.NameDomainPropertyId, element.Name)});

					new BarkerErModelIsForAbstractionModel(barkerModel, element);

					BarkerERModelGenerationSetting generationSetting = new BarkerERModelGenerationSetting(store,
						new PropertyAssignment(
						BarkerERModelGenerationSetting.CoreAlgorithmVersionDomainPropertyId, CurrentCoreAlgorithmVersion),
						new PropertyAssignment(
						BarkerERModelGenerationSetting.NameAlgorithmVersionDomainPropertyId, CurrentNameAlgorithmVersion));

					new GenerationSettingTargetsBarkerERModel(generationSetting, barkerModel);

					new ORMCore.GenerationStateHasGenerationSetting(ORMCore.GenerationState.EnsureGenerationState(store), generationSetting);

					notifyAdded.ElementAdded(barkerModel, true);

					FullyGenerateBarkerERModel(barkerModel, element, notifyAdded);
				}
				else
				{
					BarkerERModelGenerationSetting generationSetting = GenerationSettingTargetsBarkerERModel.GetGenerationSetting(barkerModel);
					bool regenerateAll = generationSetting == null || generationSetting.CoreAlgorithmVersion != CurrentCoreAlgorithmVersion;
					bool regenerateNames = false;
					if (!regenerateAll)
					{
						foreach (EntityType barkerEntity in barkerModel.EntityTypeCollection)
						{
							if (null == EntityTypeIsPrimarilyForConceptType.GetLinkToConceptType(barkerEntity))
							{
								regenerateAll = true;
								break;
							}
							// Theoretically we should also check that all attributes and uniqueness constraints
							// are pathed back to the abstraction model. However, this is far from a full validation,
							// and the scenario we're trying to cover is the abstraction model regenerating during
							// load and removing our bridge elements. The entity type check above is sufficient.
						}
						regenerateNames = !regenerateAll && generationSetting.NameAlgorithmVersion != CurrentNameAlgorithmVersion;
						generationSetting.NameAlgorithmVersion = CurrentNameAlgorithmVersion;
					}
					else
					{
						if (generationSetting == null)
						{
							generationSetting = new BarkerERModelGenerationSetting(store, 
								new PropertyAssignment(
								BarkerERModelGenerationSetting.CoreAlgorithmVersionDomainPropertyId, CurrentCoreAlgorithmVersion), 
								new PropertyAssignment(
								BarkerERModelGenerationSetting.NameAlgorithmVersionDomainPropertyId, CurrentNameAlgorithmVersion));

							new GenerationSettingTargetsBarkerERModel(generationSetting, barkerModel);

							new ORMCore.GenerationStateHasGenerationSetting(ORMCore.GenerationState.EnsureGenerationState(store), generationSetting);
						}
						else
						{
							regenerateNames = generationSetting.NameAlgorithmVersion != CurrentNameAlgorithmVersion;
							generationSetting.CoreAlgorithmVersion = CurrentCoreAlgorithmVersion;
							generationSetting.NameAlgorithmVersion = CurrentNameAlgorithmVersion;
						}
					}
					if (regenerateAll)
					{
						barkerModel.BinaryAssociationCollection.Clear();
						barkerModel.EntityTypeCollection.Clear();
						barkerModel.ExclusiveArcCollection.Clear();

						FullyGenerateBarkerERModel(barkerModel, element, notifyAdded);
					}
					else if (regenerateNames)
					{
						//NameGeneration.GenerateAllNames(barkerModel);
					}
				}
			}
		}
		#endregion
	}
	#endregion // GenerateBarkerERModelFixupListener class
}
