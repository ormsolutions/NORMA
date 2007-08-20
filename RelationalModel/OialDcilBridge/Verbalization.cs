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
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	partial class ColumnHasConceptTypeChild : IVerbalize
	{
		#region IVerbalize Implementation
		/// <summary>
		/// Implements <see cref="IVerbalize.GetVerbalization"/>
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			// We are redirected to this point by the associated Column element
			Column column = this.Column;
			bool firstWrite = true;
			foreach (ConceptTypeChild child in ColumnHasConceptTypeChild.GetConceptTypeChildPath(this.Column))
			{
				foreach (FactType factType in ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(child))
				{
					if (firstWrite)
					{
						firstWrite = false;
					}
					else
					{
						writer.WriteLine();
					}
					verbalizationContext.DeferVerbalization(factType, null);
				}
			}
			return false;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, isNegative);
		}
		#endregion // IVerbalize Implementation
	}
}
