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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="TypeConverter"/> for <see cref="FrequencyConstraint.MinFrequency"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FrequencyConstraintMinConverter : TypeConverter
	{
		/// <summary>
		/// Standard override. Allow string conversion.
		/// </summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}
		/// <summary>
		/// Standard override. Map any value less than 1 to 1.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = (value == null) ? "" : value.ToString();
			int result = int.Parse(stringValue);
			return (result > 1) ? result : 1;
		}
	}
	/// <summary>
	/// <see cref="TypeConverter"/> for <see cref="FrequencyConstraint.MaxFrequency"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FrequencyConstraintMaxConverter : TypeConverter
	{
		/// <summary>
		/// Standard override. Allow string conversion.
		/// </summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}
		/// <summary>
		/// Standard override. Map non-positive values to 0, meaning unbounded.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string stringValue = (value == null) ? "" : value.ToString();
			if (0 == string.Compare(stringValue, ResourceStrings.FrequencyConstraintUnboundedMaxValueText, StringComparison.CurrentCultureIgnoreCase))
			{
				return 0;
			}
			int result = int.Parse(stringValue);
			return (result > 0) ? result : 0;
		}
		/// <summary>
		/// Standard override. Show 'Unbounded' for the 0 value.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				int intValue = (int)value;
				if (intValue == 0)
				{
					return ResourceStrings.FrequencyConstraintUnboundedMaxValueText;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
