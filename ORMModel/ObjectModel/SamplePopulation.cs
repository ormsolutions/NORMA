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
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
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
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
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
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
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
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
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
			string newText = String.Format(CultureInfo.CurrentCulture, formatString, objectTypeName, instanceDisplayString, modelName, typeName);
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
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
	public partial class PopulationMandatoryError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectTypeInstance objectInstance = ObjectTypeInstance;
			LinkedElementCollection<Role> roles = MandatoryConstraint.RoleCollection;
			string additionalFactTypes = null;
			int roleCount = roles.Count;
			IFormatProvider formatProvider = CultureInfo.CurrentCulture;
			if (roleCount > 1)
			{
				string additionalFactTypeFormatString = ResourceStrings.ModelErrorModelHasPopulationMandatoryErrorAdditionalFactType;
				for (int i = roleCount - 1; i > 0; --i)
				{
					additionalFactTypes = string.Format(formatProvider, additionalFactTypeFormatString, roles[i].FactType.Name, additionalFactTypes);
				}
			}
			Role role = roles[0];
			string instanceDisplayString = objectInstance.Name;
			string modelName = Model.Name;
			string currentText = ErrorText;
			ObjectType rolePlayer = role.RolePlayer;
			string newText = String.Format(formatProvider, ResourceStrings.ModelErrorModelHasPopulationMandatoryError, rolePlayer != null ? rolePlayer.Name : "", instanceDisplayString, modelName, role.FactType.Name, additionalFactTypes);
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
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns all <see cref="FactType"/>s
		/// associated with the error constraint.
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return MandatoryConstraint.FactTypeCollection.ToArray();
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion

	public partial class FactTypeInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
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
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
		}

		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewFactTypeRoleInstancesError);
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
		/// AddRule: typeof(FactTypeHasRole)
		/// If a Role is added to a FactType's role collection, all FactTypeInstances of that FactType
		/// should be revalidated to ensure that they form a complete instance of the FactType
		/// </summary>
		private static void FactTypeHasRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType parent = link.FactType;
			foreach (FactTypeInstance factTypeInstance in parent.FactTypeInstanceCollection)
			{
				FrameworkDomainModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// If a Role is removed from a FactType's role collection, it will
		/// automatically propagate and destroy any role instances.  This rule
		/// will force deletion of any FactTypeInstances which no longer have
		/// any FactTypeRoleInstances.
		/// </summary>
		private static void FactTypeHasRoleDeletedRule(ElementDeletedEventArgs e)
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
						FrameworkDomainModel.DelayValidateElement(factTypeInstance, DelayValidateTooFewFactTypeRoleInstancesError);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasFactTypeInstance)
		/// If a FactTypeInstance with existing RoleInstances is added
		/// to a FactType, make sure all of the RoleInstance Roles
		/// have the same FactType as a parent
		/// </summary>
		private static void FactTypeHasFactTypeInstanceAdded(ElementAddedEventArgs e)
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
			FrameworkDomainModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
		}
		/// <summary>
		/// AddRule: typeof(FactTypeInstanceHasRoleInstance)
		/// If a RoleInstance with existing roles is added
		/// to a FactTypeInstance, make sure all of the
		/// roles have the same FactType as a parent and that a RoleInstance
		/// for the given role doesn't already exist
		/// </summary>
		private static void FactTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeInstance newInstance = link.FactTypeInstance;
			FactType existingFactType = newInstance.FactType;

			FactTypeRoleInstance roleInstance = link.RoleInstance;
			Role role = roleInstance.Role;
			newInstance.EnsureConsistentRoleOwner(existingFactType, role);
			newInstance.EnsureNonDuplicateRoleInstance(link);
			FrameworkDomainModel.DelayValidateElement(newInstance, DelayValidateTooFewFactTypeRoleInstancesError);
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// If a FactTypeRoleInstance is removed, revalidate the FactTypeInstance
		/// to ensure complete population of its roles.  If the FactTypeRoleInstance
		/// removed was the last one, remove the FactTypeInstance.
		/// </summary>
		private static void FactTypeInstanceHasRoleInstanceDeletedRule(ElementDeletedEventArgs e)
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
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateTooFewFactTypeRoleInstancesError);
				}
			}
		}
		#endregion
	}

	public partial class EntityTypeInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
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
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
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
			base.ValidateErrors(notifyAdded);
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
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
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
		#region IHasIndirectModelErrorOwner Implementation
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Inline Error Helper Methods
		private void EnsureConsistentRoleCollections(ObjectType currentEntityType, Role role)
		{
			UniquenessConstraint preferredIdentifier;
			if (currentEntityType != null &&
				null != (preferredIdentifier = currentEntityType.PreferredIdentifier))
			{
				if (!preferredIdentifier.RoleCollection.Contains(role))
				{
					// If the role didn't match any of the identifiers, throw an error
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceInvalidRolesPreferredIdentifier);
				}
			}
			// If the role is hooked to an entity type but the entityTypeInstance isn't,
			// hook up the entityTypeInstance to the same entityType
			else
			{
				// Make sure the role is actually hooked up to an EntityType
				ObjectType rolePlayer;
				if (null != (rolePlayer = role.RolePlayer) && !rolePlayer.IsValueType)
				{
					this.EntityType = rolePlayer;
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
		/// DeletingRule: typeof(EntityTypeInstance)
		/// If an implied EntityTypeInstance is deleted, then also delete the opposite
		/// ObjectTypeInstance it is associated with for some patterns.
		/// </summary>
		private static void EntityTypeInstanceDeletingRule(ElementDeletingEventArgs e)
		{
			EntityTypeInstance instance = e.ModelElement as EntityTypeInstance;
			ObjectType parent = instance.EntityType;
			UniquenessConstraint identifier;
			LinkedElementCollection<Role> roles;
			Role identifierRole;
			ObjectType identifierPlayer;
			if (null != (identifier = parent.PreferredIdentifier) &&
				identifier.IsInternal &&
				1 == (roles = identifier.RoleCollection).Count &&
				null != (identifierPlayer = (identifierRole = roles[0]).RolePlayer))
			{
				if (identifierRole.SingleRoleAlethicMandatoryConstraint != null)
				{
					int allowedRoleCount = 1;
					if (!identifierPlayer.IsValueType)
					{
						UniquenessConstraint playerIdentifier = identifierPlayer.PreferredIdentifier;
						if (playerIdentifier != null)
						{
							if (playerIdentifier.IsInternal && playerIdentifier.RoleCollection.Count == 1)
							{
								allowedRoleCount = 2;
							}
						}
					}
					if (identifierPlayer.PlayedRoleCollection.Count == allowedRoleCount)
					{
						LinkedElementCollection<EntityTypeRoleInstance> roleInstances = instance.RoleInstanceCollection;
						int roleInstanceCount = roleInstances.Count;
						for (int i = roleInstanceCount - 1; i >= 0; --i)
						{
							EntityTypeRoleInstance roleInstance = roleInstances[i];
							ObjectTypeInstance oppositeInstance = roleInstance.ObjectTypeInstance;
							if (!oppositeInstance.IsDeleting && oppositeInstance.ObjectType == identifierPlayer)
							{
								oppositeInstance.Delete();
							}
						}
					}
				}
				else
				{
					// The opposite identifier role is optional, so there population mandatory errors
					// are possible. Validate the opposite instances.
					foreach (EntityTypeRoleInstance roleInstance in instance.RoleInstanceCollection)
					{
						ObjectTypeInstance oppositeInstance = roleInstance.ObjectTypeInstance;
						if (!oppositeInstance.IsDeleting && oppositeInstance.ObjectType == identifierPlayer)
						{
							FrameworkDomainModel.DelayValidateElement(oppositeInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Clean up ValueTypeInstances when an ObjectType becomes an EntityType
		/// </summary>
		private static void EntityTypeHasPreferredIdentifierAddedRule(ElementAddedEventArgs e)
		{
			ProcessEntityTypeHasPreferredIdentifierAdded(e.ModelElement as EntityTypeHasPreferredIdentifier);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessEntityTypeHasPreferredIdentifierAdded(EntityTypeHasPreferredIdentifier link)
		{
			UniquenessConstraint pid = link.PreferredIdentifier;
			if (pid.IsInternal)
			{
				FrameworkDomainModel.DelayValidateElement(link.PreferredIdentifier, DelayValidatePreferredIdentifier);
			}
		}
		[DelayValidatePriority(1)] // Needs to run after implied mandatory validation on the role players
		private static void DelayValidatePreferredIdentifier(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				UniquenessConstraint preferredIdentifier = (UniquenessConstraint)element;
				LinkedElementCollection<Role> roles;
				Role identifierRole;
				Role identifiedRole;
				ObjectType identifiedObjectType;
				ObjectType identifyingObjectType;
				if (preferredIdentifier.IsInternal &&
					1 == (roles = preferredIdentifier.RoleCollection).Count &&
					null != (identifyingObjectType = (identifierRole = roles[0]).RolePlayer) &&
					null != (identifiedRole = identifierRole.OppositeRole as Role) &&
					(identifiedObjectType = identifiedRole.RolePlayer) == preferredIdentifier.PreferredIdentifierFor)
				{
					// This is a simple reference scheme preferred identifier. All FactTypeInstance
					// populations for this pattern are implicit, so the fact type instances should
					// not be populated. If the preferred identifier role is also mandatory (implied
					// or explicit), then all instances for the role player on that role should be referenced
					// by corresponding EntityTypeInstances on the identified role. All population mandatory
					// errors should be cleared for mandatory constraints on the identified role, and the
					// identifier role should have no population mandatory errors if it is mandatory or if the
					// identified role has any implied populated.
					identifierRole.FactType.FactTypeInstanceCollection.Clear();
					foreach (ConstraintRoleSequence sequence in identifiedRole.ConstraintRoleSequenceCollection)
					{
						MandatoryConstraint constraint = sequence as MandatoryConstraint;
						if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
						{
							constraint.PopulationMandatoryErrorCollection.Clear();
						}
					}
					LinkedElementCollection<ConstraintRoleSequence> identifierConstraintSequences = identifierRole.ConstraintRoleSequenceCollection;
					if (identifierRole.SingleRoleAlethicMandatoryConstraint != null)
					{
						// Full population is implied, clear any population mandatory errors and synchronize the sets
						foreach (ConstraintRoleSequence sequence in identifierConstraintSequences)
						{
							MandatoryConstraint constraint = sequence as MandatoryConstraint;
							if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
							{
								constraint.PopulationMandatoryErrorCollection.Clear();
							}
						}
						EnsureImpliedEntityTypeInstances(identifiedObjectType, identifierRole);
					}
					else
					{
						foreach (ObjectTypeInstance identifyingInstance in identifyingObjectType.ObjectTypeInstanceCollection)
						{
							bool requireError = true;
							bool checkedError = false;
							bool haveMandatoryConstraints = false;

							// Find disjunctive mandatory roles
							foreach (ConstraintRoleSequence sequence in identifierConstraintSequences)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
								{
									if (!checkedError)
									{
										checkedError = true;
										haveMandatoryConstraints = true;
										foreach (EntityTypeRoleInstance roleInstance in EntityTypeRoleInstance.GetLinksToRoleCollection(identifyingInstance))
										{
											if (roleInstance.Role == identifierRole)
											{
												requireError = false;
												break;
											}
										}
									}
									LinkedElementCollection<PopulationMandatoryError> errors = constraint.PopulationMandatoryErrorCollection;
									PopulationMandatoryError error = null;
									foreach (PopulationMandatoryError testError in errors)
									{
										if (testError.ObjectTypeInstance == identifyingInstance)
										{
											error = testError;
											break;
										}
									}
									if (requireError)
									{
										if (error == null)
										{
											error = new PopulationMandatoryError(element.Store);
											error.ObjectTypeInstance = identifyingInstance;
											error.MandatoryConstraint = constraint;
											error.Model = constraint.Model;
										}
										error.GenerateErrorText();
									}
									else if (error != null)
									{
										error.Delete();
									}
								}
							}
							if (!haveMandatoryConstraints)
							{
								break;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Clean up EntityTypeInstances when an ObjectType becomes a ValueTypeInstance
		/// </summary>
		private static void EntityTypeHasPreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
		{
			ProcessEntityTypeHasPreferredIdentifierDeleted(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessEntityTypeHasPreferredIdentifierDeleted(EntityTypeHasPreferredIdentifier link, ObjectType objectType)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (!objectType.IsDeleted)
			{
				objectType.EntityTypeInstanceCollection.Clear();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void EntityTypeHasPreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			Guid changedRoleGuid = e.DomainRole.Id;
			ObjectType oldObjectType = null;
			if (changedRoleGuid == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
			ProcessEntityTypeHasPreferredIdentifierDeleted(link, oldObjectType);
			ProcessEntityTypeHasPreferredIdentifierAdded(link);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is added to an EntityType's preferred identifier collection, all EntityTypeInstances of that EntityType
		/// should be revalidated to ensure that they form a complete instance of the EntityType
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
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
						FrameworkDomainModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is removed from an EntityType's preferred identifier collection, it will
		/// automatically propogate and destroy any role instances.  This rule
		/// will force deletion of any EntityTypeInstances which no longer have
		/// any EntityTypeRoleInstances.
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
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
				for (int i = 0; i < instances.Count; )
				{
					currentInstance = instances[i];
					if (!currentInstance.IsDeleted)
					{
						cleanUp = true;
						roleInstances = currentInstance.RoleInstanceCollection;
						EntityTypeRoleInstance currentRoleInstance;
						for (int j = 0; j < roleInstances.Count; )
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
							FrameworkDomainModel.DelayValidateElement(currentInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
							++i;
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeHasEntityTypeInstance)
		/// If an EntityTypeInstance with existing RoleInstances is added
		/// to an EntityType, ensure that all of the RoleInstances are hooked up to a role in the 
		/// EntityType's preferred identifier.
		/// </summary>
		private static void EntityTypeHasEntityTypeInstanceAddedRule(ElementAddedEventArgs e)
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
			FrameworkDomainModel.DelayValidateElement(entityTypeInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// Ensure that every RoleInstance added to an EntityTypeInstance involves a role
		/// in the EntityType parent's PreferredIdentifier, and that there are no duplicates.
		/// Also validate the EntityTypeInstance to ensure a full instance population.
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeRoleInstance roleInstance = link.RoleInstance;
			EntityTypeInstance entityTypeInstance = link.EntityTypeInstance;
			Role role = roleInstance.Role;
			entityTypeInstance.EnsureConsistentRoleCollections(entityTypeInstance.EntityType, role);
			entityTypeInstance.EnsureNonDuplicateRoleInstance(link);
			FrameworkDomainModel.DelayValidateElement(entityTypeInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
		}
		/// <summary>
		/// DeletingRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// Revalidate the EntityTypeInstance when it loses one of its RoleInstances,
		/// to ensure that the EntityTypeInstance is fully populated.  If the EntityTypeRoleInstance
		/// removed is the last one, remove the parent EntityTypeInstance.
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceDeletingRule(ElementDeletingEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeInstance instance = link.EntityTypeInstance;
			if (!instance.IsDeleting)
			{
				// We're in a Deleting, so we can't trust the count. Get an accurate count to
				// see if the last element is being deleted.
				foreach (EntityTypeInstanceHasRoleInstance remainingRoleInstance in EntityTypeInstanceHasRoleInstance.GetLinksToRoleInstanceCollection(instance))
				{
					if (!remainingRoleInstance.IsDeleting)
					{
						FrameworkDomainModel.DelayValidateElement(instance, DelayValidateTooFewEntityTypeRoleInstancesError);
						return;
					}
				}
				instance.Delete();
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RoleInstanceHasPopulationUniquenessError)
		/// </summary>
		private static void RoleInstanceHasPopulationUniquenessErrorDeletedRule(ElementDeletedEventArgs e)
		{
			RoleInstanceHasPopulationUniquenessError link = e.ModelElement as RoleInstanceHasPopulationUniquenessError;
			PopulationUniquenessError error = link.PopulationUniquenessError;
			if (!error.IsDeleted && error.RoleInstanceCollection.Count <= 1)
			{
				error.Delete();
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
			base.ValidateErrors(notifyAdded);
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
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleValueTypeInstanceValueError);
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
		/// AddRule: typeof(ValueTypeHasDataType)
		/// When the DataType is added, recheck the valueTypeInstance values
		/// </summary>
		private static void ValueTypeHasDataTypeAddedRule(ElementAddedEventArgs e)
		{
			ProcessValueTypeHasDataTypeAdded(e.ModelElement as ValueTypeHasDataType);
		}
		/// <summary>
		/// DeleteRule: typeof(ValueTypeHasDataType)
		/// Get rid of all instance values when we're no longer a value type
		/// </summary>
		private static void ValueTypeHasDataTypeDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectType valueType = (e.ModelElement as ValueTypeHasDataType).ValueType;
			if (!valueType.IsDeleted)
			{
				// UNDONE: Consider allowing 'value' instances to be entered
				// for entity types with no preferred identifier, then merging
				// these values into the value instances for an identifying value
				// type if a single-role internal pid resolves directly (?indirectly)
				// to a single value type
				valueType.ValueTypeInstanceCollection.Clear();
			}
		}
		/// <summary>
		/// Process a data type change for the given value type
		/// </summary>
		/// <param name="link">The ValueTypeHasType relationship instance</param>
		private static void ProcessValueTypeHasDataTypeAdded(ValueTypeHasDataType link)
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
					FrameworkDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// Process a data type change the same as a data type add
		/// </summary>
		private static void ValueTypeHasDataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				ProcessValueTypeHasDataTypeAdded(e.ElementLink as ValueTypeHasDataType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ValueTypeInstance)
		/// Whenever the value of a valueTypeInstance changes, make sure it can be parsed as the current DataType
		/// </summary>
		private static void ValueTypeInstanceValueChangedRule(ElementPropertyChangedEventArgs e)
		{
			ValueTypeInstance valueTypeInstance = e.ModelElement as ValueTypeInstance;
			if (!valueTypeInstance.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
			}
		}
		/// <summary>
		/// AddRule: typeof(ValueTypeHasValueTypeInstance)
		/// Confirms that only ObjectTypes that are actually ValueTypes
		/// can have a ValueTypeInstanceCollection, and confirms that the
		/// given ValueTypeInstance.Value is of the datatype defined in
		/// ValueType.DataType
		/// </summary>
		private static void ValueTypeHasValueTypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			ValueTypeHasValueTypeInstance link = e.ModelElement as ValueTypeHasValueTypeInstance;
			ObjectType valueType = link.ValueType;
			if (!valueType.IsValueType)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionValueTypeInstanceInvalidValueTypeParent);
			}
			ValueTypeInstance valueTypeInstance = link.ValueTypeInstance;
			FrameworkDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
			FrameworkDomainModel.DelayValidateElement(valueTypeInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
		}
		#endregion // ValueTypeInstance Rules
	}

	public partial class Role : IModelErrorOwner
	{
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
			// UNDONE: There is a major amount of work to do in population uniqueness validation
			// of sample populations.
			// 1) The error is modeled incorrectly for anything other than single-role validation. Any
			// population errors should have a relationship to the constraint being violated.
			// 2) If we're building a dictionary to do this, it should be maintained dynamically
			// as events add and remove elements. This is always tricky to do.
			// 3) Extending this to multi-column internal constraint is difficult because the unique
			// element corresponds to an entire FactTypeInstance row (complete ones only) for spanning
			// constraints, or a subset of that population for non-spanning constraints.
			// 4) Extending this to external constraints requires even more work as we would need
			// to synthesize a joined view on the roles/roleinstances (not just use a subset of rows
			// from the FactTypeInstance population). We would also require full join path and join type
			// semantics to be able to handle distributed uniqueness constraints in general, and optional
			// roles for even the simple join case.
			//
			// Basically, to properly hand population validation for uniqueness (not to mention set comparison
			// and other advanced constraints), we need both join path semantics in place as well as live
			// indexed views on the joined sample populations for most of the constraints. Obviously, there
			// is a lot of work to do in this area, but we need to get the join prerequisites in place first.
			if (!IsDeleted)
			{
				UniquenessConstraint singleRoleConstraint = this.SingleRoleAlethicUniquenessConstraint;
				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToObjectTypeInstanceCollection(this);
				int roleInstanceCount = roleInstances.Count;
				if (singleRoleConstraint != null)
				{
					HashSet<ObjectTypeInstance, RoleInstance> population = null;
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						if (population == null)
						{
							population = new HashSet<ObjectTypeInstance, RoleInstance>(RoleInstanceKeyProvider.ProviderInstance);
						}
						AddRoleInstance(population, roleInstances[i], notifyAdded);
					}
				}
				else
				{
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						PopulationUniquenessError error = roleInstances[i].PopulationUniquenessError;
						if (error != null)
						{
							error.Delete();
						}
					}
				}
			}
		}

		private static void AddRoleInstance(HashSet<ObjectTypeInstance, RoleInstance> population, RoleInstance roleInstance, INotifyElementAdded notifyAdded)
		{
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
				error = new PopulationUniquenessError(roleInstance.Store);
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
				error.Model = roleInstance.Role.FactType.Model;
				error.GenerateErrorText();
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(error, true);
				}
			}
		}
		#endregion
		#region Role Rules
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Validate population uniqueness and mandatory errors when a role is added to an internal or implied constraint
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
			{
				switch (constraint.Constraint.ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
						FrameworkDomainModel.DelayValidateElement(link.Role, DelayValidatePopulationUniquenessError);
						break;
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Validate population uniqueness and mandatory errors when a role is deleted from an internal or implied constraint
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
			{
				Role role;
				switch (constraint.Constraint.ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
						if (!(role = link.Role).IsDeleted)
						{
							FrameworkDomainModel.DelayValidateElement(role, DelayValidatePopulationUniquenessError);
						}
						break;
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(UniquenessConstraint)
		/// Validate population uniqueness errors when the modality of a uniqueness constraint changes
		/// </summary>
		private static void UniquenessConstraintChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetConstraint.ModalityDomainPropertyId)
			{
				UniquenessConstraint constraint = (UniquenessConstraint)e.ModelElement;
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
				if (constraint.IsInternal && roles.Count == 1)
				{
					FrameworkDomainModel.DelayValidateElement(roles[0], DelayValidatePopulationUniquenessError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RoleInstance)
		/// Validation population uniqueness when a RoleInstance is added
		/// </summary>
		private static void RoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			RoleInstance roleInstance = e.ModelElement as RoleInstance;
			FrameworkDomainModel.DelayValidateElement(roleInstance.Role, DelayValidatePopulationUniquenessError);
		}
		/// <summary>
		/// DeleteRule: typeof(RoleInstance)
		/// Validation population uniqueness when a RoleInstance is deleted
		/// </summary>
		private static void RoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			RoleInstance roleInstance = e.ModelElement as RoleInstance;
			Role role = roleInstance.Role;
			if (!role.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(role, DelayValidatePopulationUniquenessError);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleInstance)
		/// UNDONE: This rule is garbage, it's comparing DomainRoleId values to DomainClassId values
		/// The rule should probably be a RolePlayerPositionChangeRule, not a RolePlayerChangeRule
		/// </summary>
		private static void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			Guid changedRole = e.DomainRole.Id;
			RoleInstance roleInstance = e.ElementLink as RoleInstance;
			Role newRole = null;
			if (changedRole == Role.DomainClassId)
			{
				newRole = e.NewRolePlayer as Role;
				Role oldRole = e.OldRolePlayer as Role;
				FrameworkDomainModel.DelayValidateElement(oldRole, DelayValidatePopulationUniquenessError);
			}
			else if (changedRole == ObjectTypeInstance.DomainClassId)
			{
				newRole = roleInstance.Role;
			}
			if (!roleInstance.IsDeleted && newRole != null)
			{
				FrameworkDomainModel.DelayValidateElement(newRole, DelayValidatePopulationUniquenessError);
			}
		}
		#endregion // Role Rules
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
			if (filter == (ModelErrorUses)(-1))
			{
				foreach (PopulationMandatoryError populationMandatoryError in PopulationMandatoryErrorCollection)
				{
					yield return populationMandatoryError;
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
			ValidateInstancePopulationMandatoryError(notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateInstancePopulationMandatoryError);
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
		/// <remarks>DelayValidatePriority is set to run after implied mandatory constraint creation</remarks>
		/// </summary>
		[DelayValidatePriority(2)]
		protected static void DelayValidateInstancePopulationMandatoryError(ModelElement element)
		{
			(element as ObjectTypeInstance).ValidateInstancePopulationMandatoryError(null);
		}
		/// <summary>
		/// Return values for the <see cref="ObjectTypeInstance.GetSimpleReferenceSchemePattern"/>
		/// </summary>
		private enum SimpleReferenceSchemeRolePattern
		{
			/// <summary>
			/// The <see cref="Role"/> is not part of a <see cref="FactType"/> that
			/// matches the simple reference scheme pattern.
			/// </summary>
			None,
			/// <summary>
			/// The <see cref="Role"/> is the non-identifying role of a <see cref="FactType"/> that
			/// matches the simple reference scheme pattern. The non-identifying role has a simple
			/// mandatory constraint and non-preferred single-role uniqueness constraint. The
			/// role player is identified by a preferred single-role uniqueness constraint on
			/// the optional opposite role.
			/// </summary>
			OptionalIdentifiedRole,
			/// <summary>
			/// The <see cref="Role"/> is the non-identifying role of a <see cref="FactType"/> that
			/// matches the simple reference scheme pattern. The non-identifying role has a simple
			/// mandatory constraint and non-preferred single-role uniqueness constraint. The
			/// role player is identified by a preferred single-role uniqueness constraint on
			/// the mandatory opposite role.
			/// </summary>
			MandatoryIdentifiedRole,
			/// <summary>
			/// The <see cref="Role"/> is an optional identifying role of a <see cref="FactType"/> that
			/// matches the simple reference scheme pattern. The identifying preferred single-role
			/// uniqueness constraint that is the preferred identifier for the role player of the
			/// opposite role.
			/// </summary>
			OptionalIdentifierRole,
			/// <summary>
			/// The <see cref="Role"/> is a mandatory identifying role of a <see cref="FactType"/> that
			/// matches the simple reference scheme pattern. The identifying preferred single-role
			/// uniqueness constraint that is the preferred identifier for the role player of the
			/// opposite role.
			/// </summary>
			MandatoryIdentifierRole,
		}
		/// <summary>
		/// Determine if the role is part of a <see cref="FactType"/> that matches
		/// the simple reference scheme pattern, namely a single-role preferred identifier
		/// for the opposite role player on one of the roles.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to test</param>
		/// <param name="identifiedEntityType">The <see cref="ObjectType"/> that is identified by the reference scheme pattern.</param>
		/// <returns>A <see cref="SimpleReferenceSchemeRolePattern"/> value</returns>
		private static SimpleReferenceSchemeRolePattern GetSimpleReferenceSchemePattern(Role role, out ObjectType identifiedEntityType)
		{
			UniquenessConstraint uniqueness;
			Role oppositeRole;
			identifiedEntityType = null;
			if (null != (uniqueness = role.SingleRoleAlethicUniquenessConstraint) &&
				null != (oppositeRole = role.OppositeRole as Role))
			{
				ObjectType preferredFor = uniqueness.PreferredIdentifierFor;

				if (preferredFor != null)
				{
					// We're on the preferred end of a binary fact type that
					// matches the simple reference scheme pattern
					if (oppositeRole.RolePlayer == preferredFor)
					{
						identifiedEntityType = preferredFor;
						return role.SingleRoleAlethicMandatoryConstraint == null ?
							SimpleReferenceSchemeRolePattern.OptionalIdentifierRole :
							SimpleReferenceSchemeRolePattern.MandatoryIdentifierRole;
					}
				}
				else
				{
					preferredFor = role.RolePlayer;
					LinkedElementCollection<Role> uniquenessRoles;
					if (preferredFor != null &&
						null != (uniqueness = preferredFor.PreferredIdentifier) &&
						1 == (uniquenessRoles = uniqueness.RoleCollection).Count &&
						uniquenessRoles[0] == oppositeRole)
					{
						identifiedEntityType = preferredFor;
						return oppositeRole.SingleRoleAlethicMandatoryConstraint == null ?
							SimpleReferenceSchemeRolePattern.OptionalIdentifiedRole :
							SimpleReferenceSchemeRolePattern.MandatoryIdentifiedRole;
					}
				}
			}
			return SimpleReferenceSchemeRolePattern.None;
		}
		/// <summary>
		/// Rule helper for validating the current <see cref="ObjectTypeInstance"/>
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateInstancePopulationMandatoryError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				ObjectType objectType;
				if (null != (objectType = this.ObjectType))
				{
					LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
					int playedRoleCount = playedRoles.Count;
					LinkedElementCollection<PopulationMandatoryError> errors = this.PopulationMandatoryErrorCollection;
					if (playedRoleCount == 0)
					{
						errors.Clear();
					}
					else
					{
						ObjectTypeInstance identifyingInstance = null;
						bool retrievedIdentifyingInstance = false;
						for (int i = 0; i < playedRoleCount; ++i)
						{
							Role currentRole = playedRoles[i];
							ReadOnlyLinkedElementCollection<ObjectTypeInstance> currentRoleInstances = null;
							ObjectType identifiedEntityType;
							switch (GetSimpleReferenceSchemePattern(currentRole, out identifiedEntityType))
							{
								//case SimpleReferenceSchemeRolePattern.None:
								//    break;
								case SimpleReferenceSchemeRolePattern.OptionalIdentifiedRole:
									// This one is tricky. The implied FactTypeRoleInstance population
									// for the opposite (identifier) role is based on the set of EntityTypeInstances
									// on this role. Because the opposite role is optional, the population
									// for this role is not automatically filled in, so it is possible to
									// have PopulationMandatoryErrors on any disjunctive mandatory constraint
									// intersecting the opposite role.
									// Note that we always have an opposite role. Otherwise, the simple reference scheme
									// pattern would not hold.
									foreach (ConstraintRoleSequence sequence in currentRole.OppositeRole.Role.ConstraintRoleSequenceCollection)
									{
										MandatoryConstraint constraint = sequence as MandatoryConstraint;
										if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
										{
											if (!retrievedIdentifyingInstance)
											{
												retrievedIdentifyingInstance = true;
												LinkedElementCollection<EntityTypeRoleInstance> identifyingInstances = (this as EntityTypeInstance).RoleInstanceCollection;
												if (identifyingInstances.Count == 1)
												{
													identifyingInstance = identifyingInstances[0].ObjectTypeInstance;
												}
											}
											if (identifyingInstance == null)
											{
												break;
											}
											LinkedElementCollection<PopulationMandatoryError> oppositeErrors = constraint.PopulationMandatoryErrorCollection;
											int errorCount = oppositeErrors.Count;
											for (int j = errorCount - 1; j >= 0; --j)
											{
												PopulationMandatoryError error = oppositeErrors[j];
												if (error.ObjectTypeInstance == identifyingInstance)
												{
													error.Delete();
												}
											}
										}
									}
									continue;
								case SimpleReferenceSchemeRolePattern.MandatoryIdentifierRole:
									// Synchronize the EntityTypeInstances on the opposite role with the identifying object
									// Note that this will trigger the MandatoryIdentifiedRole case on another call.
									EnsureImpliedEntityTypeInstance(this, identifiedEntityType, currentRole);
									continue;
								case SimpleReferenceSchemeRolePattern.MandatoryIdentifiedRole:
									// Nothing to do here. These are populated automatically when instances
									// are added to the opposite role.
								case SimpleReferenceSchemeRolePattern.OptionalIdentifierRole:
									// There is nothing to do here. The implied FactTypeInstance
									// population for this FactType is controlled by the opposite EntityTypeInstance
									// population.
									continue;
							}
							foreach (ConstraintRoleSequence sequence in currentRole.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
								{
									if (currentRoleInstances == null)
									{
										currentRoleInstances = currentRole.ObjectTypeInstanceCollection;
									}
									LinkedElementCollection<Role> constraintRoles = constraint.RoleCollection;
									int constraintRoleCount = constraintRoles.Count;
									int j = 0;
									for (; j < constraintRoleCount; ++j)
									{
										Role constraintRole = constraintRoles[j];
										ReadOnlyLinkedElementCollection<ObjectTypeInstance> roleInstances = (currentRole == constraintRole) ? currentRoleInstances : constraintRole.ObjectTypeInstanceCollection;
										if (roleInstances.Contains(this))
										{
											break;
										}
									}
									if (j == constraintRoleCount)
									{
										// Make sure we have an error
										int errorCount = errors.Count;
										int k = 0;
										for (; k < errorCount; ++k)
										{
											if (errors[k].MandatoryConstraint == constraint)
											{
												break;
											}
										}
										if (k == errorCount)
										{
											PopulationMandatoryError error = new PopulationMandatoryError(this.Store);
											error.ObjectTypeInstance = this;
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
										// Make sure we have no error for this constraint
										int errorCount = errors.Count;
										for (int k = 0; k < errorCount; ++k)
										{
											if (errors[k].MandatoryConstraint == constraint)
											{
												errors[k].Delete();
												break;
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
		/// <summary>
		/// Make sure there is an <see cref="EntityTypeInstance"/> associated with the
		/// <paramref name="entityType"/> for each instance associated with the role player
		/// of <paramref name="identifierRole"/>.
		/// </summary>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="entityType"/></param>
		protected static void EnsureImpliedEntityTypeInstances(ObjectType identifiedEntityType, Role identifierRole)
		{
			ObjectType identifierObjectType = identifierRole.RolePlayer;
			if (identifierObjectType != null)
			{
				// Synchronize the opposite EntityTypeInstance collection
				foreach (ObjectTypeInstance identifierInstance in identifierObjectType.ObjectTypeInstanceCollection)
				{
					EnsureImpliedEntityTypeInstance(identifierInstance, identifiedEntityType, identifierRole);
				}
			}
		}
		/// <summary>
		/// Verify that there is an <see cref="EntityTypeInstance"/> associated with the
		/// <paramref name="entityType"/> that references the specified <paramref name="instance"/>
		/// through the provided <paramref name="identifierRole"/>. Used to create consistent
		/// implicit populations for any <see cref="FactType"/> matching the simple reference scheme
		/// pattern.
		/// </summary>
		/// <param name="instance">The <see cref="ObjectTypeInstance"/> from the identifier role</param>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="entityType"/></param>
		private static void EnsureImpliedEntityTypeInstance(ObjectTypeInstance instance, ObjectType identifiedEntityType, Role identifierRole)
		{
			bool existingInstance = false;
			foreach (EntityTypeRoleInstance roleInstanceLink in EntityTypeRoleInstance.GetLinksToRoleCollection(instance))
			{
				if (roleInstanceLink.EntityTypeInstance.EntityType == identifiedEntityType)
				{
					existingInstance = true;
					break;
				}
			}
			if (!existingInstance)
			{
				EntityTypeInstance newInstance = new EntityTypeInstance(identifiedEntityType.Store);
				newInstance.EntityType = identifiedEntityType;
				new EntityTypeRoleInstance(identifierRole, instance).EntityTypeInstance = newInstance;
			}
		}
		[DelayValidatePriority(1)]
		private static void DelayValidateRemovePopulationMandatoryError(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				RoleInstance roleInstance = (RoleInstance)element;
				Role role = roleInstance.Role;
				ObjectTypeInstance objectTypeInstance = roleInstance.ObjectTypeInstance;
				foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
				{
					MandatoryConstraint mandatory = sequence as MandatoryConstraint;
					if (mandatory != null && mandatory.Modality == ConstraintModality.Alethic)
					{
						LinkedElementCollection<PopulationMandatoryError> populationErrors = mandatory.PopulationMandatoryErrorCollection;
						int populationErrorCount = populationErrors.Count;
						for (int i = populationErrorCount - 1; i >= 0; --i)
						{
							PopulationMandatoryError error = populationErrors[i];
							if (error.ObjectTypeInstance == objectTypeInstance)
							{
								error.Delete();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Validator callback for PopulationMandatoryError. Runs after the much cheaper <see cref="DelayValidateRemovePopulationMandatoryError"/>
		/// </summary>
		[DelayValidatePriority(2)]
		private static void DelayValidateRolePopulationMandatoryError(ModelElement element)
		{
			ValidateRolePopulationMandatoryError((Role)element, null);
		}

		/// <summary>
		/// Rule helper for verifying mandatory constraints on constraint changes
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to verify population for.</param>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private static void ValidateRolePopulationMandatoryError(Role role, INotifyElementAdded notifyAdded)
		{
			if (!role.IsDeleted)
			{
				ObjectType rolePlayer;
				if (null != (rolePlayer = role.RolePlayer))
				{
					ObjectType identifiedEntityType;
					switch (GetSimpleReferenceSchemePattern(role, out identifiedEntityType))
					{
						case SimpleReferenceSchemeRolePattern.None:
							{
								int instanceCount = 0;
								ObjectTypeInstance[] instances = null;
								bool[] seenInstances = null;
								MandatoryConstraint constraint;
								IComparer<ObjectTypeInstance> comparer = HashCodeComparer<ObjectTypeInstance>.Instance;
								ReadOnlyLinkedElementCollection<ObjectTypeInstance> thisRoleObjectTypeInstances = null;
								foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
								{
									constraint = sequence as MandatoryConstraint;
									if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
									{
										int seenInstanceCount = 0;
										// Get repeated stuff once
										if (instances == null)
										{
											instances = rolePlayer.ObjectTypeInstanceCollection.ToArray();
											instanceCount = instances.Length;
											if (instanceCount == 0)
											{
												break;
											}
											Array.Sort<ObjectTypeInstance>(instances, comparer);
											seenInstances = new bool[instanceCount];
											thisRoleObjectTypeInstances = role.ObjectTypeInstanceCollection;
										}
										else
										{
											seenInstances.Initialize();
										}

										// Intersect each role with the instances on the current role player.
										// Note that a disjunctive mandatory constraint with incompatible roles
										// will clearly not intersect, but is still a population error. We do
										// not make role compatibility a prerequisite for checking population
										// mandatory errors.
										LinkedElementCollection<Role> constraintRoles = sequence.RoleCollection;
										int constraintRoleCount = constraintRoles.Count;
										for (int i = 0; i < constraintRoleCount && seenInstanceCount < instanceCount; ++i)
										{
											Role currentRole = constraintRoles[i];
											ReadOnlyLinkedElementCollection<ObjectTypeInstance> roleInstances = (currentRole == role) ? thisRoleObjectTypeInstances : currentRole.ObjectTypeInstanceCollection;
											int roleInstanceCount = roleInstances.Count;
											for (int j = 0; j < roleInstanceCount; ++j)
											{
												int index = Array.BinarySearch<ObjectTypeInstance>(instances, roleInstances[j], comparer);
												if (index >= 0 && !seenInstances[index])
												{
													++seenInstanceCount;
													seenInstances[index] = true;
													if (seenInstanceCount == instanceCount)
													{
														break;
													}
												}
											}
										}

										// We now have all instances that are covered and not covered. Synchronize
										// the error collection on the mandatory constraint
										LinkedElementCollection<PopulationMandatoryError> errors = constraint.PopulationMandatoryErrorCollection;
										int errorCount = errors.Count;
										if (seenInstanceCount == instanceCount)
										{
											if (constraintRoleCount == 1)
											{
												errors.Clear();
											}
											else
											{
												// Because we check this without first enforcing role compatibility, we should not
												// clear the errors that are not involved with the current role player
												for (int i = errorCount - 1; i >= 0; --i)
												{
													PopulationMandatoryError error = errors[i];
													if (error.ObjectTypeInstance.ObjectType == rolePlayer)
													{
														error.Delete();
													}
												}
											}
										}
										else
										{
											// Remove errors we no longer need
											for (int i = errorCount - 1; i >= 0; --i)
											{
												PopulationMandatoryError error = errors[i];
												int index = Array.BinarySearch<ObjectTypeInstance>(instances, error.ObjectTypeInstance, comparer);
												if (index >= 0)
												{
													if (seenInstances[index])
													{
														error.Delete();
													}
													else
													{
														// Make sure the error text is up to date
														error.GenerateErrorText();
														// Use to indicate that we already have an error
														seenInstances[index] = true;
														++seenInstanceCount;
													}
												}
											}

											// Add new errors
											for (int i = 0; i < instanceCount && seenInstanceCount < instanceCount; ++i)
											{
												if (!seenInstances[i])
												{
													++seenInstanceCount;
													PopulationMandatoryError error = new PopulationMandatoryError(role.Store);
													error.ObjectTypeInstance = instances[i];
													error.MandatoryConstraint = constraint;
													error.Model = constraint.Model;
													error.GenerateErrorText();
													if (notifyAdded != null)
													{
														notifyAdded.ElementAdded(error);
													}
												}
											}
										}
									}
								}
							}
							break;
						case SimpleReferenceSchemeRolePattern.OptionalIdentifiedRole:
							// This one is tricky. The implied FactTypeRoleInstance population
							// for the opposite (identifier) role is based on the set of EntityTypeInstances
							// on this role. Because the opposite role is optional, the population
							// for this role is not automatically filled in, so it is possible to
							// have PopulationMandatoryErrors on any disjunctive mandatory constraint
							// intersecting the opposite role.
							// Note that we always have an opposite role. Otherwise, the simple reference scheme
							// pattern would not hold.
							LinkedElementCollection<ObjectTypeInstance> rolePlayerInstances = null;
							foreach (ConstraintRoleSequence sequence in role.OppositeRole.Role.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null)
								{
									if (rolePlayerInstances == null)
									{
										rolePlayerInstances = rolePlayer.ObjectTypeInstanceCollection;
									}
									LinkedElementCollection<PopulationMandatoryError> oppositeErrors = constraint.PopulationMandatoryErrorCollection;
									int errorCount = oppositeErrors.Count;
									for (int j = errorCount - 1; j >= 0; --j)
									{
										PopulationMandatoryError error = oppositeErrors[j];
										if (rolePlayerInstances.Contains(error.ObjectTypeInstance))
										{
											error.Delete();
										}
										else
										{
											error.GenerateErrorText();
										}
									}
								}
							}
							break;
						case SimpleReferenceSchemeRolePattern.MandatoryIdentifierRole:
							// Full population is implied, clear any population mandatory errors and synchronize the sets
							EnsureImpliedEntityTypeInstances(identifiedEntityType, role);
							break;
						case SimpleReferenceSchemeRolePattern.MandatoryIdentifiedRole:
							// Nothing to do here. These are populated automatically when instances
							// are added to the opposite role.
							break;
						case SimpleReferenceSchemeRolePattern.OptionalIdentifierRole:
							// There is nothing to do here. The implied FactTypeInstance
							// population for this FactType is controlled by the opposite EntityTypeInstance
							// population. However, we need to regenerate text for existing mandatory errors.
							foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null)
								{
									foreach (PopulationMandatoryError error in constraint.PopulationMandatoryErrorCollection)
									{
										error.GenerateErrorText();
									}
								}
							}
							break;
					}
				}
			}
		}
		#endregion // PopulationMandatoryError Validation
		#region ObjectTypeInstance Rules
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint == null)
			{
				return;
			}
			switch (constraint.Constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
				case ConstraintType.ExternalUniqueness:
					RoleBase oppositeRoleBase;
					Role oppositeRole;
					ObjectType rolePlayer;
					if (null != (oppositeRoleBase = link.Role.OppositeRole) &&
						null != (oppositeRole = oppositeRoleBase.Role) &&
						null != (rolePlayer = oppositeRole.RolePlayer))
					{
						foreach (ObjectTypeInstance instance in rolePlayer.ObjectTypeInstanceCollection)
						{
							FrameworkDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
						}
					}
					break;
				case ConstraintType.SimpleMandatory:
				case ConstraintType.DisjunctiveMandatory:
				case ConstraintType.ImpliedMandatory:
					FrameworkDomainModel.DelayValidateElement(link.Role, DelayValidateRolePopulationMandatoryError);
					break;
			}
		}

		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint == null)
			{
				return;
			}
			switch (constraint.Constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
				case ConstraintType.ExternalUniqueness:
					Role role;
					RoleBase oppositeRoleBase;
					Role oppositeRole;
					ObjectType rolePlayer;
					if (!(role = link.Role).IsDeleted &&
						null != (oppositeRoleBase = role.OppositeRole) &&
						null != (oppositeRole = oppositeRoleBase.Role) &&
						null != (rolePlayer = oppositeRole.RolePlayer))
					{
						foreach (ObjectTypeInstance instance in rolePlayer.ObjectTypeInstanceCollection)
						{
							FrameworkDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
						}
					}
					break;
				case ConstraintType.SimpleMandatory:
				case ConstraintType.ImpliedMandatory:
				case ConstraintType.DisjunctiveMandatory:
					if (!constraint.IsDeleted)
					{
						if (!(role = link.Role).IsDeleted)
						{
							FrameworkDomainModel.DelayValidateElement(role, DelayValidateRolePopulationMandatoryError);
						}
						foreach (Role remainingRole in constraint.RoleCollection)
						{
							FrameworkDomainModel.DelayValidateElement(remainingRole, DelayValidateRolePopulationMandatoryError);
						}
					}
					break;
			}
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((EntityTypeInstanceHasRoleInstance)e.ModelElement).EntityTypeInstance, DelayValidateObjectTypeInstanceNamePartChanged);
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeInstance instance = link.EntityTypeInstance;
			if (!instance.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
		}
		// UNDONE: This rule is garbage, it's comparing DomainRoleId values to DomainClassId values
		// The rule should probably be a RolePlayerPositionChangeRule, not a RolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			Guid changedRole = e.DomainRole.Id;
			EntityTypeInstanceHasRoleInstance link = e.ElementLink as EntityTypeInstanceHasRoleInstance;
			EntityTypeInstance newInstance = null;
			if (changedRole == EntityTypeInstance.DomainClassId)
			{
				newInstance = e.NewRolePlayer as EntityTypeInstance;
				EntityTypeInstance oldInstance = e.OldRolePlayer as EntityTypeInstance;
				FrameworkDomainModel.DelayValidateElement(oldInstance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
			else if (changedRole == RoleInstance.DomainClassId)
			{
				newInstance = link.EntityTypeInstance;
			}
			if (newInstance != null)
			{
				FrameworkDomainModel.DelayValidateElement(newInstance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ValueTypeInstance)
		/// </summary>
		private static void ValueTypeInstanceValueChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			if (attributeGuid == ValueTypeInstance.ValueDomainPropertyId)
			{
				ValueTypeInstance instance = e.ModelElement as ValueTypeInstance;
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateObjectTypeInstanceNamePartChanged);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Validate population mandatory errors when the <see cref="SetConstraint.Modality"/> of a
		/// <see cref="MandatoryConstraint"/> changes
		/// </summary>
		private static void MandatoryConstraintChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetConstraint.ModalityDomainPropertyId)
			{
				foreach (Role role in ((SetConstraint)e.ModelElement).RoleCollection)
				{
					FrameworkDomainModel.DelayValidateElement(role, DelayValidateRolePopulationMandatoryError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RoleInstance)
		/// Validation population mandatory conditions when a RoleInstance is added
		/// </summary>
		private static void RoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			RoleInstance roleInstance = e.ModelElement as RoleInstance;
			FrameworkDomainModel.DelayValidateElement(roleInstance, DelayValidateRemovePopulationMandatoryError);
		}
		/// <summary>
		/// DeleteRule: typeof(RoleInstance)
		/// Validation population mandatory conditions when a RoleInstance is deleted
		/// </summary>
		private static void RoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			RoleInstance roleInstance = e.ModelElement as RoleInstance;
			Role role = roleInstance.Role;
			if (!role.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(role, DelayValidateRolePopulationMandatoryError);
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Connecting an ObjectType needs to 
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			ProcessRolePlayerAdded((ObjectTypePlaysRole)e.ModelElement);
		}
		private static void ProcessRolePlayerAdded(ObjectTypePlaysRole link)
		{
			FrameworkDomainModel.DelayValidateElement(link.PlayedRole, DelayValidateRolePopulationMandatoryError);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// If a roleplayer link is deleted but the role and roleplayer remain,
		/// then we need to explicitly clear all role instances.
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			ProcessRolePlayerDeleted((ObjectTypePlaysRole)e.ModelElement, null, null);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Treat a role player change on the played role as both an add and delete
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (link.IsDeleted)
			{
				return;
			}
			if (e.DomainRole.Id == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
			{
				ProcessRolePlayerDeleted(link, (Role)e.OldRolePlayer, null);
			}
			else
			{
				ProcessRolePlayerDeleted(link, null, (ObjectType)e.OldRolePlayer);
			}
			ProcessRolePlayerAdded(link);
		}
		private static void ProcessRolePlayerDeleted(ObjectTypePlaysRole link, Role role, ObjectType rolePlayer)
		{
			if (role == null)
			{
				role = link.PlayedRole;
			}
			if (rolePlayer == null)
			{
				rolePlayer = link.RolePlayer;
			}
			if (!role.IsDeleted &&
				!rolePlayer.IsDeleted)
			{
				ReadOnlyCollection<RoleInstance> instances = RoleInstance.GetLinksToObjectTypeInstanceCollection(role);
				int instanceCount = instances.Count;
				for (int i = instanceCount - 1; i >= 0; --i)
				{
					Debug.Assert(instances[i].ObjectTypeInstance.ObjectType == rolePlayer);
					instances[i].Delete();
				}
			}
		}
		#endregion // ObjectTypeInstance Rules
	}
}
