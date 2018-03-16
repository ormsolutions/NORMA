using System;
using System.IO;
using System.Linq;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using System.Collections.ObjectModel;

using ikvm.io;
using java.io;
using ikvm.lang;
using org.semanticweb.owlapi.model;
using org.semanticweb.owlapi.apibinding;
using org.semanticweb.owlapi.reasoner;
using org.semanticweb.owlapi.util;
using org.semanticweb.owlapi.vocab;
//using org.coode.owlapi.manchesterowlsyntax;
using org.semanticweb.owlapi.io;
using java.util;
using org.semanticweb.owlapi.reasoner.impl;

namespace unibz.ORMInferenceEngine
{
    class ORMInferenceGenerator
    {
        OWLReasonerFactory reasonerFactory; 
        ConsoleProgressMonitor progressMonitor;
        OWLReasonerConfiguration reasonerConfig;
		OWLOntology inferredOntology;

        public ORMInferenceGenerator()
        {
			reasonerFactory = new uk.ac.manchester.cs.jfact.JFactFactory();
            progressMonitor = new ConsoleProgressMonitor();
            reasonerConfig = new SimpleConfiguration(progressMonitor);
			inferredOntology = null;
        }

        private String prepareForOWL(String str)
        {
            return ORM2OWLTranslationManager.prepareForOWL(str);
        }

        private static String restoreForORM(String str)
        {
            return ORM2OWLTranslationManager.restoreForORM(str);
        }

        public static ObjectType findObjectTypeByName(ORMModel model, String name)
        {
			//REDO: use map to connect ontology names and GUIDS and then
			//model.Partition.ElementDirectory.FindElement(GUID)
            String restoredName = restoreForORM(name);
			return model.ObjectTypeCollection.Find(o => o.Name.Equals(restoredName));
        }

        public static FactType findFactTypeByName(ORMModel model, String name)
        {
			//REDO: use map to connect ontology names and GUIDS and then
			//model.Partition.ElementDirectory.FindElement(GUID)
			String restoredName = restoreForORM(name);
			return model.FactTypeCollection.Find(f => f.Name.Equals(restoredName));
        }

        public void deriveInconsistencies(ORMModel model, OWLOntology ontology, 
            Collection<FactType> unsatFactTypes, Collection<ObjectType> unsatEntityTypes)
        {
            if (null != model && null != ontology && ontology.getAxiomCount() > 0)
            {
                OWLReasoner reasoner = reasonerFactory.createReasoner(ontology, reasonerConfig);
                reasoner.precomputeInferences();

                Node bottomNode = reasoner.getUnsatisfiableClasses();
                String clsName = "";
                java.util.Set unsatisfiable = bottomNode.getEntitiesMinusBottom();
                object[] unsatArray = unsatisfiable.toArray();

                HashSet<String> unsatFactTypeNames = new HashSet<String>();
                HashSet<String> unsatEntityTypeNames = new HashSet<String>();
                foreach (OWLClass cls in unsatArray)
                {
                    clsName = cls.getIRI().getFragment();
                    if (clsName.StartsWith("obj_")) // unsatisfiable relation
                        unsatFactTypeNames.Add(clsName.Substring(4));
                    else 
                        unsatEntityTypeNames.Add(clsName);
                }

                foreach (ObjectType elem in model.ObjectTypeCollection)
                {
                    if (elem.IsValueType)
                        continue;
                    if (unsatEntityTypeNames.Contains(elem.Name))
                        unsatEntityTypes.Add(elem);
                }

                foreach (FactType elem in model.FactTypeCollection)
                {
                    // in java tool we wanted "to exclude R-SETsub connected with reference modes", but I don't remember what does it actually mean!
                    //if (elem.)
                    //    continue;
                    if (unsatFactTypeNames.Contains(elem.Name))
                        unsatFactTypes.Add(elem);
                }
            }
        }

