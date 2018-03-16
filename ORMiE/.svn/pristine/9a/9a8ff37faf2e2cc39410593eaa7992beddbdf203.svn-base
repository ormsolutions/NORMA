using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		void CreateInferredMandatory(InferredConstraints container, string mand)
		{
			Partition outputPartition = container.Partition;
			ORMModel model = container.Model;

			MandatoryConstraint myInternalMandatoryConstraint = MandatoryConstraint.CreateSimpleMandatoryConstraint(outputPartition, outputPartition.DomainDataDirectory.GetDomainClass(InferredMandatoryConstraint.DomainClassId));
			myInternalMandatoryConstraint.Name = "Mandatory " + mandOrUniqForORM(mand);
			new SetConstraintIsInferred(container, myInternalMandatoryConstraint);

			//return myInternalMandatoryConstraint;
		}

		
	}
}