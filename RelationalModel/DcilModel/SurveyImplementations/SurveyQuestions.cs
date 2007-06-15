#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using System.Diagnostics;
using Neumont.Tools.Modeling.Design;
using System.ComponentModel;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;

namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	#region Survey Question Type Enums
	/// <summary>
	/// The list of possible answers for the Schema grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveySchemaType, Catalog>))]
	public enum SurveySchemaType
	{
		/// <summary>
		/// This element is a schema
		/// </summary>
		Schema,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = Schema,
	}
	/// <summary>
	/// The list of possible answers for the SchemaChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveySchemaChildType, Catalog>))]
	public enum SurveySchemaChildType
	{
		/// <summary>
		/// This element is a Table
		/// </summary>
		Table,
		/// <summary>
		/// The element is a Domain
		/// </summary>
		Domain,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = Domain,
	}
	/// <summary>
	/// The list of possible answers for the TableChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyTableChildType, Catalog>))]
	public enum SurveyTableChildType
	{
		/// <summary>
		/// This element is a Column
		/// </summary>
		Column,
		/// <summary>
		/// The element is a Constraint
		/// </summary>
		Constraint,
		/// <summary>
		/// The element is a ReferenceConstraint
		/// </summary>
		ReferenceConstraint,
	}
	/// <summary>
	/// The list of possible answers for the TableChildGlyphType category
	/// </summary>
	public enum SurveyTableChildGlyphType
	{
		/// <summary>
		/// The element is a column
		/// </summary>
		Column,
		/// <summary>
		/// The element is a foreign key
		/// </summary>
		ReferenceConstraint,
		/// <summary>
		/// The element is a uniqueness constraint
		/// </summary>
		UniquenessConstraint,
		/// <summary>
		/// The element is a primary uniqueness constraint
		/// </summary>
		PrimaryUniquenessConstraint,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = PrimaryUniquenessConstraint,
	}
	/// <summary>
	/// The list of possible answers for the ReferenceConstraintChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyReferenceConstraintChildType, Catalog>))]
	public enum SurveyReferenceConstraintChildType
	{
		/// <summary>
		/// The table being referenced
		/// </summary>
		TableReference,
		/// <summary>
		/// The column being referenced
		/// </summary>
		ColumnReference,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = ColumnReference,
	}
	/// <summary>
	/// The list of possible answers for the ForeignKeyChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyUniquenessConstraintChildType, Catalog>))]
	public enum SurveyUniquenessConstraintChildType
	{
		/// <summary>
		/// The column being referenced
		/// </summary>
		ColumnReference,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = ColumnReference,
	}
	#endregion // Survey Question Type Enums
	#region Schema answers
	partial class Schema : IAnswerSurveyQuestion<SurveySchemaType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveySchemaType> Implementation
		int IAnswerSurveyQuestion<SurveySchemaType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveySchemaType.Schema;
		}
		#endregion // IAnswerSurveyQuestion<SurveySchemaType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, Schema.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Schema.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(Schema), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// The key used to retrieve expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return SurveyExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // Schema answers
	#region Table answers
	partial class Table : IAnswerSurveyQuestion<SurveySchemaChildType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveySchemaChildType> Implementation
		int IAnswerSurveyQuestion<SurveySchemaChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveySchemaChildType.Table;
		}
		#endregion // IAnswerSurveyQuestion<SurveySchemaChildType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, Table.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Table.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(Table), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// The key used to retrieve expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return SurveyExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // Table answers
	#region Column answers
	partial class Column : IAnswerSurveyQuestion<SurveyTableChildType>, IAnswerSurveyQuestion<SurveyTableChildGlyphType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveyTableChildType.Column;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		#region IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildGlyphType>.AskQuestion()
		{
			return AskElementQuestionForGlyph();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestionForGlyph()
		{
			return (int)SurveyTableChildGlyphType.Column;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, Column.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Column.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(Column), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // Column answers
	#region ReferenceConstraint answers
	partial class ReferenceConstraint : IAnswerSurveyQuestion<SurveyTableChildType>, IAnswerSurveyQuestion<SurveyTableChildGlyphType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveyTableChildType.ReferenceConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		#region IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildGlyphType>.AskQuestion()
		{
			return AskElementQuestionForGlyph();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestionForGlyph()
		{
			return (int)SurveyTableChildGlyphType.ReferenceConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, ReferenceConstraint.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, ReferenceConstraint.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ReferenceConstraint), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// The key used to retrieve expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return SurveyExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // ReferenceConstraint answers
	#region UniquenessConstraint answers
	partial class UniquenessConstraint : IAnswerSurveyQuestion<SurveyTableChildType>, IAnswerSurveyQuestion<SurveyTableChildGlyphType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveyTableChildType.Constraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildType> Implementation
		#region IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		int IAnswerSurveyQuestion<SurveyTableChildGlyphType>.AskQuestion()
		{
			return AskElementQuestionForGlyph();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestionForGlyph()
		{
			return IsPrimary ? (int)SurveyTableChildGlyphType.PrimaryUniquenessConstraint : (int)SurveyTableChildGlyphType.UniquenessConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyTableChildGlyphType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(this, UniquenessConstraint.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, UniquenessConstraint.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(UniquenessConstraint), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// The key used to retrieve expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return SurveyExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // UniquenessConstraint answers
	#region ReferenceConstraintTargetsTable answers
	partial class ReferenceConstraintTargetsTable : IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> Implementation
		int IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveyReferenceConstraintChildType.TableReference;
		}
		#endregion // IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(TargetTable, Table.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return TargetTable.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return TargetTable.Name;
			}
			set
			{
				Table targetTable = TargetTable;
				DomainTypeDescriptor.CreatePropertyDescriptor(targetTable, Table.NameDomainPropertyId).SetValue(targetTable, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ReferenceConstraintTargetsTable), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // ReferenceConstraintTargetsTable answers
	#region ColumnReference answers
	partial class ColumnReference : IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> Implementation
		int IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected static int AskElementQuestion()
		{
			return (int)SurveyReferenceConstraintChildType.ColumnReference;
		}
		#endregion // IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(SourceColumn, Column.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				// UNDONE: Localize the format string
				return string.Format(CultureInfo.InvariantCulture, "{0}->{1}", SourceColumn.Name, TargetColumn.Name);
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return SourceColumn.Name;
			}
			set
			{
				Column sourceColumn = SourceColumn;
				DomainTypeDescriptor.CreatePropertyDescriptor(sourceColumn, Column.NameDomainPropertyId).SetValue(sourceColumn, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ColumnReference), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // ColumnReference answers
	#region UniquenessConstraintIncludesColumn answers
	partial class UniquenessConstraintIncludesColumn : IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType> Implementation
		int IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		protected int AskElementQuestion()
		{
			return (int)SurveyUniquenessConstraintChildType.ColumnReference;
		}
		#endregion // IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(Column, Column.NameDomainPropertyId).IsReadOnly;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				return Column.Name;
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return Column.Name;
			}
			set
			{
				Column targetColumn = Column;
				DomainTypeDescriptor.CreatePropertyDescriptor(targetColumn, Column.NameDomainPropertyId).SetValue(targetColumn, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(UniquenessConstraintIncludesColumn), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	#endregion // ReferenceConstraintTargetsTable answers
	#region ISurveyNodeProvider Implementation
	partial class ConceptualDatabaseDomainModel : ISurveyNodeProvider
	{
		#region ISurveyNodeProvider Implementation
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes(object context, object expansionKey)
		{
			return this.GetSurveyNodes(context, expansionKey);
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeProvider.GetSurveyNodes"/>
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes(object context, object expansionKey)
		{
			if (expansionKey == null)
			{
				IElementDirectory elementDirectory = Store.ElementDirectory;
				foreach (Schema element in elementDirectory.FindElements<Schema>(true))
				{
					yield return element;
				}
			}
			else if (expansionKey == Schema.SurveyExpansionKey)
			{
				Schema schema = context as Schema;
				if (schema != null)
				{
					foreach (Table table in schema.TableCollection)
					{
						yield return table;
					}
				}
			}
			else if (expansionKey == Table.SurveyExpansionKey)
			{
				Table table = context as Table;
				if (table != null)
				{
					foreach (Column column in table.ColumnCollection)
					{
						yield return column;
					}
					foreach (ReferenceConstraint referenceConstraint in table.ReferenceConstraintCollection)
					{
						yield return referenceConstraint;
					}
					foreach (UniquenessConstraint uniquenessConstraint in table.UniquenessConstraintCollection)
					{
						yield return uniquenessConstraint;
					}
				}
			}
			else if (expansionKey == ReferenceConstraint.SurveyExpansionKey)
			{
				ReferenceConstraint referenceConstraint = context as ReferenceConstraint;
				if (referenceConstraint != null)
				{
					// UNDONE: Do the table reference directly as a link in the model browser when the construct is available
					ReferenceConstraintTargetsTable tableLink = ReferenceConstraintTargetsTable.GetLinkToTargetTable(referenceConstraint);
					if (tableLink != null)
					{
						yield return tableLink;
					}
					foreach (ColumnReference columnRef in referenceConstraint.ColumnReferenceCollection)
					{
						yield return columnRef;
					}
				}
			}
			else if (expansionKey == UniquenessConstraint.SurveyExpansionKey)
			{
				UniquenessConstraint uniquenessConstraint = context as UniquenessConstraint;
				if (uniquenessConstraint != null)
				{
					// UNDONE: Do this directly as a link in the model browser when the construct is available
					foreach (UniquenessConstraintIncludesColumn columnRef in UniquenessConstraintIncludesColumn.GetLinksToColumnCollection(uniquenessConstraint))
					{
						yield return columnRef;
					}
				}
			}
		}
		#endregion // ISurveyNodeProvider Implementation
	}
	#endregion // ISurveyNodeProvider Implementation
}
