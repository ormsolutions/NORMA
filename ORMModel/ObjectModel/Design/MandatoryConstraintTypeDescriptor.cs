#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="MandatoryConstraint"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MandatoryConstraintTypeDescriptor<TModelElement> : SetConstraintTypeDescriptor<TModelElement>
		where TModelElement : MandatoryConstraint
	{
		/// <summary>
		/// Initializes a new instance of <see cref="MandatoryConstraintTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public MandatoryConstraintTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Redirect the displayed component name if this <see cref="MandatoryConstraint"/> is part of an
		/// <c>Exclusive Or</c> constraint.
		/// </summary>
		public override string GetComponentName()
		{
			ExclusionConstraint exclusionConstraint = ModelElement.ExclusiveOrExclusionConstraint;
			return (exclusionConstraint != null) ? TypeDescriptor.GetComponentName(exclusionConstraint) : base.GetComponentName();
		}

		/// <summary>
		/// Distinguish between simple <see cref="MandatoryConstraint"/>s, disjunctive <see cref="MandatoryConstraint"/>s,
		/// and <c>Exclusive Or</c> constraints in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			MandatoryConstraint mandatoryConstraint = ModelElement;
			return mandatoryConstraint.IsSimple ?
				ResourceStrings.SimpleMandatoryConstraint :
				mandatoryConstraint.IsImplied ?
					ResourceStrings.ImpliedMandatoryConstraint :
					(mandatoryConstraint.ExclusiveOrExclusionConstraint == null) ?
						ResourceStrings.DisjunctiveMandatoryConstraint :
						ResourceStrings.ExclusiveOrConstraint;
		}

		/// <summary>
		/// Add a <c>Name</c> property for the associated <see cref="ExclusionConstraint"/> if an
		/// <see cref="ExclusiveOrConstraintCoupler"/> is attached to this <see cref="MandatoryConstraint"/>.
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			ExclusionConstraint exclusionConstraint = ModelElement.ExclusiveOrExclusionConstraint;
			if (exclusionConstraint != null)
			{
				DomainPropertyInfo exclusionConstraintNameDomainProperty = exclusionConstraint.GetDomainClass().NameDomainProperty;
				properties.Add(new ElementPropertyDescriptor(this, exclusionConstraint, exclusionConstraintNameDomainProperty,
					base.GetDomainPropertyAttributes(exclusionConstraintNameDomainProperty)));
			}
			return properties;
		}
		/// <summary>
		/// Make sure the <see cref="SetConstraint.Modality">Modality</see> property
		/// is read only for simple mandatory constraints on the Objectification end of an implied fact type.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			MandatoryConstraint element;
			LinkedElementCollection<Role> roles;
			FactType factType;
			if (propertyDescriptor.DomainPropertyInfo.Id == MandatoryConstraint.ModalityDomainPropertyId &&
				(element = ModelElement).Store != null &&
				(element.IsImplied ||
				(element.IsSimple &&
				(roles = element.RoleCollection).Count == 1 &&
				((factType = roles[0].FactType).ImpliedByObjectification != null ||
				factType is SubtypeFact))))
			{
				return true;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		/// <summary>
		/// Distinguish between the <c>Name</c> properties for the <see cref="ExclusionConstraint"/> and
		/// the <see cref="MandatoryConstraint"/> that make up an <c>Exclusive Or</c> constraint.
		/// </summary>
		protected override string GetPropertyDescriptorDisplayName(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == MandatoryConstraint.NameDomainPropertyId &&
				ModelElement.Store != null &&
				ModelElement.ExclusiveOrExclusionConstraint != null)
			{
				return propertyDescriptor.ModelElement is MandatoryConstraint ?
					ResourceStrings.ExclusiveOrConstraintMandatoryConstraintNameDisplayName :
					ResourceStrings.ExclusiveOrConstraintExclusionConstraintNameDisplayName;
			}
			return base.GetPropertyDescriptorDisplayName(propertyDescriptor);
		}
	}
}
