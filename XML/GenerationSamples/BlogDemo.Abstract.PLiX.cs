using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace BlogDemo
{
	#region BlogEntryLabel
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogEntryLabel : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogEntryLabel()
		{
		}
		#region BlogEntryLabel INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // BlogEntryLabel INotifyPropertyChanged Implementation
		#region BlogEntryLabel Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[4], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> blogEntryIdChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntryLabel.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntryLabel.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnblogEntryIdChanging(BlogEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>(this, "blogEntryId", this.blogEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> blogEntryIdChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntryLabel.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntryLabel.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnblogEntryIdChanged(BlogEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>(this, "blogEntryId", oldValue, this.blogEntryId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("blogEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> blogLabelIdChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntryLabel.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntryLabel.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnblogLabelIdChanging(BlogLabel newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>(this, "blogLabelId", this.blogLabelId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> blogLabelIdChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntryLabel.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntryLabel.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnblogLabelIdChanged(BlogLabel oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>(this, "blogLabelId", oldValue, this.blogLabelId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("blogLabelId");
			}
		}
		#endregion // BlogEntryLabel Property Change Events
		#region BlogEntryLabel Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogEntry blogEntryId
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogLabel blogLabelId
		{
			get;
			set;
		}
		#endregion // BlogEntryLabel Abstract Properties
		#region BlogEntryLabel ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "BlogEntryLabel{0}{{{0}{1}blogEntryId = {2},{0}{1}blogLabelId = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogEntryLabel ToString Methods
	}
	#endregion // BlogEntryLabel
	#region BlogEntry
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogEntry : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogEntry()
		{
		}
		#region BlogEntry INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // BlogEntry INotifyPropertyChanged Implementation
		#region BlogEntry Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[10], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntry_IdChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnBlogEntry_IdChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, int>(this, "BlogEntry_Id", this.BlogEntry_Id, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntry_IdChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnBlogEntry_IdChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, int>(this, "BlogEntry_Id", oldValue, this.BlogEntry_Id), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogEntry_Id");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryTitleChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnentryTitleChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, "entryTitle", this.entryTitle, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryTitleChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnentryTitleChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, "entryTitle", oldValue, this.entryTitle), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("entryTitle");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryBodyChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnentryBodyChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, "entryBody", this.entryBody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryBodyChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnentryBodyChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, "entryBody", oldValue, this.entryBody), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("entryBody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> postedDate_MDYValueChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnpostedDate_MDYValueChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, int>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, int>(this, "postedDate_MDYValue", this.postedDate_MDYValue, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> postedDate_MDYValueChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnpostedDate_MDYValueChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, int>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, int>(this, "postedDate_MDYValue", oldValue, this.postedDate_MDYValue), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("postedDate_MDYValue");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> userIdChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[8], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[8], value);
				}
			}
		}
		protected bool OnuserIdChanging(User newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, User>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, User>>)events[8]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, User>(this, "userId", this.userId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> userIdChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[9], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[9], value);
				}
			}
		}
		protected void OnuserIdChanged(User oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, User>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, User>>)events[9]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, User>(this, "userId", oldValue, this.userId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("userId");
			}
		}
		#endregion // BlogEntry Property Change Events
		#region BlogEntry Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract int BlogEntry_Id
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string entryTitle
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string entryBody
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract int postedDate_MDYValue
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract User userId
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogEntryLabel> BlogEntryLabelViablogEntryIdCollection
		{
			get;
		}
		#endregion // BlogEntry Abstract Properties
		#region BlogEntry ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"BlogEntry{0}{{{0}{1}BlogEntry_Id = ""{2}"",{0}{1}entryTitle = ""{3}"",{0}{1}entryBody = ""{4}"",{0}{1}postedDate_MDYValue = ""{5}"",{0}{1}userId = {6}{0}}}", Environment.NewLine, @"	", this.BlogEntry_Id, this.entryTitle, this.entryBody, this.postedDate_MDYValue, "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogEntry ToString Methods
	}
	#endregion // BlogEntry
	#region User
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class User : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected User()
		{
		}
		#region User INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // User INotifyPropertyChanged Implementation
		#region User Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[8], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> firstNameChanging
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnfirstNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "firstName", this.firstName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> firstNameChanged
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnfirstNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "firstName", oldValue, this.firstName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("firstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> lastNameChanging
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnlastNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "lastName", this.lastName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> lastNameChanged
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnlastNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "lastName", oldValue, this.lastName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("lastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> usernameChanging
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[4], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[4], value);
				}
			}
		}
		protected bool OnusernameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "username", this.username, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> usernameChanged
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[5], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[5], value);
				}
			}
		}
		protected void OnusernameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "username", oldValue, this.username), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("username");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> passwordChanging
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[6], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[6], value);
				}
			}
		}
		protected bool OnpasswordChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "password", this.password, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> passwordChanged
		{
			add
			{
				if ((object)value != null)
				{
					User.InterlockedDelegateCombine(ref this.Events[7], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					User.InterlockedDelegateRemove(ref events[7], value);
				}
			}
		}
		protected void OnpasswordChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "password", oldValue, this.password), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("password");
			}
		}
		#endregion // User Property Change Events
		#region User Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract string firstName
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string lastName
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string username
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string password
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogEntry> BlogEntryViauserIdCollection
		{
			get;
		}
		#endregion // User Abstract Properties
		#region User ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"User{0}{{{0}{1}firstName = ""{2}"",{0}{1}lastName = ""{3}"",{0}{1}username = ""{4}"",{0}{1}password = ""{5}""{0}}}", Environment.NewLine, @"	", this.firstName, this.lastName, this.username, this.password);
		}
		#endregion // User ToString Methods
	}
	#endregion // User
	#region BlogLabel
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogLabel : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogLabel()
		{
		}
		#region BlogLabel INotifyPropertyChanged Implementation
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Combine(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
			remove
			{
				if ((object)value != null)
				{
					PropertyChangedEventHandler currentHandler;
					while ((object)System.Threading.Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this._propertyChangedEventHandler, (PropertyChangedEventHandler)System.Delegate.Remove(currentHandler = this._propertyChangedEventHandler, value), currentHandler) != (object)currentHandler)
					{
					}
				}
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler;
			if ((object)(eventHandler = this._propertyChangedEventHandler) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion // BlogLabel INotifyPropertyChanged Implementation
		#region BlogLabel Property Change Events
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				System.Delegate[] localEvents;
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[2], null) ?? localEvents;
			}
		}
		private static void InterlockedDelegateCombine(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Combine(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		private static void InterlockedDelegateRemove(ref System.Delegate location, System.Delegate value)
		{
			System.Delegate currentHandler;
			while ((object)System.Threading.Interlocked.CompareExchange<System.Delegate>(ref location, System.Delegate.Remove(currentHandler = location, value), currentHandler) != (object)currentHandler)
			{
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogLabel, string>> titleChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogLabel.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogLabel.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OntitleChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogLabel, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogLabel, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogLabel, string>(this, "title", this.title, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogLabel, string>> titleChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogLabel.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogLabel.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OntitleChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogLabel, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogLabel, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogLabel, string>(this, "title", oldValue, this.title), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("title");
			}
		}
		#endregion // BlogLabel Property Change Events
		#region BlogLabel Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract string title
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogEntryLabel> BlogEntryLabelViablogLabelIdCollection
		{
			get;
		}
		#endregion // BlogLabel Abstract Properties
		#region BlogLabel ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"BlogLabel{0}{{{0}{1}title = ""{2}""{0}}}", Environment.NewLine, @"	", this.title);
		}
		#endregion // BlogLabel ToString Methods
	}
	#endregion // BlogLabel
	#region IHasBlogDemoContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	public interface IHasBlogDemoContext
	{
		BlogDemoContext Context
		{
			get;
		}
	}
	#endregion // IHasBlogDemoContext
	#region IBlogDemoContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	public interface IBlogDemoContext
	{
		BlogEntryLabel GetBlogEntryLabelByInternalUniquenessConstraint20(BlogEntry blogEntryId, BlogLabel blogLabelId);
		bool TryGetBlogEntryLabelByInternalUniquenessConstraint20(BlogEntry blogEntryId, BlogLabel blogLabelId, out BlogEntryLabel BlogEntryLabel);
		User GetUserByExternalUniquenessConstraint1(string firstName, string lastName);
		bool TryGetUserByExternalUniquenessConstraint1(string firstName, string lastName, out User User);
		BlogEntry GetBlogEntryByBlogEntry_Id(int BlogEntry_Id);
		bool TryGetBlogEntryByBlogEntry_Id(int BlogEntry_Id, out BlogEntry BlogEntry);
		BlogEntryLabel CreateBlogEntryLabel(BlogEntry blogEntryId, BlogLabel blogLabelId);
		IEnumerable<BlogEntryLabel> BlogEntryLabelCollection
		{
			get;
		}
		BlogEntry CreateBlogEntry(int BlogEntry_Id, string entryTitle, string entryBody, int postedDate_MDYValue, User userId);
		IEnumerable<BlogEntry> BlogEntryCollection
		{
			get;
		}
		User CreateUser(string firstName, string lastName, string username, string password);
		IEnumerable<User> UserCollection
		{
			get;
		}
		BlogLabel CreateBlogLabel();
		IEnumerable<BlogLabel> BlogLabelCollection
		{
			get;
		}
	}
	#endregion // IBlogDemoContext
}
