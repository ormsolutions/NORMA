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
using System.Reflection;
using System.Resources;
using System.Security.Permissions;
using System.Globalization;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	#region ResourceAccessor class
	/// <summary>
	/// Helper class for <see cref="ResourceAccessor{Object}"/> to create
	/// the generate ResourceAccessor class on demand.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static class ResourceAccessor
	{
		/// <summary>
		/// Retrieve a <see cref="System.Resources.ResourceManager"/> for the specified <paramref name="resourceManagerType"/>
		/// </summary>
		/// <param name="resourceManagerType">The type associated with a resource</param>
		/// <returns>The associated <see cref="System.Resources.ResourceManager"/></returns>
		public static ResourceManager GetResourceManager(Type resourceManagerType)
		{
			Type genericType = typeof(ResourceAccessor<>).MakeGenericType(resourceManagerType);
			return (ResourceManager)genericType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly | BindingFlags.Public).GetValue(null, null);
		}
	}
	/// <summary>
	/// Provides access to the singleton <see cref="ResourceManager"/> instance for <typeparamref name="TResourceManagerSource"/>.
	/// </summary>
	/// <typeparam name="TResourceManagerSource">
	/// The type from which the <see cref="System.Resources.ResourceManager"/> should be obtained.
	/// </typeparam>
	/// <remarks>
	/// If <typeparamref name="TResourceManagerSource"/> does not have a gettable <see langword="static"/> property named
	/// <c>SingletonResourceManager</c> with a return type of <see cref="System.Resources.ResourceManager"/>, a
	/// <see cref="System.Resources.ResourceManager"/> will be created by calling
	/// <see cref="System.Resources.ResourceManager(String,Assembly)"/>, passing the <see cref="Type.FullName"/> of
	/// <typeparamref name="TResourceManagerSource"/> as the first parameter and the <see cref="Type.Assembly"/> of
	/// <typeparamref name="TResourceManagerSource"/> as the second parameter.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static class ResourceAccessor<TResourceManagerSource>
	{
		/// <summary>
		/// The <see cref="System.Resources.ResourceManager"/> obtained for <typeparamref name="TResourceManagerSource"/>.
		/// </summary>
		public static ResourceManager ResourceManager
		{
			get
			{
				return resourceManager;
			}
		}
		private static readonly ResourceManager resourceManager = RetrieveResourceManager();

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
	#endregion // ResourceAccessor class
	#region PropertyDescriptor accessor classes
	/// <summary>
	/// A <see cref="CategoryAttribute"/> that uses the <see cref="ResourceAccessor"/>
	/// class to efficient retrieve a localized category.
	/// </summary>
	public sealed class ResourceAccessorCategoryAttribute : CategoryAttribute
	{
		private string myCategory;
		private Type myResourceSourceType;
		/// <summary>
		/// Create a new <see cref="ResourceAccessorCategoryAttribute"/> for the specified
		/// resource type and identifier.
		/// </summary>
		public ResourceAccessorCategoryAttribute(Type resourceType, string categoryResourceId)
			: base(categoryResourceId)
		{
			myResourceSourceType = resourceType;
			myCategory = categoryResourceId;
		}
		/// <summary>
		/// Get a localized string from the associated resource
		/// </summary>
		protected sealed override string GetLocalizedString(string value)
		{
			Type type = myResourceSourceType;
			string retVal = myCategory;
			if (type != null)
			{
				myResourceSourceType = null;
				ResourceManager manager;
				string resourceString;
				if (null != (manager = ResourceAccessor.GetResourceManager(type)) &&
					null != (resourceString = manager.GetString(retVal, CultureInfo.CurrentUICulture)))
				{
					retVal = resourceString;
				}
				myCategory = retVal;
			}
			return retVal;
		}
	}
	/// <summary>
	/// A <see cref="DescriptionAttribute"/> that uses the <see cref="ResourceAccessor"/>
	/// class to efficient retrieve a localized description.
	/// </summary>
	public sealed class ResourceAccessorDescriptionAttribute : DescriptionAttribute
	{
		private Type myResourceSourceType;
		/// <summary>
		/// Create a new <see cref="ResourceAccessorDescriptionAttribute"/> for the specified
		/// resource type and identifier.
		/// </summary>
		public ResourceAccessorDescriptionAttribute(Type resourceType, string descriptionResourceId)
			: base(descriptionResourceId)
		{
			myResourceSourceType = resourceType;
		}
		/// <summary>
		/// Get a localized string from the associated resource
		/// </summary>
		public override string Description
		{
			get
			{
				Type type = myResourceSourceType;
				string retVal = DescriptionValue;
				if (type != null)
				{
					myResourceSourceType = null;
					ResourceManager manager;
					string resourceString;
					if (null != (manager = ResourceAccessor.GetResourceManager(type)) &&
						null != (resourceString = manager.GetString(retVal, CultureInfo.CurrentUICulture)))
					{
						retVal = resourceString;
					}
					DescriptionValue = retVal;
				}
				return retVal;
			}
		}
	}
	/// <summary>
	/// A <see cref="DisplayNameAttribute"/> that uses the <see cref="ResourceAccessor"/>
	/// class to efficient retrieve a localized display name.
	/// </summary>
	public sealed class ResourceAccessorDisplayNameAttribute : DisplayNameAttribute
	{
		private Type myResourceSourceType;
		/// <summary>
		/// Create a new <see cref="ResourceAccessorDisplayNameAttribute"/> for the specified
		/// resource type and identifier.
		/// </summary>
		public ResourceAccessorDisplayNameAttribute(Type resourceType, string displayNameResourceId)
			: base(displayNameResourceId)
		{
			myResourceSourceType = resourceType;
		}
		/// <summary>
		/// Get a localized string from the associated resource
		/// </summary>
		public override string DisplayName
		{
			get
			{
				Type type = myResourceSourceType;
				string retVal = DisplayNameValue;
				if (type != null)
				{
					myResourceSourceType = null;
					ResourceManager manager;
					string resourceString;
					if (null != (manager = ResourceAccessor.GetResourceManager(type)) &&
						null != (resourceString = manager.GetString(retVal, CultureInfo.CurrentUICulture)))
					{
						retVal = resourceString;
					}
					DisplayNameValue = retVal;
				}
				return retVal;
			}
		}
	}
	#endregion // Localized PropertyDescriptor attribute classes
}
