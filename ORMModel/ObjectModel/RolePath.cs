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
		/// Return the root <see cref="LeadRolePath"/> associated
		/// with this path.
		/// </summary>
		public abstract LeadRolePath RootRolePath { get;}
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
		/// of the <see cref="LeadRolePath.RootObjectType"/> if there
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
					return returnRootObjectType ? ((LeadRolePath)currentPath).RootObjectType : null;
				}
			}
			contextPathedRole = null;
			return null;
		}
		#endregion // Accessors Properties
		#region Delayed Validation
		/// <summary>
		/// Called when a subbranch is removed. If there is one remaining subbranch,
		/// then move the remaining elements in a single branch to the end of the current branch.
		/// </summary>
		/// <param name="element">A <see cref="RoleSubPath"/></param>
		private static void DelayValidatePathCollapse(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			RolePath parentPath = (RolePath)element;
			LinkedElementCollection<RoleSubPath> subPaths = parentPath.SplitPathCollection;
			if (subPaths.Count == 1)
			{
				// Remove the tail split by moving all elements up one level
				RolePath collapsePath = subPaths[0];

				// Move pathed roles
				foreach (PathedRole pathedRole in collapsePath.PathedRoleCollection)
				{
					pathedRole.RolePath = parentPath;
				}

				// Move sub paths
				foreach (RoleSubPathIsContinuationOfRolePath subPathLink in RoleSubPathIsContinuationOfRolePath.GetLinksToSplitPathCollection(collapsePath))
				{
					subPathLink.ParentRolePath = parentPath;
				}

				// Change the parent split settings to the path we just collapsed
				parentPath.SplitIsNegated = collapsePath.SplitIsNegated;
				parentPath.SplitCombinationOperator = collapsePath.SplitCombinationOperator;

				// We're done with it. Note that this can trigger other rules which will
				// reenter this routine for the grandparent branch
				collapsePath.Delete();
			}
		}
		#endregion // Delayed Validation
		#region Rule Methods
		/// <summary>
		/// DeleteRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRole)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted &&
				rolePath.PathedRoleCollection.Count == 0 &&
				rolePath.SplitPathCollection.Count == 0)
			{
				rolePath.Delete();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == PathedRole.RolePathDomainRoleId)
			{
				RolePath rolePath = (RolePath)e.OldRolePlayer;
				if (!rolePath.IsDeleted &&
					rolePath.PathedRoleCollection.Count == 0 &&
					rolePath.SplitPathCollection.Count == 0)
				{
					rolePath.Delete();
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// If a subbranch is deleted, then delay validate if the parent branch should
		/// attempt to collapse a remaining branch.
		/// </summary>
		private static void SubPathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath parentRolePath = ((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath;
			if (!parentRolePath.IsDeleted)
			{
				if (parentRolePath.PathedRoleCollection.Count == 0 &&
					parentRolePath.SplitPathCollection.Count == 0)
				{
					parentRolePath.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Check branch collapsing for role player changes.
		/// </summary>
		private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId)
			{
				// The parent has lost a branch, validate if needed
				RolePath parentRolePath = (RolePath)e.OldRolePlayer;
				if (!parentRolePath.IsDeleted)
				{
					if (parentRolePath.PathedRoleCollection.Count == 0 &&
						parentRolePath.SplitPathCollection.Count == 0)
					{
						parentRolePath.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
					}
				}
			}
		}
		#endregion // Rule Methods
	}
	#endregion // RolePath class
	#region LeadRolePath class
	partial class LeadRolePath
	{
		#region Base overrides
		/// <summary>
		/// This path is the root of the path
		/// </summary>
		public override LeadRolePath RootRolePath
		{
			get
			{
				return this;
			}
		}
		#endregion // Base overrides
	}
	#endregion // LeadRolePath class
	#region RolePathComponent class
	partial class RolePathComponent
	{
		#region Accessor Properties
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path component.
		/// </summary>
		public ORMModel Model
		{
			get
			{
				RolePathOwner owner = RootOwner;
				return (owner != null) ? owner.Model : null;
			}
		}
		/// <summary>
		/// Get the resolved <see cref="RolePathOwner"/> for this <see cref="RolePathComponent"/>
		/// </summary>
		public RolePathOwner RootOwner
		{
			get
			{
				RolePathComponent component = this;
				RolePathOwner retVal = null;
				while (component != null)
				{
					if (null != (retVal = component.ParentOwner))
					{
						break;
					}
					component = component.ParentCompositor;
				}
				return retVal;
			}
		}
		#endregion // Accessor Properties
		#region Rule Methods
		/// <summary>
		/// Delay validator to remove detached path components from the model. Since
		/// top-level path components (LeadRolePath, RolePathCompositor) may naturally
		/// change ownership during model editing, we do not specify PropagateDelete
		/// in the model, so we need to explicitly delete a component if it has not
		/// been reattached.
		/// </summary>
		private static void DelayValidateDetachedComponent(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			RolePathComponent pathComponent = (RolePathComponent)element;
			if (pathComponent.ParentCompositor == null && pathComponent.ParentOwner == null)
			{
				pathComponent.Delete();
			}
		}
		/// <summary>
		/// Automatically collapse a compositor that no longer contains at
		/// least two components by moving the remaining component into the
		/// compositors container (either the path owner or another compositor).
		/// </summary>
		private static void DelayValidateCompositorCollapse(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			RolePathCompositor compositor = (RolePathCompositor)element;
			LinkedElementCollection<RolePathComponent> components = compositor.PathComponentCollection;
			switch (components.Count)
			{
				case 0:
					compositor.Delete();
					break;
				case 1:
					RolePathOwner parentOwner;
					RolePathCompositor parentCompositor;
					if (null != (parentOwner = compositor.ParentOwner))
					{
						components[0].ParentOwner = parentOwner;
					}
					else if (null != (parentCompositor = compositor.ParentCompositor))
					{
						components[0].ParentCompositor = parentCompositor;
					}
					// Note that this will happen delayed through this validation rule,
					// but it doesn't hurt to skip the short circuiting and do it now.
					compositor.Delete();
					break;
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathCompositorHasPathComponent)
		/// A <see cref="RolePathComponent"/> has two possible aggregating relationships,
		/// make sure that only one exists at a given time.
		/// </summary>
		private static void RolePathCompositorHasPathComponentAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasPathComponent ownerLink = RolePathOwnerHasPathComponent.GetLinkToParentOwner(((RolePathCompositorHasPathComponent)e.ModelElement).PathComponent);
			if (ownerLink != null)
			{
				ownerLink.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasPathComponent)
		/// A <see cref="RolePathComponent"/> has two possible aggregating relationships,
		/// make sure that only one exists at a given time.
		/// </summary>
		private static void RolePathOwnerHasPathComponentAddedRule(ElementAddedEventArgs e)
		{
			// Make sure a top-level component is not marked as a complement
			// UNDONE: Add exception to block complementing of a top-level component.
			RolePathComponent component = ((RolePathOwnerHasPathComponent)e.ModelElement).PathComponent;
			component.IsComplemented = false;
			RolePathCompositorHasPathComponent compositorLink = RolePathCompositorHasPathComponent.GetLinkToParentCompositor(component);
			if (compositorLink != null)
			{
				compositorLink.Delete();
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathCompositorHasPathComponent)
		/// </summary>
		private static void RolePathCompositorHasPathComponentDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathCompositorHasPathComponent link = (RolePathCompositorHasPathComponent)e.ModelElement;
			RolePathComponent component = link.PathComponent;
			if (!component.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(component, DelayValidateDetachedComponent);
			}
			RolePathCompositor compositor = link.Compositor;
			if (!compositor.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(compositor, DelayValidateCompositorCollapse);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathCompositorHasPathComponent)
		/// </summary>
		private static void RolePathCompositorHasPathComponentRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathCompositorHasPathComponent.PathComponentDomainRoleId)
			{
				RolePathOwnerHasPathComponent ownerLink = RolePathOwnerHasPathComponent.GetLinkToParentOwner((RolePathComponent)e.NewRolePlayer);
				if (ownerLink != null)
				{
					ownerLink.Delete();
				}
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateDetachedComponent);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateCompositorCollapse);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasPathComponent)
		/// </summary>
		private static void RolePathOwnerHasPathComponentDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathComponent component = ((RolePathOwnerHasPathComponent)e.ModelElement).PathComponent;
			if (!component.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(component, DelayValidateDetachedComponent);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasPathComponent)
		/// </summary>
		private static void RolePathOwnerHasPathComponentRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathOwnerHasPathComponent.PathComponentDomainRoleId)
			{
				// Make sure a top-level component is not marked as a complement
				RolePathComponent component = (RolePathComponent)e.NewRolePlayer;
				component.IsComplemented = false;
				RolePathCompositorHasPathComponent compositorLink = RolePathCompositorHasPathComponent.GetLinkToParentCompositor(component);
				if (compositorLink != null)
				{
					compositorLink.Delete();
				}
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateDetachedComponent);
			}
		}
		#endregion // Rule Methods
	}
	#endregion // RolePathComponent class
	#region RolePathOwner class
	partial class RolePathOwner
	{
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path owner.
		/// </summary>
		public abstract ORMModel Model { get;}
	}
	#endregion // RolePathOwner class
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
		public override LeadRolePath RootRolePath
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
				return parentPath as LeadRolePath;
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
				if (!newFunction.IsBoolean)
				{
					foreach (LeadRolePathSatisfiesCalculatedCondition conditionLink in LeadRolePathSatisfiesCalculatedCondition.GetLinksToRequiredForPathCollection(calculatedValue))
					{
						// A non-boolean function cannot be path condition
						conditionLink.Delete();
					}
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
				foreach (LeadRolePathSatisfiesCalculatedCondition conditionLink in LeadRolePathSatisfiesCalculatedCondition.GetLinksToRequiredForPathCollection(calculatedValue))
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
