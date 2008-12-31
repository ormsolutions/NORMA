#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.RelationalModels.ConceptualDatabase;

namespace Neumont.Tools.RelationalModels.ConceptualDatabase.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ObjectType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ColumnTypeDescriptor<TModelElement> : ConceptualDatabaseElementTypeDescriptor<TModelElement>
		where TModelElement : Column
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ColumnTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ColumnTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
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
			else if (propertyId == Column.LengthDomainPropertyId)
			{
				return null != (valueType = column.AssociatedValueType) && valueType.DataType.LengthName != null;
			}
			else if (propertyId == Column.ScaleDomainPropertyId)
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
			if (propertyId == Column.ScaleDomainPropertyId)
			{
				if (null != (column = propertyDescriptor.ModelElement as Column) &&
					null != (valueType = column.AssociatedValueType) &&
					!string.IsNullOrEmpty(displayName = valueType.DataType.ScaleName))
				{
					return displayName;
				}
			}
			else if (propertyId == Column.LengthDomainPropertyId)
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
			if (propertyId == Column.ScaleDomainPropertyId)
			{
				passthroughPropertyId = ObjectType.ScaleDomainPropertyId;
			}
			else if (propertyId == Column.LengthDomainPropertyId)
			{
				passthroughPropertyId = ObjectType.LengthDomainPropertyId;
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
	}
}
