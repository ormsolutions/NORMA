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
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="ObjectifiedFactTypeNameShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectifiedFactTypeNameShapeTypeDescriptor<TPresentationElement, TModelElement> : ORMBaseShapeTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : ObjectifiedFactTypeNameShape
		where TModelElement : ObjectType
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectifiedFactTypeNameShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ObjectifiedFactTypeNameShapeTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
			: base(parent, presentationElement, selectedElement)
		{
		}
		/// <summary>
		/// Disable modification of the <see cref="ObjectifiedFactTypeNameShape.ExpandRefMode"/> property
		/// if the preferred identifier for the reference mode is objectified.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == ObjectifiedFactTypeNameShape.ExpandRefModeDomainPropertyId)
			{
				ObjectType objectType = ModelElement;
				return objectType.HasReferenceMode && objectType.PreferredIdentifier.IsObjectifiedSingleRolePreferredIdentifier;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
}
