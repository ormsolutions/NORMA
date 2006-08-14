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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling;

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
		/// <summary>
		/// Finds the FactTypeRoleInstance for the given Role.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="selectedRole">Role to match on</param>
		/// <returns>FactTypeRoleInstance for the given role, or null if none found.</returns>
		public FactTypeRoleInstance FindRoleInstance(Role selectedRole)
		{
			LinkedElementCollection<FactTypeRoleInstance> roleInstances = RoleInstanceCollection;
			int roleInstanceCount = roleInstances.Count;
			FactTypeRoleInstance roleInstance;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				if ((roleInstance = roleInstances[i]).Role == selectedRole)
				{
					return roleInstance;
				}
			}
			return null;
		}
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
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
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
		private void ValidateTooFewFactTypeRoleInstances()
		{
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewFactTypeRoleInstancesError
		/// should be attached to the FactTypeInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewFactTypeRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasError = false;
				LinkedElementCollection<FactTypeRoleInstance> roleInstances = RoleInstanceCollection;
				FactType parent = FactType;
				LinkedElementCollection<RoleBase> factRoles;
				if (parent != null && roleInstances != null && (factRoles = parent.RoleCollection) != null)
				{
					bool roleMatch;
					int roleCollectionCount = factRoles.Count;
					int roleInstancesCount = roleInstances.Count;
					if (roleCollectionCount != roleInstancesCount)
					{
						hasError = true;
					}
					else
					{
						for (int i = 0; !hasError && i < roleCollectionCount; ++i)
						{
							roleMatch = false;
							for (int j = 0; !hasError && j < roleInstancesCount; ++j)
							{
								if (factRoles[i].Role == roleInstances[j].Role)
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
				}
				TooFewFactTypeRoleInstancesError tooFew = this.TooFewFactTypeRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = new TooFewFactTypeRoleInstancesError(this.Store);
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
					tooFew.Delete();
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
				else if (existingFactType != candidateFactType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceInconsistentRoleOwners);
				}
			}
			else if (existingFactType != null)
			{
				role.FactType = existingFactType;
			}
		}

		private void EnsureNonDuplicateRoleInstance(FactTypeInstanceHasRoleInstance link)
		{
			Role role = link.RoleInstance.Role;
			if (role != null)
			{

				ReadOnlyCollection<FactTypeInstanceHasRoleInstance> currentLinks = FactTypeInstanceHasRoleInstance.GetLinksToRoleInstanceCollection(this);
				int linkCount = currentLinks.Count;
				for (int i = linkCount - 1; i >= 0; --i)
				{
					FactTypeInstanceHasRoleInstance currentLink = currentLinks[i];
					if (link != currentLink && role == currentLink.RoleInstance.Role)
					{
						currentLink.Delete();
						break;
					}
				}
			}
		}
		#endregion
		#region FactTypeInstance Rules
		/// <summary>
		/// If a Role is added to a FactType's role collection, all FactTypeInstances of that FactType
		/// should be revalidated to ensure that they form a complete instance of the FactType
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // AddRule
		private sealed class FactTypeHasRoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType parent = link.FactType;
				foreach (FactTypeInstance factTypeInstance in parent.FactTypeInstanceCollection)
				{
					ORMCoreDomainModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
				}
			}
		}

		/// <summary>
		/// If a Role is removed from a FactType's role collection, it will
		/// automatically propagate and destroy any role instances.  This rule
		/// will force deletion of any FactTypeInstances which no longer have
		/// any FactTypeRoleInstances.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // DeleteRule
		private sealed class FactTypeHasRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				if (!factType.IsDeleted)
				{
					LinkedElementCollection<FactTypeInstance> factTypeInstances = factType.FactTypeInstanceCollection;
					int factTypeInstanceCount = factTypeInstances.Count;
					for (int i = 0; i < factTypeInstanceCount; ++i)
					{
						FactTypeInstance factTypeInstance = factTypeInstances[i];
						if (!factTypeInstance.IsDeleted)
						{
							ORMCoreDomainModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
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
		[RuleOn(typeof(FactTypeHasFactTypeInstance))] // AddRule
		private sealed class FactTypeHasFactTypeInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasFactTypeInstance link = e.ModelElement as FactTypeHasFactTypeInstance;
				FactType existingFactType = link.FactType;

				FactTypeInstance newInstance = link.FactTypeInstance;
				LinkedElementCollection<FactTypeRoleInstance> roleInstances = newInstance.RoleInstanceCollection;
				int roleInstanceCount = roleInstances.Count;
				for (int i = 0; i < roleInstanceCount; ++i)
				{
					// Check each role being related to the FactType
					newInstance.EnsureConsistentRoleOwner(existingFactType, roleInstances[i].Role);
				}
				ORMCoreDomainModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}

		/// <summary>
		/// If a RoleInstance with existing roles is added
		/// to a FactTypeInstance, make sure all of the
		/// roles have the same FactType as a parent and that a RoleInstance
		/// for the given role doesn't already exist
		/// </summary>
		[RuleOn(typeof(FactTypeInstanceHasRoleInstance))] // AddRule
		private sealed class FactTypeInstanceHasRoleInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				FactTypeInstance newInstance = link.FactTypeInstance;
				FactType existingFactType = newInstance.FactType;

				FactTypeRoleInstance roleInstance = link.RoleInstance;
				Role role = roleInstance.Role;
				newInstance.EnsureConsistentRoleOwner(existingFactType, role);
				newInstance.EnsureNonDuplicateRoleInstance(link);
				ORMCoreDomainModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}

		/// <summary>
		/// If a FactTypeRoleInstance is removed, revalidate the FactTypeInstance
		/// to ensure complete population of its roles.  If the FactTypeRoleInstance
		/// removed was the last one, remove the FactTypeInstance.
		/// </summary>
		[RuleOn(typeof(FactTypeInstanceHasRoleInstance), FireTime=TimeToFire.LocalCommit)] // DeleteRule
		private sealed class FactTypeInstanceHasRoleInstanceDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
				FactTypeInstance instance = link.FactTypeInstance;
				if (!instance.IsDeleted)
				{
					if (instance.RoleInstanceCollection.Count == 0)
					{
						instance.Delete();
					}
					else
					{
						ORMCoreDomainModel.DelayValidateElement(instance, DelayValidateTooFewFactTypeRoleInstancesError);
					}
				}
			}
		}
		#endregion
	}
	public partial class EntityTypeInstance : IModelErrorOwner
	{
		/// <summary>
		/// Finds the EntityTypeRoleInstance for the given Role.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="selectedRole">Role to match on</param>
		/// <returns>EntityTypeRoleInstance for the given role, or null if none found.</returns>
		public EntityTypeRoleInstance FindRoleInstance(Role selectedRole)
		{
			LinkedElementCollection<EntityTypeRoleInstance> roleInstances = RoleInstanceCollection;
			int roleInstanceCount = roleInstances.Count;
			EntityTypeRoleInstance roleInstance;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				if ((roleInstance = roleInstances[i]).Role == selectedRole)
				{
					return roleInstance;
				}
			}
			return null;
		}

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
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
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
		private void ValidateTooFewEntityTypeRoleInstances()
		{
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewEntityTypeRoleInstancesError
		/// should be attached to the EntityTypeInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewEntityTypeRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasError = false;
				LinkedElementCollection<EntityTypeRoleInstance> roleInstances = RoleInstanceCollection;
				ObjectType parent = EntityType;
				UniquenessConstraint preferredIdent;
				if (parent != null && roleInstances != null && (preferredIdent = parent.PreferredIdentifier) != null)
				{
					LinkedElementCollection<Role> entityPreferredIdentRoles = preferredIdent.RoleCollection;
					bool roleMatch;
					int identifierRoleCount = entityPreferredIdentRoles.Count;
					int roleInstancesCount = roleInstances.Count;
					if (identifierRoleCount != roleInstancesCount)
					{
						hasError = true;
					}
					else
					{
						for (int i = 0; !hasError && i < identifierRoleCount; ++i)
						{
							roleMatch = false;
							for (int j = 0; !hasError && j < roleInstancesCount; ++j)
							{
								if (entityPreferredIdentRoles[i] == roleInstances[j].Role)
								{
									roleMatch = true;
									break;
								}
							}
							if (!roleMatch)
							{
								hasError = true;
								break;
							}
						}
					}
				}
				TooFewEntityTypeRoleInstancesError tooFew = this.TooFewEntityTypeRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = new TooFewEntityTypeRoleInstancesError(this.Store);
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
					tooFew.Delete();
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
				LinkedElementCollection<Role> identifierRoles = identifierRoleSequence.RoleCollection;
				int identifierRolesCount = identifierRoles.Count;
				for (int i = 0; i < identifierRolesCount; ++i)
				{
					// If role is in the identifier collection, all done
					if (role == identifierRoles[i])
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

		private void EnsureNonDuplicateRoleInstance(EntityTypeInstanceHasRoleInstance link)
		{
			Role role = link.RoleInstance.Role;
			if (role != null)
			{
				ReadOnlyCollection<EntityTypeInstanceHasRoleInstance> currentLinks = EntityTypeInstanceHasRoleInstance.GetLinksToRoleInstanceCollection(this);
				int linkCount = currentLinks.Count;
				for (int i = linkCount - 1; i >= 0; --i)
				{
					EntityTypeInstanceHasRoleInstance currentLink = currentLinks[i];
					if (link != currentLink && role == currentLink.RoleInstance.Role)
					{
						currentLink.Delete();
						break;
					}
				}
			}
		}
		#endregion
		#region EntityTypeInstance Rules
		/// <summary>
		/// Clean up ValueTypeInstances when an ObjectType becomes an EntityType
		/// </summary>
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))] // AddRule
		private sealed class EntityTypeHasPreferredIdentifierAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType entityType = link.PreferredIdentifierFor;
				if (!entityType.IsValueType)
				{
					entityType.ValueTypeInstanceCollection.Clear();
				}
			}
		}

		/// <summary>
		/// Clean up EntityTypeInstances when an ObjectType becomes a ValueTypeInstance
		/// </summary>
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))] // DeleteRule
		private sealed class EntityTypeHasPreferredIdentifierDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType valueType = link.PreferredIdentifierFor;
				if (valueType.IsValueType)
				{
					valueType.EntityTypeInstanceCollection.Clear();
				}
			}
		}

		/// <summary>
		/// If a Role is added to an EntityType's preferred identifier collection, all EntityTypeInstances of that EntityType
		/// should be revalidated to ensure that they form a complete instance of the EntityType
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint = sequence as UniquenessConstraint;
				ObjectType parent;
				if (uniConstraint != null && (parent = uniConstraint.PreferredIdentifierFor) != null)
				{
					foreach (EntityTypeInstance entityTypeInstance in parent.EntityTypeInstanceCollection)
					{
						if (!entityTypeInstance.IsDeleted)
						{
							ORMCoreDomainModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
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
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeleteRule
		private sealed class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint = sequence as UniquenessConstraint;
				ObjectType parent;
				if (uniConstraint != null && (parent = uniConstraint.PreferredIdentifierFor) != null)
				{
					Role removedRole = link.Role;
					LinkedElementCollection<EntityTypeRoleInstance> roleInstances;
					LinkedElementCollection<EntityTypeInstance> instances = parent.EntityTypeInstanceCollection;
					EntityTypeInstance currentInstance;
					bool cleanUp;
					for(int i = 0; i < instances.Count;)
					{
						currentInstance = instances[i];
						if (!currentInstance.IsDeleted)
						{
							cleanUp = true;
							roleInstances = currentInstance.RoleInstanceCollection;
							EntityTypeRoleInstance currentRoleInstance;
							for (int j = 0; j < roleInstances.Count;)
							{
								currentRoleInstance = roleInstances[j];
								if (!currentRoleInstance.IsDeleted)
								{
									if (currentRoleInstance.Role == removedRole)
									{
										currentRoleInstance.Delete();
										j = 0;
									}
									else
									{
										cleanUp = false;
										++j;
									}
								}
							}
							if (cleanUp)
							{
								currentInstance.Delete();
								i = 0;
							}
							else
							{
								ORMCoreDomainModel.DelayValidateElement(currentInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
								++i;
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
		[RuleOn(typeof(EntityTypeHasEntityTypeInstance))] // AddRule
		private sealed class EntityTypeHasEntityTypeInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasEntityTypeInstance link = e.ModelElement as EntityTypeHasEntityTypeInstance;
				ObjectType entity = link.EntityType;
				if (entity.IsValueType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceInvalidEntityTypeParent);
				}
				EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
				ReadOnlyLinkedElementCollection<Role> entityTypeRoleInstances = entityTypeInstance.RoleCollection;
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
		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))] // AddRule
		private sealed class EntityTypeInstanceHasRoleInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeRoleInstance roleInstance = link.RoleInstance;
				EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
				Role role = roleInstance.Role;
				entityTypeInstance.EnsureConsistentRoleCollections(entityTypeInstance.EntityType, role);
				entityTypeInstance.EnsureNonDuplicateRoleInstance(link);
				ORMCoreDomainModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
			}
		}

		/// <summary>
		/// Revalidate the EntityTypeInstance when it loses one of its RoleInstances,
		/// to ensure that the EntityTypeInstance is fully populated.  If the EntityTypeRoleInstance
		/// removed is the last one, remove the parent EntityTypeInstance.
		/// </summary>
		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))] // DeleteRule
		private sealed class EntityTypeInstanceHasRoleInstanceDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = link.EntityTypeInstance;
				if (!instance.IsDeleted)
				{
					if (instance.RoleInstanceCollection.Count == 0)
					{
						instance.Delete();
					}
					else
					{
						ORMCoreDomainModel.DelayValidateElement(instance, DelayValidateTooFewEntityTypeRoleInstancesError);
					}
				}
			}
		}
		#endregion
	}
	
	public partial class ValueTypeInstance : IModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Display the value for ToString
		/// </summary>
		public override string ToString()
		{
			return Value;
		}
		#endregion // Base overrides
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
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
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
		private void ValidateCompatibleValueTypeInstanceValue()
		{
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
		}

		/// <summary>
		/// Rule helper to determine whether or not the given value
		/// matches the data type of the parent Value Type
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateCompatibleValueTypeInstanceValueError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				ObjectType parent = this.ValueType;
				bool hasError = false;
				if (parent != null)
				{
					DataType dataType = parent.DataType;
					if (!dataType.CanParseAnyValue && !dataType.CanParse(this.Value))
					{
						hasError = true;
					}
				}
				CompatibleValueTypeInstanceValueError badValue = this.CompatibleValueTypeInstanceValueError;
				if (hasError)
				{
					if (badValue == null)
					{
						badValue = new CompatibleValueTypeInstanceValueError(this.Store);
						badValue.ValueTypeInstance = this;
						badValue.Model = parent.Model;
						badValue.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(badValue);
						}
					}
					else
					{
						// Refresh the error text
						badValue.GenerateErrorText();
					}
				}
				else if (badValue != null)
				{
					badValue.Delete();
				}
			}
		}
		#endregion
		#region ValueTypeInstance Rules
		/// <summary>
		/// When the DataType is added, recheck the valueTypeInstance values
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))] // AddRule
		private sealed class ValueTypeHasDataTypeAdded : AddRule
		{
			/// <summary>
			/// Process a data type change for the given value type
			/// </summary>
			/// <param name="link">The ValueTypeHasType relationship instance</param>
			public static void Process(ValueTypeHasDataType link)
			{
				DataType dataType = link.DataType;
				ObjectType valueType = link.ValueType;
				bool clearErrors = dataType.CanParseAnyValue;
				foreach (ValueTypeInstance valueTypeInstance in valueType.ValueTypeInstanceCollection)
				{
					if (clearErrors)
					{
						valueTypeInstance.CompatibleValueTypeInstanceValueError = null;
					}
					else
					{
						ORMCoreDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
					}
				}
			}
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				Process(e.ModelElement as ValueTypeHasDataType);
			}
		}
		/// <summary>
		/// When the DataType is changed, recheck the instance values
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))] // RolePlayerChangeRule
		private sealed class ValueTypeHasDataTypeRolePlayerChange : RolePlayerChangeRule
		{
			public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
				{
					ValueTypeHasDataTypeAdded.Process(e.ElementLink as ValueTypeHasDataType);
				}
			}
		}

		/// <summary>
		/// Whenever the value of a valueTypeInstance changes, make sure it can be parsed as the current DataType
		/// </summary>
		[RuleOn(typeof(ValueTypeInstance))] // ChangeRule
		private sealed class ValueTypeInstanceValueChanged : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				ValueTypeInstance valueTypeInstance = e.ModelElement as ValueTypeInstance;
				if (!valueTypeInstance.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
				}
			}
		}
		
		/// <summary>
		/// Confirms that only ObjectTypes that are actually ValueTypes
		/// can have a ValueTypeInstanceCollection, and confirms that the
		/// given ValueTypeInstance.Value is of the datatype defined in
		/// ValueType.DataType
		/// </summary>
		[RuleOn(typeof(ValueTypeHasValueTypeInstance))] // AddRule
		private sealed class ValueTypeHasValueTypeInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
				ObjectType valueType = link.ValueType;
				if (!valueType.IsValueType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionValueTypeInstanceInvalidValueTypeParent);
				}
				ValueTypeInstance valueTypeInstance = link.ValueTypeInstance;
				ORMCoreDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
			}
		}
		#endregion

	}

	public partial class ObjectTypeInstance
	{
		/// <summary>
		/// Display the value for ToString
		/// </summary>
		public override string ToString()
		{
			EntityTypeInstance entityTypeInstance;
			ValueTypeInstance valueTypeInstance;
			ObjectType parent = null;
			if (null != (entityTypeInstance = this as EntityTypeInstance))
			{
				parent = entityTypeInstance.EntityType;
			}
			else if (null != (valueTypeInstance = this as ValueTypeInstance))
			{
				parent = valueTypeInstance.ValueType;
			}
			return ObjectTypeInstance.GetDisplayString(this, parent);
		}

		/// <summary>
		/// Returns the display string for the given instance
		/// </summary>
		/// <param name="objectTypeInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentType">Parent Type of the instance.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(ObjectTypeInstance objectTypeInstance, ObjectType parentType)
		{
			return GetDisplayString(objectTypeInstance, parentType, null, null, null);
		}

		/// <summary>
		/// Returns the display string for the given instance
		/// </summary>
		/// <param name="objectTypeInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentType">Parent Type of the instance.</param>
		/// <param name="formatProvider">Format provider for desired culture.</param>
		/// <param name="valueNonTextFormat">Format string for non text value type instances.</param>
		/// <param name="valueTextFormat">Format string for text value type instances.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(ObjectTypeInstance objectTypeInstance, ObjectType parentType, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
		{
			StringBuilder outputText = null;
			if (valueTextFormat == "{0}")
			{
				valueTextFormat = null;
			}
			if (valueNonTextFormat == "{0}")
			{
				valueNonTextFormat = null;
			}
			string retVal = (parentType == null) ? "" : RecurseObjectTypeInstanceValue(objectTypeInstance, parentType, null, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat);
			return (outputText != null) ? outputText.ToString() : retVal;
		}

		private static string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectTypeInstance, ObjectType parentType, string listSeparator, ref StringBuilder outputText, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
		{
			ValueTypeInstance valueInstance;
			EntityTypeInstance entityTypeInstance;
			if (parentType == null)
			{
				if (outputText != null)
				{
					outputText.Append(" ");
				}
				return " ";
			}
			else if (parentType.IsValueType)
			{
				valueInstance = objectTypeInstance as ValueTypeInstance;
				string valueText = " ";
				if (valueInstance != null)
				{
					if (valueTextFormat != null && parentType.DataType is TextDataType)
					{
						if (formatProvider == null)
						{
							formatProvider = CultureInfo.CurrentCulture;
						}
						valueText = String.Format(formatProvider, valueTextFormat, valueInstance.Value);
					}
					else if (valueNonTextFormat != null)
					{
						if (formatProvider == null)
						{
							formatProvider = CultureInfo.CurrentCulture;
						}
						valueText = String.Format(formatProvider, valueNonTextFormat, valueInstance.Value);
					}
					else
					{
						valueText = valueInstance.Value;
					}
				}
				if (outputText != null)
				{
					outputText.Append(valueText);
					return null;
				}
				return valueText;
			}
			else
			{
				entityTypeInstance = objectTypeInstance as EntityTypeInstance;
				UniquenessConstraint identifier = parentType.PreferredIdentifier;
				if (identifier == null)
				{
					string valueText = " ";
					if (outputText != null)
					{
						outputText.Append(valueText);
						return null;
					}
					return valueText;
				}
				LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
				int identifierCount = identifierRoles.Count;
				if (identifierCount == 1)
				{
					ObjectTypeInstance nestedInstance = null;
					if (entityTypeInstance != null)
					{
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = entityTypeInstance.RoleInstanceCollection;
						if (roleInstances.Count > 0)
						{
							nestedInstance = roleInstances[0].ObjectTypeInstance;
						}
					}
					return RecurseObjectTypeInstanceValue(nestedInstance, identifierRoles[0].RolePlayer, listSeparator, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat);
				}
				else
				{
					LinkedElementCollection<EntityTypeRoleInstance> roleInstances = null;
					int roleInstanceCount = 0;
					if (entityTypeInstance != null)
					{
						roleInstances = entityTypeInstance.RoleInstanceCollection;
						roleInstanceCount = roleInstances.Count;
					}
					if (outputText == null)
					{
						outputText = new StringBuilder();
					}
					outputText.Append("(");
					if (listSeparator == null)
					{
						if (formatProvider == null)
						{
							listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
						}
						else
						{
							listSeparator = (formatProvider as CultureInfo).TextInfo.ListSeparator;
						}
					}
					for (int i = 0; i < identifierCount; ++i)
					{
						Role identifierRole = identifierRoles[i];
						bool match = false;
						if (i != 0)
						{
							outputText.Append(listSeparator);
						}
						if (roleInstanceCount != 0)
						{
							for (int j = 0; j < roleInstanceCount; ++j)
							{
								EntityTypeRoleInstance instance = roleInstances[j];
								if (instance.Role == identifierRole)
								{
									RecurseObjectTypeInstanceValue(instance.ObjectTypeInstance, identifierRole.RolePlayer, listSeparator, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat);
									match = true;
									break;
								}
							}
						}
						else if (i == 0)
						{
							outputText.Append(" ");
						}
						if (!match)
						{
							RecurseObjectTypeInstanceValue(null, identifierRole.RolePlayer, listSeparator, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat);
						}
					}
					outputText.Append(")");
				}
				return null;
			}
		}
	}
}
