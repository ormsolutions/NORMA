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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	#region FrequencyConstraintMaxPicker class
	/// <summary>
	/// An element picker to hide the fact that 0 means unbounded for the <see cref="FrequencyConstraint.MaxFrequency"/> property
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class FrequencyConstraintMaxPicker : ElementPicker<FrequencyConstraintMaxPicker>
	{
		private int myStartValue;
		/// <summary>
		/// Returns the Unbounded value, as well as the current value if it is not unbounded
		/// </summary>
		protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
		{
			int intValue = (int)value;
			myStartValue = intValue;
			return (intValue == 0) ?
				new string[]{ResourceStrings.FrequencyConstraintUnboundedMaxValueText} :
				new string[]{intValue.ToString(CultureInfo.CurrentCulture), ResourceStrings.FrequencyConstraintUnboundedMaxValueText};
		}
		/// <summary>
		/// Translate a value back to the integer. This will be either the starting value or 0, meaning unbounded
		/// </summary>
		protected override object TranslateFromDisplayObject(int newIndex, object newObject)
		{
			return (newIndex == 1) ? 0 : myStartValue;
		}
		/// <summary>
		/// Get the display object for the current value
		/// </summary>
		protected override object TranslateToDisplayObject(object initialObject, IList contentList)
		{
			int value = (int)initialObject;
			return (value == 0 && contentList.Count == 2) ? contentList[1] : contentList[0];
		}
	}
	#endregion // FrequencyConstraintMaxPicker class
}
