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
	/// <param name="depth">The distance from the initial recursion point. depth
	/// 0 is the starting object.</param>
	/// <returns>true to continue iteration, false to stop</returns>
	[CLSCompliant(true)]
	public delegate ObjectTypeVisitorResult ObjectTypeVisitor(ObjectType type, int depth);
	/// <summary>
	/// Expected results from the ObjectTypeVisitor delegate
	/// </summary>
	[CLSCompliant(true)]
	public enum ObjectTypeVisitorResult
	{
		/// <summary>
		/// Continue recursion
		/// </summary>
		Continue,
		/// <summary>
		/// Stop recursion
		/// </summary>
		Stop,
		/// <summary>
		/// Continue iterating siblings, but not children
		/// </summary>
		SkipChildren,
	}
	public partial class ObjectType : INamedElementDictionaryChild, INamedElementDictionaryParent, INamedElementDictionaryRemoteParent, IModelErrorOwner
	{
		#region Public token values
		/// <summary>
		/// A key to set in the top-level transaction context to indicate that
		/// we should aggressively kill a value type associated with an entity
		/// type via the reference mode pattern. This should be set if the reference
		/// mode is collapsed on an entity type that is being explicitly deleted by
		/// the user.
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
				attributeGuid == ReferenceModeMetaAttributeGuid ||
				attributeGuid == ReferenceModeStringMetaAttributeGuid ||
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
				if (link == null)
				{
					ObjectType refModeRolePlayer = GetValueTypeForPreferredConstraint();
					if (refModeRolePlayer != null)
					{
						link = refModeRolePlayer.GetDataTypeLink();
					}
				}
				return (link == null) ? 0 : link.Scale;
			}
			else if (attributeGuid == ObjectType.LengthMetaAttributeGuid)
			{
				ValueTypeHasDataType link = GetDataTypeLink();
				if (link == null)
				{
					ObjectType refModeRolePlayer = GetValueTypeForPreferredConstraint();
					if (refModeRolePlayer != null)
					{
						link = refModeRolePlayer.GetDataTypeLink();
					}
				}
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
				ValueConstraint valueConstraint = FindValueConstraint(false);
				return (valueConstraint == null) ? "" : valueConstraint.Text;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
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
			if (attributeGuid == DataTypeDisplayMetaAttributeGuid ||
				attributeGuid == ScaleMetaAttributeGuid ||
				attributeGuid == LengthMetaAttributeGuid ||
				attributeGuid == ValueRangeTextMetaAttributeGuid)
			{
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
				return NestedFactType != null || PreferredIdentifier != null || IsSubtypeOrSupertype;
			}
			else if (elemDesc != null && elemDesc.MetaAttributeInfo.Id == ValueRangeTextMetaAttributeGuid)
			{
				return !(NestedFactType == null && (IsValueType || HasReferenceMode));
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // CustomStorage handlers
		#region PreferredIdentifier Property
		/// <summary>
		/// Get the preferred identifier for this object. The preferred identifier is
		/// either and InternalUniquenessConstraint or an ExternalUniquenessConstraint.
		/// </summary>
		public IConstraint PreferredIdentifier
		{
			// Note that this is all based on spit code. However, the internal and external
			// uniqueness constraints do not share a useful base class (ORMNamedElement is the
			// closest and the one used in the model), and the IMS engine needs classes, not
			// interfaces. Because of this limitation, and the poor code spit for 1-1 relationships
			// coming from the Phoenix engine (the 1-1 is not properly enforced), we do the normal
			// code spit by hand.
			get
			{
				return GetCounterpartRolePlayer(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid, EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid, false) as IConstraint;
			}
			set
			{
				// Note that type enforcement is done in the EntityTypeHasPreferredIdentifier.EntityTypeAddedRule
				// which is guaranteed to run for all object model modifications. We defer validation of the constraint
				// types to that routine. However, the IConstraint passed in must be an ORMNamedElement, so we
				// use an exception cast here to do a minimal sanity check before proceeding.
				ORMNamedElement typedValue = (ORMNamedElement)value;
				bool sameRolePlayer = false;
				MetaRoleInfo roleInfo = Partition.MetaDataDirectory.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				IList links = GetElementLinks(roleInfo);
				int linkCount = links.Count;
				if (linkCount != 0)
				{
					for (int i = linkCount - 1; i >= 0; --i)
					{
						ElementLink link = links[i] as ElementLink;
						if (!link.IsRemoved)
						{
							ORMNamedElement counterpart = link.GetRolePlayer(roleInfo.OppositeMetaRole) as ORMNamedElement;
							if (counterpart != null && object.ReferenceEquals(counterpart, typedValue))
							{
								sameRolePlayer = true;
							}
							else
							{
								link.Remove();
							}
							break;
						}
					}
				}
				else if (typedValue != null)
				{
					// Check the relationship on the other end to enforce 1-1
					links = typedValue.GetElementLinks(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
					linkCount = links.Count;
					if (linkCount != 0)
					{
						for (int i = linkCount - 1; i >= 0; --i)
						{
							ElementLink link = links[i] as ElementLink;
							if (!link.IsRemoved)
							{
								ObjectType counterpart = link.GetRolePlayer(roleInfo) as ObjectType;
								if (counterpart != null && object.ReferenceEquals(counterpart, this))
								{
									sameRolePlayer = true;
								}
								else
								{
									link.Remove();
								}
								break;
							}
						}
					}
				}
				if ((!sameRolePlayer) && (typedValue != null))
				{
					this.Partition.ElementFactory.CreateElementLink(
						typeof(EntityTypeHasPreferredIdentifier),
						new RoleAssignment[]
						{
							new RoleAssignment(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid, typedValue),
							new RoleAssignment(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid, this)
						});
				}
			}
		}
		#endregion // PreferredIdentifier Property
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
			RoleBaseMoveableCollection roleCollection = refFact.RoleCollection;
			roleCollection.Add(objectTypeRole);

			Role valueTypeRole = Role.CreateRole(store);
			valueTypeRole.RolePlayer = valueType;
			roleCollection.Add(valueTypeRole);

			InternalUniquenessConstraint ic = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
			ic.RoleCollection.Add(valueTypeRole); // Automatically sets FactType, setting it again will remove and delete the new constraint
			this.PreferredIdentifier = ic;

			ReadingOrder readingOrder1 = ReadingOrder.CreateReadingOrder(store);
			RoleBaseMoveableCollection roles = refFact.RoleCollection;
			RoleBaseMoveableCollection readingRoles = readingOrder1.RoleCollection;
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
							if (!link.IsRemoving && !(link is SubjectHasPresentation) && !(link is ORMExtendableElementHasExtensionElement))
							{
								++count;
								// We're expecting a ValueTypeHasDataType,
								// RoleHasRolePlayer, ModelHasObjectType, and
								// 0 or more (ignored) SubjectHasPresentation
								// and ORMExtendableElementHasExtensionElement-derived
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
		/// Retrieves the ValueConstraint to use for this ObjectType.
		/// </summary>
		/// <param name="autoCreate">If the ValueConstraint is null, should one be created?
		/// This should be false if we're simply reading the definition.</param>
		/// <returns>For ObjectTypes with a ref mode, this returns the ValueConstraint
		/// found on the ObjectType's preferred identifier role.</returns>
		public ValueConstraint FindValueConstraint(bool autoCreate)
		{
			if (HasReferenceMode)
			{
				InternalUniquenessConstraint sequence = PreferredIdentifier as InternalUniquenessConstraint;
				RoleMoveableCollection roleCollection = sequence.RoleCollection;
				if (roleCollection.Count == 1)
				{
					Role role = roleCollection[0];
					RoleValueConstraint roleValueConstraint = role.ValueConstraint;
					if (roleValueConstraint == null && autoCreate)
					{
						role.ValueConstraint = roleValueConstraint = RoleValueConstraint.CreateRoleValueConstraint(role.Store);
					}
					return roleValueConstraint as ValueConstraint;
				}
			}
			ValueTypeValueConstraint valueConstraint = this.ValueConstraint;
			if (valueConstraint == null && autoCreate)
			{
				this.ValueConstraint = valueConstraint = ValueTypeValueConstraint.CreateValueTypeValueConstraint(this.Store);
			}
			return valueConstraint as ValueConstraint;
		}
		#endregion // Customize property display
		#region Subtype and Supertype routines
		/// <summary>
		/// Get the sub types for this type
		/// </summary>
		/// <returns>Enumeration of ObjectType</returns>
		public IEnumerable<ObjectType> SubtypeCollection
		{
			get
			{
				RoleMoveableCollection playedRoles = PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				for (int i = 0; i < playedRoleCount; ++i)
				{
					Role role = playedRoles[i];
					if (role is SupertypeMetaRole)
					{
						yield return (role.FactType as SubtypeFact).Subtype;
					}
				}
			}
		}
		/// <summary>
		/// Get the super types for this type
		/// </summary>
		/// <returns>Enumeration of ObjectType</returns>
		public IEnumerable<ObjectType> SupertypeCollection
		{
			get
			{
				RoleMoveableCollection playedRoles = PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				for (int i = 0; i < playedRoleCount; ++i)
				{
					Role role = playedRoles[i];
					if (role is SubtypeMetaRole)
					{
						yield return (role.FactType as SubtypeFact).Supertype;
					}
				}
			}
		}
		/// <summary>
		/// Returns true if this ObjectType is the subtype or supertype of
		/// at least one other Objectype
		/// </summary>
		public bool IsSubtypeOrSupertype
		{
			get
			{
				RoleMoveableCollection playedRoles = PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				for (int i = 0; i < playedRoleCount; ++i)
				{
					Role role = playedRoles[i];
					if (role is SubtypeMetaRole || role is SupertypeMetaRole)
					{
						return true;
					}
				}
				return false;
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
			return (startingType != null) ? WalkSupertypes(startingType, 0, visitor) : false;
		}
		private static bool WalkSupertypes(ObjectType startingType, int depth, ObjectTypeVisitor visitor)
		{
			ObjectTypeVisitorResult result = visitor(startingType, depth);
			switch (result)
			{
				//case ObjectTypeVisitorResult.Continue:
				//    break;
				case ObjectTypeVisitorResult.SkipChildren:
					return true;
				case ObjectTypeVisitorResult.Stop:
					return false;
			}
			++depth;
			foreach (ObjectType superType in startingType.SupertypeCollection)
			{
				if (!WalkSupertypes(superType, depth, visitor))
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
			return (startingType != null) ? WalkSubtypes(startingType, 0, visitor) : false;
		}
		private static bool WalkSubtypes(ObjectType startingType, int depth, ObjectTypeVisitor visitor)
		{
			ObjectTypeVisitorResult result = visitor(startingType, depth);
			switch (result)
			{
				//case ObjectTypeVisitorResult.Continue:
				//    break;
				case ObjectTypeVisitorResult.SkipChildren:
					return true;
				case ObjectTypeVisitorResult.Stop:
					return false;
			}
			++depth;
			foreach (ObjectType subType in startingType.SubtypeCollection)
			{
				if (!WalkSubtypes(subType, depth, visitor))
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
					ObjectType objectType = e.ModelElement as ObjectType;
					ValueTypeHasDataType link = objectType.GetDataTypeLink();
					// No effect for non-value types
					if (link != null)
					{
						link.Scale = (int)e.NewValue;
					}
					else
					{
						if ((null != (objectType = objectType.GetValueTypeForPreferredConstraint()) &&
							(null != (link = objectType.GetDataTypeLink()))))
						{
							link.Scale = (int)e.NewValue;
						}
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
					ObjectType objectType = e.ModelElement as ObjectType;
					ValueTypeHasDataType link = objectType.GetDataTypeLink();
					// No effect for non-value types
					if (link != null)
					{
						link.Length = (int)e.NewValue;
					}
					else
					{
						if ((null != (objectType = objectType.GetValueTypeForPreferredConstraint()) &&
							(null != (link = objectType.GetDataTypeLink()))))
						{
							link.Length = (int)e.NewValue;
						}
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
					ValueConstraint valueConstraint = objectType.FindValueConstraint(true);
					valueConstraint.Text = (string)e.NewValue;
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
				//Now, set the dataType
				DataType dataType = null;
				if (newMode != null)
				{
					ORMModel ormModel = objectType.Model;
					dataType = ormModel.GetPortableDataType(newMode.Type);
					//Change the objectType to the ref mode's preferred valueType and set the
					//dataType on that objectType.
					//Unless things change and the refMode can be set on objects without a preferred constraint,
					//the objectType will always be changed.
					ObjectType refModeRolePlayer = objectType.GetValueTypeForPreferredConstraint();
					if (refModeRolePlayer != null)
					{
						objectType = refModeRolePlayer;
					}
				}
				objectType.DataType = dataType;
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
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { ValueTypeHasValueConstraint.ValueTypeMetaRoleGuid };
		/// <summary>
		/// Implementation of INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles. Identifies
		/// this as a remote parent for the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <returns>Guid for the ValueTypeHasValueConstraint.ValueType role</returns>
		protected static Guid[] GetNamedElementDictionaryLinkRoles()
		{
			return myRemoteNamedElementDictionaryRoles;
		}
		Guid[] INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles()
		{
			return GetNamedElementDictionaryLinkRoles();
		}
		#endregion // INamedElementDictionaryRemoteParent implementation
		#region INamedElementDictionaryParent implementation
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			if (parentMetaRoleGuid == ValueTypeHasValueConstraint.ValueTypeMetaRoleGuid)
			{
				ORMModel model = Model;
				if (model != null)
				{
					return ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
				}
			}
			return null;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetAllowDuplicateNamesContextKey(parentMetaRoleGuid, childMetaRoleGuid);
		}
		#endregion // INamedElementDictionaryParent implementation
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
		/// Validator callback for DataTypeNoteSpecifiedError
		/// </summary>
		private static void DelayValidateDataTypeNoteSpecifiedError(ModelElement element)
		{
			(element as ObjectType).ValidateDataTypeNotSpecifiedError(null);
		}
		/// <summary>
		/// Validate that a DataTypeNotSpecifiedError is present if needed, and that
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
					link.DataTypeNotSpecifiedError = error;
					error.Model = Model;
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
		/// <summary>
		/// Validator callback for EntityTypeRequiresReferenceSchemeError
		/// </summary>
		private static void DelayValidateEntityTypeRequiresReferenceSchemeError(ModelElement element)
		{
			(element as ObjectType).ValidateRequiresReferenceScheme(null);
		}
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
				else
				{
					// We can get the preferred identifier from the super type if it exists. The error
					// should appear on the supertype, not here.
					using (IEnumerator<ObjectType> superTypes = SupertypeCollection.GetEnumerator())
					{
						hasError = !superTypes.MoveNext();
					}
				}

				EntityTypeRequiresReferenceSchemeError noRefSchemeError = ReferenceSchemeError;
				if (hasError)
				{
					if (noRefSchemeError == null)
					{
						noRefSchemeError = EntityTypeRequiresReferenceSchemeError.CreateEntityTypeRequiresReferenceSchemeError(theStore);
						noRefSchemeError.ObjectType = this;
						noRefSchemeError.Model = theModel;
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
		#region ObectTypeRequiresPrimarySupertype Validation
		/// <summary>
		/// Validator callback for ObjectTypeRequiresPrimarySupertypeError
		/// </summary>
		private static void DelayValidateObjectTypeRequiresPrimarySupertypeError(ModelElement element)
		{
			(element as ObjectType).ValidateObjectTypeRequiresPrimarySupertypeError(null);
		}
		/// <summary>
		/// Rule helper to determine whether or not ObjectTypeRequiresPrimarySupertypeError should appear
		/// will assign SubFact as primary if only one exists.
		/// </summary>
		/// <param name="notifyAdded"></param>
		private void ValidateObjectTypeRequiresPrimarySupertypeError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = false;
				IList links = GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid);
				int linkCount = links.Count;
				if (linkCount != 0)
				{
					SubtypeFact firstSubtypeFact = null;
					int subtypeFactCount = 0;
					//bool hasPrimarySupertypeFact = false;
					int primaryFactCount = 0;
					for (int i = 0; i < linkCount; ++i)
					{
						ObjectTypePlaysRole link = links[i] as ObjectTypePlaysRole;
						SubtypeMetaRole subtypeRole = link.PlayedRoleCollection as SubtypeMetaRole;
						if (subtypeRole != null)
						{
							SubtypeFact subtypeFact = subtypeRole.FactType as SubtypeFact;
							if (subtypeFact != null)
							{
								if (subtypeFact.IsPrimary)
								{
									++primaryFactCount;
									firstSubtypeFact = subtypeFact;
									if (notifyAdded == null)
									{
										break;
									}
									if (primaryFactCount > 1)
									{
										for (int j = 0; j < linkCount; ++j)
										{
											link = links[j] as ObjectTypePlaysRole;
											subtypeRole = link.PlayedRoleCollection as SubtypeMetaRole;

											if (subtypeRole != null)
											{
												subtypeFact = subtypeRole.FactType as SubtypeFact;
												if (subtypeFact != null)
												{
													subtypeFact.IsPrimary = false;
												}
											}
										}
										break;
									}
									//hasPrimarySupertypeFact = true;
								}
								else if (firstSubtypeFact == null)
								{
									++subtypeFactCount;
									firstSubtypeFact = subtypeFact;
								}
								else
								{
									++subtypeFactCount;
								}
							}
						}
					}
					if (primaryFactCount != 1 && firstSubtypeFact != null)
					{
						if (subtypeFactCount == 1 && primaryFactCount == 0)
						{
							firstSubtypeFact.IsPrimary = true;
						}
						else
						{
							hasError = true;
						}
					}
				}
				ObjectTypeRequiresPrimarySupertypeError primaryRequired = this.ObjectTypeRequiresPrimarySupertypeError;
				if (hasError)
				{
					if (primaryRequired == null)
					{
						primaryRequired = ObjectTypeRequiresPrimarySupertypeError.CreateObjectTypeRequiresPrimarySupertypeError(this.Store);
						primaryRequired.ObjectType = this;
						primaryRequired.Model = this.Model;
						primaryRequired.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(primaryRequired);
						}
					}
				}
				else if (primaryRequired != null)
				{
					primaryRequired.Remove();
				}
			}
		}
		#endregion //ObectTypeRequiresPrimarySupertype Validation
		#region PreferredIdentifierRequiresMandatoryError Validation
		/// <summary>
		/// Validator callback for PreferredIdentifierRequiresMandatoryError
		/// </summary>
		private static void DelayValidatePreferredIdentifierRequiresMandatoryError(ModelElement element)
		{
			(element as ObjectType).ValidatePreferredIdentifierRequiresMandatoryError(null);
		}
		/// <summary>
		/// Called inside a transaction to force mandatory role validation
		/// </summary>
		public void ValidateMandatoryRolesForPreferredIdentifier()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidatePreferredIdentifierRequiresMandatoryError);
		}
		/// <summary>
		/// Rule helper to determine whether or not ValidatePreferredIdentifierRequiresMandatoryError
		/// should be attached to the ObjectType.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidatePreferredIdentifierRequiresMandatoryError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = false;
				ExternalUniquenessConstraint pid = PreferredIdentifier as ExternalUniquenessConstraint;
				if (pid != null)
				{
					hasError = true;
					RoleMoveableCollection constraintRoles = pid.RoleCollection;
					int constraintRoleCount = constraintRoles.Count;
					for (int i = 0; hasError && i < constraintRoleCount; ++i)
					{
						Role constrainedRole = constraintRoles[i];
						RoleBaseMoveableCollection factRoles = constrainedRole.FactType.RoleCollection;
						Debug.Assert(factRoles.Count == 2); // Should not be a preferred identifier otherwise
						Role oppositeRole = factRoles[0].Role;
						if (object.ReferenceEquals(oppositeRole, constrainedRole))
						{
							oppositeRole = factRoles[1].Role;
						}
						ConstraintRoleSequenceMoveableCollection constraintRoleSequences = oppositeRole.ConstraintRoleSequenceCollection;
						int roleSequenceCount = constraintRoleSequences.Count;
						for (int j = 0; hasError && j < roleSequenceCount; ++j)
						{
							ConstraintRoleSequence roleSequence = constraintRoleSequences[j];
							IConstraint constraint = roleSequence.Constraint;
							switch (constraint.ConstraintType)
							{
								case ConstraintType.SimpleMandatory:
									hasError = false;
									break;
								case ConstraintType.DisjunctiveMandatory:
									// If all of the roles are opposite to preferred
									// identifier then this is sufficient to satisfy the
									// mandatory condition.
									{
										RoleMoveableCollection intersectingRoles = roleSequence.RoleCollection;
										int intersectingRolesCount = intersectingRoles.Count;
										int k = 0;
										for (; k < intersectingRolesCount; ++k)
										{
											Role testRole = intersectingRoles[k];
											if (!object.ReferenceEquals(oppositeRole, testRole))
											{
												RoleBaseMoveableCollection testRoles = testRole.FactType.RoleCollection;
												if (testRoles.Count != 2)
												{
													break;
												}
												Role testOppositeRole = testRoles[0].Role;
												if (object.ReferenceEquals(testOppositeRole, testRole))
												{
													testOppositeRole = testRoles[1].Role;
												}
												if (!constraintRoles.Contains(testOppositeRole))
												{
													break;
												}
											}
										}
										if (k == intersectingRolesCount)
										{
											hasError = false;
										}
									}
									break;
							}
						}
					}
				}
				PreferredIdentifierRequiresMandatoryError mandatoryRequired = this.PreferredIdentifierRequiresMandatoryError;
				if (hasError)
				{
					if (mandatoryRequired == null)
					{
						mandatoryRequired = PreferredIdentifierRequiresMandatoryError.CreatePreferredIdentifierRequiresMandatoryError(this.Store);
						mandatoryRequired.ObjectType = this;
						mandatoryRequired.Model = this.Model;
						mandatoryRequired.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(mandatoryRequired);
						}
					}
				}
				else if (mandatoryRequired != null)
				{
					mandatoryRequired.Remove();
				}
			}
		}
		#endregion // PreferredIdentifierRequiresMandatoryError Validation
		#region CompatibleSupertypesError Validation
		/// <summary>
		/// Validator callback for CompatibleSupertypesError
		/// </summary>
		private static void DelayValidateCompatibleSupertypesError(ModelElement element)
		{
			(element as ObjectType).ValidateCompatibleSupertypesError(null);
		}
		/// <summary>
		/// Rule helper to determine whether or not CompatibleSupertypesError
		/// should be attached to the ObjectType.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateCompatibleSupertypesError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = false;
				Dictionary<ObjectType, int> visitedNodes = null;
				bool firstSupertypeComplete = false;
				ObjectTypeVisitorResult lastResult = ObjectTypeVisitorResult.Continue;
				WalkSupertypes(this, delegate(ObjectType type, int depth)
				{
					switch (depth)
					{
						case 0:
							return ObjectTypeVisitorResult.Continue; // Called for this object
						case 1:
							if (null == visitedNodes)
							{
								visitedNodes = new Dictionary<ObjectType, int>();
							}
							else if (firstSupertypeComplete)
							{
								if (lastResult != ObjectTypeVisitorResult.SkipChildren)
								{
									hasError = true;
									return ObjectTypeVisitorResult.Stop;
								}
							}
							else
							{
								firstSupertypeComplete = true;
							}
							break;
					}
					ObjectTypeVisitorResult retVal = ObjectTypeVisitorResult.Continue;
					if (firstSupertypeComplete)
					{
						int existingDepth;
						if (visitedNodes.TryGetValue(type, out existingDepth))
						{
							// If our current depth is 1 of the existing depth
							// is one then we're in a transitive condition, which
							// is not allowed.
							if (depth == 1 || existingDepth == 1)
							{
								hasError = true;
								retVal = ObjectTypeVisitorResult.Stop;
							}
							else
							{
								retVal = ObjectTypeVisitorResult.SkipChildren;
							}
						}
						else
						{
							visitedNodes.Add(type, depth);
						}
					}
					else
					{
						visitedNodes.Add(type, depth);
					}
					lastResult = retVal;
					return retVal;
				});
				if (!hasError && firstSupertypeComplete && lastResult != ObjectTypeVisitorResult.SkipChildren)
				{
					hasError = true;
				}
				CompatibleSupertypesError incompatibleSupertypes = this.CompatibleSupertypesError;
				if (hasError)
				{
					if (incompatibleSupertypes == null)
					{
						incompatibleSupertypes = CompatibleSupertypesError.CreateCompatibleSupertypesError(this.Store);
						incompatibleSupertypes.ObjectType = this;
						incompatibleSupertypes.Model = this.Model;
						incompatibleSupertypes.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(incompatibleSupertypes);
						}
					}
				}
				else if (incompatibleSupertypes != null)
				{
					incompatibleSupertypes.Remove();
				}
			}
		}
		#endregion // CompatibleSupertypesError Validation
		#region EntityTypeRequiresReferenceSchemeError Rules
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
		private class VerifyReferenceSchemeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType objectType = link.PreferredIdentifierFor;
				ORMMetaModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
				if (link.PreferredIdentifier is ExternalUniquenessConstraint)
				{
					ORMMetaModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
				}
			}
		}
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
		private class VerifyReferenceSchemeRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType objectType = link.PreferredIdentifierFor;
				if (!objectType.IsRemoved)
				{
					ORMMetaModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
					if (link.PreferredIdentifier is ExternalUniquenessConstraint)
					{
						ORMMetaModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
					}
				}
			}
		}
		[RuleOn(typeof(Objectification))]
		private class VerifyObjectificationAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				ORMMetaModel.DelayValidateElement(link.NestingType, DelayValidateEntityTypeRequiresReferenceSchemeError);
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class VerifyValueTypeHasDataTypeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ORMMetaModel.DelayValidateElement(link.ValueTypeCollection, DelayValidateEntityTypeRequiresReferenceSchemeError);
			}
		}
		[RuleOn(typeof(Objectification))]
		private class VerifyObjectificationRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				ORMMetaModel.DelayValidateElement(link.NestingType, DelayValidateEntityTypeRequiresReferenceSchemeError);
			}
		}
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class VerifyValueTypeHasDataTypeRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ORMMetaModel.DelayValidateElement(link.ValueTypeCollection, DelayValidateEntityTypeRequiresReferenceSchemeError);
			}
		}
		/// <summary>
		/// Calls the validation of all FactType related errors
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType))]
		private class ModelHasObjectTypeAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
				link.ObjectTypeCollection.DelayValidateErrors();
			}
		}
		/// <summary>
		/// The reference scheme requirements change when the supertype changes
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class SupertypeAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				if (role is SubtypeMetaRole)
				{
					ObjectType objectType = link.RolePlayer;
					ORMMetaModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
					ORMMetaModel.DelayValidateElement(objectType, DelayValidateObjectTypeRequiresPrimarySupertypeError);
					WalkSubtypes(role.RolePlayer, delegate(ObjectType type, int depth)
					{
						ORMMetaModel.DelayValidateElement(type, DelayValidateCompatibleSupertypesError);
						ValidateAttachedConstraintColumnCompatibility(type);
						return ObjectTypeVisitorResult.Continue;
					});
				}
				else if (role is SupertypeMetaRole)
				{
					WalkSupertypes(role.RolePlayer, delegate(ObjectType type, int depth)
					{
						if (depth != 0)
						{
							ValidateAttachedConstraintColumnCompatibility(type);
						}
						return ObjectTypeVisitorResult.Continue;
					});
				}
			}
		}
		/// <summary>
		/// The reference scheme requirements change when the supertype changes
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class SupertypeRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				SubtypeMetaRole role = link.PlayedRoleCollection as SubtypeMetaRole;
				if (role != null)
				{
					ObjectType objectType = link.RolePlayer;
					if (!objectType.IsRemoved)
					{
						ORMMetaModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
						ORMMetaModel.DelayValidateElement(objectType, DelayValidateObjectTypeRequiresPrimarySupertypeError);
					}
				}
			}
		}
		/// <summary>
		/// Subtypes need to check super type compatibility when a subtype link is removing
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class SupertypeRemovingRule : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				if (role is SubtypeMetaRole)
				{
					WalkSubtypes(role.RolePlayer, delegate(ObjectType type, int depth)
					{
						ORMMetaModel.DelayValidateElement(type, DelayValidateCompatibleSupertypesError);
						// Keep going while we're here to see if we need to validate compatible role
						ValidateAttachedConstraintColumnCompatibility(type);
						return ObjectTypeVisitorResult.Continue;
					});
				}
				else if (role is SupertypeMetaRole)
				{
					WalkSupertypes(role.RolePlayer, delegate(ObjectType type, int depth)
					{
						if (depth != 0) // The node itself will be picked up as a subtype, no need to do it twice
						{
							ValidateAttachedConstraintColumnCompatibility(type);
						}
						return ObjectTypeVisitorResult.Continue;
					});
				}
			}
		}
		/// <summary>
		/// Helper function for SupertypeRemovingRule
		/// </summary>
		/// <param name="type"></param>
		private static void ValidateAttachedConstraintColumnCompatibility(ObjectType type)
		{
			RoleMoveableCollection playedRoles = type.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				if (!playedRole.IsRemoving)
				{
					ConstraintRoleSequenceMoveableCollection sequences = playedRole.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = 0; j < sequenceCount; ++j)
					{
						ConstraintRoleSequence sequence = sequences[j];
						if (!sequence.IsRemoving)
						{
							IConstraint constraint = sequence.Constraint;
							if (constraint != null &&
								0 != (constraint.RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
							{
								constraint.ValidateColumnCompatibility();
							}
						}
					}
				}
			}
		}
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class MandatoryRoleAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection;
				if (sequence is MultiColumnExternalConstraintRoleSequence)
				{
					return;
				}
				ConstraintType constraintType = ((IConstraint)sequence).ConstraintType;
				switch (constraintType)
				{
					case ConstraintType.SimpleMandatory:
					case ConstraintType.DisjunctiveMandatory:
						RoleMoveableCollection roles = sequence.RoleCollection;
						int roleCount = roles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							Role role = roles[i];
							ObjectType objectType;
							ExternalUniquenessConstraint pid;
							if (null != (objectType = role.RolePlayer) &&
								null != (pid = objectType.PreferredIdentifier as ExternalUniquenessConstraint) &&
								pid.FactTypeCollection.Contains(role.FactType))
							{
								ORMMetaModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
							}
						}
						break;
				}
			}
		}
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class MandatoryRoleRemovingRule : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection;
				if (sequence is MultiColumnExternalConstraintRoleSequence)
				{
					return;
				}
				ConstraintType constraintType = ((IConstraint)sequence).ConstraintType;
				switch (constraintType)
				{
					case ConstraintType.SimpleMandatory:
					case ConstraintType.DisjunctiveMandatory:
						RoleMoveableCollection roles = sequence.RoleCollection;
						int roleCount = roles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							Role role = roles[i];
							ObjectType objectType;
							ExternalUniquenessConstraint pid;
							if (null != (objectType = role.RolePlayer) &&
								!objectType.IsRemoving &&
								null != (pid = objectType.PreferredIdentifier as ExternalUniquenessConstraint) &&
								!pid.IsRemoving &&
								pid.FactTypeCollection.Contains(role.FactType))
							{
								ORMMetaModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
							}
						}
						break;
				}
			}
		}
		#endregion // EntityTypeRequiresReferenceSchemeError Rules
		#region ObjectTypeRequiresPrimarySupertypeError Rules
		/// <summary>
		/// If a subtypefact is set as primary then clear the primary
		/// subtype from other facts.
		/// </summary>
		[RuleOn(typeof(SubtypeFact))]
		private class SubtypeFactChangeRule : ChangeRule
		{
			private bool myIgnoreRule;
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (myIgnoreRule)
				{
					return;
				}
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == SubtypeFact.IsPrimaryMetaAttributeGuid)
				{
					bool newValue = (bool)e.NewValue;
					if (!newValue)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactPrimaryMustBeTrue);
					}
					SubtypeFact changedFact = e.ModelElement as SubtypeFact;
					ObjectType subtype = changedFact.Subtype;
					try
					{
						myIgnoreRule = true;
						foreach (Role role in subtype.PlayedRoleCollection)
						{
							if (role is SubtypeMetaRole)
							{
								SubtypeFact subtypeFact = role.FactType as SubtypeFact;
								if (!object.ReferenceEquals(subtypeFact, changedFact))
								{
									subtypeFact.IsPrimary = false;
								}
							}
						}
					}
					finally
					{
						myIgnoreRule = false;
						ORMMetaModel.DelayValidateElement(subtype, DelayValidateObjectTypeRequiresPrimarySupertypeError);
					}
				}
			}
		}
		#endregion //ObjectTypeRequiresPrimarySupertypeError Rules
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the errors associated with the object.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & ModelErrorUses.Verbalize))
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

				IModelErrorOwner valueErrors = ValueConstraint as IModelErrorOwner;
				if (valueErrors != null)
				{
					foreach (ModelErrorUsage valueError in valueErrors.GetErrorCollection(filter))
					{
						yield return valueError;
					}
				}

				ObjectTypeRequiresPrimarySupertypeError primarySupertypeRequired = ObjectTypeRequiresPrimarySupertypeError;
				if (primarySupertypeRequired != null)
				{
					yield return primarySupertypeRequired;
				}

				PreferredIdentifierRequiresMandatoryError preferredRequiresMandatory = PreferredIdentifierRequiresMandatoryError;
				if (preferredRequiresMandatory != null)
				{
					yield return preferredRequiresMandatory;
				}

				CompatibleSupertypesError compatibleSupertypes = CompatibleSupertypesError;
				if (compatibleSupertypes != null)
				{
					yield return compatibleSupertypes;
				}

				ObjectTypeDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
			}

			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			ValidateDataTypeNotSpecifiedError(notifyAdded);
			ValidateRequiresReferenceScheme(notifyAdded);
			ValidateObjectTypeRequiresPrimarySupertypeError(notifyAdded);
			ValidatePreferredIdentifierRequiresMandatoryError(notifyAdded);
			ValidateCompatibleSupertypesError(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new void DelayValidateErrors()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidateDataTypeNoteSpecifiedError);
			ORMMetaModel.DelayValidateElement(this, DelayValidateEntityTypeRequiresReferenceSchemeError);
			ORMMetaModel.DelayValidateElement(this, DelayValidateObjectTypeRequiresPrimarySupertypeError);
			ORMMetaModel.DelayValidateElement(this, DelayValidatePreferredIdentifierRequiresMandatoryError);
			ORMMetaModel.DelayValidateElement(this, DelayValidateCompatibleSupertypesError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
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
					ObjectType player = newRole.RoleCollection.Role.RolePlayer;
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
				: base (modelElement , metaAttributeInfo, requestor, attributes)
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
	#region EntityTypeRequiresReferenceSchemeError class
	partial class EntityTypeRequiresReferenceSchemeError : IRepresentModelElements
	{
		#region Base Overrides
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
		#endregion // Base Overrides
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
	#endregion // EntityTypeRequiresReferenceSchemeError class
	#region ObjectTypeRequiresPrimarySupertypeError class
	public partial class ObjectTypeRequiresPrimarySupertypeError : IRepresentModelElements
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypeRequiresPrimarySupertypeError, ObjectType.Name, Model.Name);
			if (Name != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate error text when the object name changes or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get { return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange; }
		}
		#endregion //Base Overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Returns object associated with this error
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ObjectType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // ObjectTypeRequiresPrimarySupertypeError class
	#region PreferredIdentifierRequiresMandatoryError class
	public partial class PreferredIdentifierRequiresMandatoryError : IRepresentModelElements
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			Name = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypePreferredIdentifierRequiresMandatoryError, ObjectType.Name, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the object name changes or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get { return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange; }
		}
		#endregion //Base Overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Returns object associated with this error
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ObjectType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // PreferredIdentifierRequiresMandatoryError class
	#region CompatibleSupertypesError class
	public partial class CompatibleSupertypesError : IRepresentModelElements
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			Name = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypeCompatibleSupertypesError, ObjectType.Name, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the object name changes or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get { return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange; }
		}
		#endregion //Base Overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Returns object associated with this error
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.ObjectType };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // CompatibleSupertypesError class
}
