#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Framework;
using Neumont.Tools.ORM.Shell;
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
				ObjectTypeShape objectTypeShape;
				if (null != (objectTypeShape = e.ModelElement as ObjectTypeShape) &&
					e.MetaAttribute.Id == ObjectTypeShape.ExpandRefModeMetaAttributeGuid)
				{
					objectTypeShape.AutoResize();

					ObjectType objectType = objectTypeShape.ModelElement as ObjectType;
					InternalUniquenessConstraint preferredConstraint;
					if (null != (preferredConstraint = objectType.PreferredIdentifier as InternalUniquenessConstraint) &&
						preferredConstraint.RoleCollection[0].RolePlayer.IsValueType)
					{

						bool expandingRefMode = (bool)e.NewValue;
						ORMDiagram parentDiagram = objectTypeShape.Diagram as ORMDiagram;
						Dictionary<ShapeElement, bool> shapeElements = new Dictionary<ShapeElement, bool>();

						// View or Hide FactType
						FactType factType = preferredConstraint.FactType;
						if (!expandingRefMode)
						{
							RemoveShapesFromDiagram(factType, parentDiagram);
						}
						else
						{
							bool fixUpReadings = false;
							ShapeElement shapeOnDiagram;
							if (null == (shapeOnDiagram = parentDiagram.FindShapeForElement(factType)))
							{
								Diagram.FixUpDiagram(objectType.Model, factType);
								shapeOnDiagram = parentDiagram.FindShapeForElement(factType);
								shapeElements.Add(shapeOnDiagram, true);
								fixUpReadings = true;
							}

							if (fixUpReadings)
							{
								foreach (ReadingOrder readingOrder in factType.ReadingOrderCollection)
								{
									Diagram.FixUpDiagram(factType, readingOrder);
								}
							}
						}

						//View or Hide value type
						ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
						if (valueType != null)
						{
							if (expandingRefMode)
							{
								ShapeElement shapeOnDiagram;
								if (null == (shapeOnDiagram = parentDiagram.FindShapeForElement(valueType)))
								{
									Diagram.FixUpDiagram(objectType.Model, valueType);
									shapeOnDiagram = parentDiagram.FindShapeForElement(valueType);
									shapeElements.Add(shapeOnDiagram, true);
								}

								foreach (ValueTypeHasValueConstraint link in valueType.GetElementLinks(ValueTypeHasValueConstraint.ValueTypeMetaRoleGuid))
								{
									FixupValueTypeValueConstraintLink(link, null);
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
								if (expandingRefMode)
								{
									Diagram.FixUpDiagram(objectType.Model, link);
								}
								else
								{
									RemoveShapesFromDiagram(link, parentDiagram);
								}
							}
							foreach (RoleHasValueConstraint link in role.GetElementLinks(RoleHasValueConstraint.RoleMetaRoleGuid))
							{
								if (expandingRefMode)
								{
									FixupRoleValueConstraintLink(link, null);
								}
								else
								{
									FixupValueTypeValueConstraintLink(link, null);
									RemoveShapesFromDiagram(link, parentDiagram);
								}
							}
						}

						parentDiagram.AutoLayoutChildShapes(shapeElements);

					}
				}
			}//end method
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
		#region FactTypeChanged
		[RuleOn(typeof(FactTypeShape), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class FactTypeShapeChanged : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				FactTypeShape fts = e.ModelElement as FactTypeShape;
				if (fts != null)
				{
					if (e.MetaAttribute.Id == FactTypeShape.DisplayRoleNamesMetaAttributeGuid)
					{
						FactType fact = fts.ModelElement as FactType;
						if (fact != null)
						{
							RoleNameShape.SetRoleNameDisplay(fact);
						}
					}
				}
			}
		}
		#endregion // FactTypechanged
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
		#region ConstraintSetChanged fixup
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
				ConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection;
				if (!sequence.IsRemoved &&
					null != (factType = link.RoleCollection.FactType) &&
					!factType.IsRemoved &&
					null != (constraint = sequence.Constraint)
					)
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, true);
				}
			}
		}
		#endregion // ConstraintRoleSequenceRoleRemoved class
		#region ExternalRoleConstraintRemoved class
		/// <summary>
		/// Update the fact type when constraint roles are removed. Used when
		/// an entire role sequence is deleted from a multi-column external fact constraint
		/// that does not also delete the fact constraint.
		/// </summary>
		[RuleOn(typeof(ExternalRoleConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ExternalRoleConstraintRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ExternalRoleConstraint link = e.ModelElement as ExternalRoleConstraint;
				FactType factType;
				MultiColumnExternalFactConstraint factConstraint = link.FactConstraintCollection as MultiColumnExternalFactConstraint;
				if (factConstraint != null &&
					!factConstraint.IsRemoved &&
					(null != (factType = factConstraint.FactTypeCollection)) &&
					!factType.IsRemoved)
				{
					FactTypeShape.ConstraintSetChanged(factType, factConstraint.MultiColumnExternalConstraintCollection, false);
				}
			}
		}
		#endregion // ExternalRoleConstraintRemoved class
		#endregion // ConstraintSetChanged fixup
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
		#region ObjectTypePlaysRoleAdded class
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ObjectTypePlaysRoleAdded : AddRule
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
		#endregion // ObjectTypePlaysRoleAdded class
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
			FactType associatedFact = link.PlayedRoleCollection.FactType;
			if (associatedFact != null)
			{
				ObjectType rolePlayer = link.RolePlayer;
				ORMModel model = rolePlayer.Model;
				if (model != null)
				{
					FactType nestedFact = rolePlayer.NestedFactType;
					if (nestedFact != null)
					{
						Diagram.FixUpDiagram(model, nestedFact);
						Diagram.FixUpDiagram(nestedFact, rolePlayer);
					}
					else
					{
						Diagram.FixUpDiagram(model, rolePlayer);
					}
					Diagram.FixUpDiagram(model, associatedFact);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // ObjectTypePlaysRole fixup
		#region RoleHasValueConstraint fixup
		#region RoleValueConstraintAdded class
		[RuleOn(typeof(RoleHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RoleValueConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
				if (link != null)
				{
					Role r = link.Role;
					FactType factType = r.FactType;
					IList links = factType.GetElementLinks(SubjectHasPresentation.SubjectMetaRoleGuid);
					//If the factType has no presentation elements, it must be hidden. In which case,
					//we need to fixup the ValueTypeValueConstraint with this link.
					if (links.Count > 0)
					{
						FixupRoleValueConstraintLink(link, null);
					}
					else
					{
						FixupValueTypeValueConstraintLink(link, null);
					}
				}
			}
		}
		#endregion // RoleValueConstraintAdded class
		#region DisplayValueConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayRoleValueConstraintFixupListener : DeserializationFixupListener<RoleHasValueConstraint>
		{
			/// <summary>
			/// Create a new DisplayValueConstraintFixupListener
			/// </summary>
			public DisplayRoleValueConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(RoleHasValueConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRoleValueConstraintLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueConstraintFixupListener class
		#region RoleValueConstraintRemoved class
		[RuleOn(typeof(RoleHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class RoleValueConstraintRemoved : RemoveRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					link.PresentationRolePlayers.Clear();
				}
			}
		}
		#endregion // RoleValueConstraintRemoved class
		/// <summary>
		/// Helper function to display role player links.
		/// </summary>
		/// <param name="link">A RoleHasValueConstraint element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupRoleValueConstraintLink(RoleHasValueConstraint link, INotifyElementAdded notifyAdded)
		{
			// Make sure the fact type and link are displayed on the diagram
			RoleValueConstraint roleValueConstraint = link.ValueConstraint;
			Role role = roleValueConstraint.Role;
			FactType factType = role.FactType;

			if (factType != null)
			{
				ORMModel model = factType.Model;
				if(model != null)
				{
					Diagram.FixUpDiagram(model, factType);
					Diagram.FixUpDiagram(factType, roleValueConstraint);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // RoleHasValueConstraint fixup
		#region ValueTypeHasValueConstraint fixup
		#region ValueConstraintAdded class
		[RuleOn(typeof(ValueTypeHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ValueTypeValueConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
				if (link != null)
				{
					FixupValueTypeValueConstraintLink(link, null);
				}
			}
		}
		#endregion // ValueTypeValueConstraintAdded class
		#region DisplayValueTypeValueConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayValueTypeValueConstraintFixupListener : DeserializationFixupListener<ValueTypeHasValueConstraint>
		{
			/// <summary>
			/// Create a new DisplayValueConstraintFixupListener
			/// </summary>
			public DisplayValueTypeValueConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ValueTypeHasValueConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupValueTypeValueConstraintLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueTypeValueConstraintFixupListener class
		#region ValueTypeValueConstraintRemoved class
		[RuleOn(typeof(ValueTypeHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private class ValueTypeValueConstraintRemoved : RemoveRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					link.PresentationRolePlayers.Clear();
				}
			}
		}
		#endregion // ValueTypeValueConstraintRemoved class
		/// <summary>
		/// Helper function to display value type value ranges.
		/// </summary>
		/// <param name="link">A ValueTypeHasValueConstraint element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupValueTypeValueConstraintLink(ValueTypeHasValueConstraint link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			ValueTypeValueConstraint valueTypeValueConstraint = link.ValueConstraint;
			ObjectType objectType = valueTypeValueConstraint.ValueType;
			if (objectType != null)
			{
				ORMModel model = objectType.Model;
				if (model != null)
				{
					Diagram.FixUpDiagram(model, objectType);
					Diagram.FixUpDiagram(objectType, valueTypeValueConstraint);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		/// <summary>
		/// Helper function to display role value ranges on object types with a ref mode.
		/// </summary>
		/// <param name="link">A RoleHasValueConstraint element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupValueTypeValueConstraintLink(RoleHasValueConstraint link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			RoleValueConstraint roleValueConstraint = link.ValueConstraint;
			Role role = roleValueConstraint.Role;
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
					Diagram.FixUpDiagram(model, objectType);
					Diagram.FixUpDiagram(objectType, roleValueConstraint);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // ValueTypeHasValueConstraint fixup
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
									Reading newReading = FactType.GetMatchingReading(remainingOrders, order, roles[0], null, false, false, roles, true);
									if (newReading != null)
									{
										ReadingOrder newOrder = newReading.ReadingOrder;
										if (newOrder != null)
										{
											readingPel.Associate(newOrder);
											return;
										}
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
		#region ParentShapeRemoved class
		[RuleOn(typeof(ParentShapeHasRelativeChildShapes))]
		[RuleOn(typeof(ParentShapeContainsNestedChildShapes))]
		private class ParentShapeRemoved : RemoveRule
		{
			/// <summary>
			/// Deletion of a parent shape should delete the child shape.
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ParentShapeHasRelativeChildShapes linkRelative = e.ModelElement as ParentShapeHasRelativeChildShapes;
				ParentShapeContainsNestedChildShapes linkNested;
				if (linkRelative != null)
				{
					linkRelative.RelativeChildShapes.Remove();
                }
				else if ((linkNested = e.ModelElement as ParentShapeContainsNestedChildShapes) != null)
				{
					linkNested.NestedChildShapes.Remove();
				}
			}
		}
		#endregion // ParentShapeRemoved class
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
		#region LinkConnectsToNodeRemoved class
		/// <summary>
		/// Don't leave links dangling. Remove any link shape that points
		/// to no model element.
		/// </summary>
		[RuleOn(typeof(LinkConnectsToNode), FireTime=TimeToFire.LocalCommit)]
		private class LinkConnectsToNodeRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				LinkConnectsToNode link = e.ModelElement as LinkConnectsToNode;
				LinkShape linkShape = link.Link;
				NodeShape nodeShape = link.Nodes;
				ModelElement backingElement;
				if (nodeShape.IsRemoved &&
					!linkShape.IsRemoved &&
					null != (backingElement = linkShape.ModelElement) &&
					!backingElement.IsRemoved)
				{
					linkShape.Remove();
				}
			}
		}
		#endregion // LinkConnectsToNodeRemoved class
		#region ReadingOrder fixup
		/// <summary>
		/// Add shape elements for reading orders. Used during deserialization fixup
		/// and rules.
		/// </summary>
		/// <param name="link"></param>
		private static void FixupReadingOrderLink(FactTypeHasReadingOrder link)
		{
			ReadingOrder readingOrd = link.ReadingOrderCollection;
			FactType fact = link.FactType;
			Diagram.FixUpDiagram(fact.Model, fact); // Make sure the fact is already there
			Diagram.FixUpDiagram(fact, readingOrd);
		}
		[RuleOn(typeof(FactTypeHasReadingOrder), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class ReadingOrderAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FixupReadingOrderLink(e.ModelElement as FactTypeHasReadingOrder);
			}
		}
		#region DisplayReadingsFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private class DisplayReadingsFixupListener : DeserializationFixupListener<FactTypeHasReadingOrder>
		{
			/// <summary>
			/// Create a new DisplayRolePlayersFixupListener
			/// </summary>
			public DisplayReadingsFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add reading shapes when possible
			/// </summary>
			/// <param name="element">An FactTypeHasReadingOrder instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(FactTypeHasReadingOrder element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupReadingOrderLink(element);
			}
		}
		#endregion // DisplayReadingsFixupListener class
		#endregion // ReadingOrder fixup
		#region RoleName fixup
		/// <summary>
		/// Add shape elements for role names. Used during deserialization fixup
		/// and rules.
		/// </summary>
		[RuleOn(typeof(Role), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class RoleChange : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == NamedElement.NameMetaAttributeGuid)
				{
						Role role = (Role)e.ModelElement;
						if (!role.IsRemoved)
						{
							if (string.IsNullOrEmpty(role.Name))
							{
								RoleNameShape.RemoveRoleNameShapeFromRole(role);
							}
							else
							{
								Diagram.FixUpDiagram(role.FactType, role);
								if (OptionsPage.CurrentRoleNameDisplay == RoleNameDisplay.Off)
								{
									foreach (PresentationElement element in role.FactType.PresentationRolePlayers)
									{
										FactTypeShape fts = element as FactTypeShape;
										if (fts != null
											&& fts.DisplayRoleNames == DisplayRoleNames.UserDefault)
										{
											RoleNameShape.SetRoleNameDisplay(role.FactType);
										}
									}
								}
							}
						}
				}
			}
		}
		#endregion // RoleName fixup
		#region DisplayRolePlayersFixupListener class
		/// <summary>
		/// A fixup class to display role name
		/// </summary>
		private class DisplayRoleNameFixupListener : DeserializationFixupListener<Role>
		{
			/// <summary>
			/// Create a new DisplayRoleNameFixupListener
			/// </summary>
			public DisplayRoleNameFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add role name when possible
			/// </summary>
			/// <param name="role">A Role instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(Role role, Store store, INotifyElementAdded notifyAdded)
			{
				RoleNameShape.SetRoleNameDisplay(role.FactType);
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#endregion // View Fixup Rules
	}
}
