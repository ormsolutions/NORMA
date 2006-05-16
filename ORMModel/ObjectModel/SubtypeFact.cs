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
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class SubtypeFact
	{
		#region Create functions
		/// <summary>
		/// Set the derived type as a subtype of the base type
		/// </summary>
		/// <param name="subtype">The object type to use as the subtype (base type)</param>
		/// <param name="supertype">The object type to use as the supertype (derived type)</param>
		/// <returns>Subtype object</returns>
		public static SubtypeFact Create(ObjectType subtype, ObjectType supertype)
		{
			Debug.Assert(subtype != null && supertype != null);
			SubtypeFact retVal = SubtypeFact.CreateSubtypeFact(subtype.Store);
			retVal.Model = subtype.Model;
			retVal.Subtype = subtype;
			retVal.Supertype = supertype;
			return retVal;
		}

		#endregion // Create functions
		#region Accessor functions
		/// <summary>
		/// Get the subtype for this relationship
		/// </summary>
		public ObjectType Subtype
		{
			get
			{
				Role role = SubtypeRole;
				return (role != null) ? role.RolePlayer : null;
			}
			set
			{
				SubtypeRole.RolePlayer = value;
			}
		}
		/// <summary>
		/// Get the super type for this relationship
		/// </summary>
		public ObjectType Supertype
		{
			get
			{
				Role role = SupertypeRole;
				return (role != null) ? role.RolePlayer : null;
			}
			set
			{
				SupertypeRole.RolePlayer = value;
			}
		}
		/// <summary>
		/// Get the Role attached to the subtype object
		/// </summary>
		public SubtypeMetaRole SubtypeRole
		{
			get
			{
				RoleBaseMoveableCollection roles = RoleCollection;
				SubtypeMetaRole retVal = null;
				if (roles.Count == 2)
				{
					retVal = roles[0] as SubtypeMetaRole;
					if (retVal == null)
					{
						retVal = roles[1] as SubtypeMetaRole;
						Debug.Assert(retVal != null); // One of them better be a subtype
					}
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the Role attached to the supertype object
		/// </summary>
		public SupertypeMetaRole SupertypeRole
		{
			get
			{
				RoleBaseMoveableCollection roles = RoleCollection;
				// Start with checking role 1, not 0. This corresponds
				// to the indices we set in the InitializeSubtypeAddRule.
				// This is not guaranteed (the user can switch them in the xml),
				// but will be the most common case, so we check it first.
				SupertypeMetaRole retVal = null;
				if (roles.Count == 2)
				{
					retVal = roles[1] as SupertypeMetaRole;
					if (retVal == null)
					{
						retVal = roles[0] as SupertypeMetaRole;
						Debug.Assert(retVal != null); // One of them better be a supertype
					}
				}
				return retVal;
			}
		}
		#endregion // Accessor functions
		#region Customize property display
		/// <summary>
		/// Display this type with a different name than a fact type
		/// </summary>
		public override string GetClassName()
		{
			return ResourceStrings.SubtypeFact;
		}
		/// <summary>
		/// Hide the NestingType property (cannot be set for a SubtypeFact)
		/// </summary>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeId = metaAttrInfo.Id;
			if (attributeId == FactType.NestingTypeDisplayMetaAttributeGuid)
			{
				return false;
			}
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		/// <summary>
		/// Display a formatted string defining the relationship
		/// for the component name
		/// </summary>
		public override string GetComponentName()
		{
			ObjectType subType;
			ObjectType superType;
			if (null != (subType = Subtype) &&
				null != (superType = Supertype))
			{
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.SubtypeFactComponentNameFormat, subType.GetComponentName(), superType.GetComponentName());
			}
			return base.GetComponentName();
		}
		#endregion // Customize property display
		#region Initialize pattern rules
		/// <summary>
		/// A rule to create a subtype-style FactType with all
		/// of the required roles and constraints.
		/// </summary>
		[RuleOn(typeof(SubtypeFact))]
		private class InitializeSubtypeAddRule : AddRule
		{
			/// <summary>
			/// Make sure a Subtype is a 1-1 fact with a mandatory role
			/// on the base type (role 0)
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType fact = e.ModelElement as FactType;
				Store store = fact.Store;
			
				// Establish role collecton
				RoleBaseMoveableCollection roles = fact.RoleCollection;
				SubtypeMetaRole subTypeMetaRole = SubtypeMetaRole.CreateSubtypeMetaRole(store);
				SupertypeMetaRole superTypeMetaRole = SupertypeMetaRole.CreateSupertypeMetaRole(store);
				roles.Add(subTypeMetaRole);
				roles.Add(superTypeMetaRole);
			
				// Add injection constraints
				superTypeMetaRole.Multiplicity = RoleMultiplicity.ExactlyOne;
				subTypeMetaRole.Multiplicity = RoleMultiplicity.ZeroToOne;

				// Add forward reading
				ReadingOrderMoveableCollection readingOrders = fact.ReadingOrderCollection;
				ReadingOrder order = ReadingOrder.CreateReadingOrder(store);
				readingOrders.Add(order);
				roles = order.RoleCollection;
				roles.Add(subTypeMetaRole);
				roles.Add(superTypeMetaRole);
				Reading reading = Reading.CreateReading(store);
				order.ReadingCollection.Add(reading);
				reading.Text = ResourceStrings.SubtypeFactPredicateReading;
				
				// Add inverse reading
				order = ReadingOrder.CreateReadingOrder(store);
				readingOrders.Add(order);
				roles = order.RoleCollection;
				roles.Add(superTypeMetaRole);
				roles.Add(subTypeMetaRole);
				reading = Reading.CreateReading(store);
				order.ReadingCollection.Add(reading);
				reading.Text = ResourceStrings.SubtypeFactPredicateInverseReading;
			}
		}
		#endregion Initialize pattern rules
		#region Role and constraint pattern locking rules
		private static void ThrowPatternModifiedException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeConstraintAndRolePatternFixed);
		}
		/// <summary>
		/// Block internal constraints from being added to a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))]
		private class LimitSubtypeConstraintsAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraintCollection.Constraint.ConstraintIsInternal)
				{
					SubtypeFact subtypeFact = link.FactTypeCollection as SubtypeFact;
					if (subtypeFact != null)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block internal constraints from being removed from a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactSetConstraint), FireTime = TimeToFire.LocalCommit)]
		private class LimitSubtypeConstraintsRemoveRule : RemoveRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraintCollection.Constraint.ConstraintIsInternal)
				{
					SubtypeFact subtypeFact = link.FactTypeCollection as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsRemoved)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being added to a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class LimitSubtypeRolesAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				SubtypeFact subtypeFact = link.FactType as SubtypeFact;
				if (subtypeFact != null)
				{
					if (subtypeFact.Model != null)
					{
						// Allow before adding to model, not afterwards
						ThrowPatternModifiedException();
					}
				}
				else
				{
					RoleBase role = link.RoleCollection;
					if (role is SubtypeMetaRole || role is SupertypeMetaRole)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactMustBeParentOfMetaRole);
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being removed from a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class LimitSubtypeRolesRemoveRule : RemoveRule
		{
			/// <summary>
			/// Block internal role modification on subtypes
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				SubtypeFact subtypeFact = link.FactType as SubtypeFact;
				if (subtypeFact != null && !subtypeFact.IsRemoved)
				{
					if (subtypeFact.Model != null)
					{
						// Allow before adding to model, not afterwards
						ThrowPatternModifiedException();
					}
				}
			}
		}
		/// <summary>
		/// Block internal constraints from being modified on a subtype.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class LimitSubtypeConstraintRolesAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				SetConstraint ic = link.ConstraintRoleSequenceCollection as SetConstraint;
				FactTypeMoveableCollection facts;
				if (ic != null &&
					ic.Constraint.ConstraintIsInternal &&
					1 == (facts = ic.FactTypeCollection).Count)
				{
					SubtypeFact subtypeFact = facts[0] as SubtypeFact;
					if (subtypeFact != null)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being removed from subtype constraints
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class LimitSubtypeConstraintRolesRemoveRule : RemoveRule
		{
			/// <summary>
			/// Block internal role modification on subtypes
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				SetConstraint ic = link.ConstraintRoleSequenceCollection as SetConstraint;
				FactTypeMoveableCollection facts;
				if (ic != null &&
					!ic.IsRemoved &&
					ic.Constraint.ConstraintIsInternal &&
					1 == (facts = ic.FactTypeCollection).Count)
				{
					SubtypeFact subtypeFact = facts[0] as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsRemoved)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block the modality from being changed on internal constraints
		/// on subtype facts
		/// </summary>
		[RuleOn(typeof(SetConstraint))]
		private class LimitSubtypeConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Block internal attribute modification on implicit subtype constraints
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				SetConstraint constraint = e.ModelElement as SetConstraint;
				if (!constraint.IsRemoved)
				{
					FactTypeMoveableCollection testFacts = null;
					if (attributeId == UniquenessConstraint.IsInternalMetaAttributeGuid ||
						attributeId == MandatoryConstraint.IsSimpleMetaAttributeGuid)
					{
						testFacts = constraint.FactTypeCollection;
					}
					else if (attributeId == SetConstraint.ModalityMetaAttributeGuid)
					{
						if (constraint.Constraint.ConstraintIsInternal)
						{
							testFacts = constraint.FactTypeCollection;
						}
					}
					if (testFacts != null)
					{
						int testFactsCount = testFacts.Count;
						for (int i = 0; i < testFactsCount; ++i)
						{
							if (testFacts[i] is SubtypeFact)
							{
								// We never do this internally, so block any modification,
								// not just those after the subtype fact is added to the model
								ThrowPatternModifiedException();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Ensure that a role player deletion on a subtype results in a deletion
		/// of the subtype itself.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class RemoveSubtypeWhenRolePlayerRemoved : RemoveRule
		{
			/// <summary>
			/// Remove the full SubtypeFact when a role player is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				if (role != null && !role.IsRemoved)
				{
					SubtypeFact subtypeFact = role.FactType as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsRemoved)
					{
						subtypeFact.Remove();
					}
				}
			}
		}
		#endregion // Role and constraint pattern locking rules
		#region Mixed role player types rules
		private static void ThrowMixedRolePlayerTypesException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeRolePlayerTypesCannotBeMixed);
		}
		/// <summary>
		/// Ensure consistent types (EntityType or ValueType) for role
		/// players in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class EnsureConsistentRolePlayerTypesAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				SubtypeMetaRole subtypeRole;
				SubtypeFact subtypeFact;
				if (null != (subtypeRole = link.PlayedRoleCollection as SubtypeMetaRole) &&
					null != (subtypeFact = subtypeRole.FactType as SubtypeFact))
				{
					ObjectType superType = subtypeFact.Supertype;
					if (null == superType ||
						((superType.DataType == null) != (link.RolePlayer.DataType == null)))
					{
						ThrowMixedRolePlayerTypesException();
					}
				}
			}
		}
		/// <summary>
		/// Stop the ValueTypeHasDataType relationship from being
		/// added if an ObjectType participates in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class EnsureConsistentDataTypesAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueTypeCollection;
				RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				for (int i = 0; i < playedRoleCount; ++i)
				{
					Role testRole = playedRoles[i];
					if (testRole is SubtypeMetaRole ||
						testRole is SupertypeMetaRole)
					{
						if (null != testRole.FactType)
						{
							ThrowMixedRolePlayerTypesException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Stop the ValueTypeHasDataType relationship from being
		/// removed if an ObjectType participates in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class EnsureConsistentDataTypesRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueTypeCollection;
				if (!objectType.IsRemoved)
				{
					RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
					int playedRoleCount = playedRoles.Count;
					for (int i = 0; i < playedRoleCount; ++i)
					{
						Role testRole = playedRoles[i];
						if (testRole is SubtypeMetaRole ||
							testRole is SupertypeMetaRole)
						{
							if (null != testRole.FactType)
							{
								ThrowMixedRolePlayerTypesException();
							}
						}
					}
				}
			}
		}
		#endregion // Mixed role player types rules
		#region Circular reference check rule
		[RuleOn(typeof(ModelHasFactType), FireTime = TimeToFire.LocalCommit)]
		private class BlockCircularSubtypesAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasFactType link = e.ModelElement as ModelHasFactType;
				SubtypeFact newSubtypeFact = link.FactTypeCollection as SubtypeFact;
				if (newSubtypeFact != null)
				{
					ObjectType superType = newSubtypeFact.Supertype;
					if (superType != null)
					{
						ThrowOnCycle(superType, superType);
					}
				}
			}
			private static void ThrowOnCycle(ObjectType iterateOn, ObjectType startingSuper)
			{
				foreach (ObjectType nestedSuper in iterateOn.SupertypeCollection)
				{
					if (object.ReferenceEquals(nestedSuper, startingSuper))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactCycle);
					}
					ThrowOnCycle(nestedSuper, startingSuper);
				}
			}
		}
		#endregion // Circular reference check rule
		#region override property readOnly
		/// <summary>
		/// Checks to see if subtype is set as primary, if so makes it's property descriptor read only
		/// </summary>
		/// <param name="propertyDescriptor"></param>
		/// <returns>whether or not property descriptor is read only</returns>
		public override bool IsPropertyDescriptorReadOnly(System.ComponentModel.PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor elementDescriptor = propertyDescriptor as ElementPropertyDescriptor;
			if(elementDescriptor != null && elementDescriptor.MetaAttributeInfo.Id == IsPrimaryMetaAttributeGuid)
			{
				return IsPrimary;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion //property readOnly
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// validates all model errors and adds errors to the task provider.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new SubtypeFactFixupListener();
			}
		}
		/// <summary>
		/// A listener class to enforce valid subtype facts on load.
		/// Invalid subtype patterns will either be fixed up or completely
		/// removed.
		/// </summary>
		private class SubtypeFactFixupListener : DeserializationFixupListener<SubtypeFact>
		{
			/// <summary>
			/// Create a new SubtypeFactFixupListener
			/// </summary>
			public SubtypeFactFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Make sure the subtype fact constraint pattern
			/// and object types are appropriate.
			/// </summary>
			/// <param name="element">An SubtypeFact instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(SubtypeFact element, Store store, INotifyElementAdded notifyAdded)
			{
				// Note that the arity and types of the subtype/supertype roles are
				// enforced by the schema.
				Role superTypeMetaRole;
				Role subTypeMetaRole;
				ObjectType superType;
				ObjectType subType;
				if (null == (superTypeMetaRole = element.SupertypeRole) ||
					null == (subTypeMetaRole = element.SubtypeRole) ||
					null == (superType = superTypeMetaRole.RolePlayer) ||
					null == (subType = subTypeMetaRole.RolePlayer) ||
					// They must both be value types or object types, but can't switch
					((superType.DataType == null) != (subType.DataType == null)))
				{
					RemoveFact(element);
				}
				else
				{
					// Note that rules aren't on, so we can read the Multiplicity attributes,
					// but we can't set them. All changes must be made explicitly.
					if (superTypeMetaRole.Multiplicity != RoleMultiplicity.ExactlyOne)
					{
						EnsureSingleColumnUniqueAndMandatory(store, element.Model, subTypeMetaRole, true, notifyAdded);
					}
					if (subTypeMetaRole.Multiplicity != RoleMultiplicity.ZeroToOne)
					{
						EnsureSingleColumnUniqueAndMandatory(store, element.Model, superTypeMetaRole, false, notifyAdded);
					}
				}
			}
			/// <summary>
			/// Internal constraints are not fully connected at this point (FactSetConstraint instances
			/// are not implicitly constructed until a later phase), so we need to work a little harder
			/// to remove them.
			/// </summary>
			/// <param name="fact">The fact to clear of external constraints</param>
			private static void RemoveFact(FactType fact)
			{
				RoleBaseMoveableCollection factRoles = fact.RoleCollection;
				int roleCount = factRoles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = factRoles[i].Role;
					ConstraintRoleSequenceMoveableCollection sequences = role.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = sequenceCount - 1; j >= 0; --j)
					{
						SetConstraint ic = sequences[j] as SetConstraint;
						if (ic != null && ic.Constraint.ConstraintIsInternal)
						{
							ic.Remove();
						}
					}
				}
				fact.Remove();
			}
			private static void EnsureSingleColumnUniqueAndMandatory(Store store, ORMModel model, Role role, bool requireMandatory, INotifyElementAdded notifyAdded)
			{
				ConstraintRoleSequenceMoveableCollection sequences = role.ConstraintRoleSequenceCollection;
				int sequenceCount = sequences.Count;
				bool haveUniqueness = false;
				bool haveMandatory = !requireMandatory;
				SetConstraint ic;
				for (int i = sequenceCount - 1; i >= 0; --i)
				{
					ic = sequences[i] as SetConstraint;
					if (ic != null && ic.Constraint.ConstraintIsInternal)
					{
						if (ic.RoleCollection.Count == 1 && ic.Modality == ConstraintModality.Alethic)
						{
							switch (ic.Constraint.ConstraintType)
							{
								case ConstraintType.InternalUniqueness:
									if (haveUniqueness)
									{
										ic.Remove();
									}
									else
									{
										haveUniqueness = true;
									}
									break;
								case ConstraintType.SimpleMandatory:
									if (haveMandatory)
									{
										ic.Remove();
									}
									else
									{
										haveMandatory = true;
									}
									break;
							}
						}
						else
						{
							ic.Remove();
						}
					}
				}
				if (!haveUniqueness)
				{
					ic = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
				if (!haveMandatory)
				{
					ic = MandatoryConstraint.CreateSimpleMandatoryConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
			}
		}
		#endregion Deserialization Fixup
	}
}
