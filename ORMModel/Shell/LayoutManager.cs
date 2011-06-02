using System;
using System.Collections.Generic;
using System.Text;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/*
	 * Quick guide to layout files:
	 * 
	 *		LayoutManager.cs - use the LayoutManager class to lay shapes out on a diagram
	 *		LayoutEngines.cs - add/edit code for layout engines in here
	 */

	#region LayoutManager
	/// <summary>
	/// The entry point to laying out shapes on an instance of an <seealso cref="ORMDiagram"/>.
	/// NodeShapes are added to the layout manager, after which the user calls LayoutManager.Layout.
	/// </summary>
	public class LayoutManager
	{
		/// <summary>
		/// List of <see cref="LayoutShape"/> elements to place on the diagram.
		/// </summary>
		private LayoutShapeList myLayoutShapes = new LayoutShapeList();
		/// <summary>
		/// The diagram hosting the shapes.
		/// </summary>
		private ORMDiagram myDiagram;
		/// <summary>
		/// The <seealso cref="LayoutEngine"/> instance to be used to be lay out the shapes.
		/// </summary>
		private LayoutEngine myLayoutEngine;
		/// <summary>
		/// The element from which all other elements are laid out.
		/// </summary>
		private NodeShape myRootShape = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LayoutManager(ORMDiagram diagram, LayoutEngine engine)
		{
			myDiagram = diagram;
			myLayoutEngine = engine;
		}

		/// <summary>
		/// Adds the <paramref name="shape"/> with a pin value of false.
		/// </summary>
		/// <param name="shape">The shape to lay out on the diagram.</param>
		public void AddShape(ShapeElement shape)
		{
			AddShape(shape, false);
		}

		/// <summary>
		/// Adds the <paramref name="shape"/> with the specified <paramref name="pinned"/> value.
		/// </summary>
		/// <param name="shape">The shape to lay out on the diagram.</param>
		/// <param name="pinned">Indicates whether the shape is pinned in place or not.  True means the shape is pinned to its current location.</param>
		public void AddShape(ShapeElement shape, bool pinned)
		{
			NodeShape ns = shape as NodeShape;
			if (ns == null || shape.ParentShape as ORMDiagram == null)
			{
				return;
			}

			LayoutShapeList list = myLayoutShapes;
			LayoutShape layshape;

			// If the shape doesn't exist, add it, otherwise simply modify the pinned value.
			if (!list.TryGetValue(ns, out layshape))
			{
				layshape = new LayoutShape(ns, pinned);
				list.Add(layshape);
			}
			else
			{
				layshape.Pinned = pinned;
			}
		}

		/// <summary>
		/// Adds a list of <paramref name="shapes"/> with a pin value of false.
		/// </summary>
		/// <param name="shapes">The shapes to lay out on the diagram.</param>
		public void AddShapes(List<ShapeElement> shapes)
		{
			AddShapes(shapes, false);
		}

		/// <summary>
		/// Adds the <paramref name="shapes"/> with the specified <paramref name="pinned"/> value.
		/// </summary>
		/// <param name="shapes">The shapes to lay out on the diagram.</param>
		/// <param name="pinned">Indicates whether the shapes are pinned in place or not.  True means the shapes are pinned to their current location.</param>
		public void AddShapes(List<ShapeElement> shapes, bool pinned)
		{
			foreach (ShapeElement shape in shapes)
			{
				AddShape(shape, pinned);
			}
		}
		/// <summary>
		/// Organizes the shapes added with <see cref="AddShape(ShapeElement)"/> or similar methods.
		/// </summary>
		/// <param name="ignoreExistingShapes">Do not adjust for existing shapes</param>
		public void Layout(bool ignoreExistingShapes)
		{
			Layout(ignoreExistingShapes, null, false, false);
		}
		/// <summary>
		/// Organizes the shapes added with <see cref="AddShape(ShapeElement)"/> or similar methods.
		/// </summary>
		/// <param name="ignoreExistingShapes">Do not adjust for existing shapes</param>
		/// <param name="referenceShape">Layout shapes to the right of this shape.</param>
		/// <param name="layoutRightOfReferenceShape">Set to layout to the right of the <paramref name="referenceShape"/></param>
		/// <param name="layoutBelowReferenceShape">Set to layout below the <paramref name="referenceShape"/></param>
		public void Layout(bool ignoreExistingShapes, NodeShape referenceShape, bool layoutRightOfReferenceShape, bool layoutBelowReferenceShape)
		{
			LayoutShape backupRoot = null;
			LayoutShapeList allShapes = myLayoutShapes;
			switch (allShapes.Count)
			{
				case 0:
					return;
				default:
					backupRoot = allShapes[0];
					break;
			}
			myLayoutEngine.LateBind(myDiagram, allShapes);

			SizeD margin = myDiagram.NestedShapesMargin;
			if (referenceShape != null &&
				(layoutRightOfReferenceShape || layoutBelowReferenceShape))
			{
				RectangleD referenceBounds = referenceShape.AbsoluteBounds;
				if (layoutRightOfReferenceShape)
				{
					margin.Width = Math.Max(margin.Width, referenceBounds.Right);
				}
				if (layoutBelowReferenceShape)
				{
					margin.Height = Math.Max(margin.Height, referenceBounds.Bottom);
				}
			}
			PointD minimumPoint = new PointD(margin.Width, margin.Height);
			RectangleD layoutRectangle = new RectangleD(minimumPoint, SizeD.Empty);
			bool firstPass = true;
			for (; ; )
			{
				LayoutShape mostRelatives = myLayoutEngine.ResolveReferences(allShapes);
				LayoutShape root = null;
				// If the root shape was set by the user, AND the shape exists in our shape list
				if (myRootShape == null || !allShapes.TryGetValue(myRootShape, out root))
				{
					root = GetRoot(mostRelatives);
				}
				if (root == null)
				{
					if (backupRoot == null)
					{
						myLayoutEngine.PostLayout(minimumPoint);
						break;
					}
					root = backupRoot;
				}

				// run the layout of base shapes
				myLayoutEngine.PerformLayout(root, new PointD(margin.Width, layoutRectangle.Bottom + (firstPass ? 0 : root.Shape.Size.Height)), ref layoutRectangle);
				firstPass = false;
				backupRoot = null;
				myRootShape = null;
			}

			Reflow(ignoreExistingShapes);
		}

		/// <summary>
		/// Sets the central shape from which all other shapes will extend.
		/// </summary>
		public void SetRootShape(NodeShape shape)
		{
			myRootShape = shape;
		}

		private LayoutShape GetRoot(LayoutShape mostRelatives)
		{
			LayoutShape root = mostRelatives;
			if (root == null)
			{
				// Get the node in the diagram that is not a fact type or external constraint
				LayoutShape first = null;
				foreach (LayoutShape layshape in myLayoutShapes)
				{
					if (!layshape.Placed)
					{
						if (first == null)
						{
							first = layshape;
						}

						if (!(layshape.Shape is FactTypeShape) && !(layshape.Shape is ExternalConstraintShape))
						{
							root = layshape;
							break;
						}
					}
				}

				// either they're all fact type shapes, or we only found one node
				if (root == null)
				{
					root = first;
				}
			}

			return root;
		}

		/// <summary>
		/// Adjust element location for existing shapes and set the Location properties
		/// </summary>
		/// <param name="ignoreExistingShapes">Set if no attempt should be made to flow around existing shapes</param>
		private void Reflow(bool ignoreExistingShapes)
		{
			// Respect the diagram margin
			Diagram diagram = myDiagram;
			double deltaX = 0;
			double deltaY = 0;
			bool movedFirstShape = ignoreExistingShapes;

			foreach (LayoutShape shape in myLayoutShapes)
			{
				if (!shape.Pinned)
				{
					NodeShape ns = shape.Shape;
					PointD currentLocation = shape.TargetLocation;
					if (!movedFirstShape)
					{
						// The shape location has not been set until immediately after this branch
						// so finding the free area will not force a shape to move because it
						// is on itself. Also, the available free area is always down and right.
						RectangleD currentShapeBounds = ns.AbsoluteBounds;
						RectangleD currentRectangle = new RectangleD(currentLocation, currentShapeBounds.Size);
						movedFirstShape = true;
						PointD currentCenter = currentRectangle.Center;
						if (currentShapeBounds.Location.IsEmpty || !currentShapeBounds.Contains(currentCenter))
						{
							RectangleD diagramBounds = diagram.AbsoluteBounds;
							PointD adjustedLocation = diagram.FindFreeArea(currentCenter.X, currentCenter.Y, currentCenter.X, currentCenter.Y, currentRectangle.Width * 1.3, currentRectangle.Height * 1.3, currentRectangle.Width, currentRectangle.Height, currentRectangle.Left, currentRectangle.Top, double.MaxValue, double.MaxValue);
							deltaX += adjustedLocation.X - currentCenter.X;
							deltaY += adjustedLocation.Y - currentCenter.Y;
						}
					}
					ns.Location = new PointD(currentLocation.X + deltaX, currentLocation.Y + deltaY);
				}
			}
		}
	}
	#endregion
}
