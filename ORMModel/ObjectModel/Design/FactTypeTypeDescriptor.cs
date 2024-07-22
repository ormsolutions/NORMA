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
using System.Drawing.Design;

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
		/// Add unary negation custom properties as needed
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection retVal = base.GetProperties(attributes);
			FactType factType = ModelElement;
			Store store;
			if (factType.UnaryPattern == UnaryValuePattern.Negation &&
				null != (store = Utility.ValidateStore(factType.Store)))
			{
				retVal = EditorUtility.GetEditablePropertyDescriptors(retVal);
				retVal.Add(UnaryNegationPropertyDescriptor);
				FactType positiveFactType = factType.PositiveUnaryFactType;
				if (positiveFactType != null)
				{
					retVal.Add(EditorUtility.RedirectPropertyDescriptor(positiveFactType, TypeDescriptor.GetProperties(positiveFactType, attributes, false)["UnaryPattern"], typeof(FactType)));
				}
			}
			return retVal;
		}

		/// <summary>
		/// Ensure that the <see cref="FactType.Name"/> property is read-only when
		/// <see cref="FactType.Objectification"/> is <see langword="null"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid attributeId = propertyDescriptor.DomainPropertyInfo.Id;
			if (attributeId == FactType.NameDomainPropertyId)
			{
				FactType factType = ModelElement;
				FactTypeDerivationRule derivationRule;
				return factType.Objectification == null &&
					(null == (derivationRule = factType.DerivationRule as FactTypeDerivationRule) || derivationRule.DerivationCompleteness != DerivationCompleteness.FullyDerived || derivationRule.ExternalDerivation);
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
			if (propertyId == FactType.DerivationStorageDisplayDomainPropertyId)
			{
				FactType factType = ModelElement;
				return factType.DerivationRule != null;
			}
			else if (propertyId == FactType.IsExternalDomainPropertyId)
			{
				// UNDONE: Support IsExternal
				return false;
			}
			else if (propertyId == FactType.UnaryPatternDomainPropertyId)
			{
				switch (ModelElement.UnaryPattern)
				{
					case UnaryValuePattern.NotUnary:
					// This is redirected to the positive unary for the negation case.
					case UnaryValuePattern.Negation:
						return false;
				}
				return true;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Make the unary pattern type descriptor handle automated elements
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			if (domainPropertyInfo.Id == FactType.UnaryPatternDomainPropertyId)
			{
				return new AutomatedElementFilterPropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			return base.CreatePropertyDescriptor(requestor, domainPropertyInfo, attributes);
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
		/// Returns an instance of <see cref="ObjectifyingEntityTypePropertyDescriptor"/> for <see cref="Objectification.NestingType"/>.
		/// </summary>
		protected override RolePlayerPropertyDescriptor CreateRolePlayerPropertyDescriptor(ModelElement sourceRolePlayer, DomainRoleInfo targetRoleInfo, Attribute[] sourceDomainRoleInfoAttributes)
		{
			if (Utility.IsDescendantOrSelf(targetRoleInfo, Objectification.NestingTypeDomainRoleId))
			{
				return new ObjectifyingEntityTypePropertyDescriptor((FactType)sourceRolePlayer, targetRoleInfo, sourceDomainRoleInfoAttributes);
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

		private static PropertyDescriptor myUnaryNegationStatusPropertyDescriptor;
		/// <summary>
		/// Get a custom <see cref="PropertyDescriptor"/> to show when a unary negation fact type
		/// is displayed. The 'UnaryPattern' property is redirected to the positive unary.
		/// </summary>
		public static PropertyDescriptor UnaryNegationPropertyDescriptor
		{
			get
			{
				PropertyDescriptor retVal = myUnaryNegationStatusPropertyDescriptor;
				if (retVal == null)
				{
					PropertyDescriptor innerDescriptor = TypeDescriptor.CreateProperty(typeof(UnaryNegationComponent), "UnaryNegation", typeof(Boolean));
					myUnaryNegationStatusPropertyDescriptor = retVal = EditorUtility.RedirectPropertyDescriptor(
						new UnaryNegationComponent(),
						EditorUtility.ModifyPropertyDescriptorDisplay(TypeDescriptor.CreateProperty(typeof(UnaryNegationComponent), "UnaryNegation", typeof(Boolean)), "IsUnaryNegation", ResourceStrings.FactTypeUnaryNegationDisplayName, ResourceStrings.FactTypeUnaryNegationDisplayName, null, true),
						typeof(FactType));
				}
				return retVal;
			}
		}
		private class UnaryNegationComponent
		{
			public bool UnaryNegation
			{
				get
				{
					return true;
				}
			}
		}
	}
}
