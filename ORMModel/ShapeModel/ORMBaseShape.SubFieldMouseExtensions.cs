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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// An extension to NodeShape to enable Enter/Leave/Hover events
	/// on individual ShapeSubField areas. This should be in the base
	/// framework and would be more consistent there because virtual methods
	/// for these events could appear directly on ShapeSubField.
	/// </summary>
	public partial class ORMBaseShape : IHandleSubFieldMoveMove
	{
		#region Extension to add SubShape Enter/Leave/Hover events
		/// <summary>
		/// Turn off highlighting by default if sub field highlighting is on
		/// </summary>
		public override bool HasHighlighting
		{
			get
			{
				return !HasSubFieldHighlighting;
			}
		}
		/// <summary>
		/// Turn on to get highlighting of subfields instead of the whole shape
		/// </summary>
		public virtual bool HasSubFieldHighlighting
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Turn on to get Enter/Leave/Hover events on shape fields. Turning
		/// on sub shape field highlighting also means that MouseLeave and MouseEnter
		/// events are fired when the mouse moves between a ShapeSubField region
		/// and the normal ShapeNode region. This means that a MouseEnter can fire
		/// multiple times without leaving the object.
		/// 
		/// The default implementation turns this on if HasSubFieldHighlighting
		/// is turned on.
		/// </summary>
		/// <value></value>
		public virtual bool HasSubFieldMouseEnterLeaveHover
		{
			get
			{
				return HasSubFieldHighlighting;
			}
		}
		/// <summary>
		/// A shape field used to support Enter/Leave/Hover events on shape fields.
		/// Set to null after a MouseLeave, PendingShapeSubField.Token between an enter or
		/// move with no SubField set and a leave, and set to the last subfield otherwise.
		/// </summary>
		private static ShapeSubField ActiveShapeSubField;
		/// <summary>
		/// Used with ActiveShapeSubField to give an accurate leave event.
		/// </summary>
		private static ShapeField ActiveShapeField;
		/// <summary>
		/// A helper class to enable distinguishing between a null
		/// and a pending shape field.
		/// </summary>
		private sealed class PendingShapeSubField : ORMBaseShapeSubField
		{
			public static readonly PendingShapeSubField Token = new PendingShapeSubField();
			private PendingShapeSubField()
			{
			}
			public sealed override bool SubFieldEquals(object obj)
			{
				// Only one instance is created, so there is not much to do
				return this == obj;
			}
			public sealed override int SubFieldHashCode
			{
				get
				{
					return 1;
				}
			}
			public sealed override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return false;
			}
			public sealed override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
			{
				return false;
			}
			public sealed override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				return RectangleD.Empty;
			}
		}
		/// <summary>
		/// Fired when the mouse enters a subfield. This is called directly
		/// by OnMouseEnter.
		/// 
		/// If HasSubFieldHighlighting is set, the default implementation automatically
		/// sets the HighlightedShapes of the active view to the DiagramItem representing
		/// this subfield. The subfield item will be invalidated and repainted. It is
		/// up to shapefield to check the HighlightedShapes collection and draw the subfield
		/// highlighting explicitly.
		/// 
		/// Note that introducing a new ShapeSubField-derived base class would allow this
		/// to be called as the default implementation of an OnMouseEnter event on that class.
		/// However, this would need to be done at the framework level because ShapeSubField
		/// is already inherited from. Another alternative is to introduce an interface for
		/// the Enter/Leave/Hover events along with a stock implementation to help implement it.
		/// </summary>
		/// <param name="field">The parent ShapeField</param>
		/// <param name="subField">The subfield being entered</param>
		/// <param name="e">Forwarded from the OnMouseEnter event</param>
		public virtual void OnSubFieldMouseEnter(ShapeField field, ShapeSubField subField, DiagramPointEventArgs e)
		{
			if (this.HasSubFieldHighlighting)
			{
				DiagramClientView view = e.DiagramClientView;
				if (view != null)
				{
					view.HighlightedShapes.Set(new DiagramItem(this, field, subField));
				}
			}
		}
		/// <summary>
		/// Fired when the mouse leaves a subfield. This is called directly
		/// by OnMouseLeave.
		/// 
		/// If HasSubFieldHighlighting is set, the default implementation automatically
		/// removes the DiagramItem representing this subfield from the HighlightedShapes
		///  of the active view. The subfield item will be invalidated and repainted. It is
		/// up to shapefield to check the HighlightedShapes collection and draw the subfield
		/// highlighting explicitly.
		///
		/// Note that introducing a new ShapeSubField-derived base class would allow this
		/// to be called as the default implementation of an OnMouseLeave event on that class.
		/// However, this would need to be done at the framework level because ShapeSubField
		/// is already inherited from. Another alternative is to introduce an interface for
		/// the Enter/Leave/Hover events along with a stock implementation to help implement it.
		/// </summary>
		/// <param name="field">The parent ShapeField</param>
		/// <param name="subField">The subfield being left</param>
		/// <param name="e">Forwarded from the OnMouseLeave event</param>
		public virtual void OnSubFieldMouseLeave(ShapeField field, ShapeSubField subField, DiagramPointEventArgs e)
		{
			if (this.HasSubFieldHighlighting)
			{
				e.DiagramClientView.HighlightedShapes.Remove(new DiagramItem(this, field, subField));
			}
		}
		/// <summary>
		/// Fired when the mouse hovers over a subfield. This is called directly
		/// by OnMouseHover.
		/// 
		/// The default implementation is empty.
		/// 
		/// Note that introducing a new ShapeSubField-derived base class would allow this
		/// to be called as the default implementation of an OnMouseHover event on that class.
		/// However, this would need to be done at the framework level because ShapeSubField
		/// is already inherited from. Another alternative is to introduce an interface for
		/// the Enter/Leave/Hover events along with a stock implementation to help implement it.
		/// </summary>
		/// <param name="field">The parent ShapeField</param>
		/// <param name="subField">The subfield being hovered over</param>
		/// <param name="e">Forwarded from the OnMouseHover event</param>
		public virtual void OnSubFieldMouseHover(ShapeField field, ShapeSubField subField, DiagramPointEventArgs e)
		{
		}
		/// <summary>
		/// Translate OnSubFieldMouseMove events into OnSubFieldMouseEnter and OnSubFieldMouseMove
		/// events
		/// </summary>
		public virtual void OnSubFieldMouseMove(ShapeField field, ShapeSubField subField, DiagramMouseEventArgs e)
		{
			if (HasSubFieldMouseEnterLeaveHover)
			{
				ShapeSubField oldSubField = ActiveShapeSubField;
				if (oldSubField != null)
				{
					if (PendingShapeSubField.Token.SubFieldEquals(oldSubField))
					{
						ActiveShapeSubField = subField;
						ActiveShapeField = field;
						base.OnMouseLeave(e);
						OnSubFieldMouseEnter(field, subField, e);
					}
					else if (!oldSubField.SubFieldEquals(subField))
					{
						OnSubFieldMouseLeave(ActiveShapeField, oldSubField, e);
						ActiveShapeSubField = subField;
						ActiveShapeField = field;
						OnSubFieldMouseEnter(field, subField, e);
					}
				}
				else
				{
					ActiveShapeSubField = subField;
					ActiveShapeField = field;
					OnSubFieldMouseEnter(field, subField, e);
				}
				e.Handled = true; // Must mark as handled, or the framework will forward this OnMouseMove
			}
		}
		/// <summary>
		/// Translate mouse events into OnSubFieldMouseEnter/OnSubFieldMouseLeave events
		/// </summary>
		/// <param name="e">DiagramMouseEventArgs</param>
		public override void OnMouseMove(DiagramMouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (HasSubFieldMouseEnterLeaveHover)
			{
				Debug.Assert(e.DiagramHitTestInfo.HitDiagramItem.SubField == null); // Should be in OnSubFieldMouseMove, not here
				ShapeSubField oldSubField = ActiveShapeSubField;
				if (oldSubField != null && !PendingShapeSubField.Token.SubFieldEquals(oldSubField))
				{
					ShapeField oldShapeField = ActiveShapeField;
					ActiveShapeField = null;
					ActiveShapeSubField = PendingShapeSubField.Token;
					OnSubFieldMouseLeave(oldShapeField, oldSubField, e);
					base.OnMouseEnter(e);
				}
			}
		}
		/// <summary>
		/// Translate mouse events into OnSubFieldMouseEnter/OnSubFieldMouseLeave events
		/// </summary>
		/// <param name="e">DiagramPointEventArgs</param>
		public override void OnMouseEnter(DiagramPointEventArgs e)
		{
			base.OnMouseEnter(e);
			if (HasSubFieldMouseEnterLeaveHover)
			{
				DiagramItem item = e.DiagramHitTestInfo.HitDiagramItem;
				ShapeSubField subField = item.SubField;
				ShapeField shapeField = item.Field;
				// Note that we ignore any existing cached fields here. They
				// should be null at this point, but ignoring them guards against
				// the possibility that a leave event did not fire.
				if (subField == null || shapeField == null)
				{
					ActiveShapeSubField = PendingShapeSubField.Token;
					ActiveShapeField = null;
				}
				else
				{
					ActiveShapeSubField = subField;
					ActiveShapeField = shapeField;
					OnSubFieldMouseEnter(shapeField, subField, e);
				}
			}
		}
		/// <summary>
		/// Translate mouse events into OnSubFieldMouseEnter/OnSubFieldMouseLeave events
		/// </summary>
		/// <param name="e">DiagramPointEventArgs</param>
		public override void OnMouseLeave(DiagramPointEventArgs e)
		{
			base.OnMouseLeave(e);
			if (HasSubFieldMouseEnterLeaveHover)
			{
				ShapeSubField subField = ActiveShapeSubField;
				ActiveShapeSubField = null;
				if (subField != null && !PendingShapeSubField.Token.SubFieldEquals(subField))
				{
					ShapeField shapeField = ActiveShapeField;
					ActiveShapeField = null;
					OnSubFieldMouseLeave(shapeField, subField, e);
				}
			}
		}
		/// <summary>
		/// Translate mouse event into OnSubFieldMouseHover events
		/// </summary>
		/// <param name="e">DiagramPointEventArgs</param>
		public override void OnMouseHover(DiagramPointEventArgs e)
		{
			base.OnMouseHover(e);
			if (HasSubFieldMouseEnterLeaveHover)
			{
				ShapeSubField subField = ActiveShapeSubField;
				if (subField != null && !PendingShapeSubField.Token.SubFieldEquals(subField))
				{
					Debug.Assert(ActiveShapeField != null); // Should be able to set sub field without shape field
					OnSubFieldMouseHover(ActiveShapeField, subField, e);
				}
			}
		}
		#endregion // Extension to add SubShape Enter/Leave/Hover events
	}
}
