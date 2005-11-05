using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class SubtypeLink
	{
		#region Customize appearance
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				return LinkDecorator.DecoratorFilledArrow;
			}
			set
			{
			}
		}
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			IORMFontAndColorService colorService = (this.Store as IORMToolServices).FontAndColorService;
			Color lineColor = colorService.GetForeColor(ORMDesignerColor.Constraint);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.8F / 72.0F; // 1.8 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			penSettings = new PenSettings();
			penSettings.Width = 1.4F / 72.0F; // Soften the arrow a bit
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = lineColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// Subtype links need to be selectable to enable readings, etc
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Get a geometry we can click on
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return ObliqueBinaryLinkShapeGeometry.ShapeGeometry;
			}
		}
		#endregion // Customize appearance
		#region SubtypeLink specific
		/// <summary>
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public SubtypeFact AssociatedSubtypeFact
		{
			get
			{
				return ModelElement as SubtypeFact;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public override void ConfiguringAsChildOf(ORMDiagram diagram)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				SubtypeFact subtypeFact = AssociatedSubtypeFact;
				ObjectType subType = subtypeFact.Subtype;
				ObjectType superType = subtypeFact.Supertype;
				FactType nestedSubFact = subType.NestedFactType;
				FactType nestedSuperFact = superType.NestedFactType;
				NodeShape fromShape;
				NodeShape toShape;
				if (null != (toShape = diagram.FindShapeForElement((nestedSuperFact == null) ? superType as ModelElement : nestedSuperFact) as NodeShape) &&
					null != (fromShape = diagram.FindShapeForElement((nestedSubFact == null) ? subType as ModelElement : nestedSubFact) as NodeShape))
				{
					Connect(fromShape, toShape);
				}
			}
		}
		#endregion // SubtypeLink specific
	}
	public partial class ORMShapeModel
	{
		#region  DisplaySubtypeLinkFixupListener
		/// <summary>
		/// A fixup class to display subtype links
		/// </summary>
		private class DisplaySubtypeLinkFixupListener : DeserializationFixupListener<ModelHasFactType>
		{
			/// <summary>
			/// Create a new DisplayRolePlayersFixupListener
			/// </summary>
			public DisplaySubtypeLinkFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add subtype links when possible
			/// </summary>
			/// <param name="element">An ModelHasFactType instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ModelHasFactType element, Store store, INotifyElementAdded notifyAdded)
			{
				SubtypeFact subTypeFact = element.FactTypeCollection as SubtypeFact;
				if (subTypeFact != null)
				{
					Diagram.FixUpDiagram(subTypeFact.Model, subTypeFact);
				}
			}
		}
		#endregion // DisplaySubtypeLinkFixupListener class
	}
}
