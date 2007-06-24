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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ExternalConstraintShape : IStickyObject, IModelErrorActivation
	{
		#region Customize appearance
		/// <summary>
		/// A brush used to draw portions of mandatory constraints
		/// </summary>
		protected static readonly StyleSetResourceId ExternalConstraintBrush = new StyleSetResourceId("Neumont", "ExternalConstraintBrush");
		/// <summary>
		/// A style set used for drawing deontic constraints
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Constraints are drawn as circles
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return CustomFoldCircleShapeGeometry.ShapeGeometry;
			}
		}
		/// <summary>
		/// Initialize a pen and a brush for drawing the constraint
		/// outlines and contents.
		/// </summary>
		/// <param name="classStyleSet">StyleSet</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings penSettings = new PenSettings();
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			Color constraintColor = colorService.GetForeColor(ORMDesignerColor.Constraint);
			penSettings.Color = constraintColor;
			penSettings.Width = 1.35F / 72.0F; // 1.35 Point.
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = constraintColor;
			classStyleSet.AddBrush(ExternalConstraintBrush, DiagramBrushes.ShapeBackground, brushSettings);
			penSettings.Color = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ShapeOutline, penSettings);
		}
		/// <summary>
		/// Switch between alethic and deontic style sets
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				IConstraint constraint = AssociatedConstraint;
				// Note that we don't do anything with fonts with this style set, so the
				// static one is sufficient. Instance style sets also go through a font initiation
				// step inside the framework
				return (constraint != null && constraint.Modality == ConstraintModality.Deontic) ?
					DeonticClassStyleSet :
					base.StyleSet;
			}
		}
		/// <summary>
		/// Create an alternate style set for deontic constraints
		/// </summary>
		protected virtual StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					// Set up an alternate style set for drawing deontic constraints
					retVal = new StyleSet(ClassStyleSet);
					IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
					Color constraintColor = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
					PenSettings penSettings = new PenSettings();
					penSettings.Color = constraintColor;
					retVal.OverridePen(DiagramPens.ShapeOutline, penSettings);
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = constraintColor;
					retVal.OverrideBrush(ExternalConstraintBrush, brushSettings);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}

		#region Setup Paint Tools
		// Warning: This will break horribly if this code is ever run on multiple threads simultaneously.
		private Color myPaintPenStartColor;
		private Color myPaintBrushStartColor;
		private int myPaintRefCount;
		private bool myPaintRestoreColor;
		private Pen myPaintPen;
		private Brush myPaintBrush;
		/// <summary>
		/// The <see cref="Pen"/> to use for painting.
		/// </summary>
		protected Pen PaintPen
		{
			get
			{
				return this.myPaintPen;
			}
		}
		/// <summary>
		/// The <see cref="Brush"/> to use for painting.
		/// </summary>
		protected Brush PaintBrush
		{
			get
			{
				return this.myPaintBrush;
			}
		}
		/// <summary>
		/// Setup the tools (<see cref="Pen"/>s and <see cref="Brush"/>s) used for painting, as appropriate.
		/// </summary>
		/// <remarks>
		/// Make sure that you call <see cref="DisposePaintTools"/> when you are done with the paint tools.
		/// </remarks>
		protected void InitializePaintTools(DiagramPaintEventArgs e)
		{
			if (this.myPaintRefCount++ == 0)
			{
				base.OnPaintShape(e);
				StyleSet styles = this.StyleSet;
				this.myPaintPen = styles.GetPen(OutlinePenId);
				this.myPaintBrush = styles.GetBrush(ExternalConstraintBrush);
				SolidBrush coloredBrush = this.myPaintBrush as SolidBrush;

				// Keep the pen color in sync with the color being used for highlighting
				this.myPaintPenStartColor = this.UpdateGeometryLuminosity(e.View, this.myPaintPen);
				this.myPaintBrushStartColor = default(Color);
				if (coloredBrush != null)
				{
					this.myPaintBrushStartColor = coloredBrush.Color;
					coloredBrush.Color = this.myPaintPen.Color;
				}
				this.myPaintRestoreColor = this.myPaintPenStartColor != this.myPaintPen.Color;
				if (!this.myPaintRestoreColor && coloredBrush != null)
				{
					this.myPaintRestoreColor = this.myPaintBrushStartColor != coloredBrush.Color;
				}
			}
		}
		/// <summary>
		/// Restore the original settings for the paint tools.
		/// </summary>
		/// <seealso cref="InitializePaintTools"/>
		protected void DisposePaintTools()
		{
			if (--this.myPaintRefCount == 0)
			{
				// Restore pen and/or brush color
				if (this.myPaintRestoreColor)
				{
					this.myPaintPen.Color = this.myPaintPenStartColor;
					SolidBrush coloredBrush = this.myPaintBrush as SolidBrush;
					if (coloredBrush != null)
					{
						coloredBrush.Color = this.myPaintBrushStartColor;
					}
				}

				this.myPaintBrush = null;
				this.myPaintPen = null;
				this.myPaintPenStartColor = default(Color);
				this.myPaintBrushStartColor = default(Color);
				this.myPaintRestoreColor = false;
			}
		}
		#endregion

		/// <summary>
		/// Draw the various constraint types
		/// </summary>
		/// <param name="e">DiagramPaintEventArgs</param>
		public override void OnPaintShape(DiagramPaintEventArgs e)
		{
			// In this method, and this method only, don't call base.OnPaintShape,
			// since this.InitializePaintTools does it for us
			this.InitializePaintTools(e);

			IConstraint constraint = AssociatedConstraint;
			RectangleD bounds = AbsoluteBounds;
			RectangleF boundsF = RectangleD.ToRectangleF(bounds);
			Graphics g = e.Graphics;
			const double cos45 = 0.70710678118654752440084436210485;

			Pen pen = this.PaintPen;
			Brush brush = this.myPaintBrush;

			switch (constraint.ConstraintType)
			{
				#region Frequency
				case ConstraintType.Frequency:
					{
						break;
					}
				#endregion
				#region Ring
				case ConstraintType.Ring:
					// Note: goto default here restores the frowny face. However,
					// with the error feedback, we already have UI indicating there
					// is a problem.
					break;
				#endregion
				#region Equality
				case ConstraintType.Equality:
					{
						double xOffset = bounds.Width * .3;
						float xLeft = (float)(bounds.Left + xOffset);
						float xRight = (float)(bounds.Right - xOffset);
						double yCenter = bounds.Top + bounds.Height / 2;
						double yOffset = (double)pen.Width * 1.0;
						float y = (float)(yCenter - yOffset);
						g.DrawLine(pen, xLeft, y, xRight, y);
						y = (float)(yCenter + yOffset);
						g.DrawLine(pen, xLeft, y, xRight, y);
						break;
					}
				#endregion
				#region Mandatory
				case ConstraintType.DisjunctiveMandatory:
					{
						// Draw the dot
						RectangleF shrinkBounds = boundsF;
						shrinkBounds.Inflate(-boundsF.Width * .22f, -boundsF.Height * .22f);
						g.FillEllipse(brush, shrinkBounds);
						if (null != ExclusiveOrConstraintCoupler.GetExclusiveOrExclusionConstraint((MandatoryConstraint)constraint))
						{
							goto case ConstraintType.Exclusion;
						}
						break;
					}
				#endregion
				#region Exclusion
				case ConstraintType.Exclusion:
					{
						// Draw the X
						double offset = (bounds.Width + pen.Width) * (1 - cos45) / 2;
						float leftX = (float)(bounds.Left + offset);
						float rightX = (float)(bounds.Right - offset);
						float topY = (float)(bounds.Top + offset);
						float bottomY = (float)(bounds.Bottom - offset);
						g.DrawLine(pen, leftX, topY, rightX, bottomY);
						g.DrawLine(pen, leftX, bottomY, rightX, topY);
						break;
					}
				#endregion
				#region Uniqueness
				case ConstraintType.ExternalUniqueness:
					{
						// Draw a single line for a uniqueness constraint and a double
						// line for preferred uniqueness
						UniquenessConstraint euc = constraint as UniquenessConstraint;
						double widthAdjust = (double)pen.Width / 2;
						float xLeft = (float)(bounds.Left + widthAdjust);
						float xRight = (float)(bounds.Right - widthAdjust);
						if (euc.IsPreferred)
						{
							double yCenter = bounds.Top + bounds.Height / 2;
							double yOffset = (double)pen.Width * .7;
							float y = (float)(yCenter - yOffset);
							g.DrawLine(pen, xLeft, y, xRight, y);
							y = (float)(yCenter + yOffset);
							g.DrawLine(pen, xLeft, y, xRight, y);
						}
						else
						{
							float y = (float)(bounds.Top + bounds.Height / 2);
							g.DrawLine(pen, xLeft, y, xRight, y);
						}
						break;
					}
				#endregion
				#region Subset
				case ConstraintType.Subset:
					{
						RectangleD arcBounds = bounds;
						double shrinkBy = -bounds.Height * .35;
						double yOffset = pen.Width * .7;
						double xOffset = shrinkBy * .35;
						arcBounds.Inflate(shrinkBy, shrinkBy);
						arcBounds.Offset(xOffset, -yOffset);
						g.DrawArc(pen, RectangleD.ToRectangleF(arcBounds), 90, 180);
						float xLeft = (float)arcBounds.Center.X;
						float xRight = (float)(bounds.Right + shrinkBy - xOffset);
						float y = (float)arcBounds.Top;
						g.DrawLine(pen, xLeft, y, xRight, y);
						y = (float)arcBounds.Bottom;
						g.DrawLine(pen, xLeft, y, xRight, y);
						y = (float)(arcBounds.Bottom + yOffset + yOffset);
						g.DrawLine(pen, (float)arcBounds.Left, y, xRight, y);
						break;
					}
				#endregion
				#region Fallback (default)
				default:
					{
						// Draws a frowny face
						float eyeY = boundsF.Y + (boundsF.Height / 3);
						PointF leftEyeStart = new PointF(boundsF.X + (boundsF.Width * 0.3f), eyeY);
						PointF leftEyeEnd = new PointF(boundsF.X + (boundsF.Width * 0.4f), eyeY);
						PointF rightEyeStart = new PointF(boundsF.X + (boundsF.Width * 0.6f), eyeY);
						PointF rightEyeEnd = new PointF(boundsF.X + (boundsF.Width * 0.7f), eyeY);
						g.DrawLine(pen, leftEyeStart, leftEyeEnd);
						g.DrawLine(pen, rightEyeStart, rightEyeEnd);

						float mouthLeft = boundsF.X + (boundsF.Width * 0.25f);
						float mouthTop = boundsF.Y + (boundsF.Height * 0.55f);
						RectangleF mouthRectangle = new RectangleF(mouthLeft, mouthTop, boundsF.Width * 0.5f, boundsF.Height * 0.25f);
						g.DrawArc(pen, mouthRectangle, 180, 180);

						break;
					}
				#endregion
			}
			if (constraint.Modality == ConstraintModality.Deontic && constraint.ConstraintType != ConstraintType.Ring)
			{
				float startPenWidth = pen.Width;
				pen.Width = startPenWidth * .70f;
				float boxSide = (float)((1 - cos45) * bounds.Width);
				g.FillEllipse(this.StyleSet.GetBrush(DiagramBrushes.DiagramBackground), (float)bounds.Left, (float)bounds.Top, boxSide, boxSide);
				g.DrawArc(pen, (float)bounds.Left, (float)bounds.Top, boxSide, boxSide, 0, 360);
				pen.Width = startPenWidth;
			}
			this.DisposePaintTools();
		}
		/// <summary>
		/// Use the sticky object pen to draw the outline
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				ORMDiagram ormDiagram;
				StyleSetResourceId id;
				if (null != (ormDiagram = this.Diagram as ORMDiagram)
					&& ormDiagram.StickyObject == this)
				{
					id = ORMDiagram.StickyBackgroundResource;
				}
				else
				{
					id = DiagramPens.ShapeOutline;
				}
				return id;
			}
		}
		#endregion // Customize appearance
		#region ExternalConstraintShape specific
		/// <summary>
		/// Get the typed model element associated with this shape
		/// </summary>
		public IConstraint AssociatedConstraint
		{
			get
			{
				return ModelElement as IConstraint;
			}
		}
		#endregion // ExternalConstraintShape specific
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			bool retVal = true;
			ConstraintDuplicateNameError duplicateName;
			if (error is TooFewRoleSequencesError)
			{
				ActivateNewRoleSequenceAction(null);
			}
			else if (null != (duplicateName = error as ConstraintDuplicateNameError))
			{
				ActivateNameProperty((ModelElement)duplicateName.ConstraintCollection[0]);
			}
			else
			{
				retVal = false;
			}
			return retVal;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region IStickyObject implementation
		/// <summary>
		/// Implements IStickyObject.StickyInitialize
		/// </summary>
		protected void StickyInitialize()
		{
			RedrawAssociatedPels(true);
		}
		void IStickyObject.StickyInitialize()
		{
			StickyInitialize();
		}
		/// <summary>
		/// Implements IStickyObject.StickySelectable
		/// </summary>
		protected bool StickySelectable(ModelElement mel)
		{
			bool rVal = false;
			Role r;
			IConstraint constraint = AssociatedConstraint;
			if (mel == constraint)
			{
				rVal = true;
			}
			else if (null != (r = mel as Role))
			{
				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SetConstraint:
						rVal = (constraint as SetConstraint).RoleCollection.Contains(r);
						break;
					case ConstraintStorageStyle.SetComparisonConstraint:
						foreach (SetComparisonConstraintRoleSequence roleSequence in (constraint as SetComparisonConstraint).RoleSequenceCollection)
						{
							if (roleSequence.RoleCollection.Contains(r))
							{
								rVal = true;
								break;
							}
						}
						break;
				}
			}
			return rVal;
		}
		bool IStickyObject.StickySelectable(ModelElement mel)
		{
			return StickySelectable(mel);
		}
		/// <summary>
		/// Implements IStickyObject.StickyRedraw
		/// </summary>
		protected void StickyRedraw()
		{
			RedrawAssociatedPels(true);
		}
		void IStickyObject.StickyRedraw()
		{
			StickyRedraw();
		}
		private void RedrawAssociatedPels(bool includeFacts)
		{
			IConstraint constraint = AssociatedConstraint;
			if (null != constraint)
			{
				Diagram diagram = this.Diagram;
				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SetConstraint:
						SetConstraint scec = constraint as SetConstraint;
						foreach (FactSetConstraint factConstraint in DomainRoleInfo.GetElementLinks<FactSetConstraint>(scec, FactSetConstraint.SetConstraintDomainRoleId))
						{
							// Redraw the line
							RedrawPelsOnDiagram(factConstraint, diagram);
							if (includeFacts)
							{
								// Redraw the fact type
								RedrawPelsOnDiagram(factConstraint.FactType, diagram);
							}
						}
						break;
					case ConstraintStorageStyle.SetComparisonConstraint:
						SetComparisonConstraint mcec = constraint as SetComparisonConstraint;
						foreach (FactSetComparisonConstraint factConstraint in DomainRoleInfo.GetElementLinks<FactSetComparisonConstraint>(mcec, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId))
						{
							// Redraw the line
							RedrawPelsOnDiagram(factConstraint, diagram);
							if (includeFacts)
							{
								// Redraw the fact type
								RedrawPelsOnDiagram(factConstraint.FactType, diagram);
							}
						}
						break;
				}
			}
			Invalidate(true);
		}
		private static void RedrawPelsOnDiagram(ModelElement element, Diagram diagram)
		{
			LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(element);
			int pelsCount = pels.Count;
			for (int i = 0; i < pelsCount; ++i)
			{
				ShapeElement shape = pels[i] as ShapeElement;
				if (shape != null && shape.Diagram == diagram)
				{
					shape.Invalidate(true);
				}
			}
		}
		#endregion // IStickyObject implementation
		#region Mouse Handling
		/// <summary>
		/// A mouse click event has occurred on this ExternalConstraintShape
		/// </summary>
		/// <param name="e">DiagramPointEventArgs</param>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
			if (!ORMBaseShape.AttemptErrorActivation(e))
			{
				ActivateNewRoleSequenceAction(e.DiagramClientView);
			}
			base.OnDoubleClick(e);
		}
		private void ActivateNewRoleSequenceAction(DiagramClientView clientView)
		{
			ORMDiagram diagram;
			IConstraint constraint;
			if (null != (diagram = this.Diagram as ORMDiagram)
				&& diagram.StickyObject == this
				&& null != (constraint = this.AssociatedConstraint))
			{
				ExternalConstraintConnectAction connectAction = diagram.ExternalConstraintConnectAction;

				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SetConstraint:
						connectAction.ConstraintRoleSequenceToEdit = constraint as ConstraintRoleSequence;
						break;
					case ConstraintStorageStyle.SetComparisonConstraint:
						int maximum = ConstraintUtility.RoleSequenceCountMaximum(constraint);
						if (maximum > 0 && ((SetComparisonConstraint)constraint).RoleSequenceCollection.Count >= maximum)
						{
							return;
						}
						if (constraint.ConstraintType == ConstraintType.Exclusion)
						{
							// If this is a subtype connect action already, then give it the first sequence
							ExclusionConstraint exclusion = (ExclusionConstraint)constraint;
							foreach (FactType existingFactType in exclusion.FactTypeCollection)
							{
								if (existingFactType is SubtypeFact)
								{
									connectAction.ConstraintRoleSequenceToEdit = exclusion.RoleSequenceCollection[0];
									break;
								}
							}
						}
						break;
				}

				if (!connectAction.IsActive)
				{
					if (clientView == null)
					{
						clientView = diagram.ActiveDiagramView.DiagramClientView;
					}
					connectAction.ChainMouseAction(this, clientView);
				}
			}
		}
		#endregion // Mouse Handling
		#region Store Event Handlers
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="ExternalConstraintShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRole(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId), new EventHandler<RolePlayerOrderChangedEventArgs>(RolePlayerOrderChangedEvent), action);

			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(SetComparisonConstraint.ModalityDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(SetComparisonConstraintChangedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(SetConstraint.ModalityDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(SetConstraintChangedEvent), action);

			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(PreferredIdentifierAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(PreferredIdentifierRemovedEvent), action);
		}
		private static void RolePlayerOrderChangedEvent(object sender, RolePlayerOrderChangedEventArgs e)
		{
			SetComparisonConstraint constraint;
			ExternalConstraintShape ecs;
			ORMDiagram ormDiagram;
			if (null != (constraint = e.SourceElement as SetComparisonConstraint))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(constraint))
				{
					if (null != (ecs = pel as ExternalConstraintShape))
					{
						// If the constraint being changed is also the current stick object,
						// then refresh the linked facts as well
						ecs.RedrawAssociatedPels(null != (ormDiagram = ecs.Diagram as ORMDiagram)
							&& ecs == ormDiagram.StickyObject);
					}
				}
			}
		}
		private static void SetComparisonConstraintChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			SetComparisonConstraint constraint;
			ExternalConstraintShape ecs;
			ORMDiagram ormDiagram;
			if (null != (constraint = e.ModelElement as SetComparisonConstraint))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(constraint))
				{
					if (null != (ecs = pel as ExternalConstraintShape))
					{
						// If the constraint being changed is also the current stick object,
						// then refresh the linked facts as well
						ecs.RedrawAssociatedPels(null != (ormDiagram = ecs.Diagram as ORMDiagram)
							&& ecs == ormDiagram.StickyObject);
					}
				}
			}
		}
		private static void SetConstraintChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			SetConstraint constraint;
			ExternalConstraintShape ecs;
			ORMDiagram ormDiagram;
			if (null != (constraint = e.ModelElement as SetConstraint))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(constraint))
				{
					if (null != (ecs = pel as ExternalConstraintShape))
					{
						// If the constraint being changed is also the current stick object,
						// then refresh the linked facts as well
						ecs.RedrawAssociatedPels(null != (ormDiagram = ecs.Diagram as ORMDiagram)
							&& ecs == ormDiagram.StickyObject);
					}
				}
			}
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers being added.
		/// </summary>
		public static void PreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			UniquenessConstraint constraint = link.PreferredIdentifier;
			if (!constraint.IsInternal)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(constraint))
				{
					ExternalConstraintShape constraintShape = pel as ExternalConstraintShape;
					if (constraintShape != null)
					{
						constraintShape.Invalidate(true);
					}
				}
			}
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers being removed.
		/// </summary>
		public static void PreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			UniquenessConstraint constraint = link.PreferredIdentifier;
			if (!constraint.IsInternal &&
				!constraint.IsDeleted)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(constraint))
				{
					ExternalConstraintShape constraintShape = pel as ExternalConstraintShape;
					if (constraintShape != null)
					{
						constraintShape.Invalidate(true);
					}
				}
			}
		}
		#endregion // Store Event Handlers
		#region ExclusiveOrCoupler rules
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// Remove shapes associated with the exclusion constraint
		/// when exclusion and mandatory constraints are coupled.
		/// </summary>
		private static void ExclusiveOrCouplerAddedRule(ElementAddedEventArgs e)
		{
			ExclusiveOrConstraintCoupler link = e.ModelElement as ExclusiveOrConstraintCoupler;
			MandatoryConstraint mandatory = link.MandatoryConstraint;
			ExclusionConstraint exclusion = link.ExclusionConstraint;
			LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(exclusion);
			int pelCount = pels.Count;
			for (int i = pelCount - 1; i >= 0; --i)
			{
				ExternalConstraintShape shape = pels[i] as ExternalConstraintShape;
				if (shape != null)
				{
					shape.Delete();
				}
			}
			pels = PresentationViewsSubject.GetPresentation(mandatory);
			pelCount = pels.Count;
			for (int i = 0; i < pelCount; ++i)
			{
				ExternalConstraintShape shape = pels[i] as ExternalConstraintShape;
				if (shape != null)
				{
					shape.InvalidateRequired(true);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// Split a single shape into two shapes when a exclusion constraint
		/// is decoupled from a mandatory constraint
		/// </summary>
		private static void ExclusiveOrCouplerDeletedRule(ElementDeletedEventArgs e)
		{
			ExclusiveOrConstraintCoupler link = e.ModelElement as ExclusiveOrConstraintCoupler;
			MandatoryConstraint mandatory = link.MandatoryConstraint;
			ExclusionConstraint exclusion = link.ExclusionConstraint;
			if (!mandatory.IsDeleted && !exclusion.IsDeleted)
			{
				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(mandatory);
				int pelCount = pels.Count;
				for (int i = 0; i < pelCount; ++i)
				{
					ExternalConstraintShape shape = pels[i] as ExternalConstraintShape;
					if (shape != null)
					{
						ORMDiagram diagram = (ORMDiagram)shape.Diagram;
						RectangleD bounds = shape.AbsoluteBounds;
						double width = bounds.Width;
						bounds.Offset(-width / 2, 0);
						bounds = diagram.BoundsRules.GetCompliantBounds(shape, bounds);
						shape.AbsoluteBounds = bounds;
						bounds.Offset(width, 0);
						diagram.PlaceORMElementOnDiagram(null, exclusion, bounds.Location, ORMPlacementOption.None);
					}
				}
			}
		}
		#endregion // ExclusiveOrCoupler rules
		#region PreferredIdentifier Shape Redraw rules
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void PreferredIdentifierAddRule(ElementAddedEventArgs e)
		{
			ProcessPreferredIdentifier(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessPreferredIdentifier(EntityTypeHasPreferredIdentifier link, UniquenessConstraint preferredIdentifier)
		{
			if (preferredIdentifier == null)
			{
				preferredIdentifier = link.PreferredIdentifier;
			}
			if (!preferredIdentifier.IsDeleted)
			{
				ModelElement element = preferredIdentifier;
				if (preferredIdentifier.IsInternal)
				{
					LinkedElementCollection<FactType> factTypes = preferredIdentifier.FactTypeCollection;
					if (factTypes.Count == 1)
					{
						element = factTypes[0];
					}
					else
					{
						return;
					}
				}

				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(element);
				int pelCount = pels.Count;
				for (int i = pelCount - 1; i >= 0; --i)
				{
					// ORMBaseShape handles ExternalConstraintShape and FactTypeShape
					ORMBaseShape shape = pels[i] as ORMBaseShape;
					if (shape != null)
					{
						shape.InvalidateRequired(true);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
		{
			ProcessPreferredIdentifier(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			UniquenessConstraint oldPreferredIdentifier = null;
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId)
			{
				oldPreferredIdentifier = (UniquenessConstraint)e.OldRolePlayer;
			}
			ProcessPreferredIdentifier(e.ElementLink as EntityTypeHasPreferredIdentifier, oldPreferredIdentifier);
		}
		#endregion // PreferredIdentifier Shape Redraw rules
	}
}
