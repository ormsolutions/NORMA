#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;

namespace Neumont.Tools.Modeling
{
	#region IModelingEventSubscriber
	/// <summary>
	/// This interface provides needed methods that are required to add Events to the Object Model.
	/// </summary>
	public interface IModelingEventSubscriber
	{
		/// <summary>
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called before the document is loaded and when <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="isReload">
		/// The model is being reloaded.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action);
		/// <summary>
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called after the document is loaded and when <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="isReload">
		/// The model is being reloaded.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action);
		/// <summary>
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called when survey questions (used to load the model browser) are required and when
		/// <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="isReload">
		/// The model is being reloaded.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action);
	}
	#endregion // IModelingEventSubscriber
	#region EventHandlerAction enum
	/// <summary>
	/// Indicates the action that should be taken for an <see cref="EventHandler{TEventArgs}"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public enum EventHandlerAction
	{
		/// <summary>
		/// Indicates that the <see cref="EventHandler{TEventArgs}"/> should be added as a handler.
		/// </summary>
		Add = 0,
		/// <summary>
		/// Indicates that the <see cref="EventHandler{TEventArgs}"/> should be removed as a handler.
		/// </summary>
		Remove = 1
	}
	#endregion // EventHandlerAction enum
	#region IModelingEventManagerProvider interface
	/// <summary>
	/// An interface to implement on a Store to provide
	/// a <see cref="ModelingEventManager"/> class for the store.
	/// </summary>
	public interface IModelingEventManagerProvider
	{
		/// <summary>
		/// Return the <see cref="ModelingEventManager"/> corresponding
		/// to the current store.
		/// </summary>
		ModelingEventManager ModelingEventManager { get;}
	}
	#endregion // ISafeEventManagerProvider interface
	#region ModelingEventManager class
	/// <summary>
	/// An event manager used to protected the state of
	/// event listeners from event handlers that throw exceptions.
	/// Event handlers should not throw exceptions, but it is
	/// too risky to chance breaking the VS session because
	/// of one non-compliant event handler.
	/// </summary>
	public abstract class ModelingEventManager
	{
		#region TypeAndDomainObjectKey struct
		private struct TypeAndDomainObjectKey
		{
			private Type myHandlerType;
			private DomainObjectInfo myInfoIdentifier1;
			private DomainObjectInfo myInfoIdentifier2;
			public TypeAndDomainObjectKey(Type handlerType, DomainObjectInfo infoIdentifier1, DomainObjectInfo infoIdentifier2)
			{
				Debug.Assert(typeof(ModelingEventArgs).IsAssignableFrom(handlerType), "Not a valid event handler type");
				myHandlerType = handlerType;
				myInfoIdentifier1 = infoIdentifier1;
				myInfoIdentifier2 = infoIdentifier2;
			}
			public override int GetHashCode()
			{
				int retVal = myHandlerType.GetHashCode();
				if (myInfoIdentifier1 != null)
				{
					retVal ^= RotateRight(myInfoIdentifier1.GetHashCode(), 1);
				}
				if (myInfoIdentifier2 != null)
				{
					retVal ^= RotateRight(myInfoIdentifier2.GetHashCode(), 2);
				}
				return retVal;
			}
			private static int RotateRight(int value, int places)
			{
				places = places & 0x1F;
				if (places == 0)
				{
					return value;
				}
				int mask = ~0x7FFFFFF >> (places - 1);
				return ((value >> places) & ~mask) | ((value << (32 - places)) & mask);
			}
		}
		#endregion // TypeAndDomainObjectKey struct
		#region EventHandlerWrapper class (abstract base and generic versions)
		private abstract class EventHandlerWrapper
		{
			protected EventHandlerWrapper()
			{
			}
			public abstract Delegate InnerHandler { get;set;}
		}
		private class EventHandlerWrapper<TEventArgs> : EventHandlerWrapper where TEventArgs : ModelingEventArgs
		{
			private EventHandler<TEventArgs> myInnerHandler;
			private ModelingEventManager myEventManager;
			public EventHandlerWrapper(EventHandler<TEventArgs> handler, ModelingEventManager eventManager)
			{
				myInnerHandler = handler;
				myEventManager = eventManager;
			}
			/// <summary>
			/// Get the inner handler so that it can be directly managed without
			/// changing the wrapped element in the dictionary.
			/// </summary>
			public override Delegate InnerHandler
			{
				get
				{
					return myInnerHandler;
				}
				set
				{
					myInnerHandler = (EventHandler<TEventArgs>)value;
				}
			}
			/// <summary>
			/// The public method to handle the event fired by the store.
			/// </summary>
			public void Handler(object sender, TEventArgs e)
			{
				Delegate[] targets = myInnerHandler.GetInvocationList();
				for (int i = 0; i < targets.Length; ++i)
				{
					try
					{
						((EventHandler<TEventArgs>)targets[i]).Invoke(sender, e);
					}
					catch (Exception ex)
					{
						if (myEventManager.OnException(ex))
						{
							throw;
						}
					}
				}
			}
		}
		#endregion // EventHandlerWrapper class (abstract base and generic versions)
		#region Member Variables
		private Store myStore;
		private Dictionary<TypeAndDomainObjectKey, EventHandlerWrapper> myDictionary;
		private EventHandler<ElementEventsBegunEventArgs> myRegisteredBeginningEvents;
		private EventHandler<ElementEventsEndedEventArgs> myRegisteredEndedEvents;
		private Exception myPendingException;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a new ModelingEventManager class. Used to ensure that all events fire,
		/// even if an event throws in the middle.
		/// </summary>
		/// <param name="store">The store to attach to</param>
		protected ModelingEventManager(Store store)
		{
			myStore = store;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			eventDirectory.ElementEventsBegun.Add(new EventHandler<ElementEventsBegunEventArgs>(EventsBeginning));
			eventDirectory.ElementEventsEnded.Add(new EventHandler<ElementEventsEndedEventArgs>(EventsEnding));
			myDictionary = new Dictionary<TypeAndDomainObjectKey, EventHandlerWrapper>();
		}
		#endregion // Constructors
		#region Helper methods
		/// <summary>
		/// Return a <see cref="ModelingEventManager"/> associated with the store implementation
		/// </summary>
		/// <param name="store">The <see cref="Store"/> object. Must implement <see cref="IModelingEventManagerProvider"/></param>
		/// <returns>Associated <see cref="ModelingEventManager"/></returns>
		public static ModelingEventManager GetModelingEventManager(Store store)
		{
			return ((IModelingEventManagerProvider)store).ModelingEventManager;
		}
		#endregion // Helper methods
		#region Abstract methods
		/// <summary>
		/// A virtual function to allow different displays of exception messages
		/// </summary>
		/// <param name="ex"></param>
		protected abstract void DisplayException(Exception ex);
		#endregion // Abstract methods
		#region ElementEventsBegun/ElementEventsEnded Functions
		/// <summary>
		/// Track a thrown exception to display later
		/// </summary>
		/// <param name="exception">The thrown exception</param>
		/// <returns>true to rethrow the exception, false to swallow it</returns>
		private bool OnException(Exception exception)
		{
			if (Utility.IsCriticalException(exception))
			{
				return true;
			}
			if (myPendingException == null)
			{
				myPendingException = exception;
			}
			return false;
		}
		private void EventsBeginning(object sender, ElementEventsBegunEventArgs e)
		{
			myPendingException = null;
			EventHandler<ElementEventsBegunEventArgs> beginningEvents = myRegisteredBeginningEvents;
			if (beginningEvents != null)
			{
				beginningEvents(sender, e);
			}
		}
		private void EventsEnding(object sender, ElementEventsEndedEventArgs e)
		{
			EventHandler<ElementEventsEndedEventArgs> endedEvents = myRegisteredEndedEvents;
			if (endedEvents != null)
			{
				endedEvents(sender, e);
			}
			Exception ex = myPendingException;
			if (ex != null)
			{
				myPendingException = null;
				DisplayException(ex);
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementEventsBegun"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(EventHandler<ElementEventsBegunEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementEventsBegunEventArgs>(handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myRegisteredBeginningEvents = (EventHandler<ElementEventsBegunEventArgs>)wrapperDelegate;
				}
				else
				{
					myRegisteredBeginningEvents = null;
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementEventsEnded"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(EventHandler<ElementEventsEndedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementEventsEndedEventArgs>(handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myRegisteredEndedEvents = (EventHandler<ElementEventsEndedEventArgs>)wrapperDelegate;
				}
				else
				{
					myRegisteredEndedEvents = null;
				}

			}
		}
		#endregion // ElementEventsBegun/ElementEventsEnded Functions
		#region Typed AddOrRemove Functions
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementAdded"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementAddedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementAdded.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementAdded.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementAdded"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementAddedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementAdded.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementAdded.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementDeleted"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementDeletedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementDeleted.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementDeleted.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementDeleted"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementDeletedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementDeleted.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementDeleted.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementMoved"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementMovedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementMoved.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementMoved.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementMoved"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementMovedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementMoved.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementMoved.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementPropertyChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementPropertyChangedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementPropertyChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementPropertyChangedEventArgs>(domainClass, domainProperty, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Add(domainClass, domainProperty, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Remove(domainClass, domainProperty, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementPropertyChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementPropertyChangedEventArgs>(domainProperty, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Add(domainProperty, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Remove(domainProperty, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.ElementPropertyChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<ElementPropertyChangedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.ElementPropertyChanged.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerChangedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerChangedEventArgs>(domainRole, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Add(domainRole, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Remove(domainRole, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerChangedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerChanged.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerOrderChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerOrderChangedEventArgs>(domainClass, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Add(domainClass, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Remove(domainClass, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerOrderChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerOrderChangedEventArgs>(counterpartDomainRole, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Add(counterpartDomainRole, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Remove(counterpartDomainRole, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.RolePlayerOrderChanged"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<RolePlayerOrderChangedEventArgs>(domainModel, handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Add(domainModel, wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.RolePlayerOrderChanged.Remove(domainModel, wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.TransactionBeginning"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(EventHandler<TransactionBeginningEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<TransactionBeginningEventArgs>(handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.TransactionBeginning.Add(wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.TransactionBeginning.Remove(wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.TransactionCommitted"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(EventHandler<TransactionCommitEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<TransactionCommitEventArgs>(handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.TransactionCommitted.Add(wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.TransactionCommitted.Remove(wrapperDelegate);
				}
			}
		}
		/// <summary>
		/// Add or remove an event handler. Use in place of the Add and Remove methods available through
		/// <see cref="EventManagerDirectory.TransactionRolledBack"/>. Set the addHandler parameter to true for an Add
		/// and false for a remove.
		/// </summary>
		public void AddOrRemoveHandler(EventHandler<TransactionRollbackEventArgs> handler, EventHandlerAction action)
		{
			bool addHandler = action == EventHandlerAction.Add;
			Delegate wrapperDelegate = AddOrRemove<TransactionRollbackEventArgs>(handler, addHandler);
			if (wrapperDelegate != null)
			{
				if (addHandler)
				{
					myStore.EventManagerDirectory.TransactionRolledBack.Add(wrapperDelegate);
				}
				else
				{
					myStore.EventManagerDirectory.TransactionRolledBack.Remove(wrapperDelegate);
				}
			}
		}
		#endregion // Typed AddOrRemove Functions
		#region Generic AddOrRemove Helper Functions
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), null, null), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="domainClass">The domainClass to add</param>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(DomainClassInfo domainClass, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), domainClass, null), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="domainClass">The domainClass to add</param>
		/// <param name="domainProperty">The domainProperty to add</param>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), domainClass, domainProperty), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="domainProperty">The domainProperty to add</param>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(DomainPropertyInfo domainProperty, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), domainProperty, null), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="domainRole">The domainRole to add</param>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(DomainRoleInfo domainRole, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), domainRole, null), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="domainModel">The domainModel to add</param>
		/// <param name="handler">The event to add or remove</param>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(DomainModelInfo domainModel, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			return AddOrRemove<TEventArgs>(new TypeAndDomainObjectKey(typeof(TEventArgs), domainModel, null), handler, addHandler);
		}
		/// <summary>
		/// Add or remove the provided handler
		/// </summary>
		/// <typeparam name="TEventArgs">The type of the handler</typeparam>
		/// <param name="addHandler">true to add an event listener, false to remove one</param>
		/// <param name="key">The key to the current handler</param>
		/// <param name="handler">The event to add or remove</param>
		/// <returns>Returns non-null if the wrapper needs to be added or removed</returns>
		private Delegate AddOrRemove<TEventArgs>(TypeAndDomainObjectKey key, EventHandler<TEventArgs> handler, bool addHandler) where TEventArgs : ModelingEventArgs
		{
			Delegate retVal = null;
			EventHandlerWrapper currentWrapper = null;
			if (myDictionary.TryGetValue(key, out currentWrapper))
			{
				if (addHandler)
				{
					currentWrapper.InnerHandler = MulticastDelegate.Combine(currentWrapper.InnerHandler, handler);
				}
				else
				{
					Delegate newDelegate = MulticastDelegate.Remove(currentWrapper.InnerHandler, handler);
					currentWrapper.InnerHandler = newDelegate;
					if (newDelegate == null)
					{
						myDictionary.Remove(key);
						retVal = new EventHandler<TEventArgs>(((EventHandlerWrapper<TEventArgs>)currentWrapper).Handler);
					}
				}
			}
			else
			{
				EventHandlerWrapper<TEventArgs> wrappedHandler = new EventHandlerWrapper<TEventArgs>(handler, this);
				myDictionary.Add(key, wrappedHandler);
				retVal = new EventHandler<TEventArgs>(wrappedHandler.Handler);
			}
			return retVal;
		}
		#endregion // Generic AddOrRemove Helper Functions
	}
	#endregion // ModelingEventManager class
}