using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;

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
				RoleMoveableCollection roles = RoleCollection;
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
				RoleMoveableCollection roles = RoleCollection;
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
				RoleMoveableCollection roles = fact.RoleCollection;
				SubtypeMetaRole subTypeMetaRole = SubtypeMetaRole.CreateSubtypeMetaRole(store);
				SupertypeMetaRole superTypeMetaRole = SupertypeMetaRole.CreateSupertypeMetaRole(store);
				roles.Add(subTypeMetaRole);
				roles.Add(superTypeMetaRole);
			
				// Add injection constraints
				superTypeMetaRole.Multiplicity = RoleMultiplicity.ZeroToOne;
				subTypeMetaRole.Multiplicity = RoleMultiplicity.ExactlyOne;

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
		[RuleOn(typeof(FactTypeHasInternalConstraint))]
		private class LimitSubtypeConstraintsAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				SubtypeFact subtypeFact = link.FactType as SubtypeFact;
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
		/// <summary>
		/// Block internal constraints from being removed from a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class LimitSubtypeConstraintsRemoveRule : RemoveRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
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
				InternalConstraint ic = link.ConstraintRoleSequenceCollection as InternalConstraint;
				if (ic != null)
				{
					SubtypeFact subtypeFact = ic.FactType as SubtypeFact;
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
				InternalConstraint ic = link.ConstraintRoleSequenceCollection as InternalConstraint;
				if (ic != null && !ic.IsRemoved)
				{
					SubtypeFact subtypeFact = ic.FactType as SubtypeFact;
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
	}
}