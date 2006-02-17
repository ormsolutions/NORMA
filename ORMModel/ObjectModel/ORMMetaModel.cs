using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// A delegate callback to use with the <see cref="ORMMetaModel.DelayValidateElement"/> method.
	/// The delegate will be called when the current transaction finishes committing.
	/// </summary>
	/// <param name="element">The element to validate</param>
	[CLSCompliant(true)]
	public delegate void ElementValidator(ModelElement element);
	public partial class ORMMetaModel
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
			IDictionary contextDictionary = store.TransactionManager.CurrentTransaction.Context.ContextInfo;
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
	}
}
