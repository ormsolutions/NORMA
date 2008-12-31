#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="SetConstraint"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SetConstraintTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : SetConstraint
	{
		/// <summary>
		/// Initializes a new instance of <see cref="SetConstraintTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public SetConstraintTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		/// <summary>
		/// Customize the description of the Name property
		/// </summary>
		protected override string GetDescription(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == SetConstraint.NameDomainPropertyId)
			{
				return ResourceStrings.ConstraintNameDescription;
			}
			return base.GetDescription(propertyDescriptor);
		}
	}
}
