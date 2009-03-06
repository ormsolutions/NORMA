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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ORMBaseBinaryLinkShape : IReconfigureableLink, IConfigureAsChildShape, IInvalidateDisplay
#if LINKS_ALWAYS_CONNECT
		, IBinaryLinkAnchor
		#endif //LINKS_ALWAYS_CONNECT
	{
		#region SubtypeLink Hack
		/// <summary>
		/// UNDONE: 2006-08 DSL Tools port: Hack for link-for-a-class
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool HasBackgroundGradient
		{
			get
			{
				return false;
			}
		}
		#endregion // SubtypeLink Hack
		#region MultipleShapesSupport
		#if LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		protected abstract BinaryLinkAnchor Anchor { get;}
		BinaryLinkAnchor IBinaryLinkAnchor.Anchor
		{
			get
			{
				return Anchor;
			}
		}
		#endif //LINKS_ALWAYS_CONNECT
		/// <summary>See <see cref="ShapeElement.FixUpChildShapes"/>.</summary>
		public override ShapeElement FixUpChildShapes(ModelElement childElement)
		{
			return MultiShapeUtility.FixUpChildShapes(this, childElement);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Debug.Fail("Classes derived from ORMBaseBinaryLinkShape must implement IReconfigurableLink.Reconfigure");
		}
		#endregion //MultipleShapesSupport
		#region Customize appearance
		/// <summary>
		/// Specify CenterToCenter routing style so we can
		/// locate our objects in DoFoldToShape
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
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			// ORM lines cross, they don't jump. However, the RouteJumpType cannot
			// be set before the diagram is in place, so this property cannot be set
			// from initialization code in the shape itself.
			RouteJumpType = VGObjectLineJumpCode.VGObjectJumpCodeNever;
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
		}

		#endregion Customize appearance
		#region Auto-invalidate tracking, IInvalidateRequired implementation
		/// <summary>
		/// Implements <see cref="IInvalidateDisplay.InvalidateRequired()"/>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		protected void InvalidateRequired()
		{
			InvalidateRequired(false);
		}
		void IInvalidateDisplay.InvalidateRequired()
		{
			InvalidateRequired();
		}
		/// <summary>
		/// Implements <see cref="IInvalidateDisplay.InvalidateRequired(bool)"/>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		/// <param name="refreshBitmap">Value to forward to the Invalidate method's refreshBitmap property during event playback</param>
		protected void InvalidateRequired(bool refreshBitmap)
		{
			long? newValue = ORMShapeDomainModel.GetNewUpdateCounterValue(this, refreshBitmap);
			if (newValue.HasValue)
			{
				UpdateCounter = newValue.Value;
			}
		}
		void IInvalidateDisplay.InvalidateRequired(bool refreshBitmap)
		{
			InvalidateRequired(refreshBitmap);
		}
		/// <summary>
		/// Called during event playback before an <see cref="ShapeElement.Invalidate()"/> call triggered
		/// via the <see cref="InvalidateRequired()"/> mechanism is called. The default implementation
		/// does nothing.
		/// </summary>
		protected virtual void BeforeInvalidate()
		{
		}
		private long GetUpdateCounterValue()
		{
			return ORMShapeDomainModel.GetCurrentUpdateCounterValue(this);
		}
		private void SetUpdateCounterValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log entry
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="ORMBaseShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ORMBaseBinaryLinkShape.UpdateCounterDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(UpdateRequiredEvent), action);
		}
		private static void UpdateRequiredEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ORMBaseBinaryLinkShape shape = (ORMBaseBinaryLinkShape)e.ModelElement;
			if (!shape.IsDeleted)
			{
				shape.BeforeInvalidate();
				shape.Invalidate(Math.Abs(unchecked((long)e.OldValue - (long)e.NewValue)) != 1L);
			}
		}
		#endregion // Auto-invalidate tracking, IInvalidateRequired implementation
		#region DuplicateNameError Activation Helper
		/// <summary>
		/// Activate the Name property in the Properties Window
		/// for the specified element
		/// </summary>
		/// <param name="targetElement">The underlying model element with a name property</param>
		protected void ActivateNameProperty(ORMNamedElement targetElement)
		{
			Store store = Store;
			EditorUtility.ActivatePropertyEditor(
				(store as IORMToolServices).ServiceProvider,
				DomainTypeDescriptor.CreateNamePropertyDescriptor(targetElement),
				false);
		}
		#endregion // DuplicateNameError Activation Helper
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
		#region LinkChangeRule
		/// <summary>
		/// ChangeRule: typeof(ORMBaseBinaryLinkShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// Keep relative child elements a fixed distance away from the fact
		/// when the shape changes.
		/// </summary>
		private static void LinkChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == ORMBaseBinaryLinkShape.EdgePointsDomainPropertyId)
			{
				ORMBaseBinaryLinkShape parentShape = e.ModelElement as ORMBaseBinaryLinkShape;
				LinkedElementCollection<ShapeElement> childShapes = parentShape.RelativeChildShapes;
				int childCount = childShapes.Count;
				for (int i = 0; i < childCount; ++i)
				{
					LinkConnectorShape linkConnector = childShapes[i] as LinkConnectorShape;
					if (linkConnector != null)
					{
						RectangleD bounds = parentShape.AbsoluteBoundingBox;
						linkConnector.Location = new PointD(bounds.Width / 2, bounds.Height / 2);
						ReadOnlyCollection<LinkConnectsToNode> links = DomainRoleInfo.GetElementLinks<LinkConnectsToNode>(linkConnector, LinkConnectsToNode.NodesDomainRoleId);
						int linksCount = links.Count;
						for (int j = 0; j < linksCount; ++j)
						{
							LinkConnectsToNode link = links[j];
							BinaryLinkShape linkShape = link.Link as BinaryLinkShape;
							if (linkShape != null)
							{
								// Changing the location is not reliably reconnecting all shapes, especially
								// during load. Force the link to reconnect with a RecalculateRoute call
								linkShape.RecalculateRoute();
							}

						}
						break;
					}
				}
			}
		}
		#endregion // LinkChangeRule
		#endregion // LinkConnectorShape management
		#region Accessibility Properties
		/// <summary>
		/// Return the localized <see cref="AccessibleValue"/>. Obtained by combining
		/// <see cref="FromAccessibleValue"/> with <see cref="ToAccessibleValue"/>.
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.DefaultLinkShapeAccessibleValueFormat, FromAccessibleValue, ToAccessibleValue);
			}
		}
		/// <summary>
		/// Combined with the <see cref="ToAccessibleValue"/> to form an AccessibleValue for
		/// the link. Defaults to the component name of the <see cref="ModelElement"/>
		/// associated with the <see cref="BinaryLinkShapeBase.FromShape"/>.
		/// </summary>
		protected virtual string FromAccessibleValue
		{
			get
			{
				return TypeDescriptor.GetComponentName(FromShape.ModelElement);
			}
		}
		/// <summary>
		/// Combined with the <see cref="FromAccessibleValue"/> to form an AccessibleValue for
		/// the link. Defaults to the component name of the <see cref="ModelElement"/>
		/// associated with the <see cref="BinaryLinkShapeBase.ToShape"/>.
		/// </summary>
		protected virtual string ToAccessibleValue
		{
			get
			{
				return TypeDescriptor.GetComponentName(ToShape.ModelElement);
			}
		}
		#endregion // Accessibility Properties
	}
	#region LinkConnectorShape class
	public partial class LinkConnectorShape : IProxyConnectorShape
	{
		#region IProxyConnectorShape implementation
		/// <summary>
		/// Implements IProxyConnectorShape.ProxyConnectorShapeFor
		/// </summary>
		protected ShapeElement ProxyConnectorShapeFor
		{
			get
			{
				return ParentShape;
			}
		}
		ShapeElement IProxyConnectorShape.ProxyConnectorShapeFor
		{
			get
			{
				return ProxyConnectorShapeFor;
			}
		}
		#endregion // IProxyConnectorShape implementation
		#region ClickThroughRectangleGeometry
		private class ClickThroughRectangleGeometry : RectangleShapeGeometry
		{
			#region Constructor and singleton
			/// <summary>
			/// Singleton ClickThroughRectangleGeometry instance
			/// </summary>
			public static readonly ShapeGeometry ShapeGeometry = new ClickThroughRectangleGeometry();
			/// <summary>
			/// Protected default constructor. The class should be used
			/// as a singleton instead of being publicly constructed.
			/// </summary>
			protected ClickThroughRectangleGeometry()
			{
			}
			#endregion // Constructor and singleton
			/// <summary>
			/// Make sure this shape never interferes with the mouse
			/// </summary>
			public override bool DoHitTest(IGeometryHost geometryHost, PointD hitPoint, DiagramHitTestInfo hitTestInfo, bool includeTolerance)
			{
				return false;
			}
		}
		#endregion // ClickThroughRectangleGeometry
		#region MultipleShapesSupport
		/// <summary>See <see cref="ShapeElement.FixUpChildShapes"/>.</summary>
		public override ShapeElement FixUpChildShapes(ModelElement childElement)
		{
			return MultiShapeUtility.FixUpChildShapes(this, childElement);
		}
		#endregion //MultipleShapesSupport
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
		/// Link connect shapes do not show a shadow
		/// </summary>
		/// <value>false</value>
		public override bool HasShadow
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
		/// Default to no sides being resizable.
		/// </summary>
		public override NodeSides ResizableSides
		{
			get
			{
				return NodeSides.None;
			}
		}
		/// <summary>
		/// Link connector shapes do not have an outline
		/// </summary>
		public override bool HasOutline
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// The default implementation picks up the diagram grid size
		/// as the minimum, which makes this visible. We don't want it visible.
		/// </summary>
		public override SizeD MinimumSize
		{
			get
			{
				return SizeD.Empty;
			}
		}
		/// <summary>
		/// Make sure this shape does not interfere with the mouse
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return ClickThroughRectangleGeometry.ShapeGeometry;
			}
		}
	}
	#endregion // LinkConnectorShape class
}
