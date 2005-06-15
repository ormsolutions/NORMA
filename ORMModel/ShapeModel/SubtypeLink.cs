using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class SubtypeLink
	{
		#region Customize appearance
		/// <summary>
		/// Draw an arrow on the subtype end
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				return LinkDecorator.DecoratorEmptyArrow;
			}
			set
			{
			}
		}
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.8F / 72.0F; // 1.8 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
		}
		/// <summary>
		/// Use a straight line routing style
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
		#endregion // Customize appearance
		#region SubtypeLink specific
		/// <summary>
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public SubtypeFact AssociatedSubtypeFact
		{
			get
			{
				return ModelElement as SubtypeFact;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public void ConfiguringAsChildOf(ORMDiagram diagram)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				SubtypeFact subtypeFact = AssociatedSubtypeFact;
				ObjectType subType = subtypeFact.Subtype;
				ObjectType superType = subtypeFact.Supertype;
				FactType nestedSubFact = subType.NestedFactType;
				FactType nestedSuperFact = superType.NestedFactType;
				NodeShape fromShape;
				NodeShape toShape;
				if (null != (toShape = diagram.FindShapeForElement((nestedSuperFact == null) ? superType as ModelElement : nestedSuperFact) as NodeShape) &&
					null != (fromShape = diagram.FindShapeForElement((nestedSubFact == null) ? subType as ModelElement : nestedSubFact) as NodeShape))
				{
					Connect(fromShape, toShape);
				}
			}
		}
		#endregion // SubtypeLink specific
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
	}
	public partial class ORMDiagram
	{
		#region  DisplaySubtypeLinkFixupListener
		/// <summary>
		/// A fixup class to display subtype links
		/// </summary>
		private class DisplaySubtypeLinkFixupListener : DeserializationFixupListener<ModelHasFactType>
		{
			/// <summary>
			/// Create a new DisplayRolePlayersFixupListener
			/// </summary>
			public DisplaySubtypeLinkFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitPresentationElements)
			{
			}
			/// <summary>
			/// Add subtype links when possible
			/// </summary>
			/// <param name="element">An ModelHasFactType instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ModelHasFactType element, Store store, INotifyElementAdded notifyAdded)
			{
				SubtypeFact subTypeFact = element.FactTypeCollection as SubtypeFact;
				if (subTypeFact != null)
				{
					FixUpDiagram(subTypeFact.Model, subTypeFact);
				}
			}
		}
		#endregion // DisplaySubtypeLinkFixupListener class
	}
}
