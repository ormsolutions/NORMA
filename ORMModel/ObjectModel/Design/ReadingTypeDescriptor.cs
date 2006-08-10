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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Reading"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReadingTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : Reading
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ReadingTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ReadingTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
		}

		/// <summary>
		/// Ensure that the <see cref="Reading.IsPrimaryForReadingOrder"/> and <see cref="Reading.IsPrimaryForFactType"/>
		/// properties are read-only when they are <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (propertyId.Equals(Reading.IsPrimaryForReadingOrderDomainPropertyId))
			{
				return ModelElement.IsPrimaryForReadingOrder;
			}
			else if (propertyId.Equals(Reading.IsPrimaryForFactTypeDomainPropertyId))
			{
				return ModelElement.IsPrimaryForFactType;
			}
			else
			{
				return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
			}
		}

		/// <summary>See <see cref="ElementTypeDescriptor.GetComponentName"/>.</summary>
		public override string GetComponentName()
		{
			Reading reading = ModelElement;
			ReadingOrder readingOrder = reading.ReadingOrder;
			if (readingOrder != null)
			{
				// UNDONE: Localize the format string
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}{2}", readingOrder.FactType.Name, ResourceStrings.ReadingType, readingOrder.ReadingCollection.IndexOf(reading) + 1);
			}
			return base.GetComponentName();
		}
	}
}
