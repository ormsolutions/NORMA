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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Neumont.Tools.ORM.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ShapeModel
{
	[TypeDescriptionProvider(typeof(Design.ORMPresentationTypeDescriptionProvider<ORMBaseBinaryLinkShape, ElementLink, ORMPresentationElementTypeDescriptor<ORMBaseBinaryLinkShape, ElementLink>>))]
	public partial class ORMBaseBinaryLinkShape
	{
		#region GetVGEdge method
		private delegate VGEdge GetVGEdgeDelegate(LinkShape @this);
		private static readonly GetVGEdgeDelegate GetVGEdgeInternal = InitializeGetVGEdgeInternal();
		private static GetVGEdgeDelegate InitializeGetVGEdgeInternal()
		{
			Type linkShapeType = typeof(LinkShape);
			MethodInfo graphEdgeMethodInfo = linkShapeType.GetMethod("GraphEdge", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding);
			if (graphEdgeMethodInfo == null)
			{
				return null;
			}
			Type graphEdgeType = linkShapeType.Assembly.GetType(linkShapeType.Namespace + Type.Delimiter + "GraphEdge", false, false);
			if (graphEdgeType == null)
			{
				return null;
			}
			PropertyInfo edgePropertyInfo = graphEdgeType.GetProperty("Edge", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding);
			if (edgePropertyInfo == null)
			{
				return null;
			}
			MethodInfo edgePropertyGetMethod = edgePropertyInfo.GetGetMethod(true);
			if (edgePropertyGetMethod == null)
			{
				return null;
			}

			DynamicMethod dynamicMethod = new DynamicMethod("GetVGEdge", typeof(VGEdge), new Type[] { linkShapeType }, linkShapeType, true);
			// ILGenerator tends to be rather aggressive with capacity checks, so we'll ask for a bit more than we really need
			// (which is 18 bytes) in order to avoid it resizing its buffer towards the end when it looks like we're running low
			ILGenerator ilGenerator = dynamicMethod.GetILGenerator(24);
			Label returnNullLabel = ilGenerator.DefineLabel();
			ilGenerator.Emit(OpCodes.Ldarg_0);
			ilGenerator.EmitCall(OpCodes.Call, graphEdgeMethodInfo, null);
			ilGenerator.Emit(OpCodes.Dup);
			ilGenerator.Emit(OpCodes.Brfalse_S, returnNullLabel);
			ilGenerator.EmitCall(OpCodes.Call, edgePropertyGetMethod, null);
			ilGenerator.Emit(OpCodes.Ret);
			ilGenerator.MarkLabel(returnNullLabel);
			ilGenerator.Emit(OpCodes.Pop);
			ilGenerator.Emit(OpCodes.Ldnull);
			ilGenerator.Emit(OpCodes.Ret);

			return (GetVGEdgeDelegate)dynamicMethod.CreateDelegate(typeof(GetVGEdgeDelegate));
		}
		private VGEdge GetVGEdge()
		{
			GetVGEdgeDelegate getVGEdgeInternal = GetVGEdgeInternal;
			// If GetVGEdgeInternal is null, the internals of one of the DSL Tools assemblies may have changed.
			// If that is the case, GetVGEdgeInternal needs to be updated...
			System.Diagnostics.Debug.Assert(getVGEdgeInternal != null);
			return (getVGEdgeInternal != null) ? getVGEdgeInternal(this) : null;
		}
		#endregion // GetVGEdge method

		#region Customize appearance
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
		/// Calls <see cref="InitializeLineRouting()"/>.
		/// </summary>
		public override void OnInitialize()
		{
			base.OnInitialize();
			this.InitializeLineRouting();
		}
		/// <summary>
		/// Calls <see cref="InitializeLineRouting()"/>.
		/// </summary>
		public override void OnShapeInserted()
		{
			base.OnShapeInserted();
			this.InitializeLineRouting();
		}
		/// <summary>
		/// Initializes line routing with correct settings.
		/// </summary>
		public void InitializeLineRouting()
		{
			VGEdge vgEdge = GetVGEdge();
			if (vgEdge != null)
			{
				// ORM lines cross, they don't jump.
				vgEdge.RouteJumpType = (int)VGObjectLineJumpCode.VGObjectJumpCodeNever;
				vgEdge.RoutingStyle = VGRoutingStyle.VGRouteCenterToCenter;

				// This call will be ignored if we're not in a transaction, but that's OK...
				base.RecalculateRoute();
			}
		}
		/// <summary>
		/// Abstract method to configure this link after it has been added to
		/// the diagram.
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public abstract void ConfiguringAsChildOf(ORMDiagram diagram);
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
				ORMTypeDescriptor.CreateNamePropertyDescriptor(targetElement),
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
		[RuleOn(typeof(ORMBaseBinaryLinkShape), FireTime = TimeToFire.LocalCommit, Priority = DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class LinkChangeRule : ChangeRule
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
		/// Link connector shapes cannot be moved
		/// </summary>
		public override bool CanMove
		{
			get
			{
				return false;
			}
		}
	}
	#endregion // LinkConnectorShape class
}
