#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMAbstraction;

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region CurrentAlgorithmVersion constant
		/// <summary>
		/// The algorithm version written to the file
		/// </summary>
		public const string CurrentAlgorithmVersion = "1.012";
		#endregion // CurrentAlgorithmVersion constant
		#region ValidationPriority enum
		/// <summary>
		/// DelayValidate ordering constants. Handles non-zero values for
		/// validation methods in this class and nested classes.
		/// </summary>
		private static class ValidationPriority
		{
			/// <summary>
			/// A new <see cref="ObjectType"/> has been added to the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayNewObjectType = -100;
			/// <summary>
			/// An <see cref="ObjectType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayReconsiderObjectType = -90;
			/// <summary>
			/// A <see cref="FactType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayNewFactType = -80;
			/// <summary>
			/// A <see cref="FactType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayReconsiderFactType = -70;
			/// <summary>
			/// Gateway exclusion relationships have been added, remove other existing
			/// bridge relationships
			/// </summary>
			public const int GatewayRemoveExcludedBridgeRelationships = -60;
			/// <summary>
			/// A new element has been added and passed gateway filtering
			/// </summary>
			public const int GatewayAddElement = -50;
			/// <summary>
			/// Validate the model. Current rebuilds the entire model.
			/// </summary>
			public const int ValidateModel = 100;
			/// <summary>
			/// Reset mandatory properties. This is done after ValidateModel
			/// so that we don't waste time 
			/// </summary>
			public const int ValidateMandatory = 110;
		}
		#endregion // ValidationPriority enum
		#region Element tracking transaction support
		#region ModelElementModification enum
		/// <summary>
		/// An enumeration of changes that may affect the ORM model
		/// </summary>
		[Flags]
		private enum ModelElementModification
		{
			/// <summary>
			/// An element from the ORM object model was added
			/// </summary>
			ORMElementAdded = 1,
			/// <summary>
			/// An element from the ORM object model was changed
			/// </summary>
			ORMElementChanged = 2,
			/// <summary>
			/// An element from the ORM object model was deleted
			/// </summary>
			ORMElementDeleted = 4,
			/// <summary>
			/// A bridge element was removed from this abstraction element
			/// </summary>
			AbstractionElementDetached = 8,
		}
		#endregion // ModelElementModification enum
		private static readonly object Key = new object();

		private static Dictionary<ModelElement, int> EnsureContextKeyExists(Store store)
		{
			Dictionary<object, object> contextDictionary = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			object elementList = null;

			if (!contextDictionary.TryGetValue(Key, out elementList))
			{
				elementList = new Dictionary<ModelElement, int>();
				contextDictionary.Add(Key, elementList);
			}
			return (Dictionary<ModelElement, int>)elementList;
		}

		private static void AddTransactedModelElement(ModelElement element, ModelElementModification action)
		{
			Dictionary<ModelElement, int> elementList;
			elementList = EnsureContextKeyExists(element.Store);

			if (elementList.ContainsKey(element))
			{
				elementList[element] = (elementList[element] | (int)action);
			}
			else
			{
				elementList.Add(element, (int)action);
			}
		}
		#endregion // Element tracking transaction support
		#region Model validation
		private bool myRebuildingAbstractionModel;
		/// <summary>
		/// Delays the validate model.
		/// </summary>
		/// <param name="element">The element.</param>
		[DelayValidatePriority(ValidationPriority.ValidateModel, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
		private static void DelayValidateModel(ModelElement element)
		{
			ValidateORMModel((ORMModel)element, false);
		}
		private static void ValidateORMModel(ORMModel model, bool notifyBeforeRebuild)
		{
			Dictionary<object, object> contextDictionary = model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;

			if (contextDictionary.ContainsKey(Key))
			{
				// Get the elements affected within the transaction
				Dictionary<ModelElement, int> elementList = (Dictionary<ModelElement, int>)contextDictionary[Key];

				// Elements that were both added & deleted in the transaction can be ignored for map change analysis
				EliminateRedundantElements(elementList);

				// TODO: scan for changes that actually affect the model; all others can be filtered out

				if (elementList.Count == 0)
				{
					return;
				}
			}

			Store store = model.Store;

			// Get the link from the given ORMModel
			AbstractionModelIsForORMModel oialModelIsForORMModel = AbstractionModelIsForORMModel.GetLinkToAbstractionModel(model);
			AbstractionModel oialModel = null;

			// If the link exists, clear it out. There is no need to recreate it completely.
			if (oialModelIsForORMModel != null)
			{
				// Make sure the RebuildingAbstractionModel returns true during the rebuild
				oialModelIsForORMModel.myRebuildingAbstractionModel = true;
				try
				{
					oialModel = oialModelIsForORMModel.AbstractionModel;
					IAbstractionModelRebuilding[] rebuildListeners;
					if (notifyBeforeRebuild && null != (rebuildListeners = ((IFrameworkServices)store).GetTypedDomainModelProviders<IAbstractionModelRebuilding>()))
					{
						foreach (IAbstractionModelRebuilding rebuildListener in rebuildListeners)
						{
							rebuildListener.AbstractionModelRebuilding(oialModel);
						}
					}

					oialModel.ConceptTypeCollection.Clear();
					oialModel.InformationTypeFormatCollection.Clear();

					ReadOnlyCollection<FactTypeMapsTowardsRole> factTypeMaps = store.ElementDirectory.FindElements<FactTypeMapsTowardsRole>(false);
					int factTypeMapCount = factTypeMaps.Count;
					for (int i = factTypeMapCount - 1; i >= 0; --i)
					{
						FactTypeMapsTowardsRole factTypeMap = factTypeMaps[i];
						if (factTypeMap.FactType.Model == model)
						{
							factTypeMap.Delete();
						}
					}
					// Apply ORM to OIAL algorithm
					oialModelIsForORMModel.TransformORMtoOial();
				}
				finally
				{
					oialModelIsForORMModel.myRebuildingAbstractionModel = false;
				}
			}
			else
			{
				oialModel = new AbstractionModel(
					store,
					new PropertyAssignment(AbstractionModel.NameDomainPropertyId, model.Name));
				oialModelIsForORMModel = new AbstractionModelIsForORMModel(oialModel, model);

				// Set initial object exclusion states
				ORMElementGateway.Initialize(model, null);

				// Apply ORM to OIAL algorithm
				oialModelIsForORMModel.TransformORMtoOial();
			}
			if (oialModel != null)
			{
				AbstractionModelGenerationSetting generationSetting = GenerationSettingTargetsAbstractionModel.GetGenerationSetting(oialModel);
				if (generationSetting == null)
				{
					generationSetting = new AbstractionModelGenerationSetting(store, new PropertyAssignment(AbstractionModelGenerationSetting.AlgorithmVersionDomainPropertyId, CurrentAlgorithmVersion));
					new GenerationSettingTargetsAbstractionModel(generationSetting, oialModel);
					new GenerationStateHasGenerationSetting(GenerationState.EnsureGenerationState(store), generationSetting);
				}
				else
				{
					generationSetting.AlgorithmVersion = CurrentAlgorithmVersion;
				}
			}
		}
		/// <summary>
		/// Initiate a model rebuild if a rebuild is not already under way
		/// </summary>
		/// <param name="model">The <see cref="AbstractionModel"/> to check for rebuilding</param>
		/// <returns><see langword="true"/> if the requested rebuild will occur.</returns>
		private static bool TestRebuildAbstractionModel(AbstractionModel model)
		{
			if (model == null)
			{
				return false;
			}
			AbstractionModelIsForORMModel link = AbstractionModelIsForORMModel.GetLinkToORMModel(model);
			bool allow = !(link != null && link.myRebuildingAbstractionModel);
			if (allow)
			{
				FrameworkDomainModel.DelayValidateElement(link.ORMModel, DelayValidateModel);
			}
			return allow;
		}

		/// <summary>
		/// Removes elements from the list that were both created and deleted in the transaction.
		/// </summary>
		/// <param name="elementList">The list of elements that were affected by the transaction.</param>
		private static void EliminateRedundantElements(Dictionary<ModelElement, int> elementList)
		{
			foreach (ModelElement element in elementList.Keys)
			{
				int actions = elementList[element];
				if ((actions & (int)ModelElementModification.ORMElementAdded) != 0 && (actions & (int)ModelElementModification.ORMElementDeleted) != 0)
				{
					elementList.Remove(element);
				}
			}
		}
		#endregion
		#region ORM to OIAL Algorithm Methods
		/// <summary>
		/// Transforms the data in <see cref="ORMModel"/> into this <see cref="AbstractionModel"/>.
		/// </summary>
		private void TransformORMtoOial()
		{
			ORMModel model = this.ORMModel;
			LinkedElementCollection<FactType> modelFactTypes = model.FactTypeCollection;

			FactTypeMappingDictionary decidedManyToOneFactTypeMappings = new FactTypeMappingDictionary();
			FactTypeMappingDictionary decidedOneToOneFactTypeMappings = new FactTypeMappingDictionary();
			FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings = new FactTypeMappingListDictionary();

			PerformInitialFactTypeMappings(modelFactTypes, decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
			FilterFactTypeMappings(decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

			FactTypeMappingPermuter permuter = new FactTypeMappingPermuter(decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
			FactTypeMappingDictionary decidedFactTypeMappings = permuter.Run();

			GenerateOialModel(decidedFactTypeMappings);
		}
		/// <summary>
		/// Determine if an entity type has a value type with an auto counter
		/// type as part of its direct identification scheme.
		/// </summary>
		private static bool EntityTypeIsAutoIdentified(ObjectType entityType)
		{
			UniquenessConstraint pid;
			if (null != entityType &&
				null != (pid = entityType.PreferredIdentifier))
			{
				foreach (Role identifyingRole in pid.RoleCollection)
				{
					ObjectType identifyingRolePlayer;
					ValueTypeHasDataType dataTypeUse;
					if (null != (identifyingRolePlayer = identifyingRole.RolePlayer) &&
						null != (dataTypeUse = ValueTypeHasDataType.GetLinkToDataType(identifyingRolePlayer)) &&
						dataTypeUse.AutoGenerated &&
						dataTypeUse.DataType.AutoGenerationIncremental)
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Determines the obvious fact type mappings, and all other potential mappings.
		/// </summary>
		/// <param name="modelFactTypes">The <see cref="FactType"/> objects of the model</param>
		/// <param name="decidedManyToOneFactTypeMappings">The decided many-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="decidedOneToOneFactTypeMappings">The decided one-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void PerformInitialFactTypeMappings(LinkedElementCollection<FactType> modelFactTypes, FactTypeMappingDictionary decidedManyToOneFactTypeMappings, FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			// For each fact type in the model...
			foreach (FactType factType in modelFactTypes)
			{
				if (ShouldIgnoreFactType(factType))
				{
					continue;
				}
				SubtypeFact subtypeFact = factType as SubtypeFact;

				// If it's a subtype relation...
				MandatoryConstraint mandatory;
				if (subtypeFact != null)
				{
					Role subtypeRole = subtypeFact.SubtypeRole;
					ObjectType subtype = subtypeRole.RolePlayer;
					Role supertypeRole = subtypeFact.SupertypeRole;
					mandatory = supertypeRole.SingleRoleAlethicMandatoryConstraint; // Note that the only way to get a mandatory on the supertype role is with an implied mandatory, verified explicitly below

					FactTypeMapping factTypeMapping = new FactTypeMapping(
						subtypeFact,
						subtypeRole,
						supertypeRole,
						null,
						FactTypeMappingFlags.Subtype |
							// Map deeply toward the supertype unless the subtype has a generated identifier, in which case
							// we disallow a deep mapping.
							((subtypeFact.ProvidesPreferredIdentifier || !EntityTypeIsAutoIdentified(subtype)) ? FactTypeMappingFlags.DeepMapping : FactTypeMappingFlags.None) |
							FactTypeMappingFlags.FromRoleMandatory |
							((mandatory != null && mandatory.IsImplied) ? FactTypeMappingFlags.TowardsRoleMandatory | FactTypeMappingFlags.TowardsRoleImpliedMandatory : FactTypeMappingFlags.None) |
							(subtype.IsValueType ? FactTypeMappingFlags.FromValueType | FactTypeMappingFlags.TowardsValueType : FactTypeMappingFlags.None));
					decidedOneToOneFactTypeMappings.Add(subtypeFact, factTypeMapping);
				}
				else if (factType.UnaryPattern != UnaryValuePattern.NotUnary)
				{
					FactType inverseFactType;
					FactTypeMappingFlags inverseFlags;
					ResolveInverseFactType(factType, out inverseFactType, out inverseFlags);
					Role unaryRole = factType.UnaryRole;
					ObjectType rolePlayer = unaryRole.RolePlayer;
					// A unary role cannot have a normal mandatory constraint. However, it can have an implied mandatory constraint
					// if it is the only non-identifying role on the entity type.
					decidedManyToOneFactTypeMappings.Add(factType, new FactTypeMapping(factType, null, unaryRole, inverseFactType, FactTypeMappingFlags.FromValueType | (rolePlayer.IsValueType ? FactTypeMappingFlags.TowardsValueType : FactTypeMappingFlags.None) | inverseFlags | ((rolePlayer.ImpliedMandatoryConstraint != null && unaryRole.SingleRoleAlethicMandatoryConstraint != null) ? FactTypeMappingFlags.TowardsRoleMandatory : FactTypeMappingFlags.None)));
				}
				else
				{
					LinkedElementCollection<RoleBase> roles = factType.RoleCollection;

					Debug.Assert(roles.Count == 2 && factType.Objectification == null, "Non-binarized fact types should have been filtered out already.");

					FactType inverseFactType;
					FactTypeMappingFlags inverseFlags;
					ResolveInverseFactType(factType, out inverseFactType, out inverseFlags);

					Role firstRole = roles[0].Role;
					Role secondRole = roles[1].Role;
					ObjectType firstRolePlayer = firstRole.RolePlayer;
					ObjectType secondRolePlayer = secondRole.RolePlayer;

					UniquenessConstraint firstRoleUniquenessConstraint = (UniquenessConstraint)firstRole.SingleRoleAlethicUniquenessConstraint;
					UniquenessConstraint secondRoleUniquenessConstraint = (UniquenessConstraint)secondRole.SingleRoleAlethicUniquenessConstraint;
					
					bool firstRolePlayerIsValueType = firstRolePlayer.IsValueType;
					bool secondRolePlayerIsValueType = secondRolePlayer.IsValueType;
					
					bool firstRoleIsUnique = (firstRoleUniquenessConstraint != null);
					bool secondRoleIsUnique = (secondRoleUniquenessConstraint != null);

					// These are used to make sure that we never shallowly map towards a preferred identifier.
					// Tracking this also allows us to prefer a deep mapping when a 1-1 maps towards its only identifier.
					bool firstRoleIsUniqueAndPreferred = firstRoleIsUnique && firstRoleUniquenessConstraint.IsPreferred;
					bool secondRoleIsUniqueAndPreferred = secondRoleIsUnique && secondRoleUniquenessConstraint.IsPreferred;

					bool firstRoleIsMandatory = null != (mandatory = firstRole.SingleRoleAlethicMandatoryConstraint);
					bool firstRoleIsImplicitlyMandatory = firstRoleIsMandatory && mandatory.IsImplied;
					bool firstRoleIsExplicitlyMandatory = firstRoleIsMandatory && !firstRoleIsImplicitlyMandatory;
					bool firstRoleIsInherentlyMandatory = firstRoleIsExplicitlyMandatory && null != mandatory.InherentForObjectType;
					bool secondRoleIsMandatory = null != (mandatory = secondRole.SingleRoleAlethicMandatoryConstraint);
					bool secondRoleIsImplicitlyMandatory = secondRoleIsMandatory && mandatory.IsImplied;
					bool secondRoleIsExplicitlyMandatory = secondRoleIsMandatory && !secondRoleIsImplicitlyMandatory;
					bool secondRoleIsInherentlyMandatory = secondRoleIsExplicitlyMandatory && null != mandatory.InherentForObjectType;

					// Classify inherent mandatory constraints as implied depending on the opposite mandatory state.
					if (firstRoleIsInherentlyMandatory)
					{
						if (secondRoleIsInherentlyMandatory)
						{
							// Downgrade both constraints to implied mandatory
							firstRoleIsImplicitlyMandatory = secondRoleIsImplicitlyMandatory = true;
							firstRoleIsExplicitlyMandatory = secondRoleIsExplicitlyMandatory = false;
						}
						else if (secondRoleIsExplicitlyMandatory)
						{
							// Prioritize the explicit mandatory on the towards role by downgrading the from role to implied
							firstRoleIsImplicitlyMandatory = true;
							firstRoleIsExplicitlyMandatory = false;
						}
					}
					else if (secondRoleIsInherentlyMandatory && firstRoleIsExplicitlyMandatory)
					{
						// Prioritize the explicit mandatory on the from role by downgrading the towards role to implied
						secondRoleIsImplicitlyMandatory = true;
						secondRoleIsExplicitlyMandatory = false;
					}

					// We don't need to worry about shallow mappings towards preferred identifiers on many-to-ones, since the preferred
					// identifier pattern ensures that these cases will always map towards the object type being identified anyway.

					const int FIRST_SECOND_SHALLOW = 1;
					const int FIRST_SECOND_DEEP = 2;
					const int SECOND_FIRST_SHALLOW = 4;
					const int SECOND_FIRST_DEEP = 8;
					int possibilityBits = 0;
					bool manyToOne = false;

					// If only firstRole is unique...
					if (firstRoleIsUnique && !secondRoleIsUnique)
					{
						// Shallow map toward firstRolePlayer.
						possibilityBits |= SECOND_FIRST_SHALLOW;
						manyToOne = true;
					}
					else if (!firstRoleIsUnique && secondRoleIsUnique) // ...only secondRole is unique...
					{
						// Shallow map toward secondRolePlayer.
						possibilityBits |= FIRST_SECOND_SHALLOW;
						manyToOne = true;
					}
					else if (firstRoleIsUnique && secondRoleIsUnique) // ...both roles are unique...
					{
						// If this is a ring fact type...
						if (firstRolePlayer == secondRolePlayer)
						{
							// If only firstRole is mandatory...
							if (firstRoleIsExplicitlyMandatory && !secondRoleIsExplicitlyMandatory)
							{
								// Shallow map toward firstRolePlayer (mandatory role player).
								possibilityBits |= FIRST_SECOND_SHALLOW;
							}
							else if (!firstRoleIsExplicitlyMandatory && secondRoleIsExplicitlyMandatory) // ...only secondRole is mandatory...
							{
								// Shallow map toward secondRolePlayer (mandatory role player).
								possibilityBits |= SECOND_FIRST_SHALLOW;
							}
							else // ...otherwise...
							{
								// Shallow map toward firstRolePlayer.
								possibilityBits |= FIRST_SECOND_SHALLOW;
							}
						}
						else // ...not a ring fact type...
						{
							// If neither role is mandatory...
							if (!firstRoleIsExplicitlyMandatory && !secondRoleIsExplicitlyMandatory)
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									possibilityBits |= SECOND_FIRST_SHALLOW;
								}

								// If secondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									possibilityBits |= FIRST_SECOND_SHALLOW;
								}
							}
							else if (firstRoleIsExplicitlyMandatory && !secondRoleIsExplicitlyMandatory) // ...only firstRole is mandatory...
							{
								// Note that the first role cannot be be preferred if the second role is not mandatory
								// Shallow map toward firstRolePlayer.
								possibilityBits |= SECOND_FIRST_SHALLOW;
#if FALSE
								// UNDONE: This is a much deeper check for 1-1 patterns off of mandatory constraints. Both this and
								// the current much quicker check are meant to limit permutation checks on patterns where numerous
								// 1-1 fact types reference the same optional role player. Both of these systems are extremely fragile
								// to one of the uses of the value being made mandatory, in which case this loop does not apply.
								// We need to find a quicker mechanism for determining when we should allow an absorption towards
								// a fully optional object type, or some other check to eliminate extremely long chains (these can
								// easily consume all memory on a machine).
								bool allowMappingTowardsOptionalRole = true;
								if (null != (mandatory = secondRolePlayer.ImpliedMandatoryConstraint))
								{
									if (!(secondRoleIsUniqueAndPreferred && factType.ImpliedByObjectification != null))
									{
										foreach (Role testRole in mandatory.RoleCollection)
										{
											if (testRole != secondRole && !ShouldIgnoreFactType((testRole.Proxy ?? (RoleBase)testRole).FactType))
											{
												Role oppositeRole;
												if (testRole.SingleRoleAlethicUniquenessConstraint != null &&
													null != (oppositeRole = testRole.OppositeRoleAlwaysResolveProxy as Role) &&
													null != oppositeRole.SingleRoleAlethicUniquenessConstraint &&
													null != (mandatory = oppositeRole.SingleRoleAlethicMandatoryConstraint) &&
													!mandatory.IsImplied)
												{
													// If there are multiple 1-1 mappings towards an object type with
													// an implied mandatory, then don't consider any mappings towards
													// this entity.
													allowMappingTowardsOptionalRole = false;
													break;
												}
											}
										}
									}
								}

								if (allowMappingTowardsOptionalRole)
#endif // FALSE
								if ((null == secondRolePlayer.ImpliedMandatoryConstraint && null == secondRolePlayer.InherentMandatoryConstraint) ||
									((secondRoleIsUniqueAndPreferred && !secondRolePlayerIsValueType) || factType.ImpliedByObjectification != null))
								{
									// If secondRole is not preferred...
									if (!secondRoleIsUniqueAndPreferred)
									{
										// Shallow map toward secondRolePlayer.
										possibilityBits |= FIRST_SECOND_SHALLOW;
									}

									// Deep map toward secondRolePlayer unless the first role player
									// is auto identified.
									if (!EntityTypeIsAutoIdentified(firstRolePlayer))
									{
										possibilityBits |= FIRST_SECOND_DEEP;
									}
								}
							}
							else if (!firstRoleIsExplicitlyMandatory && secondRoleIsExplicitlyMandatory) // ...only secondRole is mandatory...
							{
								// Note that the second role cannot be preferred if the first role is not mandatory
								// Shallow map toward secondRolePlayer.
								possibilityBits |= FIRST_SECOND_SHALLOW;

#if FALSE
								// UNDONE: See comments above, duplicate code switching first and second
								bool allowMappingTowardsOptionalRole = true;
								if (null != (mandatory = firstRolePlayer.ImpliedMandatoryConstraint))
								{
									if (!(firstRoleIsUniqueAndPreferred && factType.ImpliedByObjectification != null))
									{
										foreach (Role testRole in mandatory.RoleCollection)
										{
											if (testRole != firstRole && !ShouldIgnoreFactType((testRole.Proxy ?? (RoleBase)testRole).FactType))
											{
												Role oppositeRole;
												if (testRole.SingleRoleAlethicUniquenessConstraint != null &&
													null != (oppositeRole = testRole.OppositeRoleAlwaysResolveProxy as Role) &&
													null != oppositeRole.SingleRoleAlethicUniquenessConstraint &&
													null != (mandatory = oppositeRole.SingleRoleAlethicMandatoryConstraint) &&
													!mandatory.IsImplied)
												{
													// If there are multiple 1-1 mappings towards an object type with
													// an implied mandatory, then don't consider any mappings towards
													// this entity.
													allowMappingTowardsOptionalRole = false;
													break;
												}
											}
										}
									}
								}

								if (allowMappingTowardsOptionalRole)
#endif // FALSE
								if ((null == firstRolePlayer.ImpliedMandatoryConstraint && null == firstRolePlayer.InherentMandatoryConstraint) ||
									((firstRoleIsUniqueAndPreferred && !firstRolePlayerIsValueType) || factType.ImpliedByObjectification != null))
								{
									// If firstRole is not preferred...
									if (!firstRoleIsUniqueAndPreferred)
									{
										// Shallow map toward firstRolePlayer.
										possibilityBits |= SECOND_FIRST_SHALLOW;
									}

									// Deep map toward firstRolePlayer unless the second role player
									// is auto identified.
									if (!EntityTypeIsAutoIdentified(secondRolePlayer))
									{
										possibilityBits |= SECOND_FIRST_DEEP;
									}
								}
							}
							else // ...both roles are mandatory...
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									possibilityBits |= SECOND_FIRST_SHALLOW;
								}

								// If secondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									possibilityBits |= FIRST_SECOND_SHALLOW;
								}

								// Possible deep map toward firstRolePlayer and toward secondRolePlayer,
								// depending on whether the opposite role player is auto identified.
								if (!EntityTypeIsAutoIdentified(firstRolePlayer))
								{
									possibilityBits |= FIRST_SECOND_DEEP;
								}
								if (!EntityTypeIsAutoIdentified(secondRolePlayer))
								{
									possibilityBits |= SECOND_FIRST_DEEP;
								}
							}
						}
					}
					Debug.Assert(possibilityBits != 0);
					switch (possibilityBits)
					{
						case FIRST_SECOND_SHALLOW:
							(manyToOne ? decidedManyToOneFactTypeMappings : decidedOneToOneFactTypeMappings).Add(factType, new FactTypeMapping(factType, firstRole, secondRole, inverseFactType, inverseFlags | GetFlags(false, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRoleIsUniqueAndPreferred, secondRoleIsUniqueAndPreferred)));
							break;
						case SECOND_FIRST_SHALLOW:
							(manyToOne ? decidedManyToOneFactTypeMappings : decidedOneToOneFactTypeMappings).Add(factType, new FactTypeMapping(factType, secondRole, firstRole, inverseFactType, inverseFlags | GetFlags(false, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRoleIsUniqueAndPreferred, firstRoleIsUniqueAndPreferred)));
							break;
						case FIRST_SECOND_DEEP:
							(manyToOne ? decidedManyToOneFactTypeMappings : decidedOneToOneFactTypeMappings).Add(factType, new FactTypeMapping(factType, firstRole, secondRole, inverseFactType, inverseFlags | GetFlags(true, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRoleIsUniqueAndPreferred, secondRoleIsUniqueAndPreferred)));
							break;
						case SECOND_FIRST_DEEP:
							(manyToOne ? decidedManyToOneFactTypeMappings : decidedOneToOneFactTypeMappings).Add(factType, new FactTypeMapping(factType, secondRole, firstRole, inverseFactType, inverseFlags | GetFlags(true, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRoleIsUniqueAndPreferred, firstRoleIsUniqueAndPreferred)));
							break;
						default:
							{
								// UNDONE: I don't see any reason to use a heavy-weight list structure here. The
								// same information could be stored in an UndecidedFactTypeMapping structure that
								// uses a bit field mechanism similar to this block of code. This would allow us
								// to store the factType/firstRole/secondRole once and interpret the contents based
								// on a single bitfield. In the meantime, keep the list as small as possible.
								int countBits = possibilityBits;
								int count = 0;
								while (countBits != 0)
								{
									if (0 != (countBits & 1))
									{
										++count;
									}
									countBits >>= 1;
								}
								FactTypeMappingList potentialMappingList = new FactTypeMappingList(count);
								count = -1;
								if (0 != (possibilityBits & FIRST_SECOND_SHALLOW))
								{
									potentialMappingList.Add(new FactTypeMapping(factType, firstRole, secondRole, inverseFactType, inverseFlags | GetFlags(false, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRoleIsUniqueAndPreferred, secondRoleIsUniqueAndPreferred)));
								}
								if (0 != (possibilityBits & SECOND_FIRST_SHALLOW))
								{
									potentialMappingList.Add(new FactTypeMapping(factType, secondRole, firstRole, inverseFactType, inverseFlags | GetFlags(false, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRoleIsUniqueAndPreferred, firstRoleIsUniqueAndPreferred)));
								}
								if (0 != (possibilityBits & FIRST_SECOND_DEEP))
								{
									potentialMappingList.Add(new FactTypeMapping(factType, firstRole, secondRole, inverseFactType, inverseFlags | GetFlags(true, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRoleIsUniqueAndPreferred, secondRoleIsUniqueAndPreferred)));
								}
								if (0 != (possibilityBits & SECOND_FIRST_DEEP))
								{
									potentialMappingList.Add(new FactTypeMapping(factType, secondRole, firstRole, inverseFactType, inverseFlags | GetFlags(true, secondRolePlayerIsValueType, secondRoleIsMandatory, secondRoleIsImplicitlyMandatory, firstRolePlayerIsValueType, firstRoleIsMandatory, firstRoleIsImplicitlyMandatory, secondRoleIsUniqueAndPreferred, firstRoleIsUniqueAndPreferred)));
								}
								undecidedOneToOneFactTypeMappings.Add(factType, potentialMappingList);
								break;
							}
					}
				}
			}
		}
		private void ResolveInverseFactType(FactType factType, out FactType inverseFactType, out FactTypeMappingFlags inverseMappingFlags)
		{
			inverseMappingFlags = FactTypeMappingFlags.None;
			inverseFactType = null;
			UnaryValuePattern unaryPattern = factType.UnaryPattern;
			if (unaryPattern == UnaryValuePattern.NotUnary)
			{
				factType = factType.ImpliedByObjectification?.NestedFactType;
				if (factType == null || UnaryValuePattern.NotUnary == (unaryPattern = factType.UnaryPattern))
				{
					return;
				}
			}

			switch (unaryPattern)
			{
				case UnaryValuePattern.OptionalWithoutNegation:
				case UnaryValuePattern.OptionalWithoutNegationDefaultTrue:
					break;
				case UnaryValuePattern.Negation:
					inverseFactType = factType.PositiveUnaryFactType;
					if (inverseFactType != null)
					{
						inverseMappingFlags = FactTypeMappingFlags.FactTypeIsNegation;
						switch (inverseFactType.UnaryPattern)
						{
							case UnaryValuePattern.RequiredWithNegation:
							case UnaryValuePattern.RequiredWithNegationDefaultTrue:
							case UnaryValuePattern.RequiredWithNegationDefaultFalse:
								inverseMappingFlags |= FactTypeMappingFlags.InversePairIsMandatory;
								break;
						}
					}
					break;
				case UnaryValuePattern.RequiredWithNegation:
				case UnaryValuePattern.RequiredWithNegationDefaultTrue:
				case UnaryValuePattern.RequiredWithNegationDefaultFalse:
					inverseFactType = factType.NegationUnaryFactType;
					inverseMappingFlags = FactTypeMappingFlags.InversePairIsMandatory;
					break;
				default:
					inverseFactType = factType.NegationUnaryFactType;
					break;
			}

			if (inverseFactType != null)
			{
				// The unary fact type itself is ignored if objectified, switch to the link fact type.
				if (inverseFactType.Objectification != null)
				{
					if (null == (inverseFactType = inverseFactType?.UnaryRole?.Proxy?.FactType) || ShouldIgnoreFactType(inverseFactType))
					{
						inverseFactType = null;
						inverseMappingFlags = FactTypeMappingFlags.None;
					}
				}

				if (0 == (inverseMappingFlags & FactTypeMappingFlags.InversePairIsMandatory) && inverseFactType != null)
				{
					// The UnaryPattern controls all constraints the mandatory constraints on a unary pair--except for the implied mandatory.
					// In the degenerate case where the entire table is the pair unary, there may still be an implied mandatory constraint on both roles.
					// If the implied mandatory contains one paired unary role it will contain both because they both have the same mandatory state
					Role role = factType.UnaryRole;
					LinkedElementCollection<Role> constraintRoles = role?.RolePlayer?.ImpliedMandatoryConstraint?.RoleCollection;
					if (constraintRoles != null && constraintRoles.Count == 2 && constraintRoles.Contains(role))
					{
						inverseMappingFlags |= FactTypeMappingFlags.InversePairIsMandatory;
					}
				}
			}
		}
		private static FactTypeMappingFlags GetFlags(bool deepMapping, bool fromValueType, bool fromMandatory, bool fromImpliedMandatory, bool towardsValueType, bool towardsMandatory, bool towardsImpliedMandatory, bool fromRoleSimplePreferred, bool towardsRoleSimplePreferred)
		{
			FactTypeMappingFlags flags = deepMapping ? FactTypeMappingFlags.DeepMapping : FactTypeMappingFlags.None;
			if (fromValueType)
			{
				flags |= FactTypeMappingFlags.FromValueType;
			}
			if (fromMandatory)
			{
				flags |= fromImpliedMandatory ? (FactTypeMappingFlags.FromRoleMandatory | FactTypeMappingFlags.FromRoleImpliedMandatory) : FactTypeMappingFlags.FromRoleMandatory;
			}
			if (towardsValueType)
			{
				flags |= FactTypeMappingFlags.TowardsValueType;
			}
			if (towardsMandatory)
			{
				flags |= towardsImpliedMandatory ? (FactTypeMappingFlags.TowardsRoleMandatory | FactTypeMappingFlags.TowardsRoleImpliedMandatory) : FactTypeMappingFlags.TowardsRoleMandatory;
			}
			if (fromRoleSimplePreferred)
			{
				flags |= FactTypeMappingFlags.FromRoleSimplePreferred;
			}
			if (towardsRoleSimplePreferred)
			{
				flags |= FactTypeMappingFlags.TowardsRoleSimplePreferred;
			}
			return flags;
		}

		/// <summary>
		/// Runs various algorithms on the undecided fact type mappings in an attempt to decide as many as possible.
		/// </summary>
		/// <param name="decidedManyToOneFactTypeMappings">The decided many-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="decidedOneToOneFactTypeMappings">The decided one-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void FilterFactTypeMappings(FactTypeMappingDictionary decidedManyToOneFactTypeMappings, FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			bool changed;
			do
			{
				RemoveImpossiblePotentialFactTypeMappings(decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

				changed = MapTrivialOneToOneFactTypesWithTwoMandatories(decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings) > 0;
			} while (changed);
		}

		#region Filter Algorithms Methods
		/// <summary>
		/// Filters out deep potential mappings that are from an <see cref="ObjectType"/> that already has a decided deep
		/// mapping from it.
		/// </summary>
		/// <param name="decidedOneToOneFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void RemoveImpossiblePotentialFactTypeMappings(FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			Dictionary<ObjectType, object> deeplyMappedObjectTypes = new Dictionary<ObjectType, object>(decidedOneToOneFactTypeMappings.Count + undecidedOneToOneFactTypeMappings.Count);

			// For each decided fact type mapping...
			foreach (FactTypeMapping factTypeMapping in decidedOneToOneFactTypeMappings.Values)
			{
				// If it's a deep mapping...
				if (factTypeMapping.MappingDepth == MappingDepth.Deep)
				{
					deeplyMappedObjectTypes[factTypeMapping.FromObjectType] = null;
				}
			}

			List<FactType> factsPendingDeletion = new List<FactType>();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedFactTypeMapping in undecidedOneToOneFactTypeMappings)
			{
				FactType factType = undecidedFactTypeMapping.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedFactTypeMapping.Value;

				// For each potential fact type mapping...
				for (int i = potentialFactTypeMappings.Count - 1; i >= 0; --i)
				{
					FactTypeMapping potentialFactTypeMapping = potentialFactTypeMappings[i];

					// If it is maped away from an ObjectType that is already determined to be mapped elsewhere...
					if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep &&  deeplyMappedObjectTypes.ContainsKey(potentialFactTypeMapping.FromObjectType))
					{
						// Remove it as a possibility.
						potentialFactTypeMappings.RemoveAt(i);
					}
				}

				Debug.Assert(potentialFactTypeMappings.Count > 0);

				// If there is only one possibility left...
				if (potentialFactTypeMappings.Count == 1)
				{
					// Mark it decided.
					decidedOneToOneFactTypeMappings.Add(factType, potentialFactTypeMappings[0]);
					factsPendingDeletion.Add(factType);
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factsPendingDeletion)
			{
				undecidedOneToOneFactTypeMappings.Remove(key);
			}
		}

		/// <summary>
		/// Maps one-to-one fact types with two simple alethic mandatories away from the object type that has no possible
		/// (potential or decided) deep mappings away from it.  If both role players meet this criteria then no action is
		/// taken.
		/// </summary>
		/// <param name="decidedOneToOneFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> posibilities.</param>
		/// <returns>The number of previously potential one-to-one fact type mappings that are now decided.</returns>
		private int MapTrivialOneToOneFactTypesWithTwoMandatories(FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			List<FactType> factTypesPendingDeletion = new List<FactType>();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedPair in undecidedOneToOneFactTypeMappings)
			{
				FactType factType = undecidedPair.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedPair.Value;
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;

				Debug.Assert(roles.Count == 2, "All fact type mappings should be for fact types with exactly two roles.");

				Role firstRole = roles[0].Role;
				Role secondRole = roles[1].Role;
				ObjectType firstRolePlayer = firstRole.RolePlayer;
				ObjectType secondRolePlayer = secondRole.RolePlayer;

				// If this is a one-to-one fact type with two simple alethic mandatories...
				if (firstRole.SingleRoleAlethicMandatoryConstraint != null && secondRole.SingleRoleAlethicMandatoryConstraint != null)
				{
					FactTypeMapping deepMappingTowardsFirstRole = null;
					FactTypeMapping deepMappingTowardsSecondRole = null;

					// Find our potential deep mappings.
					foreach (FactTypeMapping potentialFactTypeMapping in potentialFactTypeMappings)
					{
						if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep)
						{
							if (potentialFactTypeMapping.TowardsRole == firstRole)
							{
								deepMappingTowardsFirstRole = potentialFactTypeMapping;
							}
							else
							{
								Debug.Assert(potentialFactTypeMapping.TowardsRole == secondRole);
								deepMappingTowardsSecondRole = potentialFactTypeMapping;
							}
						}
					}

					bool firstRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(firstRolePlayer, factType, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
					bool secondRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(secondRolePlayer, factType, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

					// UNDONE: We need to do cycle checking at or before this point, since otherwise this can create a decided deep mapping cycle.

					// If secondRolePlayer has no possible deep mappings away from it, and firstRolePlayer does...
					if (firstRolePlayerHasPossibleDeepMappingsAway && !secondRolePlayerHasPossibleDeepMappingsAway)
					{
						// Make sure that this deep mapping was one of our potentials.
						if (deepMappingTowardsFirstRole != null)
						{
							Debug.Assert(firstRole.SingleRoleAlethicUniquenessConstraint != null);
							// Make sure we're not going towards a preferred identifier. Doing so is valid, and sometimes optimal,
							// but not always, hence the permuter needs to consider it.
							if (!firstRole.SingleRoleAlethicUniquenessConstraint.IsPreferred)
							{
								// Deep map toward firstRolePlayer
								decidedOneToOneFactTypeMappings.Add(factType, deepMappingTowardsFirstRole);
								factTypesPendingDeletion.Add(factType);
							}
						}
					}
					// ...and vice-versa...
					else if (!firstRolePlayerHasPossibleDeepMappingsAway && secondRolePlayerHasPossibleDeepMappingsAway)
					{
						// Make sure that this deep mapping was one of our potentials.
						if (deepMappingTowardsSecondRole != null)
						{
							Debug.Assert(secondRole.SingleRoleAlethicUniquenessConstraint != null);
							// Make sure we're not going towards a preferred identifier. Doing so is valid, and sometimes optimal,
							// but not always, hence the permuter needs to consider it.
							if (!secondRole.SingleRoleAlethicUniquenessConstraint.IsPreferred)
							{
								// Deep map toward secondRolePlayer
								decidedOneToOneFactTypeMappings.Add(factType, deepMappingTowardsSecondRole);
								factTypesPendingDeletion.Add(factType);
							}
						}
					}
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factTypesPendingDeletion)
			{
				undecidedOneToOneFactTypeMappings.Remove(key);
			}

			return factTypesPendingDeletion.Count;
		}

		/// <summary>
		/// Determins wheather or not an object type has any possible (potential or decided)
		/// deep fact type mappings away from it.
		/// </summary>
		/// <param name="objectType">The object type to consider</param>
		/// <param name="excludedFactType">The fact type to ignore.  This parameter may be null.</param>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		/// <returns>True if objectType has possible deep mappings away from it, otherwise false.</returns>
		private bool ObjectTypeHasPossibleDeepMappingsAway(ObjectType objectType, FactType excludedFactType, FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			LinkedElementCollection<Role> rolesPlayed = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

			foreach (Role rolePlayed in rolesPlayed)
			{
				// NOTE: We don't need the ShouldIgnoreFactType filter here, because fact types that we want to ignore won't be in the dictionaries in the first place.
				FactType factType = rolePlayed.BinarizedOrSameFactType;

				if (factType == excludedFactType)
				{
					continue;
				}

				FactTypeMapping decidedFactTypeMapping;
				bool decidedFactTypeMappingExists = decidedFactTypeMappings.TryGetValue(factType, out decidedFactTypeMapping);
				FactTypeMappingList potentialFactTypeMappings;
				bool potentialFactTypeMappingsExist = undecidedFactTypeMappings.TryGetValue(factType, out potentialFactTypeMappings);

				// If there's a decided deep fact type mapping away from objectType...
				if (decidedFactTypeMappingExists && decidedFactTypeMapping.MappingDepth == MappingDepth.Deep && decidedFactTypeMapping.FromObjectType == objectType)
				{
					return true;
				}
				else if (potentialFactTypeMappingsExist)
				{
					foreach (FactTypeMapping potentialFactTypeMapping in potentialFactTypeMappings)
					{
						// If there's a potential deep fact type mapping away from objectType...
						if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep && potentialFactTypeMapping.FromObjectType == objectType)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
		#endregion // Filter Algorithms Methods

		/// <summary>
		/// Populates this <see cref="AbstractionModel"/> given the decided <see cref="FactTypeMapping"/> objects.
		/// </summary>
		/// <param name="factTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		private void GenerateOialModel(FactTypeMappingDictionary factTypeMappings)
		{
			GenerateInformationTypeFormats();
			GenerateConceptTypes(factTypeMappings);
			GenerateConceptTypeChildren(factTypeMappings);
			GenerateFactTypeMappings(factTypeMappings);
			GenerateUniqueness(factTypeMappings);
			GenerateAssociations();
		}

		#region Generation Algorithm Methods
		/// <summary>
		/// Generates the <see cref="InformationTypeFormat"/> objects and adds them to the model.
		/// </summary>
		private void GenerateInformationTypeFormats()
		{
			ORMModel model = this.ORMModel;
			IEnumerable<ObjectType> modelValueTypes = model.ValueTypeCollection;
			AbstractionModel oialModel = this.AbstractionModel;

			// For each ValueType in the model...
			foreach (ObjectType valueType in modelValueTypes)
			{
				if (ShouldIgnoreObjectType(valueType))
				{
					continue;
				}
				// Create InformationTypeFormat.
				PropertyAssignment namePropertyAssignment = new PropertyAssignment(InformationTypeFormat.NameDomainPropertyId, valueType.Name);
				InformationTypeFormat informationTypeFormat = new InformationTypeFormat(Store, namePropertyAssignment);
				InformationTypeFormatIsForValueType informationTypeFormatIsForValueType = new InformationTypeFormatIsForValueType(informationTypeFormat, valueType);
				// TODO: information type format data types

				// Add it to the model.
				oialModel.InformationTypeFormatCollection.Add(informationTypeFormat);
			}
		}

		/// <summary>
		/// Generages the <see cref="ConceptType"/> objects along with any relationships that they have and adds them to the
		/// model.
		/// </summary>
		/// <param name="factTypeMappings">A dictionary of all the final decided FactTypeMapping objects.</param>
		private void GenerateConceptTypes(FactTypeMappingDictionary factTypeMappings)
		{
			ORMModel model = this.ORMModel;
			LinkedElementCollection<ObjectType> modelObjectTypes = model.ObjectTypeCollection;
			AbstractionModel oialModel = this.AbstractionModel;

			// For each object type in the model...
			foreach (ObjectType objectType in modelObjectTypes)
			{
				if (ShouldIgnoreObjectType(objectType))
				{
					continue;
				}
				// If it should have a concept type...
				if (ObjectTypeIsConceptType(objectType, factTypeMappings))
				{
					// Create the ConceptType object.
					PropertyAssignment name = new PropertyAssignment(ConceptType.NameDomainPropertyId, objectType.Name);
					ConceptType conceptType = new ConceptType(Store, name);
					ConceptTypeIsForObjectType conceptTypeIsForObjectType = new ConceptTypeIsForObjectType(conceptType, objectType);

					// Add it to the model.
					oialModel.ConceptTypeCollection.Add(conceptType);

					// If this conceptType is for a ValueType...
					if (objectType.IsValueType)
					{
						InformationTypeFormat valueTypeInformationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(objectType);

						RoleAssignment conceptTypeRole = new RoleAssignment(InformationType.ConceptTypeDomainRoleId, conceptType);
						RoleAssignment informationTypeFormat = new RoleAssignment(InformationType.InformationTypeFormatDomainRoleId, valueTypeInformationTypeFormat);
						RoleAssignment[] roleAssignments = { conceptTypeRole, informationTypeFormat };
						PropertyAssignment isMandatory = new PropertyAssignment(InformationType.IsMandatoryDomainPropertyId, true);
						PropertyAssignment informationTypeNameProperty = new PropertyAssignment(InformationType.NameDomainPropertyId, String.Concat(objectType.Name, "Value"));
						PropertyAssignment[] informationTypePropertyAssignments = { isMandatory, informationTypeNameProperty };

						// ConceptType for conceptType gets an InformationType that references InformationTypeFormat for conceptType.
						InformationType informationType = new InformationType(Store, roleAssignments, informationTypePropertyAssignments);

						PropertyAssignment uniquenessNameProperty = new PropertyAssignment(Uniqueness.NameDomainPropertyId, String.Concat(objectType.Name, "Uniqueness"));
						PropertyAssignment isPreferred = new PropertyAssignment(Uniqueness.IsPreferredDomainPropertyId, true);
						PropertyAssignment[] uniquenessPropertyAssignments = { uniquenessNameProperty, isPreferred };

						// Uniqueness constraint
						Uniqueness uniqueness = new Uniqueness(Store, uniquenessPropertyAssignments);
						UniquenessIncludesConceptTypeChild uniquenessIncludesConceptTypeChild = new UniquenessIncludesConceptTypeChild(uniqueness, informationType);

						conceptType.UniquenessCollection.Add(uniqueness);
					}
				}
			}
		}

		/// <summary>
		/// Generates the <see cref="ConceptTypeChild">concept type children</see> for each
		/// <see cref="ConceptType"/> in the <see cref="AbstractionModel">OIAL model</see>.
		/// </summary>
		/// <param name="factTypeMappings">
		/// The set of all decided <see cref="FactTypeMapping">fact type mappings</see>.
		/// </param>
		private void GenerateConceptTypeChildren(FactTypeMappingDictionary factTypeMappings)
		{
			List<FactType> factTypePath = new List<FactType>();
			Dictionary<FactType, ConceptTypeChild> inverseChildrenByFactType = new Dictionary<FactType, ConceptTypeChild>();

			foreach (ConceptType conceptType in this.AbstractionModel.ConceptTypeCollection)
			{
				ObjectType objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType);
				bool? conceptTypeHasDeepMappingAway = null;

				// NOTE: We don't need the ShouldIgnoreObjectType filter here, because object
				// types that we want to ignore won't be in the OIAL model in the first place.

				foreach (Role playedRole in objectType.PlayedRoleCollection)
				{
					// NOTE: We don't need the ShouldIgnoreFactType filter here, because fact types that
					// we want to ignore won't be in the set of fact type mappings in the first place.
					FactType playedFactType = playedRole.BinarizedOrSameFactType;

					FactTypeMapping factTypeMapping;

					if (factTypeMappings.TryGetValue(playedFactType, out factTypeMapping) &&
						factTypeMapping.TowardsRole == playedRole)
					{
						// The fact type has a mapping and that mapping is towards the role played by
						// this concept type, so we need to generate concept type children for it.
						GenerateConceptTypeChildrenForFactTypeMapping(factTypeMappings, conceptType, ref conceptTypeHasDeepMappingAway, factTypeMapping, factTypePath, true, inverseChildrenByFactType);
					}
				}
			}
		}

		/// <summary>
		/// Generates the appropriate <see cref="ConceptTypeChild">concept type children</see> in <paramref name="parentConceptType"/>
		/// for <paramref name="factTypeMapping"/>.
		/// </summary>
		/// <param name="factTypeMappings">
		/// The set of all decided <see cref="FactTypeMapping">fact type mappings</see>.
		/// </param>
		/// <param name="parentConceptType">
		/// The <see cref="ConceptType"/> into which <see cref="ConceptTypeChild">concept type children</see> should be generated.
		/// </param>
		/// <param name="parentConceptTypeHasDeepAway">
		/// Test if the parent concept type has a deep mapping away from it. Handles some cyclic cases by making a potential assimilation
		/// into a reference. Delay calculated because this is not always needed.
		/// </param>
		/// <param name="factTypeMapping">
		/// The <see cref="FactTypeMapping"/> for which <see cref="ConceptTypeChild">concept type children</see> should be generated.
		/// </param>
		/// <param name="factTypePath">
		/// The path of <see cref="FactType">fact types</see> leading from <paramref name="parentConceptType"/> to <paramref name="factTypeMapping"/>
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether every step in <paramref name="factTypePath"/> is mandatory for the parent concept type (towards object type).
		/// </param>
		/// <param name="inverseChildrenByFactType">
		/// Provide a dictionary a fact types that map to a concept type child used in an inverse (unary positive/negative) pairing. This allows
		/// the pairs to be created in any order.
		/// </param>
		private static void GenerateConceptTypeChildrenForFactTypeMapping(FactTypeMappingDictionary factTypeMappings, ConceptType parentConceptType, ref bool? parentConceptTypeHasDeepAway, FactTypeMapping factTypeMapping, List<FactType> factTypePath, bool isMandatorySoFar, Dictionary<FactType, ConceptTypeChild> inverseChildrenByFactType)
		{
			// Push the current fact type onto the path.
			factTypePath.Add(factTypeMapping.FactType);

			bool isMandatory = isMandatorySoFar && (factTypeMapping.TowardsRoleMandatory);

			ConceptTypeChild newConceptTypeChild;

			ConceptType fromConceptType;
			bool unaryMapping = factTypeMapping.FromRole == null;
			if (!unaryMapping && null != (fromConceptType = ConceptTypeIsForObjectType.GetConceptType(factTypeMapping.FromObjectType)))
			{
				// The mapping is coming from a concept type, so we will create a concept type reference to it.

				// Set up the property assignments that are common to both kinds of concept type references.
				PropertyAssignment isMandatoryPropertyAssignment = new PropertyAssignment(ConceptTypeChild.IsMandatoryDomainPropertyId, isMandatory);

				Role fromRole = factTypeMapping.FromRole;
				Role toRole = factTypeMapping.TowardsRole;
				RoleBase fromRoleBase = fromRole;
				RoleBase toRoleBase = toRole;
				RoleProxy proxy;
				if (null != (proxy = fromRole.Proxy))
				{
					if (toRole.FactType != fromRole.FactType)
					{
						fromRoleBase = proxy;
					}
				}
				else if (null != (proxy = toRole.Proxy) &&
					toRole.FactType != fromRole.FactType)
				{
					toRoleBase = proxy;
				}

				string name = ResolveRoleName(fromRoleBase);
				string oppositeName = ResolveRoleName(toRoleBase);

				// UNDONE: Yes, these are backwards, but they need to remain so for compatibility reasons until we do a file format change.
				// Pattern also followed in UpdateChildNamesForFactTypeDelayed
				PropertyAssignment namePropertyAssignment = new PropertyAssignment(ConceptTypeChild.NameDomainPropertyId, oppositeName);
				PropertyAssignment oppositeNamePropertyAssignment = new PropertyAssignment(ConceptTypeReferencesConceptType.OppositeNameDomainPropertyId, name);

				if (factTypeMapping.MappingDepth == MappingDepth.Deep)
				{
					// Since this is a deep mapping, we will create a concept type assimilation for it.

					SubtypeFact subtypeFact = factTypeMapping.FactType as SubtypeFact;

					// UNDONE: The handling here for IsPreferredForParent and IsPreferredForTarget may not be correct
					// if we have more than one fact type in the fact type path.

					bool isPreferredForTarget;
					if (subtypeFact != null)
					{
						// For subtype assimilations, IsPreferredForTarget matches the ProvidesPreferredIdentifier
						// property of the ORM subtype fact.
						isPreferredForTarget = subtypeFact.ProvidesPreferredIdentifier;
					}
					else
					{
						// For non-subtype assimilations, IsPreferredForTarget is true if the role played by the object
						// type corresponding to the parent concept type has the preferred identifying uniqueness constraint
						// for the target concept type.
						isPreferredForTarget = factTypeMapping.TowardsRole.SingleRoleAlethicUniquenessConstraint.IsPreferred;
					}

					bool isPreferredForParent = factTypeMapping.IsFromPreferredIdentifier;
					// The IsPreferredForParent property on concept type assimilations indicates that the assimilation, on its own,
					// provides the preferred identifier for the assimilating concept type. Although the IsFromPreferredIdentifier
					// property on the fact type mapping will be true even if the from role is part of a multi-role preferred identifier,
					// ORM currently doesn't allow a role with a single role alethic uniqueness constraint (which is required for this to
					// be a deep mapping) to be part of any other uniqueness constraint. However, this may change in the future as our
					// handling of derivations, implications, equivalences, and logical rules becomes more sophisticated. We assert here
					// in order to make this case easier to catch if it happens, since this method may need to be adjusted in that case
					// to ensure that it continues to produce correct results.

					// This assert assumes an error-free ORM state, which cannot be assumed here. An external uniqueness can be set as
					// preferred when it is implied by a single-role internal uniqueness, which triggers this assert.
					//Debug.Assert(!isPreferredForParent || factTypeMapping.FromRole.SingleRoleAlethicUniquenessConstraint.IsPreferred);

					newConceptTypeChild = new ConceptTypeAssimilatesConceptType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId, fromConceptType)
						},
						new PropertyAssignment[]
						{
							isMandatoryPropertyAssignment,
							namePropertyAssignment,
							oppositeNamePropertyAssignment,
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId, subtypeFact != null),
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId, isPreferredForParent),
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId, isPreferredForTarget),
						});
				}
				else
				{
					Debug.Assert(factTypeMapping.MappingDepth == MappingDepth.Shallow,
						"Collapse mappings should not come from object types that have a concept type.");
					
					// Since this is a shallow mapping, we will create a concept type relation for it.
					newConceptTypeChild = new ConceptTypeRelatesToConceptType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId, fromConceptType)
						},
						new PropertyAssignment[]
						{
							isMandatoryPropertyAssignment,
							namePropertyAssignment,
							oppositeNamePropertyAssignment
						});
				}
			}
			else
			{
				// The mapping is not coming from a concept type, meaning that we either need an information
				// type for an atomic value type (which will already have an information type format created
				// for it), or we need to collapse the preferred identifier of an entity type or structured
				// value type.

				InformationTypeFormat fromInformationTypeFormat;
				if (unaryMapping)
				{
					bool isNegation = factTypeMapping.FactType.UnaryPattern == UnaryValuePattern.Negation;
					AbstractionModel model = parentConceptType.Model;
					fromInformationTypeFormat = isNegation ? (InformationTypeFormat)model.NegativeUnaryInformationTypeFormat : model.PositiveUnaryInformationTypeFormat;

					if (fromInformationTypeFormat == null)
					{
						if (isNegation)
						{
							fromInformationTypeFormat = new NegativeUnaryInformationTypeFormat(parentConceptType.Partition);
							fromInformationTypeFormat.Name = "_negative_unary";
						}
						else
						{
							fromInformationTypeFormat = new PositiveUnaryInformationTypeFormat(parentConceptType.Partition);
							fromInformationTypeFormat.Name = "_positive_unary";
						}
						fromInformationTypeFormat.Model = model;
					}
				}
				else
				{
					fromInformationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(factTypeMapping.FromObjectType);
				}
				if (fromInformationTypeFormat != null)
				{
					// We have an information type format, which means that we need to create an information type.

					string name = ResolveRoleName(unaryMapping ? factTypeMapping.TowardsRole : factTypeMapping.FromRole);

					newConceptTypeChild = new InformationType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(InformationType.ConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(InformationType.InformationTypeFormatDomainRoleId, fromInformationTypeFormat)
						},
						new PropertyAssignment[]
						{
							new PropertyAssignment(ConceptTypeChild.IsMandatoryDomainPropertyId, isMandatory),
							new PropertyAssignment(ConceptTypeChild.NameDomainPropertyId, name)
						});
				}
				else
				{
					// We do not have an information type format, which means that we need to collapse the fact
					// types in the preferred identifier of the FromObjectType into the parent concept type.

					newConceptTypeChild = null;

					UniquenessConstraint preferredIdentifier = factTypeMapping.FromObjectType.PreferredIdentifier ?? factTypeMapping.FromObjectType.ResolvedPreferredIdentifier;
					Debug.Assert(preferredIdentifier != null);
					LinkedElementCollection<Role> pidRoles = preferredIdentifier.RoleCollection;

					foreach (Role preferredIdentifierRole in pidRoles)
					{
						// NOTE: We don't need the ShouldIgnoreFactType filter here, because we would have ignored
						// this object type if we were ignoring any of the fact types in its preferred identifier.
						FactType preferredIdentifierFactType = preferredIdentifierRole.BinarizedOrSameFactType;

						FactTypeMapping preferredIdentifierFactTypeMapping = factTypeMappings[preferredIdentifierFactType];

						if (preferredIdentifierFactType == factTypeMapping.FactType)
						{
							// We just got back to the fact that we were already mapping. This should only happen
							// when the object type has a single fact type in its preferred identifier and it is
							// deeply mapped away from the object type.
							Debug.Assert(preferredIdentifier.RoleCollection.Count == 1 && preferredIdentifierFactTypeMapping.MappingDepth == MappingDepth.Deep);

							// UNDONE: For now, we just ignore this fact type entirely. What we should be doing is:
							// 1) If everything along the path is mandatory, then we're done, since we know that instances of the parent
							// concept type always identify an instance of the object type that we're trying to map.
							// 2) Otherwise, check if there are any other relationships that would allow use to derive whether an instance
							// of the parent concept type identifies an instance of the object type that we're trying to map. Examples
							// of things that would allow us to do this would be a mandatory role played by the object type that we're
							// we're trying to map that gets absorbed into some concept type. The reference to an instance of this concept
							// type from it allows us to tell if this instance identifies an instance of the object type.
							// 3) If no other relationship allows us to derive this information, we need to add a boolean information type
							// that indicates for each instance of the parent concept type whether it identifies an instance of the object
							// type that we're trying to map.
							break;
						}

						// If we have a single fact type in the preferred identifier, it might be mapped
						// deeply away from the object type that we are collapsing. For this case, we need
						// to create a "fake" mapping and process it instead.
						if (preferredIdentifierFactTypeMapping.TowardsRole == preferredIdentifierRole)
						{
							// Make sure this is actually the situation we are trying to handle, since it shouldn't be possible in any other scenario.
							Debug.Assert(preferredIdentifier.RoleCollection.Count == 1 && preferredIdentifierFactTypeMapping.MappingDepth == MappingDepth.Deep);

							// UNDONE: Would we ever want to use a depth other than shallow here? Probably not, but it might be worth looking in to.
							FactTypeMappingFlags currentFlags = preferredIdentifierFactTypeMapping.Flags;
							preferredIdentifierFactTypeMapping = new FactTypeMapping(preferredIdentifierFactType, preferredIdentifierFactTypeMapping.TowardsRole, preferredIdentifierFactTypeMapping.FromRole, null, (currentFlags & FactTypeMappingFlags.Subtype) | GetFlags(false, 0 != (currentFlags & FactTypeMappingFlags.TowardsValueType), 0 != (currentFlags & FactTypeMappingFlags.TowardsRoleMandatory), 0 != (currentFlags & FactTypeMappingFlags.TowardsRoleImpliedMandatory), 0 != (currentFlags & FactTypeMappingFlags.FromValueType), 0 != (currentFlags & FactTypeMappingFlags.FromRoleMandatory), 0 != (currentFlags & FactTypeMappingFlags.FromRoleImpliedMandatory), pidRoles.Count == 1, false));
						}
						else if (preferredIdentifierFactTypeMapping.MappingDepth == MappingDepth.Deep)
						{
							// Handle cyclic deep mapping scenario with collapsed entities.
							// The primary scenario here is:
							// 1) B is a subtype of A and identified by A's identifier
							// 2) A and B participate in an objectified 1-1 FactType
							// 3) The uniqueness constraint on the A role is the preferred identifier
							// 4) The A role is mandatory
							// In this case, without this code, you get an assimilation mapping B into A
							// and mapping A into B. We fix this case by forwarding a shallow mapping,
							// which generates a reference instad of an assimilation.
							if (!parentConceptTypeHasDeepAway.HasValue)
							{
								ObjectType objectType = ConceptTypeIsForObjectType.GetObjectType(parentConceptType);
								foreach (Role role in ConceptTypeIsForObjectType.GetObjectType(parentConceptType).PlayedRoleCollection)
								{
									FactType factType;
									FactTypeMapping testMapping;
									if (null != (factType = role.BinarizedOrSameFactType) &&
										factTypeMappings.TryGetValue(factType, out testMapping) &&
										testMapping.MappingDepth == MappingDepth.Deep &&
										testMapping.FromObjectType == objectType)
									{
										preferredIdentifierFactTypeMapping = new FactTypeMapping(preferredIdentifierFactType, preferredIdentifierFactTypeMapping.FromRole, preferredIdentifierFactTypeMapping.TowardsRole, null, preferredIdentifierFactTypeMapping.Flags & ~FactTypeMappingFlags.DeepMapping);
										parentConceptTypeHasDeepAway = true;
										break;
									}
								}
								if (!parentConceptTypeHasDeepAway.HasValue)
								{
									parentConceptTypeHasDeepAway = false;
								}
							}
							else if (parentConceptTypeHasDeepAway.Value)
							{
								preferredIdentifierFactTypeMapping = new FactTypeMapping(preferredIdentifierFactType, preferredIdentifierFactTypeMapping.FromRole, preferredIdentifierFactTypeMapping.TowardsRole, null, preferredIdentifierFactTypeMapping.Flags & ~FactTypeMappingFlags.DeepMapping);
							}
						}

						GenerateConceptTypeChildrenForFactTypeMapping(factTypeMappings, parentConceptType, ref parentConceptTypeHasDeepAway, preferredIdentifierFactTypeMapping, factTypePath, isMandatory, inverseChildrenByFactType);
					}
				}
			}

			// If we created a new concept type child, populate its fact type path.
			if (newConceptTypeChild != null)
			{
				FactType inverseFactType = factTypeMapping.InverseFactType;
				if (inverseFactType != null)
				{
					ConceptTypeChild pairedChild;
					if (inverseChildrenByFactType.TryGetValue(inverseFactType, out pairedChild))
					{
						if (pairedChild != null)
						{
							inverseChildrenByFactType[inverseFactType] = null; // Sanity protected, should not be hit.

							FactTypeMappingFlags flags = factTypeMapping.Flags;
							InverseConceptTypeChild inverseChildRef = 0 != (flags & FactTypeMappingFlags.FactTypeIsNegation) ?
								new InverseConceptTypeChild(pairedChild, newConceptTypeChild) :
								new InverseConceptTypeChild(newConceptTypeChild, pairedChild);
							if (0 != (flags & FactTypeMappingFlags.InversePairIsMandatory))
							{
								inverseChildRef.PairIsMandatory = true;
							}
						}
					}
					else
					{
						inverseChildrenByFactType[factTypeMapping.FactType] = newConceptTypeChild;
					}
				}

				foreach (FactType pathFactType in factTypePath)
				{
					ConceptTypeChildHasPathFactType conceptTypeChildHasPathFactType = new ConceptTypeChildHasPathFactType(newConceptTypeChild, pathFactType);
				}
			}

			// Pop the current fact type off of the path.
			Debug.Assert(factTypePath[factTypePath.Count - 1] == factTypeMapping.FactType, "Fact type path stack is corrupt.");
			factTypePath.RemoveAt(factTypePath.Count - 1);
		}

		/// <summary>
		/// Generates the <see cref="Uniqueness"/> objects for the <see cref="AbstractionModel"/>.
		/// </summary>
		/// <param name="factTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		private void GenerateUniqueness(FactTypeMappingDictionary factTypeMappings)
		{
			// TODO: clean this up.
			AbstractionModel oialModel = this.AbstractionModel;

			// For each concept type in the model...
			foreach (ConceptType conceptType in oialModel.ConceptTypeCollection)
			{
				ObjectType objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType);

				// For each role played by its object type...
				foreach (Role role in objectType.PlayedRoleCollection)
				{
					RoleBase oppositeRoleBase;
					if (ShouldIgnoreFactType(role.BinarizedOrSameFactType) || null == (oppositeRoleBase = role.OppositeRoleAlwaysResolveProxy))
					{
						continue;
					}

					Role oppositeRole = oppositeRoleBase.Role;

					// For each constraint on the opposite role...
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uniquenessConstraint = constraintRoleSequence as UniquenessConstraint;

						// If it is a uniqueness constraint...
						if (uniquenessConstraint != null && uniquenessConstraint.Modality == ConstraintModality.Alethic)
						{
							if (UniquenessIsForUniquenessConstraint.GetUniqueness(uniquenessConstraint) != null)
							{
								continue;
							}

							bool hasFactTypeThatShouldBeIgnored = false;
							bool allChildrenMapTowardObjectType = true;
							IList<FactType> factTypes = new List<FactType>();

							foreach (Role childRole in uniquenessConstraint.RoleCollection)
							{
								FactType binarizedFactType = childRole.BinarizedOrSameFactType;
								if (ShouldIgnoreFactType(binarizedFactType))
								{
									hasFactTypeThatShouldBeIgnored = true;
									break;
								}
								FactTypeMapping factTypeMapping = factTypeMappings[binarizedFactType];

								RoleBase oppositeChildBase = childRole.OppositeRoleAlwaysResolveProxy;
								if (factTypeMapping.TowardsRole != (oppositeChildBase != null ? oppositeChildBase.Role : childRole))
								{
									allChildrenMapTowardObjectType = false;
									break;
								}
								else
								{
									factTypes.Add(binarizedFactType);
								}
							}
							if (hasFactTypeThatShouldBeIgnored)
							{
								continue;
							}

							if (allChildrenMapTowardObjectType)
							{
								IList<ConceptTypeChild> conceptTypeChildren = new List<ConceptTypeChild>();
								bool skipThisUniquenessConstraint = false;

								foreach (FactType factType in factTypes)
								{
									bool childWasAssimilation = false;
									bool missedChild = true;
									foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
									{
										if (conceptTypeChild.Parent != conceptType)
										{
											// This ConceptTypeChild is of a different ConceptType, so go on to the next ConceptTypeChild.
											continue;
										}
										if (conceptTypeChild is ConceptTypeAssimilatesConceptType)
										{
											childWasAssimilation = true;
											break;
										}

										missedChild = false;
										conceptTypeChildren.Add(conceptTypeChild);
									}

									if (childWasAssimilation)
									{
										skipThisUniquenessConstraint = true;
										break;
									}
									if (missedChild)
									{
										// We couldn't find a ConceptTypeChild for this FactType, so just bail out.
										skipThisUniquenessConstraint = true;
										break;
									}
								}

								if (!skipThisUniquenessConstraint)
								{
									PropertyAssignment name = new PropertyAssignment(Uniqueness.NameDomainPropertyId, uniquenessConstraint.Name);
									PropertyAssignment isPreferred = new PropertyAssignment(Uniqueness.IsPreferredDomainPropertyId, uniquenessConstraint.IsPreferred);
									PropertyAssignment[] propertyAssignments = { name, isPreferred };

									// Create uniquenesss
									Uniqueness uniqueness = new Uniqueness(Store, propertyAssignments);
									uniqueness.ConceptType = conceptType;
									new UniquenessIsForUniquenessConstraint(uniqueness, uniquenessConstraint);

									foreach (ConceptTypeChild conceptTypeChild in conceptTypeChildren)
									{
										UniquenessIncludesConceptTypeChild uniquenessIncludesConceptTypeChild = new UniquenessIncludesConceptTypeChild(uniqueness, conceptTypeChild);
									}
								}
							}
						}
					}
				}
			}
		}

		private void GenerateFactTypeMappings(FactTypeMappingDictionary factTypeMappings)
		{
			foreach (FactTypeMapping mapping in factTypeMappings.Values)
			{
				// Note that there will currently be no existing maps, but this is
				// likely to change once we do more incremental work. If the FactTypeMapsTowardsRole
				// instance already exists for this FactType, then the following will throw.
				// Any role changes here need to properly update the existing link's mapping
				// pattern properties. This should be done with an OnRolePlayerChanged call
				// (or a rule) inside FactTypeMapsTowardsRole.
				FactTypeMapsTowardsRole.Create(mapping.FactType, mapping.TowardsRole, mapping.MappingDepth);
			}
		}

		private void GenerateAssociations()
		{
			AbstractionModel oialModel = this.AbstractionModel;
			foreach (ConceptType ct in oialModel.ConceptTypeCollection)
			{
				ObjectType ot = ConceptTypeIsForObjectType.GetObjectType(ct);
				Objectification objectification = ot.Objectification;
				if (objectification != null)
				{
					LinkedElementCollection<ConceptTypeChild> associationChildren = ConceptTypeHasChildAsPartOfAssociation.GetTargetCollection(ct);

					// CT becomes an associationChild for all concept types related to the binarized fact types
					foreach (FactType factType in objectification.ImpliedFactTypeCollection)
					{
						foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
						{
							if (conceptTypeChild.Parent == ct || conceptTypeChild.Target == ct)
							{
								associationChildren.Add(conceptTypeChild);
							}
						} 
					}
				}
			}
		}
		#endregion // Generation Algorithm Methods
		#region Helper Methods
		/// <summary>
		/// Resolve the name that will be used for a <see cref="ConceptTypeChild"/> given the <see cref="RoleBase"/>
		/// for the role it results from.
		/// </summary>
		/// <param name="roleBase">The <see cref="RoleBase"/> that the <see cref="ConceptTypeChild"/> is resulting from.</param>
		/// <returns>The name to use for the <see cref="ConceptTypeChild"/>.</returns>
		private static string ResolveRoleName(RoleBase roleBase)
		{
			// HACK: This is only here until we implement a better alternative.
			Role role = roleBase.Role;
			string name = role.Name;
			string defaultUnaryName = null;

			if (String.IsNullOrEmpty(name))
			{
				name = role.RolePlayer.Name;
				for (;;)
				{
					FactType factType;
					if (null != (factType = roleBase.FactType))
					{
						bool snapUnaryReading;
						bool unaryNegation;
						switch (factType.UnaryPattern)
						{
							case UnaryValuePattern.NotUnary:
								snapUnaryReading = unaryNegation = false;
								break;
							case UnaryValuePattern.Negation:
								snapUnaryReading = unaryNegation = true;
								break;
							default:
								snapUnaryReading = true;
								unaryNegation = false;
								break;
						}

						foreach (ReadingOrder order in factType.ReadingOrderCollection)
						{
							int roleIndex;
							LinkedElementCollection<RoleBase> roles;
							if (-1 != (roleIndex = (roles = order.RoleCollection).IndexOf(roleBase)))
							{
								foreach (Reading reading in order.ReadingCollection)
								{
									string formatText;
									if (null != (formatText = VerbalizationHyphenBinder.GetFormatStringForHyphenBoundRole(reading.Text, roleIndex)))
									{
										return string.Format(CultureInfo.InvariantCulture, formatText, name);
									}

									if (snapUnaryReading)
									{
										snapUnaryReading = false;
										defaultUnaryName = string.Format(reading.Text, string.Empty).Trim();
									}
								}
							}
						}

						if (snapUnaryReading && unaryNegation)
						{
							factType = factType.PositiveUnaryFactType;
							if (factType != null)
							{
								foreach (ReadingOrder order in factType.ReadingOrderCollection)
								{
									foreach (Reading reading in order.ReadingCollection)
									{
										defaultUnaryName = "not " + string.Format(reading.Text, string.Empty).Trim();
										break;
									}
									break;
								}
							}
						}
					}
					if ((object)roleBase == role)
					{
						break;
					}
					// If we didn't find a hyphen-bound name on the link fact types, then
					// go ahead and look for the name on the objectified fact type.
					roleBase = role;
				}
			}

			return defaultUnaryName ?? name;
		}

		/// <summary>
		/// Determins wheather an <see cref="ObjectType"/> is destined to be a top-level <see cref="ConceptType"/> or not.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to test.</param>
		/// <param name="factTypeMappings">A dictionary of all the final decided <see cref="FactTypeMapping"/> objects.</param>
		/// <returns>Returns true if the <see cref="ObjectType"/> is destined to be a top-level <see cref="ConceptType"/>.</returns>
		private bool ObjectTypeIsTopLevelConceptType(ObjectType objectType, FactTypeMappingDictionary factTypeMappings)
		{
			if (ObjectTypeIsConceptType(objectType, factTypeMappings))
			{
				LinkedElementCollection<Role> roles = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

				foreach (Role role in roles)
				{
					FactType factType = role.BinarizedOrSameFactType;
					if (ShouldIgnoreFactType(factType))
					{
						continue;
					}
					FactTypeMapping factTypeMapping = factTypeMappings[factType];

					if (factTypeMapping.FromObjectType == objectType && factTypeMapping.MappingDepth == MappingDepth.Deep)
					{
						return false;
					}
				}
				return true;
			}

			return false;
		}

		/// <summary>
		/// Determins wheather an <see cref="ObjectType"/> should have a <see cref="ConceptType"/> or not.
		/// </summary>
		/// <remarks>
		/// An <see cref="ObjectType"/> should have a <see cref="ConceptType"/> if:
		/// <list>
		/// <item><description>It is independent.</description></item>
		/// <item><description>It is a subtype.</description></item>
		/// <item><description>It has a <see cref="FactTypeMapping"/> towards it for a <see cref="FactType"/> that is not part of its preferred identifier.</description></item>
		/// </list>
		/// </remarks>
		/// <param name="objectType">The <see cref="ObjectType"/> to test.</param>
		/// <param name="factTypeMappings">A dictionary of all the final decided <see cref="FactTypeMapping"/> objects.</param>
		/// <returns>Returns true if the <see cref="ObjectType"/> should have <see cref="ConceptType"/>.</returns>
		private bool ObjectTypeIsConceptType(ObjectType objectType, FactTypeMappingDictionary factTypeMappings)
		{
			// If objectType is independent...
			if (objectType.TreatAsIndependent)
			{
				return true;
			}

			foreach (Role role in ObjectTypePlaysRole.GetPlayedRoleCollection(objectType))
			{
				FactType factType = role.BinarizedOrSameFactType;
				if (ShouldIgnoreFactType(factType))
				{
					continue;
				}

				// If it is a subtype fact, we need a concept type. Although the algorithm only calls for this in the case
				// of subtype meta roles, supertype meta roles will always match the patterns below, so we can immediately
				// return true for them as well.
				if (factType is SubtypeFact)
				{
					return true;
				}

				FactTypeMapping factTypeMapping = factTypeMappings[factType];

				if (factTypeMapping.MappingDepth == MappingDepth.Deep)
				{
					// Catch deep mapping that are not subtypes. The pattern is very similar, and we
					// need a concept type on both ends just like with subtype facts.
					return true;
				}

				// If fact type mapping is toward objectType...
				Role testPreferredRole;
				if (factTypeMapping.TowardsObjectType == objectType && null != (testPreferredRole = factTypeMapping.FromRole ?? factTypeMapping.TowardsRole)) // No FromRole indicates a unary fact type
				{
					bool isPartOfPreferredIdentifier = false;
					foreach (ConstraintRoleSequence constraintRoleSequence in testPreferredRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uniquenessConstraint = constraintRoleSequence as UniquenessConstraint;
						if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred)
						{
							// If the uniqueness constraint is over an auto increment role then
							// we always want this to have its own concept type. Absorbing an
							// auto counter attribute into another table does not represent the
							// model, so we choose to always create a concept type in this case.
							ObjectType rolePlayer;
							ValueTypeHasDataType dataTypeUse;
							if (null != (rolePlayer = testPreferredRole.RolePlayer) &&
								null != (dataTypeUse = ValueTypeHasDataType.GetLinkToDataType(rolePlayer)) &&
								dataTypeUse.AutoGenerated &&
								dataTypeUse.DataType.AutoGenerationIncremental)
							{
								return true;
							}
							isPartOfPreferredIdentifier = true;
							break;
						}
					}

					if (!isPartOfPreferredIdentifier)
					{
						// This FactType is not part of the preferred identifier.
						return true;
					}
				}
			}

			return false;
		}
		#endregion // Helper Methods
		#endregion // ORM to OIAL Algorithm Methods
	}
}
