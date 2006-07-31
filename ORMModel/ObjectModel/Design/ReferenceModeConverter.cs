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
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel.Design
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
					ObjectType instance = EditorUtility.ResolveContextInstance(context.Instance, true) as ObjectType;
					if (instance != null)
					{
						IList<ReferenceMode> referenceModes = ReferenceMode.FindReferenceModesByName(refMode, instance.Model);
						switch (referenceModes.Count)
						{
							case 0:
								return refMode;
							case 1:
								return referenceModes[0];
							default:
								throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeAmbiguousName);
						}
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
				return referenceMode.Name;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
