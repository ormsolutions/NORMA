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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region IVerbalize interface
	/// <summary>
	/// An enum representing the type of verbalization
	/// for a given element
	/// </summary>
	public enum VerbalizationContent
	{
		/// <summary>
		/// Normal content
		/// </summary>
		Normal = 0,
		/// <summary>
		/// An error report
		/// </summary>
		ErrorReport = 1,
	}
    /// <summary>
    /// An enum representing the sign of the requested verbalization.
    /// </summary>
    [Flags]
    public enum VerbalizationSign
    {
        /// <summary>
        /// Verbalize with the positive form
        /// </summary>
        Positive = 1,
        /// <summary>
        /// Verbalize with the negative form
        /// </summary>
        Negative = 2,
        /// <summary>
        /// If verbalization is not available for the requested
        /// form, then attempt to verbalize with the opposite sign.
        /// </summary>
        AttemptOppositeSign = 4,
    }
	/// <summary>
	/// Options for the <see cref="IVerbalizationContext.DeferVerbalization"/> method
	/// </summary>
	[Flags]
	public enum DeferVerbalizationOptions
	{
		/// <summary>
		/// Default behavior
		/// </summary>
		None = 0,
		/// <summary>
		/// The target object may be verbalized multiple times, do not
		/// block subsequent verbalizations.
		/// </summary>
		MultipleVerbalizations = 1,
		/// <summary>
		/// Always write an additional line, unless this is the first write to the document.
		/// If this is not set and <see cref="NeverWriteLine"/> is not set, then the context
		/// setting is used.
		/// </summary>
		AlwaysWriteLine = 2,
		/// <summary>
		/// Never write an additional line. If this is not set and <see cref="AlwaysWriteLine"/>
		/// is not set, then the context setting is used.
		/// </summary>
		NeverWriteLine = 4,
	}
	/// <summary>
	/// An Interface to provide context services to <see cref="IVerbalize.GetVerbalization"/>. Allows elements
	/// being verbalized to call back to the outer verbalization engine to do a natural inline verbalization
	/// of a referenced element.
	/// </summary>
	public interface IVerbalizationContext
	{
		/// <summary>
		/// Called by implementations of <see cref="IVerbalize.GetVerbalization"/> to inform
		/// the verbalization context that it is about to begin verbalizing.
		/// This enables the host window to delay writing outer
		/// content until it knows that text is about to be written by
		/// the verbalizer to the writer
		/// </summary>
		/// <param name="content">The style of verbalization content</param>
		void BeginVerbalization(VerbalizationContent content);
		/// <summary>
		/// Defer verbalization at this point to the target object.
		/// </summary>
		/// <param name="target">
		/// Any instance that implements disjunctive mandatory combinations of 
		/// <see cref="IVerbalize"/>, <see cref="IRedirectVerbalization"/>,
		/// <see cref="IVerbalizeChildren"/>, and <see cref="IVerbalizeCustomChildren"/>
		/// </param>
		/// <param name="options">Options modifying verbalization.</param>
		/// <param name="childFilter">the filter used to remove aggregates.</param>
		void DeferVerbalization(object target, DeferVerbalizationOptions options, IVerbalizeFilterChildren childFilter);
		/// <summary>
		/// Explicitly test if an element has already been verbalized
		/// </summary>
		/// <param name="target">
		/// Any instance that implements <see cref="IVerbalize"/> or <see cref="IRedirectVerbalization"/>
		/// </param>
		/// <returns>true if the element is already verbalized</returns>
		bool AlreadyVerbalized(object target);
		/// <summary>
		/// Get the name of the verbalization target
		/// </summary>
		string VerbalizationTarget { get;}
		/// <summary>
		/// Test if an element has explicitly been marked as being
		/// verbalized within the current top-level element, and
		/// track it as verbalized if it has not been verbalized yet.
		/// </summary>
		/// <param name="target">Any tracked element</param>
		/// <returns><see langword="true"/> if an element has been marked
		/// as verbalized.</returns>
		bool TestVerbalizedLocally(object target);
	}
	/// <summary>
	/// Interface for verbalization
	/// </summary>
	public interface IVerbalize
	{
		/// <summary>
		/// Verbalize in the requested form
		/// </summary>
		/// <param name="writer">The output text writer</param>
		/// <param name="snippetsDictionary">The IVerbalizationSets to use</param>
		/// <param name="verbalizationContext">A set callback function to interact with the outer verbalization context</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <returns>true to continue with child verbalization, otherwise false</returns>
		bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign);
	}
	/// <summary>
	/// Interface to redirect verbalization. Called for top-level selected objects only
	/// </summary>
	public interface IRedirectVerbalization
	{
		/// <summary>
		/// Use the returned object as the verbalizer
		/// </summary>
		IVerbalize SurrogateVerbalizer { get;}
	}
	#endregion // IVerbalize interface
	#region IVerbalizeChildren interface
	/// <summary>
	/// Implement this interface to let the verbalization engine
	/// automatically verbalize child elements without implementing
	/// IVerbalize. IVerbalizeChildren is ignored for the top-level
	/// verbalization object if IRedirectVerbalization is specified.
	/// </summary>
	public interface IVerbalizeChildren { }
	#endregion // IVerbalizeChildren interface
	#region CustomChildVerbalizer struct
	/// <summary>
	/// Structure to hold return information from the IVerbalizeFilterChildren.FilterChildVerbalizer
	/// and IVerbalizeCustomChildren.GetCustomChildVerbalizations methods
	/// </summary>
	public struct CustomChildVerbalizer : IEquatable<CustomChildVerbalizer>
	{
		#region BlockVerbalize class
		private sealed class BlockVerbalize : IVerbalize
		{
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				Debug.Fail("Placeholder instance, should not be called");
				return false;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // BlockVerbalize class
		#region DeferVerbalizationWrapper class
		/// <summary>
		/// A wrapper class to do deferred verbalization.
		/// </summary>
		private sealed class DeferVerbalizationWrapper : IVerbalize
		{
			private IVerbalize myVerbalizeTarget;
			private readonly IVerbalizeFilterChildren myDeferFilter;
			private readonly DeferVerbalizationOptions myDeferOptions;
			private readonly bool myDisposeAfterVerbalization;
			/// <summary>
			/// Defer to the specified target
			/// </summary>
			/// <param name="verbalizeTarget">The instance to defer to</param>
			/// <param name="deferOptions">Defer options</param>
			/// <param name="childFilter">The child filter</param>
			/// <param name="disposeAfterVerbalization">Disposes the instance after verbalization</param>
			public DeferVerbalizationWrapper(IVerbalize verbalizeTarget, DeferVerbalizationOptions deferOptions, IVerbalizeFilterChildren childFilter, bool disposeAfterVerbalization)
			{
				myVerbalizeTarget = verbalizeTarget;
				myDeferOptions = deferOptions;
				myDeferFilter = childFilter;
				myDisposeAfterVerbalization = disposeAfterVerbalization;
			}
			#region IVerbalize Implementation
			bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
			{
				IVerbalize verbalize = myVerbalizeTarget;
				if (verbalize != null)
				{
					verbalizationContext.DeferVerbalization(verbalize, myDeferOptions, myDeferFilter);
					if (myDisposeAfterVerbalization)
					{
						IDisposable dispose = verbalize as IDisposable;
						if (dispose != null)
						{
							myVerbalizeTarget = null;
							dispose.Dispose();
						}
					}
				}
				return false;
			}
			#endregion // IVerbalize Implementation
		}
		#endregion // DeferVerbalizationWrapper class
		#region Member Variables
		private readonly IVerbalize myInstance;
		private readonly bool myDisposeAfterVerbalization;
		#endregion // Member Variables
		#region Private constructors
		/// <summary>
		/// Create an VerbalizationFilterResult structure
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		/// <param name="disposeAfterVerbalization">Indicate if the instance should be
		/// disposed after verbalization is complete.</param>
		private CustomChildVerbalizer(IVerbalize instance, bool disposeAfterVerbalization)
		{
			myInstance = instance;
			myDisposeAfterVerbalization = disposeAfterVerbalization;
		}
		/// <summary>
		/// Create an VerbalizationFilterResult structure for deferred verbalization
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		/// <param name="deferOptions">Defer options</param>
		/// <param name="childFilter">The filter to apply to child verbalization</param>
		/// <param name="disposeAfterVerbalization">Indicate if the instance should be
		/// disposed after verbalization is complete.</param>
		private CustomChildVerbalizer(IVerbalize instance, DeferVerbalizationOptions deferOptions, IVerbalizeFilterChildren childFilter, bool disposeAfterVerbalization)
		{
			myInstance = new DeferVerbalizationWrapper(instance, deferOptions, childFilter, disposeAfterVerbalization);
			myDisposeAfterVerbalization = false;
		}
		#endregion // Private constructors
		#region Public Members
		/// <summary>
		/// Any empty VerbalizationFilterResult structure
		/// </summary>
		public readonly static CustomChildVerbalizer Empty = default(CustomChildVerbalizer);
		/// <summary>
		/// A verbalization structure that explicitly blocks the element from verbalizing
		/// </summary>
		public readonly static CustomChildVerbalizer Block = new CustomChildVerbalizer(new BlockVerbalize(), false);
		/// <summary>
		/// Test if the structure is empty
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				// Access the property here to deal with the Block value
				return Instance == null;
			}
		}
		/// <summary>
		/// Test if the structure is blocked. Note that <see cref="IsEmpty"/> will always
		/// be true if IsBlocked is true.
		/// </summary>
		public bool IsBlocked
		{
			get
			{
				return myInstance == Block.myInstance;
			}
		}
		/// <summary>
		/// Verbalize a single instance and do not try to verbalize child instances.
		/// Dose not dispose the instance after verbalization.
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		public static CustomChildVerbalizer VerbalizeInstance(IVerbalize instance)
		{
			return new CustomChildVerbalizer(instance, false);
		}
		/// <summary>
		/// Verbalize a single instance and do not try to verbalize child instances.
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		/// <param name="disposeAfterVerbalization">Indicate if the instance should be
		/// disposed after verbalization is complete.</param>
		public static CustomChildVerbalizer VerbalizeInstance(IVerbalize instance, bool disposeAfterVerbalization)
		{
			return new CustomChildVerbalizer(instance, disposeAfterVerbalization);
		}
		/// <summary>
		/// Verbalize an instance and its child instances using <see cref="IVerbalizationContext.DeferVerbalization"/>
		/// Does not dispose the instance after verbalization.
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		/// <param name="deferOptions">Defer options</param>
		/// <param name="childFilter">The filter to apply to child verbalization</param>
		public static CustomChildVerbalizer VerbalizeInstanceWithChildren(IVerbalize instance, DeferVerbalizationOptions deferOptions, IVerbalizeFilterChildren childFilter)
		{
			return new CustomChildVerbalizer(instance, deferOptions, childFilter, false);
		}
		/// <summary>
		/// Verbalize an instance and its child instances using <see cref="IVerbalizationContext.DeferVerbalization"/>
		/// </summary>
		/// <param name="instance">The child instance to verbalize</param>
		/// <param name="deferOptions">Defer options</param>
		/// <param name="childFilter">The filter to apply to child verbalization</param>
		/// <param name="disposeAfterVerbalization">Indicate if the instance should be
		/// disposed after verbalization is complete.</param>
		public static CustomChildVerbalizer VerbalizeInstanceWithChildren(IVerbalize instance, DeferVerbalizationOptions deferOptions, IVerbalizeFilterChildren childFilter, bool disposeAfterVerbalization)
		{
			return new CustomChildVerbalizer(instance, deferOptions, childFilter, disposeAfterVerbalization);
		}
		/// <summary>
		/// The instance.
		/// </summary>
		public IVerbalize Instance
		{
			get
			{
				IVerbalize retVal = myInstance;
				// Careful, access field on Block directly
				return (retVal == Block.myInstance) ? null : retVal;
			}
		}
		/// <summary>
		/// Options for using the instance
		/// </summary>
		public bool DisposeAfterVerbalization
		{
			get
			{
				return myDisposeAfterVerbalization;
			}
		}
		#endregion // Public members
		#region Equality and casting routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is CustomChildVerbalizer) && Equals((CustomChildVerbalizer)obj);
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			IVerbalize instance = myInstance;
			if (instance != null)
			{
				return ORMSolutions.ORMArchitect.Framework.Utility.GetCombinedHashCode(instance.GetHashCode(), myDisposeAfterVerbalization.GetHashCode());
			}
			return 0;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(CustomChildVerbalizer obj)
		{
			return myInstance == obj.myInstance && myDisposeAfterVerbalization == obj.myDisposeAfterVerbalization;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(CustomChildVerbalizer left, CustomChildVerbalizer right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(CustomChildVerbalizer left, CustomChildVerbalizer right)
		{
			return !left.Equals(right);
		}
		#endregion // Equality and casting routines
	}
	#endregion // CustomChildVerbalizer struct
	#region IVerbalizeCustomChildren interface
	/// <summary>
	/// Implement to verbalize children that are not naturally aggregated
	/// </summary>
	public interface IVerbalizeCustomChildren
	{
		/// <summary>
		/// Retrieve children to verbalize that are not part of the standard
		/// verbalization.
		/// </summary>
		/// <param name="filter">A <see cref="IVerbalizeFilterChildren"/> instance. Can be <see langword="null"/>.
		/// If the <see cref="IVerbalizeFilterChildren.FilterChildVerbalizer">FilterChildVerbalizer</see> method returns
		/// <see cref="CustomChildVerbalizer.Block"/> for any constituent components used to create a <see cref="CustomChildVerbalizer"/>,
		/// then that custom child should not be created</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <returns>IEnumerable of CustomChildVerbalizer structures</returns>
		IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, VerbalizationSign sign);
	}
	#endregion // IVerbalizeCustomChildren interface
	#region IVerbalizeExtensionChildren interface
	/// <summary>
	/// Interface to allow extension elements to add child verbalizations
	/// to any element.
	/// </summary>
	public interface IVerbalizeExtensionChildren
	{
		/// <summary>
		/// Retrieve children to verbalize that are provided by
		/// extension elements.
		/// </summary>
		/// <param name="parentElement">The parent element being verbalized.</param>
		/// <param name="filter">A <see cref="IVerbalizeFilterChildren"/> instance. Can be <see langword="null"/>.
		/// If the <see cref="IVerbalizeFilterChildren.FilterChildVerbalizer">FilterChildVerbalizer</see> method returns
		/// <see cref="CustomChildVerbalizer.Block"/> for any constituent components used to create a <see cref="CustomChildVerbalizer"/>,
		/// then that custom child should not be created</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <returns>IEnumerable of CustomChildVerbalizer structures</returns>
		IEnumerable<CustomChildVerbalizer> GetExtensionChildVerbalizations(object parentElement, IVerbalizeFilterChildren filter, VerbalizationSign sign);
	}
	#endregion // IVerbalizeExtensionChildren interface
	#region IExtensionVerbalizerService interface
	/// <summary>
	/// Standard mechanism to register and unregister extension child
	/// verbalizers for specific types.
	/// </summary>
	public interface IExtensionVerbalizerService
	{
		/// <summary>
		/// Registers or unregisters an <see cref="IVerbalizeExtensionChildren"/> specified by
		/// <paramref name="extensionVerbalizer"/> for the type specified by <paramref name="verbalizedElementType"/>.
		/// </summary>
		/// <param name="verbalizedElementType">The type for which the <paramref name="extensionVerbalizer"/> should be called.</param>
		/// <param name="extensionVerbalizer">The <see cref="IVerbalizeExtensionChildren"/> being registered.</param>
		/// <param name="includeSubtypes">Specifies whether the <paramref name="extensionVerbalizer"/> should also be registered for subtypes of <paramref name="verbalizedElementType"/>. Supported only for <see cref="ModelElement"/>-derived types.</param>
		/// <param name="action">Specifies whether the property provider is being added or removed. See <see cref="EventHandlerAction"/></param>
		void AddOrRemoveExtensionVerbalizer(Type verbalizedElementType, IVerbalizeExtensionChildren extensionVerbalizer, bool includeSubtypes, EventHandlerAction action);

		/// <summary>
		/// Get an <see cref="IVerbalizeExtensionChildren"/> instance for the
		/// specified <paramref name="verbalizedElement"/>
		/// </summary>
		/// <param name="verbalizedElement">An element being verbalized.</param>
		/// <returns>Child verbalizer, or <see langword="null"/></returns>
		IVerbalizeExtensionChildren GetExtensionVerbalizer(object verbalizedElement);
	}
	#endregion // IExtensionVerbalizerService interface
	#region IVerbalizeFilterChildren interface
	/// <summary>
	/// Implement to remove or provide an alternate verbalization for
	/// aggregated children that are naturally verbalized.
	/// </summary>
	public interface IVerbalizeFilterChildren
	{
		/// <summary>
		/// Provides an opportunity for a parent object to filter the
		/// verbalization of aggregated child verbalization implementations
		/// </summary>
		/// <param name="child">A direct or indirect child object.</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <returns>Return the provided childVerbalizer to verbalize normally, null to block verbalization, or an
		/// alternate IVerbalize. The value is returned with a boolean option. The element will be disposed with
		/// this is true.</returns>
		CustomChildVerbalizer FilterChildVerbalizer(object child, VerbalizationSign sign);
	}
	#endregion // IVerbalizeFilterChildren interface
	#region IVerbalizeFilterChildrenByRole
	/// <summary>
	/// Implement to block verbalization of embedded child
	/// elements based on the relationship to the child element.
	/// This runs before an element is offered to <see cref="IVerbalizeFilterChildren"/>
	/// and is used as an optimization to aid verbalization of
	/// high-level container elements.
	/// </summary>
	public interface IVerbalizeFilterChildrenByRole
	{
		/// <summary>
		/// Test if aggregated elements should be blocked from being
		/// verbalized based on the type of child relationship.
		/// </summary>
		/// <param name="embeddingRole">A role on this object that is
		/// the container side of an embedding relationship</param>
		/// <returns><see langword="true"/> to block verbalization</returns>
		bool BlockEmbeddedVerbalization(DomainRoleInfo embeddingRole);
	}
	#endregion // IVerbalizeFilterChildrenByRole
	#region IVerbalizationSets interface
	/// <summary>A base interface for the generic VerbalizationSets interface.</summary>
	public interface IVerbalizationSets
	{
	}
	#endregion // IVerbalizationSets interface
	#region IVerbalizationSets interface
	/// <summary>An interface representing generic verbalization sets.</summary>
	/// <typeParam name="TEnum">An enumeration representing the verbalization sets</typeParam>
	public interface IVerbalizationSets<TEnum> : IVerbalizationSets
		where TEnum : struct
	{
		/// <summary>Retrieve a snippet for the specified type and criteria.</summary>
		/// <param name="snippetType">A value from the TEnum enum.</param>
		/// <param name="isDeontic">Set to true to retrieve the snippet for a deontic verbalization, false for alethic.</param>
		/// <param name="isNegative">Set to true to retrieve the snippet for a negative reading, false for positive.</param>
		/// <returns>Snippet string</returns>
		string GetSnippet(TEnum snippetType, bool isDeontic, bool isNegative);
		/// <summary>Retrieve a snippet for the specified type with default criteria.</summary>
		/// <param name="snippetType">A value from the TEnum enum.</param>
		/// <returns>Snippet string</returns>
		string GetSnippet(TEnum snippetType);
	}
	#endregion // Genereic IVerbalizationSets interface
	#region Generic VerbalizationSets class
	/// <summary>A generic class containing one VerbalizationSet structure for each combination of {alethic,deontic} and {positive,negative} snippets.</summary>
	/// <typeparam name="TEnum">The enumeration type of snippet set</typeparam>
	public abstract class VerbalizationSets<TEnum> : IVerbalizationSets<TEnum>
		where TEnum : struct
	{
		#region VerbalizationSet class
		/// <summary>An abstract class holding an array of strings. Strings are retrieved with values from CoreVerbalizationSnippetType.</summary>
		protected abstract class VerbalizationSet
		{
			/// <summary>Retrieve a snippet value</summary>
			/// <param name="snippetType">A value from the CoreVerbalizationSnippetType enum representing the snippet string to retrieve.</param>
			/// <param name="owner">The VerbalizationSets object that is the owner of the snippet sets.</param>
			/// <returns>Snippet string</returns>
			public abstract string GetSnippet(TEnum snippetType, VerbalizationSets<TEnum> owner);
		}
		#endregion // VerbalizationSet class
		#region ArrayVerbalizationSet class
		/// <summary>A class holding an array of strings. Strings are retrieved with values from CoreVerbalizationSnippetType.</summary>
		protected class ArrayVerbalizationSet : VerbalizationSet
		{
			private string[] mySnippets;
			/// <summary>VerbalizationSet constructor.</summary>
			/// <param name="snippets">An array of strings with one string for each value in the CoreVerbalizationSnippetType enum.</param>
			public ArrayVerbalizationSet(string[] snippets)
			{
				this.mySnippets = snippets;
			}
			/// <summary>Retrieve a snippet value</summary>
			/// <param name="snippetType">A value from the CoreVerbalizationSnippetType enum representing the snippet string to retrieve.</param>
			/// <param name="owner">The VerbalizationSets object that is the owner of the snippet sets.</param>
			/// <returns>Snippet string</returns>
			public override string GetSnippet(TEnum snippetType, VerbalizationSets<TEnum> owner)
			{
				return this.mySnippets[owner.ValueToIndex(snippetType)];
			}
		}
		#endregion // ArrayVerbalizationSet class
		#region DictionaryVerbalizationSet class
		/// <summary>A class holding dictionary items that refer to values from the enumeration of CoreVerbalizationSnippetType.</summary>
		protected class DictionaryVerbalizationSet : VerbalizationSet
		{
			private Dictionary<TEnum, string> mySnippets;
			/// <summary>Retrieves all of the IDictionary snippets in the snippet set</summary>
			public IDictionary<TEnum, string> Dictionary
			{
				get
				{
					return mySnippets;
				}
			}
			/// <summary>VerbalizationSet constructor.</summary>
			public DictionaryVerbalizationSet()
			{
				this.mySnippets = new Dictionary<TEnum, string>();
			}
			/// <summary>Retrieve a snippet value</summary>
			/// <param name="snippetType">A value from the CoreVerbalizationSnippetType enum representing the snippet string to retrieve.</param>
			/// <param name="owner">The VerbalizationSets object that is the owner of the snippet sets.</param>
			/// <returns>Snippet string</returns>
			public override string GetSnippet(TEnum snippetType, VerbalizationSets<TEnum> owner)
			{
				string retVal = null;
				this.mySnippets.TryGetValue(snippetType, out retVal);
				return retVal;
			}
		}
		#endregion // DictionaryVerbalizationSet class
		#region IVerbalizationSets<TEnum> Implementation
		private VerbalizationSet[] mySets;
		/// <summary>Retrieve a snippet for the specified type with default criteria.</summary>
		/// <param name="snippetType">A value from the CoreVerbalizationSnippetType enum representing the snippet string to retrieve.</param>
		/// <returns>Snippet string</returns>
		protected string GetSnippet(TEnum snippetType)
		{
			return this.GetSnippet(snippetType, false, false);
		}
		string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType)
		{
			return this.GetSnippet(snippetType);
		}
		/// <summary>Retrieve a snippet for the specified type and criteria.</summary>
		/// <param name="snippetType">A value from the CoreVerbalizationSnippetType enum representing the snippet string to retrieve.</param>
		/// <param name="isDeontic">Set to true to retrieve the snippet for a deontic verbalization, false for alethic.</param>
		/// <param name="isNegative">Set to true to retrieve the snippet for a negative reading, false for positive.</param>
		/// <returns>Snippet string</returns>
		protected string GetSnippet(TEnum snippetType, bool isDeontic, bool isNegative)
		{
			VerbalizationSet set = this.mySets[VerbalizationSets<TEnum>.GetSetIndex(isDeontic, isNegative)];
			if (set != null)
			{
				return set.GetSnippet(snippetType, this);
			}
			else
			{
				return null;
			}
		}
		string IVerbalizationSets<TEnum>.GetSnippet(TEnum snippetType, bool isDeontic, bool isNegative)
		{
			return this.GetSnippet(snippetType, isDeontic, isNegative);
		}
		#endregion // IVerbalizationSets<TEnum> Implementation
		#region VerbalizationSets Specific
		/// <summary>Get the snippet index of the deontic/negative VerbalizationSet</summary>
		/// <param name="isDeontic">Set to true to retrieve the snippet for a deontic verbalization, false for alethic.</param>
		/// <param name="isNegative">Set to true to retrieve the snippet for a negative reading, false for positive.</param>
		/// <returns>0-based index</returns>
		protected static int GetSetIndex(bool isDeontic, bool isNegative)
		{
			int setIndex = 0;
			if (isDeontic)
			{
				setIndex = setIndex + 1;
			}
			if (isNegative)
			{
				setIndex = setIndex + 2;
			}
			return setIndex;
		}
		/// <summary>Method to populate verbalization sets of an abstract VerbalizationSets object.</summary>
		/// <param name="sets">The empty verbalization sets to be populated</param>
		/// <param name="userData">User-defined data passed to the Create method</param>
		protected abstract void PopulateVerbalizationSets(VerbalizationSet[] sets, object userData);
		/// <summary>Method to convert enum value to integer index value</summary>
		/// <param name="enumValue">The enum value to be converted</param>
		/// <returns>integer value of enum type</returns>
		protected abstract int ValueToIndex(TEnum enumValue);
		/// <summary>Creates an instance of the VerbalizationSets class and calls the PopulateVerbalizationSets method.</summary>
		/// <typeparam name="DerivedType">Name of class to instantiate that derives from VerbalizationSets.</typeparam>
		/// <param name="userPopulationData">User-defined data passed forward to PopulateVerbalizationSets</param>
		/// <returns>Returns a generic VerbalizationSetsobject with snippet sets</returns>
		public static VerbalizationSets<TEnum> Create<DerivedType>(object userPopulationData)
			where DerivedType : VerbalizationSets<TEnum>, new()
		{
			VerbalizationSets<TEnum> retVal = new DerivedType();
			Initialize(retVal, userPopulationData);
			return retVal;
		}
		/// <summary>Initializes an instance of the VerbalizationSets class and calls the PopulateVerbalizationSets method.</summary>
		/// <param name="target">The newly created object to populate.</param>
		/// <param name="userPopulationData">User-defined data passed forward to PopulateVerbalizationSets</param>
		/// <returns>Returns a generic VerbalizationSets object with snippet sets</returns>
		public static void Initialize(VerbalizationSets<TEnum> target, object userPopulationData)
		{
			VerbalizationSet[] newSets = new VerbalizationSet[4];
			target.PopulateVerbalizationSets(newSets, userPopulationData);
			target.mySets = newSets;
		}
		#endregion // VerbalizationSets Specific
	}
	#endregion // Generic VerbalizationSets class
	#region Static verbalization helpers on FactType class
	/// <summary>
	/// Options for <see cref="FactType.GetMatchingReading"/>
	/// </summary>
	public enum MatchingReadingOptions
	{
		/// <summary>
		/// Use default settings
		/// </summary>
		None = 0,
		/// <summary>
		/// Invert the roles represented by the matchLeadRole and matchAnyLeadRole arguments.
		/// </summary>
		InvertLeadRoles = 1,
		/// <summary>
		/// Find a reading with no front text
		/// </summary>
		NoFrontText = 2,
		/// <summary>
		/// Find a reading with no hyphen binding
		/// </summary>
		NotHyphenBound = 4,
		/// <summary>
		/// Find a reading where no lead roles are hyphen bound
		/// </summary>
		LeadRolesNotHyphenBound = 8,
		/// <summary>
		/// Attempt to match the lead order, but do not return
		/// null if some matching reading is available.
		/// </summary>
		AllowAnyOrder = 0x10,
		/// <summary>
		/// Find a reading with no trailing text
		/// </summary>
		NoTrailingText = 0x20,
	}
	/// <summary>
	/// A callback for the <see cref="FactType.PopulatePredicateText(IReading,IFormatProvider,String,RoleReplacementProvider)"/>
	/// and <see cref="VerbalizationHyphenBinder.PopulatePredicateText(IReading,IFormatProvider,String,IList{RoleBase},RoleReplacementProvider)"/> methods.
	/// </summary>
	/// <param name="roleBase">The <see cref="RoleBase"/> in the fact type to get a replacement field for.</param>
	/// <param name="hyphenBindingFormatString">The hyphen bound format string for the replacement role. If this
	/// is provided, then there is a single replacement field for the role player, and any additional quantification
	/// should treat the hyphen-bound text as a single unit.</param>
	/// <returns>Replacement string</returns>
	public delegate string RoleReplacementProvider(RoleBase roleBase, string hyphenBindingFormatString);
	public partial class FactType
	{
		/// <summary>
		/// Helper function to get a matching reading order. The match priority is
		/// specified by the parameter order
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="ignoreReadingOrder">Ignore this reading order in the readingOrders collection</param>
		/// <param name="matchLeadRole">Choose any order that begins with this role. If defaultRoleOrder is also
		/// set and starts with this role and the order is defined, then use it.</param>
		/// <param name="matchAnyLeadRole">Same as matchLeadRole, except with a set match. An IList of RoleBase elements.</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="readingOptions"><see cref="MatchingReadingOptions"/> to control the returned reading.</param>
		/// <returns>A matching <see cref="IReading"/> instance. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public IReading GetMatchingReading(LinkedElementCollection<ReadingOrder> readingOrders, ReadingOrder ignoreReadingOrder, RoleBase matchLeadRole, IList matchAnyLeadRole, IList<RoleBase> defaultRoleOrder, MatchingReadingOptions readingOptions)
		{
			bool invertLeadRoles = 0 != (readingOptions & MatchingReadingOptions.InvertLeadRoles);
			bool noFrontText = 0 != (readingOptions & MatchingReadingOptions.NoFrontText);
			bool noTrailingText = 0 != (readingOptions & MatchingReadingOptions.NoTrailingText);
			bool notHyphenBound = 0 != (readingOptions & MatchingReadingOptions.NotHyphenBound);
			bool leadNotHyphenBound = !notHyphenBound && 0 != (readingOptions & MatchingReadingOptions.LeadRolesNotHyphenBound);
			bool allowAnyOrder = 0 != (readingOptions & MatchingReadingOptions.AllowAnyOrder);
			int orderCount = readingOrders.Count;
			IReading retVal = null;
			Reading readingMatch = null;
			bool blockTestDefault = false; // If we have specific lead role requirements, then default is only used to enforce them, or as the default for any allowed order
			bool useImplicitReadings = HasImplicitReadings;
			if (orderCount != 0 || useImplicitReadings)
			{
				int ignoreReadingOrderIndex = (ignoreReadingOrder == null) ? -1 : readingOrders.IndexOf(ignoreReadingOrder);
				if (ignoreReadingOrderIndex != -1 && orderCount == 1)
				{
					return null;
				}

				// Match a single lead role, prefer the default order
				if (matchLeadRole != null)
				{
					if (invertLeadRoles)
					{
						int matchAllCount = defaultRoleOrder.Count;
						for (int i = 0; i < matchAllCount; ++i)
						{
							RoleBase currentRole = defaultRoleOrder[i];
							if (currentRole != matchLeadRole)
							{
								if (useImplicitReadings)
								{
									retVal = GetImplicitReading(currentRole);
									if (retVal != null)
									{
										break;
									}
								}
								else if (GetMatchingReadingInternal(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, noTrailingText, notHyphenBound, leadNotHyphenBound, !allowAnyOrder, ref readingMatch))
								{
									retVal = readingMatch;
									break;
								}
							}
						}
					}
					else if (useImplicitReadings)
					{
						retVal = GetImplicitReading(matchLeadRole);
					}
					else if (GetMatchingReadingInternal(readingOrders, ignoreReadingOrderIndex, matchLeadRole, defaultRoleOrder, noFrontText, noTrailingText, notHyphenBound, leadNotHyphenBound, !allowAnyOrder, ref readingMatch))
					{
						retVal = readingMatch;
					}
					if (retVal == null && matchAnyLeadRole == null)
					{
						blockTestDefault = !allowAnyOrder;
					}
				}

				if (retVal == null && matchAnyLeadRole != null)
				{
					int matchAnyCount = matchAnyLeadRole.Count;
					if (invertLeadRoles)
					{
						int matchAllCount = defaultRoleOrder.Count;
						if (matchAllCount > matchAnyCount)
						{
							for (int i = 0; i < matchAllCount; ++i)
							{
								RoleBase currentRole = defaultRoleOrder[i];
								if (!matchAnyLeadRole.Contains(currentRole.Role))
								{
									if (useImplicitReadings)
									{
										retVal = GetImplicitReading(currentRole);
										if (retVal != null)
										{
											break;
										}
									}
									else if (GetMatchingReadingInternal(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, noTrailingText, notHyphenBound, leadNotHyphenBound, !allowAnyOrder, ref readingMatch))
									{
										retVal = readingMatch;
										break;
									}
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < matchAnyCount; ++i)
						{
							if (useImplicitReadings)
							{
								retVal = GetImplicitReading((RoleBase)matchAnyLeadRole[i]);
								if (retVal != null)
								{
									break;
								}
							}
							else if (GetMatchingReadingInternal(readingOrders, ignoreReadingOrderIndex, (RoleBase)matchAnyLeadRole[i], defaultRoleOrder, noFrontText, noTrailingText, notHyphenBound, leadNotHyphenBound, !allowAnyOrder, ref readingMatch))
							{
								retVal = readingMatch;
								break;
							}
						}
					}
					if (retVal == null)
					{
						blockTestDefault = !allowAnyOrder;
					}
				}

				if (retVal == null && readingMatch != null)
				{
					retVal = readingMatch;
				}
				else if (retVal == null && defaultRoleOrder != null && !blockTestDefault)
				{
					if (useImplicitReadings)
					{
						retVal = GetImplicitReading(defaultRoleOrder[0]);
					}
					else
					{
						for (int i = 0; i < orderCount; ++i)
						{
							if (i == ignoreReadingOrderIndex)
							{
								continue;
							}
							ReadingOrder testOrder = readingOrders[i];
							LinkedElementCollection<RoleBase> testRoles = testOrder.RoleCollection;
							int testRolesCount = testRoles.Count;
							int j;
							for (j = 0; j < testRolesCount; ++j)
							{
								if (testRoles[j] != defaultRoleOrder[j])
								{
									break;
								}
							}
							if (j == testRolesCount)
							{
								retVal = testOrder.PrimaryReading;
								break;
							}
						}
					}
				}

				if (retVal == null && allowAnyOrder && !useImplicitReadings)
				{
					retVal = readingOrders[(ignoreReadingOrderIndex == 0) ? 1 : 0].PrimaryReading;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper function for GetMatchingReading
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="ignoreReadingOrderIndex">Ignore the reading order at this index</param>
		/// <param name="matchLeadRole">The role to match as a lead</param>
		/// <param name="defaultRoleOrder">The default role order. If not specified, any match will be considered optimal</param>
		/// <param name="testNoFrontText">Test for no front text if true.</param>
		/// <param name="testNoTrailingText">Test for no trailing text if true.</param>
		/// <param name="notHyphenBound">Do not return a reading with hyphen-bound text.</param>
		/// <param name="leadNotHyphenBound">Do not return a reading with hyphen-bound text on a lead role.</param>
		/// <param name="strictMatch">Ignored if testNoFrontText, testNoTrailingText, notHyphenBound, and leadNotHyphenBound are all false. Otherwise, do not set matchingReading if frontText, trailing text, and hyphen binding requirements not satisfied</param>
		/// <param name="matchingReading">The matching reading. Can be non-null to start with</param>
		/// <returns>true if an optimal match was found. retVal will be false if a match is found but
		/// a more optimal match is possible</returns>
		private static bool GetMatchingReadingInternal(LinkedElementCollection<ReadingOrder> readingOrders, int ignoreReadingOrderIndex, RoleBase matchLeadRole, IList<RoleBase> defaultRoleOrder, bool testNoFrontText, bool testNoTrailingText, bool notHyphenBound, bool leadNotHyphenBound, bool strictMatch, ref Reading matchingReading)
		{
			ReadingOrder matchingOrder = null;
			int matchingRoleCount = 0;
			int orderCount = readingOrders.Count;
			ReadingOrder testOrder;
			bool optimalMatch = false;
			LinkedElementCollection<RoleBase> testRoles;
			int testRoleCount;
			if (orderCount != 0)
			{
				if (matchLeadRole != null)
				{
					for (int i = 0; i < orderCount; ++i)
					{
						if (i == ignoreReadingOrderIndex)
						{
							continue;
						}
						testOrder = readingOrders[i];
						testRoles = testOrder.RoleCollection;
						testRoleCount = testRoles.Count;
						if (testRoleCount != 0 && testRoles[0] == matchLeadRole)
						{
							if (defaultRoleOrder != null)
							{
								int j;
								for (j = 0; j < testRoleCount; ++j)
								{
									if (testRoles[j] != defaultRoleOrder[j])
									{
										break;
									}
								}
								if (j == testRoleCount)
								{
									matchingOrder = testOrder;
									optimalMatch = true;
									break;
								}
								if (matchingOrder == null)
								{
									matchingOrder = testOrder; // Remember the first one
									matchingRoleCount = testRoleCount;
								}
							}
							else
							{
								matchingOrder = testOrder;
								matchingRoleCount = testRoleCount;
								optimalMatch = true;
								break;
							}
						}
					}
				}
			}
			if (matchingOrder != null)
			{
				if (!(testNoFrontText || testNoTrailingText || notHyphenBound || leadNotHyphenBound))
				{
					matchingReading = matchingOrder.PrimaryReading;
				}
				else
				{
					LinkedElementCollection<Reading> readings = matchingOrder.ReadingCollection;
					Reading strictReadingMatch = null;
					int readingCount = readings.Count;
					string trailingFormatText = null;
					for (int i = 0; i < readingCount; ++i)
					{
						Reading testReading = readings[i];
						if (notHyphenBound && VerbalizationHyphenBinder.IsHyphenBound(testReading))
						{
							continue;
						}
						else
						{
							string readingText = testReading.Text;
							if ((!testNoFrontText || readingText.StartsWith("{0}", StringComparison.Ordinal)) &&
								(!testNoTrailingText || readingText.StartsWith(trailingFormatText ?? (trailingFormatText = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", matchingRoleCount - 1)), StringComparison.Ordinal)) &&
								(!leadNotHyphenBound || i != 0 || !VerbalizationHyphenBinder.IsHyphenBound(readingText, 0)))
							{
								strictReadingMatch = testReading;
								break;
							}
						}
					}
					if (strictReadingMatch != null)
					{
						matchingReading = strictReadingMatch;
					}
					else if (strictMatch)
					{
						optimalMatch = false;
					}
					else
					{
						matchingReading = readings[0];
						optimalMatch = false;
					}
				}
			}
			return optimalMatch;
		}
		/// <summary>
		/// Helper function to reliably return the index of a role in a fact.
		/// </summary>
		/// <param name="factRoles"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		/// <remarks>The role collection of a FactType is a RoleBase collection, but
		/// all constraint role collections are made up of Role. Without overriding
		/// the equality operator (Equals method, etc) (undesirable because we often
		/// do need to know the difference), this means that factRoles.IndexOf(role)
		/// will return a false negative, so we write our own helper function.</remarks>
		public static int IndexOfRole(IList<RoleBase> factRoles, RoleBase role)
		{
			Role testRole = role as Role;
			if (testRole == null)
			{
				return factRoles.IndexOf(role);
			}
			int roleCount = factRoles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				if (factRoles[i].Role == testRole)
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Populate the predicate text with the supplied replacement fields.
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
		/// <param name="roleReplacements">The replacement fields. The length of the replacement array can be greater than
		/// the number of roles in the defaultOrder collection</param>
		/// <returns>The populated predicate text</returns>
		public static string PopulatePredicateText(IReading reading, IFormatProvider formatProvider, string predicatePartDecorator, IList<RoleBase> defaultOrder, string[] roleReplacements)
		{
			string retVal = null;
			if (reading != null)
			{
				IList<RoleBase> readingRoles = reading.RoleCollection;
				int readingRoleCount = readingRoles.Count;
				Exception exceptionToFormat = null;
				try
				{
					retVal = Reading.ReplaceFields(
						reading.Text,
						formatProvider,
						predicatePartDecorator,
						readingRoleCount == 1 ?
							(ReadingTextFieldReplace)delegate(int fieldIndex)
							{
								// Single role reading for a unary FactType. The roleReplacement will always have a single
								// element. However, the unary role can come second in the default order. The safest approach
								// is to simple ignore defaultOrder in this case
								return roleReplacements[0];
							} :
							delegate(int fieldIndex)
							{
								RoleBase readingRole = readingRoles[fieldIndex];
								if (readingRole != defaultOrder[fieldIndex])
								{
									return roleReplacements[defaultOrder.IndexOf(readingRole)];
								}
								return roleReplacements[fieldIndex];
							});
				}
				catch (FormatException ex)
				{
					exceptionToFormat = ex;
				}
				catch (IndexOutOfRangeException ex)
				{
					exceptionToFormat = ex;
				}
				if (null != exceptionToFormat)
				{
					// UNDONE: Localize
					retVal = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", reading.Text, exceptionToFormat.Message);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Populate the predicate text using a <see cref="RoleReplacementProvider"/> callback
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="replacementProvider">A callback for generating role replacement text.</param>
		/// <returns>The populated predicate text</returns>
		public static string PopulatePredicateText(IReading reading, IFormatProvider formatProvider, string predicatePartDecorator, RoleReplacementProvider replacementProvider)
		{
			string retVal = null;
			if (reading != null)
			{
				IList<RoleBase> readingRoles = reading.RoleCollection;
				int readingRoleCount = readingRoles.Count;
				Exception exceptionToFormat = null;
				try
				{
					retVal = Reading.ReplaceFields(
						reading.Text,
						formatProvider,
						predicatePartDecorator,
						delegate(int fieldIndex)
						{
							return replacementProvider(readingRoles[fieldIndex], null);
						});
				}
				catch (FormatException ex)
				{
					exceptionToFormat = ex;
				}
				catch (IndexOutOfRangeException ex)
				{
					exceptionToFormat = ex;
				}
				if (null != exceptionToFormat)
				{
					// UNDONE: Localize
					retVal = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", reading.Text, exceptionToFormat.Message);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Match the first non whitespace/html character
		/// </summary>
		private static readonly Regex FirstBodyCharacterPatternAny = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>[^<\s])", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the first non whitespace/html character, but only if it is lower case
		/// </summary>
		private static readonly Regex FirstBodyCharacterPatternLower = new Regex(@"^(?:((<[^>]*?>)|\s)*?)(?<1>\p{Ll})", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Match the last non whitespace/html character
		/// </summary>
		private static readonly Regex LastBodyCharacterPattern = new Regex(@"(?<1>[^<\s])((<[^>]*?>)|\s)*?\z", RegexOptions.Compiled | RegexOptions.Singleline);
		/// <summary>
		/// Helper function for turning verbalizations into true sentences. Handles html and plain text
		/// body text.
		/// </summary>
		public static void WriteVerbalizerSentence(TextWriter writer, string body, string closeSentenceWith)
		{
			if (string.IsNullOrEmpty(body))
			{
				return;
			}
			Match match = FirstBodyCharacterPatternLower.Match(body);
			if (match.Success)
			{
				Group group = match.Groups[1];
				if (group.Success)
				{
					int charIndex = group.Index;
					if (charIndex != 0)
					{
						writer.Write(body.Substring(0, charIndex));
					}
					writer.Write(char.ToUpper(body[charIndex], CultureInfo.CurrentCulture));
					string trailingText = body.Substring(charIndex + 1);
					if (closeSentenceWith.Length != 0 && CloseSentence(writer, trailingText, closeSentenceWith))
					{
						return;
					}
					writer.Write(trailingText);
					return;
				}
			}
			else if (closeSentenceWith.Length != 0 && CloseSentence(writer, body, closeSentenceWith))
			{
				return;
			}
			writer.Write(body);
		}
		/// <summary>
		/// Create a verbalizer sentence, returned as a string.
		/// </summary>
		public static string CreateVerbalizerSentence(string body, string closeSentenceWith)
		{
			StringWriter writer = new StringWriter();
			WriteVerbalizerSentence(writer, body, closeSentenceWith);
			return writer.ToString();
		}
		private static bool CloseSentence(TextWriter writer, string body, string closeSentenceWith)
		{
			// Note that the closeSentenceWith value must go inside any html tags with the last text
			// because these contain indentation styles which may be cleared, causing the sentence closure
			// to write in the wrong location
			Match match = LastBodyCharacterPattern.Match(body);
			if (match.Success)
			{
				string modifiedClose = closeSentenceWith;
				// We need to strip any html tags from around the sentence closure and compare the
				// contents, not the whole string
				int replaceLength = closeSentenceWith.Length;
				int modifiedReplaceLength = replaceLength;
				if (replaceLength > 1)
				{
					// UNDONE: Cache the last closure string, we'll be getting the same query every time
					Match closeStartMatch = FirstBodyCharacterPatternAny.Match(closeSentenceWith);
					if (closeStartMatch.Success)
					{
						Group closeStartGroup = closeStartMatch.Groups[1];
						if (closeStartGroup.Success)
						{
							Match closeLastMatch = LastBodyCharacterPattern.Match(closeSentenceWith);
							if (closeLastMatch.Success)
							{
								int startIndex = closeStartGroup.Index;
								int endIndex = closeLastMatch.Index;
								if (startIndex <= endIndex)
								{
									modifiedClose = closeSentenceWith.Substring(startIndex, endIndex - startIndex + 1);
									modifiedReplaceLength = modifiedClose.Length;
								}
							}
						}
					}
				}
				int charIndex = match.Index;
				if ((modifiedReplaceLength > charIndex) || (modifiedClose != body.Substring(charIndex - modifiedReplaceLength + 1, modifiedReplaceLength)))
				{
					if (charIndex != 0)
					{
						writer.Write(body.Substring(0, charIndex + 1));
					}
					writer.Write(closeSentenceWith);
					writer.Write(body.Substring(charIndex + 1));
					return true;
				}
			}
			return false;
		}
	}
	#endregion // Static verbalization helpers on FactType class
	#region VerbalizationHyphenBinder struct
	/// <summary>
	/// A helper structure to enable hyphen binding
	/// </summary>
	public struct VerbalizationHyphenBinder
	{
		#region Member Variables
		/// <summary>
		/// The reading text modified for verbalization. This will
		/// always be set if there are any hyphens in the reading's
		/// format text, and the replacement fields will always correspond
		/// to the default fact order.
		/// </summary>
		private string myModifiedReadingText;
		/// <summary>
		/// An array of format strings for individual roles
		/// </summary>
		private string[] myFormatReplacementFields;
		/// <summary>
		/// A regex pattern to find regions around hyphenated readings
		/// </summary>
		private static Regex myMainRegex;
		/// <summary>
		/// A regex pattern to extract the replacement index from a reading replacement field
		/// </summary>
		private static Regex myIndexMapRegex;
		#endregion // Member Variables
		#region Regex properties
		private static Regex MainRegex
		{
			get
			{
				#region Commented main regex pattern
				//string mainPatternCommented = @"(?xn)
				//\G
				//# Test if we're at format replacement field
				//(?((?<!\{)\{\d+\}(?!\}))
				//	# If so, just match the replacement field
				//	(((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\})))
				//	|
				//	# Test if there is a hyphen binding match before the format replacement field
				//	((?((.(?!\s+-\S))*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))
				//		# If there is a hyphen bind before the next replacement field then use it
				//		((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)(?<!(?<!\{)\{\d+\}(?!\}))-(?<AfterLeftHyphen>\s.*?))
				//		|
				//		# Otherwise, pick up all text before the next format replacement field
				//		((?<BeforeLeftHyphenWord>.*?))
				//	)
				//	# Get the format replacement field
				//	((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\})))
				//)
				//# Get any trailing information if it exists prior to the next format field
				//(
				//	(?=
				//		# Positive lookahead to see if there is a next format string
				//		(?(.+(?<!\{)\{\d+\}(?!\}))
				//			# Check before if there is a next format string
				//			(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-(?!(?<!\{)\{\d+\}(?!\}))\S((?!(?<!\{)\{\d+\}(?!\})).)*?(?<!\{)\{\d+\}(?!\}))
				//			|
				//			# Get any trailer if there is not a next format string
				//			([^\-]*?\s-\S.*?)
				//		)
				//	)
				//	# Get the before hyphen and right hyphen word if the look ahead succeeded
				//	(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>((?!(?<!\{)\{\d+\}(?!\}))\S)+)
				//)?";
				#endregion // Commented main regex pattern
				Regex regexMain = myMainRegex;
				if (regexMain == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myMainRegex,
						new Regex(
							@"(?n)\G(?((?<!\{)\{\d+\}(?!\}))(((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\})))|((?((.(?!\s+-\S))*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)(?<!(?<!\{)\{\d+\}(?!\}))-(?<AfterLeftHyphen>\s.*?))|((?<BeforeLeftHyphenWord>.*?)))((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))))((?=(?(.+(?<!\{)\{\d+\}(?!\}))(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-(?!(?<!\{)\{\d+\}(?!\}))\S((?!(?<!\{)\{\d+\}(?!\})).)*?(?<!\{)\{\d+\}(?!\}))|([^\-]*?\s-\S.*?)))(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>((?!(?<!\{)\{\d+\}(?!\}))\S)+))?",
							RegexOptions.Compiled),
						null);
					regexMain = myMainRegex;
				}
				return regexMain;
			}
		}
		private static Regex IndexMapRegex
		{
			get
			{
				Regex regexIndexMap = myIndexMapRegex;
				if (regexIndexMap == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myIndexMapRegex,
						new Regex(
							@"(?n)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					regexIndexMap = myIndexMapRegex;
				}
				return regexIndexMap;
			}
		}
		#endregion // Regex properties
		#region Constructor
		/// <summary>
		/// Initialize a structure to hyphen-bind the verbalization for a reading
		/// </summary>
		/// <param name="reading">The reading to test.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="defaultOrder">The roles from the parent fact type. Provides the order of the expected replacement fields.</param>
		/// <param name="unaryRoleIndex">Treat as a unary role if this index is set.</param>
		/// <param name="replacementFormatString">The string used to format replacement fields. The format string is used to build another
		/// format string with one replacement field. It must consist of a {{0}} representing the eventual replacement field, a {0} for the leading
		/// hyphen-bound text, and a {1} for the trailing hyphen-bound text.</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields. Provides supplemental formatting
		/// for the leading and trailing replacement fields in <paramref name="replacementFormatString"/>.</param>
		public VerbalizationHyphenBinder(IReading reading, IFormatProvider formatProvider, IList<RoleBase> defaultOrder, int? unaryRoleIndex, string replacementFormatString, string predicatePartDecorator)
		{
			string readingText;
			int roleCount;
			int[] indexMap = null;

			// First test if there is any hyphen to look for
			if (reading == null ||
				-1 == (readingText = reading.Text).IndexOf('-'))
			{
				myModifiedReadingText = null;
				myFormatReplacementFields = null;
				return;
			}
			else if (unaryRoleIndex.HasValue)
			{
				roleCount = 1;
			}
			else
			{
				// Now see the reading has the same order as the fact. If not,
				// create an indexMap array that maps the reading role order to
				// the fact role order.
				roleCount = defaultOrder.Count;
				IList<RoleBase> readingRoles = reading.RoleCollection;
				Debug.Assert(readingRoles.Count == roleCount);
				int firstIndexChange = -1;
				for (int i = 0; i < roleCount; ++i)
				{
					RoleBase readingRole = readingRoles[i];
					if (readingRole == defaultOrder[i])
					{
						if (indexMap != null)
						{
							indexMap[i] = i;
						}
						continue;
					}
					if (indexMap == null)
					{
						indexMap = new int[roleCount];
						// Catch up to where we are now
						for (int j = 0; j < i; ++j)
						{
							indexMap[j] = j;
						}
						firstIndexChange = i;
					}
					for (int j = firstIndexChange; j < roleCount; ++j)
					{
						if (readingRole == defaultOrder[j])
						{
							indexMap[i] = j;
							break;
						}
					}
				}
			}

			// Make sure the regex objects are initialied
			Regex regexMain = MainRegex;
			Regex regexIndexMap = IndexMapRegex;

			// Build the new format string and do index mapping along the way
			formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
			string[] hyphenBoundFormatStrings = null;
			bool decoratePredicateText = !string.IsNullOrEmpty(predicatePartDecorator) && predicatePartDecorator != "{0}";
			myModifiedReadingText = regexMain.Replace(
				readingText,
				delegate(Match match)
				{
					string retVal;
					GroupCollection groups = match.Groups;
					string stringReplaceIndex = groups["ReplaceIndex"].Value;
					int replaceIndex = int.Parse(stringReplaceIndex, formatProvider);
					string leftWord = groups["LeftHyphenWord"].Value;
					string rightWord = groups["RightHyphenWord"].Value;
					string leadText = groups["BeforeLeftHyphenWord"].Value;
					if (leftWord.Length != 0 || rightWord.Length != 0)
					{
						bool validIndex = replaceIndex < roleCount;
						if (validIndex)
						{
							leftWord = NormalizeLeftHyphen(leftWord, groups["AfterLeftHyphen"].Value);
							rightWord = NormalizeRightHyphen(groups["BeforeRightHyphen"].Value, rightWord);
							if (decoratePredicateText)
							{
								leftWord = string.Format(formatProvider, predicatePartDecorator, leftWord);
								rightWord = string.Format(formatProvider, predicatePartDecorator, rightWord);
							}
							string boundFormatter = string.Format(formatProvider, replacementFormatString, leftWord, rightWord);
							if (hyphenBoundFormatStrings == null)
							{
								hyphenBoundFormatStrings = new string[roleCount];
							}
							if (indexMap != null)
							{
								replaceIndex = indexMap[replaceIndex];
							}
							hyphenBoundFormatStrings[replaceIndex] = boundFormatter;
						}
						if (leadText.Length != 0 && indexMap != null)
						{
							leadText = regexIndexMap.Replace(
								leadText,
								delegate(Match innerMatch)
								{
									int innerReplaceIndex = int.Parse(innerMatch.Groups["ReplaceIndex"].Value, formatProvider);
									return (innerReplaceIndex < roleCount) ?
										string.Concat("{", indexMap[innerReplaceIndex].ToString(formatProvider), "}") :
										string.Concat("{{", innerReplaceIndex.ToString(formatProvider), "}}");
								});
						}
						retVal = string.Concat(
							leadText,
							validIndex ? "{" : "{{",
							(indexMap == null) ? stringReplaceIndex : replaceIndex.ToString(formatProvider),
							validIndex ? "}" : "}}");
					}
					else if (indexMap != null)
					{
						retVal = regexIndexMap.Replace(
							match.Value,
							delegate(Match innerMatch)
							{
								int innerReplaceIndex = int.Parse(innerMatch.Groups["ReplaceIndex"].Value, formatProvider);
								return (innerReplaceIndex < roleCount) ?
									string.Concat("{", indexMap[innerReplaceIndex].ToString(formatProvider), "}") :
									string.Concat("{{", innerReplaceIndex.ToString(formatProvider), "}}");
							});
					}
					else
					{
						retVal = match.Value;
					}
					return retVal;
				});
			myFormatReplacementFields = hyphenBoundFormatStrings;
		}
		#endregion // Constructor
		#region Member Functions
		/// <summary>
		/// Perform any necessary hyphen-binding on the provided role replacement field
		/// </summary>
		/// <param name="basicRoleReplacement">The basic replacement field. Generally consists of a formatted object name.</param>
		/// <param name="roleIndex">The index of the represented role in the fact order</param>
		/// <returns>A modified replacement</returns>
		public string HyphenBindRoleReplacement(string basicRoleReplacement, int roleIndex)
		{
			string[] formatFields = myFormatReplacementFields;
			string formatField;
			if (formatFields != null &&
				roleIndex < formatFields.Length &&
				null != (formatField = formatFields[roleIndex]))
			{
				return string.Format(CultureInfo.CurrentCulture, formatField, basicRoleReplacement);
			}
			return basicRoleReplacement;
		}
		/// <summary>
		/// Populate the predicate text with the supplied replacement fields. Defers to
		/// <see cref="FactType.PopulatePredicateText(IReading,IFormatProvider,String,IList{RoleBase},String[])"/> if no hyphen-bind occurred.
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields.
		/// This must match the default order passed to the constructor.</param>
		/// <param name="roleReplacements">The replacement fields</param>
		/// <param name="unmodifiedRoleReplacements">The roleReplacements array have not been modified with the HyphenBindRoleReplacement method</param>
		/// <returns>The populated predicate text</returns>
		public string PopulatePredicateText(IReading reading, IFormatProvider formatProvider, string predicatePartDecorator, IList<RoleBase> defaultOrder, string[] roleReplacements, bool unmodifiedRoleReplacements)
		{
			string formatText = myModifiedReadingText;
			if (formatText == null)
			{
				return FactType.PopulatePredicateText(reading, formatProvider, predicatePartDecorator, defaultOrder, roleReplacements);
			}
			else
			{
				string[] formatFields = unmodifiedRoleReplacements ? myFormatReplacementFields : null;
				return Reading.ReplaceFields(
					formatText,
					formatProvider,
					predicatePartDecorator,
					(formatFields == null) ?
						(ReadingTextFieldReplace)delegate(int fieldIndex)
						{
							return roleReplacements[fieldIndex];
						} :
						delegate(int fieldIndex)
						{
							// The default order passed in here is assumed to be the same one
							// passed to the constructor. If the format is modified, then the
							// replacement fields have been renumbered in the format string to
							// correspond to this order, so there is no translation needed here.
							string useFormat = formatFields[fieldIndex];
							return (useFormat == null) ? roleReplacements[fieldIndex] : string.Format(formatProvider, useFormat, roleReplacements[fieldIndex]);
						});
			}
		}
		/// <summary>
		/// Populate the predicate text with the supplied replacement fields. Defers to
		/// <see cref="FactType.PopulatePredicateText(IReading,IFormatProvider,String,RoleReplacementProvider)"/>
		/// if no hyphen-bind occurred. To pre-populate modified replacement fields, use the
		/// <see cref="PopulatePredicateText(IReading,IFormatProvider,String,IList{RoleBase},String[],Boolean)"/>
		/// variant of this method.
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields.
		/// This must match the default order passed to the constructor.</param>
		/// <param name="replacementProvider">A callback for generating role replacement text.</param>
		/// <returns>The populated predicate text</returns>
		public string PopulatePredicateText(IReading reading, IFormatProvider formatProvider, string predicatePartDecorator, IList<RoleBase> defaultOrder, RoleReplacementProvider replacementProvider)
		{
			string formatText = myModifiedReadingText;
			if (formatText == null)
			{
				return FactType.PopulatePredicateText(reading, formatProvider, predicatePartDecorator, replacementProvider);
			}
			else
			{
				string[] formatFields = myFormatReplacementFields;
				return Reading.ReplaceFields(
					formatText,
					formatProvider,
					predicatePartDecorator,
					(formatFields == null) ?
						(ReadingTextFieldReplace)delegate(int fieldIndex)
						{
							return replacementProvider(defaultOrder[fieldIndex], null);
						} :
						delegate(int fieldIndex)
						{
							// The default order passed in here is assumed to be the same one
							// passed to the constructor. If the format is modified, then the
							// replacement fields have been reordered in the format string to
							// correspond to this order, so there is no translation needed here.
							return replacementProvider(defaultOrder[fieldIndex], formatFields[fieldIndex]);
						});
			}
		}
		#endregion // Member Functions
		#region Static Functions
		/// <summary>
		/// Determines whether or not the given predicate uses hyphen binding
		/// for any <see cref="RoleBase"/> in a <paramref name="reading"/>.
		/// </summary>
		/// <param name="reading">The reading to test.</param>
		/// <returns>True if the predicate is hyphen bound</returns>
		public static bool IsHyphenBound(IReading reading)
		{
			string readingText;

			// First test if there is any hyphen to look for
			if (reading == null ||
				-1 == (readingText = reading.Text).IndexOf('-'))
			{
				return false;
			}

			Match match = MainRegex.Match(readingText);
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string leftWord = groups["LeftHyphenWord"].Value;
				string rightWord = groups["RightHyphenWord"].Value;
				if (leftWord.Length != 0 || rightWord.Length != 0)
				{
					return true;
				}
				match = match.NextMatch();
			}
			return false;
		}
		/// <summary>
		/// Determines whether or not the given predicate uses hyphen binding
		/// for a specific <paramref name="role"/> in a <paramref name="reading"/>.
		/// </summary>
		/// <param name="reading">The reading to test.</param>
		/// <param name="role">The specific <see cref="RoleBase"/> to locate.</param>
		/// <returns>True if the given role is hyphen-bound in this predicate.</returns>
		public static bool IsHyphenBound(IReading reading, RoleBase role)
		{
			string readingText;

			// First test if there is any hyphen to look for
			if (reading == null ||
				-1 == (readingText = reading.Text).IndexOf('-'))
			{
				return false;
			}

			Match match = MainRegex.Match(readingText);
			int roleIndex = -1;
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string leftWord = groups["LeftHyphenWord"].Value;
				string rightWord = groups["RightHyphenWord"].Value;
				if (leftWord.Length != 0 || rightWord.Length != 0)
				{
					if (roleIndex == -1)
					{
						roleIndex = reading.RoleCollection.IndexOf(role);
						if (roleIndex == -1)
						{
							return false;
						}
					}
					if (roleIndex == int.Parse(groups["ReplaceIndex"].Value, CultureInfo.InvariantCulture))
					{
						return true;
					}
				}
				match = match.NextMatch();
			}
			return false;
		}
		/// <summary>
		/// Determines whether or not the given predicate using hyphen binding
		/// for a specific <paramref name="roleIndex"/> in a <paramref name="reading"/>.
		/// </summary>
		/// <param name="readingText">The reading text to test.</param>
		/// <param name="roleIndex">The index to test.</param>
		/// <returns>True if the role at the given index is hyphen-bound in this predicate.</returns>
		public static bool IsHyphenBound(string readingText, int roleIndex)
		{
			// First test if there is any hyphen to look for
			if (string.IsNullOrEmpty(readingText) ||
				-1 == readingText.IndexOf('-'))
			{
				return false;
			}

			Match match = MainRegex.Match(readingText);
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string leftWord = groups["LeftHyphenWord"].Value;
				string rightWord = groups["RightHyphenWord"].Value;
				if (leftWord.Length != 0 || rightWord.Length != 0)
				{
					if (roleIndex == int.Parse(groups["ReplaceIndex"].Value, CultureInfo.InvariantCulture))
					{
						return true;
					}
				}
				match = match.NextMatch();
			}
			return false;
		}
		/// <summary>
		/// Get a format string to hyphen bind a single role
		/// </summary>
		/// <param name="reading">The reading to use</param>
		/// <param name="role">The role to get a string for</param>
		/// <param name="unaryRoleIndex">Treat as a unary role if this index is set.</param>
		/// <returns>A format string with a single replacement field if the role is hyphen bound, or <see langword="null"/> otherwise.</returns>
		public static string GetFormatStringForHyphenBoundRole(IReading reading, RoleBase role, int? unaryRoleIndex)
		{
			IList<RoleBase> roles = reading.RoleCollection;
			int roleCount = roles.Count;
			bool isUnary = unaryRoleIndex.HasValue;
			IFormatProvider formatProvider = CultureInfo.CurrentCulture;
			Match match = MainRegex.Match(reading.Text);
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string leftWord = groups["LeftHyphenWord"].Value;
				string rightWord = groups["RightHyphenWord"].Value;
				if (leftWord.Length != 0 || rightWord.Length != 0)
				{
					string stringReplaceIndex = groups["ReplaceIndex"].Value;
					int replaceIndex = int.Parse(stringReplaceIndex, formatProvider);
					if ((isUnary && replaceIndex == 0) ||
						(replaceIndex < roleCount && roles[replaceIndex] == role))
					{
						return NormalizeLeftHyphen(leftWord, groups["AfterLeftHyphen"].Value) + "{0}" + NormalizeRightHyphen(groups["BeforeRightHyphen"].Value, rightWord);
					}
				}
				match = match.NextMatch();
			}
			return null;
		}
		/// <summary>
		/// Combine the left hyphen bound word and the following text, collapsing the first trailing
		/// space if a hyphen remains at the end of the left word. This enables a hyphen to be specified
		/// in the reading without a space after it. 'FORE-- WORD' will produce FORE-WORD.
		/// </summary>
		/// <param name="leftWord">The left word, verified for a trailing hyphen.</param>
		/// <param name="afterHyphenBind">The text after the hyphen-space pair (including the space).</param>
		/// <returns>A combined string.</returns>
		private static string NormalizeLeftHyphen(string leftWord, string afterHyphenBind)
		{
			if (afterHyphenBind.Length != 0)
			{
				if (leftWord.EndsWith("-") && afterHyphenBind.Length != 0 && char.IsWhiteSpace(afterHyphenBind, 0))
				{
					return leftWord + afterHyphenBind.Substring(1);
				}
				return leftWord + afterHyphenBind;
			}
			return leftWord;
		}
		/// <summary>
		/// Combine the right hyphen word and the preceding text, collapsing the last preceding
		/// space if a hyphen remains part of the right word.
		/// </summary>
		/// <param name="beforeHyphenBind">The text before the space-hyphen pair (including the space).</param>
		/// <param name="rightWord">The right word, verified for a lead hyphen.</param>
		/// <returns>A combined string.</returns>
		private static string NormalizeRightHyphen(string beforeHyphenBind, string rightWord)
		{
			if (beforeHyphenBind.Length != 0)
			{
				int beforeHyphenBindLength;
				if (rightWord.StartsWith("-") && (beforeHyphenBindLength = beforeHyphenBind.Length) != 0 && char.IsWhiteSpace(beforeHyphenBind, beforeHyphenBindLength - 1))
				{
					return beforeHyphenBind.Substring(0, beforeHyphenBindLength - 1) + rightWord;
				}
				return beforeHyphenBind + rightWord;
			}
			return rightWord;
		}
		#endregion // Static Functions
	}
	#endregion // VerbalizationHyphenBinder struct
	#region VerbalizationCallbackWriter class
	/// <summary>
	/// Handles the writing of utility snippets for Verbalization targets
	/// </summary>
	public class VerbalizationCallbackWriter
	{
		/// <summary>
		/// Writer used to write Verbalizations
		/// </summary>
		private TextWriter myWriter;
		/// <summary>
		/// Replacement fields for CSS stylesheet declaration
		/// </summary>
		private string[] myDocumentHeaderReplacementFields;
		/// <summary>
		/// List of Core Verbalization Snippets
		/// </summary>
		private IVerbalizationSets<CoreVerbalizationSnippetType> myCoreSnippets;

		/// <summary>
		/// Initializes a new instance of <see cref="VerbalizationCallbackWriter"/>
		/// </summary>
		/// <param name="snippets">The <see cref="IVerbalizationSets"/> containing the Verbalization snippet set to use</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to</param>
		public VerbalizationCallbackWriter(IVerbalizationSets<CoreVerbalizationSnippetType> snippets, TextWriter writer)
		{
			myCoreSnippets = snippets;
			myWriter = writer;
			writer.NewLine = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerNewLine);
		}
		/// <summary>
		/// Initializes a new instance of <see cref="VerbalizationCallbackWriter"/>
		/// </summary>
		/// <param name="snippets">The <see cref="IVerbalizationSets"/> containing the Verbalization snippet set to use</param>
		/// <param name="writer">The <see cref="TextWriter"/> to write to</param>
		/// <param name="documentHeaderReplacementFields">The replacement fields used to define the colors for the CSS applied to snippets</param>
		public VerbalizationCallbackWriter(IVerbalizationSets<CoreVerbalizationSnippetType> snippets, TextWriter writer, string[] documentHeaderReplacementFields)
			: this(snippets, writer)
		{
			myDocumentHeaderReplacementFields = documentHeaderReplacementFields;
		}
		/// <summary>
		/// Gets the underlying TextWriter
		/// </summary>
		public TextWriter Writer
		{
			get { return myWriter; }
		}
		/// <summary>
		/// Writes the Document Header defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteDocumentHeader()
		{
			myWriter.Write(string.Format(CultureInfo.InvariantCulture, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentHeader), myDocumentHeaderReplacementFields));
		}
		/// <summary>
		/// Writes the Document Footer defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteDocumentFooter()
		{
			myWriter.Write(string.Format(CultureInfo.InvariantCulture, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentFooter)));
		}
		/// <summary>
		/// Writes the verbalization opener defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteOpen()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerOpenVerbalization));
		}
		/// <summary>
		/// Writes the verbalization closer defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void WriteClose()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerCloseVerbalization));
		}
		/// <summary>
		/// Writes the snippet used to increase indentation defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void IncreaseIndent()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerIncreaseIndent));
		}
		/// <summary>
		/// Writes the snippet used to decrease indentation defined in the current Verbalization Snippet Set
		/// </summary>
		public virtual void DecreaseIndent()
		{
			myWriter.Write(myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDecreaseIndent));
		}
	}
	#endregion // VerbalizationCallbackWriter class
	#region VerbalizationHelper class
	/// <summary>
	/// Provides helper methods for Verbalizations
	/// </summary>
	public static class VerbalizationHelper
	{
		#region VerbalizationResult Enum
		/// <summary>
		/// An enum to determine callback handling during verbalization
		/// </summary>
		private enum VerbalizationResult
		{
			/// <summary>
			/// The element was successfully verbalized
			/// </summary>
			Verbalized,
			/// <summary>
			/// The element was previously verbalized
			/// </summary>
			AlreadyVerbalized,
			/// <summary>
			/// The element was not verbalized
			/// </summary>
			NotVerbalized,
		}
		#endregion // VerbalizationResult Enum
		#region VerbalizationHandler Delegate
		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate VerbalizationResult VerbalizationHandler(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, VerbalizationSign sign, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel);
		#endregion // VerbalizationHandler Delegate
		#region VerbalizationContextImpl class
		private sealed class VerbalizationContextImpl : IVerbalizationContext
		{
			/// <summary>
			/// A callback delegate enabling a verbalizer to tell
			/// the hosting window that it is about to begin verbalizing.
			/// This enables the host window to delay writing content outer
			/// content until it knows that text is about to be written by
			/// the verbalizer to the writer
			/// </summary>
			/// <param name="content">The style of verbalization content</param>
			public delegate void NotifyBeginVerbalization(VerbalizationContent content);
			public delegate void NotifyDeferVerbalization(object target, DeferVerbalizationOptions options, IVerbalizeFilterChildren childFilter);
			public delegate bool NotifyAlreadyVerbalized(object target);
			private NotifyBeginVerbalization myBeginCallback;
			private NotifyDeferVerbalization myDeferCallback;
			private NotifyAlreadyVerbalized myAlreadyVerbalizedCallback;
			private NotifyAlreadyVerbalized myVerbalizedLocallyCallback;
			private string myVerbalizationTarget;
			public VerbalizationContextImpl(NotifyBeginVerbalization beginCallback, NotifyDeferVerbalization deferCallback, NotifyAlreadyVerbalized alreadyVerbalizedCallback, NotifyAlreadyVerbalized locallyVerbalizedCallback, string verbalizationTarget)
			{
				myBeginCallback = beginCallback;
				myDeferCallback = deferCallback;
				myAlreadyVerbalizedCallback = alreadyVerbalizedCallback;
				myVerbalizedLocallyCallback = locallyVerbalizedCallback;
				myVerbalizationTarget = verbalizationTarget;
			}
			#region IVerbalizationContext Implementation
			void IVerbalizationContext.BeginVerbalization(VerbalizationContent content)
			{
				myBeginCallback(content);
			}
			void IVerbalizationContext.DeferVerbalization(object target, DeferVerbalizationOptions options, IVerbalizeFilterChildren childFilter)
			{
				if (myDeferCallback != null)
				{
					myDeferCallback(target, options, childFilter);
				}
			}
			bool IVerbalizationContext.AlreadyVerbalized(object target)
			{
				if (myAlreadyVerbalizedCallback != null)
				{
					return myAlreadyVerbalizedCallback(target);
				}
				return false;
			}
			string IVerbalizationContext.VerbalizationTarget
			{
				get
				{
					return myVerbalizationTarget;
				}
			}
			bool IVerbalizationContext.TestVerbalizedLocally(object target)
			{
				if (myVerbalizedLocallyCallback != null)
				{
					return myVerbalizedLocallyCallback(target);
				}
				return false;
			}
			#endregion // IVerbalizationContext Implementation
		}
		#endregion // VerbalizationContextImpl class
		#region VerbalizeElement methods
		/// <summary>
		/// Handles the callback made by VerbalizeElement to execute verbalization
		/// </summary>
		/// <param name="writer">The VerbalizationCallbackWriter object used to write target specific snippets</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="extensionVerbalizer"></param>
		/// <param name="verbalizationTarget"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="locallyVerbalized"></param>
		/// <param name="verbalizer">The IVerbalize element to verbalize</param>
		/// <param name="callback">The original callback handler.</param>
		/// <param name="indentationLevel">The indentation level of the verbalization</param>
		/// <param name="sign"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		/// <returns></returns>
		private static VerbalizationResult VerbalizeElement_VerbalizationResult(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, VerbalizationSign sign, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			if (indentationLevel == 0 &&
				alreadyVerbalized != null &&
				alreadyVerbalized.ContainsKey(verbalizer))
			{
				return VerbalizationResult.AlreadyVerbalized;
			}
			bool localFirstWrite = firstWrite;
			bool localFirstCallPending = firstCallPending;
			int localLastLevel = lastLevel;
			bool retVal = verbalizer.GetVerbalization(
				writer.Writer,
				snippetsDictionary,
				new VerbalizationContextImpl(
				delegate(VerbalizationContent content)
				{
					// Prepare for verbalization on this element. Everything
					// is delayed to this point in case the verbalization implementation
					// does not callback to the text writer.
					if (localFirstWrite)
					{
						if (localFirstCallPending)
						{
							localFirstCallPending = false;
							// Write the HTML header to the buffer
							writer.WriteDocumentHeader();
						}

						// write open tag for new verbalization
						writer.WriteOpen();
						localFirstWrite = false;
					}
					else if (writeSecondaryLines)
					{
						writer.Writer.WriteLine();
					}

					// Write indentation tags as needed
					if (indentationLevel > localLastLevel)
					{
						do
						{
							writer.IncreaseIndent();
							++localLastLevel;
						} while (localLastLevel != indentationLevel);
					}
					else if (localLastLevel > indentationLevel)
					{
						do
						{
							writer.DecreaseIndent();
							--localLastLevel;
						} while (localLastLevel != indentationLevel);
					}
				},
				delegate(object target, DeferVerbalizationOptions options, IVerbalizeFilterChildren childFilter)
				{
					bool localfcp = localFirstCallPending;
					bool localfw = localFirstWrite;
					int localll = localLastLevel;
					VerbalizationHelper.VerbalizeElement(
						target,
						snippetsDictionary,
						extensionVerbalizer,
						verbalizationTarget,
						(0 == (options & DeferVerbalizationOptions.MultipleVerbalizations)) ? alreadyVerbalized : null,
						locallyVerbalized,
						childFilter,
						writer,
						callback,
						sign,
						indentationLevel,
						(0 != (options & DeferVerbalizationOptions.AlwaysWriteLine)) ?
							true :
							((0 != (options & DeferVerbalizationOptions.NeverWriteLine)) ? false : writeSecondaryLines),
						ref localfcp,
						ref localfw,
						ref localll);
					localFirstCallPending = localfcp;
					localFirstWrite = localfw;
					localLastLevel = localll;
				},
				delegate(object target)
				{
					IVerbalize verbalizerKey = null;
					IRedirectVerbalization surrogateRedirect;
					if (alreadyVerbalized != null &&
						(null == (surrogateRedirect = target as IRedirectVerbalization) ||
						null == (verbalizerKey = surrogateRedirect.SurrogateVerbalizer)))
					{
						verbalizerKey = target as IVerbalize;
					}
					return (verbalizerKey == null) ? false : alreadyVerbalized.ContainsKey(verbalizerKey);
				},
				delegate(object target)
				{
					if (target == null || locallyVerbalized == null)
					{
						return false;
					}
					bool retValLoc = locallyVerbalized.ContainsKey(target);
					if (!retValLoc)
					{
						locallyVerbalized.Add(target, target);
					}
					return retValLoc;
				},
				verbalizationTarget),
				sign);
			lastLevel = localLastLevel;
			firstWrite = localFirstWrite;
			firstCallPending = localFirstCallPending;
			if (retVal)
			{
				if (indentationLevel == 0 && alreadyVerbalized != null)
				{
					alreadyVerbalized.Add(verbalizer, verbalizer);
				}
				return VerbalizationResult.Verbalized;
			}
			else
			{
				return VerbalizationResult.NotVerbalized;
			}
		}
		/// <summary>
		/// Determine the indentation level for verbalizing a ModelElement, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="element">The element to verbalize</param>
		/// <param name="snippetsDictionary">The default or loaded verbalization sets. Passed through all verbalization calls.</param>
		/// <param name="extensionVerbalizer">The service to retrieve additional child verbalizations from extension elements</param>
		/// <param name="verbalizationTarget">The verbalization target name, representing the container for the verbalization output.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="locallyVerbalized">A dictionary of elements verbalized during the current top level verbalization.</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeElement(object element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizeElement(
				element,
				snippetsDictionary,
				extensionVerbalizer,
				verbalizationTarget,
				alreadyVerbalized,
				locallyVerbalized,
				null,
				writer,
				new VerbalizationHandler(VerbalizeElement_VerbalizationResult),
				sign,
				0,
				writeSecondaryLines,
				ref localFirstCallPending,
				ref firstWrite,
				ref lastLevel);
			while (lastLevel > 0)
			{
				writer.DecreaseIndent();
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.WriteClose();
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Determine the indentation level for verbalizing custom and extension children, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="customChildren">The enumeration of custom children to verbalize</param>
		/// <param name="extensionChildren">The enumeration of extension children to verbalize</param>
		/// <param name="snippetsDictionary">The default or loaded verbalization sets. Passed through all verbalization calls.</param>
		/// <param name="extensionVerbalizer">The service to retrieve additional child verbalizations from extension elements</param>
		/// <param name="verbalizationTarget">The verbalization target name, representing the container for the verbalization output.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="locallyVerbalized">A dictionary of elements verbalized during the current top level verbalization.</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeChildren(IEnumerable<CustomChildVerbalizer> customChildren, IEnumerable<CustomChildVerbalizer> extensionChildren, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			if (customChildren == null && extensionChildren == null)
			{
				return;
			}
			VerbalizationHandler handler = new VerbalizationHandler(VerbalizeElement_VerbalizationResult);
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			if (customChildren != null)
			{
				VerbalizeCustomChildren(
					customChildren,
					writer,
					handler,
					snippetsDictionary,
					extensionVerbalizer,
					verbalizationTarget,
					alreadyVerbalized,
					locallyVerbalized,
					sign,
					0,
					writeSecondaryLines,
					ref localFirstCallPending,
					ref firstWrite,
					ref lastLevel);
			}
			if (extensionChildren != null)
			{
				VerbalizeCustomChildren(
					extensionChildren,
					writer,
					handler,
					snippetsDictionary,
					extensionVerbalizer,
					verbalizationTarget,
					alreadyVerbalized,
					locallyVerbalized,
					sign,
					0,
					writeSecondaryLines,
					ref localFirstCallPending,
					ref firstWrite,
					ref lastLevel);
			}
			while (lastLevel > 0)
			{
				writer.DecreaseIndent();
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.WriteClose();
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Verbalize the passed in element and all its children
		/// </summary>
		/// <param name="element"></param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="extensionVerbalizer"></param>
		/// <param name="verbalizationTarget"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="locallyVerbalized"></param>
		/// <param name="outerFilter"></param>
		/// <param name="writer"></param>
		/// <param name="callback"></param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="indentLevel"></param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeElement(object element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalizeFilterChildren outerFilter, VerbalizationCallbackWriter writer, VerbalizationHandler callback, VerbalizationSign sign, int indentLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			IVerbalize parentVerbalize = null;
			IRedirectVerbalization surrogateRedirect;
			if (indentLevel == 0 &&
				null != (surrogateRedirect = element as IRedirectVerbalization) &&
				null != (parentVerbalize = surrogateRedirect.SurrogateVerbalizer))
			{
				element = parentVerbalize;
			}
			else
			{
				parentVerbalize = element as IVerbalize;
			}
			bool disposeVerbalizer = false;
			if (outerFilter != null && parentVerbalize != null)
			{
				CustomChildVerbalizer filterResult = outerFilter.FilterChildVerbalizer(parentVerbalize, sign);
				parentVerbalize = filterResult.Instance;
				disposeVerbalizer = filterResult.DisposeAfterVerbalization;
			}
			try
			{
				VerbalizationResult result = (parentVerbalize != null) ? callback(writer, snippetsDictionary, extensionVerbalizer, verbalizationTarget, alreadyVerbalized, locallyVerbalized, parentVerbalize, callback, indentLevel, sign, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel) : VerbalizationResult.NotVerbalized;
				if (result == VerbalizationResult.AlreadyVerbalized)
				{
					return;
				}
				ModelElement modelElement = element as ModelElement;
				bool parentVerbalizeOK = result == VerbalizationResult.Verbalized;
				bool verbalizeChildren = parentVerbalizeOK ? (modelElement != null) : (element is IVerbalizeChildren);
				IVerbalizeCustomChildren customChildren = element as IVerbalizeCustomChildren;
				IVerbalizeExtensionChildren extensionChildren = extensionVerbalizer.GetExtensionVerbalizer(element);
				if (verbalizeChildren || (customChildren != null) || (extensionChildren != null))
				{
					if (parentVerbalizeOK)
					{
						++indentLevel;
					}
					IVerbalizeFilterChildren filter = parentVerbalize as IVerbalizeFilterChildren;
					if (filter != null)
					{
						if (outerFilter != null)
						{
							filter = new CompositeChildFilter(outerFilter, filter);
						}
					}
					else
					{
						filter = outerFilter;
					}

					if (verbalizeChildren)
					{
						IVerbalizeFilterChildrenByRole roleFilter = parentVerbalize as IVerbalizeFilterChildrenByRole;
						ReadOnlyCollection<DomainRoleInfo> aggregatingList = modelElement.GetDomainClass().AllDomainRolesPlayed;
						int aggregatingCount = aggregatingList.Count;
						for (int i = 0; i < aggregatingCount; ++i)
						{
							DomainRoleInfo roleInfo = aggregatingList[i];
							if (roleInfo.IsEmbedding &&
								(roleFilter == null || !roleFilter.BlockEmbeddedVerbalization(roleInfo)))
							{
								LinkedElementCollection<ModelElement> children = roleInfo.GetLinkedElements(modelElement);
								int childCount = children.Count;
								for (int j = 0; j < childCount; ++j)
								{
									VerbalizeElement(children[j], snippetsDictionary, extensionVerbalizer, verbalizationTarget, alreadyVerbalized, locallyVerbalized, filter, writer, callback, sign, indentLevel, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
								}
							}
						}
					}
					// TODO: Need BeforeNaturalChildren/AfterNaturalChildren/SkipNaturalChildren settings for IVerbalizeCustomChildren
					if (customChildren != null)
					{
						VerbalizeCustomChildren(
							customChildren.GetCustomChildVerbalizations(filter, sign),
							writer,
							callback,
							snippetsDictionary,
							extensionVerbalizer,
							ORMCoreDomainModel.VerbalizationTargetName,
							alreadyVerbalized,
							locallyVerbalized,
							sign,
							indentLevel,
							writeSecondaryLines,
							ref firstCallPending,
							ref firstWrite,
							ref lastLevel);
					}
					if (extensionChildren != null)
					{
						VerbalizeCustomChildren(
							extensionChildren.GetExtensionChildVerbalizations(element, filter, sign),
							writer,
							callback,
							snippetsDictionary,
							extensionVerbalizer,
							ORMCoreDomainModel.VerbalizationTargetName,
							alreadyVerbalized,
							locallyVerbalized,
							sign,
							indentLevel,
							writeSecondaryLines,
							ref firstCallPending,
							ref firstWrite,
							ref lastLevel);
					}
				}
				else if (!parentVerbalizeOK && indentLevel == 0)
				{
					ISurveyNodeReference surveyReference = element as ISurveyNodeReference;
					if (surveyReference != null)
					{
						SurveyNodeReferenceOptions options = surveyReference.SurveyNodeReferenceOptions;
						object deferToElement;
						if (0 != (options & SurveyNodeReferenceOptions.SelectReferenceReason) &&
							null != (deferToElement = surveyReference.SurveyNodeReferenceReason))
						{
							VerbalizeElement(deferToElement, snippetsDictionary, extensionVerbalizer, verbalizationTarget, alreadyVerbalized, locallyVerbalized, sign, writer, writeSecondaryLines, ref firstCallPending);
							return;
						}
						if (null != (deferToElement = surveyReference.ReferencedElement))
						{
							VerbalizeElement(deferToElement, snippetsDictionary, extensionVerbalizer, verbalizationTarget, alreadyVerbalized, locallyVerbalized, sign, writer, writeSecondaryLines, ref firstCallPending);
						}
					}
				}
			}
			finally
			{
				if (disposeVerbalizer)
				{
					IDisposable dispose = parentVerbalize as IDisposable;
					if (dispose != null)
					{
						dispose.Dispose();
					}
				}
			}
		}
		#region CompositeChildFilter class
		private sealed class CompositeChildFilter : IVerbalizeFilterChildren
		{
			#region Member Variables
			private IVerbalizeFilterChildren[] myFilters;
			#endregion // Member Variables
			#region Constructor
			public CompositeChildFilter(params IVerbalizeFilterChildren[] filters)
			{
				myFilters = filters;
			}
			#endregion // Constructor
			#region IVerbalizeFilterChildren Implementation
			CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(object child, VerbalizationSign sign)
			{
				CustomChildVerbalizer retVal = CustomChildVerbalizer.Empty;
				IVerbalizeFilterChildren[] filters = myFilters;
				for (int i = 0; i < filters.Length; ++i)
				{
					retVal = filters[i].FilterChildVerbalizer(child, sign);
					if (!retVal.IsEmpty)
					{
						break;
					}
					else if (retVal.IsBlocked)
					{
						break;
					}
				}
				return retVal;
			}
			#endregion // IVerbalizeFilterChildren Implementation
		}
		#endregion // CompositeChildFilter class
		/// <summary>
		/// Verbalizes CustomChildVerbalizer elements
		/// </summary>
		/// <param name="customChildren">The CustomChildVerbalizer elements to verbalize</param>
		/// <param name="writer">The target specific Writer to use</param>
		/// <param name="callback">The delegate used to handle the verbalization</param>
		/// <param name="snippetsDictionary"></param>
		/// <param name="extensionVerbalizer"></param>
		/// <param name="verbalizationTarget"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="locallyVerbalized"></param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="indentationLevel">The current level of indentation</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeCustomChildren(IEnumerable<CustomChildVerbalizer> customChildren, VerbalizationCallbackWriter writer, VerbalizationHandler callback, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, int indentationLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
		{
			if (customChildren == null)
			{
				return;
			}
			foreach (CustomChildVerbalizer customChild in customChildren)
			{
				IVerbalize childVerbalize = customChild.Instance;
				if (childVerbalize != null)
				{
					try
					{
						callback(writer, snippetsDictionary, extensionVerbalizer, verbalizationTarget, alreadyVerbalized, locallyVerbalized, childVerbalize, callback, indentationLevel, sign, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
					}
					finally
					{
						if (customChild.DisposeAfterVerbalization)
						{
							IDisposable dispose = childVerbalize as IDisposable;
							if (dispose != null)
							{
								dispose.Dispose();
							}
						}
					}
				}
			}
		}
		#endregion // VerbalizeElement methods
	}
	#endregion // VerbalizationHelper class
	#region ExtensionVerbalizerService class
	/// <summary>
	/// A standard implementation of <see cref="IExtensionVerbalizerService"/>
	/// </summary>
	public sealed class ExtensionVerbalizerService : IExtensionVerbalizerService
	{
		#region VerbalizeExtensionChildrenWrapper class
		private sealed class VerbalizeExtensionChildrenWrapper : IVerbalizeExtensionChildren
		{
			private List<IVerbalizeExtensionChildren> myWrappedItems;
			private VerbalizeExtensionChildrenWrapper()
			{
				myWrappedItems = new List<IVerbalizeExtensionChildren>();
			}
			#region Helper methods
			/// <summary>
			/// Combine two extension verbalizers.
			/// </summary>
			/// <param name="left">A starting element.</param>
			/// <param name="right">An element to combine with the starting element</param>
			/// <returns>Combined implementation</returns>
			public static IVerbalizeExtensionChildren Combine(IVerbalizeExtensionChildren left, IVerbalizeExtensionChildren right)
			{
				VerbalizeExtensionChildrenWrapper leftWrapper;
				VerbalizeExtensionChildrenWrapper rightWrapper;
				if (right == null)
				{
					return left;
				}
				else if (left == null)
				{
					return right;
				}
				else if (null != (leftWrapper = left as VerbalizeExtensionChildrenWrapper))
				{
					rightWrapper = right as VerbalizeExtensionChildrenWrapper;
					if (rightWrapper != null)
					{
						leftWrapper.myWrappedItems.AddRange(rightWrapper.myWrappedItems);
					}
					else
					{
						leftWrapper.myWrappedItems.Add(right);
					}
					return leftWrapper;
				}
				else if (null != (rightWrapper = left as VerbalizeExtensionChildrenWrapper))
				{
					rightWrapper.myWrappedItems.Insert(0, left);
					return rightWrapper;
				}
				else
				{
					rightWrapper = new VerbalizeExtensionChildrenWrapper();
					List<IVerbalizeExtensionChildren> list = rightWrapper.myWrappedItems;
					list.Add(left);
					list.Add(right);
					return rightWrapper;
				}
			}
			/// <summary>
			/// Remove an extension verbalizer from a verbalizer previously returned by
			/// <see cref="Combine"/>
			/// </summary>
			/// <param name="combinedChildVerbalizer">The combined <see cref="IVerbalizeExtensionChildren"/> element.</param>
			/// <param name="removeVerbalizer">An element to remove;</param>
			/// <returns>Combined implementation</returns>
			public static IVerbalizeExtensionChildren Remove(IVerbalizeExtensionChildren combinedChildVerbalizer, IVerbalizeExtensionChildren removeVerbalizer)
			{
				VerbalizeExtensionChildrenWrapper removeFromWrapper;
				if (removeVerbalizer == null)
				{
					return combinedChildVerbalizer;
				}
				else if (combinedChildVerbalizer == null)
				{
					return null;
				}
				else if (combinedChildVerbalizer == removeVerbalizer)
				{
					return null;
				}
				else if (null != (removeFromWrapper = combinedChildVerbalizer as VerbalizeExtensionChildrenWrapper))
				{
					List<IVerbalizeExtensionChildren> removeFromList = removeFromWrapper.myWrappedItems;
					VerbalizeExtensionChildrenWrapper removeWrapper = removeVerbalizer as VerbalizeExtensionChildrenWrapper;
					int matchIndex;
					if (removeWrapper != null)
					{
						foreach (IVerbalizeExtensionChildren removeItem in removeWrapper.myWrappedItems)
						{
							matchIndex = removeFromList.IndexOf(removeItem);
							if (matchIndex != -1)
							{
								removeFromList.RemoveAt(matchIndex);
							}
						}
					}
					else
					{
						matchIndex = removeFromList.IndexOf(removeVerbalizer);
						if (matchIndex != -1)
						{
							removeFromList.RemoveAt(matchIndex);
						}
					}
					switch (removeFromList.Count)
					{
						case 0:
							return null;
						case 1:
							return removeFromList[0];
					}
					return combinedChildVerbalizer;
				}
				else
				{
					return combinedChildVerbalizer;
				}
			}
			#endregion // Helper methods
			#region IVerbalizeExtensionChildren Implementation
			public IEnumerable<CustomChildVerbalizer> GetExtensionChildVerbalizations(object parentElement, IVerbalizeFilterChildren filter, VerbalizationSign sign)
			{
				foreach (IVerbalizeExtensionChildren child in myWrappedItems)
				{
					foreach (CustomChildVerbalizer nestedVerbalizer in child.GetExtensionChildVerbalizations(parentElement, filter, sign))
					{
						yield return nestedVerbalizer;
					}
				}
			}
			#endregion // IVerbalizeExtensionChildren Implementation
		}
		#endregion // VerbalizeExtensionChildrenWrapper class
		#region Member Variables
		private readonly Store myStore;
		private readonly Dictionary<RuntimeTypeHandle, IVerbalizeExtensionChildren> myExtensionVerbalizersDictionary;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a <see cref="ExtensionVerbalizerService"/> for the specified <see cref="Store"/>
		/// </summary>
		public ExtensionVerbalizerService(Store store)
		{
			Debug.Assert(store != null);
			this.myStore = store;
			this.myExtensionVerbalizersDictionary = new Dictionary<RuntimeTypeHandle, IVerbalizeExtensionChildren>(RuntimeTypeHandleComparer.Instance);
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// Get the context <see cref="Store"/>
		/// </summary>
		private Store Store
		{
			get
			{
				return myStore;
			}
		}
		#endregion // Access Properties
		#region IExtensionVerbalizerService Implementation
		void IExtensionVerbalizerService.AddOrRemoveExtensionVerbalizer(Type verbalizedElementType, IVerbalizeExtensionChildren extensionVerbalizer, bool includeSubtypes, EventHandlerAction action)
		{
			bool register = action == EventHandlerAction.Add;
			if (register)
			{
				this.RegisterExtensionVerbalizer(verbalizedElementType.TypeHandle, extensionVerbalizer);
			}
			else
			{
				this.UnregisterExtensionVerbalizer(verbalizedElementType.TypeHandle, extensionVerbalizer);
			}
			if (includeSubtypes)
			{
				Store store = this.myStore;
				DomainClassInfo domainClassInfo = store.DomainDataDirectory.FindDomainClass(verbalizedElementType);
				if (domainClassInfo != null)
				{
					foreach (DomainClassInfo subtypeInfo in domainClassInfo.AllDescendants)
					{
						if (register)
						{
							this.RegisterExtensionVerbalizer(subtypeInfo.ImplementationClass.TypeHandle, extensionVerbalizer);
						}
						else
						{
							this.UnregisterExtensionVerbalizer(subtypeInfo.ImplementationClass.TypeHandle, extensionVerbalizer);
						}
					}
				}
			}
		}
		private void RegisterExtensionVerbalizer(RuntimeTypeHandle key, IVerbalizeExtensionChildren extensionVerbalizer)
		{
			Dictionary<RuntimeTypeHandle, IVerbalizeExtensionChildren> dictionary = myExtensionVerbalizersDictionary;
			IVerbalizeExtensionChildren existingElement = null;
			if (dictionary.TryGetValue(key, out existingElement))
			{
				dictionary[key] = VerbalizeExtensionChildrenWrapper.Combine(existingElement, extensionVerbalizer);
			}
			else
			{
				dictionary[key] = extensionVerbalizer;
			}
		}
		private void UnregisterExtensionVerbalizer(RuntimeTypeHandle key, IVerbalizeExtensionChildren extensionVerbalizer)
		{
			Dictionary<RuntimeTypeHandle, IVerbalizeExtensionChildren> dictionary = this.myExtensionVerbalizersDictionary;
			IVerbalizeExtensionChildren existingElement;
			if (dictionary.TryGetValue(key, out existingElement))
			{
				IVerbalizeExtensionChildren newElement = VerbalizeExtensionChildrenWrapper.Remove(existingElement, extensionVerbalizer);
				if (newElement == null)
				{
					dictionary.Remove(key);
				}
				else if (newElement != existingElement)
				{
					dictionary[key] = newElement;
				}
			}
		}
		IVerbalizeExtensionChildren IExtensionVerbalizerService.GetExtensionVerbalizer(object verbalizedElement)
		{
			IVerbalizeExtensionChildren retVal;
			if (verbalizedElement != null && myExtensionVerbalizersDictionary.TryGetValue(verbalizedElement.GetType().TypeHandle, out retVal))
			{
				return retVal;
			}
			return null;
		}
		#endregion // IExtensionVerbalizerService Implementation
	}
	#endregion // ExtensionVerbalizerService class
	#region RolePathVerbalizer class
	/// <summary>
	/// Enumeration for text snippets and format strings
	/// used by <see cref="RolePathVerbalizer"/>. The actual
	/// strings are provided by an implementation of
	/// <see cref="IRolePathRenderer"/>
	/// </summary>
	public enum RolePathVerbalizerSnippetType
	{
		/// <summary>
		/// Specify a space-separated list of items to determine
		/// if split lists are rendered as integrated or separate
		/// blocks. Allowed values are {And, Or, Xor, !And, !Or, !Xor}
		/// </summary>
		HeaderListDirective,
		/// <summary>
		/// Specify a space-separated list of items to determine
		/// if a list style supports collapsing a repeated lead
		/// role. Allowed values are {Chain, And, Or, Xor, !And, !Or, !Xor, !Chain}.
		/// </summary>
		CollapsibleLeadDirective,
		/// <summary>
		/// An impersonal pronoun used as a replacement for
		/// a role player with an immediate back reference: 'that'.
		/// </summary>
		ImpersonalPronoun,
		/// <summary>
		/// An impersonal pronoun used as a replacement for
		/// a role player with an immediate back reference: 'who'.
		/// </summary>
		PersonalPronoun,
		/// <summary>
		/// Get an existential quantifier with a single role replacement: some {0}.
		/// </summary>
		ExistentialQuantifier,
		/// <summary>
		/// Get a negated existential quantifier with a single role replacmement: no {0}.
		/// </summary>
		NegatedExistentialQuantifier,
		/// <summary>
		/// Get a back reference quantifier with a single role replacement: that {0}.
		/// </summary>
		BackReferenceQuantifier,
		/// <summary>
		/// Relate two variables of different types that represent the same instance
		/// where the first variable is an impersonal object type: {0} that is {1}.
		/// </summary>
		ImpersonalIdentityCorrelation,
		/// <summary>
		/// Relate two variable names of different types that represent the same instance
		/// where the first variable is a personal object type: {0} who is {1}.
		/// </summary>
		PersonalIdentityCorrelation,
		/// <summary>
		/// Combine different hyphen-bound predicate parts around a central replacement
		/// field occupied by the role player: {0}{{0}}{1}
		/// </summary>
		HyphenBoundPredicatePart,
		/// <summary>
		/// Project a calculation or constant value onto a head variable: {0} = {1}
		/// </summary>
		HeadVariableProjection,
		/// <summary>
		/// Provide a scope for an aggregated parameter input: {0} of each {1}
		/// </summary>
		AggregateParameterScope,
		/// <summary>
		/// Limit values from an aggregate parameter input to distinct values: distinct {0}
		/// </summary>
		AggregateSetProjection,
		/// <summary>
		/// A space separated list of list closure snippet names from this
		/// enum that reverse an indentation. Trailing outdents can be tracked
		/// specially during formatting so that external text on the same line
		/// as the outdent keeps the same indentation level.
		/// </summary>
		ListCloseOutdentSnippets,
		/// <summary>
		/// </summary>
		ChainedListOpen,
		/// <summary>
		/// A separator for a chained list where the chained restriction
		/// applies only to elements contained in the preceding fact statement.
		/// </summary>
		ChainedListLocalRestrictionSeparator,
		/// <summary>
		/// A separator for a chained list where the chained restriction
		/// introduces additional fact statements. Note that the complex
		/// restriction separator is not used before a TailListOpen of
		/// an operator separated list, which is any split list not specific
		/// in the HeaderListDirective snippet.
		/// </summary>
		ChainedListComplexRestrictionSeparator,
		/// <summary>
		/// Used in place of the ChainedListComplexRestrictionSeparator if the
		/// lead role player of a chained list is the same as the previous statement.
		/// Chained lists can collapse the lead role if the list type is listed
		/// in the CollapsibleLeadDirective snippet.
		/// </summary>
		ChainedListComplexRestrictionCollapsedLeadSeparator,
		/// <summary>
		/// The text for a collapsed separator in a chained list. Generally
		/// just a space.
		/// </summary>
		ChainedListCollapsedSeparator,
		/// <summary>
		/// </summary>
		ChainedListClose,
		/// <summary>
		/// </summary>
		NegatedChainedListOpen,
		/// <summary>
		/// </summary>
		NegatedChainedListClose,
		/// <summary>
		/// </summary>
		AndLeadListOpen,
		/// <summary>
		/// </summary>
		AndLeadListSeparator,
		/// <summary>
		/// </summary>
		AndLeadListClose,
		/// <summary>
		/// </summary>
		AndTailListOpen,
		/// <summary>
		/// </summary>
		AndTailListSeparator,
		/// <summary>
		/// </summary>
		AndTailListClose,
		/// <summary>
		/// </summary>
		AndNestedListOpen,
		/// <summary>
		/// </summary>
		AndNestedListSeparator,
		/// <summary>
		/// </summary>
		AndNestedListClose,
		/// <summary>
		/// </summary>
		NegatedAndLeadListOpen,
		/// <summary>
		/// </summary>
		NegatedAndLeadListSeparator,
		/// <summary>
		/// </summary>
		NegatedAndLeadListClose,
		/// <summary>
		/// </summary>
		NegatedAndTailListOpen,
		/// <summary>
		/// </summary>
		NegatedAndTailListSeparator,
		/// <summary>
		/// </summary>
		NegatedAndTailListClose,
		/// <summary>
		/// </summary>
		NegatedAndNestedListOpen,
		/// <summary>
		/// </summary>
		NegatedAndNestedListSeparator,
		/// <summary>
		/// </summary>
		NegatedAndNestedListClose,
		/// <summary>
		/// </summary>
		OrLeadListOpen,
		/// <summary>
		/// </summary>
		OrLeadListSeparator,
		/// <summary>
		/// </summary>
		OrLeadListClose,
		/// <summary>
		/// </summary>
		OrTailListOpen,
		/// <summary>
		/// </summary>
		OrTailListSeparator,
		/// <summary>
		/// </summary>
		OrTailListClose,
		/// <summary>
		/// </summary>
		OrNestedListOpen,
		/// <summary>
		/// </summary>
		OrNestedListSeparator,
		/// <summary>
		/// </summary>
		OrNestedListClose,
		/// <summary>
		/// </summary>
		NegatedOrLeadListOpen,
		/// <summary>
		/// </summary>
		NegatedOrLeadListSeparator,
		/// <summary>
		/// </summary>
		NegatedOrLeadListClose,
		/// <summary>
		/// </summary>
		NegatedOrTailListOpen,
		/// <summary>
		/// </summary>
		NegatedOrTailListSeparator,
		/// <summary>
		/// </summary>
		NegatedOrTailListClose,
		/// <summary>
		/// </summary>
		NegatedOrNestedListOpen,
		/// <summary>
		/// </summary>
		NegatedOrNestedListSeparator,
		/// <summary>
		/// </summary>
		NegatedOrNestedListClose,
		/// <summary>
		/// </summary>
		XorLeadListOpen,
		/// <summary>
		/// </summary>
		XorLeadListSeparator,
		/// <summary>
		/// </summary>
		XorLeadListClose,
		/// <summary>
		/// </summary>
		XorTailListOpen,
		/// <summary>
		/// </summary>
		XorTailListSeparator,
		/// <summary>
		/// </summary>
		XorTailListClose,
		/// <summary>
		/// </summary>
		XorNestedListOpen,
		/// <summary>
		/// </summary>
		XorNestedListSeparator,
		/// <summary>
		/// </summary>
		XorNestedListClose,
		/// <summary>
		/// </summary>
		NegatedXorLeadListOpen,
		/// <summary>
		/// </summary>
		NegatedXorLeadListSeparator,
		/// <summary>
		/// </summary>
		NegatedXorLeadListClose,
		/// <summary>
		/// </summary>
		NegatedXorTailListOpen,
		/// <summary>
		/// </summary>
		NegatedXorTailListSeparator,
		/// <summary>
		/// </summary>
		NegatedXorTailListClose,
		/// <summary>
		/// </summary>
		NegatedXorNestedListOpen,
		/// <summary>
		/// </summary>
		NegatedXorNestedListSeparator,
		/// <summary>
		/// </summary>
		NegatedXorNestedListClose,
		/// <summary>
		/// Duplicate value for the last item in this enum
		/// </summary>
		Last = NegatedXorNestedListClose,
	}
	/// <summary>
	/// Options used with the <see cref="IRolePathRendererContext.RenderAssociatedRolePlayer"/> method.
	/// </summary>
	[Flags]
	public enum RolePathRolePlayerRenderingOptions
	{
		/// <summary>
		/// Render the role player with default subscripting and no quantifier.
		/// </summary>
		None = 0,
		/// <summary>
		/// The role player should be quantified with an existential quantifier or back reference.
		/// </summary>
		Quantify = 1,
		/// <summary>
		/// The role player rendering is being used in the verbalization head. Head variables
		/// are given priority when pairing identities.
		/// </summary>
		UsedInVerbalizationHead = 2,
		/// <summary>
		/// By default, a role player use is subscripted if there is a single variable and
		/// one or more fully existential uses of the same object type. Setting this option
		/// suppresses subscripts in this situation.
		/// </summary>
		MinimizeHeadSubscripting = 4,
	}
	/// <summary>
	/// Provide callback services to the verbalization engine
	/// making requests on the <see cref="IRolePathRenderer"/> interface.
	/// </summary>
	public interface IRolePathRendererContext
	{
		/// <summary>
		/// Render a role player with appropriate subscripting, formatting, and quantification.
		/// </summary>
		/// <param name="rolePlayerFor">The role player this is for.</param>
		/// <param name="hyphenBindingFormatString">The hyphen bound format string for the replacement role. If this
		/// is provided, then there is a single replacement field for the role player, and any additional quantification
		/// should treat the hyphen-bound text as a single unit.</param>
		/// <param name="renderingOptions">Options from the <see cref="RolePathRolePlayerRenderingOptions"/> values.</param>
		/// <returns>String replacement field with formatting and subscripting applied
		/// by the <see cref="IRolePathRenderer.RenderRolePlayer"/></returns>
		string RenderAssociatedRolePlayer(object rolePlayerFor, string hyphenBindingFormatString, RolePathRolePlayerRenderingOptions renderingOptions);
	}
	/// <summary>
	/// Rendering interface for the <see cref="RolePathVerbalizer"/> class
	/// </summary>
	public interface IRolePathRenderer
	{
		/// <summary>
		/// Resolve a <see cref="RolePathVerbalizerSnippetType"/> into a string.
		/// </summary>
		/// <param name="snippet">The <see cref="RolePathVerbalizerSnippetType"/> to resolve.</param>
		/// <returns>The corresponding string.</returns>
		string ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType snippet);
		/// <summary>
		/// Get the <see cref="IFormatProvider"/> for this rendering.
		/// </summary>
		IFormatProvider FormatProvider { get;}
		/// <summary>
		/// Render a role player with the current settings
		/// </summary>
		/// <param name="rolePlayer">The role player. If this is <see langword="null"/>,
		/// then using a string representing a missing role player.</param>
		/// <param name="subscript">The subscript to render to distinguish multiple
		/// uses of the same role player. 0 indicates no subscript.</param>
		/// <param name="fullyExistential"><see langword="true"/> if the result
		/// is used fully existentially, meaning that the element is never referenced.
		/// A fully existential request will always have a subscript of 0.</param>
		/// <returns>Role player replacement.</returns>
		string RenderRolePlayer(ObjectType rolePlayer, int subscript, bool fullyExistential);
		/// <summary>
		/// Get a format string with one replacement field for a <paramref name="factType"/>
		/// that can be used between replacement fields in predicate text parts.
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> the predicate parts are for.</param>
		/// <returns>A format string, or an empty or null string for no decoration.</returns>
		string GetPredicatePartDecorator(FactType factType);
		/// <summary>
		/// Render a value constraint
		/// </summary>
		/// <param name="valueConstraint">The <see cref="PathConditionRoleValueConstraint"/> to render.</param>
		/// <param name="rendererContext">The rendering context. Used to retrieve variable names.</param>
		/// <param name="builder">A <see cref="StringBuilder"/> for scratch strings. Return to original position on exit.</param>
		/// <returns>Formatted string describing the value constraint</returns>
		string RenderValueCondition(PathConditionRoleValueConstraint valueConstraint, IRolePathRendererContext rendererContext, StringBuilder builder);
		/// <summary>
		/// Render a calculation
		/// </summary>
		/// <param name="calculation">The <see cref="CalculatedPathValue"/> to render.</param>
		/// <param name="rendererContext">The rendering context. Used to retrieve variable names.</param>
		/// <param name="builder">A <see cref="StringBuilder"/> for scratch strings. Return
		/// to original position on exit.</param>
		/// <returns>Formatted string of the calculation.</returns>
		string RenderCalculation(CalculatedPathValue calculation, IRolePathRendererContext rendererContext, StringBuilder builder);
		/// <summary>
		/// Render a constant value
		/// </summary>
		/// <param name="constant">The <see cref="PathConstant"/> to render.</param>
		/// <returns>Formatted string with the constant.</returns>
		string RenderConstant(PathConstant constant);
	}
	/// <summary>
	/// Standard implementation of <see cref="IRolePathRenderer"/> based on
	/// a <see cref="CoreVerbalizationSnippetType"/> mapping.
	/// </summary>
	public class StandardRolePathRenderer : IRolePathRenderer
	{
		// UNDONE: RolePathVerbalizerPending Add verbalization generation patterns for this entire
		// class and create with VerbalizationGenerator. Move all snippets into the core snippets.
		#region Member Variables
		private IVerbalizationSets<CoreVerbalizationSnippetType> myCoreSnippets;
		private IFormatProvider myFormatProvider;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new <see cref="StandardRolePathRenderer"/>
		/// </summary>
		/// <param name="coreSnippets">Core verbalization snippets</param>
		/// <param name="formatProvider">Context format provider</param>
		public StandardRolePathRenderer(IVerbalizationSets<CoreVerbalizationSnippetType> coreSnippets, IFormatProvider formatProvider)
		{
			myCoreSnippets = coreSnippets;
			myFormatProvider = formatProvider;
		}
		#endregion // Constructor
		#region IRolePathRenderer Implementation
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.ResolveVerbalizerSnippet"/>
		/// </summary>
		protected string ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType snippet)
		{
			string retVal = null;
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = myCoreSnippets;
			switch (snippet)
			{
				case RolePathVerbalizerSnippetType.HeaderListDirective:
					return "!And !Or Xor !Xor";
				case RolePathVerbalizerSnippetType.CollapsibleLeadDirective:
					return "And Or Chain !Chain";
				case RolePathVerbalizerSnippetType.ImpersonalPronoun:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.ImpersonalPronoun);
				case RolePathVerbalizerSnippetType.PersonalPronoun:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.PersonalPronoun);
				case RolePathVerbalizerSnippetType.ExistentialQuantifier:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, false, false);
				case RolePathVerbalizerSnippetType.NegatedExistentialQuantifier:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.ExistentialQuantifier, false, true);
				case RolePathVerbalizerSnippetType.BackReferenceQuantifier:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.DefiniteArticle, false, false);
				case RolePathVerbalizerSnippetType.ImpersonalIdentityCorrelation:
					return @"{0} <span class=""quantifier""/>that is</span> {1}";
				case RolePathVerbalizerSnippetType.PersonalIdentityCorrelation:
					return @"{0} <span class=""quantifier""/>who is</span> {1}";
				case RolePathVerbalizerSnippetType.HyphenBoundPredicatePart:
					return snippets.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart);
				case RolePathVerbalizerSnippetType.HeadVariableProjection:
					return @"{0} <span class=""logicalOperator"">=</span> {1}";
				case RolePathVerbalizerSnippetType.AggregateParameterScope:
					return @"{0} <span class=""quantifier"">of each</span> {1}";
				case RolePathVerbalizerSnippetType.AggregateSetProjection:
					return @"<span class=""quantifier"">distinct</span> {0}";

				// List management
				case RolePathVerbalizerSnippetType.ListCloseOutdentSnippets:
					return @"NegatedChainedListClose AndTailListClose AndNestedListClose NegatedAndLeadListClose NegatedAndTailListClose NegatedAndNestedListClose OrTailListClose OrNestedListClose NegatedOrLeadListClose NegatedOrTailListClose NegatedOrNestedListClose XorLeadListClose XorTailListClose XorNestedListClose NegatedXorLeadListClose NegatedXorTailListClose NegatedXorNestedListClose";
				case RolePathVerbalizerSnippetType.ChainedListOpen:
					return "";
				case RolePathVerbalizerSnippetType.ChainedListLocalRestrictionSeparator:
					return @" <span class=""quantifier"">where</span> ";
				case RolePathVerbalizerSnippetType.ChainedListComplexRestrictionSeparator:
					return @"<br/><span class=""quantifier"">where</span> ";
				case RolePathVerbalizerSnippetType.ChainedListComplexRestrictionCollapsedLeadSeparator:
					return @"<br/><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.ChainedListCollapsedSeparator:
					return @" ";
				case RolePathVerbalizerSnippetType.ChainedListClose:
					return @"";
				case RolePathVerbalizerSnippetType.NegatedChainedListOpen:
					return @"<span class=""quantifier"">the following does not hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedChainedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.AndLeadListOpen:
					return @"";
				case RolePathVerbalizerSnippetType.AndLeadListSeparator:
					return @"<br/><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.AndLeadListClose:
					return @"";
				case RolePathVerbalizerSnippetType.AndTailListOpen:
					return @"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.AndTailListSeparator:
					return @"<br/><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.AndTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.AndNestedListOpen:
					return @"<span>";
				case RolePathVerbalizerSnippetType.AndNestedListSeparator:
					return @"</span><br/><span class=""smallIndent""><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.AndNestedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedAndLeadListOpen:
					return @"<span class=""quantifier"">one or more of the following do not hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedAndLeadListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedAndLeadListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedAndTailListOpen:
					return @"<span class=""quantifier"">one or more of the following do not hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedAndTailListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedAndTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedAndNestedListOpen:
					return @"<span class=""quantifier"">one or more of the following do not hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedAndNestedListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedAndNestedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.OrLeadListOpen:
					return @"";
				case RolePathVerbalizerSnippetType.OrLeadListSeparator:
					return @"<br/><span class=""quantifier"">or</span> ";
				case RolePathVerbalizerSnippetType.OrLeadListClose:
					return @"";
				case RolePathVerbalizerSnippetType.OrTailListOpen:
					return @"<br/><span class=""smallIndent""><span class=""quantifier"">and</span> ";
				case RolePathVerbalizerSnippetType.OrTailListSeparator:
					return @"<br/><span class=""quantifier"">or</span> ";
				case RolePathVerbalizerSnippetType.OrTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.OrNestedListOpen:
					return @"<span>";
				case RolePathVerbalizerSnippetType.OrNestedListSeparator:
					return @"</span><br/><span class=""smallIndent""><span class=""quantifier"">or</span> ";
				case RolePathVerbalizerSnippetType.OrNestedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedOrLeadListOpen:
					return @"<span class=""quantifier"">none of the following hold::</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedOrLeadListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedOrLeadListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedOrTailListOpen:
					return @"<span class=""quantifier"">none of the following hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedOrTailListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedOrTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedOrNestedListOpen:
					return @"<span class=""quantifier"">none of the following hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedOrNestedListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedOrNestedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.XorLeadListOpen:
					return @"<span class=""quantifier"">exactly one of the following holds:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.XorLeadListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.XorLeadListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.XorTailListOpen:
					return @"<span class=""quantifier"">exactly one of the following holds:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.XorTailListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.XorTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.XorNestedListOpen:
					return @"<span class=""quantifier"">exactly one of the following holds:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.XorNestedListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.XorNestedListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedXorLeadListOpen:
					return @"<span class=""quantifier"">either zero or more than one of the following hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedXorLeadListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedXorLeadListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedXorTailListOpen:
					return @"<span class=""quantifier"">either zero or more than one of the following hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedXorTailListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedXorTailListClose:
					return @"</span>";
				case RolePathVerbalizerSnippetType.NegatedXorNestedListOpen:
					return @"<span class=""quantifier"">either zero or more than one of the following hold:</span><br/><span class=""smallIndent"">";
				case RolePathVerbalizerSnippetType.NegatedXorNestedListSeparator:
					return @"<span class=""listSeparator"">;</span><br/>";
				case RolePathVerbalizerSnippetType.NegatedXorNestedListClose:
					return @"</span>";
			}
			Debug.Assert(retVal != null);
			return retVal;
		}
		string IRolePathRenderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType snippetType)
		{
			return ResolveVerbalizerSnippet(snippetType);
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.FormatProvider"/>
		/// </summary>
		protected IFormatProvider FormatProvider
		{
			get
			{
				return myFormatProvider;
			}
		}
		IFormatProvider IRolePathRenderer.FormatProvider
		{
			get
			{
				return FormatProvider;
			}
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.RenderRolePlayer"/>
		/// </summary>
		protected string RenderRolePlayer(ObjectType rolePlayer, int subscript, bool fullyExistential)
		{
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = myCoreSnippets;
			if (rolePlayer == null)
			{
				if (subscript == 0)
				{
					return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing), "");
				}
				return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeMissing), subscript);
			}
			else
			{
				string idString = rolePlayer.Id.ToString("D");
				if (subscript == 0)
				{
					return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType), rolePlayer.Name, idString);
				}
				return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript), rolePlayer.Name, idString, subscript);
			}
		}
		string IRolePathRenderer.RenderRolePlayer(ObjectType rolePlayer, int subscript, bool fullyExistential)
		{
			return RenderRolePlayer(rolePlayer, subscript, fullyExistential);
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.GetPredicatePartDecorator"/>
		/// </summary>
		protected string GetPredicatePartDecorator(FactType factType)
		{
			return string.Format(myFormatProvider, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.PredicatePart), factType.Name, factType.Id.ToString("D"));
		}
		string IRolePathRenderer.GetPredicatePartDecorator(FactType factType)
		{
			return GetPredicatePartDecorator(factType);
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.RenderValueCondition"/>
		/// </summary>
		protected string RenderValueCondition(PathConditionRoleValueConstraint valueConstraint, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			// UNDONE: Code lifted from generated ValueTypeValueConstraint verbalization. Replace with
			// generated code, along with the result of this class.
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = myCoreSnippets;
			IFormatProvider formatProvider = myFormatProvider;
			int restoreBuilder = builder.Length;
			const bool isNegative = false;
			const bool isDeontic = false;
			LinkedElementCollection<ValueRange> ranges = valueConstraint.ValueRangeCollection;
			bool isSingleValue = ranges.Count == 1 && ranges[0].MinValue == ranges[0].MaxValue;
			bool isText = valueConstraint.IsText;
			CoreVerbalizationSnippetType variableSnippetSnippetType1 = 0;
			if (isSingleValue)
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.SingleValueValueConstraint;
			}
			else
			{
				variableSnippetSnippetType1 = CoreVerbalizationSnippetType.MultiValueValueConstraint;
			}
			string variableSnippetFormat1 = snippets.GetSnippet(variableSnippetSnippetType1, isDeontic, isNegative);
			string variableSnippet1Replace1 = rendererContext.RenderAssociatedRolePlayer(valueConstraint.PathedRole, null, RolePathRolePlayerRenderingOptions.Quantify);
			string variableSnippet1Replace2 = null;
			int rangeCount = ranges.Count;
			for (int i = 0; i < rangeCount; ++i)
			{
				string minValue = ranges[i].MinValue;
				string maxValue = ranges[i].MaxValue;
				RangeInclusion minInclusion = ranges[i].MinInclusion;
				RangeInclusion maxInclusion = ranges[i].MaxInclusion;
				CoreVerbalizationSnippetType listSnippet;
				if (i == 0)
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListOpen;
				}
				else if (i == rangeCount - 1)
				{
					if (i == 1)
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListPairSeparator;
					}
					else
					{
						listSnippet = CoreVerbalizationSnippetType.CompactSimpleListFinalSeparator;
					}
				}
				else
				{
					listSnippet = CoreVerbalizationSnippetType.CompactSimpleListSeparator;
				}
				builder.Append(snippets.GetSnippet(listSnippet, isDeontic, isNegative));
				CoreVerbalizationSnippetType variableSnippet1ReplaceSnippetType2 = 0;
				if (minValue == maxValue)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.SelfReference;
				}
				else if (minInclusion != RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxUnbounded;
				}
				else if (minInclusion == RangeInclusion.Open && maxValue.Length == 0)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxUnbounded;
				}
				else if (minValue.Length == 0 && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxClosed;
				}
				else if (minValue.Length == 0 && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinUnboundedMaxOpen;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxClosed;
				}
				else if (minInclusion != RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinClosedMaxOpen;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion != RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxClosed;
				}
				else if (minInclusion == RangeInclusion.Open && maxInclusion == RangeInclusion.Open)
				{
					variableSnippet1ReplaceSnippetType2 = CoreVerbalizationSnippetType.MinOpenMaxOpen;
				}
				string variableSnippet1ReplaceFormat2 = snippets.GetSnippet(variableSnippet1ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType1 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType1 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat1 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType1, isDeontic, isNegative);
				string variableSnippet1Replace2Replace1Replace1 = null;
				variableSnippet1Replace2Replace1Replace1 = minValue;
				variableSnippet1Replace2Replace1 = string.Format(formatProvider, variableSnippet1Replace2ReplaceFormat1, variableSnippet1Replace2Replace1Replace1);
				string variableSnippet1Replace2Replace2 = null;
				CoreVerbalizationSnippetType variableSnippet1Replace2ReplaceSnippetType2 = 0;
				if (isText)
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.TextInstanceValue;
				}
				else
				{
					variableSnippet1Replace2ReplaceSnippetType2 = CoreVerbalizationSnippetType.NonTextInstanceValue;
				}
				string variableSnippet1Replace2ReplaceFormat2 = snippets.GetSnippet(variableSnippet1Replace2ReplaceSnippetType2, isDeontic, isNegative);
				string variableSnippet1Replace2Replace2Replace1 = null;
				variableSnippet1Replace2Replace2Replace1 = maxValue;
				variableSnippet1Replace2Replace2 = string.Format(formatProvider, variableSnippet1Replace2ReplaceFormat2, variableSnippet1Replace2Replace2Replace1);
				variableSnippet1Replace2 = string.Format(formatProvider, variableSnippet1ReplaceFormat2, variableSnippet1Replace2Replace1, variableSnippet1Replace2Replace2);
				builder.Append(variableSnippet1Replace2);
				if (i == rangeCount - 1)
				{
					builder.Append(snippets.GetSnippet(CoreVerbalizationSnippetType.CompactSimpleListClose, isDeontic, isNegative));
				}
			}
			variableSnippet1Replace2 = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
			builder.Length = restoreBuilder;
			return string.Format(formatProvider, variableSnippetFormat1, variableSnippet1Replace1, variableSnippet1Replace2);
		}
		string IRolePathRenderer.RenderValueCondition(PathConditionRoleValueConstraint valueConstraint, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			return RenderValueCondition(valueConstraint, rendererContext, builder);
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.RenderCalculation"/>
		/// </summary>
		protected string RenderCalculation(CalculatedPathValue calculation, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			int restoreBuilder = builder.Length;
			Function function = calculation.Function;
			PathedRole calculationScope = calculation.Scope;
			FunctionParameter parameter;
			CalculatedPathValueInput input;
			if (function != null)
			{
				LinkedElementCollection<FunctionParameter> parameters = function.ParameterCollection;
				int parameterCount = parameters.Count;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculation.InputCollection;
				int inputCount = inputs.Count;
				string operatorName = function.OperatorSymbol;
				if (!string.IsNullOrEmpty(operatorName) && (parameterCount == 1 || parameterCount == 2))
				{
					// Render as an operator
					if (parameterCount == 1)
					{
						parameter = parameters[0];
						builder.Append(operatorName);
						if (inputCount == 1)
						{
							builder.Append(RenderParameter(inputs[0], parameter, calculationScope, rendererContext, builder));
						}
						else
						{
							builder.Append("[");
							builder.Append(parameter.Name);
							builder.Append("]");
						}
					}
					else
					{
						for (int i = 0; i < 2; ++i)
						{
							parameter = parameters[i];
							int j = 0;
							for (; j < inputCount; ++j)
							{
								input = inputs[j];
								if (input.Parameter == parameter)
								{
									builder.Append(RenderParameter(input, parameter, calculationScope, rendererContext, builder));
									break;
								}
							}
							if (j == inputCount)
							{
								builder.Append("[");
								builder.Append(parameter.Name);
								builder.Append("]");
							}
							if (i == 0)
							{
								builder.Append(" ");
								builder.Append(operatorName);
								builder.Append(" ");
							}
						}
					}
				}
				else
				{
					builder.Append(function.Name);
					builder.Append("(");
					for (int i = 0; i < parameterCount; ++i)
					{
						if (i != 0)
						{
							builder.Append(", ");
						}
						parameter = parameters[i];
						int j = 0;
						for (; j < inputCount; ++j)
						{
							input = inputs[j];
							if (input.Parameter == parameter)
							{
								builder.Append(RenderParameter(input, parameter, calculationScope, rendererContext, builder));
								break;
							}
						}
						if (j == inputCount)
						{
							builder.Append("[");
							builder.Append(parameter.Name);
							builder.Append("]");
						}
					}
					builder.Append(")");
				}
			}
			string result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
			builder.Length = restoreBuilder;
			return result;
		}
		string IRolePathRenderer.RenderCalculation(CalculatedPathValue calculation, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			return RenderCalculation(calculation, rendererContext, builder);
		}
		private string RenderParameter(CalculatedPathValueInput calculatedValueInput, FunctionParameter parameter, PathedRole aggregateScope, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			int restoreBuilder = builder.Length;
			PathedRole sourceRole;
			CalculatedPathValue sourceCalculation;
			PathConstant sourceConstant;
			if (null != (sourceRole = calculatedValueInput.SourcePathedRole))
			{
				builder.Append(rendererContext.RenderAssociatedRolePlayer(sourceRole, null, RolePathRolePlayerRenderingOptions.None));
			}
			else if (null != (sourceCalculation = calculatedValueInput.SourceCalculatedValue))
			{
				builder.Append(RenderCalculation(sourceCalculation, rendererContext, builder));
			}
			else if (null != (sourceConstant = calculatedValueInput.SourceConstant))
			{
				builder.Append(RenderConstant(sourceConstant));
			}
			string result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
			builder.Length = restoreBuilder;
			if (parameter.BagInput)
			{
				string scopeVariable = rendererContext.RenderAssociatedRolePlayer(aggregateScope, null, RolePathRolePlayerRenderingOptions.None);
				if (!string.IsNullOrEmpty(scopeVariable))
				{
					result = string.Format(myFormatProvider, ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.AggregateParameterScope), result, scopeVariable);
				}
				if (calculatedValueInput.DistinctValues)
				{
					result = string.Format(myFormatProvider, ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.AggregateSetProjection), result);
				}
			}
			return result;
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.RenderConstant"/>
		/// </summary>
		string RenderConstant(PathConstant constant)
		{
			return string.Format(myFormatProvider, myCoreSnippets.GetSnippet(CoreVerbalizationSnippetType.NonTextInstanceValue), constant.LexicalValue);
		}
		string IRolePathRenderer.RenderConstant(PathConstant constant)
		{
			return RenderConstant(constant);
		}
		#endregion // IRolePathRenderer Implementation
	}
	/// <summary>
	/// Options for the <see cref="RolePathVerbalizer"/> class
	/// </summary>
	[Flags]
	public enum RolePathVerbalizerOptions
	{
		/// <summary>
		/// No options
		/// </summary>
		None = 0,
		/// <summary>
		/// Add a {0} replacement string at the last location in the
		/// verbalized role path before an outdent operation. Setting
		/// this flag allows a multi-line path verbalization with indentation
		/// be used as part of an outer verbalization structure that places
		/// text after the verbalized path but before a following new line.
		/// The <see cref="RolePathVerbalizer.FormatResolveOutdent"/> method is
		/// provided to help with processing of outdent data.
		/// </summary>
		MarkTrailingOutdentStart,
	}
	/// <summary>
	/// A class to assist in verbalization of a role path
	/// </summary>
	public abstract class RolePathVerbalizer : IRolePathRendererContext
	{
		#region RolePlayerVariable class
		/// <summary>
		/// An object type variable representing a replacement field.
		/// </summary>
		protected class RolePlayerVariable
		{
			[Flags]
			private enum StateFlags
			{
				/// <summary>
				/// No flags set
				/// </summary>
				None = 0,
				/// <summary>
				/// The variable is used outside the join path. External variables
				/// can be explicitly added and correlated with other external variables.
				/// A head variable is an external variable that supports special
				/// quantification settings.
				/// </summary>
				IsExternalVariable = 1,
				/// <summary>
				/// The variable has been marked as a head variable. Head
				/// variables can be treated as used without being marked
				/// as used in the current use phase.
				/// </summary>
				IsHeadVariable = 2,
				/// <summary>
				/// Do not subscript this variable if it is the only one
				/// of its type other than existential uses.
				/// </summary>
				MinimizeHeadSubscripting = 4,
			}
			private StateFlags myFlags;
			private ObjectType myRolePlayer;
			private int mySubscript;
			private int myUsePhase;
			/// <summary>
			/// Create a <see cref="RolePlayerVariable"/> with default settings
			/// </summary>
			/// <param name="rolePlayer">The participating role player</param>
			public RolePlayerVariable(ObjectType rolePlayer)
			{
				myRolePlayer = rolePlayer;
				mySubscript = -1;
			}
			/// <summary>
			/// Get the associated role player
			/// </summary>
			public ObjectType RolePlayer
			{
				get
				{
					return myRolePlayer;
				}
			}
			/// <summary>
			/// Get or set the current subscript. A subscript of -1
			/// indicates that the value has not been initialized. 0
			/// indicates no subscript. Note that subscripts remain
			/// consistent across use phases, so once a subscript
			/// for a variable has been set it does not change.
			/// </summary>
			public int Subscript
			{
				get
				{
					return mySubscript;
				}
				set
				{
					mySubscript = value;
				}
			}
			/// <summary>
			/// Mark a variable as used during a given phase of the verbalization.
			/// </summary>
			/// <param name="usePhase">The use phase to test. The use of a phase enables
			/// a new phase to begin a clean use slate without touching the current data.</param>
			/// <param name="treatHeadVariablesAsUsed">Set to <see langword="true"/> if a head variable should
			/// be treated as used.</param>
			/// <returns>Returns <see langword="true"/> if this the first use of the variable during the specified phase.</returns>
			public bool Use(int usePhase, bool treatHeadVariablesAsUsed)
			{
				if (usePhase != myUsePhase)
				{
					myUsePhase = usePhase;
					if (!treatHeadVariablesAsUsed || 0 == (myFlags & StateFlags.IsHeadVariable))
					{
						return true;
					}
				}
				return false;
			}
			/// <summary>
			/// Test if a variable has been used during a given use phase.
			/// </summary>
			/// <param name="usePhase">A non-repeating cookie value indicating the current use phase.</param>
			/// <param name="treatHeadVariablesAsUsed">Set to <see langword="true"/> if a head variable should
			/// be treated as used.</param>
			/// <returns><see langword="true"/> if the variable has been used.</returns>
			public bool HasBeenUsed(int usePhase, bool treatHeadVariablesAsUsed)
			{
				return usePhase == myUsePhase || treatHeadVariablesAsUsed && 0 != (myFlags & StateFlags.IsHeadVariable);
			}
			/// <summary>
			/// Set if the variable is used externally to the path verbalization.
			/// </summary>
			public bool IsExternalVariable
			{
				get
				{
					return 0 != (myFlags & StateFlags.IsExternalVariable);
				}
				set
				{
					if (value)
					{
						myFlags |= StateFlags.IsExternalVariable;
					}
					else
					{
						myFlags &= ~StateFlags.IsExternalVariable;
					}
				}
			}
			/// <summary>
			/// Set if the variable is used in the head statement that owns this path.
			/// </summary>
			public bool IsHeadVariable
			{
				get
				{
					return 0 != (myFlags & StateFlags.IsHeadVariable);
				}
				set
				{
					if (value)
					{
						myFlags |= StateFlags.IsHeadVariable;
					}
					else
					{
						myFlags &= ~StateFlags.IsHeadVariable;
					}
				}
			}
			/// <summary>
			/// Set if subscripting of a head variable should be optimized to
			/// not produce a subscript for a head variable that is the only
			/// variable of its type and is existentially used.
			/// </summary>
			public bool MinimizeHeadSubscripting
			{
				get
				{
					return (StateFlags.IsHeadVariable | StateFlags.MinimizeHeadSubscripting) == (myFlags & (StateFlags.IsHeadVariable | StateFlags.MinimizeHeadSubscripting));
				}
				set
				{
					if (value)
					{
						myFlags |= StateFlags.MinimizeHeadSubscripting;
					}
					else
					{
						myFlags &= ~StateFlags.MinimizeHeadSubscripting;
					}
				}
			}
		}
		#endregion // RolePlayerVariable class
		#region RelatedRolePlayerVariables struct
		/// <summary>
		/// A structure to enhance a <see cref="RolePlayerVariable"/>
		/// linked list with information on whether the object type
		/// is also used in a fully extensional fashion, meaning that
		/// it is mentioned without being referenced.
		/// </summary>
		private struct RelatedRolePlayerVariables
		{
			private LinkedNode<RolePlayerVariable> myHeadNode;
			private bool myUsedFullyExistentially;
			/// <summary>
			/// Construct a new <see cref="RelatedRolePlayerVariables"/>
			/// </summary>
			/// <param name="rolePlayerVariable">Initial <see cref="RolePlayerVariable"/> to track.</param>
			/// <param name="usedFullyExistentially">Set to <see lanword="true"/> to
			/// indicate a use of the object type that is not referenced. A fully existential
			/// element is never subscripted, but it might force a subscript on another
			/// element of the same type.</param>
			public RelatedRolePlayerVariables(RolePlayerVariable rolePlayerVariable, bool usedFullyExistentially)
			{
				myHeadNode = rolePlayerVariable != null ? new LinkedNode<RolePlayerVariable>(rolePlayerVariable) : null;
				myUsedFullyExistentially = usedFullyExistentially;
			}
			/// <summary>
			/// The head <see cref="ObjectType"/> node. This
			/// can be null if the object type is used fully
			/// existentially only.
			/// </summary>
			public LinkedNode<RolePlayerVariable> HeadRolePlayerVariableNode
			{
				get
				{
					return myHeadNode;
				}
			}
			/// <summary>
			/// Set if this object type is used in a fully existential
			/// fashion without additional references.
			/// </summary>
			public bool UsedFullyExistentially
			{
				get
				{
					return myUsedFullyExistentially;
				}
				set
				{
					myUsedFullyExistentially = value;
				}
			}
			/// <summary>
			/// Add a new <see cref="RolePlayerVariable"/> to the set of related variables.
			/// </summary>
			/// <param name="variable">The <see cref="RolePlayerVariable"/> to add.</param>
			/// <returns>True if the structure was modified</returns>
			public bool AddRolePlayerVariable(RolePlayerVariable variable)
			{
				LinkedNode<RolePlayerVariable> headNode = myHeadNode;
				if (headNode == null)
				{
					myHeadNode = new LinkedNode<RolePlayerVariable>(variable);
					return true;
				}
				headNode.GetTail().SetNext(new LinkedNode<RolePlayerVariable>(variable), ref headNode);
				return false; // Adding to the tail means the structure was not changed.
			}
			/// <summary>
			/// Get a subscript for a <see cref="RolePlayerVariable"/>. Subscripts are determined
			/// based on the request order.
			/// </summary>
			/// <param name="variable">The variable to index.</param>
			/// <returns>The subscript. Subscripts are 1-based. A value of 0 indicates no subscript.</returns>
			public int ReserveSubscript(RolePlayerVariable variable)
			{
				int retVal = variable.Subscript;
				if (retVal == -1) // Not calculated yet, figure it out
				{
					LinkedNode<RolePlayerVariable> node = myHeadNode;
					LinkedNode<RolePlayerVariable> nextNode = node.Next;
					if (nextNode == null)
					{
						// Single node, nothing to search for
						Debug.Assert(variable == node.Value);
						retVal = (myUsedFullyExistentially && !variable.MinimizeHeadSubscripting) ? 1 : 0;
					}
					else
					{
						retVal = Math.Max(node.Value.Subscript, retVal);
						while (nextNode != null)
						{
							retVal = Math.Max(nextNode.Value.Subscript, retVal);
							nextNode = nextNode.Next;
						}
						if (retVal == -1)
						{
							retVal = 0;
						}
						++retVal;
					}
					variable.Subscript = retVal;
				}
				return retVal;
			}
		}
		#endregion // RelatedRolePlayerVariables struct
		#region PathVariableUse struct
		/// <summary>
		/// A structure representing the use of a <see cref="RolePlayerVariable"/>
		/// for a node in a role path. Represents primary, join correlated, and
		/// projection correlated related types.
		/// </summary>
		protected struct RolePlayerVariableUse
		{
			#region Member Variables
			private readonly RolePlayerVariable myPrimaryVariable;
			private readonly PathedRole myCorrelationRoot;
			private RolePlayerVariable myJoinedToVariable;
			private LinkedNode<RolePlayerVariable> myCorrelatedVariables;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a <see cref="RolePlayerVariableUse"/> with a single primary variable
			/// and no related variables.
			/// </summary>
			/// <param name="primaryVariable">The primary variable</param>
			/// <param name="joinedToVariable">If this is joined to a variable that is compatible
			/// with the primary variable but not the same type, then we want to give it verbalization
			/// priority when relating this variable to other correlated variables.</param>
			/// <param name="correlationRoot">The root correlation key if this is instance
			/// is keyed off of another <see cref="PathedRole"/>. Corresponds to the normalized
			/// correlation root used to register a correlated projection variable.</param>
			public RolePlayerVariableUse(RolePlayerVariable primaryVariable, RolePlayerVariable joinedToVariable, PathedRole correlationRoot)
			{
				myPrimaryVariable = primaryVariable;
				myJoinedToVariable = joinedToVariable;
				myCorrelationRoot = correlationRoot;
				myCorrelatedVariables = null;
			}
			#endregion // Constructors
			#region RelatedUsed methods
			/// <summary>
			/// Add a correlated <see cref="RolePlayerVariable"/>. No change is
			/// made if the correlated variable is already used as a primary
			/// or correlated variable.
			/// </summary>
			/// <param name="variable">The variable to add.</param>
			/// <returns><see langword="true"/> if the structure was modified</returns>
			/// <remarks>If the variable use if keyed off a pathed role, then correlated
			/// uses should only be added to the correlation root. If the variable use
			/// is not keyed off a <see cref="PathedRole"/>, then the correlated uses
			/// of the two variable uses unioned with the primary variable should be
			/// the same set.</remarks>
			public bool AddCorrelatedVariable(RolePlayerVariable variable)
			{
				if (myPrimaryVariable == variable)
				{
					return false;
				}
				LinkedNode<RolePlayerVariable> node = myCorrelatedVariables;
				if (node != null)
				{
					LinkedNode<RolePlayerVariable> lastNode = null;
					while (node != null)
					{
						if (node.Value == variable)
						{
							return false;
						}
						lastNode = node;
						node = node.Next;
					}
					lastNode.SetNext(new LinkedNode<RolePlayerVariable>(variable), ref myCorrelatedVariables);
				}
				else
				{
					myCorrelatedVariables = new LinkedNode<RolePlayerVariable>(variable);
				}
				return true;
			}
			#endregion // RelatedUsed methods
			#region Accessor Properties
			/// <summary>
			/// Get the primary variable for this variable use
			/// </summary>
			public RolePlayerVariable PrimaryRolePlayerVariable
			{
				get
				{
					return myPrimaryVariable;
				}
			}
			/// <summary>
			/// Get the first correlated variable
			/// </summary>
			public LinkedNode<RolePlayerVariable> CorrelatedVariablesHead
			{
				get
				{
					return myCorrelatedVariables;
				}
			}
			/// <summary>
			/// Enumerate all variables associated with this variable use
			/// </summary>
			/// <param name="includePrimary"><see langword="true"/> to include the
			/// primary variable, otherwise include the correlation list only.</param>
			/// <returns><see cref="RolePlayerVariable"/> enumeration</returns>
			public IEnumerable<RolePlayerVariable> GetCorrelatedVariables(bool includePrimary)
			{
				RolePlayerVariable variable;
				if (includePrimary && null != (variable = myPrimaryVariable))
				{
					yield return variable;
				}
				LinkedNode<RolePlayerVariable> correlatedVariableNode = myCorrelatedVariables;
				while (correlatedVariableNode != null)
				{
					yield return correlatedVariableNode.Value;
					correlatedVariableNode = correlatedVariableNode.Next;
				}
			}
			/// <summary>
			/// If this use is for a <see cref="PathedRole"/> key, then
			/// retrieve the normalized root correlation <see cref="PathedRole"/>
			/// for this key.
			/// </summary>
			public PathedRole CorrelationRoot
			{
				get
				{
					return myCorrelationRoot;
				}
			}
			/// <summary>
			/// Update the variable directly joined to this one.
			/// </summary>
			public RolePlayerVariable JoinedToVariable
			{
				get
				{
					return myJoinedToVariable;
				}
				set
				{
					myJoinedToVariable = value;
				}
			}
			#endregion // Accessor Properties
		}
		#endregion // PathVariableUse struct
		#region RolePathCache struct
		/// <summary>
		/// Helper structure to maintain role path collections
		/// and structure so that we do not need to constantly
		/// reretrieve them to look forwards and backwards in
		/// the path.
		/// </summary>
		private struct RolePathCache
		{
			#region PathInfo struct
			private struct PathInfo
			{
				public readonly ReadOnlyCollection<PathedRole> PathedRoles;
				public readonly LinkedElementCollection<RoleSubPath> SplitPaths;
				public readonly RolePath ParentPath;
				public PathInfo(RolePath path)
				{
					// We'll need all of the fields eventually, get them all up front
					PathedRoles = path.PathedRoleCollection;
					SplitPaths = path.SplitPathCollection;
					RoleSubPath subPath = path as RoleSubPath;
					ParentPath = subPath != null ? subPath.ParentRolePath : null;
				}
			}
			#endregion // PathInfo struct
			#region Cache Initialization
			private Dictionary<RolePath, PathInfo> myCache;
			public static RolePathCache InitializeCache()
			{
				return new RolePathCache(new Dictionary<RolePath, PathInfo>());
			}
			private RolePathCache(Dictionary<RolePath, PathInfo> cache)
			{
				myCache = cache;
			}
			#endregion // Cache Initialization
			#region Accessors
			private PathInfo GetPathInfo(RolePath rolePath)
			{
				PathInfo retVal;
				if (!myCache.TryGetValue(rolePath, out retVal))
				{
					retVal = new PathInfo(rolePath);
					myCache.Add(rolePath, retVal);
				}
				return retVal;
			}
			/// <summary>
			/// Get the cached <see cref="PathedRole"/>s for a given <see cref="RolePath"/>
			/// </summary>
			public ReadOnlyCollection<PathedRole> PathedRoleCollection(RolePath rolePath)
			{
				return GetPathInfo(rolePath).PathedRoles;
			}
			/// <summary>
			/// Get the cached <see cref="RoleSubPath"/> split paths for a given <see cref="RolePath"/>
			/// </summary>
			public LinkedElementCollection<RoleSubPath> SplitPathCollection(RolePath rolePath)
			{
				return GetPathInfo(rolePath).SplitPaths;
			}
			/// <summary>
			/// Get the cached parent <see cref="RolePath"/> for a given <see cref="RolePath"/>,
			/// or null for a root path.
			/// </summary>
			public RolePath ParentRolePath(RolePath rolePath)
			{
				return GetPathInfo(rolePath).ParentPath;
			}
			/// <summary>
			/// Enumerate all pathed roles preceding a specified role
			/// </summary>
			/// <param name="startRole">The initial <see cref="PathedRole"/></param>
			/// <param name="visitStartRole"><see langword="true"/> if the <paramref name="startRole"/> should be
			/// enumerated.</param>
			/// <returns>Enumeration</returns>
			public IEnumerable<PathedRole> GetPrecedingPathedRoles(PathedRole startRole, bool visitStartRole)
			{
				PathInfo pathInfo = GetPathInfo(startRole.RolePath);
				ReadOnlyCollection<PathedRole> pathedRoles = pathInfo.PathedRoles;
				int pathedRoleIndex = (pathedRoles.Count == 1 ? 0 : pathedRoles.IndexOf(startRole)) - (visitStartRole ? 0 : 1);
				for (;;)
				{
					for (int i = pathedRoleIndex; i >= 0; --i)
					{
						yield return pathedRoles[i];
					}
					RolePath parentPath = pathInfo.ParentPath;
					if (parentPath == null)
					{
						break;
					}
					pathInfo = GetPathInfo(parentPath);
					pathedRoles = pathInfo.PathedRoles;
					pathedRoleIndex = pathedRoles.Count - 1;
				}
			}
			/// <summary>
			/// Determine the upper-most <see cref="PathedRole"/> that is implicitly or
			/// explicitly correlated with the provided <paramref name="pathedRole"/>.
			/// If the role has a <see cref="PathedRole.PathedRolePurpose"/> of
			/// <see cref="PathedRolePurpose.StartRole"/>, then return the initial
			/// start role in the path.
			/// </summary>
			/// <param name="pathedRole">A <see cref="PathedRole"/> to resolve.</param>
			/// <returns>The input pathed role, or the pathed role at the top of
			/// the implicit and explicit correlation chain for the role.</returns>
			public PathedRole GetCorrelationRootPathedRole(PathedRole pathedRole)
			{
				PathedRole retVal = pathedRole;
				bool checkExplicitCorrelation = false;
				switch (pathedRole.PathedRolePurpose)
				{
					case PathedRolePurpose.StartRole:
						// Find the first
						retVal = GetInitialStartRole(pathedRole);
						break;
					case PathedRolePurpose.SameFactType:
						checkExplicitCorrelation = true;
						break;
					case PathedRolePurpose.PostInnerJoin:
					case PathedRolePurpose.PostOuterJoin:
						foreach (PathedRole precedingPathedRole in GetPrecedingPathedRoles(pathedRole, false))
						{
							PathedRolePurpose purpose = precedingPathedRole.PathedRolePurpose;
							if (purpose == PathedRolePurpose.StartRole)
							{
								retVal = GetInitialStartRole(precedingPathedRole);
								break;
							}
							else if (purpose == PathedRolePurpose.SameFactType)
							{
								retVal = precedingPathedRole;
								checkExplicitCorrelation = true;
								break;
							}
						}
						break;
				}
				if (checkExplicitCorrelation)
				{
					PathedRole explicitParent = pathedRole.CorrelatingParent;
					while (explicitParent != null)
					{
						retVal = explicitParent;
						explicitParent = retVal.CorrelatingParent;
					}
				}
				return retVal;
			}
			/// <summary>
			/// Find the first start role in the lead path for the specified <see cref="PathedRole"/>
			/// </summary>
			public PathedRole GetInitialStartRole(PathedRole pathedRole)
			{
				RolePath rolePath = pathedRole.RolePath;
				PathInfo pathInfo = GetPathInfo(rolePath);
				RolePath parentPath = pathInfo.ParentPath;
				while (parentPath != null)
				{
					pathInfo = GetPathInfo(parentPath);
					parentPath = pathInfo.ParentPath;
				}
				return GetInitialStartRole(pathInfo);
			}
			/// <summary>
			/// Find the first start role in a <see cref="LeadRolePath"/>
			/// </summary>
			public PathedRole GetInitialStartRole(LeadRolePath leadRolePath)
			{
				return GetInitialStartRole(GetPathInfo(leadRolePath));
			}
			private PathedRole GetInitialStartRole(PathInfo pathInfo)
			{
				foreach (PathedRole pathedRole in pathInfo.PathedRoles)
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.StartRole)
					{
						return pathedRole;
					}
				}
				foreach (RoleSubPath subPath in pathInfo.SplitPaths)
				{
					PathedRole pathedRole = GetInitialStartRole(GetPathInfo(subPath));
					if (pathedRole != null)
					{
						return pathedRole;
					}
				}
				return null;
			}
			/// <summary>
			/// 
			/// </summary>
			public bool IsInitialized
			{
				get
				{
					return myCache != null;
				}
			}
			#endregion // Accessors
		}
		#endregion // RolePathCache struct
		#region CorrelatedVariablePairing struct
		/// <summary>
		/// A structure representing two variables that have
		/// been related with a verbalization phrase. Structure
		/// equality is symmetric, so {A,B} is equivalent to {B,A}
		/// </summary>
		private struct CorrelatedVariablePairing
		{
			#region Public Fields and Constructor
			public readonly RolePlayerVariable Variable1;
			public readonly RolePlayerVariable Variable2;
			public CorrelatedVariablePairing(RolePlayerVariable variable1, RolePlayerVariable variable2)
			{
				Variable1 = variable1;
				Variable2 = variable2;
			}
			#endregion // Public Fields and Constructor
			#region Equality overrides
			/// <summary>
			/// Equals operator override
			/// </summary>
			public static bool operator ==(CorrelatedVariablePairing pairing1, CorrelatedVariablePairing pairing2)
			{
				return pairing1.Equals(pairing2);
			}
			/// <summary>
			/// Not equals operator override
			/// </summary>
			public static bool operator !=(CorrelatedVariablePairing pairing1, CorrelatedVariablePairing pairing2)
			{
				return !(pairing1.Equals(pairing2));
			}
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				return (obj is CorrelatedVariablePairing) ? Equals((CorrelatedVariablePairing)obj) : false;
			}
			/// <summary>
			/// Typed Equals method, make equality symmetric
			/// </summary>
			public bool Equals(CorrelatedVariablePairing obj)
			{
				return (Variable1 == obj.Variable1 && Variable2 == obj.Variable2) ||
					(Variable1 == obj.Variable2 && Variable2 == obj.Variable1);
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override int GetHashCode()
			{
				// Hash for symmetry, assume two variables are different
				return Variable1.GetHashCode() ^ Variable2.GetHashCode();
			}
			#endregion // Equality overrides
		}
		#endregion // CorrelatedPair struct
		#region Verbalization plan
		#region VerbalizationPlanNodeType enum
		/// <summary>
		/// Specify the type of verbalization node
		/// </summary>
		private enum VerbalizationPlanNodeType
		{
			/// <summary>
			/// Data describes a <see cref="FactType"/> and the corresponding
			/// <see cref="PathedRole"/> that represents the entry into that
			/// a use of that FactType in the role path.
			/// </summary>
			FactType,
			/// <summary>
			/// An evaluation of a conditional function
			/// </summary>
			CalculatedCondition,
			/// <summary>
			/// A value constraint on a pathed role
			/// </summary>
			ValueConstraint,
			/// <summary>
			/// A project of a head variable onto a calculated value
			/// </summary>
			HeadCalculatedValueProjection,
			/// <summary>
			/// A project of a head variable onto a constant value
			/// </summary>
			HeadConstantProjection,
			/// <summary>
			/// A branching node, data is a LinkedList of child nodes
			/// </summary>
			Branch,
			/// <summary>
			/// A node used to indicate the start of a role path where
			/// the root variable is not directly referenced by any roles
			/// in the path. Later uses of correlated variables will
			/// the context floating root variable.
			/// </summary>
			FloatingRootVariableContext,
			/// <summary>
			/// A root variable declaration chained before other
			/// fact type nodes.
			/// </summary>
			ChainedRootVariable,
		}
		#endregion // VerbalizationPlanNodeType enum
		#region VerbalizationPlanBranchType enum
		private enum VerbalizationPlanBranchType
		{
			/// <summary>
			/// Not a branch node
			/// </summary>
			None,
			/// <summary>
			/// A flat list of elements with no implied branching.
			/// </summary>
			Chain,
			/// <summary>
			/// A list of chained items where the first item is negated
			/// </summary>
			NegatedChain,
			/// <summary>
			/// An and split
			/// </summary>
			AndSplit,
			/// <summary>
			/// An or split
			/// </summary>
			OrSplit,
			/// <summary>
			/// An xor split
			/// </summary>
			XorSplit,
			/// <summary>
			/// A negated and split
			/// </summary>
			NegatedAndSplit,
			/// <summary>
			/// A negated or split
			/// </summary>
			NegatedOrSplit,
			/// <summary>
			/// A negated xor split
			/// </summary>
			NegatedXorSplit,
		}
		#endregion // VerbalizationPlanBranchType enum
		#region VerbalizationPlanReadingOptions enum
		/// <summary>
		/// Options to specify if the reading associated with
		/// a fact type node can be collapsed in any way.
		/// </summary>
		[Flags]
		private enum VerbalizationPlanReadingOptions
		{
			/// <summary>
			/// No options
			/// </summary>
			None = 0,
			/// <summary>
			/// Completely eliminate the first role during role replacement.
			/// Used during branch conditions when a reading is found that
			/// matches the lead role player for the lead fact type in the
			/// previous branch. Set during the reading resolution phase.
			/// </summary>
			FullyCollapseFirstRole = 1,
			/// <summary>
			/// Use a personal or impersonal back referencing pronoun in place
			/// of the first role. Used for a joined fact type where the previous
			/// reading ends with the role player name. Set during the reading
			/// resolution phase.
			/// </summary>
			BackReferenceFirstRole = 2,
			/// <summary>
			/// Use a negated existential quantifier for the non-entry role.
			/// Set during path execution if the first fact type in a negated
			/// chain is a binary fact type with an opposite role player with
			/// an associated variable that has not been used.
			/// </summary>
			NegatedExitRole = 4,
			/// <summary>
			/// Set during fact type analysis to determine if it is possible
			/// that conditions might be met while the path is being verbalized
			/// to apply the NegatedExitRole option. If the opposite negated
			/// role is fully existential, then the first flag can be set
			/// when the readings are bound.
			/// </summary>
			DynamicNegatedExitRole = 8,
		}
		#endregion // VerbalizationPlanReadingOptions enum
		#region VerbalizationPlanBranchCorrelationStyle enum
		/// <summary>
		/// Specify the list rendering style. Controls variable
		/// combination options based on whether items in the list
		/// share state with their parent and sibling variables.
		/// </summary>
		private enum VerbalizationPlanBranchRenderingStyle
		{
			/// <summary>
			/// The list style uses an operator between
			/// different items in a list. Each statement
			/// is treated as part of the larger block, so
			/// variable subtype correlations and other
			/// cross-statement correlation does not need to
			/// be repeated. For example, 'A or B'
			/// </summary>
			OperatorSeparated = 0,
			/// <summary>
			/// The list style uses a header to describe the
			/// contents of a list. Each individual statement
			/// in the list is treated as a standalone block
			/// that is related to items in the parent context,
			/// but not other blocks in the list. For example,
			/// 'At least one of the following holds: A;B'
			/// </summary>
			HeaderList = 1,
			/// <summary>
			/// Each statement in a list is completely standalone
			/// and does not depend on items specified in other lists.
			/// Used for top-level items that share head variables but
			/// nothing else.
			/// </summary>
			IsolatedList = 2,
		}
		#endregion // VerbalizationPlanBranchRenderingStyle enum
		#region VerbalizationPlanNode class
		private abstract class VerbalizationPlanNode
		{
			#region Member Variables and Constructor
			private VerbalizationPlanNode myParentNode;
			private VerbalizationPlanNode(VerbalizationPlanNode parentNode)
			{
				if (parentNode != null)
				{
					myParentNode = parentNode;
					LinkedNode<VerbalizationPlanNode> currentHead = parentNode.FirstChildNode;
					LinkedNode<VerbalizationPlanNode> newNode = new LinkedNode<VerbalizationPlanNode>(this);
					if (currentHead == null)
					{
						parentNode.FirstChildNode = newNode;
					}
					else
					{
						LinkedNode<VerbalizationPlanNode> newHead = currentHead;
						currentHead.GetTail().SetNext(newNode, ref newHead);
						if (newHead != currentHead)
						{
							parentNode.FirstChildNode = newHead;
						}
					}
				}
			}
			#endregion // Member Variables and Constructor
			#region Node Creation Methods
			/// <summary>
			/// Create and attach a new fact type node.
			/// </summary>
			/// <param name="factTypeEntry">The <see cref="FactType"/> for this node.</param>
			/// <param name="factType">The <see cref="PathedRole"/> that is the first in the <paramref name="FactType"/>.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddFactTypeEntryNode(FactType factType, PathedRole factTypeEntry, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new FactTypeNode(parentNode, factType, factTypeEntry);
			}
			/// <summary>
			/// Create and attach a new branching node.
			/// </summary>
			/// <param name="branchType">The <see cref="VerbalizationPlanBranchType"/> of the new branch.</param>
			/// <param name="renderingStyle">The <see cref="VerbalizationPlanBranchRenderingStyle"/> of the new branch.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddBranchNode(VerbalizationPlanBranchType branchType, VerbalizationPlanBranchRenderingStyle renderingStyle, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				VerbalizationPlanNode newNode = new BranchNode(parentNode, branchType, renderingStyle);
				if (parentNode == null)
				{
					rootNode = newNode;
				}
				return newNode;
			}
			/// <summary>
			/// Create and attach a new value constraint node.
			/// </summary>
			/// <param name="valueConstraint">The <see cref="PathConditionRoleValueConstraint"/> to add.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddValueConstraintNode(PathConditionRoleValueConstraint valueConstraint, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new ValueConstraintNode(parentNode, valueConstraint);
			}
			/// <summary>
			/// Create and attach a new calculated condition node.
			/// </summary>
			/// <param name="calculatedCondition">The <see cref="CalculatedPathValue"/> to add.</param>
			/// <param name="restrictsSingleFactType">Is this condition node being added immediately after a  fact
			/// type instance where all pathed role inputs to the function are contained in that instance?</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddCalculatedConditionNode(CalculatedPathValue calculatedCondition, bool restrictsSingleFactType, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new CalculatedConditionNode(parentNode, calculatedCondition, restrictsSingleFactType);
			}
			/// <summary>
			/// Create and attach a new projection calculation node.
			/// </summary>
			/// <param name="headVariableKey">The projection key.</param>
			/// <param name="calculation">The projected <see cref="CalculatedPathValue"/> to add.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddProjectedCalculationNode(object headVariableKey, CalculatedPathValue calculation, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new ProjectedCalculationNode(parentNode, headVariableKey, calculation);
			}
			/// <summary>
			/// Create and attach a new projection calculation node.
			/// </summary>
			/// <param name="headVariableKey">The projection key.</param>
			/// <param name="constant">The projected <see cref="PathConstant"/> to add.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddProjectedConstantNode(object headVariableKey, PathConstant constant, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new ProjectedConstantNode(parentNode, headVariableKey, constant);
			}
			/// <summary>
			/// Create and attach a new chained root variable node.
			/// </summary>
			/// <param name="rootVariable">The root variable for a path</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddChainedRootVariableNode(RolePlayerVariable rootVariable, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref rootNode);
				}
				return new ChainedRootVariableNode(parentNode, rootVariable);
			}
			/// <summary>
			/// Create and attach a new floating root variable node and inject it into
			/// the verbalization plan tree. Floating root nodes are inserted after all
			/// analysis for the tree is complete.
			/// </summary>
			/// <param name="floatingRootVariable">The floating root variable.</param>
			/// <param name="childNode">The child node that should be in scope for this root.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>The new node with the floating point context.</returns>
			public static VerbalizationPlanNode InsertFloatingRootVariableNode(RolePlayerVariable floatingRootVariable, VerbalizationPlanNode childNode, ref VerbalizationPlanNode rootNode)
			{
				VerbalizationPlanNode newNode = null;
				if (childNode != null)
				{
					VerbalizationPlanNode parentNode = childNode.ParentNode;
					newNode = new FloatingRootVariableNode(parentNode, floatingRootVariable, childNode);
					childNode.myParentNode = newNode;
					if (parentNode == null)
					{
						rootNode = newNode;
					}
					else
					{
						LinkedNode<VerbalizationPlanNode> childNodeLink = parentNode.FirstChildNode;
						while (childNodeLink != null)
						{
							if (childNodeLink.Value == childNode)
							{
								childNodeLink.Value = newNode;
								break;
							}
							childNodeLink = childNodeLink.Next;
						}
					}
					return newNode;
				}
				return null;
			}
			#endregion // Node Creation Methods
			#region Accessors Properties
			/// <summary>
			/// Get the <see cref="VerbalizationPlanNodeType"/> for this node.
			/// </summary>
			public abstract VerbalizationPlanNodeType NodeType { get;}
			/// <summary>
			/// The parent node
			/// </summary>
			public VerbalizationPlanNode ParentNode
			{
				get
				{
					return myParentNode;
				}
			}
			/// <summary>
			/// The first child node, appropriate for a branch node
			/// </summary>
			public virtual LinkedNode<VerbalizationPlanNode> FirstChildNode
			{
				get
				{
					return null;
				}
				set
				{
				}
			}
			/// <summary>
			/// The branch type of a branch node
			/// </summary>
			public virtual VerbalizationPlanBranchType BranchType
			{
				get
				{
					return VerbalizationPlanBranchType.None;
				}
			}
			/// <summary>
			/// Get the branch rendering style. Applies to a branch node.
			/// </summary>
			public virtual VerbalizationPlanBranchRenderingStyle BranchRenderingStyle
			{
				get
				{
					return VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
				}
			}
			/// <summary>
			/// Get the entry <see cref="FactType"/> for a fact type node.
			/// </summary>
			public virtual FactType FactType
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// The reading options associated with a fact type node.
			/// </summary>
			public virtual VerbalizationPlanReadingOptions ReadingOptions
			{
				get
				{
					return VerbalizationPlanReadingOptions.None;
				}
				set
				{
				}
			}
			/// <summary>
			/// The <see cref="IReading"/> associated with a fact type node.
			/// </summary>
			public virtual IReading Reading
			{
				get
				{
					return null;
				}
				set
				{
				}
			}
			/// <summary>
			/// Get the entry <see cref="PathedRole"/> of a fact type node.
			/// </summary>
			public virtual PathedRole FactTypeEntry
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Get the <see cref="PathConditionRoleValueConstraint"/> for
			/// a value constraint node.
			/// </summary>
			public virtual PathConditionRoleValueConstraint ValueConstraint
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Get the <see cref="CalculatedPathValue"/> for a calculated
			/// condition node or calculated head variable projection.
			/// </summary>
			public virtual CalculatedPathValue Calculation
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Get the <see cref="PathConstant"/> for a constant head
			/// variable projection.
			/// </summary>
			public virtual PathConstant Constant
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Get the head variable key for a calculated or
			/// constant head variable projection.
			/// </summary>
			public virtual object HeadVariableKey
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Test if a node is a restriction limited to values in the
			/// previous fact type node.
			/// </summary>
			public virtual bool RestrictsPreviousFactType
			{
				get
				{
					return false;
				}
			}
			/// <summary>
			/// The root variable for a floating root variable context or
			/// a chained root variable.
			/// </summary>
			public virtual RolePlayerVariable RootVariable
			{
				get
				{
					return null;
				}
			}
			#endregion // Accessor Properties
			#region Node Management Methods
			/// <summary>
			/// Replace a child node with its child nodes
			/// </summary>
			/// <param name="childNode">The node to collapse.</param>
			public virtual void CollapseChildNode(VerbalizationPlanNode childNode)
			{
				// Empty implementation, implemented in BranchNode
			}
			/// <summary>
			/// Replace a child node with its child nodes
			/// </summary>
			/// <param name="childNodeLink">The node to collapse.</param>
			public virtual void CollapseChildNode(LinkedNode<VerbalizationPlanNode> childNodeLink)
			{
				// Empty implementation, implemented in BranchNode
			}
			#endregion // Node Management Methods
			#region Node type specific types
			private sealed class FactTypeNode : VerbalizationPlanNode
			{
				private readonly FactType myFactType;
				private readonly PathedRole myEntryPathedRole;
				private IReading myReading;
				private VerbalizationPlanReadingOptions myOptions;
				public FactTypeNode(VerbalizationPlanNode parentNode, FactType factType, PathedRole factTypeEntryRole)
					: base(parentNode)
				{
					myFactType = factType;
					myEntryPathedRole = factTypeEntryRole;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.FactType; }
				}
				public override FactType FactType
				{
					get
					{
						return myFactType;
					}
				}
				public override PathedRole FactTypeEntry
				{
					get
					{
						return myEntryPathedRole;
					}
				}
				public override IReading Reading
				{
					get
					{
						return myReading;
					}
					set
					{
						myReading = value;
					}
				}
				public override VerbalizationPlanReadingOptions ReadingOptions
				{
					get
					{
						return myOptions;
					}
					set
					{
						myOptions = value;
					}
				}
			}
			private sealed class BranchNode : VerbalizationPlanNode
			{
				private const int BranchTypeMask = 0xffff;
				private const int IsolatedRenderingBit = 0x10000;
				private const int HeaderListRenderingBit = 0x20000;
				private readonly int mySettings;
				private LinkedNode<VerbalizationPlanNode> myChildNodes;
				public BranchNode(VerbalizationPlanNode parentNode, VerbalizationPlanBranchType branchType, VerbalizationPlanBranchRenderingStyle renderingStyle)
					: base(parentNode)
				{
					int settings = (int)branchType;
					switch (renderingStyle)
					{
						case VerbalizationPlanBranchRenderingStyle.HeaderList:
							settings |= HeaderListRenderingBit;
							break;
						case VerbalizationPlanBranchRenderingStyle.IsolatedList:
							settings |= IsolatedRenderingBit;
							break;
					}
					mySettings = settings;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.Branch; }
				}
				public override LinkedNode<VerbalizationPlanNode> FirstChildNode
				{
					get
					{
						return myChildNodes;
					}
					set
					{
						myChildNodes = value;
					}
				}
				public override void CollapseChildNode(VerbalizationPlanNode childNode)
				{
					LinkedNode<VerbalizationPlanNode> currentLinkNode = FirstChildNode;
					while (currentLinkNode != null)
					{
						if (currentLinkNode.Value == childNode)
						{
							CollapseChildNode(currentLinkNode);
							break;
						}
						currentLinkNode = currentLinkNode.Next;
					}
				}
				public override void CollapseChildNode(LinkedNode<VerbalizationPlanNode> childNodeLink)
				{
					if (childNodeLink == null)
					{
						return;
					}
					LinkedNode<VerbalizationPlanNode> headChildNode = myChildNodes;
					LinkedNode<VerbalizationPlanNode> previousLinkNode = childNodeLink.Previous;
					LinkedNode<VerbalizationPlanNode> replacementNodes = childNodeLink.Value.FirstChildNode;
					childNodeLink.Detach(ref headChildNode);
					if (replacementNodes != null)
					{
						// Reparent while the replacement linked list is isolated
						LinkedNode<VerbalizationPlanNode> currentLinkNode = replacementNodes;
						while (currentLinkNode != null)
						{
							currentLinkNode.Value.myParentNode = this;
							currentLinkNode = currentLinkNode.Next;
						}

						if (previousLinkNode != null)
						{
							previousLinkNode.SetNext(replacementNodes, ref headChildNode);
						}
						else
						{
							LinkedNode<VerbalizationPlanNode> oldHead = headChildNode;
							headChildNode = replacementNodes;
							if (oldHead != null)
							{
								headChildNode.GetTail().SetNext(oldHead, ref headChildNode);
							}
						}
					}
					myChildNodes = headChildNode;
				}
				public override VerbalizationPlanBranchType BranchType
				{
					get
					{
						return (VerbalizationPlanBranchType)(mySettings & BranchTypeMask);
					}
				}
				public override VerbalizationPlanBranchRenderingStyle BranchRenderingStyle
				{
					get
					{
						int settings = mySettings;
						if (0 != (settings & HeaderListRenderingBit))
						{
							return VerbalizationPlanBranchRenderingStyle.HeaderList;
						}
						else if (0 != (settings & IsolatedRenderingBit))
						{
							return VerbalizationPlanBranchRenderingStyle.IsolatedList;
						}
						return VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
					}
				}
			}
			private sealed class ValueConstraintNode : VerbalizationPlanNode
			{
				private readonly PathConditionRoleValueConstraint myValueConstraint;
				public ValueConstraintNode(VerbalizationPlanNode parentNode, PathConditionRoleValueConstraint valueConstraint)
					: base(parentNode)
				{
					myValueConstraint = valueConstraint;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.ValueConstraint; }
				}
				public override bool RestrictsPreviousFactType
				{
					get
					{
						// All value constraint nodes are created with the fact
						// type use they restrict and therefore restrict the
						// previous fact type
						return true;
					}
				}
				public override PathConditionRoleValueConstraint ValueConstraint
				{
					get
					{
						return myValueConstraint;
					}
				}
			}
			private sealed class CalculatedConditionNode : VerbalizationPlanNode
			{
				private readonly CalculatedPathValue myCondition;
				private readonly bool myRestrictsSingleFactType;
				public CalculatedConditionNode(VerbalizationPlanNode parentNode, CalculatedPathValue condition, bool restrictsSingleFactType)
					: base(parentNode)
				{
					myCondition = condition;
					myRestrictsSingleFactType = restrictsSingleFactType;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.CalculatedCondition; }
				}
				public override CalculatedPathValue Calculation
				{
					get
					{
						return myCondition;
					}
				}
				public override bool RestrictsPreviousFactType
				{
					get
					{
						// A calculated condition node that restricts a single fact type
						// is verbalized immediately after the fact type instance.
						return myRestrictsSingleFactType;
					}
				}
			}
			private sealed class ProjectedCalculationNode : VerbalizationPlanNode
			{
				private readonly CalculatedPathValue myCalculation;
				private readonly object myHeadVariableKey;
				public ProjectedCalculationNode(VerbalizationPlanNode parentNode, object headVariableKey, CalculatedPathValue calculation)
					: base(parentNode)
				{
					myCalculation = calculation;
					myHeadVariableKey = headVariableKey;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.HeadCalculatedValueProjection; }
				}
				public override CalculatedPathValue Calculation
				{
					get
					{
						return myCalculation;
					}
				}
				public override object HeadVariableKey
				{
					get
					{
						return myHeadVariableKey;
					}
				}
			}
			private sealed class ProjectedConstantNode : VerbalizationPlanNode
			{
				private readonly PathConstant myConstant;
				private readonly object myHeadVariableKey;
				public ProjectedConstantNode(VerbalizationPlanNode parentNode, object headVariableKey, PathConstant constant)
					: base(parentNode)
				{
					myConstant = constant;
					myHeadVariableKey = headVariableKey;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get { return VerbalizationPlanNodeType.HeadConstantProjection; }
				}
				public override PathConstant Constant
				{
					get
					{
						return myConstant;
					}
				}
				public override object HeadVariableKey
				{
					get
					{
						return myHeadVariableKey;
					}
				}
			}
			private sealed class FloatingRootVariableNode : VerbalizationPlanNode
			{
				private RolePlayerVariable myFloatingRootVariable;
				private LinkedNode<VerbalizationPlanNode> myChildNodeLink;
				public FloatingRootVariableNode(VerbalizationPlanNode parentNode, RolePlayerVariable floatingRootVariable, VerbalizationPlanNode childNode)
					: base(parentNode)
				{
					myFloatingRootVariable = floatingRootVariable;
					// Note that we only need to store a single child here, but
					// the child list makes this consistent with a branch node
					// and easier to integrate with other parts of the code.
					myChildNodeLink = new LinkedNode<VerbalizationPlanNode>(childNode);
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get
					{
						return VerbalizationPlanNodeType.FloatingRootVariableContext;
					}
				}
				/// <summary>
				/// Get the floating root variable for this context
				/// </summary>
				public override RolePlayerVariable RootVariable
				{
					get
					{
						return myFloatingRootVariable;
					}
				}
				/// <summary>
				/// Get the first (and only) child node
				/// </summary>
				public override LinkedNode<VerbalizationPlanNode> FirstChildNode
				{
					get
					{
						return myChildNodeLink;
					}
					set
					{
						myChildNodeLink = value;
					}
				}
			}
			private sealed class ChainedRootVariableNode : VerbalizationPlanNode
			{
				private RolePlayerVariable myRootVariable;
				public ChainedRootVariableNode(VerbalizationPlanNode parentNode, RolePlayerVariable rootVariable)
					: base(parentNode)
				{
					myRootVariable = rootVariable;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get
					{
						return VerbalizationPlanNodeType.ChainedRootVariable;
					}
				}
				/// <summary>
				/// Get the root variable for this context
				/// </summary>
				public override RolePlayerVariable RootVariable
				{
					get
					{
						return myRootVariable;
					}
				}
			}
			#endregion // Node type specific types
		}
		#endregion // VerbalizationPlanNode class
		#endregion // Verbalization plan
		#region Member Variables
		/// <summary>
		/// Track uses of a path role player variable. The key can be any equatable object.
		/// </summary>
		private Dictionary<object, RolePlayerVariableUse> myUseToVariableMap;
		/// <summary>
		/// Track path role players by object type.
		/// </summary>
		private Dictionary<ObjectType, RelatedRolePlayerVariables> myObjectTypeToVariableMap;
		/// <summary>
		/// Track the same information as with myObjectTypeToVariableMap for missing role players.
		/// The result is that missing role players get variables and are subscripted.
		/// </summary>
		private RelatedRolePlayerVariables? myMissingRolePlayerVariables;
		/// <summary>
		/// Role path structure cache
		/// </summary>
		private RolePathCache myRolePathCache;
		/// <summary>
		/// The latest requested use phase.
		/// </summary>
		private int myLatestUsePhase;
		/// <summary>
		/// Track multiple active use phases so that we can repeat
		/// variable use and pairings in list-style branches, where
		/// separate detached statements are used that require variables
		/// to be re-paired. Note that existential quantification and
		/// backreferencing is tracked based on the quantification use
		/// phase, which will always be the first element in this list.
		/// </summary>
		private List<int> myUsePhases;
		/// <summary>
		/// Track the use phase when a <see cref="CorrelatedVariablePairing"/> was last
		/// applied.
		/// </summary>
		private Dictionary<CorrelatedVariablePairing, int> myCorrelatedVariablePairing;
		/// <summary>
		/// A dictionary to track correlations between external variable declarations. External variables
		/// are added externally to the core pathing system and cannot be correlated via a role path
		/// because a path may not exist, or the external variables can span multiple paths which can't
		/// be correlated with a path-specific correlation root. There is no natural head correlation
		/// for a list of external correlations, so all elements in a list are a key that points to
		/// that list.
		/// </summary>
		private Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> myCorrelatedExternalVariables;
		/// <summary>
		/// The root node in the verbalization plan
		/// </summary>
		private VerbalizationPlanNode myRootPlanNode;
		/// <summary>
		/// The current branch node, used during path initialization
		/// </summary>
		private VerbalizationPlanNode myCurrentBranchNode;
		/// <summary>
		/// Single path owner, used in place of instantiating myPathOwnerToVerbalizationPlanMap
		/// for a single path owner.
		/// </summary>
		private RolePathOwner mySingleRolePathOwner;
		/// <summary>
		/// Support for tracking verbalization plans for multiple
		/// <see cref="RolePathOwner"/> instances using the same
		/// variables. Populated if <see cref="InitializeRolePathOwner"/>
		/// is called multiple times for the same verbalization helper.
		/// </summary>
		private Dictionary<RolePathOwner, VerbalizationPlanNode> myPathOwnerToVerbalizationPlanMap;
		/// <summary>
		/// Rendering callbacks
		/// </summary>
		private IRolePathRenderer myRenderer;
		/// <summary>
		/// Options controlling verbalization
		/// </summary>
		private RolePathVerbalizerOptions myOptions;
		/// <summary>
		/// Used during path rendering to track a floating
		/// root variable that is not naturally used in the path.
		/// </summary>
		private RolePlayerVariable myFloatingRootVariable;
		/// <summary>
		/// A bit per branch type to determine which branch types
		/// render using an isolated list. Initialized on demand.
		/// </summary>
		private int myHeaderListBranchingBits;
		/// <summary>
		/// A bit per branch type to determine which branch types
		/// render allow collapsing lead roles. Initialized on demand.
		/// </summary>
		private int myCollapsibleLeadBranchingBits;
		/// <summary>
		/// Bits to track which snippets result in an outdent operation.
		/// Enables trailing outdent tracking so that text on the same
		/// line as the end of a complex path verbalization maintains
		/// the correct outdent level. Initialized on demand.
		/// </summary>
		private BitTracker myOutdentSnippetBits;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/>
		/// </summary>
		private RolePathVerbalizer(IRolePathRenderer rolePathRenderer)
		{
			// We will assume these two elements are always used, even when the
			// outer construct ends up with no role paths. In this degenerate case,
			// the role path verbalizer is also used for generating subscripts, pairing,
			// and quantifying head variables.
			myUseToVariableMap = new Dictionary<object, RolePlayerVariableUse>();
			myObjectTypeToVariableMap = new Dictionary<ObjectType, RelatedRolePlayerVariables>();
			myRenderer = rolePathRenderer;
			myHeaderListBranchingBits = -1;
			myCollapsibleLeadBranchingBits = -1;
			myOutdentSnippetBits = new BitTracker(0);
			// A use phase of 1 instead of 0 eliminates the need for an
			// explicit call to BeginVerbalization for the first verbalization pass
			myLatestUsePhase = 1;
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// Get and set current <see cref="RolePathVerbalizerOptions"/> settings for the path verbalizer.
		/// </summary>
		public RolePathVerbalizerOptions Options
		{
			get
			{
				return myOptions;
			}
			set
			{
				myOptions = value;
			}
		}
		#endregion // Accessor Properties
		#region Analysis Methods
		/// <summary>
		/// Initialize verbalization information for a single <see cref="RolePathOwner"/>.
		/// </summary>
		/// <param name="pathOwner">The role path to verbalize.</param>
		protected void InitializeRolePathOwner(RolePathOwner pathOwner)
		{
			// If we've been called previously, then catalog the previous
			// results by the previous owner.
			RolePathOwner previousPathOwner = mySingleRolePathOwner;
			VerbalizationPlanNode previousRootNode = myRootPlanNode;

			// Reset the per-owner member variables
			myRootPlanNode = null;
			myCurrentBranchNode = null;
			mySingleRolePathOwner = null;

			// Populate paths for this owner
			AddPathProjections(pathOwner);
			LeadRolePath simplePath = pathOwner.SingleLeadRolePath;
			if (simplePath != null)
			{
				InitializeRolePath(simplePath);
			}
			else
			{
				bool first = true;
				foreach (RolePathComponent component in pathOwner.PathComponentCollection)
				{
					LeadRolePath rolePath;
					if (null != (rolePath = component as LeadRolePath))
					{
						if (first)
						{
							first = false;
							myCurrentBranchNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.OrSplit, VerbalizationPlanBranchRenderingStyle.IsolatedList, null, ref myRootPlanNode);
						}
						InitializeRolePath(rolePath);
					}
					// UNDONE: RolePathCombination verbalization
				}
			}
			VerbalizationPlanNode newRootNode = myRootPlanNode;
			if (newRootNode != null)
			{
				RolePlayerVariable contextLeadVariable = null;
				RolePlayerVariable contextTrailingVariable = null;
				ResolveReadings(newRootNode, null, false, ref contextLeadVariable, ref contextTrailingVariable);

				Dictionary<RolePathOwner, VerbalizationPlanNode> planMap = myPathOwnerToVerbalizationPlanMap;
				if (planMap != null)
				{
					planMap.Add(pathOwner, newRootNode);
					myRootPlanNode = null;
				}
				else if (previousRootNode == null)
				{
					mySingleRolePathOwner = pathOwner;
				}
				else
				{
					myPathOwnerToVerbalizationPlanMap = planMap = new Dictionary<RolePathOwner, VerbalizationPlanNode>();
					planMap.Add(previousPathOwner, previousRootNode);
					planMap.Add(pathOwner, newRootNode);
					myRootPlanNode = null;
				}
			}
			else if (previousRootNode != null)
			{
				// We didn't get any new verbalization plan for this owner,
				// restore previous state.
				myRootPlanNode = previousRootNode;
				mySingleRolePathOwner = previousPathOwner;
			}
			// Use phases are used during both initialization and rendering. Make
			// sure a use phase is passed so that we don't see quantified elements
			// as a side effect of initialization.
			BeginQuantificationUsePhase();
		}
		private void InitializeRolePath(LeadRolePath leadRolePath)
		{
			LinkedElementCollection<CalculatedPathValue> pathConditions = leadRolePath.CalculatedConditionCollection;
			int pathConditionCount = pathConditions.Count;
			BitTracker processedPathConditions = new BitTracker(pathConditionCount);
			Stack<LinkedElementCollection<RoleBase>> factTypeRolesStack = new Stack<LinkedElementCollection<RoleBase>>();
			BitTracker roleUseTracker = new BitTracker(0);
			int roleUseBaseIndex = -1;
			int resolvedRoleIndex;
			RolePathCache rolePathCache = EnsureRolePathCache();
			RolePlayerVariable rootObjectTypeVariable = RegisterRolePlayerUse(leadRolePath.RootObjectType, null, leadRolePath, rolePathCache.GetInitialStartRole(leadRolePath));
			++myLatestUsePhase;
			PathedRole pendingContext = null;
			ReadOnlyCollection<PathedRole> pendingPathedRoles = null;
			int pendingPathedRoleIndex = -1;
			PathedRole pendingForSameFactType = null;
			// A list (acting like a stack) to get the full history of the parent pathed roles.
			List<ReadOnlyCollection<PathedRole>> contextPathedRoles = null;
			PushConditionalChainNode();
			if (VerbalizeRootObjectType)
			{
				myCurrentBranchNode = VerbalizationPlanNode.AddChainedRootVariableNode(rootObjectTypeVariable, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
				rootObjectTypeVariable = null; // Make sure this doesn't get used as a floating root node below
			}
			VisitRolePathParts(
				leadRolePath,
				null,
				delegate(PathedRole currentPathedRole, RolePath currentPath, ReadOnlyCollection<PathedRole> currentPathedRoles, int currentPathedRoleIndex, PathedRole contextPathedRole, bool unwinding)
				{
					if (currentPathedRole != null)
					{
						PathedRolePurpose purpose = currentPathedRole.PathedRolePurpose;
						bool sameFactType = purpose == PathedRolePurpose.SameFactType;
						Role currentRole;
						if (unwinding)
						{
							if (pendingForSameFactType != null)
							{
								Debug.Assert(pendingForSameFactType == currentPathedRole);
								// We reached a leaf same fact type node. Treat it as fully existential
								// if it is not used for any other purpose.
								pendingForSameFactType = null;
								pendingContext = null;
								pendingPathedRoles = null;
								currentRole = currentPathedRole.Role;
								if (IsPathedRoleReferencedOutsidePath(currentPathedRole))
								{
									RegisterRolePlayerUse(currentRole.RolePlayer, null, currentPathedRole, currentPathedRole);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
								}
							}
							if (!sameFactType)
							{
								// Unwind the stack
								roleUseBaseIndex = PopFactType(factTypeRolesStack, ref roleUseTracker);
								if (currentPathedRole.IsNegated)
								{
									PopNegatedChainNode();
								}
							}
							if (currentPathedRoleIndex == 0)
							{
								// Remove and collapse the chain node added to support multiple elements
								// created by a path section.
								PopConditionalChainNode();
							}
						}
						else
						{
							if (currentPathedRoleIndex == 0)
							{
								// Push a chain node so that we can add multiple elements if needed
								PushConditionalChainNode();
							}
							if (sameFactType)
							{
								if (currentPathedRoleIndex == 0)
								{
									#region Lead same fact type condition processing
									// Get the chain node we just pushed
									VerbalizationPlanNode contextChainNode = myCurrentBranchNode;

									// Determine how many lead same fact type roles to process
									int currentRoleCount = currentPathedRoles.Count;
									for (int i = 1; i < currentRoleCount; ++i)
									{
										if (currentPathedRoles[i].PathedRolePurpose != PathedRolePurpose.SameFactType)
										{
											// Restrict the role count
											currentRoleCount = i;
											break;
										}
									}

									// Get value constraints for roles in this section of the path
									for (int i = 0; i < currentRoleCount; ++i)
									{
										PathedRole processPathedRole = currentPathedRoles[i];
										// UNDONE: RolePathCombination, check combination-specific value constraints
										PathConditionRoleValueConstraint valueConstraint = processPathedRole.DirectValueConstraint;
										if (valueConstraint != null)
										{
											VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, contextChainNode, ref myRootPlanNode);
										}
									}

									// Look for functions that use roles restricted to those roles used
									// up to this point in the context fact type entry and this section.
									if (pathConditionCount != 0)
									{
										int contextSectionsCount = contextPathedRoles != null ? contextPathedRoles.Count : 0;
										for (int i = 0; i < pathConditionCount; ++i)
										{
											if (!processedPathConditions[i])
											{
												CalculatedPathValue calculation = pathConditions[i];
												bool? isLocal = IsLocalCalculatedValue(
													calculation,
													delegate(PathedRole testPathedRole)
													{
														// Look in this section
														for (int j = 0; j < currentRoleCount; ++j)
														{
															if (currentPathedRoles[j] == testPathedRole)
															{
																return true;
															}
														}
														// Look in context sections
														for (int j = contextSectionsCount - 1; j >= 0; --j)
														{
															ReadOnlyCollection<PathedRole> contextRoles = contextPathedRoles[j];
															int k = contextRoles.Count - 1;
															for (; k >= 0; --k)
															{
																PathedRole checkPathedRole = contextRoles[k];
																if (checkPathedRole == testPathedRole)
																{
																	// Note that we include entry roles in the check,
																	// we just don't go any further.
																	return true;
																}
																if (checkPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
																{
																	break;
																}
															}
															if (k >= 0)
															{
																// We broke before the beginning of the context section, stop looking
																break;
															}
														}
														return false;
													});
												if (isLocal.GetValueOrDefault(false))
												{
													// Although this is a single fact type, it occurs as part of a split
													// after the fact type has been defined, so the fact type is not immediately
													// before this one and we can't set the restrictsSingleFactType parameter to true.
													VerbalizationPlanNode.AddCalculatedConditionNode(calculation, false, contextChainNode, ref myRootPlanNode);
												}
											}
										}
									}
									#endregion // Lead same fact type condition processing
								}
								if (null != pendingForSameFactType)
								{
									// The previous same fact type role was not used for a join,
									// make sure it is used for something else before assigning
									// a variable to it.
									if (IsPathedRoleReferencedOutsidePath(pendingForSameFactType))
									{
										currentRole = pendingForSameFactType.Role;
										RegisterRolePlayerUse(currentRole.RolePlayer, null, pendingForSameFactType, pendingForSameFactType);
										resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
										if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
										{
											roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
										}
									}
								}
								pendingForSameFactType = currentPathedRole;
								pendingContext = contextPathedRole;
								pendingPathedRoles = currentPathedRoles;
								pendingPathedRoleIndex = currentPathedRoleIndex;
							}
							else
							{
								if (pendingForSameFactType != null)
								{
									// We're joining to a new fact type, so the same fact type role
									// is definitely in use and needs a variable regardless of whether
									// it is used for anything else. This section affects variables,
									// but not the verbalization plan.
									currentRole = pendingForSameFactType.Role;
									RegisterRolePlayerUse(currentRole.RolePlayer, null, pendingForSameFactType, pendingForSameFactType);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
									pendingForSameFactType = null;
									pendingPathedRoles = null;
									pendingContext = null;
								}
								currentRole = currentPathedRole.Role;
								RegisterFactTypeEntryRolePlayerUse(currentPathedRole, contextPathedRole, leadRolePath);
								if (currentPathedRole.IsNegated)
								{
									PushNegatedChainNode();
								}
								roleUseBaseIndex = PushFactType(ResolvePathedEntryRoleFactType(currentPathedRole, currentPath, currentPathedRoles, currentPathedRoleIndex), currentPathedRole, currentPathedRoles, currentPathedRoleIndex, factTypeRolesStack, ref roleUseTracker, pathConditions, ref processedPathConditions);
								resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
								if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
								{
									roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
								}
							}
						}
					}
					else if (currentPath != null)
					{
						// Note that we ignore any pending same fact type role here. These
						// roles affect variables, but not the verbalization plan, so we
						// can branch the verbalization plan without processing pending
						// same fact type roles.
						if (unwinding)
						{
							contextPathedRoles.RemoveAt(contextPathedRoles.Count - 1);
							PopSplit(currentPath);
						}
						else
						{
							(contextPathedRoles ?? (contextPathedRoles = new List<ReadOnlyCollection<PathedRole>>())).Add(currentPathedRoles);
							PushSplit(currentPath);
						}
					}
				});

			// Chain unprocessed condition nodes at the end
			if (pathConditionCount != 0)
			{
				VerbalizationPlanNode contextChainNode = null;
				for (int i = 0; i < pathConditionCount; ++i)
				{
					if (!processedPathConditions[i])
					{
						if (contextChainNode == null)
						{
							contextChainNode = myCurrentBranchNode;
							if (contextChainNode == null || contextChainNode.BranchType != VerbalizationPlanBranchType.Chain)
							{
								// Don't bother to change myCurrentBranchNode, we'd just need to change it
								// back when the loop finishes.
								contextChainNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, contextChainNode, ref myRootPlanNode);
							}
						}
						VerbalizationPlanNode.AddCalculatedConditionNode(pathConditions[i], false, contextChainNode, ref myRootPlanNode);
					}
				}
			}
			AddCalculatedAndConstantProjections(leadRolePath);
			PopConditionalChainNode();
			if (rootObjectTypeVariable != null &&
				!rootObjectTypeVariable.HasBeenUsed(myLatestUsePhase, false) &&
				myCurrentBranchNode != null)
			{
				myCurrentBranchNode = VerbalizationPlanNode.InsertFloatingRootVariableNode(rootObjectTypeVariable, myCurrentBranchNode, ref myRootPlanNode);
			}
			++myLatestUsePhase;
		}
		private RolePathCache EnsureRolePathCache()
		{
			RolePathCache retVal = myRolePathCache;
			if (!retVal.IsInitialized)
			{
				myRolePathCache = retVal = RolePathCache.InitializeCache();
			}
			return retVal;
		}
		/// <summary>
		/// Determine the <see cref="VerbalizationPlanBranchRenderingStyle"/> from a
		/// <see cref="VerbalizationPlanBranchType"/> branch type. The settings here
		/// are based on context-provided snippet to enable different renderings of
		/// the same branching operation. Note that <see cref="VerbalizationPlanBranchType.Chain">Chain</see>
		/// and <see cref="VerbalizationPlanBranchType.NegatedChain">NegatedChain</see> branch types
		/// are not dynamic and are always given a rendering style of <see cref="VerbalizationPlanBranchRenderingStyle.OperatorSeparated"/>.
		/// </summary>
		private VerbalizationPlanBranchRenderingStyle GetRenderingStyleFromBranchType(VerbalizationPlanBranchType branchType)
		{
			switch (branchType)
			{
				case VerbalizationPlanBranchType.None:
				case VerbalizationPlanBranchType.NegatedChain:
				case VerbalizationPlanBranchType.Chain:
					// Chains are always operator separated. Headers on negated chains are handled
					// specially to support inline negation.
					return VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
			}
			int headerListBits = myHeaderListBranchingBits;
			if (headerListBits == -1)
			{
				#region Translate directive snippet to bits
				headerListBits = 0;
				string[] headerSplitStrings = myRenderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.HeaderListDirective).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
				if (headerSplitStrings != null)
				{
					Type enumType = typeof(VerbalizationPlanBranchType);
					for (int i = 0; i < headerSplitStrings.Length; ++i)
					{
						string directiveString = headerSplitStrings[i];
						if (directiveString[0] == '!')
						{
							directiveString = "Negated" + directiveString.Substring(1) + "Split";
						}
						else
						{
							directiveString += "Split";
						}
						object result = null;
						try
						{
							result = Enum.Parse(enumType, directiveString, true);
						}
						catch (ArgumentException)
						{
							// Swallow it
						}
						if (result != null)
						{
							headerListBits |= (1 << ((int)(VerbalizationPlanBranchType)result - 1));
						}
					}
				}
				myHeaderListBranchingBits = headerListBits;
				#endregion // Translate directive snippet to bits
			}
			return (0 != (headerListBits & (1 << ((int)branchType - 1)))) ? VerbalizationPlanBranchRenderingStyle.HeaderList : VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
		}
		/// <summary>
		/// Determine the <see cref="VerbalizationPlanBranchRenderingStyle"/> from a
		/// <see cref="VerbalizationPlanBranchType"/> branch type. The settings here
		/// are based on context-provided snippet to enable different renderings of
		/// the same branching operation. Note that <see cref="VerbalizationPlanBranchType.Chain">Chain</see>
		/// and <see cref="VerbalizationPlanBranchType.NegatedChain">NegatedChain</see> branch types
		/// are not dynamic and are always given a rendering style of <see cref="VerbalizationPlanBranchRenderingStyle.OperatorSeparated"/>.
		/// </summary>
		private bool GetCollapsibleLeadAllowedFromBranchType(VerbalizationPlanBranchType branchType)
		{
			if (branchType == VerbalizationPlanBranchType.None)
			{
				return false;
			}
			int collapsibleLeadBits = myCollapsibleLeadBranchingBits;
			if (collapsibleLeadBits == -1)
			{
				#region Translate directive snippet to bits
				collapsibleLeadBits = 0;
				string[] collapsibleLeadStrings = myRenderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.CollapsibleLeadDirective).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
				if (collapsibleLeadStrings != null)
				{
					Type enumType = typeof(VerbalizationPlanBranchType);
					for (int i = 0; i < collapsibleLeadStrings.Length; ++i)
					{
						string directiveString = collapsibleLeadStrings[i];
						if (directiveString[0] == '!')
						{
							if (0 == string.Compare(directiveString, "!Chain", StringComparison.InvariantCultureIgnoreCase))
							{
								directiveString = "NegatedChain";
							}
							else
							{
								directiveString = "Negated" + directiveString.Substring(1) + "Split";
							}
						}
						else if (0 != string.Compare(directiveString, "Chain", StringComparison.InvariantCultureIgnoreCase))
						{
							directiveString += "Split";
						}
						object result = null;
						try
						{
							result = Enum.Parse(enumType, directiveString, true);
						}
						catch (ArgumentException)
						{
							// Swallow it
						}
						if (result != null)
						{
							collapsibleLeadBits |= (1 << ((int)(VerbalizationPlanBranchType)result - 1));
						}
					}
				}
				myCollapsibleLeadBranchingBits = collapsibleLeadBits;
				#endregion // Translate directive snippet to bits
			}
			return 0 != (collapsibleLeadBits & (1 << ((int)branchType - 1)));
		}
		/// <summary>
		/// Return true if this is a splitting branch as opposed to a chained branch.
		/// </summary>
		/// <param name="branchType">The <see cref="VerbalizationPlanBranchType"/> to test.</param>
		/// <returns><see langword="false"/> for chain or undefined branch types, <see langword="true"/> otherwise.</returns>
		private bool BranchSplits(VerbalizationPlanBranchType branchType)
		{
			switch (branchType)
			{
				case VerbalizationPlanBranchType.None:
				case VerbalizationPlanBranchType.NegatedChain:
				case VerbalizationPlanBranchType.Chain:
					return false;
			}
			return true;
		}
		/// <summary>
		/// Determine if a specific <see cref="RolePathVerbalizerSnippetType"/>
		/// value is marked as an outdent bit by the dyamic <see cref="RolePathVerbalizerSnippetType.ListCloseOutdentSnippets"/>
		/// snippet.
		/// </summary>
		private bool IsOutdentSnippet(RolePathVerbalizerSnippetType snippetType)
		{
			BitTracker tracker = myOutdentSnippetBits;
			if (tracker.Count == 0)
			{
				#region Translate outdent snippet to bits
				tracker.Resize((int)RolePathVerbalizerSnippetType.Last + 1);
				string[] outdentSnippets = myRenderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.ListCloseOutdentSnippets).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
				if (outdentSnippets != null)
				{
					Type enumType = typeof(RolePathVerbalizerSnippetType);
					for (int i = 0; i < outdentSnippets.Length; ++i)
					{
						string outdentSnippetName = outdentSnippets[i];
						object result = null;
						try
						{
							result = Enum.Parse(enumType, outdentSnippets[i], true);
						}
						catch (ArgumentException)
						{
							// Swallow it
						}
						if (result != null)
						{
							tracker[(int)(RolePathVerbalizerSnippetType)result] = true;
						}
					}
				}
				myOutdentSnippetBits = tracker;
				#endregion // Translate outdent snippet to bits
			}
			return tracker[(int)snippetType];
		}
		private void ResolveReadings(VerbalizationPlanNode verbalizationNode, LinkedNode<VerbalizationPlanNode> verbalizationNodeLink, bool canCollapseLead, ref RolePlayerVariable contextLeadVariable, ref RolePlayerVariable contextTrailingVariable)
		{
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			switch (verbalizationNode.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					FactType factType = verbalizationNode.FactType;
					PathedRole factTypeEntry = verbalizationNode.FactTypeEntry;
					RoleBase entryRoleBase = ResolveRoleBaseInFactType(factTypeEntry.Role, factType);
					RolePlayerVariable localContextLeadVariable = contextLeadVariable;
					RolePlayerVariable localContextTrailingVariable = contextTrailingVariable;
					// UNDONE: RolePathVerbalizerPending Do we want to get smarter on correlation here, or
					// just attempt to check across exact types?
					RolePlayerVariable entryVariable = GetRolePlayerVariableUse(factTypeEntry).Value.PrimaryRolePlayerVariable;
					LinkedElementCollection<ReadingOrder> readingOrders = factType.ReadingOrderCollection;
					bool matchedContextLead = false;
					bool matchedContextTrailing = false;
					IReading reading = null;
					if (localContextLeadVariable != null)
					{
						// Continue with another reading starting with the lead role of the
						// preceding fact type.
						if (localContextLeadVariable == entryVariable)
						{
							reading = factType.GetMatchingReading(readingOrders, null, entryRoleBase, null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound);
						}
						else
						{
							VisitPathedRolesForFactTypeEntry(
								factTypeEntry,
								delegate(PathedRole testPathedRole)
								{
									if (GetRolePlayerVariableUse(testPathedRole).Value.PrimaryRolePlayerVariable == localContextLeadVariable)
									{
										reading = factType.GetMatchingReading(readingOrders, null, ResolveRoleBaseInFactType(testPathedRole.Role, factType), null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound);
										return false;
									}
									return true;
								});
						}
						matchedContextLead = reading != null;
					}
					if (!matchedContextLead && localContextTrailingVariable != null)
					{
						if (localContextTrailingVariable == entryVariable)
						{
							reading = factType.GetMatchingReading(readingOrders, null, entryRoleBase, null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound);
						}
						else
						{
							VisitPathedRolesForFactTypeEntry(
								factTypeEntry,
								delegate(PathedRole testPathedRole)
								{
									if (GetRolePlayerVariableUse(testPathedRole).Value.PrimaryRolePlayerVariable == localContextTrailingVariable)
									{
										reading = factType.GetMatchingReading(readingOrders, null, ResolveRoleBaseInFactType(testPathedRole.Role, factType), null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound);
										return false;
									}
									return true;
								});
						}
						matchedContextTrailing = reading != null;
					}
					if (reading == null)
					{
						// Fall back on any reading for the current entry, or a mocked-up reading if no others are available
						reading = factType.GetMatchingReading(readingOrders, null, entryRoleBase, null, null, MatchingReadingOptions.AllowAnyOrder) ?? factType.GetDefaultReading();
					}

					// Determine lead and trailing variables
					VerbalizationPlanReadingOptions options = VerbalizationPlanReadingOptions.None;
					IList<RoleBase> roles = reading.RoleCollection;
					if (canCollapseLead)
					{
						if (localContextLeadVariable != null)
						{
							if (matchedContextLead)
							{
								options |= VerbalizationPlanReadingOptions.FullyCollapseFirstRole;
							}
							else
							{
								contextLeadVariable = null;
							}
						}
						if (contextLeadVariable == null)
						{
							RoleBase firstRoleBase = roles[0];
							if (reading.Text.StartsWith("{0}") && !VerbalizationHyphenBinder.IsHyphenBound(reading, firstRoleBase))
							{
								Role findRole = firstRoleBase.Role;
								// Find the corresponding variable in the fact type entry
								VisitPathedRolesForFactTypeEntry(
									factTypeEntry,
									delegate(PathedRole testPathedRole)
									{
										if (testPathedRole.Role == findRole)
										{
											localContextLeadVariable = GetRolePlayerVariableUse(testPathedRole).Value.PrimaryRolePlayerVariable;
											return false;
										}
										return true;
									});
								contextLeadVariable = localContextLeadVariable;
							}
						}
					}
					else
					{
						contextLeadVariable = null;
					}

					if (matchedContextTrailing)
					{
						options |= VerbalizationPlanReadingOptions.BackReferenceFirstRole;
					}
					
					// Get the trailing information for the next step
					int roleCount = roles.Count;
					localContextTrailingVariable = null;
					PathedRole oppositePathedRole = null;
					VerbalizationPlanNode parentNode;
					bool checkOppositeNegation =
						roleCount == 2 &&
						roles[0] == factTypeEntry.Role &&
						verbalizationNodeLink != null &&
						verbalizationNodeLink.Previous == null &&
						null != (parentNode = verbalizationNode.ParentNode) &&
						parentNode.BranchType == VerbalizationPlanBranchType.NegatedChain;
					bool hasTrailingRolePlayer = roleCount > 1 && reading.Text.EndsWith("{" + (roleCount - 1).ToString(CultureInfo.InvariantCulture) + "}");
					if (checkOppositeNegation || hasTrailingRolePlayer)
					{
						Role findRole = roles[roleCount - 1].Role;
						VisitPathedRolesForFactTypeEntry(
							factTypeEntry,
							delegate(PathedRole testPathedRole)
							{
								if (testPathedRole.Role == findRole)
								{
									oppositePathedRole = testPathedRole;
									if (hasTrailingRolePlayer)
									{
										localContextTrailingVariable = GetRolePlayerVariableUse(testPathedRole).Value.PrimaryRolePlayerVariable;
									}
									return false;
								}
								return true;
							});
					}
					contextTrailingVariable = localContextTrailingVariable;

					// Check the possibility for using a negated existential quantifier
					// on the second role.
					if (checkOppositeNegation)
					{
						if (oppositePathedRole != null)
						{
							if (oppositePathedRole.RolePath == factTypeEntry.RolePath) // Make sure there is no intervening split
							{
								options |= VerbalizationPlanReadingOptions.DynamicNegatedExitRole;
							}
						}
						else
						{
							// Fully existential case, always use the negated article
							options |= VerbalizationPlanReadingOptions.NegatedExitRole;
						}
					}
					verbalizationNode.Reading = reading;
					verbalizationNode.ReadingOptions = options;
					break;
				case VerbalizationPlanNodeType.Branch:
					VerbalizationPlanBranchType branchType = verbalizationNode.BranchType;
					bool childCanCollapseLead = GetCollapsibleLeadAllowedFromBranchType(branchType);
					bool splitBlocksTrailingVariable = BranchSplits(branchType);
					contextTrailingVariable = null;
					RolePlayerVariable startContextLeadVariable;
					if (childCanCollapseLead)
					{
						startContextLeadVariable = contextLeadVariable;
					}
					else
					{
						contextLeadVariable = startContextLeadVariable = null;
					}
					childNodeLink = verbalizationNode.FirstChildNode;
					while (childNodeLink != null)
					{
						if (splitBlocksTrailingVariable)
						{
							contextTrailingVariable = null;
						}
						ResolveReadings(childNodeLink.Value, childNodeLink, childCanCollapseLead, ref contextLeadVariable, ref contextTrailingVariable);
						childNodeLink = childNodeLink.Next;
					}
					if (startContextLeadVariable != contextLeadVariable)
					{
						contextLeadVariable = null;
					}
					break;
				case VerbalizationPlanNodeType.HeadCalculatedValueProjection:
				case VerbalizationPlanNodeType.HeadConstantProjection:
				case VerbalizationPlanNodeType.CalculatedCondition:
				case VerbalizationPlanNodeType.ValueConstraint:
					if (!verbalizationNode.RestrictsPreviousFactType)
					{
						// Allow restriction conditions without losing the lead
						contextLeadVariable = null;
					}
					contextTrailingVariable = null;
					break;
				case VerbalizationPlanNodeType.ChainedRootVariable:
					contextLeadVariable = null;
					contextTrailingVariable = verbalizationNode.RootVariable;
					break;
				case VerbalizationPlanNodeType.FloatingRootVariableContext:
					childNodeLink = verbalizationNode.FirstChildNode;
					// There will always be exactly one child node link at this point
					ResolveReadings(childNodeLink.Value, childNodeLink, canCollapseLead, ref contextLeadVariable, ref contextTrailingVariable);
					break;
			}
		}
		/// <summary>
		/// Determine the index of a <see cref="Role"/> in a list of fact type
		/// roles. If the role is not found, try to find the proxy.
		/// </summary>
		private static int ResolveRoleIndex(LinkedElementCollection<RoleBase> factTypeRoles, Role role)
		{
			int retVal = factTypeRoles.IndexOf(role);
			RoleProxy proxy;
			if (retVal == -1 &&
				null != (proxy = role.Proxy))
			{
				retVal = factTypeRoles.IndexOf(proxy);
			}
			return retVal;
		}
		/// <summary>
		/// Given a <see cref="Role"/> and <see cref="FactType"/>, determine
		/// the corresponding <see cref="RoleBase"/> that is either in the
		/// normal or implied fact type.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to resolve.</param>
		/// <param name="factType">The <see cref="FactType"/> to get the returned <see cref="RoleBase"/> in.</param>
		/// <returns>The resolved <see cref="RoleBase"/></returns>
		private static RoleBase ResolveRoleBaseInFactType(Role role, FactType factType)
		{
			if (role.FactType != factType)
			{
				RoleProxy proxy = role.Proxy;
				if (proxy != null && proxy.FactType == factType)
				{
					return proxy;
				}
			}
			return role;
		}
		/// <summary>
		/// Helper method to determine if an entry role is associated with
		/// an objectified fact type or the corresponding link fact type.
		/// </summary>
		private FactType ResolvePathedEntryRoleFactType(PathedRole pathedRole, RolePath rolePath, ReadOnlyCollection<PathedRole> pathedRoles, int pathedRoleIndex)
		{
			Debug.Assert(pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType);
			Role role = pathedRole.Role;
			RoleProxy proxy = role.Proxy;
			if (null == proxy)
			{
				return role.FactType;
			}
			PathedRole followingSameFactTypePathedRole = GetNextSameFactTypePathedRole(rolePath, pathedRoles, pathedRoleIndex + 1);
			return followingSameFactTypePathedRole == null ? role.FactType : followingSameFactTypePathedRole.Role.FactType;
		}
		/// <summary>
		/// Recursive helper method for ResolvePathedEntryRoleFactType
		/// </summary>
		private PathedRole GetNextSameFactTypePathedRole(RolePath rolePath, ReadOnlyCollection<PathedRole> pathedRoles, int pathedRoleIndex)
		{
			PathedRole retVal = null;
			if (pathedRoleIndex < pathedRoles.Count)
			{
				retVal = pathedRoles[pathedRoleIndex];
				if (retVal.PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					retVal = null;
				}
			}
			else
			{
				// Look for same fact type entry roles down the split paths
				RolePathCache cache = EnsureRolePathCache();
				foreach (RolePath nestedPath in cache.SplitPathCollection(rolePath))
				{
					if (null != (retVal = GetNextSameFactTypePathedRole(nestedPath, cache.PathedRoleCollection(nestedPath), 0)))
					{
						break;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Callback used with <see cref="VisitPathedRolesForFactTypeEntry"/>
		/// </summary>
		/// <param name="pathedRole">A <see cref="PathedRole"/> in the current fact type occurrence.</param>
		/// <returns>Return <see langword="true"/> to continue.</returns>
		private delegate bool PathedRoleVisitor(PathedRole pathedRole);
		/// <summary>
		/// Recursive helper method for resolving all pathed roles used by a single fact type entry
		/// </summary>
		/// <param name="factTypeEntry">The <see cref="PathedRole"/> that enters the fact type instance.</param>
		/// <param name="visitor">A <see cref="PathedRoleVisitor"/> callback.</param>
		private void VisitPathedRolesForFactTypeEntry(PathedRole factTypeEntry, PathedRoleVisitor visitor)
		{
			RolePath rolePath = factTypeEntry.RolePath;
			ReadOnlyCollection<PathedRole> pathedRoles = EnsureRolePathCache().PathedRoleCollection(rolePath);
			int pathCount = pathedRoles.Count;
			int entryIndex = pathedRoles.IndexOf(factTypeEntry);
			bool switchedFactTypes = false;
			for (int i = entryIndex; i < pathCount; ++i)
			{
				PathedRole pathedRole = pathedRoles[i];
				if (i != entryIndex && pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					switchedFactTypes = true;
					break;
				}
				if (!visitor(pathedRole))
				{
					return;
				}
			}
			if (!switchedFactTypes)
			{
				NotifyLeadSameFactTypeRoles(rolePath, visitor);
			}
		}
		private bool NotifyLeadSameFactTypeRoles(RolePath parentRolePath, PathedRoleVisitor visitor)
		{
			RolePathCache cache = myRolePathCache;
			foreach (RolePath childRolePath in cache.SplitPathCollection(parentRolePath))
			{
				bool switchedFactTypes = false;
				foreach (PathedRole pathedRole in cache.PathedRoleCollection(childRolePath))
				{
					if (pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
					{
						switchedFactTypes = true;
						break;
					}
					if (!visitor(pathedRole))
					{
						return false;
					}
				}
				if (!switchedFactTypes)
				{
					if (!NotifyLeadSameFactTypeRoles(childRolePath, visitor))
					{
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Add the use of a fact type instance to the verbalization map
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to add.</param>
		/// <param name="factTypeEntry">The <see cref="PathedRole"/> that is used to join into the <see cref="FactType"/></param>
		/// <param name="pathedRoles">The <see cref="PathedRole"/> collection that contains <paramref name="factTypeEntry"/></param>
		/// <param name="factTypeEntryIndex">The index of <paramref name="factTypeEntry"/> in <paramref name="pathedRoles"/></param>
		/// <param name="baseRolesStack">A stack of <see cref="RoleBase"/> collections.</param>
		/// <param name="usedRoles">A <see cref="BitTracker"/> with one bit for each role in each fact type on the <paramref name="baseRolesStack"/>.</param>
		/// <param name="pathConditions">Conditions for the path currently being processed. If all pathed roles
		/// in a condition are satisfied by this part of the fact type, then process it with the fact type.</param>
		/// <param name="processedConditions">A <see cref="BitTracker"/> with one bit for each of the <paramref name="pathConditions"/>.
		/// Used to track which conditions have been processed.</param>
		/// <returns>The base index in the <paramref name="usedRoles"/> tracker corresponding
		/// to the first role in the new top of the <paramref name="baseRolesStack"/></returns>
		private int PushFactType(
			FactType factType,
			PathedRole factTypeEntry,
			ReadOnlyCollection<PathedRole> pathedRoles,
			int factTypeEntryIndex,
			Stack<LinkedElementCollection<RoleBase>> baseRolesStack,
			ref BitTracker usedRoles,
			LinkedElementCollection<CalculatedPathValue> pathConditions,
			ref BitTracker processedConditions)
		{
			// Manage the role and fact type stacks
			int retVal = usedRoles.Count;
			LinkedElementCollection<RoleBase> factTypeRoles = factType.RoleCollection;
			baseRolesStack.Push(factTypeRoles);
			usedRoles.Resize(retVal + factTypeRoles.Count);

			// Find the index of the last pathed role in the same fact type
			// in this chain. We use this count multiple times, and don't
			// want to recalculate.
			int roleCount = pathedRoles.Count;
			for (int i = factTypeEntryIndex + 1; i < roleCount; ++i)
			{
				if (pathedRoles[i].PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					// Restrict the role count
					roleCount = i;
					break;
				}
			}

			// Get value constraints for roles in this section of the path
			bool addedFactTypeEntry = false;
			VerbalizationPlanNode contextChainNode = null;
			for (int i = factTypeEntryIndex; i < roleCount; ++i)
			{
				PathedRole currentPathedRole = pathedRoles[i];
				// UNDONE: RolePathCombination, check combination-specific value constraints
				PathConditionRoleValueConstraint valueConstraint = currentPathedRole.DirectValueConstraint;
				if (valueConstraint != null)
				{
					if (!addedFactTypeEntry)
					{
						addedFactTypeEntry = true;
						contextChainNode = EnsureChainNodeForFactTypeConditions(factType, factTypeEntry);
					}
					VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, contextChainNode, ref myRootPlanNode);
				}
			}
			
			// Look for functions that use roles restricted to this section of the path
			int conditionCount = pathConditions.Count;
			if (conditionCount != 0)
			{
				for (int i = 0; i < conditionCount; ++i)
				{
					if (!processedConditions[i])
					{
						CalculatedPathValue calculation = pathConditions[i];
						bool? isLocal = IsLocalCalculatedValue(
							calculation,
							delegate(PathedRole testPathedRole)
							{
								for (int j = factTypeEntryIndex; j < roleCount; ++j)
								{
									if (pathedRoles[j] == testPathedRole)
									{
										return true;
									}
								}
								return false;
							});
						if (isLocal.GetValueOrDefault(false))
						{
							if (!addedFactTypeEntry)
							{
								addedFactTypeEntry = true;
								contextChainNode = EnsureChainNodeForFactTypeConditions(factType, factTypeEntry);
							}
							processedConditions[i] = true;
							VerbalizationPlanNode.AddCalculatedConditionNode(calculation, true, contextChainNode, ref myRootPlanNode);
						}
					}
				}
			}

			if (!addedFactTypeEntry)
			{
				myCurrentBranchNode = VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for <see cref="PushFactType"/>
		/// </summary>
		private VerbalizationPlanNode EnsureChainNodeForFactTypeConditions(FactType factType, PathedRole factTypeEntry)
		{
			VerbalizationPlanNode contextChainNode = myCurrentBranchNode;
			if (contextChainNode == null)
			{
				myCurrentBranchNode = contextChainNode = VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
			}
			else
			{
				if (contextChainNode.BranchType != VerbalizationPlanBranchType.Chain)
				{
					contextChainNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, contextChainNode, ref myRootPlanNode);
				}
				VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, contextChainNode, ref myRootPlanNode);
			}
			return contextChainNode;
		}
		/// <summary>
		/// Determine if a calculated value is based solely on local inputs.
		/// </summary>
		/// <param name="calculatedValue">The calculation to test.</param>
		/// <param name="isLocalPathedRole">A callback to determine if a <see cref="PathedRole"/> is local.</param>
		/// <returns>true if all parts of the calculation depend on local pathed roles, false if a non-local pathed
		/// role is used, and null if no pathed roles are used.</returns>
		private static bool? IsLocalCalculatedValue(CalculatedPathValue calculatedValue, Predicate<PathedRole> isLocalPathedRole)
		{
			bool seenLocalPathedRole = false;
			foreach (CalculatedPathValueInput input in calculatedValue.InputCollection)
			{
				PathedRole sourcePathedRole;
				CalculatedPathValue sourceCalculation;
				if (null != (sourcePathedRole = input.SourcePathedRole))
				{
					if (isLocalPathedRole(sourcePathedRole))
					{
						seenLocalPathedRole = true;
					}
					else
					{
						return false;
					}
				}
				else if (null != (sourceCalculation = input.SourceCalculatedValue))
				{
					// Recurse
					bool? recursiveResult = IsLocalCalculatedValue(sourceCalculation, isLocalPathedRole);
					if (recursiveResult.HasValue)
					{
						if (recursiveResult.Value)
						{
							seenLocalPathedRole = true;
						}
						else
						{
							return false;
						}
					}
				}
			}
			return seenLocalPathedRole ? (bool?)true : null;
		}
		/// <summary>
		/// Pop a <see cref="FactType"/> added with <see cref="PushFactType"/>. Registers
		/// a use of each unused role to support correct variable subscripting.
		/// </summary>
		/// <param name="baseRolesStack">A stack of <see cref="RoleBase"/> collections.</param>
		/// <param name="usedRoles">A <see cref="BitTracker"/> with one bit for each role in each fact type on the <paramref name="baseRolesStack"/>.</param>
		/// <returns>The base index in the <paramref name="usedRoles"/> tracker corresponding
		/// to the first role in the new top of the <paramref name="baseRolesStack"/></returns>
		private int PopFactType(Stack<LinkedElementCollection<RoleBase>> baseRolesStack, ref BitTracker usedRoles)
		{
			LinkedElementCollection<RoleBase> factTypeRoles = baseRolesStack.Pop();
			int roleCount = factTypeRoles.Count;
			int baseRoleIndex = usedRoles.Count - roleCount;
			for (int i = 0; i < roleCount; ++i)
			{
				ObjectType rolePlayer;
				if (!usedRoles[baseRoleIndex + i] &&
					null != (rolePlayer = factTypeRoles[i].Role.RolePlayer) &&
					!rolePlayer.IsImplicitBooleanValue)
				{
					RegisterRolePlayerUse(rolePlayer, null, null, null);
				}
			}
			usedRoles.Resize(baseRoleIndex);
			return baseRolesStack.Count == 0 ? -1 : baseRoleIndex - baseRolesStack.Peek().Count;
		}
		/// <summary>
		/// Push a split into the verbalization plan
		/// </summary>
		/// <param name="splitFrom">The <see cref="RolePath"/> being split from. All
		/// pre-split fact types have been handled at this point.</param>
		private void PushSplit(RolePath splitFrom)
		{
			VerbalizationPlanBranchType branchType = GetBranchType(splitFrom);
			if (branchType != VerbalizationPlanBranchType.None)
			{
				VerbalizationPlanNode parentNode = myCurrentBranchNode;
				// Make sure that we have a permanent parent node
				if (parentNode == null)
				{
					parentNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref myRootPlanNode);
				}
				myCurrentBranchNode = VerbalizationPlanNode.AddBranchNode(branchType, GetRenderingStyleFromBranchType(branchType), parentNode, ref myRootPlanNode);
			}
		}
		/// <summary>
		/// Step to the parent of the current branch in the verbalization plan
		/// </summary>
		private void PopSplit(RolePath splitFrom)
		{
			if (GetBranchType(splitFrom) != VerbalizationPlanBranchType.None)
			{
				VerbalizationPlanNode branchNode = myCurrentBranchNode;
				VerbalizationPlanNode newParentNode = branchNode.ParentNode;
				myCurrentBranchNode = newParentNode;

				// See if any collapsing is possible when we pop the branch off.
				// For now, we collapse if an 'and' or 'or' branch contains
				// branches of the same type. We may collapse other combinations
				// in the future.
				VerbalizationPlanBranchType branchType = branchNode.BranchType;
				switch (branchType)
				{
					case VerbalizationPlanBranchType.AndSplit:
					case VerbalizationPlanBranchType.OrSplit:
						LinkedNode<VerbalizationPlanNode> childNode = branchNode.FirstChildNode;
						if (childNode == null)
						{
							// Get rid of the empty node
							if (newParentNode != null)
							{
								newParentNode.CollapseChildNode(branchNode);
							}
						}
						else
						{
							bool differentChildType = false;
							while (childNode != null)
							{
								if (childNode.Value.BranchType != branchType)
								{
									differentChildType = true;
									break;
								}
								childNode = childNode.Next;
							}
							if (!differentChildType)
							{
								// Collapse the child nodes into the branch node
								childNode = branchNode.FirstChildNode;
								while (childNode != null)
								{
									LinkedNode<VerbalizationPlanNode> nextNode = childNode.Next; // Get next before collapsing
									branchNode.CollapseChildNode(childNode);
									childNode = nextNode;
								}
							}
						}
					break;
				}
			}
		}
		/// <summary>
		/// Push a new chain node. The chain node may later
		/// be collapsed to a single child node when it is
		/// popped off the stakc using <see cref="PopConditionalChainNode"/>
		/// </summary>
		private void PushConditionalChainNode()
		{
			VerbalizationPlanNode parentNode = myCurrentBranchNode;
			// Make sure that we have a permanent parent node
			if (parentNode == null)
			{
				parentNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref myRootPlanNode);
			}
			myCurrentBranchNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, parentNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Remove a node added with the <see cref="PushConditionalChainNode"/> and
		/// collapse the node if it is not needed.
		/// </summary>
		private void PopConditionalChainNode()
		{
			VerbalizationPlanNode chainNode = myCurrentBranchNode;
			Debug.Assert(chainNode.BranchType == VerbalizationPlanBranchType.Chain);
			VerbalizationPlanNode parentNode = chainNode.ParentNode;
			myCurrentBranchNode = parentNode;
			LinkedNode<VerbalizationPlanNode> headChildNode = chainNode.FirstChildNode;
			// If the node has no children or one child or the parent is a chain node,
			// then flatten the hierarchy.
			if (headChildNode == null ||
				headChildNode.Next == null ||
				parentNode.BranchType == VerbalizationPlanBranchType.Chain)
			{
				parentNode.CollapseChildNode(chainNode);
			}
		}
		/// <summary>
		/// Push a new negated chain node.
		/// </summary>
		private void PushNegatedChainNode()
		{
			VerbalizationPlanNode parentNode = myCurrentBranchNode;
			// Make sure that we have a permanent parent node
			if (parentNode == null)
			{
				parentNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, null, ref myRootPlanNode);
			}
			myCurrentBranchNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.NegatedChain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, parentNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Pop a negated chain node pushed with <see cref="PushNegatedChainNode"/>
		/// </summary>
		private void PopNegatedChainNode()
		{
			myCurrentBranchNode = myCurrentBranchNode.ParentNode;
		}
		private VerbalizationPlanBranchType GetBranchType(RolePath contextPath)
		{
			switch (contextPath.SplitCombinationOperator)
			{
				case LogicalCombinationOperator.And:
					return contextPath.SplitIsNegated ? VerbalizationPlanBranchType.NegatedAndSplit : VerbalizationPlanBranchType.AndSplit;
				case LogicalCombinationOperator.Or:
					return contextPath.SplitIsNegated ? VerbalizationPlanBranchType.NegatedOrSplit : VerbalizationPlanBranchType.OrSplit;
				case LogicalCombinationOperator.Xor:
					return contextPath.SplitIsNegated ? VerbalizationPlanBranchType.NegatedXorSplit : VerbalizationPlanBranchType.XorSplit;
			}
			return VerbalizationPlanBranchType.None;
		}
		/// <summary>
		/// Test if a <see cref="PathedRole"/> is reference.
		/// </summary>
		private bool IsPathedRoleReferencedOutsidePath(PathedRole pathedRole)
		{
			return DomainRoleInfo.GetAllElementLinks(pathedRole).Count != 0;
		}
		/// <summary>
		/// Used with <see cref="VisitRolePathParts"/> as the callback for walking a role path.
		/// Walks the path both forward and backwards, allowing for stack-based node analysis.
		/// </summary>
		/// <param name="currentPathedRole">The current pathed role. Set unless this is a notification of a split.</param>
		/// <param name="currentPath">The current path. If <paramref name="currentPathedRole"/> is not set, then this is
		/// a split notification and the context path provides the split settings.</param>
		/// <param name="currentPathedRoles">All pathed roles for the current path.</param>
		/// <param name="currentPathedRoleIndex">The index of <paramref name="currentPathedRole"/> in <paramref name="currentPathedRoles"/></param>
		/// <param name="contextPathedRole">The <see cref="PathedRole"/> prior to the <paramref name="currentPathedRole"/></param>
		/// <param name="unwinding">The path is being walked in reverse.</param>
		private delegate void RolePathNodeVisitor(PathedRole currentPathedRole, RolePath currentPath, ReadOnlyCollection<PathedRole> currentPathedRoles, int currentPathedRoleIndex, PathedRole contextPathedRole, bool unwinding);
		private void VisitRolePathParts(RolePath rolePath, PathedRole contextPathedRole, RolePathNodeVisitor visitor)
		{
			PathedRole splitContext = contextPathedRole;
			RolePathCache cache = EnsureRolePathCache();
			ReadOnlyCollection<PathedRole> pathedRoles = cache.PathedRoleCollection(rolePath);
			int pathedRoleCount = pathedRoles.Count;
			for (int i = 0; i < pathedRoleCount; ++i)
			{
				PathedRole pathedRole = pathedRoles[i];
				visitor(pathedRole, rolePath, pathedRoles, i, splitContext, false);
				splitContext = pathedRole;
			}
			bool notifiedSplit = false;
			foreach (RoleSubPath subPath in cache.SplitPathCollection(rolePath))
			{
				if (!notifiedSplit)
				{
					notifiedSplit = true;
					visitor(null, rolePath, pathedRoles, -1, splitContext, false);
				}
				VisitRolePathParts(subPath, splitContext, visitor);
			}
			// Unwind the path
			if (notifiedSplit)
			{
				visitor(null, rolePath, pathedRoles, -1, splitContext, true);
			}
			for (int i = pathedRoleCount - 1; i >= 0; --i)
			{
				visitor(pathedRoles[i], rolePath, pathedRoles, i, i == 0 ? contextPathedRole : pathedRoles[i - 1], true);
			}
		}
		/// <summary>
		/// Associate path variables with an entry role.
		/// </summary>
		/// <param name="pathedRole">The entry into a fact type.</param>
		/// <param name="joinSourcePathedRole">The join source.</param>
		/// <param name="leadRolePath">The <see cref="LeadRolePath"/>, which should be associated with a variable for the <see cref="LeadRolePath.RootObjectType"/> before this call.</param>
		private void RegisterFactTypeEntryRolePlayerUse(PathedRole pathedRole, PathedRole joinSourcePathedRole, LeadRolePath leadRolePath)
		{
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			RolePlayerVariable joinToVariable = null;
			RolePlayerVariableUse joinSourceVariableUse;
			ObjectType targetRolePlayer = pathedRole.Role.RolePlayer;
			if (joinSourcePathedRole != null)
			{
				if (useMap.TryGetValue(joinSourcePathedRole, out joinSourceVariableUse))
				{
					joinToVariable = joinSourceVariableUse.PrimaryRolePlayerVariable;
				}
			}
			else if (leadRolePath != null &&
				pathedRole.PathedRolePurpose == PathedRolePurpose.StartRole &&
				useMap.TryGetValue(leadRolePath, out joinSourceVariableUse))
			{
				// Note that the lead role path does not get alternate variables
				joinToVariable = joinSourceVariableUse.PrimaryRolePlayerVariable;
			}
			RegisterRolePlayerUse(targetRolePlayer, joinToVariable, pathedRole, pathedRole);
		}
		/// <summary>
		/// Register a use of an object type before verbalization. All uses
		/// must be registered to ensure correct subscripting.
		/// </summary>
		/// <param name="rolePlayer">The object type to use. If this is null, then the variable
		/// is tracked as a missing role player.</param>
		/// <param name="joinToVariable">An existing variable to join to. Directly joined variables
		/// of compatible but different types get priority over correlated variables.</param>
		/// <param name="usedFor">The object use. If this is <see langword="null"/>, then
		/// this is a fully existential use of the role player.</param>
		/// <param name="correlateWith">The <see cref="PathedRole"/> to correlate this variable
		/// with. The list of correlated variables is stored with the normalize root correlation
		/// variable. This may be the same instance as <paramref name="usedFor"/> and does not
		/// need to be pre-normalized before this call.</param>
		/// <returns>New or existing variable</returns>
		private RolePlayerVariable RegisterRolePlayerUse(ObjectType rolePlayer, RolePlayerVariable joinToVariable, object usedFor, PathedRole correlateWith)
		{
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			RolePlayerVariable existingVariable = null;
			bool addNewVariableToCorrelationRoot = false;
			RolePlayerVariableUse correlationRootVariableUse = default(RolePlayerVariableUse);
			if (correlateWith != null)
			{
				// Normalize the correlation target
				correlateWith = EnsureRolePathCache().GetCorrelationRootPathedRole(correlateWith);
				if (useMap.TryGetValue(correlateWith, out correlationRootVariableUse))
				{
					// Find an existing variable of the correct type in the correlation list
					bool seenExternal = false;
					foreach (RolePlayerVariable testMatchingVariable in correlationRootVariableUse.GetCorrelatedVariables(true))
					{
						if (testMatchingVariable.RolePlayer == rolePlayer)
						{
							existingVariable = testMatchingVariable;
							break;
						}
						if (!seenExternal && testMatchingVariable.IsExternalVariable)
						{
							seenExternal = true;
						}
					}

					// If we didn't find an existing variable, but one or more of the correlated variables
					// is external, then look again through possible external correlation lists for a variable of
					// the correct type.
					Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> externalCorrelations;
					if (existingVariable == null &&
						seenExternal &&
						null != (externalCorrelations = myCorrelatedExternalVariables))
					{
						// Repeat the exercise, looking deeper through correlated external lists for a variable match
						foreach (RolePlayerVariable testMatchingVariable in correlationRootVariableUse.GetCorrelatedVariables(true))
						{
							LinkedNode<RolePlayerVariable> externalCorrelationHead;
							if (testMatchingVariable.IsExternalVariable &&
								externalCorrelations.TryGetValue(testMatchingVariable, out externalCorrelationHead))
							{
								foreach (RolePlayerVariable testMatchingVariable2 in externalCorrelationHead)
								{
									if (testMatchingVariable2 != testMatchingVariable &&
										testMatchingVariable2.RolePlayer == rolePlayer)
									{
										existingVariable = testMatchingVariable2;
										addNewVariableToCorrelationRoot = true;
										break;
									}
								}
								if (existingVariable != null)
								{
									break;
								}
							}
						}
					}
				}
				else
				{
					// Note that if joinToVariable is set here, then it comes from an external
					// correlation. Otherwise, the join would have the same correlation root
					// as the current pathed role.
					ObjectType correlationRootRolePlayer = correlateWith.Role.RolePlayer;
					RegisterRolePlayerUse(correlationRootRolePlayer, joinToVariable, correlateWith, null);
					correlationRootVariableUse = useMap[correlateWith];
					if (correlationRootRolePlayer == rolePlayer)
					{
						existingVariable = correlationRootVariableUse.PrimaryRolePlayerVariable;
					}
				}
				if (existingVariable == null)
				{
					addNewVariableToCorrelationRoot = true;
				}
			}
			bool joinedToExternalVariable = false;
			if (existingVariable == null && joinToVariable != null)
			{
				Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> externalCorrelations;
				LinkedNode<RolePlayerVariable> externalCorrelationHead;
				joinedToExternalVariable = joinToVariable.IsExternalVariable;
				if (joinToVariable.RolePlayer == rolePlayer)
				{
					existingVariable = joinToVariable;
				}
				else if (joinedToExternalVariable &&
					null != (externalCorrelations = myCorrelatedExternalVariables) &&
					externalCorrelations.TryGetValue(joinToVariable, out externalCorrelationHead))
				{
					foreach (RolePlayerVariable testMatchingVariable in externalCorrelationHead)
					{
						if (testMatchingVariable != joinToVariable &&
							testMatchingVariable.RolePlayer == rolePlayer)
						{
							existingVariable = testMatchingVariable;
							break;
						}
					}
				}
			}
			if (existingVariable != null)
			{
				if (existingVariable.RolePlayer == rolePlayer)
				{
					if (usedFor != null)
					{
						if (joinToVariable == existingVariable)
						{
							joinToVariable = null;
						}
						RolePlayerVariableUse existingVariableUse;
						if (useMap.TryGetValue(usedFor, out existingVariableUse))
						{
							if (existingVariableUse.JoinedToVariable != joinToVariable)
							{
								existingVariableUse.JoinedToVariable = joinToVariable;
								UpdateRolePlayerVariableUse(usedFor, existingVariableUse);
							}
						}
						else
						{
							useMap.Add(usedFor, new RolePlayerVariableUse(existingVariable, joinToVariable, correlateWith == usedFor ? null : correlateWith));
						}
						if (addNewVariableToCorrelationRoot && correlationRootVariableUse.AddCorrelatedVariable(existingVariable))
						{
							// An external variable was found that is not in the local correlation list
							UpdateRolePlayerVariableUse(correlateWith, correlationRootVariableUse);
						}
					}
					// Track use phase during registration to see if the root variable is
					// referenced by the path.
					existingVariable.Use(myLatestUsePhase, false);
					return existingVariable;
				}
				else
				{
					existingVariable = null;
				}
			}
			Dictionary<ObjectType, RelatedRolePlayerVariables> objectTypeMap = myObjectTypeToVariableMap;
			RelatedRolePlayerVariables relatedVariables;
			if (usedFor == null)
			{
				// This is a fully existential use of the role player
				if (rolePlayer == null)
				{
					// Special case for missing role players. Apply the same treatment as
					// known role players without using the dictionary.
					RelatedRolePlayerVariables? missingVariables = myMissingRolePlayerVariables;
					if (missingVariables.HasValue)
					{
						relatedVariables = missingVariables.Value;
						if (!relatedVariables.UsedFullyExistentially)
						{
							relatedVariables.UsedFullyExistentially = true;
							myMissingRolePlayerVariables = relatedVariables;
						}
					}
					else
					{
						myMissingRolePlayerVariables = new RelatedRolePlayerVariables(null, true);
					}
				}
				else if (!objectTypeMap.TryGetValue(rolePlayer, out relatedVariables))
				{
					objectTypeMap.Add(rolePlayer, new RelatedRolePlayerVariables(null, true));
				}
				else if (!relatedVariables.UsedFullyExistentially)
				{
					relatedVariables.UsedFullyExistentially = true;
					objectTypeMap[rolePlayer] = relatedVariables;
				}
				return null;
			}
			else
			{
				RolePlayerVariableUse existingVariableUse;
				if (useMap.TryGetValue(usedFor, out existingVariableUse))
				{
					existingVariable = existingVariableUse.PrimaryRolePlayerVariable;
					if (existingVariable == joinToVariable)
					{
						joinToVariable = null;
					}
					if (existingVariableUse.JoinedToVariable != joinToVariable)
					{
						existingVariableUse.JoinedToVariable = joinToVariable;
						useMap[usedFor] = existingVariableUse;
					}
				}
				else
				{
					existingVariable = new RolePlayerVariable(rolePlayer);
					existingVariableUse = new RolePlayerVariableUse(existingVariable, joinToVariable, correlateWith == usedFor ? null : correlateWith);
					useMap[usedFor] = existingVariableUse;
					if (rolePlayer == null)
					{
						// Similar to null role player treatment above, duplicates non-null case without using the dictionary.
						RelatedRolePlayerVariables? missingVariables = myMissingRolePlayerVariables;
						if (missingVariables.HasValue)
						{
							relatedVariables = missingVariables.Value;
							if (relatedVariables.AddRolePlayerVariable(existingVariable))
							{
								myMissingRolePlayerVariables = relatedVariables;
							}
						}
						else
						{
							myMissingRolePlayerVariables = new RelatedRolePlayerVariables(existingVariable, false);
						}
					}
					else if (objectTypeMap.TryGetValue(rolePlayer, out relatedVariables))
					{
						if (relatedVariables.AddRolePlayerVariable(existingVariable))
						{
							objectTypeMap[rolePlayer] = relatedVariables;
						}
					}
					else
					{
						objectTypeMap.Add(rolePlayer, new RelatedRolePlayerVariables(existingVariable, false));
					}
				}
				if (addNewVariableToCorrelationRoot && existingVariable != null && correlationRootVariableUse.AddCorrelatedVariable(existingVariable))
				{
					UpdateRolePlayerVariableUse(correlateWith, correlationRootVariableUse);
				}
				if (joinToVariable != null && joinedToExternalVariable && !existingVariable.IsExternalVariable)
				{
					existingVariable.IsExternalVariable = true;
					CorrelateExternalVariables(joinToVariable, existingVariable);
				}
				existingVariable.Use(myLatestUsePhase, false);
				return existingVariable;
			}
		}
		/// <summary>
		/// Get the <see cref="RolePlayerVariableUse"/> registered for a given key, if any
		/// </summary>
		protected RolePlayerVariableUse? GetRolePlayerVariableUse(object usedFor)
		{
			RolePlayerVariableUse variableUse;
			if (usedFor != null &&
				myUseToVariableMap.TryGetValue(usedFor, out variableUse))
			{
				return variableUse;
			}
			return null;
		}
		/// <summary>
		/// Update the <see cref="RolePlayerVariableUse"/> registered for a given key
		/// </summary>
		protected void UpdateRolePlayerVariableUse(object usedFor, RolePlayerVariableUse variableUse)
		{
			myUseToVariableMap[usedFor] = variableUse;
		}
		/// <summary>
		/// A chance for a subtype to add path projections using the
		/// <see cref="AddExternalVariable"/> method.
		/// </summary>
		/// <param name="pathOwner">The <see cref="RolePathOwner"/></param>
		protected virtual void AddPathProjections(RolePathOwner pathOwner)
		{
			// Default implementation is empty
		}
		/// <summary>
		/// Add calculations and constants that are bound directly to a
		/// head variable registered during <see cref="AddPathProjections"/>
		/// using the <see cref="ProjectExternalVariable(Object,CalculatedPathValue)"/> and
		/// <see cref="ProjectExternalVariable(Object,PathConstant)"/> methods.
		/// </summary>
		/// <param name="leadPathComponent">A <see cref="LeadRolePath"/> or <see cref="RolePathCombination"/>
		/// with associated projections.</param>
		protected virtual void AddCalculatedAndConstantProjections(RolePathComponent leadPathComponent)
		{
			// Default implementation is empty
		}
		/// <summary>
		/// Begin the verbalization with a quantified root variable
		/// </summary>
		protected virtual bool VerbalizeRootObjectType
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Add a variable for use while verbalizing elements associated with the pathed role.
		/// </summary>
		/// <param name="headVariableKey">The required key for the head variable. Used later to retrieve information
		/// about this variable.</param>
		/// <param name="existingExternalVariable">A <see cref="RolePlayerVariable"/> returned from a previous
		/// call to this method for elements in another registered path.</param>
		/// <param name="rolePlayer">The <see cref="ObjectType"/> for the associated role player.</param>
		/// <param name="associatedPathedRole">A <see cref="PathedRole"/> that should be correlated with this variable.</param>
		/// <returns>New or existing <see cref="RolePlayerVariable"/></returns>
		protected RolePlayerVariable AddExternalVariable(object headVariableKey, RolePlayerVariable existingExternalVariable, ObjectType rolePlayer, PathedRole associatedPathedRole)
		{
			Debug.Assert(!(headVariableKey is PathedRole));
			// Handle cases with projections from multiple paths with the same owner
			RolePlayerVariableUse? existingVariableUse = GetRolePlayerVariableUse(headVariableKey);
			if (existingVariableUse.HasValue)
			{
				RolePlayerVariable existingVariable = existingVariableUse.Value.PrimaryRolePlayerVariable;
				existingVariable.IsExternalVariable = true;
				if (existingExternalVariable != null)
				{
					// Note that correlation is a noop if the variables are already externally correlated
					CorrelateExternalVariables(existingExternalVariable, existingVariable);
				}
				else
				{
					existingExternalVariable = existingVariable;
				}
			}
			RolePlayerVariable retVal = RegisterRolePlayerUse(rolePlayer, existingExternalVariable, headVariableKey, associatedPathedRole);
			retVal.IsExternalVariable = true;
			return retVal;
		}
		/// <summary>
		/// Correlate two external variables
		/// </summary>
		private void CorrelateExternalVariables(RolePlayerVariable externalVariable1, RolePlayerVariable externalVariable2)
		{
			if (externalVariable1 == externalVariable2 || externalVariable1 == null || externalVariable2 == null)
			{
				return;
			}
			Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> externalCorrelations = myCorrelatedExternalVariables;
			LinkedNode<RolePlayerVariable> existingListHeadNode1 = null;
			LinkedNode<RolePlayerVariable> existingListHeadNode2 = null;
			if (null == externalCorrelations)
			{
				myCorrelatedExternalVariables = externalCorrelations = new Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>>();
			}
			else
			{
				externalCorrelations.TryGetValue(externalVariable1, out existingListHeadNode1);
				externalCorrelations.TryGetValue(externalVariable2, out existingListHeadNode2);
			}
			if (existingListHeadNode1 == null)
			{
				if (existingListHeadNode2 == null)
				{
					// Create the new two item list and store the same list in both places.
					existingListHeadNode1 = new LinkedNode<RolePlayerVariable>(externalVariable1);
					existingListHeadNode1.SetNext(new LinkedNode<RolePlayerVariable>(externalVariable2), ref existingListHeadNode1);
					externalCorrelations[externalVariable1] = existingListHeadNode1;
					externalCorrelations[externalVariable2] = existingListHeadNode1;
				}
				else
				{
					// Just add to the tail. The head will not change here, so we do not need to
					// reset existing keys.
					existingListHeadNode2.GetTail().SetNext(new LinkedNode<RolePlayerVariable>(externalVariable1), ref existingListHeadNode2);
					externalCorrelations[externalVariable1] = existingListHeadNode2;
				}
			}
			else if (existingListHeadNode2 == null)
			{
				// Just add to the tail. The head will not change here, so we do not need to
				// reset existing keys.
				existingListHeadNode1.GetTail().SetNext(new LinkedNode<RolePlayerVariable>(externalVariable2), ref existingListHeadNode1);
				externalCorrelations[externalVariable2] = existingListHeadNode1;
			}
			else if (existingListHeadNode1 != existingListHeadNode2)
			{
				// Merge the second list into the first
				// Move all elements in the second list node into the other node, and re-key the
				// merged elements to point to the new list.
				LinkedNode<RolePlayerVariable> newHead = null;
				LinkedNode<RolePlayerVariable> mergeWithTail = existingListHeadNode2.GetTail();
				while (mergeWithTail != null)
				{
					LinkedNode<RolePlayerVariable> mergeNode = mergeWithTail;
					mergeWithTail = mergeNode.Previous;
					mergeNode.Detach(ref existingListHeadNode2);
					RolePlayerVariable testVariable = mergeNode.Value;
					LinkedNode<RolePlayerVariable> relatedNode = existingListHeadNode1;
					while (relatedNode != null)
					{
						if (relatedNode.Value == testVariable)
						{
							break;
						}
						relatedNode = relatedNode.Next;
					}
					if (relatedNode == null)
					{
						// The related variable was not in the new list, track it and attach it after this loop.
						if (newHead == null)
						{
							newHead = mergeNode;
						}
						else
						{
							mergeNode.SetNext(newHead, ref newHead);
						}
					}
					externalCorrelations[testVariable] = existingListHeadNode1;
				}
				if (newHead != null)
				{
					// Attach unique merged variables to the tail to complete the process
					existingListHeadNode1.GetTail().SetNext(newHead, ref existingListHeadNode1);
				}
			}
		}
		/// <summary>
		/// Add a projection to a calculation based on variables using
		/// in the path. Called during <see cref="AddCalculatedAndConstantProjections"/> after
		/// all other variables in the path have been declared.
		/// </summary>
		/// <param name="headVariableKey">The projection key used in <see cref="AddExternalVariable"/>.</param>
		/// <param name="calculation">A <see cref="CalculatedPathValue"/> projected onto this head variable.</param>
		protected void ProjectExternalVariable(object headVariableKey, CalculatedPathValue calculation)
		{
			VerbalizationPlanNode.AddProjectedCalculationNode(headVariableKey, calculation, myCurrentBranchNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Add a projection to a constant based on variables using
		/// in the path. Called during <see cref="AddCalculatedAndConstantProjections"/> after
		/// all other variables in the path have been declared.
		/// </summary>
		/// <param name="headVariableKey">The projection key used in <see cref="AddExternalVariable"/>.</param>
		/// <param name="pathConstant">A <see cref="PathConstant"/> projected onto this head variable.</param>
		protected void ProjectExternalVariable(object headVariableKey, PathConstant pathConstant)
		{
			VerbalizationPlanNode.AddProjectedConstantNode(headVariableKey, pathConstant, myCurrentBranchNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Start a new quantification use phase
		/// </summary>
		private void BeginQuantificationUsePhase()
		{
			++myLatestUsePhase;
			List<int> phases = myUsePhases;
			if (phases != null)
			{
				phases.Clear();
			}
		}
		/// <summary>
		/// Get the use phase currently used for quantification
		/// </summary>
		private int CurrentQuantificationUsePhase
		{
			get
			{
				List<int> phases = myUsePhases;
				return (phases != null && phases.Count != 0) ? phases[0] : myLatestUsePhase;
			}
		}
		/// <summary>
		/// Get the phase number for the latest pushed pairing or quantification phase.
		/// </summary>
		private int CurrentPairingUsePhase
		{
			get
			{
				List<int> phases = myUsePhases;
				int length;
				return (phases != null && (length = phases.Count) != 0) ? phases[length - 1] : myLatestUsePhase;
			}
		}
		private bool IsPairingUsePhaseInScope(int usePhase)
		{
			List<int> phases = myUsePhases;
			if (phases != null && phases.Count != 0)
			{
				return phases.Contains(usePhase);
			}
			return usePhase == myLatestUsePhase;
		}
		private void PushPairingUsePhase()
		{
			List<int> phases = myUsePhases;
			if (phases == null)
			{
				phases = new List<int>();
				myUsePhases = phases;
				phases.Add(myLatestUsePhase);
			}
			else if (phases.Count == 0)
			{
				phases.Add(myLatestUsePhase);
			}
			phases.Add(++myLatestUsePhase);
		}
		private void PopPairingUsePhase()
		{
			List<int> phases = myUsePhases;
			phases.RemoveAt(phases.Count - 1);
		}
		#endregion // Analysis Methods
		#region Rendering Methods
		/// <summary>
		/// Begin a verbalization of the path(s) and and associate head statement
		/// </summary>
		public void BeginVerbalization()
		{
			BeginQuantificationUsePhase();
		}
		/// <summary>
		/// Test if a path verbalization is available for the specified <see cref="RolePathOwner"/>
		/// </summary>
		public bool HasPathVerbalization(RolePathOwner pathOwner)
		{
			if (pathOwner == null)
			{
				return false;
			}
			Dictionary<RolePathOwner, VerbalizationPlanNode> planMap = myPathOwnerToVerbalizationPlanMap;
			return planMap != null ? planMap.ContainsKey(pathOwner) : pathOwner == mySingleRolePathOwner;
		}
		/// <summary>
		/// Render the path verbalization for a specific owner.
		/// </summary>
		/// <param name="pathOwner">The <see cref="RolePathOwner"/> to render.</param>
		/// <param name="builder">An existing <see cref="StringBuilder"/>. The current state
		/// of the builder will be restored before returning.</param>
		/// <returns>The completed verbalization string for the path.</returns>
		public string RenderPathVerbalization(RolePathOwner pathOwner, StringBuilder builder)
		{
			Dictionary<RolePathOwner, VerbalizationPlanNode> planMap = myPathOwnerToVerbalizationPlanMap;
			VerbalizationPlanNode planNode = null;
			if (planMap != null)
			{
				planMap.TryGetValue(pathOwner, out planNode);
			}
			else if (pathOwner == mySingleRolePathOwner)
			{
				planNode = myRootPlanNode;
			}
			if (planNode == null)
			{
				return "";
			}
			if (builder == null)
			{
				builder = new StringBuilder();
			}
			int outdentPosition;
			return RenderVerbalizationPlanNode(builder, planNode, null, out outdentPosition);
		}
		/// <summary>
		/// Render a role player for use outside in an external predicate replacement.
		/// </summary>
		/// <param name="rolePlayerFor">The role player this is for.</param>
		/// <param name="hyphenBindingFormatString">The hyphen bound format string for the replacement role. If this
		/// is provided, then there is a single replacement field for the role player, and any additional quantification
		/// should treat the hyphen-bound text as a single unit.</param>
		/// <param name="renderingOptions">Options from the <see cref="RolePathRolePlayerRenderingOptions"/> values.</param>
		/// <returns>String replacement field with formatting and subscripting applied
		/// by the current <see cref="IRolePathRenderer"/></returns>
		public string RenderAssociatedRolePlayer(object rolePlayerFor, string hyphenBindingFormatString, RolePathRolePlayerRenderingOptions renderingOptions)
		{
			string retVal = null;
			RoleBase roleBase; // Fallback for common case with non-projected role
			RolePlayerVariableUse? nullableVariableUse = GetRolePlayerVariableUse(rolePlayerFor);
			bool firstUse = true;
			if (nullableVariableUse.HasValue)
			{
				RolePlayerVariable variable = nullableVariableUse.Value.PrimaryRolePlayerVariable;
				if (0 != (renderingOptions & RolePathRolePlayerRenderingOptions.UsedInVerbalizationHead))
				{
					variable.IsHeadVariable = true;
					if (0 != (renderingOptions & RolePathRolePlayerRenderingOptions.MinimizeHeadSubscripting))
					{
						variable.MinimizeHeadSubscripting = true;
					}
				}
				firstUse = variable.Use(CurrentQuantificationUsePhase, false);
				retVal = GetSubscriptedRolePlayerName(variable);
				Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> externalCorrelations;
				LinkedNode<RolePlayerVariable> externalCorrelationNode;

				// Look for partnering with another used external variable
				if (null != (externalCorrelations = myCorrelatedExternalVariables) &&
					externalCorrelations.TryGetValue(variable, out externalCorrelationNode))
				{
					// If we haven't already been paired with an external correlation
					// then pair up now.
					RolePlayerVariable partnerWithVariable = GetUnpairedPartnerVariable(variable, firstUse, externalCorrelationNode);
					if (partnerWithVariable != null)
					{
						retVal = PartnerVariables(variable, retVal, partnerWithVariable, null);
					}
				}
			}
			else if (null != (roleBase = rolePlayerFor as RoleBase))
			{
				retVal = myRenderer.RenderRolePlayer(roleBase.Role.RolePlayer, 0, true);
			}
			if (retVal != null)
			{
				if (hyphenBindingFormatString != null)
				{
					retVal = string.Format(myRenderer.FormatProvider, hyphenBindingFormatString, retVal);
				}
				if (0 != (renderingOptions & RolePathRolePlayerRenderingOptions.Quantify))
				{
					retVal = QuantifyRolePlayerName(retVal, firstUse, false);
				}
				return retVal;
			}
			return "";
		}
		private string RenderVerbalizationPlanNode(StringBuilder builder, VerbalizationPlanNode node, LinkedNode<VerbalizationPlanNode> nodeLink, out int outdentPosition)
		{
			outdentPosition = -1;
			if (node == null)
			{
				return "";
			}
			FactType factType;
			PathedRole entryPathedRole;
			IReading reading;
			IList<RoleBase> factRoles;
			int factRoleCount;
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			VerbalizationPlanNode childNode;
			VerbalizationPlanReadingOptions readingOptions;
			string result;
			RolePlayerVariable rootVariable;
			IRolePathRenderer renderer = myRenderer;
			switch (node.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					factType = node.FactType;
					entryPathedRole = node.FactTypeEntry;
					reading = node.Reading;
					factRoles = reading.RoleCollection;
					factRoleCount = factRoles.Count;
					readingOptions = node.ReadingOptions;
					PathedRole[] pathedRoles = new PathedRole[factRoleCount];
					int replacedRoleCount = 0;
					string predicatePartDecorator = renderer.GetPredicatePartDecorator(factType);
					VerbalizationHyphenBinder hyphenBinder = new VerbalizationHyphenBinder(reading, renderer.FormatProvider, factRoles, null, renderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.HyphenBoundPredicatePart), predicatePartDecorator);
					bool negateExitRole = 0 != (readingOptions & VerbalizationPlanReadingOptions.NegatedExitRole);
					int negatedExitRoleIndex = -1;

					// Iterate and cache the pathed roles in one pass through the
					// fact type use in the path. Delay calling QuantifyRolePlayer
					// so that subscript ordering occurs in reading order, not role
					// path order.
					VisitPathedRolesForFactTypeEntry(
						entryPathedRole,
						delegate(PathedRole pathedRole)
						{
							int resolvedRoleIndex = factRoles.IndexOf(ResolveRoleBaseInFactType(pathedRole.Role, factType));
							if (resolvedRoleIndex != -1 && null == pathedRoles[resolvedRoleIndex])
							{
								if (negateExitRole && pathedRole != entryPathedRole)
								{
									negatedExitRoleIndex = resolvedRoleIndex;
								}
								pathedRoles[resolvedRoleIndex] = pathedRole;
								++replacedRoleCount;
							}
							return replacedRoleCount < factRoleCount; // Optimization so that the visitor doesn't need to look harder than necessary
						});

					// Get replacements in reading order
					string[] roleReplacements = new string[factRoleCount];
					for (int i = 0; i < factRoleCount; ++i)
					{
						PathedRole pathedRole = pathedRoles[i];
						string replacement = null;
						if (pathedRole != null)
						{
							if (i == 0)
							{
								if (0 != (readingOptions & VerbalizationPlanReadingOptions.BackReferenceFirstRole))
								{
									ObjectType rolePlayer = pathedRole.Role.RolePlayer;
									replacement = renderer.ResolveVerbalizerSnippet(rolePlayer != null && rolePlayer.IsPersonal ? RolePathVerbalizerSnippetType.PersonalPronoun : RolePathVerbalizerSnippetType.ImpersonalPronoun);
								}
								else if (0 != (readingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole))
								{
									replacement = "";
								}
							}
							if (replacement == null)
							{
								replacement = QuantifyRolePlayer(pathedRole, i == negatedExitRoleIndex, hyphenBinder, i);
							}
						}
						else
						{
							// Given that the entry role is never fully existential, if negateExitRole is true,
							// then this must be the opposite role and we do not need to check the index.
							replacement = QuantifyFullyExistentialRolePlayer(factRoles[i].Role.RolePlayer, negateExitRole, hyphenBinder, i);
						}
						roleReplacements[i] = replacement;
					}
					return hyphenBinder.PopulatePredicateText(reading, renderer.FormatProvider, predicatePartDecorator, factRoles, roleReplacements, false);
				case VerbalizationPlanNodeType.Branch:
					int restoreBuilder = builder.Length;
					VerbalizationPlanBranchType branchType = node.BranchType;
					VerbalizationPlanBranchRenderingStyle renderingStyle = VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
					childNodeLink = node.FirstChildNode;
					bool first = true;
					bool isNestedBranch = false;
					bool isTailBranch = false;
					bool inlineNegatedChain = false;
					int nestedOutdent = -1;
					RolePathVerbalizerSnippetType snippet;
					VerbalizationPlanBranchType childBranchType;
					VerbalizationPlanNodeType previousChildNodeType = (VerbalizationPlanNodeType)(-1);
					while (childNodeLink != null)
					{
						snippet = (RolePathVerbalizerSnippetType)(-1);
						childNode = childNodeLink.Value;
						if (first)
						{
							first = false;
							renderingStyle = GetRenderingStyleFromBranchType(branchType);
							switch (renderingStyle)
							{
								case VerbalizationPlanBranchRenderingStyle.HeaderList:
									PushPairingUsePhase();
									break;
								case VerbalizationPlanBranchRenderingStyle.IsolatedList:
									BeginQuantificationUsePhase();
									break;
							}
							VerbalizationPlanNode parentNode = node.ParentNode;
							if (parentNode != null)
							{
								switch (parentNode.BranchType)
								{
									case VerbalizationPlanBranchType.Chain:
									case VerbalizationPlanBranchType.NegatedChain:
										isTailBranch = nodeLink.Previous != null;
										break;
									default:
										isNestedBranch = true;
										break;
								}
							}
							switch (branchType)
							{
								case VerbalizationPlanBranchType.Chain:
									snippet = RolePathVerbalizerSnippetType.ChainedListOpen;
									break;
								case VerbalizationPlanBranchType.AndSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.AndTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.AndNestedListOpen : RolePathVerbalizerSnippetType.AndLeadListOpen);
									break;
								case VerbalizationPlanBranchType.OrSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.OrTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.OrNestedListOpen : RolePathVerbalizerSnippetType.OrLeadListOpen);
									break;
								case VerbalizationPlanBranchType.XorSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.XorTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.XorNestedListOpen : RolePathVerbalizerSnippetType.XorLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedAndSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedAndTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedAndNestedListOpen : RolePathVerbalizerSnippetType.NegatedAndLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedOrSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedOrTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedOrNestedListOpen : RolePathVerbalizerSnippetType.NegatedOrLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedXorSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedXorTailListOpen : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedXorNestedListOpen : RolePathVerbalizerSnippetType.NegatedXorLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedChain:
									// See if we can inline the negation. Negation is inlined
									// if the lead fact type is binary and the opposite role is
									// either fully existential (not referenced) or has not
									// yet been referenced.
									readingOptions = VerbalizationPlanReadingOptions.None;
									if (0 != ((readingOptions = childNode.ReadingOptions) & VerbalizationPlanReadingOptions.DynamicNegatedExitRole) &&
										null != (entryPathedRole = childNode.FactTypeEntry))
									{
										// The dynamic flag if there is a trailing pathed role in the same role path
										ReadOnlyCollection<PathedRole> childPathedRoles = myRolePathCache.PathedRoleCollection(entryPathedRole.RolePath);
										int testChildIndex = childPathedRoles.IndexOf(entryPathedRole) + 1;
										if (testChildIndex < childPathedRoles.Count &&
											GetRolePlayerVariableUse(childPathedRoles[testChildIndex]).Value.PrimaryRolePlayerVariable.HasBeenUsed(CurrentQuantificationUsePhase, true))
										{
											readingOptions |= VerbalizationPlanReadingOptions.NegatedExitRole;
											childNode.ReadingOptions = readingOptions;
										}
									}
									if (0 == (readingOptions & VerbalizationPlanReadingOptions.NegatedExitRole))
									{
										snippet = RolePathVerbalizerSnippetType.NegatedChainedListOpen;
									}
									else
									{
										inlineNegatedChain = true;
										snippet = RolePathVerbalizerSnippetType.ChainedListOpen;
									}
									break;
							}
						}
						else
						{
							switch (branchType)
							{
								case VerbalizationPlanBranchType.Chain:
								case VerbalizationPlanBranchType.NegatedChain:
									if (0 != ((readingOptions = childNode.ReadingOptions) & VerbalizationPlanReadingOptions.BackReferenceFirstRole))
									{
										snippet = RolePathVerbalizerSnippetType.ChainedListCollapsedSeparator;
									}
									// Check for a 'TailList', which will render its own lead separator in place of the chain separator.
									else if (!(childNode.NodeType == VerbalizationPlanNodeType.Branch &&
										BranchSplits(childBranchType = childNode.BranchType) &&
										GetRenderingStyleFromBranchType(childBranchType) == VerbalizationPlanBranchRenderingStyle.OperatorSeparated))
									{
										snippet = (0 != (readingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole)) ?
											RolePathVerbalizerSnippetType.ChainedListComplexRestrictionCollapsedLeadSeparator :
											((childNode.RestrictsPreviousFactType || previousChildNodeType == VerbalizationPlanNodeType.ChainedRootVariable) ?
												RolePathVerbalizerSnippetType.ChainedListLocalRestrictionSeparator :
												RolePathVerbalizerSnippetType.ChainedListComplexRestrictionSeparator);
									}
									break;
								case VerbalizationPlanBranchType.AndSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.AndTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.AndNestedListSeparator : RolePathVerbalizerSnippetType.AndLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.OrSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.OrTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.OrNestedListSeparator : RolePathVerbalizerSnippetType.OrLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.XorSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.XorTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.XorNestedListSeparator : RolePathVerbalizerSnippetType.XorLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedAndSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedAndTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedAndNestedListSeparator : RolePathVerbalizerSnippetType.NegatedAndLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedOrSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedOrTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedOrNestedListSeparator : RolePathVerbalizerSnippetType.NegatedOrLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedXorSplit:
									snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedXorTailListSeparator : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedXorNestedListSeparator : RolePathVerbalizerSnippetType.NegatedXorLeadListSeparator);
									break;
							}
						}
						if (snippet != (RolePathVerbalizerSnippetType)(-1))
						{
							builder.Append(renderer.ResolveVerbalizerSnippet(snippet));
						}
						nestedOutdent = -1;
						string childText = RenderVerbalizationPlanNode(builder, childNode, childNodeLink, out nestedOutdent);
						if (!string.IsNullOrEmpty(childText))
						{
							if (nestedOutdent != -1)
							{
								nestedOutdent += builder.Length - restoreBuilder;
							}
							builder.Append(childText);
						}
						previousChildNodeType = childNode.NodeType;
						childNodeLink = childNodeLink.Next;
					}
					if (!first)
					{
						snippet = (RolePathVerbalizerSnippetType)(-1);
						switch (branchType)
						{
							case VerbalizationPlanBranchType.Chain:
								snippet = RolePathVerbalizerSnippetType.ChainedListClose;
								break;
							case VerbalizationPlanBranchType.NegatedChain:
								snippet = inlineNegatedChain ?
									RolePathVerbalizerSnippetType.ChainedListClose :
									RolePathVerbalizerSnippetType.NegatedChainedListClose;
								break;
							case VerbalizationPlanBranchType.AndSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.AndTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.AndNestedListClose : RolePathVerbalizerSnippetType.AndLeadListClose);
								break;
							case VerbalizationPlanBranchType.OrSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.OrTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.OrNestedListClose : RolePathVerbalizerSnippetType.OrLeadListClose);
								break;
							case VerbalizationPlanBranchType.XorSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.XorTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.XorNestedListClose : RolePathVerbalizerSnippetType.XorLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedAndSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedAndTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedAndNestedListClose : RolePathVerbalizerSnippetType.NegatedAndLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedOrSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedOrTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedOrNestedListClose : RolePathVerbalizerSnippetType.NegatedOrLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedXorSplit:
								snippet = isTailBranch ? RolePathVerbalizerSnippetType.NegatedXorTailListClose : (isNestedBranch ? RolePathVerbalizerSnippetType.NegatedXorNestedListClose : RolePathVerbalizerSnippetType.NegatedXorLeadListClose);
								break;
						}
						if (snippet != (RolePathVerbalizerSnippetType)(-1))
						{
							string closeSnippet = renderer.ResolveVerbalizerSnippet(snippet);
							if (!string.IsNullOrEmpty(closeSnippet))
							{
								if (0 != (myOptions & RolePathVerbalizerOptions.MarkTrailingOutdentStart) &&
									IsOutdentSnippet(snippet))
								{
									outdentPosition = nestedOutdent != -1 ? nestedOutdent : (builder.Length - restoreBuilder);
								}
								builder.Append(closeSnippet);
							}
							else if (nestedOutdent != -1)
							{
								outdentPosition = nestedOutdent;
							}
						}
						if (renderingStyle == VerbalizationPlanBranchRenderingStyle.HeaderList)
						{
							PopPairingUsePhase();
						}
					}
					if (nodeLink == null && outdentPosition != -1 && outdentPosition < (builder.Length - restoreBuilder))
					{
						builder.Insert(restoreBuilder + outdentPosition, "{0}");
					}
					result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
					builder.Length = restoreBuilder;
					return result;
				case VerbalizationPlanNodeType.CalculatedCondition:
					return renderer.RenderCalculation(node.Calculation, this, builder);
				case VerbalizationPlanNodeType.ValueConstraint:
					return renderer.RenderValueCondition(node.ValueConstraint, this, builder);
				case VerbalizationPlanNodeType.HeadCalculatedValueProjection:
					return string.Format(
						renderer.FormatProvider,
						renderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.HeadVariableProjection),
						RenderAssociatedRolePlayer(node.HeadVariableKey, null, RolePathRolePlayerRenderingOptions.None),
						renderer.RenderCalculation(node.Calculation, this, builder));
				case VerbalizationPlanNodeType.HeadConstantProjection:
					return string.Format(
						renderer.FormatProvider,
						renderer.ResolveVerbalizerSnippet(RolePathVerbalizerSnippetType.HeadVariableProjection),
						RenderAssociatedRolePlayer(node.HeadVariableKey, null, RolePathRolePlayerRenderingOptions.None),
						renderer.RenderConstant(node.Constant));
				case VerbalizationPlanNodeType.ChainedRootVariable:
					rootVariable = node.RootVariable;
					return QuantifyRolePlayerName(GetSubscriptedRolePlayerName(rootVariable), rootVariable.Use(CurrentQuantificationUsePhase, true), false);
				case VerbalizationPlanNodeType.FloatingRootVariableContext:
					// There will always be exactly one child node link at this point
					childNodeLink = node.FirstChildNode;
					rootVariable = myFloatingRootVariable; // Push existing (should be null, but doesn't hurt)
					myFloatingRootVariable = node.RootVariable;
					result = RenderVerbalizationPlanNode(builder, childNodeLink.Value, childNodeLink, out outdentPosition);
					myFloatingRootVariable = rootVariable;
					return result;
			}
			return null;
		}
		private string QuantifyRolePlayer(PathedRole pathedRole, bool negateExistentialQuantifier, VerbalizationHyphenBinder hyphenBinder, int hyphenBinderRoleIndex)
		{
			Role role = pathedRole.Role;
			RolePlayerVariableUse variableUse = GetRolePlayerVariableUse(pathedRole).Value;
			RolePlayerVariable primaryVariable = variableUse.PrimaryRolePlayerVariable;
			int quantificationUsePhase = CurrentQuantificationUsePhase;
			bool firstUseOfPrimaryVariable = !primaryVariable.HasBeenUsed(quantificationUsePhase, false);
			string result = GetSubscriptedRolePlayerName(primaryVariable);
			IRolePathRenderer renderer = myRenderer;

			// Potentially add a single correlation relationship as follows.
			// 1) If there are any used variables (other than the current primary variable)
			//    in the correlation list (including the correlation root) and this variable
			//    is not paired with any of them, then choose the used variable to pair with
			//    based on the following prioritized criteria:
			//    a) The joined to variable (provides continuity through the reading)
			//    b) A head variable
			//    c) The first used variable in the list
			// 2) To emphasize the join, even if we are paired with another correlated variable,
			//    if the joined to variable is used but not paired with the primary, then pair
			//    with it. Note that the join to variable will always be the correlation root
			//    or one of the variables.
			RolePlayerVariable partnerWithVariable = null;
			Dictionary<CorrelatedVariablePairing, int> pairings = myCorrelatedVariablePairing;
			RolePlayerVariable joinedToVariable = variableUse.JoinedToVariable;
			int pairedDuringPhase;
			if (joinedToVariable != null &&
				joinedToVariable.HasBeenUsed(quantificationUsePhase, false) &&
				(firstUseOfPrimaryVariable || pairings == null || !pairings.TryGetValue(new CorrelatedVariablePairing(primaryVariable, joinedToVariable), out pairedDuringPhase) || !IsPairingUsePhaseInScope(pairedDuringPhase)))
			{
				// Satisfied condition 2. Note that at some point up the join chain we
				// will have used or paired with other correlated elements.
				partnerWithVariable = joinedToVariable;
			}
			else
			{
				PathedRole correlationRoot = variableUse.CorrelationRoot;
				if (correlationRoot == null)
				{
					if (variableUse.CorrelatedVariablesHead != null)
					{
						partnerWithVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, variableUse.GetCorrelatedVariables(false)); // No reason to test the primary variable
					}
				}
				else
				{
					partnerWithVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, GetRolePlayerVariableUse(correlationRoot).Value.GetCorrelatedVariables(true));
				}
			}
			Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> externalCorrelations = myCorrelatedExternalVariables;
			string preRenderedPartnerWith = null;
			if (externalCorrelations != null)
			{
				RolePlayerVariable externalPartnerVariable = null;
				LinkedNode<RolePlayerVariable> correlationHead;
				if (primaryVariable.IsExternalVariable && externalCorrelations.TryGetValue(primaryVariable, out correlationHead))
				{
					externalPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, correlationHead);
				}
				if (partnerWithVariable == null)
				{
					partnerWithVariable = externalPartnerVariable;
				}
				else
				{
					if (externalPartnerVariable == partnerWithVariable)
					{
						externalPartnerVariable = null;
					}
					if (partnerWithVariable.IsExternalVariable && externalCorrelations.TryGetValue(partnerWithVariable, out correlationHead))
					{
						RolePlayerVariable partnerExternalPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, correlationHead);
						if (partnerExternalPartnerVariable != null &&
							partnerExternalPartnerVariable != primaryVariable &&
							partnerExternalPartnerVariable != partnerWithVariable &&
							partnerExternalPartnerVariable != externalPartnerVariable)
						{
							if (externalPartnerVariable != null)
							{
								preRenderedPartnerWith = PartnerVariables(externalPartnerVariable, null, partnerExternalPartnerVariable, null);
							}
							else
							{
								externalPartnerVariable = partnerExternalPartnerVariable;
							}
						}
					}
					if (externalPartnerVariable != null)
					{
						preRenderedPartnerWith = PartnerVariables(partnerWithVariable, null, externalPartnerVariable, preRenderedPartnerWith);
					}
				}
			}

			if (partnerWithVariable != null)
			{
				result = PartnerVariables(primaryVariable, result, partnerWithVariable, preRenderedPartnerWith);
			}
			return QuantifyRolePlayerName(hyphenBinder.HyphenBindRoleReplacement(result, hyphenBinderRoleIndex), primaryVariable.Use(quantificationUsePhase, true), negateExistentialQuantifier);
		}
		/// <summary>
		/// Get an unused partner variable pairing from a list of partners. If
		/// the <paramref name="primaryVariable"/> is currently paired with a
		/// variable in the list then a new partner is not returned.
		/// </summary>
		/// <param name="primaryVariable">The variable to pair with.</param>
		/// <param name="firstUseOfPrimaryVariable">This represents the first use of the primary variable. Do not look for existing pairings.</param>
		/// <param name="possibleCorrelationPartners">Potential partners, possibly including the primary variable.</param>
		/// <returns>An unpaired variable, or <see langword="null"/></returns>
		private RolePlayerVariable GetUnpairedPartnerVariable(RolePlayerVariable primaryVariable, bool firstUseOfPrimaryVariable, IEnumerable<RolePlayerVariable> possibleCorrelationPartners)
		{
			if (possibleCorrelationPartners != null)
			{
				RolePlayerVariable firstUnpairedNormalVariable = null; // Normal meaning not the head
				RolePlayerVariable firstUnpairedHeadVariable = null;
				int pairedDuringPhase;
				int quantificationUsePhase = CurrentQuantificationUsePhase;
				Dictionary<CorrelatedVariablePairing, int> pairings = myCorrelatedVariablePairing;
				RolePlayerVariable floatingRootVariable = myFloatingRootVariable;
				bool unusedFloatingRootVariable = false;
				foreach (RolePlayerVariable possiblePartner in possibleCorrelationPartners)
				{
					if (possiblePartner == primaryVariable)
					{
						continue;
					}
					bool isHeadVariable = possiblePartner.IsHeadVariable;
					if (isHeadVariable || possiblePartner.HasBeenUsed(quantificationUsePhase, false))
					{
						if (!firstUseOfPrimaryVariable &&
							pairings != null &&
							pairings.TryGetValue(new CorrelatedVariablePairing(primaryVariable, possiblePartner), out pairedDuringPhase) &&
							IsPairingUsePhaseInScope(pairedDuringPhase))
						{
							// We have an existing pairing, get out
							firstUnpairedHeadVariable = null;
							firstUnpairedNormalVariable = null;
							break;
						}
						if (possiblePartner.IsHeadVariable)
						{
							if (firstUnpairedHeadVariable == null)
							{
								firstUnpairedHeadVariable = possiblePartner;
							}
						}
						else if (firstUnpairedNormalVariable == null)
						{
							firstUnpairedNormalVariable = possiblePartner;
						}
					}
					else if (possiblePartner == floatingRootVariable)
					{
						unusedFloatingRootVariable = true;
					}
				}
				return firstUnpairedHeadVariable ?? firstUnpairedNormalVariable ?? (unusedFloatingRootVariable ? floatingRootVariable : null);
			}
			return null;
		}
		/// <summary>
		/// Partner two unpartnered variables using the current pairing phase.
		/// The partnered variable should have been retrieved with <see cref="GetUnpairedPartnerVariable"/>.
		/// </summary>
		/// <param name="primaryVariable">The primary (left) variable to partner.</param>
		/// <param name="preRenderedPrimary">Already rendered text to represent the primary variable, or null for a standard replacement.</param>
		/// <param name="partnerWithVariable">The partner (right) variable.</param>
		/// <param name="preRenderedPartner">Already rendered text to represent the partner variable, or null for a standard replacement.</param>
		/// <returns>A combined string</returns>
		private string PartnerVariables(RolePlayerVariable primaryVariable, string preRenderedPrimary, RolePlayerVariable partnerWithVariable, string preRenderedPartner)
		{
			if (preRenderedPrimary == null)
			{
				preRenderedPrimary = GetSubscriptedRolePlayerName(primaryVariable);
			}
			if (preRenderedPartner == null)
			{
				preRenderedPartner = GetSubscriptedRolePlayerName(partnerWithVariable);
			}
			IRolePathRenderer renderer = myRenderer;
			ObjectType leftRolePlayer = primaryVariable.RolePlayer;
			Dictionary<CorrelatedVariablePairing, int> pairings = myCorrelatedVariablePairing;
			string retVal = string.Format(
				renderer.FormatProvider,
				renderer.ResolveVerbalizerSnippet(leftRolePlayer != null && leftRolePlayer.IsPersonal ? RolePathVerbalizerSnippetType.PersonalIdentityCorrelation : RolePathVerbalizerSnippetType.ImpersonalIdentityCorrelation),
				preRenderedPrimary,
				QuantifyRolePlayerName(preRenderedPartner, partnerWithVariable.Use(CurrentQuantificationUsePhase, true), false));
			if (pairings == null)
			{
				myCorrelatedVariablePairing = pairings = new Dictionary<CorrelatedVariablePairing, int>();
			}
			pairings[new CorrelatedVariablePairing(primaryVariable, partnerWithVariable)] = CurrentPairingUsePhase;
			return retVal;
		}
		/// <summary>
		/// Add quantification to a previously formatted role player name.
		/// </summary>
		/// <param name="rolePlayerName">The formatted role player name.</param>
		/// <param name="existentialQuantifier">Apply an existential quantifier instead of a back reference.</param>
		/// <param name="negateExistentialQuantifier">Negate the existential quantifier.</param>
		/// <returns>Quantified name.</returns>
		private string QuantifyRolePlayerName(string rolePlayerName, bool existentialQuantifier, bool negateExistentialQuantifier)
		{
			IRolePathRenderer renderer = myRenderer;
			string formatString = renderer.ResolveVerbalizerSnippet(existentialQuantifier ? (negateExistentialQuantifier ? RolePathVerbalizerSnippetType.NegatedExistentialQuantifier : RolePathVerbalizerSnippetType.ExistentialQuantifier) : RolePathVerbalizerSnippetType.BackReferenceQuantifier);
			if (string.IsNullOrEmpty(formatString) || formatString == "{0}")
			{
				return rolePlayerName;
			}
			return string.Format(renderer.FormatProvider, formatString, rolePlayerName);
		}
		private string GetSubscriptedRolePlayerName(RolePlayerVariable variable)
		{
			ObjectType rolePlayer = variable.RolePlayer;
			RelatedRolePlayerVariables relatedVariables = rolePlayer == null ? myMissingRolePlayerVariables.Value : myObjectTypeToVariableMap[rolePlayer];
			return myRenderer.RenderRolePlayer(rolePlayer, relatedVariables.ReserveSubscript(variable), false);
		}
		private string QuantifyFullyExistentialRolePlayer(ObjectType rolePlayer, bool negateExistentialQuantifier, VerbalizationHyphenBinder hyphenBinder, int hyphenBinderRoleIndex)
		{
			return QuantifyRolePlayerName(hyphenBinder.HyphenBindRoleReplacement(myRenderer.RenderRolePlayer(rolePlayer, 0, true), hyphenBinderRoleIndex), true, negateExistentialQuantifier);
		}
		#endregion // Rendering Methods
		#region // Static Helper Methods
		private static Regex myFieldRegex;
		/// <summary>
		/// The regular expression used to find fields and indices in
		/// a reading format string. Capture fields are named "Field"
		/// for the full replacement field and "Index" for the numeric portion.
		/// </summary>
		private static Regex FieldRegex
		{
			get
			{
				Regex retVal = myFieldRegex;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFieldRegex,
						new Regex(
							@"(?n)(?<Field>((?<!\{)\{)(?<Index>[0-9]+)(\}(?!\})))",
							RegexOptions.Compiled),
						null);
					retVal = myFieldRegex;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Perform the equivalent of a string format with outdent resolution
		/// for each replacement field. Each replacement field can itself contain
		/// a single replacement field that is the position in the string before
		/// one or more outdents (opposite of indent). All parts of the following string
		/// (whether from the format string or a following replacement field) that occur
		/// before the next <paramref name="newLine"/> sequence will be inserted at
		/// the outdent location, with the remainder of the string inserted after
		/// this point.
		/// </summary>
		/// <param name="formatProvider">The <see cref="IFormatProvider"/></param>
		/// <param name="builder">A <see cref="StringBuilder"/> to append to.
		/// If this is <see langword="null"/>, then a string is returned.</param>
		/// <param name="newLine">The new line sequence to search for in the format
		/// and replacement strings.</param>
		/// <param name="formatString">The master format string.</param>
		/// <param name="replacementFields">The replacement fields, each of which can
		/// contain a single replacement field.</param>
		/// <returns>The formatted string, unless a .</returns>
		public static string FormatResolveOutdent(IFormatProvider formatProvider, StringBuilder builder, string newLine, string formatString, params string[] replacementFields)
		{
			bool returnString = builder == null;
			if (returnString)
			{
				builder = new StringBuilder();
			}
			RecurseFormatResolveOutdent(formatProvider, newLine, formatString, replacementFields, builder, 0, formatString.Length);
			return returnString ? builder.ToString() : null;
		}
		private static void RecurseFormatResolveOutdent(IFormatProvider formatProvider, string newLine, string formatString, string[] replacementFields, StringBuilder builder, int formatStringStartsAt, int formatStringLength)
		{
			if (formatStringStartsAt >= formatStringLength)
			{
				return;
			}
			Match match = FieldRegex.Match(formatString, formatStringStartsAt);
			if (!match.Success)
			{
				builder.Append(formatString, formatStringStartsAt, formatStringLength - formatStringStartsAt);
			}
			else
			{
				for (; ; )
				{
					Group fieldGroup = match.Groups["Field"];
					// Append up to this point
					int fieldGroupIndex = fieldGroup.Index;
					if (fieldGroupIndex > formatStringStartsAt)
					{
						builder.Append(formatString, formatStringStartsAt, fieldGroupIndex - formatStringStartsAt);
					}
					int replacementIndex;
					if (int.TryParse(match.Groups["Index"].Value, NumberStyles.None, formatProvider, out replacementIndex) &&
						replacementIndex >= 0 &&
						replacementIndex < replacementFields.Length)
					{
						string replacementField = replacementFields[replacementIndex];
						Match replacementMatch = FieldRegex.Match(replacementField);
						int replacementIndexTest;
						if (replacementMatch.Success &&
							int.TryParse(replacementMatch.Groups["Index"].Value, NumberStyles.None, formatProvider, out replacementIndexTest) &&
							replacementIndexTest == 0)
						{
							// Append text before the replacement
							Group replacementFieldGroup = replacementMatch.Groups["Field"];
							int replacementFieldGroupIndex = replacementFieldGroup.Index;
							if (replacementFieldGroupIndex != 0)
							{
								builder.Append(replacementField, 0, replacementFieldGroupIndex);
							}

							// Do some additional testing on the replacement field before recursing
							int afterReplacementFieldGroup = replacementFieldGroupIndex + replacementFieldGroup.Length;
							int replacementLength = replacementField.Length;
							if (afterReplacementFieldGroup < replacementLength) // Sanity check, indicates there is text after the replacement field
							{
								// Recurse the format string to get additional text and
								// divide the following text based on newLine position.
								int restoreBuilder = builder.Length;
								RecurseFormatResolveOutdent(formatProvider, newLine, formatString, replacementFields, builder, fieldGroupIndex + fieldGroup.Length, formatStringLength);
								string followingText = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
								builder.Length = restoreBuilder;
								int newLineIndex = followingText.IndexOf(newLine);
								if (newLineIndex != -1)
								{
									// Put the text before the new line at the replacement point, trailing text with the newline after
									if (newLineIndex != 0)
									{
										builder.Append(followingText, 0, newLineIndex);
									}
									builder.Append(replacementField, afterReplacementFieldGroup, replacementLength - afterReplacementFieldGroup);
									builder.Append(followingText, newLineIndex, followingText.Length - newLineIndex);
								}
								else
								{
									builder.Append(followingText);
									builder.Append(replacementField, afterReplacementFieldGroup, replacementLength - afterReplacementFieldGroup);
								}
								break;
							}
						}
						else
						{
							builder.Append(replacementField);
						}
					}

					// For simple fields, recurse in this loop without recursing the method
					formatStringStartsAt = fieldGroupIndex + fieldGroup.Length;
					if (formatStringStartsAt < formatStringLength)
					{
						match = match.NextMatch();
						if (!match.Success)
						{
							builder.Append(formatString, formatStringStartsAt, formatStringLength - formatStringStartsAt);
							break;
						}
					}
					else
					{
						break;
					}
				}
			}
		}
		#endregion // Static Helper Methods
		#region Type-specific Creation Methods
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="FactTypeDerivationRule"/>
		/// </summary>
		public static RolePathVerbalizer Create(FactTypeDerivationRule derivationRule, IRolePathRenderer rolePathRenderer)
		{
			FactTypeDerivationRuleVerbalizer retVal = new FactTypeDerivationRuleVerbalizer(rolePathRenderer);
			retVal.InitializeRolePathOwner(derivationRule);
			return retVal;
		}
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="SetComparisonConstraint"/>
		/// </summary>
		public static RolePathVerbalizer Create(SetComparisonConstraint setComparisonConstraint, IRolePathRenderer rolePathRenderer)
		{
			SetComparisonConstraintVerbalizer retVal = new SetComparisonConstraintVerbalizer(rolePathRenderer);
			retVal.Initialize(setComparisonConstraint);
			return retVal;
		}
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="SubtypeDerivationRule"/>.
		/// Rendering of the subtype variable can be retrieved using the <paramref name="derivationRule"/>
		/// as the key for <see cref="RenderAssociatedRolePlayer"/>.
		/// </summary>
		public static RolePathVerbalizer Create(SubtypeDerivationRule derivationRule, IRolePathRenderer rolePathRenderer)
		{
			SubTypeDerivationRuleVerbalizer retVal = new SubTypeDerivationRuleVerbalizer(rolePathRenderer);
			retVal.InitializeRolePathOwner(derivationRule);
			retVal.AddExternalVariable(derivationRule, null, derivationRule.Subtype, null);
			return retVal;
		}
		#endregion // Type-specific Creation Methods
		#region Type-specific Classes
		#region FactTypeDerivationRuleVerbalizer class
		/// <summary>
		/// A class to assist in verbalization of a fact type derivation
		/// </summary>
		private sealed class FactTypeDerivationRuleVerbalizer : RolePathVerbalizer
		{
			public FactTypeDerivationRuleVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Override to add and correlate variables for projection bindings
			/// </summary>
			protected override void AddPathProjections(RolePathOwner pathOwner)
			{
				// Overlay all projection information
				FactTypeDerivationRule derivationRule = (FactTypeDerivationRule)pathOwner;
				foreach (FactTypeDerivationProjection projection in FactTypeDerivationProjection.GetLinksToProjectedPathComponentCollection(derivationRule))
				{
					if (projection.PathComponent is RolePathCombination)
					{
						// UNDONE: RolePathCombination
						continue;
					}
					foreach (FactTypeRoleProjection roleProjection in FactTypeRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						Role role = roleProjection.ProjectedRole;
						AddExternalVariable(role, null, role.RolePlayer, roleProjection.ProjectedFromPathedRole);
					}
				}
			}
			/// <summary>
			/// Override to bind calculation and constant projections
			/// </summary>
			protected override void AddCalculatedAndConstantProjections(RolePathComponent leadPathComponent)
			{
				if (leadPathComponent is RolePathCombination)
				{
					// UNDONE: RolePathCombination
					return;
				}
				// Overlay projection information
				FactTypeDerivationProjection projection = FactTypeDerivationProjection.GetLinkToFactTypeDerivationRuleProjection(leadPathComponent);
				if (projection != null)
				{
					foreach (FactTypeRoleProjection roleProjection in FactTypeRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						CalculatedPathValue calculation;
						PathConstant constant;
						if (null != (calculation = roleProjection.ProjectedFromCalculatedValue))
						{
							ProjectExternalVariable(roleProjection.ProjectedRole, calculation);
						}
						else if (null != (constant = roleProjection.ProjectedFromConstant))
						{
							ProjectExternalVariable(roleProjection.ProjectedRole, constant);
						}
					}
				}
			}
		}
		#endregion // FactTypeDerivationRuleVerbalizer class
		#region SubTypeDerivationRuleVerbalizer class
		/// <summary>
		/// A class to assist in verbalization of a sub type derivation
		/// </summary>
		private sealed class SubTypeDerivationRuleVerbalizer : RolePathVerbalizer
		{
			public SubTypeDerivationRuleVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Verbalize the root object type, which is assumed to be
			/// placed inside a snippet similar to 'each SUBTYPE is PATH'.
			/// </summary>
			protected override bool VerbalizeRootObjectType
			{
				get
				{
					return true;
				}
			}
		}
		#endregion // SubTypeDerivationRuleVerbalizer class
		#region SetComparisonConstraintVerbalizer class
		private sealed class SetComparisonConstraintVerbalizer : RolePathVerbalizer
		{
			// Member variables, used during initialization callbacks to
			// correlate columns.
			private RolePlayerVariable[] myColumnVariables;
			private ReadOnlyCollection<ConstraintRoleSequenceHasRole> myCurrentRoleSequence;
			public SetComparisonConstraintVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Initialize all column and join path bindings
			/// </summary>
			/// <param name="setComparisonConstraint">The <see cref="SetComparisonConstraint"/> to analyze.</param>
			public void Initialize(SetComparisonConstraint setComparisonConstraint)
			{
				LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = setComparisonConstraint.RoleSequenceCollection;
				int sequenceCount = sequences.Count;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole>[] allSequencedRoleLinks = new ReadOnlyCollection<ConstraintRoleSequenceHasRole>[sequenceCount];
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> sequencedRoleLinks;
				int columnCount = 0;
				for (int i = 0; i < sequenceCount; ++i)
				{
					sequencedRoleLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(sequences[i]);
					// Note that these should all be the same, but this makes the code bomb proof against an
					// arity mismatch error.
					columnCount = Math.Max(columnCount, sequencedRoleLinks.Count);
					allSequencedRoleLinks[i] = sequencedRoleLinks;
				}
				RolePlayerVariable[] columnVariables = new RolePlayerVariable[columnCount];
				myColumnVariables = columnVariables;
				for (int i = 0; i < sequenceCount; ++i)
				{
					ConstraintRoleSequence sequence = sequences[i];
					ConstraintRoleSequenceJoinPath joinPath = sequence.JoinPath;
					if (joinPath != null)
					{
						myCurrentRoleSequence = allSequencedRoleLinks[i];
						InitializeRolePathOwner(joinPath);
					}
				}
				myColumnVariables = null;
				myCurrentRoleSequence = null;
				
				// Go back and fill in any constraint roles that do not have an associated variable.
				// Note that this is an error condition for pathed sequences, which should be fully
				// projected, but not for non-pathed sequences. We register variables for all uses to
				// support subscripting across role sequences. Checking for pathed sequences also
				// enables better verbalization of incomplete structures.
				for (int i = 0; i < sequenceCount; ++i)
				{
					sequencedRoleLinks = allSequencedRoleLinks[i];
					int sequenceLength = sequencedRoleLinks.Count;
					for (int j = 0; j < sequenceLength; ++j)
					{
						ConstraintRoleSequenceHasRole constraintRole = sequencedRoleLinks[j];
						RolePlayerVariableUse? variableUse = GetRolePlayerVariableUse(constraintRole);
						if (!variableUse.HasValue)
						{
							RolePlayerVariable existingVariable = columnVariables[j];
							RolePlayerVariable newVariable = AddExternalVariable(constraintRole, existingVariable, constraintRole.Role.RolePlayer, null);
							if (existingVariable == null)
							{
								columnVariables[j] = newVariable;
							}
						}
					}
				}
				// Use phases are used during both initialization and rendering. Make
				// sure a use phase is passed so that we don't see quantified elements
				// as a side effect of initialization.
				BeginQuantificationUsePhase();
			}
			/// <summary>
			/// Override to add and correlate variables for projection bindings
			/// </summary>
			protected override void AddPathProjections(RolePathOwner pathOwner)
			{
				// Overlay all projection information
				ConstraintRoleSequenceJoinPath joinPath = (ConstraintRoleSequenceJoinPath)pathOwner;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = myCurrentRoleSequence;
				RolePlayerVariable[] columnVariables = myColumnVariables;
				foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(joinPath))
				{
					if (projection.PathComponent is RolePathCombination)
					{
						// UNDONE: RolePathCombination
						continue;
					}
					foreach (ConstraintRoleProjection constraintRoleProjection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						ConstraintRoleSequenceHasRole constraintRole = constraintRoleProjection.ProjectedConstraintRole;
						// Correlate down the columns
						int roleIndex = constraintRoles.IndexOf(constraintRole);
						RolePlayerVariable existingVariable = columnVariables[roleIndex];
						RolePlayerVariable newVariable = AddExternalVariable(constraintRole, existingVariable, constraintRole.Role.RolePlayer, constraintRoleProjection.ProjectedFromPathedRole);
						if (existingVariable == null)
						{
							columnVariables[roleIndex] = newVariable;
						}
					}
				}
			}
			/// <summary>
			/// Override to bind calculation and constant projections
			/// </summary>
			protected override void AddCalculatedAndConstantProjections(RolePathComponent leadPathComponent)
			{
				if (leadPathComponent is RolePathCombination)
				{
					// UNDONE: RolePathCombination
					return;
				}
				// Overlay projection information
				ConstraintRoleSequenceJoinPathProjection projection = ConstraintRoleSequenceJoinPathProjection.GetLinkToConstraintRoleSequenceJoinPathProjection(leadPathComponent);
				if (projection != null)
				{
					foreach (ConstraintRoleProjection constraintRoleProjection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						CalculatedPathValue calculation;
						PathConstant constant;
						if (null != (calculation = constraintRoleProjection.ProjectedFromCalculatedValue))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, calculation);
						}
						else if (null != (constant = constraintRoleProjection.ProjectedFromConstant))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, constant);
						}
					}
				}
			}
		}
		#endregion // SetComparisonConstraintVerbalizer class
		#endregion // Type-specific Classes
	}
	#endregion // RolePathVerbalizer class
}
