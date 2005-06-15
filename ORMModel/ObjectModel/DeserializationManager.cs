using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Reflection;

namespace Northface.Tools.ORM.ObjectModel
{
	#region INotifyElementAdded interface
	/// <summary>
	/// An interface for notifying when an element has been added
	/// to a Store.
	/// </summary>
	[CLSCompliant(true)]
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
	#region DeserializationFixupManager class
	/// <summary>
	/// A class to manage post-deserialization model fixup
	/// </summary>
	public class DeserializationFixupManager : INotifyElementAdded
	{
		#region Member Variables
		private Store myStore;
		private List<IDeserializationFixupListener> myListeners;
		private int[] myPhases;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// A class to manage the complex process of post-deserialization
		/// model fixup. Generally, rules are suspended during loading as
		/// it is difficult to enforce any complex rules when only portions
		/// off the model are in place. This means that there is no guarantee
		/// after a load sequence that the model is in workable valid state.
		/// The invalid state can occur because certain model elements are not
		/// serialized, or because of the Notepad factor (edits to the model file
		/// outside a sanctioned editor). Post-deserialization fixups allow different
		/// elements in the model to bring the model up to a consistent state
		/// so that all subsequent edits run against a model in a known state.
		/// </summary>
		/// <param name="phaseEnum">An enum with a list of phase
		/// values. The values are sorted (by value, not by member order in
		/// the enum) and are then interpreted by each listener. If derived
		/// models need to add values, then they should include all values
		/// from the original enums, and add 'phases' with new numbers. Enum values
		/// should be entered with plenty of space between them to facilitate
		/// derived models that need to add serialization phases.</param>
		/// <param name="store">The store being deserialized to.</param>
		public DeserializationFixupManager(Type phaseEnum, Store store)
		{
			myStore = store;
			myListeners = new List<IDeserializationFixupListener>();
			int[] phases = (int[])Enum.GetValues(phaseEnum);
			Array.Sort<int>(phases);
			myPhases = phases;
		}
		#endregion // Constructors
		#region INotifyElementAdded Implementation
		/// <summary>
		/// Note that this matches the signature for the
		/// System.VisualStudio.Modeling.Diagnostics.XmlSerialization.Deserialized
		/// delegate, so can be used as the callback point for the
		/// Microsoft-provided IMS deserialization engine.
		/// </summary>
		/// <param name="element">The newly added element</param>
		void INotifyElementAdded.ElementAdded(ModelElement element)
		{
			ElementAdded(element);
		}
		/// <summary>
		/// Implements INotifyElementAdded.ElementAdded(ModelElement).
		/// </summary>
		/// <param name="element">The newly added element</param>
		protected void ElementAdded(ModelElement element)
		{
			List<IDeserializationFixupListener> listeners = myListeners;
			int listenerCount = listeners.Count;
			for (int i = 0; i < listenerCount; ++i)
			{
				listeners[i].ElementAdded(element);
			}
		}
		/// <summary>
		/// Implements INotifyElementAdded.ElementAdded(ModelElement, bool)
		/// </summary>
		/// <param name="element">The newly added element</param>
		/// <param name="addLinks">true if all links attached directly to the
		/// element should also be added. Defaults to false.</param>
		protected void ElementAdded(ModelElement element, bool addLinks)
		{
			// Call through the interface to support overrides
			INotifyElementAdded notify = this;
			notify.ElementAdded(element);
			if (addLinks)
			{
				IList links = element.GetElementLinks();
				int linkCount = links.Count;
				for (int i = 0; i < linkCount; ++i)
				{
					notify.ElementAdded((ModelElement)links[i]);
				}
			}
		}
		void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
		{
			ElementAdded(element, addLinks);
		}
		#endregion // INotifyElementAdded Implementation
		#region DeserializationFixupManager specific
		/// <summary>
		/// Add a deserialization fixup listener
		/// </summary>
		/// <param name="listener">The listener to add</param>
		public void AddListener(IDeserializationFixupListener listener)
		{
			myListeners.Add(listener);
		}
		/// <summary>
		/// Deserialization has been completed. Proceed with the
		/// fixup process.
		/// </summary>
		public virtual void DeserializationComplete()
		{
			int[] phases = myPhases;
			int phaseCount = phases.Length;
			List<IDeserializationFixupListener> listeners = myListeners;
			int listenerCount = listeners.Count;
			Store store = myStore;
			for (int phaseIndex = 0; phaseIndex < phaseCount; ++phaseIndex)
			{
				int phase = phases[phaseIndex];
				bool phaseComplete = false;
				// Process elements on the current phase until HasElements
				// returns false for all listeners in the phase.
				while (!phaseComplete)
				{
					phaseComplete = true;
					for (int i = 0; i < listenerCount; ++i)
					{
						IDeserializationFixupListener listener = listeners[i];
						if (listener.HasElements(phase, store))
						{
							phaseComplete = false;
							listener.ProcessElements(phase, store, this);
						}
					}
				}
				for (int i = 0; i < listenerCount; ++i)
				{
					listeners[i].PhaseCompleted(phase, store);
				}
			}
#if DEBUG
			// Walk through one more time and assert if elements
			// were added to the listener for any phase after
			// it was completed.
			for (int phaseIndex = 0; phaseIndex < phaseCount; ++phaseIndex)
			{
				int phase = phases[phaseIndex];
				for (int i = 0; i < listenerCount; ++i)
				{
					IDeserializationFixupListener listener = listeners[i];
					if (listener.HasElements(phase, store))
					{
						Debug.Fail(string.Format(CultureInfo.InvariantCulture, "A fixup phase after phase {0} added elements to an IDeserializationFixupListener of type {1}.", phase, listener.GetType().FullName));
					}
				}
			}
#endif // DEBUG
		}
		#endregion // DeserializationFixupManager specific
	}
	#endregion // DeserializationFixupManager class
	#region DeserializationFixupListener class
	/// <summary>
	/// A base implementation of a fixup listener that enables
	/// listening for a given type and phase of the deserialization
	/// fixup process.
	/// </summary>
	/// <typeparam name="ElementType">The type of element to watch out for.
	/// Will frequently be an interface.</typeparam>
	public abstract class DeserializationFixupListener<ElementType> : IDeserializationFixupListener where ElementType : class
	{
		#region Member Variables
		private int myPhase;
		private Collection<ElementType> myCollection;
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
				Collection<ElementType> coll = myCollection;
				myCollection = null;
				foreach (ElementType element in coll)
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
		/// be added if the ModelElement has a type matching ElementType
		/// type parameter
		/// </summary>
		/// <param name="element">The element to add</param>
		protected void ElementAdded(ModelElement element)
		{
			ElementType typedElement = element as ElementType;
			if (typedElement != null)
			{
				ElementCollection.Add(typedElement);
			}
		}
		void INotifyElementAdded.ElementAdded(ModelElement element)
		{
			ElementAdded(element);
		}
		/// <summary>
		/// Implements INotifyElementAdded.ElementAdded(ModelElement, bool).
		/// The default implementation throws a NotImplementedException.
		/// </summary>
		/// <param name="element">The newly added element</param>
		/// <param name="addLinks">true if all links attached directly to the
		/// element should also be added. Defaults to false.</param>
		protected void ElementAdded(ModelElement element, bool addLinks)
		{
			Debug.Assert(false); // Direct call not support
			throw new NotImplementedException();
		}
		void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
		{
			ElementAdded(element, addLinks);
		}
		#endregion // INotifyElementAdded Implementation
		#region DeserializationFixupListener specific
		private Collection<ElementType> ElementCollection
		{
			get
			{
				Collection<ElementType> coll = myCollection;
				if (coll == null)
				{
					myCollection = coll = new Collection<ElementType>();
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
		protected abstract void ProcessElement(ElementType element, Store store, INotifyElementAdded notifyAdded);
		/// <summary>
		/// The phase is complete. Default implementation does nothing.
		/// </summary>
		/// <param name="store">The context store</param>
		protected virtual void PhaseCompleted(Store store)
		{
		}
		#endregion // DeserializationFixupListener specific
	}
	#endregion // DeserializationFixupListener class
}	