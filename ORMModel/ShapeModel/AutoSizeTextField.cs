using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// An extension of a TextField shape. The minimum size is recalculated
	/// according to the current shape contents.
	/// </summary>
	public class AutoSizeTextField : TextField
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AutoSizeTextField()
		{
		}
		/// <summary>
		/// Get the minimum width of the shape field for the current text.
		/// </summary>
		/// <param name="parentShape">ShapeElement</param>
		/// <returns>Width of current text</returns>
		public override double GetMinimumWidth(ShapeElement parentShape)
		{
			return Math.Max(base.GetMinimumWidth(parentShape), GetTextSize(parentShape).Width);
		}
		/// <summary>
		/// Get the minimum height of the shape field for the current text.
		/// </summary>
		/// <param name="parentShape">ShapeElement</param>
		/// <returns>Width of current text</returns>
		public override double GetMinimumHeight(ShapeElement parentShape)
		{
			return Math.Max(base.GetMinimumHeight(parentShape), GetTextSize(parentShape).Height);
		}
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern IntPtr GetDesktopWindow();
		/// <summary>
		/// Get the size of the given text in the context of this parent shape.
		/// Helper function for GetMinimumWidth and GetMinimumHeight.
		/// </summary>
		/// <param name="parentShape">ShapeElement</param>
		/// <returns></returns>
		private SizeF GetTextSize(ShapeElement parentShape)
		{
			SizeF textSize = SizeF.Empty;
			string text = GetDisplayText(parentShape);
			if (text != null && text.Length != 0)
			{
				using (Font font = GetFont(parentShape))
				{
					using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
					{
						textSize = g.MeasureString(text, font);
					}
				}
			}
			return textSize;
		}
		/// <summary>
		/// Modify the display text for independent object types.
		/// </summary>
		/// <param name="parentShape">The ShapeElement to get the display text for.</param>
		/// <returns>The text to display.</returns>
		public override string GetDisplayText(ShapeElement parentShape)
		{
			string text = base.GetDisplayText(parentShape);
			ObjectModel.ObjectType obj;
			if (!(this is ReferenceModeAutoSizeTextField) &&
				null != (obj = parentShape.ModelElement as ObjectModel.ObjectType))
			{
				if (obj.IsIndependent)
				{
					text = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeIsIndependentReading, text);
				}
			}
			return text;
		}
	}
}
