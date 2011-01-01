#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMSolutions.ORMArchitect.Framework.Diagrams.Design
{
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.PresentationElement"/>s
	/// of type <typeparamref name="TPresentationElement"/>.
	/// </summary>
	/// <typeparam name="TPresentationElement">
	/// The type of the <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.PresentationElement"/> that this
	/// <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
	/// </typeparam>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="T:Microsoft.VisualStudio.Modeling.ModelElement"/> that
	/// <typeparamref name="TPresentationElement"/> is associated with.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class PresentationElementTypeDescriptor<TPresentationElement, TModelElement> : PresentationElementTypeDescriptor
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
	{
		// NOTE: Keep in sync with ElementTypeDescriptor<TModelElement> and DiagramTypeDescriptor<TPresentationElement,TModelElement>
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> for
		/// the instance of <typeparamref name="TPresentationElement"/> specified by <paramref name="presentationElement"/>
		/// that is associated with the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public PresentationElementTypeDescriptor(ICustomTypeDescriptor parent, TPresentationElement presentationElement, TModelElement selectedElement)
#if VISUALSTUDIO_10_0
			: base(parent, presentationElement)
#else
			: base(parent, presentationElement, selectedElement)
#endif
		{
			// The PresentationElementTypeDescriptor constructor already checked presentationElement for null.
			myPresentationElement = presentationElement;
			// The ElementTypeDescriptor constructor already checked selectedElement for null.
#if VISUALSTUDIO_10_0
			myModelElement = (TModelElement)presentationElement.ModelElement;
#else
			myModelElement = selectedElement;
#endif
		}
		#endregion // Constructor
		#region PresentationElement property
		private readonly TPresentationElement myPresentationElement;
		/// <summary>
		/// The <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.PresentationElement"/> of type <typeparamref name="TPresentationElement"/>
		/// that this <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		public new TPresentationElement PresentationElement
		{
			get
			{
				return this.myPresentationElement;
			}
		}
		#endregion // PresentationElement property
		#region ModelElement property
		private readonly TModelElement myModelElement;
		/// <summary>
		/// The <see cref="T:Microsoft.VisualStudio.Modeling.ModelElement"/> of type <typeparamref name="TModelElement"/> that
		/// <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}.PresentationElement"/> is associated with.
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
		/// Returns the class name of <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}.ModelElement"/>
		/// (the <typeparamref name="TModelElement"/> that
		/// <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}.PresentationElement"/> is associated with).
		/// </summary>
		public override string GetClassName()
		{
			ModelElement modelElement = this.ModelElement;
			return modelElement != null ? TypeDescriptor.GetClassName(modelElement) : "";
		}
		#endregion // GetClassName method
		#region GetComponentName method
		/// <summary>
		/// Returns the component name of <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}.ModelElement"/>
		/// (the <typeparamref name="TModelElement"/> that
		/// <see cref="PresentationElementTypeDescriptor{TPresentationElement,TModelElement}.PresentationElement"/> is associated with).
		/// </summary>
		public override string GetComponentName()
		{
			ModelElement modelElement = this.ModelElement;
			return modelElement != null ? TypeDescriptor.GetComponentName(modelElement) : "";
		}
		#endregion // GetComponentName method
		#region GetDisplayProperties method
		/// <summary>
		/// Blocks editor access to <see cref="ElementTypeDescriptor.GetDisplayProperties(ModelElement,ref PropertyDescriptor)"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is not supported.", true)]
		protected new PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			throw new NotSupportedException();
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Blocks editor access to <see cref="ElementTypeDescriptor.GetDisplayProperties(ModelElement,Store,ref PropertyDescriptor)"/>.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is not supported.", true)]
		protected new PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, Store store, ref PropertyDescriptor defaultPropertyDescriptor)
		{
			throw new NotSupportedException();
		}
#endif // VISUALSTUDIO_10_0
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
			return EditorUtility.GetAttributeArray(DomainTypeDescriptor.GetRawAttributes(domainPropertyInfo.PropertyInfo));
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
			return EditorUtility.GetAttributeArray(DomainTypeDescriptor.GetRawAttributes(domainRole.LinkPropertyInfo));
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
		/// Replaces <see cref="PresentationElementTypeDescriptor.GetProperties(Attribute[])"/>.
		/// </summary>
		/// <seealso cref="PresentationElementTypeDescriptor.GetProperties(Attribute[])"/>
		/// <seealso cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			// Get the properties from the associated ModelElement to start off with.
			PropertyDescriptorCollection propertyDescriptors;
			TModelElement modelElement = this.ModelElement;
			if (modelElement != null && !modelElement.IsDeleted && !modelElement.IsDeleting)
			{
				propertyDescriptors = TypeDescriptor.GetProperties(modelElement);
				int descriptorCount = propertyDescriptors.Count;
				if (0 != descriptorCount)
				{
					PropertyDescriptor[] wrappedDescriptors = new PropertyDescriptor[descriptorCount];
					for (int i = 0; i < descriptorCount; ++i)
					{
						wrappedDescriptors[i] = EditorUtility.RedirectPropertyDescriptor(modelElement, propertyDescriptors[i], typeof(TPresentationElement));
					}
					propertyDescriptors = new PropertyDescriptorCollection(wrappedDescriptors, false);
				}
			}
			else
			{
				propertyDescriptors = new PropertyDescriptorCollection(null);
			}
			
			// Add the properties for this PresentationElement.
			TPresentationElement requestor = this.PresentationElement;
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
					ElementPropertyDescriptor propertyDescriptor = this.CreatePropertyDescriptor(requestor, domainPropertyInfo, this.GetDomainPropertyAttributes(domainPropertyInfo));
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
					// UNDONE: NOW VS2010 Add wrappers for role-based properties similar to the wrapped property descriptors on the backing element.
					HashSet<string, string> oppositePropertyNames = new HashSet<string, string>(KeyProvider<string, string>.Default, StringComparer.Ordinal, StringComparer.Ordinal);

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

				// Add any extension properties registered on the presentation element
				((IFrameworkServices)requestor.Store).PropertyProviderService.GetProvidedProperties(requestor, propertyDescriptors);
			}
			return propertyDescriptors;
		}
		#endregion // GetProperties methods
		#region Relationship property overrides
		/// <summary>
		/// Block embedding relationship property display
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}
		/// <summary>
		/// Block opposite role player property display
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			return false;
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Block collection property display
		/// </summary>
		protected override bool IncludeCollectionRoleProperties(ModelElement requestor)
		{
			return false;
		}
#endif // VISUALSTUDIO_10_0
		#endregion // Relationship property overrides
	}
}
