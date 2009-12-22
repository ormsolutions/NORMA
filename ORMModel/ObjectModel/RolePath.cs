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
			if (!rolePath.IsDeleted &&
				rolePath.PathedRoleCollection.Count == 0 &&
				rolePath.SplitPathCollection.Count == 0)
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
				if (!rolePath.IsDeleted &&
					rolePath.PathedRoleCollection.Count == 0 &&
					rolePath.SplitPathCollection.Count == 0)
				{
					rolePath.Delete();
				}
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
				if (parentRolePath.PathedRoleCollection.Count == 0 &&
					parentRolePath.SplitPathCollection.Count == 0)
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
					if (parentRolePath.PathedRoleCollection.Count == 0 &&
						parentRolePath.SplitPathCollection.Count == 0)
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
	partial class RolePathComponent
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
			foreach (RolePathComponent pathComponent in PathComponentCollection)
			{
				LeadRolePath rolePath = pathComponent as LeadRolePath;
				if (rolePath != null)
				{
					VisitPathedRoles(
						rolePath,
						delegate(PathedRole pathedRole)
						{
							foreach (ValueConstraint valueConstraint in pathedRole.ValueConstraintCollection)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}
						});
				}
			}
			if (errors != null)
			{
				foreach (ModelErrorUsage errorUsage in errors)
				{
					yield return errorUsage;
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
		/// AddRule: typeof(RolePathOwnerHasPathComponent)
		/// See if the SingleLeadRolePath has changed
		/// </summary>
		private static void RolePathComponentAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((RolePathOwnerHasPathComponent)e.ModelElement).PathOwner, DelayValidateSingleLeadRolePath);
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
		#endregion // Rule Methods
	}
	#endregion // RolePathOwner class
	#region FactTypeDerivationRule class
	partial class FactTypeDerivationRule : IModelErrorDisplayContext, IHasIndirectModelErrorOwner
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
	}
	#endregion // FactTypeDerivationRule class
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
	}
	#endregion // SubtypeDerivationRule class
	#region ConstraintRoleSequenceJoinPath class
	partial class ConstraintRoleSequenceJoinPath : IModelErrorDisplayContext, IHasIndirectModelErrorOwner
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
	}
	#endregion // ConstraintRoleSequenceJoinPath class
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
	partial class PathedRole : IElementLinkRoleHasIndirectModelErrorOwner
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
	partial class CalculatedPathValue
	{
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
					foreach (RolePathComponentSatisfiesCalculatedCondition conditionLink in RolePathComponentSatisfiesCalculatedCondition.GetLinksToRequiredForPathCollection(calculatedValue))
					{
						// A non-boolean function cannot be path condition
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
				foreach (RolePathComponentSatisfiesCalculatedCondition conditionLink in RolePathComponentSatisfiesCalculatedCondition.GetLinksToRequiredForPathCollection(calculatedValue))
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
	}
	#endregion // CalculatedPathValue class
	#region FactTypeDerivationExpression class (transitional)
	// Transitional code to synchronize derivation storage settings on
	// the old (expression) and new (path) derivation mechanisms.
	partial class FactTypeDerivationExpression
	{
		#region Helper Methods
		private static void SynchronizeExpression(FactTypeDerivationExpression expression, DerivationCompleteness completeness, DerivationStorage storage)
		{
			switch (completeness)
			{
				case DerivationCompleteness.FullyDerived:
					expression.DerivationStorage = (storage == ObjectModel.DerivationStorage.Stored) ? DerivationExpressionStorageType.DerivedAndStored : DerivationExpressionStorageType.Derived;
					break;
				case DerivationCompleteness.PartiallyDerived:
					expression.DerivationStorage = (storage == ObjectModel.DerivationStorage.Stored) ? DerivationExpressionStorageType.PartiallyDerivedAndStored : DerivationExpressionStorageType.PartiallyDerived;
					break;
			}
		}
		private static void SynchronizeRule(FactTypeDerivationRule rule, DerivationExpressionStorageType storageType)
		{
			switch (storageType)
			{
				case DerivationExpressionStorageType.Derived:
					rule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
					break;
				case DerivationExpressionStorageType.DerivedAndStored:
					rule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
					break;
				case DerivationExpressionStorageType.PartiallyDerived:
					rule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
					break;
				case DerivationExpressionStorageType.PartiallyDerivedAndStored:
					rule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
					break;
			}
		}
		#endregion // Helper Methods
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
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
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
				FactTypeDerivationRule rule;
				if (!element.IsDeleted &&
					null != (rule = element.FactType.DerivationRule))
				{
					// Expressions were around before rules, synchronize rules with
					// the expression storage.
					SynchronizeRule(rule, element.DerivationRule.DerivationStorage);
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(FactTypeHasDerivationExpression)
		/// </summary>
		private static void DerivationExpressionAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasDerivationExpression link = (FactTypeHasDerivationExpression)e.ModelElement;
			FactTypeDerivationRule rule;
			if (null != (rule = link.FactType.DerivationRule))
			{
				SynchronizeExpression(link.DerivationRule, rule.DerivationCompleteness, rule.DerivationStorage);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationExpression)
		/// </summary>
		private static void DerivationExpressionChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactTypeDerivationExpression expression = (FactTypeDerivationExpression)e.ModelElement;
			FactType factType;
			FactTypeDerivationRule rule;
			if (null != (factType = expression.FactType) &&
				null != (rule = factType.DerivationRule))
			{
				SynchronizeRule(rule, expression.DerivationStorage);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasDerivationRule)
		/// </summary>
		private static void DerivationRuleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasDerivationRule link = (FactTypeHasDerivationRule)e.ModelElement;
			FactTypeDerivationExpression expression;
			if (null != (expression = link.FactType.DerivationExpression))
			{
				SynchronizeRule(link.DerivationRule, expression.DerivationStorage);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationRule)
		/// </summary>
		private static void DerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactTypeDerivationRule rule = (FactTypeDerivationRule)e.ModelElement;
			FactType factType;
			FactTypeDerivationExpression expression;
			if (null != (factType = rule.FactType) &&
				null != (expression = factType.DerivationExpression))
			{
				SynchronizeExpression(expression, rule.DerivationCompleteness, rule.DerivationStorage);
			}
		}
		#endregion // Rule Methods
	}
	#endregion // FactTypeDerivationExpression class
}