        public void manualDeriveInconsistencies(ORMModel model, OWLOntology ontology,
                                                 Collection<FactType> unsatFactTypes, Collection<ObjectType> unsatEntityTypes)
        {
            if (null != model && null != ontology && ontology.getAxiomCount() > 0)
            {
                OWLDataFactory factory = ontology.getOWLOntologyManager().getOWLDataFactory();
                OWLReasoner reasoner = reasonerFactory.createReasoner(ontology, reasonerConfig);
                OWLClass bottom = factory.getOWLNothing();
                reasoner.precomputeInferences();

                String ontoIRI = ontology.getOntologyID().getOntologyIRI().toString();

                foreach (ObjectType elem in model.ObjectTypeCollection)
                {
                    if (elem.IsValueType)
                        continue;
                    OWLClass clsEntType = factory.getOWLClass(IRI.create(ontoIRI + "#" + prepareForOWL(elem.Name)));
                    OWLAxiom axiom = factory.getOWLSubClassOfAxiom(clsEntType, bottom);
                    if (reasoner.isEntailed(axiom))
                        unsatEntityTypes.Add(elem);
                }

                foreach (FactType elem in model.FactTypeCollection)
                {
                    // in java tool we wanted "to exclude R-SETsub connected with reference modes", but I don't remember what does it actually mean!
                    //if (elem.)
                    //    continue;
                    OWLClass clsObjectifiedRelation = factory.getOWLClass(IRI.create(ontoIRI + "#obj_" + prepareForOWL(elem.Name)));
                    OWLAxiom axiom = factory.getOWLSubClassOfAxiom(clsObjectifiedRelation, bottom);
                    if (reasoner.isEntailed(axiom))
                        unsatFactTypes.Add(elem);
                }
            }
        }


		public void deriveFreshConstraints(ORMModel model, OWLOntology ontology, Partition outputPartition)
		{
			OWLReasoner reasoner = reasonerFactory.createReasoner(ontology, reasonerConfig);
			reasoner.precomputeInferences();
			OWLClassNode unsatClasses = reasoner.getUnsatisfiableClasses() as OWLClassNode;

			deriveFreshConstraints(model, ontology, reasoner, outputPartition, unsatClasses);
		}

		public void deriveFreshConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, OWLClassNode unsatClasses)
		{
			computeInferredOntology(ontology, reasoner);
			if (null != inferredOntology && inferredOntology.getAxiomCount() > 0 && null != model)
				extractFreshConstraints(model, ontology, reasoner, outputPartition, unsatClasses);
		}

