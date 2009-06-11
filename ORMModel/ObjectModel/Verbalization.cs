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
		/// <param name="matchAnyLeadRole">Same as matchAnyLeadRole, except with a set match. An IList of RoleBase elements.</param>
		/// <param name="invertLeadRoles">Invert the matchLeadRole and matchAnyLeadRole values</param>
		/// <param name="noFrontText">Match a reading with no front text if possible</param>
		/// <param name="notHyphenBound">If true, do not return a reading that uses any hyphen binding.</param>
		/// <param name="defaultRoleOrder">The default order to match</param>
		/// <param name="allowAnyOrder">If true, use the first reading order if there are no other matches</param>
		/// <returns>A matching <see cref="IReading"/> instance. Can return null if allowAnyOrder is false, or the readingOrders collection is empty.</returns>
		public IReading GetMatchingReading(LinkedElementCollection<ReadingOrder> readingOrders, ReadingOrder ignoreReadingOrder, RoleBase matchLeadRole, IList matchAnyLeadRole, bool invertLeadRoles, bool noFrontText, bool notHyphenBound, IList<RoleBase> defaultRoleOrder, bool allowAnyOrder)
		{
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
								else if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, notHyphenBound, !allowAnyOrder, ref readingMatch))
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
					else if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, matchLeadRole, defaultRoleOrder, noFrontText, notHyphenBound, !allowAnyOrder, ref readingMatch))
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
									else if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, currentRole, defaultRoleOrder, noFrontText, notHyphenBound, !allowAnyOrder, ref readingMatch))
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
							else if (GetMatchingReading(readingOrders, ignoreReadingOrderIndex, (RoleBase)matchAnyLeadRole[i], defaultRoleOrder, noFrontText, notHyphenBound, !allowAnyOrder, ref readingMatch))
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
		/// Helper function for public method of the same name
		/// </summary>
		/// <param name="readingOrders">The ReadingOrder collection to search</param>
		/// <param name="ignoreReadingOrderIndex">Ignore the reading order at this index</param>
		/// <param name="matchLeadRole">The role to match as a lead</param>
		/// <param name="defaultRoleOrder">The default role order. If not specified, any match will be considered optimal</param>
		/// <param name="testNoFrontText">Test for no front text if true.</param>
		/// <param name="notHyphenBound">Do not return a reading with hyphen-bound text.</param>
		/// <param name="strictMatch">Ignored if testNoFrontText is false and notHyphenBound is false. Otherwise, do not set matchingReading if frontText and hyphen binding requirements not satisfied</param>
		/// <param name="matchingReading">The matching reading. Can be non-null to start with</param>
		/// <returns>true if an optimal match was found. retVal will be false if a match is found but
		/// a more optimal match is possible</returns>
		private static bool GetMatchingReading(LinkedElementCollection<ReadingOrder> readingOrders, int ignoreReadingOrderIndex, RoleBase matchLeadRole, IList<RoleBase> defaultRoleOrder, bool testNoFrontText, bool notHyphenBound, bool strictMatch, ref Reading matchingReading)
		{
			ReadingOrder matchingOrder = null;
			int orderCount = readingOrders.Count;
			ReadingOrder testOrder;
			bool optimalMatch = false;
			LinkedElementCollection<RoleBase> testRoles;
			int testRolesCount;
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
						testRolesCount = testRoles.Count;
						if (testRolesCount != 0 && testRoles[0] == matchLeadRole)
						{
							if (defaultRoleOrder != null)
							{
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
									matchingOrder = testOrder;
									optimalMatch = true;
									break;
								}
								if (matchingOrder == null)
								{
									matchingOrder = testOrder; // Remember the first one
								}
							}
							else
							{
								matchingOrder = testOrder;
								optimalMatch = true;
								break;
							}
						}
					}
				}
			}
			if (matchingOrder != null)
			{
				if (!(testNoFrontText || notHyphenBound))
				{
					matchingReading = matchingOrder.PrimaryReading;
				}
				else
				{
					LinkedElementCollection<Reading> readings = matchingOrder.ReadingCollection;
					Reading strictReadingMatch = null;
					int readingCount = readings.Count;
					for (int i = 0; i < readingCount; ++i)
					{
						Reading testReading = readings[i];
						if (notHyphenBound && VerbalizationHyphenBinder.IsHyphenBound(testReading))
						{
							continue;
						}
						else if (!testNoFrontText || testReading.Text.StartsWith("{0}", StringComparison.Ordinal))
						{
							strictReadingMatch = testReading;
							break;
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
				string[] useReplacements = roleReplacements;
				int roleCount = defaultOrder.Count;
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
				//            string mainPatternCommented = @"(?xn)
				//\G
				//# Test if there is a hyphen binding match before the next format replacement field
				//(?((.(?!\s+-\S))*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))
				//	# If there is a hyphen bind before the next replacement field then use it
				//	((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)(?<!(?<!\{)\{\d+\}(?!\}))-(?<AfterLeftHyphen>\s.*?))
				//	|
				//	# Otherwise, pick up all text before the next format replacement field
				//	((?<BeforeLeftHyphenWord>.*?))
				//)
				//# Get the format replacement field
				//((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))
				//# Get any trailing information if it exists prior to the next format field
				//(
				//	(?=
				//		# Positive lookahead to see if there is a next format string
				//		(?(.+(?<!\{)\{\d+\}(?!\}))
				//			# Check before if there is a next format string
				//			(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-\S.*?(?<!\{)\{\d+\}(?!\}))
				//			|
				//			# Get any trailer if there is not a next format string
				//			([^\-]*?\s-\S.*?)
				//		)
				//	)
				//	# Get the before hyphen and right hyphen word if the look ahead succeeded
				//	(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>\S+)
				//)?";
				#endregion // Commented main regex pattern
				Regex regexMain = myMainRegex;
				if (regexMain == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myMainRegex,
						new Regex(
							@"(?n)\G(?((.(?!\s+-\S))*?\S-\s.*?(?<!\{)\{\d+\}(?!\}))((?<BeforeLeftHyphenWord>.*?\s??)(?<LeftHyphenWord>\S+?)(?<!(?<!\{)\{\d+\}(?!\}))-(?<AfterLeftHyphen>\s.*?))|((?<BeforeLeftHyphenWord>.*?)))((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))((?=(?(.+(?<!\{)\{\d+\}(?!\}))(((?!(?<!\{)\{\d+\}(?!\})).)*?\s-\S.*?(?<!\{)\{\d+\}(?!\}))|([^\-]*?\s-\S.*?)))(?<BeforeRightHyphen>.*?\s+?)-(?<RightHyphenWord>\S+))?",
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
							leftWord = leftWord + groups["AfterLeftHyphen"];
							rightWord = groups["BeforeRightHyphen"].Value + rightWord;
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
		/// FactType.PopulatePredicateText if no hyphen-bind occurred
		/// </summary>
		/// <param name="reading">The reading to populate.</param>
		/// <param name="formatProvider">A <see cref="IFormatProvider"/>, or null to use the current culture</param>
		/// <param name="predicatePartDecorator">A format string applied to predicate text between fields.</param>
		/// <param name="defaultOrder">The default role order. Corresponds to the order of the role replacement fields</param>
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
							string useFormat = formatFields[fieldIndex];
							return (useFormat == null) ? roleReplacements[fieldIndex] : string.Format(formatProvider, useFormat, roleReplacements[fieldIndex]);
						});
			}
		}
		#endregion // Member Functions
		#region Static Functions
		/// <summary>
		/// Determines whether or not the given predicate is hyphen bound.
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
						return leftWord + groups["AfterLeftHyphen"].Value + "{0}" + groups["BeforeRightHyphen"].Value + rightWord;
					}
				}
				match = match.NextMatch();
			}
			return null;
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
		public static void VerbalizeElement(IEnumerable<CustomChildVerbalizer> customChildren, IEnumerable<CustomChildVerbalizer> extensionChildren, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
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
}
