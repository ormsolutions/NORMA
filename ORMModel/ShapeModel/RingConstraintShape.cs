#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class RingConstraintShape : ExternalConstraintShape, IModelErrorActivation
	{
		#region Customize appearance
		/// <summary>
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for deontic constraints
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

		private RingConstraint AssociatedRingConstraint
		{
			get
			{
				return (RingConstraint)this.AssociatedConstraint;
			}
		}

		private enum RingConstraintOuterShape
		{
			None = 0,
			Circle = 1,
			Oval = 2,
			Triangle = 3
		}

		private RingConstraintOuterShape OuterShape
		{
			get
			{
				RingConstraint constraint = this.AssociatedRingConstraint;
				RingConstraintType ringConstraintType = (constraint != null) ? constraint.RingType : RingConstraintType.Undefined;
				switch ((constraint != null) ? constraint.RingType : RingConstraintType.Undefined)
				{
					case RingConstraintType.Undefined:
						return RingConstraintOuterShape.None;
					case RingConstraintType.Reflexive:
					case RingConstraintType.PurelyReflexive:
					case RingConstraintType.Irreflexive:
					case RingConstraintType.Acyclic:
					case RingConstraintType.AcyclicTransitive:
					case RingConstraintType.AcyclicIntransitive:
					case RingConstraintType.AcyclicStronglyIntransitive:
					case RingConstraintType.ReflexiveTransitive:
					case RingConstraintType.TransitiveIrreflexive:
						return RingConstraintOuterShape.Circle;
					case RingConstraintType.Transitive:
					case RingConstraintType.Intransitive:
					case RingConstraintType.StronglyIntransitive:
						return RingConstraintOuterShape.Triangle;
					default:
						return RingConstraintOuterShape.Oval;
				}
			}
		}

		/// <summary>
		/// See <see cref="ExternalConstraintShape.ShapeGeometry"/>.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				switch (this.OuterShape)
				{
					case RingConstraintOuterShape.Circle:
						return CustomFoldCircleShapeGeometry.ShapeGeometry;
					case RingConstraintOuterShape.Oval:
						return CustomFoldEllipseShapeGeometry.ShapeGeometry;
					case RingConstraintOuterShape.Triangle:
						return CustomFoldTriangleShapeGeometry.ShapeGeometry;
					default:
						return base.ShapeGeometry;
				}
			}
		}
		/// <summary>
		/// Return a standard size (4/3 of the normal constraint size) for
		/// drawing ring constraints, with an additional 50% stretch for the width.
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				RingConstraintOuterShape outerShape = this.OuterShape;
				SizeD contentSize = this.DefaultSize;
				return (outerShape == RingConstraintOuterShape.Oval) ? 
					new SizeD(contentSize.Width * 1.5f, contentSize.Height) :
					contentSize;
			}
		}

		/// <summary>
		/// Draw all ring constraint types
		/// </summary>
		protected override void OnPaintShape(DiagramPaintEventArgs e, ref PaintHelper helper)
		{
			base.OnPaintShape(e, ref helper);
			RingConstraint ringConstraint = this.AssociatedRingConstraint;
			RectangleF boundsF = RectangleD.ToRectangleF(this.AbsoluteBounds);
			Graphics g = e.Graphics;
			RingConstraintType ringType = ringConstraint.RingType;
			RectangleF innerBounds;
			Pen pen = helper.Pen;
			Brush brush = helper.Brush;

			switch (ringType)
			{
				case RingConstraintType.Undefined:
					break;
				case RingConstraintType.Reflexive:
					DrawLeftDot(g, boundsF, brush);
					break;
				case RingConstraintType.PurelyReflexive:
					DrawLeftDot(g, boundsF, brush);
					{
						float height = boundsF.Height;
						float left = boundsF.Left + boundsF.Width / 2;
						DashStyle originalDashStyle = pen.DashStyle;
						pen.DashStyle = DashStyle.Solid;
						g.DrawLine(pen, left, boundsF.Top + height * (.5f - SMALL_LENGTH_FACTOR / 2), left, boundsF.Top + height * (.5f + SMALL_LENGTH_FACTOR / 2));
						pen.DashStyle = originalDashStyle;
					}
					break;
				case RingConstraintType.Irreflexive:
					DrawLeftDot(g, boundsF, brush);
					DrawRightLine(g, boundsF, pen, 1f);
					break;
				case RingConstraintType.Asymmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, true);
					DrawBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.Antisymmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, false);
					DrawBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.Transitive:
					DrawTriangleDots(g, boundsF, brush, false);
					break;
				case RingConstraintType.Intransitive:
				case RingConstraintType.StronglyIntransitive:
					DrawTriangleDots(g, boundsF, brush, ringType == RingConstraintType.StronglyIntransitive);
					DrawTriangleBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.Acyclic:
					DrawTriangleDots(g, boundsF, brush, false);
					DrawBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.AcyclicTransitive:
					DrawTriangle(g, boundsF, pen);
					DrawTriangleDots(g, boundsF, brush, false);
					DrawBottomLine(g, boundsF, pen, .6f);
					break;
				case RingConstraintType.AcyclicIntransitive:
				case RingConstraintType.AcyclicStronglyIntransitive:
					DrawTriangle(g, boundsF, pen);
					DrawTriangleBottomLine(g, boundsF, pen);
					DrawTriangleDots(g, boundsF, brush, ringType == RingConstraintType.AcyclicStronglyIntransitive);
					DrawBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.TransitiveAsymmetric:
				case RingConstraintType.TransitiveAntisymmetric:
				case RingConstraintType.AsymmetricIntransitive:
				case RingConstraintType.AsymmetricStronglyIntransitive:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, ringType != RingConstraintType.TransitiveAntisymmetric);
					DrawBottomLine(g, boundsF, pen);
					innerBounds = GetInnerBounds(boundsF);
					DrawTriangle(g, innerBounds, pen);
					DrawTriangleDots(g, innerBounds, brush, ringType == RingConstraintType.AsymmetricStronglyIntransitive);
					switch (ringType)
					{
						case RingConstraintType.AsymmetricIntransitive:
						case RingConstraintType.AsymmetricStronglyIntransitive:
							DrawTriangleBottomLine(g, innerBounds, pen);
							break;
					}
					break;
				case RingConstraintType.ReflexiveTransitive:
				case RingConstraintType.TransitiveIrreflexive:
					DrawLeftDot(g, boundsF, brush);
					if (ringType == RingConstraintType.TransitiveIrreflexive)
					{
						DrawRightLine(g, boundsF, pen, .75f);
					}
					innerBounds = GetInnerBounds(boundsF);
					DrawTriangle(g, innerBounds, pen);
					DrawTriangleDots(g, innerBounds, brush, false);
					break;
				case RingConstraintType.Symmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, true);
					break;
				case RingConstraintType.SymmetricTransitive:
				case RingConstraintType.SymmetricIntransitive:
				case RingConstraintType.SymmetricStronglyIntransitive:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, true);
					innerBounds = GetInnerBounds(boundsF);
					DrawTriangle(g, innerBounds, pen);
					DrawTriangleDots(g, innerBounds, brush, ringType == RingConstraintType.SymmetricStronglyIntransitive);
					if (ringType != RingConstraintType.SymmetricTransitive)
					{
						DrawTriangleBottomLine(g, innerBounds, pen);
					}
					break;
				case RingConstraintType.SymmetricIrreflexive:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, true);
					innerBounds = GetReflexiveInSymmetricBounds(boundsF);
					DrawEllipse(g, innerBounds, pen);
					DrawRightLine(g, innerBounds, pen, 1f);
					break;
				case RingConstraintType.ReflexiveSymmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, true);
					DrawEllipse(g, GetReflexiveInSymmetricBounds(boundsF), pen);
					break;
				case RingConstraintType.ReflexiveAntisymmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, false);
					DrawEllipse(g, GetReflexiveInSymmetricBounds(boundsF), pen);
					DrawBottomLine(g, boundsF, pen);
					break;
				case RingConstraintType.ReflexiveTransitiveAntisymmetric:
					DrawLeftDot(g, boundsF, brush);
					DrawRightDot(g, boundsF, pen, brush, false);
					DrawBottomLine(g, boundsF, pen);
					innerBounds = GetInnerBounds(boundsF);
					DrawTriangle(g, innerBounds, pen);
					DrawTriangleDots(g, innerBounds, brush, false);
					g.DrawArc(pen, GetReflexiveInSymmetricBounds(boundsF), 27, 300); // Angles determined empirically to stop on triangle, not worth calculating
					break;
			}
		}

		private const float SMALL_LENGTH_FACTOR = 0.35f;
		private static RectangleF GetInnerBounds(RectangleF bounds)
		{
			float innerBoundsLength = bounds.Height * 0.6f;
			float halfInnerBoundsLength = innerBoundsLength / 2;
			float innerBoundsX = bounds.X + (bounds.Width / 2) - halfInnerBoundsLength;
			float innerBoundsY = bounds.Y + (bounds.Height / 2) - halfInnerBoundsLength;
			return new RectangleF(innerBoundsX, innerBoundsY, innerBoundsLength, innerBoundsLength);
		}
		private static RectangleF GetReflexiveInSymmetricBounds(RectangleF bounds)
		{
			float startHeight = bounds.Height;
			float adjustedHeight = startHeight * .72f;
			return new RectangleF(bounds.X, bounds.Y + (startHeight - adjustedHeight) / 2, adjustedHeight, adjustedHeight);
		}
		private static PointF GetMidpoint(RectangleF bounds)
		{
			return new PointF(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));
		}
		private static void DrawTriangle(Graphics g, RectangleF bounds, Pen pen)
		{
			g.DrawLines(pen, GeometryUtility.GetTrianglePointsF(bounds));
		}
		private static void DrawEllipse(Graphics g, RectangleF bounds, Pen pen)
		{
			g.DrawEllipse(pen, bounds);
		}
		private static void DrawBottomLine(Graphics g, RectangleF bounds, Pen pen)
		{
			DrawBottomLine(g, bounds, pen, 1f);
		}
		private static void DrawBottomLine(Graphics g, RectangleF bounds, Pen pen, float topPercent)
		{
			DashStyle originalDashStyle = pen.DashStyle;
			pen.DashStyle = DashStyle.Solid;
			PointF midpoint = GetMidpoint(bounds);
			float halfLength = (bounds.Height * SMALL_LENGTH_FACTOR) / 2;
			g.DrawLine(pen, midpoint.X, bounds.Bottom - topPercent * halfLength, midpoint.X, bounds.Bottom + halfLength);
			pen.DashStyle = originalDashStyle;
		}
		private static void DrawRightLine(Graphics g, RectangleF bounds, Pen pen, float leftPercent)
		{
			DashStyle originalDashStyle = pen.DashStyle;
			pen.DashStyle = DashStyle.Solid;
			PointF midpoint = GetMidpoint(bounds);
			float halfLength = (bounds.Height * SMALL_LENGTH_FACTOR) / 2;
			g.DrawLine(pen, bounds.Right - leftPercent * halfLength, midpoint.Y, bounds.Right + halfLength, midpoint.Y);
			pen.DashStyle = originalDashStyle;
		}
		private static void DrawTriangleBottomLine(Graphics g, RectangleF bounds, Pen pen)
		{
			DashStyle originalDashStyle = pen.DashStyle;
			pen.DashStyle = DashStyle.Solid;
			float midpointX = GetMidpoint(bounds).X;
			float bottomY = GeometryUtility.GetTrianglePointsF(bounds)[1].Y;
			float halfLength = (bounds.Height * SMALL_LENGTH_FACTOR) / 2;
			g.DrawLine(pen, new PointF(midpointX, bottomY - halfLength), new PointF(midpointX, bottomY + (halfLength * 1.15f)));
			pen.DashStyle = originalDashStyle;
		}
		private static void DrawLeftDot(Graphics g, RectangleF bounds, Brush brush)
		{
			PointF midpoint = GetMidpoint(bounds);
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			g.FillEllipse(brush, bounds.Left - halfLength, midpoint.Y - halfLength, length, length);
		}
		private void DrawRightDot(Graphics g, RectangleF bounds, Pen pen, Brush brush, bool fillDot)
		{
			PointF midpoint = GetMidpoint(bounds);
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			float x = bounds.Right - halfLength;
			float y = midpoint.Y - halfLength;
			if (fillDot)
			{
				g.FillEllipse(brush, x, y, length, length);
			}
			else
			{
				DashStyle originalDashStyle = pen.DashStyle;
				pen.DashStyle = DashStyle.Solid;
				g.FillEllipse(this.StyleSet.GetBrush(DiagramBrushes.DiagramBackground), x, y, length, length);
				g.DrawEllipse(pen, x, y, length, length);
				pen.DashStyle = originalDashStyle;
			}
		}
		private static void DrawTriangleDots(Graphics g, RectangleF bounds, Brush brush, bool middleRightDot)
		{
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			PointF[] trianglePoints = GeometryUtility.GetTrianglePointsF(bounds);
			for (int i = 0; i < 3; ++i)
			{
				PointF point = trianglePoints[i];
				g.FillEllipse(brush, point.X - halfLength, point.Y - halfLength, length, length);
			}
			if (middleRightDot)
			{
				g.FillEllipse(brush, (trianglePoints[0].X + trianglePoints[2].X) / 2 - halfLength, (trianglePoints[0].Y + trianglePoints[2].Y) / 2 - halfLength, length, length);
			}
		}

		#endregion // Customize appearance
		#region Base overrides
		/// <summary>
		/// Maintain center position on resize
		/// </summary>
		public override void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty)
			{
				RectangleD oldBounds = (RectangleD)AbsoluteBounds;
				if (!(oldBounds.IsEmpty ||
					Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ORMBaseShape.PlaceAllChildShapes)))
				{
					SizeD oldSize = oldBounds.Size;
					double xDelta = contentSize.Width - oldSize.Width;
					double yDelta = contentSize.Height - oldSize.Height;
					bool xChanged = !VGConstants.FuzzZero(xDelta, VGConstants.FuzzDistance);
					bool yChanged = !VGConstants.FuzzZero(yDelta, VGConstants.FuzzDistance);
					if (xChanged || yChanged)
					{
						PointD location = oldBounds.Location;
						location.Offset(xChanged ? -xDelta / 2 : 0d, yChanged ? -yDelta / 2 : 0d);
						AbsoluteBounds = new RectangleD(location, contentSize);
						return;
					}
				}
				Size = contentSize;
			}
		}
		#endregion // Base overrides
		#region RingConstraintPropertyChangeRule
		/// <summary>
		/// ChangeRule: typeof(RingConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void RingConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == RingConstraint.RingTypeDomainPropertyId)
			{
				RingConstraint ringConstraint = (RingConstraint)e.ModelElement;
				if (!ringConstraint.IsDeleted)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(ringConstraint))
					{
						RingConstraintShape ringConstraintShape = pel as RingConstraintShape;
						if (ringConstraintShape != null)
						{
							foreach (LinkConnectsToNode connection in DomainRoleInfo.GetElementLinks<LinkConnectsToNode>(ringConstraintShape, LinkConnectsToNode.NodesDomainRoleId))
							{
								BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
								if (binaryLink != null)
								{
									binaryLink.RecalculateRoute();
								}
							}
							SizeD oldSize = ringConstraintShape.Size;
							ringConstraintShape.AutoResize();
							if (oldSize == ringConstraintShape.Size)
							{
								ringConstraintShape.InvalidateRequired(true);
							}
						}
					}
				}
			}
		}
		#endregion // RingConstraintPropertyChangeRule
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		protected new bool ActivateModelError(ModelError error)
		{
			RingConstraintTypeNotSpecifiedError ringTypeError;
			bool retVal = true;
			if (null != (ringTypeError = error as RingConstraintTypeNotSpecifiedError))
			{
				Store store = Store;
				RingConstraint constraint = ringTypeError.RingConstraint;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					DomainTypeDescriptor.CreatePropertyDescriptor(constraint, RingConstraint.RingTypeDomainPropertyId),
					true);
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
