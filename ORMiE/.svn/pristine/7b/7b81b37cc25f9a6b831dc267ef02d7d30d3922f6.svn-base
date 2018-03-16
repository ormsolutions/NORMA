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
		private HashSet<string> alreadyProcessedFactTypes2 = new HashSet<string>();

		void CreateInferredPredicatesHierarchyOld(InferredConstraints container, InferredHierarchyPredicates hierarchyPreds)
		{
			ORMModel model = container.Model;
			Store store = model.Store;
			Partition outputPartition = container.Partition;

			

			alreadyProcessedFactTypes2.Clear();

			java.util.ArrayList fathers = hierarchyPreds.getFathers();

			//For each father
			foreach (String father in fathers)
			{

				//Get the ObjectType of the father
				//FactType factType = findFactTypeByName(model, cleanPredicateNameForORM(father));
				//se il facttype e' diverso da null e il factTypeName non e' gia' stato processato ed e' una radice
				//if(factType !=null && alreadyProcessedFactTypes2.Contains(factType.Name)==false /*&& father.Contains("1, 2")*/ )
				{

					//lo aggiungo a quelli processati
					//alreadyProcessedFactTypes2.Add(factType.Name);

					//Get him to the top level
					//TopLevelFactType topLevelFactType = new TopLevelFactType(hierarchyContainer, factType);
					if (hierarchyPreds.getSons(father).size() > 0)
					{
						FactType superFactType = findFactTypeByName(model, cleanPredicateNameForORM(father));
						foreach (String son in hierarchyPreds.getSons(father))
						{
							FactType subFactType = findFactTypeByName(model, cleanPredicateNameForORM(son));
							SetComparisonConstraint subset = new InferredSubsetConstraint(outputPartition);
							subset.Name = mandOrUniqForORM(son) + " \u2286 " + mandOrUniqForORM(father);
						}
					}


					//Dmitri code for good subset
					/*
					{
						// The only possible way is when the arity is equal
						// In ORMzero we have only BINARY! 
						String rel1str = subclass.Substring(4); // 4 - index of element after "obj_"
						String rel2str = superclass.Substring(4); // 4 - index of element after "obj_"

						FactType factType1 = findFactTypeByName(model, rel1str);
						FactType factType2 = findFactTypeByName(model, rel2str);
						if (null == factType1 || null == factType2)
							continue;
						if (factType1.RoleCollection.Count != factType2.RoleCollection.Count
							&& factType1.RoleCollection.Count == 2)
							continue;

						SetComparisonConstraint targetConstraint = new InferredSubsetConstraint(outputPartition);
						new SetComparisonConstraintIsInferred(container, targetConstraint);

						SetComparisonConstraintRoleSequence roleseq1 = new SetComparisonConstraintRoleSequence(outputPartition);
						SetComparisonConstraintRoleSequence roleseq2 = new SetComparisonConstraintRoleSequence(outputPartition);

						roleseq1.RoleCollection.Add(factType1.RoleCollection.ToArray()[0].Role);
						roleseq1.RoleCollection.Add(factType1.RoleCollection.ToArray()[1].Role);

						roleseq2.RoleCollection.Add(factType2.RoleCollection.ToArray()[0].Role);
						roleseq2.RoleCollection.Add(factType2.RoleCollection.ToArray()[1].Role);

						targetConstraint.RoleSequenceCollection.Add(roleseq1);
						targetConstraint.RoleSequenceCollection.Add(roleseq2);
					}
					*/

				}
			}
		}
	}
}