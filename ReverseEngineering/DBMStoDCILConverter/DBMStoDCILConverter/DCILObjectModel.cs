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
	[XmlRoot(ElementName = "schema", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILSchema : IXmlSerializable
	{
		private string _name;
		private List<DCILTable> _tables;
		private List<DCILProcedure> _procedures;
		private IDCILSchemaProvider _dataProvider;

		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces Namespaces;

		public static void Serialize(DCILSchema schema, XmlWriter writer)
		{
			schema.Namespaces = new XmlSerializerNamespaces();
			schema.Namespaces.Add("dcl", "http://schema.orm.net/DIL/DCIL");
			schema.Namespaces.Add("dil", "http://schemas.orm.net/DIL/DIL");
			schema.Namespaces.Add("ddt", "http://schemas.orm.net/DIL/DILDT");
			schema.Namespaces.Add("dep", "http://schemas.orm.net/DIL/DILEP");
			schema.Namespaces.Add("dml", "http://schemas.orm.net/DIL/DMIL");
			schema.Namespaces.Add("ddl", "http://schemas.orm.net/DIL/DDIL");
			XmlSerializer s = new XmlSerializer(typeof(DCILSchema));
			s.Serialize(writer, schema);
		}

		public DCILSchema()
		{
			this._name = "UNKNOWN";
		}

		public DCILSchema(IDCILSchemaProvider dataProvider, string name)
		{
			this._name = name;
			this._dataProvider = dataProvider;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (this._name != value)
				{
					this._name = value;
					this._tables = null;
					this._procedures = null;
				}
			}
		}

		public List<DCILTable> Tables
		{
			get
			{
				if (this._tables == null && this._dataProvider != null)
				{
					this._tables = _dataProvider.LoadTables(_name);
				}
				return this._tables;
			}
		}

		public List<DCILProcedure> Procedures
		{
			get
			{
				if (this._procedures == null && this._dataProvider != null)
				{
					this._procedures = _dataProvider.LoadProcedures(_name);
				}
				return this._procedures;
			}
		}

		public override string ToString()
		{
			return _name;
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			List<DCILTable> tables = Tables;
			int tableCount = tables.Count;
			XmlSerializer tableSerializer = new XmlSerializer(typeof(DCILTable));
			for (int i = 0; i < tableCount; ++i)
			{
				tableSerializer.Serialize(writer, tables[i]);
			}
		}

		#endregion
	}

	[XmlRoot(ElementName = "table", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILTable : IXmlSerializable
	{
		private IDCILSchemaProvider _dataProvider;
		private string _schemaName, _tableName;
		private List<DCILColumn> _columns;
		private List<DCILUniquenessConstraint> _uniConstraints;
		private List<DCILReferenceConstraint> _refConstraints;

		public DCILTable()
		{
			this._schemaName = "UNKNOWN";
			this._tableName = "UNKNOWN";
		}

		public DCILTable(IDCILSchemaProvider dataProvider, string schemaName, string tableName)
		{
			this._dataProvider = dataProvider;
			this._schemaName = schemaName;
			this._tableName = tableName;
		}

		public string SchemaName
		{
			get
			{
				return this._schemaName;
			}
			set
			{
				if (this._schemaName != value)
				{
					this._schemaName = value;
					this._columns = null;
					this._uniConstraints = null;
					this._refConstraints = null;
				}
			}
		}

		public string TableName
		{
			get
			{
				return this._tableName;
			}
			set
			{
				if (this._tableName != value)
				{
					this._tableName = value;
					this._columns = null;
					this._uniConstraints = null;
					this._refConstraints = null;
				}
			}
		}

		public List<DCILColumn> Columns
		{
			get
			{
				if (this._columns == null && this._dataProvider != null)
				{
					this._columns = _dataProvider.LoadColumns(_schemaName, _tableName);
				}
				return this._columns;
			}
		}

		public List<DCILUniquenessConstraint> UniquenessConstraints
		{
			get
			{
				if (this._uniConstraints == null && this._dataProvider != null)
				{
					this._uniConstraints = _dataProvider.LoadIndexes(_schemaName, _tableName);
				}
				return this._uniConstraints;
			}
		}

		public List<DCILReferenceConstraint> ReferenceConstraints
		{
			get
			{
				if (this._refConstraints == null && this._dataProvider != null)
				{
					this._refConstraints = _dataProvider.LoadForeignKeys(_schemaName, _tableName);
				}
				return this._refConstraints;
			}
		}

		public override string ToString()
		{
			return _schemaName + "." + _tableName;
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _tableName);

			List<DCILColumn> columns = Columns;
			int columnCount = columns.Count;
			XmlSerializer columnSerializer = new XmlSerializer(typeof(DCILColumn));
			for (int i = 0; i < columnCount; ++i)
			{
				columnSerializer.Serialize(writer, columns[i]);
			}

			List<DCILUniquenessConstraint> uniquenessConstraints = UniquenessConstraints;
			int uniquenessCount = uniquenessConstraints.Count;
			XmlSerializer uniquenessSerializer = new XmlSerializer(typeof(DCILUniquenessConstraint));
			for (int i = 0; i < uniquenessCount; ++i)
			{
				uniquenessSerializer.Serialize(writer, uniquenessConstraints[i]);
			}

			List<DCILReferenceConstraint> referenceConstraints = ReferenceConstraints;
			int referenceCount = referenceConstraints.Count;
			XmlSerializer referenceSerializer = new XmlSerializer(typeof(DCILReferenceConstraint));
			for (int i = 0; i < referenceCount; ++i)
			{
				referenceSerializer.Serialize(writer, referenceConstraints[i]);
			}
		}

		#endregion
	}

	[XmlRoot(ElementName = "column", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILColumn : IXmlSerializable
	{
		private string _name;
		private bool _isNullable, _isIdentity;
		private DCILDataType _dataType;

		public DCILColumn()
		{
			this._name = "UNKNOWN";
		}

		public DCILColumn(string name, DCILDataType dataType, bool isNullable, bool isIdentity)
		{
			this._name = name;
			this._dataType = dataType;
			this._isNullable = isNullable;
			this._isIdentity = isIdentity;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
			set
			{
				this._isNullable = value;
			}
		}

		public bool IsIdentity
		{
			get
			{
				return this._isIdentity;
			}
			set
			{
				this._isIdentity = value;
			}
		}

		public override string ToString()
		{
			return _name;
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			writer.WriteAttributeString("isNullable", _isNullable.ToString());
			writer.WriteAttributeString("isIdentity", _isIdentity.ToString());
			XmlSerializer dataTypeSerializer = new XmlSerializer(typeof(DCILDataType));
			dataTypeSerializer.Serialize(writer, _dataType);
		}

		#endregion
	}

	[XmlRoot(ElementName = "uniquenessConstraint", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILUniquenessConstraint : IXmlSerializable
	{
		private bool _isPrimary;
		private string _name, _schema;
		private string _parentTable, _parentTableSchema;
		private StringCollection _columns;

		public DCILUniquenessConstraint()
		{
			this._schema = "UNKNOWN";
			this._name = "UNKNOWN";
			this._parentTable = "UNKNOWN";
			this._parentTableSchema = "UNKNOWN";
		}

		public DCILUniquenessConstraint(string schema, string name, string parentTableSchema, string parentTable, StringCollection columns, bool isPrimary)
		{
			this._schema = schema;
			this._name = name;
			this._parentTableSchema = parentTableSchema;
			this._parentTable = parentTable;
			this._columns = columns;
			this._isPrimary = isPrimary;
		}

		public bool IsPrimary
		{
			get
			{
				return this._isPrimary;
			}
			set
			{
				this._isPrimary = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				this._schema = value;
			}
		}

		public string ParentTable
		{
			get
			{
				return this._parentTable;
			}
			set
			{
				this._parentTable = value;
			}
		}

		public string ParentTableSchema
		{
			get
			{
				return this._parentTableSchema;
			}
			set
			{
				this._parentTableSchema = value;
			}
		}

		public StringCollection Columns
		{
			get
			{
				return this._columns;
			}
			set
			{
				this._columns = value;
			}
		}

		public override string ToString()
		{
			return _schema + "." + _name;
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			writer.WriteAttributeString("isPrimary", _isPrimary.ToString());
			StringCollection columns = _columns;
			int columnCount = columns.Count;
			for (int i = 0; i < columnCount; ++i)
			{
				writer.WriteStartElement("columnRef");
				writer.WriteAttributeString("name", columns[i]);
				writer.WriteEndElement();
			}
		}

		#endregion
	}

	[XmlRoot(ElementName = "referenceConstraint", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILReferenceConstraint : IXmlSerializable
	{
		private string _schema, _name;
		private string _sourceTableSchema, _sourceTable, _targetTableSchema, _targetTable;
		private StringCollection _sourceColumns, _targetColumns;

		public DCILReferenceConstraint()
		{
			this._schema = "UNKNOWN";
			this._name = "UNKNOWN";
			this._sourceTable = "UNKNOWN";
			this._sourceTableSchema = "UNKNOWN";
			this._targetTable = "UNKNOWN";
			this._targetTableSchema = "UNKNOWN";
		}

		public DCILReferenceConstraint(string schema, string name, string sourceTableSchema, string sourceTable, string targetTableSchema, string targetTable, StringCollection sourceColumns, StringCollection targetColumns)
		{
			this._schema = schema;
			this._name = name;
			this._sourceTableSchema = sourceTableSchema;
			this._sourceTable = sourceTable;
			this._targetTableSchema = targetTableSchema;
			this._targetTable = targetTable;
			this._sourceColumns = sourceColumns;
			this._targetColumns = targetColumns;
		}

		public string Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				this._schema = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string SourceTableSchema
		{
			get
			{
				return this._sourceTableSchema;
			}
			set
			{
				this._sourceTableSchema = value;
			}
		}

		public string SourceTable
		{
			get
			{
				return this._sourceTable;
			}
			set
			{
				this._sourceTable = value;
			}
		}

		public string TargetTableSchema
		{
			get
			{
				return this._targetTableSchema;
			}
			set
			{
				this._targetTableSchema = value;
			}
		}

		public string TargetTable
		{
			get
			{
				return this._targetTable;
			}
			set
			{
				this._targetTable = value;
			}
		}

		public StringCollection SourceColumns
		{
			get
			{
				return this._sourceColumns;
			}
			set
			{
				this._sourceColumns = value;
			}
		}

		public StringCollection TargetColumns
		{
			get
			{
				return this._targetColumns;
			}
			set
			{
				this._targetColumns = value;
			}
		}

		public override string ToString()
		{
			return _schema + "." + _name;
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			writer.WriteAttributeString("targetTable", _targetTable);
			StringCollection sourceColumns = _sourceColumns;
			StringCollection targetColumns = _targetColumns;
			int sourceColumnCount = sourceColumns.Count;
			int targetColumnCount = targetColumns.Count;
			if (sourceColumnCount == targetColumnCount)
			{
				for (int i = 0; i < sourceColumnCount; ++i)
				{
					writer.WriteStartElement("columnRef");
					writer.WriteAttributeString("sourceName", sourceColumns[i]);
					writer.WriteAttributeString("targetName", targetColumns[i]);
					writer.WriteEndElement();
				}
			}
		}

		#endregion
	}

	[XmlRoot(ElementName = "predefinedDataType")]
	public sealed class DCILDataType : IXmlSerializable
	{
		public enum DCILType
		{
			CharacterLargeObject,
			Character,
			CharacterVarying,
			BinaryLargeObject,
			Numeric,
			Decimal,
			SmallInt,
			Integer,
			BigInt,
			Float,
			Real,
			DoublePrecision,
			Boolean,
			Date,
			Time,
			Timestamp,
			Interval
		}

		private DCILType _type;
		private int _size, _scale;
		private short _precision;

		public DCILDataType()
			: this(DCILType.CharacterVarying)
		{
		}

		public DCILDataType(DCILType type)
			: this(type, -1, -1, -1)
		{
		}

		public DCILDataType(DCILType type, int size, int scale, short precision)
		{
			this._type = type;
			this._size = size;
			this._scale = scale;
			this._precision = precision;
		}

		public DCILType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
			}
		}

		public int Scale
		{
			get
			{
				return this._scale;
			}
			set
			{
				this._scale = value;
			}
		}

		public short Precision
		{
			get
			{
				return this._precision;
			}
			set
			{
				this._precision = value;
			}
		}

		public DbType ConvertToADODataType()
		{
			switch (this._type)
			{
				case DCILType.CharacterLargeObject:
				case DCILType.BinaryLargeObject:
					return DbType.Binary;
				case DCILType.Character:
					return DbType.StringFixedLength;
				case DCILType.CharacterVarying:
					return DbType.String;
				case DCILType.Numeric:
				case DCILType.Decimal:
					return DbType.Decimal;
				case DCILType.SmallInt:
					return DbType.Int16;
				case DCILType.Integer:
					return DbType.Int32;
				case DCILType.BigInt:
					return DbType.Int64;
				case DCILType.DoublePrecision:
				case DCILType.Float:
					return DbType.Double;
				case DCILType.Real:
					return DbType.Single;
				case DCILType.Boolean:
					return DbType.Boolean;
				case DCILType.Date:
					return DbType.Date;
				case DCILType.Time:
					return DbType.Time;
				case DCILType.Timestamp:
				case DCILType.Interval:
					return DbType.DateTime;
				default:
					return DbType.Object;
			}
		}

		public override string ToString()
		{
			return _type.ToString();
		}

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _type.ToString().ToUpperInvariant());
			if (_size != -1)
			{
				writer.WriteAttributeString("length", _size.ToString());
			}
			if (_precision != -1)
			{
				writer.WriteAttributeString("precision", _precision.ToString());
			}
			if (_scale != -1)
			{
				writer.WriteAttributeString("scale", _scale.ToString());
			}
		}

		#endregion
	}

	[XmlRoot(ElementName = "procedure", Namespace = "http://schema.orm.net/DIL/DCIL")]
	public sealed class DCILProcedure
	{
	}
}
