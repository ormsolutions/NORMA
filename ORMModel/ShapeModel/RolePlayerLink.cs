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

// Defining LINKS_ALWAYS_CONNECT allows multiple links from a single ShapeA to different instances of ShapeB.
// In this case, the 'anchor' end is always connected if an opposite shape is available.
// The current behavior is to only create a link if, given an instance of ShapeA, the closest candidate
// ShapeB instance is not closer to a different instance of ShapeA.
// Note that LINKS_ALWAYS_CONNECT is also used in other files, so you should turn this on
// in the project properties if you want to experiment. This is here for reference only.
//#define LINKS_ALWAYS_CONNECT

//#define IMPLIEDJOINPATH

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class RolePlayerLink : IReconfigureableLink
	{
#if IMPLIEDJOINPATH
		#region ImpliedFactJoinPathDecorator class
		/// <summary>
		/// A decorator used to display the join path role boxes for
		/// implied facts when we're in join path editing mode. We only
		/// decorate one end of a role player link, so we are able to use
		/// the opposite decorator for nefarious purposes such as this.
		/// </summary>
		protected class ImpliedFactJoinPathDecorator : LinkDecorator, ILinkDecoratorSettings
		{
			// UNDONE: Constants stolen from FactTypeShape, do something about it
			private const double RoleBoxHeight = 0.11;
			private const double RoleBoxWidth = 0.16;
			private RolePlayerLink myLinkShape;
			/// <summary>
			/// Create an ImpliedFactJoinPathDecorator
			/// </summary>
			/// <param name="linkShape">The associated link shape</param>
			public ImpliedFactJoinPathDecorator(RolePlayerLink linkShape)
			{
				myLinkShape = linkShape;
			}
			/// <summary>
			/// Return a circle slightly smaller than the standard decorator
			/// as the path
			/// </summary>
			/// <param name="bounds">A bounding rectangle for the decorator</param>
			/// <returns>A circle path</returns>
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				bounds.Height /= 2;
				if (IsFlipped)
				{
					bounds.Y += bounds.Height;
				}
				path.AddRectangle(RectangleD.ToRectangleF(bounds));
				return path;
			}
			private bool IsFlipped
			{
				get
				{
					PointD fromPoint = myLinkShape.FromEndPoint;
					PointD toPoint = myLinkShape.ToEndPoint;
					return toPoint.X < fromPoint.X;
				}
			}
		#region ILinkDecoratorSettings Implementation
			/// <summary>
			/// Implements ILinkDecoratorSettings.DecoratorSize.
			/// </summary>
			protected static SizeD DecoratorSize
			{
				get
				{
					return new SizeD(2 * RoleBoxWidth, 2 * RoleBoxHeight);
				}
			}
			SizeD ILinkDecoratorSettings.DecoratorSize
			{
				get
				{
					return DecoratorSize;
				}
			}
			/// <summary>
			/// Implements ILinkDecoratorSettings.OffsetBy
			/// </summary>
			protected double OffsetBy
			{
				get
				{
					// Note: FromShape == FactTypeShape, so the endpoint
					// is inside the shape. The ToShape will always have the
					// endpoint attaching at the shape, so we do not have to
					// recalculate our attach point
					PointD fromPoint = myLinkShape.FromEndPoint;
					PointD toPoint = myLinkShape.ToEndPoint;
					double xDif = toPoint.X - fromPoint.X;
					double yDif = toPoint.Y - fromPoint.Y;
					return -Math.Sqrt(xDif * xDif + yDif * yDif) / 2 + RoleBoxWidth ;
				}
			}
			double ILinkDecoratorSettings.OffsetBy
			{
				get
				{
					return OffsetBy;
				}
			}
		#endregion // ILinkDecoratorSettings Implementation
		}
		#endregion // ImpliedFactJoinPathDecorator class
