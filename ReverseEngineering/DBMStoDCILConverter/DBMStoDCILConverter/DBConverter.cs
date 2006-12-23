using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DBMStoDCILConverter
{
	public interface IDCILSchemaProvider
	{
		DCILSchema LoadSchema(string schemaName);
		List<DCILTable> LoadTables(string schemaName);
		List<DCILColumn> LoadColumns(string schemaName, string tableName);
		List<DCILUniquenessConstraint> LoadIndexes(string schemaName, string tableName);
		List<DCILReferenceConstraint> LoadForeignKeys(string schemaName, string tableName);
		List<DCILProcedure> LoadProcedures(string schemaName);
	}

	public class SQLServer2005_DCILSchemaProvider : IDCILSchemaProvider
	{
		private SqlConnection _conn;

		public SQLServer2005_DCILSchemaProvider(SqlConnection conn)
		{
			this._conn = conn;
		}

		public DCILSchema LoadSchema(string schemaName)
		{
			return new DCILSchema(this, schemaName);
		}

		public List<DCILTable> LoadTables(string schemaName)
		{
			List<DCILTable> tables = new List<DCILTable>();
			try
			{
				_conn.Open();
				SqlCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				string commandText = "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
				if(!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				cmd.CommandText = commandText;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string schema = reader.GetString(0);
					string table = reader.GetString(1);
					tables.Add(new DCILTable(this, schema, table));
				}
				reader.Close();
				return tables;
			}
			finally
			{
				if (_conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}

		public List<DCILColumn> LoadColumns(string schemaName, string tableName)
		{
			List<DCILColumn> columns = new List<DCILColumn>();
			try
			{
				_conn.Open();
				SqlCommand cmd = _conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				string commandText = "SELECT COLUMN_NAME, IS_NULLABLE, COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') IS_IDENTITY, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, COALESCE(NUMERIC_PRECISION, DATETIME_PRECISION), NUMERIC_SCALE FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME IS NOT NULL";
				if (!String.IsNullOrEmpty(schemaName))
				{
					commandText += " AND TABLE_SCHEMA = '" + schemaName + "'";
				}
				if (!String.IsNullOrEmpty(tableName))
				{
					commandText += " AND TABLE_NAME = '" + tableName + "'";
				}
				cmd.CommandText = commandText;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string columnName = reader.GetString(0);
					bool isNullable = (reader.GetString(1).ToUpperInvariant() == "YES" ? true : false);
					bool isIdentity = (reader.GetInt32(2) == 1 ? true : false);
					DCILDataType.DCILType type = DCILDataType.DCILType.CharacterVarying;
					int size = (reader.IsDBNull(4) ? -1 : reader.GetInt32(4));
					short precision = (reader.IsDBNull(5) ? (short)-1 : reader.GetInt16(5));
					int scale = (reader.IsDBNull(6) ? -1 : reader.GetInt32(6));
					columns.Add(new DCILColumn(columnName, new DCILDataType(type, size, scale, precision), isNullable, isIdentity));
				}
				reader.Close();
				return columns;
			}
			finally
			{
				if (_conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}

		public List<DCILUniquenessConstraint> LoadIndexes(string schemaName, string tableName)
		{
			List<DCILUniquenessConstraint> constraints = new List<DCILUniquenessConstraint>();
			try
			{
				_conn.Open();
				SqlCommand cmd = _conn.CreateCommand();
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
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string constraintSchema = reader.GetString(0);
					string constraintName = reader.GetString(1);
					string tableSchema = reader.GetString(2);
					string table = reader.GetString(3);
					bool isPrimary = (reader.GetInt32(4) == 1 ? true : false);
					constraints.Add(new DCILUniquenessConstraint(constraintSchema, constraintName, tableSchema, table, new StringCollection(), isPrimary));
				}
				reader.Close();
				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					DCILUniquenessConstraint constraint = constraints[i];
					SqlCommand columns = _conn.CreateCommand();
					columns.CommandType = CommandType.Text;
					columns.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE CONSTRAINT_SCHEMA = '" + constraint.Schema + "' AND CONSTRAINT_NAME = '" + constraint.Name + "'";
					SqlDataReader columnReader = columns.ExecuteReader();
					StringCollection columnList = new StringCollection();
					while (columnReader.Read())
					{
						constraint.Columns.Add(columnReader.GetString(0));
					}
					columnReader.Close();
				}
				return constraints;
			}
			finally
			{
				if (_conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}

		public List<DCILReferenceConstraint> LoadForeignKeys(string schemaName, string tableName)
		{
			List<DCILReferenceConstraint> constraints = new List<DCILReferenceConstraint>();
			try
			{
				_conn.Open();
				SqlCommand cmd = _conn.CreateCommand();
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
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					string constraintSchema = reader.GetString(0);
					string constraintName = reader.GetString(1);
					string sourceTableSchema = reader.GetString(2);
					string sourceTable = reader.GetString(3);
					constraints.Add(new DCILReferenceConstraint(constraintSchema, constraintName, sourceTableSchema, sourceTable, "", "", new StringCollection(), new StringCollection()));
				}
				reader.Close();
				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					DCILReferenceConstraint constraint = constraints[i];
					SqlCommand sourceColumns = _conn.CreateCommand();
					sourceColumns.CommandType = CommandType.Text;
					sourceColumns.CommandText = "SELECT ccu.COLUMN_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON ccu.CONSTRAINT_SCHEMA = rc.UNIQUE_CONSTRAINT_SCHEMA AND ccu.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME WHERE rc.CONSTRAINT_NAME = '" + constraint.Name + "' AND rc.CONSTRAINT_SCHEMA = '" + constraint.Schema + "'";
					SqlDataReader sourceColumnReader = sourceColumns.ExecuteReader();
					while (sourceColumnReader.Read())
					{
						constraint.SourceColumns.Add(sourceColumnReader.GetString(0));
					}
					sourceColumnReader.Close();
					SqlCommand targetColumns = _conn.CreateCommand();
					targetColumns.CommandType = CommandType.Text;
					targetColumns.CommandText = "SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE CONSTRAINT_NAME = '" + constraint.Name + "' AND CONSTRAINT_SCHEMA = '" + constraint.Schema + "'";
					SqlDataReader targetColumnReader = targetColumns.ExecuteReader();
					StringCollection targetColumnList = new StringCollection();
					while (targetColumnReader.Read())
					{
						if (constraint.TargetTableSchema == "")
						{
							constraint.TargetTableSchema = targetColumnReader.GetString(0);
							constraint.TargetTable = targetColumnReader.GetString(1);
						}
						constraint.TargetColumns.Add(targetColumnReader.GetString(2));
					}
					targetColumnReader.Close();
				}
				return constraints;
			}
			finally
			{
				if (_conn.State == ConnectionState.Open)
				{
					_conn.Close();
				}
			}
		}

		public List<DCILProcedure> LoadProcedures(string schemaName)
		{
			return new List<DCILProcedure>();
		}
	}
}