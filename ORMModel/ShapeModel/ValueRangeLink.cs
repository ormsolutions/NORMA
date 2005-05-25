using System;
using System.Collections;
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
	public partial class ValueRangeLink
	{
		#region Customize appearance
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			ORMDesignerFontsAndColors colorService = ORMDesignerPackage.FontAndColorService;
			PenSettings settings = new PenSettings();
			settings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			settings.DashStyle = DashStyle.Dash;
			settings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, settings);

			settings.Color = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, settings);
		}
		/// <summary>
		/// Use a straight line routing style
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
		/// Selecting role player links gets in the way of selecting roleboxes, etc.
		/// It is best just to turn them off. This also eliminates a bunch of unnamed
		/// roles from the property grid element picker.
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
				return DiagramPens.ConnectionLine;
			}
		}
		#endregion // Customize appearance
		#region ValueRangeLink specific
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
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public RoleHasValueRangeDefinition AssociatedRangeDefinitionLink
		{
			get
			{
				return ModelElement as RoleHasValueRangeDefinition;
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
				RoleHasValueRangeDefinition modelLink = ModelElement as RoleHasValueRangeDefinition;
				ValueRangeDefinition valueRangeDefn = modelLink.ValueRangeDefinition;
				Role role = modelLink.Role;
				NodeShape fromShape;
				NodeShape toShape;
				if (null != (fromShape = diagram.FindShapeForElement(role.FactType) as NodeShape) &&
					null != (toShape = diagram.FindShapeForElement(valueRangeDefn) as NodeShape))
				{
					Connect(fromShape, toShape);
				}
			}
		}
		#endregion // ValueRangeLink specific
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
