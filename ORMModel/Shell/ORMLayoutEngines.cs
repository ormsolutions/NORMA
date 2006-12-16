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
	#region ORMRadialLayoutEngine - actual implementation
	/// <summary>
	/// Performs a standard radial layout, starting at <paramref name="rootshape"/>.
	/// 
	/// The general logic is fairly simple: the density of a shape is the number of
	/// shapes that it talks to; higher-density shapes throw their related shapes out
	/// farther to avoid overlap.  Caveat caller, this routine does absolutely no overlap
	/// checking.
	/// 
	/// See <see cref="RadialBlockLayoutEngine"/> for an example of one that attempts
	/// overlap avoidance, and a comment for a more optimal algorithm.
	/// </summary>
	public class ORMRadialLayoutEngine
		: RadialLayoutEngine
	{
		LayoutShapeList myConstraintShapes = new LayoutShapeList();
		LayoutShape myLastPlacedShape = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		public ORMRadialLayoutEngine()
			: base()
		{
		}

		/// <summary>
		/// Resolves references between shapes (indicated as lines on the diagram)
		/// </summary>
		/// <param name="list">Any list of <seealso cref="LayoutShape"/>s that need to have the shape references resolved</param>
		/// <returns>The shape with the most references in the list (typically treated as the root shape)</returns>
		public override LayoutShape ResolveReferences(LayoutShapeList list)
		{
			LayoutShape mostchildren = null;
			foreach (LayoutShape layshape in list)
			{
				layshape.ResolveReferences(myShapeResolver);
				if ((mostchildren == null || layshape.Count > mostchildren.Count) && !(layshape.Shape is FactTypeShape) && !(layshape.Shape is ExternalConstraintShape))
				{
					mostchildren = layshape;
				}
			}
			return mostchildren;
		}

		/// <summary>
		/// Since engines are treated as single-instance, we might need to clear state information between calls
		/// </summary>
		protected override void ClearState()
		{
			myConstraintShapes.Clear();
			myLastPlacedShape = null;
			base.ClearState();
		}

		/// <summary>
		/// Perform pre-layout tasks
		/// </summary>
		protected override void PreLayout()
		{
			// Separate out the external constraint shapes
			for (int i=0; i < myLayoutShapes.Count;)
			{
				LayoutShape layshape = myLayoutShapes[i];
				if (layshape.Shape is ExternalConstraintShape)
				{
					myConstraintShapes.Add(layshape);
					myLayoutShapes.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		/// <summary>
		/// Places each external constraint shape at the point corresponding to the average of all of its referenced shapes.
		/// Frequency constraints are handled differently, since they apply to only one fact type (but 1 or more roles in that
		/// fact type) at any time.
		/// </summary>
		protected override void PostLayout()
		{
			ResolveReferences(myConstraintShapes);

			foreach (LayoutShape shape in myConstraintShapes)
			{
				PointD avg = new PointD(0, 0);
				bool isFrequencyConstraint = !(shape.Shape is FrequencyConstraintShape);
				int count = shape.Count;	// default for non-frequency constraints

				if (isFrequencyConstraint)
				{
					for (int i = 0; i < shape.Count; i++)
					{
						NodeShape ns = shape.RelatedShapes[i].Shape;
						avg.X += ns.Location.X;
						avg.Y += ns.Location.Y;
					}
				}
				else
				{
					FrequencyConstraintShape freqShape = shape.Shape as FrequencyConstraintShape;
					FrequencyConstraint constraint = freqShape.ModelElement as FrequencyConstraint;
					if (constraint.FactTypeCollection.Count == 0 || constraint.RoleCollection.Count == 0)
					{
						continue;
					}

					FactType factType = constraint.FactTypeCollection[0];
					FactTypeShape factShape = (FactTypeShape)myDiagram.FindShape(factType);
					count = constraint.RoleCollection.Count;
					double width = factShape.Size.Width;
					double height = factShape.Size.Height;

					for (int i = 0; i < count; i++)
					{
						int targetIndex = factType.RoleCollection.IndexOf(constraint.RoleCollection[i]);
						if (factShape.DisplayOrientation == DisplayOrientation.Horizontal)
						{
							avg.X += (width / (targetIndex + 1)) + factShape.Location.X;
							avg.Y += factShape.Location.Y - height;
						}
						else
						{
							avg.X += factShape.Location.X + width;
							avg.Y += (height / (targetIndex + 1)) + factShape.Location.Y;
						}
					}
				}
				if (count != 0)
				{
					avg.X /= count;
					avg.Y /= count;
				}

				shape.Shape.Location = new PointD(avg.X, avg.Y);
			}
			
			// Now add the shapes back into the main myLayoutShape list for reflow
			foreach (LayoutShape shape in myConstraintShapes)
			{
				myLayoutShapes.Add(shape);
			}
		}

		/// <summary>
		/// Perform operations on <paramref name="shape"/> before it is placed on the diagram
		/// </summary>
		/// <param name="shape">A LayoutShape that is being placed on the diagram</param>
		protected override void PreShapePlacement(LayoutShape shape)
		{
			base.PreShapePlacement(shape);
		}

		/// <summary>
		/// This method ensures that the role in the related fact type is closest (spatially) to the related object shape.
		/// </summary>
		/// <param name="shape">The shape that was most recently placed on the diagram</param>
		protected override void PostShapePlacement(LayoutShape shape)
		{
			if (shape.Parent == null || !(shape.Shape is FactTypeShape))
			{
				return;
			}

			FactTypeShape factShape = shape.Shape as FactTypeShape;
			NodeShape objectShape = shape.Parent.Shape;
			ModelElement objectElement = shape.Parent.Shape.ModelElement;
			if (objectElement is FactType)
			{
				Objectification otherElementObj = (objectElement as FactType).Objectification;
				if (otherElementObj != null)
				{
					objectElement = otherElementObj.NestingType;
				}
			}
			FactType factElement = factShape.ModelElement as FactType;
			LinkedElementCollection<RoleBase> roles = factShape.GetEditableDisplayRoleOrder();

			// set the index at which the role will be closest to otherLayoutShape
			int targetIndex = 0;
			if (objectShape.Location.X > shape.TargetLocation.X + (factShape.Size.Width / 2))
			{
				targetIndex = roles.Count - 1;
			}

			// find actual index
			for (int i = 0; i < roles.Count; i++)
			{
				if (roles[i].Role.RolePlayer == objectElement)
				{
					roles.Move(i, targetIndex);
					break;
				}
			}

			myLastPlacedShape = shape;
		}
	}
	#endregion
}
