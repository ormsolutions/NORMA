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
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ValueRangeLink : IReconfigureableLink, IConfigureAsChildShape, IDynamicColorGeometryHost
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
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for deontic constraints
		/// </summary>
		protected virtual StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					// Set up an alternate style set for drawing deontic constraints
					retVal = new StyleSet(ClassStyleSet);
					InitializeDeonticClassStyleSet(retVal);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Initialize a <see cref="StyleSet"/> for rendering deontic constraints.
		/// The style set is created in <see cref="DeonticClassStyleSet"/> and
		/// initialized here.
		/// </summary>
		/// <remarks>(Currently there are no derived classes, so this is informational
		/// if a derived class is added for some reason).
		/// If a derived class does not modify additional resources in the
		/// default style set, then this method is not required and any derived deontic
		/// style set can be based on the deontic style set for this base class. However,
		/// if new resources are introduced, then the derived class should base a
		/// deontic style set on the derived class style set and reinitialize the
		/// deontic settings in that style set.</remarks>
		protected virtual void InitializeDeonticClassStyleSet(StyleSet styleSet)
		{
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			PenSettings penSettings = new PenSettings();
			penSettings.Color = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
			styleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
		}
		/// <summary>
		/// Switch between alethic and deontic style sets to draw
		/// the mandatory dot correctly
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				RoleHasValueConstraint link;
				RoleValueConstraint constraint;
				if ((null != (link = AssociatedValueConstraintLink)) &&
					(null != (constraint = link.ValueConstraint)) &&
					constraint.Modality == ConstraintModality.Deontic)
				{
					// Note that we don't do anything with fonts with this style set, so the
					// static one is sufficient. Instance style sets also go through a font initiation
					// step inside the framework
					return DeonticClassStyleSet;
				}
				return base.StyleSet;
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
		/// <summary>
		/// Set ZOrder layer
		/// </summary>
		public override double ZOrder
		{
			get
			{
				return base.ZOrder + ZOrderLayer.ConstraintConnectors;
			}
		}
		#endregion // Customize appearance
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			RoleHasValueConstraint link;
			RoleValueConstraint constraint;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ValueRangeLink, RoleValueConstraint>[] providers;
			Store store;
			if (penId == DiagramPens.ConnectionLine &&
				null != (link = ModelElement as RoleHasValueConstraint) &&
				null != (constraint = link.ValueConstraint) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ValueRangeLink, RoleValueConstraint>>(true)))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(constraint.Modality == ConstraintModality.Deontic ? ORMDiagramDynamicColor.DeonticConstraint : ORMDiagramDynamicColor.Constraint, this, constraint);
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
		protected static Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return Color.Empty;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return UpdateDynamicColor(brushId, brush);
		}
		#endregion // IDynamicColorGeometryHost Implementation
		#region ValueRangeLink specific
		/// <summary>
		/// Get the <see cref="RoleHasValueConstraint"/> link associated with this link shape
		/// </summary>
		public RoleHasValueConstraint AssociatedValueConstraintLink
		{
			get
			{
				return ModelElement as RoleHasValueConstraint;
			}
		}
		/// <summary>
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected new void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			base.ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
			// We have to make sure each shape is connected to its parent,
			// so we can't use the MultiShapeUtility.

			Diagram diagram = parentShape as Diagram;
			if (diagram == null)
			{
				return;
			}
			RoleHasValueConstraint modelLink = ModelElement as RoleHasValueConstraint;
			ModelElement factType = modelLink.Role.FactType;
			//connect the first unconnected constraint shape to its parent
			foreach (ValueConstraintShape shape in MultiShapeUtility.FindAllShapesForElement<ValueConstraintShape>(diagram, modelLink.ValueConstraint))
			{
				bool connected = false;
				foreach (ShapeElement link in shape.ToRoleLinkShapes)
				{
					//check if the shape is already connected
					ValueRangeLink valueRangeLink;
					if ((valueRangeLink = link as ValueRangeLink) != null &&
						valueRangeLink.FromShape.ModelElement == factType)
					{
						connected = true;
						break;
					}
				}
				FactTypeShape factTypeShape;
				if (connected ||
					null == (factTypeShape = shape.ParentShape as FactTypeShape) ||
					factTypeShape.DisplayAsObjectType)
				{
					continue;
				}


				Connect(factTypeShape, shape);
				return;
			}
			Delete();
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
			// We want to leave the link so that the child remains connected to its parent,
			// unless one of the connected shapes is being deleted.
			if (discludedShape != null && discludedShape == ToShape || discludedShape == FromShape)
			{
				Delete();
			}
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
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
