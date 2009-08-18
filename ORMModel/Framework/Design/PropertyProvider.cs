#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	#region PropertyProviderFeatures enum
	/// <summary>
	/// Advanced features supported by an <see cref="IPropertyProvider"/> implementation
	/// </summary>
	[Flags]
	public enum PropertyProviderFeatures
	{
		/// <summary>
		/// The property provider creates <see cref="PropertyDescriptor"/>s that operate
		/// on a specific instance. Used with <see cref="IPropertyProvider.AddPropertyDescriptors"/>.
		/// </summary>
		InstanceDescriptors = 1,
		/// <summary>
		/// The property provider creates <see cref="PropertyDescriptor"/>s that can
		/// operate on any instance. <see cref="IPropertyProvider.AddTypePropertyDescriptors"/>
		/// can be called with a <see langword="null"/> extendableElement parameter.
		/// </summary>
		StaticDescriptors = 2,
		/// <summary>
		/// The property provider provides notifications when a property changes.
		/// This feature must be set to support the <see cref="IPropertyProvider.PropertyChanged"/>
		/// event.
		/// </summary>
		ChangeNotifications = 4,
	}
	#endregion // PropertyProviderFeatures enum
	#region ExtensionPropertyChangedEventHandler delegate
	/// <summary>
	/// Event signature used by the <see cref="IPropertyProvider.PropertyChanged"/> event to signal
	/// that a property has changed.
	/// </summary>
	/// <param name="sender">The <see cref="IPropertyProvider"/> instance raising the event.</param>
	/// <param name="e"><see cref="ExtensionPropertyChangedEventArgs"/> specifying the instance and property the change occurred on.</param>
	public delegate void ExtensionPropertyChangedEventHandler(object sender, ExtensionPropertyChangedEventArgs e);
	#endregion // ExtensionPropertyChangedEventHandler delegate
	#region ExtensionPropertyChangedEventArgs class
	/// <summary>
	/// Event arguments used by the <see cref="IPropertyProvider.PropertyChanged"/> notification
	/// </summary>
	public sealed class ExtensionPropertyChangedEventArgs : EventArgs
	{
		#region Public Fields
		/// <summary>
		/// The instance of the modified extended element.
		/// </summary>
		public readonly object Instance;
		/// <summary>
		/// The <see cref="IPropertyProvider"/> instance that provides the modified property value.
		/// </summary>
		public readonly IPropertyProvider Provider;
		/// <summary>
		/// The property descriptor that was modified, or <see langword="null"/> to refresh all
		/// properties for the given provider.
		/// </summary>
		public readonly PropertyDescriptor Descriptor;
		#endregion // Public Fields
		#region Constructor
		/// <summary>
		/// Notify listeners that the value of an extension property has changed
		/// for a given instance.
		/// </summary>
		/// <param name="instance">The instance of the modified extended element.</param>
		/// <param name="provider">The <see cref="IPropertyProvider"/> instance that provides the modified property value.</param>
		/// <param name="descriptor">The property descriptor that was modified, or <see langword="null"/> to refresh all
		/// properties for the given provider.</param>
		public ExtensionPropertyChangedEventArgs(object instance, IPropertyProvider provider, PropertyDescriptor descriptor)
		{
			Instance = instance;
			Provider = provider;
			Descriptor = descriptor;
		}
		#endregion // Constructor
	}
	#endregion // ExtensionPropertyChangedEventArgs class
	#region IPropertyProvider interface
	/// <summary>
	/// Provide extension <see cref="PropertyDescriptor"/> instances for elements registered
	/// with <see cref="IPropertyProviderService.AddOrRemovePropertyProvider(Type, IPropertyProvider, Boolean, EventHandlerAction)"/>.
	/// If static providers and/or notifications are not needed, then extension properties can
	/// be more easily provided with a basic <see cref="PropertyProvider"/> callback.
	/// </summary>
	public interface IPropertyProvider
	{
		/// <summary>
		/// Add <see cref="PropertyDescriptor"/> instances for the provided <paramref name="extendableElement"/>
		/// </summary>
		/// <param name="extendableElement">The instance to extend.</param>
		/// <param name="properties">A <see cref="PropertyDescriptorCollection"/> to add descriptors to.</param>
		void AddPropertyDescriptors(object extendableElement, PropertyDescriptorCollection properties);
		/// <summary>
		/// Specify support levels for static property descriptors and change notifications.
		/// </summary>
		PropertyProviderFeatures ProviderFeatures { get;}
		/// <summary>
		/// Add static <see cref="PropertyDescriptor"/> instances. Static descriptor instances must operate correctly
		/// for any instance of an extendable element. Called if the <see cref="PropertyProviderFeatures.StaticDescriptors"/>
		/// feature is specified by the <see cref="IPropertyProvider.ProviderFeatures"/> property.
		/// </summary>
		/// <param name="extendableElementType">The <see cref="Type"/> of element to extend.</param>
		/// <param name="properties">The <see cref="PropertyDescriptorCollection"/> to populate.</param>
		void AddTypePropertyDescriptors(Type extendableElementType, PropertyDescriptorCollection properties);
		/// <summary>
		/// Add an event handler to notify a listener when an extension property has been modified.
		/// Called if the <see cref="PropertyProviderFeatures.ChangeNotifications"/> feature is
		/// specified by the <see cref="IPropertyProvider.ProviderFeatures"/> property.
		/// </summary>
		event ExtensionPropertyChangedEventHandler PropertyChanged;
	}
	#endregion // IPropertyProvider interface
	#region PropertyProvider delegate
	/// <summary>
	/// Adds extension <see cref="PropertyDescriptor"/>s for <paramref name="extendableElement"/>
	/// to the <see cref="PropertyDescriptorCollection"/> specified by <paramref name="properties"/>.
	/// Additional provider options are offered with the <see cref="IPropertyProvider"/> interface.
	/// </summary>
	public delegate void PropertyProvider(object extendableElement, PropertyDescriptorCollection properties);
	#endregion // PropertyProvider delegate
	#region IPropertyProviderService interface
	/// <summary>
	/// Provides methods for registering and unregistering <see cref="PropertyProvider"/>s for
	/// <see cref="ModelElement"/> instances.
	/// </summary>
	public interface IPropertyProviderService
	{
		/// <summary>
		/// Registers or unregisters the <see cref="PropertyProvider"/> specified by <paramref name="provider"/> for the
		/// type specified by <paramref name="extendableElementType"/>.
		/// </summary>
		/// <param name="extendableElementType">The <see cref="Type"/> of element to extend.</param>
		/// <param name="provider">The <see cref="PropertyProvider"/> being registered.</param>
		/// <param name="includeSubtypes">Specifies whether the <see cref="PropertyProvider"/> should also be registered for subtypes of <paramref name="extendableElementType"/>. Applicable only if the extended element type is derived from <see cref="ModelElement"/></param>
		/// <param name="action">Specifies whether the property provider is being added or removed. See <see cref="EventHandlerAction"/></param>
		void AddOrRemovePropertyProvider(Type extendableElementType, PropertyProvider provider, bool includeSubtypes, EventHandlerAction action);
		/// <summary>
		/// Registers or unregisters the <see cref="IPropertyProvider"/> specified by <paramref name="provider"/> for the
		/// type specified by <paramref name="extendableElementType"/>. The caller is responsible for registering and
		/// unregistering with the same provider instance.
		/// </summary>
		/// <param name="extendableElementType">The <see cref="Type"/> of element to extend.</param>
		/// <param name="provider">The <see cref="IPropertyProvider"/> being registered. The same instance should be used to both register and unregister.</param>
		/// <param name="includeSubtypes">Specifies whether the <see cref="PropertyProvider"/> should also be registered for subtypes of <paramref name="extendableElementType"/>. Applicable only if the extended element type is derived from <see cref="ModelElement"/></param>
		/// <param name="action">Specifies whether the property provider is being added or removed. See <see cref="EventHandlerAction"/></param>
		void AddOrRemovePropertyProvider(Type extendableElementType, IPropertyProvider provider, bool includeSubtypes, EventHandlerAction action);
		/// <summary>
		/// Adds extension <see cref="PropertyDescriptor"/>s for <paramref name="extendableElement"/>
		/// to the <see cref="PropertyDescriptorCollection"/> specified by <paramref name="properties"/>.
		/// </summary>
		void GetProvidedProperties(object extendableElement, PropertyDescriptorCollection properties);
		/// <summary>
		/// Adds static extension <see cref="PropertyDescriptor"/>s that recognize instances of
		/// <paramref name="extendableElementType"/> to the <see cref="PropertyDescriptorCollection"/>
		/// specified by <paramref name="properties"/>. Providers for Type-bound properties implement
		/// an <see cref="IPropertyProvider"/> that supports <see cref="PropertyProviderFeatures.StaticDescriptors"/>.
		/// </summary>
		void GetProvidedProperties(Type extendableElementType, PropertyDescriptorCollection properties);
		/// <summary>
		/// Register change listeners for property changes on the specified <paramref name="extendableElementType"/>
		/// </summary>
		/// <param name="extendableElementType">The <see cref="Type"/> of with registered property extensions.</param>
		/// <param name="handler">The <see cref="ExtensionPropertyChangedEventHandler"/> callback delegate.</param>
		/// <param name="action">Specify whether to add or remove the callback.</param>
		void AddOrRemoveChangeListener(Type extendableElementType, ExtensionPropertyChangedEventHandler handler, EventHandlerAction action);
	}
	#endregion // IPropertyProviderService interface
	#region PropertyProviderService class
	/// <summary>
	/// A standard implementation of <see cref="IPropertyProviderService"/>
	/// </summary>
	public sealed class PropertyProviderService : IPropertyProviderService
	{
		#region ProviderInstance struct
		/// <summary>
		/// Allow ordered tracking of both delegate and interface callbacks
		/// </summary>
		private struct ProviderInstance
		{
			private readonly PropertyProvider myDelegate;
			private readonly IPropertyProvider myInstance;
			public ProviderInstance(PropertyProvider provider)
			{
				myDelegate = provider;
				myInstance = null;
			}
			public ProviderInstance(IPropertyProvider provider)
			{
				myInstance = provider;
				myDelegate = null;
			}
			/// <summary>
			/// Equals without all of the annoying operator requirements
			/// </summary>
			public bool IsEquivalentTo(ProviderInstance other)
			{
				return myDelegate != null ? myDelegate == other.myDelegate : myInstance == other.myInstance;
			}
			/// <summary>
			/// Get the current instance property descriptors for this provider
			/// </summary>
			public void GetProvidedProperties(object extendableElement, PropertyDescriptorCollection properties)
			{
				PropertyProvider delegateInstance;
				IPropertyProvider interfaceInstance;
				if (null != (object)(delegateInstance = myDelegate))
				{
					delegateInstance(extendableElement, properties);
				}
				else if (null != (interfaceInstance = myInstance) &&
					0 != (interfaceInstance.ProviderFeatures & PropertyProviderFeatures.InstanceDescriptors))
				{
					interfaceInstance.AddPropertyDescriptors(extendableElement, properties);
				}
			}
			/// <summary>
			/// Get the current static property descriptors for this provider
			/// </summary>
			public void GetProvidedProperties(Type extendableElementType, PropertyDescriptorCollection properties)
			{
				IPropertyProvider interfaceInstance;
				if (null != (interfaceInstance = myInstance) &&
					0 != (interfaceInstance.ProviderFeatures & PropertyProviderFeatures.StaticDescriptors))
				{
					interfaceInstance.AddTypePropertyDescriptors(extendableElementType, properties);
				}
			}
			/// <summary>
			/// Add or remove change listeners as needed
			/// </summary>
			public void AddOrRemoveChangeListener(ExtensionPropertyChangedEventHandler handler, EventHandlerAction action)
			{
				IPropertyProvider interfaceInstance;
				if (null != (interfaceInstance = myInstance) &&
					0 != (interfaceInstance.ProviderFeatures & PropertyProviderFeatures.ChangeNotifications))
				{
					if (action == EventHandlerAction.Add)
					{
						interfaceInstance.PropertyChanged += handler;
					}
					else
					{
						interfaceInstance.PropertyChanged -= handler;
					}
				}
			}
		}
		#endregion // ProviderInstance struct
		#region Member Variables
		private readonly Store myStore;
		private readonly Dictionary<RuntimeTypeHandle, LinkedNode<ProviderInstance>> myProviderDictionary;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a <see cref="PropertyProviderService"/> for the specified <see cref="Store"/>
		/// </summary>
		public PropertyProviderService(Store store)
			: base()
		{
			Debug.Assert(store != null);
			this.myStore = store;
			this.myProviderDictionary = new Dictionary<RuntimeTypeHandle, LinkedNode<ProviderInstance>>(RuntimeTypeHandleComparer.Instance);
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// Get the context <see cref="Store"/>
		/// </summary>
		public Store Store
		{
			get
			{
				return myStore;
			}
		}
		#endregion // Access Properties
		#region IPropertyProviderService implementation
		private void AddOrRemovePropertyProvider(Type extendableElementType, ProviderInstance providerInstance, bool includeSubtypes, EventHandlerAction action)
		{
			bool register = action == EventHandlerAction.Add;
			if (register)
			{
				this.RegisterPropertyProvider(extendableElementType.TypeHandle, providerInstance);
			}
			else
			{
				this.UnregisterPropertyProvider(extendableElementType.TypeHandle, providerInstance);
			}
			if (includeSubtypes)
			{
				Store store = this.myStore;
				DomainClassInfo domainClassInfo = store.DomainDataDirectory.FindDomainClass(extendableElementType);
				if (null != domainClassInfo)
				{
					foreach (DomainClassInfo subtypeInfo in domainClassInfo.AllDescendants)
					{
						if (register)
						{
							this.RegisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, providerInstance);
						}
						else
						{
							this.UnregisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, providerInstance);
						}
					}
				}
			}
		}
		void IPropertyProviderService.AddOrRemovePropertyProvider(Type extendableElementType, PropertyProvider provider, bool includeSubtypes, EventHandlerAction action)
		{
			if ((object)provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			AddOrRemovePropertyProvider(extendableElementType, new ProviderInstance(provider), includeSubtypes, action);
		}
		void IPropertyProviderService.AddOrRemovePropertyProvider(Type extendableElementType, IPropertyProvider provider, bool includeSubtypes, EventHandlerAction action)
		{
			if ((object)provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			AddOrRemovePropertyProvider(extendableElementType, new ProviderInstance(provider), includeSubtypes, action);
		}
		private void RegisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, ProviderInstance providerInstance)
		{
			Dictionary<RuntimeTypeHandle, LinkedNode<ProviderInstance>> providerDictionary = this.myProviderDictionary;
			LinkedNode<ProviderInstance> existingProviderNode;
			if (providerDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingProviderNode))
			{
				LinkedNode<ProviderInstance> lastNode = null;
				LinkedNode<ProviderInstance> testNode = existingProviderNode;
				while (testNode != null)
				{
					if (testNode.Value.IsEquivalentTo(providerInstance))
					{
						return; // Don't add the same callback twice
					}
					lastNode = testNode;
					testNode = testNode.Next;
				}
				lastNode.SetNext(new LinkedNode<ProviderInstance>(providerInstance), ref existingProviderNode);
			}
			else
			{
				providerDictionary[extendableElementRuntimeTypeHandle] = new LinkedNode<ProviderInstance>(providerInstance);
			}
		}
		private void UnregisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, ProviderInstance providerInstance)
		{
			Dictionary<RuntimeTypeHandle, LinkedNode<ProviderInstance>> providerDictionary = this.myProviderDictionary;
			LinkedNode<ProviderInstance> existingProviderNode;
			if (providerDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingProviderNode))
			{
				LinkedNode<ProviderInstance> testNode = existingProviderNode;
				while (testNode != null)
				{
					if (testNode.Value.IsEquivalentTo(providerInstance))
					{
						LinkedNode<ProviderInstance> head = existingProviderNode;
						testNode.Detach(ref head);
						if (head == null)
						{
							providerDictionary.Remove(extendableElementRuntimeTypeHandle);
						}
						else if (head != existingProviderNode)
						{
							providerDictionary[extendableElementRuntimeTypeHandle] = head;
						}
						return;
					}
					testNode = testNode.Next;
				}
			}
		}
		void IPropertyProviderService.GetProvidedProperties(object extendableElement, PropertyDescriptorCollection properties)
		{
			if (extendableElement == null)
			{
				throw new ArgumentNullException("extendableElement");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			LinkedNode<ProviderInstance> providerNode;
			if (myProviderDictionary.TryGetValue(extendableElement.GetType().TypeHandle, out providerNode))
			{
				while (providerNode != null)
				{
					providerNode.Value.GetProvidedProperties(extendableElement, properties);
					providerNode = providerNode.Next;
				}
			}
		}
		void IPropertyProviderService.GetProvidedProperties(Type extendableElementType, PropertyDescriptorCollection properties)
		{
			if (extendableElementType == null)
			{
				throw new ArgumentNullException("extendableElementType");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			LinkedNode<ProviderInstance> providerNode;
			if (myProviderDictionary.TryGetValue(extendableElementType.TypeHandle, out providerNode))
			{
				while (providerNode != null)
				{
					providerNode.Value.GetProvidedProperties(extendableElementType, properties);
					providerNode = providerNode.Next;
				}
			}
		}
		void IPropertyProviderService.AddOrRemoveChangeListener(Type extendableElementType, ExtensionPropertyChangedEventHandler handler, EventHandlerAction action)
		{
			if (extendableElementType == null)
			{
				throw new ArgumentNullException("extendableElementType");
			}
			if ((object)handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			LinkedNode<ProviderInstance> providerNode;
			if (myProviderDictionary.TryGetValue(extendableElementType.TypeHandle, out providerNode))
			{
				while (providerNode != null)
				{
					providerNode.Value.AddOrRemoveChangeListener(handler, action);
					providerNode = providerNode.Next;
				}
			}
		}
		#endregion // IPropertyProviderService implementation
	}
	#endregion // PropertyProviderService class
}
