using System;
using System.Drawing;
using System.Windows.Forms;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Modeling;
using org.semanticweb.owlapi.model;
using org.semanticweb.owlapi.reasoner;
using System.ComponentModel;
using org.semanticweb.owlapi.reasoner.impl;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace unibz.ORMInferenceEngine
{
	partial class UnsatisfiableDomain
	{
    	#region Rule Methods
        // We are using new the mechanism of events, not rules
        ///// <summary>
        ///// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
        ///// </summary>
        //private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
        //{
        //    FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateObjectTypeAdded);
        //}
        //private static void DelayValidateObjectTypeAdded(ModelElement element)
        //{
        //    if (!element.IsDeleted)
        //    {
        //        ModelHasObjectType link = (ModelHasObjectType)element;
        //        ObjectType objectType = link.ObjectType;
        //        ORMModel model = link.Model;

        //        UnsatisfiableDomain unsatisfiableDomain;
        //        if (null != (unsatisfiableDomain = UnsatisfiableDomainIsForORMModel.GetUnsatisfiableDomain(model)))
        //        {
        //            unsatisfiableDomain.rebuildUnsatisfiableDomain(model);
        //        }
        //    }
        //}
        // /// <summary>
        // /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
        // /// </summary>
        // private static void ObjectTypeDeletedRule(ElementDeletedEventArgs e)
        // {
        //     FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateObjectTypeDeleted);
        // }
        // private static void DelayValidateObjectTypeDeleted(ModelElement element)
        // {
        //     if (!element.IsDeleted)
        //     {
        //         ModelHasObjectType link = (ModelHasObjectType)element;
        //         ObjectType objectType = link.ObjectType;
        //         ORMModel model = link.Model;

        //         UnsatisfiableDomain unsatisfiableDomain;
        //         if (null != (unsatisfiableDomain = UnsatisfiableDomainIsForORMModel.GetUnsatisfiableDomain(model)))
        //         {
        //             unsatisfiableDomain.rebuildUnsatisfiableDomain(model);
        //         }
        //     }
        // }

        // /// <summary>
        // /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType)
        // /// </summary>
        // private static void FactTypeAddedRule(ElementAddedEventArgs e)
        // {
        //     FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateFactTypeAdded);
        // }
        // private static void DelayValidateFactTypeAdded(ModelElement element)
        // {
        //     if (!element.IsDeleted)
        //     {
        //         ModelHasFactType link = (ModelHasFactType)element;
        //         FactType objectType = link.FactType;
        //         ORMModel model = link.Model;

        //         UnsatisfiableDomain unsatisfiableDomain;
        //         if (null != (unsatisfiableDomain = UnsatisfiableDomainIsForORMModel.GetUnsatisfiableDomain(model)))
        //         {
        //             unsatisfiableDomain.rebuildUnsatisfiableDomain(model);
        //         }
        //     }
        // }
        // /// <summary>
        // /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType)
        // /// </summary>
        // private static void FactTypeDeletedRule(ElementDeletedEventArgs e)
        // {
        //     FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateFactTypeDeleted);
        // }
        // private static void DelayValidateFactTypeDeleted(ModelElement element)
        // {
        //     if (!element.IsDeleted)
        //     {
        //         ModelHasFactType link = (ModelHasFactType)element;
        //         FactType objectType = link.FactType;
        //         ORMModel model = link.Model;

        //         UnsatisfiableDomain unsatisfiableDomain;
        //         if (null != (unsatisfiableDomain = UnsatisfiableDomainIsForORMModel.GetUnsatisfiableDomain(model)))
        //         {
        //             unsatisfiableDomain.rebuildUnsatisfiableDomain(model);
        //         }
        //     }
        // }
        #endregion // Rule Methods


        #region Event Integration
        /// <summary>
        /// Integrate state change event handlers.
        /// </summary>
        public static void ManageModelStateEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
        {
            DomainDataDirectory directory = store.DomainDataDirectory;
            EventHandler<ElementDeletedEventArgs> standardDeleteHandler = new EventHandler<ElementDeletedEventArgs>(ModelElementRemovedEvent);

            DomainClassInfo classInfo;

            //Inferred Unsatisfiable Domain
            classInfo = directory.FindDomainClass(UnsatisfiableObjectType.DomainClassId);
            eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(UnsatisfiableObjectTypeAddedEvent), action);
            eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);
            classInfo = directory.FindDomainClass(UnsatisfiableFactType.DomainClassId);
            eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(UnsatisfiableFactTypeAddedEvent), action);
            eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

        }

        /// <summary>
        /// Survey event handler for addition of an <see cref="UnsatisfiableObjectType"/>
        /// </summary>
        private static void UnsatisfiableObjectTypeAddedEvent(object sender, ElementAddedEventArgs e)
        {
            INotifySurveyElementChanged eventNotify;
            UnsatisfiableObjectType objType = (UnsatisfiableObjectType)e.ModelElement;
            if (!objType.IsDeleted && null != (eventNotify = (e.ModelElement.Store as IORMToolServices).NotifySurveyElementChanged))
            {
                eventNotify.ElementAdded(objType, null);
            }
        }

        /// <summary>
        /// Survey event handler for addition of an <see cref="UnsatisfiableFactType"/>
        /// </summary>
        private static void UnsatisfiableFactTypeAddedEvent(object sender, ElementAddedEventArgs e)
        {
            INotifySurveyElementChanged eventNotify;
            UnsatisfiableFactType factType = (UnsatisfiableFactType)e.ModelElement;
            if (!factType.IsDeleted && null != (eventNotify = (e.ModelElement.Store as IORMToolServices).NotifySurveyElementChanged))
            {
                eventNotify.ElementAdded(factType, null);
            }
        }

        /// <summary>
        /// Standard handler when an element needs to be removed from the model browser
        /// </summary>
        private static void ModelElementRemovedEvent(object sender, ElementDeletedEventArgs e)
        {
            INotifySurveyElementChanged eventNotify;
            ModelElement element = e.ModelElement;
            if (null != element && null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
            {
                eventNotify.ElementDeleted(element);
            }
        }

        #endregion //Event Integration

	};
}