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
	/// <see cref="TypeConverter"/> for <see cref="ReferenceMode"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReferenceModeConverter : TypeConverter
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ReferenceModeConverter"/>.
		/// </summary>
		public ReferenceModeConverter()
			: base()
		{
		}
		/// <summary>See <see cref="TypeConverter.CanConvertFrom(ITypeDescriptorContext,Type)"/>.</summary>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}
		/// <summary>See <see cref="TypeConverter.ConvertFrom(ITypeDescriptorContext,CultureInfo,Object)"/>.</summary>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (context != null)
			{
				string refMode = value as string;
				if (refMode != null)
				{
					object instance = EditorUtility.ResolveContextInstance(context.Instance, true);
					FactType factType;
					ObjectType objectType;
					if (null != (objectType = instance as ObjectType) ||
						(null != (factType = instance as FactType) && null != (objectType = factType.NestingType)))
					{
						ReferenceMode singleMode = ReferenceMode.GetReferenceModeForDecoratedName(refMode, objectType.Model, false);
						return (object)singleMode ?? refMode;
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
		/// <summary>See <see cref="TypeConverter.ConvertTo(ITypeDescriptorContext,CultureInfo,Object,Type)"/>.</summary>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ReferenceMode referenceMode;
			if (destinationType == typeof(string) && (referenceMode = value as ReferenceMode) != null)
			{
				return referenceMode.DecoratedName;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
