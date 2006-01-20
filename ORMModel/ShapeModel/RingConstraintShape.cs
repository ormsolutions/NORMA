using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ShapeModel
{
	public sealed partial class RingConstraintShape : ExternalConstraintShape
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
					case RingConstraintType.Irreflexive:
					case RingConstraintType.Acyclic:
					case RingConstraintType.AcyclicIntransitive:
						return RingConstraintOuterShape.Circle;
					case RingConstraintType.Intransitive:
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
		private SizeD myContentSize;
		/// <summary>
		/// See <see cref="P:ExternalConstraintShape.ContentSize"/>.
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				if (this.myContentSize.IsEmpty)
				{
					RingConstraintOuterShape outerShape = this.OuterShape;

					SizeD baseContentSize = (base.ContentSize.IsEmpty ? this.DefaultSize : base.ContentSize);

					double contentSizeHeight = baseContentSize.Height * 1.33;

					if (outerShape == RingConstraintOuterShape.None)
					{
						this.myContentSize = baseContentSize;
					}
					else if (outerShape == RingConstraintOuterShape.Circle || outerShape == RingConstraintOuterShape.Triangle)
					{
						this.myContentSize = new SizeD(contentSizeHeight, contentSizeHeight);
					}
					else // RingConstraintOuterShape.Oval
					{
						this.myContentSize = new SizeD(baseContentSize.Width * 2.0, contentSizeHeight);
					}
				}
				return this.myContentSize;
			}
		}

		/// <summary>
		/// See <see cref="ExternalConstraintShape.OnPaintShape"/>.
		/// </summary>
		public override void OnPaintShape(DiagramPaintEventArgs e)
		{
			this.InitializePaintTools(e);
			base.OnPaintShape(e);

			RingConstraint ringConstraint = this.AssociatedRingConstraint;
			RectangleF boundsF = RectangleD.ToRectangleF(this.AbsoluteBounds);
			Graphics g = e.Graphics;

			PointF midpoint = new PointF(boundsF.Width / 2, boundsF.Height / 2);
			float midpointLeft = boundsF.Left + midpoint.X;
			float midpointTop = boundsF.Top + midpoint.Y;
			float sideLength = boundsF.Width * SMALL_LENGTH_FACTOR;
			float halfSideLength = sideLength / 2;

			switch (ringConstraint.RingType)
			{
				case RingConstraintType.Undefined:
				{
					break;
				}
				case RingConstraintType.Irreflexive:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.Symmetric:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, true);
					break;
				}
				case RingConstraintType.Asymmetric:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, true);
					this.DrawBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.Antisymmetric:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, false);
					this.DrawBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.Intransitive:
				{
					this.DrawTriangleDots(g, boundsF);
					this.DrawTriangleBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.Acyclic:
				{
					this.DrawTriangleDots(g, boundsF);
					this.DrawBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.AcyclicIntransitive:
				{
					this.DrawTriangle(g, boundsF);
					this.DrawTriangleBottomLine(g, boundsF);
					this.DrawTriangleDots(g, boundsF);
					this.DrawBottomLine(g, boundsF);
					break;
				}
				case RingConstraintType.AsymmetricIntransitive:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, true);
					this.DrawBottomLine(g, boundsF);
					RectangleF innerBounds = GetInnerBounds(boundsF);
					this.DrawTriangle(g, innerBounds);
					this.DrawTriangleDots(g, innerBounds);
					this.DrawTriangleBottomLine(g, innerBounds);
					break;
				}
				case RingConstraintType.SymmetricIntransitive:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, true);
					RectangleF innerBounds = GetInnerBounds(boundsF);
					this.DrawTriangle(g, innerBounds);
					this.DrawTriangleDots(g, innerBounds);
					this.DrawTriangleBottomLine(g, innerBounds);
					break;
				}
				case RingConstraintType.SymmetricIrreflexive:
				{
					this.DrawLeftDot(g, boundsF);
					this.DrawRightDot(g, boundsF, true);
					RectangleF innerBounds = GetInnerBounds(boundsF);
					this.DrawEllipse(g, innerBounds);
					this.DrawLeftDot(g, innerBounds);
					this.DrawBottomLine(g, innerBounds);
					break;
				}
			}

			this.DisposePaintTools();
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
		private static PointF GetMidpoint(RectangleF bounds)
		{
			return new PointF(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));
		}
		private void DrawTriangle(Graphics g, RectangleF bounds)
		{
			g.DrawLines(this.PaintPen, GeometryUtility.GetTrianglePoints(bounds));
		}
		private void DrawEllipse(Graphics g, RectangleF bounds)
		{
			g.DrawEllipse(this.PaintPen, bounds);
		}
		private void DrawBottomLine(Graphics g, RectangleF bounds)
		{
			Pen pen = this.PaintPen;
			DashStyle originalDashStyle = pen.DashStyle;
			pen.DashStyle = DashStyle.Solid;
			PointF midpoint = GetMidpoint(bounds);
			float halfLength = (bounds.Height * SMALL_LENGTH_FACTOR) / 2;
			g.DrawLine(pen, midpoint.X, bounds.Bottom - halfLength, midpoint.X, bounds.Bottom + halfLength);
			pen.DashStyle = originalDashStyle;
		}
		private void DrawTriangleBottomLine(Graphics g, RectangleF bounds)
		{
			Pen pen = this.PaintPen;
			DashStyle originalDashStyle = pen.DashStyle;
			pen.DashStyle = DashStyle.Solid;
			float midpointX = GetMidpoint(bounds).X;
			float bottomY = GeometryUtility.GetTrianglePoints(bounds)[1].Y;
			float halfLength = (bounds.Height * SMALL_LENGTH_FACTOR) / 2;
			g.DrawLine(pen, new PointF(midpointX, bottomY - halfLength), new PointF(midpointX, bottomY + (halfLength * 1.15f)));
			pen.DashStyle = originalDashStyle;
		}
		private void DrawLeftDot(Graphics g, RectangleF bounds)
		{
			PointF midpoint = GetMidpoint(bounds);
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			g.FillEllipse(this.PaintBrush, bounds.Left - halfLength, midpoint.Y - halfLength, length, length);
		}
		private void DrawRightDot(Graphics g, RectangleF bounds, bool fillDot)
		{
			PointF midpoint = GetMidpoint(bounds);
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			float x = bounds.Right - halfLength;
			float y = midpoint.Y - halfLength;
			if (fillDot)
			{
				g.FillEllipse(this.PaintBrush, x, y, length, length);
			}
			else
			{
				Pen pen = this.PaintPen;
				DashStyle originalDashStyle = pen.DashStyle;
				pen.DashStyle = DashStyle.Solid;
				g.FillEllipse(this.StyleSet.GetBrush(DiagramBrushes.DiagramBackground), x, y, length, length);
				g.DrawEllipse(this.PaintPen, x, y, length, length);
				pen.DashStyle = originalDashStyle;
			}
		}
		private void DrawTriangleDots(Graphics g, RectangleF bounds)
		{
			Brush brush = this.PaintBrush;
			float length = bounds.Height * SMALL_LENGTH_FACTOR;
			float halfLength = length / 2;
			PointF[] trianglePoints = GeometryUtility.GetTrianglePoints(bounds);
			for (int i = 0; i < 3; ++i)
			{
				PointF point = trianglePoints[i];
				g.FillEllipse(brush, point.X - halfLength, point.Y - halfLength, length, length);
			}
		}

		#endregion // Customize appearance
		#region RingConstraintAttributeChangeRule class
		[RuleOn(typeof(RingConstraint), FireTime = TimeToFire.LocalCommit)]
		private class RingConstraintAttributeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == RingConstraint.RingTypeMetaAttributeGuid)
				{
					RingConstraint ringConstraint = (RingConstraint)e.ModelElement;
					if (!ringConstraint.IsRemoved)
					{
						foreach (object obj in ringConstraint.AssociatedPresentationElements)
						{
							RingConstraintShape ringConstraintShape = obj as RingConstraintShape;
							if (ringConstraintShape != null)
							{
								foreach (LinkConnectsToNode connection in ringConstraintShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
								{
									BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
									if (binaryLink != null)
									{
										binaryLink.RipUp();
									}
								}
								SizeD oldSize = ringConstraintShape.myContentSize;
								ringConstraintShape.myContentSize = SizeD.Empty;
								if (!oldSize.IsEmpty)
								{
									RectangleD bounds = ringConstraintShape.Bounds;
									bounds.Offset(-((ringConstraintShape.ContentSize.Width - oldSize.Width) / 2), 0);
									ringConstraintShape.Bounds = bounds;
								}
								ringConstraintShape.AutoResize();
								ringConstraintShape.Invalidate(true);
							}
						}
					}
				}
			}
		}
		#endregion // RingConstraintAttributeChangeRule class
	}
}
