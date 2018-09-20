#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ExternalConstraintLink : IReconfigureableLink, IConfigureAsChildShape, IDynamicColorGeometryHost
	{
		#region TargetArrowDecorator class
		/// <summary>
		/// The link decorator used to draw the arrow head
		/// for subset and value comparison constraints.
		/// </summary>
		protected class TargetArrowDecorator : DynamicColorDecoratorFilledArrow, ILinkDecoratorSettings
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public new static readonly LinkDecorator Decorator = new TargetArrowDecorator();
			/// <summary>
			/// Instantiates a new TargetArrowDecorator
			/// </summary>
			protected TargetArrowDecorator()
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
		#endregion // TargetArrowDecorator class
		#region TargetArrowStickyDecorator class
		/// <summary>
		/// A TargetArrowDecorator to be used when the connector is currently selected (i.e. is sticky)
		/// </summary>
		protected class TargetArrowStickyDecorator : TargetArrowDecorator
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static new readonly LinkDecorator Decorator = new TargetArrowStickyDecorator();
			/// <summary>
			/// Instantiates a new TargetArrowStickyDecorator
			/// </summary>
			protected TargetArrowStickyDecorator() { }
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
		#endregion // TargetArrowStickyDecorator class
		#region Customize appearance
#if VISUALSTUDIO_10_0
		// Based on reflecting the code, VS2010 attempts to 'fix' the GDI+ rendering
		// bug with dotted lines (uninitialized data in the metafile results in blobs or
		// crashes on some platforms and printers) by not rendering non-solid lines to the
		// metafile. This is done with the Microsoft.VisualStudio.Modeling.Diagrams.Diagram.MetafileCreationContext
		// class, which modifies ShapeOutline and ConnectionLine pens before metafile rendering.
		// This is a lame approach because it isn't comprehensive (non-solid lines can be attached
		// to other pen identifiers or simply drawn with a temporarily modified pen). The only ways
		// to fix the issue are to either fix GDI+ or create a meta-file iterator that cleans up the
		// garbage data in the metafile stream. Constraint lines are the only NORMA pens affected by this
		// 'fix', so we simply give ours a different resource identifier to avoid the problem altogether.
		/// <summary>
		/// Pen to draw dotted connection lines in VS2010
		/// </summary>
		private static readonly StyleSetResourceId CustomConnectionLinePen = new StyleSetResourceId("ORMArchitect", "CustomConnectionLine");
#endif // VISUALSTUDIO_10_0
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
#if VISUALSTUDIO_10_0
			classStyleSet.AddPen(CustomConnectionLinePen, DiagramPens.ConnectionLine, settings);
#else
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, settings);
#endif
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
#if VISUALSTUDIO_10_0
					retVal.OverridePen(CustomConnectionLinePen, penSettings);
#else
					retVal.OverridePen(DiagramPens.ConnectionLine, penSettings);
#endif
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
				IConstraint constraint;
				return (null != (constraint = AssociatedConstraint) && constraint.Modality == ConstraintModality.Deontic) ?
					// Note that we don't do anything with fonts with this style set, so the
					// static one is sufficient. Instance style sets also go through a font initialization
					// step inside the framework.
					DeonticClassStyleSet :
					base.StyleSet;
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
#if VISUALSTUDIO_10_0
				return CustomConnectionLinePen;
#else
				return DiagramPens.ConnectionLine;
