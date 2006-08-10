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
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ShapeModel
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
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			PenSettings settings = new PenSettings();
			settings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			settings.DashStyle = DashStyle.Dash;
			settings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, settings);

			settings.Color = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, settings);
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
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public RoleHasValueConstraint AssociatedValueConstraintLink
		{
			get
			{
				return ModelElement as RoleHasValueConstraint;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		public override void ConfiguringAsChildOf(ORMDiagram diagram, bool createdDuringViewFixup)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				RoleHasValueConstraint modelLink = ModelElement as RoleHasValueConstraint;
				ValueConstraint valueRangeDefn = modelLink.ValueConstraint;
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
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.ValueRangeLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.ValueRangeLinkAccessibleDescription;
			}
		}
		#endregion // Accessibility Properties
	}
}
