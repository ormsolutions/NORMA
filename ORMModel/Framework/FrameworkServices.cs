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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.Modeling
{
	#region IFrameworkServices interface
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
		/// Can be implemented using an instance of the <see cref="SurveyTree"/> class which
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
		/// 
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
	#region IAllowsStandardCommands interface
	/// <summary>
	/// Implement the <see cref="IAllowStandardCommands"/> to turn
	/// on standard command handling for a shape or element. Standard
	/// commands allow element and shape deletion, shape alignment, and layout.
	/// </summary>
	public interface IAllowStandardCommands
	{
	}
	#endregion // IAllowsStandardCommands interface
}
