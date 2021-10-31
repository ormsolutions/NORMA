#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ReferenceModeNamingUse enum
	/// <summary>
	/// Determine the target usage for <see cref="ReferenceModeNaming"/> settings
	/// </summary>
	public enum ReferenceModeNamingUse
	{
		/// <summary>
		/// The naming pattern is used for references to the EntityType
		/// with the given reference mode.
		/// </summary>
		ReferenceToEntityType,
		/// <summary>
		/// The naming pattern is used when a ValueType matching the
		/// reference mode pattern is used as a simple primary identifier column.
		/// </summary>
		PrimaryIdentifier,
	}
	#endregion // ReferenceModeNamingUse enum
	#region DefaultReferenceModeNamingOwnerAttribute class
	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to find
	/// an existing <see cref="DefaultReferenceModeNaming"/> instance based on context information.
	/// </summary>
	/// <param name="component">The context instance used to resolve the default instance.</param>
	/// <param name="referenceModeType">The type of reference mode to get the default for.</param>
	/// <param name="findNearest">If there is a hierarchy of naming defaults always find the nearest (returning null if not found).</param>
	/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
	/// generally the parent of the instance. This is used as a reference by the callback, not the calling code. If this is null, then the
	/// component is a context instance and global state should never be used to infer an instance.</param>
	/// <returns>An existing DefaultReferenceModeNaming instance.</returns>
	public delegate DefaultReferenceModeNaming GetDefaultReferenceModeNamingFromComponent(object component, ReferenceModeType referenceModeType, bool findNearest, Type defaultNamingLinkType);

	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to find
	/// and existing <see cref="ReferenceModeNaming"/> instance based on context information.
	/// </summary>
	/// <param name="contextInstance">Allow binding to a specific instance without use of global state. For resolution using
	/// the type information the model must have a single instance identifiable from the link and hint information. However,
	/// during save all name generation instances will be accessed, not just the global defaults. This will be null for
	/// a global request and set for an instance request, taking precedent of the following type information.</param>
	/// <param name="objectType">The object type to resolve from.</param>
	/// <param name="findNearest">If there is a hierarchy of naming owners always find the nearest (returning null if not found).</param>
	/// <param name="namingLinkType">A link type indicating the type of relationship. This is interpreted by the callback only.</param>
	/// <returns>The corresponding <see cref="ReferenceModeNaming"/> instance.</returns>
	public delegate ReferenceModeNaming GetReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, bool findNearest, Type namingLinkType);

	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to find
	/// the nearest <see cref="DefaultReferenceModeNaming"/> instance for an <see cref="ObjectType"/> with context information.
	/// </summary>
	/// <param name="contextInstance">Allow binding to a specific instance without use of global state. For resolution using
	/// the type information the model must have a single instance identifiable from the link and hint information. However,
	/// during save an name generation all instances will be accessed, not just the global defaults. This will be null for
	/// a global request and set for an instance request, taking precedent of the following type information. Note that the
	/// context object is not necessarily the returned owner element.</param>
	/// <param name="objectType">The object type to resolve from.</param>
	/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
	/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
	/// <param name="namingLinkType">A link associated with a possible ReferenceModeNaming instance. This is interpreted by the callback.</param>
	/// <returns>A DefaultReferenceModeNaming instance</returns>
	public delegate DefaultReferenceModeNaming GetDefaultReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, Type defaultNamingLinkType, Type namingLinkType);

	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to find
	/// a context <see cref="DefaultReferenceModeNaming"/> for an instance associated with a given
	/// link type. This is a static supplement to <see cref="DefaultReferenceModeNaming.ContextDefaultReferenceModeNaming"/>,
	/// which is used when an instance is known.
	/// </summary>
	/// <param name="component">The context instance used to resolve default naming instance.</param>
	/// <param name="referenceModeType">The type of reference mode to get the default for.</param>
	/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
	/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
	/// <returns>Return null if there is only one level of defaults, otherwise the instance of the nearest existing context default.</returns>
	public delegate DefaultReferenceModeNaming GetContextDefaultReferenceModeNaming(object component, ReferenceModeType referenceModeType, Type defaultNamingLinkType);

	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to find
	/// a context <see cref="ReferenceModeNaming"/> for an instance associated with a given <see cref="ObjectType"/> and
	/// reference link type. This is a static supplement to <see cref="ReferenceModeNaming.ContextReferenceModeNaming"/>,
	/// which is used when an instance is known.
	/// </summary>
	/// <param name="objectType">The object type the naming customization applies to.</param>
	/// <param name="namingLinkType">The link type used to distinguish this instance.</param>
	/// <returns>Return null if there is only one level of defaults, otherwise the instance of the nearest existing context default.</returns>
	public delegate ReferenceModeNaming GetContextReferenceModeNaming(ObjectType objectType, Type namingLinkType);

	/// <summary>
	/// Delegate used with the <see cref="DefaultReferenceModeNamingOwnerAttribute"/> to add a suffix to property
	/// descriptor names so that both a global and context-specific version of a reference mode naming property can
	/// be used in the same set of properties.
	/// </summary>
	/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
	/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
	/// <param name="namingLinkType">The type of a link from the <see cref="ReferenceModeNaming"/> instance to an associated instance,
	/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
	/// <returns>String suffix.</returns>
	/// <remarks>If this is for a default instance then the <paramref name="namingLinkType"/> will not be set.</remarks>
	public delegate string GetPropertyDescriptorSuffix(Type defaultNamingLinkType, Type namingLinkType);

	/// <summary>
	/// Provide owner resolution for <see cref="DefaultReferenceModeNaming"/> instantiation based on context information.
	/// This allows property descriptors with no current backing instances to accurate display default values and create
	/// new instances on demand.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class DefaultReferenceModeNamingOwnerAttribute : Attribute
	{
		private string myDRMNFromComponentCallbackName;
		private string myRMNFromObjectTypeCallbackName;
		private string myDRMNFromObjectTypeCallbackName;
		private string myContextDRMNCallbackName;
		private string myContextRMNCallbackName;
		private string myPropertySuffixCallbackName;

		/// <summary>
		/// Create a new <see cref="DefaultReferenceModeNamingOwnerAttribute"/>
		/// </summary>
		/// <param name="getDefaultReferenceModeNamingFromComponentName">The name of a static method matching the <see cref="GetDefaultReferenceModeNamingFromComponent"/>
		/// signature that is implemented on the class this attribute is attached to. The function should return an instance of the hosting class type.</param>
		/// <param name="getReferenceModeNamingFromObjectTypeCallbackName">The name of a static method matching the <see cref="GetReferenceModeNamingFromObjectType"/>
		/// signature that is implemented on the singleton container type. This translates an object type and context information into a <see cref="DefaultReferenceModeNaming"/> instance.</param>
		/// <param name="getDefaultReferenceModeNamingFromObjectTypeCallbackName">The name of a static method matching the <see cref="GetDefaultReferenceModeNamingFromObjectType"/>
		/// signature that is implemented on the singleton container type. This translates an object type and context information into a <see cref="ReferenceModeNaming"/> instance.</param>
		/// <param name="getContextDefaultReferenceModeNaming">The name of a static method matching the <see cref="GetContextDefaultReferenceModeNaming"/>
		/// signature that is implemented on the singleton container type. This translates the distinguishing link type of a more specific default set into a context (generally global) default.</param>
		/// <param name="getContextReferenceModeNaming">The name of a static method matching the <see cref="GetContextReferenceModeNaming"/>
		/// signature that is implemented on the singleton container type. This translates the distinguishing link type of a more specific default set into a more general ReferenceModeNaming instance for the object type.</param>
		/// <param name="getPropertyDescriptorSuffixCallbackName">The name of a static method matching the <see cref="GetPropertyDescriptorSuffix"/>
		/// signature that is implemented on the singleton container type. This returns a suffix to add to property descriptors for cases where
		/// multiple sets of reference mode naming properties appear on the same object. Note that 'other sets' may actually come from a different
		/// extension, so this should always be populated.</param>

		public DefaultReferenceModeNamingOwnerAttribute(string getDefaultReferenceModeNamingFromComponentName, string getReferenceModeNamingFromObjectTypeCallbackName, string getDefaultReferenceModeNamingFromObjectTypeCallbackName, string getContextDefaultReferenceModeNaming, string getContextReferenceModeNaming, string getPropertyDescriptorSuffixCallbackName)
		{
			myDRMNFromComponentCallbackName = getDefaultReferenceModeNamingFromComponentName;
			myRMNFromObjectTypeCallbackName = getReferenceModeNamingFromObjectTypeCallbackName;
			myDRMNFromObjectTypeCallbackName = getDefaultReferenceModeNamingFromObjectTypeCallbackName;
			myContextDRMNCallbackName = getContextDefaultReferenceModeNaming;
			myContextRMNCallbackName = getContextReferenceModeNaming;
			myPropertySuffixCallbackName = getPropertyDescriptorSuffixCallbackName;
		}

		/// <summary>
		/// The static function to return the <see cref="DefaultReferenceModeNaming"/> instance from a context component.
		/// </summary>
		public string DefaultReferenceModeNamingFromComponentCallbackName
		{
			get
			{
				return myDRMNFromComponentCallbackName;
			}
		}

		/// <summary>
		/// The static function to return the <see cref="ReferenceModeNaming"/> instance from an <see cref="ObjectType"/>.
		/// </summary>
		public string ReferenceModeNamingFromObjectTypeCallbackName
		{
			get
			{
				return myRMNFromObjectTypeCallbackName;
			}
		}

		/// <summary>
		/// The static function to return the <see cref="DefaultReferenceModeNaming"/> instance from an <see cref="ObjectType"/>.
		/// </summary>
		public string DefaultReferenceModeNamingFromObjectTypeCallbackName
		{
			get
			{
				return myDRMNFromObjectTypeCallbackName;
			}
		}
		/// <summary>
		/// The static function to return the <see cref="DefaultReferenceModeNaming"/> context based on child link type.
		/// </summary>
		public string ContextDefaultReferenceModeNamingCallbackName
		{
			get
			{
				return myContextDRMNCallbackName;
			}
		}

		/// <summary>
		/// The static function to return the <see cref="ReferenceModeNaming"/> context based on child link type.
		/// </summary>
		public string ContextReferenceModeNamingCallbackName
		{
			get
			{
				return myContextRMNCallbackName;
			}
		}

		/// <summary>
		/// The static function name to get the suffix for property descriptors.
		/// </summary>
		public string PropertyDescriptorSuffixCallbackName
		{
			get
			{
				return myPropertySuffixCallbackName;
			}
		}
	}
	#endregion // DefaultReferenceModeNamingOwnerAttribute class

	partial class ReferenceModeNaming
	{
		#region Abstract Extension Points
		/// <summary>
		/// Get or set the object type these name settings are used for.
		/// </summary>
		public abstract ObjectType ResolvedObjectType { get; set; }
		/// <summary>
		/// Attach the context elements to an instance dynamically created by a property descriptor.
		/// </summary>
		/// <param name="namingLinkType">The callback-interpret reference type indicating how to attach the instance.</param>
		public abstract void AttachDynamicInstance(Type namingLinkType);
		/// <summary>
		/// Resolve an owner given an object type.
		/// </summary>
		public abstract DefaultReferenceModeNaming ResolveDefaultReferenceModeNaming();
		#endregion // Abstract Extension Points
		#region Instance Resolution Helper
		/// <summary>
		/// Helper functions to resolve different instance associated with reference mode naming
		/// </summary>
		/// <typeparam name="DRMN">The <see cref="DefaultReferenceModeNaming"/> type that sources these functions.</typeparam>
		protected static class InstanceResolver<DRMN>
			where DRMN : DefaultReferenceModeNaming
		{
			private static GetDefaultReferenceModeNamingFromComponent myDRMNFromComponentCallback;
			private static GetReferenceModeNamingFromObjectType myRMNFromObjectTypeCallback;
			private static GetDefaultReferenceModeNamingFromObjectType myDRMNFromObjectType;
			private static GetContextDefaultReferenceModeNaming myContextDRMNCallback;
			private static GetContextReferenceModeNaming myContextRMNCallback;
			private static GetPropertyDescriptorSuffix myPropertySuffixCallback;
			/// <summary>
			/// Find the <see cref="DefaultReferenceModeNaming"/> instance for the given component.
			/// </summary>
			/// <param name="component">The context object, such as comes from a selected object.</param>
			/// <param name="referenceModeType">The reference mode type that matches the default settings.</param>
			/// <param name="findNearest">If there are multiple levels of default settings, only match the closest
			/// one as indicated by the <paramref name="defaultNamingLinkType"/>.</param>
			/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
			/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
			/// <returns>The matching instance if one exists.</returns>
			public static DefaultReferenceModeNaming ResolveDefaultReferenceModeNamingFromComponent(object component, ReferenceModeType referenceModeType, bool findNearest, Type defaultNamingLinkType)
			{
				GetDefaultReferenceModeNamingFromComponent callback = myDRMNFromComponentCallback;
				if (callback == null)
				{
					BindCallbacks();
					callback = myDRMNFromComponentCallback;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(component, referenceModeType, findNearest, defaultNamingLinkType);
			}
			/// <summary>
			/// Get an existing <see cref="ReferenceModeNaming"/> instance based on context information.
			/// </summary>
			/// <param name="contextInstance">Allow binding to a specific instance without use of global state. For resolution using
			/// the type information the model must have a single instance identifiable from the link and hint information. However,
			/// during save all name generation instances will be accessed, not just the global defaults. This will be null for
			/// a global request and set for an instance request, taking precedent of the following type information.</param>
			/// <param name="objectType">The object type to resolve from.</param>
			/// <param name="findNearest">If there is a hierarchy of naming owners always find the nearest (returning null if not found).</param>
			/// <param name="namingLinkType">A link type indicating the type of relationship. This is interpreted by the callback only.</param>
			/// <returns>The corresponding <see cref="ReferenceModeNaming"/> instance.</returns>
			public static ReferenceModeNaming ResolveReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, bool findNearest, Type namingLinkType)
			{
				GetReferenceModeNamingFromObjectType callback = myRMNFromObjectTypeCallback;
				if (callback == null)
				{
					BindCallbacks();
					callback = myRMNFromObjectTypeCallback;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(contextInstance, objectType, findNearest, namingLinkType);
			}
			/// <summary>
			/// Get the nearest <see cref="DefaultReferenceModeNaming"/> instance for an <see cref="ObjectType"/> with context information.
			/// </summary>
			/// <param name="contextInstance">Allow binding to a specific instance without use of global state. For resolution using
			/// the type information the model must have a single instance identifiable from the link and hint information. However,
			/// during save an name generation all instances will be accessed, not just the global defaults. This will be null for
			/// a global request and set for an instance request, taking precedent of the following type information. Note that the
			/// context object is not necessarily the returned owner element.</param>
			/// <param name="objectType">The object type to resolve from.</param>
			/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
			/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
			/// <param name="namingLinkType">A link associated with a possible ReferenceModeNaming instance. This is interpreted by the callback.</param>
			/// <returns>A DefaultReferenceModeNaming instance if one is available.</returns>
			public static DefaultReferenceModeNaming ResolveDefaultReferenceModeNamingFromObjectType(ModelElement contextInstance, ObjectType objectType, Type defaultNamingLinkType, Type namingLinkType)
			{
				GetDefaultReferenceModeNamingFromObjectType callback = myDRMNFromObjectType;
				if (callback == null)
				{
					BindCallbacks();
					callback = myDRMNFromObjectType;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(contextInstance, objectType, defaultNamingLinkType, namingLinkType);
			}
			/// <summary>
			/// Get a context <see cref="DefaultReferenceModeNaming"/> for an instance associated with a given
			/// link type. This is a static supplement to <see cref="DefaultReferenceModeNaming.ContextDefaultReferenceModeNaming"/>,
			/// which is used when an instance is known.
			/// </summary>
			/// <param name="component">The context instance used to resolve the default naming instance.</param>
			/// <param name="referenceModeType">The type of reference mode to get the default for.</param>
			/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
			/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
			/// <returns>Return null if there is only one level of defaults, otherwise the instance of the nearest existing context default.</returns>
			public static DefaultReferenceModeNaming ResolveContextDefaultReferenceModeNaming(object component, ReferenceModeType referenceModeType, Type defaultNamingLinkType)
			{
				GetContextDefaultReferenceModeNaming callback = myContextDRMNCallback;
				if (callback == null)
				{
					BindCallbacks();
					callback = myContextDRMNCallback;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(component, referenceModeType, defaultNamingLinkType);
			}
			/// <summary>
			/// Get a context <see cref="ReferenceModeNaming"/> for an instance associated with a given <see cref="ObjectType"/> and
			/// reference link type. This is a static supplement to <see cref="ReferenceModeNaming.ContextReferenceModeNaming"/>,
			/// which is used when an instance is known.
			/// </summary>
			/// <param name="objectType">The object type the naming customization applies to.</param>
			/// <param name="namingLinkType">The link type used to distinguish this instance.</param>
			/// <returns>Return null if there is only one level of defaults, otherwise the instance of the nearest existing context default.</returns>
			public static ReferenceModeNaming ResolveContextReferenceModeNaming(ObjectType objectType, Type namingLinkType)
			{
				GetContextReferenceModeNaming callback = myContextRMNCallback;
				if (callback == null)
				{
					BindCallbacks();
					callback = myContextRMNCallback;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(objectType, namingLinkType);
			}
			/// <summary>
			/// Get a suffix suffix for a property descriptor to distinguish naming sets from different extensions as well
			/// as global and context-specific naming options from the same extension. This allos multiple reference mode naming
			/// instances to be used in the same property grid.
			/// </summary>
			/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
			/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
			/// <param name="namingLinkType">The type of a link from the <see cref="ReferenceModeNaming"/> instance to an associated instance,
			/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
			/// <returns>String suffix.</returns>
			/// <remarks>If this is for a default instance then the <paramref name="namingLinkType"/> will not be set.</remarks>
			public static string GetPropertySuffix(Type defaultNamingLinkType, Type namingLinkType)
			{
				GetPropertyDescriptorSuffix callback = myPropertySuffixCallback;
				if (callback == null)
				{
					BindCallbacks();
					callback = myPropertySuffixCallback;
					if (callback == null)
					{
						return null;
					}
				}
				return callback(defaultNamingLinkType, namingLinkType);
			}
			private static void BindCallbacks()
			{
				Type drmnType = typeof(DRMN);
				object[] attrs = drmnType.GetCustomAttributes(typeof(DefaultReferenceModeNamingOwnerAttribute), false);
				if (attrs.Length != 0)
				{
					DefaultReferenceModeNamingOwnerAttribute attr = (DefaultReferenceModeNamingOwnerAttribute)attrs[0];
					MethodInfo methodInfo = drmnType.GetMethod(attr.DefaultReferenceModeNamingFromComponentCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(object), typeof(ReferenceModeType), typeof(bool), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myDRMNFromComponentCallback, (GetDefaultReferenceModeNamingFromComponent)Delegate.CreateDelegate(typeof(GetDefaultReferenceModeNamingFromComponent), methodInfo), null);
					}

					methodInfo = drmnType.GetMethod(attr.ReferenceModeNamingFromObjectTypeCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(ModelElement), typeof(ObjectType), typeof(bool), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myRMNFromObjectTypeCallback, (GetReferenceModeNamingFromObjectType)Delegate.CreateDelegate(typeof(GetReferenceModeNamingFromObjectType), methodInfo), null);
					}

					methodInfo = drmnType.GetMethod(attr.DefaultReferenceModeNamingFromObjectTypeCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(ModelElement), typeof(ObjectType), typeof(Type), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myDRMNFromObjectType, (GetDefaultReferenceModeNamingFromObjectType)Delegate.CreateDelegate(typeof(GetDefaultReferenceModeNamingFromObjectType), methodInfo), null);
					}

					methodInfo = drmnType.GetMethod(attr.ContextDefaultReferenceModeNamingCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(object), typeof(ReferenceModeType), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myContextDRMNCallback, (GetContextDefaultReferenceModeNaming)Delegate.CreateDelegate(typeof(GetContextDefaultReferenceModeNaming), methodInfo), null);
					}

					methodInfo = drmnType.GetMethod(attr.ContextReferenceModeNamingCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(ObjectType), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myContextRMNCallback, (GetContextReferenceModeNaming)Delegate.CreateDelegate(typeof(GetContextReferenceModeNaming), methodInfo), null);
					}

					methodInfo = drmnType.GetMethod(attr.PropertyDescriptorSuffixCallbackName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Type), typeof(Type) }, null);
					if (methodInfo != null)
					{
						System.Threading.Interlocked.CompareExchange(ref myPropertySuffixCallback, (GetPropertyDescriptorSuffix)Delegate.CreateDelegate(typeof(GetPropertyDescriptorSuffix), methodInfo), null);
					}
				}
			}
		}
		#endregion // Instance Resolution Helper
		#region Resource String Helper
		private static string GetResourceString<ResourceOwner>(string resourceName)
			where ResourceOwner : class
		{
			string retVal = ResourceAccessor<ResourceOwner>.ResourceManager.GetString(resourceName);
			Debug.Assert(!String.IsNullOrEmpty(retVal), "Unrecognized ReferenceModeNaming resource string: " + resourceName);
			return retVal ?? String.Empty;
		}
		#endregion // Resource String Helper
		#region ReferenceToEntityTypeNamingChoicePropertyDescriptor class
		/// <summary>
		/// Property descriptor for the entity type reference mode naming choice.
		/// </summary>
		protected sealed class ReferenceToEntityTypeNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : ReferenceModeNamingPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly ReferenceToEntityTypeNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> Instance = new ReferenceToEntityTypeNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(false);

			/// <summary>
			/// Use a single typed instance for this property descriptor displayed with other instances of this descriptor
			/// </summary>
			public static readonly ReferenceToEntityTypeNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> ExtensionSpecificInstance = new ReferenceToEntityTypeNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(true);
			private ReferenceToEntityTypeNamingChoicePropertyDescriptor(bool displayExtensionSpecificName)
				: base("ReferenceToEntityTypeNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), typeof(NamingLink)), displayExtensionSpecificName)
			{
			}
			/// <summary>
			/// Standard override
			/// </summary>
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override object GetEditor(Type editorBaseType)
			{
				return (editorBaseType == typeof(UITypeEditor)) ? ReferenceModeNamingEditor.Instance : base.GetEditor(editorBaseType);
			}

			/// <summary>
			/// Standard override
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return ReferenceModeNamingEnumConverter.Instance;
				}
			}
			private sealed class ReferenceModeNamingEditor : ReferenceModeNamingEditorBase
			{
				public static readonly ReferenceModeNamingEditor Instance = new ReferenceModeNamingEditor();
				private ReferenceModeNamingEditor() { }
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.ReferenceToEntityType; }
				}
			}

			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			private sealed class ReferenceModeNamingEnumConverter : ReferenceModeNamingEnumConverterBase
			{
				public static readonly ReferenceModeNamingEnumConverter Instance = new ReferenceModeNamingEnumConverter();
				private ReferenceModeNamingEnumConverter() { }
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.ReferenceToEntityType; }
				}
			}
		}
		#endregion // ReferenceToEntityTypeNamingChoicePropertyDescriptor class
		#region // PrimaryIdentifierNamingChoicePropertyDescriptor class
		/// <summary>
		/// Property descriptor for the primary identifier reference mode naming choice.
		/// </summary>
		protected sealed class PrimaryIdentifierNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : ReferenceModeNamingPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly PrimaryIdentifierNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> Instance = new PrimaryIdentifierNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(false);

			/// <summary>
			/// Use a single typed instance for this property descriptor displayed with other instances of this descriptor
			/// </summary>
			public static readonly PrimaryIdentifierNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> ExtensionSpecificInstance = new PrimaryIdentifierNamingChoicePropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(true);
			private PrimaryIdentifierNamingChoicePropertyDescriptor(bool displayExtensionSpecificName)
				: base("PrimaryIdentifierNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), typeof(NamingLink)), displayExtensionSpecificName)
			{
			}
			/// <summary>
			/// This property is for a primary identifier
			/// </summary>
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override object GetEditor(Type editorBaseType)
			{
				return (editorBaseType == typeof(UITypeEditor)) ? ReferenceModeNamingEditor.Instance : base.GetEditor(editorBaseType);
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return ReferenceModeNamingEnumConverter.Instance;
				}
			}
			private sealed class ReferenceModeNamingEditor : ReferenceModeNamingEditorBase
			{
				public static readonly ReferenceModeNamingEditor Instance = new ReferenceModeNamingEditor();
				private ReferenceModeNamingEditor() { }
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.PrimaryIdentifier; }
				}
			}

			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			private sealed class ReferenceModeNamingEnumConverter : ReferenceModeNamingEnumConverterBase
			{
				public static readonly ReferenceModeNamingEnumConverter Instance = new ReferenceModeNamingEnumConverter();
				private ReferenceModeNamingEnumConverter() { }

				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.PrimaryIdentifier; }
				}
			}
		}
		/// <summary>
		/// A base class for creating reference mode naming property descriptors on an <see cref="ObjectType"/>
		/// </summary>
		/// <typeparam name="RMN">A concrete ReferenceModeNaming class</typeparam>
		/// <typeparam name="NamingLink">The type of the link to the object type for the ReferenceModeNaming implementation provided in <typeparamref name="RMN"/>.
		/// This is used as a reference by the callback, not interpreted by the calling code.</typeparam>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		protected abstract class ReferenceModeNamingPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : PropertyDescriptor
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class // Resource owner, must have all enum values with the requested names.
		{
			#region ReferenceModeNameEnumConverterBase
			/// <summary>
			/// A base converter for the <see cref="ReferenceModeNamingChoice"/> enumeration values
			/// </summary>
			[CLSCompliant(false)]
			protected abstract class ReferenceModeNamingEnumConverterBase : EnumConverter<ReferenceModeNamingChoice, ORMModel>
			{
				/// <summary>
				/// Get the target use for this editor
				/// </summary>
				protected abstract ReferenceModeNamingUse TargetUse { get; }

				// Note that the values are explicitly ordered  and match the values listed in the ReferenceModeNamingEditor
				private static readonly StandardValuesCollection OrderedValues = new StandardValuesCollection(new object[] { ReferenceModeNamingChoice.ModelDefault, ReferenceModeNamingChoice.EntityTypeName, ReferenceModeNamingChoice.ReferenceModeName, ReferenceModeNamingChoice.ValueTypeName, ReferenceModeNamingChoice.CustomFormat});
				protected ReferenceModeNamingEnumConverterBase()
				{
				}
				/// <summary>
				/// List the items in a consistent order
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return OrderedValues;
				}

				/// <summary>
				/// Standard override
				/// </summary>
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					ObjectType objectType;
					IReferenceModePattern referenceMode;
					if (context != null &&
						destinationType == typeof(string) &&
						value is ReferenceModeNamingChoice &&
						null != (objectType = EditorUtility.ResolveContextInstance(context.Instance, true) as ObjectType) &&
						null != (referenceMode = objectType.ReferenceModePattern))
					{
						string currentName = null;
						string resourceId = null;
						switch ((ReferenceModeNamingChoice)value)
						{
							case ReferenceModeNamingChoice.ValueTypeName:
								resourceId = "ReferenceModeNaming.CurrentFormatString.ValueTypeName";
								currentName = objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.Name;
								break;
							case ReferenceModeNamingChoice.EntityTypeName:
								resourceId = "ReferenceModeNaming.CurrentFormatString.EntityTypeName";
								currentName = objectType.Name;
								break;
							case ReferenceModeNamingChoice.ReferenceModeName:
								resourceId = "ReferenceModeNaming.CurrentFormatString.ReferenceModeName";
								currentName = referenceMode.Name;
								break;
							case ReferenceModeNamingChoice.CustomFormat:
								resourceId = "ReferenceModeNaming.CurrentFormatString.CustomFormat";
								currentName = ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(null, objectType, ReferenceModeNamingChoice.CustomFormat, TargetUse);
								break;
							case ReferenceModeNamingChoice.ModelDefault:
								switch (GetNamingChoiceFromDefault(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink)), referenceMode.ReferenceModeType, TargetUse))
								{
									case EffectiveReferenceModeNamingChoice.ValueTypeName:
										resourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.ValueTypeName";
										currentName = objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.Name;
										break;
									case EffectiveReferenceModeNamingChoice.ReferenceModeName:
										resourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.ReferenceModeName";
										currentName = referenceMode.Name;
										break;
									case EffectiveReferenceModeNamingChoice.EntityTypeName:
										resourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.EntityTypeName";
										currentName = objectType.Name;
										break;
									case EffectiveReferenceModeNamingChoice.CustomFormat:
										resourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.CustomFormat";
										currentName = ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(null, objectType, ReferenceModeNamingChoice.CustomFormat, TargetUse);
										break;
								}
								break;
						}
						return string.Format(culture, GetResourceString<TResourceOwner>(resourceId), currentName);
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}
			}
			#endregion // ReferenceModeNamingEnumConverterBase
			#region ReferenceModeNamingEditor class
			/// <summary>
			/// Base class for choosing <see cref="ReferenceModeNamingChoice"/> elements with customized names
			/// </summary>
			[CLSCompliant(false)]
			protected abstract class ReferenceModeNamingEditorBase : ElementPicker<ReferenceModeNamingEditorBase>
			{
				/// <summary>
				/// Get the target use for this editor
				/// </summary>
				protected abstract ReferenceModeNamingUse TargetUse { get;}
				/// <summary>
				/// Standard editor override
				/// </summary>
				protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
				{
					switch (newIndex)
					{
						case 1:
							return ReferenceModeNamingChoice.EntityTypeName;
						case 2:
							return ReferenceModeNamingChoice.ReferenceModeName;
						case 3:
							return ReferenceModeNamingChoice.ValueTypeName;
						case 4:
							return ReferenceModeNamingChoice.CustomFormat;
						// case 0:
						default:
							return ReferenceModeNamingChoice.ModelDefault;
					}
				}
				/// <summary>
				/// Standard editor override
				/// </summary>
				protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
				{
					return contentList[NamingChoiceToIndex((ReferenceModeNamingChoice)initialObject)];
				}
				private static int NamingChoiceToIndex(ReferenceModeNamingChoice namingChoice)
				{
					switch (namingChoice)
					{
						case ReferenceModeNamingChoice.ValueTypeName:
							return 3;
						case ReferenceModeNamingChoice.EntityTypeName:
							return 1;
						case ReferenceModeNamingChoice.ReferenceModeName:
							return 2;
						case ReferenceModeNamingChoice.CustomFormat:
							return 4;
						//case ReferenceModeNamingChoice.ModelDefault:
						default:
							return 0;
					}
				}
				/// <summary>
				/// Standard editor override
				/// </summary>
				protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
				{
					ObjectType objectType;
					IReferenceModePattern referenceMode;
					if (null != (objectType = EditorUtility.ResolveContextInstance(context.Instance, true) as ObjectType) &&
						null != (referenceMode = objectType.ReferenceModePattern))
					{
						string currentModelDefaultResourceId = null;
						string currentName = null;
						string entityTypeName = objectType.Name;
						string referenceModeName = referenceMode.Name;
						string valueTypeName = objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.Name;
						string customFormatName = ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(null, objectType, ReferenceModeNamingChoice.CustomFormat, TargetUse);
						switch (GetNamingChoiceFromDefault(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink)), referenceMode.ReferenceModeType, TargetUse))
						{
							case EffectiveReferenceModeNamingChoice.ValueTypeName:
								currentModelDefaultResourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.ValueTypeName";
								currentName = valueTypeName;
								break;
							case EffectiveReferenceModeNamingChoice.ReferenceModeName:
								currentModelDefaultResourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.ReferenceModeName";
								currentName = referenceModeName;
								break;
							case EffectiveReferenceModeNamingChoice.EntityTypeName:
								currentModelDefaultResourceId = "ReferenceModeNaming.CurrentModelDefaultFormatString.EntityTypeName";
								currentName = entityTypeName;
								break;
							case EffectiveReferenceModeNamingChoice.CustomFormat:
								currentModelDefaultResourceId ="ReferenceModeNaming.CurrentModelDefaultFormatString.CustomFormat";
								currentName = customFormatName;
								break;
						}
						CultureInfo culture = CultureInfo.CurrentCulture;
						return new string[]
						{
							string.Format(GetResourceString<TResourceOwner>(currentModelDefaultResourceId), currentName),
							string.Format(culture, GetResourceString<TResourceOwner>("ReferenceModeNaming.CurrentFormatString.EntityTypeName"), entityTypeName),
							string.Format(culture,GetResourceString<TResourceOwner>("ReferenceModeNaming.CurrentFormatString.ReferenceModeName"), referenceModeName),
							string.Format(culture, GetResourceString<TResourceOwner>("ReferenceModeNaming.CurrentFormatString.ValueTypeName"), valueTypeName),
							string.Format(culture, GetResourceString<TResourceOwner>("ReferenceModeNaming.CurrentFormatString.CustomFormat"), customFormatName),
						};
					}
					return null;
				}
			}
			#endregion // ReferenceModeNamingEditor class
			private readonly bool myDisplayExtensionSpecificName;

			/// <summary>
			/// Shared constructor
			/// </summary>
			/// <param name="name">The name used to distinguish this in the collection of object type properties.</param>
			/// <param name="displayExtensionSpecificName">Set to true if this is to be displayed with like properties from other extensions.</param>
			protected ReferenceModeNamingPropertyDescriptor(string name, bool displayExtensionSpecificName)
				: base(name, null)
			{
				myDisplayExtensionSpecificName = displayExtensionSpecificName;
			}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
			private static ObjectType GetObjectTypeFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ObjectType;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the model browser
				ObjectType objectType = GetObjectTypeFromComponent(component);
				ReferenceModeNaming naming = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, true, typeof(NamingLink));
				if (naming == null)
				{
					return false;
				}

				ReferenceModeNaming contextNaming = naming != null ? naming.ContextReferenceModeNaming : InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
				return GetNamingChoice(naming, TargetUse) != GetNamingChoice(contextNaming, TargetUse);
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string DisplayName
			{
				get
				{
					bool extensionSpecific = myDisplayExtensionSpecificName;
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>(extensionSpecific ? "ReferenceModeNaming.NamingChoiceProperty.DisplayName.ExtensionSpecific" : "ReferenceModeNaming.NamingChoiceProperty.DisplayName") :
						GetResourceString<TResourceOwner>(extensionSpecific ? "ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.DisplayName.ExtensionSpecific" : "ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.DisplayName");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Category
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNamingProperty.Category");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.NamingChoiceProperty.Description") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.PrimaryIdentifierNamingChoiceProperty.Description");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override object GetValue(object component)
			{
				return GetNamingChoice(InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, GetObjectTypeFromComponent(component), false, typeof(NamingLink)), TargetUse);
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ObjectType);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(ReferenceModeNamingChoice);
				}	
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void ResetValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, ReferenceModeNamingChoice.ModelDefault);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void SetValue(object component, object value)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, (ReferenceModeNamingChoice)value);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			private void SetValue(ObjectType objectType, ReferenceModeNamingChoice value)
			{
				IReferenceModePattern referenceMode = objectType.ReferenceModePattern;
				if (referenceMode == null)
				{
					return; // Sanity check, should not happen because property descriptor will not be displayed
				}

				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(GetResourceString<TResourceOwner>("ReferenceModeNaming.NamingChoiceProperty.TransactionName")))
				{
					ReferenceModeNaming naming = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, true, typeof(NamingLink));
					ReferenceModeNamingUse targetUse = TargetUse;
					if (naming != null)
					{
						if (targetUse == ReferenceModeNamingUse.PrimaryIdentifier)
						{
							naming.PrimaryIdentifierNamingChoice = value;
						}
						else
						{
							naming.NamingChoice = value;
						}
					}
					else if (value != ReferenceModeNamingChoice.ModelDefault)
					{
						ReferenceModeNaming contextRMN = InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
						ReferenceModeNamingChoice pidNamingChoice;
						ReferenceModeNamingChoice refNamingChoice;
						if (contextRMN != null)
						{
							if (targetUse == ReferenceModeNamingUse.PrimaryIdentifier)
							{
								pidNamingChoice = value;
								refNamingChoice = contextRMN.NamingChoice;
							}
							else
							{
								pidNamingChoice = contextRMN.PrimaryIdentifierNamingChoice;
								refNamingChoice = value;
							}
						}
						else
						{
							ReferenceModeType kind = referenceMode.ReferenceModeType;
							DefaultReferenceModeNaming contextDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink));
							if (targetUse == ReferenceModeNamingUse.PrimaryIdentifier)
							{
								pidNamingChoice = value;
								refNamingChoice = ReferenceModeNamingChoice.ModelDefault;
							}
							else
							{
								pidNamingChoice = ReferenceModeNamingChoice.ModelDefault;
								refNamingChoice = value;
							}
						}

						// Empty custom format indicates that the context value should be used (matching default if ModelDefault set, context RMN if not set to model default)
						naming = typeof(RMN).GetConstructor(new Type[] { typeof(Store), typeof(PropertyAssignment[]) }).Invoke(new object[] {
							store,
							new PropertyAssignment[] {
								new PropertyAssignment(ReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, pidNamingChoice),
								new PropertyAssignment(ReferenceModeNaming.NamingChoiceDomainPropertyId, refNamingChoice)
							}
						}) as ReferenceModeNaming;
						naming.AttachDynamicInstance(typeof(NamingLink));
						naming.ResolvedObjectType = objectType;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // PrimaryIdentifierNamingChoicePropertyDescriptor class
		#region ReferenceModeNamingCustomFormatTypeConverter class
		/// <summary>
		/// Base class for displaying and parsing custom format properties.
		/// </summary>
		protected abstract class CustomFormatTypeConverter<TResourceOwner> : StringConverter
			where TResourceOwner : class
		{
			private static Regex myParseRegex;
			private static Regex myCleanDanglingBracesRegex;
			private struct CustomFormatDisplayMapping
			{
				public CustomFormatDisplayMapping(string native, string display, string shortParse)
				{
					Native = native;
					Display = display;
					ShortParse = shortParse;
				}
				public string Native;
				public string Display;
				public string ShortParse;
			}
			private static CustomFormatDisplayMapping[] myDisplayMappings;
			/// <summary>
			/// Protected constructor for abstract class.
			/// </summary>
			protected CustomFormatTypeConverter()
			{
			}
			private static CustomFormatDisplayMapping[] DisplayMappings
			{
				get
				{
					CustomFormatDisplayMapping[] retVal = myDisplayMappings;
					if (retVal == null)
					{
						myDisplayMappings = retVal = new CustomFormatDisplayMapping[]
						{
							new CustomFormatDisplayMapping("{0}", GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementField.ValueTypeName"), GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementFieldShortForm.ValueTypeName")),
							new CustomFormatDisplayMapping("{1}", GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementField.EntityTypeName"), GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementFieldShortForm.EntityTypeName")),
							new CustomFormatDisplayMapping("{2}", GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementField.ReferenceModeName"), GetResourceString<TResourceOwner>("ReferenceModeNaming.DisplayedReplacementFieldShortForm.ReferenceModeName")),
						};
					}
					return retVal;
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					CustomFormatDisplayMapping[] mappings = DisplayMappings;
					return string.Format(culture, value.ToString(), mappings[0].Display, mappings[1].Display, mappings[2].Display);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			private static string CleanDanglingBraces(string startString, Regex cleanDanglingBracesRegex)
			{
				return cleanDanglingBracesRegex.Replace(
					startString,
					delegate(Match match)
					{
						GroupCollection groups = match.Groups;
						Group group = groups["Open"];
						if (group.Success)
						{
							return "{{";
						}
						group = groups["Close"];
						if (group.Success)
						{
							return "}}";
						}
						return match.Value;
					});
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				bool seenReplacementField;
				return TestConvertFrom(culture, value, out seenReplacementField);
			}
			/// <summary>
			/// Attempt to convert displayed naming choice property values
			/// </summary>
			protected object TestConvertFrom(CultureInfo culture, object value, out bool seenReplacementField)
			{
				#region Regex expressions
				Regex parseRegex = myParseRegex;
				if (parseRegex == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myParseRegex,
						new Regex(
							@"(?n)\G(?(?=((\{\{)|[^\{])*\{.+?\})((?<Before>((\{\{)|[^\{])*)(?<Replace>\{.+?\}))|(.*))",
							RegexOptions.Compiled),
						null);
					parseRegex = myParseRegex;
				}
				Regex cleanDanglingBracesRegex = myCleanDanglingBracesRegex;
				if (cleanDanglingBracesRegex == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myCleanDanglingBracesRegex,
						new Regex(
							@"(?n)\G(?<Open>\{)|(?<Close>\})|[^\{\}]*",
							RegexOptions.Compiled),
						null);
					cleanDanglingBracesRegex = myCleanDanglingBracesRegex;
				}
				#endregion // Regex expressions
				bool localSeenKnownToken = false;
				string retVal = parseRegex.Replace(
					(string)value,
					delegate(Match match)
					{
						GroupCollection groups = match.Groups;
						Group replaceGroup = groups["Replace"];
						if (replaceGroup.Success)
						{
							string knownToken = null;
							string replacementField = replaceGroup.Value;
							CustomFormatDisplayMapping[] mappings = DisplayMappings;
							for (int i = 0; i < mappings.Length; ++i)
							{
								CustomFormatDisplayMapping mapping = mappings[i];
								if (0 == string.Compare(replacementField, mapping.Display, true, culture) ||
									0 == string.Compare(replacementField, mapping.ShortParse, true, culture))
								{
									knownToken = mapping.Native;
									break;
								}
							}
							if (knownToken != null)
							{
								localSeenKnownToken = true;
								return CleanDanglingBraces(groups["Before"].Value, cleanDanglingBracesRegex) + knownToken;
							}
						}
						return CleanDanglingBraces(match.Value, cleanDanglingBracesRegex);
					});
				seenReplacementField = localSeenKnownToken;
				return retVal;
			}
		}
		#endregion // ReferenceModeNamingCustomFormatTypeConverter class
		#region ReferenceModeNamingCustomFormatPropertyDescriptor class
		/// <summary>
		/// Property descriptor for the entity type custom format.
		/// </summary>
		protected sealed class ReferenceToEntityTypeCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : ReferenceModeNamingCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly ReferenceToEntityTypeCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> Instance = new ReferenceToEntityTypeCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(false);

			/// <summary>
			/// Use a single typed instance for this property descriptor displayed with other instances of this descriptor
			/// </summary>
			public static readonly ReferenceToEntityTypeCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> ExtensionSpecificInstance = new ReferenceToEntityTypeCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(true);
			private ReferenceToEntityTypeCustomFormatPropertyDescriptor(bool displayExtensionSpecificName)
				: base("ReferenceToEntityTypeCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), typeof(NamingLink)), displayExtensionSpecificName)
			{
			}
			/// <summary>
			/// This property descriptor represents a reference to an entity type
			/// </summary>
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return DefaultAwareReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			private sealed class DefaultAwareReferenceModeNamingCustomFormatTypeConverter : DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase
			{
				/// <summary>
				/// Use a single typed instance for this property descriptor.
				/// </summary>
				public static readonly DefaultAwareReferenceModeNamingCustomFormatTypeConverter Instance = new DefaultAwareReferenceModeNamingCustomFormatTypeConverter();
				private DefaultAwareReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.ReferenceToEntityType; }
				}
			}
		}
		/// <summary>
		/// Property descriptor for the primary identifier custom format.
		/// </summary>
		protected sealed class PrimaryIdentifierCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : ReferenceModeNamingCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly PrimaryIdentifierCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> Instance = new PrimaryIdentifierCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(false);

			/// <summary>
			/// Use a single typed instance for this property descriptor displayed with other instances of this descriptor
			/// </summary>
			public static readonly PrimaryIdentifierCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> ExtensionSpecificInstance = new PrimaryIdentifierCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(true);
			private PrimaryIdentifierCustomFormatPropertyDescriptor(bool displayExtensionSpecificName)
				: base("PrimaryIdentifierCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), typeof(NamingLink)), displayExtensionSpecificName)
			{
			}
			/// <summary>
			/// Specify if this is being used as a primary identifier or a reference
			/// </summary>
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return DefaultAwareReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			private sealed class DefaultAwareReferenceModeNamingCustomFormatTypeConverter : DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase
			{
				public static readonly DefaultAwareReferenceModeNamingCustomFormatTypeConverter Instance = new DefaultAwareReferenceModeNamingCustomFormatTypeConverter();
				private DefaultAwareReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.PrimaryIdentifier; }
				}
			}
		}
		/// <summary>
		/// Property descriptor for custom format properties
		/// </summary>
		/// <typeparam name="RMN">The concrete ReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="NamingLink">The type of the link to the object type for the ReferenceModeNaming implementation provided in <typeparamref name="RMN"/>.
		/// This is used as a reference by the callback, not interpreted by the calling code.</typeparam>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		protected abstract class ReferenceModeNamingCustomFormatPropertyDescriptor<RMN, NamingLink, DRMN, DefaultNamingLink, TResourceOwner> : PropertyDescriptor
			where RMN : ReferenceModeNaming
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			#region DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			/// <summary>
			/// Converter base class for custom format properties
			/// </summary>
			protected abstract class DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase : CustomFormatTypeConverter<TResourceOwner>
			{
				/// <summary>
				/// Get the target use for this property descriptor
				/// </summary>
				protected abstract ReferenceModeNamingUse TargetUse { get;}
				/// <summary>
				/// Standard override
				/// </summary>
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						string stringValue = value as string;
						if (string.IsNullOrEmpty(stringValue))
						{
							// Fill in the backing default value if no data is currently shown
							ObjectType objectType;
							IReferenceModePattern referenceMode;
							if (null != (objectType = GetObjectTypeFromComponent(context.Instance))&&
								null != (referenceMode = objectType.ReferenceModePattern))
							{
								ReferenceModeNamingUse targetUse = TargetUse;
								ReferenceModeNaming contextRMN = InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
								value = (null != contextRMN && !string.IsNullOrEmpty(stringValue = GetCustomFormat(contextRMN, targetUse))) ?
									stringValue :
									GetCustomFormatFromDefault<TResourceOwner>(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink)), referenceMode.ReferenceModeType, targetUse);
							}
						}
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}
				/// <summary>
				/// Standard override
				/// </summary>
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					object newValue = base.ConvertFrom(context, culture, value);
					ObjectType objectType;
					IReferenceModePattern referenceMode;
					ReferenceModeNamingUse targetUse = TargetUse;
					if (null != (objectType = GetObjectTypeFromComponent(context.Instance)) &&
						null != (referenceMode = objectType.ReferenceModePattern))
					{
						ReferenceModeNaming currentRMN = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, false, typeof(NamingLink));
						ReferenceModeNaming contextRMN = currentRMN != null ? currentRMN.ContextReferenceModeNaming : InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
						string contextFormat = null;
						if (contextRMN == null ||
							string.IsNullOrEmpty(contextFormat = GetCustomFormat(contextRMN, targetUse)))
						{
							contextFormat = GetCustomFormatFromDefault<TResourceOwner>(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink)), referenceMode.ReferenceModeType, targetUse);
						}
						if (contextFormat == newValue.ToString())
						{
							return "";
						}
					}
					return newValue;
				}
			}
			#endregion // DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			private readonly bool myDisplayExtensionSpecificName;
			/// <summary>
			/// Protected constructor for abstract class
			/// </summary>
			protected ReferenceModeNamingCustomFormatPropertyDescriptor(string name, bool displayExtensionSpecificName)
				: base(name, null)
			{
				myDisplayExtensionSpecificName = displayExtensionSpecificName;
			}

			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
			private static ObjectType GetObjectTypeFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ObjectType;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool ShouldSerializeValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				ReferenceModeNaming naming;
				IReferenceModePattern referenceModePattern;
				if (null == (naming = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, true, typeof(NamingLink))) ||
					null == (referenceModePattern = objectType.ReferenceModePattern))
				{
					return false;
				}

				ReferenceModeNamingUse namingUse = TargetUse;
				string objectFormat = GetCustomFormat(naming, namingUse);
				if (string.IsNullOrEmpty(objectFormat))
				{
					return false;
				}

				ReferenceModeNaming contextNaming = naming != null ? naming.ContextReferenceModeNaming : InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
				if (contextNaming != null)
				{
					return objectFormat != GetCustomFormat(contextNaming, namingUse);
				}

				return objectFormat != GetCustomFormatFromDefault<TResourceOwner>(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink)), referenceModePattern.ReferenceModeType, namingUse);
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string DisplayName
			{
				get
				{
					bool extensionSpecific = myDisplayExtensionSpecificName;
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>(extensionSpecific ? "ReferenceModeNaming.CustomFormatProperty.DisplayName.ExtensionSpecific" : "ReferenceModeNaming.CustomFormatProperty.DisplayName") :
						GetResourceString<TResourceOwner>(extensionSpecific ? "ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.DisplayName.ExtensionSpecific" : "ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.DisplayName");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Category
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNamingProperty.Category");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.CustomFormatProperty.Description") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.PrimaryIdentifierCustomFormatProperty.Description");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override object GetValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				IReferenceModePattern referenceModePattern = objectType.ReferenceModePattern;
				if (referenceModePattern != null)
				{
					ReferenceModeNaming naming = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, false, typeof(NamingLink));
					if (naming != null)
					{
						return GetCustomFormat(naming, TargetUse);
					}

					DefaultReferenceModeNaming defaultNaming = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink));
					if (defaultNaming != null)
					{
						return GetCustomFormatFromDefault<TResourceOwner>(defaultNaming, referenceModePattern.ReferenceModeType, TargetUse);
					}
				}
				return "";
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ObjectType);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(string);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void ResetValue(object component)
			{
				ObjectType objectType;
				IReferenceModePattern referenceModePattern;
				if (null != (objectType = GetObjectTypeFromComponent(component)) &&
					null != (referenceModePattern = objectType.ReferenceModePattern))
				{
					SetValue(objectType, "");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void SetValue(object component, object value)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, (string)value);
				}
			}
			private void SetValue(ObjectType objectType, string value)
			{
				IReferenceModePattern referenceMode = objectType.ReferenceModePattern;
				if (referenceMode == null)
				{
					return; // Sanity check, should not happen because property descriptor will not be displayed
				}

				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(GetResourceString<TResourceOwner>("ReferenceModeNaming.CustomFormatProperty.TransactionName")))
				{
					ReferenceModeNaming naming = InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(null, objectType, true, typeof(NamingLink));
					ReferenceModeNamingUse targetUse = TargetUse;
					if (naming != null)
					{
						if (targetUse == ReferenceModeNamingUse.PrimaryIdentifier)
						{
							naming.PrimaryIdentifierCustomFormat = value;
						}
						else
						{
							naming.CustomFormat = value;
						}
					}
					else if (!string.IsNullOrEmpty(value))
					{
						ReferenceModeNaming contextRMN = InstanceResolver<DRMN>.ResolveContextReferenceModeNaming(objectType, typeof(NamingLink));
						ReferenceModeNamingChoice pidNamingChoice;
						ReferenceModeNamingChoice refNamingChoice;
						if (contextRMN != null)
						{
							pidNamingChoice = contextRMN.PrimaryIdentifierNamingChoice;
							refNamingChoice = contextRMN.NamingChoice;
						}
						else
						{
							ReferenceModeType kind = referenceMode.ReferenceModeType;
							DefaultReferenceModeNaming contextDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromObjectType(null, objectType, typeof(DefaultNamingLink), typeof(NamingLink));
							pidNamingChoice = refNamingChoice = ReferenceModeNamingChoice.ModelDefault; // This is a new ReferenceModeNaming instance, simply defer to the default settings
						}

						// Empty custom format indicates that the context value should be used (matching default if ModelDefault set, context RMN if not set to model default)
						naming = typeof(RMN).GetConstructor(new Type[] { typeof(Store), typeof(PropertyAssignment[]) }).Invoke(new object[] {
							store,
							new PropertyAssignment[] {
								new PropertyAssignment(ReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, pidNamingChoice),
								new PropertyAssignment(ReferenceModeNaming.NamingChoiceDomainPropertyId, refNamingChoice),
								new PropertyAssignment(targetUse == ReferenceModeNamingUse.PrimaryIdentifier ? ReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId : ReferenceModeNaming.CustomFormatDomainPropertyId, value)
							}
						}) as ReferenceModeNaming;
						naming.AttachDynamicInstance(typeof(NamingLink));
						naming.ResolvedObjectType = objectType;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // ReferenceModeNamingCustomFormatPropertyDescriptor class
		#region DefaultNamingChoicePropertyDescriptor class
		private sealed class UnitBasedDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN: DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly UnitBasedDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new UnitBasedDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private UnitBasedDefaultNamingChoicePropertyDescriptor()
				: base("UnitBasedDefaultNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN: DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class PopularDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly PopularDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new PopularDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private PopularDefaultNamingChoicePropertyDescriptor()
				: base("PopularDefaultNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class GeneralDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly GeneralDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new GeneralDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private GeneralDefaultNamingChoicePropertyDescriptor()
				: base("GeneralDefaultNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private abstract class DefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : PropertyDescriptor
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			#region DefaultReferenceModeNamingEnumConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			public class DefaultReferenceModeNamingEnumConverter : EnumConverter<EffectiveReferenceModeNamingChoice, ORMModel>
			{
				public static readonly DefaultReferenceModeNamingEnumConverter Instance = new DefaultReferenceModeNamingEnumConverter();

				// Note that the values are explicitly ordered  and match the values listed in the ReferenceModeNamingEditor
				private static readonly StandardValuesCollection OrderedValues = new StandardValuesCollection(new object[] { EffectiveReferenceModeNamingChoice.EntityTypeName, EffectiveReferenceModeNamingChoice.ReferenceModeName, EffectiveReferenceModeNamingChoice.ValueTypeName, EffectiveReferenceModeNamingChoice.CustomFormat });
				private DefaultReferenceModeNamingEnumConverter()
				{
				}
				/// <summary>
				/// List the items in a consistent order
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return OrderedValues;
				}
			}
			#endregion // DefaultReferenceModeNamingEnumConverter
			protected DefaultNamingChoicePropertyDescriptor(string name)
				: base(name, null)
			{
			}
			private string TransactionName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultNamingChoiceProperty.TransactionName") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.TransactionName");
				}
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultNamingChoiceProperty.DisplayName") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.DisplayName");
				}
			}
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultNamingChoiceProperty.Description") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierNamingChoiceProperty.Description");
				}
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract ReferenceModeType TargetKind { get;}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
			public override TypeConverter Converter
			{
				get
				{
					return DefaultReferenceModeNamingEnumConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the property grid
				ReferenceModeType referenceModeType = TargetKind;
				DefaultReferenceModeNaming resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, true, typeof(DefaultNamingLink));
				if (resolvedDRMN == null)
				{
					// There is no way to override the default without an instance.
					return false;
				}
				DefaultReferenceModeNaming contextDRMN = resolvedDRMN.ContextDefaultReferenceModeNaming;
				return GetNamingChoiceFromDefault(resolvedDRMN, referenceModeType, TargetUse) != GetNamingChoiceFromDefault(contextDRMN, referenceModeType, TargetUse);
			}
			public sealed override string Category
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNamingProperty.Category");
				}
			}
			public sealed override object GetValue(object component)
			{
				ReferenceModeType referenceModeType = TargetKind;
				return GetNamingChoiceFromDefault(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, false, typeof(DefaultNamingLink)), referenceModeType, TargetUse);
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(TComponent);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(EffectiveReferenceModeNamingChoice);
				}
			}
			public sealed override void ResetValue(object component)
			{
				ReferenceModeType referenceModeType = TargetKind;
				DefaultReferenceModeNaming resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, true, typeof(DefaultNamingLink));
				if (resolvedDRMN != null)
				{
					DefaultReferenceModeNaming contextDRMN = resolvedDRMN.ContextDefaultReferenceModeNaming;
					SetValue(EditorUtility.ResolveContextInstance(component, false) as ModelElement, resolvedDRMN, GetNamingChoiceFromDefault(contextDRMN, referenceModeType, TargetUse));
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				SetValue(EditorUtility.ResolveContextInstance(component, false) as ModelElement, InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, TargetKind, true, typeof(DefaultNamingLink)), (EffectiveReferenceModeNamingChoice)value);
			}
			private void SetValue(ModelElement contextElement, DefaultReferenceModeNaming defaultNaming, EffectiveReferenceModeNamingChoice value)
			{
				Store store = contextElement.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(TransactionName))
				{
					ReferenceModeType kind = TargetKind;
					ReferenceModeNamingUse targetUse = TargetUse;
					bool setPrimaryIdentifier = targetUse == ReferenceModeNamingUse.PrimaryIdentifier;
					if (defaultNaming != null)
					{
						if (setPrimaryIdentifier)
						{
							defaultNaming.PrimaryIdentifierNamingChoice = value;
						}
						else
						{
							defaultNaming.NamingChoice = value;
						}
					}
					else
					{
						DefaultReferenceModeNaming contextDRMN = InstanceResolver<DRMN>.ResolveContextDefaultReferenceModeNaming(contextElement, kind, typeof(DefaultNamingLink));
						if (value != GetNamingChoiceFromDefault(contextDRMN, kind, targetUse))
						{
							// The only way to not have one of these already is with a default NamingChoice. However, DefaultReferenceModeNaming does
							// not know the default for this reference mode kind, so we set the three other properties.
							defaultNaming = typeof(DRMN).GetConstructor(new Type[] { typeof(Store), typeof(PropertyAssignment[]) }).Invoke(new object[] {
								store,
								new PropertyAssignment[] {
									new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, kind),
									new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, setPrimaryIdentifier ? GetNamingChoiceFromDefault(contextDRMN, kind, ReferenceModeNamingUse.ReferenceToEntityType) : value),
									new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, setPrimaryIdentifier ? value : GetNamingChoiceFromDefault(contextDRMN, kind, ReferenceModeNamingUse.PrimaryIdentifier)),
									new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, kind, ReferenceModeNamingUse.ReferenceToEntityType)),
									new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId, GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, kind, ReferenceModeNamingUse.PrimaryIdentifier))
								}
							}) as DefaultReferenceModeNaming;
							defaultNaming.AttachDynamicInstance(contextElement, typeof(DefaultNamingLink));
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // DefaultNamingChoicePropertyDescriptor class
		#region DefaultCustomFormatPropertyDescriptor class
		private sealed class UnitBasedDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN: DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly UnitBasedDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new UnitBasedDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private UnitBasedDefaultCustomFormatPropertyDescriptor()
				: base("UnitBasedDefaultCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class PopularDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly PopularDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new PopularDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private PopularDefaultCustomFormatPropertyDescriptor()
				: base("PopularDefaultCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class GeneralDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly GeneralDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new GeneralDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private GeneralDefaultCustomFormatPropertyDescriptor()
				: base("GeneralDefaultCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			public static readonly GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private abstract class DefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : PropertyDescriptor
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : class
		{
			#region ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter class
			private class ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter : CustomFormatTypeConverter<TResourceOwner>
			{
				public static readonly ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter Instance = new ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter();
				private ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					bool seenReplacementField;
					object retVal = TestConvertFrom(culture, value, out seenReplacementField);
					if (!seenReplacementField)
					{
						throw new InvalidOperationException(GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormat.InvalidDefaultCustomFormatException"));
					}
					return retVal;
				}
			}
			#endregion // ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter class
			protected DefaultCustomFormatPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the default custom format for this type of object
			/// </summary>
			private string DefaultCustomFormat
			{
				get
				{
					return GetDefaultCustomFormat<TResourceOwner>(TargetKind, TargetUse);
				}
			}
			private string TransactionName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormatProperty.TransactionName") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.TransactionName");
				}
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormatProperty.DisplayName") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.DisplayName");
				}
			}
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormatProperty.Description") :
						GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormatProperty.Description");
				}
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract ReferenceModeType TargetKind { get;}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
			public override TypeConverter Converter
			{
				get
				{
					return ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the property grid
				ReferenceModeType referenceModeType = TargetKind;
				DefaultReferenceModeNaming resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, true, typeof(DefaultNamingLink));
				if (resolvedDRMN == null)
				{
					// There is no way to override a default without a current instance.
					return false;
				}

				DefaultReferenceModeNaming contextDRMN = resolvedDRMN.ContextDefaultReferenceModeNaming;
				return GetCustomFormatFromDefault<TResourceOwner>(resolvedDRMN, referenceModeType, TargetUse) != (contextDRMN != null ? GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, referenceModeType, TargetUse) : DefaultCustomFormat);
			}
			public sealed override string Category
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNamingProperty.Category");
				}
			}
			public sealed override object GetValue(object component)
			{
				// Note that we have 'findNearest' here as false, which allows context defaults to be returned automatically.
				ReferenceModeType referenceModeType = TargetKind;
				return GetCustomFormatFromDefault<TResourceOwner>(InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, false, typeof(DefaultNamingLink)), referenceModeType, TargetUse);
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(TComponent);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(string);
				}
			}
			public sealed override void ResetValue(object component)
			{
				ReferenceModeType referenceModeType = TargetKind;
				DefaultReferenceModeNaming resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, referenceModeType, true, typeof(DefaultNamingLink));
				if (resolvedDRMN != null)
				{
					DefaultReferenceModeNaming contextDRMN = resolvedDRMN.ContextDefaultReferenceModeNaming;
					SetValue(EditorUtility.ResolveContextInstance(component, false) as ModelElement, resolvedDRMN, contextDRMN != null ? GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, referenceModeType, TargetUse) : DefaultCustomFormat);
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				SetValue(EditorUtility.ResolveContextInstance(component, false) as ModelElement, InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(component, TargetKind, true, typeof(DefaultNamingLink)), value as string);
			}
			private void SetValue(ModelElement contextElement, DefaultReferenceModeNaming naming, string value)
			{
				Store store = contextElement.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(TransactionName))
				{
					ReferenceModeType kind = TargetKind;
					ReferenceModeNamingUse targetUse = TargetUse;
					bool setPrimaryIdentifier = targetUse == ReferenceModeNamingUse.PrimaryIdentifier;
					if (naming != null)
					{
						if (setPrimaryIdentifier)
						{
							naming.PrimaryIdentifierCustomFormat = value;
						}
						else
						{
							naming.CustomFormat = value;
						}
					}
					else if (!string.IsNullOrEmpty(value))
					{
						DefaultReferenceModeNaming contextDRMN = InstanceResolver<DRMN>.ResolveContextDefaultReferenceModeNaming(contextElement, kind, typeof(DefaultNamingLink));
						if (value != GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, kind, targetUse))
						{
							// The only way to not have one of these already is with a default NamingChoice. However, DefaultReferenceModeNaming does
							// not know the default for this reference mode kind, so we set the three other properties.
							naming = typeof(DRMN).GetConstructor(new Type[] { typeof(Store), typeof(PropertyAssignment[]) }).Invoke(new object[] {
								store,
								new PropertyAssignment[] {
									new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, kind),
									new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, GetNamingChoiceFromDefault(contextDRMN, kind, ReferenceModeNamingUse.ReferenceToEntityType)),
									new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, GetNamingChoiceFromDefault(contextDRMN, kind, ReferenceModeNamingUse.PrimaryIdentifier)),
									new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, setPrimaryIdentifier ? GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, kind, ReferenceModeNamingUse.ReferenceToEntityType) : value),
									new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId, setPrimaryIdentifier ? value : GetCustomFormatFromDefault<TResourceOwner>(contextDRMN, kind, ReferenceModeNamingUse.PrimaryIdentifier))
								}
							}) as DefaultReferenceModeNaming;
							naming.AttachDynamicInstance(contextElement, typeof(DefaultNamingLink));
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // DefaultCustomFormatPropertyDescriptor class
		#region DefaultGroupingPropertyDescriptor class
		/// <summary>
		/// A property descriptor for adding settings for unit based reference mode uses
		/// </summary>
		protected class UnitBasedGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : ModelElement
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly UnitBasedGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new UnitBasedGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private UnitBasedGroupingPropertyDescriptor()
				: base("UnitBasedGroupingPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string DisplayName
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.UnitBased.DisplayName");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Description
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.UnitBased.Description");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						UnitBasedDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						UnitBasedDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance};
				}
			}
		}
		/// <summary>
		/// A property descriptor for adding settings for popular reference mode uses
		/// </summary>
		protected class PopularGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : ModelElement
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly PopularGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new PopularGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private PopularGroupingPropertyDescriptor()
				: base("PopularGroupingPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string DisplayName
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.Popular.DisplayName");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Description
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.Popular.Description");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						PopularDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						PopularDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance};
				}
			}
		}
		/// <summary>
		/// A property descriptor for adding settings for general reference mode uses
		/// </summary>
		protected class GeneralGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : DefaultGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : ModelElement
		{
			/// <summary>
			/// Use a single typed instance for this property descriptor.
			/// </summary>
			public static readonly GeneralGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> Instance = new GeneralGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>();
			private GeneralGroupingPropertyDescriptor()
				: base("GeneralGroupingPropertyDescriptor" + InstanceResolver<DRMN>.GetPropertySuffix(typeof(DefaultNamingLink), null))
			{
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string DisplayName
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.General.DisplayName");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Description
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.General.Description");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						GeneralDefaultNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						GeneralDefaultCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance,
						GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent>.Instance};
				}
			}
		}
		/// <summary>
		/// A base class for created property descriptors with expandable subproperties for each reference mode type.
		/// </summary>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">An information link type used for callback disambiguation.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		/// <typeparam name="TComponent">The component type this property descriptor is attached to.</typeparam>
		protected abstract class DefaultGroupingPropertyDescriptor<DRMN, DefaultNamingLink, TResourceOwner, TComponent> : PropertyDescriptor
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
			where TComponent : ModelElement
		{
			#region ExpandableTypeConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			private class ExpandableTypeConverter : TypeConverter
			{
				public static readonly ExpandableTypeConverter Instance = new ExpandableTypeConverter();
				private ExpandableTypeConverter()
				{
				}
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return false;
				}
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return destinationType == typeof(string);
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					return null;
				}
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						return context.PropertyDescriptor.ShouldSerializeValue(context.Instance) ?
							GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.DisplayValue.Custom") :
							GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultGroup.DisplayValue.Default");
					}
					return null;
				}
				public override bool GetPropertiesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
				public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
				{
					return context.PropertyDescriptor.GetChildProperties(context.Instance, attributes);
				}
			}
			#endregion // ExpandableTypeConverter
			/// <summary>
			/// Create a grouping property descriptor
			/// </summary>
			/// <param name="name">The non-localized name of the property in the set of all properties of the extended element.</param>
			protected DefaultGroupingPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Return an array of <see cref="PropertyDescriptor"/>s to use in the default expansion
			/// </summary>
			protected abstract PropertyDescriptor[] PropertyDescriptors { get;}
			private PropertyDescriptorCollection myProperties;
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
			{
				PropertyDescriptorCollection retVal = myProperties;
				if (retVal == null)
				{
					myProperties = retVal = new PropertyDescriptorCollection(PropertyDescriptors, true);
				}
				return retVal;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return ExpandableTypeConverter.Instance;
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool ShouldSerializeValue(object component)
			{
				foreach (PropertyDescriptor descriptor in GetChildProperties(null, null))
				{
					if (descriptor.ShouldSerializeValue(component))
					{
						return true;
					}
				}
				return false;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override string Category
			{
				get
				{
					return GetResourceString<TResourceOwner>("ReferenceModeNamingProperty.Category");
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override object GetValue(object component)
			{
				return component;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(TComponent);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(object);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void ResetValue(object component)
			{
				foreach (PropertyDescriptor descriptor in GetChildProperties(null, null))
				{
					descriptor.ResetValue(component);
				}
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public sealed override void SetValue(object component, object value)
			{
			}
		}
		#endregion // DefaultGroupingPropertyDescriptor class
		#region Static helper functions
		private static Regex myFormatStringParser;
		private static Regex FormatStringParser
		{
			get
			{
				Regex retVal = myFormatStringParser;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFormatStringParser,
						new Regex(
							@"(?n)\G(?<Before>.*?)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					retVal = myFormatStringParser;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Retrieve the name generated for a reference to the <paramref name="entityType"/> for the
		/// given <paramref name="namingChoice"/>
		/// </summary>
		/// <param name="contextInstance">An element providing context for this request. This is understood and passed to the callbacks
		/// on the corresponding <typeparamref name="DRMN">DefaultReferenceModeNaming</typeparamref> instance.</param>
		/// <param name="entityType">An entity type to generate the name for</param>
		/// <param name="namingChoice">The <see cref="ReferenceModeNamingChoice"/> to get the name for.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <returns>A name, or <see langword="null"/> if the ference mode pattern does not resolve.</returns>
		/// <typeparam name="NamingLink">The type of the link to the object type for the ReferenceModeNaming implementation.
		/// This is used as a reference by the callback, not interpreted by the calling code.</typeparam>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		public static string ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(ModelElement contextInstance, ObjectType entityType, ReferenceModeNamingChoice namingChoice, ReferenceModeNamingUse targetUse)
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			bool consumedValueTypeDummy;
			return ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(contextInstance, entityType, null, null, true, targetUse, namingChoice, null, null, out consumedValueTypeDummy);
		}
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="contextInstance">An element providing context for this request. This is understood and passed to the callbacks
		/// on the corresponding <typeparamref name="DRMN">DefaultReferenceModeNaming</typeparamref> instance.</param>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="alternateEntityType">An <see cref="ObjectType"/> that is a subtype of <paramref name="possibleEntityType"/>. <paramref name="possibleEntityType"/> is used to resolve any reference mode relationship, but the name for this type is generated.</param>
		/// <param name="preferEntityType">If true and a reference mode pattern is not found, then use the name of the <paramref name="possibleEntityType"/> by default.
		/// Otherwise, use the <paramref name="possibleValueType"/> name as the default when a reference mode pattern is not found.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <param name="nameGenerator">An optional <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name.</param>
		/// <returns>True if the valuetype was used as part of the generated name.</returns>
		/// <typeparam name="NamingLink">The type of the link to the object type for the ReferenceModeNaming implementation.
		/// This is used as a reference by the callback, not interpreted by the calling code.</typeparam>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		public static bool ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(ModelElement contextInstance, ObjectType possibleEntityType, ObjectType possibleValueType, ObjectType alternateEntityType, bool preferEntityType, ReferenceModeNamingUse targetUse, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			bool retVal;
			ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(contextInstance, possibleEntityType, possibleValueType, alternateEntityType, preferEntityType, targetUse, null, nameGenerator, addNamePartCallback, out retVal);
			return retVal;
		}
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="contextInstance">An element providing context for this request. This is understood and passed to the callbacks
		/// on the corresponding <typeparamref name="DRMN">DefaultReferenceModeNaming</typeparamref> instance.</param>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="alternateEntityType">An <see cref="ObjectType"/> that is a subtype of <paramref name="possibleEntityType"/>. <paramref name="possibleEntityType"/> is used to resolve any reference mode relationship, but the name for this type is generated.</param>
		/// <param name="preferEntityType">If true and a reference mode pattern is not found, then use the name of the <paramref name="possibleEntityType"/> by default.
		/// Otherwise, use the <paramref name="possibleValueType"/> name as the default when a reference mode pattern is not found.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <param name="forceNamingChoice">Use this naming choice (if specified) instead of the current setting on <paramref name="possibleEntityType"/></param>
		/// <param name="nameGenerator">An optional <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name. If this parameter is set,
		/// then we attempt to split the ValueTypeName into pieces and add through the callback instead of using the return value.</param>
		/// <param name="consumedValueType">Output set to true if the valuetype was used as part of the generated name.</param>
		/// <returns>An appropriate name, or <see langword="null"/> if the expected relationship does not pan out and <paramref name="addNamePartCallback"/> is <see langword="null"/>.</returns>
		/// <typeparam name="NamingLink">The type of the link to the object type for the ReferenceModeNaming implementation.
		/// This is used as a reference by the callback, not interpreted by the calling code.</typeparam>
		/// <typeparam name="DRMN">The concrete DefaultReferenceModeNaming type this is instantiated for.</typeparam>
		/// <typeparam name="DefaultNamingLink">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</typeparam>
		/// <typeparam name="TResourceOwner">The resource owner. This must have all of the used resource strings.</typeparam>
		private static string ResolveObjectTypeName<NamingLink, DRMN, DefaultNamingLink, TResourceOwner>(ModelElement contextInstance, ObjectType possibleEntityType, ObjectType possibleValueType, ObjectType alternateEntityType, bool preferEntityType, ReferenceModeNamingUse targetUse, ReferenceModeNamingChoice? forceNamingChoice, NameGenerator nameGenerator, AddNamePart addNamePartCallback, out bool consumedValueType)
			where NamingLink : ElementLink
			where DRMN : DefaultReferenceModeNaming
			where DefaultNamingLink : ElementLink
			where TResourceOwner : class
		{
			// Using reference mode naming data here is straightforward if there is a global default and a per-object override. However, if there
			// are context-specific instances of DefaultReferenceModeNaming and ReferenceModeNaming in addition to the global settings
			// (usually related to extensions that have IGeneratorTargetProvider support) then there are additional choices to make because there
			// are now four possible sources of data that need to be prioritized (global default, targeted default, global object type, targeted object type).
			// There are two competing sort orders here, namely (global, targeted) and (default, object type). The question is which to apply first.
			// If we do global then targeted, then a local object-type specific setting is override by a default (targeted default beats global object type).
			// As this seems counterintuitive, we use the other prioritization, so the priority (highest to lowest) is
			// targeted object type
			// global object type
			// targeted default
			// global default
			// This means we can get the nearest ReferenceModeNaming, with a fallback to the nearest DefaultReferenceModeNaming if a ReferenceModeNaming is available
			// or the 'ModelDefault' naming choice is in force.
			consumedValueType = false;
			IReferenceModePattern referenceMode;
			if (possibleEntityType != null &&
				null != (referenceMode = possibleEntityType.ReferenceModePattern))
			{
				ReferenceModeType referenceModeType = referenceMode.ReferenceModeType;
				ObjectType actualValueType = possibleEntityType.PreferredIdentifier.RoleCollection[0].RolePlayer;
				ObjectType resolveReferenceModeEntityType = possibleEntityType;
				if (alternateEntityType != null)
				{
					// Use the alternate for all naming purposes
					possibleEntityType = alternateEntityType;
				}
				if (possibleValueType != null && actualValueType != possibleValueType)
				{
					consumedValueType = !preferEntityType;
					if (addNamePartCallback != null)
					{
						SeparateObjectTypeParts(preferEntityType ? possibleEntityType : possibleValueType, nameGenerator, addNamePartCallback);
					}
					return null;
				}

				ReferenceModeNamingChoice choice = forceNamingChoice.HasValue ? forceNamingChoice.Value : GetNamingChoice(InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(contextInstance, resolveReferenceModeEntityType, false, typeof(NamingLink)), targetUse);
				DefaultReferenceModeNaming resolvedDRMN = null;
				consumedValueType = true;
				switch (choice)
				{
					case ReferenceModeNamingChoice.ModelDefault:
						switch (GetNamingChoiceFromDefault(resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(contextInstance, referenceModeType, false, typeof(DefaultNamingLink)), referenceModeType, targetUse))
						{
							case EffectiveReferenceModeNamingChoice.EntityTypeName:
								if (addNamePartCallback != null)
								{
									SeparateObjectTypeParts(possibleEntityType, nameGenerator, addNamePartCallback);
									return null;
								}
								else
								{
									return possibleEntityType.Name;
								}
							case EffectiveReferenceModeNamingChoice.ValueTypeName:
								if (addNamePartCallback != null)
								{
									string abbreviatedName = actualValueType.GetAbbreviatedName(nameGenerator, false);
									if (abbreviatedName != null)
									{
										addNamePartCallback(abbreviatedName, null);
									}
									else
									{
										SeparateReferenceModeParts(referenceMode, referenceModeType, possibleEntityType, nameGenerator, addNamePartCallback);
									}
									return null;
								}
								else
								{
									return actualValueType.Name;
								}
							case EffectiveReferenceModeNamingChoice.ReferenceModeName:
								if (addNamePartCallback != null)
								{
									addNamePartCallback(referenceMode.Name, null);
									return null;
								}
								else
								{
									return referenceMode.Name;
								}
						}
						break;
					case ReferenceModeNamingChoice.EntityTypeName:
						if (addNamePartCallback != null)
						{
							SeparateObjectTypeParts(possibleEntityType, nameGenerator, addNamePartCallback);
							return null;
						}
						else
						{
							return possibleEntityType.Name;
						}
					case ReferenceModeNamingChoice.ValueTypeName:
						if (addNamePartCallback != null)
						{
							string abbreviatedName = actualValueType.GetAbbreviatedName(nameGenerator, false);
							if (abbreviatedName != null)
							{
								addNamePartCallback(abbreviatedName, null);
							}
							else
							{
								SeparateReferenceModeParts(referenceMode, referenceModeType, possibleEntityType, nameGenerator, addNamePartCallback);
							}
							return null;
						}
						else
						{
							return actualValueType.Name;
						}
					case ReferenceModeNamingChoice.ReferenceModeName:
						if (addNamePartCallback != null)
						{
							// Use the alternate for all naming purposes
							addNamePartCallback(referenceMode.Name, null);
							return null;
						}
						else
						{
							return referenceMode.Name;
						}
				}

				// All that's left is custom format
				string customFormat = GetCustomFormat(InstanceResolver<DRMN>.ResolveReferenceModeNamingFromObjectType(contextInstance, possibleEntityType, false, typeof(NamingLink)), targetUse);
				if (string.IsNullOrEmpty(customFormat))
				{
					customFormat = GetCustomFormatFromDefault<TResourceOwner>(resolvedDRMN ?? (resolvedDRMN = InstanceResolver<DRMN>.ResolveDefaultReferenceModeNamingFromComponent(contextInstance, referenceModeType, false, typeof(DefaultNamingLink))), referenceModeType, targetUse);
				}
				if (!string.IsNullOrEmpty(customFormat))
				{
					if (addNamePartCallback != null)
					{
						SeparateCustomFormatParts(customFormat, referenceMode, referenceModeType, possibleEntityType, actualValueType, nameGenerator, addNamePartCallback);
						return null;
					}
					else
					{
						return string.Format(CultureInfo.CurrentCulture, customFormat, actualValueType.Name, possibleEntityType.Name, referenceMode.Name);
					}
				}
				consumedValueType = false;
			}
			if (addNamePartCallback != null)
			{
				consumedValueType = preferEntityType ? possibleEntityType == null : possibleValueType != null;
				if (possibleEntityType != null && alternateEntityType != null)
				{
					possibleEntityType = alternateEntityType;
				}
				SeparateObjectTypeParts(preferEntityType ? (possibleEntityType ?? possibleValueType) : (possibleValueType ?? possibleEntityType), nameGenerator, addNamePartCallback);
			}
			return null;
		}

		/// <summary>
		/// Used to separate value types into their constituent parts and and specify whether to explicitly case the word or not
		/// </summary>
		/// <param name="referenceMode">Used to get the name of the <see cref="IReferenceModePattern"/></param>
		/// <param name="referenceModeType">Used to determine if the mode is UnitBased and should therefore be explicitly cased</param>
		/// <param name="entityType">The EntityType that has this <paramref name="referenceMode"/></param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		private static void SeparateReferenceModeParts(IReferenceModePattern referenceMode, ReferenceModeType referenceModeType, ObjectType entityType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			string referenceModeFormatString = referenceMode.FormatString;
			Match match = FormatStringParser.Match(referenceModeFormatString);
			int trailingTextIndex = 0;
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string before = groups["Before"].Value;
				if (before.Length != 0)
				{
					addNamePartCallback(before, null);
				}
				switch (int.Parse(groups["ReplaceIndex"].Value))
				{
					case 0:
						SeparateObjectTypeParts(entityType, nameGenerator, addNamePartCallback);
						break;
					case 1:
						addNamePartCallback(new NamePart(referenceMode.Name, referenceModeType == ReferenceModeType.UnitBased ? NamePartOptions.ExplicitCasing : NamePartOptions.None), null);
						break;
				}
				trailingTextIndex += match.Length;
				match = match.NextMatch();
			}
			if (trailingTextIndex < referenceModeFormatString.Length)
			{
				addNamePartCallback(referenceModeFormatString.Substring(trailingTextIndex), null);
			}
		}
		/// <summary>
		/// Used to separate value types into their constituent parts and and specify whether to explicitly case the word or not
		/// </summary>
		/// <param name="customFormat">The custom format string</param>
		/// <param name="referenceMode">Used to get the name of the <see cref="IReferenceModePattern"/></param>
		/// <param name="referenceModeType">Used to determine if the mode is UnitBased and should therefore be explicitly cased</param>
		/// <param name="entityType">The EntityType that has this <paramref name="referenceMode"/></param>
		/// <param name="valueType">The ValueType that satisfies the <paramref name="referenceMode"/> with the <paramref name="entityType"/></param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		private static void SeparateCustomFormatParts(string customFormat, IReferenceModePattern referenceMode, ReferenceModeType referenceModeType, ObjectType entityType, ObjectType valueType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			Match match = FormatStringParser.Match(customFormat);
			int trailingTextIndex = 0;
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string before = groups["Before"].Value;
				if (before.Length != 0)
				{
					addNamePartCallback(before, null);
				}
				switch (int.Parse(groups["ReplaceIndex"].Value))
				{
					case 0: // ValueType
						string abbreviatedName = valueType.GetAbbreviatedName(nameGenerator, false);
						if (abbreviatedName != null)
						{
							addNamePartCallback(abbreviatedName, null);
						}
						else
						{
							SeparateReferenceModeParts(referenceMode, referenceModeType, entityType, nameGenerator, addNamePartCallback);
						}
						break;
					case 1: // EntityType
						SeparateObjectTypeParts(entityType, nameGenerator, addNamePartCallback);
						break;
					case 2: // ReferenceMode
						addNamePartCallback(new NamePart(referenceMode.Name, referenceModeType == ReferenceModeType.UnitBased ? NamePartOptions.ExplicitCasing : NamePartOptions.None), null);
						break;
				}
				trailingTextIndex += match.Length;
				match = match.NextMatch();
			}
			if (trailingTextIndex < customFormat.Length)
			{
				addNamePartCallback(customFormat.Substring(trailingTextIndex), null);
			}
		}
		/// <summary>
		/// Used to separate <see cref="ObjectType"/> names into constituent parts if the names are
		/// generated, such as with an objectification or an ValueType associated with a <see cref="ReferenceMode"/>.
		/// </summary>
		/// <param name="objectType">The EntityType to test</param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		public static void SeparateObjectTypeParts(ObjectType objectType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			if (objectType == null)
			{
				return;
			}
			string abbreviatedName = objectType.GetAbbreviatedName(nameGenerator, false);
			if (abbreviatedName != null)
			{
				addNamePartCallback(abbreviatedName, null);
				return;
			}
			string nativeName = objectType.Name;
			FactType objectifiedFactType;
			IReferenceModePattern valueTypeReferenceMode;
			if (null != (objectifiedFactType = objectType.NestedFactType))
			{
				IReading defaultReading;
				if (nativeName == objectifiedFactType.DefaultName &&
					null != (defaultReading = objectifiedFactType.GetDefaultReading()) &&
					!string.IsNullOrEmpty(defaultReading.Text))
				{
					SeparateReadingParts(defaultReading, nameGenerator, addNamePartCallback);
					return;
				}
			}
			else if (null != (valueTypeReferenceMode = objectType.ResolvedModel.GetReferenceModeForValueType(objectType)))
			{
				SeparateReferenceModeParts(valueTypeReferenceMode, valueTypeReferenceMode.ReferenceModeType, objectType, nameGenerator, addNamePartCallback);
				return;
			}
			addNamePartCallback(nativeName, null);
		}
		/// <summary>
		/// Used to separate <see cref="IReading"/> into constituent parts if the names are generated,
		/// such as with an objectification or an ValueType associated with a <see cref="ReferenceMode"/>.
		/// </summary>
		/// <param name="reading">The reading to analyze.</param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for role players</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		public static void SeparateReadingParts(IReading reading, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			string readingText;
			if (null != reading &&
				!string.IsNullOrEmpty(readingText = reading.Text))
			{
				Match match = FormatStringParser.Match(readingText);
				int trailingTextIndex = 0;
				IList<RoleBase> roles = reading.RoleCollection;
				while (match.Success)
				{
					GroupCollection groups = match.Groups;
					string before = groups["Before"].Value;
					if (before.Length != 0)
					{
						addNamePartCallback(before.Replace("- ", " ").Replace(" -", " "), null);
					}
					SeparateObjectTypeParts(roles[int.Parse(groups["ReplaceIndex"].Value)].Role.RolePlayer, nameGenerator, addNamePartCallback);
					trailingTextIndex += match.Length;
					match = match.NextMatch();
				}
				if (trailingTextIndex < readingText.Length)
				{
					addNamePartCallback(readingText.Substring(trailingTextIndex).Replace("- ", " ").Replace(" -", " "), null);
				}
			}
		}
		/// <summary>
		/// Return the naming choice for a <see cref="ReferenceModeNaming"/> instance
		/// </summary>
		/// <param name="naming">The instance to use, or null if no instance is found. Can be retrieved with the <see cref="InstanceResolver{DRMN}"/> helper.</param>
		/// <param name="targetUse">How the choice is used (identifier or reference)</param>
		/// <returns>The resolved choice.</returns>
		protected static ReferenceModeNamingChoice GetNamingChoice(ReferenceModeNaming naming, ReferenceModeNamingUse targetUse)
		{
			return (null != naming) ?
				((targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.NamingChoice : naming.PrimaryIdentifierNamingChoice) :
				ReferenceModeNamingChoice.ModelDefault;
		}
		private static string GetCustomFormat(ReferenceModeNaming naming, ReferenceModeNamingUse targetUse)
		{
			return (null != naming) ?
				((targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.CustomFormat : naming.PrimaryIdentifierCustomFormat) :
				"";
		}
		/// <summary>
		/// Given a <see cref="DefaultReferenceModeNaming"/> instance, determine the stored <see cref="EffectiveReferenceModeNamingChoice"/> for
		/// the specified <see cref="ReferenceModeNamingUse"/>.
		/// </summary>
		/// <param name="defaultNaming">The <see cref="DefaultReferenceModeNaming"/> instance to use. This can be retrieved with the <see cref="InstanceResolver{DRMN}"/> helper.</param>
		/// <param name="referenceModeType">The type of reference mode this applies to.</param>
		/// <param name="targetUse">How the choice is used (identifier or reference)</param>
		protected static EffectiveReferenceModeNamingChoice GetNamingChoiceFromDefault(DefaultReferenceModeNaming defaultNaming, ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			if (defaultNaming != null)
			{
				return (targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? defaultNaming.NamingChoice : defaultNaming.PrimaryIdentifierNamingChoice;
			}
			return GetDefaultNamingChoice(referenceModeType, targetUse);
		}
		/// <summary>
		/// Get the default <see cref="EffectiveReferenceModeNamingChoice"/> for a given <see cref="ReferenceModeType"/>
		/// and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		private static EffectiveReferenceModeNamingChoice GetDefaultNamingChoice(ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			return referenceModeType == ReferenceModeType.Popular ?
				EffectiveReferenceModeNamingChoice.CustomFormat :
				(targetUse == ReferenceModeNamingUse.PrimaryIdentifier ? EffectiveReferenceModeNamingChoice.ValueTypeName : EffectiveReferenceModeNamingChoice.EntityTypeName);
		}
		/// <summary>
		/// Given a <see cref="DefaultReferenceModeNaming"/>, determine the stored custom format used for reference mode naming
		/// for the given <see cref="ReferenceModeType"/> and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		private static string GetCustomFormatFromDefault<TResourceOwner>(DefaultReferenceModeNaming defaultNaming, ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
			where TResourceOwner : class
		{
			if (defaultNaming != null)
			{
				return (targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? defaultNaming.CustomFormat : defaultNaming.PrimaryIdentifierCustomFormat;
			}
			return GetDefaultCustomFormat<TResourceOwner>(referenceModeType, targetUse);
		}
		/// <summary>
		/// Get the default custom format used for reference mode naming of the given <see cref="ReferenceModeType"/>
		/// and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		protected static string GetDefaultCustomFormat<TResourceOwner>(ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
			where TResourceOwner : class
		{
			if (targetUse == ReferenceModeNamingUse.ReferenceToEntityType)
			{
				switch (referenceModeType)
				{
					case ReferenceModeType.UnitBased:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormat.UnitBased");
					case ReferenceModeType.Popular:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormat.Popular");
					// case ReferenceModeType.General:
					default:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultCustomFormat.General");
				}
			}
			else
			{
				switch (referenceModeType)
				{
					case ReferenceModeType.UnitBased:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.UnitBased");
					case ReferenceModeType.Popular:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.Popular");
					// case ReferenceModeType.General:
					default:
						return GetResourceString<TResourceOwner>("ReferenceModeNaming.DefaultPrimaryIdentifierCustomFormat.General");
				}
			}
		}

		/// <summary>
		/// Determine if there are multiple implementations of ReferenceModeNaming in the store. If this
		/// is the case then the extension properties attached to an object type need to be decorated names
		/// to be distinguishable when categories are turned off in the properties window.
		/// </summary>
		/// <param name="store">Any element, used on first call to resolve type information.</param>
		/// <returns>true if there are multiple loaded extensions that implement ReferenceModeNaming.</returns>
		protected static bool HasMultipleObjectTypeExtensionSources(Store store)
		{
			// This is not a hard calculation and redoing it is easier than caching the results.
			// This cannot be statically cached because the Store can be reloaded without touching the type.
			return store.DomainDataDirectory.GetDomainClass(ReferenceModeNaming.DomainClassId).LocalDescendants.Count > 1;
		}
		#endregion // Static helper functions
		#region Instance helper functions
		/// <summary>
		/// Allow a <see cref="ReferenceModeNaming"/> instance to use another ReferenceModeNaming as
		/// its base. This allows targeted settings to override global defaults for an object type
		/// without deferring straight to the context <see cref="DefaultReferenceModeNaming"/>.
		/// </summary>
		/// <remarks>If this is set, then the corresponding function on the <see cref="DefaultReferenceModeNamingOwnerAttribute"/>
		/// should also be set, which does this based on link type instead of instance information.</remarks>
		public virtual ReferenceModeNaming ContextReferenceModeNaming
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Determine if the <see cref="CustomFormat"/> string is currently used
		/// </summary>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> to test</param>
		/// <param name="requireFormatString">Set to <see langword="true"/> if a current format string is required.</param>
		/// <returns><see langword="true"/> if the CustomFormat field is currently in use.</returns>
		private bool UsesCustomFormat(ReferenceModeNamingUse targetUse, bool requireFormatString)
		{
			if (requireFormatString && string.IsNullOrEmpty(CustomFormat))
			{
				return false;
			}
			switch (NamingChoice)
			{
				case ReferenceModeNamingChoice.CustomFormat:
					return true;
				case ReferenceModeNamingChoice.ModelDefault:
					{
						ObjectType objectType;
						IReferenceModePattern referenceMode;
						DefaultReferenceModeNaming defaultNaming;
						if (null != (objectType = this.ResolvedObjectType) &&
							null != (referenceMode = objectType.ReferenceModePattern) &&
							null != (defaultNaming = ResolveDefaultReferenceModeNaming()))
						{
							return GetNamingChoiceFromDefault(defaultNaming, referenceMode.ReferenceModeType, targetUse) == EffectiveReferenceModeNamingChoice.CustomFormat;
						}
					}
					break;
			}
			return false;
		}
		#endregion // Instance helper functions
	}
	partial class DefaultReferenceModeNaming
	{
		#region Abstract and Virtual Extension Points
		/// <summary>
		/// Attach the owning element to an instance dynamically created by a property descriptor.
		/// </summary>
		/// <param name="contextElement">The selected element that the property descriptors are attached to.</param>
		/// <param name="defaultNamingLinkType">The type of a link from the <see cref="DefaultReferenceModeNaming"/> instance to an associated instance,
		/// generally the parent of the instance. This is used as a reference by the callback, not the calling code.</param>
		public abstract void AttachDynamicInstance(ModelElement contextElement, Type defaultNamingLinkType);
		/// <summary>
		/// Allow a <see cref="DefaultReferenceModeNaming"/> instance to use another DefaultReferenceModeNaming as
		/// its base. This allows targeted defaults to override global defaults.
		/// </summary>
		public virtual DefaultReferenceModeNaming ContextDefaultReferenceModeNaming
		{
			get
			{
				return null;
			}
		}
		#endregion // Abstract and Virtual Extension Points
	}
}
