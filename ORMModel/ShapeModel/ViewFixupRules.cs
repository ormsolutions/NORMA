#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using System.Reflection.Emit;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ORMShapeDomainModel
	{
		#region View Fixup Rules
		#region ModelHasObjectType fixup
		#region ObjectTypedAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectTypedAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			ModelHasObjectType link;
			ObjectType objectType;
			if (!element.IsDeleted &&
				(objectType = (link = (ModelHasObjectType)element).ObjectType).NestedFactType == null && // Otherwise, fix up with the fact type
				AllowElementFixup(objectType))
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
		/// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			ObjectTypeShape objectTypeShape = null;
			ObjectifiedFactTypeNameShape objectifiedShape = null;
			FactTypeShape factTypeShape = null;
			Guid attributeId = e.DomainProperty.Id;
			ModelElement element = e.ModelElement;
			if ((attributeId == ObjectTypeShape.DisplayRefModeDomainPropertyId &&
				null != (objectTypeShape = element as ObjectTypeShape)) ||
				(attributeId == ObjectifiedFactTypeNameShape.DisplayRefModeDomainPropertyId &&
				null != (objectifiedShape = element as ObjectifiedFactTypeNameShape)) ||
				(attributeId == FactTypeShape.DisplayRefModeDomainPropertyId &&
				null != (factTypeShape = element as FactTypeShape) &&
				factTypeShape.DisplayAsObjectType))
			{
				RefModeDisplay oldValue = (RefModeDisplay)e.OldValue;
				if (oldValue == RefModeDisplay.HideCreateShapes)
				{
					// This is always a temporary value that will be reverted to RefModeDisplay.Hide
					// from inside this routine so no further processing is needed.
					return;
				}
				bool expandShapes = false;
				bool collapseShapes = false;
				bool resizeShape = false;
				bool clearCreate = false;
				switch ((RefModeDisplay)e.NewValue)
				{
					case RefModeDisplay.Show:
						collapseShapes = true;
						resizeShape = true;
						break;
					case RefModeDisplay.Hide:
						resizeShape = true;
						break;
					case RefModeDisplay.HideCreateShapes:
						expandShapes = clearCreate = true;
						resizeShape = oldValue == RefModeDisplay.Show;
						break;
				}
				ObjectType objectType = null;
				ORMModel model;
				NodeShape targetShape;
				if (objectTypeShape != null)
				{
					if (resizeShape)
					{
						objectTypeShape.AutoResize();
					}
					if (clearCreate)
					{
						objectTypeShape.DisplayRefMode = RefModeDisplay.Hide;
					}
					objectType = objectTypeShape.ModelElement as ObjectType;
					targetShape = objectTypeShape;
				}
				else if (objectifiedShape != null)
				{
					if (resizeShape)
					{
						objectifiedShape.AutoResize();
					}
					if (clearCreate)
					{
						objectifiedShape.DisplayRefMode = RefModeDisplay.Hide;
					}
					objectType = objectifiedShape.ModelElement as ObjectType;
					targetShape = objectifiedShape;
				}
				else
				{
					if (resizeShape)
					{
						factTypeShape.AutoResize();
					}
					if (clearCreate)
					{
						factTypeShape.DisplayRefMode = RefModeDisplay.Hide;
					}
					FactType objectifiedFactType;
					if (null != (objectifiedFactType = factTypeShape.AssociatedFactType))
					{
						objectType = objectifiedFactType.NestingType;
					}
					targetShape = factTypeShape;
				}
				if (objectType == null ||
					null == (model = objectType.Model))
				{
					return;
				}

				UniquenessConstraint preferredConstraint;
				LinkedElementCollection<Role> constraintRoles;
				ObjectType rolePlayer;
				if (null != (preferredConstraint = objectType.PreferredIdentifier) &&
					preferredConstraint.IsInternal &&
					1 == (constraintRoles = preferredConstraint.RoleCollection).Count &&
					null != (rolePlayer = constraintRoles[0].RolePlayer) &&
					rolePlayer.IsValueType)
				{
					bool resetAttachmentsForPreexistingFactTypeShape = false;
					if (preferredConstraint.IsObjectifiedSingleRolePreferredIdentifier)
					{
						// Back up the property descriptor, which sets the ExpandRefMode property
						// to readonly in this case
						if (!expandShapes)
						{
							if (objectTypeShape != null)
							{
								objectTypeShape.ExpandRefMode = true;
							}
							else if (objectifiedShape != null)
							{
								objectifiedShape.ExpandRefMode = true;
							}
							else
							{
								factTypeShape.ExpandRefMode = true;
							}
						}
						return;
					}

					if (!(collapseShapes || expandShapes))
					{
						return;
					}

					ORMDiagram parentDiagram = targetShape.Diagram as ORMDiagram;
					Dictionary<ShapeElement, bool> shapeElements = new Dictionary<ShapeElement, bool>();

					// View or Hide FactType
					FactType factType = preferredConstraint.FactTypeCollection[0];
					bool removedFactType = false;
					if (collapseShapes)
					{
						if (!parentDiagram.ShouldDisplayFactType(factType))
						{
							removedFactType = true;
							RemoveShapesFromDiagram(factType, parentDiagram);
						}
						else if (null != objectifiedShape)
						{
							// For the other shapes, a shape resize rebinds links,
							// but not for an objectifiedShape because the resize
							// is on the wrong shape.
							MultiShapeUtility.AttachLinkConfigurationChanged(objectifiedShape.ParentShape);
						}
					}
					else if (expandShapes)
					{
						// Stop the reading shape from ending up in the wrong place during refmode expansion
						e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ORMBaseShape.PlaceAllChildShapes] = null;

						if (!parentDiagram.ElementHasShape(factType))
						{
							if (AllowElementFixup(factType))
							{
								Diagram.FixUpDiagram(model, factType);
							}

							foreach (ShapeElement shapeOnDiagram in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(parentDiagram, factType))
							{
								shapeElements.Add(shapeOnDiagram, true);
							}

							foreach (ReadingOrder readingOrder in factType.ReadingOrderCollection)
							{
								Diagram.FixUpDiagram(factType, readingOrder);
							}
						}
						else
						{
							// Although we block directly adding a reference mode fact type
							// if all corresponding object type shapes on the diagram have
							// ExpandRefMode=false, it is possible to add these identifying
							// fact types to the diagram first. If they already exist, we need
							// to make sure that they attach. We delay calls to do this until
							// after the value type shape is created.
							resetAttachmentsForPreexistingFactTypeShape = true;
						}
						if (null != objectifiedShape)
						{
							// For the other shapes, a shape resize rebinds links automatically,
							// but not for an objectifiedShape because the resize is on a child shape,
							// not the shape attached to links.
							MultiShapeUtility.AttachLinkConfigurationChanged(objectifiedShape.ParentShape);
						}
					}

					//View or Hide value type
					ObjectType valueType = preferredConstraint.RoleCollection[0].RolePlayer;
					if (valueType != null)
					{
						if (expandShapes)
						{
							if (!parentDiagram.ElementHasShape(valueType))
							{
								if (AllowElementFixup(valueType))
								{
									Diagram.FixUpDiagram(model, valueType);
								}

								foreach (ShapeElement shapeOnDiagram in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(parentDiagram, valueType))
								{
									shapeElements.Add(shapeOnDiagram, true);
								}
							}

							foreach (ValueTypeHasValueConstraint link in DomainRoleInfo.GetElementLinks<ValueTypeHasValueConstraint>(valueType, ValueTypeHasValueConstraint.ValueTypeDomainRoleId))
							{
								FixupValueTypeValueConstraintLink(link, null);
							}

							foreach (ObjectTypeHasCardinalityConstraint link in DomainRoleInfo.GetElementLinks<ObjectTypeHasCardinalityConstraint>(valueType, ObjectTypeHasCardinalityConstraint.ObjectTypeDomainRoleId))
							{
								FixupObjectTypeCardinality(link, null);
							}
						}
						else if (removedFactType)
						{
							if (!objectType.ReferenceModeValueTypeAlsoUsedNormally && // Easy check first
								!parentDiagram.ShouldDisplayObjectType(valueType)) // More involved check second
							{
								RemoveShapesFromDiagram(valueType, parentDiagram);
							}
						}
					}

					// Reattach preexisting fact type shapes after the value type is added
					if (resetAttachmentsForPreexistingFactTypeShape)
					{
						foreach (ShapeElement shapeOnDiagram in MultiShapeUtility.FindAllShapesForElement<ShapeElement>(parentDiagram, factType))
						{
							MultiShapeUtility.AttachLinkConfigurationChanged(shapeOnDiagram);
						}
					}

					//View or Hide value constraint shapes and links. Role player links come and go automatically
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						foreach (RoleHasValueConstraint link in DomainRoleInfo.GetElementLinks<RoleHasValueConstraint>(roleBase.Role, RoleHasValueConstraint.RoleDomainRoleId))
						{
							if (expandShapes)
							{
								// Remove child shapes associated with this value constraint
								RoleValueConstraint valueConstraint = link.ValueConstraint;
								foreach (PresentationElement childPel in targetShape.RelativeChildShapes)
								{
									if (childPel.ModelElement == valueConstraint)
									{
										childPel.Delete();
										break;
									}
								}
								FixupRoleValueConstraintLink(link, null);
							}
							else if (collapseShapes)
							{
								FixupRoleValueConstraintLinkForIdentifiedEntityType(link, null);
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
						bl.Layout(false, objectifiedShape != null ? objectifiedShape.ParentShape as NodeShape : targetShape, true, true);
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
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void FactTypedAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				ModelHasFactType link = (ModelHasFactType)e.ModelElement;
				FactType factType = link.FactType;
				if (AllowElementFixup(factType))
				{
					Diagram.FixUpDiagram(link.Model, factType);
				}
			}
		}
		#endregion // FactTypeAddedRule
		#endregion // ModelHasFactType fixup
		#region ModelHasConstraint fixup
		#region SetComparisonConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetComparisonConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SetComparisonConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				ModelHasSetComparisonConstraint link = (ModelHasSetComparisonConstraint)element;
				SetComparisonConstraint constraint = link.SetComparisonConstraint;
				if (AllowElementFixup(constraint))
				{
					Diagram.FixUpDiagram(link.Model, constraint);
				}
			}
		}
		#endregion // SetComparisonConstraintAddedRule
		#region SetConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SetConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				ModelHasSetConstraint link = (ModelHasSetConstraint)e.ModelElement;
				SetConstraint constraint = link.SetConstraint;
				// Shapes are never added for internal constraints, so there is no point in attempting a fixup
				if (!((IConstraint)constraint).ConstraintIsInternal &&
					AllowElementFixup(constraint))
				{
					Diagram.FixUpDiagram(link.Model, constraint);
				}
			}
		}
		#endregion // SetConstraintAddedRule
		#region ConstraintSetChanged fixup
		#region ConstraintRoleSequenceRoleAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Update the fact type when constraint roles are removed
		/// </summary>
		private static void ConstraintRoleSequenceRoleAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			ConstraintRoleSequenceHasRole link;
			FactType factType;
			IConstraint constraint;
			if (!element.IsDeleted &&
				null != (factType = (link = (ConstraintRoleSequenceHasRole)element).Role.FactType) &&
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
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
		/// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExternalRoleConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void RoleAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(((FactTypeHasRole)element).FactType))
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						shape.AutoResize();
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Complementary rule to <see cref="RoleAddedRule"/>. Modifies RoleDisplayOrder early
		/// in the rule sequence so that any other rules that resize or redraw a FactTypeShape
		/// will not crash due to an arity mismatch in the sequence
		/// </summary>
		private static void RoleAddedRuleInline(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				FactTypeHasRole link = (FactTypeHasRole)element;
				FactType factType = link.FactType;
				RoleBase newRoleBase = link.Role;

				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						//This part handles inserting the role in the correct location if the facttypeshape has 
						//a different display order for the roles than the native one.
						LinkedElementCollection<RoleBase> roles = shape.RoleDisplayOrderCollection;

						if (factType.UnaryRole != null)
						{
							if (!roles.Contains(newRoleBase))
							{
								roles.Add(newRoleBase);
							}
							return;
						}
						if (roles.Count != 0 && !roles.Contains(newRoleBase))
						{
							Store store = shape.Store;
							Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
							object contextRole;
							int insertIndex = -1;
							if (contextInfo.TryGetValue(FactType.InsertAfterRoleKey, out contextRole))
							{
								insertIndex = roles.IndexOf(contextRole as RoleBase);
								if (insertIndex != -1)
								{
									++insertIndex;
								}
							}
							else if (contextInfo.TryGetValue(FactType.InsertBeforeRoleKey, out contextRole))
							{
								insertIndex = roles.IndexOf(contextRole as RoleBase);
							}
							if (insertIndex != -1)
							{
								roles.Insert(insertIndex, newRoleBase);
							}
							else
							{
								roles.Add(newRoleBase);
							}
						}
					}
				}
			}
		}
		#endregion // RoleAddedRule
		#region RoleDeletedRule
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
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
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
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
			// For simple reference scheme cases, the identified ObjectType displays
			// errors that will not be removed and automatically updated because the
			// actual error state has not changed.
			foreach (ConstraintRoleSequence constraintSequence in link.PlayedRole.ConstraintRoleSequenceCollection)
			{
				UniquenessConstraint uniquenessConstraint;
				ObjectType identified;
				if (null != (uniquenessConstraint = constraintSequence as UniquenessConstraint) &&
					null != (identified = uniquenessConstraint.PreferredIdentifierFor))
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(identified))
					{
						IInvalidateDisplay updateShape = pel as IInvalidateDisplay;
						if (updateShape != null)
						{
							updateShape.InvalidateRequired(true);
						}
					}
				}
			}
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
			FactType associatedFactType;
			FactType associatedProxyFactType;
			ObjectType rolePlayer;
			Role role;
			RoleProxy proxy;
			ORMModel model;
			if (!link.IsDeleted &&
				null != (associatedFactType = (role = link.PlayedRole).FactType) &&
				null != (model = (rolePlayer = link.RolePlayer).Model))
			{
				proxy = role.Proxy;
				associatedProxyFactType = (proxy != null) ? proxy.FactType : null;

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
						foreach (FactTypeShape shapeElement in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(diagram, associatedFactType))
						{
							try
							{
								topLevelContextInfo[ORMDiagram.CreatingRolePlayerLinkKey] = null; // Stop fixupLocalDiagram from trying both link types
								diagram.FixUpLocalDiagram(link);
								break;
							}
							finally
							{
								topLevelContextInfo.Remove(ORMDiagram.CreatingRolePlayerLinkKey);
							}
						}
						if (associatedProxyFactType != null)
						{
							foreach (FactTypeShape shapeElement in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(diagram, associatedProxyFactType))
							{
								try
								{
									topLevelContextInfo[ORMDiagram.CreatingRolePlayerProxyLinkKey] = null;
									diagram.FixUpLocalDiagram(link);
									break;
								}
								finally
								{
									topLevelContextInfo.Remove(ORMDiagram.CreatingRolePlayerProxyLinkKey);
								}
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
		#endregion // ObjectTypePlaysRole fixup
		#region RoleHasValueConstraint fixup
		#region RoleValueConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RoleHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			FactType factType;
			RoleHasValueConstraint link;
			if (!element.IsDeleted &&
				null != (factType = (link = (RoleHasValueConstraint)element).Role.FactType))
			{
				//If the factType has no presentation elements, it must be hidden. In which case,
				//we need to fixup the ValueTypeValueConstraint with this link.
				if (PresentationViewsSubject.GetLinksToPresentation(factType).Count > 0)
				{
					FixupRoleValueConstraintLink(link, null);
				}
				FixupRoleValueConstraintLinkForIdentifiedEntityType(link, null);
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
			FactType factType;
			ORMModel model;

			if (null != (factType = roleValueConstraint.Role.FactType) &&
				null != (model = factType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(factType, roleValueConstraint);

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
		#endregion // RoleHasValueConstraint fixup
		#region ValueTypeHasValueConstraint fixup
		#region ValueConstraintAdded class
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ValueTypeValueConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				FixupValueTypeValueConstraintLink((ValueTypeHasValueConstraint)element, null);
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
			ValueTypeValueConstraint valueConstraint = link.ValueConstraint;
			ObjectType objectType = link.ValueType;
			ORMModel model;
			if (null != (model = objectType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(objectType, valueConstraint);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		/// <summary>
		/// Helper function to display role value ranges on object types with a ref mode.
		/// </summary>
		/// <param name="link">A RoleHasValueConstraint element</param>
		/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
		private static void FixupRoleValueConstraintLinkForIdentifiedEntityType(RoleHasValueConstraint link, INotifyElementAdded notifyAdded)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			RoleValueConstraint roleValueConstraint = link.ValueConstraint;
			Role role = roleValueConstraint.Role;
			UniquenessConstraint uniquenessConstraint;
			ObjectType objectType;
			ORMModel model;
			if (null != (uniquenessConstraint = role.SingleRoleAlethicUniquenessConstraint) &&
				null != (objectType = uniquenessConstraint.PreferredIdentifierFor) &&
				null != (model = objectType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(objectType, roleValueConstraint);
				FactType objectifiedFactType;
				if (null != (objectifiedFactType = objectType.NestedFactType))
				{
					// We may have a 'display as object type' fact type that needs to display
					// this value constraint.
					Diagram.FixUpDiagram(objectifiedFactType, roleValueConstraint);
				}
				Diagram.FixUpDiagram(model, link);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		#endregion // ValueTypeHasValueConstraint fixup
		#region CardinalityConstraint fixup
		#region DisplayObjectTypeCardinalityConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayObjectTypeCardinalityConstraintFixupListener : DeserializationFixupListener<ObjectTypeHasCardinalityConstraint>
		{
			/// <summary>
			/// Create a new DisplayValueConstraintFixupListener
			/// </summary>
			public DisplayObjectTypeCardinalityConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ObjectTypeHasCardinalityConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupObjectTypeCardinality(element, notifyAdded);
			}
		}
		#endregion // DisplayObjectTypeCardinalityConstraintFixupListener class
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCardinalityConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectTypeCardinalityAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				FixupObjectTypeCardinality((ObjectTypeHasCardinalityConstraint)element, null);
			}
		}
		private static void FixupObjectTypeCardinality(ObjectTypeHasCardinalityConstraint link, INotifyElementAdded notifyAdded)
		{
			// Make sure the cardinality constraint is displayed with each shape representing
			// the object type.
			ObjectTypeCardinalityConstraint cardinalityConstraint = link.CardinalityConstraint;
			ObjectType objectType = link.ObjectType;
			ORMModel model;
			if (null != (model = objectType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(objectType, cardinalityConstraint);
				FactType objectifiedFactType;
				if (null != (objectifiedFactType = objectType.NestedFactType))
				{
					Diagram.FixUpDiagram(objectifiedFactType, cardinalityConstraint);
				}

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		#region DisplayUnaryRoleCardinalityConstraintFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayUnaryRoleCardinalityConstraintFixupListener : DeserializationFixupListener<UnaryRoleHasCardinalityConstraint>
		{
			/// <summary>
			/// Create a new DisplayUnaryRoleCardinalityConstraintFixupListener
			/// </summary>
			public DisplayUnaryRoleCardinalityConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add value range links when possible
			/// </summary>
			/// <param name="element">A RoleHasValueConstraint instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(UnaryRoleHasCardinalityConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupUnaryRoleCardinality(element, notifyAdded);
			}
		}
		#endregion // DisplayUnaryRoleCardinalityConstraintFixupListener class
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.UnaryRoleHasCardinalityConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void UnaryRoleCardinalityAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				FixupUnaryRoleCardinality((UnaryRoleHasCardinalityConstraint)element, null);
			}
		}
		private static void FixupUnaryRoleCardinality(UnaryRoleHasCardinalityConstraint link, INotifyElementAdded notifyAdded)
		{
			UnaryRoleCardinalityConstraint cardinalityConstraint = link.CardinalityConstraint;
			Role role = link.UnaryRole;
			FactType factType;
			ORMModel model;
			if (null != (factType = role.FactType) &&
				null != (model = factType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(factType, cardinalityConstraint);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		#endregion // CardinalityConstraint fixup
		#region FactConstraint fixup
		#region FactConstraintAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void FactConstraintAddedRule(ElementAddedEventArgs e)
		{
			FixupExternalConstraintLink((FactConstraint)e.ModelElement);
		}
		#endregion // FactConstraintAddedRule
		#region FactConstraintDeletedRule
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
				FixupExternalConstraintLink(element);
			}
		}
		#endregion // DisplayExternalConstraintLinksFixupListener class
		/// <summary>
		/// Helper function to display external constraint links.
		/// </summary>
		private static void FixupExternalConstraintLink(FactConstraint link)
		{
			// Make sure the constraint, fact type, and link
			// are displayed on the diagram
			IFactConstraint ifc;
			IConstraint constraint;
			FactType factType;
			ORMModel model;
			if (!link.IsDeleted &&
				null != (constraint = (ifc = (IFactConstraint)link).Constraint) &&
				!constraint.ConstraintIsImplied &&
				null != (factType = ifc.FactType))
			{
				if (constraint.ConstraintIsInternal)
				{
					FactTypeShape.ConstraintSetChanged(factType, constraint, false, false);
				}
				else if (null != (model = factType.Model))
				{
					ModelElement constraintElement = (ModelElement)constraint;
					if (AllowElementFixup(constraintElement))
					{
						Diagram.FixUpDiagram(model, constraintElement);
					}
					if (AllowElementFixup(factType))
					{
						Diagram.FixUpDiagram(model, factType);
					}
					LinkedElementCollection<ConstraintRoleSequenceHasRole> singleRoleFixups =
						(0 == (constraint.RoleSequenceStyles & RoleSequenceStyles.ConnectIndividualRoles)) ?
							null :
							link.ConstrainedRoleCollection;
					int roleCount;
					BitTracker tracker;
					if (singleRoleFixups != null)
					{
						roleCount = singleRoleFixups.Count;
						tracker = new BitTracker(roleCount);
					}
					else
					{
						roleCount = 0;
						tracker = default(BitTracker);
					}

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
							foreach (ExternalConstraintShape constraintShape in MultiShapeUtility.FindAllShapesForElement<ExternalConstraintShape>(diagram, constraintElement))
							{
								if (singleRoleFixups == null)
								{
									bool haveExistingShape = false;
									foreach (ExternalConstraintLink attachedLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<ExternalConstraintLink>(constraintShape))
									{
										if (attachedLink.AssociatedFactConstraint == link)
										{
											haveExistingShape = true;
											break;
										}
									}
									if (!haveExistingShape &&
										null == diagram.FixUpLocalDiagram(link))
									{
										constraintShape.Delete();
									}
								}
								else
								{
									tracker.Reset();
									int matchedConstraintRoles = 0;
									foreach (ExternalConstraintLink attachedLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<ExternalConstraintLink>(constraintShape))
									{
										ConstraintRoleSequenceHasRole attachedConstraintRole = attachedLink.AssociatedConstraintRole;
										int attachedIndex;
										if (attachedConstraintRole != null &&
											-1 != (attachedIndex = singleRoleFixups.IndexOf(attachedConstraintRole)))
										{
											if (!tracker[attachedIndex])
											{
												tracker[attachedIndex] = true;
												if (++matchedConstraintRoles == roleCount)
												{
													break;
												}
											}
										}
									}
									if (matchedConstraintRoles < roleCount)
									{
										for (int i = 0; i < roleCount; ++i)
										{
											if (!tracker[i] &&
												null == diagram.FixUpLocalDiagram(singleRoleFixups[i]))
											{
												constraintShape.Delete();
												break;
											}
										}
									}
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
		private static void FixupReadingOrderLink(FactTypeHasReadingOrder link)
		{
			FactType factType;
			ORMModel model;
			if (!link.IsDeleted &&
				!((factType = link.FactType) is SubtypeFact) &&
				null != (model = factType.Model))
			{
				object AllowMultipleShapes;
				Dictionary<object, object> topLevelContextInfo;
				bool containedAllowMultipleShapes;
				if (!(containedAllowMultipleShapes = (topLevelContextInfo = link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
				{
					topLevelContextInfo.Add(AllowMultipleShapes, null);
				}

				Diagram.FixUpDiagram(factType, link.ReadingOrder);

				if (!containedAllowMultipleShapes)
				{
					topLevelContextInfo.Remove(AllowMultipleShapes);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
		{
			FixupReadingOrderLink((FactTypeHasReadingOrder)e.ModelElement);
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
		#region ModelNote fixup
		#region ModelNoteAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasModelNote), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ModelNoteAddedRule(ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted)
			{
				ModelHasModelNote link = (ModelHasModelNote)e.ModelElement;
				ModelNote note = link.Note;
				if (AllowElementFixup(note))
				{
					Diagram.FixUpDiagram(link.Model, note);
				}
			}
		}
		#endregion // ModelNoteAddedRule
		#region ModelNoteReferencedAddedRule
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelNoteReferencesModelElement), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ModelNoteReferenceAddedRule(ElementAddedEventArgs e)
		{
			FixupModelNoteLink((ModelNoteReferencesModelElement)e.ModelElement);
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
		#region Auto populated diagram fixup
		#region DisplayAutoPopulatedShapesFixupListener class
		/// <summary>
		/// A fixup class to create top-level shapes for automatically populated ORM diagrams.
		/// </summary>
		/// <remarks>This used to happen automatically when fixup listeners for implicit
		/// links verified the existence of the shapes they were attaching too. However, this
		/// had too many side effects, so we now check this condition explicitly on load.</remarks>
		private sealed class DisplayAutoPopulatedShapesFixupListener : DeserializationFixupListener<ORMDiagram>
		{
			/// <summary>
			/// Create a new DisplayAutoPopulatedShapesFixupListener
			/// </summary>
			public DisplayAutoPopulatedShapesFixupListener()
				: base((int)ORMDeserializationFixupPhase.AutoCreateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Add top-level auto populated shapes to a diagram
			/// </summary>
			/// <param name="element">An ORMDiagram instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ORMDiagram element, Store store, INotifyElementAdded notifyAdded)
			{
				if (element.AutoPopulateShapes)
				{
					IElementDirectory elementDir = store.ElementDirectory;
					ShapeElement shape;
					foreach (ObjectType objectType in elementDir.FindElements<ObjectType>(false))
					{
						Objectification objectification;
						if (null == (objectification = objectType.Objectification) || !objectification.IsImplied)
						{
							if (null != (shape = element.FixUpLocalDiagram(element, objectType)))
							{
								notifyAdded.ElementAdded(shape, true);
							}
						}
					}
					foreach (FactType factType in elementDir.FindElements<FactType>(false))
					{
						if (null == factType.ImpliedByObjectification)
						{
							if (null != (shape = element.FixUpLocalDiagram(element, factType)))
							{
								notifyAdded.ElementAdded(shape, true);
							}
						}

					}
				}
			}
		}
		#endregion // DisplayModelNoteLinksFixupListener class
		#endregion // Auto populated diagram fixup
		#region ForceClearViewFixupDataListRuleClass
		partial class ForceClearViewFixupDataListRuleClass
		{
			#region Dynamic Microsoft.VisualStudio.Modeling.Diagrams.Diagram.GetViewFixupDataListCount implementation
			private delegate int GetViewFixupDataListCountDelegate(Diagram @this);
			private static readonly GetViewFixupDataListCountDelegate GetViewFixupDataListCount = CreateGetViewFixupDataListCount();
			private static GetViewFixupDataListCountDelegate CreateGetViewFixupDataListCount()
			{
				Type diagramType = typeof(Diagram);
				Type viewFixupDataListType;
				PropertyInfo viewFixupDataListProperty;
				MethodInfo viewFixupDataListPropertyGet;
				PropertyInfo countProperty;
				MethodInfo countPropertyGet;
				if (null == (viewFixupDataListProperty = diagramType.GetProperty("ViewFixupDataList", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) ||
					null == (viewFixupDataListType = viewFixupDataListProperty.PropertyType) ||
					null == (countProperty = viewFixupDataListType.GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)) ||
					null == (countPropertyGet = countProperty.GetGetMethod(true)) ||
					null == (viewFixupDataListPropertyGet = viewFixupDataListProperty.GetGetMethod(true)))
				{
					Debug.Fail("The internal structure of the Diagram class has changed, IL generation will fail");
					return null;
				}
				// Approximate method being written:
				// int GetViewFixupDataListCount()
				// {
				//     return ViewFixupDataList.Count;
				// }
				DynamicMethod dynamicMethod = new DynamicMethod(
					"GetViewFixupDataListCount",
					typeof(int),
					new Type[] { diagramType },
					diagramType.Module,
					true);
				// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for more than the required 12 bytes
				// to avoid a resize to an even larger buffer.
				ILGenerator il = dynamicMethod.GetILGenerator(16);
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, viewFixupDataListPropertyGet);
				il.Emit(OpCodes.Call, countPropertyGet);

				// Return the count (already on the stack)
				il.Emit(OpCodes.Ret);
				return (GetViewFixupDataListCountDelegate)dynamicMethod.CreateDelegate(typeof(GetViewFixupDataListCountDelegate));
			}
			#endregion // Dynamic Microsoft.VisualStudio.Modeling.Diagrams.Diagram.GetViewFixupDataListCount implementation
			/// <summary>
			/// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority + 1;
			/// Only the first element of the internal diagram ViewFixupDataList is processed whenever the
			/// DoViewFixup is set to true, but we generally need to process all of these items. Keep setting
			/// the property to true until all of the items are processed.
			/// </summary>
			private void ForceClearViewFixupDataListRule(ElementPropertyChangedEventArgs e)
			{
				Diagram diagram;
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == Diagram.DoViewFixupDomainPropertyId &&
					!((bool)e.NewValue) &&
					!(diagram = (e.ModelElement as Diagram)).IsDeleted &&
					0 != GetViewFixupDataListCount(diagram))
				{
					diagram.Store.DomainDataDirectory.GetDomainProperty(propertyId).SetValue(diagram, true);
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
		/// Also defer to the multi shape utility to clear its caches.
		/// </summary>
		private static void ClearCachesOnCommittingRule(TransactionCommitEventArgs e)
		{
			Dictionary<object, object> contextDictionary = e.Transaction.Context.ContextInfo;
			if (contextDictionary.ContainsKey(InvalidateRequiredKey))
			{
				contextDictionary.Remove(InvalidateRequiredKey);
			}
			MultiShapeUtility.ClearCachedContextInfo(contextDictionary);
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
		#region Merge context validation rules
		/// <summary>
		/// Helper method to determine if a fixup operation
		/// should be attempted for a top-level added element. Fixup
		/// is not needed if presentation elements are dropped,
		/// but it is needed if non-presentation elements are dropped
		/// directly.
		/// </summary>
		/// <param name="element">An element that might require a shape.</param>
		/// <returns><see langword="true"/> to continue with fixup.</returns>
		public static bool AllowElementFixup(ModelElement element)
		{
			Transaction transaction = element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction;
			if (!DesignSurfaceMergeContext.HasContext(transaction))
			{
				return true;
			}
			if (DesignSurfaceMergeContext.GetRootPresentationElements(transaction).Count == 0)
			{
				IList rootElements = DesignSurfaceMergeContext.GetRootModelElements(transaction);
				// Merge if no context is given or no root elements are available
				return rootElements.Count == 0 || rootElements.Contains(element);
			}
			return false;
		}
		#endregion // Merge context validation rules
		#endregion // View Fixup Rules
	}
}
