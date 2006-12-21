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
			string currentText = ErrorText;
			string newText = string.Format(ResourceStrings.ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesMessage, entityName, modelName);
			if (currentText != newText)
			{
				ErrorText = newText;
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
			string currentText = ErrorText;
			string newText = string.Format(ResourceStrings.ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesMessage, factName, modelName);
			if (currentText != newText)
			{
				ErrorText = newText;
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
			string currentText = ErrorText;
			string newText = string.Format(ResourceStrings.ModelErrorValueTypeInstanceCompatibleValueTypeInstanceValueMessage, value, valueName, modelName, dataType);
			if (currentText != newText)
			{
				ErrorText = newText;
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
	public partial class PopulationUniquenessError : IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Return the duplicated ObjectTypeInstance
		/// </summary>
		public ObjectTypeInstance DuplicateObjectTypeInstance
		{
			get
			{
				return this.RoleInstanceCollection[0].ObjectTypeInstance;
			}
		}

		/// <summary>
		/// Return the common role
		/// </summary>
		public Role CommonRole
		{
			get
			{
				return this.RoleInstanceCollection[0].Role;
			}
		}

		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectTypeInstance duplicateInstance = DuplicateObjectTypeInstance;
			Role commonRole = CommonRole;
			string instanceDisplayString = duplicateInstance.Name;
			string formatString, typeName;
			if(commonRole.Name.Length != 0)
			{
				formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithNamedRole;
				typeName = commonRole.Name;
			}
			else
			{
				formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithUnnamedRole;
				typeName = commonRole.FactType.Name;
			}
			string modelName = Model.Name;
			string objectTypeName = commonRole.RolePlayer.Name;
			string currentText = ErrorText;
			string newText = String.Format(formatString, objectTypeName, instanceDisplayString, modelName, typeName);
			if (currentText != newText)
			{
				ErrorText = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion // Base overrides

		#region IModelErrorOwner Implementation
		/*
		#region IModelErrorOwner Members

		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			yield return new ModelErrorUsage(this);
			foreach (ModelErrorUsage modelErrorUsage in base.GetErrorCollection(filter))
			{
				yield return modelErrorUsage;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// No validation here
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
			// No Validation Here
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		*/
		#endregion
		#region IHasIndirectModelErrorOwner Members

		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RoleInstanceHasPopulationUniquenessError.PopulationUniquenessErrorDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}

				#endregion

	}

	public partial class PopulationMandatoryError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectTypeInstance objectInstance = ObjectTypeInstance;
			//UNDONE: Should handle mandatory constraints which constraint multiple roles
			Role role = MandatoryConstraint.RoleCollection[0];
			string instanceDisplayString = objectInstance.Name;
			string modelName = Model.Name;
			string currentText = ErrorText;
			string newText = String.Format(ResourceStrings.ModelErrorModelHasPopulationMandatoryError, role.RolePlayer.Name, instanceDisplayString, modelName, role.FactType.Name);
			if (currentText != newText)
			{
				ErrorText = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Members
		private ModelElement[] GetRepresentedElements()
		{
			List<ModelElement> modelElements = new List<ModelElement>();
			LinkedElementCollection<Role> roles = this.MandatoryConstraint.RoleCollection;
			int roleCount = roles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				FactType factType = roles[i].FactType;
				int index = modelElements.BinarySearch(factType, HashCodeComparer<ModelElement>.Instance);
				if (index < 0)
				{
					modelElements.Insert(0, factType);
					modelElements.Sort(HashCodeComparer<ModelElement>.Instance);
				}
			}
			return modelElements.ToArray();
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion

	public partial class FactTypeInstance : IModelErrorOwner
	{
		#region Helper Methods
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
		#endregion
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

				LinkedElementCollection<FactTypeRoleInstance> roleInstances = this.RoleInstanceCollection;
				int roleInstanceCount = roleInstances.Count;
				for (int i = 0; i < roleInstanceCount; ++i)
				{
					PopulationUniquenessError uniquenessError = roleInstances[i].PopulationUniquenessError;
					if (uniquenessError != null)
					{
						yield return uniquenessError;
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
		private sealed partial class FactTypeHasRoleAdded : AddRule
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
		private sealed partial class FactTypeHasRoleDeleted : DeleteRule
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
		private sealed partial class FactTypeHasFactTypeInstanceAdded : AddRule
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
		private sealed partial class FactTypeInstanceHasRoleInstanceAdded : AddRule
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
		private sealed partial class FactTypeInstanceHasRoleInstanceDeleted : DeleteRule
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
		#region Helper Methods
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
		#endregion
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

		[RuleOn(typeof(EntityTypeInstance))] // DeletingRule
		private sealed partial class EntityTypeInstanceDeleting : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				EntityTypeInstance instance = e.ModelElement as EntityTypeInstance;
				ObjectType parent = instance.EntityType;
				UniquenessConstraint identifier = parent.PreferredIdentifier;
				if (identifier != null)
				{
					LinkedElementCollection<Role> roles = identifier.RoleCollection;
					if (roles.Count == 1)
					{
						ObjectType identifierPlayer = roles[0].RolePlayer;
						int allowedRoleCount = 1;
						if (!identifierPlayer.IsValueType)
						{
							UniquenessConstraint playerIdentifier = identifierPlayer.PreferredIdentifier;
							if (playerIdentifier != null)
							{
								if (playerIdentifier.RoleCollection.Count == 1)
								{
									allowedRoleCount = 2;
								}
							}
						}
						if (identifierPlayer.PlayedRoleCollection.Count == allowedRoleCount)
						{
							LinkedElementCollection<EntityTypeRoleInstance> roleInstances = instance.RoleInstanceCollection;
							int roleInstanceCount = roleInstances.Count;
							for (int i = 0; i < roleInstanceCount; ++i)
							{
								EntityTypeRoleInstance roleInstance = roleInstances[0];
								roleInstance.ObjectTypeInstance.Delete();
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Clean up ValueTypeInstances when an ObjectType becomes an EntityType
		/// </summary>
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))] // AddRule
		private sealed partial class EntityTypeHasPreferredIdentifierAdded : AddRule
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
		private sealed partial class EntityTypeHasPreferredIdentifierDeleted : DeleteRule
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
		private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
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
		private sealed partial class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
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
		private sealed partial class EntityTypeHasEntityTypeInstanceAdded : AddRule
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
		private sealed partial class EntityTypeInstanceHasRoleInstanceAdded : AddRule
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
		private sealed partial class EntityTypeInstanceHasRoleInstanceDeleted : DeleteRule
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

		[RuleOn(typeof(RoleInstanceHasPopulationUniquenessError))] // DeleteRule
		private sealed partial class RoleInstanceHasPopulationUniquenessErrorDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				RoleInstanceHasPopulationUniquenessError link = e.ModelElement as RoleInstanceHasPopulationUniquenessError;
				PopulationUniquenessError error = link.PopulationUniquenessError;
				if (!error.IsDeleted && error.RoleInstanceCollection.Count <= 1)
				{
					error.Delete();
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
		private sealed partial class ValueTypeHasDataTypeAdded : AddRule
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
		private sealed partial class ValueTypeHasDataTypeRolePlayerChange : RolePlayerChangeRule
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
		private sealed partial class ValueTypeInstanceValueChanged : ChangeRule
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
		private sealed partial class ValueTypeHasValueTypeInstanceAdded : AddRule
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

	public partial class Role : IModelErrorOwner
	{
		private HashSet<ObjectTypeInstance, RoleInstance> myPopulation;

		#region PopulationUniquenessError Validation
		/// <summary>
		/// Validator callback for PopulationUniquenessError
		/// </summary>
		private static void DelayValidatePopulationUniquenessError(ModelElement element)
		{
			(element as Role).ValidatePopulationUniquenessError(null);
		}

		private sealed class RoleInstanceKeyProvider : IKeyProvider<ObjectTypeInstance, RoleInstance>
		{
			private static RoleInstanceKeyProvider provider;

			public static RoleInstanceKeyProvider ProviderInstance
			{
				get
				{
					return provider ?? (provider = new RoleInstanceKeyProvider());
				}
			}

			#region IKeyProvider<ObjectTypeInstance, RoleInstance> Members
			ObjectTypeInstance IKeyProvider<ObjectTypeInstance, RoleInstance>.GetKey(RoleInstance value)
			{
				return value.ObjectTypeInstance;
			}
			#endregion
		}

		/// <summary>
		/// Rule helper for managing the creation and tear down of the cached population verification, as well as validating
		/// whether the population is currently valid.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidatePopulationUniquenessError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				HashSet<ObjectTypeInstance, RoleInstance> population = myPopulation;
				ConstraintRoleSequence singleRoleConstraint = this.SingleRoleAlethicUniquenessConstraint;
				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToObjectTypeInstanceCollection(this);
				int roleInstanceCount = roleInstances.Count;
				if (singleRoleConstraint != null)
				{
					//if (population == null)
					//{
						myPopulation = population = new HashSet<ObjectTypeInstance, RoleInstance>(RoleInstanceKeyProvider.ProviderInstance);
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							AddRoleInstance(roleInstances[i], notifyAdded);
						}
					//}
				}
				else
				{
					if (population != null)
					{
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							PopulationUniquenessError error = roleInstances[i].PopulationUniquenessError;
							if (error != null)
							{
								error.Delete();
							}
						}
						myPopulation = null;
					}
				}
			}
		}

		private void AddRoleInstance(RoleInstance roleInstance, INotifyElementAdded notifyAdded)
		{
			HashSet<ObjectTypeInstance, RoleInstance> population = myPopulation;
			bool duplicate = !population.Add(roleInstance);
			IList<RoleInstance> knownInstances = population.GetValues(roleInstance.ObjectTypeInstance);
			int knownInstanceCount = knownInstances.Count;
			PopulationUniquenessError error = null;
			FactTypeRoleInstance factTypeRoleInstance;
			EntityTypeRoleInstance entityTypeRoleInstance;
			for (int j = 0; error == null && j < knownInstanceCount; ++j)
			{
				error = knownInstances[j].PopulationUniquenessError;
			}
			if (error != null)
			{
				if (duplicate)
				{
					RoleInstanceHasPopulationUniquenessError newLink = null;
					if (null != (factTypeRoleInstance = roleInstance as FactTypeRoleInstance))
					{
						newLink = new FactTypeRoleInstanceHasPopulationUniquenessError(factTypeRoleInstance, error);
					}
					else if (null != (entityTypeRoleInstance = roleInstance as EntityTypeRoleInstance))
					{
						newLink = new EntityTypeRoleInstanceHasPopulationUniquenessError(entityTypeRoleInstance, error);
					}
					if (notifyAdded != null && newLink != null)
					{
						notifyAdded.ElementAdded(newLink);
					}
				}
				else
				{
					error.Delete();
				}
			}
			else if (duplicate)
			{
				error = new PopulationUniquenessError(this.Store);
				for (int i = 0; i < knownInstanceCount; ++i)
				{
					RoleInstance knownInstance = knownInstances[i];
					if (null != (factTypeRoleInstance = knownInstance as FactTypeRoleInstance))
					{
						new FactTypeRoleInstanceHasPopulationUniquenessError(factTypeRoleInstance, error);
					}
					else if (null != (entityTypeRoleInstance = knownInstance as EntityTypeRoleInstance))
					{
						new EntityTypeRoleInstanceHasPopulationUniquenessError(entityTypeRoleInstance, error);
					}
				}
				//error.RoleInstanceCollection.AddRange(knownInstances);
				error.Model = this.FactType.Model;
				error.GenerateErrorText();
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(error, true);
				}
			}
		}
		#endregion
		#region PopulationMandatoryError Validation
		/// <summary>
		/// Validator callback for PopulationMandatoryError
		/// </summary>
		private static void DelayValidatePopulationMandatoryError(ModelElement element)
		{
			(element as Role).ValidatePopulationMandatoryError(null);
		}

		/// <summary>
		/// Rule helper for verifying mandatory constraints on constraint changes
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidatePopulationMandatoryError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				ObjectType parentType;
				MandatoryConstraint constraint;
				if (null != (parentType = this.RolePlayer) &&
					null != (constraint = this.SimpleMandatoryConstraint))
				{
					List<ObjectTypeInstance> instances = new List<ObjectTypeInstance>(RoleInstance.GetObjectTypeInstanceCollection(this));
					instances.Sort(HashCodeComparer<ObjectTypeInstance>.Instance);
					List<ObjectTypeInstance> invalidInstances = new List<ObjectTypeInstance>();
					if (parentType.IsValueType)
					{
						if (parentType.PlayedRoleCollection.Count != 1)
						{
							LinkedElementCollection<ValueTypeInstance> valueInstances = parentType.ValueTypeInstanceCollection;
							int valueInstanceCount = valueInstances.Count;
							for (int i = 0; i < valueInstanceCount; ++i)
							{
								ValueTypeInstance valueInstance = valueInstances[i];
								int index = instances.BinarySearch(valueInstance, HashCodeComparer<ObjectTypeInstance>.Instance);
								if (index < 0)
								{
									invalidInstances.Add(valueInstance);
								}
							}
						}
					}
					else
					{
						ConstraintRoleSequence identifier = parentType.PreferredIdentifier;
						RoleBase oppositeRole = this.OppositeRole;
						if (identifier != null && oppositeRole != null && !identifier.RoleCollection.Contains(oppositeRole.Role))
						{
							LinkedElementCollection<EntityTypeInstance> entityInstances = parentType.EntityTypeInstanceCollection;
							int entityInstanceCount = entityInstances.Count;
							for (int i = 0; i < entityInstanceCount; ++i)
							{
								EntityTypeInstance entityInstance = entityInstances[i];
								int index = instances.BinarySearch(entityInstance, HashCodeComparer<ObjectTypeInstance>.Instance);
								if (index < 0)
								{
									invalidInstances.Add(entityInstance);
								}
							}
						}
					}
					LinkedElementCollection<PopulationMandatoryError> errors = constraint.PopulationMandatoryErrorCollection;
					int errorCount = errors.Count;
					int invalidInstanceCount = invalidInstances.Count;
					if (invalidInstanceCount > 0)
					{
						int instanceIndex;
						for (int i = 0; i < errorCount; ++i)
						{
							PopulationMandatoryError error = errors[i];
							if (-1 != (instanceIndex = invalidInstances.IndexOf(error.ObjectTypeInstance)))
							{
								invalidInstances.RemoveAt(instanceIndex);
							}
							else
							{
								error.Delete();
							}
						}
						invalidInstanceCount = invalidInstances.Count;
						for (int i = 0; i < invalidInstanceCount; ++i)
						{
							PopulationMandatoryError error = new PopulationMandatoryError(this.Store);
							error.ObjectTypeInstance = invalidInstances[i];
							error.MandatoryConstraint = constraint;
							error.Model = constraint.Model;
							error.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(error);
							}
						}
					}
					else
					{
						for (int i = errorCount - 1; i >= 0; --i)
						{
							errors[i].Delete();
						}
					}
				}
			}
		}
		#endregion
		#region Role Rules
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence constraint = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint;
				MandatoryConstraint mandConstraint;
				if(null != (uniConstraint = constraint as UniquenessConstraint))
				{
					Role selectedRole = link.Role;
					if (!selectedRole.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(selectedRole, DelayValidatePopulationUniquenessError);
					}
				}
				else if (null != (mandConstraint = constraint as MandatoryConstraint))
				{
					Role selectedRole = link.Role;
					if (!selectedRole.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(selectedRole, DelayValidatePopulationMandatoryError);
					}
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeleteRule
		private sealed partial class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence constraint = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint;
				MandatoryConstraint mandConstraint;
				if (null != (uniConstraint = constraint as UniquenessConstraint))
				{
					Role selectedRole = link.Role;
					if (!selectedRole.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(selectedRole, DelayValidatePopulationUniquenessError);
					}
				}
				else if (null != (mandConstraint = constraint as MandatoryConstraint))
				{
					Role selectedRole = link.Role;
					if (!selectedRole.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(selectedRole, DelayValidatePopulationMandatoryError);
					}
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // RolePlayerChangedRule
		private sealed partial class ConstraintRoleSequenceHasRoleRolePlayerChanged : RolePlayerChangeRule
		{
			public sealed override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ElementLink as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence constraint = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint;
				MandatoryConstraint mandConstraint;
				if(null != (uniConstraint = constraint as UniquenessConstraint))
				{
					Guid changedRole = e.DomainRole.Id;
					if (changedRole == ConstraintRoleSequence.DomainClassId)
					{
						ORMCoreDomainModel.DelayValidateElement(e.NewRolePlayer as Role, DelayValidatePopulationUniquenessError);
					}
					else if (changedRole == Role.DomainClassId)
					{
						ORMCoreDomainModel.DelayValidateElement(link.Role, DelayValidatePopulationUniquenessError);
					}
				}
				else if (null != (mandConstraint = constraint as MandatoryConstraint))
				{
					Guid changedRole = e.DomainRole.Id;
					if (changedRole == ConstraintRoleSequence.DomainClassId)
					{
						ORMCoreDomainModel.DelayValidateElement(e.NewRolePlayer as Role, DelayValidatePopulationMandatoryError);
					}
					else if (changedRole == Role.DomainClassId)
					{
						ORMCoreDomainModel.DelayValidateElement(link.Role, DelayValidatePopulationMandatoryError);
					}
				}
			}
		}

		[RuleOn(typeof(RoleInstance))] // AddRule
		private sealed partial class RoleInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				RoleInstance roleInstance = e.ModelElement as RoleInstance;
				Role role = roleInstance.Role;
				if (!roleInstance.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(role, DelayValidatePopulationUniquenessError);
				}
			}
		}

		[RuleOn(typeof(RoleInstance))] // DeleteRule
		private sealed partial class RoleInstanceDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				RoleInstance roleInstance = e.ModelElement as RoleInstance;
				Role role = roleInstance.Role;
				if (!roleInstance.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(role, DelayValidatePopulationUniquenessError);
				}
			}
		}

		[RuleOn(typeof(RoleInstance))] // RolePlayerChangedRule
		private sealed partial class RoleInstanceRolePlayerChanged : RolePlayerChangeRule
		{
			public sealed override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Guid changedRole = e.DomainRole.Id;
				RoleInstance roleInstance = e.ElementLink as RoleInstance;
				Role newRole = null;
				if (changedRole == Role.DomainClassId)
				{
					newRole = e.NewRolePlayer as Role;
					Role oldRole = e.OldRolePlayer as Role;
					ORMCoreDomainModel.DelayValidateElement(oldRole, DelayValidatePopulationUniquenessError);
				}
				else if (changedRole == ObjectTypeInstance.DomainClassId)
				{
					newRole = roleInstance.Role;
				}
				if (!roleInstance.IsDeleted && newRole != null)
				{
					ORMCoreDomainModel.DelayValidateElement(newRole, DelayValidatePopulationUniquenessError);
				}
			}
		}
		#endregion
	}

	public partial class ObjectTypeInstance : IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				LinkedElementCollection<PopulationMandatoryError> mandatoryErrorCollection;
				if (null != (mandatoryErrorCollection = PopulationMandatoryErrorCollection))
				{
					int errorCount = mandatoryErrorCollection.Count;
					for (int i = 0; i < errorCount; ++i)
					{
						yield return mandatoryErrorCollection[i];
					}
				}

				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToRoleCollection(this);
				int roleInstanceCount = roleInstances.Count;
				for (int i = 0; i < roleInstanceCount; ++i)
				{
					PopulationUniquenessError error = roleInstances[i].PopulationUniquenessError;
					if (error != null)
					{
						yield return error;
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
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			ValidatePopulationMandatoryError(notifyAdded);
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
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidatePopulationMandatoryError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Custom Storage Handlers
		private string GetNameValue()
		{
			Store store = Store;
			if (store.InUndo || store.InRedo)
			{
				return myGeneratedName;
			}
			else if (!store.TransactionManager.InTransaction)
			{
				string generatedName = myGeneratedName;
				return String.IsNullOrEmpty(generatedName) ? myGeneratedName = GenerateName() : generatedName;
			}
			else
			{
				string generatedName = myGeneratedName;
				if ((object)generatedName != null && generatedName.Length == 0)
				{
					// The == null here is a hack. Use myGeneratedName = null before calling to skip setting this during a transaction
					return myGeneratedName = GenerateName();
				}
				return generatedName ?? String.Empty;
			}
		}
		private void OnObjectTypeInstanceNameChanged()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				NameChanged = tmgr.CurrentTransaction.SequenceNumber;
			}
		}
		private long GetNameChangedValue()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Subtract 1 so that we get a difference in the transaction log
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 1);
			}
			else
			{
				return 0L;
			}
		}
		private void SetNameChangedValue(long newValue)
		{
			if (Store.InUndoRedoOrRollback)
			{
				myGeneratedName = null;
			}
		}
		#endregion
		#region Automatic Name Generation
		private static void DelayValidateObjectTypeInstanceNamePartChanged(ModelElement element)
		{
			ObjectTypeInstance objectTypeInstance = element as ObjectTypeInstance;
			if (!objectTypeInstance.IsDeleted)
			{
				Store store = element.Store;
				string oldGeneratedName = objectTypeInstance.myGeneratedName;
				string newGeneratedName = null;
				bool haveNewName = false;
				//if (string.IsNullOrEmpty(oldGeneratedName))
				//{
				//    objectTypeInstance.myGeneratedName = null; // Set explicitly to null, see notes in GetValueForCustomStoredAttribute
				//}
				//objectTypeInstance.myGeneratedName = newGeneratedName; // See notes in SetValueForCustomStoredAttribute on setting myGeneratedName
				// Now move on to any model errors
				foreach (ModelError error in (objectTypeInstance as IModelErrorOwner).GetErrorCollection(ModelErrorUses.None))
				{
					if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
					{
						if (newGeneratedName == null)
						{
							newGeneratedName = objectTypeInstance.GenerateName();
							haveNewName = true;
							if (newGeneratedName == oldGeneratedName)
							{
								newGeneratedName = null;
								break; // Look no further, name did not change
							}
							else
							{
								if (string.IsNullOrEmpty(oldGeneratedName))
								{
									objectTypeInstance.myGeneratedName = null; // Set explicitly to null, see notes in GetValueForCustomStoredAttribute
								}
								objectTypeInstance.myGeneratedName = newGeneratedName; // See notes in SetValueForCustomStoredAttribute on setting myGeneratedName
							}
						}
						error.GenerateErrorText();
					}
				}
				if (!haveNewName && newGeneratedName == null)
				{
					if (!String.IsNullOrEmpty(oldGeneratedName))
					{
						objectTypeInstance.myGeneratedName = null;
					}
				}
				// Since the name changed, tell any RoleInstances which use it to revalidate since their name probaly changed
				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToRoleCollection(objectTypeInstance);
				int roleInstanceCount = roleInstances.Count;
				EntityTypeRoleInstance entityRoleInstance;
				for (int i = 0; i < roleInstanceCount; ++i)
				{
					if (null != (entityRoleInstance = roleInstances[i] as EntityTypeRoleInstance))
					{
						DelayValidateObjectTypeInstanceNamePartChanged(entityRoleInstance.EntityTypeInstance);
					}
				}
				objectTypeInstance.OnObjectTypeInstanceNameChanged();
			}
		}
		/// <summary>
		/// Helper function to get the current setting for the generated Name property
		/// </summary>
		private string GenerateName()
		{
			string retVal = "";
			EntityTypeInstance entityInstance;
			ValueTypeInstance valueInstance;
			if(null != (entityInstance = this as EntityTypeInstance))
			{
				retVal = ObjectTypeInstance.GetDisplayString(entityInstance, entityInstance.EntityType);
			}
			else if(null != (valueInstance = this as ValueTypeInstance))
			{
				retVal = ObjectTypeInstance.GetDisplayString(valueInstance, valueInstance.ValueType);
			}
			return retVal;
		}
		private string myGeneratedName = String.Empty;
		/// <summary>
		/// The auto-generated name for this object type instance.
		/// </summary>
		public string GeneratedName
		{
			get
			{
				string retVal = myGeneratedName;
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = GenerateName();
					if (retVal.Length != 0)
					{
						if (Store.TransactionManager.InTransaction)
						{
							myGeneratedName = null; // Set explicitly to null, see notes in GetNameValue
							myGeneratedName = retVal;
						}
						else
						{
							myGeneratedName = retVal;
						}
					}
				}
				return retVal ?? String.Empty;
			}
		}
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// Do nothing here. The base calls SetUniqueName, but we don't enforce
			// unique names on the generated ObjectTypeInstance name.
		}
		#endregion // Automatic Name Generation
		#region Base overrides
		/// <summary>
		/// Display the value for ToString
		/// </summary>
		public override string ToString()
		{
			return this.Name;
		}
		#endregion // Base overrides
		#region Helper Methods
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
		#endregion
		#region PopulationMandatoryError Validation
		/// <summary>
		/// Validator callback for PopulationMandatoryError
		/// </summary>
		private static void DelayValidatePopulationMandatoryError(ModelElement element)
		{
			(element as ObjectTypeInstance).ValidatePopulationMandatoryError(null);
		}

		/// <summary>
		/// Rule helper for validating the given instance
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidatePopulationMandatoryError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				EntityTypeInstance entityTypeInstance;
				ValueTypeInstance valueTypeInstance;
				ObjectType parent = null;
				List<Role> playedRoles = null;
				if (null != (entityTypeInstance = this as EntityTypeInstance))
				{
					parent = entityTypeInstance.EntityType;
					if (parent != null)
					{
						playedRoles = new List<Role>(parent.PlayedRoleCollection);
						ConstraintRoleSequence identifier = parent.PreferredIdentifier;
						if (identifier != null)
						{
							LinkedElementCollection<Role> identifierRoles = identifier.RoleCollection;
							int identifierRoleCount = identifierRoles.Count;
							for (int i = 0; i < identifierRoleCount; ++i)
							{
								FactType parentFactType = identifierRoles[i].FactType;
								LinkedElementCollection<RoleBase> factTypeRoles = parentFactType.RoleCollection;
								Role playerRole = null;
								int roleCount = factTypeRoles.Count;
								for (int j = 0; j < roleCount; ++j)
								{
									if (factTypeRoles[j].Role.RolePlayer == parent)
									{
										playerRole = factTypeRoles[j].Role;
										break;
									}
								}
								// If this FactType is connect by a uniqueness constraint, it should contain a role played by
								// the parent ObjectType
								Debug.Assert(playerRole != null);
								playedRoles.Remove(playerRole);
							}
						}
					}
				}
				else if (null != (valueTypeInstance = this as ValueTypeInstance))
				{
					parent = valueTypeInstance.ValueType;
					playedRoles = new List<Role>(parent.PlayedRoleCollection);
				}
				LinkedElementCollection<PopulationMandatoryError> errors = this.PopulationMandatoryErrorCollection;
				List<MandatoryConstraint> violatedConstraints = new List<MandatoryConstraint>();
				if (playedRoles != null)
				{
					List<Role> instanceRoles = new List<Role>(RoleInstance.GetRoleCollection(this));
					instanceRoles.Sort(HashCodeComparer<Role>.Instance);
					int playedRoleCount = playedRoles.Count;
					for (int i = 0; i < playedRoleCount; ++i)
					{
						Role selectedRole = playedRoles[i];
						MandatoryConstraint simpleMandatory = selectedRole.SimpleMandatoryConstraint;
						if (simpleMandatory != null)
						{
							int index = instanceRoles.BinarySearch(playedRoles[i], HashCodeComparer<Role>.Instance);
							if (index < 0)
							{
								violatedConstraints.Add(simpleMandatory);
							}
						}
					}
				}
				int violatedConstraintCount = violatedConstraints.Count;
				int errorCount = errors.Count;
				if (violatedConstraintCount > 0)
				{
					int constraintIndex;
					for (int i = 0; i < errorCount; ++i)
					{
						PopulationMandatoryError error = errors[i];
						if (-1 != (constraintIndex = violatedConstraints.IndexOf(error.MandatoryConstraint as MandatoryConstraint)))
						{
							violatedConstraints.RemoveAt(constraintIndex);
						}
						else
						{
							error.Delete();
						}
					}
					violatedConstraintCount = violatedConstraints.Count;
					for (int i = 0; i < violatedConstraintCount; ++i)
					{
						PopulationMandatoryError error = new PopulationMandatoryError(this.Store);
						error.ObjectTypeInstance = this;
						MandatoryConstraint constraint = violatedConstraints[i];
						error.MandatoryConstraint = constraint;
						error.Model = constraint.Model;
						error.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(error);
						}
					}
				}
				else
				{
					for (int i = 0; i < errorCount; ++i)
					{
						errors[i].Delete();
					}
				}
			}
		}
		#endregion
		#region ObjectTypeInstance Rules
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence constraint = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint;
				if (null != (uniConstraint = constraint as UniquenessConstraint))
				{
					Role oppositeRole = link.Role.OppositeRole.Role;
					if (oppositeRole != null)
					{
						ObjectType rolePlayer = oppositeRole.RolePlayer;
						if (rolePlayer.IsValueType)
						{
							LinkedElementCollection<ValueTypeInstance> instances = rolePlayer.ValueTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
						else
						{
							LinkedElementCollection<EntityTypeInstance> instances = rolePlayer.EntityTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
					}
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeleteRule
		private sealed partial class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence constraint = link.ConstraintRoleSequence;
				UniquenessConstraint uniConstraint;
				if (null != (uniConstraint = constraint as UniquenessConstraint))
				{
					Role oppositeRole = link.Role.OppositeRole.Role;
					if(oppositeRole != null)
					{
						ObjectType rolePlayer = oppositeRole.RolePlayer;
						if (rolePlayer.IsValueType)
						{
							LinkedElementCollection<ValueTypeInstance> instances = rolePlayer.ValueTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
						else
						{
							LinkedElementCollection<EntityTypeInstance> instances = rolePlayer.EntityTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
					}
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // RolePlayerChangedRule
		private sealed partial class ConstraintRoleSequenceHasRoleRolePlayerChanged : RolePlayerChangeRule
		{
			public sealed override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Guid changedRole = e.DomainRole.Id;
				ConstraintRoleSequenceHasRole link = e.ElementLink as ConstraintRoleSequenceHasRole;
				ObjectType newRolePlayer = null;
				if (changedRole == Role.DomainClassId)
				{
					Role oppositeRole = (e.NewRolePlayer as Role).OppositeRole.Role;
					if (oppositeRole != null)
					{
						newRolePlayer = oppositeRole.RolePlayer;
					}
					oppositeRole = (e.OldRolePlayer as Role).OppositeRole.Role;
					if (oppositeRole != null)
					{
						ObjectType oldRolePlayer = oppositeRole.RolePlayer;
						if (oldRolePlayer.IsValueType)
						{
							LinkedElementCollection<ValueTypeInstance> instances = oldRolePlayer.ValueTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
						else
						{
							LinkedElementCollection<EntityTypeInstance> instances = oldRolePlayer.EntityTypeInstanceCollection;
							int instanceCount = instances.Count;
							for (int i = 0; i < instanceCount; ++i)
							{
								ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
							}
						}
					}
				}
				else if (changedRole == ConstraintRoleSequence.DomainClassId)
				{
					Role oppositeRole = link.Role.OppositeRole.Role;
					if(oppositeRole != null)
					{
						newRolePlayer = oppositeRole.RolePlayer;
					}
				}
				if (newRolePlayer != null)
				{
					if (newRolePlayer.IsValueType)
					{
						LinkedElementCollection<ValueTypeInstance> instances = newRolePlayer.ValueTypeInstanceCollection;
						int instanceCount = instances.Count;
						for (int i = 0; i < instanceCount; ++i)
						{
							ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
						}
					}
					else
					{
						LinkedElementCollection<EntityTypeInstance> instances = newRolePlayer.EntityTypeInstanceCollection;
						int instanceCount = instances.Count;
						for (int i = 0; i < instanceCount; ++i)
						{
							ORMCoreDomainModel.DelayValidateElement(instances[i], DelayValidateObjectTypeInstanceNamePartChanged);
						}
					}
				}
			}
		}

		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))] // AddRule
		private sealed partial class EntityTypeInstanceHasRoleInstanceAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = link.EntityTypeInstance;
				ORMCoreDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
		}

		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))] // DeleteRule
		private sealed partial class EntityTypeInstanceHasRoleInstanceDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance instance = link.EntityTypeInstance;
				ORMCoreDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
		}

		[RuleOn(typeof(EntityTypeInstanceHasRoleInstance))] // RolePlayerChangedRule
		private sealed partial class EntityTypeInstanceHasRoleInstanceRolePlayerChanged : RolePlayerChangeRule
		{
			public sealed override void  RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Guid changedRole = e.DomainRole.Id;
				EntityTypeInstanceHasRoleInstance link = e.ElementLink as EntityTypeInstanceHasRoleInstance;
				EntityTypeInstance newInstance = null;
				if (changedRole == EntityTypeInstance.DomainClassId)
				{
					newInstance = e.NewRolePlayer as EntityTypeInstance;
					EntityTypeInstance oldInstance = e.OldRolePlayer as EntityTypeInstance;
					ORMCoreDomainModel.DelayValidateElement(oldInstance, DelayValidateObjectTypeInstanceNamePartChanged);
				}
				else if (changedRole == RoleInstance.DomainClassId)
				{
					newInstance = link.EntityTypeInstance;
				}
				if (newInstance != null)
				{
					ORMCoreDomainModel.DelayValidateElement(newInstance, DelayValidateObjectTypeInstanceNamePartChanged);
				}
			}
		}

		[RuleOn(typeof(ValueTypeInstance))] // ChangeRule
		private sealed partial class ValueTypeInstanceValueChanged : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == ValueTypeInstance.ValueDomainPropertyId)
				{
					ValueTypeInstance instance = e.ModelElement as ValueTypeInstance;
					ORMCoreDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
				}
			}
		}
		#endregion
	}
}
