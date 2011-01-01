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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ExclusionConstraint"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ExclusionConstraintTypeDescriptor<TModelElement> : SetComparisonConstraintTypeDescriptor<TModelElement>
		where TModelElement : ExclusionConstraint
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ExclusionConstraintTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ExclusionConstraintTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Distinguish between <see cref="ExclusionConstraint"/>s and <c>Exclusive Or</c> constraints
		/// in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			return (ModelElement.ExclusiveOrMandatoryConstraint == null) ? base.GetClassName() : ResourceStrings.ExclusiveOrConstraint;
		}
		
		/// <summary>
		/// Add a <c>Name</c> property for the associated <see cref="MandatoryConstraint"/> if an
		/// <see cref="ExclusiveOrConstraintCoupler"/> is attached to this <see cref="ExclusionConstraint"/>.
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			MandatoryConstraint mandatoryConstraint = ModelElement.ExclusiveOrMandatoryConstraint;
			if (mandatoryConstraint != null)
			{
				DomainPropertyInfo mandatoryConstraintNameDomainProperty = mandatoryConstraint.GetDomainClass().NameDomainProperty;
				properties.Add(EditorUtility.RedirectPropertyDescriptor(
					mandatoryConstraint,
					new ElementPropertyDescriptor(
						mandatoryConstraint,
						mandatoryConstraintNameDomainProperty,
						base.GetDomainPropertyAttributes(mandatoryConstraintNameDomainProperty)),
					typeof(ExclusionConstraint)));
			}
			return properties;
		}

		/// <summary>
		/// Distinguish between the <c>Name</c> properties for the <see cref="ExclusionConstraint"/> and
		/// the <see cref="MandatoryConstraint"/> that make up an <c>Exclusive Or</c> constraint.
		/// </summary>
		protected override string GetPropertyDescriptorDisplayName(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == ExclusionConstraint.NameDomainPropertyId &&
				ModelElement.Store != null &&
				ModelElement.ExclusiveOrMandatoryConstraint != null)
			{
				return propertyDescriptor.ModelElement is ExclusionConstraint ?
					ResourceStrings.ExclusiveOrConstraintExclusionConstraintNameDisplayName :
					ResourceStrings.ExclusiveOrConstraintMandatoryConstraintNameDisplayName;
			}
			return base.GetPropertyDescriptorDisplayName(propertyDescriptor);
		}
	}
}
