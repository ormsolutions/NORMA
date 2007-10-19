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
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(transactionManager, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRefFromVendorAddress(transactionManager, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(int vendorIdRef, int start, int pageLength, out int count)
		{
			return this.GetByVendorIdRefFromVendorAddress(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Address objects from the datasource by VendorIDRef in the
		/// VendorAddress table. Table Address is related to table Vendor
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByVendorIdRefFromVendorAddress(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength, out int count);
		#endregion // GetByVendorIdRefFromVendorAddress
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.AddressKey key)
		{
			return this.Delete(transactionManager, key.AddressId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="addressId">Primary key</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int addressId)
		{
			return this.Delete(null, addressId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressId">Primary key</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(Tiers.AdventureWorks.Data.TransactionManager transactionManager, int addressId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(stateProvinceId, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="stateProvinceId"/>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(transactionManager, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId, int start, int pageLength)
		{
			int count = -1;
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(int stateProvinceId, int start, int pageLength, out int count)
		{
			return this.GetByStateProvinceId(null, stateProvinceId, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_StateProvinceID key.
		/// FK_StateProvinceID Description: Foreign key constraint referencing StateProvince.StateProvinceID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="stateProvinceId"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Address objects.</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Address> GetByStateProvinceId(TransactionManager transactionManager, int stateProvinceId, int start, int pageLength, out int count);
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// Gets a row from the DataSource based on its primary key.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override void Get(TransactionManager transactionManager, AddressKey key, int start, int pageLength)
		{
			return this.GetBy(transactionManager, key.AddressId, start, pageLength);
		}
		#endregion // Get By Index Functions
	}
	#endregion // Classes for Address
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
	public abstract class AddressProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.Vendor, NetTiersPort.EntityLayer.VendorKey>
	{
		#region Get from Many To Many Relationship Functions
		#region GetByAddressIdRefFromVendorAddress
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="addressIdRef"/>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(transactionManager, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRefFromVendorAddress(transactionManager, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(int addressIdRef, int start, int pageLength, out int count)
		{
			return this.GetByAddressIdRefFromVendorAddress(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets Vendor objects from the datasource by AddressIDRef in the
		/// VendorAddress table. Table Vendor is related to table Address
		/// through the (M:N) relationship defined in the VendorAddress table.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of Vendor objects.</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.Vendor> GetByAddressIdRefFromVendorAddress(TransactionManager transactionManager, int addressIdRef, int start, int pageLength, out int count);
		#endregion // GetByAddressIdRefFromVendorAddress
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorKey key)
		{
			return this.Delete(transactionManager, key.VendorId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="vendorId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int vendorId)
		{
			return this.Delete(null, vendorId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(Tiers.AdventureWorks.Data.TransactionManager transactionManager, int vendorId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// Gets a row from the DataSource based on its primary key.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override void Get(TransactionManager transactionManager, VendorKey key, int start, int pageLength)
		{
			return this.GetBy(transactionManager, key.VendorId, start, pageLength);
		}
		#endregion // Get By Index Functions
	}
	#endregion // Classes for Vendor
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
	public abstract class AddressProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.VendorAddress, NetTiersPort.EntityLayer.VendorAddressKey>
	{
		#region Get from Many To Many Relationship Functions
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.VendorAddressKey key)
		{
			return this.Delete(transactionManager, key.VendorIdRef, key.AddressIdRef);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int vendorIdRef, int addressIdRef)
		{
			return this.Delete(null, vendorIdRef, addressIdRef);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="addressIdRef"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(Tiers.AdventureWorks.Data.TransactionManager transactionManager, int vendorIdRef, int addressIdRef);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRef(addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef)
		{
			int count = -1;
			return this.GetByAddressIdRef(transactionManager, addressIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRef(transactionManager, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByAddressIdRef(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(int addressIdRef, int start, int pageLength, out int count)
		{
			return this.GetByAddressIdRef(null, addressIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_AddressID key.
		/// FK_AddressID Description: Foreign key constraint referencing Address.AddressID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="addressIdRef"/>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByAddressIdRef(TransactionManager transactionManager, int addressIdRef, int start, int pageLength, out int count);
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRef(vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef)
		{
			int count = -1;
			return this.GetByVendorIdRef(transactionManager, vendorIdRef, 0, int.MaxValue, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRef(transactionManager, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef, int start, int pageLength)
		{
			int count = -1;
			return this.GetByVendorIdRef(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(int vendorIdRef, int start, int pageLength, out int count)
		{
			return this.GetByVendorIdRef(null, vendorIdRef, start, pageLength, out count);
		}
		/// <summary>
		/// Gets rows from the datasource based on the FK_VendorID key.
		/// FK_VendorID Description: Foreign key constraint referencing Vendor.VendorID</summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="vendorIdRef">Primary key. Foreign key to Vendor.VendorID.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <returns>Returns a typed collection of VendorAddress objects.</returns>
		public abstract NetTiersPort.EntityLayer.TList<NetTiersPort.EntityLayer.VendorAddress> GetByVendorIdRef(TransactionManager transactionManager, int vendorIdRef, int start, int pageLength, out int count);
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// Gets a row from the DataSource based on its primary key.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override void Get(TransactionManager transactionManager, VendorAddressKey key, int start, int pageLength)
		{
			return this.GetBy(transactionManager, key.VendorIdRef, key.AddressIdRef, start, pageLength);
		}
		#endregion // Get By Index Functions
	}
	#endregion // Classes for VendorAddress
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
	public abstract class AddressProviderBaseCore : EntityProviderBase<NetTiersPort.EntityLayer.StateProvince, NetTiersPort.EntityLayer.StateProvinceKey>
	{
		#region Get from Many To Many Relationship Functions
		#endregion // Get from Many To Many Relationship Functions
		#region Delete Methods
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, NetTiersPort.EntityLayer.StateProvinceKey key)
		{
			return this.Delete(transactionManager, key.StateProvinceId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="stateProvinceId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(int stateProvinceId)
		{
			return this.Delete(null, stateProvinceId);
		}
		/// <summary>
		/// Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">
		///               <see cref="TransactionManager"/> object</param>
		/// <param name="stateProvinceId"/>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(Tiers.AdventureWorks.Data.TransactionManager transactionManager, int stateProvinceId);
		#endregion // Delete Methods
		#region Get By Foreign Key Functions
		#endregion // Get By Foreign Key Functions
		#region Get By Index Functions
		/// <summary>
		/// Gets a row from the DataSource based on its primary key.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override void Get(TransactionManager transactionManager, StateProvinceKey key, int start, int pageLength)
		{
			return this.GetBy(transactionManager, key.StateProvinceId, start, pageLength);
		}
		#endregion // Get By Index Functions
	}
	#endregion // Classes for StateProvince
}
