#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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

namespace Neumont.Tools.Modeling.Design
{
	#region PropertyProvider delegate
	/// <summary>
	/// Adds extension <see cref="PropertyDescriptor"/>s for a <see cref="ModelElement"/> instance specified
	/// by <paramref name="extendableElement"/> to the <see cref="PropertyDescriptorCollection"/> specified by
	/// <paramref name="properties"/>.
	/// </summary>
	public delegate void PropertyProvider(ModelElement extendableElement, PropertyDescriptorCollection properties);
	#endregion // PropertyProvider delegate
	#region IPropertyProviderService interface
	/// <summary>
	/// Provides methods for registrating and unregistrating <see cref="PropertyProvider"/>s for
	/// <see cref="ModelElement"/> instances.
	/// </summary>
	public interface IPropertyProviderService
	{
		/// <summary>
		/// Registers or unregisters the <see cref="PropertyProvider"/> specified by <paramref name="provider"/> for the
		/// type specified by <typeparamref name="TExtendableElement"/>.
		/// </summary>
		/// <typeparam name="TExtendableElement">
		/// The type for which the <see cref="PropertyProvider"/> should be added. This type specified must
		/// by a subtype of <see cref="ModelElement"/>.
		/// </typeparam>
		/// <param name="provider">
		/// The <see cref="PropertyProvider"/> being registered.
		/// </param>
		/// <param name="includeSubtypes">
		/// Specifies whether the <see cref="PropertyProvider"/> should also be registered for subtypes of
		/// <typeparamref name="TExtendableElement"/>.
		/// </param>
		/// <param name="action">
		/// Specifies whether the property provider is being added or removed. See <see cref="EventHandlerAction"/></param>
		void AddOrRemovePropertyProvider<TExtendableElement>(PropertyProvider provider, bool includeSubtypes, EventHandlerAction action)
			where TExtendableElement : ModelElement;

		/// <summary>
		/// Adds extension <see cref="PropertyDescriptor"/>s for the <see cref="ModelElement"/> instance specified
		/// by <paramref name="extendableElement"/> to the <see cref="PropertyDescriptorCollection"/> specified by
		/// <paramref name="properties"/>.
		/// </summary>
		void GetProvidedProperties(ModelElement extendableElement, PropertyDescriptorCollection properties);
	}
	#endregion // IPropertyProviderService interface
	#region PropertyProviderService class
	/// <summary>
	/// A standard implementation of <see cref="IPropertyProviderService"/>
	/// </summary>
	public sealed class PropertyProviderService : IPropertyProviderService
	{
		#region Member Variables
		private readonly Store myStore;
		private readonly Dictionary<RuntimeTypeHandle, PropertyProvider> myProviderDictionary;
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
			this.myProviderDictionary = new Dictionary<RuntimeTypeHandle, PropertyProvider>(RuntimeTypeHandleComparer.Instance);
		}
		#endregion // Constructor
		#region IPropertyProviderService implementation
		void IPropertyProviderService.AddOrRemovePropertyProvider<TExtendableElement>(PropertyProvider provider, bool includeSubtypes, EventHandlerAction action)
		{
			if ((object)provider == null)
			{
				throw new ArgumentNullException("provider");
			}

			bool register = action == EventHandlerAction.Add;

			Type extendableElementType = typeof(TExtendableElement);

			if (register)
			{
				this.RegisterPropertyProvider(extendableElementType.TypeHandle, provider);
			}
			else
			{
				this.UnregisterPropertyProvider(extendableElementType.TypeHandle, provider);
			}
			if (includeSubtypes)
			{
				Store store = this.myStore;
				DomainClassInfo domainClassInfo = store.DomainDataDirectory.GetDomainClass(extendableElementType);
				foreach (DomainClassInfo subtypeInfo in domainClassInfo.AllDescendants)
				{
					if (register)
					{
						this.RegisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, provider);
					}
					else
					{
						this.UnregisterPropertyProvider(subtypeInfo.ImplementationClass.TypeHandle, provider);
					}
				}
			}
		}
		private void RegisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, PropertyProvider provider)
		{
			Dictionary<RuntimeTypeHandle, PropertyProvider> providerDictionary = this.myProviderDictionary;
			PropertyProvider existingPropertyProvider;
			providerDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingPropertyProvider);
			providerDictionary[extendableElementRuntimeTypeHandle] = (PropertyProvider)Delegate.Combine(existingPropertyProvider, provider);
		}

		private void UnregisterPropertyProvider(RuntimeTypeHandle extendableElementRuntimeTypeHandle, PropertyProvider provider)
		{
			Dictionary<RuntimeTypeHandle, PropertyProvider> providerDictionary = this.myProviderDictionary;
			PropertyProvider existingPropertyProvider;
			providerDictionary.TryGetValue(extendableElementRuntimeTypeHandle, out existingPropertyProvider);
			existingPropertyProvider = (PropertyProvider)Delegate.Remove(existingPropertyProvider, provider);
			if ((object)existingPropertyProvider == null)
			{
				providerDictionary.Remove(extendableElementRuntimeTypeHandle);
			}
			else
			{
				providerDictionary[extendableElementRuntimeTypeHandle] = existingPropertyProvider;
			}
		}
		void IPropertyProviderService.GetProvidedProperties(ModelElement extendableElement, PropertyDescriptorCollection properties)
		{
			if (extendableElement == null)
			{
				throw new ArgumentNullException("extendableElement");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			PropertyProvider provider;
			if (this.myProviderDictionary.TryGetValue(extendableElement.GetType().TypeHandle, out provider))
			{
				// We don't need to check provider for null, since UnregisterPropertyProvider would have removed it from the
				// dictionary if there were no providers left.
				provider(extendableElement, properties);
			}
		}
		#endregion // IPropertyProviderService implementation
	}
	#endregion // PropertyProviderService class
}
