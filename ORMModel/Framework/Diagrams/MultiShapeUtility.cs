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

// Defining LINKS_ALWAYS_CONNECT allows multiple links from a single ShapeA to different instances of ShapeB.
// In this case, the 'anchor' end is always connected if an opposite shape is available.
// The current behavior is to only create a link if, given an instance of ShapeA, the closest candidate
// ShapeB instance is not closer to a different instance of ShapeA.
// Note that LINKS_ALWAYS_CONNECT is also used in other files, so you should turn this on
// in the project properties if you want to experiment. This is here for reference only.
//#define LINKS_ALWAYS_CONNECT

using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.Modeling.Diagrams
{
	/// <summary>
	/// Provides methods to support multiple <see cref="ShapeElement"/>s for the same <see cref="ModelElement"/>
	/// </summary>
	public static class MultiShapeUtility
	{
		#region Delegates for ShapeElement Methods
		[Serializable]
		private delegate void ConfigureAndPlaceChildShapeDelegate(ShapeElement @this, ShapeElement childShape, ModelElement childElement, bool createdDuringViewFixup);
		private static readonly ConfigureAndPlaceChildShapeDelegate ConfigureAndPlaceChildShape = (ConfigureAndPlaceChildShapeDelegate)
			InitializeDelegate<ConfigureAndPlaceChildShapeDelegate>("ConfigureAndPlaceChildShape", new Type[] { typeof(ShapeElement), typeof(ModelElement), typeof(bool) });
		[Serializable]
		private delegate ShapeElement CreateChildShapeDelegate(ShapeElement @this, ModelElement element);
		private static readonly CreateChildShapeDelegate CreateChildShape = (CreateChildShapeDelegate)
			InitializeDelegate<CreateChildShapeDelegate>("CreateChildShape", new Type[] { typeof(ModelElement) });
		[Serializable]
		private delegate bool ShouldAddShapeForElementDelegate(ShapeElement @this, ModelElement element);
		private static readonly ShouldAddShapeForElementDelegate ShouldAddShapeForElement = (ShouldAddShapeForElementDelegate)
			InitializeDelegate<ShouldAddShapeForElementDelegate>("ShouldAddShapeForElement", new Type[] { typeof(ModelElement) });
		[Serializable]
		private delegate void OnChildConfiguringDelegate(ShapeElement @this, ShapeElement child, bool createdDuringViewFixup);
		private static readonly OnChildConfiguringDelegate OnChildConfiguring = (OnChildConfiguringDelegate)
			InitializeDelegate<OnChildConfiguringDelegate>("OnChildConfiguring", new Type[] { typeof(ShapeElement), typeof(bool) });
		[Serializable]
		private delegate void OnChildConfiguredDelegate(ShapeElement @this, ShapeElement child, bool childWasPlaced, bool createdDuringViewFixup);
		private static readonly OnChildConfiguredDelegate OnChildConfigured = (OnChildConfiguredDelegate)
			InitializeDelegate<OnChildConfiguredDelegate>("OnChildConfigured", new Type[] { typeof(ShapeElement), typeof(bool), typeof(bool) });
		private static Delegate InitializeDelegate<T>(string methodName, Type[] parameterTypes)
		{
			const BindingFlags bindingFlags =
				BindingFlags.DeclaredOnly |
				BindingFlags.ExactBinding |
				BindingFlags.Instance |
				BindingFlags.NonPublic |
				// Technically, we don't need the public flags, but if the method is changed to being public in the future,
				// having it here will allow this to keep working.
				BindingFlags.Public;

			Type shapeElementType = typeof(ShapeElement);
			MethodInfo methodInfo = shapeElementType.GetMethod(methodName, bindingFlags, null, parameterTypes, null);

			if (methodInfo != null)
			{
				return Delegate.CreateDelegate(typeof(T), null, methodInfo, true);
			}
			throw new MissingMethodException(shapeElementType.Name, methodName);
		}
		#endregion //Delegates for ShapeElement Methods
		#region FixUpChildShapes
		/// <summary>
		/// If present in the <see cref="TransactionContext.ContextInfo"/> for the top-level <see cref="Transaction"/>,
		/// <see cref="FixUpChildShapes"/> will add a new <see cref="ShapeElement"/>, even if one already
		/// exists on the <see cref="Diagram"/> for the <see cref="ModelElement"/> being processed, as long as
		/// <see cref="ShapeElement.ShouldAddShapeForElement"/> returns <see langword="true"/>.
		/// </summary>
		public static readonly object AllowMultipleShapes = new object();
		/// <summary>
		/// An alternative to <see cref="ShapeElement.FixUpChildShapes"/> that supports multiple shapes
		/// </summary>
		/// <param name="existingParentShape">The parent <see cref="ShapeElement"/></param>
		/// <param name="childElement">The child <see cref="ModelElement"/></param>
		/// <returns>The child <see cref="ShapeElement"/>, if any</returns>
		public static ShapeElement FixUpChildShapes(ShapeElement existingParentShape, ModelElement childElement)
		{
			if (existingParentShape == null)
			{
				throw new ArgumentNullException("existingParentShape");
			}
			if (childElement == null)
			{
				throw new ArgumentNullException("childElement");
			}

			Store store = existingParentShape.Store;
			if (store == null || !store.TransactionActive)
			{
				// UNDONE: Provide a localized error message for this
				throw new InvalidOperationException();
			}

			Transaction currentTransaction = store.TransactionManager.CurrentTransaction;
			Transaction topLevelTransaction = currentTransaction.TopLevelTransaction;
			bool allowMultipleShapesForChildren = topLevelTransaction.Context.ContextInfo.ContainsKey(AllowMultipleShapes);

			if (allowMultipleShapesForChildren)
			{
				if (!TargetElementIsOnDiagram(topLevelTransaction, existingParentShape))
				{
					return CreateAndConfigureChildShape(existingParentShape, childElement);
				}
			}

			ShapeElement unparentedChildShape = null;
			ShapeElement existingChildShape = null;

			//search for an unparented child shape and an existing child shape
			foreach (PresentationElement childPresentationElement in PresentationViewsSubject.GetPresentation(childElement))
			{
				ShapeElement childShapeElement;
				if ((childShapeElement = childPresentationElement as ShapeElement) != null)
				{
					if (unparentedChildShape == null && childShapeElement.ParentShape == null)
					{
						unparentedChildShape = childShapeElement;
					}
					else if (existingChildShape == null)
					{
						if (childShapeElement.ParentShape == existingParentShape)
						{
							existingChildShape = childShapeElement;
						}
					}
					else if (unparentedChildShape != null)
					{
						break;
					}
				}
			}

			if (allowMultipleShapesForChildren)
			{
				if (unparentedChildShape == null)
				{
					return CreateAndConfigureChildShape(existingParentShape, childElement);
				}
				else
				{
					ConfigureAndPlaceChildShape(existingParentShape, unparentedChildShape, childElement, false);
					return unparentedChildShape;
				}
			}
			else
			{
				if (existingChildShape == null)
				{
					if (unparentedChildShape == null)
					{
						return CreateAndConfigureChildShape(existingParentShape, childElement);
					}
					else
					{
						ConfigureAndPlaceChildShape(existingParentShape, unparentedChildShape, childElement, false);
						return unparentedChildShape;
					}
				}
				else
				{
					//fix up the existing child shape
					OnChildConfiguring(existingParentShape, existingChildShape, false);
					bool childWasPlaced = true;
					if (UnplacedShapesContext.HasUnplacedShapesContext(currentTransaction) && (existingParentShape.Diagram != null))
					{
						childWasPlaced = !UnplacedShapesContext.GetUnplacedShapesMap(currentTransaction, existingParentShape.Diagram.Id).Contains(existingChildShape);
					}
					OnChildConfigured(existingParentShape, existingChildShape, childWasPlaced, false);
					existingChildShape.OnBoundsFixup(BoundsFixupState.ViewFixup, 0, false);

					//fix up grand child shapes
					ICollection GrandChildShapes = existingChildShape.GetChildElements(childElement);
					foreach (ModelElement GrandChildShape in GrandChildShapes)
					{
						existingChildShape.FixUpChildShapes(GrandChildShape);
					}

					return existingChildShape;
				}
			}
		}
		private static bool TargetElementIsOnDiagram(Transaction topLevelTransaction, ShapeElement existingParentShape)
		{
			ShapeElement targetElement;
			return (((targetElement = DesignSurfaceMergeContext.GetTargetElement(topLevelTransaction) as ShapeElement) != null) &&
				(targetElement.Diagram == existingParentShape.Diagram));
		}
		private static ShapeElement CreateAndConfigureChildShape(ShapeElement existingParentShape, ModelElement childElement)
		{
			if (ShouldAddShapeForElement(existingParentShape, childElement))
			{
				ShapeElement newChildShape;
				if ((newChildShape = CreateChildShape(existingParentShape, childElement)) != null)
				{
					ConfigureAndPlaceChildShape(existingParentShape, newChildShape, childElement, true);
				}
				return newChildShape;
			}
			return null;
		}
		#endregion //FixUpChildShapes
		#region FindAllShapesForElement
		/// <summary>
		/// Locate all existing typed shapes on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="diagram">The diagram to search</param>
		/// <param name="element">The element to search</param>
		/// <returns>An IEnumerable for enumeration through all existing shapes</returns>
		public static IEnumerable<TShape> FindAllShapesForElement<TShape>(Diagram diagram, ModelElement element) where TShape : ShapeElement
		{
			foreach (TShape shape in FindAllShapesForElement<TShape>(diagram, element, false))
			{
				yield return shape;
			}
		}
		/// <summary>
		/// Locate all existing typed shapes on this diagram corresponding to this element
		/// </summary>
		/// <typeparam name="TShape">The type of the shape to return</typeparam>
		/// <param name="diagram">The diagram to search</param>
		/// <param name="element">The element to search</param>
		/// <param name="filterDeleting">Do not return an element where the <see cref="ModelElement.IsDeleting"/> property is true</param>
		/// <returns>An IEnumerable for enumeration through all existing shapes</returns>
		public static IEnumerable<TShape> FindAllShapesForElement<TShape>(Diagram diagram, ModelElement element, bool filterDeleting) where TShape : ShapeElement
		{
			if (element != null)
			{
				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(element);
				int pelCount = pels.Count;
				for (int i = pelCount - 1; i >= 0; --i)
				{
					// Walk backwards so we can support shape deletion out of this loop
					TShape shape = pels[i] as TShape;
					if (shape != null && shape.Diagram == diagram && (!filterDeleting || !shape.IsDeleting))
					{
						yield return shape;
					}
				}
			}
		}
		#endregion //FindAllShapesForElement
		#region Link Configuration
		/// <summary>
		/// Determines if the relationship should be visited, and reconfigures any links
		/// </summary>
		/// <param name="walker">The current <see cref="ElementWalker"/></param>
		/// <param name="sourceElement">The <see cref="ModelElement"/> being deleted</param>
		/// <param name="domainRelationshipInfo">The relationship information</param>
		/// <param name="targetRelationship">The other <see cref="ModelElement"/> in the relationship</param>
		/// <returns>Whether to visit the relationship</returns>
		public static bool ShouldVisitOnDelete(ElementWalker walker, ModelElement sourceElement, DomainRelationshipInfo domainRelationshipInfo, ElementLink targetRelationship)
		{
			bool retVal = true;

			ShapeElement originalShape;
			if (walker != null && domainRelationshipInfo != null &&
				(originalShape = sourceElement as ShapeElement) != null)
			{
				foreach (DomainRoleInfo domainRoleInfo in domainRelationshipInfo.DomainRoles)
				{
					ShapeElement otherElement;
					if ((otherElement = domainRoleInfo.GetRolePlayer(targetRelationship) as ShapeElement) != null &&
						otherElement != sourceElement && !walker.Visited(otherElement))
					{
						if (otherElement is IReconfigureableLink)
						{
							CheckLinks(originalShape, true);
							retVal = false;
						}
					}
				}
			}

			return retVal;
		}
		/// <summary>
		/// Check and reconfigure links related to a <see cref="ShapeElement"/> when it moves
		/// </summary>
		/// <param name="originalShape">The shape moved</param>
		public static void CheckLinksOnBoundsChanged(ShapeElement originalShape)
		{
			CheckLinks(originalShape, false);
		}
		private static void CheckLinks(ShapeElement checkShape, bool discludeOriginal)
		{
			ShapeElement originalShape = ResolvePrimaryShape(checkShape);
			ShapeElement discludedShape = discludeOriginal ? originalShape : null;

			if (originalShape != null && originalShape.ModelElement != null)
			{
				List<IReconfigureableLink> reconfigureableLinks = null;
				//check the links for each shape for the model element
				foreach (ShapeElement shape in FindAllShapesForElement<ShapeElement>(originalShape.Diagram, originalShape.ModelElement))
				{
#if LINKS_ALWAYS_CONNECT
					bool shapeIsOriginal = (shape == originalShape);
					foreach (ShapeElement toShape in GetExistingLinks(shape, true, false))
					{
						CheckLink(toShape, shapeIsOriginal, BinaryLinkAnchor.ToShape, discludedShape);
					}
					foreach (ShapeElement fromShape in GetExistingLinks(shape, false, true))
					{
						CheckLink(fromShape, shapeIsOriginal, BinaryLinkAnchor.FromShape, discludedShape);
					}
#else
					foreach (ShapeElement linkShape in GetExistingLinks(shape, true, true, null))
					{
						BinaryLinkShapeBase link;
						if ((link = linkShape as BinaryLinkShapeBase) != null)
						{
							if (link is IEnsureConnectorShapeForLink)
							{
								//this link may have other links connected to it, so check those links as well
								CheckLinks(link, false);
							}
							IReconfigureableLink reconfigureableLink;
							if ((reconfigureableLink = link as IReconfigureableLink) != null)
							{
								if (reconfigureableLinks == null)
								{
									reconfigureableLinks = new List<IReconfigureableLink>();
								}
								reconfigureableLinks.Add(reconfigureableLink);
							}
						}
					}
#endif //LINKS_ALWAYS_CONNECT
				}
				if (reconfigureableLinks != null)
				{
					foreach (IReconfigureableLink link in reconfigureableLinks)
					{
						link.Reconfigure(discludedShape);
					}
				}
			}
		}
#if LINKS_ALWAYS_CONNECT
		private static void CheckLink(ShapeElement linkShape, bool shapeIsOriginal, BinaryLinkAnchor checkAnchor, ShapeElement discludedShape)
		{
			BinaryLinkShapeBase toLink;
			if ((toLink = linkShape as BinaryLinkShapeBase) != null)
			{
				if (linkShape is IEnsureConnectorShapeForLink)
				{
					//this link may have other links connected to it, so check those links as well
					CheckLinks(linkShape, false);
				}
				IReconfigureableLink reconfigureableLink;
				if ((reconfigureableLink = toLink as IReconfigureableLink) != null)
				{
					IBinaryLinkAnchor linkWithAnchor;
					//if this is the anchoring side, only the original shape's links need to be checked
					if (shapeIsOriginal || ((linkWithAnchor = toLink as IBinaryLinkAnchor) != null && linkWithAnchor.Anchor == checkAnchor))
					{
						reconfigureableLink.Reconfigure(discludedShape);
					}
				}
			}
		}
#endif //LINKS_ALWAYS_CONNECT
		/// <summary>
		/// Reconfigure a link to connect the appropriate <see cref="NodeShape"/>
		/// </summary>
		/// <param name="link">The link to reconfigure</param>
		/// <param name="fromElement">The <see cref="ModelElement"/> the link connects from</param>
		/// <param name="toElement">The <see cref="ModelElement"/> the link connects to</param>
		/// <param name="discludedShape">A <see cref="ShapeElement"/> to disclude from potential nodes to connect</param>
		public static void ReconfigureLink(BinaryLinkShapeBase link, ModelElement fromElement, ModelElement toElement, ShapeElement discludedShape)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}

			if (fromElement != null && toElement != null)
			{
				Diagram diagram = link.Diagram;
				if (diagram == null)
				{
					throw new NullReferenceException();
				}

#if LINKS_ALWAYS_CONNECT
				//get the anchoring side, default to the from shape
				IBinaryLinkAnchor linkWithAnchor;
				bool anchorsToFromShape = true;
				if ((linkWithAnchor = link as IBinaryLinkAnchor) != null)
				{
					anchorsToFromShape = (linkWithAnchor.Anchor == BinaryLinkAnchor.FromShape);
				}

				foreach (ShapeElement currentFromShape in FindAllShapesForElement<ShapeElement>(diagram, anchorsToFromShape ? fromElement : toElement, true))
#else
				ShapeElement closestFromShape = null;
				ShapeElement closestToShape = null;
				double minimumDistance = double.MaxValue;

				foreach (ShapeElement currentFromShape in FindAllShapesForElement<ShapeElement>(diagram, fromElement, true))
#endif //LINKS_ALWAYS_CONNECT
				{
					if (discludedShape != null && discludedShape == ResolvePrimaryShape(currentFromShape)
#if LINKS_ALWAYS_CONNECT
						|| AlreadyConnectedTo(currentFromShape, toElement, anchorsToFromShape, link)
#endif //LINKS_ALWAYS_CONNECT
)
					{
						continue;
					}

#if LINKS_ALWAYS_CONNECT
					ShapeElement closestToShape = null;
					double minimumDistance = double.MaxValue;

					foreach (ShapeElement currentToShape in FindAllShapesForElement<ShapeElement>(diagram, anchorsToFromShape ? toElement : fromElement, true))
#else
					double existingDistance = GetExistingConnectionDistance(currentFromShape, toElement, true, link);

					foreach (ShapeElement currentToShape in FindAllShapesForElement<ShapeElement>(diagram, toElement, true))
#endif //LINKS_ALWAYS_CONNECT
					{
						if (discludedShape != null && discludedShape == ResolvePrimaryShape(currentToShape))
						{
							continue;
						}

						double distanceX;
						double distanceY;
						double currentDistance;
						if ((currentDistance = (distanceX = currentFromShape.Center.X - currentToShape.Center.X) * distanceX
							+ (distanceY = currentFromShape.Center.Y - currentToShape.Center.Y) * distanceY) < minimumDistance
#if !LINKS_ALWAYS_CONNECT
 && currentDistance < existingDistance && currentDistance < GetExistingConnectionDistance(currentToShape, fromElement, false, link)
#endif //!LINKS_ALWAYS_CONNECT
)
						{
							minimumDistance = currentDistance;
							closestToShape = currentToShape;
#if !LINKS_ALWAYS_CONNECT
							closestFromShape = currentFromShape;
#endif //!LINKS_ALWAYS_CONNECT
						}
					}
#if LINKS_ALWAYS_CONNECT
					if (closestToShape == null)
					{
						//there are no potential to shapes
						break;
					}
					ShapeElement closestFromShape = currentFromShape;
#else
				}
#endif //LINKS_ALWAYS_CONNECT

				if (closestFromShape != null && closestToShape != null)
				{
					NodeShape toShape;
					NodeShape fromShape;

					IEnsureConnectorShapeForLink ensuresLinkConnectorShape;
					IProvideConnectorShape getsUniqueConnectorShape;

					if ((ensuresLinkConnectorShape = closestToShape as IEnsureConnectorShapeForLink) != null)
					{
						toShape = ensuresLinkConnectorShape.EnsureLinkConnectorShape();
					}
					else
					{
						toShape = closestToShape as NodeShape;
					}
					if ((ensuresLinkConnectorShape = closestFromShape as IEnsureConnectorShapeForLink) != null)
					{
						fromShape = ensuresLinkConnectorShape.EnsureLinkConnectorShape();
					}
					else
					{
						fromShape = closestFromShape as NodeShape;
					}

					NodeShape temp = toShape;
					if ((getsUniqueConnectorShape = closestToShape as IProvideConnectorShape) != null)
					{
						toShape = getsUniqueConnectorShape.GetUniqueConnectorShape(fromShape);
					}
					if ((getsUniqueConnectorShape = closestFromShape as IProvideConnectorShape) != null)
					{
						fromShape = getsUniqueConnectorShape.GetUniqueConnectorShape(temp);
					}

					if (toShape != null && fromShape != null)
					{
#if LINKS_ALWAYS_CONNECT
							if (anchorsToFromShape)
							{
#endif //LINKS_ALWAYS_CONNECT
						//In order to actually re-connect an already connected link, 
						// the properties need to be set AND the connect method called.
						link.FromShape = fromShape;
						link.ToShape = toShape;
						link.Connect(fromShape, toShape);
#if LINKS_ALWAYS_CONNECT
							}
							else
							{
								link.FromShape = toShape;
								link.ToShape = fromShape;
								link.Connect(toShape, fromShape);
							}
#endif //LINKS_ALWAYS_CONNECT
						return;
					}
				}
			}
#if LINKS_ALWAYS_CONNECT
			}
