using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class ObjectType : INamedElementDictionaryChild
	{
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in ObjectTypeChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == IsValueTypeMetaAttributeGuid ||
				attributeGuid == ScaleMetaAttributeGuid ||
				attributeGuid == LengthMetaAttributeGuid ||
				attributeGuid == NestedFactTypeDisplayMetaAttributeGuid)
			{
				// Handled by ObjectTypeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == IsValueTypeMetaAttributeGuid)
			{
				return this.DataType != null;
			}
			else if (attributeGuid == ObjectType.ScaleMetaAttributeGuid)
			{
				ValueTypeHasDataType link = GetDataTypeLink();
				return (link == null) ? 0 : link.Scale;
			}
			else if (attributeGuid == ObjectType.LengthMetaAttributeGuid)
			{
				ValueTypeHasDataType link = GetDataTypeLink();
				return (link == null) ? 0 : link.Length;
			}
			else if (attributeGuid == ObjectType.NestedFactTypeDisplayMetaAttributeGuid)
			{
				return NestedFactType;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Return the link object between a value type and its referenced
		/// data type object.
		/// </summary>
		/// <returns>ValueTypeHasDataType relationship</returns>
		public ValueTypeHasDataType GetDataTypeLink()
		{
			ElementLink goodLink = null;
			System.Collections.IList links = GetElementLinks(ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid);
			foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
			{
				if (!link.IsRemoved)
				{
					goodLink = link;
					break;
				}
			}
			return goodLink as ValueTypeHasDataType;
		}
		/// <summary>
		/// Standard override determine when derived attributes are
		/// displayed in the property grid. Called for all attributes.
		/// </summary>
		/// <param name="metaAttrInfo">MetaAttributeInfo</param>
		/// <returns></returns>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeGuid = metaAttrInfo.Id;
			if (attributeGuid == TypeNameMetaAttributeGuid ||
				attributeGuid == ScaleMetaAttributeGuid ||
				attributeGuid == LengthMetaAttributeGuid)
			{
				return IsValueType;
			}
			else if (attributeGuid == NestedFactTypeDisplayMetaAttributeGuid)
			{
				return !IsValueType && PreferredIdentifier == null;
			}
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		/// <summary>
		/// Standard override. Determines when derived properties are read-only. Called
		/// if the ReadOnly setting on the element is one of the SometimesUIReadOnly* values.
		/// Currently, IsValueType is readonly if there is a nested fact type.
		/// </summary>
		/// <param name="propertyDescriptor">PropertyDescriptor</param>
		/// <returns></returns>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor elemDesc = propertyDescriptor as ElementPropertyDescriptor;
			if (elemDesc != null && elemDesc.MetaAttributeInfo.Id == IsValueTypeMetaAttributeGuid)
			{
				return NestedFactType != null || PreferredIdentifier != null;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // CustomStorage handlers
		#region Customize property display
		/// <summary>
		/// Distinguish between a value type and object
		/// type in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			return IsValueType ? ResourceStrings.ValueType : ResourceStrings.EntityType;
		}
		#endregion // Customize property display
		#region ObjectTypeChangeRule class
		[RuleOn(typeof(ObjectType))]
		private class ObjectTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// Add or remove a ValueTypeHasDataType link depending on the value
			/// of the IsValueType property.
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ObjectType.IsValueTypeMetaAttributeGuid)
				{
					bool newValue = (bool)e.NewValue;
					DataType dataType = null;
					if (newValue)
					{
						dataType = DataType.CreateDataType(e.ModelElement.Store);
					}
					(e.ModelElement as ObjectType).DataType = dataType;
				}
				else if (attributeGuid == ObjectType.ScaleMetaAttributeGuid)
				{
					ValueTypeHasDataType link = (e.ModelElement as ObjectType).GetDataTypeLink();
					// No effect for non-value types
					if (link != null)
					{
						link.Scale = (int)e.NewValue;
					}
				}
				else if (attributeGuid == ObjectType.LengthMetaAttributeGuid)
				{
					ValueTypeHasDataType link = (e.ModelElement as ObjectType).GetDataTypeLink();
					// No effect for non-value types
					if (link != null)
					{
						link.Length = (int)e.NewValue;
					}
				}
				else if (attributeGuid == ObjectType.NestedFactTypeDisplayMetaAttributeGuid)
				{
					(e.ModelElement as ObjectType).NestedFactType = e.NewValue as FactType;
				}
			}
		}
		#endregion // ObjectTypeChangeRule class
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasObjectType.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region CheckForIncompatibleRelationshipRule class
		/// <summary>
		/// Ensure consistency among relationships attached to ObjectType roles.
		/// This is an object model backup for the UI, which does not offer these
		/// conditions to the user.
		/// </summary>
		[RuleOn(typeof(NestingEntityTypeHasFactType)), RuleOn(typeof(ValueTypeHasDataType)), RuleOn(typeof(ObjectTypePlaysRole))]
		private class CheckForIncompatibleRelationshipRule : AddRule
		{
			/// <summary>
			/// Called when an attempt is made to turn an ObjectType into either
			/// a value type or a nesting type.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				NestingEntityTypeHasFactType nester;
				ValueTypeHasDataType valType;
				ObjectTypePlaysRole roleLink;
				FactTypeHasRole newRole;
				ModelElement element = e.ModelElement;
				bool incompatibleValueTypeCombination = false;
				bool incompatibleNestingAndRoleCombination = false;
				// Note that the other portion of this condition is
				// checked in a separate add rule for EntityTypeHasPreferredIdentifier
				bool incompatiblePreferredIdentifierCombination = false;
				if (null != (nester = element as NestingEntityTypeHasFactType))
				{
					ObjectType nestingType = nester.NestingType;
					if (!(incompatibleValueTypeCombination = nestingType.IsValueType) &&
						!(incompatiblePreferredIdentifierCombination = null != nestingType.PreferredIdentifier))
					{
						foreach (Role role in nester.NestedFactType.RoleCollection)
						{
							if (role.RolePlayer == nestingType)
							{
								incompatibleNestingAndRoleCombination = true;
								break;
							}
						}
					}
				}
				else if (null != (valType = element as ValueTypeHasDataType))
				{
					if (!(incompatibleValueTypeCombination = valType.ValueTypeCollection.NestedFactType != null))
					{
						incompatiblePreferredIdentifierCombination = null != valType.ValueTypeCollection.PreferredIdentifier;
					}
				}
				else if (null != (roleLink = element as ObjectTypePlaysRole))
				{
					FactType fact = roleLink.PlayedRoleCollection.FactType;
					if (fact != null)
					{
						incompatibleNestingAndRoleCombination = fact.NestingType == roleLink.RolePlayer;
					}
				}
				else if (null != (newRole = element as FactTypeHasRole))
				{
					ObjectType player = newRole.RoleCollection.RolePlayer;
					if (player != null)
					{
						incompatibleNestingAndRoleCombination = player == newRole.FactType.NestingType;
					}
				}

				// Raise an exception if any of the objectype-linked relationship
				// combinations are invalid
				string exceptionString = null;
				if (incompatibleValueTypeCombination)
				{
					exceptionString = ResourceStrings.ModelExceptionEnforceValueTypeNotNestingType;
				}
				else if (incompatibleNestingAndRoleCombination)
				{
					exceptionString = ResourceStrings.ModelExceptionEnforceRolePlayerNotNestingType;
				}
				else if (incompatiblePreferredIdentifierCombination)
				{
					exceptionString = ResourceStrings.ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType;
				}
				if (exceptionString != null)
				{
					throw new InvalidOperationException(exceptionString);
				}
			}
			/// <summary>
			/// Fire early. There is no reason to put this in the transaction log
			/// if it isn't valid.
			/// </summary>
			public override bool FireBefore
			{
				get
				{
					return true;
				}
			}
		}
		#endregion // CheckForIncompatibleRelationshipRule class
	}
} 