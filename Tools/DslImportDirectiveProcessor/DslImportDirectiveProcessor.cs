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
using Microsoft.VisualStudio.Modeling.DslDefinition;
using Microsoft.VisualStudio.TextTemplating;

namespace Neumont.Tools.ORM.Framework
{
	public class DslImportDirectiveProcessor : DslDirectiveProcessorBase
	{
		public const string DslImportDirectiveProcessorName = "DslImportDirectiveProcessor";

		protected override string FriendlyName
		{
			get
			{
				return DslImportDirectiveProcessorName;
			}
		}

		public override void StartProcessingRun(System.CodeDom.Compiler.CodeDomProvider languageProvider, string templateContents, System.CodeDom.Compiler.CompilerErrorCollection errors)
		{
			base.StartProcessingRun(languageProvider, templateContents, errors);

			Dictionary<string, string> requiresArguments = new Dictionary<string, string>(1, StringComparer.OrdinalIgnoreCase);
			requiresArguments.Add("FileName", "Dummy");
			Dictionary<string, string> providesArguments = new Dictionary<string, string>(1, StringComparer.OrdinalIgnoreCase);
			this.InitializeProvidesDictionary("Dsl", providesArguments);

			// Call base.GenerateTransformCode so that oneTimeCodeGenerated gets set to true.
			base.GenerateTransformCode("Dsl", new System.Text.StringBuilder(), languageProvider, requiresArguments, providesArguments);
		}

		protected override void InitializeProvidesDictionary(string directiveName, System.Collections.Generic.IDictionary<string, string> providesDictionary)
		{
			if (string.Equals(directiveName, "Dsl", StringComparison.InvariantCultureIgnoreCase))
			{
				providesDictionary["DslLibrary"] = "ImportedDslLibrary";
			}
		}

		protected override void GeneratePreInitializationCode(string directiveName, System.Text.StringBuilder codeBuffer, System.CodeDom.Compiler.CodeDomProvider languageProvider, System.Collections.Generic.IDictionary<string, string> requiresArguments, System.Collections.Generic.IDictionary<string, string> providesArguments)
		{
			// Do nothing, and don't call the base implementation.
			// This prevents the calls to AddDomainModel from being generated.
		}

		public override string GetPostInitializationCodeForProcessingRun()
		{
			return "using (Microsoft.VisualStudio.Modeling.Transaction outterDslImportTransaction = this.Store.TransactionManager.BeginTransaction(\"Load Files\", true)) { " + base.GetPostInitializationCodeForProcessingRun();
		}
	}

	public class DslImportEndDirectiveProcessor : DirectiveProcessor
	{
		public override string GetPostInitializationCodeForProcessingRun()
		{
			return " outterDslImportTransaction.Commit(); }";
		}

		#region Does nothing
		public override void FinishProcessingRun()
		{
			// Do nothing
		}
		public override string GetClassCodeForProcessingRun()
		{
			return string.Empty;
		}
		public override string[] GetImportsForProcessingRun()
		{
			return new string[0];
		}
		public override string GetPreInitializationCodeForProcessingRun()
		{
			return string.Empty;
		}
		public override string[] GetReferencesForProcessingRun()
		{
			return new string[0];
		}
		public override bool IsDirectiveSupported(string directiveName)
		{
			return string.Equals(directiveName, "Dsl", StringComparison.InvariantCultureIgnoreCase);
		}
		public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
		{
			// Do nothing
		}
		#endregion
	}
}
