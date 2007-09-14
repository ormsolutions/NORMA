using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using SchemaExplorer;

namespace SchemaExplorer.DILSchemaProvider
{
	public class DILSchemaProvider : SchemaExplorer.IDbSchemaProvider
	{
		private XmlDocument _doc;
		private XmlNamespaceManager _manager;
		private string _cachedConnectionString;
		private DateTime _creationDate;

		public DILSchemaProvider()
		{
			_doc = new XmlDocument();
			_creationDate = DateTime.Now;
			_manager = new XmlNamespaceManager(_doc.NameTable);
			_manager.AddNamespace("dcl", "http://schemas.orm.net/DIL/DCIL");
			_manager.AddNamespace("dil", "http://schemas.orm.net/DIL/DIL");
			_manager.AddNamespace("ddt", "http://schemas.orm.net/DIL/DILDT");
			_manager.AddNamespace("dep", "http://schemas.orm.net/DIL/DILEP");
			_manager.AddNamespace("dml", "http://schemas.orm.net/DIL/DMIL");
			_manager.AddNamespace("ddl", "http://schemas.orm.net/DIL/DDIL");
		}

		#region IDbSchemaProvider Members

		public string Name
		{
			get
			{
				return "DILSchemaProvider";
			}
		}

		public string Description
		{
			get
			{
				return "DIL Schema Provider";
			}
		}

		public string GetDatabaseName(string connectionString)
		{
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNode databaseName = _doc.SelectSingleNode("dcl:schema/@name", _manager);
			if (databaseName != null)
			{
				return databaseName.Value;
			}
			else
			{
				return String.Empty;
			}
		}

		public ExtendedProperty[] GetExtendedProperties(string connectionString, SchemaObjectBase schemaObject)
		{
			return new ExtendedProperty[0];
		}

		public ColumnSchema[] GetTableColumns(string connectionString, TableSchema table)
		{
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNodeList columns = _doc.SelectNodes("dcl:schema/dcl:table[@name='" + table.Name + "']/dcl:column", _manager);
			int columnCount = columns.Count;
			ColumnSchema[] ret = new ColumnSchema[columnCount];
			for (int i = 0; i < columnCount; ++i)
			{
				byte precision = 0;
				int size = -1, scale = 0;
				string nativeDataType = "", columnName = "";
				DbType dbType = 0;
				bool isNullable = false;

				XmlNode currentColumn = columns[i];
				XmlAttribute columnNameNode = currentColumn.Attributes["name"];
				if (columnNameNode != null)
				{
					columnName = columnNameNode.Value.Trim('\"').Replace("\"\"", "\"");
				}

				XmlAttribute isNullableNode = currentColumn.Attributes["isNullable"];
				if (isNullableNode != null)
				{
					isNullable = bool.Parse(isNullableNode.Value);
				}

				XmlNode predefinedDataTypeNode = currentColumn.SelectSingleNode("dcl:predefinedDataType", _manager);
				if (predefinedDataTypeNode == null)
				{
					XmlNode refNameNode = currentColumn.SelectSingleNode("dcl:domainRef/@name", _manager);
					if (refNameNode != null)
					{
						string refName = refNameNode.Value;
						predefinedDataTypeNode = currentColumn.SelectSingleNode("dcl:schema/dcl:domain[@name='" + refName +"']/dcl:predefinedDataType", _manager);
					}
				}
				if (predefinedDataTypeNode != null)
				{
					XmlAttribute nativeDataTypeNode = predefinedDataTypeNode.Attributes["name"];
					if (nativeDataTypeNode != null)
					{
						nativeDataType = nativeDataTypeNode.Value.ToLower();
					}

					switch (nativeDataType.ToUpper())
					{
						case "CHARACTER LARGE OBJECT":
						case "BINARY LARGE OBJECT":
							dbType = DbType.Binary;
							size = 16;
							break;
						case "CHARACTER":
							dbType = DbType.StringFixedLength;
							size = 100;
							break;
						case "CHARACTER VARYING":
							dbType = DbType.String;
							size = 100;
							break;
						case "NUMERIC":
						case "DECIMAL":
							dbType = DbType.Decimal;
							size = 32;
							break;
						case "SMALLINT":
							dbType = DbType.Int16;
							size = 2;
							break;
						case "INTEGER":
							dbType = DbType.Int32;
							size = 4;
							break;
						case "BIGINT":
							dbType = DbType.Int64;
							size = 8;
							precision = 19;
							break;
						case "DOUBLE PRECISION":
						case "FLOAT":
							dbType = DbType.Double;
							size = 8;
							break;
						case "REAL":
							dbType = DbType.Single;
							size = 4;
							break;
						case "BOOLEAN":
							dbType = DbType.Boolean;
							size = 1;
							break;
						case "DATE":
							dbType = DbType.Date;
							size = 4;
							break;
						case "TIME":
							dbType = DbType.Time;
							size = 3;
							break;
						case "TIMESTAMP":
						case "INTERVAL":
							dbType = DbType.DateTime;
							size = 10;
							break;
						default:
							dbType = 0;
							size = -1;
							break;
					}

					XmlAttribute precisionNode = currentColumn.Attributes["precision"];
					if (precisionNode != null)
					{
						precision = Byte.Parse(precisionNode.Value);
					}

					XmlAttribute lengthNode = currentColumn.Attributes["length"];
					if (lengthNode != null)
					{
						size = int.Parse(lengthNode.Value);
					}

					XmlAttribute scaleNode = currentColumn.Attributes["scale"];
					if (scaleNode != null)
					{
						scale = int.Parse(scaleNode.Value);
					}
				}
				ret[i] = new ColumnSchema(table, columnName, dbType, nativeDataType, size, precision, scale, isNullable);
			}
			return ret;
		}

