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
		/// Set the default size for this object.
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return new SizeD(.16, .16);
			}
		}
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
			PenSettings penSettings = new PenSettings();
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			penSettings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			penSettings.Width = 1.35F / 72.0F; // 1.35 Point.
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = penSettings.Color;
			classStyleSet.AddBrush(ExternalConstraintBrush, DiagramBrushes.ShapeBackground, brushSettings);

			penSettings.Color = colorService.GetBackColor(ORMDesignerColor.ActiveConstraint);
			classStyleSet.AddPen(ORMDiagram.StickyBackgroundResource, DiagramPens.ShapeOutline, penSettings);
		}
		/// <summary>
		/// Draw the various constraint types
		/// </summary>
		/// <param name="e">DiagramPaintEventArgs</param>
		public override void OnPaintShape(DiagramPaintEventArgs e)
		{
			base.OnPaintShape(e);
			Pen pen = StyleSet.GetPen(OutlinePenId);

			// Keep the pen color in sync with the color being used for highlighting
			Color startColor = UpdateGeometryLuminosity(e.View, pen);
			bool restoreColor = startColor != pen.Color;
			IConstraint constraint = AssociatedConstraint;
			RectangleD bounds = AbsoluteBounds;
			Graphics g = e.Graphics;

			switch (constraint.ConstraintType)
			{
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
				case ConstraintType.DisjunctiveMandatory:
				{
					// Draw the dot
					bounds.Inflate(-Bounds.Width * .22, -Bounds.Height * .22);
					Brush brush = StyleSet.GetBrush(ExternalConstraintBrush);
					SolidBrush coloredBrush = brush as SolidBrush;
					coloredBrush.Color = pen.Color;
					g.FillEllipse(brush, RectangleD.ToRectangleF(bounds));
					break;
				}
				case ConstraintType.Exclusion:
				{
					const double cos45 = 0.70710678118654752440084436210485;
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
				case ConstraintType.ExternalUniqueness:
				{
					// Draw a single line for a uniqueness constraint and a double
					// line for preferred uniqueness
					ExternalUniquenessConstraint euc = constraint as ExternalUniquenessConstraint;
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
			}
			// Restore pen color
			if (restoreColor)
			{
				pen.Color = startColor;
			}
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
					&& object.ReferenceEquals(ormDiagram.StickyObject, this))
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
		/// <summary>
		/// Helper function for rules
		/// </summary>
		/// <param name="element">The model element to redraw</param>
		private static void InvalidateElementPresentation(ModelElement element)
		{
			foreach (object obj in element.AssociatedPresentationElements)
			{
				ShapeElement shape = obj as ShapeElement;
				if (shape != null)
				{
					shape.Invalidate();
				}
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
		#region Shape display update rules
		#region ExternalUniquenessConstraint ShapeChangeRule class
		[RuleOn(typeof(ExternalUniquenessConstraint), FireTime = TimeToFire.TopLevelCommit)]
		private class ShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				// UNDONE: Why is this here? A rule on a derived attribute
				// is highly questionable. Any updates should be made based on
				// changes to the underlying relationship.
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == ExternalUniquenessConstraint.IsPreferredMetaAttributeGuid)
				{
					InvalidateElementPresentation(e.ModelElement);
				}
			}
		}
		#endregion // ExternalUniquenessConstraint ShapeChangeRule class
		#endregion // Shape display update rules
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		/// <param name="error">Activated model error</param>
		protected void ActivateModelError(ModelError error)
		{
			if (error is TooFewRoleSequencesError)
			{
				ORMDiagram diagram = Diagram as ORMDiagram;
				// UNDONE: We may want to do something different here, like
				// making this the sticky object instead of directly chaining
				// the mouse action.
				diagram.ExternalConstraintConnectAction.ChainMouseAction(this, diagram.ActiveDiagramView.DiagramClientView);
			}
		}
		void IModelErrorActivation.ActivateModelError(ModelError error)
		{
			ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region IStickyObject implementation
		/// <summary>
		/// Implements IStickyObject.StickyInitialize
		/// </summary>
		protected void StickyInitialize()
		{
			RedrawAssociatedPels();
		}
		void IStickyObject.StickyInitialize()
		{
			StickyInitialize();
		}
		/// <summary>
		/// Implements IStickyObject.StickySelectable
		/// </summary>
		/// <param name="mel"></param>
		/// <returns></returns>
		protected bool StickySelectable(ModelElement mel)
		{
			bool rVal = false;
			Role r;
			if (mel == this.AssociatedConstraint)
			{
				rVal = true;
			}
			else if (null != (r = mel as Role))
			{
				MultiColumnExternalConstraint mcec;
				if (null != (mcec = AssociatedConstraint as MultiColumnExternalConstraint))
				{
					foreach (MultiColumnExternalConstraintRoleSequence roleSequence in mcec.RoleSequenceCollection)
					{
						if (roleSequence.RoleCollection.IndexOf(r) >= 0)
						{
							rVal = true;
						}
					}
				}
			}
			return rVal;
		}
		bool IStickyObject.StickySelectable(ModelElement mel)
		{
			return StickySelectable(mel);
		}
		/// <summary>
		/// Implements IStickyObject.StickySelectable
		/// </summary>
		protected void StickyRedraw()
		{
			RedrawAssociatedPels();
		}
		/// <summary>
		/// Implements IStickyObject.StickySelectable
		/// </summary>
		void IStickyObject.StickyRedraw()
		{
			StickyRedraw();
		}
		private void RedrawAssociatedPels()
		{
			IConstraint constraint = AssociatedConstraint;
			if (null != constraint)
			{
				Diagram diagram = this.Diagram;
				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SingleColumnExternalConstraint:
						SingleColumnExternalConstraint scec = constraint as SingleColumnExternalConstraint;
						foreach (SingleColumnExternalFactConstraint factConstraint in scec.GetElementLinks(SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid))
						{
							// Redraw the line
							RedrawPelsOnDiagram(factConstraint, diagram);
							// Redraw the fact type
							RedrawPelsOnDiagram(factConstraint.FactTypeCollection, diagram);
						}
						break;
					case ConstraintStorageStyle.MultiColumnExternalConstraint:
						MultiColumnExternalConstraint mcec = constraint as MultiColumnExternalConstraint;
						foreach (MultiColumnExternalFactConstraint factConstraint in mcec.GetElementLinks(MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid))
						{
							// Redraw the line
							RedrawPelsOnDiagram(factConstraint, diagram);
							// Redraw the fact type
							RedrawPelsOnDiagram(factConstraint.FactTypeCollection, diagram);
						}
						break;
				}
			}
			Invalidate(true);
		}
		private static void RedrawPelsOnDiagram(ModelElement element, Diagram diagram)
		{
			PresentationElementMoveableCollection pels = element.PresentationRolePlayers;
			int pelsCount = pels.Count;
			for (int i = 0; i < pelsCount; ++i)
			{
				ShapeElement shape = pels[i] as ShapeElement;
				if (shape != null && object.ReferenceEquals(shape.Diagram, diagram))
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
			ORMDiagram diagram;
			IConstraint constraint;
			if (null != (diagram = this.Diagram as ORMDiagram)
				&& diagram.StickyObject == this
				&& null != (constraint = this.AssociatedConstraint))
			{
				ExternalConstraintConnectAction connectAction = diagram.ExternalConstraintConnectAction;

				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SingleColumnExternalConstraint:
						Debug.Assert(this.AssociatedConstraint != null);
						connectAction.ConstraintRoleSequenceToEdit = constraint as ConstraintRoleSequence;
						break;
					default:
						break;
				}

				if (!connectAction.IsActive)
				{
					connectAction.ChainMouseAction(this, e.DiagramClientView);
				}
			}
		}
		#endregion // Mouse Handling
		#region Store Event Handlers
		/// <summary>
		/// Attach event handlers to the store
		/// </summary>
		/// <param name="s"></param>
		public static void AttachEventHandlers(Store s)
		{
			MetaDataDirectory dataDirectory = s.MetaDataDirectory;
			EventManagerDirectory eventDirectory = s.EventManagerDirectory;

			MetaRoleInfo roleInfo = dataDirectory.FindMetaRole(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
			eventDirectory.RolePlayerOrderChanged.Add(roleInfo, new RolePlayerOrderChangedEventHandler(RolePlayerOrderChangedEvent));
			roleInfo = dataDirectory.FindMetaRole(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
		}
		/// <summary>
		/// Detach event handlers from the store
		/// </summary>
		/// <param name="s"></param>
		public static void DetachEventHandlers(Store s)
		{
			MetaDataDirectory dataDirectory = s.MetaDataDirectory;
			EventManagerDirectory eventDirectory = s.EventManagerDirectory;

			MetaRoleInfo roleInfo = dataDirectory.FindMetaRole(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
			eventDirectory.RolePlayerOrderChanged.Remove(roleInfo, new RolePlayerOrderChangedEventHandler(RolePlayerOrderChangedEvent));
			roleInfo = dataDirectory.FindMetaRole(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
		}
		private static void RolePlayerOrderChangedEvent(object sender, RolePlayerOrderChangedEventArgs e)
		{
			MultiColumnExternalConstraint constraint;
			ExternalConstraintShape ecs;
			ORMDiagram ormDiagram;
			if (null != (constraint = e.SourceElement as MultiColumnExternalConstraint))
			{
				foreach (PresentationElement pel in constraint.AssociatedPresentationElements)
				{
					if (null != (ecs = pel as ExternalConstraintShape)
						&& null != (ormDiagram = ecs.Diagram as ORMDiagram)
						&& object.ReferenceEquals(ecs, ormDiagram.StickyObject))
					{
						ormDiagram.StickyObject.StickyRedraw();
					}
				}
			}
		}
		#endregion // Store Event Handlers
	}
}
