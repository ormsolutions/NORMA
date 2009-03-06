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
using System.Reflection;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptionProvider"/> for <typeparamref name="TPresentationElement"/>s.
	/// </summary>
	/// <remarks>
	/// <typeparamref name="TTypeDescriptor"/> must have a constructor which accepts three parameters,
	/// with the first parameter being of type <see cref="ICustomTypeDescriptor"/>, the second being of the type
	/// specified by <typeparamref name="TPresentationElement"/>, and the third being of the type specified by
	/// <typeparamref name="TModelElement"/>.
	/// <code>
	/// public MyPresentationElementTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
	///		: base(parent, presentationElement, selectedElement)
	/// {
	///		// ...
	/// }
	/// </code>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class PresentationElementTypeDescriptionProvider<TPresentationElement, TModelElement, TTypeDescriptor> : PresentationElementTypeDescriptionProvider
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
		where TTypeDescriptor : PresentationElementTypeDescriptor
	{
		private static readonly RuntimeTypeHandle TypeDescriptorTypeHandle = typeof(TTypeDescriptor).TypeHandle;
		private static readonly RuntimeMethodHandle TypeDescriptorConstructorHandle = Type.GetTypeFromHandle(TypeDescriptorTypeHandle).GetConstructor(
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
			null, new Type[] { typeof(ICustomTypeDescriptor), typeof(TPresentationElement), typeof(TModelElement) }, null).MethodHandle;

		/// <summary>See <see cref="PresentationElementTypeDescriptionProvider.CreatePresentationElementTypeDescriptor"/>.</summary>
		protected sealed override PresentationElementTypeDescriptor CreatePresentationElementTypeDescriptor(ICustomTypeDescriptor parent, PresentationElement presentationElement, ModelElement selectedElement)
		{
			return (PresentationElementTypeDescriptor)((ConstructorInfo)ConstructorInfo.GetMethodFromHandle(TypeDescriptorConstructorHandle, TypeDescriptorTypeHandle)).Invoke(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
				null, new object[] { parent, presentationElement as TPresentationElement, selectedElement as TModelElement }, null);
		}
	}
}
