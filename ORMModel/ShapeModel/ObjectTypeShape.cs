using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Northface.Tools.ORM;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ObjectTypeShape
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField = null;
		private const double HorizontalMargin = 0.2;
		private const double VerticalMargin = 0.075;
		private static readonly StyleSetResourceId DashedShapeOutlinePen = new StyleSetResourceId("Northface", "DashedShapeOutlinePen");
		#endregion // Member Variables
		#region Customize appearance
		/// <summary>
		/// Switch between the standard solid pen and
		/// a dashed pen depending on the objectification settings
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				ObjectType associatedObjectType = ModelElement as ObjectType;
				return (associatedObjectType != null && associatedObjectType.IsValueType) ? DashedShapeOutlinePen : DiagramPens.ShapeOutline;
			}
		}
		/// <summary>
		/// Add a dashed pen to the class resource set
		/// </summary>
		/// <param name="classStyleSet">Shared class styleset instance</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings settings = new PenSettings();
			settings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(DashedShapeOutlinePen, DiagramPens.ShapeOutline, settings);
		}
		/// <summary>
		/// Set the default size for this object. This value is basically
		/// ignored because the size is ultimately based on the contained
		/// text, but it needs to be set.
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return new SizeD(.7, .35);
			}
		}
		/// <summary>
		/// Get the shape of an object type. Controllable via the ORM Designer
		/// tab on the options page.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				ShapeGeometry useShape;
				switch (Shell.OptionsPage.CurrentObjectTypeShape)
				{
					case Shell.ObjectTypeShape.Ellipse:
						useShape = EllipseShapeGeometryEx.ShapeGeometry; // ShapeGeometries.Ellipse
						break;
					case Shell.ObjectTypeShape.HardRectangle:
						useShape = ShapeGeometries.Rectangle;
						break;
					case Shell.ObjectTypeShape.SoftRectangle:
					default:
						useShape = ShapeGeometries.RoundedRectangle;
						break;
				}
				return useShape;
			}
		}

		/// <summary>
		/// Size to ContentSize plus some margin padding.
		/// </summary>
		public override void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty)
			{
				contentSize.Width += HorizontalMargin + HorizontalMargin;
				contentSize.Height += VerticalMargin + VerticalMargin;
				Size = contentSize;
			}
		}
		/// <summary>
		/// Set the content size to the text size
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				SizeD retVal = SizeD.Empty;
				TextField textShape = TextShapeField;
				if (textShape != null)
				{
					retVal = textShape.GetBounds(this).Size;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Retrieve the (singleton) shape field for the text
		/// </summary>
		protected TextField TextShapeField
		{
			get
			{
				return myTextShapeField;
			}
		}
		/// <summary>
		/// Creates and adds shape fields to this shape type. Called once per type.
		/// </summary>
		/// <param name="shapeFields">The shape fields collection for this shape type.</param>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize field
			AutoSizeTextField field = new AutoSizeTextField();
			field.DrawBorder = false;
			field.FillBackground = false;
			field.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			field.DefaultPenId = DiagramPens.ShapeOutline;
			field.DefaultFontId = DiagramFonts.ShapeTitle;
			field.DefaultFocusable = true;
			field.DefaultText = "Object";

			StringFormat fieldFormat = new StringFormat(StringFormatFlags.NoClip);
			fieldFormat.Alignment = StringAlignment.Center;
			field.DefaultStringFormat = fieldFormat;
			field.AssociateValueWith(Store, ObjectTypeShape.ShapeNameMetaAttributeGuid, NamedElement.NameMetaAttributeGuid);
			
			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(field);

			// Modify anchoring behavior
			AnchoringBehavior anchor = field.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.CenterVertically();

			Debug.Assert(myTextShapeField == null); // Only called once
			myTextShapeField = field;
		}
		#endregion // Customize appearance
		#region Customize connection points
		/// <summary>
		/// Enable custom connection points
		/// </summary>
		/// <value>true</value>
		public override bool HasConnectionPoints
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Center the default connection point. The actual connection
		/// point is adjusted by other routines. This is called if
		/// there is no override for CreateConnectionPoint.
		/// </summary>
		protected override PointD ConnectionPoint
		{
			get
			{
				RectangleD bounds = AbsoluteBounds;
				return new PointD(bounds.X + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			}
		}
		#endregion // Customize connection points
		#region Shape display update rules
		[RuleOn(typeof(ObjectType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == NamedElement.NameMetaAttributeGuid)
				{
					foreach (object obj in e.ModelElement.AssociatedPresentationElements)
					{
						ORMBaseShape shape = obj as ORMBaseShape;
						if (shape != null)
						{
							shape.AutoResize();
						}
					}
				}
				else if (attributeGuid == ObjectType.IsValueTypeMetaAttributeGuid)
				{
					foreach (object obj in e.ModelElement.AssociatedPresentationElements)
					{
						ShapeElement shape = obj as ShapeElement;
						if (shape != null)
						{
							shape.Invalidate();
						}
					}
				}
			}
		}
		#endregion // Shape display update rules
	}
	/// <summary>
	/// UNDONE: An attempt to get connection lines correctly attaching
	/// to the border of an ellipse shape
	/// </summary>
	public class EllipseShapeGeometryEx : EllipseShapeGeometry
	{
		/// <summary>
		/// Singleton EllipseShapeGeometryEx instance
		/// </summary>
		public static readonly ShapeGeometry ShapeGeometry = new EllipseShapeGeometryEx();
		/// <summary>
		/// Protected default constructor. The class should be used
		/// as a singleton isntead of being publicly constructed.
		/// </summary>
		protected EllipseShapeGeometryEx()
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
			// The vectorEndPoint value is coming in (negative, negative) for the lower
			// right quadrant instead of (positive, positive). All other values are
			// (positive, positive), so we switch the end point to make the rest of the work
			// easier.
			// UNDONE: Should this be considered a bug?
			vectorEndPoint = new PointD(-vectorEndPoint.X, -vectorEndPoint.Y);

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
}
