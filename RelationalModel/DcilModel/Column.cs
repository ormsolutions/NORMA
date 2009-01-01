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
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	partial class Column
	{
		#region Guids for roles and objects leading back to the column's value type
		/// <summary>
		/// ConceptTypeChildHasPathFactType.ConceptTypeChild domain role Id.
		/// </summary>
		private static readonly global::System.Guid TowardsFactTypeRoleId = new global::System.Guid(0x309ecb8e, 0x8840, 0x48fb, 0x95, 0x91, 0xef, 0x74, 0xcc, 0x1b, 0x80, 0x5c);
		/// <summary>
		/// FactTypeMapsTowardsRole.FactType domain role Id.
		/// </summary>
		private static readonly global::System.Guid TowardsRoleRoleId = new global::System.Guid(0x05abb37c, 0x0363, 0x4bd5, 0xbe, 0x02, 0xf6, 0x88, 0xbf, 0xdb, 0x55, 0xa5);
		/// <summary>
		/// InformationType.ConceptType domain role Id.
		/// </summary>
		private static readonly global::System.Guid InformationTypeConceptTypeDomainRoleId = new global::System.Guid(0x88eca698, 0xb81f, 0x49a5, 0x99, 0x45, 0xe3, 0xa3, 0x66, 0x97, 0x17, 0x7b);
		/// <summary>
		/// InformationType domain class Id.
		/// </summary>
		private static readonly global::System.Guid InformationTypeDomainClassId = new global::System.Guid(0x10dbc480, 0x9dd5, 0x47fb, 0x85, 0x33, 0x98, 0x2c, 0x27, 0x98, 0x5e, 0xe5);
		/// <summary>
		/// ConceptType domain role Id.
		/// </summary>
		private static readonly global::System.Guid ConceptTypeIsForObjectTypeConceptTypeDomainRoleId = new global::System.Guid(0xee5f768c, 0xb308, 0x480e, 0xa4, 0x44, 0xf8, 0x6f, 0x81, 0xb0, 0x2f, 0x46);
		#endregion // Guids for roles and objects leading back to the column's value type
		#region Custom Storage handlers
		private DataType GetDataTypeValue()
		{
			ObjectType valueType = AssociatedValueType;
			return (valueType != null) ? valueType.DataType : null;
		}
		private void SetDataTypeValue(object value)
		{
			if (Store.TransactionActive)
			{
				ObjectType valueType = AssociatedValueType;
				if (valueType != null)
				{
					valueType.DataType = (DataType)value;
				}
			}
		}
		private int GetDataTypeLengthValue()
		{
			ObjectType valueType = AssociatedValueType;
			return (valueType != null) ? valueType.DataTypeLength : 0;
		}
		private void SetDataTypeLengthValue(int value)
		{
			if (Store.TransactionActive)
			{
				ObjectType valueType = AssociatedValueType;
				if (valueType != null)
				{
					valueType.DataTypeLength = value;
				}
			}
		}
		private int GetDataTypeScaleValue()
		{
			ObjectType valueType = AssociatedValueType;
			return (valueType != null) ? valueType.DataTypeScale : 0;
		}
		private void SetDataTypeScaleValue(int value)
		{
			if (Store.TransactionActive)
			{
				ObjectType valueType = AssociatedValueType;
				if (valueType != null)
				{
					valueType.DataTypeScale = value;
				}
			}
		}
		#endregion // Custom Storage handlers
		#region Helper methods
		/// <summary>
		/// Get the <see cref="ObjectType">ValueType</see> associated with this <see cref="Column"/>
		/// </summary>
		public ObjectType AssociatedValueType
		{
			get
			{
				ObjectType retVal = null;
				DomainRoleInfo roleInfo;
				LinkedElementCollection<ModelElement> oppositeElements;
				RoleBase targetRoleBase;
				Role targetRole;
				int elementCount;
				DomainDataDirectory dataDir = Store.DomainDataDirectory;
				// Get the ConceptTypeChild elements associated with this column
				if (null != (roleInfo = dataDir.FindDomainRole(ColumnBridgeRoleId)) &&
					0 != (elementCount = (oppositeElements = roleInfo.GetLinkedElements(this)).Count) &&
					// Get the FactType elements. If there is no FactType path available, then this
					// is in a table that was created for a value type
					null != (roleInfo = dataDir.FindDomainRole(TowardsFactTypeRoleId)))
				{
					ModelElement conceptTypeChild;
					ModelElement conceptType;
					if (0 != (elementCount = (oppositeElements = roleInfo.GetLinkedElements(conceptTypeChild = oppositeElements[elementCount - 1])).Count) &&
						// Get the role we're mapping towards
						null != (roleInfo = dataDir.FindDomainRole(TowardsRoleRoleId)) &&
						null != (targetRoleBase = roleInfo.GetLinkedElement(oppositeElements[elementCount - 1]) as RoleBase) &&
						null != (targetRoleBase = targetRoleBase.OppositeRole) &&
						null != (targetRole = targetRoleBase.Role))
					{
						retVal = targetRole.RolePlayer;
						if (!retVal.IsValueType)
						{
							retVal = null;
						}
					}
					else if (conceptTypeChild.GetDomainClass().Id == InformationTypeDomainClassId &&
						null != (conceptType = DomainRoleInfo.GetRolePlayer((ElementLink)conceptTypeChild, InformationTypeConceptTypeDomainRoleId)) &&
						null != (roleInfo = dataDir.FindDomainRole(ConceptTypeIsForObjectTypeConceptTypeDomainRoleId)) &&
						null != (retVal = roleInfo.GetLinkedElement(conceptType) as ObjectType))
					{
						if (!retVal.IsValueType)
						{
							retVal = null;
						}
					}
				}
				return retVal;
			}
		}
		#endregion // Helper methods
	}
}
