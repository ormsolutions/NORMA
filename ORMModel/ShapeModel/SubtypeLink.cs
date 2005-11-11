using System;
using System.Collections;
using System.Collections.Generic;
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
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			IORMFontAndColorService colorService = (this.Store as IORMToolServices).FontAndColorService;
			Color lineColor = colorService.GetForeColor(ORMDesignerColor.Constraint);
			Color stickyColor = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			Color activeColor = colorService.GetBackColor(ORMDesignerColor.RolePicker);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.8F / 72.0F; // 1.8 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			penSettings.Color = stickyColor;
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, penSettings);
			penSettings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.ActiveBackgroundResource, DiagramPens.ConnectionLine, penSettings);
			penSettings = new PenSettings();
			penSettings.Width = 1.4F / 72.0F; // Soften the arrow a bit
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			penSettings.Color = stickyColor;
			classStyleSet.AddPen(ORMDiagram.StickyConnectionLineDecoratorResource, DiagramPens.ConnectionLineDecorator, penSettings);
			penSettings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.ActiveConnectionLineDecoratorResource, DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = lineColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
			brushSettings.Color = stickyColor;
			classStyleSet.AddBrush(ORMDiagram.StickyConnectionLineDecoratorResource, DiagramBrushes.ConnectionLineDecorator, brushSettings);
			brushSettings.Color = activeColor;
			classStyleSet.AddBrush(ORMDiagram.ActiveConnectionLineDecoratorResource, DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// Specifies the three different color styles used to draw
		/// a subtype link
		/// </summary>
		private enum DrawColorStyle
		{
			/// <summary>
			/// Draw as a normal constraint
			/// </summary>
			Normal,
			/// <summary>
			/// Draw as an active part of a sticky object
			/// </summary>
			Sticky,
			/// <summary>
			/// Draw as a currently selected item in an active
			/// constraint editing operation
			/// </summary>
			Active,
		}
		private DrawColorStyle ColorStyle
		{
			get
			{
				ORMDiagram diagram = Diagram as ORMDiagram;
				ExternalConstraintConnectAction action = diagram.ExternalConstraintConnectAction;
				IConstraint testConstraint = action.ActiveConstraint;
				IList<Role> selectedRoles = null;
				if (testConstraint == null)
				{
					IStickyObject sticky = diagram.StickyObject;
					if (sticky != null)
					{
						ExternalConstraintShape shape = sticky as ExternalConstraintShape;
						if (shape != null)
						{
							testConstraint = shape.AssociatedConstraint;
						}
					}
				}
				else
				{
					selectedRoles = action.SelectedRoleCollection;
				}
				if (testConstraint != null)
				{
					SubtypeFact associatedSubtype = AssociatedSubtypeFact;
					if (null != selectedRoles && selectedRoles.Contains(associatedSubtype.SupertypeRole))
					{
						return DrawColorStyle.Active;
					}
					else
					{
						FactTypeMoveableCollection facts = null;
						switch (testConstraint.ConstraintStorageStyle)
						{
							case ConstraintStorageStyle.SingleColumnExternalConstraint:
								facts = ((SingleColumnExternalConstraint)testConstraint).FactTypeCollection;
								break;
							case ConstraintStorageStyle.MultiColumnExternalConstraint:
								facts = ((MultiColumnExternalConstraint)testConstraint).FactTypeCollection;
								break;
						}
						if (facts != null && facts.Contains(AssociatedSubtypeFact))
						{
							return DrawColorStyle.Sticky;
						}
					}
				}
				return DrawColorStyle.Normal;
			}
		}
		/// <summary>
		/// A filled arrow decorator drawn with sticky pens and brushes
		/// </summary>
		private class StickyFilledArrowDecorator : DecoratorFilledArrow
		{
			public static readonly LinkDecorator Decorator = new StickyFilledArrowDecorator();
			public override StyleSetResourceId BrushId
			{
				get
				{
					return ORMDiagram.StickyConnectionLineDecoratorResource;
				}
			}
			public override StyleSetResourceId PenId
			{
				get
				{
					return ORMDiagram.StickyConnectionLineDecoratorResource;
				}
			}
		}
		/// <summary>
		/// A filled arrow decorator drawn with active pens and brushes
		/// </summary>
		private class ActiveFilledArrowDecorator : DecoratorFilledArrow
		{
			public static readonly LinkDecorator Decorator = new ActiveFilledArrowDecorator();
			public override StyleSetResourceId BrushId
			{
				get
				{
					return ORMDiagram.ActiveConnectionLineDecoratorResource;
				}
			}
			public override StyleSetResourceId PenId
			{
				get
				{
					return ORMDiagram.ActiveConnectionLineDecoratorResource;
				}
			}
		}
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				DrawColorStyle style = ColorStyle;
				switch (style)
				{
					case DrawColorStyle.Sticky:
						return StickyFilledArrowDecorator.Decorator;
					case DrawColorStyle.Active:
						return ActiveFilledArrowDecorator.Decorator;
					default:
						Debug.Assert(style == DrawColorStyle.Normal);
						return LinkDecorator.DecoratorFilledArrow;
				}
			}
			set
			{
			}
		}
		/// <summary>
		/// Change the connection line pen if the subtype is sticky or
		/// a selected role in an active constraint
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				DrawColorStyle style = ColorStyle;
				switch (style)
				{
					case DrawColorStyle.Sticky:
						return ORMDiagram.StickyBackgroundResource;
					case DrawColorStyle.Active:
						return ORMDiagram.ActiveBackgroundResource;
					default:
						Debug.Assert(style == DrawColorStyle.Normal);
						return DiagramPens.ConnectionLine;
				}
			}
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
