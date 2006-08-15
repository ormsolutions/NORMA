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
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMBaseBinaryLinkShape
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
		/// Abstract method to configure this link after it has been added to
		/// the diagram.
		/// </summary>
		public abstract void ConfiguringAsChildOf(ORMDiagram diagram, bool createdDuringViewFixup);
		#endregion Customize appearance
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
			LinkedElementCollection<ShapeElement> childShapes = RelativeChildShapes;
			foreach (ShapeElement shape in childShapes)
			{
				retVal = shape as LinkConnectorShape;
				if (retVal != null)
				{
					return retVal;
				}
			}
			Store store = this.Store;
			retVal = new LinkConnectorShape(store);
			RectangleD bounds = AbsoluteBoundingBox;
			childShapes.Add(retVal);
			retVal.Location = new PointD(bounds.Width / 2, bounds.Height / 2);
			return retVal;
		}

		#region LinkChangeRule class
		/// <summary>
		/// Keep relative child elements a fixed distance away from the fact
		/// when the shape changes.
		/// </summary>
		[RuleOn(typeof(ORMBaseBinaryLinkShape), FireTime = TimeToFire.LocalCommit, Priority = DiagramFixupConstants.AutoLayoutShapesRulePriority)] // ChangeRule
		private sealed partial class LinkChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
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
		}
		#endregion // LinkChangeRule class
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
		/// <summary>
		/// MSBUG: Call PurgeLayoutObjects before the shape deletion processing gets too serious
		/// </summary>
		protected override void OnDeleting()
		{
			base.OnDeleting();
			PurgeLayoutObjects();
		}
		#endregion // Accessibility Properties
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
		/// MSBUG: Call PurgeLayoutObjects before the shape deletion processing gets too serious
		/// </summary>
		protected override void OnDeleting()
		{
			base.OnDeleting();
			PurgeLayoutObjects();
		}
	}
	#endregion // LinkConnectorShape class
}
