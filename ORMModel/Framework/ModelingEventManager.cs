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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.Modeling
{
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

	#region ModelingEventManager class
	/// <summary>
	/// Provides a more reliable and performant alternative to <see cref="EventManagerDirectory"/> for the purpose
	/// of managing <see cref="EventHandler{TEventArgs}"/> subscriptions and invocations for a <see cref="Store"/>.
	/// </summary>
	public sealed partial class ModelingEventManager
	{
		#region ModelingEventManagerKeyProvider class
		[Serializable]
		private sealed class ModelingEventManagerKeyProvider : IKeyProvider<Store, ModelingEventManager>
		{
			private ModelingEventManagerKeyProvider()
				: base()
			{
			}
			public static readonly ModelingEventManagerKeyProvider Instance = new ModelingEventManagerKeyProvider();
			public Store GetKey(ModelingEventManager value)
			{
				return value._store;
			}
		}
		#endregion // ModelingEventManagerKeyProvider class

		// We lock on a separate object rather than locking on 'this' to prevent deadlocks in case other code
		// locks on instances of this class.
		private readonly object _lockObject = new object();
		private readonly Store _store;
		private ModelingEventManager(Store store)
			: base()
		{
			this._store = store;
			store.StoreDisposing += ModelingEventManager.StoreDisposingEventHandler;
		}

		#region Store/ModelingEventManager storage
		private static readonly HashSet<Store, ModelingEventManager> _modelingEventManagers = new HashSet<Store, ModelingEventManager>(ModelingEventManagerKeyProvider.Instance);

		private static readonly EventHandler StoreDisposingEventHandler = new EventHandler(ModelingEventManager.HandleStoreDisposing);
		private static void HandleStoreDisposing(object sender, EventArgs e)
		{
			Store store = (Store)sender;
			HashSet<Store, ModelingEventManager> modelingEventManagers = ModelingEventManager._modelingEventManagers;
			lock (modelingEventManagers)
			{
				modelingEventManagers.RemoveAll(store);
			}
			store.StoreDisposing -= ModelingEventManager.StoreDisposingEventHandler;
		}

		private static ModelingEventManager GetModelingEventManager(Store store, EventHandlerAction action)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			if (store.Disposed)
			{
				throw new ObjectDisposedException(store.GetType().Name);
			}
			HashSet<Store, ModelingEventManager> modelingEventManagers = ModelingEventManager._modelingEventManagers;
			ModelingEventManager modelingEventManager = modelingEventManagers.FindAnyValue(store, null);
			if (modelingEventManager == null && action == EventHandlerAction.Add)
			{
				lock (modelingEventManagers)
				{
					modelingEventManager = modelingEventManagers.FindAnyValue(store, null);
					if (modelingEventManager == null)
					{
						modelingEventManagers[store] = modelingEventManager = new ModelingEventManager(store);
						store.StoreDisposing += ModelingEventManager.StoreDisposingEventHandler;
					}
				}
			}
			return modelingEventManager;
		}

		/// <summary>
		/// Retrieves the <see cref="ModelingEventManager"/> for the <see cref="Store"/> specified by <paramref name="store"/>.
		/// </summary>
		/// <param name="store">
		/// The <see cref="Store"/> for which the <see cref="ModelingEventManager"/> should be retrieved.
		/// </param>
		/// <returns>
		/// The <see cref="ModelingEventManager"/> for the <see cref="Store"/> specified by <paramref name="store"/>.
		/// </returns>
		/// <remarks>
		/// If no <see cref="ModelingEventManager"/> exists for <paramref name="store"/>, a new <see cref="ModelingEventManager"/>
		/// will be instantiated.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="store"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException"><paramref name="store"/> has been disposed.</exception>
		public static ModelingEventManager GetModelingEventManager(Store store)
		{
			return GetModelingEventManager(store, EventHandlerAction.Add);
		}
		#endregion // Store/ModelingEventManager storage

		#region GuidPair struct
		[Serializable]
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
		private struct GuidPair : IEquatable<GuidPair>
		{
			public GuidPair(Guid guid1, Guid guid2)
			{
				this.Guid1 = guid1;
				this.Guid2 = guid2;
			}
			public readonly Guid Guid1;
			public readonly Guid Guid2;
			public override bool Equals(object obj)
			{
				return obj is GuidPair && this.Equals((GuidPair)obj);
			}
			public bool Equals(GuidPair other)
			{
				return this.Guid1.Equals(other.Guid1) && this.Guid2.Equals(other.Guid2);
			}
			public override int GetHashCode()
			{
				return this.Guid1.GetHashCode() ^ this.Guid2.GetHashCode();
			}
		}
		#endregion // GuidPair struct

		#region Static helper methods
		private static void AddOrRemoveHandler<TEventArgs>(List<EventHandler<TEventArgs>> handlers, EventHandler<TEventArgs> handler, EventHandlerAction action)
			where TEventArgs : EventArgs
		{
			if (handlers != null)
			{
				switch (action)
				{
					case EventHandlerAction.Add:
						handlers.Add(handler);
						break;
					case EventHandlerAction.Remove:
						handlers.Remove(handler);
						break;
					default:
						throw new InvalidEnumArgumentException("action", (int)action, typeof(EventHandlerAction));
				}
			}
		}

		private static void AddOrRemoveHandler<TKey, TEventArgs>(Dictionary<TKey, List<EventHandler<TEventArgs>>> handlersDictionary, TKey key, EventHandler<TEventArgs> handler, EventHandlerAction action)
			where TKey : struct
			where TEventArgs : EventArgs
		{
			if (handlersDictionary != null)
			{
				List<EventHandler<TEventArgs>> handlers;
				if (!handlersDictionary.TryGetValue(key, out handlers))
				{
					if (action != EventHandlerAction.Add)
					{
						return;
					}
					lock (handlersDictionary)
					{
						if (!handlersDictionary.TryGetValue(key, out handlers))
						{
							handlers = handlersDictionary[key] = new List<EventHandler<TEventArgs>>();
						}
					}
				}
				ModelingEventManager.AddOrRemoveHandler<TEventArgs>(handlers, handler, action);
			}
		}

		private static void InvokeHandlers<TEventArgs>(IServiceProvider serviceProvider, List<EventHandler<TEventArgs>> handlers, object sender, TEventArgs e)
			where TEventArgs : EventArgs
		{
			IUIService uiService = null;
			int handlersCount = handlers.Count;
			for (int i = 0; i < handlersCount; i++)
			{
				try
				{
					handlers[i].Invoke(sender, e);
				}
				catch (Exception ex)
				{
					// UNDONE: We may want to use a callback here to allow for other ways to handle exceptions
					if (uiService == null)
					{
						uiService = (IUIService)serviceProvider.GetService(typeof(IUIService));
					}
					if (uiService != null)
					{
						// DialogResult.Cancel corresponds to Continue
						// DialogResult.Abort corresponds to Quit (only shown if Application.AllowQuit returns true)
						// DialogResult.Yes corresponds to Help (only shown if ex is WarningException)
						if (uiService.ShowDialog(new ThreadExceptionDialog(ex)) == DialogResult.Abort)
						{
							// The user wants to quit, so try to invoke the exit command
							IMenuCommandService menuCommandService = (IMenuCommandService)serviceProvider.GetService(typeof(IMenuCommandService));
							if (menuCommandService != null)
							{
								menuCommandService.GlobalInvoke(new CommandID(VSConstants.GUID_VSStandardCommandSet97, (int)VSConstants.VSStd97CmdID.Exit));
							}
						}
					}
					do
					{
						if (ex is StackOverflowException ||
							ex is OutOfMemoryException ||
							ex is ThreadAbortException ||
							ex is ExecutionEngineException ||
							ex is AccessViolationException)
						{
							throw;
						}
					} while ((ex = ex.InnerException) != null);
				}
			}
		}

		private static void InvokeHandlers<TEventArgs>(IServiceProvider serviceProvider, Dictionary<Guid, List<EventHandler<TEventArgs>>> handlersDictionary, object sender, TEventArgs e)
			where TEventArgs : GenericEventArgs
		{
			List<EventHandler<TEventArgs>> handlers;

			// Invoke the handlers for this specific element (except for ElementAddedEventArgs)
			if (typeof(TEventArgs) != typeof(ElementAddedEventArgs) && handlersDictionary.TryGetValue(e.ElementId, out handlers))
			{
				ModelingEventManager.InvokeHandlers<TEventArgs>(serviceProvider, handlers, sender, e);
			}

			// Invoke the handlers for this specific DomainModel
			if (handlersDictionary.TryGetValue(e.DomainModel.Id, out handlers))
			{
				ModelingEventManager.InvokeHandlers<TEventArgs>(serviceProvider, handlers, sender, e);
			}

			// Invoke the handlers for this specific DomainClass and each of its base DomainClasses
			DomainClassInfo domainClassInfo = e.DomainClass;
			do
			{
				if (handlersDictionary.TryGetValue(domainClassInfo.Id, out handlers))
				{
					ModelingEventManager.InvokeHandlers<TEventArgs>(serviceProvider, handlers, sender, e);
				}
			} while ((domainClassInfo = domainClassInfo.BaseDomainClass) != null);
		}
		#endregion // Static helper methods

	}
	#endregion // ModelingEventManager class
}