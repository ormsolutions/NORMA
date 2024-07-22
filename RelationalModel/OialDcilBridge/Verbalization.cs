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
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
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
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
		#endregion // IVerbalize Implementation
	}
	partial class ColumnHasConceptTypeChild : IVerbalize
	{
		#region IVerbalize Implementation
		/// <summary>
		/// Implements <see cref="IVerbalize.GetVerbalization"/>
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			// We are redirected to this point by the associated Column element
			Column column = this.Column;
			bool firstWrite = true;
			foreach (ColumnHasConceptTypeChild childLink in ColumnHasConceptTypeChild.GetLinksToConceptTypeChildPath(this.Column))
			{
				ConceptTypeChild child = childLink.ConceptTypeChild;
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

					FactType renderFactType = factType;
					if (null != childLink.InverseConceptTypeChild && factType.UnaryPattern == UnaryValuePattern.Negation)
					{
						// If an inverse is available, then the name generation will always use the positive
						// form even if the negative form is in the path. This happens, for example, if the
						// positive fact type is objectified and the negation is not.
						renderFactType = renderFactType.PositiveUnaryFactType ?? renderFactType;
					}
					verbalizationContext.DeferVerbalization(renderFactType, DeferVerbalizationOptions.MultipleVerbalizations, null);
				}
			}
			return false;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
		#endregion // IVerbalize Implementation
	}
	partial class UniquenessConstraintIsForUniqueness : IVerbalize
	{
		#region IVerbalize Implementation
		/// <summary>
		/// Implements <see cref="IVerbalize.GetVerbalization"/>
		/// </summary>
		protected bool GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			ORMCore.UniquenessConstraint constraint = UniquenessIsForUniquenessConstraint.GetUniquenessConstraint(this.Uniqueness);
			if (constraint != null)
			{
				verbalizationContext.DeferVerbalization(constraint, DeferVerbalizationOptions.None, null);
			}
			return false;
		}
		bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			return GetVerbalization(writer, snippetsDictionary, verbalizationContext, sign);
		}
		#endregion // IVerbalize Implementation
	}
}
