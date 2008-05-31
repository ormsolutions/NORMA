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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Diagrams;
using System.Reflection.Emit;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMShapeDomainModel
	{
		#region View Fixup Rules
		#region ModelHasObjectType fixup
		#region ObjectTypedAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectTypedAddedRule(ElementAddedEventArgs e)
		{
			ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
			ObjectType objectType = link.ObjectType;
			if (objectType.NestedFactType == null) // Otherwise, fix up with the fact type
			{
				Diagram.FixUpDiagram(link.Model, objectType);
			}
		}
		#endregion // ObjectTypedAddedRule
		#endregion // ModelHasObjectType fixup
		#region ModelHasFactType fixup
		#region ObjectTypeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// ChangeRule: typeof(ObjectifiedFactTypeNameShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			ObjectTypeShape objectTypeShape = null;
			ObjectifiedFactTypeNameShape objectifiedShape = null;
			Guid attributeId = e.DomainProperty.Id;
			if ((attributeId == ObjectTypeShape.ExpandRefModeDomainPropertyId &&
				null != (objectTypeShape = e.ModelElement as ObjectTypeShape)) ||
				(attributeId == ObjectifiedFactTypeNameShape.ExpandRefModeDomainPropertyId &&
				null != (objectifiedShape = e.ModelElement as ObjectifiedFactTypeNameShape)))
			{
				if (objectTypeShape != null)
				{
					objectTypeShape.AutoResize();
				}
				else
				{
					objectifiedShape.AutoResize();
				}

				ObjectType objectType = ((objectTypeShape != null) ? objectTypeShape.ModelElement : objectifiedShape.ModelElement) as ObjectType;
				UniquenessConstraint preferredConstraint;
				LinkedElementCollection<Role> constraintRoles;
				ObjectType rolePlayer;
				if (null != (preferredConstraint = objectType.PreferredIdentifier) &&
					preferredConstraint.IsInternal &&
					1 == (constraintRoles = preferredConstraint.RoleCollection).Count &&
					null != (rolePlayer = constraintRoles[0].RolePlayer) &&
					rolePlayer.IsValueType)
				{

					bool expandingRefMode = (bool)e.NewValue;
					if (preferredConstraint.IsObjectifiedSingleRolePreferredIdentifier)
					{
						// Back up the property descriptor, which sets the ExpandRefMode property
						// to readonly in this case
						if (!expandingRefMode)
						{
							if (objectTypeShape != null)
							{
								objectTypeShape.ExpandRefMode = true;
							}
							else
							{
								objectifiedShape.ExpandRefMode = true;
							}
						}
						return;
					}
					ORMDiagram parentDiagram = ((objectTypeShape != null) ? objectTypeShape.Diagram : objectifiedShape.Diagram) as ORMDiagram;
					Dictionary<ShapeElement, bool> shapeElements = new Dictionary<ShapeElement, bool>();

					// View or Hide FactType
					FactType factType = preferredConstraint.FactTypeCollection[0];
					if (!expandingRefMode)
					{
						RemoveShapesFromDiagram(factType, parentDiagram);
					}
					else
					{
						// Stop the reading shape from ending up in the wrong place during refmode expansion
						e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;

						if (!parentDiagram.ElementHasShape(factType))
						{
							Diagram.FixUpDiagram(objectType.Model, factType);

							foreach (ShapeElement shapeOnDiagram in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(parentDiagram, factType))
							{
								shapeElements.Add(shapeOnDiagram, true);
							}

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
							if (!parentDiagram.ElementHasShape(valueType))
							{
								Diagram.FixUpDiagram(objectType.Model, valueType);

								foreach (ShapeElement shapeOnDiagram in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(parentDiagram, valueType))
								{
									shapeElements.Add(shapeOnDiagram, true);
								}
							}

							foreach (ValueTypeHasValueConstraint link in DomainRoleInfo.GetElementLinks<ValueTypeHasValueConstraint>(valueType, ValueTypeHasValueConstraint.ValueTypeDomainRoleId))
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
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						Role role = roleBase.Role;
						foreach (ObjectTypePlaysRole link in DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId))
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
						foreach (RoleHasValueConstraint link in DomainRoleInfo.GetElementLinks<RoleHasValueConstraint>(role, RoleHasValueConstraint.RoleDomainRoleId))
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

					if (shapeElements.Count > 0)
					{
						NodeShape rootShape = e.ModelElement as NodeShape;
						// Turn off auto placement
						e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;

						// Tell the layout manager to calculate locations for the new elements
						LayoutManager bl = new LayoutManager(parentDiagram, (parentDiagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine)));
						foreach (ShapeElement shape in shapeElements.Keys)
						{
							if (shape == rootShape)
							{
								continue;
							}
							// The bool value of shapeElements says whether to position the shape or not.  If not, it's already on the diagram, so don't move it!
							bl.AddShape(shape, !shapeElements[shape]);
						}
						bl.AddShape(rootShape, true);
						bl.SetRootShape(rootShape);
						bl.Layout(false);
					}
				}
			}
		}
		/// <summary>
		/// Helper function to remove shapes on the diagram for a specific element.
		/// All child shapes will also be removed.
		/// </summary>
		private static void RemoveShapesFromDiagram(ModelElement element, Diagram diagram)
		{
			LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(element);
			int pelCount = pels.Count;
			for (int i = pelCount - 1; i >= 0; --i) // Walk backwards so we can safely remove
			{
				ShapeElement shape = pels[i] as ShapeElement;
				if (shape != null && shape.Diagram == diagram)
				{
					// Delete propagation in the CoreDesignSurface domain model will clear
					// any nested and relative shapes
					shape.Delete();
				}
			}
		}
		#endregion // ObjectTypeShapeChangeRule
		#region FactTypeAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void FactTypedAddedRule(ElementAddedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			if (link != null)
			{
				Diagram.FixUpDiagram(link.Model, link.FactType);
			}
		}
		#endregion // FactTypeAddedRule
		#region FactTypeChangedRule
		/// <summary>
		/// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void FactTypeShapeChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactTypeShape fts = e.ModelElement as FactTypeShape;
			if (fts != null)
			{
				if (e.DomainProperty.Id == FactTypeShape.DisplayRoleNamesDomainPropertyId)
				{
					FactType fact = fts.ModelElement as FactType;
					if (fact != null)
					{
						RoleNameShape.SetRoleNameDisplay(fact);
					}
				}
			}
		}
		#endregion // FactTypeChangedRule
		#endregion // ModelHasFactType fixup
		#region ModelHasConstraint fixup
		#region SetComparisonConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetComparisonConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SetComparisonConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelHasSetComparisonConstraint link = e.ModelElement as ModelHasSetComparisonConstraint;
			if (link != null)
			{
				Diagram.FixUpDiagram(link.Model, link.SetComparisonConstraint);
			}
		}
		#endregion // SetComparisonConstraintAddedRule
		#region SetConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SetConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelHasSetConstraint link = e.ModelElement as ModelHasSetConstraint;
			if (link != null)
			{
				Diagram.FixUpDiagram(link.Model, link.SetConstraint);
			}
		}
		#endregion // SetConstraintAddedRule
		#region ConstraintSetChanged fixup
		#region ConstraintRoleSequenceRoleAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Update the fact type when constraint roles are removed
		/// </summary>
		private static void ConstraintRoleSequenceRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			FactType factType;
			IConstraint constraint;
			if (null != (factType = link.Role.FactType) &&
				null != (constraint = link.ConstraintRoleSequence.Constraint))
			{
				FactTypeShape.ConstraintSetChanged(factType, constraint, true, false);
				// Other FactTypes might have renumbered, do a less expensive refresh on them
				foreach (FactType remainingFactType in (constraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint) ? ((SetComparisonConstraint)constraint).FactTypeCollection : ((SetConstraint)constraint).FactTypeCollection)
				{
					if (remainingFactType != factType)
					{
						FactTypeShape.ConstraintSetChanged(remainingFactType, constraint, false, true);
					}
				}
			}
		}
		#endregion // ConstraintRoleSequenceRoleAddedRule
		#region ConstraintRoleSequenceRoleDeletedRule
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Update the fact type when constraint roles are removed
		/// </summary>
		private static void ConstraintRoleSequenceRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			FactType factType;
			IConstraint constraint;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			if (!sequence.IsDeleted &&
				null != (factType = link.Role.FactType) &&
				!factType.IsDeleted &&
				null != (constraint = sequence.Constraint)
				)
			{
				FactTypeShape.ConstraintSetChanged(factType, constraint, true, false);
				// Other FactTypes might have renumbered, do a less expensive refresh on them
				foreach (FactType remainingFactType in (constraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint) ? ((SetComparisonConstraint)constraint).FactTypeCollection : ((SetConstraint)constraint).FactTypeCollection)
				{
					if (remainingFactType != factType)
					{
						FactTypeShape.ConstraintSetChanged(remainingFactType, constraint, false, true);
					}
				}
			}
		}
		#endregion // ConstraintRoleSequenceRoleDeletedRule
		#region ConstraintRoleSequencePositionChangedRule
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Update the redraw for the selected for cases when the constraint is active
		/// </summary>
		private static void ConstraintRoleSequencePositionChangedRule(RolePlayerOrderChangedEventArgs e)
		{
			IConstraint constraint;
			ModelElement constraintElement;
			ConstraintRoleSequence sequence = (ConstraintRoleSequence)e.SourceElement;
			if (!sequence.IsDeleted &&
				null != (constraint = sequence.Constraint) &&
				!(constraintElement = (ModelElement)constraint).IsDeleted)
			{
				foreach (FactType factType in (constraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint) ? ((SetComparisonConstraint)constraintElement).FactTypeCollection : ((SetConstraint)constraintElement).FactTypeCollection)
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, false, true);
				}
			}
		}
		#endregion // ConstraintRoleSequencePositionChangedRule
		#region ExternalRoleConstraintDeletedRule
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ExternalRoleConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Update the fact type when constraint roles are removed. Used when
		/// an entire role sequence is deleted from a multi-column external fact constraint
		/// that does not also delete the fact constraint.
		/// </summary>
		private static void ExternalRoleConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			ExternalRoleConstraint link = e.ModelElement as ExternalRoleConstraint;
			FactType factType;
			FactSetComparisonConstraint factConstraint = link.FactConstraint as FactSetComparisonConstraint;
			if (factConstraint != null &&
				!factConstraint.IsDeleted &&
				(null != (factType = factConstraint.FactType)) &&
				!factType.IsDeleted)
			{
				FactTypeShape.ConstraintSetChanged(factType, factConstraint.SetComparisonConstraint, false, false);
			}
		}
		#endregion // ExternalRoleConstraintDeletedRule
		#endregion // ConstraintSetChanged fixup
		#endregion // ModelHasConstraint fixup
		#region FactTypeHasRole fixup
		#region RoleAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void RoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;

			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
			{
				FactTypeShape shape = pel as FactTypeShape;
				if (shape != null)
				{
					shape.AutoResize();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Complementary rule to <see cref="RoleAddedRule"/>. Modifies RoleDisplayOrder early
		/// in the rule sequence so that any other rules that resize or redraw a FactTypeShape
		/// will not crash due to an arity mismatch in the sequence
		/// </summary>
		private static void RoleAddedRuleInline(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;

			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
			{
				FactTypeShape shape = pel as FactTypeShape;
				if (shape != null)
				{
					//This part handles inserting the role in the correct location if the facttypeshape has 
					//a different display order for the roles than the native one.
					LinkedElementCollection<RoleBase> roles = shape.RoleDisplayOrderCollection;

					RoleBase newRole = link.Role;
					if (factType.UnaryRole != null)
					{
						ObjectType rolePlayer;
						if (!(null != (rolePlayer = newRole.Role.RolePlayer) && rolePlayer.IsImplicitBooleanValue))
						{
							if (!roles.Contains(newRole))
							{
								roles.Add(newRole);
							}
						}
						return;
					}
					if (roles.Count != 0)
					{
						Store store = shape.Store;
						Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
						int insertIndex = -1;
						if (contextInfo.ContainsKey(FactTypeShape.InsertAfterRoleKey))
						{
							RoleBase insertAfter = (RoleBase)contextInfo[FactTypeShape.InsertAfterRoleKey];
							insertIndex = roles.IndexOf(insertAfter);
							if (insertIndex != -1)
							{
								++insertIndex;
							}
						}
						else if (contextInfo.ContainsKey(FactTypeShape.InsertBeforeRoleKey))
						{
							RoleBase insertBefore = (RoleBase)contextInfo[FactTypeShape.InsertBeforeRoleKey];
							insertIndex = roles.IndexOf(insertBefore);
						}
						if (insertIndex != -1)
						{
							roles.Insert(insertIndex, newRole);
						}
						else
						{
							roles.Add(newRole);
						}
					}
				}
			}
		}
		#endregion // RoleAddedRule
		#region RoleDeletedRule
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void RoleDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;
			if (!factType.IsDeleted)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						Role unaryRole = factType.UnaryRole;
						if (unaryRole != null)
						{
							LinkedElementCollection<RoleBase> displayOrder = shape.RoleDisplayOrderCollection;
							if (!displayOrder.Contains(unaryRole))
							{
								displayOrder.Add(factType.UnaryRole);
							}
						}
						shape.AutoResize();
					}
				}
			}
		}
		#endregion // RoleDeletedRule
		#endregion // FactTypeHasRole fixup
		#region ObjectTypePlaysRole fixup
		#region ObjectTypePlaysRoleAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ObjectTypePlaysRoleAddedRule(ElementAddedEventArgs e)
		{
			FixupRolePlayerLink((ObjectTypePlaysRole)e.ModelElement);
		}
		#endregion // ObjectTypePlaysRoleAddedRule
		#region DisplayRolePlayersFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayRolePlayersFixupListener : DeserializationFixupListener<ObjectTypePlaysRole>
		{
			/// <summary>
			/// Create a new DisplayRolePlayersFixupListener
			/// </summary>
			public DisplayRolePlayersFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add role player links when possible
			/// </summary>
			/// <param name="element">An ObjectTypePlaysRole instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ObjectTypePlaysRole element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRolePlayerLink(element);
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#region ObjectTypePlaysRoleRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// Add and remove links when a role player changes
		/// </summary>
		private static void ObjectTypePlaysRoleRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (link.IsDeleted)
			{
				return;
			}
			PresentationViewsSubject.GetPresentation(link).Clear();
			FixupRolePlayerLink(link);
		}
		#endregion // ObjectTypePlaysRoleRolePlayerChangeRule
		/// <summary>
		/// Helper function to display role player links.
		/// </summary>
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupRolePlayerLink(ObjectTypePlaysRole link)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			FactType associatedFact;
			ObjectType rolePlayer;
			ORMModel model;
			if (!link.IsDeleted &&
				null != (associatedFact = link.PlayedRole.FactType) &&
				null != (model = (rolePlayer = link.RolePlayer).Model))
			{
				FactType nestedFact;
				if (FactTypeShape.ShouldDrawObjectification(nestedFact = rolePlayer.NestedFactType))
				{
					Diagram.FixUpDiagram(model, nestedFact);
					Diagram.FixUpDiagram(nestedFact, rolePlayer);
				}
				else
				{
					Diagram.FixUpDiagram(model, rolePlayer);
				}
				Diagram.FixUpDiagram(model, associatedFact);

				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				foreach (PresentationViewsSubject presentationViewsSubject in DomainRoleInfo.GetElementLinks<PresentationViewsSubject>(model, PresentationViewsSubject.SubjectDomainRoleId))
				{
					ORMDiagram diagram;
					if ((diagram = presentationViewsSubject.Presentation as ORMDiagram) != null)
					{
						//add a link shape for each fact type shape on the diagram for the played role
						foreach (FactTypeShape shapeElement in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(diagram, associatedFact))
						{
							diagram.FixUpLocalDiagram(link);
						}
					}
				}

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		#endregion // ObjectTypePlaysRole fixup
		#region RoleHasValueConstraint fixup
		#region RoleValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.RoleHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
			if (link != null)
			{
				Role r = link.Role;
				FactType factType = r.FactType;
				ReadOnlyCollection<PresentationViewsSubject> links = PresentationViewsSubject.GetLinksToPresentation(factType);
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
		#endregion // RoleValueConstraintAddedRule
		#region DisplayValueConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayRoleValueConstraintFixupListener : DeserializationFixupListener<RoleHasValueConstraint>
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
			protected sealed override void ProcessElement(RoleHasValueConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRoleValueConstraintLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueConstraintFixupListener class
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
				if (model != null)
				{
					Diagram.FixUpDiagram(model, factType);
					Diagram.FixUpDiagram(factType, roleValueConstraint);

					object AllowMultipleShapes;
					Dictionary<object, object> topLevelContextInfo;
					bool containedAllowMultipleShapes;
					if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
					{
						topLevelContextInfo.Add(AllowMultipleShapes, null);
					}

					foreach (PresentationViewsSubject presentationViewsSubject in DomainRoleInfo.GetElementLinks<PresentationViewsSubject>(model, PresentationViewsSubject.SubjectDomainRoleId))
					{
						ORMDiagram diagram;
						if ((diagram = presentationViewsSubject.Presentation as ORMDiagram) != null)
						{
							//add a link shape for each constraint shape
							foreach (ValueConstraintShape shapeElement in MultiShapeUtility.FindAllShapesForElement<ValueConstraintShape>(diagram, roleValueConstraint))
							{
								diagram.FixUpLocalDiagram(link);
							}
						}
					}

					if (!containedAllowMultipleShapes)
					{
						topLevelContextInfo.Remove(AllowMultipleShapes);
					}
				}
			}
		}
		#endregion // RoleHasValueConstraint fixup
		#region ValueTypeHasValueConstraint fixup
		#region ValueConstraintAdded class
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ValueTypeValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
			if (link != null)
			{
				FixupValueTypeValueConstraintLink(link, null);
			}
		}
		#endregion // ValueTypeValueConstraintAdded class
		#region DisplayValueTypeValueConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayValueTypeValueConstraintFixupListener : DeserializationFixupListener<ValueTypeHasValueConstraint>
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
			protected sealed override void ProcessElement(ValueTypeHasValueConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupValueTypeValueConstraintLink(element, notifyAdded);
			}
		}
		#endregion // DisplayValueTypeValueConstraintFixupListener class
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
			foreach (RoleBase rBase in factType.RoleCollection)
			{
				Role r = rBase.Role;
				if (r != role)
				{
					objectType = r.RolePlayer;
				}
			}
			if (objectType != null)
			{
				ORMModel model = objectType.Model;
				if (model != null)
				{
					if (null == objectType.NestedFactType)
					{
						Diagram.FixUpDiagram(model, objectType);
					}
					Diagram.FixUpDiagram(objectType, roleValueConstraint);

					object AllowMultipleShapes;
					Dictionary<object, object> topLevelContextInfo;
					bool containedAllowMultipleShapes;
					if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
					{
						topLevelContextInfo.Add(AllowMultipleShapes, null);
					}

					Diagram.FixUpDiagram(model, link);

					if (!containedAllowMultipleShapes)
					{
						topLevelContextInfo.Remove(AllowMultipleShapes);
					}
				}
			}
		}
		#endregion // ValueTypeHasValueConstraint fixup
		#region FactConstraint fixup
		#region FactConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void FactConstraintAddedRule(ElementAddedEventArgs e)
		{
			FactConstraint link = e.ModelElement as FactConstraint;
			if (link != null)
			{
				FixupExternalConstraintLink(link);
			}
		}
		#endregion // FactConstraintAddedRule
		#region FactConstraintDeletedRule
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void FactConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			IFactConstraint link;
			IConstraint constraint;
			if (null != (link = e.ModelElement as IFactConstraint) &&
				null != (constraint = link.Constraint))
			{
				FactType fact = link.FactType;
				if (!fact.IsDeleted)
				{
					FactTypeShape.ConstraintSetChanged(fact, constraint, false, false);
				}
			}
		}
		#endregion // FactConstraintDeletedRule
		#region DisplayExternalConstraintLinksFixupListener class
		/// <summary>
		/// A fixup class to display external constraint links for
		/// when both endpoints are represented on the diagram
		/// </summary>
		private sealed class DisplayExternalConstraintLinksFixupListener : DeserializationFixupListener<FactConstraint>
		{
			/// <summary>
			/// Create a new DisplayExternalConstraintLinksFixupListener
			/// </summary>
			public DisplayExternalConstraintLinksFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add external fact constraint links to the diagram
			/// </summary>
			/// <param name="element">A FactConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					FixupExternalConstraintLink(element);
				}
			}
		}
		#endregion // DisplayExternalConstraintLinksFixupListener class
		/// <summary>
		/// Helper function to display external constraint links.
		/// </summary>
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupExternalConstraintLink(FactConstraint link)
		{
			// Make sure the constraint, fact type, and link
			// are displayed on the diagram
			IFactConstraint ifc = link as IFactConstraint;
			IConstraint constraint = ifc.Constraint;
			FactType factType;
			ORMModel model;
			ModelElement constraintElement = (ModelElement)constraint;
			if (!constraintElement.IsDeleted &&
				!constraint.ConstraintIsImplied &&
				null != (factType = ifc.FactType) &&
				!factType.IsDeleted)
			{
				if (constraint.ConstraintIsInternal)
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, false, false);
				}
				else if (null != (model = factType.Model))
				{
					Debug.Assert(model == constraint.Model);

					Diagram.FixUpDiagram(model, constraint as ModelElement);
					Diagram.FixUpDiagram(model, factType);

					object AllowMultipleShapes;
					Dictionary<object, object> topLevelContextInfo;
					bool containedAllowMultipleShapes;
					if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
					{
						topLevelContextInfo.Add(AllowMultipleShapes, null);
					}

					foreach (PresentationViewsSubject presentationViewsSubject in DomainRoleInfo.GetElementLinks<PresentationViewsSubject>(model, PresentationViewsSubject.SubjectDomainRoleId))
					{
						ORMDiagram diagram;
						if ((diagram = presentationViewsSubject.Presentation as ORMDiagram) != null)
						{
							//add a link shape for each constraint shape
							foreach (ExternalConstraintShape shapeElement in MultiShapeUtility.FindAllShapesForElement<ExternalConstraintShape>(diagram, constraint as ModelElement))
							{
								if (null == diagram.FixUpLocalDiagram(link))
								{
									shapeElement.Delete();
								}
							}
						}
					}

					if (!containedAllowMultipleShapes)
					{
						topLevelContextInfo.Remove(AllowMultipleShapes);
					}
				}
			}
		}
		#endregion // FactConstraint fixup
		#region ReadingOrder fixup
		/// <summary>
		/// Add shape elements for reading orders. Used during deserialization fixup
		/// and rules.
		/// </summary>
		/// <param name="link"></param>
		private static void FixupReadingOrderLink(FactTypeHasReadingOrder link)
		{
			ReadingOrder readingOrd = link.ReadingOrder;
			FactType fact = link.FactType;
			ORMModel model = fact.Model;
			if (!fact.IsDeleted && !(fact is SubtypeFact) && model != null)
			{
				Diagram.FixUpDiagram(model, fact); // Make sure the fact is already there

				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(fact, readingOrd);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
		{
			FixupReadingOrderLink(e.ModelElement as FactTypeHasReadingOrder);
		}
		#region DisplayReadingsFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayReadingsFixupListener : DeserializationFixupListener<FactTypeHasReadingOrder>
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
			protected sealed override void ProcessElement(FactTypeHasReadingOrder element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupReadingOrderLink(element);
			}
		}
		#endregion // DisplayReadingsFixupListener class
		#endregion // ReadingOrder fixup
		#region DisplayRolePlayersFixupListener class
		/// <summary>
		/// A fixup class to display role name
		/// </summary>
		private sealed class DisplayRoleNameFixupListener : DeserializationFixupListener<Role>
		{
			/// <summary>
			/// Create a new DisplayRoleNameFixupListener
			/// </summary>
			public DisplayRoleNameFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add role name when possible
			/// </summary>
			/// <param name="role">A Role instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(Role role, Store store, INotifyElementAdded notifyAdded)
			{
				if (!role.IsDeleted)
				{
					RoleNameShape.SetRoleNameDisplay(role.FactType);
				}
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#region ModelNote fixup
		#region ModelNoteAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasModelNote), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ModelNoteAddedRule(ElementAddedEventArgs e)
		{
			ModelHasModelNote link = e.ModelElement as ModelHasModelNote;
			if (link != null)
			{
				Diagram.FixUpDiagram(link.Model, link.Note);
			}
		}
		#endregion // ModelNoteAddedRule
		#region ModelNoteReferencedAddedRule
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelNoteReferencesModelElement), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ModelNoteReferenceAddedRule(ElementAddedEventArgs e)
		{
			FixupModelNoteLink(e.ModelElement as ModelNoteReferencesModelElement);
		}
		#endregion // ModelNoteReferencedAddedRule
		#region DisplayModelNoteLinksFixupListener class
		/// <summary>
		/// A fixup class to display external constraint links for
		/// when both endpoints are represented on the diagram
		/// </summary>
		private sealed class DisplayModelNoteLinksFixupListener : DeserializationFixupListener<ModelNoteReferencesModelElement>
		{
			/// <summary>
			/// Create a new DisplayModelNoteLinksFixupListener
			/// </summary>
			public DisplayModelNoteLinksFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add model note links on the diagram
			/// </summary>
			/// <param name="element">A ModelNoteReferencesModelElement instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ModelNoteReferencesModelElement element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupModelNoteLink(element);
			}
		}
		#endregion // DisplayModelNoteLinksFixupListener class
		/// <summary>
		/// Helper function to display model note links.
		/// </summary>
		/// <param name="link">An ModelNoteReferencesModelElement element</param>
		private static void FixupModelNoteLink(ModelNoteReferencesModelElement link)
		{
			// Make sure the element and note are displayed on the
			ModelNote note = link.Note;
			ModelElement element = link.Element;
			ORMModel model;
			if (!(note.IsDeleted || element.IsDeleted) &&
				null != (model = note.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(model, link);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		#endregion // ModelNote fixup
		#region ForceClearViewFixupDataListRuleClass
		partial class ForceClearViewFixupDataListRuleClass
		{
			#region Dynamic Microsoft.VisualStudio.Modeling.Diagrams.Diagram.ClearViewFixupDataList implementation
			private delegate void ClearDiagramViewFixupDataListDelegate(Diagram @this);
			/// <summary>
			/// The Microsoft.VisualStudio.Modeling.Diagrams.Diagram.ViewFixupData class and corresponding
			/// Microsoft.VisualStudio.Modeling.Diagrams.Diagram.ViewFixupDataList property are internal
			/// Microsoft.VisualStudio.Modeling.ModelCommand is internal, but the partition and EventArgs
			/// elements are not. Retrieve these with a dynamic method.
			/// </summary>
			private static readonly ClearDiagramViewFixupDataListDelegate ClearViewFixupDataList = CreateClearViewFixupDataList();
			private static ClearDiagramViewFixupDataListDelegate CreateClearViewFixupDataList()
			{
				Type diagramType = typeof(Diagram);
				Type viewFixupDataListType;
				PropertyInfo viewFixupDataListProperty;
				MethodInfo viewFixupDataListPropertyGet;
				MethodInfo clearMethod;
				if (null == (viewFixupDataListProperty = diagramType.GetProperty("ViewFixupDataList", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) ||
					null == (viewFixupDataListType = viewFixupDataListProperty.PropertyType) ||
					null == (clearMethod = viewFixupDataListType.GetMethod("Clear", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) ||
					null == (viewFixupDataListPropertyGet = viewFixupDataListProperty.GetGetMethod(true)))
				{
					Debug.Fail("The internal structure of the Diagram class has changed, IL generation will fail");
					return null;
				}

				// Approximate method being written (assuming TransactionItem context):
				// void ClearViewFixupDataList()
				// {
				//     ViewFixupDataList.Clear();
				// }
				DynamicMethod dynamicMethod = new DynamicMethod(
					"ClearViewFixupDataList",
					null,
					new Type[] { diagramType },
					diagramType.Module,
					true);
				// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 12 bytes
				// to avoid a resize to an even larger buffer.
				ILGenerator il = dynamicMethod.GetILGenerator(16);
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, viewFixupDataListPropertyGet);
				il.Emit(OpCodes.Call, clearMethod);

				// Return the array (already on the stack)
				il.Emit(OpCodes.Ret);
				return (ClearDiagramViewFixupDataListDelegate)dynamicMethod.CreateDelegate(typeof(ClearDiagramViewFixupDataListDelegate));
			}
			#endregion // Dynamic Microsoft.VisualStudio.Modeling.Diagrams.Diagram.ClearViewFixupDataList implementation
			/// <summary>
			/// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority + 1;
			/// Make sure the internal diagram ViewFixupDataList is cleared once fixup is completed.
			/// The core implementation does not clear more than a single element.
			/// </summary>
			private void ForceClearViewFixupDataListRule(ElementPropertyChangedEventArgs e)
			{
				Diagram diagram;
				if (e.DomainProperty.Id == Diagram.DoViewFixupDomainPropertyId &&
					!(diagram = (e.ModelElement as Diagram)).IsDeleted)
				{
					ClearViewFixupDataList(diagram);
				}
			}
		}
		#endregion // ForceClearViewFixupDataListRuleClass
		#region Shape Invalidation Routines
		private static readonly object InvalidateRequiredKey = new object();
		/// <summary>
		/// TransactionCommittingRule: typeof(ORMShapeDomainModel)
		/// Don't store the shape invalidation cache long term with the
		/// transaction log, we don't need it.
		/// </summary>
		private static void ClearInvalidateCacheOnCommittingRule(TransactionCommitEventArgs e)
		{
			Dictionary<object, object> contextDictionary = e.Transaction.Context.ContextInfo;
			if (contextDictionary.ContainsKey(InvalidateRequiredKey))
			{
				contextDictionary.Remove(InvalidateRequiredKey);
			}
		}
		/// <summary>
		/// Get a new update counter value if one is required. Use with
		/// the <see cref="GetCurrentUpdateCounterValue"/> to implement
		/// the delayed validation pattern
		/// </summary>
		/// <param name="element">A <see cref="ShapeElement"/> that implements
		/// the delayed validation pattern</param>
		/// <param name="refreshBitmap">Should the bitmap be refreshed?</param>
		/// <returns>A new value, or <see langword="null"/> if validation is already in place for this element</returns>
		public static long? GetNewUpdateCounterValue(ShapeElement element, bool refreshBitmap)
		{
			TransactionManager tmgr = element.Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				Transaction currentTransaction = tmgr.CurrentTransaction;
				Dictionary<object, object> contextInfo = currentTransaction.TopLevelTransaction.Context.ContextInfo;
				object validationStatesObject;
				Dictionary<ShapeElement, bool> validationStates;
				bool existingRefresh;
				bool requiresUpdate = true;
				if (!contextInfo.TryGetValue(InvalidateRequiredKey, out validationStatesObject) || null == (validationStates = validationStatesObject as Dictionary<ShapeElement, bool>))
				{
					validationStates = new Dictionary<ShapeElement, bool>();
					contextInfo[InvalidateRequiredKey] = validationStates;
					validationStates.Add(element, refreshBitmap);
				}
				else if (!validationStates.TryGetValue(element, out existingRefresh))
				{
					validationStates.Add(element, refreshBitmap);
				}
				else if (refreshBitmap && !existingRefresh)
				{
					validationStates[element] = true;
				}
				else
				{
					requiresUpdate = false;
				}
				if (requiresUpdate)
				{
					return unchecked(tmgr.CurrentTransaction.SequenceNumber - (refreshBitmap ? 0L : 1L));
				}
			}
			return null;
		}
		/// <summary>
		/// Get the current update counter value. Used with the
		/// <see cref="GetNewUpdateCounterValue"/> to manage setting
		/// an UpdateCounter value. Events on this value indicate that
		/// a shape needs to be invalidated.
		/// </summary>
		/// <param name="element">A <see cref="ShapeElement"/> that implements
		/// the delayed validation pattern</param>
		public static long GetCurrentUpdateCounterValue(ShapeElement element)
		{
			TransactionManager tmgr = element.Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Using subtract 2 and set to 1 under to indicate
				// the difference between an Invalidate(true) and
				// and Invalidate(false)
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 2);
			}
			else
			{
				return 0L;
			}
		}
		#endregion // Shape Invalidation Routines
		#endregion // View Fixup Rules
	}
}
