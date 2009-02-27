#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using System.Drawing;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ComponentModel;
using Microsoft.VisualStudio.OLE.Interop;

namespace Neumont.Tools.Modeling.Diagrams
{
	#region IShapeExtender interface
	/// <summary>
	/// Provides a generic interface to use from <see cref="ShapeElement"/> overrides for supporting
	/// add child shapes beyond those specified in the domain model for the main shape.
	/// </summary>
	public interface IShapeExtender<S> where S : ShapeElement
	{
		/// <summary>
		/// Extension callback for the <see cref="M:ShapeElement.ShouldAddShapeForElement"/> method
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="element">The element to add to the diagram</param>
		/// <returns><see langword="true"/> to continue with shape creation for this element</returns>
		bool ShouldAddShapeForElement(S extendedShape, ModelElement element);
		/// <summary>
		/// Extension callback for the <see cref="M:ShapeElement.CreateChildShape"/> method
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="element">The element to create a child shape for</param>
		/// <returns>A new child shape</returns>
		ShapeElement CreateChildShape(S extendedShape, ModelElement element);
		/// <summary>
		/// Get toolbox filter attributes used by this extender
		/// </summary>
		ICollection<ToolboxItemFilterAttribute> GetToolboxFilterAttributes();
		/// <summary>
		/// Get the mouse action for the selected toolbox item, which can
		/// be tested with the <see cref="M:DiagramView.SelectedToolboxItemSupportsFilterString"/>
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="activeView">The active <see cref="DiagramView"/> for the shape</param>
		/// <returns>A <see cref="MouseAction"/>, or null if the selected item does not match.</returns>
		MouseAction GetMouseAction(S extendedShape, DiagramView activeView);
		/// <summary>
		/// Provide drag feedback in a drag over. Called if the <paramref name="extendedShape"/> does not
		/// directly handle the drag over event. If the extender handles this event, then the <see cref="P:DiagramDragEventArgs.Handled"/>
		/// property should be set before returning.
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="e">Forwarded <see cref="DiagramDragEventArgs"/></param>
		void OnDragOver(S extendedShape, DiagramDragEventArgs e);
		/// <summary>
		/// Provide drag feedback in a drag drop event. Called if the <paramref name="extendedShape"/> does not
		/// directly handle the drag drop event. If the extender handles this event, then the <see cref="P:DiagramDragEventArgs.Handled"/>
		/// property should be set before returning.
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="e">Forwarded <see cref="DiagramDragEventArgs"/></param>
		void OnDragDrop(S extendedShape, DiagramDragEventArgs e);
		/// <summary>
		/// If the extended shape implements <see cref="IDisposable"/>, then allow any shape extenders
		/// to explicitly dispose state as well
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		void ExtendedShapeDisposed(S extendedShape);
	}
	#endregion // IShapeExtender interface
	#region IConfigureChildShape interface
	/// <summary>
	/// An interface to implement <see cref="M:ShapeElement.OnChildConfiguring"/>
	/// on the child shape instead of the parent. Implementing child configuration
	/// on the parent shape requires that the parent know the full set of child
	/// shape elements.
	/// </summary>
	public interface IConfigureAsChildShape
	{
		/// <summary>
		/// Called during <see cref="M:ShapeElement.OnChildConfiguring"/> from the parent shape to
		/// configure a shape immediately after it is added as a child of the parent shape or diagram.
		/// </summary>
		/// <param name="parentShape">The parent shape or diagram</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup);
	}
	#endregion // IConfigureChildShape interface
	#region IDynamicShapeColorProvider interface
	/// <summary>
	/// An extension interface to support specifying custom
	/// colors for different shapes.
	/// </summary>
	/// <typeparam name="C">An <see cref="Enum"/> type specifying the supported
	/// set of colors. This should be the same <see cref="IDynamicColorSetConsumer{Diagram}.DynamicColorSet"/>
	/// proprerty used to match the colors.</typeparam>
	/// <typeparam name="S">The type of shape element being drawn</typeparam>
	/// <typeparam name="T">An element associated with the shape. This does
	/// not have to be the subject of the shape itself, allowing this interface
	/// to be used to draw items associated with <see cref="ShapeField"/> and
	/// <see cref="ShapeSubField"/> elements.</typeparam>
	public interface IDynamicShapeColorProvider<C, S, T>
		where C : struct
		where S : ShapeElement
		where T : class
	{
		/// <summary>
		/// Get the color for the specified <paramref name="colorRole"/>,
		/// or return <see cref="F:Color.Empty"/> if no data is available.
		/// </summary>
		/// <param name="colorRole">The role of the color to retrieve.</param>
		/// <param name="shapeElement">The <see cref="ShapeElement"/> to get an alternate color for.</param>
		/// <param name="elementPart">Generally, this is the <see cref="ModelElement"/> that corresponds to the
		/// <paramref name="shapeElement"/>, but the type is not restricted to ModelElement-derived instances.</param>
		Color GetDynamicColor(C colorRole, S shapeElement, T elementPart);
	}
	#endregion // IDynamicShapeColorProvider interface
	#region IDynamicColorSetConsumer interface
	/// <summary>
	/// Indicate that a <see cref="DomainModel"/> uses a
	/// color set <see cref="Enum"/> for a specific DiagramType.
	/// </summary>
	/// <typeparam name="D">The type of <see cref="Diagram"/> the element will be
	/// displayed on.</typeparam>
	public interface IDynamicColorSetConsumer<D>
		where D : Diagram
	{
		/// <summary>
		/// An <see cref="Enum"/> type that provides a list roles for the supported colors.
		/// </summary>
		Type DynamicColorSet { get;}
	}
	#endregion // IDynamicColorSetConsumer interface
	#region IDynamicColorAlsoUsedBy interface
	/// <summary>
	/// Support dynamic color updates on secondary shapes
	/// </summary>
	public interface IDynamicColorAlsoUsedBy
	{
		/// <summary>
		/// Get the set of all shapes that use the dynamic
		/// colors associated with the primary backing object
		/// for this displayed element.
		/// </summary>
		IEnumerable<ShapeElement> RelatedDynamicallyColoredShapes { get;}
	}
	#endregion // IDynamicColorAlsoUsedBy interface
	#region IInvalidateDisplay interface
	/// <summary>
	/// Implemented to support the general pattern of
	/// invalidating a <see cref="ShapeElement"/> from
	/// within a <see cref="Transaction"/> in such a way
	/// that the display will be refreshed during Undo/Redo
	/// cycles in addition to the original transaction.
	/// </summary>
	public interface IInvalidateDisplay
	{
		/// <summary>
		/// Invalidate the element. This eventually
		/// calls <see cref="ShapeElement.Invalidate(bool)"/> with
		/// the refreshBitmap parameter set to <see langword="false"/>
		/// </summary>
		void InvalidateRequired();
		/// <summary>
		/// Invalidate the element. This eventually
		/// calls <see cref="ShapeElement.Invalidate(bool)"/> method.
		/// </summary>
		/// <param name="refreshBitmap">Forwarded to the
		/// corresponding parameter on <see cref="ShapeElement.Invalidate(bool)"/>.
		/// Most uses pass <see langword="true"/> to this parameter.</param>
		void InvalidateRequired(bool refreshBitmap);
	}
	#endregion // IInvalidateDisplay interface
}