		public TableKeySchema[] GetTableKeys(string connectionString, TableSchema table)
		{
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNodeList foreignKeys = _doc.SelectNodes("dcl:schema/dcl:table[@name='" + table.Name + "']/dcl:referenceConstraint|dcl:schema/dcl:table/dcl:referenceConstraint[@targetTable='" + table.Name + "']", _manager);
			int foreignKeyCount = foreignKeys.Count;
			TableKeySchema[] ret = new TableKeySchema[foreignKeyCount];
			for (int i = 0; i < foreignKeyCount; ++i)
			{
				XmlNode foreignKey = foreignKeys[i];
				string foreignKeyName = "", foreignKeyTable = "", primaryKeyTable = "";
				string[] foreignKeyColumns, primaryKeyColumns;

				XmlAttribute foreignKeyTableNode = foreignKey.ParentNode.Attributes["name"];
				if (foreignKeyTableNode != null)
				{
					foreignKeyTable = foreignKeyTableNode.Value;
				}

				XmlAttribute primaryKeyTableNode = foreignKey.Attributes["targetTable"];
				if (primaryKeyTableNode != null)
				{
					primaryKeyTable = primaryKeyTableNode.Value;
				}

				XmlAttribute foreignKeyNameNode = foreignKey.Attributes["name"];
				if (foreignKeyNameNode != null)
				{
					foreignKeyName = foreignKeyNameNode.Value;
				}

				XmlNodeList columnRefNodeList = foreignKey.SelectNodes("dcl:columnRef", _manager);
				int columnRefNodeListCount = columnRefNodeList.Count;
				foreignKeyColumns = new string[columnRefNodeListCount];
				primaryKeyColumns = new string[columnRefNodeListCount];
				for (int j = 0; j < columnRefNodeListCount; ++j)
				{
					XmlNode columnRefNode = columnRefNodeList[j];
					XmlAttribute sourceNameAttribute = columnRefNode.Attributes["sourceName"];
					if (sourceNameAttribute != null)
					{
						foreignKeyColumns[j] = sourceNameAttribute.Value;
					}
					XmlAttribute targetNameAttribute = columnRefNode.Attributes["targetName"];
					if (targetNameAttribute != null)
					{
						primaryKeyColumns[j] = targetNameAttribute.Value;
					}
				}
				ret[i] = new TableKeySchema(table.Database, foreignKeyName, foreignKeyColumns, foreignKeyTable, primaryKeyColumns, primaryKeyTable);
			}
			return ret;
		}

		public PrimaryKeySchema GetTablePrimaryKey(string connectionString, TableSchema table)
		{
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNode primaryKey = _doc.SelectSingleNode("dcl:schema/dcl:table[@name='" + table.Name.Substring(table.Name.LastIndexOf(".") + 1, table.Name.Length - table.Name.LastIndexOf(".") - 1) + "']/dcl:uniquenessConstraint", _manager);
			string keyName = "";
			string[] memberColumns;
			if (primaryKey != null)
			{
				XmlAttribute keyNameNode = primaryKey.Attributes["name"];
				if (keyNameNode != null)
				{
					keyName = keyNameNode.Value;
				}

				XmlNodeList memberColumnsNodeList = primaryKey.SelectNodes("dcl:columnRef", _manager);
				int memberColumnsCount = memberColumnsNodeList.Count;
				memberColumns = new string[memberColumnsCount];
				for (int i = 0; i < memberColumnsCount; ++i)
				{
					XmlAttribute nameAttribute = memberColumnsNodeList[i].Attributes["name"];
					if (nameAttribute != null)
					{
						memberColumns[i] = nameAttribute.Value;
					}
				}
			}
			else
			{
				memberColumns = new string[0];
			}
			return new PrimaryKeySchema(table, keyName, memberColumns);
		}

