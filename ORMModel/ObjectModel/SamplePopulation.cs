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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region SamplePopulation ModelError Classes
	public partial class TooFewEntityTypeRoleInstancesError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			EntityTypeInstance entityTypeInstance = EntityTypeInstance;
			ObjectType entityType = (entityTypeInstance != null) ? entityTypeInstance.EntityType : null;
			string entityName = (entityType != null) ? entityType.Name : "";
			string modelName = Model.Name;
			string currentText = Name;
			string newText = string.Format(ResourceStrings.ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesMessage, entityName, modelName);
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	public partial class TooFewFactTypeRoleInstancesError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FactTypeInstance factTypeInstance = FactTypeInstance;
			FactType factType = (factTypeInstance != null) ? factTypeInstance.FactType : null;
			string factName = (factType != null) ? factType.Name : "";
			string modelName = Model.Name;
			string currentText = Name;
			string newText = string.Format(ResourceStrings.ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesMessage, factName, modelName);
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	public partial class CompatibleValueTypeInstanceValueError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ValueTypeInstance valueTypeInstance = ValueTypeInstance;
			ObjectType valueType = (valueTypeInstance != null) ? valueTypeInstance.ValueType : null;
			string valueName = (valueType != null) ? valueType.Name : "";
			string dataType = (valueType != null) ? valueType.DataType.PortableDataType.ToString() : "";
			string value = (valueTypeInstance != null) ? valueTypeInstance.Value : "";
			string modelName = Model.Name;
			string currentText = Name;
			string newText = string.Format(ResourceStrings.ModelErrorValueTypeInstanceCompatibleValueTypeInstanceValueMessage, value, valueName, modelName, dataType);
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	#endregion

	public partial class FactTypeInstance : IModelErrorOwner
	{
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
				TooFewFactTypeRoleInstancesError tooFew = TooFewFactTypeRoleInstancesError;
				if (tooFew != null)
				{
					yield return tooFew;
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
			ValidateTooFewFactTypeRoleInstancesError(notifyAdded);
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
			ORMMetaModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
		}

		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region TooFewFactTypeRoleInstancesError Validation
		/// <summary>
		/// Validator callback for TooFewFactTypeRoleInstancesError
		/// </summary>
		private static void DelayValidateTooFewFactTypeRoleInstancesError(ModelElement element)
		{
			(element as FactTypeInstance).ValidateTooFewFactTypeRoleInstancesError(null);
		}

		/// <summary>
		/// Called inside a transaction to force entity role instance validation
		/// </summary>
		public void ValidateTooFewFactTypeRoleInstances()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewFactTypeRoleInstancesError
		/// should be attached to the FactTypeInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewFactTypeRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = false;
				FactTypeRoleInstanceMoveableCollection roleInstances = RoleInstanceCollection;
				FactType parent = FactType;
				RoleBaseMoveableCollection factRoles;
				if (parent != null && roleInstances != null && (factRoles = parent.RoleCollection) != null)
				{
					bool roleMatch;
					int roleCollectionCount = factRoles.Count;
					int roleInstancesCount = roleInstances.Count;
					if (roleCollectionCount != roleInstancesCount)
					{
						hasError = true;
					}
					for (int i = 0; !hasError && i < roleCollectionCount; ++i)
					{
						roleMatch = false;
						for (int j = 0; !hasError && j < roleInstancesCount; ++j)
						{
							if (object.ReferenceEquals(factRoles[i].Role, roleInstances[j].RoleCollection))
							{
								roleMatch = true;
								break;
							}
						}
						if (!roleMatch)
						{
							hasError = true;
						}
					}
				}
				TooFewFactTypeRoleInstancesError tooFew = this.TooFewFactTypeRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = TooFewFactTypeRoleInstancesError.CreateTooFewFactTypeRoleInstancesError(this.Store);
						tooFew.FactTypeInstance = this;
						tooFew.Model = parent.Model;
						tooFew.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooFew);
						}
					}
				}
				else if (tooFew != null)
				{
					tooFew.Remove();
				}
			}
		}
		#endregion
		#region Inline Error Helper Methods
		/// <summary>
		/// Ensure that the role is owned by the same
		/// fact type as the fact type instance. This method should
		/// be called from inside a transaction
		/// and will throw
		/// </summary>
		private void EnsureConsistentRoleOwner(FactType existingFactType, Role role)
		{
			FactType candidateFactType = role.FactType;
			if (candidateFactType != null)
			{
				if (existingFactType == null)
				{
					FactType = candidateFactType;
				}
				else if (!object.ReferenceEquals(existingFactType, candidateFactType))
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceInconsistentRoleOwners);
				}
			}
			else if (existingFactType != null)
			{
				role.FactType = existingFactType;
			}
		}

		private void EnsureNonDuplicateRoleInstance(Role instanceRole)
		{
			FactTypeRoleInstanceMoveableCollection roleInstances = RoleInstanceCollection;
			int roleInstanceCount = roleInstances.Count;
			int roleCount = 0;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				if (object.ReferenceEquals(roleInstances[i].RoleCollection, instanceRole))
				{
					++roleCount;
				}
			}
			// Since this checks after the instance has already been added, it needs to see if there are TWO instances that use the role,
			// one being the current role we're checking the add for, one being a possible duplicate.
			// UNDONE: Better way/place to check for duplicates?
			if(roleCount >= 2)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceDuplicateRoleInstance);
			}
		}
		#endregion
		#region FactTypeInstance Rules
		/// <summary>
		/// If a Role is added to a FactType's role collection, all FactTypeInstances of that FactType
		/// should be revalidated to ensure that they form a complete instance of the FactType
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class FactTypeHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType parent = link.FactType;
				foreach (FactTypeInstance factTypeInstance in parent.FactTypeInstanceCollection)
				{
					ORMMetaModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
				}
			}
		}

		/// <summary>
		/// If a Role is removed from a FactType's role collection, it will
		/// automatically propagate and destroy any role instances.  This rule
		/// will force deletion of any FactTypeInstances which no longer have
		/// any FactTypeRoleInstances.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class FactTypeHasRoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				if (!factType.IsRemoved)
				{
					FactTypeInstanceMoveableCollection factTypeInstances = factType.FactTypeInstanceCollection;
					int factTypeInstanceCount = factTypeInstances.Count;
					for (int i = 0; i < factTypeInstanceCount; ++i)
					{
						FactTypeInstance factTypeInstance = factTypeInstances[i];
						if (!factTypeInstance.IsRemoved)
						{
							ORMMetaModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
						}
					}
				}
			}
		}

		/// <summary>
		/// If a FactTypeInstance with existing RoleInstances is added
		/// to a FactType, make sure all of the RoleInstance Roles
		/// have the same FactType as a parent
		/// </summary>
		[RuleOn(typeof(FactTypeHasFactTypeInstance))]
		private class FactTypeHasFactTypeInstanceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				FactType existingFactType = link.FactType;

				FactTypeInstance newInstance = link.FactTypeInstanceCollection;
				FactTypeRoleInstanceMoveableCollection roleInstances = newInstance.RoleInstanceCollection;
				int roleInstanceCount = roleInstances.Count;
				for (int i = 0; i < roleInstanceCount; ++i)
				{
					// Check each role being related to the FactType
					newInstance.EnsureConsistentRoleOwner(existingFactType, roleInstances[i].RoleCollection);
				}
				ORMMetaModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}

		/// <summary>
		/// If a RoleInstance with existing roles is added
		/// to a FactTypeInstance, make sure all of the
		/// roles have the same FactType as a parent and that a RoleInstance
		/// for the given role doesn't already exist
		/// </summary>
		[RuleOn(typeof(FactTypeInstanceHasRoleInstance))]
		private class FactTypeInstanceHasRoleInstanceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				FactTypeInstance newInstance = link.FactTypeInstance;
				FactType existingFactType = newInstance.FactType;

				FactTypeRoleInstance roleInstance = link.RoleInstanceCollection;
				Role role = roleInstance.RoleCollection;
				newInstance.EnsureConsistentRoleOwner(existingFactType, role);
				newInstance.EnsureNonDuplicateRoleInstance(role);
				ORMMetaModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}

		///// <summary>
		///// If a FactTypeRoleInstance is removed, revalidate the FactTypeInstance
		///// to ensure complete population of its roles.
		///// </summary>
		//[RuleOn(typeof(FactTypeInstanceHasRoleInstance))]
		//private class FactTypeInstanceHasRoleInstanceRemoved : RemoveRule
		//{
		//    public override void ElementRemoved(ElementRemovedEventArgs e)
		//    {
		//        FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
		//        FactTypeInstance instance = link.FactTypeInstance;
		//        if (!instance.IsRemoved)
		//        {
		//            ORMMetaModel.DelayValidateElement(instance, DelayValidateTooFewFactTypeRoleInstancesError);
		//        }
		//    }
		//}

		/// <summary>
		/// If a FactTypeRoleInstance is removed, revalidate the FactTypeInstance
		/// to ensure complete population of its roles.
		/// </summary>
		[RuleOn(typeof(FactTypeInstanceHasRoleInstance), FireTime=TimeToFire.LocalCommit)]
		private class FactTypeInstanceHasRoleInstanceRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				FactTypeInstance instance = link.FactTypeInstance;
				if (!instance.IsRemoved)
				{
					if (instance.RoleInstanceCollection.Count == 0)
					{
						instance.Remove();
					}
					else
					{
						ORMMetaModel.DelayValidateElement(instance, DelayValidateTooFewFactTypeRoleInstancesError);
					}
				}
			}
		}
		#endregion
	}
	public partial class EntityTypeInstance : IModelErrorOwner
	{
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
				TooFewEntityTypeRoleInstancesError tooFew = TooFewEntityTypeRoleInstancesError;
				if (tooFew != null)
				{
					yield return tooFew;
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
			ValidateTooFewEntityTypeRoleInstancesError(notifyAdded);
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
			ORMMetaModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
		}

		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region TooFewEntityTypeRoleInstancesError Validation
		/// <summary>
		/// Validator callback for TooFewEntityTypeRoleInstancesError
		/// </summary>
		private static void DelayValidateTooFewEntityTypeRoleInstancesError(ModelElement element)
		{
			(element as EntityTypeInstance).ValidateTooFewEntityTypeRoleInstancesError(null);
		}
		
		/// <summary>
		/// Called inside a transaction to force entity role instance validation
		/// </summary>
		public void ValidateTooFewEntityTypeRoleInstances()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewEntityTypeRoleInstancesError
		/// should be attached to the EntityTypeInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewEntityTypeRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = false;
				EntityTypeRoleInstanceMoveableCollection roleInstances = RoleInstanceCollection;
				ObjectType parent = EntityType;
				UniquenessConstraint preferredIdent;
				if (parent != null && roleInstances != null && (preferredIdent = parent.PreferredIdentifier) != null)
				{
					RoleMoveableCollection entityPreferredIdentRoles = preferredIdent.RoleCollection;
					bool roleMatch;
					int identifierRoleCount = entityPreferredIdentRoles.Count;
					int roleInstancesCount = roleInstances.Count;
					if (identifierRoleCount != roleInstancesCount)
					{
						hasError = true;
					}
					for (int i = 0; !hasError && i < identifierRoleCount; ++i)
					{
						roleMatch = false;
						for (int j = 0; !hasError && j < roleInstancesCount; ++j)
						{
							if (object.ReferenceEquals(entityPreferredIdentRoles[i], roleInstances[j].RoleCollection))
							{
								roleMatch = true;
								break;
							}
						}
						if (!roleMatch)
						{
							hasError = true;
						}
					}
				}
				TooFewEntityTypeRoleInstancesError tooFew = this.TooFewEntityTypeRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = TooFewEntityTypeRoleInstancesError.CreateTooFewEntityTypeRoleInstancesError(this.Store);
						tooFew.EntityTypeInstance = this;
						tooFew.Model = parent.Model;
						tooFew.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooFew);
						}
					}
				}
				else if (tooFew != null)
				{
					tooFew.Remove();
				}
			}
		}
		#endregion
		#region Inline Error Helper Methods
		private void EnsureConsistentRoleCollections(ObjectType currentEntityType, Role role)
		{
			if (currentEntityType != null && currentEntityType.PreferredIdentifier != null)
			{
				UniquenessConstraint identifierRoleSequence = currentEntityType.PreferredIdentifier;
				RoleMoveableCollection identifierRoles = identifierRoleSequence.RoleCollection;
				int identifierRolesCount = identifierRoles.Count;
				for (int i = 0; i < identifierRolesCount; ++i)
				{
					// If role is in the identifier collection, all done
					if (object.ReferenceEquals(role, identifierRoles[i]))
					{
						return;
					}
				}
				// If the role didn't match any of the identifiers, throw an error
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceInvalidRolesPreferredIdentifier);
			}
			// If the role is hooked to an entity type but the entityTypeInstance isn't,
			// hook up the entityTypeInstance to the same entityType
			else
			{
				// Make sure the role is actually hooked up to an EntityType
				if (role.RolePlayer != null && !role.RolePlayer.IsValueType)
				{
					this.EntityType = role.RolePlayer;
				}
			}
		}

		private void EnsureNonDuplicateRoleInstance(EntityTypeInstanceHasRoleInstance link, Role role)
		{
			IList links = GetElementLinks(EntityTypeInstanceHasRoleInstance.EntityTypeInstanceMetaRoleGuid);
			Debug.Assert(object.ReferenceEquals(role, link.RoleInstanceCollection.RoleCollection));
			int linkCount = links.Count;
			for (int i = 0; i < linkCount; ++i)
			{
				EntityTypeInstanceHasRoleInstance testLink = (EntityTypeInstanceHasRoleInstance)links[i];
				if (!object.ReferenceEquals(testLink, link) &&
					object.ReferenceEquals(role, testLink.RoleInstanceCollection.RoleCollection))
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceDuplicateRoleInstance);
				}
			}
		}
		#endregion
		#region EntityTypeInstance Rules
		/// <summary>
		/// If a Role is added to an EntityType's preferred identifier collection, all EntityTypeInstances of that EntityType
		/// should be revalidated to ensure that they form a complete instance of the EntityType
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection;
				UniquenessConstraint uniConstraint = sequence as UniquenessConstraint;
				ObjectType parent;
				if (uniConstraint != null && (parent = uniConstraint.PreferredIdentifierFor) != null)
				{
					foreach (EntityTypeInstance entityTypeInstance in parent.EntityTypeInstanceCollection)
					{
						if (!entityTypeInstance.IsRemoved)
						{
							ORMMetaModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
						}
					}
				}
			}
		}

		/// <summary>
		/// If a Role is removed from an EntityType's preferred identifier collection, it will
		/// automatically propogate and destroy any role instances.  This rule
		/// will force deletion of any EntityTypeInstances which no longer have
		/// any EntityTypeRoleInstances.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ConstraintRoleSequenceHasRoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection;
				UniquenessConstraint uniConstraint = sequence as UniquenessConstraint;
				ObjectType parent;
				if (uniConstraint != null && (parent = uniConstraint.PreferredIdentifierFor) != null)
				{
					Role removedRole = link.RoleCollection;
					EntityTypeRoleInstanceMoveableCollection roleInstances;
					bool cleanUp;
					foreach (EntityTypeInstance entityTypeInstance in parent.EntityTypeInstanceCollection)
					{
						if (!entityTypeInstance.IsRemoved)
						{
							cleanUp = true;
							roleInstances = entityTypeInstance.RoleInstanceCollection;
							foreach (EntityTypeRoleInstance entityTypeRoleInstance in roleInstances)
							{
								if (!entityTypeRoleInstance.IsRemoved)
								{
									if (object.ReferenceEquals(entityTypeRoleInstance.RoleCollection, removedRole))
									{
										entityTypeRoleInstance.Remove();
									}
									else
									{
										cleanUp = false;
									}
								}
							}
							if (cleanUp)
							{
								entityTypeInstance.Remove();
							}
							else
							{
								ORMMetaModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
							}
						}
					}
				}
			}
		}
		
		/// <summary>
		/// If an EntityTypeInstance with existing RoleInstances is added
		/// to an EntityType, ensure that all of the RoleInstances are hooked up to a role in the 
		/// EntityType's preferred identifier.
		/// </summary>
		[RuleOn(typeof(EntityTypeHasEntityTypeInstance))]
		private class EntityTypeHasEntityTypeInstanceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entity = link.EntityType;
				if (entity.IsValueType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceInvalidEntityTypeParent);
				}
				EntityTypeInstance entityTypeInstance = link.EntityTypeInstanceCollection;
				RoleMoveableCollection entityTypeRoleInstances = entityTypeInstance.RoleCollection;
				int roleCount = entityTypeRoleInstances.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					entityTypeInstance.EnsureConsistentRoleCollections(entity, entityTypeRoleInstances[i]);
				}
			}
		}

		/// <summary>
		/// Ensure that every RoleInstance added to an EntityTypeInstance involves a role
		/// in the EntityType parent's PreferredIdentifier, and that there are no duplicates.
		/// Also validate the EntityTypeInstance to ensure a full instance population.
		/// </summary>
		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))]
		private class EntityTypeInstanceHasRoleInstanceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeRoleInstance roleInstance = link.RoleInstanceCollection;
				EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
				Role role = roleInstance.RoleCollection;
				entityTypeInstance.EnsureConsistentRoleCollections(entityTypeInstance.EntityType, role);
				entityTypeInstance.EnsureNonDuplicateRoleInstance(link, role);
				ORMMetaModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
			}
		}

		/// <summary>
		/// Revalidate the EntityTypeInstance when it loses one of its RoleInstances,
		/// to ensure that the EntityTypeInstance is fully populated.
		/// </summary>
		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))]
		private class EntityTypeInstanceHasRoleInstanceRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeRoleInstance roleInstance = link.RoleInstanceCollection;
				EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
				ORMMetaModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
			}
		}
		#endregion
	}
	public partial class ValueTypeInstance : IModelErrorOwner
	{
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
				CompatibleValueTypeInstanceValueError badValue = CompatibleValueTypeInstanceValueError;
				if (badValue != null)
				{
					yield return badValue;
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
			ValidateCompatibleValueTypeInstanceValueError(notifyAdded);
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
			ORMMetaModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
		}

		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region CompatibleValueTypeInstanceValueError Validation
		/// <summary>
		/// Validator callback for TooFewEntityTypeRoleInstancesError
		/// </summary>
		private static void DelayValidateCompatibleValueTypeInstanceValueError(ModelElement element)
		{
			(element as ValueTypeInstance).ValidateCompatibleValueTypeInstanceValueError(null);
		}

		/// <summary>
		/// Called inside a transaction to force entity role instance validation
		/// </summary>
		public void ValidateCompatibleValueTypeInstanceValue()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
		}

		/// <summary>
		/// Rule helper to determine whether or not the given value
		/// matches the data type of the parent Value Type
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateCompatibleValueTypeInstanceValueError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				ObjectType parent = this.ValueType;
				CompatibleValueTypeInstanceValueError badValue = this.CompatibleValueTypeInstanceValueError;
				if (parent != null)
				{
					DataType dataType = parent.DataType;
					if(!dataType.CanParse(this.Value))
					{
						if (badValue == null)
						{
							badValue = CompatibleValueTypeInstanceValueError.CreateCompatibleValueTypeInstanceValueError(this.Store);
							badValue.ValueTypeInstance = this;
							badValue.Model = parent.Model;
							badValue.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(badValue);
							}
						}
					}
				}
				else if (badValue != null)
				{
					badValue.Remove();
				}
			}
		}
		#endregion
		#region ValueTypeInstance Rules
		/// <summary>
		/// When the DataType changes, recheck the valueTypeInstance values
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class ValueTypeHasDataTypeAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				DataType dataType = link.DataType;
				ObjectType valueType = link.ValueTypeCollection;
				bool clearErrors = dataType.CanParseAnyValue;
				foreach (ValueTypeInstance valueTypeInstance in valueType.ValueTypeInstanceCollection)
				{
					if (clearErrors)
					{
						valueTypeInstance.CompatibleValueTypeInstanceValueError = null;
					}
					else
					{
						ORMMetaModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
					}
				}
			}
		}

		/// <summary>
		/// Whenever the value of a valueTypeInstance changes, make sure it can be parsed as the current DataType
		/// </summary>
		[RuleOn(typeof(ValueTypeInstance))]
		private class ValueTypeInstanceValueChanged : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ValueTypeInstance valueTypeInstance = e.ModelElement as ValueTypeInstance;
				ObjectType valueType = valueTypeInstance.ValueType;
				if (valueType != null)
				{
					DataType dataType = valueType.DataType;
					if (!dataType.CanParse(e.NewValue.ToString()))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionValueTypeInstanceInvalidValue);
					}
				}
			}
		}
		
		/// <summary>
		/// Confirms that only ObjectTypes that are actually ValueTypes
		/// can have a ValueTypeInstanceCollection, and confirms that the
		/// given ValueTypeInstance.Value is of the datatype defined in
		/// ValueType.DataType
		/// </summary>
		[RuleOn(typeof(ValueTypeHasValueTypeInstance))]
		private class ValueTypeHasValueTypeInstanceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				ObjectType valueType = link.ValueType;
				if (!valueType.IsValueType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionValueTypeInstanceInvalidValueTypeParent);
				}
				ValueTypeInstance valueTypeInstance = link.ValueTypeInstanceCollection;
				ORMMetaModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
			}
		}
		#endregion
	}
}
