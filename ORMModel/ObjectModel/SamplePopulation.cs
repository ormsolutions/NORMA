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

// UNDONE: NOW Kill EntityTypeRoleInstance
// This change is not complete. Beyond the file format change (using accurate names for instances), this eliminates the notion of EntityTypeRoleInstance,
// which populated identifying roles directly and block population of fact types used in a preferred identifier. This notion has been eliminated,
// so now identified entity instances participate directly in facts for their preferred identifier. The point of this transition was to allow sample
// population to be used both before a preferred identifier was chosen (through an IdentityTag property and corresponding column in the editor) as
// well as to survive the deletion or addition of the preferred identifier. Since object instances (old names: ValueTypeInstance, EntityTypeInstance,
// EntityTypeSubtypeInstance) were tied to the identification form they did not survive an identifier change so subtypes of ObjectTypeInstance were used.
// Now, there are no subtypes and the (renamed) ObjectInstance is maintained across transitions.
// This means there are 5 possible primary states for an instance:
// 1) The object type is a value type. In this state the Value property is used and IdentityTag (and AutoIdentityTag, which indicates if this was set
//    by the user or the machine) is not used. This is the old ValueTypeInstance type.
// 2) The object type has a direct preferred identifier. In this case IdentityTag can also be set, as can AutoIdentityTag. This is the old EntityTypeInstance.
// 3) The object type has an indirect preferred identifier (ResolvedPrefferedIdentifier). This is the old EntityTypeSubtypeInstance, which holds a reference
//    to the identifying supertype instance. We do not preserve or show identity tags in this case--the tag used is from the supertype.
// 4) The object type has an identifying supertype but the supertype does not have a preferred identifier. In this case the weakest available supertype is
//    used to target NEW instances of the EntityTypeSubtypeInstance relationships.
// 5) The object type has none of the above, which means the IdentityTag is used to identify. The tag is shown in square brackets as part of the instance name.
//    If the tag and direct identifier are both populated, then the form is [tag]=id (single id) or [tag]=(compound id list).
//
// The difficult part of this is transitions between the identified and non-identified states.
// 1-5/5-1 Transition between tagged and valued simply moves the data between the two fields. This is considered user-entered data and is not marked as automatic.
// 2-5 (deleting the direct preferred identifier, no subtype available) In this case any instances that are used after the id deletion (this may include facts in
//    the formerly identifying fact types) are saved; others are deleted. A use is any normal fact type use, as a supertype instance, or by subtype instances, or with
//    a non-automatic identity tag. Remaining objects that are used but are not tagged get a numbered tag based on the object type name, but are flagged as AutoIdentityTag.
//    This flag is never set automatically. It is the responsibility of the editor to clear it when the user edits the name. The editor displays user-set names in bold.
//    The only difference with AutoIdentityTag is that it does not keep the instance alive.
// 5-2 (adding a preferred identifier) This just keeps the tags as is, but now collects and displays both the tag and identifier data. There is currently no attempt
//     to move tag values to identifiers when a single valued identifier can be resolved, but this is not precluded in the future, either automatically or with an
//     explicit migration command set on the identity column. Note that identity tag columns are shown if either IdentityTag is required OR if any instance has an
//     identity tag. Since the identifier is not populated when added it does not make sense to automatically clear the identity tag.
// 5-4, 5-3 This kills the identity tags. Subtype instances get tagged by the supertype instance.
// 
// Transitions involving subtype instances. This is where the major remaining work is (besides going over all of the UNDONE flags for Kill EntityTypeRoleInstance).
// The issue here is that if an identifier remains then we can transition the instance reference to a new compatible supertype. However, if there is no identifier
//    on the supertype (case 4) then we can't just kill the old one as this loses instance data for populated fact types. This means that, while new instances
//    can get the top supertype, existing instance may have remaining supertypes hanging on after the transition. There is no guarantee these will all point to the
//    same supertype, which means rules and editor state have to check for this carefully. We may block edits on legacy links that point to the wrong type (just show
//    the existing object type name), but the main is that there are no rules written for this transition. It is completely uncoded at this point. The transition from
//    the unidentified supertype to the identified one may result in the bound supertype instance being moved to the newly resolved type.
//
// Again, this is a partial change and will be amended with future work, but I have some other more pressing items that demand full time attention for the next few months
// so the new file format transitions (this, machine.orm added with standard ref modes and custom/standard ref mode distinctions disappear, scalar fact types replacing
// functions and a complete datatype and aggregation rework) will have to resume at a later date.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.OLE.Interop;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region SamplePopulation ModelError Classes
	[ModelErrorDisplayFilter(typeof(PopulationErrorCategory))]
	public partial class TooFewEntityRoleInstancesError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectInstance entityInstance = EntityInstance;
			ObjectType entityType = (entityInstance != null) ? entityInstance.EntityType : null;
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorEntityInstanceTooFewRoleInstancesMessage,
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
				ObjectInstance entityInstance = EntityInstance;
				return string.Format(
					ResourceStrings.ModelErrorEntityInstanceTooFewRoleInstancesCompactMessage,
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
	public partial class TooFewFactRoleInstancesError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FactInstance factInstance = FactInstance;
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorFactInstanceTooFewRoleInstancesMessage,
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
				FactInstance factInstance = FactInstance;
				return string.Format(
					ResourceStrings.ModelErrorFactInstanceTooFewRoleInstancesCompactMessage,
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
	public partial class CompatibleValueInstanceError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ObjectInstance valueInstance = ValueInstance;
			ObjectType valueType = (valueInstance != null) ? valueInstance.ObjectType : null;
			// Go ahead and use the value directly here instead of looking at the invariant value.
			// If the invariant value succeeds as a backup then you don't get to this point.
			ORMModel model = Model;
			ErrorText = string.Format(
				ResourceStrings.ModelErrorValueInstanceCompatibleValueInstanceMessage,
				valueInstance != null ? valueInstance.Value : "",
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
				ObjectInstance valueInstance = ValueInstance;
				ObjectType valueType = (valueInstance != null) ? valueInstance.ValueType : null;
				// Go ahead and use the value directly here instead of looking at the invariant value.
				// If the invariant value succeeds as a backup then you don't get to this point.
				return string.Format(
					ResourceStrings.ModelErrorValueInstanceCompatibleValueInstanceCompactMessage,
					valueInstance != null ? valueInstance.Value : "",
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
		/// Return the duplicated ObjectInstance
		/// </summary>
		public ObjectInstance DuplicateObjectInstance
		{
			get
			{
				return this.RoleInstanceCollection[0].ObjectInstance;
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
				DuplicateObjectInstance.Name,
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
					DuplicateObjectInstance.Name,
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
			ObjectInstance instance = ObjectInstance;
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
					ObjectInstance.Name,
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
			ObjectInstance instance = this.ObjectInstance;
			ObjectType entityType = (instance != null) ? instance.ObjectType : null;
			ORMModel model = Model;
			ErrorText = string.Format(
				CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorEntityInstanceObjectifiedInstanceRequiredMessage,
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
					ResourceStrings.ModelErrorEntityInstanceObjectifiedInstanceRequiredCompactMessage,
					ObjectInstance.Name);
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns associated <see cref="ObjectModel.ObjectInstance"/>.
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[]{this.ObjectInstance};
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
			FactInstance instance = this.FactInstance;
			ORMModel model = Model;
			ErrorText = string.Format(
				CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorFactInstanceObjectifyingInstanceRequiredMessage,
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
					ResourceStrings.ModelErrorFactInstanceObjectifyingInstanceRequiredCompactMessage,
					FactInstance.Name);
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>. Returns associated <see cref="ObjectModel.FactInstance"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.FactInstance };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion
	#region FactInstance class
	public partial class FactInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Helper Methods
		/// <summary>
		/// Finds the <see cref="RoleInstance"/> for the given <paramref name="role"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="role">Role to match on</param>
		/// <returns>RoleInstance for the given role, or null if none found.</returns>
		public RoleInstance FindRoleInstance(Role role)
		{
			return FindRoleInstance(RoleInstanceCollection, role);
		}
		/// <summary>
		/// Finds the RoleInstance for the given <paramref name="role"/>
		/// in the provided <paramref name="roleInstances"/>.
		/// Returns null if no matching RoleInstance is found.
		/// </summary>
		/// <param name="roleInstances"></param>
		/// <param name="role">Role to match on</param>
		/// <returns>RoleInstance for the given role, or null if none found.</returns>
		public static RoleInstance FindRoleInstance(IList<RoleInstance> roleInstances, Role role)
		{
			int roleInstanceCount = roleInstances.Count;
			RoleInstance roleInstance;
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
		/// This routine safed creates a new <see cref="RoleInstance"/>. A new RoleInstance
		/// should be created directly only for new <see cref="FactInstance"/> elements.
		/// </summary>
		/// <param name="factRole">A role from the <see cref="FactType"/> to attach to</param>
		/// <param name="instance">The instance to attach</param>
		/// <returns>The new (or exisitng) <see cref="RoleInstance"/></returns>
		public RoleInstance EnsureRoleInstance(Role factRole, ObjectInstance instance)
		{
			RoleInstance roleInstance = FindRoleInstance(factRole);
			ObjectInstance existingInstance = null;
			bool sameInstance = false;
			if (roleInstance != null)
			{
				sameInstance = (existingInstance = roleInstance.ObjectInstance) == instance;
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
				if (!sameInstance)
				{
					roleInstance.Delete();
				}
#endif // !ROLEINSTANCE_ROLEPLAYERCHANGE
			}
			if (!sameInstance &&
				existingInstance != (instance = ObjectInstance.GetTypedSubtypeInstance(instance, factRole.RolePlayer)))
			{
#if ROLEINSTANCE_ROLEPLAYERCHANGE
				if (roleInstance == null)
				{
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
				roleInstance = new RoleInstance(factRole, instance);
				roleInstance.FactInstance = this;
#if ROLEINSTANCE_ROLEPLAYERCHANGE
				}
				else
				{
					roleInstance.ObjectInstance = instance;
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
				TooFewFactRoleInstancesError tooFew = TooFewFactRoleInstancesError;
				if (tooFew != null)
				{
					yield return tooFew;
				}

				ObjectifyingInstanceRequiredError objectifyingInstance = ObjectifyingInstanceRequiredError;
				if (objectifyingInstance != null)
				{
					yield return objectifyingInstance;
				}

				LinkedElementCollection<RoleInstance> roleInstances = this.RoleInstanceCollection;
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
			ValidateTooFewFactRoleInstancesError(notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewFactRoleInstancesError);
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasFactInstance.FactInstanceDomainRoleId };
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
			/// <param name="factInstance">The <see cref="FactInstance"/> to modify</param>
			/// <param name="oldName">The old name to record</param>
			/// <param name="newName">The new name to record</param>
			public static void SetName(FactInstance factInstance, string oldName, string newName)
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
		private void OnFactInstanceNameChanged()
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
		/// Delayed validation handler used to updated the name of an <see cref="ObjectInstance"/>
		/// </summary>
		protected static void DelayValidateNamePartChanged(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FactInstance factInstance = (FactInstance)element;
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
				factInstance.OnFactInstanceNameChanged();
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
		private static string GenerateInstanceName(FactType factType, FactInstance instance)
		{
			IReading reading = factType.GetDefaultReading();
			bool fakeReading = !(reading is Reading);
			string listSeparator = fakeReading ? CultureInfo.CurrentCulture.TextInfo.ListSeparator + " " : null;
			IList<RoleBase> readingRoles = reading.RoleCollection;
			int lastRoleIndex = readingRoles.Count - 1;
			IList<RoleInstance> factRoleInstances = (instance != null) ? instance.RoleInstanceCollection : null;
			return Reading.ReplaceFields(
				reading.Text,
				delegate(int replaceIndex)
				{
					RoleInstance roleInstance = (factRoleInstances != null ) ? FindRoleInstance(factRoleInstances, readingRoles[replaceIndex].Role) : null;
					string replacement = (roleInstance != null) ?
						roleInstance.ObjectInstance.Name :
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
			// unique names on the generated ObjectInstance name.
		}
		/// <summary>
		/// Reset the name for each <see cref="FactInstance"/> in the <paramref name="store"/>
		/// </summary>
		/// <param name="store">Context <see cref="Store"/> to reset names for.</param>
		public static void InvalidateNames(Store store)
		{
			foreach (FactInstance factInstance in store.ElementDirectory.FindElements<FactInstance>(false))
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
		#region TooFewFactRoleInstancesError Validation
		/// <summary>
		/// Validator callback for TooFewFactRoleInstancesError
		/// </summary>
		private static void DelayValidateTooFewFactRoleInstancesError(ModelElement element)
		{
			(element as FactInstance).ValidateTooFewFactRoleInstancesError(null);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewFactRoleInstancesError
		/// should be attached to the FactInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewFactRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasError = false;
				LinkedElementCollection<RoleInstance> roleInstances = RoleInstanceCollection;
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
				TooFewFactRoleInstancesError tooFew = this.TooFewFactRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = new TooFewFactRoleInstancesError(Partition);
						tooFew.FactInstance = this;
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
			// Internal justification: The error being validated is on the FactInstance,
			// but there is a corresponding error on the ObjectInstance which is validated
			// in response to the same rules. Make the delayvalidation routines internal so
			// that we can call them from a single location without duplicating delete and
			// roleplayer changed rules.
			((FactInstance)element).ValidateObjectifyingInstanceRequiredError(null);
		}
		private void ValidateObjectifyingInstanceRequiredError(INotifyElementAdded notifyAdded)
		{
			// Validate that an objectifying instance is specified and that the instance
			// has attached role players. No specified role players results in
			// ObjectifyingInstanceRequiredError instead of TooFewEntityRoleInstancesError
			if (!this.IsDeleted)
			{
				FactType factType;
				ObjectType entityType;
				ObjectInstance objectifyingInstance = null;
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
					ObjectInstance entityInstance = objectifyingInstance.SupertypeInstance ?? objectifyingInstance;
					UniquenessConstraint pid;
					if (entityInstance != null &&
						null != (pid = entityInstance.ObjectType?.PreferredIdentifier))
					{
						hasError = true;
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						int pidRoleCount = pidRoles.Count;
						for (int i = 0; i < pidRoleCount; ++i)
						{
							if (null != entityInstance.FindIdentifyingRoleInstance(pidRoles[i]))
							{
								hasError = false;
								break;
							}
						}
					}
				}

				ObjectifyingInstanceRequiredError error = this.ObjectifyingInstanceRequiredError;
				if (hasError)
				{
					if (error == null)
					{
						error = new ObjectifyingInstanceRequiredError(Partition);
						error.FactInstance = this;
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
					throw new InvalidOperationException(ResourceStrings.ModelExceptionFactInstanceInconsistentRoleOwners);
				}
			}
			else if (existingFactType != null)
			{
				role.FactType = existingFactType;
			}
		}

		private void EnsureNonDuplicateRoleInstance(FactInstanceHasRoleInstance link)
		{
			Role role = link.RoleInstance.Role;
			if (role != null)
			{
				ReadOnlyCollection<FactInstanceHasRoleInstance> currentLinks = FactInstanceHasRoleInstance.GetLinksToRoleInstanceCollection(this);
				int linkCount = currentLinks.Count;
				for (int i = 0; i < linkCount; ++i)
				{
					FactInstanceHasRoleInstance currentLink = currentLinks[i];
					if (link != currentLink && role == currentLink.RoleInstance.Role)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionFactInstanceEnforceRoleUniqueness);
					}
				}
			}
		}
		#endregion
		#region FactInstance Rules
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// If a Role is added to a FactType's role collection, all FactInstances of that FactType
		/// should be revalidated to ensure that they form a complete instance of the FactType
		/// </summary>
		private static void FactTypeRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType parent = link.FactType;
			foreach (FactInstance factInstance in parent.FactInstanceCollection)
			{
				FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactRoleInstancesError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// If a Role is removed from a FactType's role collection, it will
		/// automatically propagate and destroy any role instances. This rule
		/// will force deletion of any FactInstances which no longer have
		/// any RoleInstances.
		/// </summary>
		private static void FactTypeRoleDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;
			if (!factType.IsDeleted)
			{
				LinkedElementCollection<FactInstance> factInstances = factType.FactInstanceCollection;
				int factInstanceCount = factInstances.Count;
				for (int i = 0; i < factInstanceCount; ++i)
				{
					FactInstance factInstance = factInstances[i];
					if (!factInstance.IsDeleted)
					{
						FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactRoleInstancesError);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasFactInstance)
		/// If a FactInstance with existing RoleInstances is added
		/// to a FactType, make sure all of the RoleInstance Roles
		/// have the same FactType as a parent
		/// Automatically add a corresponding <see cref="ObjectInstance"/> to
		/// an objectification with an internal preferred identifier in the objectified
		/// <see cref="FactType"/>
		/// </summary>
		private static void FactInstanceAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasFactInstance link = (FactTypeHasFactInstance)e.ModelElement;
			FactType factType = link.FactType;
			SubtypeFact subtypeFact;
			if (factType.ImpliedByObjectification != null ||
				(null != (subtypeFact = factType as SubtypeFact) &&
				subtypeFact.ProvidesPreferredIdentifier))
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionFactInstanceDirectionPopulationOfImpliedInstances);
			}

			// Basic structural check
			FactInstance factInstance = link.FactInstance;
			LinkedElementCollection<RoleInstance> roleInstances = factInstance.RoleInstanceCollection;
			int roleInstanceCount = roleInstances.Count;
			for (int i = 0; i < roleInstanceCount; ++i)
			{
				// Check each role being related to the FactType
				factInstance.EnsureConsistentRoleOwner(factType, roleInstances[i].Role);
			}
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactRoleInstancesError);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);

			// Objectification instance handling
			ObjectType entityType;
			UniquenessConstraint pid;
			if (null != (entityType = factType.NestingType) &&
				null != (pid = entityType.ResolvedPreferredIdentifier))
			{
				LinkedElementCollection<FactType> pidFactTypes;
				FactType identifierFactType;
				if (pid.PreferredIdentifierFor == entityType &&
					pid.IsInternal &&
					1 == (pidFactTypes = pid.FactTypeCollection).Count &&
					(identifierFactType = pidFactTypes[0]) == factType)
				{
					// Create a new EntityInstance, populate it based on the existing
					// population in the FactInstance, and associate it with
					// the FactInstance.
					ObjectInstance entityInstance = new ObjectInstance(factType.Partition);
					entityInstance.ObjectType = entityType;
					entityInstance.ObjectifiedInstance = factInstance;
				}
				else
				{
					// External identifier, this FactInstance needs an explicit ObjectificationInstance
					FrameworkDomainModel.DelayValidateElement(link.FactInstance, DelayValidateObjectifyingInstanceRequiredError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactInstanceHasRoleInstance)
		/// If a RoleInstance with existing roles is added
		/// to a FactInstance, make sure all of the
		/// roles have the same FactType as a parent and that a RoleInstance
		/// for the given role doesn't already exist
		/// </summary>
		private static void FactRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			FactInstanceHasRoleInstance link = e.ModelElement as FactInstanceHasRoleInstance;
			FactInstance factInstance = link.FactInstance;
			FactType factType = factInstance.FactType;

			// Basic structural verification
			RoleInstance factRoleInstance = link.RoleInstance;
			Role role = factRoleInstance.Role;
			factInstance.EnsureConsistentRoleOwner(factType, role);
			factInstance.EnsureNonDuplicateRoleInstance(link);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactRoleInstancesError);
			FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);
		}
		/// <summary>
		/// DeleteRule: typeof(FactInstanceHasRoleInstance), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// If a RoleInstance is removed, revalidate the FactInstance
		/// to ensure complete population of its roles. If the RoleInstance
		/// removed was the last one, remove the FactInstance.
		/// </summary>
		private static void RoleInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			FactInstanceHasRoleInstance link = (FactInstanceHasRoleInstance)e.ModelElement;
			FactInstance factInstance = link.FactInstance;
			if (!factInstance.IsDeleted)
			{
				if (factInstance.RoleInstanceCollection.Count == 0)
				{
					factInstance.Delete();
				}
				else
				{
					// Structural check
					FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateTooFewFactRoleInstancesError);
					FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);

					// See if this is part of a preferred identifier for a role instance.
					FactType factType;
					UniquenessConstraint pid;
					RoleInstance linkRoleInstance = link.RoleInstance;
					Role role = linkRoleInstance.Role; // Note that role will not be null because both steps are links
					if (!role.IsDeleted &&
						null != (factType = role.FactType))
					{
						ObjectInstance identifiedInstance = null;
						Objectification objectification = factType.Objectification;
						if (null != (pid = objectification?.NestingType.PreferredIdentifier) &&
							pid.RoleCollection.Contains(role))
						{
							identifiedInstance = factInstance.ObjectifyingInstance;
						}
						else
						{
							Role oppositeRole = role.OppositeOrUnaryRole as Role; // Identity roles in link fact have an implied population
							if (oppositeRole != null)
							{
								RoleInstance identifyingRoleInstance = null;
								if (null != (pid = role?.RolePlayer.PreferredIdentifier))
								{
									if (pid.RoleCollection.Contains(role))
									{
										// Look at the role instance for the opposite role player
										identifyingRoleInstance = factInstance.FindRoleInstance(oppositeRole);
									}
								}
								else if (oppositeRole != role && // Unary case, will hit the first branch
									null != (pid = oppositeRole.RolePlayer?.PreferredIdentifier) &&
									pid.RoleCollection.Contains(role))
								{
									identifyingRoleInstance = factInstance.FindRoleInstance(role);
								}
								if (identifyingRoleInstance != null)
								{
									identifiedInstance = identifyingRoleInstance.ObjectInstance;
								}
							}
							if (identifiedInstance != null)
							{
								// UNDONE: NOW Stopped here. Kill EntityTypeRoleInstance
								// This has to call whatever EntityTypeRoleInstanceDeleted would have run before.
								// Consider moving those validations into FactInstance instead of EntityInstance, which
								// is basically empty at this point.
							}
						}
					}

#if FALSE // UNDONE: NOW Dead code
					// ObjectificationInstance handling, manage the implied entityInstance
					EntityInstance entityInstance;
					ObjectType entityType;
					UniquenessConstraint pid;
					LinkedElementCollection<FactType> pidFactTypes;
					RoleInstance linkRoleInstance = link.RoleInstance;
					Role role = linkRoleInstance.Role; // Note that role will not be null because both steps are links
					FactType identifierFactType;
					if (!role.IsDeleted &&
						null != (entityInstance = factInstance.ObjectifyingInstance as EntityInstance) &&
						null != (factType = factInstance.FactType) &&
						null != (entityType = entityInstance.EntityType) &&
						null != (pid = entityType.PreferredIdentifier) &&
						pid.IsInternal &&
						1 == (pidFactTypes = pid.FactTypeCollection).Count &&
						(identifierFactType = pidFactTypes[0]) == factType)
					{
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						EntityTypeRoleInstance roleInstance;
						if (pidRoles.Contains(role) &&
							null != (roleInstance = entityInstance.FindRoleInstance(role)) &&
							!roleInstance.IsDeleting &&
							// The final instance check allows a new instance to be attached to the role before this rule fires
							roleInstance.ObjectInstance == linkRoleInstance.ObjectInstance)
						{
							roleInstance.Delete();
						}
					}
#endif
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
				foreach (FactInstance instance in ((FactType)e.ModelElement).FactInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ObjectInstance)
		/// Update generated names when the role fields change predicate changes
		/// </summary>
		private static void ObjectInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ObjectInstance.NameChangedDomainPropertyId)
			{
				foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection((ObjectInstance)e.ModelElement))
				{
					FrameworkDomainModel.DelayValidateElement(roleInstance.FactInstance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleInstance)
		/// Update generated names when the associated instances change
		/// </summary>
		private static void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			FactInstance factInstance;
			if (e.DomainRole.Id == RoleInstance.ObjectInstanceDomainRoleId && // Note that the opposite role change is blocked in a RoleInstance rule
				null != (factInstance = ((RoleInstance)e.ElementLink).FactInstance))
			{
				FrameworkDomainModel.DelayValidateElement(factInstance, DelayValidateNamePartChanged);
				// UNDONE: NOW Kill EntityTypeRoleInstance EntityInstance names will also change. Might handle in DelayValidateNamePartChanged.
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(FactInstanceHasRoleInstance)
		/// Block role instance from moving to different instance
		/// </summary>
		private static void FactInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionFactInstanceEnforceFixedRoleInstance);
		}
		#endregion // FactInstance Rules
	}
	#endregion // FactInstance class
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

		private sealed class RoleInstanceKeyProvider : IKeyProvider<ObjectInstance, RoleInstance>
		{
			private static RoleInstanceKeyProvider provider;

			public static RoleInstanceKeyProvider ProviderInstance
			{
				get
				{
					return provider ?? (provider = new RoleInstanceKeyProvider());
				}
			}

			#region IKeyProvider<ObjectInstance, RoleInstance> Members
			ObjectInstance IKeyProvider<ObjectInstance, RoleInstance>.GetKey(RoleInstance value)
			{
				return value.ObjectInstance;
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
			// element corresponds to an entire FactInstance row (complete ones only) for spanning
			// constraints, or a subset of that population for non-spanning constraints.
			// 4) Extending this to external constraints requires even more work as we would need
			// to synthesize a joined view on the roles/roleinstances (not just use a subset of rows
			// from the FactInstance population). We would also require full join path and join type
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
				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToObjectInstanceCollection(this);
				int roleInstanceCount = roleInstances.Count;
				if (singleRoleConstraint != null)
				{
					HashSet<ObjectInstance, RoleInstance> population = null;
					for (int i = 0; i < roleInstanceCount; ++i)
					{
						RoleInstance roleInstance = roleInstances[i];
						ObjectInstance boundInstance = roleInstance.ObjectInstance;
						if (population == null)
						{
							population = new HashSet<ObjectInstance, RoleInstance>(RoleInstanceKeyProvider.ProviderInstance);
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
								RoleInstanceHasPopulationUniquenessError newLink = new RoleInstanceHasPopulationUniquenessError(roleInstance, error);
								if (notifyAdded != null)
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
								new RoleInstanceHasPopulationUniquenessError(knownInstances[j], error);
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
			// Note that role changes are blocked by the corresponding rule in ObjectInstance.RoleInstanceRolePlayerChangedRule
			if (e.DomainRole.Id == RoleInstance.ObjectInstanceDomainRoleId && e.OldRolePlayer != e.NewRolePlayer)
			{
				FrameworkDomainModel.DelayValidateElement(((RoleInstance)e.ElementLink).Role, DelayValidatePopulationUniquenessError);
			}
		}
		#endregion // Role Rules
	}
	#endregion // Role class
	#region ObjectInstance class
	partial class ObjectInstance : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ObjectTypeHasObjectInstance.ObjectInstanceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
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
				// Instance style (value, entity, subtype entity) specific error checks
				ObjectType objectType = this.ObjectType;
				if (objectType != null)
				{
					if (objectType.DataType != null)
					{
						// Value instance errors
						CompatibleValueInstanceError badValue = CompatibleValueInstanceError;
						if (badValue != null)
						{
							yield return badValue;
						}
					}
					else if (objectType.PreferredIdentifier != null)
					{
						// Entity instance errors
						TooFewEntityRoleInstancesError tooFew = TooFewEntityRoleInstancesError;
						if (tooFew != null)
						{
							yield return tooFew;
						}
					}
					else if (objectType.ReferenceSchemeError != null)
					{
						// Entity subtype instance errors
						// UNDONE: NOW Kill EntityTypeRoleInstance When we can transition instance styles there will need to be an error for an unconnected supertype instance
					}
				}

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

			ObjectType objectType = this.ObjectType;
			if (objectType != null)
			{
				if (objectType.DataType != null)
				{
					ValidateCompatibleValueInstanceError(notifyAdded);
				}
				else if (objectType.PreferredIdentifier != null)
				{
					ValidateTooFewEntityTypeRoleInstancesError(notifyAdded);
				}
			}
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

			ObjectType objectType = this.ObjectType;
			if (objectType != null)
			{
				if (objectType.DataType != null)
				{
					FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleValueInstanceError);
				}
				else if (objectType.PreferredIdentifier != null)
				{
					FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityRoleInstancesError);
				}
			}
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
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
						foreach (ObjectInstance instance in element.ValueType.ObjectInstanceCollection)
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

						// Regarding ObjectType.InvariantDefaultValue and Role.InvariantDefaultValue:
						// This fixup (and ValueConstraint.InvariantValueFixupListener) were added when InvariantValue support
						// was first added, so the normal state was to not have an invariant value on file load for previous files.
						// Role.DefaultValue and ObjectType.DefaultValue, on the other hand, were introduced in the same commit as
						// the invariant forms, so we have never written a file without the paired values (when needed). Given the
						// startup cost of calling Role.WalkDescendedValueRoles for all value types and the lack of benefit for files
						// created by the tool, we do not attempt to backfill invariant default values in this deserialize phase.
					}
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Automatic Name Generation
		private string myGeneratedName = string.Empty;
		private string myGeneratedIdentifierName = string.Empty;
		/// <summary>
		/// Provide storage for the generated <see cref="P:Name"/>
		/// </summary>
		private string GeneratedName
		{
			get
			{
				// The value-based name is not generated or stored.
				return ValueType != null ? NormalizedValue : myGeneratedName;
			}
			set
			{
				if (ValueType == null)
				{
					myGeneratedName = value;
				}
			}
		}
		/// <summary>
		/// Provide storage for the generated <see cref="P:IdentifierName"/>
		/// </summary>
		private string GeneratedIdentifierName
		{
			get
			{
				// The value-based name is not generated or stored.
				return ValueType != null ? NormalizedValue : myGeneratedIdentifierName;
			}
			set
			{
				if (ValueType == null)
				{
					myGeneratedIdentifierName = value;
				}
			}
		}
		partial class NamePropertyHandler
		{
			/// <summary>
			/// Add a Name modification to the transaction log
			/// without reading the current Name, which forces it to regenerated
			/// </summary>
			/// <param name="objectInstance">The <see cref="ObjectInstance"/> to modify</param>
			/// <param name="oldName">The old name to record</param>
			/// <param name="newName">The new name to record</param>
			public static void SetName(ObjectInstance objectInstance, string oldName, string newName)
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
			/// <param name="objectInstance">The <see cref="ObjectInstance"/> to modify</param>
			/// <param name="oldIdentifierName">The old name to record</param>
			/// <param name="newIdentifierName">The new name to record</param>
			public static void SetIdentifierName(ObjectInstance objectInstance, string oldIdentifierName, string newIdentifierName)
			{
				objectInstance.GeneratedIdentifierName = newIdentifierName;
				Instance.ValueChanged(objectInstance, oldIdentifierName, newIdentifierName);
			}
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:Name"/> property
		/// </summary>
		private string GenerateName()
		{
			ObjectType entityType = this.EntityType;
			return entityType != null ? ObjectInstance.GetDisplayString(this, entityType, false) : null;
		}
		/// <summary>
		/// Generate the current value for the <see cref="P:IdentifierName"/> property
		/// </summary>
		private string GenerateIdentifierName()
		{
			ObjectType entityType = this.EntityType;
			return entityType != null ? ObjectInstance.GetDisplayString(this, entityType, true) : null;
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
		private string NormalizedValue
		{
			get
			{
				ObjectType valueType = this.ValueType;
				return valueType != null ? valueType.DataType.NormalizeDisplayText(Value, InvariantValue) : Value;
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
		private void OnObjectInstanceNameChanged()
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
		/// Delayed validation handler used to updated the name of an <see cref="ObjectInstance"/>
		/// </summary>
		private static void DelayValidateNamePartChanged(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ObjectInstance objectInstance = (ObjectInstance)element;
				bool logEmptyName = false;
				bool logEmptyIdentifierName = false;
				if (objectInstance.HasGeneratedNames)
				{
					string oldGeneratedName = objectInstance.GeneratedName;
					if (!string.IsNullOrEmpty(oldGeneratedName))
					{
						string newGeneratedName = objectInstance.GenerateName();
						if (newGeneratedName == oldGeneratedName)
						{
							return;
						}
						else
						{
							// Force a change in the transaction log so that we can
							// undo the generated name as needed
							NamePropertyHandler.SetName(objectInstance, oldGeneratedName, newGeneratedName);
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
					string oldGeneratedIdentifierName = objectInstance.GeneratedIdentifierName;
					if (!string.IsNullOrEmpty(oldGeneratedIdentifierName))
					{
						// We don't use this name directly, clear it so that it can regenerate on demand
						IdentifierNamePropertyHandler.SetIdentifierName(objectInstance, oldGeneratedIdentifierName, "");
					}
					else
					{
						logEmptyIdentifierName = true;
					}
				}

				// Keep going with an empty name. Any callback to the Name
				// property will generate on demand
				foreach (ModelError error in (objectInstance as IModelErrorOwner).GetErrorCollection(ModelErrorUses.None))
				{
					if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
					{
						error.GenerateErrorText();
					}
				}

				// Since the name changed, tell any RoleInstances which use it to revalidate since their name probably changed
				foreach (RoleInstance roleInstance in RoleInstance.GetLinksToRoleCollection(objectInstance))
				{
					ObjectInstance relatedEntityInstance = roleInstance.IdentifiedEntityInstance;
					if (relatedEntityInstance != null && relatedEntityInstance != objectInstance)
					{
						FrameworkDomainModel.DelayValidateElement(relatedEntityInstance, DelayValidateNamePartChanged);
					}
				}

				// Subtype instances need refreshing
				foreach (ObjectInstance subtypeInstance in objectInstance.EntitySubtypeInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(subtypeInstance, DelayValidateNamePartChanged);
				}

				if (logEmptyName && string.IsNullOrEmpty(objectInstance.GeneratedName))
				{
					// If the name has not been regenerated on demand, then log an empty entry so
					// that an undo clears any non-logged generated name
					NamePropertyHandler.SetName(objectInstance, "", "");
				}
				if (logEmptyIdentifierName && string.IsNullOrEmpty(objectInstance.GeneratedIdentifierName))
				{
					// Same comments as name, we need a history to revert the state on undo
					IdentifierNamePropertyHandler.SetIdentifierName(objectInstance, "", "");
				}
				objectInstance.OnObjectInstanceNameChanged();
			}
		}
		/// <summary>
		/// Enable name generation for entity instances
		/// </summary>
		private bool HasGeneratedNames
		{
			get
			{
				ObjectType objectType = this.ObjectType;
				return objectType != null && objectType.DataType == null; // Names are generated for an entity type
			}
		}
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// Do nothing here. The base calls SetUniqueName, but we don't enforce
			// unique names on the generated ObjectInstance name.
		}
		/// <summary>
		/// Reset the name for each <see cref="ObjectInstance"/> with a cached name in the <paramref name="store"/>
		/// </summary>
		/// <param name="store">Context <see cref="Store"/> to reset names for.</param>
		public static void InvalidateNames(Store store)
		{
			IElementDirectory directory = store.ElementDirectory;
			foreach (ObjectInstance entityInstance in directory.FindElements<ObjectInstance>(false))
			{
				if (entityInstance.EntityType != null)
				{
					FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateNamePartChanged);
				}
			}
		}
		#endregion // Automatic Name Generation
		#region Base overrides
		/// <summary>
		/// Display the Value for a value instance and the calculated Name for an entity instance
		/// </summary>
		public override string ToString()
		{
			return ValueType != null ? NormalizedValue : Name;
		}
		#endregion // Base overrides
		#region General ObjectInstance Helper Methods
		/// <summary>
		/// Return a non-null ObjectType if the <see cref="ObjectType"/> associated with this
		/// instance is an entity type. Otherwise, return null.
		/// </summary>
		public ObjectType EntityType
		{
			get
			{
				ObjectType objectType = this.ObjectType;
				return objectType != null && objectType.DataType == null ? objectType : null;
			}
		}
		/// <summary>
		/// Return a non-null ObjectType if the <see cref="ObjectType"/> associated with this
		/// instance is a value type. Otherwise, return null.
		/// </summary>
		public ObjectType ValueType
		{
			get
			{
				ObjectType objectType = this.ObjectType;
				return objectType != null && objectType.DataType != null ? objectType : null;
			}
		}
		/// <summary>
		/// Returns the display string for the given <paramref name="objectInstance"/>, or the
		/// nested tuple structure for the <paramref name="parentObjectType"/> if the instance is not provided.
		/// </summary>
		/// <param name="objectInstance">Instance to format into a display string. Can be null.</param>
		/// <param name="parentObjectType">Parent Type of the instance.</param>
		/// <param name="ignoreObjectification">Set to true to generate a name based solely on the identifier instead of using the objectified FactType.</param>
		/// <returns>String representation of the instance.</returns>
		public static string GetDisplayString(ObjectInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification)
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
		public static string GetDisplayString(FactInstance factInstance, FactType parentFactType)
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
		public static string GetDisplayString(ObjectInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
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
			OutputIdentityTag identityTagger = CreateIdentityTagger(objectInstance);
			string listSeparator = null;
			string retVal = (parentObjectType == null) ? "" : RecurseObjectInstanceValue(objectInstance, parentObjectType, ignoreObjectification, false, null, null, ref listSeparator, ref outputText, ref identityTagger, formatProvider, valueTextFormat, valueNonTextFormat, false, null);
			if (identityTagger != null)
			{
				identityTagger(ref outputText);
			}
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
		public static string GetDisplayString(FactInstance factInstance, FactType parentFactType, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat)
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
			OutputIdentityTag identityTagger = null;
			string listSeparator = null;
			string retVal = (parentFactType == null) ? "" : RecurseObjectInstanceValue(null, null, false, false, factInstance, parentFactType, ref listSeparator, ref outputText, ref identityTagger, formatProvider, valueTextFormat, valueNonTextFormat, false, null);
			retVal = (outputText != null) ? outputText.ToString() : retVal;
			return (retVal == null) ? "" : retVal.Trim();
		}

		private delegate void OutputIdentityTag(ref StringBuilder outputText);
		private static OutputIdentityTag CreateIdentityTagger(ObjectInstance taggedInstance)
		{
			string identityTag;
			if (taggedInstance == null || string.IsNullOrEmpty(identityTag = taggedInstance.IdentityTag))
			{
				return null;
			}
			return delegate (ref StringBuilder outputText)
			{
				if (outputText == null)
				{
					outputText = new StringBuilder();
				}
				outputText.Append("[");
				outputText.Append(identityTag);
				outputText.Append("]");
			};
		}
		/// <summary>
		/// Combine two identity taggers by adding parentheses for the inner one.
		/// It is the responsibility of the caller to close the parentheses.
		/// </summary>
		private static OutputIdentityTag NestSimpleIdentityTagger(OutputIdentityTag outerTag, OutputIdentityTag innerTag)
		{
			return delegate (ref StringBuilder outputText)
			{
				outerTag(ref outputText);
				outputText.Append("=(");
				innerTag(ref outputText);
			};
		}

		private static string RecurseObjectInstanceValue(ObjectInstance objectInstance, ObjectType parentObjectType, bool ignoreObjectification, bool nestedLeadValue, FactInstance factInstance, FactType parentFactType, ref string listSeparator, ref StringBuilder outputText, ref OutputIdentityTag identityTagger, IFormatProvider formatProvider, string valueTextFormat, string valueNonTextFormat, bool outerGrouping, Predicate<ObjectType> stackCheck)
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
				string valueText = blankValueText;
				if (objectInstance != null)
				{
					valueText = dataType.NormalizeDisplayText(objectInstance.Value, objectInstance.InvariantValue);
					if (valueTextFormat != null && parentObjectType.DataType is TextDataType)
					{
						valueText = string.Format(formatProvider ?? dataType.CurrentCulture, valueTextFormat, valueText);
					}
					else if (valueNonTextFormat != null)
					{
						valueText = string.Format(formatProvider ?? dataType.CurrentCulture, valueNonTextFormat, valueText);
					}
				}

				if (identityTagger != null && !string.IsNullOrEmpty(valueText))
				{
					identityTagger(ref outputText);
					outputText.Append("=");
					identityTagger = null;
				}

				if (outputText != null)
				{
					outputText.Append(valueText);
					return null;
				}
				return valueText;
			}
			else if (parentObjectType != null && objectInstance == null && stackCheck != null && stackCheck(parentObjectType))
			{
				if (outputText != null)
				{
					outputText.Append(blankValueText);
					return null;
				}
				return blankValueText;
			}
			else
			{
				IList<Role> roles = null;
				int roleCount = 0;
				FactType objectifiedFactType = null;
				ObjectInstance entityInstance = null;
				Predicate<ObjectType> recurseStackCheck = null;
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
							return RecurseObjectInstanceValue(
								null,
								null,
								false,
								false,
								(objectInstance != null) ? objectInstance.ObjectifiedInstance : null,
								objectifiedFactType,
								ref listSeparator,
								ref outputText,
								ref identityTagger,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								outerGrouping,
								stackCheck);
						}
						else if (preferredFor != null && preferredFor != parentObjectType)
						{
							// The subtype is associated with an objectified facttype. Show
							// the identifier from the supertype with the objectified FactType

							// When there is a supertype we always use the identity tag from the supertype instance.
							// This should not be set in this case, so this is primarily defensive.
							identityTagger = null;

							if (outputText == null)
							{
								outputText = new StringBuilder();
							}
							if (!outerGrouping)
							{
								outputText.Append("(");
							}
							ObjectInstance supertypeInstance = objectInstance?.SupertypeInstance;
							OutputIdentityTag supertypeTagger = CreateIdentityTagger(supertypeInstance);
							RecurseObjectInstanceValue(
								supertypeInstance,
								preferredFor,
								false,
								true,
								null,
								null,
								ref listSeparator,
								ref outputText,
								ref supertypeTagger,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								false,
								recurseStackCheck ?? (recurseStackCheck = CreateRecursiveIdentifierStackCheck(preferredFor, stackCheck)));
							outputText.Append(listSeparator ?? (listSeparator = GetListSeparator(formatProvider)));
							RecurseObjectInstanceValue(
								null,
								null,
								false,
								false,
								(objectInstance != null) ? objectInstance.ObjectifiedInstance : null,
								objectifiedFactType,
								ref listSeparator,
								ref outputText,
								ref supertypeTagger,
								formatProvider,
								valueTextFormat,
								valueNonTextFormat,
								true,
								stackCheck);

							if (supertypeTagger != null)
							{
								supertypeTagger(ref outputText);
							}
							if (!outerGrouping)
							{
								outputText.Append(")");
							}
							return null;
						}
					}
					if (preferredFor != null && preferredFor != parentObjectType)
					{
						identityTagger = null; // See previous comment for this line of code
						ObjectInstance supertypeInstance = objectInstance?.SupertypeInstance;
						OutputIdentityTag supertypeTagger = CreateIdentityTagger(supertypeInstance);
						string retVal = RecurseObjectInstanceValue(
							supertypeInstance,
							preferredFor,
							false,
							nestedLeadValue,
							null,
							null,
							ref listSeparator,
							ref outputText,
							ref supertypeTagger,
							formatProvider,
							valueTextFormat,
							valueNonTextFormat,
							outerGrouping,
							recurseStackCheck ?? (recurseStackCheck = CreateRecursiveIdentifierStackCheck(preferredFor, stackCheck)));
						if (supertypeTagger != null)
						{
							supertypeTagger(ref outputText);
							return null;
						}
						return retVal;
					}
					entityInstance = objectInstance == null ? null : (objectInstance.SupertypeInstance == null ? objectInstance : null);
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
							roleInstance = entityInstance.FindIdentifyingRoleInstance(role);
						}
						else if (factInstance != null)
						{
							roleInstance = factInstance.FindRoleInstance(role);
						}
						ObjectType rolePlayer = role.RolePlayer;
						ObjectInstance recurseInstance = (roleInstance != null) ? roleInstance.ObjectInstance : null;

						OutputIdentityTag activeTagger = identityTagger;
						OutputIdentityTag recurseTagger = CreateIdentityTagger(recurseInstance);
						bool taggerCombined = false;
						if (recurseTagger != null)
						{
							if (identityTagger != null)
							{
								// We're rendering an edge case case that shows the single-valued identifier without parentheses.
								// If there is a tag on both the current output and the nested output then we inject the
								// parentheses so we get '[tag1]=([tag2]=val)' instead of '[tag1]=[tag2]=val'
								// We don't know if the recursion is going to use the tagger, so we need to be prepared for
								// both the used and unused scenarios.
								activeTagger = NestSimpleIdentityTagger(identityTagger, recurseTagger);
								taggerCombined = true;
							}
							else
							{
								activeTagger = recurseTagger;
							}
						}

						string retVal = RecurseObjectInstanceValue(
							(roleInstance != null) ? roleInstance.ObjectInstance : null,
							rolePlayer,
							false,
							nestedLeadValue,
							null,
							null,
							ref listSeparator,
							ref outputText,
							ref activeTagger,
							formatProvider,
							valueTextFormat,
							valueNonTextFormat,
							false,
							recurseStackCheck ?? (recurseStackCheck = CreateRecursiveIdentifierStackCheck(rolePlayer, stackCheck)));

						if (activeTagger != null)
						{
							activeTagger(ref outputText);
						}
						identityTagger = null;
						recurseTagger = null;

						if (taggerCombined)
						{
							outputText.Append(")");
						}

						if (objectifiedFactType == null)
						{
							return retVal;
						}
					}
					else
					{
						LinkedElementCollection<RoleInstance> factRoleInstances = null;
						int roleInstanceCount = 0;
						if (factInstance != null)
						{
							factRoleInstances = factInstance.RoleInstanceCollection;
							roleInstanceCount = factRoleInstances.Count;
						}
						if (outputText == null)
						{
							outputText = new StringBuilder();
						}
						if (identityTagger != null)
						{
							identityTagger(ref outputText);
							outputText.Append("=");
							identityTagger = null;
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
							bool unaryRole = parentObjectType != null && role.FactType.UnaryPattern != UnaryValuePattern.NotUnary;
							if (i != 0)
							{
								outputText.Append(listSeparator);
							}

							if (entityInstance != null)
							{
								matchInstance = entityInstance.FindIdentifyingRoleInstance(role);
							}
							else if (roleInstanceCount != 0)
							{
								for (int j = 0; j < roleInstanceCount; ++j)
								{
									RoleInstance instance = factRoleInstances[j];
									if (instance.Role == role)
									{
										matchInstance = instance;
										break;
									}
								}
							}

							if (unaryRole)
							{
								outputText.Append(matchInstance != null ? "\x2713" : blankValueText);
							}
							else
							{
								ObjectType rolePlayer = role.RolePlayer;
								ObjectInstance recurseInstance = (matchInstance != null) ? matchInstance.ObjectInstance : null;
								OutputIdentityTag recurseTagger = CreateIdentityTagger(recurseInstance);
								RecurseObjectInstanceValue(
									recurseInstance,
									rolePlayer,
									false,
									i == 0 && !outerGrouping,
									null,
									null,
									ref listSeparator,
									ref outputText,
									ref recurseTagger,
									formatProvider,
									valueTextFormat,
									valueNonTextFormat,
									false,
									recurseStackCheck ?? (recurseStackCheck = CreateRecursiveIdentifierStackCheck(rolePlayer, stackCheck)));

								if (recurseTagger != null)
								{
									recurseTagger(ref outputText);
								}
							}
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
					RecurseObjectInstanceValue(
						null,
						null,
						false,
						false,
						(entityInstance != null) ? entityInstance.ObjectifiedInstance : null,
						objectifiedFactType,
						ref listSeparator,
						ref outputText,
						ref identityTagger,
						formatProvider,
						valueTextFormat,
						valueNonTextFormat,
						true,
						stackCheck);
					if (!outerGrouping)
					{
						outputText.Append(")");
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Helper to create a delegate to determine if an identifier is recursively defined. If we have
		/// a recursive identifier then we do not continue past an empty instance.
		/// </summary>
		/// <param name="rolePlayer">The current role player.</param>
		/// <param name="contextStackCheck">A context delegate from higher up in the object tree.</param>
		/// <returns>New stack check.</returns>
		private static Predicate<ObjectType> CreateRecursiveIdentifierStackCheck(ObjectType rolePlayer, Predicate<ObjectType> contextStackCheck)
		{
			return rolePlayer == null ?
				contextStackCheck :
				delegate(ObjectType recursiveRolePlayer)
				{
					return rolePlayer == recursiveRolePlayer || (contextStackCheck != null && contextStackCheck(recursiveRolePlayer));
				};
		}
		private static string GetListSeparator(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
			return cultureInfo.TextInfo.ListSeparator + " ";
		}
		#endregion // General ObjectInstance Helper Methods
		#region Entity Instance Helper Methods
		/// <summary>
		/// Check <see cref="FactInstance"/> associated to see if we have a matching
		/// role instance for this role.
		/// </summary>
		/// <param name="identifyingRole">A role in the preferred identifier for the entity type.</param>
		/// <returns>A <see cref="RoleInstance"/> matching the role.</returns>
		/// <remarks>This method assumes that this.EntityType.PreferredIdentifier.RoleCollection.Contains(identifiyingRole)
		/// is true. Since this function is highly likely to be called for all roles in the preferred identifier
		/// we do not reverify this condition in the function., so false positive results may occur if this precondition is not met.</remarks>
		public RoleInstance FindIdentifyingRoleInstance(Role identifyingRole)
		{
			FactType objectifiedFactType = this.EntityType?.NestedFactType;
			if (objectifiedFactType != null && objectifiedFactType == identifyingRole.FactType)
			{
				// We need a role instance for the matching fact instance, if it exists
				return ObjectifiedInstance?.FindRoleInstance(identifyingRole);
			}

			// Otherwise, look at the role instance for the opposite role.
			// We use the first (any duplication will be an error on the fact type population, so we do not track the implied duplication on the entity).
			Role oppositeRole = identifyingRole.OppositeOrUnaryRole as Role; // UNDONE: NOW Edge case, handle objectified role player identified by (alternately id-ed) objectification. The opposite or unary will be a proxy, population inferred from objectified fact.
			if (oppositeRole != null)
			{
				RoleInstance boundInstance = RoleInstance.GetLinks(oppositeRole, this).FirstOrDefault();
				if (boundInstance != null)
				{
					return oppositeRole == identifyingRole ? boundInstance : boundInstance.FactInstance.FindRoleInstance(identifyingRole);
				}
			}
			return null;
		}

		/// <summary>
		/// Does this instance populate a role in any identifying fact type?
		/// </summary>
		public bool HasIdentifyingRoleInstance
		{
			get
			{
				UniquenessConstraint pid = this.EntityType?.PreferredIdentifier;
				if (pid != null)
				{
					LinkedElementCollection<FactType> identifyingFactTypes = pid.FactTypeCollection;
					if (identifyingFactTypes != null)
					{
						FactInstance objectifiedInstance = this.ObjectifiedInstance;
						if (objectifiedInstance != null && identifyingFactTypes.Contains(objectifiedInstance.FactType))
						{
							LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
							foreach (RoleInstance roleInstance in objectifiedInstance.RoleInstanceCollection)
							{
								if (pidRoles.Contains(roleInstance.Role))
								{
									return true;
								}
							}
						}

						foreach (Role role in this.RoleCollection)
						{
							if (identifyingFactTypes.Contains(role.FactType))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Get all identifying role instances in the order of the preferred identifier roles.
		/// </summary>
		/// <param name="callback">Provide the identifying role, matching instance (if set). Returns true to break iteration.</param>
		/// <returns>True if any callback returns true.</returns>
		public bool IterateIdentifyingRoleInstances(Func<Role, RoleInstance, bool> callback)
		{
			UniquenessConstraint pid = this.EntityType?.PreferredIdentifier;
			if (pid != null)
			{
				LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
				int pidRoleCount = pidRoles.Count;
				for (int i = 0; i < pidRoleCount; ++i)
				{
					Role role = pidRoles[i];
					RoleInstance roleInstance = this.FindIdentifyingRoleInstance(role);
					if (callback(role, roleInstance))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Attach the <paramref name="instance"/> to the specified <paramref name="identifyingRole"/>.
		/// This routine safed creates a new <see cref="FactInstance"/> and <see cref="RoleInstance"/>
		/// as needed.
		/// </summary>
		/// <param name="identifyingRole">A role from the entity identifier to attach to</param>
		/// <param name="instance">The instance to attach. Null for a unary identifier role.</param>
		public void EnsureIdentifyingRoleInstance(Role identifyingRole, ObjectInstance instance)
		{
			FactType objectifiedFactType = this.EntityType?.NestedFactType;
			if (objectifiedFactType != null && objectifiedFactType == identifyingRole.FactType)
			{
				// This is a role in the objectified fact type itself.
				FactInstance objectifiedInstance = ObjectifiedInstance;
				RoleInstance roleInstance = null;
				if (objectifiedInstance == null)
				{
					objectifiedInstance = new FactInstance(this.Partition);
					objectifiedInstance.FactType = objectifiedFactType;
					this.ObjectifiedInstance = objectifiedInstance;
				}
				else
				{
					roleInstance = objectifiedInstance.FindRoleInstance(identifyingRole);
				}

				if (roleInstance != null)
				{
					if (objectifiedFactType.UnaryPattern != UnaryValuePattern.NotUnary)
					{
						// There is nothing else to check on an objectified unary. The instance is ignored
						return;
					}

					if (roleInstance.ObjectInstance == instance)
					{
						return;
					}
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
					roleInstance.Delete();
					new RoleInstance(identifyingRole, instance).FactInstance = objectifiedInstance;
#else
					roleInstance.ObjectInstance = instance;
#endif // !ROLEINSTANCE_ROLEPLAYERCHANGE
					return;
				}
			}

			// Otherwise, look at the role instance for the opposite role.
			// We use the first (any duplication will be an error on the fact type population, so we do not track the implied duplication on the entity).
			Role oppositeRole = identifyingRole.OppositeOrUnaryRole as Role; // UNDONE: NOW Edge case, handle objectified role player identified by (alternately id-ed) objectification. The opposite or unary will be a proxy, population inferred from objectified fact.
			if (oppositeRole != null)
			{
				RoleInstance boundInstance = RoleInstance.GetLinks(oppositeRole, this).FirstOrDefault();
				if (boundInstance != null)
				{
					if (oppositeRole == identifyingRole)
					{
						// There is nothing else to check on a unary identifying fact type. The instance is ignored.
						return;
					}

					FactInstance boundFactInstance = boundInstance.FactInstance;
					RoleInstance identifyingRoleInstance = boundFactInstance.FindRoleInstance(identifyingRole);
					if (identifyingRoleInstance != null)
					{
						if (identifyingRoleInstance.ObjectInstance == instance)
						{
							return;
						}

#if !ROLEINSTANCE_ROLEPLAYERCHANGE
						identifyingRoleInstance.Delete();
						new RoleInstance(identifyingRole, instance).FactInstance = boundFactInstance;
#else
						identifyingRoleInstance.ObjectInstance = instance;
#endif // !ROLEINSTANCE_ROLEPLAYERCHANGE
					}
				}
				else
				{
					// We need to create a new fact instance with this instance attached to the opposite role.
					FactInstance factInstance = new FactInstance(this.Partition);
					factInstance.FactType = oppositeRole.FactType;
					new RoleInstance(oppositeRole, this).FactInstance = factInstance;
					if (oppositeRole != identifyingRole)
					{
						new RoleInstance(identifyingRole, instance).FactInstance = factInstance;
					}
				}
			}
		}
		#endregion // Entity Instance Helper Methods
		#region Entity Subtype Instance Helper Methods
		/// <summary>
		/// Retrieve an <see cref="ObjectInstance"/> representing a subutype instance for the <paramref name="entityInstance"/>
		/// that matches the specified <paramref name="subtype"/>.
		/// </summary>
		/// <param name="entityInstance">The <see cref="ObjectInstance"/> to use as the <see cref="P:SuperTypeInstance"/></param>
		/// <param name="subtype">The <see cref="ObjectType"/> that is a subtype identified by the <see cref="P:EntityInstance.EntityType"/>
		/// of the <paramref name="entityInstance"/></param>
		/// <param name="checkExisting">True to check existing instances for an existing subtype instance.</param>
		/// <param name="forceCreate">If <paramref name="checkExisting"/> is set and no match is found, then create a new instance</param>
		/// <returns>An <see cref="ObjectInstance"/> or <see langword="null"/>, depending on the specified options.</returns>
		public static ObjectInstance GetSubtypeInstance(ObjectInstance entityInstance, ObjectType subtype, bool checkExisting, bool forceCreate)
		{
			if (checkExisting)
			{
				// Note that the total set of subtype instances for any given entity is unbound as the population grows.
				// However, the total number of subtype instances is bounded by the number of identified subtypes in the
				// model, not the number of instances.
				foreach (ObjectInstance testSubtypeInstance in entityInstance.EntitySubtypeInstanceCollection)
				{
					if (testSubtypeInstance.ObjectType == subtype)
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
				ObjectInstance newSubtypeInstance = new ObjectInstance(entityInstance.Partition);
				newSubtypeInstance.SupertypeInstance = entityInstance;
				newSubtypeInstance.ObjectType = subtype;
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
		public static ObjectInstance GetTypedSubtypeInstance(ObjectInstance instance, ObjectType relatedType)
		{
			if (relatedType != null &&
				instance.ObjectType != relatedType)
			{
				if (instance.EntityType != null)
				{
					ObjectInstance entityInstance = instance;
					ObjectInstance supertypeInstance = instance.SupertypeInstance;
					if (supertypeInstance != null)
					{
						if (supertypeInstance.ObjectType == relatedType)
						{
							return supertypeInstance;
						}
						entityInstance = supertypeInstance;
					}
					if (null != entityInstance)
					{
						return GetSubtypeInstance(entityInstance, relatedType, true, true);
					}
				}
			}
			return instance;
		}
		#endregion // Entity Subtype Instance Helper Methods
		#region ObjectificationInstance Validation
		/// <summary>
		/// Validate objectification instances for a new <see cref="Objectification"/>
		/// </summary>
		[DelayValidatePriority(5)] // Run after subtype instances are validated
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
			ORMModel model = null;
			if (!deletedLink &&
				null != (pid = entityType.ResolvedPreferredIdentifier) &&
				pid.PreferredIdentifierFor == entityType &&
				pid.IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				(identifierFactType = pidFactTypes[0]) == factType)
			{
				// Preferred uniqueness constraint is internal to the objectified FactType,
				// so the EntityType instances are implied
				LinkedElementCollection<FactInstance> factInstances = factType.FactInstanceCollection;
				int factInstanceCount = factInstances.Count;
				for (int i = 0; i < factInstanceCount; ++i)
				{
					FactInstance factInstance = factInstances[i];
					if (factInstance.ObjectifyingInstance == null)
					{
						factInstance.ObjectifyingInstanceRequiredError = null;
						ObjectInstance entityInstance = new ObjectInstance(partition);
						entityInstance.ObjectType = entityType;
						entityInstance.ObjectifiedInstance = factInstance;
					}
				}

				LinkedElementCollection<ObjectInstance> objectInstances = entityType.ObjectInstanceCollection;
				int objectInstanceCount = objectInstances.Count;
				int unattachedCount = objectInstanceCount - factInstanceCount;
				if (unattachedCount > 0)
				{
					// Unlikely, but cleans up any non-implicit entity instances
					for (int i = objectInstanceCount - 1; i >= 0; --i)
					{
						ObjectInstance objectInstance = objectInstances[i];
						if (objectInstance.ObjectifiedInstance == null)
						{
							ObjectifiedInstanceRequiredError error = objectInstance.ObjectifiedInstanceRequiredError;
							if (objectInstance.ObjectifiedInstance == null)
							{
								if (error == null)
								{
									error = new ObjectifiedInstanceRequiredError(partition);
									error.ObjectInstance = objectInstance;
									error.Model = model ?? (model = entityType.ResolvedModel);
									error.GenerateErrorText();
								}
							}
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
				// the single instance validation routines, but it is not worth delay validating
				// each instance to avoid this.
				foreach (FactInstance factInstance in factType.FactInstanceCollection)
				{
					ObjectInstance objectifyingInstance = factInstance.ObjectifyingInstance;
					bool hasError = objectifyingInstance == null;
					ObjectifyingInstanceRequiredError error = factInstance.ObjectifyingInstanceRequiredError;
					if (hasError)
					{
						if (error == null)
						{
							error = new ObjectifyingInstanceRequiredError(partition);
							error.FactInstance = factInstance;
							error.Model = model ?? (model = factType.ResolvedModel);
							error.GenerateErrorText();
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
				foreach (ObjectInstance objectInstance in entityType.ObjectInstanceCollection)
				{
					ObjectifiedInstanceRequiredError error = objectInstance.ObjectifiedInstanceRequiredError;
					if (objectInstance.ObjectifiedInstance == null)
					{
						if (error == null)
						{
							error = new ObjectifiedInstanceRequiredError(partition);
							error.ObjectInstance = objectInstance;
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
				foreach (FactInstance factInstance in factType.FactInstanceCollection)
				{
					factInstance.ObjectifyingInstance = null;
					factInstance.ObjectifyingInstanceRequiredError = null;
				}
				foreach (ObjectInstance objectInstance in entityType.ObjectInstanceCollection)
				{
					objectInstance.ObjectifiedInstance = null;
					objectInstance.ObjectifiedInstanceRequiredError = null;
				}
			}
		}
		#endregion // ObjectificationInstance Validation
		#region PopulationMandatoryError Validation
		/// <summary>
		/// Validator callback for PopulationMandatoryError
		/// <remarks>DelayValidatePriority is set to run after implied mandatory constraint creation</remarks>
		/// </summary>
		[DelayValidatePriority(7)]
		protected static void DelayValidateInstancePopulationMandatoryError(ModelElement element)
		{
			(element as ObjectInstance).ValidateInstancePopulationMandatoryError(null);
		}
		#region InstanceTyper struct
		/// <summary>
		/// Helper struct to get a typed version of a given instance.
		/// </summary>
		private struct InstanceTyper
		{
			private readonly ObjectInstance myInstance;
			private readonly ObjectType myType;
			private bool myInitialized;
			private ObjectInstance mySupertypeInstance;
			private ObjectType mySupertypeType;
			private LinkedElementCollection<ObjectInstance> mySubtypeInstances;
			/// <summary>
			/// Create a new instance typer for the given instance.
			/// </summary>
			/// <param name="instance">The instance to base other types on.</param>
			public InstanceTyper(ObjectInstance instance)
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
			public ObjectInstance TypedInstance(ObjectType type)
			{
				ObjectInstance instance = myInstance;
				if (myType == type)
				{
					return instance;
				}
				if (!myInitialized)
				{
					myInitialized = true;
					ObjectInstance entityInstance = instance.SupertypeInstance ?? instance;
					mySupertypeInstance = entityInstance;
					mySupertypeType = entityInstance.ObjectType;
					mySubtypeInstances = entityInstance.EntitySubtypeInstanceCollection;

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
					foreach (ObjectInstance subtypeInstance in mySubtypeInstances)
					{
						if (subtypeInstance.ObjectType == type)
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
			public static bool RepresentsSameInstance(ObjectInstance instance1, ObjectInstance instance2)
			{
				return NormalizeInstance(instance1) == NormalizeInstance(instance2);
			}
			private static ObjectInstance NormalizeInstance(ObjectInstance instance)
			{
				return instance.SupertypeInstance ?? instance;
			}
		}
		#endregion // InstanceTyper struct
		/// <summary>
		/// Rule helper for validating the current <see cref="ObjectInstance"/>
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
						ObjectInstance identifyingInstance = null;
						bool retrievedIdentifyingInstance = false;
						for (int i = 0; i < playedRoleCount; ++i)
						{
							Role currentRole = playedRoles[i];
							ReadOnlyLinkedElementCollection<ObjectInstance> currentRoleInstances = null;
							ObjectType identifiedEntityType;
							bool impliedObjectificationRole = false;
							switch (currentRole.GetReferenceSchemePattern(out identifiedEntityType))
							{
								//case ReferenceSchemeRolePattern.None:
								//    break;
								case ReferenceSchemeRolePattern.OptionalSimpleIdentifiedRole:
									// This one is tricky. The implied RoleInstance population
									// for the opposite (identifier) role is based on the set of EntityInstances
									// on this role. Because the opposite role is optional, the population
									// for this role is not automatically filled in, so it is possible to
									// have PopulationMandatoryErrors on any disjunctive mandatory constraint
									// intersecting the opposite role.
									// Note that we always have an opposite role. Otherwise, the reference scheme
									// pattern would not hold.
									if (notifyAdded == null) // The opposite instance will be validated during load, no need to repeat
									{
										Role identifyingRole = currentRole.OppositeOrUnaryRole.Role; ;
										foreach (ConstraintRoleSequence sequence in identifyingRole.ConstraintRoleSequenceCollection)
										{
											MandatoryConstraint constraint = sequence as MandatoryConstraint;
											if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
											{
												if (!retrievedIdentifyingInstance)
												{
													retrievedIdentifyingInstance = true;
													identifyingInstance = this.FindIdentifyingRoleInstance(identifyingRole)?.ObjectInstance;
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
													if (InstanceTyper.RepresentsSameInstance(error.ObjectInstance, identifyingInstance))
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
								//    // entity instances of the opposite role, not the implied FactInstances.
								//    // However, because these instances are part of the ObjectInstanceCollection
								//    // on the role, there is no special handling needed.
								//    continue;
								case ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole:
									// Synchronize the EntityInstances on the opposite role with the identifying object
									// Note that this will trigger the MandatoryIdentifiedRole case on another call.
									EnsureImpliedEntityInstance(this, identifiedEntityType, currentRole);
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
								//    // Evaluate these. Although the population of the FactInstance
								//    // is implied, the role should set get a population mandatory error
								//    // if an instance is not used by another entity instance.
								//    break;
								case ReferenceSchemeRolePattern.ImpliedObjectificationRole:
									impliedObjectificationRole = true;
									break;
							}
							SubtypeMetaRole subtypeRole;
							bool impliedRolePopulation = impliedObjectificationRole || ((null != (subtypeRole = currentRole as SubtypeMetaRole)) && this.SupertypeInstance != null && ((SubtypeFact)subtypeRole.FactType).ProvidesPreferredIdentifier);
							foreach (ConstraintRoleSequence sequence in currentRole.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
								{
									bool hasError = false;
									ObjectInstance typedInstance = null;
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
												ReadOnlyLinkedElementCollection<ObjectInstance> roleInstances;
												ObjectInstance findInstance;
												ObjectType roleType;
												if (currentRole == constraintRole)
												{
													if (currentRoleInstances == null)
													{
														currentRoleInstances = currentRole.ObjectInstanceCollection;
													}
													roleInstances = currentRoleInstances;
													roleType = objectType;
												}
												else
												{
													roleInstances = constraintRole.ObjectInstanceCollection;
													roleType = constraintRole.RolePlayer;
												}
												if (null != (findInstance = ((trackedInstanceType == roleType) ? typedInstance : instanceTyper.TypedInstance(roleType))) &&
													roleInstances.Contains(findInstance))
												{
													break;
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
											error.ObjectInstance = typedInstance;
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
		/// Make sure there is an <see cref="ObjectInstance"/> associated with the
		/// <paramref name="identifiedEntityType"/> for each instance associated with the role player
		/// of <paramref name="identifierRole"/>.
		/// </summary>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="identifiedEntityType"/></param>
		protected static void EnsureImpliedEntityInstances(ObjectType identifiedEntityType, Role identifierRole)
		{
			// UNDONE: NOW Kill EntityTypeRoleInstance The place that called this has been removed. However, the remaining EnsureImpliedEntityInstance code looks legit. Either reevaluate/restore the deleted call point in DelayValidatePreferredIdentifier or remove this code.
			ObjectType identifierObjectType = identifierRole.RolePlayer;
			if (identifierObjectType != null)
			{
				// Synchronize the opposite EntityInstance collection
				foreach (ObjectInstance identifierInstance in identifierObjectType.ObjectInstanceCollection)
				{
					EnsureImpliedEntityInstance(identifierInstance, identifiedEntityType, identifierRole);
				}
			}
		}
		/// <summary>
		/// Verify that there is an <see cref="ObjectInstance"/> representing an entity associated with the
		/// <paramref name="identifiedEntityType"/> that references the specified <paramref name="instance"/>
		/// through the provided <paramref name="identifierRole"/>. Used to create consistent
		/// implicit populations for any <see cref="FactType"/> matching the reference scheme
		/// pattern.
		/// </summary>
		/// <param name="instance">The <see cref="ObjectInstance"/> from the identifier role</param>
		/// <param name="identifiedEntityType">The <see cref="ObjectType">entity type</see> being identified</param>
		/// <param name="identifierRole">The role from the preferred identifier constraint associated with the <paramref name="identifiedEntityType"/></param>
		private static void EnsureImpliedEntityInstance(ObjectInstance instance, ObjectType identifiedEntityType, Role identifierRole)
		{
			foreach (RoleInstance roleInstance in RoleInstance.GetLinks(identifierRole, instance))
			{
				ObjectInstance identifiedInstance = roleInstance.IdentifiedEntityInstance;
				if (identifiedInstance == null)
				{
					Role oppositeRole = identifierRole.OppositeOrUnaryRole as Role;
					if (oppositeRole != null && oppositeRole != identifierRole)
					{
						ObjectInstance newInstance = new ObjectInstance(identifiedEntityType.Partition);
						newInstance.ObjectType = identifiedEntityType;
						new RoleInstance(oppositeRole, newInstance).FactInstance = roleInstance.FactInstance;
					}
				}
			}
		}
		/// <summary>
		/// See if a role instance addition change forces remove of a mandatory error.
		/// </summary>
		/// <param name="element">A <see cref="RoleInstance"/></param>
		[DelayValidatePriority(3)]
		protected static void DelayValidateRemovePopulationMandatoryError(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				RoleInstance roleInstance = (RoleInstance)element;
				ObjectInstance objectInstance = roleInstance.ObjectInstance;
				Role role = roleInstance.Role;

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
							if (InstanceTyper.RepresentsSameInstance(error.ObjectInstance, objectInstance))
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
		[DelayValidatePriority(7)]
		protected static void DelayValidateRolePopulationMandatoryError(ModelElement element)
		{
			// UNDONE: NOW Kill EntityTypeRoleInstance. This whole validation needs review. I'm not sure the reference scheme pattern is even needed here. It might be a nice shortcut, but we're also looking at data that may no longer be needed here.
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
								ObjectInstance[] instances = null;
								ObjectType instancesOfType = null;
								BitTracker seenInstances = default(BitTracker);
								MandatoryConstraint constraint;
								IComparer<ObjectInstance> comparer = ModelElementIdComparer<ObjectInstance>.Instance;
								SubtypeMetaRole subtypeRole;
								bool impliedRolePopulation = (null != (subtypeRole = role as SubtypeMetaRole)) && ((SubtypeFact)subtypeRole.FactType).ProvidesPreferredIdentifier;
								foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
								{
									constraint = sequence as MandatoryConstraint;
									if (constraint != null)
									{
										if (impliedRolePopulation || constraint.Modality != ConstraintModality.Alethic)
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

										// Get repeated stuff once
										if (instancesOfType != trackedInstanceType)
										{
											instancesOfType = trackedInstanceType;
											LinkedElementCollection<ObjectInstance> instancesCollection = trackedInstanceType.ObjectInstanceCollection;
											if (0 == (instanceCount = instancesCollection.Count))
											{
												instances = null;
												continue;
											}
											instances = instancesCollection.ToArray();
											Array.Sort<ObjectInstance>(instances, comparer);
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
												foreach (ObjectInstance subtypeInstance in supertypeRole.OppositeRole.Role.RolePlayer.ObjectInstanceCollection)
												{
													InstanceTyper instanceTyper = new InstanceTyper(subtypeInstance);
													ObjectInstance findInstance;
													int index;
													if (null != (findInstance = instanceTyper.TypedInstance(trackedInstanceType)) &&
														0 <= (index = Array.BinarySearch<ObjectInstance>(instances, findInstance, comparer)) &&
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
												ReadOnlyLinkedElementCollection<ObjectInstance> roleInstances;
												ObjectType differentRolePlayer;
												roleInstances = currentRole.ObjectInstanceCollection;
												if (trackedInstanceType == (differentRolePlayer = currentRole.RolePlayer))
												{
													differentRolePlayer = null;
												}
												int roleInstanceCount = roleInstances.Count;
												for (int j = 0; j < roleInstanceCount; ++j)
												{
													ObjectInstance findInstance = roleInstances[j];
													if (differentRolePlayer != null)
													{
														findInstance = new InstanceTyper(findInstance).TypedInstance(trackedInstanceType);
													}
													int index;
													if (findInstance != null &&
														0 <= (index = Array.BinarySearch<ObjectInstance>(instances, findInstance, comparer)) &&
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
												ObjectInstance previousErrorInstance = error.ObjectInstance;
												int index;
												if (previousErrorInstance.ObjectType != trackedInstanceType)
												{
													// We only report errors on the nearest shared compatible type.
													// Clear any errors that are not involved with this type.
													error.Delete();
												}
												else if (0 <= (index = Array.BinarySearch<ObjectInstance>(instances, previousErrorInstance, comparer)))
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
													error.ObjectInstance = instances[i];
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
							// This one is tricky. The implied RoleInstance population
							// for the opposite (identifier) role is based on the set of EntityInstances
							// on this role. Because the opposite role is optional, the population
							// for this role is not automatically filled in, so it is possible to
							// have PopulationMandatoryErrors on any disjunctive mandatory constraint
							// intersecting the opposite role.
							// Note that we always have an opposite role. Otherwise, the reference scheme
							// pattern would not hold.
							LinkedElementCollection<ObjectInstance> rolePlayerInstances = null;
							foreach (ConstraintRoleSequence sequence in role.OppositeOrUnaryRole.Role.ConstraintRoleSequenceCollection)
							{
								MandatoryConstraint constraint = sequence as MandatoryConstraint;
								if (constraint != null)
								{
									if (rolePlayerInstances == null)
									{
										rolePlayerInstances = rolePlayer.ObjectInstanceCollection;
									}
									LinkedElementCollection<PopulationMandatoryError> oppositeErrors = constraint.PopulationMandatoryErrorCollection;
									int errorCount = oppositeErrors.Count;
									for (int j = errorCount - 1; j >= 0; --j)
									{
										// This is a single role uniqueness. There is no need to resolve the supertype
										// for the constraint or check subtypes.
										PopulationMandatoryError error = oppositeErrors[j];
										if (rolePlayerInstances.Contains(error.ObjectInstance))
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
							// UNDONE: NOW Kill EntityTypeInstance. This filled an implied populated
						case ReferenceSchemeRolePattern.ImpliedObjectificationRole:
							// Nothing to do here, the implied population corresponds to the
							// FactInstance and EntityInstance population, which are correlated
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
			// Internal justification: See comments in FactInstance.DelayValidateObjectifyingInstanceRequiredError
			((ObjectInstance)element).ValidateObjectifiedInstanceRequiredError(null);
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
						error.ObjectInstance = this;
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
		#region CompatibleValueInstanceError Validation
		/// <summary>
		/// Validator callback for CompatibleValueInstanceError
		/// </summary>
		[DelayValidatePriority(-1)] // Run this early because it also sets or clears invariant values, which are used in other validators
		private static void DelayValidateCompatibleValueInstanceError(ModelElement element)
		{
			(element as ObjectInstance).ValidateCompatibleValueInstanceError(null);
		}

		/// <summary>
		/// Rule helper to determine whether or not the given value
		/// matches the data type of the parent Value Type
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateCompatibleValueInstanceError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				ObjectType parent;
				DataType dataType;
				bool hasError = false;
				if (null != (parent = ValueType) &&
					null != (dataType = parent.DataType))
				{
					if (!dataType.CanParseAnyValue)
					{
						string value = Value;
						if (dataType.IsCultureSensitive)
						{
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
							hasError = !dataType.CanParse(value);
							InvariantValue = "";
						}
					}
				}
				CompatibleValueInstanceError badValue = this.CompatibleValueInstanceError;
				if (hasError)
				{
					if (badValue == null)
					{
						badValue = new CompatibleValueInstanceError(Partition);
						badValue.ValueInstance = this;
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
		#region TooFewEntityTypeRoleInstancesError Validation
		/// <summary>
		/// Validator callback for TooFewEntityRoleInstancesError
		/// </summary>
		[DelayValidatePriority(2)]
		private static void DelayValidateTooFewEntityRoleInstancesError(ModelElement element)
		{
			(element as ObjectInstance).ValidateTooFewEntityTypeRoleInstancesError(null);
		}

		/// <summary>
		/// Rule helper to determine whether or not TooFewEntityTypeRoleInstancesError
		/// should be attached to the EntityInstance.
		/// </summary>
		/// <param name="notifyAdded">Element notification, set during deserialization</param>
		private void ValidateTooFewEntityTypeRoleInstancesError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasError = false;
				ObjectType parent = EntityType;
				UniquenessConstraint pid;
				if (parent != null && (pid = parent.PreferredIdentifier) != null)
				{
					int roleInstanceCount = 0;
					this.IterateIdentifyingRoleInstances((pidRole, identityInstance) =>
					{
						if (identityInstance != null)
						{
							++roleInstanceCount;
						}
						return false; // continue
					});

					if (roleInstanceCount == 0)
					{
						// The instance is alive so that a FactInstance can be referenced
						// via an ObjectInstance and the associated ObjectificationInstance.
						// We report rules as if these empty instances do not exist.
						// If we are currently deserializing (notifyAdded is not null), then
						// the error verification will happen with the FactInstance. This
						// routine just needs to verify that the instance is still needed.
						hasError = false;
						bool referencedForFactInstance = false;
						FactInstance factInstance;
						if (null != (factInstance = ObjectifiedInstance))
						{
							referencedForFactInstance = RoleCollection.Count != 0;
							if (notifyAdded == null)
							{
								FactInstance.DelayValidateObjectifyingInstanceRequiredError(factInstance);
							}
						}
						LinkedElementCollection<ObjectInstance> subtypeInstances = EntitySubtypeInstanceCollection;
						int subtypeInstanceCount = subtypeInstances.Count;
						for (int i = subtypeInstanceCount - 1; i >= 0; --i)
						{
							ObjectInstance subtypeInstance = subtypeInstances[i];
							bool subtypeInstancedReferencedForFactInstance = false;
							if (null != (factInstance = subtypeInstance.ObjectifiedInstance))
							{
								subtypeInstancedReferencedForFactInstance = subtypeInstance.RoleCollection.Count != 0;
								if (notifyAdded == null)
								{
									FactInstance.DelayValidateObjectifyingInstanceRequiredError(factInstance);
								}
							}
							if (subtypeInstancedReferencedForFactInstance)
							{
								// UNDONE: NOW Kill EntityTypeRoleInstance This looks very suspicious (including original). The existence of a subtype instance should be treated as a use whether or not the subtype instance itself is used.
								referencedForFactInstance = true;
							}
							else
							{
								subtypeInstance.Delete();
							}
						}

						if (!referencedForFactInstance)
						{
							if (string.IsNullOrEmpty(this.IdentityTag) || this.AutoIdentityTag)
							{
								this.Delete();
								return;
							}
							hasError = true;
						}
						else
						{
							FrameworkDomainModel.DelayValidateElement(parent, DelayAutoCreateIdentityTags);
						}
					}
					else
					{
						LinkedElementCollection<Role> pidRoles = pid.RoleCollection;
						int pidRoleCount = pidRoles.Count;
						if (pid.IsObjectifiedPreferredIdentifier) // This indicates all roles are required, do not look at mandatory role states
						{
							hasError = pidRoleCount != roleInstanceCount;
						}
						else
						{
							FactType objectifiedFactType = parent.Objectification?.NestedFactType;
							FactInstance objectifiedFact = objectifiedFactType != null ? this.ObjectifiedInstance : null;
							Predicate<Role> nonObjectifiedRoleIsPopulated = testRole =>
							{
								// Objectification check is a prerequisite.
								RoleBase oppositeRoleBase = testRole.OppositeOrUnaryRole;
								if (oppositeRoleBase != null)
								{
									RoleProxy proxy = oppositeRoleBase as RoleProxy;
									// For a proxy, an objectified role player is identifier by the objectification. This requires a functional
									// objectified role. If this entity instance is in an instance of the objectified fact type that
									// has an objectifying instance then we have enough to identify us. However, if the objectified
									// fact type does not have an objectified instance then we already have an error on that instance,
									// so this is actually a chained error state. Similarly, if there is more than one fact instance
									// populating this pidRole/instance then we will have a population uniqueness error on the fact
									// instance. We do not report these chained errors here, so we consider ourself identified if this
									// instance participates in any fact instance for the proxy target role.
									// Similarly for the normal (non-proxy) case, if there is more than one possible identifier we will
									// have an error on the fact instance population and do not need to repeat it here.
									return (proxy != null ? proxy.TargetRole : (Role)oppositeRoleBase).ObjectInstanceCollection.Contains(this);
								}
								return false;
							};

							Func<Role, Role> matchIdentifierRole = mandatoryRole =>
							{
								Role opposite = mandatoryRole.OppositeOrUnaryRole as Role;
								return opposite != null && pidRoles.IndexOf(opposite) != -1 ? opposite : null;
							};

							for (int i = 0; i < pidRoleCount; ++i)
							{
								Role pidRole = pidRoles[i];
								if (objectifiedFactType != null && objectifiedFactType == pidRole.FactType)
								{
									// Objectified roles are always mandatory.
									if (objectifiedFact == null || objectifiedFact.FindRoleInstance(pidRole) == null)
									{
										hasError = true;
										break;
									}
								}
								else if (!nonObjectifiedRoleIsPopulated(pidRole))
								{
									Role checkMandatoryRole = pidRole.OppositeOrUnaryRole as Role;
									if (checkMandatoryRole != null)
									{
										foreach (MandatoryConstraint mandatory in checkMandatoryRole.ConstraintRoleSequenceCollection.OfType<MandatoryConstraint>().Where(mc => mc.Modality == ConstraintModality.Alethic))
										{
											LinkedElementCollection<Role> mandatoryRoles;
											if (mandatory.IsSimple || (mandatoryRoles = mandatory.RoleCollection).Count == 1)
											{
												hasError = true;
												break;
											}
											else
											{
												if (
													// The mandatory constraint does not have roles outside the identifier
													!mandatoryRoles.Any(mandatoryRole => mandatoryRole != checkMandatoryRole && null == matchIdentifierRole(mandatoryRole)) &&
													// And none of the roles in the mandatory constraint are populated
													!mandatoryRoles.Any(mandatoryRole => mandatoryRole != checkMandatoryRole && nonObjectifiedRoleIsPopulated(matchIdentifierRole(mandatoryRole)))
												)
												{
													hasError = true;
													break;
												}
											}
										}
									}
									else
									{
										hasError = true;
										break;
									}
								}
							}
						}

						// We have at least one role, so make sure related FactInstances
						// do not have an ObjectifyingInstanceRequiredError
						if (null == notifyAdded)
						{
							FactInstance factInstance = ObjectifiedInstance;
							if (factInstance != null)
							{
								factInstance.ObjectifyingInstanceRequiredError = null;
							}
							foreach (ObjectInstance subtypeInstance in EntitySubtypeInstanceCollection)
							{
								if (null != (factInstance = subtypeInstance.ObjectifiedInstance))
								{
									factInstance.ObjectifyingInstanceRequiredError = null;
								}
							}
						}
					}
				}
				else if (string.IsNullOrEmpty(this.IdentityTag))
				{
					if (parent != null)
					{
						FrameworkDomainModel.DelayValidateElement(parent, DelayAutoCreateIdentityTags);
					}
				}

				TooFewEntityRoleInstancesError tooFew = this.TooFewEntityRoleInstancesError;
				if (hasError)
				{
					if (tooFew == null)
					{
						tooFew = new TooFewEntityRoleInstancesError(Partition);
						tooFew.EntityInstance = this;
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
		#region General ObjectInstance Rules
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
					if (null != (oppositeRoleBase = link.Role.OppositeOrUnaryRole) &&
						null != (oppositeRole = oppositeRoleBase.Role) &&
						null != (rolePlayer = oppositeRole.RolePlayer))
					{
						foreach (ObjectInstance instance in rolePlayer.ObjectInstanceCollection)
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


		private static Dictionary<string, object> myAutoIdentitRenderingOptions;
		private static IDictionary<string, object> AutoIdentityRenderingOptions
		{
			get
			{
				Dictionary<string, object> options;
				if (null == (options = myAutoIdentitRenderingOptions))
				{
					options = new Dictionary<string, object>();
					options[CoreVerbalizationOption.ObjectTypeNameDisplay] = ObjectTypeNameVerbalizationStyle.CombineNamesLeadWithLower;
					options[CoreVerbalizationOption.RemoveObjectTypeNameCharactersOnSeparate] = ".:_";
					options[CoreVerbalizationOption.FixedCaseWordPrefix] = "\x1";
					System.Threading.Interlocked.CompareExchange<Dictionary<string, object>>(ref myAutoIdentitRenderingOptions, options, null);
					options = myAutoIdentitRenderingOptions;
				}
				return options;
			}
		}
		/// <summary>
		/// Create automatic identity tags for empty identity tags that are still in use.
		/// </summary>
		[DelayValidatePriority(6)]
		private static void DelayAutoCreateIdentityTags(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}

			ObjectType objectType = (ObjectType)element;
			if (objectType.DataType == null)
			{
				// ValueType transitions move data between Value and IdentitTag in earlier rules. There is nothing left to check here.
				UniquenessConstraint pid = objectType.ResolvedPreferredIdentifier;
				bool isSupertype = objectType.IsSupertype;
				if (pid != null ? pid.PreferredIdentifierFor != objectType : null != objectType.GetPreferredTerminalSupertype()) // TerminalSupertype does not return itself
				{
					// This clears the identity tags on the current item since we know where the supertype instance points
					// The supertype instance itself is sufficient to keep the element alive.
					foreach (ObjectInstance instance in objectType.ObjectInstanceCollection)
					{
						if (instance.ObjectifiedInstance == null &&
							instance.SupertypeInstance == null &&
							RoleInstance.GetLinksToRoleCollection(instance).Count == 0 &&
							(!isSupertype || EntitySubtypeInstanceHasSupertypeInstance.GetLinksToEntitySubtypeInstanceCollection(instance).Count == 0))
						{
							instance.Delete();
							continue;
						}
						instance.IdentityTag = string.Empty;
						instance.AutoIdentityTag = false;
					}
				}
				else
				{
					// UNDONE: NOW Kill EntityTypeRole Instance. We don't have code yet to enable the transition from subtype
					// where supertype has a pid and a subtype where the ultimate supertype does not. In this case, the supertype
					// instances resolve to the resolved unidentified supertype. However, if a subtype relationship was already
					// set and we cannot result the ultimate supertype then we leave the previous state in place. In other words,
					// when we aren't sure due to lack of evidence on the types, the existing supertype instance relationship
					// has the final say on what is current attached as the supertype.

					// Tags are owned by elements on this instance. Fill in automatic names for surviving instances.
					LinkedElementCollection<ObjectInstance> instances = objectType.ObjectInstanceCollection;
					if (instances.Count != 0)
					{
						Dictionary<string, object> usedNames = new Dictionary<string, object>();

						// Walk this twice so we know what all of the previously set names are
						bool retag = false;
						foreach (ObjectInstance instance in instances)
						{
							string identityTag = instance.IdentityTag;
							if (string.IsNullOrEmpty(identityTag))
							{
								retag = true;
							}
							else
							{
								usedNames[identityTag] = null;
							}
						}

						if (retag)
						{
							string baseName = null;
							string format = null;
							int nextCounter = 0;
							foreach (ObjectInstance instance in instances)
							{
								string identityTag = instance.IdentityTag;
								bool untagged = string.IsNullOrEmpty(identityTag);
								bool autoTag = instance.AutoIdentityTag;
								if (autoTag || untagged)
								{
									if (instance.ObjectifiedInstance == null &&
										instance.SupertypeInstance == null &&
										RoleInstance.GetLinksToRoleCollection(instance).Count == 0 &&
										(!isSupertype || EntitySubtypeInstanceHasSupertypeInstance.GetLinksToEntitySubtypeInstanceCollection(instance).Count == 0))
									{
										instance.Delete();
										continue;
									}

									if (untagged)
									{
										if (baseName == null)
										{
											// Initialize generation info
											baseName = VerbalizationHelper.NormalizeObjectTypeName(objectType, AutoIdentityRenderingOptions);
											format = Char.IsDigit(baseName[baseName.Length - 1]) ? "{0}_{1}" : "{0}{1}";
										}

										string candidateName;
										do
										{
											candidateName = string.Format(format, baseName, ++nextCounter);
										} while (usedNames.ContainsKey(candidateName));

										usedNames[candidateName] = null;
										instance.AutoIdentityTag = true;
										instance.IdentityTag = candidateName;
									}
								}
							}
						}
					}
				}
			}
		}

		[DelayValidatePriority(5)] // Do this before DelayAutoCreateEntityTags so that we cache accurate values.
		private static void DelayUpdateAutoIdentityTags(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ObjectType objectType = (ObjectType)element;
				LinkedElementCollection<ObjectInstance> instances = objectType.ObjectInstanceCollection;
				if (instances.Count != 0)
				{
					if (objectType.DataType == null && objectType.ResolvedPreferredIdentifier == null && objectType.GetPreferredTerminalSupertype() == null)
					{
						// Reset auto-generated names. Note that this some existing case may be deleted by DelayAutoCreateEntityTags,
						// but the most common case is to simply rename, and holes are harmless.
						string baseName = null;
						string format = null;
						int nextCounter = 0;
						foreach (ObjectInstance instance in instances)
						{
							if (instance.AutoIdentityTag)
							{
								if (baseName == null)
								{
									baseName = VerbalizationHelper.NormalizeObjectTypeName(objectType, AutoIdentityRenderingOptions);
									format = Char.IsDigit(baseName[baseName.Length - 1]) ? "{0}_{1}" : "{0}{1}";
								}
								instance.IdentityTag = string.Format(format, baseName, ++nextCounter);
							}
						}
					}
				}
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
						null != (oppositeRoleBase = role.OppositeOrUnaryRole) &&
						null != (oppositeRole = oppositeRoleBase.Role) &&
						null != (rolePlayer = oppositeRole.RolePlayer))
					{
						foreach (ObjectInstance instance in rolePlayer.ObjectInstanceCollection)
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
		/// AddRule: typeof(EntitySubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntitySupertypeInstanceAddedRule(ElementAddedEventArgs e)
		{
			EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ModelElement;
			ObjectInstance subtypeInstance = link.EntitySubtypeInstance;
			ObjectType subtype;
			if (null != (subtype = subtypeInstance.ObjectType))
			{
				ValidateAttachedSubtypeInstance(subtype, subtypeInstance, link.SupertypeInstance);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(EntitySubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntitySupertypeInstanceDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectInstance supertypeInstance = ((EntitySubtypeInstanceHasSupertypeInstance)e.ModelElement).SupertypeInstance;
			if (!supertypeInstance.IsDeleted)
			{
				supertypeInstance.DelayValidateIfEmpty();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntitySubtypeInstanceHasSupertypeInstance)
		/// </summary>
		private static void EntitySupertypeInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == EntitySubtypeInstanceHasSupertypeInstance.EntitySubtypeInstanceDomainRoleId)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEntitySubtypeInstanceEnforceInitialSubtypeInstance);
			}
			EntitySubtypeInstanceHasSupertypeInstance link = (EntitySubtypeInstanceHasSupertypeInstance)e.ElementLink;
			ObjectInstance subtypeInstance = link.EntitySubtypeInstance;
			ObjectType subtype;
			if (null != (subtype = subtypeInstance.ObjectType))
			{
				ValidateAttachedSubtypeInstance(subtype, subtypeInstance, link.SupertypeInstance);

				ObjectInstance detachedSupertypeInstance = (ObjectInstance)e.OldRolePlayer;
				detachedSupertypeInstance.DelayValidateIfEmpty();
				ObjectType resolvedSupertype;
				if (null != (resolvedSupertype = detachedSupertypeInstance.ObjectType))
				{
					// See if the supertype is a direct supertype or if there are indirect
					// stages, in which case we treat this as a use of the indirect supertypes,
					// not the identifying supertype.
					LinkedElementCollection<ObjectInstance> relatedDetachedSubtypeInstances = null;
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
										relatedDetachedSubtypeInstances = detachedSupertypeInstance.EntitySubtypeInstanceCollection;
										relatedDetachedSubtypeInstancesCount = relatedDetachedSubtypeInstances.Count;
									}
									for (int i = 0; i < relatedDetachedSubtypeInstancesCount; ++i)
									{
										ObjectInstance instance = relatedDetachedSubtypeInstances[i];
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
										relatedDetachedSubtypeInstances = detachedSupertypeInstance.EntitySubtypeInstanceCollection;
										relatedDetachedSubtypeInstancesCount = relatedDetachedSubtypeInstances.Count;
										if (relatedDetachedSubtypeInstancesCount == 0)
										{
											// Nothing left to do
											return ObjectTypeVisitorResult.Stop;
										}
									}
									for (int i = 0; i < relatedDetachedSubtypeInstancesCount; ++i)
									{
										ObjectInstance otherInstance = relatedDetachedSubtypeInstances[i];
										if (otherInstance.ObjectType == subsubtype)
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
		/// An <see cref="ObjectInstance"/> is not valid until it is attached to
		/// both its supertype instance and the subtype. This validation helper runs when
		/// both relationships have been established, which is assumed to happen before the
		/// end of a transaction.
		/// Verify that the specified a <paramref name="subtypeInstance"/> is the only instance of its <paramref name="subtype"/>
		/// with the given <paramref name="supertypeInstance"/>
		/// </summary>
		/// <exception cref="InvalidOperationException">For a given supertype instance a corresponding subtype instance must
		/// not be specified more than once per subtype.</exception>
		private static void ValidateAttachedSubtypeInstance(ObjectType subtype, ObjectInstance subtypeInstance, ObjectInstance supertypeInstance)
		{
			// Verify that the supertype instance is associated with only one EntitySubtypeInstance
			// on the subtype. Note that the size of the supertypeInstance.EntitySubtypeInstanceCollection
			// collection is limited by the total number of subtypes, whereas the subtype.EntitySubtypeInstanceCollection
			// is unbounded. We walk the smaller collection.
			foreach (ObjectInstance testInstance in supertypeInstance.EntitySubtypeInstanceCollection)
			{
				if (testInstance != subtypeInstance && testInstance.ObjectType == subtype)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionEntitySubtypeInstanceDuplicateSupertypeInstance);
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
				LinkedElementCollection<ObjectInstance> relatedSubtypeInstances = null;
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
									relatedSubtypeInstances = supertypeInstance.EntitySubtypeInstanceCollection;
								}
								foreach (ObjectInstance instance in relatedSubtypeInstances)
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
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Validate population mandatory errors when the <see cref="SetConstraint.Modality"/> of a
		/// <see cref="MandatoryConstraint"/> changes
		/// </summary>
		private static void MandatoryModalityChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetConstraint.ModalityDomainPropertyId)
			{
				foreach (Role role in ((SetConstraint)e.ModelElement).RoleCollection)
				{
					FrameworkDomainModel.DelayValidateElement(role, DelayValidateRolePopulationMandatoryError);

					Role oppositeRole = role.OppositeOrUnaryRole.Role;
					if (oppositeRole != null)
					{
						foreach (ObjectType entityType in oppositeRole.ConstraintRoleSequenceCollection.OfType<UniquenessConstraint>().Select(uniqueness => uniqueness.PreferredIdentifierFor).Where(entity => entity != null))
						{
							FrameworkDomainModel.DelayValidateElement(entityType, DelayValidateInstancesOfEntityType);
						}
					}
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
				ObjectInstance instance;
				if (rolePlayer != null && rolePlayer != (instance = roleInstance.ObjectInstance).ObjectType)
				{
					// Note that this will throw in ObjectInstanceRolePlayerChangedRule if the current ObjectType is not null
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
			if (e.DomainRole.Id == RoleInstance.RoleDomainRoleId)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionRoleInstanceEnforceInitialRole);
			}
#if !ROLEINSTANCE_ROLEPLAYERCHANGE
			Debug.Fail("A DSLTools framework bug is preventing Undo of ObjectInstance role player changes with duplicate role/instance pairs, do not call until this is resolved.");
#endif // ROLEINSTANCE_ROLEPLAYERCHANGE
			RoleInstance roleInstance = (RoleInstance)e.ElementLink;
			ObjectType rolePlayer = roleInstance.Role.RolePlayer;
			ObjectInstance instance = roleInstance.ObjectInstance;
			if (rolePlayer != null &&
				rolePlayer != instance.ObjectType &&
				CopyMergeUtility.GetIntegrationPhase(roleInstance.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				// Note that this will throw in ObjectInstanceRolePlayerChangedRule the current ObjectType is not null
				instance.ObjectType = rolePlayer;
			}
			FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
			FrameworkDomainModel.DelayValidateElement(roleInstance, DelayValidateRemovePopulationMandatoryError);
			instance = (ObjectInstance)e.OldRolePlayer;
			instance.DelayValidateIfEmpty();
			FrameworkDomainModel.DelayValidateElement(instance, DelayValidateInstancePopulationMandatoryError);
		}

		/// <summary>
		/// ChangeRule: typeof(ObjectInstance)
		/// </summary>
		private static void IdentityTagChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propId = e.DomainProperty.Id;
			if (propId == ObjectInstance.IdentityTagDomainPropertyId)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateNamePartChanged);
			}
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
			ObjectInstance objectInstance = roleInstance.ObjectInstance;
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
					ReadOnlyCollection<RoleInstance> instances = RoleInstance.GetLinksToObjectInstanceCollection(role);
					int instanceCount = instances.Count;
					for (int i = instanceCount - 1; i >= 0; --i)
					{
						Debug.Assert(instances[i].ObjectInstance.ObjectType == rolePlayer);
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
						subtypeFact.FactInstanceCollection.Clear();
					}
				}
			}
		}
		partial class EntitySubtypeInstanceDeletingRuleClass
		{
			private bool myIsDisabled;
			/// <summary>
			/// DeletingRule: typeof(ObjectInstance)
			/// When an EntitySubtypeInstance is deleting, delete any subtype instances
			/// attached to the same supertype instance.
			/// </summary>
			private void EntitySubtypeInstanceDeletingRule(ElementDeletingEventArgs e)
			{
				if (myIsDisabled)
				{
					return;
				}
				myIsDisabled = true;
				try
				{
					ObjectInstance subtypeInstance = (ObjectInstance)e.ModelElement;
					ObjectType subtype;
					ObjectInstance supertypeInstance;
					if (null != (subtype = subtypeInstance.ObjectType) &&
						!subtype.IsDeleting &&
						null != (supertypeInstance = subtypeInstance.SupertypeInstance) &&
						!supertypeInstance.IsDeleting)
					{
						LinkedElementCollection<ObjectInstance> allSubtypeInstances = null;
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
										allSubtypeInstances = supertypeInstance.EntitySubtypeInstanceCollection;
										allSubtypeInstancesCount = allSubtypeInstances.Count;
										if (allSubtypeInstancesCount < 2)
										{
											// The only one left is the one currently being deleted for this notification
											return ObjectTypeVisitorResult.Stop;
										}
									}
									for (int i = allSubtypeInstancesCount - 1; i >= 0; --i)
									{
										ObjectInstance otherInstance = allSubtypeInstances[i];
										if (!otherInstance.IsDeleting && otherInstance.ObjectType == subSubtype)
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
												allSubtypeInstances = supertypeInstance.EntitySubtypeInstanceCollection;
												allSubtypeInstancesCount = allSubtypeInstances.Count;
											}
											for (int i = 0; i < allSubtypeInstancesCount; ++i)
											{
												ObjectInstance instance = allSubtypeInstances[i];
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
		[DelayValidatePriority(4)] // Validate subtype instances after all other instances are in place
		private static void DelayValidateSubtypeInstance(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ObjectInstance subtypeInstance = (ObjectInstance)element;
				ObjectType subtype;
				ObjectType supertype;
				ObjectInstance supertypeInstance;
				UniquenessConstraint pid;
				ObjectType preferredFor;
				if (null == (subtype = subtypeInstance.ObjectType) ||
					null == (supertypeInstance = subtypeInstance.SupertypeInstance) ||
					null == (supertype = supertypeInstance.EntityType) ||
					null == (pid = subtype.ResolvedPreferredIdentifier) ||
					null == (preferredFor = pid.PreferredIdentifierFor) ||
					preferredFor != supertype)
				{
					// UNDONE: NOW Kill EntityTypeRoleInstance Preserve ObjectInstance
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
								LinkedElementCollection<ObjectInstance> intermediateInstances = intermediateSupertype.ObjectInstanceCollection;
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
									ObjectInstance.GetSubtypeInstance(supertypeInstance, intermediateSupertype, false, true);
								}
							}
							// Always skip, only do one level at a time
							return ObjectTypeVisitorResult.SkipChildren;
						});
				}
			}
		}
		[DelayValidatePriority(4)] // Validate subtype instances after all other instances are in place
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
					// UNDONE: NOW Kill EntityTypeRoleInstance. Preserve instances through instance kind switch
					subtype.ObjectInstanceCollection.Clear();
				}
				if (preferredFor != null && preferredFor != subtype)
				{
					// Make sure that the instances resolve to the identifying supertype
					LinkedElementCollection<ObjectInstance> subtypeInstances = subtype.ObjectInstanceCollection;
					int subtypeInstanceCount = subtypeInstances.Count;
					for (int i = subtypeInstanceCount - 1; i >= 0; --i)
					{
						ObjectInstance subtypeInstance = subtypeInstances[i];
						ObjectInstance supertypeInstance;
						if (null == (supertypeInstance = subtypeInstance.SupertypeInstance) ||
							preferredFor != supertypeInstance.EntityType)
						{
							// UNDONE: NOW Kill EntityTypeRoleInstance. Preserve instances through instance kind switch
							subtypeInstance.Delete();
						}
					}

					// Make sure that, for each of the remaining instances, all intermediate subtypes also
					// are also linked to the identifying supertype instances
					subtypeInstanceCount = subtypeInstances.Count;
					if (subtypeInstanceCount != 0)
					{
						ObjectInstance[] supertypeInstances = null;
						BitTracker instanceMatches = default(BitTracker);
						IComparer<ObjectInstance> comparer = null;
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
										supertypeInstances = new ObjectInstance[subtypeInstanceCount];
										instanceMatches = new BitTracker(subtypeInstanceCount);
										for (int i = 0; i < subtypeInstanceCount; ++i)
										{
											supertypeInstances[i] = subtypeInstances[i].SupertypeInstance;
										}
										Array.Sort<ObjectInstance>(supertypeInstances, comparer = ModelElementIdComparer<ObjectInstance>.Instance);
									}
									int matched = 0;
									foreach (ObjectInstance intermediateInstance in supertype.ObjectInstanceCollection)
									{
										ObjectInstance boundInstance = intermediateInstance.SupertypeInstance;
										if (boundInstance.EntityType == preferredFor) // Sanity check, will be cleaned up by other iterations of this function
										{
											int matchingIndex = Array.BinarySearch<ObjectInstance>(supertypeInstances, boundInstance, comparer);
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
												ObjectInstance.GetSubtypeInstance(supertypeInstances[i], supertype, false, true);
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
		/// AddRule: typeof(ObjectTypeHasObjectInstance)
		/// Verify that implied instances were added by us, and that
		/// objectified instances all get an error.
		/// </summary>
		private static void ObjectInstanceAddedRule(ElementAddedEventArgs e)
		{
			ObjectTypeHasObjectInstance link = (ObjectTypeHasObjectInstance)e.ModelElement;
			ObjectType objectType = link.ObjectType;
			ObjectInstance instance = link.ObjectInstance;
			if (null != objectType.NestedFactType)
			{
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateObjectifiedInstanceRequiredError);
			}

			if (objectType.DataType != null)
			{
				// Confirm that the given ValueInstance.LexicalValue is of the datatype defined in objectType.DataType
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateCompatibleValueInstanceError);
			}

			FrameworkDomainModel.DelayValidateElement(instance, ObjectInstance.DelayValidateInstancePopulationMandatoryError);

			// Remove external creation dependency between attaching the supertype instance and the object type.
			ObjectInstance supertypeInstance = instance.SupertypeInstance;
			if (supertypeInstance != null)
			{
				ValidateAttachedSubtypeInstance(objectType, instance, supertypeInstance);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypeHasObjectInstance)
		/// </summary>
		private static void ObjectInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectInstanceEnforceFixedRolePlayers);
		}
		#region Handle objectified FactType inclusion in generated name
		/// <summary>
		/// ChangeRule: typeof(FactInstance)
		/// Update objectified instance name
		/// </summary>
		private static void FactInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			ObjectInstance objectInstance;
			if (e.DomainProperty.Id == FactInstance.NameChangedDomainPropertyId &&
				null != (objectInstance = ((FactInstance)e.ModelElement).ObjectifyingInstance))
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
				foreach (ObjectInstance objectInstance in objectType.ObjectInstanceCollection)
				{
					// Note that these are generally done through the FactInstance, but a
					// missing ObjectificationInstance link requires us to watch the FactType directly
					FrameworkDomainModel.DelayValidateElement(objectInstance, DelayValidateNamePartChanged);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(Objectification)
		/// Update objectified instance name and validate instances
		/// </summary>
		private static void ObjectificationAddedRule(ElementAddedEventArgs e)
		{
			Objectification objectification = (Objectification)e.ModelElement;
			DelayValidateInstanceNames(objectification.NestingType);
			FrameworkDomainModel.DelayValidateElement(objectification, DelayValidateObjectificationInstances);
		}
		/// <summary>
		/// DeleteRule: typeof(Objectification), Priority=1;
		/// Remove all <see cref="ObjectificationInstance"/> links when an objectification
		/// is deleted while both of its endpoints remain intact and rename previously objectified instances.
		/// </summary>
		private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
		{
			Objectification link = (Objectification)e.ModelElement;
			ObjectType entityType;
			if (!(entityType = link.NestingType).IsDeleted)
			{
				// Note that we run this at priority 1 to let the preferred identifier
				// deletion process first, which will potentially clear the ObjectInstanceCollection
				// and leave us with less to do here.
				foreach (ObjectInstance instance in entityType.ObjectInstanceCollection)
				{
					instance.ObjectifiedInstance = null;
					instance.ObjectifiedInstanceRequiredError = null; // No need to delay validate to clear errors

					// Note that these are generally done through the FactInstance, but a
					// missing ObjectificationInstance link requires us to watch the FactType directly
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
				}
			}

			FactType factType;
			if (!(factType = link.NestedFactType).IsDeleted)
			{
				foreach (FactInstance instance in factType.FactInstanceCollection)
				{
					// All ObjectificationInstance relationships will have been taken care of by the previous loop
					instance.ObjectifyingInstanceRequiredError = null;
				}
			}
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
			ObjectInstance objectInstance = ((ObjectificationInstance)e.ModelElement).ObjectifyingInstance;
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
		private static void ObjectificationInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectificationInstance.ObjectifyingInstanceDomainRoleId)
			{
				ObjectInstance oldInstance = (ObjectInstance)e.OldRolePlayer;
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
		/// RolePlayerChangeRule: typeof(Objectification), Priority=1;
		/// Clean up <see cref="ObjectificationInstance"/> relationships for the old objectification/instance pair and reset names
		/// </summary>
		private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			// Note that we run this at priority 1 to let the preferred identifier
			// deletion process first, which will potentially clear the ObjectInstanceCollection
			// and leave us with less to do here.
			Objectification objectification = (Objectification)e.ElementLink;
			FactType factType;
			ObjectType entityType;
			if (e.DomainRole.Id == Objectification.NestingTypeDomainRoleId)
			{
				factType = objectification.NestedFactType;
				entityType = (ObjectType)e.OldRolePlayer;
				DelayValidateInstanceNames(objectification.NestingType);
			}
			else
			{
				factType = (FactType)e.OldRolePlayer;
				entityType = objectification.NestingType;
			}
			foreach (ObjectInstance instance in entityType.ObjectInstanceCollection)
			{
				instance.ObjectifiedInstance = null;
				instance.ObjectifiedInstanceRequiredError = null; // No need to delay validate
				FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
			}
			foreach (FactInstance instance in factType.FactInstanceCollection)
			{
				// All ObjectificationInstance relationships will have been taken care of by the previous loop
				instance.ObjectifyingInstanceRequiredError = null;
			}
			FrameworkDomainModel.DelayValidateElement(objectification, DelayValidateObjectificationInstances);
		}
		/// <summary>
		/// Validate an <see cref="ObjectInstance"/> using the <see cref="DelayValidateIfEmpty"/> method.
		/// </summary>
		protected static void DelayValidateEmptyInstance(ObjectInstance objectInstance)
		{
			objectInstance.DelayValidateIfEmpty();
		}
		/// <summary>
		/// If this identifier is empty, then verify that it is both associated with
		/// a <see cref="FactInstance"/> and included in another instance definition.
		/// </summary>
		private void DelayValidateIfEmpty()
		{
			if (!IsDeleted && this.EntityType != null)
			{
				ObjectInstance supertypeInstance;
				if (null != (supertypeInstance = SupertypeInstance))
				{
					DelayValidateEmptyInstance(supertypeInstance);
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(this, DelayValidateTooFewEntityRoleInstancesError);
				}
			}
		}
		#endregion // Handle objectified FactType inclusion in generated name
		#region Regenerate any auto identity tags when the name changes
		/// <summary>
		/// ChangeRule: typeof(ObjectType)
		/// Regenerate auto identity tags for a name change
		/// </summary>
		private static void ObjectTypeNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ObjectType.NameDomainPropertyId)
			{
				ObjectType objectType = (ObjectType)e.ModelElement;
				if (!objectType.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(objectType, DelayUpdateAutoIdentityTags);
				}
			}
		}
		#endregion // Regenerate any auto identity tags when the name changes
		#endregion // General ObjectInstance Rules
		#region Value Instance Rules
		/// <summary>
		/// AddRule: typeof(ValueTypeHasDataType)
		/// When the DataType is added, recheck the valueInstance values
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
				// Move Value data type the IdenityTag field
				foreach (ObjectInstance instance in valueType.ObjectInstanceCollection)
				{
					string value = instance.Value;
					if (string.IsNullOrEmpty(value))
					{
						instance.Delete();
					}
					else
					{
						instance.IdentityTag = value;
						instance.AutoIdentityTag = false;
						instance.Value = string.Empty;
					}
				}
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
			foreach (ObjectInstance valueInstance in valueType.ObjectInstanceCollection)
			{
				string value = valueInstance.Value;
				string identityTag;
				if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(identityTag = valueInstance.IdentityTag))
				{
					valueInstance.Value = identityTag;
					valueInstance.IdentityTag = string.Empty;
					valueInstance.AutoIdentityTag = false;
				}
				if (clearErrors)
				{
					valueInstance.CompatibleValueInstanceError = null;
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(valueInstance, DelayValidateCompatibleValueInstanceError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// Process a data type change the same as a data type add
		/// </summary>
		private static void ValueTypeHasDataTypeRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				ProcessValueTypeHasDataTypeAdded(e.ElementLink as ValueTypeHasDataType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ObjectInstance)
		/// Whenever the lexical value of a ValueInstance changes, make sure it can be parsed as the current DataType
		/// </summary>
		private static void ValueInstanceValueChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ObjectInstance.ValueDomainPropertyId)
			{
				ObjectInstance instance = (ObjectInstance)e.ModelElement;
				if (instance.ObjectType?.DataType != null)
				{
					instance.InvariantValue = ""; // Clear the invariant value so that it can be reset when the value changes
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateCompatibleValueInstanceError);
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateNamePartChanged);
				}
			}
		}
		#endregion // Value Instance Rules
		#region Entity Instance Rules
		/// <summary>entit
		/// DeletingRule: typeof(ObjectInstance)
		/// If an implied EntityInstance is deleted, then also delete the opposite
		/// ObjectInstance it is associated with for some patterns.
		/// </summary>
		private static void EntityInstanceDeletingRule(ElementDeletingEventArgs e)
		{
			// UNDONE: NOW Kill EntityTypeRoleInstance This and EntitySubtypeInstanceDeletingRule should be one function, they're
			// both doing the same thing. Also, setting a DataType on the type should also clear out the supertype entity relationships,
			// so this code can also be used for that purpose.s
			ObjectInstance instance = e.ModelElement as ObjectInstance;
			ObjectType parent = instance.ObjectType;
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
						RoleInstance identifyingRoleInstance = instance.FindIdentifyingRoleInstance(identifierRole);
						if (identifyingRoleInstance != null)
						{
							ObjectInstance oppositeInstance = identifyingRoleInstance.ObjectInstance;
							FactInstance identifyingFact = identifyingRoleInstance.FactInstance;
							if (!oppositeInstance.IsDeleting)
							{
								oppositeInstance.Delete();
							}
							if (identifyingFact != null && !identifyingFact.IsDeleting)
							{
								identifyingFact.Delete();
							}
						}
					}
				}
				else
				{
					// The opposite identifier role is optional, so population mandatory errors
					// are possible. Validate the opposite instances.
					RoleInstance identifyingRoleInstance = instance.FindIdentifyingRoleInstance(identifierRole);
					if (identifyingRoleInstance != null)
					{
						ObjectInstance oppositeInstance = identifyingRoleInstance.ObjectInstance;
						FactInstance identifyingFact = identifyingRoleInstance.FactInstance;
						if (!oppositeInstance.IsDeleting && oppositeInstance.ObjectType == identifierPlayer)
						{
							FrameworkDomainModel.DelayValidateElement(oppositeInstance, ObjectInstance.DelayValidateInstancePopulationMandatoryError);
						}
						if (identifyingFact != null && !identifyingFact.IsDeleting)
						{
							identifyingFact.Delete();
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Clean up ValueInstances when an ObjectType becomes an EntityType
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
		}
		[DelayValidatePriority(3)] // Needs to run after implied mandatory validation on the role players and removal of subtype instances
		private static void DelayValidatePreferredIdentifier(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				UniquenessConstraint preferredIdentifier = (UniquenessConstraint)element;
				ObjectType identifiedObjectType;
				if (null != (identifiedObjectType = preferredIdentifier.PreferredIdentifierFor))
				{
					// An ObjectType with a preferred identifier cannot be an entity subtype instance
					foreach (ObjectInstance instance in identifiedObjectType.ObjectInstanceCollection)
					{
						EntitySubtypeInstanceHasSupertypeInstance supertypeInstanceLink = EntitySubtypeInstanceHasSupertypeInstance.GetLinkToSupertypeInstance(instance);
						if (supertypeInstanceLink != null)
						{
							supertypeInstanceLink.Delete();
						}
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Clean up EntityInstances when an ObjectType becomes a ValueType
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
			ProcessPreferredIdentifierDeleted(link, oldObjectType);
			ProcessPreferredIdentifierAdded(link);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is added to an EntityType's preferred identifier collection, all EntityInstances of that EntityType
		/// should be revalidated to ensure that they form a complete instance of the EntityType
		///
		/// If an objectified internal preferred identifier is extended, then
		/// automatically fill in the additional roles based on the associated
		/// FactInstance collection.
		/// </summary>
		private static void PreferredIdentifierRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			UniquenessConstraint pid;
			MandatoryConstraint mandatory;
			ObjectType entityType;
			Role newRole;
			Role oppositeRole;
			if (null != (pid = sequence as UniquenessConstraint))
			{
				if (null != (entityType = pid.PreferredIdentifierFor))
				{
					// Structural check
					foreach (ObjectInstance entityInstance in entityType.ObjectInstanceCollection)
					{
						FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityRoleInstancesError);
					}
				}
			}
			else if (
				null != (mandatory = sequence as MandatoryConstraint) &&
				mandatory.Modality == ConstraintModality.Alethic &&
				null != (entityType = (newRole = link.Role).RolePlayer) &&
				null != (pid = entityType.PreferredIdentifier) &&
				null != (oppositeRole = newRole.OppositeOrUnaryRole?.Role) &&
				pid.RoleCollection.Contains(oppositeRole)
			)
			{
				foreach (ObjectInstance entityInstance in entityType.ObjectInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityRoleInstancesError);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a Role is removed from an EntityType's preferred identifier collection, it will
		/// automatically propagate and destroy any role instances. This rule
		/// will force deletion of any EntityInstances which no longer have
		/// any EntityTypeRoleInstances.
		/// </summary>
		private static void PreferredIdentifierRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			Role removedRole = link.Role;
			if (removedRole.IsDeleted)
			{
				return;
			}

			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			UniquenessConstraint pid;
			MandatoryConstraint mandatory;
			ObjectType entityType;
			ObjectType validateEntityType = null;
			Role oppositeRole;
			if (null != (pid = sequence as UniquenessConstraint))
			{
				validateEntityType = pid.PreferredIdentifierFor;
			}
			else if (
				null != (mandatory = sequence as MandatoryConstraint) &&
				mandatory.Modality == ConstraintModality.Alethic &&
				null != (oppositeRole = removedRole.OppositeOrUnaryRole?.Role) &&
				null != (entityType = removedRole.RolePlayer) &&
				null != (pid = entityType.PreferredIdentifier) &&
				pid.RoleCollection.Contains(oppositeRole)
			)
			{
				validateEntityType = entityType;
			}

			if (null != validateEntityType)
			{
				foreach (ObjectInstance entityInstance in validateEntityType.ObjectInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityRoleInstancesError);
				}
			}
		}
		[DelayValidatePriority(1)]
		private static void DelayValidateInstancesOfEntityType(ModelElement element)
		{
			ObjectType entityType = (ObjectType)element;
			if (entityType.PreferredIdentifier != null) // Match the entity instance pattern
			{
				foreach (ObjectInstance instance in entityType.ObjectInstanceCollection)
				{
					FrameworkDomainModel.DelayValidateElement(instance, DelayValidateTooFewEntityRoleInstancesError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactInstanceHasRoleInstance)
		/// Ensure that a RoleInstance for an identifying fact revalidates an identified entity instance.
		/// </summary>
		private static void IdentifyingRoleInstanceAddedRule(ElementAddedEventArgs e)
		{
			ObjectInstance entityInstance = ((FactInstanceHasRoleInstance)e.ModelElement).RoleInstance.IdentifiedEntityInstance;
			if (entityInstance != null)
			{
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityRoleInstancesError);
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateNamePartChanged);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(FactInstanceHasRoleInstance)
		/// </summary>
		private static void IdentifyingRoleInstanceDeletingRule(ElementDeletingEventArgs e)
		{
			RoleInstance roleInstance = ((FactInstanceHasRoleInstance)e.ModelElement).RoleInstance;
			ObjectInstance entityInstance = roleInstance.IdentifiedEntityInstance;
			if (entityInstance != null)
			{
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateTooFewEntityRoleInstancesError);
				FrameworkDomainModel.DelayValidateElement(entityInstance, DelayValidateNamePartChanged);
			}

			// Note that InstancePopulationMandatoryError is handled through the fact instance.
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
		#endregion // Entity Instance Rules
	}
	#endregion // ObjectInstance class
	#region RoleInstance class
	partial class RoleInstance
	{
		#region Helper Methods
		/// <summary>
		/// Find the <see cref="ObjectInstance"/> that is directly identified by this <see cref="RoleInstance"/>.
		/// The identified instance can either be the associated <see cref="ObjectInstance"/>or the instance of the opposite role.
		/// </summary>
		/// <returns>The identified instance, or null. This looks only at immediately identified role players, not indirectly identified subtypes.</returns>
		public ObjectInstance IdentifiedEntityInstance
		{
			get
			{
				Role role = this.Role;
				FactType factType = role.FactType;
				ObjectType objectifiedType = factType.NestingType;
				UniquenessConstraint pid;
				if (objectifiedType != null)
				{
					pid = objectifiedType.PreferredIdentifier;
					if (pid != null && pid.RoleCollection.Contains(role))
					{
						return this.FactInstance.ObjectifyingInstance;
					}
				}

				pid = role.RolePlayer?.PreferredIdentifier;
				LinkedElementCollection<Role> pidRoles;
				if (pid != null)
				{
					pidRoles = pid.RoleCollection;
					int roleCount = pidRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						if (pidRoles[i].FactType == factType)
						{
							// Note that this covers a unary role in an identifier
							return this.ObjectInstance;
						}
					}
				}

				// This role instance is not bound to an identified role player. Look on the opposite role and see if this
				// is part of a preferred identifier.
				Role oppositeRole = role.OppositeRole as Role;
				if (oppositeRole?.RolePlayer?.PreferredIdentifier?.RoleCollection.Contains(role) == true)
				{
					return this.FactInstance.FindRoleInstance(oppositeRole)?.ObjectInstance;
				}
				return null;
			}
		}
		#endregion // Helper Methods
	}
	#endregion // RoleInstance class
	#region FactInstanceHasRoleInstance class
	partial class FactInstanceHasRoleInstance
	{
		#region Helper Methods
		/// <summary>
		/// Similar to <see cref="RoleInstance.IdentifiedEntityInstance"/> but handles deleted states
		/// better and always returns the identifying role, meaning the role that is in the preferred
		/// identifier. This is designed for use during events, so there is liberal checking for deletion.
		/// This can resolve a deletion of one of the two identifying role instances in a binary fact, but
		/// does not reliably resolve if the full fact is deleted.
		/// </summary>
		/// <remarks>This replaces a built-in lookup with the earlier form of sample population, where
		/// entity instances were bound directly to their identifying values. In this earlier structure
		/// there was never an identifying fact bound to the entity instance and the 'entity role instance'
		/// was on the identifying role, so there was no relationship binding the near (identified) role with
		/// the role instance. With the new structure both roles are populated, but we don't really care
		/// which out is represented because we're getting the identifying information, namely the <see cref="ObjectInstance"/>
		/// and the role it identifies.</remarks>
		/// <param name="identifiedInstance">The identified entity instance</param>
		/// <param name="identifyingRole">The identifying role, opposite an identified object.</param>
		public void FindEntityWithIdentifyingRole(out ObjectInstance identifiedInstance, out Role identifyingRole)
		{
			identifiedInstance = null;
			identifyingRole = null;
			RoleInstance roleInstance = this.RoleInstance;
			Role role = roleInstance.Role;
			FactType factType = role.FactType;
			if (!role.IsDeleted && !factType.IsDeleted)
			{
				UniquenessConstraint pid;
				ObjectType objectifiedType = factType.NestingType;
				if (objectifiedType != null)
				{
					pid = objectifiedType.PreferredIdentifier;
					if (pid != null && pid.RoleCollection.Contains(role))
					{
						FactInstance factInstance = roleInstance.FactInstance;
						identifiedInstance = roleInstance.FactInstance.ObjectifyingInstance;
						if (identifiedInstance != null && !identifiedInstance.IsDeleted)
						{
							identifyingRole = role;
						}
						return;
					}
				}

				pid = role.RolePlayer?.PreferredIdentifier;
				LinkedElementCollection<Role> pidRoles;
				if (pid != null)
				{
					// Check the role potentially played by an identified entity type
					pidRoles = pid.RoleCollection;
					int pidRoleCount = pidRoles.Count;
					for (int i = 0; i < pidRoleCount; ++i)
					{
						Role pidRole = pidRoles[i];
						if (pidRole.FactType == factType)
						{
							// Note that this covers a unary role in an identifier
							ObjectInstance entityInstance = roleInstance.ObjectInstance;
							if (entityInstance != null && !entityInstance.IsDeleted)
							{
								identifiedInstance = entityInstance;
								identifyingRole = pidRole;
							}
							return;
						}
					}

					// Check the opposite role and see if it is part of a preferred identifier
					Role oppositeRole = role.OppositeRole as Role;
					if (oppositeRole?.RolePlayer?.PreferredIdentifier?.RoleCollection.Contains(role) == true)
					{
						FactInstance fact = this.FactInstance;
						if (!fact.IsDeleted)
						{
							ObjectInstance entityInstance = fact.FindRoleInstance(oppositeRole)?.ObjectInstance;
							if (entityInstance != null && !entityInstance.IsDeleted)
							{
								identifiedInstance = entityInstance;
								identifyingRole = oppositeRole;
							}
						}
					}
				}
			}
		}
		#endregion // Helper Methods

	}
	#endregion // FactInstanceHasRoleInstance class
	#region ObjectificationInstance class
	partial class ObjectificationInstance
	{
		#region Rule methods
		/// <summary>
		/// AddRule: typeof(ObjectificationInstance)
		/// Verify that an <see cref="ObjectificationInstance"/> is consistent with the
		/// <see cref="Objectification"/> link between the associated <see cref="FactType"/>
		/// and <see cref="ObjectType"/>
		/// </summary>
		private static void ObjectificationInstanceAddedRule(ElementAddedEventArgs e)
		{
			ObjectificationInstance link = (ObjectificationInstance)e.ModelElement;
			FactInstance factInstance = link.ObjectifiedInstance;
			ObjectInstance objectInstance = link.ObjectifyingInstance;
			FactType factType;
			ObjectType entityType;
			if (null == (factType = factInstance.FactType) ||
				null == (entityType = objectInstance.ObjectType) ||
				factType.NestingType != entityType)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceIncompleteRolePlayers);
			}
			// The entityInstance has a trivial error condition (any FactInstance is required, even empty ones)
			// However, the factInstance condition is non-trivial as an ObjectifyingInstanceRequiredError
			// is displayed if the non-implied identifier instance is empty.
			FrameworkDomainModel.DelayValidateElement(factInstance, FactInstance.DelayValidateObjectifyingInstanceRequiredError);
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
			FactInstance factInstance = link.ObjectifiedInstance;
			ObjectInstance objectInstance = link.ObjectifyingInstance;
			FactType factType;
			ObjectType entityType;
			EntityTypeHasPreferredIdentifier pidLink;
			Objectification objectification;
			UniquenessConstraint pid;
			LinkedElementCollection<FactType> pidFactTypes;
			FactType identifierFactType;
			bool factInstanceDeleting = factInstance.IsDeleting;
			bool entityInstanceDeleting = objectInstance.IsDeleting;
			if (factInstanceDeleting &&
				!entityInstanceDeleting &&
				null != (factType = factInstance.FactType) &&
				!factType.IsDeleting &&
				null != (entityType = objectInstance.ObjectType) &&
				!entityType.IsDeleting &&
				null != (objectification = entityType.Objectification) && // Allow invalidated links to be blown away without turning off this rule
				objectification.NestedFactType == factType &&
				null != (pidLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifier(entityType)) && // A subtype instance does not have an internal identifier
				!pidLink.IsDeleting &&
				(pid = pidLink.PreferredIdentifier).IsInternal &&
				1 == (pidFactTypes = pid.FactTypeCollection).Count &&
				(identifierFactType = pidFactTypes[0]) == factType)
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
						// then an empty FactInstance must be created without an attached RoleInstance.
						// These empty instances need to be deleted if the objectification link or the corresponding
						// entity instance is deleted.
						factInstance.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(factInstance, FactInstance.DelayValidateObjectifyingInstanceRequiredError);
					}
				}

				if (!entityInstanceDeleting && objectInstance.ObjectType?.PreferredIdentifier != null)
				{
					if (string.IsNullOrEmpty(objectInstance.IdentityTag) && !objectInstance.HasIdentifyingRoleInstance)
					{
						// The same logic applies to the other end: an empty EntityInstance may need
						// to be created so that it can be associated with a populated FactInstance
						objectInstance.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(objectInstance, ObjectInstance.DelayValidateObjectifiedInstanceRequiredError);
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
			FactInstance factInstance = link.ObjectifiedInstance;
			ObjectInstance objectInstance = link.ObjectifyingInstance;
			if (e.DomainRole.Id == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
			{
				FactInstance oldFactInstance = (FactInstance)e.OldRolePlayer;
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
					FrameworkDomainModel.DelayValidateElement(oldFactInstance, FactInstance.DelayValidateObjectifyingInstanceRequiredError);
				}
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, FactInstance.DelayValidateObjectifyingInstanceRequiredError);
			}
			else
			{
				ObjectInstance oldObjectInstance = (ObjectInstance)e.OldRolePlayer;
				if (oldObjectInstance.ObjectType != objectInstance.ObjectType)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationInstanceIncompleteRolePlayers);
				}
				if (oldObjectInstance != null &&
					oldObjectInstance.ObjectType?.PreferredIdentifier != null && // Match the directly identified entity instance pattern
					string.IsNullOrEmpty(oldObjectInstance.IdentityTag) &&
					!oldObjectInstance.HasIdentifyingRoleInstance)
				{
					// See comments in ObjectificationInstanceDeletingRule
					oldObjectInstance.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, ObjectInstance.DelayValidateObjectifiedInstanceRequiredError);
				}
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, ObjectInstance.DelayValidateObjectifiedInstanceRequiredError);
			}
		}
		#endregion // Rule methods
	}
	#endregion // ObjectificationInstance class
	#region FactInstanceImplication struct
	/// <summary>
	/// Helper structure to determine when the set of <see cref="FactInstance"/>
	/// elements associated with a <see cref="FactType"/> is implied by the population
	/// of another <see cref="ObjectType"/>. Implied population occurs if the <see cref="FactType"/>
	/// is used as part of the identification scheme for the implying entity type or subtype.
	/// </summary>
	public struct FactInstanceImplication
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
		/// objectification, then specifiy the associated <see cref="RoleProxy"/>.
		/// </summary>
		public readonly RoleProxy ImpliedProxyRole;
		/// <summary>
		/// If this is implied by an entity type and the identifying preferred identifier
		/// has more than one role then set this to the identifying role in this fact type.
		/// </summary>
		public readonly Role ImpliedIdentifyingRole;
		#endregion // Fields
		#region Constructor
		/// <summary>
		/// Get information about the <see cref="ObjectType">EntityType</see>
		/// that provides an implied population for this <paramref name="factType"/>
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to retrieve implied
		/// information for.</param>
		public FactInstanceImplication(FactType factType)
		{
			ObjectType impliedByEntityType = null;
			ObjectType identifyingSuperType = null;
			RoleProxy impliedProxyRole = null;
			Role identifyingRole = null;
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
					null != (impliedProxyRole = (testRole = factRoles[0]) as RoleProxy ?? (testRole = factRoles[1]) as RoleProxy))
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
							LinkedElementCollection<Role> identifyingRoles = uc.RoleCollection;
							int roleCount = identifyingRoles.Count;
							if (roleCount > 1)
							{
								for (int i = 0; i < roleCount; ++i)
								{
									Role testIdentifyingRole = identifyingRoles[i];
									if (testIdentifyingRole.FactType == factType)
									{
										identifyingRole = testIdentifyingRole;
										break;
									}
								}
							}
							break;
						}
					}
				}
			}
			ImpliedByEntityType = impliedByEntityType;
			IdentifyingSupertype = identifyingSuperType;
			ImpliedProxyRole = impliedProxyRole;
			ImpliedIdentifyingRole = identifyingRole;
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
			if (obj is FactInstanceImplication)
			{
				return Equals((FactInstanceImplication)obj);
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
		public bool Equals(FactInstanceImplication other)
		{
			return ImpliedByEntityType == other.ImpliedByEntityType && IdentifyingSupertype == other.IdentifyingSupertype && ImpliedProxyRole == other.ImpliedProxyRole;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(FactInstanceImplication left, FactInstanceImplication right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(FactInstanceImplication left, FactInstanceImplication right)
		{
			return !left.Equals(right);
		}
		#endregion // Equality
	}
	#endregion // FactInstanceImplication struct
}
