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
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.Shell;

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
		/// <summary>
		/// Get the current verbalization options
		/// </summary>
		IDictionary<string, object> VerbalizationOptions { get;}
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
	/// Interface to redirect verbalization. Called for top-level selected objects only.
	/// Shape selection automatically defers to the presented subject, but can be redirect
	/// by this interface to an alternate element.
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
		/// <param name="verbalizationOptions">Current verbalization options</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <returns>IEnumerable of CustomChildVerbalizer structures</returns>
		IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign);
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
	#region Generic IVerbalizationSets interface
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
	#endregion // Generic IVerbalizationSets interface
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
								// is to simply ignore defaultOrder in this case
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
		/// Match the first non whitespace/html character, but only if it is lower case.
		/// Text blocks consisting solely of symbols, spaces, and punctuation are ignored.
		/// </summary>
		private static readonly Regex FirstBodyCharacterPatternLower = new Regex(@"^(?:((?:<[^>]*?>)|\s|(?:(?:(?!<)(?:\p{P}|\p{S}|\s))+?)(?=<))*?)(?<1>\p{Ll})", RegexOptions.Compiled | RegexOptions.Singleline);
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
		/// <param name="defaultOrder">The roles from the parent fact type. Provides the order of the expected replacement fields.
		/// If this is <see langword="null"/>, then the order of the <paramref name="reading"/> roles is used directly.</param>
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
				IList<RoleBase> readingRoles = reading.RoleCollection;
				roleCount = readingRoles.Count;
				if (defaultOrder == null)
				{
					defaultOrder = readingRoles;
				}
				else
				{
					Debug.Assert(defaultOrder.Count == roleCount);
					if ((object)readingRoles != defaultOrder)
					{
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
		/// Get the format string used to hyphen bind a specific role
		/// </summary>
		/// <param name="roleIndex">The index of the represented role in the fact order</param>
		/// <returns>A format string, or <see langword="null"/></returns>
		public string GetRoleFormatString(int roleIndex)
		{
			string[] formatFields = myFormatReplacementFields;
			return (formatFields != null && roleIndex < formatFields.Length) ? formatFields[roleIndex] : null;
		}
		/// <summary>
		/// Get the modified reading text with the hyphen-bound parts removed and the replacement fields
		/// reindexed to match the default role order.
		/// </summary>
		/// <returns>Returns <see langword="null"/> if there is no hyphen binding in the reading.</returns>
		public string ModifiedReadingText
		{
			get
			{
				return myModifiedReadingText;
			}
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
		/// Determines whether or not the given predicate uses hyphen binding
		/// for a specific <paramref name="roleIndex"/> in some <paramref name="readingText"/>.
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
	#region ObjectTypeNameVerbalizationStyle enum
	/// <summary>
	/// Provide options controlling the display of object type names
	/// in fact readings
	/// </summary>
	public enum ObjectTypeNameVerbalizationStyle
	{
		/// <summary>
		/// Present the name exactly as entered by the user
		/// </summary>
		AsIs,
		/// <summary>
		/// If the entered name has embedded capitalization,
		/// extractmultiple words based on the capitalization
		/// and display as lower case.
		/// </summary>
		SeparateCombinedNames,
		/// <summary>
		/// If a name has multiple words, combine them into a
		/// single word with interior capitalization separating
		/// the words. Lead with an upper case letter (PascalStyle)
		/// </summary>
		CombineNamesLeadWithUpper,
		/// <summary>
		/// If a name has multiple words, combine them into a
		/// single word with interior capitalization separating
		/// the words. Lead with a lower case letter (camelStyle)
		/// </summary>
		CombineNamesLeadWithLower,
	}
	#endregion // ObjectTypeNameVerbalizationStyle enum
	#region CoreVerbalizationOption class
	/// <summary>
	/// A list of names for verbalization options.
	/// </summary>
	public static class CoreVerbalizationOption
	{
		/// <summary>
		/// The option name for determining if a single-role uniqueness and
		/// simple mandatory constraint on the same role verbalize as a single constraint.
		/// </summary>
		public const string CombineSimpleMandatoryAndUniqueness = "CombineSimpleMandatoryAndUniqueness";
		/// <summary>
		/// The option name for determining if verbalization is included for the lack of a uniqueness constraint.
		/// </summary>
		public const string ShowDefaultConstraint = "ShowDefaultConstraint";
		/// <summary>
		/// The option name for determining if fact types are listed with an object type verbalization
		/// </summary>
		public const string FactTypesWithObjectType = "FactTypesWithObjectType";
		/// <summary>
		/// The option name to determine how object type names are displayed.
		/// </summary>
		public const string ObjectTypeNameDisplay = "ObjectTypeNameDisplay";
		/// <summary>
		/// The option to determine which separator characters are turned into spaces
		/// when the name display separates combined names.
		/// </summary>
		public const string RemoveObjectTypeNameCharactersOnSeparate = "RemoveObjectTypeNameCharactersOnSeparate";
	}
	#endregion // CoreVerbalizationOption class
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
		private delegate VerbalizationResult VerbalizationHandler(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, VerbalizationSign sign, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel);
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
			private IDictionary<string, object> myVerbalizationOptions;
			public VerbalizationContextImpl(NotifyBeginVerbalization beginCallback, NotifyDeferVerbalization deferCallback, NotifyAlreadyVerbalized alreadyVerbalizedCallback, NotifyAlreadyVerbalized locallyVerbalizedCallback, string verbalizationTarget, IDictionary<string, object> verbalizationOptions)
			{
				myBeginCallback = beginCallback;
				myDeferCallback = deferCallback;
				myAlreadyVerbalizedCallback = alreadyVerbalizedCallback;
				myVerbalizedLocallyCallback = locallyVerbalizedCallback;
				myVerbalizationTarget = verbalizationTarget;
				myVerbalizationOptions = verbalizationOptions;
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
			IDictionary<string, object> IVerbalizationContext.VerbalizationOptions
			{
				get
				{
					return myVerbalizationOptions;
				}
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
		/// <param name="verbalizationOptions"></param>
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
		private static VerbalizationResult VerbalizeElement_VerbalizationResult(VerbalizationCallbackWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalize verbalizer, VerbalizationHandler callback, int indentationLevel, VerbalizationSign sign, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
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
						verbalizationOptions,
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
				verbalizationTarget,
				verbalizationOptions),
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
		/// <param name="verbalizationOptions">The current verbalization options.</param>
		/// <param name="verbalizationTarget">The verbalization target name, representing the container for the verbalization output.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="locallyVerbalized">A dictionary of elements verbalized during the current top level verbalization.</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeElement(object element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizeElement(
				element,
				snippetsDictionary,
				extensionVerbalizer,
				verbalizationOptions,
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
		/// <param name="verbalizationOptions">A dictionary of named verbalization options</param>
		/// <param name="verbalizationTarget">The verbalization target name, representing the container for the verbalization output.</param>
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="locallyVerbalized">A dictionary of elements verbalized during the current top level verbalization.</param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="writer">The VerbalizationCallbackWriter for verbalization output</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		public static void VerbalizeChildren(IEnumerable<CustomChildVerbalizer> customChildren, IEnumerable<CustomChildVerbalizer> extensionChildren, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, VerbalizationCallbackWriter writer, bool writeSecondaryLines, ref bool firstCallPending)
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
					verbalizationOptions,
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
					verbalizationOptions,
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
		/// <param name="verbalizationOptions"></param>
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
		private static void VerbalizeElement(object element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, IVerbalizeFilterChildren outerFilter, VerbalizationCallbackWriter writer, VerbalizationHandler callback, VerbalizationSign sign, int indentLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
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
				VerbalizationResult result = (parentVerbalize != null) ? callback(writer, snippetsDictionary, extensionVerbalizer, verbalizationOptions, verbalizationTarget, alreadyVerbalized, locallyVerbalized, parentVerbalize, callback, indentLevel, sign, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel) : VerbalizationResult.NotVerbalized;
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
									VerbalizeElement(children[j], snippetsDictionary, extensionVerbalizer, verbalizationOptions, verbalizationTarget, alreadyVerbalized, locallyVerbalized, filter, writer, callback, sign, indentLevel, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
								}
							}
						}
					}
					// TODO: Need BeforeNaturalChildren/AfterNaturalChildren/SkipNaturalChildren settings for IVerbalizeCustomChildren
					if (customChildren != null)
					{
						VerbalizeCustomChildren(
							customChildren.GetCustomChildVerbalizations(filter, verbalizationOptions, sign),
							writer,
							callback,
							snippetsDictionary,
							extensionVerbalizer,
							verbalizationOptions,
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
							verbalizationOptions,
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
							VerbalizeElement(deferToElement, snippetsDictionary, extensionVerbalizer, verbalizationOptions, verbalizationTarget, alreadyVerbalized, locallyVerbalized, sign, writer, writeSecondaryLines, ref firstCallPending);
							return;
						}
						if (null != (deferToElement = surveyReference.ReferencedElement))
						{
							VerbalizeElement(deferToElement, snippetsDictionary, extensionVerbalizer, verbalizationOptions, verbalizationTarget, alreadyVerbalized, locallyVerbalized, sign, writer, writeSecondaryLines, ref firstCallPending);
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
		/// <param name="verbalizationOptions"></param>
		/// <param name="verbalizationTarget"></param>
		/// <param name="alreadyVerbalized"></param>
		/// <param name="locallyVerbalized"></param>
		/// <param name="sign">The preferred verbalization sign</param>
		/// <param name="indentationLevel">The current level of indentation</param>
		/// <param name="writeSecondaryLines">True to automatically add a line between callbacks. Set to <see langword="true"/> for multi-select scenarios.</param>
		/// <param name="firstCallPending"></param>
		/// <param name="firstWrite"></param>
		/// <param name="lastLevel"></param>
		private static void VerbalizeCustomChildren(IEnumerable<CustomChildVerbalizer> customChildren, VerbalizationCallbackWriter writer, VerbalizationHandler callback, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IExtensionVerbalizerService extensionVerbalizer, IDictionary<string, object> verbalizationOptions, string verbalizationTarget, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, IDictionary<object, object> locallyVerbalized, VerbalizationSign sign, int indentationLevel, bool writeSecondaryLines, ref bool firstCallPending, ref bool firstWrite, ref int lastLevel)
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
						callback(writer, snippetsDictionary, extensionVerbalizer, verbalizationOptions, verbalizationTarget, alreadyVerbalized, locallyVerbalized, childVerbalize, callback, indentationLevel, sign, writeSecondaryLines, ref firstCallPending, ref firstWrite, ref lastLevel);
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
		#region NormalizeObjectTypeName method
		/// <summary>
		/// A regex pattern to match both replacement fields and preceding text text with replacement fields.
		/// </summary>
		private static Regex myFormatStringSeparator;
		private static Regex FormatStringSeparator
		{
			get
			{
				Regex retVal = myFormatStringSeparator;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFormatStringSeparator,
						new Regex(
							@"(?n)\G(?<Before>.*?)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					retVal = myFormatStringSeparator;
				}
				return retVal;
			}
		}
		/// <summary>
		/// A regex pattern to determine if a character is symbol or punctuation
		/// </summary>
		private static Regex myIsPunctuationOrSymbol;
		private static Regex IsPunctuationOrSymbol
		{
			get
			{
				Regex retVal = myIsPunctuationOrSymbol;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myIsPunctuationOrSymbol,
						new Regex(
							@"\p{P}|\p{S}",
							RegexOptions.Compiled),
						null);
					retVal = myIsPunctuationOrSymbol;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get an object type name based on the current user settings.
		/// </summary>
		/// <param name="originalName">The name as entered in the model.</param>
		/// <param name="verbalizationOptions">The context verbalization options</param>
		/// <returns>The name adjusted according to the current user settings.</returns>
		public static string NormalizeObjectTypeName(string originalName, IDictionary<string, object> verbalizationOptions)
		{
			return NormalizeObjectTypeName(null, originalName, verbalizationOptions);
		}
		/// <summary>
		/// Get an object type name based on the current user settings. If the object type
		/// is a reference mode value type, then it will be broken into its constituent parts,
		/// which will then be treated as separate words.
		/// </summary>
		/// <param name="objectType">The object type to name.</param>
		/// <param name="verbalizationOptions">The context verbalization options</param>
		/// <returns>The name adjusted according to the current user settings.</returns>
		public static string NormalizeObjectTypeName(ObjectType objectType, IDictionary<string, object> verbalizationOptions)
		{
			return NormalizeObjectTypeName(objectType, null, verbalizationOptions);
		}
		/// <summary>
		/// Get an object type name based on the current user settings
		/// </summary>
		/// <param name="objectType">The object type to get the name from. If the object type is a
		/// reference mode value type, then it is treated as multiple names.</param>
		/// <param name="originalName">The name as entered in the model.</param>
		/// <param name="verbalizationOptions">The context verbalization options</param>
		/// <returns>The name adjusted according to the current user settings.</returns>
		private static string NormalizeObjectTypeName(ObjectType objectType, string originalName, IDictionary<string, object> verbalizationOptions)
		{
			ObjectTypeNameVerbalizationStyle style = (ObjectTypeNameVerbalizationStyle)verbalizationOptions[CoreVerbalizationOption.ObjectTypeNameDisplay];
			string removeSeparatedCharacters = style != ObjectTypeNameVerbalizationStyle.AsIs ? (string)verbalizationOptions[CoreVerbalizationOption.RemoveObjectTypeNameCharactersOnSeparate] : null;
			bool spacePending = false;
			bool makeLower = style == ObjectTypeNameVerbalizationStyle.CombineNamesLeadWithLower;
			return NormalizeObjectTypeName(
				objectType,
				originalName,
				style,
				(removeSeparatedCharacters != null && removeSeparatedCharacters.Length == 0) ? null : removeSeparatedCharacters,
				null,
				ref spacePending,
				ref makeLower);
		}
		/// <summary>
		/// Get an object type name based on the current user settings
		/// </summary>
		/// <param name="objectType">The object type to get the name from. If the object type is a
		/// reference mode value type, then it is treated as multiple names.</param>
		/// <param name="originalName">The name as entered in the model.</param>
		/// <param name="style">The verbalization style for this name.</param>
		/// <param name="removeSeparatedCharacters">A list of characters to remove from the generated name and treat as spaces. Used when separating combined names.</param>
		/// <param name="builder">A string builder to append new names to. Can be null on first call, in which case a string is returned.</param>
		/// <param name="spacePending">A space should be added before the next text part. Used with recursive calls when the style has spaces between the names. Initially false.</param>
		/// <param name="makeLower">The initial text should be lower cased. Used with combined names with a lead lower case letter. Initially false.</param>
		/// <returns>The name adjusted according to the current user settings, or null if a StringBuilder was specified.</returns>
		private static string NormalizeObjectTypeName(ObjectType objectType, string originalName, ObjectTypeNameVerbalizationStyle style, string removeSeparatedCharacters, StringBuilder builder, ref bool spacePending, ref bool makeLower)
		{
			if (originalName == null)
			{
				originalName = objectType.Name;
				if (style != ObjectTypeNameVerbalizationStyle.AsIs &&
					objectType.IsValueType)
				{
					// See if this is part of a reference mode pattern
					ORMModel model = null;
					foreach (Role role in objectType.PlayedRoleCollection)
					{
						foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
						{
							UniquenessConstraint pid;
							ObjectType preferredFor;
							ReferenceMode mode;
							if (null != (pid = sequence as UniquenessConstraint) &&
								pid.IsInternal &&
								pid.Modality == ConstraintModality.Alethic &&
								null != (preferredFor = pid.PreferredIdentifierFor) &&
								pid.RoleCollection.Count == 1 &&
								null != (mode = ReferenceMode.FindReferenceModeFromEntityNameAndValueName(originalName, preferredFor.Name, model ?? (model = objectType.Model))))
							{
								ReferenceModeType modeType = mode.Kind.ReferenceModeType;
								if (modeType == ReferenceModeType.General)
								{
									return NormalizeObjectTypeName(null, originalName, style, removeSeparatedCharacters, builder, ref spacePending, ref makeLower);
								}
								else
								{
									string modeFormatString = mode.FormatString;
									Match match = FormatStringSeparator.Match(modeFormatString);
									if (match.Success) // Sanity check, should always be true, don't bother about breaking loop if not
									{
										int trailingTextIndex = 0;
										bool returnText;
										if (returnText = (builder == null))
										{
											builder = new StringBuilder();
										}
										while (match.Success)
										{
											GroupCollection groups = match.Groups;
											string before = groups["Before"].Value;
											if (before.Length != 0)
											{
												NormalizeObjectTypeName(null, before, style, removeSeparatedCharacters, builder, ref spacePending, ref makeLower);
											}
											switch (int.Parse(groups["ReplaceIndex"].Value))
											{
												case 0:
													NormalizeObjectTypeName(null, preferredFor.Name, style, removeSeparatedCharacters, builder, ref spacePending, ref makeLower);
													break;
												case 1:
													string modeName = mode.Name;
													if (modeType == ReferenceModeType.UnitBased)
													{
														if (style == ObjectTypeNameVerbalizationStyle.SeparateCombinedNames &&
															spacePending)
														{
															builder.Append(" ");
														}
														builder.Append(modeName);
														makeLower = false;
														spacePending = true;
													}
													else
													{
														NormalizeObjectTypeName(null, modeName, style, removeSeparatedCharacters, builder, ref spacePending, ref makeLower);
													}
													break;
											}
											trailingTextIndex += match.Length;
											match = match.NextMatch();
										}
										if (trailingTextIndex < modeFormatString.Length)
										{
											NormalizeObjectTypeName(null, modeFormatString.Substring(trailingTextIndex), style, removeSeparatedCharacters, builder, ref spacePending, ref makeLower);
										}
										return returnText ? builder.ToString() : null;
									}
								}
							}
						}
					}
				}
			}
			switch (style)
			{
				case ObjectTypeNameVerbalizationStyle.CombineNamesLeadWithLower:
				case ObjectTypeNameVerbalizationStyle.CombineNamesLeadWithUpper:
					{
						Match match = Utility.MatchNameParts(originalName);
						bool returnText;
						if (returnText = (builder == null))
						{
							builder = new StringBuilder();
						}
						while (match.Success)
						{
							string matchText = match.Value;
							GroupCollection groups = match.Groups;
							if (groups["TrailingUpper"].Success)
							{
								builder.Append(matchText);
								makeLower = false;
							}
							else if (groups["PunctuationOrSymbol"].Success)
							{
								if (removeSeparatedCharacters == null || !removeSeparatedCharacters.Contains(matchText))
								{
									builder.Append(matchText);
									// Leave the upper/lower state alone until we see something other than a symbol
								}
							}
							else if (makeLower)
							{
								makeLower = false;
								builder.Append(Utility.LowerCaseFirstLetter(matchText));
							}
							else
							{
								builder.Append(Utility.UpperCaseFirstLetter(matchText));
							}
							match = match.NextMatch();
						}
						return returnText ? builder.ToString() : null;
					}
				case ObjectTypeNameVerbalizationStyle.SeparateCombinedNames:
					if (Utility.IsMultiPartName(originalName))
					{
						bool returnText;
						if (returnText = (builder == null))
						{
							builder = new StringBuilder();
						}
						Match match = Utility.MatchNameParts(originalName);
						while (match.Success)
						{
							GroupCollection groups = match.Groups;
							string matchText = match.Value;
							if (groups["TrailingUpper"].Success)
							{
								// Multiple caps, leave as is
								if (spacePending)
								{
									builder.Append(" ");
								}
								builder.Append(matchText);
								spacePending = true;
							}
							else if (groups["PunctuationOrSymbol"].Success)
							{
								// This returns a single symbol. Either append it or do nothing
								// and leave the space pending setting untouched.
								if (removeSeparatedCharacters == null || !removeSeparatedCharacters.Contains(matchText))
								{
									builder.Append(matchText);
									spacePending = false;
								}
							}
							else if (groups["Numeric"].Success)
							{
								// No space added, no casing
								builder.Append(matchText);
								spacePending = true;
							}
							else
							{
								if (spacePending)
								{
									builder.Append(" ");
								}
								builder.Append(Utility.LowerCaseFirstLetter(matchText));
								spacePending = true;
							}
							match = match.NextMatch();
						}
						return returnText ? builder.ToString() : null;
					}
					else if (originalName.Length == 1 &&
						IsPunctuationOrSymbol.Match(originalName).Success)
					{
						// Case is not caught by 'IsMultiPartName', but spacing is different than
						// the non-symbol case.
						if (removeSeparatedCharacters == null || !removeSeparatedCharacters.Contains(originalName))
						{
							spacePending = false; // Don't space around symbols, ignore current spacePending value
							if (builder != null)
							{
								builder.Append(originalName);
								return null;
							}
							return originalName;
						}
						if (spacePending)
						{
							spacePending = false;
							if (builder != null)
							{
								builder.Append(" ");
								return null;
							}
							return " ";
						}
						return builder != null ? null : "";
					}
					else if (builder != null)
					{
						if (spacePending)
						{
							builder.Append(" ");
						}
						builder.Append(Utility.LowerCaseFirstLetter(originalName));
						spacePending = true;
						return null;
					}
					spacePending = true;
					return Utility.LowerCaseFirstLetter(originalName);
				//case ObjectTypeNameVerbalizationStyle.AsIs:
				default:
					if (builder != null)
					{
						builder.Append(originalName);
						return null;
					}
					return originalName;
			}
		}
		#endregion // NormalizeObjectTypeName method
		#region GetDocumentHeaderReplacementFields method
		/// <summary>
		/// Get the 8 document header replacement fields from the current font and color settings.
		/// Colors are returned in HTML format.
		/// </summary>
		public static string[] GetDocumentHeaderReplacementFields(Store store, IVerbalizationSets<CoreVerbalizationSnippetType> snippets)
		{
			// The replacement fields, pulled from VerbalizationGenerator.xsd
			//{0} font-family
			//{1} font-size
			//{2} predicate text color
			//{3} predicate text bold
			//{4} object name color
			//{5} object name bold
			//{6} formal item color
			//{7} formal item bold
			//{8} notes item color
			//{9} notes item bold
			//{10} refmode item color
			//{11} refmode item bold
			//{12} instance value item color
			//{13} instance value item bold
			string boldWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightBold);
			string normalWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightNormal);
			string[] retVal = new string[] { "Tahoma", "8", "darkgreen", normalWeight, "purple", normalWeight, "mediumblue", boldWeight, "black", normalWeight, "brown", normalWeight, "brown", normalWeight };
			IORMFontAndColorService colorService = ((IORMToolServices)store).FontAndColorService;
			using (Font font = colorService.GetFont(ORMDesignerColorCategory.Verbalizer))
			{
				retVal[0] = font.FontFamily.Name;
				retVal[1] = (font.Size * 72f).ToString(CultureInfo.InvariantCulture);
				retVal[2] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerPredicateText));
				retVal[3] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerPredicateText) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[4] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerObjectName));
				retVal[5] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerObjectName) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[6] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerFormalItem));
				retVal[7] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerFormalItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[8] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerNotesItem));
				retVal[9] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerNotesItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[10] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerRefMode));
				retVal[11] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerRefMode) & FontStyle.Bold)) ? boldWeight : normalWeight;
				retVal[12] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerInstanceValue));
				retVal[13] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerInstanceValue) & FontStyle.Bold)) ? boldWeight : normalWeight;
				// Changes here need to be synchronized with corresponding changes in Core.Load.VerbalizationManager.GetFontColor and GetFontBold
			}
			return retVal;
		}
		#endregion // GetDocumentHeaderReplacementFields method
	}
	#endregion // VerbalizationHelper class
	#region VerbalizationSubscripter class
	/// <summary>
	/// Helper class for generated subscripting code. Completes subscripting
	/// on demand, so that subscripted elements are numbered in order of first
	/// appearance.
	/// </summary>
	public struct VerbalizationSubscripter
	{
		private Dictionary<string, int> myLastSubscripts;
		private IFormatProvider myFormatProvider;
		/// <summary>
		/// Create a <see cref="VerbalizationSubscripter"/>.
		/// </summary>
		/// <param name="formatProvider">The current format provider.</param>
		public VerbalizationSubscripter(IFormatProvider formatProvider)
		{
			myFormatProvider = formatProvider;
			myLastSubscripts = null;
		}
		/// <summary>
		/// Get a subscripted name for the specified role replacement data.
		/// </summary>
		/// <param name="index">The array index of the first dimension of the
		/// replacement data.</param>
		/// <param name="replacementData">An array with three columns. The first column
		/// is the unsubscripted data, the second is either a format string or the final
		/// subscripted data, the third is either null (indicating that subscripting is not
		/// used for this item), the empty string (indicating that the subscripted
		/// data is still a format string), or a non-null string (the format string originally
		/// stored in the main second column). Storing the original format string enables
		/// the data to be reset.</param>
		/// <returns>The subscripted name.</returns>
		public string GetSubscriptedName(int index, string[,] replacementData)
		{
			string data = replacementData[index, 1];
			string formatData = replacementData[index, 2];
			if (null != formatData && formatData.Length == 0)
			{
				Dictionary<string, int> dict = myLastSubscripts;
				string key = replacementData[index, 0];
				int subscript;
				if (dict == null)
				{
					myLastSubscripts = dict = new Dictionary<string, int>();
					subscript = 1;
				}
				else if (dict.TryGetValue(key, out subscript))
				{
					++subscript;
				}
				else
				{
					subscript = 1;
				}
				dict[key] = subscript;
				replacementData[index, 2] = data;
				IFormatProvider formatProvider = myFormatProvider;
				replacementData[index, 1] = data = string.Format(formatProvider, data, subscript.ToString(formatProvider));
			}
			return data;
		}
		/// <summary>
		/// Prepare a subscripted format string for delayed subscripting with
		/// <see cref="GetSubscriptedName"/>.
		/// </summary>
		/// <param name="subscriptedFormatString">The format string for the subscripted name.
		/// The string has three replacements. 0=the name to subscript, 1=id of subscripted element,
		/// 2=field for the subscript number.</param>
		/// <param name="subscriptedElementName">The name of the element to subscript.</param>
		/// <param name="subscriptedElementId">The id of the element to subscript</param>
		/// <returns>Format string appropriate for use with <see cref="GetSubscriptedName"/>.</returns>
		public string PrepareSubscriptFormatString(string subscriptedFormatString, string subscriptedElementName, string subscriptedElementId)
		{
			return string.Format(myFormatProvider, subscriptedFormatString, subscriptedElementName.Replace("{", "{{").Replace("}", "}}"), subscriptedElementId.Replace("{", "{{").Replace("}", "}}"), "{0}");
		}
		/// <summary>
		/// Reset the subscript data for replacement fields.
		/// </summary>
		/// <param name="allReplacements">Replacement data created using <see cref="PrepareSubscriptFormatString"/> and
		/// possibly modified with <see cref="GetSubscriptedName"/>.</param>
		public void ResetSubscripts(string[][,] allReplacements)
		{
			Dictionary<string, int> dict = myLastSubscripts;
			if (dict != null && dict.Count != 0)
			{
				dict.Clear();
				for (int i = 0; i < allReplacements.Length; ++i)
				{
					string[,] replacements = allReplacements[i];
					int length = replacements.GetLength(0);
					for (int j = 0; j < length; ++j)
					{
						string replacementData = replacements[j, 2];
						if (replacementData != null && replacementData.Length != 0)
						{
							replacements[j, 1] = replacementData;
							replacements[j, 2] = string.Empty;
						}
					}
				}
			}
		}
	}
	#endregion // Verbalization Subscripter class
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
	#region RolePathRolePlayerRenderingOptions enum
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
		/// <summary>
		/// The role player directly associated with a role player key should be replaced
		/// with a role player of a resolved supertype. Determination of the associated
		/// resolved supertype is dependent on the type of verbalization.
		/// </summary>
		ResolveSupertype = 8,
	}
	#endregion // RolePathRolePlayerRenderingOptions enum
	#region IRolePathRendererContext interface
	/// <summary>
	/// Provide callback services to the verbalization engine
	/// making requests on the <see cref="IRolePathRenderer"/> interface.
	/// </summary>
	public interface IRolePathRendererContext
	{
		/// <summary>
		/// Render a role player with appropriate subscripting, formatting, and quantification.
		/// </summary>
		/// <param name="rolePlayerFor">The key to this role player.</param>
		/// <param name="hyphenBindingFormatString">The hyphen bound format string for the replacement role. If this
		/// is provided, then there is a single replacement field for the role player, and any additional quantification
		/// should treat the hyphen-bound text as a single unit.</param>
		/// <param name="renderingOptions">Options from the <see cref="RolePathRolePlayerRenderingOptions"/> values.</param>
		/// <returns>String replacement field with formatting and subscripting applied
		/// by the <see cref="IRolePathRenderer.RenderRolePlayer"/></returns>
		string RenderAssociatedRolePlayer(object rolePlayerFor, string hyphenBindingFormatString, RolePathRolePlayerRenderingOptions renderingOptions);
	}
	#endregion // IRolePathRendererContext interface
	#region IRolePathRenderer interface
	/// <summary>
	/// Rendering interface for the <see cref="RolePathVerbalizer"/> class
	/// </summary>
	public interface IRolePathRenderer : IVerbalizationSets<CoreVerbalizationSnippetType>
	{
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
		/// <param name="valueConstraint">The <see cref="PathConditionRoleValueConstraint"/> or <see cref="PathConditionRootValueConstraint"/> to render.</param>
		/// <param name="pathContext">The path use context this value constraint applies to.</param>
		/// <param name="rendererContext">The rendering context. Used to retrieve variable names.</param>
		/// <param name="builder">A <see cref="StringBuilder"/> for scratch strings. Return to original position on exit.</param>
		/// <returns>Formatted string describing the value constraint</returns>
		string RenderValueCondition(ValueConstraint valueConstraint, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder);
		/// <summary>
		/// Render a calculation
		/// </summary>
		/// <param name="calculation">The <see cref="CalculatedPathValue"/> to render.</param>
		/// <param name="pathContext">The path use context this value constraint applies to.</param>
		/// <param name="rendererContext">The rendering context. Used to retrieve variable names.</param>
		/// <param name="builder">A <see cref="StringBuilder"/> for scratch strings. Return
		/// to original position on exit.</param>
		/// <returns>Formatted string of the calculation.</returns>
		string RenderCalculation(CalculatedPathValue calculation, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder);
		/// <summary>
		/// Render a list of equivalent variables
		/// </summary>
		/// <param name="equivalentVariableKeys">A list of variable keys to render as equivalent.</param>
		/// <param name="renderingOptions">Rendering options to apply to keyed variables.</param>
		/// <param name="pairingSnippet">A snippet type used to combined paired variables, or -1 for the default equality list.</param>
		/// <param name="pathContext">The path use context these keys apply to.</param>
		/// <param name="rendererContext">The rendering context. Used to retrieve variable names.</param>
		/// <param name="builder">A <see cref="StringBuilder"/> for scratch strings. Return
		/// to original position on exit.</param>
		/// <returns>Formatted string of the equivalence.</returns>
		string RenderVariableEquivalence(IList<object> equivalentVariableKeys, RolePathRolePlayerRenderingOptions renderingOptions, CoreVerbalizationSnippetType pairingSnippet, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder);
		/// <summary>
		/// Render a constant value
		/// </summary>
		/// <param name="constant">The <see cref="PathConstant"/> to render.</param>
		/// <returns>Formatted string with the constant.</returns>
		string RenderConstant(PathConstant constant);
	}
	#endregion // IRolePathRenderer interface
	#region StandardRolePathRenderer class
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
		private IDictionary<string, object> myVerbalizationOptions;
		private IFormatProvider myFormatProvider;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a new <see cref="StandardRolePathRenderer"/>
		/// </summary>
		/// <param name="coreSnippets">Core verbalization snippets</param>
		/// <param name="verbalizationContext">The current verbalization context</param>
		/// <param name="formatProvider">Context format provider</param>
		public StandardRolePathRenderer(IVerbalizationSets<CoreVerbalizationSnippetType> coreSnippets, IVerbalizationContext verbalizationContext, IFormatProvider formatProvider)
		{
			myCoreSnippets = coreSnippets;
			myFormatProvider = formatProvider;
			myVerbalizationOptions = verbalizationContext.VerbalizationOptions;
		}
		#endregion // Constructor
		#region IRolePathRenderer Implementation
		/// <summary>
		/// Implements <see cref="IVerbalizationSets{CoreVerbalizationSnippetType}.GetSnippet(CoreVerbalizationSnippetType,System.Boolean,System.Boolean)"/>
		/// </summary>
		protected string GetSnippet(CoreVerbalizationSnippetType snippetType, bool isDeontic, bool isNegative)
		{
			return myCoreSnippets.GetSnippet(snippetType, isDeontic, isNegative);
		}
		string IVerbalizationSets<CoreVerbalizationSnippetType>.GetSnippet(CoreVerbalizationSnippetType snippetType, bool isDeontic, bool isNegative)
		{
			return GetSnippet(snippetType, isDeontic, isNegative);
		}
		/// <summary>
		/// Implements <see cref="IVerbalizationSets{CoreVerbalizationSnippetType}.GetSnippet(CoreVerbalizationSnippetType)"/>
		/// </summary>
		protected string GetSnippet(CoreVerbalizationSnippetType snippetType)
		{
			return myCoreSnippets.GetSnippet(snippetType);
		}
		string IVerbalizationSets<CoreVerbalizationSnippetType>.GetSnippet(CoreVerbalizationSnippetType snippetType)
		{
			return GetSnippet(snippetType);
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
					return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectType), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer.Name, myVerbalizationOptions), idString);
				}
				return string.Format(myFormatProvider, snippets.GetSnippet(CoreVerbalizationSnippetType.ObjectTypeWithSubscript), VerbalizationHelper.NormalizeObjectTypeName(rolePlayer.Name, myVerbalizationOptions), idString, subscript);
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
		protected string RenderValueCondition(ValueConstraint valueConstraint, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
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
			string variableSnippet1Replace1 = "";
			PathConditionRoleValueConstraint pathedRoleValueConstraint;
			PathConditionRootValueConstraint pathRootValueConstraint;
			if (null != (pathedRoleValueConstraint = valueConstraint as PathConditionRoleValueConstraint))
			{
				variableSnippet1Replace1 = rendererContext.RenderAssociatedRolePlayer(new RolePathNode(pathedRoleValueConstraint.PathedRole, pathContext), null, RolePathRolePlayerRenderingOptions.Quantify);
			}
			else if (null != (pathRootValueConstraint = valueConstraint as PathConditionRootValueConstraint))
			{
				variableSnippet1Replace1 = rendererContext.RenderAssociatedRolePlayer(new RolePathNode(pathRootValueConstraint.PathRoot, pathContext), null, RolePathRolePlayerRenderingOptions.Quantify);
			}
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
		string IRolePathRenderer.RenderValueCondition(ValueConstraint valueConstraint, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			return RenderValueCondition(valueConstraint, pathContext, rendererContext, builder);
		}
		/// <summary>
		/// Implements <see cref="IRolePathRenderer.RenderCalculation"/>
		/// </summary>
		protected string RenderCalculation(CalculatedPathValue calculation, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			int restoreBuilder = builder.Length;
			Function function = calculation.Function;
			FunctionParameter parameter;
			CalculatedPathValueInput input;
			if (function != null)
			{
				RolePathNode[] aggregationContext = null;
				if (function.IsAggregate)
				{
					LinkedElementCollection<RolePathObjectTypeRoot> aggregateRoots = calculation.AggregationContextPathRootCollection;
					int aggregateRootCount = aggregateRoots.Count;
					LinkedElementCollection<PathedRole> aggregateRoles = calculation.AggregationContextPathedRoleCollection;
					int aggregateRoleCount = aggregateRoles.Count;
					int aggregateCount = aggregateRootCount + aggregateRoleCount;
					if (aggregateCount != 0)
					{
						aggregationContext = new RolePathNode[aggregateCount];
						aggregateCount = -1;
						foreach (RolePathObjectTypeRoot pathRoot in aggregateRoots)
						{
							aggregationContext[++aggregateCount] = new RolePathNode(pathRoot, pathContext);
						}
						foreach (PathedRole pathedRole in aggregateRoles)
						{
							aggregationContext[++aggregateCount] = new RolePathNode(pathedRole, pathContext);
						}
					}
				}
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
							builder.Append(RenderParameter(pathContext, inputs[0], parameter, aggregationContext, rendererContext, builder));
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
									builder.Append(RenderParameter(pathContext, input, parameter, aggregationContext, rendererContext, builder));
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
								builder.Append(RenderParameter(pathContext, input, parameter, aggregationContext, rendererContext, builder));
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
		string IRolePathRenderer.RenderCalculation(CalculatedPathValue calculation, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			return RenderCalculation(calculation, pathContext, rendererContext, builder);
		}
		private string RenderVariableEquivalence(IList<object> equivalentVariableKeys, RolePathRolePlayerRenderingOptions renderingOptions, CoreVerbalizationSnippetType pairingSnippet, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			int restoreBuilder = builder.Length;
			int varCount = equivalentVariableKeys.Count;
			if (pairingSnippet != (CoreVerbalizationSnippetType)(-1) &&
				varCount == 2)
			{
				return string.Format(myFormatProvider, myCoreSnippets.GetSnippet(pairingSnippet), rendererContext.RenderAssociatedRolePlayer(equivalentVariableKeys[0], null, renderingOptions), rendererContext.RenderAssociatedRolePlayer(equivalentVariableKeys[1], null, renderingOptions));
			}
			else
			{
				for (int i = 0; i < varCount; ++i)
				{
					if (i != 0)
					{
						builder.Append(" = ");
					}
					// UNDONE: Consider adding a sorting mechanism based on name then subscript for ensuring that
					// the variables in this list are consistently ordered.
					builder.Append(rendererContext.RenderAssociatedRolePlayer(equivalentVariableKeys[i], null, renderingOptions));
				}
				string result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
				builder.Length = restoreBuilder;
				return result;
			}
		}
		string IRolePathRenderer.RenderVariableEquivalence(IList<object> equivalentVariableKeys, RolePathRolePlayerRenderingOptions renderingOptions, CoreVerbalizationSnippetType pairingSnippet, object pathContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			return RenderVariableEquivalence(equivalentVariableKeys, renderingOptions, pairingSnippet, pathContext, rendererContext, builder);
		}
		private string RenderParameter(object pathContext, CalculatedPathValueInput calculatedValueInput, FunctionParameter parameter, RolePathNode[] aggregationContext, IRolePathRendererContext rendererContext, StringBuilder builder)
		{
			int restoreBuilder = builder.Length;
			PathedRole sourceRole;
			RolePathObjectTypeRoot sourceRoot;
			CalculatedPathValue sourceCalculation;
			PathConstant sourceConstant;
			if (null != (sourceRole = calculatedValueInput.SourcePathedRole))
			{
				builder.Append(rendererContext.RenderAssociatedRolePlayer(new RolePathNode(sourceRole, pathContext), null, RolePathRolePlayerRenderingOptions.None));
			}
			else if (null != (sourceRoot = calculatedValueInput.SourcePathRoot))
			{
				builder.Append(rendererContext.RenderAssociatedRolePlayer(new RolePathNode(sourceRoot, pathContext), null, RolePathRolePlayerRenderingOptions.None));
			}
			else if (null != (sourceCalculation = calculatedValueInput.SourceCalculatedValue))
			{
				builder.Append(RenderCalculation(sourceCalculation, pathContext, rendererContext, builder));
			}
			else if (null != (sourceConstant = calculatedValueInput.SourceConstant))
			{
				builder.Append(RenderConstant(sourceConstant));
			}
			string result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
			builder.Length = restoreBuilder;
			if (parameter.BagInput)
			{
				if (aggregationContext != null)
				{
					string scopeVariable;
					if (aggregationContext.Length == 1)
					{
						scopeVariable = rendererContext.RenderAssociatedRolePlayer(aggregationContext[0], null, RolePathRolePlayerRenderingOptions.None);
						if (!string.IsNullOrEmpty(scopeVariable))
						{
							result = string.Format(myFormatProvider, GetSnippet(CoreVerbalizationSnippetType.AggregateParameterDecorator), result, string.Format(myFormatProvider, GetSnippet(CoreVerbalizationSnippetType.AggregateParameterSimpleAggregationContext), scopeVariable));
						}
					}
					else
					{
						int processed = 0;
						string firstScopeVariable = null;
						for (int i = 0; i < aggregationContext.Length; ++i)
						{
							scopeVariable = rendererContext.RenderAssociatedRolePlayer(aggregationContext[i], null, RolePathRolePlayerRenderingOptions.None);
							if (!string.IsNullOrEmpty(scopeVariable))
							{
								switch (processed)
								{
									case 0:
										firstScopeVariable = scopeVariable;
										++processed;
										break;
									case 1:
										builder.Append(GetSnippet(CoreVerbalizationSnippetType.AggregateParameterComplexAggregationContextListOpen));
										builder.Append(firstScopeVariable);
										goto default;
									default:
										builder.Append(GetSnippet(CoreVerbalizationSnippetType.AggregateParameterComplexAggregationContextListSeparator));
										builder.Append(scopeVariable);
										++processed;
										break;
								}
							}
						}
						if (processed != 0)
						{
							string aggregationDescription;
							if (processed == 1)
							{
								aggregationDescription = string.Format(myFormatProvider, GetSnippet(CoreVerbalizationSnippetType.AggregateParameterSimpleAggregationContext), firstScopeVariable);
							}
							else
							{
								builder.Append(GetSnippet(CoreVerbalizationSnippetType.AggregateParameterComplexAggregationContextListClose));
								aggregationDescription = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
								builder.Length = restoreBuilder;
							}
							result = string.Format(myFormatProvider, GetSnippet(CoreVerbalizationSnippetType.AggregateParameterDecorator), result, aggregationDescription);
						}
					}
				}
				result = string.Format(myFormatProvider, GetSnippet(calculatedValueInput.DistinctValues ? CoreVerbalizationSnippetType.AggregateSetProjection : CoreVerbalizationSnippetType.AggregateBagProjection), result);
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
	#endregion // StandardRolePathRenderer class
	#region RolePathVerbalizerOptions enum
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
	#endregion // RolePathVerbalizerOptions enum
	#region CoreSnippetIdentifier struct
	/// <summary>
	/// Identify a specific snippet from the <see cref="CoreVerbalizationSnippetType"/> snippets
	/// </summary>
	public struct CoreSnippetIdentifier
	{
		/// <summary>
		/// Create an unambiguous identifier for a core snippet.
		/// </summary>
		public CoreSnippetIdentifier(CoreVerbalizationSnippetType snippet, bool isDeontic, bool isNegative)
		{
			Snippet = snippet;
			IsDeontic = isDeontic;
			IsNegative = isNegative;
		}
		/// <summary>
		/// The enumerated snippet value
		/// </summary>
		public readonly CoreVerbalizationSnippetType Snippet;
		/// <summary>
		/// True to use a deontic snippet
		/// </summary>
		public readonly bool IsDeontic;
		/// <summary>
		/// True to use a negated snippet
		/// </summary>
		public readonly bool IsNegative;
	}
	#endregion // CoreSnippetIdentifier struct
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
				/// <summary>
				/// A custom correlated variable is correlated with another list
				/// of variables via an inter-path key, such as the keys used
				/// with external variables or with embedded subqueries.
				/// </summary>
				IsCustomCorrelated = 8,
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
			/// Return the current use phase for this variable
			/// </summary>
			public int UsePhase
			{
				get
				{
					return myUsePhase;
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
			/// Set if the variable is used externally to the path verbalization.
			/// </summary>
			public bool IsCustomCorrelated
			{
				get
				{
					return 0 != (myFlags & StateFlags.IsCustomCorrelated);
				}
				set
				{
					if (value)
					{
						myFlags |= StateFlags.IsCustomCorrelated;
					}
					else
					{
						myFlags &= ~StateFlags.IsCustomCorrelated;
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
			private readonly object myCorrelationRoot;
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
			/// is keyed off of another path node (pathed role or path root). Corresponds to
			/// the normalized correlation root used to register a correlated projection variable.</param>
			public RolePlayerVariableUse(RolePlayerVariable primaryVariable, RolePlayerVariable joinedToVariable, object correlationRoot)
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
			/// <remarks>If the variable use is keyed off a path node (pathed role or path
			/// root), then correlated uses should only be added to the correlation root.
			/// If the variable use is not keyed off a pathed node, then the correlated
			/// uses of the two variable uses unioned with the primary variable should be
			/// the same set.</remarks>
			public bool AddCorrelatedVariable(RolePlayerVariable variable)
			{
				if (myPrimaryVariable == variable || variable == null)
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
			/// Retrieve the normalized correlation root associated with the key to
			/// this variable use.
			/// </summary>
			public object CorrelationRoot
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
				public readonly LinkedElementCollection<RoleSubPath> SubPaths;
				public readonly RolePath ParentPath;
				public readonly ObjectType RootObjectType;
				public readonly RolePathObjectTypeRoot RootObjectTypeLink;
				public PathInfo(RolePath path)
				{
					// We'll need all of the fields eventually, get them all up front
					PathedRoles = path.PathedRoleCollection;
					SubPaths = path.SubPathCollection;
					RoleSubPath subPath = path as RoleSubPath;
					ParentPath = subPath != null ? subPath.ParentRolePath : null;
					RootObjectTypeLink = path.PathRoot;
					RootObjectType = (RootObjectTypeLink != null) ? RootObjectTypeLink.RootObjectType : null;
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
			public LinkedElementCollection<RoleSubPath> SubPathCollection(RolePath rolePath)
			{
				return GetPathInfo(rolePath).SubPaths;
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
			/// Get the cached <see cref="RolePath.RootObjectType"/> for a given <see cref="RolePath"/>.
			/// </summary>
			public ObjectType RootObjectType(RolePath rolePath)
			{
				return GetPathInfo(rolePath).RootObjectType;
			}
			/// <summary>
			/// Get the cached <see cref="RolePathObjectTypeRoot"/> for a the root of a given <see cref="RolePath"/>.
			/// </summary>
			public RolePathObjectTypeRoot RootObjectTypeLink(RolePath rolePath)
			{
				return GetPathInfo(rolePath).RootObjectTypeLink;
			}
			/// <summary>
			/// Enumerate all pathed nodes preceding a specified role
			/// </summary>
			/// <param name="startNode">The initial <see cref="RolePathNode"/></param>
			/// <param name="visitStartNode"><see langword="true"/> if the <paramref name="startNode"/> should
			/// be included in the enumeration.</param>
			/// <returns>Enumeration</returns>
			public IEnumerable<RolePathNode> GetPrecedingPathNodes(RolePathNode startNode, bool visitStartNode)
			{
				object pathContext = startNode.Context;
				PathedRole startRole = startNode;
				PathInfo pathInfo;
				ReadOnlyCollection<PathedRole> pathedRoles;
				RolePath parentPath;
				int pathedRoleIndex;
				if (startRole != null)
				{
					pathInfo = GetPathInfo(startRole.RolePath);
					pathedRoles = pathInfo.PathedRoles;
					pathedRoleIndex = (pathedRoles.Count == 1 ? 0 : pathedRoles.IndexOf(startRole)) - (visitStartNode ? 0 : 1);
				}
				else
				{
					if (visitStartNode)
					{
						yield return startNode;
					}
					parentPath = GetPathInfo(startNode.PathRoot.RolePath).ParentPath;
					if (parentPath == null)
					{
						yield break;
					}
					pathInfo = GetPathInfo(parentPath);
					pathedRoles = pathInfo.PathedRoles;
					pathedRoleIndex = pathedRoles.Count - 1;
				}
				for (; ; )
				{
					for (int i = pathedRoleIndex; i >= 0; --i)
					{
						yield return new RolePathNode(pathedRoles[i], pathContext);
					}
					RolePathObjectTypeRoot pathRoot;
					if (null != (pathRoot = pathInfo.RootObjectTypeLink))
					{
						yield return new RolePathNode(pathRoot, pathContext);
					}
					parentPath = pathInfo.ParentPath;
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
			/// Get a correlation root object for a <see cref="RolePathNode"/>
			/// </summary>
			/// <param name="pathNode">A non-empty <see cref="RolePathNode"/>.</param>
			/// <returns>A <see cref="PathedRole"/>, <see cref="RolePath"/>, or <see cref="PathObjectUnifier"/>
			/// that represents the normalized correlation root for provided node.</returns>
			/// <remarks>Correlations are limited to the current path and do not include any
			/// path context information, so should not be used directly as key values in collections
			/// that assume context information.</remarks>
			public object GetCorrelationRoot(RolePathNode pathNode)
			{
				PathedRole pathedRole = pathNode;
				RolePathObjectTypeRoot pathRoot = null;
				object pathContext = pathNode.Context;
				if (pathedRole != null)
				{
					switch (pathedRole.PathedRolePurpose)
					{
						case PathedRolePurpose.PostInnerJoin:
						case PathedRolePurpose.PostOuterJoin:
							foreach (RolePathNode precedingPathNode in GetPrecedingPathNodes(new RolePathNode(pathedRole, pathContext), false))
							{
								PathedRole precedingPathedRole = precedingPathNode;
								if (precedingPathedRole != null)
								{
									if (precedingPathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
									{
										pathedRole = precedingPathedRole;
										break;
									}
								}
								else
								{
									pathRoot = precedingPathNode;
									break;
								}
							}
							break;
					}
					if (pathRoot == null)
					{
						PathObjectUnifier objectUnifier = pathedRole.ObjectUnifier;
						if (objectUnifier != null)
						{
							return objectUnifier;
						}
						if (pathedRole.Role is SubtypeMetaRole &&
							pathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
						{
							foreach (RolePathNode precedingPathNode in GetPrecedingPathNodes(new RolePathNode(pathedRole, pathContext), false))
							{
								// UNDONE: Consider supporting this pattern for negation as well.
								PathedRole precedingPathedRole = precedingPathNode;
								if (precedingPathedRole != null &&
									precedingPathedRole.Role is SupertypeMetaRole &&
									!precedingPathedRole.IsNegated)
								{
									return GetCorrelationRoot(new RolePathNode(precedingPathedRole, pathContext));
								}
								break;
							}
						}
						return pathedRole;
					}
				}
				else
				{
					pathRoot = pathNode;
				}
				if (pathRoot != null)
				{
					return (object)pathRoot.ObjectUnifier ?? pathRoot;
				}
				return null; // Fallback
			}
			/// <summary>
			/// Cache-assisted version of <see cref="PathedRole.PreviousPathNode"/>
			/// and <see cref="RolePathObjectTypeRoot.PreviousPathNode"/>
			/// </summary>
			public RolePathNode GetPreviousPathNode(RolePathNode pathNode)
			{
				foreach (RolePathNode previousNode in GetPrecedingPathNodes(pathNode, false))
				{
					return previousNode;
				}
				return RolePathNode.Empty;
			}
			/// <summary>
			/// True if the cache is initialized
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
		#region InlineSubqueryContext class
		/// <summary>
		/// A class representing an inline expansion of a subquery. A single
		/// subquery can be used multiple times (directly or nested) within a
		/// single derivation path, so the nodes in the query derivation path
		/// are insufficient to uniquely identify the inlined elements within
		/// the outer structure. A subquery context represents a parent context,
		/// the use of a subquery in that parent context (identified by the pathed
		/// role used to enter the query), and the lead role path for the query
		/// derivation rule being processed. For nested subqueries, the parent
		/// context can itself be another subquery context. Otherwise, the parent
		/// context is null. When combined with nodes in the query derivation rule,
		/// these context elements form an unambiguous identification scheme for
		/// each used path node.
		/// </summary>
		/// <remarks>This construct may be abstracted/expanded/renamed/duplicated in the future.
		/// The only difference between expanding a subquery inline and expanding fact type
		/// or subtype derivations inline is that the subqueries support parameterization
		/// in addition to bound roles. A feature enabling inline expansion of one or
		/// more referenced fact types in a derivation may be desirable.</remarks>
		private sealed class InlineSubqueryContext
		{
			#region Member variables
			private readonly object myParentContext;
			private readonly PathedRole myEntryRole;
			private readonly LeadRolePath myQueryPath;
			#endregion // Member variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="InlineSubqueryContext"/>
			/// </summary>
			/// <param name="parentContext">The context representing the derivation path that invokes this subquery.</param>
			/// <param name="parentEntryRole">The <see cref="PathedRole"/> in the context path used to invoke an instance of this subquery.</param>
			/// <param name="queryPath">The <see cref="LeadRolePath"/> owned by the query derivation rule used as the body of the query.</param>
			public InlineSubqueryContext(object parentContext, PathedRole parentEntryRole, LeadRolePath queryPath)
			{
				myParentContext = parentContext;
				myEntryRole = parentEntryRole;
				myQueryPath = queryPath;
			}
			#endregion // Constructor
			#region Accessor Properties
			/// <summary>
			/// Retrieve the parent context for this invocation of a subquery.
			/// </summary>
			public object ParentContext
			{
				get
				{
					return myParentContext;
				}
			}
			/// <summary>
			/// Retrieve the entry role in the parent context used to invoke this subquery.
			/// </summary>
			public PathedRole ParentEntryRole
			{
				get
				{
					return myEntryRole;
				}
			}
			/// <summary>
			/// Retrieve the role path used for the body of the subquery.
			/// </summary>
			public LeadRolePath QueryPath
			{
				get
				{
					return myQueryPath;
				}
			}
			#endregion // Accessor Properties
			#region Equality overrides
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				InlineSubqueryContext typedObj;
				return (null != (typedObj = obj as InlineSubqueryContext)) && this.Equals(typedObj);
			}
			/// <summary>
			/// Standard GetHashCode override
			/// </summary>
			public override int GetHashCode()
			{
				object context = myParentContext;
				return context != null ? Utility.GetCombinedHashCode(myEntryRole.GetHashCode(), myQueryPath.GetHashCode(), context.GetHashCode()) : Utility.GetCombinedHashCode(myEntryRole.GetHashCode(), myQueryPath.GetHashCode());
			}
			/// <summary>
			/// Typed Equals method
			/// </summary>
			public bool Equals(InlineSubqueryContext other)
			{
				object context = myParentContext;
				return other != null &&
					((context == null) ? other.myParentContext == null : context.Equals(other.myParentContext)) && // Context objects may be recreated, use Equals instead of ==
					myEntryRole == other.myEntryRole &&
					myQueryPath == other.myQueryPath;
			}
			/// <summary>
			/// Equality operator
			/// </summary>
			public static bool operator ==(InlineSubqueryContext left, InlineSubqueryContext right)
			{
				return ((object)left != null) ? left.Equals(right) : (object)right == null;
			}
			/// <summary>
			/// Inequality operator
			/// </summary>
			public static bool operator !=(InlineSubqueryContext left, InlineSubqueryContext right)
			{
				return ((object)left != null) ? !left.Equals(right) : (object)right != null;
			}
			#endregion // Equality overrides
		}
		#endregion // InlineSubqueryContext class
		#region InlineSubqueryRole class
		/// <summary>
		/// A class representing a role in an inline expansion of a subquery. A single
		/// subquery can be used multiple times (directly or nested) within a
		/// single derivation path, so the nodes in the query derivation path
		/// are insufficient to uniquely identify the inlined elements within
		/// the outer structure. A subquery role provides a key for a single role in
		/// a subquery use by representing the parent context, the use of a subquery in
		/// that context (identified by the pathed role used to enter the query), and the
		/// role in the subquery.
		/// </summary>
		/// <remarks>See remarks on <see cref="InlineSubqueryContext"/>.</remarks>
		private sealed class InlineSubqueryRole
		{
			#region Member variables
			private readonly object myParentContext;
			private readonly PathedRole myEntryPathedRole;
			private readonly Role myRole;
			#endregion // Member variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="InlineSubqueryRole"/>
			/// </summary>
			/// <param name="parentContext">The context representing the derivation path that invokes this subquery.</param>
			/// <param name="parentEntryRole">The <see cref="PathedRole"/> in the context path used to invoke an instance of this subquery.</param>
			/// <param name="role">The <see cref="Role"/> in the subquery definition.</param>
			public InlineSubqueryRole(object parentContext, PathedRole parentEntryRole, Role role)
			{
				myParentContext = parentContext;
				myEntryPathedRole = parentEntryRole;
				myRole = role;
			}
			#endregion // Constructor
			#region Accessor Properties
			/// <summary>
			/// Retrieve the parent context for this invocation of a subquery.
			/// </summary>
			public object ParentContext
			{
				get
				{
					return myParentContext;
				}
			}
			/// <summary>
			/// Retrieve the entry role in the parent context used to invoke this subquery.
			/// </summary>
			public PathedRole ParentEntryRole
			{
				get
				{
					return myEntryPathedRole;
				}
			}
			/// <summary>
			/// Retrieve the role being used.
			/// </summary>
			public Role Role
			{
				get
				{
					return myRole;
				}
			}
			#endregion // Accessor Properties
			#region Equality overrides
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				InlineSubqueryRole typedObj;
				return (null != (typedObj = obj as InlineSubqueryRole)) && this.Equals(typedObj);
			}
			/// <summary>
			/// Standard GetHashCode override
			/// </summary>
			public override int GetHashCode()
			{
				object context = myParentContext;
				return context != null ? Utility.GetCombinedHashCode(myEntryPathedRole.GetHashCode(), myRole.GetHashCode(), context.GetHashCode()) : Utility.GetCombinedHashCode(myEntryPathedRole.GetHashCode(), myRole.GetHashCode());
			}
			/// <summary>
			/// Typed Equals method
			/// </summary>
			public bool Equals(InlineSubqueryRole other)
			{
				object context = myParentContext;
				return other != null &&
					((context == null) ? other.myParentContext == null : context.Equals(other.myParentContext)) && // Context objects may be recreated, use Equals instead of ==
					myEntryPathedRole == other.myEntryPathedRole &&
					myRole == other.myRole;
			}
			/// <summary>
			/// Equality operator
			/// </summary>
			public static bool operator ==(InlineSubqueryRole left, InlineSubqueryRole right)
			{
				return ((object)left != null) ? left.Equals(right) : (object)right == null;
			}
			/// <summary>
			/// Inequality operator
			/// </summary>
			public static bool operator !=(InlineSubqueryRole left, InlineSubqueryRole right)
			{
				return ((object)left != null) ? !left.Equals(right) : (object)right != null;
			}
			#endregion // Equality overrides
		}
		#endregion // InlineSubqueryRole class
		#region InlineSubqueryParameter class
		/// <summary>
		/// A class representing a parameter in an inline expansion of a subquery. A single
		/// subquery can be used multiple times (directly or nested) within a
		/// single derivation path, so the nodes in the query derivation path
		/// are insufficient to uniquely identify the inlined elements within
		/// the outer structure. A subquery parameter provides a key for a single parameter in
		/// a subquery use by representing the parent context, the use of a subquery in
		/// that context (identified by the pathed role used to enter the query), and the
		/// parameter in the subquery.
		/// </summary>
		/// <remarks>See remarks on <see cref="InlineSubqueryContext"/>.</remarks>
		private sealed class InlineSubqueryParameter
		{
			#region Member variables
			private readonly object myParentContext;
			private readonly PathedRole myEntryPathedRole;
			private readonly QueryParameter myParameter;
			#endregion // Member variables
			#region Constructor
			/// <summary>
			/// Create a new <see cref="InlineSubqueryParameter"/>
			/// </summary>
			/// <param name="parentContext">The context representing the derivation path that invokes this subquery.</param>
			/// <param name="parentEntryRole">The <see cref="PathedRole"/> in the context path used to invoke an instance of this subquery.</param>
			/// <param name="parameter">The <see cref="QueryParameter"/> in the subquery definition.</param>
			public InlineSubqueryParameter(object parentContext, PathedRole parentEntryRole, QueryParameter parameter)
			{
				myParentContext = parentContext;
				myEntryPathedRole = parentEntryRole;
				myParameter = parameter;
			}
			#endregion // Constructor
			#region Accessor Properties
			/// <summary>
			/// Retrieve the parent context for this invocation of a subquery.
			/// </summary>
			public object ParentContext
			{
				get
				{
					return myParentContext;
				}
			}
			/// <summary>
			/// Retrieve the entry role in the parent context used to invoke this subquery.
			/// </summary>
			public PathedRole ParentEntryRole
			{
				get
				{
					return myEntryPathedRole;
				}
			}
			/// <summary>
			/// Retrieve the parameter being used.
			/// </summary>
			public QueryParameter Parameter
			{
				get
				{
					return myParameter;
				}
			}
			#endregion // Accessor Properties
			#region Equality overrides
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				InlineSubqueryParameter typedObj;
				return (null != (typedObj = obj as InlineSubqueryParameter)) && this.Equals(typedObj);
			}
			/// <summary>
			/// Standard GetHashCode override
			/// </summary>
			public override int GetHashCode()
			{
				object context = myParentContext;
				return context != null ? Utility.GetCombinedHashCode(myEntryPathedRole.GetHashCode(), myParameter.GetHashCode(), context.GetHashCode()) : Utility.GetCombinedHashCode(myEntryPathedRole.GetHashCode(), myParameter.GetHashCode());
			}
			/// <summary>
			/// Typed Equals method
			/// </summary>
			public bool Equals(InlineSubqueryParameter other)
			{
				object context = myParentContext;
				return other != null &&
					((context == null) ? other.myParentContext == null : context.Equals(other.myParentContext)) && // Context objects may be recreated, use Equals instead of ==
					myEntryPathedRole == other.myEntryPathedRole &&
					myParameter == other.myParameter;
			}
			/// <summary>
			/// Equality operator
			/// </summary>
			public static bool operator ==(InlineSubqueryParameter left, InlineSubqueryParameter right)
			{
				return ((object)left != null) ? left.Equals(right) : (object)right == null;
			}
			/// <summary>
			/// Inequality operator
			/// </summary>
			public static bool operator !=(InlineSubqueryParameter left, InlineSubqueryParameter right)
			{
				return ((object)left != null) ? !left.Equals(right) : (object)right != null;
			}
			#endregion // Equality overrides
		}
		#endregion // InlineSubqueryParameter class
		#region UnifierWithContext struct
		/// <summary>
		/// A structure representing a <see cref="PathObjectUnifier"/> combined with a
		/// path context.
		/// </summary>
		private struct ContextBoundUnifier
		{
			#region Constructor and Fields
			private readonly PathObjectUnifier myUnifier;
			private readonly object myContext;
			/// <summary>
			/// Create a <see cref="ContextBoundUnifier"/>
			/// </summary>
			public ContextBoundUnifier(PathObjectUnifier unifier, object context)
			{
				myUnifier = unifier;
				myContext = context;
			}
			#endregion // Constructor and Fields
			#region Accessor Properties
			/// <summary>
			/// Get the <see cref="PathObjectUnifier"/> associated with this node.
			/// </summary>
			public PathObjectUnifier Unifier
			{
				get
				{
					return myUnifier;
				}
			}
			/// <summary>
			/// Retrieve the context object specified with the constructor.
			/// </summary>
			public object Context
			{
				get
				{
					return myContext;
				}
			}
			#endregion // Accessor Properties
			#region Equality and casting routines
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				if (obj is ContextBoundUnifier)
				{
					return ((ContextBoundUnifier)obj).Equals(this);
				}
				return obj == myUnifier && myContext == null;
			}
			/// <summary>
			/// Standard GetHashCode override
			/// </summary>
			public override int GetHashCode()
			{
				object obj = myUnifier;
				if (obj != null)
				{
					object context = myContext;
					return (context == null) ? obj.GetHashCode() : Utility.GetCombinedHashCode(obj.GetHashCode(), context.GetHashCode());
				}
				return 0;
			}
			/// <summary>
			/// Typed Equals method
			/// </summary>
			public bool Equals(ContextBoundUnifier other)
			{
				object context = myContext;
				return myUnifier == other.myUnifier &&
					((context == null) ? other.myContext == null : context.Equals(other.myContext));
			}
			/// <summary>
			/// Equality operator
			/// </summary>
			public static bool operator ==(ContextBoundUnifier left, ContextBoundUnifier right)
			{
				return left.Equals(right);
			}
			/// <summary>
			/// Inequality operator
			/// </summary>
			public static bool operator !=(ContextBoundUnifier left, ContextBoundUnifier right)
			{
				return !left.Equals(right);
			}
			/// <summary>
			/// Automatically cast this structure to an <see cref="PathObjectUnifier"/>
			/// </summary>
			public static implicit operator PathObjectUnifier(ContextBoundUnifier boundUnifier)
			{
				return boundUnifier.myUnifier;
			}
			#endregion // Equality and casting routines
		}
		#endregion // UnifierWithContext struct
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
			/// <summary>
			/// An explicit existence statement. This node is always
			/// rendered, unlike required context variables, which
			/// are used as a backup.
			/// </summary>
			VariableExistence,
			/// <summary>
			/// A list of 2 or more variables that should be listed
			/// as equal in the current context. 
			/// </summary>
			VariableEquivalence,
		}
		#endregion // VerbalizationPlanNodeType enum
		#region IVariableEquivalenceQuantified interface
		/// <summary>
		/// Implement on the variable list associated with a VariableEquivalence node
		/// to quantify the listed variables.
		/// </summary>
		private interface IVariableEquivalenceQuantified
		{
		}
		#endregion /// IVariableEquivalenceQuantified interface
		#region IVariableEquivalenceIdentifiedBy interface
		/// <summary>
		/// Implement on the variable list associated with a VariableEquivalence node
		/// to use an 'is identified by' snippet instead of an equality list. Applies
		/// to variable lists with exactly two nodes and the first identified by the
		/// second.
		/// </summary>
		private interface IVariableEquivalenceIdentifiedBy : IVariableEquivalenceQuantified
		{
		}
		#endregion // IVariableEquivalenceIdentifiedBy interface
		#region VariableEquivalenceIdentifiedByImpl class
		/// <summary>
		/// A helper class to implement <see cref="IVariableEquivalenceIdentifiedBy"/>
		/// </summary>
		private class VariableEquivalenceIdentifiedByImpl : List<object>, IVariableEquivalenceIdentifiedBy
		{
			/// <summary>
			/// Create a new <see cref="VariableEquivalenceIdentifiedByImpl"/> instance with identified
			/// and identifying keys.
			/// </summary>
			public VariableEquivalenceIdentifiedByImpl(object identifiedKey, object identifiedByKey)
			{
				this.Add(identifiedKey);
				this.Add(identifiedByKey);
			}
		}
		#endregion // VariableEquivalenceIdentifiedByImpl class
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
			/// A reading has a basic lead role, which is a role with no hyphen
			/// binding or front text associated with it. This state will always
			/// be set along with the <see cref="FullyCollapseFirstRole"/> and
			/// <see cref="BackReferenceFirstRole"/> settings, but can also hold
			/// when these conditions do not. Set during the reading resolution phase.
			/// </summary>
			BasicLeadRole = 1,
			/// <summary>
			/// Completely eliminate the first role during role replacement.
			/// Used during branch conditions when a reading is found that
			/// matches the lead role player for the lead fact type in the
			/// previous branch. Set during the reading resolution phase.
			/// </summary>
			FullyCollapseFirstRole = 2,
			/// <summary>
			/// Use a personal or impersonal back referencing pronoun in place
			/// of the first role. Used for a joined fact type where the previous
			/// reading ends with the role player name. Set during the reading
			/// resolution phase.
			/// </summary>
			BackReferenceFirstRole = 4,
			/// <summary>
			/// Use a negated existential quantifier for the non-entry role.
			/// Set during path execution if the first fact type in a negated
			/// chain is a binary fact type with an opposite role player with
			/// an associated variable that has not been used.
			/// </summary>
			NegatedExitRole = 8,
			/// <summary>
			/// Set during fact type analysis to determine if it is possible
			/// that conditions might be met while the path is being verbalized
			/// to apply the NegatedExitRole option. If the opposite negated
			/// role is fully existential, then the first flag can be set
			/// when the readings are bound.
			/// </summary>
			DynamicNegatedExitRole = 0x10,
			/// <summary>
			/// Set during rendering so that we do not repeat analysis of the
			/// negated exit role status.
			/// </summary>
			DynamicNegatedExitRoleEvaluated = 0x20,
			/// <summary>
			/// Ignore the <see cref="FullyCollapseFirstRole"/> setting.
			/// Added dynamically during rendering if required variables
			/// are listed before the fact statement, thereby changing,
			/// or if dynamic negation inlining is blocked and the negation
			/// snippet does not support back referencing.
			/// </summary>
			BlockFullyCollapseFirstRole = 0x40,
			/// <summary>
			/// Ignore the <see cref="BackReferenceFirstRole"/> setting.
			/// Added dynamically during rendering if required variables
			/// are listed before the fact statement, thereby changing,
			/// or if dynamic negation inlining is blocked and the negation
			/// snippet does not support back referencing.
			/// </summary>
			BlockBackReferenceFirstRole = 0x80,
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
			private object myPathContext;
			private VerbalizationPlanNode(object pathContext, VerbalizationPlanNode parentNode)
			{
				myPathContext = pathContext;
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
			/// <param name="factType">The <see cref="PathedRole"/> that is the first in the <paramref name="factType"/>.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddFactTypeEntryNode(FactType factType, PathedRole factTypeEntry, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new FactTypeNode(pathContext, parentNode, factType, factTypeEntry);
			}
			/// <summary>
			/// Create and attach a new branching node.
			/// </summary>
			/// <param name="branchType">The <see cref="VerbalizationPlanBranchType"/> of the new branch.</param>
			/// <param name="renderingStyle">The <see cref="VerbalizationPlanBranchRenderingStyle"/> of the new branch.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddBranchNode(VerbalizationPlanBranchType branchType, VerbalizationPlanBranchRenderingStyle renderingStyle, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				VerbalizationPlanNode newNode = new BranchNode(pathContext, parentNode, branchType, renderingStyle);
				if (parentNode == null)
				{
					rootNode = newNode;
				}
				return newNode;
			}
			/// <summary>
			/// Create and attach a new value constraint node.
			/// </summary>
			/// <param name="valueConstraint">The <see cref="PathConditionRoleValueConstraint"/> or <see cref="PathConditionRootValueConstraint"/> to add.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddValueConstraintNode(ValueConstraint valueConstraint, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new ValueConstraintNode(pathContext, parentNode, valueConstraint);
			}
			/// <summary>
			/// Create and attach a new calculated condition node.
			/// </summary>
			/// <param name="calculatedCondition">The <see cref="CalculatedPathValue"/> to add.</param>
			/// <param name="restrictsSingleFactType">Is this condition node being added immediately after a  fact
			/// type instance where all pathed role inputs to the function are contained in that instance?</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddCalculatedConditionNode(CalculatedPathValue calculatedCondition, bool restrictsSingleFactType, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new CalculatedConditionNode(pathContext, parentNode, calculatedCondition, restrictsSingleFactType);
			}
			/// <summary>
			/// Create and attach a new projection calculation node.
			/// </summary>
			/// <param name="headVariableKey">The projection key.</param>
			/// <param name="calculation">The projected <see cref="CalculatedPathValue"/> to add.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddProjectedCalculationNode(object headVariableKey, CalculatedPathValue calculation, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new ProjectedCalculationNode(pathContext, parentNode, headVariableKey, calculation);
			}
			/// <summary>
			/// Create and attach a new projection calculation node.
			/// </summary>
			/// <param name="headVariableKey">The projection key.</param>
			/// <param name="constant">The projected <see cref="PathConstant"/> to add.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddProjectedConstantNode(object headVariableKey, PathConstant constant, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new ProjectedConstantNode(pathContext, parentNode, headVariableKey, constant);
			}
			/// <summary>
			/// Create and attach a new variable existence node. Variable existence is always
			/// created with positive semantics, but may be changed later if a negated chain
			/// begins with a positive existence node.
			/// </summary>
			/// <param name="requiredVariableKey">The key for the variable to declare.</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddVariableExistenceNode(object requiredVariableKey, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new VariableExistenceNode(pathContext, parentNode, requiredVariableKey);
			}
			/// <summary>
			/// Create and attach a new variable equality node.
			/// </summary>
			/// <param name="equivalentVariableKeys">A list of keys to render as equivalent</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddVariableEquivalenceNode(IList<object> equivalentVariableKeys, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new VariableEquivalenceNode(pathContext, parentNode, equivalentVariableKeys);
			}
			/// <summary>
			/// Create and attach a new chained root variable node.
			/// </summary>
			/// <param name="rootVariable">The root variable for a path</param>
			/// <param name="pathContext">The path context for this node.</param>
			/// <param name="parentNode">The parent <see cref="VerbalizationPlanNode"/> for the new node.</param>
			/// <param name="rootNode">A reference to the root node of the chain.</param>
			/// <returns>New <see cref="VerbalizationPlanNode"/></returns>
			public static VerbalizationPlanNode AddChainedRootVariableNode(RolePlayerVariable rootVariable, object pathContext, VerbalizationPlanNode parentNode, ref VerbalizationPlanNode rootNode)
			{
				if (parentNode == null)
				{
					parentNode = AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref rootNode);
				}
				return new ChainedRootVariableNode(pathContext, parentNode, rootVariable);
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
					newNode = new FloatingRootVariableNode(childNode.PathContext, null, floatingRootVariable, childNode);
					childNode.myParentNode = newNode;
					newNode.myParentNode = parentNode;
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
			/// The context where this node is used.
			/// </summary>
			public object PathContext
			{
				get
				{
					return myPathContext;
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
			/// The variable used as a back reference with a type change
			/// for a fact type node. Provides the context variable that
			/// is being referenced. Set only if a back reference implies
			/// a type correlation.
			/// </summary>
			public virtual RolePlayerVariable CorrelateWithBackReferencedVariable
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
			/// Get the <see cref="PathConditionRoleValueConstraint"/> or <see cref="PathConditionRootValueConstraint"/> for
			/// a value constraint node.
			/// </summary>
			public virtual ValueConstraint ValueConstraint
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
			/// <summary>
			/// Get the head node of variable keys that are required to render this node.
			/// </summary>
			public virtual LinkedNode<object> RequiredContextVariableUseKeys
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Add a context variable key. The associated variable needs to be
			/// declared before this node is stated.
			/// </summary>
			/// <param name="contextVariableKeys">A list of context variable nodes.</param>
			public virtual void RequireContextVariables(LinkedNode<object> contextVariableKeys)
			{
				Debug.Fail("RequireContextVariables not implemented for this node type");
			}
			/// <summary>
			/// Set to true by the rendering engine if a required context variable was
			/// rendered.
			/// </summary>
			public virtual bool RenderedRequiredContextVariable
			{
				get
				{
					return false;
				}
				set
				{
				}
			}
			public virtual IList<object> EquivalentVariableKeys
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Set to use a negated existential on a VariableExistence node
			/// </summary>
			public virtual bool NegateExistence
			{
				get
				{
					return false;
				}
				set
				{
				}
			}
			/// <summary>
			/// The variable key used with a VariableExistence node
			/// </summary>
			public virtual object VariableKey
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
			#region Helper Intermediate Types
			/// <summary>
			/// Implementation of <see cref="RequiredContextVariableUseKeys"/> and <see cref="RequireContextVariables"/>.
			/// Does not implement <see cref="RenderedRequiredContextVariable"/>
			/// </summary>
			private abstract class RequireContextVariableNode : VerbalizationPlanNode
			{
				private LinkedNode<object> myRequiredContextVariableUseKeys;
				protected RequireContextVariableNode(object pathContext, VerbalizationPlanNode parentNode)
					: base(pathContext, parentNode)
				{
				}
				public override void RequireContextVariables(LinkedNode<object> contextVariableUseKeys)
				{
					LinkedNode<object> existingVarKeys = myRequiredContextVariableUseKeys;
					if (existingVarKeys == null)
					{
						myRequiredContextVariableUseKeys = contextVariableUseKeys;
					}
					else
					{
						existingVarKeys.GetTail().SetNext(contextVariableUseKeys, ref existingVarKeys);
					}
				}
				public override LinkedNode<object> RequiredContextVariableUseKeys
				{
					get
					{
						return myRequiredContextVariableUseKeys;
					}
				}
				public abstract override bool RenderedRequiredContextVariable
				{
					get;
					set;
				}
			}
			#endregion // Helper Intermediate Types
			#region Node type specific types
			private sealed class FactTypeNode : RequireContextVariableNode
			{
				private readonly FactType myFactType;
				private readonly PathedRole myEntryPathedRole;
				private IReading myReading;
				private const int RenderedRequiredContextVariableBit = 0x10000;
				private int myOptions; // VerbalizationPlanReadingOptions and the RenderedRequiredContextVariableBit
				private RolePlayerVariable myBackReferencedVariable;
				public FactTypeNode(object pathContext, VerbalizationPlanNode parentNode, FactType factType, PathedRole factTypeEntryRole)
					: base(pathContext, parentNode)
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
				public override RolePlayerVariable CorrelateWithBackReferencedVariable
				{
					get
					{
						return myBackReferencedVariable;
					}
					set
					{
						myBackReferencedVariable = value;
					}
				}
				public override VerbalizationPlanReadingOptions ReadingOptions
				{
					get
					{
						return (VerbalizationPlanReadingOptions)(myOptions & ~RenderedRequiredContextVariableBit);
					}
					set
					{
						int options = myOptions;
						if (0 != (myOptions & RenderedRequiredContextVariableBit))
						{
							myOptions = (int)value | RenderedRequiredContextVariableBit;
						}
						else
						{
							myOptions = (int)value;
						}
					}
				}
				public override bool RenderedRequiredContextVariable
				{
					get
					{
						return 0 != (myOptions & RenderedRequiredContextVariableBit);
					}
					set
					{
						if (value)
						{
							myOptions |= RenderedRequiredContextVariableBit;
						}
						else
						{
							myOptions &= ~RenderedRequiredContextVariableBit;
						}
					}
				}
			}
			private sealed class BranchNode : RequireContextVariableNode
			{
				private const int BranchTypeMask = 0xffff;
				private const int IsolatedRenderingBit = 0x10000;
				private const int HeaderListRenderingBit = 0x20000;
				private const int RenderedRequiredContextVariablesBit = 0x40000;
				private int mySettings;
				private LinkedNode<VerbalizationPlanNode> myChildNodes;
				public BranchNode(object pathContext, VerbalizationPlanNode parentNode, VerbalizationPlanBranchType branchType, VerbalizationPlanBranchRenderingStyle renderingStyle)
					: base(pathContext, parentNode)
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
				public override bool RenderedRequiredContextVariable
				{
					get
					{
						return 0 != (mySettings & RenderedRequiredContextVariablesBit);
					}
					set
					{
						if (value)
						{
							mySettings |= RenderedRequiredContextVariablesBit;
						}
						else
						{
							mySettings &= ~RenderedRequiredContextVariablesBit;
						}
					}
				}
			}
			private sealed class ValueConstraintNode : VerbalizationPlanNode
			{
				private readonly ValueConstraint myValueConstraint;
				public ValueConstraintNode(object pathContext, VerbalizationPlanNode parentNode, ValueConstraint valueConstraint)
					: base(pathContext, parentNode)
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
						return myValueConstraint is PathConditionRoleValueConstraint;
					}
				}
				public override ValueConstraint ValueConstraint
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
				public CalculatedConditionNode(object pathContext, VerbalizationPlanNode parentNode, CalculatedPathValue condition, bool restrictsSingleFactType)
					: base(pathContext, parentNode)
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
				public ProjectedCalculationNode(object pathContext, VerbalizationPlanNode parentNode, object headVariableKey, CalculatedPathValue calculation)
					: base(pathContext, parentNode)
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
				public ProjectedConstantNode(object pathContext, VerbalizationPlanNode parentNode, object headVariableKey, PathConstant constant)
					: base(pathContext, parentNode)
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
				public FloatingRootVariableNode(object pathContext, VerbalizationPlanNode parentNode, RolePlayerVariable floatingRootVariable, VerbalizationPlanNode childNode)
					: base(pathContext, parentNode)
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
				private readonly RolePlayerVariable myRootVariable;
				public ChainedRootVariableNode(object pathContext, VerbalizationPlanNode parentNode, RolePlayerVariable rootVariable)
					: base(pathContext, parentNode)
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
			private sealed class VariableExistenceNode : RequireContextVariableNode
			{
				private object myExistentialVariableKey;
				private const int NegateExistenceBit = 0x1;
				private const int RenderedRequiredContextVariablesBit = 0x2;
				private int myFlags;
				public VariableExistenceNode(object pathContext, VerbalizationPlanNode parentNode, object existentialVariableKey)
					: base(pathContext, parentNode)
				{
					myExistentialVariableKey = existentialVariableKey;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get
					{
						return VerbalizationPlanNodeType.VariableExistence;
					}
				}
				public override object VariableKey
				{
					get
					{
						return myExistentialVariableKey;
					}
				}
				public override bool NegateExistence
				{
					get
					{
						return 0 != (myFlags & NegateExistenceBit);
					}
					set
					{
						if (value)
						{
							myFlags |= NegateExistenceBit;
						}
						else
						{
							myFlags &= ~NegateExistenceBit;
						}
					}
				}
				public override bool RenderedRequiredContextVariable
				{
					get
					{
						return 0 != (myFlags & RenderedRequiredContextVariablesBit);
					}
					set
					{
						if (value)
						{
							myFlags |= RenderedRequiredContextVariablesBit;
						}
						else
						{
							myFlags &= ~RenderedRequiredContextVariablesBit;
						}
					}
				}
			}
			private sealed class VariableEquivalenceNode : VerbalizationPlanNode
			{
				private readonly IList<object> myEquivalentVariableKeys;
				public VariableEquivalenceNode(object pathContext, VerbalizationPlanNode parentNode, IList<object> equivalentVariableKeys)
					: base(pathContext, parentNode)
				{
					myEquivalentVariableKeys = equivalentVariableKeys;
				}
				public override VerbalizationPlanNodeType NodeType
				{
					get
					{
						return VerbalizationPlanNodeType.VariableEquivalence;
					}
				}
				public override IList<object> EquivalentVariableKeys
				{
					get
					{
						return myEquivalentVariableKeys;
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
		/// A dictionary to track correlations between variable declarations. Custom variable correlation
		/// occurs externally to the core pathing system and cannot be correlated via a role path
		/// because a path may not exist, the variables can be correlated over a subquery, or the
		/// variables can be external and span multiple paths which can't be correlated with a path-specific
		/// correlation root. There is no natural head correlation for a list of custom correlations, so all
		/// elements in a list are a key that points to the complete list.
		/// </summary>
		private Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> myCustomCorrelatedVariables;
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
		/// Basic option for quantifying the first requested variable.
		/// </summary>
		private CoreSnippetIdentifier? myLeadVariableQuantifier;
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
		/// A bit per branch type to determine which branch type
		/// rendering allows collapsing lead roles. Initialized on demand.
		/// </summary>
		private int myCollapsibleLeadBranchingBits;
		/// <summary>
		/// A bit per branch type to determine which branch type
		/// rendering allows the list open snippet to be collapsed
		/// for a back reference. Initialized on demand.
		/// </summary>
		private int myCollapsibleListOpenForBackReferenceBranchingBits;
		/// <summary>
		/// Bits to track which snippets result in an outdent operation.
		/// Enables trailing outdent tracking so that text on the same
		/// line as the end of a complex path verbalization maintains
		/// the correct outdent level. Initialized on demand.
		/// </summary>
		private BitTracker myOutdentSnippetBits;
		/// <summary>
		/// Bits to track which trailing list snippets need to be placed
		/// before an active outdent position. Initialized on demand.
		/// </summary>
		private BitTracker myOutdentAwareSnippetBits;
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
			myCollapsibleListOpenForBackReferenceBranchingBits = -1;
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
		/// <summary>
		/// Get and set the <see cref="CoreSnippetIdentifier"/> used to quantify
		/// the lead variable in the path. This is cleared automatically after it is used
		/// to quantify a variable. The assumption is that this is set after <see cref="KeyedVariableLeadsVerbalization"/>
		/// has been called to verify that the lead variable verbalizes first, and before <see cref="RenderPathVerbalization"/>
		/// </summary>
		public CoreSnippetIdentifier? LeadVariableQuantifier
		{
			get
			{
				return myLeadVariableQuantifier;
			}
			set
			{
				myLeadVariableQuantifier = value;
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

			// Populate path variables for this owner and process all role paths in the root (null) path context
			LinkedNode<object> pendingRequiredVariableKeys = null;
			InitializeRolePaths(null, null, null, pathOwner, AddPathProjections(pathOwner), ref pendingRequiredVariableKeys);

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
			// sure a use phase is pushed so that we don't see quantified elements
			// as a side effect of initialization.
			BeginQuantificationUsePhase();
		}
		private delegate object PathContextCreator(LeadRolePath rolePath);
		private void InitializeRolePaths(object pathContext, PathContextCreator contextCreator, VariableKeyDecorator keyDecorator, RolePathOwner pathOwner, IDictionary<LeadRolePath, IList<IList<object>>> equivalentVariableKeysByPath, ref LinkedNode<object> pendingRequiredVariableKeys)
		{
			// Determine owned paths to verbalize
			// Verbalize in owned/shared order. The LeadRolePathCollection is unordered.
			ReadOnlyLinkedElementCollection<LeadRolePath> ownedRolePaths = pathOwner.OwnedLeadRolePathCollection;
			int ownedPathCount = ownedRolePaths.Count;
			ReadOnlyLinkedElementCollection<LeadRolePath> sharedRolePaths = pathOwner.SharedLeadRolePathCollection;
			int sharedPathCount = sharedRolePaths.Count;
			int rolePathCount = ownedPathCount + sharedPathCount;
			if (rolePathCount != 0)
			{
				LeadRolePath[] filteredPaths = new LeadRolePath[rolePathCount];
				int filteredPathCount = 0;
				foreach (LeadRolePath leadRolePath in ownedRolePaths)
				{
					if (VerbalizesPath(pathOwner, leadRolePath))
					{
						filteredPaths[filteredPathCount] = leadRolePath;
						++filteredPathCount;
					}
				}
				foreach (LeadRolePath leadRolePath in sharedRolePaths)
				{
					if (VerbalizesPath(pathOwner, leadRolePath))
					{
						filteredPaths[filteredPathCount] = leadRolePath;
						++filteredPathCount;
					}
				}
				if (filteredPathCount != 0)
				{
					object outerContext = pathContext;
					LeadRolePath filteredPath;
					if (filteredPathCount != 1)
					{
						// Note that the or split and required variables are given the current (outer) context.
						// New contexts are created for the individual role paths.
						PushSplit(outerContext, VerbalizationPlanBranchType.OrSplit, ref pendingRequiredVariableKeys);
					}
					for (int i = 0; i < filteredPathCount; ++i)
					{
						filteredPath = filteredPaths[i];
						if (contextCreator != null)
						{
							pathContext = contextCreator(filteredPath);
						}
						IList<IList<object>> equivalentVariables = null;
						if (equivalentVariableKeysByPath != null)
						{
							equivalentVariableKeysByPath.TryGetValue(filteredPath, out equivalentVariables);
						}
						if (equivalentVariables != null)
						{
							PushSplit(outerContext, VerbalizationPlanBranchType.AndSplit, ref pendingRequiredVariableKeys);
						}
						InitializeRolePath(pathContext, pathOwner, filteredPath, keyDecorator, ref pendingRequiredVariableKeys);
						if (equivalentVariables != null)
						{
							int equivalentVariableSetCount = equivalentVariables.Count;
							for (int j = 0; j < equivalentVariableSetCount; ++j)
							{
								VerbalizationPlanNode.AddVariableEquivalenceNode(equivalentVariables[j], outerContext, myCurrentBranchNode, ref myRootPlanNode);
							}
							PopSplit(VerbalizationPlanBranchType.AndSplit);
						}
					}
					if (filteredPathCount != 1)
					{
						PopSplit(VerbalizationPlanBranchType.OrSplit);
					}
				}
			}
		}
		private void InitializeRolePath(object initialPathContext, RolePathOwner pathOwner, LeadRolePath leadRolePath, VariableKeyDecorator keyDecorator, ref LinkedNode<object> requiredVariableKeys)
		{
			LinkedElementCollection<CalculatedPathValue> pathConditions = leadRolePath.CalculatedConditionCollection;
			int pathConditionCount = pathConditions.Count;
			BitTracker processedPathConditions = new BitTracker(pathConditionCount);
			Stack<LinkedElementCollection<RoleBase>> factTypeRolesStack = new Stack<LinkedElementCollection<RoleBase>>();
			Stack<LinkedElementCollection<QueryParameter>> queryParametersStack = null;
			BitTracker roleUseTracker = new BitTracker(0);
			int roleUseBaseIndex = -1;
			int resolvedRoleIndex;
			RolePathObjectTypeRoot pathRoot = leadRolePath.PathRoot;
			RolePlayerVariable rootObjectTypeVariable = null;
			RolePathNode pathNode;
			if (pathRoot != null)
			{
				// UNDONE: IntraPathRoot All root variables should be treated similarly with a variable introduction
				// before they are used (if needed). The VerbalizeRootObjectType setting is used for subtypes
				// only. See if we can integrate this notion with a normal variable introduction node and handle
				// this inline in VisitRolePathParts.
				pathNode = new RolePathNode(pathRoot, initialPathContext);
				rootObjectTypeVariable = RegisterRolePlayerUse(pathRoot.RootObjectType, null, pathNode, pathNode);
			}
			if (initialPathContext == null)
			{
				++myLatestUsePhase;
			}
			ReadOnlyCollection<PathedRole> pendingPathedRoles = null;
			int pendingPathedRoleIndex = -1;
			PathedRole pendingForSameFactType = null;
			InlineSubqueryRole pendingForSameFactTypeQueryRoleKey = null;
			LinkedNode<object> pendingRequiredVariableKeys = requiredVariableKeys;
			// A list (acting like a stack) to get the full history of the parent pathed roles.
			List<ReadOnlyCollection<PathedRole>> contextPathedRoles = null;
			List<RolePathObjectTypeRoot> contextPathRoots = null;
			List<InlineSubqueryRole> inlineSubqueryRoles = null;
			int inlineSubqueryRoleBaseIndex = 0;
			List<InlineSubqueryParameter> inlineSubqueryParameters = null;
			int inlineSubqueryParameterBaseIndex = 0;
			VerbalizationPlanNode startingBranchNode = myCurrentBranchNode;
			LinkedNode<VerbalizationPlanNode> startingBranchTailLink = startingBranchNode != null ? startingBranchNode.FirstChildNode : null;
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			if (startingBranchTailLink != null)
			{
				startingBranchTailLink = startingBranchTailLink.GetTail();
			}
			PushConditionalChainNode(initialPathContext);
			if (VerbalizeRootObjectType && rootObjectTypeVariable != null)
			{
				myCurrentBranchNode = VerbalizationPlanNode.AddChainedRootVariableNode(rootObjectTypeVariable, initialPathContext, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
				rootObjectTypeVariable = null; // Make sure this doesn't get used as a floating root node below
			}
			VisitRolePathParts(
				initialPathContext,
				leadRolePath,
				RolePathNode.Empty,
				delegate(object pathContext, PathedRole currentPathedRole, RolePathObjectTypeRoot currentPathRoot, RolePath currentPath, ReadOnlyCollection<PathedRole> currentPathedRoles, int currentPathedRoleIndex, RolePathNode contextPathNode, bool unwinding)
				{
					if (currentPathedRole != null)
					{
						PathedRolePurpose purpose = currentPathedRole.PathedRolePurpose;
						bool sameFactType = purpose == PathedRolePurpose.SameFactType;
						bool isSubqueryRole = currentPathedRole.Role.FactType is Subquery;
						Role currentRole;
						if (unwinding)
						{
							if (pendingForSameFactType != null)
							{
								Debug.Assert(pendingForSameFactType == currentPathedRole);
								// We reached a leaf same fact type node. Treat it as fully existential
								// if it is not used for any other purpose.
								pendingForSameFactType = null;
								pendingPathedRoles = null;
								currentRole = currentPathedRole.Role;
								if (pendingForSameFactTypeQueryRoleKey != null ||
									IsPathedRoleReferencedOutsidePath(currentPathedRole))
								{
									pathNode = new RolePathNode(currentPathedRole, pathContext);
									RegisterRolePlayerUse(currentRole.RolePlayer, null, pathNode, pathNode);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
									if (pendingForSameFactTypeQueryRoleKey != null)
									{
										CustomCorrelateVariables(useMap[pathNode].PrimaryRolePlayerVariable, useMap[pendingForSameFactTypeQueryRoleKey].PrimaryRolePlayerVariable);
										pendingForSameFactTypeQueryRoleKey = null;
									}
								}
							}
							if (!sameFactType || contextPathNode.PathedRole == null)
							{
								// Unwind the stack
								if (isSubqueryRole)
								{
									int popCount = factTypeRolesStack.Peek().Count;
									inlineSubqueryRoles.RemoveRange(inlineSubqueryRoleBaseIndex, popCount);
									inlineSubqueryRoleBaseIndex = inlineSubqueryRoles.Count;
									LinkedElementCollection<QueryParameter> popParameters = queryParametersStack.Pop();
									if (popParameters != null)
									{
										popCount = popParameters.Count;
										inlineSubqueryParameters.RemoveRange(inlineSubqueryParameterBaseIndex, popCount);
										inlineSubqueryParameterBaseIndex = inlineSubqueryParameters.Count;
									}
								}
								roleUseBaseIndex = PopFactType(factTypeRolesStack, !isSubqueryRole, ref roleUseTracker);
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
								PushConditionalChainNode(pathContext);
							}
							if (sameFactType)
							{
								if (contextPathNode.PathedRole == null)
								{
									// Error condition, missing an entry role, push the fact type
									// so the stack does not get out of balance.
									currentRole = currentPathedRole.Role;
									if (isSubqueryRole)
									{
										PushSubquery(pathContext, leadRolePath, currentRole.FactType, currentPathedRole, currentPathedRoles, currentPathedRoleIndex, factTypeRolesStack, ref roleUseTracker, ref roleUseBaseIndex, pathConditions, ref processedPathConditions, ref queryParametersStack, ref inlineSubqueryRoles, ref inlineSubqueryRoleBaseIndex, ref inlineSubqueryParameters, ref inlineSubqueryParameterBaseIndex, ref pendingRequiredVariableKeys);
									}
									else
									{
										roleUseBaseIndex = PushFactType(pathContext, currentRole.FactType, currentPathedRole, currentPathedRoles, currentPathedRoleIndex, factTypeRolesStack, ref roleUseTracker, pathConditions, ref processedPathConditions, ref pendingRequiredVariableKeys);
										resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
										if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
										{
											roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
										}
									}
								}
								else if (currentPathedRoleIndex == 0)
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
										PathConditionRoleValueConstraint valueConstraint = processPathedRole.ValueConstraint;
										if (valueConstraint != null)
										{
											VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, pathContext, contextChainNode, ref myRootPlanNode);
										}
									}

									// Look for functions that use roles restricted to those roles used
									// up to this point in the context fact type entry and this section.
									if (pathConditionCount != 0)
									{
										int contextSectionsCount = contextPathedRoles != null ? contextPathedRoles.Count : 0;
										int contextRootsCount = contextPathRoots != null ? contextPathRoots.Count : 0;
										for (int i = 0; i < pathConditionCount; ++i)
										{
											if (!processedPathConditions[i])
											{
												CalculatedPathValue calculation = pathConditions[i];
												bool? isLocal = IsLocalCalculatedValue(
													pathContext,
													calculation,
													delegate(RolePathNode testPathNode)
													{
														PathedRole testPathedRole;
														RolePathObjectTypeRoot testPathRoot;
														if (null != (testPathedRole = testPathNode))
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
														}
														else if (null != (testPathRoot = testPathNode))
														{
															// Check the current node
															if (testPathRoot == currentPathRoot)
															{
																return true;
															}
															// Look at the context roots
															for (int j = contextRootsCount - 1; j >= 0; --j)
															{
																if (testPathRoot == contextPathRoots[j])
																{
																	return true;
																}
															}
														}
														return false;
													});
												if (isLocal.GetValueOrDefault(false))
												{
													// Although this is a single fact type, it occurs as part of a split
													// after the fact type has been defined, so the fact type is not immediately
													// before this one and we can't set the restrictsSingleFactType parameter to true.
													VerbalizationPlanNode.AddCalculatedConditionNode(calculation, false, pathContext, contextChainNode, ref myRootPlanNode);
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
									if (pendingForSameFactTypeQueryRoleKey != null ||
										IsPathedRoleReferencedOutsidePath(pendingForSameFactType))
									{
										currentRole = pendingForSameFactType.Role;
										pathNode = new RolePathNode(pendingForSameFactType, pathContext);
										RegisterRolePlayerUse(currentRole.RolePlayer, null, pathNode, pathNode);
										resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
										if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
										{
											roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
										}
										if (pendingForSameFactTypeQueryRoleKey != null)
										{
											CustomCorrelateVariables(useMap[pathNode].PrimaryRolePlayerVariable, useMap[pendingForSameFactTypeQueryRoleKey].PrimaryRolePlayerVariable);
										}
									}
								}
								pendingForSameFactType = currentPathedRole;
								pendingPathedRoles = currentPathedRoles;
								pendingPathedRoleIndex = currentPathedRoleIndex;
								pendingForSameFactTypeQueryRoleKey = isSubqueryRole ? inlineSubqueryRoles[inlineSubqueryRoleBaseIndex + ResolveRoleIndex(factTypeRolesStack.Peek(), currentPathedRole.Role)] : null;
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
									pathNode = new RolePathNode(pendingForSameFactType, pathContext);
									RegisterRolePlayerUse(currentRole.RolePlayer, null, pathNode, pathNode);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
									if (pendingForSameFactTypeQueryRoleKey != null)
									{
										CustomCorrelateVariables(useMap[pathNode].PrimaryRolePlayerVariable, useMap[pendingForSameFactTypeQueryRoleKey].PrimaryRolePlayerVariable);
										pendingForSameFactTypeQueryRoleKey = null;
									}
									pendingForSameFactType = null;
									pendingPathedRoles = null;
								}
								RegisterFactTypeEntryRolePlayerUse(pathContext, currentPathedRole, contextPathNode);
								if (currentPathedRole.IsNegated)
								{
									PushNegatedChainNode(pathContext, currentPathedRole, ref pendingRequiredVariableKeys);
								}
								FactType factType = ResolvePathedEntryRoleFactType(currentPathedRole, currentPath, currentPathedRoles, currentPathedRoleIndex);
								if (isSubqueryRole)
								{
									PushSubquery(pathContext, leadRolePath, factType, currentPathedRole, currentPathedRoles, currentPathedRoleIndex, factTypeRolesStack, ref roleUseTracker, ref roleUseBaseIndex, pathConditions, ref processedPathConditions, ref queryParametersStack, ref inlineSubqueryRoles, ref inlineSubqueryRoleBaseIndex, ref inlineSubqueryParameters, ref inlineSubqueryParameterBaseIndex, ref pendingRequiredVariableKeys);
								}
								else
								{
									roleUseBaseIndex = PushFactType(pathContext, factType, currentPathedRole, currentPathedRoles, currentPathedRoleIndex, factTypeRolesStack, ref roleUseTracker, pathConditions, ref processedPathConditions, ref pendingRequiredVariableKeys);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentPathedRole.Role);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
								}
							}
						}
					}
					else if (currentPathRoot != null)
					{
						if (unwinding)
						{
							contextPathRoots.RemoveAt(contextPathRoots.Count - 1);
							while (pendingRequiredVariableKeys != null)
							{
								myCurrentBranchNode = VerbalizationPlanNode.AddVariableExistenceNode(pendingRequiredVariableKeys.Value, pathContext, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
								pendingRequiredVariableKeys = pendingRequiredVariableKeys.Next;
							}
							if (currentPathRoot.IsNegated)
							{
								PopNegatedChainNode();
							}
						}
						else
						{
							(contextPathRoots ?? (contextPathRoots = new List<RolePathObjectTypeRoot>())).Add(currentPathRoot);
							if (pendingForSameFactType != null)
							{
								// We reached a leaf same fact type node. Treat it as fully existential
								// if it is not used for any other purpose.
								Role currentRole = pendingForSameFactType.Role;
								if (pendingForSameFactTypeQueryRoleKey != null ||
									IsPathedRoleReferencedOutsidePath(pendingForSameFactType))
								{
									pathNode = new RolePathNode(pendingForSameFactType, pathContext);
									RegisterRolePlayerUse(currentRole.RolePlayer, null, pathNode, pathNode);
									resolvedRoleIndex = ResolveRoleIndex(factTypeRolesStack.Peek(), currentRole);
									if (resolvedRoleIndex != -1) // Defensive, guard against bogus path
									{
										roleUseTracker[roleUseBaseIndex + resolvedRoleIndex] = true;
									}
									if (pendingForSameFactTypeQueryRoleKey != null)
									{
										CustomCorrelateVariables(
											useMap[pathNode].PrimaryRolePlayerVariable,
											useMap[pendingForSameFactTypeQueryRoleKey].PrimaryRolePlayerVariable);
										pendingForSameFactTypeQueryRoleKey = null;
									}
								}
								pendingForSameFactType = null;
								pendingPathedRoles = null;
							}
							if (currentPath != leadRolePath) // Handled earlier, see notes above
							{
								pathNode = new RolePathNode(currentPathRoot, pathContext);
								RegisterRolePlayerUse(currentPathRoot.RootObjectType, null, pathNode, pathNode);
							}

							if (currentPathRoot.IsNegated)
							{
								// We need existence nodes for previous nodes instead of required context variables,
								// which will end up collapsing if the variables are in the head, resulting in an
								// incomplete rule body with no existence assertion.
								while (pendingRequiredVariableKeys != null)
								{
									myCurrentBranchNode = VerbalizationPlanNode.AddVariableExistenceNode(pendingRequiredVariableKeys.Value, pathContext, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
									pendingRequiredVariableKeys = pendingRequiredVariableKeys.Next;
								}
								PushNegatedChainNode(pathContext, null, ref pendingRequiredVariableKeys);
							}
							ValueConstraint valueConstraint;
							if (null != (valueConstraint = currentPathRoot.ValueConstraint))
							{
								myCurrentBranchNode = VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, pathContext, myCurrentBranchNode, ref myRootPlanNode).ParentNode;
							}
							else
							{
								if (pendingRequiredVariableKeys == null)
								{
									pendingRequiredVariableKeys = new LinkedNode<object>(new RolePathNode(currentPathRoot, pathContext));
								}
								else
								{
									pendingRequiredVariableKeys.GetTail().SetNext(new LinkedNode<object>(new RolePathNode(currentPathRoot, pathContext)), ref pendingRequiredVariableKeys);
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
							PopSplit(GetBranchType(currentPath));
						}
						else
						{
							(contextPathedRoles ?? (contextPathedRoles = new List<ReadOnlyCollection<PathedRole>>())).Add(currentPathedRoles);
							PushSplit(pathContext, GetBranchType(currentPath), ref pendingRequiredVariableKeys);
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
								contextChainNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, initialPathContext, contextChainNode, ref myRootPlanNode);
							}
						}
						VerbalizationPlanNode.AddCalculatedConditionNode(pathConditions[i], false, initialPathContext, contextChainNode, ref myRootPlanNode);
					}
				}
			}
			AddCalculatedAndConstantProjections(initialPathContext, pathOwner, leadRolePath, keyDecorator);
			PopConditionalChainNode();
			if (rootObjectTypeVariable != null &&
				!rootObjectTypeVariable.HasBeenUsed(myLatestUsePhase, false) &&
				myCurrentBranchNode != null)
			{
				// UNDONE: IntraPathRoot The notion of floating root variables needs to be available for intra-path roots
				// as well as lead roots.
				if (startingBranchNode != null)
				{
					LinkedNode<VerbalizationPlanNode> newTailLink = startingBranchTailLink ?? startingBranchNode.FirstChildNode;
					if (newTailLink != null)
					{
						newTailLink = newTailLink.GetTail();
					}
					if (newTailLink != startingBranchTailLink)
					{
						VerbalizationPlanNode.InsertFloatingRootVariableNode(rootObjectTypeVariable, newTailLink.Value, ref myRootPlanNode);
					}
				}
				else
				{
					myCurrentBranchNode = VerbalizationPlanNode.InsertFloatingRootVariableNode(rootObjectTypeVariable, myCurrentBranchNode, ref myRootPlanNode);
				}
			}
			requiredVariableKeys = pendingRequiredVariableKeys;
			if (initialPathContext == null)
			{
				++myLatestUsePhase;
			}
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
				myHeaderListBranchingBits = headerListBits = TranslateBranchTypeDirective(CoreVerbalizationSnippetType.RolePathHeaderListDirective);
			}
			return (0 != (headerListBits & (1 << ((int)branchType - 1)))) ? VerbalizationPlanBranchRenderingStyle.HeaderList : VerbalizationPlanBranchRenderingStyle.OperatorSeparated;
		}
		/// <summary>
		/// Determine whether a <see cref="VerbalizationPlanBranchType"/> branch style allows
		/// a lead role player to be collapsed in a non-lead list item.
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
				myCollapsibleLeadBranchingBits = collapsibleLeadBits = TranslateBranchTypeDirective(CoreVerbalizationSnippetType.RolePathCollapsibleLeadDirective);
			}
			return 0 != (collapsibleLeadBits & (1 << ((int)branchType - 1)));
		}
		/// <summary>
		/// Determine whether a <see cref="VerbalizationPlanBranchType"/> branch style allows
		/// the list open text to be collapsed to allow a direct back reference.
		/// </summary>
		private bool GetCollapsibleListOpenForBackReferenceAllowedFromBranchType(VerbalizationPlanBranchType branchType)
		{
			if (branchType == VerbalizationPlanBranchType.None)
			{
				return false;
			}
			int collapsibleLeadBits = myCollapsibleListOpenForBackReferenceBranchingBits;
			if (collapsibleLeadBits == -1)
			{
				myCollapsibleListOpenForBackReferenceBranchingBits = collapsibleLeadBits = TranslateBranchTypeDirective(CoreVerbalizationSnippetType.RolePathCollapsibleListOpenForBackReferenceDirective);
			}
			return 0 != (collapsibleLeadBits & (1 << ((int)branchType - 1)));
		}
		/// <summary>
		/// Translate a directive snippet consisting of a space-separated string
		/// of branch type directives into bits, with bits corresponding to the
		/// numeric values of <see cref="VerbalizationPlanBranchType"/>
		/// </summary>
		private int TranslateBranchTypeDirective(CoreVerbalizationSnippetType branchDirectiveSnippet)
		{
			int directiveBits = 0;
			string[] directiveStrings = myRenderer.GetSnippet(branchDirectiveSnippet).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			if (directiveStrings != null)
			{
				Type enumType = typeof(VerbalizationPlanBranchType);
				for (int i = 0; i < directiveStrings.Length; ++i)
				{
					string directiveString = directiveStrings[i];
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
						directiveBits |= (1 << ((int)(VerbalizationPlanBranchType)result - 1));
					}
				}
			}
			return directiveBits;
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
		/// Determine if a specific <see cref="CoreVerbalizationSnippetType"/>
		/// value is marked as an outdent snippet by the dynamic
		/// <see cref="CoreVerbalizationSnippetType.RolePathListCloseOutdentSnippets"/> snippet.
		/// </summary>
		private bool IsOutdentSnippet(CoreVerbalizationSnippetType snippetType)
		{
			BitTracker tracker = myOutdentSnippetBits;
			if (tracker.Count == 0)
			{
				myOutdentSnippetBits = tracker = TranslateSnippetTypeDirective(CoreVerbalizationSnippetType.RolePathListCloseOutdentSnippets);
			}
			return tracker[(int)snippetType];
		}
		/// <summary>
		/// Determine if a specific <see cref="CoreVerbalizationSnippetType"/>
		/// value is marked as an outdent aware snippet by the dynamic
		/// <see cref="CoreVerbalizationSnippetType.RolePathOutdentAwareTrailingListSnippets"/> snippet.
		/// </summary>
		private bool IsOutdentAwareSnippet(CoreVerbalizationSnippetType snippetType)
		{
			BitTracker tracker = myOutdentAwareSnippetBits;
			if (tracker.Count == 0)
			{
				myOutdentAwareSnippetBits = tracker = TranslateSnippetTypeDirective(CoreVerbalizationSnippetType.RolePathOutdentAwareTrailingListSnippets);
			}
			return tracker[(int)snippetType];
		}
		/// <summary>
		/// Helper method to return a bit tracker with true values for
		/// any snippet type in the space separated string.
		/// </summary>
		private BitTracker TranslateSnippetTypeDirective(CoreVerbalizationSnippetType snippetType)
		{
			BitTracker retVal = new BitTracker((int)CoreVerbalizationSnippetType.Last + 1);
			string[] outdentSnippets = myRenderer.GetSnippet(snippetType).Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			if (outdentSnippets != null)
			{
				Type enumType = typeof(CoreVerbalizationSnippetType);
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
						retVal[(int)(CoreVerbalizationSnippetType)result] = true;
					}
				}
			}
			return retVal;
		}
		private void ResolveReadings(VerbalizationPlanNode verbalizationNode, LinkedNode<VerbalizationPlanNode> verbalizationNodeLink, bool canCollapseLead, ref RolePlayerVariable contextLeadVariable, ref RolePlayerVariable contextTrailingVariable)
		{
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			switch (verbalizationNode.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					FactType factType = verbalizationNode.FactType;
					PathedRole factTypeEntry = verbalizationNode.FactTypeEntry;
					object pathContext = verbalizationNode.PathContext;
					RoleBase entryRoleBase = ResolveRoleBaseInFactType(factTypeEntry.Role, factType);
					RolePlayerVariable localContextLeadVariable = contextLeadVariable;
					RolePlayerVariable localContextTrailingVariable = contextTrailingVariable;
					LinkedElementCollection<ReadingOrder> readingOrders = factType.ReadingOrderCollection;
					IReading reading = null;
					VerbalizationPlanReadingOptions options = VerbalizationPlanReadingOptions.None;
					if (localContextLeadVariable != null)
					{
						// Continue with another reading starting with the lead role of the
						// preceding fact type.
						if (localContextLeadVariable == GetRolePlayerVariableUse(new RolePathNode(factTypeEntry, pathContext)).Value.PrimaryRolePlayerVariable)
						{
							// Optimization of next branch to test the entry variable without invoking the delegate
							reading = factType.GetMatchingReading(readingOrders, null, entryRoleBase, null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound);
						}
						else
						{
							VisitPathedRolesForFactTypeEntry(
								factTypeEntry,
								delegate(PathedRole testPathedRole)
								{
									RolePlayerVariableUse? nullableVariableUse;
									return !(testPathedRole != factTypeEntry && // Tested in previous branch
										(nullableVariableUse = GetRolePlayerVariableUse(new RolePathNode(testPathedRole, pathContext))).HasValue &&
										nullableVariableUse.Value.PrimaryRolePlayerVariable == localContextLeadVariable &&
										null != (reading = factType.GetMatchingReading(readingOrders, null, ResolveRoleBaseInFactType(testPathedRole.Role, factType), null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound)));
								});
						}
						if (reading != null)
						{
							options |= VerbalizationPlanReadingOptions.FullyCollapseFirstRole | VerbalizationPlanReadingOptions.BasicLeadRole;
						}
					}
					if (reading == null && localContextTrailingVariable != null)
					{
						IReading pairedReading = null;
						VisitPathedRolesForFactTypeEntry(
							factTypeEntry,
							delegate(PathedRole testPathedRole)
							{
								bool exactMatch;
								if (CanPartnerWithVariable(new RolePathNode(testPathedRole, pathContext), pathContext, localContextTrailingVariable, out exactMatch) &&
									null != (reading = factType.GetMatchingReading(readingOrders, null, ResolveRoleBaseInFactType(testPathedRole.Role, factType), null, null, MatchingReadingOptions.NoFrontText | MatchingReadingOptions.LeadRolesNotHyphenBound)))
								{
									// Keep going if we don't have an exact variable match, we might still find one.
									if (exactMatch)
									{
										return false;
									}
									if (pairedReading == null)
									{
										pairedReading = reading;
									}
									reading = null;
								}
								return true;
							});
						if (reading != null)
						{
							// We have a forward primary reading starting with the same variable
							options |= VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.BasicLeadRole;
						}
						else if (pairedReading != null)
						{
							// The primary reading has a lead role that can be successfully
							// paired with the context variable.
							reading = pairedReading;
							options |= VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.BasicLeadRole;
							verbalizationNode.CorrelateWithBackReferencedVariable = localContextTrailingVariable;
						}
					}

					// Determine basic lead reading settings plus lead and trailing variables
					string readingText;
					if (reading == null)
					{
						// Fall back on any reading for the current entry, or a mocked-up reading if no others are available
						reading = factType.GetMatchingReading(readingOrders, null, entryRoleBase, null, null, MatchingReadingOptions.AllowAnyOrder) ?? factType.GetDefaultReading();
						readingText = reading.Text;
						if (readingText.StartsWith("{0}") && !VerbalizationHyphenBinder.IsHyphenBound(readingText, 0))
						{
							options |= VerbalizationPlanReadingOptions.BasicLeadRole;
						}
					}
					else
					{
						readingText = reading.Text;
					}
					IList<RoleBase> roles = reading.RoleCollection;
					if (canCollapseLead)
					{
						if (null != localContextLeadVariable &&
							0 == (options & VerbalizationPlanReadingOptions.FullyCollapseFirstRole))
						{
							contextLeadVariable = null;
						}
						if (contextLeadVariable == null &&
							0 != (options & VerbalizationPlanReadingOptions.BasicLeadRole))
						{
							Role findRole = roles[0].Role;
							// Find the corresponding variable in the fact type entry
							VisitPathedRolesForFactTypeEntry(
								factTypeEntry,
								delegate(PathedRole testPathedRole)
								{
									if (testPathedRole.Role == findRole)
									{
										RolePlayerVariableUse? testPathedRoleVariableUse = GetRolePlayerVariableUse(new RolePathNode(testPathedRole, pathContext));
										if (testPathedRoleVariableUse.HasValue)
										{
											localContextLeadVariable = testPathedRoleVariableUse.Value.PrimaryRolePlayerVariable;
										}
										return false;
									}
									return true;
								});
							contextLeadVariable = localContextLeadVariable;
						}
					}
					else
					{
						contextLeadVariable = null;
					}
					
					// Get the trailing information for the next step
					int roleCount = roles.Count;
					localContextTrailingVariable = null;
					PathedRole oppositePathedRole = null;
					VerbalizationPlanNode parentNode;
					bool reverseReading = false;
					Role entryRole;
					bool checkOppositeNegation =
						roleCount == 2 &&
						(roles[0] == (entryRole = factTypeEntry.Role) ||
						(reverseReading = (roles[1] == entryRole))) &&
						verbalizationNodeLink != null &&
						verbalizationNodeLink.Previous == null &&
						null != (parentNode = verbalizationNode.ParentNode) &&
						parentNode.BranchType == VerbalizationPlanBranchType.NegatedChain;
					bool hasTrailingRolePlayer = roleCount > 1 && readingText.EndsWith("{" + (roleCount - 1).ToString(CultureInfo.InvariantCulture) + "}");
					if (checkOppositeNegation || hasTrailingRolePlayer)
					{
						Role findTrailingRole = (hasTrailingRolePlayer || !reverseReading) ? roles[roleCount - 1].Role : null;
						Role findLeadRole = (reverseReading && checkOppositeNegation) ? roles[0].Role : null;
						VisitPathedRolesForFactTypeEntry(
							factTypeEntry,
							delegate(PathedRole testPathedRole)
							{
								Role testRole = testPathedRole.Role;
								if (testRole == findTrailingRole)
								{
									if (!reverseReading || checkOppositeNegation)
									{
										oppositePathedRole = testPathedRole;
									}
									if (hasTrailingRolePlayer)
									{
										RolePlayerVariableUse? pathedRoleVariableUse = GetRolePlayerVariableUse(new RolePathNode(testPathedRole, pathContext));
										if (pathedRoleVariableUse.HasValue)
										{
											localContextTrailingVariable = pathedRoleVariableUse.Value.PrimaryRolePlayerVariable;
										}
									}
									return false;
								}
								if (testRole == findLeadRole &&
									reverseReading)
								{
									oppositePathedRole = testPathedRole;
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
						// Always use the negated article for the fully existential case unless
						// the type changed for the lead role, in which case both the variable
						// pairing and the fact statement need to be inside the same negation
						// and not inlined.
						else if (myRolePathCache.GetPreviousPathNode(new RolePathNode(factTypeEntry, verbalizationNode.PathContext)).ObjectType != factTypeEntry.Role.RolePlayer)
						{
							// Resolve the variable pairing during rendering
							options |= VerbalizationPlanReadingOptions.DynamicNegatedExitRole;
						}
						else
						{
							options |= VerbalizationPlanReadingOptions.NegatedExitRole;
						}
					}
					verbalizationNode.Reading = reading;
					verbalizationNode.ReadingOptions = options;
					break;
				case VerbalizationPlanNodeType.Branch:
					VerbalizationPlanBranchType branchType = verbalizationNode.BranchType;

					// Check role collapsing constructs based on branch type.
					// Dynamic negation inlining means that we may not know until rendering
					// the path if a construct can be inlined or not, so we recheck the collapsible
					// lead permissions for a negated chain dynamically based on whether or
					// not the negation is inlined. We need to calculate collapsed lead settings
					// just in case we use them, so we always mark collapsing as possible
					// irrespective of the directive settings for the negated chain.
					bool childCanCollapseLead = branchType == VerbalizationPlanBranchType.NegatedChain ? canCollapseLead : GetCollapsibleLeadAllowedFromBranchType(branchType);
					RolePlayerVariable blockContextLeadVariable; // Either the context lead variable, or the lead variable for the first child if no context is provided
					bool blockContextLeadVariableIsStable = true; // Switch to false if the block context variable changes for different nodes. The only stable change is a shift from null for the first node.
					if (childCanCollapseLead)
					{
						blockContextLeadVariable = contextLeadVariable;
					}
					else
					{
						contextLeadVariable = blockContextLeadVariable = null;
					}

					// Handle back reference constructs based on branch type, with similar
					// special handling for negated chains as employed role collapsing.
					// Back references are supported on the first node in a branch only, but
					// can be nested, with a nested branch providing the first back reference.
					if (contextTrailingVariable != null &&
						branchType != VerbalizationPlanBranchType.NegatedChain &&
						!GetCollapsibleListOpenForBackReferenceAllowedFromBranchType(branchType))
					{
						contextTrailingVariable = null;
					}

					// If this is a splitting branch instead of a chaining branch, then the
					// trailing variable (providing the anchor for a possible future back reference)
					// must be cleared after each step. Verify this with the branch type up front.
					bool isSplittingBranch = BranchSplits(branchType);

					childNodeLink = verbalizationNode.FirstChildNode;
					bool first = true;
					while (childNodeLink != null)
					{
						VerbalizationPlanNode childNode = childNodeLink.Value;
						ResolveReadings(childNode, childNodeLink, childCanCollapseLead, ref contextLeadVariable, ref contextTrailingVariable);
						switch (GetLeadContextChange(childNode, false))
						{
							case LeadContextChange.Cleared:
							case LeadContextChange.NestedChange:
								blockContextLeadVariableIsStable = false;
								contextLeadVariable = blockContextLeadVariable = null;
								break;
							case LeadContextChange.InitialChange:
								if (first)
								{
									if (blockContextLeadVariable == null || !isSplittingBranch)
									{
										blockContextLeadVariable = contextLeadVariable;
									}
									else if (blockContextLeadVariable != contextLeadVariable)
									{
										blockContextLeadVariableIsStable = false;
									}
								}
								else if (blockContextLeadVariable != contextLeadVariable || !isSplittingBranch)
								{
									blockContextLeadVariableIsStable = false;
								}
								break;
						}
						first = false;
						if (isSplittingBranch)
						{
							contextTrailingVariable = null;
						}
						childNodeLink = childNodeLink.Next;
					}
					contextLeadVariable = (canCollapseLead && blockContextLeadVariableIsStable) ? blockContextLeadVariable : null;
					contextTrailingVariable = null;
					break;
				case VerbalizationPlanNodeType.HeadCalculatedValueProjection:
				case VerbalizationPlanNodeType.HeadConstantProjection:
				case VerbalizationPlanNodeType.CalculatedCondition:
				case VerbalizationPlanNodeType.ValueConstraint:
					if (!verbalizationNode.RestrictsPreviousFactType)
					{
						// Allow restriction conditions without losing the lead
						// If this is changed, synchronize the corresponding case in GetLeadContextChange
						contextLeadVariable = null;
					}
					contextTrailingVariable = null;
					break;
				case VerbalizationPlanNodeType.ChainedRootVariable:
					contextLeadVariable = null;
					contextTrailingVariable = verbalizationNode.RootVariable;
					break;
				case VerbalizationPlanNodeType.VariableExistence:
					// These should be leaf nodes, so the settings here should not matter.
					contextLeadVariable = null;
					contextTrailingVariable = null;
					break;
				case VerbalizationPlanNodeType.FloatingRootVariableContext:
					childNodeLink = verbalizationNode.FirstChildNode;
					// There will always be exactly one child node link at this point
					ResolveReadings(childNodeLink.Value, childNodeLink, canCollapseLead, ref contextLeadVariable, ref contextTrailingVariable);
					break;
			}
		}
		/// <summary>
		/// Return values for <see cref="GetLeadContextChange"/>
		/// </summary>
		private enum LeadContextChange
		{
			/// <summary>
			/// The lead context did not change
			/// </summary>
			None,
			/// <summary>
			/// The context was cleared by a nested node
			/// </summary>
			Cleared,
			/// <summary>
			/// The first node that affects context changed the context
			/// </summary>
			InitialChange,
			/// <summary>
			/// The lead context changes at a nested node
			/// </summary>
			NestedChange,
		}
		/// <summary>
		/// Test whether rendering of a node will change or clear the current lead context.
		/// If this is a <paramref name="dynamicCheck"/>, then this method should be called after
		/// the node has been rendered. This method determines when we can safely collapse lead
		/// roles, which must restate the role player if the previous verbalization clears or modifies
		/// the context. Normally this can be done during static analysis, but variable introduction
		/// and dynamic inline negation means that collapsed roles may need to be restated based
		/// on head variable conditions and other issues that are unknown when the tree is produced.
		/// </summary>
		/// <param name="verbalizationNode">The verbalization node to verify.</param>
		/// <param name="dynamicCheck"><see langword="true"/> if this is being called while the path
		/// is being rendered. A static treats all inline nodes that may be negated as nodes that
		/// will be inlined.</param>
		private LeadContextChange GetLeadContextChange(VerbalizationPlanNode verbalizationNode, bool dynamicCheck)
		{
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			if (dynamicCheck && verbalizationNode.RenderedRequiredContextVariable)
			{
				return LeadContextChange.Cleared;
			}
			switch (verbalizationNode.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					// Note that a negated fact type options are handled inline with a NegatedChain branch
					return (0 != (verbalizationNode.ReadingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole)) ? LeadContextChange.None : LeadContextChange.InitialChange;
				case VerbalizationPlanNodeType.Branch:
					VerbalizationPlanBranchType branchType = verbalizationNode.BranchType;
					childNodeLink = null;
					switch (branchType)
					{
						case VerbalizationPlanBranchType.Chain:
							childNodeLink = verbalizationNode.FirstChildNode;
							break;
						case VerbalizationPlanBranchType.NegatedChain:
							childNodeLink = verbalizationNode.FirstChildNode;
							if (childNodeLink != null &&
								0 == (childNodeLink.Value.ReadingOptions & (dynamicCheck ? VerbalizationPlanReadingOptions.NegatedExitRole : (VerbalizationPlanReadingOptions.NegatedExitRole | VerbalizationPlanReadingOptions.DynamicNegatedExitRole))) &&
								!GetCollapsibleLeadAllowedFromBranchType(VerbalizationPlanBranchType.NegatedChain))
							{
								// If the negation is not inlined and the negation snippet does not allow collapsing,
								// then we 'clear' at this point instead of registering an initial change because
								// regardless of whether or not the negated chain branch type allows collapsing,
								// the lead variable is being introduced inside negation.
								return LeadContextChange.Cleared;
							}
							// The lead negated fact type is treated as a normal fact type, continue normal processing
							break;
						default:
							if (GetRenderingStyleFromBranchType(branchType) != VerbalizationPlanBranchRenderingStyle.OperatorSeparated)
							{
								return LeadContextChange.Cleared;
							}
							childNodeLink = verbalizationNode.FirstChildNode;
							break;
					}
					bool first = true;
					while (childNodeLink != null)
					{
						switch (GetLeadContextChange(childNodeLink.Value, dynamicCheck))
						{
							case LeadContextChange.Cleared:
								return LeadContextChange.Cleared;
							case LeadContextChange.InitialChange:
								return first ? LeadContextChange.InitialChange : LeadContextChange.NestedChange;
							case LeadContextChange.NestedChange:
								return LeadContextChange.NestedChange;
						}
						first = false;
						childNodeLink = childNodeLink.Next;
					}
					break;
				case VerbalizationPlanNodeType.HeadCalculatedValueProjection:
				case VerbalizationPlanNodeType.HeadConstantProjection:
				case VerbalizationPlanNodeType.CalculatedCondition:
				case VerbalizationPlanNodeType.ValueConstraint:
					if (!verbalizationNode.RestrictsPreviousFactType)
					{
						// Allow restriction conditions without losing the lead
						// If this is changed, synchronize the corresponding case in ResolveReadings
						return LeadContextChange.Cleared;
					}
					break;
				case VerbalizationPlanNodeType.ChainedRootVariable:
				case VerbalizationPlanNodeType.VariableExistence:
					return LeadContextChange.Cleared;
				case VerbalizationPlanNodeType.FloatingRootVariableContext:
					// This is a directive node, pass through the children
					childNodeLink = verbalizationNode.FirstChildNode;
					if (childNodeLink != null)
					{
						return GetLeadContextChange(childNodeLink.Value, dynamicCheck);
					}
					break;
			}
			return LeadContextChange.None;
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
				foreach (RolePath nestedPath in cache.SubPathCollection(rolePath))
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
		/// Recursive helper method for resolving all pathed roles used by a single fact type entry
		/// </summary>
		/// <param name="factTypeEntry">The <see cref="PathedRole"/> that enters the fact type instance.</param>
		/// <param name="visitor">A <see cref="Predicate{PathedRole}"/> callback.
		/// Return <see langword="true"/> to continue.</param>
		private void VisitPathedRolesForFactTypeEntry(PathedRole factTypeEntry, Predicate<PathedRole> visitor)
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
		private bool NotifyLeadSameFactTypeRoles(RolePath parentRolePath, Predicate<PathedRole> visitor)
		{
			RolePathCache cache = myRolePathCache;
			foreach (RolePath childRolePath in cache.SubPathCollection(parentRolePath))
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
		/// <param name="pathContext">The path context for this fact type.</param>
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
		/// <param name="pendingRequiredVariableKeys">Current required variable keys that will be required for the fact type.</param>
		/// <returns>The base index in the <paramref name="usedRoles"/> tracker corresponding
		/// to the first role in the new top of the <paramref name="baseRolesStack"/></returns>
		private int PushFactType(
			object pathContext,
			FactType factType,
			PathedRole factTypeEntry,
			ReadOnlyCollection<PathedRole> pathedRoles,
			int factTypeEntryIndex,
			Stack<LinkedElementCollection<RoleBase>> baseRolesStack,
			ref BitTracker usedRoles,
			LinkedElementCollection<CalculatedPathValue> pathConditions,
			ref BitTracker processedConditions,
			ref LinkedNode<object> pendingRequiredVariableKeys)
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
				PathConditionRoleValueConstraint valueConstraint = currentPathedRole.ValueConstraint;
				if (valueConstraint != null)
				{
					if (!addedFactTypeEntry)
					{
						addedFactTypeEntry = true;
						contextChainNode = EnsureChainNodeForFactTypeConditions(pathContext, factType, factTypeEntry, ref pendingRequiredVariableKeys);
					}
					VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, pathContext, contextChainNode, ref myRootPlanNode);
				}
			}
			
			// Look for functions that use roles restricted to this section of the path
			int conditionCount = pathConditions.Count;
			if (conditionCount != 0)
			{
				object[] pathedRoleCorrelationRoots = null;
				RolePathCache rolePathCache = default(RolePathCache);
				for (int i = 0; i < conditionCount; ++i)
				{
					if (!processedConditions[i])
					{
						CalculatedPathValue calculation = pathConditions[i];
						bool? isLocal = IsLocalCalculatedValue(
							pathContext,
							calculation,
							delegate(RolePathNode testPathNode)
							{
								if (pathedRoleCorrelationRoots == null)
								{
									rolePathCache = EnsureRolePathCache();
									pathedRoleCorrelationRoots = new object[roleCount - factTypeEntryIndex];
									for (int j = factTypeEntryIndex; j < roleCount; ++j)
									{
										pathedRoleCorrelationRoots[j - factTypeEntryIndex] = rolePathCache.GetCorrelationRoot(new RolePathNode(pathedRoles[j], pathContext));	
									}
								}
								object testCorrelationRoot = rolePathCache.GetCorrelationRoot(testPathNode);
								for (int j = factTypeEntryIndex; j < roleCount; ++j)
								{
									if (pathedRoleCorrelationRoots[j - factTypeEntryIndex] == testCorrelationRoot)
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
								contextChainNode = EnsureChainNodeForFactTypeConditions(pathContext, factType, factTypeEntry, ref pendingRequiredVariableKeys);
							}
							processedConditions[i] = true;
							VerbalizationPlanNode.AddCalculatedConditionNode(calculation, true, pathContext, contextChainNode, ref myRootPlanNode);
						}
					}
				}
			}

			if (!addedFactTypeEntry)
			{
				VerbalizationPlanNode newFactTypeNode;
				myCurrentBranchNode = (newFactTypeNode = VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, pathContext, myCurrentBranchNode, ref myRootPlanNode)).ParentNode;
				if (pendingRequiredVariableKeys != null)
				{
					newFactTypeNode.RequireContextVariables(pendingRequiredVariableKeys);
					pendingRequiredVariableKeys = null;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for <see cref="PushFactType"/>
		/// </summary>
		private VerbalizationPlanNode EnsureChainNodeForFactTypeConditions(object pathContext, FactType factType, PathedRole factTypeEntry, ref LinkedNode<object> pendingRequiredVariableKeys)
		{
			VerbalizationPlanNode contextChainNode = myCurrentBranchNode;
			VerbalizationPlanNode newFactTypeNode;
			if (contextChainNode == null)
			{
				myCurrentBranchNode = contextChainNode = (newFactTypeNode = VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, pathContext, myCurrentBranchNode, ref myRootPlanNode)).ParentNode;
			}
			else
			{
				if (contextChainNode.BranchType != VerbalizationPlanBranchType.Chain)
				{
					contextChainNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, contextChainNode, ref myRootPlanNode);
				}
				newFactTypeNode = VerbalizationPlanNode.AddFactTypeEntryNode(factType, factTypeEntry, pathContext, contextChainNode, ref myRootPlanNode);
			}
			if (pendingRequiredVariableKeys != null)
			{
				newFactTypeNode.RequireContextVariables(pendingRequiredVariableKeys);
				pendingRequiredVariableKeys = null;
			}
			return contextChainNode;
		}
		/// <summary>
		/// Helper method for handling subqueries as an alternate to PushFactType.
		/// </summary>
		/// <remarks>PopFactType handles both this and PushFactType. Parameters correspond either
		/// to variables in InitializeRolePath or parameters in PushFactType</remarks>
		private void PushSubquery(
			object pathContext,
			LeadRolePath leadRolePath,
			FactType factType,
			PathedRole factTypeEntry,
			ReadOnlyCollection<PathedRole> pathedRoles,
			int factTypeEntryIndex,
			Stack<LinkedElementCollection<RoleBase>> baseRolesStack,
			ref BitTracker usedRoles,
			ref int roleUseBaseIndex,
			LinkedElementCollection<CalculatedPathValue> pathConditions,
			ref BitTracker processedConditions,
			ref Stack<LinkedElementCollection<QueryParameter>> queryParametersStack,
			ref List<InlineSubqueryRole> inlineSubqueryRoles,
			ref int inlineSubqueryRoleBaseIndex,
			ref List<InlineSubqueryParameter> inlineSubqueryParameters,
			ref int inlineSubqueryParameterBaseIndex,
			ref LinkedNode<object> pendingRequiredVariableKeys)
		{
			// To minimize variance, set up role tracking the same way we do with PushFactType.
			roleUseBaseIndex = usedRoles.Count;
			LinkedElementCollection<RoleBase> queryRoles = factType.RoleCollection;
			int roleCount = queryRoles.Count;
			QueryBase query = factType as QueryBase;
			LinkedElementCollection<QueryParameter> queryParameters = query != null ? query.ParameterCollection : null;
			SubqueryParameterInputs parameterInputs = null;
			int parameterCount = 0;
			(queryParametersStack ?? (queryParametersStack = new Stack<LinkedElementCollection<QueryParameter>>())).Push(queryParameters);
			baseRolesStack.Push(queryRoles);
			usedRoles.Resize(roleUseBaseIndex + roleCount);

			inlineSubqueryRoleBaseIndex = (inlineSubqueryRoles ?? (inlineSubqueryRoles = new List<InlineSubqueryRole>())).Count;
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			if (queryParameters != null)
			{
				parameterCount = queryParameters.Count;
				inlineSubqueryParameterBaseIndex = (inlineSubqueryParameters ?? (inlineSubqueryParameters = new List<InlineSubqueryParameter>())).Count;
				parameterInputs = SubqueryParameterInputs.GetLink(leadRolePath, factTypeEntry);
				for (int i = 0; i < parameterCount; ++i)
				{
					// See notes below in role processing. We process parameters first as input/output is
					// the natural order for the free variables
					QueryParameter parameter = queryParameters[i];
					InlineSubqueryParameter queryParameterKey = new InlineSubqueryParameter(pathContext, factTypeEntry, parameter);
					inlineSubqueryParameters.Add(queryParameterKey);
					if (pendingRequiredVariableKeys == null)
					{
						pendingRequiredVariableKeys = new LinkedNode<object>(queryParameterKey);
					}
					else
					{
						pendingRequiredVariableKeys.GetTail().SetNext(new LinkedNode<object>(queryParameterKey), ref pendingRequiredVariableKeys);
					}
					RegisterRolePlayerUse(parameter.ParameterType, null, queryParameterKey, RolePathNode.Empty);
					// Correlate the parameter key with input pathed roles and path roots. Input constants
					// and calculated values will be bound later.
					SubqueryParameterInput parameterInput;
					if (parameterInputs != null &&
						null != (parameterInput = SubqueryParameterInput.GetLink(parameterInputs, parameter)))
					{
						PathedRole inputPathedRole;
						RolePathObjectTypeRoot inputPathRoot;
						RolePathNode inputNode = RolePathNode.Empty;
						if (null != (inputPathedRole = parameterInput.InputFromPathedRole))
						{
							inputNode = new RolePathNode(inputPathedRole, pathContext);
						}
						else if (null != (inputPathRoot = parameterInput.InputFromPathRoot))
						{
							inputNode = new RolePathNode(inputPathRoot, pathContext);
						}
						if (!inputNode.IsEmpty)
						{
							// The input nodes are not guaranteed to be registered at this point,
							// so we need to verify existing before accessing them.
							object inputKey = inputNode;
							RolePlayerVariableUse? existingInputUse = GetRolePlayerVariableUse(inputKey);
							CustomCorrelateVariables(existingInputUse.HasValue ? existingInputUse.Value.PrimaryRolePlayerVariable : RegisterRolePlayerUse(inputNode.ObjectType, null, inputKey, inputNode), useMap[queryParameterKey].PrimaryRolePlayerVariable);
						}
					}
				}
			}
			Role factTypeEntryRole = factTypeEntry.Role;
			for (int i = 0; i < roleCount; ++i)
			{
				// If a role is associated with a pathed role in the current section of the path then we
				// associate that path node with the key for that variable. The variable for the entry role
				// must be defined at this point. However, other variables may need to be existentially defined,
				// including variables used in path splits. The goal here is to place the new existential variables
				// in the defined order of the subquery roles, so the conditionally required roles will be attached
				// as required variables on the other roles. In the common case where all subquery roles are bound
				// to existing variables, this will result in no existentials for the subquery.
				// Note that the pathed roles and role in the subquery can have different types, so we are careful
				// to register the role player type from the query role.

				Role queryRole = queryRoles[i].Role;
				InlineSubqueryRole queryRoleKey = new InlineSubqueryRole(pathContext, factTypeEntry, queryRole);
				inlineSubqueryRoles.Add(queryRoleKey);
				if (pendingRequiredVariableKeys == null)
				{
					pendingRequiredVariableKeys = new LinkedNode<object>(queryRoleKey);
				}
				else
				{
					pendingRequiredVariableKeys.GetTail().SetNext(new LinkedNode<object>(queryRoleKey), ref pendingRequiredVariableKeys);
				}
				RegisterRolePlayerUse(queryRole.RolePlayer, null, queryRoleKey, RolePathNode.Empty);
				if (queryRole == factTypeEntryRole)
				{
					CustomCorrelateVariables(
						useMap[new RolePathNode(factTypeEntry, pathContext)].PrimaryRolePlayerVariable,
						useMap[queryRoleKey].PrimaryRolePlayerVariable);
				}
			}

			RolePathOwner subqueryRule = factType.DerivationRule;

			// Bind subquery role projection information to the outer inline roles
			Dictionary<LeadRolePath, InlineSubqueryContext> subqueryContexts = new Dictionary<LeadRolePath, InlineSubqueryContext>();
			InlineSubqueryContext innerPathContext = null;
			LeadRolePath innerPathContextForPath = null;
			List<InlineSubqueryRole> localInlineSubqueryRoles = inlineSubqueryRoles;
			int localInlineSubqueryRoleBaseIndex = inlineSubqueryRoleBaseIndex;
			List<InlineSubqueryParameter> localInlineSubqueryParameters = inlineSubqueryParameters;
			int localInlineSubqueryParameterBaseIndex = inlineSubqueryParameterBaseIndex;
			VariableKeyDecorator keyDecorator = delegate(object key)
			{
				Role role;
				QueryParameter parameter;
				int index;
				if (null != (role = key as Role))
				{
					if (-1 != (index = ResolveRoleIndex(queryRoles, role))) // Guard against bogus path
					{
						return localInlineSubqueryRoles[localInlineSubqueryRoleBaseIndex + index];
					}
				}
				else if (null != (parameter = key as QueryParameter))
				{
					if (-1 != (index = queryParameters.IndexOf(parameter)))
					{
						return localInlineSubqueryParameters[localInlineSubqueryParameterBaseIndex + index]; // Guard against bogus path
					}
				}
				return key; // Fallback, shouldn't reach here
			};
			IDictionary<LeadRolePath, IList<IList<object>>> equivalentSubqueryProjectionKeysByPath = GenerateRoleAndParameterProjections(
				subqueryRule,
				delegate(object key, LeadRolePath forRolePath, ObjectType variableType, RolePathNode correlationNode)
				{
					if (innerPathContextForPath != forRolePath)
					{
						innerPathContextForPath = forRolePath;
						if (!subqueryContexts.TryGetValue(forRolePath, out innerPathContext))
						{
							subqueryContexts[forRolePath] = innerPathContext = new InlineSubqueryContext(pathContext, factTypeEntry, forRolePath);
						}
					}
					if (!correlationNode.IsEmpty)
					{
						correlationNode = new RolePathNode(correlationNode, innerPathContext);
					}
					RolePlayerVariable newVar = RegisterRolePlayerUse(variableType, null, correlationNode, correlationNode);
					if (!correlationNode.IsEmpty)
					{
						RolePlayerVariableUse? correlateWithVar = GetRolePlayerVariableUse(key);
						if (correlateWithVar.HasValue)
						{
							CustomCorrelateVariables(correlateWithVar.Value.PrimaryRolePlayerVariable, newVar);
						}
					}
				},
				keyDecorator);

			// Add value constraints and local condition code based on roles in the outer path
			int currentPathedRoleCount = pathedRoles.Count;
			for (int i = factTypeEntryIndex + 1; i < currentPathedRoleCount; ++i)
			{
				PathedRole testPathedRole = pathedRoles[i];
				if (testPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					// Restrict the pathed role count
					currentPathedRoleCount = i;
					break;
				}
			}

			VerbalizationPlanNode contextConditionNode = null;
			for (int j = factTypeEntryIndex; j < currentPathedRoleCount; ++j)
			{
				ValueConstraint valueConstraint = pathedRoles[j].ValueConstraint;
				if (valueConstraint != null)
				{
					if (contextConditionNode == null)
					{
						PushSplit(pathContext, VerbalizationPlanBranchType.AndSplit, ref pendingRequiredVariableKeys);
						contextConditionNode = myCurrentBranchNode;
					}
					VerbalizationPlanNode.AddValueConstraintNode(valueConstraint, pathContext, contextConditionNode, ref myRootPlanNode);
				}
			}

			// Look for functions that use roles restricted to this section of the path
			int conditionCount = pathConditions.Count;
			if (conditionCount != 0)
			{
				object[] pathedRoleCorrelationRoots = null;
				RolePathCache rolePathCache = default(RolePathCache);
				for (int i = 0; i < conditionCount; ++i)
				{
					if (!processedConditions[i])
					{
						CalculatedPathValue calculation = pathConditions[i];
						bool? isLocal = IsLocalCalculatedValue(
							pathContext,
							calculation,
							delegate(RolePathNode testPathNode)
							{
								if (pathedRoleCorrelationRoots == null)
								{
									rolePathCache = EnsureRolePathCache();
									pathedRoleCorrelationRoots = new object[currentPathedRoleCount - factTypeEntryIndex];
									for (int j = factTypeEntryIndex; j < currentPathedRoleCount; ++j)
									{
										pathedRoleCorrelationRoots[j - factTypeEntryIndex] = rolePathCache.GetCorrelationRoot(new RolePathNode(pathedRoles[j], pathContext));
									}
								}
								object testCorrelationRoot = rolePathCache.GetCorrelationRoot(testPathNode);
								for (int j = factTypeEntryIndex; j < currentPathedRoleCount; ++j)
								{
									if (pathedRoleCorrelationRoots[j - factTypeEntryIndex] == testCorrelationRoot)
									{
										return true;
									}
								}
								return false;
							});
						if (isLocal.GetValueOrDefault(false))
						{
							if (contextConditionNode == null)
							{
								PushSplit(pathContext, VerbalizationPlanBranchType.AndSplit, ref pendingRequiredVariableKeys);
								contextConditionNode = myCurrentBranchNode;
							}
							processedConditions[i] = true;
							VerbalizationPlanNode.AddCalculatedConditionNode(calculation, false, pathContext, contextConditionNode, ref myRootPlanNode);
						}
					}
				}
			}

			// Add constant and functional parameter inputs
			if (parameterInputs != null)
			{
				for (int i = 0; i < parameterCount; ++i)
				{
					QueryParameter parameter = queryParameters[i];
					SubqueryParameterInput parameterInput;
					if (null != (parameterInput = SubqueryParameterInput.GetLink(parameterInputs, parameter)))
					{
						CalculatedPathValue calculatedInput;
						PathConstant constantInput;
						if (null != (calculatedInput = parameterInput.InputFromCalculatedValue))
						{
							if (contextConditionNode == null)
							{
								PushSplit(pathContext, VerbalizationPlanBranchType.AndSplit, ref pendingRequiredVariableKeys);
								contextConditionNode = myCurrentBranchNode;
							}
							ProjectExternalVariable(inlineSubqueryParameters[inlineSubqueryParameterBaseIndex + i], calculatedInput, pathContext);
						}
						else if (null != (constantInput = parameterInput.InputFromConstant))
						{
							if (contextConditionNode == null)
							{
								PushSplit(pathContext, VerbalizationPlanBranchType.AndSplit, ref pendingRequiredVariableKeys);
								contextConditionNode = myCurrentBranchNode;
							}
							ProjectExternalVariable(inlineSubqueryParameters[inlineSubqueryParameterBaseIndex + i], constantInput, pathContext);
						}
					}
				}
			}
			InitializeRolePaths(
				pathContext,
				delegate(LeadRolePath subqueryPath)
				{
					if (innerPathContextForPath != subqueryPath)
					{
						innerPathContextForPath = subqueryPath;
						if (!subqueryContexts.TryGetValue(subqueryPath, out innerPathContext))
						{
							subqueryContexts[subqueryPath] = innerPathContext = new InlineSubqueryContext(pathContext, factTypeEntry, subqueryPath);
						}
					}
					return innerPathContext;
				},
				keyDecorator,
				subqueryRule,
				equivalentSubqueryProjectionKeysByPath,
				ref pendingRequiredVariableKeys);

			if (contextConditionNode != null)
			{
				PopSplit(VerbalizationPlanBranchType.AndSplit);
			}
		}
		/// <summary>
		/// Determine if a calculated value is based solely on local inputs.
		/// </summary>
		/// <param name="pathContext">The context for testing path nodes.</param>
		/// <param name="calculatedValue">The calculation to test.</param>
		/// <param name="isLocalPathNode">A callback to determine if a <see cref="PathedRole"/> is local.</param>
		/// <returns>true if all parts of the calculation depend on local path nodes, false if a non-local pathed
		/// role is used, and null if no pathed roles are used.</returns>
		private static bool? IsLocalCalculatedValue(object pathContext, CalculatedPathValue calculatedValue, Predicate<RolePathNode> isLocalPathNode)
		{
			bool seenLocalPathedNode = false;
			foreach (CalculatedPathValueInput input in calculatedValue.InputCollection)
			{
				PathedRole sourcePathedRole;
				RolePathObjectTypeRoot sourcePathRoot;
				CalculatedPathValue sourceCalculation;
				if (null != (sourcePathedRole = input.SourcePathedRole))
				{
					if (isLocalPathNode(new RolePathNode(sourcePathedRole, pathContext)))
					{
						seenLocalPathedNode = true;
					}
					else
					{
						return false;
					}
				}
				else if (null != (sourcePathRoot = input.SourcePathRoot))
				{
					if (isLocalPathNode(new RolePathNode(sourcePathRoot, pathContext)))
					{
						seenLocalPathedNode = true;
					}
					else
					{
						return false;
					}
				}
				else if (null != (sourceCalculation = input.SourceCalculatedValue))
				{
					// Recurse
					bool? recursiveResult = IsLocalCalculatedValue(pathContext, sourceCalculation, isLocalPathNode);
					if (recursiveResult.HasValue)
					{
						if (recursiveResult.Value)
						{
							seenLocalPathedNode = true;
						}
						else
						{
							return false;
						}
					}
				}

				if (input.Parameter.BagInput)
				{
					// Any aggregation context needs to be local along with other inputs
					foreach (RolePathObjectTypeRoot pathRoot in calculatedValue.AggregationContextPathRootCollection)
					{
						if (isLocalPathNode(new RolePathNode(pathRoot, pathContext)))
						{
							seenLocalPathedNode = true;
						}
						else
						{
							return false;
						}
					}
					foreach (PathedRole pathedRole in calculatedValue.AggregationContextPathedRoleCollection)
					{
						if (isLocalPathNode(new RolePathNode(pathedRole, pathContext)))
						{
							seenLocalPathedNode = true;
						}
						else
						{
							return false;
						}
					}
				}
			}
			return seenLocalPathedNode ? (bool?)true : null;
		}
		/// <summary>
		/// Pop a <see cref="FactType"/> added with <see cref="PushFactType"/>. Registers
		/// a use of each unused role to support correct variable subscripting.
		/// </summary>
		/// <param name="baseRolesStack">A stack of <see cref="RoleBase"/> collections.</param>
		/// <param name="existentiallyRegisterUnusedRoles">If set, then any role not marked as used will be registered as a pure existential use of the variable type.</param>
		/// <param name="usedRoles">A <see cref="BitTracker"/> with one bit for each role in each fact type on the <paramref name="baseRolesStack"/>.</param>
		/// <returns>The base index in the <paramref name="usedRoles"/> tracker corresponding
		/// to the first role in the new top of the <paramref name="baseRolesStack"/></returns>
		private int PopFactType(Stack<LinkedElementCollection<RoleBase>> baseRolesStack, bool existentiallyRegisterUnusedRoles, ref BitTracker usedRoles)
		{
			LinkedElementCollection<RoleBase> factTypeRoles = baseRolesStack.Pop();
			int roleCount = factTypeRoles.Count;
			int baseRoleIndex = usedRoles.Count - roleCount;
			if (existentiallyRegisterUnusedRoles)
			{
				for (int i = 0; i < roleCount; ++i)
				{
					ObjectType rolePlayer;
					if (!usedRoles[baseRoleIndex + i] &&
						null != (rolePlayer = factTypeRoles[i].Role.RolePlayer) &&
						!rolePlayer.IsImplicitBooleanValue)
					{
						RegisterRolePlayerUse(rolePlayer, null, null, RolePathNode.Empty);
					}
				}
			}
			usedRoles.Resize(baseRoleIndex);
			return baseRolesStack.Count == 0 ? -1 : baseRoleIndex - baseRolesStack.Peek().Count;
		}
		/// <summary>
		/// Push a split into the verbalization plan
		/// </summary>
		/// <param name="pathContext">The context representing the use of this path.</param>
		/// <param name="branchType">The type of branch.</param>
		/// <param name="pendingRequiredVariables">Required pending variables.</param>
		private void PushSplit(object pathContext, VerbalizationPlanBranchType branchType, ref LinkedNode<object> pendingRequiredVariables)
		{
			if (branchType != VerbalizationPlanBranchType.None)
			{
				VerbalizationPlanNode parentNode = myCurrentBranchNode;
				// Make sure that we have a permanent parent node
				if (parentNode == null)
				{
					parentNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref myRootPlanNode);
				}
				VerbalizationPlanNode newNode;
				myCurrentBranchNode = newNode = VerbalizationPlanNode.AddBranchNode(branchType, GetRenderingStyleFromBranchType(branchType), pathContext, parentNode, ref myRootPlanNode);
				if (pendingRequiredVariables != null)
				{
					newNode.RequireContextVariables(pendingRequiredVariables);
					pendingRequiredVariables = null;
				}
			}
		}
		/// <summary>
		/// Step to the parent of the current branch in the verbalization plan
		/// </summary>
		private void PopSplit(VerbalizationPlanBranchType branchType)
		{
			if (branchType != VerbalizationPlanBranchType.None)
			{
				VerbalizationPlanNode branchNode = myCurrentBranchNode;
				VerbalizationPlanNode newParentNode = branchNode.ParentNode;
				myCurrentBranchNode = newParentNode;

				// See if any collapsing is possible when we pop the branch off.
				// For now, we collapse if an 'and' or 'or' branch contains
				// branches of the same type. We may collapse other combinations
				// in the future.
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
		/// popped off the stack using <see cref="PopConditionalChainNode"/>
		/// </summary>
		private void PushConditionalChainNode(object pathContext)
		{
			VerbalizationPlanNode parentNode = myCurrentBranchNode;
			// Make sure that we have a permanent parent node
			if (parentNode == null)
			{
				parentNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref myRootPlanNode);
			}
			myCurrentBranchNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, parentNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Remove a node added with the <see cref="PushConditionalChainNode"/> and
		/// collapse the node if it is not needed.
		/// </summary>
		private void PopConditionalChainNode()
		{
			VerbalizationPlanNode chainNode = myCurrentBranchNode;
			object pathContext = chainNode.PathContext;
			Debug.Assert(chainNode.BranchType == VerbalizationPlanBranchType.Chain);
			VerbalizationPlanNode parentNode = chainNode.ParentNode;
			myCurrentBranchNode = parentNode;
			LinkedNode<VerbalizationPlanNode> headChildNode = chainNode.FirstChildNode;

			// Walk the child nodes and remove any supertype entry fact types with trailing
			// trailing chained information on the subtype role. If there is no trailing
			// information then this is a subtype assertion (supertype A must be a subtype B)
			// and cannot be collapased. The goal here is to verbalize an explicit crossing
			// of a subtype link the same as an implicit subtype crossing. So, if B->A and 'B r',
			// a path rooted at A and starting directly to the 'r' role should verbalize the
			// same as a path rooted at A, starting at the supertype role of the subtype instance
			// relationship, then joining the subtype role to the 'r' role.
			if (headChildNode != null)
			{
				LinkedNode<VerbalizationPlanNode> currentNode = headChildNode;
				LinkedNode<VerbalizationPlanNode> nextNode = currentNode.Next;
				while (nextNode != null)
				{
					VerbalizationPlanNode testPlanNode = currentNode.Value;
					PathedRole supertypePathedRole;
					if (testPlanNode.NodeType == VerbalizationPlanNodeType.FactType &&
						(supertypePathedRole = testPlanNode.FactTypeEntry).Role is SupertypeMetaRole)
					{
						PathedRole subtypePathedRole = null;
						VisitPathedRolesForFactTypeEntry(
							supertypePathedRole,
							delegate(PathedRole pathedRole)
							{
								if (pathedRole != supertypePathedRole)
								{
									// Conditions should always be true for a clean path,
									// provided as defensive sanity code.
									SubtypeMetaRole subtypeRole;
									if (null != (subtypeRole = pathedRole.Role as SubtypeMetaRole) &&
										subtypeRole.FactType == subtypeRole.FactType)
									{
										subtypePathedRole = pathedRole;
									}
									return false;
								}
								return true;
							});
						if (subtypePathedRole != null)
						{
							// We only do something here if we've actually defined a variable for
							// this pathed role. Otherwise, the pathed role is there but does nothing.
							Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
							RolePlayerVariableUse supertypeVariableUse;
							RolePlayerVariableUse subtypeVariableUse;
							if (useMap.TryGetValue(new RolePathNode(supertypePathedRole, pathContext), out supertypeVariableUse) &&
								useMap.TryGetValue(new RolePathNode(subtypePathedRole, pathContext), out subtypeVariableUse))
							{
								currentNode.Detach(ref headChildNode);
								// Note that the variables are already correlated, all we need to do is pull the path.
							}
						}
					}
					currentNode = nextNode;
					nextNode = nextNode.Next;
				}
				chainNode.FirstChildNode = headChildNode;
			}

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
		private void PushNegatedChainNode(object pathContext, PathedRole negatedEntryRole, ref LinkedNode<object> pendingRequiredVariableKeys)
		{
			// Make sure that we have a permanent parent node
			VerbalizationPlanNode parentNode = myCurrentBranchNode ?? VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.Chain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, null, ref myRootPlanNode);
			VerbalizationPlanNode newNode = VerbalizationPlanNode.AddBranchNode(VerbalizationPlanBranchType.NegatedChain, VerbalizationPlanBranchRenderingStyle.OperatorSeparated, pathContext, parentNode, ref myRootPlanNode);
			if (pendingRequiredVariableKeys != null)
			{
				newNode.RequireContextVariables(pendingRequiredVariableKeys);
				pendingRequiredVariableKeys = null;
			}
			if (negatedEntryRole != null)
			{
				newNode.RequireContextVariables(new LinkedNode<object>(new RolePathNode(negatedEntryRole, pathContext)));
			}
			myCurrentBranchNode = newNode;
		}
		/// <summary>
		/// Pop a negated chain node pushed with <see cref="PushNegatedChainNode"/>
		/// </summary>
		private void PopNegatedChainNode()
		{
			VerbalizationPlanNode negatedChainNode = myCurrentBranchNode;
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			VerbalizationPlanNode parentNode = negatedChainNode.ParentNode;
			if (null != (childNodeLink = negatedChainNode.FirstChildNode) &&
				childNodeLink.Next == null)
			{
				VerbalizationPlanNode childNode = childNodeLink.Value;
				VerbalizationPlanNodeType nodeType = childNode.NodeType;
				LinkedNode<object> transferRequiredContextKeys;
				switch (nodeType)
				{
					case VerbalizationPlanNodeType.VariableExistence:
						if (childNode.RequiredContextVariableUseKeys == null)
						{
							// Remove the wrapped existential negation in favor of a single negated statement
							childNode.NegateExistence = !childNode.NegateExistence;
							if (null != (transferRequiredContextKeys = negatedChainNode.RequiredContextVariableUseKeys))
							{
								childNode.RequireContextVariables(transferRequiredContextKeys);
							}
							parentNode.CollapseChildNode(negatedChainNode);
						}
						break;
					case VerbalizationPlanNodeType.Branch:
						if (childNode.BranchType == VerbalizationPlanBranchType.Chain)
						{
							// Collapse a chain node directly into a parent negated chain
							if (null != (transferRequiredContextKeys = childNode.RequiredContextVariableUseKeys))
							{
								negatedChainNode.RequireContextVariables(transferRequiredContextKeys);
							}
							negatedChainNode.CollapseChildNode(childNode);
						}
						break;
				}
			}
			myCurrentBranchNode = parentNode;
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
		/// Test if a <see cref="PathedRole"/> is referenced by something other
		/// than its inclusion in the role path.
		/// </summary>
		private bool IsPathedRoleReferencedOutsidePath(PathedRole pathedRole)
		{
			DomainModelInfo nativeModel = null;
			foreach (ElementLink link in DomainRoleInfo.GetAllElementLinks(pathedRole))
			{
				if (link.GetDomainRelationship().DomainModel == (nativeModel ?? (nativeModel = pathedRole.GetDomainClass().DomainModel)))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Used with <see cref="VisitRolePathParts"/> as the callback for walking a role path.
		/// Walks the path both forward and backwards, allowing for stack-based node analysis.
		/// </summary>
		/// <param name="pathContext">The context object for this path iteration. Used to provide a context for
		/// inline path expansion, especially for subquery invocation. The context is passed to the visitor and
		/// should be used to create all <see cref="RolePathNode"/> instances.</param>
		/// <param name="currentPathedRole">The current pathed role. Set unless <paramref name="currentPathRoot"/> is set or this is a notification of a split.</param>
		/// <param name="currentPathRoot">The current path root. Set unless <paramref name="currentPathedRole"/> is set or this is a notification of a split</param>
		/// <param name="currentPath">The current path. If <paramref name="currentPathedRole"/> and <paramref name="currentPathRoot"/> are not set, then this is
		/// either a split notification where the path provides the split settings.</param>
		/// <param name="currentPathedRoles">All pathed roles for the current path.</param>
		/// <param name="currentPathedRoleIndex">The index of <paramref name="currentPathedRole"/> in <paramref name="currentPathedRoles"/></param>
		/// <param name="contextPathNode">The <see cref="RolePathNode"/> prior to the <paramref name="currentPathedRole"/> or root <paramref name="currentPath"/></param>
		/// <param name="unwinding">The path is being walked in reverse.</param>
		private delegate void RolePathNodeVisitor(object pathContext, PathedRole currentPathedRole, RolePathObjectTypeRoot currentPathRoot, RolePath currentPath, ReadOnlyCollection<PathedRole> currentPathedRoles, int currentPathedRoleIndex, RolePathNode contextPathNode, bool unwinding);
		private void VisitRolePathParts(object pathContext, RolePath rolePath, RolePathNode contextPathNode, RolePathNodeVisitor visitor)
		{
			RolePathNode splitContext = contextPathNode;
			RolePathNode rootContext = splitContext;
			RolePathCache cache = EnsureRolePathCache();
			ReadOnlyCollection<PathedRole> pathedRoles = cache.PathedRoleCollection(rolePath);
			int pathedRoleCount = pathedRoles.Count;
			RolePathObjectTypeRoot pathRoot = cache.RootObjectTypeLink(rolePath);
			if (pathRoot != null)
			{
				rootContext = new RolePathNode(pathRoot, pathContext);
				visitor(pathContext, null, pathRoot, rolePath, pathedRoles, -1, splitContext, false);
				splitContext = rootContext;
			}
			for (int i = 0; i < pathedRoleCount; ++i)
			{
				PathedRole pathedRole = pathedRoles[i];
				visitor(pathContext, pathedRole, null, rolePath, pathedRoles, i, splitContext, false);
				splitContext = new RolePathNode(pathedRole, pathContext);
			}
			bool notifiedSplit = false;
			LinkedElementCollection<RoleSubPath> subPaths = cache.SubPathCollection(rolePath);
			foreach (RoleSubPath subPath in subPaths)
			{
				if (!notifiedSplit)
				{
					if (subPaths.Count != 1)
					{
						notifiedSplit = true;
						visitor(pathContext, null, null, rolePath, pathedRoles, -1, splitContext, false);
					}
				}
				VisitRolePathParts(pathContext, subPath, splitContext, visitor);
			}
			// Unwind the path
			if (notifiedSplit)
			{
				visitor(pathContext, null, null, rolePath, pathedRoles, -1, splitContext, true);
			}
			for (int i = pathedRoleCount - 1; i >= 0; --i)
			{
				visitor(pathContext, pathedRoles[i], null, rolePath, pathedRoles, i, i == 0 ? (pathRoot != null ? rootContext : contextPathNode) : new RolePathNode(pathedRoles[i - 1], pathContext), true);
			}
			if (pathRoot != null)
			{	
				visitor(pathContext, null, pathRoot, rolePath, pathedRoles, -1, contextPathNode, true);
			}
		}
		/// <summary>
		/// Associate path variables with an entry role.
		/// </summary>
		/// <param name="pathContext">The path context to record this node in.</param>
		/// <param name="pathedRole">The entry into a fact type.</param>
		/// <param name="joinSourcePathNode">The join source.</param>
		private void RegisterFactTypeEntryRolePlayerUse(object pathContext, PathedRole pathedRole, RolePathNode joinSourcePathNode)
		{
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			RolePlayerVariable joinToVariable = null;
			RolePlayerVariableUse joinSourceVariableUse;
			ObjectType targetRolePlayer = pathedRole.Role.RolePlayer;
			RolePathNode pathNode = new RolePathNode(pathedRole, pathContext);
			if (!joinSourcePathNode.IsEmpty)
			{
				if (useMap.TryGetValue(joinSourcePathNode, out joinSourceVariableUse))
				{
					joinToVariable = joinSourceVariableUse.PrimaryRolePlayerVariable;
				}
			}
			RegisterRolePlayerUse(targetRolePlayer, joinToVariable, pathNode, pathNode);
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
		/// <param name="correlateWithNode">The <see cref="RolePathNode"/> to correlate this variable
		/// with. The list of correlated variables is stored with the normalized root correlation
		/// variable. This may be the same instance as <paramref name="usedFor"/> and should not
		/// be pre-normalized before this call.</param>
		/// <returns>New or existing variable</returns>
		private RolePlayerVariable RegisterRolePlayerUse(ObjectType rolePlayer, RolePlayerVariable joinToVariable, object usedFor, RolePathNode correlateWithNode)
		{
			Dictionary<object, RolePlayerVariableUse> useMap = myUseToVariableMap;
			RolePlayerVariable existingVariable = null;
			bool addNewVariableToCorrelationRoot = false;
			RolePlayerVariableUse correlationRootVariableUse = default(RolePlayerVariableUse);
			object correlateWith = null;
			if (!correlateWithNode.IsEmpty &&
				null != (correlateWith = EnsureRolePathCache().GetCorrelationRoot(correlateWithNode)))
			{
				// Inline expansion of CorrelationRootToContextBoundKey allows us to use the correlation
				// components later
				PathedRole correlateWithPathedRole;
				RolePathObjectTypeRoot correlateWithPathRoot = null;
				PathObjectUnifier correlateWithObjectUnifier = null;
				object pathContext = correlateWithNode.Context;
				object correlateWithKey;
				if (null != (correlateWithPathedRole = correlateWith as PathedRole))
				{
					correlateWithKey = new RolePathNode(correlateWithPathedRole, pathContext);
				}
				else if (null != (correlateWithPathRoot = correlateWith as RolePathObjectTypeRoot))
				{
					correlateWithKey = new RolePathNode(correlateWithPathRoot, pathContext);
				}
				else
				{
					correlateWithKey = new ContextBoundUnifier(correlateWithObjectUnifier = (PathObjectUnifier)correlateWith, pathContext);
				}

				// Normalize the correlation target
				if (useMap.TryGetValue(correlateWithKey, out correlationRootVariableUse))
				{
					// Find an existing variable of the correct type in the correlation list
					bool seenCustom = false;
					foreach (RolePlayerVariable testMatchingVariable in correlationRootVariableUse.GetCorrelatedVariables(true))
					{
						if (testMatchingVariable.RolePlayer == rolePlayer)
						{
							existingVariable = testMatchingVariable;
							break;
						}
						if (!seenCustom && testMatchingVariable.IsCustomCorrelated)
						{
							seenCustom = true;
						}
					}

					// If we didn't find an existing variable, but one or more of the correlated variables
					// is custom, then look again through possible custom correlation lists for a variable of
					// the correct type.
					Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations;
					if (existingVariable == null &&
						seenCustom &&
						null != (customCorrelations = myCustomCorrelatedVariables))
					{
						// Repeat the exercise, looking deeper through correlated external lists for a variable match
						foreach (RolePlayerVariable testMatchingVariable in correlationRootVariableUse.GetCorrelatedVariables(true))
						{
							LinkedNode<RolePlayerVariable> customCorrelationHead;
							if (testMatchingVariable.IsCustomCorrelated &&
								customCorrelations.TryGetValue(testMatchingVariable, out customCorrelationHead))
							{
								foreach (RolePlayerVariable testMatchingVariable2 in customCorrelationHead)
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
					// We have to get some variable registered with the correlation root, so we
					// have to crack 'correlateWith' to get that role player. We'll either have
					// a PathedRole, a RolePathObjectTypeRoot, or an ObjectUnifier. For a unifier,
					// the choice is arbitrary, so we choose use the first path root if available,
					// and the first PathedRole otherwise.
					ObjectType correlationRootRolePlayer = null;
					if (null != correlateWithPathedRole)
					{
						correlationRootRolePlayer = correlateWithPathedRole.Role.RolePlayer;
					}
					else if (null != correlateWithPathRoot)
					{
						correlationRootRolePlayer = correlateWithPathRoot.RootObjectType;
					}
					else
					{
						LinkedElementCollection<RolePathObjectTypeRoot> unifiedRoots;
						LinkedElementCollection<PathedRole> unifiedPathedRoles;
						if (0 != (unifiedRoots = correlateWithObjectUnifier.PathRootCollection).Count)
						{
							correlationRootRolePlayer = unifiedRoots[0].RootObjectType;
						}
						else if (0 != (unifiedPathedRoles = correlateWithObjectUnifier.PathedRoleCollection).Count)
						{
							correlationRootRolePlayer = unifiedPathedRoles[0].Role.RolePlayer;
						}
					}

					// Note that if joinToVariable is set here, then it comes from an external
					// correlation. Otherwise, the join would have the same correlation root
					// as the current pathed role.
					RegisterRolePlayerUse(correlationRootRolePlayer, joinToVariable, correlateWithKey, RolePathNode.Empty);
					correlationRootVariableUse = useMap[correlateWithKey];
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
			bool joinedToCustomCorrelatedVariable = false;
			bool joinedToExternalVariable = false;
			if (existingVariable == null && joinToVariable != null)
			{
				Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations;
				LinkedNode<RolePlayerVariable> customCorrelationHead;
				joinedToCustomCorrelatedVariable = joinToVariable.IsCustomCorrelated;
				joinedToExternalVariable = joinToVariable.IsExternalVariable;
				if (joinToVariable.RolePlayer == rolePlayer)
				{
					existingVariable = joinToVariable;
				}
				else if (joinedToCustomCorrelatedVariable &&
					null != (customCorrelations = myCustomCorrelatedVariables) &&
					customCorrelations.TryGetValue(joinToVariable, out customCorrelationHead))
				{
					foreach (RolePlayerVariable testMatchingVariable in customCorrelationHead)
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
							if (joinToVariable != null && existingVariableUse.JoinedToVariable != joinToVariable)
							{
								existingVariableUse.JoinedToVariable = joinToVariable;
								if (usedFor.Equals(correlateWith))
								{
									// Anything in the join should be in the head list as well
									existingVariableUse.AddCorrelatedVariable(joinToVariable);
								}
								else if (correlateWith != null && correlationRootVariableUse.AddCorrelatedVariable(joinToVariable))
								{
									if (correlationRootVariableUse.AddCorrelatedVariable(joinToVariable))
									{
										useMap[correlateWith] = correlationRootVariableUse;
									}
								}
								useMap[usedFor] = existingVariableUse;
							}
						}
						else
						{
							useMap.Add(usedFor, new RolePlayerVariableUse(existingVariable, joinToVariable, usedFor.Equals(correlateWith) ? null : correlateWith));
						}
						if (addNewVariableToCorrelationRoot && correlationRootVariableUse.AddCorrelatedVariable(existingVariable))
						{
							// An external variable was found that is not in the local correlation list
							useMap[correlateWith] = correlationRootVariableUse;
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
					if (joinToVariable != null && existingVariableUse.JoinedToVariable != joinToVariable)
					{
						existingVariableUse.JoinedToVariable = joinToVariable;
						if (usedFor.Equals(correlateWith))
						{
							// Anything in the join should be in the head list as well
							existingVariableUse.AddCorrelatedVariable(joinToVariable);
						}
						else if (correlateWith != null && correlationRootVariableUse.AddCorrelatedVariable(joinToVariable))
						{
							if (correlationRootVariableUse.AddCorrelatedVariable(joinToVariable))
							{
								useMap[correlateWith] = correlationRootVariableUse;
							}
						}
						useMap[usedFor] = existingVariableUse;
					}
				}
				else
				{
					existingVariable = new RolePlayerVariable(rolePlayer);
					existingVariableUse = new RolePlayerVariableUse(existingVariable, joinToVariable, usedFor.Equals(correlateWith) ? null : correlateWith);
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
					useMap[correlateWith] = correlationRootVariableUse;
				}
				if (joinToVariable != null)
				{
					if (joinedToExternalVariable && !existingVariable.IsExternalVariable)
					{
						existingVariable.IsExternalVariable = true;
						CustomCorrelateVariables(joinToVariable, existingVariable);
					}
					else if (joinedToCustomCorrelatedVariable && !existingVariable.IsCustomCorrelated)
					{
						CustomCorrelateVariables(joinToVariable, existingVariable);
					}
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
		/// A chance for a subtype to add path projections using the
		/// <see cref="AddExternalVariable"/> method.
		/// </summary>
		/// <param name="pathOwner">The <see cref="RolePathOwner"/></param>
		/// <returns>Dictionary keyed by the lead role path contains sets of variable
		/// keys that should be combined into the path verbalization as equivalent nodes.</returns>
		protected virtual IDictionary<LeadRolePath, IList<IList<object>>> AddPathProjections(RolePathOwner pathOwner)
		{
			// Default implementation is empty
			return null;
		}
		/// <summary>
		/// A callback for decorating variable keys.
		/// </summary>
		/// <param name="key">The key value.</param>
		/// <returns>The original key or a decorated value.</returns>
		protected delegate object VariableKeyDecorator(object key);
		/// <summary>
		/// Add calculations and constants that are bound directly to a
		/// head variable registered during <see cref="AddPathProjections"/>
		/// using the <see cref="ProjectExternalVariable(Object,CalculatedPathValue,Object)"/> and
		/// <see cref="ProjectExternalVariable(Object,PathConstant,Object)"/> methods.
		/// </summary>
		/// <param name="pathContext">The context representing a use of this path.</param>
		/// <param name="pathOwner">The <see cref="RolePathOwner"/></param>
		/// <param name="rolePath">A <see cref="LeadRolePath"/> with associated projections.</param>
		/// <param name="keyDecorator">A callback function for decorating keys of projected items.</param>
		protected virtual void AddCalculatedAndConstantProjections(object pathContext, RolePathOwner pathOwner, LeadRolePath rolePath, VariableKeyDecorator keyDecorator)
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
		/// Determine an alternate key for variable rendering that corresponds
		/// to a variable with a resolved supertype appropriate associated with
		/// the role player key.
		/// </summary>
		/// <param name="rolePlayerFor">The requested role player key</param>
		/// <returns>An alternate key. The default implementation returns
		/// the same key.</returns>
		protected virtual object ResolveSupertypeKey(object rolePlayerFor)
		{
			return rolePlayerFor;
		}
		/// <summary>
		/// Override to add a filtering to establish which paths should be verbalized.
		/// Return <see langword="true"/> to allow verbalization of the path.
		/// </summary>
		/// <param name="pathOwner">The context <see cref="RolePathOwner"/> to test.</param>
		/// <param name="rolePath">The <see cref="LeadRolePath"/> to test for verbalization.</param>
		/// <remarks>All verbalizers may be requested to verbalize role projected derivation rules
		/// as part of subquery verbalizations. The default implementation checks for this type of rule
		/// and other implementations should check the base result to handle this case.</remarks>
		protected virtual bool VerbalizesPath(RolePathOwner pathOwner, LeadRolePath rolePath)
		{
			RoleProjectedDerivationRule derivationRule;
			return null != (derivationRule = pathOwner as RoleProjectedDerivationRule) &&
				null != RoleSetDerivationProjection.GetLink(derivationRule, rolePath);
		}
		/// <summary>
		/// Add a variable for use while verbalizing elements associated with the pathed role.
		/// </summary>
		/// <param name="headVariableKey">The required key for the head variable. Used later to retrieve information
		/// about this variable.</param>
		/// <param name="existingExternalVariable">A <see cref="RolePlayerVariable"/> returned from a previous
		/// call to this method for elements in another registered path.</param>
		/// <param name="rolePlayer">The <see cref="ObjectType"/> for the associated role player.</param>
		/// <param name="associatedPathNode">A <see cref="RolePathNode"/> that should be correlated with this variable.</param>
		/// <returns>New or existing <see cref="RolePlayerVariable"/></returns>
		protected RolePlayerVariable AddExternalVariable(object headVariableKey, RolePlayerVariable existingExternalVariable, ObjectType rolePlayer, RolePathNode associatedPathNode)
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
					CustomCorrelateVariables(existingExternalVariable, existingVariable);
				}
				else
				{
					existingExternalVariable = existingVariable;
				}
			}
			RolePlayerVariable retVal = RegisterRolePlayerUse(rolePlayer, existingExternalVariable, headVariableKey, associatedPathNode);
			retVal.IsExternalVariable = true;
			return retVal;
		}
		/// <summary>
		/// Correlate two variables related by a relationship other than an intra-path correlation. Note that the
		/// variables can have the same type, in which case the subscripts are coordinated and the pairings collapsed
		/// when both variables are used.
		/// </summary>
		private void CustomCorrelateVariables(RolePlayerVariable correlatedVariable1, RolePlayerVariable correlatedVariable2)
		{
			if (correlatedVariable1 == correlatedVariable2 || correlatedVariable1 == null || correlatedVariable2 == null)
			{
				return;
			}
			correlatedVariable1.IsCustomCorrelated = true;
			correlatedVariable2.IsCustomCorrelated = true;
			Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations = myCustomCorrelatedVariables;
			LinkedNode<RolePlayerVariable> existingListHeadNode1 = null;
			LinkedNode<RolePlayerVariable> existingListHeadNode2 = null;
			if (null == customCorrelations)
			{
				myCustomCorrelatedVariables = customCorrelations = new Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>>();
			}
			else
			{
				customCorrelations.TryGetValue(correlatedVariable1, out existingListHeadNode1);
				customCorrelations.TryGetValue(correlatedVariable2, out existingListHeadNode2);
			}
			if (existingListHeadNode1 == null)
			{
				if (existingListHeadNode2 == null)
				{
					// Create the new two item list and store the same list in both places.
					existingListHeadNode1 = new LinkedNode<RolePlayerVariable>(correlatedVariable1);
					existingListHeadNode1.SetNext(new LinkedNode<RolePlayerVariable>(correlatedVariable2), ref existingListHeadNode1);
					customCorrelations[correlatedVariable1] = existingListHeadNode1;
					customCorrelations[correlatedVariable2] = existingListHeadNode1;
				}
				else
				{
					// Just add to the tail. The head will not change here, so we do not need to
					// reset existing keys.
					existingListHeadNode2.GetTail().SetNext(new LinkedNode<RolePlayerVariable>(correlatedVariable1), ref existingListHeadNode2);
					customCorrelations[correlatedVariable1] = existingListHeadNode2;
				}
			}
			else if (existingListHeadNode2 == null)
			{
				// Just add to the tail. The head will not change here, so we do not need to
				// reset existing keys.
				existingListHeadNode1.GetTail().SetNext(new LinkedNode<RolePlayerVariable>(correlatedVariable2), ref existingListHeadNode1);
				customCorrelations[correlatedVariable2] = existingListHeadNode1;
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
					customCorrelations[testVariable] = existingListHeadNode1;
				}
				if (newHead != null)
				{
					// Attach unique merged variables to the tail to complete the process
					existingListHeadNode1.GetTail().SetNext(newHead, ref existingListHeadNode1);
				}
			}
		}
		/// <summary>
		/// Add a projection to a calculation based on variables used
		/// in the path. Called during <see cref="AddCalculatedAndConstantProjections"/> after
		/// all other variables in the path have been declared.
		/// </summary>
		/// <param name="headVariableKey">The projection key used in <see cref="AddExternalVariable"/>.</param>
		/// <param name="calculation">A <see cref="CalculatedPathValue"/> projected onto this head variable.</param>
		/// <param name="pathContext">The path context for this projection.</param>
		protected void ProjectExternalVariable(object headVariableKey, CalculatedPathValue calculation, object pathContext)
		{
			VerbalizationPlanNode.AddProjectedCalculationNode(headVariableKey, calculation, pathContext, myCurrentBranchNode, ref myRootPlanNode);
		}
		/// <summary>
		/// Add a projection to a constant based on variables used
		/// in the path. Called during <see cref="AddCalculatedAndConstantProjections"/> after
		/// all other variables in the path have been declared.
		/// </summary>
		/// <param name="headVariableKey">The projection key used in <see cref="AddExternalVariable"/>.</param>
		/// <param name="pathConstant">A <see cref="PathConstant"/> projected onto this head variable.</param>
		/// <param name="pathContext">The path context for this projection.</param>
		protected void ProjectExternalVariable(object headVariableKey, PathConstant pathConstant, object pathContext)
		{
			VerbalizationPlanNode.AddProjectedConstantNode(headVariableKey, pathConstant, pathContext, myCurrentBranchNode, ref myRootPlanNode);
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
		/// Begin a verbalization of the path(s) and associated head statement
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
			bool blockNextLeadRoleCollapse = false;
			return RenderVerbalizationPlanNode(builder, planNode, null, ref blockNextLeadRoleCollapse, out outdentPosition);
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
			if (0 != (renderingOptions & RolePathRolePlayerRenderingOptions.ResolveSupertype))
			{
				rolePlayerFor = ResolveSupertypeKey(rolePlayerFor);
			}
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
				Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations;
				LinkedNode<RolePlayerVariable> customCorrelationNode;

				// Look for partnering with another used external variable
				if (null != (customCorrelations = myCustomCorrelatedVariables) &&
					customCorrelations.TryGetValue(variable, out customCorrelationNode))
				{
					// If we haven't already been paired with a custom correlation
					// then pair up now.
					RolePlayerVariable partnerWithVariable = GetUnpairedPartnerVariable(variable, firstUse, customCorrelationNode);
					while (partnerWithVariable != null)
					{
						GetPartneredSubscriptedRolePlayerName(variable, ref partnerWithVariable, false);
						if (partnerWithVariable != null)
						{
							retVal = PartnerVariables(variable, null, partnerWithVariable, null, false);
							break;
						}
						else
						{
							// The first partner collapsed, try another.
							partnerWithVariable = GetUnpairedPartnerVariable(variable, false, customCorrelationNode); // false=Check existing pairings
						}
					}
				}
				if (retVal == null)
				{
					retVal = GetSubscriptedRolePlayerName(variable);
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
		/// <summary>
		/// Get a list of role path nodes containing all join nodes that occur at or
		/// before the projected nodes (roles or roots) of a path, and all nodes at or
		/// above nodes correlated with these variables. The return nodes correspond
		/// to the first occur use of the project nodes (or correlated nodes) in the path
		/// based on a depth-first iteration. Currently supported only for set constraint
		/// role paths.
		/// </summary>
		/// <param name="projectionKeys">The keys used to .</param>
		/// <returns>List of nodes, or <see langword="null"/>.</returns>
		public IList<object> GetPreProjectionPrimaryNodeKeys<KeyType>(IEnumerable<KeyType> projectionKeys) where KeyType : class
		{
			if (projectionKeys != null)
			{
				List<RolePlayerVariableUse> variableUses = new List<RolePlayerVariableUse>();
				// Key the correlation root to the project key
				Dictionary<object, object> normalizedProjectionKeys = new Dictionary<object, object>();
				// Keyed to itself, includes correlation roots and anything in the head above
				Dictionary<object, object> correlatedNodes = new Dictionary<object,object>();
				// Value is true if processed, false if already seen but processing pending.
				Dictionary<RolePathNode, bool> processedNodes = new Dictionary<RolePathNode, bool>();
				Queue<RolePathNode> toProcess = null;
				RolePathCache cache = EnsureRolePathCache();
				Action<RolePathNode> alsoProcess = delegate(RolePathNode node)
				{
					if (!processedNodes.ContainsKey(node))
					{
						processedNodes[node] = false;
						(toProcess ?? (toProcess = new Queue<RolePathNode>())).Enqueue(node);
					}
				};
				Action<RolePathNode> processNode = delegate(RolePathNode startNode)
				{
					bool alreadyProcessed;
					if (processedNodes.TryGetValue(startNode, out alreadyProcessed) && alreadyProcessed)
					{
						return;
					}
					processedNodes[startNode] = true;
					object resolvedNode;
					foreach (RolePathNode node in cache.GetPrecedingPathNodes(startNode, false))
					{
						if (processedNodes.TryGetValue(node, out alreadyProcessed) && alreadyProcessed)
						{
							return;
						}
						processedNodes[node] = true;
						PathedRole pathedRole;
						RolePathObjectTypeRoot pathRoot = null;
						if (null != (pathedRole = node.PathedRole))
						{
							if (pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
							{
								resolvedNode = cache.GetCorrelationRoot(node);
								correlatedNodes[resolvedNode] = resolvedNode;
							}
						}
						else
						{
							pathRoot = node.PathRoot;
							resolvedNode = cache.GetCorrelationRoot(node);
							correlatedNodes[resolvedNode] = resolvedNode;
						}

						PathObjectUnifier unifier = node.ObjectUnifier;
						if (unifier != null)
						{
							// We also need to walk up the path from unified nodes.
							// Add them to the stack of items to process.
							foreach (PathedRole unifiedPathedRole in unifier.PathedRoleCollection)
							{
								if (unifiedPathedRole != pathedRole)
								{
									alsoProcess(new RolePathNode(unifiedPathedRole));
								}
							}
							foreach (RolePathObjectTypeRoot unifiedPathRoot in unifier.PathRootCollection)
							{
								if (unifiedPathRoot != pathRoot)
								{
									alsoProcess(new RolePathNode(unifiedPathRoot));
								}
							}
						}
						// UNDONE: PENDING Subquery processing may need to look at parameter binding and roles in subqueries?
					}
				};
				foreach (KeyType key in projectionKeys)
				{
					RolePlayerVariableUse? testVariableUse = GetRolePlayerVariableUse(key);
					RolePlayerVariableUse variableUse;
					object correlationRoot;
					if (testVariableUse.HasValue &&
						null != (correlationRoot = (variableUse = testVariableUse.Value).CorrelationRoot))
					{
						normalizedProjectionKeys[correlationRoot] = key;
						PathObjectUnifier unifier;
						PathedRole pathedRole;
						RolePathObjectTypeRoot pathRoot;
						if (null != (pathedRole = correlationRoot as PathedRole))
						{
							processNode(new RolePathNode(pathedRole));
						}
						else if (null != (pathRoot = correlationRoot as RolePathObjectTypeRoot))
						{
							processNode(new RolePathNode(pathRoot));
						}
						else if (null != (unifier = correlationRoot as PathObjectUnifier))
						{
							foreach (PathedRole unifiedPathedRole in unifier.PathedRoleCollection)
							{
								processNode(new RolePathNode(unifiedPathedRole));
							}
							foreach (RolePathObjectTypeRoot unifiedPathRoot in unifier.PathRootCollection)
							{
								processNode(new RolePathNode(unifiedPathRoot));
							}
						}
					}
				}
				if (toProcess != null)
				{
					while (toProcess.Count != 0)
					{
						RolePathNode nextNode = toProcess.Dequeue();
						if (!processedNodes[nextNode])
						{
							processNode(nextNode);
						}
					}
				}
				if (correlatedNodes.Count != 0)
				{
					// Make sure we didn't pick up any other projected nodes
					foreach (object key in normalizedProjectionKeys.Keys)
					{
						if (correlatedNodes.ContainsKey(key))
						{
							correlatedNodes.Remove(key);
						}
					}
					int nodeCount = correlatedNodes.Count;
					if (nodeCount != 0)
					{
						// The nodes are randomly ordered. Walk the path from the top to
						// get a natural order.
						object[] retVal = new object[nodeCount];
						int nextIndex = 0;
						Predicate<RolePath> pathWalker = null;
						pathWalker = delegate(RolePath path)
						{
							RolePathObjectTypeRoot pathRoot = path.PathRoot;
							object testValue;
							object key;
							if (pathRoot != null)
							{
								key = (object)pathRoot.ObjectUnifier ?? pathRoot;
								if (correlatedNodes.TryGetValue(key, out testValue) && testValue != null)
								{
									correlatedNodes[key] = null;
									retVal[nextIndex] = pathRoot;
									if (++nextIndex == nodeCount)
									{
										return false; // Stop, we're done.
									}
								}
							}
							ReadOnlyCollection<PathedRole> pathedRoles = path.PathedRoleCollection;
							int pathedRoleCount = pathedRoles.Count;
							for (int i = 0; i < pathedRoleCount; ++i)
							{
								PathedRole pathedRole = pathedRoles[i];
								if (pathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType) // Correlation roots are never join nodes
								{
									key = (object)pathedRole.ObjectUnifier ?? pathedRole;
									if (correlatedNodes.TryGetValue(key, out testValue) && testValue != null)
									{
										correlatedNodes[key] = null;
										retVal[nextIndex] = pathedRole;
										if (++nextIndex == nodeCount)
										{
											return false; // Stop, we're done.
										}
									}
								}
							}
							foreach (RolePath subPath in path.SubPathCollection)
							{
								if (!pathWalker(subPath))
								{
									return false;
								}
							}
							return true;
						};
						RolePathOwner singleOwner;
						Dictionary<RolePathOwner, VerbalizationPlanNode> ownerMap;
						if (null != (singleOwner = mySingleRolePathOwner))
						{
							foreach (RolePath path in singleOwner.LeadRolePathCollection)
							{
								if (!pathWalker(path))
								{
									return retVal;
								}
							}
						}
						else if (null != (ownerMap = myPathOwnerToVerbalizationPlanMap) &&
							ownerMap.Count != 0)
						{
							foreach (RolePathOwner owner in ownerMap.Keys)
							{
								foreach (RolePath path in owner.LeadRolePathCollection)
								{
									if (!pathWalker(path))
									{
										return retVal;
									}
								}
							}
						}
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Determine if a rendering of the provided variable leads the verbalization.
		/// </summary>
		/// <remarks>Always returns false if there are multiple available verbalization plans.</remarks>
		/// <param name="pathOwner">The role path owner</param>
		/// <param name="variableKey">A key associated with a variable.</param>
		/// <returns><see langword="true"/> if this variable leads.</returns>
		public bool KeyedVariableLeadsVerbalization(RolePathOwner pathOwner, object variableKey)
		{
			VerbalizationPlanNode node;
			RolePlayerVariableUse variableUse;
			Dictionary<object, RolePlayerVariableUse> useMap;
			if (pathOwner == mySingleRolePathOwner &&
				null != (node = myRootPlanNode) &&
				(useMap = myUseToVariableMap).TryGetValue(variableKey, out variableUse))
			{
				RolePlayerVariable leadVariable = variableUse.PrimaryRolePlayerVariable;
				while (node != null)
				{
					VerbalizationPlanNodeType nodeType = node.NodeType;
					if (nodeType == VerbalizationPlanNodeType.Branch)
					{
						LinkedNode<VerbalizationPlanNode> nodeLink;
						LinkedNode<object> contextKey;
						if (node.BranchRenderingStyle != VerbalizationPlanBranchRenderingStyle.OperatorSeparated ||
							(null != (contextKey = node.RequiredContextVariableUseKeys) && (contextKey.Next != null || useMap[contextKey.Value].PrimaryRolePlayerVariable != leadVariable)) ||
							null == (nodeLink = node.FirstChildNode))
						{
							break;
						}
						node = nodeLink.Value;
					}
					else if (nodeType == VerbalizationPlanNodeType.FactType)
					{
						PathedRole entryRole;
						if (0 != (node.ReadingOptions & VerbalizationPlanReadingOptions.BasicLeadRole) &&
							(entryRole = node.FactTypeEntry).Role == node.Reading.RoleCollection[0].Role &&
							useMap.TryGetValue(entryRole, out variableUse) &&
							variableUse.PrimaryRolePlayerVariable == leadVariable)
						{
							return true;
						}
						break;
					}
					else
					{
						break;
					}
				}
			}
			return false;
		}
		private string RenderVerbalizationPlanNode(StringBuilder builder, VerbalizationPlanNode node, LinkedNode<VerbalizationPlanNode> nodeLink, ref bool blockNextLeadRoleCollapse, out int outdentPosition)
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
			object variableUseKey;
			RolePlayerVariableUse variableUse;
			object pathContext;
			IRolePathRenderer renderer = myRenderer;
			switch (node.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					factType = node.FactType;
					entryPathedRole = node.FactTypeEntry;
					pathContext = node.PathContext;
					reading = node.Reading;
					factRoles = reading.RoleCollection;
					factRoleCount = factRoles.Count;
					readingOptions = node.ReadingOptions;
					PathedRole[] pathedRoles = new PathedRole[factRoleCount];
					int replacedRoleCount = 0;
					string predicatePartDecorator = renderer.GetPredicatePartDecorator(factType);
					VerbalizationHyphenBinder hyphenBinder = new VerbalizationHyphenBinder(reading, renderer.FormatProvider, factRoles, null, renderer.GetSnippet(CoreVerbalizationSnippetType.HyphenBoundPredicatePart), predicatePartDecorator);
					bool negateExitRole = 0 != (readingOptions & VerbalizationPlanReadingOptions.NegatedExitRole);
					int negatedExitRoleIndex = -1;

					// Iterate and cache the pathed roles in one pass through the
					// fact type use in the path. Delay calling QuantifyVariableUse
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
							RolePlayerVariableUse? pathedRoleVariableUse = GetRolePlayerVariableUse(new RolePathNode(pathedRole, pathContext));
							if (pathedRoleVariableUse.HasValue)
							{
								if (i == 0)
								{
									if (VerbalizationPlanReadingOptions.BackReferenceFirstRole == (readingOptions & (VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.BlockBackReferenceFirstRole)))
									{
										RolePlayerVariable backReferenceVariable = node.CorrelateWithBackReferencedVariable;
										ObjectType rolePlayer;
										if (backReferenceVariable != null)
										{
											// Inline a significantly simplified version of QuantifyRolePlayerName. This assumes that
											// a back referenced variable is already sufficiently paired with other variables that
											// no additional external pairing work needs to be done. There are three possible states at this point:
											// 1) The primary variable has not been used (corollary: the variables have not been paired)
											// 2) The primary variable has been used, but not paired with the back referenced variable.
											// 3) The variables have already been paired.
											// The first two cases are the same apart from different quantifiers. The third case is the
											// same as the same variable case except that the IsPersonal setting is pulled from the back
											// referenced variable, not the current role.

											// We'll use the lead (im)personal pronoun regardless, get it first
											rolePlayer = backReferenceVariable.RolePlayer;
											replacement = renderer.GetSnippet(rolePlayer != null && rolePlayer.TreatAsPersonal ? CoreVerbalizationSnippetType.PersonalPronoun : CoreVerbalizationSnippetType.ImpersonalPronoun);

											// Figure out if we need to the variables as well
											Dictionary<CorrelatedVariablePairing, int> pairings = myCorrelatedVariablePairing;
											RolePlayerVariable primaryVariable = pathedRoleVariableUse.Value.PrimaryRolePlayerVariable;
											CorrelatedVariablePairing pairing = new CorrelatedVariablePairing(primaryVariable, backReferenceVariable);
											int pairedDuringPhase;
											if (pairings == null ||
												!pairings.TryGetValue(pairing, out pairedDuringPhase) ||
												!IsPairingUsePhaseInScope(pairedDuringPhase))
											{
												rolePlayer = pathedRole.Role.RolePlayer;
												replacement = string.Format(
													renderer.FormatProvider,
													renderer.GetSnippet(rolePlayer != null && rolePlayer.TreatAsPersonal ? CoreVerbalizationSnippetType.PersonalLeadIdentityCorrelation : CoreVerbalizationSnippetType.ImpersonalLeadIdentityCorrelation),
													QuantifyRolePlayerName(GetSubscriptedRolePlayerName(primaryVariable), primaryVariable.Use(CurrentQuantificationUsePhase, true), false),
													replacement);
												if (pairings == null)
												{
													myCorrelatedVariablePairing = pairings = new Dictionary<CorrelatedVariablePairing, int>();
												}
												pairings[pairing] = CurrentPairingUsePhase;
											}
										}
										else
										{
											rolePlayer = pathedRole.Role.RolePlayer;
											replacement = renderer.GetSnippet(rolePlayer != null && rolePlayer.TreatAsPersonal ? CoreVerbalizationSnippetType.PersonalPronoun : CoreVerbalizationSnippetType.ImpersonalPronoun);
										}
									}
									else if (VerbalizationPlanReadingOptions.FullyCollapseFirstRole == (readingOptions & (VerbalizationPlanReadingOptions.FullyCollapseFirstRole | VerbalizationPlanReadingOptions.BlockFullyCollapseFirstRole)))
									{
										if (blockNextLeadRoleCollapse)
										{
											blockNextLeadRoleCollapse = false;
										}
										else
										{
											// Note that dynamic inline negation settings are resolve by the parent node, so
											// we do not need to check if NegatedExitRole is resolved at this point. However,
											// if we are not collapsed for inline negation, then we do need to check if the
											// non-inlined negation snippet supports collapsing. This is done much earlier for
											// other branch types.
											VerbalizationPlanNode parentNode;
											VerbalizationPlanReadingOptions inlineNegationOptions = readingOptions & (VerbalizationPlanReadingOptions.DynamicNegatedExitRole | VerbalizationPlanReadingOptions.NegatedExitRole);
											if (0 != inlineNegationOptions)
											{
												if (0 != (inlineNegationOptions & VerbalizationPlanReadingOptions.NegatedExitRole))
												{
													replacement = "";
												}
												else if (null != (parentNode = node.ParentNode) &&
													GetCollapsibleLeadAllowedFromBranchType(parentNode.BranchType))
												{
													replacement = "";
												}
											}
											else
											{
												replacement = "";
											}
										}
									}
								}
								if (replacement == null)
								{
									replacement = QuantifyVariableUse(pathedRoleVariableUse.Value, pathContext, i == negatedExitRoleIndex, i == 0 && 0 != (readingOptions & VerbalizationPlanReadingOptions.BasicLeadRole), hyphenBinder, i);
								}
							}
							else
							{
								replacement = QuantifyFullyExistentialRolePlayer(pathedRole.Role.RolePlayer, i == negatedExitRoleIndex, hyphenBinder, i);
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
					blockNextLeadRoleCollapse = false; // Rendering a fact type will always reuse or introduce a new context
					return hyphenBinder.PopulatePredicateText(reading, renderer.FormatProvider, predicatePartDecorator, factRoles, roleReplacements, false);
				case VerbalizationPlanNodeType.Branch:
					int restoreBuilder = builder.Length;
					VerbalizationPlanBranchType branchType = node.BranchType;
					VerbalizationPlanBranchRenderingStyle renderingStyle = node.BranchRenderingStyle;
					childNodeLink = node.FirstChildNode;
					bool first = true;
					bool isNestedBranch = false;
					bool isTailBranch = false;
					bool inlineNegatedChain = false;
					int nestedOutdent = -1;
					CoreVerbalizationSnippetType snippet;
					VerbalizationPlanBranchType childBranchType;
					VerbalizationPlanNodeType previousChildNodeType = (VerbalizationPlanNodeType)(-1);
					VerbalizationPlanNode previousChildNode = null;
					while (childNodeLink != null)
					{
						snippet = (CoreVerbalizationSnippetType)(-1);
						childNode = childNodeLink.Value;
						switch (renderingStyle)
						{
							case VerbalizationPlanBranchRenderingStyle.HeaderList:
								if (!first)
								{
									PopPairingUsePhase();
								}
								PushPairingUsePhase();
								break;
							case VerbalizationPlanBranchRenderingStyle.IsolatedList:
								BeginQuantificationUsePhase();
								break;
						}
						string requiredVariableContextPhrase = null;
						LinkedNode<object> requiredVariableUseKeyLink = childNode.RequiredContextVariableUseKeys;
						if (requiredVariableUseKeyLink != null)
						{
							int usePhase = CurrentQuantificationUsePhase;
							while (requiredVariableUseKeyLink != null)
							{
								variableUse = GetRolePlayerVariableUse(variableUseKey = requiredVariableUseKeyLink.Value).Value;
								if (!IsCorrelatedInstanceDeclared(variableUse, PathContextFromVariableUseKey(variableUseKey)) &&
									!NodeStartsWithVariable(childNode, variableUse.PrimaryRolePlayerVariable))
								{
									childNode.RenderedRequiredContextVariable = true;
									LinkedNode<VerbalizationPlanNode> modifyNodeLink = childNode.FirstChildNode;
									if (modifyNodeLink != null)
									{
										LinkedNode<VerbalizationPlanNode> blockLeadRoleModificationNodeLink = modifyNodeLink;
										while (blockLeadRoleModificationNodeLink != null)
										{
											VerbalizationPlanNode blockLeadRoleModificationNode = blockLeadRoleModificationNodeLink.Value;
											switch (blockLeadRoleModificationNode.NodeType)
											{
												case VerbalizationPlanNodeType.Branch:
													blockLeadRoleModificationNodeLink = blockLeadRoleModificationNodeLink.Value.FirstChildNode;
													break;
												case VerbalizationPlanNodeType.FactType:
													readingOptions = blockLeadRoleModificationNode.ReadingOptions;
													if (0 != (readingOptions & (VerbalizationPlanReadingOptions.FullyCollapseFirstRole | VerbalizationPlanReadingOptions.BackReferenceFirstRole)))
													{
														blockLeadRoleModificationNode.ReadingOptions = readingOptions | VerbalizationPlanReadingOptions.BlockFullyCollapseFirstRole | VerbalizationPlanReadingOptions.BlockBackReferenceFirstRole;
													}
													blockLeadRoleModificationNodeLink = null;
													break;
												default:
													blockLeadRoleModificationNodeLink = null;
													break;
											}
										}

										// Also turn off lead role collapsing on a subsequent child node. Backreferencing is still valid.
										blockLeadRoleModificationNodeLink = modifyNodeLink.Next;
										while (blockLeadRoleModificationNodeLink != null)
										{
											VerbalizationPlanNode blockLeadRoleModificationNode = blockLeadRoleModificationNodeLink.Value;
											switch (blockLeadRoleModificationNode.NodeType)
											{
												case VerbalizationPlanNodeType.Branch:
													blockLeadRoleModificationNodeLink = blockLeadRoleModificationNodeLink.Value.FirstChildNode;
													break;
												case VerbalizationPlanNodeType.FactType:
													readingOptions = blockLeadRoleModificationNode.ReadingOptions;
													if (0 != (readingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole))
													{
														blockLeadRoleModificationNode.ReadingOptions = readingOptions | VerbalizationPlanReadingOptions.BlockFullyCollapseFirstRole;
													}
													blockLeadRoleModificationNodeLink = null;
													break;
												default:
													blockLeadRoleModificationNodeLink = null;
													break;
											}
										}
									}
									requiredVariableContextPhrase = QuantifyVariableUse(variableUse, node.PathContext, false, false, default(VerbalizationHyphenBinder), -1);
									requiredVariableUseKeyLink = requiredVariableUseKeyLink.Next;
									while (requiredVariableUseKeyLink != null)
									{
										variableUse = GetRolePlayerVariableUse(variableUseKey = requiredVariableUseKeyLink.Value).Value;
										object keyContext;
										if (!IsCorrelatedInstanceDeclared(variableUse, keyContext = PathContextFromVariableUseKey(variableUseKey)) &&
											!NodeStartsWithVariable(childNode, variableUse.PrimaryRolePlayerVariable))
										{
											requiredVariableContextPhrase = requiredVariableContextPhrase + renderer.GetSnippet(CoreVerbalizationSnippetType.VariableIntroductionSeparator) + QuantifyVariableUse(variableUse, keyContext, false, false, default(VerbalizationHyphenBinder), -1);
										}
										requiredVariableUseKeyLink = requiredVariableUseKeyLink.Next;
									}
									requiredVariableContextPhrase = string.Format(
										renderer.FormatProvider,
										renderer.GetSnippet(CoreVerbalizationSnippetType.VariableIntroductionClause),
										requiredVariableContextPhrase);
									break;
								}
								requiredVariableUseKeyLink = requiredVariableUseKeyLink.Next;
							}
						}
						if (first)
						{
							first = false;
							VerbalizationPlanNode parentNode = node.ParentNode;
							if (parentNode != null)
							{
								switch (parentNode.BranchType)
								{
									case VerbalizationPlanBranchType.Chain:
									case VerbalizationPlanBranchType.NegatedChain:
										if (node.RenderedRequiredContextVariable)
										{
											isNestedBranch = true;
										}
										else
										{
											isTailBranch = nodeLink.Previous != null;
										}
										break;
									default:
										isNestedBranch = true;
										break;
								}
							}
							bool testForBackReferenceOpen = true;
							switch (branchType)
							{
								case VerbalizationPlanBranchType.Chain:
									snippet = CoreVerbalizationSnippetType.ChainedListOpen;
									testForBackReferenceOpen = false;
									break;
								case VerbalizationPlanBranchType.AndSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.AndTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.AndNestedListOpen : CoreVerbalizationSnippetType.AndLeadListOpen);
									break;
								case VerbalizationPlanBranchType.OrSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.OrTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.OrNestedListOpen : CoreVerbalizationSnippetType.OrLeadListOpen);
									break;
								case VerbalizationPlanBranchType.XorSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.XorTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.XorNestedListOpen : CoreVerbalizationSnippetType.XorLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedAndSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedAndTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedAndNestedListOpen : CoreVerbalizationSnippetType.NegatedAndLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedOrSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedOrTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedOrNestedListOpen : CoreVerbalizationSnippetType.NegatedOrLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedXorSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedXorTailListOpen : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedXorNestedListOpen : CoreVerbalizationSnippetType.NegatedXorLeadListOpen);
									break;
								case VerbalizationPlanBranchType.NegatedChain:
									// Check for inline negation
									readingOptions = ResolveDynamicNegatedExitRole(childNode);
									if (0 == (readingOptions & VerbalizationPlanReadingOptions.NegatedExitRole))
									{
										snippet = CoreVerbalizationSnippetType.NegatedChainedListOpen;
									}
									else
									{
										inlineNegatedChain = true;
										snippet = CoreVerbalizationSnippetType.ChainedListOpen;
									}
									testForBackReferenceOpen = false;
									break;
							}
							if (testForBackReferenceOpen &&
								snippet != (CoreVerbalizationSnippetType)(-1) &&
								(isTailBranch || isNestedBranch) &&
								LeadTextIsBackReference(childNode))
							{
								snippet = (CoreVerbalizationSnippetType)((int)snippet + 1);
							}
							nestedOutdent = -1; // Ignore previous nested outdents for lead snippets
						}
						else
						{
							switch (GetLeadContextChange(previousChildNode, true))
							{
								case LeadContextChange.InitialChange:
									blockNextLeadRoleCollapse = false;
									break;
								case LeadContextChange.Cleared:
								case LeadContextChange.NestedChange:
									blockNextLeadRoleCollapse = true;
									break;
							}
							switch (branchType)
							{
								case VerbalizationPlanBranchType.Chain:
								case VerbalizationPlanBranchType.NegatedChain:
									if (VerbalizationPlanReadingOptions.BackReferenceFirstRole == ((readingOptions = childNode.ReadingOptions) & (VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.BlockBackReferenceFirstRole)))
									{
										snippet = CoreVerbalizationSnippetType.ChainedListCollapsedSeparator;
									}
									// Check for a 'TailList', which will render its own lead separator in place of the chain separator.
									else if (!(!childNode.RenderedRequiredContextVariable &&
										childNode.NodeType == VerbalizationPlanNodeType.Branch &&
										BranchSplits(childBranchType = childNode.BranchType) &&
										GetRenderingStyleFromBranchType(childBranchType) == VerbalizationPlanBranchRenderingStyle.OperatorSeparated))
									{
										snippet = (0 != (readingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole)) ?
											((isTailBranch || isNestedBranch) ? CoreVerbalizationSnippetType.ChainedListComplexRestrictionCollapsedLeadSeparator : CoreVerbalizationSnippetType.ChainedListTopLevelComplexRestrictionCollapsedLeadSeparator) :
											((childNode.RestrictsPreviousFactType ||
											previousChildNodeType == VerbalizationPlanNodeType.ChainedRootVariable) ?
												(LeadTextIsBackReference(childNode) ? CoreVerbalizationSnippetType.ChainedListLocalRestrictionBackReferenceSeparator : CoreVerbalizationSnippetType.ChainedListLocalRestrictionSeparator) :
												((isTailBranch || isNestedBranch) ?
													(LeadTextIsBackReference(childNode) ? CoreVerbalizationSnippetType.ChainedListComplexRestrictionBackReferenceSeparator : CoreVerbalizationSnippetType.ChainedListComplexRestrictionSeparator) :
													(LeadTextIsBackReference(childNode) ? CoreVerbalizationSnippetType.ChainedListTopLevelComplexRestrictionBackReferenceSeparator : CoreVerbalizationSnippetType.ChainedListTopLevelComplexRestrictionSeparator)));
									}
									break;
								case VerbalizationPlanBranchType.AndSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.AndTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.AndNestedListSeparator : CoreVerbalizationSnippetType.AndLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.OrSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.OrTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.OrNestedListSeparator : CoreVerbalizationSnippetType.OrLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.XorSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.XorTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.XorNestedListSeparator : CoreVerbalizationSnippetType.XorLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedAndSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedAndTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedAndNestedListSeparator : CoreVerbalizationSnippetType.NegatedAndLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedOrSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedOrTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedOrNestedListSeparator : CoreVerbalizationSnippetType.NegatedOrLeadListSeparator);
									break;
								case VerbalizationPlanBranchType.NegatedXorSplit:
									snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedXorTailListSeparator : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedXorNestedListSeparator : CoreVerbalizationSnippetType.NegatedXorLeadListSeparator);
									break;
							}
						}
						string separatorText;
						if (snippet != (CoreVerbalizationSnippetType)(-1) &&
							!string.IsNullOrEmpty(separatorText = renderer.GetSnippet(snippet)))
						{
							if (nestedOutdent != -1 &&
								IsOutdentAwareSnippet(snippet) &&
								(nestedOutdent + restoreBuilder) < builder.Length)
							{
								builder.Insert(nestedOutdent + restoreBuilder, separatorText);
								nestedOutdent += separatorText.Length;
							}
							else
							{
								builder.Append(separatorText);
							}
						}
						if (requiredVariableContextPhrase != null)
						{
							builder.Append(requiredVariableContextPhrase);
						}
						nestedOutdent = -1;
						string childText = RenderVerbalizationPlanNode(builder, childNode, childNodeLink, ref blockNextLeadRoleCollapse, out nestedOutdent);
						if (!string.IsNullOrEmpty(childText))
						{
							if (nestedOutdent != -1)
							{
								nestedOutdent += builder.Length - restoreBuilder;
							}
							builder.Append(childText);
						}
						previousChildNode = childNode;
						previousChildNodeType = childNode.NodeType;
						childNodeLink = childNodeLink.Next;
					}
					if (!first)
					{
						snippet = (CoreVerbalizationSnippetType)(-1);
						switch (branchType)
						{
							case VerbalizationPlanBranchType.Chain:
								snippet = CoreVerbalizationSnippetType.ChainedListClose;
								break;
							case VerbalizationPlanBranchType.NegatedChain:
								snippet = inlineNegatedChain ?
									CoreVerbalizationSnippetType.ChainedListClose :
									CoreVerbalizationSnippetType.NegatedChainedListClose;
								break;
							case VerbalizationPlanBranchType.AndSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.AndTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.AndNestedListClose : CoreVerbalizationSnippetType.AndLeadListClose);
								break;
							case VerbalizationPlanBranchType.OrSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.OrTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.OrNestedListClose : CoreVerbalizationSnippetType.OrLeadListClose);
								break;
							case VerbalizationPlanBranchType.XorSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.XorTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.XorNestedListClose : CoreVerbalizationSnippetType.XorLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedAndSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedAndTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedAndNestedListClose : CoreVerbalizationSnippetType.NegatedAndLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedOrSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedOrTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedOrNestedListClose : CoreVerbalizationSnippetType.NegatedOrLeadListClose);
								break;
							case VerbalizationPlanBranchType.NegatedXorSplit:
								snippet = isTailBranch ? CoreVerbalizationSnippetType.NegatedXorTailListClose : (isNestedBranch ? CoreVerbalizationSnippetType.NegatedXorNestedListClose : CoreVerbalizationSnippetType.NegatedXorLeadListClose);
								break;
						}
						if (snippet != (CoreVerbalizationSnippetType)(-1))
						{
							string closeSnippet = renderer.GetSnippet(snippet);
							if (!string.IsNullOrEmpty(closeSnippet))
							{
								if (nestedOutdent != -1 &&
									IsOutdentAwareSnippet(snippet) &&
									(nestedOutdent + restoreBuilder) < builder.Length)
								{
									builder.Insert(nestedOutdent + restoreBuilder, closeSnippet);
									outdentPosition = IsOutdentSnippet(snippet) ?
										nestedOutdent : // Nested outdent snippets, the position does not move
										nestedOutdent + closeSnippet.Length; // Current node nested, outdent moves after it
								}
								else
								{
									if (IsOutdentSnippet(snippet))
									{
										outdentPosition = nestedOutdent != -1 ? nestedOutdent : (builder.Length - restoreBuilder);
									}
									builder.Append(closeSnippet);
								}
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
					if (nodeLink == null && // Outermost
						0 != (myOptions & RolePathVerbalizerOptions.MarkTrailingOutdentStart) && // Options require an outdent mark
						outdentPosition != -1 && // Have a trailing outdent with no visible text after it
						outdentPosition < (builder.Length - restoreBuilder)) // Outdent has some text after it
					{
						builder.Insert(restoreBuilder + outdentPosition, "{0}");
					}
					result = builder.ToString(restoreBuilder, builder.Length - restoreBuilder);
					builder.Length = restoreBuilder;
					return result;
				case VerbalizationPlanNodeType.CalculatedCondition:
					return renderer.RenderCalculation(node.Calculation, node.PathContext, this, builder);
				case VerbalizationPlanNodeType.ValueConstraint:
					return renderer.RenderValueCondition(node.ValueConstraint, node.PathContext, this, builder);
				case VerbalizationPlanNodeType.HeadCalculatedValueProjection:
					return string.Format(
						renderer.FormatProvider,
						renderer.GetSnippet(CoreVerbalizationSnippetType.HeadVariableProjection),
						RenderAssociatedRolePlayer(node.HeadVariableKey, null, RolePathRolePlayerRenderingOptions.None),
						renderer.RenderCalculation(node.Calculation, node.PathContext, this, builder));
				case VerbalizationPlanNodeType.HeadConstantProjection:
					return string.Format(
						renderer.FormatProvider,
						renderer.GetSnippet(CoreVerbalizationSnippetType.HeadVariableProjection),
						RenderAssociatedRolePlayer(node.HeadVariableKey, null, RolePathRolePlayerRenderingOptions.None),
						renderer.RenderConstant(node.Constant));
				case VerbalizationPlanNodeType.ChainedRootVariable:
					rootVariable = node.RootVariable;
					return QuantifyRolePlayerName(GetSubscriptedRolePlayerName(rootVariable), rootVariable.Use(CurrentQuantificationUsePhase, true), false);
				case VerbalizationPlanNodeType.VariableExistence:
					variableUse = GetRolePlayerVariableUse(node.VariableKey).Value;
					bool negateExistence = node.NegateExistence;
					return string.Format(
						renderer.FormatProvider,
						renderer.GetSnippet((negateExistence && variableUse.PrimaryRolePlayerVariable.HasBeenUsed(CurrentQuantificationUsePhase, true)) ? CoreVerbalizationSnippetType.NegatedVariableExistence : CoreVerbalizationSnippetType.VariableExistence),
						QuantifyVariableUse(variableUse, node.PathContext, negateExistence, false, default(VerbalizationHyphenBinder), -1));
				case VerbalizationPlanNodeType.VariableEquivalence:
					IList<object> equivalentKeys = node.EquivalentVariableKeys;
					return renderer.RenderVariableEquivalence(equivalentKeys, equivalentKeys is IVariableEquivalenceQuantified ? RolePathRolePlayerRenderingOptions.Quantify : RolePathRolePlayerRenderingOptions.None, equivalentKeys is IVariableEquivalenceIdentifiedBy ? CoreVerbalizationSnippetType.IsIdentifiedBy : (CoreVerbalizationSnippetType)(-1), node.PathContext, this, builder);
				case VerbalizationPlanNodeType.FloatingRootVariableContext:
					// There will always be exactly one child node link at this point
					childNodeLink = node.FirstChildNode;
					rootVariable = myFloatingRootVariable; // Push existing (should be null, but doesn't hurt)
					myFloatingRootVariable = node.RootVariable;
					result = RenderVerbalizationPlanNode(builder, childNodeLink.Value, childNodeLink, ref blockNextLeadRoleCollapse, out outdentPosition);
					myFloatingRootVariable = rootVariable;
					return result;
			}
			return null;
		}
		/// <summary>
		/// Test if the first non-formatting text for a node is a back reference construct.
		/// </summary>
		private bool LeadTextIsBackReference(VerbalizationPlanNode node)
		{
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			VerbalizationPlanBranchType branchType;
			if (node.RenderedRequiredContextVariable)
			{
				return false;
			}
			switch (branchType = node.BranchType)
			{
				case VerbalizationPlanBranchType.None:
					return VerbalizationPlanReadingOptions.BackReferenceFirstRole == (node.ReadingOptions & (VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.FullyCollapseFirstRole));
				case VerbalizationPlanBranchType.NegatedChain:
					if (null != (childNodeLink = node.FirstChildNode))
					{
						VerbalizationPlanReadingOptions nestedReadingOptions = ResolveDynamicNegatedExitRole(childNodeLink.Value);
						if ((VerbalizationPlanReadingOptions.NegatedExitRole | VerbalizationPlanReadingOptions.BackReferenceFirstRole) == (nestedReadingOptions & (VerbalizationPlanReadingOptions.NegatedExitRole | VerbalizationPlanReadingOptions.BackReferenceFirstRole | VerbalizationPlanReadingOptions.FullyCollapseFirstRole | VerbalizationPlanReadingOptions.BlockBackReferenceFirstRole)))
						{
							return true;
						}
						goto default;
					}
					break;
				default:
					if (GetCollapsibleListOpenForBackReferenceAllowedFromBranchType(branchType) &&
						null != (childNodeLink = node.FirstChildNode))
					{
						return LeadTextIsBackReference(childNodeLink.Value);
					}
					break;
			}
			return false;
		}
		/// <summary>
		/// Test if negation can be inline for the node. Negation is inlined
		/// if the fact type is binary and the non-entry role is either
		/// fully existential (not referenced) or has not yet been referenced.
		/// This should be called before rendering.
		/// </summary>
		private VerbalizationPlanReadingOptions ResolveDynamicNegatedExitRole(VerbalizationPlanNode node)
		{
			VerbalizationPlanReadingOptions readingOptions = node.ReadingOptions;
			if (0 == (readingOptions & VerbalizationPlanReadingOptions.DynamicNegatedExitRoleEvaluated))
			{
				readingOptions |= VerbalizationPlanReadingOptions.DynamicNegatedExitRoleEvaluated;
				bool inlineNegation = false;
				if (0 != (readingOptions & VerbalizationPlanReadingOptions.DynamicNegatedExitRole))
				{
					PathedRole entryPathedRole;
					if (null != (entryPathedRole = node.FactTypeEntry))
					{
						// The dynamic flag is set if there is a trailing pathed role on the
						// binary in the same role path. We can use the collapsed negated form
						// if the variable has not been introduced yet.
						ReadOnlyCollection<PathedRole> childPathedRoles = myRolePathCache.PathedRoleCollection(entryPathedRole.RolePath);
						int testChildIndex = childPathedRoles.IndexOf(entryPathedRole);
						testChildIndex = testChildIndex == 0 ? 1 : 0;
						int usePhase = CurrentQuantificationUsePhase;
						object pathContext = node.PathContext;
						RolePlayerVariableUse? resolvedChildVariableUse;
						if (testChildIndex >= childPathedRoles.Count || // Indicates a pure existential, we're here because the entry role can possibly be partnered.
							((resolvedChildVariableUse = GetRolePlayerVariableUse(new RolePathNode(childPathedRoles[testChildIndex], pathContext))).HasValue && !resolvedChildVariableUse.Value.PrimaryRolePlayerVariable.HasBeenUsed(usePhase, true)))
						{
							RolePlayerVariableUse variableUse = GetRolePlayerVariableUse(new RolePathNode(entryPathedRole, pathContext)).Value;
							RolePlayerVariable primaryVariable = variableUse.PrimaryRolePlayerVariable;
							object correlateWithKey = CorrelationRootToContextBoundKey(variableUse.CorrelationRoot, pathContext);
							if (null == GetUnpairedPartnerVariable(
								primaryVariable,
								!primaryVariable.HasBeenUsed(usePhase, true),
								correlateWithKey != null ? GetRolePlayerVariableUse(correlateWithKey).Value.GetCorrelatedVariables(true) : variableUse.GetCorrelatedVariables(false)))
							{
								inlineNegation = true;
								readingOptions |= VerbalizationPlanReadingOptions.NegatedExitRole;
							}
						}
					}
				}
				if (!inlineNegation)
				{
					if (0 != (readingOptions & VerbalizationPlanReadingOptions.FullyCollapseFirstRole) &&
						!GetCollapsibleLeadAllowedFromBranchType(VerbalizationPlanBranchType.NegatedChain))
					{
						readingOptions |= VerbalizationPlanReadingOptions.BlockFullyCollapseFirstRole;
					}
					if (0 != (readingOptions & VerbalizationPlanReadingOptions.BackReferenceFirstRole) &&
						!GetCollapsibleListOpenForBackReferenceAllowedFromBranchType(VerbalizationPlanBranchType.NegatedChain))
					{
						readingOptions |= VerbalizationPlanReadingOptions.BlockBackReferenceFirstRole;
					}
				}
				node.ReadingOptions = readingOptions;
			}
			return readingOptions;
		}
		private string QuantifyVariableUse(RolePlayerVariableUse variableUse, object pathContext, bool negateExistentialQuantifier, bool basicLeadRole, VerbalizationHyphenBinder hyphenBinder, int hyphenBinderRoleIndex)
		{
			RolePlayerVariable primaryVariable = variableUse.PrimaryRolePlayerVariable;
			int quantificationUsePhase = CurrentQuantificationUsePhase;
			bool firstUseOfPrimaryVariable = !primaryVariable.HasBeenUsed(quantificationUsePhase, false);

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
			IEnumerable<RolePlayerVariable> partnerCorrelatedVariables = null;
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
				object correlationRoot = variableUse.CorrelationRoot;
				if (correlationRoot == null)
				{
					if (variableUse.CorrelatedVariablesHead != null)
					{
						partnerCorrelatedVariables = variableUse.GetCorrelatedVariables(false); // No reason to test the primary variable
					}
				}
				else
				{
					partnerCorrelatedVariables = GetRolePlayerVariableUse(CorrelationRootToContextBoundKey(correlationRoot, pathContext)).Value.GetCorrelatedVariables(true);
				}
				if (partnerCorrelatedVariables != null)
				{
					partnerWithVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, partnerCorrelatedVariables);
				}
			}
			Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations = myCustomCorrelatedVariables;
			string preRenderedPartnerWith = null;
			if (customCorrelations != null)
			{
				RolePlayerVariable customCorrelatedPartnerVariable = null;
				LinkedNode<RolePlayerVariable> customCorrelationHead = null;
				if (primaryVariable.IsCustomCorrelated && customCorrelations.TryGetValue(primaryVariable, out customCorrelationHead))
				{
					customCorrelatedPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, customCorrelationHead);
				}
				if (partnerWithVariable == null)
				{
					partnerWithVariable = customCorrelatedPartnerVariable;
				}
				else
				{
					if (customCorrelatedPartnerVariable == partnerWithVariable)
					{
						customCorrelatedPartnerVariable = null;
					}
					LinkedNode<RolePlayerVariable> partnerCustomCorrelationHead;
					if (partnerWithVariable.IsCustomCorrelated && customCorrelations.TryGetValue(partnerWithVariable, out partnerCustomCorrelationHead))
					{
						RolePlayerVariable partnerCustomCorrelatedPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, firstUseOfPrimaryVariable, partnerCustomCorrelationHead);
						while (partnerCustomCorrelatedPartnerVariable != null &&
							partnerCustomCorrelatedPartnerVariable != primaryVariable &&
							partnerCustomCorrelatedPartnerVariable != partnerWithVariable &&
							partnerCustomCorrelatedPartnerVariable != customCorrelatedPartnerVariable)
						{
							if (customCorrelatedPartnerVariable != null)
							{
								GetPartneredSubscriptedRolePlayerName(customCorrelatedPartnerVariable, ref partnerCustomCorrelatedPartnerVariable, false);
								if (partnerCustomCorrelatedPartnerVariable != null)
								{
									preRenderedPartnerWith = PartnerVariables(customCorrelatedPartnerVariable, null, partnerCustomCorrelatedPartnerVariable, null, false);
									break;
								}
								else
								{
									// The first potential partner collapsed. See if there is another partner available.
									partnerCustomCorrelatedPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, false, partnerCustomCorrelationHead); // false=Check existing pairings
								}
							}
							else
							{
								customCorrelatedPartnerVariable = partnerCustomCorrelatedPartnerVariable;
								customCorrelationHead = partnerCustomCorrelationHead;
								break;
							}
						}
					}
					while (customCorrelatedPartnerVariable != null)
					{
						GetPartneredSubscriptedRolePlayerName(partnerWithVariable, ref customCorrelatedPartnerVariable, false);
						if (customCorrelatedPartnerVariable != null)
						{
							preRenderedPartnerWith = PartnerVariables(partnerWithVariable, null, customCorrelatedPartnerVariable, preRenderedPartnerWith, false);
							break;
						}
						else if (customCorrelationHead == null)
						{
							break;
						}
						else
						{
							// The first potential partner collapsed. See if there is another partner available.
							customCorrelatedPartnerVariable = GetUnpairedPartnerVariable(primaryVariable, false, customCorrelationHead); // false=Check existing pairings
						}
					}
				}
			}

			string result;
			if (partnerWithVariable != null)
			{
				result = null;
				while (partnerWithVariable != null)
				{
					result = GetPartneredSubscriptedRolePlayerName(primaryVariable, ref partnerWithVariable, true);
					if (partnerWithVariable != null)
					{
						if (basicLeadRole && !negateExistentialQuantifier && preRenderedPartnerWith == null)
						{
							// Use the optimized lead version of the identity correlation.
							return PartnerVariables(primaryVariable, QuantifyRolePlayerName(result, primaryVariable.Use(quantificationUsePhase, true), negateExistentialQuantifier), partnerWithVariable, preRenderedPartnerWith, true);
						}
						// Note that we never chain with the optimized lead form
						result = PartnerVariables(primaryVariable, result, partnerWithVariable, preRenderedPartnerWith, false);
						break;
					}
					else if (partnerCorrelatedVariables == null)
					{
						break;
					}
					else
					{
						partnerWithVariable = GetUnpairedPartnerVariable(primaryVariable, false, partnerCorrelatedVariables); // false=Check existing pairings
					}
				}
			}
			else
			{
				result = GetSubscriptedRolePlayerName(primaryVariable);
			}
			return QuantifyRolePlayerName(hyphenBinderRoleIndex >= 0 ? hyphenBinder.HyphenBindRoleReplacement(result, hyphenBinderRoleIndex) : result, primaryVariable.Use(quantificationUsePhase, true), negateExistentialQuantifier);
		}
		/// <summary>
		/// Test if two variables can be partnered, meaning that they represent different types
		/// of the same underlying instance.
		/// </summary>
		/// <param name="useKey">Key for the variable use to partner with</param>
		/// <param name="pathContext">The path context for this key.</param>
		/// <param name="variable">The variable to test for available partnership.</param>
		/// <param name="exactMatch">The primary variable for the <paramref name="useKey"/>
		/// is the <paramref name="variable"/>.</param>
		/// <returns><see langword="true"/> if the two variables represent the same instance.</returns>
		private bool CanPartnerWithVariable(object useKey, object pathContext, RolePlayerVariable variable, out bool exactMatch)
		{
			RolePlayerVariableUse? optionalVariableUse = GetRolePlayerVariableUse(useKey);
			if (optionalVariableUse.HasValue)
			{
				RolePlayerVariableUse variableUse = optionalVariableUse.Value;
				RolePlayerVariable primaryUseVariable = variableUse.PrimaryRolePlayerVariable;
				if (variable == primaryUseVariable)
				{
					exactMatch = true;
					return true;
				}
				exactMatch = false;
				if (variable == variableUse.JoinedToVariable)
				{
					return true;
				}
				object correlationRoot;
				if (variableUse.CorrelatedVariablesHead != null)
				{
					foreach (RolePlayerVariable testVariable in variableUse.GetCorrelatedVariables(false))
					{
						if (testVariable == variable)
						{
							return true;
						}
					}
				}
				else if (null != (correlationRoot = variableUse.CorrelationRoot))
				{
					foreach (RolePlayerVariable testVariable in GetRolePlayerVariableUse(CorrelationRootToContextBoundKey(correlationRoot, pathContext)).Value.GetCorrelatedVariables(true))
					{
						if (testVariable == variable)
						{
							return true;
						}
					}
				}
				Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations;
				LinkedNode<RolePlayerVariable> customlLinks;
				if (null != (customCorrelations = myCustomCorrelatedVariables) &&
					customCorrelations.TryGetValue(primaryUseVariable, out customlLinks))
				{
					foreach (RolePlayerVariable testVariable in customlLinks)
					{
						if (testVariable == variable)
						{
							return true;
						}
					}
				}
			}
			exactMatch = false;
			return false;
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
				RolePlayerVariable firstUnpairedSameTypedVariable = null; // With subquery process, we can end up pairing variables of the same type
				RolePlayerVariable firstUnpairedNormalVariable = null; // Normal meaning not the head
				RolePlayerVariable firstUnpairedHeadVariable = null;
				int pairedDuringPhase;
				RolePlayerVariable floatingRootVariable = myFloatingRootVariable;
				ObjectType primaryRolePlayer = primaryVariable.RolePlayer;
				bool unusedFloatingRootVariable = false;
				foreach (RolePlayerVariable possiblePartner in possibleCorrelationPartners)
				{
					if (possiblePartner == primaryVariable)
					{
						continue;
					}
					bool isHeadVariable = possiblePartner.IsHeadVariable;
					if (isHeadVariable || possiblePartner.HasBeenUsed(CurrentQuantificationUsePhase, false))
					{
						Dictionary<CorrelatedVariablePairing, int> pairings;
						if (!firstUseOfPrimaryVariable &&
							null != (pairings = myCorrelatedVariablePairing) &&
							pairings.TryGetValue(new CorrelatedVariablePairing(primaryVariable, possiblePartner), out pairedDuringPhase) &&
							IsPairingUsePhaseInScope(pairedDuringPhase))
						{
							// We have an existing pairing, get out
							firstUnpairedSameTypedVariable = null;
							firstUnpairedHeadVariable = null;
							firstUnpairedNormalVariable = null;
							break;
						}
						if (possiblePartner.RolePlayer == primaryRolePlayer)
						{
							if (firstUnpairedSameTypedVariable == null)
							{
								firstUnpairedSameTypedVariable = possiblePartner;
							}
						}
						else if (possiblePartner.IsHeadVariable)
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
				return firstUnpairedSameTypedVariable ?? firstUnpairedHeadVariable ?? firstUnpairedNormalVariable ?? (unusedFloatingRootVariable ? floatingRootVariable : null);
			}
			return null;
		}
		/// <summary>
		/// Check the types of two variables that need to be partnered and apply special handling if
		/// the variables have the same type, resulting in a single subscripted role player name.
		/// </summary>
		/// <param name="primaryVariable">The variable that needs to be partnered.</param>
		/// <param name="partnerWithVariable">The partner variable. Set to null if the types are the same and the
		/// two variables can be collapsed into a single name.</param>
		/// <param name="render">True to render the name. Otherwise, skips rendering but marks variables as used and reserves subscripts.</param>
		/// <returns>Subscripted role player name</returns>
		private string GetPartneredSubscriptedRolePlayerName(RolePlayerVariable primaryVariable, ref RolePlayerVariable partnerWithVariable, bool render)
		{
			string result = null;
			if (partnerWithVariable.RolePlayer == primaryVariable.RolePlayer)
			{
				// Skip partnering if possible. Pretend that the variables are the same variable by giving them
				// the same subscript.
				int partnerSubscript = partnerWithVariable.Subscript;
				int primarySubscript = primaryVariable.Subscript;
				if (primarySubscript == -1)
				{
					if (partnerSubscript != -1)
					{
						// Use the existing subscript
						primaryVariable.Subscript = primarySubscript = partnerSubscript;
						if (IsPairingUsePhaseInScope(partnerWithVariable.UsePhase))
						{
							primaryVariable.Use(CurrentQuantificationUsePhase, false);
						}
					}
					if (render)
					{
						result = GetSubscriptedRolePlayerName(primaryVariable);
					}
					else
					{
						ReserveSubscript(primaryVariable);
					}
					if (partnerSubscript == -1)
					{
						partnerWithVariable.Subscript = primarySubscript = partnerSubscript = primaryVariable.Subscript; // Primary subscript now reserved and set by GetSubscriptedRolePlayerName, read off new value
					}
				}
				else
				{
					if (render)
					{
						result = GetSubscriptedRolePlayerName(primaryVariable);
					}
					if (partnerSubscript == -1)
					{
						partnerWithVariable.Subscript = partnerSubscript = primarySubscript;
						if (IsPairingUsePhaseInScope(primaryVariable.UsePhase))
						{
							partnerWithVariable.Use(CurrentQuantificationUsePhase, false);
						}
					}
				}
				if (primarySubscript == partnerSubscript) // False for this is highly unlikely, but check to be safe. Produces paired variables of the same type, which are verbose but harmless.
				{
					(myCorrelatedVariablePairing ?? (myCorrelatedVariablePairing = new Dictionary<CorrelatedVariablePairing, int>()))[new CorrelatedVariablePairing(primaryVariable, partnerWithVariable)] = CurrentPairingUsePhase;
					partnerWithVariable = null;
				}
			}
			else if (render)
			{
				result = GetSubscriptedRolePlayerName(primaryVariable);
			}
			return result;
		}
		/// <summary>
		/// Check if the instance represented by <paramref name="variableUse"/>
		/// has been declared by testing if this or any other correlated variable
		/// has already been declared.
		/// </summary>
		/// <param name="variableUse">The variable to test</param>
		/// <param name="pathContext">The path context used to key this variable. Can be determined from the
		/// variable use key with <see cref="PathContextFromVariableUseKey"/></param>
		/// <returns>True if any variable corresponding to this variable has been declared.</returns>
		private bool IsCorrelatedInstanceDeclared(RolePlayerVariableUse variableUse, object pathContext)
		{
			int quantificationUsePhase = CurrentQuantificationUsePhase;
			RolePlayerVariable primaryVariable = variableUse.PrimaryRolePlayerVariable;
			if (primaryVariable.HasBeenUsed(quantificationUsePhase, true))
			{
				return true;
			}
			object correlationRootKey = CorrelationRootToContextBoundKey(variableUse.CorrelationRoot, pathContext);
			foreach (RolePlayerVariable correlatedVariable in (correlationRootKey != null) ? GetRolePlayerVariableUse(correlationRootKey).Value.GetCorrelatedVariables(true) : variableUse.GetCorrelatedVariables(false))
			{
				if (correlatedVariable != primaryVariable &&
					correlatedVariable.HasBeenUsed(quantificationUsePhase, true))
				{
					return true;
				}
			}
			Dictionary<RolePlayerVariable, LinkedNode<RolePlayerVariable>> customCorrelations;
			LinkedNode<RolePlayerVariable> customCorrelationNode;

			// Look for partnering with another used custom correlated variable
			if (null != (customCorrelations = myCustomCorrelatedVariables) &&
				customCorrelations.TryGetValue(primaryVariable, out customCorrelationNode))
			{
				// Treat a custom correlation as an instance use
				foreach (RolePlayerVariable correlatedVariable in customCorrelationNode)
				{
					if (correlatedVariable.HasBeenUsed(quantificationUsePhase, true))
					{
						return true;
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Get the path context from key structures that contain context information
		/// </summary>
		/// <param name="variableUseKey">The key to analyze.</param>
		/// <returns>A path context, or <see langword="null"/>.</returns>
		private static object PathContextFromVariableUseKey(object variableUseKey)
		{
			object pathContext = null;
			if (variableUseKey is RolePathNode)
			{
				pathContext = ((RolePathNode)variableUseKey).Context;
			}
			else if (variableUseKey is ContextBoundUnifier)
			{
				pathContext = ((ContextBoundUnifier)variableUseKey).Context;
			}
			else if (variableUseKey is InlineSubqueryRole)
			{
				pathContext = ((InlineSubqueryRole)variableUseKey).ParentContext;
			}
			return pathContext;
		}
		private static object CorrelationRootToContextBoundKey(object correlationRoot, object pathContext)
		{
			object key = null;
			PathedRole correlateWithPathedRole;
			RolePathObjectTypeRoot correlateWithPathRoot;
			if (correlationRoot != null)
			{
				if (null != (correlateWithPathedRole = correlationRoot as PathedRole))
				{
					key = new RolePathNode(correlateWithPathedRole, pathContext);
				}
				else if (null != (correlateWithPathRoot = correlationRoot as RolePathObjectTypeRoot))
				{
					key = new RolePathNode(correlateWithPathRoot, pathContext);
				}
				else
				{
					key = new ContextBoundUnifier((PathObjectUnifier)correlationRoot, pathContext);
				}
			}
			return key;
		}
		/// <summary>
		/// Test if a node starts with a given variable. Front text is allowed.
		/// </summary>
		private bool NodeStartsWithVariable(VerbalizationPlanNode node, RolePlayerVariable variable)
		{
			LinkedNode<VerbalizationPlanNode> childNodeLink;
			switch (node.NodeType)
			{
				case VerbalizationPlanNodeType.FactType:
					PathedRole entryPathedRole;
					RolePlayerVariableUse? entryPathedRoleVariableUse;
					return 0 != (node.ReadingOptions & VerbalizationPlanReadingOptions.BasicLeadRole) &&
						(entryPathedRoleVariableUse = GetRolePlayerVariableUse(new RolePathNode(entryPathedRole = node.FactTypeEntry, node.PathContext))).HasValue &&
						variable == entryPathedRoleVariableUse.Value.PrimaryRolePlayerVariable &&
						entryPathedRole.Role == node.Reading.RoleCollection[0].Role;
				case VerbalizationPlanNodeType.Branch:
					VerbalizationPlanBranchType branchType = node.BranchType;
					switch (branchType)
					{
						case VerbalizationPlanBranchType.NegatedChain:
							VerbalizationPlanNode negatedChildNode;
							if (null != (childNodeLink = node.FirstChildNode) &&
								0 != ((negatedChildNode = childNodeLink.Value).ReadingOptions & VerbalizationPlanReadingOptions.BasicLeadRole))
							{
								if (0 != (ResolveDynamicNegatedExitRole(negatedChildNode) & VerbalizationPlanReadingOptions.NegatedExitRole) ||
									GetCollapsibleLeadAllowedFromBranchType(VerbalizationPlanBranchType.NegatedChain))
								{
									return NodeStartsWithVariable(negatedChildNode, variable);
								}
							}
							break;
						default:
							if (null != (childNodeLink = node.FirstChildNode) &&
								GetCollapsibleLeadAllowedFromBranchType(branchType))
							{
								return NodeStartsWithVariable(childNodeLink.Value, variable);
							}
							break;
					}
					break;
			}
			return false;
		}
		/// <summary>
		/// Partner two unpartnered variables using the current pairing phase.
		/// The partnered variable should have been retrieved with <see cref="GetUnpairedPartnerVariable"/>.
		/// </summary>
		/// <param name="primaryVariable">The primary (left) variable to partner.</param>
		/// <param name="preRenderedPrimary">Already rendered text to represent the primary variable, or null for a standard replacement.</param>
		/// <param name="partnerWithVariable">The partner (right) variable.</param>
		/// <param name="preRenderedPartner">Already rendered text to represent the partner variable, or null for a standard replacement.</param>
		/// <param name="leadRolePattern">Use the basic lead role pattern, which gives a smoother reading by using a separate statement
		/// to form an identity pair at the expense of only being usable in lead constructs. The universally applicable but somewhat clumsy
		/// 'some A that is that B' becomes 'that B is some A that', or in a collapsed form using a personal or impersonal pronoun 'that is some A that'.</param>
		/// <returns>A combined string</returns>
		private string PartnerVariables(RolePlayerVariable primaryVariable, string preRenderedPrimary, RolePlayerVariable partnerWithVariable, string preRenderedPartner, bool leadRolePattern)
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
				renderer.GetSnippet(leftRolePlayer != null && leftRolePlayer.TreatAsPersonal ? (leadRolePattern ? CoreVerbalizationSnippetType.PersonalLeadIdentityCorrelation : CoreVerbalizationSnippetType.PersonalIdentityCorrelation) : (leadRolePattern ? CoreVerbalizationSnippetType.ImpersonalLeadIdentityCorrelation : CoreVerbalizationSnippetType.ImpersonalIdentityCorrelation)),
				preRenderedPrimary,
				QuantifyRolePlayerName(preRenderedPartner, partnerWithVariable.Use(CurrentQuantificationUsePhase, true), false));
			if (pairings == null)
			{
				myCorrelatedVariablePairing = pairings = new Dictionary<CorrelatedVariablePairing, int>();
			}
			pairings[new CorrelatedVariablePairing(primaryVariable, partnerWithVariable)] = CurrentPairingUsePhase;
			return retVal;
		}
		private delegate void RegisterSubqueryProjectionVariable(object key, LeadRolePath rolePath, ObjectType variableType, RolePathNode correlationNode);
		/// <summary>
		/// Register variables required for projection by a subquery object. Helper method to
		/// ensure consistency in subquery projection processing by <see cref="InitializeRolePath"/> and
		/// top-level query handling in <see cref="QueryDerivationRuleVerbalizer.AddPathProjections"/>.
		/// </summary>
		/// <param name="pathOwner">The owner of the role path(s) being projected.</param>
		/// <param name="variableRegistrar">A callback to register variables.</param>
		/// <param name="keyDecorator">A callback to translate a projection key (either a role or a parameter) into a decorated key.
		/// If specified, keys are preprocessed by this call before being passed to the variable registrar.</param>
		private IDictionary<LeadRolePath, IList<IList<object>>> GenerateRoleAndParameterProjections(RolePathOwner pathOwner, RegisterSubqueryProjectionVariable variableRegistrar, VariableKeyDecorator keyDecorator)
		{
			// Overlay all projection information
			// Make the QueryDerivationRule optional so that this supports inlining of derived fact types
			RoleProjectedDerivationRule derivationRule = (RoleProjectedDerivationRule)pathOwner;
			bool checkParameters = derivationRule is QueryDerivationRule;
			IDictionary<LeadRolePath, IList<IList<object>>> retVal = null;
			BitTracker projectionHandledThroughEquality = new BitTracker();
			RolePathCache cache = EnsureRolePathCache();
			foreach (RoleSetDerivationProjection projection in RoleSetDerivationProjection.GetLinksToProjectedPathComponentCollection(derivationRule))
			{
				// If the same node projects (directly or via the normalized correlation root) on two or more
				// different roles in the same projection, then we only project one of these roles and return
				// the keys for these variables to the caller to inject equality nodes into the tree.
				// An alternate approach is to only do this manipulation if there is some lead path that does
				// not use the same projection combination. However, this results in a lack of subscripting for
				// ring roles, does not emphasize the equality of the role projections, and results in a changeable
				// verbalization as paths are added. This alternate approach is also much harder to compute.
				// This code is expanded to add the same considerations for query parameters. The result is
				// that path projected roles and parameter bindings are treated collectively as free variables.
				List<IList<object>> equalRolePairings = null;
				ReadOnlyCollection<DerivedRoleProjection> roleProjections = DerivedRoleProjection.GetLinksToProjectedRoleCollection(projection);
				int roleProjectionCount = roleProjections.Count;
				LeadRolePath rolePath = projection.RolePath;
				// Get the parameter bindings associated with the query
				ReadOnlyCollection<QueryParameterBinding> parameterBindings;
				int parameterBindingCount;
				if (checkParameters)
				{
					parameterBindings = QueryParameterBinding.GetLinksToParameterBindings(rolePath);
					parameterBindingCount = parameterBindings.Count;
				}
				else
				{
					parameterBindings = null;
					parameterBindingCount = 0;
				}
				projectionHandledThroughEquality.Resize(roleProjectionCount + parameterBindingCount);
				projectionHandledThroughEquality.Reset();
				for (int i = 0; i < roleProjectionCount; ++i)
				{
					if (!projectionHandledThroughEquality[i])
					{
						DerivedRoleProjection roleProjection = roleProjections[i];
						Role role = roleProjection.ProjectedRole;
						object roleKey = keyDecorator != null ? keyDecorator(role) : role;
						RolePathNode correlationNode = ResolveCorrelationNode(roleProjection);
						if (!correlationNode.IsEmpty)
						{
							object correlationKey = cache.GetCorrelationRoot(correlationNode);
							ObjectType objectType = correlationNode.ObjectType;
							if (objectType != null)
							{
								bool correlatedWithValueType = objectType.IsValueType;
								if (null != (objectType = role.RolePlayer) &&
									correlatedWithValueType != objectType.IsValueType)
								{
									// Add an 'identified by' equivalence node
									if (equalRolePairings == null)
									{
										(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
										equalRolePairings.Add(new VariableEquivalenceIdentifiedByImpl(correlatedWithValueType ? roleKey : correlationKey, correlatedWithValueType ? correlationKey : roleKey));
									}
								}
							}
							List<object> equalRolePairing = null;
							for (int j = i + 1; j < roleProjectionCount; ++j)
							{
								DerivedRoleProjection testRoleProjection = roleProjections[j];
								RolePathNode testCorrelationNode = ResolveCorrelationNode(testRoleProjection);
								// Note that there is no context here and we know that these are elements from
								// the role path itself, so there is no need to call CorrelationRootToContextBoundKey
								// or use Equals instead of == to compare the correlation keys, which will be roots,
								// pathed roles, or unifiers.
								// The same comment applies to other uses of correlation keys below.
								if (!testCorrelationNode.IsEmpty &&
									correlationKey == cache.GetCorrelationRoot(testCorrelationNode))
								{
									projectionHandledThroughEquality[j] = true;
									if (equalRolePairing == null)
									{
										equalRolePairing = new List<object>();
										equalRolePairing.Add(roleKey);
										if (equalRolePairings == null)
										{
											(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
										}
										equalRolePairings.Add(equalRolePairing);
									}
									Role equalRole = testRoleProjection.ProjectedRole;
									object equalRoleKey = keyDecorator != null ? keyDecorator(equalRole) : equalRole;
									variableRegistrar(equalRoleKey, rolePath, equalRole.RolePlayer, RolePathNode.Empty);
									equalRolePairing.Add(equalRoleKey);
								}
							}
							for (int j = 0; j < parameterBindingCount; ++j)
							{
								if (!projectionHandledThroughEquality[j + roleProjectionCount])
								{
									QueryParameterBinding testBinding = parameterBindings[j];
									RolePathNode testCorrelationNode = ResolveCorrelationNode(testBinding);
									if (!testCorrelationNode.IsEmpty &&
										correlationKey == cache.GetCorrelationRoot(testCorrelationNode))
									{
										projectionHandledThroughEquality[j + roleProjectionCount] = true;
										if (equalRolePairing == null)
										{
											equalRolePairing = new List<object>();
											equalRolePairing.Add(roleKey);
											if (equalRolePairings == null)
											{
												(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
											}
											equalRolePairings.Add(equalRolePairing);
										}
										QueryParameter equalParameter = testBinding.QueryParameter;
										object equalParameterKey = keyDecorator != null ? keyDecorator(equalParameter) : equalParameter;
										variableRegistrar(equalParameterKey, rolePath, equalParameter.ParameterType, RolePathNode.Empty);
										equalRolePairing.Add(equalParameterKey);
									}
								}
							}
						}
						variableRegistrar(roleKey, rolePath, role.RolePlayer, correlationNode);
					}
				}
				for (int i = 0; i < parameterBindingCount; ++i)
				{
					if (!projectionHandledThroughEquality[i + roleProjectionCount])
					{
						QueryParameterBinding parameterBinding = parameterBindings[i];
						QueryParameter parameter = parameterBinding.QueryParameter;
						object parameterKey = keyDecorator != null ? keyDecorator(parameter) : parameter;
						RolePathNode correlationNode = ResolveCorrelationNode(parameterBinding);
						if (!correlationNode.IsEmpty)
						{
							object correlationKey = cache.GetCorrelationRoot(correlationNode);
							List<object> equalRolePairing = null;
							for (int j = i + 1; j < parameterBindingCount; ++j)
							{
								if (!projectionHandledThroughEquality[j + roleProjectionCount])
								{
									QueryParameterBinding testBinding = parameterBindings[j];
									RolePathNode testCorrelationNode = ResolveCorrelationNode(testBinding);
									if (!testCorrelationNode.IsEmpty &&
										correlationKey == cache.GetCorrelationRoot(testCorrelationNode))
									{
										projectionHandledThroughEquality[j + roleProjectionCount] = true;
										if (equalRolePairing == null)
										{
											equalRolePairing = new List<object>();
											equalRolePairing.Add(parameterKey);
											if (equalRolePairings == null)
											{
												(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
											}
											equalRolePairings.Add(equalRolePairing);
										}
										QueryParameter equalParameter = testBinding.QueryParameter;
										object equalParameterKey = keyDecorator != null ? keyDecorator(equalParameter) : equalParameter;
										variableRegistrar(equalParameterKey, rolePath, equalParameter.ParameterType, RolePathNode.Empty);
										equalRolePairing.Add(equalParameterKey);
									}
								}
							}
						}
						variableRegistrar(parameterKey, rolePath, parameter.ParameterType, correlationNode);
					}
				}
			}

			if (checkParameters)
			{
				// The previous loop handles parameter for all paths with role projections. However, it is possible in
				// degenerate or incomplete cases to have bound parameters with no projections, so we need to find all
				// parameter bindings on paths with no associated projection and reapply the previous code.
				foreach (LeadRolePath rolePath in derivationRule.LeadRolePathCollection)
				{
					ReadOnlyCollection<QueryParameterBinding> parameterBindings = QueryParameterBinding.GetLinksToParameterBindings(rolePath);
					int parameterBindingCount = parameterBindings.Count;
					if (0 != parameterBindingCount &&
						0 == RoleSetDerivationProjection.GetLinksToRoleProjectedDerivationRuleProjectionCollection(rolePath).Count)
					{
						List<IList<object>> equalRolePairings = null;
						projectionHandledThroughEquality.Resize(parameterBindingCount);
						projectionHandledThroughEquality.Reset();
						for (int i = 0; i < parameterBindingCount; ++i)
						{
							if (!projectionHandledThroughEquality[i])
							{
								QueryParameterBinding parameterBinding = parameterBindings[i];
								QueryParameter parameter = parameterBinding.QueryParameter;
								object parameterKey = keyDecorator != null ? keyDecorator(parameter) : parameter;
								RolePathNode correlationNode = ResolveCorrelationNode(parameterBinding);
								if (!correlationNode.IsEmpty)
								{
									object correlationKey = cache.GetCorrelationRoot(correlationNode);
									ObjectType objectType = correlationNode.ObjectType;
									if (objectType != null)
									{
										bool correlatedWithValueType = objectType.IsValueType;
										if (null != (objectType = parameter.ParameterType) &&
											correlatedWithValueType != objectType.IsValueType)
										{
											// Add an 'identified by' equivalence node
											if (equalRolePairings == null)
											{
												(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
												equalRolePairings.Add(new VariableEquivalenceIdentifiedByImpl(correlatedWithValueType ? parameterKey : correlationKey, correlatedWithValueType ? correlationKey : parameterKey));
											}
										}
									}
									List<object> equalRolePairing = null;
									for (int j = i + 1; j < parameterBindingCount; ++j)
									{
										if (!projectionHandledThroughEquality[j])
										{
											QueryParameterBinding testBinding = parameterBindings[j];
											RolePathNode testCorrelationNode = ResolveCorrelationNode(testBinding);
											if (!testCorrelationNode.IsEmpty &&
												correlationKey == cache.GetCorrelationRoot(testCorrelationNode))
											{
												projectionHandledThroughEquality[j] = true;
												if (equalRolePairing == null)
												{
													equalRolePairing = new List<object>();
													equalRolePairing.Add(parameterKey);
													if (equalRolePairings == null)
													{
														(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[rolePath] = equalRolePairings = new List<IList<object>>();
													}
													equalRolePairings.Add(equalRolePairing);
												}
												QueryParameter equalParameter = testBinding.QueryParameter;
												object equalParameterKey = keyDecorator != null ? keyDecorator(equalParameter) : equalParameter;
												variableRegistrar(equalParameterKey, rolePath, equalParameter.ParameterType, RolePathNode.Empty);
												equalRolePairing.Add(equalParameterKey);
											}
										}
									}
								}
								variableRegistrar(parameterKey, rolePath, parameter.ParameterType, correlationNode);
							}
						}
					}
				}
			}
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
			CoreVerbalizationSnippetType snippet = CoreVerbalizationSnippetType.DefiniteArticle;
			bool isDeontic = false;
			bool isNegative = negateExistentialQuantifier && existentialQuantifier;
			if (existentialQuantifier)
			{
				CoreSnippetIdentifier? forceSnippet = myLeadVariableQuantifier;
				if (forceSnippet.HasValue)
				{
					CoreSnippetIdentifier snippetId = forceSnippet.Value;
					snippet = snippetId.Snippet;
					isDeontic = snippetId.IsDeontic;
					isNegative = snippetId.IsNegative;
					myLeadVariableQuantifier = null;
				}
				else
				{
					snippet = CoreVerbalizationSnippetType.ExistentialQuantifier;
				}
			}
			string formatString = renderer.GetSnippet(snippet, isDeontic, isNegative);
			if (string.IsNullOrEmpty(formatString) || formatString == "{0}")
			{
				return rolePlayerName;
			}
			return string.Format(renderer.FormatProvider, formatString, rolePlayerName);
		}
		private string GetSubscriptedRolePlayerName(RolePlayerVariable variable)
		{
			return myRenderer.RenderRolePlayer(variable.RolePlayer, ReserveSubscript(variable), false);
		}
		private int ReserveSubscript(RolePlayerVariable variable)
		{
			ObjectType rolePlayer = variable.RolePlayer;
			return (rolePlayer == null ? myMissingRolePlayerVariables.Value : myObjectTypeToVariableMap[rolePlayer]).ReserveSubscript(variable);
		}
		private string QuantifyFullyExistentialRolePlayer(ObjectType rolePlayer, bool negateExistentialQuantifier, VerbalizationHyphenBinder hyphenBinder, int hyphenBinderRoleIndex)
		{
			return QuantifyRolePlayerName(hyphenBinder.HyphenBindRoleReplacement(myRenderer.RenderRolePlayer(rolePlayer, 0, true), hyphenBinderRoleIndex), true, negateExistentialQuantifier);
		}
		#endregion // Rendering Methods
		#region Static Helper Methods
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
		/// <summary>
		/// Helper method to get a correlation object from a fact type derivation role projection
		/// </summary>
		private static RolePathNode ResolveCorrelationNode(DerivedRoleProjection roleProjection)
		{
			PathedRole pathedRole;
			RolePathObjectTypeRoot pathRoot;
			if (null != (pathedRole = roleProjection.ProjectedFromPathedRole))
			{
				return new RolePathNode(pathedRole);
			}
			else if (null != (pathRoot = roleProjection.ProjectedFromPathRoot))
			{
				return new RolePathNode(pathRoot);
			}
			return RolePathNode.Empty;
		}
		/// <summary>
		/// Helper method to get a correlation object from a constraint role projection
		/// </summary>
		private static RolePathNode ResolveCorrelationNode(ConstraintRoleProjection constraintRoleProjection)
		{
			PathedRole pathedRole;
			RolePathObjectTypeRoot pathRoot;
			if (null != (pathedRole = constraintRoleProjection.ProjectedFromPathedRole))
			{
				return new RolePathNode(pathedRole);
			}
			else if (null != (pathRoot =constraintRoleProjection.ProjectedFromPathRoot))
			{
				return new RolePathNode(pathRoot);
			}
			return RolePathNode.Empty;
		}
		/// <summary>
		/// Helper method to get a correlation object from a query parameter binding
		/// </summary>
		private static RolePathNode ResolveCorrelationNode(QueryParameterBinding parameterBinding)
		{
			PathedRole pathedRole; 
			RolePathObjectTypeRoot pathRoot;
			if (null != (pathedRole = parameterBinding.BoundToPathedRole))
			{
				return new RolePathNode(pathedRole);
			}
			else if (null != (pathRoot = parameterBinding.BoundToPathRoot))
			{
				return new RolePathNode(pathRoot);
			}
			return RolePathNode.Empty;
		}
		#endregion // Static Helper Methods
		#region Type-specific Creation Methods
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="FactTypeDerivationRule"/>
		/// </summary>
		public static RolePathVerbalizer Create(FactTypeDerivationRule derivationRule, IRolePathRenderer rolePathRenderer)
		{
			RoleProjectedDerivationRuleVerbalizer retVal = new RoleProjectedDerivationRuleVerbalizer(rolePathRenderer);
			retVal.InitializeRolePathOwner(derivationRule);
			return retVal;
		}
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="FactTypeDerivationRule"/>
		/// </summary>
		public static RolePathVerbalizer Create(QueryDerivationRule derivationRule, IRolePathRenderer rolePathRenderer)
		{
			RoleProjectedDerivationRuleVerbalizer retVal = new RoleProjectedDerivationRuleVerbalizer(rolePathRenderer);
			retVal.InitializeRolePathOwner(derivationRule);
			return retVal;
		}
		/// <summary>
		/// Create a new <see cref="RolePathVerbalizer"/> for a given <see cref="SetConstraint"/>
		/// </summary>
		public static RolePathVerbalizer Create(SetConstraint setConstraint, IRolePathRenderer rolePathRenderer)
		{
			SetConstraintVerbalizer retVal = new SetConstraintVerbalizer(rolePathRenderer);
			retVal.Initialize(setConstraint);
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
			retVal.AddExternalVariable(derivationRule, null, derivationRule.Subtype, RolePathNode.Empty);
			return retVal;
		}
		#endregion // Type-specific Creation Methods
		#region Type-specific Classes
		#region RoleProjectedDerivationRuleVerbalizer class
		/// <summary>
		/// A class to assist in verbalization of fact type and query derivations
		/// </summary>
		private class RoleProjectedDerivationRuleVerbalizer : RolePathVerbalizer
		{
			public RoleProjectedDerivationRuleVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Override to add and correlate variables for projection bindings
			/// </summary>
			protected override IDictionary<LeadRolePath, IList<IList<object>>> AddPathProjections(RolePathOwner pathOwner)
			{
				return GenerateRoleAndParameterProjections(
					pathOwner,
					delegate(object key, LeadRolePath forRolePath, ObjectType variableType, RolePathNode correlationNode)
					{
						AddExternalVariable(key, null, variableType, correlationNode);
					},
					null);
			}
			/// <summary>
			/// Override to bind calculation and constant projections
			/// </summary>
			protected override void AddCalculatedAndConstantProjections(object pathContext, RolePathOwner pathOwner, LeadRolePath rolePath, VariableKeyDecorator keyDecorator)
			{
				// Overlay projection information
				RoleSetDerivationProjection projection = RoleSetDerivationProjection.GetLink((RoleProjectedDerivationRule)pathOwner, rolePath);
				if (projection != null)
				{
					foreach (DerivedRoleProjection roleProjection in DerivedRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						CalculatedPathValue calculation;
						PathConstant constant;
						Role role;
						if (null != (calculation = roleProjection.ProjectedFromCalculatedValue))
						{
							role = roleProjection.ProjectedRole;
							ProjectExternalVariable(keyDecorator != null ? keyDecorator(role) : role, calculation, pathContext);
						}
						else if (null != (constant = roleProjection.ProjectedFromConstant))
						{
							role = roleProjection.ProjectedRole;
							ProjectExternalVariable(keyDecorator != null ? keyDecorator(role) : role, constant, pathContext);
						}
					}
				}
			}
		}
		#endregion // RoleProjectedDerivationRuleVerbalizer class
		#region QueryDerivationRuleVerbalizer class
		/// <summary>
		/// Add query parameter processing to standard role projected derivation.
		/// </summary>
		private sealed class QueryDerivationRuleVerbalizer : RoleProjectedDerivationRuleVerbalizer
		{
			public QueryDerivationRuleVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Bind parameters the same as projections. As far as the rule body
			/// is concerned, these all verbalize as free variables.
			/// </summary>
			protected override IDictionary<LeadRolePath, IList<IList<object>>> AddPathProjections(RolePathOwner pathOwner)
			{
				return GenerateRoleAndParameterProjections(
					pathOwner,
					delegate(object key, LeadRolePath forRolePath, ObjectType variableType, RolePathNode correlationNode)
					{
						AddExternalVariable(key, null, variableType, correlationNode);
					},
					null);
			}
		}
		#endregion // QueryDerivationRuleVerbalizer class
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
			/// <summary>
			/// Verbalize a path if it has a supertype root object type.
			/// </summary>
			protected override bool VerbalizesPath(RolePathOwner pathOwner, LeadRolePath rolePath)
			{
				if (base.VerbalizesPath(pathOwner, rolePath))
				{
					return true;
				}
				SubtypeDerivationRule derivationRule;
				ObjectType rootObjectType;
				ObjectType subtype;
				return null != (derivationRule = pathOwner as SubtypeDerivationRule) &&
					null != (rootObjectType = rolePath.RootObjectType) &&
					null != (subtype = derivationRule.Subtype) &&
					rootObjectType != subtype &&
					ObjectType.GetNearestCompatibleTypes(new ObjectType[] { subtype, rootObjectType }, true).Length != 0;
			}
		}
		#endregion // SubTypeDerivationRuleVerbalizer class
		#region SetConstraintVerbalizer class
		private sealed class SetConstraintVerbalizer : RolePathVerbalizer
		{
			// Member variables, used during initialization callbacks to
			// correlate columns.
			public SetConstraintVerbalizer(IRolePathRenderer rolePathRenderer)
				: base(rolePathRenderer)
			{
			}
			/// <summary>
			/// Initialize all column and join path bindings
			/// </summary>
			/// <param name="setConstraint">The <see cref="SetConstraint"/> to analyze.</param>
			public void Initialize(SetConstraint setConstraint)
			{
				ConstraintRoleSequenceJoinPath joinPath = setConstraint.JoinPath;
				if (joinPath != null)
				{
					InitializeRolePathOwner(joinPath);
				}

				// Fill in any constraint roles that do not have an associated variable.
				// Note that this is an error condition for a pathed sequence, which should be fully
				// projected, but not for a non-pathed sequence. We register variables for all uses to
				// support subscripting for ring situations within the sequence. Checking this condition
				// for pathed sequences also enables better verbalization of incomplete structures.
				bool compatible = 0 != (((IConstraint)setConstraint).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns);
				RolePlayerVariable compatibleVariable = null;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(setConstraint);

				if (compatible)
				{
					ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(
						constraintRoles,
						delegate(ConstraintRoleSequenceHasRole constraintRole) { return constraintRole.Role.RolePlayer; },
						false);
					while (compatibleTypes.Length > 1)
					{
						compatibleTypes = ObjectType.GetNearestCompatibleTypes(compatibleTypes, false);
					}
					compatibleVariable = AddExternalVariable(0, null, compatibleTypes.Length != 0 ? compatibleTypes[0] : null, RolePathNode.Empty);
				}

				foreach (ConstraintRoleSequenceHasRole constraintRole in constraintRoles)
				{
					RolePlayerVariableUse? variableUse = GetRolePlayerVariableUse(constraintRole);
					if (!variableUse.HasValue)
					{
						RolePlayerVariable newVariable = AddExternalVariable(constraintRole, compatibleVariable, constraintRole.Role.RolePlayer, RolePathNode.Empty);
						if (compatible && compatibleVariable == null)
						{
							compatibleVariable = newVariable;
						}
					}
					else if (compatible && compatibleVariable == null)
					{
						compatibleVariable = variableUse.Value.PrimaryRolePlayerVariable;
					}
				}

				if (setConstraint is MandatoryConstraint)
				{
					Dictionary<ObjectType, RelatedRolePlayerVariables> map = myObjectTypeToVariableMap;
					// Mark unconstrained roles in fact types as a pure existential use. This forces subscripts
					// on constrained roles if there is a ring situation, but not on quantified requests of
					// other roles with the same type.
					LinkedElementCollection<Role> roles = setConstraint.RoleCollection;
					int roleCount = roles.Count;
					foreach (FactType factType in setConstraint.FactTypeCollection)
					{
						foreach (RoleBase roleBase in factType.RoleCollection)
						{
							Role role = roleBase.Role;
							ObjectType rolePlayer;
							RelatedRolePlayerVariables vars;
							int roleIndex;
							if (-1 == (roleIndex = roles.IndexOf(role)))
							{
								if (null != (rolePlayer = role.RolePlayer) &&
									map.TryGetValue(rolePlayer, out vars) &&
									!vars.UsedFullyExistentially)
								{
									vars.UsedFullyExistentially = true;
									map[rolePlayer] = vars;
								}
							}
							else if (roleCount > 1)
							{
								// If more than one constrained role is in the same fact type
								// then we mark the variable as used fully existentially so that
								// we can force a subscript when the role is accessed through the
								// constrained role and use the same role to represent a fully
								// existentially quantified variable when the variable is requested
								// through the role itself.
								// This forces verbalization patterns that do not need this
								// feature to minimize head subscripting so that they do not
								// subscript unncessarily, but this is not the place for pattern
								// matching.
								for (int i = 0; i < roleCount; ++i)
								{
									if (i != roleIndex &&
										roles[i].FactType == factType &&
										null != (rolePlayer = role.RolePlayer) &&
										map.TryGetValue(rolePlayer, out vars) &&
										!vars.UsedFullyExistentially)
									{
										vars.UsedFullyExistentially = true;
										map[rolePlayer] = vars;
									}
								}
							}
						}
					}
				}

				// Use phases are used during both initialization and rendering. Make
				// sure a use phase is pushed so that we don't see quantified elements
				// as a side effect of initialization.
				BeginQuantificationUsePhase();
			}
			/// <summary>
			/// The resolved supertype for the set constraint is the compatible column,
			/// which is keyed with the column number (0).
			/// </summary>
			protected override object ResolveSupertypeKey(object rolePlayerFor)
			{
				ConstraintRoleSequenceHasRole constraintRole;
				if (null != (constraintRole = rolePlayerFor as ConstraintRoleSequenceHasRole) &&
					myUseToVariableMap.ContainsKey(0))
				{
					return 0;
				}
				return base.ResolveSupertypeKey(rolePlayerFor);
			}
			/// <summary>
			/// Verbalize a path if it is projected
			/// </summary>
			protected override bool VerbalizesPath(RolePathOwner pathOwner, LeadRolePath rolePath)
			{
				if (base.VerbalizesPath(pathOwner, rolePath))
				{
					return true;
				}
				ConstraintRoleSequenceJoinPath joinPath;
				return null != (joinPath = pathOwner as ConstraintRoleSequenceJoinPath) &&
					null != ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, rolePath);
			}
			/// <summary>
			/// Override to add and correlate variables for projection bindings
			/// </summary>
			protected override IDictionary<LeadRolePath, IList<IList<object>>> AddPathProjections(RolePathOwner pathOwner)
			{
				// Overlay all projection information
				ConstraintRoleSequenceJoinPath joinPath = (ConstraintRoleSequenceJoinPath)pathOwner;
				ConstraintRoleSequence roleSequence = joinPath.RoleSequence;
				bool correlateProjectedRoles = 0 != (((IConstraint)roleSequence).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns);
				RolePlayerVariable correlatedProjectionVariable = null;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(roleSequence);
				IDictionary<LeadRolePath, IList<IList<object>>> retVal = null;
				BitTracker projectionHandledThroughEquality = new BitTracker();
				RolePathCache cache = EnsureRolePathCache();
				foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(joinPath))
				{

					ReadOnlyCollection<ConstraintRoleProjection> constraintRoleProjections = ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection);
					int constraintRoleProjectionCount = constraintRoleProjections.Count;
					List<IList<object>> equalRolePairings = null;
					if (!correlateProjectedRoles)
					{
						// Guard against projecting the same variables onto different roles
						projectionHandledThroughEquality.Resize(constraintRoleProjectionCount);
						projectionHandledThroughEquality.Reset();
					}
					for (int i = 0; i < constraintRoleProjectionCount; ++i)
					{
						if (correlateProjectedRoles || !projectionHandledThroughEquality[i])
						{
							ConstraintRoleProjection constraintRoleProjection = constraintRoleProjections[i];
							RolePathNode correlationNode = ResolveCorrelationNode(constraintRoleProjection);
							ConstraintRoleSequenceHasRole constraintRole = constraintRoleProjection.ProjectedConstraintRole;
							if (!correlateProjectedRoles && !correlationNode.IsEmpty)
							{
								object correlationKey = cache.GetCorrelationRoot(correlationNode);
								ObjectType objectType = correlationNode.ObjectType;
								if (objectType != null)
								{
									bool correlatedWithValueType = objectType.IsValueType;
									if (null != (objectType = constraintRole.Role.RolePlayer) &&
										correlatedWithValueType != objectType.IsValueType)
									{
										// Add an 'identified by' equivalence node
										if (equalRolePairings == null)
										{
											(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[projection.RolePath] = equalRolePairings = new List<IList<object>>();
											equalRolePairings.Add(new VariableEquivalenceIdentifiedByImpl(correlatedWithValueType ? constraintRole : correlationKey, correlatedWithValueType ? correlationKey : constraintRole));
										}
									}
								}
								List<object> equalRolePairing = null;
								for (int j = i + 1; j < constraintRoleProjectionCount; ++j)
								{
									ConstraintRoleProjection testRoleProjection = constraintRoleProjections[j];
									RolePathNode testCorrelationNode = ResolveCorrelationNode(testRoleProjection);
									// Note that there is no context here and we know that these are elements from
									// the role path itself, so there is no need to call CorrelationRootToContextBoundKey
									// or use Equals instead of == to compare the correlation keys.
									if (!testCorrelationNode.IsEmpty &&
										correlationKey == cache.GetCorrelationRoot(testCorrelationNode))
									{
										projectionHandledThroughEquality[j] = true;
										if (equalRolePairing == null)
										{
											equalRolePairing = new List<object>();
											equalRolePairing.Add(constraintRole);
											if (equalRolePairings == null)
											{
												(retVal ?? (retVal = new Dictionary<LeadRolePath, IList<IList<object>>>()))[projection.RolePath] = equalRolePairings = new List<IList<object>>();
											}
											equalRolePairings.Add(equalRolePairing);
										}
										ConstraintRoleSequenceHasRole equalRole = testRoleProjection.ProjectedConstraintRole;
										AddExternalVariable(equalRole, null, equalRole.Role.RolePlayer, RolePathNode.Empty);
										equalRolePairing.Add(equalRole);
									}
								}
							}
							RolePlayerVariable newVariable = AddExternalVariable(constraintRole, correlatedProjectionVariable, constraintRole.Role.RolePlayer, correlationNode);
							if (correlateProjectedRoles && correlatedProjectionVariable == null)
							{
								correlatedProjectionVariable = newVariable;
							}
						}
					}
				}
				return retVal;
			}
			/// <summary>
			/// Override to bind calculation and constant projections
			/// </summary>
			protected override void AddCalculatedAndConstantProjections(object pathContext, RolePathOwner pathOwner, LeadRolePath rolePath, VariableKeyDecorator keyDecorator)
			{
				// Overlay projection information.
				// Key decorator is ignored as this is always called top level.
				ConstraintRoleSequenceJoinPathProjection projection = ConstraintRoleSequenceJoinPathProjection.GetLink((ConstraintRoleSequenceJoinPath)pathOwner, rolePath);
				if (projection != null)
				{
					foreach (ConstraintRoleProjection constraintRoleProjection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						CalculatedPathValue calculation;
						PathConstant constant;
						if (null != (calculation = constraintRoleProjection.ProjectedFromCalculatedValue))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, calculation, pathContext);
						}
						else if (null != (constant = constraintRoleProjection.ProjectedFromConstant))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, constant, pathContext);
						}
					}
				}
			}
		}
		#endregion // SetConstraintVerbalizer class
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

				// Determine a shared supertype for each column and register a variable of the appropriate
				// type for each column.
				for (int i = 0; i < columnCount; ++i)
				{
					ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(
						allSequencedRoleLinks,
						i,
						delegate(ConstraintRoleSequenceHasRole constraintRole) { return constraintRole.Role.RolePlayer; },
						false);
					while (compatibleTypes.Length > 1)
					{
						compatibleTypes = ObjectType.GetNearestCompatibleTypes(compatibleTypes, false);
					}
					columnVariables[i] = AddExternalVariable(i, null, compatibleTypes.Length != 0 ? compatibleTypes[0] : null, RolePathNode.Empty);
				}

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
							RolePlayerVariable newVariable = AddExternalVariable(constraintRole, existingVariable, constraintRole.Role.RolePlayer, RolePathNode.Empty);
							if (existingVariable == null)
							{
								columnVariables[j] = newVariable;
							}
						}
					}
				}
				// Use phases are used during both initialization and rendering. Make
				// sure a use phase is pushed so that we don't see quantified elements
				// as a side effect of initialization.
				BeginQuantificationUsePhase();
			}
			/// <summary>
			/// The resolved supertype for each column is keyed by the column
			/// number. Return the column number for a constraint role.
			/// </summary>
			protected override object ResolveSupertypeKey(object rolePlayerFor)
			{
				ConstraintRoleSequenceHasRole constraintRole;
				if (null != (constraintRole = rolePlayerFor as ConstraintRoleSequenceHasRole))
				{
					return constraintRole.ConstraintRoleSequence.RoleCollection.IndexOf(constraintRole.Role);
				}
				return base.ResolveSupertypeKey(rolePlayerFor);
			}
			/// <summary>
			/// Verbalize a path if it is projected
			/// </summary>
			protected override bool VerbalizesPath(RolePathOwner pathOwner, LeadRolePath rolePath)
			{
				if (base.VerbalizesPath(pathOwner, rolePath))
				{
					return true;
				}
				ConstraintRoleSequenceJoinPath joinPath;
				return null != (joinPath = pathOwner as ConstraintRoleSequenceJoinPath) &&
					null != ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, rolePath);
			}
			/// <summary>
			/// Override to add and correlate variables for projection bindings
			/// </summary>
			protected override IDictionary<LeadRolePath, IList<IList<object>>> AddPathProjections(RolePathOwner pathOwner)
			{
				// Overlay all projection information
				ConstraintRoleSequenceJoinPath joinPath = (ConstraintRoleSequenceJoinPath)pathOwner;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = myCurrentRoleSequence;
				RolePlayerVariable[] columnVariables = myColumnVariables;
				foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(joinPath))
				{
					foreach (ConstraintRoleProjection constraintRoleProjection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						ConstraintRoleSequenceHasRole constraintRole = constraintRoleProjection.ProjectedConstraintRole;
						// Correlate down the columns
						int roleIndex = constraintRoles.IndexOf(constraintRole);
						RolePlayerVariable existingVariable = columnVariables[roleIndex];
						RolePlayerVariable newVariable = AddExternalVariable(constraintRole, existingVariable, constraintRole.Role.RolePlayer, ResolveCorrelationNode(constraintRoleProjection));
						if (existingVariable == null)
						{
							columnVariables[roleIndex] = newVariable;
						}
					}
				}
				return null; // UNDONE: PENDING Add equality lists to path projections for SetComparisonConstraint
			}
			/// <summary>
			/// Override to bind calculation and constant projections
			/// </summary>
			protected override void AddCalculatedAndConstantProjections(object pathContext, RolePathOwner pathOwner, LeadRolePath rolePath, VariableKeyDecorator keyDecorator)
			{
				// Overlay projection information
				// Key decorator is ignored as this is always called top level.
				ConstraintRoleSequenceJoinPathProjection projection = ConstraintRoleSequenceJoinPathProjection.GetLink((ConstraintRoleSequenceJoinPath)pathOwner, rolePath);
				if (projection != null)
				{
					foreach (ConstraintRoleProjection constraintRoleProjection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection))
					{
						CalculatedPathValue calculation;
						PathConstant constant;
						if (null != (calculation = constraintRoleProjection.ProjectedFromCalculatedValue))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, calculation, pathContext);
						}
						else if (null != (constant = constraintRoleProjection.ProjectedFromConstant))
						{
							ProjectExternalVariable(constraintRoleProjection.ProjectedConstraintRole, constant, pathContext);
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
