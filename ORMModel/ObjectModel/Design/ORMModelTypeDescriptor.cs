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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ORMModel"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMModelTypeDescriptor : ORMModelElementTypeDescriptor<ORMModel>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ORMModelTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ORMModelTypeDescriptor(ICustomTypeDescriptor parent, ORMModel selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Add custom display properties
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = EditorUtility.GetEditablePropertyDescriptors(base.GetProperties(attributes));
			properties.Add(ModelErrorDisplayFilterDisplayPropertyDescriptor);
			return properties;
		}
		/// <summary>
		/// Customize the description of the Name property
		/// </summary>
		protected override string GetDescription(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == ORMModel.NameDomainPropertyId)
			{
				return ResourceStrings.ModelNameDescription;
			}
			return base.GetDescription(propertyDescriptor);
		}
		#endregion // Base overrides
		#region Non-DSL Custom Property Descriptors
		private static PropertyDescriptor myModelErrorDisplayFilterDisplayPropertyDescriptor;
		/// <summary>
		/// Get a <see cref="PropertyDescriptor"/> for the <see cref="P:ORMModel.ModelErrorDisplayFilterDisplay"/> property
		/// </summary>
		public static PropertyDescriptor ModelErrorDisplayFilterDisplayPropertyDescriptor
		{
			get
			{
				PropertyDescriptor retVal = myModelErrorDisplayFilterDisplayPropertyDescriptor;
				if (retVal == null)
				{
					myModelErrorDisplayFilterDisplayPropertyDescriptor = retVal = EditorUtility.ReflectStoreEnabledPropertyDescriptor(typeof(ORMModel), "ModelErrorDisplayFilterDisplay", typeof(ModelErrorDisplayFilter), ResourceStrings.ModelModelErrorDisplayFilterDisplayDisplayName, ResourceStrings.ModelModelErrorDisplayFilterDisplayDescription, null);
				}
				return retVal;
			}
		}
		#endregion // Non-DSL Custom Property Descriptors
	}
}
