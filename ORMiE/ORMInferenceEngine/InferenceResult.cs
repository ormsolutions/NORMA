using System;
using System.Drawing;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Modeling;
using System.Threading;
using System.ComponentModel;

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
using org.semanticweb.owlapi.reasoner.impl;
using System.Windows.Forms;


namespace unibz.ORMInferenceEngine
{
	partial class InferenceResult
	{
		#region Deserialization Fixup Classes
		/// <summary>
		/// A <see cref="IDeserializationFixupListener"/> for synchronizing the abstraction model on load
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new OWLHierachyFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class OWLHierachyFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// ORMModelFixupListener constructor
			/// </summary>
			public OWLHierachyFixupListener()
				: base((int)ORMInferenceEngineDeserializationFixupPhase.CreateInferenceResult)
			{
			}
			/// <summary>
			/// Make sure we have our tracker attached to all loaded models.
			/// </summary>
			/// <param name="element">An ORMModel element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ORMModel element, Store store, INotifyElementAdded notifyAdded)
			{
				InferenceResult inferenceResult = new InferenceResult(store);
				inferenceResult.Model = element;
				notifyAdded.ElementAdded(inferenceResult, true);
			}
		}
		#endregion // Deserialization Fixup Classes
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
		/// </summary>
		private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
		{
           FrameworkDomainModel.DelayValidateElement(((ModelHasObjectType)e.ModelElement).Model, DelayValidateUpdateInferenceResult);
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
		/// </summary>
		private static void ObjectTypeDeletedRule(ElementDeletedEventArgs e)
		{
			ORMModel model = ((ModelHasObjectType)e.ModelElement).Model;
			if (!model.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateUpdateInferenceResult);
			}
		}
		private static void DelayValidateUpdateInferenceResult(ModelElement element)
		{
			if (!element.IsDeleted)
			{
                ORMModel model = (ORMModel)element;
				Store store = element.Store;
				Partition partition = Partition.FindByAlternateId(store, typeof(InferenceResult));
				if (partition == null)
				{
					partition = new Partition(store);
					partition.AlternateId = typeof(InferenceResult);
				}
//				InferenceResult inferenceResult = new InferenceResult(partition);
//              inferenceResult.Model = (ORMModel)element;
				// UNDONE: Create the rest of the InferenceResult based on the ORMModel instance,
				// plus any filters you want.
                InferredConstraints container = InferredConstraintsTargetORMModel.GetInferredConstraints(model);
                if (container == null)
                {
                    container = new InferredConstraints(partition);
                    container.Model = model;
                }
                else
                {
                    container.SetConstraintCollection.Clear();
                    container.SetComparisonConstraintCollection.Clear();
                    container.SubtypeFactCollection.Clear();
                }

				/*
                ORM2OWLTranslationManager translationManager = new ORM2OWLTranslationManager();
                ORMInferenceGenerator inferenceGenerator = new ORMInferenceGenerator();
                OWLOntology ontology = translationManager.translateToOWL(model);

                OWLReasoner reasoner = inferenceGenerator.getPrecomputedReasoner(ontology);
                OWLClassNode unsatClasses = reasoner.getUnsatisfiableClasses() as OWLClassNode;

                inferenceGenerator.deriveFreshConstraints(model, ontology, reasoner, partition, container, unsatClasses);
                inferenceGenerator.deriveRefinedConstraints(model, ontology, reasoner, partition, container, unsatClasses);
				*/

//                inferenceResult.IsProcessed = true;

			}
		}
		#endregion // Rule Methods
		#region Event Integration
		/// <summary>
		/// Integrate state change event handlers.
		/// </summary>
        public static void ManageModelStateEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
        {
            DomainDataDirectory dataDir = store.DomainDataDirectory;
            DomainClassInfo classInfo = dataDir.FindDomainClass(InferenceResult.DomainClassId);
//			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(InferenceResultAddedEvent), action);
//			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(InferenceResultDeletedEvent), action);
        }
        BackgroundWorker myWorker;
        public BackgroundWorker getWorker()
        { return myWorker; }
        public void setWorker(BackgroundWorker worker)
        { myWorker = worker; }


//        private static void InferenceResultAddedEvent(object sender, ElementAddedEventArgs e)
//        {
//            InferenceResult inferenceResult = (InferenceResult)e.ModelElement;
//            if (!inferenceResult.IsDeleted)
//            {
//                // Add processing for the worker thread.
//                BackgroundWorker worker = new BackgroundWorker();
//                worker.DoWork += new DoWorkEventHandler(InferenceResultProcessor);
//                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProcessingCompleted);
//                inferenceResult.myWorker = worker;
//                worker.RunWorkerAsync(inferenceResult);
//            }
//        }
//        private static void InferenceResultDeletedEvent(object sender, ElementDeletedEventArgs e)
//        {
//            InferenceResult inferenceResult = (InferenceResult)e.ModelElement;
//            BackgroundWorker worker;

//            if (null != (worker = inferenceResult.myWorker))
//            {
//                inferenceResult.myWorker = null;
//                worker.CancelAsync();
//            }
//        }