#endif // IMPLIEDJOINPATH
		#region MandatoryDotDecorator class
		/// <summary>
		/// The link decorator used to draw the mandatory
		/// constraint dot on a link.
		/// </summary>
		protected class MandatoryDotDecorator : LinkDecorator, ILinkDecoratorSettings
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static readonly LinkDecorator Decorator = new MandatoryDotDecorator();
			private MandatoryDotDecorator()
			{
				FillDecorator = true;
			}
			/// <summary>
			/// Return a circle slightly smaller than the standard decorator
			/// as the path
			/// </summary>
			/// <param name="bounds">A bounding rectangle for the decorator</param>
			/// <returns>A circle path</returns>
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				path.AddArc(RectangleD.ToRectangleF(bounds), 0, 360);
				return path;
			}

			#region ILinkDecoratorSettings Implementation
			/// <summary>
			/// Implements ILinkDecoratorSettings.DecoratorSize.
			/// </summary>
			protected static SizeD DecoratorSize
			{
				get
				{
					return new SizeD(.075d, .075d);
				}
			}
			SizeD ILinkDecoratorSettings.DecoratorSize
			{
				get
				{
					return DecoratorSize;
				}
			}
			/// <summary>
			/// Implements ILinkDecoratorSettings.OffsetBy
			/// </summary>
			protected static double OffsetBy
			{
				get
				{
					return .0375d;
				}
			}
			double ILinkDecoratorSettings.OffsetBy
			{
				get
				{
					return OffsetBy;
				}
			}
			#endregion // ILinkDecoratorSettings Implementation
		}
		#endregion // MandatoryDotDecorator class
		#region Customize appearance
		/// <summary>
		/// Draw the mandatory dot on the role box end, depending
		/// on the options settings
		/// </summary>
		public override LinkDecorator DecoratorFrom
		{
			get
			{
				if ((OptionsPage.CurrentMandatoryDotPlacement != MandatoryDotPlacement.ObjectShapeEnd ||
					OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay != EntityRelationshipBinaryMultiplicityDisplay.Off) &&
					DrawMandatoryDot)
				{
					return MandatoryDotDecorator.Decorator;
				}
#if IMPLIEDJOINPATH
				else if (OptionsPage.CurrentMandatoryDotPlacement == MandatoryDotPlacement.ObjectShapeEnd &&
						OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay == EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					return new ImpliedFactJoinPathDecorator(this);
				}
#endif // IMPLIEDJOINPATH
				return base.DecoratorFrom;
			}
			set
			{
			}
		}
		/// <summary>
		/// Draw the mandatory dot on the object type end, depending
		/// on the options settings
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				if (OptionsPage.CurrentMandatoryDotPlacement != MandatoryDotPlacement.RoleBoxEnd &&
					OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay == EntityRelationshipBinaryMultiplicityDisplay.Off &&
					DrawMandatoryDot)
				{
					return MandatoryDotDecorator.Decorator;
				}
#if IMPLIEDJOINPATH
				else if (OptionsPage.CurrentMandatoryDotPlacement == MandatoryDotPlacement.RoleBoxEnd ||
						OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay != EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					return new ImpliedFactJoinPathDecorator(this);
				}
