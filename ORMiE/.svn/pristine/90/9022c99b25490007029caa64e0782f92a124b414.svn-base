using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		void CreateInferredExhaustiveTypesBackup(InferredConstraints container, string father, java.util.Set sons)
		{
			//Vedere di farlo con InferredUniquenessConstraint nel caso
			Partition outputPartition = container.Partition;
			ORMModel model = container.Model;

			MandatoryConstraint myInternalMandatoryConstraint = MandatoryConstraint.CreateSimpleMandatoryConstraint(outputPartition, outputPartition.DomainDataDirectory.GetDomainClass(InferredMandatoryConstraint.DomainClassId));
			myInternalMandatoryConstraint.Name = "Covering: " + father + " :" + sons.ToString();
			new SetConstraintIsInferred(container, myInternalMandatoryConstraint);

		}
	}
}