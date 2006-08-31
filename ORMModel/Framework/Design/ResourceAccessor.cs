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
using System.Reflection;
using System.Resources;
using System.Security.Permissions;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// Provides access to the singleton <see cref="ResourceManager"/> instance for <typeparamref name="TResourceManagerSource"/>.
	/// </summary>
	/// <typeparam name="TResourceManagerSource">
	/// The type from which the <see cref="T:ResourceManager"/> should be obtained.
	/// </typeparam>
	/// <remarks>
	/// If <typeparamref name="TResourceManagerSource"/> does not have a gettable <see langword="static"/> property named
	/// <c>SingletonResourceManager</c> with a return type of <see cref="T:ResourceManager"/>, a <see cref="T:ResourceManager"/>
	/// will be created by calling <see cref="T:ResourceManager(String,Assembly)"/>, passing the <see cref="Type.FullName"/> of
	/// <typeparamref name="TResourceManagerSource"/> as the first parameter and the <see cref="Type.Assembly"/> of
	/// <typeparamref name="TResourceManagerSource"/> as the second parameter.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static class ResourceAccessor<TResourceManagerSource>
	{
		/// <summary>
		/// The <see cref="T:ResourceManager"/> obtained for <typeparamref name="TResourceManagerSource"/>.
		/// </summary>
		public static readonly ResourceManager ResourceManager = RetrieveResourceManager();

		private static ResourceManager RetrieveResourceManager()
		{
			const string ResourceManagerPropertyName = "SingletonResourceManager";

			Type sourceType = typeof(TResourceManagerSource);

			PropertyInfo propertyInfo = sourceType.GetProperty(ResourceManagerPropertyName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy,
				null, typeof(ResourceManager), Type.EmptyTypes, null);

			return (propertyInfo != null ? (ResourceManager)propertyInfo.GetValue(null, null) : null) ?? new ResourceManager(sourceType.FullName, sourceType.Assembly);
		}
	}
}
