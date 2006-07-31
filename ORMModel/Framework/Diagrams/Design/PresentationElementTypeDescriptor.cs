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
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.Modeling.Diagrams.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="PresentationElement"/>s of type <typeparamref name="TPresentationElement"/>.
	/// </summary>
	/// <typeparam name="TPresentationElement">
	/// The type of the <see cref="PresentationElement"/> that this <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
	/// </typeparam>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="ModelElement"/> that <typeparamref name="TPresentationElement"/> is associated with.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class PresentationElementTypeDescriptor<TPresentationElement, TModelElement> : PresentationElementTypeDescriptor
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Initializes a new instance of <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> for
		/// the instance of <typeparamref name="TPresentationElement"/> specified by <paramref name="presentationElement"/>
		/// that is associated with the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		protected PresentationElementTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
		{
			// The PresentationElementTypeDescriptor constructor already checked presentationElement for null.
			myPresentationElement = presentationElement;
			// The ElementTypeDescriptor constructor already checked selectedElement for null.
			myModelElement = selectedElement;
		}

		private readonly TPresentationElement myPresentationElement;
		/// <summary>
		/// The <see cref="T:PresentationElement"/> of type <typeparamref name="TPresentationElement"/> that
		/// this <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		protected new TPresentationElement PresentationElement
		{
			get
			{
				return myPresentationElement;
			}
		}

		private readonly TModelElement myModelElement;
		/// <summary>
		/// The <see cref="T:ModelElement"/> of type <typeparamref name="TModelElement"/> that
		/// <see cref="M:PresentationElement"/> is associated with.
		/// </summary>
		protected new TModelElement ModelElement
		{
			get
			{
				return myModelElement;
			}
		}

		/// <summary>
		/// Calls <see cref="PresentationElementTypeDescriptor.GetProperties(Attribute[])"/>,
		/// passing <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		/// <summary>
		/// Returns the class name of <see cref="M:ModelElement"/>
		/// (the <typeparamref name="TModelElement"/> that
		/// <see cref="M:PresentationElement"/> is associated with).
		/// </summary>
		public override string GetClassName()
		{
			return TypeDescriptor.GetClassName(ModelElement);
		}

		/// <summary>
		/// Returns the component name of <see cref="M:ModelElement"/>
		/// (the <typeparamref name="TModelElement"/> that
		/// <see cref="M:PresentationElement"/> is associated with).
		/// </summary>
		public override string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(ModelElement);
		}
	}
}
