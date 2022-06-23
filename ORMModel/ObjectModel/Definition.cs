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
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class Definition
	{
		#region DefinitionChangedRule
		/// <summary>
		/// ChangeRule: typeof(Definition)
		/// Any text changes after creation than result in empty text should delete the definition
		/// </summary>
		private static void DefinitionChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == Definition.TextDomainPropertyId && string.IsNullOrEmpty(e.NewValue as string))
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateRequireDefinitionText);
			}
		}
		private static void DelayValidateRequireDefinitionText(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				Definition definition = (Definition)element;
				if (string.IsNullOrEmpty(definition.Text))
				{
					definition.Delete();
				}
			}
		}
		#endregion // DefinitionChangedRule
	}
}
