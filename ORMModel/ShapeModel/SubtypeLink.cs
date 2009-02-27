#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class SubtypeLink : ORMBaseBinaryLinkShape, IModelErrorActivation, IProvideConnectorShape, IReconfigureableLink, IConfigureAsChildShape, IAutoCreatedSelectableShape, IDynamicColorGeometryHost
	{
		#region Customize appearance
		//The Resource ID's for the given subtype drawing type.
		/// <summary>
		/// resource id for pen to draw non primary subtype facts that are using the normal pen
		/// </summary>
		protected static readonly StyleSetResourceId NonPrimaryNormalResource = new StyleSetResourceId("Neumont", "NonPrimarySupertypeLinkNormalResource");
		/// <summary>
		/// resource id for pen to draw non primary subtype facts that are using the sticky pen
		/// </summary>
		protected static readonly StyleSetResourceId NonPrimaryStickyResource = new StyleSetResourceId("Neumont", "NonPrimarySupertypeLinkStickyResource");
		/// <summary>
		/// resource id for pen to draw non primary subtype facts that are using the active pen
		/// </summary>
		protected static readonly StyleSetResourceId NonPrimaryActiveResource = new StyleSetResourceId("Neumont", "NonPrimarySupertypeLinkActiveResource");
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService colorService = (this.Store as IORMToolServices).FontAndColorService;
			Color lineColor = colorService.GetForeColor(ORMDesignerColor.Constraint);
			Color stickyColor = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			Color activeColor = colorService.GetBackColor(ORMDesignerColor.RolePicker);
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.8F / 72.0F; // 1.8 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			//Supporting Dashed subtypefacts when not primary
			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(NonPrimaryNormalResource, DiagramPens.ConnectionLine, penSettings);
			penSettings.DashStyle = DashStyle.Solid;
			penSettings.Color = stickyColor;
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ConnectionLine, penSettings);

			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(NonPrimaryStickyResource, DiagramPens.ConnectionLine, penSettings);
			penSettings.DashStyle = DashStyle.Solid;
			penSettings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.ActiveBackgroundResource, DiagramPens.ConnectionLine, penSettings);

			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(NonPrimaryActiveResource, DiagramPens.ConnectionLine, penSettings);
			penSettings.DashStyle = DashStyle.Solid;

			penSettings = new PenSettings();
			penSettings.Width = 1.4F / 72.0F; // Soften the arrow a bit
			penSettings.Color = lineColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			penSettings.Color = stickyColor;
			classStyleSet.AddPen(ORMDiagram.StickyConnectionLineDecoratorResource, DiagramPens.ConnectionLineDecorator, penSettings);
			penSettings.Color = activeColor;
			classStyleSet.AddPen(ORMDiagram.ActiveConnectionLineDecoratorResource, DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = lineColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
			brushSettings.Color = stickyColor;
			classStyleSet.AddBrush(ORMDiagram.StickyConnectionLineDecoratorResource, DiagramBrushes.ConnectionLineDecorator, brushSettings);
			brushSettings.Color = activeColor;
			classStyleSet.AddBrush(ORMDiagram.ActiveConnectionLineDecoratorResource, DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// Specifies the three different color styles used to draw
		/// a subtype link
		/// </summary>
		private enum DrawColorStyle
		{
			/// <summary>
			/// Draw as a normal constraint
			/// </summary>
			Normal,
			/// <summary>
			/// Draw as an active part of a sticky object
			/// </summary>
			Sticky,
			/// <summary>
			/// Draw as a currently selected item in an active
			/// constraint editing operation
			/// </summary>
			Active,
		}
		private DrawColorStyle ColorStyle
		{
			get
			{
				ORMDiagram diagram = Diagram as ORMDiagram;
				ExternalConstraintConnectAction action = diagram.ExternalConstraintConnectAction;
				IConstraint testConstraint = action.ActiveConstraint;
				IList<Role> selectedRoles = null;
				if (testConstraint == null)
				{
					IStickyObject sticky = diagram.StickyObject;
					if (sticky != null)
					{
						ExternalConstraintShape shape = sticky as ExternalConstraintShape;
						if (shape != null)
						{
							testConstraint = shape.AssociatedConstraint;
						}
					}
				}
				else
				{
					selectedRoles = action.SelectedRoleCollection;
				}
				if (testConstraint != null)
				{
					SubtypeFact associatedSubtype = AssociatedSubtypeFact;
					if (null != selectedRoles && selectedRoles.Contains(associatedSubtype.SupertypeRole))
					{
						return DrawColorStyle.Active;
					}
					else
					{
						LinkedElementCollection<FactType> facts = null;
						switch (testConstraint.ConstraintStorageStyle)
						{
							case ConstraintStorageStyle.SetConstraint:
								facts = ((SetConstraint)testConstraint).FactTypeCollection;
								break;
							case ConstraintStorageStyle.SetComparisonConstraint:
								facts = ((SetComparisonConstraint)testConstraint).FactTypeCollection;
								break;
						}
						if (facts != null && facts.Contains(AssociatedSubtypeFact))
						{
							return DrawColorStyle.Sticky;
						}
					}
				}
				return DrawColorStyle.Normal;
			}
		}
		/// <summary>
		/// A filled arrow decorator drawn with sticky pens and brushes
		/// </summary>
		private sealed class StickyFilledArrowDecorator : DecoratorFilledArrow
		{
			public static readonly LinkDecorator Decorator = new StickyFilledArrowDecorator();
			public sealed override StyleSetResourceId BrushId
			{
				get
				{
					return ORMDiagram.StickyConnectionLineDecoratorResource;
				}
			}
			public sealed override StyleSetResourceId PenId
			{
				get
				{
					return ORMDiagram.StickyConnectionLineDecoratorResource;
				}
			}
		}
		/// <summary>
		/// A filled arrow decorator drawn with active pens and brushes
		/// </summary>
		private sealed class ActiveFilledArrowDecorator : DecoratorFilledArrow
		{
			public static readonly LinkDecorator Decorator = new ActiveFilledArrowDecorator();
			public sealed override StyleSetResourceId BrushId
			{
				get
				{
					return ORMDiagram.ActiveConnectionLineDecoratorResource;
				}
			}
			public sealed override StyleSetResourceId PenId
			{
				get
				{
					return ORMDiagram.ActiveConnectionLineDecoratorResource;
				}
			}
		}
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				DrawColorStyle style = ColorStyle;
				switch (style)
				{
					case DrawColorStyle.Sticky:
						return StickyFilledArrowDecorator.Decorator;
					case DrawColorStyle.Active:
						return ActiveFilledArrowDecorator.Decorator;
					default:
						Debug.Assert(style == DrawColorStyle.Normal);
						return DynamicColorDecoratorFilledArrow.Decorator;
				}
			}
			set
			{
			}
		}
		/// <summary>
		/// Change the connection line pen if the subtype is sticky or
		/// a selected role in an active constraint
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				SubtypeFact associatedFact = AssociatedSubtypeFact;
				if (associatedFact != null)
				{
					bool isPrimary = associatedFact.ProvidesPreferredIdentifier;
					DrawColorStyle style = ColorStyle;
					switch (style)
					{
						case DrawColorStyle.Sticky:
							if (isPrimary)
							{
								return ORMDiagram.StickyBackgroundResource;
							}
							else
							{
								return NonPrimaryStickyResource;
							}
						case DrawColorStyle.Active:
							if (isPrimary)
							{
								return ORMDiagram.ActiveBackgroundResource;
							}
							else
							{
								return NonPrimaryActiveResource;
							}
						default:
							Debug.Assert(style == DrawColorStyle.Normal);
							if (isPrimary)
							{
								return DiagramPens.ConnectionLine;
							}
							else
							{
								return NonPrimaryNormalResource;
							}
					}
				}
				else
				{
					return DiagramPens.ConnectionLine;
				}
			}
		}
		/// <summary>
		/// Subtype links need to be selectable to enable readings, etc
		/// </summary>
		public override bool CanSelect
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
		#endregion // Customize appearance
		#region Mouse handling
		/// <summary>
		/// Attempt model error activation
		/// </summary>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
			ORMBaseShape.AttemptErrorActivation(e);
			base.OnDoubleClick(e);
		}
		#endregion // Mouse handling
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>[] providers;
			if ((penId == DiagramPens.ConnectionLine ||
				penId == NonPrimaryNormalResource ||
				penId == DiagramPens.ConnectionLineDecorator) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>>()))
			{
				SubtypeFact element = (SubtypeFact)ModelElement;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.Constraint, this, element);
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
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>[] providers;
			if (brushId == DiagramBrushes.ConnectionLineDecorator &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, SubtypeLink, SubtypeFact>>()))
			{
				SubtypeFact element = (SubtypeFact)ModelElement;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.Constraint, this, element);
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
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			PopulationMandatoryError populationMandatoryError;
			if (null != (populationMandatoryError = error as PopulationMandatoryError))
			{
				ORMSamplePopulationToolWindow window = ORMDesignerPackage.SamplePopulationEditorWindow;
				window.AutoCorrectMandatoryError(populationMandatoryError);
				return true;
			}
			return false;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region SubtypeLink specific
		/// <summary>
		/// Get the <see cref="SubtypeFact"/> associated with this shape
		/// </summary>
		public SubtypeFact AssociatedSubtypeFact
		{
			get
			{
				return ModelElement as SubtypeFact;
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
			SubtypeFact subtypeFact = AssociatedSubtypeFact;
			ObjectType subType = subtypeFact.Subtype;
			ObjectType superType = subtypeFact.Supertype;
			FactType nestedSubFact = subType.NestedFactType;
			FactType nestedSuperFact = superType.NestedFactType;

			MultiShapeUtility.ReconfigureLink(this, (nestedSubFact == null) ? subType as ModelElement : nestedSubFact as ModelElement,
				(nestedSuperFact == null) ? superType as ModelElement : nestedSuperFact as ModelElement, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
		/// <summary>
		/// ORM diagrams need to connect links to other links, but this is
		/// not supported directly by the framework, so we create a dummy
		/// node shape that tracks the center of the link line and connect
		/// to the shape instead. The currently ORM diagram requirements
		/// are simple: we are only called with an opposite ExternalConstraintShape,
		/// and we can return the same shape for all requests.
		/// Implements <see cref="IProvideConnectorShape.GetUniqueConnectorShape"/>
		/// </summary>
		protected NodeShape GetUniqueConnectorShape(ShapeElement oppositeShape, ModelElement ignoreLinkShapesFor)
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
			retVal = new LinkConnectorShape(Partition);
			RectangleD bounds = AbsoluteBoundingBox;
			childShapes.Add(retVal);
			retVal.Location = new PointD(bounds.Width / 2, bounds.Height / 2);
			return retVal;
		}
		NodeShape IProvideConnectorShape.GetUniqueConnectorShape(ShapeElement oppositeShape, ModelElement ignoreLinkShapesFor)
		{
			return GetUniqueConnectorShape(oppositeShape, ignoreLinkShapesFor);
		}
#if LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		protected override BinaryLinkAnchor Anchor
		{
			get
			{
				return BinaryLinkAnchor.FromShape;
			}
		}
#endif //LINKS_ALWAYS_CONNECT
		#region HACK: Size property
		// UNDONE: 2006-06 DSL Tools port: SubtypeLink gets the generated code for a shape even though it is a link,
		// since links must be related to DomainRelationship elements, not DomainClass elements.
		// UNDONE: 2006-08 DSL Tools port: This is called from a different location now, so it is internal rather
		// than private.
		/// <summary>HACK: Pretend this property isn't here.</summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal SizeD Size
		{
			set
			{
			}
		}
		#endregion HACK: Size property
		#endregion // SubtypeLink specific
		#region Store Event Handlers
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="SubtypeLink"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			eventManager.AddOrRemoveHandler(store.DomainDataDirectory.FindDomainProperty(SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(ProvidesPreferredIdentifierChangedEvent), action);
		}
		/// <summary>
		/// Event handler for <see cref="SubtypeFact.ProvidesPreferredIdentifier"/> property on the associated subtype fact
		/// </summary>
		private static void ProvidesPreferredIdentifierChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			SubtypeFact fact;
			if (null != (fact = e.ModelElement as SubtypeFact))
			{
				if (!fact.IsDeleted)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(fact))
					{
						SubtypeLink linkShape;
						if (null != (linkShape = pel as SubtypeLink))
						{
							linkShape.Invalidate(true);
						}
					}
				}
			}
		}
		#endregion // Store Event Handlers
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.SubtypeLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.SubtypeLinkAccessibleDescription;
			}
		}
		#endregion // Accessibility Properties
	}
	public partial class ORMShapeDomainModel : ILayoutEngineProvider
	{
		#region  DisplaySubtypeLinkFixupListener
		/// <summary>
		/// A fixup class to display subtype links
		/// </summary>
		private sealed class DisplaySubtypeLinkFixupListener : DeserializationFixupListener<ModelHasFactType>
		{
			/// <summary>
			/// Create a new DisplaySubtypeLinkFixupListener
			/// </summary>
			public DisplaySubtypeLinkFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add subtype links when possible
			/// </summary>
			/// <param name="element">An ModelHasFactType instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ModelHasFactType element, Store store, INotifyElementAdded notifyAdded)
			{
				SubtypeFact subTypeFact = element.FactType as SubtypeFact;
				if (subTypeFact != null && !subTypeFact.IsDeleted)
				{
					ORMModel model = subTypeFact.Model;
					ObjectType rolePlayer = subTypeFact.Subtype;
					FactType nestedFact = rolePlayer.NestedFactType;
					if (FactTypeShape.ShouldDrawObjectification(nestedFact))
					{
						Diagram.FixUpDiagram(model, nestedFact);
						Diagram.FixUpDiagram(nestedFact, rolePlayer);
					}
					else
					{
						Diagram.FixUpDiagram(model, rolePlayer);
					}
					rolePlayer = subTypeFact.Supertype;
					nestedFact = rolePlayer.NestedFactType;
					if (FactTypeShape.ShouldDrawObjectification(nestedFact))
					{
						Diagram.FixUpDiagram(model, nestedFact);
						Diagram.FixUpDiagram(nestedFact, rolePlayer);
					}
					else
					{
						Diagram.FixUpDiagram(model, rolePlayer);
					}

					object AllowMultipleShapes;
					Dictionary<object, object> topLevelContextInfo;
					bool containedAllowMultipleShapes;
					if (!(containedAllowMultipleShapes = (topLevelContextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo).ContainsKey(AllowMultipleShapes = MultiShapeUtility.AllowMultipleShapes)))
					{
						topLevelContextInfo.Add(AllowMultipleShapes, null);
					}

					foreach (PresentationViewsSubject presentationViewsSubject in DomainRoleInfo.GetElementLinks<PresentationViewsSubject>(model, PresentationViewsSubject.SubjectDomainRoleId))
					{
						ORMDiagram diagram;
						if ((diagram = presentationViewsSubject.Presentation as ORMDiagram) != null)
						{
							//add a link shape for each fact type shape on the diagram for the played role
							foreach (ObjectTypeShape shapeElement in MultiShapeUtility.FindAllShapesForElement<ObjectTypeShape>(diagram, subTypeFact.Subtype))
							{
								diagram.FixUpLocalDiagram(subTypeFact);
							}
						}
					}

					if (!containedAllowMultipleShapes)
					{
						topLevelContextInfo.Remove(AllowMultipleShapes);
					}
				}
			}
		}
		#endregion // DisplaySubtypeLinkFixupListener class

		#region ILayoutEngineProvider
		/// <summary>
		/// Returns a list of layout engines provided by the default NORMA installation
		/// </summary>
		protected LayoutEngineData[] ProvideLayoutEngineData
		{
			get
			{
				LayoutEngineData[] retVal = {
					new LayoutEngineData(typeof(ORMRadialLayoutEngine), new ORMRadialLayoutEngine())
				};
				return retVal;
			}
		}
		LayoutEngineData[] ILayoutEngineProvider.ProvideLayoutEngineData()
		{
			return ProvideLayoutEngineData;
		}
		#endregion
	}
}
