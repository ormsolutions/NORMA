using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class ObjectType : INamedElementDictionaryChild
	{
		#region Public token values
		/// <summary>
		/// A key to return from INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// if duplicate names should be allowed.
		/// </summary>
		public static readonly object DeleteReferenceModeValueType = new object();
		#endregion // Public token values
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
				attributeGuid == NestedFactTypeDisplayMetaAttributeGuid ||
				attributeGuid == ReferenceModeDisplayMetaAttributeGuid)
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
			else if (attributeGuid == ObjectType.ReferenceModeDisplayMetaAttributeGuid)
			{
				InternalConstraint prefConstraint = this.PreferredIdentifier as InternalConstraint;

				//If there is a preferred internal uniqueness constraint and that uniqueness constraint's role
				// player is a value type then return the refence mode name.
				if (prefConstraint != null)
				{
					ObjectType valueType = prefConstraint.RoleCollection[0].RolePlayer;
					Northface.Tools.ORM.ObjectModel.ReferenceMode refMode = Northface.Tools.ORM.ObjectModel.ReferenceMode.FindReferenceModeFromEnitityNameAndValueName(valueType.Name, this.Name, this.Model);

					if (valueType.IsValueType)
					{
						if (refMode == null)
						{
							return valueType.Name;
						}
						else
						{
							return refMode.Name;
						}
					}
				}
				return "";
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
			else if (attributeGuid == ReferenceModeDisplayMetaAttributeGuid)
			{
				return !IsValueType && NestedFactType == null;
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

		/// <summary>
		/// Enforces Change Rules
		/// </summary>
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
				else if (attributeGuid == ObjectType.NameMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					Store store = objectType.Store;
					InternalUniquenessConstraint prefConstraint = objectType.PreferredIdentifier as InternalUniquenessConstraint;

					if (prefConstraint != null)
					{
						string newValue = (string)e.NewValue;
						string oldValue = (string)e.OldValue;
						string oldReferenceModeName = "";

						ReferenceMode referenceMode = ReferenceMode.FindReferenceModeFromEnitityNameAndValueName(objectType.ReferenceModeDisplay, oldValue, objectType.Model);

						if (referenceMode != null)
						{
							string name = newValue;
							oldReferenceModeName = referenceMode.Name;
							name = referenceMode.GenerateValueTypeName(name);

							if (name != oldReferenceModeName)
							{
								this.RenameReferenceMode(name, objectType, prefConstraint, store);
							}
						}
					}
				}
				else if (attributeGuid == ObjectType.ReferenceModeDisplayMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					Store store = objectType.Store;
					InternalUniquenessConstraint prefConstraint = objectType.PreferredIdentifier as InternalUniquenessConstraint;
					bool aggressivelyKillValueType = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Contains(DeleteReferenceModeValueType);

					string newValue = (string)e.NewValue;
					string oldValue = (string)e.OldValue;

					ICollection<ReferenceMode> referenceModes = ReferenceMode.FindReferenceModesByName(newValue, objectType.Model);

					//TODO: What if we get multiple back?
					string name = newValue;
					foreach (ReferenceMode referenceMode in referenceModes)
					{
						name = referenceMode.GenerateValueTypeName(objectType.Name);
					}

					if (newValue.Length == 0 && oldValue.Length != 0)
					{
						this.KillReferenceMode(prefConstraint, aggressivelyKillValueType);
					}
					else if (newValue.Length != 0 && oldValue.Length != 0)
					{
						this.RenameReferenceMode(name, objectType, prefConstraint, store);
					}
					else if (newValue.Length != 0 && oldValue.Length == 0)
					{
						this.CreateReferenceMode(name, objectType, store);
					}
				}
			}

			#region UtilityMethods
			/// <summary>
			/// Utility function to create the reference mode objects.  Creates the fact, value type, and
			/// preffered internal uniqueness constraint.
			/// </summary>
			private	void CreateReferenceMode(string	valueTypeName, ObjectType objectType, Store	store)
			{				
				ORMModel objModel = objectType.Model;
				ObjectType valueType = FindValueType(valueTypeName, objModel);

				FactType refFact = FactType.CreateFactType(store);
				refFact.Model = objModel;

				if (valueType  == null)
				{
					valueType = ObjectType.CreateObjectType(store);
					valueType.IsValueType =	true;
					valueType.Name = valueTypeName;
					valueType.Model = objModel;
				}

				Role objectTypeRole	= Role.CreateRole(store);
				objectTypeRole.RolePlayer =	objectType;
				RoleMoveableCollection roleCollection =	refFact.RoleCollection;
				roleCollection.Add(objectTypeRole);

				Role valueTypeRole = Role.CreateRole(store);
				valueTypeRole.RolePlayer = valueType;
				roleCollection.Add(valueTypeRole);

				InternalUniquenessConstraint ic	= InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
				ic.RoleCollection.Add(valueTypeRole); // Automatically sets FactType, setting it again will remove and delete the new constraint
				objectType.PreferredIdentifier = ic;
			}

			private ObjectType FindValueType(string name, ORMModel objModel)
			{

				LocatedElement element = objModel.ObjectTypesDictionary.GetElement(name);
				if (!element.IsEmpty)
				{
					if (element.SingleElement != null)
					{
						return (ObjectType)element.SingleElement;
					}
					else if (element.MultipleElements.Count > 0)
					{
						foreach (ObjectType objectType in element.MultipleElements)
						{
							return objectType;
						}
					}
				}
				return null;
			}


			/// <summary>
			///  Utility function to cahnge the name of an existing reference mode.
			/// </summary>
			/// <param name="valueTypeName"></param>
			/// <param name="objectType"></param>
			/// <param name="preferredConstraint"></param>
			/// <param name="store"></param>
			private void RenameReferenceMode(string valueTypeName, ObjectType objectType, InternalUniquenessConstraint preferredConstraint, Store store)
			{
				ORMModel objModel = objectType.Model;
				ObjectType valueType = FindValueType(valueTypeName, objModel);
				if (!IsValueTypeShared(preferredConstraint, objModel) && valueType == null)
				{
					valueType = preferredConstraint.RoleCollection[0].RolePlayer;
					if (valueType.IsValueType)
					{
						valueType.Name = valueTypeName;
					}
				}
				else
				{					
					if (valueType == null)
					{
						valueType = ObjectType.CreateObjectType(store);
						valueType.IsValueType = true;
						valueType.Name = valueTypeName;
						valueType.Model = objModel;
					}

					if (!IsValueTypeShared(preferredConstraint, objModel))
					{
						preferredConstraint.RoleCollection[0].RolePlayer.Remove();
					}

					preferredConstraint.RoleCollection[0].RolePlayer = valueType;
				}
			}

			/// <summary>
			/// Utility function to remove the reference mode objects.  Removes the fact, value type, and
			/// preffered internal uniqueness constraint.
			/// </summary>
			/// <param name="preferredConstraint"></param>
			/// <param name="aggressivelyKillValueType">Allow removing the value type along with the reference mode predicate</param>
			private void KillReferenceMode(InternalUniquenessConstraint preferredConstraint, bool aggressivelyKillValueType)
			{
				ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
				if (valueType.IsValueType)
				{
					FactType refFact = preferredConstraint.RoleCollection[0].FactType;
					if (!IsValueTypeShared(preferredConstraint, valueType.Model) && aggressivelyKillValueType)
					{
						valueType.Remove();
					}
					refFact.Remove();
				}
			}

			private bool IsValueTypeShared(InternalUniquenessConstraint preferredConstraint, ORMModel model)
			{
				if (preferredConstraint != null)
				{
					ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
					if (valueType.IsValueType)
					{
						int count = 0;
						foreach (ElementLink link in valueType.GetElementLinks())
						{
							if (!link.IsRemoving && !(link is SubjectHasPresentation))
							{
								++count;
								// We're expecting a ValueTypeHasDataType,
								// RoleHasRolePlayer, ModelHasObjectType, and
								// 0 or more (ignored) SubjectHasPresentation
								// links. Any other links indicate a shared value type.
								if (count > 3)
								{
									return true;
								}
							}
						}					
					}
				}
				return false;
			}
			#endregion
		}
		#endregion // ObjectTypeChangeRule class
		#region ObjectTypeChangeRule class

		/// <summary>
		/// Enforces Delete Rules
		/// </summary>
		[RuleOn(typeof(ObjectType))]
		private class ObjectTypeRemoveRule : RemovingRule
		{
			/// <summary>
			/// Executes when an object is removing
			/// </summary>
			/// <param name="e"></param>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectType objectType = (ObjectType)e.ModelElement;
				objectType.ReferenceModeDisplay = "";
			}
		}
		#endregion //ObjectTypeChangeRule class
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