#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	#region ValueConstraintShapeDisplayBoundPicker class
	/// <summary>
	/// An element picker to hide the fact that 0 means unlimited for the
	/// <see cref="ValueConstraintShape.MaximumDisplayedColumns"/> and <see cref="ValueConstraintShape.MaximumDisplayedValues"/>
	/// properties.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ValueConstraintShapeDisplayBoundPicker : ElementPicker<ValueConstraintShapeDisplayBoundPicker>
	{
		private short myStartValue;
		/// <summary>
		/// Returns the 'Unlimited' value, as well as the current value if it is not unlimited
		/// </summary>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			short shortValue = (short)value;
			myStartValue = shortValue;
			return (shortValue == 0) ?
				new string[] { ResourceStrings.ValueConstraintShapeUnlimitedDisplayBoundText } :
				new string[] { shortValue.ToString(CultureInfo.CurrentCulture), ResourceStrings.ValueConstraintShapeUnlimitedDisplayBoundText };
		}
		/// <summary>
		/// Translate a value back to the integer. This will be either the starting value or 0, meaning unlimited
		/// </summary>
		protected override object TranslateFromDisplayObject(int newIndex, object newObject)
		{
			return (newIndex == 1) ? (short)0 : myStartValue;
		}
		/// <summary>
		/// Get the display object for the current value
		/// </summary>
		protected override object TranslateToDisplayObject(object initialObject, IList contentList)
		{
			short value = (short)initialObject;
			return (value == 0 && contentList.Count == 2) ? contentList[1] : contentList[0];
		}
	}
	#endregion // ValueConstraintShapeDisplayBoundPicker class
}
