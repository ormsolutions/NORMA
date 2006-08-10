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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ModelNoteLink : ISelectionContainerFilter
	{
		#region Customize appearance
		/// <summary>
		/// Override the connection line pen with a dashed pen
		/// </summary>
		/// <param name="classStyleSet"></param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings penSettings = new PenSettings();
			penSettings.DashStyle = DashStyle.Dot;
			penSettings.Color = SystemColors.GrayText;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
		}
		/// <summary>
		/// Display comment lines behind all others
		/// </summary>
		public override double ZOrder
		{
			get
			{
				return 0;
			}
			set
			{
				// Don't set
			}
		}
		#endregion // Customize appearance
		#region ModelNoteLink specific
		/// <summary>
		/// Get the ModelNoteReferencesModelElement link associated with this link shape. The
		/// note link can be used to get the associated roles.
		/// </summary>
		public ModelNoteReferencesModelElement AssociatedNoteLink
		{
			get
			{
				return ModelElement as ModelNoteReferencesModelElement;
			}
		}
		/// <summary>
		/// Allow link selection. This is the only way to delete the link.
		/// The link is selectable, but does not appear as part of the selection
		/// container because it implements ISelectionContainerFilter.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Highlight the link to indicate it is selectable
		/// </summary>
		public override bool HasHighlighting
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
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		public override void ConfiguringAsChildOf(ORMDiagram diagram, bool createdDuringViewFixup)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				ModelNoteReferencesModelElement link = ModelElement as ModelNoteReferencesModelElement;
				ORMBaseShape fromShape;
				ORMBaseShape toShape;
				if (null != (fromShape = diagram.FindShapeForElement<ORMBaseShape>(link.Note)) &&
					null != (toShape = diagram.FindShapeForElement<ORMBaseShape>(link.Element)))
				{
					Connect(fromShape, toShape);
				}
			}
		}
		#endregion // ModelNoteLink specific
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.ModelNoteLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.ModelNoteLinkAccessibleDescription;
			}
		}
		#endregion // Accessibility Properties
		#region ISelectionContainerFilter Implementation
		/// <summary>
		/// Implements ISelectionContainerFilter.IncludeInSelectionContainer
		/// </summary>
		protected static bool IncludeInSelectionContainer
		{
			get
			{
				return false;
			}
		}
		bool ISelectionContainerFilter.IncludeInSelectionContainer
		{
			get
			{
				return IncludeInSelectionContainer;
			}
		}
		#endregion // ISelectionContainerFilter Implementation
	}
}
