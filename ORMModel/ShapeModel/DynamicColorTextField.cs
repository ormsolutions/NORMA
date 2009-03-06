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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing.Drawing2D;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// An extension of the <see cref="TextField"/> that checks
	/// the parent <see cref="ShapeElement"/> for the support of
	/// the <see cref="IDynamicColorGeometryHost"/>.
	/// </summary>
	public class DynamicColorTextField : TextField
	{
		private const float InflateFocus = 3f / 72f; // 3 points inflation
		private const float TextFocusTopPadding = 1f / 72f; // 1 extra point above text
		private const float TextFocusBottomPadding = 0f; // No extra padding below
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field, forwarded to base class.</param>
		public DynamicColorTextField(string fieldName)
			: base(fieldName)
		{
		}
		/// <summary>
		/// Copy of <see cref="TextField.DoPaint"/> modified to support the
		/// <see cref="IDynamicColorGeometryHost"/> on the parent shape.
		/// </summary>
		public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
		{
			DiagramClientView clientView = e.View;
			if (!HasPendingEdit(parentShape, clientView))
			{
				if (GetVisible(parentShape))
				{
					string text = GetDisplayText(parentShape);
					StyleSet styleSet = parentShape.StyleSet;
					Graphics g = e.Graphics;
					RectangleF clip = g.ClipBounds;
					clip.Inflate(InflateFocus, InflateFocus);
					clip.Height -= InflateFocus;
					g.SetClip(clip);
					RectangleD shapeBounds = GetBounds(parentShape);
					RectangleF shapeRect = RectangleD.ToRectangleF(shapeBounds);
					Matrix startTransform = null;
					if (!DefaultIsHorizontal)
					{
						PointF point = PointD.ToPointF(shapeBounds.Center);
						startTransform = g.Transform;
						Matrix verticalTransform = g.Transform;
						verticalTransform.RotateAt(-90f, point);
						verticalTransform.Translate(0f, (-point.X / 2f) - shapeRect.X);
						g.Transform = verticalTransform;
					}
					if (parentShape.ClipWhenDrawingFields)
					{
						RectangleD parentBounds = parentShape.BoundingBox;
						shapeRect.Intersect(new RectangleF(0f, 0f, (float)parentBounds.Width, (float)parentBounds.Height));
					}
					if (FillBackground)
					{
						Color startColor = Color.White;
						Brush brush = GetBackgroundBrush(clientView, parentShape, ref startColor);
						g.FillRectangle(brush, shapeRect);
						SolidBrush solidBrush = brush as SolidBrush;
						if (solidBrush != null)
						{
							solidBrush.Color = startColor;
						}
					}
					if (DrawBorder)
					{
						Color oldColor = Color.White;
						Pen pen = GetPen(clientView, parentShape, ref oldColor);
						GeometryUtility.SafeDrawRectangle(g, pen, shapeRect.X, shapeRect.Y, shapeRect.Width, shapeRect.Height);
						pen.Color = oldColor;
					}
					if (text.Length > 0x0)
					{
						using (Font font = GetFont(parentShape))
						{
							// Note that this ignores the base GetTextBrush, which is trivial
							// and has no overrides. Note that we follow the convention used with
							// the base and do not update luminosity on the text.
							StyleSetResourceId textBrushId = GetTextBrushId(clientView, parentShape);
							Brush textBrush = styleSet.GetBrush(textBrushId);
							Color restoreTextColor = Color.Empty;
							IDynamicColorGeometryHost dynamicColors = parentShape as IDynamicColorGeometryHost;
							if (dynamicColors != null)
							{
								restoreTextColor = dynamicColors.UpdateDynamicColor(textBrushId, textBrush);
							}
							g.DrawString(text, font, textBrush, shapeRect, GetStringFormat(parentShape));
							SolidBrush solidTextBrush;
							if (!restoreTextColor.IsEmpty &&
								null != (solidTextBrush = textBrush as SolidBrush))
							{
								solidTextBrush.Color = restoreTextColor;
							}
						}
					}
					if (HasFocusedAppearance(parentShape, clientView))
					{
						// Note that the base makes a copy of shapeRect and
						// assymetrically modifies the focus rectangle. I don't
						// think this adds anything and gives focus floating shapes a
						// focus rectangle that arbitrarily overlaps nearby shapes.
						//RectangleF focusRect = shapeRect;
						//focusRect.Inflate(0f, InflateFocus);
						//focusRect.Height -= InflateFocus;
						//GeometryUtility.SafeDrawRectangle(g, styleSet.GetPen(DiagramPens.FocusIndicatorBackground), focusRect.X, focusRect.Y, focusRect.Width, focusRect.Height);
						//GeometryUtility.SafeDrawRectangle(g, styleSet.GetPen(DiagramPens.FocusIndicator), focusRect.X, focusRect.Y, focusRect.Width, focusRect.Height);
						// UNDONE: The bottom line is drawing clipped. The original code has the same problem
						// with both the top and bottom lines. This appears to be an issue with the Center
						// alignment on the default focus indicator pens.
						shapeRect.Inflate(0f, TextFocusTopPadding);
						shapeRect.Height -= TextFocusTopPadding - TextFocusBottomPadding;
						GeometryUtility.SafeDrawRectangle(g, styleSet.GetPen(DiagramPens.FocusIndicatorBackground), shapeRect.X, shapeRect.Y, shapeRect.Width, shapeRect.Height);
						GeometryUtility.SafeDrawRectangle(g, styleSet.GetPen(DiagramPens.FocusIndicator), shapeRect.X, shapeRect.Y, shapeRect.Width, shapeRect.Height);
					}
					if (startTransform != null)
					{
						g.Transform = startTransform;
					}
				}
			}
		}
		/// <summary>
		/// Replacement for <see cref="ShapeField.GetBackgroundBrush"/> that recognizes
		/// <see cref="IDynamicColorGeometryHost"/>
		/// </summary>
		public override Brush GetBackgroundBrush(DiagramClientView view, ShapeElement parentShape, ref Color oldColor)
		{
			StyleSet styleSet = (parentShape != null) ? parentShape.StyleSet : null;
			Brush brush = null;
			SolidBrush solidBrush = null;;
			Color restoreColor = Color.Empty;
			if (styleSet != null)
			{
				StyleSetResourceId brushId = GetBackgroundBrushId(view, parentShape);
				brush = styleSet.GetBrush(brushId);
				IDynamicColorGeometryHost dynamicColors = parentShape as IDynamicColorGeometryHost;
				if (dynamicColors == null ||
					(restoreColor = dynamicColors.UpdateDynamicColor(brushId, brush)).IsEmpty)
				{
					if (view != null)
					{
						restoreColor = parentShape.UpdateGeometryLuminosity(view, brush);
					}
				}
				else if (view != null)
				{
					parentShape.UpdateGeometryLuminosity(view, brush);
				}
			}
			if (restoreColor.IsEmpty)
			{
				if ((solidBrush ?? (solidBrush = brush as SolidBrush)) != null)
				{
					restoreColor = solidBrush.Color;
				}
			}
			if (!restoreColor.IsEmpty)
			{
				oldColor = restoreColor;
			}
			return brush;
		}
		/// <summary>
		/// Replacement for <see cref="ShapeField.GetBackgroundBrush"/> that recognizes
		/// <see cref="IDynamicColorGeometryHost"/>
		/// </summary>
		public override Pen GetPen(DiagramClientView view, ShapeElement parentShape, ref Color oldColor)
		{
			StyleSet styleSet = (parentShape != null) ? parentShape.StyleSet : null;
			Color restoreColor = Color.Empty;
			Pen pen = null;
			if (styleSet != null)
			{
				StyleSetResourceId penId = GetPenId(parentShape);
				pen = styleSet.GetPen(penId);
				IDynamicColorGeometryHost dynamicColors = parentShape as IDynamicColorGeometryHost;
				if (dynamicColors == null ||
					(restoreColor = dynamicColors.UpdateDynamicColor(penId, pen)).IsEmpty)
				{
					if (view != null)
					{
						restoreColor = parentShape.UpdateGeometryLuminosity(view, pen);
					}
				}
				else if (view != null)
				{
					parentShape.UpdateGeometryLuminosity(view, pen);
				}
			}
			if (pen != null && !restoreColor.IsEmpty)
			{
				restoreColor = pen.Color;
			}
			return pen;
		}
	}
}
