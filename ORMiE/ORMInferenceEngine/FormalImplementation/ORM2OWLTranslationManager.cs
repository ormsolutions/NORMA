using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

using org.unibz.ucmf.tellAPI.constraints;
using org.unibz.ucmf.tellAPI.signatures;
using org.unibz.ucmf.manager;
using org.unibz.ucmf.askAPI;

using ConstraintType = ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintType;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{


		private static ObjectType findObjectTypeByName(ORMModel model, String name)
		{
			//REDO: use map to connect ontology names and GUIDS and then
			//model.Partition.ElementDirectory.FindElement(GUID)
			String restoredName = restoreForORM(name);
			return model.ObjectTypeCollection.Find(o => o.Name.Equals(restoredName));
		}

		private static FactType findFactTypeByName(ORMModel model, String name)
		{
			//REDO: use map to connect ontology names and GUIDS and then
			//model.Partition.ElementDirectory.FindElement(GUID)
			String restoredName = restoreForORM(name);
			return model.FactTypeCollection.Find(f => f.Name.Equals(restoredName));
		}


		#region OWL output manager

		/*

		private void writeAxiom(OWLAxiom axiom)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileOWL = @"\OrmieOntology.txt";
            using (StreamWriter sw = System.IO.File.AppendText(@desktopPath + fileOWL))
            {
                sw.WriteLine(axiom.ToString());
            }
        }
		*/

		#endregion OWL output manager

		public static String prepareForOWL(String str)
		{
			String result = str.Replace("#", "+");
			result = result.Replace(" ", "_");
			return result;
		}
		public static String restoreForORM(String str)
		{
			String result = str.Replace("+", "#");
			return result;
		}
		public static int roleIndex(Role role)
		{
			int index = 0;
			index = role.FactType.OrderedRoleCollection.IndexOf(role);
			return index + 1;
		}
		private int maxFactTypeArity(ORMModel model)
		{
			int max = 2;
			foreach (FactType elem in model.FactTypeCollection)
			{
				if (elem.RoleCollection.Count > max)
					max = elem.RoleCollection.Count;
			}
			return max;
		}


		/// <summary>
		/// Helper method to return the roles for a constraint if the constraint has
		/// no structural errors, no join path (temporary), and does not constraint any
		/// ignored fact types or object types.
		/// </summary>
		/// <param name="constraint"><see cref="SetConstraint"/> to verify.</param>
		/// <param name="ignoredFactTypesAndObjectTypes">Dictionary of elements (fact types and object types) that are ignored.</param>
		/// <returns></returns>
		private static LinkedElementCollection<Role> ReturnRolesIfValidConstraint(SetConstraint constraint, IDictionary<ORMModelElement, bool> ignoredFactTypesAndObjectTypes)
		{
			if (constraint.TooFewRoleSequencesError != null || constraint.TooManyRoleSequencesError != null || constraint.CompatibleRolePlayerTypeError != null ||
				constraint.JoinPath != null || constraint.JoinPathRequiredError != null) // UNDONE: Handle reasoning over join paths
			{
				return null;
			}
			LinkedElementCollection<Role> roles = constraint.RoleCollection;
			foreach (Role role in roles)
			{
				ObjectType rolePlayer;
				if (null != (rolePlayer = role.RolePlayer) && ignoredFactTypesAndObjectTypes.ContainsKey(rolePlayer) || ignoredFactTypesAndObjectTypes.ContainsKey(role.FactType))
				{
					return null;
				}
			}
			return roles;
		}
		/// <summary>
		/// Helper function to determine if all fact types in a collection are subtype facts and recognized
		/// </summary>
		private static bool AreAllFactTypesAreRecognizedSubtypes(LinkedElementCollection<FactType> factTypes, IDictionary<ORMModelElement, bool> recognizedFactTypes)
		{
			foreach (FactType factType in factTypes)
			{
				if (!(factType is SubtypeFact && recognizedFactTypes.ContainsKey(factType)))
				{
					return false;
				}
			}
			return true;
		}

		public void translateToOWL(ORMModel model)
		{
			if (null != model)
			{

				//Create the ORM Model by custom API
				org.unibz.ucmf.ORMModel ormModel = new org.unibz.ucmf.ORMModel();

				Partition ormPartition = model.Partition;
				Dictionary<FactType, ObjectType> refModeFactTypes = new Dictionary<FactType, ObjectType>();
				Dictionary<ObjectType, int> refModeValueTypes = new Dictionary<ObjectType, int>();
				Dictionary<ORMModelElement, bool> ignoredElements = new Dictionary<ORMModelElement, bool>();

				foreach (EntityTypeHasPreferredIdentifier pidLink in ormPartition.ElementDirectory.FindElements<EntityTypeHasPreferredIdentifier>())
				{
					LinkedElementCollection<Role> pidRoles = pidLink.PreferredIdentifier.RoleCollection;
					ObjectType rolePlayer;
					Role role;
					if (pidRoles.Count == 1 &&
						null != (rolePlayer = (role = pidRoles[0]).RolePlayer) &&
						rolePlayer.DataType != null) // Value type
					{
						// This is a reference mode pattern. Most of the fact types and value types will be
						// ignored, but we don't know for sure.
						refModeFactTypes[role.FactType] = rolePlayer;
						int useCount;
						refModeValueTypes[rolePlayer] = refModeValueTypes.TryGetValue(rolePlayer, out useCount) ? (useCount + 1) : 1;
						ignoredElements[rolePlayer] = true; // Assume ignored, pull out later if false so we're not modifying a collection currently being iterated.
					}
				}

				// Look at the value types. If the count is the role collection count then it is used only as a reference mode value type
				foreach (KeyValuePair<ObjectType, int> kvp in refModeValueTypes)
				{
					if (kvp.Value != kvp.Key.PlayedRoleCollection.Count && ignoredElements.ContainsKey(kvp.Key))
					{
						ignoredElements.Remove(kvp.Key);
					}
				}

				// Go through the reference mode fact types and make sure we can eliminate those fact types (if the value types are eliminated)
				foreach (KeyValuePair<FactType, ObjectType> kvp in refModeFactTypes)
				{
					if (ignoredElements.ContainsKey(kvp.Value))
					{
						ignoredElements[kvp.Key] = true;
					}
				}

				LinkedElementCollection<FactType> modelFactTypes = model.FactTypeCollection;
				Dictionary<IConstraint, bool> supertypeConstraints = null;

				// Find other eliminated elements
				foreach (FactType factType in modelFactTypes)
				{
					if (ignoredElements.ContainsKey(factType))
					{
						continue;
					}
					SubtypeFact subtypeFact = null;
					FactType referenceModeFactType;
					if (factType.ImpliedByObjectification != null ||
						(null != (subtypeFact = factType as SubtypeFact)
							&& (subtypeFact.ProvidesPreferredIdentifier ||
							(null != (referenceModeFactType = subtypeFact.Subtype.ReferenceModeFactType) && ignoredElements.ContainsKey(referenceModeFactType))))) // Treat a secondary subtype as a normal fact type instead of a 1-1 if the reference scheme is ignored
					{
						ignoredElements[factType] = true;
						if (subtypeFact != null)
						{
							// The fact type is eliminated for a preferred identifier path, but we need the
							// relationship in the model. Non-preferred paths are treated as one-to-one relationships
							// by leaving the fact type in the model.
							SupertypeMetaRole supertypeRole = subtypeFact.SupertypeRole;
							ormModel.addConstraint(new SubtypeOf(new ORMEntityType(subtypeFact.Subtype.Name), new ORMEntityType(supertypeRole.RolePlayer.Name)));

							// Supertype role has attached constraints. Collect the supertype constraints that might be considered for
							// mapping (we don't currently handle some constructs) and test later if we should map them as subtype constraints
							// by verifying that all of the constrained roles are on subtype facts that we are mapping as subtypes instead of
							// one-to-one fact types.
							foreach (ConstraintRoleSequence constraintSequence in supertypeRole.ConstraintRoleSequenceCollection)
							{
								IConstraint constraint = constraintSequence.Constraint;
								switch (constraintSequence.Constraint.ConstraintType)
								{
									case ConstraintType.DisjunctiveMandatory:
									case ConstraintType.Subset:
									case ConstraintType.Exclusion:
										(supertypeConstraints ?? (supertypeConstraints = new Dictionary<IConstraint, bool>()))[constraint] = true;
										break;
								}
							}
						}
					}
					else
					{
						//Entity types creation
						java.util.ArrayList list = new java.util.ArrayList();
						foreach (RoleBase r in factType.RoleCollection)
						{
							ObjectType rolePlayer = r.Role.RolePlayer;
							if (!rolePlayer.IsImplicitBooleanValue)
							{
								//For each role extract the entity type name
								list.add(new org.unibz.ucmf.tellAPI.signatures.ORMEntityType(r.Role.RolePlayer.Name));
							}
							else
							{
								ignoredElements[rolePlayer] = true;
							}
						}
						ormModel.addConstraint(new org.unibz.ucmf.tellAPI.declarations.FactType(factType.Name, list));
					}
				}

				if (supertypeConstraints != null)
				{
					foreach (IConstraint constraint in supertypeConstraints.Keys)
					{
						switch (constraint.ConstraintType)
						{
							case ConstraintType.DisjunctiveMandatory:
								MandatoryConstraint mandatoryConstraint = (MandatoryConstraint)constraint;
								if (AreAllFactTypesAreRecognizedSubtypes(mandatoryConstraint.FactTypeCollection, ignoredElements))
								{
									ObjectType supertype = null;
									java.util.Set sonsList = new java.util.HashSet();
									foreach (Role role in mandatoryConstraint.RoleCollection)
									{
										if (supertype == null)
										{
											supertype = role.RolePlayer;
										}
										else if (role.RolePlayer != supertype)
										{
											sonsList = null; // We only handle direct children
											break;
										}
										sonsList.add(new org.unibz.ucmf.tellAPI.signatures.ORMEntityType(role.OppositeRole.Role.RolePlayer.Name));
									}
									if (sonsList != null)
									{
										ormModel.addConstraint(new org.unibz.ucmf.tellAPI.constraints.ExhaustiveTypes(sonsList, new org.unibz.ucmf.tellAPI.signatures.ORMEntityType(supertype.Name)));
									}
								}
								break;
							//
							//THIS CODE IS NOT ABLE TO DETECT THE SUBSET CONSTRAINT
							//
							case ConstraintType.Subset:
								SubsetConstraint subsetConstraint = (SubsetConstraint)constraint;
								if (AreAllFactTypesAreRecognizedSubtypes(subsetConstraint.FactTypeCollection, ignoredElements) &&
									!(null != subsetConstraint.ArityMismatchError ||
										null != subsetConstraint.TooFewRoleSequencesError ||
										null != subsetConstraint.TooManyRoleSequencesError ||
										subsetConstraint.CompatibleRolePlayerTypeErrorCollection.Count > 0))
								{
									LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = subsetConstraint.RoleSequenceCollection;
									LinkedElementCollection<Role> roles1 = roleSequences[0].RoleCollection;
									LinkedElementCollection<Role> roles2 = roleSequences[1].RoleCollection;
									java.util.ArrayList firstListOrmRole = new java.util.ArrayList();
									java.util.ArrayList secondListOrmRole = new java.util.ArrayList();
									for (int i = 0, roleCount = roles1.Count; i < roleCount; ++i)
									{
										FactType factType = roles1[i].FactType;
										firstListOrmRole.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole((factType.Name + factType.Id), i));
										factType = roles2[i].FactType;
										secondListOrmRole.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole((factType.Name + factType.Id), i));
									}

									//ormModel.addConstraint(new org.unibz.ucmf.tellAPI.constraints.SubsetOf(firstListOrmRole, secondListOrmRole));
								}
								break;
								//
								//THIS CODE IS NOT ABLE TO DETECT THE EXCLUSION CONSTRAINT
								//
							case ConstraintType.Exclusion:
								ExclusionConstraint exclusionConstraint = (ExclusionConstraint)constraint;
								if (AreAllFactTypesAreRecognizedSubtypes(exclusionConstraint.FactTypeCollection, ignoredElements) &&
									!(null != exclusionConstraint.ArityMismatchError ||
										null != exclusionConstraint.TooFewRoleSequencesError ||
										null != exclusionConstraint.TooManyRoleSequencesError ||
										exclusionConstraint.CompatibleRolePlayerTypeErrorCollection.Count > 0))
								{
									LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = exclusionConstraint.RoleSequenceCollection;
									LinkedElementCollection<Role> roles1 = roleSequences[0].RoleCollection;
									LinkedElementCollection<Role> roles2 = roleSequences[1].RoleCollection;
									java.util.ArrayList firstListOrmRole = new java.util.ArrayList();
									java.util.ArrayList secondListOrmRole = new java.util.ArrayList();
									for (int i = 0, roleCount = roles1.Count; i < roleCount; ++i)
									{
										FactType factType = roles1[i].FactType;
										firstListOrmRole.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole((factType.Name + factType.Id), i));
										factType = roles2[i].FactType;
										secondListOrmRole.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole((factType.Name + factType.Id), i));
									}

									//It seems to be working only for types and not for predicates
									//ormModel.addConstraint(new tellAPI.constraints.Exclusive(firstListOrmRole, secondListOrmRole));
								}

								break;
						}
					}
				}

				//FIX SUBSET
				foreach (var sc_constraint in model.SetComparisonConstraintCollection)
				{
					if (typeof(SubsetConstraint) == sc_constraint.GetType()) // if it's a RSETsub constraint
					{
						// If this constraint is a wrong constraint, i.e. has some problems with it, is not fully specified, 
						// is not correctly specified, then we just skip it. 
						// TO REVISE: Maybe we should show some warning message? 
						if (null != sc_constraint.ArityMismatchError || null != sc_constraint.TooFewRoleSequencesError
							|| null != sc_constraint.TooManyRoleSequencesError || sc_constraint.CompatibleRolePlayerTypeErrorCollection.Count > 0)
							continue;


						java.util.ArrayList rolesSup = new java.util.ArrayList();
						java.util.ArrayList rolesSub = new java.util.ArrayList();

						// RSETsub may be only between two role sequences
						if (sc_constraint.RoleSequenceCollection.Count == 2)
						{
							Role[] roles1 = sc_constraint.RoleSequenceCollection.ToArray()[0].RoleCollection.ToArray();
							Role[] roles2 = sc_constraint.RoleSequenceCollection.ToArray()[1].RoleCollection.ToArray();

							int rolesNumber = roles1.Length;
							for (int i = 0; i < rolesNumber; i++)
							{

								FactType factType = roles1[i].FactType;
								String x = factType.Name + i + 1;
								rolesSub.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole(factType.Name, i + 1));
								factType = roles2[i].FactType;
								String y = factType.Name + i + 1;
								rolesSup.add(new org.unibz.ucmf.tellAPI.signatures.ORMRole(factType.Name, i + 1));

							}

							ormModel.addConstraint(new org.unibz.ucmf.tellAPI.constraints.SubsetOf(rolesSub, rolesSup));
						}
					}
				}

				//FIX EXCLUSIVETYPES and EXCLUSIVESET
				foreach (SetComparisonConstraint elem in model.SetComparisonConstraintCollection)
				{
					if (typeof(ExclusionConstraint) == elem.GetType())
					{
						if (elem.FactTypeCollection[0].GetType() == typeof(SubtypeFact))
						{
							java.util.ArrayList setExclusiveTypes = new java.util.ArrayList();
							foreach (SubtypeFact subtypefact in elem.FactTypeCollection)
							{
								String subTypeName = subtypefact.Subtype.Name;
								String superTypeName = subtypefact.Supertype.Name;
								org.unibz.ucmf.tellAPI.signatures.ORMEntityType type = new org.unibz.ucmf.tellAPI.signatures.ORMEntityType(subTypeName);
								setExclusiveTypes.add(type);
							}
							org.unibz.ucmf.tellAPI.constraints.ExclusiveTypes exclusiveTypes = new org.unibz.ucmf.tellAPI.constraints.ExclusiveTypes(setExclusiveTypes);
							ormModel.addConstraint(exclusiveTypes);
						}

						if (elem.FactTypeCollection[0].GetType() == typeof(FactType))
						{
							java.util.ArrayList set = new java.util.ArrayList();

							foreach (FactType fact in elem.FactTypeCollection)
							{
								java.util.ArrayList factTypeRoles = new java.util.ArrayList();

								foreach (Role role in fact.RoleCollection)
								{
									org.unibz.ucmf.tellAPI.signatures.ORMRole theRole = new org.unibz.ucmf.tellAPI.signatures.ORMRole(fact.Name, roleIndex(role));
									factTypeRoles.add(theRole);
								}
								set.add(factTypeRoles);
							}
							ExclusivePredicates exclusive = new ExclusivePredicates(set);
							ormModel.addConstraint(exclusive);
						}
					}
				}



				foreach (SetConstraint constraint in model.SetConstraintCollection)
				{
					LinkedElementCollection<Role> roles;
					Role role;
					ObjectType rolePlayer;
					FactType factType;
					switch (((IConstraint)constraint).ConstraintType)
					{
						//case ConstraintType.ImpliedMandatory:
						case ConstraintType.SimpleMandatory:
							roles = ReturnRolesIfValidConstraint(constraint, ignoredElements);
							if (roles != null &&
								roles.Count == 1 && // Verify for implied constraint case
								null != (rolePlayer = (role = roles[0]).RolePlayer) &&
								!ignoredElements.ContainsKey(factType = role.FactType))
							{
								java.util.ArrayList mandlist = new java.util.ArrayList();
								mandlist.add(new ORMRole(factType.Name, roleIndex(roles[0])));
								ormModel.addConstraint(new Mandatory(new ORMEntityType(rolePlayer.Name), mandlist));
							}
							break;
						case ConstraintType.InternalUniqueness:
							roles = ReturnRolesIfValidConstraint(constraint, ignoredElements);
							if (roles != null && roles.Count == 1)
							{
								role = roles[0];
								factType = role.FactType;
								java.util.ArrayList uniqlist = new java.util.ArrayList();
								String f = factType.Name;
								uniqlist.add(new ORMRole(factType.Name, roleIndex(roles[0])));
								ormModel.addConstraint(new org.unibz.ucmf.tellAPI.constraints.Unique(uniqlist));
							}
							break;
					}

				}

				//Let's set the inferred constraints container for the layout
				InferredConstraints inferredConstraints = GetCleanInferredConstraints(model);

				//OWL Generation (and also dlrTree generation)
				ormModel.generateOWL();

				//Write ontology to disk
				//ormModel.saveOntologyOnFile("C://Users//Francesco//Desktop//onto.owl");

				//Ask inferred knowledge
				AskManager ask = new AskManager(ormModel);
				ask.getUnsatClasses();
				ask.getEquivalentTypes();
				ask.getEquivalentPredicates();
				ask.getExclusiveTypes();
				ask.getExclusiveSet();
				ask.getInferredSimpleMandatory();
				ask.getInferredUnique();
				ask.getEntityTypesHierarchy();
				ask.getPredicatesHierarchy();

				//All the inferred knowledge is in ask.getInferredModel which is an InferredModel Object
				InferredModel infModel = ask.getInferredModel();

				//Select the EMPTYSET
				InferredEmptySet emptySet = infModel.getEmptySet();
				CreateInferredEmptySet(inferredConstraints, emptySet);

				//Select the HIERARCHY for TYPES
				//Add a tiny fix for the TYPE without children
				InferredHierarchyTypes hierarchyTypes = infModel.getHierarchyTypes();
				CreateInferredEntityTypesHierarchy(inferredConstraints, hierarchyTypes);

				//Select the HIERARCHY for PREDICATES
				//WE NEED another HEADER IN THE ORM MODEL similar to Hierarchy for types, named Predicates Hierarchy
				//InferredHierarchyPredicates hierarchyPredicates = infModel.getHierarchyPredicates();
				//CreateInferredEntityTypesPredicates(inferredConstraints, hierarchyPredicates);

				//Select the EQUIVALENT TYPES
				//ANOTHER HEADER NEEDED named Equivalent Types
				//InferredEquivalentTypes equivalentTypes = infModel.getInferredEquivalentTypes();
				//CreateInferredEquivalentTypes(inferredConstraints, equivalentTypes);

				//Select the EQUIVALENT PREDICATES
				//ANOTHER HEADER NEEDED named Equivalent Predicates
				//InferredEquivalentPredicates equivalentPredicates = infModel.getInferredEquivalentPredicates();
				//CreateInferredEquivalentPredicates(inferredConstraints, equivalentPredicates);

				//FAKE ZONE
				CreateInferredExclusiveTypes(inferredConstraints, "Type1", "Type2");
				CreateInferredExclusiveSet(inferredConstraints, "Set1", "Set2");
				CreateInferredMandatory(inferredConstraints, "Fake-Mandatory");
				CreateInferredUniqueness(inferredConstraints, "Fake-Uniqueness");

				/*
				foreach (Inferred inferredConstraint in infModel.getInferredConstraints())
				{
					InferredExclusiveTypes exclusiveType;
					InferredExclusiveSet exclusiveSet;
					InferredSimpleMandatory mandatory;
					InferredSimpleUnique unique;
					if (null != (exclusiveType = inferredConstraint as InferredExclusiveTypes))
					{
						CreateInferredExclusiveTypes(inferredConstraints, exclusiveType.getFirst(), exclusiveType.getSecond());
					}
					else if (null != (exclusiveSet = inferredConstraint as InferredExclusiveSet))
					{
						CreateInferredExclusiveSet(inferredConstraints, exclusiveSet.getFirst(), exclusiveSet.getSecond());
					}
					else if (null != (mandatory = inferredConstraint as InferredSimpleMandatory))
					{
						CreateInferredMandatory(inferredConstraints, mandatory.getMandatoryRoleName());
					}
					else if (null != (unique = inferredConstraint as InferredSimpleUnique))
					{
						CreateInferredUniqueness(inferredConstraints, unique.getUniqueRoleName());
					}
				}
				*/
			}

		}

		private InferredConstraints GetCleanInferredConstraints(ORMModel model)
		{
			Store store = model.Store;
			Partition partition = Partition.FindByAlternateId(store, typeof(InferenceResult));
			InferredConstraints retVal;
			if (partition == null)
			{
				partition = new Partition(store);
				partition.AlternateId = typeof(InferenceResult);
				retVal = new InferredConstraints(partition);
				retVal.Model = model;
			}
			else if (null == (retVal = InferredConstraintsTargetORMModel.GetInferredConstraints(model)))
			{
				retVal = new InferredConstraints(partition);
				retVal.Model = model;
			}
			else
			{
				retVal.SetConstraintCollection.Clear();
				retVal.SetComparisonConstraintCollection.Clear();
				retVal.SubtypeFactCollection.Clear();
			}
			return retVal;
		}




		#region DERIVATION RULES

		#region Subtype Derivation Rules into OWL

		/*
				private void subtypeDR(ORMModel model, String ontoIRI, java.util.Set axioms)
				{
					foreach (ObjectType elem in model.ObjectTypeCollection)
						if (elem.DerivationRule != null && elem.IsSubtype)
						{
							//Store Derivation Rule in a var
							SubtypeDerivationRule dr = elem.DerivationRule;
							//MessageBox.Show(dr.DerivationStorage.ToString());

							//Set axiom to encode the derivation rule
							OWLAxiom axiom;

							//Set leftSide
							OWLClass leftSide = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI + "#" + prepareForOWL(elem.Name)));

							//Set righside
							OWLClassExpression rightSide;

							//Set rootObject
							string rootName = dr.SingleLeadRolePath.RootObjectType.Name;
							OWLClass rootObjectDR = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI + "#" + prepareForOWL(rootName)));

							//Set hashset for node traversal algorithm
							java.util.Set hashSet = new java.util.HashSet();
							//DR with more subpaths
							if (dr.SingleLeadRolePath.SubPathCollection.Count > 0)
							{
								rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, DrNodeTraversal(dr.SingleLeadRolePath, ontoIRI, hashSet));
								axiom = myOWLDataFactory.getOWLEquivalentClassesAxiom(leftSide, rightSide);
								axioms.add(axiom); addAxiom(axioms, axiom);
								////System.IO.File.WriteAllText(@"C:\Users\Simone\Desktop\WriteAxiom.txt", axiom.ToString());
							}

							//otherwise, simple DR
							else
							{
								// Smoker = Person
								ReadOnlyCollection<PathedRole> lista = dr.SingleLeadRolePath.PathedRoleCollection;
								if (lista.Count == 0)
								{
									rightSide = rootObjectDR;
									axiom = myOWLDataFactory.getOWLEquivalentClassesAxiom(leftSide, rightSide);
									axioms.add(axiom); addAxiom(axioms, axiom);
									//System.IO.File.WriteAllText(@"C:\Users\Simone\Desktop\WriteAxiom.txt", axiom.ToString());
								}

								//R.C R.notC
								//not(R.C) notR.notC
								//Female = Person and not has Sex where Sex is M
								else if (lista.Count == 3)
								{
									PathedRole firstRoleR = lista[0]; //R1
									PathedRole secondRoleR = lista[1]; //R2
									PathedRole concept = lista[2]; //C
									String conceptName = concept.Role.FactType.RoleCollection[0].Role.RolePlayer.PlayedRoleCollection[0].RolePlayer.ToString();

									OWLClassExpression conceptNameOWLClass = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI + "#" + prepareForOWL(conceptName)));

									if (concept.IsNegated)
									{
										//conceptNameOWLClass = myOWLDataFactory.getOWLObjectComplementOf(conceptNameOWLClass);

									}
									if (concept.IsNegated || (firstRoleR.IsNegated && concept.IsNegated))
									{
										conceptNameOWLClass = myOWLDataFactory.getOWLObjectComplementOf(conceptNameOWLClass);
									}
									OWLObjectProperty roleIndex2 = myOWLDataFactory.getOWLObjectProperty(IRI.create(ontoIRI + "#" + roleIndex(concept.Role)));
									OWLClassExpression lastEx = myOWLDataFactory.getOWLObjectSomeValuesFrom(roleIndex2, conceptNameOWLClass);
									string name = firstRoleR.Role.FactType.Name;
									OWLClassExpression objClass = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI + "#obj_" + prepareForOWL(name)));
									OWLClassExpression intersec = myOWLDataFactory.getOWLObjectIntersectionOf(objClass, lastEx);
									OWLObjectProperty firstInv = myOWLDataFactory.getOWLObjectProperty(IRI.create(ontoIRI + "#" + roleIndex(firstRoleR.Role)));
									if (firstRoleR.IsNegated)
									{
										OWLClassExpression firstEx = myOWLDataFactory.getOWLObjectSomeValuesFrom(firstInv.getInverseProperty(), intersec);
										OWLClassExpression complement = myOWLDataFactory.getOWLObjectComplementOf(firstEx);
										rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, complement);
										axiom = myOWLDataFactory.getOWLEquivalentClassesAxiom(leftSide, rightSide);
										axioms.add(axiom); addAxiom(axioms, axiom);
									}
									else
									{
										OWLClassExpression firstEx = myOWLDataFactory.getOWLObjectSomeValuesFrom(firstInv.getInverseProperty(), intersec);
										rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, firstEx);
										axiom = myOWLDataFactory.getOWLEquivalentClassesAxiom(leftSide, rightSide);
										axioms.add(axiom); addAxiom(axioms, axiom);
									}


								}

								else
								{
									// Smoker = Person and smokes
									OWLClass item = null;
									OWLClassExpression complement = null;
									rightSide = null;

									foreach (PathedRole obj in lista)
									{
										if (obj.Role.GetType() == typeof(Role))
										{
											rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, PredicateToOwl(obj, ontoIRI));
										}
										//isEntity
										else
										{
											//nome intero del Fact Type tipo:  MalatoIsASubtypeOfPerson
											string nameEntity = obj.Role.FactType.Name;
											//MessageBox.Show(obj.Role.FactType.RoleCollection[0].Role.RolePlayer.ToString());
											//isola solo il nome del Entity Type, altrimenti l'assioma e' inconsistente
											//MalatoIsASubtypeOfPerson ---> Malato
											if (nameEntity.Contains("IsASubtypeOf"))
											{
												nameEntity = nameEntity.Substring(0, nameEntity.LastIndexOf("IsASubtypeOf"));
											}
											item = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI + "#" + prepareForOWL(nameEntity)));

											if (obj.IsNegated)
											{
												complement = myOWLDataFactory.getOWLObjectComplementOf(item);
												rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, complement);
											}
											else
											{
												rightSide = myOWLDataFactory.getOWLObjectIntersectionOf(rootObjectDR, item);
											}
										}

									}
									axiom = myOWLDataFactory.getOWLEquivalentClassesAxiom(leftSide, rightSide);
									axioms.add(axiom); addAxiom(axioms, axiom);
									//System.IO.File.WriteAllText(@"C:\Users\Simone\Desktop\WriteAxiom.txt", axiom.ToString());
								}
							}
						}
				}

			*/

		/*
	private OWLClassExpression DrNodeTraversal(RolePath p, String ontoIRI2, java.util.Set h)
	{
		foreach (RoleSubPath item in p.SubPathCollection)
		{
			if (item.SubPathCollection.Count == 0)
			//nodo foglia
			{
				ReadOnlyCollection<PathedRole> pathedRoleCollection = item.PathedRoleCollection;
				foreach (PathedRole obj in pathedRoleCollection)
				{

					//se è un ruolo
					if (obj.Role.GetType() == typeof(Role))
					{
						h.add(PredicateToOwl(obj, ontoIRI2));
					}

					//altrimenti se è un entity
					else
					{

						string name = obj.Role.FactType.RoleCollection[0].Role.RolePlayer.Name;
						if (name.Contains("IsASubtypeOf"))
						{
							name = name.Substring(0, name.LastIndexOf("IsASubtypeOf"));
						}
						OWLClassExpression leaf = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI2 + "#" + prepareForOWL(name)));
						if (obj.IsNegated)
						{
							leaf = myOWLDataFactory.getOWLObjectComplementOf(leaf);
						}
						h.add(leaf);
						//MessageBox.Show("DEBUG LEAF: " + leaf.ToString());
						//MessageBox.Show("----debug HASHSET" + h.ToString());
					}
				}

				//MessageBox.Show("RECURSIVE---LEAF ID: " + item.Id + " operatore " + item.SplitCombinationOperator);
			}
			else
			{
				//nodo NON foglia
				if (item.SplitCombinationOperator.ToString().Equals("And"))
				{
					java.util.Set tmpSet1 = new java.util.HashSet();
					OWLClassExpression tmp1 = (DrNodeTraversal(item, ontoIRI2, tmpSet1));
					h.add(tmp1); //BUGFIX
					//MessageBox.Show("DEBUG NT AND: " + tmp1.ToString());
					//MessageBox.Show("----debug HASHSET" + h.ToString());

				}
				//if (item.SplitCombinationOperator.ToString().Equals("Or") || item.SplitCombinationOperator.ToString().Equals("Xor"))
				else
				{

					java.util.Set tmpSet2 = new java.util.HashSet();
					OWLClassExpression tmp2 = (DrNodeTraversal(item, ontoIRI2, tmpSet2));
					h.add(tmp2);  //BUGFIX
					//MessageBox.Show("DEBUG NT OR: " + tmp2.ToString());
					//MessageBox.Show("----debug HASHSET" + h.ToString());

				}
				//MessageBox.Show("RECURSIVE---NT ID: " + item.Id + " operatore " + item.SplitCombinationOperator);
				//DrNodeTraversal(item.SubPathCollection, ontoIRI2, e, h);
			}
		}
		//MessageBox.Show("----debug HASHSET" + h.ToString());

		OWLClassExpression e = null;
		if (p.SplitCombinationOperator.ToString().Equals("And"))
		{

			e = myOWLDataFactory.getOWLObjectIntersectionOf(h);
			return e;
		}
		else
		{

			e = myOWLDataFactory.getOWLObjectUnionOf(h);
			return e;
		}

	}
	private OWLClassExpression PredicateToOwl(PathedRole p, String ontoIRI2)
	{
		Role role = p.Role;

		if (role.FactType.UnaryRole != null)
		{
			string factTypeName = p.Role.FactType.Name;
			OWLClass c = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI2 + "#obj_" + prepareForOWL(factTypeName)));
			OWLObjectProperty attTranslation = myOWLDataFactory.getOWLObjectProperty(IRI.create(ontoIRI2 + "#" + roleIndex(p.Role)));
			OWLClassExpression unaryObjExpression = myOWLDataFactory.getOWLObjectSomeValuesFrom(attTranslation.getInverseProperty(), c);
			if (p.IsNegated)
			{
				unaryObjExpression = myOWLDataFactory.getOWLObjectComplementOf(unaryObjExpression);
			}
			return unaryObjExpression;
		}
		//OBIETTIVO SECONDARIO: leggerli con il corretto ordine
		//studiare la classe ReadinOrderCollection
		// obj.Role.FactType.OrderedRoleCollection

		int arity = role.FactType.RoleCollection.Count;
		string name = role.FactType.Name;
		//creo obj_
		OWLClassExpression objClass = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI2 + "#obj_" + prepareForOWL(name)));
		//MessageBox.Show(objClass.ToString());
		//Creo la LISTA di tutti i roleplayers
		LinkedList<OWLClassExpression> lis = new LinkedList<OWLClassExpression>();
		//aggiungi gli oggetti alla lista
		foreach (RoleBase rolePlayerItem in role.FactType.RoleCollection)
		{
			lis.AddLast(partialExpression(rolePlayerItem, ontoIRI2));
		}
		//rimuovo il firstRoleR, l'intruso
		lis.RemoveFirst();


		//Creo il set di tutti i roleplayers
		java.util.Set predicateSet = new java.util.HashSet();
		//aggiungo al set obj_
		predicateSet.add(objClass);
		//riempio set usando la func che ritorna una expression per ciascuno
		foreach (OWLClassExpression elem in lis)
		{
			predicateSet.add(elem);
		}

		foreach (RoleBase rolePlayerItem in role.FactType.RoleCollection)
		{
			predicateSet.add(partialExpression(rolePlayerItem, ontoIRI2));
		}


		OWLClassExpression allPredicates = myOWLDataFactory.getOWLObjectIntersectionOf(predicateSet);
		OWLObjectProperty roleNumber = myOWLDataFactory.getOWLObjectProperty(IRI.create(ontoIRI2 + "#" + roleIndex(role.Role)));
		OWLClassExpression finalExpression = myOWLDataFactory.getOWLObjectSomeValuesFrom(roleNumber.getInverseProperty(), allPredicates);

		return finalExpression;
	}
	private OWLClassExpression partialExpression(RoleBase rolePlayer, String ontoIRI2)
	{
		string name = rolePlayer.Role.RolePlayer.ToString();
		OWLClass elem = myOWLDataFactory.getOWLClass(IRI.create(ontoIRI2 + "#" + prepareForOWL(name)));
		OWLObjectProperty attTranslation = myOWLDataFactory.getOWLObjectProperty(IRI.create(ontoIRI2 + "#" + roleIndex(rolePlayer.Role)));
		OWLClassExpression partialExpression = myOWLDataFactory.getOWLObjectSomeValuesFrom(attTranslation, elem);
		//MessageBox.Show("Attore: " + name + " id: " + rolePlayer.Role.RolePlayer.ReferenceMode.Id + "roleindex: " + roleIndex(rolePlayer.Role));
		//MessageBox.Show(partialExpression.ToString());
		return partialExpression;
	}

*/

		#endregion Subtype Derivation Rules into OWL

		#endregion DERIVATION RULES


	}
}