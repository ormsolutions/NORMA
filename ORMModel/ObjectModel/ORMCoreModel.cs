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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A delegate callback to use with the <see cref="ORMCoreDomainModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The element to validate</param>
	public delegate void ElementValidator(ModelElement element);
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
		#region Delayed Model Validation
		private static readonly object DelayedValidationContextKey = new object();
		/// <summary>
		/// Class to delay validate rules when a transaction is committing.
		/// </summary>
		[RuleOn(typeof(ORMCoreDomainModel), FireTime = TimeToFire.LocalCommit)] // TransactionCommittingRule
		private sealed class DelayValidateElements : TransactionCommittingRule
		{
			public sealed override void TransactionCommitting(TransactionCommitEventArgs e)
			{
				Dictionary<object, object> contextInfo = e.Transaction.Context.ContextInfo;
				while (contextInfo.ContainsKey(DelayedValidationContextKey)) // Let's do a while in case a new validator is added by the other validators
				{
					Dictionary<ElementValidatorKey, object> validators = (Dictionary<ElementValidatorKey, object>)contextInfo[DelayedValidationContextKey];
					contextInfo.Remove(DelayedValidationContextKey);
					foreach (ElementValidatorKey key in validators.Keys)
					{
						key.Validate();
					}
				}
			}
		}
		/// <summary>
		/// A structure to use as a key for tracking <see cref="ModelElement"/> validation.
		/// </summary>
		private struct ElementValidatorKey
		{
			/// <summary>
			/// Initializes a new instance of <see cref="ElementValidatorKey"/>.
			/// </summary>
			public ElementValidatorKey(ModelElement element, ElementValidator validator)
			{
				Element = element;
				Validator = validator;
			}
			/// <summary>
			/// Invokes <see cref="Validator"/>, passing it <see cref="Element"/>.
			/// </summary>
			public void Validate()
			{
				this.Validator(this.Element);
			}
			/// <summary>
			/// The <see cref="ModelElement"/> to validate.
			/// </summary>
			public readonly ModelElement Element;
			/// <summary>
			/// The callback valdiation function.
			/// </summary>
			public readonly ElementValidator Validator;
		}
		/// <summary>
		/// Called inside a transaction register a callback function validate an
		/// element when the transaction completes. There are multiple model changes
		/// that can trigger most validation rules. Registering validation rules for delayed
		/// validation ensures that the validation code only runs once for any given objects.
		/// </summary>
		/// <param name="element">The element to validate</param>
		/// <param name="validator">The validation function to run</param>
		public static void DelayValidateElement(ModelElement element, ElementValidator validator)
		{
			Store store = element.Store;
			Debug.Assert(store.TransactionActive);
			Dictionary<object, object> contextDictionary = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			Dictionary<ElementValidatorKey, object> dictionary = null;
			ElementValidatorKey key = new ElementValidatorKey(element, validator);
			if (contextDictionary.ContainsKey(DelayedValidationContextKey))
			{
				dictionary = (Dictionary<ElementValidatorKey, object>)contextDictionary[DelayedValidationContextKey];
				if (dictionary.ContainsKey(key))
				{
					return; // We're already validating this one
				}
			}
			else
			{
				dictionary = new Dictionary<ElementValidatorKey, object>();
				contextDictionary[DelayedValidationContextKey] = dictionary;
			}
			dictionary[key] = null;
		}
		#endregion // Delayed Model Validation
		#region IORMModelEventSubscriber Implementation
		/// <summary>
		/// Implements IORMModelEventSubscriber.AddPreLoadModelingEventHandlers
		/// </summary>
		protected static void AddPreLoadModelingEventHandlers()
		{
		}
		void IORMModelEventSubscriber.AddPreLoadModelingEventHandlers()
		{
			AddPreLoadModelingEventHandlers();
		}
		/// <summary>
		/// Implements IORMModelEventSubscriber.AddPostLoadModelingEventHandlers
		/// </summary>
		protected void AddPostLoadModelingEventHandlers()
		{
			NamedElementDictionary.AttachEventHandlers(Store);
		}
		void IORMModelEventSubscriber.AddPostLoadModelingEventHandlers()
		{
			AddPostLoadModelingEventHandlers();
		}
		/// <summary>
		/// Implements IORMModelEventSubscriber.RemoveModelingEventHandlers
		/// </summary>
		protected void RemoveModelingEventHandlers(bool preLoadAdded, bool postLoadAdded, bool surveyHandlerAdded)
		{
			if (postLoadAdded)
			{
				NamedElementDictionary.DetachEventHandlers(Store);
			}
		}
		void IORMModelEventSubscriber.RemoveModelingEventHandlers(bool preLoadAdded, bool postLoadAdded, bool surveyHandlerAdded)
		{
			RemoveModelingEventHandlers(preLoadAdded, postLoadAdded, surveyHandlerAdded);
		}
		/// <summary>
		/// Implementes IORMModelEventSubscriber.SurveyQuestionLoad
		/// </summary>
		protected void SurveyQuestionLoad()
		{
			DomainDataDirectory directory = this.Store.DomainDataDirectory;
			EventManagerDirectory eventDirectory = this.Store.EventManagerDirectory;
			DomainClassInfo classInfo = directory.FindDomainRelationship(ModelHasObjectType.DomainClassId);

			eventDirectory.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementAdded));
			eventDirectory.ElementDeleted.Add(classInfo, new EventHandler<ElementDeletedEventArgs>(ModelElementRemoved));
			eventDirectory.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged));
			eventDirectory.ElementPropertyChanged.Add(directory.FindDomainClass(FactType.DomainClassId), new EventHandler<ElementPropertyChangedEventArgs>(FactTypeNameChanged));
		}
		void IORMModelEventSubscriber.SurveyQuestionLoad()
		{
			this.SurveyQuestionLoad();
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
		#region ISurveyNodeProvider Members
		IEnumerable<SampleDataElementNode> ISurveyNodeProvider.GetSurveyNodes()
		{
			return this.GetSurveyNodes();
		}
		/// <summary>
		/// Provides an <see cref="IEnumerable{SampleDataElementNode}"/> for the <see cref="SurveyTreeControl"/>.
		/// </summary>
		protected IEnumerable<SampleDataElementNode> GetSurveyNodes()
		{
			foreach (ModelElement element in this.Store.ElementDirectory.FindElements(FactType.DomainClassId, true))
			{
				yield return new SampleDataElementNode(element);
			}
			foreach (ModelElement element in this.Store.ElementDirectory.FindElements(ObjectType.DomainClassId, true))
			{
				yield return new SampleDataElementNode(element);
			}
		}
		#endregion
		#region SurveyEventHandling
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementAdded events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementAdded(object sender, ElementAddedEventArgs e)
		{	
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementAdded((object)element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementDeleted events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted((object)element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementChanged events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ISurveyQuestionTypeInfo[] effectedQuestions = (this as ISurveyQuestionProvider).GetSurveyQuestionTypeInfo();
				eventNotify.ElementChanged((object)element, effectedQuestions);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementPropertyChanged events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed((object)element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FactTypeNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify = (e.ModelElement.Store as IORMToolServices).NotifySurveyElementChanged;
			if (eventNotify != null)
			{
				//TODO: find a way to get the changed model element off of CustomModelEventArgs or the sender
				eventNotify.ElementRenamed(sender);
			}
		}
		#endregion //SurveyEventHandling
	}
}
