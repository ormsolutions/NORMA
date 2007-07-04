using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region Constraint rules
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint)
		/// </summary>
		private static void ConstraintAddRule(ElementAddedEventArgs e)
		{
			ModelHasSetConstraint link = (ModelHasSetConstraint)e.ModelElement;
			SetConstraint constraint = link.SetConstraint;
			if (IsValidConstraintType(constraint))
			{
				AddTransactedModelElement(constraint, OialModelElementAction.Add);
				FrameworkDomainModel.DelayValidateElement(link.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint)
		/// </summary>
		private static void ConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			SetConstraint constraint = (SetConstraint)e.ModelElement;
			if (IsValidConstraintType(constraint))
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == SetConstraint.NameDomainPropertyId || attributeId == SetConstraint.ModalityDomainPropertyId)
				{
					AddTransactedModelElement(constraint, OialModelElementAction.Change);
					FrameworkDomainModel.DelayValidateElement(constraint.Model, AbstractionModelIsForORMModel.DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint)
		/// </summary>
		private static void ConstraintDeleteRule(ElementDeletedEventArgs e)
		{
			ModelHasSetConstraint link = (ModelHasSetConstraint)e.ModelElement;
			ORMModel model = link.Model;
			SetConstraint constraint;
			if (!model.IsDeleted && IsValidConstraintType((constraint = link.SetConstraint)))
			{
				AddTransactedModelElement(constraint, OialModelElementAction.Delete);
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleAddRule(ElementAddedEventArgs e)
		{
			IConstraint constraint = ((ConstraintRoleSequenceHasRole)e.ModelElement).ConstraintRoleSequence.Constraint;
			if (IsValidConstraintType(constraint))
			{
				AddTransactedModelElement((ModelElement)constraint, OialModelElementAction.Change);
				ORMModel model = constraint.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleDeletingRule(ElementDeletingEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint != null &&
				!constraint.IsDeleting &&
				IsValidConstraintType(constraint))
			{
				AddTransactedModelElement(constraint, OialModelElementAction.Change);
				ORMModel model = constraint.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		#endregion // Constraint rules
	}
}
