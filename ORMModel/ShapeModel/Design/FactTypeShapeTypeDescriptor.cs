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
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Diagrams.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.ShapeModel.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="FactTypeShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FactTypeShapeTypeDescriptor<TPresentationElement, TModelElement> : ORMBaseShapeTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : FactTypeShape
		where TModelElement : FactType
	{
		/// <summary>
		/// Initializes a new instance of <see cref="FactTypeShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public FactTypeShapeTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
			: base(parent, presentationElement, selectedElement)
		{
		}

		private static readonly object LockObject = new object();
		private static volatile bool Initialized;
		private static DomainPropertyInfo ConstraintDisplayPositionDomainPropertyInfo;
		private static Attribute[] ConstraintDisplayPositionDomainPropertyAttributes;
		private static DomainPropertyInfo DisplayRoleNamesDomainPropertyInfo;
		private static Attribute[] DisplayRoleNamesDomainPropertyAttributes;
		private static DomainPropertyInfo NameDomainPropertyInfo;
		private static Attribute[] NameDomainPropertyAttributes;
		private static DomainPropertyInfo IsIndependentDomainPropertyInfo;
		private static Attribute[] IsIndependentDomainPropertyAttributes;
		private static DomainPropertyInfo NestedFactTypeDisplayDomainPropertyInfo;
		private static Attribute[] NestedFactTypeDisplayDomainPropertyAttributes;
		private static DomainPropertyInfo NestingTypeDisplayDomainPropertyInfo;
		private static Attribute[] NestingTypeDisplayDomainPropertyAttributes;

		private void EnsureDomainPropertiesInitialized(DomainDataDirectory domainDataDirectory)
		{
			if (!Initialized)
			{
				lock (LockObject)
				{
					if (!Initialized)
					{
						ConstraintDisplayPositionDomainPropertyAttributes = GetDomainPropertyAttributes(ConstraintDisplayPositionDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactTypeShape.ConstraintDisplayPositionDomainPropertyId));
						DisplayRoleNamesDomainPropertyAttributes = GetDomainPropertyAttributes(DisplayRoleNamesDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRoleNamesDomainPropertyId));
						NameDomainPropertyAttributes = GetDomainPropertyAttributes(NameDomainPropertyInfo = domainDataDirectory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId));
						IsIndependentDomainPropertyAttributes = GetDomainPropertyAttributes(IsIndependentDomainPropertyInfo = domainDataDirectory.FindDomainProperty(ObjectType.IsIndependentDomainPropertyId));
						NestedFactTypeDisplayDomainPropertyAttributes = ProcessAttributes(GetDomainPropertyAttributes(NestedFactTypeDisplayDomainPropertyInfo = domainDataDirectory.FindDomainProperty(ObjectType.NestedFactTypeDisplayDomainPropertyId)));
						NestingTypeDisplayDomainPropertyAttributes = ProcessAttributes(GetDomainPropertyAttributes(NestingTypeDisplayDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactType.NestingTypeDisplayDomainPropertyId)));
						Initialized = true;
					}
				}
			}
		}
		private static Attribute[] ProcessAttributes(Attribute[] attributes)
		{
			// Remove the EditorAtttribute if it is present
			int editorAttributeIndex = -1;
			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i] is EditorAttribute)
				{
					editorAttributeIndex = i;
					break;
				}
			}
			Attribute[] newAttributes = new Attribute[attributes.Length + (editorAttributeIndex < 0 ? 1 : 0)];
			int destIndex = 0;
			for (int sourceIndex = 0; sourceIndex < attributes.Length; sourceIndex++)
			{
				if (sourceIndex != editorAttributeIndex)
				{
					newAttributes[destIndex++] = attributes[sourceIndex];
				}
			}
			// Add the TypeConverterAttribute
			newAttributes[destIndex] = new TypeConverterAttribute(typeof(ExpandableElementConverter));
			return newAttributes;
		}

		/// <summary>
		/// Show selected properties from the <see cref="FactType.NestingType"/> and the
		/// <see cref="Objectification.NestedFactType"/> for an objectified <see cref="FactType"/>,
		/// as well as expandable nodes for each of the underlying instances.
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			FactType factType = ModelElement;
			if (FactTypeShape.ShouldDrawObjectification(factType))
			{
				FactTypeShape factTypeShape = PresentationElement;
				ObjectType nestingType = factType.NestingType;
				EnsureDomainPropertiesInitialized(factType.Store.DomainDataDirectory);

				return new PropertyDescriptorCollection(new PropertyDescriptor[]{
					CreatePropertyDescriptor(factTypeShape, ConstraintDisplayPositionDomainPropertyInfo, ConstraintDisplayPositionDomainPropertyAttributes),
					CreatePropertyDescriptor(factTypeShape, DisplayRoleNamesDomainPropertyInfo, DisplayRoleNamesDomainPropertyAttributes),
					CreatePropertyDescriptor(nestingType, NameDomainPropertyInfo, NameDomainPropertyAttributes),
					CreatePropertyDescriptor(nestingType, IsIndependentDomainPropertyInfo, IsIndependentDomainPropertyAttributes),
					CreatePropertyDescriptor(factType, NestingTypeDisplayDomainPropertyInfo, NestingTypeDisplayDomainPropertyAttributes),
					CreatePropertyDescriptor(nestingType, NestedFactTypeDisplayDomainPropertyInfo, NestedFactTypeDisplayDomainPropertyAttributes)
				});
			}
			return base.GetProperties(attributes);
		}
	}
}
