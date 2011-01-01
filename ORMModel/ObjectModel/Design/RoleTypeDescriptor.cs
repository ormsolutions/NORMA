#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Role"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RoleTypeDescriptor : ORMModelElementTypeDescriptor<Role>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="RoleTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RoleTypeDescriptor(ICustomTypeDescriptor parent, Role selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Add custom display properties
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = EditorUtility.GetEditablePropertyDescriptors(base.GetProperties(attributes));
			properties.Add(GetRolePlayerDisplayPropertyDescriptor(ModelElement));
			return properties;
		}
		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid propertyId = domainProperty.Id;
			if (propertyId.Equals(Role.MultiplicityDomainPropertyId))
			{
				FactType factType = ModelElement.FactType;
				LinkedElementCollection<RoleBase> roles;
				// Display for binary fact types
				return factType != null && (roles = factType.RoleCollection).Count == 2 && !FactType.GetUnaryRoleIndex(roles).HasValue;
			}
			else if (propertyId.Equals(Role.MandatoryConstraintNameDomainPropertyId) ||
				propertyId.Equals(Role.MandatoryConstraintModalityDomainPropertyId))
			{
				return ModelElement.SimpleMandatoryConstraint != null;
			}
			else if (propertyId.Equals(Role.ObjectificationOppositeRoleNameDomainPropertyId))
			{
				FactType fact = ModelElement.FactType;
				return fact != null && fact.Objectification != null;
			}
			else
			{
				return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
			}
		}

		/// <summary>
		/// Ensure that the <see cref="Role.ValueRangeText"/> property is read-only when the
		/// <see cref="Role.RolePlayer"/> is an entity type without a <see cref="ReferenceMode"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (propertyId == Role.ValueRangeTextDomainPropertyId)
			{
				return ModelElement.ValueConstraint == null && !ModelElement.IsValueRole;
			}
			else if (propertyId == Role.IsMandatoryDomainPropertyId ||
				propertyId == Role.MandatoryConstraintModalityDomainPropertyId ||
				propertyId == Role.MultiplicityDomainPropertyId)
			{
				Role role = ModelElement;
				FactType factType;
				if (role is SubtypeMetaRole ||
					role is SupertypeMetaRole ||
					null != (factType = role.FactType)
					&& null != factType.UnaryRole)
				{
					return true;
				}
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // Base overrides
		#region Non-DSL Custom Property Descriptors
		private static PropertyDescriptor myRolePlayerDisplayPropertyDescriptor;
		private static PropertyDescriptor myRolePlayerDisplayReadOnlyPropertyDescriptor;
		/// <summary>
		/// Get a <see cref="PropertyDescriptor"/> for the <see cref="P:ORMModel.ModelErrorDisplayFilterDisplay"/> property
		/// </summary>
		public static PropertyDescriptor GetRolePlayerDisplayPropertyDescriptor(Role role)
		{
			bool isReadOnly = false;
			ObjectType rolePlayer;
			FactType factType;
			if (role is SubtypeMetaRole ||
				role is SupertypeMetaRole ||
				(null != (factType = role.FactType) &&
				null != factType.ImpliedByObjectification) ||
				(null != (rolePlayer = role.RolePlayer) && rolePlayer.IsImplicitBooleanValue))
			{
				isReadOnly = true;
			}
			PropertyDescriptor retVal = isReadOnly ? myRolePlayerDisplayReadOnlyPropertyDescriptor : myRolePlayerDisplayPropertyDescriptor;
			if (retVal == null)
			{
				PropertyDescriptor reflectedDescriptor = TypeDescriptor.CreateProperty(typeof(Role), "RolePlayerDisplay", typeof(ObjectType));
				string displayName = ResourceStrings.RoleRolePlayerDisplayDisplayName;
				string description = ResourceStrings.RoleRolePlayerDisplayDescription;
				myRolePlayerDisplayPropertyDescriptor = new StoreEnabledPropertyDescriptor(reflectedDescriptor, displayName, description, null);
				myRolePlayerDisplayReadOnlyPropertyDescriptor = new StoreEnabledReadOnlyPropertyDescriptor(reflectedDescriptor, displayName, description, null);
				retVal = isReadOnly ? myRolePlayerDisplayReadOnlyPropertyDescriptor : myRolePlayerDisplayPropertyDescriptor;
			}
			return retVal;
		}
		#endregion // Non-DSL Custom Property Descriptors
	}
}
