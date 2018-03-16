using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace unibz.ORMInferenceEngine
{
	partial class ORM2OWLTranslationManager
	{
		UniquenessConstraint CreateInferredUniqueness(InferredConstraints container, string uniq)
		{
			//Vedere di farlo con InferredUniquenessConstraint nel caso
			Partition outputPartition = container.Partition;
			ORMModel model = container.Model;

			UniquenessConstraint myInternalUniquenessConstraint = UniquenessConstraint.CreateInternalUniquenessConstraint(outputPartition, outputPartition.DomainDataDirectory.GetDomainClass(InferredUniquenessConstraint.DomainClassId));
			myInternalUniquenessConstraint.Name = "Uniqueness " + mandOrUniqForORM(uniq);
			new SetConstraintIsInferred(container, myInternalUniquenessConstraint);

			return myInternalUniquenessConstraint;
		}
	}
}