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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region ModelNoteShape class
	public partial class ModelNoteShape : IConfigureAsChildShape, IDynamicColorGeometryHost
	{
		private static AutoSizeTextField myTextField;
		// Now combined in DSL InitialWidth and InitialHeight
		private const double EdgeMargin = .012;
		#region Customize Appearance
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
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// Ensure initial size.
		/// </summary>
		protected void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			AutoResize();
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
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
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			ModelNote element;
			Store store;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ModelNoteShape, ModelNote>[] providers;
			if (penId == DiagramPens.ShapeOutline &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ModelNoteShape, ModelNote>>()) &&
				null != (element = (ModelNote)ModelElement))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.Outline, this, element);
					if (alternateColor != Color.Empty)
					{
						retVal = pen.Color;
						pen.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			return UpdateDynamicColor(penId, pen);
		}
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Brush)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			Color retVal = Color.Empty;
			SolidBrush solidBrush;
			ModelNote element;
			Store store;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ModelNoteShape, ModelNote>[] providers;
			bool isBackgroundBrush;
			if (((isBackgroundBrush = brushId == DiagramBrushes.DiagramBackground || brushId == ORMDiagram.TransparentBrushResource) ||
				brushId == DiagramBrushes.ShapeText) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ModelNoteShape, ModelNote>>()) &&
				null != (element = (ModelNote)ModelElement))
			{
				ORMDiagramDynamicColor requestColor = isBackgroundBrush ? ORMDiagramDynamicColor.Background : ORMDiagramDynamicColor.ForegroundText;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(requestColor, this, element);
					if (alternateColor != Color.Empty)
					{
						retVal = solidBrush.Color;
						solidBrush.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return UpdateDynamicColor(brushId, brush);
		}
		#endregion // IDynamicColorGeometryHost Implementation
		#endregion // Customize Appearance
		#region TextField Integration
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
		#endregion // TextField Integration
		#region Shape display update rules
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Note), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
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
		#region Deserialization Fixup
		/// <summary>
		/// <see cref="ModelNoteShape"/> elements are rendering with different
		/// sizes on different Visual Studio/DSL versions. Resize them on load
		/// until we can get consistent drawing across the different platforms.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ModelNoteShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset the size of a ModelNoteShape.
		/// </summary>
		private sealed class ModelNoteShapeFixupListener : DeserializationFixupListener<ModelNoteShape>
		{
			/// <summary>
			/// Create a new ModelNoteShapeFixupListener
			/// </summary>
			public ModelNoteShapeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Update the shape size on load.
			/// </summary>
			protected sealed override void ProcessElement(ModelNoteShape element, Store store, INotifyElementAdded notifyAdded)
			{
				element.AutoResize();
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // ModelNoteShape class
}
