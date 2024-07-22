#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.PlatformUI;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region DynamicRule class
	partial class DynamicRule: IModelErrorOwner, IModelErrorDisplayContext, IRolePathOwner
	{
		#region IRolePathOwner Implementation
		/// <summary>
		/// Resolve the model through the IRolePathOwner interface
		/// </summary>
		ORMModel IRolePathOwner.Model
		{
			get
			{
				return ResolvedModel;
			}
		}
		IList<LeadRolePath> IRolePathOwner.OwnedLeadRolePaths
		{
			get
			{
				LeadRolePath rolePath = LeadRolePath;
				return rolePath != null ? new[] { rolePath } : Array.Empty<LeadRolePath>();
			}
		}
		IList<LeadRolePath> IRolePathOwner.SharedLeadRolePaths
		{
			get
			{
				return Array.Empty<LeadRolePath>();
			}
		}
		IList<LeadRolePath> IRolePathOwner.LeadRolePaths
		{
			get
			{
				return ((IRolePathOwner)this).OwnedLeadRolePaths;
			}
		}
		bool IRolePathOwner.IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return calculation.BoundInputCollection.Count != 0 || calculation.RequiredForLeadRolePath != null;
		}
		void IRolePathOwner.ValidateRolePathOwnerSpecifics(INotifyElementAdded notifyAdded)
		{
			ValidateDynamicState(this, notifyAdded);
		}
		#endregion // IRolePathOwner Implementation
		#region IModelErrorOwner Implementation
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (0 != (filter & ModelErrorUses.BlockVerbalization))
			{
				// Allow verbalization to give it a try
				yield break;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			List<ModelErrorUsage> errors = null;
			ModelError error;
			LeadRolePath rolePath = this.LeadRolePath;
			if (rolePath != null)
			{

				if (null != (error = rolePath.RootObjectTypeRequiredError))
				{
					yield return error;
				}
				bool pathHasNodes = false;
				RolePathNode.VisitPathNodes(
					rolePath,
					RolePathNode.Empty,
					false,
					delegate (RolePathNode currentPathNode, RolePathNode previousPathNode, bool unwinding)
					{
						pathHasNodes = true;
						PathedRole pathedRole = currentPathNode;
						if (null != pathedRole)
						{
							if (previousPathNode.IsEmpty && // Root object types are only required at the root
								null != (error = pathedRole.RolePath.RootObjectTypeRequiredError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.SameFactTypeRoleWithoutJoinError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.JoinCompatibilityError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.MandatoryOuterJoinError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							ValueConstraint valueConstraint = pathedRole.ValueConstraint;
							if (valueConstraint != null)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}

							// Dynamic rule errors
							if (null != (error = pathedRole.MismatchedJoinDynamicStateError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.MismatchedRolePlayerDynamicStateError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.DynamicAddFactTypeUnboundRolesError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.DynamicObjectAddPartialIdentifierError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.DynamicObjectificationAddRequiresLinkFactTypesError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.DynamicActionRoleUndeclaredObjectError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
						}
						else
						{
							RolePathObjectTypeRoot pathRoot = currentPathNode;
							ValueConstraint valueConstraint = pathRoot.ValueConstraint;
							if (valueConstraint != null)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}
							if (null != (error = pathRoot.DynamicObjectAddPartialIdentifierError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							// UNDONE: IntraPathRoot We'll need additional errors for path roots, including compatibility errors for set comparators
						}
						return true; // Continue iteration
					});
				if (errors != null && errors.Count != 0)
				{
					foreach (ModelErrorUsage errorUsage in errors)
					{
						yield return errorUsage;
					}
					errors.Clear();
				}
				else if (!pathHasNodes &&
					null != (error = rolePath.RootObjectTypeRequiredError))
				{
					yield return error;
				}
				foreach (PathObjectUnifier objectUnifier in rolePath.ObjectUnifierCollection)
				{
					if (null != (error = objectUnifier.CompatibilityError))
					{
						yield return error;
					}
					if (null != (error = objectUnifier.MismatchedDynamicStateError))
					{
						yield return error;
					}
					if (null != (error = objectUnifier.DynamicObjectAddPartialIdentifierError))
					{
						yield return error;
					}
				}
				foreach (CalculatedPathValue calculation in rolePath.CalculatedValueCollection)
				{
					if (null != (error = calculation.FunctionRequiredError))
					{
						yield return error;
					}
					foreach (CalculatedPathValueParameterBindingError bindingError in calculation.ParameterBindingErrorCollection)
					{
						yield return bindingError;
					}
					if (null != (error = calculation.AggregationContextRequiredError))
					{
						yield return error;
					}
					if (null != (error = calculation.ConsumptionRequiredError))
					{
						yield return error;
					}
				}
			}

			// GeneralRule errors (naming)
			foreach (ModelErrorUsage errorUsage in base.GetErrorCollection(startFilter))
			{
				yield return errorUsage;
			}

			// DynamicRule errors
			if (null != (error = this.RequiresEventAndActionError))
			{
				yield return error;
			}
			if (null !=(error = this.NoDisjunctiveOrNegatedActionError))
			{
				yield return error;
			}
			if (null != (error = this.DisjunctionRequiresPositiveEventError))
			{
				yield return error;
			}
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			RolePathOwner.ValidateRolePaths(this, true, notifyAdded);
			ValidateDynamicState(this, notifyAdded);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			FrameworkDomainModel.DelayValidateElement(this, RolePathOwner.DelayValidateLeadRolePathsWithCalculations);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateDynamicState);
		}
		#endregion // IModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				ORMModel model = ResolvedModel;
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextDynamicRule, Name, model != null ? model.Name : "");
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region Validation Rules
		/// <summary>
		/// AddRule: typeof(DynamicRule)
		/// </summary>
		private static void DynamicRuleAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidationDynamicRule);
		}
		/// <summary>
		/// AddRule: typeof(DynamicRuleOwnsLeadRolePath)
		/// </summary>
		private static void DynamicRuleRolePathAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((DynamicRuleOwnsLeadRolePath)e.ModelElement).DynamicRule, DelayValidationDynamicRule);
		}
		/// <summary>
		/// DeleteRule: typeof(DynamicRuleOwnsLeadRolePath)
		/// </summary>
		private static void DynamicRuleRolePathDeletedRule(ElementDeletedEventArgs e)
		{
			DynamicRule rule = ((DynamicRuleOwnsLeadRolePath)e.ModelElement).DynamicRule;
			if (!rule.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(rule, DelayValidationDynamicRule);
			}

		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(DynamicRuleOwnsLeadRolePath)
		/// </summary>
		private static void DynamicRuleRolePathRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == DynamicRuleOwnsLeadRolePath.RolePathDomainRoleId)
			{
				// The role player change is not propagating deletion. Delete with a delay so that any other
				// link is not deleted before it can listen to this rule.
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayRemoveElement);
				FrameworkDomainModel.DelayValidateElement(((DynamicRuleOwnsLeadRolePath)e.ElementLink).DynamicRule, DelayValidationDynamicRule);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleDynamicStateChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == PathedRole.DynamicRuleStateDomainPropertyId ||
				propertyId == PathedRole.IsNegatedDomainPropertyId)
			{
				PathedRole pathedRole = (PathedRole)e.ModelElement;
				DynamicRule owner = pathedRole.RolePath.RootOwner as DynamicRule;
				if (owner != null)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDynamicState);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(RolePathObjectTypeRoot)
		/// </summary>
		private static void PathRootDynamicStateChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == RolePathObjectTypeRoot.DynamicRuleStateDomainPropertyId ||
				propertyId == RolePathObjectTypeRoot.IsNegatedDomainPropertyId)
			{
				RolePathObjectTypeRoot pathRoot = (RolePathObjectTypeRoot)e.ModelElement;
				DynamicRule owner = pathRoot.RolePath.RootOwner as DynamicRule;
				if (owner != null)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDynamicState);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(RolePath)
		/// </summary>
		private static void RolePathChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == RolePath.SplitCombinationOperatorDomainPropertyId ||
				propertyId == RolePath.SplitIsNegatedDomainPropertyId)
			{
				RolePath rolePath = (RolePath)e.ModelElement;
				DynamicRule owner = rolePath.RootOwner as DynamicRule;
				if (owner != null)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDynamicState);
				}
			}
		}
		#endregion // Validation Rules
		#region Delayed Validation
		[DelayValidatePriority(-1)] // Clean early so we don't do other validation.
		private static void DelayRemoveElement(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				element.Delete();
			}
		}
		[DelayValidateReplaces("DelayValidateDynamicState")]
		private static void DelayValidationDynamicRule(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			DynamicRule dynamicRule = (DynamicRule)element;
			RolePathOwner.ValidateRolePaths(dynamicRule, false, null);
			ValidateDynamicState(dynamicRule, null);
		}
		[DelayValidatePriority(1)] // Run after RolePathOwner.DelayValidateLeadRolePathsWithCalculations
		private static void DelayValidateDynamicState(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ValidateDynamicState((DynamicRule)element, null);
			}
		}
		/// <summary>
		/// Determine if an <see cref="Objectification"/> has an internal or auto-generated identification
		/// scheme. If this is the case, then an objectification can be added directly by placing the
		/// FactType in the Add state. Otherwise, the objectification object must be added and associated
		/// with the objectified role players via the link fact types.
		/// </summary>
		/// <param name="objectification">The objectification to test.</param>
		/// <returns><see langword="true"/> if the populated objectification fact type itself is sufficient
		/// to identify the objectifying object type.</returns>
		private static bool ObjectificationHasInternalOrAutogeneratedId(Objectification objectification)
		{
			FactType factType = objectification.NestedFactType;
			ObjectType objectType = objectification.NestingType;
			UniquenessConstraint pid;
			if (null != (pid = objectType.PreferredIdentifier))
			{
				if (pid.IsObjectifiedPreferredIdentifier)
				{
					return true;
				}
			}
			else
			{
				pid = objectType.ResolvedPreferredIdentifier;
			}
			return pid != null && PreferredIdentifierIsAutoGenerated(pid);
		}
		/// <summary>
		/// Determine if an object type has an auto-generated identifier
		/// </summary>
		private static bool ObjectTypeHasAutogeneratedId(ObjectType objectType)
		{
			UniquenessConstraint pid = objectType.ResolvedPreferredIdentifier;
			return pid != null && PreferredIdentifierIsAutoGenerated(pid);
		}
		private static bool PreferredIdentifierIsAutoGenerated(UniquenessConstraint pid)
		{
			LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
			ObjectType rolePlayer;
			ValueTypeHasDataType dataTypeUse;
			if (pidRoles.Count == 1 &&
				null != (rolePlayer = pidRoles[0].RolePlayer) &&
				null != (dataTypeUse = rolePlayer.GetDataTypeLink()) &&
				dataTypeUse.AutoGenerated)
			{
				return true;
			}
			return false;
		}
		private static void ValidateDynamicState(DynamicRule dynamicRule, INotifyElementAdded notifyAdded)
		{
			Partition partition = dynamicRule.Partition;
			ORMModel model = null;

			LeadRolePath leadRolePath = dynamicRule.LeadRolePath;
			bool haveEvent = false;
			bool haveAction = false;
			bool actionUnderDisjunctionOrNegation = false;
			bool disjunctionWithNoPositiveEvent = false;
			if (leadRolePath != null)
			{
				int disjunctionDepth = 0;
				int negationDepth = 0;
				int eventCount = 0;

				// During forward iteration, push a non-negated disjunctive branch on the stack, or null if an event is already in context.
				// When an event is encountered then all nodes in the list will be set to null. Any node left during unwind means no positive
				// event was seen.
				List<object> disjunctiveBranches = null;

				Stack<RolePathNode> contextTypes = new Stack<RolePathNode>();
				Stack<PathedRole> contextFactTypes = new Stack<PathedRole>();
				Stack<int> addFactTypeRemainingRoles = null;
				Dictionary<ModelElement, List<Tuple<Role, bool>>> identifyingRoles = null;
				Dictionary<ModelElement, DynamicObjectAddPartialIdentifierError> existingPartialIdErrors = null;
				Dictionary<PathedRole, ModelElement> verifyActionRoleDeclarations = null; // Key a canonical variable use by an Add/Delete pathed role
				Dictionary<ModelElement, object> bodyDeclarations = null; // Flag a canonical variable use as being declared in the rule body
				Action<RolePathNode> pushRequiredIdRoles = (RolePathNode node) =>
				{
					// The need here is to determine which roles must be played for an added object type to
					// be fully identified. We also require that an objectifying entity type have a fully
					// populated objectified fact type via the link fact types (there is no way to jump
					// from/to an objectifying entity type and the objectified fact type, so the link fact types
					// must be used to populate if the entity is added).

					// If the identifier is auto-generated then the identifying role is not required. However, we
					// do not disallow population of an auto-generated identifier as a dynamic rule could theoretically
					// be used to generate the identifier, or the generator could be treated with 'generate if not null'
					// semantics. Therefore, we simply ignore the preferred identifier if it is auto-generated.

					// The lookup key for this is the canonical entry. This is called with a non-empty node during the
					// forward walk of the path, so the topmost node for this object is known to be provided. However,
					// this node can also be unified, so the object may already be known through another path.
					PathedRole pathedRole = node;

					ModelElement canonicalKey = null;
					ObjectType objectType = null;
					if (pathedRole != null)
					{
						canonicalKey = (ModelElement)pathedRole.ObjectUnifier ?? pathedRole;
						objectType = pathedRole.Role.RolePlayer;
					}
					else
					{
						RolePathObjectTypeRoot pathRoot = node;
						objectType = pathRoot.RootObjectType;
						canonicalKey = (ModelElement)pathRoot.ObjectUnifier ?? pathRoot;
					}

					List<Tuple<Role, bool>> roleList = null;
					if (identifyingRoles == null)
					{
						identifyingRoles = new Dictionary<ModelElement, List<Tuple<Role, bool>>>();
					}
					else if (identifyingRoles.TryGetValue(canonicalKey, out roleList))
					{
						if (roleList != null && pathedRole != null) // The list is null if there are no required roles
						{
							Role role = pathedRole.Role;
							for (int count = roleList.Count, i = 0; i < count; ++i)
							{
								Tuple<Role, bool> roleState = roleList[i];
								if (roleState.Item1 == role)
								{
									if (!roleState.Item2)
									{
										roleList[i] = new Tuple<Role, bool>(role, true);
									}
									break;
								}
							}
						}
						return;
					}

					if (objectType != null)
					{
						Objectification objectification = objectType.Objectification;
						UniquenessConstraint pid = objectType.ResolvedPreferredIdentifier;
						FactType objectifiedFactType = null;
						Role currentRole = pathedRole != null ? pathedRole.Role : null;

						if (objectification != null)
						{
							// Link fact types for objectified roles are always required.
							objectifiedFactType = objectification.NestedFactType;
							LinkedElementCollection<RoleBase> roles = objectifiedFactType.RoleCollection;
							roleList = new List<Tuple<Role, bool>>();
							for (int count = roles.Count, i = 0; i < count; ++i)
							{
								Role linkRole = (Role)roles[i].OppositeRoleAlwaysResolveProxy;
								roleList.Add(new Tuple<Role, bool>(linkRole, currentRole == linkRole));
							}
						}
						if (pid != null && !PreferredIdentifierIsAutoGenerated(pid))
						{
							LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
							for (int count = pidRoles.Count, i = 0; i < count; ++i)
							{
								Role idRole = pidRoles[0];
								if ((objectifiedFactType == null || idRole.FactType != objectifiedFactType) &&
									null != (idRole = idRole.OppositeOrUnaryRole as Role))
								{
									(roleList ?? (roleList = new List<Tuple<Role, bool>>())).Add(new Tuple<Role, bool>(idRole, currentRole == idRole));
								}
							}
						}
					}

					identifyingRoles[canonicalKey] = roleList; // Note that roleList may still be null
				};

				Action<RolePathNode, PathedRole> flagIdRoleUsed = (RolePathNode typeNode, PathedRole pathedRole) =>
				{
					if (identifyingRoles != null)
					{
						PathedRole typePathedRole = typeNode;
						RolePathObjectTypeRoot typePathRoot = null;
						ModelElement canonicalKey = null;
						ObjectType objectType = null;
						if (pathedRole != null)
						{
							canonicalKey = (ModelElement)pathedRole.ObjectUnifier ?? pathedRole;
							objectType = pathedRole.Role.RolePlayer;
						}
						else
						{
							typePathRoot = typeNode;
							canonicalKey = (ModelElement)typePathRoot.ObjectUnifier ?? typePathRoot;
						}

						List<Tuple<Role, bool>> rolesList;
						if (identifyingRoles.TryGetValue(canonicalKey, out rolesList) && rolesList != null)
						{
							Role testRole = pathedRole.Role;
							for (int count = rolesList.Count, i = 0; i < count; ++i)
							{
								Tuple<Role, bool> roleBinding = rolesList[i];
								if (testRole == roleBinding.Item1)
								{
									if (!roleBinding.Item2)
									{
										rolesList[i] = new Tuple<Role, bool>(testRole, true);
									}
									break;
								}
							}
						}
					}
				};

				Action<RolePathNode> setContextType = (RolePathNode node) =>
				{
					if (node.IsEmpty)
					{
						contextTypes.Pop();
					}
					else
					{
						contextTypes.Push(node);
						DynamicRuleNodeState rolePlayerState = node.DynamicRuleState;
						if (rolePlayerState == DynamicRuleNodeState.Add)
						{
							pushRequiredIdRoles(node);
						}

						PathedRole pathedRole = node;
						if (pathedRole != null)
						{
							MismatchedRolePlayerDynamicStateError rolePlayerMismatchError = pathedRole.MismatchedRolePlayerDynamicStateError;
							DynamicObjectAddPartialIdentifierError partialIdError = pathedRole.DynamicObjectAddPartialIdentifierError;
							if (partialIdError != null)
							{
								(existingPartialIdErrors ?? (existingPartialIdErrors = new Dictionary<ModelElement, DynamicObjectAddPartialIdentifierError>()))[pathedRole] = partialIdError;
							}

							if (contextFactTypes.Count != 0) // Defensive, will only happen with an invalid role path
							{
								DynamicRuleNodeState factTypeState = contextFactTypes.Peek().DynamicRuleState;
								if (factTypeState == DynamicRuleNodeState.Add)
								{
									addFactTypeRemainingRoles.Push(addFactTypeRemainingRoles.Pop() - 1);
								}
								bool mismatch = false;
								bool bodyDeclaration = true;
								bool sameActionState = false;
								switch (factTypeState)
								{
									case DynamicRuleNodeState.Current:
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Current:
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Delete:
												break;
											default:
												mismatch = true;
												break;
										}
										break;
									case DynamicRuleNodeState.Initial:
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Delete:
												break;
											default:
												mismatch = true;
												break;
										}
										break;
									case DynamicRuleNodeState.Added:
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Current:
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Added:
												break;
											default:
												mismatch = true;
												break;
										}
										break;
									case DynamicRuleNodeState.Deleted:
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Current:
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Deleted:
											case DynamicRuleNodeState.Delete:
												break;
											default:
												mismatch = true;
												break;
										}
										break;
									case DynamicRuleNodeState.Add:
										bodyDeclaration = false;
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Current:
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Added:
												break;
											case DynamicRuleNodeState.Add:
												sameActionState = true;
												break;
											default:
												mismatch = true;
												break;
										}
										break;
									case DynamicRuleNodeState.Delete:
										bodyDeclaration = false;
										switch (rolePlayerState)
										{
											case DynamicRuleNodeState.Current:
											case DynamicRuleNodeState.Initial:
											case DynamicRuleNodeState.Deleted:
												break;
											case DynamicRuleNodeState.Delete:
												sameActionState = true;
												break;
											default:
												mismatch = true;
												break;
										}
										break;
								}

								if (!mismatch)
								{
									if (rolePlayerMismatchError != null)
									{
										rolePlayerMismatchError.Delete();
									}

									if (bodyDeclaration)
									{
										pathedRole.DynamicActionRoleUndeclaredObjectError = null;
										if (negationDepth == 0 && disjunctionDepth == 0)
										{
											(bodyDeclarations ?? (bodyDeclarations = new Dictionary<ModelElement, object>()))[(ModelElement)pathedRole.ObjectUnifier ?? pathedRole] = null;
										}
									}
									else if (sameActionState)
									{
										pathedRole.DynamicActionRoleUndeclaredObjectError = null;
									}
									else
									{
										(verifyActionRoleDeclarations ?? (verifyActionRoleDeclarations = new Dictionary<PathedRole, ModelElement>()))[pathedRole] = (ModelElement)pathedRole.ObjectUnifier ?? pathedRole;
									}
								}
								else
								{
									pathedRole.DynamicActionRoleUndeclaredObjectError = null;
									if (rolePlayerMismatchError == null)
									{
										rolePlayerMismatchError = new MismatchedRolePlayerDynamicStateError(partition);
										rolePlayerMismatchError.PathedRole = pathedRole;
										rolePlayerMismatchError.Model = model ?? (model = dynamicRule.ResolvedModel);
										rolePlayerMismatchError.GenerateErrorText(factTypeState, rolePlayerState);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(rolePlayerMismatchError, true);
										}
									}
									else
									{
										// Update in case items have changed.
										rolePlayerMismatchError.GenerateErrorText(factTypeState, rolePlayerState);
									}
								}
							}
							else
							{
								pathedRole.DynamicActionRoleUndeclaredObjectError = null;
								if (rolePlayerMismatchError != null)
								{
									rolePlayerMismatchError.Delete();
								}
							}
						}
						else
						{
							RolePathObjectTypeRoot pathRoot = node;
							switch (pathRoot.DynamicRuleState)
							{
								case DynamicRuleNodeState.Current:
								case DynamicRuleNodeState.Initial:
								case DynamicRuleNodeState.Added:
								case DynamicRuleNodeState.Deleted:
									if (negationDepth == 0 && disjunctionDepth == 0)
									{
										(bodyDeclarations ?? (bodyDeclarations = new Dictionary<ModelElement, object>()))[(ModelElement)pathRoot.ObjectUnifier ?? pathRoot] = null;
									}
									break;
							}
							DynamicObjectAddPartialIdentifierError partialIdError = pathRoot.DynamicObjectAddPartialIdentifierError;
							if (partialIdError != null)
							{
								(existingPartialIdErrors ?? (existingPartialIdErrors = new Dictionary<ModelElement, DynamicObjectAddPartialIdentifierError>()))[pathRoot] = partialIdError;
							}

						}
					}
				};
				Action<PathedRole> enterFactType = (PathedRole entry) =>
				{
					if (entry == null)
					{
						PathedRole entryRole = contextFactTypes.Pop();
						DynamicAddFactTypeUnboundRolesError unboundRolesError = entryRole.DynamicAddFactTypeUnboundRolesError;
						if (entryRole.DynamicRuleState == DynamicRuleNodeState.Add &&
							addFactTypeRemainingRoles.Pop() > 0)
						{
							unboundRolesError = new DynamicAddFactTypeUnboundRolesError(partition);
							unboundRolesError.PathedRole = entryRole;
							unboundRolesError.Model = model ?? (model = dynamicRule.ResolvedModel);
							unboundRolesError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(unboundRolesError, true);
							}
						}
						else if (unboundRolesError != null)
						{
							unboundRolesError.Delete();
						}
					}
					else
					{
						// Track this entry
						contextFactTypes.Push(entry);
						DynamicObjectificationAddRequiresLinkFactTypesError unidentifiableObjectificationError = entry.DynamicObjectificationAddRequiresLinkFactTypesError;
						DynamicObjectAddPartialIdentifierError partialIdError = entry.DynamicObjectAddPartialIdentifierError;
						if (partialIdError != null)
						{
							(existingPartialIdErrors ?? (existingPartialIdErrors = new Dictionary<ModelElement, DynamicObjectAddPartialIdentifierError>()))[entry] = partialIdError;
						}

						// See how many additional roles we need for an Add, where all roles must be bound.
						if (entry.DynamicRuleState == DynamicRuleNodeState.Add)
						{
							int requiredRoleCount = 0;
							FactType factType;
							Role role = entry.Role;
							bool unidentifiableObjectification = false;
							if (null != (factType = role.FactType))
							{
								LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
								requiredRoleCount = roles.Count;

								--requiredRoleCount; // We already have one role
								Objectification objectification = factType.Objectification;
								if (objectification != null)
								{
									bool inLinkFactType = false;
									RoleProxy proxyRole;
									if (requiredRoleCount != 0 && null != (proxyRole = role.Proxy))
									{
										// Peek ahead to see if we're on the link fact type or the normal fact type
										IList<PathedRole> pathedRoles = entry.RolePath.PathedRoleCollection;
										int followingIndex = pathedRoles.IndexOf(entry) + 1;
										if (followingIndex < pathedRoles.Count)
										{
											PathedRole testRole = pathedRoles[followingIndex];
											if (testRole.PathedRolePurpose == PathedRolePurpose.SameFactType &&
												testRole.Role.FactType == proxyRole.FactType)
											{
												inLinkFactType = true;
												requiredRoleCount = 1; // We're on the link fact type, which will always have two roles
											}
										}
									}

									if (!inLinkFactType)
									{
										unidentifiableObjectification = !ObjectificationHasInternalOrAutogeneratedId(objectification);
									}
								}
								(addFactTypeRemainingRoles ?? (addFactTypeRemainingRoles = new Stack<int>())).Push(requiredRoleCount);
							}

							if (unidentifiableObjectification)
							{
								if (unidentifiableObjectificationError == null)
								{
									unidentifiableObjectificationError = new DynamicObjectificationAddRequiresLinkFactTypesError(partition);
									unidentifiableObjectificationError.PathedRole = entry;
									unidentifiableObjectificationError.Model = model ?? (model = dynamicRule.ResolvedModel);
									unidentifiableObjectificationError.GenerateErrorText();
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(unidentifiableObjectificationError, true);
									}
								}
							}
							else if (unidentifiableObjectificationError != null)
							{
								unidentifiableObjectificationError.Delete();
							}
						}
						else if (unidentifiableObjectificationError != null)
						{
							unidentifiableObjectificationError.Delete();
						}

						MismatchedJoinDynamicStateError joinMismatchError = entry.MismatchedJoinDynamicStateError;
						if (contextTypes.Count != 0)
						{
							RolePathNode contextTypeNode = contextTypes.Peek();
							DynamicRuleNodeState contextState = contextTypeNode.DynamicRuleState;
							DynamicRuleNodeState entryState = entry.DynamicRuleState;
							bool mismatch = false;
							bool bodyDeclaration = false;
							bool sameActionState = false;
							switch (contextState)
							{
								case DynamicRuleNodeState.Current:
									switch (entryState)
									{
										case DynamicRuleNodeState.Current:
										case DynamicRuleNodeState.Added:
										case DynamicRuleNodeState.Deleted:
											bodyDeclaration = true;
											break;
										case DynamicRuleNodeState.Add:
										case DynamicRuleNodeState.Delete:
											break;
										default:
											mismatch = true;
											break;
									}
									break;
								case DynamicRuleNodeState.Initial:
									switch (entryState)
									{
										case DynamicRuleNodeState.Current:
										case DynamicRuleNodeState.Added:
										case DynamicRuleNodeState.Deleted:
										case DynamicRuleNodeState.Initial:
											bodyDeclaration = true;
											break;
										// Initial role players can be used with any state.
									}
									break;
								case DynamicRuleNodeState.Added:
									switch (entryState)
									{
										case DynamicRuleNodeState.Added:
											bodyDeclaration = true;
											break;
										case DynamicRuleNodeState.Add:
											break;
										default:
											mismatch = true;
											break;
									}
									break;
								case DynamicRuleNodeState.Deleted:
									switch (entryState)
									{
										case DynamicRuleNodeState.Current:
										case DynamicRuleNodeState.Initial:
										case DynamicRuleNodeState.Deleted:
											bodyDeclaration = true;
											break;
										case DynamicRuleNodeState.Delete:
											break;
										default:
											mismatch = true;
											break;
									}
									break;
								case DynamicRuleNodeState.Add:
									flagIdRoleUsed(contextTypeNode, entry);
									switch (entryState)
									{
										case DynamicRuleNodeState.Add:
											sameActionState = true;
											break;
										default:
											mismatch = true;
											break;
									}
									break;
								case DynamicRuleNodeState.Delete:
									switch (entryState)
									{
										case DynamicRuleNodeState.Current:
										case DynamicRuleNodeState.Initial:
										case DynamicRuleNodeState.Added:
										case DynamicRuleNodeState.Deleted:
											bodyDeclaration = true;
											break;
										case DynamicRuleNodeState.Delete:
											sameActionState = true;
											break;
										default:
											mismatch = true;
											break;
									}
									break;
							}

							if (!mismatch)
							{
								if (joinMismatchError != null)
								{
									joinMismatchError.Delete();
								}

								if (sameActionState)
								{
									entry.DynamicActionRoleUndeclaredObjectError = null;
								}
								else
								{
									PathedRole typeRole = contextTypeNode;
									RolePathObjectTypeRoot typeRoot;
									ModelElement canonicalTypeKey = typeRole != null ? ((ModelElement)typeRole.ObjectUnifier ?? typeRole) : ((ModelElement)(typeRoot = contextTypeNode).ObjectUnifier ?? typeRoot);
									if (bodyDeclaration)
									{
										entry.DynamicActionRoleUndeclaredObjectError = null;
										if (negationDepth == 0 && disjunctionDepth == 0)
										{
											(bodyDeclarations ?? (bodyDeclarations = new Dictionary<ModelElement, object>()))[canonicalTypeKey] = null;
										}
									}
									else
									{
										(verifyActionRoleDeclarations ?? (verifyActionRoleDeclarations = new Dictionary<PathedRole, ModelElement>()))[entry] = canonicalTypeKey;
									}
								}
							}
							else
							{
								entry.DynamicActionRoleUndeclaredObjectError = null;
								if (joinMismatchError == null)
								{
									joinMismatchError = new MismatchedJoinDynamicStateError(partition);
									joinMismatchError.PathedRole = entry;
									joinMismatchError.Model = model ?? (model = dynamicRule.ResolvedModel);
									joinMismatchError.GenerateErrorText(contextState, entryState);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(joinMismatchError, true);
									}
								}
								else
								{
									// Update in case items have changed.
									joinMismatchError.GenerateErrorText(contextState, entryState);
								}
							}
						}
						else
						{
							entry.DynamicActionRoleUndeclaredObjectError = null;
							if (joinMismatchError != null)
							{
								joinMismatchError.Delete();
							}
						}
					}
				};
				RolePathNode.VisitPathNodes(
					leadRolePath,
					RolePathNode.Empty,
					true,
					delegate (RolePathNode pathNode, RolePathNode previousPathNode, bool unwinding)
					{
						PathedRole currentPathedRole = pathNode;
						RolePathObjectTypeRoot currentPathRoot = currentPathedRole == null ? (RolePathObjectTypeRoot)pathNode : null;
						RolePath previousRolePath = previousPathNode.RolePath;
						DynamicRuleNodeState state = pathNode.DynamicRuleState;
						if (unwinding)
						{
							// Replicate the logic from the forward path to unwind negation and disjunction tracking
							if (currentPathedRole != null)
							{
								switch (currentPathedRole.PathedRolePurpose)
								{
									case PathedRolePurpose.PostInnerJoin:
										enterFactType(null);
										if (currentPathedRole.IsNegated)
										{
											--negationDepth;
										}
										break;
									case PathedRolePurpose.PostOuterJoin:
										enterFactType(null);
										--disjunctionDepth;
										if (currentPathedRole.IsNegated)
										{
											--negationDepth;
										}
										else if (!disjunctionWithNoPositiveEvent)
										{
											int lastIndex = disjunctiveBranches.Count - 1;
											if (disjunctiveBranches[lastIndex] != null)
											{
												disjunctionWithNoPositiveEvent = true;
												disjunctiveBranches = null;
											}
											else
											{
												disjunctiveBranches.RemoveAt(lastIndex);
											}
										}
										break;
									case PathedRolePurpose.SameFactType:
										setContextType(RolePathNode.Empty);
										break;
								}
							}
							else
							{
								setContextType(RolePathNode.Empty);
								if (currentPathRoot.IsNegated)
								{
									--negationDepth;
								}
							}

							if (negationDepth != 0 || disjunctionDepth != 0)
							{
								if (previousRolePath != null && pathNode.RolePath != previousRolePath)
								{
									if (previousRolePath.SplitCombinationOperator != LogicalCombinationOperator.And)
									{
										--disjunctionDepth;
										if (previousRolePath.SplitIsNegated)
										{
											--negationDepth;
										}
										else if (!disjunctionWithNoPositiveEvent)
										{
											int lastIndex = disjunctiveBranches.Count - 1;
											if (disjunctiveBranches[lastIndex] != null)
											{
												disjunctionWithNoPositiveEvent = true;
												disjunctiveBranches = null;
											}
											else
											{
												disjunctiveBranches.RemoveAt(lastIndex);
											}
										}
									}
									else if (previousRolePath.SplitIsNegated)
									{
										--negationDepth;
									}
								}
							}

							switch (state)
							{
								case DynamicRuleNodeState.Added:
								case DynamicRuleNodeState.Deleted:
									--eventCount;
									break;
							}
						}
						else
						{
							// Track if we're inside negation or disjunction. It is possible than we
							// can have multiple hits on the same node (one from the node, one from a path switch).
							// It isn't worth avoiding this as we're just looking for >0 hits.
							if (previousRolePath != null && pathNode.RolePath != previousRolePath)
							{
								if (previousRolePath.SplitCombinationOperator != LogicalCombinationOperator.And)
								{
									++disjunctionDepth;
									if (previousRolePath.SplitIsNegated)
									{
										++negationDepth;
									}
									else if (!disjunctionWithNoPositiveEvent)
									{
										(disjunctiveBranches ?? (disjunctiveBranches = new List<object>())).Add(eventCount == 0 ? previousRolePath : null); // The instance is a placeholder, any non-null object will do.
									}
								}
								else if (previousRolePath.SplitIsNegated)
								{
									++negationDepth;
								}
							}

							if (currentPathedRole != null)
							{
								switch (currentPathedRole.PathedRolePurpose)
								{
									case PathedRolePurpose.PostInnerJoin:
										if (currentPathedRole.IsNegated)
										{
											++negationDepth;
										}
										enterFactType(currentPathedRole);
										break;
									case PathedRolePurpose.PostOuterJoin:
										++disjunctionDepth;
										if (currentPathedRole.IsNegated)
										{
											++negationDepth;
										}
										else if (!disjunctionWithNoPositiveEvent)
										{
											(disjunctiveBranches ?? (disjunctiveBranches = new List<object>())).Add(eventCount == 0 ? currentPathedRole : null);
										}
										enterFactType(currentPathedRole);
										break;
									case PathedRolePurpose.SameFactType:
										setContextType(currentPathedRole);
										break;
								}
							}
							else
							{
								if (currentPathRoot.IsNegated)
								{
									++negationDepth;
								}
								setContextType(currentPathRoot);
							}

							switch (state)
							{
								case DynamicRuleNodeState.Added:
								case DynamicRuleNodeState.Deleted:
									++eventCount;
									haveEvent = true;
									if (negationDepth == 0)
									{
										if (disjunctiveBranches != null)
										{
											for (int i = 0, count = disjunctiveBranches.Count; i < count; ++i)
											{
												disjunctiveBranches[i] = null;
											}
										}
									}
									break;
								case DynamicRuleNodeState.Add:
								case DynamicRuleNodeState.Delete:
									if (disjunctionDepth != 0 || negationDepth != 0)
									{
										actionUnderDisjunctionOrNegation = true;
									}
									haveAction = true;
									break;
							}
						}
						return true; // Finish the path
					});

				// Check object unifiers. This should be maintained by any rule editor but still needs to be checked.
				foreach (PathObjectUnifier unifier in leadRolePath.ObjectUnifierCollection)
				{
					bool first = true;
					bool mismatch = false;
					DynamicObjectAddPartialIdentifierError partialIdError = unifier.DynamicObjectAddPartialIdentifierError;
					if (partialIdError != null)
					{
						(existingPartialIdErrors ?? (existingPartialIdErrors = new Dictionary<ModelElement, DynamicObjectAddPartialIdentifierError>()))[unifier] = partialIdError;
					}
					DynamicRuleNodeState dynamicState = DynamicRuleNodeState.Current;
					foreach (PathedRole pathedRole in unifier.PathedRoleCollection)
					{
						if (first)
						{
							dynamicState = pathedRole.DynamicRuleState;
							first = false;
						}
						else if (dynamicState != pathedRole.DynamicRuleState)
						{
							mismatch = true;
							break;
						}
					}

					if (!mismatch)
					{
						foreach (RolePathObjectTypeRoot pathRoot in unifier.PathRootCollection)
						{
							if (first)
							{
								dynamicState = pathRoot.DynamicRuleState;
								first = false;
							}
							else if (dynamicState != pathRoot.DynamicRuleState)
							{
								mismatch = true;
								break;
							}
						}
					}

					ObjectUnifierMismatchedDynamicStateError unifierDynamicStateError = unifier.MismatchedDynamicStateError;
					if (!mismatch)
					{
						if (unifierDynamicStateError != null)
						{
							unifierDynamicStateError.Delete();
						}
					}
					else if (null == unifierDynamicStateError)
					{
						unifierDynamicStateError = new ObjectUnifierMismatchedDynamicStateError(partition);
						unifierDynamicStateError.ObjectUnifier = unifier;
						unifierDynamicStateError.Model = model ?? (model = dynamicRule.ResolvedModel);
						unifierDynamicStateError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(unifierDynamicStateError, true);
						}
					}
				}

				// Resolve all of the data we've picked up and create new errors as needed
				if (identifyingRoles != null)
				{
					foreach (KeyValuePair<ModelElement, List<Tuple<Role, bool>>> kvp in identifyingRoles)
					{
						ModelElement canonicalKey = kvp.Key;
						DynamicObjectAddPartialIdentifierError partialIdError = null;
						if (existingPartialIdErrors != null && existingPartialIdErrors.TryGetValue(canonicalKey, out partialIdError))
						{
							existingPartialIdErrors.Remove(canonicalKey);
						}

						List<Tuple<Role, bool>> rolesList = kvp.Value;
						bool hasError = false;
						if (rolesList != null)
						{
							for (int count = rolesList.Count, i = 0; i < count; ++i)
							{
								if (!rolesList[i].Item2)
								{
									hasError = true;
									break;
								}
							}
						}

						if (hasError)
						{
							if (partialIdError == null)
							{
								PathedRole pathedRoleKey;
								RolePathObjectTypeRoot pathRootKey;
								PathObjectUnifier unifierKey;
								partialIdError = new DynamicObjectAddPartialIdentifierError(partition);

								if (null != (pathedRoleKey = canonicalKey as PathedRole))
								{
									partialIdError.PathedRole = pathedRoleKey;
								}
								else if (null != (pathRootKey = canonicalKey as RolePathObjectTypeRoot))
								{
									partialIdError.PathRoot = pathRootKey;
								}
								else if (null != (unifierKey = canonicalKey as PathObjectUnifier))
								{
									partialIdError.ObjectUnifier = unifierKey;
								}
								partialIdError.Model = model ?? (model = dynamicRule.ResolvedModel);
								partialIdError.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(partialIdError, true);
								}
							}
						}
						else if (partialIdError != null)
						{
							partialIdError.Delete();
						}
					}
				}

				if (existingPartialIdErrors != null && existingPartialIdErrors.Count != 0)
				{
					foreach (DynamicObjectAddPartialIdentifierError unboundRule in existingPartialIdErrors.Values)
					{
						unboundRule.Delete();
					}
				}

				if (verifyActionRoleDeclarations != null)
				{
					foreach (KeyValuePair<PathedRole, ModelElement> kvp in verifyActionRoleDeclarations)
					{
						PathedRole actionRole = kvp.Key;
						DynamicActionRoleUndeclaredObjectError declarationError = actionRole.DynamicActionRoleUndeclaredObjectError;
						if (bodyDeclarations == null || !bodyDeclarations.ContainsKey(kvp.Value))
						{
							if (declarationError == null)
							{
								declarationError = new DynamicActionRoleUndeclaredObjectError(partition);
								declarationError.PathedRole = actionRole;
								declarationError.Model = model ?? (model = dynamicRule.ResolvedModel);
								declarationError.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(declarationError, true);
								}
							}
						}
						else if (declarationError != null)
						{
							declarationError.Delete();
						}
					}
				}
			}

			DynamicRuleRequiresEventAndActionError requireEventAndActionError = dynamicRule.RequiresEventAndActionError;
			if (haveEvent && haveAction)
			{
				if (requireEventAndActionError != null)
				{
					requireEventAndActionError.Delete();
				}
			}
			else if (null == requireEventAndActionError)
			{
				requireEventAndActionError = new DynamicRuleRequiresEventAndActionError(partition);
				requireEventAndActionError.DynamicRule = dynamicRule;
				requireEventAndActionError.Model = model ?? (model = dynamicRule.ResolvedModel);
				requireEventAndActionError.GenerateErrorText();
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(requireEventAndActionError, true);
				}
			}

			DynamicRuleNoDisjunctiveOrNegatedActionError noDisjunctiveOrNegatedActionError = dynamicRule.NoDisjunctiveOrNegatedActionError;
			if (!actionUnderDisjunctionOrNegation)
			{
				if (noDisjunctiveOrNegatedActionError != null)
				{
					noDisjunctiveOrNegatedActionError.Delete();
				}
			}
			else if (null == noDisjunctiveOrNegatedActionError)
			{
				noDisjunctiveOrNegatedActionError = new DynamicRuleNoDisjunctiveOrNegatedActionError(partition);
				noDisjunctiveOrNegatedActionError.DynamicRule = dynamicRule;
				noDisjunctiveOrNegatedActionError.Model = model ?? (model = dynamicRule.ResolvedModel);
				noDisjunctiveOrNegatedActionError.GenerateErrorText();
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(noDisjunctiveOrNegatedActionError, true);
				}
			}

			DynamicRuleDisjunctionRequiresPositiveEventError disjunctionRequiresPositiveEventError = dynamicRule.DisjunctionRequiresPositiveEventError;
			if (!disjunctionWithNoPositiveEvent)
			{
				if (disjunctionRequiresPositiveEventError != null)
				{
					disjunctionRequiresPositiveEventError.Delete();
				}
			}
			else if (null == disjunctionRequiresPositiveEventError)
			{
				disjunctionRequiresPositiveEventError = new DynamicRuleDisjunctionRequiresPositiveEventError(partition);
				disjunctionRequiresPositiveEventError.DynamicRule = dynamicRule;
				disjunctionRequiresPositiveEventError.Model = model ?? (model = dynamicRule.ResolvedModel);
				disjunctionRequiresPositiveEventError.GenerateErrorText();
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(disjunctionRequiresPositiveEventError, true);
				}
			}
		}
		#endregion // Delayed Validation
	}
	#endregion // DynamicRule class
	#region DynamicRule Errors
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicRuleRequiresEventAndActionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleRequiresEventAndAction, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleRequiresEventAndActionCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicRuleNoDisjunctiveOrNegatedActionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleNoDisjunctiveOrNegatedAction, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleNoDisjunctiveOrNegatedActionCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicRuleDisjunctionRequiresPositiveEventError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleDisjunctionRequiresPositiveEvent, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleDisjunctionRequiresPositiveEventCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class MismatchedJoinDynamicStateError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			// The calculating routines always provide the states, but the
			// text may be regenerated for other reasons.
			PathedRole pathedRole = this.PathedRole;
			DynamicRuleNodeState entryState = pathedRole.DynamicRuleState;
			RolePathNode contextNode = GetContextNode(pathedRole);
			GenerateErrorText(contextNode.IsEmpty ? DynamicRuleNodeState.Current : contextNode.DynamicRuleState, entryState);
		}
		/// <summary>
		/// Determine the context node that joins to this pathed role.
		/// </summary>
		/// <param name="pathedRole">The initial pathed role, move up from here.</param>
		/// <param name="pathedRoles">Previously retrieved pathed roles, used for clean recursion.</param>
		/// <param name="pathedRoleIndex">Known index, used for clean recursion.</param>
		/// <returns></returns>
		private static RolePathNode GetContextNode(PathedRole pathedRole, IList<PathedRole> pathedRoles = null, int pathedRoleIndex = -1)
		{
			// The pathed role will always be a join type
			if (pathedRoles == null)
			{
				pathedRoles = pathedRole.RolePath.PathedRoleCollection;
				pathedRoleIndex = pathedRoles.IndexOf(pathedRole);
			}
			if (pathedRoleIndex == 0)
			{
				RolePath rolePath = pathedRole.RolePath;
				while (rolePath != null)
				{
					RolePathObjectTypeRoot root = rolePath.PathRoot;
					if (root != null)
					{
						return root;
					}

					RoleSubPath subPath;
					if (null != (subPath = rolePath as RoleSubPath) &&
						null != (rolePath = subPath.ParentRolePath))
					{
						pathedRoles = rolePath.PathedRoleCollection;
						pathedRoleIndex = pathedRoles.Count;
						if (pathedRoleIndex != 0)
						{
							--pathedRoleIndex;
							pathedRole = pathedRoles[pathedRoleIndex];
							if (pathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
							{
								return pathedRole;
							}
							else
							{
								// On another join node. This is unusual, but possible with chained unary fact types
								return GetContextNode(pathedRole, pathedRoles, pathedRoleIndex);
							}
						}
					}
					else
					{
						rolePath = null;
					}
				}
			}
			else
			{
				--pathedRoleIndex;
				pathedRole = pathedRoles[pathedRoleIndex];
				if (pathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
				{
					return pathedRole;
				}
				return GetContextNode(pathedRole, pathedRoles, pathedRoleIndex);
			}
			return RolePathNode.Empty;
		}
		/// <summary>
		/// Generate error text with known dynamic states
		/// </summary>
		/// <param name="contextState">The dynamic state of the node being joined to.</param>
		/// <param name="entryState">The dynamic static of the fact type entry.</param>
		public void GenerateErrorText(DynamicRuleNodeState contextState, DynamicRuleNodeState entryState)
		{
			IModelErrorDisplayContext displayContext = PathedRole.RolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicState, displayContext != null ? displayContext.ErrorDisplayContext : "", SingleState(contextState), SingleState(entryState), AllowedStates(contextState)));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateCompact;
			}
		}
		private static string SingleState(DynamicRuleNodeState state)
		{
			string retVal = string.Empty;
			switch (state)
			{
				case DynamicRuleNodeState.Add:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayAdd;
					break;
				case DynamicRuleNodeState.Added:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayAdded;
					break;
				case DynamicRuleNodeState.Current:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayCurrent;
					break;
				case DynamicRuleNodeState.Delete:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayDelete;
					break;
				case DynamicRuleNodeState.Deleted:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayDeleted;
					break;
				case DynamicRuleNodeState.Initial:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayInitial;
					break;
			}
			return retVal;
		}
		private static string AllowedStates(DynamicRuleNodeState state)
		{
			string retVal = string.Empty;
			switch (state)
			{
				case DynamicRuleNodeState.Add:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForAdd;
					break;
				case DynamicRuleNodeState.Added:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForAdded;
					break;
				case DynamicRuleNodeState.Current:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForCurrent;
					break;
				case DynamicRuleNodeState.Delete:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForDelete;
					break;
				case DynamicRuleNodeState.Deleted:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForDeleted;
					break;
				case DynamicRuleNodeState.Initial:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedJoinDynamicStateAllowedForInitial;
					break;
			}
			return retVal;
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class MismatchedRolePlayerDynamicStateError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			PathedRole rolePlayerPathedRole = this.PathedRole;
			// The entry role must be in the same role path (we don't support intra-fact type splits any more)
			IList<PathedRole> pathedRoles = rolePlayerPathedRole.RolePath.PathedRoleCollection;
			int index = pathedRoles.IndexOf(rolePlayerPathedRole);
			PathedRole entryPathedRole = null;
			for (int i = index - 1; i >= 0; --i)
			{
				PathedRole testPathedRole = pathedRoles[i];
				if (testPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					entryPathedRole = testPathedRole;
					break;
				}
			}
			GenerateErrorText(entryPathedRole != null ? entryPathedRole.DynamicRuleState : DynamicRuleNodeState.Current, rolePlayerPathedRole.DynamicRuleState);
		}
		/// <summary>
		/// Generate error text with known dynamic states
		/// </summary>
		/// <param name="factTypeState">The dynamic state of the fact type entry node.</param>
		/// <param name="rolePlayerState">The dynamic static of the role player.</param>
		public void GenerateErrorText(DynamicRuleNodeState factTypeState, DynamicRuleNodeState rolePlayerState)
		{
			IModelErrorDisplayContext displayContext = PathedRole.RolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicState, displayContext != null ? displayContext.ErrorDisplayContext : "", SingleState(factTypeState), SingleState(rolePlayerState), AllowedStates(factTypeState)));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateCompact;
			}
		}
		private static string SingleState(DynamicRuleNodeState state)
		{
			string retVal = string.Empty;
			switch (state)
			{
				case DynamicRuleNodeState.Add:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayAdd;
					break;
				case DynamicRuleNodeState.Added:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayAdded;
					break;
				case DynamicRuleNodeState.Current:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayCurrent;
					break;
				case DynamicRuleNodeState.Delete:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayDelete;
					break;
				case DynamicRuleNodeState.Deleted:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayDeleted;
					break;
				case DynamicRuleNodeState.Initial:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedDynamicStateDisplayInitial;
					break;
			}
			return retVal;
		}
		private static string AllowedStates(DynamicRuleNodeState state)
		{
			string retVal = string.Empty;
			switch (state)
			{
				case DynamicRuleNodeState.Add:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForAdd;
					break;
				case DynamicRuleNodeState.Added:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForAdded;
					break;
				case DynamicRuleNodeState.Current:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForCurrent;
					break;
				case DynamicRuleNodeState.Delete:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForDelete;
					break;
				case DynamicRuleNodeState.Deleted:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForDeleted;
					break;
				case DynamicRuleNodeState.Initial:
					retVal = ResourceStrings.ModelErrorDynamicRuleMismatchedRolePlayerDynamicStateAllowedForInitial;
					break;
			}
			return retVal;
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicAddFactTypeUnboundRolesError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole.RolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleAddFactTypeUnboundRoles, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleAddFactTypeUnboundRolesCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicActionRoleUndeclaredObjectError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole.RolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleActionRoleUndeclaredObject, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleActionRoleUndeclaredObjectCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class ObjectUnifierMismatchedDynamicStateError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = ObjectUnifier.LeadRolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleObjectUnifierMismatchedDynamicState, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleObjectUnifierMismatchedDynamicStateCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicObjectAddPartialIdentifierError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			PathedRole pathedRole;
			RolePathObjectTypeRoot pathRoot;
			PathObjectUnifier objectUnifier;
			IModelErrorDisplayContext displayContext = null;
			if (null != (pathedRole = PathedRole))
			{
				displayContext = pathedRole.RolePath.RootOwner as DynamicRule;
			}
			else if (null != (pathRoot = PathRoot))
			{
				displayContext = pathRoot.RolePath.RootOwner as DynamicRule;
			}
			else if (null != (objectUnifier = ObjectUnifier))
			{
				displayContext = objectUnifier.LeadRolePath.RootOwner as DynamicRule;
			}
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRuleObjectAddPartialIdentifier, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRuleObjectAddPartialIdentifierCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(DynamicRuleErrorCategory))]
	partial class DynamicObjectificationAddRequiresLinkFactTypesError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole.RolePath.RootOwner as DynamicRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDynamicRulePopulateExternallyIdentifiedObjectificationWithLinkFactTypes, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDynamicRulePopulateExternallyIdentifiedObjectificationWithLinkFactTypesCompact;
			}
		}
	}
	#endregion // DynamicRule Errors
}