        public static void startInference(InferenceResult inferenceResult)
        {
            if (!inferenceResult.IsDeleted)
            {
                Store store = inferenceResult.Store;
                RuleManager ruleMgr = store.RuleManager;
                Type ruleType = typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram);
                ruleType = ruleType.Assembly.GetType(ruleType.Namespace + Type.Delimiter + "DiagramCommittingRule");
                try
                {
                    ruleMgr.DisableRule(ruleType);
                    using (Transaction t = store.TransactionManager.BeginTransaction(Resources.InferredConstraints_StartInference_TransactionName))
                    {
                        Partition partition = inferenceResult.Partition;
                        ORMModel model = inferenceResult.Model;
                        InferredConstraints container = InferredConstraintsTargetORMModel.GetInferredConstraints(model);
                        if (container == null)
                        {
                            container = new InferredConstraints(partition);
                            container.Model = model;
                        }
                        else
                        {
                            container.SetConstraintCollection.Clear();
                            container.SetComparisonConstraintCollection.Clear();
                            container.SubtypeFactCollection.Clear();
                        }

						/*
                        ORM2OWLTranslationManager translationManager = new ORM2OWLTranslationManager();
                        ORMInferenceGenerator inferenceGenerator = new ORMInferenceGenerator();
                        OWLOntology ontology = translationManager.translateToOWL(model);

                        OWLReasoner reasoner = inferenceGenerator.getPrecomputedReasoner(ontology);
                        OWLClassNode unsatClasses = reasoner.getUnsatisfiableClasses() as OWLClassNode;

                        inferenceGenerator.deriveFreshConstraints(model, ontology, reasoner, partition, container, unsatClasses);
                        inferenceGenerator.deriveRefinedConstraints(model, ontology, reasoner, partition, container, unsatClasses);
						*/


                        inferenceResult.IsProcessed = true;
                        t.Commit();
                    }
                }
                finally
                {
                    ruleMgr.EnableRule(ruleType);
                }
            }
        }

//        private static void ProcessingCompleted(object sender, RunWorkerCompletedEventArgs e)
//        {
//            //return;

//            Store store;
//            InferenceResult contextInferenceResult; // UNDONE: Temporary placeholder for data object, return real data or attach it to ProcessInferenceResult, which is in the sender object
//            if (!e.Cancelled &&
//                null != (contextInferenceResult = e.Result as InferenceResult) &&
//                null != (store = Utility.ValidateStore(contextInferenceResult.Store)) &&
//                !contextInferenceResult.IsDeleted)
//            {
//                contextInferenceResult.myWorker = null;
//                Action<InferenceResult> doTransaction = startInference;
//                TransactionManager txMgr = store.TransactionManager;
//                // See if we're in a transaction currently and add a temporary TransactionCompleted
//                // event handler to process this later.
//                if (txMgr.InTransaction)
//                {
//                    EventHandler<TransactionEventArgs> transactionComplete = delegate(object completedSender, TransactionEventArgs txE)
//                    {
//                        doTransaction(contextInferenceResult);
//                    };
//                    try
//                    {
//                        // UNDONE: Not sure if we should run this in transactionCompleted, or in ElementEventsEnded after the
//                        // transaction is done.
//                        store.EventManagerDirectory.TransactionCommitted.Add(transactionComplete);
//                        store.EventManagerDirectory.TransactionRolledBack.Add(transactionComplete);
//                    }
//                    finally
//                    {
//                        store.EventManagerDirectory.TransactionCommitted.Remove(transactionComplete);
//                        store.EventManagerDirectory.TransactionRolledBack.Remove(transactionComplete);
//                    }
//                }
//                else
//                {
//                    doTransaction(contextInferenceResult);
//                }
//            }
//        }
		#endregion // Event Integration
		#region Worker Thread
		private static void InferenceResultProcessor(object sender, DoWorkEventArgs e)
		{
			InferenceResult inferenceResult = (InferenceResult)e.Argument;
			e.Result = inferenceResult;
		}
		#endregion // Worker Thread
	};
}