#endif // IMPLIEDJOINPATH
				return base.DecoratorTo;
			}
			set
			{
			}
		}
		/// <summary>
		/// Helper function to determine if we should draw a mandatory dot
		/// </summary>
		protected bool DrawMandatoryDot
		{
			get
			{
				bool retVal = false;
				ObjectTypePlaysRole link;
				Role role;
				FactType factType;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRole)) &&
					role.IsMandatory &&
					// Do not draw the dot on a Binarized Unary
					!(null != (factType = role.FactType) && factType.UnaryRole != null))
				{
					retVal = true;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Switch between alethic and deontic style sets to draw
		/// the mandatory dot correctly
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				Role role;
				ObjectTypePlaysRole link;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRole)) &&
					role.IsMandatory &&
					role.MandatoryConstraintModality == ConstraintModality.Deontic)
				{
					// Note that we don't do anything with fonts with this style set, so the
					// static one is sufficient. Instance style sets also go through a font initiation
					// step inside the framework
					return DeonticClassStyleSet;
				}
				return base.StyleSet;
			}
		}
		/// <summary>
		/// Pen to draw dotted line on optional ER roles
		/// </summary>
		private static readonly StyleSetResourceId BarkerEROptionalPen = new StyleSetResourceId("Neumont", "BarkerEROptionalPen");
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.2F / 72.0F; // 1.2 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(BarkerEROptionalPen, DiagramPens.ConnectionLine, penSettings);
			IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
			Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.Constraint);
			penSettings = new PenSettings();
			penSettings.Color = constraintForeColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = constraintForeColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// A style set used for drawing deontic mandatory decorators
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(ClassStyleSet);
					IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
					Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.DeonticConstraint);
					PenSettings penSettings = new PenSettings();
					penSettings.Color = constraintForeColor;
					retVal.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
					SolidBrush backgroundBrush = retVal.GetBrush(DiagramBrushes.DiagramBackground) as SolidBrush;
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = (backgroundBrush == null) ? constraintForeColor : backgroundBrush.Color;
					retVal.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		#endregion // Customize appearance
		#region EntityRelationship learning mode support
		private const double CrowsFootHeight = .12;
		private const double CrowsFootHalfWidth = .05;
		private const double InfEngOuterOneMarkOffset = .03;
		private const double InfEngInnerOneMarkOffset = .06;
		private const double InfEngMarkerHalfWidth = .04;
		private const bool CrowsFootParallelMode = true;
		#region EntityRelationshipShapeGeometry class
		private sealed class EntityRelationshipShapeGeometry : ClickThroughObliqueBinaryLinkShapeGeometry
		{
			#region Constructor and singleton
			/// <summary>
			/// Singleton EntityRelationshipShapeGeometry instance
			/// </summary>
			public static new readonly ShapeGeometry ShapeGeometry = new EntityRelationshipShapeGeometry();
			/// <summary>
			/// Protected default constructor. The class should be used
			/// as a singleton instead of being publicly constructed.
			/// </summary>
			private EntityRelationshipShapeGeometry()
			{
			}
			#endregion // Constructor and singleton
			/// <summary>
			/// Paint the solid crowsfoot on the end of an optional line
			/// </summary>
			protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				RolePlayerLink connector;
				EdgePointCollection edgePoints;
				int edgePointCount;
				Pen pen;
				ShapeElement shapeHost;
				IOffsetBorderPoint offsetPointProvider;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker &&
					RoleMultiplicity.ZeroToMany == (connector = (RolePlayerLink)geometryHost).GetDisplayRoleMultiplicity(displaySetting) &&
					null != (edgePoints = connector.EdgePoints) &&
					1 < (edgePointCount = edgePoints.Count) &&
					null != (shapeHost = connector.ToShape) &&
					null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint) &&
					null != (pen = geometryHost.GeometryStyleSet.GetPen(DiagramPens.ConnectionLine)))
				{
					Color restoreColor = pen.Color;
					pen.Color = geometryHost.UpdateGeometryLuminosity(e.View, pen);
					PointD pointOnBorder = edgePoints[edgePointCount - 1].Point;
					double angle = GeometryUtility.CalculateRadiansRotationAngle(edgePoints[0].Point, pointOnBorder);
					PointD vertexPoint = pointOnBorder;
					vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
					e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(pointOnBorder));
					PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
					if (offsetBorderPoint.HasValue)
					{
						e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
					}
					offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
					if (offsetBorderPoint.HasValue)
					{
						e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
					}
					pen.Color = restoreColor;
				}
				base.DoPaintGeometry(e, geometryHost);
			}
			/// <summary>
			/// Return a path modified to include any ER multiplicity decorators
			/// </summary>
			public override GraphicsPath GetPath(IGeometryHost geometryHost)
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					return base.GetPath(geometryHost);
				}
				RolePlayerLink connector = (RolePlayerLink)geometryHost;
				EdgePointCollection edgePoints;
				int edgePointCount;
				RoleMultiplicity multiplicity;
				if (RoleMultiplicity.Unspecified != (multiplicity = connector.GetDisplayRoleMultiplicity(displaySetting)) &&
					1 < (edgePointCount = (edgePoints = connector.EdgePoints).Count))
				{
					switch (displaySetting)
					{
						case EntityRelationshipBinaryMultiplicityDisplay.CrowsFootOnly:
						CrowsFootOnly:
							switch (multiplicity)
							{
								case RoleMultiplicity.OneToMany:
								case RoleMultiplicity.ZeroToMany:
									{
										PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
										PointD roleBoxEndPoint = edgePoints[0].Point;
										double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
										GraphicsPath path = base.UninitializedPath;
										path.Reset();
										path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
										PointD vertexPoint = objectTypeEndPoint;
										vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
										ShapeElement shapeHost;
										IOffsetBorderPoint offsetPointProvider;
										if (null != (shapeHost = connector.ToShape) &&
											null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
										{
											PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
											if (offsetBorderPoint.HasValue)
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
											}
											offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
											if (offsetBorderPoint.HasValue)
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
											}
										}
										return path;
									}
							}
							break;
						case EntityRelationshipBinaryMultiplicityDisplay.Barker:
							// Stop short of the crows foot if optional, draw the broken line under the solid crows foot
							switch (multiplicity)
							{
								case RoleMultiplicity.OneToMany:
									// Single pen only, include the crowsfoot as part of the path
									goto CrowsFootOnly;
								case RoleMultiplicity.ZeroToMany:
									// Stop the path at the vertex point, use a different pen for the crowsfoot in DoPaintGeometry
									{
										PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
										PointD roleBoxEndPoint = edgePoints[0].Point;
										double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
										GraphicsPath path = base.UninitializedPath;
										path.Reset();
										objectTypeEndPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
										path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
										return path;
									}
							}
							break;
						case EntityRelationshipBinaryMultiplicityDisplay.InformationEngineering:
							{
								PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
								PointD roleBoxEndPoint = edgePoints[0].Point;
								double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
								double cosAngle = Math.Cos(angle);
								double sinAngle = Math.Sin(angle);
								GraphicsPath path = base.UninitializedPath;
								path.Reset();
								switch (multiplicity)
								{
									case RoleMultiplicity.ExactlyOne:
										{
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
											path.StartFigure();
											PointD oneMarkLeft = objectTypeEndPoint;
											oneMarkLeft.Offset(InfEngInnerOneMarkOffset * cosAngle, InfEngInnerOneMarkOffset * sinAngle);
											PointD oneMarkRight = oneMarkLeft;
											oneMarkLeft.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));
											oneMarkLeft.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											oneMarkRight.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));
											break;
										}
									case RoleMultiplicity.ZeroToOne:
										{
											PointD circleStart = objectTypeEndPoint;
											circleStart.Offset((InfEngInnerOneMarkOffset + InfEngOuterOneMarkOffset + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * cosAngle, (InfEngInnerOneMarkOffset + InfEngOuterOneMarkOffset + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * sinAngle);
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(circleStart));
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddArc(
												(float)(circleStart.X - InfEngMarkerHalfWidth),
												(float)(circleStart.Y - InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												0,
												360);
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));
											circleStart.Offset(-InfEngOuterOneMarkOffset * cosAngle, -InfEngOuterOneMarkOffset * sinAngle);
											PointD oneMarkRight = circleStart;
											circleStart.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(oneMarkRight));
											break;
										}
									case RoleMultiplicity.ZeroToMany:
										{
											PointD circleStart = objectTypeEndPoint;
											circleStart.Offset((CrowsFootHeight + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * cosAngle, (CrowsFootHeight + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * sinAngle);
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(circleStart));
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddArc(
												(float)(circleStart.X - InfEngMarkerHalfWidth),
												(float)(circleStart.Y - InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												0,
												360);
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));

											ShapeElement shapeHost;
											IOffsetBorderPoint offsetPointProvider;
											if (null != (shapeHost = connector.ToShape) &&
												null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));
												PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, circleStart, CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(offsetBorderPoint.Value));
												}
												offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, circleStart, -CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(offsetBorderPoint.Value));
												}
											}
											break;
										}
									case RoleMultiplicity.OneToMany:
										{
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
											PointD vertexPoint = objectTypeEndPoint;
											vertexPoint.Offset(CrowsFootHeight * cosAngle, CrowsFootHeight * sinAngle);

											// Add the one mark
											PointD oneMarkLeft = vertexPoint;
											oneMarkLeft.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											PointD oneMarkRight = oneMarkLeft;
											oneMarkLeft.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));

											ShapeElement shapeHost;
											IOffsetBorderPoint offsetPointProvider;
											if (null != (shapeHost = connector.ToShape) &&
												null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
											{
												PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
												}
												offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
												}
											}
											break;
										}
								}
								return path;
							}
					}
				}
				return base.GetPath(geometryHost);
			}
		}
		#endregion // EntityRelationshipShapeGeometry class
		/// <summary>
		/// Add crowsfoot area to excluded clipregion if it is not included in the path. This
		/// is synchronized with EntityRelationshipShapeGeometry.DoPaintGeometry
		/// </summary>
		public override void ExcludeFromClipRegion(Graphics g, Matrix matrix, GraphicsPath perimeter)
		{
			base.ExcludeFromClipRegion(g, matrix, perimeter);
			EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
			EdgePointCollection edgePoints;
			int edgePointCount;
			ShapeElement shapeHost;
			IOffsetBorderPoint offsetPointProvider;
			if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker &&
				RoleMultiplicity.ZeroToMany == GetDisplayRoleMultiplicity(displaySetting) &&
				null != (edgePoints = EdgePoints) &&
				1 < (edgePointCount = edgePoints.Count) &&
				null != (shapeHost = ToShape) &&
				null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
			{
				PointD pointOnBorder = edgePoints[edgePointCount - 1].Point;
				double angle = GeometryUtility.CalculateRadiansRotationAngle(edgePoints[0].Point, pointOnBorder);
				PointD vertexPoint = pointOnBorder;
				vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
				GraphicsPath path = ExcludePath;
				PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
				if (offsetBorderPoint.HasValue)
				{
					path.AddLine(PointD.ToPointF(pointOnBorder), PointD.ToPointF(offsetBorderPoint.Value));
					path.AddLine(PointD.ToPointF(offsetBorderPoint.Value), PointD.ToPointF(vertexPoint));
				}
				else
				{
					path.AddLine(PointD.ToPointF(pointOnBorder), PointD.ToPointF(vertexPoint));
				}
				offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
				if (offsetBorderPoint.HasValue)
				{
					path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
				}
				path.CloseFigure();
				g.SetClip(path, CombineMode.Exclude);
			}
		}
		/// <summary>
		/// Change the geometry if we're drawing any Entity Relationship options
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				RoleMultiplicity multiplicity = GetDisplayRoleMultiplicity(displaySetting);
				if (multiplicity != RoleMultiplicity.Unspecified)
				{
					if (displaySetting != EntityRelationshipBinaryMultiplicityDisplay.CrowsFootOnly ||
						multiplicity == RoleMultiplicity.OneToMany ||
						multiplicity == RoleMultiplicity.ZeroToMany)
					{
						return EntityRelationshipShapeGeometry.ShapeGeometry;
					}
				}
				return base.ShapeGeometry;
			}
		}
		/// <summary>
		/// Return the display role multiplicity for this link. If the options
		/// page settings do not display multiplicity then we always return Unspecified.
		/// Note that Information Engineering and UML fold multiplicity and cardinality the
		/// the same, but the Barker solid line (mandatory) is on the same side as the ORM
		/// mandatory dot. This will monkey with the multiplicity for Barker mode to reflect
		/// the mandatory on the near side.
		/// </summary>
		private RoleMultiplicity GetDisplayRoleMultiplicity(EntityRelationshipBinaryMultiplicityDisplay displaySetting)
		{
			ObjectTypePlaysRole link;
			Role role;
			RoleMultiplicity multiplicity;
			if (displaySetting != EntityRelationshipBinaryMultiplicityDisplay.Off &&
				null != (link = AssociatedRolePlayerLink) &&
				null != (role = link.PlayedRole) &&
				RoleMultiplicity.Indeterminate != (multiplicity = role.Multiplicity))
			{
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker)
				{
					bool mandatory = role.IsMandatory;
					switch (multiplicity)
					{
						case RoleMultiplicity.ZeroToMany:
						case RoleMultiplicity.OneToMany:
							multiplicity = mandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
							break;
						case RoleMultiplicity.ZeroToOne:
						case RoleMultiplicity.ExactlyOne:
							multiplicity = mandatory ? RoleMultiplicity.ExactlyOne : RoleMultiplicity.ZeroToOne;
							break;
					}
				}
				return multiplicity;
			}
			return RoleMultiplicity.Unspecified;
		}
		/// <summary>
		/// Return a dashed pen if we're optional and doing BarkerER compatibility
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker)
				{
					switch (GetDisplayRoleMultiplicity(displaySetting))
					{
						case RoleMultiplicity.ZeroToMany:
						case RoleMultiplicity.ZeroToOne:
							return BarkerEROptionalPen;
					}
				}
				return base.OutlinePenId;
			}
		}
		#endregion // EntityRelationship learning mode support
		#region RolePlayerLink specific
		/// <summary>
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public ObjectTypePlaysRole AssociatedRolePlayerLink
		{
			get
			{
				return ModelElement as ObjectTypePlaysRole;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		public override void ConfiguringAsChildOf(ORMDiagram diagram, bool createdDuringViewFixup)
		{
			Reconfigure(null);
		}
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected void Reconfigure(ShapeElement discludedShape)
		{
			ObjectTypePlaysRole modelLink = ModelElement as ObjectTypePlaysRole;
			ObjectType rolePlayer = modelLink.RolePlayer;
			FactType nestedFact = rolePlayer.NestedFactType;

			MultiShapeUtility.ReconfigureLink(this, modelLink.PlayedRole.FactType, (nestedFact == null) ? rolePlayer as ModelElement : nestedFact, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
		#if LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		protected override BinaryLinkAnchor Anchor
		{
			get
			{
				return BinaryLinkAnchor.FromShape;
			}
		}
		#endif //LINKS_ALWAYS_CONNECT
		#endregion // RolePlayerLink specific
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.RolePlayerLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.RolePlayerLinkAccessibleDescription;
			}
		}
		/// <summary>
		/// Describe the from role in terms of FactName.RoleName(RolePosition)
		/// </summary>
		protected override string FromAccessibleValue
		{
			get
			{
				ObjectTypePlaysRole link = ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				FactType fact = role.FactType;
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.RolePlayerLinkAccessibleFromValueFormat, fact.Name, role.Name, (fact.RoleCollection.IndexOf(role) + 1).ToString(CultureInfo.CurrentCulture));
			}
		}
		#endregion // Accessibility Properties
		#region Store Event Handlers
		/// <summary>
		///  Helper function to update the mandatory dot in response to events
		/// </summary>
		private static void UpdateDotDisplayOnMandatoryConstraintChange(Role role)
		{
			foreach (ObjectTypePlaysRole objectTypePlaysRole in DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(objectTypePlaysRole))
				{
					ShapeElement shape = pel as ShapeElement;
					if (shape != null)
					{
						shape.Invalidate(true);
					}
				}
			}
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="RolePlayerLink"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(MandatoryConstraint.ModalityDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(InternalConstraintChangedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRelationship(FactSetConstraint.DomainClassId), new EventHandler<ElementAddedEventArgs>(InternalConstraintRoleSequenceAddedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId), new EventHandler<ElementDeletedEventArgs>(InternalConstraintRoleSequenceRoleRemovedEvent), action);
		}
		/// <summary>
		/// Update the link displays when the modality of a simple mandatory constraint changes
		/// </summary>
		private static void InternalConstraintChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			MandatoryConstraint smc = e.ModelElement as MandatoryConstraint;
			if (smc != null && !smc.IsDeleted && smc.IsSimple)
			{
				LinkedElementCollection<Role> roles = smc.RoleCollection;
				if (roles.Count != 0)
				{
					UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
				}
			}
		}
		/// <summary>
		/// Update the link displays when a role sequence for a mandatory constraint is added
		/// </summary>
		private static void InternalConstraintRoleSequenceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactSetConstraint link = e.ModelElement as FactSetConstraint;
			MandatoryConstraint constraint = link.SetConstraint as MandatoryConstraint;
			if (constraint != null && !constraint.IsDeleted && constraint.IsSimple)
			{
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
				if (roles.Count > 0)
				{
					Debug.Assert(roles.Count == 1); // Mandatory constraints have a single role only
					UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
				}
			}
		}
		/// <summary>
		/// Update the link display when a mandatory constraint role is removed
		/// </summary>
		private static void InternalConstraintRoleSequenceRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			Role role;
			MandatoryConstraint constraint;
			if (null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				constraint.IsSimple &&
				(null != (role = link.Role)) &&
				!role.IsDeleted)
			{
				UpdateDotDisplayOnMandatoryConstraintChange(role);
			}
		}
		#endregion // Store Event Handlers
	}
}
