#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
	/// Provides an implementation of IDcilSchemaProvider for MySQL 5.0
	/// </summary>
	public class MySqlDcilSchemaProvider : IDcilSchemaProvider
	{
		private IDbConnection _conn;
        /// <summary>
        /// Instantiates a new instance of ORMSolutions.ORMArchitect.DatabaseImport.MySqlDcilSchemaProvider
        /// </summary>
        /// <param name="conn">The <see cref="System.Data.IDbConnection"/> object for the target Database</param>
        public MySqlDcilSchemaProvider(IDbConnection conn)
		{
            this._conn = conn;
		}
        /// <summary>
        /// When implemented in a child class, retrieves a list of available schema names
        /// </summary>
        /// <returns>List of available schema names</returns>
        public IList<string> GetAvailableSchemaNames()
        {
			return new string[]{_conn.Database};
        }
        /// <summary>
        /// Loads the specified MySQL 5.0 Schema into a <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilSchema"/> object
        /// </summary>
        /// <param name="schemaName">Name of the Schema to load</param>
		public DcilSchema LoadSchema(string schemaName)
		{
			return new DcilSchema(this, schemaName);
		}
        /// <summary>
        /// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified MySQL 5.0 Schema
        /// </summary>
        /// <param name="schemaName">Name of the Schema from which to load the Tables</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilTable"/> objects for the specified MySQL 5.0 Schema</returns>
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
				string commandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
				if(!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				cmd.CommandText = commandText;

                using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        string table = reader.GetString(0);
                        tables.Add(new DcilTable(this, schemaName, table));
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
        /// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified MySQL 5.0 Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the MySQL 5.0 Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Columns</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilColumn"/> objects for the specified MySQL 5.0 Schema and Table</returns>
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
                string commandText = "SELECT COLUMN_NAME, IS_NULLABLE, CASE extra WHEN 'auto_increment' then '1' ELSE '0' END AS `IS_IDENTITY`, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_SCALE FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME IS NOT NULL";
				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += " AND TABLE_NAME = '" + tableName + "'";
				}
				cmd.CommandText = commandText;
                using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        string columnName = reader.GetString(0);
                        bool isNullable = (reader.GetString(1).ToUpperInvariant() == "YES");
                        bool isIdentity = (reader.GetInt32(2) == 1);
                        DcilDataType.DCILType type = ConvertMySqlDataType(reader.GetString(3));
                        long size = (reader.IsDBNull(4) ? -1 : reader.GetInt64(4));
                        long scale = (reader.IsDBNull(5) ? -1 : reader.GetInt64(5));
                        columns.Add(new DcilColumn(columnName, new DcilDataType(type, size, scale, -1), isNullable, isIdentity));
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
        /// Converts the given string representation of a SQL Data Type to its equilavent <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilDataType.DCILType"/>
        /// </summary>
        private DcilDataType.DCILType ConvertMySqlDataType(string dataType)
        {
            dataType = dataType.ToLowerInvariant();
            switch (dataType)
            {
                case "nvarchar":
                case "varchar":
                case "ntext":
                case "text":
                case "enum":
                case "longtext":
                    return DcilDataType.DCILType.CharacterVarying;
                case "char":
                case "nchar":
                    return DcilDataType.DCILType.Character;
                case "int":
                    return DcilDataType.DCILType.Integer;
                case "smallint":
                case "tinyint":
                    return DcilDataType.DCILType.SmallInt;
                case "bigint":
                    return DcilDataType.DCILType.BigInt;
                case "smalldatetime":
                case "datetime":
                    return DcilDataType.DCILType.Date;
                case "timestamp":
                    return DcilDataType.DCILType.Timestamp;
                case "bit":
                    return DcilDataType.DCILType.Boolean;
                case "decimal":
                case "money":
                case "smallmoney":
                    return DcilDataType.DCILType.Decimal;
                case "float":
                    return DcilDataType.DCILType.Float;
                case "numeric":
                    return DcilDataType.DCILType.Numeric;
                case "double":
                    return DcilDataType.DCILType.DoublePrecision;
                case "image":
                case "binary":
                case "varbinary":
                    return DcilDataType.DCILType.BinaryLargeObject;
                case "real":
                    return DcilDataType.DCILType.Real;
                default:
                    return DcilDataType.DCILType.CharacterVarying;
            }
        }
        /// <summary>
        /// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified MySQL 5.0 Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the MySQL 5.0 Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilUniquenessConstraint"/> objects (representing Uniqueness Constraints) for the specified MySQL 5.0 Schema and Table</returns>
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
				string commandText = "SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, TABLE_SCHEMA, TABLE_NAME, CASE WHEN CONSTRAINT_TYPE = 'PRIMARY KEY' THEN 1 ELSE 0 END AS IS_PRIMARY FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE (CONSTRAINT_TYPE = 'PRIMARY KEY' OR CONSTRAINT_TYPE = 'UNIQUE')";
				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += " AND TABLE_NAME = '" + tableName + "'";
				}
				cmd.CommandText = commandText;
                using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        string constraintSchema = reader.GetString(0);
                        string constraintName = reader.GetString(1);
                        string tableSchema = reader.GetString(2);
                        string table = reader.GetString(3);
                        bool isPrimary = (reader.GetInt32(4) == 1 ? true : false);
                        constraints.Add(new DcilUniquenessConstraint(constraintSchema, constraintName, tableSchema, table, new StringCollection(), isPrimary));
                    }
                }

				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					DcilUniquenessConstraint constraint = constraints[i];
					IDbCommand columns = _conn.CreateCommand();
					columns.CommandType = CommandType.Text;
                    columns.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_SCHEMA = '" + constraint.Schema + "' AND CONSTRAINT_NAME = '" + constraint.Name + "' AND TABLE_SCHEMA = '" + constraint.ParentTableSchema + "' AND TABLE_NAME = '" + constraint.ParentTable + "'";
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
        /// When implemented in a child class, loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified MySQL 5.0 Schema and Table
        /// </summary>
        /// <param name="schemaName">Name of the MySQL 5.0 Schema for which the given Table resides in</param>
        /// <param name="tableName">Name of the Table from which to load the Indexes</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilReferenceConstraint"/> objects (representing Foreign Keys) for the specified MySQL 5.0 Schema and Table</returns>
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
				string commandText = "SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'";
				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += " AND TABLE_NAME = '" + tableName + "'";
				}
				cmd.CommandText = commandText;
                using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {
                        string constraintSchema = reader.GetString(0);
                        string constraintName = reader.GetString(1);
                        string sourceTableSchema = reader.GetString(2);
                        string sourceTable = reader.GetString(3);
                        constraints.Add(new DcilReferenceConstraint(constraintSchema, constraintName, sourceTableSchema, sourceTable, "", "", new StringCollection(), new StringCollection()));
                    }
                }
				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					DcilReferenceConstraint constraint = constraints[i];
                    IDbCommand sourceColumns = _conn.CreateCommand();
					sourceColumns.CommandType = CommandType.Text;
                    sourceColumns.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '" + constraint.Name + "' AND CONSTRAINT_SCHEMA = '" + constraint.Schema + "' AND TABLE_SCHEMA = '" + constraint.SourceTableSchema + "' AND TABLE_NAME = '" + constraint.SourceTable + "'";
                    using (IDataReader sourceColumnReader = sourceColumns.ExecuteReader(CommandBehavior.Default))
                    {
                        while (sourceColumnReader.Read())
                        {
                            constraint.SourceColumns.Add(sourceColumnReader.GetString(0));
                        }
                    }
                    IDbCommand targetColumns = _conn.CreateCommand();
					targetColumns.CommandType = CommandType.Text;
                    targetColumns.CommandText = "SELECT REFERENCED_TABLE_SCHEMA, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME = '" + constraint.Name + "' AND CONSTRAINT_SCHEMA = '" + constraint.Schema + "'  AND TABLE_SCHEMA = '" + constraint.SourceTableSchema + "' AND TABLE_NAME = '" + constraint.SourceTable + "'";
                    StringCollection targetColumnList = new StringCollection();
                    using (IDataReader targetColumnReader = targetColumns.ExecuteReader(CommandBehavior.Default))
                    {
                        while (targetColumnReader.Read())
                        {
                            if (constraint.TargetTableSchema == "")
                            {
                                constraint.TargetTableSchema = targetColumnReader.GetString(0);
                                constraint.TargetTable = targetColumnReader.GetString(1);
                            }
                            constraint.TargetColumns.Add(targetColumnReader.GetString(2));
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
        /// Loads a generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified MySQL 5.0 Schema
        /// </summary>
        /// <param name="schemaName">Name of the MySQL 5.0 Schema from which to laod the Stored Procedures</param>
        /// <returns>Generic list of <see cref="ORMSolutions.ORMArchitect.DatabaseImport.DcilProcedure"/> objects for the specified MySQL 5.0 Schema</returns>
		public IList<DcilProcedure> LoadProcedures(string schemaName)
		{
			return new List<DcilProcedure>().AsReadOnly();
		}
	}
}