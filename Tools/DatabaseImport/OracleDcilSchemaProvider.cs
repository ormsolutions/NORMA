#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ORMSolutions.ORMArchitect.DatabaseImport
{
	/// <summary>
	/// Provides an implementation of IDcilSchemaProvider for Oracle
	/// </summary>
	public class OracleDcilSchemaProvider : IDcilSchemaProvider
	{
		private IDbConnection _conn;
		/// <summary>
		/// Instantiates a new instance of ORMSolutions.ORMArchitect.DatabaseImport.OracleDcilSchemaProvider
		/// </summary>
		/// <param name="conn">The <see cref="System.Data.IDbConnection"/> object for the target Database</param>
		public OracleDcilSchemaProvider(IDbConnection conn)
		{
			this._conn = conn;
		}
		/// <summary>
		/// When implemented in a child class, retrieves a list of available schema names
		/// </summary>
		/// <returns>List of available schema names</returns>
		public IList<string> GetAvailableSchemaNames()
		{
			IList<string> schemaNames = new List<string>();
			bool opened = false;
			try
			{
				if (_conn.State != ConnectionState.Open)
				{
					_conn.Open();
					opened = true;
				}
				using (IDbCommand cmd = _conn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "select distinct owner from all_objects order by 1";
					using (IDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							schemaNames.Add(reader[0].ToString());
						}
					}
				}
			}
			finally
			{
				if (opened && _conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
			return schemaNames;
		}
		/// <summary>
		/// Loads the specified Oracle Schema into a <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilSchema"/> object
		/// </summary>
		/// <param name="schemaName">Name of the Schema to load</param>
		public DcilSchema LoadSchema(string schemaName)
		{
			return new DcilSchema(this, schemaName);
		}
		/// <summary>
		/// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified Oracle Schema
		/// </summary>
		/// <param name="schemaName">Name of the Schema from which to load the Tables</param>
		/// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified Oracle Schema</returns>
		public IList<DcilTable> LoadTables(string schemaName)
		{
			IList<DcilTable> tables = new List<DcilTable>();
			bool opened = false;
			try
			{
				if (_conn.State != ConnectionState.Open)
				{
					_conn.Open();
					opened = true;
				}
				IDbCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				bool haveSchema = !string.IsNullOrEmpty(schemaName);
				string commandText = string.Format("select owner, table_name from all_tables {0}",
					haveSchema ? "where owner = '" + schemaName + "'" : null);

				cmd.CommandText = commandText;

				using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
				{
					while (reader.Read())
					{
						string schema = reader.GetString(0);
						string table = reader.GetString(1);
						tables.Add(new DcilTable(this, schema, table));
					}
				}
				return tables;
			}
			finally
			{
				if (opened && _conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}
		/// <summary>
		/// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified Oracle Schema and Table
		/// </summary>
		/// <param name="schemaName">Name of the Oracle Schema for which the given Table resides in</param>
		/// <param name="tableName">Name of the Table from which to load the Columns</param>
		/// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified Oracle Schema and Table</returns>
		public IList<DcilColumn> LoadColumns(string schemaName, string tableName)
		{
			IList<DcilColumn> columns = new List<DcilColumn>();
			bool opened = false;
			try
			{
				if (_conn.State != ConnectionState.Open)
				{
					_conn.Open();
					opened = true;
				}
				IDbCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				bool haveSchema = !string.IsNullOrEmpty(schemaName);
				string commandText = "select column_name, nullable, data_type, " +
					"nvl2(char_used, char_length, data_length) length, data_precision, data_scale " +
					"from all_tab_columns " +
					"where 1 = 1 ";

				if (!string.IsNullOrEmpty(schemaName))
				{
					commandText += "and owner = '" + schemaName + "' ";
				}
				if (!string.IsNullOrEmpty(tableName))
				{
					commandText += "and table_name = '" + tableName + "' ";
				}

				cmd.CommandText = commandText;
				using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
				{
					while (reader.Read())
					{
						string columnName = reader.GetString(0);
						bool isNullable = (reader.GetString(1).ToUpperInvariant() == "Y");
						bool isIdentity = false;
						DcilDataType.DCILType type = ConvertOracleDataType(reader.GetString(2));						
						int size = Convert.ToInt32(reader[3]);
						short precision = (reader.IsDBNull(4) ? (short)-1 : Convert.ToInt16(reader[4]));
						int scale = (reader.IsDBNull(5) ? -1 : Convert.ToInt32(reader[5]));
						columns.Add(new DcilColumn(columnName, new DcilDataType(type, size, scale, precision), isNullable, isIdentity));
					}
				}
				return columns;
			}
			finally
			{
				if (opened && _conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}
		/// <summary>
		/// Converts the given string representation of an Oracle Data Type to its equivalent <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilDataType.DCILType"/>
		/// </summary>
		private DcilDataType.DCILType ConvertOracleDataType(string dataType)
		{
			dataType = dataType.ToUpperInvariant();
			if (dataType.IndexOf("TIMESTAMP") != -1)
				return DcilDataType.DCILType.Timestamp;

			switch (dataType)
			{
				case "BINARY_FLOAT":
				case "BINARY_DOUBLE":
				case "FLOAT":
					return DcilDataType.DCILType.Float;
				case "DATE":
					return DcilDataType.DCILType.Date;
				case "LONG":
				case "VARCHAR2":
				case "NVARCHAR2":
					return DcilDataType.DCILType.CharacterVarying;
				case "CHAR":
				case "NCHAR":
					return DcilDataType.DCILType.Character;
				case "NUMBER":
					return DcilDataType.DCILType.Decimal;
				case "RAW":
				case "LONG RAW":
				case "BLOB":
					return DcilDataType.DCILType.BinaryLargeObject;
				case "CLOB":
				case "NCLOB":
					return DcilDataType.DCILType.CharacterLargeObject;

				default:
					return DcilDataType.DCILType.CharacterVarying;
			}
		}
		/// <summary>
		/// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Oracle Schema and Table
		/// </summary>
		/// <param name="schemaName">Name of the Oracle Schema for which the given Table resides in</param>
		/// <param name="tableName">Name of the Table from which to load the Indexes</param>
		/// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified Oracle Schema and Table</returns>
		public IList<DcilUniquenessConstraint> LoadIndexes(string schemaName, string tableName)
		{
			IList<DcilUniquenessConstraint> constraints = new List<DcilUniquenessConstraint>();
			bool opened = false;
			try
			{
				if (_conn.State != ConnectionState.Open)
				{
					_conn.Open();
					opened = true;
				}
				IDbCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				string commandText = "select owner, table_name, constraint_name, constraint_type " +
					"from all_constraints " +
					"where constraint_type in ('P', 'U') ";

				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += " and owner = '" + schemaName + "'";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += " and table_name = '" + tableName + "'";
				}
				cmd.CommandText = commandText;
				using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
				{
					while (reader.Read())
					{
						string constraintSchema = reader.GetString(0);
						string table = reader.GetString(1);
						string constraintName = reader.GetString(2);
						string tableSchema = reader.GetString(0);
						bool isPrimary = reader.GetString(3) == "P" ? true : false;
						constraints.Add(new DcilUniquenessConstraint(constraintSchema, constraintName, tableSchema, table, new StringCollection(), isPrimary));
					}
				}

				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					DcilUniquenessConstraint constraint = constraints[i];
					IDbCommand columns = _conn.CreateCommand();
					columns.CommandType = CommandType.Text;
					columns.CommandText = string.Format("select column_name from all_cons_columns " +
						"where owner = '{0}' and table_name = '{1}' and constraint_name = '{2}' " +
						"order by position", constraint.Schema, constraint.ParentTable, constraint.Name);

					StringCollection columnList = new StringCollection();
					using (IDataReader columnReader = columns.ExecuteReader(CommandBehavior.Default))
					{
						while (columnReader.Read())
						{
							constraint.Columns.Add(columnReader.GetString(0));
						}
					}
				}
				return constraints;
			}
			finally
			{
				if (opened && _conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}
		/// <summary>
		/// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Oracle Schema and Table
		/// </summary>
		/// <param name="schemaName">Name of the Oracle Schema for which the given Table resides in</param>
		/// <param name="tableName">Name of the Table from which to load the Indexes</param>
		/// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified Oracle Schema and Table</returns>
		public IList<DcilReferenceConstraint> LoadForeignKeys(string schemaName, string tableName)
		{
			IList<DcilReferenceConstraint> constraints = new List<DcilReferenceConstraint>();
			bool opened = false;
			try
			{
				if (_conn.State != ConnectionState.Open)
				{
					_conn.Open();
					opened = true;
				}
				IDbCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				string commandText = "select a.owner, a.table_name, a.constraint_name, a.r_owner, b.table_name r_table_name, a.r_constraint_name " +
					"from all_constraints a " +
					"   join all_constraints b on (b.owner = a.r_owner and b.constraint_name = a.r_constraint_name) " +
					"where a.constraint_type in ('R') ";
   
				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += "and a.owner = '" + schemaName + "' ";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += "and a.table_name = '" + tableName + "' ";
				}

				cmd.CommandText = commandText;
				IList<string> targetConstraintNames = new List<string>();
				using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
				{
					while (reader.Read())
					{
						string constraintSchema = reader.GetString(0);
						string sourceTableSchema = reader.GetString(0);
						string sourceTable = reader.GetString(1);
						string constraintName = reader.GetString(2);
						string targetTableSchema = reader.GetString(3);
						string targetTable = reader.GetString(4);
						targetConstraintNames.Add(reader.GetString(5));
						constraints.Add(new DcilReferenceConstraint(constraintSchema, constraintName, sourceTableSchema, sourceTable, 
							targetTableSchema, targetTable, new StringCollection(), new StringCollection()));
					}
				}
				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{				
					DcilReferenceConstraint constraint = constraints[i];
					IDbCommand sourceColumns = _conn.CreateCommand();
					sourceColumns.CommandType = CommandType.Text;
					sourceColumns.CommandText = string.Format("select column_name from all_cons_columns " +
						"where owner = '{0}' and table_name = '{1}' and constraint_name = '{2}' " +
						"order by position", constraint.Schema, constraint.SourceTable, constraint.Name);

					using (IDataReader sourceColumnReader = sourceColumns.ExecuteReader(CommandBehavior.Default))
					{
						while (sourceColumnReader.Read())
						{
							constraint.SourceColumns.Add(sourceColumnReader.GetString(0));
						}
					}

					string targetConstraintName = targetConstraintNames[i];

					IDbCommand targetColumns = _conn.CreateCommand();
					targetColumns.CommandType = CommandType.Text;
					targetColumns.CommandText = string.Format("select column_name from all_cons_columns " +
						"where owner = '{0}' and table_name = '{1}' and constraint_name = '{2}' " +
						"order by position", constraint.TargetTableSchema, constraint.TargetTable, targetConstraintName);

					using (IDataReader targetColumnReader = targetColumns.ExecuteReader(CommandBehavior.Default))
					{
						while (targetColumnReader.Read())
						{							
							constraint.TargetColumns.Add(targetColumnReader.GetString(0));
						}
					}
				}
				return constraints;
			}
			finally
			{
				if (opened && _conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}
		/// <summary>
		/// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified Oracle Schema
		/// </summary>
		/// <param name="schemaName">Name of the Oracle Schema from which to load the Stored Procedures</param>
		/// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified Oracle Schema</returns>
		public IList<DcilProcedure> LoadProcedures(string schemaName)
		{
			return new List<DcilProcedure>().AsReadOnly();
		}
	}
}