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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;

namespace ORMSolutions.ORMArchitect.Views.RelationalView.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="FactType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal class RelationalDiagramTypeDescriptor : ElementTypeDescriptor<RelationalDiagram>
	{
		/// <summary>
		/// Initializes a new instance of <see cref="RelationalDiagramTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RelationalDiagramTypeDescriptor(ICustomTypeDescriptor parent, RelationalDiagram selectedElement)
			: base(parent, selectedElement)
		{
		}
		/// <summary>
		/// Add schema properties
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			// The relational diagram is currently bound to the catalog, which
			// currently always has one schema. Redirect to the schema properties.
			PropertyDescriptorCollection retVal = EditorUtility.GetEditablePropertyDescriptors(base.GetProperties(attributes));
			RelationalDiagram diagram;
			Catalog catalog;
			LinkedElementCollection<Schema> schemas;
			if (null != (diagram = this.ModelElement) &&
				null != (catalog = diagram.Subject as Catalog) &&
				(schemas = catalog.SchemaCollection).Count == 1)
			{
				foreach (PropertyDescriptor schemaDescriptor in TypeDescriptor.GetProperties(schemas[0], attributes))
				{
					retVal.Add(schemaDescriptor);
				}
			}
			return retVal;
		}
	}
}
