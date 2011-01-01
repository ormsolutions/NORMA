#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams
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
	#region IOffsetBorderPoint interface
	/// <summary>
	/// Interface implemented on a <see cref="ShapeGeometry"/> to support
	/// calculating a secondary border point offset from a known border point.
	/// </summary>
	public interface IOffsetBorderPoint
	{
		/// <summary>
		/// Offset a border point coming into the shape by a given offset, with
		/// the offset measured orthogonal to the incoming line. The goal is to find another
		/// point on the border close to the original border point. All coordinates are
		/// assumed to be absolute.
		/// </summary>
		/// <param name="geometryHost">The <see cref="IGeometryHost"/> to calculate</param>
		/// <param name="borderPoint">An existing point on the border</param>
		/// <param name="outsidePoint">A point outside the shape. The returned point will point
		/// to this value unless <paramref name="parallelVector"/> is set, in which case this
		/// point is used to determine the direction and angle of the reference vector</param>
		/// <param name="offset">The offset to take. Positive values move counterclockwise (increasing angle), negative values move clockwise (decreasing angle)
		/// moving from the outsidePoint to the borderPoint.</param>
		/// <param name="parallelVector">If this is <see langword="true"/>, then the returned point will be parallel to the
		/// incoming line</param>
		/// <returns>A <see cref="Nullable{PointD}"/> either on the border, or <see langword="null"/>.</returns>
		PointD? OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector);
	}
	#endregion // IOffsetBorderPoint interface
	#region IDynamicColorGeometryHost interface
	/// <summary>
	/// Interface to add custom color support to a shape
	/// </summary>
	public interface IDynamicColorGeometryHost
	{
		/// <summary>
		/// Update the color of a <see cref="Pen"/>
		/// </summary>
		/// <param name="penId">The <see cref="StyleSetResourceId"/> of the <paramref name="pen"/></param>
		/// <param name="pen">The <see cref="Pen"/></param>
		/// <returns>The old pen <see cref="Color"/>, or <see cref="Color.Empty"/> if the color was not changed.</returns>
		Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen);
		/// <summary>
		/// Update the color of a <see cref="Brush"/>
		/// </summary>
		/// <param name="brushId">The <see cref="StyleSetResourceId"/> of the <paramref name="brush"/></param>
		/// <param name="brush">The <see cref="Pen"/></param>
		/// <returns>The old brush <see cref="Color"/>, or <see cref="Color.Empty"/> if the color was not changed.</returns>
		Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush);
	}
	#endregion // IDynamicColorGeometryHost interface
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
		/// <param name="vectorEndPoint">PointD (passed from DoFoldToShape)</param>
		/// <returns>Absolute location of end point</returns>
		public static PointD AdjustVectorEndPoint(IGeometryHost geometryHost, PointD vectorEndPoint)
		{
			NodeShape oppositeShape;
			return AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
		}
		/// <summary>
		/// Calculate the rotation angle in radians
		/// </summary>
		/// <param name="sourcePoint">The point to move from</param>
		/// <param name="targetPoint">The point to move towards</param>
		/// <returns>Rotation angle in radians</returns>
		public static double CalculateRadiansRotationAngle(PointD sourcePoint, PointD targetPoint)
		{
			if (VGConstants.FuzzEqual(sourcePoint.X, targetPoint.X, VGConstants.FuzzDistance))
			{
				// Vertical line, get angle by comparing y
				return (sourcePoint.Y > targetPoint.Y) ? Math.PI / 2 : Math.PI / -2;
			}
			else
			{
				double retVal;
				double xDif = targetPoint.X - sourcePoint.X;
				retVal = Math.Atan((targetPoint.Y - sourcePoint.Y) / xDif);
				if (xDif > 0d)
				{
					retVal += Math.PI;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Locate the opposite shape based on the given points and
		/// adjust the opposite the endpoint accordingly. The endpoint
		/// is also modified to represent an absolute value. Use VectorEndPointForBase
		/// to restore the vector endpoint to its natural value.
		/// CenterToCenter routing is assumed.
		/// </summary>
		/// <param name="geometryHost">IGeometryHost (passed from DoFoldToShape)</param>
		/// <param name="vectorEndPoint">PointD (passed from DoFoldToShape)</param>
		/// <param name="oppositeShape">The located opposite shape at this location</param>
		/// <returns>Absolute location of end point</returns>
		public static PointD AdjustVectorEndPoint(IGeometryHost geometryHost, PointD vectorEndPoint, out NodeShape oppositeShape)
		{
			oppositeShape = null;
			// The vectorEndPoint value is coming in (negative, negative) for the lower
			// right quadrant instead of (positive, positive). All other values are
			// (positive, positive), so we switch the end point to make the rest of the work
			// easier. For CenterToCenter routing, subtracting the vectorEndPoint from the
			// lower right corner gives the correct value.
			RectangleD absoluteBoundingBox = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
#if VISUALSTUDIO_10_0
			PointD absoluteCenter = absoluteBoundingBox.Center;
			vectorEndPoint = new PointD(absoluteCenter.X - vectorEndPoint.X, absoluteCenter.Y - vectorEndPoint.Y);
#else
			vectorEndPoint = new PointD(absoluteBoundingBox.Right - vectorEndPoint.X, absoluteBoundingBox.Bottom - vectorEndPoint.Y);
#endif

			NodeShape shape = geometryHost as NodeShape;
			if (shape != null)
			{
				ReadOnlyCollection<LinkConnectsToNode> links = DomainRoleInfo.GetElementLinks<LinkConnectsToNode>(shape, LinkConnectsToNode.NodesDomainRoleId);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					LinkConnectsToNode link = links[i];
					BinaryLinkShape linkShape = link.Link as BinaryLinkShape;
					if (link != null)
					{
						// Get the opposite shape
						NodeShape testShape;
						if (linkShape.FromLinkConnectsToNode == link)
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
		/// <param name="geometryHost">The geometryHost passed to FoldToShape</param>
		/// <param name="vectorEndPoint">An adjusted vector end point</param>
		/// <returns>An unadjusted value</returns>
		public static PointD VectorEndPointForBase(IGeometryHost geometryHost, PointD vectorEndPoint)
		{
			RectangleD absoluteBoundingBox = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
#if VISUALSTUDIO_10_0
			PointD absoluteCenter = absoluteBoundingBox.Center;
			return new PointD(absoluteCenter.X - vectorEndPoint.X, absoluteCenter.X - vectorEndPoint.Y);
#else
			return new PointD(absoluteBoundingBox.Right - vectorEndPoint.X, absoluteBoundingBox.Bottom - vectorEndPoint.Y);
#endif
		}
		#region GeometryHostWrapper class
		/// <summary>
		/// A passthrough implementation of IGeometryHost. Allows IGeometryHost
		/// instances to be forwarded without support for ICustomShapeFolding or
		/// IProxyConnectorShape.
		/// </summary>
		private class GeometryHostWrapper : IGeometryHost
		{
			private IGeometryHost myInner;
			public GeometryHostWrapper(IGeometryHost inner)
			{
				myInner = inner;
			}
			#region IGeometryHost Implementation
			void IGeometryHost.ExcludeGeometryFromClipRegion(Graphics graphics, Matrix matrix, GraphicsPath perimeter)
			{
				myInner.ExcludeGeometryFromClipRegion(graphics, matrix, perimeter);
			}
			StyleSetResourceId IGeometryHost.GeometryBackgroundBrushId
			{
				get
				{
					return myInner.GeometryBackgroundBrushId;
				}
			}
			RectangleD IGeometryHost.GeometryBoundingBox
			{
				get
				{
					return myInner.GeometryBoundingBox;
				}
			}
			bool IGeometryHost.GeometryHasFilledBackground
			{
				get
				{
					return myInner.GeometryHasFilledBackground;
				}
			}
			bool IGeometryHost.GeometryHasOutline
			{
				get
				{
					return myInner.GeometryHasOutline;
				}
			}
			bool IGeometryHost.GeometryHasShadow
			{
				get
				{
					return myInner.GeometryHasShadow;
				}
			}
			StyleSetResourceId IGeometryHost.GeometryOutlinePenId
			{
				get
				{
					return myInner.GeometryOutlinePenId;
				}
			}
			StyleSet IGeometryHost.GeometryStyleSet
			{
				get
				{
					return myInner.GeometryStyleSet;
				}
			}
			RectangleD IGeometryHost.TranslateGeometryToAbsoluteBounds(RectangleD relativeBounds)
			{
				return myInner.TranslateGeometryToAbsoluteBounds(relativeBounds);
			}
			RectangleD IGeometryHost.TranslateGeometryToRelativeBounds(RectangleD absoluteBounds)
			{
				return myInner.TranslateGeometryToRelativeBounds(absoluteBounds);
			}
			Color IGeometryHost.UpdateGeometryLuminosity(DiagramClientView view, Brush brush)
			{
				return myInner.UpdateGeometryLuminosity(view, brush);
			}
			Color IGeometryHost.UpdateGeometryLuminosity(DiagramClientView view, Pen pen)
			{
				return myInner.UpdateGeometryLuminosity(view, pen);
			}
			#endregion // IGeometryHost Implementation
		}
		#endregion // GeometryHostWrapper class
		/// <summary>
		/// A utility function to attempt to perfom custom shape folding.
		/// Custom shape folding uses shape-specific information beyond that
		/// available to the ShapeGeometry to determine connection points.
		/// This function should be call after AdjustVectorEndPoint.
		/// </summary>
		/// <param name="geometryHost">The geometryHost value passed to DoFoldToShape</param>
		/// <param name="vectorEndPoint">The vectorEndPoint passed to DoFoldToShape and adjusted by <see cref="AdjustVectorEndPoint(IGeometryHost,PointD,out NodeShape)"/></param>
		/// <param name="oppositeShape">The opposite shape returned by <see cref="AdjustVectorEndPoint(IGeometryHost,PointD,out NodeShape)"/></param>
		/// <returns>Nullable&lt;PointD&gt; with a value on success, without a value if custom folding is not available</returns>
		public static PointD? DoCustomFoldShape(IGeometryHost geometryHost, PointD vectorEndPoint, NodeShape oppositeShape)
		{
			ICustomShapeFolding customFolding;
			IProxyConnectorShape proxyConnector;
			PointD customPoint;
			NodeShape realHost;
			if (oppositeShape != null &&
				null != (customFolding = geometryHost as ICustomShapeFolding) &&
				!(customPoint = customFolding.CalculateConnectionPoint(oppositeShape)).IsEmpty)
			{
				// Translate back to local coordinates
				PointD location = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox).Location;
				customPoint.Offset(-location.X, -location.Y);
				return customPoint;
			}
			else if (null != (proxyConnector = geometryHost as IProxyConnectorShape) &&
				null != (realHost = proxyConnector.ProxyConnectorShapeFor as NodeShape))
			{
				SizeD size = realHost.Size;
				customPoint = realHost.ShapeGeometry.DoFoldToShape(new GeometryHostWrapper(realHost), new PointD(size.Width / 2, size.Height / 2), GeometryUtility.VectorEndPointForBase(realHost, vectorEndPoint));
				PointD location = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox).Location;
				PointD realLocation = realHost.AbsoluteBounds.Location;
				customPoint.Offset(realLocation.X - location.X, realLocation.Y - location.Y);
				return customPoint;
			}
			return null;
		}
		/// <summary>
		/// Determine if the opposite shape is a proxy connector shape
		/// and adjust the vectorEndPoint appropriately.
		/// </summary>
		/// <param name="vectorEndPoint">The vectorEndPoint passed to DoFoldToShape and adjusted by <see cref="AdjustVectorEndPoint(IGeometryHost,PointD,out NodeShape)"/></param>
		/// <param name="oppositeShape">The opposite shape returned by <see cref="AdjustVectorEndPoint(IGeometryHost,PointD,out NodeShape)"/></param>
		/// <returns>The original or adjusted points.</returns>
		public static PointD ResolveProxyConnectorVectorEndPoint(PointD vectorEndPoint, NodeShape oppositeShape)
		{
			IProxyConnectorShape proxyConnector = oppositeShape as IProxyConnectorShape;
			if (proxyConnector != null)
			{
				NodeShape proxyFor = proxyConnector.ProxyConnectorShapeFor as NodeShape;
				if (proxyFor != null)
				{
					return proxyFor.AbsoluteCenter;
				}
			}
			return vectorEndPoint;
		}

		/// <summary>
		/// Gets the <see cref="PointF"/>s of a triangle that fills the circle that files <paramref name="boundingBox"/>.
		/// </summary>
		/// <param name="boundingBox">The <see cref="RectangleF"/> that the triangle should fill the circle that fills.</param>
		/// <returns>An array of <see cref="PointF"/>s that represent a triangle that fills the circle that fills <paramref name="boundingBox"/>.</returns>
		/// <remarks>
		/// Four <see cref="PointF"/>s are returned in the array.
		/// The first <see cref="PointF"/> (index 0) is the top center corner of the triangle.
		/// The second <see cref="PointF"/> (index 1) is the bottom left corner of the triangle.
		/// The third <see cref="PointF"/> (index 2) is the bottom right corner of the triangle.
		/// The fourth <see cref="PointF"/> (index 3) is the top center corner of the triangle.
		/// The first and fourth <see cref="PointF"/>s are equivalent.
		/// </remarks>
		public static PointF[] GetTrianglePointsF(RectangleF boundingBox)
		{
			float radius = boundingBox.Width / 2f;
			float bottomOffsetX = (float)(Math.Sqrt(3) * radius / 2);
			float bottomOffsetY = radius / 2f;
			float centerX = boundingBox.X + radius;
			float centerY = boundingBox.Y + radius;

			PointF topPoint = new PointF(centerX, boundingBox.Y);
			return new PointF[] { topPoint, new PointF(centerX - bottomOffsetX, centerY + bottomOffsetY), new PointF(centerX + bottomOffsetX, centerY + bottomOffsetY), topPoint };
		}
		/// <summary>
		/// Gets the <see cref="PointD"/>s of a triangle that fills the circle that files <paramref name="boundingBox"/>.
		/// </summary>
		/// <param name="boundingBox">The <see cref="RectangleD"/> that the triangle should fill the circle that fills.</param>
		/// <returns>An array of <see cref="PointD"/>s that represent a triangle that fills the circle that fills <paramref name="boundingBox"/>.</returns>
		/// <remarks>
		/// Four <see cref="PointD"/>s are returned in the array.
		/// The first <see cref="PointD"/> (index 0) is the top center corner of the triangle.
		/// The second <see cref="PointD"/> (index 1) is the bottom left corner of the triangle.
		/// The third <see cref="PointD"/> (index 2) is the bottom right corner of the triangle.
		/// The fourth <see cref="PointD"/> (index 3) is the top center corner of the triangle.
		/// The first and fourth <see cref="PointD"/>s are equivalent.
		/// </remarks>
		public static PointD[] GetTrianglePointsD(RectangleD boundingBox)
		{
			double radius = boundingBox.Width / 2d;
			double bottomOffsetX = (float)(Math.Sqrt(3) * radius / 2);
			double bottomOffsetY = radius / 2d;
			double centerX = boundingBox.X + radius;
			double centerY = boundingBox.Y + radius;

			PointD topPoint = new PointD(centerX, boundingBox.Y);
			return new PointD[] { topPoint, new PointD(centerX - bottomOffsetX, centerY + bottomOffsetY), new PointD(centerX + bottomOffsetX, centerY + bottomOffsetY), topPoint };
		}
		/// <summary>
		/// Paint the background and outline of a <see cref="IGeometryHost"/> using
		/// dynamic colors provided by the <see cref="IDynamicColorGeometryHost"/> interface.
		/// This is a helper method designed to be called by an override of the <see cref="NodeShapeGeometry.DoPaintGeometry"/> method.
		/// </summary>
		/// <param name="e">The <see cref="DiagramPaintEventArgs"/> passed to DoPaintGeometry</param>
		/// <param name="geometryHost">The <see cref="IGeometryHost"/> passed to DoPaintGeometry</param>
		/// <param name="dynamicColors">The <see cref="IDynamicColorGeometryHost"/> retrieved from the <paramref name="geometryHost"/></param>
		/// <param name="shapeGeometry">The <see cref="ShapeGeometry"/> this is a helper for.</param>
		/// <param name="hasFilledBackground">The result of the <see cref="ShapeGeometry.HasFilledBackground"/> method</param>
		/// <param name="hasOutline">The result of the <see cref="ShapeGeometry.HasOutline"/> method</param>
		/// <remarks>
		/// The DoPaintGeometry override should look similar to
		/// <code>
		/// protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		/// {
		///     IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
		///     if (dynamicColors != null)
		///     {
		///         GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
		///     }
		///     else
		///     {
		///         base.DoPaintGeometry(e, geometryHost);
		///     }
		/// }
		/// </code>
		/// </remarks>
		public static void PaintDynamicColorGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost, IDynamicColorGeometryHost dynamicColors, ShapeGeometry shapeGeometry, bool hasFilledBackground, bool hasOutline)
		{
			if (hasFilledBackground || hasOutline)
			{
				StyleSet geometryStyleSet = geometryHost.GeometryStyleSet;
				GraphicsPath path;
				StyleSetResourceId penId = null;
				Pen pen = null;
				StyleSetResourceId brushId = null;
				Brush brush = null;
				if (null != (path = shapeGeometry.GetPath(geometryHost)) &&
					(!hasFilledBackground || null != (brush = geometryStyleSet.GetBrush(brushId = shapeGeometry.GetBackgroundBrushId(geometryHost)))) &&
					(!hasOutline || null != (pen = geometryStyleSet.GetPen(penId = shapeGeometry.GetOutlinePenId(geometryHost)))))
				{
					Graphics g = e.Graphics;
					DiagramClientView clientView = e.View;
					Color restoreColor;
					if (brush != null)
					{
						restoreColor = Color.Empty;
						if (dynamicColors == null ||
							(restoreColor = dynamicColors.UpdateDynamicColor(brushId, brush)).IsEmpty)
						{
							if (clientView != null)
							{
								restoreColor = geometryHost.UpdateGeometryLuminosity(clientView, brush);
							}
						}
						else if (clientView != null)
						{
							geometryHost.UpdateGeometryLuminosity(clientView, brush);
						}
						g.FillPath(brush, path);
						SolidBrush solidBrush = brush as SolidBrush;
						if (!restoreColor.IsEmpty &&
							null != (solidBrush = brush as SolidBrush))
						{
							solidBrush.Color = restoreColor;
						}
					}
					if (pen != null)
					{
						restoreColor = Color.Empty;
						if (dynamicColors == null ||
							(restoreColor = dynamicColors.UpdateDynamicColor(penId, pen)).IsEmpty)
						{
							if (clientView != null)
							{
								restoreColor = geometryHost.UpdateGeometryLuminosity(clientView, pen);
							}
						}
						else if (clientView != null)
						{
							geometryHost.UpdateGeometryLuminosity(clientView, pen);
						}
						SafeDrawPath(g, pen, path);
						if (!restoreColor.IsEmpty)
						{
							pen.Color = restoreColor;
						}
					}
				}
			}
		}
		#region Helper functions (unmodified Reflector copies)
		/// <summary>
		/// Draw a safe path catching common exceptions
		/// </summary>
		/// <remarks>
		/// This is a copy of Microsoft.VisualStudio.Modeling.Diagrams.DrawHelper.SafeDrawPath.
		/// Without the source code, we don't know why this method exists, but when in Rome...
		/// </remarks>
		public static void SafeDrawPath(Graphics g, Pen pen, GraphicsPath path)
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
		/// <summary>
		/// Draw a safe rectangle catching common exceptions
		/// </summary>
		/// <remarks>
		/// This is a copy of Microsoft.VisualStudio.Modeling.Diagrams.DrawHelper.SafeDrawRectangle.
		/// Without the source code, we don't know why this method exists, but when in Rome...
		/// </remarks>
		public static void SafeDrawRectangle(Graphics g, Pen pen, float x, float y, float width, float height)
		{
			try
			{
				if ((g != null) && (pen != null))
				{
					g.DrawRectangle(pen, x, y, width, height);
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
	#endregion // GeometryUtility class
	#region CustomFoldEllipseShapeGeometry class
	/// <summary>
	/// Attach connection lines correctly to an ellipse border. Designed
	/// to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldEllipseShapeGeometry : EllipseShapeGeometry, IOffsetBorderPoint
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
		#region Shape Folding
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
			NodeShape oppositeShape;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
			PointD? customPoint = GeometryUtility.DoCustomFoldShape(geometryHost, vectorEndPoint, oppositeShape);
			if (customPoint.HasValue)
			{
				return customPoint.Value;
			}
			vectorEndPoint = GeometryUtility.ResolveProxyConnectorVectorEndPoint(vectorEndPoint, oppositeShape);

			// The point returned needs to be relative to the upper left corner of the bounding
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
			RectangleD box = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
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
		#endregion // Shape Folding
		#region IOffsetBorderPoint Implementation
		/// <summary>
		/// Implements <see cref="IOffsetBorderPoint.OffsetBorderPoint"/>
		/// </summary>
		protected PointD? OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			double angle = GeometryUtility.CalculateRadiansRotationAngle(outsidePoint, borderPoint);

			// Get the sample point
			PointD samplePoint = borderPoint;
			samplePoint.Offset(-offset * Math.Sin(angle), offset * Math.Cos(angle));

			// Figure out the slope, either parallel to the incoming line, or pointed at the outside point
			PointD slopeThrough = parallelVector ? borderPoint : samplePoint;

			// Translate the ellipse to the origin
			RectangleD bounds = geometryHost.GeometryBoundingBox;
			PointD hostCenter = bounds.Center;
			samplePoint.Offset(-hostCenter.X, -hostCenter.Y);

			double px = samplePoint.X;
			double py = samplePoint.Y;
			double solvedX;
			double solvedY;
			bool checkAlternate = false;
			double solvedXAlternate = 0;
			double solvedYAlternate = 0;

			if (VGConstants.FuzzEqual(slopeThrough.X, outsidePoint.X, VGConstants.FuzzDistance))
			{
				// Vertical line, can't get slope, y = +/-b * sqrt(1-(x0/a)^2)
				double discriminant = px / (bounds.Width / 2);
				discriminant = 1 - discriminant * discriminant;
				if (VGConstants.FuzzZero(discriminant, VGConstants.FuzzGeneral))
				{
					solvedX = px;
					solvedY = 0;
				}
				else if (discriminant < 0)
				{
					// Equation is not solvable
					return null;
				}
				else
				{
					solvedX = px;
					solvedY = (bounds.Height / 2) * Math.Sqrt(discriminant);
					solvedXAlternate = px;
					solvedYAlternate = -solvedY;
					checkAlternate = true;
				}
			}
			else if (VGConstants.FuzzEqual(slopeThrough.Y, outsidePoint.Y, VGConstants.FuzzDistance))
			{
				// Horizontal line, main equation works, but this is a lot cleaner.
				// Switch axes from vertical block
				double discriminant = py / (bounds.Height / 2);
				discriminant = 1 - discriminant * discriminant;
				if (VGConstants.FuzzZero(discriminant, VGConstants.FuzzGeneral))
				{
					solvedY = py;
					solvedX = 0;
				}
				else if (discriminant < 0)
				{
					// Equation is not solvable
					return null;
				}
				else
				{
					solvedY = py;
					solvedX = (bounds.Width / 2) * Math.Sqrt(discriminant);
					solvedYAlternate = py;
					solvedXAlternate = -solvedX;
					checkAlternate = true;
				}
			}
			else
			{
				double slope = (outsidePoint.Y - slopeThrough.Y) / (outsidePoint.X - slopeThrough.X);
				double xRadiusSquared = bounds.Width / 2;
				xRadiusSquared *= xRadiusSquared;
				double yRadiusSquared = bounds.Height / 2;
				yRadiusSquared *= yRadiusSquared;
				// The A/B/C below refer to the quadratic equation (Ax^2 + Bx + C = 0) gives
				// The equations involved are
				// Ellipse equation: x^2/a^2 + y^2/b^2 = 1 for the ellipse (a = width/2, b = height/2, centered at 0,0)
				// Line equation: x = m(x - x0) + y0 (m = slope, x0 = px above, y0 = py above). Solving into
				// quadratic form gives the following equations:
				double quadA = yRadiusSquared / xRadiusSquared + slope * slope;
				double sharedQuadPart = py - slope * px;
				double quadB = (slope + slope) * sharedQuadPart;
				double quadC = sharedQuadPart * sharedQuadPart - yRadiusSquared;
				double discriminant = quadB * quadB - 4 * quadA * quadC;
				if (VGConstants.FuzzZero(discriminant, VGConstants.FuzzGeneral))
				{
					// Tangential line, one possibility only
					solvedX = -quadB / (quadA + quadA);
					solvedY = slope * (solvedX - px) + py;
				}
				else if (discriminant < 0)
				{
					// Equation is not solvable
					return null;
				}
				else
				{
					// We want the point that is closest to the sample point.
					discriminant = Math.Sqrt(discriminant);
					solvedX = (-quadB + discriminant) / (quadA + quadA);
					solvedY = slope * (solvedX - px) + py;
					solvedXAlternate = (-quadB - discriminant) / (quadA + quadA);
					solvedYAlternate = slope * (solvedXAlternate - px) + py;
					checkAlternate = true;
				}
			}

			// Choose the best match and translate the point back out
			if (checkAlternate)
			{
				// Note that simple quadrant checks are no sufficient here, we must
				// find the closest solution to the starting point
				double xDif = px - solvedX;
				double yDif = py - solvedY;
				double xDifAlternate = px - solvedXAlternate;
				double yDifAlternate = py - solvedYAlternate;
				if ((xDif * xDif + yDif * yDif) > (xDifAlternate * xDifAlternate + yDifAlternate * yDifAlternate))
				{
					solvedX = solvedXAlternate;
					solvedY = solvedYAlternate;
				}
			}
			return new PointD(solvedX + hostCenter.X, solvedY + hostCenter.Y);
		}
		PointD? IOffsetBorderPoint.OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			return OffsetBorderPoint(geometryHost, borderPoint, outsidePoint, offset, parallelVector);
		}
		#endregion // IOffsetBorderPoint Implementation
		#region Dynamic Color Support
		/// <summary>
		/// Replacement for <see cref="NodeShapeGeometry.DoPaintGeometry"/> that supports
		/// dynamic background and outline colors.
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
			if (dynamicColors != null)
			{
				GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
			}
			else
			{
				base.DoPaintGeometry(e, geometryHost);
			}
		}
		#endregion // Dynamic Color Support
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
		#region Shape Folding
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
			NodeShape oppositeShape;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
			PointD? customPoint = GeometryUtility.DoCustomFoldShape(geometryHost, vectorEndPoint, oppositeShape);
			if (customPoint.HasValue)
			{
				return customPoint.Value;
			}
			vectorEndPoint = GeometryUtility.ResolveProxyConnectorVectorEndPoint(vectorEndPoint, oppositeShape);

			// The point returned needs to be relative to the upper left corner of the bounding
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
			RectangleD box = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
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
				double x = radius / Math.Sqrt(1 + slope * slope);
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
		#endregion // Shape Folding
		#region Dynamic Color Support
		/// <summary>
		/// Replacement for <see cref="NodeShapeGeometry.DoPaintGeometry"/> that supports
		/// dynamic background and outline colors.
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
			if (dynamicColors != null)
			{
				GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
			}
			else
			{
				base.DoPaintGeometry(e, geometryHost);
			}
		}
		#endregion // Dynamic Color Support
	}
	#endregion // CustomFoldCircleShapeGeometry class
	#region CustomFoldRectangleShapeGeometry class
	/// <summary>
	/// A geometry shape for custom shape folding on rectangles.
	/// Designed to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldRectangleShapeGeometry : RectangleShapeGeometry, IOffsetBorderPoint
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
		#region Shape Folding
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
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
			PointD? customPoint = GeometryUtility.DoCustomFoldShape(geometryHost, vectorEndPoint, oppositeShape);
			if (customPoint.HasValue)
			{
				return customPoint.Value;
			}
			vectorEndPoint = GeometryUtility.ResolveProxyConnectorVectorEndPoint(vectorEndPoint, oppositeShape);

			// Fold to the shape
			// This is used for center to center routing, so the potential point is the
			// center of the shape. We need to see where a line through the center intersects
			// the rectangle border and return relative coordinates.
			RectangleD bounds = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
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
		#endregion // Shape Folding
		#region IOffsetBorderPoint Implementation
		/// <summary>
		/// Implements <see cref="IOffsetBorderPoint.OffsetBorderPoint"/>
		/// </summary>
		protected PointD? OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			double angle = GeometryUtility.CalculateRadiansRotationAngle(outsidePoint, borderPoint);

			// Get the sample point
			PointD samplePoint = borderPoint;
			samplePoint.Offset(-offset * Math.Sin(angle), offset * Math.Cos(angle));

			// Figure out the slope, either parallel to the incoming line, or pointed at the outside point
			PointD slopeThrough = parallelVector ? borderPoint : samplePoint;

			// Translate the rectangle to the origin
			RectangleD bounds = geometryHost.GeometryBoundingBox;
			PointD hostCenter = bounds.Center;
			samplePoint.Offset(-hostCenter.X, -hostCenter.Y);

			double px = samplePoint.X;
			double py = samplePoint.Y;
			double solvedX;
			double solvedY;

			if (VGConstants.FuzzEqual(slopeThrough.X, outsidePoint.X, VGConstants.FuzzDistance))
			{
				// Vertical line, hit the same edge as the border point
				if (Math.Abs(px) > (bounds.Width / 2 + VGConstants.FuzzDistance))
				{
					// Line hits outside rectangle
					return null;
				}
				solvedX = px;
				solvedY = borderPoint.Y - hostCenter.Y;
			}
			else if (VGConstants.FuzzEqual(slopeThrough.Y, outsidePoint.Y, VGConstants.FuzzDistance))
			{
				// Horizontal line, hit the same edge as the border point
				if (Math.Abs(py) > (bounds.Height / 2 + VGConstants.FuzzDistance))
				{
					// Line hits outside rectangle
					return null;
				}
				solvedY = py;
				solvedX = borderPoint.X - hostCenter.X;
			}
			else
			{
				int hitCount = 0;
				solvedX = 0;
				solvedY = 0;
				double solvedXAlternate = 0;
				double solvedYAlternate = 0;

				// We've already checked vertical and horizontal lines, so we know the lines will intersect either
				// zero or two sides of the rectangle. Find the two sides.
				// The intersecting line equation is y = m(x - px) + py (solved for y) or x = 1/m(y-py) + px (solved for x)
				// The rectangle borders are y = halfHeight, y = -halfHeight, x = halfWidth, x = -halfWidth
				double halfWidth = bounds.Width / 2;
				double halfHeight = bounds.Height / 2;

				double slope = (outsidePoint.Y - slopeThrough.Y) / (outsidePoint.X - slopeThrough.X);
				double inverseSlope = 1 / slope;

				double testIntersect;
				
				// Top edge
				testIntersect = inverseSlope * (halfHeight - py) + px;
				if (Math.Abs(testIntersect) < (halfWidth + VGConstants.FuzzDistance))
				{
					solvedX = testIntersect;
					solvedY = halfHeight;
					hitCount = 1;
				}

				// Bottom edge
				testIntersect = inverseSlope * (-halfHeight - py) + px;
				if (Math.Abs(testIntersect) < (halfWidth + VGConstants.FuzzDistance))
				{
					if (hitCount == 1)
					{
						solvedXAlternate = testIntersect;
						solvedYAlternate = -halfHeight;
					}
					else
					{
						solvedX = testIntersect;
						solvedY = -halfHeight;
					}
					++hitCount;
				}

				// Right edge
				if (hitCount != 2)
				{
					testIntersect = slope * (halfWidth - px) + py;
					if (Math.Abs(testIntersect) < (halfHeight + VGConstants.FuzzDistance))
					{
						if (hitCount == 1)
						{
							solvedYAlternate = testIntersect;
							solvedXAlternate = halfWidth;
						}
						else
						{
							solvedY = testIntersect;
							solvedX = halfWidth;
						}
						++hitCount;
					}
				}

				// Left edge
				if (hitCount != 2)
				{
					testIntersect = slope * (-halfWidth - px) + py;
					if (Math.Abs(testIntersect) < (halfHeight + VGConstants.FuzzDistance))
					{
						if (hitCount == 1)
						{
							solvedYAlternate = testIntersect;
							solvedXAlternate = -halfWidth;
						}
						else
						{
							solvedY = testIntersect;
							solvedX = -halfWidth;
						}
						++hitCount;
					}
				}

				// Choose the best match and translate the point back out
				if (hitCount == 2)
				{
					// Find the point closest to the sample point
					double xDif = px - solvedX;
					double yDif = py - solvedY;
					double xDifAlternate = px - solvedXAlternate;
					double yDifAlternate = py - solvedYAlternate;
					if ((xDif * xDif + yDif * yDif) > (xDifAlternate * xDifAlternate + yDifAlternate * yDifAlternate))
					{
						solvedX = solvedXAlternate;
						solvedY = solvedYAlternate;
					}
				}
				else
				{
					// Unsolvable
					return null;
				}
			}
			return new PointD(solvedX + hostCenter.X, solvedY + hostCenter.Y);
		}
		PointD? IOffsetBorderPoint.OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			return OffsetBorderPoint(geometryHost, borderPoint, outsidePoint, offset, parallelVector);
		}
		#endregion // IOffsetBorderPoint Implementation
		#region Dynamic Color Support
		/// <summary>
		/// Replacement for <see cref="NodeShapeGeometry.DoPaintGeometry"/> that supports
		/// dynamic background and outline colors.
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
			if (dynamicColors != null)
			{
				GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
			}
			else
			{
				base.DoPaintGeometry(e, geometryHost);
			}
		}
		#endregion // Dynamic Color Support
	}
	#endregion // CustomFoldRectangleShapeGeometry class
	#region CustomFoldRoundedRectangleShapeGeometry class
	/// <summary>
	/// A geometry shape for custom shape folding on rounded rectangles with a standard
	/// 1.5mm corner radius.
	/// Designed to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldRoundedRectangleShapeGeometry : CustomFoldRoundedRectangleShapeGeometryBase
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
			: base(1.5 / 25.4) // 1.5 mm corner
		{
		}
	}
	/// <summary>
	/// A geometry shape for custom shape folding on rounded rectangles. Derived
	/// concreted classes should provide a singleton and a corner radius to the constructor.
	/// Designed to work with CenterToCenter routing.
	/// </summary>
	public abstract class CustomFoldRoundedRectangleShapeGeometryBase : RoundedRectangleShapeGeometry, IOffsetBorderPoint
	{
		/// <summary>
		/// Protected default constructor. Derived classes should be used
		/// as singletons instead of being publicly constructed.
		/// </summary>
		/// <param name="cornerRadius">The corner radius specified in inches</param>
		protected CustomFoldRoundedRectangleShapeGeometryBase(double cornerRadius)
			: base(cornerRadius)
		{
		}
		#region Shape Folding
		/// <summary>
		/// Provide custom shape folding for rectangular fact types
		/// </summary>
		/// <param name="geometryHost">The host view</param>
		/// <param name="potentialPoint">A point on the rectangular boundary of the shape</param>
		/// <param name="vectorEndPoint">A point on the opposite end of the connecting line</param>
		/// <returns>A point on the rounded rectangle border</returns>
		public override PointD DoFoldToShape(IGeometryHost geometryHost, PointD potentialPoint, PointD vectorEndPoint)
		{
			// Get an endpoint we can work with
			NodeShape oppositeShape;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
			PointD? customPoint = GeometryUtility.DoCustomFoldShape(geometryHost, vectorEndPoint, oppositeShape);
			if (customPoint.HasValue)
			{
				return customPoint.Value;
			}
			vectorEndPoint = GeometryUtility.ResolveProxyConnectorVectorEndPoint(vectorEndPoint, oppositeShape);

			// This is used for center to center routing, so the potential point is the
			// center of the shape. We need to see where a line through the center intersects
			// the rectangle border and return relative coordinates.
			RectangleD bounds = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
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
		#endregion // Shape Folding
		#region IOffsetBorderPoint Implementation
		/// <summary>
		/// Implements <see cref="IOffsetBorderPoint.OffsetBorderPoint"/>
		/// </summary>
		protected PointD? OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			double angle = GeometryUtility.CalculateRadiansRotationAngle(outsidePoint, borderPoint);

			// Get the sample point
			PointD samplePoint = borderPoint;
			samplePoint.Offset(-offset * Math.Sin(angle), offset * Math.Cos(angle));

			// Figure out the slope, either parallel to the incoming line, or pointed at the outside point
			PointD slopeThrough = parallelVector ? borderPoint : samplePoint;

			// Translate the rectangle to the origin
			RectangleD bounds = geometryHost.GeometryBoundingBox;
			PointD hostCenter = bounds.Center;
			double hcx = hostCenter.X;
			double hcy = hostCenter.Y;
			samplePoint.Offset(-hcx, -hcy);
			borderPoint.Offset(-hcx, -hcy);

			double px = samplePoint.X;
			double py = samplePoint.Y;
			double solvedX;
			double solvedY;

			double halfWidth = bounds.Width / 2;
			double halfHeight = bounds.Height / 2;
			double r = Radius;
			if (VGConstants.FuzzEqual(slopeThrough.X, outsidePoint.X, VGConstants.FuzzDistance))
			{
				double absX = Math.Abs(px);
				// Vertical line, hit the same edge as the border point
				if (absX > (halfWidth + VGConstants.FuzzDistance))
				{
					// Line hits outside rectangle
					return null;
				}

				solvedX = px;
				solvedY = halfHeight;
				absX = halfWidth - absX;
				if (absX < r)
				{
					// We're on the rounded corner. Figure out how far down the circle we need to go.
					solvedY -= r - Math.Sqrt(absX * (r + r - absX));
				}
				if (borderPoint.Y < 0)
				{
					solvedY = -solvedY;
				}
			}
			else if (VGConstants.FuzzEqual(slopeThrough.Y, outsidePoint.Y, VGConstants.FuzzDistance))
			{
				double absY = Math.Abs(py);
				// Horizontal line, hit the same edge as the border point
				if (absY > (halfHeight + VGConstants.FuzzDistance))
				{
					// Line hits outside rectangle
					return null;
				}
				solvedY = py;
				solvedX = halfWidth;
				absY = halfHeight - absY;
				if (absY < r)
				{
					// We're on the rounded corner. Figure out how far down the circle we need to go.
					solvedX -= r - Math.Sqrt(absY * (r + r - absY));
				}
				if (borderPoint.X < 0)
				{
					solvedX = -solvedX;
				}
			}
			else
			{
				int hitCount = 0;
				solvedX = 0;
				solvedY = 0;
				double solvedXAlternate = 0;
				double solvedYAlternate = 0;
				PointD? corner; // Use for corner tracking (both the center and the hit points)
				CornerQuadrant quadrant = 0;

				// We've already checked vertical and horizontal lines, so we know the lines will intersect either
				// zero or two sides of the rectangle. Find the two sides.
				// The intersecting line equation is y = m(x - px) + py (solved for y) or x = 1/m(y-py) + px (solved for x)
				// The rectangle borders are y = halfHeight, y = -halfHeight, x = halfWidth, x = -halfWidth
				double slope = (outsidePoint.Y - slopeThrough.Y) / (outsidePoint.X - slopeThrough.X);
				double inverseSlope = 1 / slope;

				double testIntersect;

				// Bottom edge
				testIntersect = inverseSlope * (halfHeight - py) + px;
				if (Math.Abs(testIntersect) < (halfWidth + VGConstants.FuzzDistance))
				{
					solvedX = testIntersect;
					solvedY = halfHeight;
					corner = null;
					if (solvedX > 0)
					{
						if (r > (halfWidth - solvedX))
						{
							corner = new PointD(halfWidth - r, halfHeight - r);
							quadrant = CornerQuadrant.LowerRight;
						}
					}
					else if (r > (halfWidth + solvedX))
					{
						corner = new PointD(-halfWidth + r, halfHeight - r);
						quadrant = CornerQuadrant.LowerLeft;
					}

					if (corner.HasValue)
					{
						corner = FindCornerHit(corner.Value, samplePoint, slope, r, quadrant);
						if (!corner.HasValue)
						{
							return null;
						}
						solvedX = corner.Value.X;
						solvedY = corner.Value.Y;
					}
					hitCount = 1;
				}

				// Top edge
				testIntersect = inverseSlope * (-halfHeight - py) + px;
				if (Math.Abs(testIntersect) < (halfWidth + VGConstants.FuzzDistance))
				{
					solvedXAlternate = testIntersect;
					solvedYAlternate = -halfHeight;
					corner = null;
					if (solvedXAlternate > 0)
					{
						if (r > (halfWidth - solvedXAlternate))
						{
							corner = new PointD(halfWidth - r, -halfHeight + r);
							quadrant = CornerQuadrant.UpperRight;
						}
					}
					else if (r > (halfWidth + solvedXAlternate))
					{
						corner = new PointD(-halfWidth + r, -halfHeight + r);
						quadrant = CornerQuadrant.UpperLeft;
					}
					if (corner.HasValue)
					{
						corner = FindCornerHit(corner.Value, samplePoint, slope, r, quadrant);
						if (!corner.HasValue)
						{
							return null;
						}
						solvedXAlternate = corner.Value.X;
						solvedYAlternate = corner.Value.Y;
					}
					if (hitCount == 0)
					{
						solvedX = solvedXAlternate;
						solvedY = solvedYAlternate;
					}
					++hitCount;
				}

				// Right edge
				if (hitCount != 2)
				{
					testIntersect = slope * (halfWidth - px) + py;
					if (Math.Abs(testIntersect) < (halfHeight + VGConstants.FuzzDistance))
					{
						solvedYAlternate = testIntersect;
						solvedXAlternate = halfWidth;
						corner = null;
						if (solvedYAlternate > 0)
						{
							if (r > (halfHeight - solvedYAlternate))
							{
								corner = new PointD(halfWidth - r, halfHeight - r);
								quadrant = CornerQuadrant.LowerRight;
							}
						}
						else if (r > (halfHeight + solvedYAlternate))
						{
							corner = new PointD(halfWidth - r, -halfHeight + r);
							quadrant = CornerQuadrant.UpperRight;
						}
						if (corner.HasValue)
						{
							corner = FindCornerHit(corner.Value, samplePoint, slope, r, quadrant);
							if (!corner.HasValue)
							{
								return null;
							}
							solvedXAlternate = corner.Value.X;
							solvedYAlternate = corner.Value.Y;
						}
						if (hitCount == 0)
						{
							solvedX = solvedXAlternate;
							solvedY = solvedYAlternate;
						}
						++hitCount;
					}
				}

				// Left edge
				if (hitCount == 1)
				{
					testIntersect = slope * (-halfWidth - px) + py;
					if (Math.Abs(testIntersect) < (halfHeight + VGConstants.FuzzDistance))
					{
						solvedYAlternate = testIntersect;
						solvedXAlternate = -halfWidth;
						corner = null;
						if (solvedYAlternate > 0)
						{
							if (r > (halfHeight - solvedYAlternate))
							{
								corner = new PointD(-halfWidth + r, halfHeight - r);
								quadrant = CornerQuadrant.LowerLeft;
							}
						}
						else if (r > (halfHeight + solvedYAlternate))
						{
							corner = new PointD(-halfWidth + r, -halfHeight + r);
							quadrant = CornerQuadrant.UpperLeft;
						}
						if (corner.HasValue)
						{
							corner = FindCornerHit(corner.Value, samplePoint, slope, r, quadrant);
							if (!corner.HasValue)
							{
								return null;
							}
							solvedXAlternate = corner.Value.X;
							solvedYAlternate = corner.Value.Y;
						}
						++hitCount;
					}
				}

				// Choose the best match and translate the point back out
				if (hitCount == 2)
				{
					// Find the point closest to the sample point
					double xDif = px - solvedX;
					double yDif = py - solvedY;
					double xDifAlternate = px - solvedXAlternate;
					double yDifAlternate = py - solvedYAlternate;
					if ((xDif * xDif + yDif * yDif) > (xDifAlternate * xDifAlternate + yDifAlternate * yDifAlternate))
					{
						solvedX = solvedXAlternate;
						solvedY = solvedYAlternate;
					}
				}
				else
				{
					// Unsolvable
					return null;
				}
			}
			return new PointD(solvedX + hcx, solvedY + hcy);
		}
		private enum CornerQuadrant
		{
			UpperRight,
			UpperLeft,
			LowerLeft,
			LowerRight,
		}
		private static PointD? FindCornerHit(PointD cornerCenter, PointD samplePoint, double slope, double radius, CornerQuadrant quadrant)
		{
			double ccx = cornerCenter.X;
			double ccy = cornerCenter.Y;
			double px = samplePoint.X - ccx;
			double py = samplePoint.Y - ccy;
			double quadA = 1 + slope * slope;
			double sharedQuadPart = py - slope * px;
			double quadB = (slope + slope) * sharedQuadPart;
			double quadC = sharedQuadPart * sharedQuadPart - radius * radius;
			double discriminant = quadB * quadB - 4 * quadA * quadC;
			double solvedX;
			double solvedY;
			if (VGConstants.FuzzZero(discriminant, VGConstants.FuzzGeneral))
			{
				solvedX = -quadB / (quadA + quadA);
				solvedY = slope * (solvedX - px) + py;
				return TestQuadrant(quadrant, solvedX, solvedY) ? new PointD(solvedX + ccx, solvedY + ccy) : new PointD?();
			}
			else if (discriminant < 0)
			{
				return null;
			}
			else
			{
				discriminant = Math.Sqrt(discriminant);
				solvedX = (-quadB + discriminant) / (quadA + quadA);
				solvedY = slope * (solvedX - px) + py;
				bool solvedInQuadrant = TestQuadrant(quadrant, solvedX, solvedY);
				double solvedXAlternate = (-quadB - discriminant) / (quadA + quadA);
				double solvedYAlternate = slope * (solvedXAlternate - px) + py;
				bool solvedInQuadrantAlternate = TestQuadrant(quadrant, solvedXAlternate, solvedYAlternate);
				if (solvedInQuadrant)
				{
					if (solvedInQuadrantAlternate)
					{
						double xDif = px - solvedX;
						double yDif = py - solvedY;
						double xDifAlternate = px - solvedXAlternate;
						double yDifAlternate = py - solvedYAlternate;
						if ((xDif * xDif + yDif * yDif) > (xDifAlternate * xDifAlternate + yDifAlternate * yDifAlternate))
						{
							solvedX = solvedXAlternate;
							solvedY = solvedYAlternate;
						}
					}
				}
				else if (solvedInQuadrantAlternate)
				{
					solvedX = solvedXAlternate;
					solvedY = solvedYAlternate;
				}
				else
				{
					return null;
				}
			}
			return new PointD(solvedX + ccx, solvedY + ccy);
		}
		private static bool TestQuadrant(CornerQuadrant quadrant, double x, double y)
		{
			switch (quadrant)
			{
				case CornerQuadrant.UpperRight:
					return x > -VGConstants.FuzzGeneral && y < VGConstants.FuzzGeneral;
				case CornerQuadrant.UpperLeft:
					return x < VGConstants.FuzzGeneral && y < VGConstants.FuzzGeneral;
				case CornerQuadrant.LowerLeft:
					return x < VGConstants.FuzzGeneral && y > -VGConstants.FuzzGeneral;
				//case CornerQuadrant.LowerRight:
				default:
					return x > -VGConstants.FuzzGeneral && y > -VGConstants.FuzzGeneral;
			}
		}
		PointD? IOffsetBorderPoint.OffsetBorderPoint(IGeometryHost geometryHost, PointD borderPoint, PointD outsidePoint, double offset, bool parallelVector)
		{
			return OffsetBorderPoint(geometryHost, borderPoint, outsidePoint, offset, parallelVector);
		}
		#endregion // IOffsetBorderPoint Implementation
		#region Dynamic Color Support
		/// <summary>
		/// Replacement for <see cref="NodeShapeGeometry.DoPaintGeometry"/> that supports
		/// dynamic background and outline colors.
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
			if (dynamicColors != null)
			{
				GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
			}
			else
			{
				base.DoPaintGeometry(e, geometryHost);
			}
		}
		#endregion // Dynamic Color Support
	}
	#endregion // CustomFoldRoundedRectangleShapeGeometry class
	#region CustomFoldTriangleShapeGeometry class
	/// <summary>
	/// Attach connection lines correctly to triangular border. Designed
	/// to work with CenterToCenter routing.
	/// </summary>
	public class CustomFoldTriangleShapeGeometry : DynamicColorNodeShapeGeometry
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
			// Get an endpoint we can work with
			NodeShape oppositeShape;
			vectorEndPoint = GeometryUtility.AdjustVectorEndPoint(geometryHost, vectorEndPoint, out oppositeShape);
			PointD? customPoint = GeometryUtility.DoCustomFoldShape(geometryHost, vectorEndPoint, oppositeShape);
			if (customPoint.HasValue)
			{
				return customPoint.Value;
			}
			vectorEndPoint = GeometryUtility.ResolveProxyConnectorVectorEndPoint(vectorEndPoint, oppositeShape);
			RectangleD bounds = geometryHost.TranslateGeometryToAbsoluteBounds(geometryHost.GeometryBoundingBox);
			PointD center = bounds.Center;
			PointD[] trianglePoints = GeometryUtility.GetTrianglePointsD(bounds);
			double offsetByX = -center.X;
			double offsetByY = -center.Y;
			for (int i = 0; i < trianglePoints.Length; ++i)
			{
				trianglePoints[i].Offset(offsetByX, offsetByY);
			}
			vectorEndPoint.Offset(offsetByX, offsetByY);
			bool negativeX = vectorEndPoint.X < 0;
			bool negativeY = vectorEndPoint.Y < 0;
			double halfWidth = bounds.Width / 2;
			double halfHeight = bounds.Height / 2;

			if (VGConstants.FuzzZero(vectorEndPoint.X, VGConstants.FuzzDistance))
			{
				// Vertical line, skip slope calculations
				return new PointD(halfWidth, trianglePoints[negativeY ? 0 : 1].Y + halfHeight);
			}
			else if (VGConstants.FuzzZero(vectorEndPoint.Y, VGConstants.FuzzDistance))
			{
				// Horizontal line, skip slope calculations
				// Solve two-point form of line between triangle points for x with y = 0.
				PointD topPoint = trianglePoints[0];
				PointD bottomPoint = trianglePoints[negativeX ? 1 : 2];
				return new PointD(topPoint.X - topPoint.Y * (bottomPoint.X - topPoint.X)/(bottomPoint.Y - topPoint.Y) + halfWidth, halfHeight);
			}
			else
			{
				double slope = vectorEndPoint.Y / vectorEndPoint.X;
				double x;
				double y;
				if (negativeY || (Math.Abs(slope) < (halfHeight / halfWidth))) // Reasonable, but allows y to spill below the bottom
				{
					// Try to attach to the left/right lines
					PointD topPoint = trianglePoints[0];
					PointD bottomPoint = trianglePoints[negativeX ? 1 : 2];
					double inverseTriangleSlope = (bottomPoint.X - topPoint.X) / (bottomPoint.Y - topPoint.Y);
					x = (topPoint.X - topPoint.Y * inverseTriangleSlope) / (1 - slope * inverseTriangleSlope);
					y = slope * x;
					if (y > bottomPoint.Y)
					{
						// Adjust for a y below the bottom point.
						y = bottomPoint.Y;
						x = y / slope;
					}
				}
				else
				{
					// Attach to the bottom edge
					y = trianglePoints[1].Y;
					x = y / slope;
				}
				return new PointD(x + halfWidth, y + halfHeight);
			}
		}

		/// <summary>
		/// See <see cref="M:ShapeGeometry.GetPath"/>.
		/// </summary>
		protected override GraphicsPath GetPath(RectangleD boundingBox)
		{
			GraphicsPath graphicsPath = base.UninitializedPath;
			graphicsPath.Reset();
			graphicsPath.AddPolygon(GeometryUtility.GetTrianglePointsF(RectangleD.ToRectangleF(boundingBox)));
			return graphicsPath;
		}
	}
	#endregion // CustomFoldTriangleShapeGeometry class
	#region DynamicColorNodeShapeGeometry class
	/// <summary>
	/// A version of <see cref="NodeShapeGeometry"/> that supports dynamic colors
	/// specified by implementing the <see cref="IDynamicColorGeometryHost"/> interface
	/// along with <see cref="IGeometryHost"/>
	/// </summary>
	public class DynamicColorNodeShapeGeometry : NodeShapeGeometry
	{
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton instead of being publicly constructed.
		/// </summary>
		protected DynamicColorNodeShapeGeometry()
		{
		}
		/// <summary>
		/// Replacement for <see cref="NodeShapeGeometry.DoPaintGeometry"/> that supports
		/// dynamic background and outline colors.
		/// </summary>
		protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
		{
			IDynamicColorGeometryHost dynamicColors = geometryHost as IDynamicColorGeometryHost;
			if (dynamicColors != null)
			{
				GeometryUtility.PaintDynamicColorGeometry(e, geometryHost, dynamicColors, this, this.HasFilledBackground(geometryHost), this.HasOutline(geometryHost));
			}
			else
			{
				base.DoPaintGeometry(e, geometryHost);
			}
		}
	}
	#endregion // DynamicColorNodeShapeGeometry class
	#region DynamicColorLinkDecorator class
	/// <summary>
	/// Modify <see cref="LinkDecorator"/> to a <see cref="IGeometryHost"/>
	/// that also implements the <see cref="IDynamicColorGeometryHost"/> interface
	/// </summary>
	[Serializable]
	public abstract class DynamicColorLinkDecorator : LinkDecorator
	{
		/// <summary>
		/// Reimplementation of <see cref="LinkDecorator.DoPaintShape"/> that
		/// recognizes <see cref="IDynamicColorGeometryHost"/>
		/// </summary>
		public override void DoPaintShape(RectangleD bounds, IGeometryHost shape, DiagramPaintEventArgs e)
		{
			StyleSet styleSet = shape.GeometryStyleSet;
			GraphicsPath decoratorPath = GetPath(bounds);
			StyleSetResourceId penId = PenId;
			Pen pen = styleSet.GetPen(penId);
			StyleSetResourceId brushId = BrushId;
			Brush brush = styleSet.GetBrush(brushId);
			if (((decoratorPath != null) && (pen != null)) && (brush != null))
			{
				DiagramClientView clientView = e.View;
				IDynamicColorGeometryHost dynamicColors = shape as IDynamicColorGeometryHost;
				Graphics g = e.Graphics;
				Color restoreColor;
				if (FillDecorator)
				{
					restoreColor = Color.Empty;
					if (dynamicColors == null ||
						(restoreColor = dynamicColors.UpdateDynamicColor(brushId, brush)).IsEmpty)
					{
						if (clientView != null)
						{
							restoreColor = shape.UpdateGeometryLuminosity(clientView, brush);
						}
					}
					else if (clientView != null)
					{
						shape.UpdateGeometryLuminosity(clientView, brush);
					}
					g.FillPath(brush, decoratorPath);
					SolidBrush solidBrush;
					if (!restoreColor.IsEmpty &&
						null != (solidBrush = brush as SolidBrush))
					{
						solidBrush.Color = restoreColor;
					}
				}
				restoreColor = Color.Empty;
				if (dynamicColors == null ||
					(restoreColor = dynamicColors.UpdateDynamicColor(penId, pen)).IsEmpty)
				{
					if (clientView != null)
					{
						restoreColor = shape.UpdateGeometryLuminosity(clientView, pen);
					}
				}
				else if (clientView != null)
				{
					shape.UpdateGeometryLuminosity(clientView, pen);
				}
				GeometryUtility.SafeDrawPath(e.Graphics, pen, decoratorPath);
				if (!restoreColor.IsEmpty)
				{
					pen.Color = restoreColor;
				}
			}
		}
	}
	#endregion // DynamicColorLinkDecorator class
	#region DynamicColorDecoratorFilledArrow class
	/// <summary>
	/// A replacement for <see cref="DecoratorFilledArrow"/> that
	/// supports the <see cref="IDynamicColorGeometryHost"/> interface.
	/// </summary>
	[Serializable]
	public class DynamicColorDecoratorFilledArrow : DynamicColorLinkDecorator
	{
		/// <summary>
		/// Singleton instance of this decorator
		/// </summary>
		public static readonly LinkDecorator Decorator = new DynamicColorDecoratorFilledArrow();
		/// <summary>
		/// Create a new DynamicColorDecoratorFilledArrow
		/// </summary>
		protected DynamicColorDecoratorFilledArrow()
		{
			FillDecorator = true;
		}
		/// <summary>
		/// Get the path for a filled arrow.
		/// </summary>
		protected override GraphicsPath GetPath(RectangleD bounds)
		{
			// Decorator paths are draw at 0 radians
			GraphicsPath path = base.DecoratorPath;
			PointF upperCorner = PointD.ToPointF(bounds.Location);
			PointF arrowTip = new PointF((float) bounds.Right, (float) (bounds.Top + (bounds.Height / 2.0)));
			PointF bottomCorner = new PointF((float) bounds.Left, (float) bounds.Bottom);
			path.AddLine(upperCorner, arrowTip);
			path.AddLine(arrowTip, bottomCorner);
			path.AddLine(bottomCorner, upperCorner);
			return path;
		}
	}
	#endregion // DynamicColorDecoratorFilledArrow class
}
