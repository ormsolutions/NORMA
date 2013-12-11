using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace BlogDemo
{
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
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntryIdChanging
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
		protected bool OnBlogEntryIdChanging(int newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, int>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, int>(this, "BlogEntryId", this.BlogEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntryIdChanged
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
		protected void OnBlogEntryIdChanged(int oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, int>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, int>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, int>(this, "BlogEntryId", oldValue, this.BlogEntryId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryTitleChanging
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
		protected bool OnEntryTitleChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, "EntryTitle", this.EntryTitle, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryTitleChanged
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
		protected void OnEntryTitleChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, "EntryTitle", oldValue, this.EntryTitle), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("EntryTitle");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryBodyChanging
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
		protected bool OnEntryBodyChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, "EntryBody", this.EntryBody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryBodyChanged
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
		protected void OnEntryBodyChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, "EntryBody", oldValue, this.EntryBody), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("EntryBody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>> MDYValueChanging
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
		protected bool OnMDYValueChanging(System.DateTime newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, System.DateTime>(this, "MDYValue", this.MDYValue, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, System.DateTime>> MDYValueChanged
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
		protected void OnMDYValueChanged(System.DateTime oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, System.DateTime>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, System.DateTime>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, System.DateTime>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, System.DateTime>(this, "MDYValue", oldValue, this.MDYValue), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("MDYValue");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> UserIdChanging
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
		protected bool OnUserIdChanging(User newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntry, User>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntry, User>>)events[8]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, User>(this, "UserId", this.UserId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> UserIdChanged
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
		protected void OnUserIdChanged(User oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntry, User>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntry, User>>)events[9]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, User>(this, "UserId", oldValue, this.UserId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("UserId");
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
		public abstract int BlogEntryId
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string EntryTitle
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string EntryBody
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract System.DateTime MDYValue
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract User UserId
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
		public abstract IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogEntryIdCollection
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
			return string.Format(provider, @"BlogEntry{0}{{{0}{1}BlogEntryId = ""{2}"",{0}{1}EntryTitle = ""{3}"",{0}{1}EntryBody = ""{4}"",{0}{1}MDYValue = ""{5}"",{0}{1}UserId = {6},{0}{1}BlogComment = {7},{0}{1}NonCommentEntry = {8}{0}}}", Environment.NewLine, @"	", this.BlogEntryId, this.EntryTitle, this.EntryBody, this.MDYValue, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogEntry ToString Methods
		#region BlogEntry Children Support
		#region BlogEntry Child Support (BlogComment)
		public static explicit operator BlogComment(BlogEntry blogEntry)
		{
			if ((object)blogEntry == null)
			{
				return null;
			}
			BlogComment blogComment;
			if ((object)(blogComment = blogEntry.BlogComment) == null)
			{
				throw new InvalidCastException();
			}
			return blogComment;
		}
		#endregion // BlogEntry Child Support (BlogComment)
		#region BlogEntry Child Support (NonCommentEntry)
		public static explicit operator NonCommentEntry(BlogEntry blogEntry)
		{
			if ((object)blogEntry == null)
			{
				return null;
			}
			NonCommentEntry nonCommentEntry;
			if ((object)(nonCommentEntry = blogEntry.NonCommentEntry) == null)
			{
				throw new InvalidCastException();
			}
			return nonCommentEntry;
		}
		#endregion // BlogEntry Child Support (NonCommentEntry)
		#endregion // BlogEntry Children Support
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
		public event EventHandler<PropertyChangingEventArgs<User, string>> FirstNameChanging
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
		protected bool OnFirstNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "FirstName", this.FirstName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> FirstNameChanged
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
		protected void OnFirstNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "FirstName", oldValue, this.FirstName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("FirstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> LastNameChanging
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
		protected bool OnLastNameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "LastName", this.LastName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> LastNameChanged
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
		protected void OnLastNameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "LastName", oldValue, this.LastName), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("LastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> UsernameChanging
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
		protected bool OnUsernameChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[4]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "Username", this.Username, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> UsernameChanged
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
		protected void OnUsernameChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[5]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "Username", oldValue, this.Username), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Username");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> PasswordChanging
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
		protected bool OnPasswordChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<User, string>>)events[6]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, "Password", this.Password, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> PasswordChanged
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
		protected void OnPasswordChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<User, string>>)events[7]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, "Password", oldValue, this.Password), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Password");
			}
		}
		#endregion // User Property Change Events
		#region User Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract string FirstName
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string LastName
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string Username
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract string Password
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogEntry> BlogEntryViaUserIdCollection
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
			return string.Format(provider, @"User{0}{{{0}{1}FirstName = ""{2}"",{0}{1}LastName = ""{3}"",{0}{1}Username = ""{4}"",{0}{1}Password = ""{5}""{0}}}", Environment.NewLine, @"	", this.FirstName, this.LastName, this.Username, this.Password);
		}
		#endregion // User ToString Methods
	}
	#endregion // User
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
		public event EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> ParentEntryIdChanging
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
		protected bool OnParentEntryIdChanging(NonCommentEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogComment, NonCommentEntry>(this, "ParentEntryId", this.ParentEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> ParentEntryIdChanged
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
		protected void OnParentEntryIdChanged(NonCommentEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogComment, NonCommentEntry>(this, "ParentEntryId", oldValue, this.ParentEntryId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("ParentEntryId");
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
		public abstract NonCommentEntry ParentEntryId
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
			return string.Format(provider, "BlogComment{0}{{{0}{1}ParentEntryId = {2},{0}{1}BlogEntry = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogComment ToString Methods
		#region BlogComment Parent Support (BlogEntry)
		public static implicit operator BlogEntry(BlogComment blogComment)
		{
			if ((object)blogComment == null)
			{
				return null;
			}
			return blogComment.BlogEntry;
		}
		public virtual int BlogEntryId
		{
			get
			{
				return this.BlogEntry.BlogEntryId;
			}
			set
			{
				this.BlogEntry.BlogEntryId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntryIdChanging
		{
			add
			{
				this.BlogEntry.BlogEntryIdChanging += value;
			}
			remove
			{
				this.BlogEntry.BlogEntryIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntryIdChanged
		{
			add
			{
				this.BlogEntry.BlogEntryIdChanged += value;
			}
			remove
			{
				this.BlogEntry.BlogEntryIdChanged -= value;
			}
		}
		public virtual string EntryTitle
		{
			get
			{
				return this.BlogEntry.EntryTitle;
			}
			set
			{
				this.BlogEntry.EntryTitle = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryTitleChanging
		{
			add
			{
				this.BlogEntry.EntryTitleChanging += value;
			}
			remove
			{
				this.BlogEntry.EntryTitleChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryTitleChanged
		{
			add
			{
				this.BlogEntry.EntryTitleChanged += value;
			}
			remove
			{
				this.BlogEntry.EntryTitleChanged -= value;
			}
		}
		public virtual string EntryBody
		{
			get
			{
				return this.BlogEntry.EntryBody;
			}
			set
			{
				this.BlogEntry.EntryBody = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryBodyChanging
		{
			add
			{
				this.BlogEntry.EntryBodyChanging += value;
			}
			remove
			{
				this.BlogEntry.EntryBodyChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryBodyChanged
		{
			add
			{
				this.BlogEntry.EntryBodyChanged += value;
			}
			remove
			{
				this.BlogEntry.EntryBodyChanged -= value;
			}
		}
		public virtual System.DateTime MDYValue
		{
			get
			{
				return this.BlogEntry.MDYValue;
			}
			set
			{
				this.BlogEntry.MDYValue = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>> MDYValueChanging
		{
			add
			{
				this.BlogEntry.MDYValueChanging += value;
			}
			remove
			{
				this.BlogEntry.MDYValueChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, System.DateTime>> MDYValueChanged
		{
			add
			{
				this.BlogEntry.MDYValueChanged += value;
			}
			remove
			{
				this.BlogEntry.MDYValueChanged -= value;
			}
		}
		public virtual User UserId
		{
			get
			{
				return this.BlogEntry.UserId;
			}
			set
			{
				this.BlogEntry.UserId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> UserIdChanging
		{
			add
			{
				this.BlogEntry.UserIdChanging += value;
			}
			remove
			{
				this.BlogEntry.UserIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> UserIdChanged
		{
			add
			{
				this.BlogEntry.UserIdChanged += value;
			}
			remove
			{
				this.BlogEntry.UserIdChanged -= value;
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
		public virtual IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogEntryIdCollection
		{
			get
			{
				return this.BlogEntry.BlogEntryLabelViaBlogEntryIdCollection;
			}
		}
		#endregion // BlogComment Parent Support (BlogEntry)
	}
	#endregion // BlogComment
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
		public event EventHandler<PropertyChangingEventArgs<BlogLabel, string>> TitleChanging
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
		protected bool OnTitleChanging(string newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogLabel, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogLabel, string>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogLabel, string>(this, "Title", this.Title, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogLabel, string>> TitleChanged
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
		protected void OnTitleChanged(string oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogLabel, string>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogLabel, string>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogLabel, string>(this, "Title", oldValue, this.Title), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("Title");
			}
		}
		#endregion // BlogLabel Property Change Events
		#region BlogLabel Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, true)]
		public abstract string Title
		{
			get;
			set;
		}
		[DataObjectField(false, false, true)]
		public abstract IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogLabelIdCollection
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
			return string.Format(provider, @"BlogLabel{0}{{{0}{1}Title = ""{2}""{0}}}", Environment.NewLine, @"	", this.Title);
		}
		#endregion // BlogLabel ToString Methods
	}
	#endregion // BlogLabel
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
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> BlogEntryIdChanging
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
		protected bool OnBlogEntryIdChanging(BlogEntry newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>)events[0]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>(this, "BlogEntryId", this.BlogEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> BlogEntryIdChanged
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
		protected void OnBlogEntryIdChanged(BlogEntry oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>)events[1]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>(this, "BlogEntryId", oldValue, this.BlogEntryId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> BlogLabelIdChanging
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
		protected bool OnBlogLabelIdChanging(BlogLabel newValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>)events[2]) != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>(this, "BlogLabelId", this.BlogLabelId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> BlogLabelIdChanged
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
		protected void OnBlogLabelIdChanged(BlogLabel oldValue)
		{
			System.Delegate[] events;
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> eventHandler;
			if ((object)(events = this._events) != null && (object)(eventHandler = (EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>)events[3]) != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>(this, "BlogLabelId", oldValue, this.BlogLabelId), this._propertyChangedEventHandler);
			}
			else
			{
				this.OnPropertyChanged("BlogLabelId");
			}
		}
		#endregion // BlogEntryLabel Property Change Events
		#region BlogEntryLabel Abstract Properties
		public abstract BlogDemoContext Context
		{
			get;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogEntry BlogEntryId
		{
			get;
			set;
		}
		[DataObjectField(false, false, false)]
		public abstract BlogLabel BlogLabelId
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
			return string.Format(provider, "BlogEntryLabel{0}{{{0}{1}BlogEntryId = {2},{0}{1}BlogLabelId = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		#endregion // BlogEntryLabel ToString Methods
	}
	#endregion // BlogEntryLabel
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
		public abstract IEnumerable<BlogComment> BlogCommentViaParentEntryIdCollection
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
		public static implicit operator BlogEntry(NonCommentEntry nonCommentEntry)
		{
			if ((object)nonCommentEntry == null)
			{
				return null;
			}
			return nonCommentEntry.BlogEntry;
		}
		public virtual int BlogEntryId
		{
			get
			{
				return this.BlogEntry.BlogEntryId;
			}
			set
			{
				this.BlogEntry.BlogEntryId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntryIdChanging
		{
			add
			{
				this.BlogEntry.BlogEntryIdChanging += value;
			}
			remove
			{
				this.BlogEntry.BlogEntryIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntryIdChanged
		{
			add
			{
				this.BlogEntry.BlogEntryIdChanged += value;
			}
			remove
			{
				this.BlogEntry.BlogEntryIdChanged -= value;
			}
		}
		public virtual string EntryTitle
		{
			get
			{
				return this.BlogEntry.EntryTitle;
			}
			set
			{
				this.BlogEntry.EntryTitle = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryTitleChanging
		{
			add
			{
				this.BlogEntry.EntryTitleChanging += value;
			}
			remove
			{
				this.BlogEntry.EntryTitleChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryTitleChanged
		{
			add
			{
				this.BlogEntry.EntryTitleChanged += value;
			}
			remove
			{
				this.BlogEntry.EntryTitleChanged -= value;
			}
		}
		public virtual string EntryBody
		{
			get
			{
				return this.BlogEntry.EntryBody;
			}
			set
			{
				this.BlogEntry.EntryBody = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> EntryBodyChanging
		{
			add
			{
				this.BlogEntry.EntryBodyChanging += value;
			}
			remove
			{
				this.BlogEntry.EntryBodyChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> EntryBodyChanged
		{
			add
			{
				this.BlogEntry.EntryBodyChanged += value;
			}
			remove
			{
				this.BlogEntry.EntryBodyChanged -= value;
			}
		}
		public virtual System.DateTime MDYValue
		{
			get
			{
				return this.BlogEntry.MDYValue;
			}
			set
			{
				this.BlogEntry.MDYValue = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, System.DateTime>> MDYValueChanging
		{
			add
			{
				this.BlogEntry.MDYValueChanging += value;
			}
			remove
			{
				this.BlogEntry.MDYValueChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, System.DateTime>> MDYValueChanged
		{
			add
			{
				this.BlogEntry.MDYValueChanged += value;
			}
			remove
			{
				this.BlogEntry.MDYValueChanged -= value;
			}
		}
		public virtual User UserId
		{
			get
			{
				return this.BlogEntry.UserId;
			}
			set
			{
				this.BlogEntry.UserId = value;
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> UserIdChanging
		{
			add
			{
				this.BlogEntry.UserIdChanging += value;
			}
			remove
			{
				this.BlogEntry.UserIdChanging -= value;
			}
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> UserIdChanged
		{
			add
			{
				this.BlogEntry.UserIdChanged += value;
			}
			remove
			{
				this.BlogEntry.UserIdChanged -= value;
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
		public virtual IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogEntryIdCollection
		{
			get
			{
				return this.BlogEntry.BlogEntryLabelViaBlogEntryIdCollection;
			}
		}
		#endregion // NonCommentEntry Parent Support (BlogEntry)
	}
	#endregion // NonCommentEntry
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
		BlogEntry GetBlogEntryByBlogEntryId(int blogEntryId);
		bool TryGetBlogEntryByBlogEntryId(int blogEntryId, out BlogEntry blogEntry);
		User GetUserByFirstNameAndLastName(string firstName, string lastName);
		bool TryGetUserByFirstNameAndLastName(string firstName, string lastName, out User user);
		BlogLabel GetBlogLabelByBlogLabelId(int blogLabelId);
		bool TryGetBlogLabelByBlogLabelId(int blogLabelId, out BlogLabel blogLabel);
		BlogEntryLabel GetBlogEntryLabelByBlogEntryIdAndBlogLabelId(BlogEntry blogEntryId, BlogLabel blogLabelId);
		bool TryGetBlogEntryLabelByBlogEntryIdAndBlogLabelId(BlogEntry blogEntryId, BlogLabel blogLabelId, out BlogEntryLabel blogEntryLabel);
		BlogEntry CreateBlogEntry(int blogEntryId, string entryTitle, string entryBody, System.DateTime MDYValue, User userId);
		IEnumerable<BlogEntry> BlogEntryCollection
		{
			get;
		}
		User CreateUser(string firstName, string lastName, string username, string password);
		IEnumerable<User> UserCollection
		{
			get;
		}
		BlogComment CreateBlogComment(NonCommentEntry parentEntryId, BlogEntry blogEntry);
		IEnumerable<BlogComment> BlogCommentCollection
		{
			get;
		}
		BlogLabel CreateBlogLabel();
		IEnumerable<BlogLabel> BlogLabelCollection
		{
			get;
		}
		BlogEntryLabel CreateBlogEntryLabel(BlogEntry blogEntryId, BlogLabel blogLabelId);
		IEnumerable<BlogEntryLabel> BlogEntryLabelCollection
		{
			get;
		}
		NonCommentEntry CreateNonCommentEntry(BlogEntry blogEntry);
		IEnumerable<NonCommentEntry> NonCommentEntryCollection
		{
			get;
		}
	}
	#endregion // IBlogDemoContext
}
