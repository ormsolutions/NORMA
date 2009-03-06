#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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

// Defining LINKS_ALWAYS_CONNECT allows multiple links from a single ShapeA to different instances of ShapeB.
// In this case, the 'anchor' end is always connected if an opposite shape is available.
// The current behavior is to only create a link if, given an instance of ShapeA, the closest candidate
// ShapeB instance is not closer to a different instance of ShapeA.
// Note that LINKS_ALWAYS_CONNECT is also used in other files, so you should turn this on
// in the project properties if you want to experiment. This is here for reference only.
//#define LINKS_ALWAYS_CONNECT

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ModelNoteLink : ISelectionContainerFilter, IReconfigureableLink, IConfigureAsChildShape, IAutoCreatedSelectableShape
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
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected new void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			base.ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
			Reconfigure(null);
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
		}
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected void Reconfigure(ShapeElement discludedShape)
		{
			ModelNoteReferencesModelElement link = ModelElement as ModelNoteReferencesModelElement;
			MultiShapeUtility.ReconfigureLink(this, link.Note, link.Element, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
#if LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		protected override ORMSolutions.ORMArchitect.Framework.Diagrams.BinaryLinkAnchor Anchor
		{
			get
			{
				return ORMSolutions.ORMArchitect.Framework.Diagrams.BinaryLinkAnchor.FromShape;
			}
		}
#endif //LINKS_ALWAYS_CONNECT
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
