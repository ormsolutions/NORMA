#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase
{
	partial class Column
	{
		#region Custom Storage handlers
		private string GetEditNameValue()
		{
			return Name;
		}
		private void SetEditNameValue(string value)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				if (string.IsNullOrEmpty(value))
				{
					// Name generators should listen to this property to regenerate
					// the name.
					CustomName = false;
				}
				else
				{
					CustomName = true;
					Name = value;
				}
			}
		}
		#endregion // Custom Storage handlers
		#region Helper methods
		/// <summary>
		/// Get the <see cref="ColumnValueType">ValueType</see> associated with this <see cref="Column"/>
		/// </summary>
		public ColumnValueType? AssociatedValueType
		{
			get
			{
				IFrameworkServices services;
				IProvideMappedColumnInfo[] mappers;
				int mapperCount;
				if (null != (services = this.Store as IFrameworkServices) &&
					0 != (mapperCount = (mappers = services.GetTypedDomainModelProviders<IProvideMappedColumnInfo>(true)).Length))
				{
					for (int i = mapperCount - 1; i >= 0; --i) // Reverse dependency order, allow implementation overrides
					{
						IMappedColumnInfo columnInfo = mappers[i].GetColumnInfo(this);
						if (columnInfo != null)
						{
							return columnInfo.ValueType;
						}
					}
				}
				return null;
			}
		}
		#endregion // Helper methods
	}
	#region IProvideMappedColumnInfo interface
	/// <summary>
	/// A structure defining the backing ORM type for a column.
	/// This will represent either a value type or special values for
	/// unary boolean (true/false or true only) columns.
	/// </summary>
	public struct ColumnValueType : IEquatable<ColumnValueType>
	{
		// If neither of these are set then this is a true/false boolean
		private readonly ObjectType myValueType;
		private readonly bool myTrueOnlyBoolean;
		/// <summary>
		/// Create a ColumnValueType structure for a value type
		/// </summary>
		/// <param name="valueType">Create as an object type.</param>
		public ColumnValueType(ObjectType valueType)
		{
			myValueType = valueType;
			myTrueOnlyBoolean = false;
		}
		/// <summary>
		/// Create a ColumnValueType structure for a boolean type
		/// </summary>
		/// <param name="trueOnly">This represents a true-only boolean.</param>
		public ColumnValueType(bool trueOnly)
		{
			myValueType = null;
			myTrueOnlyBoolean = trueOnly;
		}
		/// <summary>
		/// The value type, if set.
		/// </summary>
		public ObjectType ValueType
		{
			get
			{
				return myValueType;
			}
		}
		/// <summary>
		/// Returns true if this is a true/false boolean
		/// </summary>
		public bool IsTrueFalseBoolean
		{
			get
			{
				return myValueType == null && !myTrueOnlyBoolean;
			}
		}
		/// <summary>
		/// Returns true if this is a true-only boolean
		/// </summary>
		public bool IsTrueOnlyBoolean
		{
			get
			{
				return myTrueOnlyBoolean;
			}
		}
		#region Equality and casting routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is ColumnValueType)
			{
				return Equals((ColumnValueType)obj);
			}
			return false;
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			ObjectType valueType = myValueType;
			if (valueType != null)
			{
				return valueType.GetHashCode();
			}
			return myTrueOnlyBoolean.GetHashCode();
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(ColumnValueType other)
		{
			return myValueType == other.myValueType && myTrueOnlyBoolean == other.myTrueOnlyBoolean;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(ColumnValueType left, ColumnValueType right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(ColumnValueType left, ColumnValueType right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Automatically cast this structure to an ObjectType
		/// </summary>
		public static implicit operator ObjectType(ColumnValueType columnValueType)
		{
			return columnValueType.myValueType;
		}
		/// <summary>
		/// Automatically cast an ObjectType to this structure
		/// </summary>
		public static implicit operator ColumnValueType(ObjectType valueType)
		{
			return new ColumnValueType(valueType);
		}
		/// <summary>
		/// Automatically cast this structure to a nullable boolean. Returns true for a true-only
		/// boolean, false for a true/false boolean and null otherwise.
		/// </summary>
		public static implicit operator bool?(ColumnValueType columnValueType)
		{
			return columnValueType.myValueType == null ? (bool?)columnValueType.myTrueOnlyBoolean : null;
		}
		/// <summary>
		/// Automatically cast a boolean to this structure. A true value represents a read-only boolean
		/// and false represents a true/false boolean.
		/// </summary>
		public static implicit operator ColumnValueType(bool isReadOnlyBoolean)
		{
			return new ColumnValueType(isReadOnlyBoolean);
		}
		#endregion // Equality and casting routines
	}

	/// <summary>
	/// Provide column-related information from a model that maps
	/// a <see cref="Column"/> to elements in the ORM model.
	/// </summary>
	public interface IMappedColumnInfo
	{
		/// <summary>
		/// Get the <see cref="ColumnValueType"/> associated with this column.
		/// </summary>
		ColumnValueType ValueType { get; }
	}

	/// <summary>
	/// An interface to implement on a domain model that maps
	/// ORM elements to a <see cref="Column"/>
	/// </summary>
	public interface IProvideMappedColumnInfo
	{
		/// <summary>
		/// Retrieve column info for a given column.
		/// </summary>
		/// <param name="column">The column to map</param>
		/// <returns>Column information, or null if the column is not mapped by this provider.</returns>
		IMappedColumnInfo GetColumnInfo(Column column);
	}
	#endregion // IProvideMappedColumnInfo struct
}
