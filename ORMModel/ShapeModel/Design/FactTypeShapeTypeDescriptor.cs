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
using Neumont.Tools.ORM.ObjectModel.Design;
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

		/// <summary>
		/// Ensure that the <see cref="ObjectType.IsIndependent"/> property displayed
		/// as a top-level property is read-only when the <see cref="ObjectType"/> is
		/// part of an implied objectification or <see cref="ObjectType.AllowIsIndependent"/>
		/// returns <see langword="false"/>
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == ObjectType.IsIndependentDomainPropertyId)
			{
				ObjectType objectType = ModelElement.NestingType;
				return !(objectType.IsIndependent || objectType.AllowIsIndependent(false));
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}

		private static readonly object LockObject = new object();
		private static volatile bool Initialized;
		private static DomainPropertyInfo ConstraintDisplayPositionDomainPropertyInfo;
		private static Attribute[] ConstraintDisplayPositionDomainPropertyAttributes;
		private static DomainPropertyInfo DisplayOrientationDomainPropertyInfo;
		private static Attribute[] DisplayOrientationDomainPropertyAttributes;
		private static DomainPropertyInfo DisplayRoleNamesDomainPropertyInfo;
		private static Attribute[] DisplayRoleNamesDomainPropertyAttributes;
		private static DomainPropertyInfo NameDomainPropertyInfo;
		private static Attribute[] NameDomainPropertyAttributes;
		private static DomainPropertyInfo IsIndependentDomainPropertyInfo;
		private static Attribute[] IsIndependentDomainPropertyAttributes;
		private static DomainRoleInfo NestedFactTypeDomainRoleInfo;
		private static Attribute[] NestedFactTypeDomainRoleAttributes;
		private static DomainRoleInfo NestingTypeDomainRoleInfo;
		private static Attribute[] NestingTypeDomainRoleAttributes;

		private void EnsureDomainPropertiesInitialized(DomainDataDirectory domainDataDirectory)
		{
			if (!Initialized)
			{
				lock (LockObject)
				{
					if (!Initialized)
					{
						ConstraintDisplayPositionDomainPropertyAttributes = GetDomainPropertyAttributes(ConstraintDisplayPositionDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactTypeShape.ConstraintDisplayPositionDomainPropertyId));
						DisplayOrientationDomainPropertyAttributes = GetDomainPropertyAttributes(DisplayOrientationDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayOrientationDomainPropertyId));
						DisplayRoleNamesDomainPropertyAttributes = GetDomainPropertyAttributes(DisplayRoleNamesDomainPropertyInfo = domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRoleNamesDomainPropertyId));
						NameDomainPropertyAttributes = GetDomainPropertyAttributes(NameDomainPropertyInfo = domainDataDirectory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId));
						IsIndependentDomainPropertyAttributes = GetDomainPropertyAttributes(IsIndependentDomainPropertyInfo = domainDataDirectory.FindDomainProperty(ObjectType.IsIndependentDomainPropertyId));
						NestedFactTypeDomainRoleAttributes = ProcessAttributes(GetRolePlayerPropertyAttributes(NestedFactTypeDomainRoleInfo = domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId)));
						NestingTypeDomainRoleAttributes = ProcessAttributes(GetRolePlayerPropertyAttributes(NestingTypeDomainRoleInfo = domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId)));
						Initialized = true;
					}
				}
			}
		}
		private static Attribute[] ProcessAttributes(Attribute[] attributes)
		{
			List<Attribute> newAttributes = new List<Attribute>(attributes.Length + 1);
			foreach (Attribute attribute in attributes)
			{
				// Don't add the EditorAttribute and TypeConverterAttribute
				if (!(attribute is EditorAttribute) && !(attribute is TypeConverterAttribute))
				{
					newAttributes.Add(attribute);
				}
			}
			newAttributes.Add(new TypeConverterAttribute(typeof(ExpandableElementConverter)));
			return newAttributes.ToArray();
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
					CreatePropertyDescriptor(factTypeShape, DisplayOrientationDomainPropertyInfo, DisplayOrientationDomainPropertyAttributes),
					CreatePropertyDescriptor(factTypeShape, DisplayRoleNamesDomainPropertyInfo, DisplayRoleNamesDomainPropertyAttributes),
					CreatePropertyDescriptor(nestingType, NameDomainPropertyInfo, NameDomainPropertyAttributes),
					CreatePropertyDescriptor(nestingType, IsIndependentDomainPropertyInfo, IsIndependentDomainPropertyAttributes),
					new ObjectificationRolePlayerPropertyDescriptor(factType, NestingTypeDomainRoleInfo, NestedFactTypeDomainRoleAttributes),
					new ObjectificationRolePlayerPropertyDescriptor(nestingType, NestedFactTypeDomainRoleInfo, NestingTypeDomainRoleAttributes)
				});
			}
			return base.GetProperties(attributes);
		}
	}
}
