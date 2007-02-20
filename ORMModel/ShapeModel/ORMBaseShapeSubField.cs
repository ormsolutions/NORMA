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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// Implement to receive mouse move notifications from <see cref="ORMBaseShapeSubField"/>s.
	/// </summary>
	public interface IHandleSubFieldMouseMove
	{
		/// <summary>
		/// Called from <see cref="ShapeSubField.OnMouseMove"/> in <see cref="ORMBaseShapeSubField"/>.
		/// </summary>
		void OnSubFieldMouseMove(ShapeField field, ShapeSubField subField, DiagramMouseEventArgs e);
	}
	/// <summary>
	/// <see cref="ShapeSubField"/> that calls back to parent shape from <see cref="OnMouseMove"/>
	/// if parent shape implements <see cref="IHandleSubFieldMouseMove"/>.
	/// </summary>
	public abstract class ORMBaseShapeSubField : ShapeSubField
	{
		/// <summary>
		/// Calls back to parent's implementation of <see cref="IHandleSubFieldMouseMove.OnSubFieldMouseMove"/>.
		/// </summary>
		public override void OnMouseMove(DiagramMouseEventArgs e)
		{
			base.OnMouseMove(e);
			DiagramItem diagramItem = e.DiagramHitTestInfo.HitDiagramItem;
			if (diagramItem != null && diagramItem.Field != null)
			{
				IHandleSubFieldMouseMove handlerShape = diagramItem.Shape as IHandleSubFieldMouseMove;
				if (handlerShape != null)
				{
					handlerShape.OnSubFieldMouseMove(diagramItem.Field, diagramItem.SubField, e);
				}
			}
		}
	}
}
