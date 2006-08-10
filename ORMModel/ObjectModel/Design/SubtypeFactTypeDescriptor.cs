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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="SubtypeFact"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SubtypeFactTypeDescriptor<TModelElement> : FactTypeTypeDescriptor<TModelElement>
		where TModelElement : SubtypeFact
	{
		/// <summary>
		/// Initializes a new instance of <see cref="SubtypeFactTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public SubtypeFactTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Display this type with a different name than a <see cref="FactType"/>.
		/// </summary>
		public override string GetClassName()
		{
			return ResourceStrings.SubtypeFact;
		}

		/// <summary>
		/// Display a formatted string defining the relationship
		/// for the component name.
		/// </summary>
		public override string GetComponentName()
		{
			SubtypeFact subtypeFact = ModelElement;
			ObjectType subtype;
			ObjectType supertype;
			if ((subtype = subtypeFact.Subtype) != null && (supertype = subtypeFact.Supertype) != null)
			{
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.SubtypeFactComponentNameFormat, TypeDescriptor.GetComponentName(subtype), TypeDescriptor.GetComponentName(supertype));
			}
			return base.GetComponentName();
		}

		/// <summary>
		/// Hide the <see cref="FactType.NestingTypeDisplay"/> property.
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			if (domainProperty.Id.Equals(FactType.NestingTypeDisplayDomainPropertyId))
			{
				return false;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Ensure that the <see cref="SubtypeFact.IsPrimary"/> property is read-only when
		/// it is <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id.Equals(SubtypeFact.IsPrimaryDomainPropertyId))
			{
				return ModelElement.IsPrimary;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
}
