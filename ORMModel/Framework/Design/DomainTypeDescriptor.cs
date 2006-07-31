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
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// Provides helper methods for working with <see cref="ICustomTypeDescriptor"/>s,
	/// <see cref="PropertyDescriptor"/>s, <see cref="TypeConverter"/>s, and
	/// <see cref="Attribute"/>s, as well as their DSL Tools implementations.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class DomainTypeDescriptor
	{
		#region TypeDescriptorContext class
		/// <summary>
		/// Simple implementation of <see cref="ITypeDescriptorContext"/> for a
		/// <see cref="PropertyDescriptor"/> on a <see cref="ModelElement"/>.
		/// </summary>
		[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
		public sealed class TypeDescriptorContext : ITypeDescriptorContext
		{
			private readonly ModelElement myInstance;
			private readonly PropertyDescriptor myPropertyDescriptor;

			/// <summary>
			/// Initializes a new instance of <see cref="TypeDescriptorContext"/>.
			/// </summary>
			/// <param name="component">
			/// The instance of <see cref="ModelElement"/> containing the
			/// <see cref="PropertyDescriptor"/>that this <see cref="TypeDescriptorContext"/>
			/// is for.
			/// <see langword="null"/> may be specified for this parameter.
			/// </param>
			/// <param name="propertyDescriptor">
			/// The <see cref="PropertyDescriptor"/> that this <see cref="TypeDescriptorContext"/>
			/// if for.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="propertyDescriptor"/> is <see langword="null"/>.
			/// </exception>
			public TypeDescriptorContext(ModelElement component, PropertyDescriptor propertyDescriptor)
			{
				this.myInstance = component;
				if (propertyDescriptor == null)
				{
					throw new ArgumentNullException("propertyDescriptor");
				}
				this.myPropertyDescriptor = propertyDescriptor;
			}

			/// <summary>
			/// Returns the <see cref="ModelElement"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned may be <see langword="null"/>.
			/// </summary>
			public ModelElement Instance
			{
				get
				{
					return this.myInstance;
				}
			}
			/// <summary>
			/// Returns the <see cref="ModelElement"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned may be <see langword="null"/>.
			/// </summary>
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return this.myInstance;
				}
			}
			/// <summary>
			/// Returns the <see cref="PropertyDescriptor"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned will not be <see langword="null"/>.
			/// </summary>
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this.myPropertyDescriptor;
				}
			}
			/// <summary>
			/// If <see cref="Instance"/> is not <see langword="null"/>, calls <see cref="Store.GetService"/>
			/// on the instance of <see cref="Store"/> returned by <see cref="ModelElement.Store"/>. Otherwise,
			/// returns <see langword="null"/>.
			/// </summary>
			public object GetService(Type serviceType)
			{
				ModelElement instance = myInstance;
				if (instance != null)
				{
					return instance.Store.GetService(serviceType);
				}
				return null;
			}
			/// <summary>Does nothing.</summary>
			/// <value>Always returns <see langword="null"/>.</value>
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}
			/// <summary>Does nothing.</summary>
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}
			/// <summary>Does nothing.</summary>
			/// <returns>Always returns <see langword="false"/>.</returns>
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}
		}
		#endregion // TypeDescriptorContext class

		#region GetMemberAttributes method
		/// <summary>
		/// Returns the custom <see cref="Attribute"/>s from <paramref name="memberInfo"/>.
		/// </summary>
		/// <param name="memberInfo">
		/// The <see cref="MemberInfo"/> for which the custom <see cref="Attribute"/>s are desired.
		/// </param>
		/// <returns>
		/// The custom <see cref="Attribute"/>s from <paramref name="memberInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="memberInfo"/> is <see langword="null"/>.
		/// </exception>
		public static Attribute[] GetMemberAttributes(MemberInfo memberInfo)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			object[] attributeObjects = memberInfo.GetCustomAttributes(true);
			Attribute[] attributes = new Attribute[attributeObjects.Length];
			int targetIndex = 0;
			for (int sourceIndex = 0; sourceIndex < attributeObjects.Length; sourceIndex++)
			{
				Attribute attribute = attributeObjects[sourceIndex] as Attribute;
				if (attribute != null)
				{
					attributes[targetIndex++] = attribute;
				}
			}
			if (targetIndex != attributes.Length)
			{
				Array.Resize(ref attributes, targetIndex);
			}
			return attributes;
		}
		#endregion // GetMemberAttributes method

		#region CreateTypeDescriptorContext method
		/// <summary>
		/// Creates a <see cref="TypeDescriptorContext"/> for the <see cref="DomainPropertyInfo"/>
		/// specified by <paramref name="domainPropertyInfo"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a
		/// <see cref="TypeDescriptorContext"/> should be created.
		/// <see langword="null"/> may be specified for this parameter.
		/// </param>
		/// <param name="domainPropertyInfo">
		/// The <see cref="DomainPropertyInfo"/> for which a <see cref="TypeDescriptorContext"/>
		/// should be created.
		/// </param>
		/// <returns>
		/// A <see cref="TypeDescriptorContext"/> for the <see cref="DomainPropertyInfo"/>
		/// specified by <paramref name="domainPropertyInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainPropertyInfo"/> is <see langword="null"/>.
		/// </exception>
		public static TypeDescriptorContext CreateTypeDescriptorContext(ModelElement element, DomainPropertyInfo domainPropertyInfo)
		{
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			return new TypeDescriptorContext(element, DomainTypeDescriptor.CreatePropertyDescriptor(element, domainPropertyInfo));
		}
		#endregion // CreateTypeDescriptorContext method

		#region CreatePropertyDescriptor methods
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <c>Name</c> property of <paramref name="element"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the <c>Name</c> property for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <c>Name</c> property of <paramref name="element"/>
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <paramref name="element"/> does not have a <c>Name</c> property.
		/// </exception>
		public static PropertyDescriptor CreateNamePropertyDescriptor(ModelElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			DomainPropertyInfo nameDomainPropertyInfo = element.GetDomainClass().NameDomainProperty;
			if (nameDomainPropertyInfo == null)
			{
				// Cheat and use the Exception that DSL Tools throws so that we don't need to worry about localizing it.
				try
				{
					DomainClassInfo.GetName(element);
				}
				catch (InvalidOperationException ex)
				{
					throw ex; // Yes, we want to reset the stack trace.
				}
			}
			return DomainTypeDescriptor.CreatePropertyDescriptor(element, nameDomainPropertyInfo);
		}
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> with a
		/// <see cref="DomainObjectInfo.Id"/> equal to <paramref name="domainPropertyId"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <param name="domainPropertyId">
		/// The <see cref="DomainObjectInfo.Id"/> of the <see cref="DomainPropertyInfo"/> for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> with a
		/// <see cref="DomainObjectInfo.Id"/> equal to <paramref name="domainPropertyId"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="DomainDataNotFoundException">
		/// A <see cref="DomainPropertyInfo"/> with a <see cref="DomainObjectInfo.Id"/> equal to
		/// <paramref name="domainPropertyId"/> could not be found.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, Guid domainPropertyId)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return DomainTypeDescriptor.CreatePropertyDescriptor(element, element.Store.DomainDataDirectory.GetDomainProperty(domainPropertyId));
		}
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> specified
		/// by <paramref name="domainPropertyInfo"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a <see cref="PropertyDescriptor"/>
		/// should be created.
		/// <see langword="null"/> may be specified for this parameter.
		/// </param>
		/// <param name="domainPropertyInfo">
		/// The <see cref="DomainPropertyInfo"/> for which a <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> specified
		/// by <paramref name="domainPropertyInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainPropertyInfo"/> is <see langword="null"/>.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, DomainPropertyInfo domainPropertyInfo)
		{
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			if (element != null)
			{
				ICustomTypeDescriptor typeDescriptor = TypeDescriptor.GetProvider(element).GetTypeDescriptor(element.GetType(), element);
				Type propertyType = domainPropertyInfo.PropertyType;
				string propertyName = domainPropertyInfo.Name;
				PropertyDescriptorCollection propertyDescriptors = typeDescriptor.GetProperties();
				foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
				{
					if (propertyDescriptor.PropertyType == propertyType && propertyDescriptor.Name == propertyName)
					{
						return propertyDescriptor;
					}
				}
			}
			return new ElementPropertyDescriptor(element, domainPropertyInfo, GetMemberAttributes(domainPropertyInfo.PropertyInfo));
		}
		#endregion // CreatePropertyDescriptor methods

		#region GetDisplayName methods
		/// <summary>
		/// Returns the <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="component"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="Object"/> for which the <see cref="DisplayNameAttribute.DisplayName"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="component"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="component"/> is <see langword="null"/> or does not have a <see cref="DisplayNameAttribute"/>,
		/// the <see cref="DisplayNameAttribute.DisplayName"/> for <see cref="DisplayNameAttribute.Default"/> is returned.
		/// </remarks>
		public static string GetDisplayName(object component)
		{
			return ((DisplayNameAttribute)TypeDescriptor.GetAttributes(component)[typeof(DisplayNameAttribute)]).DisplayName;
		}
		/// <summary>
		/// Returns the <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">
		/// The <see cref="Type"/> for which the <see cref="DisplayNameAttribute.DisplayName"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="componentType"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="componentType"/> is <see langword="null"/> or does not have a <see cref="DisplayNameAttribute"/>,
		/// the <see cref="DisplayNameAttribute.DisplayName"/> for <see cref="DisplayNameAttribute.Default"/> is returned.
		/// </remarks>
		public static string GetDisplayName(Type componentType)
		{
			return ((DisplayNameAttribute)TypeDescriptor.GetAttributes(componentType)[typeof(DisplayNameAttribute)]).DisplayName;
		}
		#endregion // GetDisplayName methods

		#region GetDescription methods
		/// <summary>
		/// Returns the <see cref="DescriptionAttribute.Description"/> for <paramref name="component"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="Object"/> for which the <see cref="DescriptionAttribute.Description"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DescriptionAttribute.Description"/> for <paramref name="component"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="component"/> is <see langword="null"/> or does not have a <see cref="DescriptionAttribute"/>,
		/// <see cref="DescriptionAttribute.Description"/> for <see cref="DescriptionAttribute.Default"/> will be returned.
		/// </remarks>
		public static string GetDescription(object component)
		{
			return ((DescriptionAttribute)TypeDescriptor.GetAttributes(component)[typeof(DescriptionAttribute)]).Description;
		}
		/// <summary>
		/// Returns the <see cref="DescriptionAttribute.Description"/> for <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">
		/// The <see cref="Type"/> for which the <see cref="DescriptionAttribute.Description"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DescriptionAttribute.Description"/> for <paramref name="componentType"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="componentType"/> is <see langword="null"/> or does not have a <see cref="DescriptionAttribute"/>,
		/// <see cref="DescriptionAttribute.Description"/> for <see cref="DescriptionAttribute.Default"/> will be returned.
		/// </remarks>
		public static string GetDescription(Type componentType)
		{
			return ((DescriptionAttribute)TypeDescriptor.GetAttributes(componentType)[typeof(DescriptionAttribute)]).Description;
		}
		#endregion // GetDescription methods
	}
}
