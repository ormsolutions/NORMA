using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		/// <summary>
		/// Covers both internal uniqueness and simple mandatory constraints.
		/// </summary>
		private partial class ConstraintRule
		{
			[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
			private partial class ConstraintAddRule : AddRule
			{
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					IConstraint constraint = null;
					if (IsValidConstraintType((ConstraintRoleSequenceHasRole)e.ModelElement, ref constraint))
					{
						AddTransactedModelElement((ModelElement)constraint, OialModelElementAction.Add);

						// HACK: we are not guarenteed that this constraint has a model attached to it.
						if (constraint.Model != null)
						{
							ORMCoreDomainModel.DelayValidateElement(constraint.Model, AbstractionModelIsForORMModel.DelayValidateModel);
						}
					}
				}
			}

			//[RuleOn(typeof(SetComparisonConstraintHasRoleSequence))]
			//private partial class SetContraintAddRule : AddRule
			//{
			//    public override void ElementAdded(ElementAddedEventArgs e)
			//    {
			//    }
			//}

			[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
			private partial class ConstraintChangeRule : ChangeRule
			{
				public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
				{
					IConstraint constraint = null;
					if (IsValidConstraintType((ConstraintRoleSequenceHasRole)e.ModelElement, ref constraint))
					{
						AddTransactedModelElement((ModelElement)constraint, OialModelElementAction.Change);
						ORMCoreDomainModel.DelayValidateElement(constraint.Model, AbstractionModelIsForORMModel.DelayValidateModel);
					}
				}
			}

			[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
			private partial class ConstraintDeleteRule : DeleteRule
			{
				public override void ElementDeleted(ElementDeletedEventArgs e)
				{
					IConstraint constraint = null;
					if (IsValidConstraintType((ConstraintRoleSequenceHasRole)e.ModelElement, ref constraint))
					{
						AddTransactedModelElement((ModelElement)constraint, OialModelElementAction.Delete);
						ORMCoreDomainModel.DelayValidateElement(constraint.Model, AbstractionModelIsForORMModel.DelayValidateModel);
					}
				}
			}
		}
	}
}