		public void deriveFreshConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, InferredConstraints container, OWLClassNode unsatClasses)
		{
			computeInferredOntology(ontology, reasoner);
			if (null != inferredOntology && inferredOntology.getAxiomCount() > 0 && null != model)
				extractFreshConstraints(model, ontology, reasoner, outputPartition, container, unsatClasses);
		}


		public OWLReasoner getPrecomputedReasoner(OWLOntology ontology)
		{
			OWLReasoner reasoner = reasonerFactory.createReasoner(ontology, reasonerConfig);
			reasoner.precomputeInferences();
			return reasoner;
		}

		public void computeInferredOntology(OWLOntology ontology, OWLReasoner reasoner)
		{
			if (null != ontology && ontology.getAxiomCount() > 0)
			{
				java.util.List generators = new java.util.ArrayList();
				generators.add(new InferredSubClassAxiomGenerator());
				generators.add(new InferredEquivalentClassAxiomGenerator());
				generators.add(new InferredDisjointClassesAxiomGenerator());
				//generators.add(new InferredSubDataPropertyAxiomGenerator());
				//generators.add(new InferredSubObjectPropertyAxiomGenerator());
				//generators.add(new InferredObjectPropertyCharacteristicAxiomGenerator());
				// Put the inferred axioms into a fresh empty ontology.
				inferredOntology = ontology.getOWLOntologyManager().createOntology();
				InferredOntologyGenerator iog = new InferredOntologyGenerator(reasoner, generators);
				//iog.fillOntology(ontology.getOWLOntologyManager(), inferredOntology);
                //writeOntology(inferredOntology);
				
			}
			else
				inferredOntology = null;
		}

		public void extractFreshConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, OWLClassNode unsatClasses)
		{
			if (null != model && null != inferredOntology && null != ontology && inferredOntology.getAxiomCount() > 0)
			{
				InferredConstraints container = InferredConstraintsTargetORMModel.GetInferredConstraints(model);
				if (container == null)
				{
					container = new InferredConstraints(outputPartition);
					container.Model = model;
				}
				else
				{
					container.SetConstraintCollection.Clear();
					container.SetComparisonConstraintCollection.Clear();
				}

				extractFreshConstraints(model, ontology, reasoner, outputPartition, container, unsatClasses);
			}
		}

		public void extractFreshConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, InferredConstraints container, OWLClassNode unsatClasses)
        {
			if (null != container && null != model && null != inferredOntology && null != ontology && inferredOntology.getAxiomCount() > 0)
            {
                if (inferredOntology.getAxiomCount(AxiomType.DISJOINT_CLASSES) > 0)
                {
                    object[] disjAxiomsArray = inferredOntology.getAxioms(AxiomType.DISJOINT_CLASSES).toArray();
                    foreach (OWLDisjointClassesAxiom axiom in disjAxiomsArray)
                    {
                        if(ontology.containsAxiom(axiom))
                            continue;

                        object[] pairaxioms = axiom.asPairwiseAxioms().toArray();
                        foreach (OWLDisjointClassesAxiom pairaxiom in pairaxioms)
                        {
                            if (ontology.containsAxiom(pairaxiom))
                                continue;
                            
                            OWLClass owlClass1 = ((OWLClass)pairaxiom.getClassesInSignature().toArray()[0]);
                            OWLClass owlClass2 = ((OWLClass)pairaxiom.getClassesInSignature().toArray()[1]);
                            if (!reasoner.isSatisfiable(owlClass1) || !reasoner.isSatisfiable(owlClass2))
                                continue;

                            String class1 = owlClass1.getIRI().getFragment();
                            String class2 = owlClass2.getIRI().getFragment();
                            if (class1.StartsWith("_ATOP") || class2.StartsWith("_ATOP") || class1.Equals("Thing") || class2.Equals("Thing"))
                                continue;
                        
                            //// Not really sure we need this check + too lazy to implement it
                            //if (isAlreadyInCompositeEXC(pairaxiom, ontology))
                            //    continue;

                            if(!class1.StartsWith("obj_") && !class2.StartsWith("obj_")) // O-SETex
                            {
                                ObjectType subObjType1 = findObjectTypeByName(model, class1);
                                ObjectType subObjType2 = findObjectTypeByName(model, class2);
                                if (null == subObjType1 || null == subObjType2)
                                    continue;

								//find subtypeFact1 and subtypeFact2
								SubtypeFact forSubType1 = null;
								SubtypeFact forSubType2 = null;
								if (!ObjectType.WalkSupertypeRelationships(
									subObjType1,
									delegate(SubtypeFact subtypeFact, ObjectType subtype, int depth)
									{
										forSubType1 = subtypeFact;
										return ObjectTypeVisitorResult.Stop;
									}))
								{
									ObjectType.WalkSupertypeRelationships(
										subObjType2,
										delegate(SubtypeFact subtypeFact, ObjectType subtype, int depth)
										{
											forSubType2 = subtypeFact;
											return ObjectTypeVisitorResult.Stop;
										});
								}
								if (forSubType2 != null)
								{
									SetComparisonConstraint targetConstraint = new InferredExclusionConstraint(outputPartition);
									SetComparisonConstraintRoleSequence sequence = new SetComparisonConstraintRoleSequence(outputPartition);
									new SetComparisonConstraintHasRoleSequence(targetConstraint, sequence);
									new ConstraintRoleSequenceHasRole(sequence, forSubType1.SupertypeRole);
									sequence = new SetComparisonConstraintRoleSequence(outputPartition);
									new SetComparisonConstraintHasRoleSequence(targetConstraint, sequence);
									new ConstraintRoleSequenceHasRole(sequence, forSubType2.SupertypeRole);

									new SetComparisonConstraintIsInferred(container, targetConstraint);
								}
                            }
                            else if(class1.StartsWith("obj_") && class2.StartsWith("obj_")) // R-SETex
                            {
                                // The only possible way is when the arity is equal
                                // In ORMzero we have only BINARY! 
                                String rel1str = class1.Substring(4); // 4 - index of element after "obj_"
                                String rel2str = class2.Substring(4); // 4 - index of element after "obj_"

                                FactType factType1 = findFactTypeByName(model, rel1str);
                                FactType factType2 = findFactTypeByName(model, rel2str);
                                if (null == factType1 || null == factType2)
                                    continue;
                                if (factType1.RoleCollection.Count != factType2.RoleCollection.Count 
                                    && factType1.RoleCollection.Count == 2)
                                    continue;

								SetComparisonConstraint targetConstraint = new InferredExclusionConstraint(outputPartition);
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
                        }
                    }
                }

                if (inferredOntology.getAxiomCount(AxiomType.SUBCLASS_OF) > 0)
                {
                    object[] subAxiomsArray = inferredOntology.getAxioms(AxiomType.SUBCLASS_OF).toArray();
                    foreach (OWLSubClassOfAxiom axiom in subAxiomsArray)
                    {
                        if (ontology.containsAxiom(axiom))
                            continue;

                        OWLClass owlClassSub = axiom.getSubClass().asOWLClass();
                        OWLClass owlClassSuper = axiom.getSuperClass().asOWLClass();

                        if (!reasoner.isSatisfiable(owlClassSub) || !reasoner.isSatisfiable(owlClassSuper))
                            continue;

                        String subclass = owlClassSub.getIRI().getFragment();
                        String superclass = owlClassSuper.getIRI().getFragment();

                        if (subclass.StartsWith("_ATOP") || superclass.StartsWith("_ATOP") || subclass.Equals("Thing") || superclass.Equals("Thing"))
                            continue;

                        //// Not really sure we need this check + too lazy to implement it
                        //if (isAlreadyInCompositeISA(axm, ont))
                        //    continue;

                        if (!subclass.StartsWith("obj_") && !superclass.StartsWith("obj_")) // O-SETsub
                        {
                            ObjectType subObjType = findObjectTypeByName(model, subclass);
                            ObjectType superObjType = findObjectTypeByName(model, superclass);

							InferredSubtypeFact subtypefact = new InferredSubtypeFact(outputPartition);
							new SubtypeFactIsInferred(container, subtypefact);

							subtypefact.Subtype = subObjType;
							subtypefact.Supertype = superObjType;
							if(subObjType.IsValueType)
							{
								subtypefact.ProvidesPreferredIdentifier = true;
							}
							//foreach (var internalConstraint in subtypefact.GetInternalConstraints<SetConstraint>())
							//{
							//    new SetConstraintIsInferred(container, internalConstraint);
							//}
                        }
                        else if (subclass.StartsWith("obj_") && superclass.StartsWith("obj_")) // O-RETsub
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
                    }
                }

                //// Wouldn't it be that this axiom will be duplicated by 2 subclass axioms? + too lazy to implement now
                //if (inferredOntology.getAxiomCount(AxiomType.EQUIVALENT_CLASSES) > 0)
                //{
                //    object[] eqAxiomsArray = inferredOntology.getAxioms(AxiomType.EQUIVALENT_CLASSES).toArray();
                //    foreach (OWLEquivalentClassesAxiom axiom in eqAxiomsArray)
                //    {
                //        if (ontology.containsAxiom(axiom))
                //            continue;
                //    }
                //}
            }
        }

		public void deriveRefinedConstraints(ORMModel model, OWLOntology ontology, Partition outputPartition, InferredConstraints container)
		{
			if (null != model && null != ontology && ontology.getAxiomCount() > 0)
			{
				OWLReasoner reasoner = reasonerFactory.createReasoner(ontology, reasonerConfig);
				reasoner.precomputeInferences();
				OWLClassNode unsatClasses = reasoner.getUnsatisfiableClasses() as OWLClassNode;
				deriveRefinedConstraints(model, ontology, reasoner, outputPartition, container, unsatClasses);
			}
		}

		public void deriveRefinedConstraints(ORMModel model, OWLOntology ontology, Partition outputPartition)
		{
			if (null != model && null != ontology && ontology.getAxiomCount() > 0)
			{
				InferredConstraints container = InferredConstraintsTargetORMModel.GetInferredConstraints(model);
				if (container == null)
				{
					container = new InferredConstraints(outputPartition);
					container.Model = model;
				}
				else
				{
					container.SetConstraintCollection.Clear();
					container.SetComparisonConstraintCollection.Clear();
				}
				deriveRefinedConstraints(model, ontology, outputPartition, container);
			}
		}

		public void deriveRefinedConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, OWLClassNode unsatClasses)
		{
			if (null != model && null != ontology && ontology.getAxiomCount() > 0)
			{
				InferredConstraints container = InferredConstraintsTargetORMModel.GetInferredConstraints(model);
				if (container == null)
				{
					container = new InferredConstraints(outputPartition);
					container.Model = model;
				}
				else
				{
					container.SetConstraintCollection.Clear();
					container.SetComparisonConstraintCollection.Clear();
				}
				deriveRefinedConstraints(model, ontology, reasoner, outputPartition, container, unsatClasses);
			}
		}

		public void deriveRefinedConstraints(ORMModel model, OWLOntology ontology, OWLReasoner reasoner, Partition outputPartition, InferredConstraints container, OWLClassNode unsatClasses)
        {
            if (null != container && null != model && null != ontology && ontology.getAxiomCount() > 0)
            {
				OWLClassExpression clsMin;
                OWLClassExpression clsMax;
                OWLClassExpression clsExact;
                OWLClassExpression clsIntersect;

                OWLClass clsObjType;
                OWLClass clsObjectifiedRelation;
                OWLObjectProperty attTranslation;
                OWLClassExpression locRoleTranslation;
                OWLClassExpression locInvRoleTranslation;

                OWLClass clsObjType2;
                OWLClass clsObjectifiedRelation2;
                OWLObjectProperty att2Translation;
                OWLClassExpression locRole2Translation;

                OWLAxiom axiom;
                bool isEntailed = false;

                var roleElements = model.Partition.ElementDirectory.FindElements<Role>();
                OWLDataFactory factory = ontology.getOWLOntologyManager().getOWLDataFactory();
                //IRI ontIRI = ontology.getOntologyID().getOntologyIRI();
				IRI ontIRI = null;

				int lr_i = 0;
                int lr_j = 0; 
                foreach (Role role in roleElements)
                {
                    lr_i++;

                    //NB: We want to exclude R-SETsub connected with reference modes and internal objectification!!!!  
                    if (null != role.RolePlayer.ReferenceModeFactType && role.RolePlayer.ReferenceModeFactType.Id == role.FactType.Id)
                        continue;
					if (null != role.FactType.ImpliedByObjectification)
						continue;
                    if (typeof(SupertypeMetaRole) == role.GetType() || typeof(SubtypeMetaRole) == role.GetType())
                        continue;

                    attTranslation = factory.getOWLObjectProperty(IRI.create(ontIRI + "#" + ORM2OWLTranslationManager.roleIndex(role)));
                    clsObjectifiedRelation = factory.getOWLClass(IRI.create(ontIRI + "#obj_" + prepareForOWL(role.FactType.Name)));
                    clsObjType = factory.getOWLClass(IRI.create(ontIRI + "#" + prepareForOWL(role.RolePlayer.Name)));
                    locRoleTranslation = factory.getOWLObjectSomeValuesFrom(attTranslation.getInverseProperty(), clsObjectifiedRelation);

					if (unsatClasses.contains(clsObjType) || unsatClasses.contains(clsObjectifiedRelation))
						continue;

                    // Test exact frequency = uniqueness
                    clsExact = factory.getOWLObjectExactCardinality(1, attTranslation.getInverseProperty(), clsObjectifiedRelation);
                    axiom = factory.getOWLSubClassOfAxiom(locRoleTranslation, clsExact);
                    isEntailed = reasoner.isEntailed(axiom);
                    if (isEntailed && !ontology.containsAxiom(axiom))
                    {
						//MessageBox.Show("FREQ(" + role.FactType.Name + "." + ORM2OWLTranslationManager.roleIndex(role) + ", (1,1)");
						UniquenessConstraint targetConstraint = UniquenessConstraint.CreateInternalUniquenessConstraint(outputPartition, outputPartition.DomainDataDirectory.GetDomainClass(InferredUniquenessConstraint.DomainClassId));
						new ConstraintRoleSequenceHasRole(targetConstraint, role); // ==== targetConstraint.RoleCollection.Add(role);
						new SetConstraintIsInferred(container, targetConstraint); // maybe will be set automatically!!! may not need
                    }
                    // Test minmax frequency (currently supported only (0,1)
                    else if (!isEntailed)
                    {
                        clsMin = factory.getOWLObjectMinCardinality(0, attTranslation.getInverseProperty(), clsObjectifiedRelation);
                        clsMax = factory.getOWLObjectMaxCardinality(1, attTranslation.getInverseProperty(), clsObjectifiedRelation);
                        clsIntersect = factory.getOWLObjectIntersectionOf(clsMin, clsMax);
                        axiom = factory.getOWLSubClassOfAxiom(locRoleTranslation, clsIntersect);
                        isEntailed = reasoner.isEntailed(axiom);
                        if (isEntailed && !ontology.containsAxiom(axiom))
                        {
                            //MessageBox.Show("FREQ(" + role.FactType.Name + "." + role.Id + "(0,1)");
							FrequencyConstraint targetConstraint = new InferredFrequencyConstraint(outputPartition);
							targetConstraint.MinFrequency = 0;
							targetConstraint.MaxFrequency = 1;
							new ConstraintRoleSequenceHasRole(targetConstraint, role);
							new SetConstraintIsInferred(container, targetConstraint); // maybe will be set automatically!!! may not need
						}
                    }
                    // Test mandatory
                    locInvRoleTranslation = factory.getOWLObjectSomeValuesFrom(attTranslation.getInverseProperty(), clsObjectifiedRelation);
                    axiom = factory.getOWLSubClassOfAxiom(clsObjType, locInvRoleTranslation);
                    isEntailed = reasoner.isEntailed(axiom);

                    if (isEntailed && !ontology.containsAxiom(axiom))
                    {
						Set axms = ontology.getAxioms(clsObjType);
						MandatoryConstraint targetConstraint = MandatoryConstraint.CreateSimpleMandatoryConstraint(outputPartition, outputPartition.DomainDataDirectory.GetDomainClass(InferredMandatoryConstraint.DomainClassId));
						new ConstraintRoleSequenceHasRole(targetConstraint, role); // ==== targetConstraint.RoleCollection.Add(role);
						new SetConstraintIsInferred(container, targetConstraint); // maybe will be set automatically!!! may not need
                    }
                    // Test R-SET for roles
                    // NB: We want to exclude R-SETsub connected with reference modes!!!!
                    foreach (Role role2 in roleElements)
                    {
                        lr_j++; // needed to exclude double R-SETex appearance

                        //NB: We want to exclude R-SETsub connected with reference modes!!!!  
                        if (null != role2.RolePlayer.ReferenceModeFactType && role2.RolePlayer.ReferenceModeFactType.Id == role2.FactType.Id)
                            continue;
						if (null != role2.FactType.ImpliedByObjectification)
							continue;
						if (role.Id == role2.Id) // BUG - was skipping subset constraints between roles played by the same objecttype || role.RolePlayer.Id == role2.RolePlayer.Id)
                            continue;
                        if (typeof(SupertypeMetaRole) == role2.GetType() || typeof(SubtypeMetaRole) == role2.GetType())
                            continue;

						att2Translation = factory.getOWLObjectProperty(IRI.create(ontIRI + "#" + ORM2OWLTranslationManager.roleIndex(role2)));
                        clsObjectifiedRelation2 = factory.getOWLClass(IRI.create(ontIRI + "#obj_" + prepareForOWL(role2.FactType.Name)));
                        clsObjType2 = factory.getOWLClass(IRI.create(ontIRI + "#" + prepareForOWL(role2.RolePlayer.Name)));
                        locRole2Translation = factory.getOWLObjectSomeValuesFrom(att2Translation.getInverseProperty(), clsObjectifiedRelation2);

						if (unsatClasses.contains(clsObjType2) || unsatClasses.contains(clsObjectifiedRelation2))
							continue;
						
						// Test R-SETsub
                        axiom = factory.getOWLSubClassOfAxiom(locRoleTranslation, locRole2Translation);
                        isEntailed = reasoner.isEntailed(axiom);
                        // if the more general constraint is entailed (subrelation), don't show role inclusion
                        axiom = factory.getOWLSubClassOfAxiom(clsObjectifiedRelation, clsObjectifiedRelation2);
                        if (isEntailed && !ontology.containsAxiom(axiom) && !reasoner.isEntailed(axiom))
                        {
							//MessageBox.Show("R-SETsub({" + role.FactType.Name + "." + ORM2OWLTranslationManager.roleIndex(role) + "},"
							//    + "{" + role2.FactType.Name + "." + ORM2OWLTranslationManager.roleIndex(role2) + "})");

							SetComparisonConstraint targetConstraint = new InferredSubsetConstraint(outputPartition);
							new SetComparisonConstraintIsInferred(container, targetConstraint);

							SetComparisonConstraintRoleSequence roleseq1 = new SetComparisonConstraintRoleSequence(outputPartition);
							SetComparisonConstraintRoleSequence roleseq2 = new SetComparisonConstraintRoleSequence(outputPartition);

							roleseq1.RoleCollection.Add(role);
							roleseq2.RoleCollection.Add(role2);

							targetConstraint.RoleSequenceCollection.Add(roleseq1);
							targetConstraint.RoleSequenceCollection.Add(roleseq2);
                        }

                        // Test R-SETex
                        if (lr_i < lr_j)
                            continue;

                        axiom = factory.getOWLDisjointClassesAxiom(locRoleTranslation, locRole2Translation);
                        isEntailed = reasoner.isEntailed(axiom);
                        // if the more general constraint is entailed (exrelation), don't show role exclusion
                        axiom = factory.getOWLDisjointClassesAxiom(clsObjectifiedRelation, clsObjectifiedRelation2);
                        if (isEntailed && !ontology.containsAxiom(axiom) && !reasoner.isEntailed(axiom))
                        {
							//MessageBox.Show("R-SETexc({" + role.FactType.Name + "." + ORM2OWLTranslationManager.roleIndex(role) + "},"
							//    + "{" + role2.FactType.Name + "." + ORM2OWLTranslationManager.roleIndex(role2) + "})");

							SetComparisonConstraint targetConstraint = new InferredExclusionConstraint(outputPartition);
							new SetComparisonConstraintIsInferred(container, targetConstraint);

							SetComparisonConstraintRoleSequence roleseq1 = new SetComparisonConstraintRoleSequence(outputPartition);
							SetComparisonConstraintRoleSequence roleseq2 = new SetComparisonConstraintRoleSequence(outputPartition);

							roleseq1.RoleCollection.Add(role);
							roleseq2.RoleCollection.Add(role2);

							targetConstraint.RoleSequenceCollection.Add(roleseq1);
							targetConstraint.RoleSequenceCollection.Add(roleseq2);
                        }
                    }
                }
            }
        }

        private void writeOntology(OWLOntology ontology)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileOWL = @"\InferredConstraints.txt";
            using (StreamWriter sw = System.IO.File.AppendText(@desktopPath + fileOWL))
            {
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine(ontology.getAxioms().ToString());
            }
        }
    }


}
