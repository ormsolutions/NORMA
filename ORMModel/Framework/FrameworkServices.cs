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
		/// Retrieve a cached set of domain model instances that implement the requested interface.
		/// Can be implemented using the <see cref="TypedDomainModelProviderCache"/> class.
		/// </summary>
		/// <typeparam name="T">The interface to test</typeparam>
		/// <param name="dependencyOrder">If true, then return multiple matches in
		/// dependency order, with the least dependent models first.</param>
		/// <returns>An array of instances, or null</returns>
		T[] GetTypedDomainModelProviders<T>(bool dependencyOrder) where T : class;
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
	#region SoftExtensionDependencyAttribute class
	/// <summary>
	/// An attribute to add to a domain model to create a dependency on a domain
	/// model that might not be loaded. This will force the attributed domain model
	/// to load before or after the targeted domain model if the targeted model is
	/// loaded for some other reason. This allows relative load order to be specified
	/// between extension models that do not have hard dependencies on each other.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class SoftExtensionDependencyAttribute : Attribute
	{
		private Guid myDependentExtensionId;
		private bool myLoadBefore;
		/// <summary>
		/// Create a default <see cref="SoftExtensionDependencyAttribute"/>
		/// </summary>
		public SoftExtensionDependencyAttribute()
		{
			myDependentExtensionId = Guid.Empty;
		}
		/// <summary>
		/// Create a <see cref="SoftExtensionDependencyAttribute"/> for a specific domain model.
		/// </summary>
		/// <param name="domainModelId">The model if for soft dependency.</param>
		public SoftExtensionDependencyAttribute(string domainModelId)
		{
			myDependentExtensionId = new Guid(domainModelId);
		}
		/// <summary>
		/// Create a <see cref="SoftExtensionDependencyAttribute"/> for a specific domain model.
		/// </summary>
		/// <param name="domainModelId">The model if for soft dependency.</param>
		/// <param name="loadBefore">Load this model before the referenced dependency instead of after.</param>
		public SoftExtensionDependencyAttribute(string domainModelId, bool loadBefore)
		{
			myDependentExtensionId = new Guid(domainModelId);
			myLoadBefore = loadBefore;
		}
		/// <summary>
		/// The <see cref="Guid"/> form of the identifier.
		/// </summary>
		public Guid DependentExtensionId
		{
			get
			{
				return myDependentExtensionId;
			}
		}
		/// <summary>
		/// The string form of the model identifier. Used during
		/// attribute construction.
		/// </summary>
		public string DependentExtensionIdString
		{
			get
			{
				return myDependentExtensionId.ToString();
			}
			set
			{
				myDependentExtensionId = new Guid(value);
			}
		}
		/// <summary>
		/// Load this model before the dependency instead of after.
		/// </summary>
		public bool LoadBefore
		{
			get
			{
				return myLoadBefore;
			}
			set
			{
				myLoadBefore = value;
			}
		}
		/// <summary>
		/// Standard override, empty if id not set
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return myDependentExtensionId == Guid.Empty;
		}
	}
	#endregion // SoftExtensionDependencyAttribute class
	#region AlsoLoadDomainModelAttribute class
	/// <summary>
	/// Add as an attribute to a registered extension type to force additional domain
	/// models to load with the registered domain model. Apart from being public,
	/// there are no restrictions or assumptions about the secondary models, including
	/// no topological dependencies between the models.
	/// </summary>
	/// <remarks>These models are assumed to be secondary, so they do not appear in the
	/// extension manager. All extensions are identified with a namespace URI, so we require
	/// URIs here as well for consistency.</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class AlsoLoadDomainModelAttribute : Attribute
	{
		private string myNamespaceURI;
		private Type myAlsoLoadType;
		private bool myIsNonGenerative;
		/// <summary>
		/// Create a default <see cref="AlsoLoadDomainModelAttribute"/>
		/// </summary>
		public AlsoLoadDomainModelAttribute()
		{
		}
		/// <summary>
		/// Create a <see cref="AlsoLoadDomainModelAttribute"/> for a specific domain model.
		/// </summary>
		public AlsoLoadDomainModelAttribute(string namespaceURI, Type type)
		{
			myAlsoLoadType = type;
			myNamespaceURI = namespaceURI;
			myIsNonGenerative = false;
		}
		/// <summary>
		/// Create a <see cref="AlsoLoadDomainModelAttribute"/> for a specific domain model.
		/// </summary>
		public AlsoLoadDomainModelAttribute(string namespaceURI, Type type, bool isNonGenerative)
		{
			myAlsoLoadType = type;
			myNamespaceURI = namespaceURI;
			myIsNonGenerative = isNonGenerative;
		}
		/// <summary>
		/// The additional domain model that is loaded with the context domain model.
		/// </summary>
		public Type AlsoLoadType
		{
			get
			{
				return myAlsoLoadType;
			}
			set
			{
				myAlsoLoadType = value;
			}
		}
		/// <summary>
		/// Load this model before the dependency instead of after.
		/// </summary>
		public string NamespaceURI
		{
			get
			{
				return myNamespaceURI;
			}
			set
			{
				myNamespaceURI = value;
			}
		}
		/// <summary>
		/// Treat this as a non-generative model for external loading.
		/// If any registered model that sets this attribute has the
		/// NonGenerative attribute marked then this is automatically
		/// considered non generative, so this only needs to be set
		/// if the primary model is used for code generation but the
		/// also-loaded model is not.
		/// </summary>
		public bool IsNonGenerative
		{
			get
			{
				return myIsNonGenerative;
			}
			set
			{
				myIsNonGenerative = value;
			}
		}
		/// <summary>
		/// Standard override, empty if data not set.
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return myNamespaceURI == null && myAlsoLoadType == null && !myIsNonGenerative;
		}
	}
	#endregion // AlsoLoadDomainModelAttribute class
	#region DomainModelTypeProviderCache class
	/// <summary>
	/// Helper class to implement <see cref="M:IFrameworkServices.GetTypedDomainModelProviders"/>
	/// </summary>
	public sealed class TypedDomainModelProviderCache
	{
		private Store myStore;
		private Dictionary<Type, Array> myProviderCache;
		private Dictionary<Type, Array> myDependecyOrderedProviderCache;
		private DomainModel[] myDependencyOrderedDomainModels;
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
		/// <param name="dependencyOrder">Return matching domain models in strict
		/// dependency order, with less dependent domain models listed first.</param>
		public T[] GetTypedDomainModelProviders<T>(bool dependencyOrder) where T : class
		{
			T[] retVal;
			Dictionary<Type, Array> cache = dependencyOrder ? myDependecyOrderedProviderCache : myProviderCache;
			Type TType = typeof(T);
			if (cache == null)
			{
				cache = new Dictionary<Type, Array>();
				IEnumerable<DomainModel> models;
				if (dependencyOrder)
				{
					myDependecyOrderedProviderCache = cache;
					models = myDependencyOrderedDomainModels = GetDependencyOrderedDomainModels(myStore);
				}
				else
				{
					myProviderCache = cache;
					models = myStore.DomainModels;
				}
				retVal = Utility.GetTypedDomainModels<T>(models);
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
					retVal = Utility.GetTypedDomainModels<T>(dependencyOrder ? (IEnumerable<DomainModel>)myDependencyOrderedDomainModels : myStore.DomainModels);
					cache.Add(TType, retVal);
				}
			}
			return retVal;
		}
		private static DomainModel[] GetDependencyOrderedDomainModels(Store store)
		{
			// Get basic information
			ICollection<DomainModel> startModels = store.DomainModels;
			int modelCount = startModels.Count;
			DomainModel[] allModels = new DomainModel[modelCount];
			startModels.CopyTo(allModels, 0);

			// There is no way to do a dependency graph sort using a quicksort, so we do a
			// topological sort instead (this is a directed acyclic graph)

			// Track head nodes (that nothing depends on). We default these to the original
			// model index, then change to ~index as we examine the contents to indicate a
			// non-head node. This also gives us a quick lookup to see if a referenced target
			// is actually included in our original list.
			Dictionary<Guid, int> headNodes = new Dictionary<Guid, int>();
			for (int i = 0; i < modelCount; ++i)
			{
				headNodes[allModels[i].DomainModelInfo.Id] = i;
			}

			// A dictionary of direct dependencies. We filter the dependencies before adding to
			// guarantee that the target is available.
			Dictionary<Guid, List<Guid>> directDependencies = new Dictionary<Guid, List<Guid>>();

			// Retrieve direct dependency information for both hard and soft dependencies.
			// Note that soft dependency allows reversing the dependency, so dependencies
			// can be added before or after the model dependencies are walked. Defer all
			// sort operations until all models are processed.
			for (int iModel = 0; iModel < modelCount; ++iModel)
			{
				DomainModel domainModel = allModels[iModel];
				Type type = domainModel.GetType();
				Guid currentModelId = domainModel.DomainModelInfo.Id;
				List<Guid> dependentIds;
				bool currentModelHadDependencies = directDependencies.TryGetValue(currentModelId, out dependentIds);
#if VISUALSTUDIO_10_0
				object[] extendsAttributes = type.GetCustomAttributes(typeof(DependsOnDomainModelAttribute), false);
#else
				object[] extendsAttributes = type.GetCustomAttributes(typeof(ExtendsDomainModelAttribute), false);
#endif
				for (int i = 0; i < extendsAttributes.Length; ++i)
				{
#if VISUALSTUDIO_10_0
					Type extendedModelType = ((DependsOnDomainModelAttribute)extendsAttributes[i]).ExtendedDomainModelType;
					object[] extensionIdAttributes = extendedModelType.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
					Guid dependentId = (extensionIdAttributes.Length != 0) ? ((DomainObjectIdAttribute)extensionIdAttributes[0]).Id : Guid.Empty;
#else
					Guid dependentId = ((ExtendsDomainModelAttribute)extendsAttributes[i]).ExtendedModelId;
#endif
					int headIndex;
					if (dependentId != Guid.Empty && headNodes.TryGetValue(dependentId, out headIndex))
					{
						if (headIndex >= 0)
						{
							headNodes[dependentId] = ~headIndex;
						}
						(dependentIds ?? (dependentIds = new List<Guid>())).Add(dependentId);
					}
				}

				// Do the same for soft extensions, which apply only if the target model is loaded
				// but have no effect otherwise.
				extendsAttributes = type.GetCustomAttributes(typeof(SoftExtensionDependencyAttribute), false);
				for (int i = 0; i < extendsAttributes.Length; ++i)
				{
					SoftExtensionDependencyAttribute softExtension = ((SoftExtensionDependencyAttribute)extendsAttributes[i]);
					Guid dependentId = softExtension.DependentExtensionId;
					int headIndex;
					if (dependentId != Guid.Empty && headNodes.TryGetValue(dependentId, out headIndex))
					{
						if (softExtension.LoadBefore)
						{
							// Reverse the dependency, modify the opposite list and
							// make sure this is marked as a leaf node.
							List<Guid> oppositeDependencies;
							if (directDependencies.TryGetValue(dependentId, out oppositeDependencies))
							{
								if (oppositeDependencies.Contains(currentModelId))
								{
									oppositeDependencies = null; // Sanity test, already tracked, don't touch it.
								}
							}
							else
							{
								directDependencies[dependentId] = oppositeDependencies = new List<Guid>();
							}
							if (oppositeDependencies != null)
							{
								oppositeDependencies.Add(currentModelId);

								// Flip the original index so we know this model is used.
								if (headNodes[currentModelId] == iModel)
								{
									headNodes[currentModelId] = ~iModel;
								}
							}
						}
						else
						{
							if (headIndex >= 0)
							{
								headNodes[dependentId] = ~headIndex;
							}
							(dependentIds ?? (dependentIds = new List<Guid>())).Add(dependentId);
						}
					}
				}

				if (dependentIds != null && !currentModelHadDependencies)
				{
					directDependencies[currentModelId] = dependentIds;
				}
			}

			// Sort all dependency lists based on the original order so we preserve as much
			// of the default (class name based) order as we can.
			foreach (List<Guid> dependentIds in directDependencies.Values)
			{
				if (dependentIds.Count > 1)
				{
					dependentIds.Sort(delegate (Guid x, Guid y)
					{
						int index1 = headNodes[x];
						if (index1 < 0)
						{
							index1 = ~index1;
						}
						int index2 = headNodes[y];
						if (index2 < 0)
						{
							index2 = ~index1;
						}
						return index1.CompareTo(index2);
					});
				}
			}

			// Apply a recursive depth-first-sort algorithm starting from head nodes, record the results.
			DomainModel[] retVal = new DomainModel[modelCount];
			int nextResultIndex = 0;
			foreach (KeyValuePair<Guid, int> kvp in headNodes)
			{
				if (kvp.Value >= 0) // Index not inverted, nothing references this
				{
					VisitDependencies(kvp.Key, headNodes, directDependencies, allModels, retVal, ref nextResultIndex);
				}
			}

			return retVal;
		}
		private static void VisitDependencies(Guid modelId, Dictionary<Guid, int> headNodes, Dictionary<Guid, List<Guid>> directDependencies, DomainModel[] allModels, DomainModel[] results, ref int nextResultIndex)
		{
			int modelIndex = headNodes[modelId];
			if (modelIndex < 0)
			{
				modelIndex = ~modelIndex;
			}
			DomainModel model = allModels[modelIndex];
			if (model == null)
			{
				// Already processed
				return;
			}
			allModels[modelIndex] = null;

			List<Guid> dependencies;
			if (directDependencies.TryGetValue(modelId, out dependencies))
			{
				for (int i = 0, count = dependencies.Count; i < count; ++i)
				{
					VisitDependencies(dependencies[i], headNodes, directDependencies, allModels, results, ref nextResultIndex);
				}
			}
			results[nextResultIndex] = model;
			++nextResultIndex;
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
	#region GeneratorTarget struct
	/// <summary>
	/// A structure representing a three part identifier that controls the
	/// generation of different output targets. The general pattern is that
	/// one artifact of a given type is generated per model file. Used with the
	/// <see cref="IGeneratorTargetList"/> interface, this allows multiple
	/// artifacts of the same type to be generated from the same model file.
	/// </summary>
	[Serializable]
	public struct GeneratorTarget
	{
		#region Member Variables
		/// <summary>
		/// The type of target to modify. A generator registers this basic
		/// string value in its list of generator target types. This is a
		/// case sensitive value.
		/// </summary>
		public readonly string TargetType;
		/// <summary>
		/// The name for this instance of the given type. This is used as
		/// a decorator in the file name of the given type. An empty or null
		/// return indicates no special name. The type and name pairs need
		/// to be unique across the model. This is a case-insensitive value.
		/// </summary>
		public readonly string TargetName;
		/// <summary>
		/// The identifier for this instance. This value depends on the type
		/// of data being used and will relate back to an id argument in the
		/// .orm file or some other unique value in another generator input,
		/// thereby allowing the generator to determine the data that should
		/// be used to create the correct artifact.
		/// </summary>
		/// <remarks>Although the type and id will generally be unique this is
		/// not actually required, so the identifier is ignored in equality
		/// and hashcode comparisons comparing two <see cref="GeneratorTarget"/> instances.
		/// Equality comparison ignores (ordinal) case for the target name.</remarks>
		public readonly string TargetId;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a new instance with a type, name and id
		/// </summary>
		public GeneratorTarget(string type, string name, string id)
		{
			TargetType = type;
			TargetName = name;
			TargetId = id;
		}
		#endregion // Constructors
		#region Equality routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is GeneratorTarget) && Equals((GeneratorTarget)obj);
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			string type = TargetType;
			string name = TargetName;
			return Utility.GetCombinedHashCode(type != null ? type.GetHashCode() : 0, name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(name) : 0);
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(GeneratorTarget obj)
		{
			// Note that the id is intentionally ignored for equality to allow an
			// instance to be used as a type/name key.
			return TargetType == obj.TargetType && StringComparer.OrdinalIgnoreCase.Equals(TargetName, obj.TargetName);
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(GeneratorTarget left, GeneratorTarget right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(GeneratorTarget left, GeneratorTarget right)
		{
			return !left.Equals(right);
		}
		#endregion // Equality routines
		#region Static helpers
		/// <summary>
		/// Consolidate data from all implementations of <see cref="IGeneratorTargetProvider"/> in loaded
		/// domain models into a dictionary keyed by generator type.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static Dictionary<string, GeneratorTarget[]> ConsolidateGeneratorTargets(IFrameworkServices services)
		{
			Dictionary<string, List<GeneratorTarget>> targetsByType = null;
			foreach (IGeneratorTargetProvider provider in services.GetTypedDomainModelProviders<IGeneratorTargetProvider>())
			{
				foreach (IGeneratorTargetList targetList in provider.GetGeneratorTargets())
				{
					IList<GeneratorTarget> localTargets;
					int targetCount;
					if (null != (localTargets = targetList.GeneratorTargets) &&
						0 != (targetCount = localTargets.Count))
					{
						for (int i = 0; i < targetCount; ++i)
						{
							GeneratorTarget target = localTargets[i];
							string targetType = target.TargetType;
							List<GeneratorTarget> combinedTargets;
							if (null == targetsByType)
							{
								targetsByType = new Dictionary<string, List<GeneratorTarget>>();
								targetsByType[targetType] = combinedTargets = new List<GeneratorTarget>();
							}
							else if (!targetsByType.TryGetValue(targetType, out combinedTargets))
							{
								combinedTargets = new List<GeneratorTarget>();
								targetsByType[targetType] = combinedTargets;
							}

							combinedTargets.Add(target);
						}
					}
				}
			}

			// This returns a dictionary keyed by a generator type
			// with array values corresponding to unique GeneratorType
			// instances for that target.
			Dictionary<string, GeneratorTarget[]> serializeableResult = null;
			if (targetsByType != null)
			{
				Dictionary<GeneratorTarget, object> keyedByTarget = new Dictionary<GeneratorTarget, object>();
				serializeableResult = new Dictionary<string, GeneratorTarget[]>();
				foreach (KeyValuePair<string, List<GeneratorTarget>> kvp in targetsByType)
				{
					List<GeneratorTarget> targetList = kvp.Value;
					keyedByTarget.Clear();
					int i;
					int count;
					for (i = 0, count = targetList.Count; i < count; ++i)
					{
						// This limits any duplicate key to a single entry
						keyedByTarget[targetList[i]] = null;
					}

					count = keyedByTarget.Count;
					GeneratorTarget[] targets = new GeneratorTarget[count];
					serializeableResult[kvp.Key] = targets;
					keyedByTarget.Keys.CopyTo(targets, 0);
				}
			}
			return serializeableResult;
		}
		#endregion // Static helpers
	}
	#endregion // GeneratorTarget struct
	#region IGeneratorTargetList interface
	/// <summary>
	/// An interface to implement on elements that controls the generation
	/// of different output targets.
	/// </summary>
	public interface IGeneratorTargetList
	{
		/// <summary>
		/// Generator targets to associate with a group setting.
		/// </summary>
		IList<GeneratorTarget> GeneratorTargets { get; }
	}
	#endregion // IGeneratorTargetList interface
	#region IGeneratorTargetProvider interface
	/// <summary>
	/// An interface to implement on a domain model to provide
	/// generator target objects. Each object then produces a
	/// generator target list.
	/// </summary>
	public interface IGeneratorTargetProvider
	{
		/// <summary>
		/// Enumeration of elements that can have generator targets
		/// </summary>
		IEnumerable<IGeneratorTargetList> GetGeneratorTargets();
	}
	#endregion // IGeneratorTargetProvider interface
}
