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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Neumont.Tools.ORM.ShapeModel
{
	#region ICustomShapeFolding interface
	/// <summary>
	/// Support custom connection points during shape folding. If this
	/// interface is not supported, then line routing will assume that
	/// the opposite connection points to the center of the shape.
	/// </summary>
	public interface ICustomShapeFolding
	{
		/// <summary>
		/// Get the connection point corresponding to the opposite shape element.
		/// The value returned uses absolute coordinates.
		/// </summary>
		/// <param name="oppositeShape">The opposite shape to attach to</param>
		/// <returns>An absolute point, or PointD.Empty to proceed with normal shape folding</returns>
		PointD CalculateConnectionPoint(NodeShape oppositeShape);
	}
	#endregion // ICustomShapeFolding interface
	#region GeometryUtility class
	/// <summary>
	/// Helper functions for custom shape folding
	/// </summary>
	public static class GeometryUtility
	{
		/// <summary>
		/// Locate the opposite shape based on the given points and
		/// adjust the opposite the endpoint accordingly. The endpoint
		/// is also modified to represent an absolute value. Use VectorEndPointForBase
		/// to restore the vector endpoint to its natural value.
		/// CenterToCenter routing is assumed.
		/// </summary>
		/// <param name="geometryHost">IGeometryHost (passed from DoFoldToShape)</param>
		/// <param name="potentialPoint">PointD (passed from DoFoldToShape)</param>
		/// <param name="vectorEndPoint">PointD (passed from DoFoldToShape)</param>
		/// <returns>Absolute location of end point</returns>
		public static PointD AdjustVectorEndPoint(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			NodeShape oppositeShape;
			return AdjustVectorEndPoint(geometryHost, potentialPoint, vectorEndPoint, out oppositeShape);
		}
		/// <summary>
		/// Locate the opposite shape based on the given points and
		/// adjust the opposite the endpoint accordingly. The endpoint
		/// is also modified to represent an absolute value. Use VectorEndPointForBase
		/// to restore the vector endpoint to its natural value.
		/// CenterToCenter routing is assumed.
		/// </summary>
		/// <param name="geometryHost">IGeometryHost (passed from DoFoldToShape)</param>
		/// <param name="potentialPoint">PointD (passed from DoFoldToShape)</param>
		/// <param name="vectorEndPoint">PointD (passed from DoFoldToShape)</param>
		/// <param name="oppositeShape">The located opposite shape at this location</param>
		/// <returns>Absolute location of end point</returns>
		public static PointD AdjustVectorEndPoint(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint, out NodeShape oppositeShape)
		{
			oppositeShape = null;
			// The vectorEndPoint value is coming in (negative, negative) for the lower
			// right quadrant instead of (positive, positive). All other values are
			// (positive, positive), so we switch the end point to make the rest of the work
			// easier. For CenterToCenter routing, adjusting by the potential point gives the
			// best value.
			// UNDONE: Should any of this weirdness be considered a bug?
			vectorEndPoint = new PointD(-vectorEndPoint.X + potentialPoint.X, -vectorEndPoint.Y + potentialPoint.Y);
			NodeShape shape = geometryHost as NodeShape;
			if (shape != null)
			{
				IList links = shape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					LinkConnectsToNode link = (LinkConnectsToNode)links[i];
					BinaryLinkShape linkShape = link.Link as BinaryLinkShape;
					if (link != null)
					{
						// Get the opposite shape
						NodeShape testShape;
						if (object.ReferenceEquals(linkShape.FromLinkConnectsToNode, link))
						{
							testShape = linkShape.ToShape;
						}
						else
						{
							testShape = linkShape.FromShape;
						}

						PointD shapeCenter = testShape.AbsoluteCenter;
						if (VGConstants.FuzzEqual(shapeCenter.X, vectorEndPoint.X, VGConstants.FuzzDistance) &&
							VGConstants.FuzzEqual(shapeCenter.Y, vectorEndPoint.Y, VGConstants.FuzzDistance))
						{
							oppositeShape = testShape;
							ICustomShapeFolding customFolding = testShape as ICustomShapeFolding;
							if (customFolding != null)
							{
								PointD customEndPoint = customFolding.CalculateConnectionPoint(shape);
								if (!customEndPoint.IsEmpty)
								{
									vectorEndPoint = customEndPoint;
								}
							}
							break;
						}
					}
				}
			}
			return vectorEndPoint;
		}
		/// <summary>
		/// Adjust a vector end point retrieved from AdjustVectorEndPoint into
		/// the value in its original (very strange) coordinate system.
		/// </summary>
		/// <param name="potentialPoint">The potential end point passed to FoldToShape</param>
		/// <param name="vectorEndPoint">An adjusted vector end point</param>
		/// <returns>An unadjusted value</returns>
		public static PointD VectorEndPointForBase(PointD potentialPoint, PointD vectorEndPoint)
		{
			return new PointD(-(vectorEndPoint.X - potentialPoint.X), -(vectorEndPoint.Y - potentialPoint.Y));
		}

		/// <summary>
		/// Gets the <see cref="System.Drawing.PointF"/>s of a triangle that fills the circle that files <paramref name="boundingBox"/>.
		/// </summary>
		/// <param name="boundingBox">The <see cref="System.Drawing.RectangleF"/> that the triangle should fill the circle that fills.</param>
		/// <returns>An array of <see cref="System.Drawing.PointF"/>s that represent a triangle that fills the circle that fills <paramref name="boundingBox"/>.</returns>
		/// <remarks>
		/// Four <see cref="System.Drawing.PointF"/>s are returned in the array.
		/// The first <see cref="System.Drawing.PointF"/> (index 0) is the top center corner of the triangle.
		/// The second <see cref="System.Drawing.PointF"/> (index 1) is the bottom left corner of the triangle.
		/// The third <see cref="System.Drawing.PointF"/> (index 2) is the bottom right corner of the triangle.
		/// The fourth <see cref="System.Drawing.PointF"/> (index 3) is the top center corner of the triangle.
		/// The first and fourth <see cref="System.Drawing.PointF"/>s are equivalent.
		/// </remarks>
		public static System.Drawing.PointF[] GetTrianglePoints(System.Drawing.RectangleF boundingBox)
		{
			float radius = boundingBox.Width / 2f;
			float bottomOffsetX = (float)(Math.Sqrt(3) * radius / 2);
			float bottomOffsetY = radius / 2f;
			float centerX = boundingBox.X + radius;
			float centerY = boundingBox.Y + radius;

			System.Drawing.PointF topPoint = new System.Drawing.PointF(centerX, boundingBox.Y);
			System.Drawing.PointF leftPoint = new System.Drawing.PointF(centerX - bottomOffsetX, centerY + bottomOffsetY);
			System.Drawing.PointF rightPoint = new System.Drawing.PointF(centerX + bottomOffsetX, centerY + bottomOffsetY);
			return new System.Drawing.PointF[] { topPoint, leftPoint, rightPoint, topPoint };
		}
	}
	#endregion // GeometryUtility class
	#region CustomFoldEllipseShapeGeometry class
	/// <summary>
	/// Attach connection lines correctly to an ellipse border. Designed
	/// to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldEllipseShapeGeometry : EllipseShapeGeometry
	{
		/// <summary>
		/// Singleton CustomFoldEllipseShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new CustomFoldEllipseShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected CustomFoldEllipseShapeGeometry()
		{
		}
		/// <summary>
		/// Implement shape folding on the ellipse boundary
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the ellipse border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			// Get an endpoint we can work with
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, potentialPoint, vectorEndPoint);

			// The point returns needs to be relative to the upper left corner of the bounding
			// box. The goal is to get a point on the ellipse that points to the center of the
			// line. To do this, we translate the coordinate system to the center of the ellipse,
			// get a slope from the vectorEndPoint/ellipse center, then solve the ellipse equation
			// and retranslate the coordinates back out.
			// The quadrant we're coming in from can be determined by the position of the vectorEndPoint
			// relative to the ellipse center.
			//
			// The pertinent equations are:
			// vectorEndPoint (relative point) = (xe, ye)
			// center = (xc, yc)
			// ellipse radii = xr, yr
			// slope = m = (ye - yc)/(xe - xc)
			// line equation: y = mx
			// ellipse equation (centered at origin): x^2/xr^2 + y^2/yr^2 = 1
			// solving gives us: x = +/-((yr*xr)/sqrt(yr^2 + m^2 * xr^2))
			// Plugging back into the line equation gives us a +/- y value
			// Final point = (xc, yc) + (x, y)
			// The quadrant is determined by the relative position of the vectorEndPoint
			RectangleD box = geometryHost.GeometryBoundingBox;
			PointD boxCenter = box.Center;
			double xRadius = box.Width / 2;
			double yRadius = box.Height / 2;

			if (VGConstants.FuzzEqual(vectorEndPoint.X, boxCenter.X, VGConstants.FuzzDistance))
			{
				return new PointD(xRadius, (vectorEndPoint.Y < boxCenter.Y) ? 0 : box.Height);
			}
			else if (VGConstants.FuzzEqual(vectorEndPoint.Y, boxCenter.Y, VGConstants.FuzzDistance))
			{
				return new PointD((vectorEndPoint.X < boxCenter.X) ? 0 : box.Width, yRadius);
			}
			else
			{
				bool negativeX = vectorEndPoint.X < boxCenter.X;
				bool negativeY = vectorEndPoint.Y < boxCenter.Y;

				double slope = (vectorEndPoint.Y - boxCenter.Y) / (vectorEndPoint.X - boxCenter.X);
				double x = (xRadius * yRadius) / Math.Sqrt(yRadius * yRadius + slope * slope * xRadius * xRadius);
				double y = slope * x;
				x = Math.Abs(x);
				y = Math.Abs(y);
				if (negativeX)
				{
					x = -x;
					if (negativeY)
					{
						y = -y;
					}
				}
				else if (negativeY)
				{
					y = -y;
				}
				// Return a point relative to the shape
				return new PointD(x + xRadius, y + yRadius);
			}
		}
	}
	#endregion // CustomFoldEllipseShapeGeometry class
	#region CustomFoldCircleShapeGeometry class
	/// <summary>
	/// Attach connection lines correctly to circular border. Designed
	/// to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldCircleShapeGeometry : CircleShapeGeometry
	{
		/// <summary>
		/// Singleton CustomFoldCircleShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new CustomFoldCircleShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected CustomFoldCircleShapeGeometry()
		{
		}
		/// <summary>
		/// Implement shape folding on the ellipse boundary
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the ellipse border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			// Get an endpoint we can work with
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, potentialPoint, vectorEndPoint);

			// The point returns needs to be relative to the upper left corner of the bounding
			// box. The goal is to get a point on the circle that points to the center of the
			// line. To do this, we translate the coordinate system to the center of the circle,
			// get a slope from the vectorEndPoint/circle center, then solve the circle equation
			// and retranslate the coordinates back out.
			// The quadrant we're coming in from can be determined by the position of the vectorEndPoint
			// relative to the ellipse center.
			//
			// The pertinent equations are:
			// vectorEndPoint (relative point) = (xe, ye)
			// center = (xc, yc)
			// circle radius = r
			// slope = m = (ye - yc)/(xe - xc)
			// line equation: y = mx
			// circle equation (centered at origin): x^2 + y^2 = r^2
			// solving gives us: x = +/-(r/sqrt(1 + m^2))
			// Plugging back into the line equation gives us a +/- y value
			// Final point = (xc, yc) + (x, y)
			// The quadrant is determined by the relative position of the vectorEndPoint
			RectangleD box = geometryHost.GeometryBoundingBox;
			PointD boxCenter = box.Center;
			double radius = Math.Min(box.Width / 2, box.Height / 2);

			if (VGConstants.FuzzEqual(vectorEndPoint.X, boxCenter.X, VGConstants.FuzzDistance))
			{
				return new PointD(box.Width / 2, (vectorEndPoint.Y < boxCenter.Y) ? 0 : box.Height);
			}
			else if (VGConstants.FuzzEqual(vectorEndPoint.Y, boxCenter.Y, VGConstants.FuzzDistance))
			{
				return new PointD((vectorEndPoint.X < boxCenter.X) ? 0 : box.Width, box.Height / 2);
			}
			else
			{
				bool negativeX = vectorEndPoint.X < boxCenter.X;
				bool negativeY = vectorEndPoint.Y < boxCenter.Y;

				double slope = (vectorEndPoint.Y - boxCenter.Y) / (vectorEndPoint.X - boxCenter.X);
				double x = radius  / Math.Sqrt(1 + slope * slope);
				double y = slope * x;
				x = Math.Abs(x);
				y = Math.Abs(y);
				if (negativeX)
				{
					x = -x;
					if (negativeY)
					{
						y = -y;
					}
				}
				else if (negativeY)
				{
					y = -y;
				}
				// Return a point relative to the shape
				return new PointD(x + radius, y + radius);
			}
		}
	}
	#endregion // CustomFoldCircleShapeGeometry class
	#region CustomFoldRectangleShapeGeometry class
	/// <summary>
	/// A geometry shape for custom shape folding on rectangles.
	/// Designed to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldRectangleShapeGeometry : RectangleShapeGeometry
	{
		/// <summary>
		/// Singleton CustomFoldRectangleShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new CustomFoldRectangleShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected CustomFoldRectangleShapeGeometry()
		{
		}
		/// <summary>
		/// Provide custom shape folding for rectangular fact types
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the rectangle edge border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			NodeShape oppositeShape;
			ICustomShapeFolding customFolding;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, potentialPoint, vectorEndPoint, out oppositeShape);
			PointD customPoint;
			if (oppositeShape != null &&
				null != (customFolding = geometryHost as ICustomShapeFolding) &&
				!(customPoint = customFolding.CalculateConnectionPoint(oppositeShape)).IsEmpty)
			{
				// Translate back to local coordinates
				PointD location = geometryHost.GeometryBoundingBox.Location;
				customPoint.Offset(-location.X, -location.Y);
				return customPoint;
			}
			else
			{
				// This is used for center to center routing, so the potential point is the
				// center of the shape. We need to see where a line through the center intersects
				// the rectangle border and return relative coordinates.
				RectangleD bounds = geometryHost.GeometryBoundingBox;
				PointD center = bounds.Center;
				vectorEndPoint.Offset(-center.X, -center.Y);
				bool negativeX = vectorEndPoint.X < 0;
				bool negativeY = vectorEndPoint.Y < 0;
				if (VGConstants.FuzzZero(vectorEndPoint.X, VGConstants.FuzzDistance))
				{
					// Vertical line, skip slope calculations
					return new PointD(bounds.Width / 2, negativeY ? 0 : bounds.Height);
				}
				else if (VGConstants.FuzzZero(vectorEndPoint.Y, VGConstants.FuzzDistance))
				{
					// Horizontal line, skip slope calculations
					return new PointD(negativeX ? 0 : bounds.Width, bounds.Height / 2);
				}
				else
				{
					double slope = vectorEndPoint.Y / vectorEndPoint.X;
					// The intersecting line equation is y = mx. We can tell
					// whether to use the vertical or horizontal lines by
					// comparing the relative sizes of the rectangle sides
					// with the slope
					double x;
					double y;
					if (Math.Abs(slope) < (bounds.Height / bounds.Width))
					{
						// Attach to left/right edges
						// Intersect with line x = +/- bounds.Width / 2
						x = bounds.Width / 2;
						if (negativeX)
						{
							x = -x;
						}
						y = x * slope;
					}
					else
					{
						// Attach to top/bottom edges
						// Intersect with line y = +/- bounds.Height / 2
						y = bounds.Height / 2;
						if (negativeY)
						{
							y = -y;
						}
						x = y / slope;
					}
					return new PointD(x + bounds.Width / 2, y + bounds.Height / 2);
				}
			}
		}
	}
	#endregion // CustomFoldRectangleShapeGeometry class
	#region CustomFoldRoundedRectangleShapeGeometry class
	/// <summary>
	/// A geometry shape for custom shape folding on rectangles
	/// Designed to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldRoundedRectangleShapeGeometry : RoundedRectangleShapeGeometry
	{
		/// <summary>
		/// Singleton CustomFoldRoundedRectangleShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new CustomFoldRoundedRectangleShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected CustomFoldRoundedRectangleShapeGeometry()
		{
		}
		/// <summary>
		/// Provide custom shape folding for rectangular fact types
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the rounded rectangle border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			NodeShape oppositeShape;
			ICustomShapeFolding customFolding;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, potentialPoint, vectorEndPoint, out oppositeShape);
			PointD customPoint;
			if (oppositeShape != null &&
				null != (customFolding = geometryHost as ICustomShapeFolding) &&
				!(customPoint = customFolding.CalculateConnectionPoint(oppositeShape)).IsEmpty)
			{
				// Translate back to local coordinates
				PointD location = geometryHost.GeometryBoundingBox.Location;
				customPoint.Offset(-location.X, -location.Y);
				return customPoint;
			}
			else
			{
				// This is used for center to center routing, so the potential point is the
				// center of the shape. We need to see where a line through the center intersects
				// the rectangle border and return relative coordinates.
				RectangleD bounds = geometryHost.GeometryBoundingBox;
				PointD center = bounds.Center;
				vectorEndPoint.Offset(-center.X, -center.Y);
				bool negativeX = vectorEndPoint.X < 0;
				bool negativeY = vectorEndPoint.Y < 0;
				if (VGConstants.FuzzZero(vectorEndPoint.X, VGConstants.FuzzDistance))
				{
					// Vertical line, skip slope calculations
					return new PointD(bounds.Width / 2, negativeY ? 0 : bounds.Height);
				}
				else if (VGConstants.FuzzZero(vectorEndPoint.Y, VGConstants.FuzzDistance))
				{
					// Horizontal line, skip slope calculations
					return new PointD(negativeX ? 0 : bounds.Width, bounds.Height / 2);
				}
				else
				{
					double slope = vectorEndPoint.Y / vectorEndPoint.X;
					// The intersecting line equation is y = mx. We can tell
					// whether to use the vertical or horizontal lines by
					// comparing the relative sizes of the rectangle sides
					// with the slope
					double x;
					double y;
					double r = Radius;
					double halfHeight = bounds.Height / 2;
					double halfWidth = bounds.Width / 2;
					bool cornerHit;
					if (Math.Abs(slope) < (bounds.Height / bounds.Width))
					{
						// Attach to left/right edges
						// Intersect with line x = +/- bounds.Width / 2
						x = halfWidth;
						if (negativeX)
						{
							x = -x;
						}
						y = x * slope;
						cornerHit = Math.Abs(y) > (halfHeight - r);
					}
					else
					{
						// Attach to top/bottom edges
						// Intersect with line y = +/- bounds.Height / 2
						y = halfHeight;
						if (negativeY)
						{
							y = -y;
						}
						x = y / slope;
						cornerHit = Math.Abs(x) > (halfWidth - r);
					}
					if (cornerHit)
					{
						// The equation here is significantly more complicated than
						// other shapes because of the off center circle, which is
						// centered at (ccx, ccy) in these equations. The raw equations are:
						// (x - ccx)^2 + (y - ccy)^2 = r^2
						// y = m*x where m is the slope
						// Solving for x gives (algebra is non-trivial and ommitted):
						// v1 = 1 + m*m
						// v2 = m*ccx + ccy
						// v3 = sqrt(r^2*v1 - v2^2)
						// x = (+/-v3 + ccx-m*ccy)/v1
						// Note that picking the correct center is absolutely necessary.
						// Unlike the other shapes, where all lines will pass the ellipse/circle
						// at some point, the off-center circle means that choosing the wrong
						// center will result in an unsolvable equation (taking the square
						// root will throw).
						double ccx = halfWidth - r; // Corner center x value
						double ccy = halfHeight - r; // Corner center y value
						bool useNegativeSquareRoot = false;
						if (negativeX) // Left quadrants
						{
							ccx = -ccx;
							useNegativeSquareRoot = true;
							if (!negativeY)
							{
								// Lower left quadrant
								ccy = -ccy;
							}
						}
						else if (!negativeY) // Right quadrants
						{
							// Lower right quadrant
							ccy = -ccy;
						}
						double v1 = 1 + slope * slope;
						double v2 = slope * ccx + ccy;
						double v3 = Math.Sqrt(r * r * v1 - v2 * v2);
						if (useNegativeSquareRoot)
						{
							v3 = -v3;
						}
						x = (v3 + ccx - slope * ccy) / v1;
						y = slope * x;
					}
					return new PointD(x + halfWidth, y + halfHeight);
				}
			}
		}
	}
	#endregion // CustomFoldRoundedRectangleShapeGeometry class
	#region CustomFoldTriangleShapeGeometry class
	/// <summary>
	/// Attach connection lines correctly to triangular border. Designed
	/// to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldTriangleShapeGeometry : NodeShapeGeometry
	{
		/// <summary>
		/// Singleton CustomFoldTriangleShapeGeometry instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new CustomFoldTriangleShapeGeometry();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected CustomFoldTriangleShapeGeometry()
		{
		}
		/// <summary>
		/// Implement shape folding on the triangle boundary
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the triangular border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			// UNDONE: Triangles aren't ellipses, so this isn't going to work right.
			// UNDONE: DoHitTest needs to be overridden as well.
			return potentialPoint;
		}

		/// <summary>
		/// See <see cref="M:ShapeGeometry.GetPath"/>.
		/// </summary>
		protected override System.Drawing.Drawing2D.GraphicsPath GetPath(RectangleD boundingBox)
		{
			System.Drawing.Drawing2D.GraphicsPath graphicsPath = base.UninitializedPath;
			graphicsPath.Reset();
			graphicsPath.AddPolygon(GeometryUtility.GetTrianglePoints(RectangleD.ToRectangleF(boundingBox)));
			return graphicsPath;
		}
	}
	#endregion // CustomFoldTriangleShapeGeometry class
} 
