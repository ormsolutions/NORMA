using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.Shell
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
		private const double MIN_MARGIN = 0.6;

		/// <summary>
		/// List of <see cref="LayoutShape"/> elements to place on the diagram.
		/// </summary>
		private LayoutShapeList myLayoutShapes = new LayoutShapeList();
		/// <summary>
		/// Associative array between a <see cref="NodeShape"/> and its <see cref="LayoutShape"/> container.
		/// </summary>
		private NodeShapeToLayoutShapeResolver myShapeResolver = new NodeShapeToLayoutShapeResolver();
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
			if (shape as NodeShape == null || shape.ParentShape as ORMDiagram == null)
			{
				return;
			}

			NodeShape ns = shape as NodeShape;
			LayoutShapeList list = myLayoutShapes;
			LayoutShape layshape = null;

			// If the shape doesn't exist, add it, otherwise simply modify the pinned value.
			if ((layshape = list.FindByNodeShape(ns)) == null)
			{
				layshape = new LayoutShape(ns, pinned);
				list.Add(layshape);
				if (list == myLayoutShapes)
				{
					myShapeResolver.Add(ns, layshape);
				}
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
		/// Organizes the shapes in <see cref="myLayoutShapes"/>.
		/// </summary>
		public void Layout()
		{
			LayoutShape backupRoot = null;
			switch (myLayoutShapes.Count)
			{
				case 0:
					return;
				default:
					backupRoot = myLayoutShapes[0];
					break;
			}
			myLayoutEngine.LateBind(myDiagram, myLayoutShapes, myShapeResolver);
			LayoutShape mostrelatives = myLayoutEngine.ResolveReferences(myLayoutShapes);
			LayoutShape root = null;
			// If the root shape was set by the user, AND the shape exists in our shape list
			if (myRootShape != null && myShapeResolver.ContainsKey(myRootShape))
			{
				root = myShapeResolver[myRootShape];
			}
			else
			{
				root = GetRoot(mostrelatives);
			}
			if (root == null)
			{
				root = backupRoot;
			}

			double minX = 0, minY = 0;
			// run the layout of base shapes
			myLayoutEngine.PerformLayout(root, ref minX, ref minY);

			// shift the graph so that it's all visible
			if (minX <= 0 || minY <= 0)
			{
				Reflow(-minX, -minY);
			}
		}

		/// <summary>
		/// Sets the central shape from which all other shapes will extend.
		/// </summary>
		/// <param name="shape"></param>
		public void SetRootShape(NodeShape shape)
		{
			myRootShape = shape;
		}

		private LayoutShape GetRoot(LayoutShape mostrelatives)
		{
			LayoutShape root = mostrelatives;
			if (root == null)
			{
				// Get the node in the diagram that is not a fact type or external constraint
				LayoutShape first = null;
				foreach (LayoutShape layshape in myLayoutShapes)
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

				// either they're all fact type shapes, or we only found one node
				if (root == null)
				{
					root = first;
				}
			}

			return root;
		}

		private void Reflow(double deltaX, double deltaY)
		{
			// leave a left & top margin
			// (note that DSL has a minimum margin that corresponds roughly to the numbers (in inches) below
			deltaX += MIN_MARGIN;
			deltaY += MIN_MARGIN;

			foreach (LayoutShape shape in myLayoutShapes)
			{
				NodeShape ns = shape.Shape;
				ns.Location = new PointD(ns.Location.X + deltaX, ns.Location.Y + deltaY);
			}
		}
	}
	#endregion
}
