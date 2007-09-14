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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Role"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RoleTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : Role
	{
		/// <summary>
		/// Initializes a new instance of <see cref="RoleTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RoleTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
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
				return !ModelElement.IsValueRole;
			}
			else if (propertyId == Role.IsMandatoryDomainPropertyId ||
				propertyId == Role.RolePlayerDisplayDomainPropertyId ||
				propertyId == Role.MandatoryConstraintModalityDomainPropertyId ||
				propertyId == Role.MultiplicityDomainPropertyId)
			{
				Role role = ModelElement;
				if (role is SubtypeMetaRole ||
					role is SupertypeMetaRole ||
					null != role.FactType.UnaryRole)
				{
					return true;
				}
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
}
