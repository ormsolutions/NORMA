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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMCore = ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	partial class TableIsPrimarilyForConceptType : IVerbalize
	{
		#region IVerbalize Implementation
		/// <summary>
		/// Implements <see cref="IVerbalize.GetVerbalization"/>
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			// We are redirected to this point by the associated Table element
			ConceptType conceptType = this.ConceptType;
			ObjectType objectType;
			if (null != (objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType)))
			{
				verbalizationContext.DeferVerbalization(objectType, DeferVerbalizationOptions.None, null);
				foreach (ConceptType alsoForConceptType in TableIsAlsoForConceptType.GetConceptType(this.Table))
				{
					if (null != (objectType = ConceptTypeIsForObjectType.GetObjectType(alsoForConceptType)))
					{
						writer.WriteLine();
						verbalizationContext.DeferVerbalization(objectType, DeferVerbalizationOptions.None, null);
					}
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
					verbalizationContext.DeferVerbalization(factType, DeferVerbalizationOptions.MultipleVerbalizations, null);
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
	partial class UniquenessConstraintIsForUniqueness : IVerbalize
	{
		#region IVerbalize Implementation
		/// <summary>
		/// Implements <see cref="IVerbalize.GetVerbalization"/>
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, bool isNegative)
		{
			ORMCore.UniquenessConstraint constraint = UniquenessIsForUniquenessConstraint.GetUniquenessConstraint(this.Uniqueness);
			if (constraint != null)
			{
				verbalizationContext.DeferVerbalization(constraint, DeferVerbalizationOptions.None, null);
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
