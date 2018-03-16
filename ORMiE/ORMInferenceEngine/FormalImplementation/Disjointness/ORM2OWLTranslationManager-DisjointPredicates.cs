using Microsoft.VisualStudio.Modeling;
using org.unibz.ucmf.askAPI;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System;
using System.Collections.Generic;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		private HashSet<FactType> alreadyProcessedDisjointPreds = new HashSet<FactType>();

		void CreateInferredDisjointPreds(InferredConstraints container, InferredDisjointPredicates disjTypes)
		{
			ORMModel model = container.Model;
			Store store = model.Store;
			Partition hierarchyPartition = Partition.FindByAlternateId(store, typeof(Hierarchy));

			if (hierarchyPartition == null)
			{
				hierarchyPartition = new Partition(store);
				hierarchyPartition.AlternateId = typeof(Hierarchy);
			}

			InferredHierarchy inferredHierarchyContainer = InferredHierarchyIsForORMModel.GetInferredHierarchy(model);
			if (inferredHierarchyContainer == null)
			{
				inferredHierarchyContainer = new InferredHierarchy(hierarchyPartition);
				inferredHierarchyContainer.Model = model;
			}
			else
			{
				inferredHierarchyContainer.DisjointTopFactTypeCollection.Clear();
			}


			alreadyProcessedDisjointTypes.Clear();

			foreach (FactType fact in model.FactTypeCollection)
			{
				//prendo la lista dei figli di type in formato stringa
				java.util.ArrayList listDisjointFrom = disjTypes.getAllPredsDisjointTo(fact.Name);
				//se questa lista è diversa da null e il type non e' gia stato processato, allora ci sono degli equivalenti
				if (listDisjointFrom != null && alreadyProcessedDisjointPreds.Contains(fact) == false)
				{

					//setto type
					DisjointTopLevelFactType top = new DisjointTopLevelFactType(inferredHierarchyContainer, fact);

					//per ciascun figlio
					foreach (String item in listDisjointFrom)
					{
						FactType subFactType = findFactTypeByName(model, item);
						alreadyProcessedDisjointPreds.Add(subFactType);
						new DisjointFactTypeContainment(top, subFactType);

					}
				}

			}
		}
	}
}