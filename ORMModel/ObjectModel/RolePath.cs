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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region RolePath class
	partial class RolePath
	{
		#region Abstract Properties
		/// <summary>
		/// Return the root <see cref="LeadRolePath"/> associated
		/// with this path.
		/// </summary>
		public abstract LeadRolePath RootRolePath { get;}
		#endregion // Abstract Properties
		#region Accessors Properties
		/// <summary>
		/// Return the <see cref="PathedRole"/> relationships attached
		/// to this <see cref="RolePath"/>
		/// </summary>
		public ReadOnlyCollection<PathedRole> PathedRoleCollection
		{
			get
			{
				return PathedRole.GetLinksToRoleCollection(this);
			}
		}
		/// <summary>
		/// Get the <see cref="ObjectType"/> that provides the starting
		/// point for this <see cref="RolePath"/> but is not the root
		/// object type. Use <see cref="ContextOrRootObjectType"/> to get
		/// a fully resolved starting object type.
		/// </summary>
		public ObjectType ContextObjectType
		{
			get
			{
				PathedRole dummy;
				return GetContextObjectType(false, out dummy);
			}
		}
		/// <summary>
		/// Get the <see cref="ObjectType"/> that provides the starting
		/// point for this <see cref="RolePath"/>. The starting object type
		/// will be the role player of last role in a containing path,
		/// of the <see cref="LeadRolePath.RootObjectType"/> if there
		/// is no containing path. Use <see cref="ContextObjectType"/> to
		/// ignore the root object type.
		/// </summary>
		public ObjectType ContextOrRootObjectType
		{
			get
			{
				PathedRole dummy;
				return GetContextObjectType(true, out dummy);
			}
		}
		/// <summary>
		/// Get the <see cref="PathedRole"/> that occurs immediately before
		/// this path.
		/// </summary>
		public PathedRole ContextPathedRole
		{
			get
			{
				PathedRole retVal;
				GetContextObjectType(false, out retVal);
				return retVal;
			}
		}
		private ObjectType GetContextObjectType(bool returnRootObjectType, out PathedRole contextPathedRole)
		{
			RolePath currentPath = this;
			while (currentPath != null)
			{
				RoleSubPath subPath;
				if (null != (subPath = currentPath as RoleSubPath))
				{
					RolePath parentPath = subPath.ParentRolePath;
					ReadOnlyCollection<PathedRole> pathedRoles = parentPath.PathedRoleCollection;
					int pathRoleCount = pathedRoles.Count;
					if (pathRoleCount == 0)
					{
						currentPath = parentPath;
					}
					else
					{
						return (contextPathedRole = pathedRoles[pathRoleCount - 1]).Role.RolePlayer;
					}
				}
				else
				{
					contextPathedRole = null;
					return returnRootObjectType ? ((LeadRolePath)currentPath).RootObjectType : null;
				}
			}
			contextPathedRole = null;
			return null;
		}
		#endregion // Accessors Properties
		#region Delayed Validation
		/// <summary>
		/// Called when a subbranch is removed. If there is one remaining subbranch,
		/// then move the remaining elements in a single branch to the end of the current branch.
		/// </summary>
		/// <param name="element">A <see cref="RoleSubPath"/></param>
		private static void DelayValidatePathCollapse(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			RolePath parentPath = (RolePath)element;
			LinkedElementCollection<RoleSubPath> subPaths = parentPath.SplitPathCollection;
			if (subPaths.Count == 1)
			{
				// Remove the tail split by moving all elements up one level
				RolePath collapsePath = subPaths[0];

				// Move pathed roles
				foreach (PathedRole pathedRole in collapsePath.PathedRoleCollection)
				{
					pathedRole.RolePath = parentPath;
				}

				// Move sub paths
				foreach (RoleSubPathIsContinuationOfRolePath subPathLink in RoleSubPathIsContinuationOfRolePath.GetLinksToSplitPathCollection(collapsePath))
				{
					subPathLink.ParentRolePath = parentPath;
				}

				// Change the parent split settings to the path we just collapsed
				parentPath.SplitIsNegated = collapsePath.SplitIsNegated;
				parentPath.SplitCombinationOperator = collapsePath.SplitCombinationOperator;

				// We're done with it. Note that this can trigger other rules which will
				// reenter this routine for the grandparent branch
				collapsePath.Delete();
			}
		}
		#endregion // Delayed Validation
		#region Rule Methods
		/// <summary>
		/// DeleteRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRole)e.ModelElement).RolePath;
			LeadRolePath leadRolePath;
			if (!rolePath.IsDeleted &&
				rolePath.PathedRoleCollection.Count == 0 &&
				rolePath.SplitPathCollection.Count == 0 &&
				(null == (leadRolePath = rolePath as LeadRolePath) || leadRolePath.RootObjectType == null))
			{
				rolePath.Delete();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == PathedRole.RolePathDomainRoleId)
			{
				RolePath rolePath = (RolePath)e.OldRolePlayer;
				LeadRolePath leadRolePath;
				if (!rolePath.IsDeleted &&
					rolePath.PathedRoleCollection.Count == 0 &&
					rolePath.SplitPathCollection.Count == 0 &&
					(null == (leadRolePath = rolePath as LeadRolePath) || leadRolePath.RootObjectType == null))
				{
					rolePath.Delete();
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(LeadRolePathHasRootObjectType), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches
		/// </summary>
		private static void RootObjectTypeDeletedRule(ElementDeletedEventArgs e)
		{
			LeadRolePath leadRolePath = ((LeadRolePathHasRootObjectType)e.ModelElement).LeadRolePath;
			if (!leadRolePath.IsDeleted &&
				leadRolePath.PathedRoleCollection.Count == 0 &&
				leadRolePath.SplitPathCollection.Count == 0 &&
				leadRolePath.RootObjectType == null)
			{
				leadRolePath.Delete();
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// If a subbranch is deleted, then delay validate if the parent branch should
		/// attempt to collapse a remaining branch.
		/// </summary>
		private static void SubPathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath parentRolePath = ((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath;
			if (!parentRolePath.IsDeleted)
			{
				LeadRolePath leadRolePath;
				if (parentRolePath.PathedRoleCollection.Count == 0 &&
					parentRolePath.SplitPathCollection.Count == 0 &&
					(null == (leadRolePath = parentRolePath as LeadRolePath) || leadRolePath.RootObjectType == null))
				{
					parentRolePath.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Check branch collapsing for role player changes.
		/// </summary>
		private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId)
			{
				// The parent has lost a branch, validate if needed
				RolePath parentRolePath = (RolePath)e.OldRolePlayer;
				if (!parentRolePath.IsDeleted)
				{
					LeadRolePath leadRolePath;
					if (parentRolePath.PathedRoleCollection.Count == 0 &&
						parentRolePath.SplitPathCollection.Count == 0 &&
						(null == (leadRolePath = parentRolePath as LeadRolePath) || leadRolePath.RootObjectType == null))
					{
						parentRolePath.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
					}
				}
			}
		}
		#endregion // Rule Methods
	}
	#endregion // RolePath class
	#region LeadRolePath class
	partial class LeadRolePath : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// This path is the root of the path
		/// </summary>
		public override LeadRolePath RootRolePath
		{
			get
			{
				return this;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathOwnerHasPathComponent.PathComponentDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // LeadRolePath class
	#region RolePathComponent class
	partial class RolePathComponent : IModelErrorDisplayContext
	{
		#region Accessor Properties
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path component.
		/// </summary>
		public ORMModel Model
		{
			get
			{
				RolePathOwner owner = RootOwner;
				return (owner != null) ? owner.Model : null;
			}
		}
		/// <summary>
		/// Get the resolved <see cref="RolePathOwner"/> for this <see cref="RolePathComponent"/>
		/// </summary>
		public RolePathOwner RootOwner
		{
			get
			{
				RolePathComponent component = this;
				// The inheritance hierarchy is artificial here so
				// that a LeadRolePath can be both a RolePath and
				// a RolePathComponent. This means that RoleSubPath,
				// which is never directly parented by an owner or
				// compositor, is also a component. Treat this as
				// a special case with a supertype accessing a subtype.
				RoleSubPath subPath = component as RoleSubPath;
				if (subPath != null)
				{
					component = subPath.RootRolePath;
				}
				return component.PathOwner;
			}
		}
		#endregion // Accessor Properties
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the component level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = PathOwner as IModelErrorDisplayContext;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // RolePathComponent class
	#region RolePathCompositor class
	partial class RolePathCombination : IHasIndirectModelErrorOwner
	{
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathOwnerHasPathComponent.PathComponentDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Rule Methods
		/// <summary>
		/// DeleteRule: typeof(RolePathCombinationHasPathComponent), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Remove a combination when the last component is removed
		/// </summary>
		private static void RolePathCombinationHasPathComponentDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathCombination combination = ((RolePathCombinationHasPathComponent)e.ModelElement).Combination;
			if (!combination.IsDeleted)
			{
				if (combination.PathComponentCollection.Count == 0)
				{
					combination.Delete();
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathCombinationCorrelationCorrelatesPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Remove a correlation group when the last role is removed
		/// </summary>
		private static void CombininedCorrelatedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathCombinationCorrelation correlation = ((RolePathCombinationCorrelationCorrelatesPathedRole)e.ModelElement).Correlation;
			if (!correlation.IsDeleted)
			{
				if (correlation.CorrelatedRoleCollection.Count == 0)
				{
					correlation.Delete();
				}
			}
		}
		#endregion // Rule Methods
	}
	#endregion // RolePathCompositor class
	#region RolePathOwner class
	partial class RolePathOwner : IModelErrorOwner
	{
		#region Abstract members
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path owner.
		/// </summary>
		public abstract ORMModel Model { get;}
		#endregion // Abstract members
		#region Helper Methods
		private delegate void PathedRoleVisitor(PathedRole pathedRole);
		private static void VisitPathedRoles(RolePathComponent pathComponent, PathedRoleVisitor visitor)
		{
			LeadRolePath rolePath;
			RolePathCombination combination;
			if (null != (rolePath = pathComponent as LeadRolePath))
			{
				VisitPathedRoles(rolePath, visitor);
			}
			else if (null != (combination = pathComponent as RolePathCombination))
			{
				foreach (RolePathComponent childComponent in combination.PathComponentCollection)
				{
					VisitPathedRoles(childComponent, visitor);
				}
			}
		}
		private static void VisitPathedRoles(RolePath rolePath, PathedRoleVisitor visitor)
		{
			foreach (PathedRole pathedRole in rolePath.PathedRoleCollection)
			{
				visitor(pathedRole);
			}
			foreach (RoleSubPath subPath in rolePath.SplitPathCollection)
			{
				VisitPathedRoles(subPath, visitor);
			}
		}
		#endregion // Helper Methods
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			List<ModelErrorUsage> errors = null;
			ModelError error;
			foreach (RolePathComponent pathComponent in PathComponentCollection)
			{
				LeadRolePath rolePath = pathComponent as LeadRolePath;
				if (rolePath != null)
				{
					if (null != (error = rolePath.RootObjectTypeRequiredError))
					{
						yield return error;
					}
					// UNDONE: PathedRole and PathComponent should probably be their own model error owners,
					// although all validation (except for value constraints) will come through the owner.
					VisitPathedRoles(
						rolePath,
						delegate(PathedRole pathedRole)
						{
							if (null != (error = pathedRole.StartRoleError))
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
							if (null != (error = pathedRole.CorrelationCompatibilityError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.MandatoryOuterJoinError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							foreach (ValueConstraint valueConstraint in pathedRole.ValueConstraintCollection)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}
						});
					if (errors != null && errors.Count != 0)
					{
						foreach (ModelErrorUsage errorUsage in errors)
						{
							yield return errorUsage;
						}
						errors.Clear();
					}
				}
				foreach (CalculatedPathValue calculation in pathComponent.CalculatedValueCollection)
				{
					if (null != (error = calculation.FunctionRequiredError))
					{
						yield return error;
					}
					foreach (CalculatedPathValueParameterBindingError bindingError in calculation.ParameterBindingErrorCollection)
					{
						yield return bindingError;
					}
					if (null != (error = calculation.ConsumptionRequiredError))
					{
						yield return error;
					}
				}
			}
			foreach (ModelErrorUsage errorUsage in base.GetErrorCollection(startFilter))
			{
				yield return errorUsage;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.ValidateErrors"/>
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidatePathComponents(true, notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.DelayValidateErrors"/>
		/// </summary>
		protected new void DelayValidateErrors()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidatePathComponentsWithCalculations);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Deserialization Fixup
		#region Deprecated Element Removal
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// replaces deprecated role path elements.
		/// </summary>
		public static IDeserializationFixupListener UpdateRolePathFixupListener
		{
			get
			{
				return new ReplaceDeprecatedElementsFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Replaces deprecated role path representations.
		/// </summary>
		private sealed class ReplaceDeprecatedElementsFixupListener : DeserializationFixupListener<RolePathOwner>
		{
			/// <summary>
			/// ReplaceDeprecatedElementsFixupListener constructor
			/// </summary>
			public ReplaceDeprecatedElementsFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Check that this was created on load, not programmatically during
			/// fixup. The only path owners that are automatically created are
			/// automatic join paths, which will be parented before this is called.
			/// </summary>
			protected override bool VerifyElementType(ModelElement element)
			{
				ConstraintRoleSequenceJoinPath joinPath = element as ConstraintRoleSequenceJoinPath;
				return joinPath == null || joinPath.RoleSequence == null;
			}
			/// <summary>
			/// Replace deprecated role path structures
			/// </summary>
			protected sealed override void ProcessElement(RolePathOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					// The deprecated patterns still supported in the schema and loader are:
					// 1) OLD: RolePathCombination recursively aggregates other path components.
					//    NEW: RolePathCombination and LeadRolePath are kept in the same top-level
					//    collection.
					// 2) OLD: CalculatedPathValue is stored at the level or the RolePathOwner.
					//    NEW: CalculatedPathValue is stored with each PathComponent it applies to.
					// 3) OLD: FactType and constraint role derivation is stored as a many-to-one
					//    relationship on the role or constraint role.
					//    NEW: Each top-level path or path combination can specify a projection.
					
					// Unwind nested role paths
					RolePathOwnerHasPathComponent_Deprecated rootContainmentLink = RolePathOwnerHasPathComponent_Deprecated.GetLinkToPathComponent(element);
					if (rootContainmentLink != null)
					{
						RolePathComponent topLevelComponent = rootContainmentLink.PathComponent;
						rootContainmentLink.Delete(); // Does not propagate, the component is now unparented
						// Unwind the nested path components and reattach them to the owner using the new
						// flat containment relationship.
						AttachPathComponent(element, topLevelComponent, notifyAdded);
					}

					// Note that we shouldn't get these if we have no single root container, but the
					// XML still officially supports them. Given that functions for path combinations
					// came in after this change, we will reasonable expect calculated values to use
					// roles from a single LeadRolePath and delete them otherwise.
					ReadOnlyCollection<RolePathOwnerCalculatesCalculatedPathValue_Deprecated> calculatedValueLinks = RolePathOwnerCalculatesCalculatedPathValue_Deprecated.GetLinksToCalculatedValueCollection(element);
					bool checkedSingleRolePath = false;
					LeadRolePath singleLeadRolePath = null;
					bool noLeadRolePath = false;
					if (calculatedValueLinks.Count != 0)
					{
						FindSingleLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						checkedSingleRolePath = true;
						foreach (RolePathOwnerCalculatesCalculatedPathValue_Deprecated calculatedValueLink in calculatedValueLinks)
						{
							CalculatedPathValue calculatedValue = calculatedValueLink.CalculatedValue;
							RolePathComponentCalculatesCalculatedPathValue newCalculatedValueLink = null;
							calculatedValueLink.Delete();
							if (noLeadRolePath)
							{
								calculatedValue.Delete();
							}
							else if (singleLeadRolePath != null)
							{
								newCalculatedValueLink = new RolePathComponentCalculatesCalculatedPathValue(singleLeadRolePath, calculatedValue);
							}
							else
							{
								LeadRolePath parentRolePath = ResolveCalculatedValueRolePath(calculatedValue);
								if (parentRolePath != null)
								{
									newCalculatedValueLink = new RolePathComponentCalculatesCalculatedPathValue(singleLeadRolePath, calculatedValue);
								}
								else
								{
									calculatedValue.Delete();
								}
							}
							if (notifyAdded != null && newCalculatedValueLink != null)
							{
								notifyAdded.ElementAdded(newCalculatedValueLink);
							}
						}
					}

					// Port old projections on fact type derivation rules and join paths
					FactTypeDerivationRule factTypeDerivation;
					ConstraintRoleSequenceJoinPath joinPath;
					if (null != (factTypeDerivation = element as FactTypeDerivationRule))
					{
						if (!checkedSingleRolePath)
						{
							FindSingleLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						}
						LinkedElementCollection<RoleBase> factRoles = factTypeDerivation.FactType.RoleCollection;
						int factRoleCount = factRoles.Count;
						LeadRolePath resolvedLeadRolePath = null;
						FactTypeDerivationProjection derivationProjection = null;
						bool repeatLoop = true;
						while (repeatLoop)
						{
							repeatLoop = false;
							foreach (RoleBase roleBase in factRoles)
							{
								Role role = roleBase as Role;
								if (role != null)
								{
									RoleDerivesFromPathedRole_Deprecated sourcePathedRoleLink;
									RoleDerivesFromCalculatedPathValue_Deprecated sourceCalculatedValueLink;
									PathConstant pathConstant;
									RoleDerivesFromPathConstant_Deprecated sourceConstantLink;
									if (null != (sourcePathedRoleLink = RoleDerivesFromPathedRole_Deprecated.GetLinkToDerivedFromPathedRole(role)))
									{
										if (noLeadRolePath)
										{
											sourcePathedRoleLink.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											// factTypeDerivation, singleLeadRolePath, role, elementAdded, ref derivationProjection
											FactTypeRoleProjection roleProjection = EnsureFactTypeRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											roleProjection.ProjectedFromPathedRole = null;
											FactTypeRoleProjectedFromPathedRole pathedRoleLink = new FactTypeRoleProjectedFromPathedRole(roleProjection, sourcePathedRoleLink.Source);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(pathedRoleLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromCalculatedValue = null;
											if (null != (pathConstant = roleProjection.ProjectedFromConstant))
											{
												pathConstant.Delete();
											}
											sourcePathedRoleLink.Delete();
										}
										else if (resolvedLeadRolePath == null)
										{
											resolvedLeadRolePath = sourcePathedRoleLink.Source.RolePath.RootRolePath;
											if (resolvedLeadRolePath == null)
											{
												noLeadRolePath = true;
												repeatLoop = true;
												break;
											}
										}
										else if (resolvedLeadRolePath != sourcePathedRoleLink.Source.RolePath.RootRolePath)
										{
											// Can't resolve the binding to a single path, treat the same as no lead role path
											resolvedLeadRolePath = null;
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (null != (sourceCalculatedValueLink = RoleDerivesFromCalculatedPathValue_Deprecated.GetLinkToDerivedFromCalculatedValue(role)))
									{
										if (noLeadRolePath)
										{
											sourceCalculatedValueLink.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											FactTypeRoleProjection roleProjection = EnsureFactTypeRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											roleProjection.ProjectedFromCalculatedValue = null;
											FactTypeRoleProjectedFromCalculatedPathValue calculatedValueLink = new FactTypeRoleProjectedFromCalculatedPathValue(roleProjection, sourceCalculatedValueLink.Source);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(calculatedValueLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromPathedRole = null;
											if (null != (pathConstant = roleProjection.ProjectedFromConstant))
											{
												pathConstant.Delete();
											}
											sourceCalculatedValueLink.Delete();
										}
										else if (resolvedLeadRolePath == null)
										{
											resolvedLeadRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source);
											if (resolvedLeadRolePath == null)
											{
												noLeadRolePath = true;
												repeatLoop = true;
												break;
											}
										}
										else if (resolvedLeadRolePath != ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source))
										{
											// Can't resolve the binding to a single path, treat the same as no lead role path
											resolvedLeadRolePath = null;
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (null != (sourceConstantLink = RoleDerivesFromPathConstant_Deprecated.GetLinkToDerivedFromConstant(role)))
									{
										if (noLeadRolePath)
										{
											sourceConstantLink.Source.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											FactTypeRoleProjection roleProjection = EnsureFactTypeRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											pathConstant = sourceConstantLink.Source;
											sourceConstantLink.Delete(); // Introducing a second aggregate, make sure we only have one live at a time.
											roleProjection.ProjectedFromConstant = null;
											FactTypeRoleProjectedFromPathConstant constantLink = new FactTypeRoleProjectedFromPathConstant(roleProjection, pathConstant);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(constantLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromPathedRole = null;
											roleProjection.ProjectedFromCalculatedValue = null;
										}
										// Constants can't be used to determine a role path
									}
								}
							}
							if (!repeatLoop && !noLeadRolePath && singleLeadRolePath == null && resolvedLeadRolePath != null)
							{
								singleLeadRolePath = resolvedLeadRolePath;
								repeatLoop = true;
							}
						}
					}
					else if (null != (joinPath = element as ConstraintRoleSequenceJoinPath))
					{
						if (!checkedSingleRolePath)
						{
							FindSingleLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						}
						ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(joinPath.RoleSequence);
						int constraintRoleCount = constraintRoles.Count;
						LeadRolePath resolvedLeadRolePath = null;
						ConstraintRoleSequenceJoinPathProjection sequenceProjection = null;
						bool repeatLoop = true;
						while (repeatLoop)
						{
							repeatLoop = false;
							foreach (ConstraintRoleSequenceHasRole constraintRole in constraintRoles)
							{
								ConstraintRoleProjectedFromPathedRole_Deprecated sourcePathedRoleLink;
								ConstraintRoleProjectedFromCalculatedPathValue_Deprecated sourceCalculatedValueLink;
								PathConstant pathConstant;
								ConstraintRoleProjectedFromPathConstant_Deprecated sourceConstantLink;
								if (null != (sourcePathedRoleLink = ConstraintRoleProjectedFromPathedRole_Deprecated.GetLinkToProjectedFromPathedRole(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourcePathedRoleLink.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										constraintRoleProjection.ProjectedFromPathedRole = null;
										ConstraintRoleProjectedFromPathedRole pathedRoleLink = new ConstraintRoleProjectedFromPathedRole(constraintRoleProjection, sourcePathedRoleLink.Source);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(pathedRoleLink);
										}
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
										if (null != (pathConstant = constraintRoleProjection.ProjectedFromConstant))
										{
											pathConstant.Delete();
										}
										sourcePathedRoleLink.Delete();
									}
									else if (resolvedLeadRolePath == null)
									{
										resolvedLeadRolePath = sourcePathedRoleLink.Source.RolePath.RootRolePath;
										if (resolvedLeadRolePath == null)
										{
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (resolvedLeadRolePath != sourcePathedRoleLink.Source.RolePath.RootRolePath)
									{
										// Can't resolve the binding to a single path, treat the same as no lead role path
										resolvedLeadRolePath = null;
										noLeadRolePath = true;
										repeatLoop = true;
										break;
									}
								}
								else if (null != (sourceCalculatedValueLink = ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.GetLinkToProjectedFromCalculatedValue(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourceCalculatedValueLink.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
										ConstraintRoleProjectedFromCalculatedPathValue calculatedValueLink = new ConstraintRoleProjectedFromCalculatedPathValue(constraintRoleProjection, sourceCalculatedValueLink.Source);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(calculatedValueLink);
										}
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromPathedRole = null;
										if (null != (pathConstant = constraintRoleProjection.ProjectedFromConstant))
										{
											pathConstant.Delete();
										}
										sourceCalculatedValueLink.Delete();
									}
									else if (resolvedLeadRolePath == null)
									{
										resolvedLeadRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source);
										if (resolvedLeadRolePath == null)
										{
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (resolvedLeadRolePath != ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source))
									{
										// Can't resolve the binding to a single path, treat the same as no lead role path
										resolvedLeadRolePath = null;
										noLeadRolePath = true;
										repeatLoop = true;
										break;
									}
								}
								else if (null != (sourceConstantLink = ConstraintRoleProjectedFromPathConstant_Deprecated.GetLinkToProjectedFromConstant(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourceConstantLink.Source.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										pathConstant = sourceConstantLink.Source;
										sourceConstantLink.Delete(); // Introducing a second aggregate, make sure we only have one live at a time.
										constraintRoleProjection.ProjectedFromConstant = null;
										ConstraintRoleProjectedFromPathConstant constantLink = new ConstraintRoleProjectedFromPathConstant(constraintRoleProjection, pathConstant);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(constantLink);
										}
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromPathedRole = null;
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
									}
									// Constants can't be used to determine a role path
								}
							}
							if (!repeatLoop && !noLeadRolePath && singleLeadRolePath == null && resolvedLeadRolePath != null)
							{
								singleLeadRolePath = resolvedLeadRolePath;
								repeatLoop = true;
							}
						}
					}
				}
			}
			private static FactTypeRoleProjection EnsureFactTypeRoleProjection(ref FactTypeDerivationProjection derivationProjection, FactTypeDerivationRule factTypeDerivationRule, LeadRolePath projectedFromRolePath, Role projectedOnRole, INotifyElementAdded notifyAdded)
			{
				FactTypeRoleProjection roleProjection = null;
				if (null == derivationProjection &&
					null == (derivationProjection = FactTypeDerivationProjection.GetLink(factTypeDerivationRule, projectedFromRolePath)))
				{
					derivationProjection = new FactTypeDerivationProjection(factTypeDerivationRule, projectedFromRolePath);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(derivationProjection);
					}
				}
				else
				{
					roleProjection = FactTypeRoleProjection.GetLink(derivationProjection, projectedOnRole);
				}
				if (roleProjection == null)
				{
					roleProjection = new FactTypeRoleProjection(derivationProjection, projectedOnRole);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(roleProjection);
					}
				}
				return roleProjection;
			}
			private static ConstraintRoleProjection EnsureConstraintRoleProjection(ref ConstraintRoleSequenceJoinPathProjection joinPathProjection, ConstraintRoleSequenceJoinPath joinPath, LeadRolePath projectedFromRolePath, ConstraintRoleSequenceHasRole projectedOnConstraintRole, INotifyElementAdded notifyAdded)
			{
				ConstraintRoleProjection constraintRoleProjection = null;
				if (null == joinPathProjection &&
					null == (joinPathProjection = ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, projectedFromRolePath)))
				{
					joinPathProjection = new ConstraintRoleSequenceJoinPathProjection(joinPath, projectedFromRolePath);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(joinPathProjection);
					}
				}
				else
				{
					constraintRoleProjection = ConstraintRoleProjection.GetLink(joinPathProjection, projectedOnConstraintRole);
				}
				if (constraintRoleProjection == null)
				{
					constraintRoleProjection = new ConstraintRoleProjection(joinPathProjection, projectedOnConstraintRole);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(constraintRoleProjection);
					}
				}
				return constraintRoleProjection;
			}
			private static void FindSingleLeadRolePath(RolePathOwner pathOwner, out LeadRolePath singleLeadRolePath, out bool noLeadRolePath)
			{
				LinkedElementCollection<RolePathComponent> components = pathOwner.PathComponentCollection;
				singleLeadRolePath = null;
				noLeadRolePath = false;
				switch (components.Count)
				{
					case 0:
						// Any functions cannot be reattached
						noLeadRolePath = true;
						break;
					case 1:
						singleLeadRolePath = components[0] as LeadRolePath;
						if (singleLeadRolePath == null)
						{
							noLeadRolePath = true;
						}
						break;
					default:
						bool hasLeadRolePath = false;
						foreach (RolePathComponent component in components)
						{
							if (singleLeadRolePath == null)
							{
								singleLeadRolePath = component as LeadRolePath;
								hasLeadRolePath = singleLeadRolePath != null;
							}
							else if (component is LeadRolePath)
							{
								singleLeadRolePath = null;
								break;
							}
						}
						noLeadRolePath = !hasLeadRolePath;
						break;
				}
			}
			private static LeadRolePath ResolveCalculatedValueRolePath(CalculatedPathValue calculatedValue)
			{
				LeadRolePath retVal = null;
				foreach (CalculatedPathValueInput input in calculatedValue.InputCollection)
				{
					LeadRolePath inputRolePath = null;
					PathedRole sourcePathedRole;
					CalculatedPathValue sourceCalculatedValue;
					if (null != (sourcePathedRole = input.SourcePathedRole))
					{
						inputRolePath = sourcePathedRole.RolePath.RootRolePath;
					}
					else if (null != (sourceCalculatedValue = input.SourceCalculatedValue))
					{
						inputRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValue);
					}
					// No data available from a constant.
					if (inputRolePath != null)
					{
						if (retVal == null)
						{
							retVal = inputRolePath;
						}
						else if (inputRolePath != retVal)
						{
							retVal = null; // Disagreement, can't get a reliable answer
							break;
						}
					}
				}
				return retVal;
			}
			/// <summary>
			/// Recursive helper function to attach nested paths. The deepest nested paths
			/// are attached first to optimize the resulting file structure.
			/// </summary>
			private static void AttachPathComponent(RolePathOwner pathOwner, RolePathComponent pathComponent, INotifyElementAdded notifyAdded)
			{
				RolePathCombination pathCombination;
				if (null != (pathCombination = pathComponent as RolePathCombination))
				{
					foreach (RolePathCompositorHasPathComponent_Deprecated nestedContainmentLink in RolePathCompositorHasPathComponent_Deprecated.GetLinksToPathComponentCollection(pathCombination))
					{
						RolePathComponent nestedComponent = nestedContainmentLink.PathComponent;
						nestedContainmentLink.Delete(); // Does not propagate or modify the readonly collection, unparented
						AttachPathComponent(pathOwner, nestedComponent, notifyAdded);
						RolePathCombinationHasPathComponent pathCombinationLink = new RolePathCombinationHasPathComponent(pathCombination, nestedComponent);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(pathCombinationLink);
						}
					}
				}
				RolePathOwnerHasPathComponent pathOwnerLink = new RolePathOwnerHasPathComponent(pathOwner, pathComponent);
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(pathOwnerLink);
				}
			}
		}
		#endregion // Deprecated Element Removal
		#region Implicit Element Creation
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// populates
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new AddImplicitElementsFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Replaces deprecated role path representations.
		/// </summary>
		private sealed class AddImplicitElementsFixupListener : DeserializationFixupListener<RolePathOwner>
		{
			/// <summary>
			/// ReplaceDeprecatedElementsFixupListener constructor
			/// </summary>
			public AddImplicitElementsFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Make sure implicit elements are added
			/// </summary>
			protected sealed override void ProcessElement(RolePathOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					LinkedElementCollection<RolePathComponent> components = element.PathComponentCollection;
					LeadRolePath rolePath;
					if (components.Count == 1 &&
						null != (rolePath = components[0] as LeadRolePath))
					{
						notifyAdded.ElementAdded(new RolePathOwnerHasSingleLeadRolePath(element, rolePath));
					}
				}
			}
		}
		#endregion // Implicit Element Creation
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// Attach rule notifications for subtype hierarchy changes
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public static void EnableRuleNotifications(Store store)
		{
			ObjectType.AddSubtypeHierarchyChangeRuleNotification(store, ValidatePathCompatibility);
		}
		/// <summary>
		/// Check compatibility on subtype hierarchy changes
		/// </summary>
		private static void ValidatePathCompatibility(ObjectType type)
		{
			LinkedElementCollection<Role> playedRoles = type.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				if (!playedRole.IsDeleting)
				{
					foreach (RolePath rolePath in playedRole.RolePathCollection)
					{
						AddDelayedPathValidation(rolePath);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((PathedRole)e.ModelElement).RolePath);
		}
		/// <summary>
		/// ChangeRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == PathedRole.PathedRolePurposeDomainPropertyId || propertyId == PathedRole.IsNegatedDomainPropertyId)
			{
				AddDelayedPathValidation(((PathedRole)e.ModelElement).RolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRole)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == PathedRole.RoleDomainRoleId)
			{
				AddDelayedPathValidation(((PathedRole)e.ElementLink).RolePath);
			}
			else
			{
				AddDelayedPathValidation((RolePath)e.OldRolePlayer);
				AddDelayedPathValidation((RolePath)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(PathedRoleIsRemotelyCorrelatedWithPathedRole)
		/// </summary>
		private static void RemoteCorrelationAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((PathedRoleIsRemotelyCorrelatedWithPathedRole)e.ModelElement).CorrelatingParent.RolePath);
		}
		/// <summary>
		/// DeleteRule: typeof(PathedRoleIsRemotelyCorrelatedWithPathedRole)
		/// </summary>
		private static void RemoteCorrelationDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRoleIsRemotelyCorrelatedWithPathedRole)e.ModelElement).CorrelatingParent.RolePath;
			if (!rolePath.IsDeleted)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRoleIsRemotelyCorrelatedWithPathedRole)
		/// </summary>
		private static void RemoteCorrelationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			// This will always change within the same lead path. Just use the new role player.
			AddDelayedPathValidation(((PathedRole)e.NewRolePlayer).RolePath);
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwner)
		/// </summary>
		private static void RolePathOwnerAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwner owner = (RolePathOwner)e.ModelElement;
			FrameworkDomainModel.DelayValidateElement(owner, DelayValidatePathComponents);
			owner.NewlyCreated();
		}
		/// <summary>
		/// ChangeRule: typeof(RolePath)
		/// </summary>
		private static void RolePathChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == RolePath.SplitIsNegatedDomainPropertyId) // The validation does not currently check the split type.
			{
				AddDelayedPathValidation((RolePath)e.ModelElement);
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasPathComponent)
		/// See if the SingleLeadRolePath has changed
		/// </summary>
		private static void RolePathComponentAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasPathComponent link = (RolePathOwnerHasPathComponent)e.ModelElement;
			RolePathOwner owner = link.PathOwner;
			FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
			FrameworkDomainModel.DelayValidateElement(owner, DelayValidatePathComponents);
			foreach (CalculatedPathValue calculation in link.PathComponent.CalculatedValueCollection)
			{
				// This would be unusual, but we need to make sure that any pre-attached calculations are validated
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasPathComponent)
		/// See if the SingleLeadRolePath has changed
		/// </summary>
		private static void RolePathComponentDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathOwner owner = ((RolePathOwnerHasPathComponent)e.ModelElement).PathOwner;
			if (!owner.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidatePathComponents);
			}
		}
		/// <summary>
		/// If path component changes result in a single <see cref="LeadRolePath"/>,
		/// then populate <see cref="RolePathOwnerHasSingleLeadRolePath"/> relationship.
		/// </summary>
		private static void DelayValidateSingleLeadRolePath(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				RolePathOwner owner = (RolePathOwner)element;
				LinkedElementCollection<RolePathComponent> components = owner.PathComponentCollection;
				LeadRolePath singleRolePath = components.Count == 1 ? components[0] as LeadRolePath : null;
				if (owner.SingleLeadRolePath != singleRolePath)
				{
					owner.SingleLeadRolePath = singleRolePath;
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathComponentCalculatesCalculatedPathValue)
		/// </summary>
		private static void CalculationAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((RolePathComponentCalculatesCalculatedPathValue)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
			// Note that we don't have a delete rule: all of the error deletion propagates
		}
		/// <summary>
		/// AddRule: typeof(RolePathComponentSatisfiesCalculatedCondition)
		/// </summary>
		private static void CalculationAsConditionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((RolePathComponentSatisfiesCalculatedCondition)e.ModelElement).CalculatedCondition, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathComponentSatisfiesCalculatedCondition)
		/// </summary>
		private static void CalculationAsConditionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((RolePathComponentSatisfiesCalculatedCondition)e.ModelElement).CalculatedCondition;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(((CalculatedPathValueIsCalculatedWithFunction)e.ElementLink).CalculatedValue, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueHasInput)
		/// </summary>
		private static void CalculationInputAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((CalculatedPathValueHasInput)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueHasInput)
		/// </summary>
		private static void CalculationInputDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueHasInput)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInputBindsToCalculatedPathValue link = (CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement;
			CalculatedPathValue calculation = link.Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
			FrameworkDomainModel.DelayValidateElement(link.Source, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInputBindsToCalculatedPathValue link = (CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement;
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = link.Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
			if (!(calculation = link.Source).IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToCalculatedPathValue)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
				FrameworkDomainModel.DelayValidateElement((CalculatedPathValue)e.OldRolePlayer, DelayValidateCalculatedPathValue);
				FrameworkDomainModel.DelayValidateElement((CalculatedPathValue)e.NewRolePlayer, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathConstant)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathedRole)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			foreach (RolePath rolePath in ((ObjectTypePlaysRole)e.ModelElement).PlayedRole.RolePathCollection)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			Role playedRole = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole;
			if (!playedRole.IsDeleted)
			{
				foreach (RolePath rolePath in playedRole.RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				foreach (RolePath rolePath in ((ObjectTypePlaysRole)e.ElementLink).PlayedRole.RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
			else
			{
				foreach (RolePath rolePath in ((Role)e.OldRolePlayer).RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
				foreach (RolePath rolePath in ((Role)e.NewRolePlayer).RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(LeadRolePathHasRootObjectType)
		/// </summary>
		private static void RootObjectTypeAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((LeadRolePathHasRootObjectType)e.ModelElement).LeadRolePath, DelayValidatePathComponent);
		}
		/// <summary>
		/// DeleteRule: typeof(LeadRolePathHasRootObjectType)
		/// </summary>
		private static void RootObjectTypeDeletedRule(ElementDeletedEventArgs e)
		{
			LeadRolePath leadRolePath = ((LeadRolePathHasRootObjectType)e.ModelElement).LeadRolePath;
			if (!leadRolePath.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(leadRolePath, DelayValidatePathComponent);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(LeadRolePathHasRootObjectType)
		/// </summary>
		private static void RootObjectTypeRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == LeadRolePathHasRootObjectType.RootObjectTypeDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(((LeadRolePathHasRootObjectType)e.ElementLink).LeadRolePath, DelayValidatePathComponent);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			MandatoryConstraint constraint;
			if (null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				(constraint.IsSimple || constraint.IsImplied)) // Note that implied single role mandatories can disqualify outer joins
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(link.Role))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintChangedRule(ElementPropertyChangedEventArgs e)
		{
			MandatoryConstraint constraint;
			LinkedElementCollection<Role> constraintRoles;
			if (e.DomainProperty.Id == MandatoryConstraint.ModalityDomainPropertyId &&
				((constraint = (MandatoryConstraint)e.ModelElement).IsSimple || constraint.IsImplied) &&
				1 == (constraintRoles = constraint.RoleCollection).Count)
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(constraintRoles[0]))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			Role role = link.Role;
			MandatoryConstraint constraint;
			if (!role.IsDeleted &&
				null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				(constraint.IsSimple || constraint.IsImplied)) // Note that implied single role mandatories can disqualify outer joins
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(role))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath);
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath parentPath = ((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath;
			if (!parentPath.IsDeleted)
			{
				 AddDelayedPathValidation(parentPath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId)
			{
				AddDelayedPathValidation(((RoleSubPathIsContinuationOfRolePath)e.ElementLink).ParentRolePath);
			}
			else
			{
				AddDelayedPathValidation((RolePath)e.OldRolePlayer);
				AddDelayedPathValidation((RolePath)e.NewRolePlayer);
			}
		}
		#endregion // Rule Methods
		#region Path Validation
		/// <summary>
		/// Helper to choose between root path and subpath validators
		/// </summary>
		private static void AddDelayedPathValidation(RolePath path)
		{
			if (path is LeadRolePath)
			{
				FrameworkDomainModel.DelayValidateElement(path, DelayValidatePathComponent);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(path, DelayValidateSubpath);
			}
		}
		/// <summary>
		/// Intermediate validator to allow rules to delay validate
		/// the closest element in the path hierarchy.
		/// </summary>
		/// <param name="element">A <see cref="RoleSubPath"/></param>
		[DelayValidatePriority(-2)] // Before component validation
		private static void DelayValidateSubpath(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(((RoleSubPath)element).RootRolePath, DelayValidatePathComponent);
			}
		}
		/// <summary>
		/// Intermediate validator to allow rules to delay validate the closest
		/// element in the path hierarchy. Defers indirectly to the owner validator.
		/// </summary>
		/// <param name="element">A <see cref="LeadRolePath"/> or <see cref="RolePathCombination"/></param>
		[DelayValidatePriority(-1)] // Run before the owner validation
		private static void DelayValidatePathComponent(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(((RolePathComponent)element).PathOwner, DelayValidatePathComponents);
			}
		}
		/// <summary>
		/// Validate all role path components except calculations
		/// </summary>
		/// <param name="element">A <see cref="RolePathOwner"/></param>
		private static void DelayValidatePathComponents(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RolePathOwner)element).ValidatePathComponents(false, null);
			}
		}
		/// <summary>
		/// Validate all role path components including calculations
		/// </summary>
		/// <param name="element">A <see cref="RolePathOwner"/></param>
		private static void DelayValidatePathComponentsWithCalculations(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RolePathOwner)element).ValidatePathComponents(true, null);
			}
		}
		/// <summary>
		/// Validate all path components
		/// </summary>
		/// <param name="validateCalculations">Validate calculated values with the path components.</param>
		/// <param name="notifyAdded">Notification callback for added errors.</param>
		private void ValidatePathComponents(bool validateCalculations, INotifyElementAdded notifyAdded)
		{
			Store store = Store;
			ORMModel model = null;
			BitTracker roleUseTracker = new BitTracker(0);
			Stack<LinkedElementCollection<RoleBase>> factTypeRolesStack = new Stack<LinkedElementCollection<RoleBase>>();
			ObjectType[] compatibilityTester = null;
			foreach (RolePathComponent pathComponent in PathComponentCollection)
			{
				// Validate the role paths first, then the combinations
				LeadRolePath leadRolePath = pathComponent as LeadRolePath;
				if (leadRolePath != null)
				{
					#region Root Object Type Validation
					ObjectType rootObjectType = leadRolePath.RootObjectType;
					PathRequiresRootObjectTypeError rootError = leadRolePath.RootObjectTypeRequiredError;
					if (rootObjectType == null)
					{
						if (rootError == null)
						{
							rootError = new PathRequiresRootObjectTypeError(store);
							rootError.LeadRolePath = leadRolePath;
							rootError.Model = model ?? (model = this.Model);
							rootError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(rootError, true);
							}
						}
					}
					else if (rootError != null)
					{
						rootError.Delete();
					}
					#endregion // Root Object Type Validation

					// Walk all pathed roles to check path structure and join compatibility. Errors
					// are specified on the pathed roles.
					VisitPathedRolesForValidation(
						leadRolePath,
						null,
						delegate(PathedRole pathedRole, PathedRole previousPathedRole, bool unwinding)
						{
							PathedRolePurpose purpose = pathedRole.PathedRolePurpose;
							if (unwinding)
							{
								#region FactType Role Stack Maintenance
								// All errors are checked moving forward in the path. All we need to do
								// is keep the fact type roles stack in order so that we don't lose
								// track of the current fact type an in intra-fact type split. This is a
								// skeleton version of the corresponding conditions in the forward part
								// of the callback.
								bool popFactType = false;
								if (previousPathedRole == null)
								{
									popFactType = true;
								}
								else
								{
									bool testPreviousFactType = false;
									bool sameFactTypeRole = true;
									switch (purpose)
									{
										case PathedRolePurpose.StartRole:
											testPreviousFactType = true;
											break;
										case PathedRolePurpose.SameFactType:
											sameFactTypeRole = true;
											testPreviousFactType = true;
											break;
										case PathedRolePurpose.PostInnerJoin:
										case PathedRolePurpose.PostOuterJoin:
											popFactType = true;
											break;
									}
									if (testPreviousFactType)
									{
										// See comments in the !unwinding branch for discussion
										// of conditions here.
										Role currentRole = pathedRole.Role;
										FactType currentFactType = currentRole.FactType;
										Role previousRole = previousPathedRole.Role;
										FactType previousFactType = previousRole.FactType;
										if (previousFactType != currentFactType)
										{
											popFactType = true;
											if (sameFactTypeRole)
											{
												Objectification objectification;
												RoleProxy proxy;
												if (previousPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType &&
													((null != (objectification = currentFactType.ImpliedByObjectification) &&
													null != (proxy = previousRole.Proxy) &&
													proxy.FactType == currentFactType) ||
													(null != (proxy = currentRole.Proxy) &&
													previousFactType == proxy.FactType)))
												{
													// Do nothing. No fact type was pushed in the forward iteration
													popFactType = false;
												}
											}
										}
									}
								}
								if (popFactType)
								{
									roleUseTracker.Resize(roleUseTracker.Count - factTypeRolesStack.Pop().Count);
								}
								#endregion // FactType Role Stack Maintenance
							}
							else
							{
								#region Tracked errors
								bool hasStartRoleError = false;
								bool hasSameFactTypeWithoutJoinError = false;
								bool hasOuterJoinError = false;
								bool hasJoinCompatibilityError = false;
								bool hasCorrelationCompatibilityError = false;
								#endregion // Tracked errors
								#region Validate PathedRole Combinations
								// State information
								Role currentRole = pathedRole.Role;
								FactType currentFactType;
								FactType pushFactType = null;
								ObjectType testCompatibilityWith = null;
								ObjectType currentRolePlayer = null;
								LinkedElementCollection<RoleBase> factTypeRoles;
								int resolvedRoleIndex;

								if (previousPathedRole == null)
								{
									switch (purpose)
									{
										case PathedRolePurpose.StartRole:
											testCompatibilityWith = rootObjectType;
											break;
										case PathedRolePurpose.SameFactType:
										case PathedRolePurpose.PostInnerJoin:
											hasStartRoleError = true;
											break;
										case PathedRolePurpose.PostOuterJoin:
											hasOuterJoinError = pathedRole.Role.SingleRoleAlethicMandatoryConstraint != null;
											hasStartRoleError = true;
											break;
									}
									currentFactType = pushFactType = currentRole.FactType;
								}
								else
								{
									bool testPreviousFactType = false;
									bool sameFactTypeRole = false;
									switch (purpose)
									{
										case PathedRolePurpose.StartRole:
											hasStartRoleError = true;
											// This is in the wrong position, treat the same as SameFactType for
											// invalid fact type transitions, but do not do any compatibility
											// checking until we get it in the right position.
											testPreviousFactType = true;
											break;
										case PathedRolePurpose.SameFactType:
											sameFactTypeRole = true;
											testPreviousFactType = true;
											break;
										case PathedRolePurpose.PostInnerJoin:
											testCompatibilityWith = previousPathedRole.Role.RolePlayer;
											pushFactType = currentRole.FactType;
											break;
										case PathedRolePurpose.PostOuterJoin:
											hasOuterJoinError = pathedRole.Role.SingleRoleAlethicMandatoryConstraint != null;
											testCompatibilityWith = previousPathedRole.Role.RolePlayer;
											pushFactType = currentRole.FactType;
											break;
									}
									if (testPreviousFactType)
									{
										// Push a new fact type if this role is not part of the previous fact type.
										// This is complicated by the ability of any role in an objectification
										// to be associated either with the objectified fact type or the link fact type.
										currentFactType = currentRole.FactType;
										Role previousRole = previousPathedRole.Role;
										FactType previousFactType = previousRole.FactType;
										if (previousFactType != currentFactType)
										{
											pushFactType = currentFactType;
											if (sameFactTypeRole)
											{
												if (previousPathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
												{
													hasSameFactTypeWithoutJoinError = true;
												}
												else
												{
													// See if we're encountering a link fact type situation here.
													Objectification objectification;
													RoleProxy proxy;
													if (null != (objectification = currentFactType.ImpliedByObjectification) &&
														null != (proxy = previousRole.Proxy) &&
														proxy.FactType == currentFactType)
													{
														// We're using an objectified role as an entry role
														// into the link fact type. We guessed wrong and pushed
														// the wrong fact type on the previous step, so update
														// the information to correspond to this fact type.
														factTypeRoles = factTypeRolesStack.Pop();
														int roleUseBaseIndex = roleUseTracker.Count - factTypeRoles.Count;
														roleUseTracker.Resize(roleUseBaseIndex);
														factTypeRoles = currentFactType.RoleCollection;
														roleUseTracker.Resize(roleUseBaseIndex + factTypeRoles.Count);
														roleUseTracker[roleUseBaseIndex + factTypeRoles.IndexOf(ResolveRoleBaseInFactType(previousRole, currentFactType))] = true;
														factTypeRolesStack.Push(factTypeRoles);
														pushFactType = null; // Set the use for the remaining role with continued processing
													}
													else if (null != (proxy = currentRole.Proxy) &&
														previousFactType == proxy.FactType)
													{
														// We're using the role opposite the proxy for the entry role.
														// We have the right fact type on the stack, so there is nothing more to do.
														currentFactType = previousFactType; // We need the link fact type to resolve the role index below
														pushFactType = null;
													}
													else
													{
														hasSameFactTypeWithoutJoinError = true;
													}
												}
											}
										}
										else if (sameFactTypeRole &&
											((currentRole == previousRole &&
											previousPathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType &&
											null != currentRole.Proxy) ||
											(-1 != (resolvedRoleIndex = (factTypeRoles = factTypeRolesStack.Peek()).IndexOf(ResolveRoleBaseInFactType(currentRole, currentFactType))) &&
											roleUseTracker[roleUseTracker.Count - factTypeRoles.Count + resolvedRoleIndex])))
										{
											// Make sure the current role is not already pathed in this fact type entry. The
											// first condition is a special test for a transition from a link to the same role
											// in an absorbed fact type. In this case, it isn't worth the trouble to see if
											// we have the right set of fact type roles (link or normal) because we can test
											hasSameFactTypeWithoutJoinError = true;
											pushFactType = currentFactType; // Push another fact type use, we should have a join here
										}
									}
									else
									{
										currentFactType = pushFactType;
									}
								}
								if (pushFactType != null)
								{
									factTypeRoles = pushFactType.RoleCollection;
									roleUseTracker.Resize(roleUseTracker.Count + factTypeRoles.Count);
									factTypeRolesStack.Push(factTypeRoles);
								}
								else
								{
									factTypeRoles = factTypeRolesStack.Peek();
								}
								resolvedRoleIndex = factTypeRoles.IndexOf(ResolveRoleBaseInFactType(currentRole, currentFactType));
								if (resolvedRoleIndex != -1) // Defensive, should always be set at this point
								{
									roleUseTracker[resolvedRoleIndex + roleUseTracker.Count - factTypeRoles.Count] = true;
								}
								#endregion // Validate PathedRole Combinations
								#region Compatibility Verification
								if (testCompatibilityWith != null)
								{
									currentRolePlayer = (currentRolePlayer ?? (currentRolePlayer = currentRole.RolePlayer));
									if (currentRolePlayer != null &&
										currentRolePlayer != testCompatibilityWith)
									{
										(compatibilityTester ?? (compatibilityTester = new ObjectType[2]))[0] = currentRolePlayer;
										compatibilityTester[1] = testCompatibilityWith;
										if (ObjectType.GetNearestCompatibleTypes(compatibilityTester).Length == 0)
										{
											hasJoinCompatibilityError = true;
										}
									}
								}
								PathedRole correlatingParent;
								if (null != (correlatingParent = pathedRole.CorrelatingParent) &&
									null != (testCompatibilityWith = correlatingParent.Role.RolePlayer))
								{
									currentRolePlayer = (currentRolePlayer ?? (currentRolePlayer = currentRole.RolePlayer));
									if (currentRolePlayer != null &&
										currentRolePlayer != testCompatibilityWith)
									{
										(compatibilityTester ?? (compatibilityTester = new ObjectType[2]))[0] = currentRolePlayer;
										compatibilityTester[1] = testCompatibilityWith;
										if (ObjectType.GetNearestCompatibleTypes(compatibilityTester).Length == 0)
										{
											hasCorrelationCompatibilityError = true;
										}
									}
								}
								#endregion // Compatibility Verification
								#region Attach or clear pathedRole errors
								PathStartRoleFollowsRootObjectTypeError startRoleError = pathedRole.StartRoleError;
								if (hasStartRoleError)
								{
									if (startRoleError == null)
									{
										startRoleError = new PathStartRoleFollowsRootObjectTypeError(store);
										startRoleError.PathedRole = pathedRole;
										startRoleError.Model = model ?? (model = this.Model);
										startRoleError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(startRoleError, true);
										}
									}
								}
								else if (startRoleError != null)
								{
									startRoleError.Delete();
								}

								PathSameFactTypeRoleFollowsJoinError sameFactTypeWithoutJoinError = pathedRole.SameFactTypeRoleWithoutJoinError;
								if (hasSameFactTypeWithoutJoinError)
								{
									if (sameFactTypeWithoutJoinError == null)
									{
										sameFactTypeWithoutJoinError = new PathSameFactTypeRoleFollowsJoinError(store);
										sameFactTypeWithoutJoinError.PathedRole = pathedRole;
										sameFactTypeWithoutJoinError.Model = model ?? (model = this.Model);
										sameFactTypeWithoutJoinError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(sameFactTypeWithoutJoinError, true);
										}
									}
								}
								else if (sameFactTypeWithoutJoinError != null)
								{
									sameFactTypeWithoutJoinError.Delete();
								}

								PathOuterJoinRequiresOptionalRoleError outerJoinError = pathedRole.MandatoryOuterJoinError;
								if (hasOuterJoinError)
								{
									if (outerJoinError == null)
									{
										outerJoinError = new PathOuterJoinRequiresOptionalRoleError(store);
										outerJoinError.PathedRole = pathedRole;
										outerJoinError.Model = model ?? (model = this.Model);
										outerJoinError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(outerJoinError, true);
										}
									}
								}
								else if (outerJoinError != null)
								{
									outerJoinError.Delete();
								}

								JoinedPathRoleRequiresCompatibleRolePlayerError joinCompatibilityError = pathedRole.JoinCompatibilityError;
								if (hasJoinCompatibilityError)
								{
									if (joinCompatibilityError == null)
									{
										joinCompatibilityError = new JoinedPathRoleRequiresCompatibleRolePlayerError(store);
										joinCompatibilityError.PathedRole = pathedRole;
										joinCompatibilityError.Model = model ?? (model = this.Model);
										joinCompatibilityError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(joinCompatibilityError, true);
										}
									}
								}
								else if (joinCompatibilityError != null)
								{
									joinCompatibilityError.Delete();
								}

								CorrelatedPathRoleRequiresCompatibleRolePlayerError correlationCompatibilityError = pathedRole.CorrelationCompatibilityError;
								if (hasCorrelationCompatibilityError)
								{
									if (correlationCompatibilityError == null)
									{
										correlationCompatibilityError = new CorrelatedPathRoleRequiresCompatibleRolePlayerError(store);
										correlationCompatibilityError.PathedRole = pathedRole;
										correlationCompatibilityError.Model = model ?? (model = this.Model);
										correlationCompatibilityError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(correlationCompatibilityError, true);
										}
									}
								}
								else if (correlationCompatibilityError != null)
								{
									correlationCompatibilityError.Delete();
								}
								#endregion // Attach or clear pathedRole errors
							}
						});
				}
				//else
				//{
				//    // UNDONE: Validate RolePathCombinations
				//}

				#region Validate calculation completeness
				if (validateCalculations)
				{
					foreach (CalculatedPathValue calculation in pathComponent.CalculatedValueCollection)
					{
						ValidateCalculatedPathValue(calculation, this, notifyAdded, ref model);
					}
				}
				#endregion // Validate calculation completeness
			}

			// Give owner derivations a chance for additional validation
			ValidateDerivedRolePathOwner(notifyAdded);
		}
		/// <summary>
		/// A callback point used during path validation to enable extensions
		/// to be validated along with the base.
		/// </summary>
		/// <param name="notifyAdded">Notification callback for added elements and errors.</param>
		protected virtual void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			// Intentionally empty
		}
		/// <summary>
		/// A helper callback for derived classes that consume calculations. Called during rule execution.
		/// </summary>
		/// <param name="calculation">A <see cref="CalculatedPathValue"/> to verify errors for.</param>
		protected static void CalculatedValueUseChangedInRule(CalculatedPathValue calculation)
		{
			FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// Called when a new <see cref="RolePathOwner"/> is created. Allows
		/// derived classes to add their own validators on initial creation.
		/// This is called before any parent relationships are established,
		/// so it is intended for registering delayed validation routines only.
		/// </summary>
		protected virtual void NewlyCreated()
		{
			// Intentionally empty
		}
		/// <summary>
		/// Determine if a <see cref="CalculatedPathValue"/> is consumed by the path owner.
		/// Derived classes can override this method to add additional consumption patterns
		/// for calculated values.
		/// </summary>
		protected virtual bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return calculation.BoundInputCollection.Count != 0 || calculation.RequiredForPathComponentCollection.Count != 0;
		}
		/// <summary>
		/// Delayed validator for changes in a <see cref="CalculatedPathValue"/>
		/// </summary>
		private static void DelayValidateCalculatedPathValue(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ORMModel model = null;
				ValidateCalculatedPathValue((CalculatedPathValue)element, null, null, ref model);
			}
		}
		/// <summary>
		/// Validate calculation structural requirements. The function must be specified,
		/// all parameters must be bound and have some input source, and the calculation
		/// result must be consumed in the model.
		/// </summary>
		/// <param name="calculation">The <see cref="CalculatedPathValue"/> to validate.</param>
		/// <param name="rolePathOwner">The containing <see cref="RolePathOwner"/> for this calculation.</param>
		/// <param name="notifyAdded">Callback notification used during deserialization.</param>
		/// <param name="contextModel">The context <see cref="ORMModel"/>. Calculated automatically if <see langword="null"/></param>
		private static void ValidateCalculatedPathValue(CalculatedPathValue calculation, RolePathOwner rolePathOwner, INotifyElementAdded notifyAdded, ref ORMModel contextModel)
		{
			Function function = calculation.Function;
			Store store = calculation.Store;
			CalculatedPathValueRequiresFunctionError functionRequiredError = calculation.FunctionRequiredError;
			if (function == null)
			{
				calculation.ParameterBindingErrorCollection.Clear();
				calculation.InputCollection.Clear(); // Doesn't make sense without a function
				if (functionRequiredError == null &&
					(null != contextModel || null != (contextModel = calculation.Model)))
				{
					if (contextModel == null)
					{
						contextModel = calculation.Model;
					}
					functionRequiredError = new CalculatedPathValueRequiresFunctionError(store);
					functionRequiredError.CalculatedPathValue = calculation;
					functionRequiredError.Model = contextModel;
					functionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(functionRequiredError, true);
					}
				}
			}
			else
			{
				if (functionRequiredError != null)
				{
					functionRequiredError.Delete();
				}
				LinkedElementCollection<FunctionParameter> parameters = function.ParameterCollection;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculation.InputCollection;
				int inputCount = inputs.Count;
				if (parameters.Count == 0)
				{
					inputs.Clear(); // No parameters means no inputs
				}
				else
				{
					// There are several options here for matching parameters and inputs.
					// Jumping from the parameters to all inputs is easier code, but the
					// sets involved are not bounded in any way. Nesting the parameters
					// and defined inputs will be the cleanest approach.
					ReadOnlyCollection<CalculatedPathValueHasUnboundParameterError> bindingErrorLinks = CalculatedPathValueHasUnboundParameterError.GetLinksToParameterBindingErrorCollection(calculation);
					int originalBindingErrorCount = bindingErrorLinks.Count;
					foreach (FunctionParameter parameter in parameters)
					{
						bool hasError = true;
						if (inputCount != 0)
						{
							foreach (CalculatedPathValueInput input in inputs)
							{
								if (input.Parameter == parameter)
								{
									// We've matched the parameter. Now, make sure that there
									// is some data on the input.
									if (input.SourcePathedRole != null ||
										input.SourceCalculatedValue != null ||
										input.SourceConstant != null)
									{
										hasError = false;
									}
									break;
								}
							}
						}
						if (originalBindingErrorCount != 0)
						{
							foreach (CalculatedPathValueHasUnboundParameterError bindingErrorLink in bindingErrorLinks)
							{
								if (!bindingErrorLink.IsDeleted && bindingErrorLink.Parameter == parameter)
								{
									if (!hasError)
									{
										--originalBindingErrorCount;
										bindingErrorLink.Delete();
									}
									else
									{
										hasError = false; // No more processing needed, we have an intact error object
									}
									break;
								}
							}
						}
						if (hasError &&
							(null != contextModel || null != (contextModel = calculation.Model)))
						{
							CalculatedPathValueParameterBindingError bindingError = new CalculatedPathValueParameterBindingError(store);
							CalculatedPathValueHasUnboundParameterError bindingErrorLink = new CalculatedPathValueHasUnboundParameterError(calculation, bindingError);
							bindingErrorLink.Parameter = parameter;
							bindingError.Model = contextModel;
							bindingError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(bindingError, false);
								notifyAdded.ElementAdded(bindingErrorLink, true);
							}
						}
					}
					if (inputCount != 0)
					{
						for (int i = inputCount - 1; i >= 0; --i) // Walk backwards to support deletion
						{
							CalculatedPathValueInput input = inputs[i];
							FunctionParameter parameter;
							if (null == (parameter = input.Parameter) || // Defensive, highly unlikely
								parameter.Function != function)
							{
								input.Delete();
							}
						}
					}
				}
			}

			// Finally, test if anything is using this calculation
			CalculatedPathValueMustBeConsumedError consumptionError = calculation.ConsumptionRequiredError;
			RolePathComponent component;
			if ((null != rolePathOwner ||
				(null != (component = calculation.PathComponent) &&
				null != (rolePathOwner = component.PathOwner))) &&
				!rolePathOwner.IsCalculatedPathValueConsumed(calculation))
			{
				if (consumptionError == null &&
					(null != contextModel || null != (contextModel = rolePathOwner.Model)))
				{
					consumptionError = new CalculatedPathValueMustBeConsumedError(store);
					consumptionError.CalculatedPathValue = calculation;
					consumptionError.Model = contextModel;
					consumptionError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(consumptionError, true);
					}
				}
			}
			else if (consumptionError != null)
			{
				consumptionError.Delete();
			}
		}
		/// <summary>
		/// Callback for <see cref="VisitPathedRolesForValidation"/>
		/// </summary>
		/// <param name="currentPathedRole">The current pathed role</param>
		/// <param name="previousPathedRole">The previous pathed role</param>
		/// <param name="unwinding">The stack is unwinding.</param>
		private delegate void RolePathValidationVisitor(PathedRole currentPathedRole, PathedRole previousPathedRole, bool unwinding);
		/// <summary>
		/// Iterate pathed roles for validation
		/// </summary>
		private void VisitPathedRolesForValidation(RolePath rolePath, PathedRole contextPathedRole, RolePathValidationVisitor visitor)
		{
			PathedRole splitContext = contextPathedRole;
			ReadOnlyCollection<PathedRole> pathedRoles = rolePath.PathedRoleCollection;
			int pathedRoleCount = pathedRoles.Count;
			for (int i = 0; i < pathedRoleCount; ++i)
			{
				PathedRole pathedRole = pathedRoles[i];
				visitor(pathedRole, splitContext, false);
				splitContext = pathedRole;
			}
			foreach (RoleSubPath subPath in rolePath.SplitPathCollection)
			{
				VisitPathedRolesForValidation(subPath, splitContext, visitor);
			}
			for (int i = pathedRoleCount - 1; i >= 0; --i)
			{
				visitor(pathedRoles[i], i == 0 ? contextPathedRole : pathedRoles[i - 1], true);
			}
		}
		/// <summary>
		/// Given a <see cref="Role"/> and <see cref="FactType"/>, determine
		/// the corresponding <see cref="RoleBase"/> that is either in the
		/// normal or implied fact type.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to resolve.</param>
		/// <param name="factType">The <see cref="FactType"/> to get the returned <see cref="RoleBase"/> in.</param>
		/// <returns>The resolved <see cref="RoleBase"/></returns>
		private static RoleBase ResolveRoleBaseInFactType(Role role, FactType factType)
		{
			if (role.FactType != factType)
			{
				RoleProxy proxy = role.Proxy;
				if (proxy != null && proxy.FactType == factType)
				{
					return proxy;
				}
			}
			return role;
		}
		#endregion // Path Validation
	}
	#endregion // RolePathOwner class
	#region FactTypeDerivationRule class
	partial class FactTypeDerivationRule : IModelErrorDisplayContext, IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="FactType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				FactType factType = FactType;
				return factType != null ? factType.Model : null;
			}
		}
		/// <summary>
		/// Add role projections to the set of base elements than can consume a <see cref="CalculatedPathValue"/>
		/// </summary>
		protected override bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return base.IsCalculatedPathValueConsumed(calculation) || FactTypeRoleProjectedFromCalculatedPathValue.GetFactTypeRoleProjections(calculation).Count != 0;
		}
		/// <summary>
		/// Register validation for a new <see cref="FactTypeDerivationRule"/>
		/// </summary>
		protected override void NewlyCreated()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateProjections);
		}
		/// <summary>
		/// Check derivation projections
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback.</param>
		protected override void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			if (notifyAdded != null)
			{
				// We do all projection analysis independently after the deserialization request
				ValidateProjections(notifyAdded);
			}
		}
		/// <summary>
		/// Verify all projection errors
		/// </summary>
		/// <param name="element">A <see cref="FactTypeDerivationRule"/></param>
		private static void DelayValidateProjections(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((FactTypeDerivationRule)element).ValidateProjections(null);
			}
		}
		/// <summary>
		/// Validate projections for existence and completeness
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback. Perform extra
		/// structural checks if this is set.</param>
		private void ValidateProjections(INotifyElementAdded notifyAdded)
		{
			bool seenProjection = false;
			int factTypeRoleCount = 0;
			Store store = Store;
			ORMModel model = null;
			foreach (FactTypeDerivationProjection projection in FactTypeDerivationProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				if (factTypeRoleCount == 0)
				{
					LinkedElementCollection<RoleBase> factTypeRoles = FactType.RoleCollection;
					factTypeRoleCount = factTypeRoles.Count;
					if (factTypeRoleCount == 2 &&
						FactType.GetUnaryRoleIndex(factTypeRoles).HasValue)
					{
						factTypeRoleCount = 1;
					}
				}
				int projectedRoleCount;
				if (notifyAdded != null)
				{
					// Clean up projections with no source information on load. Empty
					// projections are automatically removed after deserialization, so
					// verify that we have a consistent state.
					ReadOnlyCollection<FactTypeRoleProjection> projectionLinks = FactTypeRoleProjection.GetLinksToProjectedRoleCollection(projection);
					projectedRoleCount = projectionLinks.Count;
					foreach (FactTypeRoleProjection roleProjection in projectionLinks)
					{
						if (null == roleProjection.ProjectedFromPathedRole &&
							null == roleProjection.ProjectedFromCalculatedValue &&
							null == roleProjection.ProjectedFromConstant)
						{
							--projectedRoleCount;
							roleProjection.Delete();
						}
					}
					if (projectedRoleCount == 0)
					{
						projection.Delete();
						continue;
					}
				}
				else
				{
					projectedRoleCount = projection.ProjectedRoleCollection.Count;
				}
				seenProjection = true;
				PartialFactTypeDerivationProjectionError partialProjectionError = projection.PartialProjectionError;
				if (projectedRoleCount < factTypeRoleCount)
				{
					if (partialProjectionError == null)
					{
						partialProjectionError = new PartialFactTypeDerivationProjectionError(store);
						partialProjectionError.DerivationProjection = projection;
						partialProjectionError.Model = model ?? (model = Model);
						partialProjectionError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(partialProjectionError, true);
						}
					}
				}
				else if (partialProjectionError != null)
				{
					partialProjectionError.Delete();
				}
			}
			FactTypeDerivationRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
			if (!seenProjection && !ExternalDerivation)
			{
				if (projectionRequiredError == null)
				{
					projectionRequiredError = new FactTypeDerivationRequiresProjectionError(store);
					projectionRequiredError.DerivationRule = this;
					projectionRequiredError.Model = model ?? (model = Model);
					projectionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(projectionRequiredError, true);
					}
				}
			}
			else if (projectionRequiredError != null)
			{
				projectionRequiredError.Delete();
			}
		}
		#endregion // Base overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				string factTypeName = null;
				string modelName = null;
				FactType factType = FactType;
				if (factType != null)
				{
					factTypeName = factType.Name;
					ORMModel model = factType.Model;
					if (model != null)
					{
						modelName = model.Name;
					}
				}
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextFactTypeDerivationRule, factTypeName ?? "", modelName ?? "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasDerivationRule.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				FactTypeDerivationRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
				if (projectionRequiredError != null)
				{
					yield return projectionRequiredError;
				}
				else
				{
					foreach (FactTypeDerivationProjection projection in FactTypeDerivationProjection.GetLinksToProjectedPathComponentCollection(this))
					{
						PartialFactTypeDerivationProjectionError partialProjectionError = projection.PartialProjectionError;
						if (partialProjectionError != null)
						{
							yield return partialProjectionError;
						}
					}
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// DeleteRule: typeof(FactTypeDerivationRuleHasDerivationNote), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void DerivationNoteDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeDerivationRule derivationRule = ((FactTypeDerivationRuleHasDerivationNote)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted &&
				derivationRule.ExternalDerivation &&
				derivationRule.PathComponentCollection.Count == 0)
			{
				derivationRule.Delete();
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationRule)
		/// </summary>
		private static void FactTypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ExternalDerivationDomainPropertyId)
			{
				// Projection errors with come and go based on this setting
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateProjections);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void FactTypeRoleProjectionOnCalculatedPathValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedValueUseChangedInRule(((FactTypeRoleProjectedFromCalculatedPathValue)e.ModelElement).Source);
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void FactTypeRoleProjectionOnCalculatedPathValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((FactTypeRoleProjectedFromCalculatedPathValue)e.ModelElement).Source;
			if (!calculation.IsDeleted)
			{
				CalculatedValueUseChangedInRule(calculation);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactTypeRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void FactTypeRoleProjectionOnCalculatedPathValueRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == FactTypeRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.OldRolePlayer);
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void FactTypeRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeDerivationRule derivationRule;
			if (null != (derivationRule = ((FactTypeHasRole)e.ModelElement).FactType.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void FactTypeRoleDeletedRule(ElementDeletedEventArgs e)
		{
			FactType factType = ((FactTypeHasRole)e.ModelElement).FactType;
			FactTypeDerivationRule derivationRule;
			if (!factType.IsDeleted &&
				null != (derivationRule = factType.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeDerivationProjection)
		/// </summary>
		private static void ProjectionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((FactTypeDerivationProjection)e.ModelElement).DerivationRule, DelayValidateProjections);
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeDerivationProjection)
		/// </summary>
		private static void ProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeDerivationRule derivationRule = ((FactTypeDerivationProjection)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactTypeDerivationProjection)
		/// </summary>
		private static void ProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == FactTypeDerivationProjection.DerivationRuleDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateProjections);
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateProjections);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(((FactTypeDerivationProjection)e.ElementLink).DerivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasPathComponent), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when elements are added to the path.
		/// </summary>
		private static void RolePathComponentAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasPathComponent link = (RolePathOwnerHasPathComponent)e.ModelElement;
			FactTypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as FactTypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeRoleProjection)
		/// </summary>
		private static void RoleProjectionAddedRule(ElementAddedEventArgs e)
		{
			FactTypeDerivationRule derivationRule = ((FactTypeRoleProjection)e.ModelElement).DerivationProjection.DerivationRule;
			if (derivationRule != null)
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjection)
		/// </summary>
		private static void RoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeDerivationProjection projection = ((FactTypeRoleProjection)e.ModelElement).DerivationProjection;
			FactTypeDerivationRule derivationRule;
			if (!projection.IsDeleted &&
				null != (derivationRule = projection.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactTypeRoleProjection)
		/// </summary>
		private static void RoleProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == FactTypeRoleProjection.DerivationProjectionDomainRoleId)
			{
				FactTypeDerivationRule derivationRule = ((FactTypeDerivationProjection)e.OldRolePlayer).DerivationRule;
				if (derivationRule != null)
				{
					FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
				}
				derivationRule = ((FactTypeDerivationProjection)e.NewRolePlayer).DerivationRule;
				if (derivationRule != null)
				{
					FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjections);
				}
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // FactTypeDerivationRule class
	#region FactTypeDerivationProjection class
	partial class FactTypeDerivationProjection : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeDerivationProjection.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the projection level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = DerivationRule;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // FactTypeDerivationProjection class
	#region FactTypeRoleProjection class
	partial class FactTypeRoleProjection
	{
		#region Role derivation validation rules
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjection), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a fact type derivation projection if there are no more contained role projections.
		/// </summary>
		private static void FactTypeRoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeDerivationProjection projection = ((FactTypeRoleProjection)e.ModelElement).DerivationProjection;
			if (!projection.IsDeleted &&
				projection.ProjectedRoleCollection.Count == 0)
			{
				projection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ProjectedFromCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			FactTypeRoleProjection roleProjection = ((FactTypeRoleProjectedFromCalculatedPathValue)e.ModelElement).RoleProjection;
			roleProjection.ProjectedFromConstant = null;
			roleProjection.ProjectedFromPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjectedFromCalculatedPathValue), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((FactTypeRoleProjectedFromCalculatedPathValue)e.ModelElement).RoleProjection);
		}
		private static void DeleteIfEmpty(FactTypeRoleProjection factTypeRoleProjection)
		{
			if (!factTypeRoleProjection.IsDeleted &&
				null == factTypeRoleProjection.ProjectedFromPathedRole &&
				null == factTypeRoleProjection.ProjectedFromCalculatedValue &&
				null == factTypeRoleProjection.ProjectedFromConstant)
			{
				factTypeRoleProjection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeRoleProjectedFromPathConstant)
		/// </summary>
		private static void ProjectedFromConstantAddedRule(ElementAddedEventArgs e)
		{
			FactTypeRoleProjection roleProjection = ((FactTypeRoleProjectedFromPathConstant)e.ModelElement).RoleProjection;
			roleProjection.ProjectedFromPathedRole = null;
			roleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjectedFromPathConstant), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromConstantDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((FactTypeRoleProjectedFromPathConstant)e.ModelElement).RoleProjection);
		}
		/// <summary>
		/// AddRule: typeof(FactTypeRoleProjectedFromPathedRole)
		/// </summary>
		private static void ProjectedFromPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeRoleProjection roleProjection = ((FactTypeRoleProjectedFromPathedRole)e.ModelElement).RoleProjection;
			roleProjection.ProjectedFromConstant = null;
			roleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeRoleProjectedFromPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((FactTypeRoleProjectedFromPathedRole)e.ModelElement).RoleProjection);
		}
		#endregion // Role derivation validation rules
	}
	#endregion // FactTypeRoleProjection class
	#region SubtypeDerivationRule class
	partial class SubtypeDerivationRule : IModelErrorDisplayContext, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="ObjectType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ObjectType objectType = Subtype;
				return objectType != null ? objectType.Model : null;
			}
		}
		#endregion // Base overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				string subTypeName = null;
				string modelName = null;
				ObjectType subType = Subtype;
				if (subType != null)
				{
					subTypeName = subType.Name;
					ORMModel model = subType.Model;
					if (model != null)
					{
						modelName = model.Name;
					}
				}
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSubtypeDerivationRule, subTypeName ?? "", modelName ?? "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { SubtypeHasDerivationRule.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// DeleteRule: typeof(SubtypeDerivationRuleHasDerivationNote), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void DerivationNoteDeletedRule(ElementDeletedEventArgs e)
		{
			SubtypeDerivationRule derivationRule = ((SubtypeDerivationRuleHasDerivationNote)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted &&
				derivationRule.ExternalDerivation &&
				derivationRule.PathComponentCollection.Count == 0)
			{
				derivationRule.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasPathComponent), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when elements are added to the path.
		/// </summary>
		private static void RolePathComponentAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasPathComponent link = (RolePathOwnerHasPathComponent)e.ModelElement;
			SubtypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as SubtypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // SubtypeDerivationRule class
	#region ConstraintRoleSequenceJoinPath class
	partial class ConstraintRoleSequenceJoinPath : IModelErrorDisplayContext, IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="ConstraintRoleSequence"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ConstraintRoleSequence roleSequence;
				IConstraint constraint;
				return (null != (roleSequence = RoleSequence) && null != (constraint = roleSequence.Constraint)) ? constraint.Model : null;
			}
		}
		/// <summary>
		/// Add role projections to the set of base elements than can consume a <see cref="CalculatedPathValue"/>
		/// </summary>
		protected override bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return base.IsCalculatedPathValueConsumed(calculation) || ConstraintRoleProjectedFromCalculatedPathValue.GetConstraintRoleProjections(calculation).Count != 0;
		}
		/// <summary>
		/// Register validation for a new <see cref="FactTypeDerivationRule"/>
		/// </summary>
		protected override void NewlyCreated()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateProjections);
		}
		/// <summary>
		/// Check projections
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback.</param>
		protected override void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			if (notifyAdded != null)
			{
				// We do all projection analysis independently after the deserialization request
				ValidateProjections(notifyAdded);
			}
		}
		/// <summary>
		/// Verify all projection errors
		/// </summary>
		/// <param name="element">A <see cref="ConstraintRoleSequenceJoinPath"/></param>
		private static void DelayValidateProjections(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((ConstraintRoleSequenceJoinPath)element).ValidateProjections(null);
			}
		}
		/// <summary>
		/// Validate projections for existence and completeness
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback. Perform extra
		/// structural checks if this is set.</param>
		private void ValidateProjections(INotifyElementAdded notifyAdded)
		{
			bool seenProjection = false;
			int constraintRoleCount = 0;
			Store store = Store;
			ORMModel model = null;
			foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				if (constraintRoleCount == 0)
				{
					constraintRoleCount = RoleSequence.RoleCollection.Count;
				}
				int projectedRoleCount;
				if (notifyAdded != null)
				{
					// Clean up projections with no source information on load. Empty
					// projections are automatically removed after deserialization, so
					// verify that we have a consistent state.
					ReadOnlyCollection<ConstraintRoleProjection> projectionLinks = ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection);
					projectedRoleCount = projectionLinks.Count;
					foreach (ConstraintRoleProjection roleProjection in projectionLinks)
					{
						if (null == roleProjection.ProjectedFromPathedRole &&
							null == roleProjection.ProjectedFromCalculatedValue &&
							null == roleProjection.ProjectedFromConstant)
						{
							--projectedRoleCount;
							roleProjection.Delete();
						}
					}
					if (projectedRoleCount == 0)
					{
						projection.Delete();
						continue;
					}
				}
				else
				{
					projectedRoleCount = projection.ProjectedRoleCollection.Count;
				}
				seenProjection = true;
				PartialConstraintRoleSequenceJoinPathProjectionError partialProjectionError = projection.PartialProjectionError;
				if (projectedRoleCount < constraintRoleCount)
				{
					if (partialProjectionError == null)
					{
						partialProjectionError = new PartialConstraintRoleSequenceJoinPathProjectionError(store);
						partialProjectionError.JoinPathProjection = projection;
						partialProjectionError.Model = model ?? (model = Model);
						partialProjectionError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(partialProjectionError, true);
						}
					}
				}
				else if (partialProjectionError != null)
				{
					partialProjectionError.Delete();
				}
			}
			ConstraintRoleSequenceJoinPathRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
			if (!seenProjection)
			{
				if (projectionRequiredError == null)
				{
					projectionRequiredError = new ConstraintRoleSequenceJoinPathRequiresProjectionError(store);
					projectionRequiredError.JoinPath = this;
					projectionRequiredError.Model = model ?? (model = Model);
					projectionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(projectionRequiredError, true);
					}
				}
			}
			else if (projectionRequiredError != null)
			{
				projectionRequiredError.Delete();
			}
		}
		#endregion // Base overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				ConstraintRoleSequence roleSequence = RoleSequence;
				SetConstraint setConstraint;
				SetComparisonConstraintRoleSequence setComparisonSequence;
				if (null != (setConstraint = roleSequence as SetConstraint))
				{
					ORMModel model = Model;
					return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetConstraintJoinPath, setConstraint.Name, model != null ? model.Name : "");
				}
				else if (null != (setComparisonSequence = roleSequence as SetComparisonConstraintRoleSequence))
				{
					string modelName = null;
					int sequenceNumber = 0;
					string constraintName = null;
					SetComparisonConstraint constraint = setComparisonSequence.ExternalConstraint;
					if (constraint != null)
					{
						constraintName = constraint.Name;
						ORMModel model = constraint.Model;
						if (model != null)
						{
							modelName = model.Name;
						}
						sequenceNumber = constraint.RoleSequenceCollection.IndexOf(setComparisonSequence) + 1;
					}
					return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetComparisonConstraintSequenceJoinPath, constraintName ?? "", modelName ?? "", sequenceNumber);
				}
				return "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ConstraintRoleSequenceJoinPathRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
				if (projectionRequiredError != null)
				{
					yield return projectionRequiredError;
				}
				else
				{
					foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(this))
					{
						PartialConstraintRoleSequenceJoinPathProjectionError partialProjectionError = projection.PartialProjectionError;
						if (partialProjectionError != null)
						{
							yield return partialProjectionError;
						}
					}
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath;
			if (null != (joinPath = ((ConstraintRoleSequenceHasRole)e.ModelElement).ConstraintRoleSequence.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequence roleSequence = ((ConstraintRoleSequenceHasRole)e.ModelElement).ConstraintRoleSequence;
			ConstraintRoleSequenceJoinPath joinPath;
			if (!roleSequence.IsDeleted &&
				null != (joinPath = roleSequence.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedValueUseChangedInRule(((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).Source);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).Source;
			if (!calculation.IsDeleted)
			{
				CalculatedValueUseChangedInRule(calculation);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.OldRolePlayer);
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ConstraintRoleSequenceJoinPathProjection)e.ModelElement).JoinPath, DelayValidateProjections);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.ModelElement).JoinPath;
			if (!joinPath.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateProjections);
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateProjections);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(((ConstraintRoleSequenceJoinPathProjection)e.ElementLink).JoinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleProjection)e.ModelElement).JoinPathProjection.JoinPath;
			if (joinPath != null)
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceJoinPathProjection projection = ((ConstraintRoleProjection)e.ModelElement).JoinPathProjection;
			ConstraintRoleSequenceJoinPath joinPath;
			if (!projection.IsDeleted &&
				null != (joinPath = projection.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleProjection.JoinPathProjectionDomainRoleId)
			{
				ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.OldRolePlayer).JoinPath;
				if (joinPath != null)
				{
					FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
				}
				joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.NewRolePlayer).JoinPath;
				if (joinPath != null)
				{
					FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjections);
				}
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // ConstraintRoleSequenceJoinPath class
	#region ConstraintRoleSequenceJoinPathProjection class
	partial class ConstraintRoleSequenceJoinPathProjection : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the projection level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = JoinPath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // ConstraintRoleSequenceJoinPathProjection class
	#region RoleSubPath class
	partial class RoleSubPath : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Recursive find the path root
		/// </summary>
		public override LeadRolePath RootRolePath
		{
			get
			{
				RoleSubPath subPath = this;
				RolePath parentPath = null;
				while (subPath != null)
				{
					parentPath = subPath.ParentRolePath;
					subPath = parentPath as RoleSubPath;
				}
				return parentPath as LeadRolePath;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // RoleSubPath class
	#region PathedRole class
	partial class PathedRole : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessor Properties
		/// <summary>
		/// Get the previous pathed role in either this or the
		/// containing role path.
		/// </summary>
		public PathedRole PreviousPathedRole
		{
			get
			{
				RolePath rolePath = RolePath;
				ReadOnlyCollection<PathedRole> steps = rolePath.PathedRoleCollection;
				int index = steps.IndexOf(this);
				switch (index)
				{
					case -1:
						break;
					case 0:
						RoleSubPath subPath;
						while (null != (subPath = rolePath as RoleSubPath) &&
							null != (rolePath = subPath.ParentRolePath))
						{
							steps = rolePath.PathedRoleCollection;
							int stepCount = steps.Count;
							if (stepCount != 0)
							{
								return steps[stepCount - 1];
							}
						}
						break;
					default:
						return steps[index - 1];
				}
				return null;
			}
		}
		/// <summary>
		/// A <see cref="PathedRole"/> can have a <see cref="PathConditionRoleValueConstraint"/>
		/// for each <see cref="RolePathCombination"/> containing the context <see cref="LeadRolePath"/>.
		/// DirectValueConstraint sets the value constraint for the native role path, not a path combination.
		/// A <see cref="Transaction"/> must be active to set this property.
		/// </summary>
		public PathConditionRoleValueConstraint DirectValueConstraint
		{
			get
			{
				foreach (PathConditionRoleValueConstraint valueConstraint in ValueConstraintCollection)
				{
					if (valueConstraint.AppliesToPathCombination == null)
					{
						return valueConstraint;
					}
				}
				return null;
			}
			set
			{
				PathConditionRoleValueConstraint existingValueConstraint = DirectValueConstraint;
				if (value == null)
				{
					if (existingValueConstraint != null)
					{
						existingValueConstraint.Delete();
					}
				}
				else if (existingValueConstraint != value)
				{
					// Make sure this is not used elsewhere
					PathConditionRoleValueConstraintAppliesToRolePathCombination combinationLink = PathConditionRoleValueConstraintAppliesToRolePathCombination.GetLinkToAppliesToPathCombination(value);
					if (combinationLink != null)
					{
						// Avoid delete propagation
						combinationLink.Delete(PathConditionRoleValueConstraintAppliesToRolePathCombination.ValueConstraintDomainRoleId);
					}
					PathedRoleHasValueConstraint existingLink = PathedRoleHasValueConstraint.GetLinkToPathedRole(value);
					if (existingLink != null)
					{
						if (existingLink.PathedRole == this)
						{
							return;
						}
						// Avoid delete propagation
						existingLink.Delete(PathedRoleHasValueConstraint.ValueConstraintDomainRoleId);
					}

					if (existingValueConstraint != null)
					{
						// Note that we could potentially do a role player change here, but there
						// aren't any rules in place to validate the change, so we delete and readd.
						existingValueConstraint.Delete();
					}
					new PathedRoleHasValueConstraint(this, value);
				}
			}
		}
		#endregion // Accessor Properties
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { PathedRole.RolePathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the pathed role level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = RolePath.RootRolePath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // PathedRole class
	#region Function class
	partial class Function : IModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Use the function name for string display
		/// </summary>
		public override string ToString()
		{
			string symbol = OperatorSymbol;
			return string.IsNullOrEmpty(symbol) ? Name : symbol;
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				FunctionDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
	}
	#endregion // Function class
	#region CalculatedPathValue class
	partial class CalculatedPathValue : IHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessors
		/// <summary>
		/// Get the context <see cref="ORMModel"/> for this calculation
		/// </summary>
		public ORMModel Model
		{
			get
			{
				RolePathComponent parent = PathComponent;
				return (parent != null) ? parent.Model : null;
			}
		}
		#endregion // Accessors
		#region Rule Methods
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// When an assigned function changes, map old inputs to new inputs based on
		/// parameter position. This preserves as much data as possible for the most
		/// common cases.
		/// </summary>
		private static void FunctionChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				CalculatedPathValueIsCalculatedWithFunction link = (CalculatedPathValueIsCalculatedWithFunction)e.ElementLink;
				CalculatedPathValue calculatedValue = link.CalculatedValue;
				LinkedElementCollection<FunctionParameter> oldParameters = ((Function)e.OldRolePlayer).ParameterCollection;
				Function newFunction = link.Function;
				LinkedElementCollection<FunctionParameter> newParameters = newFunction.ParameterCollection;
				int newParameterCount = newParameters.Count;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculatedValue.InputCollection;
				for (int i = inputs.Count - 1; i >= 0; --i)
				{
					CalculatedPathValueInput input = inputs[i];
					int oldParameterIndex = oldParameters.IndexOf(input.Parameter);
					FunctionParameter newParameter;
					if (oldParameterIndex != -1 &&
						oldParameterIndex < newParameterCount &&
						(!(newParameter = newParameters[oldParameterIndex]).BagInput ||
						input.SourceConstant == null))
					{
						input.Parameter = newParameters[oldParameterIndex];
					}
					else
					{
						input.Delete();
					}
				}
				if (!newFunction.IsBoolean)
				{
					foreach (RolePathComponentSatisfiesCalculatedCondition conditionLink in RolePathComponentSatisfiesCalculatedCondition.GetLinksToRequiredForPathComponentCollection(calculatedValue))
					{
						// A non-boolean function cannot be a path condition
						conditionLink.Delete();
					}
				}
			}
			else
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEnforceInitialCalculatedValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void FunctionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculatedValue = ((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue;
			if (!calculatedValue.IsDeleted)
			{
				calculatedValue.InputCollection.Clear();

				// If there is no function, then we cannot assume that it is a boolean function
				// that is eligible to satisfy a condition
				foreach (RolePathComponentSatisfiesCalculatedCondition conditionLink in RolePathComponentSatisfiesCalculatedCondition.GetLinksToRequiredForPathComponentCollection(calculatedValue))
				{
					conditionLink.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToCalculatedValueRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement).Input;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourcePathedRole = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToConstantRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input;
			FunctionParameter parameter;
			if (null != (parameter = calculatedInput.Parameter) &&
				parameter.BagInput)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionPathConstantInconsistentWithBagInput);
			}
			calculatedInput.SourcePathedRole = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToPathedRoleRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		#endregion // Rule Methods
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathComponentCalculatesCalculatedPathValue.CalculatedValueDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the calculation level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = PathComponent;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // CalculatedPathValue class
	#region Path Errors
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathRequiresRootObjectTypeError
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
			IModelErrorDisplayContext displayContext = LeadRolePath;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathRequiresRootObjectType, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathStartRoleFollowsRootObjectTypeError
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
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathStartRoleFollowsRootObjectType, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathSameFactTypeRoleFollowsJoinError
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
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathSameFactTypeRoleFollowsJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class JoinedPathRoleRequiresCompatibleRolePlayerError
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
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathIncompatibleJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CorrelatedPathRoleRequiresCompatibleRolePlayerError
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
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathIncompatibleCorrelation, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathOuterJoinRequiresOptionalRoleError
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
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture,ResourceStrings.ModelErrorRolePathMandatoryOuterJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueRequiresFunctionError
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
			IModelErrorDisplayContext displayContext = CalculatedPathValue;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueRequiresFunction, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueParameterBindingError
	{
		/// <summary>
		/// Get the associated <see cref="FunctionParameter"/>
		/// </summary>
		public FunctionParameter Parameter
		{
			get
			{
				// The function parameter is on the owning link to
				// assist with event handling.
				CalculatedPathValueHasUnboundParameterError errorLink = CalculatedPathValueHasUnboundParameterError.GetLinkToCalculatedPathValue(this);
				return errorLink != null ? errorLink.Parameter : null;
			}
		}
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
			CalculatedPathValueHasUnboundParameterError errorLink = CalculatedPathValueHasUnboundParameterError.GetLinkToCalculatedPathValue(this);
			IModelErrorDisplayContext displayContext = errorLink.CalculatedPathValue;
			FunctionParameter parameter = errorLink.Parameter;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueParameterBinding, displayContext != null ? displayContext.ErrorDisplayContext : "", parameter != null ? parameter.Name : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueMustBeConsumedError
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
			IModelErrorDisplayContext displayContext = CalculatedPathValue;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueMustBeConsumed, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class FactTypeDerivationRequiresProjectionError
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
			IModelErrorDisplayContext displayContext = DerivationRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFactTypeDerivationRuleProjectionRequired, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PartialFactTypeDerivationProjectionError
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
			IModelErrorDisplayContext displayContext = DerivationProjection;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFactTypeDerivationRulePartialProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class ConstraintRoleSequenceJoinPathRequiresProjectionError
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
			IModelErrorDisplayContext displayContext = JoinPath;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorJoinPathProjectionRequired, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PartialConstraintRoleSequenceJoinPathProjectionError
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
			IModelErrorDisplayContext displayContext = JoinPathProjection;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorJoinPathPartialProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
	}
	#endregion // Path Errors
	#region FactTypeDerivationExpression class (transitional)
	// Transitional code to move all information stored in the
	// old derivation expression elements into the new derivation
	// path elements. The old expression elements are removed when
	// the file is loaded.
	partial class FactTypeDerivationExpression
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DerivationRuleFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class DerivationRuleFixupListener : DeserializationFixupListener<FactTypeHasDerivationExpression>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public DerivationRuleFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Process FactTypeHasDerivationExpression elements
			/// </summary>
			/// <param name="element">An FactTypeHasDerivationExpression element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactTypeHasDerivationExpression element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					FactType factType = element.FactType;
					FactTypeDerivationRule derivationRule = factType.DerivationRule;
					FactTypeDerivationExpression derivationExpression = element.DerivationRule;
					string expressionBody = derivationExpression.Body;
					DerivationNote derivationNote;
					if (derivationRule != null)
					{
						derivationNote = derivationRule.DerivationNote;
					}
					else
					{
						notifyAdded.ElementAdded(derivationRule = new FactTypeDerivationRule(
							store,
							new PropertyAssignment(FactTypeDerivationRule.ExternalDerivationDomainPropertyId, true)));
						notifyAdded.ElementAdded(new FactTypeHasDerivationRule(factType, derivationRule));
						derivationNote = null;
					}

					// Expressions were around before rules, so we synchronize the two part rule values
					// with the expression storage settings.
					switch (derivationExpression.DerivationStorage)
					{
						case DerivationExpressionStorageType.Derived:
							derivationRule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
							break;
						case DerivationExpressionStorageType.DerivedAndStored:
							derivationRule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
							break;
						case DerivationExpressionStorageType.PartiallyDerived:
							derivationRule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
							break;
						case DerivationExpressionStorageType.PartiallyDerivedAndStored:
							derivationRule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
							break;
					}

					// Migrate settings from the expression body
					if (!string.IsNullOrEmpty(expressionBody))
					{
						if (derivationNote == null)
						{
							notifyAdded.ElementAdded(derivationNote = new DerivationNote(
								store,
								new PropertyAssignment(DerivationNote.BodyDomainPropertyId, expressionBody)));
							notifyAdded.ElementAdded(new FactTypeDerivationRuleHasDerivationNote(derivationRule, derivationNote));
						}
						else
						{
							string existingBody = derivationNote.Body;
							derivationNote.Body = string.IsNullOrEmpty(existingBody) ? expressionBody : existingBody + "\r\n" + expressionBody;
						}
					}

					// Remove the deprecated expression
					element.Delete();
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(FactTypeDerivationExpression)
		/// </summary>
		private static void DeprecateFactTypeDerivationExpression(ElementAddedEventArgs e)
		{
			// There is no need to localize this, we'll pull it altogether on the next file format upgrade.
			throw new InvalidOperationException("FactTypeDerivationExpression is deprecated. Use FactTypeDerivationRule instead.");
		}
		#endregion // Rule Methods
	}
	#endregion // FactTypeDerivationExpression class
	#region SubtypeDerivationExpression class (transitional)
	// Transitional code to move all information stored in the
	// old derivation expression elements into the new derivation
	// path elements. The old expression elements are removed when
	// the file is loaded.
	partial class SubtypeDerivationExpression
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DerivationRuleFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class DerivationRuleFixupListener : DeserializationFixupListener<SubtypeHasDerivationExpression>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public DerivationRuleFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Process SubtypeHasDerivationExpression elements
			/// </summary>
			/// <param name="element">An SubtypeHasDerivationExpression element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(SubtypeHasDerivationExpression element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					ObjectType subType = element.Subtype;
					SubtypeDerivationExpression derivationExpression = element.DerivationRule;
					string expressionBody = derivationExpression.Body;
					if (!string.IsNullOrEmpty(expressionBody))
					{
						SubtypeDerivationRule derivationRule = subType.DerivationRule;
						DerivationNote derivationNote;
						if (derivationRule != null)
						{
							derivationNote = derivationRule.DerivationNote;
						}
						else
						{
							notifyAdded.ElementAdded(derivationRule = new SubtypeDerivationRule(
								store,
								new PropertyAssignment(SubtypeDerivationRule.ExternalDerivationDomainPropertyId, true)));
							notifyAdded.ElementAdded(new SubtypeHasDerivationRule(subType, derivationRule));
							derivationNote = null;
						}
						if (derivationNote == null)
						{
							notifyAdded.ElementAdded(derivationNote = new DerivationNote(
								store,
								new PropertyAssignment(DerivationNote.BodyDomainPropertyId, expressionBody)));
							notifyAdded.ElementAdded(new SubtypeDerivationRuleHasDerivationNote(derivationRule, derivationNote));
						}
						else
						{
							string existingBody = derivationNote.Body;
							derivationNote.Body = string.IsNullOrEmpty(existingBody) ? expressionBody : existingBody + "\r\n" + expressionBody;
						}
					}

					// Remove the deprecated expression
					element.Delete();
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(SubtypeDerivationExpression)
		/// </summary>
		private static void DeprecateSubtypeDerivationExpression(ElementAddedEventArgs e)
		{
			// There is no need to localize this, we'll pull it altogether on the next file format upgrade.
			throw new InvalidOperationException("SubtypeDerivationExpression is deprecated. Use SubtypeDerivationRule instead.");
		}
		#endregion // Rule Methods
	}
	#endregion // SubtypeDerivationExpression class
	#region DerivationNote class
	partial class DerivationNote
	{
		#region Rule Methods
		/// <summary>
		/// ChangeRule: typeof(DerivationNote)
		/// Delete a <see cref="DerivationNote"/> if the <see cref="Expression.Body"/> property is empty.
		/// </summary>
		private static void DerivationNoteChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == DerivationNote.BodyDomainPropertyId &&
				string.IsNullOrEmpty((string)e.NewValue))
			{
				e.ModelElement.Delete();
			}
		}
		#endregion // Rule Methods
	}
	#endregion // DerivationNote class
}
