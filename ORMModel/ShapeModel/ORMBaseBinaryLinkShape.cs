using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMBaseBinaryLinkShape
	{
		#region Customize appearance
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
		/// Selecting links gets in the way of selecting roleboxes, etc.
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
		/// Stop the user from manually routing link lines
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
		/// Display the name of the underlying element as the
		/// component name in the property grid.
		/// </summary>
		public override string GetComponentName()
		{
			ModelElement element = ModelElement;
			return (element != null) ? element.GetComponentName() : base.GetComponentName();
		}
		/// <summary>
		/// Display the class of the underlying element as the
		/// component name in the property grid.
		/// </summary>
		public override string GetClassName()
		{
			if (Store.Disposed)
			{
				return GetType().Name;
			}
			ModelElement element = ModelElement;
			return (element != null) ? element.GetClassName() : base.GetClassName();
		}
		/// <summary>
		/// Abstract method to configure this link after it has been added to
		/// the diagram.
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public abstract void ConfiguringAsChildOf(ORMDiagram diagram);
		#endregion Customize appearance
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
		#region LinkConnectorShape management
		/// <summary>
		/// ORM diagrams need to connect links to other links, but this is
		/// not supported directly by the framework, so we create a dummy
		/// node shape that tracks the center of the link line and connect
		/// to the shape instead.
		/// </summary>
		/// <returns>LinkConnectorShape</returns>
		public LinkConnectorShape EnsureLinkConnectorShape()
		{
			LinkConnectorShape retVal = null;
			ShapeElementMoveableCollection childShapes = RelativeChildShapes;
			foreach (ShapeElement shape in childShapes)
			{
				retVal = shape as LinkConnectorShape;
				if (retVal != null)
				{
					return retVal;
				}
			}
			Store store = this.Store;
			retVal = LinkConnectorShape.CreateLinkConnectorShape(store);
			RectangleD bounds = AbsoluteBoundingBox;
			retVal.Location = new PointD(bounds.Width / 2, bounds.Height / 2);
			childShapes.Add(retVal);
			return retVal;
		}
		#region LinkChangeRule class
		/// <summary>
		/// Keep relative child elements a fixed distance away from the fact
		/// when the shape changes.
		/// </summary>
		[RuleOn(typeof(ORMBaseBinaryLinkShape), FireTime = TimeToFire.LocalCommit, Priority = DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private class LinkChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == ORMBaseBinaryLinkShape.EdgePointsMetaAttributeGuid)
				{
					ORMBaseBinaryLinkShape parentShape = e.ModelElement as ORMBaseBinaryLinkShape;
					ShapeElementMoveableCollection childShapes = parentShape.RelativeChildShapes;
					int childCount = childShapes.Count;
					for (int i = 0; i < childCount; ++i)
					{
						LinkConnectorShape linkConnector = childShapes[i] as LinkConnectorShape;
						if (linkConnector != null)
						{
							RectangleD bounds = parentShape.AbsoluteBoundingBox;
							linkConnector.Location = new PointD(bounds.Width / 2, bounds.Height / 2);
							IList links = linkConnector.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid);
							int linksCount = links.Count;
							for (int j = 0; j < linksCount; ++j)
							{
								LinkConnectsToNode link = links[j] as LinkConnectsToNode;
								BinaryLinkShape linkShape = link.Link as BinaryLinkShape;
								if (linkShape != null)
								{
									// Changing the location is not reliably reconnecting all shapes, especially
									// during load. Force the link to reconnect with a RipUp call
									linkShape.RipUp();
								}
								
							}
							break;
						}
					}
				}
			}
		}
		#endregion // LinkChangeRule class
		#endregion // LinkConnectorShape management
	}
	#region LinkConnectorShape class
	public partial class LinkConnectorShape
	{
		/// <summary>
		/// Link connector shapes are not selectable
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Link connector shapes should not be highlighted
		/// </summary>
		public override bool HasHighlighting
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Link connector shapes cannot be moved
		/// </summary>
		public override bool CanMove
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Dummy shapes have no size
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return SizeD.Empty;
			}
		}
	}
	#endregion // LinkConnectorShape class
}
