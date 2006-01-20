using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class FrequencyConstraintShape : ExternalConstraintShape
	{
		#region Customize appearance
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern IntPtr GetDesktopWindow();
		/// <summary>
		/// Return a  size for this frequency constraint
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				double width = DefaultSize.Width;
				String text = GetFrequencyString();
				SizeF textSize = SizeF.Empty;
				if (text != null && text.Length != 0)
				{
					using (Font font = StyleSet.GetFont(DiagramFonts.CommentText))
					{
						using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
						{
							textSize = g.MeasureString(text, font);
							textSize.Width += textSize.Height / 2;
						}
					}
				}
				if (width < textSize.Width)
				{
					width = (double)textSize.Width;
				}
				return new SizeD(width, width);
			}
		}
		/// <summary>
		/// Determine whether or not to draw the default shape outline.
		/// </summary>
		public override bool HasOutline
		{
			get
			{
				if (IsSingleFactFrequencyConstraint())
				{
					//Don't draw the outline for Frequency Constraints applied to one facttype
					return false;
				}
				else
				{
					return base.HasOutline;
				}
			}
		}
		/// <summary>
		/// Frequency Constraints applied to a single facttype are handled slightly differently. Test if the constraint meets this definition.
		/// </summary>
		/// <returns>True if constraint is a Frequency Constraint applied to a single facttype; otherwise, false.</returns>
		private bool IsSingleFactFrequencyConstraint()
		{
			IConstraint constraint = AssociatedConstraint;
			if (constraint != null &&
				constraint.ConstraintType == ConstraintType.Frequency)
			{
				FrequencyConstraint fc = (FrequencyConstraint)constraint;
				return fc.FactTypeCollection.Count == 1;
			}
			return false;
		}
		/// <summary>
		/// Draw the various constraint types
		/// </summary>
		/// <param name="e">DiagramPaintEventArgs</param>
		public override void OnPaintShape(DiagramPaintEventArgs e)
		{
			this.InitializePaintTools(e);
			base.OnPaintShape(e);
			RectangleD bounds = AbsoluteBounds;
			Graphics g = e.Graphics;
			StringFormat stringFormat = new StringFormat();
			stringFormat.LineAlignment = StringAlignment.Center;
			stringFormat.Alignment = StringAlignment.Center;
			Font font = StyleSet.GetFont(DiagramFonts.CommentText);
			Brush brush = this.PaintBrush;
			g.DrawString(GetFrequencyString(), font, brush, RectangleD.ToRectangleF(bounds), stringFormat);
			this.DisposePaintTools();
		}
		/// <summary>
		/// Builds a string representing a Frequency Constraint
		/// </summary>
		private string GetFrequencyString()
		{
			FrequencyConstraint fc = AssociatedConstraint as FrequencyConstraint;
			string freqString = "";
			if (null != fc)
			{
				int minFreq = fc.MinFrequency;
				int maxFreq = fc.MaxFrequency;
				//TODO: Frequencies should not be negative values. An "invalid value" error
				//should be shown in the error list if a frequency is < 0.
				//Similarly, an error should be shown if minFreq > maxFreq.
				if (minFreq > 0 && maxFreq == 0)
				{
					freqString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FrequencyConstraintMinimumFormatString, minFreq);
				}
				else if (minFreq == maxFreq && minFreq != 0)
				{
					return minFreq.ToString(CultureInfo.InvariantCulture);
				}
				else if (minFreq + maxFreq != 0)
				{
					freqString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FrequencyConstraintBetweenFormatString, minFreq, maxFreq);
				}
				else
				{
					Debug.Assert(false); // Model should have prevented this
				}
			}
			return freqString;
		}
		#endregion // Customize appearance
		#region Shape display update rules
		#region FrequencyConstraintAttributeChangeRule class
		[RuleOn(typeof(FrequencyConstraint), FireTime = TimeToFire.LocalCommit)]
		private class FrequencyConstraintAttributeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == FrequencyConstraint.MinFrequencyMetaAttributeGuid ||
					attributeId == FrequencyConstraint.MaxFrequencyMetaAttributeGuid)
				{
					FrequencyConstraint fc = e.ModelElement as FrequencyConstraint;
					if (!fc.IsRemoved)
					{
						// Resize the frequency constraint wherever it is displayed, and make sure
						// the object type is made visible in the same location.
						foreach (object obj in fc.AssociatedPresentationElements)
						{
							ExternalConstraintShape externalConstraintShape = obj as ExternalConstraintShape;
							if (externalConstraintShape != null)
							{
								externalConstraintShape.AutoResize();
								externalConstraintShape.Invalidate(true);
							}
						}
					}
				}
			}
		}
		#endregion // FrequencyConstraintAttributeChangeRule class
		#endregion // Shape display update rules
	}
}
