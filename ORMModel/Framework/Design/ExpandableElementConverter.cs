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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// An <see cref="ExpandableObjectConverter"/> for <see cref="ModelElement"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ExpandableElementConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ExpandableElementConverter"/>.
		/// </summary>
		public ExpandableElementConverter()
			: base()
		{
		}

		/// <summary>See <see cref="TypeConverter.CanConvertFrom(ITypeDescriptorContext,Type)"/>.</summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			object component;
			PropertyDescriptor propertyDescriptor;
			ModelElement element;
			if (sourceType == typeof(string) &&
				context != null &&
				(component = context.Instance) != null &&
				(propertyDescriptor = context.PropertyDescriptor) != null &&
				(element = propertyDescriptor.GetValue(component) as ModelElement) != null)
			{
				return DomainClassInfo.HasNameProperty(element);
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>See <see cref="TypeConverter.ConvertFrom(ITypeDescriptorContext,CultureInfo,Object)"/>.</summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			object component;
			PropertyDescriptor elementPropertyDescriptor;
			string stringValue;
			ModelElement element;
			PropertyDescriptor namePropertyDescriptor;
			if (context != null &&
				(component = context.Instance) != null &&
				(elementPropertyDescriptor = context.PropertyDescriptor) != null &&
				(object)(stringValue = value as string) != null &&
				(element = elementPropertyDescriptor.GetValue(component) as ModelElement) != null &&
				(namePropertyDescriptor = DomainTypeDescriptor.CreateNamePropertyDescriptor(element)) != null)
			{
				namePropertyDescriptor.SetValue(element, stringValue);
				return element;
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>See <see cref="TypeConverter.ConvertTo(ITypeDescriptorContext,CultureInfo,Object,Type)"/>.</summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ModelElement element;
			string name;
			if (destinationType == typeof(string) && (element = value as ModelElement) != null && DomainClassInfo.TryGetName(element, out name))
			{
				return name;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
