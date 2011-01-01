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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	/// <summary>
	/// Provides helper methods for working with <see cref="ICustomTypeDescriptor"/>s,
	/// <see cref="PropertyDescriptor"/>s, <see cref="TypeConverter"/>s, and
	/// <see cref="Attribute"/>s, as well as their DSL Tools implementations.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class DomainTypeDescriptor
	{
		#region TypeDescriptorContext support

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
				ModelElement instance = this.myInstance;
				if (instance != null)
				{
					return instance.Store.GetService(serviceType);
				}
				return null;
			}
			/// <summary>
			/// Returns the result of calling <see cref="GetService"/> for
			/// <see cref="IContainer"/>.
			/// </summary>
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return this.GetService(typeof(IContainer)) as IContainer;
				}
			}
			/// <summary>
			/// Attempts to notify the <see cref="IComponentChangeService"/> via
			/// <see cref="IComponentChangeService.OnComponentChanged"/>.
			/// </summary>
			void ITypeDescriptorContext.OnComponentChanged()
			{
				IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(this.myInstance, this.myPropertyDescriptor, null, null);
				}
			}
			/// <summary>
			/// Attempts to notify the <see cref="IComponentChangeService"/> via
			/// <see cref="IComponentChangeService.OnComponentChanging"/>.
			/// </summary>
			/// <returns>
			/// <see langword="true"/> if the component can be changed; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					try
					{
						componentChangeService.OnComponentChanging(this.myInstance, this.myPropertyDescriptor);
					}
					catch (CheckoutException ex)
					{
						if (ex != CheckoutException.Canceled)
						{
							throw;
						}
						return false;
					}
				}
				return true;
			}
		}
		#endregion // TypeDescriptorContext class

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

		#endregion // TypeDescriptorContext support

		#region GetRawAttributes support

		#region MemberInfoKey struct
		[Serializable]
		[StructLayout(LayoutKind.Auto)]
		[DebuggerDisplay(@"\{{System.Type.GetTypeFromHandle(DeclaringType).Name,nq}.{Name,nq} : {System.Type.GetTypeFromHandle(MemberType).Name,nq}\}")]
		private struct MemberInfoKey : IEquatable<MemberInfoKey>
		{
			public readonly string Name;
			public readonly RuntimeTypeHandle DeclaringType;
			public readonly RuntimeTypeHandle MemberType;

			public MemberInfoKey(string name, RuntimeTypeHandle declaringType, RuntimeTypeHandle memberType)
			{
				this.Name = name;
				this.DeclaringType = declaringType;
				this.MemberType = memberType;
			}

			public override bool Equals(object obj)
			{
				return obj is MemberInfoKey && this.Equals((MemberInfoKey)obj);
			}
			public bool Equals(MemberInfoKey other)
			{
				return this.DeclaringType.Value == other.DeclaringType.Value &&
					this.MemberType.Value == other.MemberType.Value &&
					this.Name == other.Name;
			}
			public override int GetHashCode()
			{
				return Utility.GetCombinedHashCode(
					(int)this.DeclaringType.Value,
					(int)this.MemberType.Value,
					this.Name.GetHashCode());
			}
		}
		#endregion // MemberInfoKey struct

		#region MemberInfoKeyEqualityComparer class
		[Serializable]
		private sealed class MemberInfoKeyEqualityComparer : EqualityComparer<MemberInfoKey>
		{
			private MemberInfoKeyEqualityComparer()
				: base()
			{
			}
			public static readonly MemberInfoKeyEqualityComparer Instance = new MemberInfoKeyEqualityComparer();
			public sealed override bool Equals(MemberInfoKey x, MemberInfoKey y)
			{
				return x.DeclaringType.Value == y.DeclaringType.Value &&
					x.MemberType.Value == y.MemberType.Value &&
					x.Name == y.Name;
			}
			public sealed override int GetHashCode(MemberInfoKey obj)
			{
				return Utility.GetCombinedHashCode(
					(int)obj.DeclaringType.Value,
					(int)obj.MemberType.Value,
					obj.Name.GetHashCode());
			}
		}
		#endregion // MemberInfoKeyEqualityComparer class

		#region GetRawAttributes(EventInfo) support

		private static readonly Dictionary<MemberInfoKey, AttributeCollection> EventInfoAttributesCache =
			new Dictionary<MemberInfoKey, AttributeCollection>(MemberInfoKeyEqualityComparer.Instance);

		#region GetRawAttributes(EventInfo) method
		/// <summary>
		/// Returns the <see cref="Attribute"/>s for <paramref name="eventInfo"/>.
		/// </summary>
		/// <param name="eventInfo">
		/// The <see cref="EventInfo"/> for which the <see cref="Attribute"/>s are desired.
		/// </param>
		/// <returns>
		/// The <see cref="Attribute"/>s for <paramref name="eventInfo"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method bypasses the normal <c>ComponentModel</c> <see cref="Attribute"/> lookup
		/// mechanisms for events, and is only intended for use by <see cref="EventDescriptor"/>
		/// implementations. For other scenarios, <see cref="Attribute"/>s should be obtained via the
		/// appropriate <see cref="EventDescriptor"/> obtained through the <see cref="TypeDescriptor"/>
		/// class.
		/// </para>
		/// <para>
		/// The <see cref="Attribute"/>s returned will include any <see cref="Attribute"/>s applied to:
		/// <list type="number">
		/// <item><description><paramref name="eventInfo"/></description></item>
		/// <item><description>
		/// any public events in the base types of the <see cref="MemberInfo.DeclaringType"/>
		/// of <paramref name="eventInfo"/> with the same <see cref="MemberInfo.Name"/> as
		/// <paramref name="eventInfo"/>
		/// </description></item>
		/// </list>
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="eventInfo"/> is <see langword="null"/>.
		/// </exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static AttributeCollection GetRawAttributes(EventInfo eventInfo)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}

			string name = eventInfo.Name;
			Type declaringType = eventInfo.DeclaringType.UnderlyingSystemType;
			Type memberType = eventInfo.EventHandlerType.UnderlyingSystemType;
			MemberInfoKey memberInfoKey = new MemberInfoKey(name, declaringType.TypeHandle, memberType.TypeHandle);

			Dictionary<MemberInfoKey, AttributeCollection> attributesCache = DomainTypeDescriptor.EventInfoAttributesCache;
			AttributeCollection attributeCollection;
			if (!attributesCache.TryGetValue(memberInfoKey, out attributeCollection))
			{
				lock (attributesCache)
				{
					if (!attributesCache.TryGetValue(memberInfoKey, out attributeCollection))
					{
						attributesCache[memberInfoKey] = attributeCollection =
							TypeDescriptor.CreateEvent(declaringType, name, memberType, null).Attributes;
					}
				}
			}
			return attributeCollection;
		}
		#endregion // GetRawAttributes(EventInfo) method
		#endregion // GetRawAttributes(EventInfo) support

		#region GetRawAttributes(PropertyInfo) support

		private static readonly Dictionary<MemberInfoKey, AttributeCollection> PropertyInfoAttributesCache =
			new Dictionary<MemberInfoKey, AttributeCollection>(MemberInfoKeyEqualityComparer.Instance);
		private static readonly Dictionary<RuntimeTypeHandle, List<MemberInfoKey>> PropertyTypeDependencies =
			DomainTypeDescriptor.InitializePropertyTypeDependencies();

		private static Dictionary<RuntimeTypeHandle, List<MemberInfoKey>> InitializePropertyTypeDependencies()
		{
			TypeDescriptor.Refreshed += TypeDescriptorRefreshed;
			return new Dictionary<RuntimeTypeHandle, List<MemberInfoKey>>(RuntimeTypeHandleComparer.Instance);
		}
		private static void TypeDescriptorRefreshed(RefreshEventArgs e)
		{
			if (e.ComponentChanged != null)
			{
				// We don't care about component changes, since they don't impact what attributes are provided for a property.
				return;
			}
			RuntimeTypeHandle typeChangedHandle = e.TypeChanged.TypeHandle;
			Dictionary<RuntimeTypeHandle, List<MemberInfoKey>> dependencies = DomainTypeDescriptor.PropertyTypeDependencies;
			if (dependencies.ContainsKey(typeChangedHandle))
			{
				Dictionary<MemberInfoKey, AttributeCollection> attributesCache = DomainTypeDescriptor.PropertyInfoAttributesCache;
				lock (attributesCache)
				{
					foreach (MemberInfoKey memberInfoKey in dependencies[typeChangedHandle])
					{
						// Clear out the attributes cache for every property that is dependent on the type that was refreshed.
						attributesCache[memberInfoKey] = null;
					}
				}
			}
		}

		#region GetRawAttributes(PropertyInfo) method
		/// <summary>
		/// Returns the <see cref="Attribute"/>s for <paramref name="propertyInfo"/>.
		/// </summary>
		/// <param name="propertyInfo">
		/// The <see cref="PropertyInfo"/> for which the <see cref="Attribute"/>s are desired.
		/// </param>
		/// <returns>
		/// The <see cref="Attribute"/>s for <paramref name="propertyInfo"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method bypasses the normal <c>ComponentModel</c> <see cref="Attribute"/> lookup
		/// mechanisms for properties, and is only intended for use by <see cref="PropertyDescriptor"/>
		/// implementations. For other scenarios, <see cref="Attribute"/>s should be obtained via the
		/// appropriate <see cref="PropertyDescriptor"/> obtained through the <see cref="TypeDescriptor"/>
		/// class.
		/// </para>
		/// <para>
		/// The <see cref="Attribute"/>s returned will include any <see cref="Attribute"/>s applied to:
		/// <list type="number">
		/// <item><description><paramref name="propertyInfo"/></description></item>
		/// <item><description>
		/// any properties in the base types of the <see cref="MemberInfo.DeclaringType"/>
		/// of <paramref name="propertyInfo"/> with the same <see cref="MemberInfo.Name"/> as
		/// <paramref name="propertyInfo"/>
		/// </description></item>
		/// <item><description>
		/// the <see cref="PropertyInfo.PropertyType"/> of <paramref name="propertyInfo"/>
		/// </description></item>
		/// <item><description>
		/// the base types of the <see cref="PropertyInfo.PropertyType"/> of
		/// <paramref name="propertyInfo"/> 
		/// </description></item>
		/// </list>
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="propertyInfo"/> is <see langword="null"/>.
		/// </exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		[ReflectionPermission(SecurityAction.LinkDemand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public static AttributeCollection GetRawAttributes(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}

			string name = propertyInfo.Name;
			Type declaringType = propertyInfo.DeclaringType.UnderlyingSystemType;
			Type memberType = propertyInfo.PropertyType.UnderlyingSystemType;
			MemberInfoKey memberInfoKey = new MemberInfoKey(name, declaringType.TypeHandle, memberType.TypeHandle);

			Dictionary<MemberInfoKey, AttributeCollection> attributesCache = DomainTypeDescriptor.PropertyInfoAttributesCache;
			AttributeCollection attributeCollection;
			if (!attributesCache.TryGetValue(memberInfoKey, out attributeCollection) || attributeCollection == null)
			{
				lock (attributesCache)
				{
					// If there is an entry in the cache for this property, then the dependencies for it
					// have already been calculated previously.
					bool dependenciesAlreadyCalculated = attributesCache.TryGetValue(memberInfoKey, out attributeCollection);
					if (!dependenciesAlreadyCalculated || attributeCollection == null)
					{
						// This property either hasn't had its attributes retrieved, or one of the types on which
						// it is dependent has been refreshed since the attributes were last retrieved. Either way,
						// we need to recalculate the attributes for this property.
						attributesCache[memberInfoKey] = attributeCollection =
							TypeDescriptor.CreateProperty(declaringType, name, memberType, null).Attributes;

						if (!dependenciesAlreadyCalculated)
						{
							// Since we haven't calculated the dependencies of this property before, we need to do
							// so now so that we know what properties to update when the TypeDescriptor.Refreshed
							// event is raised.
							Dictionary<RuntimeTypeHandle, List<MemberInfoKey>> dependencies = DomainTypeDescriptor.PropertyTypeDependencies;
							while (memberType != null)
							{
								RuntimeTypeHandle memberTypeHandle = memberType.TypeHandle;
								List<MemberInfoKey> dependentProperties;
								if (!dependencies.TryGetValue(memberTypeHandle, out dependentProperties))
								{
									dependencies[memberTypeHandle] = dependentProperties = new List<MemberInfoKey>();
								}

								dependentProperties.Add(memberInfoKey);
								memberType = memberType.BaseType;
							}
						}
					}
				}
			}
			return attributeCollection;
		}
		#endregion // GetRawAttributes(PropertyInfo) method
		#endregion // GetRawAttributes(PropertyInfo) support
		#endregion // GetRawAttributes support

		#region CreateNamePropertyDescriptor method
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
		#endregion // CreateNamePropertyDescriptor method

		#region CreatePropertyDescriptor methods
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> or
		/// <see cref="DomainRoleInfo"/> with a <see cref="DomainObjectInfo.Id"/> equal to
		/// <paramref name="domainPropertyOrRoleId"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <param name="domainPropertyOrRoleId">
		/// The <see cref="DomainObjectInfo.Id"/> of the <see cref="DomainPropertyInfo"/> or
		/// <see cref="DomainRoleInfo"/> for which a <see cref="PropertyDescriptor"/> should
		/// be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> or
		/// <see cref="DomainRoleInfo"/> with a <see cref="DomainObjectInfo.Id"/> equal to
		/// <paramref name="domainPropertyOrRoleId"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="DomainDataNotFoundException">
		/// Neither a <see cref="DomainPropertyInfo"/> nor a <see cref="DomainRoleInfo"/>
		/// with a <see cref="DomainObjectInfo.Id"/> equal to
		/// <paramref name="domainPropertyOrRoleId"/> could be found.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, Guid domainPropertyOrRoleId)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			DomainDataDirectory domainDataDirectory = element.Store.DomainDataDirectory;
			DomainPropertyInfo domainPropertyInfo = domainDataDirectory.FindDomainProperty(domainPropertyOrRoleId);
			if (domainPropertyInfo != null)
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(element, domainPropertyInfo);
			}
			else
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(element, domainDataDirectory.GetDomainRole(domainPropertyOrRoleId));
			}
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
			Type propertyType = domainPropertyInfo.PropertyType;
			string propertyName = domainPropertyInfo.Name;
			Guid domainPropertyId = domainPropertyInfo.Id;
			PropertyDescriptor propertyDescriptor;
			Type elementType;

			if (element != null)
			{
				// Try the default type descriptor for the element (including custom type descriptors).
				propertyDescriptor = DomainTypeDescriptor.FindPropertyDescriptorInternal(TypeDescriptor.GetProperties(element, false), propertyType, propertyName, domainPropertyId);
				if (propertyDescriptor != null)
				{
					return propertyDescriptor;
				}
				// Try the default type descriptor for the element (excluding custom type descriptors, in case it is filtering out the property we want).
				propertyDescriptor = DomainTypeDescriptor.FindPropertyDescriptorInternal(TypeDescriptor.GetProperties(element, true), propertyType, propertyName, domainPropertyId);
				if (propertyDescriptor != null)
				{
					return propertyDescriptor;
				}
				elementType = element.GetType();
			}
			else
			{
				elementType = domainPropertyInfo.DomainClass.ImplementationClass;
			}

			// Try the default type descriptor for the Type of the element.

			// The following line is never returning in VS2010 (ValueRange.MinInclusion as
			// as a sample, but others are likely to experience the same behavior). This
			// will skip custom descriptors and return reflected property descriptors anyway,
			// so we just create the specific property descriptor directly.
			// propertyDescriptor = DomainTypeDescriptor.FindPropertyDescriptorInternal(TypeDescriptor.GetProperties(elementType), propertyType, propertyName, domainPropertyId);
			propertyDescriptor = TypeDescriptor.CreateProperty(elementType, propertyName, propertyType);
			if (propertyDescriptor != null)
			{
				return propertyDescriptor;
			}

			// GetRawAttributes(PropertyInfo) has a LinkDemand for ReflectionPermission with MemberAccess. In order to avoid any security
			// issues, we do a full Demand for the same permission here. We could have instead put the same LinkDemand on this method,
			// but in the majority of cases this permission won't actually be necessary. By doing it this way, callers without this
			// permission can still successfully use this method as long as the PropertyDescriptor is found before this point.
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();

			// Since we couldn't find a PropertyDescriptor, we'll have to construct one ourselves.
			return new ElementPropertyDescriptor(element, domainPropertyInfo, EditorUtility.GetAttributeArray(DomainTypeDescriptor.GetRawAttributes(domainPropertyInfo.PropertyInfo)));
		}
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainRoleInfo"/> specified
		/// by <paramref name="domainRoleInfo"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> playing the <see cref="DomainRoleInfo"/> specified by 
		/// <paramref name="domainRoleInfo"/> for which a <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <param name="domainRoleInfo">
		/// The <see cref="DomainRoleInfo"/> for which a <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainRoleInfo"/> specified by
		/// <paramref name="domainRoleInfo"/> played by the <see cref="ModelElement"/> specified by
		/// <paramref name="element"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// /// <exception cref="ArgumentNullException">
		/// <paramref name="domainRoleInfo"/> is <see langword="null"/>.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, DomainRoleInfo domainRoleInfo)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (domainRoleInfo == null)
			{
				throw new ArgumentNullException("domainRoleInfo");
			}
			PropertyInfo linkPropertyInfo = domainRoleInfo.LinkPropertyInfo;
			Type propertyType = linkPropertyInfo.PropertyType;
			string propertyName = domainRoleInfo.PropertyName;
			DomainRoleInfo oppositeDomainRoleInfo = domainRoleInfo.OppositeDomainRole;
			Guid oppositeDomainRoleId = oppositeDomainRoleInfo.Id;
			PropertyDescriptor propertyDescriptor;

			// Try the default type descriptor for the element (including custom type descriptors).
			propertyDescriptor = DomainTypeDescriptor.FindRolePlayerPropertyDescriptorInternal(TypeDescriptor.GetProperties(element, false), propertyType, propertyName, oppositeDomainRoleId);
			if (propertyDescriptor != null)
			{
				return propertyDescriptor;
			}
			// Try the default type descriptor for the element (excluding custom type descriptors, in case it is filtering out the property we want).
			propertyDescriptor = DomainTypeDescriptor.FindRolePlayerPropertyDescriptorInternal(TypeDescriptor.GetProperties(element, true), propertyType, propertyName, oppositeDomainRoleId);
			if (propertyDescriptor != null)
			{
				return propertyDescriptor;
			}
			// Try the default type descriptor for the Type of the element.
			propertyDescriptor = DomainTypeDescriptor.FindRolePlayerPropertyDescriptorInternal(TypeDescriptor.GetProperties(element.GetType()), propertyType, propertyName, oppositeDomainRoleId);
			if (propertyDescriptor != null)
			{
				return propertyDescriptor;
			}

			// GetRawAttributes(PropertyInfo) has a LinkDemand for ReflectionPermission with MemberAccess. In order to avoid any security
			// issues, we do a full Demand for the same permission here. We could have instead put the same LinkDemand on this method,
			// but in the majority of cases this permission won't actually be necessary. By doing it this way, callers without this
			// permission can still successfully use this method as long as the PropertyDescriptor is found before this point.
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();

			// Since we couldn't find a PropertyDescriptor, we'll have to construct one ourselves.
			return new RolePlayerPropertyDescriptor(element, oppositeDomainRoleInfo, EditorUtility.GetAttributeArray(DomainTypeDescriptor.GetRawAttributes(linkPropertyInfo)));
		}
		#endregion // CreatePropertyDescriptor methods

		#region FindPropertyDescriptor methods
		/// <summary>
		/// Attempts to locate a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/>
		/// specified by <paramref name="domainPropertyInfo"/> in the <see cref="PropertyDescriptorCollection"/>
		/// specified by <paramref name="propertyDescriptors"/>.
		/// </summary>
		/// <param name="propertyDescriptors">
		/// The <see cref="PropertyDescriptorCollection"/> in which to search for a <see cref="PropertyDescriptor"/>
		/// for the <see cref="DomainPropertyInfo"/> specified by <paramref name="domainPropertyInfo"/>.
		/// </param>
		/// <param name="domainPropertyInfo">
		/// The <see cref="DomainPropertyInfo"/> for which a <see cref="PropertyDescriptor"/> should be searched for
		/// in the <see cref="PropertyDescriptorCollection"/> specified by <paramref name="propertyDescriptors"/>.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> specified by
		/// <paramref name="domainPropertyInfo"/> if one could be located in the <see cref="PropertyDescriptorCollection"/>
		/// specified by <paramref name="propertyDescriptors"/>; otherwise, <see langword="null"/>.
		/// </returns>
		public static PropertyDescriptor FindPropertyDescriptor(PropertyDescriptorCollection propertyDescriptors, DomainPropertyInfo domainPropertyInfo)
		{
			if (propertyDescriptors == null)
			{
				throw new ArgumentNullException("propertyDescriptors");
			}
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			return DomainTypeDescriptor.FindPropertyDescriptorInternal(propertyDescriptors, domainPropertyInfo.PropertyType, domainPropertyInfo.Name, domainPropertyInfo.Id);
		}
		/// <summary>
		/// Attempts to locate a <see cref="PropertyDescriptor"/> for the <see cref="DomainRoleInfo"/>
		/// specified by <paramref name="domainRoleInfo"/> in the <see cref="PropertyDescriptorCollection"/>
		/// specified by <paramref name="propertyDescriptors"/>.
		/// </summary>
		/// <param name="propertyDescriptors">
		/// The <see cref="PropertyDescriptorCollection"/> in which to search.
		/// </param>
		/// <param name="domainRoleInfo">
		/// The <see cref="DomainRoleInfo"/> for which a <see cref="PropertyDescriptor"/> should be searched for
		/// in the <see cref="PropertyDescriptorCollection"/> specified by <paramref name="propertyDescriptors"/>.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainRoleInfo"/> specified by
		/// <paramref name="domainRoleInfo"/> if one could be located in the <see cref="PropertyDescriptorCollection"/>
		/// specified by <paramref name="propertyDescriptors"/>; otherwise, <see langword="null"/>.
		/// </returns>
		public static PropertyDescriptor FindPropertyDescriptor(PropertyDescriptorCollection propertyDescriptors, DomainRoleInfo domainRoleInfo)
		{
			if (propertyDescriptors == null)
			{
				throw new ArgumentNullException("propertyDescriptors");
			}
			if (domainRoleInfo == null)
			{
				throw new ArgumentNullException("domainRoleInfo");
			}
			return DomainTypeDescriptor.FindRolePlayerPropertyDescriptorInternal(propertyDescriptors, domainRoleInfo.LinkPropertyInfo.PropertyType, domainRoleInfo.PropertyName, domainRoleInfo.OppositeDomainRole.Id);
		}
		private static PropertyDescriptor FindPropertyDescriptorInternal(PropertyDescriptorCollection propertyDescriptors, Type propertyType, string propertyName, Guid domainPropertyId)
		{
			foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
			{
				ElementPropertyDescriptor elementPropertyDescriptor = propertyDescriptor as ElementPropertyDescriptor;
				if ((elementPropertyDescriptor != null && elementPropertyDescriptor.DomainPropertyInfo.Id == domainPropertyId) ||
					(propertyDescriptor.PropertyType == propertyType && propertyDescriptor.Name == propertyName))
				{
					return propertyDescriptor;
				}
			}
			return null;
		}
		private static PropertyDescriptor FindRolePlayerPropertyDescriptorInternal(PropertyDescriptorCollection propertyDescriptors, Type propertyType, string propertyName, Guid oppositeDomainRoleId)
		{
			foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
			{
				RolePlayerPropertyDescriptor rolePlayerPropertyDescriptor = propertyDescriptor as RolePlayerPropertyDescriptor;
				if ((rolePlayerPropertyDescriptor != null && rolePlayerPropertyDescriptor.DomainRoleInfo.Id == oppositeDomainRoleId) ||
					(propertyDescriptor.PropertyType == propertyType && propertyDescriptor.Name == propertyName))
				{
					return propertyDescriptor;
				}
			}
			return null;
		}
		#endregion // FindPropertyDescriptor methods

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
