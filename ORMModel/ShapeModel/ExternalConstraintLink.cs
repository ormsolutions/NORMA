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
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Diagrams;
using Neumont.Tools.Modeling;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ExternalConstraintLink : IReconfigureableLink
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
			settings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
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
			Reconfigure(null);
		}
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected void Reconfigure(ShapeElement discludedShape)
		{
			IFactConstraint modelLink = ModelElement as IFactConstraint;
			//During the delete routine for an external constrant, 
			// the model element reference gets removed before checking ShouldVisitRelationship,
			// so we have to check the null case.
			if (modelLink != null)
			{
				MultiShapeUtility.ReconfigureLink(this, modelLink.FactType, modelLink.Constraint as ModelElement, discludedShape);
			}
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
#if LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		protected override Neumont.Tools.Modeling.Diagrams.BinaryLinkAnchor Anchor
		{
			get
			{
				return Neumont.Tools.Modeling.Diagrams.BinaryLinkAnchor.ToShape;
			}
		}
#endif //LINKS_ALWAYS_CONNECT
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
		/// DeletingRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
		/// External constraint shapes can only be drawn if they show all of their
		/// links, so automatically remove them if a connecting shape is removed.
		/// </summary>
		private static void DeleteDanglingConstraintShapeDeletingRule(ElementDeletingEventArgs e)
		{
			LinkConnectsToNode connectLink = e.ModelElement as LinkConnectsToNode;
			ExternalConstraintLink link = connectLink.Link as ExternalConstraintLink;
			ModelElement linkMel;
			ModelElement shapeMel;
			if (null != (link = connectLink.Link as ExternalConstraintLink) &&
				null != (linkMel = link.ModelElement) &&
				!linkMel.IsDeleting)
			{
				NodeShape linkNode = connectLink.Nodes;
				ExternalConstraintShape constraintShape = linkNode as ExternalConstraintShape;
				NodeShape oppositeShape = null;
				// The ToShape (as opposed to FromShape) here needs to be in
				// sync with the code in ConfiguringAsChildOf
				if (constraintShape == null)
				{
					constraintShape = link.ToShape as ExternalConstraintShape;
					oppositeShape = linkNode;
				}
				else
				{
					oppositeShape = link.FromShape;
				}
				if (oppositeShape != null &&
					constraintShape != null &&
					null != (shapeMel = constraintShape.ModelElement) &&
					!shapeMel.IsDeleting)
				{
					if (!constraintShape.IsDeleting)
					{
						FrameworkDomainModel.DelayValidateElement(constraintShape, DelayValidateExternalConstraintShapeFullyConnected);
					}
					else
					{
						FactTypeShape factTypeShape = MultiShapeUtility.ResolvePrimaryShape(oppositeShape) as FactTypeShape;
						if (factTypeShape != null)
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
		/// <summary>
		/// RolePlayerChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
		/// External constraint shapes can only be drawn if they show all of their
		/// links, so automatically remove them if a link is moved off the constraint
		/// shape.
		/// </summary>
		private static void DeleteDanglingConstraintShapeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			IElementDirectory directory;
			ShapeElement oldShape;
			if (e.DomainRole.Id == LinkConnectsToNode.NodesDomainRoleId &&
				(directory = e.ElementLink.Store.ElementDirectory).ContainsElement(e.OldRolePlayerId) &&
				null != (oldShape = directory.GetElement(e.OldRolePlayerId) as ShapeElement))
			{
				ExternalConstraintShape constraintShape;
				FactTypeShape factTypeShape;
				if (null != (constraintShape = oldShape as ExternalConstraintShape))
				{
					FrameworkDomainModel.DelayValidateElement(constraintShape, DelayValidateExternalConstraintShapeFullyConnected);
				}
				else if (null != (factTypeShape = MultiShapeUtility.ResolvePrimaryShape(oldShape) as FactTypeShape))
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
		/// <summary>
		/// AddRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority + 1;
		/// External constraint shapes can only be drawn if they show all of their
		/// links, so automatically remove them if multishape link handling never
		/// attaches the link after it is added.
		/// </summary>
		private static void DeleteDanglingConstraintShapeAddRule(ElementAddedEventArgs e)
		{
			DelayValidateExternalConstraintShapeFullyConnected(e.ModelElement);
		}
		private static void DelayValidateExternalConstraintShapeFullyConnected(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ExternalConstraintShape shape = (ExternalConstraintShape)element;
				int linkCount = 0;
				foreach (LinkConnectsToNode link in LinkConnectsToNode.GetLinksToLink(shape))
				{
					if (link.Link is ExternalConstraintLink)
					{
						++linkCount;
					}
				}
				bool keepShape = false;
				IConstraint constraint = shape.AssociatedConstraint;
				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SetConstraint:
						keepShape = ((SetConstraint)constraint).FactTypeCollection.Count == linkCount;
						break;
					case ConstraintStorageStyle.SetComparisonConstraint:
						keepShape = ((SetComparisonConstraint)constraint).FactTypeCollection.Count == linkCount;
						break;
				}
				if (!keepShape)
				{
					shape.Delete();
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