		public TableSchema[] GetTables(string connectionString, DatabaseSchema database)
		{
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNodeList tables = _doc.SelectNodes("dcl:schema/dcl:table", _manager);
			int tableCount = tables.Count;
			TableSchema[] ret = new TableSchema[tableCount];
			for (int i = 0; i < tableCount; ++i)
			{
				XmlNode currentTable = tables[i];
				string tableName = "";

				XmlAttribute nameNode = currentTable.Attributes["name"];
				if (nameNode != null)
				{
					tableName = nameNode.Value; //nameNode.Value; //
				}
				ret[i] = new TableSchema(database, tableName, database.Name, _creationDate);
			}
			return ret;
		}

		public IndexSchema[] GetTableIndexes(string connectionString, TableSchema table)
		{
			// In our implementation, return an "index" for every uniquenessConstraint
			if (_cachedConnectionString != connectionString)
			{
				_cachedConnectionString = connectionString;
				_doc.Load(connectionString);
			}
			XmlNodeList indexes = _doc.SelectNodes("dcl:schema/dcl:table[@name='" + table.Name + "']/dcl:uniquenessConstraint", _manager);
			int indexCount = indexes.Count;
			IndexSchema[] ret = new IndexSchema[indexCount];
			for (int i = 0; i < indexCount; ++i)
			{
				string indexName = "";
				bool isPrimary = false;
				string[] sourceColumns;

				XmlNode index = indexes[i];

				XmlAttribute indexNameAttribute = index.Attributes["name"];
				if (indexNameAttribute != null)
				{
					indexName = indexNameAttribute.Value;
				}

				XmlAttribute isPrimaryAttribute = index.Attributes["isPrimary"];
				if (isPrimaryAttribute != null)
				{
					isPrimary = bool.Parse(isPrimaryAttribute.Value);
				}

				XmlNodeList columnRefNodeList = index.SelectNodes("dcl:columnRef", _manager);
				int columnRefNodeListCount = columnRefNodeList.Count;
				sourceColumns = new string[columnRefNodeListCount];
				for (int j = 0; j < columnRefNodeListCount; ++j)
				{
					XmlNode columnRefNode = columnRefNodeList[j];
					XmlAttribute sourceNameAttribute = columnRefNode.Attributes["name"];
					if (sourceNameAttribute != null)
					{
						sourceColumns[j] = sourceNameAttribute.Value;
					}
				}
				ret[i] = new IndexSchema(table, indexName, isPrimary, true, true, sourceColumns);
			}
			return ret;
		}

		#region Unimplemented Interface Methods
		public ParameterSchema[] GetCommandParameters(string connectionString, CommandSchema command)
		{
			// NetTiers shouldn't be using this, so unimplemented right now
			return new ParameterSchema[0];
		}

		public CommandResultSchema[] GetCommandResultSchemas(string connectionString, CommandSchema command)
		{
			// NetTiers shouldn't be using this, so unimplemented right now
			return new CommandResultSchema[0];
		}

		public string GetCommandText(string connectionString, CommandSchema command)
		{
			// NetTiers shouldn't be using this, so unimplemented right now
			return "";
		}

		public CommandSchema[] GetCommands(string connectionString, DatabaseSchema database)
		{
			// NetTiers shouldn't be using this, so unimplemented right now
			return new CommandSchema[0];
		}

		public DataTable GetTableData(string connectionString, TableSchema table)
		{
			return new DataTable();
		}

		public ViewColumnSchema[] GetViewColumns(string connectionString, ViewSchema view)
		{
			return new ViewColumnSchema[0];
		}

		public DataTable GetViewData(string connectionString, ViewSchema view)
		{
			return new DataTable();
		}

		public string GetViewText(string connectionString, ViewSchema view)
		{
			return "";
		}

		public ViewSchema[] GetViews(string connectionString, DatabaseSchema database)
		{
			return new ViewSchema[0];
		}
		#endregion
		#endregion
	}
}
