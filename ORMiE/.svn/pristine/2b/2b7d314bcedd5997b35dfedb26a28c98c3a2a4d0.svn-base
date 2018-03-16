using System;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using org.semanticweb.owlapi.model;
using org.semanticweb.owlapi.reasoner;
using uk.ac.manchester.cs.jfact;
using System.Collections.Generic;
using org.unibz.ucmf.askAPI;
using System.Collections;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{

		//private HashSet<FactType> alreadyProcessedEquivalentPreds = new HashSet<FactType>();

		void CreateInferredEquivalentPredicatesOld(InferredConstraints container, InferredEquivalentPredicates equiPreds)
		{
			Partition outputPartition = container.Partition;
			ORMModel model = container.Model;

			//prendo la lista di tutti gli elementi di tutti i set
			java.util.ArrayList listEquiv = equiPreds.getAllPreds();

			//per ogni elemento vedo se ha equivalenti

			if (listEquiv != null)
			{

			foreach (String item in listEquiv)
			{
				java.util.ArrayList listEquivToPred = equiPreds.getAllPredsEquivalentTo(item);
				if (listEquivToPred != null)
				{
					//setto fact a padre
					//EquivalentTopLevelFactType top = new EquivalentTopLevelFactType(hierarchyContainer, fact);
					//lo metto in quelli gia processati
					//alreadyProcessedEquivalentPreds.Add(fact);
					String top = item;

					//per ciascun figlio
					foreach (String son in listEquivToPred)
					{
						//recupero facttype  dal model
						//FactType subFactType = findFactTypeByName(model, son);
						//new EquivalentFactTypeContainment(top, subFactType);
						//lo metto in quelli gia processati
						//alreadyProcessedEquivalentPreds.Add(subFactType);

						SetComparisonConstraint eq = new InferredEqualityConstraint(outputPartition);
						eq.Name = "Equivalent Predicates: " + mandOrUniqForORM(top) + " = " + mandOrUniqForORM(son);

					}
					//elimino il set che ho appena processato, per non avere cicli!
					equiPreds.removeAllPredsEquivalentTo(item);
				}

			}
		}

			/*
				foreach (FactType fact in model.FactTypeCollection)
			{
				//prendo la lista dei figli di fact in formato stringa
				java.util.ArrayList listEquivToPred = equiPreds.getAllPredsEquivalentTo(fact.Name);

				//se questa lista è diversa da null e il fact non e' gia stato processato, allora ci sono degli equivalenti

				if(!(fact.GetType() == typeof(SubtypeFact)))
				{

				if(listEquivToPred !=null )
				{
						//setto fact a padre
						//EquivalentTopLevelFactType top = new EquivalentTopLevelFactType(hierarchyContainer, fact);
						//lo metto in quelli gia processati
						//alreadyProcessedEquivalentPreds.Add(fact);
						String top = fact.Name;
				
					//per ciascun figlio
					foreach (String son in listEquivToPred)
					{
						//recupero facttype  dal model
						FactType subFactType = findFactTypeByName(model, son);
							//new EquivalentFactTypeContainment(top, subFactType);
							//lo metto in quelli gia processati
							//alreadyProcessedEquivalentPreds.Add(subFactType);

							SetComparisonConstraint eq = new InferredEqualityConstraint(outputPartition);
							eq.Name = "EquivPreds: " + top + son;

						}
					//elimino il set che ho appena processato, per non avere cicli!
					equiPreds.removeAllPredsEquivalentTo(fact.Name);
				}
				else
				{
					// e' inutile far vedere tra gli equivalent chi non ha figli
					//EquivalentTopLevelFactType top = new EquivalentTopLevelFactType(hierarchyContainer, fact);
				}
				}
			}
			*/



		}
	}
}