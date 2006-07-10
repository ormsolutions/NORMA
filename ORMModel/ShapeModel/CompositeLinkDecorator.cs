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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// Draw a link decorator by deferring painting to a list
	/// of other decorators.
	/// </summary>
	public abstract class CompositeLinkDecorator : LinkDecorator
	{
		private IList<LinkDecorator> myDecorators;
		/// <summary>
		/// Create a new CompositeLinkDecorator.
		/// </summary>
		protected CompositeLinkDecorator()
		{
		}
		/// <summary>
		/// Return a list of decorators to be drawn. The list will
		/// be drawn in reverse order, so the first specified decorator
		/// will be on top.
		/// </summary>
		protected abstract IList<LinkDecorator> DecoratorCollection { get;}
		/// <summary>
		/// Paint the composite shape decorators. 
		/// </summary>
		/// <param name="bounds">Forwarded to composite decorators</param>
		/// <param name="shape">Forwarded to composite decorators</param>
		/// <param name="e">Forwarded to composite decorators</param>
		public override void DoPaintShape(RectangleD bounds, IGeometryHost shape, DiagramPaintEventArgs e)
		{
			if (myDecorators == null)
			{
				myDecorators = DecoratorCollection;
				if (myDecorators == null)
				{
					myDecorators = new LinkDecorator[]{};
				}
			}
			for (int i = myDecorators.Count - 1; i >= 0; --i)
			{
				myDecorators[i].DoPaintShape(bounds, shape, e);
			}
		}
		/// <summary>
		/// Return a null path. Not used.
		/// </summary>
		protected override GraphicsPath GetPath(RectangleD bounds)
		{
			 Debug.Fail("Not used. If we see this is firing, then we need to extend " +
			 "this class to specify one of the composite paths (or a custom path) " +
			 "as the path to represent the whole item. Default to the last of the " +
			 "composite paths (the first one drawn).\r\n" +
			 "I have a feeling this may be used in the future if selection drawing " +
			 "around link lines is improved.");
			return null;
		}

	}
}  
