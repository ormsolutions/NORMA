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
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="ORMBaseShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMBaseShapeTypeDescriptor<TPresentationElement, TModelElement> : PresentationElementTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : ORMBaseShape
		where TModelElement : ORMModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ORMBaseShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ORMBaseShapeTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
			: base(parent, presentationElement, selectedElement)
		{
		}

		/// <summary>
		/// Adds extension properties to the <see cref="PropertyDescriptorCollection"/> before returning it.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor{TModelElement}.GetProperties(Attribute[])"/>
		/// <seealso cref="ICustomTypeDescriptor.GetProperties(Attribute[])"/>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			TPresentationElement shapeElement = PresentationElement;
			if (shapeElement != null && null != Utility.ValidateStore(shapeElement.Store))
			{
				properties = ExtendableElementUtility.GetExtensionProperties(shapeElement, properties, typeof(TPresentationElement));
			}
			return properties;
		}
	}
}
