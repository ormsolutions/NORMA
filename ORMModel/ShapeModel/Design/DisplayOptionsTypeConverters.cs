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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	#region CustomBinaryFactTypeReadingDisplayTypeConverter class
	/// <summary>
	/// Type converter to reflect the current default value in the display
	/// of the <see cref="P:ORMDiagram.DisplayReverseReadings"/> or <see cref="P:FactTypeShape.DisplayReverseReading"/> property.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[CLSCompliant(false)]
	public class CustomBinaryFactTypeReadingDisplayTypeConverter : EnumConverter<CustomBinaryFactTypeReadingDisplay, ORMDiagram>
	{
		/// <summary>
		/// Modify the default value string to include the data from the
		/// current default display settings.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal = null;
			if (context != null &&
				destinationType == typeof(string) &&
				!CultureInfo.InvariantCulture.Equals(culture)) // Used for serialization, not display
			{
				object instance;
				if (value is CustomBinaryFactTypeReadingDisplay &&
					((CustomBinaryFactTypeReadingDisplay)value) == CustomBinaryFactTypeReadingDisplay.Default &&
					null != (instance = context.Instance))
				{
					ORMDiagram diagram;
					FactTypeShape shape;
					bool useDefault = false;
					object resolvedDefault = null;
					if (null != (diagram = instance as ORMDiagram))
					{
						// We've already checked the value as default
						useDefault = true;
					}
					else if (null != (shape = instance as FactTypeShape) && null != (diagram = shape.Diagram as ORMDiagram))
					{
						useDefault = true;
						CustomBinaryFactTypeReadingDisplay customDisplay = diagram.DisplayReverseReadings;
						if (customDisplay != CustomBinaryFactTypeReadingDisplay.Default)
						{
							resolvedDefault = customDisplay;
						}
					}
					if (useDefault)
					{
						retVal = string.Format(culture, (string)base.ConvertTo(context, culture, value, destinationType), TypeDescriptor.GetConverter(typeof(CustomBinaryFactTypeReadingDisplay)).ConvertToString(resolvedDefault != null ? resolvedDefault : diagram.Store.ElementDirectory.FindElements<ORMDiagramDisplayOptions>()[0].DisplayReverseReadings == BinaryFactTypeReadingDisplay.ShowReverseReading ? CustomBinaryFactTypeReadingDisplay.ShowReverseReading : CustomBinaryFactTypeReadingDisplay.OnlyOneReading));
					}
				}
			}
			return retVal != null ? retVal : base.ConvertTo(context, culture, value, destinationType);
		}
		/// <summary>
		/// Match the default display settings value to the current string before
		/// turning this over to the base, which will throw when it does not recognize
		/// the value.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string valueString;
			object defaultValue;
			if (context != null &&
				null != (valueString = value as string) &&
				valueString.Equals(ConvertTo(context, culture, defaultValue = CustomBinaryFactTypeReadingDisplay.Default, typeof(string)) as string))
			{
				return defaultValue;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
	#endregion // CustomBinaryFactTypeReadingDisplayTypeConverter class
	#region CustomRoleNameDisplayTypeConverter class
	/// <summary>
	/// Type converter to reflect the current default value in the display
	/// of the <see cref="P:ORMDiagram.DisplayRoleNames"/> or <see cref="P:FactTypeShape.DisplayRoleNames"/> property.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[CLSCompliant(false)]
	public class CustomRoleNameDisplayTypeConverter : EnumConverter<CustomRoleNameDisplay, ORMDiagram>
	{
		/// <summary>
		/// Modify the default value string to include the data from the
		/// current default display settings.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal = null;
			if (context != null &&
				destinationType == typeof(string) &&
				!CultureInfo.InvariantCulture.Equals(culture)) // Used for serialization, not display
			{
				object instance;
				if (value is CustomRoleNameDisplay &&
					((CustomRoleNameDisplay)value) == CustomRoleNameDisplay.Default &&
					null != (instance = context.Instance))
				{
					ORMDiagram diagram;
					FactTypeShape shape;
					bool useDefault = false;
					object resolvedDefault = null;
					if (null != (diagram = instance as ORMDiagram))
					{
						// We've already checked the value as default
						useDefault = true;
					}
					else if (null != (shape = instance as FactTypeShape) && null != (diagram = shape.Diagram as ORMDiagram))
					{
						useDefault = true;
						CustomRoleNameDisplay customDisplay = diagram.DisplayRoleNames;
						if (customDisplay != CustomRoleNameDisplay.Default)
						{
							resolvedDefault = customDisplay;
						}
					}
					if (useDefault)
					{
						retVal = string.Format(culture, (string)base.ConvertTo(context, culture, value, destinationType), TypeDescriptor.GetConverter(typeof(CustomRoleNameDisplay)).ConvertToString(resolvedDefault != null ? resolvedDefault : diagram.Store.ElementDirectory.FindElements<ORMDiagramDisplayOptions>()[0].DisplayRoleNames == RoleNameDisplay.On ? CustomRoleNameDisplay.On : CustomRoleNameDisplay.Off));
					}
				}
			}
			return retVal != null ? retVal : base.ConvertTo(context, culture, value, destinationType);
		}
		/// <summary>
		/// Match the default display settings value to the current string before
		/// turning this over to the base, which will throw when it does not recognize
		/// the value.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string valueString;
			object defaultValue;
			if (context != null &&
				null != (valueString = value as string) &&
				valueString.Equals(ConvertTo(context, culture, defaultValue = CustomRoleNameDisplay.Default, typeof(string)) as string))
			{
				return defaultValue;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
	#endregion // CustomRoleNameDisplayTypeConverter class
	#region CustomReadingDirectionIndicatorDisplayTypeConverter class
	/// <summary>
	/// Type converter to reflect the current default value in the display
	/// of the <see cref="P:ORMDiagram.DisplayReadingDirection"/> or <see cref="P:FactTypeShape.DisplayReadingDirection"/> property.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[CLSCompliant(false)]
	public class CustomReadingDirectionIndicatorDisplayTypeConverter : EnumConverter<CustomReadingDirectionIndicatorDisplay, ORMDiagram>
	{
		/// <summary>
		/// Modify the default value string to include the data from the
		/// current default display settings.
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal = null;
			if (context != null &&
				destinationType == typeof(string) &&
				!CultureInfo.InvariantCulture.Equals(culture)) // Used for serialization, not display
			{
				object instance;
				if (value is CustomReadingDirectionIndicatorDisplay &&
					((CustomReadingDirectionIndicatorDisplay)value) == CustomReadingDirectionIndicatorDisplay.Default &&
					null != (instance = context.Instance))
				{
					ORMDiagram diagram;
					FactTypeShape shape;
					bool useDefault = false;
					object resolvedDefault = null;
					if (null != (diagram = instance as ORMDiagram))
					{
						// We've already checked the value as default
						useDefault = true;
					}
					else if (null != (shape = instance as FactTypeShape) && null != (diagram = shape.Diagram as ORMDiagram))
					{
						useDefault = true;
						CustomReadingDirectionIndicatorDisplay customDisplay = diagram.DisplayReadingDirection;
						if (customDisplay != CustomReadingDirectionIndicatorDisplay.Default)
						{
							resolvedDefault = customDisplay;
						}
					}
					if (useDefault)
					{
						retVal = string.Format(culture, (string)base.ConvertTo(context, culture, value, destinationType), TypeDescriptor.GetConverter(typeof(CustomReadingDirectionIndicatorDisplay)).ConvertToString(resolvedDefault != null ? resolvedDefault : (CustomRoleNameDisplay)diagram.Store.ElementDirectory.FindElements<ORMDiagramDisplayOptions>()[0].DisplayReadingDirection));
					}
				}
			}
			return retVal != null ? retVal : base.ConvertTo(context, culture, value, destinationType);
		}
		/// <summary>
		/// Match the default display settings value to the current string before
		/// turning this over to the base, which will throw when it does not recognize
		/// the value.
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string valueString;
			object defaultValue;
			if (context != null &&
				null != (valueString = value as string) &&
				valueString.Equals(ConvertTo(context, culture, defaultValue = CustomReadingDirectionIndicatorDisplay.Default, typeof(string)) as string))
			{
				return defaultValue;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
	#endregion // CustomReadingDirectionIndicatorDisplayTypeConverter class

}
