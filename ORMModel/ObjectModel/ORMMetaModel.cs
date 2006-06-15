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
using Neumont.Tools.ORM.Framework;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A delegate callback to use with the <see cref="ORMMetaModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The element to validate</param>
	[CLSCompliant(true)]
	public delegate void ElementValidator(ModelElement element);
	public partial class ORMMetaModel : IORMModelEventSubscriber, IVerbalizationSnippetsProvider, ISurveyNodeProvider
	{
		#region InitializingToolboxItems property
		private static bool myReflectRulesSuspended;
		/// <summary>
		/// Static property to disable rule reflection for fast
		/// model load. This needs to be on a meta model, not the
		/// package, so that the models also load from the command line.
		/// </summary>
		public static bool InitializingToolboxItems
		{
			get
			{
				return !myReflectRulesSuspended;
			}
			set
			{
				Debug.Assert(value || !myReflectRulesSuspended, "InitializingToolboxItems already turned off");
				myReflectRulesSuspended = !value;
			}
		}
		#endregion // InitializingToolboxItems property
		#region Delayed Model Validation
		private const string DelayedValidationContextKey = "{F8B7BB89-78A2-4F53-8C55-FC69B8A0FEF3}";
		/// <summary>
		/// Class to delay validate rules when a transaction is committing.
		/// </summary>
		[RuleOn(typeof(ORMMetaModel), FireTime = TimeToFire.LocalCommit)]
		private class DelayValidateElements : TransactionCommittingRule
		{
			public override void TransactionCommitting(TransactionCommitEventArgs e)
			{
				IDictionary contextInfo = e.Transaction.Context.ContextInfo;
				while (contextInfo.Contains(DelayedValidationContextKey)) // Let's do a while in case a new validator is added by the other validators
				{
					Dictionary<ElementValidatorKey, object> validators = (Dictionary<ElementValidatorKey, object>)contextInfo[DelayedValidationContextKey];
					contextInfo.Remove(DelayedValidationContextKey);
					foreach (ElementValidatorKey key in validators.Keys)
					{
						key.Validator(key.Element);
					}
				}
			}
		}
		/// <summary>
		/// A structure to use as a key for tracking element validation
		/// </summary>
		private struct ElementValidatorKey
		{
			/// <summary>
			/// Create a new key
			/// </summary>
			public ElementValidatorKey(ModelElement element, ElementValidator validator)
			{
				Element = element;
				Validator = validator;
			}
			/// <summary>
			/// The element to validate
			/// </summary>
			public ModelElement Element;
			/// <summary>
			/// The callback valdiation function
			/// </summary>
			public ElementValidator Validator;
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
			IDictionary contextDictionary = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			Dictionary<ElementValidatorKey, object> dictionary = null;
			ElementValidatorKey key = new ElementValidatorKey(element, validator);
			if (contextDictionary.Contains(DelayedValidationContextKey))
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
			MetaDataDirectory directory = this.Store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = this.Store.EventManagerDirectory;
			MetaClassInfo classInfo = directory.FindMetaRelationship(ModelHasObjectType.MetaRelationshipGuid);

			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ModelElementAdded));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ModelElementRemoved));
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ModelElementNameChanged));
			eventDirectory.CustomModelEventManager.Add(FactTypeNameChangedEvent.CustomModelEventId, new CustomModelEventHandler(FactTypeNameChanged));
		}
		void IORMModelEventSubscriber.SurveyQuestionLoad()
		{
			this.SurveyQuestionLoad();
		}
		#endregion // IORMModelEventSubscriber Implementation
		#region IVerbalizationSnippetsProvider Implementation
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
		#endregion // IVerbalizationSnippetsProvider Implementation
		#region ISurveyNodeProvider Members
		IEnumerable<SampleDataElementNode> ISurveyNodeProvider.GetSurveyNodes()
		{
			return this.GetSurveyNodes();
		}
		/// <summary>
		/// provides an IEnumerable of sampleDataElementNodes for the SurveyTree
		/// </summary>
		/// <returns></returns>
		protected IEnumerable<SampleDataElementNode> GetSurveyNodes()
		{
			Guid[] elementTypes = { FactType.MetaClassGuid, SubtypeFact.MetaClassGuid, ObjectType.MetaClassGuid };
			for (int i = 0; i < elementTypes.Length; ++i)
			{
				IList tempElements = this.Store.ElementDirectory.GetElements(elementTypes[i], true);
				for (int j = 0; j < tempElements.Count; ++j)
				{
					SampleDataElementNode currentElement = new SampleDataElementNode(tempElements[j]);
					yield return currentElement;
				}
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
		/// wired on SurveyQuestionLoad as event handler for ElementRemoved events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementRemoved(object sender, ElementRemovedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRemoved((object)element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementChanged events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementChanged(object sender, ElementAttributeChangedEventArgs e)
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
		/// wired on SurveyQuestionLoad as event handler for ElementAttributeChanged events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ModelElementNameChanged(object sender, ElementAttributeChangedEventArgs e)
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
		protected void FactTypeNameChanged(object sender, CustomModelEventArgs e)
		{
			INotifySurveyElementChanged eventNotify = (e.Store as IORMToolServices).NotifySurveyElementChanged;
			if (eventNotify != null)
			{
				//TODO: find a way to get the changed model element off of CustomModelEventArgs or the sender
				eventNotify.ElementRenamed(sender);
			}
		}
		#endregion //SurveyEventHandling
	}
}
