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
		#region ModelHasObjectType fixup
		#region ObjectTypedAdded class
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
		#endregion // ObjectTypedAdded class
		#endregion // ModelHasObjectType fixup
		#region ModelHasFactType fixup
		#region FactTypeAdded class
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
		#endregion // FactTypeAdded class
		#endregion // ModelHasFactType fixup
		#region ModelHasConstraint fixup
		#region MultiColumnExternalConstraintAdded class
		[RuleOn(typeof(ModelHasMultiColumnExternalConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class MultiColumnExternalConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasMultiColumnExternalConstraint link = e.ModelElement as ModelHasMultiColumnExternalConstraint;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.MultiColumnExternalConstraintCollection);
				}
			}
		}
		#endregion // MultiColumnExternalConstraintAdded class
		#region SingleColumnExternalConstraintAdded class
		[RuleOn(typeof(ModelHasSingleColumnExternalConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SingleColumnExternalConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasSingleColumnExternalConstraint link = e.ModelElement as ModelHasSingleColumnExternalConstraint;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.SingleColumnExternalConstraintCollection);
				}
			}
		}
		#endregion // SingleColumnExternalConstraintAdded class
		#region InternalConstraint fixup
		#region FactTypeHasInternalConstraintAdded class
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class FactTypeHasInternalConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link;
				FactType fact;
				InternalConstraint constraint;
				if (null != (link = e.ModelElement as FactTypeHasInternalConstraint) &&
					null != (fact = link.FactType) &&
					null != (constraint = link.InternalConstraintCollection))
				{
					foreach (PresentationElement pel in fact.PresentationRolePlayers)
					{
						FactTypeShape factShape = pel as FactTypeShape;
						if (factShape != null)
						{
							factShape.ConstraintSetChanged(constraint);
						}
					}
				}
			}
		}
		#endregion // FactTypeHasInternalConstraintAdded class
		#region FactTypeHasInternalConstraintRemoved class
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class FactTypeHasInternalConstraintRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasInternalConstraint link;
				FactType fact;
				InternalConstraint constraint;
				if (null != (link = e.ModelElement as FactTypeHasInternalConstraint) &&
					null != (fact = link.FactType) &&
					null != (constraint = link.InternalConstraintCollection))
				{
					if (!fact.IsRemoved)
					{
						foreach (PresentationElement pel in fact.PresentationRolePlayers)
						{
							FactTypeShape factShape = pel as FactTypeShape;
							if (factShape != null)
							{
								factShape.ConstraintSetChanged(constraint);
							}
						}
					}
				}
			}
		}
		#endregion // FactTypeHasInternalConstraintRemoved class
		#region PrimaryIdentifierAdded class
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class PrimaryIdentifierAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link;
				InternalUniquenessConstraint constraint;
				if (null != (link = e.ModelElement as EntityTypeHasPreferredIdentifier) &&
					null != (constraint = link.PreferredIdentifier as InternalUniquenessConstraint))
				{
					foreach (PresentationElement pel in constraint.FactType.PresentationRolePlayers)
					{
						FactTypeShape factShape = pel as FactTypeShape;
						if (factShape != null)
						{
							factShape.Invalidate(true);
						}
					}
				}
			}
		}
		#endregion // PrimaryIdentifierAdded class
		#region PrimaryIdentifierRemoved class
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class PrimaryIdentifierRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link;
				InternalUniquenessConstraint constraint;
				if (null != (link = e.ModelElement as EntityTypeHasPreferredIdentifier) &&
					null != (constraint = link.PreferredIdentifier as InternalUniquenessConstraint))
				{
					if (!constraint.IsRemoved)
					{
						foreach (PresentationElement pel in constraint.FactType.PresentationRolePlayers)
						{
							FactTypeShape factShape = pel as FactTypeShape;
							if (factShape != null)
							{
								factShape.Invalidate(true);
							}
						}
					}
				}
			}
		}
		#endregion // PrimaryIdentifierRemoved class
		#endregion // InternalConstraint fixup
		#endregion // ModelHasConstraint fixup
		#region FactTypeHasRole fixup
		#region RoleAdded class
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class RoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				foreach (PresentationElement pel in factType.PresentationRolePlayers)
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						shape.AutoResize();
					}
				}
			}
		}
		#endregion // RoleAdded class
		#region RoleRemoved class
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class RoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType factType = link.FactType;
				if (!factType.IsRemoved)
				{
					foreach (PresentationElement pel in factType.PresentationRolePlayers)
					{
						FactTypeShape shape = pel as FactTypeShape;
						if (shape != null)
						{
							shape.AutoResize();
						}
					}
				}
			}
		}
		#endregion // RoleRemoved class
		#endregion // FactTypeHasRole fixup
		#region ObjectTypePlaysRole fixup
		#region RolePlayerAdded class
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RolePlayerAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				if (link != null)
				{
					FixupRolePlayerLink(link);
				}
			}
		}
		#endregion // RolePlayerAdded class
		#region DisplayRolePlayersFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayRolePlayersFixupListener : DeserializationFixupListener<ObjectTypePlaysRole>
		{
			/// <summary>
			/// Create a new DisplayRolePlayersFixupListener
			/// </summary>
			public DisplayRolePlayersFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add role player links when possible
			/// </summary>
			/// <param name="element">An ObjectTypePlaysRole instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ObjectTypePlaysRole element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRolePlayerLink(element);
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#region ObjectTypePlaysRoleRemoved class
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
		#endregion // ObjectTypePlaysRoleRemoved class
		/// <summary>
		/// Helper function to display role player links.
		/// </summary>
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupRolePlayerLink(ObjectTypePlaysRole link)
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
		#endregion // ObjectTypePlaysRole fixup
		#region ExternalFactConstraint fixup
		#region ExternalFactConstraintAdded class
		[RuleOn(typeof(ExternalFactConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ExternalFactConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ExternalFactConstraint link = e.ModelElement as ExternalFactConstraint;
				if (link != null)
				{
					FixupExternalConstraintLink(link);
				}
			}
		}
		#endregion // ExternalFactConstraintAdded class
		#region ExternalFactConstraintRemoved class
		[RuleOn(typeof(ExternalFactConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ExternalFactConstraintRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				IFactConstraint link;
				IConstraint constraint;
				if (null != (link = e.ModelElement as IFactConstraint) &&
					null != (constraint = link.Constraint))
				{
					FactType fact = link.FactType;
					if (!fact.IsRemoved)
					{
						foreach (PresentationElement pel in fact.PresentationRolePlayers)
						{
							FactTypeShape factShape = pel as FactTypeShape;
							if (factShape != null)
							{
								factShape.ConstraintSetChanged(constraint);
							}
						}
					}
				}
			}
		}
		#endregion // ExternalFactConstraintRemoved class
		#region DisplayExternalConstraintLinksFixupListener class
		/// <summary>
		/// A fixup class to display external constraint links for
		/// when both endpoints are represented on the diagram
		/// </summary>
		private class DisplayExternalConstraintLinksFixupListener : DeserializationFixupListener<ExternalFactConstraint>
		{
			/// <summary>
			/// Create a new DisplayExternalConstraintLinksFixupListener
			/// </summary>
			public DisplayExternalConstraintLinksFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add external fact constraint links to the diagram
			/// </summary>
			/// <param name="element">A ExternalFactConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ExternalFactConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupExternalConstraintLink(element);
			}
		}
		#endregion // DisplayExternalConstraintLinksFixupListener class
		/// <summary>
		/// Helper function to display external constraint links.
		/// </summary>
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupExternalConstraintLink(ExternalFactConstraint link)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			IFactConstraint ifc = link as IFactConstraint;
			IConstraint constraint = ifc.Constraint;
			FactType factType = ifc.FactType;
			if (factType != null)
			{
				ORMModel model = factType.Model;
				if (model != null)
				{
					Debug.Assert(model == constraint.Model);
					Diagram.FixUpDiagram(model, constraint as ModelElement);
					Diagram.FixUpDiagram(model, factType);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // ExternalFactConstraint fixup
		#region SubjectHasPresentation fixup
		#region PresentationLinkRemoved class
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
		#endregion // PresentationLinkRemoved class
		#region EliminateOrphanedShapesFixupListener class
		/// <summary>
		/// A fixup class to remove orphaned pels
		/// </summary>
		private class EliminateOrphanedShapesFixupListener : DeserializationFixupListener<PresentationElement>
		{
			/// <summary>
			/// Create a new EliminateOrphanedShapesFixupListener
			/// </summary>
			public EliminateOrphanedShapesFixupListener() : base((int)ORMDeserializationFixupPhase.RemoveOrphanedPresentationElements)
			{
			}
			/// <summary>
			/// Remove all orphaned pels
			/// </summary>
			/// <param name="element">A PresentationElement instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(PresentationElement element, Store store, INotifyElementAdded notifyAdded)
			{
				ModelElement backingElement = element.ModelElement;
				if (backingElement == null || backingElement.IsRemoved)
				{
					element.Remove();
				}
			}
		}
		#endregion // EliminateOrphanedShapesFixupListener class
		#endregion // SubjectHasPresentation fixup
		#endregion // View Fixup Rules
	}
}