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
		private sealed class ObjectTypedAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasObjectType link = e.ModelElement as ModelHasObjectType;
				if (link != null)
				{
					ObjectType objectType = link.ObjectType;
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
		[RuleOn(typeof(ObjectifiedFactTypeNameShape), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectTypeShapeChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
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
					if (null != (preferredConstraint = objectType.PreferredIdentifier) &&
						preferredConstraint.IsInternal &&
						preferredConstraint.RoleCollection[0].RolePlayer.IsValueType)
					{

						bool expandingRefMode = (bool)e.NewValue;
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
				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(element);
				int pelCount = pels.Count;
				for (int i = pelCount - 1; i >= 0; --i) // Walk backwards so we can safely remove
				{
					ShapeElement shape = pels[i] as ShapeElement;
					if (shape != null && shape.Diagram == diagram)
					{
						ClearChildShapes(shape.NestedChildShapes);
						ClearChildShapes(shape.RelativeChildShapes);
						shape.Delete();
					}
				}
			}
			/// <summary>
			/// Helper function to recursively delete child shapes. Used by RemoveShapesFromDiagram.
			/// </summary>
			private void ClearChildShapes(LinkedElementCollection<ShapeElement> shapes)
			{
				int count = shapes.Count;
				if (count > 0)
				{
					for (int i = count - 1; i >= 0; --i) // Walk backwards so we can safely remove the shape
					{
						ShapeElement shape = shapes[i];
						ClearChildShapes(shape.NestedChildShapes);
						ClearChildShapes(shape.RelativeChildShapes);
						shape.Delete();
					}
				}
			}
		}
		#endregion // ObjectTypeShapeChangeRule class
		#region FactTypeAdded class
		[RuleOn(typeof(ModelHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypedAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasFactType link = e.ModelElement as ModelHasFactType;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.FactType);
				}
			}
		}
		#endregion // FactTypeAdded class
		#region FactTypeChanged
		[RuleOn(typeof(FactTypeShape), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypeShapeChanged : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
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
		}
		#endregion // FactTypechanged
		#endregion // ModelHasFactType fixup
		#region ModelHasConstraint fixup
		#region SetComparisonConstraintAdded class
		[RuleOn(typeof(ModelHasSetComparisonConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetComparisonConstraintAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasSetComparisonConstraint link = e.ModelElement as ModelHasSetComparisonConstraint;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.SetComparisonConstraint);
				}
			}
		}
		#endregion // SetComparisonConstraintAdded class
		#region SetConstraintAdded class
		[RuleOn(typeof(ModelHasSetConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetConstraintAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasSetConstraint link = e.ModelElement as ModelHasSetConstraint;
				if (link != null)
				{
					Diagram.FixUpDiagram(link.Model, link.SetConstraint);
				}
			}
		}
		#endregion // SetConstraintAdded class
		#region ConstraintSetChanged fixup
		#region ConstraintRoleSequenceRoleAdded class
		/// <summary>
		/// Update the fact type when constraint roles are removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequenceRoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				FactType factType;
				IConstraint constraint;
				if (null != (factType = link.Role.FactType) &&
					null != (constraint = link.ConstraintRoleSequence.Constraint))
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
		private sealed class ConstraintRoleSequenceRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
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
		private sealed class ExternalRoleConstraintDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ExternalRoleConstraint link = e.ModelElement as ExternalRoleConstraint;
				FactType factType;
				FactSetComparisonConstraint factConstraint = link.FactConstraint as FactSetComparisonConstraint;
				if (factConstraint != null &&
					!factConstraint.IsDeleted &&
					(null != (factType = factConstraint.FactType)) &&
					!factType.IsDeleted)
				{
					FactTypeShape.ConstraintSetChanged(factType, factConstraint.SetComparisonConstraint, false);
				}
			}
		}
		#endregion // ExternalRoleConstraintRemoved class
		#endregion // ConstraintSetChanged fixup
		#endregion // ModelHasConstraint fixup
		#region FactTypeHasRole fixup
		#region RoleAdded class
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
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
						if (roles.Count != 0)
						{
							Store store = shape.Store;
							RoleBase newRole = link.Role;
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
						shape.AutoResize();
					}
				}
			}
		}
		#endregion // RoleAdded class
		#region RoleRemoved class
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
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
		private sealed class ObjectTypePlaysRoleAdded : AddRule
		{
			public static void Process(ObjectTypePlaysRole link)
			{
				FixupRolePlayerLink(link);
			}
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				Process(e.ModelElement as ObjectTypePlaysRole);
			}
		}
		#endregion // ObjectTypePlaysRoleAdded class
		#region DisplayRolePlayersFixupListener class
		/// <summary>
		/// A fixup class to display role player links
		/// </summary>
		private sealed class DisplayRolePlayersFixupListener : DeserializationFixupListener<ObjectTypePlaysRole>
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
			protected sealed override void ProcessElement(ObjectTypePlaysRole element, Store store, INotifyElementAdded notifyAdded)
			{
				FixupRolePlayerLink(element);
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#region ObjectTypePlaysRoleDeleted class
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleDeleted : DeleteRule
		{
			/// <summary>
			/// Remove presentation elements when the associated RolePlayer link is removed
			/// </summary>
			public static void Process(ObjectTypePlaysRole link)
			{
				// This will fire the PresentationLinkRemoved rule
				PresentationViewsSubject.GetPresentation(link).Clear();
			}
			/// <summary>
			/// Remove presentation elements when the associated RolePlayer link is removed
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				Process(e.ModelElement as ObjectTypePlaysRole);
			}
		}
		#endregion // ObjectTypePlaysRoleDeleted class
		#region ObjectTypePlaysRoleRolePlayerChange class
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleRolePlayerChange : RolePlayerChangeRule
		{
			/// <summary>
			/// Add and remove links when a role player changes
			/// </summary>
			public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
				if (link.IsDeleted)
				{
					return;
				}
				ObjectTypePlaysRoleDeleted.Process(link);
				ObjectTypePlaysRoleAdded.Process(link);
			}
		}
		#endregion // ObjectTypePlaysRoleRolePlayerChange class
		/// <summary>
		/// Helper function to display role player links.
		/// </summary>
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupRolePlayerLink(ObjectTypePlaysRole link)
		{
			// Make sure the object type, fact type, and link
			// are displayed on the diagram
			FactType associatedFact = link.PlayedRole.FactType;
			if (associatedFact != null)
			{
				ObjectType rolePlayer = link.RolePlayer;
				ORMModel model = rolePlayer.Model;
				if (model != null)
				{
					FactType nestedFact = rolePlayer.NestedFactType;
					if (FactTypeShape.ShouldDrawObjectification(nestedFact))
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
		private sealed class RoleValueConstraintAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
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
		}
		#endregion // RoleValueConstraintAdded class
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
		#region RoleValueConstraintRemoved class
		[RuleOn(typeof(RoleHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class RoleValueConstraintDeleted : DeleteRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				RoleHasValueConstraint link = e.ModelElement as RoleHasValueConstraint;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					PresentationViewsSubject.GetPresentation(link).Clear();
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
		private sealed class ValueTypeValueConstraintAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
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
		#region ValueTypeValueConstraintRemoved class
		[RuleOn(typeof(ValueTypeHasValueConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ValueTypeValueConstraintDeleted : DeleteRule
		{
			/// <summary>
			/// Remove presentation elements when the associated ValueRange link is removed
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ValueTypeHasValueConstraint link = e.ModelElement as ValueTypeHasValueConstraint;
				if (link != null)
				{
					// This will fire the PresentationLinkRemoved rule
					PresentationViewsSubject.GetPresentation(link).Clear();
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
					Diagram.FixUpDiagram(model, objectType);
					Diagram.FixUpDiagram(objectType, roleValueConstraint);
					Diagram.FixUpDiagram(model, link);
				}
			}
		}
		#endregion // ValueTypeHasValueConstraint fixup
		#region FactConstraint fixup
		#region FactConstraintAdded class
		[RuleOn(typeof(FactConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class FactConstraintAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactConstraint link = e.ModelElement as FactConstraint;
				if (link != null)
				{
					FixupExternalConstraintLink(link);
				}
			}
		}
		#endregion // FactConstraintAdded class
		#region FactConstraintRemoved class
		[RuleOn(typeof(FactConstraint), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class FactConstraintDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				IFactConstraint link;
				IConstraint constraint;
				if (null != (link = e.ModelElement as IFactConstraint) &&
					null != (constraint = link.Constraint))
				{
					FactType fact = link.FactType;
					if (!fact.IsDeleted)
					{
						FactTypeShape.ConstraintSetChanged(fact, constraint, false);
					}
				}
			}
		}
		#endregion // FactConstraintRemoved class
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
			public DisplayExternalConstraintLinksFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
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
		/// <param name="link">An ObjectTypePlaysRole element</param>
		private static void FixupExternalConstraintLink(FactConstraint link)
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
		#endregion // FactConstraint fixup
		#region PresentationViewsSubject fixup
		#region PresentationLinkRemoved class
		[RuleOn(typeof(PresentationViewsSubject))]
		private sealed class PresentationLinkDeleted : DeleteRule
		{
			/// <summary>
			/// Clearing the PresentationRolePlayers collection does not automatically
			/// remove the PELs (propagatedelete is false). Add this rule in code here.
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				PresentationViewsSubject link = e.ModelElement as PresentationViewsSubject;
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
							if (fact != null && !fact.IsDeleted)
							{
								LinkedElementCollection<ReadingOrder> remainingOrders = fact.ReadingOrderCollection;
								if (remainingOrders.Count != 0)
								{
									LinkedElementCollection<RoleBase> roles = fact.RoleCollection;
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
						presenter.Delete();
					}
				}
			}
		}
		#endregion // PresentationLinkRemoved class
		#region ParentShapeRemoved class
		[RuleOn(typeof(ParentShapeHasRelativeChildShapes))]
		[RuleOn(typeof(ParentShapeContainsNestedChildShapes))]
		private sealed class ParentShapeDeleted : DeleteRule
		{
			/// <summary>
			/// Deletion of a parent shape should delete the child shape.
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ParentShapeHasRelativeChildShapes linkRelative = e.ModelElement as ParentShapeHasRelativeChildShapes;
				ParentShapeContainsNestedChildShapes linkNested;
				if (linkRelative != null)
				{
					ShapeElement relativeShape = linkRelative.RelativeChildShapes;
					// Don't remove an ObjectifiedFactTypeNameShape in line. We need
					// it to reposition an ObjectTypeShape if the objectedified fact is
					// deleted.
					if (!(relativeShape is ObjectifiedFactTypeNameShape))
					{
						relativeShape.Delete();
					}
                }
				else if ((linkNested = e.ModelElement as ParentShapeContainsNestedChildShapes) != null)
				{
					linkNested.NestedChildShapes.Delete();
				}
			}
		}
		[RuleOn(typeof(ParentShapeHasRelativeChildShapes), FireTime=TimeToFire.LocalCommit, Priority=int.MaxValue)]
		private sealed class RelativeParentShapeDeleted : DeleteRule
		{
			/// <summary>
			/// Backup deletion of an ObjectifiedFactTypeNameShape, skipped during inline rule
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ParentShapeHasRelativeChildShapes link = e.ModelElement as ParentShapeHasRelativeChildShapes;
				ShapeElement childShape = link.RelativeChildShapes;
				if (!childShape.IsDeleted)
				{
					childShape.Delete();
				}
			}
		}
		#endregion // ParentShapeRemoved class
		#region EliminateOrphanedShapesFixupListener class
		/// <summary>
		/// A fixup class to remove orphaned pels
		/// </summary>
		private sealed class EliminateOrphanedShapesFixupListener : DeserializationFixupListener<PresentationElement>
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
			protected sealed override void ProcessElement(PresentationElement element, Store store, INotifyElementAdded notifyAdded)
			{
				ModelElement backingElement = element.ModelElement;
				if (backingElement == null || backingElement.IsDeleted)
				{
					element.Delete();
				}
			}
		}
		#endregion // EliminateOrphanedShapesFixupListener class
		#endregion // PresentationViewsSubject fixup
		#region LinkConnectsToNodeRemoved class
		/// <summary>
		/// Don't leave links dangling. Remove any link shape that points
		/// to no model element.
		/// </summary>
		[RuleOn(typeof(LinkConnectsToNode), FireTime=TimeToFire.LocalCommit)]
		private sealed class LinkConnectsToNodeDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				LinkConnectsToNode link = e.ModelElement as LinkConnectsToNode;
				LinkShape linkShape = link.Link;
				NodeShape nodeShape = link.Nodes;
				ModelElement backingElement;
				if (nodeShape.IsDeleted &&
					!linkShape.IsDeleted &&
					null != (backingElement = linkShape.ModelElement) &&
					!backingElement.IsDeleted)
				{
					linkShape.Delete();
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
			ReadingOrder readingOrd = link.ReadingOrder;
			FactType fact = link.FactType;
			ORMModel model = fact.Model;
			if (!fact.IsDeleted && !(fact is SubtypeFact) && model != null)
			{
				Diagram.FixUpDiagram(model, fact); // Make sure the fact is already there
				Diagram.FixUpDiagram(fact, readingOrd);
			}
		}
		[RuleOn(typeof(FactTypeHasReadingOrder), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingOrderAdded : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FixupReadingOrderLink(e.ModelElement as FactTypeHasReadingOrder);
			}
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
		#region RoleName fixup
		/// <summary>
		/// Add shape elements for role names. Used during deserialization fixup
		/// and rules.
		/// </summary>
		[RuleOn(typeof(Role), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class RoleChange : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == Role.NameDomainPropertyId)
				{
						Role role = (Role)e.ModelElement;
						if (!role.IsDeleted)
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
									foreach (PresentationElement element in PresentationViewsSubject.GetPresentation(role.FactType))
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
		private sealed class DisplayRoleNameFixupListener : DeserializationFixupListener<Role>
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
			protected sealed override void ProcessElement(Role role, Store store, INotifyElementAdded notifyAdded)
			{
				if (!role.IsDeleted)
				{
					RoleNameShape.SetRoleNameDisplay(role.FactType);
				}
			}
		}
		#endregion // DisplayRolePlayersFixupListener class
		#endregion // View Fixup Rules
	}
}
