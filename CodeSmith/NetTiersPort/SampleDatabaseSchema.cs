using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NetTiersPort;
using NetTiersPort.Data;
using NetTiersPort.EntityLayer;
namespace NetTiersPort.Data.Bases
{
	#region Classes for Address
	/// <summary>
	/// This class is the base class for any <see cref="AddressProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract partial class AddressProviderBase : AddressProviderBaseCore
	{
	}
	/// <summary>
	/// This class is the base class for any <see cref="AddressProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract class AddressProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.Address, NetTiersPort.EntityLayer.AddressKey>
	{
		#region Get from Many To Many Relationship Functions
		#region GetByVendorIdRefFromVendorAddress
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(transactionManager, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(transactionManager, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef, int start, int pageLength, out int count)
		{
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Address objects from the datasource by VendorIDRef in the
		/// 										VendorAddress table. Table Address is related to table Vendor
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength, out int count);
		#endregion // GetByVendorIdRefFromVendorAddress
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager">
		/// 								A <see cref="TransactionManager"/> object.
		/// 							</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.AddressKey key)
		{
			return this.Delete(transactionManager, key.AddressId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="addressId">Primary key</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int addressId)
		{
			return this.Delete(null, addressId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 							</param>
		/// <param name="addressId">Primary key</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(NetTiersPort.Data.TransactionManager transactionManager, int addressId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(stateProvinceId, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="stateProvinceId"/>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId, int start, int pageLength, out int count)
		{
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_StateProvinceID key.
		/// 										FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Address objects.
		/// 									</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength, out int count);
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// 										Gets a row from the DataSource based on its primary key.
		/// 									</summary>
		/// <param name="transactionManager">
		/// 										A <see cref="TransactionManager"/> object.
		/// 									</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override NetTiersPort.EntityLayer.Address Get(TransactionManager transactionManager, AddressKey key, int start, int pageLength)
		{
			return this.GetByAddressId(transactionManager, key.AddressId, start, pageLength);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Address index.
		/// 								</summary>
		/// <param name="addressId">Primary key</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Address GetByAddressId(int addressId)
		{
			int count = -1;
			return this.GetByAddressId(null, addressId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Address index.
		/// 								</summary>
		/// <param name="addressId">Primary key</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Address GetByAddressId(int addressId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressId(null, addressId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Address index.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="addressId">Primary key</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Address GetByAddressId(TransactionManager transactionManager, int addressId)
		{
			int count = -1;
			return this.GetByAddressId(transactionManager, addressId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="addressId">Primary key</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Address GetByAddressId(TransactionManager transactionManager, int addressId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressId(transactionManager, addressId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="addressId">Primary key</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Address GetByAddressId(int addressId, int start, int pageLength, out int count)
		{
			return this.GetByAddressId(null, addressId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="addressId">Primary key</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Address"/> class.
		/// 								</returns>
		public abstract NetTiersPort.EntityLayer.Address GetByAddressId(TransactionManager transactionManager, int addressId, int start, int pageLength, out int count);
		#endregion // Get By Index Functions
		#region Helper Functions
		/// <summary>
		/// 							Fill a <see cref="NetTiersPort.EntityLayer.Address"/> from a DataReader.
		/// 						</summary>
		/// <param name="reader">DataReader</param>
		/// <param name="rows">The collection to fill.</param>
		/// <param name="start">row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows.</param>
		/// <returns>
		/// 							a <see cref="NetTiersPort.EntityLayer.TList<Address>"/>
		/// 						</returns>
		public static NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> Fill(IDataReader reader, int start, int pageLength)
		{
			for (int i = 0; i < start; ++i)
			{
				if (!reader.Read())
				{
					return rows;
				}
			}
			for (int i = 0; i < pageLength; ++i)
			{
				if (!reader.Read())
				{
					break;
				}
				string key = null;
				NetTiersPort.EntityLayer.Address c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = (new System.Text.StringBuilder("Address")).Append(!reader.IsDBNull((int)NetTiersPort.EntityLayer.AddressColumn.AddressId - 1) ? 0 : reader[(int)NetTiersPort.EntityLayer.AddressColumn.AddressId - 1]).ToString();
					c = EntityManager.LocateOrCreate<Address>(key.ToString(), "Address", DataRepository.Provider.EntityCreationalFactoryType, DataRepository.Provider.EnableEntityTracking);
				}
				else
				{
					c = new NetTiersPort.EntityLayer.Address();
				}
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added || DataRepository.Provider.EnableEntityTracking && (DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.PreserveChanges && c.EntityState == EntityState.Unchanged || DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.DiscardChanges && (c.EntityState == EntityState.Unchanged || c.EntityState == EntityState.Changed)))
				{
					c.SuppressEntityEvents = true;
					c.AddressId = (int)reader["AddressId"];
					c.OriginalAddressId = c.AddressId;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.Address"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Address"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(IDataReader reader, NetTiersPort.EntityLayer.Address entity)
		{
			if (!reader.Read())
			{
				return;
			}
			entity.AddressId = (int)reader["AddressId"];
			entity.OriginalAddressId = (int)reader["AddressId"];
			entity.AcceptChanges();
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.Address"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Address"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(System.Data.DataSet dataSet, NetTiersPort.EntityLayer.Address entity)
		{
			System.Data.DataRow dataRow = dataSet.Tables[0].Rows[0];
			entity.AddressId = (int)dataRow["AddressId"];
			entity.OriginalAddressId = (int)dataRow["AddressId"];
			entity.AcceptChanges();
		}
		#endregion // Helper Functions
		#region DeepLoad Methods
		/// <summary>
		/// 							Deep Loads the <see cref="IEntity" /> object with criteria based of the child property collections only N Levels Deep based on the <see cref="DeepLoadType" />.
		/// 						</summary>
		/// <remarks>Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.</remarks>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Address" /> object to load.
		/// 						</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.Address Property Collection Type Array To Include or Exclude from Load
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		/// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
		/// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, NetTiersPort.EntityLayer.Address entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return;
			}
			object[] pkItems;
			#region StateProvinceIDSource
			if (this.CanDeepLoad(entity, "StateProvince|StateProvinceIDSource", deepLoadType, innerList) && entity.StateProvinceIDSource == null)
			{
				pkItems = new object[]{
					entity.StateProvinceID};
				NetTiersPort.EntityLayer.StateProvince tmpEntity = NetTiersPort.EntityLayer.EntityManager.LocateEntity<NetTiersPort.EntityLayer.StateProvince>(NetTiersPort.EntityLayer.EntityLocator.ConstructKeyFromPkItems(typeof(NetTiersPort.EntityLayer.StateProvince), pkItems), NetTiersPort.Data.DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
				{
					entity.StateProvinceIDSource = tmpEntity;
				}
				else
				{
					entity.StateProvinceIDSource = NetTiersPort.Data.DataRepository.StateProvinceProvider.GetByStateProvinceID(transactionManager, entity.StateProvinceID);
				}
				if (deep && entity.StateProvinceIDSource != null)
				{
					innerList.SkipChildren = true;
					NetTiersPort.Data.DataRepository.StateProvinceProvider.DeepLoad(transactionManager, entity.StateProvinceIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
			}
			#endregion // StateProvinceIDSource
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>> deepHandles = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles.Values)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			deepHandles = null;
		}
		#endregion // DeepLoad Methods
		#region DeepSave Methods
		/// <summary>
		/// 							Deep Save the entire object graph of the [CLASS NAME] object with criteria based off the child type property array and DeepSaveType.
		/// 						</summary>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Address" /> instance.
		/// 						</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.Address Property Collection Type Array To Include or Exclude from Save
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		internal override bool DeepSave(NetTiersPort.Data.TransactionManager transactionManager, NetTiersPort.EntityLayer.Address entity, NetTiersPort.Data.DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return false;
			}
			#region Composite Parent Properties
			// Save Source Composite Properties, however, don't call deep save on them.
			// So they only get saved a single level deep.
			#region StateProvinceIDSource
			if (base.CanDeepSave(entity, "StateProvince|StateProvinceIDSource", deepSaveType, innerList) && entity.StateProvinceIDSource != null)
			{
				NetTiersPort.Data.DataRepository.StateProvinceProvider.Save(transactionManager, entity.StateProvinceIDSource);
				entity.StateProvinceID = entity.StateProvinceIDSource.StateProvinceID;
			}
			#endregion // StateProvinceIDSource
			System.Collections.Generic.Dictionary<System.Delegate, object> deepHandles = new System.Collections.Generic.Dictionary<System.Delegate, object>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			if (entity.IsDeleted)
			{
				this.Save(transactionManager, entity);
			}
			deepHandles = null;
			return true;
			#endregion // Composite Parent Properties
		}
		#endregion // DeepSave Methods
	}
	#endregion // Classes for Address
	#region AddressChildEntityTypes
	/// <summary>
	///              Enumeration used to expose the different child entity types 
	///              for child properties in <c>NetTiersPort.EntityLayer.Address</c>
	///             </summary>
	public enum AddressChildEntityTypes
	{
		/// <summary>
		///              Composite Property for <c>StateProvince</c> at StateProvinceIDSource
		///             </summary>
		[ChildEntityType(typeof(NetTiersPort.EntityLayer.StateProvince))]
		StateProvince,
	}
	#endregion // AddressChildEntityTypes
	#region Classes for Vendor
	/// <summary>
	/// This class is the base class for any <see cref="VendorProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract partial class VendorProviderBase : VendorProviderBaseCore
	{
	}
	/// <summary>
	/// This class is the base class for any <see cref="VendorProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract class VendorProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.Vendor, NetTiersPort.EntityLayer.VendorKey>
	{
		#region Get from Many To Many Relationship Functions
		#region GetByAddressIdRefFromVendorAddress
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(transactionManager, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(transactionManager, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef, int start, int pageLength, out int count)
		{
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets Vendor objects from the datasource by AddressIDRef in the
		/// 										VendorAddress table. Table Vendor is related to table Address
		/// 										through the (M:N) relationship defined in the VendorAddress table.
		/// 									</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 											</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of Vendor objects.
		/// 									</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef, int start, int pageLength, out int count);
		#endregion // GetByAddressIdRefFromVendorAddress
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager">
		/// 								A <see cref="TransactionManager"/> object.
		/// 							</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorKey key)
		{
			return this.Delete(transactionManager, key.VendorId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="vendorId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int vendorId)
		{
			return this.Delete(null, vendorId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 							</param>
		/// <param name="vendorId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(NetTiersPort.Data.TransactionManager transactionManager, int vendorId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// 										Gets a row from the DataSource based on its primary key.
		/// 									</summary>
		/// <param name="transactionManager">
		/// 										A <see cref="TransactionManager"/> object.
		/// 									</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override NetTiersPort.EntityLayer.Vendor Get(TransactionManager transactionManager, VendorKey key, int start, int pageLength)
		{
			return this.GetByVendorId(transactionManager, key.VendorId, start, pageLength);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Vendor index.
		/// 								</summary>
		/// <param name="vendorId"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Vendor GetByVendorId(int vendorId)
		{
			int count = -1;
			return this.GetByVendorId(null, vendorId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Vendor index.
		/// 								</summary>
		/// <param name="vendorId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Vendor GetByVendorId(int vendorId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorId(null, vendorId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_Vendor index.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorId"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Vendor GetByVendorId(TransactionManager transactionManager, int vendorId)
		{
			int count = -1;
			return this.GetByVendorId(transactionManager, vendorId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Vendor GetByVendorId(TransactionManager transactionManager, int vendorId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorId(transactionManager, vendorId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="vendorId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.Vendor GetByVendorId(int vendorId, int start, int pageLength, out int count)
		{
			return this.GetByVendorId(null, vendorId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.Vendor"/> class.
		/// 								</returns>
		public abstract NetTiersPort.EntityLayer.Vendor GetByVendorId(TransactionManager transactionManager, int vendorId, int start, int pageLength, out int count);
		#endregion // Get By Index Functions
		#region Helper Functions
		/// <summary>
		/// 							Fill a <see cref="NetTiersPort.EntityLayer.Vendor"/> from a DataReader.
		/// 						</summary>
		/// <param name="reader">DataReader</param>
		/// <param name="rows">The collection to fill.</param>
		/// <param name="start">row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows.</param>
		/// <returns>
		/// 							a <see cref="NetTiersPort.EntityLayer.TList<Vendor>"/>
		/// 						</returns>
		public static NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> Fill(IDataReader reader, int start, int pageLength)
		{
			for (int i = 0; i < start; ++i)
			{
				if (!reader.Read())
				{
					return rows;
				}
			}
			for (int i = 0; i < pageLength; ++i)
			{
				if (!reader.Read())
				{
					break;
				}
				string key = null;
				NetTiersPort.EntityLayer.Vendor c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = (new System.Text.StringBuilder("Vendor")).Append(!reader.IsDBNull((int)NetTiersPort.EntityLayer.VendorColumn.VendorId - 1) ? 0 : reader[(int)NetTiersPort.EntityLayer.VendorColumn.VendorId - 1]).ToString();
					c = EntityManager.LocateOrCreate<Vendor>(key.ToString(), "Vendor", DataRepository.Provider.EntityCreationalFactoryType, DataRepository.Provider.EnableEntityTracking);
				}
				else
				{
					c = new NetTiersPort.EntityLayer.Vendor();
				}
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added || DataRepository.Provider.EnableEntityTracking && (DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.PreserveChanges && c.EntityState == EntityState.Unchanged || DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.DiscardChanges && (c.EntityState == EntityState.Unchanged || c.EntityState == EntityState.Changed)))
				{
					c.SuppressEntityEvents = true;
					c.VendorId = (int)reader["VendorId"];
					c.OriginalVendorId = c.VendorId;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.Vendor"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Vendor"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(IDataReader reader, NetTiersPort.EntityLayer.Vendor entity)
		{
			if (!reader.Read())
			{
				return;
			}
			entity.VendorId = (int)reader["VendorId"];
			entity.OriginalVendorId = (int)reader["VendorId"];
			entity.AcceptChanges();
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.Vendor"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Vendor"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(System.Data.DataSet dataSet, NetTiersPort.EntityLayer.Vendor entity)
		{
			System.Data.DataRow dataRow = dataSet.Tables[0].Rows[0];
			entity.VendorId = (int)dataRow["VendorId"];
			entity.OriginalVendorId = (int)dataRow["VendorId"];
			entity.AcceptChanges();
		}
		#endregion // Helper Functions
		#region DeepLoad Methods
		/// <summary>
		/// 							Deep Loads the <see cref="IEntity" /> object with criteria based of the child property collections only N Levels Deep based on the <see cref="DeepLoadType" />.
		/// 						</summary>
		/// <remarks>Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.</remarks>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Vendor" /> object to load.
		/// 						</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.Vendor Property Collection Type Array To Include or Exclude from Load
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		/// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
		/// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, NetTiersPort.EntityLayer.Vendor entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return;
			}
			object[] pkItems;
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>> deepHandles = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles.Values)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			deepHandles = null;
		}
		#endregion // DeepLoad Methods
		#region DeepSave Methods
		/// <summary>
		/// 							Deep Save the entire object graph of the [CLASS NAME] object with criteria based off the child type property array and DeepSaveType.
		/// 						</summary>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.Vendor" /> instance.
		/// 						</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.Vendor Property Collection Type Array To Include or Exclude from Save
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		internal override bool DeepSave(NetTiersPort.Data.TransactionManager transactionManager, NetTiersPort.EntityLayer.Vendor entity, NetTiersPort.Data.DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return false;
			}
			#region Composite Parent Properties
			// Save Source Composite Properties, however, don't call deep save on them.
			// So they only get saved a single level deep.
			System.Collections.Generic.Dictionary<System.Delegate, object> deepHandles = new System.Collections.Generic.Dictionary<System.Delegate, object>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			if (entity.IsDeleted)
			{
				this.Save(transactionManager, entity);
			}
			deepHandles = null;
			return true;
			#endregion // Composite Parent Properties
		}
		#endregion // DeepSave Methods
	}
	#endregion // Classes for Vendor
	#region VendorChildEntityTypes
	/// <summary>
	///              Enumeration used to expose the different child entity types 
	///              for child properties in <c>NetTiersPort.EntityLayer.Vendor</c>
	///             </summary>
	public enum VendorChildEntityTypes
	{
	}
	#endregion // VendorChildEntityTypes
	#region Classes for VendorAddress
	/// <summary>
	/// This class is the base class for any <see cref="VendorAddressProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract partial class VendorAddressProviderBase : VendorAddressProviderBaseCore
	{
	}
	/// <summary>
	/// This class is the base class for any <see cref="VendorAddressProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract class VendorAddressProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.VendorAddress, NetTiersPort.EntityLayer.VendorAddressKey>
	{
		#region Get from Many To Many Relationship Functions
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager">
		/// 								A <see cref="TransactionManager"/> object.
		/// 							</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorAddressKey key)
		{
			return this.Delete(transactionManager, key.VendorIdRef, key.AddressIdRef);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int vendorIdRef, int addressIdRef)
		{
			return this.Delete(null, vendorIdRef, addressIdRef);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 							</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(NetTiersPort.Data.TransactionManager transactionManager, int vendorIdRef, int addressIdRef);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRef(addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRef(transactionManager, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRef(transactionManager, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRef(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef, int start, int pageLength, out int count)
		{
			return this.GetByAddressIdRef(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_AddressID key.
		/// 										FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef, int start, int pageLength, out int count);
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRef(vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRef(transactionManager, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRef(transactionManager, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRef(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef, int start, int pageLength, out int count)
		{
			return this.GetByVendorIdRef(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 										Gets rows from the datasource based on the FK_VendorID key.
		/// 										FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 									</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>
		/// 										Returns a typed collection of VendorAddress objects.
		/// 									</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength, out int count);
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// 										Gets a row from the DataSource based on its primary key.
		/// 									</summary>
		/// <param name="transactionManager">
		/// 										A <see cref="TransactionManager"/> object.
		/// 									</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override NetTiersPort.EntityLayer.VendorAddress Get(TransactionManager transactionManager, VendorAddressKey key, int start, int pageLength)
		{
			return this.GetByVendorIdRefAddressIdRef(transactionManager, key.VendorIdRef, key.AddressIdRef, start, pageLength);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_VendorAddress index.
		/// 								</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(int vendorIdRef, int addressIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefAddressIdRef(null, vendorIdRef, addressIdRef, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_VendorAddress index.
		/// 								</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(int vendorIdRef, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefAddressIdRef(null, vendorIdRef, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key PK_VendorAddress index.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(TransactionManager transactionManager, int vendorIdRef, int addressIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefAddressIdRef(transactionManager, vendorIdRef, addressIdRef, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(TransactionManager transactionManager, int vendorIdRef, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefAddressIdRef(transactionManager, vendorIdRef, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(int vendorIdRef, int addressIdRef, int start, int pageLength, out int count)
		{
			return this.GetByVendorIdRefAddressIdRef(null, vendorIdRef, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> class.
		/// 								</returns>
		public abstract NetTiersPort.EntityLayer.VendorAddress GetByVendorIdRefAddressIdRef(TransactionManager transactionManager, int vendorIdRef, int addressIdRef, int start, int pageLength, out int count);
		#endregion // Get By Index Functions
		#region Helper Functions
		/// <summary>
		/// 							Fill a <see cref="NetTiersPort.EntityLayer.VendorAddress"/> from a DataReader.
		/// 						</summary>
		/// <param name="reader">DataReader</param>
		/// <param name="rows">The collection to fill.</param>
		/// <param name="start">row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows.</param>
		/// <returns>
		/// 							a <see cref="NetTiersPort.EntityLayer.TList<VendorAddress>"/>
		/// 						</returns>
		public static NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> Fill(IDataReader reader, int start, int pageLength)
		{
			for (int i = 0; i < start; ++i)
			{
				if (!reader.Read())
				{
					return rows;
				}
			}
			for (int i = 0; i < pageLength; ++i)
			{
				if (!reader.Read())
				{
					break;
				}
				string key = null;
				NetTiersPort.EntityLayer.VendorAddress c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = (new System.Text.StringBuilder("VendorAddress")).Append(!reader.IsDBNull((int)NetTiersPort.EntityLayer.VendorAddressColumn.AddressIdRef - 1) ? 0 : reader[(int)NetTiersPort.EntityLayer.VendorAddressColumn.AddressIdRef - 1]).Append(!reader.IsDBNull((int)NetTiersPort.EntityLayer.VendorAddressColumn.VendorIdRef - 1) ? 0 : reader[(int)NetTiersPort.EntityLayer.VendorAddressColumn.VendorIdRef - 1]).ToString();
					c = EntityManager.LocateOrCreate<VendorAddress>(key.ToString(), "VendorAddress", DataRepository.Provider.EntityCreationalFactoryType, DataRepository.Provider.EnableEntityTracking);
				}
				else
				{
					c = new NetTiersPort.EntityLayer.VendorAddress();
				}
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added || DataRepository.Provider.EnableEntityTracking && (DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.PreserveChanges && c.EntityState == EntityState.Unchanged || DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.DiscardChanges && (c.EntityState == EntityState.Unchanged || c.EntityState == EntityState.Changed)))
				{
					c.SuppressEntityEvents = true;
					c.VendorIdRef = (int)reader["VendorIdRef"];
					c.OriginalVendorIdRef = c.VendorIdRef;
					c.AddressIdRef = (int)reader["AddressIdRef"];
					c.OriginalAddressIdRef = c.AddressIdRef;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.VendorAddress"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(IDataReader reader, NetTiersPort.EntityLayer.VendorAddress entity)
		{
			if (!reader.Read())
			{
				return;
			}
			entity.VendorIdRef = (int)reader["VendorIdRef"];
			entity.OriginalVendorIdRef = (int)reader["VendorIdRef"];
			entity.AddressIdRef = (int)reader["AddressIdRef"];
			entity.OriginalAddressIdRef = (int)reader["AddressIdRef"];
			entity.AcceptChanges();
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.VendorAddress"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.VendorAddress"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(System.Data.DataSet dataSet, NetTiersPort.EntityLayer.VendorAddress entity)
		{
			System.Data.DataRow dataRow = dataSet.Tables[0].Rows[0];
			entity.VendorIdRef = (int)dataRow["VendorIdRef"];
			entity.OriginalVendorIdRef = (int)dataRow["VendorIdRef"];
			entity.AddressIdRef = (int)dataRow["AddressIdRef"];
			entity.OriginalAddressIdRef = (int)dataRow["AddressIdRef"];
			entity.AcceptChanges();
		}
		#endregion // Helper Functions
		#region DeepLoad Methods
		/// <summary>
		/// 							Deep Loads the <see cref="IEntity" /> object with criteria based of the child property collections only N Levels Deep based on the <see cref="DeepLoadType" />.
		/// 						</summary>
		/// <remarks>Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.</remarks>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.VendorAddress" /> object to load.
		/// 						</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.VendorAddress Property Collection Type Array To Include or Exclude from Load
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		/// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
		/// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorAddress entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return;
			}
			object[] pkItems;
			#region AddressIDSource
			if (this.CanDeepLoad(entity, "Address|AddressIDSource", deepLoadType, innerList) && entity.AddressIDSource == null)
			{
				pkItems = new object[]{
					entity.AddressID};
				NetTiersPort.EntityLayer.Address tmpEntity = NetTiersPort.EntityLayer.EntityManager.LocateEntity<NetTiersPort.EntityLayer.Address>(NetTiersPort.EntityLayer.EntityLocator.ConstructKeyFromPkItems(typeof(NetTiersPort.EntityLayer.Address), pkItems), NetTiersPort.Data.DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
				{
					entity.AddressIDSource = tmpEntity;
				}
				else
				{
					entity.AddressIDSource = NetTiersPort.Data.DataRepository.AddressProvider.GetByAddressID(transactionManager, entity.AddressID);
				}
				if (deep && entity.AddressIDSource != null)
				{
					innerList.SkipChildren = true;
					NetTiersPort.Data.DataRepository.AddressProvider.DeepLoad(transactionManager, entity.AddressIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
			}
			#endregion // AddressIDSource
			#region VendorIDSource
			if (this.CanDeepLoad(entity, "Vendor|VendorIDSource", deepLoadType, innerList) && entity.VendorIDSource == null)
			{
				pkItems = new object[]{
					entity.VendorID};
				NetTiersPort.EntityLayer.Vendor tmpEntity = NetTiersPort.EntityLayer.EntityManager.LocateEntity<NetTiersPort.EntityLayer.Vendor>(NetTiersPort.EntityLayer.EntityLocator.ConstructKeyFromPkItems(typeof(NetTiersPort.EntityLayer.Vendor), pkItems), NetTiersPort.Data.DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
				{
					entity.VendorIDSource = tmpEntity;
				}
				else
				{
					entity.VendorIDSource = NetTiersPort.Data.DataRepository.VendorProvider.GetByVendorID(transactionManager, entity.VendorID);
				}
				if (deep && entity.VendorIDSource != null)
				{
					innerList.SkipChildren = true;
					NetTiersPort.Data.DataRepository.VendorProvider.DeepLoad(transactionManager, entity.VendorIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
			}
			#endregion // VendorIDSource
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>> deepHandles = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles.Values)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			deepHandles = null;
		}
		#endregion // DeepLoad Methods
		#region DeepSave Methods
		/// <summary>
		/// 							Deep Save the entire object graph of the [CLASS NAME] object with criteria based off the child type property array and DeepSaveType.
		/// 						</summary>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.VendorAddress" /> instance.
		/// 						</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.VendorAddress Property Collection Type Array To Include or Exclude from Save
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		internal override bool DeepSave(NetTiersPort.Data.TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorAddress entity, NetTiersPort.Data.DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return false;
			}
			#region Composite Parent Properties
			// Save Source Composite Properties, however, don't call deep save on them.
			// So they only get saved a single level deep.
			#region AddressIDSource
			if (base.CanDeepSave(entity, "Address|AddressIDSource", deepSaveType, innerList) && entity.AddressIDSource != null)
			{
				NetTiersPort.Data.DataRepository.AddressProvider.Save(transactionManager, entity.AddressIDSource);
				entity.AddressID = entity.AddressIDSource.AddressID;
			}
			#endregion // AddressIDSource
			#region VendorIDSource
			if (base.CanDeepSave(entity, "Vendor|VendorIDSource", deepSaveType, innerList) && entity.VendorIDSource != null)
			{
				NetTiersPort.Data.DataRepository.VendorProvider.Save(transactionManager, entity.VendorIDSource);
				entity.VendorID = entity.VendorIDSource.VendorID;
			}
			#endregion // VendorIDSource
			System.Collections.Generic.Dictionary<System.Delegate, object> deepHandles = new System.Collections.Generic.Dictionary<System.Delegate, object>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			if (entity.IsDeleted)
			{
				this.Save(transactionManager, entity);
			}
			deepHandles = null;
			return true;
			#endregion // Composite Parent Properties
		}
		#endregion // DeepSave Methods
	}
	#endregion // Classes for VendorAddress
	#region VendorAddressChildEntityTypes
	/// <summary>
	///              Enumeration used to expose the different child entity types 
	///              for child properties in <c>NetTiersPort.EntityLayer.VendorAddress</c>
	///             </summary>
	public enum VendorAddressChildEntityTypes
	{
		/// <summary>
		///              Composite Property for <c>Address</c> at AddressIDSource
		///             </summary>
		[ChildEntityType(typeof(NetTiersPort.EntityLayer.Address))]
		Address,
		/// <summary>
		///              Composite Property for <c>Vendor</c> at VendorIDSource
		///             </summary>
		[ChildEntityType(typeof(NetTiersPort.EntityLayer.Vendor))]
		Vendor,
	}
	#endregion // VendorAddressChildEntityTypes
	#region Classes for StateProvince
	/// <summary>
	/// This class is the base class for any <see cref="StateProvinceProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract partial class StateProvinceProviderBase : StateProvinceProviderBaseCore
	{
	}
	/// <summary>
	/// This class is the base class for any <see cref="StateProvinceProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	/// </summary>
	public abstract class StateProvinceProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.StateProvince, NetTiersPort.EntityLayer.StateProvinceKey>
	{
		#region Get from Many To Many Relationship Functions
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager">
		/// 								A <see cref="TransactionManager"/> object.
		/// 							</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.StateProvinceKey key)
		{
			return this.Delete(transactionManager, key.StateProvinceId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="stateProvinceId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int stateProvinceId)
		{
			return this.Delete(null, stateProvinceId);
		}
		/// <summary>
		/// 								Deletes a row from the DataSource.
		/// 							</summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object
		/// 							</param>
		/// <param name="stateProvinceId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(NetTiersPort.Data.TransactionManager transactionManager, int stateProvinceId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// 										Gets a row from the DataSource based on its primary key.
		/// 									</summary>
		/// <param name="transactionManager">
		/// 										A <see cref="TransactionManager"/> object.
		/// 									</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override NetTiersPort.EntityLayer.StateProvince Get(TransactionManager transactionManager, StateProvinceKey key, int start, int pageLength)
		{
			return this.GetByStateProvinceId(transactionManager, key.StateProvinceId, start, pageLength);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key StateProvince index.
		/// 								</summary>
		/// <param name="stateProvinceId"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(null, stateProvinceId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key StateProvince index.
		/// 								</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets rows from the datasource based on the primary key StateProvince index.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="stateProvinceId"/>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, 0, int.MaxValue(), out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(int stateProvinceId, int start, int pageLength, out int count)
		{
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// 									Gets a row from the DataSource based on its primary key.
		/// 								</summary>
		/// <param name="transactionManager">
		/// 									A <see cref="TransactionManager"/> object.
		/// 								</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>
		/// 									Returns an instance of the <see cref="NetTiersPort.EntityLayer.StateProvince"/> class.
		/// 								</returns>
		public abstract NetTiersPort.EntityLayer.StateProvince GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength, out int count);
		#endregion // Get By Index Functions
		#region Helper Functions
		/// <summary>
		/// 							Fill a <see cref="NetTiersPort.EntityLayer.StateProvince"/> from a DataReader.
		/// 						</summary>
		/// <param name="reader">DataReader</param>
		/// <param name="rows">The collection to fill.</param>
		/// <param name="start">row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows.</param>
		/// <returns>
		/// 							a <see cref="NetTiersPort.EntityLayer.TList<StateProvince>"/>
		/// 						</returns>
		public static NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.StateProvince> Fill(IDataReader reader, int start, int pageLength)
		{
			for (int i = 0; i < start; ++i)
			{
				if (!reader.Read())
				{
					return rows;
				}
			}
			for (int i = 0; i < pageLength; ++i)
			{
				if (!reader.Read())
				{
					break;
				}
				string key = null;
				NetTiersPort.EntityLayer.StateProvince c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = (new System.Text.StringBuilder("StateProvince")).Append(!reader.IsDBNull((int)NetTiersPort.EntityLayer.StateProvinceColumn.StateProvinceId - 1) ? 0 : reader[(int)NetTiersPort.EntityLayer.StateProvinceColumn.StateProvinceId - 1]).ToString();
					c = EntityManager.LocateOrCreate<StateProvince>(key.ToString(), "StateProvince", DataRepository.Provider.EntityCreationalFactoryType, DataRepository.Provider.EnableEntityTracking);
				}
				else
				{
					c = new NetTiersPort.EntityLayer.StateProvince();
				}
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added || DataRepository.Provider.EnableEntityTracking && (DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.PreserveChanges && c.EntityState == EntityState.Unchanged || DataRepository.Provider.CurrentLoadPolicy == LoadPolicy.DiscardChanges && (c.EntityState == EntityState.Unchanged || c.EntityState == EntityState.Changed)))
				{
					c.SuppressEntityEvents = true;
					c.StateProvinceId = (int)reader["StateProvinceId"];
					c.OriginalStateProvinceId = c.StateProvinceId;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.StateProvince"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.StateProvince"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(IDataReader reader, NetTiersPort.EntityLayer.StateProvince entity)
		{
			if (!reader.Read())
			{
				return;
			}
			entity.StateProvinceId = (int)reader["StateProvinceId"];
			entity.OriginalStateProvinceId = (int)reader["StateProvinceId"];
			entity.AcceptChanges();
		}
		/// <summary>
		/// 							Refreshes the <see cref="NetTiersPort.EntityLayer.StateProvince"/> object from the <see cref="IDataReader"/>.
		/// 						</summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.StateProvince"/> object to refresh.
		/// 						</param>
		public static void RefreshEntity(System.Data.DataSet dataSet, NetTiersPort.EntityLayer.StateProvince entity)
		{
			System.Data.DataRow dataRow = dataSet.Tables[0].Rows[0];
			entity.StateProvinceId = (int)dataRow["StateProvinceId"];
			entity.OriginalStateProvinceId = (int)dataRow["StateProvinceId"];
			entity.AcceptChanges();
		}
		#endregion // Helper Functions
		#region DeepLoad Methods
		/// <summary>
		/// 							Deep Loads the <see cref="IEntity" /> object with criteria based of the child property collections only N Levels Deep based on the <see cref="DeepLoadType" />.
		/// 						</summary>
		/// <remarks>Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.</remarks>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.StateProvince" /> object to load.
		/// 						</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.StateProvince Property Collection Type Array To Include or Exclude from Load
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		/// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
		/// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, NetTiersPort.EntityLayer.StateProvince entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return;
			}
			object[] pkItems;
			System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>> deepHandles = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.KeyValuePair<System.Delegate, object>>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles.Values)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			deepHandles = null;
		}
		#endregion // DeepLoad Methods
		#region DeepSave Methods
		/// <summary>
		/// 							Deep Save the entire object graph of the [CLASS NAME] object with criteria based off the child type property array and DeepSaveType.
		/// 						</summary>
		/// <param name="transactionManager"><see cref="TransactionManager" /> object</param>
		/// <param name="entity">
		/// 							The <see cref="NetTiersPort.EntityLayer.StateProvince" /> instance.
		/// 						</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">NetTiersPort.EntityLayer.StateProvince Property Collection Type Array To Include or Exclude from Save
		/// 						</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
		internal override bool DeepSave(NetTiersPort.Data.TransactionManager transactionManager, NetTiersPort.EntityLayer.StateProvince entity, NetTiersPort.Data.DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{
			if (entity == null)
			{
				return false;
			}
			#region Composite Parent Properties
			// Save Source Composite Properties, however, don't call deep save on them.
			// So they only get saved a single level deep.
			System.Collections.Generic.Dictionary<System.Delegate, object> deepHandles = new System.Collections.Generic.Dictionary<System.Delegate, object>();
			foreach (System.Collections.Generic.KeyValuePair<System.Delegate, object> pair in deepHandles)
			{
				pair.Key.DynamicInvoke((object[])pair.Value);
			}
			if (entity.IsDeleted)
			{
				this.Save(transactionManager, entity);
			}
			deepHandles = null;
			return true;
			#endregion // Composite Parent Properties
		}
		#endregion // DeepSave Methods
	}
	#endregion // Classes for StateProvince
	#region StateProvinceChildEntityTypes
	/// <summary>
	///              Enumeration used to expose the different child entity types 
	///              for child properties in <c>NetTiersPort.EntityLayer.StateProvince</c>
	///             </summary>
	public enum StateProvinceChildEntityTypes
	{
	}
	#endregion // StateProvinceChildEntityTypes
}
