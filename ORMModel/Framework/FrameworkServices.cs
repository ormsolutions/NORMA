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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Framework
{
	#region IFrameworkServices interface
	/// <summary>
	/// Specify how an automatically added element should be
	/// treated by a presentation layer.
	/// </summary>
	public enum AutomatedElementDirective
	{
		/// <summary>
		/// No directive is specified for the element
		/// </summary>
		None,
		/// <summary>
		/// The element was added automatically and should always be ignored
		/// by the presentation layer.
		/// </summary>
		Ignore,
		/// <summary>
		/// The element was added intentionally and should never be ignored
		/// by the presentation layer. If multiple directives are provided,
		/// this takes precedence over <see cref="Ignore"/>
		/// </summary>
		NeverIgnore,
	}
	/// <summary>
	/// A callback used by <see cref="IFrameworkServices.AutomatedElementFilter"/>
	/// and <see cref="IFrameworkServices.GetAutomatedElementDirective"/>
	/// </summary>
	/// <param name="element">The element to test</param>
	/// <returns><see cref="AutomatedElementDirective"/></returns>
	public delegate AutomatedElementDirective AutomatedElementFilterCallback(ModelElement element);
	/// <summary>
	/// An interface that should be implemented by a <see cref="Store"/> that
	/// loads the domain models. This is meant to act as a base interface for
	/// tool-specific interfaces that derive from it.
	/// </summary>
	public interface IFrameworkServices
	{
		/// <summary>
		/// Retrieve the <see cref="IPropertyProviderService"/> service for registering
		/// and unregistering <see cref="PropertyProvider"/> delegates to enable extension
		/// properties to be added to any timeof element. Can be implemented using the
		/// <see cref="T:PropertyProviderService"/> class.
		/// </summary>
		IPropertyProviderService PropertyProviderService { get;}
		/// <summary>
		/// Retrieve the <see cref="INotifySurveyElementChanged"/> interface for this store.
		/// Can be implemented using an instance of the <see cref="SurveyTree{Store}"/> class that
		/// implements this interface.
		/// </summary>
		INotifySurveyElementChanged NotifySurveyElementChanged { get;}
		/// <summary>
		/// Retrieve a cached set of domain model instances that implement the requested interface.
		/// Can be implemented using the <see cref="TypedDomainModelProviderCache"/> class.
		/// </summary>
		/// <typeparam name="T">The interface to test</typeparam>
		/// <returns>An array of instances, or null</returns>
		T[] GetTypedDomainModelProviders<T>() where T : class;
		/// <summary>
		/// Retrieve the <see cref="ICopyClosureManager"/> interface for this store.
		/// Can be implemented using an instance of the <see cref="CopyClosureManager"/>
		/// class that implements this interface.
		/// </summary>
		ICopyClosureManager CopyClosureManager { get;}
		/// <summary>
		/// Add callbacks to determine the result of <see cref="GetAutomatedElementDirective"/>.
		/// Can be implemented by deferring to the <see cref="AutomatedElementFilterService"/> class.
		/// Callbacks registered with this event are generally removed at the end of
		/// a transaction. If the callback is not removed, it should be added with an implementation
		/// of the <see cref="IPermanentAutomatedElementFilterProvider"/> on a domain model.
		/// </summary>
		event AutomatedElementFilterCallback AutomatedElementFilter;
		/// <summary>
		/// Provided directives regarding automatically added elements based
		/// on listeners attached to <see cref="AutomatedElementFilter"/>.
		/// This allows rules and editors to easily notify presentation layers
		/// to respond differently when new elements are being added in
		/// an automated fashion. Can be implemented by deferring to the
		/// <see cref="AutomatedElementFilterService"/> class.
		/// </summary>
		AutomatedElementDirective GetAutomatedElementDirective(ModelElement element);
	}
	#endregion // IFrameworkServices interface
	#region IRepresentedModelElements interface
	/// <summary>
	/// General mechanism to retrieve a ModelElement associated
	/// with an arbitrary object.
	/// </summary>
	public interface IRepresentModelElements
	{
		/// <summary>
		/// Retrieve the model element associated with this item.
		/// </summary>
		ModelElement[] GetRepresentedElements();
	}
	#endregion // IRepresentedModelElements interface
	#region IElementReference interface
	/// <summary>
	/// This element represents a reference to another
	/// element. Generally, most actions on the element
	/// will also apply to the reference with the notable
	/// exception of deletion.
	/// </summary>
	public interface IElementReference
	{
		/// <summary>
		/// The referenced element
		/// </summary>
		object ReferencedElement { get;}
	}
	#endregion // IElementReference interface
	#region DomainModelTypeProviderCache class
	/// <summary>
	/// Helper class to implement <see cref="M:IFrameworkServices.GetTypedDomainModelProviders"/>
	/// </summary>
	public sealed class TypedDomainModelProviderCache
	{
		private Store myStore;
		private Dictionary<Type, Array> myProviderCache;
		/// <summary>
		/// Create a new DomainModelTypeProviderCache
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public TypedDomainModelProviderCache(Store store)
		{
			myStore = store;
		}
		/// <summary>
		/// The current <see cref="Store"/>
		/// </summary>
		public Store Store
		{
			get
			{
				return myStore;
			}
		}
		/// <summary>
		/// Get an array of providers of the requested type, or null if the interface is not implemented
		/// </summary>
		public T[] GetTypedDomainModelProviders<T>() where T : class
		{
			T[] retVal;
			Dictionary<Type, Array> cache = myProviderCache;
			Type TType = typeof(T);
			if (cache == null)
			{
				myProviderCache = cache = new Dictionary<Type, Array>();
				retVal = Utility.GetTypedDomainModels<T>(myStore.DomainModels);
				cache.Add(TType, retVal);
			}
			else
			{
				Array outArray;
				if (cache.TryGetValue(TType, out outArray))
				{
					retVal = (T[])outArray;
				}
				else
				{
					retVal = Utility.GetTypedDomainModels<T>(myStore.DomainModels);
					cache.Add(TType, retVal);
				}
			}
			return retVal;
		}
	}
	#endregion // DomainModelTypeProviderCache class
	#region IMergeIndirectElements<T> interface
	/// <summary>
	/// Provide a mechanism for merging extension elements into a parent element.
	/// Retrieve extenders through <see cref="M:IFrameworkServices.GetTypedDomainModelProviders"/>.
	/// Defer to extender elements from a <see cref="IMergeElements"/> implementation.
	/// </summary>
	/// <typeparam name="T">A <see cref="ModelElement"/> type</typeparam>
	public interface IMergeIndirectElements<T> where T : ModelElement
	{
		/// <summary>
		/// Test if an element can be indirectly merged into a parent context.
		/// See <see cref="M:IMergeElements.CanMerge"/> for additional information.
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="rootElement">The element to merge</param>
		/// <param name="elementGroupPrototype">The element prototype information</param>
		/// <returns><see langword="true"/> if merge is possible</returns>
		bool CanMergeIndirect(T mergeContext, ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype);
		/// <summary>
		/// Chose an indirect merge target for an <see cref="ElementGroup"/> into a parent context.
		/// See <see cref="M:IMergeElements.ChooseIndirectMergeTarget(ElementGroup)"/> for additional information.
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="elementGroup">The <see cref="ElementGroup"/> to merge</param>
		/// <returns>A merge target, or <see langword="null"/> if a merge target is not available</returns>
		ModelElement ChooseIndirectMergeTarget(T mergeContext, ElementGroup elementGroup);
		/// <summary>
		/// Chose an indirect merge target for an <see cref="ElementGroupPrototype"/> into a parent context.
		/// See <see cref="M:IMergeElements.ChooseIndirectMergeTarget(ElementGroupPrototype)"/> for additional information.
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="elementGroupPrototype">The element prototype information</param>
		/// <returns>A merge target, or <see langword="null"/> if a merge target is not available</returns>
		ModelElement ChooseIndirectMergeTarget(T mergeContext, ElementGroupPrototype elementGroupPrototype);
		/// <summary>
		/// Configure an <see cref="ElementGroup"/> into an indirect context
		/// See <see cref="M:IMergeElements.MergeConfigure"/> for additional information.
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="elementGroup">The <see cref="ElementGroup"/> to merge</param>
		/// <returns><see langword="true"/> if the element is properly merged</returns>
		bool MergeConfigureIndirect(T mergeContext, ElementGroup elementGroup);
		/// <summary>
		/// Indirectly disconnect an element from a parent context
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="sourceElement">The element to disconnect</param>
		/// <returns><see langword="true"/> if the source element was successfully disconnected</returns>
		bool MergeDisconnectIndirect(T mergeContext, ModelElement sourceElement);
		/// <summary>
		/// Indirectly relate an element into a parent context
		/// </summary>
		/// <param name="mergeContext">The context parent element</param>
		/// <param name="sourceElement">The element to attach</param>
		/// <param name="elementGroup">The context <see cref="ElementGroup"/> that the element came from</param>
		/// <returns><see langword="true"/> if the source element was successfully linked</returns>
		bool MergeRelateIndirect(T mergeContext, ModelElement sourceElement, ElementGroup elementGroup);
	}
	#endregion // IMergeIndirectElements<T> interface
	#region IAllowStandardCommands interface
	/// <summary>
	/// Implement IAllowStandardCommands to enable standard command
	/// handling for a shape or element. Standard commands allow
	/// element and shape deletion, shape alignment, and layout.
	/// </summary>
	public interface IAllowStandardCommands
	{
	}
	#endregion // IAllowStandardCommands interface
	#region ICustomElementDeletion interface
	/// <summary>
	/// Implement ICustomElementDeletion to replace the standard
	/// <see cref="ModelElement.Delete()"/> to handle deletion of
	/// the implementing element. ICustomElementDeletion can be
	/// coupled with <see cref="IAllowStandardCommands"/> to
	/// enable deletion on extension elements.
	/// </summary>
	public interface ICustomElementDeletion
	{
		/// <summary>
		/// Delete the current element.
		/// </summary>
		void DeleteCustomElement();
	}
	#endregion // ICustomElementDeletion interface
	#region IPersistentSessionKeys interface
	/// <summary>
	/// Allow a <see cref="DomainModel"/> to provide a set
	/// of keys used in the <see cref="Store.PropertyBag"/>
	/// that need to be maintained across a store reload.
	/// </summary>
	public interface IPersistentSessionKeys
	{
		/// <summary>
		/// Retrieve keys used with this domain model
		/// that need to be copied into the <see cref="Store.PropertyBag"/>
		/// of a reloaded <see cref="Store"/>
		/// </summary>
		IEnumerable<object> GetPersistentSessionKeys();
	}
	#endregion // IPersistentSessionKeys interface
	#region IPermanentAutomatedElementFilterProvider interface
	/// <summary>
	/// Allow a domain model to install automated element filters that
	/// are always active. These filters are automatically included in
	/// requests to <see cref="IFrameworkServices.GetAutomatedElementDirective"/>.
	/// Permanent filters apply to all transactions, whereas <see cref="IFrameworkServices.AutomatedElementFilter"/>
	/// should be used to install filters for a specific transaction.
	/// </summary>
	public interface IPermanentAutomatedElementFilterProvider
	{
		/// <summary>
		/// Provided automated element filters to control creation of
		/// display elements for automatically created model elements.
		/// </summary>
		IEnumerable<AutomatedElementFilterCallback> GetAutomatedElementFilters();
	}
	#endregion // IPermanentAutomatedElementFilterProvider interface
	#region AutomatedElementFilterService class
	/// <summary>
	/// A helper class to provide a stock implementation of the
	/// <see cref="IFrameworkServices.AutomatedElementFilter"/> event
	/// and <see cref="IFrameworkServices.GetAutomatedElementDirective"/> methods.
	/// </summary>
	public sealed class AutomatedElementFilterService
	{
		private Delegate myAutomatedElementFilter;
		/// <summary>
		/// Create an automated element filter with permanent filters automatically installed.
		/// </summary>
		/// <param name="services">A <see cref="IFrameworkServices"/> instance.</param>
		public AutomatedElementFilterService(IFrameworkServices services)
		{
			IPermanentAutomatedElementFilterProvider[] providers = services.GetTypedDomainModelProviders<IPermanentAutomatedElementFilterProvider>();
			if (providers != null)
			{
				Delegate filter = null;
				for (int i = 0; i < providers.Length; ++i)
				{
					foreach (AutomatedElementFilterCallback callback in providers[i].GetAutomatedElementFilters())
					{
						filter = Delegate.Combine(filter, callback);
					}
				}
				myAutomatedElementFilter = filter;
			}
		}
		/// <summary>
		/// Implement the <see cref="IFrameworkServices.GetAutomatedElementDirective"/> method
		/// </summary>
		public AutomatedElementDirective GetAutomatedElementDirective(ModelElement element)
		{
			AutomatedElementDirective retVal = AutomatedElementDirective.None;
			Delegate filterList = myAutomatedElementFilter;
			if (filterList != null)
			{
				Delegate[] targets = filterList.GetInvocationList();
				for (int i = 0; i < targets.Length; ++i)
				{
					switch (((AutomatedElementFilterCallback)targets[i])(element))
					{
						case AutomatedElementDirective.NeverIgnore:
							// Strongest form, return immediately
							return AutomatedElementDirective.NeverIgnore;
						case AutomatedElementDirective.Ignore:
							retVal = AutomatedElementDirective.Ignore;
							break;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Implement the <see cref="IFrameworkServices.AutomatedElementFilter"/> event
		/// </summary>
		public event AutomatedElementFilterCallback AutomatedElementFilter
		{
			add
			{
				myAutomatedElementFilter = Delegate.Combine(myAutomatedElementFilter, value);
			}
			remove
			{
				myAutomatedElementFilter = Delegate.Remove(myAutomatedElementFilter, value);
			}
		}
	}
	#endregion // AutomatedElementFilterService class
	#region ICreateSignalTransactionItems
	/// <summary>
	/// Some model changes are used solely to trigger other behavior within
	/// a transaction. However, the system does not distinguish between
	/// these changes and normal changes. If a signal occurs without a normal
	/// change, then a transaction may appear to be meaningful to the user
	/// by displaying in the undo/redo stack even when no significant changes
	/// have occurred. A domain model class can implement this interface to
	/// indicate which changes are signal changes and can be safely ignored when
	/// determining if a transacted change should be displayed to the user.
	/// </summary>
	/// <remarks>Currently, we are only checking for property changes. This may
	/// be updated in the future to enable other types of signals to be ignored.</remarks>
	public interface IRegisterSignalChanges
	{
		/// <summary>
		/// Enumerate domain properties defined by this domain class that result
		/// in signal changes only and should not be visible to the user if they
		/// do not occur with other non-signal changes.
		/// </summary>
		/// <returns>Enumeration of domain property id/callback function pairs.
		/// If a callback function is not provided, then all changes of this
		/// type can be ignored. Otherwise, return true from the registered
		/// predicate to support a change.</returns>
		IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> GetSignalPropertyChanges();
	}
	#endregion // ICreateSignalTransactionItems
}
