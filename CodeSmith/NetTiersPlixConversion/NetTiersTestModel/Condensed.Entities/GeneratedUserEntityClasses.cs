using System;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace NetTiersTestModel.Entities
{

	public abstract partial class EntityBase : EntityBaseCore
	{
	}

	/// <summary>
	/// The base object for each database table's unique identifier.
	/// </summary>
	/// <remarks>
	/// This file is generated once and will never be overwritten.
	/// </remarks>
	[Serializable]
	public abstract partial class EntityKeyBase : EntityKeyBaseCore
	{

	}

	///<summary>
	/// An object representation of the 'Item' table. [No description found the database]	
	///</summary>
	/// <remarks>
	/// This file is generated once and will never be overwritten.
	/// </remarks>	
	[Serializable]
	public partial class Item : ItemBase
	{
		#region Constructors

		///<summary>
		/// Creates a new <see cref="Item"/> instance.
		///</summary>
		public Item() : base()
		{
		}

		#endregion
	}

	///<summary>
	/// An object representation of the '"Order"' table. [No description found the database]	
	///</summary>
	/// <remarks>
	/// This file is generated once and will never be overwritten.
	/// </remarks>	
	[Serializable]
	public partial class Order : OrderBase
	{
		#region Constructors

		///<summary>
		/// Creates a new <see cref="Order"/> instance.
		///</summary>
		public Order() : base()
		{
		}

		#endregion
	}
}
