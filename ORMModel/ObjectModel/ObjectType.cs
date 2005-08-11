using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM;
using System.Globalization;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A callback definition used for walking subtype and supertype hierarchies.
	/// </summary>
	/// <param name="type">The ObjectType being visited</param>
	/// <returns>true to continue iteration, false to stop</returns>
	[CLSCompliant(true)]
	public delegate bool ObjectTypeVisitor(ObjectType type);
	public partial class ObjectType : INamedElementDictionaryChild, IModelErrorOwner
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
				attributeGuid == DataTypeDisplayMetaAttributeGuid ||
				attributeGuid == LengthMetaAttributeGuid ||
				attributeGuid == NestedFactTypeDisplayMetaAttributeGuid ||
				attributeGuid == ReferenceModeDisplayMetaAttributeGuid ||
				attributeGuid == ValueRangeTextMetaAttributeGuid)
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
			else if (attributeGuid == ObjectType.DataTypeDisplayMetaAttributeGuid)
			{
				//If this objectype has a reference mode, return the datatype corresponding
				//to the ref mode's datatype.
				ObjectType refModeRolePlayer = GetValueTypeForPreferredConstraint();
				if (refModeRolePlayer != null)
				{
					return refModeRolePlayer.DataType;
				}
				return this.DataType;
			}
			else if (attributeGuid == ObjectType.ReferenceModeDisplayMetaAttributeGuid)
			{
				ReferenceMode refMode;
				string referenceModeString;
				this.GetReferenceMode(out refMode, out referenceModeString);
				return (refMode != null) ? (object)refMode : referenceModeString;
			}
			else if (attributeGuid == ObjectType.ReferenceModeStringMetaAttributeGuid)
			{
				ReferenceMode refMode;
				string referenceModeString;
				this.GetReferenceMode(out refMode, out referenceModeString);
				return referenceModeString;
			}
			else if (attributeGuid == ObjectType.ReferenceModeMetaAttributeGuid)
			{
				ReferenceMode refMode;
				string referenceModeString;
				GetReferenceMode(out refMode, out referenceModeString);
				return refMode;
			}
			else if (attributeGuid == ObjectType.NestedFactTypeDisplayMetaAttributeGuid)
			{
				return NestedFactType;
			}
			else if (attributeGuid == ValueRangeTextMetaAttributeGuid)
			{
				ValueRangeDefinition defn = FindValueRangeDefinition(false);
				return (defn == null) ? "" : defn.Text;
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
			if (attributeGuid == ScaleMetaAttributeGuid ||
				attributeGuid == LengthMetaAttributeGuid)
			{
				return IsValueType;
			}
			else if (attributeGuid == DataTypeDisplayMetaAttributeGuid ||
				attributeGuid == ValueRangeTextMetaAttributeGuid)
			{
				if (!IsValueType && HasReferenceMode)
				{
					ArrayList pels = this.AssociatedPresentationElements;
					foreach (object obj in pels)
					{
						ShapeModel.ObjectTypeShape objectShape;
						if (null != (objectShape = obj as ShapeModel.ObjectTypeShape))
						{
							return !objectShape.ExpandRefMode;
						}
					}
				}
				return NestedFactType == null && (IsValueType || HasReferenceMode);
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
		/// Return a custom property descriptor for the ReferenceModeDisplay property
		/// </summary>
		/// <param name="modelElement"></param>
		/// <param name="metaAttributeInfo"></param>
		/// <param name="requestor"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes)
		{
			if (metaAttributeInfo.Id == ReferenceModeDisplayMetaAttributeGuid)
			{
				return new ReferenceModeDisplayPropertyDescriptor(modelElement, metaAttributeInfo, requestor, attributes);
			}
			return base.CreatePropertyDescriptor(modelElement, metaAttributeInfo, requestor, attributes);
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
			else if (elemDesc != null && elemDesc.MetaAttributeInfo.Id == ValueRangeTextMetaAttributeGuid)
			{
				return !(NestedFactType == null && (IsValueType || HasReferenceMode));
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

		#region UtilityMethods
		/// <summary>
		/// Utility function to create the reference mode objects.  Creates the fact, value type, and
		/// preffered internal uniqueness constraint.
		/// </summary>
		private void CreateReferenceMode(string valueTypeName)
		{
			ORMModel model = this.Model;
			Store store = model.Store;
			ObjectType valueType = FindValueType(valueTypeName, model);

			FactType refFact = FactType.CreateFactType(store);
			refFact.Model = model;

			if (valueType == null)
			{
				valueType = ObjectType.CreateObjectType(store);
				valueType.Name = valueTypeName;
				valueType.Model = model;
				valueType.IsValueType = true;
			}

			Role objectTypeRole = Role.CreateRole(store);
			objectTypeRole.RolePlayer = this;
			RoleMoveableCollection roleCollection = refFact.RoleCollection;
			roleCollection.Add(objectTypeRole);

			Role valueTypeRole = Role.CreateRole(store);
			valueTypeRole.RolePlayer = valueType;
			roleCollection.Add(valueTypeRole);

			InternalUniquenessConstraint ic = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
			ic.RoleCollection.Add(valueTypeRole); // Automatically sets FactType, setting it again will remove and delete the new constraint
			this.PreferredIdentifier = ic;

			ReadingOrder readingOrder1 = ReadingOrder.CreateReadingOrder(store);
			RoleMoveableCollection roles = refFact.RoleCollection;
			RoleMoveableCollection readingRoles = readingOrder1.RoleCollection;
			readingRoles.Add(roles[0]);
			readingRoles.Add(roles[1]);
			readingOrder1.AddReading(ResourceStrings.ReferenceModePredicateReading);
			readingOrder1.FactType = refFact;

			ReadingOrder readingOrder2 = ReadingOrder.CreateReadingOrder(store);
			readingRoles = readingOrder2.RoleCollection;
			readingRoles.Add(roles[1]);
			readingRoles.Add(roles[0]);
			readingOrder2.AddReading(ResourceStrings.ReferenceModePredicateInverseReading);
			readingOrder2.FactType = refFact;
		}

		private static ObjectType FindValueType(string name, ORMModel objModel)
		{
			return objModel.ObjectTypesDictionary.GetElement(name).FirstElement as ObjectType;
		}
		/// <summary>
		///  Utility function to cahnge the name of an existing reference mode.
		/// </summary>
		/// <param name="valueTypeName"></param>
		public void RenameReferenceMode(string valueTypeName)
		{
			ORMModel model = this.Model;
			InternalUniquenessConstraint preferredConstraint = this.PreferredIdentifier as InternalUniquenessConstraint;
			ObjectType valueType = FindValueType(valueTypeName, model);
			if (!IsValueTypeShared(preferredConstraint) && valueType == null)
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
					Store store = model.Store;
					valueType = ObjectType.CreateObjectType(store);
					valueType.Name = valueTypeName;
					valueType.Model = model;
					valueType.IsValueType = true;
				}

				if (!IsValueTypeShared(preferredConstraint))
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
		/// <param name="aggressivelyKillValueType">Allow removing the value type along with the reference mode predicate</param>
		private void KillReferenceMode(bool aggressivelyKillValueType)
		{
			InternalUniquenessConstraint preferredConstraint = this.PreferredIdentifier as InternalUniquenessConstraint;
			ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
			if (valueType.IsValueType)
			{
				FactType refFact = preferredConstraint.RoleCollection[0].FactType;
				if (!IsValueTypeShared(preferredConstraint) && aggressivelyKillValueType)
				{
					valueType.Remove();
				}
				refFact.Remove();
			}
		}
		/// <summary>
		/// Returns true if the reference mode pattern is sharing a value
		/// type with another object type
		/// </summary>
		public bool ReferenceModeSharesValueType
		{
			get
			{
				InternalUniquenessConstraint preferredConstraint = PreferredIdentifier as InternalUniquenessConstraint;
				return (preferredConstraint != null) ? IsValueTypeShared(preferredConstraint) : false;
			}
		}
		private static bool IsValueTypeShared(InternalUniquenessConstraint preferredConstraint)
		{
			if (preferredConstraint != null)
			{
				ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
				if (valueType.IsValueType)
				{
					IList links = valueType.GetElementLinks();
					int linkCount = links.Count;
					if (linkCount > 3) // Easy initial check
					{
						int count = 0;
						for (int i = 0; i < linkCount; ++i)
						{
							ElementLink link = (ElementLink)links[i];
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
			}
			return false;
		}
		/// <summary>
		/// Indicates whether this ObjectType has a reference mode set.
		/// </summary>
		public bool HasReferenceMode
		{
			get
			{
				return GetValueTypeForPreferredConstraint() != null;
			}
		}
		#endregion

		/// <summary>
		/// Retrieves the role player on the preferred internal uniquiness constraint.
		/// </summary>
		/// <returns>The role player as an ObjectType if it exists; otherwise, null.</returns>
		private ObjectType GetObjectTypeForPreferredConstraint()
		{
			InternalConstraint prefConstraint = this.PreferredIdentifier as InternalConstraint;

			//If there is a preferred internal uniqueness constraint and that uniqueness constraint's role
			// player is a value type then return the value type.
			if (prefConstraint != null)
			{
				return prefConstraint.RoleCollection[0].RolePlayer;
			}
			return null;
		}
		/// <summary>
		/// Retrieves the ValueType object representing this ObjectType's value type.
		/// </summary>
		/// <returns>The value type as an ObjectType if it exists; otherwise, null.</returns>
		private ObjectType GetValueTypeForPreferredConstraint()
		{
			//If there is a preferred internal uniqueness constraint and that uniqueness constraint's role
			// player is a value type then return the value type.
			ObjectType valueTypeCandidate = GetObjectTypeForPreferredConstraint();
			if (null != valueTypeCandidate && valueTypeCandidate.IsValueType)
			{
				return valueTypeCandidate;
			}
			return null;
		}
		/// <summary>
		/// Returns the Reference Mode for the given object if one exists
		/// </summary>
		/// <returns></returns>
		public ReferenceMode GetReferenceMode()
		{
			ObjectType objectType;
			if (null != (objectType = GetObjectTypeForPreferredConstraint()))
			{
				Neumont.Tools.ORM.ObjectModel.ReferenceMode refMode = Neumont.Tools.ORM.ObjectModel.ReferenceMode.FindReferenceModeFromEntityNameAndValueName(objectType.Name, this.Name, this.Model);
				return refMode;
			}
			return null;
		}
		/// <summary>
		/// Returns the Reference Mode for the given object if one exists
		/// </summary>
		/// <returns></returns>
		public ReferenceMode GetReferenceMode(string formatString)
		{
			ObjectType objectType;
			if (null != (objectType = GetObjectTypeForPreferredConstraint()))
			{
				Neumont.Tools.ORM.ObjectModel.ReferenceMode refMode = Neumont.Tools.ORM.ObjectModel.ReferenceMode.FindReferenceModeFromEntityNameAndValueName(objectType.Name, this.Name, formatString, this.Model);
				return refMode;
			}
			return null;
		}
		/// <summary>
		/// Returns the Reference Mode for the given object if one exists
		/// </summary>
		/// <returns></returns>
		public ReferenceMode GetReferenceMode(string formatString, string referenceModeName, string oldReferenceModeName)
		{
			ObjectType objectType;
			if (null != (objectType = GetObjectTypeForPreferredConstraint()))
			{
				Neumont.Tools.ORM.ObjectModel.ReferenceMode refMode = Neumont.Tools.ORM.ObjectModel.ReferenceMode.FindReferenceModeFromEntityNameAndValueName(objectType.Name, this.Name, formatString, referenceModeName, oldReferenceModeName, this.Model);
				return refMode;
			}
			return null;
		}
		/// <summary>
		/// Determines whether to return the valuetype name as the reference mode or, if there
		/// is a reference mode, it returns the reference mode name
		/// </summary>
		/// <param name="refMode"></param>
		/// <param name="refModeString"></param>
		private void GetReferenceMode(out ReferenceMode refMode, out string refModeString)
		{
			refMode = null;
			refModeString = "";
			ObjectType valueType;
			if (null != (valueType = GetValueTypeForPreferredConstraint()))
			{
				string valueTypeName = valueType.Name;
				refMode = ReferenceMode.FindReferenceModeFromEntityNameAndValueName(valueTypeName, this.Name, this.Model);
				refModeString = (refMode == null) ? valueTypeName : refMode.Name;
			}
		}
		/// <summary>
		/// Retrieves the ValueRangeDefinition to use for this ObjectType.
		/// </summary>
		/// <param name="autoCreate">If the ValueRangeDefinition is null, should one be created?
		/// This should be false if we're simply reading the definition.</param>
		/// <returns>For ObjectTypes with a ref mode, this returns the ValueRangeDefinition
		/// found on the ObjectType's preferred identifier role.</returns>
		public ValueRangeDefinition FindValueRangeDefinition(bool autoCreate)
		{
			if (HasReferenceMode)
			{
				ConstraintRoleSequence sequence = PreferredIdentifier;
				RoleMoveableCollection roleCollection = sequence.RoleCollection;
				if (roleCollection.Count == 1)
				{
					Role role = roleCollection[0];
					RoleValueRangeDefinition defn = role.ValueRangeDefinition;
					if (defn == null && autoCreate)
					{
						role.ValueRangeDefinition = defn = RoleValueRangeDefinition.CreateRoleValueRangeDefinition(role.Store);
					}
					return defn as ValueRangeDefinition;
				}
			}
			ValueTypeValueRangeDefinition valueDefn = this.ValueRangeDefinition;
			if (valueDefn == null && autoCreate)
			{
				this.ValueRangeDefinition = valueDefn = ValueTypeValueRangeDefinition.CreateValueTypeValueRangeDefinition(this.Store);
			}
			return valueDefn as ValueRangeDefinition;
		}
		#endregion // Customize property display
		#region Subtype and Supertype routines
		/// <summary>
		/// Get the sub types for this type
		/// </summary>
		/// <returns>Enumeration of ObjectType</returns>
		[CLSCompliant(false)]
		public IEnumerable<ObjectType> SubtypeCollection
		{
			get
			{
				foreach (Role role in PlayedRoleCollection)
				{
					SubtypeFact subtypeFact = role.FactType as SubtypeFact;
					if (subtypeFact != null)
					{
						// If we're the derived type
						if (subtypeFact.Supertype == this)
						{
							yield return subtypeFact.Subtype;
						}
					}
				}
			}
		}
		/// <summary>
		/// Get the super types for this type
		/// </summary>
		/// <returns>Enumeration of ObjectType</returns>
		[CLSCompliant(false)]
		public IEnumerable<ObjectType> SupertypeCollection
		{
			get
			{
				foreach (Role role in PlayedRoleCollection)
				{
					SubtypeFact subtypeFact = role.FactType as SubtypeFact;
					if (subtypeFact != null)
					{
						// If we're the derived type
						if (subtypeFact.Subtype == this)
						{
							yield return subtypeFact.Supertype;
						}
					}
				}
			}
		}
		/// <summary>
		/// Recursively walk all supertypes of a given type (including the type itself).
		/// </summary>
		/// <param name="startingType">The type to begin recursion with</param>
		/// <param name="visitor">A callback delegate. Should return true to continue recursion.</param>
		/// <returns>true if the iteration completes, false if it is stopped by a positive response</returns>
		public static bool WalkSupertypes(ObjectType startingType, ObjectTypeVisitor visitor)
		{
			if (!visitor(startingType))
			{
				return false;
			}
			foreach (ObjectType superType in startingType.SupertypeCollection)
			{
				if (!WalkSupertypes(superType, visitor))
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Recursively walk all subtypes of a given type (including the type itself).
		/// </summary>
		/// <param name="startingType">The type to begin recursion with</param>
		/// <param name="visitor">A callback delegate. Should return true to continue recursion.</param>
		/// <returns>true if the iteration completes, false if it is stopped by a positive response</returns>
		public static bool WalkSubtypes(ObjectType startingType, ObjectTypeVisitor visitor)
		{
			if (!visitor(startingType))
			{
				return false;
			}
			foreach (ObjectType subType in startingType.SubtypeCollection)
			{
				if (!WalkSubtypes(subType, visitor))
				{
					return false;
				}
			}
			return true;
		}
		#endregion // Subtype and Supertype routines
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
					ObjectType objectType = e.ModelElement as ObjectType;
					bool newValue = (bool)e.NewValue;
					DataType dataType = null;
					if (newValue)
					{
						dataType = objectType.Model.DefaultDataType;
					}
					objectType.DataType = dataType;
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
				else if (attributeGuid == ObjectType.DataTypeDisplayMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					//If this objectype has a reference mode, return the datatype corresponding
					//to the ref mode's datatype.
					ObjectType refModeRolePlayer = objectType.GetValueTypeForPreferredConstraint();
					if (refModeRolePlayer != null)
					{
						objectType = refModeRolePlayer;
					}
					objectType.DataType = e.NewValue as DataType;
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
					InternalUniquenessConstraint prefConstraint = objectType.PreferredIdentifier as InternalUniquenessConstraint;

					if (prefConstraint != null)
					{
						string newValue = (string)e.NewValue;
						string oldValue = (string)e.OldValue;
						string oldReferenceModeName = "";

						ReferenceMode referenceMode = ReferenceMode.FindReferenceModeFromEntityNameAndValueName(objectType.ReferenceModeString, oldValue, objectType.Model);

						if (referenceMode != null)
						{
							string name = newValue;
							oldReferenceModeName = referenceMode.Name;
							name = referenceMode.GenerateValueTypeName(name);

							if (name != oldReferenceModeName)
							{
								objectType.RenameReferenceMode(name);
							}
						}
					}
				}
				else if (attributeGuid == ObjectType.ReferenceModeDisplayMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					SetReferenceMode(objectType, e.NewValue as ReferenceMode, e.OldValue as ReferenceMode, e.NewValue as string, e.OldValue as string);
				}
				else if (attributeGuid == ObjectType.ReferenceModeStringMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					string newName = (string)e.NewValue;

					// Find the unique reference mode for this object type and reference mode string
					IList<ReferenceMode> referenceModes = ReferenceMode.FindReferenceModesByName(newName, objectType.Model);
					ReferenceMode singleMode = null;
					int modeCount = referenceModes.Count;
					if (modeCount == 1)
					{
						singleMode = referenceModes[0];
						newName = null;
					}
					else if (modeCount > 1)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeAmbiguousName);
					}
					SetReferenceMode(objectType, singleMode, null, newName, e.OldValue as string);
				}
				else if (attributeGuid == ObjectType.ReferenceModeMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					SetReferenceMode(objectType, (ReferenceMode)e.NewValue, (ReferenceMode)e.OldValue, null, null);
				}
				else if (attributeGuid == ObjectType.ValueRangeTextMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					ValueRangeDefinition defn = objectType.FindValueRangeDefinition(true);
					defn.Text = (string)e.NewValue;
				}
			}
			/// <summary>
			/// Determines if the value type associated with the reference mode pattern needs to be 
			/// removed, renamed, or created based on the new and old values of the property.
			/// Value Type modifications won't be seen unless the object type is viewed in expanded mode.
			/// </summary>
			/// <param name="objectType">the selected object</param>
			/// <param name="newMode">The new reference mode</param>
			/// <param name="oldMode">The old reference mode</param>
			/// <param name="newModeName">The new reference mode name</param>
			/// <param name="oldModeName">The old reference mode name</param>
			private static void SetReferenceMode(ObjectType objectType, ReferenceMode newMode, ReferenceMode oldMode, string newModeName, string oldModeName)
			{
				Store store = objectType.Store;
				bool aggressivelyKillValueType = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Contains(DeleteReferenceModeValueType);

				string newValue = newModeName;
				if (newValue == null)
				{
					newValue = "";
				}
				string oldValue = oldModeName;
				if (oldValue == null)
				{
					oldValue = "";
				}

				string name = newValue;
				if (newMode != null)
				{
					if (name.Length == 0)
					{
						name = newMode.Name;
					}
					name = newMode.GenerateValueTypeName(objectType.Name);
					newValue = newMode.Name;
					if (oldMode != null)
					{
						oldValue = oldMode.Name;
					}
				}
				bool haveNew = newMode != null || newValue.Length != 0;
				bool hadOld = oldMode != null || oldValue.Length != 0;
				if (hadOld)
				{
					if (haveNew)
					{
						objectType.RenameReferenceMode(name);
					}
					else
					{
						objectType.KillReferenceMode(aggressivelyKillValueType);
					}
				}
				else
				{
					Debug.Assert(haveNew);
					objectType.CreateReferenceMode(name);
				}
			}
		}
		#endregion // ObjectTypeChangeRule class
		#region ObjectTypeRemoveRule class

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
		#endregion //ObjectTypeRemoveRule class
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
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasObjectType.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region DataTypeNotSpecifiedError retrieval and validation
		/// <summary>
		/// Returns an error object if the data type is the unspecified data
		/// type. The UnspecifiedDataType is different than a null DataType, which
		/// is simply implying that this ObjectType is not a ValueType.
		/// </summary>
		public DataTypeNotSpecifiedError DataTypeNotSpecifiedError
		{
			get
			{
				IList list = GetElementLinks(ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid);
				if (list.Count != 0)
				{
					ValueTypeHasDataType link = (ValueTypeHasDataType)list[0];
					UnspecifiedDataType dataType = link.DataType as UnspecifiedDataType;
					if (dataType != null)
					{
						return link.DataTypeNotSpecifiedError;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Validate that a DataTypeNotSpecifiedError is present if neede, and that
		/// the data type is an unspecified type instance if the error is present.
		/// </summary>
		private void ValidateDataTypeNotSpecifiedError(INotifyElementAdded notifyAdded)
		{
			IList list = GetElementLinks(ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid);
			if (list.Count != 0)
			{
				ValueTypeHasDataType link = (ValueTypeHasDataType)list[0];
				DataTypeNotSpecifiedError error = link.DataTypeNotSpecifiedError;
				UnspecifiedDataType dataType = link.DataType as UnspecifiedDataType;
				if (dataType == null)
				{
					if (error != null)
					{
						error.Remove();
					}
				}
				else if (error == null)
				{
					error = DataTypeNotSpecifiedError.CreateDataTypeNotSpecifiedError(Store);
					error.Model = Model;
					link.DataTypeNotSpecifiedError = error;
					error.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(error, true);
					}
				}
			}
		}
		#endregion // DataTypeNotSpecifiedError retrieval and validation
		#region EntityTypeRequiresReferenceSchemeError Validation
		private void ValidateRequiresReferenceScheme(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = true;
				Store theStore = Store;
				ORMModel theModel = Model;
				if (IsValueType == true || NestedFactType != null || this.PreferredIdentifier != null)
				{
					hasError = false;
				}

				EntityTypeRequiresReferenceSchemeError noRefSchemeError = ReferenceSchemeError;
				if (hasError)
				{
					if (noRefSchemeError == null)
					{
						noRefSchemeError = EntityTypeRequiresReferenceSchemeError.CreateEntityTypeRequiresReferenceSchemeError(theStore);
						noRefSchemeError.Model = theModel;
						noRefSchemeError.ObjectType = this;
						noRefSchemeError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(noRefSchemeError, true);
						}
					}
				}
				else
				{
					if (noRefSchemeError != null)
					{
						noRefSchemeError.Remove();
					}
				}
			}
		}

		#endregion // EntityTypeRequiresReferenceSchemeError Validation
		#region EntityTypeRequiresReferenceSchemeError Rules
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.LocalCommit)]
		private class VerifyReferenceSchemeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				link.PreferredIdentifierFor.ValidateRequiresReferenceScheme(null);
			}
		}
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.LocalCommit)]
		private class VerifyReferenceSchemeRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				link.PreferredIdentifierFor.ValidateRequiresReferenceScheme(null);
			}
		}
		[RuleOn(typeof(Objectification), FireTime = TimeToFire.LocalCommit)]
		private class VerifyObjectificationAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				link.NestingType.ValidateRequiresReferenceScheme(null);
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.LocalCommit)]
		private class VerifyValueTypeHasDataTypeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				link.ValueTypeCollection.ValidateRequiresReferenceScheme(null);
			}
		}
		[RuleOn(typeof(Objectification), FireTime = TimeToFire.LocalCommit)]
		private class VerifyObjectificationRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				link.NestingType.ValidateRequiresReferenceScheme(null);
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.LocalCommit)]
		private class VerifyValueTypeHasDataTypeRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				link.ValueTypeCollection.ValidateRequiresReferenceScheme(null);
			}
		}
		/// <summary>
		/// Calls the validation of all FactType related errors
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType), FireTime=TimeToFire.LocalCommit)]
		private class ModelHasObjectTypeAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
				link.ObjectTypeCollection.ValidateErrors(null);
			}
		}
		#endregion // EntityTypeRequiresReferenceSchemeError Rules
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the errors associated with the object.
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				DataTypeNotSpecifiedError unspecifiedDataTypeError = DataTypeNotSpecifiedError;
				if (unspecifiedDataTypeError != null)
				{
					yield return unspecifiedDataTypeError;
				}

				EntityTypeRequiresReferenceSchemeError requiredReferenceSchemeError = ReferenceSchemeError;
				if (requiredReferenceSchemeError != null)
				{
					yield return requiredReferenceSchemeError;
				}
			}
		}
		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateDataTypeNotSpecifiedError(notifyAdded);
			ValidateRequiresReferenceScheme(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner implementation
		#region CheckForIncompatibleRelationshipRule class
		/// <summary>
		/// Ensure consistency among relationships attached to ObjectType roles.
		/// This is an object model backup for the UI, which does not offer these
		/// conditions to the user.
		/// </summary>
		[RuleOn(typeof(Objectification)), RuleOn(typeof(ValueTypeHasDataType)), RuleOn(typeof(ObjectTypePlaysRole))]
		private class CheckForIncompatibleRelationshipRule : AddRule
		{
			/// <summary>
			/// Called when an attempt is made to turn an ObjectType into either
			/// a value type or a nesting type.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Objectification nester;
				ValueTypeHasDataType valType;
				ObjectTypePlaysRole roleLink;
				FactTypeHasRole newRole;
				ModelElement element = e.ModelElement;
				bool incompatibleValueTypeCombination = false;
				bool incompatibleNestingAndRoleCombination = false;
				bool subtypesNotNested = false;
				// Note that the other portion of this condition is
				// checked in a separate add rule for EntityTypeHasPreferredIdentifier
				bool incompatiblePreferredIdentifierCombination = false;
				if (null != (nester = element as Objectification))
				{
					if (nester.NestedFactType is SubtypeFact)
					{
						subtypesNotNested = true;
					}
					else
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
				else if (subtypesNotNested)
				{
					exceptionString = ResourceStrings.ModelExceptionSubtypeFactNotNested;
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
		#region ReferenceModeDisplayPropertyDescriptor class
		/// <summary>
		/// A property descriptor that filters out some standard values from
		/// the type converter.
		/// </summary>
		protected class ReferenceModeDisplayPropertyDescriptor : ElementPropertyDescriptor
		{
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="modelElement">Passed to base</param>
			/// <param name="metaAttributeInfo">Passed to base</param>
			/// <param name="requestor">Passed to base</param>
			/// <param name="attributes">Passed to base</param>
			public ReferenceModeDisplayPropertyDescriptor(ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes)
				: base(modelElement, metaAttributeInfo, requestor, attributes)
			{
			}
			/// <summary>
			/// Return a custom typeconverter that
			/// limits the predefined values.
			/// </summary>
			/// <value></value>
			public override TypeConverter Converter
			{
				get
				{
					return new ReferenceModeDisplayConverter();
				}
			}
			#region ReferenceModeDisplayConverter class
			private class ReferenceModeDisplayConverter : TypeConverter
			{
				public ReferenceModeDisplayConverter()
				{
				}

				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					if (sourceType == typeof(string))
					{
						return true;
					}
					return false;
				}
				public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
				{
					string refMode = value as string;
					ObjectType instance = (ObjectType)Editors.EditorUtility.ResolveContextInstance(context.Instance, true);
					IList<ReferenceMode> referenceModes = ReferenceMode.FindReferenceModesByName(refMode, instance.Model);

					int modeCount = referenceModes.Count;
					if (modeCount == 0)
					{
						return refMode;
					}
					else if (modeCount == 1)
					{
						return referenceModes[0] as ReferenceMode;
					}
					else
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeAmbiguousName);
					}
				}
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					if (destinationType == typeof(ReferenceMode))
					{
						return true;
					}
					return false;
				}
				public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						ReferenceMode mode = value as ReferenceMode;
						return (mode != null) ? mode.Name : value.ToString();
					}
					return null;
				}
			}
			#endregion // ReferenceModeDisplayConverter class
		}
		#endregion // ReferenceModeDisplayPropertyDescriptor class
	}

	#region class EntityTypeRequiresReferenceSchemeError
	partial class EntityTypeRequiresReferenceSchemeError : IRepresentModelElements
	{
		#region overrides
		/// <summary>
		/// Creates error text for when an EntityType lacks a reference scheme.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorEntityTypeRequiresReferenceSchemeMessage, ObjectType.Name, Model.Name);
			if (Name != newText)
			{
				Name = newText;
			}
		}

		/// <summary>
		/// Sets regernate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion

		#region IRepresentModelElements Members
		/// <summary>
		/// The EntityType to which the error belongs
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ObjectType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion // class EntityTypeRequiresReferenceSchemeError
}
