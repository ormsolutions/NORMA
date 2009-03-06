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
	#region ORMRadialLayoutEngine - actual implementation
	/// <summary>
	/// Performs a standard radial layout
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
		/// <remarks>Any <see cref="LayoutShape"/> where the <see cref="LayoutShape.Placed"/> property is <see langword="true"/>
		/// should be ignored.</remarks>
		public override LayoutShape ResolveReferences(LayoutShapeList list)
		{
			LayoutShape mostChildren = null;
			LayoutShapeList allShapes = myLayoutShapes;
			foreach (LayoutShape layshape in list)
			{
				if (!layshape.Placed)
				{
					layshape.ResolveReferences(allShapes);
					if ((mostChildren == null || layshape.Count > mostChildren.Count) && !(layshape.Shape is FactTypeShape) && !(layshape.Shape is ExternalConstraintShape))
					{
						mostChildren = layshape;
					}
				}
			}
			return mostChildren;
		}

		/// <summary>
		/// Since engines are treated as single-instance, we might need to clear state information between calls
		/// </summary>
		protected override void ClearState()
		{
			myConstraintShapes.Clear();
			base.ClearState();
		}

		/// <summary>
		/// Perform pre-layout tasks
		/// </summary>
		protected override void PreLayout()
		{
			// Separate out the external constraint shapes
			for (int i = myLayoutShapes.Count - 1; i >= 0; --i)
			{
				LayoutShape layshape = myLayoutShapes[i];
				if (layshape.Shape is ExternalConstraintShape)
				{
					myConstraintShapes.Add(layshape);
					myLayoutShapes.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Places each external constraint shape at the point corresponding to the average of all of its referenced shapes.
		/// Frequency constraints are handled differently, since they apply to only one fact type (but 1 or more roles in that
		/// fact type) at any time.
		/// </summary>
		/// <param name="minimumPoint">The minimum location for new element placement</param>
		public override void PostLayout(PointD minimumPoint)
		{
			ResolveReferences(myConstraintShapes);

			foreach (LayoutShape shape in myConstraintShapes)
			{
				if (!shape.Pinned)
				{
					PointD avg = new PointD(0, 0);
					NodeShape nodeShape = shape.Shape;
					FrequencyConstraintShape freqShape;
					FrequencyConstraint constraint;
					LinkedElementCollection<FactType> relatedFactTypes;

					if (null != (freqShape = nodeShape as FrequencyConstraintShape) &&
						null != (constraint = freqShape.ModelElement as FrequencyConstraint) &&
						1 == (relatedFactTypes = constraint.FactTypeCollection).Count)
					{
						Diagram diagram = myDiagram;
						FactType factType = relatedFactTypes[0];
						FactTypeShape factTypeShape = null;
						LayoutShape factTypeLayoutShape = null;
						foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
						{
							FactTypeShape testShape = pel as FactTypeShape;
							if (testShape != null && testShape.Diagram == diagram)
							{
								if (factTypeShape == null)
								{
									factTypeShape = testShape;
								}
								if (myLayoutShapes.TryGetValue(testShape, out factTypeLayoutShape))
								{
									factTypeShape = testShape;
									break;
								}
							}
						}

						LinkedElementCollection<Role> constraintRoles = constraint.RoleCollection;
						LinkedElementCollection<RoleBase> displayOrder = factTypeShape.DisplayedRoleOrder;
						DisplayOrientation orientation = factTypeShape.DisplayOrientation;
						RectangleD shapeBounds = factTypeShape.AbsoluteBounds;
						SizeD shapeSize = factTypeShape.Size;
						PointD location = (factTypeLayoutShape != null) ? factTypeLayoutShape.TargetLocation : shapeBounds.Location;
						int count = constraintRoles.Count;
						double width = shapeSize.Width;
						double height = shapeSize.Height;

						for (int i = 0; i < count; i++)
						{
							int targetIndex = displayOrder.IndexOf(constraintRoles[i]);
							switch (orientation)
							{
								case DisplayOrientation.Horizontal:
									avg.Offset((width / (targetIndex + 1)) + location.X, location.Y - height);
									break;
								case DisplayOrientation.VerticalRotatedRight:
									avg.Offset(location.X + width, (height / (targetIndex + 1)) + location.Y);
									break;
								case DisplayOrientation.VerticalRotatedLeft:
									avg.Offset(location.X + width, height - (height / (targetIndex + 1)) + location.Y);
									break;
							}
						}
						avg.X /= count;
						avg.Y /= count;
					}
					else
					{
						int count = shape.Count;
						for (int i = 0; i < count; ++i)
						{
							PointD location = shape.RelatedShapes[i].TargetLocation;
							avg.Offset(location.X, location.Y);
						}
						avg.X /= count;
						avg.Y /= count;
						// Constraints are frequently ending up directly on top of
						// an ObjectTypeShape, bump them up a bit
						double bumpAdjust = nodeShape.Size.Height * 2;
						avg.Y -= bumpAdjust;
						if (avg.Y < minimumPoint.Y)
						{
							avg.Y += bumpAdjust + bumpAdjust;
						}
					}

					shape.TargetLocation = avg;
				}
				shape.Placed = true;
			}
			
			// Now add the shapes back into the main myLayoutShape list for reflow
			foreach (LayoutShape shape in myConstraintShapes)
			{
				myLayoutShapes.Add(shape);
			}
			myConstraintShapes.Clear();
		}

		/// <summary>
		/// This method ensures that the role in the related fact type is closest (spatially) to the related object shape.
		/// </summary>
		/// <param name="shape">The shape that was most recently placed on the diagram</param>
		protected override void PostShapePlacement(LayoutShape shape)
		{
			FactTypeShape factShape;
			LayoutShape parentShape;
			if (shape.Pinned || null == (parentShape = shape.Parent) || null == (factShape = shape.Shape as FactTypeShape))
			{
				return;
			}

			NodeShape objectShape = parentShape.Shape;
			ModelElement objectElement = parentShape.Shape.ModelElement;
			FactType factElement;
			if (null != (factElement = objectElement as FactType))
			{
				Objectification objectification = factElement.Objectification;
				if (objectification != null)
				{
					objectElement = objectification.NestingType;
				}
			}
			factElement = factShape.ModelElement as FactType;
			LinkedElementCollection<RoleBase> roles = factShape.DisplayedRoleOrder;

			// set the index at which the role will be closest to otherLayoutShape
			int targetIndex = 0;
			LayoutShape objectLayoutShape;
			PointD objectShapeLocation = myLayoutShapes.TryGetValue(objectShape, out objectLayoutShape) ? objectLayoutShape.TargetLocation : objectShape.Location;
			SizeD objectShapeSize = objectShape.Size;
			objectShapeLocation.Offset(objectShapeSize.Width / 2, objectShapeSize.Height / 2);
			if (objectShapeLocation.X > shape.TargetLocation.X + (factShape.Size.Width / 2))
			{
				targetIndex = roles.Count - 1;
			}

			// find actual index
			bool haveEditableOrder = false;
			int roleCount = roles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				if (roles[i].Role.RolePlayer == objectElement)
				{
					if (i == targetIndex)
					{
						if (!haveEditableOrder)
						{
							haveEditableOrder = true;
							roles = factShape.GetEditableDisplayRoleOrder();
						}
						roles.Move(i, targetIndex);
						break;
					}
				}
			}
		}
	}
	#endregion
}
