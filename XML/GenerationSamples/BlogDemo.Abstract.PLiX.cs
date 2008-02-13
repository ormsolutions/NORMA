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
				return (localEvents = this._events) ?? System.Threading.Interlocked.CompareExchange<System.Delegate[]>(ref this._events, localEvents = new System.Delegate[14], null) ?? localEvents;
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
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>> BlogCommentChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[10], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[10], value);
				}
			}
		}
		protected bool OnBlogCommentChanging(BlogComment newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>>)events[10]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, BlogComment>(this, "BlogComment", this.BlogComment, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>> BlogCommentChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[11], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[11], value);
				}
			}
		}
		protected void OnBlogCommentChanged(BlogComment oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>>)events[11]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, BlogComment>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, BlogComment>(this, "BlogComment", oldValue, this.BlogComment), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogComment");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[12], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[12], value);
				}
			}
		}
		protected bool OnNonCommentEntryChanging(NonCommentEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>>)events[12]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, NonCommentEntry>(this, "NonCommentEntry", this.NonCommentEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogEntry.InterlockedDelegateCombine(ref this.Events[13], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogEntry.InterlockedDelegateRemove(ref events[13], value);
				}
			}
		}
		protected void OnNonCommentEntryChanged(NonCommentEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>>)events[13]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, NonCommentEntry>(this, "NonCommentEntry", oldValue, this.NonCommentEntry), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("NonCommentEntry");
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
		public abstract BlogComment BlogComment
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract NonCommentEntry NonCommentEntry
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
			return string.Format(provider, @"BlogEntry{0}{{{0}{1}BlogEntry_Id = ""{2}"",{0}{1}entryTitle = ""{3}"",{0}{1}entryBody = ""{4}"",{0}{1}postedDate_MDYValue = ""{5}"",{0}{1}userId = {6},{0}{1}BlogComment = {7},{0}{1}NonCommentEntry = {8}{0}}}", Environment.NewLine, @"	", this.BlogEntry_Id, this.entryTitle, this.entryBody, this.postedDate_MDYValue, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogEntry ToString Methods
		#region BlogEntry Children Support
		#region BlogEntry Child Support (BlogComment)
		public static explicit operator BlogComment(BlogEntry BlogEntry)
		{
			if ((object)BlogEntry == null)
			{
				return null;
			}
			BlogComment BlogComment;
			if ((object)(BlogComment = BlogEntry.BlogComment) == null)
			{
				throw new InvalidCastException();
			}
			return BlogComment;
		}
		#endregion // BlogEntry Child Support (BlogComment)
		#region BlogEntry Child Support (NonCommentEntry)
		public static explicit operator NonCommentEntry(BlogEntry BlogEntry)
		{
			if ((object)BlogEntry == null)
			{
				return null;
			}
			NonCommentEntry NonCommentEntry;
			if ((object)(NonCommentEntry = BlogEntry.NonCommentEntry) == null)
			{
				throw new InvalidCastException();
			}
			return NonCommentEntry;
		}
		#endregion // BlogEntry Child Support (NonCommentEntry)
		#endregion // BlogEntry Children Support
	}
	#endregion // BlogEntry
	#region BlogComment
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogComment : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogComment()
		{
		}
		#region BlogComment INotifyPropertyChanged Implementation
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
		#endregion // BlogComment INotifyPropertyChanged Implementation
		#region BlogComment Property Change Events
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
		public event EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> parentEntryIdChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogComment.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogComment.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnparentEntryIdChanging(NonCommentEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogComment, NonCommentEntry>(this, "parentEntryId", this.parentEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> parentEntryIdChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogComment.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogComment.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnparentEntryIdChanged(NonCommentEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogComment, NonCommentEntry>(this, "parentEntryId", oldValue, this.parentEntryId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("parentEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>> BlogEntryChanging
		{
			add
			{
				if ((object)value != null)
				{
					BlogComment.InterlockedDelegateCombine(ref this.Events[2], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogComment.InterlockedDelegateRemove(ref events[2], value);
				}
			}
		}
		protected bool OnBlogEntryChanging(BlogEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogComment, BlogEntry>(this, "BlogEntry", this.BlogEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>> BlogEntryChanged
		{
			add
			{
				if ((object)value != null)
				{
					BlogComment.InterlockedDelegateCombine(ref this.Events[3], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					BlogComment.InterlockedDelegateRemove(ref events[3], value);
				}
			}
		}
		protected void OnBlogEntryChanged(BlogEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogComment, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogComment, BlogEntry>(this, "BlogEntry", oldValue, this.BlogEntry), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogEntry");
			}
		}
		#endregion // BlogComment Property Change Events
		#region BlogComment Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract NonCommentEntry parentEntryId
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogEntry BlogEntry
		{
			get;
			set;
		}
		#endregion // BlogComment Abstract Properties
		#region BlogComment ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "BlogComment{0}{{{0}{1}parentEntryId = {2},{0}{1}BlogEntry = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogComment ToString Methods
		#region BlogComment Parent Support (BlogEntry)
		public static implicit operator BlogEntry(BlogComment BlogComment)
		{
			if ((object)BlogComment == null)
			{
				return null;
			}
			return BlogComment.BlogEntry;
		}
		public virtual int BlogEntry_Id
		{
			get
			{
				return this.BlogEntry.BlogEntry_Id;
			}
			set
			{
				this.BlogEntry.BlogEntry_Id = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntry_IdChanging
		{
			add
			{
				this.BlogEntry.BlogEntry_IdChanging += value;
			}
			remove
			{
				this.BlogEntry.BlogEntry_IdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntry_IdChanged
		{
			add
			{
				this.BlogEntry.BlogEntry_IdChanged += value;
			}
			remove
			{
				this.BlogEntry.BlogEntry_IdChanged -= value;
			}
		}
		public virtual string entryTitle
		{
			get
			{
				return this.BlogEntry.entryTitle;
			}
			set
			{
				this.BlogEntry.entryTitle = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryTitleChanging
		{
			add
			{
				this.BlogEntry.entryTitleChanging += value;
			}
			remove
			{
				this.BlogEntry.entryTitleChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryTitleChanged
		{
			add
			{
				this.BlogEntry.entryTitleChanged += value;
			}
			remove
			{
				this.BlogEntry.entryTitleChanged -= value;
			}
		}
		public virtual string entryBody
		{
			get
			{
				return this.BlogEntry.entryBody;
			}
			set
			{
				this.BlogEntry.entryBody = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryBodyChanging
		{
			add
			{
				this.BlogEntry.entryBodyChanging += value;
			}
			remove
			{
				this.BlogEntry.entryBodyChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryBodyChanged
		{
			add
			{
				this.BlogEntry.entryBodyChanged += value;
			}
			remove
			{
				this.BlogEntry.entryBodyChanged -= value;
			}
		}
		public virtual int postedDate_MDYValue
		{
			get
			{
				return this.BlogEntry.postedDate_MDYValue;
			}
			set
			{
				this.BlogEntry.postedDate_MDYValue = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> postedDate_MDYValueChanging
		{
			add
			{
				this.BlogEntry.postedDate_MDYValueChanging += value;
			}
			remove
			{
				this.BlogEntry.postedDate_MDYValueChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> postedDate_MDYValueChanged
		{
			add
			{
				this.BlogEntry.postedDate_MDYValueChanged += value;
			}
			remove
			{
				this.BlogEntry.postedDate_MDYValueChanged -= value;
			}
		}
		public virtual User userId
		{
			get
			{
				return this.BlogEntry.userId;
			}
			set
			{
				this.BlogEntry.userId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> userIdChanging
		{
			add
			{
				this.BlogEntry.userIdChanging += value;
			}
			remove
			{
				this.BlogEntry.userIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> userIdChanged
		{
			add
			{
				this.BlogEntry.userIdChanged += value;
			}
			remove
			{
				this.BlogEntry.userIdChanged -= value;
			}
		}
		public virtual NonCommentEntry NonCommentEntry
		{
			get
			{
				return this.BlogEntry.NonCommentEntry;
			}
			set
			{
				this.BlogEntry.NonCommentEntry = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanging
		{
			add
			{
				this.BlogEntry.NonCommentEntryChanging += value;
			}
			remove
			{
				this.BlogEntry.NonCommentEntryChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanged
		{
			add
			{
				this.BlogEntry.NonCommentEntryChanged += value;
			}
			remove
			{
				this.BlogEntry.NonCommentEntryChanged -= value;
			}
		}
		public virtual IEnumerable<BlogEntryLabel> BlogEntryLabelViablogEntryIdCollection
		{
			get
			{
				return this.BlogEntry.BlogEntryLabelViablogEntryIdCollection;
			}
		}
		#endregion // BlogComment Parent Support (BlogEntry)
	}
	#endregion // BlogComment
	#region NonCommentEntry
	[DataObject()]
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class NonCommentEntry : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected NonCommentEntry()
		{
		}
		#region NonCommentEntry INotifyPropertyChanged Implementation
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
		#endregion // NonCommentEntry INotifyPropertyChanged Implementation
		#region NonCommentEntry Property Change Events
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
		public event EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>> BlogEntryChanging
		{
			add
			{
				if ((object)value != null)
				{
					NonCommentEntry.InterlockedDelegateCombine(ref this.Events[0], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NonCommentEntry.InterlockedDelegateRemove(ref events[0], value);
				}
			}
		}
		protected bool OnBlogEntryChanging(BlogEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<NonCommentEntry, BlogEntry>(this, "BlogEntry", this.BlogEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>> BlogEntryChanged
		{
			add
			{
				if ((object)value != null)
				{
					NonCommentEntry.InterlockedDelegateCombine(ref this.Events[1], value);
				}
			}
			remove
			{
				System.Delegate[] events;
				if ((object)value != null && (object)(events = this._events) != null)
				{
					NonCommentEntry.InterlockedDelegateRemove(ref events[1], value);
				}
			}
		}
		protected void OnBlogEntryChanged(BlogEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<NonCommentEntry, BlogEntry>(this, "BlogEntry", oldValue, this.BlogEntry), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogEntry");
			}
		}
		#endregion // NonCommentEntry Property Change Events
		#region NonCommentEntry Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogEntry BlogEntry
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogComment> BlogCommentViaparentEntryIdCollection
		{
			get;
		}
		#endregion // NonCommentEntry Abstract Properties
		#region NonCommentEntry ToString Methods
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "NonCommentEntry{0}{{{0}{1}BlogEntry = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // NonCommentEntry ToString Methods
		#region NonCommentEntry Parent Support (BlogEntry)
		public static implicit operator BlogEntry(NonCommentEntry NonCommentEntry)
		{
			if ((object)NonCommentEntry == null)
			{
				return null;
			}
			return NonCommentEntry.BlogEntry;
		}
		public virtual int BlogEntry_Id
		{
			get
			{
				return this.BlogEntry.BlogEntry_Id;
			}
			set
			{
				this.BlogEntry.BlogEntry_Id = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntry_IdChanging
		{
			add
			{
				this.BlogEntry.BlogEntry_IdChanging += value;
			}
			remove
			{
				this.BlogEntry.BlogEntry_IdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntry_IdChanged
		{
			add
			{
				this.BlogEntry.BlogEntry_IdChanged += value;
			}
			remove
			{
				this.BlogEntry.BlogEntry_IdChanged -= value;
			}
		}
		public virtual string entryTitle
		{
			get
			{
				return this.BlogEntry.entryTitle;
			}
			set
			{
				this.BlogEntry.entryTitle = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryTitleChanging
		{
			add
			{
				this.BlogEntry.entryTitleChanging += value;
			}
			remove
			{
				this.BlogEntry.entryTitleChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryTitleChanged
		{
			add
			{
				this.BlogEntry.entryTitleChanged += value;
			}
			remove
			{
				this.BlogEntry.entryTitleChanged -= value;
			}
		}
		public virtual string entryBody
		{
			get
			{
				return this.BlogEntry.entryBody;
			}
			set
			{
				this.BlogEntry.entryBody = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryBodyChanging
		{
			add
			{
				this.BlogEntry.entryBodyChanging += value;
			}
			remove
			{
				this.BlogEntry.entryBodyChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryBodyChanged
		{
			add
			{
				this.BlogEntry.entryBodyChanged += value;
			}
			remove
			{
				this.BlogEntry.entryBodyChanged -= value;
			}
		}
		public virtual int postedDate_MDYValue
		{
			get
			{
				return this.BlogEntry.postedDate_MDYValue;
			}
			set
			{
				this.BlogEntry.postedDate_MDYValue = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> postedDate_MDYValueChanging
		{
			add
			{
				this.BlogEntry.postedDate_MDYValueChanging += value;
			}
			remove
			{
				this.BlogEntry.postedDate_MDYValueChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> postedDate_MDYValueChanged
		{
			add
			{
				this.BlogEntry.postedDate_MDYValueChanged += value;
			}
			remove
			{
				this.BlogEntry.postedDate_MDYValueChanged -= value;
			}
		}
		public virtual User userId
		{
			get
			{
				return this.BlogEntry.userId;
			}
			set
			{
				this.BlogEntry.userId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> userIdChanging
		{
			add
			{
				this.BlogEntry.userIdChanging += value;
			}
			remove
			{
				this.BlogEntry.userIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> userIdChanged
		{
			add
			{
				this.BlogEntry.userIdChanged += value;
			}
			remove
			{
				this.BlogEntry.userIdChanged -= value;
			}
		}
		public virtual BlogComment BlogComment
		{
			get
			{
				return this.BlogEntry.BlogComment;
			}
			set
			{
				this.BlogEntry.BlogComment = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>> BlogCommentChanging
		{
			add
			{
				this.BlogEntry.BlogCommentChanging += value;
			}
			remove
			{
				this.BlogEntry.BlogCommentChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>> BlogCommentChanged
		{
			add
			{
				this.BlogEntry.BlogCommentChanged += value;
			}
			remove
			{
				this.BlogEntry.BlogCommentChanged -= value;
			}
		}
		public virtual IEnumerable<BlogEntryLabel> BlogEntryLabelViablogEntryIdCollection
		{
			get
			{
				return this.BlogEntry.BlogEntryLabelViablogEntryIdCollection;
			}
		}
		#endregion // NonCommentEntry Parent Support (BlogEntry)
	}
	#endregion // NonCommentEntry
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
		BlogComment CreateBlogComment(NonCommentEntry parentEntryId, BlogEntry BlogEntry);
		IEnumerable<BlogComment> BlogCommentCollection
		{
			get;
		}
		NonCommentEntry CreateNonCommentEntry(BlogEntry BlogEntry);
		IEnumerable<NonCommentEntry> NonCommentEntryCollection
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
