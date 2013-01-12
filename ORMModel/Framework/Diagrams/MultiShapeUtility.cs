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
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams
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
		/// <see cref="FixUpChildShapes"/> will allow addition of a new <see cref="ShapeElement"/>, even if one already
		/// exists on the <see cref="Diagram"/> for the <see cref="ModelElement"/> being processed, as long as
		/// <see cref="ShapeElement.ShouldAddShapeForElement"/> returns <see langword="true"/>. Generally, this is
		/// used during rule processing where additional shapes are allowed by not required. To force a new shape
		/// to be created use the <see cref="ForceMultipleShapes"/> flag instead.
		/// </summary>
		public static readonly object AllowMultipleShapes = new object();
		/// <summary>
		/// If present in the <see cref="TransactionContext.ContextInfo"/> for the top-level <see cref="Transaction"/>,
		/// <see cref="FixUpChildShapes"/> will always a new <see cref="ShapeElement"/>, even if one already
		/// exists on the <see cref="Diagram"/> for the <see cref="ModelElement"/> being processed, as long as
		/// <see cref="ShapeElement.ShouldAddShapeForElement"/> returns <see langword="true"/>.
		/// </summary>
		public static readonly object ForceMultipleShapes = new object();
		/// <summary>
		/// An alternative to <see cref="ShapeElement.FixUpChildShapes"/> that supports multiple shapes
		/// </summary>
		/// <param name="existingParentShape">The parent <see cref="ShapeElement"/></param>
		/// <param name="childElement">The child <see cref="ModelElement"/></param>
		/// <param name="linkShapeType">The expected type for the generated link. If more than one
		/// shape element can exist at a given time for the same child element then this should be
		/// called for each type. Note that this does not affect which type of element is created.
		/// The type of element to create must be control independently of this call, generally with
		/// a context key to disambiguate the shape creation routine.</param>
		/// <returns>The child <see cref="ShapeElement"/>, if any</returns>
		public static ShapeElement FixUpChildShapes(ShapeElement existingParentShape, ModelElement childElement, Type linkShapeType)
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
			bool forceMultipleShapesForChildren;
			bool allowMultipleShapesForChildren;
			if (!(childElement is IReconfigureableLink))
			{
				Dictionary<object, object> contextInfo = topLevelTransaction.Context.ContextInfo;
				forceMultipleShapesForChildren = contextInfo.ContainsKey(ForceMultipleShapes);
				allowMultipleShapesForChildren = forceMultipleShapesForChildren || contextInfo.ContainsKey(AllowMultipleShapes);
			}
			else
			{
				allowMultipleShapesForChildren = forceMultipleShapesForChildren = false;
			}

			if (allowMultipleShapesForChildren)
			{
				if (!TargetElementIsOnMergeContextDiagram(topLevelTransaction, existingParentShape))
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
				if ((childShapeElement = childPresentationElement as ShapeElement) != null &&
					(null == linkShapeType || childShapeElement.GetType() == linkShapeType))
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

			if (forceMultipleShapesForChildren ||
				(allowMultipleShapesForChildren &&
				(existingChildShape == null || existingChildShape is LinkShape || existingChildShape.ParentShape != existingParentShape)))
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
					ICollection grandChildShapes = existingChildShape.GetChildElements(childElement);
					foreach (ModelElement grandChildShape in grandChildShapes)
					{
						existingChildShape.FixUpChildShapes(grandChildShape);
					}

					return existingChildShape;
				}
			}
		}
		/// <summary>
		/// Test if there is a current merge context shape and if it is on the same
		/// diagram as the existing shape. Returns true if there is no merge context target.
		/// </summary>
		/// <returns></returns>
		private static bool TargetElementIsOnMergeContextDiagram(Transaction topLevelTransaction, ShapeElement existingParentShape)
		{
			ModelElement targetElement;
			ShapeElement targetShape;
			return null == (targetElement = DesignSurfaceMergeContext.GetTargetElement(topLevelTransaction)) ||
				(null != (targetShape = targetElement as ShapeElement) &&
				targetShape.Diagram == existingParentShape.Diagram);
		}
		private static ShapeElement CreateAndConfigureChildShape(ShapeElement existingParentShape, ModelElement childElement)
		{
			if (ShouldAddShapeForElement(existingParentShape, childElement))
			{
				ShapeElement newChildShape;
				if ((newChildShape = CreateChildShape(existingParentShape, childElement)) != null)
				{
					ConfigureAndPlaceChildShape(existingParentShape, newChildShape, childElement, true);
					if (newChildShape.IsDeleted)
					{
						newChildShape = null;
					}
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
		#region GetEffectiveAttachedLinkShapes variants
		/// <summary>
		/// Get all link shapes of a given type attached to the specified <paramref name="shape"/>
		/// or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="LinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapes<LinkShapeType>(ShapeElement shape) where LinkShapeType : LinkShape
		{
			NodeShape nodeShape = shape as NodeShape;
			if (nodeShape != null)
			{
				foreach (LinkShape link in LinkConnectsToNode.GetLink(nodeShape))
				{
					LinkShapeType linkShape = link as LinkShapeType;
					if (linkShape != null)
					{
						yield return linkShape;
					}
				}
			}
			if (shape is IProvideConnectorShape)
			{
				foreach (ShapeElement child in shape.RelativeChildShapes)
				{
					NodeShape childShape;
					IProxyConnectorShape proxy;
					if (null != (childShape = child as NodeShape) &&
						null != (proxy = childShape as IProxyConnectorShape))
					{
						Debug.Assert(proxy.ProxyConnectorShapeFor == shape);
						foreach (LinkShape link in LinkConnectsToNode.GetLink(childShape))
						{
							LinkShapeType linkShape = link as LinkShapeType;
							if (linkShape != null)
							{
								yield return linkShape;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Get all link shapes of a given type attached to the specified <paramref name="shape"/>
		/// or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="LinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <param name="snapshot">The caller may change the resulting set, return a snapshot copy of the current data. Default false.</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapes<LinkShapeType>(ShapeElement shape, bool snapshot) where LinkShapeType : LinkShape
		{
			IEnumerable<LinkShapeType> retVal = GetEffectiveAttachedLinkShapes<LinkShapeType>(shape);
			return snapshot ? new List<LinkShapeType>(retVal) : retVal;
		}
		/// <summary>
		/// Get all <see cref="BinaryLinkShape">binary link shapes</see> of a given type originating from
		/// the specified <paramref name="shape"/> or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="LinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapesFrom<LinkShapeType>(ShapeElement shape) where LinkShapeType : BinaryLinkShape
		{
			NodeShape nodeShape = shape as NodeShape;
			if (nodeShape != null)
			{
				foreach (LinkShape link in LinkConnectsToNode.GetLink(nodeShape))
				{
					LinkShapeType linkShape = link as LinkShapeType;
					if (linkShape != null && linkShape.FromShape == shape)
					{
						yield return linkShape;
					}
				}
			}
			if (shape is IProvideConnectorShape)
			{
				foreach (ShapeElement child in shape.RelativeChildShapes)
				{
					NodeShape childShape;
					IProxyConnectorShape proxy;
					if (null != (childShape = child as NodeShape) &&
						null != (proxy = childShape as IProxyConnectorShape))
					{
						Debug.Assert(proxy.ProxyConnectorShapeFor == shape);
						foreach (LinkShape link in LinkConnectsToNode.GetLink(childShape))
						{
							LinkShapeType linkShape = link as LinkShapeType;
							if (linkShape != null && linkShape.FromShape == childShape)
							{
								yield return linkShape;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Get all <see cref="BinaryLinkShape">binary link shapes</see> of a given type originating from
		/// the specified <paramref name="shape"/> or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="LinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <param name="snapshot">The caller may change the resulting set, return a snapshot copy of the current data. Default false.</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapesFrom<LinkShapeType>(ShapeElement shape, bool snapshot) where LinkShapeType : BinaryLinkShape
		{
			IEnumerable<LinkShapeType> retVal = GetEffectiveAttachedLinkShapesFrom<LinkShapeType>(shape);
			return snapshot ? new List<LinkShapeType>(retVal) : retVal;
		}
		/// <summary>
		/// Get all <see cref="BinaryLinkShape">binary link shapes</see> of a given type going to
		/// the specified <paramref name="shape"/> or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="BinaryLinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapesTo<LinkShapeType>(ShapeElement shape) where LinkShapeType : BinaryLinkShape
		{
			NodeShape nodeShape = shape as NodeShape;
			if (nodeShape != null)
			{
				foreach (LinkShape link in LinkConnectsToNode.GetLink(nodeShape))
				{
					LinkShapeType linkShape = link as LinkShapeType;
					if (linkShape != null && linkShape.ToShape == shape)
					{
						yield return linkShape;
					}
				}
			}
			if (shape is IProvideConnectorShape)
			{
				foreach (ShapeElement child in shape.RelativeChildShapes)
				{
					NodeShape childShape;
					IProxyConnectorShape proxy;
					if (null != (childShape = child as NodeShape) &&
						null != (proxy = childShape as IProxyConnectorShape))
					{
						Debug.Assert(proxy.ProxyConnectorShapeFor == shape);
						foreach (LinkShape link in LinkConnectsToNode.GetLink(childShape))
						{
							LinkShapeType linkShape = link as LinkShapeType;
							if (linkShape != null && linkShape.ToShape == childShape)
							{
								yield return linkShape;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Get all <see cref="BinaryLinkShape">binary link shapes</see> of a given type going to
		/// the specified <paramref name="shape"/> or to any relative child shapes that implement <see cref="IProxyConnectorShape"/>.
		/// </summary>
		/// <typeparam name="LinkShapeType">A link shape type derived from <see cref="BinaryLinkShape"/></typeparam>
		/// <param name="shape">The shape to retrieve links for</param>
		/// <param name="snapshot">The caller may change the resulting set, return a snapshot copy of the current data. Default false.</param>
		/// <returns><see cref="IEnumerable{LinkShapeType}"/></returns>
		public static IEnumerable<LinkShapeType> GetEffectiveAttachedLinkShapesTo<LinkShapeType>(ShapeElement shape, bool snapshot) where LinkShapeType : BinaryLinkShape
		{
			IEnumerable<LinkShapeType> retVal = GetEffectiveAttachedLinkShapesTo<LinkShapeType>(shape);
			return snapshot ? new List<LinkShapeType>(retVal) : retVal;
		}
		#endregion // GetEffectiveAttachedLinkShapes variants
		#region Link Configuration
		private static readonly object DeletingSubjectsKey = new object();
		/// <summary>
		/// Determines if the relationship should be visited, and reconfigures any links
		/// </summary>
		/// <param name="walker">The current <see cref="ElementWalker"/></param>
		/// <param name="sourceElement">The <see cref="ModelElement"/> being deleted</param>
		/// <param name="sourceRoleInfo">The role information</param>
		/// <param name="domainRelationshipInfo">The relationship information</param>
		/// <param name="targetRelationship">The other <see cref="ModelElement"/> in the relationship</param>
		/// <returns>Whether to visit the relationship</returns>
		public static bool ShouldVisitOnDelete(ElementWalker walker, ModelElement sourceElement, DomainRoleInfo sourceRoleInfo, DomainRelationshipInfo domainRelationshipInfo, ElementLink targetRelationship)
		{
			bool retVal = true;
			Dictionary<object, object> contextDictionary;
			object subjectsDictionaryObject;
			Dictionary<ModelElement, object> deletingSubjectsDictionary;
			ShapeElement primaryShape;
			object subjectsKey;
			ModelElement subject;


			NodeShape sourceShape;
			IReconfigureableLink reconfigurableLink;
			if (walker != null)
			{
				if (sourceRoleInfo != null &&
					sourceRoleInfo.Id == PresentationViewsSubject.SubjectDomainRoleId)
				{
					contextDictionary = sourceElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
					if (contextDictionary.TryGetValue(subjectsKey = DeletingSubjectsKey, out subjectsDictionaryObject))
					{
						deletingSubjectsDictionary = (Dictionary<ModelElement, object>)subjectsDictionaryObject;
					}
					else
					{
						contextDictionary[subjectsKey] = deletingSubjectsDictionary = new Dictionary<ModelElement, object>();
					}
					deletingSubjectsDictionary[sourceElement] = null;
				}
				else if (domainRelationshipInfo != null &&
					domainRelationshipInfo.Id == LinkConnectsToNode.DomainClassId &&
					null != (sourceShape = sourceElement as NodeShape) &&
					null != (reconfigurableLink = (targetRelationship as LinkConnectsToNode).Link as IReconfigureableLink) &&
					null != (primaryShape = ResolvePrimaryShape(sourceShape)) &&
					(!sourceElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(DeletingSubjectsKey, out subjectsDictionaryObject) ||
					(null != (subject = primaryShape.ModelElement) &&
					!((Dictionary<ModelElement, object>)subjectsDictionaryObject).ContainsKey(subject))))
				{
					reconfigurableLink.Reconfigure(primaryShape);
					retVal = false;
				}
			}

			return retVal;
		}
		/// <summary>
		/// Remove cached data from a top-level transaction context dictionary.
		/// </summary>
		/// <param name="contextDictionary">The dictionary to clear.</param>
		public static void ClearCachedContextInfo(Dictionary<object, object> contextDictionary)
		{
			object key = DeletingSubjectsKey;
			if (contextDictionary.ContainsKey(key))
			{
				contextDictionary.Remove(key);
			}
		}
		/// <summary>
		/// The <see cref="IConfigureableLinkEndpoint.CanAttachLink"/> results have changed.
		/// </summary>
		/// <param name="shapeElement"></param>
		public static void AttachLinkConfigurationChanged(ShapeElement shapeElement)
		{
			CheckLinks(shapeElement, false);
		}
		/// <summary>
		/// Check and reconfigure links related to a <see cref="NodeShape"/> when it moves. Called
		/// from the ChangeRule of any <see cref="NodeShape"/> that supports multiple shapes at
		/// TopLevelCommit with priority <see cref="DiagramFixupConstants.AddConnectionRulePriority"/>
		/// </summary>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs">event args</see> from a
		/// <see cref="ChangeRule"/></param>
		public static void CheckLinksOnBoundsChange(ElementPropertyChangedEventArgs e)
		{
			NodeShape shapeElement;
			if (e.DomainProperty.Id == NodeShape.AbsoluteBoundsDomainPropertyId &&
				(shapeElement = e.ModelElement as NodeShape) != null &&
				!shapeElement.IsDeleted &&
				!(shapeElement is IProxyConnectorShape) &&
				// This is fired on TopLevelCommit, so the bounds may have
				// changed multiple times. Only use the latest.
				((RectangleD)e.NewValue == shapeElement.AbsoluteBounds))
			{
				CheckLinks(shapeElement, false);
			}
		}
		/// <summary>
		/// Detach all links from a shape in preparation for deletion
		/// of the element.
		/// </summary>
		/// <param name="shapeElement">The shape to detach from.</param>
		public static void DetachLinks(ShapeElement shapeElement)
		{
			CheckLinks(shapeElement, true);
		}
		/// <summary>
		/// Helper for <see cref="CheckLinks"/> to key links off of
		/// the backing ModelElement and the type of link shape.
		/// </summary>
		[DebuggerStepThrough]
		private struct LinkTypeKey : IEquatable<LinkTypeKey>
		{
			private Type myLinkType;
			private ModelElement myBackingElement;
			/// <summary>
			/// Create a new LinkTypeKey structure with a default priority
			/// </summary>
			/// <param name="linkType">The type of the link shape.</param>
			/// <param name="backingElement">The backing element for the link shape.</param>
			public LinkTypeKey(Type linkType, ModelElement backingElement)
			{
				myLinkType = linkType;
				myBackingElement = backingElement;
			}
			/// <summary>See <see cref="Object.GetHashCode()"/>.</summary>
			public override int GetHashCode()
			{
				return Utility.GetCombinedHashCode(
					myLinkType.GetHashCode(),
					myBackingElement.GetHashCode());
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is LinkTypeKey && this.Equals((LinkTypeKey)obj);
			}
			/// <summary>See <see cref="IEquatable{LinkTypeKey}.Equals"/>.</summary>
			public bool Equals(LinkTypeKey other)
			{
				return myLinkType == other.myLinkType && myBackingElement == other.myBackingElement;
			}
		}
		private static void CheckLinks(ShapeElement checkShape, bool discludeOriginal)
		{

			ShapeElement originalShape = ResolvePrimaryShape(checkShape);
			ShapeElement discludedShape = discludeOriginal ? originalShape : null;
			ModelElement element;

			if (null != originalShape &&
				null != (element = originalShape.ModelElement))
			{
				//check the links for each shape for the model element
				Diagram diagram = originalShape.Diagram;
				Dictionary<LinkTypeKey, IReconfigureableLink> reconfigureableLinks = null;
				foreach (ShapeElement shape in FindAllShapesForElement<ShapeElement>(diagram, element))
				{
					foreach (BinaryLinkShape linkShape in GetExistingLinks(shape, true, true, null, true))
					{
						if (linkShape is IProvideConnectorShape)
						{
							// UNDONE: MULTISHAPE. Recursion should happen inside Reconfigure
							//this link may have other links connected to it, so check those links as well
							CheckLinks(linkShape, false);
						}
						IReconfigureableLink reconfigureableLink;
						if ((reconfigureableLink = linkShape as IReconfigureableLink) != null)
						{
							if (reconfigureableLinks == null)
							{
								reconfigureableLinks = new Dictionary<LinkTypeKey, IReconfigureableLink>();
							}
							reconfigureableLinks[new LinkTypeKey(linkShape.GetType(), linkShape.ModelElement)] = reconfigureableLink;
						}
					}
				}
				if (reconfigureableLinks != null)
				{
					foreach (IReconfigureableLink link in reconfigureableLinks.Values)
					{
						link.Reconfigure(discludedShape);
					}
				}
				IConfigureableLinkEndpoint configurableEndpoint = originalShape as IConfigureableLinkEndpoint;
				if (configurableEndpoint != null)
				{
					configurableEndpoint.FixupUnattachedLinkElements(diagram);
				}
			}
		}
		/// <summary>
		/// Reconfigure a link to connect the appropriate <see cref="NodeShape"/>
		/// </summary>
		/// <param name="link">The link to reconfigure</param>
		/// <param name="fromElement">The <see cref="ModelElement"/> the link connects from</param>
		/// <param name="toElement">The <see cref="ModelElement"/> the link connects to</param>
		/// <param name="discludedShape">A <see cref="ShapeElement"/> to disclude from potential nodes to connect</param>
		public static void ReconfigureLink(BinaryLinkShape link, ModelElement fromElement, ModelElement toElement, ShapeElement discludedShape)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			Store store = link.Store;
			if (store.InUndoRedoOrRollback)
			{
				return;
			}
			Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			if (IsSecondaryLinkReconfigureBlocked(contextInfo, link))
			{
				return;
			}

			if (fromElement != null && toElement != null)
			{
				ModelElement backingLink;
				Diagram diagram;
				ShapeElement parentShape;
				if (null == (backingLink = link.ModelElement) ||
					null == (parentShape = link.ParentShape) ||
					// Note that getting the diagram is a recursive call with the parent shape as the first step 
					null == (diagram = parentShape.Diagram))
				{
					throw new NullReferenceException();
				}

				bool originalLinkProcessed = false;
				Type linkType = link.GetType();

				foreach (ShapeElement currentFromShapeIter in FindAllShapesForElement<ShapeElement>(diagram, fromElement, true))
				{
					ShapeElement currentFromShape = ResolvePrimaryShape(currentFromShapeIter);
					if (discludedShape != null && discludedShape == currentFromShape)
					{
						continue;
					}
					IConfigureableLinkEndpoint configurableFromEndpoint;
					bool detachFromShape = null != (configurableFromEndpoint = currentFromShape as IConfigureableLinkEndpoint) &&
						AttachLinkResult.Attach != configurableFromEndpoint.CanAttachLink(backingLink, false);
					bool closerFromShapeBlocking;
					ShapeElement closerFromShape;
					ShapeElement closestToShape;
					if (FindNearestShapeForElement(diagram, currentFromShape, toElement, backingLink, discludedShape, detachFromShape, out closestToShape, out closerFromShape, out closerFromShapeBlocking))
					{
						BinaryLinkShape connectLink = null;
						ShapeElement connectFromShape = null;
						ShapeElement connectToShape = null;
						BinaryLinkShape existingLink = null;
						BinaryLinkShape pendingDeleteLinkShape = null;
						foreach (BinaryLinkShape linkShape in GetEffectiveAttachedLinkShapesTo<BinaryLinkShape>(closestToShape))
						{
							if (linkShape.ModelElement == backingLink &&
								linkShape.GetType() == linkType)
							{
								// Note that there can only be one of these satisfying the criteria
								existingLink = linkShape;
								break;
							}
						}

						if (existingLink != null)
						{
							ShapeElement resolvedFromShape = ResolvePrimaryShape(existingLink.FromShape);
							bool testDeleteLinksOnCurrentFromShape = false;
							if (closerFromShapeBlocking)
							{
								pendingDeleteLinkShape = existingLink;
								if (existingLink == link)
								{
									link = null;
								}
							}
							else if (resolvedFromShape == currentFromShape)
							{
								// If the closerFromShape is null, then we're correctly attached for this link.
								// If not, then move the link to the closer from shape unless the closer from
								// shape is already attached for this link. Note that the closerFromShape may
								// be reevaluated later in the loop and the link could move a second time, but
								// this rare possibility is better than doing recursive analysis here and will
								// save existing links.
								if (closerFromShape != null)
								{
									foreach (BinaryLinkShape linkShape in GetEffectiveAttachedLinkShapesFrom<BinaryLinkShape>(closerFromShape))
									{
										if (linkShape.ModelElement == backingLink)
										{
											if (detachFromShape)
											{
												// Leave the existing link alone
												testDeleteLinksOnCurrentFromShape = true;
											}
											else
											{
												pendingDeleteLinkShape = linkShape;
												if (linkShape == link)
												{
													link = null;
												}
											}
											break;
										}
									}
									if (!testDeleteLinksOnCurrentFromShape)
									{
										// Instead of deleting, move the existing link
										connectLink = existingLink;
										connectFromShape = closerFromShape;
										connectToShape = closestToShape;
										if (existingLink == link)
										{
											originalLinkProcessed = true;
										}
									}
								}
								else if (detachFromShape)
								{
									testDeleteLinksOnCurrentFromShape = true;
								}
								else if (existingLink == link)
								{
									originalLinkProcessed = true;
								}
							}
							else if (closerFromShape != null || detachFromShape)
							{
								// No links needed on current from shape
								testDeleteLinksOnCurrentFromShape = true;
							}
							else
							{
								// Move this link to the current from shape after blowing away
								// the links currently on the from shape
								connectLink = existingLink;
								connectFromShape = currentFromShape;
								connectToShape = closestToShape;
								testDeleteLinksOnCurrentFromShape = true;
								if (existingLink == link)
								{
									originalLinkProcessed = true;
								}
							}
							if (testDeleteLinksOnCurrentFromShape)
							{
								foreach (BinaryLinkShape linkShape in GetEffectiveAttachedLinkShapesFrom<BinaryLinkShape>(currentFromShape))
								{
									if (linkShape.ModelElement == backingLink)
									{
										pendingDeleteLinkShape = linkShape;
										if (linkShape == link)
										{
											link = null;
										}
										break;
									}
								}
							}
						}
						else
						{
							// The to shape does not have a link on it. Check if there is an
							// existing one on the from shape we need to delete or move to the
							// to shape.
							foreach (BinaryLinkShape linkShape in GetEffectiveAttachedLinkShapesFrom<BinaryLinkShape>(currentFromShape))
							{
								if (linkShape.ModelElement == backingLink &&
									linkShape.GetType() == linkType)
								{
									// Note that there can only be one of these satisfying the criteria
									existingLink = linkShape;
									break;
								}
							}
							if (existingLink != null)
							{
								Debug.Assert(ResolvePrimaryShape(existingLink.ToShape) != closestToShape, "The link would also have been attached to the to shape");
								if (closerFromShapeBlocking)
								{
									pendingDeleteLinkShape = existingLink;
									if (existingLink == link)
									{
										link = null;
									}
								}
								else if (closerFromShape != null)
								{
									// If the closer from shape is not already attached to any link, then attempt to preserve this
									// link by moving it to the closer from shape. Note that the link will not be moved if the
									// closer from shape has already been processed in the outer loop, and may still be deleted
									// in the future when the primary processing for the closer shape occurs. This gives us the
									// best chance of preserving the link.
									foreach (BinaryLinkShape linkShape in GetEffectiveAttachedLinkShapesFrom<BinaryLinkShape>(closerFromShape))
									{
										if (linkShape.ModelElement == backingLink)
										{
											pendingDeleteLinkShape = existingLink;
											if (existingLink == link)
											{
												link = null;
											}
											break;
										}
									}
									connectLink = existingLink;
									connectToShape = closestToShape;
									connectFromShape = closerFromShape;
									if (existingLink == link)
									{
										originalLinkProcessed = true;
									}
								}
								else
								{
									connectLink = existingLink;
									connectToShape = closestToShape;
									connectFromShape = currentFromShape;
									if (existingLink == link)
									{
										originalLinkProcessed = true;
									}
								}
							}
							else if (closerFromShape == null && !detachFromShape)
							{
								connectFromShape = currentFromShape;
								connectToShape = closestToShape;
							}
						}

						// If we have a connect to make or adjust, do it now.
						if (connectFromShape != null && connectToShape != null)
						{
							if (pendingDeleteLinkShape != null)
							{
								// Disconnect pending deletes without propagating so that reconnects
								// can use existing connector shapes on the connected nodes. Full deletion
								// of the link postponed until after reconnect so that links to links can be maintained.
								foreach (LinkConnectsToNode disconnectLink in LinkConnectsToNode.GetLinksToNodes(pendingDeleteLinkShape))
								{
									disconnectLink.Delete(LinkConnectsToNode.LinkDomainRoleId);
								}
							}

							// The first step is to get a link. If the connectLink is
							// provided then use it. Otherwise, if the passed in link is
							// pending delete or not connected then use it. Otherwise, use
							// information from the passed in link to create a new one.
							if (connectLink == null)
							{
								if (link != null &&
									link.FromShape == null &&
									link.ToShape == null)
								{
									connectLink = link;
									link = null;
								}
								if (connectLink == null)
								{
									if ((connectLink = (BinaryLinkShape)CreateChildShape(parentShape, backingLink)) != null)
									{
										BlockSecondaryLinkReconfigure(contextInfo, connectLink);
										ConfigureAndPlaceChildShape(parentShape, connectLink, backingLink, false);
										UnblockSecondaryLinkReconfigure(contextInfo, connectLink);
									}
								}
							}
							else if (connectLink == link)
							{
								link = null;
							}
							NodeShape toShape = null;
							NodeShape fromShape = null;
							IProvideConnectorShape getsUniqueConnectorShape;

							if ((getsUniqueConnectorShape = connectToShape as IProvideConnectorShape) != null)
							{
								toShape = getsUniqueConnectorShape.GetUniqueConnectorShape(fromShape, backingLink);
							}
							if (toShape == null)
							{
								toShape = connectToShape as NodeShape;
							}
							if ((getsUniqueConnectorShape = connectFromShape as IProvideConnectorShape) != null)
							{
								fromShape = getsUniqueConnectorShape.GetUniqueConnectorShape(connectToShape, backingLink);
							}
							if (fromShape == null)
							{
								fromShape = connectFromShape as NodeShape;
							}

							if (toShape != null && fromShape != null)
							{
								bool changedFromShape = connectLink.FromShape != fromShape;
								bool changedToShape = connectLink.ToShape != toShape;
								if (changedFromShape || changedToShape)
								{
									//In order to actually re-connect an already connected link, 
									// the properties need to be set AND the connect method called.
									if (changedFromShape)
									{
										connectLink.FromShape = fromShape;
									}
									if (changedToShape)
									{
										connectLink.ToShape = toShape;
									}
									connectLink.Connect(fromShape, toShape);
								}
							}
						}
						if (pendingDeleteLinkShape != null)
						{
							if (pendingDeleteLinkShape is IProvideConnectorShape)
							{
								// A link could be attached to this link. Make sure the
								// link is moved to a new location or cleanly deleted before
								// the link itself is deleted.
								foreach (BinaryLinkShape recursiveLink in GetExistingLinks(pendingDeleteLinkShape, true, true, null, true))
								{
									IReconfigureableLink recurseReconfigureLink = recursiveLink as IReconfigureableLink;
									if (recurseReconfigureLink != null)
									{
										recurseReconfigureLink.Reconfigure(pendingDeleteLinkShape);
									}
								}
							}
							pendingDeleteLinkShape.Delete();
						}
					}
				}
				if (link != null && !originalLinkProcessed)
				{
					// no shapes need to be connected, so delete the link. Disconnecting
					// it first tells ShouldVisitOnDelete to not attempt to reconfigure
					// this link.
					foreach (LinkConnectsToNode disconnectLink in LinkConnectsToNode.GetLinksToNodes(link))
					{
						disconnectLink.Delete(LinkConnectsToNode.LinkDomainRoleId);
					}
					link.Delete();
				}
			}
		}
		/// <summary>
		/// Find the nearest shape on this diagram relative to another shape.
		/// </summary>
		/// <param name="diagram">The <see cref="Diagram"/> to search. Populated automaticatlly
		/// from <paramref name="fromShape"/> if not set.</param>
		/// <param name="fromShape">A known shape.</param>
		/// <param name="toElement">The backing element to find the nearest shape for.</param>
		/// <param name="forLink">The backing link associated with this type of connection.</param>
		/// <returns>The nearest shape if one is available, or <see langword="null"/></returns>
		public static ShapeElement FindNearestShapeForElement(Diagram diagram, ShapeElement fromShape, ModelElement toElement, ModelElement forLink)
		{
			ShapeElement closestToShape;
			ShapeElement closerFromShape;
			bool closerFromShapeBlocking;
			IConfigureableLinkEndpoint configurableFromEndpoint;
			bool detachFromShape = null != (configurableFromEndpoint = fromShape as IConfigureableLinkEndpoint) &&
				AttachLinkResult.Attach != configurableFromEndpoint.CanAttachLink(forLink, false);
			return (!detachFromShape &&
				FindNearestShapeForElement(diagram, fromShape, toElement, forLink, null, false, out closestToShape, out closerFromShape, out closerFromShapeBlocking) &&
				closerFromShape == null) ?
					closestToShape :
					null;
		}
		/// <summary>
		/// Find the nearest shape on this diagram relative to another shape.
		/// </summary>
		/// <param name="diagram">The <see cref="Diagram"/> to search. Populated automaticatlly
		/// from <paramref name="fromShape"/> if not set.</param>
		/// <param name="fromShape">A known shape.</param>
		/// <param name="toElement">The backing element to find the nearest shape for.</param>
		/// <param name="forLink">The element backing of a link shape associated with this type of connection.</param>
		/// <param name="discludedShape">A shape that should be ignored</param>
		/// <param name="detachedFromShape">True if the from shape cannot be attached.</param>
		/// <param name="closestToShape">The closest matching shape</param>
		/// <param name="closerFromShape">A shape of the same type as the fromShape that
		/// is closer to the closestToShape.</param>
		/// <param name="closerFromShapeBlocking">Set if the closer from shape blocks
		/// connecting the closestToShape to the fromShape.</param>
		/// <returns>The nearest shape if one is available, or <see langword="null"/></returns>
		private static bool FindNearestShapeForElement(Diagram diagram, ShapeElement fromShape, ModelElement toElement, ModelElement forLink, ShapeElement discludedShape, bool detachedFromShape, out ShapeElement closestToShape, out ShapeElement closerFromShape, out bool closerFromShapeBlocking)
		{
			ModelElement fromElement;
			closestToShape = null;
			closerFromShape = null;
			closerFromShapeBlocking = false;
			if ((null == diagram && (null == (diagram = fromShape.Diagram))) ||
				null == (fromElement = fromShape.ModelElement))
			{
				return false;
			}

			// Find the nearest to shape
			double closestToShapeDistance = double.MaxValue;
			PointD closestToShapeCenter = default(PointD);
			double distanceX;
			double distanceY;
			double currentDistance;
			PointD center = GetReliableShapeCenter(fromShape);
			double testCenterX = center.X;
			double testCenterY = center.Y;
			foreach (ShapeElement currentToShapeIter in FindAllShapesForElement<ShapeElement>(diagram, toElement, true))
			{
				ShapeElement currentToShape = ResolvePrimaryShape(currentToShapeIter);
				if (discludedShape != null && discludedShape == currentToShape)
				{
					continue;
				}
				bool blockingShape = false;
				IConfigureableLinkEndpoint configurableToEndpoint = currentToShape as IConfigureableLinkEndpoint;
				if (configurableToEndpoint != null)
				{
					switch (configurableToEndpoint.CanAttachLink(forLink, true))
					{
						//case AttachLinkResult.Attach:
						//    break;
						case AttachLinkResult.Defer:
							// Find a farther one if possible
							continue;
						case AttachLinkResult.Block:
							// If this is closest, pretend we didn't find any
							blockingShape = true;
							break;
					}
				}
				center = GetReliableShapeCenter(currentToShape);
				if ((currentDistance = (distanceX = testCenterX - center.X) * distanceX
					+ (distanceY = testCenterY - center.Y) * distanceY) < closestToShapeDistance)
				{
					closestToShapeDistance = currentDistance;
					closestToShapeCenter = center;
					closestToShape = blockingShape ? null : currentToShape;
				}
			}
			if (closestToShape != null)
			{
				// We have a to shape to connect to, but that does not mean that the
				// elements are either currently connected or should be connected.
				// Before connecting, find out up front if we have a closer from shape than the current from shape.
				// If we do, then the current to/from shapes should not be connected for this link.
				double closerFromShapeDistance = detachedFromShape ? double.MaxValue : closestToShapeDistance;
				testCenterX = closestToShapeCenter.X;
				testCenterY = closestToShapeCenter.Y;
				// See if there is another closer fromShape to the closest to shape
				foreach (ShapeElement currentFromShapeIter2 in FindAllShapesForElement<ShapeElement>(diagram, fromElement, true))
				{
					ShapeElement currentFromShape2 = ResolvePrimaryShape(currentFromShapeIter2);
					if (currentFromShape2 == fromShape ||
						(discludedShape != null && discludedShape == currentFromShape2))
					{
						continue;
					}
					bool blockingShape = false;
					bool deferredShape = false;
					IConfigureableLinkEndpoint configurableCloserFromEndpoint = currentFromShape2 as IConfigureableLinkEndpoint;
					if (configurableCloserFromEndpoint != null)
					{
						switch (configurableCloserFromEndpoint.CanAttachLink(forLink, false))
						{
							//case AttachLinkResult.Attach:
							//    break;
							case AttachLinkResult.Defer:
								// Find a farther one if possible
								deferredShape = true;
								break;
							case AttachLinkResult.Block:
								// If this is closest, pretend we didn't find any
								blockingShape = true;
								break;
						}
					}
					if (!deferredShape)
					{
						center = GetReliableShapeCenter(currentFromShape2);
						if ((currentDistance = (distanceX = testCenterX - center.X) * distanceX
							+ (distanceY = testCenterY - center.Y) * distanceY) < closerFromShapeDistance)
						{
							closerFromShapeDistance = currentDistance;
							closerFromShape = currentFromShape2;
							closerFromShapeBlocking = blockingShape;
						}
					}
				}
				return true;
			}
			return false;
		}
		private static object SecondaryLinkReconfigureKey = new object();
		private static void BlockSecondaryLinkReconfigure(Dictionary<object, object> contextInfo, BinaryLinkShape linkShape)
		{
			object blockInfo;
			if (contextInfo.TryGetValue(SecondaryLinkReconfigureKey, out blockInfo))
			{
				Dictionary<BinaryLinkShape, BinaryLinkShape> dictionary;
				if (null != (dictionary = blockInfo as Dictionary<BinaryLinkShape, BinaryLinkShape>))
				{
					dictionary[linkShape] = linkShape;
				}
				else
				{
					dictionary = new Dictionary<BinaryLinkShape, BinaryLinkShape>();
					BinaryLinkShape existingBlockedLink = (BinaryLinkShape)blockInfo;
					dictionary[existingBlockedLink] = existingBlockedLink;
					dictionary[linkShape] = linkShape;
					contextInfo[SecondaryLinkReconfigureKey] = dictionary;
				}
			}
			else
			{
				contextInfo[SecondaryLinkReconfigureKey] = linkShape;
			}
		}
		private static void UnblockSecondaryLinkReconfigure(Dictionary<object, object> contextInfo, BinaryLinkShape linkShape)
		{
			object blockInfo;
			if (contextInfo.TryGetValue(SecondaryLinkReconfigureKey, out blockInfo))
			{
				Dictionary<BinaryLinkShape, BinaryLinkShape> dictionary;
				if (blockInfo == linkShape)
				{
					contextInfo.Remove(SecondaryLinkReconfigureKey);
				}
				else if (null != (dictionary = blockInfo as Dictionary<BinaryLinkShape, BinaryLinkShape>) &&
					dictionary.ContainsKey(linkShape))
				{
					dictionary.Remove(linkShape);
				}
			}
		}
		private static bool IsSecondaryLinkReconfigureBlocked(Dictionary<object, object> contextInfo, BinaryLinkShape linkShape)
		{
			object blockInfo;
			if (contextInfo.TryGetValue(SecondaryLinkReconfigureKey, out blockInfo))
			{
				Dictionary<BinaryLinkShape, BinaryLinkShape> dictionary;
				return (blockInfo == linkShape) ||
					(null != (dictionary = blockInfo as Dictionary<BinaryLinkShape, BinaryLinkShape>) &&
					dictionary.ContainsKey(linkShape));
			}
			return false;
		}
		/// <summary>
		/// Link shapes are not repositioned until well after we need an accurate
		/// location for the link. Use this method to get what the center will be
		/// on completion.
		/// </summary>
		private static PointD GetReliableShapeCenter(ShapeElement shape)
		{
			BinaryLinkShape linkShape;
			NodeShape toShape;
			NodeShape fromShape;
			if (null != (linkShape = shape as BinaryLinkShape) &&
				null != (toShape = ResolvePrimaryShape(linkShape.ToShape) as NodeShape) &&
				null != (fromShape = ResolvePrimaryShape(linkShape.FromShape) as NodeShape))
			{
				PointD toShapeCenter = toShape.Center;
				PointD fromShapeCenter = fromShape.Center;
				PointD? testPoint;
				PointD fromPoint =
					(testPoint = GeometryUtility.DoCustomFoldShape(
					fromShape,
					GeometryUtility.VectorEndPointForBase(toShape, toShapeCenter),
					toShape)).HasValue ? testPoint.Value : fromShapeCenter;
				PointD toPoint =
					(testPoint = GeometryUtility.DoCustomFoldShape(
					toShape,
					GeometryUtility.VectorEndPointForBase(fromShape, fromPoint),
					fromShape)).HasValue ? testPoint.Value : toShapeCenter;
				return new PointD((fromPoint.X + toPoint.X) / 2, (fromPoint.Y + toPoint.Y) / 2);
			}
			return shape.AbsoluteCenter;
		}
		/// <summary>
		/// Gets all existing links for the shape and its proxy connectors.
		/// This method will check for the <see cref="IProvideConnectorShape"/> interface.
		/// </summary>
		/// <param name="shape">The shape to check</param>
		/// <param name="getToLinks">True to collect all to role links</param>
		/// <param name="getFromLinks">True to collect all from role links</param>
		/// <param name="linkBackingElement">Only return links that have this element as the <see cref="PresentationElement.ModelElement">ModelElement</see>.</param>
		/// <param name="snapshot">The caller may change the resulting set, return a snapshot copy of the current data.</param>
		/// <returns>The attached link shapes</returns>
		private static IEnumerable<BinaryLinkShape> GetExistingLinks(ShapeElement shape, bool getToLinks, bool getFromLinks, ModelElement linkBackingElement, bool snapshot)
		{
			Debug.Assert(getToLinks || getFromLinks, "Either getToLinks or fromFromLinks needs to be true");

			foreach (BinaryLinkShape linkShape in
				(getToLinks && getFromLinks) ?
					MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(shape, snapshot) :
					(getToLinks ?
					MultiShapeUtility.GetEffectiveAttachedLinkShapesTo<BinaryLinkShape>(shape, snapshot) :
					MultiShapeUtility.GetEffectiveAttachedLinkShapesFrom<BinaryLinkShape>(shape, snapshot)))
			{
				if (linkBackingElement == null || linkShape.ModelElement == linkBackingElement)
				{
					yield return linkShape;
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
		public static ShapeElement ResolvePrimaryShape(ShapeElement shape)
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
		#region Utility Methods
		/// <summary>
		/// Determines if a the model element backing the shape element
		/// is represented by a shape of the same type elsewhere in the presentation
		/// layer. If a derived shape displays multiple presentations, they should
		/// implement the <see cref="IDisplayMultiplePresentations"/> interface.
		/// Shapes of different types can also included if both the context and
		/// target shapes implement <see cref="IRecognizedSharedPresentationType"/>
		/// </summary>
		public static bool ElementHasMultiplePresentations(ShapeElement shapeElement)
		{
			ModelElement modelElement;
			if (null != shapeElement &&
				null != (modelElement = shapeElement.ModelElement))
			{
				LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(modelElement);
				int pelCount = presentationElements.Count;
				if (pelCount != 0)
				{
					Partition shapePartition = shapeElement.Partition;
					Type thisType = shapeElement.GetType();
					IRecognizedSharedPresentationType sharedType = shapeElement as IRecognizedSharedPresentationType;
					string sharedKey = sharedType == null ? null : sharedType.SharedPresentationTypeKey;
					for (int i = 0; i < pelCount; ++i)
					{
						PresentationElement pel = presentationElements[i];
						IRecognizedSharedPresentationType sharedPelType;
						if (shapeElement != pel &&
							shapePartition == pel.Partition &&
							(sharedKey == null ?
								thisType.IsAssignableFrom(pel.GetType()) :
								(null != (sharedPelType = pel as IRecognizedSharedPresentationType) && sharedPelType.SharedPresentationTypeKey == sharedKey)))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Iterate shapes associated with a given element
		/// </summary>
		/// <param name="modelElement">The parent element to iterate. Can be <see langword="null"/> if <paramref name="shapeElement"/> is specified.</param>
		/// <param name="shapeElement">The shape to reference. Can be <see langword="null"/> if <paramref name="modelElement"/> is specified.</param>
		/// <param name="onePerDiagram">True to filter the shapes to one per diagram</param>
		/// <param name="visitor">A <see cref="Predicate{ShapeElement}"/> callback.
		/// Return <see langword="true"/> to continue iteration.</param>
		public static void VisitAssociatedShapes(ModelElement modelElement, ShapeElement shapeElement, bool onePerDiagram, Predicate<ShapeElement> visitor)
		{
			if (modelElement == null && shapeElement != null)
			{
				modelElement = shapeElement.ModelElement;
			}
			if (modelElement != null)
			{
				LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(modelElement);
				int pelCount = presentationElements.Count;
				if (pelCount != 0)
				{
					List<Diagram> visitedDiagrams = null;
					Diagram visitedDiagram = null;

					Partition shapePartition = (shapeElement != null) ? shapeElement.Partition : modelElement.Partition;
					Type thisType;
					string sharedKey = null;
					if (shapeElement != null)
					{
						thisType = shapeElement.GetType();
						IRecognizedSharedPresentationType sharedType;
						if (null != (sharedType = shapeElement as IRecognizedSharedPresentationType))
						{
							sharedKey = sharedType.SharedPresentationTypeKey;
						}
					}
					else
					{
						thisType = typeof(ShapeElement);
					}
					for (int i = 0; i < pelCount; ++i)
					{
						PresentationElement pel = presentationElements[i];
						IRecognizedSharedPresentationType sharedPelType;
						if (shapePartition == pel.Partition &&
							(sharedKey == null ?
								thisType.IsAssignableFrom(pel.GetType()) :
								(null != (sharedPelType = pel as IRecognizedSharedPresentationType) && sharedPelType.SharedPresentationTypeKey == sharedKey)))
						{
							ShapeElement sel = pel as ShapeElement;
							Diagram selDiagram = sel.Diagram;
							if (!onePerDiagram || (visitedDiagram != selDiagram && (visitedDiagrams == null || !visitedDiagrams.Contains(selDiagram))))
							{
								if (!visitor(sel))
								{
									return;
								}
								if (onePerDiagram)
								{
									if (visitedDiagram == null)
									{
										visitedDiagram = selDiagram;
									}
									else
									{
										if (visitedDiagrams == null)
										{
											visitedDiagrams = new List<Diagram>();
										}
										visitedDiagrams.Add(selDiagram);
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // Utility Methods
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
	/// Use with <see cref="IConfigureableLinkEndpoint.CanAttachLink"/> to
	/// determine whether a link can be attached to an endpoint shape.
	/// </summary>
	public enum AttachLinkResult
	{
		/// <summary>
		/// The link can be attached to this shape
		/// </summary>
		Attach,
		/// <summary>
		/// The link cannot be attached to this shape, but a
		/// shape that is farther away may be attached to this
		/// link.
		/// </summary>
		Defer,
		/// <summary>
		/// The link cannot be attached to this shape, and no
		/// shape that is farther away should be attached to
		/// this link.
		/// </summary>
		Block,
	}
	/// <summary>
	/// Test if a shape supports being used as a relationship endpoint.
	/// Allows dynamic configuration on a per-instance basis.
	/// </summary>
	public interface IConfigureableLinkEndpoint
	{
		/// <summary>
		/// Allow an individual <see cref="ShapeElement"/> to determine if a
		/// configurable link should attach to the shape.
		/// </summary>
		/// <param name="element">The <see cref="ModelElement"/> backing the link shape</param>
		/// <param name="toRole">Set to true if the 'to' role is being attached to this shape, false for the 'from' role.</param>
		/// <returns><see cref="AttachLinkResult"/> values.</returns>
		AttachLinkResult CanAttachLink(ModelElement element, bool toRole);
		/// <summary>
		/// Fixup any elements that are displayed as links and whose endpoints can
		/// refuse to attach the link using <see cref="CanAttachLink"/>.
		/// The implementation should do fixup on the local diagram for any link
		/// element that is not represented by any shape on the current diagram.
		/// </summary>
		/// <param name="diagram">The context diagram.</param>
		void FixupUnattachedLinkElements(Diagram diagram);
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
		ShapeElement ProxyConnectorShapeFor { get;}
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
		/// <param name="ignoreLinkShapesFor">A <see cref="ModelElement"/> (generally an <see cref="ElementLink"/>)
		/// that should be ignored when checking existing links. This can be <see langword="null"/>, but will be
		/// set while reconfiguring links.
		/// </param>
		/// <returns>NodeShape</returns>
		NodeShape GetUniqueConnectorShape(ShapeElement oppositeShape, ModelElement ignoreLinkShapesFor);
	}
	/// <summary>
	/// Indicate that the shape changes its display when more
	/// than one presentation is visible. Derived shapes should
	/// use the <see cref="MultiShapeUtility.ElementHasMultiplePresentations"/>
	/// method to determine when multiple presentations should be displayed.
	/// The default behavior requires that the diagram types be exactly the
	/// same. To match shapes across different diagram types, both diagram
	/// types should also implement <see cref="IRecognizedSharedPresentationType"/>,
	/// which derives from this interface.
	/// </summary>
	public interface IDisplayMultiplePresentations
	{
		// No methods
	}
	/// <summary>
	/// An extension to <see cref="IDisplayMultiplePresentations"/> to allow shapes of
	/// different types to be included in the 'Select on Diagram' list. All shared types
	/// must returned the same type key to enable cross-navigation.
	/// </summary>
	public interface IRecognizedSharedPresentationType : IDisplayMultiplePresentations
	{
		/// <summary>
		/// Return a string indicating a named type
		/// </summary>
		string SharedPresentationTypeKey { get;}
	}
	#endregion //Interfaces
}
