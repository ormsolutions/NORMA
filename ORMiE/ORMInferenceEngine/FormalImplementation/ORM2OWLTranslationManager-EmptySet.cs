using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using org.unibz.ucmf.askAPI;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{

		HashSet<string> unsatObjectTypes = new HashSet<String>();

		void CreateInferredEmptySet(InferredConstraints container, InferredEmptySet emptySet)
		{

			ORMModel model = container.Model;
			Store store = model.Store;

			Partition unsatPartition = Partition.FindByAlternateId(store, typeof(UnsatisfiableDomain));

			if (unsatPartition == null)
			{
				unsatPartition = new Partition(store);
				unsatPartition.AlternateId = typeof(UnsatisfiableDomain);
			}

			InferredUnsatisfiableDomain unsatContainer = InferredUnsatisfiableDomainIsForORMModel.GetInferredUnsatisfiableDomain(model);
			if (unsatContainer == null)
			{
				unsatContainer = new InferredUnsatisfiableDomain(unsatPartition);
				unsatContainer.Model = model;
			}
			else
			{
				unsatContainer.ObjectTypeCollection.Clear();
				unsatContainer.FactTypeCollection.Clear();
			}

			foreach (String name in emptySet.getEmptySet())
			{

				ObjectType objectType = findObjectTypeByName(model, name);
				if (null != objectType)
				{
					unsatObjectTypes.Add(objectType.Name);
					new UnsatisfiableObjectType(unsatContainer, objectType);
					continue;
				}


				FactType factType = findFactTypeByName(model, cleanPredicateNameForORM(name));
				if (null != factType)
				{
					new UnsatisfiableFactType(unsatContainer, factType);
				}
			}
		}

		private String cleanPredicateNameForORM(String input)
		{
			int index = input.IndexOf("{");
			if (index > 0)
			{
				input = input.Substring(0, index);
			}
				
			input = input.Remove(input.Length - 1);
			return input;
		}
	}
}