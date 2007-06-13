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
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class FactType
	{
		/// <summary>
		/// Get the unary <see cref="Role"/> on a binarized unary <see cref="FactType"/>.
		/// </summary>
		/// <returns>The unary <see cref="Role"/></returns>
		public Role UnaryRole
		{
			get { return UnaryBinarizationUtility.GetUnaryRole(this.RoleCollection); }
		}

		/// <summary>
		/// Get the index of the unary role in this role collection. Returns -1 if none
		/// of the roles are unary.
		/// </summary>
		/// <param name="roles">The <see cref="RoleBase"/> collection to find a unary role for</param>
		/// <returns>null if no unary role is found</returns>
		public static int? GetUnaryRoleIndex(IList<RoleBase> roles)
		{
			return UnaryBinarizationUtility.GetUnaryRoleIndex(roles);
		}

		/// <summary>
		/// Provides support for the binarization of unary <see cref="FactType"/>s.
		/// </summary>
		private static partial class UnaryBinarizationUtility
		{
			private static string GetImplicitBooleanValueTypeName(Role unaryRole)
			{
				//Debug.Assert(!(unaryRole is ImplicitBooleanRole));
				string unaryRoleName = unaryRole.Name;
				ObjectType unaryRolePlayer = unaryRole.RolePlayer;
				FactType unaryFactType = unaryRole.FactType;
				if (unaryRolePlayer != null)
				{
					if (!string.IsNullOrEmpty(unaryRoleName))
					{
						// UNDONE: Localize the space? (Some languages may not use spaces between words.)
						//return unaryRolePlayer.Name + " " + unaryRoleName;
						// UNDONE: As a temporary favor to code generators, don't add spaces in object names.
						// Eventually all generators will handle this cleanly, but they don't yet, so leave out
						// the space to handle the normal case cleanly.
						return unaryRolePlayer.Name + unaryRoleName;
					}
				}
				return unaryFactType.Name;
			}

			public static Role GetUnaryRole(IList<RoleBase> roleCollection)
			{
				int? index = GetUnaryRoleIndex(roleCollection);
				return index.HasValue ? roleCollection[index.Value].Role : null;
			}

			public static int? GetUnaryRoleIndex(IList<RoleBase> roleCollection)
			{
				int roleCount = roleCollection.Count;
				if (roleCount == 2)
				{
					for (int i = 0; i < 2; ++i)
					{
						Role implicitBooleanRole;
						ObjectType rolePlayer;
						if (null != (implicitBooleanRole = (roleCollection[i] as Role)) &&
							null != (rolePlayer = implicitBooleanRole.RolePlayer) &&
							rolePlayer.IsImplicitBooleanValue)
						{
							return (i + 1) % 2;
						}
					}
				}
				return null;
			}

			public static Role GetImplicitBooleanRole(LinkedElementCollection<RoleBase> roleCollection)
			{
				int roleCount = roleCollection.Count;
				if (roleCount == 2)
				{
					// We set up the boolean role as the second role, although there
					// is nothing to stop it from being first. Walk backwards to hit the
					// most likely case.
					for (int i = 1; i >= 0; --i)
					{
						Role implicitBooleanRole;
						ObjectType rolePlayer;
						if (null != (implicitBooleanRole = (roleCollection[i] as Role)) &&
							null != (rolePlayer = implicitBooleanRole.RolePlayer) &&
							rolePlayer.IsImplicitBooleanValue)
						{
							return implicitBooleanRole;
						}
					}
				}
				return null;
			}

			/// <summary>
			/// Binarizes the unary <see cref="FactType"/> specified by <paramref name="unaryFactType"/>, defaulting
			/// to using open-world assumption. The caller is responsible for making sure <paramref name="unaryFactType"/>
			/// is in fact a unary fact type.
			/// </summary>
			public static void BinarizeUnary(FactType unaryFactType, INotifyElementAdded notifyAdded)
			{
				Store store = unaryFactType.Store;
				LinkedElementCollection<RoleBase> roleCollection = unaryFactType.RoleCollection;
				Debug.Assert(roleCollection.Count == 1, "Unaries should only have one role.");

				Role unaryRole = (Role)roleCollection[0];
				string implicitBooleanValueTypeName = GetImplicitBooleanValueTypeName(unaryRole);

				// UNDONE: We are using open-world assumption now
				// Setup the mandatory constraint (for closed-world assumption)
				//MandatoryConstraint mandatoryConstraint = MandatoryConstraint.CreateSimpleMandatoryConstraint(unaryRole);
				//mandatoryConstraint.Model = unaryFactType.Model;
				//if (notifyAdded != null)
				//{
				//    notifyAdded.ElementAdded(mandatoryConstraint, true);
				//}

				// Setup the uniqueness constraint (to make the newly binarized FactType valid)
				if (unaryRole.SingleRoleAlethicUniquenessConstraint == null)
				{
					UniquenessConstraint uniquenessConstraint = UniquenessConstraint.CreateInternalUniquenessConstraint(unaryFactType);
					uniquenessConstraint.RoleCollection.Add(unaryRole);
					uniquenessConstraint.Model = unaryFactType.Model;
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(uniquenessConstraint, true);
					}
				}

				// Setup the boolean role (to make the FactType a binary)
				Role implicitBooleanRole = new Role(store, null);
				implicitBooleanRole.Name = unaryRole.Name;

				// Setup the boolean value type (because the boolean role needs a role player)
				ObjectType implicitBooleanValueType = new ObjectType(store, new PropertyAssignment(ObjectType.IsImplicitBooleanValueDomainPropertyId, true));
				Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				try
				{
					contextInfo[ORMModel.AllowDuplicateNamesKey] = null;
					implicitBooleanValueType.Name = implicitBooleanValueTypeName;
					implicitBooleanValueType.Model = unaryFactType.Model;
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(implicitBooleanValueType, true);
					}
				}
				finally
				{
					contextInfo.Remove(ORMModel.AllowDuplicateNamesKey);
				}
				implicitBooleanValueType.DataType = store.ElementDirectory.FindElements<TrueOrFalseLogicalDataType>(false)[0];

				// Set value constraint on implicit boolean ValueType for open-world assumption
				ValueTypeValueConstraint implicitBooleanValueConstraint = implicitBooleanValueType.ValueConstraint
					= new ValueTypeValueConstraint(implicitBooleanValueType.Store, null);

				// Add the true-only ValueRange to the value constraint for open-world assumption
				implicitBooleanValueConstraint.ValueRangeCollection.Add(new ValueRange(store,
					new PropertyAssignment(ValueRange.MinValueDomainPropertyId, bool.TrueString),
					new PropertyAssignment(ValueRange.MaxValueDomainPropertyId, bool.TrueString)));

				// Make the boolean value type the role player for the implicit boolean role
				implicitBooleanRole.RolePlayer = implicitBooleanValueType;

				LinkedElementCollection<ReadingOrder> readings = unaryFactType.ReadingOrderCollection;
				int readingCount = readings.Count;

				// Add the boolean role to the FactType
				roleCollection.Add(implicitBooleanRole);
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(implicitBooleanRole, true);
				}
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
					Role implicitBooleanRole = binarizedUnaryFactRoleCollection[i].Role;
					if (implicitBooleanRole != null)
					{
						ObjectType implicitBooleanValueType = implicitBooleanRole.RolePlayer;
						if (implicitBooleanValueType != null && implicitBooleanValueType.IsImplicitBooleanValue)
						{
							foundImplicitBoolean = true;

							// Delete the implicit boolean value type (which will also remove any value constraints on it)
							implicitBooleanValueType.IsImplicitBooleanValue = false;
							implicitBooleanValueType.Delete();
						}
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
							}

							// UNDONE: We are using open-world assumption now
							//MandatoryConstraint simpleMandatoryConstraint = role.SimpleMandatoryConstraint;
							//if (simpleMandatoryConstraint != null && simpleMandatoryConstraint.Modality == ConstraintModality.Alethic)
							//{
							//    // Delete the simple mandatory constraint (for closed-world assumption), if present
							//    simpleMandatoryConstraint.Delete();
							//}
						}
					}
				}
			}

			public static void ProcessFactType(FactType factType)
			{
				LinkedElementCollection<RoleBase> roleCollection = factType.RoleCollection;
				int roleCollectionCount = roleCollection.Count;

				if (roleCollectionCount == 1)
				{
					// If we have a unary, binarize it
					BinarizeUnary(factType, null);
					return;
				}
				else if (roleCollectionCount == 2)
				{
					// If we have a binary that has an implicit boolean role in it, make sure it matches the pattern
					Role implicitBooleanRole = GetImplicitBooleanRole(roleCollection);
					if (implicitBooleanRole != null)
					{
						Role unaryRole = implicitBooleanRole.OppositeRole.Role;
						Debug.Assert(unaryRole != null);
						string implicitBooleanValueTypeName = GetImplicitBooleanValueTypeName(unaryRole);
						if (implicitBooleanRole.RolePlayer.Name != implicitBooleanValueTypeName)
						{
							Dictionary<object, object> contextInfo = factType.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
							try
							{
								contextInfo[ORMModel.AllowDuplicateNamesKey] = null;
								implicitBooleanRole.RolePlayer.Name = implicitBooleanValueTypeName;
							}
							finally
							{
								contextInfo.Remove(ORMModel.AllowDuplicateNamesKey);
							}
						}
						if (!ValidateConstraints(unaryRole, implicitBooleanRole) || !ValidateImplictBooleanValueType(implicitBooleanRole.RolePlayer))
						{
							LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
							DebinarizeUnary(roles);
							// Append to the reading orders
							LinkedElementCollection<ReadingOrder> readingOrders = factType.ReadingOrderCollection;
							int readingOrderCount = readingOrders.Count;
							for (int i = 0; i < readingOrderCount; ++i)
							{
								if (!readingOrders[i].RoleCollection.Contains(implicitBooleanRole))
								{
									readingOrders[i].RoleCollection.Add(implicitBooleanRole);
								}

								// UNDONE: iterate readings and set text
								LinkedElementCollection<Reading> readings = readingOrders[i].ReadingCollection;
								int readingCount = readings.Count;
								for (int j = 0; j < readingCount; ++j)
								{
									readings[j].SetAutoText(readings[j].Text + "  {1}");
								}
							}
						}
					}
				}
				else
				{
					// If we have an n-ary, remove any implicit boolean roles in it
					for (int i = roleCollectionCount - 1; i >= 0; --i)
					{
						Role implicitBooleanRole = roleCollection[i].Role;
						if (implicitBooleanRole != null && implicitBooleanRole.RolePlayer != null && implicitBooleanRole.RolePlayer.IsImplicitBooleanValue)
						{
							DebinarizeUnary(factType.RoleCollection);
							// Delete our implicit boolean role
							implicitBooleanRole.Delete();
							break;
						}
					}
				}
			}

			/// <summary>
			/// Checks the <see cref="IConstraint"/>s on the binarized unary <see cref="FactType"/> as specified by
			/// the <see cref="FactType"/>'s <paramref name="unaryRole"/> and the <paramref name="unaryRole"/>.
			/// </summary>
			private static bool ValidateConstraints(Role unaryRole, Role implicitBooleanRole)
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
						case ConstraintType.InternalUniqueness:
						case ConstraintType.Frequency:
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
				if (!implicitBooleanValueType.IsValueType || !implicitBooleanValueType.IsImplicitBooleanValue || implicitBooleanValueType.IsIndependent ||
					implicitBooleanValueType.PlayedRoleCollection.Count != 1 || !(implicitBooleanValueType.DataType is TrueOrFalseLogicalDataType))
				{
					return false;
				}

				ValueTypeValueConstraint valueConstraint = implicitBooleanValueType.ValueConstraint;
				if (valueConstraint != null)
				{
					// UNDONE: We need to check for alethic here once ValueTypeValueConstraint supports Modality...
					LinkedElementCollection<ValueRange> valueRangeCollection = valueConstraint.ValueRangeCollection;
					ValueRange valueRange;
					bool value;
					if (valueRangeCollection.Count != 1 ||
						!bool.TryParse((valueRange = valueRangeCollection[0]).MinValue, out value) || !value ||
						!bool.TryParse(valueRange.MaxValue, out value) || !value)
					{
						return false;
					}
				}
				else
				{
					// UNDONE: We are only allowing open-world assumption for now, so the value constraint is required.
					return false;
				}

				return true;
			}

			[RuleOn(typeof(FactType))] // ChangeRule
			private sealed partial class FactTypeNameChanged : ChangeRule
			{
				public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
				{
					if (e.DomainProperty.Id == FactType.GeneratedNameDomainPropertyId)
					{
						ORMCoreDomainModel.DelayValidateElement((e.ModelElement as FactType), DelayValidateUnaryBinarization);
					}
				}
			}

			[RuleOn(typeof(ObjectTypePlaysRole))] // AddRule
			private sealed partial class ObjectTypePlaysRoleAdded : AddRule
			{
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ObjectTypePlaysRole o = e.ModelElement as ObjectTypePlaysRole;
					if (o.PlayedRole.FactType != null)
					{
						ORMCoreDomainModel.DelayValidateElement((e.ModelElement as ObjectTypePlaysRole).PlayedRole.FactType, DelayValidateUnaryBinarization);
					}
				}
			}

			[RuleOn(typeof(ObjectTypePlaysRole))] // RolePlayerChangeRule
			private sealed partial class ObjectTypePlaysRoleRolePlayerChanged : RolePlayerChangeRule
			{
				public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
				{
					if ((e.OldRolePlayer as ObjectType).IsImplicitBooleanValue && !(e.NewRolePlayer as ObjectType).IsImplicitBooleanValue)
					{
						Debug.Fail("Cannot change the role player on the implicit boolean role on a binarized unary.");
						(e.ElementLink as ObjectTypePlaysRole).PlayedRole.RolePlayer = (ObjectType)e.OldRolePlayer;
					}
					ORMCoreDomainModel.DelayValidateElement((e.ElementLink as ObjectTypePlaysRole).PlayedRole.FactType, DelayValidateUnaryBinarization);
				}
			}

			[RuleOn(typeof(ObjectTypePlaysRole))] // DeleteRule
			private sealed partial class ObjectTypePlaysRoleDeleted : DeleteRule
			{
				public override void ElementDeleted(ElementDeletedEventArgs e)
				{
					// After this point, it is unknown whether the FactType was a unary, so we just delete it for now
					if ((e.ModelElement as ObjectTypePlaysRole).RolePlayer.IsImplicitBooleanValue)
					{
						(e.ModelElement as ObjectTypePlaysRole).PlayedRole.FactType.Delete();
					}
					//ORMCoreDomainModel.DelayValidateElement((e.ModelElement as ObjectTypePlaysRole).PlayedRole.FactType, DelayValidateUnaryBinarization);
				}
			}

			#region Rules for FactTypeHasRole
			[RuleOn(typeof(FactTypeHasRole))] // AddRule
			private sealed partial class FactTypeHasRoleAdded : AddRule
			{
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ORMCoreDomainModel.DelayValidateElement((e.ModelElement as FactTypeHasRole).FactType, DelayValidateUnaryBinarization);
				}
			}
			[ORMCoreDomainModel.DelayValidatePriority(-100)]
			private static void DelayValidateUnaryBinarization(ModelElement element)
			{
				FactType factType = (FactType)element;
				if (!factType.IsDeleted)
				{
					ProcessFactType(factType);
				}
			}

			[RuleOn(typeof(FactTypeHasRole))] // RolePlayerChangeRule
			private sealed partial class FactTypeHasRoleRolePlayerChanged : RolePlayerChangeRule
			{
				public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
				{
					ORMCoreDomainModel.DelayValidateElement(((FactTypeHasRole)e.ElementLink).FactType, DelayValidateUnaryBinarization);
				}
			}

			[RuleOn(typeof(FactTypeHasRole))] // DeletingRule
			private sealed partial class FactTypeHasRoleDeleting : DeletingRule
			{
				public override void ElementDeleting(ElementDeletingEventArgs e)
				{
					FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
					if (!factType.IsDeleted)
					{
						if (factType.UnaryRole != null)
						{
							// Delete the Implicit Boolean ValueType
							if (factType.RoleCollection[0].Role.RolePlayer != null && factType.RoleCollection[0].Role.RolePlayer.IsImplicitBooleanValue)
							{
								factType.RoleCollection[0].Role.RolePlayer.Delete();
							}
							else
							{
								factType.RoleCollection[1].Role.RolePlayer.Delete();
							}
							// Delete the Unary FactType
							(e.ModelElement as FactTypeHasRole).FactType.Delete();
						}
					}
					//ORMCoreDomainModel.DelayValidateElement(((FactTypeHasRole)e.ModelElement).FactType, DelayValidateUnaryBinarization);
				}
			}
			#endregion //Rules for FactTypeHasRole

			#region Rules for ConstraintRoleSequenceHasRole
			/// <summary>
			/// 
			/// </summary>
			[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
			private sealed partial class ConstraintRoleSequenceHasRoleAdded : AddRule
			{
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ORMCoreDomainModel.DelayValidateElement((e.ModelElement as ConstraintRoleSequenceHasRole).Role.FactType, DelayValidateUnaryBinarization);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // RolePlayerChangeRule
			private sealed partial class ConstraintRoleSequenceHasRoleRolePlayerChanged : RolePlayerChangeRule
			{
				public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
				{
					if ((e.ElementLink as ConstraintRoleSequenceHasRole).Role.FactType != null)
					{
						ORMCoreDomainModel.DelayValidateElement((e.ElementLink as ConstraintRoleSequenceHasRole).Role.FactType, DelayValidateUnaryBinarization);
					}
				}
			}

			/// <summary>
			/// 
			/// </summary>
			[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeleteRule
			private sealed partial class ConstraintRoleSequenceHasRoleDeleted : DeleteRule
			{
				public override void ElementDeleted(ElementDeletedEventArgs e)
				{
					if ((e.ModelElement as ConstraintRoleSequenceHasRole).Role.FactType != null)
					{
						ORMCoreDomainModel.DelayValidateElement((e.ModelElement as ConstraintRoleSequenceHasRole).Role.FactType, DelayValidateUnaryBinarization);
					}
				}
			}
			#endregion Rules for ConstraintRoleSequenceHasRole
		}
	}
}