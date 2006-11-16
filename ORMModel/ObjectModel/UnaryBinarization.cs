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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	partial class ImplicitBooleanRole
	{
		/// <summary>
		/// Replaces this <see cref="ImplicitBooleanRole"/> with a real <see cref="Role"/>
		/// </summary>
		/// <returns></returns>
		public Role ConvertToRealRole()
		{
			// Create the replacement role
			Role replacementRole = new Role(this.Store, null);

			// Replace this ImplicitBooleanRole with it
			Role.ReplaceRole(this, replacementRole);

			// Delete this ImplicitBooleanRole
			this.Delete();

			return replacementRole;
		}
	}

	/// <summary>
	/// Provides support for the binarization of unary <see cref="FactType"/>s.
	/// </summary>
	public static class UnaryBinarizationUtility
	{
		private static string GetImplicitBooleanValueTypeName(Role unaryRole)
		{
			Debug.Assert(!(unaryRole is ImplicitBooleanRole));
			string unaryRoleName = unaryRole.Name;
			ObjectType unaryRolePlayer = unaryRole.RolePlayer;
			if (unaryRolePlayer != null)
			{
				if (string.IsNullOrEmpty(unaryRoleName))
				{
					FactType unaryFactType = unaryRole.FactType;
					if (unaryFactType != null)
					{
						LinkedElementCollection<ReadingOrder> readingOrderCollection = unaryFactType.ReadingOrderCollection;
						if (readingOrderCollection.Count > 0)
						{
							ReadingOrder readingOrder = readingOrderCollection[0];
							LinkedElementCollection<Reading> readings = readingOrder.ReadingCollection;
							if (readings.Count > 0)
							{
								return string.Format(CultureInfo.InvariantCulture, readings[0].Text, unaryRolePlayer.Name);
							}
						}
					}
				}

				// UNDONE: Localize the space? (Some languages may not use spaces between words.)
				return unaryRolePlayer.Name + " " + unaryRoleName;
			}
			return unaryRoleName;
		}
		private static Role GetUnaryRole(LinkedElementCollection<RoleBase> roleCollection)
		{
			int roleCollectionCount = roleCollection.Count;
			Debug.Assert(roleCollectionCount > 0 && roleCollectionCount <= 2);

			Role firstRole = (Role)roleCollection[0];
			return (roleCollectionCount == 1 ? firstRole : (firstRole is ImplicitBooleanRole ? (Role)roleCollection[1] : firstRole));
		}
		private static ImplicitBooleanRole GetImplicitBooleanRole(FactType binarizedUnaryFactType)
		{
			return GetImplicitBooleanRole(binarizedUnaryFactType.RoleCollection);
		}
		private static ImplicitBooleanRole GetImplicitBooleanRole(LinkedElementCollection<RoleBase> roleCollection)
		{
			Debug.Assert(roleCollection.Count >= 2);
			foreach (RoleBase roleBase in roleCollection)
			{
				ImplicitBooleanRole implicitBooleanRole = roleBase as ImplicitBooleanRole;
				if (implicitBooleanRole != null)
				{
					return implicitBooleanRole;
				}
			}
			return null;
		}

		/// <summary>
		/// Binarizes the unary <see cref="FactType"/> specified by <paramref name="unaryFactType"/>, defaulting
		/// to using closed-world assumption. The caller is responsible for making sure <paramref name="unaryFactType"/>
		/// is in fact a unary fact type.
		/// </summary>
		internal static void BinarizeUnary(FactType unaryFactType)
		{
			Store store = unaryFactType.Store;
			LinkedElementCollection<RoleBase> roleCollection = unaryFactType.RoleCollection;
			Debug.Assert(roleCollection.Count == 1, "Unaries should only have one role.");
			
			Role unaryRole = (Role)roleCollection[0];
			string implicitBooleanValueTypeName = GetImplicitBooleanValueTypeName(unaryRole);

			// Setup the mandatory constraint (for closed-world assumption)
			MandatoryConstraint mandatoryConstraint = MandatoryConstraint.CreateSimpleMandatoryConstraint(unaryRole);
			mandatoryConstraint.Name = implicitBooleanValueTypeName;
			
			// Setup the boolean role (to make the FactType a binary)
			ImplicitBooleanRole implicitBooleanRole = new ImplicitBooleanRole(store, null);
			implicitBooleanRole.Name = unaryRole.Name;
			roleCollection.Add(implicitBooleanRole);

			// Setup the uniqueness constraint (to make the newly binarized FactType valid)
			UniquenessConstraint uniquenessConstraint = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
			unaryRole.ConstraintRoleSequenceCollection.Add(uniquenessConstraint);
			uniquenessConstraint.Name = implicitBooleanValueTypeName;

			// Setup the boolean value type (because the boolean role needs a role player)
			ObjectType implicitBooleanValueType = new ObjectType(store, null);
			implicitBooleanValueType.Name = implicitBooleanValueTypeName;
			implicitBooleanValueType.DataType = store.ElementDirectory.FindElements<TrueOrFalseLogicalDataType>(false)[0];

			// Make the boolean value type the role player for the implicit boolean role
			implicitBooleanRole.RolePlayer = implicitBooleanValueType;
		}

		

		/// <summary>
		/// Reverses the binarization process performed by <see cref="BinarizeUnary"/>. Typically used when
		/// <paramref name="binarizedUnaryFactType"/> no longer qualifies to be a binarized unary <see cref="FactType"/>.
		/// </summary>
		private static void DebinarizeUnary(LinkedElementCollection<RoleBase> binarizedUnaryFactRoleCollection)
		{
			// UNDONE: We need to make sure the debinarization happens BEFORE the implied Objectification rules run on the binarized unary FactType.

			bool foundImplicitBoolean = false;
			for (int i = binarizedUnaryFactRoleCollection.Count - 1; i > 0; i--)
			{
				ImplicitBooleanRole implicitBooleanRole = binarizedUnaryFactRoleCollection[i] as ImplicitBooleanRole;
				if (implicitBooleanRole != null)
				{
					foundImplicitBoolean = true;

					ObjectType implicitBooleanValueType = implicitBooleanRole.RolePlayer;
					if (implicitBooleanValueType != null)
					{
						// Delete the implicit boolean value type (which will also remove any value constraints on it)
						implicitBooleanValueType.Delete();
					}

					// Delete the implicit boolean role
					implicitBooleanRole.Delete();
				}
			}

			if (foundImplicitBoolean)
			{
				int binarizedUnaryFactRoleCollectionCount = binarizedUnaryFactRoleCollection.Count;
				for (int i = 0; i < binarizedUnaryFactRoleCollectionCount; i++)
				{
					Role role = binarizedUnaryFactRoleCollection[i] as Role;
					if (role != null)
					{
						ConstraintRoleSequence singleRoleAlethicUniquenessConstraint = role.SingleRoleAlethicUniquenessConstraint;
						if (singleRoleAlethicUniquenessConstraint != null)
						{
							// Delete the uniqueness constraint
							singleRoleAlethicUniquenessConstraint.Delete();

							MandatoryConstraint simpleMandatoryConstraint = role.SimpleMandatoryConstraint;
							if (simpleMandatoryConstraint != null && simpleMandatoryConstraint.Modality == ConstraintModality.Alethic)
							{
								// Delete the simple mandatory constraint (for closed-world assumption), if present
								simpleMandatoryConstraint.Delete();
							}
						}
					}
				}
			}
		}


		private static void ProcessFactType(FactType factType)
		{
			LinkedElementCollection<RoleBase> roleCollection = factType.RoleCollection;
			int roleCollectionCount = roleCollection.Count;

			if (roleCollectionCount == 1)
			{
				// If we have a unary, binarize it
				BinarizeUnary(factType);
				return;
			}
			else if (roleCollectionCount == 2)
			{
				// If we have a binary that has an implicit boolean role in it, make sure it matches the pattern
				ImplicitBooleanRole implicitBooleanRole = GetImplicitBooleanRole(roleCollection);
				if (implicitBooleanRole != null)
				{
					Role unaryRole = GetUnaryRole(roleCollection);
					Debug.Assert(unaryRole != null);
					if (!ValidateConstraints(unaryRole, implicitBooleanRole) || !ValidateImplictBooleanValueType(implicitBooleanRole.RolePlayer))
					{
						implicitBooleanRole.ConvertToRealRole();
					}
				}
			}
			else
			{
				// If we have an n-ary, remove any implicit boolean roles in it
				for (int i = 0; i < roleCollectionCount; i++)
				{
					ImplicitBooleanRole implicitBooleanRole = roleCollection[i] as ImplicitBooleanRole;
					if (implicitBooleanRole != null)
					{
						ObjectType implicitBooleanValueType = implicitBooleanRole.RolePlayer;
						if (implicitBooleanValueType != null)
						{
							// Delete the implicit boolean value type (which will also remove any value constraints on it)
							implicitBooleanValueType.Delete();
						}

						// Delete the implicit boolean role
						implicitBooleanRole.Delete();
					}
				}
			}
		}


		/// <summary>
		/// Checks the <see cref="IConstraint"/>s on the binarized unary <see cref="FactType"/> as specified by
		/// the <see cref="FactType"/>'s <paramref name="unaryRole"/> and the <paramref name="unaryRole"/>.
		/// </summary>
		private static bool ValidateConstraints(Role unaryRole, ImplicitBooleanRole implicitBooleanRole)
		{
			ConstraintRoleSequence unaryRoleUniquenessConstraint = unaryRole.SingleRoleAlethicUniquenessConstraint;
			if (unaryRoleUniquenessConstraint == null)
			{
				// The alethic single role uniqueness constraint is missing from the unary role.
				return false;
			}

			// Validate the constraints on the unary role
			foreach (ConstraintRoleSequence constraintRoleSequence in unaryRole.ConstraintRoleSequenceCollection)
			{
				IConstraint constraint = constraintRoleSequence.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
					case ConstraintType.ExternalUniqueness:
					case ConstraintType.Frequency:
						if (constraintRoleSequence != unaryRoleUniquenessConstraint)
						{
							// The unary role has a constraint attached to it that it shouldn't
							return false;
						}
						break;
				}
			}

			// Validate the constraints on the implicit boolean role
			foreach (ConstraintRoleSequence constraintRoleSequence in implicitBooleanRole.ConstraintRoleSequenceCollection)
			{
				IConstraint constraint = constraintRoleSequence.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.DisjunctiveMandatory:
					case ConstraintType.Equality:
					case ConstraintType.Exclusion:
					case ConstraintType.Ring:
					case ConstraintType.SimpleMandatory:
					case ConstraintType.Subset:
						// The implicit boolean role has a constraint attached to it that it shouldn't
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Checks that only one role is played by the value type, that it has a boolean data type, and that
		/// if it has a value constraint, that value constraint is alethic and only allows the value 'true'.
		/// </summary>
		private static bool ValidateImplictBooleanValueType(ObjectType implicitBooleanValueType)
		{
			if (implicitBooleanValueType.PlayedRoleCollection.Count != 1 || !(implicitBooleanValueType.DataType is TrueOrFalseLogicalDataType))
			{
				return false;
			}

			ValueTypeValueConstraint valueConstraint = implicitBooleanValueType.ValueConstraint;
			if (valueConstraint != null)
			{
				// UNDONE: We need to check for alethic here once ValueTypeValueConstraint supports Modality...
				LinkedElementCollection<ValueRange> valueRangeCollection = valueConstraint.ValueRangeCollection;
				ValueRange valueRange;
				if (valueRangeCollection.Count != 1 ||
					!string.Equals((valueRange = valueRangeCollection[0]).MinValue, "TRUE", StringComparison.OrdinalIgnoreCase) ||
					!string.Equals(valueRange.MaxValue, "TRUE", StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}

			return true;
		}





		//private static void ValidateBinarizedUnaryPattern(FactType factType)
		//{
		//    // We should't need to process implied FactTypes, since factTypes with implied 
		//    // unaries shouldn't be objectified.
		//    if (factType == null || factType.IsDeleted || factType.ImpliedByObjectification != null)
		//    {
		//        return;
		//    }
		//    Store store = factType.Store;
		//    LinkedElementCollection<RoleBase> roleCollection =  factType.RoleCollection;
		//    int roleCount = rollCollection.Count;
		//    switch (roleCount)
		//    {
		//        case 1:
		//                BinarizeUnary(factType);
		//                break;
		//        case 2:
		//            //check for a valid pattern for a binarized form of a unary.
		//            Role role = rollCollection[0] as Role;
		//            //if it has a implicit boolean role and is no longer a valid pattern.
		//            if ((role !=null) && role is ImplicitBooleanRole)
		//            {
		//                Role unaryRole = role.OppositeRole;
		//                ObjectType booleanValueType = role.RolePlayer;
		//                if (unaryRole.SingleRoleUniquenessConstraint == null || !(booleanValueType.IsValueType) || !(booleanValueType.DataType is TrueOrFalseLogicalDataType))
		//                {
		//                    Role.ReplaceRole(role, new Role(store, null));
		//                    //Remove the implicitBooleanRole because it is no longer a valid pattern
		//                    role.Delete();
		//                    break;
		//                }
		//                // valid
		//                // break;
		//            }
		//            else
		//            {
		//                // My sibling should be a ImplicitBooleanRole
		//                ImplicitBooleanRole implicitBooleanRole = role.OppositeRole as ImplicitBooleanRole;
		//                if (implicitBooleanRole != null)
		//                {
		//                    ObjectType booleanValueType = implicitBooleanRole.RolePlayer;
		//                    if (role.SingleRoleUniquenessConstraint == null || !(booleanValueType.IsValueType) || !(booleanValueType.DataType is TrueOrFalseLogicalDataType))
		//                    {
		//                        Role.ReplaceRole(role, new Role(store, null));
		//                        //Remove the implicitBooleanRole because it is no longer a valid pattern
		//                        role.Delete();
		//                        break;
		//                    }

		//                }
		//                else {
		//                    break;
		//                }
		//            }
		//        default:
		//            foreach (Role role in rollCollection)
		//            {
		//                if (role is ImplicitBooleanRole)
		//                {
		//                    Role.ReplaceRole(role, new Role(store, null));
		//                    //Remove the implicitBooleanRole because it is no longer a valid pattern
		//                    role.Delete();
		//                }
		//            }
		//    }
		//}

		//private static void MoveValidExternalConstraintsToFarRole(FactType binarizedFact)
		//{
		//    Store store = binarizedFact.Store;


		//}

		//[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		//private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
		//{
		//    public override void ElementAdded(ElementAddedEventArgs e)
		//    {
		//        ConstraintRoleSequenceHasRole constraintRoleSequenceHasRole =  e.ModelElement as ConstraintRoleSequenceHasRole;
		//        if(constraintRoleSequenceHasRole == null || constraintRoleSequenceHasRole.ConstraintRoleSequence.)
		//        ConstraintRoleSequence crs = constraintRoleSequenceHasRole.ConstraintRoleSequence;
		//        switch (crs.Constraint.ConstraintType)
		//        { 
		//            case ConstraintType.Frequency:
		//            case ConstraintType.ExternalUniqueness:
		//                //check the kind of factType (and role that I am on
		//                //If I am the IBR then fine if not then move.
		//                break;
		//        }
		//    }
		//}

		//[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		//private sealed partial class ConstraintRoleSequenceHasRoleRolePlayerChanged : RolePlayerChangeRule
		//{
			
		//}

		//[RuleOn(typeof(ObjectTypePlaysRole))]
		//private sealed partial class ObjectTypePlaysRoleRolePlayerChanged : RolePlayerChangeRule
		//{
		//}

		//[RuleOn(typeof(ObjectTypePlaysRole))]
		//private sealed partial class ObjectTypePlaysRoleDeleted : DeleteRule
		//{
		//    public override void ElementDeleted(ElementDeletedEventArgs e)
		//    {

		//    }
		//}

		//#region Rules for FactTypeHasRole
		//[RuleOn(typeof(FactTypeHasRole))]
		//private sealed partial class FactTypeHasRoleAdded : AddRule
		//{

		//}

		//[RuleOn(typeof(FactTypeHasRole))]
		//private sealed partial class FactTypeHasRoleRolePlayerChanged : RolePlayerChangeRule
		//{
		//    public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
		//    {
		//        Guid roleId = e.DomainRole.Id;
		//        FactTypeHasRole fthr = e.ElementLink as FactTypeHasRole;
		//        if (roleId.Equals(FactTypeHasRole.FactTypeDomainRoleId))
		//        {

		//        }
		//        else if (roleId.Equals(FactTypeHasRole.RoleDomainRoleId))
		//        {

		//        }
		//    }
		//}

		//[RuleOn(typeof(FactTypeHasRole))]
		//private sealed partial class FactTypeHasRoleDeleted : DeleteRule
		//{

		//}
		//#endregion //Rules for FactTypeHasRole

		//#region Rules for ValueTypeHasDataType
		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ValueTypeHasDataType))]
		//private sealed partial class ValueTypeHasDataTypeRolePlayerChanged : RolePlayerChangeRule
		//{
		//    public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
		//    {
		//        Guid changedRole = e.DomainRole.Id;
		//        if (changedRole == ObjectType.DomainClassId)
		//        {

		//        }
		//        else if (changedRole == DataType.DomainClassId)
		//        {

		//        }
		//    }

		//}
		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ValueTypeHasDataType))]
		//private sealed partial class ValueTypeHasDataTypeDeleted : DeleteRule
		//{

		//}
		//#endregion//Rules for ValueTypeHasDataType

		//#region Rules for ValueTypeHasValueConstraint
		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ValueTypeHasValueConstraint))]
		//private sealed partial class ValueTypeHasValueConstraintAdded : AddRule
		//{

		//}

		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ValueTypeHasValueConstraint))]
		//private sealed partial class ValueTypeHasValueConstraintRolePlayerChanged : RolePlayerChangeRule
		//{

		//}

		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ValueTypeHasValueConstraint))]
		//private sealed partial class ValueTypeHasValueConstraintDeleted : DeleteRule
		//{

		//}
		//#endregion//Rules for ValueTypeHasValueConstraint

		//#region Rules for ConstraintRoleSequenceHasRole
		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		//private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
		//{

		//}

		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		//private sealed partial class ConstraintRoleSequenceHasRoleRolePlayerChanged : RolePlayerChangeRule
		//{

		//}

		///// <summary>
		///// 
		///// </summary>
		//[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		//private sealed partial class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
		//{
			

		//}
		//#endregion //Rules for ConstraintRoleSequenceHasRole
	}
}
