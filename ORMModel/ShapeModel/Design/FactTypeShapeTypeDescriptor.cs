#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Design;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel.Design
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
		/// part of an implied objectification or <see cref="ObjectType.AllowIsIndependent()"/>
		/// returns <see langword="false"/>
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == ObjectType.IsIndependentDomainPropertyId)
			{
				ObjectType objectType = ModelElement.NestingType;
				return !objectType.AllowIsIndependent();
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}

		/// <summary>
		/// Block display of the DisplayRelatedTypes, DisplayAsObjectType, and ExpandRefMode
		/// properties for an unobjectified FactType
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid domainPropertyId = domainProperty.Id;
			if (domainPropertyId == FactTypeShape.DisplayRelatedTypesDomainPropertyId ||
				domainPropertyId == FactTypeShape.DisplayAsObjectTypeDomainPropertyId ||
				domainPropertyId == FactTypeShape.ExpandRefModeDomainPropertyId)
			{
				return false;
			}
			else if (domainPropertyId == FactTypeShape.DisplayReverseReadingDomainPropertyId)
			{
				return ResolveFactTypeArity(requestor as FactTypeShape) == 2;
			}
			else if (domainPropertyId == FactTypeShape.DisplayReadingDirectionDomainPropertyId)
			{
				return ResolveFactTypeArity(requestor as FactTypeShape) > 1;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		private int ResolveFactTypeArity(FactTypeShape shape)
		{
			FactType factType;
			IList<RoleBase> roles;
			int arity = 0;
			if (null != shape && null != (factType = shape.AssociatedFactType))
			{
				arity = (roles = factType.RoleCollection).Count;
				if (arity == 2 && FactType.GetUnaryRoleIndex(roles).HasValue)
				{
					arity = 1;
				}
			}
			return arity;
		}

		private static readonly object LockObject = new object();
		private static volatile bool myCustomPropertyAttributesInitialized;
		private static Attribute[] ConstraintDisplayPositionDomainPropertyAttributes;
		private static Attribute[] DisplayOrientationDomainPropertyAttributes;
		private static Attribute[] DisplayRelatedTypesDomainPropertyAttributes;
		private static Attribute[] DisplayRoleNamesDomainPropertyAttributes;
		private static Attribute[] DisplayReverseReadingDomainPropertyAttributes;
		private static Attribute[] DisplayReadingDirectionDomainPropertyAttributes;
		private static Attribute[] NameDomainPropertyAttributes;
		private static Attribute[] IsIndependentDomainPropertyAttributes;
		private static Attribute[] DisplayAsObjectTypeDomainPropertyAttributes;
		private static Attribute[] ExpandRefModeDomainPropertyAttributes;
		private static Attribute[] NestedFactTypeDomainRoleAttributes;
		private static Attribute[] NestingTypeDomainRoleAttributes;

		private void EnsureDomainAttributesInitialized(DomainDataDirectory domainDataDirectory)
		{
			if (!myCustomPropertyAttributesInitialized)
			{
				lock (LockObject)
				{
					if (!myCustomPropertyAttributesInitialized)
					{
						ConstraintDisplayPositionDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.ConstraintDisplayPositionDomainPropertyId));
						DisplayOrientationDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayOrientationDomainPropertyId));
						DisplayRelatedTypesDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRelatedTypesDomainPropertyId));
						DisplayRoleNamesDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRoleNamesDomainPropertyId));
						DisplayReverseReadingDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayReverseReadingDomainPropertyId));
						DisplayReadingDirectionDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayReadingDirectionDomainPropertyId));
						DisplayAsObjectTypeDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayAsObjectTypeDomainPropertyId));
						ExpandRefModeDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(FactTypeShape.ExpandRefModeDomainPropertyId));
						NameDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId));
						IsIndependentDomainPropertyAttributes = GetDomainPropertyAttributes(domainDataDirectory.FindDomainProperty(ObjectType.IsIndependentDomainPropertyId));
						NestedFactTypeDomainRoleAttributes = AddExpandableElementTypeConverterAttribute(GetRolePlayerPropertyAttributes(domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId)));
						NestingTypeDomainRoleAttributes = AddExpandableElementTypeConverterAttribute(GetRolePlayerPropertyAttributes(domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId)));
						myCustomPropertyAttributesInitialized = true;
					}
				}
			}
		}
		private static Attribute[] AddExpandableElementTypeConverterAttribute(Attribute[] attributes)
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
				Objectification objectification = factType.Objectification;
				ObjectType nestingType = objectification.NestingType;
				bool nestingTypeHasRelatedTypes = nestingType.IsSubtypeOrSupertype;
				DomainDataDirectory domainDataDirectory = factType.Store.DomainDataDirectory;
				EnsureDomainAttributesInitialized(domainDataDirectory);

				PropertyDescriptorCollection retVal;
				if (factTypeShape.DisplayAsObjectType)
				{
					retVal = new PropertyDescriptorCollection(null);
					Type componentType = typeof(FactTypeShape);
					foreach (PropertyDescriptor nestingTypeDescriptor in TypeDescriptor.GetProperties(nestingType))
					{
						if (nestingTypeDescriptor.Name == "NestedFactType")
						{
							continue;
						}
						retVal.Add(EditorUtility.RedirectPropertyDescriptor(nestingType, nestingTypeDescriptor, componentType));
					}
					retVal.Add(CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayAsObjectTypeDomainPropertyId), DisplayAsObjectTypeDomainPropertyAttributes));
					retVal.Add(CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.ExpandRefModeDomainPropertyId), ExpandRefModeDomainPropertyAttributes));
					retVal.Add(new ObjectifyingEntityTypePropertyDescriptor(factType, domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId), NestedFactTypeDomainRoleAttributes));
					retVal.Add(new ObjectifiedFactTypePropertyDescriptor(nestingType, domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId), NestingTypeDomainRoleAttributes));
					if (nestingTypeHasRelatedTypes)
					{
						retVal.Add(CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRelatedTypesDomainPropertyId), DisplayRelatedTypesDomainPropertyAttributes));
					}
				}
				else
				{
					int descriptorCount = 8;
					int arity = ResolveFactTypeArity(factTypeShape);
					if (nestingTypeHasRelatedTypes)
					{
						++descriptorCount;
					}
					if (arity > 1)
					{
						++descriptorCount;
						if (arity == 2)
						{
							++descriptorCount;
						}
					}
					PropertyDescriptor[] descriptors = new PropertyDescriptor[descriptorCount];
					descriptors[0] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.ConstraintDisplayPositionDomainPropertyId), ConstraintDisplayPositionDomainPropertyAttributes);
					descriptors[1] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayOrientationDomainPropertyId), DisplayOrientationDomainPropertyAttributes);
					descriptors[2] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRoleNamesDomainPropertyId), DisplayRoleNamesDomainPropertyAttributes);
					descriptors[3] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayAsObjectTypeDomainPropertyId), DisplayAsObjectTypeDomainPropertyAttributes);
					descriptors[4] = CreatePropertyDescriptor(nestingType, domainDataDirectory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId), NameDomainPropertyAttributes);
					descriptors[5] = CreatePropertyDescriptor(nestingType, domainDataDirectory.FindDomainProperty(ObjectType.IsIndependentDomainPropertyId), IsIndependentDomainPropertyAttributes);
					descriptors[6] = new ObjectifyingEntityTypePropertyDescriptor(factType, domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId), NestedFactTypeDomainRoleAttributes);
					descriptors[7] = new ObjectifiedFactTypePropertyDescriptor(nestingType, domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId), NestingTypeDomainRoleAttributes);
					if (nestingTypeHasRelatedTypes)
					{
						descriptors[8] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayRelatedTypesDomainPropertyId), DisplayRelatedTypesDomainPropertyAttributes);
					}
					if (arity > 1)
					{
						descriptors[descriptorCount - (arity == 2 ? 2 : 1)] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayReadingDirectionDomainPropertyId), DisplayReadingDirectionDomainPropertyAttributes);
						if (arity == 2)
						{
							descriptors[descriptorCount - 1] = CreatePropertyDescriptor(factTypeShape, domainDataDirectory.FindDomainProperty(FactTypeShape.DisplayReverseReadingDomainPropertyId), DisplayReverseReadingDomainPropertyAttributes);
						}
					}
					retVal = new PropertyDescriptorCollection(descriptors);
				}

				// This mockup of important properties means that extension providers cannot add properties
				// here by adding to the objecttype or facttype. Use an extension on the Objectification type
				// itself to add extension properties.
				((IFrameworkServices)factType.Store).PropertyProviderService.GetProvidedProperties(objectification, retVal); 
				return retVal;
			}
			return base.GetProperties(attributes);
		}
	}
}
