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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="FactType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FactTypeTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : FactType
	{
		/// <summary>
		/// Initializes a new instance of <see cref="FactTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public FactTypeTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Ensure that the <see cref="FactType.Name"/> property is read-only when
		/// <see cref="FactType.Objectification"/> is <see langword="null"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id.Equals(FactType.NameDomainPropertyId))
			{
				return ModelElement.Objectification == null;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}

		/// <summary>
		/// Stop the <see cref="FactType.DerivationStorageDisplay"/> property from displaying if
		/// no <see cref="FactType.DerivationRule"/> is specified.
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid propertyId = domainProperty.Id;
			if (propertyId.Equals(FactType.DerivationStorageDisplayDomainPropertyId))
			{
				return ModelElement.DerivationRule != null;
			}
			else if (propertyId.Equals(FactType.IsExternalDomainPropertyId))
			{
				// UNDONE: Support IsExternal
				return false;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Allow <see cref="RolePlayerPropertyDescriptor"/>s.
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return true;
		}

		/// <summary>
		/// Only create <see cref="RolePlayerPropertyDescriptor"/>s for <see cref="Objectification.NestingType"/>.
		/// </summary>
		protected override bool ShouldCreateRolePlayerPropertyDescriptor(ModelElement sourceRolePlayer, DomainRoleInfo sourceRole)
		{
			return Utility.IsDescendantOrSelf(sourceRole, Objectification.NestedFactTypeDomainRoleId);
		}

		/// <summary>
		/// Returns an instance of <see cref="ObjectificationRolePlayerPropertyDescriptor"/> for <see cref="Objectification.NestingType"/>.
		/// </summary>
		protected override RolePlayerPropertyDescriptor CreateRolePlayerPropertyDescriptor(ModelElement sourceRolePlayer, DomainRoleInfo targetRoleInfo, Attribute[] sourceDomainRoleInfoAttributes)
		{
			if (Utility.IsDescendantOrSelf(targetRoleInfo, Objectification.NestingTypeDomainRoleId))
			{
				return new ObjectificationRolePlayerPropertyDescriptor(sourceRolePlayer, targetRoleInfo, sourceDomainRoleInfoAttributes);
			}
			return base.CreateRolePlayerPropertyDescriptor(sourceRolePlayer, targetRoleInfo, sourceDomainRoleInfoAttributes);
		}


		/// <summary>
		/// Distinguish between objectified and non-objectified <see cref="FactType"/>s in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			Objectification objectification = ModelElement.Objectification;
			return (objectification == null || objectification.IsImplied) ? ResourceStrings.FactType : ResourceStrings.ObjectifiedFactType;
		}
	}
}
