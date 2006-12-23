using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.ObjectBuilder;

namespace NetTiersTestModel.Entities
{
	/// <summary>
	/// Entity Cache provides a caching mechanism for entities on a by entity level.
	/// </summary>
	public static class EntityCache
	{
		private static volatile CacheManagerFactory cacheManagerFactory;
		private static volatile CacheManager cacheManager;
		private static string cacheManagerKey = "NetTiersTestModel.Entities.EntityCache";
		private static object syncObject = new object();
		private static string configurationFile;

		/// <summary>
		/// Gets the cache manager.
		/// </summary>
		/// <value>The cache manager.</value>		
		public static CacheManager CacheManager
		{
			get
			{
				if (cacheManager == null)
					cacheManager = CacheManagerFactory.Create(cacheManagerKey);

				return cacheManager;
			}
		}

		/// <summary>
		/// Generates the configuration.
		/// </summary>
		/// <returns>DictionaryConfigurationSource to Configure the cache</returns>
		internal static DictionaryConfigurationSource GenerateConfiguration()
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
			sections.Add(CacheManagerSettings.SectionName, GenerateDefaultCacheManagerSettings());
			return sections;
		}

		#region GenerateDefaultCacheManagerSettings
		/// <summary>
		/// Generates the default cache manager settings.
		/// </summary>
		/// <returns></returns>
		private static CacheManagerSettings GenerateDefaultCacheManagerSettings()
		{
			CacheManagerSettings settings = new CacheManagerSettings();
			settings.BackingStores.Add(new CacheStorageData("inMemoryWithEncryptor", typeof(NullBackingStore), "dpapiEncryptor"));
			settings.EncryptionProviders.Add(new SymmetricStorageEncryptionProviderData("dpapiEncryptor", "dpapi1"));
			settings.CacheManagers.Add(
				new CacheManagerData(cacheManagerKey,
						10000, //Polling time
						1000, //Items to store
						100, //Items to remove 
						"inMemoryWithEncryptor"));
			return settings;
		}
		#endregion

		#region ConfigurationFile
		/// <summary>
		/// Gets or sets the configuration file.
		/// </summary>
		/// <value>The configuration file.</value>
		public static string ConfigurationFile
		{
			get
			{
				return configurationFile;
			}
			set
			{
				lock (syncObject)
					configurationFile = value;
			}
		}
		#endregion

		#region CacheManagerFactory
		/// <summary>
		/// Gets or sets the cache manager factory.
		/// </summary>
		/// <value>The cache manager factory.</value>
		public static CacheManagerFactory CacheManagerFactory
		{
			get
			{
				if (cacheManagerFactory == null)
				{
					lock (syncObject)
					{
						IConfigurationSource configurationSource = null;
						//From specified config
						if (ConfigurationFile != null && System.IO.File.Exists(ConfigurationFile))
						{
							configurationSource = new FileConfigurationSource(ConfigurationFile);
							cacheManagerFactory = new CacheManagerFactory(configurationSource);
						}
						else
						{
							try
							{
								//Try reading from default Configuration Source web/app config
								IConfigurationSource userConfiguration = new SystemConfigurationSource();
								cacheManagerFactory = new CacheManagerFactory(userConfiguration);

								//Test if CacheManagerKey is Configured
								cacheManagerFactory.Create(CacheManagerKey);
							}
							catch (Exception)
							{
								// Currently not configured, generate configuration
								configurationSource = GenerateConfiguration();
								cacheManagerFactory = new CacheManagerFactory(configurationSource);
							}
						}
					}
				}
				return cacheManagerFactory;
			}
			set
			{
				cacheManagerFactory = value;
			}
		}
		#endregion

		#region CacheManagerKey
		/// <summary>
		/// Assigns the Default CacheManagerKey To Be Used.
		/// </summary>
		public static string CacheManagerKey
		{
			get
			{
				return cacheManagerKey;
			}
			set
			{
				lock (syncObject)
				{
					cacheManagerKey = value;
				}
			}
		}
		#endregion

		#region RemoveItem
		/// <summary>
		/// Removes the item.
		/// </summary>
		/// <param name="id">The id.</param>
		public static void RemoveItem(string id)
		{
			CacheManager.Remove(id);
		}
		#endregion

		#region Flush Objects
		/// <summary>
		/// Flushes the cache manager.
		/// </summary>
		public static void FlushCacheManager()
		{
			cacheManager = null;
		}

		/// <summary>
		/// Flushes the cache.
		/// </summary>
		public static void FlushCache()
		{
			CacheManager.Flush();
		}
		#endregion

		#region AddCache
		/// <summary>
		/// Adds the cache.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="entity">The entity.</param>
		public static void AddCache(string id, object entity)
		{
			CacheManager.Add(id, entity);
		}
		#endregion

