using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ORMDiagram
	{
		#region View Fixup Rules
		[RuleOn(typeof(ModelHasObjectType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ObjectTypedAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
				if (link != null)
				{
					ObjectType objectType = link.ObjectTypeCollection;
					if (objectType.NestedFactType == null) // Otherwise, fix up with the fact type
					{
						Diagram.FixUpDiagram(link.Model, objectType);
					}
				}
			}
		}
		[RuleOn(typeof(ModelHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class FactTypedAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasFactType link = e.ModelElement as ModelHasFactType;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.FactTypeCollection);
				}
			}
		}
		[RuleOn(typeof(ModelHasConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ExternalConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasConstraint link = e.ModelElement as ModelHasConstraint;
				if (link != null &&
					link.ConstraintCollection is ExternalConstraint)
				{
					Diagram.FixUpDiagram(link.Model, link.ConstraintCollection);
				}
			}
		}
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class RoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				if (link != null)
				{
					// UNDONE: We'll need to do something to the fact type, but not
					// this. The role is represented as a ShapeSubField inside the Fact object
					//Diagram.FixUpDiagram(link.FactType, link.RoleCollection);
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RolePlayerAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				if (link != null)
				{
					// Make sure the object type, fact type, and link
					// are displayed on the diagram
					ObjectType rolePlayer = link.RolePlayer;
					FactType nestedType = rolePlayer.NestedFactType;
					Role playedRole = link.PlayedRoleCollection;
					FactType associatedFact = playedRole.FactType;
					if (associatedFact != null)
					{
						ORMModel model = rolePlayer.Model;
						if (model != null)
						{
							Debug.Assert(model == associatedFact.Model);
							Diagram.FixUpDiagram(model, (nestedType == null) ? rolePlayer as ModelElement : nestedType);
							Diagram.FixUpDiagram(model, associatedFact);
							Diagram.FixUpDiagram(model, link);
						}

					}
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ObjectTypePlaysRoleRemoved : RemoveRule
		{
			/// <summary>
			/// Remove presentation elements when the associated RolePlayer link is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					link.PresentationRolePlayers.Clear();
				}
			}
		}
		[RuleOn(typeof(ExternalFactConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ExternalFactConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ExternalFactConstraint link = e.ModelElement as ExternalFactConstraint;
				if (link != null)
				{
					// Make sure the object type, fact type, and link
					// are displayed on the diagram
					ExternalConstraint constraint = link.ExternalConstraintCollection;
					FactType factType = link.FactTypeCollection;
					if (factType != null)
					{
						ORMModel model = factType.Model;
						if (model != null)
						{
							Debug.Assert(model == constraint.Model);
							Diagram.FixUpDiagram(model, constraint);
							Diagram.FixUpDiagram(model, factType);
							Diagram.FixUpDiagram(model, link);
						}
					}
				}
			}
		}
		[RuleOn(typeof(SubjectHasPresentation))]
		private class PresentationLinkRemoved : RemoveRule
		{
			/// <summary>
			/// Clearing the PresentationRolePlayers collection does not automatically
			/// remove the PELs (propagatedelete is false). Add this rule in code here.
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				SubjectHasPresentation link = e.ModelElement as SubjectHasPresentation;
				if (link != null)
				{
					ShapeElement presenter = link.Presentation as ShapeElement;
					if (presenter != null) // Option role, may not be there
					{
						presenter.Invalidate();
						presenter.Remove();
					}
				}
			}
		}
		#endregion // View Fixup Rules
	}
}