using Microsoft.VisualStudio.Modeling;
using org.unibz.ucmf.askAPI;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System;
using System.Collections.Generic;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		private HashSet<ObjectType> alreadyProcessedDisjointTypes = new HashSet<ObjectType>();

		void CreateInferredDisjointTypes(InferredConstraints container, InferredDisjointEntityTypes disjTypes)
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
				inferredHierarchyContainer.DisjointTopObjectTypeCollection.Clear();
			}


			alreadyProcessedDisjointTypes.Clear();

			foreach (ObjectType type in model.ObjectTypeCollection)
			{
				//prendo la lista dei figli di type in formato stringa
				java.util.ArrayList listDisjointFrom = disjTypes.gellAllTypeDisjointFrom(type.Name);
				//se questa lista è diversa da null e il type non e' gia stato processato, allora ci sono degli equivalenti
				if (listDisjointFrom != null && alreadyProcessedEquivalentTypes.Contains(type) == false)
				{

					//setto type
					DisjointTopLevelObjectType top = new DisjointTopLevelObjectType(inferredHierarchyContainer, type);

					//per ciascun figlio
					foreach (String son in listDisjointFrom)
					{
						ObjectType subObjectType = findObjectTypeByName(model, son);
						alreadyProcessedDisjointTypes.Add(subObjectType);
						new DisjointObjectTypeContainment(top, subObjectType);

					}
				}

			}
		}
	}
}