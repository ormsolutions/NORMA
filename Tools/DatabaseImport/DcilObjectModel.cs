#region zlib/libpng Copyright Notice
/**************************************************************************\
* Database Intermediate Language                                           *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Neumont.Tools.ORM.DatabaseImport
{
    /// <summary>
    /// Represents a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "schema", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilSchema : IXmlSerializable
	{
		private string _name;
		private IList<DcilTable> _tables;
		private IList<DcilProcedure> _procedures;
		private IDcilSchemaProvider _dataProvider;
        /// <summary>
        /// Namespaces that are associated with the Schema
        /// </summary>
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces Namespaces;
        /// <summary>
        /// Serializes an instance of <see cref="DcilSchema"/>
        /// </summary>
        /// <param name="schema">The <see cref="DcilSchema"/> to Serialize</param>
        /// <param name="writer">The <see cref="System.IO.TextWriter"/> to write to</param>
		public static void Serialize(DcilSchema schema, System.IO.TextWriter writer)
		{
			schema.Namespaces = new XmlSerializerNamespaces();
			schema.Namespaces.Add("dcl", "http://schemas.orm.net/DIL/DCIL");
			schema.Namespaces.Add("dil", "http://schemas.orm.net/DIL/DIL");
			schema.Namespaces.Add("ddt", "http://schemas.orm.net/DIL/DILDT");
			schema.Namespaces.Add("dep", "http://schemas.orm.net/DIL/DILEP");
			schema.Namespaces.Add("dml", "http://schemas.orm.net/DIL/DMIL");
			schema.Namespaces.Add("ddl", "http://schemas.orm.net/DIL/DDIL");
			XmlSerializer s = new XmlSerializer(typeof(DcilSchema));
			s.Serialize(writer, schema);
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilSchema"/>
        /// </summary>
		public DcilSchema()
		{
			this._name = "UNKNOWN";
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilSchema"/>
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="name"></param>
		public DcilSchema(IDcilSchemaProvider dataProvider, string name)
		{
			this._name = name;
			this._dataProvider = dataProvider;
		}
        /// <summary>
        /// Returns a <see cref="DcilSchema"/> object from given schema name
        /// </summary>
        /// <param name="schemaName">The name of the schema to load</param>
        /// <param name="connection">The IDbConnection to use to retrieve the information</param>
        /// <param name="dataProviderName">The invariant name of the Data Provider</param>
        /// <exception cref="NotSupportedException"/>
        /// <returns><see cref="DcilSchema"/></returns>
        public static DcilSchema FromSchemaName(string schemaName, IDbConnection connection, string dataProviderName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (String.IsNullOrEmpty(schemaName)) throw new ArgumentException("The name of the schema must not be empty", "schemaName");
            IDcilSchemaProvider provider = GetDcilSchemaProvider(connection, dataProviderName);
            return provider.LoadSchema(schemaName);
        }
        /// <summary>
        /// Returns a <see cref="IDcilSchemaProvider"/> object from given connection and data provider
        /// </summary>
        /// <param name="connection">The IDbConnection to use to retrieve the information</param>
        /// <param name="dataProviderName">The invariant name of the Data Provider</param>
        /// <returns><see cref="IDcilSchemaProvider"/></returns>
        public static IDcilSchemaProvider GetDcilSchemaProvider(IDbConnection connection, string dataProviderName)
        {
            IDcilSchemaProvider provider = null;
            switch (dataProviderName)
            {
                case "MySql.Data.MySqlClient":
                    provider = new MySqlDcilSchemaProvider(connection);
                    break;
                case "System.Data.SqlClient":
                    provider = new SqlServer2005DcilSchemaProvider(connection);
                    break;
            }
            if (provider == null) throw new NotSupportedException("The specified invariant name is not supported.");
            return provider;
        }
        /// <summary>
        /// Gets or Sets the Name of the Schema
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the list of <see cref="DcilTable"/> objects that belong to this Schema
        /// </summary>
		public IList<DcilTable> Tables
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
        /// <summary>
        /// Gets or Sets the list of <see cref="DcilProcedure"/> objects that belong to this Schema
        /// </summary>
		public IList<DcilProcedure> Procedures
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
        /// <summary>
        /// Returns the name of the Schema
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _name;
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is written</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			IList<DcilTable> tables = Tables;
			int tableCount = tables.Count;
			XmlSerializer tableSerializer = new XmlSerializer(typeof(DcilTable));
			for (int i = 0; i < tableCount; ++i)
			{
				tableSerializer.Serialize(writer, tables[i]);
			}
		}
		#endregion
	}
    /// <summary>
    /// Represents a DCIL Table element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "table", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilTable : IXmlSerializable
	{
		private IDcilSchemaProvider _dataProvider;
		private string _schemaName, _tableName;
		private IList<DcilColumn> _columns;
		private IList<DcilUniquenessConstraint> _uniConstraints;
		private IList<DcilReferenceConstraint> _refConstraints;
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilTable"/>
        /// </summary>
		public DcilTable()
		{
			this._schemaName = "UNKNOWN";
			this._tableName = "UNKNOWN";
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilTable"/>
        /// </summary>
		public DcilTable(IDcilSchemaProvider dataProvider, string schemaName, string tableName)
		{
			this._dataProvider = dataProvider;
			this._schemaName = schemaName;
			this._tableName = tableName;
		}
        /// <summary>
        /// Gets or Sets the name of the Schema to which the Table belongs to
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Table
        /// </summary>
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
        /// <summary>
        /// Gets the list of <see cref="DcilColumn"/> objects that belong to the Table
        /// </summary>
		public IList<DcilColumn> Columns
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
        /// <summary>
        /// Gets the list of <see cref="DcilUniquenessConstraint"/> objects that belong to the Table
        /// </summary>
		public IList<DcilUniquenessConstraint> UniquenessConstraints
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
        /// <summary>
        /// Gets the list of <see cref="DcilReferenceConstraint"/> objects that belong to the Table
        /// </summary>
		public IList<DcilReferenceConstraint> ReferenceConstraints
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
        /// <summary>
        /// Returns the Schema-qualified name of the Table
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _schemaName + "." + _tableName;
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is written</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _tableName);
            writer.WriteAttributeString("oilRefName", _tableName);

			IList<DcilColumn> columns = Columns;
			int columnCount = columns.Count;
			XmlSerializer columnSerializer = new XmlSerializer(typeof(DcilColumn));
			for (int i = 0; i < columnCount; ++i)
			{
				columnSerializer.Serialize(writer, columns[i]);
			}

			IList<DcilUniquenessConstraint> uniquenessConstraints = UniquenessConstraints;
			int uniquenessCount = uniquenessConstraints.Count;
			XmlSerializer uniquenessSerializer = new XmlSerializer(typeof(DcilUniquenessConstraint));
			for (int i = 0; i < uniquenessCount; ++i)
			{
				uniquenessSerializer.Serialize(writer, uniquenessConstraints[i]);
			}

			IList<DcilReferenceConstraint> referenceConstraints = ReferenceConstraints;
			int referenceCount = referenceConstraints.Count;
			XmlSerializer referenceSerializer = new XmlSerializer(typeof(DcilReferenceConstraint));
			for (int i = 0; i < referenceCount; ++i)
			{
				referenceSerializer.Serialize(writer, referenceConstraints[i]);
			}
		}
		#endregion
	}
    /// <summary>
    /// Represents a DCIL Column element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "column", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilColumn : IXmlSerializable
	{
		private string _name;
		private bool _isNullable, _isIdentity;
		private DcilDataType _dataType;
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilColumn"/>
        /// </summary>
		public DcilColumn()
		{
			this._name = "UNKNOWN";
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilColumn"/>
        /// </summary>
		public DcilColumn(string name, DcilDataType dataType, bool isNullable, bool isIdentity)
		{
			this._name = name;
			this._dataType = dataType;
			this._isNullable = isNullable;
			this._isIdentity = isIdentity;
		}
        /// <summary>
        /// Gets or Sets the name of the Column
        /// </summary>
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
        /// <summary>
        /// Gets or Sets a value indicating whether or not the Columns allows nulls
        /// </summary>
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
        /// <summary>
        /// Gets or Sets a value indicating whether or not the Column has an Identity Specification
        /// </summary>
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
        /// <summary>
        /// Returns the name of the Column
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _name;
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is written</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("oilRefName", _name);
			writer.WriteAttributeString("isNullable", _isNullable.ToString().ToLowerInvariant());
			writer.WriteAttributeString("isIdentity", _isIdentity.ToString().ToLowerInvariant());
			XmlSerializer dataTypeSerializer = new XmlSerializer(typeof(DcilDataType));
			dataTypeSerializer.Serialize(writer, _dataType);
		}
		#endregion
	}
    /// <summary>
    /// Represents a DCIL UniquenessConstraint element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "uniquenessConstraint", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilUniquenessConstraint : IXmlSerializable
	{
		private bool _isPrimary;
		private string _name, _schema;
		private string _parentTable, _parentTableSchema;
		private StringCollection _columns;
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilUniquenessConstraint"/>
        /// </summary>
		public DcilUniquenessConstraint()
		{
			this._schema = "UNKNOWN";
			this._name = "UNKNOWN";
			this._parentTable = "UNKNOWN";
			this._parentTableSchema = "UNKNOWN";
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilUniquenessConstraint"/>
        /// </summary>
		public DcilUniquenessConstraint(string schema, string name, string parentTableSchema, string parentTable, StringCollection columns, bool isPrimary)
		{
			this._schema = schema;
			this._name = name;
			this._parentTableSchema = parentTableSchema;
			this._parentTable = parentTable;
			this._columns = columns;
			this._isPrimary = isPrimary;
		}
        /// <summary>
        /// Gets or Sets a value indicating whether or not the Column has an Identity Specification
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Uniqueness Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Schema to which the Uniqueness Constraint belongs to
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Table to which the Uniqueness Constraint belongs to
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Schema of the parent Table to which the Uniqueness Constraint belongs to
        /// </summary>
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
        /// <summary>
        /// Gets or Sets a collection of Column names
        /// </summary>
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
        /// <summary>
        /// Returns the Schema-qualified name of the Uniqueness Constraint
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _schema + "." + _name;
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is written</param>
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("name", _name);
			writer.WriteAttributeString("isPrimary", _isPrimary.ToString().ToLowerInvariant());
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
    /// <summary>
    /// Represents a DCIL ReferenceConstraint element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "referenceConstraint", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilReferenceConstraint : IXmlSerializable
	{
		private string _schema, _name;
		private string _sourceTableSchema, _sourceTable, _targetTableSchema, _targetTable;
		private StringCollection _sourceColumns, _targetColumns;
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilReferenceConstraint"/>
        /// </summary>
		public DcilReferenceConstraint()
		{
			this._schema = "UNKNOWN";
			this._name = "UNKNOWN";
			this._sourceTable = "UNKNOWN";
			this._sourceTableSchema = "UNKNOWN";
			this._targetTable = "UNKNOWN";
			this._targetTableSchema = "UNKNOWN";
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilReferenceConstraint"/>
        /// </summary>
		public DcilReferenceConstraint(string schema, string name, string sourceTableSchema, string sourceTable, string targetTableSchema, string targetTable, StringCollection sourceColumns, StringCollection targetColumns)
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
        /// <summary>
        /// Gets or Sets the name of the Schema to which the Reference Constraint belongs to
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the name of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the Schema of the Source Table of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the Source Table of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the Schema of the Target Table of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the Target Table of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the collection of Source Columns of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the collection of Target Columns of the Reference Constraint
        /// </summary>
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
        /// <summary>
        /// Returns the Schema-qualified name of the Reference Constraint
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _schema + "." + _name;
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is written</param>
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
    /// <summary>
    /// Represents a DCIL PredefinedDataType element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "predefinedDataType", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilDataType : IXmlSerializable
	{
        /// <summary>
        /// Represents a predefined data type as defined in the DILDT Scehma
        /// </summary>
		public enum DCILType
		{
            /// <summary>
            /// Fixed-length character data
            /// </summary>
			CharacterLargeObject,
            /// <summary>
            /// Fixed-length character data
            /// </summary>
			Character,
            /// <summary>
            /// Variable-length data
            /// </summary>
			CharacterVarying,
            /// <summary>
            /// Binary data
            /// </summary>
			BinaryLargeObject,
            /// <summary>
            /// Fixed precision and scale numeric data
            /// </summary>
			Numeric,
            /// <summary>
            /// Fixed precision and scale numeric data
            /// </summary>
			Decimal,
            /// <summary>
            /// Integer data from -2^15 through 2^15 - 1
            /// </summary>
			SmallInt,
            /// <summary>
            /// Integer data from -2^31 through 2^31 - 1
            /// </summary>
			Integer,
            /// <summary>
            /// Integer data from -2^63 through 2^63-1
            /// </summary>
			BigInt,
            /// <summary>
            /// Floating precision number data from -1.79E + 308 through 1.79E + 308
            /// </summary>
			Float,
            /// <summary>
            /// Floating precision number data from -3.40E + 38 through 3.40E + 38
            /// </summary>
			Real,
            /// <summary>
            /// Double precision number data
            /// </summary>
			DoublePrecision,
            /// <summary>
            /// Boolean data
            /// </summary>
			Boolean,
            /// <summary>
            /// Date data
            /// </summary>
			Date,
            /// <summary>
            /// Time data
            /// </summary>
			Time,
            /// <summary>
            /// Date and time data
            /// </summary>
			Timestamp,
            /// <summary>
            /// Interval data
            /// </summary>
			Interval
		}

		private DCILType _type;
		private long _size, _scale;
		private short _precision;
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilDataType"/>
        /// </summary>
		public DcilDataType()
			: this(DCILType.CharacterVarying)
		{
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilDataType"/>
        /// </summary>
		public DcilDataType(DCILType type)
			: this(type, -1, -1, -1)
		{
		}
        /// <summary>
        /// Instantiates a new instance of <see cref="DcilDataType"/>
        /// </summary>
		public DcilDataType(DCILType type, long size, long scale, short precision)
		{
			this._type = type;
			this._size = size;
			this._scale = scale;
			this._precision = precision;
		}
        /// <summary>
        /// Gets or Sets the <see cref="DcilDataType.DCILType"/> of the DcilDataType
        /// </summary>
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
        /// <summary>
        /// Gets or Sets the size of the Data Type
        /// </summary>
		public long Size
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
        /// <summary>
        /// Gets or Sets the scale of the Data Type
        /// </summary>
		public long Scale
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
        /// <summary>
        /// Gets or Sets the precision of the Data Type
        /// </summary>
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
        /// <summary>
        /// Converts the <see cref="DcilDataType.DCILType"/> of the DcilDataType to a <see cref="System.Data.DbType"/>
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the string representation of the <see cref="DcilDataType.DCILType"/> of the DcilDataType
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return _type.ToString();
		}

		#region IXmlSerializable Members
        /// <summary>
        /// This property is reserved, apply the <see cref="System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead
        /// </summary>
        /// <returns></returns>
		public XmlSchema GetSchema()
		{
			return null;
		}
        /// <summary>
        /// Generates an object from its XML representation
        /// </summary>
        /// <param name="reader">The <see cref="System.Xml.XmlReader"/> stream from which the object is read</param>
		public void ReadXml(XmlReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}
        /// <summary>
        /// Converts the object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="System.Xml.XmlWriter"/> stream to which the object is written</param>
		public void WriteXml(XmlWriter writer)
		{
            char[] charArray = new char[26];
            for(int i = 0; i < 26; ++i)
            {
                charArray[i] = (char)(i + 65);
            }

            
            string typeString = _type.ToString();
            if (typeString.IndexOf("Int") == -1)
            {
                typeString = Regex.Replace(typeString, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            typeString = typeString.ToUpperInvariant();

			writer.WriteAttributeString("name", typeString);
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
    /// <summary>
    /// Represents a DCIL Procedure element of a DCIL Document
    /// </summary>
	[XmlRoot(ElementName = "procedure", Namespace = "http://schemas.orm.net/DIL/DCIL")]
	public sealed class DcilProcedure
	{
	}
}