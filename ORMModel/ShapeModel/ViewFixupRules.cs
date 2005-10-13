using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Framework;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeModel
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
		#region ObjectTypeChangeRule class
		[RuleOn(typeof(ObjectTypeShape), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ObjectTypeShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ObjectTypeShape objectTypeShape = e.ModelElement as ObjectTypeShape;
				if (objectTypeShape != null)
				{
					Guid attributeId = e.MetaAttribute.Id;
					#region ExpandRefMode
					if (attributeId == ObjectTypeShape.ExpandRefModeMetaAttributeGuid)
					{
						bool turnOn = (bool)e.NewValue;
						ObjectType objectType =  objectTypeShape.ModelElement as ObjectType;
						objectTypeShape.AutoResize();
						InternalUniquenessConstraint preferredConstraint = objectType.PreferredIdentifier as InternalUniquenessConstraint;
						ORMDiagram parentDiagram = objectTypeShape.Diagram as ORMDiagram;
						Dictionary<ShapeElement, bool> shapeElements = new Dictionary<ShapeElement, bool>();

						if (preferredConstraint != null)
						{
							// View or Hide FactType
							FactType factType = preferredConstraint.FactType;
							if (turnOn)
							{
								Diagram.FixUpDiagram(objectType.Model, factType);
								foreach (ReadingOrder readingOrder in factType.ReadingOrderCollection)
								{
									Diagram.FixUpDiagram(factType, readingOrder);
								}
								shapeElements.Add(parentDiagram.FindShapeForElement(factType), true);
							}
							else
							{
								RemoveShapesFromDiagram(factType, parentDiagram);
							}
							//View or Hide value type
							ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
							if (valueType != null)
							{
								if (turnOn)
								{
									bool moveValueType = true;
									if (parentDiagram.FindShapeForElement(valueType) != null)
									{
										moveValueType = false;
									}
									Diagram.FixUpDiagram(objectType.Model, valueType);
									shapeElements.Add(parentDiagram.FindShapeForElement(valueType), moveValueType);
									foreach (ValueTypeHasValueRangeDefinition link in valueType.GetElementLinks(ValueTypeHasValueRangeDefinition.ValueTypeMetaRoleGuid))
									{
										FixupValueTypeValueRangeDefinitionLink(link, null);
									}
								}
								else
								{
									if (!objectType.ReferenceModeSharesValueType || // Easy check first
										!parentDiagram.ShouldDisplayObjectType(valueType)) // More involved check second
									{
										RemoveShapesFromDiagram(valueType, parentDiagram);
									}
								}
							}
							//View or Hide ObjectTypePlaysRole links
							foreach (Role role in factType.RoleCollection)
							{
								foreach (ObjectTypePlaysRole link in role.GetElementLinks(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid))
								{
									if (turnOn)
									{
										Diagram.FixUpDiagram(objectType.Model, link);
									}
									else
									{
										RemoveShapesFromDiagram(link, parentDiagram);
									}
								}
								foreach (RoleHasValueRangeDefinition link in role.GetElementLinks(RoleHasValueRangeDefinition.RoleMetaRoleGuid))
								{
									if (turnOn)
									{
										FixupRoleValueRangeDefinitionLink(link, null);
									}
									else
									{
										FixupValueTypeValueRangeDefinitionLink(link, null);
										RemoveShapesFromDiagram(link, parentDiagram);
									}
								}
							}
							parentDiagram.AutoLayoutChildShapes(shapeElements);
						}
					}
					#endregion //ExpandRefMode
				}
			}
			/// <summary>
			/// Helper function to remove shapes on the diagram for a specific element.
			/// All child shapes will also be removed.
			/// </summary>
			private void RemoveShapesFromDiagram(ModelElement element, Diagram diagram)
			{
				PresentationElementMoveableCollection pels = element.PresentationRolePlayers;
				int pelCount = pels.Count;
				for (int i = pelCount - 1; i >= 0; --i) // Walk backwards so we can safely remove
				{
					ShapeElement shape = pels[i] as ShapeElement;
					if (shape != null && object.ReferenceEquals(shape.Diagram, diagram))
					{
						ClearChildShapes(shape.NestedChildShapes);
						ClearChildShapes(shape.RelativeChildShapes);
						shape.Remove();
					}
				}
			}
			/// <summary>
			/// Helper function to recursively delete child shapes. Used by RemoveShapesFromDiagram.
			/// </summary>
			private void ClearChildShapes(ShapeElementMoveableCollection shapes)
			{
				int count = shapes.Count;
				if (count > 0)
				{
					for (int i = count - 1; i >= 0; --i) // Walk backwards so we can safely remove the shape
					{
						ShapeElement shape = shapes[i];
						ClearChildShapes(shape.NestedChildShapes);
						ClearChildShapes(shape.RelativeChildShapes);
						shape.Remove();
					}
				}
			}
		}
		#endregion // ObjectTypeShapeChangeRule class
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
					FactTypeShape.ConstraintSetChanged(fact, constraint, false);
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
						FactTypeShape.ConstraintSetChanged(fact, constraint, false);
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
		#region ConstraintRoleSequenceRoleAdded class
		/// <summary>
		/// Update the fact type when constraint roles are removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ConstraintRoleSequenceRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				FactType factType;
				IConstraint constraint;
				if (null != (factType = link.RoleCollection.FactType) &&
					null != (constraint = link.ConstraintRoleSequenceCollection.Constraint))
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, true);
				}
			}
		}
		#endregion // ConstraintRoleSequenceRoleAdded class
		#region ConstraintRoleSequenceRoleRemoved class
		/// <summary>
		/// Update the fact type when constraint roles are removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ConstraintRoleSequenceRoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				FactType factType;
				IConstraint constraint;
				ConstraintRoleSequence sequence;
				if (null != (factType = link.RoleCollection.FactType) &&
					!factType.IsRemoved &&
					null != (sequence = link.ConstraintRoleSequenceCollection) &&
					!sequence.IsRemoved &&
					null != (constraint = sequence.Constraint)
					)
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, true);
				}
			}
		}
		#endregion // ConstraintRoleSequenceRoleRemoved class
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
		#region RoleHasValueRangeDefinition fixup
		#region RoleValueRangeDefinitionAdded class
		[RuleOn(typeof(RoleHasValueRangeDefinition), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RoleValueRangeDefinitionAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				RoleHasValueRangeDefinition link = e.ModelElement as RoleHasValueRangeDefinition;
				if (link != null)
				{
					Role r = link.Role;
					FactType factType = r.FactType;
					IList links = factType.GetElementLinks(SubjectHasPresentation.SubjectMetaRoleGuid);
					//If the factType has no presentation elements, it must be hidden. In which case,
					//we need to fixup the ValueTypeValueRangeDefinition with this link.
					if (links.Count > 0)
					{
						FixupRoleValueRangeDefinitionLink(link, null);
					}
					else
					{
						FixupValueTypeValueRangeDefinitionLink(link, null);
					}
				}
			}
		}
		#endregion // RoleValueRangeDefinitionAdded class
		#region DisplayValueRangeDefinitionFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayRoleValueRangeDefinitionFixupListener : DeserializationFixupListener<RoleHasValueRangeDefinition>
		{
			/// <summary>
			/// Create a new DisplayValueRangeDefinitionFixupListener
			/// </summary>
			public DisplayRoleValueRangeDefinitionFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueRangeDefinition instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(RoleHasValueRangeDefinition element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRoleValueRangeDefinitionLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueRangeDefinitionFixupListener class
		#region RoleValueRangeDefinitionRemoved class
		[RuleOn(typeof(RoleHasValueRangeDefinition), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RoleValueRangeDefinitionRemoved : RemoveRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				RoleHasValueRangeDefinition link = e.ModelElement as RoleHasValueRangeDefinition;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					link.PresentationRolePlayers.Clear();
				}
			}
		}
		#endregion // RoleValueRangeDefinitionRemoved class
		/// <summary>
		/// Helper function to display role player links.
		/// </summary>
		/// <param name="link">A RoleHasValueRangeDefinition element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupRoleValueRangeDefinitionLink(RoleHasValueRangeDefinition link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			RoleValueRangeDefinition roleValueRangeDefn = link.ValueRangeDefinition;
			Role role = roleValueRangeDefn.Role;
			FactType factType = role.FactType;
			if (factType != null)
			{
				ORMModel model = factType.Model;
				if (model != null)
				{
					if (notifyAdded == null) // These elements will already exist during fixup
					{
						Diagram.FixUpDiagram(model, factType);
						Diagram.FixUpDiagram(factType, roleValueRangeDefn);
					}
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // RoleHasValueRangeDefinition fixup
		#region ValueTypeHasValueRangeDefinition fixup
		#region ValueRangeDefinitionAdded class
		[RuleOn(typeof(ValueTypeHasValueRangeDefinition), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ValueTypeValueRangeDefinitionAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueRangeDefinition link = e.ModelElement as ValueTypeHasValueRangeDefinition;
				if (link != null)
				{
					FixupValueTypeValueRangeDefinitionLink(link, null);
				}
			}
		}
		#endregion // ValueTypeValueRangeDefinitionAdded class
		#region DisplayValueTypeValueRangeDefinitionFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayValueTypeValueRangeDefinitionFixupListener : DeserializationFixupListener<ValueTypeHasValueRangeDefinition>
		{
			/// <summary>
			/// Create a new DisplayValueRangeDefinitionFixupListener
			/// </summary>
			public DisplayValueTypeValueRangeDefinitionFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueRangeDefinition instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ValueTypeHasValueRangeDefinition element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupValueTypeValueRangeDefinitionLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueTypeValueRangeDefinitionFixupListener class
		#region ValueTypeValueRangeDefinitionRemoved class
		[RuleOn(typeof(ValueTypeHasValueRangeDefinition), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ValueTypeValueRangeDefinitionRemoved : RemoveRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ValueTypeHasValueRangeDefinition link = e.ModelElement as ValueTypeHasValueRangeDefinition;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					link.PresentationRolePlayers.Clear();
				}
			}
		}
		#endregion // ValueTypeValueRangeDefinitionRemoved class
		/// <summary>
		/// Helper function to display value type value ranges.
		/// </summary>
		/// <param name="link">A ValueTypeHasValueRangeDefinition element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupValueTypeValueRangeDefinitionLink(ValueTypeHasValueRangeDefinition link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			ValueTypeValueRangeDefinition valueTypeValueRangeDefn = link.ValueRangeDefinition;
			ObjectType objectType = valueTypeValueRangeDefn.ValueType;
			if (objectType != null)
			{
				ORMModel model = objectType.Model;
				if (model != null)
				{
					if (notifyAdded == null)
					{
						Diagram.FixUpDiagram(objectType, valueTypeValueRangeDefn);
						Diagram.FixUpDiagram(model, objectType);
					}
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		/// <summary>
		/// Helper function to display value type value ranges.
		/// </summary>
		/// <param name="link">A ValueTypeHasValueRangeDefinition element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupValueTypeValueRangeDefinitionLink(RoleHasValueRangeDefinition link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			RoleValueRangeDefinition roleValueRangeDefn = link.ValueRangeDefinition;
			Role role = roleValueRangeDefn.Role;
			FactType factType = role.FactType;
			ObjectType objectType = null;
			foreach (Role r in factType.RoleCollection)
			{
				if (!Object.ReferenceEquals(r, role))
				{
					objectType = r.RolePlayer;
				}
			}
			if (objectType != null)
			{
				ORMModel model = objectType.Model;
				if (model != null)
				{
					if (notifyAdded == null)
					{
						Diagram.FixUpDiagram(objectType, roleValueRangeDefn);
						Diagram.FixUpDiagram(model, objectType);
					}
					Diagram.FixUpDiagram(model, link);
				}
			}
		}

		#endregion // ValueTypeHasValueRangeDefinition fixup
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
						FactTypeShape.ConstraintSetChanged(fact, constraint, false);
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
			// Make sure the constraint, fact type, and link
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
						// If the presenter is a ReadingShape, then see if we
						// can attach it to another ReadingOrder instead of
						// removing it altogether
						ReadingShape readingPel = presenter as ReadingShape;
						if (readingPel != null)
						{
							ReadingOrder order = (ReadingOrder)link.Subject;
							FactType fact = order.FactType;
							if (fact != null && !fact.IsRemoved)
							{
								ReadingOrderMoveableCollection remainingOrders = fact.ReadingOrderCollection;
								if (remainingOrders.Count != 0)
								{
									RoleMoveableCollection roles = fact.RoleCollection;
									ReadingOrder newOrder = FactType.GetMatchingReadingOrder(remainingOrders, order, roles[0], null, false, false, roles, true);
									if (newOrder != null)
									{
										readingPel.Associate(newOrder);
										return;
									}
								}
							}
						}
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