using System;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace NetTiersTestModel.Entities
{
	public partial class EntityBaseCore
	{
		#region Common Columns
		#endregion
	}

	public partial interface IEntity
	{
		#region Common Columns
		#endregion
	}

	#region Item Table
	#region ItemEventArgs class
	/// <summary>
	/// Provides data for the ColumnChanging and ColumnChanged events.
	/// </summary>
	/// <remarks>
	/// The ColumnChanging and ColumnChanged events occur when a change is made to the value 
	/// of a property of a <see cref="Item"/> object.
	/// </remarks>
	public class ItemEventArgs : System.EventArgs
	{
		private ItemColumn column;
		private object value;


		///<summary>
		/// Initalizes a new Instance of the ItemEventArgs class.
		///</summary>
		public ItemEventArgs(ItemColumn column)
		{
			this.column = column;
		}

		///<summary>
		/// Initalizes a new Instance of the ItemEventArgs class.
		///</summary>
		public ItemEventArgs(ItemColumn column, object value)
		{
			this.column = column;
			this.value = value;
		}


		///<summary>
		/// The ItemColumn that was modified, which has raised the event.
		///</summary>
		///<value cref="ItemColumn" />
		public ItemColumn Column
		{
			get
			{
				return this.column;
			}
		}

		/// <summary>
		/// Gets the current value of the column.
		/// </summary>
		/// <value>The current value of the column.</value>
		public object Value
		{
			get
			{
				return this.value;
			}
		}

	}
	#endregion

	///<summary>
	/// Define a delegate for all Item related events.
	///</summary>
	public delegate void ItemEventHandler(object sender, ItemEventArgs e);

	///<summary>
	/// An object representation of the 'Item' table. [No description found the database]	
	///</summary>
	[Serializable, DataObject]
	public abstract partial class ItemBase : EntityBase, IEntityId<ItemKey>, System.IComparable, System.ICloneable, IEditableObject, IComponent, INotifyPropertyChanged
	{
		#region Variable Declarations

		/// <summary>
		/// 	Old the inner data of the entity.
		/// </summary>
		private ItemEntityData entityData;

		// <summary>
		// 	Old the original data of the entity.
		// </summary>
		//ItemEntityData originalData;

		/// <summary>
		/// 	Old a backup of the inner data of the entity.
		/// </summary>
		private ItemEntityData backupData;

		/// <summary>
		/// 	Key used if Tracking is Enabled for the <see cref="EntityLocator" />.
		/// </summary>
		private string entityTrackingKey;

		[NonSerialized]
		private TList<Item> parentCollection;
		private bool inTxn = false;


		/// <summary>
		/// Occurs when a value is being changed for the specified column.
		/// </summary>	
		[field: NonSerialized]
		public event ItemEventHandler ColumnChanging;


		/// <summary>
		/// Occurs after a value has been changed for the specified column.
		/// </summary>
		[field: NonSerialized]
		public event ItemEventHandler ColumnChanged;
		#endregion "Variable Declarations"

		#region Constructors
		///<summary>
		/// Creates a new <see cref="ItemBase"/> instance.
		///</summary>
		public ItemBase()
		{
			this.entityData = new ItemEntityData();
			this.backupData = null;
		}

		///<summary>
		/// Creates a new <see cref="ItemBase"/> instance.
		///</summary>
		///<param name="itemPrice"></param>
		///<param name="itemName"></param>
		///<param name="itemItemID"></param>
		public ItemBase(System.String itemPrice, System.String itemName, System.Int64 itemItemID)
		{
			this.entityData = new ItemEntityData();
			this.backupData = null;

			this.Price = itemPrice;
			this.Name = itemName;
			this.ItemID = itemItemID;
		}

		///<summary>
		/// A simple factory method to create a new <see cref="Item"/> instance.
		///</summary>
		///<param name="itemPrice"></param>
		///<param name="itemName"></param>
		///<param name="itemItemID"></param>
		public static Item CreateItem(System.String itemPrice, System.String itemName, System.Int64 itemItemID)
		{
			Item newItem = new Item();
			newItem.Price = itemPrice;
			newItem.Name = itemName;
			newItem.ItemID = itemItemID;
			return newItem;
		}

		#endregion Constructors


		#region Events trigger
		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ItemColumn"/> which has raised the event.</param>
		public void OnColumnChanging(ItemColumn column)
		{
			OnColumnChanging(column, null);
			return;
		}

		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ItemColumn"/> which has raised the event.</param>
		public void OnColumnChanged(ItemColumn column)
		{
			OnColumnChanged(column, null);
			return;
		}


		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ItemColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public void OnColumnChanging(ItemColumn column, object value)
		{
			if (IsEntityTracked && EntityState != EntityState.Added)
				EntityManager.StopTracking(EntityTrackingKey);

			if (!SuppressEntityEvents)
			{
				ItemEventHandler handler = ColumnChanging;
				if (handler != null)
				{
					handler(this, new ItemEventArgs(column, value));
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ItemColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public void OnColumnChanged(ItemColumn column, object value)
		{
			if (!SuppressEntityEvents)
			{
				ItemEventHandler handler = ColumnChanged;
				if (handler != null)
				{
					handler(this, new ItemEventArgs(column, value));
				}

				// warn the parent list that i have changed
				OnEntityChanged();
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// 	Gets or sets the Price property. 
		///		
		/// </summary>
		/// <value>This type is character varying.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(false, false, false, 100)]
		public virtual System.String Price
		{
			get
			{
				return this.entityData.Price;
			}

			set
			{
				if (this.entityData.Price == value)
					return;


				OnColumnChanging(ItemColumn.Price, this.entityData.Price);
				this.entityData.Price = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(ItemColumn.Price, this.entityData.Price);
				OnPropertyChanged("Price");
			}
		}

		/// <summary>
		/// 	Gets or sets the Name property. 
		///		
		/// </summary>
		/// <value>This type is character varying.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(true, false, false, 100)]
		public virtual System.String Name
		{
			get
			{
				return this.entityData.Name;
			}

			set
			{
				if (this.entityData.Name == value)
					return;


				OnColumnChanging(ItemColumn.Name, this.entityData.Name);
				this.entityData.Name = value;
				this.EntityId.Name = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(ItemColumn.Name, this.entityData.Name);
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// 	Get the original value of the Name property.
		///		
		/// </summary>
		/// <remarks>This is the original value of the Name property.</remarks>
		/// <value>This type is character varying</value>
		[BrowsableAttribute(false)/*, XmlIgnoreAttribute()*/]
		public virtual System.String OriginalName
		{
			get
			{
				return this.entityData.OriginalName;
			}
			set
			{
				this.entityData.OriginalName = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the ItemID property. 
		///		
		/// </summary>
		/// <value>This type is bigint.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(false, false, false)]
		public virtual System.Int64 ItemID
		{
			get
			{
				return this.entityData.ItemID;
			}

			set
			{
				if (this.entityData.ItemID == value)
					return;


				OnColumnChanging(ItemColumn.ItemID, this.entityData.ItemID);
				this.entityData.ItemID = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(ItemColumn.ItemID, this.entityData.ItemID);
				OnPropertyChanged("ItemID");
			}
		}


		#region Source Foreign Key Property

		#endregion

		#region Table Meta Data
		/// <summary>
		///		The name of the underlying database table.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string TableName
		{
			get
			{
				return "Item";
			}
		}

		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string[] TableColumns
		{
			get
			{
				return new string[] { "Price", "Name", "Item_ID" };
			}
		}
		#endregion


		/// <summary>
		///	Holds a collection of Order objects
		///	which are related to this object through the relation Item_FK
		/// </summary>	
		[BindableAttribute()]
		public TList<Order> OrderCollection
		{
			get
			{
				return entityData.OrderCollection;
			}
			set
			{
				entityData.OrderCollection = value;
			}
		}

		#endregion

		#region IEditableObject

		#region  CancelAddNew Event
		/// <summary>
		/// The delegate for the CancelAddNew event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void CancelAddNewEventHandler(object sender, EventArgs e);

		/// <summary>
		/// The CancelAddNew event.
		/// </summary>
		[field: NonSerialized]
		public event CancelAddNewEventHandler CancelAddNew;

		/// <summary>
		/// Called when [cancel add new].
		/// </summary>
		public void OnCancelAddNew()
		{
			if (!SuppressEntityEvents)
			{
				CancelAddNewEventHandler handler = CancelAddNew;
				if (handler != null)
				{
					handler(this, EventArgs.Empty);
				}
			}
		}
		#endregion

		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		void IEditableObject.BeginEdit()
		{
			//Console.WriteLine("Start BeginEdit");
			if (!inTxn)
			{
				this.backupData = this.entityData.Clone() as ItemEntityData;
				inTxn = true;
				//Console.WriteLine("BeginEdit");
			}
			//Console.WriteLine("End BeginEdit");
		}

		/// <summary>
		/// Discards changes since the last <c>BeginEdit</c> call.
		/// </summary>
		void IEditableObject.CancelEdit()
		{
			//Console.WriteLine("Start CancelEdit");
			if (this.inTxn)
			{
				this.entityData = this.backupData;
				this.backupData = null;
				this.inTxn = false;

				if (this.bindingIsNew)
				//if (this.EntityState == EntityState.Added)
				{
					if (this.parentCollection != null)
						this.parentCollection.Remove((Item)this);
				}
			}
			//Console.WriteLine("End CancelEdit");
		}

		/// <summary>
		/// Pushes changes since the last <c>BeginEdit</c> or <c>IBindingList.AddNew</c> call into the underlying object.
		/// </summary>
		void IEditableObject.EndEdit()
		{
			//Console.WriteLine("Start EndEdit" + this.custData.id + this.custData.lastName);
			if (this.inTxn)
			{
				this.backupData = null;
				if (this.IsDirty)
				{
					if (this.bindingIsNew)
					{
						this.EntityState = EntityState.Added;
						this.bindingIsNew = false;
					}
					else
						if (this.EntityState == EntityState.Unchanged)
							this.EntityState = EntityState.Changed;
				}

				this.bindingIsNew = false;
				this.inTxn = false;
			}
			//Console.WriteLine("End EndEdit");
		}

		/// <summary>
		/// Gets or sets the parent collection.
		/// </summary>
		/// <value>The parent collection.</value>
		[XmlIgnore]
		[Browsable(false)]
		public override object ParentCollection
		{
			get
			{
				return (object)this.parentCollection;
			}
			set
			{
				this.parentCollection = value as TList<Item>;
			}
		}

		/// <summary>
		/// Called when the entity is changed.
		/// </summary>
		private void OnEntityChanged()
		{
			if (!SuppressEntityEvents && !inTxn && this.parentCollection != null)
			{
				this.parentCollection.EntityChanged(this as Item);
			}
		}


		#endregion

		#region Methods

		///<summary>
		///  Revert all changes and restore original values.
		///  Currently not supported.
		///</summary>
		public override void CancelChanges()
		{
			IEditableObject obj = (IEditableObject)this;
			obj.CancelEdit();
		}

		#region ICloneable Members
		///<summary>
		///  Returns a Typed Item Entity 
		///</summary>
		public virtual Item Copy()
		{
			//shallow copy entity
			Item copy = new Item();
			copy.Price = this.Price;
			copy.Name = this.Name;
			copy.OriginalName = this.OriginalName;
			copy.ItemID = this.ItemID;

			copy.AcceptChanges();
			return (Item)copy;
		}

		///<summary>
		/// ICloneable.Clone() Member, returns the Shallow Copy of this entity.
		///</summary>
		public object Clone()
		{
			return this.Copy();
		}

		///<summary>
		/// Returns a deep copy of the child collection object passed in.
		///</summary>
		public static object MakeCopyOf(object x)
		{
			if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
				throw new System.NotSupportedException("Object Does Not Implement the ICloneable Interface.");
		}

		///<summary>
		///  Returns a Typed Item Entity which is a deep copy of the current entity.
		///</summary>
		public virtual Item DeepCopy()
		{
			return EntityHelper.Clone<Item>(this as Item);
		}
		#endregion

		///<summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		///</summary>
		///<param name="toObject">An object to compare to this instance.</param>
		///<returns>true if toObject is a <see cref="ItemBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(ItemBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}


		///<summary>
		/// Determines whether the specified <see cref="ItemBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="ItemBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="ItemBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(ItemBase Object1, ItemBase Object2)
		{
			// both are null
			if (Object1 == null && Object2 == null)
				return true;

			// one or the other is null, but not both
			if (Object1 == null ^ Object2 == null)
				return false;

			bool equal = true;
			if (Object1.Price != Object2.Price)
				equal = false;
			if (Object1.Name != Object2.Name)
				equal = false;
			if (Object1.ItemID != Object2.ItemID)
				equal = false;
			return equal;
		}

		#endregion

		#region IComparable Members
		///<summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		///<param name="obj">An object to compare to this instance, or a null reference (Nothing in Visual Basic).</param>
		///</summary>
		///<returns>A signed integer that indicates the relative order of this instance and obj.</returns>
		public virtual int CompareTo(object obj)
		{
			throw new NotImplementedException();
			// TODO -> generate a strongly typed IComparer in the concrete class
			//return this. GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name) .CompareTo(((ItemBase)obj).GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name));
		}

		/*
		// static method to get a Comparer object
		public static ItemComparer GetComparer()
		{
			return new ItemComparer();
		}
		*/

		// Comparer delegates back to Item
		// Employee uses the integer's default
		// CompareTo method
		/*
		public int CompareTo(Item rhs)
		{
			return this.Id.CompareTo(rhs.Id);
		}
		*/

		/*
				// Special implementation to be called by custom comparer
				public int CompareTo(Item rhs, ItemColumn which)
				{
					switch (which)
					{
            	
            	
						case ItemColumn.Price:
							return this.Price.CompareTo(rhs.Price);
            		
            		                 
            	
            	
						case ItemColumn.Name:
							return this.Name.CompareTo(rhs.Name);
            		
            		                 
            	
            	
						case ItemColumn.ItemID:
							return this.ItemID.CompareTo(rhs.ItemID);
            		
            		                 
					}
					return 0;
				}
				*/

		#endregion

		#region IComponent Members

		private ISite _site = null;

		/// <summary>
		/// Gets or Sets the site where this data is located.
		/// </summary>
		[XmlIgnore]
		[SoapIgnore]
		[Browsable(false)]
		public ISite Site
		{
			get
			{
				return this._site;
			}
			set
			{
				this._site = value;
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Notify those that care when we dispose.
		/// </summary>
		[field: NonSerialized]
		public event System.EventHandler Disposed;

		/// <summary>
		/// Clean up. Nothing here though.
		/// </summary>
		public void Dispose()
		{
			this.parentCollection = null;
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				EventHandler handler = Disposed;
				if (handler != null)
					handler(this, EventArgs.Empty);
			}
		}

		#endregion

		#region IEntityKey<ItemKey> Members

		// member variable for the EntityId property
		private ItemKey _entityId;

		/// <summary>
		/// Gets or sets the EntityId property.
		/// </summary>
		[XmlIgnore]
		public ItemKey EntityId
		{
			get
			{
				if (_entityId == null)
				{
					_entityId = new ItemKey(this);
				}

				return _entityId;
			}
			set
			{
				if (value != null)
				{
					value.Entity = this;
				}

				_entityId = value;
			}
		}

		#endregion

		#region EntityTrackingKey
		///<summary>
		/// Provides the tracking key for the <see cref="EntityLocator"/>
		///</summary>
		[XmlIgnore]
		public override string EntityTrackingKey
		{
			get
			{
				if (entityTrackingKey == null)
					entityTrackingKey = @"Item"
					+ this.Name.ToString();
				return entityTrackingKey;
			}
			set
			{
				if (value != null)
					entityTrackingKey = value;
			}
		}
		#endregion

		#region ToString Method

		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{4}{3}- Price: {0}{3}- Name: {1}{3}- ItemID: {2}{3}",
				this.Price,
				this.Name,
				this.ItemID,
				Environment.NewLine,
				this.GetType());
		}

		#endregion ToString Method

		#region Inner data class

		/// <summary>
		///		The data structure representation of the 'Item' table.
		/// </summary>
		/// <remarks>
		/// 	This struct is generated by a tool and should never be modified.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Serializable]
		internal class ItemEntityData : ICloneable
		{
			#region Variable Declarations

			#region Primary key(s)
			/// <summary>			
			/// Name : 
			/// </summary>
			/// <remarks>Member of the primary key of the underlying table "Item"</remarks>
			public System.String Name;

			/// <summary>
			/// keep a copy of the original so it can be used for editable primary keys.
			/// </summary>
			public System.String OriginalName;

			#endregion

			#region Non Primary key(s)


			/// <summary>
			/// Price : 
			/// </summary>
			public System.String Price = string.Empty;

			/// <summary>
			/// Item_ID : 
			/// </summary>
			public System.Int64 ItemID = (long)0;
			#endregion

			#endregion Variable Declarations

			#region Clone
			public Object Clone()
			{
				ItemEntityData _tmp = new ItemEntityData();

				_tmp.Name = this.Name;
				_tmp.OriginalName = this.OriginalName;

				_tmp.Price = this.Price;
				_tmp.ItemID = this.ItemID;

				return _tmp;
			}
			#endregion

			#region Data Properties

			#region OrderCollection

			private TList<Order> orderItemItemID;

			/// <summary>
			///	Holds a collection of entity objects
			///	which are related to this object through the relation orderItemItemID
			/// </summary>	
			public TList<Order> OrderCollection
			{
				get
				{
					if (orderItemItemID == null)
					{
						orderItemItemID = new TList<Order>();
					}

					return orderItemItemID;
				}
				set
				{
					orderItemItemID = value;
				}
			}

			#endregion

			#endregion Data Properties

		}//End struct


		#endregion

		#region Validation

		/// <summary>
		/// Assigns validation rules to this object based on model definition.
		/// </summary>
		/// <remarks>This method overrides the base class to add schema related validation.</remarks>
		protected override void AddValidationRules()
		{
			//Validation rules based on database schema.
			ValidationRules.AddRule(Validation.CommonRules.NotNull, "Price");
			ValidationRules.AddRule(Validation.CommonRules.StringMaxLength, new Validation.CommonRules.MaxLengthRuleArgs("Price", 100));
			ValidationRules.AddRule(Validation.CommonRules.NotNull, "Name");
			ValidationRules.AddRule(Validation.CommonRules.StringMaxLength, new Validation.CommonRules.MaxLengthRuleArgs("Name", 100));
		}
		#endregion

	} // End Class

	#region ItemComparer

	/// <summary>
	///	Strongly Typed IComparer
	/// </summary>
	public class ItemComparer : System.Collections.Generic.IComparer<Item>
	{
		ItemColumn whichComparison;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ItemComparer"/> class.
		/// </summary>
		public ItemComparer()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:%=className%>Comparer"/> class.
		/// </summary>
		/// <param name="column">The column to sort on.</param>
		public ItemComparer(ItemColumn column)
		{
			this.whichComparison = column;
		}

		/// <summary>
		/// Determines whether the specified <c cref="Item"/> instances are considered equal.
		/// </summary>
		/// <param name="a">The first <c cref="Item"/> to compare.</param>
		/// <param name="b">The second <c>Item</c> to compare.</param>
		/// <returns>true if objA is the same instance as objB or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public bool Equals(Item a, Item b)
		{
			return this.Compare(a, b) == 0;
		}

		/// <summary>
		/// Gets the hash code of the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public int GetHashCode(Item entity)
		{
			return entity.GetHashCode();
		}

		/// <summary>
		/// Performs a case-insensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="a">The first object to compare.</param>
		/// <param name="b">The second object to compare.</param>
		/// <returns></returns>
		public int Compare(Item a, Item b)
		{
			EntityPropertyComparer entityPropertyComparer = new EntityPropertyComparer(this.whichComparison.ToString());
			return entityPropertyComparer.Compare(a, b);
		}

		/// <summary>
		/// Gets or sets the column that will be used for comparison.
		/// </summary>
		/// <value>The comparison column.</value>
		public ItemColumn WhichComparison
		{
			get
			{
				return this.whichComparison;
			}
			set
			{
				this.whichComparison = value;
			}
		}
	}

	#endregion

	#region ItemKey Class

	/// <summary>
	/// Wraps the unique identifier values for the <see cref="Item"/> object.
	/// </summary>
	[Serializable]
	public class ItemKey : EntityKeyBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ItemKey class.
		/// </summary>
		public ItemKey()
		{
		}

		/// <summary>
		/// Initializes a new instance of the ItemKey class.
		/// </summary>
		public ItemKey(ItemBase entity)
		{
			Entity = entity;

			#region Init Properties

			if (entity != null)
			{
				this.name = entity.Name;
			}

			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the ItemKey class.
		/// </summary>
		public ItemKey(System.String name)
		{
			#region Init Properties

			this.name = name;

			#endregion
		}

		#endregion Constructors

		#region Properties

		// member variable for the Entity property
		private ItemBase _entity;

		/// <summary>
		/// Gets or sets the Entity property.
		/// </summary>
		public ItemBase Entity
		{
			get
			{
				return _entity;
			}
			set
			{
				_entity = value;
			}
		}

		// member variable for the Name property
		private System.String name;

		/// <summary>
		/// Gets or sets the Name property.
		/// </summary>
		public System.String Name
		{
			get
			{
				return name;
			}
			set
			{
				if (Entity != null)
				{
					Entity.Name = value;
				}

				name = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads values from the supplied <see cref="IDictionary"/> object into
		/// properties of the current object.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> instance that contains
		/// the key/value pairs to be used as property values.</param>
		public override void Load(IDictionary values)
		{
			#region Init Properties

			if (values != null)
			{
				Name = (values["Name"] != null) ? (System.String)EntityUtil.ChangeType(values["Name"], typeof(System.String)) : string.Empty;
			}

			#endregion
		}

		/// <summary>
		/// Creates a new <see cref="IDictionary"/> object and populates it
		/// with the property values of the current object.
		/// </summary>
		/// <returns>A collection of name/value pairs.</returns>
		public override IDictionary ToDictionary()
		{
			IDictionary values = new Hashtable();

			#region Init Dictionary

			values.Add("Name", Name);

			#endregion Init Dictionary

			return values;
		}

		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return String.Format("Name: {0}{1}",
								Name,
								Environment.NewLine);
		}

		#endregion Methods
	}

	#endregion

	#region ItemColumn Enum

	/// <summary>
	/// Enumerate the Item columns.
	/// </summary>
	[Serializable]
	public enum ItemColumn : int
	{
		/// <summary>
		/// Price : 
		/// </summary>
		[EnumTextValue("Price")]
		[ColumnEnum("Price", typeof(System.String), System.Data.DbType.String, false, false, false, 100)]
		Price = 1,
		/// <summary>
		/// Name : 
		/// </summary>
		[EnumTextValue("Name")]
		[ColumnEnum("Name", typeof(System.String), System.Data.DbType.String, true, false, false, 100)]
		Name = 2,
		/// <summary>
		/// ItemID : 
		/// </summary>
		[EnumTextValue("Item_ID")]
		[ColumnEnum("Item_ID", typeof(System.Int64), System.Data.DbType.Int64, false, false, false)]
		ItemID = 3
	}//End enum

	#endregion ItemColumn Enum
	#endregion

	#region Order Table
	#region OrderEventArgs class
	/// <summary>
	/// Provides data for the ColumnChanging and ColumnChanged events.
	/// </summary>
	/// <remarks>
	/// The ColumnChanging and ColumnChanged events occur when a change is made to the value 
	/// of a property of a <see cref="Order"/> object.
	/// </remarks>
	public class OrderEventArgs : System.EventArgs
	{
		private OrderColumn column;
		private object value;


		///<summary>
		/// Initalizes a new Instance of the OrderEventArgs class.
		///</summary>
		public OrderEventArgs(OrderColumn column)
		{
			this.column = column;
		}

		///<summary>
		/// Initalizes a new Instance of the OrderEventArgs class.
		///</summary>
		public OrderEventArgs(OrderColumn column, object value)
		{
			this.column = column;
			this.value = value;
		}


		///<summary>
		/// The OrderColumn that was modified, which has raised the event.
		///</summary>
		///<value cref="OrderColumn" />
		public OrderColumn Column
		{
			get
			{
				return this.column;
			}
		}

		/// <summary>
		/// Gets the current value of the column.
		/// </summary>
		/// <value>The current value of the column.</value>
		public object Value
		{
			get
			{
				return this.value;
			}
		}

	}
	#endregion

	///<summary>
	/// Define a delegate for all Order related events.
	///</summary>
	public delegate void OrderEventHandler(object sender, OrderEventArgs e);

	///<summary>
	/// An object representation of the '"Order"' table. [No description found the database]	
	///</summary>
	[Serializable, DataObject]
	public abstract partial class OrderBase : EntityBase, IEntityId<OrderKey>, System.IComparable, System.ICloneable, IEditableObject, IComponent, INotifyPropertyChanged
	{
		#region Variable Declarations

		/// <summary>
		/// 	Old the inner data of the entity.
		/// </summary>
		private OrderEntityData entityData;

		// <summary>
		// 	Old the original data of the entity.
		// </summary>
		//OrderEntityData originalData;

		/// <summary>
		/// 	Old a backup of the inner data of the entity.
		/// </summary>
		private OrderEntityData backupData;

		/// <summary>
		/// 	Key used if Tracking is Enabled for the <see cref="EntityLocator" />.
		/// </summary>
		private string entityTrackingKey;

		[NonSerialized]
		private TList<Order> parentCollection;
		private bool inTxn = false;


		/// <summary>
		/// Occurs when a value is being changed for the specified column.
		/// </summary>	
		[field: NonSerialized]
		public event OrderEventHandler ColumnChanging;


		/// <summary>
		/// Occurs after a value has been changed for the specified column.
		/// </summary>
		[field: NonSerialized]
		public event OrderEventHandler ColumnChanged;
		#endregion "Variable Declarations"

		#region Constructors
		///<summary>
		/// Creates a new <see cref="OrderBase"/> instance.
		///</summary>
		public OrderBase()
		{
			this.entityData = new OrderEntityData();
			this.backupData = null;
		}

		///<summary>
		/// Creates a new <see cref="OrderBase"/> instance.
		///</summary>
		///<param name="orderPersonPersonID"></param>
		///<param name="orderOrderID"></param>
		///<param name="orderItemItemID"></param>
		public OrderBase(System.Int64 orderPersonPersonID, System.Int64 orderOrderID, System.Int64 orderItemItemID)
		{
			this.entityData = new OrderEntityData();
			this.backupData = null;

			this.PersonPersonID = orderPersonPersonID;
			this.OrderID = orderOrderID;
			this.ItemItemID = orderItemItemID;
		}

		///<summary>
		/// A simple factory method to create a new <see cref="Order"/> instance.
		///</summary>
		///<param name="orderPersonPersonID"></param>
		///<param name="orderOrderID"></param>
		///<param name="orderItemItemID"></param>
		public static Order CreateOrder(System.Int64 orderPersonPersonID, System.Int64 orderOrderID, System.Int64 orderItemItemID)
		{
			Order newOrder = new Order();
			newOrder.PersonPersonID = orderPersonPersonID;
			newOrder.OrderID = orderOrderID;
			newOrder.ItemItemID = orderItemItemID;
			return newOrder;
		}

		#endregion Constructors


		#region Events trigger
		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="OrderColumn"/> which has raised the event.</param>
		public void OnColumnChanging(OrderColumn column)
		{
			OnColumnChanging(column, null);
			return;
		}

		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="OrderColumn"/> which has raised the event.</param>
		public void OnColumnChanged(OrderColumn column)
		{
			OnColumnChanged(column, null);
			return;
		}


		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="OrderColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public void OnColumnChanging(OrderColumn column, object value)
		{
			if (IsEntityTracked && EntityState != EntityState.Added)
				EntityManager.StopTracking(EntityTrackingKey);

			if (!SuppressEntityEvents)
			{
				OrderEventHandler handler = ColumnChanging;
				if (handler != null)
				{
					handler(this, new OrderEventArgs(column, value));
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="OrderColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public void OnColumnChanged(OrderColumn column, object value)
		{
			if (!SuppressEntityEvents)
			{
				OrderEventHandler handler = ColumnChanged;
				if (handler != null)
				{
					handler(this, new OrderEventArgs(column, value));
				}

				// warn the parent list that i have changed
				OnEntityChanged();
			}
		}
		#endregion

		#region Properties

		/// <summary>
		/// 	Gets or sets the PersonPersonID property. 
		///		
		/// </summary>
		/// <value>This type is bigint.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(false, false, false)]
		public virtual System.Int64 PersonPersonID
		{
			get
			{
				return this.entityData.PersonPersonID;
			}

			set
			{
				if (this.entityData.PersonPersonID == value)
					return;


				OnColumnChanging(OrderColumn.PersonPersonID, this.entityData.PersonPersonID);
				this.entityData.PersonPersonID = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(OrderColumn.PersonPersonID, this.entityData.PersonPersonID);
				OnPropertyChanged("PersonPersonID");
			}
		}

		/// <summary>
		/// 	Gets or sets the OrderID property. 
		///		
		/// </summary>
		/// <value>This type is bigint.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(true, false, false)]
		public virtual System.Int64 OrderID
		{
			get
			{
				return this.entityData.OrderID;
			}

			set
			{
				if (this.entityData.OrderID == value)
					return;


				OnColumnChanging(OrderColumn.OrderID, this.entityData.OrderID);
				this.entityData.OrderID = value;
				this.EntityId.OrderID = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(OrderColumn.OrderID, this.entityData.OrderID);
				OnPropertyChanged("OrderID");
			}
		}

		/// <summary>
		/// 	Get the original value of the Order_ID property.
		///		
		/// </summary>
		/// <remarks>This is the original value of the Order_ID property.</remarks>
		/// <value>This type is bigint</value>
		[BrowsableAttribute(false)/*, XmlIgnoreAttribute()*/]
		public virtual System.Int64 OriginalOrderID
		{
			get
			{
				return this.entityData.OriginalOrderID;
			}
			set
			{
				this.entityData.OriginalOrderID = value;
			}
		}

		/// <summary>
		/// 	Gets or sets the ItemItemID property. 
		///		
		/// </summary>
		/// <value>This type is bigint.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), BindableAttribute()]
		[DataObjectField(false, false, false)]
		public virtual System.Int64 ItemItemID
		{
			get
			{
				return this.entityData.ItemItemID;
			}

			set
			{
				if (this.entityData.ItemItemID == value)
					return;


				OnColumnChanging(OrderColumn.ItemItemID, this.entityData.ItemItemID);
				this.entityData.ItemItemID = value;
				if (this.EntityState == EntityState.Unchanged)
				{
					this.EntityState = EntityState.Changed;
				}
				OnColumnChanged(OrderColumn.ItemItemID, this.entityData.ItemItemID);
				OnPropertyChanged("ItemItemID");
			}
		}


		#region Source Foreign Key Property

		private Item _itemItemIDSource = null;

		/// <summary>
		/// Gets or sets the source <see cref="Item"/>.
		/// </summary>
		/// <value>The source Item for ItemItemID.</value>
		[XmlIgnore()]
		[Browsable(false), BindableAttribute()]
		public virtual Item ItemItemIDSource
		{
			get
			{
				return this._itemItemIDSource;
			}
			set
			{
				this._itemItemIDSource = value;
			}
		}
		#endregion

		#region Table Meta Data
		/// <summary>
		///		The name of the underlying database table.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string TableName
		{
			get
			{
				return "Order";
			}
		}

		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string[] TableColumns
		{
			get
			{
				return new string[] { "Person_Person_ID", "Order_ID", "Item_Item_ID" };
			}
		}
		#endregion


		#endregion

		#region IEditableObject

		#region  CancelAddNew Event
		/// <summary>
		/// The delegate for the CancelAddNew event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void CancelAddNewEventHandler(object sender, EventArgs e);

		/// <summary>
		/// The CancelAddNew event.
		/// </summary>
		[field: NonSerialized]
		public event CancelAddNewEventHandler CancelAddNew;

		/// <summary>
		/// Called when [cancel add new].
		/// </summary>
		public void OnCancelAddNew()
		{
			if (!SuppressEntityEvents)
			{
				CancelAddNewEventHandler handler = CancelAddNew;
				if (handler != null)
				{
					handler(this, EventArgs.Empty);
				}
			}
		}
		#endregion

		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		void IEditableObject.BeginEdit()
		{
			//Console.WriteLine("Start BeginEdit");
			if (!inTxn)
			{
				this.backupData = this.entityData.Clone() as OrderEntityData;
				inTxn = true;
				//Console.WriteLine("BeginEdit");
			}
			//Console.WriteLine("End BeginEdit");
		}

		/// <summary>
		/// Discards changes since the last <c>BeginEdit</c> call.
		/// </summary>
		void IEditableObject.CancelEdit()
		{
			//Console.WriteLine("Start CancelEdit");
			if (this.inTxn)
			{
				this.entityData = this.backupData;
				this.backupData = null;
				this.inTxn = false;

				if (this.bindingIsNew)
				//if (this.EntityState == EntityState.Added)
				{
					if (this.parentCollection != null)
						this.parentCollection.Remove((Order)this);
				}
			}
			//Console.WriteLine("End CancelEdit");
		}

		/// <summary>
		/// Pushes changes since the last <c>BeginEdit</c> or <c>IBindingList.AddNew</c> call into the underlying object.
		/// </summary>
		void IEditableObject.EndEdit()
		{
			//Console.WriteLine("Start EndEdit" + this.custData.id + this.custData.lastName);
			if (this.inTxn)
			{
				this.backupData = null;
				if (this.IsDirty)
				{
					if (this.bindingIsNew)
					{
						this.EntityState = EntityState.Added;
						this.bindingIsNew = false;
					}
					else
						if (this.EntityState == EntityState.Unchanged)
							this.EntityState = EntityState.Changed;
				}

				this.bindingIsNew = false;
				this.inTxn = false;
			}
			//Console.WriteLine("End EndEdit");
		}

		/// <summary>
		/// Gets or sets the parent collection.
		/// </summary>
		/// <value>The parent collection.</value>
		[XmlIgnore]
		[Browsable(false)]
		public override object ParentCollection
		{
			get
			{
				return (object)this.parentCollection;
			}
			set
			{
				this.parentCollection = value as TList<Order>;
			}
		}

		/// <summary>
		/// Called when the entity is changed.
		/// </summary>
		private void OnEntityChanged()
		{
			if (!SuppressEntityEvents && !inTxn && this.parentCollection != null)
			{
				this.parentCollection.EntityChanged(this as Order);
			}
		}


		#endregion

		#region Methods

		///<summary>
		///  Revert all changes and restore original values.
		///  Currently not supported.
		///</summary>
		public override void CancelChanges()
		{
			IEditableObject obj = (IEditableObject)this;
			obj.CancelEdit();
		}

		#region ICloneable Members
		///<summary>
		///  Returns a Typed Order Entity 
		///</summary>
		public virtual Order Copy()
		{
			//shallow copy entity
			Order copy = new Order();
			copy.PersonPersonID = this.PersonPersonID;
			copy.OrderID = this.OrderID;
			copy.OriginalOrderID = this.OriginalOrderID;
			copy.ItemItemID = this.ItemItemID;

			copy.AcceptChanges();
			return (Order)copy;
		}

		///<summary>
		/// ICloneable.Clone() Member, returns the Shallow Copy of this entity.
		///</summary>
		public object Clone()
		{
			return this.Copy();
		}

		///<summary>
		/// Returns a deep copy of the child collection object passed in.
		///</summary>
		public static object MakeCopyOf(object x)
		{
			if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
				throw new System.NotSupportedException("Object Does Not Implement the ICloneable Interface.");
		}

		///<summary>
		///  Returns a Typed Order Entity which is a deep copy of the current entity.
		///</summary>
		public virtual Order DeepCopy()
		{
			return EntityHelper.Clone<Order>(this as Order);
		}
		#endregion

		///<summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		///</summary>
		///<param name="toObject">An object to compare to this instance.</param>
		///<returns>true if toObject is a <see cref="OrderBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(OrderBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}


		///<summary>
		/// Determines whether the specified <see cref="OrderBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="OrderBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="OrderBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(OrderBase Object1, OrderBase Object2)
		{
			// both are null
			if (Object1 == null && Object2 == null)
				return true;

			// one or the other is null, but not both
			if (Object1 == null ^ Object2 == null)
				return false;

			bool equal = true;
			if (Object1.PersonPersonID != Object2.PersonPersonID)
				equal = false;
			if (Object1.OrderID != Object2.OrderID)
				equal = false;
			if (Object1.ItemItemID != Object2.ItemItemID)
				equal = false;
			return equal;
		}

		#endregion

		#region IComparable Members
		///<summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		///<param name="obj">An object to compare to this instance, or a null reference (Nothing in Visual Basic).</param>
		///</summary>
		///<returns>A signed integer that indicates the relative order of this instance and obj.</returns>
		public virtual int CompareTo(object obj)
		{
			throw new NotImplementedException();
			// TODO -> generate a strongly typed IComparer in the concrete class
			//return this. GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name) .CompareTo(((OrderBase)obj).GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0].Name));
		}

		/*
		// static method to get a Comparer object
		public static OrderComparer GetComparer()
		{
			return new OrderComparer();
		}
		*/

		// Comparer delegates back to Order
		// Employee uses the integer's default
		// CompareTo method
		/*
		public int CompareTo(Item rhs)
		{
			return this.Id.CompareTo(rhs.Id);
		}
		*/

		/*
				// Special implementation to be called by custom comparer
				public int CompareTo(Order rhs, OrderColumn which)
				{
					switch (which)
					{
            	
            	
						case OrderColumn.PersonPersonID:
							return this.PersonPersonID.CompareTo(rhs.PersonPersonID);
            		
            		                 
            	
            	
						case OrderColumn.OrderID:
							return this.OrderID.CompareTo(rhs.OrderID);
            		
            		                 
            	
            	
						case OrderColumn.ItemItemID:
							return this.ItemItemID.CompareTo(rhs.ItemItemID);
            		
            		                 
					}
					return 0;
				}
				*/

		#endregion

		#region IComponent Members

		private ISite _site = null;

		/// <summary>
		/// Gets or Sets the site where this data is located.
		/// </summary>
		[XmlIgnore]
		[SoapIgnore]
		[Browsable(false)]
		public ISite Site
		{
			get
			{
				return this._site;
			}
			set
			{
				this._site = value;
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Notify those that care when we dispose.
		/// </summary>
		[field: NonSerialized]
		public event System.EventHandler Disposed;

		/// <summary>
		/// Clean up. Nothing here though.
		/// </summary>
		public void Dispose()
		{
			this.parentCollection = null;
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				EventHandler handler = Disposed;
				if (handler != null)
					handler(this, EventArgs.Empty);
			}
		}

		#endregion

		#region IEntityKey<OrderKey> Members

		// member variable for the EntityId property
		private OrderKey _entityId;

		/// <summary>
		/// Gets or sets the EntityId property.
		/// </summary>
		[XmlIgnore]
		public OrderKey EntityId
		{
			get
			{
				if (_entityId == null)
				{
					_entityId = new OrderKey(this);
				}

				return _entityId;
			}
			set
			{
				if (value != null)
				{
					value.Entity = this;
				}

				_entityId = value;
			}
		}

		#endregion

		#region EntityTrackingKey
		///<summary>
		/// Provides the tracking key for the <see cref="EntityLocator"/>
		///</summary>
		[XmlIgnore]
		public override string EntityTrackingKey
		{
			get
			{
				if (entityTrackingKey == null)
					entityTrackingKey = @"Order"
					+ this.OrderID.ToString();
				return entityTrackingKey;
			}
			set
			{
				if (value != null)
					entityTrackingKey = value;
			}
		}
		#endregion

		#region ToString Method

		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{4}{3}- PersonPersonID: {0}{3}- OrderID: {1}{3}- ItemItemID: {2}{3}",
				this.PersonPersonID,
				this.OrderID,
				this.ItemItemID,
				Environment.NewLine,
				this.GetType());
		}

		#endregion ToString Method

		#region Inner data class

		/// <summary>
		///		The data structure representation of the '"Order"' table.
		/// </summary>
		/// <remarks>
		/// 	This struct is generated by a tool and should never be modified.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Serializable]
		internal class OrderEntityData : ICloneable
		{
			#region Variable Declarations

			#region Primary key(s)
			/// <summary>			
			/// Order_ID : 
			/// </summary>
			/// <remarks>Member of the primary key of the underlying table ""Order""</remarks>
			public System.Int64 OrderID;

			/// <summary>
			/// keep a copy of the original so it can be used for editable primary keys.
			/// </summary>
			public System.Int64 OriginalOrderID;

			#endregion

			#region Non Primary key(s)


			/// <summary>
			/// Person_Person_ID : 
			/// </summary>
			public System.Int64 PersonPersonID = (long)0;

			/// <summary>
			/// Item_Item_ID : 
			/// </summary>
			public System.Int64 ItemItemID = (long)0;
			#endregion

			#endregion Variable Declarations

			#region Clone
			public Object Clone()
			{
				OrderEntityData _tmp = new OrderEntityData();

				_tmp.OrderID = this.OrderID;
				_tmp.OriginalOrderID = this.OriginalOrderID;

				_tmp.PersonPersonID = this.PersonPersonID;
				_tmp.ItemItemID = this.ItemItemID;

				return _tmp;
			}
			#endregion

			#region Data Properties

			#endregion Data Properties

		}//End struct


		#endregion

		#region Validation

		/// <summary>
		/// Assigns validation rules to this object based on model definition.
		/// </summary>
		/// <remarks>This method overrides the base class to add schema related validation.</remarks>
		protected override void AddValidationRules()
		{
			//Validation rules based on database schema.
		}
		#endregion

	} // End Class

	#region OrderComparer

	/// <summary>
	///	Strongly Typed IComparer
	/// </summary>
	public class OrderComparer : System.Collections.Generic.IComparer<Order>
	{
		OrderColumn whichComparison;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:OrderComparer"/> class.
		/// </summary>
		public OrderComparer()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:%=className%>Comparer"/> class.
		/// </summary>
		/// <param name="column">The column to sort on.</param>
		public OrderComparer(OrderColumn column)
		{
			this.whichComparison = column;
		}

		/// <summary>
		/// Determines whether the specified <c cref="Order"/> instances are considered equal.
		/// </summary>
		/// <param name="a">The first <c cref="Order"/> to compare.</param>
		/// <param name="b">The second <c>Order</c> to compare.</param>
		/// <returns>true if objA is the same instance as objB or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public bool Equals(Order a, Order b)
		{
			return this.Compare(a, b) == 0;
		}

		/// <summary>
		/// Gets the hash code of the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public int GetHashCode(Order entity)
		{
			return entity.GetHashCode();
		}

		/// <summary>
		/// Performs a case-insensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="a">The first object to compare.</param>
		/// <param name="b">The second object to compare.</param>
		/// <returns></returns>
		public int Compare(Order a, Order b)
		{
			EntityPropertyComparer entityPropertyComparer = new EntityPropertyComparer(this.whichComparison.ToString());
			return entityPropertyComparer.Compare(a, b);
		}

		/// <summary>
		/// Gets or sets the column that will be used for comparison.
		/// </summary>
		/// <value>The comparison column.</value>
		public OrderColumn WhichComparison
		{
			get
			{
				return this.whichComparison;
			}
			set
			{
				this.whichComparison = value;
			}
		}
	}

	#endregion

	#region OrderKey Class

	/// <summary>
	/// Wraps the unique identifier values for the <see cref="Order"/> object.
	/// </summary>
	[Serializable]
	public class OrderKey : EntityKeyBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the OrderKey class.
		/// </summary>
		public OrderKey()
		{
		}

		/// <summary>
		/// Initializes a new instance of the OrderKey class.
		/// </summary>
		public OrderKey(OrderBase entity)
		{
			Entity = entity;

			#region Init Properties

			if (entity != null)
			{
				this.orderID = entity.OrderID;
			}

			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the OrderKey class.
		/// </summary>
		public OrderKey(System.Int64 orderID)
		{
			#region Init Properties

			this.orderID = orderID;

			#endregion
		}

		#endregion Constructors

		#region Properties

		// member variable for the Entity property
		private OrderBase _entity;

		/// <summary>
		/// Gets or sets the Entity property.
		/// </summary>
		public OrderBase Entity
		{
			get
			{
				return _entity;
			}
			set
			{
				_entity = value;
			}
		}

		// member variable for the OrderID property
		private System.Int64 orderID;

		/// <summary>
		/// Gets or sets the OrderID property.
		/// </summary>
		public System.Int64 OrderID
		{
			get
			{
				return orderID;
			}
			set
			{
				if (Entity != null)
				{
					Entity.OrderID = value;
				}

				orderID = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads values from the supplied <see cref="IDictionary"/> object into
		/// properties of the current object.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> instance that contains
		/// the key/value pairs to be used as property values.</param>
		public override void Load(IDictionary values)
		{
			#region Init Properties

			if (values != null)
			{
				OrderID = (values["OrderID"] != null) ? (System.Int64)EntityUtil.ChangeType(values["OrderID"], typeof(System.Int64)) : (long)0;
			}

			#endregion
		}

		/// <summary>
		/// Creates a new <see cref="IDictionary"/> object and populates it
		/// with the property values of the current object.
		/// </summary>
		/// <returns>A collection of name/value pairs.</returns>
		public override IDictionary ToDictionary()
		{
			IDictionary values = new Hashtable();

			#region Init Dictionary

			values.Add("OrderID", OrderID);

			#endregion Init Dictionary

			return values;
		}

		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return String.Format("OrderID: {0}{1}",
								OrderID,
								Environment.NewLine);
		}

		#endregion Methods
	}

	#endregion

	#region OrderColumn Enum

	/// <summary>
	/// Enumerate the Order columns.
	/// </summary>
	[Serializable]
	public enum OrderColumn : int
	{
		/// <summary>
		/// PersonPersonID : 
		/// </summary>
		[EnumTextValue("Person_Person_ID")]
		[ColumnEnum("Person_Person_ID", typeof(System.Int64), System.Data.DbType.Int64, false, false, false)]
		PersonPersonID = 1,
		/// <summary>
		/// OrderID : 
		/// </summary>
		[EnumTextValue("Order_ID")]
		[ColumnEnum("Order_ID", typeof(System.Int64), System.Data.DbType.Int64, true, false, false)]
		OrderID = 2,
		/// <summary>
		/// ItemItemID : 
		/// </summary>
		[EnumTextValue("Item_Item_ID")]
		[ColumnEnum("Item_Item_ID", typeof(System.Int64), System.Data.DbType.Int64, false, false, false)]
		ItemItemID = 3
	}//End enum

	#endregion OrderColumn Enum
	#endregion
}
