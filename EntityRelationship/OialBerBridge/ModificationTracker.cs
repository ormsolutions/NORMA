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
using ORMSolutions.ORMArchitect.Framework;
using System.Diagnostics;
using System.Collections;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using System.Collections.ObjectModel;
using ORMCore = ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge
{
	partial class ORMAbstractionToBarkerERBridgeDomainModel
	{
		#region Regeneration rule delay validation methods
		private static partial class ModificationTracker
		{
			#region Abstraction model modification rules
			// For now, the abstraction model is fully regenerated whenever
			// an potential mapping change occurs. If there were any
			// concept types in the model they will be deleted and regenerated,
			// so it is currently sufficient to listen to a limited number of changes
			// and fully regenerate below the schema. The added rules are only needed
			// for cases where we start from an empty ORM model

			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptTypeChild)
			/// </summary>
			private static void ConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
			{
				//if (e.DomainProperty.Id == ConceptTypeChild.IsMandatoryDomainPropertyId)
				//{
				//    ValidateAssociatedColumnsIsNullable((ConceptTypeChild)e.ModelElement);
				//}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			
			private static void RebuildAbstractionModel(AbstractionModel model)
			{
				if (model != null &&
					!model.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(model, RebuildForAbstractionModelDelayed);
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void RebuildForAbstractionModelDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					AbstractionModel model = (AbstractionModel)element;
					BarkerErModel barkerModel = BarkerErModelIsForAbstractionModel.GetBarkerErModel(model);
					if (barkerModel != null)
					{
						barkerModel.BinaryAssociationCollection.Clear();
						barkerModel.EntityTypeCollection.Clear();
						barkerModel.ExclusiveArcCollection.Clear();

						FullyGenerateBarkerERModel(barkerModel, model, null);
					}
				}
			}
			#endregion // Abstraction model modification rules
			#region Bridge element modification rules
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModel)
			/// Update the schema name when the abstraction model name changes
			/// </summary>
			private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == AbstractionModel.NameDomainPropertyId)
				{
					//AbstractionModel abstractionModel = (AbstractionModel)e.ModelElement;
					//Schema schema = SchemaIsForAbstractionModel.GetSchema(abstractionModel);
					//if (schema != null)
					//{
					//    schema.Name = abstractionModel.Name;
					//}
				}
			}
			#endregion // Bridge element modification rules
		}
		#endregion // Regeneration rule delay validation methods
	}
}