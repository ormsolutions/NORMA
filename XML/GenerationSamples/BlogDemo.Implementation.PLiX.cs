using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
namespace BlogDemo
{
	#region BlogDemoContext
	[System.CodeDom.Compiler.GeneratedCode("OIALtoPLiX", "1.0")]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
	public sealed class BlogDemoContext : IBlogDemoContext
	{
		public BlogDemoContext()
		{
			Dictionary<RuntimeTypeHandle, object> constraintEnforcementCollectionCallbacksByTypeDictionary = this._ContraintEnforcementCollectionCallbacksByTypeDictionary = new Dictionary<RuntimeTypeHandle, object>(4, RuntimeTypeHandleEqualityComparer.Instance);
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<BlogEntry, BlogEntryLabel>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<BlogEntry, BlogEntryLabel>(new PotentialCollectionModificationCallback<BlogEntry, BlogEntryLabel>(this.OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionAdding), new CommittedCollectionModificationCallback<BlogEntry, BlogEntryLabel>(this.OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionAdded), null, new CommittedCollectionModificationCallback<BlogEntry, BlogEntryLabel>(this.OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<User, BlogEntry>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<User, BlogEntry>(new PotentialCollectionModificationCallback<User, BlogEntry>(this.OnUserBlogEntryViaUserIdCollectionAdding), new CommittedCollectionModificationCallback<User, BlogEntry>(this.OnUserBlogEntryViaUserIdCollectionAdded), null, new CommittedCollectionModificationCallback<User, BlogEntry>(this.OnUserBlogEntryViaUserIdCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<BlogLabel, BlogEntryLabel>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<BlogLabel, BlogEntryLabel>(new PotentialCollectionModificationCallback<BlogLabel, BlogEntryLabel>(this.OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionAdding), new CommittedCollectionModificationCallback<BlogLabel, BlogEntryLabel>(this.OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionAdded), null, new CommittedCollectionModificationCallback<BlogLabel, BlogEntryLabel>(this.OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionRemoved)));
			constraintEnforcementCollectionCallbacksByTypeDictionary.Add(typeof(ConstraintEnforcementCollection<NonCommentEntry, BlogComment>).TypeHandle, new ConstraintEnforcementCollectionCallbacks<NonCommentEntry, BlogComment>(new PotentialCollectionModificationCallback<NonCommentEntry, BlogComment>(this.OnNonCommentEntryBlogCommentViaParentEntryIdCollectionAdding), new CommittedCollectionModificationCallback<NonCommentEntry, BlogComment>(this.OnNonCommentEntryBlogCommentViaParentEntryIdCollectionAdded), null, new CommittedCollectionModificationCallback<NonCommentEntry, BlogComment>(this.OnNonCommentEntryBlogCommentViaParentEntryIdCollectionRemoved)));
			this._BlogEntryReadOnlyCollection = new ReadOnlyCollection<BlogEntry>(this._BlogEntryList = new List<BlogEntry>());
			this._UserReadOnlyCollection = new ReadOnlyCollection<User>(this._UserList = new List<User>());
			this._BlogCommentReadOnlyCollection = new ReadOnlyCollection<BlogComment>(this._BlogCommentList = new List<BlogComment>());
			this._BlogLabelReadOnlyCollection = new ReadOnlyCollection<BlogLabel>(this._BlogLabelList = new List<BlogLabel>());
			this._BlogEntryLabelReadOnlyCollection = new ReadOnlyCollection<BlogEntryLabel>(this._BlogEntryLabelList = new List<BlogEntryLabel>());
			this._NonCommentEntryReadOnlyCollection = new ReadOnlyCollection<NonCommentEntry>(this._NonCommentEntryList = new List<NonCommentEntry>());
		}
		#region Exception Helpers
		private static ArgumentException GetDifferentContextsException()
		{
			return BlogDemoContext.GetDifferentContextsException("value");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
		private static ArgumentException GetDifferentContextsException(string paramName)
		{
			return new ArgumentException("All objects in a relationship must be part of the same Context.", paramName);
		}
		private static ArgumentException GetConstraintEnforcementFailedException(string paramName)
		{
			return new ArgumentException("Argument failed constraint enforcement.", paramName);
		}
		#endregion // Exception Helpers
		#region Lookup and External Constraint Enforcement
		private readonly Dictionary<int, BlogEntry> _BlogEntryBlogEntryIdDictionary = new Dictionary<int, BlogEntry>();
		public BlogEntry GetBlogEntryByBlogEntryId(int blogEntryId)
		{
			return this._BlogEntryBlogEntryIdDictionary[blogEntryId];
		}
		public bool TryGetBlogEntryByBlogEntryId(int blogEntryId, out BlogEntry blogEntry)
		{
			return this._BlogEntryBlogEntryIdDictionary.TryGetValue(blogEntryId, out blogEntry);
		}
		private readonly Dictionary<Tuple<string, string>, User> _UserFirstNameAndLastNameDictionary = new Dictionary<Tuple<string, string>, User>();
		public User GetUserByFirstNameAndLastName(string firstName, string lastName)
		{
			return this._UserFirstNameAndLastNameDictionary[Tuple.CreateTuple<string, string>(firstName, lastName)];
		}
		public bool TryGetUserByFirstNameAndLastName(string firstName, string lastName, out User user)
		{
			return this._UserFirstNameAndLastNameDictionary.TryGetValue(Tuple.CreateTuple<string, string>(firstName, lastName), out user);
		}
		private bool OnFirstNameAndLastNameChanging(User instance, Tuple<string, string> newValue)
		{
			if ((object)newValue != null)
			{
				User currentInstance;
				if (this._UserFirstNameAndLastNameDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnFirstNameAndLastNameChanged(User instance, Tuple<string, string> oldValue, Tuple<string, string> newValue)
		{
			if ((object)oldValue != null)
			{
				this._UserFirstNameAndLastNameDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._UserFirstNameAndLastNameDictionary.Add(newValue, instance);
			}
		}
		private readonly Dictionary<int, BlogLabel> _BlogLabelBlogLabelIdDictionary = new Dictionary<int, BlogLabel>();
		public BlogLabel GetBlogLabelByBlogLabelId(int blogLabelId)
		{
			return this._BlogLabelBlogLabelIdDictionary[blogLabelId];
		}
		public bool TryGetBlogLabelByBlogLabelId(int blogLabelId, out BlogLabel blogLabel)
		{
			return this._BlogLabelBlogLabelIdDictionary.TryGetValue(blogLabelId, out blogLabel);
		}
		private readonly Dictionary<Tuple<BlogEntry, BlogLabel>, BlogEntryLabel> _BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary = new Dictionary<Tuple<BlogEntry, BlogLabel>, BlogEntryLabel>();
		public BlogEntryLabel GetBlogEntryLabelByBlogEntryIdAndBlogLabelId(BlogEntry blogEntryId, BlogLabel blogLabelId)
		{
			return this._BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary[Tuple.CreateTuple<BlogEntry, BlogLabel>(blogEntryId, blogLabelId)];
		}
		public bool TryGetBlogEntryLabelByBlogEntryIdAndBlogLabelId(BlogEntry blogEntryId, BlogLabel blogLabelId, out BlogEntryLabel blogEntryLabel)
		{
			return this._BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary.TryGetValue(Tuple.CreateTuple<BlogEntry, BlogLabel>(blogEntryId, blogLabelId), out blogEntryLabel);
		}
		private bool OnBlogEntryIdAndBlogLabelIdChanging(BlogEntryLabel instance, Tuple<BlogEntry, BlogLabel> newValue)
		{
			if ((object)newValue != null)
			{
				BlogEntryLabel currentInstance;
				if (this._BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary.TryGetValue(newValue, out currentInstance))
				{
					return (object)currentInstance == (object)instance;
				}
			}
			return true;
		}
		private void OnBlogEntryIdAndBlogLabelIdChanged(BlogEntryLabel instance, Tuple<BlogEntry, BlogLabel> oldValue, Tuple<BlogEntry, BlogLabel> newValue)
		{
			if ((object)oldValue != null)
			{
				this._BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary.Remove(oldValue);
			}
			if ((object)newValue != null)
			{
				this._BlogEntryLabelBlogEntryIdAndBlogLabelIdDictionary.Add(newValue, instance);
			}
		}
		#endregion // Lookup and External Constraint Enforcement
		#region ConstraintEnforcementCollection
		private delegate bool PotentialCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
			where TClass : class, IHasBlogDemoContext;
		private delegate void CommittedCollectionModificationCallback<TClass, TProperty>(TClass instance, TProperty item)
			where TClass : class, IHasBlogDemoContext;
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollectionCallbacks<TClass, TProperty>
			where TClass : class, IHasBlogDemoContext
		{
			public ConstraintEnforcementCollectionCallbacks(PotentialCollectionModificationCallback<TClass, TProperty> adding, CommittedCollectionModificationCallback<TClass, TProperty> added, PotentialCollectionModificationCallback<TClass, TProperty> removing, CommittedCollectionModificationCallback<TClass, TProperty> removed)
			{
				this.Adding = adding;
				this.Added = added;
				this.Removing = removing;
				this.Removed = removed;
			}
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Adding;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Added;
			public readonly PotentialCollectionModificationCallback<TClass, TProperty> Removing;
			public readonly CommittedCollectionModificationCallback<TClass, TProperty> Removed;
		}
		private ConstraintEnforcementCollectionCallbacks<TClass, TProperty> GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>()
			where TClass : class, IHasBlogDemoContext
		{
			return (ConstraintEnforcementCollectionCallbacks<TClass, TProperty>)this._ContraintEnforcementCollectionCallbacksByTypeDictionary[typeof(ConstraintEnforcementCollection<TClass, TProperty>).TypeHandle];
		}
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class RuntimeTypeHandleEqualityComparer : IEqualityComparer<RuntimeTypeHandle>
		{
			public static readonly RuntimeTypeHandleEqualityComparer Instance = new RuntimeTypeHandleEqualityComparer();
			private RuntimeTypeHandleEqualityComparer()
			{
			}
			public bool Equals(RuntimeTypeHandle x, RuntimeTypeHandle y)
			{
				return x.Equals(y);
			}
			public int GetHashCode(RuntimeTypeHandle obj)
			{
				return obj.GetHashCode();
			}
		}
		private readonly Dictionary<RuntimeTypeHandle, object> _ContraintEnforcementCollectionCallbacksByTypeDictionary;
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class ConstraintEnforcementCollection<TClass, TProperty> : ICollection<TProperty>
			where TClass : class, IHasBlogDemoContext
		{
			private readonly TClass _Instance;
			private readonly List<TProperty> _List = new List<TProperty>();
			public ConstraintEnforcementCollection(TClass instance)
			{
				this._Instance = instance;
			}
			private System.Collections.IEnumerator GetNonGenericEnumerator()
			{
				return this.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetNonGenericEnumerator();
			}
			public IEnumerator<TProperty> GetEnumerator()
			{
				return this._List.GetEnumerator();
			}
			public void Add(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				TClass instance = this._Instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> adding = callbacks.Adding;
				if ((object)adding == null || adding(instance, item))
				{
					this._List.Add(item);
					CommittedCollectionModificationCallback<TClass, TProperty> added = callbacks.Added;
					if ((object)added != null)
					{
						added(instance, item);
					}
				}
			}
			public bool Remove(TProperty item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				TClass instance = this._Instance;
				ConstraintEnforcementCollectionCallbacks<TClass, TProperty> callbacks = instance.Context.GetConstraintEnforcementCollectionCallbacks<TClass, TProperty>();
				PotentialCollectionModificationCallback<TClass, TProperty> removing = callbacks.Removing;
				if ((object)removing == null || removing(instance, item))
				{
					if (this._List.Remove(item))
					{
						CommittedCollectionModificationCallback<TClass, TProperty> removed = callbacks.Removed;
						if ((object)removed != null)
						{
							removed(instance, item);
						}
						return true;
					}
				}
				return false;
			}
			public void Clear()
			{
				List<TProperty> list = this._List;
				for (int i = list.Count - 1; i > 0; --i)
				{
					this.Remove(list[i]);
				}
			}
			public bool Contains(TProperty item)
			{
				return item != null && this._List.Contains(item);
			}
			public void CopyTo(TProperty[] array, int arrayIndex)
			{
				this._List.CopyTo(array, arrayIndex);
			}
			public int Count
			{
				get
				{
					return this._List.Count;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}
		#endregion // ConstraintEnforcementCollection
		#region BlogEntry
		public BlogEntry CreateBlogEntry(int blogEntryId, string entryTitle, string entryBody, System.DateTime MDYValue, User userId)
		{
			if ((object)entryTitle == null)
			{
				throw new ArgumentNullException("entryTitle");
			}
			if ((object)entryBody == null)
			{
				throw new ArgumentNullException("entryBody");
			}
			if ((object)userId == null)
			{
				throw new ArgumentNullException("userId");
			}
			if (!this.OnBlogEntryBlogEntryIdChanging(null, blogEntryId))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("blogEntryId");
			}
			if (!this.OnBlogEntryEntryTitleChanging(null, entryTitle))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("entryTitle");
			}
			if (!this.OnBlogEntryEntryBodyChanging(null, entryBody))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("entryBody");
			}
			if (!this.OnBlogEntryMDYValueChanging(null, MDYValue))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("MDYValue");
			}
			if (!this.OnBlogEntryUserIdChanging(null, userId))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("userId");
			}
			return new BlogEntryImpl(this, blogEntryId, entryTitle, entryBody, MDYValue, userId);
		}
		private bool OnBlogEntryBlogEntryIdChanging(BlogEntry instance, int newValue)
		{
			BlogEntry currentInstance;
			if (this._BlogEntryBlogEntryIdDictionary.TryGetValue(newValue, out currentInstance))
			{
				if ((object)currentInstance != (object)instance)
				{
					return false;
				}
			}
			return true;
		}
		private void OnBlogEntryBlogEntryIdChanged(BlogEntry instance, Nullable<int> oldValue)
		{
			this._BlogEntryBlogEntryIdDictionary.Add(instance.BlogEntryId, instance);
			if (oldValue.HasValue)
			{
				this._BlogEntryBlogEntryIdDictionary.Remove(oldValue.GetValueOrDefault());
			}
		}
		private bool OnBlogEntryEntryTitleChanging(BlogEntry instance, string newValue)
		{
			return true;
		}
		private bool OnBlogEntryEntryBodyChanging(BlogEntry instance, string newValue)
		{
			return true;
		}
		private bool OnBlogEntryMDYValueChanging(BlogEntry instance, System.DateTime newValue)
		{
			return true;
		}
		private bool OnBlogEntryUserIdChanging(BlogEntry instance, User newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnBlogEntryUserIdChanged(BlogEntry instance, User oldValue)
		{
			((ICollection<BlogEntry>)instance.UserId.BlogEntryViaUserIdCollection).Add(instance);
			if ((object)oldValue != null)
			{
				((ICollection<BlogEntry>)oldValue.BlogEntryViaUserIdCollection).Remove(instance);
			}
		}
		private bool OnBlogEntryBlogCommentChanging(BlogEntry instance, BlogComment newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
				{
					throw BlogDemoContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnBlogEntryBlogCommentChanged(BlogEntry instance, BlogComment oldValue)
		{
			if ((object)instance.BlogComment != null)
			{
				instance.BlogComment.BlogEntry = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.BlogEntry = null;
			}
		}
		private bool OnBlogEntryNonCommentEntryChanging(BlogEntry instance, NonCommentEntry newValue)
		{
			if ((object)newValue != null)
			{
				if ((object)this != (object)newValue.Context)
				{
					throw BlogDemoContext.GetDifferentContextsException();
				}
			}
			return true;
		}
		private void OnBlogEntryNonCommentEntryChanged(BlogEntry instance, NonCommentEntry oldValue)
		{
			if ((object)instance.NonCommentEntry != null)
			{
				instance.NonCommentEntry.BlogEntry = instance;
			}
			if ((object)oldValue != null)
			{
				oldValue.BlogEntry = null;
			}
		}
		private bool OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionAdding(BlogEntry instance, BlogEntryLabel item)
		{
			if ((object)this != (object)item.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionAdded(BlogEntry instance, BlogEntryLabel item)
		{
			item.BlogEntryId = instance;
		}
		private void OnBlogEntryBlogEntryLabelViaBlogEntryIdCollectionRemoved(BlogEntry instance, BlogEntryLabel item)
		{
			if ((object)item.BlogEntryId == (object)instance)
			{
				item.BlogEntryId = null;
			}
		}
		private readonly List<BlogEntry> _BlogEntryList;
		private readonly ReadOnlyCollection<BlogEntry> _BlogEntryReadOnlyCollection;
		public IEnumerable<BlogEntry> BlogEntryCollection
		{
			get
			{
				return this._BlogEntryReadOnlyCollection;
			}
		}
		#region BlogEntryImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class BlogEntryImpl : BlogEntry
		{
			public BlogEntryImpl(BlogDemoContext context, int blogEntryId, string entryTitle, string entryBody, System.DateTime MDYValue, User userId)
			{
				this._Context = context;
				this._BlogEntryLabelViaBlogEntryIdCollection = new ConstraintEnforcementCollection<BlogEntry, BlogEntryLabel>(this);
				this._BlogEntryId = blogEntryId;
				context.OnBlogEntryBlogEntryIdChanged(this, null);
				this._EntryTitle = entryTitle;
				this._EntryBody = entryBody;
				this._MDYValue = MDYValue;
				this._UserId = userId;
				context.OnBlogEntryUserIdChanged(this, null);
				context._BlogEntryList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private int _BlogEntryId;
			public sealed override int BlogEntryId
			{
				get
				{
					return this._BlogEntryId;
				}
				set
				{
					int oldValue = this._BlogEntryId;
					if (oldValue != value)
					{
						if (this._Context.OnBlogEntryBlogEntryIdChanging(this, value) && base.OnBlogEntryIdChanging(value))
						{
							this._BlogEntryId = value;
							this._Context.OnBlogEntryBlogEntryIdChanged(this, oldValue);
							base.OnBlogEntryIdChanged(oldValue);
						}
					}
				}
			}
			private string _EntryTitle;
			public sealed override string EntryTitle
			{
				get
				{
					return this._EntryTitle;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._EntryTitle;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnBlogEntryEntryTitleChanging(this, value) && base.OnEntryTitleChanging(value))
						{
							this._EntryTitle = value;
							base.OnEntryTitleChanged(oldValue);
						}
					}
				}
			}
			private string _EntryBody;
			public sealed override string EntryBody
			{
				get
				{
					return this._EntryBody;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._EntryBody;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnBlogEntryEntryBodyChanging(this, value) && base.OnEntryBodyChanging(value))
						{
							this._EntryBody = value;
							base.OnEntryBodyChanged(oldValue);
						}
					}
				}
			}
			private System.DateTime _MDYValue;
			public sealed override System.DateTime MDYValue
			{
				get
				{
					return this._MDYValue;
				}
				set
				{
					System.DateTime oldValue = this._MDYValue;
					if (oldValue != value)
					{
						if (this._Context.OnBlogEntryMDYValueChanging(this, value) && base.OnMDYValueChanging(value))
						{
							this._MDYValue = value;
							base.OnMDYValueChanged(oldValue);
						}
					}
				}
			}
			private User _UserId;
			public sealed override User UserId
			{
				get
				{
					return this._UserId;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					User oldValue = this._UserId;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogEntryUserIdChanging(this, value) && base.OnUserIdChanging(value))
						{
							this._UserId = value;
							this._Context.OnBlogEntryUserIdChanged(this, oldValue);
							base.OnUserIdChanged(oldValue);
						}
					}
				}
			}
			private BlogComment _BlogComment;
			public sealed override BlogComment BlogComment
			{
				get
				{
					return this._BlogComment;
				}
				set
				{
					BlogComment oldValue = this._BlogComment;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogEntryBlogCommentChanging(this, value) && base.OnBlogCommentChanging(value))
						{
							this._BlogComment = value;
							this._Context.OnBlogEntryBlogCommentChanged(this, oldValue);
							base.OnBlogCommentChanged(oldValue);
						}
					}
				}
			}
			private NonCommentEntry _NonCommentEntry;
			public sealed override NonCommentEntry NonCommentEntry
			{
				get
				{
					return this._NonCommentEntry;
				}
				set
				{
					NonCommentEntry oldValue = this._NonCommentEntry;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogEntryNonCommentEntryChanging(this, value) && base.OnNonCommentEntryChanging(value))
						{
							this._NonCommentEntry = value;
							this._Context.OnBlogEntryNonCommentEntryChanged(this, oldValue);
							base.OnNonCommentEntryChanged(oldValue);
						}
					}
				}
			}
			private readonly IEnumerable<BlogEntryLabel> _BlogEntryLabelViaBlogEntryIdCollection;
			public sealed override IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogEntryIdCollection
			{
				get
				{
					return this._BlogEntryLabelViaBlogEntryIdCollection;
				}
			}
		}
		#endregion // BlogEntryImpl
		#endregion // BlogEntry
		#region User
		public User CreateUser(string firstName, string lastName, string username, string password)
		{
			if ((object)firstName == null)
			{
				throw new ArgumentNullException("firstName");
			}
			if ((object)lastName == null)
			{
				throw new ArgumentNullException("lastName");
			}
			if ((object)username == null)
			{
				throw new ArgumentNullException("username");
			}
			if ((object)password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (!this.OnUserFirstNameChanging(null, firstName))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("firstName");
			}
			if (!this.OnUserLastNameChanging(null, lastName))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("lastName");
			}
			if (!this.OnUserUsernameChanging(null, username))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("username");
			}
			if (!this.OnUserPasswordChanging(null, password))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("password");
			}
			return new UserImpl(this, firstName, lastName, username, password);
		}
		private bool OnUserFirstNameChanging(User instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnFirstNameAndLastNameChanging(instance, Tuple.CreateTuple<string, string>(newValue, instance.LastName)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnUserFirstNameChanged(User instance, string oldValue)
		{
			Tuple<string, string> FirstNameAndLastNameOldValueTuple;
			if ((object)oldValue != null)
			{
				FirstNameAndLastNameOldValueTuple = Tuple.CreateTuple<string, string>(oldValue, instance.LastName);
			}
			else
			{
				FirstNameAndLastNameOldValueTuple = null;
			}
			this.OnFirstNameAndLastNameChanged(instance, FirstNameAndLastNameOldValueTuple, Tuple.CreateTuple<string, string>(instance.FirstName, instance.LastName));
		}
		private bool OnUserLastNameChanging(User instance, string newValue)
		{
			if ((object)instance != null)
			{
				if (!this.OnFirstNameAndLastNameChanging(instance, Tuple.CreateTuple<string, string>(instance.FirstName, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnUserLastNameChanged(User instance, string oldValue)
		{
			Tuple<string, string> FirstNameAndLastNameOldValueTuple;
			if ((object)oldValue != null)
			{
				FirstNameAndLastNameOldValueTuple = Tuple.CreateTuple<string, string>(instance.FirstName, oldValue);
			}
			else
			{
				FirstNameAndLastNameOldValueTuple = null;
			}
			this.OnFirstNameAndLastNameChanged(instance, FirstNameAndLastNameOldValueTuple, Tuple.CreateTuple<string, string>(instance.FirstName, instance.LastName));
		}
		private bool OnUserUsernameChanging(User instance, string newValue)
		{
			return true;
		}
		private bool OnUserPasswordChanging(User instance, string newValue)
		{
			return true;
		}
		private bool OnUserBlogEntryViaUserIdCollectionAdding(User instance, BlogEntry item)
		{
			if ((object)this != (object)item.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnUserBlogEntryViaUserIdCollectionAdded(User instance, BlogEntry item)
		{
			item.UserId = instance;
		}
		private void OnUserBlogEntryViaUserIdCollectionRemoved(User instance, BlogEntry item)
		{
			if ((object)item.UserId == (object)instance)
			{
				item.UserId = null;
			}
		}
		private readonly List<User> _UserList;
		private readonly ReadOnlyCollection<User> _UserReadOnlyCollection;
		public IEnumerable<User> UserCollection
		{
			get
			{
				return this._UserReadOnlyCollection;
			}
		}
		#region UserImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class UserImpl : User
		{
			public UserImpl(BlogDemoContext context, string firstName, string lastName, string username, string password)
			{
				this._Context = context;
				this._BlogEntryViaUserIdCollection = new ConstraintEnforcementCollection<User, BlogEntry>(this);
				this._FirstName = firstName;
				context.OnUserFirstNameChanged(this, null);
				this._LastName = lastName;
				context.OnUserLastNameChanged(this, null);
				this._Username = username;
				this._Password = password;
				context._UserList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private string _FirstName;
			public sealed override string FirstName
			{
				get
				{
					return this._FirstName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._FirstName;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnUserFirstNameChanging(this, value) && base.OnFirstNameChanging(value))
						{
							this._FirstName = value;
							this._Context.OnUserFirstNameChanged(this, oldValue);
							base.OnFirstNameChanged(oldValue);
						}
					}
				}
			}
			private string _LastName;
			public sealed override string LastName
			{
				get
				{
					return this._LastName;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._LastName;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnUserLastNameChanging(this, value) && base.OnLastNameChanging(value))
						{
							this._LastName = value;
							this._Context.OnUserLastNameChanged(this, oldValue);
							base.OnLastNameChanged(oldValue);
						}
					}
				}
			}
			private string _Username;
			public sealed override string Username
			{
				get
				{
					return this._Username;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Username;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnUserUsernameChanging(this, value) && base.OnUsernameChanging(value))
						{
							this._Username = value;
							base.OnUsernameChanged(oldValue);
						}
					}
				}
			}
			private string _Password;
			public sealed override string Password
			{
				get
				{
					return this._Password;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					string oldValue = this._Password;
					if ((object)oldValue != (object)value && !value.Equals(oldValue))
					{
						if (this._Context.OnUserPasswordChanging(this, value) && base.OnPasswordChanging(value))
						{
							this._Password = value;
							base.OnPasswordChanged(oldValue);
						}
					}
				}
			}
			private readonly IEnumerable<BlogEntry> _BlogEntryViaUserIdCollection;
			public sealed override IEnumerable<BlogEntry> BlogEntryViaUserIdCollection
			{
				get
				{
					return this._BlogEntryViaUserIdCollection;
				}
			}
		}
		#endregion // UserImpl
		#endregion // User
		#region BlogComment
		public BlogComment CreateBlogComment(NonCommentEntry parentEntryId, BlogEntry blogEntry)
		{
			if ((object)parentEntryId == null)
			{
				throw new ArgumentNullException("parentEntryId");
			}
			if ((object)blogEntry == null)
			{
				throw new ArgumentNullException("blogEntry");
			}
			if (!this.OnBlogCommentParentEntryIdChanging(null, parentEntryId))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("parentEntryId");
			}
			if (!this.OnBlogCommentBlogEntryChanging(null, blogEntry))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("blogEntry");
			}
			return new BlogCommentImpl(this, parentEntryId, blogEntry);
		}
		private bool OnBlogCommentParentEntryIdChanging(BlogComment instance, NonCommentEntry newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnBlogCommentParentEntryIdChanged(BlogComment instance, NonCommentEntry oldValue)
		{
			((ICollection<BlogComment>)instance.ParentEntryId.BlogCommentViaParentEntryIdCollection).Add(instance);
			if ((object)oldValue != null)
			{
				((ICollection<BlogComment>)oldValue.BlogCommentViaParentEntryIdCollection).Remove(instance);
			}
		}
		private bool OnBlogCommentBlogEntryChanging(BlogComment instance, BlogEntry newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnBlogCommentBlogEntryChanged(BlogComment instance, BlogEntry oldValue)
		{
			instance.BlogEntry.BlogComment = instance;
			if ((object)oldValue != null)
			{
				oldValue.BlogComment = null;
			}
		}
		private readonly List<BlogComment> _BlogCommentList;
		private readonly ReadOnlyCollection<BlogComment> _BlogCommentReadOnlyCollection;
		public IEnumerable<BlogComment> BlogCommentCollection
		{
			get
			{
				return this._BlogCommentReadOnlyCollection;
			}
		}
		#region BlogCommentImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class BlogCommentImpl : BlogComment
		{
			public BlogCommentImpl(BlogDemoContext context, NonCommentEntry parentEntryId, BlogEntry blogEntry)
			{
				this._Context = context;
				this._ParentEntryId = parentEntryId;
				context.OnBlogCommentParentEntryIdChanged(this, null);
				this._BlogEntry = blogEntry;
				context.OnBlogCommentBlogEntryChanged(this, null);
				context._BlogCommentList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private NonCommentEntry _ParentEntryId;
			public sealed override NonCommentEntry ParentEntryId
			{
				get
				{
					return this._ParentEntryId;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					NonCommentEntry oldValue = this._ParentEntryId;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogCommentParentEntryIdChanging(this, value) && base.OnParentEntryIdChanging(value))
						{
							this._ParentEntryId = value;
							this._Context.OnBlogCommentParentEntryIdChanged(this, oldValue);
							base.OnParentEntryIdChanged(oldValue);
						}
					}
				}
			}
			private BlogEntry _BlogEntry;
			public sealed override BlogEntry BlogEntry
			{
				get
				{
					return this._BlogEntry;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					BlogEntry oldValue = this._BlogEntry;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogCommentBlogEntryChanging(this, value) && base.OnBlogEntryChanging(value))
						{
							this._BlogEntry = value;
							this._Context.OnBlogCommentBlogEntryChanged(this, oldValue);
							base.OnBlogEntryChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // BlogCommentImpl
		#endregion // BlogComment
		#region BlogLabel
		public BlogLabel CreateBlogLabel()
		{
			return new BlogLabelImpl(this);
		}
		private bool OnBlogLabelTitleChanging(BlogLabel instance, string newValue)
		{
			return true;
		}
		private bool OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionAdding(BlogLabel instance, BlogEntryLabel item)
		{
			if ((object)this != (object)item.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionAdded(BlogLabel instance, BlogEntryLabel item)
		{
			item.BlogLabelId = instance;
		}
		private void OnBlogLabelBlogEntryLabelViaBlogLabelIdCollectionRemoved(BlogLabel instance, BlogEntryLabel item)
		{
			if ((object)item.BlogLabelId == (object)instance)
			{
				item.BlogLabelId = null;
			}
		}
		private readonly List<BlogLabel> _BlogLabelList;
		private readonly ReadOnlyCollection<BlogLabel> _BlogLabelReadOnlyCollection;
		public IEnumerable<BlogLabel> BlogLabelCollection
		{
			get
			{
				return this._BlogLabelReadOnlyCollection;
			}
		}
		#region BlogLabelImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class BlogLabelImpl : BlogLabel
		{
			public BlogLabelImpl(BlogDemoContext context)
			{
				this._Context = context;
				this._BlogEntryLabelViaBlogLabelIdCollection = new ConstraintEnforcementCollection<BlogLabel, BlogEntryLabel>(this);
				context._BlogLabelList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private string _Title;
			public sealed override string Title
			{
				get
				{
					return this._Title;
				}
				set
				{
					string oldValue = this._Title;
					if (!object.Equals(oldValue, value))
					{
						if (this._Context.OnBlogLabelTitleChanging(this, value) && base.OnTitleChanging(value))
						{
							this._Title = value;
							base.OnTitleChanged(oldValue);
						}
					}
				}
			}
			private readonly IEnumerable<BlogEntryLabel> _BlogEntryLabelViaBlogLabelIdCollection;
			public sealed override IEnumerable<BlogEntryLabel> BlogEntryLabelViaBlogLabelIdCollection
			{
				get
				{
					return this._BlogEntryLabelViaBlogLabelIdCollection;
				}
			}
		}
		#endregion // BlogLabelImpl
		#endregion // BlogLabel
		#region BlogEntryLabel
		public BlogEntryLabel CreateBlogEntryLabel(BlogEntry blogEntryId, BlogLabel blogLabelId)
		{
			if ((object)blogEntryId == null)
			{
				throw new ArgumentNullException("blogEntryId");
			}
			if ((object)blogLabelId == null)
			{
				throw new ArgumentNullException("blogLabelId");
			}
			if (!this.OnBlogEntryLabelBlogEntryIdChanging(null, blogEntryId))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("blogEntryId");
			}
			if (!this.OnBlogEntryLabelBlogLabelIdChanging(null, blogLabelId))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("blogLabelId");
			}
			return new BlogEntryLabelImpl(this, blogEntryId, blogLabelId);
		}
		private bool OnBlogEntryLabelBlogEntryIdChanging(BlogEntryLabel instance, BlogEntry newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnBlogEntryIdAndBlogLabelIdChanging(instance, Tuple.CreateTuple<BlogEntry, BlogLabel>(newValue, instance.BlogLabelId)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnBlogEntryLabelBlogEntryIdChanged(BlogEntryLabel instance, BlogEntry oldValue)
		{
			((ICollection<BlogEntryLabel>)instance.BlogEntryId.BlogEntryLabelViaBlogEntryIdCollection).Add(instance);
			Tuple<BlogEntry, BlogLabel> BlogEntryIdAndBlogLabelIdOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<BlogEntryLabel>)oldValue.BlogEntryLabelViaBlogEntryIdCollection).Remove(instance);
				BlogEntryIdAndBlogLabelIdOldValueTuple = Tuple.CreateTuple<BlogEntry, BlogLabel>(oldValue, instance.BlogLabelId);
			}
			else
			{
				BlogEntryIdAndBlogLabelIdOldValueTuple = null;
			}
			this.OnBlogEntryIdAndBlogLabelIdChanged(instance, BlogEntryIdAndBlogLabelIdOldValueTuple, Tuple.CreateTuple<BlogEntry, BlogLabel>(instance.BlogEntryId, instance.BlogLabelId));
		}
		private bool OnBlogEntryLabelBlogLabelIdChanging(BlogEntryLabel instance, BlogLabel newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			if ((object)instance != null)
			{
				if (!this.OnBlogEntryIdAndBlogLabelIdChanging(instance, Tuple.CreateTuple<BlogEntry, BlogLabel>(instance.BlogEntryId, newValue)))
				{
					return false;
				}
			}
			return true;
		}
		private void OnBlogEntryLabelBlogLabelIdChanged(BlogEntryLabel instance, BlogLabel oldValue)
		{
			((ICollection<BlogEntryLabel>)instance.BlogLabelId.BlogEntryLabelViaBlogLabelIdCollection).Add(instance);
			Tuple<BlogEntry, BlogLabel> BlogEntryIdAndBlogLabelIdOldValueTuple;
			if ((object)oldValue != null)
			{
				((ICollection<BlogEntryLabel>)oldValue.BlogEntryLabelViaBlogLabelIdCollection).Remove(instance);
				BlogEntryIdAndBlogLabelIdOldValueTuple = Tuple.CreateTuple<BlogEntry, BlogLabel>(instance.BlogEntryId, oldValue);
			}
			else
			{
				BlogEntryIdAndBlogLabelIdOldValueTuple = null;
			}
			this.OnBlogEntryIdAndBlogLabelIdChanged(instance, BlogEntryIdAndBlogLabelIdOldValueTuple, Tuple.CreateTuple<BlogEntry, BlogLabel>(instance.BlogEntryId, instance.BlogLabelId));
		}
		private readonly List<BlogEntryLabel> _BlogEntryLabelList;
		private readonly ReadOnlyCollection<BlogEntryLabel> _BlogEntryLabelReadOnlyCollection;
		public IEnumerable<BlogEntryLabel> BlogEntryLabelCollection
		{
			get
			{
				return this._BlogEntryLabelReadOnlyCollection;
			}
		}
		#region BlogEntryLabelImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class BlogEntryLabelImpl : BlogEntryLabel
		{
			public BlogEntryLabelImpl(BlogDemoContext context, BlogEntry blogEntryId, BlogLabel blogLabelId)
			{
				this._Context = context;
				this._BlogEntryId = blogEntryId;
				context.OnBlogEntryLabelBlogEntryIdChanged(this, null);
				this._BlogLabelId = blogLabelId;
				context.OnBlogEntryLabelBlogLabelIdChanged(this, null);
				context._BlogEntryLabelList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private BlogEntry _BlogEntryId;
			public sealed override BlogEntry BlogEntryId
			{
				get
				{
					return this._BlogEntryId;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					BlogEntry oldValue = this._BlogEntryId;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogEntryLabelBlogEntryIdChanging(this, value) && base.OnBlogEntryIdChanging(value))
						{
							this._BlogEntryId = value;
							this._Context.OnBlogEntryLabelBlogEntryIdChanged(this, oldValue);
							base.OnBlogEntryIdChanged(oldValue);
						}
					}
				}
			}
			private BlogLabel _BlogLabelId;
			public sealed override BlogLabel BlogLabelId
			{
				get
				{
					return this._BlogLabelId;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					BlogLabel oldValue = this._BlogLabelId;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnBlogEntryLabelBlogLabelIdChanging(this, value) && base.OnBlogLabelIdChanging(value))
						{
							this._BlogLabelId = value;
							this._Context.OnBlogEntryLabelBlogLabelIdChanged(this, oldValue);
							base.OnBlogLabelIdChanged(oldValue);
						}
					}
				}
			}
		}
		#endregion // BlogEntryLabelImpl
		#endregion // BlogEntryLabel
		#region NonCommentEntry
		public NonCommentEntry CreateNonCommentEntry(BlogEntry blogEntry)
		{
			if ((object)blogEntry == null)
			{
				throw new ArgumentNullException("blogEntry");
			}
			if (!this.OnNonCommentEntryBlogEntryChanging(null, blogEntry))
			{
				throw BlogDemoContext.GetConstraintEnforcementFailedException("blogEntry");
			}
			return new NonCommentEntryImpl(this, blogEntry);
		}
		private bool OnNonCommentEntryBlogEntryChanging(NonCommentEntry instance, BlogEntry newValue)
		{
			if ((object)this != (object)newValue.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException();
			}
			return true;
		}
		private void OnNonCommentEntryBlogEntryChanged(NonCommentEntry instance, BlogEntry oldValue)
		{
			instance.BlogEntry.NonCommentEntry = instance;
			if ((object)oldValue != null)
			{
				oldValue.NonCommentEntry = null;
			}
		}
		private bool OnNonCommentEntryBlogCommentViaParentEntryIdCollectionAdding(NonCommentEntry instance, BlogComment item)
		{
			if ((object)this != (object)item.Context)
			{
				throw BlogDemoContext.GetDifferentContextsException("item");
			}
			return true;
		}
		private void OnNonCommentEntryBlogCommentViaParentEntryIdCollectionAdded(NonCommentEntry instance, BlogComment item)
		{
			item.ParentEntryId = instance;
		}
		private void OnNonCommentEntryBlogCommentViaParentEntryIdCollectionRemoved(NonCommentEntry instance, BlogComment item)
		{
			if ((object)item.ParentEntryId == (object)instance)
			{
				item.ParentEntryId = null;
			}
		}
		private readonly List<NonCommentEntry> _NonCommentEntryList;
		private readonly ReadOnlyCollection<NonCommentEntry> _NonCommentEntryReadOnlyCollection;
		public IEnumerable<NonCommentEntry> NonCommentEntryCollection
		{
			get
			{
				return this._NonCommentEntryReadOnlyCollection;
			}
		}
		#region NonCommentEntryImpl
		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
		private sealed class NonCommentEntryImpl : NonCommentEntry
		{
			public NonCommentEntryImpl(BlogDemoContext context, BlogEntry blogEntry)
			{
				this._Context = context;
				this._BlogCommentViaParentEntryIdCollection = new ConstraintEnforcementCollection<NonCommentEntry, BlogComment>(this);
				this._BlogEntry = blogEntry;
				context.OnNonCommentEntryBlogEntryChanged(this, null);
				context._NonCommentEntryList.Add(this);
			}
			private readonly BlogDemoContext _Context;
			public sealed override BlogDemoContext Context
			{
				get
				{
					return this._Context;
				}
			}
			private BlogEntry _BlogEntry;
			public sealed override BlogEntry BlogEntry
			{
				get
				{
					return this._BlogEntry;
				}
				set
				{
					if ((object)value == null)
					{
						throw new ArgumentNullException("value");
					}
					BlogEntry oldValue = this._BlogEntry;
					if ((object)oldValue != (object)value)
					{
						if (this._Context.OnNonCommentEntryBlogEntryChanging(this, value) && base.OnBlogEntryChanging(value))
						{
							this._BlogEntry = value;
							this._Context.OnNonCommentEntryBlogEntryChanged(this, oldValue);
							base.OnBlogEntryChanged(oldValue);
						}
					}
				}
			}
			private readonly IEnumerable<BlogComment> _BlogCommentViaParentEntryIdCollection;
			public sealed override IEnumerable<BlogComment> BlogCommentViaParentEntryIdCollection
			{
				get
				{
					return this._BlogCommentViaParentEntryIdCollection;
				}
			}
		}
		#endregion // NonCommentEntryImpl
		#endregion // NonCommentEntry
	}
	#endregion // BlogDemoContext
}
