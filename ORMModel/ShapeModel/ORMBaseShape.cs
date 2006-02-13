using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ORMBaseShape
	{
		#region Virtual extensions
		/// <summary>
		/// Called during the OnChildConfiguring from the parent shape.
		/// The default implementation does nothing.
		/// </summary>
		/// <param name="parent">The parent shape. May be a diagram.</param>
		public virtual void ConfiguringAsChildOf(NodeShape parent)
		{
		}
		/// <summary>
		/// Override to modify the content size of an item.
		/// By default, the AutoResize function will use this
		/// size (if it is set) with no margins.
		/// </summary>
		protected virtual SizeD ContentSize
		{
			get
			{
				return SizeD.Empty;
			}
		}
		/// <summary>
		/// Sizes to object to the size of the contents.
		/// The default implementation uses the ContentSize
		/// if it is set and does not do any margin adjustments.
		/// </summary>
		public virtual void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty)
			{
				Size = contentSize;
			}
		}
		#endregion // Virtual extensions
		#region Customize appearance
		/// <summary>
		/// Determines if a <see cref="ShapeElement"/> should be drawn with a shadow because it
		/// appears in more than one place in a model.
		/// </summary>
		/// <seealso cref="ObjectTypeShape.HasShadow"/>
		/// <seealso cref="FactTypeShape.HasShadow"/>
		public static bool ShouldHaveShadow(ShapeElement shapeElement)
		{
			ModelElement modelElement = shapeElement.ModelElement;
			if (modelElement != null)
			{
				PresentationElementMoveableCollection presentationElements = modelElement.PresentationRolePlayers;
				if (presentationElements.Count > 1)
				{
					Type thisType = shapeElement.GetType();
					for (int i = 0; i < presentationElements.Count; i++)
					{
						PresentationElement presentationElement = presentationElements[i];
						if (shapeElement != presentationElement && thisType.Equals(presentationElement.GetType()))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Turn off the shadow by default
		/// </summary>
		/// <value>false</value>
		public override bool HasShadow
		{
			get { return false; }
		}
		/// <summary>
		/// Size the object appropriately
		/// </summary>
		public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration)
		{
			base.OnBoundsFixup(fixupState, iteration);
			if (fixupState == BoundsFixupState.ViewFixup)
			{
				AutoResize();
			}
		}
		/// <summary>
		/// Make sure the shape fields are available very early. This is
		/// needed during deserialization as well as initial creation, so
		/// it is placed in OnCreated instead of OnInitialized.
		/// </summary>
		public override void OnCreated()
		{
			base.OnCreated();
			// Force early initialization of shape fields so auto sizing based on
			// content always works
			ShapeFieldCollection shapes = ShapeFields;
		}
		/// <summary>
		/// Do early initialization so sizing mechanisms work correctly
		/// </summary>
		public override void OnInitialized()
		{
			base.OnInitialized();
			SizeD defSize = DefaultSize;
			AbsoluteBounds = new RectangleD(0.0, 0.0, defSize.Width, defSize.Height);
		}
		/// <summary>
		/// Defer to ConfiguringAsChildOf for ORMBaseShape children
		/// </summary>
		/// <param name="child">The child being configured</param>
		protected override void OnChildConfiguring(ShapeElement child)
		{
			ORMBaseShape baseShape;
			if (null != (baseShape = child as ORMBaseShape))
			{
				baseShape.ConfiguringAsChildOf(this);
			}
		}
		#endregion // Customize appearance
		#region Customize property display
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
		#endregion // Customize property display
		#region Accessibility Properties
		/// <summary>
		/// Return the class name as the accessible name
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return GetClassName();
			}
		}
		/// <summary>
		/// Return the component name as the accessible value
		/// </summary>
		public override string AccessibleValue
		{
			get
			{
				return GetComponentName();
			}
		}
		#endregion // Accessibility Properties
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
}