using System;
using System.Drawing;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.Load;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;

namespace unibz.ORMInferenceEngine
{
	sealed partial class ORMInferenceEngineDomainModel :
		//IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>,
		IModelingEventSubscriber
	{
		static ORMInferenceEngineDomainModel()
		{
			NORMAExtensionCompatibilityAttribute.VerifyCompatibility(Assembly.GetExecutingAssembly());
		}

		//Color IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>.GetDynamicColor(ORMDiagramDynamicColor colorRole, SubtypeLink shapeElement, SubtypeFact elementPart)
		//{
		//    return colorRole == ORMDiagramDynamicColor.Constraint ? Color.Red : Color.Empty;
		//}


		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if ((EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.ModelStateEvents) == (reasons & (EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.ModelStateEvents)))
			{
				InferenceResult.ManageModelStateEventHandlers(Store, eventManager, action);
                Hierarchy.ManageModelStateEventHandlers(Store, eventManager, action);
                UnsatisfiableDomain.ManageModelStateEventHandlers(Store, eventManager, action);
            }
            if (0 != (reasons & EventSubscriberReasons.SurveyQuestionEvents))
			{
				ManageSurveyQuestionEventHandlers(eventManager, action);
			}
		}
	};
}