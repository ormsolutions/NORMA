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
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptionProvider"/> for <typeparamref name="TModelElement"/>s.
	/// </summary>
	/// <remarks>
	/// <typeparamref name="TTypeDescriptor"/> must have a constructor which accepts two
	/// parameters, with the first parameter being of type <see cref="ICustomTypeDescriptor"/>
	/// and the second being of the type specified by <typeparamref name="TModelElement"/>.
	/// <code>
	/// public MyElementTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
	///		: base(parent, selectedElement)
	/// {
	///		// ...
	/// }
	/// </code>
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ElementTypeDescriptionProvider<TModelElement, TTypeDescriptor> : ElementTypeDescriptionProvider
		where TModelElement : ModelElement
		where TTypeDescriptor : ElementTypeDescriptor
	{
		private static readonly RuntimeTypeHandle TypeDescriptorTypeHandle = typeof(TTypeDescriptor).TypeHandle;
		private static readonly RuntimeMethodHandle TypeDescriptorConstructorHandle = Type.GetTypeFromHandle(TypeDescriptorTypeHandle).GetConstructor(
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
			null, new Type[] { typeof(ICustomTypeDescriptor), typeof(TModelElement) }, null).MethodHandle;

		/// <summary>See <see cref="ElementTypeDescriptionProvider.CreateTypeDescriptor"/>.</summary>
		protected sealed override ElementTypeDescriptor CreateTypeDescriptor(ICustomTypeDescriptor parent, ModelElement element)
		{
			return (ElementTypeDescriptor)((ConstructorInfo)ConstructorInfo.GetMethodFromHandle(TypeDescriptorConstructorHandle, TypeDescriptorTypeHandle)).Invoke(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
				null, new object[] { parent, element as TModelElement }, null);
		}
	}
}
