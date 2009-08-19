#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region RolePath class
	partial class RolePath
	{
		#region Abstract Properties
		/// <summary>
		/// Return the root <see cref="PrimaryRolePath"/> associated
		/// with this path.
		/// </summary>
		public abstract PrimaryRolePath RootRolePath { get;}
		#endregion // Abstract Properties
		#region Accessors Properties
		/// <summary>
		/// Return the <see cref="PathedRole"/> relationships attached
		/// to this <see cref="RolePath"/>
		/// </summary>
		public ReadOnlyCollection<PathedRole> PathedRoleCollection
		{
			get
			{
				return PathedRole.GetLinksToRoleCollection(this);
			}
		}
		/// <summary>
		/// Get the <see cref="ObjectType"/> that provides the starting
		/// point for this <see cref="RolePath"/> but is not the root
		/// object type. Use <see cref="ContextOrRootObjectType"/> to get
		/// a fully resolved starting object type.
		/// </summary>
		public ObjectType ContextObjectType
		{
			get
			{
				PathedRole dummy;
				return GetContextObjectType(false, out dummy);
			}
		}
		/// <summary>
		/// Get the <see cref="ObjectType"/> that provides the starting
		/// point for this <see cref="RolePath"/>. The starting object type
		/// will be the role player of last role in a containing path,
		/// of the <see cref="PrimaryRolePath.RootObjectType"/> if there
		/// is no containing path. Use <see cref="ContextObjectType"/> to
		/// ignore the root object type.
		/// </summary>
		public ObjectType ContextOrRootObjectType
		{
			get
			{
				PathedRole dummy;
				return GetContextObjectType(true, out dummy);
			}
		}
		/// <summary>
		/// Get the <see cref="PathedRole"/> that occurs immediately before
		/// this path.
		/// </summary>
		public PathedRole ContextPathedRole
		{
			get
			{
				PathedRole retVal;
				GetContextObjectType(false, out retVal);
				return retVal;
			}
		}
		private ObjectType GetContextObjectType(bool returnRootObjectType, out PathedRole contextPathedRole)
		{
			RolePath currentPath = this;
			while (currentPath != null)
			{
				RoleSubPath subPath;
				if (null != (subPath = currentPath as RoleSubPath))
				{
					RolePath parentPath = subPath.ParentRolePath;
					ReadOnlyCollection<PathedRole> pathedRoles = parentPath.PathedRoleCollection;
					int pathRoleCount = pathedRoles.Count;
					if (pathRoleCount == 0)
					{
						currentPath = parentPath;
					}
					else
					{
						return (contextPathedRole = pathedRoles[pathRoleCount - 1]).Role.RolePlayer;
					}
				}
				else
				{
					contextPathedRole = null;
					return returnRootObjectType ? ((PrimaryRolePath)currentPath).RootObjectType : null;
				}
			}
			contextPathedRole = null;
			return null;
		}
		#endregion // Accessors Properties
	}
	#endregion // RolePath class
	#region PrimaryRolePath class
	partial class PrimaryRolePath
	{
		/// <summary>
		/// This path is the root of the path
		/// </summary>
		public override PrimaryRolePath RootRolePath
		{
			get
			{
				return this;
			}
		}
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path.
		/// </summary>
		public abstract ORMModel Model { get;}
	}
	#endregion // PrimaryRolePath class
	#region FactTypeDerivationRule class
	partial class FactTypeDerivationRule
	{
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="FactType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				FactType factType = FactType;
				return factType != null ? factType.Model : null;
			}
		}
	}
	#endregion // FactTypeDerivationRule class
	#region SubtypeDerivationRule class
	partial class SubtypeDerivationRule
	{
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="ObjectType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ObjectType objectType = Subtype;
				return objectType != null ? objectType.Model : null;
			}
		}
	}
	#endregion // SubtypeDerivationRule class
	#region RoleSubPath class
	partial class RoleSubPath
	{
		/// <summary>
		/// Recursive find the path root
		/// </summary>
		public override PrimaryRolePath RootRolePath
		{
			get
			{
				RoleSubPath subPath = this;
				RolePath parentPath = null;
				while (subPath != null)
				{
					parentPath = subPath.ParentRolePath;
					subPath = parentPath as RoleSubPath;
				}
				return parentPath as PrimaryRolePath;
			}
		}
	}
	#endregion // RoleSubPath class
	#region PathedRole class
	partial class PathedRole
	{
		#region Accessor Properties
		/// <summary>
		/// Get the previous pathed role in either this or the
		/// containing role path.
		/// </summary>
		public PathedRole PreviousPathedRole
		{
			get
			{
				RolePath rolePath = RolePath;
				ReadOnlyCollection<PathedRole> steps = rolePath.PathedRoleCollection;
				int index = steps.IndexOf(this);
				switch (index)
				{
					case -1:
						break;
					case 0:
						RoleSubPath subPath;
						while (null != (subPath = rolePath as RoleSubPath) &&
							null != (rolePath = subPath.ParentRolePath))
						{
							steps = rolePath.PathedRoleCollection;
							int stepCount = steps.Count;
							if (stepCount != 0)
							{
								return steps[stepCount - 1];
							}
						}
						break;
					default:
						return steps[index - 1];
				}
				return null;
			}
		}
		#endregion // Accessor Properties
	}
	#endregion // PathedRole class
	#region Function class
	partial class Function : IModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Use the function name for string display
		/// </summary>
		public override string ToString()
		{
			string symbol = OperatorSymbol;
			return string.IsNullOrEmpty(symbol) ? Name : symbol;
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				FunctionDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
	}
	#endregion // Function class
	#region CalculatedPathValue class
	partial class CalculatedPathValue
	{
		#region Rule Methods
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// When an assigned function changes, map old inputs to new inputs based on
		/// parameter position. This preserves as much data as possible for the most
		/// common cases.
		/// </summary>
		private static void FunctionChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				CalculatedPathValueIsCalculatedWithFunction link = (CalculatedPathValueIsCalculatedWithFunction)e.ElementLink;
				CalculatedPathValue calculatedValue = link.CalculatedValue;
				LinkedElementCollection<FunctionParameter> oldParameters = ((Function)e.OldRolePlayer).ParameterCollection;
				Function newFunction = link.Function;
				LinkedElementCollection<FunctionParameter> newParameters = newFunction.ParameterCollection;
				int newParameterCount = newParameters.Count;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculatedValue.InputCollection;
				for (int i = inputs.Count - 1; i >= 0; --i)
				{
					CalculatedPathValueInput input = inputs[i];
					int oldParameterIndex = oldParameters.IndexOf(input.Parameter);
					FunctionParameter newParameter;
					if (oldParameterIndex != -1 &&
						oldParameterIndex < newParameterCount &&
						(!(newParameter = newParameters[oldParameterIndex]).BagInput ||
						input.SourceConstant == null))
					{
						input.Parameter = newParameters[oldParameterIndex];
					}
					else
					{
						input.Delete();
					}
				}
				PrimaryRolePathSatisfiesCalculatedCondition conditionLink;
				if (!newFunction.IsBoolean &&
					null != (conditionLink = PrimaryRolePathSatisfiesCalculatedCondition.GetLinkToPrimaryRolePath(calculatedValue)))
				{
					// A non-boolean function cannot be path condition
					conditionLink.Delete();
				}
			}
			else
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEnforceInitialCalculatedValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void FunctionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculatedValue = ((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue;
			if (!calculatedValue.IsDeleted)
			{
				calculatedValue.InputCollection.Clear();

				// If there is no function, then we cannot assume that it is a boolean function
				// that is eligible to satisfy a condition
				PrimaryRolePathSatisfiesCalculatedCondition conditionLink = PrimaryRolePathSatisfiesCalculatedCondition.GetLinkToPrimaryRolePath(calculatedValue);
				if (conditionLink != null)
				{
					conditionLink.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToCalculatedValueRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement).Input;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourcePathedRole = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToConstantRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input;
			FunctionParameter parameter;
			if (null != (parameter = calculatedInput.Parameter) &&
				parameter.BagInput)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionPathConstantInconsistentWithBagInput);
			}
			calculatedInput.SourcePathedRole = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// Make the three source types (role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToPathedRoleRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		#endregion // Rule Methods
	}
	#endregion // CalculatedPathValue class
	#region FactTypeDerivationExpression class (transitional)
	// Transitional code to synchronize derivation storage settings on
	// the old (expression) and new (path) derivation mechanisms.
	partial class FactTypeDerivationExpression
	{
		#region Helper Methods
		private static void SynchronizeExpression(FactTypeDerivationExpression expression, DerivationCompleteness completeness, DerivationStorage storage)
		{
			switch (completeness)
			{
				case DerivationCompleteness.FullyDerived:
					expression.DerivationStorage = (storage == ObjectModel.DerivationStorage.Stored) ? DerivationExpressionStorageType.DerivedAndStored : DerivationExpressionStorageType.Derived;
					break;
				case DerivationCompleteness.PartiallyDerived:
					expression.DerivationStorage = (storage == ObjectModel.DerivationStorage.Stored) ? DerivationExpressionStorageType.PartiallyDerivedAndStored : DerivationExpressionStorageType.PartiallyDerived;
					break;
			}
		}
		private static void SynchronizeRule(FactTypeDerivationRule rule, DerivationExpressionStorageType storageType)
		{
			switch (storageType)
			{
				case DerivationExpressionStorageType.Derived:
					rule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
					break;
				case DerivationExpressionStorageType.DerivedAndStored:
					rule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
					break;
				case DerivationExpressionStorageType.PartiallyDerived:
					rule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
					break;
				case DerivationExpressionStorageType.PartiallyDerivedAndStored:
					rule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
					rule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
					break;
			}
		}
		#endregion // Helper Methods
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DerivationRuleFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class DerivationRuleFixupListener : DeserializationFixupListener<FactTypeHasDerivationExpression>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public DerivationRuleFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Process FactTypeHasDerivationExpression elements
			/// </summary>
			/// <param name="element">An FactTypeHasDerivationExpression element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactTypeHasDerivationExpression element, Store store, INotifyElementAdded notifyAdded)
			{
				FactTypeDerivationRule rule;
				if (!element.IsDeleted &&
					null != (rule = element.FactType.DerivationRule))
				{
					// Expressions were around before rules, synchronize rules with
					// the expression storage.
					SynchronizeRule(rule, element.DerivationRule.DerivationStorage);
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(FactTypeHasDerivationExpression)
		/// </summary>
		private static void DerivationExpressionAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasDerivationExpression link = (FactTypeHasDerivationExpression)e.ModelElement;
			FactTypeDerivationRule rule;
			if (null != (rule = link.FactType.DerivationRule))
			{
				SynchronizeExpression(link.DerivationRule, rule.DerivationCompleteness, rule.DerivationStorage);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationExpression)
		/// </summary>
		private static void DerivationExpressionChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactTypeDerivationExpression expression = (FactTypeDerivationExpression)e.ModelElement;
			FactType factType;
			FactTypeDerivationRule rule;
			if (null != (factType = expression.FactType) &&
				null != (rule = factType.DerivationRule))
			{
				SynchronizeRule(rule, expression.DerivationStorage);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasDerivationRule)
		/// </summary>
		private static void DerivationRuleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasDerivationRule link = (FactTypeHasDerivationRule)e.ModelElement;
			FactTypeDerivationExpression expression;
			if (null != (expression = link.FactType.DerivationExpression))
			{
				SynchronizeRule(link.DerivationRule, expression.DerivationStorage);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationRule)
		/// </summary>
		private static void DerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactTypeDerivationRule rule = (FactTypeDerivationRule)e.ModelElement;
			FactType factType;
			FactTypeDerivationExpression expression;
			if (null != (factType = rule.FactType) &&
				null != (expression = factType.DerivationExpression))
			{
				SynchronizeExpression(expression, rule.DerivationCompleteness, rule.DerivationStorage);
			}
		}
		#endregion // Rule Methods
	}
	#endregion // FactTypeDerivationExpression class
}
