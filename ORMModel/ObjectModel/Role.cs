using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class Role
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
				attributeGuid == IsMandatoryMetaAttributeGuid)
			{
				// Handled by RoleChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
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
				ConstraintRoleSetMoveableCollection constraintRoleSets = ConstraintRoleSetCollection;
				int roleSetCount = constraintRoleSets.Count;
				for (int i = 0; i < roleSetCount; ++i)
				{
					ConstraintRoleSet roleSet = constraintRoleSets[i];
					Constraint constraint = roleSet.Constraint;
					if (constraint.ConstraintType == ConstraintType.Mandatory)
					{
						return true;
					}
				}
				return false;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		#endregion // CustomStorage handlers
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
				else if (attributeGuid == Role.IsMandatoryMetaAttributeGuid)
				{
					Role role = e.ModelElement as Role;
					if ((bool)e.NewValue)
					{
						// Add a mandatory constraint
						Store store = role.Store;
						FactType factType;
						ORMModel model;
						if ((null == (factType = role.FactType)) ||
							(null == (model = factType.Model)))
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionIsMandatoryRequiresAttachedFactType);
						}
						InternalConstraintRoleSet roleSet = InternalConstraintRoleSet.CreateInternalConstraintRoleSet(store);
						roleSet.RoleCollection.Add(role);
						InternalConstraint constraint = MandatoryConstraint.CreateMandatoryConstraint(store);
						constraint.Model = model;
						constraint.RoleSet = roleSet;
					}
					else
					{
						// Find and remove the mandatory constraint
						ConstraintRoleSetMoveableCollection constraintRoleSets = role.ConstraintRoleSetCollection;
						int roleSetCount = constraintRoleSets.Count;
						for (int i = 0; i < roleSetCount; ++i)
						{
							Constraint constraint = constraintRoleSets[i].Constraint;
							if (constraint.ConstraintType == ConstraintType.Mandatory)
							{
								constraint.Remove();
								// Should only have one of these, but we might as well keep going
								// because any of them would make the property appear to be true
							}
						}
					}
				}
			}
		}
		#endregion // RoleChangeRule class
	}
}