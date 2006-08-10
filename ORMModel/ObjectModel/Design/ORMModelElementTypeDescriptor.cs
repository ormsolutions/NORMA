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
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ORMModelElement"/>s of type <typeparamref name="TModelElement"/>.
	/// </summary>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="ORMModelElement"/> that this <see cref="ORMModelElementTypeDescriptor{TModelElement}"/> is for.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMModelElementTypeDescriptor<TModelElement> : ElementTypeDescriptor<TModelElement>
		where TModelElement : ORMModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ORMModelElementTypeDescriptor{TModelElement}"/> for
		/// the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ORMModelElementTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Adds extension properties to the <see cref="PropertyDescriptorCollection"/> before returning it.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>.
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			ExtendableElementUtility.GetExtensionProperties(ModelElement, properties);
			return properties;
		}

		/// <summary>
		/// Not used, don't look for them.
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}

		/// <summary>
		/// Let our *Display properties handle these.
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			// UNDONE: We may want to lose the *Display properties. Need a way to filter
			// the contents of a RolePlayerPropertyDescriptor dropdown list
			// UNDONE: MSBUG RolePlayerPropertyDescriptor should respect the System.ComponentModel.EditorAttribute on the
			// generated property.
			return false;
		}
	}
}
