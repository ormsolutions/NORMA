using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using System.Diagnostics;
using System.Collections;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel
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
			/// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
			/// </summary>
			private static void InformationTypeFormatAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasInformationTypeFormat)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
			/// </summary>
			private static void InformationTypeFormatDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasInformationTypeFormat)e.ModelElement).Model);
			}
			private static void RebuildAbstractionModel(AbstractionModel model)
			{
				if (model != null &&
					!model.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(model, RebuildForAbstractionModelDelayed);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(UniquenessConstraintIsForUniqueness)
			/// Propagate deletion of a Uniqueness
			/// </summary>
			private static void UniquenessDeletedRule(ElementDeletedEventArgs e)
			{
				UniquenessConstraint constraint = ((UniquenessConstraintIsForUniqueness)e.ModelElement).UniquenessConstraint;
				if (!constraint.IsDeleted)
				{
					constraint.Delete();
				}
			}
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModel)
			/// Update the schema name when the abstraction model name changes
			/// </summary>
			private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == AbstractionModel.NameDomainPropertyId)
				{
					AbstractionModel abstractionModel = (AbstractionModel)e.ModelElement;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(abstractionModel);
					if (schema != null)
					{
						schema.Name = abstractionModel.Name;
					}
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void RebuildForAbstractionModelDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					AbstractionModel model = (AbstractionModel)element;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(model);
					if (schema != null)
					{
						schema.TableCollection.Clear();
						schema.DomainCollection.Clear();
						FullyGenerateConceptualDatabaseModel(schema, model);
					}
				}
			}
			#endregion // Abstraction model modification rules
		}
		#endregion // Regeneration rule delay validation methods
	}
}