using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ExternalConstraintShape
	{
		#region Customize appearance
		private static readonly StyleSetResourceId MandatoryDotBrush = new StyleSetResourceId("Northface", "ExternalConstraintMandatoryDotBrush");
		/// <summary>
		/// Return a consistent size for all constraints
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				return new SizeD(.16, .16);
			}
		}
		/// <summary>
		/// Constraints are drawn as circles
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return CustomFoldCircleShapeGeometry.ShapeGeometry;
			}
		}
		/// <summary>
		/// Initialize a pen and a brush for drawing the constraint
		/// outlines and contents.
		/// </summary>
		/// <param name="classStyleSet">StyleSet</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			PenSettings penSettings = new PenSettings();
			penSettings.Color = ORMDesignerPackage.FontAndColorService.GetForeColor(ORMDesignerColor.Constraint);
			penSettings.Width = 1.35F / 72.0F; // 1.35 Point.
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = penSettings.Color;
			classStyleSet.AddBrush(MandatoryDotBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		/// <summary>
		/// Draw the various constraint types
		/// </summary>
		/// <param name="e">DiagramPaintEventArgs</param>
		public override void OnPaintShape(DiagramPaintEventArgs e)
		{
			base.OnPaintShape(e);
			Pen pen = StyleSet.GetPen(DiagramPens.ShapeOutline);
			// Keep the pen color in sync with the color being used for highlighting
			Color startColor = UpdateGeometryLuminosity(e.View, pen);
			bool restoreColor = startColor != pen.Color;
			IConstraint constraint = AssociatedConstraint;
			RectangleD bounds = AbsoluteBounds;
			Graphics g = e.Graphics;
			switch (constraint.ConstraintType)
			{
				case ConstraintType.Equality:
				{
					double xOffset = bounds.Width * .3;
					float xLeft = (float)(bounds.Left + xOffset);
					float xRight = (float)(bounds.Right - xOffset);
					double yCenter = bounds.Top + bounds.Height / 2;
					double yOffset = (double)pen.Width * 1.0;
					float y = (float)(yCenter - yOffset);
					g.DrawLine(pen, xLeft, y, xRight, y);
					y = (float)(yCenter + yOffset);
					g.DrawLine(pen, xLeft, y, xRight, y);
					break;
				}
				case ConstraintType.DisjunctiveMandatory:
				{
					// Draw the dot
					bounds.Inflate(-Bounds.Width * .22, -Bounds.Height * .22);
					Brush brush = StyleSet.GetBrush(MandatoryDotBrush);
					SolidBrush coloredBrush = null;
					if (restoreColor)
					{
						coloredBrush = brush as SolidBrush;
						if (coloredBrush != null)
						{
							Debug.Assert(coloredBrush.Color == startColor); // Pen and brush should have the same base color
							coloredBrush.Color = pen.Color;
						}
					}
					g.FillEllipse(brush, RectangleD.ToRectangleF(bounds));
					if (coloredBrush != null)
					{
						coloredBrush.Color = startColor;
					}
					break;
				}
				case ConstraintType.Exclusion:
				{
					const double cos45 = 0.70710678118654752440084436210485;
					// Draw the X
					double offset = (bounds.Width + pen.Width) * (1 - cos45) / 2;
					float leftX = (float)(bounds.Left + offset);
					float rightX = (float)(bounds.Right - offset);
					float topY = (float)(bounds.Top + offset);
					float bottomY = (float)(bounds.Bottom - offset);
					g.DrawLine(pen, leftX, topY, rightX, bottomY);
					g.DrawLine(pen, leftX, bottomY, rightX, topY);
					break;
				}
				case ConstraintType.ExternalUniqueness:
				{
					// Draw a single line for a uniqueness constraint and a double
					// line for preferred uniqueness
					ExternalUniquenessConstraint euc = constraint as ExternalUniquenessConstraint;
					double widthAdjust = (double)pen.Width / 2;
					float xLeft = (float)(bounds.Left + widthAdjust);
					float xRight = (float)(bounds.Right - widthAdjust);
					if (euc.IsPreferred)
					{
						double yCenter = bounds.Top + bounds.Height / 2;
						double yOffset = (double)pen.Width * .7;
						float y = (float)(yCenter - yOffset);
						g.DrawLine(pen, xLeft, y, xRight, y);
						y = (float)(yCenter + yOffset);
						g.DrawLine(pen, xLeft, y, xRight, y);
					}
					else
					{
						float y = (float)(bounds.Top + bounds.Height / 2);
						g.DrawLine(pen, xLeft, y, xRight, y);
					}
					break;
				}
				case ConstraintType.Subset:
				{
					RectangleD arcBounds = bounds;
					double shrinkBy = -bounds.Height * .35;
					double yOffset = pen.Width * .7;
					double xOffset = shrinkBy * .35;
					arcBounds.Inflate(shrinkBy, shrinkBy);
					arcBounds.Offset(xOffset, -yOffset);
					g.DrawArc(pen, RectangleD.ToRectangleF(arcBounds), 90, 180);
					float xLeft = (float)arcBounds.Center.X;
					float xRight = (float)(bounds.Right + shrinkBy - xOffset);
					float y = (float)arcBounds.Top;
					g.DrawLine(pen, xLeft, y, xRight, y);
					y = (float)arcBounds.Bottom;
					g.DrawLine(pen, xLeft, y, xRight, y);
					y = (float)(arcBounds.Bottom + yOffset + yOffset);
					g.DrawLine(pen, (float)arcBounds.Left, y, xRight, y);
					break;
				}
			}

			// Restore pen color
			if (restoreColor)
			{
				pen.Color = startColor;
			}
		}
		/// <summary>
		/// Helper function for rules
		/// </summary>
		/// <param name="element">The model element to redraw</param>
		private static void InvalidateElementPresentation(ModelElement element)
		{
			foreach (object obj in element.AssociatedPresentationElements)
			{
				ShapeElement shape = obj as ShapeElement;
				if (shape != null)
				{
					shape.Invalidate();
				}
			}
		}
		#endregion // Customize appearance
		#region ExternalConstraintShape specific
		/// <summary>
		/// Get the typed model element associated with this shape
		/// </summary>
		public IConstraint AssociatedConstraint
		{
			get
			{
				return ModelElement as IConstraint;
			}
		}
		#endregion // ExternalConstraintShape specific
		#region Shape display update rules
		[RuleOn(typeof(ExternalUniquenessConstraint), FireTime = TimeToFire.TopLevelCommit)]
		private class ShapeChangeRule1 : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ExternalUniquenessConstraint.IsPreferredMetaAttributeGuid)
				{
					InvalidateElementPresentation(e.ModelElement);
				}
			}
		}
		#endregion // Shape display update rules
	}
}
