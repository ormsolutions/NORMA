#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class FrequencyConstraintShape : ExternalConstraintShape, IModelErrorActivation
	{
		#region Customize appearance
		private static readonly StyleSetResourceId FrequencyConstraintTextResource = new StyleSetResourceId("ORMArchitect", "FrequencyConstraintText");
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
					using (Font font = StyleSet.GetFont(FrequencyConstraintTextResource))
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
			if (null == Utility.ValidateStore(Store))
			{
				return;
			}
			InitializePaintTools(e);
			base.OnPaintShape(e);
			RectangleD bounds = AbsoluteBounds;
			Graphics g = e.Graphics;
			StringFormat stringFormat = new StringFormat();
			stringFormat.LineAlignment = StringAlignment.Center;
			stringFormat.Alignment = StringAlignment.Center;
			Font font = StyleSet.GetFont(FrequencyConstraintTextResource);
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
				else if (minFreq == 1 && maxFreq > 1)
				{
					freqString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FrequencyConstraintMinimumOneFormatString, maxFreq);
				}
				else if (minFreq + maxFreq != 0)
				{
					freqString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FrequencyConstraintBetweenFormatString, minFreq, maxFreq);
				}
				else
				{
					Debug.Fail("Model should have prevented this");
				}
			}
			return freqString;
		}
		/// <summary>
		/// Set the font size
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			FontSettings textSettings = new FontSettings();
			classStyleSet.AddFont(FrequencyConstraintTextResource, DiagramFonts.CommentText, textSettings);
		}
		/// <summary>
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for deontic constraints
		/// </summary>
		protected override StyleSet DeonticClassStyleSet
		{
			get
			{
				// This class introduces additional resources, so we
				// need to create our own deontic class style set
				// based on our own class set instead of the deontic
				// class style set of the base.
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(ClassStyleSet);
					InitializeDeonticClassStyleSet(retVal);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Draw frequency constraints with a transparent background
		/// in non-error states.
		/// </summary>
		protected override bool HasTransparentBackground
		{
			get
			{
				return true;
			}
		}
		#endregion // Customize appearance
		#region Shape display update rules
		#region FrequencyConstraintPropertyChangeRule
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void FrequencyConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == FrequencyConstraint.MinFrequencyDomainPropertyId ||
				attributeId == FrequencyConstraint.MaxFrequencyDomainPropertyId)
			{
				FrequencyConstraint fc = e.ModelElement as FrequencyConstraint;
				if (!fc.IsDeleted)
				{
					// Resize the frequency constraint wherever it is displayed, and make sure
					// the object type is made visible in the same location.
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(fc))
					{
						ExternalConstraintShape externalConstraintShape = pel as ExternalConstraintShape;
						if (externalConstraintShape != null)
						{
							SizeD oldSize = externalConstraintShape.Size;
							externalConstraintShape.AutoResize();
							if (oldSize == externalConstraintShape.Size)
							{
								((IInvalidateDisplay)externalConstraintShape).InvalidateRequired(true);
							}
						}
					}
				}
			}
		}
		#endregion // FrequencyConstraintPropertyChangeRule
		#region FrequencyConstraintConversionDeletingRule
		/// <summary>
		/// DeletingRule: typeof(FrequencyConstraintShape)
		/// If the FrequencyConstraintShape is being deleted as a result of conversion to a uniqueness constraint
		/// </summary>
		private static void FrequencyConstraintConversionDeletingRule(ElementDeletingEventArgs e)
		{
			FrequencyConstraintShape shape;
			UniquenessConstraint convertingTo;
			IDictionary<object, object> contextInfo;
			if (e.ChangeSource == ChangeSource.Propagate &&
				(contextInfo = (shape = (FrequencyConstraintShape)e.ModelElement).Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(FrequencyConstraint.ConvertingToUniquenessConstraintKey) &&
				!(convertingTo = (UniquenessConstraint)contextInfo[FrequencyConstraint.ConvertingToUniquenessConstraintKey]).IsInternal &&
				shape.ModelElement == contextInfo[FrequencyConstraint.ConvertingFromFrequencyConstraintKey])
			{
				((ORMDiagram)shape.Diagram).PlaceORMElementOnDiagram(null, convertingTo, shape.Location, ORMPlacementOption.AllowMultipleShapes, null, null);
			}
		}
		#endregion // FrequencyConstraintConversionDeletingRule
		#endregion // Shape display update rules
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorActivation.ActivateModelError"/> for
		/// the <see cref="FrequencyConstraintExactlyOneError"/>
		/// </summary>
		protected new bool ActivateModelError(ModelError error)
		{
			FrequencyConstraintExactlyOneError exactlyOneError;
			bool retVal = false;
			if (null != (exactlyOneError = error as FrequencyConstraintExactlyOneError))
			{
				retVal = ((FrequencyConstraint)AssociatedConstraint).ConvertToUniquenessConstraint();
			}
			return retVal ? true : base.ActivateModelError(error);
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
}
