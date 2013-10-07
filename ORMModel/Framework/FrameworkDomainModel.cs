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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Diagnostics;
using ORMSolutions.ORMArchitect.Framework.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Reflection;

namespace ORMSolutions.ORMArchitect.Framework
{
	#region ElementValidation delegate
	/// <summary>
	/// A delegate callback to use with the <see cref="FrameworkDomainModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The <see cref="ModelElement"/> to validate</param>
	public delegate void ElementValidation(ModelElement element);
	#endregion // ElementValidation delegate
	#region DelayValidatePriorityOrder enum
	/// <summary>
	/// Determines the order where delay validation routines run relative
	/// to the routines in the <see cref="P:DelayValidatePriorityAttribute.DomainModelType"/>
	/// </summary>
	public enum DelayValidatePriorityOrder
	{
		/// <summary>
		/// Run this validation routine before routines that run with the domain model.
		/// All routines in this priority order will complete before routines running
		/// in the <see cref="WithDomainModel"/> ordering begin.
		/// </summary>
		BeforeDomainModel = -1,
		/// <summary>
		/// Run this routine with other routines that run with the domain model
		/// All routines in this priority order will complete before routines running
		/// in the <see cref="AfterDomainModel"/> ordering begin.
		/// </summary>
		WithDomainModel = 0,
		/// <summary>
		/// Run this routine with other routines that run after the domain model.
		/// </summary>
		AfterDomainModel = 1,
	}
	#endregion // DelayValidatePriorityOrder enum
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
		private DelayValidatePriorityOrder myOrder;
		private Type myDomainModelType;
		/// <summary>
		/// Create a new <see cref="DelayValidatePriorityAttribute"/>
		/// </summary>
		[DebuggerStepThrough]
		public DelayValidatePriorityAttribute()
		{
		}
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
		/// <summary>
		/// The <see cref="DelayValidatePriorityOrder">order</see>/> relative to the <see cref="DomainModelType"/> that
		/// this routine will run in.
		/// </summary>
		public DelayValidatePriorityOrder Order
		{
			[DebuggerStepThrough]
			get
			{
				return myOrder;
			}
			[DebuggerStepThrough]
			set
			{
				myOrder = value;
			}
		}
	}
	#endregion // DelayValidatePriorityAttribute class
	#region DelayValidateReplacesAttribute class
	/// <summary>
	/// Place on a static delay validate method to mark the method as a replacement for
	/// another delay validate method. A replacement validator is generally a method that
	/// does more detailed validation than the replaced method(s). Replacement methods chain
	/// naturally, so duplicate information should not be specified.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public sealed class DelayValidateReplacesAttribute : Attribute
	{
		private string myReplacesValidatorName;
		private Type myReplacesValidatorType;
		/// <summary>
		/// The method name that this validator replaces. At runtime, if
		/// one of the replaced validators is already registered with
		/// an element delayed validation, then specifying this validator
		/// will unregister the element from another validator. Similarly,
		/// a future registration with the replaced validator and element
		/// will not readd the replaced validation.
		/// </summary>
		/// <param name="replacesValidator">The method name to replace, defined
		/// on the same type as this method.</param>
		public DelayValidateReplacesAttribute(string replacesValidator)
		{
			myReplacesValidatorName = replacesValidator;
		}
		/// <summary>
		/// The method name that this validator replaces. At runtime, if
		/// one of the replaced validators is already registered with
		/// an element delayed validation, then specifying this validator
		/// will unregister the element from another validator. Similarly,
		/// a future registration with the replaced validator and element
		/// will not readd the replaced validation.
		/// </summary>
		/// <param name="replacesValidator">The method name to replace</param>
		/// <param name="replacesValidatorType">The type to find the replacement
		/// method on. Needed only if the replacement method is on a different type.</param>
		public DelayValidateReplacesAttribute(string replacesValidator, Type replacesValidatorType)
		{
			myReplacesValidatorName = replacesValidator;
			myReplacesValidatorType = replacesValidatorType;
		}
		/// <summary>
		/// The method name that this validator replaces.
		/// </summary>
		public string ReplacesValidator
		{
			[DebuggerStepThrough]
			get
			{
				return myReplacesValidatorName;
			}
		}
		/// <summary>
		/// The type used to find the <see cref="ReplacesValidator"/> method.
		/// </summary>
		public Type ReplacesValidatorType
		{
			[DebuggerStepThrough]
			get
			{
				return myReplacesValidatorType;
			}
		}
	}
	#endregion // DelayValidateReplacesAttribute class
	partial class FrameworkDomainModel : IPersistentSessionKeys
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
		/// The framework transaction engine can force all inline rules to
		/// commit time. When this happens, we need our 'inline' rules to run
		/// before any commit-time rules. All inline priorities need to run
		/// at a reasonable offset from this value to avoid issues.
		/// </summary>
		public const int InlineRulePriority = -100000;
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
		/// <summary>
		/// A rule priority for <see cref="TimeToFire.TopLevelCommit"/> and <see cref="TimeToFire.LocalCommit"/>
		/// rules to run when copy closure expansion is complete. At this point, closure elements have been
		/// fully expanded, so elements are no longer in a partial state. This rule priority runs prior to
		/// <see cref="BeforeDelayValidateRulePriority"/> and <see cref="DelayValidateRulePriority"/>. Inline
		/// rules that assume full state before validation should create multiple rules at different priorities
		/// and check the <see cref="CopyMergeUtility.GetIntegrationPhase"/> to determine if the rule should
		/// be processed.
		/// </summary>
		public const int CopyClosureExpansionCompletedRulePriority = BeforeDelayValidateRulePriority - 500;
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
			Dictionary<object, object> contextInfo = GetContextInfo(store.TransactionManager.CurrentTransaction);
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
									// Handle replacements when moving in new validators. The side effect
									// validators are internally consistent because the strongest validator
									// is checked before the key is added, but all bets are off when it comes
									// to merging these elements into the existing validators.
									IEnumerable<ElementValidation> relatedValidators;
									ModelElement validatedElement = newValidator.Element;
									if (null != (relatedValidators = newValidator.ReplacedByValidators))
									{
										foreach (ElementValidation strongerValidator in relatedValidators)
										{
											if (validators.ContainsKey(new ElementValidator(validatedElement, strongerValidator)))
											{
												// The existing validator is stronger, use it instead of this one.
												continue;
											}
										}
									}
									if (null != (relatedValidators = newValidator.ReplacedValidators))
									{
										ElementValidator weakerKey;
										foreach (ElementValidation weakerValidator in relatedValidators)
										{
											if (validators.ContainsKey(weakerKey = new ElementValidator(validatedElement, weakerValidator)))
											{
												validators.Remove(weakerKey);
												int removeIndex = sortedValidators.BinarySearch(weakerKey, comparer);
												// The removeIndex here will have the same order, but it is not guaranteed
												// to be the same item. We only know that we're in the block of items with
												// the same order, so we need to search forwards and backwards from this
												// point to find the item.
												if (!weakerKey.Equals(sortedValidators[removeIndex]))
												{
													bool haveMatch = false;
													keyCount = sortedValidators.Count;
													for (int i = removeIndex + 1; i < keyCount; ++i)
													{
														ElementValidator currentValidator;
														if (weakerKey.Equals(currentValidator = sortedValidators[i]))
														{
															haveMatch = true;
															removeIndex = i;
															break;
														}
														else if (0 != comparer.Compare(currentValidator, weakerKey))
														{
															break;
														}
													}
													if (!haveMatch)
													{
														for (int i = removeIndex -1; i >= 0; --i)
														{
															// We must find it somewhere
															if (weakerKey.Equals(sortedValidators[i]))
															{
																removeIndex = i;
																break;
															}
														}
													}
												}
												sortedValidators.RemoveAt(removeIndex);
											}
										}
									}
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
					DelayValidatePriorityOrder relativeOrder1 = order1.Order;
					DelayValidatePriorityOrder relativeOrder2 = order2.Order;
					if (relativeOrder1 == relativeOrder2)
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
					else if (relativeOrder1 < relativeOrder2)
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
			private DelayValidatePriorityOrder myOrder;
			private int myPriority;
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			public ElementValidatorOrder(DomainModel domainModel)
			{
				myDomainModel = domainModel;
				myOrder = DelayValidatePriorityOrder.WithDomainModel;
				myPriority = 0;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority and explicit order
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="order">The <see cref="DelayValidatePriorityOrder"/> the validator runs in relative to the <paramref name="domainModel"/>.</param>
			public ElementValidatorOrder(DomainModel domainModel, DelayValidatePriorityOrder order)
			{
				myDomainModel = domainModel;
				myOrder = order;
				myPriority = 0;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with explicit order and priority
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="order">The <see cref="DelayValidatePriorityOrder"/> the validator runs in relative to the <paramref name="domainModel"/>.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorOrder(DomainModel domainModel, DelayValidatePriorityOrder order, int priority)
			{
				myDomainModel = domainModel;
				myOrder = order;
				myPriority = priority;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with an explicit priority
			/// </summary>
			/// <param name="domainModel">The <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorOrder(DomainModel domainModel, int priority)
			{
				myDomainModel = domainModel;
				myPriority = priority;
				myOrder = DelayValidatePriorityOrder.WithDomainModel;
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
			/// The order relative to other validate routines in the same domain model
			/// </summary>
			public DelayValidatePriorityOrder Order
			{
				get
				{
					return myOrder;
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
				return Utility.GetCombinedHashCode(
					(domainModel != null) ? domainModel.GetHashCode() : 0,
					myOrder.GetHashCode(),
					myPriority.GetHashCode());
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is ElementValidatorOrder && this.Equals((ElementValidatorOrder)obj);
			}
			/// <summary>See <see cref="IEquatable{ElementValidatorOrder}.Equals"/>.</summary>
			public bool Equals(ElementValidatorOrder other)
			{
				return myDomainModel == other.myDomainModel && myOrder == other.myOrder && myPriority == other.myPriority;
			}
		}
		[DebuggerStepThrough]
		private struct ElementValidatorInfoCache : IEquatable<ElementValidatorInfoCache>
		{
			private Guid myDomainModelId;
			private DelayValidatePriorityOrder myOrder;
			private int myPriority;
			private List<RuntimeMethodHandle> myReplacesMethodHandles;
			private List<RuntimeMethodHandle> myReplacedByMethodHandles;
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			public ElementValidatorInfoCache(Guid domainModelId)
			{
				myDomainModelId = domainModelId;
				myOrder = DelayValidatePriorityOrder.WithDomainModel;
				myPriority = 0;
				myReplacesMethodHandles = null;
				myReplacedByMethodHandles = null;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with a default priority and explicit order
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="order">The <see cref="DelayValidatePriorityOrder"/> the validator runs in relative to the <paramref name="domainModelId"/>.</param>
			public ElementValidatorInfoCache(Guid domainModelId, DelayValidatePriorityOrder order)
			{
				myDomainModelId = domainModelId;
				myOrder = order;
				myPriority = 0;
				myReplacesMethodHandles = null;
				myReplacedByMethodHandles = null;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with explicit order and priority
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="order">The <see cref="DelayValidatePriorityOrder"/> the validator runs in relative to the <paramref name="domainModelId"/>.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorInfoCache(Guid domainModelId, DelayValidatePriorityOrder order, int priority)
			{
				myDomainModelId = domainModelId;
				myOrder = order;
				myPriority = priority;
				myReplacesMethodHandles = null;
				myReplacedByMethodHandles = null;
			}
			/// <summary>
			/// Create a new ElementValidatorOrder structure with an explicit priority
			/// </summary>
			/// <param name="domainModelId">The id for the <see cref="DomainModel"/> the validator runs with.</param>
			/// <param name="priority">A custom priority. The default priority is 0. Anything less runs before, anything higher afterwards</param>
			public ElementValidatorInfoCache(Guid domainModelId, int priority)
			{
				myDomainModelId = domainModelId;
				myPriority = priority;
				myOrder = DelayValidatePriorityOrder.WithDomainModel;
				myReplacesMethodHandles = null;
				myReplacedByMethodHandles = null;
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
			/// The order relative to other validate routines in the same domain model
			/// </summary>
			public DelayValidatePriorityOrder Order
			{
				get
				{
					return myOrder;
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


			/// <summary>
			/// Populate the info cache for the specified method. This should be called only
			/// if the cache information for this method is not already cached.
			/// </summary>
			/// <param name="method">The method to create a cache for.</param>
			/// <param name="store">The requesting <see cref="Store"/>, used to find domain model identifiers.</param>
			/// <param name="cacheMap">The caching dictionary that owns these elements</param>
			public static ElementValidatorInfoCache PopulateValidatorInfoCache(MethodInfo method, Store store, Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap)
			{
				ElementValidatorInfoCache infoCache = default(ElementValidatorInfoCache);
				bool haveInfoCache = false;
				object[] attributes = method.GetCustomAttributes(typeof(DelayValidatePriorityAttribute), false);
				DelayValidatePriorityOrder order = DelayValidatePriorityOrder.WithDomainModel;
				int priority = 0;
				Type explicitDomainModelType = null;
				Type declaringType;
				if (attributes.Length != 0)
				{
					DelayValidatePriorityAttribute priorityAttr = (DelayValidatePriorityAttribute)attributes[0];
					priority = priorityAttr.Priority;
					order = priorityAttr.Order;
					explicitDomainModelType = priorityAttr.DomainModelType;
				}
				if (explicitDomainModelType != null)
				{
					infoCache = new ElementValidatorInfoCache(store.DomainDataDirectory.GetDomainModel(explicitDomainModelType).Id, order, priority);
					haveInfoCache = true;
				}
				else
				{
					declaringType = method.DeclaringType;
					while (declaringType != null)
					{
						object[] idAttributes = declaringType.GetCustomAttributes(typeof(DomainObjectIdAttribute), false);
						if (idAttributes.Length != 0)
						{
							DomainClassInfo classInfo = store.DomainDataDirectory.FindDomainClass(((DomainObjectIdAttribute)idAttributes[0]).Id);
							if (classInfo != null)
							{
								infoCache = new ElementValidatorInfoCache(classInfo.DomainModel.Id, order, priority);
								haveInfoCache = true;
								break;
							}
						}
						declaringType = declaringType.DeclaringType;
					}
					Debug.Assert(!haveInfoCache || store.FindDomainModel(infoCache.DomainModelId) != null, "Cannot find DomainModel for delay validation function: " + method.DeclaringType.FullName + "." + method.Name);
				}
				if (haveInfoCache)
				{
					RuntimeMethodHandle methodHandle = method.MethodHandle;

					// We have the basic cached information, continue with replacement methods
					attributes = method.GetCustomAttributes(typeof(DelayValidateReplacesAttribute), false);
					int attributeLength = attributes.Length;
					if (attributeLength != 0)
					{
						declaringType = method.DeclaringType;
						Type[] parameterTypes = new Type[] { typeof(ModelElement) };
						for (int i = 0; i < attributeLength; ++i)
						{
							DelayValidateReplacesAttribute replacesAttribute = (DelayValidateReplacesAttribute)attributes[i];
							Type methodType = replacesAttribute.ReplacesValidatorType ?? declaringType;
							MethodInfo replacesMethod = methodType.GetMethod(replacesAttribute.ReplacesValidator, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);
							if (replacesMethod != null)
							{
								RuntimeMethodHandle replacesMethodHandle = replacesMethod.MethodHandle;
								ElementValidatorInfoCache replacesInfoCache;
								if (!cacheMap.TryGetValue(replacesMethodHandle, out replacesInfoCache))
								{
									replacesInfoCache = PopulateValidatorInfoCache(replacesMethod, store, cacheMap);
								}

								// Mark the replaced method as being replaced by this method. This
								// also recursively marks methods already replaced by the replaced
								// method as being replaced by this one.
								replacesInfoCache.AddReplacedByMethodHandle(methodHandle, replacesMethodHandle, cacheMap);

								// Mark this method as replacing the replaced method.
								infoCache.AddReplacesMethodHandle(replacesMethodHandle, methodHandle, cacheMap);
							}
							else
							{
								Debug.Fail("Delay validator replacement method " + replacesAttribute.ReplacesValidator + " on " + methodType.FullName + " not found");
							}
						}
					}
					cacheMap[methodHandle] = infoCache;
				}
				return infoCache;
			}
			/// <summary>
			/// Add a method handle that this one replaces.
			/// </summary>
			/// <param name="replacesMethodHandle">The method handle that this method replaces.</param>
			/// <param name="thisMethodHandle">The method handle for the method corresponding to this information.</param>
			/// <param name="cacheMap">The cache that stores these elements.</param>
			private void AddReplacesMethodHandle(RuntimeMethodHandle replacesMethodHandle, RuntimeMethodHandle thisMethodHandle, Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap)
			{
				// Add the 'replaces' handle to ourselves
				List<RuntimeMethodHandle> handles;
				if (null == (handles = myReplacesMethodHandles))
				{
					myReplacesMethodHandles = handles = new List<RuntimeMethodHandle>();
					cacheMap[thisMethodHandle] = this;
				}
				handles.Add(replacesMethodHandle);

				// Recurse to add the same information to other methods that replace us.
				if (null != (handles = myReplacedByMethodHandles))
				{
					int handleCount = handles.Count;
					for (int i = 0; i < handleCount; ++i)
					{
						RuntimeMethodHandle parentHandle = handles[i];
						cacheMap[parentHandle].AddReplacesMethodHandle(replacesMethodHandle, parentHandle, cacheMap);
					}
				}
			}
			/// <summary>
			/// See if this validator can replace a different validator.
			/// </summary>
			public bool CanReplaceValidators
			{
				get
				{
					return myReplacesMethodHandles != null;
				}
			}
			/// <summary>
			/// Enumerate callback functions that can be replace this method.
			/// </summary>
			public IEnumerable<ElementValidation> GetReplacedValidators()
			{
				return EnumerateDelegates(myReplacesMethodHandles);
			}
			/// <summary>
			/// Add a method handle that this one is replaced by.
			/// </summary>
			/// <param name="replacedByMethodHandle">The method handle that replaces this method.</param>
			/// <param name="thisMethodHandle">The method handle for the method corresponding to this information.</param>
			/// <param name="cacheMap">The cache that stores these elements.</param>
			private void AddReplacedByMethodHandle(RuntimeMethodHandle replacedByMethodHandle, RuntimeMethodHandle thisMethodHandle, Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap)
			{
				// Add the 'replaced by' handle to ourselves.
				List<RuntimeMethodHandle> handles;
				if (null == (handles = myReplacedByMethodHandles))
				{
					myReplacedByMethodHandles = handles = new List<RuntimeMethodHandle>();
					cacheMap[thisMethodHandle] = this;
				}
				handles.Add(replacedByMethodHandle);

				// Recurse to add the same information to other methods that we already replace.
				if (null != (handles = myReplacesMethodHandles))
				{
					int handleCount = handles.Count;
					for (int i = 0; i < handleCount; ++i)
					{
						RuntimeMethodHandle childHandle = handles[i];
						cacheMap[childHandle].AddReplacedByMethodHandle(replacedByMethodHandle, childHandle, cacheMap);
					}
				}
			}
			/// <summary>
			/// See if this validator can be replaced by a different validator.
			/// </summary>
			public bool CanBeReplacedByValidators
			{
				get
				{
					return myReplacedByMethodHandles != null;
				}
			}
			/// <summary>
			/// Enumerate callback functions that can be replaced
			/// by this method.
			/// </summary>
			public IEnumerable<ElementValidation> GetReplacedByValidators()
			{
				return EnumerateDelegates(myReplacedByMethodHandles);
			}
			/// <summary>
			/// Shared code for enumerating runtime handles and turning them
			/// into delegates.
			/// </summary>
			private IEnumerable<ElementValidation> EnumerateDelegates(List<RuntimeMethodHandle> handles)
			{
				int methodCount;
				if (null != handles &&
					0 != (methodCount = handles.Count))
				{
					Type validatorType = typeof(ElementValidation);
					for (int i = 0; i < methodCount; ++i)
					{
						yield return (ElementValidation)Delegate.CreateDelegate(validatorType, (MethodInfo)MethodBase.GetMethodFromHandle(handles[i]));
					}
				}
			}
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				return Utility.GetCombinedHashCode(
					myDomainModelId.GetHashCode(),
					myOrder.GetHashCode(),
					myPriority.GetHashCode());
				// Ignore method handle information for hash code and equality computation.
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is ElementValidatorInfoCache && this.Equals((ElementValidatorInfoCache)obj);
			}
			/// <summary>See <see cref="IEquatable{ElementValidatorOrder}.Equals"/>.</summary>
			public bool Equals(ElementValidatorInfoCache other)
			{
				return myDomainModelId == other.myDomainModelId && myOrder == other.myOrder && myPriority == other.myPriority;
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
			private static Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> myMethodToElementValidatorInfoCacheMap =
				new Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache>(RuntimeMethodHandleComparer.Instance);
			/// <summary>
			/// Get the order information associated with this validator
			/// </summary>
			public ElementValidatorOrder OrderInformation
			{
				get
				{
					ElementValidatorInfoCache infoCache;
					MethodInfo methodInfo = Validation.Method;
					Store store = Element.Store;
					Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap = myMethodToElementValidatorInfoCacheMap;
					if (!cacheMap.TryGetValue(methodInfo.MethodHandle, out infoCache))
					{
						infoCache = ElementValidatorInfoCache.PopulateValidatorInfoCache(methodInfo, store, cacheMap);
					}
					return new ElementValidatorOrder(store.GetDomainModel(infoCache.DomainModelId), infoCache.Order, infoCache.Priority);
				}
			}
			/// <summary>
			/// Get element validators that this validator replaces. Can return null.
			/// </summary>
			public IEnumerable<ElementValidation> ReplacedValidators
			{
				get
				{
					ElementValidatorInfoCache infoCache;
					MethodInfo methodInfo = Validation.Method;
					Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap = myMethodToElementValidatorInfoCacheMap;
					if (!cacheMap.TryGetValue(methodInfo.MethodHandle, out infoCache))
					{
						infoCache = ElementValidatorInfoCache.PopulateValidatorInfoCache(methodInfo, Element.Store, cacheMap);
					}
					return infoCache.CanReplaceValidators ? infoCache.GetReplacedValidators() : null;
				}
			}
			/// <summary>
			/// Get element validators that can be replaced by this one. Can return null.
			/// </summary>
			public IEnumerable<ElementValidation> ReplacedByValidators
			{
				get
				{
					ElementValidatorInfoCache infoCache;
					MethodInfo methodInfo = Validation.Method;
					Dictionary<RuntimeMethodHandle, ElementValidatorInfoCache> cacheMap = myMethodToElementValidatorInfoCacheMap;
					if (!cacheMap.TryGetValue(methodInfo.MethodHandle, out infoCache))
					{
						infoCache = ElementValidatorInfoCache.PopulateValidatorInfoCache(methodInfo, Element.Store, cacheMap);
					}
					return infoCache.CanBeReplacedByValidators ? infoCache.GetReplacedByValidators() : null;
				}

			}
			/// <summary>
			/// The <see cref="ModelElement"/> to validate.
			/// </summary>
			public readonly ModelElement Element;
			/// <summary>
			/// The callback valdiation function.
			/// </summary>
			public readonly ElementValidation Validation;
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

			Dictionary<object, object> contextDictionary = GetContextInfo(store.TransactionManager.CurrentTransaction);
			object dictionaryObject;
			Dictionary<ElementValidator, object> dictionary;
			ElementValidator key = new ElementValidator(element, validation);
			bool existingDictionary = true;
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
				existingDictionary = false;
				// Create the validation signals in an alternate partition. This
				// enables elements in other alternate partitions to use delayed
				// validation without marking the primary partition as dirty.
				// The alternate partition will always be empty at the end of the transaction.
				Partition partition = Partition.FindByAlternateId(store, typeof(DelayValidateSignal));
				if (partition == null)
				{
					partition = new Partition(store);
					partition.AlternateId = typeof(DelayValidateSignal);
				}
				new DelayValidateSignal(partition);
			}
			if (existingDictionary)
			{
				IEnumerable<ElementValidation> relatedValidators;
				if (null != (relatedValidators = key.ReplacedByValidators))
				{
					foreach (ElementValidation strongerValidator in relatedValidators)
					{
						if (dictionary.ContainsKey(new ElementValidator(element, strongerValidator)))
						{
							// The validation for this element is already handled by a stronger validator
							return false;
						}
					}
				}
				if (null != (relatedValidators = key.ReplacedValidators))
				{
					ElementValidator weakerKey;
					foreach (ElementValidation weakerValidator in relatedValidators)
					{
						if (dictionary.ContainsKey(weakerKey = new ElementValidator(element, weakerValidator)))
						{
							dictionary.Remove(weakerKey);
						}
					}
				}
			}
			dictionary[key] = null;
			return true;
		}
		/// <summary>
		/// Get the appropriate context dictionary to use for these objects.
		/// </summary>
		/// <param name="currentTransaction">The current <see cref="Transaction"/></param>
		/// <returns>The context dictionary of this transaction unless the parent transaction
		/// forces all rules to commit time, in which case we use the context from the first
		/// ancestor transaction without a parent that forces rules to commit time.</returns>
		private static Dictionary<object, object> GetContextInfo(Transaction currentTransaction)
		{
			Transaction parentTransaction = currentTransaction.Parent;;
			while (parentTransaction != null &&
				parentTransaction.ForceAllRulesToCommitTime)
			{
				currentTransaction = parentTransaction;
				parentTransaction = currentTransaction.Parent;
			}
			return currentTransaction.Context.ContextInfo;
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
		#region IPersistentSessionKeys Implementation
		/// <summary>
		/// Implements <see cref="IPersistentSessionKeys.GetPersistentSessionKeys"/>
		/// Returns <see cref="CopyMergeUtility.IgnoredSourceExtensionsKey"/>
		/// </summary>
		/// <returns></returns>
		protected IEnumerable<object> GetPersistentSessionKeys()
		{
			yield return CopyMergeUtility.IgnoredSourceExtensionsKey;
		}
		IEnumerable<object> IPersistentSessionKeys.GetPersistentSessionKeys()
		{
			return GetPersistentSessionKeys();
		}
		#endregion // IPersistentSessionKeys Implementation
	}
}
