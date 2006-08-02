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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class Role : IModelErrorOwner, IRedirectVerbalization, IVerbalizeChildren, INamedElementDictionaryParent, INamedElementDictionaryRemoteParent, IHasIndirectModelErrorOwner
	{
		#region IndexOf helper method for LinkedElementCollection<RoleBase>
		/// <summary>
		/// Determines the index of a specific Role in the list, resolving
		/// RoleProxy elements as needed
		/// </summary>
		/// <param name="roleBaseCollection">The list in which to locate the role</param>
		/// <param name="value">The Role to locate in the list</param>
		/// <returns>index of object</returns>
		public static int IndexOf(LinkedElementCollection<RoleBase> roleBaseCollection, Role value)
		{
			int count = roleBaseCollection.Count;
			for (int i = 0; i < count; ++i)
			{
				if (roleBaseCollection[i].Role == value)
				{
					return i;
				}
			}
			return -1;
		}
		#endregion // IndexOf helper method for LinkedElementCollection<RoleBase>
		#region CustomStorage handlers
		#region CustomStorage setters
		private void SetRolePlayerDisplayValue(ObjectType newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetIsMandatoryValue(bool newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMandatoryConstraintNameValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMultiplicityValue(RoleMultiplicity newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetValueRangeTextValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMandatoryConstraintModalityValue(ConstraintModality newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetObjectificationOppositeRoleNameValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		#endregion // CustomStorage setters
		private RoleMultiplicity GetReverseMultiplicity(FactType factType, LinkedElementCollection<RoleBase> roles)
		{
			RoleMultiplicity retVal = RoleMultiplicity.Unspecified;
			bool haveMandatory = false;
			bool haveUniqueness = false;
			bool haveDoubleWideUniqueness = false;
			bool tooManyUniquenessConstraints = false;
			foreach (ConstraintRoleSequence roleSet in ConstraintRoleSequenceCollection)
			{
				IConstraint constraint = roleSet.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
						// Ignore multiple mandatories. Unlike
						// condition, and we ignore it in the IsMandatory
						// getter anyway.
						haveMandatory = true;
						break;
					case ConstraintType.InternalUniqueness:
						if (haveUniqueness)
						{
							tooManyUniquenessConstraints = true;
						}
						else
						{
							haveUniqueness = true;
							if (roleSet.RoleCollection.Count == 2)
							{
								haveDoubleWideUniqueness = true;
							}
						}
						break;
				}
				if (tooManyUniquenessConstraints)
				{
					break;
				}
			}
			if (tooManyUniquenessConstraints)
			{
				retVal = RoleMultiplicity.Indeterminate;
			}
			else if (!haveUniqueness)
			{
				bool haveOppositeUniqueness = false;
				Role oppositeRole = roles[0].Role;
				if (oppositeRole == this)
				{
					oppositeRole = roles[1].Role;
				}
				foreach (ConstraintRoleSequence roleSet in oppositeRole.ConstraintRoleSequenceCollection)
				{
					if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
					{
						haveOppositeUniqueness = true;
						break;
					}
				}
				if (haveOppositeUniqueness)
				{
					retVal = haveMandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
				}
			}
			else if (haveDoubleWideUniqueness)
			{
				retVal = haveMandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
			}
			else
			{
				retVal = haveMandatory ? RoleMultiplicity.ExactlyOne : RoleMultiplicity.ZeroToOne;
			}
			return retVal;
		}
		/// <summary>
		/// Gets the <see cref="MandatoryConstraint"/> associated with this <see cref="Role"/>, if any.
		/// </summary>
		public MandatoryConstraint SimpleMandatoryConstraint
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
					{
						return (MandatoryConstraint)constraint;
					}
				}
				return null;
			}
		}

		private ObjectType GetRolePlayerDisplayValue()
		{
			return RolePlayer;
		}
		private bool GetIsMandatoryValue()
		{
			return SimpleMandatoryConstraint != null;
		}
		private string GetMandatoryConstraintNameValue()
		{
			MandatoryConstraint smc = SimpleMandatoryConstraint;
			return (smc != null) ? smc.Name : String.Empty;
		}
		private RoleMultiplicity GetMultiplicityValue()
		{
			RoleMultiplicity retVal = RoleMultiplicity.Unspecified;
			FactType fact = FactType;
			if (fact != null)
			{
				LinkedElementCollection<RoleBase> roles = fact.RoleCollection;
				if (roles.Count == 2)
				{
					RoleBase oppositeRole = roles[0];
					if (oppositeRole == this)
					{
						oppositeRole = roles[1];
					}
					retVal = oppositeRole.Role.GetReverseMultiplicity(fact, roles);
				}
			}
			return retVal;
		}
		private string GetValueRangeTextValue()
		{
			RoleValueConstraint valueConstraint = ValueConstraint;
			return (valueConstraint != null) ? valueConstraint.Text : String.Empty;
		}
		private ConstraintModality GetMandatoryConstraintModalityValue()
		{
			MandatoryConstraint smc = SimpleMandatoryConstraint;
			return (smc != null) ? smc.Modality : ConstraintModality.Alethic;
		}
		private string GetObjectificationOppositeRoleNameValue()
		{
			RoleProxy roleProxy = Proxy;
			Role proxyOppositeRole;
			return (roleProxy != null && (proxyOppositeRole = roleProxy.OppositeRole as Role) != null) ? proxyOppositeRole.Name : String.Empty;
		}
		#endregion // CustomStorage handlers
		#region RoleChangeRule class
		[RuleOn(typeof(Role))] // ChangeRule
		private sealed class RoleChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// generating role property
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == Role.RolePlayerDisplayDomainPropertyId)
				{
					(e.ModelElement as Role).RolePlayer = e.NewValue as ObjectType;
				}
				else if (attributeGuid == Role.ValueRangeTextDomainPropertyId)
				{
					Role role = e.ModelElement as Role;
					RoleValueConstraint valueConstraint = role.ValueConstraint;
					if (valueConstraint == null)
					{
						role.ValueConstraint = valueConstraint = new RoleValueConstraint(role.Store);
					}
					valueConstraint.Text = (string)e.NewValue;
				}
				#region Handle IsMandatory property changes
				else if (attributeGuid == Role.IsMandatoryDomainPropertyId)
				{
					Role role = e.ModelElement as Role;
					if ((bool)e.NewValue)
					{
						// Add a mandatory constraint
						Store store = role.Store;
						FactType factType;
						if (null == (factType = role.FactType))
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionIsMandatoryRequiresAttachedFactType);
						}
						MandatoryConstraint.CreateSimpleMandatoryConstraint(role);
					}
					else
					{
						// Find and remove the mandatory constraint
						LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = role.ConstraintRoleSequenceCollection;
						int roleSequenceCount = constraintRoleSequences.Count;
						for (int i = roleSequenceCount - 1; i >= 0; --i) // The indices may change, go backwards
						{
							IConstraint constraint = constraintRoleSequences[i].Constraint;
							if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
							{
								(constraint as ModelElement).Delete();
								// Should only have one of these, but we might as well keep going
								// because any of them would make the property appear to be true
							}
						}
					}
				}
				#endregion // Handle IsMandatory property changes
				#region Handle MandatoryConstraintName property changes
				else if (attributeGuid == Role.MandatoryConstraintNameDomainPropertyId)
				{
					Role role = e.ModelElement as Role;
					MandatoryConstraint smc = role.SimpleMandatoryConstraint;
					if (smc != null)
					{
						smc.Name = (string)e.NewValue;
					}
				}
				#endregion // Handle MandatoryConstraintName property changes
				#region Handle Multiplicity property changes
				else if (attributeGuid == Role.MultiplicityDomainPropertyId)
				{
					RoleMultiplicity oldMultiplicity = (RoleMultiplicity)e.OldValue;
					RoleMultiplicity newMultiplicity = (RoleMultiplicity)e.NewValue;

					if (newMultiplicity != RoleMultiplicity.Unspecified && newMultiplicity != RoleMultiplicity.Indeterminate)
					{
						Role role = e.ModelElement as Role;
						FactType factType = role.FactType;
						LinkedElementCollection<RoleBase> factRoles = factType.RoleCollection;
						if (factType == null || factRoles.Count != 2)
						{
							return; // Ignore the request
						}

						// We implemented this backwards, so switch to the opposite role
						Role testRole = factRoles[0].Role;
						if (role == testRole)
						{
							role = factRoles[1].Role;
						}
						else
						{
							role = testRole;
						}

						// First take care of the mandatory setting. We
						// can deduce this setting directly from the multiplicity
						// values, so we don't touch the IsMandatory property unless
						// we really need to.
						bool newMandatory;
						switch (newMultiplicity)
						{
							case RoleMultiplicity.OneToMany:
							case RoleMultiplicity.ExactlyOne:
								newMandatory = true;
								break;
							default:
								newMandatory = false;
								break;
						}
						bool oldMandatory;
						switch (oldMultiplicity)
						{
							case RoleMultiplicity.Unspecified:
							case RoleMultiplicity.Indeterminate:
								oldMandatory = !newMandatory; // No data, for the property set
								break;
							case RoleMultiplicity.OneToMany:
							case RoleMultiplicity.ExactlyOne:
								oldMandatory = true;
								break;
							default:
								oldMandatory = false;
								break;
						}
						if (newMandatory ^ oldMandatory)
						{
							role.IsMandatory = newMandatory;
						}

						// Now take care of the one/many changes
						bool newOne;
						switch (newMultiplicity)
						{
							case RoleMultiplicity.ZeroToOne:
							case RoleMultiplicity.ExactlyOne:
								newOne = true;
								break;
							default:
								newOne = false;
								break;
						}
						bool oldOne;
						bool oldBroken = false;
						switch (oldMultiplicity)
						{
							case RoleMultiplicity.Unspecified:
								// Make sure we get into the main code
								oldOne = !newOne;
								break;
							case RoleMultiplicity.Indeterminate:
								oldBroken = oldOne = true;
								break;
							case RoleMultiplicity.ZeroToOne:
							case RoleMultiplicity.ExactlyOne:
								oldOne = true;
								break;
							default:
								oldOne = false;
								break;
						}
						if (oldBroken)
						{
							// If there are multiple uniqueness constraints, then remove
							// all but one. Prefer a single-role constraint to a double-role
							// constraint and pretend that our old value is a many.
							LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
							int roleSequenceCount = roleSequences.Count;
							// Go backwards so we can remove constraints
							IConstraint keepCandidate = null;
							int keepRoleMultiplicity = 0;
							bool keepCandidateIsPreferred = false;
							for (int i = roleSequenceCount - 1; i >= 0; --i) // The indices may change, go backwards
							{
								ConstraintRoleSequence roleSequence = roleSequences[i];
								IConstraint constraint = roleSequence.Constraint;
								if (constraint.ConstraintType == ConstraintType.InternalUniqueness)
								{
									int currentMultiplicity = roleSequence.RoleCollection.Count;
									if (keepCandidate == null)
									{
										keepCandidate = constraint;
										keepRoleMultiplicity = currentMultiplicity;
										if (currentMultiplicity == 1)
										{
											keepCandidateIsPreferred = (constraint as UniquenessConstraint).IsPreferred;
										}
									}
									else if (currentMultiplicity < keepRoleMultiplicity)
									{
										keepRoleMultiplicity = currentMultiplicity;
										(keepCandidate as ModelElement).Delete();
										keepCandidate = constraint;
									}
									else
									{
										// Keep a preferred over a non-preferred. Preferred
										// constraints always have a single role.
										if (!keepCandidateIsPreferred &&
											currentMultiplicity == 1 &&
											(constraint as UniquenessConstraint).IsPreferred)
										{
											(keepCandidate as ModelElement).Delete();
											keepCandidate = constraint;
											keepCandidateIsPreferred = true;
										}
										else
										{
											(constraint as ModelElement).Delete();
										}
									}
								}
							}
							if (keepRoleMultiplicity > 1)
							{
								oldOne = false;
							}
						}
						if (oldOne ^ newOne)
						{
							Store store = role.Store;
							if (newOne)
							{
								// We are considered a 'many' instead of a 'one' either
								// because there is no internal uniqueness constraint attached
								// to this role, or because there is a uniqueness constraint
								// attached to both roles. Figure out which one it is and remove
								// the spanning constraint. The other option is to remove the
								// role from the spanning constraint, but this leaves us with
								// a zero-to-one or 1-to-1 multiplicity on the opposite role,
								// which is a change from the current zero-to-many or one-to-many
								// multiplicity it currently has.
								foreach (ConstraintRoleSequence roleSequence in role.ConstraintRoleSequenceCollection)
								{
									IConstraint spanningConstraint = roleSequence.Constraint;
									if (spanningConstraint.ConstraintType == ConstraintType.InternalUniqueness)
									{
										Debug.Assert(roleSequence.RoleCollection.Count == 2);
										(spanningConstraint as ModelElement).Delete();
										// There will only be one of these because we
										// already fixed any 'broken' states earlier.
										break;
									}
								}

								// Now create a new uniqueness constraint containing only this role
								UniquenessConstraint.CreateInternalUniquenessConstraint(store).RoleCollection.Add(role);
							}
							else
							{
								bool oppositeHasUnique = false;
								bool wasUnspecified = oldMultiplicity == RoleMultiplicity.Unspecified;
								if (!wasUnspecified)
								{
									// Switch to a many by removing an internal uniqueness constraint from
									// this role. If the opposite role does not have an internal uniqueness constraint,
									// then we need to automatically create a uniqueness constraint that spans both
									// roles.
									foreach (ConstraintRoleSequence roleSequence in role.ConstraintRoleSequenceCollection)
									{
										IConstraint constraint = roleSequence.Constraint;
										if (constraint.ConstraintType == ConstraintType.InternalUniqueness)
										{
											Debug.Assert(roleSequence.RoleCollection.Count == 1);
											(constraint as ModelElement).Delete();
											break;
										}
									}
								}
								RoleBase oppositeBaseRole = factRoles[0];
								if (oppositeBaseRole.Role == role)
								{
									oppositeBaseRole = factRoles[1];
								}
								Role oppositeRole = oppositeBaseRole.Role;
								// Unspecified checks the opposite role before saying unspecified, no need to look
								if (!wasUnspecified)
								{
									foreach (ConstraintRoleSequence roleSequence in oppositeRole.ConstraintRoleSequenceCollection)
									{
										if (roleSequence.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
										{
											oppositeHasUnique = true;
											Debug.Assert(roleSequence.RoleCollection.Count == 1);
										}
									}
								}
								if (!oppositeHasUnique)
								{
									// Now create a new uniqueness constraint containing both roles
									LinkedElementCollection<Role> constraintRoles = UniquenessConstraint.CreateInternalUniquenessConstraint(factType).RoleCollection;
									constraintRoles.Add(role);
									constraintRoles.Add(oppositeRole);
								}
							}
						}
					}
				}
				#endregion // Handle Multiplicity property changes
				#region Handle MandatoryConstraintModality property changes
				else if (attributeGuid == Role.MandatoryConstraintModalityDomainPropertyId)
				{
					Role role = e.ModelElement as Role;
					MandatoryConstraint smc = role.SimpleMandatoryConstraint;
					if (smc != null)
					{
						smc.Modality = (ConstraintModality)e.NewValue;
					}
				}
				#endregion // Handle MandatoryConstraintModality property changes
				#region Handle ObjectificationOppositeRoleName property changes
				else if (attributeGuid == Role.ObjectificationOppositeRoleNameDomainPropertyId)
				{
					RoleProxy roleProxy = (e.ModelElement as Role).Proxy;
					Role oppositeRole;
					if (roleProxy != null && (oppositeRole = roleProxy.OppositeRole as Role) != null)
					{
						oppositeRole.Name = (string)e.NewValue;
					}
				}
				#endregion // Handle ObjectificationOppositeRoleName property changes
			}
		}
		#endregion // RoleChangeRule class
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				RolePlayerRequiredError requiredError;
				if (null != (requiredError = RolePlayerRequiredError))
				{
					yield return requiredError;
				}

			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			VerifyRolePlayerRequiredForRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new void DelayValidateErrors()
		{
			ORMCoreModel.DelayValidateElement(this, DelayValidateRolePlayerRequiredError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] {
					FactTypeHasRole.RoleDomainRoleId,
					ConstraintRoleSequenceHasRole.RoleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region RolePlayer validation rules
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)] // AddRule
		private sealed class RolePlayerRequiredAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				link.PlayedRole.VerifyRolePlayerRequiredForRule(null);
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))] // DeleteRule
		private sealed class RolePlayerRequiredDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				if (!role.IsDeleted)
				{
					ORMCoreModel.DelayValidateElement(role, DelayValidateRolePlayerRequiredError);
				}
			}
		}
		[RuleOn(typeof(FactTypeHasRole))] // AddRule
		private sealed class RolePlayerRequiredForNewRoleAddRule : AddRule
		{
			/// <summary>
			/// Verify that the role has a role player attached to it, and
			/// renumber other role player required error messages when roles are added
			/// and removed.
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				Role addedRole = link.Role as Role;
				if (addedRole != null)
				{
					ORMCoreModel.DelayValidateElement(addedRole, DelayValidateRolePlayerRequiredError);
					ORMCoreModel.DelayValidateElement(addedRole, DelayRenumberErrorsWithRoleNumbersAfterRole);
				}
			}
		}
		[RuleOn(typeof(FactTypeHasRole))] // DeleteRule
		private sealed class UpdatedRolePlayerRequiredErrorsDeleteRule : DeleteRule
		{
			/// <summary>
			/// Renumber role player required error messages when roles are added
			/// and removed.
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				if (!factType.IsDeleted)
				{
					ORMCoreModel.DelayValidateElement(factType, DelayRenumberErrorsWithRoleNumbers);
				}
			}
		}
		private static void DelayRenumberErrorsWithRoleNumbers(ModelElement element)
		{
			RenumberErrorsWithRoleNumbers((FactType)element, null);
		}
		private static void DelayRenumberErrorsWithRoleNumbersAfterRole(ModelElement element)
		{
			Role role = (Role)element;
			FactType fact;
			if (!role.IsDeleted &&
				(null != (fact = role.FactType)))
			{
				RenumberErrorsWithRoleNumbers(fact, role);
			}
		}
		/// <summary>
		/// The error message for role player required events includes the role number.
		/// If a role is added or deleted, then this numbering can change, so we need to
		/// regenerated the text.
		/// </summary>
		/// <param name="factType">The owning factType</param>
		/// <param name="roleAdded">The added role, or null if a role was removed.</param>
		private static void RenumberErrorsWithRoleNumbers(FactType factType, RoleBase roleAdded)
		{
			if (!factType.IsDeleted)
			{
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				bool regenerate = roleAdded == null;
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role currentRole = roles[i] as Role;
					if (regenerate && currentRole != null)
					{
						RolePlayerRequiredError error = currentRole.RolePlayerRequiredError;
						if (error != null)
						{
							error.GenerateErrorText();
						}
						RoleValueConstraint valueConstraint = currentRole.ValueConstraint;
						if (valueConstraint != null)
						{
							foreach (ValueRange range in valueConstraint.ValueRangeCollection)
							{
								MinValueMismatchError minError = range.MinValueMismatchError;
								if (minError != null)
								{
									minError.GenerateErrorText();
								}
								MaxValueMismatchError maxError = range.MaxValueMismatchError;
								if (maxError != null)
								{
									maxError.GenerateErrorText();
								}
							}
							ValueRangeOverlapError rangeOverlap = valueConstraint.ValueRangeOverlapError;
							if (rangeOverlap != null)
							{
								rangeOverlap.GenerateErrorText();
							}
						}
					}
					else if (roleAdded == currentRole)
					{
						// Regenerate on the next pass
						regenerate = true;
					}
				}
			}
		}
		/// <summary>
		/// Delayed validator for RolePlayerRequiredError
		/// </summary>
		private static void DelayValidateRolePlayerRequiredError(ModelElement element)
		{
			(element as Role).VerifyRolePlayerRequiredForRule(null);
		}
		/// <summary>
		/// Utility function to verify that a role player is present for all roles
		/// </summary>
		private void VerifyRolePlayerRequiredForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasRolePlayer = true;
				RolePlayerRequiredError rolePlayerRequired;

				if (null == RolePlayer)
				{
					FactType fact = FactType;
					// Don't show an error for roles on implied fact types,
					// this is controlled indirectly by the nested roles
					if (null != fact && null == fact.ImpliedByObjectification)
					{
						hasRolePlayer = false;
						if (null == RolePlayerRequiredError)
						{
							rolePlayerRequired = new RolePlayerRequiredError(Store);
							rolePlayerRequired.Role = this;
							rolePlayerRequired.Model = fact.Model;
							rolePlayerRequired.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(rolePlayerRequired, true);
							}
						}
					}
				}
				if (hasRolePlayer)
				{
					if (null != (rolePlayerRequired = RolePlayerRequiredError))
					{
						rolePlayerRequired.Delete();
					}
				}
			}
		}
		#endregion // RolePlayer validation rules
		#region IRedirectVerbalization Implementation
		/// <summary>
		/// Implements IRedirectVerbalization.SurrogateVerbalizer by deferring to the parent fact
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				return FactType as IVerbalize;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization Implementation
		#region INamedElementDictionaryParent implementation
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == RoleHasValueConstraint.RoleDomainRoleId)
			{
				FactType fact;
				ORMModel model;
				if ((null != (fact = FactType)) &&
					(null != (model = fact.Model)))
				{
					return ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
				}
			}
			return null;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetAllowDuplicateNamesContextKey(parentDomainRoleId, childDomainRoleId);
		}
		#endregion // INamedElementDictionaryParent implementation
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { RoleHasValueConstraint.RoleDomainRoleId };
		/// <summary>
		/// Implementation of INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles. Identifies
		/// this as a remote parent for the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <returns>Guid for the ValueTypeHasValueConstraint.ValueType role</returns>
		protected static Guid[] GetNamedElementDictionaryLinkRoles()
		{
			return myRemoteNamedElementDictionaryRoles;
		}
		Guid[] INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles()
		{
			return GetNamedElementDictionaryLinkRoles();
		}
		#endregion // INamedElementDictionaryRemoteParent implementation
	}
	public partial class RoleBase
	{
		/// <summary>
		/// Convert a RoleBase to a Role, resolving a proxy as needed
		/// </summary>
		public Role Role
		{
			get
			{
				Role retVal = this as Role;
				if (retVal == null)
				{
					RoleProxy proxy;
					if (null != (proxy = this as RoleProxy))
					{
						retVal = proxy.TargetRole;
					}
				}
				return retVal;
			}
		}
		/// <summary>
		/// Used as a shortcut to find the opposite RoleBase in a binary FactType.
		/// Returns null if the FactType is not binary.
		/// </summary>
		public RoleBase OppositeRole
		{
			get
			{
				// Only do this if it's a binary fact
				LinkedElementCollection<RoleBase> roles = this.FactType.RoleCollection;
				if (roles.Count == 2)
				{
					// loop over the collection and get the other role
					RoleBase oppositeRole = roles[0];
					if (oppositeRole == this)
					{
						return roles[1];
					}
					return oppositeRole;
				}
				else
				{
					return null;
				}
			}
		}
	}
	public partial class RolePlayerRequiredError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			string roleName = "";
			string factName = "";
			string modelName = "";
			Role role = Role;
			if (role != null)
			{
				FactType fact = role.FactType;
				if (fact != null)
				{
					factName = fact.Name;
					roleName = (fact.RoleCollection.IndexOf(role) + 1).ToString(CultureInfo.InvariantCulture);
				}
			}
			ORMModel model = Model;
			if (model != null)
			{
				modelName = model.Name;
			}
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePlayerRequiredError, roleName, factName, modelName);
			string currentText = Name;
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { Role };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
}
