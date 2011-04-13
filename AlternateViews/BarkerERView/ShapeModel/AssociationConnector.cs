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

using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Text;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;
using Barker = ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;
using System.Drawing;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
namespace ORMSolutions.ORMArchitect.Views.BarkerERView
{
	partial class AssociationConnector : IReconfigureableLink, IConfigureAsChildShape
	{
		#region Customize appearance
		/// <summary>
		/// Overridden to disallow selection of this <see cref="T:ORMSolutions.ORMArchitect.Views.BarkerERView.AssociationConnector"/>.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		
		/// <summary>
		/// Overridden to disallow manual routing of this <see cref="T:ORMSolutions.ORMArchitect.Views.BarkerERView.AssociationConnector"/>.
		/// </summary>
		/// <remarks>
		/// If this returns <see langword="true"/> while the <see cref="P:ORMSolutions.ORMArchitect.Views.BarkerERView.AssociationConnector.CanSelect"/>
		/// property returns <see langword="false"/>, the application will crash while trying to manually route the connector.
		/// </remarks>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Stop the user from manually moving link end points
		/// </summary>
		public override bool CanMoveAnchorPoints
		{
			get
			{
				return false;
			}
		}
#endif // VISUALSTUDIO_10_0
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
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected new void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			base.ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
			Reconfigure(null);
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
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
		private const double TextPaddingX = .08;
		private const double TextPaddingY = .05;
		private const double InfEngOuterOneMarkOffset = .03;
		private const double InfEngInnerOneMarkOffset = .06;
		private const double InfEngMarkerHalfWidth = .04;
		private const bool CrowsFootParallelMode = true;
		private static float[] DashPattern = new float[] { 7.0F, 3.0F };
		private sealed class EntityRelationshipShapeGeometry : LinkShapeGeometry
		{
			#region Constructor and singleton
			/// <summary>
			/// Singleton EntityRelationshipShapeGeometry instance
			/// </summary>
			public static readonly ShapeGeometry ShapeGeometry = new EntityRelationshipShapeGeometry();
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
				LinkedElementCollection<Barker.Role> roles;
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
					Barker.Role fromRole = null, toRole = null;
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
						bool fromOptional = !fromRole.IsMandatory;
						bool toOptional = !toRole.IsMandatory;
						bool bothOptional = fromOptional && toOptional;

						PointF? v1 = null, v2 = null;
						DrawAssociationEnd(
							e, geometryHost, connector.FromShape, p2, midP, fromRole.IsMultiValued, fromOptional, fromRole.IsPrimaryIdComponent,
							fromRole.PredicateText, ref v1, bothOptional);
						DrawAssociationEnd(
							e, geometryHost, connector.ToShape, p1, midP, toRole.IsMultiValued, toOptional, toRole.IsPrimaryIdComponent,
							toRole.PredicateText, ref v2, bothOptional);

						if (bothOptional && v1.HasValue && v2.HasValue)
						{
							Pen pen = geometryHost.GeometryStyleSet.GetPen(DiagramPens.ConnectionLine);
							pen = (Pen)pen.Clone();
							pen.DashPattern = DashPattern;
							e.Graphics.DrawLine(pen, v1.Value, v2.Value);
						}
					}
				}
			}
			private static void DrawAssociationEnd(
				DiagramPaintEventArgs e, IGeometryHost geometryHost,
				NodeShape node, PointD pointOnBorder, PointD endPoint,
				bool many, bool optional, bool id, string predicateText, ref PointF? lineEnd, bool bothOptional)
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
						mainLinePen.DashOffset = DashPattern[0];
					}
					if (many & optional)
					{
						if (!bothOptional)
						{
							g.DrawLine(mainLinePen, PointD.ToPointF(endPoint), PointD.ToPointF(vertexPoint));
						}
						else
						{
							lineEnd = PointD.ToPointF(vertexPoint);
						}
						g.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(pointOnBorder));
					}
					else
					{
						if (!bothOptional)
						{
							g.DrawLine(mainLinePen, PointD.ToPointF(endPoint), PointD.ToPointF(pointOnBorder));
						}
						else
						{
							lineEnd = PointD.ToPointF(pointOnBorder);
						}
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
					#region draw tick mark if necessary
					if (id)
					{
						PointD oneMarkLeft = vertexPoint;
						double cosAngle = Math.Cos(angle);
						double sinAngle = Math.Sin(angle);

						oneMarkLeft.Offset(InfEngInnerOneMarkOffset * cosAngle, InfEngInnerOneMarkOffset * sinAngle);
						PointD oneMarkRight = oneMarkLeft;
						oneMarkLeft.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
						oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);

						g.DrawLine(pen, PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));
					}
					#endregion
					#region draw text

					//determine the line's properties
					double edgeX = pointOnBorder.X;
					double edgeY = pointOnBorder.Y;
					EntitySideType whichSideShapeIsOn = EntitySide.FindWhichSide(pointOnBorder, shapeHost.GeometryBoundingBox);
					double w = 0;//, y = 0;
					angle = Math.Atan2(Math.Abs(endPoint.Y - edgeY), Math.Abs(endPoint.X - edgeX));
					double inDegrees = angle * 180 / Math.PI;
					if (inDegrees < 0)
					{
						inDegrees += 360;
					}
					
					SizeF textSize = g.MeasureString(predicateText, font);
					w = textSize.Width;
					//y = Math.Abs(w * Math.Tan(angle));

					//determine what to offset
					double textX = edgeX;
					double textY = edgeY;
					double h = textSize.Height;
					bool lessThan45 = inDegrees < 45;
					
					switch (whichSideShapeIsOn)
					{
						case EntitySideType.OnBottom:
							textY -= TextPaddingY + h;
							if (lessThan45) textX += TextPaddingX;
							else textX -= TextPaddingX + w;
							break;

						case EntitySideType.OnTop:
							textY += TextPaddingY;
							if (lessThan45)textX -= TextPaddingX + w;
							else textX += TextPaddingX;
							break;

						case EntitySideType.OnLeft:
							textX += TextPaddingX;
							if (lessThan45)textY -= TextPaddingY + h;
							else textY += TextPaddingY;
							break;

						case EntitySideType.OnRight:
							textX -= TextPaddingX + w;
							if (lessThan45) textY += TextPaddingY;
							else textY -= TextPaddingY + h;
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
