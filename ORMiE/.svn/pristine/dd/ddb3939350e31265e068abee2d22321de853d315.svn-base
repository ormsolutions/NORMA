using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		void CreateInferredSubtypeFact(InferredConstraints container, string subtypeName, string supertypeName)
		//InferredSubtypeFact CreateInferredSubtypeFact(InferredConstraints container, string subtypeName, string supertypeName)
		{
			Partition outputPartition = container.Partition;
			ORMModel model = container.Model;

			// UNDONE: Name lookup is not 100% reliable. It is more accurate to send an identifier to the ORMModel.Store.DefaultPartition.ElementDirectory
			ObjectType subObjType = (ObjectType)model.ObjectTypesDictionary.GetElement(subtypeName).FirstElement;
			ObjectType superObjType = (ObjectType)model.ObjectTypesDictionary.GetElement(supertypeName).FirstElement;

			if (subObjType == null || superObjType == null)
			{
				//return null;
				return;
			}

			// This is a straight copy of SubtypeFact.Create in the NORMA code without alternate owner support. We
			// aren't creating object types with alternate owners, so the NORMA code will not automatically create
			// the subtype fact in an alternate partition. The fact type initialization (roles, uniqueness and mandatory
			// constraints) is done by the InitializeInferredSubtypeFact rule.
			
			InferredSubtypeFact subtypeFact = new InferredSubtypeFact(container.Partition);
			new SubtypeFactIsInferred(container, subtypeFact);

			
			subtypeFact.Subtype = subObjType;
			subtypeFact.Supertype = superObjType;
			if (subObjType.IsValueType)
			{
				subtypeFact.ProvidesPreferredIdentifier = true;
			}
			//subtypeFact.Name = subtypeFact.Subtype.Name + " \u2286 " + subtypeFact.Supertype.Name;
            
			//return subtypeFact;

		}
	}
}