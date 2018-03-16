using Microsoft.VisualStudio.Modeling;
using org.unibz.ucmf.askAPI;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System;
using System.Collections.Generic;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		private HashSet<ObjectType> alreadyProcessedExhaustiveTypes = new HashSet<ObjectType>();

		void CreateInferredExhaustiveTypes(InferredConstraints container, InferredExhaustiveTypes exTypes)
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
				inferredHierarchyContainer.ExhaustiveTopObjectTypeCollection.Clear();
			}

			alreadyProcessedExhaustiveTypes.Clear();

			foreach (ObjectType type in model.ObjectTypeCollection)
			{
				//prendo la lista dei figli di type in formato stringa
				java.util.ArrayList listCoveringToType = exTypes.getAllTypesCoveringTo(type.Name);
				//se questa lista è diversa da null e il type non e' gia stato processato, allora ci sono degli equivalenti
				if (listCoveringToType != null && alreadyProcessedEquivalentTypes.Contains(type) == false)
				{

					//setto il padre
					ExhaustiveTopLevelObjectType top = new ExhaustiveTopLevelObjectType(inferredHierarchyContainer, type);

					//per ciascun figlio
					foreach (String son in listCoveringToType)
					{
						ObjectType subObjectType = findObjectTypeByName(model, son);
						//alreadyProcessedEquivalentTypes.Add(subObjectType);
						new ExhaustiveObjectTypeContainment(top, subObjectType);

					}
				}

			}

		}
	}
}