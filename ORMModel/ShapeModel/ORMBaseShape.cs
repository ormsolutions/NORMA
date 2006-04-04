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

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
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
		/// Place the child shape the first time it is placed
		/// on the diagram
		/// </summary>
		/// <param name="parent">The parent shape</param>
		public virtual void PlaceAsChildOf(NodeShape parent)
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
		/// Determines if a the model element backing the shape element
		/// is represented by a shape of the same type elsewhere in the presentation
		/// layer.
		/// </summary>
		public static bool ElementHasMultiplePresentations(ShapeElement shapeElement)
		{
			ModelElement modelElement = shapeElement.ModelElement;
			if (modelElement != null)
			{
				PresentationElementMoveableCollection presentationElements = modelElement.PresentationRolePlayers;
				int pelCount = presentationElements.Count;
				if (pelCount != 0)
				{
					Type thisType = shapeElement.GetType();
					for (int i = 0; i < pelCount; ++i)
					{
						PresentationElement pel = presentationElements[i];
						if (!object.ReferenceEquals(shapeElement, pel) && thisType.IsAssignableFrom(pel.GetType()))
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
		/// Add error brushes to the styleSet
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);

			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			//UNDONE: This color isn't permanent. probably want a better color for the errors.
			brushSettings.ForeColor = Color.FromArgb(60, colorService.GetForeColor(ORMDesignerColor.ConstraintError));
			brushSettings.HatchStyle = HatchStyle.LightDownwardDiagonal;
			brushSettings.BrushType = typeof(HatchBrush);
			classStyleSet.AddBrush(ORMDiagram.ErrorBackgroundResource, DiagramBrushes.DiagramBackground, brushSettings);
			brushSettings.ForeColor = ORMDiagram.ModifyLuminosity(brushSettings.ForeColor);
			brushSettings.BackColor = ORMDiagram.ModifyLuminosity(((SolidBrush)classStyleSet.GetBrush(DiagramBrushes.DiagramBackground)).Color);
			classStyleSet.AddBrush(ORMDiagram.HighlightedErrorBackgroundResource, DiagramBrushes.DiagramBackground, brushSettings);
		}
		/// <summary>
		/// Use a different background brush if we have errors
		/// </summary>
		public override StyleSetResourceId BackgroundBrushId
		{
			get
			{
				if (ModelError.HasErrors(ModelElement))
				{
					ORMDiagram diagram;
					DiagramView view;
					DiagramClientView clientView;
					HighlightedShapesCollection highlightedShapes;
					if (null != (diagram = Diagram as ORMDiagram) &&
						null != (view = diagram.ActiveDiagramView) &&
						null != (clientView = view.DiagramClientView) &&
						null != (highlightedShapes = clientView.HighlightedShapes) &&
						highlightedShapes.Count != 0 &&
						highlightedShapes.Contains(new DiagramItem(this)))
					{
						return ORMDiagram.HighlightedErrorBackgroundResource;
					}
					else
					{
						return ORMDiagram.ErrorBackgroundResource;
					}
				}
				else
				{
					return DiagramBrushes.DiagramBackground;
				}
			}
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
		/// <summary>
		/// If a new child shape was not placed, then defer to
		/// PlaceAsChildOf on the child. Note that AutoResize
		/// has not been called on the child prior to this call.
		/// </summary>
		/// <param name="child">A new child shape element</param>
		/// <param name="childWasPlaced">false if the child element was
		/// not previously placed.</param>
		protected override void OnChildConfigured(ShapeElement child, bool childWasPlaced)
		{
			if (!childWasPlaced)
			{
				ORMBaseShape baseShape;
				if (null != (baseShape = child as ORMBaseShape))
				{
					baseShape.PlaceAsChildOf(this);
				}
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
		#region Hack to block child shape moving twice
		/// <summary>
		/// Hack override to handle MSBUG that moves child shapes twice on the diagram. Combined
		/// with ORMDiagram.MoveByRepositioning and ORMDiagram.MovingDiagramItemsContextKey to limit
		/// to limit moving of child shapes during a transaction.
		/// </summary>
		public override bool CanMove
		{
			get
			{
				Store store = Store;
				if (store.TransactionActive)
				{
					ShapeElement parentShape = ParentShape;
					if (!(parentShape is ORMDiagram))
					{
						DiagramItemCollection movingItems = (DiagramItemCollection)store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ORMDiagram.MovingDiagramItemsContextKey];
						if (movingItems != null &&
							movingItems.Contains(new DiagramItem(parentShape)))
						{
							return false;
						}
					}
				}
				return base.CanMove;
			}
		}
		#endregion // Hack to block child shape moving twice
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
		#region Auto-invalidate tracking
		/// <summary>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		public void InvalidateRequired()
		{
			InvalidateRequired(false);
		}
		/// <summary>
		/// Call to automatically invalidate the shape during events.
		/// Invalidates during the original event sequence as well as undo and redo.
		/// </summary>
		/// <param name="refreshBitmap">Value to forward to the Invalidate method's refreshBitmap property during event playback</param>
		public void InvalidateRequired(bool refreshBitmap)
		{
			TransactionManager tmgr = TransactionManager;
			if (tmgr.InTransaction)
			{
				UpdateCounter = unchecked(tmgr.CurrentTransaction.SequenceNumber - (refreshBitmap ? 0L : 1L));
			}
		}
		/// <summary>
		/// Called during event playback before an Invalidate call triggered
		/// via the InvalidateRequired mechanism is called. The default implementation
		/// is empty.
		/// </summary>
		protected virtual void BeforeInvalidate()
		{
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeId = attribute.Id;
			if (attributeId == UpdateCounterMetaAttributeGuid)
			{
				TransactionManager tmgr = TransactionManager;
				if (tmgr.InTransaction)
				{
					// Using subtract 2 and set to 1 under to indicate
					// the difference between an Invalidate(true) and
					// and Invalidate(false)
					return unchecked(tmgr.CurrentTransaction.SequenceNumber - 2);
				}
				else
				{
					return 0L;
				}
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not stored. 
		/// </summary>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == UpdateCounterMetaAttributeGuid)
			{
				// Nothing to do, we're just trying to create a transaction log
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Attach base shape event handlers
		/// </summary>
		public static void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ORMBaseShape.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(AttributeChangedEvent));
		}
		/// <summary>
		/// Detach base shape event handlers
		/// </summary>
		public static void DetachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ORMBaseShape.MetaClassGuid);
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(AttributeChangedEvent));
		}
		private static void AttributeChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			Guid attributeId = e.MetaAttribute.Id;
			if (attributeId == UpdateCounterMetaAttributeGuid)
			{
				ORMBaseShape shape = e.ModelElement as ORMBaseShape;
				if (!shape.IsRemoved)
				{
					shape.BeforeInvalidate();
					shape.Invalidate(Math.Abs(unchecked((long)e.OldValue - (long)e.NewValue)) != 1L);
				}
			}
		}
		#endregion // Auto-invalidate tracking
		#region Update shapes on ModelError added/removed
		[RuleOn(typeof(ModelHasError))]
		private class ModelErrorAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ProcessModelErrorChange(e.ModelElement as ModelHasError);
			}
		}
		[RuleOn(typeof(ModelHasError))]
		private class ModelErrorRemoving : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ModelHasError link = e.ModelElement as ModelHasError;
				if (!link.Model.IsRemoving)
				{
					ProcessModelErrorChange(link);
				}
			}
		}
		private static void ProcessModelErrorChange(ModelHasError errorLink)
		{
			ModelError error = errorLink.ErrorCollection;
			// Give the error itself a change to have an indirect owner.
			// A ModelError can own itself.
			InvalidateIndirectErrorOwnerDisplay(error, null);
			MetaClassInfo classInfo = error.MetaClass;
			IList playedMetaRoles = classInfo.AllMetaRolesPlayed;
			int playedMetaRoleCount = playedMetaRoles.Count;
			for (int i = 0; i < playedMetaRoleCount; ++i)
			{
				MetaRoleInfo roleInfo = (MetaRoleInfo)playedMetaRoles[i];
				if (roleInfo.Id != ModelHasError.ErrorCollectionMetaRoleGuid)
				{
					IList rolePlayers = error.GetCounterpartRolePlayers(roleInfo, roleInfo.OppositeMetaRole);
					int rolePlayerCount = rolePlayers.Count;
					for (int j = 0; j < rolePlayerCount; ++j)
					{
						ModelElement rolePlayer = (ModelElement)rolePlayers[j];
						InvalidateErrorOwnerDisplay(rolePlayer);
						InvalidateIndirectErrorOwnerDisplay(rolePlayer, null);
					}
				}
			}
		}
		private static void InvalidateErrorOwnerDisplay(ModelElement element)
		{
			if (!element.IsRemoving)
			{
				PresentationElementMoveableCollection pels = element.PresentationRolePlayers;
				int pelCount = pels.Count;
				for (int i = 0; i < pelCount; ++i)
				{
					ORMBaseShape shape = pels[i] as ORMBaseShape;
					if (shape != null && !shape.IsRemoving)
					{
						shape.InvalidateRequired();
					}
				}
			}
		}
		private static void InvalidateIndirectErrorOwnerDisplay(ModelElement element, MetaDataDirectory metaDataDirectory)
		{
			IHasIndirectModelErrorOwner indirectOwner = element as IHasIndirectModelErrorOwner;
			if (indirectOwner != null)
			{
				Guid[] metaRoles = indirectOwner.GetIndirectModelErrorOwnerLinkRoles();
				int roleCount;
				if (metaRoles != null &&
					0 != (roleCount = metaRoles.Length))
				{
					if (metaDataDirectory == null)
					{
						metaDataDirectory = element.Store.MetaDataDirectory;
					}
					for (int i = 0; i < roleCount; ++i)
					{
						MetaRoleInfo metaRole = metaDataDirectory.FindMetaRole(metaRoles[i]);
						if (metaRole != null)
						{
							IList counterparts = element.GetCounterpartRolePlayers(metaRole, metaRole.OppositeMetaRole);
							int counterpartCount = counterparts.Count;
							for (int j = 0; j < counterpartCount; ++j)
							{
								ModelElement counterpart = (ModelElement)counterparts[j];
								if (counterpart is IModelErrorOwner)
								{
									InvalidateErrorOwnerDisplay(counterpart);
								}
								InvalidateIndirectErrorOwnerDisplay(counterpart, metaDataDirectory);
							}
						}
					}
				}
			}
		}
		#endregion // Update shapes on ModelError added/removed
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
