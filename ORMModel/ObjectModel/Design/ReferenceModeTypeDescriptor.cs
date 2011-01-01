#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Drawing.Design;
using System.Windows.Forms.Design;
using ORMSolutions.ORMArchitect.Core.Shell;
using System.Windows.Forms;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ReferenceMode"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReferenceModeTypeDescriptor : ORMModelElementTypeDescriptor<ReferenceMode>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ReferenceModeTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ReferenceModeTypeDescriptor(ICustomTypeDescriptor parent, ReferenceMode selectedElement)
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
			properties.Add(KindDisplayPropertyDescriptor);
			return properties;
		}
		#endregion // Base overrides
		#region Non-DSL Custom Property Descriptors
		private static PropertyDescriptor myKindDisplayPropertyDescriptor;
		/// <summary>
		/// Get a <see cref="PropertyDescriptor"/> for the <see cref="P:ReferenceMode.KindDisplay"/> property
		/// </summary>
		public static PropertyDescriptor KindDisplayPropertyDescriptor
		{
			get
			{
				PropertyDescriptor retVal = myKindDisplayPropertyDescriptor;
				if (retVal == null)
				{
					myKindDisplayPropertyDescriptor = retVal = new AutomatedElementFilterCustomPropertyDescriptor(TypeDescriptor.CreateProperty(typeof(ReferenceMode), "KindDisplay", typeof(ReferenceModeKind)), ResourceStrings.ReferenceModeKindDisplayDisplayName, ResourceStrings.ReferenceModeKindDisplayDescription, null);
				}
				return retVal;
			}
		}
		#endregion // Non-DSL Custom Property Descriptors
	}
}