		#region GetItem
		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns> 
		public static T GetItem<T>(string id) where T : class
		{
			return CacheManager.GetData(id) as T;
		}
		#endregion

	}

	/// <summary>
	/// Entity Factory provides methods to create entity types from type names as strings.
	/// </summary>
	public partial class EntityFactory : EntityFactoryBase, IEntityFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityFactory"/> class.
		/// </summary>
		public EntityFactory()
		{
			base.CurrentEntityAssembly = typeof(EntityFactory).Assembly;
		}
	}

	/// <summary>
	/// Provides the core entity factory behavior.  Will create a type based on a string
	/// or a type and will try to auto discover which assembly this type lives in, even if it's
	/// not a local referenced assembly.
	/// </summary>
	public abstract class EntityFactoryBase : IEntityFactory
	{
		#region Events
		/// <summary>
		/// Exposes an Event which fires when an Entity is about to be created.
		/// </summary>
		[field: NonSerialized]
		public static event EntityCreatingEventHandler EntityCreating;
		/// <summary>
		/// Exposes an Event which fires when an Entity has been created.
		/// </summary>
		[field: NonSerialized]
		public static event EntityCreatedEventHandler EntityCreated;

		/// <summary>
		/// A delegate to handle <see cref="EntityCreating"/> events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void EntityCreatingEventHandler(object sender, EntityEventArgs e);

		/// <summary>
		/// A delegate to handle <see cref=" EntityCreated"/> events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void EntityCreatedEventHandler(object sender, EntityEventArgs e);
		#endregion

		#region Fields
		/// <summary>
		/// The current assembly from which to attempt to create entities from.
		/// </summary>
		protected static Assembly currentAssembly = typeof(EntityFactoryBase).Assembly;

		/// <summary>
		/// This will create entities by default if not found in the current assembly
		/// </summary>
		protected static string defaultCreationalNamespace = typeof(EntityFactoryBase).Namespace;

		/// <summary>
		/// This provides a cache of already discovered types, so that there is minimal performance hit after first lookup.
		/// </summary>
		protected static Dictionary<string, Type> internalTypeCache = new Dictionary<string, Type>();

		/// <summary>
		/// A synchronization object
		/// </summary>
		protected static object syncRoot = new object();

		/// <summary>
		/// This is the namespace for the currentAssembly property
		/// </summary>
		private static string currentEntityAssemblyNamespace = defaultCreationalNamespace;
		#endregion

		#region Properties

		#region CurrentAssembly
		/// <summary>
		/// Gets the current assembly.
		/// </summary>
		/// <value>The current assembly.</value>
		public virtual System.Reflection.Assembly CurrentEntityAssembly
		{
			get
			{
				return currentAssembly;
			}
			set
			{
				if (value != null)
				{
					lock (syncRoot)
					{
						currentAssembly = value;
						CurrentEntityAssemblyNamespace = currentAssembly.FullName.Split(',')[0];
					}
				}
			}
		}

		/// <summary>
		/// Gets the current entity assembly namespace to discover types from.
		/// </summary>
		/// <value>The current assembly.</value>
		public virtual string CurrentEntityAssemblyNamespace
		{
			get
			{
				return currentEntityAssemblyNamespace;
			}
			set
			{
				if (value != null)
				{
					lock (syncRoot)
					{
						currentEntityAssemblyNamespace = value;
					}
				}
			}
		}
		#endregion

		#endregion

		#region Member Create Methods
		/// <summary>
		/// Create an entity based on a string.
		/// It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString">string of entity to discover and create</param>
		/// <param name="defaultType">if string is not found defaultType will be created.</param>
		/// <returns>Created IEntity object</returns>
		public virtual IEntity CreateEntity(string typeString, Type defaultType)
		{
			return Create(typeString, defaultType);
		}

		/// <summary>
		/// Create a view entity based on a string.
		/// It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString">string of entity to discover and create</param>
		/// <param name="defaultType">if string is not found defaultType will be created.</param>
		/// <returns>Created object</returns>
		public virtual object CreateViewEntity(string typeString, Type defaultType)
		{
			return CreateReadOnlyEntity(typeString, defaultType);
		}
		#endregion

		#region Static Create Methods
		/// <summary>
		/// Create an entity based on a string.  It will autodiscover the type based on
		/// based on any information we can gather.
		/// </summary>
		/// <param name="typeString">string of entity to discover and create</param>
		/// <returns>created IEntity object</returns>
		public static IEntity Create(string typeString)
		{
			return Create(typeString, null);
		}

		/// <summary>
		/// Create an entity of generic type T
		/// </summary>
		/// <returns>T</returns>
		public static T Create<T>() where T : IEntity, new()
		{
			Type type = typeof(T);

			if (!internalTypeCache.ContainsValue(type))
				internalTypeCache.Add(type.FullName, type);

			//fire pre creating event
			OnEntityCreating(type);

			//create entity
			T entity = new T();

			//fire post created event
			OnEntityCreated(entity, type);

			return entity;
		}

		/// <summary>
		/// Create an entity based on a string.  It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString"></param>
		/// <param name="defaultType"></param>
		/// <returns>IEntity</returns>
		public static IEntity Create(string typeString, Type defaultType)
		{
			return CoreCreate(typeString, defaultType) as IEntity;
		}

		/// <summary>
		/// Creates an <see cref="IEntity"/> object and begins tracking on the object.
		/// </summary>
		/// <param name="type">known type to create</param>
		/// <returns>created IEntity object</returns>
		public static IEntity Create(Type type)
		{
			if (type.GetInterface("IEntity") == null)
				throw new ArgumentException("Type Parameter must implement the IEntity interface.");

			IEntity entity = CoreCreate(type) as IEntity;

			return entity;
		}

		#region CreateReadOnlyEntity
		/// <summary>
		/// Create an entity based on a string from a view.  It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString"></param>
		/// <param name="defaultType"></param>
		/// <returns>Object</returns>
		public static Object CreateReadOnlyEntity(string typeString, Type defaultType)
		{
			return CoreCreate(typeString, defaultType);
		}

		/// <summary>
		/// Creates an view object.
		/// </summary>
		/// <param name="type">known type to create</param>
		/// <returns>created view ReadOnlyEntity entity object</returns>
		public static Object CreateReadOnlyEntity(Type type)
		{
			return CoreCreate(type);
		}
		#endregion

		#region CoreCreate

		/// <summary>
		/// Create an entity based on a string.  It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString"></param>
		/// <param name="defaultType"></param>
		/// <returns>object</returns>
		private static Object CoreCreate(string typeString, Type defaultType)
		{
			if (string.IsNullOrEmpty(typeString))
				throw new ArgumentException("Entity can not be null or empty when being passed into the factory.");

			typeString = string.Format("{0}.{1}", currentEntityAssemblyNamespace, typeString);

			if (internalTypeCache.ContainsKey(typeString))
				return CoreCreate(internalTypeCache[typeString]);

			// resolve the type
			Type targetType = ResolveType(typeString);

			if (targetType == null)
			{
				if (defaultType != null)
					targetType = defaultType;
				else if (defaultCreationalNamespace != null)
					targetType = ResolveType(typeString, defaultCreationalNamespace);
				else
					throw new ArgumentException(string.Format("This type '{0}' can not be resolved.  Please ensure that your NetTiersService Section is correct in the configuration file.", typeString));
			}

			return CoreCreate(targetType);
		}

		/// <summary>
		/// Creates an object and begins .
		/// </summary>
		/// <param name="type">known type to create</param>
		/// <returns>created object</returns>
		private static object CoreCreate(Type type)
		{
			if (!internalTypeCache.ContainsValue(type))
			{
				lock (syncRoot)
				{
					internalTypeCache.Add(type.FullName, type);
				}
			}

			//fire pre creating event
			OnEntityCreating(type);

			//create entity based on passed in type.
			Object entity = Activator.CreateInstance(type) as Object;

			if (entity == null)
				throw new ArgumentException(string.Format("This type '{0}' can not be resolved correctly to instatiate your entity.  Please ensure that your NetTiersService Section is correct in the configuration file.", type.FullName));

			//fire post created event
			OnEntityCreated(entity as IEntity, type);

			return entity;
		}
		#endregion

		#endregion

		#region Helper Methods
		/// <summary>
		/// Resolves a type based on a string.  It will attempt to Auto-Discover the type, 
		/// based on it's fullname or partial names.
		/// </summary>
		/// <param name="typeString">the string of the type to resolve</param>
		/// <returns>IEntity</returns>
		private static Type ResolveType(string typeString)
		{
			//string className = GetClassNameFromString(typeString);
			//string assemblyName = GetAssemblyNameFromString(typeString);

			// Get the assembly containing the handler
			System.Reflection.Assembly assembly = currentAssembly;
			//Type foundType = null;
			//TODO: Add more discovery
			return assembly.GetType(typeString, false, true);
		}

		/// <summary>
		/// Resolves a type based on a string.  It will attempt to Auto-Discover the type, 
		/// based on it's fullname or partial names.
		/// </summary>
		/// <param name="typeString">the string of the type to resolve</param>
		/// <param name="defaultNamespace">the string of the type to resolve</param>
		/// <returns>IEntity</returns>
		private static Type ResolveType(string typeString, string defaultNamespace)
		{
			//string className = GetClassNameFromString(typeString);
			//string assemblyName = GetAssemblyNameFromString(typeString);

			// Get the assembly containing the handler
			System.Reflection.Assembly assembly = currentAssembly;
			//Type foundType = null;
			//TODO: Add more discovery
			return assembly.GetType(String.Format("{0}.{1}", defaultNamespace, typeString), false, true);
		}


		/// <summary>
		/// Parses a string and gets the class name from a qualified name, based on [Type, Assembly]
		/// </summary>
		/// <param name="typeString"></param>
		/// <example>
		///     MyNamespace.MyType, MyNamespace
		/// </example>
		/// <returns>string of the qualified classname</returns>
		private static string GetClassNameFromString(string typeString)
		{
			int commaIndex = typeString.IndexOf(",");
			if (commaIndex > 0)
				return typeString.Substring(0, commaIndex).Trim();

			return typeString;
		}

		/// <summary>
		/// Parses a string and gets the class name from a qualified name, based on [Type, Assembly]
		/// </summary>
		/// <param name="typeString"></param>
		/// <example>
		///     MyNamespace.MyType, MyNamespace
		/// </example>
		/// <returns>string of the qualified Assembly Name</returns>
		private static string GetAssemblyNameFromString(string typeString)
		{
			int commaIndex = typeString.IndexOf(",");
			if (commaIndex > 0 && typeString.Length >= commaIndex + 1)
				return typeString.Substring(commaIndex + 1).Trim();

			return typeString;
		}

		/// <summary>
		/// Flushes the Internal Type Cache
		/// </summary>
		public static void FlushTypeCache()
		{
			internalTypeCache.Clear();
		}
		#endregion

		#region Fire Events

		/// <summary>
		/// Used to fire the event just before an entity is creating.
		/// </summary>
		/// <param name="type">The type.</param>
		private static void OnEntityCreating(Type type)
		{
			EntityCreatingEventHandler handler = EntityCreating;
			if (handler != null)
			{
				handler(null, new EntityEventArgs(null, type));
			}
		}

		/// <summary>
		/// Used to fire the an event when the entity has just been created.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="type">The type.</param>
		private static void OnEntityCreated(IEntity entity, Type type)
		{
			EntityCreatedEventHandler handler = EntityCreated;
			if (handler != null)
			{
				handler(null, new EntityEventArgs(entity, type));
			}
		}

		#endregion

		#region EntityEventArgs class
		/// <summary>
		/// Used to suppliment information for the EntityCreating and EntityCreated events.
		/// </summary>
		/// <remarks>
		/// The EntityCreating and EntityCreated events occur when an Entity is about to be created,
		/// or just after an Entity is created.
		/// </remarks>
		public class EntityEventArgs : System.EventArgs
		{
			private IEntity entity;
			private Type creationalType;

			/// <summary>
			/// Initalizes a new Instance of the ComponentEntityEventArgs class.
			/// </summary>
			/// <param name="entity">The entity.</param>
			/// <param name="type">The type.</param>
			public EntityEventArgs(IEntity entity, Type type)
			{
				this.entity = entity;
				this.creationalType = type;
			}

			/// <summary>
			/// The entity that is about to be created or has just been created.
			/// </summary>
			/// <value>The entity.</value>
			public IEntity Entity
			{
				get
				{
					return this.entity;
				}
			}

			/// <summary>
			/// Gets the type of the entity to be created.
			/// </summary>
			/// <value>The type of the creational.</value>
			public Type CreationalType
			{
				get
				{
					return this.creationalType;
				}
			}
		}
		#endregion
	}

	/// <summary>
	/// The base object for each database table entity.
	/// </summary>
	[Serializable]
	public abstract partial class EntityBaseCore : IEntity, INotifyPropertyChanged, IDataErrorInfo, IDeserializationCallback
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:EntityBaseCore"/> class.
		/// </summary>
		protected EntityBaseCore()
		{
		}

		#endregion

		/// <summary>
		/// The EntityState of the entity
		/// </summary>
		private EntityState currentEntityState = EntityState.Added;
		/// <summary>
		/// Determines whether the entity is being tracked by the Locator.
		/// </summary>
		private bool isEntityTracked = false;
		/// <summary>
		/// Suppresses Entity Events from Firing, 
		/// useful when loading the entities from the database.
		/// </summary>
		private bool suppressEntityEvents = false;


		/// <summary>
		///  Used by in place editing of databinding features for new inserted row.
		/// Indicates that we are in the middle of an IBinding insert transaction.
		/// </summary>
		protected bool bindingIsNew = true;

		/// <summary>
		///	The name of the underlying database table.
		/// </summary>
		public abstract string TableName
		{
			get;
		}

		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		/// <value>A string array that holds the columns names.</value>
		public abstract string[] TableColumns
		{
			get;
		}

		//private bool _isDeleted = false;
		/// <summary>
		/// 	True if object has been <see cref="MarkToDelete"/>. ReadOnly.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool IsDeleted
		{
			get
			{
				return this.currentEntityState == EntityState.Deleted;
			}
		}

		//private bool _isDirty = false;
		/// <summary>
		///		Indicates if the object has been modified from its original state.
		/// </summary>
		/// <remarks>True if object has been modified from its original state; otherwise False;</remarks>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool IsDirty
		{
			get
			{
				return this.currentEntityState != EntityState.Unchanged;
			}
		}


		//private bool _isNew = true;
		/// <summary>
		///		Indicates if the object is new.
		/// </summary>
		/// <remarks>True if objectis new; otherwise False;</remarks>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool IsNew
		{
			get
			{
				return this.currentEntityState == EntityState.Added;
			}
			set
			{
				this.currentEntityState = EntityState.Added;
			}
		}



		//private EntityState state = EntityState.Unchanged ;
		/// <summary>
		///		Indicates state of object
		/// </summary>
		/// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public virtual EntityState EntityState
		{
			get
			{
				return this.currentEntityState;
			}
			set
			{
				this.currentEntityState = value;
			}
		}

		/// <summary>
		/// Accepts the changes made to this object.
		/// </summary>
		/// <remarks>
		/// After calling this method <see cref="IsDirty"/> and <see cref="IsNew"/> are false. <see cref="IsDeleted"/> flag remain unchanged as it is handled by the parent List.
		/// </remarks>
		public virtual void AcceptChanges()
		{
			this.bindingIsNew = false;
			this.currentEntityState = EntityState.Unchanged;
			OnPropertyChanged(string.Empty);
		}


		///<summary>
		///  TODO: Revert all changes and restore original values.
		///  Currently not supported.
		///</summary>
		/// <exception cref="NotSupportedException">This method is not currently supported and always throws this exception.</exception>
		public abstract void CancelChanges();


		///<summary>
		///   Marks entity to be deleted.
		///</summary>
		public virtual void MarkToDelete()
		{

			if (this.currentEntityState != EntityState.Added)
				this.currentEntityState = EntityState.Deleted;
		}

		///<summary>
		///   Remove the "isDeleted" mark from the entity.
		///</summary>
		public virtual void RemoveDeleteMark()
		{
			if (this.currentEntityState != EntityState.Added)
			{
				this.currentEntityState = EntityState.Changed;
			}
		}

		/// <summary>
		/// Gets or sets the parent collection.
		/// </summary>
		/// <value>The parent collection.</value>
		[XmlIgnore]
		public abstract object ParentCollection
		{
			get;
			set;
		}

		#region Common Columns
		#endregion

		/// <summary>
		/// Object that contains data to associate with this object
		/// </summary>
		[NonSerialized]
		private object tag;

		/// <summary>
		///     Gets or sets the object that contains supplemental data about this object.
		/// </summary>
		/// <value>Object</value>
		[System.ComponentModel.Bindable(false)]
		[LocalizableAttribute(false)]
		[DescriptionAttribute("Object containing data to be associated with this object")]
		public virtual object Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				if (this.tag == value)
					return;

				this.tag = value;
			}
		}

		/// <summary>
		/// Determines whether this entity is being tracked.
		/// </summary>
		[System.ComponentModel.Bindable(false)]
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool IsEntityTracked
		{
			get
			{
				return isEntityTracked;
			}
			set
			{
				isEntityTracked = value;
			}
		}


		/// <summary>
		/// Determines whether this entity is to suppress events while set to true.
		/// </summary>
		[System.ComponentModel.Bindable(false)]
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool SuppressEntityEvents
		{
			get
			{
				return suppressEntityEvents;
			}
			set
			{
				suppressEntityEvents = value;
			}
		}

		///<summary>
		/// Provides the tracking key for the <see cref="EntityLocator"/>
		///</summary>
		[XmlIgnoreAttribute(), BrowsableAttribute(false)]
		public abstract string EntityTrackingKey
		{
			get;
			set;
		}

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Event to indicate that a property has changed.
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Called when a property is changed
		/// </summary>
		/// <param name="propertyName">The name of the property that has changed.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (!suppressEntityEvents)
			{
				OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Called when a property is changed
		/// </summary>
		/// <param name="e">PropertyChangedEventArgs</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (!suppressEntityEvents)
			{
				//Validate the property
				ValidationRules.ValidateRules(e.PropertyName);

				if (null != PropertyChanged)
				{
					PropertyChanged(this, e);
				}
			}
		}

		#endregion

		#region IDataErrorInfo Members

		/// <summary>
		/// Gets an error message indicating what is wrong with this object.
		/// </summary>
		/// <value></value>
		/// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>      
		public string Error
		{
			get
			{
				string errorDescription = string.Empty;
				if (!IsValid)
				{
					errorDescription = ValidationRules.GetBrokenRules().ToString();
				}
				return errorDescription;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:String"/> with the specified column name.
		/// </summary>
		/// <value></value>
		public string this[string columnName]
		{
			get
			{
				string errorDescription = string.Empty;
				if (!IsValid)
				{
					errorDescription = ValidationRules.GetBrokenRules().GetPropertyErrorDescriptions(columnName);
				}
				return errorDescription;
			}
		}

		#endregion

		#region Validation

		[NonSerialized]
		private Validation.ValidationRules _validationRules;


		/// <summary>
		/// Returns the list of <see cref="Validation.ValidationRules"/> associated with this object.
		/// </summary>
		[XmlIgnoreAttribute()]
		protected Validation.ValidationRules ValidationRules
		{
			get
			{
				if (_validationRules == null)
				{
					_validationRules = new Validation.ValidationRules(this);

					//lazy init the rules as well.
					AddValidationRules();
				}

				return _validationRules;
			}
		}

		/// <summary>
		/// Assigns validation rules to this object.
		/// </summary>
		/// <remarks>
		/// This method can be overridden in a derived class to add custom validation rules. 
		///</remarks>
		protected virtual void AddValidationRules()
		{

		}

		/// <summary>
		/// Returns <see langword="true" /> if the object is valid, 
		/// <see langword="false" /> if the object validation rules that have indicated failure. 
		/// </summary>
		[Browsable(false)]
		public virtual bool IsValid
		{
			get
			{
				return ValidationRules.IsValid;
			}
		}

		/// <summary>
		/// Returns a list of all the validation rules that failed.
		/// </summary>
		/// <returns><see cref="Validation.BrokenRulesList" /></returns>
		[XmlIgnoreAttribute()]
		public virtual Validation.BrokenRulesList BrokenRulesList
		{
			get
			{
				return ValidationRules.GetBrokenRules();
			}
		}

		/// <summary>
		/// Force this object to validate itself using the assigned business rules.
		/// </summary>
		/// <remarks>Validates all properties.</remarks>
		public void Validate()
		{
			ValidationRules.ValidateRules();
		}

		/// <summary>
		/// Force the object to validate itself using the assigned business rules.
		/// </summary>
		/// <param name="propertyName">Name of the property to validate.</param>
		public void Validate(string propertyName)
		{
			ValidationRules.ValidateRules(propertyName);
		}

		/// <summary>
		/// Force the object to validate itself using the assigned business rules.
		/// </summary>
		/// <param name="column">Column enumeration representingt the column to validate.</param>
		public void Validate(System.Enum column)
		{
			Validate(column.ToString());
		}
		#endregion

		#region IDeserializationCallback Members

		/// <summary>
		/// Runs when the entire object graph has been deserialized.
		/// </summary>
		/// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>

		public void OnDeserialization(object sender)
		{
			if (!suppressEntityEvents)
			{
				ValidationRules.Target = this;
				AddValidationRules();
			}
		}

		#endregion

	}

	/// <summary>
	/// This classes contains utilities functions for the <see cref="IEntity"/> instances and collections.
	/// </summary>
	/// <remarks>All methods static</remarks>
	public static class EntityHelper
	{

		#region SerializeBinary

		/// <summary>
		/// Serializes the entity to binary.
		/// </summary>
		/// <param name="entity">The Entity to serialize.</param>
		/// <value>A byte array that contains the serialized entity.</value>
		public static byte[] SerializeBinary(IEntity entity)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, entity);
			return ms.ToArray();
		}

		/// <summary>
		/// Serializes the entity collection to binary.
		/// </summary>
		/// <param name="entityCollection">The Entity collection to serialize.</param>
		/// <value>A byte array that contains the serialized entity.</value>
		public static byte[] SerializeBinary(IList entityCollection)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, entityCollection);
			return ms.ToArray();
		}

		/// <summary>
		/// Serializes the entity to binary and puts the data into a file.
		/// </summary>
		/// <param name="entity">The Entity to serialize.</param>
		/// <param name="path">The Path to the destination file.</param>
		public static void SerializeBinary(IEntity entity, string path)
		{
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, entity);
			fs.Close();
		}

		/// <summary>
		/// Serializes the entity collection to binary and puts the data into a file.
		/// </summary>
		/// <param name="entityCollection">The Entity collection to serialize.</param>
		/// <param name="path">The Path to the destination file.</param>
		public static void SerializeBinary(IList entityCollection, string path)
		{
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, entityCollection);
			fs.Close();
		}

		#endregion

		#region SerializeXml

		/// <summary>
		/// serialize an object to xml.
		/// </summary>
		/// <param name="item">The item to serialize.</param>
		/// <returns></returns>
		public static string XmlSerialize<T>(T item)
		{
			XmlSerializer s = new XmlSerializer(typeof(T));
			StringBuilder stringBuilder = new StringBuilder();

			StringWriter writer = new StringWriter(stringBuilder);
			s.Serialize(writer, item);
			writer.Close();

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Serializes the entity to Xml.
		/// </summary>
		/// <param name="entity">The Entity to serialize.</param>
		/// <returns>A string that contains the serialized entity.</returns>
		public static string SerializeXml(IEntity entity)
		{
			XmlSerializer ser = new XmlSerializer(entity.GetType());
			StringBuilder sb = new StringBuilder();
			TextWriter writer = new StringWriter(sb);
			ser.Serialize(writer, entity);
			writer.Close();
			return sb.ToString();
		}

		/// <summary>
		/// Serializes the <see cref="T:TList{T}"/> of IEntity to XML
		/// </summary>
		/// <typeparam name="T">type to return, type must implement IEntity</typeparam>
		/// <param name="entityCollection">TList of T type to return</param>
		/// <returns>string of serialized XML</returns>
		public static string SerializeXml<T>(TList<T> entityCollection) where T : IEntity, new()
		{
			XmlSerializer ser = new XmlSerializer(entityCollection.GetType());
			StringBuilder sb = new StringBuilder();
			TextWriter writer = new StringWriter(sb);
			ser.Serialize(writer, entityCollection);
			writer.Close();
			return sb.ToString();
		}

		/// <summary>
		/// Serializes the entity to xml and puts the data into a file.
		/// </summary>
		/// <param name="entity">The Entity to serialize.</param>
		/// <param name="path">The Path to the destination file.</param>
		public static void SerializeXml(IEntity entity, string path)
		{
			XmlSerializer ser = new XmlSerializer(entity.GetType());
			StreamWriter sw = new StreamWriter(path);
			ser.Serialize(sw, entity);
			sw.Close();
		}

		/// <summary>
		/// Serializes the entity collection to xml and puts the data into a file.
		/// </summary>
		/// <param name="entityCollection">The Entity collection to serialize.</param>
		/// <param name="path">The Path to the destination file.</param>
		public static void SerializeXml<T>(TList<T> entityCollection, string path) where T : IEntity, new()
		{
			XmlSerializer ser = new XmlSerializer(entityCollection.GetType());
			StreamWriter sw = new StreamWriter(path);
			ser.Serialize(sw, entityCollection);
			sw.Close();
		}

		/// <summary>
		/// Serializes the <see cref="T:VList{T}"/> of view entities to XML
		/// </summary>
		/// <typeparam name="T">type to return</typeparam>
		/// <param name="entityCollection">VList of T type to return</param>
		/// <returns>string of serialized XML</returns>
		public static string SerializeXml<T>(VList<T> entityCollection)
		{
			XmlSerializer ser = new XmlSerializer(entityCollection.GetType());
			StringBuilder sb = new StringBuilder();
			TextWriter writer = new StringWriter(sb);
			ser.Serialize(writer, entityCollection);
			writer.Close();
			return sb.ToString();
		}

		/// <summary>
		/// Serializes the view collection to xml and puts the data into a file.
		/// </summary>
		/// <param name="entityCollection">The Entity View collection to serialize.</param>
		/// <param name="path">The Path to the destination file.</param>
		public static void SerializeXml<T>(VList<T> entityCollection, string path)
		{
			XmlSerializer ser = new XmlSerializer(entityCollection.GetType());
			StreamWriter sw = new StreamWriter(path);
			ser.Serialize(sw, entityCollection);
			sw.Close();
		}

		#endregion

		#region Deserialize Binary

		/// <summary>
		/// Deserializes the binary data to an object instance.
		/// </summary>
		/// <param name="bytes">The byte array that contains binary serialized datas.</param>
		/// <returns>The deserialized instance</returns>
		public static object DeserializeBinary(byte[] bytes)
		{
			MemoryStream ms = new MemoryStream(bytes);
			BinaryFormatter bf = new BinaryFormatter();
			return bf.Deserialize(ms);
		}
		#endregion

		#region DeserializeXml

		/// <summary>
		/// deserialize an xml string into an object.
		/// </summary>
		/// <param name="xmlData">The XML data.</param>
		/// <returns></returns>
		public static T XmlDeserialize<T>(string xmlData)
		{
			XmlSerializer s = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(xmlData);
			T entity = (T)s.Deserialize(reader);
			reader.Close();
			return entity;
		}

		/// <summary>
		/// Deserialize an Entity from an xml string to T
		/// </summary>
		/// <typeparam name="T">T where T : IEntity</typeparam>
		/// <param name="data">string of serialized xml</param>
		/// <returns>T where T : IEntity</returns>
		public static T DeserializeEntityXml<T>(string data) where T : IEntity
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(data);
			T toReturn;
			toReturn = (T)serializer.Deserialize(reader);
			reader.Close();
			return toReturn;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"><see cref="T:TList{T}"/> where T : IEntity, new()</typeparam>
		/// <param name="data">string of serialized TList of T xml</param>
		/// <returns><see cref="T:TList{T}"/> where T : IEntity, new()</returns>
		public static TList<T> DeserializeListXml<T>(string data) where T : IEntity, new()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(TList<T>));
			TextReader reader = new StringReader(data);
			TList<T> toReturn;
			toReturn = (TList<T>)serializer.Deserialize(reader);
			reader.Close();
			return toReturn;
		}

		/// <summary>
		/// Deserializes the XML string to an instance of the specified type.
		/// </summary>
		/// <param name="root">The name of the root node.</param>
		/// <param name="type">The targeted Type.</param>
		/// <param name="reader">The xmlReader instance that point to the xml data.</param>
		/// <returns>An instance of the Type class.</returns>		
		public static object DeserializeXml(string root, Type type, XmlReader reader)
		{
			XmlRootAttribute xmlRoot = new XmlRootAttribute();
			xmlRoot.ElementName = root;
			XmlSerializer serializer = new XmlSerializer(type, xmlRoot);
			object obj = serializer.Deserialize(reader);
			return obj;
		}

		/// <summary>
		/// Deserialize a list of view entity objects from an Xml string
		/// </summary>
		/// <typeparam name="T"><see cref="T:VList{T}"/> where T is a view entity class</typeparam>
		/// <param name="data">string of serialized VList of T xml</param>
		/// <returns><see cref="T:VList{T}"/></returns>
		public static VList<T> DeserializeVListXml<T>(string data)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(VList<T>));
			TextReader reader = new StringReader(data);
			VList<T> toReturn;
			toReturn = (VList<T>)serializer.Deserialize(reader);
			reader.Close();
			return toReturn;
		}

		#endregion

		#region GetByteLength

		/// <summary>
		/// Gets the byte length of the specified object.
		/// </summary>
		/// <param name="obj">The object we want the length.</param>
		/// <returns>The byte length of the object.</returns>
		public static long GetByteLength(object obj)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, obj);
			return ms.Length;
		}

		#endregion

		#region Dynamic ToString Implementation through Reflection
		/// <summary>
		/// Give a string representation of a object, with use of reflection.
		/// </summary>
		/// <param name="o">O.</param>
		/// <returns></returns>
		public static string ToString(Object o)
		{
			StringBuilder sb = new StringBuilder();
			Type t = o.GetType();

			PropertyInfo[] pi = t.GetProperties();

			sb.Append("Properties for: " + o.GetType().Name + Environment.NewLine);
			foreach (PropertyInfo i in pi)
			{
				try
				{
					sb.Append("\t" + i.Name + "(" + i.PropertyType.ToString() + "): ");
					if (null != i.GetValue(o, null))
					{
						sb.Append(i.GetValue(o, null).ToString());
					}

				}
				catch
				{
				}
				sb.Append(Environment.NewLine);

			}

			FieldInfo[] fi = t.GetFields();

			foreach (FieldInfo i in fi)
			{
				try
				{
					sb.Append("\t" + i.Name + "(" + i.FieldType.ToString() + "): ");
					if (null != i.GetValue(o))
					{
						sb.Append(i.GetValue(o).ToString());
					}

				}
				catch
				{
				}
				sb.Append(Environment.NewLine);

			}

			return sb.ToString();
		}
		#endregion

		#region Clone
		/// <summary>
		/// Generic method to perform a deep copy of an object
		/// </summary>
		/// <typeparam name="T">Type of object being cloned and returned</typeparam>
		/// <param name="sourceEntity">Source object to be cloned.</param>
		/// <returns>An object that is a deep copy of the sourceEntity object.</returns>
		public static T Clone<T>(T sourceEntity)
		{
			BinaryFormatter bFormatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			bFormatter.Serialize(stream, sourceEntity);
			stream.Seek(0, System.IO.SeekOrigin.Begin);
			T clone = (T)bFormatter.Deserialize(stream);
			return clone;
		}
		#endregion

		#region GetBindableProperties
		/// <summary>
		/// Get the collection of properties that have been marked as Bindable
		/// </summary>
		/// <param name="type">The type of the object to get the properties for.</param>
		/// <returns><see cref="PropertyDescriptorCollection"/> of bindable properties.</returns>
		public static PropertyDescriptorCollection GetBindableProperties(Type type)
		{
			// create a filter so we only return the properties that have been designated as bindable
			Attribute[] attrs = new Attribute[] { new BindableAttribute() };

			// save the bindable properties in a local field
			return TypeDescriptor.GetProperties(type, attrs);
		}
		#endregion

		#region GetEnumTextValue
		///<summary>
		/// Allows the discovery of an enumeration text value based on the <c>EnumTextValueAttribute</c>
		///</summary>
		/// <param name="e">The enum to get the reader friendly text value for.</param>
		/// <returns><see cref="System.String"/> </returns>
		public static string GetEnumTextValue(Enum e)
		{
			string ret = "";
			Type t = e.GetType();
			MemberInfo[] members = t.GetMember(e.ToString());
			if (members != null && members.Length == 1)
			{
				object[] attrs = members[0].GetCustomAttributes(typeof(EnumTextValueAttribute), false);
				if (attrs.Length == 1)
				{
					ret = ((EnumTextValueAttribute)attrs[0]).Text;
				}
			}
			return ret;
		}
		#endregion

		#region GetEnumValue
		///<summary>
		/// Allows the discovery of an enumeration value based on the <c>EnumTextValueAttribute</c>
		///</summary>
		/// <param name="text">The text of the <c>EnumTextValueAttribute</c>.</param>
		/// <param name="enumType">The type of the enum to get the value for.</param>
		/// <returns><see cref="System.Object"/> boxed representation of the enum value </returns>
		public static object GetEnumValue(string text, Type enumType)
		{
			MemberInfo[] members = enumType.GetMembers();
			foreach (MemberInfo mi in members)
			{
				object[] attrs = mi.GetCustomAttributes(typeof(EnumTextValueAttribute), false);
				if (attrs.Length == 1)
				{
					if (((EnumTextValueAttribute)attrs[0]).Text == text)
						return Enum.Parse(enumType, mi.Name);
				}
			}
			throw new ArgumentOutOfRangeException("text", text, "The text passed does not correspond to an attributed enum value");
		}
		#endregion

		#region GetAttribute

		/// <summary>
		/// Gets the first occurrence of the specified type of <see cref="System.Attribute"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="e"></param>
		/// <returns></returns>
		public static T GetAttribute<T>(System.Enum e) where T : System.Attribute
		{
			T attribute = default(T);
			Type enumType = e.GetType();
			System.Reflection.MemberInfo[] members = enumType.GetMember(e.ToString());

			if (members != null && members.Length == 1)
			{
				object[] attrs = members[0].GetCustomAttributes(typeof(T), false);
				if (attrs.Length > 0)
				{
					attribute = (T)attrs[0];
				}
			}

			return attribute;
		}

		#endregion GetAttribute

	}

	#region BindableAttribute
	/// <summary>
	/// Attach this to every property that should be 
	/// displayed in the designer.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BindableAttribute : System.Attribute
	{
		/// <summary>
		/// Intitialize a new instance of the BindableAttribute class.
		/// </summary>
		public BindableAttribute()
		{
		}
	}
	#endregion

	#region EnumTextValue
	///<summary>
	/// Attribute used to decorate enumerations with reader friendly names
	///</summary>
	public sealed class EnumTextValueAttribute : System.Attribute
	{
		private readonly string enumTextValue;

		///<summary>
		/// Returns the text representation of the value
		///</summary>
		public string Text
		{
			get
			{
				return enumTextValue;
			}
		}

		///<summary>
		/// Allows the creation of a friendly text representation of the enumeration.
		///</summary>
		/// <param name="text">The reader friendly text to decorate the enum.</param>
		public EnumTextValueAttribute(string text)
		{
			enumTextValue = text;
		}
	}
	#endregion

	#region ColumnEnumAttribute

	/// <summary>
	/// Provides column metadata information for an entity column enumeration.
	/// </summary>
	public sealed class ColumnEnumAttribute : System.Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ColumnEnumAttribute class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="systemType"></param>
		/// <param name="dbType"></param>
		/// <param name="isPrimaryKey"></param>
		/// <param name="isIdentity"></param>
		/// <param name="allowDbNull"></param>
		/// <param name="length"></param>
		public ColumnEnumAttribute(String name, Type systemType, System.Data.DbType dbType, bool isPrimaryKey, bool isIdentity, bool allowDbNull, int length)
		{
			this.Name = name;
			this.SystemType = systemType;
			this.DbType = dbType;
			this.IsPrimaryKey = isPrimaryKey;
			this.IsIdentity = isIdentity;
			this.AllowDbNull = allowDbNull;
			this.Length = length;
		}

		/// <summary>
		/// Initializes a new instance of the ColumnEnumAttribute class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="systemType"></param>
		/// <param name="dbType"></param>
		/// <param name="isPrimaryKey"></param>
		/// <param name="isIdentity"></param>
		/// <param name="allowDbNull"></param>
		public ColumnEnumAttribute(String name, Type systemType, System.Data.DbType dbType, bool isPrimaryKey, bool isIdentity, bool allowDbNull)
			: this(name, systemType, dbType, isPrimaryKey, isIdentity, allowDbNull, -1)
		{
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// The Name member variable.
		/// </summary>
		private String name;

		/// <summary>
		/// Gets or sets the Name property.
		/// </summary>
		public String Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		/// <summary>
		/// The SystemType member variable.
		/// </summary>
		private Type systemType;

		/// <summary>
		/// Gets or sets the SystemType property.
		/// </summary>
		public Type SystemType
		{
			get
			{
				return systemType;
			}
			set
			{
				systemType = value;
			}
		}

		/// <summary>
		/// The DbType member variable.
		/// </summary>
		private System.Data.DbType dbType;

		/// <summary>
		/// Gets or sets the DbType property.
		/// </summary>
		public System.Data.DbType DbType
		{
			get
			{
				return dbType;
			}
			set
			{
				dbType = value;
			}
		}

		/// <summary>
		/// The IsPrimaryKey member variable.
		/// </summary>
		private bool isPrimaryKey;

		/// <summary>
		/// Gets or sets the IsPrimaryKey property.
		/// </summary>
		public bool IsPrimaryKey
		{
			get
			{
				return isPrimaryKey;
			}
			set
			{
				isPrimaryKey = value;
			}
		}

		/// <summary>
		/// The IsIdentity member variable.
		/// </summary>
		private bool isIdentity;

		/// <summary>
		/// Gets or sets the IsIdentity property.
		/// </summary>
		public bool IsIdentity
		{
			get
			{
				return isIdentity;
			}
			set
			{
				isIdentity = value;
			}
		}

		/// <summary>
		/// The AllowDbNull member variable.
		/// </summary>
		private bool allowDbNull;

		/// <summary>
		/// Gets or sets the AllowDbNull property.
		/// </summary>
		public bool AllowDbNull
		{
			get
			{
				return allowDbNull;
			}
			set
			{
				allowDbNull = value;
			}
		}

		/// <summary>
		/// The Length member variable.
		/// </summary>
		private int length;

		/// <summary>
		/// Gets or sets the Length property.
		/// </summary>
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				length = value;
			}
		}

		#endregion Properties
	}

	#endregion ColumnEnumAttribute

	#region GenericStateChangedEventArgs
	/// <summary>
	/// Provides a generic way to inform interested objects about state change
	/// Supplies the old value and the new value of the changed state.
	/// </summary>
	/// <typeparam name="T">State Object</typeparam>
	public class GenericStateChangedEventArgs<T> : EventArgs
	{
		private T oldValue;
		private T newValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericStateChangedEventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public GenericStateChangedEventArgs(T oldValue, T newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		/// <summary>
		/// Gets the old value.
		/// </summary>
		/// <value>The old value.</value>
		public T OldValue
		{
			get
			{
				return oldValue;
			}
		}

		/// <summary>
		/// Gets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue
		{
			get
			{
				return newValue;
			}
		}
	}
	#endregion

	/// <summary>
	/// The base object for each database table's unique identifier.
	/// </summary>
	[Serializable]
	public abstract partial class EntityKeyBaseCore : IEntityKey
	{
		/// <summary>
		/// Reads values from the supplied IDictionary object into
		/// properties of the current object.
		/// </summary>
		/// <param name="values">An IDictionary instance that contains the key/value
		/// pairs to be used as property values.</param>
		public abstract void Load(IDictionary values);

		/// <summary>
		/// Creates a new <see cref="IDictionary"/> object and populates it
		/// with the property values of the current object.
		/// </summary>
		/// <returns>A collection of name/value pairs.</returns>
		public abstract IDictionary ToDictionary();

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current object.
		/// </summary>
		/// <param name="obj">The System.Object to compare with the current object.</param>
		/// <returns>Returns true if the specified System.Object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;
			return (ToString() == obj.ToString());
		}

		/// <summary>
		/// Serves as a hash function for a particular type. GetHashCode() is suitable
		/// for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}

	/// <summary>
	/// Provides a means to weak reference and already created and untouched locate entities.
	/// </summary>	
	public class EntityLocator : Locator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityLocator"/> class.
		/// </summary>
		public EntityLocator()
			: base(null)
		{
		}

		/// <summary>
		/// Adds the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Add(string key, object value)
		{
			//TODO: Add an Entity specific logic 

			base.Add(key as object, value);
		}


		/// <summary>
		/// Determines whether [contains] [the specified key].
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="options">The options.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(string key, SearchMode options)
		{
			//TODO: Add an Entity specific logic 
			return base.Contains(key, options);
		}


		/// <summary>
		/// Gets the specified key of any object.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="options">The options.</param>
		/// <returns>object if available, else null </returns>
		public override object Get(object key, SearchMode options)
		{
			return base.Get(key, options);
		}

		/// <summary>
		/// Get's an Entity from the Tracking Locator
		/// </summary>
		/// <typeparam name="Entity">A type that implements IEntity</typeparam>
		/// <param name="key">locator list key to fetch, best used 
		/// if it's the (TypeName or TableName) + EntityKey of the this entity</param>
		/// <returns>Entity from Locator if available.</returns>
		public Entity GetEntity<Entity>(string key) where Entity : EntityBase, new()
		{
			//TODO: Add an Entity specific logic 
			return Get(key as object, SearchMode.Local) as Entity;
		}

		/// <summary>
		/// Get's a List of Entities from the Tracking Locator
		/// </summary>
		/// <typeparam name="EntityList"> a type that implements ListBase&lt;IEntity&gt;</typeparam>
		/// <param name="key">locator list key to fetch, best used 
		/// if it's like the criteria of the method used to populate this list
		/// </param>
		/// <returns>ListBase&lt;IEntity&gt; if available</returns>
		public EntityList GetList<EntityList>(string key) where EntityList : ListBase<IEntity>, new()
		{
			//TODO: Add an List specific logic 
			return Get(key as object, SearchMode.Local) as EntityList;
		}

		/// <summary>
		/// Re-Creates the key based on primary key values.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="pkItems">The pk items.</param>
		/// <returns></returns>
		public static string ConstructKeyFromPkItems(Type type, params object[] pkItems)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (pkItems.Length == 0)
				throw new ArgumentNullException("pkItems");

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(type.Name);

			for (int i = 0; i < pkItems.Length; i++)
			{
				if (pkItems[i] != null)
					sb.Append(pkItems[i].ToString());
			}

			return sb.ToString();
		}
	}

	/// <summary>
	/// Entity Manager provides the management of entity location and creation.
	/// This is responsible for providing the health and validity of entities as a whole.
	/// </summary>
	public static class EntityManager
	{
		#region Fields
		private static object syncObject = new object();
		private static Dictionary<string, IEntityFactory> entityFactoryList = new Dictionary<string, IEntityFactory>();
		private static EntityLocator entityLocator = new EntityLocator();
		#endregion

		#region LocateOrCreate<Entity>
		/// <summary>
		/// Locates an entity for retrieval from the <see cref="Locator"/>, or instatiates a new instance 
		/// of the entity if not currently being tracked.
		/// </summary>
		/// <typeparam name="Entity">Must implement <see cref="IEntity"/> and is the default type to create, and will be the return type.</typeparam>
		/// <param name="key">primary key representation</param>
		/// <param name="typeString">type string to create</param>
		/// <param name="entityFactoryType">factory used to try to create this entity.</param>
		/// <returns>Created entity of T</returns>
		public static Entity LocateOrCreate<Entity>(string key, string typeString, Type entityFactoryType) where Entity : class, IEntity, new()
		{
			return LocateOrCreate<Entity>(key, typeString, entityFactoryType, true);
		}

		/// <summary>
		/// Locates an entity for retrieval from the <see cref="Locator"/>, or instatiates a new instance 
		/// of the entity if not currently being tracked.
		/// </summary>
		/// <typeparam name="Entity">Must implement <see cref="IEntity"/> and is the default type to create, and will be the return type.</typeparam>
		/// <param name="key">primary key representation</param>
		/// <param name="typeString">type string to create</param>
		/// <param name="entityFactoryType">factory used to try to create this entity.</param>
		/// <param name="isLocatorEnabled">bool determining whether to use Entity Locating.</param>
		/// <returns>Created entity of T</returns>
		public static Entity LocateOrCreate<Entity>(string key, string typeString, Type entityFactoryType, bool isLocatorEnabled) where Entity : class, IEntity, new()
		{
			#region Validation
			if (string.IsNullOrEmpty(typeString))
				throw new ArgumentException("typeString");

			if (entityFactoryType == null)
				throw new ArgumentException("entityFactoryType");
			#endregion

			Entity entity = default(Entity);

			//Generated Table Entities Type
			Type defaultType = typeof(Entity);
			bool isCacheable = defaultType.GetInterface("IEntityCacheItem") != null;

			//see if entity is cachable, if IEntityCacheItem
			//retrieve from cache.
			if (isCacheable)
				entity = EntityCache.GetItem<Entity>(key.ToString());

			if (entity != null)
				return entity;

			IEntityFactory factory = null;
			if (EntityFactories.ContainsKey(entityFactoryType.FullName))
				factory = EntityFactories[entityFactoryType.FullName];
			else
				factory = TryAddEntityFactory(entityFactoryType);


			//attempt to locate
			if (key != null && isLocatorEnabled)
			{
				if (EntityLocator.Contains(key))
				{
					entity = EntityLocator.Get(key) as Entity;
				}
			}

			//if not found try create from factory
			if (entity == null)
				entity = factory.CreateEntity(typeString, defaultType) as Entity;

			//add to locator and start tracking.
			if (!entity.IsEntityTracked)
				StartTracking(key, entity, isLocatorEnabled);

			//add entity to Cache if IEntityCacheItem
			if (entity.GetType().GetInterface("IEntityCacheItem") != null)
				EntityCache.AddCache(key, entity);

			return entity;
		}
		#endregion

		#region CreateViewEntity
		/// <summary>
		/// instatiates a new instance of the entity for view entities that don't implement IEntity and can't be tracked
		/// </summary>
		/// <typeparam name="Entity">is the default type to create, and will be the return type.</typeparam>
		/// <param name="typeString">type string to create</param>
		/// <param name="entityFactoryType">factory used to try to create this entity.</param>
		/// <returns>Created entity of T</returns>
		public static Entity CreateViewEntity<Entity>(string typeString, Type entityFactoryType) where Entity : class, new()
		{
			#region Validation
			if (string.IsNullOrEmpty(typeString))
				throw new ArgumentException("typeString");

			if (entityFactoryType == null)
				throw new ArgumentException("entityFactoryType");
			#endregion

			Entity entity = default(Entity);

			//Generated Table Entities Type
			Type defaultType = typeof(Entity);


			IEntityFactory factory = null;
			if (EntityFactories.ContainsKey(entityFactoryType.FullName))
				factory = EntityFactories[entityFactoryType.FullName];
			else
				factory = TryAddEntityFactory(entityFactoryType);

			entity = factory.CreateViewEntity(typeString, defaultType) as Entity;

			return entity;
		}
		#endregion

		#region LocateEntity
		/// <summary>
		/// Locates an entity for retrieval from the <see cref="Locator"/> if tracking is enabled.
		/// </summary>
		/// <typeparam name="Entity">Must implement <see cref="IEntity"/> and is the default type to create, and will be the return type.</typeparam>
		/// <param name="key">primary key representation</param>
		/// <param name="isLocatorEnabled">bool determining whether to use Entity Locating.</param>
		/// <returns>found entity of T, or null</returns>
		public static Entity LocateEntity<Entity>(string key, bool isLocatorEnabled) where Entity : class, IEntity, new()
		{
			Entity entity = null;

			//attempt to locate
			if (key != null && isLocatorEnabled)
			{
				if (EntityLocator.Contains(key))
				{
					entity = EntityLocator.Get(key) as Entity;
				}
			}
			return entity;
		}
		#endregion

		#region StopTracking
		/// <summary>
		/// Stops Tracking an Entity, it will be re-added in the next round.
		/// </summary>
		/// <param name="key">Entity Key used in the Locator's Bucket</param>
		/// <returns>true if found, false if not found</returns>
		public static bool StopTracking(string key)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			return EntityLocator.Remove(key);
		}
		#endregion

		#region StartTracking
		/// <summary>
		/// Starts Tracking an Entity, it will be tracked until modified or persisted.
		/// </summary>
		/// <param name="key">Entity Key used in the Locator's Bucket</param>
		/// <param name="entity">entity to be tracked</param>
		/// <param name="isTrackingEnabled">Determines whether tracking is enabled</param>
		public static void StartTracking(string key, IEntity entity, bool isTrackingEnabled)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			if (entity == null)
				throw new ArgumentNullException("entity");

			if (!entity.IsEntityTracked && isTrackingEnabled)
			{
				EntityLocator.Add(key, entity);
				entity.IsEntityTracked = true;
				entity.EntityTrackingKey = key;
			}

			return;
		}
		#endregion

		#region EntityFactories
		/// <summary>
		/// Exposes the current entity factory instance.
		/// </summary>
		/// <value>The entity factories.</value>
		public static Dictionary<string, IEntityFactory> EntityFactories
		{
			get
			{
				return entityFactoryList;
			}
			set
			{
				if (value != null)
				{
					lock (syncObject)
					{
						entityFactoryList = value;
					}
				}
			}
		}
		#endregion

		#region EntityLocator
		/// <summary>
		/// Expose the current entity locator for consumption by the API.
		/// </summary>
		/// <value>The entity locator.</value>
		public static EntityLocator EntityLocator
		{
			get
			{
				//readonly
				return entityLocator;
			}
		}
		#endregion

		#region TryAddEntityFactory
		/// <summary>
		/// Adds a factory to the EntityFactories property if the parameter type is valid.
		/// </summary>
		/// <param name="entityFactoryTypeToCreate">The entity factory type to create.</param>
		/// <returns>true if successful.</returns>
		public static IEntityFactory TryAddEntityFactory(Type entityFactoryTypeToCreate)
		{
			lock (syncObject)
			{
				if (entityFactoryTypeToCreate == null)
					throw new ArgumentException("entityFactoryTypeToCreate");

				if (!EntityFactories.ContainsKey(entityFactoryTypeToCreate.FullName))
				{
					IEntityFactory createdFactory = Activator.CreateInstance(entityFactoryTypeToCreate) as IEntityFactory;

					if (createdFactory == null)
						throw new ArgumentException("This factory can not be found.  Please ensure that you are using a valid Entity Factory.", "entityFactoryType");

					EntityFactories.Add(entityFactoryTypeToCreate.FullName, (IEntityFactory)createdFactory);
				}
			}
			return EntityFactories[entityFactoryTypeToCreate.FullName];
		}
		#endregion
	}

	#region EntityNotValidException
	/// <summary>
	/// Exception used to pass information along to the UI when an entity is not valid. <see cref="EntityBase"/>.IsValid.
	/// </summary>
	public class EntityNotValidException : Exception
	{
		private EntityBase entity;
		private IList entityList;
		private string executingMethod;
		private static readonly string defaultMessage = "One or more entities is in an invalid state while trying to persist the entity";

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		public EntityNotValidException()
			: base(defaultMessage)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public EntityNotValidException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public EntityNotValidException(string message, Exception exception)
			: base(message, exception)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="method">The method.</param>
		public EntityNotValidException(EntityBase entity, string method)
			: this(entity, method, string.Format(defaultMessage + " for {0} during {1}.", (entity != null ? entity.GetType().Name : ""), method))
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="method">The method.</param>
		/// <param name="message">The message.</param>
		public EntityNotValidException(EntityBase entity, string method, string message)
			: base(message)
		{
			this.entity = entity;
			this.executingMethod = method;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="entityList">The entity list.</param>
		/// <param name="method">The method.</param>
		/// <param name="message">The message.</param>
		public EntityNotValidException(IList entityList, string method, string message)
			: base(message)
		{
			this.entityList = entityList;
			this.executingMethod = method;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="entityList">The entity list.</param>
		/// <param name="method">The method.</param>
		public EntityNotValidException(IList entityList, string method)
			: this(entityList, method, string.Format(defaultMessage + " for {0} during {1}.", (entityList != null ? entityList.GetType().Name : ""), method))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityNotValidException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		public EntityNotValidException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Entity = (EntityBase)info.GetValue("Entity", typeof(EntityBase));
			this.EntityList = (IList)info.GetValue("EntityList", typeof(IList));
			this.ExecutingMethod = info.GetString("ExecutingMethod");
		}

		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		/// <value>The entity.</value>
		public EntityBase Entity
		{
			get
			{
				return entity;
			}
			set
			{
				entity = value;
			}
		}

		/// <summary>
		/// Gets or sets the entity list.
		/// </summary>
		/// <value>The entity list.</value>
		public IList EntityList
		{
			get
			{
				return entityList;
			}
			set
			{
				entityList = value;
			}
		}

		/// <summary>
		/// Gets or sets the executing method.
		/// </summary>
		/// <value>The executing method.</value>
		public string ExecutingMethod
		{
			get
			{
				return executingMethod;
			}
			set
			{
				executingMethod = value;
			}
		}
	}
	#endregion 

	/// <summary>
	/// Provide a generic comparer for our entity objects.
	/// </summary>
	public class EntityPropertyComparer : IComparer
	{
		private string PropertyName;

		/// <summary>
		/// Provides Comparison opreations.
		/// </summary>
		/// <param name="propertyName">The property to compare</param>
		public EntityPropertyComparer(string propertyName)
		{
			PropertyName = propertyName;
		}

		/// <summary>
		/// Compares 2 objects by their properties, given on the constructor
		/// </summary>
		/// <param name="x">First value to compare</param>
		/// <param name="y">Second value to compare</param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			object a = x.GetType().GetProperty(PropertyName).GetValue(x, null);
			object b = y.GetType().GetProperty(PropertyName).GetValue(y, null);

			if (a != null && b == null)
				return 1;

			if (a == null && b != null)
				return -1;

			if (a == null && b == null)
				return 0;

			return ((IComparable)a).CompareTo(b);
		}
	}

	/// <summary>
	/// Provides common utility methods for interacting with objects.
	/// </summary>
	public static class EntityUtil
	{
		/// <summary>
		/// Creates a new instance of the specified type.
		/// </summary>
		/// <param name="type">The runtime type to instantiate.</param>
		/// <returns>An instance of the specified type.</returns>
		public static Object GetNewEntity(Type type)
		{
			return GetNewEntity(type, null);
		}

		/// <summary>
		/// Creates a new instance of the specified type using the supplied
		/// constructor parameters values.
		/// </summary>
		/// <param name="type">The runtime type to instantiate.</param>
		/// <param name="args">The constructor parameter values.</param>
		/// <returns>An instance of the specified type.</returns>
		public static Object GetNewEntity(Type type, params Object[] args)
		{
			ConstructorInfo c = GetConstructor(type, GetTypes(args));
			return (c != null) ? c.Invoke(args) : null;
		}

		/// <summary>
		/// Gets the default constructor for the specified type.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <returns>A <see cref="ConstructorInfo"/> object.</returns>
		public static ConstructorInfo GetConstructor(Type type)
		{
			return GetConstructor(type, null);
		}

		/// <summary>
		/// Gets the constructor for the specified type whose parameters
		/// match the supplied type array.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="types">An array of constructor parameter types.</param>
		/// <returns>A <see cref="ConstructorInfo"/> object.</returns>
		public static ConstructorInfo GetConstructor(Type type, Type[] types)
		{
			ConstructorInfo c = null;

			if (type != null)
			{
				c = type.GetConstructor(types ?? Type.EmptyTypes);
			}

			return c;
		}

		/// <summary>
		/// Gets a <see cref="PropertyInfo"/> object representing the property
		/// belonging to the object having the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>A <see cref="PropertyInfo"/> object, or null if the object
		/// instance does not have a property with the specified name.</returns>
		public static PropertyInfo GetProperty(Object item, String propertyName)
		{
			PropertyInfo prop = null;

			if (item != null)
			{
				prop = GetProperty(item.GetType(), propertyName);
			}

			return prop;
		}

		/// <summary>
		/// Gets a <see cref="PropertyInfo"/> object representing the property
		/// belonging to the runtime type having the specified name.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>A <see cref="PropertyInfo"/> object, or null if the runtime
		/// type does not have a property with the specified name.</returns>
		public static PropertyInfo GetProperty(Type type, String propertyName)
		{
			PropertyInfo prop = null;

			if (type != null && !String.IsNullOrEmpty(propertyName))
			{
				prop = type.GetProperty(propertyName);
			}

			return prop;
		}

		/// <summary>
		/// Gets a <see cref="MethodInfo"/> object representing the method
		/// belonging to the object having the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="methodName">The method name.</param>
		/// <returns>A <see cref="MethodInfo"/> object, or null if the object
		/// instance does not have a method with the specified name.</returns>
		public static MethodInfo GetMethod(Object item, String methodName)
		{
			return GetMethod(item, methodName, null);
		}

		/// <summary>
		/// Gets a <see cref="MethodInfo"/> object representing the method
		/// belonging to the object having the specified name and whose
		/// parameters match the specified types.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="types">The parameter types.</param>
		/// <returns>A <see cref="MethodInfo"/> object, or null if the object
		/// instance does not have a method with the specified name.</returns>
		public static MethodInfo GetMethod(Object item, String methodName, params Type[] types)
		{
			MethodInfo m = null;

			if (item != null)
			{
				m = GetMethod(item.GetType(), methodName, types);
			}

			return m;
		}

		/// <summary>
		/// Gets a <see cref="MethodInfo"/> object representing the method
		/// belonging to the runtime type having the specified name.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="methodName">The method name.</param>
		/// <returns>A <see cref="MethodInfo"/> object, or null if the runtime
		/// type does not have a method with the specified name.</returns>
		public static MethodInfo GetMethod(Type type, String methodName)
		{
			return GetMethod(type, methodName, null);
		}

		/// <summary>
		/// Gets a <see cref="MethodInfo"/> object representing the method
		/// belonging to the runtime type having the specified name and whose
		/// parameters match the specified types.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="types">The parameter types.</param>
		/// <returns>A <see cref="MethodInfo"/> object, or null if the runtime
		/// type does not have a method with the specified name.</returns>
		public static MethodInfo GetMethod(Type type, String methodName, params Type[] types)
		{
			MethodInfo m = null;

			if (type != null && !String.IsNullOrEmpty(methodName))
			{
				m = type.GetMethod(methodName, (types ?? Type.EmptyTypes));
			}

			return m;
		}

		/// <summary>
		/// Invokes the specified method on the object using reflection.
		/// </summary>
		/// <param name="entity">An object instance.</param>
		/// <param name="methodName">The method name.</param>
		/// <returns>The result of the method invocation.</returns>
		public static Object InvokeMethod(Object entity, String methodName)
		{
			return InvokeMethod(entity, methodName, null, null);
		}

		/// <summary>
		/// Invokes the specified method on the object using reflection.
		/// Passes the supplied arguments as method parameters.
		/// </summary>
		/// <param name="entity">An object instance.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="args">The method parameters.</param>
		/// <returns>The result of the method invocation.</returns>
		public static Object InvokeMethod(Object entity, String methodName, Object[] args)
		{
			return InvokeMethod(entity, methodName, args, GetTypes(args));
		}

		/// <summary>
		/// Invokes the specified method on the object using reflection.
		/// Passes the supplied arguments as method parameters.
		/// </summary>
		/// <param name="entity">An object instance.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="args">The method parameters.</param>
		/// <param name="types">The method parameter types.</param>
		/// <returns>The result of the method invocation.</returns>
		public static Object InvokeMethod(Object entity, String methodName, Object[] args, Type[] types)
		{
			MethodInfo m = GetMethod(entity, methodName, types);

			if (m == null)
			{
				// If this were not late-binding it would basically be a compilation error.
				// Throw an exception in order to fail early and in an obvious manner.
				string format = "The method '{0}' with arguments '{1}' could not be located on the specified entity.";
				string typesValue = (types == null) ? "()" : "(" + GetTypeNames(types) + ")";
				throw new ArgumentException(string.Format(format, methodName, typesValue));
			}

			return m.Invoke(entity, args);
		}

		/// <summary>
		/// Gets the System.Type with the specified name.
		/// </summary>
		/// <param name="typeName">The name of the type to get.</param>
		/// <returns>The System.Type with the specified name, if found; otherwise, null.</returns>
		public static Type GetType(String typeName)
		{
			Type type = null;

			if (!String.IsNullOrEmpty(typeName))
			{
				type = Type.GetType(typeName, true);
			}

			return type;
		}

		/// <summary>
		/// Gets an array of System.Type objects which match the specified objects.
		/// NOTE: this method will throw an exception if any of the values held
		/// within the args array are null.
		/// </summary>
		/// <param name="args">An array of objects.</param>
		/// <returns>An array of System.Type objects.</returns>
		public static Type[] GetTypes(params Object[] args)
		{
			Type[] types = Type.EmptyTypes;

			if (args != null)
			{
				types = Type.GetTypeArray(args);
			}

			return types;
		}

		/// <summary>
		/// Gets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>The property value.</returns>
		public static Object GetPropertyValue(Object item, String propertyName)
		{
			PropertyInfo property = null;
			return GetPropertyValue(item, propertyName, out property);
		}

		/// <summary>
		/// Gets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="property">A reference to the <see cref="PropertyInfo"/> object.</param>
		/// <returns>The property value.</returns>
		public static Object GetPropertyValue(Object item, String propertyName, out PropertyInfo property)
		{
			Object value = null;
			property = GetProperty(item, propertyName);

			if (property != null && property.CanRead)
			{
				value = property.GetValue(item, null);
			}

			return value;
		}

		/// <summary>
		/// Gets the value of the static property with the specified name.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>The property value.</returns>
		public static Object GetStaticPropertyValue(Type type, String propertyName)
		{
			PropertyInfo property = null;
			return GetStaticPropertyValue(type, propertyName, out property);
		}

		/// <summary>
		/// Gets the value of the static property with the specified name.
		/// </summary>
		/// <param name="type">The runtime type.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="property">A reference to the <see cref="PropertyInfo"/> object.</param>
		/// <returns>The property value.</returns>
		public static Object GetStaticPropertyValue(Type type, String propertyName, out PropertyInfo property)
		{
			Object value = null;
			property = GetProperty(type, propertyName);

			if (property != null && property.CanRead)
			{
				value = property.GetValue(null, null);
			}

			return value;
		}

		/// <summary>
		/// Sets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		public static void SetPropertyValue(Object item, String propertyName, Object propertyValue)
		{
			SetPropertyValue(item, propertyName, propertyValue, true);
		}

		/// <summary>
		/// Sets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		/// <param name="convertBlankToNull">Boolean indicating whether empty strings should be converted to null values.</param>
		public static void SetPropertyValue(Object item, String propertyName, Object propertyValue, bool convertBlankToNull)
		{
			PropertyInfo property = null;
			SetPropertyValue(item, propertyName, propertyValue, out property, convertBlankToNull);
		}

		/// <summary>
		/// Sets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		/// <param name="property">A reference to the <see cref="PropertyInfo"/> object.</param>
		public static void SetPropertyValue(Object item, String propertyName, Object propertyValue, out PropertyInfo property)
		{
			SetPropertyValue(item, propertyName, propertyValue, out property, true);
		}

		/// <summary>
		/// Sets the value of the property with the specified name.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		/// <param name="property">A reference to the <see cref="PropertyInfo"/> object.</param>
		/// <param name="convertBlankToNull">Boolean indicating whether empty strings should be converted to null values.</param>
		public static void SetPropertyValue(Object item, String propertyName, Object propertyValue, out PropertyInfo property, bool convertBlankToNull)
		{
			property = GetProperty(item, propertyName);

			if (property != null && property.CanWrite)
			{
				Object value = ChangeType(propertyValue, property.PropertyType, convertBlankToNull);
				property.SetValue(item, value, null);
			}
		}

		/// <summary>
		/// Sets the value of the property with the specified name to the value
		/// returned by the Guid.NewGuid() method.
		/// </summary>
		/// <param name="entity">An object instance.</param>
		/// <param name="entityKeyName">The property name.</param>
		/// <returns>The property value.</returns>
		public static Guid SetEntityKeyValue(Object entity, String entityKeyName)
		{
			PropertyInfo property = null;
			Object objId = GetPropertyValue(entity, entityKeyName, out property);
			Guid entityId = Guid.Empty;

			if (property != null && property.PropertyType.IsAssignableFrom(typeof(Guid)))
			{
				if (Guid.Empty.Equals(objId) && property.CanWrite)
				{
					entityId = Guid.NewGuid();
					property.SetValue(entity, entityId, null);
				}
			}

			return entityId;
		}

		/// <summary>
		/// Sets the properties of the specified entity based on the
		/// name/value pairs found in the specified collection.
		/// </summary>
		/// <param name="entity">The instance of an object to set the properties on.</param>
		/// <param name="values">An instance of System.Collections.IDictionary containing the name/value pairs.</param>
		public static void SetEntityValues(Object entity, IDictionary values)
		{
			if (entity != null && values != null)
			{
				Object oValue;

				foreach (Object oKey in values.Keys)
				{
					if (oKey is String)
					{
						oValue = values[oKey];
						SetPropertyValue(entity, oKey.ToString(), oValue);
					}
				}
			}
		}

		/// <summary>
		/// Initializes the properties specified in propertyNames
		/// with the value of DateTime.Now for the specified entity.
		/// </summary>
		/// <param name="entity">The instance of an object to set the properties on.</param>
		/// <param name="propertyNames">The list of property names to initialize.</param>
		public static void InitEntityDateTimeValues(Object entity, params String[] propertyNames)
		{
			if (entity != null && propertyNames != null)
			{
				PropertyInfo prop;

				foreach (String name in propertyNames)
				{
					prop = GetProperty(entity, name);

					if (prop != null && prop.CanWrite && prop.PropertyType.IsAssignableFrom(typeof(DateTime)))
					{
						prop.SetValue(entity, DateTime.Now, null);
					}
				}
			}
		}

		/// <summary>
		/// Determines if the property with the specified name equals the specified value.
		/// </summary>
		/// <param name="item">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		/// <returns>True if the property value matches the specified value; otherwise, false.</returns>
		public static bool IsPropertyValueEqual(Object item, String propertyName, Object propertyValue)
		{
			PropertyInfo property = null;
			Object prevValue = GetPropertyValue(item, propertyName, out property);

			Object currValue = null;
			bool isEqual = false;

			if (property != null)
			{
				currValue = ChangeType(propertyValue, property.PropertyType);
				isEqual = Object.Equals(prevValue, currValue);
			}

			return isEqual;
		}

		/// <summary>
		/// Converts the specified value to the specified type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="conversionType">A System.Type to convert to.</param>
		/// <returns>The results of the conversion.</returns>
		public static Object ChangeType(Object value, Type conversionType)
		{
			return ChangeType(value, conversionType, true);
		}

		/// <summary>
		/// Converts the specified value to the specified type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="conversionType">A System.Type to convert to.</param>
		/// <param name="convertBlankToNull">A value indicating whether to treat
		/// empty string objects as null values.</param>
		/// <returns>The results of the conversion.</returns>
		public static Object ChangeType(Object value, Type conversionType, bool convertBlankToNull)
		{
			Object newValue = null;

			if (convertBlankToNull && value != null)
			{
				if (value is String)
				{
					String strValue = value.ToString().Trim();

					if (String.IsNullOrEmpty(strValue))
					{
						value = null;
					}
				}
			}
			if (conversionType.IsGenericType)
			{
				newValue = ChangeGenericType(value, conversionType, convertBlankToNull);
			}
			else if (value != null)
			{
				// special handling for non-convertible values
				if (!(value is IConvertible))
				{
					// special handling of byte[] types
					if (conversionType == typeof(Byte[]))
					{
						newValue = value;
					}
					else
					{
						value = value.ToString();
					}
				}
				// special handling of Guid types
				if (conversionType == typeof(Guid))
				{
					if (!String.IsNullOrEmpty(value.ToString()))
					{
						newValue = new Guid(value.ToString());
					}
				}
				else
				{
					newValue = Convert.ChangeType(value, conversionType);
				}
			}

			return newValue;
		}

		/// <summary>
		/// Converts the specified value to the specified generic type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="conversionType">A System.Type to convert to.</param>
		/// <returns>The result of the conversion.</returns>
		public static Object ChangeGenericType(Object value, Type conversionType)
		{
			return ChangeGenericType(value, conversionType, true);
		}

		/// <summary>
		/// Converts the specified value to the specified generic type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="conversionType">A System.Type to convert to.</param>
		/// <param name="convertBlankToNull">A value indicating whether to treat
		/// empty string objects as null values.</param>
		/// <returns>The results of the conversion.</returns>
		public static Object ChangeGenericType(Object value, Type conversionType, bool convertBlankToNull)
		{
			Object newValue = null;

			if (conversionType.IsGenericType)
			{
				Type typeDef = conversionType.GetGenericTypeDefinition();
				Type[] typeArgs = conversionType.GetGenericArguments();

				if (typeArgs.Length == 1)
				{
					Type newType = typeArgs[0];
					Object arg = ChangeType(value, newType, convertBlankToNull);
					newValue = GetNewGenericEntity(typeDef, typeArgs, arg);
				}
			}

			return newValue;
		}

		/// <summary>
		/// Creates a reference to a generic type using the specified type definition
		/// and the supplied type arguments.
		/// </summary>
		/// <param name="typeDefinition">A generic type definition.</param>
		/// <param name="typeArguments">An array of System.Type arguments.</param>
		/// <returns>A System.Type representing the generic type.</returns>
		public static Type MakeGenericType(Type typeDefinition, Type[] typeArguments)
		{
			Type genericType = null;

			if (typeDefinition != null && typeArguments != null && typeArguments.Length > 0)
			{
				genericType = typeDefinition.MakeGenericType(typeArguments);
			}

			return genericType;
		}

		/// <summary>
		/// Creates a new instance of the specified generic type.
		/// </summary>
		/// <param name="typeDefinition">A generic type definition.</param>
		/// <param name="typeArguments">An array of System.Type arguments.</param>
		/// <param name="args">An array of constructor parameters values.</param>
		/// <returns>An instance of the generic type.</returns>
		public static Object GetNewGenericEntity(Type typeDefinition, Type[] typeArguments, params Object[] args)
		{
			Type genericType = MakeGenericType(typeDefinition, typeArguments);
			return GetNewGenericEntity(genericType, args);
		}

		/// <summary>
		/// Creates a new instance of the specified generic type.
		/// </summary>
		/// <param name="genericType">The runtime type.</param>
		/// <returns>An instance of the generic type.</returns>
		public static Object GetNewGenericEntity(Type genericType)
		{
			return GetNewGenericEntity(genericType, null);
		}

		/// <summary>
		/// Creates a new instance of the specified generic type.
		/// </summary>
		/// <param name="genericType">The runtime type.</param>
		/// <param name="args">An array of constructor parameters values.</param>
		/// <returns>An instance of the generic type.</returns>
		public static Object GetNewGenericEntity(Type genericType, params Object[] args)
		{
			Object entity = null;

			if (genericType != null)
			{
				// make sure a single null arg was not passed in
				if (args != null && args.Length == 1 && args[0] == null)
				{
					args = null;
				}

				entity = Activator.CreateInstance(genericType, args);
			}

			return entity;
		}

		/// <summary>
		/// Gets a value indicating whether the specified list contains any items.
		/// </summary>
		/// <param name="entities">A collection of objects.</param>
		/// <returns>True if the collection is not null and contains at least
		/// one item; otherwise false.</returns>
		public static bool HasEntities(IList entities)
		{
			return (entities != null && entities.Count > 0);
		}

		/// <summary>
		/// Gets the item within entityList whose property value matches the specifed value.
		/// </summary>
		/// <param name="entities">A collection of objects.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="propertyValue">The property value.</param>
		/// <returns>The object whose property value matches the specified value.</returns>
		public static Object GetEntity(IList entities, String propertyName, Object propertyValue)
		{
			if (HasEntities(entities))
			{
				foreach (Object entity in entities)
				{
					if (IsPropertyValueEqual(entity, propertyName, propertyValue))
					{
						return entity;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the item within entityList at the position specified by index.
		/// </summary>
		/// <param name="entityList">The collection of business objects.</param>
		/// <param name="index">The position within entityList that contains the current item.</param>
		/// <returns>The current business object.</returns>
		public static Object GetEntity(IEnumerable entityList, int index)
		{
			IList list = GetEntityList(entityList);
			Object entity = null;

			if (list.Count > index)
			{
				entity = list[index];
			}

			return entity;
		}

		/// <summary>
		/// Gets the value of the property with the specified name and returns
		/// it as a collection of objects.
		/// </summary>
		/// <param name="entity">An object instance.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>A collection of objects.</returns>
		public static IList GetEntityList(Object entity, String propertyName)
		{
			Object list = EntityUtil.GetPropertyValue(entity, propertyName);
			return GetEntityList(list);
		}

		/// <summary>
		/// Converts the specified object into a collection of objects.
		/// </summary>
		/// <param name="entityList">An object instance.</param>
		/// <returns>A collection of objects.</returns>
		public static IList GetEntityList(Object entityList)
		{
			IList list = null;

			if (entityList == null)
			{
				list = new ArrayList();
			}
			else
			{
				if (entityList is IList)
				{
					list = (IList)entityList;
				}
				else
				{
					list = new ArrayList();

					if (entityList is IEnumerable)
					{
						IEnumerable temp = entityList as IEnumerable;

						foreach (Object item in temp)
						{
							if (item != null)
							{
								list.Add(item);
							}
						}
					}
					else
					{
						list.Add(entityList);
					}
				}
			}

			return list;
		}

		/// <summary>
		/// Adds the specified object to the collection of objects.
		/// </summary>
		/// <param name="list">A collection of objects.</param>
		/// <param name="item">The obejct to add.</param>
		public static void Add(IList list, Object item)
		{
			if (list != null && item != null)
			{
				list.Add(item);
			}
		}

		/// <summary>
		/// Removes the specified object from the collection of objects.
		/// </summary>
		/// <param name="list">A collection of objects.</param>
		/// <param name="item">The object to remove.</param>
		public static void Remove(IList list, Object item)
		{
			if (list != null && item != null)
			{
				if (item is IEntity)
				{
					((IEntity)item).MarkToDelete();
				}

				list.Remove(item);
			}
		}

		/// <summary>
		/// Converts the string representation of a Guid to its Guid 
		/// equivalent. A return value indicates whether the operation 
		/// succeeded. 
		/// </summary>
		/// <param name="s">A string containing a Guid to convert.</param>
		/// <param name="result">
		/// When this method returns, contains the Guid value equivalent to 
		/// the Guid contained in <paramref name="s"/>, if the conversion 
		/// succeeded, or <see cref="Guid.Empty"/> if the conversion failed. 
		/// The conversion fails if the <paramref name="s"/> parameter is a 
		/// <see langword="null" /> reference (<see langword="Nothing" /> in 
		/// Visual Basic), or is not of the correct format. 
		/// </param>
		/// <value>
		/// <see langword="true" /> if <paramref name="s"/> was converted 
		/// successfully; otherwise, <see langword="false" />.
		/// </value>
		/// <exception cref="ArgumentNullException">
		///        Thrown if <pararef name="s"/> is <see langword="null"/>.
		/// </exception>
		public static bool GuidTryParse(string s, out Guid result)
		{
			if (s == null)
				throw new ArgumentNullException("s");

			Regex format = new Regex(
				"^[A-Fa-f0-9]{32}$|" +
				"^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
				"^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

			Match match = format.Match(s);

			if (match.Success)
			{
				result = new Guid(s);
				return true;
			}
			else
			{
				result = Guid.Empty;
				return false;
			}
		}

		/// <summary>Outputs a string containing the type names, delimited by ", "</summary>
		/// <param name="types" type="System.Type[]">
		///     <para>The types to show.</para>
		/// </param>
		/// <returns>A string value...</returns>
		public static string GetTypeNames(params Type[] types)
		{
			StringBuilder builder = new StringBuilder();

			foreach (Type type in types)
			{
				if (builder.Length > 0)
				{
					builder.Append(", ");
				}

				builder.Append(type.Name);
			}

			return builder.ToString();
		}
	}

	/// <summary>
	/// Hold a list of <see cref="Expression"/> instance.
	/// </summary>
	public sealed class Expressions : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <c>Expressions</c> class.
		/// </summary>
		/// <param name="holeFilterExpression">the filter expression that will be parsed to create the collection.</param>
		public Expressions(string holeFilterExpression)
		{
			this.SplitFilter(holeFilterExpression);
		}

		/// <summary>
		/// Initializes a new instance of the <c>Expressions</c> class.
		/// </summary>
		public Expressions()
		{
		}

		/// <summary>
		/// This method split a string filter expression anc create <c>Filter</c> instances.
		/// </summary>
		public void SplitFilter(string HoleFilterExpression)
		{
			int LastPosition = 0;

			//AND
			for (int j = 5; j <= HoleFilterExpression.Length - 5; j++)
			{

				string FiveCurrentChars = HoleFilterExpression.Substring(j - 5, 5).ToUpper();
				if (FiveCurrentChars == " AND ")
				{


					this.Add(new Expression(HoleFilterExpression.Substring(LastPosition, j - LastPosition - 5)));
					LastPosition = j;
				}
			}
			//OR
			for (int z = 4; z <= HoleFilterExpression.Length - 4; z++)
			{
				string TowCurrentChars = HoleFilterExpression.Substring(z - 4, 4).ToUpper();
				if (TowCurrentChars == " OR ")
				{
					this.Add(new Expression(HoleFilterExpression.Substring(LastPosition, z - LastPosition - 4)));
					LastPosition = z;
				}
			}

			//Ajouter le dernier ?lement ou le premier si aucun AND/OR
			this.Add(new Expression(HoleFilterExpression.Substring(LastPosition)));
		}

		/// <summary>
		/// Get the <see cref="Expression"/> at the specified index.
		/// </summary>
		/// <param name="Index">The index of the expression in the collection.</param>
		/// <returns></returns>
		public Expression Item(int Index)
		{
			return (Expression)List[Index];
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public int Add(Expression value)
		{
			return List.Add(value);
		}

		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public void Remove(Expression value)
		{
			List.Remove(value);
		}
	}

	/// <summary>
	///	 Reprensents an expression to filter a collection.
	/// </summary>
	public sealed class Expression
	{
		private string TmpPropertyValue;
		private string TmpOperator;
		private string TmpUserValue;

		/// <summary>
		/// Initializes a new instance of the <see cref="Expression"/> class.
		/// </summary>
		public Expression()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Expression"/> class.
		/// </summary>
		/// <param name="PropValue">The prop value.</param>
		/// <param name="Opr">The opr.</param>
		/// <param name="Usrvalue">The usrvalue.</param>
		public Expression(string PropValue, string Opr, string Usrvalue)
		{
			this.PropertyName = PropValue;
			this.Operator = Opr;
			this.UserValue = this.UserValue = Usrvalue;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Expression"/> class.
		/// </summary>
		/// <param name="wholeExpression">The whole expression.</param>
		public Expression(string wholeExpression)
		{
			string[] Words = new string[2];
			Words = wholeExpression.Split(new char[] { ' ' }, 3);
			this.PropertyName = Words[0];
			this.Operator = Words[1].Trim();
			this.UserValue = Words[2].Trim();
		}

		/// <summary>
		/// Gets or sets the name of the property.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName
		{
			get
			{
				return TmpPropertyValue;
			}
			set
			{
				TmpPropertyValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the operator.
		/// </summary>
		/// <value>The operator.</value>
		public string Operator
		{
			get
			{
				return TmpOperator;
			}
			set
			{
				TmpOperator = value;
			}
		}

		/// <summary>
		/// Gets or sets the user value.
		/// </summary>
		/// <value>The user value.</value>
		public string UserValue
		{
			get
			{
				return TmpUserValue;
			}
			set
			{
				TmpUserValue = value;
			}
		}
	}

	/// <summary>
	/// Represents a filter.
	/// </summary>
	public sealed class Filter<T, Entity>
		where T : ListBase<Entity>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Filter&lt;T, Entity&gt;"/> class.
		/// </summary>
		public Filter()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Filter&lt;T, Entity&gt;"/> class.
		/// </summary>
		/// <param name="objToFilter">The obj to filter.</param>
		/// <param name="filter">The filter.</param>
		public Filter(T objToFilter, string filter)
		{
			this.ApplyFilter(objToFilter, filter);
		}

		#region IsOk
		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(string ObjectPropertyValue, string Operator, string UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue.TrimEnd() == UserValue.TrimEnd())
				{
					Rt = true;
				}
			}
			else if (Operator == "<>" | Operator == "!=")
			{
				if (ObjectPropertyValue.TrimEnd() != UserValue.TrimEnd())
				{
					Rt = true;
				}
			}
			else if (Operator.ToUpper() == "LIKE")
			{
				//recherche des ?toiles
				int LastStrar = UserValue.LastIndexOf("*");
				int UserValueLenght = UserValue.Length;
				string SearchedText = UserValue.Replace("*", "");
				int TextPosition = ObjectPropertyValue.IndexOf(SearchedText);
				if (TextPosition != -1)
				{
					{
						Rt = true;
					}
				}
			}
			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type String !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(int ObjectPropertyValue, string Operator, int UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}

			else if (Operator == "<>" | Operator == "!=")
			{
				if (ObjectPropertyValue != UserValue)
				{
					Rt = true;
				}
			}

			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type int !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(decimal ObjectPropertyValue, string Operator, int UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}

			else if (Operator == "<>" | Operator == "!=")
			{
				if (ObjectPropertyValue != UserValue)
				{
					Rt = true;
				}
			}

			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type int !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(Guid ObjectPropertyValue, string Operator, Guid UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type Guid !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(double ObjectPropertyValue, string Operator, double UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}
			else
			{

				throw new Exception("The operator '" + Operator + "' does not match the type double !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(long ObjectPropertyValue, string Operator, long UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}
			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type double !");
			}
			return Rt;
		}


		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(decimal ObjectPropertyValue, string Operator, decimal UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}
			else
			{

				throw new Exception("The operator '" + Operator + "' does not match the type decimal !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(DateTime ObjectPropertyValue, string Operator, DateTime UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">")
			{
				if (ObjectPropertyValue > UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == ">=")
			{
				if (ObjectPropertyValue >= UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<")
			{
				if (ObjectPropertyValue < UserValue)
				{
					Rt = true;
				}
			}
			else if (Operator == "<=")
			{
				if (ObjectPropertyValue <= UserValue)
				{
					Rt = true;
				}
			}
			else
			{

				throw new Exception("The operator '" + Operator + "' does not match the type DateTime !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">if set to <c>true</c> [object property value].</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">if set to <c>true</c> [user value].</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(bool ObjectPropertyValue, string Operator, bool UserValue)
		{
			bool Rt = false;

			if (Operator == "=")
			{
				if (ObjectPropertyValue == UserValue)
				{
					Rt = true;
				}
			}
			else
			{
				throw new Exception("The operator '" + Operator + "' does not match the type string !");
			}
			return Rt;
		}

		/// <summary>
		/// Determines whether the specified operator is ok.
		/// </summary>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operator is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(string Operator, object UserValue)
		{
			bool Rt = false;

			if (UserValue.ToString().ToUpper() == "NULL" & Operator == "=")
			{
				Rt = true;
			}
			return Rt;
		}

		/// <summary>
		/// Corrects the user value.
		/// </summary>
		/// <param name="UserValue">The user value.</param>
		/// <returns></returns>
		private string CorrectUserValue(string UserValue)
		{
			if (UserValue.Substring(0, 1) == "'")
			{
				UserValue = UserValue.Replace("'", "");
			}

			if (UserValue.Substring(0, 1) == "#")
			{
				UserValue = UserValue.Replace("#", "");
			}

			return UserValue;
		}

		/// <summary>
		/// Determines whether the specified object property value is ok.
		/// </summary>
		/// <param name="ObjectPropertyValue">The object property value.</param>
		/// <param name="Operator">The operator.</param>
		/// <param name="UserValue">The user value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified object property value is ok; otherwise, <c>false</c>.
		/// </returns>
		private bool IsOk(object ObjectPropertyValue, string Operator, string UserValue)
		{
			bool Rt = false;

			//Type of the property value
			if (ObjectPropertyValue == null)
			{
			}

			Type TypeOfValue = ObjectPropertyValue.GetType();
			Object FilterValue = CorrectUserValue(UserValue);

			if (TypeOfValue == typeof(string))
			{
				Rt = IsOk((string)ObjectPropertyValue, Operator, (string)FilterValue);
			}
			else if (TypeOfValue == typeof(int))
			{
				Rt = IsOk((int)ObjectPropertyValue, Operator, (int)Convert.ToInt32(FilterValue));
			}
			else if (TypeOfValue == typeof(double))
			{
				Rt = IsOk((double)ObjectPropertyValue, Operator, (double)Convert.ToDouble(FilterValue));
			}
			else if (TypeOfValue == typeof(decimal))
			{
				Rt = IsOk((decimal)ObjectPropertyValue, Operator, (decimal)Convert.ToDecimal(FilterValue));
			}
			else if (TypeOfValue == typeof(DateTime))
			{
				Rt = IsOk((DateTime)ObjectPropertyValue, Operator, (DateTime)Convert.ToDateTime(FilterValue));
			}
			else if (TypeOfValue == typeof(bool))
			{
				Rt = IsOk((bool)ObjectPropertyValue, Operator, (bool)Convert.ToBoolean(FilterValue));
			}
			else if (TypeOfValue == typeof(Guid))
			{
				Rt = IsOk((Guid)ObjectPropertyValue, Operator, new Guid(FilterValue.ToString()));
			}
			else if (TypeOfValue == typeof(decimal))
			{
				Rt = IsOk((decimal)ObjectPropertyValue, Operator, (decimal)Convert.ToDecimal(FilterValue));
			}
			else if (TypeOfValue == typeof(byte))
			{
				Rt = IsOk((byte)ObjectPropertyValue, Operator, (byte)Convert.ToByte(FilterValue));
			}
			else if (TypeOfValue == typeof(long))
			{
				Rt = IsOk((long)ObjectPropertyValue, Operator, (long)Convert.ToInt64(FilterValue));
			}
			else if (TypeOfValue == typeof(System.Int16))
			{
				Rt = IsOk((System.Int16)ObjectPropertyValue, Operator, (System.Int16)Convert.ToInt16(FilterValue));
			}
			else
			{
				throw new Exception("Filtering is not possible on the type " + TypeOfValue.ToString());
			}
			return Rt;
		}
		#endregion

		/// <summary>
		/// Applies the filter.
		/// </summary>
		/// <param name="ObjectToFilter">The object to filter.</param>
		/// <param name="StrFilter">The STR filter.</param>
		public void ApplyFilter(T ObjectToFilter, string StrFilter)
		{
			if (ObjectToFilter == null)
				throw new ArgumentNullException("ObjectToFilter");
			if (string.IsNullOrEmpty(StrFilter))
				throw new ArgumentNullException("StrFilter");

			int CountValue = ObjectToFilter.Count;
			Expressions ListOfExpressions = new Expressions(StrFilter);
			Type itemType = typeof(Entity);

			//Loading items of the collection
			bool[] Validations;
			bool AllIsOK;

			PropertyInfo[] PropsInfo = new PropertyInfo[ListOfExpressions.Count];
			for (int x = 0; x < ListOfExpressions.Count; x++)
			{
				PropsInfo[x] = itemType.GetProperty(ListOfExpressions.Item(x).PropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (PropsInfo[x] == null)
				{
					throw new Exception("property " + ListOfExpressions.Item(x).PropertyName + " does not exist!");
				}
			}

			for (int f = 0; f <= (ObjectToFilter.Count - 1); f++)
			{
				object CollectionItem = ObjectToFilter[f];

				Validations = new bool[ListOfExpressions.Count];
				AllIsOK = true;

				for (int t = 0; t <= ListOfExpressions.Count - 1; t++)
				{
					PropertyInfo ItemProperty = PropsInfo[t];
					object PropertyValue = ItemProperty.GetValue(CollectionItem, new object[0]);

					if (PropertyValue == null)
					{
						Validations[t] = IsOk(ListOfExpressions.Item(t).Operator, ListOfExpressions.Item(t).UserValue);
					}
					else
					{
						Validations[t] = this.IsOk(PropertyValue, ListOfExpressions.Item(t).Operator, ListOfExpressions.Item(t).UserValue);
					}

					if (Validations[t] == false)
						AllIsOK = false;
				}

				if (AllIsOK == false)
				{
					ObjectToFilter.RemoveFilteredItem(f);
					CountValue -= 1;
					f -= 1;
				}
			}
		}
	}

	/// <summary>
	/// Provides a unified way of converting types of values to other types, as well as for accessing standard values and subproperties.
	/// Used by the nettiers strongly typed collection, so they can be saved in ViewState.
	/// </summary>
	public class GenericTypeConverter : TypeConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericTypeConverter"/> class.
		/// </summary>
		public GenericTypeConverter()
		{
		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type.
		/// </summary>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts the given value object to the specified type.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context,
					  System.Globalization.CultureInfo culture,
					  object val, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				Type valueType = val.GetType();
				ConstructorInfo ci = valueType.GetConstructor(System.Type.EmptyTypes);
				return new InstanceDescriptor(ci, null, false);
			}
			return base.ConvertTo(context, culture, val, destinationType);
		}
	}

	/// <summary>
	/// List of possible state for an entity.
	/// </summary>
	public enum EntityState
	{
		/// <summary>
		/// Entity is unchanged
		/// </summary>
		Unchanged = 0,

		/// <summary>
		/// Entity is new
		/// </summary>
		Added = 1,

		/// <summary>
		/// Entity has been modified
		/// </summary>
		Changed = 2,

		/// <summary>
		/// Entity has been deleted
		/// </summary>
		Deleted = 3
	}

	/// <summary>
	/// The interface that each business object of the model implements.
	/// </summary>
	public partial interface IEntity
	{
		/// <summary>
		///	The name of the underlying database table.
		/// </summary>
		string TableName
		{
			get;
		}

		/// <summary>
		///	Indicates if the object has been modified from its original state.
		/// </summary>
		///<value>True if object has been modified from its original state; otherwise False;</value>
		bool IsDirty
		{
			get;
		}

		/// <summary>
		///	Indicates if the object is new.
		/// </summary>
		///<value>True if objectis new; otherwise False;</value>
		bool IsNew
		{
			get;
		}

		/// <summary>
		/// True if object has been marked as deleted. ReadOnly.
		/// </summary>
		bool IsDeleted
		{
			get;
		}

		/// <summary>
		/// Indicates if the object is in a valid state
		/// </summary>
		/// <value>True if object is valid; otherwise False.</value>
		bool IsValid
		{
			get;
		}

		/// <summary>
		/// Returns one of EntityState enum values - intended to replace IsNew, IsDirty, IsDeleted.
		/// </summary>
		EntityState EntityState
		{
			get;
		}

		/// <summary>
		/// Accepts the changes made to this object by setting each flags to false.
		/// </summary>
		void AcceptChanges();

		/// <summary>
		/// Marks entity to be deleted.
		/// </summary>
		void MarkToDelete();

		/// <summary>
		/// Gets or sets the parent collection.
		/// </summary>
		/// <value>The parent collection.</value>
		object ParentCollection
		{
			get;
			set;
		}

		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		/// <value>A string array that holds the columns names.</value>
		string[] TableColumns
		{
			get;
		}

		/// <summary>
		///     Gets or sets the object that contains supplemental data about this object.
		/// </summary>
		/// <value>Object</value>
		object Tag
		{
			get;
			set;
		}

		/// <summary>
		/// Event to indicate that a property has changed.
		/// </summary>
		event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Determines whether this entity is being tracked.
		/// </summary>
		bool IsEntityTracked
		{
			get;
			set;
		}

		///<summary>
		/// The tracking key used to with the <see cref="EntityLocator" />
		///</summary>
		string EntityTrackingKey
		{
			get;
			set;
		}
	}

	/// <summary>
	/// The interface that each business object of the model implements.
	/// </summary>
	public interface IEntityCacheItem
	{
		TimeSpan EntityCacheDuration
		{
			get;
			set;
		}
		CacheItemPriority EntityCacheItemPriority
		{
			get;
			set;
		}
		ICacheItemExpiration EntityCacheItemExpiration
		{
			get;
			set;
		}
		ICacheItemRefreshAction EntityCacheItemRefreshAction
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Exposes a factory to create an entity based on a typeString and a default type.
	/// </summary>
	public interface IEntityFactory
	{
		/// <summary>
		/// Create an entity based on a string.  
		/// It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString">string of entity to discover and create</param>
		/// <param name="defaultType">if string is not found defaultType will be created.</param>
		/// <returns>Created IEntity object</returns>
		IEntity CreateEntity(string typeString, Type defaultType);

		/// <summary>
		/// Create a readonly entity based on a string for views.  
		/// It will autodiscover the type based on any information we can gather.
		/// </summary>
		/// <param name="typeString">string of entity to discover and create</param>
		/// <param name="defaultType">if string is not found defaultType will be created.</param>
		/// <returns>Created IEntity object</returns>
		Object CreateViewEntity(string typeString, Type defaultType);

		/// <summary>
		/// Gets the current assembly responsible for entity creation.
		/// </summary>
		/// <value>The current assembly.</value>
		System.Reflection.Assembly CurrentEntityAssembly
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Defines a common property which represents the
	/// unique identifier for a business object.
	/// </summary>
	/// <typeparam name="EntityKey">The value type or
	/// class to be used for the EntityId property.</typeparam>
	public interface IEntityId<EntityKey> : IEntity where EntityKey : IEntityKey, new()
	{
		/// <summary>
		/// Gets or sets the value of the unique identifier
		/// for the current business object.
		/// </summary>
		EntityKey EntityId
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Defines a method that allows setting of property values
	/// based on the key/value pairs of an IDictionary object.
	/// </summary>
	public interface IEntityKey
	{
		/// <summary>
		/// Reads values from the supplied IDictionary object into
		/// properties of the current object.
		/// </summary>
		/// <param name="values">An IDictionary instance that contains the key/value
		/// pairs to be used as property values.</param>
		void Load(IDictionary values);

		/// <summary>
		/// Creates a new <see cref="IDictionary"/> object and populates it
		/// with the property values of the current object.
		/// </summary>
		/// <returns>A collection of name/value pairs.</returns>
		IDictionary ToDictionary();
	}

	/// <summary>
	/// A abstract generic base class for the nettiers entities that are generated from tables and views. 
	/// Supports filtering, databinding, searching and sorting.
	/// </summary>
	[Serializable]
	public abstract class ListBase<T> : BindingList<T>, IBindingListView, IBindingList, IList, ICloneable, IListSource, ITypedList, IDisposable, IComponent, IRaiseItemChangedEvents, IDeserializationCallback
	{
		private List<T> _OriginalList = new List<T>();

		// Sorting
		private bool _isSorted = false;
		[NonSerialized]
		private PropertyDescriptor _sortProperty;
		private ListSortDirection _sortDirection = ListSortDirection.Descending;
		[NonSerialized]
		ListSortDescriptionCollection _sortDescriptions = new ListSortDescriptionCollection();

		//Filtering
		private string _filterString = null;
		private List<T> excludedItems = new List<T>();

		private string _listName;
		private bool _containsListCollection = false;

		[NonSerialized]
		private PropertyDescriptorCollection _propertyCollection;

		//TODO: Planned on removing
		//[NonSerialized]
		//private List<PropertyDescriptor> supportsChangeEventsProperties = new List<PropertyDescriptor>();

		[NonSerialized]
		private Dictionary<string, PropertyDescriptorCollection> _childCollectionProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TList{T}"/> class.
		/// </summary>
		public ListBase()
			: base()
		{
			InitializeList();
		}

		/// <summary>
		/// Initialize any member variables when the list is created
		/// </summary>
		private void InitializeList()
		{
			// save the bindable properties in a local field
			_propertyCollection = EntityHelper.GetBindableProperties(typeof(T));

			// save the name of the type for use in the IDE GUI
			_listName = typeof(T).Name;
		}

		#region Core Overrides

		/// <summary>
		/// Gets a value indicating whether the list supports searching. 
		/// </summary>
		protected override bool SupportsSearchingCore
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="prop">The <see cref="PropertyDescriptor"/> to search for.</param>
		/// <param name="key">The value of <i>property</i> to match.</param>
		/// <returns>The zero-based index of the item that matches the property descriptor and contains the specified value. </returns>
		protected override int FindCore(PropertyDescriptor prop, object key)
		{
			return FindCore(prop, key, 0, true);
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="prop">The <see cref="PropertyDescriptor"/> to search for.</param>
		/// <param name="key">The value of <i>property</i> to match.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		/// <returns>The zero-based index of the item that matches the property descriptor and contains the specified value. </returns>
		protected virtual int FindCore(PropertyDescriptor prop, object key, bool ignoreCase)
		{
			return FindCore(prop, key, 0, ignoreCase);
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="prop">The <see cref="PropertyDescriptor"> to search for.</see></param>
		/// <param name="key">The value of <i>property</i> to match.</param>
		/// <param name="start">The index in the list at which to start the search.</param>
		/// <param name="ignoreCase">Indicator of whether to perform a case-sensitive or case insensitive search (string properties only).</param>
		/// <returns>The zero-based index of the item that matches the property descriptor and contains the specified value. </returns>
		protected virtual int FindCore(PropertyDescriptor prop, object key, int start, bool ignoreCase)
		{
			// Simple iteration:
			for (int i = start; i < Count; i++)
			{

				T item = this[i];
				object temp = prop.GetValue(item);
				if ((key == null) && (temp == null))
				{
					return i;
				}
				else if (temp is string)
				{
					if (String.Compare(temp.ToString(), key.ToString(), ignoreCase) == 0)
						return i;
				}
				else if (temp != null && temp.Equals(key))
				{
					return i;
				}
			}
			return -1; // Not found
		}

		/// <summary>
		/// Gets a value indicating whether the list supports sorting. 
		/// </summary>
		protected override bool SupportsSortingCore
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the list is sorted. 
		/// </summary>
		protected override bool IsSortedCore
		{
			get
			{
				return _isSorted;
			}
		}

		/// <summary>
		/// Gets the direction the list is sorted.
		/// </summary>
		protected override ListSortDirection SortDirectionCore
		{
			get
			{
				return _sortDirection;
			}
		}

		/// <summary>
		/// Gets the property descriptor that is used for sorting
		/// </summary>
		/// <returns>The <see cref="PropertyDescriptor"/> used for sorting the list.</returns>
		protected override PropertyDescriptor SortPropertyCore
		{
			get
			{
				return _sortProperty;
			}
		}

		/// <summary>
		/// Sorts the items in the list
		/// </summary>
		/// <param name="prop">A <see cref="PropertyDescriptor"/> that specifies the property to sort on.</param>
		/// <param name="direction">One of the <see cref="ListSortDirection"/> values.</param>
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			_sortDirection = direction;
			_sortProperty = prop;
			SortComparer<T> comparer = new SortComparer<T>(prop, direction);
			ApplySortInternal(comparer);
		}

		/// <summary>
		/// Removes any sort applied to the list.
		/// </summary>
		protected override void RemoveSortCore()
		{
			if (!_isSorted)
				return;

			Clear();
			foreach (T item in _OriginalList)
			{
				Add(item);
			}

			_OriginalList.Clear();
			_sortProperty = null;
			_sortDescriptions = null;
			_isSorted = false;
		}



		#endregion

		#region IBindingListView Members

		/// <summary>
		/// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source.
		/// </summary>
		public string Filter
		{
			get
			{
				return _filterString;
			}
			set
			{
				if (value == this._filterString)
					return;

				this._filterString = value;
				this.ApplyFilter();
			}
		}

		/// <summary>
		/// Removes the current filter applied to the data source..
		/// </summary>
		public void RemoveFilter()
		{
			this.Filter = string.Empty;
		}

		/// <summary>
		/// Gets the collection of sort descriptions currently applied to the data source.
		/// </summary>
		public ListSortDescriptionCollection SortDescriptions
		{
			get
			{
				return _sortDescriptions;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the data source supports advanced sorting.
		/// </summary>
		public bool SupportsAdvancedSorting
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the data source supports filtering.
		/// </summary>
		public bool SupportsFiltering
		{
			get
			{
				return true;
			}
		}

		#region Sorting

		///<summary>
		/// Sorts the data source based on the given <see cref="ListSortDescriptionCollection"/>.
		///</summary>
		///<param name="sorts">The <see cref="ListSortDescriptionCollection"/> containing the sorts to apply to the data source.</param>
		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			_sortProperty = null;
			_sortDescriptions = sorts;
			SortComparer<T> comparer = new SortComparer<T>(sorts);
			ApplySortInternal(comparer);
		}

		///<summary>
		/// Sorts the data source based on a <see cref="PropertyDescriptor">PropertyDescriptor</see> and a <see cref="ListSortDirection">ListSortDirection</see>.
		///</summary>
		///<param name="property">The <see cref="PropertyDescriptor"/> to sort the collection by.</param>
		///<param name="direction">The <see cref="ListSortDirection"/> in which to sort the collection.</param>
		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			ApplySortCore(property, direction);
		}

		/// <summary>
		/// Sorts the elements in the entire list using the specified <see cref="System.Comparison{T}"/>.
		/// </summary>
		/// <param name="comparison">The <see cref="System.Comparison{T}"/> to use when comparing elements.</param>
		/// <exception cref="ArgumentNullException">comparison is a null reference.</exception>
		private void ApplySortInternal(Comparison<T> comparison)
		{
			if (comparison == null)
				throw new ArgumentNullException("The comparison parameter must be a valid object instance.");

			if (_OriginalList.Count == 0)
			{
				_OriginalList.AddRange(this);
			}

			List<T> listRef = this.Items as List<T>;

			if (listRef == null)
				return;

			listRef.Sort(comparison);
			_isSorted = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		/// <summary>
		/// Sorts the elements in the entire list using the specified comparer. 
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing elements, or a null reference (Nothing in Visual Basic) to use the default comparer <see cref="Comparer.Default"/>.</param>
		private void ApplySortInternal(IComparer<T> comparer)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			ApplySortInternal(comparer.Compare);
		}

		/// <summary>
		/// Sorts the elements in the entire list using the specified comparer. 
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing elements, or a null reference (Nothing in Visual Basic) to use the default comparer <see cref="Comparer.Default"/>.</param>
		public void Sort(IComparer<T> comparer)
		{
			ApplySortInternal(comparer);
		}

		/// <summary>
		/// Sorts the elements in the entire list using the specified <see cref="System.Comparison{T}"/>.
		/// </summary>
		/// <param name="comparison">The <see cref="System.Comparison{T}"/> to use when comparing elements.</param>
		/// <exception cref="ArgumentNullException">comparison is a null reference.</exception>
		public void Sort(Comparison<T> comparison)
		{
			ApplySortInternal(comparison);
		}

		/// <summary>
		/// Sorts the elements in the entire list using the specified Order By statement.
		/// </summary>
		/// <param name="orderBy">SQL-like string representing the properties to sort the list by.</param>
		/// <remarks><i>orderBy</i> should be in the following format: 
		/// <para>PropertyName[[ [[ASC]|DESC]][, PropertyName[ [[ASC]|DESC]][,...]]]</para></remarks>
		/// <example><c>list.Sort("Property1, Property2 DESC, Property3 ASC");</c></example>
		public void Sort(string orderBy)
		{
			SortComparer<T> sortComparer = new SortComparer<T>(orderBy);
			ApplySortInternal(sortComparer.Compare);
		}

		#endregion

		#region Filtering


		/// <summary>
		/// Indicates whether a filter is currently applied to the collection.
		/// </summary>
		public bool IsFiltering
		{
			get
			{
				return this.excludedItems.Count > 0;
			}
		}

		/// <summary>
		/// Get the list of items that are excluded by the current filter.
		/// </summary>
		public List<T> ExcludedItems
		{
			get
			{
				return this.excludedItems;
			}
		}

		/// <summary>
		/// Force the filtering of the collection, based on the filter expression set through the <c cref="Filter"/> property.
		/// </summary>
		public void ApplyFilter()
		{
			// Restore the state without filter
			for (int i = 0; i < this.excludedItems.Count; i++)
			{
				this.Add(this.excludedItems[i]);
			}

			// Clear the filterd items
			this.excludedItems.Clear();

			// Application du filtre si non vide
			if (this._filterString != null & this._filterString.Length > 0)
			{
				Filter<ListBase<T>, T> MyFilter = new Filter<ListBase<T>, T>(this, this._filterString);
			}

			// Send a IBindingList list event
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0, 0));
		}

		/// <summary>
		/// Force the filtering of the collection, based on the use of the specified Predicate.
		/// </summary>
		/// <param name="match"></param>
		public void ApplyFilter(Predicate<T> match)
		{
			this._filterString = string.Empty;

			// Restore the state without filter
			for (int i = 0; i < this.excludedItems.Count; i++)
			{
				this.Add(this.excludedItems[i]);
			}

			// Clear the filterd items
			this.excludedItems.Clear();

			for (int i = this.Items.Count - 1; i >= 0; i--)
			{
				if (!match(this.Items[i]))
				{
					RemoveFilteredItem(i);
				}
			}

			// Send a IBindingList list event
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0, 0));
		}

		/// <summary>
		/// Removes the non criteria matching item at the specified index for the current filter set.
		/// Adds the Item to the ExcludedItem  collection without changing EntityState
		/// </summary>
		/// <param name="index">The zero-based index of non criteria matching item to remove.</param>
		internal void RemoveFilteredItem(int index)
		{
			T item = Items[index];

			if (item != null)
			{
				ExcludedItems.Add(item);
				base.RemoveItem(index);
			}
		}
		#endregion

		#endregion

		#region BindingList overrides

		/*
	  //TODO: Planned on removing 
      
	  /// <summary>
      /// Inserts the specified item in the list at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index where the item is to be inserted.</param>
      /// <param name="item">The item to insert in the list.</param>
      protected override void InsertItem(int index, T item)
      {
         foreach (PropertyDescriptor propDesc in supportsChangeEventsProperties) 
		 {
            propDesc.AddValueChanged(item, OnItemChanged);
         }
		
         base.InsertItem(index, item);
      }

      //TODO: Planned on removing 
	  /// <summary>
      /// Removes the item at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index of the item to remove.</param>
      protected override void RemoveItem(int index)
      {
         T item = Items[index];
         foreach (PropertyDescriptor propDesc in TypeDescriptor.GetProperties(item))
         {
            if (propDesc.SupportsChangeEvents)
            {
               propDesc.RemoveValueChanged(item, OnItemChanged);
            }
         }

         base.RemoveItem(index);
      }
	*/

		/// <summary>
		/// Represents the method that will handle the ItemChanged event of the CurrencyManager class
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="args">An EventArgs that contains the event data.</param>
		/// <remarks>This raises the ListChanged event of the list.</remarks>
		void OnItemChanged(object sender, EventArgs args)
		{
			int index = Items.IndexOf((T)sender);
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
		}

		#endregion

		#region IRaiseItemChangedEvents Members

		/// <summary>
		/// Gets a value indicating whether the object raises <see cref="IBindingList.ListChanged"/> events.
		/// </summary>
		bool IRaiseItemChangedEvents.RaisesItemChangedEvents
		{
			get
			{
				return true;
			}
		}

		#endregion

		#region Shuffle

		/// <summary>
		///		Sorts the collection based on a random shuffle.
		/// </summary>
		/// <author>Steven Smith</author>
		/// <url>http://blogs.aspadvice.com/ssmith/archive/2005/01/27/2480.aspx</url>
		///<remarks></remarks>
		public virtual void Shuffle()
		{
			if (this._OriginalList.Count == 0)
			{
				this._OriginalList.AddRange(this);
			}

			//List<T> source = new List<T>(this);
			Random rnd = new Random();
			for (int inx = this.Count - 1; inx > 0; inx--)
			{
				int position = rnd.Next(inx + 1);
				T temp = this[inx];
				this[inx] = this[position];
				this[position] = temp;
			}
		}
		#endregion

		#region ICloneable

		///<summary>
		/// Creates an exact copy of this instance.
		///</summary>
		///<implements><see cref="ICloneable.Clone"/></implements>
		public virtual object Clone()
		{
			throw new NotImplementedException("Method not implemented.");
		}

		///<summary>
		/// Creates an exact copy of this TList{T} object.
		///</summary>
		///<returns>A new, identical copy of the TList{T} casted as object.</returns>
		public static object MakeCopyOf(object x)
		{
			if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
			{
				throw new
					System.NotSupportedException("object not cloneable");
			}
		}
		#endregion ICloneable

		#region PropertyCollection
		/// <summary>
		/// Gets or sets the property descriptor collection for T.  
		/// </summary>
		/// <value>The property collection.</value>
		protected virtual PropertyDescriptorCollection PropertyCollection
		{
			get
			{
				return _propertyCollection;
			}
			set
			{
				_propertyCollection = value;
			}
		}
		#endregion

		#region ToString
		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			string s = this.GetType().Name + " Collection" + Environment.NewLine;
			foreach (T Item in this)
			{
				s += Item.ToString() + Environment.NewLine;
			}
			return s;
		}
		#endregion

		#region EntityChanged
		/// <summary>
		/// Raises the ListChanged event indicating that a item in the list has changed.
		/// </summary>
		/// <param name="entity"></param>
		internal void EntityChanged(T entity)
		{
			OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, IndexOf(entity)));
		}
		#endregion

		#region ITypedList Members
		/// <summary>
		/// This member allows binding objects to discover the field/column information.
		/// </summary>
		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (listAccessors == null || listAccessors.Length == 0)
			{
				return _propertyCollection;
			}
			else // Expect only one argument representing a child collection
			{
				if (_childCollectionProperties == null)
					_childCollectionProperties = new Dictionary<string, PropertyDescriptorCollection>();

				string typeName = listAccessors[0].PropertyType.FullName;

				if (_childCollectionProperties.ContainsKey(typeName))
				{
					return _childCollectionProperties[typeName];
				}
				else
				{
					PropertyDescriptorCollection props = EntityHelper.GetBindableProperties(listAccessors[0].PropertyType);
					_childCollectionProperties.Add(typeName, props);

					return props;
				}
			}
		}


		/// <summary>
		/// This member returns the name displayed in the IDE.
		/// </summary>
		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return _listName;
		}

		#endregion // ITypedList

		#region IListSource Members

		/// <summary>
		/// Clean up. Nothing here though.
		/// </summary>
		public IList GetList()
		{
			return this;
		}

		/// <summary>
		/// Return TRUE if our list contains additional/child lists.
		/// </summary>
		public bool ContainsListCollection
		{
			// TODO: Implement nested lists
			get
			{
				return _containsListCollection;
			}
			set
			{
				_containsListCollection = value;
			}
		}

		#endregion // IListSource

		#region IComponent Members

		// Added to implement Site property correctly.
		private ISite _site = null;

		/// <summary>
		/// Get / Set the site where this data is located.
		/// </summary>
		public ISite Site
		{
			get
			{
				return _site;
			}
			set
			{
				_site = value;
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Notify those that care when we dispose.
		/// </summary>
		public event System.EventHandler Disposed;

		/// <summary>
		/// Clean up. Nothing here though.
		/// </summary>
		public void Dispose()
		{
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

		#region Find

		///<summary>
		/// Finds the first <see cref="IEntity" /> object in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		public virtual T Find(System.Enum column, object value)
		{
			return Find(column.ToString(), value, true);
		}

		///<summary>
		/// Finds the first <see cref="IEntity" /> object in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual T Find(System.Enum column, object value, bool ignoreCase)
		{
			return Find(column.ToString(), value, ignoreCase);
		}

		///<summary>
		/// Finds the first <see cref="IEntity" /> object in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		public virtual T Find(string propertyName, object value)
		{
			return Find(propertyName, value, true);
		}

		///<summary>
		/// Finds the first <see cref="IEntity" /> object in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual T Find(string propertyName, object value, bool ignoreCase)
		{
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
			PropertyDescriptor searchBy = props.Find(propertyName, true);

			if (searchBy != null)
			{
				int index = this.FindCore(searchBy, value, ignoreCase);

				if (index > -1)
				{
					return this[index];
				}
				else
				{
					return default(T);
				}
			}
			else
			{
				//No such property found
				return default(T);
			}
		}

		#endregion Find

		#region IDeserializationCallback Members

		/// <summary>
		/// Runs when the entire object graph has been deserialized.
		/// </summary>
		/// <param name="sender">The object that initiated the callback.</param>
		public void OnDeserialization(object sender)
		{
			InitializeList();
		}

		#endregion

		#region Added Functionality

		/// <summary>
		/// Indicates whether the specified TList object is a null reference (Nothing in Visual Basic) or an empty collection (no item in it).
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns>
		/// 	<c>true</c> if the object is null or has no item; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty(ListBase<T> list)
		{
			return list == null || list.Count == 0;
		}

		/// <summary>
		/// Performs the specified action on each element of the specified array.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ForEach(Action<T> action)
		{
			foreach (T entity in this.Items)
			{
				action(entity);
			}
		}

		/// <summary>
		/// Adds an array of items to the list of items for a TList.
		/// </summary>
		/// <param name="array">The array of items to add.</param>
		public void AddRange(T[] array)
		{
			if (array == null)
				return;
			foreach (T item in array)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Adds an array of items to the list of items for a TList.
		/// </summary>
		/// <param name="list">The list of items to add.</param>
		public void AddRange(ListBase<T> list)
		{
			if (list == null)
				return;
			foreach (T item in list)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Adds an array of items to the list of items for a VList.
		/// </summary>
		/// <param name="list">The list of items to add.</param>
		public void AddRange(List<T> list)
		{
			if (list == null)
				return;
			foreach (T item in list)
			{
				this.Add(item);
			}
		}

		/// <summary>
		/// Retrieves the all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		/// A <see cref="T:TList`1"></see> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:TList`1"></see>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>		
		public ListBase<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}

			ListBase<T> result = this.Clone() as ListBase<T>;
			result.ClearItems();
			foreach (T item in this.Items)
			{
				if (match(item))
				{
					result.Add(item);
				}
			}
			return result;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="T:TList`1"></see>.
		/// </summary>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		/// The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}

			foreach (T item in this.Items)
			{
				if (match(item))
				{
					return item;
				}
			}
			return default(T);
		}

		/*
		/// <summary>
		/// Reverses the order of the elements in the entire <see cref="T:TList{T}"></see>.
		/// </summary>
		public void Reverse()
		{
		   this.Reverse(0, this.Count);
		}
		
		/// <summary>
		/// Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		/// <exception cref="T:System.ArgumentException">index and count do not denote a valid range of elements in the <see cref="T:T:TList{T}"></see>. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than 0.-or-count is less than 0. </exception>
		public void Reverse(int index, int count)
		{
		   if ((index < 0) || (count < 0))
		   {
			  throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Parameter is less than 0.");
		   }
		   if ((this.Count - index) < count)
		   {
			  throw new ArgumentException("index and count do not denote a valid range of elements");
		   }
		   Array.Reverse(this, index, count);
		}
		*/


		#endregion

		#region ToArray
		///<summary>
		/// Convert list of entities to an <see cref="System.Array" />
		///</summary>
		public T[] ToArray()
		{
			List<T> tmpArray = new List<T>(this.Items.Count);
			foreach (T type in this.Items)
			{
				tmpArray.Add(type);
			}
			return tmpArray.ToArray();
		}
		#endregion

		#region ToDataSet
		/// <summary>
		/// Recursively adds child relationships of a <seealso cref="ListBase{T}"/> Entities and 
		/// builds out a nested dataset including <see cref="System.Data.DataRelation"/> relationships.
		/// </summary>
		/// <param name="includeChildren">You can optionally go deep by including includeChildren</param>
		/// <returns>DataSet</returns>
		/// <example>
		///  An example using the Northwind database would be to deep load a TList or VList, 
		///  and then call list.ToDataSet(true/false);
		///  <code><![CDATA[
		///    TList<Categories> list = DataRepository.CategoriesProvider.GetAll();
		///    DataRepository.CategoriesProvider.DeepLoad(list, true);
		///    DataSet ds = list.ToDataSet(true);
		///    ds.WriteXml("C:\\Test2.xml");
		///    ]]></code>
		/// </example>
		public System.Data.DataSet ToDataSet(bool includeChildren)
		{
			System.Data.DataSet dataset = new System.Data.DataSet();

			//recursively convert and fill object graph to a dataset.
			return AddRelations(dataset, null, includeChildren);
		}

		#region AddRelations
		/// <summary>
		/// Recursively adds child relationships of a TList's Entity and builds out a nested dataset including relationships.
		/// </summary>
		/// <param name="dataset">An already instatiated dataset which will be used to fill all objects.</param>
		/// <param name="parentKeys">Used to pass down the parent primary key to a child datatable to add a dataRelation</param>
		/// <param name="includeChildren">bool, include deep load of all child collections in this object graph?</param>
		/// <returns></returns>
		internal System.Data.DataSet AddRelations(System.Data.DataSet dataset, List<System.Data.DataColumn> parentKeys, bool includeChildren)
		{
			if (dataset == null)
				throw new ArgumentException("Invalid parameter context, dataset can not be null in this method context.", "dataset");

			//child property collections
			List<PropertyDescriptor> children = new List<PropertyDescriptor>();
			System.Data.DataTable dataTable = null;
			bool tableExists = false;
			string tName = typeof(T).Name;

			if (!dataset.Tables.Contains(tName))
			{
				//current type table
				dataTable = new System.Data.DataTable(tName);
				tableExists = false;
			}
			else
			{
				dataTable = dataset.Tables[tName];
				tableExists = true;
			}

			//Props
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

			//pks of current type
			List<System.Data.DataColumn> primaryKey = new List<System.Data.DataColumn>();

			//build out table if not exists
			if (!tableExists)
			{
				#region get meta data, build dataTable
				//look at current type's props, and if child hold a ref to it, so that you can recursively add the child.
				foreach (PropertyDescriptor prop in props)
				{
					//TODO: Change to custom entity attribute for PK discovery
					if (prop.Attributes.Matches(new Attribute[] { new ReadOnlyAttribute(false), new BindableAttribute(), new DescriptionAttribute() }))
						primaryKey.Add(dataTable.Columns.Add(prop.Name, prop.PropertyType));
					else if (prop.PropertyType.GetInterface("IList") != null && prop.PropertyType.IsGenericType && (prop.PropertyType.BaseType != null && prop.PropertyType.BaseType.GetGenericTypeDefinition() == typeof(ListBase<>)))
						children.Add(prop);
					else if (prop.PropertyType.GetInterface("IEntity") == null && prop.Attributes.Matches(new Attribute[] { new BindableAttribute(), new DescriptionAttribute() }))
					{
						//check if nullable property, get param, otherwise, just add property type
						Type columnType = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType);
						dataTable.Columns.Add(prop.Name, columnType);
					}
				}
				#endregion
			}

			#region Fill Entity Data
			foreach (T entity in this.Items)
			{
				System.Data.DataRow row = dataTable.NewRow();

				foreach (PropertyDescriptor currentProperty in props)
				{
					if (!dataTable.Columns.Contains(currentProperty.Name))
						continue;

					object val = currentProperty.GetValue(entity);
					row[currentProperty.Name] = (val == null ? DBNull.Value : val);
				}

				dataTable.Rows.Add(row);
			}
			#endregion

			//Add Table
			if (!tableExists)
				dataset.Tables.Add(dataTable);

			#region Add Child DataRelations
			if (parentKeys != null && !tableExists)
			{
				#region if part of a parentkey, then add relationship
				bool skip = false;

				// get last inserted table
				System.Data.DataTable childTable = dataset.Tables[dataset.Tables.Count - 1];

				System.Diagnostics.Trace.WriteLine(childTable.TableName + " - Found");

				//find fk's, add relationships
				List<System.Data.DataColumn> childCols = new List<System.Data.DataColumn>();
				if (childTable != null)
				{
					foreach (System.Data.DataColumn col in parentKeys)
					{
						if (!childTable.Columns.Contains(col.ColumnName))
							skip = true;

						System.Diagnostics.Trace.WriteLine(childTable.TableName + " - Skip " + skip);
						childCols.Add(childTable.Columns[col.ColumnName]);
					}

					if (!skip)
					{
						System.Diagnostics.Trace.WriteLine(childTable.TableName + " - relation added ");
						int key = parentKeys.GetHashCode() + childCols.GetHashCode();
						if (!dataset.Relations.Contains(key.ToString()))
						{
							System.Data.DataRelation relation = dataset.Relations.Add(key.ToString(), parentKeys.ToArray(), childCols.ToArray());
							relation.Nested = true;
						}
					}
				}
				#endregion
			}
			#endregion

			#region Include Entity Child Collections
			// if include the child collections.
			if (includeChildren)
			{
				foreach (PropertyDescriptor child in children)
				{
					//Get DataTable in DataSet, nested recursively.
					foreach (T childEntity in this.Items)
					{
						object[] childEntityParams = new object[3] { dataset, primaryKey, includeChildren };

						//add children
						System.Reflection.MethodInfo addRelationsMethod = child.PropertyType.GetMethod("AddRelations",
							  System.Reflection.BindingFlags.NonPublic |
							  System.Reflection.BindingFlags.Instance);

						//ensure method exists, invoke
						if (addRelationsMethod == null)
							throw new InvalidOperationException("The template method for converting a TList to a Dataset has been altered. Can not include child collections.");

						//create a return reference, don't assign immediately, in case it's null.
						object childDataset = null;
						childDataset = addRelationsMethod.Invoke(Convert.ChangeType(child.GetValue(childEntity), child.PropertyType), childEntityParams);

						// ensure obj not null, 
						if (childDataset == null)
							throw new ArgumentException("Could not successfully convert nested child relationships to a dataset, consider a shallow conversion.");

						//convert it to my dataset, now filled another child
						dataset = (System.Data.DataSet)childDataset;
					}
				}
			}
			#endregion

			//finally return the dataset
			return dataset;
		}
		#endregion
		#endregion

	}

	#region Sort Comparer
	/// <summary>
	/// Generic Sort comparer for the <see cref="TList{T}"/> class.
	/// </summary>
	/// <typeparam name="T">Type of object to sort.</typeparam>
	public class SortComparer<T> : IComparer<T>
	{
		/// <summary>
		/// Collection of properties to sort by.
		/// </summary>
		private ListSortDescriptionCollection m_SortCollection = null;

		/// <summary>
		/// Property to sort by.
		/// </summary>
		private PropertyDescriptor m_PropDesc = null;

		/// <summary>
		/// Direction to sort by
		/// </summary>
		private ListSortDirection m_Direction = ListSortDirection.Ascending;

		/// <summary>
		/// Collection of properties for T.
		/// </summary>
		private PropertyDescriptorCollection m_PropertyDescriptors = null;

		/// <summary>
		/// Create a new instance of the SortComparer class.
		/// </summary>
		/// <param name="propDesc">The <see cref="PropertyDescriptor"/> to sort by.</param>
		/// <param name="direction">The <see cref="ListSortDirection"/> to sort the list.</param>
		public SortComparer(PropertyDescriptor propDesc, ListSortDirection direction)
		{
			Initialize();
			m_PropDesc = propDesc;
			m_Direction = direction;
		}

		/// <summary>
		/// Create a new instance of the SortComparer class.
		/// </summary>
		/// <param name="sortCollection">A <see cref="ListSortDescriptionCollection"/> containing the properties to sort the list by.</param>
		public SortComparer(ListSortDescriptionCollection sortCollection)
		{
			Initialize();
			m_SortCollection = sortCollection;
		}

		/// <summary>
		/// Create a new instance of the SortComparer class.
		/// </summary>
		/// <param name="orderBy">SQL-like string representing the properties to sort the list by.</param>
		/// <remarks><i>orderBy</i> should be in the following format: 
		/// <para>PropertyName[[ [[ASC]|DESC]][, PropertyName[ [[ASC]|DESC]][,...]]]</para></remarks>
		/// <example><c>list.Sort("Property1, Property2 DESC, Property3 ASC");</c></example>
		public SortComparer(string orderBy)
		{
			Initialize();
			m_SortCollection = ParseOrderBy(orderBy);
		}

		#region IComparer<T> Members

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>Value is less than zero: <c>x</c> is less than <c>y</c>
		/// <para>Value is equal to zero: <c>x</c> equals <c>y</c></para>
		/// <para>Value is greater than zero: <c>x</c> is greater than <c>y</c></para>
		/// </returns>
		public int Compare(T x, T y)
		{
			if (m_PropDesc != null) // Simple sort 
			{
				object xValue = m_PropDesc.GetValue(x);
				object yValue = m_PropDesc.GetValue(y);
				return CompareValues(xValue, yValue, m_Direction);
			}
			else if (m_SortCollection != null && m_SortCollection.Count > 0)
			{
				return RecursiveCompareInternal(x, y, 0);
			}
			else
				return 0;
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Compare two objects
		/// </summary>
		/// <param name="xValue">The first object to compare</param>
		/// <param name="yValue">The second object to compare</param>
		/// <param name="direction">The direction to sort the objects in</param>
		/// <returns>Returns an integer representing the order of the objects</returns>
		private int CompareValues(object xValue, object yValue, ListSortDirection direction)
		{

			int retValue = 0;
			if (xValue != null && yValue == null)
			{
				retValue = 1;
			}
			else if (xValue == null && yValue != null)
			{
				retValue = -1;

			}
			else if (xValue == null && yValue == null)
			{
				retValue = 0;
			}
			else if (xValue is IComparable) // Can ask the x value
			{
				retValue = ((IComparable)xValue).CompareTo(yValue);
			}
			else if (yValue is IComparable) //Can ask the y value
			{
				retValue = ((IComparable)yValue).CompareTo(xValue);
			}
			else if (!xValue.Equals(yValue)) // not comparable, compare String representations
			{
				retValue = xValue.ToString().CompareTo(yValue.ToString());
			}
			if (direction == ListSortDirection.Ascending)
			{
				return retValue;
			}
			else
			{
				return retValue * -1;
			}
		}

		private int RecursiveCompareInternal(T x, T y, int index)
		{
			if (index >= m_SortCollection.Count)
				return 0; // termination condition

			ListSortDescription listSortDesc = m_SortCollection[index];
			object xValue = listSortDesc.PropertyDescriptor.GetValue(x);
			object yValue = listSortDesc.PropertyDescriptor.GetValue(y);

			int retValue = CompareValues(xValue, yValue, listSortDesc.SortDirection);
			if (retValue == 0)
			{
				return RecursiveCompareInternal(x, y, ++index);
			}
			else
			{
				return retValue;
			}
		}

		/// <summary>
		/// Parses a string into a <see cref="ListSortDescriptionCollection"/>.
		/// </summary>
		/// <param name="orderBy">SQL-like string of sort properties</param>
		/// <returns></returns>
		private ListSortDescriptionCollection ParseOrderBy(string orderBy)
		{
			if (orderBy == null || orderBy.Length == 0)
				throw new ArgumentNullException("orderBy");

			string[] props = orderBy.Split(',');
			ListSortDescription[] sortProps = new ListSortDescription[props.Length];
			string prop;
			ListSortDirection sortDirection = ListSortDirection.Ascending;

			for (int i = 0; i < props.Length; i++)
			{
				//Default to Ascending
				sortDirection = ListSortDirection.Ascending;
				prop = props[i].Trim();

				if (prop.ToUpper().EndsWith(" DESC"))
				{
					sortDirection = ListSortDirection.Descending;
					prop = prop.Substring(0, prop.ToUpper().LastIndexOf(" DESC"));
				}
				else if (prop.ToUpper().EndsWith(" ASC"))
				{
					prop = prop.Substring(0, prop.ToUpper().LastIndexOf(" ASC"));
				}

				prop = prop.Trim();

				//Get the appropriate descriptor
				PropertyDescriptor propertyDescriptor = m_PropertyDescriptors[prop];

				if (propertyDescriptor == null)
				{
					throw new ArgumentException(string.Format("The property \"{0}\" is not a valid property.", prop));
				}
				sortProps[i] = new ListSortDescription(propertyDescriptor, sortDirection);

			}

			return new ListSortDescriptionCollection(sortProps);
		}

		/// <summary>
		/// Initializes the SortComparer object
		/// </summary>
		private void Initialize()
		{
			Type instanceType = typeof(T);

			if (!instanceType.IsPublic)
				throw new ArgumentException(string.Format("Type \"{0}\" is not public.", typeof(T).FullName));

			m_PropertyDescriptors = TypeDescriptor.GetProperties(typeof(T));

		}

		#endregion
	}
	#endregion

	/// <summary>
	/// Represents a strongly typed list of .netTiers table entity that can be accessed by index. 
	/// Provides methods to search, sort, and manipulate lists.
	/// </summary>
	[Serializable]
	public class TList<T> : ListBase<T> where T : IEntity, new()
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TList{T}"/> class.
		/// </summary>
		public TList()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TList{T}"/> class based on another list.
		/// </summary>
		public TList(IList existingList)
		{
			if (existingList != null)
			{
				foreach (T item in existingList)
				{
					if (item != null)
						this.Items.Add(item);
				}
			}
		}



		#region RemoveEntity
		/// <summary>
		/// Removes the entity item at the specified index and places it in the DeletedItems collection.
		/// If this list were to be persisted, it would delete the entity from the repository.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveEntity(int index)
		{
			RemoveItem(index);
		}

		/// <summary>
		/// Removes the entity item and places it in the DeletedItems collection.
		/// If this list were to be persisted, it would delete the entity from the repository.
		/// </summary>
		/// <param name="item">The entity to delete and place in DeletedItems collection.</param>
		public void RemoveEntity(T item)
		{
			RemoveEntity(Items.IndexOf(item));
		}
		#endregion

		#region BindingList overrides

		/// <summary>
		/// Inserts the specified item in the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index where the item is to be inserted.</param>
		/// <param name="item">The item to insert in the list.</param>
		protected override void InsertItem(int index, T item)
		{
			//Set the parentCollection property
			item.ParentCollection = this;

			base.InsertItem(index, item);
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		protected override void RemoveItem(int index)
		{
			T item = Items[index];

			if (item != null)
			{
				//Move item to deleted collection(if not in added state)
				if (item.EntityState != EntityState.Added)
				{
					item.MarkToDelete();
					DeletedItems.Add(item);
				}
				base.RemoveItem(index);
			}
		}

		#endregion

		#region ICloneable

		///<summary>
		/// Creates an exact copy of this TList{T} instance.
		///</summary>
		///<returns>The TList{T} object this method creates, cast as an object.</returns>
		///<implements><see cref="ICloneable.Clone"/></implements>
		public override object Clone()
		{
			return this.Copy();
		}

		///<summary>
		/// Creates an exact copy of this TList{T} object.
		///</summary>
		///<returns>A new, identical copy of the TList{T}.</returns>
		public virtual TList<T> Copy()
		{
			TList<T> copy = new TList<T>();
			foreach (T item in this)
			{
				T itemCopy = (T)MakeCopyOf(item);
				copy.Add(itemCopy);
			}
			return copy;
		}
		#endregion ICloneable

		#region Added Functionality

		/// <summary>
		/// Accepts the changes made to underlyting entities.
		/// </summary>
		public virtual void AcceptChanges()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].AcceptChanges();
			}
		}

		/// <summary>
		/// Adds the data in this collection to another collection
		/// </summary>
		/// <param name="copyTo"></param>
		public virtual void CopyTo(TList<T> copyTo)
		{
			// make sure not to duplicate the item if it's already there
			ArrayList alreadySeenItem = new ArrayList();
			foreach (T item in copyTo)
			{
				alreadySeenItem.Add(item.ToString());
			}

			foreach (T item in this)
			{
				T itemCopy = (T)MakeCopyOf(item);
				if (alreadySeenItem.IndexOf(itemCopy.ToString()) < 0)
				{
					copyTo.Add(itemCopy);
				}
			}
		}

		/// <summary>
		///		Indicates whether the collection was modified or not, and thus if it needs to be saved.
		/// </summary>
		///<returns>true is the collection needs to be saved; otherwise false.</returns>
		public virtual bool IsDirty
		{
			get
			{
				return IsNewCount > 0 || IsDeletedCount > 0 || IsDirtyCount > 0;
			}
		}

		/// <summary>
		///		Returns the number of items that have been marked new in the collection.
		/// </summary>
		///<returns>the number of items that have been marked new in the collection</returns>
		public virtual int IsNewCount
		{
			get
			{
				int count = 0;
				foreach (T item in this)
				{
					//if(item.IsNew)
					if (item.EntityState == EntityState.Added)
						count += 1;
				}
				return count;
			}
		}

		/// <summary>
		///		Returns the number of items that have been marked to delete in the collection.
		/// </summary>
		///<returns>the number of items that have been marked for deletation in the collection</returns>
		public virtual int IsDeletedCount
		{
			get
			{
				/*
				int count = 0;
				foreach(T item in this)
				{
					//if(item.IsDeleted)
					if(item.EntityState == EntityState.Deleted)
						count += 1;
				}
				return count;
				*/
				return DeletedItems.Count;
			}
		}

		/// <summary>
		///		Returns the number of items that have been marked as modified in the collection.
		/// </summary>
		///<returns>the number of items that have been marked as modified in the collection</returns>
		public virtual int IsDirtyCount
		{
			get
			{
				int count = 0;
				foreach (T item in this)
				{
					//if(item.IsDirty)
					if (item.EntityState == EntityState.Changed)
						count += 1;
				}
				return count;
			}
		}

		/// <summary>
		/// Returns whether all items contained in the list.
		/// </summary>
		/// <value>True if all items are valid; otherwise False.</value>
		public virtual bool IsValid
		{
			get
			{
				bool rtn = true;

				foreach (T item in this)
				{
					if (!item.IsValid)
					{
						rtn = false;
						break;
					}
				}

				return rtn;
			}
		}

		/// <summary>
		/// Returns a <see cref="List{T}"/> object of invalid items.
		/// </summary>
		public List<T> InvalidItems
		{
			get
			{
				List<T> invalidItems = new List<T>();

				foreach (T item in this)
				{
					if (!item.IsValid)
					{
						invalidItems.Add(item);
					}
				}

				return invalidItems;
			}
		}

		#region Deleted items

		private List<T> deletedItems;

		/// <summary>
		/// Hold a collection of item that we want to delete. they are removed from the main collection, so the databinding is working.
		/// </summary>
		/// <remark>The save method will loop on this collection to delete item from the datasource.</remark>
		public List<T> DeletedItems
		{
			get
			{
				if (this.deletedItems == null)
				{
					this.deletedItems = new List<T>();
				}
				return this.deletedItems;
			}
		}

		#endregion

		/// <summary>
		/// Performs the specified action on each element of the specified array.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="action">The action.</param>
		public static void ForEach<U>(TList<U> list, Action<U> action) where U : IEntity, new()
		{
			list.ForEach(action);
		}

		/// <summary>
		/// Gets the range
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public TList<T> GetRange(int index, int count)
		{
			if ((index < 0) || (count < 0))
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "ArgumentOutOfRange_NeedNonNegNum");
			}

			TList<T> list1 = new TList<T>();
			for (int i = index; i < index + count && i < this.Count; i++)
			{
				list1.Add(this.Items[i]);
			}
			return list1;
		}

		#endregion	Added Functionality

		#region Find

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		public virtual TList<T> FindAll(System.Enum column, object value)
		{
			return FindAll(column.ToString(), value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual TList<T> FindAll(System.Enum column, object value, bool ignoreCase)
		{
			return FindAll(column.ToString(), value, ignoreCase);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		public virtual TList<T> FindAll(string propertyName, object value)
		{
			return FindAll(propertyName, value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual TList<T> FindAll(string propertyName, object value, bool ignoreCase)
		{
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
			PropertyDescriptor searchBy = props.Find(propertyName, true);

			TList<T> result = new TList<T>();

			int index = 0;

			while (index > -1)
			{
				index = this.FindCore(searchBy, value, index, ignoreCase);

				if (index > -1)
				{
					result.Add(this[index]);

					//Increment the index to start at the next item
					index++;
				}
			}

			return result;
		}

		/// <summary>
		/// Retrieves the all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		/// A <see cref="T:TList{T}"></see> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:TList{T}"></see>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>		
		public new TList<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}

			TList<T> result = new TList<T>();
			foreach (T item in this.Items)
			{
				if (match(item))
				{
					result.Add(item);
				}
			}
			return result;
		}

		/// <summary>
		/// Determines whether the <see cref="T:TList{T}"></see> contains elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		/// true if the <see cref="T:TList{T}"></see> contains one or more elements that match the conditions defined by the specified predicate; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>
		public bool Exists(Predicate<T> match)
		{
			return (this.FindIndex(match) != -1);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:TList{T}"></see>.
		/// </summary>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, 1.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this.Count, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:TList{T}"></see> that extends from the specified index to the last element.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, 1.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the <see cref="T:TList{T}"></see>.</exception>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this.Count - startIndex, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:TList{T}"></see> that starts at the specified index and contains the specified number of elements.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, 1.
		/// </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">startIndex is outside the range of valid indexes for the <see cref="T:TList{T}"></see>.-or-count is less than 0.-or-startIndex and count do not specify a valid section in the <see cref="T:TList{T}"></see>.</exception>
		/// <exception cref="T:System.ArgumentNullException">match is null.</exception>
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex", "index is out of range");
			}

			if ((count < 0) || (startIndex > (this.Count - count)))
			{
				throw new ArgumentOutOfRangeException("count", "count is out of range");
			}

			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			int num1 = startIndex + count;
			for (int num2 = startIndex; num2 < num1; num2++)
			{
				if (match(this[num2]))
				{
					return num2;
				}
			}
			return -1;
		}


		#region Find All By

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		public virtual TList<T> FindAllBy(FindAllByType findAllByType, System.Enum column, object value)
		{
			return FindAllBy(findAllByType, column.ToString(), value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual TList<T> FindAllBy(FindAllByType findAllByType, System.Enum column, object value, bool ignoreCase)
		{
			return FindAllBy(findAllByType, column.ToString(), value, ignoreCase);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		public virtual TList<T> FindAllBy(FindAllByType findAllByType, string propertyName, object value)
		{
			return FindAllBy(findAllByType, propertyName, value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual TList<T> FindAllBy(FindAllByType findAllByType, string propertyName, object value, bool ignoreCase)
		{
			PropertyDescriptorCollection props = base.PropertyCollection;
			PropertyDescriptor searchBy = props.Find(propertyName, true);

			TList<T> result = new TList<T>();

			int index = 0;

			while (index > -1)
			{
				index = this.FindAllBy(findAllByType, searchBy, value, index, ignoreCase);

				if (index > -1)
				{
					result.Add(this[index]);

					//Increment the index to start at the next item
					index++;
				}
			}

			return result;
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="prop">The <see cref="PropertyDescriptor"> to search for.</see></param>
		/// <param name="key">The value of <i>property</i> to match.</param>
		/// <param name="start">The index in the list at which to start the search.</param>
		/// <param name="ignoreCase">Indicator of whether to perform a case-sensitive or case insensitive search (string properties only).</param>
		/// <returns>The zero-based index of the item that matches the property descriptor and contains the specified value. </returns>
		protected virtual int FindAllBy(FindAllByType findAllByType, PropertyDescriptor prop, object key, int start, bool ignoreCase)
		{
			// Simple iteration:
			for (int i = start; i < Count; i++)
			{

				T item = this[i];
				object temp = prop.GetValue(item);
				if ((key == null) && (temp == null))
				{
					return i;
				}
				else if (temp is string)
				{
					switch (findAllByType)
					{
						case FindAllByType.StartsWith:
							{
								if (temp.ToString().StartsWith(key.ToString(), ignoreCase == true ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
									return i;
								break;
							}
						case FindAllByType.EndsWith:
							{
								if (temp.ToString().EndsWith(key.ToString(), ignoreCase == true ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
									return i;
								break;
							}
						case FindAllByType.Contains:
							{
								if (temp.ToString().Contains(key.ToString()))
									return i;
								break;
							}
					}
				}
				else if (temp != null && temp.Equals(key))
				{
					return i;
				}
			}
			return -1; // Not found
		}

		///<summary>
		/// Used to by FindAllBy method to all for easy searching.
		/// </summary>
		[Serializable]
		public enum FindAllByType
		{
			/// <summary>
			/// Starts with Value in List
			/// </summary>
			StartsWith,

			/// <summary>
			/// Ends with Value in List
			/// </summary>
			EndsWith,

			/// <summary>
			/// Contains Value in List
			/// </summary>
			Contains
		}
		#endregion Find All By

		#endregion Find
	}

	/// <summary>
	/// A generic collection for the nettiers entities that are generated from views. 
	/// Supports filtering, databinding, searching and sorting.
	/// </summary>
	[Serializable]
	public class VList<T> : ListBase<T>
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VList{T}"/> class.
		/// </summary>
		public VList()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VList{T}"/> class based on another list.
		/// </summary>
		public VList(IList existingList)
		{
			if (existingList != null)
			{
				foreach (T item in existingList)
				{
					if (item != null)
						this.Items.Add(item);
				}
			}
		}

		#region BindingList overrides

		/// <summary>
		/// Inserts the specified item in the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index where the item is to be inserted.</param>
		/// <param name="item">The item to insert in the list.</param>
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		#endregion

		#region ICloneable

		///<summary>
		/// Creates an exact copy of this VList{T} instance.
		///</summary>
		///<returns>The VList{T} object this method creates, cast as an object.</returns>
		///<implements><see cref="ICloneable.Clone"/></implements>
		public override object Clone()
		{
			return this.Copy();
		}

		///<summary>
		/// Creates an exact copy of this VList{T} object.
		///</summary>
		///<returns>A new, identical copy of the VList{T}.</returns>
		public virtual VList<T> Copy()
		{
			VList<T> copy = new VList<T>();
			foreach (T item in this)
			{
				T itemCopy = (T)MakeCopyOf(item);
				copy.Add(itemCopy);
			}
			return copy;
		}
		#endregion ICloneable

		#region Added Functionality

		/// <summary>
		/// Performs the specified action on each element of the specified array.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="action">The action.</param>
		public static void ForEach<U>(VList<U> list, Action<U> action)
		{
			list.ForEach(action);
		}

		#endregion	Added Functionality

		#region Find

		///<summary>
		/// Finds a collection of objects in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		public virtual VList<T> FindAll(System.Enum column, object value)
		{
			return FindAll(column.ToString(), value, true);
		}

		///<summary>
		/// Finds a collection of objects in the current list matching the search criteria.
		///</summary>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual VList<T> FindAll(System.Enum column, object value, bool ignoreCase)
		{
			return FindAll(column.ToString(), value, ignoreCase);
		}

		///<summary>
		/// Finds a collection of objects in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		public virtual VList<T> FindAll(string propertyName, object value)
		{
			return FindAll(propertyName, value, true);
		}

		///<summary>
		/// Finds a collection of objects in the current list matching the search criteria.
		///</summary>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual VList<T> FindAll(string propertyName, object value, bool ignoreCase)
		{
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
			PropertyDescriptor searchBy = props.Find(propertyName, true);

			VList<T> result = new VList<T>();

			int index = 0;

			while (index > -1)
			{
				index = this.FindCore(searchBy, value, index, ignoreCase);

				if (index > -1)
				{
					result.Add(this[index]);

					//Increment the index to start at the next item
					index++;
				}
			}

			return result;
		}

		#region Find All By

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		public virtual VList<T> FindAllBy(FindAllByType findAllByType, System.Enum column, object value)
		{
			return FindAllBy(findAllByType, column.ToString(), value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual VList<T> FindAllBy(FindAllByType findAllByType, System.Enum column, object value, bool ignoreCase)
		{
			return FindAllBy(findAllByType, column.ToString(), value, ignoreCase);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		public virtual VList<T> FindAllBy(FindAllByType findAllByType, string propertyName, object value)
		{
			return FindAllBy(findAllByType, propertyName, value, true);
		}

		///<summary>
		/// Finds a collection of <see cref="IEntity" /> objects in the current list matching the search criteria.
		///</summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="propertyName">Property of the object to search.</param>
		/// <param name="value">Value to find.</param>
		/// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
		public virtual VList<T> FindAllBy(FindAllByType findAllByType, string propertyName, object value, bool ignoreCase)
		{
			PropertyDescriptorCollection props = base.PropertyCollection;
			PropertyDescriptor searchBy = props.Find(propertyName, true);

			VList<T> result = new VList<T>();

			int index = 0;

			while (index > -1)
			{
				index = this.FindAllBy(findAllByType, searchBy, value, index, ignoreCase);

				if (index > -1)
				{
					result.Add(this[index]);

					//Increment the index to start at the next item
					index++;
				}
			}

			return result;
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="findAllByType"><see cref="FindAllByType" /> Type to easily search by</param>
		/// <param name="prop">The <see cref="PropertyDescriptor"> to search for.</see></param>
		/// <param name="key">The value of <i>property</i> to match.</param>
		/// <param name="start">The index in the list at which to start the search.</param>
		/// <param name="ignoreCase">Indicator of whether to perform a case-sensitive or case insensitive search (string properties only).</param>
		/// <returns>The zero-based index of the item that matches the property descriptor and contains the specified value. </returns>
		protected virtual int FindAllBy(FindAllByType findAllByType, PropertyDescriptor prop, object key, int start, bool ignoreCase)
		{
			// Simple iteration:
			for (int i = start; i < Count; i++)
			{

				T item = this[i];
				object temp = prop.GetValue(item);
				if ((key == null) && (temp == null))
				{
					return i;
				}
				else if (temp is string)
				{
					switch (findAllByType)
					{
						case FindAllByType.StartsWith:
							{
								if (temp.ToString().StartsWith(key.ToString(), ignoreCase == true ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
									return i;
								break;
							}
						case FindAllByType.EndsWith:
							{
								if (temp.ToString().EndsWith(key.ToString(), ignoreCase == true ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
									return i;
								break;
							}
						case FindAllByType.Contains:
							{
								if (temp.ToString().Contains(key.ToString()))
									return i;
								break;
							}
					}
				}
				else if (temp != null && temp.Equals(key))
				{
					return i;
				}
			}
			return -1; // Not found
		}

		///<summary>
		/// Used to by FindAllBy method to all for easy searching.
		/// </summary>
		[Serializable]
		public enum FindAllByType
		{
			/// <summary>
			/// Starts with Value in List
			/// </summary>
			StartsWith,

			/// <summary>
			/// Ends with Value in List
			/// </summary>
			EndsWith,

			/// <summary>
			/// Contains Value in List
			/// </summary>
			Contains
		}
		#endregion Find All By
		#endregion Find

	}
}

namespace NetTiersTestModel.Entities.Validation
{
	/// <summary>
	/// Object representing a broken validation rule
	/// </summary>
	[Serializable()]
	public class BrokenRule
	{
		private string _ruleName;
		private string _description;
		private string _property;

		/// <summary>
		/// Default parameterless constructor used by Reflection for Soap Serialization
		/// </summary>
		private BrokenRule()
		{
			// used by Reflection.
		}

		/// <summary>
		/// Creates a instance of the object.
		/// </summary>
		/// <param name="rule"><see cref="ValidationRuleInfo"/> containing the details about the rule that was broken.</param>
		internal BrokenRule(ValidationRuleInfo rule)
		{
			_ruleName = rule.RuleName;
			_description = rule.ValidationRuleArgs.Description;
			_property = rule.ValidationRuleArgs.PropertyName;
		}

		/// <summary>
		/// Provides access to the name of the broken rule.
		/// </summary>
		/// <value>The name of the rule.</value>
		public string RuleName
		{
			get
			{
				return _ruleName;
			}
		}

		/// <summary>
		/// The description of the broken rule.
		/// </summary>
		/// <value>The description of the rule.</value>
		public string Description
		{
			get
			{
				return _description;
			}
		}

		/// <summary>
		/// The name of the property affected by the broken rule.
		/// </summary>
		/// <value>The property affected by the rule.</value>
		public string Property
		{
			get
			{
				return _property;
			}
		}
	}

	/// <summary>
	/// A List of broken rules.
	/// </summary>
	[Serializable()]
	public class BrokenRulesList : BindingList<BrokenRule>
	{

		/// <summary>
		/// Returns the firstRule <see cref="BrokenRule" /> object
		/// corresponding to the specified property.
		/// </summary>
		/// <param name="property">The name of the property affected by the rule.</param>
		/// <returns>
		/// The firstRule BrokenRule object corresponding to the specified property, or null if 
		/// there are no rules defined for the property.
		/// </returns>
		public BrokenRule GetFirstBrokenRule(string property)
		{
			foreach (BrokenRule item in this)
				if (item.Property == property)
					return item;
			return null;
		}

		/// <summary>
		/// Internal contructor
		/// </summary>
		internal BrokenRulesList()
		{
			// limit creation to this assembly
		}

		/// <summary>
		/// Add a broken rule to the list
		/// </summary>
		/// <param name="rule"><see cref="ValidationRuleInfo"/> object containing the details about the rule.</param>
		internal void Add(ValidationRuleInfo rule)
		{
			Remove(rule);
			Add(new BrokenRule(rule));

		}

		/// <summary>
		/// Removes a broken rule from the list
		/// </summary>
		/// <param name="rule"><see cref="ValidationRuleInfo"/> object containing the details about the rule.</param>
		internal void Remove(ValidationRuleInfo rule)
		{

			for (int index = Count - 1; index >= 0; index--)
				if (this[index].RuleName == rule.RuleName)
				{
					RemoveAt(index);
					break;
				}

		}

		/// <summary>
		/// Returns a string containing all of the broken rule descriptions for the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the errors for.</param>
		/// <returns>String of the error descriptions</returns>
		public string GetPropertyErrorDescriptions(string propertyName)
		{
			System.Text.StringBuilder errorDescription = new System.Text.StringBuilder();
			bool firstRule = true;
			foreach (BrokenRule item in this)
			{
				if (string.IsNullOrEmpty(propertyName) || item.Property.Equals(propertyName))
				{
					if (firstRule)
						firstRule = false;
					else
						errorDescription.Append(Environment.NewLine);

					errorDescription.Append(item.Description);
				}
			}

			return errorDescription.ToString();
		}

		/// <summary>
		/// Returns the description of each broken rule separated by a new line.
		/// </summary>
		public override string ToString()
		{
			return GetPropertyErrorDescriptions(null);
		}
	}

	/// <summary>
	/// Static class that contains common validation rules.  Each rule conforms to the <see cref="ValidationRuleArgs"/> delegate.
	/// </summary>
	public static class CommonRules
	{
		#region NotNull

		/// <summary>
		/// Rule that does not allow a property value to be null
		/// </summary>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		/// <returns>Returns true if the property value is not null; false otherwise.</returns>
		public static bool NotNull(object target, ValidationRuleArgs e)
		{
			PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

			if (p != null)
			{
				object value = p.GetValue(target, null);

				if (value == null)
				{
					if (string.IsNullOrEmpty(e.Description))
						e.Description = string.Format("{0} can not be null.", e.PropertyName);
					return false;
				}

				return true;
			}
			else
			{
				throw new ArgumentException(string.Format("Property \"{0}\" not found on object \"{1}\"", e.PropertyName, target.GetType().ToString()));
			}
		}

		#endregion

		#region StringRequired

		/// <summary>
		/// Rule ensuring a String value contains one or more
		/// characters.
		/// </summary>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		/// <remarks>
		/// This implementation uses late binding, and will only work
		/// against String property values.
		/// </remarks>
		public static bool StringRequired(object target, ValidationRuleArgs e)
		{
			PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

			if (p != null)
			{
				string value = (string)p.GetValue(target, null);
				if (string.IsNullOrEmpty(value))
				{
					if (string.IsNullOrEmpty(e.Description))
						e.Description = e.PropertyName + " required";
					return false;
				}
				return true;
			}
			else
			{
				throw new ArgumentException(string.Format("Property \"{0}\" not found on object \"{1}\"", e.PropertyName, target.GetType().ToString()));
			}

		}

		#endregion

		#region StringMaxLength

		/// <summary>
		/// Rule ensuring a String value doesn't exceed
		/// a specified length.
		/// </summary>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		/// <remarks>
		/// This implementation uses late binding, and will only work
		/// against String property values.
		/// </remarks>
		public static bool StringMaxLength(object target, ValidationRuleArgs e)
		{
			MaxLengthRuleArgs args = e as MaxLengthRuleArgs;
			if (args != null)
			{
				int max = args.MaxLength;

				PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

				if (p != null)
				{
					if (p.PropertyType == typeof(string))
					{
						string value = (string)p.GetValue(target, null);

						if (!String.IsNullOrEmpty(value) && (value.Length > max))
						{
							if (string.IsNullOrEmpty(e.Description))
								e.Description = String.Format("{0} can not exceed {1} characters", e.PropertyName, max.ToString());
							return false;
						}
						return true;
					}
					else
					{
						throw new ArgumentException(string.Format("Property \"{0}\" is not of type String.", e.PropertyName));
					}
				}
				else
				{
					throw new ArgumentException(string.Format("Property \"{0}\" not found on object \"{1}\"", e.PropertyName, target.GetType().ToString()));
				}
			}
			else
			{
				throw new ArgumentException("Invalid ValidationRuleArgs.  e must be of type MaxLengthRuleArgs.");
			}

		}

		/// <summary>
		/// Class used with the <see cref="StringMaxLength"/>.
		/// </summary>
		public class MaxLengthRuleArgs : ValidationRuleArgs
		{
			private int _maxLength;

			/// <summary>
			/// Maximum length of the string property.
			/// </summary>
			public int MaxLength
			{
				get
				{
					return _maxLength;
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="propertyName">Property to validate</param>
			/// <param name="maxLength">Max length of the property</param>
			public MaxLengthRuleArgs(
			  string propertyName, int maxLength)
				: base(propertyName)
			{
				_maxLength = maxLength;
			}

			/// <summary>
			/// Return a string representation of the object.
			/// </summary>
			public override string ToString()
			{
				return base.ToString() + "!" + _maxLength.ToString();
			}
		}

		#endregion

		#region MaxWords

		/// <summary>
		/// Summary description for MaxWordsRuleArgs.
		/// </summary>
		public class MaxWordsRuleArgs : ValidationRuleArgs
		{
			/// <summary>
			/// Creates a new instance of the MaxWordsRuleArgs class.
			/// </summary>
			/// <param name="propertyName"></param>
			/// <param name="maxLength"></param>
			public MaxWordsRuleArgs(string propertyName, int maxLength)
				: base(propertyName)
			{
				this._maxLength = maxLength;
			}


			/// <summary>
			/// Return a string representation of the object.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return (base.ToString() + "!" + this._maxLength.ToString());
			}


			/// <summary>
			/// Gets the value of the MaxLength property.
			/// </summary>
			public int MaxLength
			{
				get
				{
					return this._maxLength;
				}
			}


			// Fields
			private int _maxLength;
		}

		/// <summary>
		/// Summary description for MaxWords.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public static bool MaxWords(object target, ValidationRuleArgs e)
		{
			CommonRules.MaxWordsRuleArgs args1 = e as CommonRules.MaxWordsRuleArgs;
			if (args1 == null)
			{
				throw new ArgumentException("Invalid ValidationRuleArgs. e must be of type MaxWordsRuleArgs.");
			}
			string text1 = @"\b\w+\b";
			PropertyInfo info1 = target.GetType().GetProperty(e.PropertyName);
			if (info1 == null)
			{
				throw new ArgumentException(string.Format("Property \"{0}\" not found on object \"{1}\"", e.PropertyName, target.GetType().ToString()));
			}
			if (info1.PropertyType != typeof(string))
			{
				throw new ArgumentException(string.Format("Property \"{0}\" is not of type String.", e.PropertyName));
			}
			string text2 = (string)info1.GetValue(target, null);
			if (Regex.Matches(text2, text1).Count > args1.MaxLength)
			{
				if (e.Description == string.Empty)
				{
					e.Description = string.Format("{0} exceed the maximum number of words", e.PropertyName, text1);
				}
				return false;
			}
			return true;
		}

		#endregion

		#region RegexIsMatch

		/// <summary>
		/// Rule ensuring a String value is matching
		/// a specified regular expression.
		/// </summary>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated, must be of type RegexRuleArgs</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		/// <remarks>
		/// This implementation uses late binding, and will only work
		/// against String property values.
		/// </remarks>
		public static bool RegexIsMatch(object target, ValidationRuleArgs e)
		{
			RegexRuleArgs args = e as RegexRuleArgs;
			if (args != null)
			{
				string expression = args.Expression;

				PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

				if (p != null)
				{
					if (p.PropertyType == typeof(string))
					{
						string value = (string)p.GetValue(target, null);

						if (value == null || !Regex.IsMatch(value, expression))
						{
							if (string.IsNullOrEmpty(e.Description))
								e.Description = String.Format("{0} do not match the regular expression {1}", e.PropertyName, expression);
							return false;
						}
						return true;
					}
					else
					{
						throw new ArgumentException(string.Format("Property \"{0}\" is not of type String.", e.PropertyName));
					}
				}
				else
				{
					throw new ArgumentException(string.Format("Property \"{0}\" not found on object \"{1}\"", e.PropertyName, target.GetType().ToString()));
				}
			}
			else
			{
				throw new ArgumentException("Invalid ValidationRuleArgs.  e must be of type RegexRuleArgs.");
			}

		}

		/// <summary>
		/// Class used with the <see cref="RegexIsMatch"/>.
		/// </summary>
		public class RegexRuleArgs : ValidationRuleArgs
		{
			private string _expression;

			/// <summary>
			/// The Regular expression that the string have to match.
			/// </summary>
			public string Expression
			{
				get
				{
					return _expression;
				}
			}

			/// <summary>
			/// Initializes a new instance of the RegexRuleArgs class.
			/// </summary>
			/// <param name="propertyName">Property to validate</param>
			/// <param name="expression">The Regular expression that the property have to match</param>
			public RegexRuleArgs(
			  string propertyName, string expression)
				: base(propertyName)
			{
				_expression = expression;
			}

			/// <summary>
			/// Return a string representation of the object.
			/// </summary>
			public override string ToString()
			{
				return base.ToString() + "!" + _expression;
			}
		}

		#endregion

		#region CompareValues

		/// <summary>
		/// Generic rule that determines if an object's property is less than a particular value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool LessThanValue<T>(object target, ValidationRuleArgs e)
		{
			return CompareValues<T>(target, e as CompareValueRuleArgs<T>, CompareType.LessThan);
		}

		/// <summary>
		/// Generic rule that determines if an object's property is less than or equal to a particular value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool LessThanOrEqualToValue<T>(object target, ValidationRuleArgs e)
		{
			return CompareValues<T>(target, e as CompareValueRuleArgs<T>, CompareType.LessThanOrEqualTo);
		}

		/// <summary>
		/// Generic rule that determines if an object's property is equal to a particular value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool EqualsValue<T>(object target, ValidationRuleArgs e)
		{
			return CompareValues<T>(target, e as CompareValueRuleArgs<T>, CompareType.EqualTo);
		}

		/// <summary>
		/// Generic rule that determines if an object's property is greater than a particular value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool GreaterThanValue<T>(object target, ValidationRuleArgs e)
		{
			return CompareValues<T>(target, e as CompareValueRuleArgs<T>, CompareType.GreaterThan
			   );
		}

		/// <summary>
		/// Generic rule that determines if an object's property is greater than or equal to a particular value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool GreaterThanOrEqualToValue<T>(object target, ValidationRuleArgs e)
		{
			return CompareValues<T>(target, e as CompareValueRuleArgs<T>, CompareType.GreaterThanOrEqualTo);
		}

		/// <summary>
		/// Private method that compares a property value with a specified value.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate.</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <param name="compareType"><see cref="CompareType"/> defining the type of comparison that will be made.</param>
		/// <returns></returns>
		private static bool CompareValues<T>(object target, CompareValueRuleArgs<T> e, CompareType compareType)
		{
			bool result = true;

			if (e != null)
			{
				T compareValue = e.CompareValue;

				PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

				T value;

				//if (p.PropertyType.Name.Equals(typeof(Nullable<>).Name))
				//{
				//}
				try
				{
					value = (T)p.GetValue(target, null);
				}
				catch (Exception)
				{
					return true;
				}

				// if the property is read from a nullable type, then a null valid is considered as allowed
				if (p.PropertyType.Name.Equals(typeof(Nullable<>).Name) && value == null)
				{
					return true;
				}

				int res = Comparer.DefaultInvariant.Compare(value, compareValue);

				switch (compareType)
				{
					case CompareType.LessThanOrEqualTo:
						result = (res <= 0);

						if (!result)
						{
							if (string.IsNullOrEmpty(e.Description))
							{
								e.Description = string.Format("{0} can not exceed {1}",
e.PropertyName, compareValue.ToString());
							}
						}
						break;

					case CompareType.LessThan:
						result = (res < 0);

						if (!result)
						{
							if (string.IsNullOrEmpty(e.Description))
							{
								e.Description = string.Format("{0} must be less than {1}",
e.PropertyName, compareValue.ToString());
							}
						}
						break;

					case CompareType.EqualTo:
						result = (res == 0);

						if (!result)
						{
							if (string.IsNullOrEmpty(e.Description))
							{
								e.Description = string.Format("{0} must equal {1}",
e.PropertyName, compareValue.ToString());
							}
						}
						break;

					case CompareType.GreaterThan:
						result = (res > 0);

						if (!result)
						{
							if (string.IsNullOrEmpty(e.Description))
							{
								e.Description = string.Format("{0} must exceed {1}",
e.PropertyName, compareValue.ToString());
							}
						}
						break;

					case CompareType.GreaterThanOrEqualTo:
						result = (res >= 0);

						if (!result)
						{
							if (string.IsNullOrEmpty(e.Description))
							{
								e.Description = string.Format("{0} must be greater than or equal to {1}",
e.PropertyName, compareValue.ToString());
							}
						}
						break;

				}

				if (!result)
				{

				}
			}
			return result;
		}

		/// <summary>
		/// Enum indicating the type of comparison that will be made.
		/// </summary>
		private enum CompareType
		{
			LessThanOrEqualTo,
			LessThan,
			EqualTo,
			GreaterThan,
			GreaterThanOrEqualTo
		}

		/// <summary>
		/// Class used with the <see cref="CompareValues{T}"/> rules.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class CompareValueRuleArgs<T> : ValidationRuleArgs
		{
			T _compareValue;

			/// <summary>
			/// Value to be compared against an object's property.
			/// </summary>
			public T CompareValue
			{
				get
				{
					return _compareValue;
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="propertyName">Name of the property to be validated.</param>
			/// <param name="compareValue">The value to be compared against the property.</param>
			public CompareValueRuleArgs(string propertyName, T compareValue)
				: base(propertyName)
			{
				_compareValue = compareValue;
			}

			/// <summary>
			/// Returns a string representation of the object.
			/// </summary>
			public override string ToString()
			{
				return base.ToString() + "!" + _compareValue.ToString();
			}
		}

		#endregion

		#region InRange

		/// <summary>
		/// Generic rule that determines if an object's property is within a specified range.
		/// </summary>
		/// <typeparam name="T">Datatype of the property to validate.  Must implement <see cref="System.IComparable{T}"/>.</typeparam>
		/// <param name="target">Object containing the data to validate.</param>
		/// <param name="e"><see cref="ValidationRuleArgs"/> containing the information about the object to be validated.</param>
		/// <returns>False if the rule is broken; true otherwise.</returns>
		public static bool InRange<T>(object target, ValidationRuleArgs e)
		{
			bool result = true;

			RangeRuleArgs<T> ruleArgs = e as RangeRuleArgs<T>;

			if (ruleArgs != null)
			{
				PropertyInfo p = target.GetType().GetProperty(e.PropertyName);

				T value = (T)p.GetValue(target, null);

				result = ruleArgs.Range.Contains(value);

				if (!result)
				{
					if (string.IsNullOrEmpty(e.Description))
						e.Description = string.Format("{0} must be between {1} and {2}.", ruleArgs.PropertyName, ruleArgs.Range.MinValue, ruleArgs.Range.MaxValue);
				}

				return result;
			}
			else
			{
				throw new ArgumentException("Must be of type RangeRuleArgs.", "e");
			}
		}

		/// <summary>
		/// Class used to do a range comparison on a property.
		/// </summary>
		/// <typeparam name="T">Datatype of the property being validated.</typeparam>
		public class Range<T>
		{
			private readonly T minValue;
			private readonly T maxValue;

			/// <summary>
			/// Creates a new instance of the <see cref="T:Range"/> class.
			/// </summary>
			/// <param name="minValue">The minimum value of the property.</param>
			/// <param name="maxValue">The maximum value of the property.</param>
			public Range(T minValue, T maxValue)
			{
				//Make sure that the user has not reversed the values
				if (Comparer.DefaultInvariant.Compare(minValue, maxValue) <= 0)
				{
					this.minValue = minValue;
					this.maxValue = maxValue;
				}
				else
				{
					//Values are reversed
					this.minValue = maxValue;
					this.maxValue = minValue;
				}
			}

			/// <summary>
			/// The minimum value in the range.
			/// </summary>
			public T MinValue
			{
				get
				{
					return this.minValue;
				}
			}

			/// <summary>
			/// The maximum value in the range.
			/// </summary>
			public T MaxValue
			{
				get
				{
					return this.maxValue;
				}
			}

			/// <summary>
			/// Compares the specified value with the <see cref="MinValue"/> and <see cref="MaxValue"/>
			/// to determine if the value is within the range.
			/// </summary>
			/// <param name="value">The value to find within the current range</param>
			/// <returns>True if the value is within the range (inclusive); False otherwise.</returns>
			public bool Contains(T value)
			{
				return Comparer.DefaultInvariant.Compare(value, MinValue) >= 0 && Comparer.DefaultInvariant.Compare(value, MaxValue) <= 0;
			}

			/// <summary>
			/// Returns a string representation of the object.
			/// </summary>
			public override string ToString()
			{
				return base.ToString() + "!" + minValue.ToString() + "-" + maxValue.ToString();
			}
		}

		/// <summary>
		/// Validation Rule Argument class
		/// </summary>
		/// <typeparam name="T">Datatype of the property being validated.</typeparam>
		public class RangeRuleArgs<T> : ValidationRuleArgs
		{
			private Range<T> range;

			/// <summary>
			/// Creates a new instance of the <see cref="T:RangeRuleArgs"/> class.
			/// </summary>
			/// <param name="propertyName">Name of the property to be validated.</param>
			/// <param name="minValue">The minimum value of the property.</param>
			/// <param name="maxValue">The maximum value of the property.</param>

			public RangeRuleArgs(string propertyName, T minValue, T maxValue)
				: base(propertyName)
			{
				range = new Range<T>(minValue, maxValue);
			}

			/// <summary>
			/// Creates a new instance of the <see cref="T:RangeRuleArgs"/> class.
			/// </summary>
			/// <param name="propertyName">Name of the property to be validated.</param>
			/// <param name="range"><see cref="T:Range"/> object containing the range of valid values for the property.</param>
			public RangeRuleArgs(string propertyName, Range<T> range)
				: base(propertyName)
			{
				this.range = range;
			}

			/// <summary>
			/// Returns the <see cref="T:Range{T}"/> object associated with this instance.
			/// </summary>
			public Range<T> Range
			{
				get
				{
					return this.range;
				}
			}

			/// <summary>
			/// Returns a string representation of the object.
			/// </summary>
			public override string ToString()
			{
				return base.ToString() + "!" + range.ToString();
			}
		}

		#endregion
	}

	/// <summary>
	/// Object that provides additional information about an validation rule.
	/// </summary>
	public class ValidationRuleArgs
	{
		private string _propertyName;
		private string _description;

		/// <summary>
		/// The name of the property to be validated.
		/// </summary>
		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
		}

		/// <summary>
		/// Detailed description of why the rule was invalidated.  This should be set from the method handling the rule.
		/// </summary>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		/// <summary>
		/// Creates an instance of the object
		/// </summary>
		/// <param name="propertyName">The name of the property to be validated.</param>
		public ValidationRuleArgs(string propertyName)
		{
			_propertyName = propertyName;
		}

		/// <summary>
		/// Return a string representation of the object.
		/// </summary>
		public override string ToString()
		{
			return _propertyName;
		}
	}

	/// <summary>
	/// Delegate providing the signature of all methods that will process validation rules.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The method handler should set the Description attribute of the 
	/// <see cref="ValidationRuleArgs"/> parameter so that a meaningful
	/// error is returned.
	/// </para><para>
	/// If the data is valid, the method must return true.  If the data is invalid,
	/// the Description should be set the false should be returned.
	/// </para>
	/// </remarks>
	public delegate bool ValidationRuleHandler(object target, ValidationRuleArgs e);

	/// <summary>
	/// Object representing a validation rule for an object
	/// </summary>
	internal class ValidationRuleInfo
	{
		private object _target;
		private ValidationRuleHandler _handler;
		private string _ruleName = String.Empty;
		private ValidationRuleArgs _args;

		/// <summary>
		/// Returns a text representation of the rule which is the <see cref="RuleName"/>.
		/// </summary>
		public override string ToString()
		{
			return _ruleName;
		}

		/// <summary>
		/// Gets the name of the rule.
		/// </summary>
		/// <remarks>
		/// The rule's name must be unique and is used
		/// to identify a broken rule in the <see cref="BrokenRulesList"/>.
		/// </remarks>
		public string RuleName
		{
			get
			{
				return _ruleName;
			}
		}

		/// <summary>
		/// Returns information about the property that is associated with the rule.
		/// </summary>
		public ValidationRuleArgs ValidationRuleArgs
		{
			get
			{
				return _args;
			}
		}

		/// <summary>
		/// Creates and initializes the rule.
		/// </summary>
		/// <param name="target">Object reference containing the data to validate.</param>
		/// <param name="handler">The address of the method implementing <see cref="ValidationRuleHandler"/>.</param>
		/// <param name="propertyName">The name of the property to which the rule applies.</param>
		public ValidationRuleInfo(object target, ValidationRuleHandler handler, string propertyName)
			: this(target, handler, new ValidationRuleArgs(propertyName))
		{
		}

		/// <summary>
		/// Creates and initializes the rule.
		/// </summary>
		/// <param name="target">Object reference containing the data to validate.</param>
		/// <param name="handler">The address of the method implementing <see cref="ValidationRuleHandler"/>.</param>
		/// <param name="args">A <see cref="ValidationRuleArgs"/> object.</param>
		public ValidationRuleInfo(object target, ValidationRuleHandler handler, ValidationRuleArgs args)
		{
			_target = target;
			_handler = handler;
			_args = args;
			_ruleName = _handler.Method.Name + "!" + _args.ToString();
		}

		/// <summary>
		/// Invokes the rule to validate the data.
		/// </summary>
		/// <returns>True if the data is valid, False if the data is invalid.</returns>
		public bool Invoke()
		{
			return _handler.Invoke(_target, _args);
		}
	}

	/// <summary>
	/// Maintains the list of validation rules associated with an object
	/// </summary>
	[Serializable()]
	public class ValidationRules
	{
		/// <summary>
		/// List of rules that have not passed validation
		/// </summary>
		private BrokenRulesList _brokenRules;

		/// <summary>
		/// Object associated with this list of rules.
		/// </summary>
		[NonSerialized()]
		private object _target;

		[NonSerialized()]
		private Dictionary<string, List<ValidationRuleInfo>> _rulesList;

		/// <summary>
		/// Creates an instance of the class and associates the target.
		/// </summary>
		/// <param name="businessEntity">Target</param>
		internal ValidationRules(object businessEntity)
		{
			this.Target = businessEntity;
		}

		/// <summary>
		/// Object associated with this list of rules.
		/// </summary>
		internal object Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = value;
			}
		}

		/// <summary>
		/// List of <see cref="BrokenRule"/> objects
		/// </summary>
		private BrokenRulesList BrokenRulesList
		{
			get
			{
				if (_brokenRules == null)
					_brokenRules = new BrokenRulesList();
				return _brokenRules;
			}
		}

		/// <summary>
		/// Read-only list of validation rules
		/// </summary>
		private Dictionary<string, List<ValidationRuleInfo>> RulesList
		{
			get
			{
				if (_rulesList == null)
					_rulesList = new Dictionary<string, List<ValidationRuleInfo>>();
				return _rulesList;
			}
		}

		#region Adding Rules

		/// <summary>
		/// Returns a list of <see cref="ValidationRuleInfo"/> objects for a specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property to get the rules for.</param>
		/// <returns>A <see cref="List{ValidationRuleInfo}"/> containing all of the rules for the specified property.</returns>
		private List<ValidationRuleInfo> GetPropertyRules(string propertyName)
		{
			List<ValidationRuleInfo> list = null;

			//See if the list of rules exists
			if (RulesList.ContainsKey(propertyName))
				list = RulesList[propertyName];
			if (list == null)
			{
				//No list found - create a new one.
				list = new List<ValidationRuleInfo>();
				RulesList.Add(propertyName, list);
			}
			return list;
		}

		/// <summary>
		/// Adds a rule to the list of validated rules.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A rule is implemented by a method which conforms to the 
		/// method signature defined by the <see cref="ValidationRuleHandler" /> delegate.
		/// </para>
		/// </remarks>
		/// <param name="handler">The method that implements the rule.</param>
		/// <param name="propertyName">
		/// The name of the property on the target object where the rule implementation can retrieve
		/// the value to be validated.
		/// </param>
		public void AddRule(ValidationRuleHandler handler, string propertyName)
		{
			AddRule(handler, new ValidationRuleArgs(propertyName));
		}

		/// <summary>
		/// Adds a rule to the list of validated rules.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A rule is implemented by a method which conforms to the 
		/// method signature defined by the <see cref="ValidationRuleHandler" /> delegate.
		/// </para>
		/// </remarks>
		/// <param name="handler">The method that implements the rule.</param>
		/// <param name="args">
		/// A <see cref="ValidationRuleArgs"/> object specifying the property name and other arguments
		/// passed to the rule method
		/// </param>
		public void AddRule(ValidationRuleHandler handler, ValidationRuleArgs args)
		{
			// get the list of rules for the property
			List<ValidationRuleInfo> list = GetPropertyRules(args.PropertyName);

			// we have the list, add our new rule
			list.Add(new ValidationRuleInfo(_target, handler, args));
		}

		#endregion

		#region Validating Rules

		/// <summary>
		/// Validates a list of rules.
		/// </summary>
		/// <remarks>
		/// This method calls the Invoke method on each rule in the list.  If the rule fails, it 
		/// is added to the <see cref="BrokenRulesList"/>
		/// </remarks>
		/// <param name="ruleList">List of rules to validate.</param>
		private void ValidateRuleList(List<ValidationRuleInfo> ruleList)
		{
			foreach (ValidationRuleInfo rule in ruleList)
			{
				if (rule.Invoke())
					BrokenRulesList.Remove(rule);
				else
					BrokenRulesList.Add(rule);
			}
		}

		/// <summary>
		/// Validates all rules for a property
		/// </summary>
		/// <param name="propertyName">Name of the property to validate.</param>
		public void ValidateRules(string propertyName)
		{
			List<ValidationRuleInfo> list;
			//Get the rules for the property
			if (RulesList.ContainsKey(propertyName))
			{
				list = RulesList[propertyName];
				if (list == null)
					return;

				ValidateRuleList(list);
			}
		}

		/// <summary>
		/// Validate all the rules for all properties.
		/// </summary>
		public void ValidateRules()
		{
			// get the rules for each rule name
			foreach (KeyValuePair<string, List<ValidationRuleInfo>> rulePair in RulesList)
			{
				ValidateRuleList(rulePair.Value);
			}
		}

		#endregion

		#region Validation Status

		/// <summary>
		/// Returns a value indicateing whether the <see cref="Target"/> object is valid.
		/// </summary>
		/// <remarks>If one or more rules are broken, the object is assumed to be invalid and 
		/// false is return.  Otherwise, True is returned.
		/// </remarks>
		/// <returns>A value indicating whether any rules are broken.</returns>
		internal bool IsValid
		{
			get
			{
				return BrokenRulesList.Count == 0;
			}
		}

		/// <summary>
		/// Return a <see cref="BrokenRulesList"/> that contains all of the invalid rules.
		/// </summary>
		public BrokenRulesList GetBrokenRules()
		{
			return BrokenRulesList;
		}

		#endregion

		/// <summary>
		/// 	Clear the rules list.
		/// </summary>
		public void Clear()
		{
			_rulesList.Clear();
		}
	}

	/// <summary>
	/// The base object for each database table entity.
	/// </summary>
	[Serializable]
	public abstract partial class EntityBase : EntityBaseCore
	{

	}
}