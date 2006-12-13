using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using AccessedThroughPropertyAttribute = System.Runtime.CompilerServices.AccessedThroughPropertyAttribute;
using GeneratedCodeAttribute = System.CodeDom.Compiler.GeneratedCodeAttribute;
using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
using LayoutKind = System.Runtime.InteropServices.LayoutKind;
using CharSet = System.Runtime.InteropServices.CharSet;
namespace BlogDemo
{
	#region BlogEntryLabel
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogEntryLabel : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogEntryLabel()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[4]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> blogEntryIdChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OnblogEntryIdChanging(BlogEntry newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogEntry>(this, this.blogEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> blogEntryIdChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnblogEntryIdChanged(BlogEntry oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogEntry>(this, oldValue, this.blogEntryId));
				this.OnPropertyChanged("blogEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> blogLabelIdChanging
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		protected bool OnblogLabelIdChanging(BlogLabel newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntryLabel, BlogLabel>(this, this.blogLabelId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> blogLabelIdChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		protected void OnblogLabelIdChanged(BlogLabel oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntryLabel, BlogLabel>(this, oldValue, this.blogLabelId));
				this.OnPropertyChanged("blogLabelId");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "BlogEntryLabel{0}{{{0}{1}blogEntryId = {2},{0}{1}blogLabelId = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
	}
	#endregion // BlogEntryLabel
	#region BlogEntry
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogEntry : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogEntry()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[14]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> BlogEntry_IdChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OnBlogEntry_IdChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, int>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<BlogEntry, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, int>(this, this.BlogEntry_Id, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> BlogEntry_IdChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnBlogEntry_IdChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, int>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BlogEntry, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, int>(this, oldValue, this.BlogEntry_Id));
				this.OnPropertyChanged("BlogEntry_Id");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryTitleChanging
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		protected bool OnentryTitleChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<BlogEntry, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, this.entryTitle, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryTitleChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		protected void OnentryTitleChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<BlogEntry, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, oldValue, this.entryTitle));
				this.OnPropertyChanged("entryTitle");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, string>> entryBodyChanging
		{
			add
			{
				this.Events[5] = System.Delegate.Combine(this.Events[5], value);
			}
			remove
			{
				this.Events[5] = System.Delegate.Remove(this.Events[5], value);
			}
		}
		protected bool OnentryBodyChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<BlogEntry, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, string>(this, this.entryBody, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, string>> entryBodyChanged
		{
			add
			{
				this.Events[6] = System.Delegate.Combine(this.Events[6], value);
			}
			remove
			{
				this.Events[6] = System.Delegate.Remove(this.Events[6], value);
			}
		}
		protected void OnentryBodyChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<BlogEntry, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, string>(this, oldValue, this.entryBody));
				this.OnPropertyChanged("entryBody");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, int>> postedDate_MDYValueChanging
		{
			add
			{
				this.Events[7] = System.Delegate.Combine(this.Events[7], value);
			}
			remove
			{
				this.Events[7] = System.Delegate.Remove(this.Events[7], value);
			}
		}
		protected bool OnpostedDate_MDYValueChanging(int newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, int>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<BlogEntry, int>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, int>(this, this.postedDate_MDYValue, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, int>> postedDate_MDYValueChanged
		{
			add
			{
				this.Events[8] = System.Delegate.Combine(this.Events[8], value);
			}
			remove
			{
				this.Events[8] = System.Delegate.Remove(this.Events[8], value);
			}
		}
		protected void OnpostedDate_MDYValueChanged(int oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, int>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<BlogEntry, int>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, int>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, int>(this, oldValue, this.postedDate_MDYValue));
				this.OnPropertyChanged("postedDate_MDYValue");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, User>> userIdChanging
		{
			add
			{
				this.Events[9] = System.Delegate.Combine(this.Events[9], value);
			}
			remove
			{
				this.Events[9] = System.Delegate.Remove(this.Events[9], value);
			}
		}
		protected bool OnuserIdChanging(User newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, User>> eventHandler = this.Events[9] as EventHandler<PropertyChangingEventArgs<BlogEntry, User>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, User>(this, this.userId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, User>> userIdChanged
		{
			add
			{
				this.Events[10] = System.Delegate.Combine(this.Events[10], value);
			}
			remove
			{
				this.Events[10] = System.Delegate.Remove(this.Events[10], value);
			}
		}
		protected void OnuserIdChanged(User oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, User>> eventHandler = this.Events[10] as EventHandler<PropertyChangedEventArgs<BlogEntry, User>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, User>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, User>(this, oldValue, this.userId));
				this.OnPropertyChanged("userId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>> BlogCommentChanging
		{
			add
			{
				this.Events[11] = System.Delegate.Combine(this.Events[11], value);
			}
			remove
			{
				this.Events[11] = System.Delegate.Remove(this.Events[11], value);
			}
		}
		protected bool OnBlogCommentChanging(BlogComment newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>> eventHandler = this.Events[11] as EventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, BlogComment>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, BlogComment>(this, this.BlogComment, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>> BlogCommentChanged
		{
			add
			{
				this.Events[12] = System.Delegate.Combine(this.Events[12], value);
			}
			remove
			{
				this.Events[12] = System.Delegate.Remove(this.Events[12], value);
			}
		}
		protected void OnBlogCommentChanged(BlogComment oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>> eventHandler = this.Events[12] as EventHandler<PropertyChangedEventArgs<BlogEntry, BlogComment>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, BlogComment>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, BlogComment>(this, oldValue, this.BlogComment));
				this.OnPropertyChanged("BlogComment");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanging
		{
			add
			{
				this.Events[13] = System.Delegate.Combine(this.Events[13], value);
			}
			remove
			{
				this.Events[13] = System.Delegate.Remove(this.Events[13], value);
			}
		}
		protected bool OnNonCommentEntryChanging(NonCommentEntry newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>> eventHandler = this.Events[13] as EventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogEntry, NonCommentEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogEntry, NonCommentEntry>(this, this.NonCommentEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>> NonCommentEntryChanged
		{
			add
			{
				this.Events[14] = System.Delegate.Combine(this.Events[14], value);
			}
			remove
			{
				this.Events[14] = System.Delegate.Remove(this.Events[14], value);
			}
		}
		protected void OnNonCommentEntryChanged(NonCommentEntry oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>> eventHandler = this.Events[14] as EventHandler<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogEntry, NonCommentEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogEntry, NonCommentEntry>(this, oldValue, this.NonCommentEntry));
				this.OnPropertyChanged("NonCommentEntry");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"BlogEntry{0}{{{0}{1}BlogEntry_Id = ""{2}"",{0}{1}entryTitle = ""{3}"",{0}{1}entryBody = ""{4}"",{0}{1}postedDate_MDYValue = ""{5}"",{0}{1}userId = {6},{0}{1}BlogComment = {7},{0}{1}NonCommentEntry = {8}{0}}}", Environment.NewLine, @"	", this.BlogEntry_Id, this.entryTitle, this.entryBody, this.postedDate_MDYValue, "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		public static explicit operator BlogComment(BlogEntry BlogEntry)
		{
			if (BlogEntry == null)
			{
				return null;
			}
			else if (BlogEntry.BlogComment == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return BlogEntry.BlogComment;
			}
		}
		public static explicit operator NonCommentEntry(BlogEntry BlogEntry)
		{
			if (BlogEntry == null)
			{
				return null;
			}
			else if (BlogEntry.NonCommentEntry == null)
			{
				throw new InvalidCastException();
			}
			else
			{
				return BlogEntry.NonCommentEntry;
			}
		}
	}
	#endregion // BlogEntry
	#region BlogComment
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogComment : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogComment()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[4]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> parentEntryIdChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OnparentEntryIdChanging(NonCommentEntry newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogComment, NonCommentEntry>(this, this.parentEntryId, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> parentEntryIdChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnparentEntryIdChanged(NonCommentEntry oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogComment, NonCommentEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogComment, NonCommentEntry>(this, oldValue, this.parentEntryId));
				this.OnPropertyChanged("parentEntryId");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>> BlogEntryChanging
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		protected bool OnBlogEntryChanging(BlogEntry newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogComment, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<BlogComment, BlogEntry>(this, this.BlogEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>> BlogEntryChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		protected void OnBlogEntryChanged(BlogEntry oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<BlogComment, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogComment, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<BlogComment, BlogEntry>(this, oldValue, this.BlogEntry));
				this.OnPropertyChanged("BlogEntry");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "BlogComment{0}{{{0}{1}parentEntryId = {2},{0}{1}BlogEntry = {3}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator BlogEntry(BlogComment BlogComment)
		{
			if (BlogComment == null)
			{
				return null;
			}
			else
			{
				return BlogComment.BlogEntry;
			}
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
	}
	#endregion // BlogComment
	#region NonCommentEntry
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class NonCommentEntry : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected NonCommentEntry()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[2]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>> BlogEntryChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OnBlogEntryChanging(BlogEntry newValue)
		{
			EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<NonCommentEntry, BlogEntry>>(eventHandler, this, new PropertyChangingEventArgs<NonCommentEntry, BlogEntry>(this, this.BlogEntry, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>> BlogEntryChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnBlogEntryChanged(BlogEntry oldValue)
		{
			EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<NonCommentEntry, BlogEntry>>(eventHandler, this, new PropertyChangedEventArgs<NonCommentEntry, BlogEntry>(this, oldValue, this.BlogEntry));
				this.OnPropertyChanged("BlogEntry");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "NonCommentEntry{0}{{{0}{1}BlogEntry = {2}{0}}}", Environment.NewLine, @"	", "TODO: Recursively call ToString for customTypes...");
		}
		public static implicit operator BlogEntry(NonCommentEntry NonCommentEntry)
		{
			if (NonCommentEntry == null)
			{
				return null;
			}
			else
			{
				return NonCommentEntry.BlogEntry;
			}
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
	}
	#endregion // NonCommentEntry
	#region User
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class User : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected User()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[8]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> firstNameChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OnfirstNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, this.firstName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> firstNameChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OnfirstNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, oldValue, this.firstName));
				this.OnPropertyChanged("firstName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> lastNameChanging
		{
			add
			{
				this.Events[3] = System.Delegate.Combine(this.Events[3], value);
			}
			remove
			{
				this.Events[3] = System.Delegate.Remove(this.Events[3], value);
			}
		}
		protected bool OnlastNameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler = this.Events[3] as EventHandler<PropertyChangingEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, this.lastName, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> lastNameChanged
		{
			add
			{
				this.Events[4] = System.Delegate.Combine(this.Events[4], value);
			}
			remove
			{
				this.Events[4] = System.Delegate.Remove(this.Events[4], value);
			}
		}
		protected void OnlastNameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler = this.Events[4] as EventHandler<PropertyChangedEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, oldValue, this.lastName));
				this.OnPropertyChanged("lastName");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> usernameChanging
		{
			add
			{
				this.Events[5] = System.Delegate.Combine(this.Events[5], value);
			}
			remove
			{
				this.Events[5] = System.Delegate.Remove(this.Events[5], value);
			}
		}
		protected bool OnusernameChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler = this.Events[5] as EventHandler<PropertyChangingEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, this.username, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> usernameChanged
		{
			add
			{
				this.Events[6] = System.Delegate.Combine(this.Events[6], value);
			}
			remove
			{
				this.Events[6] = System.Delegate.Remove(this.Events[6], value);
			}
		}
		protected void OnusernameChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler = this.Events[6] as EventHandler<PropertyChangedEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, oldValue, this.username));
				this.OnPropertyChanged("username");
			}
		}
		public event EventHandler<PropertyChangingEventArgs<User, string>> passwordChanging
		{
			add
			{
				this.Events[7] = System.Delegate.Combine(this.Events[7], value);
			}
			remove
			{
				this.Events[7] = System.Delegate.Remove(this.Events[7], value);
			}
		}
		protected bool OnpasswordChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<User, string>> eventHandler = this.Events[7] as EventHandler<PropertyChangingEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<User, string>>(eventHandler, this, new PropertyChangingEventArgs<User, string>(this, this.password, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<User, string>> passwordChanged
		{
			add
			{
				this.Events[8] = System.Delegate.Combine(this.Events[8], value);
			}
			remove
			{
				this.Events[8] = System.Delegate.Remove(this.Events[8], value);
			}
		}
		protected void OnpasswordChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<User, string>> eventHandler = this.Events[8] as EventHandler<PropertyChangedEventArgs<User, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<User, string>>(eventHandler, this, new PropertyChangedEventArgs<User, string>(this, oldValue, this.password));
				this.OnPropertyChanged("password");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"User{0}{{{0}{1}firstName = ""{2}"",{0}{1}lastName = ""{3}"",{0}{1}username = ""{4}"",{0}{1}password = ""{5}""{0}}}", Environment.NewLine, @"	", this.firstName, this.lastName, this.username, this.password);
		}
	}
	#endregion // User
	#region BlogLabel
	[DataObject()]
	[GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public abstract partial class BlogLabel : INotifyPropertyChanged, IHasBlogDemoContext
	{
		protected BlogLabel()
		{
		}
		private System.Delegate[] _events;
		private System.Delegate[] Events
		{
			get
			{
				return this._events ?? (this._events = new System.Delegate[2]);
			}
		}
		private PropertyChangedEventHandler _propertyChangedEventHandler;
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChangedEventHandler = System.Delegate.Combine(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
			remove
			{
				this._propertyChangedEventHandler = System.Delegate.Remove(this._propertyChangedEventHandler, value) as PropertyChangedEventHandler;
			}
		}
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler eventHandler = this._propertyChangedEventHandler;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync(eventHandler, this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public abstract BlogDemoContext Context
		{
			get;
		}
		public event EventHandler<PropertyChangingEventArgs<BlogLabel, string>> titleChanging
		{
			add
			{
				this.Events[1] = System.Delegate.Combine(this.Events[1], value);
			}
			remove
			{
				this.Events[1] = System.Delegate.Remove(this.Events[1], value);
			}
		}
		protected bool OntitleChanging(string newValue)
		{
			EventHandler<PropertyChangingEventArgs<BlogLabel, string>> eventHandler = this.Events[1] as EventHandler<PropertyChangingEventArgs<BlogLabel, string>>;
			if ((object)eventHandler != null)
			{
				return EventHandlerUtility.InvokeCancelableEventHandler<PropertyChangingEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangingEventArgs<BlogLabel, string>(this, this.title, newValue));
			}
			return true;
		}
		public event EventHandler<PropertyChangedEventArgs<BlogLabel, string>> titleChanged
		{
			add
			{
				this.Events[2] = System.Delegate.Combine(this.Events[2], value);
			}
			remove
			{
				this.Events[2] = System.Delegate.Remove(this.Events[2], value);
			}
		}
		protected void OntitleChanged(string oldValue)
		{
			EventHandler<PropertyChangedEventArgs<BlogLabel, string>> eventHandler = this.Events[2] as EventHandler<PropertyChangedEventArgs<BlogLabel, string>>;
			if ((object)eventHandler != null)
			{
				EventHandlerUtility.InvokeEventHandlerAsync<PropertyChangedEventArgs<BlogLabel, string>>(eventHandler, this, new PropertyChangedEventArgs<BlogLabel, string>(this, oldValue, this.title));
				this.OnPropertyChanged("title");
			}
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
		public override string ToString()
		{
			return this.ToString(null);
		}
		public virtual string ToString(IFormatProvider provider)
		{
			return string.Format(provider, @"BlogLabel{0}{{{0}{1}title = ""{2}""{0}}}", Environment.NewLine, @"	", this.title);
		}
	}
	#endregion // BlogLabel
	#region IHasBlogDemoContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
	public interface IHasBlogDemoContext
	{
		BlogDemoContext Context
		{
			get;
		}
	}
	#endregion // IHasBlogDemoContext
	#region IBlogDemoContext
	[GeneratedCode("OIALtoPLiX", "1.0")]
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
