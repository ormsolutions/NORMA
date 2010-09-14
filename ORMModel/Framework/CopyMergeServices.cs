#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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

// Temporarily uncomment this line to enable helper
// routines useful during copy closure debugging.
 #define DEBUGHELPERS
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.Framework
{
	#region DomainRoleClosureRestriction struct
	/// <summary>
	/// Specify a domain role and associated role player restriction
	/// information to limit the domain role to a given type.
	/// </summary>
	public struct DomainRoleClosureRestriction
	{
		#region Fields and Constructors
		/// <summary>
		/// The domain role identifier
		/// </summary>
		public readonly Guid RoleId;
		/// <summary>
		/// The domain class identifier used to restrict the contents
		/// of the given role. Use <see cref="Guid.Empty"/> to specify
		/// no type restriction.
		/// </summary>
		public readonly Guid RolePlayerRestrictionClassId;
		/// <summary>
		/// If <see cref="RolePlayerRestrictionClassId"/> is set, then
		/// should this restriction also apply to derived classes?
		/// </summary>
		public readonly bool IncludeClassRestrictionDescendants;
		/// <summary>
		/// Create a <see cref="DomainRoleClosureRestriction"/> with no restrictions
		/// beyond the natural role player.
		/// </summary>
		/// <param name="domainRoleId">A domain role identifier.</param>
		public DomainRoleClosureRestriction(Guid domainRoleId)
		{
			RoleId = domainRoleId;
			RolePlayerRestrictionClassId = Guid.Empty;
			IncludeClassRestrictionDescendants = false;
		}
		/// <summary>
		/// Create a <see cref="DomainRoleClosureRestriction"/> restricted to
		/// a subset of the possible role player types.
		/// </summary>
		/// <param name="domainRoleId">A domain role identifier.</param>
		/// <param name="domainClassRestriction">Restrict the role players for this role
		/// to those satisfying the specified type.</param>
		/// <param name="includeDomainClassDescendants">Including domain class types that are
		/// descendants of <paramref name="domainClassRestriction"/>.</param>
		public DomainRoleClosureRestriction(Guid domainRoleId, Guid domainClassRestriction, bool includeDomainClassDescendants)
		{
			RoleId = domainRoleId;
			RolePlayerRestrictionClassId = domainClassRestriction;
			IncludeClassRestrictionDescendants = includeDomainClassDescendants;
		}
		#endregion // Fields and Constructors
		#region Equality overrides
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(DomainRoleClosureRestriction restriction1, DomainRoleClosureRestriction restriction2)
		{
			return restriction1.Equals(restriction2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(DomainRoleClosureRestriction restriction1, DomainRoleClosureRestriction restriction2)
		{
			return !(restriction1.Equals(restriction2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return obj is DomainRoleClosureRestriction && Equals((DomainRoleClosureRestriction)obj);
		}
		/// <summary>
		/// Typed Equals method, make equality symmetric
		/// </summary>
		public bool Equals(DomainRoleClosureRestriction obj)
		{
			return RoleId == obj.RoleId &&
				RolePlayerRestrictionClassId == obj.RolePlayerRestrictionClassId &&
				IncludeClassRestrictionDescendants == obj.IncludeClassRestrictionDescendants;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			return Utility.GetCombinedHashCode(RoleId.GetHashCode(), RolePlayerRestrictionClassId.GetHashCode(), IncludeClassRestrictionDescendants.GetHashCode());
		}
		#endregion // Equality overrides
	}
	#endregion // DomainRoleClosureRestriction struct
	#region CopyClosureBehavior enum
	/// <summary>
	/// The result of iterating a far role
	/// </summary>
	public enum CopyClosureBehavior
	{
		/// <summary>
		/// The role player reached through this relationship should be ignored
		/// </summary>
		Ignored,
		/// <summary>
		/// The role player reached through this relationship is a direct container
		/// of the opposite role player.
		/// </summary>
		Container,
		/// <summary>
		/// The role player reached through this relationship is a contained part
		/// of the opposite role player.
		/// </summary>
		ContainedPart,
		/// <summary>
		/// The role player reached through this relationship is part that is
		/// contained in a shared direct or indirect ancestor container.
		/// </summary>
		InternalReferencedPart,
		/// <summary>
		/// The role player reached through this relationship is an external
		/// part of the opposite role player. If the opposite role
		/// player is duplicated, then the composite part must be duplicated.
		/// </summary>
		ExternalCompositePart,
		/// <summary>
		/// The role player reached through this relationship is a required but
		/// an external part of the opposite role player. Duplicating the opposite
		/// role player does not require duplication of this element.
		/// </summary>
		ExternalReferencedPart,
	}
	#endregion // CopyClosureBehavior enum
	#region CopyClosureDirectiveOptions enum
	/// <summary>
	/// Options for copy and merge closure registration
	/// </summary>
	[Flags]
	public enum CopyClosureDirectiveOptions
	{
		/// <summary>
		/// No options specified
		/// </summary>
		None = 0,
		/// <summary>
		/// The closure conditions apply only to an opposite
		/// role player that is a primary selection.
		/// </summary>
		RootElementOnly = 1,
	}
	#endregion //CopyClosureDirectiveOptions enum
	#region EvaluateCopyClosure delegate
	/// <summary>
	/// A delegate for custom evaluation of a closure query
	/// </summary>
	/// <param name="link">The <see cref="ElementLink"/> relationship to test.</param>
	/// <returns>A <see cref="CopyClosureBehavior"/> value</returns>
	public delegate CopyClosureBehavior EvaluateCopyClosure(ElementLink link);
	#endregion // EvaluateCopyClosure delegate
	#region EvaluateImpliedReference delegate
	/// <summary>
	/// A delegate for evaluation of an implied reference.
	/// </summary>
	/// <param name="element">The element of a registered class type for this callback.</param>
	/// <param name="notifyImpliedReference">The callback for adding implied references.</param>
	public delegate void EvaluateImpliedReference(ModelElement element, Action<ModelElement> notifyImpliedReference);
	#endregion // EvaluateImpliedReference delegate
	#region ConditionalPropertyTest delegate
	/// <summary>
	/// A delegate for evaluation of a conditionally copied property.
	/// </summary>
	/// <param name="sourceElement">The element the property should be copied from.</param>
	/// <param name="targetElement">If this is not <see langword="null"/>, then the property is being merged into an existing element.</param>
	/// <returns><see langword="true"/> to all the property to copy. If multiple conditions are registered,
	/// then any false return will block the property from being copied.</returns>
	public delegate bool ConditionalPropertyTest(ModelElement sourceElement, ModelElement targetElement);
	#endregion // ConditionalPropertyTest delegate
	#region ICopyClosureProvider interface
	/// <summary>
	/// An interface implemented by a <see cref="DomainModel"/> to
	/// support retrieval of custom copy closure information requested
	/// by the framework-provided <see cref="ICopyClosureManager"/> manager.
	/// </summary>
	public interface ICopyClosureProvider
	{
		/// <summary>
		/// Add closure directives and ignored properties for this domain model.
		/// </summary>
		/// <param name="closureManager">The context copy closure manager.</param>
		void AddCopyClosureDirectives(ICopyClosureManager closureManager);
	}
	#endregion // ICopyClosureProvider interface
	#region IInitializeAfterCopy interface
	/// <summary>
	/// Implement this interface on an element to perform custom
	/// initialization based on the source element immediately
	/// after element creation.
	/// </summary>
	public interface IInitializeAfterCopy
	{
		/// <summary>
		/// Initialize an element during creation immediately
		/// after it is copied.
		/// </summary>
		/// <param name="basedOnElement">The element being copied.</param>
		void InitializedAfterCopy(ModelElement basedOnElement);
	}
	#endregion // IInitializeAfterCopy interface
	#region ICopyClosureIntegrationListener interface
	/// <summary>
	/// An interface implemented by a <see cref="DomainModel"/> to
	/// support rule modification and other changes at the beginning
	/// and end of a copy closure integration into a store.
	/// </summary>
	public interface ICopyClosureIntegrationListener
	{
		/// <summary>
		/// Copy closure integration is beginning.
		/// </summary>
		/// <param name="closureTransaction">The local <see cref="Transaction"/>
		/// created for handling the closure integration. This may be a nested
		/// transaction.</param>
		void BeginCopyClosureIntegration(Transaction closureTransaction);
		/// <summary>
		/// Copy closure integration has completed.
		/// </summary>
		/// <param name="closureTransaction">The local <see cref="Transaction"/>
		/// created for handling the closure integration. This may be a nested
		/// transaction.</param>
		void EndCopyClosureIntegration(Transaction closureTransaction);
	}
	#endregion // ICopyClosureIntegrationListener interface
	#region CopyMergeAction enum
	/// <summary>
	/// Describe the action to take when duplicating
	/// a copy closure. The actual action taken will
	/// depend on whether the source and target documents
	/// are the same, whether an equivalent element
	/// already exists, and user options.
	/// </summary>
	public enum CopyMergeAction
	{
		/// <summary>
		/// The element should be duplicated for a copy operation,
		/// but existing elements should be found for a merge operation.
		/// </summary>
		Duplicate,
		/// <summary>
		/// The element should be matched with an existing element
		/// for copy and merge operations, and will only be recreated
		/// if it does not exist in the target context.
		/// </summary>
		Match,
		/// <summary>
		/// The element should be created as a relationship. If both role
		/// players are matched, then attempt to match the link as well.
		/// If either endpoint is duplicated, then duplicate the link.
		/// </summary>
		Link,
	}
	#endregion // CopyMergeAction enum
	#region IClosureElement interface
	/// <summary>
	/// An interface representing an element that is part of
	/// an element closure.
	/// </summary>
	public interface IClosureElement
	{
		/// <summary>
		/// The element in the closure. This can be a
		/// <see cref="ModelElement"/> or an <see cref="ElementLink"/>
		/// </summary>
		ModelElement Element { get;}
		/// <summary>
		/// The <see cref="CopyMergeAction"/> that should be applied to this element.
		/// </summary>
		CopyMergeAction Action { get;}
		/// <summary>
		/// For a <see cref="CopyMergeAction.Match"/> action, the element is created
		/// only if the things it is a part of are created. Link role players are
		/// tracked separately.
		/// </summary>
		IEnumerable<IClosureElement> ReferencedPartOf { get;}
	}
	#endregion // IClosureElement interface
	#region IClosureElementLink interface
	/// <summary>
	/// Additional closure information for a link element
	/// </summary>
	public interface IClosureElementLink : IClosureElement
	{
		/// <summary>
		/// The source role player
		/// </summary>
		ModelElement SourceRolePlayer { get;}
		/// <summary>
		/// The target role player
		/// </summary>
		ModelElement TargetRolePlayer { get;}
	}
	#endregion // IClosureElementLink interface
	#region CopyClosureIntegrationResult struct
	/// <summary>
	/// The result of the <see cref="ICopyClosureManager.IntegrateCopyClosure"/>
	/// method. Includes information about copied elements and other status.
	/// </summary>
	public struct CopyClosureIntegrationResult
	{
		/// <summary>
		/// Dictionary keyed off the closure element ids with the copied elements as values.
		/// If an element is not copied due to store differences, then it will not be included
		/// in the returned dictionary.
		/// </summary>
		public IDictionary<Guid, ModelElement> CopiedElements;
		/// <summary>
		/// An array of extension model identifiers with elements that could not be copied.
		/// </summary>
		public Guid[] MissingExtensionModels;
	}
	#endregion // CopyClosureIntegrationResult struct
	#region MergeIntegrationOrder enum
	/// <summary>
	/// Determine the order that newly merged elements are
	/// integrated into an existing ordered collection.
	/// </summary>
	public enum MergeIntegrationOrder
	{
		/// <summary>
		/// Newly merged elements will placed as far down in the collection as possible
		/// relative to other previously merged elements. If there are unrecognized elements
		/// between the recognized elements, then place an earlier element after all leading
		/// elements (immediately before the recognized element) and after all trailing elements
		/// at the end of the collection. If there are no recognized elements, this places newly
		/// merged elements at the end of the collection.
		/// </summary>
		AfterLeading,
		/// <summary>
		/// Newly merged elements will placed as near as possible to other previously merged elements.
		/// If there are unrecognized elements between the recognized elements, then place an earlier
		/// element after all leading elements (immediately before the recognized element) and after
		/// all trailing immediately after the preceding existing match. If there are no recognized elements,
		/// this places newly merged elements at the end of the collection.
		/// </summary>
		AfterLeadingBeforeTrailing,
		/// <summary>
		/// Newly merged elements will placed as high in the collection as possible relative to other
		/// previously merged elements. If there are unrecognized elements between the recognized elements,
		/// then place an earlier element before all leading elements (as far away as possible from the next
		/// recognized element) and immediately after the last recognized element. If there are no
		/// recognized elements, then this places newly merged elements at the beginning of the collection.
		/// </summary>
		BeforeLeading,
	}
	#endregion // MergeIntegrationOrder enum
	#region ICopyClosureManager interface
	/// <summary>
	/// An interface used to register copy and merge closure results.
	/// This interface is provided by the system framework and works
	/// with the <see cref="ICopyClosureProvider"/> interface
	/// implemented by each model to provide closure settings.
	/// </summary>
	public interface ICopyClosureManager
	{
		/// <summary>
		/// Add a directive for populating a recursive copy closure by
		/// stepping over a relationship where the closure result is
		/// static for the specified role.
		/// </summary>
		/// <param name="fromRole">The role attached to a role player already
		/// included in the copy closure.</param>
		/// <param name="toRole">The opposite role in the relationship.</param>
		/// <param name="directiveOptions">Options limiting when this
		/// directive should apply.</param>
		/// <param name="closureBehavior">The static closure behavior.</param>
		void AddCopyClosureDirective(DomainRoleClosureRestriction fromRole, DomainRoleClosureRestriction toRole, CopyClosureDirectiveOptions directiveOptions, CopyClosureBehavior closureBehavior);
		/// <summary>
		/// Add a directive for populating a recursive copy closure by
		/// stepping over a relationship where the closure result depends
		/// on the state of the relationship.
		/// </summary>
		/// <param name="fromRole">The role attached to a role player already
		/// included in the copy closure.</param>
		/// <param name="toRole">The opposite role in the relationship.</param>
		/// <param name="directiveOptions">Options limiting when this
		/// directive should apply.</param>
		/// <param name="customClosureEvaluation">Optional custom evaluation to
		/// allow deep analysis based on instance data after the type requirements
		/// have been satisfied.</param>
		void AddCopyClosureDirective(DomainRoleClosureRestriction fromRole, DomainRoleClosureRestriction toRole, CopyClosureDirectiveOptions directiveOptions, EvaluateCopyClosure customClosureEvaluation);
		/// <summary>
		/// Add a directive to sort merged relationships of this type from the
		/// perspective of each role player that plays this role.
		/// </summary>
		/// <param name="roleId">The role identifier.</param>
		/// <param name="order">Merge ordering directive.</param>
		void AddOrderedRole(Guid roleId, MergeIntegrationOrder order);
		/// <summary>
		/// Specify a property that is ignored during copy and merge operations.
		/// </summary>
		/// <param name="propertyId">The main property id.</param>
		void AddIgnoredProperty(Guid propertyId);
		/// <summary>
		/// Specify a test for a property that is conditionally ignored during copy and merge operations.
		/// </summary>
		/// <param name="propertyId">The domain property id to test.</param>
		/// <param name="condition">The callback delegate.</param>
		void AddConditionalProperty(Guid propertyId, ConditionalPropertyTest condition);
		/// <summary>
		/// Add evaluators for registering implied element references
		/// </summary>
		/// <param name="classId">The class or relationship identifier.</param>
		/// <param name="includeClassDescendants"><see langword="true"/> to track derived classes.</param>
		/// <param name="impliedReferenceEvaluation">A callback for adding an implicitly referenced element.</param>
		void AddImpliedReference(Guid classId, bool includeClassDescendants, EvaluateImpliedReference impliedReferenceEvaluation);
		/// <summary>
		/// Add a relationship that should be automatically populated when a child element of the
		/// embedded type is created. If the parent element does not exist then an instance will
		/// be automatically created.
		/// </summary>
		/// <param name="relationshipId">The embedding relationship identifier.</param>
		void AddRootEmbeddingRelationship(Guid relationshipId);
		/// <summary>
		/// Determine a full copy closure based on starting elements.
		/// </summary>
		/// <param name="rootElements">Root elements for the copy closure.</param>
		/// <returns>Populated dictionary keyed by element id with closure information for the element.</returns>
		IDictionary<Guid, IClosureElement> GetCopyClosure(IEnumerable<ModelElement> rootElements);
		/// <summary>
		/// Integrate a copy closure returned by <see cref="GetCopyClosure"/> from one store
		/// into another.
		/// </summary>
		/// <param name="copyClosure">A dictionary of <see cref="IClosureElement"/> instances, keyed by element identifier.</param>
		/// <param name="sourceStore">The <see cref="Store"/> the elements are being copied from.</param>
		/// <param name="targetStore">The <see cref="Store"/> the elements are being copied to. This can be the same as <paramref name="targetStore"/></param>
		/// <param name="targetContextElements">One or more root elements to integrate into.</param>
		/// <param name="attemptElementMerge">If the stores are different, attempt to merge explicitly copied elements into the target store.
		/// Merge attempts are always made for indirectly copied elements.</param>
		/// <returns>A <see cref="CopyClosureIntegrationResult"/> with information about the resulting elements.</returns>
		CopyClosureIntegrationResult IntegrateCopyClosure(IDictionary<Guid, IClosureElement> copyClosure, Store sourceStore, Store targetStore, IEnumerable<ModelElement> targetContextElements, bool attemptElementMerge);
	}
	#endregion // ICopyClosureManager interface
	#region IElementEquivalence interface
	/// <summary>
	/// Support matching an equivalent element in another <see cref="Store"/>.
	/// </summary>
	public interface IElementEquivalence
	{
		/// <summary>
		/// Record elements in a foreign <see cref="Store"/> that
		/// are equivalent to elements in the Store containing the
		/// element that implements this interface.
		/// </summary>
		/// <param name="foreignStore">The foreign store to search.</param>
		/// <param name="elementTracker">A tracker used to add one or more elements.</param>
		/// <returns><see langword="true"/> if an equivalence relationship was added for this element.</returns>
		bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker);
	}
	/// <summary>
	/// Track equivalent elements
	/// </summary>
	public interface IEquivalentElementTracker
	{
		/// <summary>
		/// Add an element equivalence relationship.
		/// </summary>
		/// <param name="nativeElement">The native element, from the <see cref="Store"/> of
		/// the instance called the the <see cref="IElementEquivalence"/> interface.</param>
		/// <param name="equivalentElement">An equivalent element in the foreign store.</param>
		/// <remarks>An implementation of this interface should ignore any call with one or
		/// more values <see langword="null"/></remarks>
		void AddEquivalentElement(ModelElement nativeElement, ModelElement equivalentElement);
		/// <summary>
		/// Get an equivalent mapping previously added with <see cref="AddEquivalentElement"/>
		/// </summary>
		/// <param name="element">The element to find a match for.</param>
		/// <returns>The equivalent element, if any.</returns>
		ModelElement GetEquivalentElement(ModelElement element);
	}
	#endregion // IElementEquivalence interface
	#region CopyClosureIntegrationPhase enum
	/// <summary>
	/// The current stage of copy closure integration.
	/// Used with <see cref="CopyMergeUtility.GetIntegrationPhase"/>.
	/// </summary>
	public enum CopyClosureIntegrationPhase
	{
		/// <summary>
		/// A copy closure is not currently being integrated
		/// </summary>
		None,
		/// <summary>
		/// New elements from the copy closure are still being created
		/// </summary>
		Integrating,
		/// <summary>
		/// All changes represented in the copy closure are now complete.
		/// </summary>
		IntegrationComplete,
	}
	#endregion // CopyClosureIntegrationPhase enum
	#region CopyMergeUtility class
	/// <summary>
	/// Utility class to handle common copy and merge
	/// operations. Includes standard key values that
	/// are added to the <see cref="Transaction.Context"/>
	/// when a copy or merge operation is being integrated
	/// into a new model.
	/// </summary>
	public static class CopyMergeUtility
	{
		#region Integration Phase Keys and Helpers
		/// <summary>
		/// A key to indicate that a copy closure is currently being
		/// integrated into a <see cref="Store"/>.
		/// </summary>
		public static readonly object CopyClosureIntegratingKey = new object();
		/// <summary>
		/// A key to indicate that all elements represented in the copy closure
		/// have not been integrated into a <see cref="Store"/>.
		/// </summary>
		public static readonly object CopyClosureIntegrationCompleteKey = new object();
		/// <summary>
		/// Find the current <see cref="CopyClosureIntegrationPhase"/> for a <see cref="Store"/>
		/// with a current <see cref="Transaction"/> based on the presence of the
		/// <see cref="CopyClosureIntegratingKey"/> and <see cref="CopyClosureIntegrationCompleteKey"/>.
		/// </summary>
		public static CopyClosureIntegrationPhase GetIntegrationPhase(Store store)
		{
			Dictionary<object,object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			if (contextInfo.ContainsKey(CopyClosureIntegratingKey))
			{
				return contextInfo.ContainsKey(CopyClosureIntegrationCompleteKey) ? CopyClosureIntegrationPhase.IntegrationComplete : CopyClosureIntegrationPhase.Integrating;
			}
			return CopyClosureIntegrationPhase.None;
		}
		#endregion // Integration Phase Keys and Helpers
		#region IEquivalentElementTracker Helpers
		/// <summary>
		/// Get an existing mapping for an element or initiate
		/// a matching operation if <paramref name="element"/>
		/// implements <see cref="IElementEquivalence"/>
		/// </summary>
		/// <typeparam name="T">The type of <see cref="ModelElement"/> to locate.</typeparam>
		/// <param name="element">The element in the source store.</param>
		/// <param name="foreignStore">The foreign store to search.</param>
		/// <param name="elementTracker">A tracker used to add one or more elements.</param>
		/// <returns>An equivalent element, or <see langword="null"/> if the element could not be found.</returns>
		public static T GetEquivalentElement<T>(T element, Store foreignStore, IEquivalentElementTracker elementTracker) where T : ModelElement
		{
			T otherElement = elementTracker.GetEquivalentElement(element) as T;
			IElementEquivalence testEquivalence;
			if (otherElement == null &&
				null != (testEquivalence = element as IElementEquivalence) &&
				testEquivalence.MapEquivalentElements(foreignStore, elementTracker))
			{
				otherElement = elementTracker.GetEquivalentElement(element) as T;
			}
			return otherElement;
		}
		#endregion // IEquivalentElementTracker Helpers
		#region IEquivalentElementTracker Stock Implementation
		#region StockEquivalentElementTracker class
		/// <summary>
		/// A helper class for establishing element equivalence during closure integration.
		/// </summary>
		private sealed class StockEquivalentElementTracker : IEquivalentElementTracker
		{
			#region Member Variables and Constructor
			private readonly Dictionary<ModelElement, ModelElement> myElementDictionary;
			public StockEquivalentElementTracker()
			{
				myElementDictionary = new Dictionary<ModelElement, ModelElement>();
			}
			#endregion // Member Variables and Constructor
			#region IEquivalentElementTracker Implementation
			ModelElement IEquivalentElementTracker.GetEquivalentElement(ModelElement element)
			{
				ModelElement retVal;
				return myElementDictionary.TryGetValue(element, out retVal) ? retVal : null;
			}
			void IEquivalentElementTracker.AddEquivalentElement(ModelElement nativeElement, ModelElement equivalentElement)
			{
				if (nativeElement != null && equivalentElement != null)
				{
					myElementDictionary[nativeElement] = equivalentElement;
				}
			}
			#endregion // IEquivalentElementTracker Implementation
		}
		#endregion // StockEquivalentElementTracker class
		/// <summary>
		/// Create a standard implementation of the <see cref="IEquivalentElementTracker"/>
		/// interface. Designed as an equivalence tracking class to enable use of
		/// <see cref="IElementEquivalence"/> implementations before copy closure
		/// integration begins.
		/// </summary>
		public static IEquivalentElementTracker CreateEquivalentElementTracker()
		{
			return new StockEquivalentElementTracker();
		}
		#endregion // IEquivalentElementTracker Stock Implementation
		#region Missing Extension Helpers
		/// <summary>
		/// A key used in the <see cref="Store.PropertyBag"/> to track
		/// source extensions that been explicitly ignored by the user
		/// for this session.
		/// </summary>
		public static readonly object IgnoredSourceExtensionsKey = new object();
		/// <summary>
		/// Use the <see cref="IgnoredSourceExtensionsKey"/> key to find missing
		/// extension elements that are not ignored for this session.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> to test.</param>
		/// <param name="missingExtensions">Missing extensions, generally returned
		/// by <see cref="ICopyClosureManager.IntegrateCopyClosure"/></param>
		/// <returns>An array of <see cref="Type"/> elements for each missing extension,
		/// or <see langword="null"/> if no non-ignored extensions are found.</returns>
		public static Guid[] GetNonIgnoredMissingExtensions(Store store, Guid[] missingExtensions)
		{
			object ignoredExtensionsObject;
			List<Guid> ignoredExtensions;
			if (missingExtensions == null || missingExtensions.Length == 0)
			{
				return null;
			}
			else if (store.PropertyBag.TryGetValue(IgnoredSourceExtensionsKey, out ignoredExtensionsObject) &&
				null != (ignoredExtensions = ignoredExtensionsObject as List<Guid>))
			{
				int testExtensionCount = missingExtensions.Length;
				BitTracker tracker = new BitTracker(testExtensionCount);
				int ignoredCount = 0;
				for (int i = 0; i < testExtensionCount; ++i)
				{
					if (ignoredExtensions.Contains(missingExtensions[i]))
					{
						++ignoredCount;
						tracker[i] = true;
					}
				}
				if (ignoredCount != 0)
				{
					if (ignoredCount == testExtensionCount)
					{
						missingExtensions = null;
					}
					else
					{
						Guid[] reducedMissingExtensions = new Guid[testExtensionCount - ignoredCount];
						int nextIndex = 0;
						for (int i = 0; i < testExtensionCount; ++i)
						{
							if (!tracker[i])
							{
								reducedMissingExtensions[nextIndex] = missingExtensions[i];
								++nextIndex;
							}
						}
						missingExtensions = reducedMissingExtensions;
					}
				}
			}
			return missingExtensions;
		}
		/// <summary>
		/// Add ignored missing extensions for this document session.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> to modify</param>
		/// <param name="ignoredMissingExtensions">The extensions to ignore. This
		/// list should have been pre-verified as original ignored extensions with
		/// the <see cref="GetNonIgnoredMissingExtensions"/> method.</param>
		public static void CacheIgnoredMissingExtensions(Store store, Guid[] ignoredMissingExtensions)
		{
			if (ignoredMissingExtensions == null || ignoredMissingExtensions.Length == 0)
			{
				return;
			}
			Dictionary<object, object> bag = store.PropertyBag;
			object ignoredExtensionsObject;
			List<Guid> ignoredExtensions;
			if (bag.TryGetValue(IgnoredSourceExtensionsKey, out ignoredExtensionsObject) &&
				null != (ignoredExtensions = ignoredExtensionsObject as List<Guid>))
			{
				ignoredExtensions.AddRange(ignoredMissingExtensions);
			}
			else
			{
				bag[IgnoredSourceExtensionsKey] = new List<Guid>(ignoredMissingExtensions);
			}
		}
		#endregion // Missing Extension Helpers
		#region Debug Helpers
#if DEBUGHELPERS
		/// <summary>
		/// Dump copy closure details to the debug window
		/// </summary>
		public static void DumpCopyClosure(IDictionary<Guid, IClosureElement> closure)
		{
			int index = 0;
			foreach (KeyValuePair<Guid, IClosureElement> pair in closure)
			{
				IClosureElement closureElement = pair.Value;
				ModelElement element = closureElement.Element;
				Type type = element.GetType();
				string typeName = type.Name;
				string elementString = element.ToString();
				if (elementString == type.FullName)
				{
					elementString = typeName;
				}
				Debug.Write(string.Format("{0},{1},{2},{3},{4}", ++index, pair.Key.ToString("D"), closureElement.Action.ToString(), typeName, elementString));
				ElementLink link = element as ElementLink;
				if (link != null)
				{
					foreach (ModelElement linkEndpoint in link.LinkedElements)
					{
						Debug.Write(',');
						Debug.Write(linkEndpoint.Id.ToString("D"));
					}
				}
				Debug.WriteLine("");
			}
		}
#endif // DEBUGHELPERS
		#endregion // Debug Helpers
	}
	#endregion // CopyMergeUtility class
	#region CopyClosureManager class
	/// <summary>
	/// A reusable implementation of the <see cref="ICopyClosureManager"/> interface
	/// </summary>
	public sealed class CopyClosureManager : ICopyClosureManager
	{
		#region BoundEntryRole struct
		/// <summary>
		/// A structure to use as a lookup key to find closure evaluation
		/// </summary>
		private struct BoundEntryRole : IEquatable<BoundEntryRole>
		{
			#region Public Fields
			/// <summary>
			/// The role identifier
			/// </summary>
			public readonly Guid EntryRoleId;
			/// <summary>
			/// A fixed type for the role player of this type, or null.
			/// </summary>
			public readonly Type EntryRolePlayerType;
			/// <summary>
			/// A fixed type for the role player opposite the entry role, or null.
			/// </summary>
			public readonly Type ExitRolePlayerType;
			#endregion // Public Fields
			#region Constructors
			/// <summary>
			/// Create a <see cref="BoundEntryRole"/> for a role with
			/// no type restrictions on role players.
			/// </summary>
			/// <param name="entryRoleId">The domain role id.</param>
			public BoundEntryRole(Guid entryRoleId)
			{
				EntryRoleId = entryRoleId;
				EntryRolePlayerType = null;
				ExitRolePlayerType = null;
			}
			/// <summary>
			/// Create a <see cref="BoundEntryRole"/> for a role with
			/// restrictions on the role player.
			/// </summary>
			/// <param name="entryRoleId">The domain role id.</param>
			/// <param name="entryRolePlayerType">An explicit role player type.</param>
			/// <param name="exitRolePlayerType">An explicit role player type.</param>
			public BoundEntryRole(Guid entryRoleId, Type entryRolePlayerType, Type exitRolePlayerType)
			{
				EntryRoleId = entryRoleId;
				EntryRolePlayerType = entryRolePlayerType;
				ExitRolePlayerType = exitRolePlayerType;
			}
			#endregion // Constructors
			#region Equality overrides
			/// <summary>
			/// Equals operator override
			/// </summary>
			public static bool operator ==(BoundEntryRole boundRole1, BoundEntryRole boundRole2)
			{
				return boundRole1.Equals(boundRole2);
			}
			/// <summary>
			/// Not equals operator override
			/// </summary>
			public static bool operator !=(BoundEntryRole boundRole1, BoundEntryRole boundRole2)
			{
				return !(boundRole1.Equals(boundRole2));
			}
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				return obj is BoundEntryRole && Equals((BoundEntryRole)obj);
			}
			/// <summary>
			/// Typed Equals method, make equality symmetric
			/// </summary>
			public bool Equals(BoundEntryRole obj)
			{
				return EntryRoleId == obj.EntryRoleId &&
					EntryRolePlayerType == obj.EntryRolePlayerType &&
					ExitRolePlayerType == obj.ExitRolePlayerType;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override int GetHashCode()
			{
				Type explicitEntryType = EntryRolePlayerType;
				Type explicitExitType = ExitRolePlayerType;
				return explicitEntryType != null ?
					(explicitExitType != null ?
						Utility.GetCombinedHashCode(explicitEntryType.GetHashCode(), EntryRoleId.GetHashCode(), explicitExitType.GetHashCode()) :
						Utility.GetCombinedHashCode(explicitEntryType.GetHashCode(), EntryRoleId.GetHashCode())) :
					(explicitExitType != null ?
						Utility.GetCombinedHashCode(0, EntryRoleId.GetHashCode(), explicitExitType.GetHashCode()) :
						Utility.GetCombinedHashCode(0, EntryRoleId.GetHashCode()));
			}
			#endregion // Equality overrides
		}
		#endregion // BoundEntryRole struct
		#region RolePlayerTypeDistance structure
		/// <summary>
		/// Structure for tracking role player inheritance depth
		/// during closure evaluation. Inheritance depth is considered
		/// if more than one potential closure option is offered
		/// for a relationship.
		/// </summary>
		private struct RolePlayerTypeDistance : IComparable<RolePlayerTypeDistance>
		{
			private readonly short myEntryDepth;
			private readonly short myExitDepth;
			/// <summary>
			/// Create a new <see cref="RolePlayerTypeDistance"/> tracking object.
			/// </summary>
			/// <param name="entryDepth">The inheritance distance from the entry role player
			/// to the explicit type restriction that resulted
			/// in this restriction type being added to the tracking
			/// dictionary, or -1 if no restriction is specified.</param>
			/// <param name="exitDepth">The inheritance distance from the exit role player
			/// to the explicit type restriction that resulted
			/// in this restriction type being added to the tracking
			/// dictionary, or -1 if no restriction is specified.</param>
			public RolePlayerTypeDistance(int entryDepth, int exitDepth)
			{
				myEntryDepth = (short)entryDepth;
				myExitDepth = (short)exitDepth;
			}
			/// <summary>
			/// The inheritance distance from the entry role player
			/// to the explicit type restriction that resulted
			/// in this restriction type being added to the tracking
			/// dictionary, or -1 if no restriction is specified.
			/// </summary>
			public int EntryDepth
			{
				get
				{
					return myEntryDepth;
				}
			}
			/// <summary>
			/// The inheritance distance from the exit role player
			/// to the explicit type restriction that resulted
			/// in this restriction type being added to the tracking
			/// dictionary, or -1 if no restriction is specified.
			/// </summary>
			public int ExitDepth
			{
				get
				{
					return myExitDepth;
				}
			}
			#region IComparable<RolePlayerTypeDistance> Implementation
			/// <summary>
			/// Compare depth. The more specific depth is less than the less specific, and
			/// entry depth takes precedences over exit depth.
			/// </summary>
			public int CompareTo(RolePlayerTypeDistance other)
			{
				// Give the entry role type priority, consistent with comparison in CopyClosureManager.ProcessAttachedLinks
				int retVal = myEntryDepth.CompareTo(other.myEntryDepth);
				return retVal == 0 ? myExitDepth.CompareTo(other.myExitDepth) : retVal;
			}
			#endregion // IComparable<RolePlayerTypeDistance> Implementation
		}
		#endregion // RolePlayerTypeDistance structure
		#region AvailableClosureBehaviors enum
		/// <summary>
		/// Enum to track different closure types for
		/// a specific entry domain role. Used to limit
		/// lookups in the behaviors dictionaries.
		/// </summary>
		[Flags]
		private enum AvailableClosureBehaviors
		{
			/// <summary>
			/// Role does not participate in the closure
			/// </summary>
			None = 0,
			/// <summary>
			/// Normal role entry with no typing restrictions
			/// </summary>
			Untyped = 1,
			/// <summary>
			/// Normal role entry with a restricted entry role
			/// and an unrestricted exit role.
			/// </summary>
			EntryRoleRestricted = 2,
			/// <summary>
			/// Normal role entry with an unrestricted entry role
			/// and a restricted exit role.
			/// </summary>
			ExitRoleRestricted = 4,
			/// <summary>
			/// Normal role entry with both the entry and exit
			/// role players restricted.
			/// </summary>
			EntryAndExitRoleRestricted = 8,
			/// <summary>
			/// Normal role entry with no typing restrictions
			/// </summary>
			PrimaryUntyped = 0x10,
			/// <summary>
			/// Normal role entry with a restricted entry role
			/// and an unrestricted exit role.
			/// </summary>
			PrimaryEntryRoleRestricted = 0x20,
			/// <summary>
			/// Normal role entry with an unrestricted entry role
			/// and a restricted exit role.
			/// </summary>
			PrimaryExitRoleRestricted = 0x40,
			/// <summary>
			/// Normal role entry with both the entry and exit
			/// role players restricted.
			/// </summary>
			PrimaryEntryAndExitRoleRestricted = 0x80,
		}
		#endregion // AvailableClosureBehaviors enum
		#region CopiedPropertiesCache struct
		/// <summary>
		/// Cache for determining which properties should be copied.
		/// </summary>
		private struct CopiedPropertiesCache
		{
			public static readonly CopiedPropertiesCache Empty = new CopiedPropertiesCache(new Guid[0], null);
			/// <summary>
			/// The property identifiers
			/// </summary>
			public readonly Guid[] Identifiers;
			/// <summary>
			/// If any of the properties are conditional, then
			/// get delegates determining copy conditions for each
			/// property. Can be null.
			/// </summary>
			public readonly ConditionalPropertyTest[][] Conditions;
			public CopiedPropertiesCache(Guid[] identifiers, ConditionalPropertyTest[][] conditions)
			{
				Identifiers = identifiers;
				Conditions = conditions;
			}
		}
		#endregion // CopiedPropertiesCache struct
		#region Member Variables
		private Store myStore;
		private Dictionary<Guid, AvailableClosureBehaviors> myAvailableBehaviors;
		private Dictionary<BoundEntryRole, EvaluateCopyClosure> myNormalBehaviors;
		private Dictionary<BoundEntryRole, EvaluateCopyClosure> myPrimaryBehaviors;
		private Dictionary<Guid, EvaluateImpliedReference> myImpliedReferenceEvaluators;
		private Dictionary<Type, LinkedNode<Guid>> myEmbeddedRoles;
		private Dictionary<Guid, MergeIntegrationOrder> myOrderedRoles; // Contains role ids and ordering directives
		private Dictionary<Guid, object> myIgnoredProperties; // Contains property ids and null values
		private Dictionary<Guid, ConditionalPropertyTest> myPropertyConditions;
		private Dictionary<Guid, CopiedPropertiesCache> myCopiedProperties; // Key is a class identifier, value is an array of properties defined on that class
		// Temporary trackers used to handle multiple behavior specifications
		private Dictionary<BoundEntryRole, RolePlayerTypeDistance> myNormalDistanceTracker;
		private Dictionary<BoundEntryRole, RolePlayerTypeDistance> myPrimaryDistanceTracker;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a <see cref="CopyClosureManager"/> for a given <see cref="Store"/>
		/// </summary>
		public CopyClosureManager(Store store)
		{
			myStore = store;
		}
		#endregion // Constructor
		#region ICopyClosureManager Implementation
		void ICopyClosureManager.AddCopyClosureDirective(DomainRoleClosureRestriction fromRole, DomainRoleClosureRestriction toRole, CopyClosureDirectiveOptions directiveOptions, CopyClosureBehavior closureBehavior)
		{
			// Note that these delegates compile to cached delegates bound to static methods
			// with the return values hard coded instead of based directly on the closureBehavior.
			// This is more efficient than linking to static functions in this class.
			EvaluateCopyClosure evaluator;
			switch (closureBehavior)
			{
				case CopyClosureBehavior.Ignored:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.Ignored; };
					break;
				case CopyClosureBehavior.Container:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.Container; };
					break;
				case CopyClosureBehavior.ContainedPart:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.ContainedPart; };
					break;
				case CopyClosureBehavior.InternalReferencedPart:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.InternalReferencedPart; };
					break;
				case CopyClosureBehavior.ExternalCompositePart:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.ExternalCompositePart; };
					break;
				case CopyClosureBehavior.ExternalReferencedPart:
					evaluator = delegate(ElementLink link) { return CopyClosureBehavior.ExternalReferencedPart; };
					break;
				default:
					return;
			}
			AddCopyClosureDirective(fromRole, toRole, directiveOptions, evaluator);
		}
		void ICopyClosureManager.AddCopyClosureDirective(DomainRoleClosureRestriction fromRole, DomainRoleClosureRestriction toRole, CopyClosureDirectiveOptions directiveOptions, EvaluateCopyClosure customClosureEvaluation)
		{
			AddCopyClosureDirective(fromRole, toRole, directiveOptions, customClosureEvaluation);
		}
		void ICopyClosureManager.AddOrderedRole(Guid roleId, MergeIntegrationOrder order)
		{
			Dictionary<Guid, MergeIntegrationOrder> orderedRoles = myOrderedRoles;
			if (orderedRoles == null)
			{
				myOrderedRoles = orderedRoles = new Dictionary<Guid, MergeIntegrationOrder>();
			}
			orderedRoles[roleId] = order;
		}
		void ICopyClosureManager.AddIgnoredProperty(Guid propertyId)
		{
			Dictionary<Guid, object> ignoredProperties = myIgnoredProperties;
			if (ignoredProperties == null)
			{
				myIgnoredProperties = ignoredProperties = new Dictionary<Guid, object>();
			}
			ignoredProperties[propertyId] = null;
		}
		void ICopyClosureManager.AddConditionalProperty(Guid propertyId, ConditionalPropertyTest condition)
		{
			Dictionary<Guid, ConditionalPropertyTest> propertyConditions = myPropertyConditions;
			ConditionalPropertyTest existingTest;
			if (propertyConditions == null)
			{
				myPropertyConditions = propertyConditions = new Dictionary<Guid, ConditionalPropertyTest>();
			}
			else if (propertyConditions.TryGetValue(propertyId, out existingTest))
			{
				existingTest += condition;
				condition = existingTest;
			}
			propertyConditions[propertyId] = condition;
		}
		void ICopyClosureManager.AddImpliedReference(Guid classId, bool includeClassDescendants, EvaluateImpliedReference impliedReferenceEvaluation)
		{
			Dictionary<Guid, EvaluateImpliedReference> evaluators = myImpliedReferenceEvaluators;
			if (evaluators == null)
			{
				myImpliedReferenceEvaluators = evaluators = new Dictionary<Guid, EvaluateImpliedReference>();
			}
			EvaluateImpliedReference existingDelegate;
			if (evaluators.TryGetValue(classId, out existingDelegate))
			{
				existingDelegate += impliedReferenceEvaluation;
				evaluators[classId] = existingDelegate;
			}
			else
			{
				evaluators.Add(classId, impliedReferenceEvaluation);
			}
			if (includeClassDescendants)
			{
				foreach (DomainClassInfo descendant in myStore.DomainDataDirectory.GetDomainClass(classId).AllDescendants)
				{
					Guid descendantId = descendant.Id;
					if (evaluators.TryGetValue(descendantId, out existingDelegate))
					{
						existingDelegate += impliedReferenceEvaluation;
						evaluators[classId] = existingDelegate;
					}
					else
					{
						evaluators.Add(descendantId, impliedReferenceEvaluation);
					}
				}
			}
		}
		void ICopyClosureManager.AddRootEmbeddingRelationship(Guid relationshipId)
		{
			// Map the type of the embedded element to the type of the entry role
			DomainRelationshipInfo relationship = myStore.DomainDataDirectory.GetDomainRelationship(relationshipId);
			if (relationship.IsEmbedding && !relationship.ImplementationClass.IsAbstract)
			{
				ReadOnlyCollection<DomainRoleInfo> roles = relationship.DomainRoles;
				int roleCount = roles.Count;
				for (int i = 0; i < roles.Count; ++i)
				{
					DomainRoleInfo embeddedRoleInfo = roles[i];
					if (!embeddedRoleInfo.IsEmbedding)
					{
						Dictionary<Type, LinkedNode<Guid>> embeddedRoleLinks = myEmbeddedRoles;
						if (embeddedRoleLinks == null)
						{
							myEmbeddedRoles = embeddedRoleLinks = new Dictionary<Type, LinkedNode<Guid>>();
						}
						Guid roleId = embeddedRoleInfo.Id;
						DomainClassInfo rolePlayerClassInfo = embeddedRoleInfo.RolePlayer;
						Type rolePlayerType = rolePlayerClassInfo.ImplementationClass;
						LinkedNode<Guid> headNode;
						if (!rolePlayerType.IsAbstract)
						{
							if (embeddedRoleLinks.TryGetValue(rolePlayerType, out headNode))
							{
								headNode.GetTail().SetNext(new LinkedNode<Guid>(roleId), ref headNode);
							}
							else
							{
								embeddedRoleLinks[rolePlayerType] = new LinkedNode<Guid>(roleId);
							}
						}
						foreach (DomainClassInfo derivedRolePlayers in rolePlayerClassInfo.AllDescendants)
						{
							rolePlayerType = derivedRolePlayers.ImplementationClass;
							if (!rolePlayerType.IsAbstract)
							{
								if (embeddedRoleLinks.TryGetValue(rolePlayerType, out headNode))
								{
									headNode.GetTail().SetNext(new LinkedNode<Guid>(roleId), ref headNode);
								}
								else
								{
									embeddedRoleLinks[rolePlayerType] = new LinkedNode<Guid>(roleId);
								}
							}
						}
						break;
					}
				}
			}
		}
		IDictionary<Guid, IClosureElement> ICopyClosureManager.GetCopyClosure(IEnumerable<ModelElement> rootElements)
		{
			return GenerateCopyClosure(rootElements);
		}
		CopyClosureIntegrationResult ICopyClosureManager.IntegrateCopyClosure(IDictionary<Guid, IClosureElement> copyClosure, Store sourceStore, Store targetStore, IEnumerable<ModelElement> targetContextElements, bool attemptElementMerge)
		{
			return IntegrateCopyClosure(copyClosure, sourceStore, targetStore, targetContextElements, attemptElementMerge);
		}
		#endregion // ICopyClosureManager Implementation
		#region Closure Duplication Methods
		#region CopiedElementType enum
		/// <summary>
		/// Track information about a copied element
		/// </summary>
		private enum CopiedElementType
		{
			/// <summary>
			/// The copied element is associated with an existing element in the model
			/// </summary>
			Existing,
			/// <summary>
			/// The element is a newly created element
			/// </summary>
			New,
		}
		#endregion // CopiedElementType enum
		#region CopiedElement struct
		/// <summary>
		/// A structure representing the copy of an element
		/// </summary>
		private struct CopiedElement
		{
			/// <summary>
			/// The copy operation element in the new model
			/// </summary>
			public readonly ModelElement Element;
			/// <summary>
			/// Whether the element was added to the copied
			/// structure as a new or existing element.
			/// </summary>
			public readonly CopiedElementType CopyType;
			/// <summary>
			/// Create a new <see cref="CopiedElement"/> structure
			/// </summary>
			/// <param name="element">The target element.</param>
			/// <param name="copyType">How the element was included.</param>
			public CopiedElement(ModelElement element, CopiedElementType copyType)
			{
				Element = element;
				CopyType = copyType;
			}
		}
		#endregion // CopiedElement struct
		#region EquivalenceTracker class
		/// <summary>
		/// A helper class for establishing element equivalence during closure integration.
		/// </summary>
		private sealed class EquivalenceTracker : IEquivalentElementTracker
		{
			#region Member Variables and Constructor
			private readonly Dictionary<Guid, CopiedElement> myEquivalentElementMap;
			private readonly Dictionary<Guid, MergeIntegrationOrder> myOrderedRoles;
			private readonly DomainDataDirectory myTargetDataDirectory;
			private Dictionary<RoleAndElement, ModelElement> myRequiresOrdering;
			public EquivalenceTracker(Dictionary<Guid, CopiedElement> equivalenceMap, Dictionary<Guid, MergeIntegrationOrder> orderedRoles, DomainDataDirectory targetDataDirectory, Dictionary<RoleAndElement, ModelElement> requiresOrdering)
			{
				myEquivalentElementMap = equivalenceMap;
				myOrderedRoles = orderedRoles;
				myTargetDataDirectory = targetDataDirectory;
				myRequiresOrdering = requiresOrdering;
			}
			#endregion // Member Variables and Constructor
			#region EquivalenceTracker specific
			public Dictionary<RoleAndElement, ModelElement> RequiresOrdering
			{
				get
				{
					return myRequiresOrdering;
				}
			}
			#endregion // EquivalenceTracker specific
			#region IEquivalentElementTracker Implementation
			ModelElement IEquivalentElementTracker.GetEquivalentElement(ModelElement element)
			{
				CopiedElement copiedElement;
				return myEquivalentElementMap.TryGetValue(element.Id, out copiedElement) ? copiedElement.Element : null;
			}
			void IEquivalentElementTracker.AddEquivalentElement(ModelElement nativeElement, ModelElement equivalentElement)
			{
				if (nativeElement != null && equivalentElement != null)
				{
					myEquivalentElementMap[nativeElement.Id] = new CopiedElement(equivalentElement, CopiedElementType.Existing);
					// If this is a link, then an existing match does not mean
					// that we do not need to verify ordering. Equivalence matches are unordered.
					ElementLink nativeLink;
					ElementLink equivalentLink;
					Dictionary<Guid, MergeIntegrationOrder> orderedRoles;
					if (null != (orderedRoles = myOrderedRoles) &&
						null != (nativeLink = nativeElement as ElementLink) &&
						null != (equivalentLink = equivalentElement as ElementLink))
					{
						DomainDataDirectory targetDataDirectory = myTargetDataDirectory;
						foreach (DomainRoleInfo nativeRoleInfo in nativeLink.GetDomainRelationship().DomainRoles)
						{
							Guid roleId = nativeRoleInfo.Id;
							DomainRoleInfo targetRoleInfo;
							if (orderedRoles.ContainsKey(roleId) &&
								null != (targetRoleInfo = targetDataDirectory.FindDomainRole(roleId)))
							{
								TrackOrdering(orderedRoles, roleId, nativeRoleInfo.GetRolePlayer(nativeLink), targetRoleInfo.GetRolePlayer(equivalentLink), ref myRequiresOrdering);
							}
						}
					}
				}
			}
			#endregion // IEquivalentElementTracker Implementation
		}
		#endregion // EquivalenceTracker class
		#region RoleAndElement struct
		/// <summary>
		/// Struct for dictionary lookup of a role/element pair
		/// </summary>
		private struct RoleAndElement : IEquatable<RoleAndElement>
		{
			/// <summary>
			/// The role to get collections through
			/// </summary>
			public readonly Guid DomainRoleId;
			/// <summary>
			/// An element that plays this role
			/// </summary>
			public readonly ModelElement Element;
			/// <summary>
			/// Initializes a new instance of <see cref="RoleAndElement"/>.
			/// </summary>
			public RoleAndElement(Guid domainRoleId, ModelElement element)
			{
				DomainRoleId = domainRoleId;
				Element = element;
			}
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				return Element.GetHashCode() ^ DomainRoleId.GetHashCode();
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is RoleAndElement && Equals((RoleAndElement)obj);
			}
			/// <summary>See <see cref="IEquatable{RoleAndElement}.Equals"/>.</summary>
			public bool Equals(RoleAndElement other)
			{
				return Element.Equals(other.Element) && DomainRoleId.Equals(other.DomainRoleId);
			}
		}
		#endregion // RoleAndElement struct
		/// <summary>
		/// Implementation of <see cref="ICopyClosureManager.IntegrateCopyClosure"/>
		/// </summary>
		private CopyClosureIntegrationResult IntegrateCopyClosure(IDictionary<Guid, IClosureElement> copyClosure, Store sourceStore, Store targetStore, IEnumerable<ModelElement> targetContextElements, bool attemptElementMerge)
		{
			// A dictionary of the new elements keyed off the closure element ids
			Dictionary<Guid, CopiedElement> resolvedElements = new Dictionary<Guid,CopiedElement>();
			Dictionary<Guid, object> missingExtensions = null;
			// The base-on-element and role are in the key, the role player to synchronize is in the value
			Dictionary<RoleAndElement, ModelElement> requiresOrdering = null;
			if (sourceStore != targetStore)
			{
				Transaction t = targetStore.TransactionManager.BeginTransaction("IntegrateCopyClosure"); // This should be a nested transaction, so the user does not see this string
				Dictionary<object, object> contextInfo = t.TopLevelTransaction.Context.ContextInfo;
				bool removeContextIntegratingKey = true;
				bool removeContextIntegrationCompleteKey = false;
				bool restoreContextIntegrationCompleteKey = false;
				if (contextInfo.ContainsKey(CopyMergeUtility.CopyClosureIntegratingKey))
				{
					removeContextIntegratingKey = false;
					if (contextInfo.ContainsKey(CopyMergeUtility.CopyClosureIntegrationCompleteKey))
					{
						contextInfo.Remove(CopyMergeUtility.CopyClosureIntegrationCompleteKey);
						restoreContextIntegrationCompleteKey = true;
					}
				}
				else
				{
					contextInfo[CopyMergeUtility.CopyClosureIntegratingKey] = null;
				}
				ICopyClosureIntegrationListener[] listeners = Utility.GetTypedDomainModels<ICopyClosureIntegrationListener>(targetStore.DomainModels);
				try
				{
					// Note that we assume ForceAllRulesToCommitTime is false at this point. Copy
					// closure integration is done before the DSL-powered shape element merging
					// that happens after this transaction has completed. If this cannot be
					// satisfied for a future scenario, then ForceAllRulesToCommitTime should
					// be set to false for this transaction.
					Debug.Assert(!t.ForceAllRulesToCommitTime);
					if (listeners != null)
					{
						foreach (ICopyClosureIntegrationListener listener in listeners)
						{
							listener.BeginCopyClosureIntegration(t);
						}
					}

					// Resolve the closure in multiple phases:
					// 1) Check all elements with a Match directive for equivalence
					// 2) Create all unmatched non-link elements and resolve links
					//    between two existing elements.
					// 3) Fill in all element links that need to be created.
					// 4) Reorder registered collections
					// 5) Update properties for all existing elements
					ElementFactory targetElementFactory = targetStore.ElementFactory;
					DomainDataDirectory targetDataDirectory = targetStore.DomainDataDirectory;
					DomainDataDirectory sourceDataDirectory = sourceStore.DomainDataDirectory;
					EquivalenceTracker tracker = new EquivalenceTracker(resolvedElements, myOrderedRoles, targetDataDirectory, requiresOrdering);
					foreach (KeyValuePair<Guid, IClosureElement> pair in copyClosure)
					{
						IClosureElement closureElement = pair.Value;
						switch (closureElement.Action)
						{
							case CopyMergeAction.Duplicate:
								if (attemptElementMerge)
								{
									goto case CopyMergeAction.Match;
								}
								break; // Do not attempt to merge duplicate elements
							case CopyMergeAction.Match:
								IElementEquivalence equivalence;
								if (null != (equivalence = closureElement.Element as IElementEquivalence) &&
									!resolvedElements.ContainsKey(pair.Key))
								{
									equivalence.MapEquivalentElements(targetStore, tracker);
								}
								break;
							case CopyMergeAction.Link:
								// UNDONE: COPYMERGE Support IElementEquivalence implementation
								// on link elements.
								// First, resolve whether this is duplicate (any role players
								// are duplicates) or not. Note that this can be recursive
								// so we should consider caching this information.
								continue;
						}
					}
					requiresOrdering = tracker.RequiresOrdering; // Will be the same or newly created during tracking

					// Second phase: Create unmatched elements and resolve links to known elements
					foreach (KeyValuePair<Guid, IClosureElement> pair in copyClosure)
					{
						IClosureElement closureElement = pair.Value;
						Guid sourceId = pair.Key;
						if (!resolvedElements.ContainsKey(sourceId))
						{
							switch (closureElement.Action)
							{
								case CopyMergeAction.Duplicate:
								case CopyMergeAction.Match:
									ModelElement basedOnElement = closureElement.Element;
									DomainClassInfo sourceClassInfo = basedOnElement.GetDomainClass();
									DomainClassInfo targetClassInfo = targetDataDirectory.FindDomainClass(sourceClassInfo.Id);
									if (targetClassInfo == null)
									{
										(missingExtensions ?? (missingExtensions = new Dictionary<Guid, object>()))[sourceClassInfo.DomainModel.Id] = null;
									}
									else
									{
										PropertyAssignment[] propertyAssignments = GetCopiedPropertyAssignments(basedOnElement, null);
										// Note that we use the domain class id, not the domain class itself because the instance
										// is different in different stores.
										ModelElement newElement = propertyAssignments != null ?
											targetElementFactory.CreateElement(targetClassInfo, propertyAssignments) :
											targetElementFactory.CreateElement(targetClassInfo.Id);
										if (InitializeNewElement(newElement, basedOnElement))
										{
											resolvedElements[sourceId] = new CopiedElement(newElement, CopiedElementType.New);
											IntegrateRootElement(basedOnElement, newElement, targetContextElements, targetDataDirectory, targetElementFactory, sourceDataDirectory, ref missingExtensions, ref requiresOrdering);
										}
									}
									break;
								case CopyMergeAction.Link:
									ResolveExistingLink((IClosureElementLink)closureElement, copyClosure, resolvedElements, targetDataDirectory, ref missingExtensions);
									break;
							}
						}
					}

					// Third phase: create element links
					foreach (KeyValuePair<Guid, IClosureElement> pair in copyClosure)
					{
						IClosureElement closureElement = pair.Value;
						switch (closureElement.Action)
						{
							case CopyMergeAction.Link:
								if (!resolvedElements.ContainsKey(pair.Key))
								{
									CreateLinkCopy((IClosureElementLink)pair.Value, targetDataDirectory, targetElementFactory, copyClosure, resolvedElements, ref missingExtensions, ref requiresOrdering);
								}
								break;
						}
					}

					// Fourth phase: update ordering for modified collections
					if (requiresOrdering != null)
					{
						Dictionary<Guid, MergeIntegrationOrder> orderedRoles = myOrderedRoles;
						foreach (KeyValuePair<RoleAndElement, ModelElement> pair in requiresOrdering)
						{
							// The source element is in the key, the merged element in the value
							RoleAndElement key = pair.Key;
							Guid roleId = key.DomainRoleId;
							DomainRoleInfo sourceRoleInfo;
							DomainRoleInfo targetRoleInfo;
							LinkedElementCollection<ModelElement> targetLinkedElements;
							LinkedElementCollection<ModelElement> sourceLinkedElements;
							if (null != (sourceRoleInfo = sourceDataDirectory.FindDomainRole(roleId)) &&
								null != (targetRoleInfo = targetDataDirectory.FindDomainRole(roleId)) &&
								1 < (targetLinkedElements = targetRoleInfo.GetLinkedElements(pair.Value)).Count &&
								0 != (sourceLinkedElements = sourceRoleInfo.GetLinkedElements(key.Element)).Count)
							{
								// UNDONE: COPYMERGE Use MergeIntegrationOrder to more accurately
								// place elements in an existing collection. This moves all merged
								// elements to the front.
								MergeIntegrationOrder order = orderedRoles[roleId];
								int resolvedIndex = 0;
								foreach (ModelElement sourceLinkedElement in sourceLinkedElements)
								{
									CopiedElement copy;
									int targetIndex;
									if (resolvedElements.TryGetValue(sourceLinkedElement.Id, out copy) &&
										-1 != (targetIndex = targetLinkedElements.IndexOf(copy.Element)))
									{
										if (targetIndex != resolvedIndex)
										{
											targetLinkedElements.Move(targetIndex, resolvedIndex);
										}
										++resolvedIndex;
									}
								}
							}
						}
					}

					// Fifth phase: update properties for all existing elements
					foreach (KeyValuePair<Guid, CopiedElement> pair in resolvedElements)
					{
						CopiedElement copy = pair.Value;
						if (copy.CopyType == CopiedElementType.Existing)
						{
							ModelElement copyElement = copy.Element;
							PropertyAssignment[] propertyAssignments = GetCopiedPropertyAssignments(copyClosure[pair.Key].Element, copyElement);
							if (propertyAssignments != null)
							{
								// Update the element with new property assignments
								for (int i = 0; i < propertyAssignments.Length; ++i)
								{
									PropertyAssignment assignment = propertyAssignments[i];
									targetDataDirectory.GetDomainProperty(assignment.PropertyId).SetValue(copyElement, assignment.Value);
								}
							}
						}
					}
					if (t.HasPendingChanges)
					{
						removeContextIntegrationCompleteKey = true;
						contextInfo[CopyMergeUtility.CopyClosureIntegrationCompleteKey] = null;
						t.Commit();
					}
				}
				catch
				{
					t.Rollback();
					throw;
				}
				finally
				{
					if (listeners != null)
					{
						foreach (ICopyClosureIntegrationListener listener in listeners)
						{
							listener.EndCopyClosureIntegration(t);
						}
					}
					if (removeContextIntegratingKey)
					{
						contextInfo.Remove(CopyMergeUtility.CopyClosureIntegratingKey);
					}
					if (restoreContextIntegrationCompleteKey)
					{
						if (!removeContextIntegrationCompleteKey)
						{
							contextInfo[CopyMergeUtility.CopyClosureIntegrationCompleteKey] = null;
						}
					}
					else if (removeContextIntegratingKey)
					{
						contextInfo.Remove(CopyMergeUtility.CopyClosureIntegrationCompleteKey);
					}
					t.Dispose();
				}
			}
			else
			{
				// UNDONE: COPYMERGE Support copy within the same store. Create a copy of anything that is marked as Duplicate
				// and leave matched elements as is.
			}
			Dictionary<Guid, ModelElement> retVal = new Dictionary<Guid, ModelElement>(resolvedElements.Count); // Note that 0 capacity is acceptable
			foreach (KeyValuePair<Guid, CopiedElement> pair in resolvedElements)
			{
				retVal.Add(pair.Key, pair.Value.Element);
			}
			CopyClosureIntegrationResult result = new CopyClosureIntegrationResult();
			result.CopiedElements = retVal;
			if (missingExtensions != null)
			{
				Guid[] extensionIds = new Guid[missingExtensions.Count];
				missingExtensions.Keys.CopyTo(extensionIds, 0);
				result.MissingExtensionModels = extensionIds;
			}
			return result;
		}
		/// <summary>
		/// Helper method for <see cref="IntegrateCopyClosure"/>. Match a link between existing elements.
		/// </summary>
		private static ModelElement ResolveExistingLink(IClosureElementLink closureLink, IDictionary<Guid, IClosureElement> copyClosure, IDictionary<Guid, CopiedElement> resolvedElements, DomainDataDirectory targetDataDirectory, ref Dictionary<Guid, object> missingExtensions)
		{
			ModelElement closureElement = closureLink.Element;
			DomainRelationshipInfo relationshipInfo = (DomainRelationshipInfo)closureElement.GetDomainClass();
			if (relationshipInfo.AllowsDuplicates)
			{
				// Duplicate relationships must be resolved with IElementEquivalence or recreated.
				// Otherwise, there is insufficient information to accurately match the link.
				return null;
			}

			ModelElement resolvedSourceElement;
			ModelElement resolvedTargetElement;
			if (null == (resolvedSourceElement = ResolveExistingRolePlayer(closureLink.SourceRolePlayer, copyClosure, resolvedElements, targetDataDirectory, ref missingExtensions)) ||
				null == (resolvedTargetElement = ResolveExistingRolePlayer(closureLink.TargetRolePlayer, copyClosure, resolvedElements, targetDataDirectory, ref missingExtensions)))
			{
				return null;
			}

			// Go find the link, preferrably moving from a one role to a many role
			// for the most efficient match retrieval. We switch to the relationshipInfo
			// as defined in the target store at this point.
			DomainRelationshipInfo resolvedRelationshipInfo = targetDataDirectory.FindDomainRelationship(relationshipInfo.Id);
			if (resolvedRelationshipInfo == null)
			{
				(missingExtensions ?? (missingExtensions = new Dictionary<Guid, object>()))[relationshipInfo.DomainModel.Id] = null;
				return null;
			}
			ReadOnlyCollection<DomainRoleInfo> domainRoles = resolvedRelationshipInfo.DomainRoles;
			int getLinksFromRoleIndex = (domainRoles[0].IsOne || !domainRoles[1].IsOne) ? 0 : 1;
			DomainRoleInfo fromRoleInfo = domainRoles[getLinksFromRoleIndex];
			DomainRoleInfo toRoleInfo = domainRoles[1 - getLinksFromRoleIndex];
			bool fromSource = fromRoleInfo.IsSource;
			// Note that GetElementLinksToElement works only for source roles. It
			// also creates an extra collection that is not needed for non-duplicate
			// relationships. This is optimized to pull the role with the smallest
			// collection if possible.
			foreach (ElementLink link in fromRoleInfo.GetElementLinks(fromSource ? resolvedSourceElement : resolvedTargetElement))
			{
				if (toRoleInfo.GetRolePlayer(link) == (fromSource ? resolvedTargetElement : resolvedSourceElement))
				{
					resolvedElements[closureElement.Id] = new CopiedElement(link, CopiedElementType.Existing);
					return link; // There will only be one match because AllowDuplicates is false
				}
			}
			return null;
		}
		/// <summary>
		/// Helper method for <see cref="ResolveExistingLink"/>. Match a link between existing elements.
		/// </summary>
		private static ModelElement ResolveExistingRolePlayer(ModelElement element, IDictionary<Guid, IClosureElement> copyClosure, IDictionary<Guid, CopiedElement> resolvedElements, DomainDataDirectory targetDataDirectory, ref Dictionary<Guid, object> missingExtensions)
		{
			// Resolve the source element
			ModelElement retVal = null;
			ElementLink linkRolePlayer;
			CopiedElement resolvedElementCopy;
			Guid id = element.Id;
			if (resolvedElements.TryGetValue(id, out resolvedElementCopy))
			{
				if (resolvedElementCopy.CopyType != CopiedElementType.Existing)
				{
					// The endpoint is not existing node, cannot create the link
					return null;
				}
				retVal = resolvedElementCopy.Element;
			}
			else if (null != (linkRolePlayer = element as ElementLink))
			{
				retVal = ResolveExistingLink((IClosureElementLink)copyClosure[id], copyClosure, resolvedElements, targetDataDirectory, ref missingExtensions);
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for <see cref="IntegrateCopyClosure"/>. Create a link based on an existing link.
		/// </summary>
		private bool CreateLinkCopy(IClosureElementLink closureLink, DomainDataDirectory targetDataDirectory, ElementFactory targetElementFactory, IDictionary<Guid, IClosureElement> copyClosure, IDictionary<Guid, CopiedElement> resolvedElements, ref Dictionary<Guid, object> missingExtensions, ref Dictionary<RoleAndElement, ModelElement> requiresOrdering)
		{
			// Resolve the role players, with recursive handling for role players
			// that are themselves element links.
			ModelElement sourceElement = closureLink.SourceRolePlayer;
			ModelElement targetElement = closureLink.TargetRolePlayer;
			CopiedElement newSourceCopy;
			CopiedElement newTargetCopy;
			ElementLink linkRolePlayer;
			IClosureElement rolePlayerClosure;
			Guid elementId;
			if ((!resolvedElements.TryGetValue(elementId = sourceElement.Id, out newSourceCopy) &&
					(null == (linkRolePlayer = sourceElement as ElementLink) ||
					!(copyClosure.TryGetValue(elementId, out rolePlayerClosure) &&
					CreateLinkCopy((IClosureElementLink)rolePlayerClosure, targetDataDirectory, targetElementFactory, copyClosure, resolvedElements, ref missingExtensions, ref requiresOrdering) &&
					resolvedElements.TryGetValue(elementId, out newSourceCopy)))) ||
				(!resolvedElements.TryGetValue(elementId = targetElement.Id, out newTargetCopy) &&
					(null == (linkRolePlayer = targetElement as ElementLink) ||
					!(copyClosure.TryGetValue(elementId, out rolePlayerClosure) &&
					CreateLinkCopy((IClosureElementLink)rolePlayerClosure, targetDataDirectory, targetElementFactory, copyClosure, resolvedElements, ref missingExtensions, ref requiresOrdering) &&
					resolvedElements.TryGetValue(elementId, out newTargetCopy)))))
			{
				return false;
			}

			DomainRelationshipInfo sourceRelationshipInfo = (DomainRelationshipInfo)closureLink.Element.GetDomainClass();
			DomainRelationshipInfo targetRelationshipInfo = targetDataDirectory.FindDomainRelationship(sourceRelationshipInfo.Id);
			if (targetRelationshipInfo == null)
			{
				(missingExtensions ?? (missingExtensions = new Dictionary<Guid, object>()))[sourceRelationshipInfo.DomainModel.Id] = null;
				return false;
			}
			ReadOnlyCollection<DomainRoleInfo> domainRoles = sourceRelationshipInfo.DomainRoles;
			DomainRoleInfo sourceRoleInfo = domainRoles[0];
			DomainRoleInfo targetRoleInfo;
			DomainRoleInfo newStoreSourceRoleInfo;
			DomainRoleInfo newStoreTargetRoleInfo;
			if (sourceRoleInfo.IsSource)
			{
				targetRoleInfo = domainRoles[1];
			}
			else
			{
				targetRoleInfo = sourceRoleInfo;
				sourceRoleInfo = domainRoles[1];
			}

			ModelElement newSourceElement = newSourceCopy.Element;
			ModelElement newTargetElement = newTargetCopy.Element;

			// Respect existing links instead of arbitrarily creating new ones,
			// which will violate multiplicity on one-to-one, one-to-many, and
			// non-duplicate many-to-many relationships.
			ElementLink copiedLink = null;
			bool manyToMany = true;
			if (sourceRoleInfo.IsOne)
			{
				manyToMany = false;
				if (newSourceCopy.CopyType == CopiedElementType.Existing)
				{
					newStoreSourceRoleInfo = targetDataDirectory.GetDomainRole(sourceRoleInfo.Id);
					foreach (ElementLink testLink in newStoreSourceRoleInfo.GetElementLinks(newSourceElement))
					{
						newStoreTargetRoleInfo = targetDataDirectory.GetDomainRole(targetRoleInfo.Id);
						copiedLink = testLink;
						if (newTargetCopy.CopyType == CopiedElementType.Existing &&
							newStoreTargetRoleInfo.GetRolePlayer(testLink) == newTargetElement)
						{
							break;
						}
						// UNDONE: COPYMERGE Add order tracker for role player changes
						newStoreTargetRoleInfo.SetRolePlayer(testLink, newTargetElement);
						break;
					}
				}
			}
			if (copiedLink == null &&
				targetRoleInfo.IsOne)
			{
				manyToMany = false;
				if (newTargetCopy.CopyType == CopiedElementType.Existing)
				{
					newStoreTargetRoleInfo = targetDataDirectory.GetDomainRole(targetRoleInfo.Id);
					foreach (ElementLink testLink in newStoreTargetRoleInfo.GetElementLinks(newTargetElement))
					{
						newStoreSourceRoleInfo = targetDataDirectory.GetDomainRole(sourceRoleInfo.Id);
						copiedLink = testLink;
						if (newSourceCopy.CopyType == CopiedElementType.Existing &&
							newStoreSourceRoleInfo.GetRolePlayer(testLink) == newSourceElement)
						{
							break;
						}
						// UNDONE: COPYMERGE Add order tracker for role player changes
						newStoreSourceRoleInfo.SetRolePlayer(testLink, newSourceElement);
						break;
					}
				}
			}
			if (copiedLink == null &&
				manyToMany &&
				!sourceRelationshipInfo.AllowsDuplicates)
			{
				DomainRoleInfo targetTargetRoleInfo = null;
				// Don't trust GetElementLinksToElement, it only works from the source
				// role info, and creates an extra collection we don't need for a
				// relationship that does not allow duplicates.
				foreach (ElementLink testElementLink in targetDataDirectory.GetDomainRole(sourceRoleInfo.Id).GetElementLinks(newSourceElement))
				{
					if ((targetTargetRoleInfo ?? (targetTargetRoleInfo = targetDataDirectory.GetDomainRole(targetRoleInfo.Id))).GetRolePlayer(testElementLink) == newTargetElement)
					{
						copiedLink = testElementLink;
						break;
					}
				}
			}

			ModelElement basedOnElement = closureLink.Element;
			CopiedElementType copyType;
			if (copiedLink != null)
			{
				// Just copy. We'll set all properties at the end of the integration phase
				copyType = CopiedElementType.Existing;
			}
			else
			{
				PropertyAssignment[] propertyAssignments = GetCopiedPropertyAssignments(basedOnElement, copiedLink);
				copyType = CopiedElementType.New;
				Guid sourceId = sourceRoleInfo.Id;
				Guid targetId = targetRoleInfo.Id;
				copiedLink = propertyAssignments != null ?
					targetElementFactory.CreateElementLink(
						targetRelationshipInfo,
						propertyAssignments,
						new RoleAssignment(sourceId, newSourceElement),
						new RoleAssignment(targetId, newTargetElement)) :
					targetElementFactory.CreateElementLink(
						targetRelationshipInfo,
						new RoleAssignment(sourceId, newSourceElement),
						new RoleAssignment(targetId, newTargetElement));
				if (!InitializeNewElement(copiedLink, basedOnElement))
				{
					return false;
				}
				Dictionary<Guid, MergeIntegrationOrder> orderedRoles = myOrderedRoles;
				if (orderedRoles != null)
				{
					TrackOrdering(orderedRoles, sourceId, sourceElement, newSourceElement, ref requiresOrdering);
					TrackOrdering(orderedRoles, targetId, targetElement, newTargetElement, ref requiresOrdering);
				}
			}
			resolvedElements[basedOnElement.Id] = new CopiedElement(copiedLink, copyType);
			return true;
		}
		private static void TrackOrdering(Dictionary<Guid, MergeIntegrationOrder> orderedRoles, Guid domainRoleId, ModelElement basedOnRolePlayer, ModelElement newRolePlayer, ref Dictionary<RoleAndElement, ModelElement> requiresOrdering)
		{
			if (orderedRoles != null &&
				orderedRoles.ContainsKey(domainRoleId))
			{
				ModelElement registeredNewRolePlayer;
				RoleAndElement key;
				if (requiresOrdering == null)
				{
					requiresOrdering = new Dictionary<RoleAndElement, ModelElement>();
					requiresOrdering[new RoleAndElement(domainRoleId, basedOnRolePlayer)] = newRolePlayer;
				}
				else if (!requiresOrdering.TryGetValue(key = new RoleAndElement(domainRoleId, basedOnRolePlayer), out registeredNewRolePlayer) ||
					registeredNewRolePlayer != newRolePlayer)
				{
					requiresOrdering[key] = newRolePlayer;
				}
			}
		}
		/// <summary>
		/// Helper function for <see cref="IntegrateCopyClosure"/>. Embeds a newly
		/// created element in an existing root element.
		/// </summary>
		private void IntegrateRootElement(ModelElement basedOnElement, ModelElement newElement, IEnumerable<ModelElement> targetContextElements, DomainDataDirectory targetDataDirectory, ElementFactory targetElementFactory, DomainDataDirectory sourceDataDirectory, ref Dictionary<Guid, object> missingExtensions, ref Dictionary<RoleAndElement, ModelElement> requiresOrdering)
		{
			Dictionary<Type, LinkedNode<Guid>> embeddedRoles;
			LinkedNode<Guid> roleNode;
			if (null != (embeddedRoles = myEmbeddedRoles) &&
				embeddedRoles.TryGetValue(newElement.GetDomainClass().ImplementationClass, out roleNode))
			{
				DomainRoleInfo embeddedRoleInfo = null;
				if (roleNode.Next == null)
				{
					// We have only one choice, short circuit tie breakers
					// Use Find instead of Get to handle source/target situations
					// where the extension models may be different.
					embeddedRoleInfo = targetDataDirectory.FindDomainRole(roleNode.Value);
				}
				else
				{
					while (roleNode != null)
					{
						if (null != (embeddedRoleInfo = sourceDataDirectory.FindDomainRole(roleNode.Value)))
						{
							if (embeddedRoleInfo.GetElementLinks(basedOnElement).Count != 0)
							{
								{
									break;
								}
							}
							embeddedRoleInfo = null;
						}
						roleNode = roleNode.Next;
					}
				}
				if (embeddedRoleInfo != null)
				{
					DomainRelationshipInfo sourceRelationshipInfo = embeddedRoleInfo.DomainRelationship;
					DomainRelationshipInfo targetRelationshipInfo = targetDataDirectory.FindDomainRelationship(sourceRelationshipInfo.Id);
					if (targetRelationshipInfo == null)
					{
						(missingExtensions ?? (missingExtensions = new Dictionary<Guid, object>()))[sourceRelationshipInfo.DomainModel.Id] = null;
					}
					else
					{
						DomainRoleInfo embeddingRoleInfo = embeddedRoleInfo.OppositeDomainRole;
						DomainClassInfo parentClassInfo = embeddingRoleInfo.RolePlayer;
						Type implementationClass = parentClassInfo.ImplementationClass;
						ModelElement rootElement = null;
						if (targetContextElements != null)
						{
							foreach (ModelElement element in targetContextElements)
							{
								if (implementationClass.IsAssignableFrom(element.GetType()))
								{
									rootElement = element;
									break;
								}
							}
						}
						if (rootElement == null)
						{
							DomainClassInfo targetClassInfo = targetDataDirectory.FindDomainClass(parentClassInfo.Id);
							if (targetClassInfo == null)
							{
								(missingExtensions ?? (missingExtensions = new Dictionary<Guid, object>()))[sourceRelationshipInfo.DomainModel.Id] = null;
							}
							else
							{
								foreach (ModelElement element in newElement.Store.ElementDirectory.FindElements(targetClassInfo))
								{
									rootElement = element;
									break;
								}
								if (rootElement == null)
								{
									rootElement = targetElementFactory.CreateElement(targetClassInfo);
								}
							}
						}
						if (rootElement != null)
						{
							Guid embeddingRoleId = embeddingRoleInfo.Id;
							Guid embeddedRoleId = embeddedRoleInfo.Id;
							targetElementFactory.CreateElementLink(
								targetRelationshipInfo,
								new RoleAssignment(embeddingRoleId, rootElement),
								new RoleAssignment(embeddedRoleId, newElement));
							Dictionary<Guid, MergeIntegrationOrder> orderedRoles = myOrderedRoles;
							if (null != (orderedRoles = myOrderedRoles))
							{
								if (orderedRoles.ContainsKey(embeddingRoleId)) // Pre check so we don't fetch elements we don't need
								{
									foreach (ModelElement basedOnRootElement in embeddedRoleInfo.GetLinkedElements(basedOnElement))
									{
										TrackOrdering(orderedRoles, embeddingRoleId, basedOnRootElement, rootElement, ref requiresOrdering);
										break;
									}
								}
								TrackOrdering(orderedRoles, embeddedRoleId, basedOnElement, newElement, ref requiresOrdering);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Run custom initialization code for an element created during a copy
		/// using the <see cref="IInitializeAfterCopy"/> interface. Note that this
		/// process can end up deleting the element, in which case this method
		/// will return false and the <paramref name="newElement"/> should be
		/// abandoned.
		/// </summary>
		private static bool InitializeNewElement(ModelElement newElement, ModelElement basedOnElement)
		{
			IInitializeAfterCopy initializer = newElement as IInitializeAfterCopy;
			if (initializer != null)
			{
				initializer.InitializedAfterCopy(basedOnElement);
				if (newElement.IsDeleted)
				{
					return false;
				}
			}
			return true;
		}
		#endregion // Closure Duplication Methods
		#region Closure Population Methods
		#region ClosureElementBase class
		private abstract class ClosureElementBase : IClosureElement
		{
			#region Member Variables
			private static readonly IClosureElement[] EmptyClosureElements = new IClosureElement[0];
			private readonly ModelElement myElement;
			private object myPartOf; // Either an IClosureElement or a LinkedNode<IClosureElement>
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="ClosureElementBase"/> for a given <see cref="ModelElement"/>
			/// </summary>
			protected ClosureElementBase(ModelElement element)
			{
				myElement = element;
			}
			#endregion // Constructor
			#region IClosureElement Implementation
			protected ModelElement Element
			{
				get
				{
					return myElement;
				}
			}
			ModelElement IClosureElement.Element
			{
				get
				{
					return myElement;
				}
			}
			CopyMergeAction IClosureElement.Action
			{
				get
				{
					// Place, overrides in derived classes
					return CopyMergeAction.Duplicate;
				}
			}
			IEnumerable<IClosureElement> IClosureElement.ReferencedPartOf
			{
				get
				{
					object partOf = myPartOf;
					IClosureElement closureElement;
					if (partOf == null)
					{
						return EmptyClosureElements;
					}
					else if (null != (closureElement = partOf as IClosureElement))
					{
						return new IClosureElement[] { closureElement};
					}
					return (LinkedNode<IClosureElement>)partOf;
				}
			}
			#endregion // IClosureElement Implementation
			#region ClosureElementBase methods
			/// <summary>
			/// Add this node as a reference part of <paramref name="createdWith"/>
			/// </summary>
			public void AddReferencedPartOf(IClosureElement createdWith)
			{
				object partOf = myPartOf;
				IClosureElement closureElement;
				LinkedNode<IClosureElement> node;
				if (partOf == null)
				{
					myPartOf = createdWith;
				}
				else if (null != (closureElement = partOf as IClosureElement))
				{
					if (createdWith != closureElement)
					{
						node = new LinkedNode<IClosureElement>(closureElement);
						node.SetNext(new LinkedNode<IClosureElement>(createdWith), ref node);
						myPartOf = node;
					}
				}
				else
				{
					LinkedNode<IClosureElement> headNode = node = (LinkedNode<IClosureElement>)partOf;
					LinkedNode<IClosureElement> lastNode;
					do
					{
						if (node.Value == createdWith)
						{
							return;
						}
						lastNode = node;
						node = node.Next;
					} while (node != null);
					lastNode.SetNext(new LinkedNode<IClosureElement>(createdWith), ref headNode);
				}
			}
			#endregion // ClosureElementBase methods
		}
		#endregion // ClosureElementBase class
		#region ClosureElement class
		private sealed class ClosureElement : ClosureElementBase, IClosureElement
		{
			#region Member Variables
			private readonly CopyMergeAction myAction;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="ClosureElement"/> for a given <see cref="ModelElement"/>
			/// and <see cref="CopyMergeAction"/>
			/// </summary>
			public ClosureElement(ModelElement element, CopyMergeAction action)
				: base(element)
			{
				myAction = action;
			}
			#endregion // Constructor
			#region IClosureElement Implementation
			CopyMergeAction IClosureElement.Action
			{
				get
				{
					return myAction;
				}
			}
			#endregion // IClosureElement Implementation
		}
		#endregion // ClosureElement class
		#region ClosureElementLink class
		private sealed class ClosureElementLink : ClosureElementBase, IClosureElement, IClosureElementLink
		{
			#region Member Variables
			private readonly ModelElement mySourceRolePlayer;
			private readonly ModelElement myTargetRolePlayer;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="ClosureElementLink"/> for a given <see cref="ElementLink"/>
			/// and <see cref="CopyMergeAction"/>
			/// </summary>
			public ClosureElementLink(ElementLink elementLink)
				: base(elementLink)
			{
				foreach (DomainRoleInfo roleInfo in elementLink.GetDomainRelationship().DomainRoles)
				{
					ModelElement rolePlayer = roleInfo.GetRolePlayer(elementLink);
					if (roleInfo.IsSource)
					{
						mySourceRolePlayer = rolePlayer;
					}
					else
					{
						myTargetRolePlayer = rolePlayer;
					}
				}
			}
			#endregion // Constructor
			#region IClosureElement Implementation
			CopyMergeAction IClosureElement.Action
			{
				get
				{
					return CopyMergeAction.Link;
				}
			}
			#endregion // IClosureElement Implementation
			#region IClosureElementLink Implementation
			ModelElement IClosureElementLink.SourceRolePlayer
			{
				get
				{
					return mySourceRolePlayer;
				}
			}
			ModelElement IClosureElementLink.TargetRolePlayer
			{
				get
				{
					return myTargetRolePlayer;
				}
			}
			#endregion // IClosureElementLink Implementation
		}
		#endregion // ClosureElementLink class
		private Dictionary<Guid, IClosureElement> GenerateCopyClosure(IEnumerable<ModelElement> rootElements)
		{
			// To avoid having to strengthen relationships from Match to Duplicate, we
			// process the full required closure of each element before moving on to the
			// potentially referenced elements.
			EnsureClosureDirectives();
			Dictionary<Guid, IClosureElement> retVal = new Dictionary<Guid, IClosureElement>();
			Dictionary<ModelElement, object> processedDuplicateNonRootParts = new Dictionary<ModelElement, object>();
			foreach (ModelElement rootElement in rootElements)
			{
				GenerateElementClosure(rootElement, retVal, null, CopyMergeAction.Duplicate, true, processedDuplicateNonRootParts);
			}
			foreach (ModelElement rootElement in rootElements)
			{
				GenerateElementClosure(rootElement, retVal, null, CopyMergeAction.Duplicate, false, processedDuplicateNonRootParts);
			}
			return retVal;
		}
		/// <summary>
		/// Process an element used in a closure.
		/// </summary>
		/// <param name="element">The element to process</param>
		/// <param name="closureDictionary">Dictionary of closure elements</param>
		/// <param name="createWith">The first element referenced as an external reference part
		/// from another block of related elements.</param>
		/// <param name="action">The merge action to apply.</param>
		/// <param name="rootParts">Process root parts only, stopping on a ExternalReferencedPart link.
		/// If this is false then continue down external path parts.</param>
		/// <param name="processedDuplicateNonRootParts">A helper dictionary to avoid recurse for non-root
		/// part tracking.</param>
		private void GenerateElementClosure(ModelElement element, Dictionary<Guid, IClosureElement> closureDictionary, IClosureElement createWith, CopyMergeAction action, bool rootParts, Dictionary<ModelElement, object> processedDuplicateNonRootParts)
		{
			Guid elementId = element.Id;
			IClosureElement existingClosure;
			if (closureDictionary.TryGetValue(elementId, out existingClosure))
			{
				if (existingClosure.Action == CopyMergeAction.Duplicate)
				{
					if (!rootParts && !processedDuplicateNonRootParts.ContainsKey(element))
					{
						processedDuplicateNonRootParts.Add(element, null);
						ProcessAttachedLinks(element, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
					}
				}
				else if (createWith != null)
				{
					((ClosureElementBase)existingClosure).AddReferencedPartOf(createWith);
				}
			}
			else
			{
				ElementLink link = element as ElementLink;
				if (link != null)
				{
					closureDictionary.Add(elementId, new ClosureElementLink(link));
					foreach (ModelElement linkedElement in link.LinkedElements)
					{
						GenerateElementClosure(linkedElement, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
					}
				}
				else
				{
					closureDictionary.Add(elementId, new ClosureElement(element, action));
				}
				ProcessAttachedLinks(element, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
			}
		}
		private void ProcessAttachedLinks(ModelElement element, Dictionary<Guid, IClosureElement> closureDictionary, IClosureElement createWith, CopyMergeAction action, bool rootParts, Dictionary<ModelElement, object> processedDuplicateNonRootParts)
		{
			Dictionary<BoundEntryRole, EvaluateCopyClosure> normalBehaviors = myNormalBehaviors;
			Dictionary<BoundEntryRole, EvaluateCopyClosure> primaryBehaviors = myPrimaryBehaviors;
			Dictionary<Guid, AvailableClosureBehaviors> availableBehaviorsDictionary = myAvailableBehaviors;
			Type nearType = element.GetType();
			DomainClassInfo domainClass = element.GetDomainClass();
			foreach (DomainRoleInfo roleInfo in domainClass.AllDomainRolesPlayed)
			{
				// Verify preliminary information for this role before getting instances
				Guid roleId = roleInfo.Id;
				AvailableClosureBehaviors availableBehaviors;
				if (availableBehaviorsDictionary.TryGetValue(roleId, out availableBehaviors))
				{
					if (action != CopyMergeAction.Duplicate)
					{
						availableBehaviors &= ~(AvailableClosureBehaviors.PrimaryUntyped | AvailableClosureBehaviors.PrimaryEntryRoleRestricted | AvailableClosureBehaviors.PrimaryExitRoleRestricted | AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted);
						if (availableBehaviors == AvailableClosureBehaviors.None)
						{
							continue;
						}
					}
					bool needExitRoleType = 0 != (availableBehaviors & AvailableClosureBehaviors.ExitRoleRestricted | AvailableClosureBehaviors.EntryAndExitRoleRestricted | AvailableClosureBehaviors.PrimaryExitRoleRestricted | AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted);
					DomainRoleInfo oppositeRoleInfo = null;

					foreach (ElementLink elementLink in roleInfo.GetElementLinks(element))
					{
						ModelElement oppositeRolePlayer = null;
						Type farType = needExitRoleType ? (oppositeRolePlayer = (oppositeRoleInfo ?? (oppositeRoleInfo = roleInfo.OppositeDomainRole)).GetRolePlayer(elementLink)).GetType() : null;

						// Test available closures, moving from strongest to weakest typing
						// Give the entry role type priority, consistent with comparison in RolePlayerTypeDistance.Compare
						EvaluateCopyClosure evaluator = null;
						if (!((0 != (availableBehaviors & AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted) && primaryBehaviors.TryGetValue(new BoundEntryRole(roleId, nearType, farType), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.PrimaryEntryRoleRestricted) && primaryBehaviors.TryGetValue(new BoundEntryRole(roleId, nearType, null), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.PrimaryExitRoleRestricted) && primaryBehaviors.TryGetValue(new BoundEntryRole(roleId, null, farType), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.PrimaryUntyped) && primaryBehaviors.TryGetValue(new BoundEntryRole(roleId), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.EntryAndExitRoleRestricted) && normalBehaviors.TryGetValue(new BoundEntryRole(roleId, nearType, farType), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.EntryRoleRestricted) && normalBehaviors.TryGetValue(new BoundEntryRole(roleId, nearType, null), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.ExitRoleRestricted) && normalBehaviors.TryGetValue(new BoundEntryRole(roleId, null, farType), out evaluator)) ||
							(0 != (availableBehaviors & AvailableClosureBehaviors.Untyped) && normalBehaviors.TryGetValue(new BoundEntryRole(roleId), out evaluator))))
						{
							continue;
						}
						Guid linkId;
						switch (evaluator(elementLink))
						{
							case CopyClosureBehavior.Ignored:
								continue;
							case CopyClosureBehavior.ExternalReferencedPart:
								if (rootParts)
								{
									// We'll pick this up on a subsequent pass.
									continue;
								}
								else
								{
									// The current element becomes the context element for this new element
									// and everything downstream.
									if (!closureDictionary.ContainsKey(linkId = elementLink.Id))
									{
										closureDictionary.Add(linkId, new ClosureElementLink(elementLink));
										GenerateElementClosure(oppositeRolePlayer ?? (oppositeRoleInfo ?? (oppositeRoleInfo = roleInfo.OppositeDomainRole)).GetRolePlayer(elementLink), closureDictionary, closureDictionary[element.Id], CopyMergeAction.Match, false, processedDuplicateNonRootParts);
										ProcessAttachedLinks(elementLink, closureDictionary, createWith, CopyMergeAction.Match, false, processedDuplicateNonRootParts);
									}
								}
								break;
							case CopyClosureBehavior.ContainedPart:
							case CopyClosureBehavior.Container:
							case CopyClosureBehavior.InternalReferencedPart:
							case CopyClosureBehavior.ExternalCompositePart:
								if (rootParts ||
									action != CopyMergeAction.Duplicate)
								{
									if (!closureDictionary.ContainsKey(linkId = elementLink.Id))
									{
										closureDictionary.Add(linkId, new ClosureElementLink(elementLink));
										GenerateElementClosure(oppositeRolePlayer ?? (oppositeRoleInfo ?? (oppositeRoleInfo = roleInfo.OppositeDomainRole)).GetRolePlayer(elementLink), closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
										ProcessAttachedLinks(elementLink, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
									}
									else if (!rootParts)
									{
										GenerateElementClosure(oppositeRolePlayer ?? (oppositeRoleInfo ?? (oppositeRoleInfo = roleInfo.OppositeDomainRole)).GetRolePlayer(elementLink), closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
										if (!processedDuplicateNonRootParts.ContainsKey(elementLink))
										{
											processedDuplicateNonRootParts.Add(elementLink, null);
											ProcessAttachedLinks(elementLink, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
										}

									}
								}
								else
								{
									// UNDONE: COPYMERGE Is the element always added in the previous pass? If so, remove the assert
									Debug.Assert(closureDictionary.ContainsKey(elementLink.Id));
									GenerateElementClosure(oppositeRolePlayer ?? (oppositeRoleInfo ?? (oppositeRoleInfo = roleInfo.OppositeDomainRole)).GetRolePlayer(elementLink), closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
									ProcessAttachedLinks(elementLink, closureDictionary, createWith, action, rootParts, processedDuplicateNonRootParts);
								}
								break;
						}
					}
				}
			}

			// See if there are any implied relationships to other elements
			if (!rootParts)
			{
				Dictionary<Guid, EvaluateImpliedReference> impliedReferenceEvaluators;
				EvaluateImpliedReference impliedReferenceEvaluator;
				if (null != (impliedReferenceEvaluators = myImpliedReferenceEvaluators) &&
					impliedReferenceEvaluators.TryGetValue(domainClass.Id, out impliedReferenceEvaluator))
				{
					impliedReferenceEvaluator(
						element,
						delegate(ModelElement impliedElement)
						{
							GenerateElementClosure(impliedElement, closureDictionary, closureDictionary[element.Id], CopyMergeAction.Match, false, processedDuplicateNonRootParts);
						});
				}
			}
		}
		/// <summary>
		/// Get a cached list of all properties that should be copied when an
		/// element is created or merged.
		/// </summary>
		/// <param name="classId">The id of a <see cref="DomainClassInfo"/> or <see cref="DomainRelationshipInfo"/></param>
		/// <returns>Array of property identifiers.</returns>
		private CopiedPropertiesCache GetCopiedProperties(Guid classId)
		{
			CopiedPropertiesCache retVal;
			Dictionary<Guid, CopiedPropertiesCache> copiedProperties = myCopiedProperties;
			if (copiedProperties == null)
			{
				myCopiedProperties = copiedProperties = new Dictionary<Guid, CopiedPropertiesCache>();
			}
			else if (copiedProperties.TryGetValue(classId, out retVal))
			{
				return retVal;
			}
			DomainDataDirectory dataDirectory = myStore.DomainDataDirectory;
			ReadOnlyCollection<DomainPropertyInfo> allProperties = dataDirectory.GetDomainClass(classId).AllDomainProperties;
			int propertyCount = allProperties.Count;
			if (propertyCount != 0)
			{
				Guid[] identifiers = new Guid[propertyCount];
				ConditionalPropertyTest[][] allConditions = null;
				int nextPropertyIndex = 0;
				Dictionary<Guid, object> ignoredProperties = myIgnoredProperties;
				Dictionary<Guid, ConditionalPropertyTest> propertyConditions = myPropertyConditions;
				int skippedPropertiesCount = 0;
				for (int i = 0; i < propertyCount; ++i)
				{
					DomainPropertyInfo propertyInfo = allProperties[i];
					Guid propertyId = propertyInfo.Id;
					if (ignoredProperties != null && ignoredProperties.ContainsKey(propertyId) ||
						propertyInfo.Kind == DomainPropertyKind.Calculated)
					{
						++skippedPropertiesCount;
						continue;
					}
					ConditionalPropertyTest condition;
					if (propertyConditions != null &&
						propertyConditions.TryGetValue(propertyId, out condition))
					{
						Delegate[] invocationList = condition.GetInvocationList();
						int invocationLength = invocationList.Length;
						ConditionalPropertyTest[] conditions = new ConditionalPropertyTest[invocationLength];
						for (int j = 0; j < invocationLength; ++j)
						{
							conditions[j] = (ConditionalPropertyTest)invocationList[j];
						}
						(allConditions ?? (allConditions = new ConditionalPropertyTest[propertyCount - skippedPropertiesCount][]))[nextPropertyIndex] = conditions;
					}
					identifiers[nextPropertyIndex] = propertyId;
					++nextPropertyIndex;
				}
				if (nextPropertyIndex < propertyCount)
				{
					if (nextPropertyIndex == 0)
					{
						retVal = CopiedPropertiesCache.Empty;
					}
					else
					{
						Array.Resize<Guid>(ref identifiers, nextPropertyIndex);
						if (null != allConditions && allConditions.Length > nextPropertyIndex)
						{
							Array.Resize<ConditionalPropertyTest[]>(ref allConditions, nextPropertyIndex);
						}
						retVal = new CopiedPropertiesCache(identifiers, allConditions);
					}
				}
				else
				{
					retVal = new CopiedPropertiesCache(identifiers, allConditions);
				}
				copiedProperties[classId] = retVal;
			}
			else
			{
				retVal = CopiedPropertiesCache.Empty;
			}
			return retVal;
		}
		/// <summary>
		/// Get property assignments to copy when creating a new element
		/// </summary>
		/// <param name="sourceElement">The element to retrieve properties from.</param>
		/// <param name="targetElement">The element the properties will be copied to.</param>
		/// <returns>An array of properties, or <see langword="null"/></returns>
		private PropertyAssignment[] GetCopiedPropertyAssignments(ModelElement sourceElement, ModelElement targetElement)
		{
			CopiedPropertiesCache copiedPropertiesInfo = GetCopiedProperties(sourceElement.GetDomainClass().Id);
			Guid[] propertyIds = copiedPropertiesInfo.Identifiers;
			int propertyCount = propertyIds.Length;
			if (propertyCount != 0)
			{
				ConditionalPropertyTest[][] allConditions = copiedPropertiesInfo.Conditions;
				DomainDataDirectory dataDirectory = sourceElement.Store.DomainDataDirectory;
				PropertyAssignment[] retVal;
				if (allConditions != null)
				{
					int unblockedPropertyCount = propertyCount;
					BitTracker blockPropertyIndices = new BitTracker(propertyCount);
					for (int i = 0; i < propertyCount; ++i)
					{
						ConditionalPropertyTest[] conditions = allConditions[i];
						if (conditions != null)
						{
							for (int j = 0; j < conditions.Length; ++j)
							{
								if (!conditions[j](sourceElement, targetElement))
								{
									--unblockedPropertyCount;
									blockPropertyIndices[i] = true;
									break;
								}
							}
						}
					}
					if (unblockedPropertyCount == 0)
					{
						return null;
					}
					retVal = new PropertyAssignment[unblockedPropertyCount];
					int nextKeptIndex = 0;
					for (int i = 0; i < propertyCount; ++i)
					{
						if (blockPropertyIndices[i])
						{
							continue;
						}
						Guid propertyId = propertyIds[i];
						retVal[nextKeptIndex] = new PropertyAssignment(propertyId, dataDirectory.GetDomainProperty(propertyId).GetValue(sourceElement));
						++nextKeptIndex;
					}
				}
				else
				{
					retVal = new PropertyAssignment[propertyCount];
					for (int i = 0; i < propertyCount; ++i)
					{
						Guid propertyId = propertyIds[i];
						retVal[i] = new PropertyAssignment(propertyId, dataDirectory.GetDomainProperty(propertyId).GetValue(sourceElement));
					}
				}
				return retVal;
			}
			return null;
		}
		#endregion // Closure Population Methods
		#region Closure Directive Population Methods
		private void AddCopyClosureDirective(DomainRoleClosureRestriction fromRole, DomainRoleClosureRestriction toRole, CopyClosureDirectiveOptions directiveOptions, EvaluateCopyClosure customClosureEvaluation)
		{
			// Choose the correct directionary
			Dictionary<BoundEntryRole, EvaluateCopyClosure> behaviorDictionary;
			Dictionary<BoundEntryRole, RolePlayerTypeDistance> distanceDictionary;
			Dictionary<Guid, AvailableClosureBehaviors> availableBehaviorsDictionary = myAvailableBehaviors;
			bool primaryClosure = 0 != (directiveOptions & CopyClosureDirectiveOptions.RootElementOnly);
			if (primaryClosure)
			{
				behaviorDictionary = myPrimaryBehaviors;
				if (behaviorDictionary == null)
				{
					myPrimaryBehaviors = behaviorDictionary = new Dictionary<BoundEntryRole, EvaluateCopyClosure>();
				}
				distanceDictionary = myPrimaryDistanceTracker;
			}
			else
			{
				behaviorDictionary = myNormalBehaviors;
				distanceDictionary = myNormalDistanceTracker;
			}
			bool updateDistanceDictionary = distanceDictionary == null;

			// Map the from and to roles to one or more explicit signatures
			Guid entryRoleId = fromRole.RoleId;
			AvailableClosureBehaviors startingRoleBehaviors;
			availableBehaviorsDictionary.TryGetValue(entryRoleId, out startingRoleBehaviors);
			AvailableClosureBehaviors availableRoleBehaviors = startingRoleBehaviors;
			Type entryRolePlayerType = null;
			Type[] entryRolePlayerTypes = null;
			Type exitRolePlayerType = null;
			Type[] exitRolePlayerTypes = null;
			int entryTypeDistance = -1;
			int exitTypeDistance = -1;
			DomainDataDirectory dataDirectory = myStore.DomainDataDirectory;
			DomainClassInfo nativeRolePlayer;
			Guid restrictionId;
			DomainClassInfo restrictionRolePlayer;

			// Get role player restriction information from the entry role
			if ((restrictionId = fromRole.RolePlayerRestrictionClassId) != Guid.Empty &&
				null != (restrictionRolePlayer = dataDirectory.FindDomainClass(restrictionId)))
			{
				// Resolve classes to types
				if (!fromRole.IncludeClassRestrictionDescendants ||
					null == (entryRolePlayerTypes = GetDerivedImplementationClasses(restrictionRolePlayer)))
				{
					entryRolePlayerType = restrictionRolePlayer.ImplementationClass;
				}

				// Determine the distance from the base role player to the root so that
				// a stronger-typed closure directive can override this one.
				nativeRolePlayer = dataDirectory.GetDomainRole(entryRoleId).RolePlayer;
				entryTypeDistance = 0;
				while (nativeRolePlayer != restrictionRolePlayer)
				{
					restrictionRolePlayer = restrictionRolePlayer.BaseDomainClass;
					++entryTypeDistance;
				}
			}

			// Get role player restriction information for exit role
			if ((restrictionId = toRole.RolePlayerRestrictionClassId) != Guid.Empty &&
				null != (restrictionRolePlayer = dataDirectory.FindDomainClass(restrictionId)))
			{
				// Resolve classes to types
				if (!toRole.IncludeClassRestrictionDescendants ||
					null == (exitRolePlayerTypes = GetDerivedImplementationClasses(restrictionRolePlayer)))
				{
					exitRolePlayerType = restrictionRolePlayer.ImplementationClass;
				}

				// Determine the distance from the base role player to the root so that
				// a stronger-typed closure directive can override this one.
				nativeRolePlayer = dataDirectory.GetDomainRole(toRole.RoleId).RolePlayer;
				exitTypeDistance = 0;
				while (nativeRolePlayer != restrictionRolePlayer)
				{
					restrictionRolePlayer = restrictionRolePlayer.BaseDomainClass;
					++exitTypeDistance;
				}
			}

			// Record all type combinations for this role
			if (entryRolePlayerTypes != null)
			{
				if (exitRolePlayerTypes != null)
				{
					for (int i = 0; i < entryRolePlayerTypes.Length; ++i)
					{
						Type entryType = entryRolePlayerTypes[i];
						for (int j = 0; j < exitRolePlayerTypes.Length; ++j)
						{
							RegisterClosureEvaluation(
								behaviorDictionary,
								new BoundEntryRole(entryRoleId, entryType, exitRolePlayerTypes[j]),
								customClosureEvaluation,
								entryTypeDistance,
								exitTypeDistance,
								ref distanceDictionary);
						}
					}
					availableRoleBehaviors |= primaryClosure ? AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted : AvailableClosureBehaviors.EntryAndExitRoleRestricted;
				}
				else
				{
					for (int i = 0; i < entryRolePlayerTypes.Length; ++i)
					{
						RegisterClosureEvaluation(
							behaviorDictionary,
							new BoundEntryRole(entryRoleId, entryRolePlayerTypes[i], exitRolePlayerType),
							customClosureEvaluation,
							entryTypeDistance,
							exitTypeDistance,
							ref distanceDictionary);
					}
					availableRoleBehaviors |= exitRolePlayerType != null ?
						(primaryClosure ? AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted : AvailableClosureBehaviors.EntryAndExitRoleRestricted) :
						(primaryClosure ? AvailableClosureBehaviors.PrimaryEntryRoleRestricted : AvailableClosureBehaviors.EntryRoleRestricted);
				}
			}
			else if (exitRolePlayerTypes != null)
			{
				for (int i = 0; i < exitRolePlayerTypes.Length; ++i)
				{
					RegisterClosureEvaluation(
						behaviorDictionary,
						new BoundEntryRole(entryRoleId, entryRolePlayerType, exitRolePlayerTypes[i]),
						customClosureEvaluation,
						entryTypeDistance,
						exitTypeDistance,
						ref distanceDictionary);
				}
				availableRoleBehaviors |= entryRolePlayerType != null ?
					(primaryClosure ? AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted : AvailableClosureBehaviors.EntryAndExitRoleRestricted) :
					(primaryClosure ? AvailableClosureBehaviors.PrimaryExitRoleRestricted : AvailableClosureBehaviors.ExitRoleRestricted);
			}
			else
			{
				RegisterClosureEvaluation(
					behaviorDictionary,
					new BoundEntryRole(entryRoleId, entryRolePlayerType, exitRolePlayerType),
					customClosureEvaluation,
					entryTypeDistance,
					exitTypeDistance,
					ref distanceDictionary);
				availableRoleBehaviors |= entryRolePlayerType != null ?
					(exitRolePlayerType != null ?
						(primaryClosure ? AvailableClosureBehaviors.PrimaryEntryAndExitRoleRestricted : AvailableClosureBehaviors.EntryAndExitRoleRestricted) :
						(primaryClosure ? AvailableClosureBehaviors.PrimaryEntryRoleRestricted : AvailableClosureBehaviors.EntryRoleRestricted)) :
					(exitRolePlayerType != null ?
						(primaryClosure ? AvailableClosureBehaviors.PrimaryExitRoleRestricted : AvailableClosureBehaviors.ExitRoleRestricted) :
						(primaryClosure ? AvailableClosureBehaviors.PrimaryUntyped : AvailableClosureBehaviors.Untyped));
			}

			if (updateDistanceDictionary &&
				distanceDictionary != null)
			{
				if (primaryClosure)
				{
					myPrimaryDistanceTracker = distanceDictionary;
				}
				else
				{
					myNormalDistanceTracker = distanceDictionary;
				}
			}
			if (availableRoleBehaviors != startingRoleBehaviors)
			{
				availableBehaviorsDictionary[entryRoleId] = availableRoleBehaviors;
			}
		}
		/// <summary>
		/// Helper for AddCopyClosureDirective to get an array of
		/// implementation classes derived from the given class. The
		/// returned array contains the implementation class for the
		/// provided <see cref="DomainClassInfo"/> if there are descendants,
		/// and returns <see langword="null"/> otherwise.
		/// </summary>
		private static Type[] GetDerivedImplementationClasses(DomainClassInfo classInfo)
		{
			IList<DomainClassInfo> descendants = classInfo.AllDescendants;
			int descendantCount = descendants.Count;
			if (descendantCount != 0)
			{
				Type[] retVal = new Type[descendantCount + 1];
				retVal[0] = classInfo.ImplementationClass;
				for (int i = 0; i < descendantCount; ++i)
				{
					retVal[i + 1] = descendants[i].ImplementationClass;
				}
				return retVal;
			}
			return null;
		}
		private static void RegisterClosureEvaluation(Dictionary<BoundEntryRole, EvaluateCopyClosure> closureDictionary, BoundEntryRole entryRole, EvaluateCopyClosure closureEvaluation, int entryRolePlayerTypeTypeDistance, int exitRolePlayerTypeDistance, ref Dictionary<BoundEntryRole, RolePlayerTypeDistance> depthTracker)
		{
			// UNDONE: COPYMERGE Modify closure signatures to allow a higher priority closure evaluation
			// to chain to a lower priority evaluation. If no type restrictions are specified, a
			// later behavior evaluation should defer to an earlier one. This will require some chaining
			// in these data structures to hold the lower priority values. Optimize to favor the
			// single case, which will be most common.
			if (entryRolePlayerTypeTypeDistance == -1 && exitRolePlayerTypeDistance == -1)
			{
				// Distance tracking does not apply
				closureDictionary[entryRole] = closureEvaluation;
			}
			else
			{
				RolePlayerTypeDistance currentDistance = new RolePlayerTypeDistance(entryRolePlayerTypeTypeDistance, exitRolePlayerTypeDistance);
				RolePlayerTypeDistance existingDistance;
				if (depthTracker != null)
				{
					if (depthTracker.TryGetValue(entryRole, out existingDistance) &&
						currentDistance.CompareTo(existingDistance) < 0)
					{
						return;
					}
				}
				else
				{
					depthTracker = new Dictionary<BoundEntryRole, RolePlayerTypeDistance>();
				}
				depthTracker[entryRole] = currentDistance;
				closureDictionary[entryRole] = closureEvaluation;
			}
		}
		private void EnsureClosureDirectives()
		{
			Dictionary<BoundEntryRole, EvaluateCopyClosure> normalBehaviors = myNormalBehaviors;
			if (normalBehaviors == null)
			{
				myNormalBehaviors = normalBehaviors = new Dictionary<BoundEntryRole, EvaluateCopyClosure>();
				myAvailableBehaviors = new Dictionary<Guid, AvailableClosureBehaviors>();
				foreach (ICopyClosureProvider provider in Utility.GetTypedDomainModels<ICopyClosureProvider>(myStore.DomainModels))
				{
					provider.AddCopyClosureDirectives(this);
				}
				// These are used only while populating a copy closure
				myNormalDistanceTracker = null;
				myPrimaryDistanceTracker = null;
			}
		}
		#endregion // Closure Directive Population Methods
	}
	#endregion // CopyClosureManager class
}