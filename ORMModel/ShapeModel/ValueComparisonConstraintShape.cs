#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ValueComparisonConstraintShape : ExternalConstraintShape, IModelErrorActivation
	{
		#region Customize appearance
		private static readonly StyleSetResourceId OperatorResource = new StyleSetResourceId("ORMArchitect", "ValueComparisonConstraintOperator");
		/// <summary>
		/// Add the operator pen to the style set.
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1 / 72f;
			penSettings.StartCap = LineCap.Square;
			penSettings.EndCap = LineCap.Square;
			classStyleSet.AddPen(OperatorResource, DiagramPens.ShapeOutline, penSettings);
		}
		/// <summary>
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for deontic constraints.
		/// Modifies the outline and sticky background pens.
		/// </summary>
		protected override StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					// Set up an alternate style set for drawing deontic constraints
					retVal = new StyleSet(base.DeonticClassStyleSet);
					PenSettings penSettings = new PenSettings();
					penSettings.DashStyle = DashStyle.Dot;
					retVal.OverridePen(ORMDiagram.StickyBackgroundResource, penSettings);
					retVal.OverridePen(DiagramPens.ShapeOutline, penSettings);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Return the <see cref="ValueComparisonConstraint"/> associated with
		/// this shape.
		/// </summary>
		public ValueComparisonConstraint AssociatedValueComparisonConstraint
		{
			get
			{
				return (ValueComparisonConstraint)this.AssociatedConstraint;
			}
		}
		private const float DOT_FACTOR = 0.35f;
		/// <summary>
		/// Paint the shape contents based on the operator type.
		/// </summary>
		protected override void OnPaintShape(DiagramPaintEventArgs e, ref PaintHelper helper)
		{
			base.OnPaintShape(e, ref helper);
			ValueComparisonConstraint comparisonConstraint = this.AssociatedValueComparisonConstraint;
			RectangleD bounds = this.AbsoluteBounds;
			RectangleF boundsF = RectangleD.ToRectangleF(bounds);
			Graphics g = e.Graphics;
			ValueComparisonOperator comparisonOperator = comparisonConstraint.Operator;
			Brush brush = helper.Brush;

			// Draw the left and right comparison dots
			float heightF = boundsF.Height;
			float dotDiameter = heightF * DOT_FACTOR;
			float dotRadius = dotDiameter / 2;
			float dotTop = boundsF.Top + heightF / 2 - dotRadius;
			g.FillEllipse(brush, boundsF.Left - dotRadius, dotTop, dotDiameter, dotDiameter);
			g.FillEllipse(brush, boundsF.Right - dotRadius, dotTop, dotDiameter, dotDiameter);

			if (comparisonOperator != ValueComparisonOperator.Undefined) // Undefined just draws the error state
			{
				// Draw the operator using a different pen.
				Pen pen = StyleSet.GetPen(OperatorResource);

				// Get the correct pen color from the provided outline pen
				Color startColor = pen.Color;
				Color shapeColor = helper.Pen.Color; // The paint helper has already updated the color, just use it.
				bool updateColor;
				if (updateColor = (startColor != shapeColor))
				{
					pen.Color = shapeColor;
				}

				// Get a clipping rectangle we can draw inside of. This
				// gives us a nice vertical clip on the open side of the
				// comparator, as well as a region safely inside the shape
				// outline.
				const double inscribedRectAngle = 1d / 3 * Math.PI; // 60 degrees from center point of shape to shape border.
				const float equalsOffsetFactor = .8f;
				const double noEqualityPiDivisor = 6d; // 30 degree angle on comparator line
				const double withEqualityPiDivisor = 7.5d; // 24 degree angle on comparator line
				double inscribedCos = Math.Cos(inscribedRectAngle);
				double inscribedSin = Math.Sin(inscribedRectAngle);
				double width = bounds.Width;
				double height = bounds.Height;
				bounds = new RectangleD(
					bounds.Left + (1 - inscribedCos) * width / 2,
					bounds.Top + (1 - inscribedSin) * height / 2,
					inscribedCos * width,
					inscribedSin * height);
				//width = bounds.Width; // not used below
				height = bounds.Height;
				boundsF = RectangleD.ToRectangleF(bounds);
				bool drawComparator = true;
				bool drawEquality = false;
				bool slashEquality = false;

				float penWidth = pen.Width;
				float openSideX = 0f;
				float closedSideX = 0f;
				switch (comparisonOperator)
				{
					case ValueComparisonOperator.LessThanOrEqual:
						drawEquality = true;
						goto case ValueComparisonOperator.LessThan;
					case ValueComparisonOperator.LessThan:
						openSideX = boundsF.Right;
						closedSideX = boundsF.Left + penWidth / 2;
						break;
					case ValueComparisonOperator.GreaterThanOrEqual:
						drawEquality = true;
						goto case ValueComparisonOperator.GreaterThan;
					case ValueComparisonOperator.GreaterThan:
						openSideX = boundsF.Left;
						closedSideX = boundsF.Right - penWidth / 2;
						break;
					case ValueComparisonOperator.Equal:
						drawComparator = false;
						drawEquality = true;
						break;
					case ValueComparisonOperator.NotEqual:
						drawComparator = false;
						drawEquality = true;
						slashEquality = true;
						break;
				}

				double halfHeight = height / 2;
				double top = bounds.Top;
				float middle = (float)(top + halfHeight);
				float topTip;
				float bottomTip;
				if (drawComparator)
				{
					g.SetClip(boundsF);
					// Use a path so that we get a clean join between the top and bottom lines.
					float equalsOffset = drawEquality ? penWidth * equalsOffsetFactor : 0f;
					double offsetSin = Math.Sin(Math.PI / (drawEquality ? withEqualityPiDivisor : noEqualityPiDivisor));
					topTip = (float)(top + halfHeight * (1d - offsetSin));
					bottomTip = (float)(top + halfHeight * (1d + offsetSin));
					using (GraphicsPath path = new GraphicsPath())
					{
						path.AddLines(new PointF[] {
							new PointF(openSideX, topTip - equalsOffset),
							new PointF(closedSideX, middle - equalsOffset),
							new PointF(openSideX, bottomTip - equalsOffset) });
						g.DrawPath(pen, path);
					}

					if (drawEquality)
					{
						bottomTip += equalsOffset;
						g.DrawLine(
							pen,
							boundsF.Left,
							bottomTip,
							boundsF.Right,
							bottomTip);
					}
				}
				else if (drawEquality)
				{
					g.SetClip(boundsF);
					openSideX = boundsF.Left; // Convenient variables, used for left and right
					closedSideX = boundsF.Right;
					float equalsOffset = 1.5f * penWidth;
					topTip = middle - equalsOffset;
					bottomTip = middle + equalsOffset;
					g.DrawLine(pen, openSideX, topTip, closedSideX, topTip);
					g.DrawLine(pen, openSideX, bottomTip, closedSideX, bottomTip);
					if (slashEquality)
					{
						// Clip tighter so that the top and bottom of the lines
						// are clipped horizontally and don't touch the edge lines.
						g.ResetClip();
						boundsF.Inflate(0f, -penWidth * 1.25f);
						g.SetClip(boundsF);
						g.DrawLine(pen, openSideX + penWidth, boundsF.Bottom, closedSideX - penWidth, boundsF.Top);
					}
				}
				g.ResetClip();

				// Restore the pen color
				if (updateColor)
				{
					pen.Color = startColor;
				}
			}
		}
		#endregion // Customize appearance
		#region Shape display update rules
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueComparisonConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ValueComparisonConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ValueComparisonConstraint.OperatorDomainPropertyId)
			{
				ModelElement element = e.ModelElement;
				if (!element.IsDeleted)
				{
					// Redraw the ring constraint wherever it is displayed.
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
					{
						ValueComparisonConstraintShape constraintShape;
						if (null != (constraintShape = pel as ValueComparisonConstraintShape))
						{
							((IInvalidateDisplay)constraintShape).InvalidateRequired(true);
							if (ValueComparisonConstraint.IsDirectionalOperator((ValueComparisonOperator)e.OldValue) ^
								ValueComparisonConstraint.IsDirectionalOperator((ValueComparisonOperator)e.NewValue))
							{
								foreach (LinkShape linkShape in LinkConnectsToNode.GetLink(constraintShape))
								{
									ExternalConstraintLink constraintLink;
									if (null != (constraintLink = linkShape as ExternalConstraintLink))
									{
										((IInvalidateDisplay)constraintLink).InvalidateRequired(true);
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // Shape display update rules
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorActivation.ActivateModelError"/> for
		/// the <see cref="ValueComparisonConstraintOperatorNotSpecifiedError"/>
		/// </summary>
		protected new bool ActivateModelError(ModelError error)
		{
			ValueComparisonConstraintOperatorNotSpecifiedError operatorError;
			ValueComparisonRolesNotComparableError comparabilityError;
			ValueComparisonConstraint constraint;
			Store store;
			bool retVal = true;
			if (null != (operatorError = error as ValueComparisonConstraintOperatorNotSpecifiedError))
			{
				store = Store;
				constraint = operatorError.ValueComparisonConstraint;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					DomainTypeDescriptor.CreatePropertyDescriptor(constraint, ValueComparisonConstraint.OperatorDomainPropertyId),
					true);
			}
			else if (null != (comparabilityError = error as ValueComparisonRolesNotComparableError))
			{
				constraint = comparabilityError.ValueComparisonConstraint;
				LinkedElementCollection<Role> constraintRoles = constraint.RoleCollection;
				Role role1;
				Role role2;
				ObjectTypePlaysRole rolePlayerLink1;
				ObjectTypePlaysRole rolePlayerLink2;
				ObjectType rolePlayer1 = null;
				ObjectType rolePlayer2 = null;
				Role[] valueRoles1 = null;
				Role[] valueRoles2 = null;
				// The default behavior is to activate the role sequence
				// for editing. However, if the problem is with a single
				// resolved value type, and the units are correct, then
				// we need to select the first directly detached object.
				if (constraintRoles.Count == 2 &&
					null != (rolePlayerLink1 = ObjectTypePlaysRole.GetLinkToRolePlayer(role1 = constraintRoles[0])) &&
					null != (rolePlayerLink2 = ObjectTypePlaysRole.GetLinkToRolePlayer(role2 = constraintRoles[1])) &&
					(rolePlayerLink1.RolePlayer == rolePlayerLink2.RolePlayer ||
					(null != (valueRoles1 = role1.GetValueRoles()) &&
					null != (valueRoles2 = role2.GetValueRoles()) &&
					DataType.IsComparableValueType(rolePlayer1 = valueRoles1[0].RolePlayer, rolePlayer2 = valueRoles2[0].RolePlayer, !constraint.IsDirectional))))
				{
					bool verifiedReferenceMode = true;
					if (valueRoles1 != null)
					{
						ORMModel model = null;
						ReferenceMode referenceMode1 = (valueRoles1.Length > 1) ?
							ReferenceMode.FindReferenceModeFromEntityNameAndValueName(rolePlayer1.Name, valueRoles1[1].RolePlayer.Name, model = constraint.ResolvedModel) :
							null;
						ReferenceMode referenceMode2 = (valueRoles2.Length > 1) ?
							ReferenceMode.FindReferenceModeFromEntityNameAndValueName(rolePlayer2.Name, valueRoles2[1].RolePlayer.Name, model ?? constraint.ResolvedModel) :
							null;
						bool referenceMode1IsUnit = referenceMode1 != null && referenceMode1.Kind.ReferenceModeType == ReferenceModeType.UnitBased;
						bool referenceMode2IsUnit = referenceMode2 != null && referenceMode2.Kind.ReferenceModeType == ReferenceModeType.UnitBased;
						verifiedReferenceMode = referenceMode1IsUnit ? (referenceMode2IsUnit && referenceMode1 == referenceMode2) : !referenceMode2IsUnit;
					}
					if (verifiedReferenceMode)
					{
						// Find a connected role player
						foreach (ExternalConstraintLink constraintLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<ExternalConstraintLink>(this))
						{
							FactTypeShape factTypeShape;
							if (constraintLink.AssociatedConstraintRole.Role == role1 &&
								null != (factTypeShape = constraintLink.FromShape as FactTypeShape))
							{
								foreach (RolePlayerLink rolePlayerLinkShape in MultiShapeUtility.GetEffectiveAttachedLinkShapes<RolePlayerLink>(factTypeShape))
								{
									if (rolePlayerLinkShape.AssociatedRolePlayerLink == rolePlayerLink1)
									{
										Diagram.ActiveDiagramView.Selection.Set(new DiagramItem(rolePlayerLinkShape.ToShape));
										return true;
									}
								}
							}
						}
					}
				}
				ActivateNewRoleSequenceConnectAction(null);
			}
			else
			{
				retVal = base.ActivateModelError(error);
			}
			return retVal;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
}