#endif
			}
		}
		private bool IsSticky()
		{
			PresentationElement stickyPel;
			IConstraint constraint;
			return (null != (stickyPel = ((ORMDiagram)Diagram).StickyObject as PresentationElement) &&
				null != (constraint = AssociatedConstraint) &&
				stickyPel.ModelElement == constraint);
		}
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorFrom
		{
			get
			{
				ModelElement element = ModelElement;
				FactSetComparisonConstraint factConstraint;
				ConstraintRoleSequenceHasRole constraintRole;
				bool showArrow = false;
				//Only change the decorator if the ExternalConstraintLink being worked on
				//is for the second constraint role sequence since we need the arrow to make
				//it look like the constraint links point from the role of the first sequence
				//to the role of the second sequence.
				if (null != (factConstraint = element as FactSetComparisonConstraint))
				{
					SubsetConstraint subsetConstraint;
					if (null != (subsetConstraint = factConstraint.Constraint as SubsetConstraint))
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
						if (factType != null)
						{
							LinkedElementCollection<RoleBase> factTypeRoles = factType.RoleCollection;
							LinkedElementCollection<SetComparisonConstraintRoleSequence> sequenceCollection = subsetConstraint.RoleSequenceCollection;
							if (sequenceCollection.Count > 1)
							{
								foreach (Role r in sequenceCollection[1].RoleCollection)
								{
									if (factTypeRoles.Contains(r))
									{
										showArrow = true;
										break;
									}
								}
							}
						}
					}
				}
				else if (null != (constraintRole = element as ConstraintRoleSequenceHasRole))
				{
					ValueComparisonConstraint comparisonConstraint;
					showArrow = null != (comparisonConstraint = constraintRole.ConstraintRoleSequence as ValueComparisonConstraint) &&
						comparisonConstraint.IsDirectional &&
						1 == comparisonConstraint.RoleCollection.IndexOf(constraintRole.Role);
				}
				return showArrow ?
					(IsSticky() ? TargetArrowStickyDecorator.Decorator : TargetArrowDecorator.Decorator) :
					base.DecoratorFrom;
			}
			set
			{
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
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ExternalConstraintLink, IConstraint>[] providers;
			IConstraint constraint;
			Store store;
			if ((penId ==
#if VISUALSTUDIO_10_0
				CustomConnectionLinePen ||
#else
				DiagramPens.ConnectionLine ||
#endif
				penId == DiagramPens.ConnectionLineDecorator) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (constraint = AssociatedConstraint) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ExternalConstraintLink, IConstraint>>(true)))
			{
				ORMDiagramDynamicColor requestColor = constraint.Modality == ConstraintModality.Deontic ? ORMDiagramDynamicColor.DeonticConstraint : ORMDiagramDynamicColor.Constraint;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(requestColor, this, constraint);
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
		protected Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			Color retVal = Color.Empty;
			SolidBrush solidBrush;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ExternalConstraintLink, IConstraint>[] providers;
			IConstraint constraint;
			Store store;
			if (brushId == DiagramBrushes.ConnectionLineDecorator &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (constraint = AssociatedConstraint) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ExternalConstraintLink, IConstraint>>(true)))
			{
				ORMDiagramDynamicColor requestColor = constraint.Modality == ConstraintModality.Deontic ? ORMDiagramDynamicColor.DeonticConstraint : ORMDiagramDynamicColor.Constraint;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(requestColor, this, constraint);
					if (alternateColor != Color.Empty)
					{
						retVal = solidBrush.Color;
						solidBrush.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return UpdateDynamicColor(brushId, brush);
		}
		#endregion // IDynamicColorGeometryHost Implementation
		#region ExternalConstraintLink specific
		/// <summary>
		/// Get the <see cref="FactConstraint"/> link associated with this
		/// link shape. The fact constraint link can be used to get the
		/// associated roles. Use <see cref="AssociatedConstraintRole"/>
		/// if the constraint connects directly to individual roles.
		/// </summary>
		public FactConstraint AssociatedFactConstraint
		{
			get
			{
				return ModelElement as FactConstraint;
			}
		}
		/// <summary>
		/// Get the <see cref="ConstraintRoleSequenceHasRole"/> associated
		/// with this link shape. This is set only for constraints that
		/// connect to individual roles instead of fact types.
		/// </summary>
		public ConstraintRoleSequenceHasRole AssociatedConstraintRole
		{
			get
			{
				return ModelElement as ConstraintRoleSequenceHasRole;
			}
		}
		/// <summary>
		/// Get the constraint associated with this link.
		/// </summary>
		public IConstraint AssociatedConstraint
		{
			get
			{
				IConstraint constraint = null;
				IFactConstraint factConstraint;
				ConstraintRoleSequenceHasRole constraintRole;
				ModelElement element = ModelElement;
				if (null != (element = ModelElement))
				{
					if (null != (factConstraint = element as IFactConstraint))
					{
						constraint = factConstraint.Constraint;
					}
					else if (null != (constraintRole = element as ConstraintRoleSequenceHasRole))
					{
						constraint = constraintRole.ConstraintRoleSequence as IConstraint;
					}
				}
				return constraint;
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
			ModelElement element = ModelElement;
			// During the delete process for an external constrant, 
			// the model element reference gets removed before checking ShouldVisitRelationship,
			// so we have to check the null case.
			if (element != null)
			{
				IFactConstraint factConstraint;
				ConstraintRoleSequenceHasRole constraintRole;
				FactType factType;
				if (null != (factConstraint = element as IFactConstraint))
				{
					MultiShapeUtility.ReconfigureLink(this, factConstraint.FactType, factConstraint.Constraint as ModelElement, discludedShape);
				}
				else if (null != (constraintRole = element as ConstraintRoleSequenceHasRole) &&
					null != (factType = constraintRole.Role.FactType))
				{
					MultiShapeUtility.ReconfigureLink(this, factType, constraintRole.ConstraintRoleSequence, discludedShape);
				}
			}
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
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
		#region Dangling constraint shape deletion and fact type shape role bar resizing
		/// <summary>
		/// AddRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
		/// Make sure connected fact type shapes properly display their role bars
		/// </summary>
		private static void VerifyedConnectedShapeAddedRule(ElementAddedEventArgs e)
		{
			LinkConnectsToNode connectLink = (LinkConnectsToNode)e.ModelElement;
			FactTypeShape factTypeShape;
			if (connectLink.Link is ExternalConstraintLink &&
				null != (factTypeShape = connectLink.Nodes as FactTypeShape))
			{
				FrameworkDomainModel.DelayValidateElement(factTypeShape, DelayValidateFactTypeShapeSize);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
		/// External constraint shapes can only be drawn if they show all of their
		/// links, so automatically remove them if a connecting shape is removed.
		/// </summary>
		private static void VerifyConnectedShapeShapeDeletingRule(ElementDeletingEventArgs e)
		{
			LinkConnectsToNode connectLink = (LinkConnectsToNode)e.ModelElement;
			ExternalConstraintLink link;
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
					// Delay, fact type shape size will not be accurate until deletion is completed.
					FactTypeShape factTypeShape = MultiShapeUtility.ResolvePrimaryShape(oppositeShape) as FactTypeShape;
					if (factTypeShape != null &&
						!factTypeShape.IsDeleting)
					{
						FrameworkDomainModel.DelayValidateElement(factTypeShape, DelayValidateFactTypeShapeSize);
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
		private static void VerifyConnectedShapeShapeRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ShapeElement shape;
			if (e.DomainRole.Id == LinkConnectsToNode.NodesDomainRoleId &&
				((LinkConnectsToNode)e.ElementLink).Link is ExternalConstraintLink &&
				null != (shape = e.OldRolePlayer as ShapeElement))
			{
				ExternalConstraintShape constraintShape;
				FactTypeShape factTypeShape;
				if (null != (constraintShape = shape as ExternalConstraintShape))
				{
					FrameworkDomainModel.DelayValidateElement(constraintShape, DelayValidateExternalConstraintShapeFullyConnected);
				}
				else if (null != (factTypeShape = MultiShapeUtility.ResolvePrimaryShape(shape) as FactTypeShape))
				{
					FrameworkDomainModel.DelayValidateElement(factTypeShape, DelayValidateFactTypeShapeSize);
					if (null != (shape = e.NewRolePlayer as ShapeElement) &&
						null != (factTypeShape = MultiShapeUtility.ResolvePrimaryShape(shape) as FactTypeShape))
					{
						FrameworkDomainModel.DelayValidateElement(factTypeShape, DelayValidateFactTypeShapeSize);
					}
				}
			}
		}
		private static void DelayValidateFactTypeShapeSize(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FactTypeShape factTypeShape = (FactTypeShape)element;
				SizeD oldSize = factTypeShape.Size;
				factTypeShape.AutoResize();
				if (oldSize == factTypeShape.Size)
				{
					((IInvalidateDisplay)factTypeShape).InvalidateRequired(true);
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
		#endregion // Dangling constraint shape deletion and fact type shape role bar resizing
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
