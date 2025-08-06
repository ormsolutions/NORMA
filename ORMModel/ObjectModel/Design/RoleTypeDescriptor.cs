#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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
using Microsoft.VisualStudio.VirtualTreeGrid;


namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Role"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RoleTypeDescriptor : ORMModelElementTypeDescriptor<Role>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="RoleTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RoleTypeDescriptor(ICustomTypeDescriptor parent, Role selectedElement)
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
			Role role = ModelElement;
			FactType factType;
			properties.Add(GetRolePlayerDisplayPropertyDescriptor(role));
			if (null != (factType = role.FactType) &&
				factType.UnaryRole == role)
			{
				properties.Add(CardinalityDisplayPropertyDescriptor);
			}
			return properties;
		}
		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid propertyId = domainProperty.Id;
			if (propertyId == Role.MultiplicityDomainPropertyId)
			{
				FactType factType = ModelElement.FactType;
				Objectification objectification;
				LinkedElementCollection<RoleBase> roles;
				// Display for binary fact types
				if (factType != null &&
					!(factType is QueryBase) &&
					(roles = factType.RoleCollection).Count == 2)
				{
					if (null != (objectification = factType.ImpliedByObjectification))
					{
						roles = objectification.NestedFactType.RoleCollection;
						if (roles.Count != 2)
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			else if (propertyId == Role.IsMandatoryDomainPropertyId)
			{
				if (ModelElement.FactType is QueryBase)
				{
					return false;
				}
			}
			else if (propertyId == Role.ValueRangeTextDomainPropertyId)
			{
				Role role = ModelElement;
				FactType factType = role.FactType;
				if (factType is QueryBase)
				{
					return false;
				}
				else if (factType is SubtypeFact)
				{
					// Allow a legacy value constraint. If we want to support value constraint and default values on
					// a subtype it needs to be model on the subtype object type to properly handle multiple primary
					// subtype paths.
					return role.ValueConstraint != null;
				}
			}
			else if (propertyId == Role.MandatoryConstraintNameDomainPropertyId ||
				propertyId == Role.MandatoryConstraintModalityDomainPropertyId)
			{
				return ModelElement.SimpleMandatoryConstraint != null;
			}
			else if (propertyId == Role.ObjectificationOppositeRoleNameDomainPropertyId)
			{
				FactType factType = ModelElement.FactType;
				return factType != null && factType.Objectification != null;
			}
			else if (propertyId == Role.DefaultValueDomainPropertyId)
			{
				Role role = ModelElement;
				FactType factType = role.FactType;
				if (factType != null)
				{
					return !(factType is SubtypeFact) && (role.FactType.UnaryPattern != UnaryValuePattern.NotUnary || role.IsValueRole || role.ResolvedDirectDefaultValue != null);
				}
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Manage read-only state of properties.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (propertyId == Role.ValueRangeTextDomainPropertyId)
			{
				return ModelElement.ValueConstraint == null && !ModelElement.IsValueRole;
			}
			else if (propertyId == Role.IsMandatoryDomainPropertyId ||
				propertyId == Role.MandatoryConstraintModalityDomainPropertyId)
			{
				Role role = ModelElement;
				FactType factType;
				if (role is SubtypeMetaRole ||
					role is SupertypeMetaRole ||
					(null != (factType = role.FactType) &&
					(null != factType.ImpliedByObjectification || null != factType.UnaryRole)))
				{
					return true;
				}
			}
			else if (propertyId == Role.MultiplicityDomainPropertyId)
			{
				Role role = ModelElement;
				FactType factType;
				if (role is SubtypeMetaRole ||
					role is SupertypeMetaRole ||
					(null != (factType = role.FactType) &&
					null != factType.UnaryRole))
				{
					return true;
				}
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}

		/// <summary>
		/// Create custom property descriptors as needed.
		/// </summary>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainPropertyInfo, Attribute[] attributes)
		{
			Guid propertyId = domainPropertyInfo.Id;
			if (propertyId == Role.DefaultValueDomainPropertyId)
			{
				// Use the DefaultValuePropertyDescriptor to manage both the DefaultValue and DefaultState settings unless this is a unary role,
				// where it is displayed and read only with a resource string.
				return new DefaultValuePropertyDescriptor(this, requestor, domainPropertyInfo, attributes, false);
			}
			return base.CreatePropertyDescriptor(requestor, domainPropertyInfo, attributes);
		}
		#endregion // Base overrides
		#region Non-DSL Custom Property Descriptors
		private static PropertyDescriptor myRolePlayerDisplayPropertyDescriptor;
		private static PropertyDescriptor myRolePlayerDisplayReadOnlyPropertyDescriptor;
		/// <summary>
		/// Get a <see cref="PropertyDescriptor"/> for the <see cref="P:ORMModel.ModelErrorDisplayFilterDisplay"/> property
		/// </summary>
		public static PropertyDescriptor GetRolePlayerDisplayPropertyDescriptor(Role role)
		{
			bool isReadOnly = false;
			FactType factType;
			if (role is SubtypeMetaRole ||
				role is SupertypeMetaRole ||
				(null != (factType = role.FactType) &&
				null != factType.ImpliedByObjectification))
			{
				isReadOnly = true;
			}
			PropertyDescriptor retVal = isReadOnly ? myRolePlayerDisplayReadOnlyPropertyDescriptor : myRolePlayerDisplayPropertyDescriptor;
			if (retVal == null)
			{
				PropertyDescriptor reflectedDescriptor = TypeDescriptor.CreateProperty(typeof(Role), "RolePlayerDisplay", typeof(ObjectType));
				string displayName = ResourceStrings.RoleRolePlayerDisplayDisplayName;
				string description = ResourceStrings.RoleRolePlayerDisplayDescription;
				myRolePlayerDisplayPropertyDescriptor = new StoreEnabledPropertyDescriptor(reflectedDescriptor, null, displayName, description, null);
				myRolePlayerDisplayReadOnlyPropertyDescriptor = new StoreEnabledReadOnlyPropertyDescriptor(reflectedDescriptor, null, displayName, description, null);
				retVal = isReadOnly ? myRolePlayerDisplayReadOnlyPropertyDescriptor : myRolePlayerDisplayPropertyDescriptor;
			}
			return retVal;
		}
		private static PropertyDescriptor myCardinalityDisplayPropertyDescriptor;
		/// <summary>
		/// Get a <see cref="PropertyDescriptor"/> for the <see cref="ObjectType.CardinalityDisplay"/> property
		/// </summary>
		public static PropertyDescriptor CardinalityDisplayPropertyDescriptor
		{
			get
			{
				PropertyDescriptor retVal = myCardinalityDisplayPropertyDescriptor;
				if (retVal == null)
				{
					myCardinalityDisplayPropertyDescriptor = retVal = EditorUtility.ReflectStoreEnabledPropertyDescriptor(typeof(Role), "CardinalityDisplay", typeof(string), null, ResourceStrings.RoleCardinalityDisplayDisplayName, ResourceStrings.RoleCardinalityDisplayDescription, null);
				}
				return retVal;
			}
		}
		#endregion // Non-DSL Custom Property Descriptors
	}
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="RoleProxy"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RoleProxyTypeDescriptor : ORMModelElementTypeDescriptor<RoleProxy>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="RoleProxyTypeDescriptor"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RoleProxyTypeDescriptor(ICustomTypeDescriptor parent, RoleProxy selectedElement)
			: base(parent, selectedElement)
		{
		}
		#endregion // Constructor
		#region Base overrides
		private static readonly string[] RemoveRoleProperties = new string[] { "Multiplicity", "ObjectificationOppositeRoleName" };
		/// <summary>
		/// Show the same properties as 'Role', except we eliminate the
		/// confusing 'Multiplicity' property and the 'ImpliedRoleName' property,
		/// which is now visible on the opposite role.
		/// </summary>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties;
			RoleProxy proxy;
			Role role;
			if (null != (proxy = ModelElement) &&
				null != (role = proxy.TargetRole))
			{
				properties = TypeDescriptor.GetProperties(role);
				bool haveEditable = false;
				for (int i = 0; i < RemoveRoleProperties.Length; ++i)
				{
					PropertyDescriptor removeDescriptor = properties.Find(RemoveRoleProperties[i], false);
					if (removeDescriptor != null)
					{
						if (!haveEditable)
						{
							haveEditable = true;
							properties = EditorUtility.GetEditablePropertyDescriptors(properties);
						}
						properties.Remove(removeDescriptor);
					}
				}
				return properties;
			}
			return base.GetProperties(attributes);
		}
		/// <summary>
		/// Display the class name the same as a role. This resource redirects to the
		/// display name for the role.
		/// </summary>
		public override string GetClassName()
		{
			return ResourceStrings.RoleProxyTypeName;
		}
		#endregion // Base overrides
	}
}
