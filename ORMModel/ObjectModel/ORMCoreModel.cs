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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A delegate callback to use with the <see cref="ORMCoreDomainModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The <see cref="ModelElement"/> to validate</param>
	public delegate void ElementValidation(ModelElement element);
	[VerbalizationSnippetsProvider("VerbalizationSnippets")]
	public partial class ORMCoreDomainModel : IORMModelEventSubscriber, ISurveyNodeProvider
	{
		private static Type[] questionTypes = new Type[] { typeof(SurveyQuestionGlyph) };
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
		#region TransactionRulesFixupHack class
		[RuleOn(typeof(CoreDomainModel))] // TransactionBeginningRule
		private sealed class TransactionRulesFixupHack : TransactionBeginningRule
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

			public sealed override void TransactionBeginning(TransactionBeginningEventArgs e)
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
		#region Delayed Model Validation
		#region DelayValidatePriorityAttribute class
		/// <summary>
		/// Place on a static delay validate method to explicitly control the execution order
		/// of the validate callbacks. All delay validate methods from a given domain model will
		/// be run before validators from dependent domain models are called. This attribute
		/// allows tighter control within a domain, or to make a delay validate function run with
		/// validators for earlier DomainModel.
		/// </summary>
		[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
		public sealed class DelayValidatePriorityAttribute : Attribute
		{
			private int myPriority;
			private Type myDomainModelType;
			/// <summary>
			/// Create a new <see cref="DelayValidatePriorityAttribute"/>
			/// </summary>
			/// <param name="priority">The relative priority to run the code. 0 is the default priority. Negative
			/// values will run before the default, positive values after.</param>
			public DelayValidatePriorityAttribute(int priority)
			{
				myPriority = priority;
			}
			/// <summary>The relative priority to run the code. 0 is the default priority. Negative
			/// values will run before the default, positive values after.</summary>
			public int Priority
			{
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
				get
				{
					return myDomainModelType;
				}
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
		[RuleOn(typeof(DelayValidateSignal), FireTime = TimeToFire.LocalCommit, Priority = DelayValidateRulePriority)] // AddRule
		private sealed partial class DelayValidateElements : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
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
		}
		/// <summary>
		/// A comparer used to sort ElementValidator elements by model priority.
		/// Note that the sort happens in reverse priority to enable pulling
		/// the highest priority items off the end of a list.
		/// </summary>
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
				this.Validation(this.Element);
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
		#region IORMModelEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers"/>.
		/// This implementation does nothing and does not need to be called.
		/// </summary>
		void IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
		}
		/// <summary>
		/// Implements <see cref="IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers"/>.
		/// </summary>
		protected void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			NamedElementDictionary.ManageEventHandlers(Store, eventManager, action);
		}
		void IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			this.ManagePostLoadModelingEventHandlers(eventManager, action);
		}
		/// <summary>
		/// Implementes <see cref="IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers"/>.
		/// </summary>
		protected void ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory directory = this.Store.DomainDataDirectory;
			//Object Type
			DomainClassInfo classInfo = directory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ModelElementRemoved), action);

			//Fact Type
			classInfo = directory.FindDomainClass(ModelHasFactType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemoved), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeAdded), action);

			//Set Constraint
			classInfo = directory.FindDomainClass(ModelHasSetConstraint.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetConstraintAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(SetConstraintDeleted), action);

			//Set Comparison
			classInfo = directory.FindDomainClass(ModelHasSetComparisonConstraint.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetComparisonConstraintAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(SetComparisonConstraintDeleted), action);

			//Track name change
			DomainPropertyInfo propertyInfo = directory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged), action);
			propertyInfo = directory.FindDomainProperty(FactType.NameChangedDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged), action);

			//FactTypeHasRole
			classInfo = directory.FindDomainClass(FactTypeHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleDeleted), action);

			//Role
			propertyInfo = directory.FindDomainProperty(Role.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChanged), action);

			//ValueTypeHasDataType
			classInfo = directory.FindDomainClass(ValueTypeHasDataType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasDataTypeAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasDataTypeDeleted), action);

			//Objectification
			classInfo = directory.FindDomainClass(Objectification.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationDeleted), action);
			propertyInfo = directory.FindDomainProperty(Objectification.IsImpliedDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectificationChanged), action);
			
			//RolePlayerChanged
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationRolePlayerChanged), action);

			//ModalityChanged
			DomainPropertyInfo info = directory.FindDomainProperty(SetConstraint.ModalityDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(ModalityChanged), action);
			info = directory.FindDomainProperty(SetComparisonConstraint.ModalityDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(ModalityChanged), action);

			//RingType changed
			info = directory.FindDomainProperty(RingConstraint.RingTypeDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(RingTypeChanged), action);

			//RingType changed
			info = directory.FindDomainProperty(UniquenessConstraint.IsPreferredDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(IsPreferredChanged), action);
			//ExclusiveOr added deleted 
			classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ExclusiveOrAdded), action);
			classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ExclusiveOrDeleted), action);

			//SubType
			info = directory.FindDomainProperty(SubtypeFact.IsPrimaryDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(SubtypeFactIsPrimaryChanged), action);
		}
			
		void IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			this.ManageSurveyQuestionModelingEventHandlers(eventManager, action);
		}
		#endregion // IORMModelEventSubscriber Implementation
		#region IVerbalizationSnippetsProvider Implementation
		private class VerbalizationSnippets : IVerbalizationSnippetsProvider
		{
			/// <summary>
			/// IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets
			/// </summary>
			protected VerbalizationSnippetsData[] ProvideVerbalizationSnippets()
			{
				return new VerbalizationSnippetsData[]
				{
					new VerbalizationSnippetsData(
						typeof(CoreVerbalizationSnippetType),
						CoreVerbalizationSets.Default,
						"Core",
						ResourceStrings.CoreVerbalizationSnippetsTypeDescription,
						ResourceStrings.CoreVerbalizationSnippetsDefaultDescription
					),
					new VerbalizationSnippetsData(
						typeof(ReportVerbalizationSnippetType),
						ReportVerbalizationSets.Default,
						"Report",
						ResourceStrings.VerbalizationReportSnippetsTypeDescription,
						ResourceStrings.VerbalizationReportSnippetsDefaultDescription
					)
				};
			}
			VerbalizationSnippetsData[] IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets()
			{
				return ProvideVerbalizationSnippets();
			}
		}
		#endregion // IVerbalizationSnippetsProvider Implementation
		#region ISurveyNodeProvider Implementation
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes(object context, object expansionKey)
		{
			return this.GetSurveyNodes(context, expansionKey);
		}
		/// <summary>
		/// Provides an <see cref="IEnumerable{SampleDataElementNode}"/> for the <see cref="SurveyTreeContainer"/>.
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes(object context, object expansionKey)
		{
			if (expansionKey == null)
			{
				IElementDirectory elementDirectory = Store.ElementDirectory;
				foreach (FactType element in elementDirectory.FindElements<FactType>(true))
				{
					if (null == element.ImpliedByObjectification)
					{
						yield return element;
					}
				}
				foreach (ObjectType element in elementDirectory.FindElements<ObjectType>(true))
				{
					yield return element;
				}

				foreach (SetConstraint element in elementDirectory.FindElements<SetConstraint>(true))
				{
					if (!((IConstraint)element).ConstraintIsInternal)
					{
						yield return element;
					}
				}

				foreach (SetComparisonConstraint element in elementDirectory.FindElements<SetComparisonConstraint>(true))
				{
					yield return element;
				}
			}
			else if (expansionKey == FactType.SurveyExpansionKey)
			{
				FactType factType = context as FactType;
				if (factType != null)
				{
					foreach (RoleBase role in factType.RoleCollection)
					{
						yield return role;
					}
					foreach (SetConstraint element in factType.GetInternalConstraints<SetConstraint>())
					{
						yield return element;
					}
					Objectification objectification = factType.Objectification;
					if (objectification != null)
					{
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							yield return impliedFactType;
						}
					}
				}
			}
		}
		#endregion // ISurveyNodeProvider Implementation
		#region SurveyEventHandling
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementAdded events
		/// </summary>
		protected void ModelElementAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				eventNotify.ElementAdded(link.ObjectType, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementDeleted events
		/// </summary>
		protected void ModelElementRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				eventNotify.ElementDeleted(link.ObjectType);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementPropertyChanged events
		/// </summary>
		protected void ModelElementNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted && null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for changes to a Role
		/// </summary>
		protected void RoleNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted && null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Role role = (Role)element;
				eventNotify.ElementRenamed(role);
				RoleProxy proxy = role.Proxy;
				if (proxy != null)
				{
					eventNotify.ElementRenamed(proxy);
				}
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void FactTypeRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasFactType element = e.ModelElement as ModelHasFactType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.FactType);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void FactTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasFactType element = e.ModelElement as ModelHasFactType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				Objectification objectification = factType.ImpliedByObjectification;
				eventNotify.ElementAdded(factType, (objectification != null) ? objectification.NestedFactType : null);
			}
		}
		/// <summary>
		/// Set Constraint added
		/// </summary>
		protected void SetConstraintAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			SetConstraint element = (e.ModelElement as ModelHasSetConstraint).SetConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				//do not add mandatory constraint if it's part of ExclusiveOr
				switch (((IConstraint)element).ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
					case ConstraintType.InternalUniqueness:
						LinkedElementCollection<FactType> factTypes = element.FactTypeCollection;
						if (factTypes.Count == 1)
						{
							// Add as a detail on the FactType, not the main list
							eventNotify.ElementAdded(element, factTypes[0]);
						}
						return;
					case ConstraintType.DisjunctiveMandatory:
						if ((element as MandatoryConstraint).ExclusiveOrExclusionConstraint != null)
						{
							return;
						}
						break;
				}
				eventNotify.ElementAdded(element, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void SetConstraintDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasSetConstraint element = e.ModelElement as ModelHasSetConstraint;			
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.SetConstraint);
			}
		}

		/// <summary>
		/// Set Comparison Constraint added
		/// </summary>
		protected void SetComparisonConstraintAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			SetComparisonConstraint element = (e.ModelElement as ModelHasSetComparisonConstraint).SetComparisonConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				//do not add the exclusion constraint if its part of ExclusiveOr. 
				if (null!=(element as ExclusionConstraint) && null !=((element as ExclusionConstraint).ExclusiveOrMandatoryConstraint))
				{
					return;
				}
				eventNotify.ElementAdded(element, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void SetComparisonConstraintDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasSetComparisonConstraint element = e.ModelElement as ModelHasSetComparisonConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.SetComparisonConstraint);
			}
		}

		/// <summary>
		/// Fact Type has role added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void FactTypeHasRoleAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			FactTypeHasRole element = e.ModelElement as FactTypeHasRole;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				eventNotify.ElementChanged(factType, typeof(SurveyQuestionGlyph));
				Role role = element.Role as Role;
				if (role != null) // ProxyRole is only added as part of an implicit fact type, don't notify separately
				{
					eventNotify.ElementAdded(role, factType);
				}
			}
		}

		/// <summary>
		/// Fact Type has role deleted
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void FactTypeHasRoleDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			FactTypeHasRole element = e.ModelElement as FactTypeHasRole;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				if (!factType.IsDeleted)
				{
					eventNotify.ElementChanged(factType, questionTypes);
					Role role = element.Role as Role;
					if (role != null)
					{
						eventNotify.ElementDeleted(role);
					}
				}
			}
		}
		/// <summary>
		/// Objectification added.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationAdded(object sender, ElementAddedEventArgs e)
		{	
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged)
				&& !element.IsImplied)
			{
					eventNotify.ElementChanged(element.NestingType, questionTypes);
					eventNotify.ElementChanged(element.NestedFactType, questionTypes);
			}
		}
		/// <summary>
		/// Objectification deleted.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged)
				&& !element.IsImplied)
			{
				ObjectType nestingType = element.NestingType;
				FactType nestedFactType = element.NestedFactType;
				if (!nestingType.IsDeleted)
				{
					eventNotify.ElementChanged(nestingType, questionTypes);
				}
				if (!nestedFactType.IsDeleted)
				{
					eventNotify.ElementChanged(nestedFactType, questionTypes);
				}
			}
		}
		/// <summary>
		/// Objectification property change.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == Objectification.IsImpliedDomainPropertyId)
				{
					ObjectType nestingType = element.NestingType;
					FactType nestedFactType = element.NestedFactType;
					if (!nestingType.IsDeleted)
					{
						eventNotify.ElementChanged(nestingType, questionTypes);
					}
					if (!nestedFactType.IsDeleted)
					{
						eventNotify.ElementChanged(nestedFactType, questionTypes);
					}
				}
			}
		}
		/// <summary>
		/// Objectifications role player changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RolePlayerChangedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationRolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
		{

			ObjectType newObjectType;			
			ObjectType oldObjectType;
			ModelElement element = e.NewRolePlayer;
			INotifySurveyElementChanged eventNotify;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				if (null != (newObjectType = element as ObjectType))
				{
					oldObjectType = e.OldRolePlayer as ObjectType;
					eventNotify.ElementChanged(newObjectType, questionTypes);
					eventNotify.ElementChanged(oldObjectType, questionTypes);
				}
			}
		}

		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ValueTypeHasDataTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ValueTypeHasDataType element = e.ModelElement as ValueTypeHasDataType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementChanged(element.ValueType, questionTypes);
			}
		}
		/// <summary>
		/// ValueTypeHasDataType Deleted
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ValueTypeHasDataTypeDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ValueTypeHasDataType element = e.ModelElement as ValueTypeHasDataType;
			ObjectType objectType = (e.ModelElement as ValueTypeHasDataType).ValueType;
			if (!objectType.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementChanged(objectType, questionTypes);
			}
		}

		/// <summary>
		/// Modality changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void ModalityChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				IConstraint constraint = element as IConstraint;
				eventNotify.ElementChanged(constraint, questionTypes);
			}
		}
		/// <summary>
		/// Ring type changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void RingTypeChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				RingConstraint ringConstraint = element as RingConstraint;
				eventNotify.ElementChanged(ringConstraint, questionTypes);
			}
		}
		/// <summary>
		/// External Uniqueness constraint IsPreferred property Changed
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void IsPreferredChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				UniquenessConstraint constraint = element as UniquenessConstraint;
				eventNotify.ElementChanged(constraint, questionTypes);
			}
		}

		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ExclusiveOrAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement as ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusiveOrConstraintCoupler coupler = element as ExclusiveOrConstraintCoupler;
				eventNotify.ElementAdded(coupler.ExclusionConstraint, null);
				eventNotify.ElementDeleted(coupler.MandatoryConstraint);
				eventNotify.ElementChanged(coupler.ExclusionConstraint, questionTypes);
			}
		}

		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ExclusiveOrDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement as ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusiveOrConstraintCoupler coupler = element as ExclusiveOrConstraintCoupler;
				if (!coupler.ExclusionConstraint.IsDeleted)
				{
					eventNotify.ElementAdded(coupler.MandatoryConstraint, null);
					eventNotify.ElementChanged(coupler.ExclusionConstraint, questionTypes);
				}
			}
		}


		/// <summary>
		/// SubType Fact IsPrimary property changed
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void SubtypeFactIsPrimaryChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				SubtypeFact subTypeFact = element as SubtypeFact;
				eventNotify.ElementChanged(subTypeFact, questionTypes);
			}
		}		

		#endregion //SurveyEventHandling
	}
}
