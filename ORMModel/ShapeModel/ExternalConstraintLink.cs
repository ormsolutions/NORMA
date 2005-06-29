using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ExternalConstraintLink
	{
		#region Customize appearance
		/// <summary>
		/// Override the connection line pen with a dashed pen
		/// </summary>
		/// <param name="classStyleSet"></param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			IORMFontAndColorService colorService = (this.Store as IORMToolServices).FontAndColorService;
			PenSettings settings = new PenSettings();
			settings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			settings.DashStyle = DashStyle.Dash;
			settings.Width = 1.0F/72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, settings);

			settings.Color = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, settings);
		}
		/// <summary>
		/// Use a center to center routing style
		/// </summary>
		[CLSCompliant(false)]
		protected override VGRoutingStyle DefaultRoutingStyle
		{
			get
			{
				return VGRoutingStyle.VGRouteCenterToCenter;
			}
		}
		/// <summary>
		/// Selecting external constraint links gets in the way of selecting other primary
		/// objects. It is best just to turn them off. This also eliminates a bunch of unnamed
		/// constraint links from the property grid element picker.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Draw the connection lines as sticky along with the constraint
		/// and associated roles
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				PresentationElement stickyPel;
				IFactConstraint factConstraint;
				if (null != (stickyPel = (Diagram as ORMDiagram).StickyObject as PresentationElement) &&
					null != (factConstraint = AssociatedFactConstraint as IFactConstraint) &&
					object.ReferenceEquals(stickyPel.ModelElement, factConstraint.Constraint))
				{
					return ORMDiagram.StickyBackgroundResource;
				}
				return DiagramPens.ConnectionLine;
			}
		}
		#endregion // Customize appearance
		#region ExternalConstraintLink specific
		/// <summary>
		/// Stop the user from manually routine link lines
		/// </summary>
		/// <value>false</value>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Get the FactConstraint link associated with this link shape. The
		/// fact constraint link can be used to get the associated roles.
		/// </summary>
		public ExternalFactConstraint AssociatedFactConstraint
		{
			get
			{
				return ModelElement as ExternalFactConstraint;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public void ConfiguringAsChildOf(ORMDiagram diagram)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				IFactConstraint modelLink = ModelElement as IFactConstraint;
				FactType attachedFact = modelLink.FactType;
				IConstraint constraint = modelLink.Constraint;
				NodeShape fromShape;
				NodeShape toShape;
				if (null != (fromShape = diagram.FindShapeForElement(constraint as ModelElement) as NodeShape) &&
					null != (toShape = diagram.FindShapeForElement(attachedFact) as NodeShape))
				{
					// Note that the from/to ordering reversal here is a hack so
					// the fact type shape folding code can find the opposite constraint
					// based on its center point. If both ends move the connection point,
					// then only the first one passed in here can find the opposite shape.
					// UNDONE: Slimy hack, should be removed if we get better framework support.
					Connect(toShape, fromShape);
				}
			}
		}
		#endregion // ExternalConstraintLink specific
		#region Luminosity Modification
		/// <summary>
		/// Redirect all luminosity modification to the ORMDiagram.ModifyLuminosity
		/// algorithm
		/// </summary>
		/// <param name="currentLuminosity">The luminosity to modify</param>
		/// <param name="view">The view containing this item</param>
		/// <returns>Modified luminosity value</returns>
		protected override int ModifyLuminosity(int currentLuminosity, DiagramClientView view)
		{
			if (view.HighlightedShapes.Contains(new DiagramItem(this)))
			{
				return ORMDiagram.ModifyLuminosity(currentLuminosity);
			}
			return currentLuminosity;
		}
		#endregion // Luminosity Modification
	}
}