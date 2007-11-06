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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ObjectTypeVisitor delegate definition
	/// <summary>
	/// A callback definition used for walking subtype and supertype hierarchies.
	/// </summary>
	/// <param name="type">The ObjectType being visited</param>
	/// <param name="depth">The distance from the initial recursion point. depth
	/// 0 is the starting object.</param>
	/// <param name="isPrimary">true if the object type is reached through a primary SubtypeFact</param>
	/// <returns>Value from <see cref="ObjectTypeVisitorResult"/> enum</returns>
	public delegate ObjectTypeVisitorResult ObjectTypeVisitor(ObjectType type, int depth, bool isPrimary);
	/// <summary>
	/// Expected results from the ObjectTypeVisitor delegate
	/// </summary>
	public enum ObjectTypeVisitorResult
	{
		/// <summary>
		/// Continue recursion
		/// </summary>
		Continue,
		/// <summary>
		/// Stop recursion of both following siblings and children
		/// </summary>
		Stop,
		/// <summary>
		/// Continue iterating siblings, but not children
		/// </summary>
		SkipChildren,
		/// <summary>
		/// Continue iterating children, but not following siblings
		/// </summary>
		SkipFollowingSiblings,
	}
	#endregion // ObjectTypeVisitor delegate definition
	public partial class ObjectType : INamedElementDictionaryChild, INamedElementDictionaryParent, INamedElementDictionaryRemoteParent, IModelErrorOwner, IHasIndirectModelErrorOwner, IVerbalizeCustomChildren, IHierarchyContextEnabled
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
		private void SetIsValueTypeValue(bool newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetScaleValue(int newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetLengthValue(int newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetDataTypeDisplayValue(DataType newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetNestedFactTypeDisplayValue(FactType newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetReferenceModeDisplayValue(object newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetReferenceModeValue(ReferenceMode newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetReferenceModeStringValue(string newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetValueRangeTextValue(string newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetValueTypeValueRangeTextValue(string newValue)
		{
			// Handled by ObjectTypeChangeRule
		}
		private void SetNoteTextValue(string newValue)
		{
			// Handled by ObjectTypeChangeRule
		}

		private bool GetIsValueTypeValue()
		{
			return this.DataType != null;
		}
		/// <summary>
		/// A variant on the <see cref="IsValueType"/> property.
		/// This will return false if the conditions to make this
		/// a value type are currently being deleted independently
		/// of the ObjectType itself.
		/// </summary>
		public bool IsValueTypeCheckDeleting
		{
			get
			{
				ValueTypeHasDataType link = ValueTypeHasDataType.GetLinkToDataType(this);
				return link != null && (!link.IsDeleting || IsDeleting);
			}
		}
		private int GetScaleValue()
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
		private int GetLengthValue()
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
		private DataType GetDataTypeDisplayValue()
		{
			// If this ObjecType has a reference mode, return its DataType.
			ObjectType refModeRolePlayer = GetValueTypeForPreferredConstraint();
			return (refModeRolePlayer != null) ? refModeRolePlayer.DataType : this.DataType;
		}
		private object GetReferenceModeDisplayValue()
		{
			ReferenceMode refMode;
			string referenceModeString;
			this.GetReferenceMode(out refMode, out referenceModeString);
			return (object)refMode ?? referenceModeString;
		}
		private string GetReferenceModeStringValue()
		{
			ReferenceMode refMode;
			string referenceModeString;
			this.GetReferenceMode(out refMode, out referenceModeString);
			return referenceModeString;
		}
		private ReferenceMode GetReferenceModeValue()
		{
			ReferenceMode refMode;
			string referenceModeString;
			GetReferenceMode(out refMode, out referenceModeString);
			return refMode;
		}
		private FactType GetNestedFactTypeDisplayValue()
		{
			return NestedFactType;
		}
		private string GetValueRangeTextValue()
		{
			ValueConstraint valueConstraint = FindValueConstraint(false);
			return (valueConstraint != null) ? valueConstraint.Text : String.Empty;
		}
		private string GetValueTypeValueRangeTextValue()
		{
			ValueConstraint valueConstraint = FindValueTypeValueConstraint(false);
			return (valueConstraint != null) ? valueConstraint.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		/// <summary>
		/// Return the link object between a value type and its referenced
		/// data type object.
		/// </summary>
		/// <returns>ValueTypeHasDataType relationship</returns>
		public ValueTypeHasDataType GetDataTypeLink()
		{
			return ValueTypeHasDataType.GetLinkToDataType(this);
		}
		#endregion // CustomStorage handlers
		#region Objectification Property
		/// <summary>
		/// Return the Objectification relationship that
		/// attaches this object to its nested fact
		/// </summary>
		public Objectification Objectification
		{
			get
			{
				return Neumont.Tools.ORM.ObjectModel.Objectification.GetLinkToNestedFactType(this);
			}
		}
		#endregion // Objectification Property
		#region Customize property display
		/// <summary>
		/// Return a simple name instead of a name decorated with the type (the
		/// default for a ModelElement). This is the easiest way to display
		/// clean names in the property grid when we reference properties.
		/// </summary>
		public override string ToString()
		{
			return Name;
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

			FactType refFact = new FactType(store);
			refFact.Model = model;

			if (valueType == null)
			{
				valueType = new ObjectType(store);
				valueType.Name = valueTypeName;
				valueType.Model = model;
				valueType.IsValueType = true;
			}

			Role objectTypeRole = new Role(store);
			objectTypeRole.RolePlayer = this;
			LinkedElementCollection<RoleBase> roleCollection = refFact.RoleCollection;
			roleCollection.Add(objectTypeRole);

			Role valueTypeRole = new Role(store);
			valueTypeRole.RolePlayer = valueType;
			roleCollection.Add(valueTypeRole);

			UniquenessConstraint ic = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
			ic.RoleCollection.Add(valueTypeRole); // Automatically sets FactType
			this.PreferredIdentifier = ic;

			ReadingOrder readingOrder1 = new ReadingOrder(store);
			LinkedElementCollection<RoleBase> roles = refFact.RoleCollection;
			LinkedElementCollection<RoleBase> readingRoles = readingOrder1.RoleCollection;
			readingRoles.Add(roles[0]);
			readingRoles.Add(roles[1]);
			readingOrder1.AddReading(ResourceStrings.ReferenceModePredicateReading);
			readingOrder1.FactType = refFact;

			ReadingOrder readingOrder2 = new ReadingOrder(store);
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
		///  Utility function to change the name of an existing reference mode.
		/// </summary>
		/// <param name="valueTypeName"></param>
		public void RenameReferenceMode(string valueTypeName)
		{
			UniquenessConstraint preferredConstraint = this.PreferredIdentifier;
			LinkedElementCollection<Role> constraintRoles;
			if (preferredConstraint.IsObjectifiedSingleRolePreferredIdentifier ||
				// Sanity check because this is a public method, will not happen from our codebase
				(constraintRoles = preferredConstraint.RoleCollection).Count != 1)
			{
				CreateReferenceMode(valueTypeName);
				return;
			}
			ORMModel model = this.Model;
			ObjectType valueType = FindValueType(valueTypeName, model);
			Role constrainedRole = constraintRoles[0];
			if (!IsValueTypeShared(preferredConstraint) && valueType == null)
			{
				valueType = constrainedRole.RolePlayer;
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
					valueType = new ObjectType(store);
					valueType.Name = valueTypeName;
					valueType.Model = model;
					valueType.IsValueType = true;
				}

				if (!IsValueTypeShared(preferredConstraint))
				{
					constrainedRole.RolePlayer.Delete();
				}

				constrainedRole.RolePlayer = valueType;
			}
		}

		/// <summary>
		/// Utility function to remove the reference mode objects.  Removes the fact, value type, and
		/// preffered internal uniqueness constraint.
		/// </summary>
		/// <param name="aggressivelyKillValueType">Allow removing the value type along with the reference mode predicate</param>
		private void KillReferenceMode(bool aggressivelyKillValueType)
		{
			UniquenessConstraint preferredConstraint = this.PreferredIdentifier;
			if (preferredConstraint.IsInternal && !preferredConstraint.IsObjectifiedSingleRolePreferredIdentifier)
			{
				LinkedElementCollection<Role> constraintRoles = preferredConstraint.RoleCollection;
				if (constraintRoles.Count == 1)
				{
					Role constrainedRole = constraintRoles[0];
					ObjectType valueType = constrainedRole.RolePlayer;
					if (valueType.IsValueType)
					{
						if (!IsValueTypeShared(preferredConstraint) && aggressivelyKillValueType)
						{
							valueType.Delete();
						}
						constrainedRole.FactType.Delete();
					}
				}
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
				UniquenessConstraint preferredConstraint = PreferredIdentifier;
				return (preferredConstraint != null && preferredConstraint.IsInternal) ? IsValueTypeShared(preferredConstraint) : false;
			}
		}
		private static bool IsValueTypeShared(UniquenessConstraint preferredConstraint)
		{
			if (preferredConstraint != null && preferredConstraint.IsInternal)
			{
				LinkedElementCollection<Role> constraintRoles = preferredConstraint.RoleCollection;
				ObjectType valueType;
				if (constraintRoles.Count == 1 && (valueType = constraintRoles[0].RolePlayer).IsValueType)
				{
					ReadOnlyCollection<ElementLink> links = DomainRoleInfo.GetAllElementLinks(valueType);
					int linkCount = links.Count;
					if (linkCount > 3) // Easy initial check
					{
						int count = 0;
						DomainModelInfo nativeModel = preferredConstraint.GetDomainClass().DomainModel;
						for (int i = 0; i < linkCount; ++i)
						{
							ElementLink link = links[i];
							if (!link.IsDeleting &&
								link.GetDomainClass().DomainModel == nativeModel &&
								!(link is ORMModelElementHasExtensionElement) &&
								!(link is ObjectTypeImpliesMandatoryConstraint) &&
								!(link is ElementAssociatedWithModelError))
							{
								++count;
								// We're expecting a ValueTypeHasDataType,
								// ObjectTypePlaysRole, and ModelHasObjectType from our
								// object model, plus an arbitrary number of links from
								// outside our model. Any other links (except
								// ORMExtendableElementHasExtensionElement-derived links)
								// indicate a shared value type.
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
		/// Retrieves the role player on the preferred internal uniqueness constraint.
		/// </summary>
		/// <returns>The role player as an ObjectType if it exists; otherwise, null.</returns>
		private ObjectType GetObjectTypeForPreferredConstraint()
		{
			UniquenessConstraint prefConstraint = this.PreferredIdentifier;

			//If there is a preferred internal uniqueness constraint and that uniqueness constraint's role
			// player is a value type then return the value type.
			if (prefConstraint != null && prefConstraint.IsInternal)
			{
				LinkedElementCollection<Role> constraintRoles = prefConstraint.RoleCollection;
				if (constraintRoles.Count == 1)
				{
					return constraintRoles[0].RolePlayer;
				}
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
				LinkedElementCollection<Role> roleCollection = PreferredIdentifier.RoleCollection;
				if (roleCollection.Count == 1)
				{
					Role role = roleCollection[0];
					RoleValueConstraint roleValueConstraint = role.ValueConstraint;
					if (roleValueConstraint == null && autoCreate)
					{
						role.ValueConstraint = roleValueConstraint = new RoleValueConstraint(role.Store);
					}
					return roleValueConstraint as ValueConstraint;
				}
			}
			ValueTypeValueConstraint valueConstraint = this.ValueConstraint;
			if (valueConstraint == null && autoCreate)
			{
				this.ValueConstraint = valueConstraint = new ValueTypeValueConstraint(this.Store);
			}
			return valueConstraint as ValueConstraint;
		}
		/// <summary>
		/// Get the ValueTypeValueConstraint for a reference mode
		/// </summary>
		/// <param name="autoCreate">Force the value constraint to be created</param>
		/// <returns>A ValueTypeValueConstraint, or null if this is not called on an EntityType with a reference mode</returns>
		private ValueTypeValueConstraint FindValueTypeValueConstraint(bool autoCreate)
		{
			ObjectType associatedValueType = GetValueTypeForPreferredConstraint();
			ValueTypeValueConstraint valueConstraint = null;
			if (associatedValueType != null)
			{
				valueConstraint = associatedValueType.ValueConstraint;
				if (valueConstraint == null && autoCreate)
				{
					associatedValueType.ValueConstraint = valueConstraint = new ValueTypeValueConstraint(this.Store);
				}
			}
			return valueConstraint;
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
				LinkedElementCollection<Role> playedRoles = PlayedRoleCollection;
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
				LinkedElementCollection<Role> playedRoles = PlayedRoleCollection;
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
				LinkedElementCollection<Role> playedRoles = PlayedRoleCollection;
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
		/// Get the PreferredIdentifier for this object type, walking primary supertypes
		/// as needed.
		/// </summary>
		public UniquenessConstraint ResolvedPreferredIdentifier
		{
			get
			{
				UniquenessConstraint retVal = PreferredIdentifier;
				if (retVal == null)
				{
					WalkSupertypes(
						this,
						delegate(ObjectType type, int depth, bool isPrimary)
						{
							ObjectTypeVisitorResult result = ObjectTypeVisitorResult.Continue;
							if (isPrimary)
							{
								retVal = type.PreferredIdentifier;
								result = (retVal == null) ? ObjectTypeVisitorResult.SkipFollowingSiblings : ObjectTypeVisitorResult.Stop;
							}
							else if (depth != 0)
							{
								result = ObjectTypeVisitorResult.SkipChildren;
							}
							return result;
						});
				}
				return retVal;
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
			return (startingType != null) ? WalkSupertypes(startingType, startingType, 0, false, visitor) == ObjectTypeVisitorResult.Continue : false;
		}
		private static ObjectTypeVisitorResult WalkSupertypes(ObjectType startingType, ObjectType currentType, int depth, bool isPrimary, ObjectTypeVisitor visitor)
		{
			if (depth != 0 && startingType == currentType)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactCycle);
			}
			ObjectTypeVisitorResult result = visitor(currentType, depth, isPrimary);
			switch (result)
			{
				//case ObjectTypeVisitorResult.SkipFollowingSiblings:
				//case ObjectTypeVisitorResult.Continue:
				//    break;
				case ObjectTypeVisitorResult.SkipChildren:
				case ObjectTypeVisitorResult.Stop:
					return result;
			}
			++depth;
			LinkedElementCollection<Role> playedRoles = currentType.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role role = playedRoles[i];
				if (role is SubtypeMetaRole)
				{
					SubtypeFact subtypeFact;
					ObjectType supertype;
					if (null != (subtypeFact = role.FactType as SubtypeFact) &&
						null != (supertype = subtypeFact.Supertype))
					{
						switch (WalkSupertypes(startingType, supertype, depth, subtypeFact.IsPrimary && !subtypeFact.IsDeleting, visitor))
						{
							case ObjectTypeVisitorResult.Stop:
								return ObjectTypeVisitorResult.Stop;
							case ObjectTypeVisitorResult.SkipChildren:
								if (result != ObjectTypeVisitorResult.SkipFollowingSiblings)
								{
									result = ObjectTypeVisitorResult.SkipChildren;
								}
								break;
						}
						if (result == ObjectTypeVisitorResult.SkipFollowingSiblings)
						{
							break;
						}
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Recursively walk all subtypes of a given type (including the type itself).
		/// </summary>
		/// <param name="startingType">The type to begin recursion with</param>
		/// <param name="visitor">A callback delegate. Should return true to continue recursion.</param>
		/// <returns>true if the iteration completes, false if it is stopped by a positive response</returns>
		public static bool WalkSubtypes(ObjectType startingType, ObjectTypeVisitor visitor)
		{
			return (startingType != null) ? WalkSubtypes(startingType, startingType, 0, false, visitor) == ObjectTypeVisitorResult.Continue : false;
		}
		private static ObjectTypeVisitorResult WalkSubtypes(ObjectType startingType, ObjectType currentType, int depth, bool isPrimary, ObjectTypeVisitor visitor)
		{
			if (depth != 0 && startingType == currentType)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactCycle);
			}
			ObjectTypeVisitorResult result = visitor(currentType, depth, isPrimary);
			switch (result)
			{
				//case ObjectTypeVisitorResult.SkipFollowingSiblings:
				//case ObjectTypeVisitorResult.Continue:
				//    break;
				case ObjectTypeVisitorResult.SkipChildren:
				case ObjectTypeVisitorResult.Stop:
					return result;
			}
			++depth;
			LinkedElementCollection<Role> playedRoles = currentType.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role role = playedRoles[i];
				if (role is SupertypeMetaRole)
				{
					SubtypeFact subtypeFact;
					ObjectType subtype;
					if (null != (subtypeFact = role.FactType as SubtypeFact) &&
						null != (subtype = subtypeFact.Subtype))
					{
						switch (WalkSubtypes(startingType, subtype, depth, subtypeFact.IsPrimary && !subtypeFact.IsDeleting, visitor))
						{
							case ObjectTypeVisitorResult.Stop:
								return ObjectTypeVisitorResult.Stop;
							case ObjectTypeVisitorResult.SkipChildren:
								if (result != ObjectTypeVisitorResult.SkipFollowingSiblings)
								{
									result = ObjectTypeVisitorResult.SkipChildren;
								}
								break;
						}
						if (result == ObjectTypeVisitorResult.SkipFollowingSiblings)
						{
							break;
						}
					}
				}
			}
			return result;
		}
		#region Helper class for GetNearestCompatibleTypes
		/// <summary>
		/// A small helper class for the GetNearestCompatibleTypes function.
		/// Tracks how many times a given object has been visited while
		/// walking supertypes of a given object.
		/// </summary>
		private sealed class NearestCompatibleTypeNode
		{
			public NearestCompatibleTypeNode(ObjectType objectType, int lastVisitedDuring)
			{
				ObjectType = objectType;
				VisitCount = 1;
				LastVisitedDuring = lastVisitedDuring;
			}
			/// <summary>
			/// Recursively increment all VisitCount fields for this
			/// node and its children
			/// </summary>
			/// <param name="dictionary">Dictionary containing child nodes</param>
			/// <param name="currentVisitIndex">current visit index</param>
			public void IncrementVisitCounts(Dictionary<ObjectType, NearestCompatibleTypeNode> dictionary, int currentVisitIndex)
			{
				if (LastVisitedDuring != currentVisitIndex)
				{
					LastVisitedDuring = currentVisitIndex;
					++VisitCount;
					LinkedList<ObjectType> children = ChildNodes;
					if (children != null)
					{
						foreach (ObjectType child in children)
						{
							dictionary[child].IncrementVisitCounts(dictionary, currentVisitIndex);
						}
					}
				}
			}
			public delegate ObjectTypeVisitorResult NodeVisitor(NearestCompatibleTypeNode node);
			/// <summary>
			/// Walk all descendants of this object type
			/// </summary>
			/// <param name="dictionary">Dictionary containing other types</param>
			/// <param name="visitor">ObjectTypeVisitor callback</param>
			/// <returns>true if walk completed</returns>
			public bool WalkDescendants(Dictionary<ObjectType, NearestCompatibleTypeNode> dictionary, NodeVisitor visitor)
			{
				ObjectTypeVisitorResult result = visitor(this);
				switch (result)
				{
					//case ObjectTypeVisitorResult.Continue:
					//    break;
					case ObjectTypeVisitorResult.SkipChildren:
						return true;
					case ObjectTypeVisitorResult.Stop:
						return false;
				}
				LinkedList<ObjectType> children = ChildNodes;
				if (children != null)
				{
					foreach (ObjectType child in children)
					{
						if (!dictionary[child].WalkDescendants(dictionary, visitor))
						{
							return false;
						}
					}
				}
				return true;
			}
			/// <summary>
			/// The object type being tracked
			/// </summary>
			public readonly ObjectType ObjectType;
			/// <summary>
			/// The number of times this node has been visited
			/// </summary>
			public int VisitCount;
			/// <summary>
			/// A linked list of child nodes
			/// </summary>
			public LinkedList<ObjectType> ChildNodes;
			/// <summary>
			/// An index specifying the last visit so we don't
			/// increment the VisitCount twice on one pass, or
			/// count a node reachable through two paths twice.
			/// </summary>
			public int LastVisitedDuring;
		}
		#endregion // Helper class for GetNearestCompatibleTypes
		/// <summary>
		/// Return an ObjectType array containing the nearest compatible
		/// types for the given role collection.
		/// </summary>
		/// <param name="roleCollectionCollection">Set of collections of roles to walk</param>
		/// <param name="column">The column to test</param>
		/// <returns>ObjectType[]</returns>
		public static ObjectType[] GetNearestCompatibleTypes(IEnumerable<IEnumerable<Role>> roleCollectionCollection, int column)
		{
			return GetNearestCompatibleTypes(GetColumnRoleCollection(roleCollectionCollection, column));
		}
		private static IEnumerable<Role> GetColumnRoleCollection(IEnumerable<IEnumerable<Role>> roleCollectionCollection, int column)
		{
			foreach (IEnumerable<Role> row in roleCollectionCollection)
			{
				int currentColumn = 0;
				foreach (Role role in row)
				{
					if (currentColumn == column)
					{
						yield return role;
					}
					++currentColumn;
				}
			}
		}
		/// <summary>
		/// Return an ObjectType array containing the nearest compatible
		/// types for the given role collection.
		/// </summary>
		/// <param name="roleCollection">Set of roles to walk</param>
		/// <returns>ObjectType[]</returns>
		public static ObjectType[] GetNearestCompatibleTypes(IEnumerable<Role> roleCollection)
		{
			int currentRoleIndex = 0;
			int expectedVisitCount = 0;
			ObjectType firstObjectType = null;
			Dictionary<ObjectType, NearestCompatibleTypeNode> dictionary = null;
			foreach (Role currentRole in roleCollection)
			{
				// Increment first so we can use with the LastVisitedDuring field. Otherwise,
				// this is not used
				++currentRoleIndex;
				ObjectType currentObjectType = currentRole.RolePlayer;
				if (firstObjectType == null)
				{
					firstObjectType = currentObjectType;
				}
				else if (firstObjectType != currentObjectType)
				{
					if (expectedVisitCount == 0)
					{
						// First different object, delay add the initial data to the set
						dictionary = new Dictionary<ObjectType, NearestCompatibleTypeNode>();
						WalkSupertypesForNearestCompatibleTypes(dictionary, firstObjectType, 1);
						expectedVisitCount = 1;
					}

					// Process the current element
					WalkSupertypesForNearestCompatibleTypes(dictionary, currentObjectType, currentRoleIndex);
					++expectedVisitCount;
				}
			}
			ObjectType[] retVal;
			if (dictionary != null)
			{
				// Walk the elements. The shallowest node we get down any given path
				// is a valid node.
				int total = 0;
				NearestCompatibleTypeNode firstNode = dictionary[firstObjectType];
				firstNode.WalkDescendants(
					dictionary,
					delegate(NearestCompatibleTypeNode node)
					{
						if (node.VisitCount == expectedVisitCount)
						{
							if (node.LastVisitedDuring != 0)
							{
								node.LastVisitedDuring = 0;
								++total;
							}
							return ObjectTypeVisitorResult.SkipChildren;
						}
						return ObjectTypeVisitorResult.Continue;
					});
				retVal = new ObjectType[total];
				if (total != 0)
				{
					int currentIndex = 0;
					firstNode.WalkDescendants(
						dictionary,
						delegate(NearestCompatibleTypeNode node)
						{
							if (node.VisitCount == expectedVisitCount)
							{
								if (node.LastVisitedDuring != -1)
								{
									node.LastVisitedDuring = -1;
									retVal[currentIndex] = node.ObjectType;
									if (++currentIndex == total)
									{
										return ObjectTypeVisitorResult.Stop;
									}
								}
								return ObjectTypeVisitorResult.SkipChildren;
							}
							return ObjectTypeVisitorResult.Continue;
						});
				}
			}
			else if (firstObjectType != null)
			{
				return new ObjectType[] { firstObjectType };
			}
			else
			{
				retVal = new ObjectType[0];
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for GetNearestCompatibleTypes
		/// </summary>
		private static void WalkSupertypesForNearestCompatibleTypes(Dictionary<ObjectType, NearestCompatibleTypeNode> dictionary, ObjectType currentType, int currentVisitIndex)
		{
			NearestCompatibleTypeNode currentNode;
			if (dictionary.TryGetValue(currentType, out currentNode))
			{
				currentNode.IncrementVisitCounts(dictionary, currentVisitIndex);
			}
			else
			{
				currentNode = new NearestCompatibleTypeNode(currentType, currentVisitIndex);
				dictionary[currentType] = currentNode;
				foreach (ObjectType childType in currentType.SupertypeCollection)
				{
					LinkedList<ObjectType> currentChildren = currentNode.ChildNodes;
					if (currentChildren == null)
					{
						currentNode.ChildNodes = currentChildren = new LinkedList<ObjectType>();
					}
					currentChildren.AddLast(new LinkedListNode<ObjectType>(childType));
					WalkSupertypesForNearestCompatibleTypes(dictionary, childType, currentVisitIndex);
				}
			}
		}
		#endregion // Subtype and Supertype routines
		#region ObjectTypeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ObjectType)
		/// Enforces Change Rules
		/// Add or remove a ValueTypeHasDataType link depending on the value
		/// of the IsValueType property.
		/// </summary>
		private static void ObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			ObjectType objectType = e.ModelElement as ObjectType;
			if (objectType.IsImplicitBooleanValue && e.ChangeSource != ChangeSource.Rule)
			{
				// UNDONE: This should only allow rules from our domain model.
				// We also allow rules from any other domain model with the current ChangeSource check
				throw new InvalidOperationException(ResourceStrings.ImplicitBooleanValueTypeRestriction);
			}
			else if (attributeGuid == ObjectType.IsValueTypeDomainPropertyId)
			{
				bool newValue = (bool)e.NewValue;
				DataType dataType = null;
				if (newValue)
				{
					dataType = objectType.Model.DefaultDataType;
				}
				objectType.DataType = dataType;
			}
			else if (attributeGuid == ObjectType.ScaleDomainPropertyId)
			{
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
			else if (attributeGuid == ObjectType.DataTypeDisplayDomainPropertyId)
			{
				//If this objectype has a reference mode, return the datatype corresponding
				//to the ref mode's datatype.
				ObjectType refModeRolePlayer = objectType.GetValueTypeForPreferredConstraint();
				if (refModeRolePlayer != null)
				{
					objectType = refModeRolePlayer;
				}
				objectType.DataType = e.NewValue as DataType;
			}
			else if (attributeGuid == ObjectType.LengthDomainPropertyId)
			{
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
			else if (attributeGuid == ObjectType.NameDomainPropertyId)
			{
				UniquenessConstraint prefConstraint = objectType.PreferredIdentifier;

				if (prefConstraint != null && prefConstraint.IsInternal)
				{
					string newValue = (string)e.NewValue;
					string oldValue = (string)e.OldValue;
					string oldReferenceModeName = string.Empty;

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
			else if (attributeGuid == ObjectType.ReferenceModeDisplayDomainPropertyId)
			{
				SetReferenceMode(objectType, e.NewValue as ReferenceMode, e.OldValue as ReferenceMode, e.NewValue as string, e.OldValue as string, true);
			}
			else if (attributeGuid == ObjectType.ReferenceModeStringDomainPropertyId)
			{
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
				SetReferenceMode(objectType, singleMode, null, newName, e.OldValue as string, false);
			}
			else if (attributeGuid == ObjectType.ReferenceModeDomainPropertyId)
			{
				SetReferenceMode(objectType, (ReferenceMode)e.NewValue, (ReferenceMode)e.OldValue, null, null, false);
			}
			else if (attributeGuid == ObjectType.ValueRangeTextDomainPropertyId)
			{
				ValueConstraint valueConstraint = objectType.FindValueConstraint(true);
				valueConstraint.Text = (string)e.NewValue;
			}
			else if (attributeGuid == ObjectType.ValueTypeValueRangeTextDomainPropertyId)
			{
				ValueTypeValueConstraint valueConstraint = objectType.FindValueTypeValueConstraint(true);
				if (valueConstraint != null)
				{
					valueConstraint.Text = (string)e.NewValue;
				}
			}
			else if (attributeGuid == ObjectType.NoteTextDomainPropertyId)
			{
				// cache the text.
				string newText = (string)e.NewValue;
				// Get the note if it exists
				Note note = objectType.Note;
				if (note != null)
				{
					// and try to set the text to the cached value.
					note.Text = newText;
				}
				else if (!string.IsNullOrEmpty(newText))
				{
					// Otherwise, create the note and set the text,
					note = new Note(objectType.Store);
					note.Text = newText;
					// then attach the note to the RootType.
					objectType.Note = note;
				}
			}
			else if (attributeGuid == ObjectType.IsIndependentDomainPropertyId)
			{
				if ((bool)e.NewValue)
				{
					objectType.AllowIsIndependent(true);
					objectType.ImpliedMandatoryConstraint = null;
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
				}
			}
			else if (attributeGuid == ObjectType.IsImplicitBooleanValueDomainPropertyId && e.ChangeSource != ChangeSource.Rule)
			{
				throw new InvalidOperationException(ResourceStrings.ImplicitBooleanValueTypePropertyRestriction);
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
		/// <param name="forceAggressivelyKillValueType">Aggressively remove an associated value type. If this is false, then
		/// it checks for the DeleteReferenceModeValueType key in the store context.</param>
		private static void SetReferenceMode(ObjectType objectType, ReferenceMode newMode, ReferenceMode oldMode, string newModeName, string oldModeName, bool forceAggressivelyKillValueType)
		{
			bool aggressivelyKillValueType = forceAggressivelyKillValueType ?
				true :
				objectType.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(DeleteReferenceModeValueType);
			string newValue = newModeName ?? string.Empty;
			string oldValue = oldModeName ?? string.Empty;

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
			if (newMode != null)
			{
				//Now, set the dataType
				DataType dataType = null;
				ORMModel ormModel = objectType.Model;
				dataType = ormModel.GetPortableDataType(newMode.Type);
				//Change the objectType to the ref mode's preferred valueType and set the
				//dataType on that objectType.
				//Unless things change and the refMode can be set on objects without a preferred constraint,
				//the objectType will always be changed.
				ObjectType refModeRolePlayer = objectType.GetValueTypeForPreferredConstraint();
				if (refModeRolePlayer != null)
				{
					refModeRolePlayer.DataType = dataType;
				}
			}
		}
		#endregion // ObjectTypeChangeRule
		#region ObjectTypeDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ObjectType)
		/// Enforces Deleting Rules
		/// </summary>
		private static void ObjectTypeDeletingRule(ElementDeletingEventArgs e)
		{
			ObjectType objectType = (ObjectType)e.ModelElement;
			objectType.ReferenceModeDisplay = string.Empty;
		}
		#endregion //ObjectTypeDeletingRule
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ModelHasObjectType.ModelDomainRoleId;
			childDomainRoleId = ModelHasObjectType.ObjectTypeDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { ValueTypeHasValueConstraint.ValueTypeDomainRoleId };
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
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		protected INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == ValueTypeHasValueConstraint.ValueTypeDomainRoleId)
			{
				ORMModel model = Model;
				if (model != null)
				{
					return ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
				}
			}
			return null;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetAllowDuplicateNamesContextKey(parentDomainRoleId, childDomainRoleId);
		}
		#endregion // INamedElementDictionaryParent implementation
		#region IsIndependent Validation
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Verify IsIndependent settings on attached objects when modality changes
		/// </summary>
		private static void MandatoryModalityChangeRule(ElementPropertyChangedEventArgs e)
		{
			// Make sure that we retest IsIndependent settings
			if (e.DomainProperty.Id == MandatoryConstraint.ModalityDomainPropertyId &&
				((ConstraintModality)e.NewValue) == ConstraintModality.Alethic)
			{
				LinkedElementCollection<Role> roles = ((MandatoryConstraint)e.ModelElement).RoleCollection;
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					ObjectType objectType;
					if (null != (objectType = roles[i].RolePlayer))
					{
						FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
					}
				}
			}
		}
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that all facts that should have implied objectifications
		/// have implied objectifications
		/// </summary>
		public static IDeserializationFixupListener IsIndependentFixupListener
		{
			get
			{
				return new TestRemoveIsIndependentFixupListener();
			}
		}
		/// <summary>
		/// A fixup listener to clear the IsIndependent setting on all object types in a model
		/// if they do not meet the IsIndependent requirements (no non-existential mandatory roles).
		/// </summary>
		private sealed class TestRemoveIsIndependentFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// Create a new SubtypeFactFixupListener
			/// </summary>
			public TestRemoveIsIndependentFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Make sure that all contained independent ObjectType elements are allowed to be independent
			/// </summary>
			/// <param name="element">An ORMModel instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ORMModel element, Store store, INotifyElementAdded notifyAdded)
			{
				int objectTypeCount;
				LinkedElementCollection<ObjectType> objectTypes;
				if (!element.IsDeleted &&
					0 != (objectTypeCount = (objectTypes = element.ObjectTypeCollection).Count))
				{
					for (int i = 0; i < objectTypeCount; ++i)
					{
						objectTypes[i].ValidateIsIndependent(notifyAdded);
					}
				}
			}
		}
		/// <summary>
		/// Validation callback to ensure that the IsIndependent property is set correctly
		/// </summary>
		/// <param name="element">Element to validate</param>
		private static void DelayValidateIsIndependent(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((ObjectType)element).ValidateIsIndependent(null);
			}
		}
		/// <summary>
		/// Verify that the <see cref="IsIndependent"/> property is off if it should
		/// not be set and automatically add/remove an implied mandatory constraint
		/// as needed.
		/// </summary>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private void ValidateIsIndependent(INotifyElementAdded notifyAdded)
		{
			bool canBeIndependent = true;
			bool currentIsIndependent = IsIndependent;
			LinkedElementCollection<Role> preferredIdentifierRoles = null;
			bool checkedPreferredRoles = false;
			LinkedElementCollection<Role> playedRoles = PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			MandatoryConstraint impliedMandatory = ImpliedMandatoryConstraint;
			LinkedElementCollection<Role> impliedMandatoryRoles = (impliedMandatory != null) ? impliedMandatory.RoleCollection : null;
			bool seenNonMandatoryRole = false;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				bool currentRoleIsAlreadyImplied = false;
				bool currentRoleIsWithPreferredIdentifier = false;
				bool currentRoleIsMandatory = false;
				LinkedElementCollection<ConstraintRoleSequence> constraints = playedRole.ConstraintRoleSequenceCollection;
				int constraintCount = constraints.Count;
				for (int j = 0; j < constraintCount; ++j)
				{
					MandatoryConstraint mandatoryConstraint;
					if (null != (mandatoryConstraint = constraints[j] as MandatoryConstraint) &&
						mandatoryConstraint.Modality == ConstraintModality.Alethic)
					{
						if (mandatoryConstraint.IsImplied)
						{
							if (impliedMandatory == null || impliedMandatory != mandatoryConstraint)
							{
								// This occurs in a role player change situation when the role player is changed
								// from one ObjectType with an existing implied MandatoryConstraint to another
								// ObjectType with an existing implied MandatoryConstraint. Other role players
								// will be cleaned up in different passes over this function.
								Debug.Assert(mandatoryConstraint.ImpliedByObjectType != this, "The implied mandatory should be on a different object type");
								continue;
							}
							currentRoleIsAlreadyImplied = true;
							continue;
						}
						else if (!canBeIndependent)
						{
							// We're only staying in the loop to look for an implied mandatory
							continue;
						}
						currentRoleIsMandatory = true;
						bool turnedOffCanBeIndependent = false;
						// The role must be part of a fact type that has a role in the preferred identifier.
						if (!checkedPreferredRoles)
						{
							checkedPreferredRoles = true;
							UniquenessConstraint preferredIdentifier = PreferredIdentifier;
							if (preferredIdentifier != null)
							{
								preferredIdentifierRoles = preferredIdentifier.RoleCollection;
							}
						}
						if (preferredIdentifierRoles == null)
						{
							canBeIndependent = false;
							turnedOffCanBeIndependent = true;
						}
						if (canBeIndependent)
						{
							RoleBase oppositeRole = playedRole.OppositeRole;
							if (null == oppositeRole || !preferredIdentifierRoles.Contains(oppositeRole.Role))
							{
								canBeIndependent = false;
								turnedOffCanBeIndependent = true;
							}
							else
							{
								currentRoleIsWithPreferredIdentifier = true;
							}
						}
						if (turnedOffCanBeIndependent && impliedMandatory == null)
						{
							// Look farther down the constraints on this role for an implied mandatory
							for (int k = j + 1; k < constraintCount; ++k)
							{
								MandatoryConstraint testImplied = constraints[k] as MandatoryConstraint;
								if (testImplied != null && testImplied.IsImplied)
								{
									currentRoleIsAlreadyImplied = true;
									break;
								}
							}
						}
					}
				}
				if (!currentRoleIsMandatory && canBeIndependent)
				{
					bool nonMandatoryRoleInPreferredIdentifier = false;
					if (!checkedPreferredRoles)
					{
						checkedPreferredRoles = true;
						UniquenessConstraint preferredIdentifier = PreferredIdentifier;
						if (preferredIdentifier != null)
						{
							preferredIdentifierRoles = preferredIdentifier.RoleCollection;
						}
					}
					if (preferredIdentifierRoles != null)
					{
						RoleBase oppositeRole = playedRole.OppositeRole;
						nonMandatoryRoleInPreferredIdentifier =
							null != oppositeRole &&
							preferredIdentifierRoles.Contains(oppositeRole.Role);
					}
					if (!nonMandatoryRoleInPreferredIdentifier)
					{
						seenNonMandatoryRole = true;
					}
				}
				if (impliedMandatory != null &&	canBeIndependent)
				{
					if (currentRoleIsWithPreferredIdentifier)
					{
						if (currentRoleIsAlreadyImplied)
						{
							impliedMandatoryRoles.Remove(playedRole);
						}
					}
					else if (!currentRoleIsAlreadyImplied)
					{
						// The cost of adding an extra role to the implied and
						// deleting it later is less than another n-squared walk later
						// on to see if roles are already implied.
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(new ConstraintRoleSequenceHasRole(playedRole, impliedMandatory), false);
						}
						else
						{
							impliedMandatoryRoles.Add(playedRole);
						}
					}
				}
			}
			if (canBeIndependent)
			{
				if (seenNonMandatoryRole && !currentIsIndependent)
				{
					if (impliedMandatory == null)
					{
						impliedMandatory = MandatoryConstraint.CreateImpliedMandatoryConstraint(this);
						impliedMandatoryRoles = impliedMandatory.RoleCollection;
						// Rewalk the roles and add any that are not opposite the preferred identifier
						// Note that any roles that were found after a role already in the implied mandatory
						// constraint is already in the implied constraint
						for (int i = 0; i < playedRoleCount; ++i)
						{
							Role playedRole = playedRoles[i];
							LinkedElementCollection<ConstraintRoleSequence> constraints = playedRole.ConstraintRoleSequenceCollection;
							int constraintCount = constraints.Count;
							bool roleIsMandatory = false;
							for (int j = 0; j < constraintCount; ++j)
							{
								MandatoryConstraint mandatoryConstraint;
								if (null != (mandatoryConstraint = constraints[j] as MandatoryConstraint) &&
									mandatoryConstraint.Modality == ConstraintModality.Alethic &&
									!mandatoryConstraint.IsImplied) // An implied mandatory here will be on the wrong object type
								{
									// This must be on the preferred identifier, already verified in the initial test to verify canBeIndependent loop
									roleIsMandatory = true;
									break;
								}
							}
							if (!roleIsMandatory)
							{
								impliedMandatoryRoles.Add(playedRole);
							}
						}
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(impliedMandatory, true);
						}
					}
					else
					{
						// Make sure all roles on the implied mandatory are attached to this object type
						for (int i = impliedMandatoryRoles.Count - 1; i >= 0; --i)
						{
							if (impliedMandatoryRoles[i].RolePlayer != this)
							{
								impliedMandatoryRoles.RemoveAt(i);
							}
						}
					}
				}
				else if (impliedMandatory != null)
				{
					impliedMandatory.Delete();
				}
			}
			else if (impliedMandatory != null)
			{
				impliedMandatory.Delete();
			}

			// Finally, turn off IsIndependent if it is no longer needed
			if (!canBeIndependent && currentIsIndependent)
			{
				RuleManager ruleManager = Store.RuleManager;
				Type ruleType = typeof(ObjectTypeChangeRuleClass);
				try
				{
					// Disable the rule so this does not recurse to this routine
					ruleManager.DisableRule(ruleType);
					IsIndependent = false;
				}
				finally
				{
					ruleManager.EnableRule(ruleType);
				}
			}
		}
		/// <summary>
		/// Indicates if this <see cref="ObjectType"/> is either explicitly marked as <see cref="IsIndependent">independent</see>
		/// or is implicitly independent because it plays no roles outside of its preferred identifier.
		/// </summary>
		public bool TreatAsIndependent
		{
			get
			{
				return this.IsIndependent || (!this.IsValueType && this.ImpliedMandatoryConstraint == null && this.AllowIsIndependent(false));
			}
		}
		/// <summary>
		/// Test if the <see cref="IsIndependent"/> property is true or can be set to true.
		/// </summary>
		/// <returns><see langword="true"/> if <see cref="IsIndependent"/> can be turned on.</returns>
		public bool AllowIsIndependent()
		{
			return IsIndependent || AllowIsIndependent(false);
		}
		/// <summary>
		/// Test if the <see cref="IsIndependent"/> property can be set to true.
		/// </summary>
		/// <param name="throwIfFalse">Set to <see langword="true"/> to throw an exception instead of returning false.</param>
		/// <returns><see langword="true"/> if <see cref="IsIndependent"/> can be turned on.</returns>
		private bool AllowIsIndependent(bool throwIfFalse)
		{
			bool retVal = true;
			LinkedElementCollection<Role> preferredIdentifierRoles = null;
			LinkedElementCollection<Role> playedRoles = PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount && retVal; ++i)
			{
				Role playedRole = playedRoles[i];
				LinkedElementCollection<ConstraintRoleSequence> constraints = playedRole.ConstraintRoleSequenceCollection;
				int constraintCount = constraints.Count;
				for (int j = 0; j < constraintCount; ++j)
				{
					MandatoryConstraint mandatoryConstraint;
					if (null != (mandatoryConstraint = constraints[j] as MandatoryConstraint) &&
						mandatoryConstraint.Modality == ConstraintModality.Alethic)
					{
						if (mandatoryConstraint.IsImplied)
						{
							// The pattern has already been validated. Any object with
							// an implied mandatory constraint can be independent.
							return true;
						}
						// The role must be part of a fact type that has a role in the preferred identifier.
						if (preferredIdentifierRoles == null)
						{
							UniquenessConstraint preferredIdentifier = PreferredIdentifier;
							if (preferredIdentifier == null)
							{
								retVal = false;
								break;
							}
							preferredIdentifierRoles = preferredIdentifier.RoleCollection;
						}
						RoleBase oppositeRole = playedRole.OppositeRole;
						if (null == oppositeRole || !preferredIdentifierRoles.Contains(oppositeRole.Role))
						{
							retVal = false;
							break;
						}
					}
				}
			}
			if (!retVal && throwIfFalse)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectTypeEnforceIsIndependentPattern);
			}
			return retVal;
		}
		#endregion // IsIndependent Validation
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
				ValueTypeHasDataType valueTypeHasDataType = ValueTypeHasDataType.GetLinkToDataType(this);
				return (valueTypeHasDataType != null && valueTypeHasDataType.DataType is UnspecifiedDataType) ?
					valueTypeHasDataType.DataTypeNotSpecifiedError : null;
			}
		}
		/// <summary>
		/// Validator callback for DataTypeNoteSpecifiedError
		/// </summary>
		private static void DelayValidateDataTypeNotSpecifiedError(ModelElement element)
		{
			(element as ObjectType).ValidateDataTypeNotSpecifiedError(null);
		}
		/// <summary>
		/// Validate that a DataTypeNotSpecifiedError is present if needed, and that
		/// the data type is an unspecified type instance if the error is present.
		/// </summary>
		private void ValidateDataTypeNotSpecifiedError(INotifyElementAdded notifyAdded)
		{
			ValueTypeHasDataType link = ValueTypeHasDataType.GetLinkToDataType(this);
			if (link != null)
			{
				DataTypeNotSpecifiedError error = link.DataTypeNotSpecifiedError;
				if (!(link.DataType is UnspecifiedDataType))
				{
					if (error != null)
					{
						error.Delete();
					}
				}
				else if (error == null)
				{
					error = new DataTypeNotSpecifiedError(Store);
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
		/// <summary>
		/// AddRule: typeof(ValueTypeHasDataType)
		/// Test if an added data type relationship points to
		/// an unspecified type
		/// </summary>
		private static void UnspecifiedDataTypeAddRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement((e.ModelElement as ValueTypeHasDataType).ValueType, DelayValidateDataTypeNotSpecifiedError);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void UnspecifiedDataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			Guid changedRoleGuid = e.DomainRole.Id;
			// If the data type changed, then validate the object type.
			// If the object type changed, then validate both object types.
			if (changedRoleGuid == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement((e.ElementLink as ValueTypeHasDataType).ValueType, DelayValidateDataTypeNotSpecifiedError);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateDataTypeNotSpecifiedError);
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateDataTypeNotSpecifiedError);
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
			if (!IsDeleted)
			{
				bool hasError = true;
				Store theStore = Store;
				ORMModel theModel = Model;
				if (IsValueType == true || this.PreferredIdentifier != null)
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
						noRefSchemeError = new EntityTypeRequiresReferenceSchemeError(theStore);
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
						noRefSchemeError.Delete();
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
			if (!IsDeleted)
			{
				bool hasError = false;
				ReadOnlyCollection<ObjectTypePlaysRole> links = ObjectTypePlaysRole.GetLinksToPlayedRoleCollection(this);
				int linkCount = links.Count;
				if (linkCount != 0)
				{
					SubtypeFact firstSubtypeFact = null;
					int subtypeFactCount = 0;
					//bool hasPrimarySupertypeFact = false;
					int primaryFactCount = 0;
					for (int i = 0; i < linkCount; ++i)
					{
						ObjectTypePlaysRole link = links[i];
						SubtypeMetaRole subtypeRole = link.PlayedRole as SubtypeMetaRole;
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
											link = links[j];
											subtypeRole = link.PlayedRole as SubtypeMetaRole;

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
						primaryRequired = new ObjectTypeRequiresPrimarySupertypeError(this.Store);
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
					primaryRequired.Delete();
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidatePreferredIdentifierRequiresMandatoryError);
		}
		/// <summary>
		/// Rule helper to determine whether or not ValidatePreferredIdentifierRequiresMandatoryError
		/// should be attached to the ObjectType.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidatePreferredIdentifierRequiresMandatoryError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasError = false;
				UniquenessConstraint pid = PreferredIdentifier;
				if (pid != null && !pid.IsInternal)
				{
					hasError = true;
					Objectification objectification = pid.PreferredIdentifierFor.Objectification;
					LinkedElementCollection<Role> constraintRoles = pid.RoleCollection;
					int constraintRoleCount = constraintRoles.Count;
					for (int i = 0; hasError && i < constraintRoleCount; ++i)
					{
						Role constrainedRole = constraintRoles[i];
						RoleProxy proxyRole;
						FactType impliedFactType;
						if (null != objectification &&
							null != (proxyRole = constrainedRole.Proxy) &&
							null != (impliedFactType = proxyRole.FactType) &&
							impliedFactType.ImpliedByObjectification == objectification)
						{
							// The opposite role will always have a simple mandatory on it
							hasError = false;
							break;
						}

						LinkedElementCollection<RoleBase> factRoles = constrainedRole.FactType.RoleCollection;
						Debug.Assert(factRoles.Count == 2); // Should not be a preferred identifier otherwise
						Role oppositeRole = factRoles[0].Role;
						if (oppositeRole == constrainedRole)
						{
							oppositeRole = factRoles[1].Role;
						}
						LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = oppositeRole.ConstraintRoleSequenceCollection;
						int roleSequenceCount = constraintRoleSequences.Count;
						for (int j = 0; hasError && j < roleSequenceCount; ++j)
						{
							ConstraintRoleSequence roleSequence = constraintRoleSequences[j];
							IConstraint constraint = roleSequence.Constraint;
							switch (constraint.ConstraintType)
							{
								case ConstraintType.SimpleMandatory:
									if (constraint.Modality == ConstraintModality.Alethic)
									{
										hasError = false;
									}
									break;
								case ConstraintType.DisjunctiveMandatory:
									// If all of the roles are opposite to preferred
									// identifier then this is sufficient to satisfy the
									// mandatory condition.
									if (constraint.Modality == ConstraintModality.Alethic)
									{
										LinkedElementCollection<Role> intersectingRoles = roleSequence.RoleCollection;
										int intersectingRolesCount = intersectingRoles.Count;
										int k = 0;
										for (; k < intersectingRolesCount; ++k)
										{
											Role testRole = intersectingRoles[k];
											if (oppositeRole != testRole)
											{
												LinkedElementCollection<RoleBase> testRoles = testRole.FactType.RoleCollection;
												if (testRoles.Count != 2)
												{
													break;
												}
												Role testOppositeRole = testRoles[0].Role;
												if (testOppositeRole == testRole)
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
						mandatoryRequired = new PreferredIdentifierRequiresMandatoryError(this.Store);
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
					mandatoryRequired.Delete();
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
			if (!IsDeleted)
			{
				bool hasError = false;
				Dictionary<ObjectType, int> visitedNodes = null;
				bool firstSupertypeComplete = false;
				ObjectTypeVisitorResult lastResult = ObjectTypeVisitorResult.Continue;
				int deepestSharedForFirstSupertype = 0;
				const int MultipleVisitsFlag = unchecked((int)0x80000000);
				const int FirstSupertypeBranchFlag = 0x40000000;
				const int AllFlags = MultipleVisitsFlag | FirstSupertypeBranchFlag;
				bool sawMultiples = false;
				WalkSupertypes(this, delegate(ObjectType type, int depth, bool isPrimary)
				{
					switch (depth)
					{
						case 0:
							return ObjectTypeVisitorResult.Continue; // Called for this object
						case 1:
							if (null == visitedNodes)
							{
								// UNDONE: We shouldn't need to create the dictionary until we know
								// we have at least two elements to track
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
					int existingDepth;
					if (firstSupertypeComplete)
					{
						if (visitedNodes.TryGetValue(type, out existingDepth))
						{
							int flags = existingDepth & AllFlags;
							existingDepth &= ~AllFlags;
							// If our current depth is one or the existing depth
							// is one then we're in a transitive condition, which
							// is not allowed.
							if (depth == 1 || existingDepth == 1)
							{
								hasError = true;
								retVal = ObjectTypeVisitorResult.Stop;
							}
							else
							{
								if (0 != (flags & FirstSupertypeBranchFlag))
								{
									if (existingDepth > deepestSharedForFirstSupertype)
									{
										deepestSharedForFirstSupertype = existingDepth;
									}
								}
								if (0 == (flags & MultipleVisitsFlag))
								{
									sawMultiples = true;
									visitedNodes[type] = existingDepth | flags | MultipleVisitsFlag;
								}
								retVal = ObjectTypeVisitorResult.SkipChildren;
							}
						}
						else
						{
							visitedNodes.Add(type, depth);
						}
					}
					else if (visitedNodes.TryGetValue(type, out existingDepth))
					{
						if (0 == (existingDepth & MultipleVisitsFlag))
						{
							sawMultiples = true;
							visitedNodes[type] = existingDepth | MultipleVisitsFlag;
						}
						retVal = ObjectTypeVisitorResult.SkipChildren;
					}
					else
					{
						// Flag nodes on the first supertype branch
						visitedNodes.Add(type, depth | FirstSupertypeBranchFlag);
					}
					lastResult = retVal;
					return retVal;
				});
				if (!hasError && firstSupertypeComplete && lastResult != ObjectTypeVisitorResult.SkipChildren)
				{
					hasError = true;
				}
				if (!hasError && sawMultiples)
				{
					// A subtype is not compatible if any of its supertypes are exclusive
					foreach (KeyValuePair<ObjectType, int> pair in visitedNodes)
					{
						int valueData = pair.Value;
						if ((0 != (valueData & MultipleVisitsFlag)) &&
							(0 == (valueData & FirstSupertypeBranchFlag) || (valueData & ~AllFlags) <= deepestSharedForFirstSupertype))
						{
							ObjectType superType = pair.Key;
							foreach (Role role in superType.PlayedRoleCollection)
							{
								SupertypeMetaRole superTypeRole = role as SupertypeMetaRole;
								if (null != superTypeRole)
								{
									foreach (ConstraintRoleSequence sequence in superTypeRole.ConstraintRoleSequenceCollection)
									{
										IConstraint constraint = sequence.Constraint;
										if (constraint != null && constraint.ConstraintType == ConstraintType.Exclusion && constraint.Modality == ConstraintModality.Alethic)
										{
											bool constraintIntersects = false;
											foreach (ConstraintRoleSequence exclusionSequence in ((ExclusionConstraint)constraint).RoleSequenceCollection)
											{
												foreach (Role sequenceRole in exclusionSequence.RoleCollection)
												{
													RoleBase oppositeRoleBase;
													Role oppositeRole;
													ObjectType oppositeRolePlayer;
													// Note that these are very unlikely to fail at this point,
													// but it doesn't hurt to be safe
													if (null != (oppositeRoleBase = sequenceRole.OppositeRole) &&
														null != (oppositeRole = oppositeRoleBase.Role) &&
														null != (oppositeRolePlayer = oppositeRole.RolePlayer) &&
														visitedNodes.ContainsKey(oppositeRolePlayer))
													{
														if (constraintIntersects)
														{
															hasError = true;
															goto ExclusionErrorBreak;
														}
														constraintIntersects = true;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			ExclusionErrorBreak:
				CompatibleSupertypesError incompatibleSupertypes = this.CompatibleSupertypesError;
				if (hasError)
				{
					if (incompatibleSupertypes == null)
					{
						incompatibleSupertypes = new CompatibleSupertypesError(this.Store);
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
					incompatibleSupertypes.Delete();
				}
			}
		}
		#endregion // CompatibleSupertypesError Validation
		#region EntityTypeRequiresReferenceSchemeError Rules
		/// <summary>
		/// AddRule: typeof(EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void VerifyReferenceSchemeAddRule(ElementAddedEventArgs e)
		{
			ProcessVerifyReferenceSchemeAdd(e.ModelElement as EntityTypeHasPreferredIdentifier);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessVerifyReferenceSchemeAdd(EntityTypeHasPreferredIdentifier link)
		{
			ObjectType objectType = link.PreferredIdentifierFor;
			FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
			if (!link.PreferredIdentifier.IsInternal)
			{
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void VerifyReferenceSchemeDeleteRule(ElementDeletedEventArgs e)
		{
			ProcessVerifyReferenceSchemeDelete(e.ModelElement as EntityTypeHasPreferredIdentifier, null, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessVerifyReferenceSchemeDelete(EntityTypeHasPreferredIdentifier link, ObjectType objectType, UniquenessConstraint preferredIdentifier)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (!objectType.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
				if (preferredIdentifier == null)
				{
					preferredIdentifier = link.PreferredIdentifier;
				}
				if (!preferredIdentifier.IsInternal)
				{
					FrameworkDomainModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void VerifyReferenceSchemeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			Guid changedRoleGuid = e.DomainRole.Id;
			ObjectType oldObjectType = null;
			UniquenessConstraint oldPreferredIdentifier = null;
			if (changedRoleGuid == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			else if (changedRoleGuid == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldPreferredIdentifier = (UniquenessConstraint)e.OldRolePlayer;
			}
			EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
			ProcessVerifyReferenceSchemeDelete(link, oldObjectType, oldPreferredIdentifier);
			ProcessVerifyReferenceSchemeAdd(link);
		}
		/// <summary>
		/// AddRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void VerifyValueTypeHasDataTypeAddRule(ElementAddedEventArgs e)
		{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				FrameworkDomainModel.DelayValidateElement(link.ValueType, DelayValidateEntityTypeRequiresReferenceSchemeError);
		}
		/// <summary>
		/// DeleteRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void VerifyValueTypeHasDataTypeDeleteRule(ElementDeletedEventArgs e)
		{
			ObjectType valueType = (e.ModelElement as ValueTypeHasDataType).ValueType;
			if (!valueType.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(valueType, DelayValidateEntityTypeRequiresReferenceSchemeError);
			}
		}
		/// <summary>
		/// AddRule: typeof(ModelHasObjectType)
		/// Calls the validation of all FactType related errors
		/// </summary>
		private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			link.ObjectType.DelayValidateErrors();
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// The reference scheme requirements change when the supertype changes
		/// </summary>
		private static void SupertypeAddedRule(ElementAddedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			Role role = link.PlayedRole;
			ObjectType rolePlayer = role.RolePlayer;
			FrameworkDomainModel.DelayValidateElement(rolePlayer, DelayValidateIsIndependent);
			if (role is SubtypeMetaRole)
			{
				ObjectType objectType = link.RolePlayer;
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateObjectTypeRequiresPrimarySupertypeError);
				WalkSubtypes(rolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
				{
					FrameworkDomainModel.DelayValidateElement(type, DelayValidateCompatibleSupertypesError);
					ValidateAttachedConstraintColumnCompatibility(type);
					return ObjectTypeVisitorResult.Continue;
				});
			}
			else if (role is SupertypeMetaRole)
			{
				WalkSupertypes(rolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
				{
					if (depth != 0)
					{
						ValidateAttachedConstraintColumnCompatibility(type);
					}
					return ObjectTypeVisitorResult.Continue;
				});
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// The reference scheme requirements change when the supertype is deleted
		/// </summary>
		private static void SupertypeDeleteRule(ElementDeletedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			ObjectType objectType = link.RolePlayer;
			if (!objectType.IsDeleted)
			{
				// IsIndependent pattern incorporates implied mandatories, reverify when a role player is disconnected
				FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
				SubtypeMetaRole role = link.PlayedRole as SubtypeMetaRole;
				if (role != null)
				{
					FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateEntityTypeRequiresReferenceSchemeError);
					FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateObjectTypeRequiresPrimarySupertypeError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Verify IsIndependent/ImpliedMandatoryConstraint pattern when a role player changes
		/// </summary>
		private static void CheckIsIndependentRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (link.IsDeleted)
			{
				return;
			}
			Guid changedRoleGuid = e.DomainRole.Id;
			if (changedRoleGuid == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(link.RolePlayer, DelayValidateIsIndependent);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(link.RolePlayer, DelayValidateIsIndependent);
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateIsIndependent);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ObjectTypePlaysRole)
		/// Subtypes need to check super type compatibility when a subtype link is removing
		/// </summary>
		private static void SupertypeDeletingRule(ElementDeletingEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			Role role = link.PlayedRole;
			if (role is SubtypeMetaRole)
			{
				WalkSubtypes(role.RolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
				{
					FrameworkDomainModel.DelayValidateElement(type, DelayValidateCompatibleSupertypesError);
					// Keep going while we're here to see if we need to validate compatible role
					ValidateAttachedConstraintColumnCompatibility(type);
					return ObjectTypeVisitorResult.Continue;
				});
			}
			else if (role is SupertypeMetaRole)
			{
				WalkSupertypes(role.RolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
				{
					if (depth != 0) // The node itself will be picked up as a subtype, no need to do it twice
					{
						ValidateAttachedConstraintColumnCompatibility(type);
					}
					return ObjectTypeVisitorResult.Continue;
				});
			}
		}
		/// <summary>
		/// Helper function for SupertypeDeletingRule
		/// </summary>
		private static void ValidateAttachedConstraintColumnCompatibility(ObjectType type)
		{
			LinkedElementCollection<Role> playedRoles = type.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				if (!playedRole.IsDeleting)
				{
					LinkedElementCollection<ConstraintRoleSequence> sequences = playedRole.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = 0; j < sequenceCount; ++j)
					{
						ConstraintRoleSequence sequence = sequences[j];
						if (!sequence.IsDeleting)
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
		/// <summary>
		/// ChangeRule: typeof(ExclusionConstraint)
		/// Reverify downstream subtypes if the exclusion constraint is attached to
		/// a supertype and the modality changes.
		/// </summary>
		private static void ExclusionModalityChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ExclusionConstraint.ModalityDomainPropertyId)
			{
				DelayValidateSubtypeExclusion((ExclusionConstraint)e.ModelElement);
			}
		}
		/// <summary>
		/// Rule helper function to walk subtypes that may be affected by an exclusion constraint
		/// </summary>
		/// <param name="exclusionConstraint">The modified <see cref="ExclusionConstraint"/></param>
		private static void DelayValidateSubtypeExclusion(ExclusionConstraint exclusionConstraint)
		{
			foreach (ConstraintRoleSequence roleSequence in exclusionConstraint.RoleSequenceCollection)
			{
				foreach (Role role in roleSequence.RoleCollection)
				{
					SupertypeMetaRole supertypeRole = role as SupertypeMetaRole;
					if (supertypeRole == null)
					{
						// We can't mix and match these, there is nothing else to do.
						return;
					}
					RoleBase oppositeRoleBase;
					Role oppositeRole;
					ObjectType oppositeRolePlayer;
					if (null != (oppositeRoleBase = supertypeRole.OppositeRole) &&
						null != (oppositeRole = oppositeRoleBase.Role) &&
						null != (oppositeRolePlayer = oppositeRole.RolePlayer))
					{
						WalkSubtypes(oppositeRolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
						{
							if (depth != 0)
							{
								DelayValidateCompatibleSupertypesError(type);
							}
							return ObjectTypeVisitorResult.Continue;
						});
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// Test subtype compatibility when an exclusion constraint sequence is
		/// added with roles already attached
		/// </summary>
		private static void ExclusionSequenceAddedRule(ElementAddedEventArgs e)
		{
			SetComparisonConstraintHasRoleSequence link = e.ModelElement as SetComparisonConstraintHasRoleSequence;
			ExclusionConstraint exclusionConstraint;
			LinkedElementCollection<Role> roles;
			if (null != (exclusionConstraint = link.ExternalConstraint as ExclusionConstraint) &&
				(roles = link.RoleSequence.RoleCollection).Count != 0 &&
				roles[0] is SupertypeMetaRole)
			{
				DelayValidateSubtypeExclusion(exclusionConstraint);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			if (sequence is SetComparisonConstraintRoleSequence)
			{
				IConstraint setComparisonConstraint = sequence.Constraint;
				if (setComparisonConstraint != null &&
					setComparisonConstraint.ConstraintType == ConstraintType.Exclusion &&
					setComparisonConstraint.Modality == ConstraintModality.Alethic)
				{
					SupertypeMetaRole superTypeRole = link.Role as SupertypeMetaRole;
					if (superTypeRole != null)
					{
						DelayValidateSubtypeExclusion((ExclusionConstraint)setComparisonConstraint);
					}
				}
				return;
			}
			IConstraint constraint = ((IConstraint)sequence);
			ConstraintType constraintType = constraint.ConstraintType;
			switch (constraintType)
			{
				case ConstraintType.SimpleMandatory:
				case ConstraintType.DisjunctiveMandatory:
					if (constraint.Modality != ConstraintModality.Alethic)
					{
						break;
					}
					ObjectType objectType = link.Role.RolePlayer;
					if (objectType != null)
					{
						// We may need to turn off IsIndependent if it is set
						FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
					}
					LinkedElementCollection<Role> roles = sequence.RoleCollection;
					int roleCount = roles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						Role role = roles[i];
						UniquenessConstraint pid;
						if (null != (objectType = role.RolePlayer) &&
							null != (pid = objectType.PreferredIdentifier) &&
							!pid.IsInternal &&
							pid.FactTypeCollection.Contains(role.FactType))
						{
							FrameworkDomainModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
						}
					}
					break;
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleDeletingRule(ElementDeletingEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			if (sequence is SetComparisonConstraintRoleSequence)
			{
				if (!link.Role.IsDeleting)
				{
					IConstraint setComparisonConstraint = sequence.Constraint;
					if (setComparisonConstraint != null &&
						setComparisonConstraint.ConstraintType == ConstraintType.Exclusion &&
						setComparisonConstraint.Modality == ConstraintModality.Alethic)
					{
						SupertypeMetaRole superTypeRole = link.Role as SupertypeMetaRole;
						if (superTypeRole != null)
						{
							DelayValidateSubtypeExclusion((ExclusionConstraint)setComparisonConstraint);
						}
					}
				}
				return;
			}
			IConstraint constraint = (IConstraint)sequence;
			ConstraintType constraintType = constraint.ConstraintType;
			ObjectType objectType;
			switch (constraintType)
			{
				case ConstraintType.SimpleMandatory:
				case ConstraintType.DisjunctiveMandatory:
					if (constraint.Modality != ConstraintModality.Alethic)
					{
						break;
					}
					LinkedElementCollection<Role> roles = sequence.RoleCollection;
					int roleCount = roles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						Role role = roles[i];
						UniquenessConstraint pid;
						if (null != (objectType = role.RolePlayer) && !objectType.IsDeleting)
						{
							FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
							if (null != (pid = objectType.PreferredIdentifier) &&
								!pid.IsDeleting &&
								!pid.IsInternal &&
								pid.FactTypeCollection.Contains(role.FactType))
							{
								FrameworkDomainModel.DelayValidateElement(objectType, DelayValidatePreferredIdentifierRequiresMandatoryError);
							}
						}
					}
					break;
				case ConstraintType.InternalUniqueness:
					if (constraint.Modality != ConstraintModality.Alethic)
					{
						break;
					}
					objectType = constraint.PreferredIdentifierFor;
					if (objectType != null)
					{
						// We may need to turn off IsIndependent if it is set
						FrameworkDomainModel.DelayValidateElement(objectType, DelayValidateIsIndependent);
					}
					break;
			}
		}
		#endregion // EntityTypeRequiresReferenceSchemeError Rules
		#region ObjectTypeRequiresPrimarySupertypeError Rules
		/// <summary>
		/// If a subtypefact is set as primary then clear the primary
		/// subtype from other facts.
		/// </summary>
		partial class SubtypeFactChangeRuleClass
		{
			private bool myIgnoreRule;
			/// <summary>
			/// ChangeRule: typeof(SubtypeFact)
			/// </summary>
			private void SubtypeFactChangeRule(ElementPropertyChangedEventArgs e)
			{
				if (myIgnoreRule)
				{
					return;
				}
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == SubtypeFact.IsPrimaryDomainPropertyId)
				{
					bool newValue = (bool)e.NewValue;
					if (!newValue)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactPrimaryMustBeTrue);
					}
					try
					{
						myIgnoreRule = true;
						SubtypeFact changedFact = e.ModelElement as SubtypeFact;
						ObjectType subtype = changedFact.Subtype;
						Role oldSupertypeRole = null;
						foreach (Role role in subtype.PlayedRoleCollection)
						{
							SubtypeMetaRole testSubtypeRole = role as SubtypeMetaRole;
							if (testSubtypeRole != null)
							{
								SubtypeFact subtypeFact = role.FactType as SubtypeFact;
								if (subtypeFact != changedFact)
								{
									subtypeFact.IsPrimary = false;
									oldSupertypeRole = subtypeFact.SupertypeRole;
									break;
								}
							}
						}

						// Now walk value roles and figure out if they need to be deleted.
						// UNDONE: VALUEROLE Note that this code makes two assumptions that may not be
						// value long term:
						// 1) Subtype and supertype meta roles are not value roles
						// 2) We only consider value constraints on the primary subtype
						//    path back to a value type, not the secondary subtyping paths.
						// This code is here instead of in ValueConstraint because of the difficulty in
						// establishing the oldSupertypeRole after this rule has been completed,
						// and the duplication of work necessary to find it before this rule runs.
						UniquenessConstraint oldIdentifier;
						LinkedElementCollection<Role> identifierRoles;
						ObjectType oldSupertype;
						if (null != oldSupertypeRole &&
							null != (oldSupertype = oldSupertypeRole.RolePlayer) &&
							null != (oldIdentifier = oldSupertype.ResolvedPreferredIdentifier) &&
							1 == (identifierRoles = oldIdentifier.RoleCollection).Count &&
							identifierRoles[0].IsValueRole)
						{
							// The old primary identification allowed value roles. If we still do, then
							// revalidate them, otherwise, delete them.
							bool visited = false;
							Role.WalkDescendedValueRoles(changedFact.Supertype, null, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
							{
								// If we get any callback here, then the role can still be a value role
								visited = true;
								if (currentValueConstraint != null && !currentValueConstraint.IsDeleting)
								{
									// Make sure that this value constraint is compatible with
									// other constraints above it.
									ObjectModel.ValueConstraint.DelayValidateValueConstraint(currentValueConstraint);
								}
								return true;
							});
							if (!visited)
							{
								// The old role player supported values, the new one does not.
								// Delete any downstream value constraints. Skip from the entity
								// type attached to the preferred identifier directly to the old
								// supertype role.
								Role.WalkDescendedValueRoles(oldIdentifier.PreferredIdentifierFor, oldSupertypeRole, delegate(Role role, ValueTypeHasDataType dataTypeLink, RoleValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint)
								{
									if (currentValueConstraint != null && !currentValueConstraint.IsDeleting)
									{
										currentValueConstraint.Delete();
									}
									return true;
								});
							}
						}
						FrameworkDomainModel.DelayValidateElement(subtype, DelayValidateObjectTypeRequiresPrimarySupertypeError);
					}
					finally
					{
						myIgnoreRule = false;
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
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
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

				//UNDONE: Eventually this shouldn't be verbalizing
				foreach (IModelErrorOwner objectTypeInstance in this.ObjectTypeInstanceCollection)
				{
					foreach (ModelErrorUsage objectTypeInstanceError in objectTypeInstance.GetErrorCollection(filter))
					{
						yield return objectTypeInstanceError;
					}
				}
			}

			if (filter == ModelErrorUses.DisplayPrimary || filter == ModelErrorUses.Verbalize)
			{
				// If we're objectified, list primary errors from the objectifying type
				// here as well. Note that we should verbalize anything we list in our
				// validation errors
				ObjectType valueType = GetValueTypeForPreferredConstraint();
				if (valueType != null)
				{
					// Always ask for 'DisplayPrimary', even if we're verbalizing
					// None of these should list as blocking verbalization here, even if they're blocking on the nesting
					foreach (ModelError valueError in (valueType as IModelErrorOwner).GetErrorCollection(ModelErrorUses.DisplayPrimary))
					{
						yield return new ModelErrorUsage(valueError, ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary);
					}
				}
			}

			if (filter == (ModelErrorUses)(-1))
			{
				// These display and verbalize with the constraints, but they use the name from this object
				IModelErrorOwner valueErrors = ValueConstraint as IModelErrorOwner;
				if (valueErrors != null)
				{
					foreach (ModelErrorUsage valueError in valueErrors.GetErrorCollection(filter))
					{
						yield return valueError;
					}
				}

				if (!this.IsValueType)
				{
					LinkedElementCollection<EntityTypeInstance> entityTypeInstances = this.EntityTypeInstanceCollection;
					int entityTypeInstanceCount = entityTypeInstances.Count;
					for (int i = 0; i < entityTypeInstanceCount; ++i)
					{
						foreach (ModelErrorUsage usage in (entityTypeInstances[i] as IModelErrorOwner).GetErrorCollection(filter))
						{
							yield return usage;
						}
					}
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
			ValidateIsIndependent(notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateDataTypeNotSpecifiedError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateEntityTypeRequiresReferenceSchemeError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateObjectTypeRequiresPrimarySupertypeError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidatePreferredIdentifierRequiresMandatoryError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleSupertypesError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateIsIndependent);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles1;
		private static Guid[] myIndirectModelErrorOwnerLinkRoles2;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			if (Objectification != null)
			{
				// Creating a static readonly guid array is causing static field initialization
				// ordering issues with the partial classes. Defer initialization.
				Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles1;
				if (linkRoles == null)
				{
					myIndirectModelErrorOwnerLinkRoles1 = linkRoles = new Guid[] { Objectification.NestingTypeDomainRoleId };
				}
				return linkRoles;
			}
			else if (IsValueType)
			{
				// This may be used as a reference mode on the other side.
				// Display data type errors on the other end.
				Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles2;
				if (linkRoles == null)
				{
					myIndirectModelErrorOwnerLinkRoles2 = linkRoles = new Guid[] { ObjectTypePlaysRole.RolePlayerDomainRoleId };
				}
				return linkRoles;
			}
			return null;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region CheckForIncompatibleRelationshipAddRule
		/// <summary>
		/// AddRule: typeof(Objectification)
		/// AddRule: typeof(ValueTypeHasDataType)
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// AddRule: typeof(FactTypeHasRole)
		/// Ensure consistency among relationships attached to ObjectType roles.
		/// This is an object model backup for the UI, which does not offer these
		/// conditions to the user.
		/// Called when an attempt is made to turn an ObjectType into either
		/// a value type or an objectifying type.
		/// </summary>
		private static void CheckForIncompatibleRelationshipAddRule(ElementAddedEventArgs e)
		{
			ProcessCheckForIncompatibleRelationship(e.ModelElement);
		}
		/// <summary>
		/// Helper function called by CheckForIncompatibleRelationshipAddRule and the corresponding CheckForIncompatibleRelationshipRolePlayerChangeRule
		/// </summary>
		public static void ProcessCheckForIncompatibleRelationship(ModelElement element)
		{
			Objectification nester;
			ValueTypeHasDataType valType;
			ObjectTypePlaysRole roleLink;
			FactTypeHasRole newRole;
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
					if (!(incompatibleValueTypeCombination = nestingType.IsValueType))
					{
						foreach (RoleBase role in nester.NestedFactType.RoleCollection)
						{
							if (role.Role.RolePlayer == nestingType)
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
				if (!(incompatibleValueTypeCombination = valType.ValueType.NestedFactType != null))
				{
					incompatiblePreferredIdentifierCombination = null != valType.ValueType.PreferredIdentifier;
				}
			}
			else if (null != (roleLink = element as ObjectTypePlaysRole))
			{
				FactType fact = roleLink.PlayedRole.FactType;
				if (fact != null)
				{
					incompatibleNestingAndRoleCombination = fact.NestingType == roleLink.RolePlayer;
				}
			}
			else if (null != (newRole = element as FactTypeHasRole))
			{
				ObjectType player = newRole.Role.Role.RolePlayer;
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
				exceptionString = ResourceStrings.ModelExceptionEnforcePreferredIdentifierForEntityType;
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
		#endregion // CheckForIncompatibleRelationshipAddRule
		#region CheckForIncompatibleRelationshipRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(Objectification)
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// RolePlayerChangeRule: typeof(FactTypeHasRole)
		/// Ensure consistency among relationships attached to ObjectType roles.
		/// This is an object model backup for the UI, which does not offer these
		/// conditions to the user.
		/// </summary>
		private static void CheckForIncompatibleRelationshipRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ProcessCheckForIncompatibleRelationship(e.ElementLink);
		}
		#endregion // CheckForIncompatibleRelationshipRolePlayerChangeRule
		#region IHierarchyContextEnabled Members
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.HierarchyContextDecrementCount"/>
		/// </summary>
		protected static int HierarchyContextDecrementCount
		{
			get
			{
				return 1;
			}
		}
		/// <summary>
		/// Gets a value indicating whether the path through the diagram should be followed through
		/// this element.
		/// </summary>
		/// <value><c>true</c> to continue walking; otherwise, <c>false</c>.</value>
		int IHierarchyContextEnabled.HierarchyContextDecrementCount
		{
			get { return HierarchyContextDecrementCount; }
		}
		/// <summary>
		/// Gets the number of generations to decriment when this object is walked.
		/// </summary>
		/// <value>The number of generations.</value>
		protected static bool ContinueWalkingHierarchyContext
		{
			get { return true; }
		}
		bool IHierarchyContextEnabled.ContinueWalkingHierarchyContext
		{
			get { return ContinueWalkingHierarchyContext; }
		}
		/// <summary>
		/// Gets the contextable object that this instance should resolve to.
		/// </summary>
		/// <value>The forward context. Null if none</value>
		/// <remarks>For example a role should resolve to a fact type since a role is displayed with a fact type</remarks>
		protected IHierarchyContextEnabled ForwardHierarchyContextTo
		{
			get { return this.NestedFactType; }
		}
		IHierarchyContextEnabled IHierarchyContextEnabled.ForwardHierarchyContextTo
		{
			get { return ForwardHierarchyContextTo; }
		}
		/// <summary>
		/// Gets the elements that the current instance is dependant on for display.
		/// The returned elements will be forced to display in the context window.
		/// </summary>
		/// <value>The dependant context elements.</value>
		protected static IEnumerable<IHierarchyContextEnabled> ForcedHierarchyContextElementCollection
		{
			get { return null; }
		}
		IEnumerable<IHierarchyContextEnabled> IHierarchyContextEnabled.ForcedHierarchyContextElementCollection
		{
			get { return ForcedHierarchyContextElementCollection; }
		}
		/// <summary>
		/// Gets the place priority. The place priority specifies the order in which the element will
		/// be placed on the context diagram.
		/// </summary>
		/// <value>The place priority.</value>
		protected static HierarchyContextPlacementPriority HierarchyContextPlacementPriority
		{
			get { return HierarchyContextPlacementPriority.VeryHigh; }
		}
		HierarchyContextPlacementPriority IHierarchyContextEnabled.HierarchyContextPlacementPriority
		{
			get { return HierarchyContextPlacementPriority; }
		}
		#endregion
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations. Responsible
		/// for instance verbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			ObjectTypeInstance[] instances = (IsValueType ? (ObjectTypeInstance[])ValueTypeInstanceCollection.ToArray() : (ObjectTypeInstance[])EntityTypeInstanceCollection.ToArray());
			int instanceCount = instances.Length;
			if (instanceCount > 0)
			{
				if (filter == null || !filter.FilterChildVerbalizer(instances[0], isNegative).IsBlocked)
				{
					ObjectTypeInstanceVerbalizer verbalizer = ObjectTypeInstanceVerbalizer.GetVerbalizer();
					verbalizer.Initialize(this, instances);
					yield return new CustomChildVerbalizer(verbalizer, true);
				}
			}
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}

		#endregion
		#region ObjectTypeInstanceVerbalizer class
		private partial class ObjectTypeInstanceVerbalizer
		{
			private ObjectType myParentObject;
			private ObjectTypeInstance[] myInstances;
			public void Initialize(ObjectType parentObject, ObjectTypeInstance[] instances)
			{
				myParentObject = parentObject;
				myInstances = instances;
			}
			private void DisposeHelper()
			{
				myInstances = null;
			}
			private ObjectType ParentObject
			{
				get { return myParentObject; }
			}
			private ObjectTypeInstance[] Instances
			{
				get { return myInstances; }
			}
		}
		#endregion // ObjectTypeInstanceVerbalizer class
	}
	#region ValueTypeHasDataType class
	public partial class ValueTypeHasDataType : IElementLinkRoleHasIndirectModelErrorOwner
	{
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ValueTypeHasDataType.ValueTypeDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
	}
	#endregion // ValueTypeHasDataType class
	#region EntityTypeRequiresReferenceSchemeError class
	[ModelErrorDisplayFilter(typeof(ReferenceSchemeErrorCategory))]
	partial class EntityTypeRequiresReferenceSchemeError
	{
		#region Base Overrides
		/// <summary>
		/// Creates error text for when an EntityType lacks a reference scheme.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorEntityTypeRequiresReferenceSchemeMessage, ObjectType.Name, Model.Name);
			if (ErrorText != newText)
			{
				ErrorText = newText;
			}
		}

		/// <summary>
		/// Sets regenerate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion // Base Overrides
	}
	#endregion // EntityTypeRequiresReferenceSchemeError class
	#region ObjectTypeRequiresPrimarySupertypeError class
	[ModelErrorDisplayFilter(typeof(ReferenceSchemeErrorCategory))]
	public partial class ObjectTypeRequiresPrimarySupertypeError
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypeRequiresPrimarySupertypeError, ObjectType.Name, Model.Name);
			if (ErrorText != newText)
			{
				ErrorText = newText;
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
	}
	#endregion // ObjectTypeRequiresPrimarySupertypeError class
	#region PreferredIdentifierRequiresMandatoryError class
	[ModelErrorDisplayFilter(typeof(ReferenceSchemeErrorCategory))]
	public partial class PreferredIdentifierRequiresMandatoryError
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			ErrorText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypePreferredIdentifierRequiresMandatoryError, ObjectType.Name, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the object name changes or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get { return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange; }
		}
		#endregion //Base Overrides
	}
	#endregion // PreferredIdentifierRequiresMandatoryError class
	#region CompatibleSupertypesError class
	[ModelErrorDisplayFilter(typeof(ReferenceSchemeErrorCategory))]
	public partial class CompatibleSupertypesError
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			ErrorText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorObjectTypeCompatibleSupertypesError, ObjectType.Name, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the object name changes or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get { return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange; }
		}
		#endregion //Base Overrides
	}
	#endregion // CompatibleSupertypesError class
}
