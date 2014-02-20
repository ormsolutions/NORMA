#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
// UNDONE: MSBUG If this is turned on, then an undo of a duplicate RoleInstance does not work correctly.
// This appears to be a problem in the ElementLink.SetRolePlayer method (and downstream internal
// helpers) when duplicate links are allowed. To test the scenario,
// 1) Uncomment the following line (and the corresponding line in SamplePopulationEditor.cs)
// 2) In a new model, use the Fact Editor to add 'A(.id) has B(.name)'
// 3) (This step simplifies the problematic transaction, but does not directly affect the problem)
//    To stop population mandatory and uniqueness errors from appearing, set the IsIndepedent property
//    for A and B to true and do not add any internal uniqueness constraints (ignore the validation error).
// 4) Select the 'A has B' FactType and enter the population {10, Foo} in the ORM Sample Population Editor
// 5) Add a second population row with {20,} (adding a second name value does not matter)
// 6) Select the '10' cell in the first row and select the '20' value from the dropdown (press F2 to activate the editor if it is not active)
// 7) Undo to get an internal error in Microsoft.VisualStudio.Modeling.RolePlayerLinksCollection.RoleLinks.Insert.
// It appears that the linkIndex value is incorrect in this case
//#define ROLEINSTANCE_ROLEPLAYERCHANGE // Keep in sync with define in Shell/SamplePopulationEditor.cs

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
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
			EntityTypeInstance entityInstance = EntityTypeInstance;
			ObjectType entityType = (entityInstance != null) ? entityInstance.EntityType : null;
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesMessage,
				entityType != null ? entityType.Name : "",
				entityInstance != null ? entityInstance.Name : "",
				model != null ? model.Name : "");
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				EntityTypeInstance entityInstance = EntityTypeInstance;
				return string.Format(
					ResourceStrings.ModelErrorEntityTypeInstanceTooFewEntityTypeRoleInstancesCompactMessage,
					entityInstance != null ? entityInstance.Name : "");
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
			FactTypeInstance factInstance = FactTypeInstance;
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesMessage,
				factInstance != null ? factInstance.Name : "",
				model != null ? model.Name : "");
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				FactTypeInstance factInstance = FactTypeInstance;
				return string.Format(
					ResourceStrings.ModelErrorFactTypeInstanceTooFewFactTypeRoleInstancesCompactMessage,
					factInstance != null ? factInstance.Name : "");
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
			// Go ahead and use the value directly here instead of looking at the invariant value.
			// If the invariant value succeeds as a backup then you don't get to this point.
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorValueTypeInstanceCompatibleValueTypeInstanceValueMessage,
				valueTypeInstance != null ? valueTypeInstance.Value : "",
				valueType != null ? valueType.Name : "",
				model != null ? model.Name : "",
				valueType != null ? valueType.DataType.PortableDataType.ToString() : "");
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				ValueTypeInstance valueTypeInstance = ValueTypeInstance;
				ObjectType valueType = (valueTypeInstance != null) ? valueTypeInstance.ValueType : null;
				// Go ahead and use the value directly here instead of looking at the invariant value.
				// If the invariant value succeeds as a backup then you don't get to this point.
				return string.Format(
					ResourceStrings.ModelErrorValueTypeInstanceCompatibleValueTypeInstanceValueCompactMessage,
					valueTypeInstance != null ? valueTypeInstance.Value : "",
					valueType != null ? valueType.DataType.PortableDataType.ToString() : "");
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
		#region PopulationUniquenessError Specific
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
		#endregion // PopulationUniquenessError Specific
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			Role commonRole = CommonRole;
			string roleName = commonRole.Name;
			string formatString;
			string typeName;
			if(roleName.Length != 0)
			{
				formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithNamedRole;
				typeName = roleName;
			}
			else
			{
				formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithUnnamedRole;
				typeName = commonRole.FactType.Name;
			}
			ORMModel model = Model;
			ErrorText = string.Format(
				CultureInfo.CurrentCulture,
				formatString,
				commonRole.RolePlayer.Name,
				DuplicateObjectTypeInstance.Name,
				model != null ? model.Name : "",
				typeName);
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				Role commonRole = CommonRole;
				string roleName = commonRole.Name;
				string formatString;
				string typeName;
				if (roleName.Length != 0)
				{
					formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithNamedRoleCompact;
					typeName = roleName;
				}
				else
				{
					formatString = ResourceStrings.ModelErrorModelHasPopulationUniquenessErrorWithUnnamedRoleCompact;
					typeName = commonRole.FactType.Name;
				}
				return string.Format(
					CultureInfo.CurrentCulture,
					formatString,
					DuplicateObjectTypeInstance.Name,
					typeName);
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
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
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
		#endregion // IHasIndirectModelErrorOwner Implementation
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
			ObjectTypeInstance instance = ObjectTypeInstance;
			ObjectType rolePlayer = instance.ObjectType;
			ORMModel model = Model;
			ErrorText = string.Format(
				formatProvider,
				ResourceStrings.ModelErrorModelHasPopulationMandatoryError,
				rolePlayer != null ? rolePlayer.Name : "",
				instance.Name,
				model != null ? model.Name : "",
				roles[0].FactType.Name,
				additionalFactTypes);
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
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
				ObjectType rolePlayer = role.RolePlayer;
				ORMModel model = Model;
				return string.Format(
					formatProvider,
					ResourceStrings.ModelErrorModelHasPopulationMandatoryErrorCompact,
					ObjectTypeInstance.Name,
					role.FactType.Name,
					additionalFactTypes);
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns all <see cref="Role"/>s
		/// associated with the error constraint, unless the population of the FactType is implied, in which case
		/// the identified <see cref="ObjectType"/> is returned. For an error on a <see cref="SubtypeMetaRole"/> or
		/// <see cref="SupertypeMetaRole"/>, the <see cref="SubtypeFact"/> is returned instead of the role.
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			MandatoryConstraint constraint = MandatoryConstraint;
			LinkedElementCollection<Role> roles = constraint.RoleCollection;
			int roleCount = roles.Count;
			ModelElement[] retVal = new ModelElement[roleCount];
			for (int i = 0; i < roleCount; ++i)
			{
				// For each role, we want the identified object type
				// if the population of the associated FactType is implied.
				// Otherwise, we want the Role. For now, we don't care
				// about duplicates in the returned list.
				Role role = roles[i];
				SupertypeMetaRole supertypeRole;
				SubtypeMetaRole subtypeRole;
				if (null != (supertypeRole = role as SupertypeMetaRole))
				{
					SubtypeFact subtypeFact = (SubtypeFact)role.FactType;
					retVal[i] = subtypeFact.ProvidesPreferredIdentifier ? (ModelElement)subtypeFact.Subtype : subtypeFact;
				}
				else if (null != (subtypeRole = role as SubtypeMetaRole))
				{
					retVal[i] = (SubtypeFact)role.FactType;
				}
				else
				{
					ObjectType identifiedType;
					switch (role.GetReferenceSchemePattern(out identifiedType))
					{
						case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
							retVal[i] = identifiedType;
							break;
						default:
							retVal[i] = role;
							break;
					}
				}
			}
			return retVal;
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
	public partial class ObjectifiedInstanceRequiredError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectTypeInstance instance = this.ObjectTypeInstance;
			ObjectType entityType = (instance != null) ? instance.ObjectType : null;
			ORMModel model = Model;
			ErrorText = string.Format(
				CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorEntityTypeInstanceObjectifiedInstanceRequiredMessage,
				instance != null ? instance.Name : "",
				entityType != null ? entityType.Name : "",
				model != null ? model.Name : "");
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return string.Format(
					CultureInfo.InvariantCulture,
					ResourceStrings.ModelErrorEntityTypeInstanceObjectifiedInstanceRequiredCompactMessage,
					ObjectTypeInstance.Name);
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns associated <see cref="ObjectTypeInstance"/>.
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[]{this.ObjectTypeInstance};
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
	public partial class ObjectifyingInstanceRequiredError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FactTypeInstance instance = this.FactTypeInstance;
			ORMModel model = Model;
			ErrorText = string.Format(
				CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorFactTypeInstanceObjectifyingInstanceRequiredMessage,
				instance != null ? instance.Name : "",
				model != null ? model.Name : "");
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return string.Format(
					CultureInfo.InvariantCulture,
					ResourceStrings.ModelErrorFactTypeInstanceObjectifyingInstanceRequiredCompactMessage,
					FactTypeInstance.Name);
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns associated <see cref="FactTypeInstance"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.FactTypeInstance };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion
	#region FactTypeInstance class
	public partial class FactTypeInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Helper Methods
		/// <summary>
		/// Finds the <see cref="FactTypeRoleInstance"/> for the given <paramref name="role"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="role">Role to match on</param>
		/// <returns>FactTypeRoleInstance for the given role, or null if none found.</returns>
		public FactTypeRoleInstance FindRoleInstance(Role role)
		{
			return FindRoleInstance(RoleInstanceCollection, role);
		}
		/// <summary>
		/// Finds the FactTypeRoleInstance for the given <paramref name="role"/>
		/// in the provided <paramref name="roleInstances"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="roleInstances"></param>
		/// <param name="role">Role to match on</param>
		/// <returns>FactTypeRoleInstance for the given role, or null if none found.</returns>
		public static FactTypeRoleInstance FindRoleInstance(IList<FactTypeRoleInstance> roleInstances, Role role)
		{
			int roleInstanceCount = roleInstances.Count;
			FactTypeRoleInstance roleInstance;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				if ((roleInstance = roleInstances[i]).Role == role)
				{
					return roleInstance;
				}
			}
			return null;
		}
		/// <summary>
		/// Attach the <paramref name="instance"/> to the specified <paramref name="factRole"/>
		/// This routine safed creates a new <see cref="FactTypeRoleInstance"/>. A new FactTypeRoleInstance
		/// should be created directly only for new <see cref="FactTypeInstance"/> elements.
		/// </summary>
		/// <param name="factRole">A role from the <see cref="FactType"/> to attach to</param>
		/// <param name="instance">The instance to attach</param>
		/// <returns>The new (or exisitng) <see cref="FactTypeRoleInstance"/></returns>
		public FactTypeRoleInstance EnsureRoleInstance(Role factRole, ObjectTypeInstance instance)
		{
			FactTypeRoleInstance roleInstance = FindRoleInstance(factRole);
			ObjectTypeInstance existingInstance = null;
			bool sameInstance = false;
			if (roleInstance != null)
			{
				sameInstance = (existingInstance = roleInstance.ObjectTypeInstance) == instance;
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
				if (!sameInstance)
				{
					roleInstance.Delete();
				}
#endif // !ROLEINSTANCE_ROLEPLAYERCHANGE
			}
			if (!sameInstance &&
				existingInstance != (instance = EntityTypeSubtypeInstance.GetTypedInstance(instance, factRole.RolePlayer)))
			{
#if ROLEINSTANCE_ROLEPLAYERCHANGE
				if (roleInstance == null)
				{
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
				roleInstance = new FactTypeRoleInstance(factRole, instance);
				roleInstance.FactTypeInstance = this;
#if ROLEINSTANCE_ROLEPLAYERCHANGE
				}
				else
				{
					roleInstance.ObjectTypeInstance = instance;
				}
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			}
			return roleInstance;
		}
		#endregion // Helper Methods
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

				ObjectifyingInstanceRequiredError objectifyingInstance = ObjectifyingInstanceRequiredError;
				if (objectifyingInstance != null)
				{
					yield return objectifyingInstance;
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
			ValidateObjectifyingInstanceRequiredError(notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateObjectifyingInstanceRequiredError);
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
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
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
		#region Automatic Name Generation
		private string myGeneratedName = String.Empty;
		partial class NamePropertyHandler
		{
			/// <summary>
			/// Add a name modification to the transaction log
			/// without reading the current name, which forces it to regenerated
			/// </summary>
			/// <param name="factInstance">The <see cref="FactTypeInstance"/> to modify</param>
			/// <param name="oldName">The old name to record</param>
			/// <param name="newName">The new name to record</param>
			public static void SetName(FactTypeInstance factInstance, string oldName, string newName)
			{
				factInstance.myGeneratedName = newName;
				Instance.ValueChanged(factInstance, oldName, newName);
			}
		}
		private string GetNameValue()
		{
			Store store = Utility.ValidateStore(Store);
			if (store == null || store.InUndoRedoOrRollback)
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
				if (string.IsNullOrEmpty(generatedName) && !IsDeleting && !IsDeleted)
				{
					generatedName = GenerateName();
					if (!string.IsNullOrEmpty(generatedName))
					{
						NamePropertyHandler.SetName(this, "", generatedName);
					}
				}
				return generatedName ?? String.Empty;
			}
		}
		private void SetNameValue(string newValue)
		{
			Debug.Assert(Store.InUndoRedoOrRollback || (Store.TransactionActive && Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ElementGroupPrototype.CreatingKey)), "Call NamePropertyHandler.SetGeneratedName directly to modify myGeneratedName field.");
			if (Store.InUndoRedoOrRollback)
			{
				// We only set this in undo/redo scenarios so that the initial
				// change on a writable property comes indirectly from the objectifying
				// type changing its name.
				myGeneratedName = newValue;
			}
		}
		private void OnFactTypeInstanceNameChanged()
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
			// Nothing to do, we're just trying to create a transaction log entry
		}
		/// <summary>
		/// Delayed validation handler used to updated the name of an <see cref="ObjectTypeInstance"/>
		/// </summary>
		protected static void DelayValidateNamePartChanged(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FactTypeInstance factInstance = (FactTypeInstance)element;
				Store store = element.Store;
				string oldGeneratedName = factInstance.myGeneratedName;
				string newGeneratedName = null;
				bool haveNewName = false;

				// Keep going with an empty name. Any callback to the Name
				// property will generate on demand
				foreach (ModelError error in ((IModelErrorOwner)factInstance).GetErrorCollection(ModelErrorUses.None))
				{
					if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
					{
						if (newGeneratedName == null)
						{
							newGeneratedName = factInstance.GenerateName();
							haveNewName = true;
							if (newGeneratedName == oldGeneratedName)
							{
								newGeneratedName = null;
								break; // Look no further, name did not change
							}
							else
							{
								// Force a change in the transaction log so that we can
								// undo the generated name as needed
								NamePropertyHandler.SetName(factInstance, oldGeneratedName, newGeneratedName);
							}
						}
						error.GenerateErrorText();
					}
				}
				if (newGeneratedName == null && !haveNewName)
				{
					// Name did not change, but no one cared inside the object model.
					// Add a simple entry to the transaction log
					if (!string.IsNullOrEmpty(oldGeneratedName))
					{
						NamePropertyHandler.SetName(factInstance, oldGeneratedName, "");
					}
				}
				factInstance.OnFactTypeInstanceNameChanged();
			}
		}
		/// <summary>
		/// Generate an empty instance name based solely on a <see cref="FactType"/>
		/// </summary>
		public static string GenerateEmptyInstanceName(FactType factType)
		{
			return GenerateInstanceName(factType, null);
		}
		/// <summary>
		/// Generate an empty instance name based solely on a <see cref="FactType"/>
		/// </summary>
		private static string GenerateInstanceName(FactType factType, FactTypeInstance instance)
		{
			IReading reading = factType.GetDefaultReading();
			bool fakeReading = !(reading is Reading);
			string listSeparator = fakeReading ? CultureInfo.CurrentCulture.TextInfo.ListSeparator + " " : null;
			IList<RoleBase> readingRoles = reading.RoleCollection;
			int lastRoleIndex = readingRoles.Count - 1;
			IList<FactTypeRoleInstance> factRoleInstances = (instance != null) ? instance.RoleInstanceCollection : null;
			return Reading.ReplaceFields(
				reading.Text,
				delegate(int replaceIndex)
				{
					FactTypeRoleInstance roleInstance = (factRoleInstances != null ) ? FindRoleInstance(factRoleInstances, readingRoles[replaceIndex].Role) : null;
					string replacement = (roleInstance != null) ?
						roleInstance.ObjectTypeInstance.Name :
						"?";
					if (fakeReading)
					{
						// Indices are ordered, we know the first and the last
						if (replaceIndex == 0)
						{
							if (0 == lastRoleIndex)
							{
								replacement = "(" + replacement + ")";
							}
							else
							{
								replacement = "(" + replacement + listSeparator;
							}
						}
						else if (replaceIndex == lastRoleIndex)
						{
							replacement = replacement + ")";
						}
						else
						{
							replacement = replacement + listSeparator;
						}
					}
					return replacement;
				});
		}
		/// <summary>
		/// Helper function to get the current setting for the generated Name property
		/// </summary>
		private string GenerateName()
		{
			return GenerateInstanceName(this.FactType, this);
		}
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// Do nothing here. The base calls SetUniqueName, but we don't enforce
			// unique names on the generated ObjectTypeInstance name.
		}
		/// <summary>
		/// Reset the name for each <see cref="FactTypeInstance"/> in the <paramref name="store"/>
		/// </summary>
		/// <param name="store">Context <see cref="Store"/> to reset names for.</param>
		public static void InvalidateNames(Store store)
		{
			foreach (FactTypeInstance factInstance in store.ElementDirectory.FindElements<FactTypeInstance>(false))
			{
				FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);
			}
		}
		#endregion // Automatic Name Generation
		#region Base overrides
		/// <summary>
		/// Display the value for ToString
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
		#endregion // Base overrides
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
						int? unaryRoleIndex;
						if (!(roleCollectionCount == 2 &&
							roleInstancesCount == 1 &&
							(unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles)).HasValue &&
							factRoles[unaryRoleIndex.Value].Role == roleInstances[0].Role))
						{
							hasError = true;
						}
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
						tooFew = new TooFewFactTypeRoleInstancesError(Partition);
						tooFew.FactTypeInstance = this;
						tooFew.Model = parent.ResolvedModel;
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
		#region ObjectifyingInstanceRequiredError Validation
		/// <summary>
		/// Validation callback for <see cref="ObjectifyingInstanceRequiredError"/>
		/// </summary>
		internal static void DelayValidateObjectifyingInstanceRequiredError(ModelElement element)
		{
			// Internal justification: The error being validated is on the FactTypeInstance,
			// but there is a corresponding error on the ObjectTypeInstance which is validated
			// in response to the same rules. Make the delayvalidation routines internal so
			// that we can call them from a single location without duplicating delete and
			// roleplayer changed rules.
			((FactTypeInstance)element).ValidateObjectifyingInstanceRequiredError(null);
		}
		private void ValidateObjectifyingInstanceRequiredError(INotifyElementAdded notifyAdded)
		{
			// Validate that an objectifying instance is specified and that the instance
			// has attached role players. No specified role players results in
			// ObjectifyingInstanceRequiredError instead of TooFewEntityTypeRoleInstancesError
			if (!this.IsDeleted)
			{
				FactType factType;
				ObjectType entityType;
				ObjectTypeInstance objectifyingInstance = null;
				bool hasError = false;
				if (null != (factType = this.FactType) &&
					null != (entityType = factType.NestingType) &&
					null == (objectifyingInstance = this.ObjectifyingInstance) &&
					null != entityType.ResolvedPreferredIdentifier)
				{
					hasError = true;
				}
				else if (objectifyingInstance != null)
				{
					EntityTypeSubtypeInstance subtypeInstance = objectifyingInstance as EntityTypeSubtypeInstance;
					EntityTypeInstance entityInstance = (subtypeInstance != null) ? subtypeInstance.SupertypeInstance : (EntityTypeInstance)objectifyingInstance;
					hasError = entityInstance.RoleInstanceCollection.Count == 0;
				}
				ObjectifyingInstanceRequiredError error = this.ObjectifyingInstanceRequiredError;
				if (hasError)
				{
					if (error == null)
					{
						error = new ObjectifyingInstanceRequiredError(Partition);
						error.FactTypeInstance = this;
						error.Model = factType.ResolvedModel;
						error.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(error, true);
						}
					}
				}
				else if (error != null)
				{
					error.Delete();
				}
			}
		}
		#endregion // ObjectifyingInstanceRequiredError Validation
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
				for (int i = 0; i < linkCount; ++i)
				{
					FactTypeInstanceHasRoleInstance currentLink = currentLinks[i];
					if (link != currentLink && role == currentLink.RoleInstance.Role)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceEnforceRoleUniqueness);
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
		private static void FactTypeRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType parent = link.FactType;
			foreach (FactTypeInstance factInstance in parent.FactTypeInstanceCollection)
			{
				FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// Treat deletion of an implicit boolean role player where the role
		/// is not deleted the same as a role add.
		/// </summary>
		private static void ImpliedBooleanRolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ModelElement;
			Role role;
			FactType factType;
			if (link.RolePlayer.IsImplicitBooleanValue &&
				!(role = link.PlayedRole).IsDeleted &&
				null != (factType = role.FactType))
			{
				foreach (FactTypeInstance factInstance in factType.FactTypeInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// If a Role is removed from a FactType's role collection, it will
		/// automatically propagate and destroy any role instances.  This rule
		/// will force deletion of any FactTypeInstances which no longer have
		/// any FactTypeRoleInstances.
		/// </summary>
		private static void FactTypeRoleDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;
			if (!factType.IsDeleted)
			{
				LinkedElementCollection<FactTypeInstance> factTypeInstances = factType.FactTypeInstanceCollection;
				int factTypeInstanceCount = factTypeInstances.Count;
				for (int i = 0; i < factTypeInstanceCount; ++i)
				{
					FactTypeInstance factInstance = factTypeInstances[i];
					if (!factInstance.IsDeleted)
					{
						FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasFactTypeInstance)
		/// If a FactTypeInstance with existing RoleInstances is added
		/// to a FactType, make sure all of the RoleInstance Roles
		/// have the same FactType as a parent
		/// Automatically add a corresponding <see cref="EntityTypeInstance"/> to
		/// an objectification with an internal preferred identifier in the objectified
		/// <see cref="FactType"/>
		/// </summary>
		private static void FactTypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasFactTypeInstance link = (FactTypeHasFactTypeInstance)e.ModelElement;
			FactType factType = link.FactType;
			SubtypeFact subtypeFact;
			if (factType.ImpliedByObjectification != null ||
				(null != (subtypeFact = factType as SubtypeFact) &&
				subtypeFact.ProvidesPreferredIdentifier))
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceDirectionPopulationOfImpliedInstances);
			}

			// Basic structural check
			FactTypeInstance factInstance = link.FactTypeInstance;
			LinkedElementCollection<FactTypeRoleInstance> roleInstances = factInstance.RoleInstanceCollection;
			int roleInstanceCount = roleInstances.Count;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				// Check each role being related to the FactType
				factInstance.EnsureConsistentRoleOwner(factType, roleInstances[i].Role);
			}
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);

			// Objectification instance handling
			ObjectType entityType;
			UniquenessConstraint pid;
			if (null != (entityType = factType.NestingType) &&
				null != (pid = entityType.ResolvedPreferredIdentifier))
			{
				LinkedElementCollection<FactType> pidFactTypes;
				FactType identifierFactType;
				Role unaryRole = null;
				ObjectifiedUnaryRole objectifiedUnaryRole = null;
				if (pid.PreferredIdentifierFor == entityType &&
					pid.IsInternal &&
					1 == (pidFactTypes = pid.FactTypeCollection).Count &&
					((identifierFactType = pidFactTypes[0]) == factType ||
					(null != (unaryRole = factType.UnaryRole) &&
					null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
					identifierFactType == objectifiedUnaryRole.FactType)))
				{
					// Create a new EntityTypeInstance, populate it based on existing
					// population in the FactTypeInstance, and associated it with
					// the FactTypeInstance.
					EntityTypeInstance entityInstance = new EntityTypeInstance(factType.Partition);
					entityInstance.EntityType = entityType;
					entityInstance.ObjectifiedInstance = factInstance;

					// Attach any preexisting role instances
					if (roleInstanceCount != 0)
					{
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						for (int i = 0; i < roleInstanceCount; ++i)
						{
							FactTypeRoleInstance factRoleInstance = roleInstances[i];
							Role role = factRoleInstance.Role;
							if (unaryRole != null)
							{
								if (role == unaryRole)
								{
									new EntityTypeRoleInstance(objectifiedUnaryRole, factRoleInstance.ObjectTypeInstance).EntityTypeInstance = entityInstance;
								}
							}
							else
							{
								if (pidRoles.Contains(role))
								{
									new EntityTypeRoleInstance(role, factRoleInstance.ObjectTypeInstance).EntityTypeInstance = entityInstance;
								}
							}
						}
					}
				}
				else
				{
					// External identifier, this FactTypeInstance needs an explicit ObjectificationInstance
					FrameworkDomainModel.DelayValidateElement(link.FactTypeInstance, DelayValidateObjectifyingInstanceRequiredError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeInstanceHasRoleInstance)
		/// If a RoleInstance with existing roles is added
		/// to a FactTypeInstance, make sure all of the
		/// roles have the same FactType as a parent and that a RoleInstance
		/// for the given role doesn't already exist
		/// For an objectification pattern with an internal identifying uniquenes,
		/// automatically populate the corresponding role on the implied <see cref="EntityTypeInstance"/>
		/// </summary>
		private static void FactTypeRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = e.ModelElement as FactTypeInstanceHasRoleInstance;
			FactTypeInstance factInstance = link.FactTypeInstance;
			FactType factType = factInstance.FactType;

			// Basic structural verification
			FactTypeRoleInstance factRoleInstance = link.RoleInstance;
			Role role = factRoleInstance.Role;
			factInstance.EnsureConsistentRoleOwner(factType, role);
			factInstance.EnsureNonDuplicateRoleInstance(link);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);

			// ObjectificationInstance handling
			EntityTypeInstance entityInstance;
			ObjectType entityType;
			UniquenessConstraint pid;
			LinkedElementCollection<FactType> pidFactTypes;
			FactType identifierFactType;
			Role unaryRole = null;
			ObjectifiedUnaryRole objectifiedUnaryRole = null;
			if (null != (factType = factInstance.FactType) &&
				null != (entityInstance = factInstance.ObjectifyingInstance as EntityTypeInstance) && // Note that an EntityTypeSubtypeInstance is externally identified and has no implicit population
				null != (entityType = entityInstance.ObjectType) &&
				null != (pid = entityType.PreferredIdentifier) &&
				pid.IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				((identifierFactType = pidFactTypes[0]) == factType ||
				(null != (unaryRole = factType.UnaryRole) &&
				null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
				identifierFactType == objectifiedUnaryRole.FactType)))
			{
				if (unaryRole != null)
				{
					if (pid.RoleCollection.Contains(objectifiedUnaryRole))
					{
						entityInstance.EnsureRoleInstance(objectifiedUnaryRole, factRoleInstance.ObjectTypeInstance);
					}
				}
				else
				{
					if (pid.RoleCollection.Contains(role))
					{
						entityInstance.EnsureRoleInstance(role, factRoleInstance.ObjectTypeInstance);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// If a FactTypeRoleInstance is removed, revalidate the FactTypeInstance
		/// to ensure complete population of its roles.  If the FactTypeRoleInstance
		/// removed was the last one, remove the FactTypeInstance.
		/// </summary>
		private static void FactTypeRoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeInstanceHasRoleInstance link = (FactTypeInstanceHasRoleInstance)e.ModelElement;
			FactTypeInstance factInstance = link.FactTypeInstance;
			if (!factInstance.IsDeleted)
			{
				if (factInstance.RoleInstanceCollection.Count == 0)
				{
					factInstance.Delete();
				}
				else
				{
					// Structural check
					FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactTypeRoleInstancesError);
					FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);

					// ObjectificationInstance handling, manage the implied entityTypeInstance
					EntityTypeInstance entityInstance;
					FactType factType;
					ObjectType entityType;
					UniquenessConstraint pid;
					LinkedElementCollection<FactType> pidFactTypes;
					RoleInstance linkRoleInstance = link.RoleInstance;
					Role role = linkRoleInstance.Role; // Note that role will not be null because both steps are links
					FactType identifierFactType;
					Role unaryRole = null;
					ObjectifiedUnaryRole objectifiedUnaryRole = null;
					if (!role.IsDeleted &&
						null != (entityInstance = factInstance.ObjectifyingInstance as EntityTypeInstance) &&
						null != (factType = factInstance.FactType) &&
						null != (entityType = entityInstance.EntityType) &&
						null != (pid = entityType.PreferredIdentifier) &&
						pid.IsInternal &&
						1 == (pidFactTypes = pid.FactTypeCollection).Count &&
						((identifierFactType = pidFactTypes[0]) == factType ||
						(null != (unaryRole = factType.UnaryRole) &&
						null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
						identifierFactType == objectifiedUnaryRole.FactType)))
					{
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						EntityTypeRoleInstance roleInstance;
						if (unaryRole != null)
						{
							role = objectifiedUnaryRole;
						}
						if (pidRoles.Contains(role) &&
							null != (roleInstance = entityInstance.FindRoleInstance(role)) &&
							!roleInstance.IsDeleting &&
							// The final instance check allows a new instance to be attached to the role before this rule fires
							roleInstance.ObjectTypeInstance == linkRoleInstance.ObjectTypeInstance)
						{
							roleInstance.Delete();
						}
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactType)
		/// Update generated names when the FactType predicate changes
		/// </summary>
		private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == FactType.NameChangedDomainPropertyId)
			{
				foreach (FactTypeInstance instance in ((FactType)e.ModelElement).FactTypeInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ObjectTypeInstance)
		/// Update generated names when the role fields change predicate changes
		/// </summary>
		private static void ObjectTypeInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ObjectTypeInstance.NameChangedDomainPropertyId)
			{
				foreach (FactTypeRoleInstance roleInstance in FactTypeRoleInstance.GetLinksToRoleCollection((ObjectTypeInstance)e.ModelElement))
				{
					FrameworkDomainModel.DelayValidateElement(roleInstance.FactTypeInstance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactTypeRoleInstance)
		/// Update generated names when the associated instances change
		/// </summary>
		private static void FactTypeRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			FactTypeInstance factInstance;
			if (e.DomainRole.Id == FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId && // Note that the opposite role change is blocked in a RoleInstance rule
				null != (factInstance = ((FactTypeRoleInstance)e.ElementLink).FactTypeInstance))
			{
				FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactTypeInstanceHasRoleInstance)
		/// Block role instance from moving to different instance
		/// </summary>
		private static void FactTypeInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionFactTypeInstanceEnforceFixedRoleInstance);
		}
		#endregion
	}
	#endregion // FactTypeInstance class
	#region EntityTypeInstance class
	public partial class EntityTypeInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		private string myGeneratedName = string.Empty;
		private string myGeneratedIdentifierName = string.Empty;
		/// <summary>
		/// Enable name generation
		/// </summary>
		protected override bool HasGeneratedNames
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:Name"/> property
		/// </summary>
		protected override string GenerateName()
		{
			return ObjectTypeInstance.GetDisplayString(this, EntityType, false);
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:IdentifierName"/> property
		/// </summary>
		protected override string GenerateIdentifierName()
		{
			return ObjectTypeInstance.GetDisplayString(this, EntityType, true);
		}
		/// <summary>
		/// Provide storage for the generated <see cref="P:Name"/>
		/// </summary>
		protected override string GeneratedName
		{
			get
			{
				return myGeneratedName;
			}
			set
			{
				myGeneratedName = value;
			}
		}
		/// <summary>
		/// Provide storage for the generated <see cref="P:IdentifierName"/>
		/// </summary>
		protected override string GeneratedIdentifierName
		{
			get
			{
				return myGeneratedIdentifierName;
			}
			set
			{
				myGeneratedIdentifierName = value;
			}
		}
		/// <summary>
		/// If this identifier is empty, then verify that it is both associated with
		/// a <see cref="FactTypeInstance"/> and included in another instance definition.
		/// </summary>
		protected override void DelayValidateIfEmpty()
		{
			if (RoleInstanceCollection.Count == 0)
			{
				FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityTypeRoleInstancesError);
			}
		}
		#endregion // Base overrides
		#region Helper Methods
		/// <summary>
		/// Finds the <see cref="EntityTypeRoleInstance"/> for the given <paramref name="role"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="role">Role to match on</param>
		/// <returns>EntityTypeRoleInstance for the given role, or null if none found.</returns>
		public EntityTypeRoleInstance FindRoleInstance(Role role)
		{
			return FindRoleInstance(RoleInstanceCollection, role);
		}
		/// <summary>
		/// Finds the <see cref="EntityTypeRoleInstance"/> for the given <paramref name="role"/>
		/// in the provided <paramref name="roleInstances"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="roleInstances"></param>
		/// <param name="role">Role to match on</param>
		/// <returns>EntityTypeRoleInstance for the given role, or null if none found.</returns>
		public static EntityTypeRoleInstance FindRoleInstance(IList<EntityTypeRoleInstance> roleInstances, Role role)
		{
			int roleInstanceCount = roleInstances.Count;
			EntityTypeRoleInstance roleInstance;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				if ((roleInstance = roleInstances[i]).Role == role)
				{
					return roleInstance;
				}
			}
			return null;
		}
		/// <summary>
		/// Attach the <paramref name="instance"/> to the specified <paramref name="identifierRole"/>.
		/// This routine safed creates a new <see cref="EntityTypeRoleInstance"/>. A new EntityTypeRoleInstance
		/// should be created directly only for new <see cref="EntityTypeInstance"/> elements.
		/// </summary>
		/// <param name="identifierRole">A role from the entity identifier to attach to</param>
		/// <param name="instance">The instance to attach</param>
		/// <returns>The new (or exisitng) <see cref="EntityTypeRoleInstance"/></returns>
		public EntityTypeRoleInstance EnsureRoleInstance(Role identifierRole, ObjectTypeInstance instance)
		{
			EntityTypeRoleInstance roleInstance = FindRoleInstance(identifierRole);
			bool sameInstance = false;
			if (roleInstance != null)
			{
				sameInstance = roleInstance.ObjectTypeInstance == instance;
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
				if (!sameInstance)
				{
					roleInstance.Delete();
				}
#endif // !ROLEINSTANCE_ROLEPLAYERCHANGE
			}
			if (!sameInstance)
			{

#if ROLEINSTANCE_ROLEPLAYERCHANGE
				if (roleInstance == null)
				{
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
					roleInstance = new EntityTypeRoleInstance(identifierRole, instance);
					roleInstance.EntityTypeInstance = this;
#if ROLEINSTANCE_ROLEPLAYERCHANGE
				}
				else
				{
					roleInstance.ObjectTypeInstance = instance;
				}
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			}
			return roleInstance;
		}
		#endregion // Helper Methods
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
				UniquenessConstraint pid;
				if (parent != null && roleInstances != null && (pid = parent.PreferredIdentifier) != null)
				{
					int roleInstanceCount = roleInstances.Count;
					if (roleInstanceCount == 0)
					{
						// The instance is alive so that a FactTypeInstance can be referenced
						// via an ObjectTypeInstance and the associated ObjectificationInstance.
						// We report rules as if these empty instances do not exist.
						// If we are currently deserializing (notifyAdded is not null), then
						// the error verification will happen with the FactTypeInstance. This
						// routine just needs to verify that the instance is still needed.
						hasError = false;
						bool referencedForFactInstance = false;
						FactTypeInstance factInstance;
						if (null != (factInstance = ObjectifiedInstance))
						{
							referencedForFactInstance = RoleCollection.Count != 0;
							if (notifyAdded == null)
							{
								FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError(factInstance);
							}
						}
						LinkedElementCollection<EntityTypeSubtypeInstance> subtypeInstances = EntityTypeSubtypeInstanceCollection;
						int subtypeInstanceCount = subtypeInstances.Count;
						for (int i = subtypeInstanceCount - 1; i >= 0; --i)
						{
							EntityTypeSubtypeInstance subtypeInstance = subtypeInstances[i];
							bool subtypeInstancedReferencedForFactInstance = false;
							if (null != (factInstance = subtypeInstance.ObjectifiedInstance))
							{
								subtypeInstancedReferencedForFactInstance = subtypeInstance.RoleCollection.Count != 0;
								if (notifyAdded == null)
								{
									FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError(factInstance);
								}
							}
							if (subtypeInstancedReferencedForFactInstance)
							{
								referencedForFactInstance = true;
							}
							else
							{
								subtypeInstance.Delete();
							}
						}
						if (!referencedForFactInstance)
						{
							this.Delete();
							return;
						}
					}
					else
					{
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						int pidRoleCount = pidRoles.Count;
						if (pidRoleCount != roleInstanceCount)
						{
							hasError = true;
						}
						else
						{
							for (int i = 0; !hasError && i < pidRoleCount; ++i)
							{
								bool roleMatch = false;
								for (int j = 0; !hasError && j < roleInstanceCount; ++j)
								{
									if (pidRoles[i] == roleInstances[j].Role)
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
						// We have at least one role, so make sure related FactTypeInstances
						// do not have an ObjectifyingInstanceRequiredError
						if (null == notifyAdded)
						{
							FactTypeInstance factInstance = ObjectifiedInstance;
							if (factInstance != null)
							{
								factInstance.ObjectifyingInstanceRequiredError = null;
							}
							foreach (EntityTypeSubtypeInstance subtypeInstance in EntityTypeSubtypeInstanceCollection)
							{
								if (null != (factInstance = subtypeInstance.ObjectifiedInstance))
								{
									factInstance.ObjectifyingInstanceRequiredError = null;
								}
							}
						}
					}
				}
				TooFewEntityTypeRoleInstancesError tooFew = this.TooFewEntityTypeRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = new TooFewEntityTypeRoleInstancesError(Partition);
						tooFew.EntityTypeInstance = this;
						tooFew.Model = parent.ResolvedModel;
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
		#region ObjectificationInstance Validation
		/// <summary>
		/// Validate objectification instances for a new <see cref="Objectification"/>
		/// </summary>
		[DelayValidatePriority(3)] // Run after subtype instances are in validated
		private static void DelayValidateObjectificationInstances(ModelElement element)
		{
			Objectification objectification = (Objectification)element;
			Partition partition = objectification.Partition;
			FactType factType = objectification.NestedFactType;
			ObjectType entityType = objectification.NestingType;
			bool deletedLink = objectification.IsDeleted;
			UniquenessConstraint pid = null;
			LinkedElementCollection<FactType> pidFactTypes;
			FactType identifierFactType;
			Role unaryRole = null;
			ObjectifiedUnaryRole objectifiedUnaryRole = null;
			if (!deletedLink &&
				null != (pid = entityType.ResolvedPreferredIdentifier) &&
				pid.PreferredIdentifierFor == entityType &&
				pid.IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				((identifierFactType = pidFactTypes[0]) == factType ||
				(null != (unaryRole = factType.UnaryRole) &&
				null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
				identifierFactType == objectifiedUnaryRole.FactType)))
			{
				// Preferred uniqueness constraint is internal to the objectified FactType,
				// so the EntityType instances are implied
				LinkedElementCollection<Role> pidRoles = null;
				LinkedElementCollection<FactTypeInstance> factInstances = factType.FactTypeInstanceCollection;
				int factInstanceCount = factInstances.Count;
				for (int i = 0; i < factInstanceCount; ++i)
				{
					FactTypeInstance factInstance = factInstances[i];
					if (factInstance.ObjectifyingInstance == null)
					{
						factInstance.ObjectifyingInstanceRequiredError = null;
						EntityTypeInstance entityInstance = new EntityTypeInstance(partition);
						entityInstance.EntityType = entityType;
						entityInstance.ObjectifiedInstance = factInstance;

						// Attach role instances
						if (unaryRole != null)
						{
							FactTypeRoleInstance factRoleInstance = factInstance.FindRoleInstance(unaryRole);
							if (factRoleInstance != null)
							{
								pidRoles = pidRoles ?? pid.RoleCollection;
								if (pidRoles.Contains(objectifiedUnaryRole))
								{
									new EntityTypeRoleInstance(objectifiedUnaryRole, factRoleInstance.ObjectTypeInstance).EntityTypeInstance = entityInstance;
								}
							}
						}
						else
						{
							LinkedElementCollection<FactTypeRoleInstance> roleInstances = factInstance.RoleInstanceCollection;
							int roleInstanceCount = roleInstances.Count;
							if (roleInstanceCount != 0)
							{
								pidRoles = pidRoles ?? pid.RoleCollection;
								for (int j = 0; j < roleInstanceCount; ++j)
								{
									FactTypeRoleInstance factRoleInstance = roleInstances[j];
									Role role = factRoleInstance.Role;
									if (pidRoles.Contains(role))
									{
										new EntityTypeRoleInstance(role, factRoleInstance.ObjectTypeInstance).EntityTypeInstance = entityInstance;
									}
								}
							}
						}
					}
				}
				LinkedElementCollection<ObjectTypeInstance> objectInstances = entityType.ObjectTypeInstanceCollection;
				int objectInstanceCount = objectInstances.Count;
				int unattachedCount = objectInstanceCount - factInstanceCount;
				if (unattachedCount > 0)
				{
					// Unlikely, but cleans up any non-implicit entity instances
					for (int i = objectInstanceCount - 1; i >= 0; --i)
					{
						ObjectTypeInstance objectInstance = objectInstances[i];
						if (objectInstance.ObjectifiedInstance == null)
						{
							objectInstance.Delete();
							if (--unattachedCount == 0)
							{
								break;
							}
						}
					}
				}
			}
			else if (!deletedLink && pid != null)
			{
				// Validate instances on each end of the relationship. This is a repeat of
				// the single instance validation routines, but it is not work delay validating
				// each instance to avoid this.
				ORMModel model = null;
				foreach (FactTypeInstance factInstance in factType.FactTypeInstanceCollection)
				{
					ObjectTypeInstance objectifyingInstance = factInstance.ObjectifyingInstance;
					bool hasError = objectifyingInstance == null;
					if (!hasError)
					{
						EntityTypeSubtypeInstance subtypeInstance = objectifyingInstance as EntityTypeSubtypeInstance;
						EntityTypeInstance entityInstance = (subtypeInstance != null) ? subtypeInstance.SupertypeInstance : (EntityTypeInstance)objectifyingInstance;
						hasError = entityInstance.RoleCollection.Count == 0;
					}
					ObjectifyingInstanceRequiredError error = factInstance.ObjectifyingInstanceRequiredError;
					if (hasError)
					{
						if (error == null)
						{
							error = new ObjectifyingInstanceRequiredError(partition);
							error.FactTypeInstance = factInstance;
							error.Model = model ?? (model = factType.ResolvedModel);
							error.GenerateErrorText();
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
				foreach (ObjectTypeInstance objectInstance in entityType.ObjectTypeInstanceCollection)
				{
					ObjectifiedInstanceRequiredError error = objectInstance.ObjectifiedInstanceRequiredError;
					if (objectInstance.ObjectifiedInstance == null)
					{
						if (error == null)
						{
							error = new ObjectifiedInstanceRequiredError(partition);
							error.ObjectTypeInstance = objectInstance;
							error.Model = model ?? (model = entityType.ResolvedModel);
							error.GenerateErrorText();
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
			}
			else
			{
				// Clear all links and errors on remaining instances
				foreach (FactTypeInstance factInstance in factType.FactTypeInstanceCollection)
				{
					factInstance.ObjectifyingInstance = null;
					factInstance.ObjectifyingInstanceRequiredError = null;
				}
				foreach (ObjectTypeInstance objectInstance in entityType.ObjectTypeInstanceCollection)
				{
					objectInstance.ObjectifiedInstance = null;
					objectInstance.ObjectifiedInstanceRequiredError = null;
				}
			}
		}
		#endregion // ObjectificationInstance Validation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
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
				for (int i = 0; i < linkCount; ++i)
				{
					EntityTypeInstanceHasRoleInstance currentLink = currentLinks[i];
					if (link != currentLink && role == currentLink.RoleInstance.Role)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceEnforceRoleUniqueness);
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
					// The opposite identifier role is optional, so population mandatory errors
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
		/// AddRule: typeof(Objectification)
		/// Validate <see cref="ObjectificationInstance"/> elements when an <see cref="Objectification"/> is added
		/// </summary>
		private static void ObjectificationAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateObjectificationInstances);
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Clean up ValueTypeInstances when an ObjectType becomes an EntityType
		/// </summary>
		private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
		{
			ProcessPreferredIdentifierAdded((EntityTypeHasPreferredIdentifier)e.ModelElement);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessPreferredIdentifierAdded(EntityTypeHasPreferredIdentifier link)
		{
			FrameworkDomainModel.DelayValidateElement(link.PreferredIdentifier, DelayValidatePreferredIdentifier);
			ObjectType entityType = link.PreferredIdentifierFor;
			Objectification objectification;
			if (null != (objectification = entityType.Objectification))
			{
				FrameworkDomainModel.DelayValidateElement(objectification, DelayValidateObjectificationInstances);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(Objectification), Priority=1;
		/// Remove all <see cref="ObjectificationInstance"/> links when an objectification
		/// is deleted while both of its endpoints remain intact.
		/// </summary>
		private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
		{
			Objectification link = (Objectification)e.ModelElement;
			ObjectType entityType;
			FactType factType;
			if (!(entityType = link.NestingType).IsDeleted &&
				!(factType = link.NestedFactType).IsDeleted)
			{
				// Note that we run this at priority 1 to let the preferred identifier
				// deletion process first, which will potentially clear the ObjectTypeInstanceCollection
				// and leave us with less to do here.
				foreach (ObjectTypeInstance instance in entityType.ObjectTypeInstanceCollection)
				{
					instance.ObjectifiedInstance = null;
					instance.ObjectifiedInstanceRequiredError = null; // No need to delay validate
				}
				foreach (FactTypeInstance instance in factType.FactTypeInstanceCollection)
				{
					// All ObjectificationInstance relationships will have been taken care of by the previous loop
					instance.ObjectifyingInstanceRequiredError = null;
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(Objectification), Priority=1;
		/// Clean up <see cref="ObjectificationInstance"/> relationships for the old objectification/instance pair
		/// </summary>
		private static void ObjectificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			// Note that we run this at priority 1 to let the preferred identifier
			// deletion process first, which will potentially clear the ObjectTypeInstanceCollection
			// and leave us with less to do here.
			Objectification objectification = (Objectification)e.ElementLink;
			FactType factType;
			ObjectType entityType;
			if (e.DomainRole.Id == Objectification.NestingTypeDomainRoleId)
			{
				factType = objectification.NestedFactType;
				entityType = (ObjectType)e.OldRolePlayer;
			}
			else
			{
				factType = (FactType)e.OldRolePlayer;
				entityType = objectification.NestingType;
			}
			foreach (ObjectTypeInstance instance in entityType.ObjectTypeInstanceCollection)
			{
				instance.ObjectifiedInstance = null;
				instance.ObjectifiedInstanceRequiredError = null; // No need to delay validate
			}
			foreach (FactTypeInstance instance in factType.FactTypeInstanceCollection)
			{
				// All ObjectificationInstance relationships will have been taken care of by the previous loop
				instance.ObjectifyingInstanceRequiredError = null;
			}
			FrameworkDomainModel.DelayValidateElement(objectification, DelayValidateObjectificationInstances);
		}
		[DelayValidatePriority(1)] // Needs to run after implied mandatory validation on the role players and removal of subtype instances
		private static void DelayValidatePreferredIdentifier(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				UniquenessConstraint preferredIdentifier = (UniquenessConstraint)element;
				ObjectType identifiedObjectType;
				if (null != (identifiedObjectType = preferredIdentifier.PreferredIdentifierFor))
				{
					// An ObjectType with a preferred identifier cannot have EntityTypeSubtypeInstances
					identifiedObjectType.EntityTypeSubtypeInstanceCollection.Clear();

					if (!preferredIdentifier.IsObjectifiedPreferredIdentifier)
					{
						// This identifier provides a reference scheme. All FactTypeInstance
						// populations for this pattern are implicit, so the fact type instances should
						// not be populated. If the preferred identifier is simple and the constrained role
						// is also mandatory (implied or explicit), then all instances for the role player
						// on that role should be referenced by corresponding EntityTypeInstances on the identified
						// role. All population mandatory errors should be cleared for mandatory constraints
						// on the identified role, and the identifier role should have no population mandatory
						// errors if it is mandatory or if the identified role has any implied population.
						LinkedElementCollection<Role> identifierRoles = preferredIdentifier.RoleCollection;
						int identifierRoleCount = identifierRoles.Count;
						for (int i = 0; i < identifierRoleCount; ++i)
						{
							Role identifierRole = identifierRoles[i];
							ObjectType identifyingObjectType = identifierRole.RolePlayer;
							Role identifiedRole = identifierRole.OppositeRole as Role;

							identifierRole.FactType.FactTypeInstanceCollection.Clear();

							if (null != identifiedRole)
							{
								foreach (ConstraintRoleSequence sequence in identifiedRole.ConstraintRoleSequenceCollection)
								{
									MandatoryConstraint constraint = sequence as MandatoryConstraint;
									if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
									{
										constraint.PopulationMandatoryErrorCollection.Clear();
									}
								}
							}
							if (identifierRoleCount == 1 && identifierRole.SingleRoleAlethicMandatoryConstraint != null)
							{
								// Full population is implied, clear any population mandatory errors and synchronize the sets
								foreach (ConstraintRoleSequence sequence in identifierRole.ConstraintRoleSequenceCollection)
								{
									MandatoryConstraint constraint = sequence as MandatoryConstraint;
									if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
									{
										constraint.PopulationMandatoryErrorCollection.Clear();
									}
								}
								EnsureImpliedEntityTypeInstances(identifiedObjectType, identifierRole);
							}
							else if (identifyingObjectType != null)
							{
								FrameworkDomainModel.DelayValidateElement(identifierRole, DelayValidateRolePopulationMandatoryError);
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
		private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
		{
			ProcessPreferredIdentifierDeleted(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessPreferredIdentifierDeleted(EntityTypeHasPreferredIdentifier link, ObjectType objectType)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (!(objectType.IsDeleted || objectType.IsDeleting))
			{
				objectType.EntityTypeInstanceCollection.Clear();
				Objectification objectification;
				if (null != (objectification = objectType.Objectification))
				{
					FrameworkDomainModel.DelayValidateElement(objectification, DelayValidateObjectificationInstances);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
			ObjectType oldObjectType = null;
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			else
			{
				// Verify if the pid has been changed for an objectification from one internal
				// constraint on the FactType to another. If this is the case, then fixup any existing
				// entity instances.
				ObjectType entityType = link.PreferredIdentifierFor;
				UniquenessConstraint oldIdentifier;
				UniquenessConstraint newIdentifier;
				FactType objectifiedFactType;
				LinkedElementCollection<FactType> pidFactTypes;
				if (null != (objectifiedFactType = entityType.NestedFactType) &&
					(oldIdentifier = (UniquenessConstraint)e.OldRolePlayer).IsInternal &&
					(newIdentifier = (UniquenessConstraint)link.PreferredIdentifier).IsInternal &&
					1 == (pidFactTypes = oldIdentifier.FactTypeCollection).Count &&
					pidFactTypes[0] == objectifiedFactType &&
					1 == (pidFactTypes = newIdentifier.FactTypeCollection).Count &&
					pidFactTypes[0] == objectifiedFactType)
				{
					bool haveRoleDiff = false;
					Role[] modifiedRoles = null;
					int[] roleOperation = null; // 0 = leave alone, 1 = add, 2 = remove
					int modifiedRoleCount = 0;
					foreach (EntityTypeInstance entityInstance in entityType.ObjectTypeInstanceCollection)
					{
						FactTypeInstance factInstance = entityInstance.ObjectifiedInstance;
						if (factInstance == null)
						{
							continue;
						}
						if (!haveRoleDiff)
						{
							haveRoleDiff = true;
							LinkedElementCollection<Role> oldRoles = oldIdentifier.RoleCollection;
							LinkedElementCollection<Role> newRoles = newIdentifier.RoleCollection;
							LinkedElementCollection<RoleBase> factRoles = objectifiedFactType.RoleCollection;
							modifiedRoleCount = factRoles.Count;
							modifiedRoles = new Role[modifiedRoleCount];
							roleOperation = new int[modifiedRoleCount];
							int nextRoleIndex = 0;
							for (int i = 0; i < modifiedRoleCount; ++i)
							{
								Role testRole = factRoles[i].Role;
								if (oldRoles.Contains(testRole))
								{
									if (newRoles.Contains(testRole))
									{
										continue;
									}
									// Deletion
									modifiedRoles[nextRoleIndex] = testRole;
									roleOperation[nextRoleIndex] = 2;
									++nextRoleIndex;
								}
								else if (newRoles.Contains(testRole))
								{
									modifiedRoles[nextRoleIndex] = testRole;
									roleOperation[nextRoleIndex] = 1;
									++nextRoleIndex;
								}
							}
							if (nextRoleIndex == 0)
							{
								// Nothing to do, the old and new populations are exactly the same
								return;
							}
							modifiedRoleCount = nextRoleIndex;
						}
						for (int i = 0; i < modifiedRoleCount; ++i)
						{
							Role testRole = modifiedRoles[i];
							FactTypeRoleInstance factRoleInstance;
							EntityTypeRoleInstance entityRoleInstance;
							switch (roleOperation[i])
							{
								case 1: // Add
									if (null != (factRoleInstance = factInstance.FindRoleInstance(testRole)))
									{
										entityInstance.EnsureRoleInstance(testRole, factRoleInstance.ObjectTypeInstance);
									}
									break;
								case 2: // Remove
									if (null != (entityRoleInstance = entityInstance.FindRoleInstance(testRole)))
									{
										entityRoleInstance.Delete();
									}
									break;
							}
						}
					}
					return;
				}
			}
			ProcessPreferredIdentifierDeleted(link, oldObjectType);
			ProcessPreferredIdentifierAdded(link);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is added to an EntityType's preferred identifier collection, all EntityTypeInstances of that EntityType
		/// should be revalidated to ensure that they form a complete instance of the EntityType
		///
		/// If an objectified internal preferred identifier is extended, then
		/// automatically fill in the additional roles based on the associated
		/// FactTypeInstance collection.
		/// </summary>
		private static void PreferredIdentifierRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			UniquenessConstraint pid;
			ObjectType entityType;
			if (null != (pid = link.ConstraintRoleSequence as UniquenessConstraint) &&
				null != (entityType = pid.PreferredIdentifierFor))
			{
				// Structural check
				foreach (EntityTypeInstance entityInstance in entityType.EntityTypeInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
				}

				// ObjectificationInstance verification
				FactType factType;
				LinkedElementCollection<FactType> pidFactTypes;
				if (null != (factType = entityType.NestedFactType) &&
					pid.IsInternal &&
					1 == (pidFactTypes = pid.FactTypeCollection).Count &&
					pidFactTypes[0] == factType)
				{
					Role newRole = link.Role;
					foreach (FactTypeInstance factInstance in factType.FactTypeInstanceCollection)
					{
						EntityTypeInstance entityInstance;
						FactTypeRoleInstance factRoleInstance;
						if (null != (entityInstance = factInstance.ObjectifyingInstance as EntityTypeInstance) &&
							null == entityInstance.FindRoleInstance(newRole) &&
							null != (factRoleInstance = factInstance.FindRoleInstance(newRole)))
						{
							entityInstance.EnsureRoleInstance(newRole, factRoleInstance.ObjectTypeInstance);
						}
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is removed from an EntityType's preferred identifier collection, it will
		/// automatically propagate and destroy any role instances.  This rule
		/// will force deletion of any EntityTypeInstances which no longer have
		/// any EntityTypeRoleInstances.
		/// </summary>
		private static void PreferredIdentifierRoleDeletedRule(ElementDeletedEventArgs e)
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
				for (int i = instances.Count - 1; i >= 0; --i)
				{
					currentInstance = instances[i];
					if (!currentInstance.IsDeleting)
					{
						bool cleanUp = true;
						roleInstances = currentInstance.RoleInstanceCollection;
						EntityTypeRoleInstance currentRoleInstance;
						for (int j = roleInstances.Count - 1; j >= 0; --j)
						{
							currentRoleInstance = roleInstances[j];
							if (!currentRoleInstance.IsDeleting)
							{
								if (currentRoleInstance.Role == removedRole)
								{
									currentRoleInstance.Delete();
								}
								else
								{
									cleanUp = false;
								}
							}
						}
						if (cleanUp)
						{
							currentInstance.Delete();
						}
						else
						{
							FrameworkDomainModel.DelayValidateElement(currentInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
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
		private static void EntityTypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntityTypeHasEntityTypeInstance link = (EntityTypeHasEntityTypeInstance)e.ModelElement;
			ObjectType entity = link.EntityType;
			if (entity.IsValueType &&
				CopyMergeUtility.GetIntegrationPhase(link.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceInvalidEntityTypeParent);
			}
			EntityTypeInstance entityInstance = link.EntityTypeInstance;
			ReadOnlyLinkedElementCollection<Role> entityTypeRoleInstances = entityInstance.RoleCollection;
			int roleCount = entityTypeRoleInstances.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				entityInstance.EnsureConsistentRoleCollections(entity, entityTypeRoleInstances[i]);
			}
			FrameworkDomainModel.DelayValidateElement(entityInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// Ensure that every RoleInstance added to an EntityTypeInstance involves a role
		/// in the EntityType parent's PreferredIdentifier, and that there are no duplicates.
		/// Also validate the EntityTypeInstance to ensure a full instance population.
		/// </summary>
		private static void EntityTypeRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = e.ModelElement as EntityTypeInstanceHasRoleInstance;
			EntityTypeRoleInstance roleInstance = link.RoleInstance;
			EntityTypeInstance entityInstance = link.EntityTypeInstance;
			Role role = roleInstance.Role;
			entityInstance.EnsureConsistentRoleCollections(entityInstance.EntityType, role);
			entityInstance.EnsureNonDuplicateRoleInstance(link);
			FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
			FrameworkDomainModel.DelayValidateElement(entityInstance, ObjectTypeInstance.DelayValidateNamePartChanged);
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Revalidate the EntityTypeInstance when it loses one of its RoleInstances,
		/// to ensure that the EntityTypeInstance is fully populated.  If the EntityTypeRoleInstance
		/// removed is the last one, remove the parent EntityTypeInstance.
		/// </summary>
		private static void EntityTypeRoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			EntityTypeInstanceHasRoleInstance link = (EntityTypeInstanceHasRoleInstance)e.ModelElement;
			EntityTypeInstance entityInstance = link.EntityTypeInstance;
			if (!entityInstance.IsDeleted)
			{
				ObjectType entityType;
				FactType factType;
				FactTypeInstance factInstance;
				UniquenessConstraint pid = null;
				LinkedElementCollection<FactType> pidFactTypes;
				FactType identifierFactType;
				Role unaryRole = null;
				ObjectifiedUnaryRole objectifiedUnaryRole = null;
				FactTypeRoleInstance factRoleInstance;
				RoleInstance linkRoleInstance;
				Role role;
				if (null != (factInstance = entityInstance.ObjectifiedInstance) &&
					null != (factType = factInstance.FactType) &&
					null != (entityType = entityInstance.EntityType) &&
					null != (pid = entityType.PreferredIdentifier) &&
					pid.IsInternal &&
					1 == (pidFactTypes = pid.FactTypeCollection).Count &&
					((identifierFactType = pidFactTypes[0]) == factType ||
					(null != (unaryRole = factType.UnaryRole) &&
					null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
					identifierFactType == objectifiedUnaryRole.FactType)) &&
					pid.RoleCollection.Contains(role = (linkRoleInstance = link.RoleInstance).Role) &&
					null != (factRoleInstance = factInstance.FindRoleInstance((role == objectifiedUnaryRole) ? unaryRole : role)) &&
					factRoleInstance.ObjectTypeInstance == linkRoleInstance.ObjectTypeInstance)
				{
					// The instances are implied. This is allowed if the
					// corresponding role instance on the fact instance is
					// deleted, otherwise it is a direct edit of an implied pattern
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceDirectModificationOfImpliedEntityTypeInstance);
				}
				// Defer to TooFew validation for a full check on FactTypeInstance references on empty instances
				FrameworkDomainModel.DelayValidateElement(entityInstance, ObjectTypeInstance.DelayValidateNamePartChanged);
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityTypeRoleInstancesError);
				ObjectTypeInstance oppositeInstance = link.RoleInstance.ObjectTypeInstance;
				if (!oppositeInstance.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(oppositeInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
				}
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
	#endregion // EntityTypeInstance class
	#region ValueTypeInstance class
	public partial class ValueTypeInstance : IModelErrorOwner
	{
		#region Base overrides
		private string NormalizedValue
		{
			get
			{
				string value = Value;
				ObjectType valueType;
				DataType dataType;
				if (null != (valueType = ValueType) &&
					null != (dataType = valueType.DataType))
				{
					value = dataType.NormalizeDisplayText(value, InvariantValue);
				}
				return value;
			}
		}
		/// <summary>
		/// Display the <see cref="Value"/> property
		/// </summary>
		public override string ToString()
		{
			// Note that we would get here eventually with the
			// base implementation, but this cuts out two virtual calls
			return NormalizedValue;
		}
		/// <summary>
		/// Provide the current value for the <see cref="P:Name"/> property
		/// </summary>
		protected override string GeneratedName
		{
			get
			{
				return NormalizedValue;
			}
			set
			{
				// Leave empty per instructions on base class
			}
		}
		/// <summary>
		/// Provide the current value for the <see cref="P:IdentifierName"/> property
		/// </summary>
		protected override string GeneratedIdentifierName
		{
			get
			{
				return NormalizedValue;
			}
			set
			{
				// Leave empty per instructions on base class
			}
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
		/// Validator callback for CompatibleValueTypeInstanceValueError
		/// </summary>
		[DelayValidatePriority(-1)] // Run this early because it also sets or clears invariant values, which are used in other validators
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
				ObjectType parent;
				DataType dataType;
				bool hasError = false;
				if (null != (parent = ValueType) &&
					null != (dataType = parent.DataType))
				{
					if (!dataType.CanParseAnyValue && dataType.IsCultureSensitive)
					{
						string value = Value;
						if (dataType.ParseNormalizeValue(value, InvariantValue, out value))
						{
							InvariantValue = value;
						}
						else
						{
							hasError = true;
						}
					}
					else
					{
						InvariantValue = "";
					}
				}
				CompatibleValueTypeInstanceValueError badValue = this.CompatibleValueTypeInstanceValueError;
				if (hasError)
				{
					if (badValue == null)
					{
						badValue = new CompatibleValueTypeInstanceValueError(Partition);
						badValue.ValueTypeInstance = this;
						badValue.Model = parent.ResolvedModel;
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
			if (e.DomainProperty.Id == ValueTypeInstance.ValueDomainPropertyId)
			{
				ValueTypeInstance instance = (ValueTypeInstance)e.ModelElement;
				instance.InvariantValue = ""; // Clear the invariant value so that it can be reset when the value changes
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateCompatibleValueTypeInstanceValueError);
				FrameworkDomainModel.DelayValidateElement(instance, ObjectTypeInstance.DelayValidateNamePartChanged);
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
			ValueTypeHasValueTypeInstance link = (ValueTypeHasValueTypeInstance)e.ModelElement;
			ObjectType valueType = link.ValueType;
			if (!valueType.IsValueType &&
				CopyMergeUtility.GetIntegrationPhase(link.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionValueTypeInstanceInvalidValueTypeParent);
			}
			ValueTypeInstance valueTypeInstance = link.ValueTypeInstance;
			FrameworkDomainModel.DelayValidateElement(valueTypeInstance, DelayValidateCompatibleValueTypeInstanceValueError);
			FrameworkDomainModel.DelayValidateElement(valueTypeInstance, ObjectTypeInstance.DelayValidateInstancePopulationMandatoryError);
		}
		#endregion // ValueTypeInstance Rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds invariant values as needed.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new InvariantValueFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class InvariantValueFixupListener : DeserializationFixupListener<ValueTypeHasDataType>
		{
			/// <summary>
			/// InvariantValueFixupListener constructor
			/// </summary>
			public InvariantValueFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Process instance collections for the ValueType of ValueTypeHasDataType elements
			/// </summary>
			/// <param name="element">A ValueTypeHasDataType element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ValueTypeHasDataType element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					DataType dataType = element.DataType;
					if (dataType.IsCultureSensitive)
					{
						foreach (ValueTypeInstance instance in element.ValueType.ValueTypeInstanceCollection)
						{
							string value;
							string invariantValue;
							if ((invariantValue = instance.InvariantValue).Length == 0 &&
								(value = instance.Value).Length != 0 &&
								dataType.TryConvertToInvariant(value, out invariantValue))
							{
								instance.InvariantValue = invariantValue;
							}
						}
					}
				}
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // ValueTypeInstance class
	#region Role class
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
						RoleInstance roleInstance = roleInstances[i];
						EntityTypeRoleInstance entityTypeRoleInstance;
						FactTypeRoleInstance factTypeRoleInstance = null;
						ObjectTypeInstance boundInstance = roleInstance.ObjectTypeInstance;
						if (null != (entityTypeRoleInstance = roleInstance as EntityTypeRoleInstance))
						{
							// Check for objectified pairs, which will end up as duplicates if
							// we don't check.
							EntityTypeInstance entityInstance;
							FactTypeInstance pairedFactInstance;
							FactTypeRoleInstance pairedRoleInstance;
							if (null != (entityInstance = entityTypeRoleInstance.EntityTypeInstance) &&
								null != (pairedFactInstance = entityInstance.ObjectifiedInstance) &&
								null != (pairedRoleInstance = pairedFactInstance.FindRoleInstance(this)) &&
								pairedRoleInstance.ObjectTypeInstance == boundInstance)
							{
								continue;
							}
						}
						else
						{
							factTypeRoleInstance = (FactTypeRoleInstance)roleInstance;
						}
						if (population == null)
						{
							population = new HashSet<ObjectTypeInstance, RoleInstance>(RoleInstanceKeyProvider.ProviderInstance);
						}
						bool duplicate = !population.Add(roleInstance);
						IList<RoleInstance> knownInstances = population.GetValues(boundInstance);
						int knownInstanceCount = knownInstances.Count;
						PopulationUniquenessError error = null;
						for (int j = 0; error == null && j < knownInstanceCount; ++j)
						{
							error = knownInstances[j].PopulationUniquenessError;
						}
						if (error != null)
						{
							if (duplicate)
							{
								RoleInstanceHasPopulationUniquenessError newLink = null;
								if (null != factTypeRoleInstance)
								{
									newLink = new FactTypeRoleInstanceHasPopulationUniquenessError(factTypeRoleInstance, error);
								}
								else
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
							error = new PopulationUniquenessError(roleInstance.Partition);
							for (int j = 0; j < knownInstanceCount; ++j)
							{
								RoleInstance knownInstance = knownInstances[j];
								if (null != (factTypeRoleInstance = knownInstance as FactTypeRoleInstance))
								{
									new FactTypeRoleInstanceHasPopulationUniquenessError(factTypeRoleInstance, error);
								}
								else
								{
									new EntityTypeRoleInstanceHasPopulationUniquenessError((EntityTypeRoleInstance)knownInstance, error);
								}
							}
							error.Model = this.FactType.ResolvedModel;
							error.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(error, true);
							}
						}
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
		/// </summary>
		private static void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			// Note that role changes are blocked by the corresponding rule in ObjectTypeInstance.RoleInstanceRolePlayerChangedRule
			if (Utility.IsDescendantOrSelf(e.DomainRole, RoleInstance.ObjectTypeInstanceDomainRoleId) && e.OldRolePlayer != e.NewRolePlayer)
			{
				FrameworkDomainModel.DelayValidateElement(((RoleInstance)e.ElementLink).Role, DelayValidatePopulationUniquenessError);
			}
		}
		#endregion // Role Rules
	}
	#endregion // Role class
	#region ObjectTypeInstance class
	partial class ObjectTypeInstance : IModelErrorOwner
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
				ObjectifiedInstanceRequiredError objectifiedInstance = ObjectifiedInstanceRequiredError;
				if (objectifiedInstance != null)
				{
					yield return objectifiedInstance;
				}
			}
			if (filter == (ModelErrorUses)(-1))
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
			ValidateObjectifiedInstanceRequiredError(notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateObjectifiedInstanceRequiredError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Automatic Name Generation
		partial class NamePropertyHandler
		{
			/// <summary>
			/// Add a Name modification to the transaction log
			/// without reading the current Name, which forces it to regenerated
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectTypeInstance"/> to modify</param>
			/// <param name="oldName">The old name to record</param>
			/// <param name="newName">The new name to record</param>
			public static void SetName(ObjectTypeInstance objectInstance, string oldName, string newName)
			{
				objectInstance.GeneratedName = newName;
				Instance.ValueChanged(objectInstance, oldName, newName);
			}
		}
		partial class IdentifierNamePropertyHandler
		{
			/// <summary>
			/// Add an IdentifierName modification to the transaction log
			/// without reading the current Identifiername, which forces it to regenerated
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectTypeInstance"/> to modify</param>
			/// <param name="oldIdentifierName">The old name to record</param>
			/// <param name="newIdentifierName">The new name to record</param>
			public static void SetIdentifierName(ObjectTypeInstance objectInstance, string oldIdentifierName, string newIdentifierName)
			{
				objectInstance.GeneratedIdentifierName = newIdentifierName;
				Instance.ValueChanged(objectInstance, oldIdentifierName, newIdentifierName);
			}
		}
		/// <summary>
		/// Generate the instance name on demand.
		/// </summary>
		protected string GetNameValue()
		{
			Store store = Utility.ValidateStore(Store);
			if (store == null || store.InUndoRedoOrRollback || !HasGeneratedNames)
			{
				return GeneratedName;
			}
			else if (!store.TransactionManager.InTransaction)
			{
				string currentName = GeneratedName;
				return String.IsNullOrEmpty(currentName) ? GeneratedName = GenerateName() : currentName;
			}
			else
			{
				string currentName = GeneratedName;
				if (string.IsNullOrEmpty(currentName) && !IsDeleting && !IsDeleted)
				{
					currentName = GenerateName();
					if (!string.IsNullOrEmpty(currentName))
					{
						NamePropertyHandler.SetName(this, "", currentName);
					}
				}
				return currentName ?? String.Empty;
			}
		}
		/// <summary>
		/// Allow derived classes to optimize name generation by deferring to another instance
		/// </summary>
		protected virtual string GetIdentifierNameValue()
		{
			Store store = Utility.ValidateStore(Store);
			if (store == null || store.InUndoRedoOrRollback || !HasGeneratedNames)
			{
				return GeneratedIdentifierName;
			}
			else if (!store.TransactionManager.InTransaction)
			{
				string currentName = GeneratedIdentifierName;
				return String.IsNullOrEmpty(currentName) ? GeneratedIdentifierName = GenerateIdentifierName() : currentName;
			}
			else
			{
				string currentName = GeneratedIdentifierName;
				if (string.IsNullOrEmpty(currentName) && !IsDeleting)
				{
					currentName = GenerateIdentifierName();
					if (!string.IsNullOrEmpty(currentName))
					{
						IdentifierNamePropertyHandler.SetIdentifierName(this, "", currentName);
					}
				}
				return currentName ?? String.Empty;
			}
		}
		private void SetNameValue(string newValue)
		{
			Debug.Assert(Store.InUndoRedoOrRollback || (Store.TransactionActive && Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ElementGroupPrototype.CreatingKey)), "Call NamePropertyHandler.SetName directly to modify the stored GeneratedName value.");
			if (Store.InUndoRedoOrRollback)
			{
				// We only set this in undo/redo scenarios so that the initial
				// change on a writable property comes indirectly from the objectifying
				// type changing its name.
				GeneratedName = newValue;
			}
		}
		private void SetIdentifierNameValue(string newValue)
		{
			Debug.Assert(Store.InUndoRedoOrRollback || (Store.TransactionActive && Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ElementGroupPrototype.CreatingKey)), "Call IdentifierNamePropertyHandler.SetIdentifierName directly to modify the store GeneratedIdentifierName value.");
			if (Store.InUndoRedoOrRollback)
			{
				// We only set this in undo/redo scenarios so that the initial
				// change on a writable property comes indirectly from the objectifying
				// type changing its name.
				GeneratedIdentifierName = newValue;
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
			// Nothing to do, we're just trying to create a transaction log entry
		}
		/// <summary>
		/// Delayed validation handler used to updated the name of an <see cref="ObjectTypeInstance"/>
		/// </summary>
		protected static void DelayValidateNamePartChanged(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ObjectTypeInstance objectTypeInstance = (ObjectTypeInstance)element;
				bool logEmptyName = false;
				bool logEmptyIdentifierName = false;
				if (objectTypeInstance.HasGeneratedNames)
				{
					string oldGeneratedName = objectTypeInstance.GeneratedName;
					if (!string.IsNullOrEmpty(oldGeneratedName))
					{
						string newGeneratedName = objectTypeInstance.GenerateName();
						if (newGeneratedName == oldGeneratedName)
						{
							return;
						}
						else
						{
							// Force a change in the transaction log so that we can
							// undo the generated name as needed
							NamePropertyHandler.SetName(objectTypeInstance, oldGeneratedName, newGeneratedName);
						}
					}
					else
					{
						// Add an entry changing the blank to a blank. If we do not do this, then
						// there is no transaction record, and a name that is generated on demand outside
						// the transaction is not cleared on undo, so it does not get regenerated with
						// the original name.
						logEmptyName = true;
					}
					string oldGeneratedIdentifierName = objectTypeInstance.GeneratedIdentifierName;
					if (!string.IsNullOrEmpty(oldGeneratedIdentifierName))
					{
						// We don't use this name directly, clear it so that it can regenerate on demand
						IdentifierNamePropertyHandler.SetIdentifierName(objectTypeInstance, oldGeneratedIdentifierName, "");
					}
					else
					{
						logEmptyIdentifierName = true;
					}
				}
				// Keep going with an empty name. Any callback to the Name
				// property will generate on demand
				foreach (ModelError error in (objectTypeInstance as IModelErrorOwner).GetErrorCollection(ModelErrorUses.None))
				{
					if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
					{
						error.GenerateErrorText();
					}
				}

				// Since the name changed, tell any RoleInstances which use it to revalidate since their name probably changed
				foreach (EntityTypeRoleInstance entityRoleInstance in EntityTypeRoleInstance.GetLinksToRoleCollection(objectTypeInstance))
				{
					EntityTypeInstance relatedEntityInstance = entityRoleInstance.EntityTypeInstance;
					if (relatedEntityInstance != null)
					{
						FrameworkDomainModel.DelayValidateElement(relatedEntityInstance, DelayValidateNamePartChanged);
					}
				}

				// Subtype instances need refreshing
				EntityTypeInstance entityInstance = objectTypeInstance as EntityTypeInstance;
				if (entityInstance != null)
				{
					foreach (EntityTypeSubtypeInstance subtypeInstance in entityInstance.EntityTypeSubtypeInstanceCollection)
					{
						FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateNamePartChanged);
					}
				}

				if (logEmptyName && string.IsNullOrEmpty(objectTypeInstance.GeneratedName))
				{
					// If the name has not been regenerated on demand, then log an empty entry so
					// that an undo clears any non-logged generated name
					NamePropertyHandler.SetName(objectTypeInstance, "", "");
				}
				if (logEmptyIdentifierName && string.IsNullOrEmpty(objectTypeInstance.GeneratedIdentifierName))
				{
					// Same comments as name, we need a history to revert the state on undo
					IdentifierNamePropertyHandler.SetIdentifierName(objectTypeInstance, "", "");
				}
				objectTypeInstance.OnObjectTypeInstanceNameChanged();
			}
		}
		/// <summary>
		/// Overide and return true to enable name generation
		/// </summary>
		protected virtual bool HasGeneratedNames
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Helper function to generate the current value for the <see cref="Name"/> property.
		/// Override with <see cref="HasGeneratedNames"/>
		/// </summary>
		protected virtual string GenerateName()
		{
			return null;
		}
		/// <summary>
		/// Helper function to generate the current value for the <see cref="IdentifierName"/> property.
		/// Override with <see cref="HasGeneratedNames"/>
		/// </summary>
		protected virtual string GenerateIdentifierName()
		{
			return null;
		}
		/// <summary>
		/// Provide the current stored value of the generated name. The setter should be
		/// be empty unless an override is also specified for <see cref="HasGeneratedNames"/>.
		/// </summary>
		protected abstract string GeneratedName { get; set; }
		/// <summary>
		/// Provide the current stored value of the generated identifier name. The setter should
		/// be empty unless an override is also specified for <see cref="HasGeneratedNames"/>.
		/// </summary>
		protected abstract string GeneratedIdentifierName { get; set; }
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// Do nothing here. The base calls SetUniqueName, but we don't enforce
			// unique names on the generated ObjectTypeInstance name.
		}
		/// <summary>
		/// Reset the name for each <see cref="ObjectTypeInstance"/> with a cached name in the <paramref name="store"/>
		/// </summary>
		/// <param name="store">Context <see cref="Store"/> to reset names for.</param>
		public static void InvalidateNames(Store store)
		{
			IElementDirectory directory = store.ElementDirectory;
			foreach (EntityTypeInstance entityInstance in directory.FindElements<EntityTypeInstance>(false))
			{
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateNamePartChanged);
			}
			foreach (EntityTypeSubtypeInstance subtypeInstance in directory.FindElements<EntityTypeSubtypeInstance>(false))
			{
				FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateNamePartChanged);
			}
		}
		#endregion // Automatic Name Generation
		#region Base overrides
		/// <summary>
		/// Display the value for ToString
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
		#endregion // Base overrides
		#region Helper Methods
		/// <summary>
		/// Returns the display string for the given <paramref name="objectInstance"/>, or the
		/// nested tuple structure for the <paramref name="parentObjectType"/> if the instance is not provided.
		/// </summary>
		/// <param name="objectInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentObjectType">Parent Type of the instance.</param>
		/// <param name="ignoreObjectification">Set to true to generate a name based solely on the identifier instead of using the objectified FactType.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(ObjectTypeInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification)
		{
			return GetDisplayString(objectInstance, parentObjectType, ignoreObjectification, null, null, null);
		}

		/// <summary>
		/// Returns the display string for the given <paramref name="factInstance"/>, or the
		/// nested tuple structure for the <paramref name="parentFactType"/> if the instance is not provided.
		/// </summary>
		/// <param name="factInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentFactType">Parent Type of the instance.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(FactTypeInstance factInstance, FactType parentFactType)
		{
			return GetDisplayString(factInstance, parentFactType, null, null, null);
		}

		/// <summary>
		/// Returns the display string for the given <paramref name="objectInstance"/>, or the
		/// nested tuple structure for the <paramref name="parentObjectType"/> if the instance is not provided.
		/// </summary>
		/// <param name="objectInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentObjectType">Parent Type of the instance.</param>
		/// <param name="ignoreObjectification">Set to true to generate a name based solely on the identifier instead of using the objectified FactType.</param>
		/// <param name="formatProvider">Format provider for desired culture.</param>
		/// <param name="valueNonTextFormat">Format string for non text value type instances.</param>
		/// <param name="valueTextFormat">Format string for text value type instances.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(ObjectTypeInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
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
			string listSeparator = null;
			string retVal = (parentObjectType == null) ? "" : RecurseObjectTypeInstanceValue(objectInstance, parentObjectType, ignoreObjectification, false, null, null, ref listSeparator, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat, false);
			retVal = (outputText != null) ? outputText.ToString() : retVal;
			return (retVal == null) ? "" : retVal.Trim();
		}

		/// <summary>
		/// Returns the display string for the given <paramref name="factInstance"/>, or the
		/// nested tuple structure for the <paramref name="parentFactType"/> if the instance is not provided.
		/// </summary>
		/// <param name="factInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentFactType">Parent Type of the instance.</param>
		/// <param name="formatProvider">Format provider for desired culture.</param>
		/// <param name="valueNonTextFormat">Format string for non text value type instances.</param>
		/// <param name="valueTextFormat">Format string for text value type instances.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(FactTypeInstance factInstance, FactType parentFactType, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
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
			string listSeparator = null;
			string retVal = (parentFactType == null) ? "" : RecurseObjectTypeInstanceValue(null, null, false, false, factInstance, parentFactType, ref listSeparator, ref outputText, formatProvider, valueTextFormat, valueNonTextFormat, false);
			retVal = (outputText != null) ? outputText.ToString() : retVal;
			return (retVal == null) ? "" : retVal.Trim();
		}

		private static string RecurseObjectTypeInstanceValue(ObjectTypeInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification, bool nestedLeadValue, FactTypeInstance factInstance, FactType parentFactType, ref string listSeparator, ref StringBuilder outputText, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat, bool outerGrouping)
		{
			string blankValueText = nestedLeadValue ? " " : "";
			DataType dataType;
			if (parentObjectType == null && parentFactType == null)
			{
				if (outputText != null)
				{
					outputText.Append(blankValueText);
				}
				return blankValueText;
			}
			else if (parentObjectType != null &&
				null != (dataType = parentObjectType.DataType)) // A ValueType
			{
				ValueTypeInstance valueInstance = objectInstance as ValueTypeInstance;
				string valueText = blankValueText;
				if (valueInstance != null)
				{
					valueText = dataType.NormalizeDisplayText(valueInstance.Value, valueInstance.InvariantValue);
					if (valueTextFormat != null && parentObjectType.DataType is TextDataType)
					{
						valueText = string.Format(formatProvider ?? dataType.CurrentCulture, valueTextFormat, valueText);
					}
					else if (valueNonTextFormat != null)
					{
						valueText = string.Format(formatProvider ?? dataType.CurrentCulture, valueNonTextFormat, valueText);
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
				IList<Role> roles = null;
				int roleCount = 0;
				FactType objectifiedFactType = null;
				EntityTypeInstance entityInstance = null;
				if (parentFactType != null)
				{
					// Just do the FactType tuple, without any identifier parts
					IList<RoleBase> factRoles = parentFactType.OrderedRoleCollection;
					roleCount = factRoles.Count;
					Role[] resolvedRoles = new Role[roleCount];
					for (int i = 0; i < roleCount; ++i)
					{
						resolvedRoles[i] = factRoles[i].Role;
					}
					roles = resolvedRoles;
				}
				else
				{
					if (!ignoreObjectification)
					{
						objectifiedFactType = parentObjectType.NestedFactType;
					}
					UniquenessConstraint identifier = parentObjectType.ResolvedPreferredIdentifier;
					ObjectType preferredFor = (identifier != null) ? identifier.PreferredIdentifierFor : null;
					if (objectifiedFactType != null)
					{
						if (preferredFor == parentObjectType && identifier.IsObjectifiedPreferredIdentifier)
						{
							// The entity is represented by the complete ordered FactType tuple, not just
							// the identifier.
							return RecurseObjectTypeInstanceValue(
								null,
								null,
								false,
								false,
								(objectInstance != null) ? objectInstance.ObjectifiedInstance : null,
								objectifiedFactType,
								ref listSeparator,
								ref outputText,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								outerGrouping);
						}
						else if (preferredFor != null && preferredFor != parentObjectType)
						{
							// The subtype is associated with an objectified facttype. Show
							// the identifier from the supertype with the objectified FactType
							if (outputText == null)
							{
								outputText = new StringBuilder();
							}
							if (!outerGrouping)
							{
								outputText.Append("(");
							}
							EntityTypeSubtypeInstance subtypeInstance = objectInstance as EntityTypeSubtypeInstance;
							RecurseObjectTypeInstanceValue(
								(subtypeInstance != null) ? subtypeInstance.SupertypeInstance : null,
								preferredFor,
								false,
								true,
								null,
								null,
								ref listSeparator,
								ref outputText,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								false);
							outputText.Append(listSeparator ?? (listSeparator = GetListSeparator(formatProvider)));
							RecurseObjectTypeInstanceValue(
								null,
								null,
								false,
								false,
								(objectInstance != null) ? objectInstance.ObjectifiedInstance : null,
								objectifiedFactType,
								ref listSeparator,
								ref outputText,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								true);
							if (!outerGrouping)
							{
								outputText.Append(")");
							}
							return null;
						}
					}
					if (preferredFor != null && preferredFor != parentObjectType)
					{
						EntityTypeSubtypeInstance subtypeInstance = objectInstance as EntityTypeSubtypeInstance;
						return RecurseObjectTypeInstanceValue(
							(subtypeInstance != null) ? subtypeInstance.SupertypeInstance : null,
							preferredFor,
							false,
							nestedLeadValue,
							null,
							null,
							ref listSeparator,
							ref outputText,
							formatProvider,
							valueTextFormat,
							valueNonTextFormat,
							outerGrouping);
					}
					entityInstance = objectInstance as EntityTypeInstance;
					if (identifier != null)
					{
						roles = identifier.RoleCollection;
						roleCount = roles.Count;
					}
					else if (objectifiedFactType == null)
					{
						if (outputText != null)
						{
							outputText.Append(blankValueText);
							return null;
						}
						return blankValueText;
					}
				}
				if (objectifiedFactType != null)
				{
					if (outputText == null)
					{
						outputText = new StringBuilder();
					}
					if (!outerGrouping)
					{
						outputText.Append("(");
						nestedLeadValue = true;
						blankValueText = " ";
					}
				}
				if (roles == null)
				{
					// Note non-objectified case handled earlier
					outputText.Append(blankValueText);
				}
				else
				{
					if (roleCount == 1 && (factInstance == null || !outerGrouping)) // Always parenthesize a fact tuple, even a unary
					{
						Role role = roles[0];
						RoleInstance roleInstance = null;
						if (entityInstance != null)
						{
							roleInstance = entityInstance.FindRoleInstance(role);
						}
						else if (factInstance != null)
						{
							roleInstance = factInstance.FindRoleInstance(role);
						}
						string retVal = RecurseObjectTypeInstanceValue(
							(roleInstance != null) ? roleInstance.ObjectTypeInstance : null,
							role.RolePlayer,
							false,
							nestedLeadValue,
							null,
							null,
							ref listSeparator,
							ref outputText,
							formatProvider,
							valueTextFormat,
							valueNonTextFormat,
							false);
						if (objectifiedFactType == null)
						{
							return retVal;
						}
					}
					else
					{
						LinkedElementCollection<EntityTypeRoleInstance> entityRoleInstances = null;
						LinkedElementCollection<FactTypeRoleInstance> factRoleInstances = null;
						int roleInstanceCount = 0;
						if (entityInstance != null)
						{
							entityRoleInstances = entityInstance.RoleInstanceCollection;
							roleInstanceCount = entityRoleInstances.Count;
						}
						else if (factInstance != null)
						{
							factRoleInstances = factInstance.RoleInstanceCollection;
							roleInstanceCount = factRoleInstances.Count;
						}
						if (outputText == null)
						{
							outputText = new StringBuilder();
						}
						if (!outerGrouping)
						{
							outputText.Append("(");
						}
						if (listSeparator == null)
						{
							listSeparator = GetListSeparator(formatProvider);
						}
						for (int i = 0; i < roleCount; ++i)
						{
							Role role = roles[i];
							RoleInstance matchInstance = null;
							if (i != 0)
							{
								outputText.Append(listSeparator);
							}
							if (roleInstanceCount != 0)
							{
								if (entityInstance != null)
								{
									for (int j = 0; j < roleInstanceCount; ++j)
									{
										EntityTypeRoleInstance instance = entityRoleInstances[j];
										if (instance.Role == role)
										{
											matchInstance = instance;
											break;
										}
									}
								}
								else if (factInstance != null)
								{
									for (int j = 0; j < roleInstanceCount; ++j)
									{
										FactTypeRoleInstance instance = factRoleInstances[j];
										if (instance.Role == role)
										{
											matchInstance = instance;
											break;
										}
									}
								}
							}
							RecurseObjectTypeInstanceValue(
								(matchInstance != null) ? matchInstance.ObjectTypeInstance : null,
								role.RolePlayer,
								false,
								i == 0 && !outerGrouping,
								null,
								null,
								ref listSeparator,
								ref outputText,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								false);
						}
						if (!outerGrouping)
						{
							outputText.Append(")");
						}
					}
				}
				if (objectifiedFactType != null)
				{
					outputText.Append(listSeparator ?? (listSeparator = GetListSeparator(formatProvider)));
					RecurseObjectTypeInstanceValue(
						null,
						null,
						false,
						false,
						(entityInstance != null) ? entityInstance.ObjectifiedInstance : null,
						objectifiedFactType,
						ref listSeparator,
						ref outputText,
						formatProvider,
						valueTextFormat,
						valueNonTextFormat,
						true);
					if (!outerGrouping)
					{
						outputText.Append(")");
					}
				}
				return null;
			}
		}
		private static string GetListSeparator(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
			return cultureInfo.TextInfo.ListSeparator + " ";
		}
		#endregion
		#region PopulationMandatoryError Validation
		/// <summary>
		/// Validator callback for PopulationMandatoryError
		/// <remarks>DelayValidatePriority is set to run after implied mandatory constraint creation</remarks>
		/// </summary>
		[DelayValidatePriority(4)]
		protected static void DelayValidateInstancePopulationMandatoryError(ModelElement element)
		{
			(element as ObjectTypeInstance).ValidateInstancePopulationMandatoryError(null);
		}
		#region InstanceTyper struct
		/// <summary>
		/// Helper struct to get a typed version of a given instance.
		/// </summary>
		private struct InstanceTyper
		{
			private readonly ObjectTypeInstance myInstance;
			private readonly ObjectType myType;
			private bool myInitialized;
			private EntityTypeInstance mySupertypeInstance;
			private ObjectType mySupertypeType;
			private LinkedElementCollection<EntityTypeSubtypeInstance> mySubtypeInstances;
			/// <summary>
			/// Create a new instance typer for the given instance.
			/// </summary>
			/// <param name="instance">The instance to base other types on.</param>
			public InstanceTyper(ObjectTypeInstance instance)
			{
				myInstance = instance;
				myType = instance.ObjectType;
				myInitialized = false;
				mySupertypeType = null;
				mySupertypeInstance = null;
				mySubtypeInstances = null;
			}
			/// <summary>
			/// Get the related instance of the given type, if available.
			/// </summary>
			public ObjectTypeInstance TypedInstance(ObjectType type)
			{
				ObjectTypeInstance instance = myInstance;
				if (myType == type)
				{
					return instance;
				}
				if (!myInitialized)
				{
					myInitialized = true;
					EntityTypeInstance entityInstance;
					EntityTypeSubtypeInstance subtypeInstance;
					if (null != (entityInstance = instance as EntityTypeInstance))
					{
						mySupertypeInstance = entityInstance;
						mySupertypeType = entityInstance.ObjectType;
						mySubtypeInstances = entityInstance.EntityTypeSubtypeInstanceCollection;
					}
					else if (null != (subtypeInstance = instance as EntityTypeSubtypeInstance))
					{
						mySupertypeInstance = entityInstance = subtypeInstance.SupertypeInstance;
						mySupertypeType = entityInstance.ObjectType;
						mySubtypeInstances = entityInstance.EntityTypeSubtypeInstanceCollection;
					}
					if (mySubtypeInstances != null &&
						mySubtypeInstances.Count == 0)
					{
						mySubtypeInstances = null;
					}
				}
				if (mySubtypeInstances != null)
				{
					if (mySupertypeType == type)
					{
						return mySupertypeInstance;
					}
					foreach (EntityTypeSubtypeInstance subtypeInstance in mySubtypeInstances)
					{
						if (subtypeInstance.EntityTypeSubtype == type)
						{
							return subtypeInstance;
						}
					}
				}
				return null;
			}
			/// <summary>
			/// Test if two instances represent the same instance, including different types
			/// of the same instance.
			/// </summary>
			public static bool RepresentsSameInstance(ObjectTypeInstance instance1, ObjectTypeInstance instance2)
			{
				return NormalizeInstance(instance1) == NormalizeInstance(instance2);
			}
			private static ObjectTypeInstance NormalizeInstance(ObjectTypeInstance instance)
			{
				EntityTypeSubtypeInstance subtypeInstance;
				return (null != (subtypeInstance = instance as EntityTypeSubtypeInstance)) ?
					subtypeInstance.SupertypeInstance :
					instance;
			}
		}
		#endregion // InstanceTyper struct
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
					InstanceTyper instanceTyper = new InstanceTyper(this);
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
							bool impliedObjectificationRole = false;
							switch (currentRole.GetReferenceSchemePattern(out identifiedEntityType))
							{
								//case ReferenceSchemeRolePattern.None:
								//    break;
								case ReferenceSchemeRolePattern.OptionalSimpleIdentifiedRole:
									// This one is tricky. The implied FactTypeRoleInstance population
									// for the opposite (identifier) role is based on the set of EntityTypeInstances
									// on this role. Because the opposite role is optional, the population
									// for this role is not automatically filled in, so it is possible to
									// have PopulationMandatoryErrors on any disjunctive mandatory constraint
									// intersecting the opposite role.
									// Note that we always have an opposite role. Otherwise, the reference scheme
									// pattern would not hold.
									if (notifyAdded == null) // The opposite instance will be validated during load, no need to repeat
									{
										foreach (ConstraintRoleSequence sequence in currentRole.OppositeRole.Role.ConstraintRoleSequenceCollection)
										{
											MandatoryConstraint constraint = sequence as MandatoryConstraint;
											if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
											{
												if (!retrievedIdentifyingInstance)
												{
													retrievedIdentifyingInstance = true;
													EntityTypeInstance entityInstance;
													if (null != (entityInstance = this as EntityTypeInstance))
													{
														LinkedElementCollection<EntityTypeRoleInstance> identifyingInstances = entityInstance.RoleInstanceCollection;
														if (identifyingInstances.Count == 1)
														{
															identifyingInstance = identifyingInstances[0].ObjectTypeInstance;
														}
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
													if (InstanceTyper.RepresentsSameInstance(error.ObjectTypeInstance, identifyingInstance))
													{
														error.Delete();
													}
												}
											}
										}
									}
									continue;
								//case ReferenceSchemeRolePattern.MandatoryCompositeIdentifierRole:
								//    // This is a special case because the instance must be represented
								//    // in a population, but the population we're concerned about are the
								//    // entity instances of the opposite role, not the implied FactTypeInstances.
								//    // However, because these instances are part of the ObjectTypeInstanceCollection
								//    // on the role, there is no special handling needed.
								//    continue;
								case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
									// Synchronize the EntityTypeInstances on the opposite role with the identifying object
									// Note that this will trigger the MandatoryIdentifiedRole case on another call.
									EnsureImpliedEntityTypeInstance(this, identifiedEntityType, currentRole);
									continue;
								case ReferenceSchemeRolePattern.CompositeIdentifiedRole:
									// Nothing to do here. These are populated manually because the composite
									// nature means the tuples cannot be populated automatically.
								case ReferenceSchemeRolePattern.MandatorySimpleIdentifiedRole:
									// Nothing to do here. These are populated automatically when instances
									// are added to the opposite role.
									continue;
								//case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
								//case ReferenceSchemeRolePattern.OptionalCompositeIdentifierRole:
								//    // Evaluate these. Although the population of the FactTypeInstance
								//    // is implied, the role should set get a population mandatory error
								//    // if an instance is not used by another entity instance.
								//    break;
								case ReferenceSchemeRolePattern.ImpliedObjectificationRole:
									impliedObjectificationRole = true;
									break;
							}
							SubtypeMetaRole subtypeRole;
							bool impliedRolePopulation = impliedObjectificationRole || ((null != (subtypeRole = currentRole as SubtypeMetaRole)) && this is EntityTypeSubtypeInstance && ((SubtypeFact)subtypeRole.FactType).ProvidesPreferredIdentifier);
							foreach (ConstraintRoleSequence sequence in currentRole.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
								{
									bool hasError = false;
									ObjectTypeInstance typedInstance = null;
									if (!impliedRolePopulation)
									{
										LinkedElementCollection<Role> constraintRoles = constraint.RoleCollection;
										ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(constraintRoles, false);
										while (compatibleTypes.Length > 1)
										{
											compatibleTypes = ObjectType.GetNearestCompatibleTypes(compatibleTypes, false);
										}
										if (compatibleTypes.Length == 0)
										{
											// Don't show population errors if there are compatible type problems
											constraint.PopulationMandatoryErrorCollection.Clear();
											continue;
										}
										ObjectType trackedInstanceType = compatibleTypes[0];
										typedInstance = this;
										if (trackedInstanceType != objectType)
										{
											if (null == (typedInstance = instanceTyper.TypedInstance(trackedInstanceType)))
											{
												errors.Clear();
												continue;
											}
										}
										int constraintRoleCount = constraintRoles.Count;
										int j = 0;
										if (!trackedInstanceType.IsImplicitBooleanValue)
										{
											for (; j < constraintRoleCount; ++j)
											{
												Role constraintRole = constraintRoles[j];
												SupertypeMetaRole supertypeRole = constraintRole as SupertypeMetaRole;
												if (supertypeRole != null && ((SubtypeFact)supertypeRole.FactType).ProvidesPreferredIdentifier)
												{
													if (null != (instanceTyper.TypedInstance(supertypeRole.OppositeRole.Role.RolePlayer)))
													{
														break;
													}
												}
												else
												{
													ReadOnlyLinkedElementCollection<ObjectTypeInstance> roleInstances;
													ObjectTypeInstance findInstance;
													ObjectType roleType;
													if (currentRole == constraintRole)
													{
														if (currentRoleInstances == null)
														{
															currentRoleInstances = currentRole.ObjectTypeInstanceCollection;
														}
														roleInstances = currentRoleInstances;
														roleType = objectType;
													}
													else
													{
														roleInstances = constraintRole.ObjectTypeInstanceCollection;
														roleType = constraintRole.RolePlayer;
													}
													if (null != (findInstance = ((trackedInstanceType == roleType) ? typedInstance : instanceTyper.TypedInstance(roleType))) &&
														roleInstances.Contains(findInstance))
													{
														break;
													}
												}
											}
										}
										hasError = j == constraintRoleCount;
									}
									if (hasError)
									{
										bool createError = true;
										if (typedInstance == this)
										{
											// Make sure we have an error
											foreach (PopulationMandatoryError error in errors)
											{
												if (error.MandatoryConstraint == constraint)
												{
													createError = false;
													break;
												}
											}
										}
										else
										{
											// Make sure there is no error on this instance
											foreach (PopulationMandatoryError error in errors)
											{
												if (error.MandatoryConstraint == constraint)
												{
													error.Delete();
													break;
												}
											}
											foreach (PopulationMandatoryError error in typedInstance.PopulationMandatoryErrorCollection)
											{
												if (error.MandatoryConstraint == constraint)
												{
													createError = false;
													break;
												}
											}
										}
										if (createError)
										{
											PopulationMandatoryError error = new PopulationMandatoryError(Partition);
											error.ObjectTypeInstance = typedInstance;
											error.MandatoryConstraint = constraint;
											error.Model = constraint.ResolvedModel;
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
										foreach (PopulationMandatoryError error in errors)
										{
											if (error.MandatoryConstraint == constraint)
											{
												error.Delete();
												break;
											}
										}
										if (typedInstance != null &&
											typedInstance != this)
										{
											foreach (PopulationMandatoryError error in typedInstance.PopulationMandatoryErrorCollection)
											{
												if (error.MandatoryConstraint == constraint)
												{
													error.Delete();
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
		}
		/// <summary>
		/// Make sure there is an <see cref="EntityTypeInstance"/> associated with the
		/// <paramref name="identifiedEntityType"/> for each instance associated with the role player
		/// of <paramref name="identifierRole"/>.
		/// </summary>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="identifiedEntityType"/></param>
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
		/// <paramref name="identifiedEntityType"/> that references the specified <paramref name="instance"/>
		/// through the provided <paramref name="identifierRole"/>. Used to create consistent
		/// implicit populations for any <see cref="FactType"/> matching the reference scheme
		/// pattern.
		/// </summary>
		/// <param name="instance">The <see cref="ObjectTypeInstance"/> from the identifier role</param>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="identifiedEntityType"/></param>
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
				EntityTypeInstance newInstance = new EntityTypeInstance(identifiedEntityType.Partition);
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
							if (InstanceTyper.RepresentsSameInstance(error.ObjectTypeInstance, objectTypeInstance))
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
		/// <remarks>Protected so that this can be triggered from subtypes.</remarks>
		[DelayValidatePriority(4)]
		protected static void DelayValidateRolePopulationMandatoryError(ModelElement element)
		{
			Role role = (Role)element;
			if (!role.IsDeleted)
			{
				ObjectType rolePlayer;
				if (null != (rolePlayer = role.RolePlayer))
				{
					ObjectType identifiedEntityType;
					switch (role.GetReferenceSchemePattern(out identifiedEntityType))
					{
						case ReferenceSchemeRolePattern.None:
						case ReferenceSchemeRolePattern.MandatoryCompositeIdentifierRole:
						case ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole:
						case ReferenceSchemeRolePattern.OptionalCompositeIdentifierRole:
							{
								int instanceCount = 0;
								ObjectTypeInstance[] instances = null;
								ObjectType instancesOfType = null;
								BitTracker seenInstances = default(BitTracker);
								MandatoryConstraint constraint;
								IComparer<ObjectTypeInstance> comparer = ModelElementIdComparer<ObjectTypeInstance>.Instance;
								SubtypeMetaRole subtypeRole;
								bool impliedRolePopulation = (null != (subtypeRole = role as SubtypeMetaRole)) && ((SubtypeFact)subtypeRole.FactType).ProvidesPreferredIdentifier;
								foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
								{
									constraint = sequence as MandatoryConstraint;
									if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
									{
										if (impliedRolePopulation)
										{
											constraint.PopulationMandatoryErrorCollection.Clear();
											continue;
										}
										LinkedElementCollection<Role> constraintRoles = sequence.RoleCollection;
										ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(constraintRoles, false);
										while (compatibleTypes.Length > 1)
										{
											compatibleTypes = ObjectType.GetNearestCompatibleTypes(compatibleTypes, false);
										}
										if (compatibleTypes.Length == 0)
										{
											// Don't show population errors if there are compatible type problems
											constraint.PopulationMandatoryErrorCollection.Clear();
											continue;
										}
										ObjectType trackedInstanceType = compatibleTypes[0];
										int seenInstanceCount = 0;
										int constraintRoleCount = 0;
										if (!trackedInstanceType.IsImplicitBooleanValue)
										{
											// Get repeated stuff once
											if (instancesOfType != trackedInstanceType)
											{
												instancesOfType = trackedInstanceType;
												LinkedElementCollection<ObjectTypeInstance> instancesCollection = trackedInstanceType.ObjectTypeInstanceCollection;
												if (0 == (instanceCount = instancesCollection.Count))
												{
													instances = null;
													continue;
												}
												instances = instancesCollection.ToArray();
												Array.Sort<ObjectTypeInstance>(instances, comparer);
												seenInstances = new BitTracker(instanceCount);
											}
											else if (instances == null)
											{
												continue;
											}
											else
											{
												seenInstances.Reset();
											}

											// Intersect each role with the instances on the compatible role player.
											// Note that we do not report population mandatory errors on constraints with
											// incompatible roles.
											constraintRoleCount = constraintRoles.Count;
											for (int i = 0; i < constraintRoleCount && seenInstanceCount < instanceCount; ++i)
											{
												Role currentRole = constraintRoles[i];
												SupertypeMetaRole supertypeRole = currentRole as SupertypeMetaRole;
												if (supertypeRole != null && ((SubtypeFact)supertypeRole.FactType).ProvidesPreferredIdentifier)
												{
													foreach (EntityTypeSubtypeInstance subtypeInstance in supertypeRole.OppositeRole.Role.RolePlayer.EntityTypeSubtypeInstanceCollection)
													{
														InstanceTyper instanceTyper = new InstanceTyper(subtypeInstance);
														ObjectTypeInstance findInstance;
														int index;
														if (null != (findInstance = instanceTyper.TypedInstance(trackedInstanceType)) &&
															0 <= (index = Array.BinarySearch<ObjectTypeInstance>(instances, findInstance, comparer)) &&
															!seenInstances[index])
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
												else
												{
													ReadOnlyLinkedElementCollection<ObjectTypeInstance> roleInstances;
													ObjectType differentRolePlayer;
													roleInstances = currentRole.ObjectTypeInstanceCollection;
													if (trackedInstanceType == (differentRolePlayer = currentRole.RolePlayer))
													{
														differentRolePlayer = null;
													}
													int roleInstanceCount = roleInstances.Count;
													for (int j = 0; j < roleInstanceCount; ++j)
													{
														ObjectTypeInstance findInstance = roleInstances[j];
														if (differentRolePlayer != null)
														{
															findInstance = new InstanceTyper(findInstance).TypedInstance(trackedInstanceType);
														}
														int index;
														if (findInstance != null &&
															0 <= (index = Array.BinarySearch<ObjectTypeInstance>(instances, findInstance, comparer)) &&
															!seenInstances[index])
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
											}
										}

										// We now have all instances that are covered and not covered. Synchronize
										// the error collection on the mandatory constraint
										LinkedElementCollection<PopulationMandatoryError> errors = constraint.PopulationMandatoryErrorCollection;
										int errorCount = errors.Count;
										if (seenInstanceCount == instanceCount)
										{
											errors.Clear();
										}
										else
										{
											// Remove errors we no longer need
											for (int i = errorCount - 1; i >= 0; --i)
											{
												PopulationMandatoryError error = errors[i];
												ObjectTypeInstance previousErrorInstance = error.ObjectTypeInstance;
												int index;
												if (previousErrorInstance.ObjectType != trackedInstanceType)
												{
													// We only report errors on the nearest shared compatible type.
													// Clear any errors that are not involved with this type.
													error.Delete();
												}
												else if (0 <= (index = Array.BinarySearch<ObjectTypeInstance>(instances, previousErrorInstance, comparer)))
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
													PopulationMandatoryError error = new PopulationMandatoryError(role.Partition);
													error.ObjectTypeInstance = instances[i];
													error.MandatoryConstraint = constraint;
													error.Model = constraint.ResolvedModel;
													error.GenerateErrorText();
												}
											}
										}
									}
								}
							}
							break;
						case ReferenceSchemeRolePattern.OptionalSimpleIdentifiedRole:
							// This one is tricky. The implied FactTypeRoleInstance population
							// for the opposite (identifier) role is based on the set of EntityTypeInstances
							// on this role. Because the opposite role is optional, the population
							// for this role is not automatically filled in, so it is possible to
							// have PopulationMandatoryErrors on any disjunctive mandatory constraint
							// intersecting the opposite role.
							// Note that we always have an opposite role. Otherwise, the reference scheme
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
										// This is a single role uniqueness. There is no need to resolve the supertype
										// for the constraint or check subtypes.
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
						case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
							// Full population is implied, clear any population mandatory errors and synchronize the sets
							EnsureImpliedEntityTypeInstances(identifiedEntityType, role);
							break;
						case ReferenceSchemeRolePattern.ImpliedObjectificationRole:
							// Nothing to do here, the implied population corresponds to the
							// FactTypeInstance and EntityTypeInstance population, which are correlated
							// with ObjectificationInstance relationships.
						case ReferenceSchemeRolePattern.CompositeIdentifiedRole:
							// Nothing to do here. These are populated manually because the composite
							// nature means the tuples cannot be populated automatically.
						case ReferenceSchemeRolePattern.MandatorySimpleIdentifiedRole:
							// Nothing to do here. These are populated automatically when instances
							// are added to the opposite role.
							break;
					}
				}
			}
		}
		#endregion // PopulationMandatoryError Validation
		#region ObjectifiedInstanceRequiredError Validation
		/// <summary>
		/// Validation callback for <see cref="ObjectifiedInstanceRequiredError"/>
		/// </summary>
		internal static void DelayValidateObjectifiedInstanceRequiredError(ModelElement element)
		{
			// Internal justification: See comments in FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError
			((ObjectTypeInstance)element).ValidateObjectifiedInstanceRequiredError(null);
		}
		private void ValidateObjectifiedInstanceRequiredError(INotifyElementAdded notifyAdded)
		{
			if (!this.IsDeleted)
			{
				FactType factType;
				ObjectType entityType;
				ObjectifiedInstanceRequiredError error = this.ObjectifiedInstanceRequiredError;
				if (null != (entityType = this.ObjectType) &&
					null != (factType = entityType.NestedFactType) &&
					null == this.ObjectifiedInstance &&
					null != entityType.ResolvedPreferredIdentifier)
				{
					if (error == null)
					{
						error = new ObjectifiedInstanceRequiredError(Partition);
						error.ObjectTypeInstance = this;
						error.Model = entityType.ResolvedModel;
						error.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(error, true);
						}
					}
				}
				else if (error != null)
				{
					error.Delete();
				}
			}
		}
		#endregion // ObjectifiedInstanceRequiredError Validation
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
							FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
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
							FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
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
		/// AddRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntityTypeSupertypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ModelElement;
			EntityTypeSubtypeInstance subtypeInstance = link.EntityTypeSubtypeInstance;
			ObjectType subtype;
			if (null != (subtype = subtypeInstance.EntityTypeSubtype))
			{
				ValidateAttachedSubtypeInstance(subtype, subtypeInstance, link.SupertypeInstance);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntityTypeSupertypeInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			EntityTypeInstance supertypeInstance = ((EntityTypeSubtypeInstanceHasSupertypeInstance)e.ModelElement).SupertypeInstance;
			if (!supertypeInstance.IsDeleted)
			{
				supertypeInstance.DelayValidateIfEmpty();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntityTypeSupertypeInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == EntityTypeSubtypeInstanceHasSupertypeInstance.EntityTypeSubtypeInstanceDomainRoleId)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeSubtypeInstanceEnforceInitialSubtypeInstance);
			}
			EntityTypeSubtypeInstanceHasSupertypeInstance link = (EntityTypeSubtypeInstanceHasSupertypeInstance)e.ElementLink;
			EntityTypeSubtypeInstance subtypeInstance = link.EntityTypeSubtypeInstance;
			ObjectType subtype;
			if (null != (subtype = subtypeInstance.EntityTypeSubtype))
			{
				ValidateAttachedSubtypeInstance(subtype, subtypeInstance, link.SupertypeInstance);

				EntityTypeInstance detachedSupertypeInstance = (EntityTypeInstance)e.OldRolePlayer;
				detachedSupertypeInstance.DelayValidateIfEmpty();
				ObjectType resolvedSupertype;
				if (null != (resolvedSupertype = detachedSupertypeInstance.ObjectType))
				{
					// See if the supertype is a direct supertype or if there are indirect
					// stages, in which case we treat this as a use of the indirect supertypes,
					// not the identifying supertype.
					LinkedElementCollection<EntityTypeSubtypeInstance> relatedDetachedSubtypeInstances = null;
					int relatedDetachedSubtypeInstancesCount = 0;
					ObjectType.WalkSupertypes(
						subtype,
						delegate(ObjectType directSupertype, int depth, bool isPrimary)
						{
							if (depth == 0)
							{
								return ObjectTypeVisitorResult.Continue;
							}
							Debug.Assert(depth == 1);
							if (isPrimary)
							{
								if (directSupertype == resolvedSupertype)
								{
									FrameworkDomainModel.DelayValidateElement(detachedSupertypeInstance, DelayValidateInstancePopulationMandatoryError);
									// Note that we should only satisfy this condition once if the subtype
									// graph is intransitive, so we could break here. However, we
									// are not in any position at this point to assume graph intransitivity
									// so we do not break the loop.
								}
								else
								{
									if (relatedDetachedSubtypeInstances == null)
									{
										relatedDetachedSubtypeInstances = detachedSupertypeInstance.EntityTypeSubtypeInstanceCollection;
										relatedDetachedSubtypeInstancesCount = relatedDetachedSubtypeInstances.Count;
									}
									for (int i = 0; i < relatedDetachedSubtypeInstancesCount; ++i)
									{
										EntityTypeSubtypeInstance instance = relatedDetachedSubtypeInstances[i];
										if (instance.ObjectType == directSupertype)
										{
											FrameworkDomainModel.DelayValidateElement(instance, DelayValidateInstancePopulationMandatoryError);
										}
									}
								}
							}
							return ObjectTypeVisitorResult.SkipChildren;
						});
					// Downstream subtype instances may still require the old instance, revalidate them.
					// CONSIDER: This will produce odd results if a roleplayer is changed in the middle
					// of a subtype graph because the old instance will reappear. The other alternatives
					// are to change the role players on all downstream instances (potentially requiring
					// new instances to be created up different branches of the graph), delete the downstream
					// instances, or to throw if the user attempts this operation.
					if (relatedDetachedSubtypeInstances == null ||
						relatedDetachedSubtypeInstancesCount != 0)
					{
						ObjectType.WalkSubtypes(
							subtype,
							delegate(ObjectType subsubtype, int depth, bool isPrimary)
							{
								if (depth == 0)
								{
									return ObjectTypeVisitorResult.Continue;
								}
								if (isPrimary)
								{
									if (relatedDetachedSubtypeInstances == null)
									{
										relatedDetachedSubtypeInstances = detachedSupertypeInstance.EntityTypeSubtypeInstanceCollection;
										relatedDetachedSubtypeInstancesCount = relatedDetachedSubtypeInstances.Count;
										if (relatedDetachedSubtypeInstancesCount == 0)
										{
											// Nothing left to do
											return ObjectTypeVisitorResult.Stop;
										}
									}
									for (int i = 0; i < relatedDetachedSubtypeInstancesCount; ++i)
									{
										EntityTypeSubtypeInstance otherInstance = relatedDetachedSubtypeInstances[i];
										if (otherInstance.EntityTypeSubtype == subsubtype)
										{
											FrameworkDomainModel.DelayValidateElement(otherInstance, DelayValidateSubtypeInstance);
										}
									}
									// We do not need to continue here, the downstream instances remain in a valid state
								}
								return ObjectTypeVisitorResult.SkipChildren;
							});
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeSubtypeHasEntityTypeSubtypeInstance)
		/// </summary>
		private static void EntityTypeSubtypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntityTypeSubtypeHasEntityTypeSubtypeInstance link = (EntityTypeSubtypeHasEntityTypeSubtypeInstance)e.ModelElement;
			EntityTypeSubtypeInstance subtypeInstance = link.EntityTypeSubtypeInstance;
			EntityTypeInstance supertypeInstance;
			if (null != (supertypeInstance = subtypeInstance.SupertypeInstance))
			{
				ValidateAttachedSubtypeInstance(link.EntityTypeSubtype, subtypeInstance, supertypeInstance);
			}
		}
		/// <summary>
		/// An <see cref="EntityTypeSubtypeInstance"/> is not valid until it is attached to
		/// both its supertype instance and the subtype. This validation helper runs when
		/// both relationships have been established, which is assumed to happen before the
		/// end of a transaction.
		/// Verify that the specified a <paramref name="subtypeInstance"/> is the only instance of its <paramref name="subtype"/>
		/// with the given <paramref name="supertypeInstance"/>
		/// </summary>
		/// <exception cref="InvalidOperationException">For a given supertype instance a corresponding subtype instance must
		/// not be specified more than once per subtype.</exception>
		private static void ValidateAttachedSubtypeInstance(ObjectType subtype, EntityTypeSubtypeInstance subtypeInstance, EntityTypeInstance supertypeInstance)
		{
			// Verify that the supertype instance is associated with only one EntityTypeSubtypeInstance
			// on the subtype. Note that the size of the supertypeInstance.EntityTypeSubtypeInstanceCollection
			// collection is limited by the total number of subtypes, whereas the subtype.EntityTypeSubtypeInstanceCollection
			// is unbounded. We walk the smaller collection.
			foreach (EntityTypeSubtypeInstance testInstance in supertypeInstance.EntityTypeSubtypeInstanceCollection)
			{
				if (testInstance != subtypeInstance && testInstance.ObjectType == subtype)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeSubtypeInstanceDuplicateSupertypeInstance);
				}
			}

			// Verify that intermediate subtypes have a corresponding instance collection and are
			// otherwise well formed.
			FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateSubtypeInstance);

			// Provide event listeners with a single type of event when the instance name changes
			FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateNamePartChanged);

			// Verify population mandatory issues
			FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateInstancePopulationMandatoryError);
			ObjectType resolvedSupertype;
			if (null != (resolvedSupertype = supertypeInstance.ObjectType))
			{
				// See if the supertype is a direct supertype or if there are indirect
				// stages, in which case we treat this as a use of the indirect supertypes,
				// not the identifying supertype.
				LinkedElementCollection<EntityTypeSubtypeInstance> relatedSubtypeInstances = null;
				ObjectType.WalkSupertypes(
					subtype,
					delegate(ObjectType directSupertype, int depth, bool isPrimary)
					{
						if (depth == 0)
						{
							return ObjectTypeVisitorResult.Continue;
						}
						Debug.Assert(depth == 1);
						if (isPrimary)
						{
							if (directSupertype == resolvedSupertype)
							{
								FrameworkDomainModel.DelayValidateElement(supertypeInstance, DelayValidateInstancePopulationMandatoryError);
								// Note that we should only satisfy this condition once if the subtype
								// graph is intransitive, so we could break here. However, we
								// are not in any position at this point to assume graph intransitivity
								// so we do not break the loop.
							}
							else
							{
								if (relatedSubtypeInstances == null)
								{
									relatedSubtypeInstances = supertypeInstance.EntityTypeSubtypeInstanceCollection;
								}
								foreach (EntityTypeSubtypeInstance instance in relatedSubtypeInstances)
								{
									if (instance != subtypeInstance && instance.ObjectType == directSupertype)
									{
										FrameworkDomainModel.DelayValidateElement(instance, DelayValidateInstancePopulationMandatoryError);
									}
								}
							}
						}
						return ObjectTypeVisitorResult.SkipChildren;
					});
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeInstanceHasRoleInstance)
		/// Block role instance from moving to different instance
		/// </summary>
		private static void EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionEntityTypeInstanceEnforceFixedRoleInstance);
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
			RoleInstance roleInstance = (RoleInstance)e.ModelElement;
			if (CopyMergeUtility.GetIntegrationPhase(roleInstance.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				ObjectType rolePlayer = roleInstance.Role.RolePlayer;
				ObjectTypeInstance instance;
				if (rolePlayer != null && rolePlayer != (instance = roleInstance.ObjectTypeInstance).ObjectType)
				{
					// Note that this will throw in ObjectTypeInstanceRolePlayerChangedRule if the current ObjectType is not null
					instance.ObjectType = rolePlayer;
				}
			}
			FrameworkDomainModel.DelayValidateElement(roleInstance, DelayValidateRemovePopulationMandatoryError);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleInstance)
		/// </summary>
		private static void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (Utility.IsDescendantOrSelf(e.DomainRole, RoleInstance.RoleDomainRoleId))
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionRoleInstanceEnforceInitialRole);
			}
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
			Debug.Fail("A DSLTools framework bug is preventing Undo of ObjectTypeInstance role player changes with duplicate role/instance pairs, do not call until this is resolved.");
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			RoleInstance roleInstance = (RoleInstance)e.ElementLink;
			ObjectType rolePlayer = roleInstance.Role.RolePlayer;
			ObjectTypeInstance instance = roleInstance.ObjectTypeInstance;
			if (rolePlayer != null &&
				rolePlayer != instance.ObjectType &&
				CopyMergeUtility.GetIntegrationPhase(roleInstance.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				// Note that this will throw in ObjectTypeInstanceRolePlayerChangedRule the current ObjectType is not null
				instance.ObjectType = rolePlayer;
			}
			FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
			FrameworkDomainModel.DelayValidateElement(roleInstance, DelayValidateRemovePopulationMandatoryError);
			instance = (ObjectTypeInstance)e.OldRolePlayer;
			instance.DelayValidateIfEmpty();
			FrameworkDomainModel.DelayValidateElement(instance, DelayValidateInstancePopulationMandatoryError);
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
			ObjectTypeInstance objectInstance = roleInstance.ObjectTypeInstance;
			if (!objectInstance.IsDeleted)
			{
				objectInstance.DelayValidateIfEmpty();
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
			if (!rolePlayer.IsDeleted)
			{
				if (!role.IsDeleted)
				{
					ReadOnlyCollection<RoleInstance> instances = RoleInstance.GetLinksToObjectTypeInstanceCollection(role);
					int instanceCount = instances.Count;
					for (int i = instanceCount - 1; i >= 0; --i)
					{
						Debug.Assert(instances[i].ObjectTypeInstance.ObjectType == rolePlayer);
						instances[i].Delete();
					}
				}
				if (role is SubtypeMetaRole)
				{
					FrameworkDomainModel.DelayValidateElement(rolePlayer, DelayValidateSubtypeInstances);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(SubtypeFact)
		/// Validate subtype instances when the preferred identification path is modified
		/// </summary>
		private static void PreferredSupertypeChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId)
			{
				SubtypeFact subtypeFact = (SubtypeFact)e.ModelElement;
				ObjectType subtype = subtypeFact.Subtype;
				if (subtype != null)
				{
					FrameworkDomainModel.DelayValidateElement(subtype, DelayValidateSubtypeInstances);
					if (!(bool)e.NewValue)
					{
						ObjectType.WalkSubtypes(
							subtype,
							delegate(ObjectType subsubtype, int depth, bool isPrimary)
							{
								if (depth == 0)
								{
									return ObjectTypeVisitorResult.Continue;
								}
								else if (isPrimary)
								{
									FrameworkDomainModel.DelayValidateElement(subsubtype, DelayValidateSubtypeInstances);
									return ObjectTypeVisitorResult.Continue;
								}
								return ObjectTypeVisitorResult.SkipChildren;
							});
					}
					else
					{
						// The FactType population is now implied by the EntityTypeSubtype instances of the subtype
						subtypeFact.FactTypeInstanceCollection.Clear();
					}
				}
			}
		}
		partial class EntityTypeSubtypeInstanceDeletingRuleClass
		{
			private bool myIsDisabled;
			/// <summary>
			/// DeletingRule: typeof(EntityTypeSubtypeInstance)
			/// When an EntityTypeSubtypeInstance is deleting, delete any subtype instances
			/// attached to the same supertype instance.
			/// </summary>
			private void EntityTypeSubtypeInstanceDeletingRule(ElementDeletingEventArgs e)
			{
				if (myIsDisabled)
				{
					return;
				}
				myIsDisabled = true;
				try
				{
					EntityTypeSubtypeInstance subtypeInstance = (EntityTypeSubtypeInstance)e.ModelElement;
					ObjectType subtype;
					EntityTypeInstance supertypeInstance;
					if (null != (subtype = subtypeInstance.EntityTypeSubtype) &&
						!subtype.IsDeleting &&
						null != (supertypeInstance = subtypeInstance.SupertypeInstance) &&
						!supertypeInstance.IsDeleting)
					{
						LinkedElementCollection<EntityTypeSubtypeInstance> allSubtypeInstances = null;
						int allSubtypeInstancesCount = 0;
						// In a deleting rule, we need to see the relationships so that we can test the
						// deleting state.
						ObjectType.WalkSubtypeRelationships(
							subtype,
							delegate(SubtypeFact subSubtypeFact, ObjectType subSubtype, int depth)
							{
								if (!subSubtypeFact.IsDeleting && !subSubtype.IsDeleting && subSubtypeFact.ProvidesPreferredIdentifier)
								{
									if (allSubtypeInstances == null)
									{
										allSubtypeInstances = supertypeInstance.EntityTypeSubtypeInstanceCollection;
										allSubtypeInstancesCount = allSubtypeInstances.Count;
										if (allSubtypeInstancesCount < 2)
										{
											// The only one left is the one currently being deleted for this notification
											return ObjectTypeVisitorResult.Stop;
										}
									}
									for (int i = allSubtypeInstancesCount - 1; i >= 0; --i)
									{
										EntityTypeSubtypeInstance otherInstance = allSubtypeInstances[i];
										if (!otherInstance.IsDeleting && otherInstance.EntityTypeSubtype == subSubtype)
										{
											// Look for another primary supertype that is not the current supertype
											otherInstance.Delete();
											if (--allSubtypeInstancesCount < 2)
											{
												// The only one left is the one currently being deleted for this method
												return ObjectTypeVisitorResult.Stop;
											}
										}
									}
									return ObjectTypeVisitorResult.Continue;
								}
								return ObjectTypeVisitorResult.SkipChildren;
							});

							// Now walk the other way to find subtype instances between this subtype and the
							// resolved supertype.
							ObjectType resolvedSupertype = supertypeInstance.ObjectType;
							ObjectType.WalkSupertypeRelationships(
								subtype,
								delegate(SubtypeFact subSuperttypeFact, ObjectType subSupertype, int depth)
								{
									if (!subSuperttypeFact.IsDeleting && !subSupertype.IsDeleting && subSuperttypeFact.ProvidesPreferredIdentifier)
									{
										if (subSupertype == resolvedSupertype)
										{
											FrameworkDomainModel.DelayValidateElement(supertypeInstance, DelayValidateInstancePopulationMandatoryError);
											// Note that we should only satisfy this condition once if the subtype
											// graph is intransitive, so we could break here. However, we
											// are not in any position at this point to assume graph intransitivity
											// so we do not break the loop.
										}
										else
										{
											if (allSubtypeInstances == null)
											{
												allSubtypeInstances = supertypeInstance.EntityTypeSubtypeInstanceCollection;
												allSubtypeInstancesCount = allSubtypeInstances.Count;
											}
											for (int i = 0; i < allSubtypeInstancesCount; ++i)
											{
												EntityTypeSubtypeInstance instance = allSubtypeInstances[i];
												if (!instance.IsDeleting && instance != subtypeInstance && instance.ObjectType == subSupertype)
												{
													FrameworkDomainModel.DelayValidateElement(instance, DelayValidateInstancePopulationMandatoryError);
												}
											}
										}
									}
									return ObjectTypeVisitorResult.SkipChildren;
								});
					}
				}
				finally
				{
					myIsDisabled = false;
				}
			}
		}
		[DelayValidatePriority(2)] // Validate subtype instances after all other instances are in place
		private static void DelayValidateSubtypeInstance(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				EntityTypeSubtypeInstance subtypeInstance = (EntityTypeSubtypeInstance)element;
				ObjectType subtype;
				ObjectType supertype;
				EntityTypeInstance supertypeInstance;
				UniquenessConstraint pid;
				ObjectType preferredFor;
				if (null == (subtype = subtypeInstance.EntityTypeSubtype) ||
					null == (supertypeInstance = subtypeInstance.SupertypeInstance) ||
					null == (supertype = supertypeInstance.EntityType) ||
					null == (pid = subtype.ResolvedPreferredIdentifier) ||
					null == (preferredFor = pid.PreferredIdentifierFor) ||
					preferredFor != supertype)
				{
					subtypeInstance.Delete();
				}
				else
				{
					ObjectType.WalkSupertypes(
						subtype,
						delegate(ObjectType intermediateSupertype, int depth, bool isPrimary)
						{
							if (depth == 0)
							{
								return ObjectTypeVisitorResult.Continue;
							}
							else if (isPrimary && intermediateSupertype != supertype)
							{
								LinkedElementCollection<EntityTypeSubtypeInstance> intermediateInstances = intermediateSupertype.EntityTypeSubtypeInstanceCollection;
								int intermediateInstanceCount = intermediateInstances.Count;
								int i = 0;
								for (; i < intermediateInstanceCount; ++i)
								{
									if (intermediateInstances[i].SupertypeInstance == supertypeInstance)
									{
										break;
									}
								}
								if (i == intermediateInstanceCount)
								{
									EntityTypeSubtypeInstance.GetSubtypeInstance(supertypeInstance, intermediateSupertype, false, true);
								}
							}
							// Always skip, only do one level at a time
							return ObjectTypeVisitorResult.SkipChildren;
						});
				}
			}
		}
		[DelayValidatePriority(2)] // Validate subtype instances after all other instances are in place
		private static void DelayValidateSubtypeInstances(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ObjectType subtype = (ObjectType)element;
				UniquenessConstraint pid;
				ObjectType preferredFor = null;
				if ((null == (pid = subtype.ResolvedPreferredIdentifier)) ||
					(null == (preferredFor = pid.PreferredIdentifierFor)) ||
					preferredFor == subtype)
				{
					subtype.EntityTypeSubtypeInstanceCollection.Clear();
				}
				if (preferredFor != null && preferredFor != subtype)
				{
					// Make sure that the instances resolve to the identifying supertype
					LinkedElementCollection<EntityTypeSubtypeInstance> subtypeInstances = subtype.EntityTypeSubtypeInstanceCollection;
					int subtypeInstanceCount = subtypeInstances.Count;
					for (int i = subtypeInstanceCount - 1; i >= 0; --i)
					{
						EntityTypeSubtypeInstance subtypeInstance = subtypeInstances[i];
						EntityTypeInstance supertypeInstance;
						if (null == (supertypeInstance = subtypeInstance.SupertypeInstance) ||
							preferredFor != supertypeInstance.EntityType)
						{
							subtypeInstance.Delete();
						}
					}

					// Make sure that, for each of the remaining instances, all intermediate subtypes also
					// are also linked to the identifying supertype instances
					subtypeInstanceCount = subtypeInstances.Count;
					if (subtypeInstanceCount != 0)
					{
						EntityTypeInstance[] supertypeInstances = null;
						BitTracker instanceMatches = default(BitTracker);
						IComparer<EntityTypeInstance> comparer = null;
						ObjectType.WalkSupertypes(
							subtype,
							delegate(ObjectType supertype, int depth, bool isPrimary)
							{
								if (depth == 0)
								{
									return ObjectTypeVisitorResult.Continue;
								}
								else if (isPrimary && supertype != preferredFor && supertype.ResolvedPreferredIdentifier == pid)
								{
									// Non-primary supertypes need to be manually populated and
									// will produce population mandatory errors.
									// Note that the subtype graph, compatibility, and the preferred path
									// have already been validated, so checking the primary state is sufficient
									// Anything else gets automatically populated
									if (supertypeInstances == null)
									{
										supertypeInstances = new EntityTypeInstance[subtypeInstanceCount];
										instanceMatches = new BitTracker(subtypeInstanceCount);
										for (int i = 0; i < subtypeInstanceCount; ++i)
										{
											supertypeInstances[i] = subtypeInstances[i].SupertypeInstance;
										}
										Array.Sort<EntityTypeInstance>(supertypeInstances, comparer = ModelElementIdComparer<EntityTypeInstance>.Instance);
									}
									int matched = 0;
									foreach (EntityTypeSubtypeInstance intermediateInstance in supertype.EntityTypeSubtypeInstanceCollection)
									{
										EntityTypeInstance boundInstance = intermediateInstance.SupertypeInstance;
										if (boundInstance.EntityType == preferredFor) // Sanity check, will be cleaned up by other iterations of this function
										{
											int matchingIndex = Array.BinarySearch<EntityTypeInstance>(supertypeInstances, boundInstance, comparer);
											if (matchingIndex >= 0 && !instanceMatches[matchingIndex])
											{
												instanceMatches[matchingIndex] = true;
												++matched;
											}
										}
									}
									if (matched != subtypeInstanceCount)
									{
										// Add the new instance
										for (int i = 0; i < subtypeInstanceCount; ++i)
										{
											if (!instanceMatches[i])
											{
												instanceMatches[i] = false; // prepare for next pass
												EntityTypeSubtypeInstance.GetSubtypeInstance(supertypeInstances[i], supertype, false, true);
											}
										}
									}
									else
									{
										instanceMatches.Reset();
									}
								}
								// Skip in all cases. If we added, then rules will trigger additional adds for other
								// intermediate supertypes
								return ObjectTypeVisitorResult.SkipChildren;
							});
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypeHasObjectTypeInstance)
		/// Verify that implied instances were added by us, and that
		/// objectified instances all get an error.
		/// </summary>
		private static void ObjectTypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			ObjectTypeHasObjectTypeInstance link = (ObjectTypeHasObjectTypeInstance)e.ModelElement;
			ObjectType entityType = link.ObjectType;
			FactType factType;
			UniquenessConstraint pid;
			if (null != (factType = entityType.NestedFactType) &&
				null != (pid = entityType.ResolvedPreferredIdentifier))
			{
				LinkedElementCollection<FactType> pidFactTypes;
				FactType identifierFactType;
				Role unaryRole;
				ObjectifiedUnaryRole objectifiedUnaryRole;
				if (pid.PreferredIdentifierFor == entityType && // quick check to rule out a subtype situation
					pid.IsInternal &&
					1 == (pidFactTypes = pid.FactTypeCollection).Count &&
					((identifierFactType = pidFactTypes[0]) == factType ||
					(null != (unaryRole = factType.UnaryRole) &&
					null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
					identifierFactType == objectifiedUnaryRole.FactType)))
				{
					// UNDONE: Better mechanism to check that this coming from the FactTypeInstance.FactTypeInstanceAddedRule,
					// not just any rule. Note that the expense of turning this rule on/off is not worth it.
					// A framework service allowing a rule to 'push' itself on a stack would be useful, but maintaining the
					// statck is not worth doing arbitrarily, only for the unusual cases. This comment also applies to any
					// other rule checking for ChangeSource.Rule.
					if (e.ChangeSource != ChangeSource.Rule)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceDirectModificationOfImpliedEntityTypeInstance);
					}
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(link.ObjectTypeInstance, DelayValidateObjectifiedInstanceRequiredError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypeHasObjectTypeInstance)
		/// </summary>
		private static void ObjectTypeInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectTypeInstanceEnforceFixedRolePlayers);
		}
		#region Handle objectified FactType inclusion in generated name
		/// <summary>
		/// ChangeRule: typeof(FactTypeInstance)
		/// Update objectified instance name
		/// </summary>
		private static void FactTypeInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			ObjectTypeInstance objectInstance;
			if (e.DomainProperty.Id == FactTypeInstance.NameChangedDomainPropertyId &&
				null != (objectInstance = ((FactTypeInstance)e.ModelElement).ObjectifyingInstance))
			{
				FrameworkDomainModel.DelayValidateElement(objectInstance, DelayValidateNamePartChanged);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactType)
		/// Update objectified instance name
		/// </summary>
		private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == FactType.NameChangedDomainPropertyId)
			{
				DelayValidateInstanceNames(((FactType)e.ModelElement).NestingType);
			}
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void DelayValidateInstanceNames(ObjectType objectType)
		{
			if (objectType != null && !objectType.IsDeleted)
			{
				foreach (ObjectTypeInstance objectInstance in objectType.ObjectTypeInstanceCollection)
				{
					// Note that these are generally done through the FactTypeInstance, but a
					// missing ObjectificationInstance link requires us to watch the FactType directly
					FrameworkDomainModel.DelayValidateElement(objectInstance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(Objectification)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationAddedRule(ElementAddedEventArgs e)
		{
			DelayValidateInstanceNames(((Objectification)e.ModelElement).NestingType);
		}
		/// <summary>
		/// DeleteRule: typeof(Objectification)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
		{
			DelayValidateInstanceNames(((Objectification)e.ModelElement).NestingType);
		}
		/// <summary>
		/// AddRule: typeof(ObjectificationInstance)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationInstanceAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ObjectificationInstance)e.ModelElement).ObjectifyingInstance, DelayValidateNamePartChanged);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectificationInstance)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectTypeInstance objectInstance = ((ObjectificationInstance)e.ModelElement).ObjectifyingInstance;
			if (!objectInstance.IsDeleted)
			{
				objectInstance.DelayValidateIfEmpty();
				FrameworkDomainModel.DelayValidateElement(objectInstance, DelayValidateNamePartChanged);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectificationInstance)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationInstanceRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectificationInstance.ObjectifyingInstanceDomainRoleId)
			{
				ObjectTypeInstance oldInstance = (ObjectTypeInstance)e.OldRolePlayer;
				oldInstance.DelayValidateIfEmpty();
				FrameworkDomainModel.DelayValidateElement(oldInstance, DelayValidateNamePartChanged);
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateNamePartChanged);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(((ObjectificationInstance)e.ElementLink).ObjectifyingInstance, DelayValidateNamePartChanged);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(Objectification)
		/// Update objectified instance name
		/// </summary>
		private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == Objectification.NestingTypeDomainRoleId)
			{
				DelayValidateInstanceNames((ObjectType)e.OldRolePlayer);
			}
			DelayValidateInstanceNames(((Objectification)e.ElementLink).NestingType);
		}
		/// <summary>
		/// Validate an <see cref="ObjectTypeInstance"/> using the <see cref="DelayValidateIfEmpty"/> method.
		/// </summary>
		protected static void DelayValidateEmptyInstance(ObjectTypeInstance objectInstance)
		{
			objectInstance.DelayValidateIfEmpty();
		}
		/// <summary>
		/// A use of a potentially empty instance is no longer referencing this instance,
		/// make sure that the existing of the empty instance is validated.
		/// </summary>
		protected virtual void DelayValidateIfEmpty()
		{
			// Default is empty
		}
		#endregion // Handle objectified FactType inclusion in generated name
		#endregion // ObjectTypeInstance Rules
	}
	#endregion // ObjectTypeInstance class
	#region EntityTypeSubtypeInstance class
	partial class EntityTypeSubtypeInstance : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		private string myGeneratedName = string.Empty;
		private string myGeneratedIdentifierName = string.Empty;
		/// <summary>
		/// Enable name generation
		/// </summary>
		protected override bool HasGeneratedNames
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:Name"/> property
		/// </summary>
		protected override string GenerateName()
		{
			return ObjectTypeInstance.GetDisplayString(this, EntityTypeSubtype, false);
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:IdentifierName"/> property
		/// </summary>
		protected override string GenerateIdentifierName()
		{
			return ObjectTypeInstance.GetDisplayString(this, EntityTypeSubtype, true);
		}
		/// <summary>
		/// Provide storage for the generated <see cref="P:Name"/>
		/// </summary>
		protected override string GeneratedName
		{
			get
			{
				return myGeneratedName;
			}
			set
			{
				myGeneratedName = value;
			}
		}
		/// <summary>
		/// Provide storage for the generated <see cref="P:IdentifierName"/>
		/// </summary>
		protected override string GeneratedIdentifierName
		{
			get
			{
				return myGeneratedIdentifierName;
			}
			set
			{
				myGeneratedIdentifierName = value;
			}
		}
		/// <summary>
		/// If this identifier is empty, then verify that it is both associated with
		/// a <see cref="FactTypeInstance"/> and included in another instance definition.
		/// </summary>
		protected override void DelayValidateIfEmpty()
		{
			EntityTypeInstance supertypeInstance;
			if (!IsDeleted &&
				null != (supertypeInstance = SupertypeInstance))
			{
				DelayValidateEmptyInstance(supertypeInstance);
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Helper Methods
		/// <summary>
		/// Retrieve an <see cref="EntityTypeSubtypeInstance"/> for the <paramref name="entityInstance"/>
		/// that matches the specified <paramref name="subtype"/>.
		/// </summary>
		/// <param name="entityInstance">The <see cref="EntityTypeInstance"/> to use as the <see cref="P:SuperTypeInstance"/></param>
		/// <param name="subtype">The <see cref="ObjectType"/> that is a subtype identified by the <see cref="P:EntityTypeInstance.EntityType"/>
		/// of the <paramref name="entityInstance"/></param>
		/// <param name="checkExisting">True to check existing instances for an existing subtype instance.</param>
		/// <param name="forceCreate">If <paramref name="checkExisting"/> is set and no match is found, then create a new instance</param>
		/// <returns>An <see cref="EntityTypeSubtypeInstance"/> or <see langword="null"/>, depending on the specified options.</returns>
		public static EntityTypeSubtypeInstance GetSubtypeInstance(EntityTypeInstance entityInstance, ObjectType subtype, bool checkExisting, bool forceCreate)
		{
			if (checkExisting)
			{
				// Note that the total set of subtype instances for any given entity is unbound as the population grows.
				// However, the total number of subtype instances is bounded by the number of identified subtypes in the
				// model, not the number of instances.
				foreach (EntityTypeSubtypeInstance testSubtypeInstance in entityInstance.EntityTypeSubtypeInstanceCollection)
				{
					if (testSubtypeInstance.EntityTypeSubtype == subtype)
					{
						return testSubtypeInstance;
					}
				}
			}
			else
			{
				forceCreate = true;
			}
			if (forceCreate)
			{
				EntityTypeSubtypeInstance newSubtypeInstance = new EntityTypeSubtypeInstance(entityInstance.Partition);
				newSubtypeInstance.SupertypeInstance = entityInstance;
				newSubtypeInstance.EntityTypeSubtype = subtype;
				return newSubtypeInstance;
			}
			return null;
		}
		/// <summary>
		/// Get an instance of the requested type that is related to the provided
		/// instance 
		/// </summary>
		/// <param name="instance">The instance to verify.</param>
		/// <param name="relatedType">The type of the related instance.</param>
		/// <returns>The same instance, or a related instance of the given type.</returns>
		public static ObjectTypeInstance GetTypedInstance(ObjectTypeInstance instance, ObjectType relatedType)
		{
			if (relatedType != null &&
				instance.ObjectType != relatedType)
			{
				EntityTypeInstance entityTypeInstance;
				EntityTypeSubtypeInstance subtypeInstance;
				if (null == (entityTypeInstance = instance as EntityTypeInstance))
				{
					if (null != (subtypeInstance = instance as EntityTypeSubtypeInstance))
					{
						entityTypeInstance = subtypeInstance.SupertypeInstance;
						if (entityTypeInstance.ObjectType == relatedType)
						{
							return entityTypeInstance;
						}
					}
				}
				if (null != entityTypeInstance)
				{
					return GetSubtypeInstance(entityTypeInstance, relatedType, true, true);
				}
			}
			return instance;
		}
		#endregion // Helper Methods
	}
	#endregion // EntityTypeSubtypeInstance class
	#region ObjectificationInstance class
	partial class ObjectificationInstance
	{
		#region Rule methods
		partial class RoleInstanceRolePlayerChangedRuleClass
		{
			private bool myIsDisabled;
			/// <summary>
			/// RolePlayerChangeRule: typeof(RoleInstance)
			/// Enforce role player change semantics for objectification associations
			/// </summary>
			private void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				if (myIsDisabled)
				{
					return;
				}
				if (Utility.IsDescendantOrSelf(e.DomainRole, RoleInstance.ObjectTypeInstanceDomainRoleId))
				{
					RoleInstance link = (RoleInstance)e.ElementLink;
					FactTypeRoleInstance factRoleInstance;
					EntityTypeRoleInstance entityRoleInstance;
					FactTypeInstance factInstance;
					EntityTypeInstance entityInstance;
					FactType factType;
					ObjectType entityType;
					UniquenessConstraint pid;
					LinkedElementCollection<FactType> pidFactTypes;
					FactType identifierFactType;
					Role unaryRole = null;
					ObjectifiedUnaryRole objectifiedUnaryRole = null;
					Role findRole;
					if (null != (factRoleInstance = link as FactTypeRoleInstance))
					{
						if (null != (factInstance = factRoleInstance.FactTypeInstance) &&
							null != (entityInstance = factInstance.ObjectifyingInstance as EntityTypeInstance) && // Note that an EntityTypeSubtypeInstance is externally identified and has no implicit population
							null != (factType = factInstance.FactType) &&
							null != (entityType = entityInstance.ObjectType) &&
							null != (pid = entityType.PreferredIdentifier) &&
							pid.IsInternal &&
							1 == (pidFactTypes = pid.FactTypeCollection).Count &&
							((identifierFactType = pidFactTypes[0]) == factType ||
							(null != (unaryRole = factType.UnaryRole) &&
							null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
							identifierFactType == objectifiedUnaryRole.FactType)) &&
							null != (entityRoleInstance = entityInstance.FindRoleInstance(((findRole = factRoleInstance.Role) == unaryRole) ? objectifiedUnaryRole : findRole)))
						{
							myIsDisabled = true;
							try
							{
								entityRoleInstance.ObjectTypeInstance = factRoleInstance.ObjectTypeInstance;
							}
							finally
							{
								myIsDisabled = false;
							}
						}
					}
					else if (null != (entityRoleInstance = link as EntityTypeRoleInstance))
					{
						if (null != (entityInstance = entityRoleInstance.EntityTypeInstance) &&
							null != (factInstance = entityInstance.ObjectifiedInstance) &&
							null != (factType = factInstance.FactType) &&
							null != (entityType = entityInstance.ObjectType) &&
							null != (pid = entityType.PreferredIdentifier) &&
							pid.IsInternal &&
							1 == (pidFactTypes = pid.FactTypeCollection).Count &&
							((identifierFactType = pidFactTypes[0]) == factType ||
							(null != (unaryRole = factType.UnaryRole) &&
							null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
							identifierFactType == objectifiedUnaryRole.FactType)))
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceDirectModificationOfImpliedEntityTypeInstance);
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectificationInstance)
		/// Verify that an <see cref="ObjectificationInstance"/> is consistent with the
		/// <see cref="Objectification"/> link between the associated <see cref="FactType"/>
		/// and <see cref="ObjectType"/>
		/// </summary>
		private static void ObjectificationInstanceAddedRule(ElementAddedEventArgs e)
		{
			ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
			FactTypeInstance factInstance = link.ObjectifiedInstance;
			ObjectTypeInstance objectInstance = link.ObjectifyingInstance;
			FactType factType;
			ObjectType entityType;
			if (null == (factType = factInstance.FactType) ||
				null == (entityType = objectInstance.ObjectType) ||
				factType.NestingType != entityType)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceIncompleteRolePlayers);
			}
			// The entityInstance has a trivial error condition (any FactTypeInstance is required, even empty ones)
			// However, the factInstance condition is non-trivial as an ObjectifyingInstanceRequiredError
			// is displayed if the non-implied identifier instance is empty.
			FrameworkDomainModel.DelayValidateElement(factInstance, FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError);
			objectInstance.ObjectifiedInstanceRequiredError = null; // Trivial condition, not worth delay validating
		}
		/// <summary>
		/// DeletingRule: typeof(ObjectificationInstance)
		/// Emulate delete propagation if the <see cref="ObjectifiedInstance"/> endpoint is
		/// deleted and the uniqueness constraint is internal to the objectified FactType.
		/// Note that we do not propagate an identifier instance deletion because there is
		/// never a good reason to toss a FactType population, and this has side-effects
		/// when preferred identifiers are changed and the old object instance is interpreted
		/// as internal because of the
		/// </summary>
		private static void ObjectificationInstanceDeletingRule(ElementDeletingEventArgs e)
		{
			ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
			FactTypeInstance factInstance = link.ObjectifiedInstance;
			ObjectTypeInstance objectInstance = link.ObjectifyingInstance;
			FactType factType;
			ObjectType entityType;
			EntityTypeHasPreferredIdentifier pidLink;
			Objectification objectification;
			UniquenessConstraint pid;
			LinkedElementCollection<FactType> pidFactTypes;
			FactType identifierFactType;
			Role unaryRole;
			ObjectifiedUnaryRoleHasRole objectifiedUnaryLink;
			bool factInstanceDeleting = factInstance.IsDeleting;
			bool entityInstanceDeleting = objectInstance.IsDeleting;
			EntityTypeInstance typedEntityInstance = null;
			if (factInstanceDeleting &&
				!entityInstanceDeleting &&
				null != (typedEntityInstance = objectInstance as EntityTypeInstance) && // A subtype instance is never an internal identifier
				null != (factType = factInstance.FactType) &&
				!factType.IsDeleting &&
				null != (entityType = objectInstance.ObjectType) &&
				!entityType.IsDeleting &&
				null != (objectification = entityType.Objectification) && // Allow invalidated links to be blown away without turning off this rule
				objectification.NestedFactType == factType &&
				null != (pidLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifier(entityType)) &&
				!pidLink.IsDeleting &&
				(pid = pidLink.PreferredIdentifier).IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				((identifierFactType = pidFactTypes[0]) == factType ||
				(null != (unaryRole = factType.UnaryRole) &&
				null != (objectifiedUnaryLink = ObjectifiedUnaryRoleHasRole.GetLinkToObjectifiedUnaryRole(unaryRole)) &&
				!objectifiedUnaryLink.IsDeleting &&
				identifierFactType == objectifiedUnaryLink.ObjectifiedUnaryRole.FactType)))
			{
				objectInstance.Delete();
			}
			else
			{
				if (!factInstanceDeleting)
				{
					if (factInstance.RoleInstanceCollection.Count == 0)
					{
						// If an attempt is made to associate an entity instance with a new fact instance,
						// then an empty FactTypeInstance must be created without an attached FactTypeRoleInstance.
						// These empty instances need to be deleted if the objectification link or the corresponding
						// entity instance is deleted.
						factInstance.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(factInstance, FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError);
					}
				}
				if (!entityInstanceDeleting)
				{
					if (typedEntityInstance != null && typedEntityInstance.RoleInstanceCollection.Count == 0)
					{
						// The same logic applies to the other end: an empty EntityTypeInstance may need
						// to be created so that it can be associated with a populated FactTypeInstance
						typedEntityInstance.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(objectInstance, ObjectTypeInstance.DelayValidateObjectifiedInstanceRequiredError);
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectificationInstance)
		/// Validate appropriate errors if an ObjectifiedInstance role changes
		/// </summary>
		private static void ObjectificationInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ObjectificationInstance link = (ObjectificationInstance)e.ElementLink;
			FactTypeInstance factInstance = link.ObjectifiedInstance;
			ObjectTypeInstance objectInstance = link.ObjectifyingInstance;
			FactType factType;
			ObjectType entityType;
			UniquenessConstraint pid;
			LinkedElementCollection<FactType> pidFactTypes;
			FactType identifierFactType;
			Role unaryRole;
			ObjectifiedUnaryRole objectifiedUnaryRole;
			if (null != (factType = factInstance.FactType) &&
				null != (entityType = objectInstance.ObjectType) &&
				null != (pid = entityType.PreferredIdentifier) &&
				!pid.IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				((identifierFactType = pidFactTypes[0]) == factType ||
				(null != (unaryRole = factType.UnaryRole) &&
				null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
				identifierFactType == objectifiedUnaryRole.FactType)))
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceDirectModificationOfImpliedEntityTypeInstance);
			}
			if (e.DomainRole.Id == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
			{
				FactTypeInstance oldFactInstance = (FactTypeInstance)e.OldRolePlayer;
				if (oldFactInstance.FactType != factInstance.FactType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceIncompleteRolePlayers);
				}
				if (oldFactInstance.RoleInstanceCollection.Count == 0)
				{
					// See comments in ObjectificationInstanceDeletingRule
					oldFactInstance.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(oldFactInstance, FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError);
				}
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, FactTypeInstance.DelayValidateObjectifyingInstanceRequiredError);
			}
			else
			{
				ObjectTypeInstance oldObjectInstance = (ObjectTypeInstance)e.OldRolePlayer;
				if (oldObjectInstance.ObjectType != objectInstance.ObjectType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceIncompleteRolePlayers);
				}
				EntityTypeInstance oldEntityInstance = oldObjectInstance as EntityTypeInstance;
				if (oldEntityInstance != null && oldEntityInstance.RoleInstanceCollection.Count == 0)
				{
					// See comments in ObjectificationInstanceDeletingRule
					oldEntityInstance.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, ObjectTypeInstance.DelayValidateObjectifiedInstanceRequiredError);
				}
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, ObjectTypeInstance.DelayValidateObjectifiedInstanceRequiredError);
			}
		}
		#endregion // Rule methods
	}
	#endregion // ObjectificationInstance class
	#region EntityTypeRoleInstance class
	partial class EntityTypeRoleInstance : IHasIndirectModelErrorOwner
	{
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // EntityTypeRoleInstance class
	#region ImpliedFactInstancePopulation struct
	/// <summary>
	/// Helper structure to determine when the set of <see cref="FactTypeInstance"/>
	/// elements associated with a <see cref="FactType"/> is implied by the population
	/// of another <see cref="ObjectType"/>. Implied population occurs if the <see cref="FactType"/>
	/// is used as part of the identification scheme for the implying entity type or subtype.
	/// </summary>
	public struct FactTypeInstanceImplication
	{
		#region Fields
		/// <summary>
		/// The entity type with the population that implies
		/// the population of this <see cref="FactType"/>
		/// </summary>
		public readonly ObjectType ImpliedByEntityType;
		/// <summary>
		/// The supertype of of the <see cref="ImpliedByEntityType"/> that
		/// provides its identification scheme. If the ProxyEntityType
		/// is self-identifying, then this is set to the same value as <see cref="ImpliedByEntityType"/>.
		/// If an identification scheme has not been specified then this
		/// is <see langword="null"/>.
		/// </summary>
		public readonly ObjectType IdentifyingSupertype;
		/// <summary>
		/// If the <see cref="FactType"/> is a link fact type implied by
		/// objectification, then specifiy the associated <see cref="RoleProxy"/>
		/// or <see cref="ObjectifiedUnaryRole"/>.
		/// </summary>
		public readonly RoleBase ImpliedProxyRole;
		#endregion // Fields
		#region Constructor
		/// <summary>
		/// Get information about the <see cref="ObjectType">EntityType</see>
		/// that provides an implied population for this <paramref name="factType"/>
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to retrieve implied
		/// information for.</param>
		public FactTypeInstanceImplication(FactType factType)
		{
			ObjectType impliedByEntityType = null;
			ObjectType identifyingSuperType = null;
			RoleBase impliedProxyRole = null;
			IList<RoleBase> factRoles;
			SubtypeFact subtypeFact;
			Objectification objectification;
			RoleBase testRole;
			UniquenessConstraint pid;
			if (factType != null && !factType.IsDeleted)
			{
				if (null != (subtypeFact = factType as SubtypeFact))
				{
					if (subtypeFact.ProvidesPreferredIdentifier)
					{
						// The population is implied for FactType
						impliedByEntityType = subtypeFact.Subtype;
						pid = impliedByEntityType.ResolvedPreferredIdentifier;
						identifyingSuperType = (pid != null) ? pid.PreferredIdentifierFor : null;
					}
				}
				else if (null != (objectification = factType.ImpliedByObjectification) &&
					2 == (factRoles = factType.OrderedRoleCollection).Count &&
					null != (impliedProxyRole = (RoleBase)((testRole = factRoles[0]) as RoleProxy) ?? testRole as ObjectifiedUnaryRole ?? (RoleBase)((testRole = factRoles[1]) as RoleProxy) ?? testRole as ObjectifiedUnaryRole))
				{
					impliedByEntityType = objectification.NestingType;
					pid = impliedByEntityType.ResolvedPreferredIdentifier;
					identifyingSuperType = (pid != null) ? pid.PreferredIdentifierFor : null;
				}
				else
				{
					foreach (SetConstraint setConstraint in factType.SetConstraintCollection)
					{
						UniquenessConstraint uc;
						ObjectType preferredFor;
						if (null != (uc = setConstraint as UniquenessConstraint) &&
							null != (preferredFor = uc.PreferredIdentifierFor) &&
							preferredFor.NestedFactType != factType)
						{
							identifyingSuperType = impliedByEntityType = preferredFor;
							break;
						}
					}
				}
			}
			ImpliedByEntityType = impliedByEntityType;
			IdentifyingSupertype = identifyingSuperType;
			ImpliedProxyRole = impliedProxyRole;
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// <see langword="true"/> if the <see cref="FactType"/> specified
		/// in the constructor has an implied population.
		/// </summary>
		public bool IsImplied
		{
			get
			{
				return ImpliedByEntityType != null;
			}
		}
		#endregion // Accessor Properties
		#region Equality
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is FactTypeInstanceImplication)
			{
				return Equals((FactTypeInstanceImplication)obj);
			}
			return false;
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			ObjectType objectType;
			RoleBase role;
			return Utility.GetCombinedHashCode(
				(objectType = ImpliedByEntityType) != null ? objectType.GetHashCode() : 0,
				(objectType = IdentifyingSupertype) != null ? objectType.GetHashCode() : 0,
				(role = ImpliedProxyRole) != null ? role.GetHashCode() : 0);
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(FactTypeInstanceImplication other)
		{
			return ImpliedByEntityType == other.ImpliedByEntityType && IdentifyingSupertype == other.IdentifyingSupertype && ImpliedProxyRole == other.ImpliedProxyRole;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(FactTypeInstanceImplication left, FactTypeInstanceImplication right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(FactTypeInstanceImplication left, FactTypeInstanceImplication right)
		{
			return !left.Equals(right);
		}
		#endregion // Equality
	}
	#endregion // ImpliedFactInstancePopulation struct
}
