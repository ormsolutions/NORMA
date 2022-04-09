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
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="ORMDiagram"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMDiagramTypeDescriptor<TPresentationElement, TModelElement> : DiagramTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : ORMDiagram
		where TModelElement : ORMModel
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ObjectTypeShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ORMDiagramTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
			: base(parent, presentationElement, selectedElement)
		{
		}
		/// <summary>
		/// Rename the model's Name property to ModelName, and add a DiagramName
		/// property for the diagram.
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = EditorUtility.GetEditablePropertyDescriptors(base.GetProperties(attributes));
			EditorUtility.ModifyPropertyDescriptorDisplay(properties, "Name", "ModelName", ResourceStrings.DiagramPropertiesModelNameDisplayName, null, null);
			ORMDiagram diagram;
			Store store;
			if (null != (diagram = this.PresentationElement) &&
				null != (store = Utility.ValidateStore(diagram.Store)))
			{
				properties.Add(EditorUtility.ModifyPropertyDescriptorDisplay(this.CreatePropertyDescriptor(diagram, store.DomainDataDirectory.GetDomainProperty(ORMDiagram.NameDomainPropertyId), attributes), "DiagramName", ResourceStrings.DiagramPropertiesDiagramNameDisplayName, ResourceStrings.DiagramPropertiesDiagramNameDescription, null));
				properties = ExtendableElementUtility.GetExtensionProperties(diagram, properties, typeof(TPresentationElement));
			}

			// Break out global and local display options into expandable properties
			List<PropertyDescriptor> localDisplayOptions = null;
			List<PropertyDescriptor> globalDisplayOptions = null;
			for (int i = properties.Count - 1; i >= 0; --i)
			{
				PropertyDescriptor descriptor = properties[i];
				foreach (Attribute attr in descriptor.Attributes)
				{
					ORMDiagramDisplayOptionAttribute displayOption;
					if (null != (displayOption = attr as ORMDiagramDisplayOptionAttribute))
					{
						(displayOption.IsGlobal ? (globalDisplayOptions ?? (globalDisplayOptions = new List<PropertyDescriptor>())) : (localDisplayOptions ?? (localDisplayOptions = new List<PropertyDescriptor>()))).Add(descriptor);
						properties.RemoveAt(i);
						break;
					}
				}
			}

			if (localDisplayOptions != null)
			{
				properties.Add(EditorUtility.ConsolidatePropertyDescriptors<ORMDiagram>(localDisplayOptions, "LocalDisplayOptions", ResourceStrings.ORMDiagramDisplayOptionLocalOptionsName, ResourceStrings.ORMDiagramDisplayOptionLocalOptionsDescription, ResourceStrings.ORMDiagramDisplayOptionCategory, ResourceStrings.ORMDiagramDisplayOptionDefaultState, ResourceStrings.ORMDiagramDisplayOptionCustomState));
			}

			if (globalDisplayOptions != null)
			{
				properties.Add(EditorUtility.ConsolidatePropertyDescriptors<ORMDiagram>(globalDisplayOptions, "GlobalDisplayOptions", ResourceStrings.ORMDiagramDisplayOptionGlobalOptionsName, ResourceStrings.ORMDiagramDisplayOptionGlobalOptionsDescription, ResourceStrings.ORMDiagramDisplayOptionCategory, ResourceStrings.ORMDiagramDisplayOptionDefaultState, ResourceStrings.ORMDiagramDisplayOptionCustomState));
			}
			return properties;
		}
	}
}
