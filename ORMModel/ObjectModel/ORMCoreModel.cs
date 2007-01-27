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
		[RuleOn(typeof(CoreDomainModel))]
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
			#region IComparable<ElementValidator> Implementation
			public int Compare(ElementValidator validator1, ElementValidator validator2)
			{
				DomainModel model1 = validator1.DomainModel;
				DomainModel model2 = validator2.DomainModel;
				if (model1 == model2)
				{
					return 0;
				}
				else if (myDomainModels.IndexOf(model1) < myDomainModels.IndexOf(model2))
				{
					// This would be -1 for forward order, reverse order here
					return 1;
				}
				return -1;
			}
			#endregion // IComparable<ElementValidator> Implementation
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
			#region RuntimeMethodHandleEqualityComparer class
			/// <summary>
			/// <see cref="RuntimeTypeHandle"/> has a strongly-typed <c>Equals</c> method (<see cref="RuntimeTypeHandle.Equals(RuntimeTypeHandle)"/>),
			/// but does not implement <see cref="IEquatable{RuntimeTypeHandle}"/>. Therefore, we use this <see cref="IEqualityComparer{RuntimeTypeHandle}"/>
			/// implementation (which defers to that method) in order to avoid boxing.
			/// </summary>
			private sealed class RuntimeMethodHandleEqualityComparer : IEqualityComparer<RuntimeMethodHandle>
			{
				private RuntimeMethodHandleEqualityComparer()
					: base()
				{
				}
				public static readonly RuntimeMethodHandleEqualityComparer Instance = new RuntimeMethodHandleEqualityComparer();
				public bool Equals(RuntimeMethodHandle x, RuntimeMethodHandle y)
				{
					return x.Equals(y);
				}
				public int GetHashCode(RuntimeMethodHandle obj)
				{
					return obj.GetHashCode();
				}
			}
			#endregion // RuntimeMethodHandleEqualityComparer class
			private static Dictionary<RuntimeMethodHandle, Guid> myMethodToDomainModelMap = new Dictionary<RuntimeMethodHandle, Guid>(RuntimeMethodHandleEqualityComparer.Instance);
			/// <summary>
			/// Get the domain model associated with this validator
			/// </summary>
			public DomainModel DomainModel
			{
				get
				{
					DomainModel retVal = null;
					MethodInfo method = Validation.Method;
					RuntimeMethodHandle methodHandle = method.MethodHandle;
					Guid domainModelId;
					Store store = Element.Store;
					if (myMethodToDomainModelMap.TryGetValue(methodHandle, out domainModelId))
					{
						retVal = store.GetDomainModel(domainModelId);
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
									domainModelId = classInfo.DomainModel.Id;
									retVal = store.GetDomainModel(domainModelId);
									myMethodToDomainModelMap[methodHandle] = domainModelId;
									break;
								}
							}
							declaringType = declaringType.DeclaringType;
						}
						Debug.Assert(retVal != null, "Cannot find DomainModel for delay validation function: " + method.DeclaringType.FullName + "." + method.Name);
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
						ResourceStrings.CoreVerbalizationSnippetsDefaultDescription)
				};
			}
			VerbalizationSnippetsData[] IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets()
			{
				return ProvideVerbalizationSnippets();
			}
		}
		#endregion // IVerbalizationSnippetsProvider Implementation
		#region ISurveyNodeProvider Implementation
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes()
		{
			return this.GetSurveyNodes();
		}
		/// <summary>
		/// Provides an <see cref="IEnumerable{SampleDataElementNode}"/> for the <see cref="SurveyTreeContainer"/>.
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes()
		{
			IElementDirectory elementDirectory = Store.ElementDirectory;
			foreach (FactType element in elementDirectory.FindElements<FactType>(true))
			{
				yield return element;
			}
			foreach (ObjectType element in elementDirectory.FindElements<ObjectType>(true))
			{
				yield return element;
			}

			foreach (SetConstraint element in elementDirectory.FindElements<SetConstraint>(true))
			{
				yield return element;
			}

			foreach (SetComparisonConstraint element in elementDirectory.FindElements<SetComparisonConstraint>(true))
			{
				yield return element;
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
				eventNotify.ElementAdded(link.ObjectType);
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
		/// wired on SurveyQuestionLoad as event handler for ElementChanged events
		/// </summary>
		protected void ModelElementChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
				ISurveyQuestionTypeInfo[] effectedQuestions = (this as ISurveyQuestionProvider).GetSurveyQuestionTypeInfo();
				eventNotify.ElementChanged(link.ObjectType, effectedQuestions);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementPropertyChanged events
		/// </summary>
		protected void ModelElementNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
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
					eventNotify.ElementAdded(element.FactType);
				}
		}
		/// <summary>
		/// Set Constraint added
		/// </summary>
		protected void SetConstraintAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasSetConstraint element = e.ModelElement as ModelHasSetConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementAdded(element.SetConstraint);
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
			ModelHasSetComparisonConstraint element = e.ModelElement as ModelHasSetComparisonConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementAdded(element.SetComparisonConstraint);
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
		#endregion //SurveyEventHandling
	}
}
