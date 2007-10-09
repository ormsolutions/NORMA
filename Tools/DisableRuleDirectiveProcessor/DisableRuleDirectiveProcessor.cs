#region Public Domain Notice
/**************************************************************************\
* Disable Rule Directive Processor for the Neumont Modeling Framework      *
*                                                                          *
* This file is hereby released into the public domain by its author. This  *
* applies worldwide. For countries in which this is not legally possible,  *
* the author grants anyone the right to use this work for any purpose,     *
* without any conditions, unless such conditions are required by law.      *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TextTemplating;

namespace Neumont.Tools.Modeling
{
	public class DisableRuleDirectiveProcessor : RequiresProvidesDirectiveProcessor
	{
		public const string DisableRuleDirectiveProcessorName = "DisableRuleDirectiveProcessor";
		public const string DisableRuleDirectiveName = "DisableRule";

		protected override string FriendlyName
		{
			get
			{
				return DisableRuleDirectiveProcessorName;
			}
		}

		public override bool IsDirectiveSupported(string directiveName)
		{
			return String.Equals(directiveName, DisableRuleDirectiveName, StringComparison.InvariantCultureIgnoreCase);
		}

		protected override void InitializeProvidesDictionary(string directiveName, IDictionary<string, string> providesDictionary)
		{
			// Do nothing.
		}

		protected override void GeneratePreInitializationCode(string directiveName, System.Text.StringBuilder codeBuffer, System.CodeDom.Compiler.CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments)
		{
			// Do nothing.
		}

		protected override void InitializeRequiresDictionary(string directiveName, IDictionary<string, string> requiresDictionary)
		{
			if (IsDirectiveSupported(directiveName))
			{
				requiresDictionary["RuleName"] = null;
			}
		}

		protected override void GeneratePostInitializationCode(string directiveName, System.Text.StringBuilder codeBuffer, System.CodeDom.Compiler.CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments)
		{
			if (IsDirectiveSupported(directiveName))
			{
				string ruleName = requiresArguments["RuleName"];

				if (String.IsNullOrEmpty(ruleName))
				{
					throw new ArgumentNullException("ruleName");
				}
				codeBuffer.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{{ System.Type ruleType = System.Type.GetType(\"{0}\");", ruleName);
				codeBuffer.AppendLine("if (ruleType != null) { this.Store.RuleManager.DisableRule(ruleType); } }");
			}
		}

		protected override void GenerateTransformCode(string directiveName, System.Text.StringBuilder codeBuffer, System.CodeDom.Compiler.CodeDomProvider languageProvider, IDictionary<string, string> requiresArguments, IDictionary<string, string> providesArguments)
		{
			// Do nothing.
		}
	}
}
