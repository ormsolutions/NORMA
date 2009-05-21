#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	partial class CustomProperty : IORMPropertyExtension
	{
		#region IORMPropertyExtension Members
		ORMExtensionPropertySettings IORMPropertyExtension.ExtensionPropertySettings
		{
			get
			{
				return ORMExtensionPropertySettings.NotDisplayed;
			}
		}
		Guid IORMPropertyExtension.ExtensionExpandableTopLevelPropertyId
		{
			get
			{
				return Guid.Empty;
			}
		}
		#endregion
	}
	partial class CustomProperty : IVerbalize
	{
		#region IVerbalize Members
		bool IVerbalize.GetVerbalization(System.IO.TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
			IVerbalizationSets<CustomPropertyVerbalizationSnippetType> snippets = (IVerbalizationSets<CustomPropertyVerbalizationSnippetType>)snippetsDictionary[typeof(CustomPropertyVerbalizationSnippetType)];
			CustomPropertyDefinition defn = CustomPropertyDefinition;
			writer.Write(string.Format(snippets.GetSnippet(CustomPropertyVerbalizationSnippetType.CustomPropertiesVerbalization), defn.Name, Value.ToString(), defn.CustomPropertyGroup.Name, defn.Description));
			return true;
		}
		#endregion
	}
}
