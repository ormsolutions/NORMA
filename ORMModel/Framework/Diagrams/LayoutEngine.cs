#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.Shell;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams
{
	#region Plugin code
	/// <summary>
	/// Interface for providing layout engine information.
	/// </summary>
	public interface ILayoutEngineProvider
	{
		/// <summary>
		/// Lists available layout engines from the provider.
		/// </summary>
		/// <returns>A list of available layout engines</returns>
		LayoutEngineData[] ProvideLayoutEngineData();
	}
	/// <summary>
	/// Container for relating a <seealso cref="LayoutEngine"/> to an instance of it
	/// </summary>
	public struct LayoutEngineData
	{
		private Type myKeyType;
		private LayoutEngine myInstance;
		/// <summary>
		/// True if the struct has been initialized
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myInstance == null;
			}
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">A descendant of <see cref="LayoutEngine"/></param>
		/// <param name="instance">An instance of <paramref name="type"/></param>
		public LayoutEngineData(Type type, LayoutEngine instance)
		{
			if (!typeof(LayoutEngine).IsAssignableFrom(type))
			{
				throw new ArgumentException("type");
			}
			if (instance == null || instance.GetType() != type)
			{
				throw new ArgumentException("instance");
			}
			myKeyType = type;
			myInstance = instance;
		}
		/// <summary>
		/// System.Type descendant of <seealso cref="LayoutEngine"/>
		/// </summary>
		public Type KeyType
		{
			get
			{
				return myKeyType;
			}
		}
		/// <summary>
		/// An instance of LayoutEngineData.KeyType
		/// </summary>
		public LayoutEngine Instance
		{
			get
			{
				return myInstance;
				
			}
		}
		/// <summary>
		/// Creates the dictionary list of layout engines based on results from <paramref name="providers"/>
		/// </summary>
		/// <param name="providers">A list of <seealso cref="ILayoutEngineProvider"/>s</param>
		/// <returns>A key/value list, with LayoutEnginData.KeyType as the key, and an instance as the value</returns>
		public static IDictionary<Type, LayoutEngineData> CreateLayoutEngineDictionary(IEnumerable<ILayoutEngineProvider> providers)
		{
			Dictionary<Type, LayoutEngineData> retVal = new Dictionary<Type,LayoutEngineData>();
			foreach (ILayoutEngineProvider provider in providers)
			{
				LayoutEngineData[] data = provider.ProvideLayoutEngineData();
				if (data != null)
				{
					for (int i = 0; i < data.Length; ++i)
					{
						LayoutEngineData currentData = data[i];
						retVal[currentData.KeyType] = currentData;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Creates the dictionary list of layout engines based on <see cref="DomainModel"/>s that
		/// implement <see cref="ILayoutEngineProvider"/> in the provided <see cref="Store"/>
		/// </summary>
		/// <param name="store">An instance of <seealso cref="Store"/></param>
		/// <returns>A key/value list, with LayoutEnginData.KeyType as the key, and an instance as the value</returns>
		public static IDictionary<Type, LayoutEngineData> CreateLayoutEngineDictionary(Store store)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			return CreateLayoutEngineDictionary(Utility.EnumerateDomainModels<ILayoutEngineProvider>(store.DomainModels));
		}
	}
	#endregion

	#region LayoutEngine - base class
	/// <summary>
	/// The base class that all layout engines must implement.
	/// </summary>
	public abstract class LayoutEngine
	{
		/// <summary>
		/// The diagram hosting the shapes.
		/// </summary>
		protected Diagram myDiagram;
		/// <summary>
		/// List of <see cref="LayoutShape"/> elements to place on the diagram.
		/// </summary>
		protected LayoutShapeList myLayoutShapes;

		/// <summary>
		/// Constructor
		/// </summary>
		public LayoutEngine()
		{
		}

		/// <summary>
		/// Perform pre-layout tasks
		/// </summary>
		protected virtual void PreLayout()
		{
			// do nothing
		}

		/// <summary>
		/// Perform post-layout tasks
		/// </summary>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		public virtual void PostLayout(PointD minimumPoint)
		{
			// do nothing
		}

		/// <summary>
		/// Perform operations on <paramref name="shape"/> before it is placed on the diagram
		/// </summary>
		/// <param name="shape">A LayoutShape that is being placed on the diagram</param>
		protected virtual void PreShapePlacement(LayoutShape shape)
		{
			// do nothing
		}

		/// <summary>
		/// Perform operations on <paramref name="shape"/> after it is placed on the diagram
		/// </summary>
		/// <param name="shape">A LayoutShape that has been placed on the diagram</param>
		protected virtual void PostShapePlacement(LayoutShape shape)
		{
			// do nothing
		}

		/// <summary>
		/// Since engines are treated as single-instance, we might need to clear state information between calls
		/// </summary>
		protected virtual void ClearState()
		{
			myLayoutShapes = null;
		}

		/// <summary>
		/// Allows the engine to bind to the layout manager data after the engine is created
		/// </summary>
		/// <param name="diagram"></param>
		/// <param name="layoutShapes"></param>
		public void LateBind(Diagram diagram, LayoutShapeList layoutShapes)
		{
			ClearState();

			myDiagram = diagram;
			myLayoutShapes = layoutShapes;

			PreLayout();
		}

		/// <summary>
		/// The implementer runs the layout algorithm here, setting the <see cref="LayoutShape.TargetLocation"/> property for every
		/// child of <paramref name="rootShape"/> and their descendants.
		/// </summary>
		/// <param name="rootShape">The element from which all other shapes are placed on the diagram.</param>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		/// <param name="layoutRectangle">The bounded rectangle for the newly layed out unpinned shapes</param>
		public abstract void PerformLayout(LayoutShape rootShape, PointD minimumPoint, ref RectangleD layoutRectangle);

		/// <summary>
		/// Resolves references between shapes (indicated as lines on the diagram)
		/// </summary>
		/// <param name="list">Any list of <seealso cref="LayoutShape"/>s that need to have the shape references resolved</param>
		/// <returns>The shape with the most references in the list (typically treated as the root shape)</returns>
		/// <remarks>Any <see cref="LayoutShape"/> where the <see cref="LayoutShape.Placed"/> property is <see langword="true"/>
		/// should be ignored.</remarks>
		public virtual LayoutShape ResolveReferences(LayoutShapeList list)
		{
			LayoutShape mostChildren = null;
			LayoutShapeList allShapes = myLayoutShapes;
			foreach (LayoutShape layshape in list)
			{
				if (!layshape.Placed)
				{
					layshape.ResolveReferences(allShapes);
					if (mostChildren == null || layshape.Count > mostChildren.Count)
					{
						mostChildren = layshape;
					}
				}
			}
			return mostChildren;
		}
	}
	#endregion
	#region BreadthFirstLayoutEngine - base class
	/// <summary>
	/// Perhaps oversimplified (or underimplemented?), this provides the basic functionality for a breadth-first layout engine.
	/// </summary>
	public abstract class BreadthFirstLayoutEngine
		: LayoutEngine
	{
		/// <summary>
		/// The order in which to place shapes on the map, breadth-first
		/// </summary>
		protected Queue<LayoutShape> myPlaceQueue;

		/// <summary>
		/// Simple delegation constructor.
		/// </summary>
		public BreadthFirstLayoutEngine()
		{
			myPlaceQueue = new Queue<LayoutShape>();
		}

		/// <summary>
		/// Since engines are treated as single-instance, we might need to clear state information between calls
		/// </summary>
		protected override void ClearState()
		{
			myPlaceQueue.Clear();
			base.ClearState();
		}

		/// <summary>
		/// Places the specified shape at the end of the queue.
		/// </summary>
		/// <param name="shape"></param>
		protected void AddShapeToQueue(LayoutShape shape)
		{
			myPlaceQueue.Enqueue(shape);
		}

		/// <summary>
		/// Retrieves the next shape in line in the queue.
		/// </summary>
		/// <returns></returns>
		protected LayoutShape GetNextShapeFromQueue()
		{
			return myPlaceQueue.Dequeue();
		}

		/// <summary>
		/// Determines whether more shapes exist in the queue to be placed.
		/// </summary>
		/// <returns>True if more shapes exist in the queue.</returns>
		protected bool HasShapes()
		{
			return myPlaceQueue.Count > 0;
		}
	}
	#endregion

	#region RadialLayoutEngine - actual implementation
	/// <summary>
	/// Performs a standard radial layout, starting at a root shape.
	/// 
	/// The general logic is fairly simple: the density of a shape is the number of
	/// shapes that it talks to; higher-density shapes throw their related shapes out
	/// farther to avoid overlap.  Caveat caller, this routine does absolutely no overlap
	/// checking.
	/// 
	/// See <see cref="RadialBlockLayoutEngine"/> for an example of one that attempts
	/// overlap avoidance, and a comment for a more optimal algorithm.
	/// </summary>
	public class RadialLayoutEngine
		: BreadthFirstLayoutEngine
	{
		/// <summary>
		/// Multiplier to convert degrees to radians
		/// </summary>
		protected const double RADIANS = (Math.PI / 180);
		/// <summary>
		/// Used when avoiding shapes already on the diagram, specifies the number of degrees to either side of that shape to avoid
		/// </summary>
		protected const int SMALLSPREAD = 10;

		/// <summary>
		/// Constructor
		/// </summary>
		public RadialLayoutEngine()
		{
		}

		/// <summary>
		/// Perform pre-layout tasks
		/// </summary>
		protected override void PreLayout()
		{
			// do nothing
		}

		/// <summary>
		/// Perform post-layout tasks
		/// </summary>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		public override void PostLayout(PointD minimumPoint)
		{
			// do nothing
		}

		/// <summary>
		/// Perform operations on <paramref name="shape"/> before it is placed on the diagram
		/// </summary>
		/// <param name="shape">A LayoutShape that is being placed on the diagram</param>
		protected override void PreShapePlacement(LayoutShape shape)
		{
			// do nothing
		}

		/// <summary>
		/// Perform operations on <paramref name="shape"/> after it is placed on the diagram
		/// </summary>
		/// <param name="shape">A LayoutShape that has been placed on the diagram</param>
		protected override void PostShapePlacement(LayoutShape shape)
		{
			// do nothing
		}

		/// <summary>
		/// Since engines are treated as single-instance, we might need to clear state information between calls
		/// </summary>
		protected override void ClearState()
		{
			base.ClearState();
		}

		/// <summary>
		/// Performs standard radial layout algorithm.
		/// </summary>
		/// <param name="rootShape">The shape determined to be the center of the diagram.</param>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		/// <param name="layoutRectangle">The bounded rectangle for the newly layed out unpinned shapes</param>
		public override void PerformLayout(LayoutShape rootShape, PointD minimumPoint, ref RectangleD layoutRectangle)
		{
			List<double> degrees = GenerateDegrees(-180, 180, 1);

			BuildTree(rootShape, null);

			// Find the starting average location to best locate the root element
			int allCount = 0;
			int pinnedCount = 0;
			double pinnedAverageX = 0;
			double pinnedAverageY = 0;
			double allAverageX = 0;
			double allAverageY = 0;
			IterateLayoutShapes(
				rootShape,
				delegate(LayoutShape shape)
				{
					RectangleD bounds = shape.Shape.AbsoluteBounds;
					if (!bounds.Location.IsEmpty)
					{
						PointD location = bounds.Center;
						++allCount;
						allAverageX += location.X;
						allAverageY += location.Y;
						if (shape.Pinned)
						{
							++pinnedCount;
							pinnedAverageX += location.X;
							pinnedAverageY += location.Y;
							// Record for easy access later
							shape.TargetLocation = location;
						}
					}
				});

			// Set a starting location for the rootShape
			PointD offsetBy = default(PointD);
			if (pinnedCount != 0)
			{
				pinnedAverageX /= pinnedCount;
				pinnedAverageY /= pinnedCount;
				offsetBy = new PointD(pinnedAverageX, pinnedAverageY);
				// Use later for adjustment
				allAverageX = pinnedAverageX;
				allAverageY = pinnedAverageY;
			}
			else if (allCount != 0)
			{
				allAverageX /= allCount;
				allAverageY /= allCount;
				offsetBy = new PointD(allAverageX, allAverageY);
			}
			rootShape.TargetLocation = offsetBy;

			// Run the main recursive algorithm
			AddShapeToQueue(rootShape);

			PlaceShapes(rootShape, degrees);

			// Determine where our shapes ended up to get a bounding box
			int newCount = 0;
			double adjustX = 0;
			double adjustY = 0;
			double minX = double.MaxValue;
			double minY = double.MaxValue;
			double maxX = double.MinValue;
			double maxY = double.MinValue;
			IterateLayoutShapes(
				rootShape,
				delegate(LayoutShape shape)
				{
					PointD location = shape.TargetLocation;
					SizeD size = shape.Shape.AbsoluteBounds.Size;
					if (!shape.Pinned)
					{
						minX = Math.Min(minX, location.X);
						minY = Math.Min(minY, location.Y);
						maxX = Math.Max(maxX, location.X + size.Width);
						maxY = Math.Max(maxY, location.Y + size.Height);
					}
					location.Offset(size.Width / 2, size.Height / 2);
					++newCount;
					adjustX += location.X;
					adjustY += location.Y;
				});

			if (minX != double.MaxValue)
			{
				// Adjust the average position of non-pinned elements and
				// set the location of the underlying elements
				if (allCount != 0)
				{
					adjustX = allAverageX - adjustX / newCount;
					minX += adjustX;
					maxX += adjustX;
					adjustY = allAverageY - adjustY / newCount;
					minY += adjustY;
					maxY += adjustY;
				}
				else
				{
					adjustX = 0;
					adjustY = 0;
				}
				double offset;
				//offset = minimumPoint.X - minX;
				if (0 < (offset = minimumPoint.X - minX))
				{
					adjustX += offset;
					minX += offset;
					maxX += offset;
				}
				//offset = minimumPoint.Y - minY;
				if (0 < (offset = minimumPoint.Y - minY))
				{
					adjustY += offset;
					minY += offset;
					maxY += offset;
				}
				layoutRectangle = new RectangleD(minX, minY, maxX - minX, maxY - minY);

				// Adjust locations and move the actual shape
				IterateLayoutShapes(
					rootShape,
					delegate(LayoutShape shape)
					{
						if (!shape.Pinned)
						{
							shape.TargetLocation.Offset(adjustX, adjustY);
						}
					});
			}

			// Now that all shapes are in the correct position, notify the new
			// placement for all shapes.
			IterateLayoutShapes(
				rootShape,
				delegate(LayoutShape shape)
				{
					PostShapePlacement(shape);
				});
		}

		/// <summary>
		/// So far the primary purpose of this method is to bring multiple references between one object and other
		/// together, adjacent in the RelatedShapes array, which minimizes line crossing.
		/// Eventually this can be expanded to include branch weighting, which helps determine what branches require 
		/// the most graph space, and should therefore be allocated more or less radial range.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="parent"></param>
		private void BuildTree(LayoutShape shape, LayoutShape parent)
		{
			int depth = (parent != null ? parent.Depth + 1 : 1);
			if (shape.Depth != 0 && shape.Depth < depth)
			{
				return;
			}

			shape.Depth = depth;
			shape.Parent = parent;
			LayoutShape grandparent = (parent == null ? null : parent.Parent);

			// This is somewhat hack-ish, since layout is supposed to be shape-neutral...but with
			// objectified fact types as they are, I have to do this.
			/*
			if (grandparent != null && grandparent.Shape is FactTypeShape)
			{
				grandparent = grandparent.Parent;
			}
			 */

			if (grandparent != null)
			{
				grandparent.Gather(shape);
			}

			for (int i = 0; i < shape.Count; i++)
			{
				BuildTree(shape.RelatedShapes[i], shape);
			}
		}
		/// <summary>
		/// Callback used by IterateTree
		/// </summary>
		/// <param name="shape">The <see cref="LayoutShape"/> to visit</param>
		private delegate void LayoutShapeVisitor(LayoutShape shape);
		/// <summary>
		/// Recursive walk all elements related to the current tree. This does
		/// a depth-first walk.
		/// </summary>
		/// <param name="shape">The <see cref="LayoutShape"/> to examine</param>
		/// <param name="visit">A <see cref="LayoutShapeVisitor"/> callback delegate</param>
		private void IterateLayoutShapes(LayoutShape shape, LayoutShapeVisitor visit)
		{
			visit(shape);
			foreach (LayoutShape child in shape.RelatedShapes)
			{
				if (child.Parent == shape)
				{
					IterateLayoutShapes(child, visit);
				}
			}
		}

		private void PlaceShapes(LayoutShape rootShape, List<double> degrees)
		{
			LayoutShape shape = null;
			PointD location = rootShape.TargetLocation;
			double originX = location.X;
			double originY = location.Y;

			/*
			 * This probably looks a little odd, but there's a reason to the madness.  This loop/queue combo allows
			 * the graph to be drawn breadth-first; depth-first is simply wrong for ORM.
			 * Related shapes are not actually drawn within the bottom half of the loop.  Rather their target location is
			 * stored for when they are dequeued.
			 */
			while (myPlaceQueue.Count > 0)
			{
				shape = GetNextShapeFromQueue();
				shape.Placed = true;

				shape.CountUnplacedRelatives();
				if (shape.UnplacedCount == 0)
				{
					continue;
				}

				// *** Everything from here down relates to child shape placement calculations ***

				// Determine the angles off the current shape that we can place the children
				List<double> relDegrees;
				if (shape != rootShape)
				{
					int spread = (shape.UnplacedCount * 17) % 360;
					if (spread > 90)
					{
						// Move this shape out farther so it doesn't interfere as much with other shapes
						spread = 90;
					}
					relDegrees = GenerateDegrees((int)shape.AngleFromParent - spread, (int)shape.AngleFromParent + spread, shape.Depth + 1);
				}
				else
				{
					relDegrees = degrees;
				}
				int degreeIndex = 0;
				// The amount to increment degreeIndex, based on the # of children and the size of the degree spread available
				int degreeIndexIncrement = (int)Math.Floor((double)relDegrees.Count / shape.UnplacedCount);

				if (shape.Pinned)
				{
					// Remove degrees to avoid already-placed shapes, then retrieve the next most appropriate degree array index.
					degreeIndex = AvoidPinnedShapes(shape, relDegrees, degreeIndex);
				}

				location = shape.TargetLocation;
				PlaceChildShapes(location.X, location.Y, shape, relDegrees, degreeIndex, degreeIndexIncrement);
			}
		}

		private int AvoidPinnedShapes(LayoutShape shape, List<double> relDegrees, int degreeIndex)
		{
			PointD shapeLocation = shape.TargetLocation;
			NodeShape nodeShape = shape.Shape;

			// We did not place the shape, there may be existing children on the map that we don't want to overlap
			foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(nodeShape))
			{
				NodeShape relation = MultiShapeUtility.ResolvePrimaryShape(binaryLink.FromShape) as NodeShape;
				if (relation == nodeShape)
				{
					relation = MultiShapeUtility.ResolvePrimaryShape(binaryLink.ToShape) as NodeShape;
				}
				LayoutShape childShape = null;
				if (relation == null ||
					(myLayoutShapes.TryGetValue(relation, out childShape) && !childShape.Pinned))
				{
					continue;
				}

				// The child shape is pinned
				PointD relationLocation = relation.Location;
				double yDelta = relationLocation.Y - shapeLocation.Y;
				double xDelta = relationLocation.X - shapeLocation.X;
				double angle = (Math.Atan2(Math.Abs(yDelta), Math.Abs(xDelta)) * (180 / Math.PI)) % 90;

				// now adjust the angle based on the quadrant
				if (yDelta > 0)
				{
					if (xDelta < 0)
					{
						angle = 180 - angle;
					}
				}
				else
				{
					if (xDelta > 0)
					{
						angle = 360 - angle;
					}
					else
					{
						angle = 180 + angle;
					}
				}

				// Filter out degrees that might overlap this child shape
				RemoveDegrees(relDegrees, angle - SMALLSPREAD, angle + SMALLSPREAD);
				if (!FindClosestAngle(relDegrees, angle - 180, ref degreeIndex))
				{
					// Not great, but it beats throwing an exception here
					relDegrees.Add(0d);
					degreeIndex = 0;
				}
			}
			return degreeIndex;
		}

		private void PlaceChildShapes(double originX, double originY, LayoutShape parent, List<double> relDegrees, int degreeIndex, int degreeIndexIncrement)
		{
			double relDegree;
			double x;
			double y;
			// Proportionally stretch the diagram out as more children have to be rendered
			double distanceMultiplier = Math.Max(0.9, parent.UnplacedCount / (double)9);

			LayoutShapeList relatedShapes = parent.RelatedShapes;
			int relatedShapesCount = relatedShapes.Count;
			int childDepth = parent.Depth + 1;
			for (int i = 0; i < relatedShapesCount; ++i)
			{
				// Get the shape
				LayoutShape childShape = relatedShapes[i];
				if (childShape.Placed)
				{
					// Already placed, move to the next one
					continue;
				}

				/* NOTE: Some diagrams render better with the following code, while others do not.  The parent assignment comes
				 * from the BuildTree method.  It may be possible to dynamically determine which diagrams favor this
				 * method, but I didn't have time.
				if (childshape.Parent != parent)
				{
					// Some other shape is allocated to render this particular shape
					continue;
				}
				 */

				// determine the angle at which this child will be drawn relative to the parent
				relDegree = relDegrees[degreeIndex];
				if (parent.Count > 1)
				{
					degreeIndex += degreeIndexIncrement;
					if (degreeIndex >= relDegrees.Count)
					{
						degreeIndex = relDegrees.Count - 1;
					}
				}

				// now calculate the position of the child based on the allocated degree and distance multiplier
				CalculateShapeXY(childShape, parent, originX, originY, Math.Max(distanceMultiplier, childShape.Count / 6), relDegree, out x, out y);

				// and assign that location to the shape
				// don't do this inside CalculateShapeXY, since x & y are to be calculated ONLY, not set
				childShape.TargetLocation = new PointD(x, y);
				childShape.AngleFromParent = relDegree;
				childShape.Depth = childDepth;

				AddShapeToQueue(childShape);
			}
		}

		/// <summary>
		/// Calculates the new position of the specified <see cref="LayoutShape"/>.
		/// </summary>
		/// <param name="shape">The <see cref="LayoutShape"/> whose <see cref="NodeShape"/> location needs to be calculated.</param>
		/// <param name="parent">The <see cref="LayoutShape"/> that is placing the current shape on the diagram.</param>
		/// <param name="originX">The X coordinate of the parent shape.</param>
		/// <param name="originY">The Y coordinate of the parent shape.</param>
		/// <param name="distanceMultiplier">The recommended distance from originX,originY (at angle <paramref name="angle"/>) to position <paramref name="shape"/>.</param>
		/// <param name="angle">The angle from originX,originY to position <paramref name="shape"/></param>
		/// <param name="x">The result of the calculation of x</param>
		/// <param name="y">The result of the calculation of y</param>
		protected virtual void CalculateShapeXY(LayoutShape shape, LayoutShape parent, double originX, double originY, double distanceMultiplier, double angle, out double x, out double y)
		{
			double xOffset = 0;
			double rad = angle * RADIANS;
			double cos = Math.Cos(rad);
			double sin = -Math.Sin(rad);
			double xDelta;
			double yDelta;
			x = cos * distanceMultiplier;
			y = sin * distanceMultiplier;

			// push the element out to form a more rectangular shape than pure circular
			xDelta = x * (sin * sin);
			yDelta = y * (cos * cos) * 0.3;

			// we may need to push the element out to the right or left
			double testWidth;
			if (Math.Abs(0 - angle) < 20 || Math.Abs(360 - angle) < 20)
			{
				// this "if" needs to be nested to avoid odd conditional outcomes with the else/if below
				if ((testWidth = parent.Shape.Size.Width) > 0.4)
				{
					xOffset = Math.Abs(testWidth - 0.4);
				}
			}
			else if (Math.Abs((180 - angle) % 180) < 40 && (testWidth = shape.Shape.Size.Width) > 0.4)
			{
				xOffset = 0.4 - testWidth;
				if (xOffset > 0)
				{
					xOffset = -xOffset;
				}
			}

			// set its location off from the parent
			x += xDelta + originX + xOffset;
			y += yDelta + originY;
		}

		/// <summary>
		/// Finds the angle in <paramref name="angles"/> that is closest to <paramref name="target"/>.
		/// </summary>
		/// <param name="angles"></param>
		/// <param name="target"></param>
		/// <param name="newIndex"></param>
		/// <returns>True if any angle is found; false generally only for zero-length angle list</returns>
		private bool FindClosestAngle(List<double> angles, double target, ref int newIndex)
		{
			target = NormalizeDegree(target);
			double minDistance = double.MaxValue;

			int angleCount = angles.Count;
			for (int i = 0; i < angleCount; ++i)
			{
				double thisDistance = Math.Abs(NormalizeDegree(angles[i]) - target);
				if (thisDistance < minDistance)
				{
					minDistance = thisDistance;
					newIndex = i;
				}
			}

			return (minDistance != double.MaxValue);
		}

		/// <summary>
		/// Creates a list of angles between <paramref name="min"/> and <paramref name="max"/>, assuming the specified <paramref name="radius"/>.
		/// </summary>
		/// <param name="min">The mininum angle, in degrees</param>
		/// <param name="max">The maximum angle, in degrees</param>
		/// <param name="radius">The radius of the circle at which the angles will be calculated</param>
		/// <returns>A list of angles between <paramref name="min"/> and <paramref name="max"/></returns>
		private List<double> GenerateDegrees(int min, int max, int radius)
		{
			int count = (max - min) * radius;
			// Start counting at the middle angle.  This ensures that the first element gets the optimal angle out
			// from the origin.  If we didn't do this, diagrams can end up looking pretty cock-eyed.
			double angle = ((max - min) / 2) + min;
			List<double> ret = new List<double>(count);
			double angleIncrement = 1 / (double)radius;
			for (int i = 0; i < count; i++)
			{
				ret.Add(angle);
				angle += angleIncrement;
				if (angle > max)
				{
					angle = min;
				}
			}
			return ret;
		}

		/// <summary>
		/// Removes the entries in <paramref name="list"/> that fall between <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <param name="list">The list to be trimmed</param>
		/// <param name="min">The low bound of the angle range to remove</param>
		/// <param name="max">The high bound of the angle range to remove</param>
		private void RemoveDegrees(List<double> list, double min, double max)
		{
			min = NormalizeDegree(min);
			max = NormalizeDegree(max);
			bool wrappedRange;
			if (wrappedRange = (max < min))
			{
				max += 360.0;
			}
			int i = 0;
			while (i < list.Count)
			{
				double currentDegree = NormalizeDegree(list[i]);
				if ((currentDegree > min && currentDegree < max) ||
					(wrappedRange && (currentDegree += 360.0) > min && currentDegree < max))
				{
					// This is a potentially expensive move -- in some cases constructing a new list of non-removed degrees is more optimal
					list.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		private static double NormalizeDegree(double degree)
		{
			return (degree >= 0.0) ? (degree % 360.0) : ((degree % 360.0) + 360.0);
		}

		/// <summary>
		/// Splits the list of angles in <paramref name="degrees"/> into <paramref name="count"/> equal parts.
		/// </summary>
		/// <param name="degrees">A list of angles on the unit circle.  May be fractions of angles.</param>
		/// <param name="count">The number of divisions to make.</param>
		/// <returns></returns>
		private List<List<double>> SplitDegrees(List<double> degrees, int count)
		{
			int splitAt = degrees.Count / count;
			List<List<double>> res = new List<List<double>>();

			int idx = 0;
			List<double> tmp = new List<double>();
			int degreeCount = degrees.Count;
			for (int i = 0; i < degreeCount; ++i)
			{
				if (idx == splitAt)
				{
					idx = 0;
					res.Add(tmp);
					tmp = new List<double>();
				}
				tmp.Add(degrees[i]);
				idx++;
			}
			res.Add(tmp);

			return res;
		}
	}
	#endregion
	#region RadialBlockLayoutEngine - actual implementation
	/// <summary>
	/// The radial layout doesn't check for overlap of shapes on other shapes.  This class
	/// essentially snaps all shapes to a fixed grid (based on the size of the largest shape)
	/// and calculates (and hopefully avoids) overlap that way.
	/// 
	/// In its current implementation, overlap is strangely not avoided -- probably due to some
	/// tweak I made, as it used to work properly.  The problem with this layout style is that
	/// mixing grid and radial layout doesn't work all that well.  The angles are often so
	/// closely spaced that several shapes will align to the same grid location, which makes
	/// it tough to work out how to space them out.  (The current method is to increase the
	/// distance multiplier so the shape goes farther out, but that rarely works well.)
	/// 
	/// A more optimal algorithm would *logically* (not physically) organize the shapes by
	/// grid points, then calculate overlap based on the restricted number of shapes that
	/// exist near that grid point.
	/// </summary>
	public class RadialBlockLayoutEngine
		: RadialLayoutEngine
	{
		private double myXInterval = 0.3;
		private double myYInterval = 0.2;
		private RectangleD myLargestShape = RectangleD.Empty;
		private Dictionary<string, object> myVisitedPoints = new Dictionary<string, object>();

		/// <summary>
		/// Constructor for RadialBlockLayoutEngine.
		/// </summary>
		public RadialBlockLayoutEngine()
		{
		}

		/// <summary>
		/// Performs the grid-based radial layout algorithm.
		/// </summary>
		/// <param name="rootShape">The shape determined to be the center of the diagram.</param>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		/// <param name="layoutRectangle">The bounded rectangle for the newly layed out unpinned shapes</param>
		public override void PerformLayout(LayoutShape rootShape, PointD minimumPoint, ref RectangleD layoutRectangle)
		{
			FindLargestShape();
			base.PerformLayout(rootShape, minimumPoint, ref layoutRectangle);
		}

		private void FindLargestShape()
		{
			double maxWidth = 0;
			double maxHeight = 0;
			foreach (LayoutShape shape in myLayoutShapes)
			{
				RectangleD bounds = shape.Shape.AbsoluteBounds;
				maxWidth = Math.Max(bounds.Width, maxWidth);
				maxHeight = Math.Max(bounds.Height, maxHeight);
			}
			myLargestShape = new RectangleD(0, 0, maxWidth, maxHeight);
			myXInterval = (Math.Round(maxWidth * 100) / 100) + 0.3;
			myYInterval = (Math.Round(maxHeight * 100) / 100) + 0.3;
		}

		/// <summary>
		/// Snaps <paramref name="x"/> and <paramref name="y"/> to a fixed grid location, then returns the availability
		/// of that location.
		/// 
		/// It uses a very simple method of converting the X,Y values to a string, then adding that string to a
		/// dictionary of occupied grid points.  Its efficiency is based on whether the dictionary uses the
		/// hash code, like a hash table.  If not, then converting to a hash table would be faster.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>True if the grid location is available.</returns>
		private bool SnapToGrid(ref double x, ref double y)
		{
			x = Math.Round(x * 100) / 100;
			y = Math.Round(y * 100) / 100;
			int xsign = (x < 0 ? -1 : 1);
			int ysign = (y < 0 ? -1 : 1);
			double delta_x = Math.Abs(x % myXInterval);
			double delta_y = Math.Abs(y % myYInterval);

			if (delta_x != 0)
			{
				if (delta_x > myXInterval / 2)
				{
					x += (myXInterval - delta_x) * xsign;
				}
				else
				{
					x -= delta_x * xsign;
				}
			}
			if (delta_y != 0)
			{
				if (delta_y > myYInterval / 2)
				{
					y += (myYInterval - delta_y) * ysign;
				}
				else
				{
					y -= delta_y * ysign;
				}
			}

			// snap to grid
			// UNDONE: This is way too expensive, and not accurate
			// Key a collection off a point, and use a comparison
			// routine that accounts for roundoff error to compare
			// the points. There is no reason to use strings here.
			string s = x.ToString() + "," + y.ToString();
			bool ret = !myVisitedPoints.ContainsKey(s);
			if (!ret)
			{
				// the grid location hadn't been encountered before, so add it
				myVisitedPoints.Add(s, null);
			}

			return ret;
		}

		/// <summary>
		/// Calculates the new position of the specified <see cref="LayoutShape"/>.
		/// </summary>
		/// <param name="shape">The <see cref="LayoutShape"/> whose <see cref="NodeShape"/> location needs to be calculated.</param>
		/// <param name="parent">The <see cref="LayoutShape"/> that is placing the current shape on the diagram.</param>
		/// <param name="originX">The X coordinate of the parent shape.</param>
		/// <param name="originY">The Y coordinate of the parent shape.</param>
		/// <param name="distanceMultiplier">The recommended distance from originX,originY (at angle <paramref name="angle"/>) to position <paramref name="shape"/>.</param>
		/// <param name="angle">The angle from originX,originY to position <paramref name="shape"/></param>
		/// <param name="x">The result of the calculation of x</param>
		/// <param name="y">The result of the calculation of y</param>
		protected override void CalculateShapeXY(LayoutShape shape, LayoutShape parent, double originX, double originY, double distanceMultiplier, double angle, out double x, out double y)
		{
			double rad = angle * RADIANS;
			double scaledCos = Math.Cos(rad) * distanceMultiplier;
			double scaledSin = -Math.Sin(rad) * distanceMultiplier;
			x = scaledCos + originX;
			y = scaledSin + originY;

			while (!SnapToGrid(ref x, ref y))
			{
				scaledCos *= 1.2;
				scaledSin *= 1.2;
				x = scaledCos + originX;
				y = scaledSin + originY;
			}
		}
	}
	#endregion

	/*
	 * DO NOT modify the code below here unless you know what you're doing.
	 */

	#region LayoutShape
	/// <summary>
	/// A simple vector (origin and direction) 
	/// </summary>
	public struct LayoutVector
	{
		/// <summary>
		/// X coordinate of the origin
		/// </summary>
		public double X;
		/// <summary>
		/// Y coordinate of the origin
		/// </summary>
		public double Y;
		/// <summary>
		/// Angle of departure from the origin
		/// </summary>
		public double Angle;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="x">X coordinate of the origin</param>
		/// <param name="y">Y coordinate of the origin</param>
		/// <param name="angle">Angle of departure from the origin</param>
		public LayoutVector(double x, double y, double angle)
		{
			X = x;
			Y = y;
			Angle = angle;
		}
	}

	/// <summary>
	/// A helper class to encapsulate information about a particular <see cref="NodeShape"/> on the diagram.
	/// </summary>
	[DebuggerDisplay("LayoutShape for '{Shape.ModelElement}'")]
	public class LayoutShape
	{
		/// <summary>
		/// Reference to the shape on the diagram.
		/// </summary>
		public NodeShape Shape;
		/// <summary>
		/// A list of all directly related <see cref="LayoutShape"/>s
		/// </summary>
		public LayoutShapeList RelatedShapes = new LayoutShapeList();
		/// <summary>
		/// The number of <see cref="LayoutShape"/>s in ChildShapes.
		/// </summary>
		public int Count;
		/// <summary>
		/// The number of unplaced <see cref="LayoutShape"/>s in ChildShapes.
		/// </summary>
		public int UnplacedCount;
		/// <summary>
		/// The pin state of the corresponding <see cref="NodeShape"/>.  When true, the shape is not moved by the layout engine.
		/// </summary>
		public bool Pinned;
		/// <summary>
		/// The placement state of the corresponding <see cref="NodeShape"/>.  When true, the shape has been placed by the layout engine (unless Pinned is true).
		/// </summary>
		public bool Placed;
		/// <summary>
		/// The target location of the <see cref="NodeShape"/>.  Only used during the layout process.
		/// </summary>
		public PointD TargetLocation;
		/// <summary>
		/// For non-deterministic layout engines, this vector provides an open-ended mechanism.
		/// </summary>
		public LayoutVector TargetVector;
		/// <summary>
		/// The LayoutShape that ends up drawing this LayoutShape on the diagram.
		/// </summary>
		public LayoutShape Parent = null;
		/// <summary>
		/// The angle from the parent that this element was drawn at.
		/// </summary>
		public double AngleFromParent = 0;
		/// <summary>
		/// The depth of this shape relative to the selected root shape.
		/// </summary>
		public int Depth = 0;

		/// <summary>
		/// Associates a grandchild shape with a child shape.  When the <seealso cref="Gather"/> method is called,
		/// this shape will move that grandchild's parent beside any prior children that also talk to that
		/// grandchild.
		/// </summary>
		private Dictionary<LayoutShape, LayoutShape> myGrandChildren = new Dictionary<LayoutShape, LayoutShape>();

		/// <summary>
		/// Constructor for <see cref="LayoutShape"/>.  Sets the Pinned and Shape properties, then determines all of the <see cref="NodeShape"/>s that it
		/// connects to.  Those <see cref="NodeShape"/>s will then be converted to <see cref="LayoutShape"/>s via <see cref="ResolveReferences(LayoutShapeList)"/>.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="pinned"></param>
		public LayoutShape(NodeShape shape, bool pinned)
		{
			Pinned = pinned;
			Shape = shape;
		}

		/// <summary>
		/// Tallies up the number of related <see cref="NodeShape"/>s that have not yet been given locations
		/// </summary>
		public void CountUnplacedRelatives()
		{
			UnplacedCount = 0;
			foreach (LayoutShape layshape in RelatedShapes)
			{
				if (!layshape.Placed)
				{
					UnplacedCount++;
				}
			}
		}

		/// <summary>
		/// Allows the <see cref="LayoutShape"/> to translate references from <see cref="NodeShape"/> instances to their <see cref="LayoutShape"/> containers.
		/// </summary>
		/// <param name="allShapes">An instance of <see cref="LayoutShapeList"/></param>
		public void ResolveReferences(LayoutShapeList allShapes)
		{
			if (RelatedShapes.Count != 0)
			{
				return;
			}
			ResolveReferencesHelper(allShapes, Shape, null);
			UnplacedCount = Count = RelatedShapes.Count;
		}
		/// <summary>
		/// Helper method for ResolveReferences to handle links attached to links
		/// </summary>
		private void ResolveReferencesHelper(LayoutShapeList allShapes, NodeShape testShape, BinaryLinkShape discludeLinkShape)
		{
			ShapeElement linkedShape;
			BinaryLinkShape attachedToLinkShape;
			NodeShape attachedToNodeShape;
			foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(testShape))
			{
				if (discludeLinkShape == binaryLink)
				{
					continue;
				}
				for (int i = 0; i < 2; ++i)
				{
					linkedShape = MultiShapeUtility.ResolvePrimaryShape(i == 0 ? binaryLink.FromShape : binaryLink.ToShape);
					if (linkedShape != Shape)
					{
						if (null != (attachedToLinkShape = linkedShape as BinaryLinkShape))
						{
							if (Shape != (attachedToNodeShape = attachedToLinkShape.FromShape))
							{
								ResolveReferencesHelper(allShapes, attachedToNodeShape, attachedToLinkShape);
							}
							if (Shape != (attachedToNodeShape = attachedToLinkShape.ToShape))
							{
								ResolveReferencesHelper(allShapes, attachedToNodeShape, attachedToLinkShape);
							}
						}
						else if (null != (attachedToNodeShape = linkedShape as NodeShape))
						{
							if (attachedToNodeShape != Shape)
							{
								LayoutShape relative;
								if (allShapes.TryGetValue(attachedToNodeShape, out relative) &&
									!RelatedShapes.Contains(relative))
								{
									RelatedShapes.Add(relative);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Assists in moving child shapes that refer to a common grandchild shape to adjacent points in the array.
		/// </summary>
		/// <param name="grandchild"></param>
		public void Gather(LayoutShape grandchild)
		{
			LayoutShape currentparent = grandchild.Parent;
			// Find the parent of grandchild that directly relates to the current shape
			while (currentparent != null && !RelatedShapes.Contains(currentparent))
			{
				currentparent = currentparent.Parent;
			}
			if (currentparent == null)
			{
				// Didn't find it, so duck out
				return;
			}

			if (!myGrandChildren.ContainsKey(grandchild))
			{
				myGrandChildren.Add(grandchild, currentparent);
			}
			else
			{
				// This can probably be optimized -- just moving the found parent to be next
				// to the last shape that talks to the same grandchild (added in opposite block)
				LayoutShape otherchild = myGrandChildren[grandchild];
				int otherchildidx = RelatedShapes.IndexOf(otherchild);
				int currentidx = RelatedShapes.IndexOf(currentparent);
				RelatedShapes.Remove(currentparent);
				RelatedShapes.Insert(otherchildidx, currentparent);
			}

			// Recursively do this all the way to the top.  This can be optimized out with some
			// big-O growth calculation.
			if (this.Parent != null)
			{
				this.Parent.Gather(grandchild);
			}
		}
	}
	#endregion
	#region Simple helper classes
	/// <summary>
	/// Helper class to find a <see cref="LayoutShape"/> by its contained <see cref="NodeShape"/>
	/// </summary>
	public class LayoutShapeList : KeyedCollection<NodeShape, LayoutShape>
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LayoutShapeList()
			: base(null, 8)
		{
		}
		/// <summary>
		/// Key the layout shape off the key shape
		/// </summary>
		protected override NodeShape GetKeyForItem(LayoutShape item)
		{
			return item.Shape;
		}
		/// <summary>
		/// Searches for a <paramref name="value"/> based on its <paramref name="key"/>
		/// </summary>
		/// <param name="key">A <see cref="NodeShape"/> that might be associated with a <see cref="LayoutShape"/></param>
		/// <param name="value">Returns a <see cref="LayoutShape"/> or <see langword="null"/></param>
		/// <returns>Returns <see langword="true"/> if the item is found.</returns>
		public bool TryGetValue(NodeShape key, out LayoutShape value)
		{
			if (Contains(key))
			{
				value = this[key];
				return true;
			}
			value = null;
			return false;
		}
	}
	#endregion
}
