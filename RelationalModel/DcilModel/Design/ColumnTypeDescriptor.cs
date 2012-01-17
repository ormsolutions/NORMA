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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;

namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ObjectType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ColumnTypeDescriptor<TModelElement> : ConceptualDatabaseElementTypeDescriptor<TModelElement>
		where TModelElement : Column
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ColumnTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ColumnTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Create property descriptors that only allow merging of DataType facet properties
		/// when the <see cref="P:ObjectType.DataType"/> instances are equal.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			Guid propertyId = domainPropertyInfo.Id;
			if (propertyId == Column.NameDomainPropertyId)
			{
				return new NamePropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			else if (propertyId == Column.DataTypeLengthDomainPropertyId || propertyId == Column.DataTypeScaleDomainPropertyId)
			{
				return new MatchDataTypePropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			return base.CreatePropertyDescriptor(requestor, domainPropertyInfo, attributes);
		}
		/// <summary>
		/// Disallow changing data types for columns based on unary predicates. These must remain boolean.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Column column;
			ObjectType valueType;
			if (propertyDescriptor.DomainPropertyInfo.Id == Column.DataTypeDomainPropertyId &&
				null != (column = propertyDescriptor.ModelElement as Column) &&
				null != (valueType = column.AssociatedValueType) &&
				valueType.IsImplicitBooleanValue)
			{
				return true;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Column column = ModelElement;
			Guid propertyId = domainProperty.Id;
			ObjectType valueType;
			if (propertyId == Column.DataTypeDomainPropertyId)
			{
				return null != column.AssociatedValueType;
			}
			else if (propertyId == Column.DataTypeLengthDomainPropertyId)
			{
				return null != (valueType = column.AssociatedValueType) && valueType.DataType.LengthName != null;
			}
			else if (propertyId == Column.DataTypeScaleDomainPropertyId)
			{
				return null != (valueType = column.AssociatedValueType) && valueType.DataType.ScaleName != null;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}
		/// <summary>
		/// Get custom display names for the Scale and Length properties
		/// </summary>
		protected override string GetPropertyDescriptorDisplayName(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			Column column;
			ObjectType valueType;
			string displayName;
			if (propertyId == Column.DataTypeScaleDomainPropertyId)
			{
				if (null != (column = propertyDescriptor.ModelElement as Column) &&
					null != (valueType = column.AssociatedValueType) &&
					!string.IsNullOrEmpty(displayName = valueType.DataType.ScaleName))
				{
					return displayName;
				}
			}
			else if (propertyId == Column.DataTypeLengthDomainPropertyId)
			{
				if (null != (column = propertyDescriptor.ModelElement as Column) &&
					null != (valueType = column.AssociatedValueType) &&
					!string.IsNullOrEmpty(displayName = valueType.DataType.LengthName))
				{
					return displayName;
				}
			}
			return base.GetPropertyDescriptorDisplayName(propertyDescriptor);
		}
		/// <summary>
		/// Provide description information for the passthrough DataType properties
		/// </summary>
		protected override string GetDescription(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			Guid passthroughPropertyId;
			if (propertyId == Column.DataTypeScaleDomainPropertyId)
			{
				passthroughPropertyId = ObjectType.DataTypeScaleDomainPropertyId;
			}
			else if (propertyId == Column.DataTypeLengthDomainPropertyId)
			{
				passthroughPropertyId = ObjectType.DataTypeLengthDomainPropertyId;
			}
			else
			{
				return base.GetDescription(propertyDescriptor);
			}
			Column column;
			ObjectType valueType;
			if (null != (column = propertyDescriptor.ModelElement as Column) &&
				null != (valueType = column.AssociatedValueType))
			{
				return DomainTypeDescriptor.CreatePropertyDescriptor(valueType, valueType.Store.DomainDataDirectory.GetDomainProperty(passthroughPropertyId)).Description;
			}
			return base.GetDescription(propertyDescriptor);
		}
		#endregion // Base overrides
		#region MatchDataTypePropertyDescriptor class
		/// <summary>
		/// An element property descriptor that merges DataType facet properties only if the
		/// DataTypes of the multi-selected elements match.
		/// </summary>
		private sealed class MatchDataTypePropertyDescriptor : ElementPropertyDescriptor
		{
			public MatchDataTypePropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes)
				: base(owner, modelElement, domainProperty, attributes)
			{
			}
			/// <summary>
			/// Allow equality only if the opposite element has the same DataType
			/// </summary>
			public override bool Equals(object obj)
			{
				bool retVal = base.Equals(obj);
				if (retVal)
				{
					MatchDataTypePropertyDescriptor oppositeDescriptor = obj as MatchDataTypePropertyDescriptor;
					Column thisElement;
					Column otherElement;
					if (oppositeDescriptor != null &&
						null != (thisElement = ModelElement as Column) &&
						null != (otherElement = oppositeDescriptor.ModelElement as Column))
					{
						retVal = thisElement.DataType == otherElement.DataType;
					}
				}
				return retVal;
			}
			/// <summary>
			/// Required with Equals override
			/// </summary>
			public override int GetHashCode()
			{
				int retVal = base.GetHashCode();
				Column element;
				DataType dataType;
				if (null != (element = ModelElement as Column) &&
					null != (dataType = element.DataType as DataType))
				{
					retVal ^= dataType.GetHashCode();
				}
				return retVal;
			}
		}
		#endregion // MatchDataTypePropertyDescriptor class
		#region NamePropertyDescriptor class
		/// <summary>
		/// A property descriptor that ties display and reset capabilities
		/// on the column name to both the Name and CustomName properties.
		/// </summary>
		private sealed class NamePropertyDescriptor : ElementPropertyDescriptor
		{
			public NamePropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes)
				: base(owner, modelElement, domainProperty, attributes)
			{
			}
			public override bool CanResetValue(object component)
			{
				return ((Column)this.ModelElement).CustomName;
			}
			public override void ResetValue(object component)
			{
				this.SetValue(component, "");
			}
			public override bool ShouldSerializeValue(object component)
			{
				return ((Column)this.ModelElement).CustomName;
			}
			public override void SetValue(object component, object value)
			{
				Column column;
				Store store;
				string newName;
				if (null == (column = ModelElement as Column) ||
					null == (store = Utility.ValidateStore(column.Store)) ||
					null == (newName = value as string) ||
					newName == column.Name)
				{
					return;
				}
				using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(this.DisplayName)))
				{
					if (string.IsNullOrEmpty(newName))
					{
						// Name generators should listen to this property to regenerate
						// the name.
						column.CustomName = false;
					}
					else
					{
						column.CustomName = true;
						column.Name = newName;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // NamePropertyDescriptor class
	}
}
