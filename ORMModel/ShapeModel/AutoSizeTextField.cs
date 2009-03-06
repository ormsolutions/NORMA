#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// An extension of a TextField shape. The minimum size is recalculated
	/// according to the current shape contents.
	/// </summary>
	public class AutoSizeTextField : DynamicColorTextField
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field, forwarded to base class.</param>
		public AutoSizeTextField(string fieldName)
			: base(fieldName)
		{
			base.DefaultAutoSize = true;
		}
		/// <summary>
		/// Always returns <see langword="true"/>.
		/// </summary>
		public sealed override bool GetAutoSize(ShapeElement parentShape)
		{
			return true;
		}
		/// <summary>
		/// Gets the minimum <see cref="SizeD"/> of this <see cref="AutoSizeTextField"/> for the current text.
		/// </summary>
		/// <param name="parentShape">
		/// The <see cref="ShapeElement"/> that this <see cref="AutoSizeTextField"/> is associated with.
		/// </param>
		/// <returns>The minimum <see cref="SizeD"/> of this <see cref="AutoSizeTextField"/>.</returns>
		public override SizeD GetMinimumSize(ShapeElement parentShape)
		{
			string text = GetDisplayText(parentShape);
			if (!string.IsNullOrEmpty(text))
			{
				using (Font font = GetFont(parentShape))
				{
					return base.MeasureDisplayText(text, font, null, parentShape.MaximumSize);
				}
			}
			return SizeD.Empty;
		}
	}
}
