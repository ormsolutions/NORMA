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
			if (domainProperty.Id.Equals(FactType.DerivationStorageDisplayDomainPropertyId))
			{
				return ModelElement.DerivationRule != null;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
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
