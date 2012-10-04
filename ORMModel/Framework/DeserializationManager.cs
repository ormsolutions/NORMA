#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.Framework
{
	#region INotifyElementAdded interface
	/// <summary>
	/// An interface for notifying when an element has been added
	/// to a Store.
	/// </summary>
	public interface INotifyElementAdded
	{
		/// <summary>
		/// An element has been added to a store
		/// </summary>
		/// <param name="element">The newly added element</param>
		void ElementAdded(ModelElement element);
		/// <summary>
		/// An element has been added to a store
		/// </summary>
		/// <param name="element">The newly added element</param>
		/// <param name="addLinks">true if all links attached directly to the
		/// element should also be added. Defaults to false.</param>
		void ElementAdded(ModelElement element, bool addLinks);
	}
	#endregion // INotifyElementAdded interface
	#region IDeserializationFixupListener interface
	/// <summary>
	/// An interface to provide an extensible plugin point
	/// for managing deserialization fixups
	/// </summary>
	public interface IDeserializationFixupListener : INotifyElementAdded
	{
		/// <summary>
		/// Test if this listeners needs to process
		/// elements for the given deserialization phase.
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the DeserializationFixupManager constructor.</param>
		/// <param name="store">The context store</param>
		/// <returns>true if this listener has elements for this phase</returns>
		bool HasElements(int phase, Store store);
		/// <summary>
		/// Process elements for the specific fixup phase. Care
		/// must be taken when implementing this method to support
		/// additional elements added to the set as a result of the fixups
		/// occuring while processing elements. The implementation does not
		/// need to process the new elements immediately (although this is an
		/// option) because the calling code will continue to call HasElements/ProcessElements
		/// for each phase until all listeners have no elements.
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the deserialization manager constructor.</param>
		/// <param name="store">The context store</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		void ProcessElements(int phase, Store store, INotifyElementAdded notifyAdded);
		/// <summary>
		/// The phase is completed, meaning that all listeners currently return false from
		/// HasElements for this phase. PhaseCompleted should not be used to make additional
		/// model changes. It is provided as the correct time to make external fixups. For example,
		/// populate the task list after all model errors are in place.
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the deserialization manager constructor.</param>
		/// <param name="store">The context store</param>
		void PhaseCompleted(int phase, Store store);
	}
	#endregion // IDeserializationFixupListener interface
	#region IDeserializationFixupListenerProvider interface
	/// <summary>
	/// An interface to implement on the meta model to dynamically provide
	/// fixup listeners from a loaded meta model.
	/// </summary>
	public interface IDeserializationFixupListenerProvider
	{
		/// <summary>
		/// Provide a interator for fixup listeners supported by this model
		/// </summary>
		IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection { get; }
		/// <summary>
		/// Provide the <see cref="Type"/> of an enum that contains the fixup values. The
		/// values in this enum should be loosely spaced to allow fixup phase enums from other
		/// provides to intersperse their own values based on another model. If this returns
		/// <see langword="null"/>, then the fixup phase is the same as an extended domain model
		/// that also supports delayed fixup.
		/// </summary>
		Type DeserializationFixupPhaseType { get; }
	}
	#endregion // IDeserializationFixupListenerProvider interface
	#region DeserializationFixupListener class
	/// <summary>
	/// A base implementation of a fixup listener that enables
	/// listening for a given type and phase of the deserialization
	/// fixup process.
	/// </summary>
	/// <typeparam name="TElement">The type of element to watch out for.
	/// Will frequently be an interface.</typeparam>
	public abstract class DeserializationFixupListener<TElement> : IDeserializationFixupListener where TElement : class
	{
		#region Member Variables
		private int myPhase;
		private Collection<TElement> myCollection;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a fixup listener for the specified deserialization phase
		/// </summary>
		/// <param name="phase">The phase to watch for. All other phase
		/// requests return false.</param>
		protected DeserializationFixupListener(int phase)
		{
			myPhase = phase;
		}
		#endregion // Constructors
		#region IDeserializationFixupListener Implementation
		/// <summary>
		/// Implements IDeserializationFixupListener.HasElements
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the DeserializationFixupManager constructor.</param>
		/// <param name="store">The context store</param>
		/// <returns>True if ProcessElements should be called for this phase</returns>
		protected bool HasElements(int phase, Store store)
		{
			return phase == myPhase && myCollection != null && myCollection.Count > 0;
		}
		bool IDeserializationFixupListener.HasElements(int phase, Store store)
		{
			return HasElements(phase, store);
		}
		/// <summary>
		/// Implements IDeserializationFixupListener.ProcessElements
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the DeserializationFixupManager constructor.</param>
		/// <param name="store">The context store</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		protected void ProcessElements(int phase, Store store, INotifyElementAdded notifyAdded)
		{
			Debug.Assert(HasElements(phase, store));
			if (phase == myPhase)
			{
				// Clear the collection so it can safely be repopulated while
				// the iterator is active.
				Collection<TElement> coll = myCollection;
				myCollection = null;
				foreach (TElement element in coll)
				{
					// Call the abstract method to do the actual work
					ProcessElement(element, store, notifyAdded);
				}
			}
		}
		void IDeserializationFixupListener.ProcessElements(int phase, Store store, INotifyElementAdded notifyAdded)
		{
			ProcessElements(phase, store, notifyAdded);
		}
		/// <summary>
		/// Implements IDeserializationFixupListener.PhaseCompleted. The
		/// default implementation does nothing.
		/// </summary>
		/// <param name="phase">An integer pulled from the
		/// phase enum passed to the DeserializationFixupManager constructor.</param>
		/// <param name="store">The context store</param>
		protected void PhaseCompleted(int phase, Store store)
		{
			if (myPhase == phase)
			{
				PhaseCompleted(store);
			}
		}
		void IDeserializationFixupListener.PhaseCompleted(int phase, Store store)
		{
			PhaseCompleted(phase, store);
		}
		#endregion // IDeserializationFixupListener implementation
		#region INotifyElementAdded Implementation
		/// <summary>
		/// Implements INotifyElementAdded.ElementAdded. Elements will
		/// be added if the ModelElement has a type matching TElement
		/// type parameter
		/// </summary>
		/// <param name="element">The element to add</param>
		protected void ElementAdded(ModelElement element)
		{
			TElement typedElement = element as TElement;
			if (typedElement != null)
			{
				if (VerifyElementType(element))
				{
					ElementCollection.Add(typedElement);
				}
			}
		}
		void INotifyElementAdded.ElementAdded(ModelElement element)
		{
			ElementAdded(element);
		}
		/// <summary>
		/// Implements INotifyElementAdded.ElementAdded(ModelElement, bool).
		/// The default implementation throws a NotSupportedException.
		/// </summary>
		/// <param name="element">The newly added element</param>
		/// <param name="addLinks">true if all links attached directly to the
		/// element should also be added. Defaults to false.</param>
		protected void ElementAdded(ModelElement element, bool addLinks)
		{
			Debug.Fail("Direct call not supported");
			throw new NotSupportedException();
		}
		void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
		{
			ElementAdded(element, addLinks);
		}
		#endregion // INotifyElementAdded Implementation
		#region DeserializationFixupListener specific
		private Collection<TElement> ElementCollection
		{
			get
			{
				Collection<TElement> coll = myCollection;
				if (coll == null)
				{
					myCollection = coll = new Collection<TElement>();
				}
				return coll;
			}
		}
		/// <summary>
		/// Override this method to process an element
		/// for this phase.
		/// </summary>
		/// <param name="element">A typed element. The element
		/// is always castable to a model element</param>
		/// <param name="store">The context store</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		protected abstract void ProcessElement(TElement element, Store store, INotifyElementAdded notifyAdded);
		/// <summary>
		/// The phase is complete. Default implementation does nothing.
		/// </summary>
		/// <param name="store">The context store</param>
		protected virtual void PhaseCompleted(Store store)
		{
		}
		/// <summary>
		/// Verify type information about the provided element. This filter is applied
		/// after the <paramref name="element"/> is known to be of type <typeparamref name="TElement"/>.
		/// Although the <see cref="ModelElement"/> instance is provided, no properties or
		/// relationships generally associated with a fully constructed element are available
		/// at this point. The only reliable information is available through <see cref="M:ModelElement.GetClassInfo"/>
		/// and <see cref="P:ModelElement.Store"/>.
		/// </summary>
		/// <param name="element">The element to verify</param>
		/// <returns><see langword="true"/> to process the element.</returns>
		protected virtual bool VerifyElementType(ModelElement element)
		{
			return true;
		}
		#endregion // DeserializationFixupListener specific
	}
	#endregion // DeserializationFixupListener class
	#region StandardFixupPhase enum
	/// <summary>
	/// Standard values to use with enum types return by <see cref="IDeserializationFixupListenerProvider.DeserializationFixupPhaseType"/>
	/// implementations. This enum is not loaded as a phase, rather it is meant as a starting
	/// point for coordinating loading across different models. These values can be used directly
	/// in other enums.
	/// </summary>
	public enum StandardFixupPhase
	{
		/// <summary>
		/// The first standard fixup phase for loading model elements. Presentation
		/// elements have a separate set of phases.
		/// </summary>
		FirstModelElementPhase = AddIntrinsicElements,
		/// <summary>
		/// Add any intrinsic elements at this stage. Intrinsic elements
		/// are not serialized but must always be present in a fully loaded
		/// model.
		/// </summary>
		AddIntrinsicElements = 100,
		/// <summary>
		/// Replace any deprecated elements with replacement patterns
		/// and remove the deprecated elements.
		/// This stage may both add and remove events.
		/// </summary>
		ReplaceDeprecatedStoredElements = 200,
		/// <summary>
		/// Verify any implied elements that are serialized with the model
		/// but must follow a proscribed pattern based on another serialized element.
		/// This stage may both add and remove elements.
		/// </summary>
		ValidateImplicitStoredElements = 300,
		/// <summary>
		/// Add implicit elements at this stage. An implicit element is
		/// not serialized and is generally created by a rule once the model
		/// is loaded.
		/// </summary>
		AddImplicitElements = 400,
		/// <summary>
		/// The last standard fixup phase for loading model elements. Presentation
		/// elements have a separate set of phases
		/// </summary>
		LastModelElementPhase = AddImplicitElements,
		/// <summary>
		/// The first standard fixup phase for loading presentation elements.
		/// </summary>
        FirstPresentationElementPhase = AutoCreateStoredPresentationElements,
        /// <summary>
        /// Initialized automatically created stored presentation elements that
        /// will be serialized with the model.
        /// </summary>
        AutoCreateStoredPresentationElements = 5100,
		/// <summary>
		/// Fixup stored presentation elements
		/// </summary>
		ValidateStoredPresentationElements = 5200,
		/// <summary>
		/// Validate presentation elements that are implicitly recreated
		/// if they are not serialized.
		/// </summary>
		ValidateImplicitStoredPresentationElements = 5300,
		/// <summary>
		/// Add any presentation elements that are implicit and not
		/// serialized with the model.
		/// </summary>
		AddImplicitPresentationElements = 5400,
		/// <summary>
		/// The last standard fixup phase for loading presentation elements.
		/// </summary>
		LastPresentationElementPhase = AddImplicitPresentationElements,
	}
	#endregion // StandardFixupPhase enum
}	
