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

using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using Neumont.Tools.EntityRelationshipModels.Barker;
using b = Neumont.Tools.EntityRelationshipModels.Barker;
using System.Drawing;
using Neumont.Tools.Modeling.Diagrams;
using Neumont.Tools.ORM.ShapeModel;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
namespace Neumont.Tools.ORM.Views.BarkerERView
{
	partial class AssociationConnector : IReconfigureableLink
	{
		#region Customize appearance
		/// <summary>
		/// Overridden to disallow selection of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Overridden to disallow manual routing of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		/// <remarks>
		/// If this returns <see langword="true"/> while the <see cref="P:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector.CanSelect"/>
		/// property returns <see langword="false"/>, the application will crash while trying to manually route the connector.
		/// </remarks>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Turn on tooltips to show column relationships
		/// </summary>
		public override bool HasToolTip
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Indicate the source/target columns for the foreign key connector
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			BinaryAssociation link = ((BarkerErModelContainsBinaryAssociation)ModelElement).BinaryAssociation;
			return string.Format(
				"Association #{0}; Role 1: {1}, Role 2: {2}", 
				link.Number, link.RoleCollection[0].PredicateText, link.RoleCollection[1].PredicateText);
		}
		

		#endregion // Customize appearance
		#region reconfigure
		/// <summary>
		/// Override of <see cref="ORMBaseBinaryLinkShape.ConfiguringAsChildOf"/>
		/// </summary>
		public override void ConfiguringAsChildOf(Neumont.Tools.ORM.ShapeModel.ORMDiagram diagram, bool createdDuringViewFixup)
		{
			Reconfigure(null);
		}
		/// <summary>
		/// Associate the <see cref="EntityRelationshipShapeGeometry"/> shape geometry with this connector shape.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return EntityRelationshipShapeGeometry.ShapeGeometry;
			}
		}
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected void Reconfigure(ShapeElement discludedShape)
		{
			//TODO is this needed?

			//BarkerErModelContainsBinaryAssociation modelLink = ModelElement as BarkerErModelContainsBinaryAssociation;
			//ObjectType rolePlayer = modelLink.RolePlayer;
			//FactType nestedFact = rolePlayer.NestedFactType;

			//MultiShapeUtility.ReconfigureLink(this,
			//    modelLink.PlayedRole.FactType,
				//(nestedFact == null) ? rolePlayer as ModelElement : nestedFact, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
		#endregion
		#region EntityRelationshipShapeGeometry class
		private const double CrowsFootHeight = .12;
		private const double CrowsFootHalfWidth = .05;
		private const double InfEngOuterOneMarkOffset = .03;
		private const double InfEngInnerOneMarkOffset = .06;
		private const double InfEngMarkerHalfWidth = .04;
		private const bool CrowsFootParallelMode = true;
		private static float[] DashPattern = new float[] { 7.0F, 3.0F };
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
				AssociationConnector connector = (AssociationConnector)geometryHost;
				BarkerErModelContainsBinaryAssociation link = connector.ModelElement as BarkerErModelContainsBinaryAssociation;

				EdgePointCollection edgePoints;
				int edgePointCount;
				BinaryAssociation association;
				LinkedElementCollection<b.Role> roles;
				if (null != (edgePoints = connector.EdgePoints) &&
					1 < (edgePointCount = edgePoints.Count) &&
					null != (association = link.BinaryAssociation) &&
					2 == (roles = association.RoleCollection).Count)
				{
					PointD p1 = edgePoints[edgePointCount - 1].Point;
					PointD p2 = edgePoints[0].Point;
					PointD midP = new PointD((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
					
					#region find correct roles
					EntityType fromEntity = connector.FromShape.ModelElement as EntityType;
					EntityType toEntity = connector.ToShape.ModelElement as EntityType;
					b.Role fromRole = null, toRole = null;
					if (fromEntity == roles[0].EntityType)
					{
						fromRole = roles[0];
					}
					else if (fromEntity == roles[1].EntityType)
					{
						fromRole = roles[1];
					}
					if (toEntity == roles[0].EntityType)
					{
						toRole = roles[0];
					}
					else if (toEntity == roles[1].EntityType)
					{
						toRole = roles[1];
					}
					#endregion
					
					if (fromRole != null && toRole != null)
					{
						DrawAssociationEnd(e, geometryHost, connector.FromShape, p2, midP, fromRole.IsMultiValued, !fromRole.IsMandatory, fromRole.PredicateText);
						DrawAssociationEnd(e, geometryHost, connector.ToShape, p1, midP, toRole.IsMultiValued, !toRole.IsMandatory, toRole.PredicateText);
					}
				}
			}

			private static void DrawAssociationEnd(
				DiagramPaintEventArgs e, IGeometryHost geometryHost,
				NodeShape node, PointD pointOnBorder, PointD endPoint,
				bool many, bool optional, string predicateText)
			{
				Pen pen;
				ShapeElement shapeHost;
				IOffsetBorderPoint offsetPointProvider;
				Font font;
				Brush brush;
				Graphics g = e.Graphics;
				if (null != (shapeHost = node) &&
					null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint) &&
					null != (pen = geometryHost.GeometryStyleSet.GetPen(DiagramPens.ConnectionLine)) &&
					null != (font = geometryHost.GeometryStyleSet.GetFont(DiagramFonts.ConnectionLine)) &&
					null != (brush = geometryHost.GeometryStyleSet.GetBrush(DiagramBrushes.ConnectionLineText)))
				{
					Color restoreColor = pen.Color;
					pen.Color = geometryHost.UpdateGeometryLuminosity(e.View, pen);
					double angle = GeometryUtility.CalculateRadiansRotationAngle(endPoint, pointOnBorder);
					PointD vertexPoint = pointOnBorder;
					vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));

					#region draw the main line

					Pen mainLinePen = (Pen)pen.Clone();
					if (optional)
					{
						mainLinePen.DashPattern = DashPattern;
					}
					if (many & optional)
					{
						g.DrawLine(mainLinePen, PointD.ToPointF(endPoint), PointD.ToPointF(vertexPoint));
						g.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(pointOnBorder));
					}
					else
					{
						g.DrawLine(mainLinePen, PointD.ToPointF(endPoint), PointD.ToPointF(pointOnBorder));
					}
					#endregion
					#region draw crow's foot if necessary
					if (many)
					{
						PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
						if (offsetBorderPoint.HasValue)
						{
							g.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
						}
						offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
						if (offsetBorderPoint.HasValue)
						{
							g.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
						}
					}
					#endregion
					#region draw text

					//determine the line's properties
					double edgeX = pointOnBorder.X;
					double edgeY = pointOnBorder.Y;
					EntitySideType whichSideShapeIsOn = EntitySide.FindWhichSide(pointOnBorder, shapeHost.GeometryBoundingBox);
					double x = 0, y = 0;
					angle = Math.Atan2(Math.Abs(endPoint.Y - edgeY), Math.Abs(endPoint.X - edgeX));
					double inDegrees = angle * 180 / Math.PI;
					if (inDegrees < 0)
					{
						inDegrees += 360;
					}
					
					SizeF textSize = g.MeasureString(predicateText, font);
					x = textSize.Width;
					y = Math.Abs(x * Math.Tan(angle));

					//determine what to offset
					double textX = edgeX;
					double textY = edgeY;

					if (whichSideShapeIsOn == EntitySideType.OnRight)
					{
						textX -= x;
						if (inDegrees < 180)
						{
							textY -= y;
						}
					}
					else if (whichSideShapeIsOn == EntitySideType.OnLeft && (inDegrees > 180 || inDegrees < 90))
					{
						textY -= y;
					}
					if (whichSideShapeIsOn != EntitySideType.OnTop)
					{
						textY -= textSize.Height;
					}
					//now add padding
					double paddingX = CrowsFootHeight, paddingY = CrowsFootHeight;
					switch (whichSideShapeIsOn)
					{
						case EntitySideType.OnBottom:
						case EntitySideType.OnLeft:
							textX += paddingX;
							textY -= paddingY;
							break;
						case EntitySideType.OnRight:
							textX -= paddingX;
							textY -= paddingY;
							break;
						case EntitySideType.OnTop:
							textX += paddingX;
							textY += paddingY;
							break;
					}

					//perform the drawing
					g.DrawString(predicateText, font, brush, new PointF((float)textX,(float)textY));

					#endregion

					pen.Color = restoreColor;
				}
			}
			#region draw text helpers
			private enum EntitySideType { OnLeft, OnRight, OnTop, OnBottom }
			private class EntitySide : IComparable<EntitySide>
			{
				public static EntitySideType FindWhichSide(PointD p, RectangleD bounds)
				{
					List<EntitySide> all = new List<EntitySide>(4);
					all.Add(new EntitySide(p, bounds, EntitySideType.OnBottom));
					all.Add(new EntitySide(p, bounds, EntitySideType.OnLeft));
					all.Add(new EntitySide(p, bounds, EntitySideType.OnRight));
					all.Add(new EntitySide(p, bounds, EntitySideType.OnTop));
					all.Sort();
					return all[0]._type;
				}

				private EntitySideType _type;
				private double _range;
				public EntitySide(PointD p, RectangleD bounds, EntitySideType type)
				{
					_type = type;
					switch (_type)
					{
						case EntitySideType.OnBottom:
							_range = Math.Abs(bounds.Top - p.Y);
							break;
						case EntitySideType.OnLeft:
							_range = Math.Abs(bounds.Right - p.X);
							break;
						case EntitySideType.OnRight:
							_range = Math.Abs(bounds.Left - p.X);
							break;
						case EntitySideType.OnTop:
							_range = Math.Abs(bounds.Bottom - p.Y);
							break;
					}
				}

				#region IComparable<EntitySide> Members

				public int CompareTo(EntitySide other)
				{
					return this._range.CompareTo(other._range);
				}

				#endregion
			}
			#endregion

			/// <summary>
			/// Return a path modified to include any ER multiplicity decorators
			/// </summary>
			public override GraphicsPath GetPath(IGeometryHost geometryHost)
			{
				//TODO get this to work properly

				//GraphicsPath path = base.UninitializedPath;
				//path.Reset();

				//AssociationConnector connector = (AssociationConnector)geometryHost;

				//EdgePointCollection edgePoints;
				//int edgePointCount;
				//if (null != (edgePoints = connector.EdgePoints) &&
				//    1 < (edgePointCount = edgePoints.Count))
				//{
				//    PointD p1 = edgePoints[edgePointCount - 1].Point;
				//    PointD p2 = edgePoints[0].Point;

				//    path.AddLine(PointD.ToPointF(p1), PointD.ToPointF(p2));

				//    //DrawAssociationEnd(e, geometryHost, connector.FromShape, p2, midP, true, true);
				//    //DrawAssociationEnd(e, geometryHost, connector.ToShape, p1, midP, true, false);
				//}

				//return path;

				return base.GetPath(geometryHost);
			}
		}
		#endregion // EntityRelationshipShapeGeometry class
	}
}
