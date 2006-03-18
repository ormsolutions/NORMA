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
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region RoleMultiplicity enum
	/// <summary>
	/// Define the multiplicity for the roles. The role
	/// multiplicity is currently displayed only on roles
	/// associated with binary fact types and is calculated
	/// based on the existing mandatory and internal uniqueness
	/// constraints associated with the fact.
	/// </summary>
	[CLSCompliant(true)]
	public enum RoleMultiplicity
	{
		/// <summary>
		/// Insufficient constraints are present to
		/// determine the user intention.
		/// </summary>
		Unspecified,
		/// <summary>
		/// Too many constraints are present to determine
		/// the user intention.
		/// </summary>
		Indeterminate,
		/// <summary>
		/// 0...1
		/// </summary>
		ZeroToOne,
		/// <summary>
		/// 0...n
		/// </summary>
		ZeroToMany,
		/// <summary>
		/// 1
		/// </summary>
		ExactlyOne,
		/// <summary>
		/// 1...n
		/// </summary>
		OneToMany,
	}
	#endregion // RoleMultiplicity enum
	public partial class Role : IModelErrorOwner, IRedirectVerbalization, IVerbalizeChildren, INamedElementDictionaryParent, INamedElementDictionaryRemoteParent, IHasIndirectModelErrorOwner
	{
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in RoleChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == RolePlayerDisplayMetaAttributeGuid ||
				attributeGuid == IsMandatoryMetaAttributeGuid ||
				attributeGuid == MandatoryConstraintNameMetaAttributeGuid ||
				attributeGuid == MultiplicityMetaAttributeGuid ||
				attributeGuid == ValueRangeTextMetaAttributeGuid ||
				attributeGuid == MandatoryConstraintModalityMetaAttributeGuid)
			{
				// Handled by RoleChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		private RoleMultiplicity GetReverseMultiplicity(FactType factType, RoleMoveableCollection roles)
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
				Role oppositeRole = roles[0];
				if (object.ReferenceEquals(oppositeRole, this))
				{
					oppositeRole = roles[1];
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
		/// Retrieve the mandatory constraint associated with this role, if any
		/// </summary>
		private SimpleMandatoryConstraint SimpleMandatoryConstraint
		{
			get
			{
				ConstraintRoleSequenceMoveableCollection constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
					{
						return (SimpleMandatoryConstraint)constraint;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == RolePlayerDisplayMetaAttributeGuid)
			{
				return RolePlayer;
			}
			else if (attributeGuid == IsMandatoryMetaAttributeGuid)
			{
				return SimpleMandatoryConstraint != null;
			}
			else if (attributeGuid == MandatoryConstraintNameMetaAttributeGuid)
			{
				SimpleMandatoryConstraint smc = SimpleMandatoryConstraint;
				return (smc != null) ? smc.Name : "";
			}
			else if (attributeGuid == MultiplicityMetaAttributeGuid)
			{
				RoleMultiplicity retVal = RoleMultiplicity.Unspecified;
				FactType fact = FactType;
				if (fact != null)
				{
					RoleMoveableCollection roles = fact.RoleCollection;
					if (roles.Count == 2)
					{
						Role oppositeRole = roles[0];
						if (object.ReferenceEquals(oppositeRole, this))
						{
							oppositeRole = roles[1];
						}
						retVal = oppositeRole.GetReverseMultiplicity(fact, roles);
					}
				}
				return retVal;
			}
			else if (attributeGuid == ValueRangeTextMetaAttributeGuid)
			{
				RoleValueConstraint valueConstraint = ValueConstraint;
				return (valueConstraint == null) ? "" : valueConstraint.Text;
			}
			#region MandatoryConstraintModality
			else if (attributeGuid == MandatoryConstraintModalityMetaAttributeGuid)
			{
				SimpleMandatoryConstraint smc = SimpleMandatoryConstraint;
				return (smc != null) ? smc.Modality : ConstraintModality.Alethic;
			}
			#endregion // MandatoryConstraintModality
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override determine when derived attributes are
		/// displayed in the property grid. Called for all attributes.
		/// </summary>
		/// <param name="metaAttrInfo">MetaAttributeInfo</param>
		/// <returns></returns>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeGuid = metaAttrInfo.Id;
			if (attributeGuid == MultiplicityMetaAttributeGuid)
			{
				FactType fact = FactType;
				// Display for binary fact types
				return fact != null && fact.RoleCollection.Count == 2;
			}
			else if (attributeGuid == MandatoryConstraintNameMetaAttributeGuid)
			{
				return SimpleMandatoryConstraint != null;
			}
			#region MandatoryConstraintModality
			else if (attributeGuid == MandatoryConstraintModalityMetaAttributeGuid)
			{
				return SimpleMandatoryConstraint != null;
			}
			#endregion // MandatoryConstraintModality
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		/// <summary>
		/// Standard override. Determines when derived properties are read-only. Called
		/// if the ReadOnly setting on the element is one of the SometimesUIReadOnly* values.
		/// Currently, ValueRangeText is readonly if the role player is an entity type without
		/// a reference mode.
		/// </summary>
		/// <param name="propertyDescriptor">PropertyDescriptor</param>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor elemDesc = propertyDescriptor as ElementPropertyDescriptor;
			if (elemDesc != null && elemDesc.MetaAttributeInfo.Id == ValueRangeTextMetaAttributeGuid)
			{
				bool readOnly = true;
				FactType fact = this.FactType;
				ObjectType rolePlayer = RolePlayer;
				if (fact != null && rolePlayer != null)
				{
					readOnly = !(rolePlayer.IsValueType || rolePlayer.HasReferenceMode);
				}
				return readOnly;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // CustomStorage handlers
		#region Multiplicity Display
		/// <summary>
		/// Override to create a custom property descriptor for
		/// multiplicity that does not include the Indeterminate
		/// and Unspecified values in the dropdown, unless either
		/// of these is the current value.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes)
		{
			if (metaAttributeInfo.Id == MultiplicityMetaAttributeGuid)
			{
				return new MultiplicityPropertyDescriptor(modelElement, metaAttributeInfo, requestor, attributes);
			}
			return base.CreatePropertyDescriptor(modelElement, metaAttributeInfo, requestor, attributes);
		}
		/// <summary>
		/// A property descriptor that filters out some standard values from
		/// the type converter.
		/// </summary>
		protected class MultiplicityPropertyDescriptor : ElementPropertyDescriptor
		{
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="modelElement">Passed to base</param>
			/// <param name="metaAttributeInfo">Passed to base</param>
			/// <param name="requestor">Passed to base</param>
			/// <param name="attributes">Passed to base</param>
			public MultiplicityPropertyDescriptor(ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes) : base(modelElement, metaAttributeInfo, requestor, attributes)
			{
			}
			/// <summary>
			/// Return a custom typeconverter that
			/// limits the predefined values.
			/// </summary>
			/// <value></value>
			public override TypeConverter Converter
			{
				get
				{
					return new FilteredMultiplicityConverter((EnumerationDomain)MetaAttributeInfo.Domain);
				}
			}
			private class FilteredMultiplicityConverter : ModelingEnumerationConverter
			{
				public FilteredMultiplicityConverter(EnumerationDomain domain) : base(domain)
				{
				}
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					// We look at the role multiplicity and modify the collection for StandardValuesCollection
					// so double-clicking the properties list will work when the role multiplicity is unspecified
					// or indeterminate.
					Role role = (Role)Editors.EditorUtility.ResolveContextInstance(context.Instance, false);
					RoleMultiplicity[] roles;
					switch (role.Multiplicity)
					{
						case RoleMultiplicity.Unspecified:
							roles = new RoleMultiplicity[] { RoleMultiplicity.Unspecified, RoleMultiplicity.ZeroToOne, RoleMultiplicity.ZeroToMany, RoleMultiplicity.ExactlyOne, RoleMultiplicity.OneToMany };
							break;
						case RoleMultiplicity.Indeterminate:
							roles = new RoleMultiplicity[] { RoleMultiplicity.Indeterminate, RoleMultiplicity.ZeroToOne, RoleMultiplicity.ZeroToMany, RoleMultiplicity.ExactlyOne, RoleMultiplicity.OneToMany };
							break;
						default:
							roles = new RoleMultiplicity[] { RoleMultiplicity.ZeroToOne, RoleMultiplicity.ZeroToMany, RoleMultiplicity.ExactlyOne, RoleMultiplicity.OneToMany };
							break;
					}
					return new StandardValuesCollection(roles);
				}
			}
		}
		#endregion // Multiplicity Display
		#region Properties
		/// <summary>
		/// Used as a shortcut to find the opposite role in a binary fact.
		/// Returns null if the fact is not a binary
		/// </summary>
		/// <value></value>
		public Role OppositeRole
		{
			get
			{
				// Only do this if it's a binary fact
				RoleMoveableCollection roles = this.FactType.RoleCollection;
				if (roles.Count == 2)
				{
					// loop over the collection and get the other role
					Role oppositeRole = null;
					foreach (Role r in roles)
					{
						if (!r.Equals(this))
						{
							oppositeRole = r;
							break;
						}
					}
					return oppositeRole;
				}
				else
				{
					return null;
				}
			}
		}
		#endregion
		#region RoleChangeRule class
		[RuleOn(typeof(Role))]
		private class RoleChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// generating role property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == Role.RolePlayerDisplayMetaAttributeGuid)
				{
					(e.ModelElement as Role).RolePlayer = e.NewValue as ObjectType;
				}
				else if (attributeGuid == Role.ValueRangeTextMetaAttributeGuid)
				{
					Role role = e.ModelElement as Role;
					RoleValueConstraint valueConstraint = role.ValueConstraint;
					if (valueConstraint == null)
					{
						role.ValueConstraint = valueConstraint = RoleValueConstraint.CreateRoleValueConstraint(role.Store);
					}
					valueConstraint.Text = (string)e.NewValue;
				}
				#region Handle IsMandatory attribute changes
				else if (attributeGuid == Role.IsMandatoryMetaAttributeGuid)
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
						InternalConstraint constraint = SimpleMandatoryConstraint.CreateSimpleMandatoryConstraint(store);
						constraint.RoleCollection.Add(role); // Automatically sets FactType, setting it again will remove and delete the new constraint
					}
					else
					{
						// Find and remove the mandatory constraint
						ConstraintRoleSequenceMoveableCollection constraintRoleSequences = role.ConstraintRoleSequenceCollection;
						int roleSequenceCount = constraintRoleSequences.Count;
						for (int i = roleSequenceCount - 1; i >= 0; --i) // The indices may change, go backwards
						{
							IConstraint constraint = constraintRoleSequences[i].Constraint;
							if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
							{
								(constraint as ModelElement).Remove();
								// Should only have one of these, but we might as well keep going
								// because any of them would make the property appear to be true
							}
						}
					}
				}
				#endregion // Handle IsMandatory attribute changes
				#region Handle MandatoryConstraintName attribute changes
				else if (attributeGuid == Role.MandatoryConstraintNameMetaAttributeGuid)
				{
					Role role = e.ModelElement as Role;
					SimpleMandatoryConstraint smc = role.SimpleMandatoryConstraint;
					if (smc != null)
					{
						smc.Name = (string)e.NewValue;
					}
				}
				#endregion // Handle MandatoryConstraintName attribute changes
				#region Handle Multiplicity attribute changes
				else if (attributeGuid == Role.MultiplicityMetaAttributeGuid)
				{
					RoleMultiplicity oldMultiplicity = (RoleMultiplicity)e.OldValue;
					RoleMultiplicity newMultiplicity = (RoleMultiplicity)e.NewValue;

					if (newMultiplicity != RoleMultiplicity.Unspecified && newMultiplicity != RoleMultiplicity.Indeterminate)
					{
						Role role = e.ModelElement as Role;
						FactType factType = role.FactType;
						RoleMoveableCollection factRoles = factType.RoleCollection;
						if (factType == null || factRoles.Count != 2)
						{
							return; // Ignore the request
						}

						// We implemented this backwards, so switch to the opposite role
						if (object.ReferenceEquals(role, factRoles[0]))
						{
							role = factRoles[1];
						}
						else
						{
							role = factRoles[0];
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
							ConstraintRoleSequenceMoveableCollection roleSequences = role.ConstraintRoleSequenceCollection;
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
											keepCandidateIsPreferred = (constraint as InternalUniquenessConstraint).IsPreferred;
										}
									}
									else if (currentMultiplicity < keepRoleMultiplicity)
									{
										keepRoleMultiplicity = currentMultiplicity;
										(keepCandidate as ModelElement).Remove();
										keepCandidate = constraint;
									}
									else
									{
										// Keep a preferred over a non-preferred. Preferred
										// constraints always have a single role.
										if (!keepCandidateIsPreferred &&
											currentMultiplicity == 1 &&
											(constraint as InternalUniquenessConstraint).IsPreferred)
										{
											(keepCandidate as ModelElement).Remove();
											keepCandidate = constraint;
											keepCandidateIsPreferred = true;
										}
										else
										{
											(constraint as ModelElement).Remove();
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
										(spanningConstraint as ModelElement).Remove();
										// There will only be one of these because we
										// already fixed any 'broken' states earlier.
										break;
									}
								}

								// Now create a new uniqueness constraint containing only this role
								InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
								iuc.RoleCollection.Add(role);  // Automatically sets FactType, setting it again will remove and delete the new constraint
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
											(constraint as ModelElement).Remove();
											break;
										}
									}
								}
								Role oppositeRole = factRoles[0];
								if (object.ReferenceEquals(oppositeRole, role))
								{
									oppositeRole = factRoles[1];
								}
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
									InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
									iuc.FactType = factType;
									RoleMoveableCollection constraintRoles = iuc.RoleCollection;
									constraintRoles.Add(role);
									constraintRoles.Add(oppositeRole);
								}
							}
						}
					}
				}
				#endregion // Handle Multiplicity attribute changes
				#region Handle MandatoryConstraintModality attribute changes
				else if (attributeGuid == Role.MandatoryConstraintModalityMetaAttributeGuid)
				{
					Role role = e.ModelElement as Role;
					SimpleMandatoryConstraint smc = role.SimpleMandatoryConstraint;
					if (smc != null)
					{
						smc.Modality = (ConstraintModality)e.NewValue;
					}
				}
				#endregion // Handle MandatoryConstraintModality attribute changes
			}
		}
		#endregion // RoleChangeRule class
		#region IModelErrorOwner Implementation
		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ErrorCollection
		/// </summary>
		protected new IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				RolePlayerRequiredError requiredError;
				if (null != (requiredError = RolePlayerRequiredError))
				{
					yield return requiredError;
				}
				// Get errors off the base
				foreach (ModelError baseError in base.ErrorCollection)
				{
					yield return baseError;
				}
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected  new void ValidateErrors(INotifyElementAdded notifyAdded)
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
		protected static new void DelayValidateErrors()
		{
			// UNDONE: DelayedValidation (Role)
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static readonly Guid[] myIndirectModelErrorOwnerLinkRoles = new Guid[] { FactTypeHasRole.RoleCollectionMetaRoleGuid };
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			return myIndirectModelErrorOwnerLinkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region RolePlayer validation rules
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class RolePlayerRequiredAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				link.PlayedRoleCollection.VerifyRolePlayerRequiredForRule(null);
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class RolePlayerRequiredRemovedRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				link.PlayedRoleCollection.VerifyRolePlayerRequiredForRule(null);
			}
		}
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class RolePlayerRequiredForNewRoleAddRule : AddRule
		{
			/// <summary>
			/// Verify that the role has a role player attached to it, and
			/// renumber other role player required error messages when roles are added
			/// and removed.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				Role addedRole = link.RoleCollection;
				addedRole.VerifyRolePlayerRequiredForRule(null);
				RenumberErrorsWithRoleNumbers(link.FactType, addedRole);
			}
		}
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class UpdatedRolePlayerRequiredErrorsRemovedRule : RemoveRule
		{
			/// <summary>
			/// Renumber role player required error messages when roles are added
			/// and removed.
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				RenumberErrorsWithRoleNumbers(factType, null);
			}
		}
		/// <summary>
		/// The error message for role player required events includes the role number.
		/// If a role is added or deleted, then this numbering can change, so we need to
		/// regenerated the text.
		/// </summary>
		/// <param name="factType">The owning factType</param>
		/// <param name="roleAdded">The added role, or null if a role was removed.</param>
		private static void RenumberErrorsWithRoleNumbers(FactType factType, Role roleAdded)
		{
			if (!factType.IsRemoved)
			{
				RoleMoveableCollection roles = factType.RoleCollection;
				bool regenerate = roleAdded == null;
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role currentRole = roles[i];
					if (regenerate)
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
		/// Utility function to verify that a role player is present for all roles
		/// </summary>
		private void VerifyRolePlayerRequiredForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
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
							rolePlayerRequired = RolePlayerRequiredError.CreateRolePlayerRequiredError(Store);
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
						rolePlayerRequired.Remove();
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
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			if (parentMetaRoleGuid == RoleHasValueConstraint.RoleMetaRoleGuid)
			{
				FactType fact;
				ORMModel model;
				if ((null != (fact = FactType)) &&
					(null != (model = fact.Model)))
				{
					return ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
				}
			}
			return null;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetAllowDuplicateNamesContextKey(parentMetaRoleGuid, childMetaRoleGuid);
		}
		#endregion // INamedElementDictionaryParent implementation
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { RoleHasValueConstraint.RoleMetaRoleGuid };
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
