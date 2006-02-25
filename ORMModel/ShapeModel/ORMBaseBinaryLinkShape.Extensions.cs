#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object Role Modeling Architect for Visual Studio                 *
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

// This file consists of overrides and replacements for code in the
// BinaryLinkShape and BinaryLinkShapeGeometry code provided by Microsoft.
// These wholesale replacements are required because of painfully lacking
// support in the area of link decorators. The following is not supported
// directly by Microsoft:
// 1) Decorators always have size {.1, .1}
// 2) Decorators can only have N/S/E/W rotation angles
// 3) Decorators are always rotated
// 4) The attach point for the decorator cannot be moved along the connection line
namespace Neumont.Tools.ORM.ShapeModel
{
	#region ILinkDecoratorSettings interface
	/// <summary>
	/// Implement on a decorator object to supply additional settings, such
	/// as a bounding size, to the decorator routines.
	/// </summary>
	public interface ILinkDecoratorSettings
	{
		/// <summary>
		/// Get the size of the decorator. The default is LinkShapeGeometry.SizeDecorator,
		/// which is set at {.1, .1}
		/// </summary>
		SizeD DecoratorSize { get;}
		/// <summary>
		/// Get the distance by which the decorator should be shifted from the end
		/// of the line. A positive value indicates moving off the end of the line,
		/// a negative value indicates moving closer to the middle of the line.
		/// </summary>
		double OffsetBy { get;}
	}
	#endregion // ILinkDecoratorSettings interface
	#region ORMBaseBinaryLinkShape class
	/// <summary>
	/// A base link shape class for sharing common code and for reimplementing
	/// LinkShape code that does not property support oblique lines
	/// </summary>
	public partial class ORMBaseBinaryLinkShape
	{
		#region Replacement implementations
		/// <summary>
		/// Hack override of BinaryLinkShape.ShapeGeometry
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return ClickThroughObliqueBinaryLinkShapeGeometry.ShapeGeometry;
			}
		}
		/// <summary>
		/// Override of BinaryLinkShape.ExcludeFromClipRegion to support moving the link
		/// decorator and properly rotating it.
		/// </summary>
		public override void ExcludeFromClipRegion(Graphics g, Matrix matrix, GraphicsPath perimeter)
		{
			ObliqueBinaryLinkShapeGeometry geometry = this.ShapeGeometry as ObliqueBinaryLinkShapeGeometry;
			Pen linePen = this.StyleSet.GetPen(DiagramPens.ConnectionLine);
			EdgePointCollection edgePoints = this.EdgePoints;
			int edgePointCount = edgePoints.Count;
			if ((geometry != null) && (linePen != null))
			{
				for (int i = 1; i < edgePointCount; ++i)
				{
					if (edgePoints[i].Flag == VGPointType.JumpEnd)
					{
						GraphicsPath excludePath = this.ExcludePath;
						geometry.AddLineArcPath(excludePath, edgePoints[i - 1].Point, edgePoints[i].Point);
						g.SetClip(excludePath, CombineMode.Exclude);
					}
					else
					{
						RectangleD excludeRect = GeometryHelpers.RectangleDFrom2Pts(edgePoints[i - 1].Point, edgePoints[i].Point);
						double inflateBy = linePen.Width / 2f;
						excludeRect.Inflate(inflateBy, inflateBy);
						g.SetClip(RectangleD.ToRectangleF(excludeRect), CombineMode.Exclude);
					}
				}
				if (edgePointCount > 1)
				{
					LinkDecorator decorator;
					if (null != (decorator = DecoratorFrom))
					{
						ExcludeDecorator(g, geometry, decorator, edgePoints[0].Point, edgePoints[1].Point);
					}
					if (null != (decorator = DecoratorTo))
					{
						ExcludeDecorator(g, geometry, decorator, edgePoints[edgePointCount - 1].Point, edgePoints[edgePointCount - 2].Point);
					}
				}
			}
		}
		private static void ExcludeDecorator(Graphics g, ObliqueBinaryLinkShapeGeometry geometry, LinkDecorator decorator, PointD fromPoint, PointD toPoint)
		{
			SizeD size = LinkShapeGeometry.SizeDecorator;
			ILinkDecoratorSettings settings = decorator as ILinkDecoratorSettings;
			double offsetBy = double.NaN;
			if (settings != null)
			{
				size = settings.DecoratorSize;
				offsetBy = settings.OffsetBy;
				if (VGConstants.FuzzZero(offsetBy, VGConstants.FuzzDistance))
				{
					offsetBy = double.NaN;
				}
			}
			RectangleD bounds = new RectangleD(fromPoint.X - size.Width, fromPoint.Y - (size.Height / 2), size.Width, size.Height);
			float rotationAngle = geometry.CalculateRotationAngle(fromPoint, toPoint);
			ExcludeDecorator(g, bounds, rotationAngle, offsetBy, fromPoint);
		}
		private static void ExcludeDecorator(Graphics g, RectangleD bounds, float rotation, double offsetBy, PointD centerRight)
		{
			bool doOffset = !double.IsNaN(offsetBy);
			Matrix rotationMatrix = g.Transform;
			float offsetX = 0f;
			float offsetY = 0f;
			if (doOffset)
			{
				double rotationRadians = rotation * Math.PI / 180;
				offsetX = (float)(offsetBy * Math.Cos(rotationRadians));
				offsetY = (float)(offsetBy * Math.Sin(rotationRadians));
				rotationMatrix.Translate(offsetX, offsetY);
			}
			rotationMatrix.RotateAt(rotation, PointD.ToPointF(centerRight));
			g.Transform = rotationMatrix;
			g.SetClip(RectangleD.ToRectangleF(bounds), CombineMode.Exclude);
			rotationMatrix.RotateAt(-rotation, PointD.ToPointF(centerRight));
			if (doOffset)
			{
				rotationMatrix.Translate(-offsetX, -offsetY);
			}
			g.Transform = rotationMatrix;
		}
		#endregion // Replacement implementations
	}
	#endregion // ORMBaseBinaryLinkShape class
	#region ClickThroughObliqueBinaryLinkShapeGeometry class
	/// <summary>
	/// A shape geometry that always fails hit testing, so that it cannot interfere
	/// with other click regions on the diagram
	/// </summary>
	public class ClickThroughObliqueBinaryLinkShapeGeometry : LinkShapeGeometry
	{
		#region Constructor and singleton
		/// <summary>
		/// Singleton ObliqueBinaryLinkShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new ClickThroughObliqueBinaryLinkShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected ClickThroughObliqueBinaryLinkShapeGeometry()
		{
		}
		#endregion // Constructor and singleton
		#region Empty DoHitTest implementation
		/// <summary>
		/// Don't let the lines interfere with clicking on other items
		/// </summary>
		public override bool DoHitTest(IGeometryHost geometryHost, PointD hitPoint, DiagramHitTestInfo hitTestInfo, bool includeTolerance)
		{
			return false;
		}
		#endregion // Empty DoHitTest implementation
		#region Reimplementations of BinaryLinkShapeGeometry functions
		/// <summary>
		/// Replacement for BinaryLinkShapeGeometry.DoPaintGeometry
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			Graphics g = e.Graphics;
			GraphicsPath path = this.GetPath(geometryHost);
			Pen pen = geometryHost.GeometryStyleSet.GetPen(this.GetOutlinePenId(geometryHost));
			if ((path != null) && (pen != null))
			{
				if (this.HasOutline(geometryHost))
				{
					Color restoreColor = geometryHost.UpdateGeometryLuminosity(e.View, pen);
					SafeDrawPath(g, pen, path);
					pen.Color = restoreColor;
				}
				IBinaryLinkGeometryData hostData;
				EdgePointCollection edgePoints;
				int edgePointCount;
				if (null != (hostData = geometryHost as IBinaryLinkGeometryData) &&
					null != (edgePoints = hostData.GeometryEdgePoints) &&
					1 < (edgePointCount = edgePoints.Count))
				{
					float rotationAngle = 0f;
					if (hostData.GeometryDecoratorFrom != null)
					{
						rotationAngle = CalculateRotationAngle(edgePoints[0].Point, edgePoints[1].Point);
						DrawDecorator(e, geometryHost, rotationAngle, edgePoints[0].Point, hostData.GeometryDecoratorFrom);
					}
					if ((hostData.GeometryDecoratorTo != null) && (hostData.GeometryEdgePoints.Count > 1))
					{
						rotationAngle = CalculateRotationAngle(edgePoints[edgePointCount - 1].Point, edgePoints[edgePointCount - 2].Point);
						DrawDecorator(e, geometryHost, rotationAngle, edgePoints[edgePointCount - 1].Point, hostData.GeometryDecoratorTo);
					}
				}
			}
		}
		/// <summary>
		/// Replacement for LinkShapeGeometry.DrawDecorator
		/// </summary>
		protected static new void DrawDecorator(DiagramPaintEventArgs e, IGeometryHost geometryHost, float rotation, PointD centerRight, LinkDecorator decorator)
		{
			SizeD size = LinkShapeGeometry.SizeDecorator;
			double offsetBy = 0d;
			bool doOffset = false;
			ILinkDecoratorSettings settings = decorator as ILinkDecoratorSettings;
			if (settings != null)
			{
				size = settings.DecoratorSize;
				offsetBy = settings.OffsetBy;
				doOffset = !VGConstants.FuzzZero(offsetBy, VGConstants.FuzzDistance);
			}
			Graphics g = e.Graphics;
			RectangleD boundingRect = new RectangleD(centerRight.X - size.Width, centerRight.Y - (size.Height / 2), size.Width, size.Height);
			Matrix rotationMatrix = g.Transform;
			float offsetX = 0f;
			float offsetY = 0f;
			if (doOffset)
			{
				double rotationRadians = rotation * Math.PI / 180;
				offsetX = (float)(offsetBy * Math.Cos(rotationRadians));
				offsetY = (float)(offsetBy * Math.Sin(rotationRadians));
				rotationMatrix.Translate(offsetX, offsetY);
			}
			rotationMatrix.RotateAt(rotation, PointD.ToPointF(centerRight));
			g.Transform = rotationMatrix;
			decorator.DoPaintShape(boundingRect, geometryHost, e);
			rotationMatrix.RotateAt(-rotation, PointD.ToPointF(centerRight));
			if (doOffset)
			{
				rotationMatrix.Translate(-offsetX, -offsetY);
			}
			g.Transform = rotationMatrix;
		}
		/// <summary>
		/// Replacement for LinkShapeGeometry.CalculateRotationAngle
		/// </summary>
		/// <param name="pt1">From point</param>
		/// <param name="pt2">To point</param>
		/// <returns>Rotation angle in degrees</returns>
		public new virtual float CalculateRotationAngle(PointD pt1, PointD pt2)
		{
			if (VGConstants.FuzzEqual(pt1.X, pt2.X, VGConstants.FuzzDistance))
			{
				// Vertical line, get angle by comparing y
				return (pt1.Y > pt2.Y) ? 90f : 270f;
			}
			else
			{
				double retVal;
				double xDif = pt2.X - pt1.X;
				retVal = Math.Atan((pt2.Y - pt1.Y) / xDif);
				if (xDif > 0d)
				{
					retVal += Math.PI;
				}
				return (float)(retVal / Math.PI * 180d);
			}
		}
		#endregion // Reimplementations of BinaryLinkShapeGeometry functions
		#region Helper functions (unmodified Reflector copies)
		/// <summary>
		/// Pulled directly from Reflector disassembly
		/// </summary>
		private static void SafeDrawPath(Graphics g, Pen pen, GraphicsPath path)
		{
			try
			{
				if ((g != null) && (pen != null))
				{
					g.DrawPath(pen, path);
				}
			}
			catch (OutOfMemoryException)
			{
				if (pen.DashStyle == DashStyle.Solid)
				{
					throw;
				}
			}
			catch (OverflowException)
			{
			}
		}
		#endregion // Helper functions (unmodified Reflector copies)
	}
	#endregion // ClickThroughObliqueBinaryLinkShapeGeometry class
	#region ObliqueBinaryLinkShapeGeometry class
	/// <summary>
	/// Rewrite of BinaryLinkShapeGeometry to add proper support
	/// for drawing decorators on oblique lines and properly hit
	/// testing the line
	/// </summary>
	public class ObliqueBinaryLinkShapeGeometry : ClickThroughObliqueBinaryLinkShapeGeometry
	{
		#region Constructor and singleton
		/// <summary>
		/// Singleton ObliqueBinaryLinkShapeGeometry instance
		/// </summary>
		public static new readonly ShapeGeometry ShapeGeometry = new ObliqueBinaryLinkShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected ObliqueBinaryLinkShapeGeometry()
		{
		}
		#endregion // Constructor and singleton
		#region DoHitTest implementation
		/// <summary>
		/// Override of DoHitTest so it works with non-rectilinear line segments
		/// </summary>
		public override bool DoHitTest(IGeometryHost geometryHost, PointD hitPoint, DiagramHitTestInfo hitTestInfo, bool includeTolerance)
		{
			bool retVal = false;
			LineSegment hitSegment = null;
			AnchorPoint anchorPoint = null;
			SizeD tolerance = base.GetHitTestTolerance(hitTestInfo);
			RectangleD perimeter = this.GetPerimeterBoundingBox(geometryHost);
			perimeter.Inflate(tolerance);
			if (perimeter.Contains(hitPoint))
			{
				LineSegment[] segments = this.CalculateSegments(geometryHost, hitTestInfo);
				int segmentCount = segments.Length;
				for (int i = 0; i < segmentCount; ++i)
				{
					LineSegment testSegment = segments[i];
					RectangleD testBounds = GeometryHelpers.RectangleDFrom2Pts(testSegment.StartPoint, testSegment.EndPoint);
					testBounds.Inflate(tolerance);
					if (testBounds.Contains(hitPoint))
					{
						anchorPoint = TestHitAnchor(geometryHost as BinaryLinkShape, testSegment, tolerance, hitPoint);
						if (anchorPoint != null)
						{
							retVal = true;
							hitSegment = testSegment;
							break;
						}
						double distance = DistanceFromPointToLine(hitPoint, testSegment.StartPoint, testSegment.EndPoint, true);
						if (!double.IsNaN(distance) && distance < (tolerance.Width + geometryHost.GeometryStyleSet.GetPen(geometryHost.GeometryOutlinePenId).Width / 2f))
						{
							retVal = true;
							hitSegment = testSegment;
							break;
						}
					}
				}
			}
			if (hitTestInfo != null)
			{
				DiagramItem diagramItem;
				if (retVal)
				{
					if (anchorPoint == null)
					{
						diagramItem = this.CreateDiagramItem(geometryHost, hitSegment);
					}
					else
					{
						diagramItem = new DiagramItem(geometryHost as LinkShape, hitSegment, anchorPoint);
					}
				}
				else
				{
					diagramItem = null;
				}
				hitTestInfo.HitDiagramItem = diagramItem;
				hitTestInfo.HitGrabHandle = null;
			}
			return retVal;
		}
		#endregion // DoHitTest implementation
		#region Point to line code
		// Adapted from http://astronomy.swin.edu.au/~pbourke/geometry/pointline/source.c based on
		// algorithm at http://astronomy.swin.edu.au/~pbourke/geometry/pointline/
		private static double Magnitude(PointD point1, PointD point2)
		{
			double x = point2.X - point1.X;
			double y = point2.Y - point1.Y;
			return Math.Sqrt(x * x + y * y);
		}
		/// <summary>
		/// Calculcate the distance from a point to a line
		/// </summary>
		/// <param name="point">The point to test distance from.</param>
		/// <param name="lineStart">The starting point for the line</param>
		/// <param name="lineEnd">The end point for the line</param>
		/// <param name="enforceSeqment">True to only return a number if the shortest
		/// distance is within the specified line segment</param>
		/// <returns>A distance, or NaN if enforceSeqment is true and the point misses</returns>
		private static double DistanceFromPointToLine( PointD point, PointD lineStart, PointD lineEnd, bool enforceSeqment)
		{
			double segmentLength = Magnitude(lineStart, lineEnd);
			if (VGConstants.FuzzZero(segmentLength, VGConstants.FuzzDistance))
			{
				return Magnitude(point, lineStart);
			}

			double u = (((point.X - lineStart.X) * (lineEnd.X - lineStart.X)) +
				((point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y))) /
				(segmentLength * segmentLength);

			if (enforceSeqment && (u < 0d || u > 1d))
			{
				// closest point does not fall within the line segment
				return double.NaN;
			}

			// Return the distance from the point to the intersection
			return Magnitude(point, new PointD(lineStart.X + u * (lineEnd.X - lineStart.X), lineStart.Y + u * (lineEnd.Y - lineStart.Y)));
		}
		#endregion // Point to line code
		#region Helper functions (unmodified Reflector copies)
		/// <summary>
		/// Pulled directly from Reflector disassembly
		/// </summary>
		private LineSegment[] CalculateSegments(IGeometryHost geometryHost, DiagramHitTestInfo hitTestInfo)
		{
			IBinaryLinkGeometryData data1 = geometryHost as IBinaryLinkGeometryData;
			EdgePointCollection collection1 = data1.GeometryEdgePointsNoJumps;
			LineSegment[] segmentArray1 = new LineSegment[collection1.Count - 1];
			Pen pen1 = geometryHost.GeometryStyleSet.GetPen(this.GetOutlinePenId(geometryHost));
			if (pen1 != null)
			{
				for (int num1 = 0; num1 < (collection1.Count - 1); num1++)
				{
					RectangleD ed1 = GeometryHelpers.RectangleDFrom2Pts(collection1[num1].Point, collection1[num1 + 1].Point);
					SizeD ed2 = base.GetHitTestTolerance(hitTestInfo);
					if (ed1.Height < ed2.Height)
					{
						ed1.Inflate(0, (pen1.Width / 2f) + ed2.Height);
					}
					else if (ed1.Width < ed2.Width)
					{
						ed1.Inflate((pen1.Width / 2f) + ed2.Width, 0);
					}
					segmentArray1[num1] = new LineSegment(collection1[num1].Point, collection1[num1 + 1].Point, num1, num1 + 1, num1 == 0, (num1 + 1) == (collection1.Count - 1), ed1);
				}
			}
			return segmentArray1;
		}
		/// <summary>
		/// Pulled directly from Reflector disassembly
		/// </summary>
		private static AnchorPoint TestHitAnchor(BinaryLinkShape linkShape, LineSegment segment, SizeD tolerance, PointD hitPoint)
		{
			RectangleD ed1 = new RectangleD(0, 0, tolerance.Width * 2, tolerance.Height * 2);
			NodeShape shape1 = null;
			bool flag1 = false;
			if (linkShape != null)
			{
				PointD td1;
				if (segment.IsStartSegment && segment.IsEndSegment)
				{
					if (ClosestEnd(segment, hitPoint))
					{
						td1 = segment.StartPoint;
						shape1 = linkShape.FromShape;
						flag1 = true;
					}
					else
					{
						td1 = segment.EndPoint;
						shape1 = linkShape.ToShape;
					}
				}
				else if (segment.IsStartSegment)
				{
					td1 = segment.StartPoint;
					shape1 = linkShape.FromShape;
					flag1 = true;
				}
				else if (segment.IsEndSegment)
				{
					td1 = segment.EndPoint;
					shape1 = linkShape.ToShape;
				}
				else
				{
					return null;
				}
				if ((shape1 != null) && !shape1.IsPort)
				{
					ed1.Offset(td1.X - tolerance.Width, td1.Y - tolerance.Height);
					if (ed1.Contains(hitPoint))
					{
						return new AnchorPoint(linkShape, segment, shape1, tolerance, flag1);
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Pulled directly from Reflector disassembly
		/// </summary>
		private static bool ClosestEnd(LineSegment segment, PointD hitPoint)
		{
			PointD td1 = segment.StartPoint;
			PointD td2 = segment.EndPoint;
			double num1 = Math.Abs((double)(td1.X - hitPoint.X)) + Math.Abs((double)(td1.Y - hitPoint.Y));
			double num2 = Math.Abs((double)(td2.X - hitPoint.X)) + Math.Abs((double)(td2.Y - hitPoint.Y));
			return (num1 < num2);
		}
		#endregion // Helper functions (unmodified Reflector copies)
	}
	#endregion // ObliqueBinaryLinkShapeGeometry class
}
