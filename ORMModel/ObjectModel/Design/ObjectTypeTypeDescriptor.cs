#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ObjectType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectTypeTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : ObjectType
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ObjectTypeTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}
		/// <summary>
		/// Create property descriptors that only allow merging of DataType facet properties
		/// when the <see cref="P:ObjectType.DataType"/> instances are equal.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			Guid propertyId = domainPropertyInfo.Id;
			if (propertyId == ObjectType.DataTypeLengthDomainPropertyId || propertyId == ObjectType.DataTypeScaleDomainPropertyId)
			{
				return new MatchDataTypePropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			else if (propertyId == ObjectType.ReferenceModeDisplayDomainPropertyId)
			{
				return new AutomatedElementFilterPropertyDescriptor(this, requestor, domainPropertyInfo, attributes);
			}
			return base.CreatePropertyDescriptor(requestor, domainPropertyInfo, attributes);
		}
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
					ObjectType thisElement;
					ObjectType otherElement;
					if (oppositeDescriptor != null &&
						null != (thisElement = ModelElement as ObjectType) &&
						null != (otherElement = oppositeDescriptor.ModelElement as ObjectType))
					{
						retVal = thisElement.DataTypeDisplay == otherElement.DataTypeDisplay;
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
				ObjectType element;
				DataType dataType;
				if (null != (element = ModelElement as ObjectType) &&
					null != (dataType = element.DataTypeDisplay))
				{
					retVal ^= dataType.GetHashCode();
				}
				return retVal;
			}
		}
		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			ObjectType objectType = ModelElement;
			Guid propertyId = domainProperty.Id;
			if (propertyId == ObjectType.DataTypeDisplayDomainPropertyId ||
				propertyId == ObjectType.ValueRangeTextDomainPropertyId)
			{
				return objectType.IsValueType || objectType.HasReferenceMode;
			}
			else if (propertyId == ObjectType.DataTypeScaleDomainPropertyId)
			{
				DataType dataType = GetObjectTypeDataType(objectType);
				return dataType != null && dataType.ScaleName != null;
			}
			else if (propertyId == ObjectType.DataTypeLengthDomainPropertyId)
			{
				DataType dataType = objectType.DataType ?? (objectType.HasReferenceMode ? objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.DataType : null);
				return dataType != null && dataType.LengthName != null;
			}
			else if (propertyId == ObjectType.ValueTypeValueRangeTextDomainPropertyId)
			{
				return objectType.HasReferenceMode;
			}
			else if (propertyId == ObjectType.ReferenceModeDisplayDomainPropertyId)
			{
				return !objectType.IsValueType;
			}
			else if (propertyId == ObjectType.DerivationNoteDisplayDomainPropertyId)
			{
				return objectType.IsSubtype;
			}
			else if (propertyId == ObjectType.DerivationStorageDisplayDomainPropertyId)
			{
				return objectType.DerivationRule != null && objectType.IsSubtype;
			}
			else if (propertyId == ObjectType.IsExternalDomainPropertyId)
			{
				// UNDONE: Support IsExternal
				return false;
			}
			else
			{
				return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
			}
		}
		private static DataType GetObjectTypeDataType(ObjectType objectType)
		{
			return (objectType != null) ?
				(objectType.DataType ?? (objectType.HasReferenceMode ? objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.DataType : null)) :
				null;
		}
		/// <summary>
		/// Get custom display names for the Scale and Length properties
		/// </summary>
		protected override string GetPropertyDescriptorDisplayName(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			DataType dataType;
			string displayName;
			if (propertyId == ObjectType.DataTypeScaleDomainPropertyId)
			{
				if (null != (dataType = GetObjectTypeDataType(propertyDescriptor.ModelElement as ObjectType)) &&
					!string.IsNullOrEmpty(displayName = dataType.ScaleName))
				{
					return displayName;
				}
			}
			else if (propertyId == ObjectType.DataTypeLengthDomainPropertyId)
			{
				if (null != (dataType = GetObjectTypeDataType(propertyDescriptor.ModelElement as ObjectType)) &&
					!string.IsNullOrEmpty(displayName = dataType.LengthName))
				{
					return displayName;
				}
			}
			return base.GetPropertyDescriptorDisplayName(propertyDescriptor);
		}

		/// <summary>
		/// Get custom description names for the Scale and Length properties
		/// </summary>
		/// <param name="propertyDescriptor"></param>
		/// <returns></returns>
		protected override string GetDescription(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			DataType dataType;
			string description;
			if (propertyId == ObjectType.NameDomainPropertyId)
			{
				return ResourceStrings.ObjectTypeNameDescription;
			}
			else if (propertyId == ObjectType.DataTypeScaleDomainPropertyId)
			{
				if (null != (dataType = GetObjectTypeDataType(propertyDescriptor.ModelElement as ObjectType)) &&
					!string.IsNullOrEmpty(description = dataType.ScaleDescription))
				{
					return description;
				}
			}
			else if (propertyId == ObjectType.DataTypeLengthDomainPropertyId)
			{
				if (null != (dataType = GetObjectTypeDataType(propertyDescriptor.ModelElement as ObjectType)) &&
					!string.IsNullOrEmpty(description = dataType.LengthDescription))
				{
					return description;
				}
			}
			return base.GetDescription(propertyDescriptor);
		}

		/// <summary>
		/// Allow <see cref="RolePlayerPropertyDescriptor"/>s if this isn't a ValueType.
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return !this.ModelElement.IsValueType;
		}

		/// <summary>
		/// Only create <see cref="RolePlayerPropertyDescriptor"/>s for <see cref="Objectification.NestedFactType"/>.
		/// </summary>
		protected override bool ShouldCreateRolePlayerPropertyDescriptor(ModelElement sourceRolePlayer, DomainRoleInfo sourceRole)
		{
			return Utility.IsDescendantOrSelf(sourceRole, Objectification.NestingTypeDomainRoleId);
		}

		/// <summary>
		/// Returns an instance of <see cref="ObjectificationRolePlayerPropertyDescriptor"/> for <see cref="Objectification.NestedFactType"/>.
		/// </summary>
		protected override RolePlayerPropertyDescriptor CreateRolePlayerPropertyDescriptor(ModelElement sourceRolePlayer, DomainRoleInfo targetRoleInfo, Attribute[] sourceDomainRoleInfoAttributes)
		{
			if (Utility.IsDescendantOrSelf(targetRoleInfo, Objectification.NestedFactTypeDomainRoleId))
			{
				return new ObjectificationRolePlayerPropertyDescriptor(sourceRolePlayer, targetRoleInfo, sourceDomainRoleInfoAttributes);
			}
			return base.CreateRolePlayerPropertyDescriptor(sourceRolePlayer, targetRoleInfo, sourceDomainRoleInfoAttributes);
		}

		/// <summary>
		/// Distinguish between a value type and an entity type in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			return ModelElement.IsValueType ? ResourceStrings.ValueType : ResourceStrings.EntityType;
		}

		/// <summary>
		/// Ensure that the <see cref="ObjectType.IsValueType"/> property is read-only when
		/// <see cref="ObjectType.NestedFactType"/> is not <see langword="null"/>,
		/// <see cref="ObjectType.PreferredIdentifier"/> is not <see langword="null"/>, or
		/// <see cref="ObjectType.IsSubtypeOrSupertype"/> is <see langword="true"/>.
		/// Ensure that the <see cref="ObjectType.ValueRangeText"/> property is read-only when
		/// <see cref="ObjectType.IsValueType"/> is <see langword="false"/> and
		/// <see cref="ObjectType.HasReferenceMode"/> is <see langword="false"/>.
		/// Ensure that the <see cref="ObjectType.IsIndependent"/> property is read-only when
		/// when <see cref="ObjectType.Objectification"/> is not <see langword="null"/> and
		/// <see cref="Objectification.IsImplied"/> is <see langword="true"/>, or
		/// <see cref="ObjectType.AllowIsIndependent()"/> returns <see langword="false"/>
		/// Ensure that the <see cref="ObjectType.ReferenceModeDisplay"/> property is read-only
		/// when <see cref="ObjectType.Objectification"/> is not <see langword="null"/> and
		/// <see cref="Objectification.IsImplied"/> is <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			ObjectType objectType = ModelElement;
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (objectType.IsImplicitBooleanValue)
			{
				return true;
			}
			else if (propertyId == ObjectType.IsValueTypeDomainPropertyId)
			{
				return objectType.NestedFactType != null || objectType.PreferredIdentifier != null || objectType.IsSubtypeOrSupertype;
			}
			else if (propertyId == ObjectType.ValueRangeTextDomainPropertyId)
			{
				return !(objectType.IsValueType || objectType.HasReferenceMode);
			}
			else if (propertyId == ObjectType.IsIndependentDomainPropertyId)
			{
				if (objectType.IsIndependent)
				{
					Objectification objectification = objectType.Objectification;
					return objectification != null && objectification.IsImplied;
				}
				else
				{
					return !objectType.AllowIsIndependent();
				}
			}
			else if (propertyId == ObjectType.ReferenceModeDisplayDomainPropertyId)
			{
				Objectification objectification = objectType.Objectification;
				return objectification != null && objectification.IsImplied;
			}
			else if (propertyId == ObjectType.TreatAsPersonalDomainPropertyId)
			{
				return objectType.IsSupertypePersonal;
			}
			else
			{
				return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
			}
		}
	}
}
