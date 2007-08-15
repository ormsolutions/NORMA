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
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Diagnostics;
using Neumont.Tools.Modeling.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Reflection;

namespace Neumont.Tools.Modeling
{
	#region ElementValidation delegate
	/// <summary>
	/// A delegate callback to use with the <see cref="FrameworkDomainModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The <see cref="ModelElement"/> to validate</param>
	public delegate void ElementValidation(ModelElement element);
	#endregion // ElementValidation delegate
	#region DelayValidatePriorityAttribute class
	/// <summary>
	/// Place on a static delay validate method to explicitly control the execution order
	/// of the validate callbacks. All delay validate methods from a given domain model will
	/// be run before validators from dependent domain models are called. This attribute
	/// allows tighter control within a domain, or to make a delay validate function run with
	/// validators for earlier DomainModel.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class DelayValidatePriorityAttribute : Attribute
	{
		private int myPriority;
		private Type myDomainModelType;
		/// <summary>
		/// Create a new <see cref="DelayValidatePriorityAttribute"/>
		/// </summary>
		/// <param name="priority">The relative priority to run the code. 0 is the default priority. Negative
		/// values will run before the default, positive values after.</param>
		[DebuggerStepThrough]
		public DelayValidatePriorityAttribute(int priority)
		{
			myPriority = priority;
		}
		/// <summary>The relative priority to run the code. 0 is the default priority. Negative
		/// values will run before the default, positive values after.</summary>
		public int Priority
		{
			[DebuggerStepThrough]
			get
			{
				return myPriority;
			}
		}
		/// <summary>
		/// The type of the domain model to run with. If this is null then the
		/// domain model is automatically configured based on the containing classes of the method.
		/// </summary>
		public Type DomainModelType
		{
			[DebuggerStepThrough]
			get
			{
				return myDomainModelType;
			}
			[DebuggerStepThrough]
			set
			{
				if (typeof(DomainModel).IsAssignableFrom(value))
				{
					myDomainModelType = value;
				}
			}
		}
	}
	#endregion // DelayValidatePriorityAttribute class
	partial class FrameworkDomainModel
	{
		#region InitializingToolboxItems property
		private static bool myInitializingToolboxItems;
		/// <summary>
		/// Static property to disable rule reflection for fast
		/// model load. This needs to be on a meta model, not the
		/// package, so that the models also load from the command line.
		/// </summary>
		public static bool InitializingToolboxItems
		{
			get
			{
				return myInitializingToolboxItems;
			}
			set
			{
				Debug.Assert(value || myInitializingToolboxItems, "InitializingToolboxItems already turned off");
				myInitializingToolboxItems = value;
			}
		}
		#endregion // InitializingToolboxItems property
		#region Delayed Model Validation
		/// <summary>
		/// The rule priority where delay validation fires during a local commit.
		/// Note that rules marked with <see cref="TimeToFire.TopLevelCommit"/> and
		/// <see cref="TimeToFire.LocalCommit"/> fire at the same time for the top
		/// level transaction. This value is set below the lowest <see cref="DiagramFixupConstants"/>
		/// value. Any shape fixup rules should use the DiagramFixupConstants values
		/// to ensure that shape fixup happens after DelayValidation.
		/// </summary>
		public const int DelayValidateRulePriority = -200;
		/// <summary>
		/// A rule priority for <see cref="TimeToFire.TopLevelCommit"/> and <see cref="TimeToFire.LocalCommit"/>
		/// rules to run at if they must run prior to delay validation. See <see cref="DelayValidateRulePriority"/>
		/// for additional information.
		/// </summary>
		public const int BeforeDelayValidateRulePriority = DelayValidateRulePriority - 500;
		private static readonly object DelayedValidationContextKey = new object();
		/// <summary>
		/// Class to delay validate rules when a transaction is committing.
		/// </summary>
		/// <summary>
		/// AddRule: typeof(DelayValidateSignal), FireTime=LocalCommit, Priority=FrameworkDomainModel.DelayValidateRulePriority;
		/// Delay validate registered elements during LocalCommit
		/// </summary>
		[DebuggerStepThrough]
		private static void DelayValidateElements(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			Store store = element.Store;
			element.Delete();
			Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.Context.ContextInfo;
			object validatorsObject;
			if (contextInfo.TryGetValue(DelayedValidationContextKey, out validatorsObject)) // Let's do a while in case a new validator is added by the other validators
			{
				Dictionary<ElementValidator, object> validators = (Dictionary<ElementValidator, object>)validatorsObject;
				Dictionary<ElementValidator, object>.KeyCollection currentKeys = validators.Keys;
				Dictionary<ElementValidator, object> sideEffectValidators = new Dictionary<ElementValidator, object>();
				contextInfo[DelayedValidationContextKey] = sideEffectValidators;
				int keyCount = currentKeys.Count;
				if (keyCount == 0)
				{
					contextInfo.Remove(DelayedValidationContextKey);
					validators = null;
				}
				else
				{
					IComparer<ElementValidator> comparer = new ElementValidatorOrderComparer(store);
					// Update the validation context key to keep us in this loop
					contextInfo[DelayedValidationContextKey] = sideEffectValidators;
					// Sort the validators based on domain model priority
					List<ElementValidator> sortedValidators = new List<ElementValidator>(currentKeys);
					sortedValidators.Sort(comparer);
					while (keyCount != 0)
					{
						--keyCount;
						ElementValidator key = sortedValidators[keyCount];
						key.Validate();
						validators.Remove(key);
						sortedValidators.RemoveAt(keyCount);
						if (sideEffectValidators.Count != 0)
						{
							// Additional validators were added as side effects. Move them
							// into the current sorted set.
							foreach (ElementValidator newValidator in sideEffectValidators.Keys)
							{
								if (!validators.ContainsKey(newValidator))
								{
									validators[newValidator] = null;
									int insertIndex = sortedValidators.BinarySearch(newValidator, comparer);
									sortedValidators.Insert((insertIndex < 0) ? ~insertIndex : insertIndex, newValidator);
								}
							}
							sideEffectValidators.Clear();
						}
						keyCount = sortedValidators.Count;
					}
					contextInfo.Remove(DelayedValidationContextKey);
				}
			}
		}
		/// <summary>
		/// A comparer used to sort ElementValidator elements by model priority.
		/// Note that the sort happens in reverse priority to enable pulling
		/// the highest priority items off the end of a list.
		/// </summary>
		[DebuggerStepThrough]
		private sealed class ElementValidatorOrderComparer : IComparer<ElementValidator>
		{
			private IList<DomainModel> myDomainModels;
			public ElementValidatorOrderComparer(Store store)
			{
				// Get the ordered domain models. Note that this list is
				// in dependency order even if the domain models are not
				// provided in depedency order, so we can use it directly.
				ICollection<DomainModel> domainModelsCollection = store.DomainModels;
				int domainModelCount = domainModelsCollection.Count;
				DomainModel[] domainModelsArray = new DomainModel[domainModelCount];
				domainModelsCollection.CopyTo(domainModelsArray, 0);
				myDomainModels = domainModelsArray;
			}
			#region IComparer<ElementValidator> Implementation
			public int Compare(ElementValidator validator1, ElementValidator validator2)
			{
				ElementValidatorOrder order1 = validator1.OrderInformation;
				ElementValidatorOrder order2 = validator2.OrderInformation;
				DomainModel model1 = order1.DomainModel;
				DomainModel model2 = order2.DomainModel;
				if (model1 == model2)
				{
					int priority1 = order1.Priority;
					int priority2 = order2.Priority;
					if (priority1 == priority2)
					{
						return 0;
					}
					else if (priority1 < priority2)
					{
						// This would be -1 for forward order, reverse order here
						return 1;
					}
					return -1;
				}
				else if (myDomainModels.IndexOf(model1) < myDomainModels.IndexOf(model2))
				{
					// This would be -1 for forward order, reverse order here
					return 1;
				}
				return -1;
			}
			#endregion // IComparer<ElementValidator> Implementation
		}
		[DebuggerStepThrough]
		private struct ElementValidatorOrder : IEquatable<ElementValidatorOrder>
		{
			private DomainModel myDomainModel;
			private int myPriority;
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			public ElementValidatorOrder(DomainModel domainModel)
			{
				myDomainModel = domainModel;
				myPriority = 0;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorOrder(DomainModel domainModel, int priority)
			{
				myDomainModel = domainModel;
				myPriority = priority;
			}
			/// <summary>
			/// The <see cref="DomainModel"/> that this element runs with
			/// </summary>
			public DomainModel DomainModel
			{
				get
				{
					return myDomainModel;
				}
			}
			/// <summary>
			/// The relative priority to run this validation code for the given model
			/// </summary>
			public int Priority
			{
				get
				{
					return myPriority;
				}
			}
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				DomainModel domainModel = myDomainModel;
				return ((domainModel != null) ? domainModel.GetHashCode() : 0) ^ myPriority.GetHashCode();
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is ElementValidatorOrder && this.Equals((ElementValidatorOrderCache)obj);
			}
			/// <summary>See <see cref="IEquatable{ElementValidatorOrder}.Equals"/>.</summary>
			public bool Equals(ElementValidatorOrder other)
			{
				return myDomainModel == other.myDomainModel && myPriority == other.myPriority;
			}
		}
		[DebuggerStepThrough]
		private struct ElementValidatorOrderCache : IEquatable<ElementValidatorOrderCache>
		{
			private Guid myDomainModelId;
			private int myPriority;
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			public ElementValidatorOrderCache(Guid domainModelId)
			{
				myDomainModelId = domainModelId;
				myPriority = 0;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorOrderCache(Guid domainModelId, int priority)
			{
				myDomainModelId = domainModelId;
				myPriority = priority;
			}
			/// <summary>
			/// The id for the <see cref="DomainModel"/> that this element runs with
			/// </summary>
			public Guid DomainModelId
			{
				get
				{
					return myDomainModelId;
				}
			}
			/// <summary>
			/// The relative priority to run this validation code for the given model
			/// </summary>
			public int Priority
			{
				get
				{
					return myPriority;
				}
			}
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				return myDomainModelId.GetHashCode() ^ myPriority.GetHashCode();
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is ElementValidatorOrderCache && this.Equals((ElementValidatorOrderCache)obj);
			}
			/// <summary>See <see cref="IEquatable{ElementValidatorOrder}.Equals"/>.</summary>
			public bool Equals(ElementValidatorOrderCache other)
			{
				return myDomainModelId == other.myDomainModelId && myPriority == other.myPriority;
			}
		}
		/// <summary>
		/// A structure to use for <see cref="ModelElement"/> validation.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerDisplay("{(Validation!=null) ? System.String.Concat(Validation.Method.DeclaringType.FullName, \".\", Validation.Method.Name, \" (\", Element.ToString(),\")\") : GetType().Name}")]
		private struct ElementValidator : IEquatable<ElementValidator>
		{
			/// <summary>
			/// Initializes a new instance of <see cref="ElementValidator"/>.
			/// </summary>
			public ElementValidator(ModelElement element, ElementValidation validation)
			{
				this.Element = element;
				this.Validation = validation;
			}
			/// <summary>
			/// Invokes <see cref="Validation"/>, passing it <see cref="Element"/>.
			/// </summary>
			public void Validate()
			{
				TraceUtility.TraceDelegateStart(Element.Store, Validation);
				Validation(Element);
				TraceUtility.TraceDelegateEnd(Element.Store, Validation);
			}
			private static Dictionary<RuntimeMethodHandle, ElementValidatorOrderCache> myMethodToElementValidatorOrderCacheMap =
				new Dictionary<RuntimeMethodHandle, ElementValidatorOrderCache>(RuntimeMethodHandleComparer.Instance);
			/// <summary>
			/// Get the order information associated with this validator
			/// </summary>
			public ElementValidatorOrder OrderInformation
			{
				get
				{
					ElementValidatorOrder retVal = new ElementValidatorOrder();
					MethodInfo method = Validation.Method;
					RuntimeMethodHandle methodHandle = method.MethodHandle;
					ElementValidatorOrderCache orderCache;
					Store store = Element.Store;
					if (myMethodToElementValidatorOrderCacheMap.TryGetValue(methodHandle, out orderCache))
					{
						retVal = new ElementValidatorOrder(store.GetDomainModel(orderCache.DomainModelId), orderCache.Priority);
					}
					else
					{
						object[] explicitPriorityAttributes = method.GetCustomAttributes(typeof(DelayValidatePriorityAttribute), false);
						int priority = 0;
						Type explicitDomainModelType = null;
						if (explicitPriorityAttributes.Length != 0)
						{
							DelayValidatePriorityAttribute priorityAttr = (DelayValidatePriorityAttribute)explicitPriorityAttributes[0];
							priority = priorityAttr.Priority;
							explicitDomainModelType = priorityAttr.DomainModelType;
						}
						if (explicitDomainModelType != null)
						{
							DomainModelInfo explicitModelInfo = store.DomainDataDirectory.GetDomainModel(explicitDomainModelType);
							retVal = new ElementValidatorOrder(store.GetDomainModel(explicitModelInfo.Id), priority);
							myMethodToElementValidatorOrderCacheMap[methodHandle] = new ElementValidatorOrderCache(explicitModelInfo.Id, priority);
						}
						else
						{
							Type declaringType = method.DeclaringType;
							while (declaringType != null)
							{
								object[] idAttributes = declaringType.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
								if (idAttributes.Length != 0)
								{
									DomainClassInfo classInfo = store.DomainDataDirectory.FindDomainClass(((DomainObjectIdAttribute)idAttributes[0]).Id);
									if (classInfo != null)
									{
										Guid domainModelId = classInfo.DomainModel.Id;
										retVal = new ElementValidatorOrder(store.GetDomainModel(domainModelId), priority);
										myMethodToElementValidatorOrderCacheMap[methodHandle] = new ElementValidatorOrderCache(domainModelId, priority);
										break;
									}
								}
								declaringType = declaringType.DeclaringType;
							}
						}
						Debug.Assert(retVal.DomainModel != null, "Cannot find DomainModel for delay validation function: " + method.DeclaringType.FullName + "." + method.Name);
					}
					return retVal;
				}
			}
			/// <summary>
			/// The <see cref="ModelElement"/> to validate.
			/// </summary>
			private readonly ModelElement Element;
			/// <summary>
			/// The callback valdiation function.
			/// </summary>
			private readonly ElementValidation Validation;
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				return this.Element.GetHashCode() ^ this.Validation.GetHashCode();
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is ElementValidator && this.Equals((ElementValidator)obj);
			}
			/// <summary>See <see cref="IEquatable{ElementValidator}.Equals"/>.</summary>
			public bool Equals(ElementValidator other)
			{
				return this.Element.Equals(other.Element) && this.Validation.Equals(other.Validation);
			}
		}
		/// <summary>
		/// Called inside a transaction register a callback function that validates an
		/// element when the transaction completes. There are multiple model changes
		/// that can trigger most validation rules. Registering validation rules for delayed
		/// validation ensures that the validation code only runs once for any given object.
		/// </summary>
		/// <param name="element">The element to validate</param>
		/// <param name="validation">The validation function to run</param>
		/// <returns>true if this element/validator pair is being added for the first time in this transaction</returns>
		[DebuggerStepThrough]
		public static bool DelayValidateElement(ModelElement element, ElementValidation validation)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if ((object)validation == null)
			{
				throw new ArgumentNullException("validation");
			}
			Store store = element.Store;
			Debug.Assert(store.TransactionActive);
#if DEBUG
			if (validation.Target != null)
			{
				Debug.Fail(validation.Target.GetType().FullName + " registered non-static DelayValidate function: " + validation.Method.Name + ". The DelayValidation pattern should only need static functions.");
			}
#endif // DEBUG

