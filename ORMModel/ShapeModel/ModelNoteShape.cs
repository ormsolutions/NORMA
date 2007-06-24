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
using Neumont.Tools.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ShapeModel
{
	#region ModelNoteShape class
	public partial class ModelNoteShape
	{
		private static AutoSizeTextField myTextField;
		// Now combined in DSL InitialWidth and InitialHeight
		private const double EdgeMargin = .012;
		/// <summary>
		/// Gets and sets the AutoSizeTextField shape for this object
		/// </summary>
		protected override AutoSizeTextField TextShapeField
		{
			get
			{
				return myTextField;
			}
			set
			{
				Debug.Assert(myTextField == null);
				myTextField = value;
			}
		}
		/// <summary>
		/// Adjust the outline pen width
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Color = SystemColors.GrayText;
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, penSettings);
		}
		/// <summary>
		/// Gets the DomainProperty to bind to the text field
		/// </summary>
		protected override Guid AssociatedModelDomainPropertyId
		{
			get
			{
				return Note.TextDomainPropertyId;
			}
		}
		/// <summary>
		/// Add an outline to the note shape
		/// </summary>
		public override bool HasOutline
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Highlight the shape when the mouse moves over it
		/// </summary>
		public override bool HasHighlighting
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Ensure initial size
		/// </summary>
		public override void ConfiguringAsChildOf(NodeShape parent, bool createdDuringViewFixup)
		{
			AutoResize();
		}
		/// <summary>
		/// Show a shadow if this <see cref="ModelNoteShape"/> represents an <see cref="ModelNote"/> that appears
		/// in more than one location.
		/// </summary>
		public override bool HasShadow
		{
			get
			{
				return ORMBaseShape.ElementHasMultiplePresentations(this);
			}
		}
		/// <summary>
		/// Support automatic appearance updating when multiple presentations are present.
		/// </summary>
		public override bool DisplaysMultiplePresentations
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Connect lines to the edge of the rectangular shape
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return CustomFoldRectangleShapeGeometry.ShapeGeometry;
			}
		}
		/// <summary>
		/// Adjust the content size
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				SizeD size = base.ContentSize;
				SizeD defaultSize = DefaultSize;
				size.Width += EdgeMargin;
				size.Height += EdgeMargin;
				size.Width = Math.Max(size.Width, defaultSize.Width);
				size.Height = Math.Max(size.Height, defaultSize.Height);
				return size;
			}
		}
		/// <summary>
		/// Create the text field
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		/// <returns>AutoSizeTextField</returns>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			return new NoteTextField(fieldName);
		}
		/// <summary>
		/// An auto-size text field for the diagram shape
		/// </summary>
		private class NoteTextField : AutoSizeTextField
		{
			/// <summary>
			/// Create a new NoteTextField
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public NoteTextField(string fieldName)
				: base(fieldName)
			{
				DefaultMultipleLine = true;
				StringFormat fieldFormat = new StringFormat(StringFormatFlags.NoClip);
				fieldFormat.Alignment = StringAlignment.Near;
				DefaultStringFormat = fieldFormat;
			}
		}
		#region Shape display update rules
		/// <summary>
		/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Note), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void NoteChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			if (attributeGuid == Note.TextDomainPropertyId)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(e.ModelElement))
				{
					ORMBaseShape shape = pel as ORMBaseShape;
					if (shape != null)
					{
						shape.AutoResize();
					}
				}
			}
		}
		#endregion // Shape display update rules
	}
	#endregion // ModelNoteShape class
}