#endif //LINKS_ALWAYS_CONNECT

			//no shapes need to be connected, so delete the link
			link.Delete();
		}
#if LINKS_ALWAYS_CONNECT
		private static bool AlreadyConnectedTo(ShapeElement currentShape, ModelElement oppositeElement, bool isFromShape, BinaryLinkShapeBase currentLink)
#else
		private static double GetExistingConnectionDistance(ShapeElement currentShape, ModelElement oppositeElement, bool isFromShape, BinaryLinkShapeBase currentLink)
#endif //LINKS_ALWAYS_CONNECT
		{
#if  !LINKS_ALWAYS_CONNECT
			double retVal = double.MaxValue;
#endif //!LINKS_ALWAYS_CONNECT
			//check each link to see if it connects to the opposite element
			foreach (ShapeElement linkShape in GetExistingLinks(currentShape, !isFromShape, isFromShape, currentLink.ModelElement))
			{
				//if the link is the one currently being configured, count it as not connected
				if (linkShape == currentLink)
				{
					continue;
				}

				BinaryLinkShapeBase binaryLinkShapeBase;
				if ((binaryLinkShapeBase = linkShape as BinaryLinkShapeBase) != null)
				{
					ShapeElement checkElement;
					NodeShape nodeShape;

					if (isFromShape)
					{
						nodeShape = binaryLinkShapeBase.ToShape;
					}
					else
					{
						nodeShape = binaryLinkShapeBase.FromShape;
					}

					checkElement = ResolvePrimaryShape(nodeShape);

					if (checkElement != null && checkElement.ModelElement == oppositeElement)
					{
#if LINKS_ALWAYS_CONNECT
						return true;
#else
						ShapeElement linkToShape = ResolvePrimaryShape(binaryLinkShapeBase.ToShape);
						ShapeElement linkFromShape = ResolvePrimaryShape(binaryLinkShapeBase.FromShape);

						if (linkFromShape != null && linkToShape != null)
						{
							PointD toShapeCenter = linkToShape.Center;
							PointD fromShapeCenter = linkFromShape.Center;
							double distanceX;
							double distanceY;
							retVal = Math.Min(((distanceX = toShapeCenter.X - fromShapeCenter.X) * distanceX
								+ (distanceY = toShapeCenter.Y - fromShapeCenter.Y) * distanceY), retVal);
						}
#endif //LINKS_ALWAYS_CONNECT
					}
				}
			}
#if LINKS_ALWAYS_CONNECT
			return false;
#else
			return retVal;
#endif //LINKS_ALWAYS_CONNECT
		}
		///// <summary>
		///// Return a new proxy connector ShapeElement, if necessary.  This method will check for 
		///// the IEnsureConnectorShapeForLink and IProvideConnectorShape interfaces.
		///// </summary>
		///// <param name="shape">The shape to resolve.</param>
		///// <param name="toShape">The opposite toShape.  Only necessary if shape implements IProvideConnectorShape.</param>
		///// <returns>The connector NodeShape.</returns>
		//private static NodeShape GetNewConnectorShape(ShapeElement shape, NodeShape toShape)
		//{
		//    NodeShape retVal = null;


		//    return retVal ?? shape as NodeShape;
		//}
		/// <summary>
		/// Gets all existing links for the shape and its proxy connectors.
		/// This method will check for the IEnsureConnectorShapeForLink
		/// and IProvideConnectorShape interfaces.
		/// </summary>
		/// <param name="shape">The shape to check</param>
		/// <param name="getToLinks">True to collect all to role links</param>
		/// <param name="getFromLinks">True to collect all from role links</param>
		/// <param name="linkBackingElement">Only return links that have this element as the <see cref="PresentationElement.ModelElement">ModelElement</see>.</param>
		/// <returns>The proxy connecter</returns>
		private static IEnumerable<ShapeElement> GetExistingLinks(ShapeElement shape, bool getToLinks, bool getFromLinks, ModelElement linkBackingElement)
		{
			Debug.Assert(!(shape is IEnsureConnectorShapeForLink) || !(shape is IProvideConnectorShape),
				"No class should implement both IEnsureConnectorShapeForLink and IProvideConnectorShape.");

			NodeShape nodeShape;
			if ((nodeShape = shape as NodeShape) != null)
			{
				if (getToLinks)
				{
					foreach (ShapeElement sel in nodeShape.ToRoleLinkShapes)
					{
						if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
						{
							yield return sel;
						}
					}
				}
				if (getFromLinks)
				{
					foreach (ShapeElement sel in nodeShape.FromRoleLinkShapes)
					{
						if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
						{
							yield return sel;
						}
					}
				}
			}

			NodeShape alternateConnector;
			IEnsureConnectorShapeForLink ensuresLinkConnectorShape;
			if (shape is IProvideConnectorShape)
			{
				foreach (ShapeElement childShape in shape.RelativeChildShapes)
				{
					if (childShape is IProxyConnectorShape &&
						(alternateConnector = childShape as NodeShape) != null)
					{
						if (getToLinks)
						{
							foreach (ShapeElement sel in alternateConnector.ToRoleLinkShapes)
							{
								if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
								{
									yield return sel;
								}
							}
						}
						if (getFromLinks)
						{
							foreach (ShapeElement sel in alternateConnector.FromRoleLinkShapes)
							{
								if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
								{
									yield return sel;
								}
							}
						}
					}
				}
			}
			else if ((ensuresLinkConnectorShape = shape as IEnsureConnectorShapeForLink) != null)
			{
				//Note that this assumes EnsureLinkConnectorShape will 
				//always return the same NodeShape with subsequent calls
				alternateConnector = ensuresLinkConnectorShape.EnsureLinkConnectorShape();
				if (getToLinks)
				{
					foreach (ShapeElement sel in alternateConnector.ToRoleLinkShapes)
					{
						if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
						{
							yield return sel;
						}
					}
				}
				if (getFromLinks)
				{
					foreach (ShapeElement sel in alternateConnector.FromRoleLinkShapes)
					{
						if (linkBackingElement == null || sel.ModelElement == linkBackingElement)
						{
							yield return sel;
						}
					}
				}
			}
		}
		/// <summary>
		/// Return the primary ShapeElement associated with the given shape.
		/// The shape should either be a primary shape or an IProxyConnectorShape
		/// associated with a primary shape.
		/// </summary>
		/// <param name="shape">The shape to resolve.</param>
		/// <returns>The primary shape.</returns>
		private static ShapeElement ResolvePrimaryShape(ShapeElement shape)
		{
			ShapeElement retVal;
			IProxyConnectorShape proxy;
			if ((proxy = (retVal = shape) as IProxyConnectorShape) != null)
			{
				if ((retVal = proxy.ProxyConnectorShapeFor) == null)
				{
					retVal = shape.ParentShape;
				}
			}
			return retVal ?? shape;
		}
		#endregion //Link Configuration
	}

	#region Interfaces
	/// <summary>
	/// Represents a link that can be reconfigured, supporting multiple shapes
	/// </summary>
	public interface IReconfigureableLink
	{
		/// <summary>
		/// Reconfigure this link to connect the appropriate <see cref="NodeShape"/>s
		/// </summary>
		/// <param name="discludedShape">A <see cref="ShapeElement"/> to disclude from potential nodes to connect</param>
		void Reconfigure(ShapeElement discludedShape);
	}
	/// <summary>
	/// Support the creation of place holder shape objects
	/// that are used to connect the same two shapes multiple
	/// times without link display ambiguity.
	/// </summary>
	public interface IProxyConnectorShape
	{
		/// <summary>
		/// Return another shape that for which this shape is
		/// acting as a proxy connector.
		/// </summary>
		NodeShape ProxyConnectorShapeFor { get;}
	}
	/// <summary>
	/// Represents a shape that needs to get unique connector shapes for links connecting to the same opposite shape
	/// </summary>
	public interface IProvideConnectorShape
	{
		/// <summary>
		/// Helper function to get a shape that is not currently
		/// attached to the oppositeShape via any other LinkShape
		/// objects. This allows multiple links between the same two
		/// objects to be calculated without ambiguity.
		/// </summary>
		/// <param name="oppositeShape">The opposite shape to get a unique connector for</param>
		/// <returns>NodeShape</returns>
		NodeShape GetUniqueConnectorShape(NodeShape oppositeShape);
	}
	/// <summary>
	/// Represents a link that needs to ensure a connector shape for other links to connect to
	/// </summary>
	public interface IEnsureConnectorShapeForLink
	{
		/// <summary>
		/// For a link, gets the child <see cref="NodeShape"/> that other links should connect to
		/// </summary>
		/// <returns>The child <see cref="NodeShape"/></returns>
		NodeShape EnsureLinkConnectorShape();
	}
#if LINKS_ALWAYS_CONNECT
	/// <summary>
	/// Controls which side a binary link is anchored to
	/// </summary>
	public enum BinaryLinkAnchor
	{
		/// <summary>
		/// Anchor to the FromShape
		/// </summary>
		FromShape = 0x0,
		/// <summary>
		/// Anchor to the ToShape
		/// </summary>
		ToShape = 0x1
	}
	/// <summary>
	/// A binary link that is anchored to one side or the other
	/// </summary>
	public interface IBinaryLinkAnchor
	{
		/// <summary>
		/// Gets whether this link is anchored to its ToShape or FromShape
		/// </summary>
		BinaryLinkAnchor Anchor { get;}
	}
#endif //LINKS_ALWAYS_CONNECT
	#endregion //Interfaces
}
