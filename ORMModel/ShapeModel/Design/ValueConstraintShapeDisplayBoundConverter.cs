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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	#region ValueConstraintShapeDisplayBoundConverter class
	/// <summary>
	/// <see cref="TypeConverter"/> for <see cref="ValueConstraintShape.MaximumDisplayedColumns"/> and
	/// <see cref="ValueConstraintShape.MaximumDisplayedValues"/> properties
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ValueConstraintShapeDisplayBoundConverter : TypeConverter
	{
		/// <summary>
		/// Standard override. Allow string conversion.
		/// </summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}
		/// <summary>
		/// Standard override. Map non-positive values to 0, meaning unlimited.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = (value == null) ? "" : value.ToString();
			if (string.IsNullOrEmpty(stringValue) || 0 == string.Compare(stringValue, ResourceStrings.ValueConstraintShapeUnlimitedDisplayBoundText, StringComparison.CurrentCultureIgnoreCase))
			{
				return (short)0;
			}
			short result = short.Parse(stringValue);
			return (result >= 0) ? result : (short)0;
		}
		/// <summary>
		/// Standard override. Show 'Unlimited' for the 0 value.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				short shortValue = (short)value;
				if (shortValue == 0)
				{
					return ResourceStrings.ValueConstraintShapeUnlimitedDisplayBoundText;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	#endregion // ValueConstraintShapeDisplayBoundConverter class
}
