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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.Modeling.Design
{
	#region ElementTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="T:Microsoft.VisualStudio.Modeling.ModelElement"/>s
	/// of type <typeparamref name="TModelElement"/>.
	/// </summary>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="T:Microsoft.VisualStudio.Modeling.ModelElement"/> that this
	/// <see cref="ElementTypeDescriptor{TModelElement}"/> is for.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ElementTypeDescriptor<TModelElement> : ElementTypeDescriptor
		where TModelElement : ModelElement
	{
		// NOTE: Keep in sync with PresentationElementTypeDescriptor<TPresentationElement,TModelElement> and DiagramTypeDescriptor<TPresentationElement,TModelElement>

		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="ElementTypeDescriptor{TModelElement}"/> for
		/// the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ElementTypeDescriptor(ICustomTypeDescriptor parent, TModelElement selectedElement)
			: base(parent, selectedElement)
		{
			// The ElementTypeDescriptor constructor already checked selectedElement for null.   
			this.myModelElement = selectedElement;
		}
		#endregion // Constructor

		#region ModelElement property
		private readonly TModelElement myModelElement;
		/// <summary>
		/// The <see cref="T:Microsoft.VisualStudio.Modeling.ModelElement"/> of type <typeparamref name="TModelElement"/>
		/// that this <see cref="ElementTypeDescriptor{TModelElement}"/> is for.
		/// </summary>
		public new TModelElement ModelElement
		{
			get
			{
				return this.myModelElement;
			}
		}
		#endregion // ModelElement property

		#region GetClassName method
		/// <summary>
		/// Returns the <see cref="DomainObjectInfo.DisplayName"/> for the
		/// <see cref="DomainClassInfo"/> of <see cref="ElementTypeDescriptor{TModelElement}.ModelElement"/>.
		/// </summary>
		public override string GetClassName()
		{
			return this.ModelElement.GetDomainClass().DisplayName;
		}
		#endregion // GetClassName method

		// ElementTypeDescriptor already has a good (name-based) override
		// for GetComponentName(), so we don't need to provide one here.

		#region GetDisplayProperties method
		/// <summary>
		/// Blocks editor access to <see cref="ElementTypeDescriptor.GetDisplayProperties"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is not supported.", true)]
		protected new PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			throw new NotSupportedException();
		}
		#endregion // GetDisplayProperties method

		#region GetDomainPropertyAttributes method
		/// <summary>
		/// Replaces <see cref="ElementTypeDescriptor.GetDomainPropertyAttributes"/>.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.GetDomainPropertyAttributes"/>
		protected new Attribute[] GetDomainPropertyAttributes(DomainPropertyInfo domainPropertyInfo)
		{
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			return DomainTypeDescriptor.AttributeCollectionToArray(DomainTypeDescriptor.GetRawAttributes(domainPropertyInfo.PropertyInfo));
		}
		#endregion // GetDomainPropertyAttributes method

		#region GetRolePlayerPropertyAttributes method
		/// <summary>
		/// Replaces <see cref="ElementTypeDescriptor.GetRolePlayerPropertyAttributes"/>.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.GetRolePlayerPropertyAttributes"/>
		protected new Attribute[] GetRolePlayerPropertyAttributes(DomainRoleInfo domainRole)
		{
			if (domainRole == null)
			{
				throw new ArgumentNullException("domainRole");
			}
			return DomainTypeDescriptor.AttributeCollectionToArray(DomainTypeDescriptor.GetRawAttributes(domainRole.LinkPropertyInfo));
		}
		#endregion // GetRolePlayerPropertyAttributes method

		#region ShouldCreatePropertyDescriptor method
		/// <summary>
		/// Replaces <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			if (domainProperty == null)
			{
				throw new ArgumentNullException("domainProperty");
			}
			return ((BrowsableAttribute)DomainTypeDescriptor.GetRawAttributes(domainProperty.PropertyInfo)[typeof(BrowsableAttribute)]).Browsable;
		}
		#endregion // ShouldCreatePropertyDescriptor method

		#region GetEvents method
		/// <summary>
		/// Calls <see cref="ICustomTypeDescriptor.GetEvents(Attribute[])"/>,
		/// passing <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override EventDescriptorCollection GetEvents()
		{
			return this.GetEvents(null);
		}
		#endregion // GetEvents method

		#region GetProperties methods
		/// <summary>
		/// Calls <see cref="GetProperties(Attribute[])"/>,
		/// passing <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return this.GetProperties(null);
		}
		/// <summary>
		/// Replaces <see cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptors = new PropertyDescriptorCollection(null);
			TModelElement requestor = this.ModelElement;
			if (!requestor.IsDeleted && !requestor.IsDeleting)
			{
				// Get the property descriptors for our DomainProperties.
				foreach (DomainPropertyInfo domainPropertyInfo in requestor.GetDomainClass().AllDomainProperties)
				{
					// Give derived types an opportunity to skip the current property.
					if (!this.ShouldCreatePropertyDescriptor(requestor, domainPropertyInfo))
					{
						continue;
					}
					ElementPropertyDescriptor propertyDescriptor =
						this.CreatePropertyDescriptor(requestor, domainPropertyInfo, this.GetDomainPropertyAttributes(domainPropertyInfo));
					if (propertyDescriptor != null)
					{
						propertyDescriptors.Add(propertyDescriptor);
					}
				}

				bool includeOppositeRolePlayerProperties = this.IncludeOppositeRolePlayerProperties(requestor);
				bool includeEmbeddingRelationshipProperties = this.IncludeEmbeddingRelationshipProperties(requestor);
				
				// Get the property descriptors for the DomainRoles we play.
				if (includeOppositeRolePlayerProperties || includeEmbeddingRelationshipProperties)
				{
					HashSet<string, string> oppositePropertyNames =
						new HashSet<string,string>(KeyProvider<string, string>.Default, StringComparer.Ordinal, StringComparer.Ordinal);

					foreach (DomainRoleInfo playedRole in requestor.GetDomainClass().AllDomainRolesPlayed)
					{
						// We only want to get property descriptors for DomainRoles when any derived types
						// have opted in, and the current property has at most one value.
						if (includeOppositeRolePlayerProperties && playedRole.IsOne)
						{
							string playedRolePropertyName = playedRole.PropertyName;
							DomainRoleInfo oppositeRole = playedRole.OppositeDomainRole;

							// Make sure that the opposite role player has a name (so that we have something to display),
							// that we don't already have an opposite role player property with this name, and that any
							// derived types approve of creating a property descriptor for this DomainRole.
							if (oppositeRole.RolePlayer.NameDomainProperty != null &&
								!oppositePropertyNames.Contains(playedRolePropertyName) &&
								this.ShouldCreateRolePlayerPropertyDescriptor(requestor, playedRole))
							{
								RolePlayerPropertyDescriptor propertyDescriptor =
									this.CreateRolePlayerPropertyDescriptor(requestor, oppositeRole, this.GetRolePlayerPropertyAttributes(playedRole));
								if (propertyDescriptor != null)
								{
									propertyDescriptors.Add(propertyDescriptor);
									oppositePropertyNames.Add(playedRolePropertyName);
								}
							}
						}

						// If any derived types have opted in, and this relationship embeds us within another ModelElement,
						// we'll include all of the properties from the instances of this relationship.
						if (includeEmbeddingRelationshipProperties &&
							playedRole.DomainRelationship.IsEmbedding &&
							!playedRole.DomainRelationship.AllowsDuplicates &&
							!playedRole.IsEmbedding)
						{
							// We exclude any ElementLinks attached to derived role players, since they'll be picked up separately.
							foreach (ElementLink elementLink in playedRole.GetElementLinks<ElementLink>(requestor, true))
							{
								foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(elementLink))
								{
									propertyDescriptors.Add(propertyDescriptor);
								}
							}
						}
					}
				}

				// Add any extension properties
				((IFrameworkServices)requestor.Store).PropertyProviderService.GetProvidedProperties(requestor, propertyDescriptors);
			}
			return propertyDescriptors;
		}
		#endregion // GetProperties methods
	}
	#endregion // ElementTypeDescriptor class
}
