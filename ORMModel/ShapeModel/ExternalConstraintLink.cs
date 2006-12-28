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
	public partial class ExternalConstraintLink
	{
		#region SubsetDecorator class
		/// <summary>
		/// The link decorator used to draw the mandatory
		/// constraint dot on a link.
		/// </summary>
		protected class SubsetDecorator : DecoratorFilledArrow, ILinkDecoratorSettings
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static readonly LinkDecorator Decorator = new SubsetDecorator();
			/// <summary>
			/// Instantiates a new SubsetDecorator
			/// </summary>
			protected SubsetDecorator()
			{
				FillDecorator = true;
			}
			#region ILinkDecoratorSettings Implementation
			/// <summary>
			/// Implements ILinkDecoratorSettings.DecoratorSize.
			/// </summary>
			protected static SizeD DecoratorSize
			{
				get
				{
					return new SizeD(.09, .07d);
				}
			}
			SizeD ILinkDecoratorSettings.DecoratorSize
			{
				get
				{
					return DecoratorSize;
				}
			}
			/// <summary>
			/// Implements ILinkDecoratorSettings.OffsetBy
			/// </summary>
			protected static double OffsetBy
			{
				get
				{
					return 0;
				}
			}
			double ILinkDecoratorSettings.OffsetBy
			{
				get
				{
					return OffsetBy;
				}
			}
			#endregion // ILinkDecoratorSettings Implementation
		}
		#endregion // SubsetDecorator class
		#region SubsetStickyDecorator class
		/// <summary>
		/// A SubsetDecorator to be used when the connector is currently selected (i.e. is sticky)
		/// </summary>
		protected class SubsetStickyDecorator : SubsetDecorator
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static new readonly LinkDecorator Decorator = new SubsetStickyDecorator();
			/// <summary>
			/// Instantiates a new SubsetStickyDecorator
			/// </summary>
			protected SubsetStickyDecorator() { }
			/// <summary>
			/// The StyleSetResource containing the pen to use for drawing this decorator.
			/// </summary>
			public override StyleSetResourceId PenId
			{
				get { return ORMDiagram.StickyForegroundResource; }
			}
			/// <summary>
			/// The StyleSetResource containing the brush to use for drawing this decorator.
			/// </summary>
			public override StyleSetResourceId BrushId
			{
				get { return ORMDiagram.StickyForegroundResource; }
			}
		}
		#endregion // SubsetStickyDecorator class
		#region Customize appearance
		/// <summary>
		/// Override the connection line pen with a dashed pen
		/// </summary>
		/// <param name="classStyleSet"></param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService colorService = (this.Store as IORMToolServices).FontAndColorService;
			Color constraintColor = colorService.GetForeColor(ORMDesignerColor.Constraint);
			Color activeColor = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			PenSettings settings = new PenSettings();
			settings.Color = constraintColor;
			settings.DashStyle = DashStyle.Dash;
			settings.Width = 1.0F/72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, settings);
			settings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, settings);

			settings = new PenSettings();
			settings.Width = 1.0F / 72.0F; // Soften the arrow a bit
			settings.Color = constraintColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, settings);
			settings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.StickyForegroundResource, DiagramPens.ConnectionLineDecorator, settings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = constraintColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
			brushSettings.Color = activeColor;
			classStyleSet.AddBrush(ORMDiagram.StickyForegroundResource, DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for drawing deontic constraints
		/// </summary>
		protected virtual StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(ClassStyleSet);
					IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
					Color constraintColor = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
					PenSettings penSettings = new PenSettings();
					constraintColor = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
					penSettings.Color = constraintColor;
					retVal.OverridePen(DiagramPens.ConnectionLine, penSettings);
					retVal.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = constraintColor;
					retVal.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Switch between alethic and deontic style sets
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				IFactConstraint factConstraint;
				IConstraint constraint;
				if (null != (factConstraint = AssociatedFactConstraint as IFactConstraint) &&
					null != (constraint = factConstraint.Constraint) &&
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
				if (IsSticky())
				{
					return ORMDiagram.StickyBackgroundResource;
				}
				return DiagramPens.ConnectionLine;
			}
		}
		private bool IsSticky()
		{
			PresentationElement stickyPel;
			IFactConstraint factConstraint;
			return (null != (stickyPel = (Diagram as ORMDiagram).StickyObject as PresentationElement) &&
				null != (factConstraint = AssociatedFactConstraint as IFactConstraint) &&
				stickyPel.ModelElement == factConstraint.Constraint);
		}
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorFrom
		{
			get
			{
				FactConstraint efc = AssociatedFactConstraint;
				//Only change the decorator if the ExternalConstraintLink being worked on
				//is for the second constraint role sequence since we need the arrow to make
				//it look like the constraint links point from the role of the first sequence
				//to the role of the second sequence.
				if (efc is FactSetComparisonConstraint)
				{
					SubsetConstraint sConstraint;
					if (null != (sConstraint = efc.LinkedElements[1] as SubsetConstraint))
					{
						NodeShape connectedShape = FromLinkConnectsToNode.Nodes;
						FactTypeShape factTypeShape;
						LinkConnectorShape connectorShape;
						FactType factType = null;
						if (null != (factTypeShape = connectedShape as FactTypeShape))
						{
							factType = factTypeShape.AssociatedFactType;
						}
						else if (null != (connectorShape = connectedShape as LinkConnectorShape))
						{
							SubtypeLink subtypeLink = connectorShape.ParentShape as SubtypeLink;
							if (null != subtypeLink)
							{
								factType = subtypeLink.AssociatedSubtypeFact;
							}
						}
						if (factType == null)
						{
							return base.DecoratorFrom;
						}
						LinkedElementCollection<RoleBase> factTypeRoles = factType.RoleCollection;
						LinkedElementCollection<SetComparisonConstraintRoleSequence> sequenceCollection = sConstraint.RoleSequenceCollection;
						if (sequenceCollection.Count > 1)
						{
							foreach (Role r in sequenceCollection[1].RoleCollection)
							{
								if (factTypeRoles.Contains(r))
								{
									if (IsSticky())
									{
										return SubsetStickyDecorator.Decorator;
									}
									else
									{
										return SubsetDecorator.Decorator;
									}
								}
							}
						}
					}
				}
				return base.DecoratorFrom;
			}
			set
			{
			}
		}
		#endregion // Customize appearance
		#region ExternalConstraintLink specific
		/// <summary>
		/// Get the FactConstraint link associated with this link shape. The
		/// fact constraint link can be used to get the associated roles.
		/// </summary>
		public FactConstraint AssociatedFactConstraint
		{
			get
			{
				return ModelElement as FactConstraint;
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
				IFactConstraint modelLink = ModelElement as IFactConstraint;
				FactType attachedFact = modelLink.FactType;
				IConstraint constraint = modelLink.Constraint;
				NodeShape fromShape = diagram.FindShapeForElement(constraint as ModelElement) as NodeShape;
				if (null != fromShape)
				{
					ShapeElement untypedToShape = diagram.FindShapeForElement(attachedFact);
					NodeShape toShape = untypedToShape as NodeShape;
					if (null == toShape)
					{
						SubtypeLink subTypeLink = untypedToShape as SubtypeLink;
						if (null != subTypeLink)
						{
							toShape = subTypeLink.EnsureLinkConnectorShape();
						}
					}
					if (null != toShape)
					{
						// Note that the from/to ordering reversal here is a hack so
						// the fact type shape folding code can find the opposite constraint
						// based on its center point. If both ends move the connection point,
						// then only the first one passed in here can find the opposite shape.
						// UNDONE: Slimy hack, should be removed if we get better framework support.
						// The order here needs to be in sync with the code in RemoveDanglingConstraintShape
						Connect(toShape, fromShape);
					}
				}
			}
		}
		/// <summary>
		/// Return the fact shape this link is attached to. Can return
		/// either a <see cref="FactTypeShape"/> or a <see cref="SubtypeLink"/>
		/// </summary>
		public ShapeElement AssociatedFactTypeShape
		{
			get
			{
				// The FromShape (as opposed to ToShape) here needs to be in
				// sync with the code in ConfiguringAsChildOf
				ShapeElement factShape = FromShape;
				LinkConnectorShape linkConnector;
				if (null != (linkConnector = factShape as LinkConnectorShape))
				{
					factShape = linkConnector.ParentShape;
				}
				return factShape;
			}
		}
		#endregion // ExternalConstraintLink specific
		#region Dangling constraint shape deletion
		/// <summary>
		/// External constraint shapes can only be drawn if they show all of their
		/// links, so automatically remove them if a connecting shape is removed.
		/// </summary>
		[RuleOn(typeof(ExternalConstraintLink))] // DeletingRule
		private sealed partial class DeleteDanglingConstraintShapeRule : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ExternalConstraintLink link = e.ModelElement as ExternalConstraintLink;
				ModelElement linkMel;
				ExternalConstraintShape shape;
				ModelElement shapeMel;
				// The ToShape (as opposed to FromShape) here needs to be in
				// sync with the code in ConfiguringAsChildOf
				if (null != (linkMel = link.ModelElement) &&
					!linkMel.IsDeleting &&
					null != (shape = link.ToShape as ExternalConstraintShape) &&
					null != (shapeMel = shape.ModelElement) &&
					!shapeMel.IsDeleting)
				{
					if (!shape.IsDeleting)
					{
						shape.Delete();
					}
					else
					{
						FactTypeShape factTypeShape;
						FactTypeLinkConnectorShape factTypeLinkConnector;
						NodeShape fromShape = link.FromShape;
						if ((null != (factTypeShape = fromShape as FactTypeShape) ||
							null != (factTypeShape = (null != (factTypeLinkConnector = fromShape as FactTypeLinkConnectorShape)) ? factTypeLinkConnector.ParentShape as FactTypeShape : null)))
						{
							SizeD oldSize = factTypeShape.Size;
							factTypeShape.AutoResize();
							if (oldSize == factTypeShape.Size)
							{
								factTypeShape.InvalidateRequired(true);
							}
						}
					}
				}
			}
		}
		#endregion // Dangling constraint shape deletion
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.ExternalConstraintLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.ExternalConstraintLinkAccessibleDescription;
			}
		}
		#endregion // Accessibility Properties
	}
}
