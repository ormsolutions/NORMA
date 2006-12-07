using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.Modeling
{
	partial class ModelingEventManager
	{
		#region ElementEventsBegun support
		private List<EventHandler<ElementEventsBegunEventArgs>> _elementEventsBegun;
		private List<EventHandler<ElementEventsBegunEventArgs>> GetElementEventsBegunHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementEventsBegunEventArgs>> elementEventsBegun = this._elementEventsBegun;
			if ((elementEventsBegun == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((elementEventsBegun = this._elementEventsBegun) == null)
					{
						this._elementEventsBegun = elementEventsBegun = new List<EventHandler<ElementEventsBegunEventArgs>>();
						this._store.EventManagerDirectory.ElementEventsBegun.Add(new EventHandler<ElementEventsBegunEventArgs>(this.HandleElementEventsBegun));
					}
				}
			}
			return elementEventsBegun;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementEventsBegunEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementEventsBegunEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementEventsBegunEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementEventsBegunEventArgs>(manager.GetElementEventsBegunHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementEventsBegunEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementEventsBegunEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementEventsBegunEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementEventsBegunEventArgs>(this.GetElementEventsBegunHandlers(action), handler, action);
			}
		}
		private void HandleElementEventsBegun(object sender, ElementEventsBegunEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<ElementEventsBegunEventArgs>(this._store, this._elementEventsBegun, sender, e);
		}
		#endregion // ElementEventsBegun support
		#region ElementEventsEnded support
		private List<EventHandler<ElementEventsEndedEventArgs>> _elementEventsEnded;
		private List<EventHandler<ElementEventsEndedEventArgs>> GetElementEventsEndedHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementEventsEndedEventArgs>> elementEventsEnded = this._elementEventsEnded;
			if ((elementEventsEnded == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((elementEventsEnded = this._elementEventsEnded) == null)
					{
						this._elementEventsEnded = elementEventsEnded = new List<EventHandler<ElementEventsEndedEventArgs>>();
						this._store.EventManagerDirectory.ElementEventsEnded.Add(new EventHandler<ElementEventsEndedEventArgs>(this.HandleElementEventsEnded));
					}
				}
			}
			return elementEventsEnded;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementEventsEndedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementEventsEndedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementEventsEndedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementEventsEndedEventArgs>(manager.GetElementEventsEndedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementEventsEndedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementEventsEndedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementEventsEndedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementEventsEndedEventArgs>(this.GetElementEventsEndedHandlers(action), handler, action);
			}
		}
		private void HandleElementEventsEnded(object sender, ElementEventsEndedEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<ElementEventsEndedEventArgs>(this._store, this._elementEventsEnded, sender, e);
		}
		#endregion // ElementEventsEnded support
		#region DeserializationBeginning support
		private List<EventHandler<EventArgs>> _deserializationBeginning;
		private List<EventHandler<EventArgs>> GetDeserializationBeginningHandlers(EventHandlerAction action)
		{
			List<EventHandler<EventArgs>> deserializationBeginning = this._deserializationBeginning;
			if ((deserializationBeginning == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((deserializationBeginning = this._deserializationBeginning) == null)
					{
						this._deserializationBeginning = deserializationBeginning = new List<EventHandler<EventArgs>>();
						this._store.EventManagerDirectory.DeserializationBeginning += new EventHandler<EventArgs>(this.HandleDeserializationBeginning);
					}
				}
			}
			return deserializationBeginning;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddDeserializationBeginningHandler(Store store, EventHandler<EventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveDeserializationBeginningHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveDeserializationBeginningHandler(Store store, EventHandler<EventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveDeserializationBeginningHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveDeserializationBeginningHandler(Store store, EventHandler<EventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<EventArgs>(manager.GetDeserializationBeginningHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddDeserializationBeginningHandler(EventHandler<EventArgs> handler)
		{
			this.AddOrRemoveDeserializationBeginningHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveDeserializationBeginningHandler(EventHandler<EventArgs> handler)
		{
			this.AddOrRemoveDeserializationBeginningHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveDeserializationBeginningHandler(EventHandler<EventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<EventArgs>(this.GetDeserializationBeginningHandlers(action), handler, action);
			}
		}
		private void HandleDeserializationBeginning(object sender, EventArgs e)
		{
			ModelingEventManager.InvokeHandlers<EventArgs>(this._store, this._deserializationBeginning, sender, e);
		}
		#endregion // DeserializationBeginning support
		#region DeserializationEnding support
		private List<EventHandler<DeserializationEndingEventArgs>> _deserializationEnding;
		private List<EventHandler<DeserializationEndingEventArgs>> GetDeserializationEndingHandlers(EventHandlerAction action)
		{
			List<EventHandler<DeserializationEndingEventArgs>> deserializationEnding = this._deserializationEnding;
			if ((deserializationEnding == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((deserializationEnding = this._deserializationEnding) == null)
					{
						this._deserializationEnding = deserializationEnding = new List<EventHandler<DeserializationEndingEventArgs>>();
						this._store.EventManagerDirectory.DeserializationEnding += new EventHandler<DeserializationEndingEventArgs>(this.HandleDeserializationEnding);
					}
				}
			}
			return deserializationEnding;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<DeserializationEndingEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<DeserializationEndingEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<DeserializationEndingEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<DeserializationEndingEventArgs>(manager.GetDeserializationEndingHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<DeserializationEndingEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<DeserializationEndingEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<DeserializationEndingEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<DeserializationEndingEventArgs>(this.GetDeserializationEndingHandlers(action), handler, action);
			}
		}
		private void HandleDeserializationEnding(object sender, DeserializationEndingEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<DeserializationEndingEventArgs>(this._store, this._deserializationEnding, sender, e);
		}
		#endregion // DeserializationEnding support
		#region TransactionBeginning support
		private List<EventHandler<TransactionBeginningEventArgs>> _transactionBeginning;
		private List<EventHandler<TransactionBeginningEventArgs>> GetTransactionBeginningHandlers(EventHandlerAction action)
		{
			List<EventHandler<TransactionBeginningEventArgs>> transactionBeginning = this._transactionBeginning;
			if ((transactionBeginning == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((transactionBeginning = this._transactionBeginning) == null)
					{
						this._transactionBeginning = transactionBeginning = new List<EventHandler<TransactionBeginningEventArgs>>();
						this._store.EventManagerDirectory.TransactionBeginning.Add(new EventHandler<TransactionBeginningEventArgs>(this.HandleTransactionBeginning));
					}
				}
			}
			return transactionBeginning;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<TransactionBeginningEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<TransactionBeginningEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<TransactionBeginningEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<TransactionBeginningEventArgs>(manager.GetTransactionBeginningHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<TransactionBeginningEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<TransactionBeginningEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<TransactionBeginningEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<TransactionBeginningEventArgs>(this.GetTransactionBeginningHandlers(action), handler, action);
			}
		}
		private void HandleTransactionBeginning(object sender, TransactionBeginningEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<TransactionBeginningEventArgs>(this._store, this._transactionBeginning, sender, e);
		}
		#endregion // TransactionBeginning support
		#region TransactionCommitted support
		private List<EventHandler<TransactionCommitEventArgs>> _transactionCommitted;
		private List<EventHandler<TransactionCommitEventArgs>> GetTransactionCommittedHandlers(EventHandlerAction action)
		{
			List<EventHandler<TransactionCommitEventArgs>> transactionCommitted = this._transactionCommitted;
			if ((transactionCommitted == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((transactionCommitted = this._transactionCommitted) == null)
					{
						this._transactionCommitted = transactionCommitted = new List<EventHandler<TransactionCommitEventArgs>>();
						this._store.EventManagerDirectory.TransactionCommitted.Add(new EventHandler<TransactionCommitEventArgs>(this.HandleTransactionCommitted));
					}
				}
			}
			return transactionCommitted;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<TransactionCommitEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<TransactionCommitEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<TransactionCommitEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<TransactionCommitEventArgs>(manager.GetTransactionCommittedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<TransactionCommitEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<TransactionCommitEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<TransactionCommitEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<TransactionCommitEventArgs>(this.GetTransactionCommittedHandlers(action), handler, action);
			}
		}
		private void HandleTransactionCommitted(object sender, TransactionCommitEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<TransactionCommitEventArgs>(this._store, this._transactionCommitted, sender, e);
		}
		#endregion // TransactionCommitted support
		#region TransactionRolledBack support
		private List<EventHandler<TransactionRollbackEventArgs>> _transactionRolledBack;
		private List<EventHandler<TransactionRollbackEventArgs>> GetTransactionRolledBackHandlers(EventHandlerAction action)
		{
			List<EventHandler<TransactionRollbackEventArgs>> transactionRolledBack = this._transactionRolledBack;
			if ((transactionRolledBack == null) && (action == EventHandlerAction.Add))
			{
				lock (this._lockObject)
				{
					if ((transactionRolledBack = this._transactionRolledBack) == null)
					{
						this._transactionRolledBack = transactionRolledBack = new List<EventHandler<TransactionRollbackEventArgs>>();
						this._store.EventManagerDirectory.TransactionRolledBack.Add(new EventHandler<TransactionRollbackEventArgs>(this.HandleTransactionRolledBack));
					}
				}
			}
			return transactionRolledBack;
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<TransactionRollbackEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<TransactionRollbackEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<TransactionRollbackEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<TransactionRollbackEventArgs>(manager.GetTransactionRolledBackHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<TransactionRollbackEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<TransactionRollbackEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<TransactionRollbackEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<TransactionRollbackEventArgs>(this.GetTransactionRolledBackHandlers(action), handler, action);
			}
		}
		private void HandleTransactionRolledBack(object sender, TransactionRollbackEventArgs e)
		{
			ModelingEventManager.InvokeHandlers<TransactionRollbackEventArgs>(this._store, this._transactionRolledBack, sender, e);
		}
		#endregion // TransactionRolledBack support
		#region ElementAdded support
		private List<EventHandler<ElementAddedEventArgs>> _elementAdded;
		private Dictionary<Guid, List<EventHandler<ElementAddedEventArgs>>> _elementAddedDictionary;
		private List<EventHandler<ElementAddedEventArgs>> GetElementAddedHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementAddedEventArgs>> elementAdded = this._elementAdded;
			if ((elementAdded == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementAddedHandlersStorage();
				elementAdded = this._elementAdded;
			}
			return elementAdded;
		}
		private Dictionary<Guid, List<EventHandler<ElementAddedEventArgs>>> GetElementAddedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<ElementAddedEventArgs>>> elementAddedDictionary = this._elementAddedDictionary;
			if ((elementAddedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementAddedHandlersStorage();
				elementAddedDictionary = this._elementAddedDictionary;
			}
			return elementAddedDictionary;
		}
		private void InitializeElementAddedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._elementAdded == null)
				{
					this._elementAdded = new List<EventHandler<ElementAddedEventArgs>>();
					this._elementAddedDictionary = new Dictionary<Guid, List<EventHandler<ElementAddedEventArgs>>>();
					this._store.EventManagerDirectory.ElementAdded.Add(new EventHandler<ElementAddedEventArgs>(this.HandleElementAdded));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementAddedEventArgs>(manager.GetElementAddedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementAddedEventArgs>(this.GetElementAddedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementAddedEventArgs>(manager.GetElementAddedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementAddedEventArgs>(this.GetElementAddedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementAddedEventArgs>(manager.GetElementAddedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementAddedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementAddedEventArgs>(this.GetElementAddedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		private void HandleElementAdded(object sender, ElementAddedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			ModelingEventManager.InvokeHandlers<ElementAddedEventArgs>(serviceProvider, this._elementAddedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<ElementAddedEventArgs>(serviceProvider, this._elementAdded, sender, e);
		}
		#endregion // ElementAdded support
		#region ElementDeleted support
		private List<EventHandler<ElementDeletedEventArgs>> _elementDeleted;
		private Dictionary<Guid, List<EventHandler<ElementDeletedEventArgs>>> _elementDeletedDictionary;
		private List<EventHandler<ElementDeletedEventArgs>> GetElementDeletedHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementDeletedEventArgs>> elementDeleted = this._elementDeleted;
			if ((elementDeleted == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementDeletedHandlersStorage();
				elementDeleted = this._elementDeleted;
			}
			return elementDeleted;
		}
		private Dictionary<Guid, List<EventHandler<ElementDeletedEventArgs>>> GetElementDeletedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<ElementDeletedEventArgs>>> elementDeletedDictionary = this._elementDeletedDictionary;
			if ((elementDeletedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementDeletedHandlersStorage();
				elementDeletedDictionary = this._elementDeletedDictionary;
			}
			return elementDeletedDictionary;
		}
		private void InitializeElementDeletedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._elementDeleted == null)
				{
					this._elementDeleted = new List<EventHandler<ElementDeletedEventArgs>>();
					this._elementDeletedDictionary = new Dictionary<Guid, List<EventHandler<ElementDeletedEventArgs>>>();
					this._store.EventManagerDirectory.ElementDeleted.Add(new EventHandler<ElementDeletedEventArgs>(this.HandleElementDeleted));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementDeletedEventArgs>(manager.GetElementDeletedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementDeletedEventArgs>(this.GetElementDeletedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(manager.GetElementDeletedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(this.GetElementDeletedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(manager.GetElementDeletedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(this.GetElementDeletedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, Guid elementId, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, Guid elementId, EventHandler<ElementDeletedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, Guid elementId, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(manager.GetElementDeletedHandlersDictionary(action), elementId, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(Guid elementId, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(Guid elementId, EventHandler<ElementDeletedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(Guid elementId, EventHandler<ElementDeletedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementDeletedEventArgs>(this.GetElementDeletedHandlersDictionary(action), elementId, handler, action);
			}
		}
		private void HandleElementDeleted(object sender, ElementDeletedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			ModelingEventManager.InvokeHandlers<ElementDeletedEventArgs>(serviceProvider, this._elementDeletedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<ElementDeletedEventArgs>(serviceProvider, this._elementDeleted, sender, e);
		}
		#endregion // ElementDeleted support
		#region ElementMoved support
		private List<EventHandler<ElementMovedEventArgs>> _elementMoved;
		private Dictionary<Guid, List<EventHandler<ElementMovedEventArgs>>> _elementMovedDictionary;
		private List<EventHandler<ElementMovedEventArgs>> GetElementMovedHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementMovedEventArgs>> elementMoved = this._elementMoved;
			if ((elementMoved == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementMovedHandlersStorage();
				elementMoved = this._elementMoved;
			}
			return elementMoved;
		}
		private Dictionary<Guid, List<EventHandler<ElementMovedEventArgs>>> GetElementMovedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<ElementMovedEventArgs>>> elementMovedDictionary = this._elementMovedDictionary;
			if ((elementMovedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementMovedHandlersStorage();
				elementMovedDictionary = this._elementMovedDictionary;
			}
			return elementMovedDictionary;
		}
		private void InitializeElementMovedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._elementMoved == null)
				{
					this._elementMoved = new List<EventHandler<ElementMovedEventArgs>>();
					this._elementMovedDictionary = new Dictionary<Guid, List<EventHandler<ElementMovedEventArgs>>>();
					this._store.EventManagerDirectory.ElementMoved.Add(new EventHandler<ElementMovedEventArgs>(this.HandleElementMoved));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementMovedEventArgs>(manager.GetElementMovedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementMovedEventArgs>(this.GetElementMovedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(manager.GetElementMovedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(this.GetElementMovedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(manager.GetElementMovedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(this.GetElementMovedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, Guid elementId, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, Guid elementId, EventHandler<ElementMovedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, Guid elementId, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(manager.GetElementMovedHandlersDictionary(action), elementId, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(Guid elementId, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(Guid elementId, EventHandler<ElementMovedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(Guid elementId, EventHandler<ElementMovedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementMovedEventArgs>(this.GetElementMovedHandlersDictionary(action), elementId, handler, action);
			}
		}
		private void HandleElementMoved(object sender, ElementMovedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			ModelingEventManager.InvokeHandlers<ElementMovedEventArgs>(serviceProvider, this._elementMovedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<ElementMovedEventArgs>(serviceProvider, this._elementMoved, sender, e);
		}
		#endregion // ElementMoved support
		#region ElementPropertyChanged support
		private List<EventHandler<ElementPropertyChangedEventArgs>> _elementPropertyChanged;
		private Dictionary<Guid, List<EventHandler<ElementPropertyChangedEventArgs>>> _elementPropertyChangedDictionary;
		private Dictionary<GuidPair, List<EventHandler<ElementPropertyChangedEventArgs>>> _elementPropertyChangedGuidPairDictionary;
		private List<EventHandler<ElementPropertyChangedEventArgs>> GetElementPropertyChangedHandlers(EventHandlerAction action)
		{
			List<EventHandler<ElementPropertyChangedEventArgs>> elementPropertyChanged = this._elementPropertyChanged;
			if ((elementPropertyChanged == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementPropertyChangedHandlersStorage();
				elementPropertyChanged = this._elementPropertyChanged;
			}
			return elementPropertyChanged;
		}
		private Dictionary<Guid, List<EventHandler<ElementPropertyChangedEventArgs>>> GetElementPropertyChangedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<ElementPropertyChangedEventArgs>>> elementPropertyChangedDictionary = this._elementPropertyChangedDictionary;
			if ((elementPropertyChangedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementPropertyChangedHandlersStorage();
				elementPropertyChangedDictionary = this._elementPropertyChangedDictionary;
			}
			return elementPropertyChangedDictionary;
		}
		private Dictionary<GuidPair, List<EventHandler<ElementPropertyChangedEventArgs>>> GetElementPropertyChangedHandlersGuidPairDictionary(EventHandlerAction action)
		{
			Dictionary<GuidPair, List<EventHandler<ElementPropertyChangedEventArgs>>> elementPropertyChangedGuidPairDictionary = this._elementPropertyChangedGuidPairDictionary;
			if ((elementPropertyChangedGuidPairDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeElementPropertyChangedHandlersStorage();
				elementPropertyChangedGuidPairDictionary = this._elementPropertyChangedGuidPairDictionary;
			}
			return elementPropertyChangedGuidPairDictionary;
		}
		private void InitializeElementPropertyChangedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._elementPropertyChanged == null)
				{
					this._elementPropertyChanged = new List<EventHandler<ElementPropertyChangedEventArgs>>();
					this._elementPropertyChangedDictionary = new Dictionary<Guid, List<EventHandler<ElementPropertyChangedEventArgs>>>();
					this._elementPropertyChangedGuidPairDictionary = new Dictionary<GuidPair, List<EventHandler<ElementPropertyChangedEventArgs>>>();
					this._store.EventManagerDirectory.ElementPropertyChanged.Add(new EventHandler<ElementPropertyChangedEventArgs>(this.HandleElementPropertyChanged));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersDictionary(action), elementId, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersDictionary(action), elementId, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainProperty, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainProperty, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersDictionary(action), domainProperty.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainProperty, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainProperty, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersDictionary(action), domainProperty.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, domainProperty, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, domainProperty, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<GuidPair, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersGuidPairDictionary(action), new GuidPair(domainClass.Id, domainProperty.Id), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, domainProperty, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, domainProperty, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, DomainPropertyInfo domainProperty, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<GuidPair, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersGuidPairDictionary(action), new GuidPair(domainClass.Id, domainProperty.Id), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainProperty, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainProperty, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<GuidPair, ElementPropertyChangedEventArgs>(manager.GetElementPropertyChangedHandlersGuidPairDictionary(action), new GuidPair(domainProperty.Id, elementId), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainProperty, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainProperty, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainProperty">The <paramref name="domainProperty"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainProperty"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainPropertyInfo domainProperty, Guid elementId, EventHandler<ElementPropertyChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<GuidPair, ElementPropertyChangedEventArgs>(this.GetElementPropertyChangedHandlersGuidPairDictionary(action), new GuidPair(domainProperty.Id, elementId), handler, action);
			}
		}
		private void HandleElementPropertyChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			List<EventHandler<ElementPropertyChangedEventArgs>> handlers;
			Guid domainProperty = e.DomainProperty.Id;
			DomainClassInfo domainClass = e.DomainClass;
			Dictionary<GuidPair, List<EventHandler<ElementPropertyChangedEventArgs>>> elementPropertyChangedGuidPairDictionary = this._elementPropertyChangedGuidPairDictionary;
			do
			{
				if (elementPropertyChangedGuidPairDictionary.TryGetValue(new GuidPair(domainClass.Id, domainProperty), out handlers))
				{
					ModelingEventManager.InvokeHandlers<ElementPropertyChangedEventArgs>(serviceProvider, handlers, sender, e);
				}
			} while ((domainClass = domainClass.BaseDomainClass) != null);
			if (elementPropertyChangedGuidPairDictionary.TryGetValue(new GuidPair(domainProperty, e.ElementId), out handlers))
			{
				ModelingEventManager.InvokeHandlers<ElementPropertyChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			Dictionary<Guid, List<EventHandler<ElementPropertyChangedEventArgs>>> elementPropertyChangedDictionary = this._elementPropertyChangedDictionary;
			if (elementPropertyChangedDictionary.TryGetValue(domainProperty, out handlers))
			{
				ModelingEventManager.InvokeHandlers<ElementPropertyChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			ModelingEventManager.InvokeHandlers<ElementPropertyChangedEventArgs>(serviceProvider, elementPropertyChangedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<ElementPropertyChangedEventArgs>(serviceProvider, this._elementPropertyChanged, sender, e);
		}
		#endregion // ElementPropertyChanged support
		#region RolePlayerChanged support
		private List<EventHandler<RolePlayerChangedEventArgs>> _rolePlayerChanged;
		private Dictionary<Guid, List<EventHandler<RolePlayerChangedEventArgs>>> _rolePlayerChangedDictionary;
		private Dictionary<GuidPair, List<EventHandler<RolePlayerChangedEventArgs>>> _rolePlayerChangedGuidPairDictionary;
		private List<EventHandler<RolePlayerChangedEventArgs>> GetRolePlayerChangedHandlers(EventHandlerAction action)
		{
			List<EventHandler<RolePlayerChangedEventArgs>> rolePlayerChanged = this._rolePlayerChanged;
			if ((rolePlayerChanged == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerChangedHandlersStorage();
				rolePlayerChanged = this._rolePlayerChanged;
			}
			return rolePlayerChanged;
		}
		private Dictionary<Guid, List<EventHandler<RolePlayerChangedEventArgs>>> GetRolePlayerChangedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<RolePlayerChangedEventArgs>>> rolePlayerChangedDictionary = this._rolePlayerChangedDictionary;
			if ((rolePlayerChangedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerChangedHandlersStorage();
				rolePlayerChangedDictionary = this._rolePlayerChangedDictionary;
			}
			return rolePlayerChangedDictionary;
		}
		private Dictionary<GuidPair, List<EventHandler<RolePlayerChangedEventArgs>>> GetRolePlayerChangedHandlersGuidPairDictionary(EventHandlerAction action)
		{
			Dictionary<GuidPair, List<EventHandler<RolePlayerChangedEventArgs>>> rolePlayerChangedGuidPairDictionary = this._rolePlayerChangedGuidPairDictionary;
			if ((rolePlayerChangedGuidPairDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerChangedHandlersStorage();
				rolePlayerChangedGuidPairDictionary = this._rolePlayerChangedGuidPairDictionary;
			}
			return rolePlayerChangedGuidPairDictionary;
		}
		private void InitializeRolePlayerChangedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._rolePlayerChanged == null)
				{
					this._rolePlayerChanged = new List<EventHandler<RolePlayerChangedEventArgs>>();
					this._rolePlayerChangedDictionary = new Dictionary<Guid, List<EventHandler<RolePlayerChangedEventArgs>>>();
					this._rolePlayerChangedGuidPairDictionary = new Dictionary<GuidPair, List<EventHandler<RolePlayerChangedEventArgs>>>();
					this._store.EventManagerDirectory.RolePlayerChanged.Add(new EventHandler<RolePlayerChangedEventArgs>(this.HandleRolePlayerChanged));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlersDictionary(action), elementId, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(Guid elementId, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlersDictionary(action), elementId, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainRole, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainRole, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlersDictionary(action), domainRole.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainRole, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainRole, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainRoleInfo domainRole, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlersDictionary(action), domainRole.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainRole, elementLinkId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainRole, elementLinkId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<GuidPair, RolePlayerChangedEventArgs>(manager.GetRolePlayerChangedHandlersGuidPairDictionary(action), new GuidPair(domainRole.Id, elementLinkId), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainRole, elementLinkId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainRole, elementLinkId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainRole">The <paramref name="domainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementLinkId">The <paramref name="elementLinkId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainRoleInfo domainRole, Guid elementLinkId, EventHandler<RolePlayerChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<GuidPair, RolePlayerChangedEventArgs>(this.GetRolePlayerChangedHandlersGuidPairDictionary(action), new GuidPair(domainRole.Id, elementLinkId), handler, action);
			}
		}
		private void HandleRolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			List<EventHandler<RolePlayerChangedEventArgs>> handlers;
			Guid domainRole = e.DomainRole.Id;
			if (this._rolePlayerChangedGuidPairDictionary.TryGetValue(new GuidPair(domainRole, e.ElementLinkId), out handlers))
			{
				ModelingEventManager.InvokeHandlers<RolePlayerChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			Dictionary<Guid, List<EventHandler<RolePlayerChangedEventArgs>>> rolePlayerChangedDictionary = this._rolePlayerChangedDictionary;
			if (rolePlayerChangedDictionary.TryGetValue(domainRole, out handlers))
			{
				ModelingEventManager.InvokeHandlers<RolePlayerChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			ModelingEventManager.InvokeHandlers<RolePlayerChangedEventArgs>(serviceProvider, rolePlayerChangedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<RolePlayerChangedEventArgs>(serviceProvider, this._rolePlayerChanged, sender, e);
		}
		#endregion // RolePlayerChanged support
		#region RolePlayerOrderChanged support
		private List<EventHandler<RolePlayerOrderChangedEventArgs>> _rolePlayerOrderChanged;
		private Dictionary<Guid, List<EventHandler<RolePlayerOrderChangedEventArgs>>> _rolePlayerOrderChangedDictionary;
		private Dictionary<GuidPair, List<EventHandler<RolePlayerOrderChangedEventArgs>>> _rolePlayerOrderChangedGuidPairDictionary;
		private List<EventHandler<RolePlayerOrderChangedEventArgs>> GetRolePlayerOrderChangedHandlers(EventHandlerAction action)
		{
			List<EventHandler<RolePlayerOrderChangedEventArgs>> rolePlayerOrderChanged = this._rolePlayerOrderChanged;
			if ((rolePlayerOrderChanged == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerOrderChangedHandlersStorage();
				rolePlayerOrderChanged = this._rolePlayerOrderChanged;
			}
			return rolePlayerOrderChanged;
		}
		private Dictionary<Guid, List<EventHandler<RolePlayerOrderChangedEventArgs>>> GetRolePlayerOrderChangedHandlersDictionary(EventHandlerAction action)
		{
			Dictionary<Guid, List<EventHandler<RolePlayerOrderChangedEventArgs>>> rolePlayerOrderChangedDictionary = this._rolePlayerOrderChangedDictionary;
			if ((rolePlayerOrderChangedDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerOrderChangedHandlersStorage();
				rolePlayerOrderChangedDictionary = this._rolePlayerOrderChangedDictionary;
			}
			return rolePlayerOrderChangedDictionary;
		}
		private Dictionary<GuidPair, List<EventHandler<RolePlayerOrderChangedEventArgs>>> GetRolePlayerOrderChangedHandlersGuidPairDictionary(EventHandlerAction action)
		{
			Dictionary<GuidPair, List<EventHandler<RolePlayerOrderChangedEventArgs>>> rolePlayerOrderChangedGuidPairDictionary = this._rolePlayerOrderChangedGuidPairDictionary;
			if ((rolePlayerOrderChangedGuidPairDictionary == null) && (action == EventHandlerAction.Add))
			{
				this.InitializeRolePlayerOrderChangedHandlersStorage();
				rolePlayerOrderChangedGuidPairDictionary = this._rolePlayerOrderChangedGuidPairDictionary;
			}
			return rolePlayerOrderChangedGuidPairDictionary;
		}
		private void InitializeRolePlayerOrderChangedHandlersStorage()
		{
			lock (this._lockObject)
			{
				if (this._rolePlayerOrderChanged == null)
				{
					this._rolePlayerOrderChanged = new List<EventHandler<RolePlayerOrderChangedEventArgs>>();
					this._rolePlayerOrderChangedDictionary = new Dictionary<Guid, List<EventHandler<RolePlayerOrderChangedEventArgs>>>();
					this._rolePlayerOrderChangedGuidPairDictionary = new Dictionary<GuidPair, List<EventHandler<RolePlayerOrderChangedEventArgs>>>();
					this._store.EventManagerDirectory.RolePlayerOrderChanged.Add(new EventHandler<RolePlayerOrderChangedEventArgs>(this.HandleRolePlayerOrderChanged));
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlers(action), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlers(action), handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlersDictionary(action), domainClass.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainClass, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainClass">The <paramref name="domainClass"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainClass"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainClassInfo domainClass, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainClass == null)
			{
				throw new ArgumentNullException("domainClass");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlersDictionary(action), domainClass.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlersDictionary(action), domainModel.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(domainModel, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="domainModel">The <paramref name="domainModel"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="domainModel"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainModelInfo domainModel, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (domainModel == null)
			{
				throw new ArgumentNullException("domainModel");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlersDictionary(action), domainModel.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void AddHandler(Store store, Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static void RemoveHandler(Store store, Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlersDictionary(action), elementId, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		public void AddHandler(Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		public void RemoveHandler(Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(elementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="elementId">The <paramref name="elementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(Guid elementId, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlersDictionary(action), elementId, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, counterpartDomainRole, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, counterpartDomainRole, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (counterpartDomainRole == null)
			{
				throw new ArgumentNullException("counterpartDomainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlersDictionary(action), counterpartDomainRole.Id, handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(counterpartDomainRole, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(counterpartDomainRole, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainRoleInfo counterpartDomainRole, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (counterpartDomainRole == null)
			{
				throw new ArgumentNullException("counterpartDomainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<Guid, RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlersDictionary(action), counterpartDomainRole.Id, handler, action);
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public static void AddHandler(Store store, DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, counterpartDomainRole, sourceElementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public static void RemoveHandler(Store store, DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			ModelingEventManager.AddOrRemoveHandler(store, counterpartDomainRole, sourceElementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.</summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void AddOrRemoveHandler(Store store, DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (counterpartDomainRole == null)
			{
				throw new ArgumentNullException("counterpartDomainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager manager = ModelingEventManager.GetModelingEventManager(store, action);
				if (manager != null)
				{
					ModelingEventManager.AddOrRemoveHandler<GuidPair, RolePlayerOrderChangedEventArgs>(manager.GetRolePlayerOrderChangedHandlersGuidPairDictionary(action), new GuidPair(counterpartDomainRole.Id, sourceElementId), handler, action);
				}
			}
		}
		/// <summary>Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public void AddHandler(DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(counterpartDomainRole, sourceElementId, handler, EventHandlerAction.Add);
		}
		/// <summary>Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be removed.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		public void RemoveHandler(DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler)
		{
			this.AddOrRemoveHandler(counterpartDomainRole, sourceElementId, handler, EventHandlerAction.Remove);
		}
		/// <summary>Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
		/// as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.</summary>
		/// <param name="counterpartDomainRole">The <paramref name="counterpartDomainRole"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="sourceElementId">The <paramref name="sourceElementId"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.</param>
		/// <param name="handler">The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
		/// specified by <paramref name="handler"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="counterpartDomainRole"/> is <see langword="null"/>.</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void AddOrRemoveHandler(DomainRoleInfo counterpartDomainRole, Guid sourceElementId, EventHandler<RolePlayerOrderChangedEventArgs> handler, EventHandlerAction action)
		{
			if (counterpartDomainRole == null)
			{
				throw new ArgumentNullException("counterpartDomainRole");
			}
			if ((object)handler != null)
			{
				ModelingEventManager.AddOrRemoveHandler<GuidPair, RolePlayerOrderChangedEventArgs>(this.GetRolePlayerOrderChangedHandlersGuidPairDictionary(action), new GuidPair(counterpartDomainRole.Id, sourceElementId), handler, action);
			}
		}
		private void HandleRolePlayerOrderChanged(object sender, RolePlayerOrderChangedEventArgs e)
		{
			IServiceProvider serviceProvider = this._store;
			List<EventHandler<RolePlayerOrderChangedEventArgs>> handlers;
			Guid counterpartDomainRole = e.CounterpartDomainRole.Id;
			if (this._rolePlayerOrderChangedGuidPairDictionary.TryGetValue(new GuidPair(counterpartDomainRole, e.SourceElementId), out handlers))
			{
				ModelingEventManager.InvokeHandlers<RolePlayerOrderChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			Dictionary<Guid, List<EventHandler<RolePlayerOrderChangedEventArgs>>> rolePlayerOrderChangedDictionary = this._rolePlayerOrderChangedDictionary;
			if (rolePlayerOrderChangedDictionary.TryGetValue(counterpartDomainRole, out handlers))
			{
				ModelingEventManager.InvokeHandlers<RolePlayerOrderChangedEventArgs>(serviceProvider, handlers, sender, e);
			}
			ModelingEventManager.InvokeHandlers<RolePlayerOrderChangedEventArgs>(serviceProvider, rolePlayerOrderChangedDictionary, sender, e);
			ModelingEventManager.InvokeHandlers<RolePlayerOrderChangedEventArgs>(serviceProvider, this._rolePlayerOrderChanged, sender, e);
		}
		#endregion // RolePlayerOrderChanged support
	}
}