			Dictionary<object, object> contextDictionary = store.TransactionManager.CurrentTransaction.Context.ContextInfo;
			object dictionaryObject;
			Dictionary<ElementValidator, object> dictionary;
			ElementValidator key = new ElementValidator(element, validation);
			if (contextDictionary.TryGetValue(DelayedValidationContextKey, out dictionaryObject))
			{
				dictionary = (Dictionary<ElementValidator, object>)dictionaryObject;
				if (dictionary.ContainsKey(key))
				{
					return false; // We're already validating this one
				}
			}
			else
			{
				contextDictionary[DelayedValidationContextKey] = dictionary = new Dictionary<ElementValidator, object>();
				new DelayValidateSignal(element.Partition);
			}
			dictionary[key] = null;
			return true;
		}
		#endregion // Delayed Model Validation
		#region TransactionRulesFixupHack class
		private sealed partial class TransactionRulesFixupHack
		{
			// UNDONE: There are still a few situations that this doesn't handle:
			//
			// If one or more new DomainModels are loaded during a transaction (before TransactionCommitting but after TransactionBeginning),
			// we will miss resorting the rules since we won't get called until the next transaction, and by then the 'committingRules' field
			// will no longer be null. We could partially work around this by resorting the rules at the start of every transaction (whether
			// or not the field is null), but the extra overhead that would cause isn't really worth it since for our purposes we don't load
			// new DomainModels in the middle of transactions anyway.
			// 
			// The fact that StoreDiagramMappingData is a static singleton and the way in which it is used leads to serious race conditions
			// when multiple Stores are being used at the same time (e.g. if two documents are open). ElementEventArgs from one transaction
			// can and do end up being processed by LineRoutingRule during the commit of completely unrelated transactions in separate Stores.
			// This has lead to some really bizarre behavior and obscure bugs. Unfortunately, we haven't been able to come up with anything
			// that we can do about it, since all of this is happening deep in the bowels of Store and Diagram.

			private static readonly RuntimeFieldHandle CommittingRulesFieldHandle = InitializeCommittingRulesFieldHandle();
			private static RuntimeFieldHandle InitializeCommittingRulesFieldHandle()
			{
				const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding;
				Type ruleManagerType = typeof(RuleManager);
				FieldInfo committingRulesFieldInfo = ruleManagerType.GetField("committingRules", bindingFlags);
				return (committingRulesFieldInfo != null && committingRulesFieldInfo.FieldType == typeof(List<TransactionCommittingRule>)) ? committingRulesFieldInfo.FieldHandle : default(RuntimeFieldHandle);
			}
			private delegate void GetCommittingRulesDelegate(RuleManager @this);
			private static readonly GetCommittingRulesDelegate GetCommittingRules = InitializeGetCommittingRules();
			private static GetCommittingRulesDelegate InitializeGetCommittingRules()
			{
				const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding;
				Type ruleManagerType = typeof(RuleManager);
				MethodInfo getCommittingRulesMethodInfo = ruleManagerType.GetMethod("GetCommittingRules", bindingFlags, null, CallingConventions.HasThis, Type.EmptyTypes, null);
				return (getCommittingRulesMethodInfo != null) ?
					(GetCommittingRulesDelegate)Delegate.CreateDelegate(typeof(GetCommittingRulesDelegate), getCommittingRulesMethodInfo, false) : null;
			}

			public TransactionRulesFixupHack()
				: base()
			{
				if (CommittingRulesFieldHandle.Value == IntPtr.Zero || (object)GetCommittingRules == null)
				{
					base.IsEnabled = false;
				}
			}

			private void ProcessTransactionBeginning(TransactionBeginningEventArgs e)
			{
				if (!base.IsEnabled)
				{
					// We have to check this ourselves since the RuleManager doesn't bother to...
					return;
				}

				RuleManager ruleManager = e.Transaction.Store.RuleManager;

				FieldInfo committingRulesFieldInfo = FieldInfo.GetFieldFromHandle(CommittingRulesFieldHandle);
				if (committingRulesFieldInfo.GetValue(ruleManager) == null)
				{
					// The committingRules field will only be null if a new DomainModel has been added since the last time this ran,
					// in which case, we need to resort the rules.
					GetCommittingRules(ruleManager);
					List<TransactionCommittingRule> committingRules = (List<TransactionCommittingRule>)committingRulesFieldInfo.GetValue(ruleManager);
					Debug.Assert(committingRules != null);
					committingRules.Sort(RulePriorityComparer<TransactionCommittingRule>.Instance);
				}
			}

			#region RulePriorityComparer class
			private sealed class RulePriorityComparer<TRule> : IComparer<TRule>
				where TRule : Rule
			{
				private RulePriorityComparer()
					: base()
				{
				}
				public static readonly RulePriorityComparer<TRule> Instance = new RulePriorityComparer<TRule>();
				public int Compare(TRule x, TRule y)
				{
					if ((object)x == (object)y)
					{
						return 0;
					}
					else if ((object)x == null)
					{
						return -1;
					}
					else if ((object)y == null)
					{
						return 1;
					}
					else
					{
						int diff = ((int)x.FireTime).CompareTo((int)y.FireTime);
						if (diff == 0)
						{
							diff = x.Priority.CompareTo(y.Priority);
							if (diff == 0)
							{
								diff = x.CompareTo(y);
							}
						}
						return diff;
					}
				}
			}
			#endregion // RulePriorityComparer class
		}
		#endregion // TransactionRulesFixupHack class
	}
}
