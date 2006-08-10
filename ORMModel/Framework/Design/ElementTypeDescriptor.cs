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

namespace Neumont.Tools.Modeling.Design
{
	#region ElementTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ModelElement"/>s of type <typeparamref name="TModelElement"/>.
	/// </summary>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="ModelElement"/> that this <see cref="ElementTypeDescriptor{TModelElement}"/> is for.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ElementTypeDescriptor<TModelElement> : ElementTypeDescriptor
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ElementTypeDescriptor{TModelElement}"/> for
		/// the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ElementTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
			// The ElementTypeDescriptor constructor already checked selectedElement for null.   
			myModelElement = selectedElement;
		}

		private readonly TModelElement myModelElement;
		/// <summary>
		/// The <see cref="T:ModelElement"/> of type <typeparamref name="TModelElement"/>
		/// that this <see cref="ElementTypeDescriptor{TModelElement}"/> is for.
		/// </summary>
		protected new TModelElement ModelElement
		{
			get
			{
				return myModelElement;
			}
		}

		/// <summary>
		/// Returns the <see cref="DomainObjectInfo.DisplayName"/> for the
		/// <see cref="DomainClassInfo"/> of <see cref="M:ModelElement"/>.
		/// </summary>
		public override string GetClassName()
		{
			return ModelElement.GetDomainClass().DisplayName;
		}

		// ElementTypeDescriptor already has a good (name-based) override
		// for GetComponentName(), so we don't need to provide one here.

		/// <summary>
		/// Calls <see cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>,
		/// passing <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}
	}
	#endregion // ElementTypeDescriptor class
}
