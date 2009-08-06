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
using System.Diagnostics;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.ComponentModel;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker
{
	#region QUESTIONS
	/// <summary>
	/// The list of possible answers for the Barker model grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyBarkerModelType, BarkerErModel>))]
	public enum SurveyBarkerModelType
	{
		/// <summary>
		/// This element is a model
		/// </summary>
		Model,
		/// <summary>
		/// The current highest-valued value in the enumeration
		/// </summary>
		Last = Model,
	}
	/// <summary>
	/// The list of possible answers for the BarkerErModel grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyBarkerElementType, BarkerErModel>))]
	public enum SurveyBarkerElementType
	{
		/// <summary>
		/// The element is an <see cref="EntityType"/>
		/// </summary>
		EntityType,
		/// <summary>
		/// The element is a <see cref="BinaryAssociation"/>
		/// </summary>
		BinaryAssociation,
		/// <summary>
		/// The element is an <see cref="ExclusiveArc"/>
		/// </summary>
		Constraint,
	}
	/// <summary>
	/// The list of possible answers for the EntityChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyEntityChildType, BarkerErModel>))]
	public enum SurveyEntityChildType
	{
		/// <summary>
		/// This element is an Attribute
		/// </summary>
		Attribute,
		/// <summary>
		/// This element is a subtype of current entity type
		/// </summary>
		Subtype,
		/// <summary>
		/// This element is role played by this entity
		/// </summary>
		RoleRef,
	}

	/// <summary>
	/// The list of possible answers for the BinaryAssociationChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyBinaryAssociationChildType, BarkerErModel>))]
	public enum SurveyBinaryAssociationChildType
	{
		/// <summary>
		/// This element is an optional role with multiplicity of one
		/// </summary>
		Role_OptionalOne,
		/// <summary>
		/// This element is a mandatory role with multiplicity of one
		/// </summary>
		Role_MandatoryOne,
		/// <summary>
		/// This element is a mandatory role with multiplicity of many
		/// </summary>
		Role_MandatoryMany,
		/// <summary>
		/// This element is an optional role with multiplicity of many
		/// </summary>
		Role_OptionalMany,
		/// <summary>
		/// This element is a mandatory role with multiplicity of one; part of primary identifier
		/// </summary>
		Role_MandatoryOnePrimary,
		/// <summary>
		/// This element is a mandatory role with multiplicity of many; part of primary identifier
		/// </summary>
		Role_MandatoryManyPrimary,
	}
	/// <summary>
	/// The list of possible answers for the AttributeChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyAttributeChildType, BarkerErModel>))]
	public enum SurveyAttributeChildType
	{
		/// <summary>
		/// This element is a possible value for the attribute
		/// </summary>
		PossibleValue,
	}
	/// <summary>
	/// The list of possible answers for the RoleChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyRoleChildType, BarkerErModel>))]
	public enum SurveyRoleChildType
	{
		/// <summary>
		/// This element is a possible value for the role
		/// </summary>
		CardinalityQualifier,
	}
	/// <summary>
	/// The list of possible answers for the ExclusiveArcChildType grouping in the model browser
	/// </summary>
	[TypeConverter(typeof(EnumConverter<SurveyExclusiveArcChildType, BarkerErModel>))]
	public enum SurveyExclusiveArcChildType
	{
		/// <summary>
		/// This element is role spanned by this exclusive arc
		/// </summary>
		RoleRef,
	}

	#endregion // Survey Question Type Enums
	#region ANSWERS
	#region Model answers
	partial class BarkerErModel : IAnswerSurveyQuestion<SurveyBarkerModelType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyBarkerModelType> Members
		int IAnswerSurveyQuestion<SurveyBarkerModelType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyBarkerModelType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyBarkerModelType.Model;
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, BarkerErModel.NameDomainPropertyId).IsReadOnly;
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
				return Name;
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
				return Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, BarkerErModel.NameDomainPropertyId).SetValue(this, value);
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
				retVal.SetData(typeof(BarkerErModel), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected static object SurveyNodeContext
		{
			get { return null; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region EntityType answers
	partial class EntityType : IAnswerSurveyQuestion<SurveyBarkerElementType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyBarkerElementType> Members
		int IAnswerSurveyQuestion<SurveyBarkerElementType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyBarkerElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyBarkerElementType.EntityType;
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, EntityType.NameDomainPropertyId).IsReadOnly;
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
				DomainTypeDescriptor.CreatePropertyDescriptor(this, EntityType.NameDomainPropertyId).SetValue(this, value);
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
				retVal.SetData(typeof(EntityType), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return BarkerErModel; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
		/// <summary>
		/// Return the name of the entity for its ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}
	}
	#endregion
	#region BinaryAssociation answers
	partial class BinaryAssociation : IAnswerSurveyQuestion<SurveyBarkerElementType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyBarkerElementType> Members
		int IAnswerSurveyQuestion<SurveyBarkerElementType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyBarkerElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyBarkerElementType.BinaryAssociation;
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, BinaryAssociation.NumberDomainPropertyId).IsReadOnly;
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
				return string.Format("Association #{0}",  this.Number.ToString(CultureInfo.CurrentCulture));
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
				return this.Number.ToString(CultureInfo.CurrentCulture);
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, BinaryAssociation.NumberDomainPropertyId).SetValue(this, int.Parse(value));
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
				retVal.SetData(typeof(BinaryAssociation), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return BarkerErModel; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region ExclusiveArc answers
	partial class ExclusiveArc : IAnswerSurveyQuestion<SurveyBarkerElementType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyBarkerElementType> Members
		int IAnswerSurveyQuestion<SurveyBarkerElementType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyBarkerElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyBarkerElementType.Constraint;
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, ExclusiveArc.NumberDomainPropertyId).IsReadOnly;
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
				return this.Number.ToString(CultureInfo.CurrentCulture);
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
				return this.Number.ToString(CultureInfo.CurrentCulture);
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, ExclusiveArc.NumberDomainPropertyId).SetValue(this, int.Parse(value));
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
				retVal.SetData(typeof(ExclusiveArc), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return BarkerErModel; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region Attribute answers
	partial class Attribute : IAnswerSurveyQuestion<SurveyEntityChildType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyEntityChildType> Members
		int IAnswerSurveyQuestion<SurveyEntityChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyEntityChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyEntityChildType.Attribute;
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, Attribute.NameDomainPropertyId).IsReadOnly;
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
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Attribute.NameDomainPropertyId).SetValue(this, value);
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
				retVal.SetData(typeof(Attribute), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return EntityType; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region EntityTypeIsSubtypeOfEntityType answers
	partial class EntityTypeIsSubtypeOfEntityType : IAnswerSurveyQuestion<SurveyEntityChildType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyEntityChildType> Members
		int IAnswerSurveyQuestion<SurveyEntityChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyEntityChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyEntityChildType.Subtype;
		}

		#endregion
		#region ISurveyNode Members

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
				return false;
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
				return this.Subtype.Name;
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
				return this.Subtype.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this.Subtype, EntityType.NameDomainPropertyId).SetValue(this.Subtype, value);
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
				retVal.SetData(typeof(EntityType), this.Subtype);
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
		public static readonly object SurveyExpansionKey = null;
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return Supertype; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region EntityTypePlaysRole answers
	partial class EntityTypePlaysRole : IAnswerSurveyQuestion<SurveyEntityChildType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyEntityChildType> Members
		int IAnswerSurveyQuestion<SurveyEntityChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyEntityChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
				return (int)SurveyEntityChildType.RoleRef;
		}

		#endregion
		#region ISurveyNode Members

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
				return false;
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
				return string.Format("{0} (association #{1})", this.Role.PredicateText, this.Role.BinaryAssociation.Number);
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
				return this.Role.PredicateText;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this.Role, Role.PredicateTextDomainPropertyId).SetValue(this.Role, value);
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
				retVal.SetData(typeof(Role), this.Role);
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
		public static readonly object SurveyExpansionKey = null;
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return EntityType; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region ExclusiveArcSpansOptionalRole answers
	partial class ExclusiveArcSpansOptionalRole : IAnswerSurveyQuestion<SurveyExclusiveArcChildType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyExclusiveArcChildType> Members
		int IAnswerSurveyQuestion<SurveyExclusiveArcChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyExclusiveArcChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyExclusiveArcChildType.RoleRef;
		}

		#endregion
		#region ISurveyNode Members

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
				return false;
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
				return string.Format("{0} (association #{1})", this.ConstrainedRole.PredicateText, this.ConstrainedRole.BinaryAssociation.Number);
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
				return this.ConstrainedRole.PredicateText;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this.ConstrainedRole, Role.PredicateTextDomainPropertyId).SetValue(this.ConstrainedRole, value);
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
				retVal.SetData(typeof(Role), this.ConstrainedRole);
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
		public static readonly object SurveyExpansionKey = null;
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return ExclusiveArc; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
	}
	#endregion
	#region Value answers
	partial class Value : ISurveyNode, IAnswerSurveyQuestion<SurveyAttributeChildType>
	{
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, Value.ValDomainPropertyId).IsReadOnly;
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
				return this.Val;
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
				return this.Val;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Value.ValDomainPropertyId).SetValue(this, value);
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
				retVal.SetData(typeof(Value), this);
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
		public static readonly object SurveyExpansionKey = null;
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

		#endregion
		#region IAnswerSurveyQuestion<SurveyAttributeChildType> Members
		int IAnswerSurveyQuestion<SurveyAttributeChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyAttributeChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyAttributeChildType.PossibleValue;
		}

		#endregion
	}
	#endregion
	#region Role answers
	partial class Role : IAnswerSurveyQuestion<SurveyBinaryAssociationChildType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyBinaryAssociationChildType> Members
		int IAnswerSurveyQuestion<SurveyBinaryAssociationChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyBinaryAssociationChildType}.AskQuestion"/>
		/// </summary>
		protected int AskQuestion(object contextElement)
		{
			bool isMandatory = this.IsMandatory;
			bool isMultiValued = this.IsMultiValued;
			bool isPrimaryIdComponent = this.IsPrimaryIdComponent;
			if (isMandatory)
			{
				if (isMultiValued)
				{
					return (int)(isPrimaryIdComponent ? SurveyBinaryAssociationChildType.Role_MandatoryManyPrimary : SurveyBinaryAssociationChildType.Role_MandatoryMany);
				}
				else
				{
					return (int)(isPrimaryIdComponent ? SurveyBinaryAssociationChildType.Role_MandatoryOnePrimary : SurveyBinaryAssociationChildType.Role_MandatoryOne);
				}
			}
			else // !isMandatory
			{
				return (int)(isMultiValued ? SurveyBinaryAssociationChildType.Role_OptionalMany : SurveyBinaryAssociationChildType.Role_OptionalOne);
			}
		}

		#endregion
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, Role.PredicateTextDomainPropertyId).IsReadOnly;
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
				return this.PredicateText;
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
				return this.PredicateText;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Role.PredicateTextDomainPropertyId).SetValue(this, value);
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
				retVal.SetData(typeof(Role), this);
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

		#endregion
		#region ISurveyNodeContext Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeContext.SurveyNodeContext"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get { return BinaryAssociation; }
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion
		/// <summary>
		/// Return the predicate text and the entity playing the role
		/// for the ToString() of the Role
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0} ({1})", PredicateText, EntityType);
		}
	}
	#endregion // BarkerDomainModel answers
	#region CardinalityQualifier answers
	partial class CardinalityQualifier : ISurveyNode, IAnswerSurveyQuestion<SurveyRoleChildType>
	{
		#region ISurveyNode Members

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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, CardinalityQualifier.NumberDomainPropertyId).IsReadOnly;
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
				return string.Format("{0} {1}", Utility.GetLocalizedEnumName(this.Operator), this.Number);
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
				return this.Number.ToString(CultureInfo.CurrentCulture);
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, CardinalityQualifier.NumberDomainPropertyId).SetValue(this, int.Parse(value));
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
				retVal.SetData(typeof(CardinalityQualifier), this);
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
		public static readonly object SurveyExpansionKey = null;//new object();
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

		#endregion
		#region IAnswerSurveyQuestion<SurveyRoleChildType> Members
		int IAnswerSurveyQuestion<SurveyRoleChildType>.AskQuestion(object contextElement)
		{
			return AskQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyRoleChildType}.AskQuestion"/>
		/// </summary>
		protected static int AskQuestion(object contextElement)
		{
			return (int)SurveyRoleChildType.CardinalityQualifier;
		}

		#endregion
	}
	#endregion
	#endregion
	#region NODE PROVIDER
	partial class BarkerDomainModel : ISurveyNodeProvider, IModelingEventSubscriber
	{
		#region ISurveyNodeProvider Members
		/// <summary>
		/// Implements <see cref="ISurveyNodeProvider.GetSurveyNodes"/>
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes(object context, object expansionKey)
		{
			if (expansionKey == null)
			{
				IElementDirectory elementDirectory = Store.ElementDirectory;
				foreach (BarkerErModel model in elementDirectory.FindElements<BarkerErModel>(true))
				{
					yield return model;
				}
			}
			else if (expansionKey == BarkerErModel.SurveyExpansionKey)
			{
				BarkerErModel model = context as BarkerErModel;
				if (model != null)
				{
					foreach (EntityType entity in model.EntityTypeCollection)
					{
						yield return entity;
					}
					foreach (BinaryAssociation association in model.BinaryAssociationCollection)
					{
						yield return association;
					}
					foreach (ExclusiveArc arc in model.ExclusiveArcCollection)
					{
						yield return arc;
					}
				}
			}
			else if (expansionKey == EntityType.SurveyExpansionKey)
			{
				EntityType entity = context as EntityType;
				if (entity != null)
				{
					foreach (Attribute attribute in entity.AttributeCollection)
					{
						yield return attribute;
					}
					foreach (EntityTypeIsSubtypeOfEntityType subtypeRef in EntityTypeIsSubtypeOfEntityType.GetLinksToSubtypesCollection(entity))
					{
						yield return subtypeRef;
					}
					foreach (EntityTypePlaysRole roleRef in EntityTypePlaysRole.GetLinksToRoleCollection(entity))
					{
						yield return roleRef;
					}
				}
			}
			else if (expansionKey == BinaryAssociation.SurveyExpansionKey)
			{
				BinaryAssociation association = context as BinaryAssociation;
				if (association != null)
				{
					foreach (Role role in association.RoleCollection)
					{
						yield return role;
					}
				}
			}
			else if (expansionKey == Attribute.SurveyExpansionKey)
			{
				Attribute attribute = context as Attribute;
				if (attribute != null)
				{
					foreach (Value val in attribute.PossibleValuesCollection)
					{
						yield return val;
					}
				}
			}
			else if (expansionKey == Role.SurveyExpansionKey)
			{
				Role role = context as Role;
				if (role != null)
				{
					if (role.CardinalityQualifier != null)
					{
						yield return role.CardinalityQualifier;
					}
				}
			}
			else if (expansionKey == ExclusiveArc.SurveyExpansionKey)
			{
				ExclusiveArc arc = context as ExclusiveArc;
				if (arc != null)
				{
					foreach (ExclusiveArcSpansOptionalRole roleRef in ExclusiveArcSpansOptionalRole.GetLinksToRoleCollection(arc))
					{
						yield return roleRef;
					}
				}
			}

		}
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes(object context, object expansionKey)
		{
			return GetSurveyNodes(context, expansionKey);
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeProvider.IsSurveyNodeExpandable"/>
		/// </summary>
		protected static bool IsSurveyNodeExpandable(object context, object expansionKey)
		{
			return expansionKey == BarkerErModel.SurveyExpansionKey ||
				expansionKey == EntityType.SurveyExpansionKey ||
				expansionKey == BinaryAssociation.SurveyExpansionKey ||
				expansionKey == Attribute.SurveyExpansionKey ||
				expansionKey == Role.SurveyExpansionKey ||
				expansionKey == ExclusiveArc.SurveyExpansionKey;
		}
		bool ISurveyNodeProvider.IsSurveyNodeExpandable(object context, object expansionKey)
		{
			return IsSurveyNodeExpandable(context, expansionKey);
		}
		#endregion
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>
		/// </summary>
		protected void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if (0 == (reasons & EventSubscriberReasons.SurveyQuestionEvents))
			{
				return;
			}

			Store store = this.Store;
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			DomainClassInfo classInfo;
			DomainPropertyInfo propertyInfo;

			// Standard handlers defined
			EventHandler<ElementDeletedEventArgs> removeHandler = new EventHandler<ElementDeletedEventArgs>(ElementRemoved);
			EventHandler<ElementPropertyChangedEventArgs> renameHandler = new EventHandler<ElementPropertyChangedEventArgs>(ElementRenamed);

			// Model elements (top level)
			classInfo = dataDir.FindDomainClass(BarkerErModel.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(BarkerErModel.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);

			// Entity Type events
			classInfo = dataDir.FindDomainClass(EntityType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(EntityType.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(EntityNameChanged), action);
			classInfo = dataDir.FindDomainRelationship(BarkerErModelContainsEntityType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(EntityTypeAdded), action);

			// Role events
			classInfo = dataDir.FindDomainClass(Role.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleChanged), action);
			classInfo = dataDir.FindDomainRelationship(BinaryAssociationContainsRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RoleAdded), action);

			// BinaryAssociation events
			classInfo = dataDir.FindDomainClass(BinaryAssociation.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(BinaryAssociation.NumberDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			classInfo = dataDir.FindDomainRelationship(BarkerErModelContainsBinaryAssociation.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(BinaryAssociationAdded), action);

			// Attribute events
			classInfo = dataDir.FindDomainClass(Attribute.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(Attribute.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			classInfo = dataDir.FindDomainRelationship(EntityTypeHasAttribute.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(AttributeAdded), action);

			// Value events
			classInfo = dataDir.FindDomainClass(Value.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(Value.ValDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			classInfo = dataDir.FindDomainRelationship(AttributeHasPossibleValue.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueAdded), action);

			// CardinalityQualifier events
			classInfo = dataDir.FindDomainClass(CardinalityQualifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(CardinalityQualifier.NumberDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			propertyInfo = dataDir.FindDomainProperty(CardinalityQualifier.OperatorDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			classInfo = dataDir.FindDomainRelationship(RoleHasCardinalityQualifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(CardinalityQualifierAdded), action);

			// ExclusiveArc events
			eventManager.AddOrRemoveHandler(classInfo, removeHandler, action);
			propertyInfo = dataDir.FindDomainProperty(ExclusiveArc.NumberDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, renameHandler, action);
			classInfo = dataDir.FindDomainRelationship(BarkerErModelContainsExclusiveArc.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ExclusiveArcAdded), action);
		}
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			ManageModelingEventHandlers(eventManager, reasons, action);
		}
		#endregion // IModelingEventSubscriber Implementation
		#region SurveyQuestion event handlers

		//TODO right now all these handlers look the same regardless of type - should we refactor or will we eventually need different handlers?

		#region Standard handlers
		/// <summary>
		/// This will work for almost all delete scenarios
		/// </summary>
		private static void ElementRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element);
			}
		}
		/// <summary>
		/// An element has been renamed
		/// </summary>
		private static void ElementRenamed(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
			}
		}
		#endregion // Standard handlers
		#region Add handlers
		private static void ModelAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementAdded(element, null);
			}
		}
		private static void EntityTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				BarkerErModelContainsEntityType link = (BarkerErModelContainsEntityType)e.ModelElement;
				eventNotify.ElementAdded(link.EntityType, link.BarkerErModel);
			}
		}
		private static void RoleAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				BinaryAssociationContainsRole link = (BinaryAssociationContainsRole)e.ModelElement;
				eventNotify.ElementAdded(link.Role, link.BinaryAssociation);
			}
		}
		private static void BinaryAssociationAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				BarkerErModelContainsBinaryAssociation link = (BarkerErModelContainsBinaryAssociation)e.ModelElement;
				eventNotify.ElementAdded(link.BinaryAssociation, link.BarkerErModel);
			}
		}
		private static void AttributeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				EntityTypeHasAttribute link = (EntityTypeHasAttribute)e.ModelElement;
				eventNotify.ElementAdded(link.Attribute, link.EntityType);
			}
		}
		private static void ValueAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				AttributeHasPossibleValue link = (AttributeHasPossibleValue)e.ModelElement;
				eventNotify.ElementAdded(link.Value, link.Attribute);
			}
		}
		private static void CardinalityQualifierAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				
			}
		}
		private static void ExclusiveArcAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				BarkerErModelContainsExclusiveArc link = (BarkerErModelContainsExclusiveArc)e.ModelElement;
				eventNotify.ElementAdded(link.ExclusiveArc, link.BarkerErModel);
			}
		}
		#endregion // Add Handlers
		#region Change handlers
		private static readonly Type[] SurveyBinaryAssociationChildQuestionTypes = new Type[] { typeof(SurveyBinaryAssociationChildType) };
		/// <summary>
		/// A role has changed, notify that the provided survey answers have changed
		/// </summary>
		private static void RoleChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == Role.IsMandatoryDomainPropertyId ||
					attributeId == Role.IsPrimaryIdComponentDomainPropertyId ||
					attributeId == Role.IsMultiValuedDomainPropertyId)
				{
					eventNotify.ElementChanged(element, SurveyBinaryAssociationChildQuestionTypes);
				}
				else if (attributeId == Role.PredicateTextDomainPropertyId)
				{
					eventNotify.ElementRenamed(element);
					Role role = (Role)element;
					EntityTypePlaysRole entityLink = EntityTypePlaysRole.GetLinkToEntityType(role);
					if (entityLink != null)
					{
						eventNotify.ElementRenamed(entityLink);
					}
					ExclusiveArcSpansOptionalRole exclusiveArcLink = ExclusiveArcSpansOptionalRole.GetLinkToExclusiveArc(role);
					if (exclusiveArcLink != null)
					{
						eventNotify.ElementRenamed(exclusiveArcLink);
					}
				}
			}
		}
		/// <summary>
		/// An entity name has changed, notify all possible display points for the name
		/// </summary>
		private static void EntityNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
				EntityType entity = (EntityType)element;
				EntityTypeIsSubtypeOfEntityType superTypeLink = EntityTypeIsSubtypeOfEntityType.GetLinkToSupertype(entity);
				if (superTypeLink != null)
				{
					eventNotify.ElementRenamed(superTypeLink);
				}
			}
		}
		#endregion // Change handlers

		#endregion // SurveyQuestion event handlers
	}
	#endregion
}
