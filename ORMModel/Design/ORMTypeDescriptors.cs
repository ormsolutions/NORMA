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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Design
{
	#region ORMTypeDescriptor utility class
	/// <summary>
	/// Provides helper methods for working with <see cref="ICustomTypeDescriptor"/>s,
	/// <see cref="PropertyDescriptor"/>s, <see cref="TypeConverter"/>s, and
	/// <see cref="Attribute"/>s, as well as their DSL Tools implementations.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class ORMTypeDescriptor
	{
		#region TypeDescriptorContext class
		/// <summary>
		/// Simple implementation of <see cref="ITypeDescriptorContext"/> for a
		/// <see cref="PropertyDescriptor"/> on a <see cref="ModelElement"/>.
		/// </summary>
		[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
		public sealed class TypeDescriptorContext : ITypeDescriptorContext
		{
			private readonly ModelElement myInstance;
			private readonly PropertyDescriptor myPropertyDescriptor;

			/// <summary>
			/// Instantiates a new instance of <see cref="TypeDescriptorContext"/>.
			/// </summary>
			/// <param name="component">
			/// The instance of <see cref="ModelElement"/> containing the
			/// <see cref="PropertyDescriptor"/>that this <see cref="TypeDescriptorContext"/>
			/// is for.
			/// <see langword="null"/> may be specified for this parameter.
			/// </param>
			/// <param name="propertyDescriptor">
			/// The <see cref="PropertyDescriptor"/> that this <see cref="TypeDescriptorContext"/>
			/// if for.
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="propertyDescriptor"/> is <see langword="null"/>.
			/// </exception>
			public TypeDescriptorContext(ModelElement component, PropertyDescriptor propertyDescriptor)
			{
				this.myInstance = component;
				if (propertyDescriptor == null)
				{
					throw new ArgumentNullException("propertyDescriptor");
				}
				this.myPropertyDescriptor = propertyDescriptor;
			}

			/// <summary>
			/// Returns the <see cref="ModelElement"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned may be <see langword="null"/>.
			/// </summary>
			public ModelElement Instance
			{
				get
				{
					return this.myInstance;
				}
			}
			/// <summary>
			/// Returns the <see cref="ModelElement"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned may be <see langword="null"/>.
			/// </summary>
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return this.myInstance;
				}
			}
			/// <summary>
			/// Returns the <see cref="PropertyDescriptor"/> that was passed to
			/// <see cref="TypeDescriptorContext(ModelElement,PropertyDescriptor)"/>.
			/// The value returned will not be <see langword="null"/>.
			/// </summary>
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this.myPropertyDescriptor;
				}
			}
			/// <summary>
			/// If <see cref="Instance"/> is not <see langword="null"/>, calls <see cref="Store.GetService"/>
			/// on the instance of <see cref="Store"/> returned by <see cref="ModelElement.Store"/>. Otherwise,
			/// returns <see langword="null"/>.
			/// </summary>
			public object GetService(Type serviceType)
			{
				ModelElement instance = myInstance;
				if (instance != null)
				{
					return instance.Store.GetService(serviceType);
				}
				return null;
			}
			/// <summary>
			/// Returns <see langword="null"/>.
			/// </summary>
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}
			/// <summary>
			/// Does nothing.
			/// </summary>
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}
			/// <summary>
			/// Does nothing.
			/// </summary>
			/// <returns>Always returns <see langword="false"/>.</returns>
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}
		}
		#endregion // TypeDescriptorContext class

		#region GetMemberAttributes method
		/// <summary>
		/// Returns the custom <see cref="Attribute"/>s from <paramref name="memberInfo"/>.
		/// </summary>
		/// <param name="memberInfo">
		/// The <see cref="MemberInfo"/> for which the custom <see cref="Attribute"/>s are desired.
		/// </param>
		/// <returns>
		/// The custom <see cref="Attribute"/>s from <paramref name="memberInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="memberInfo"/> is <see langword="null"/>.
		/// </exception>
		public static Attribute[] GetMemberAttributes(MemberInfo memberInfo)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			object[] attributeObjects = memberInfo.GetCustomAttributes(true);
			Attribute[] attributes = new Attribute[attributeObjects.Length];
			int targetIndex = 0;
			for (int sourceIndex = 0; sourceIndex < attributeObjects.Length; sourceIndex++)
			{
				Attribute attribute = attributeObjects[sourceIndex] as Attribute;
				if (attribute != null)
				{
					attributes[targetIndex++] = attribute;
				}
			}
			if (targetIndex != attributes.Length)
			{
				Array.Resize(ref attributes, targetIndex);
			}
			return attributes;
		}
		#endregion // GetMemberAttributes method

		#region CreateTypeDescriptorContext method
		/// <summary>
		/// Creates a <see cref="TypeDescriptorContext"/> for the <see cref="DomainPropertyInfo"/>
		/// specified by <paramref name="domainPropertyInfo"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a
		/// <see cref="TypeDescriptorContext"/> should be created.
		/// <see langword="null"/> may be specified for this parameter.
		/// </param>
		/// <param name="domainPropertyInfo">
		/// The <see cref="DomainPropertyInfo"/> for which a <see cref="TypeDescriptorContext"/>
		/// should be created.
		/// </param>
		/// <returns>
		/// A <see cref="TypeDescriptorContext"/> for the <see cref="DomainPropertyInfo"/>
		/// specified by <paramref name="domainPropertyInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainPropertyInfo"/> is <see langword="null"/>.
		/// </exception>
		public static TypeDescriptorContext CreateTypeDescriptorContext(ModelElement element, DomainPropertyInfo domainPropertyInfo)
		{
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			return new TypeDescriptorContext(element, ORMTypeDescriptor.CreatePropertyDescriptor(element, domainPropertyInfo));
		}
		#endregion // CreateTypeDescriptorContext method

		#region CreatePropertyDescriptor methods
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <c>Name</c> property of <paramref name="element"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the <c>Name</c> property for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <c>Name</c> property of <paramref name="element"/>
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <paramref name="element"/> does not have a <c>Name</c> property.
		/// </exception>
		public static PropertyDescriptor CreateNamePropertyDescriptor(ModelElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			DomainPropertyInfo nameDomainPropertyInfo = element.GetDomainClass().NameDomainProperty;
			if (nameDomainPropertyInfo == null)
			{
				// Cheat and use the Exception that DSL Tools throws so that we don't need to worry about localizing it.
				try
				{
					DomainClassInfo.GetName(element);
				}
				catch (InvalidOperationException ex)
				{
					throw ex; // Yes, we want to reset the stack trace.
				}
			}
			return ORMTypeDescriptor.CreatePropertyDescriptor(element, nameDomainPropertyInfo);
		}
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> with a
		/// <see cref="DomainObjectInfo.Id"/> equal to <paramref name="domainPropertyId"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <param name="domainPropertyId">
		/// The <see cref="DomainObjectInfo.Id"/> of the <see cref="DomainPropertyInfo"/> for which a
		/// <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> with a
		/// <see cref="DomainObjectInfo.Id"/> equal to <paramref name="domainPropertyId"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="element"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="DomainDataNotFoundException">
		/// A <see cref="DomainPropertyInfo"/> with a <see cref="DomainObjectInfo.Id"/> equal to
		/// <paramref name="domainPropertyId"/> could not be found.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, Guid domainPropertyId)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return ORMTypeDescriptor.CreatePropertyDescriptor(element, element.Store.DomainDataDirectory.GetDomainProperty(domainPropertyId));
		}
		/// <summary>
		/// Creates a <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> specified
		/// by <paramref name="domainPropertyInfo"/>.
		/// </summary>
		/// <param name="element">
		/// The instance of <see cref="ModelElement"/> containing the property for which a <see cref="PropertyDescriptor"/>
		/// should be created.
		/// <see langword="null"/> may be specified for this parameter.
		/// </param>
		/// <param name="domainPropertyInfo">
		/// The <see cref="DomainPropertyInfo"/> for which a <see cref="PropertyDescriptor"/> should be created.
		/// </param>
		/// <returns>
		/// A <see cref="PropertyDescriptor"/> for the <see cref="DomainPropertyInfo"/> specified
		/// by <paramref name="domainPropertyInfo"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="domainPropertyInfo"/> is <see langword="null"/>.
		/// </exception>
		public static PropertyDescriptor CreatePropertyDescriptor(ModelElement element, DomainPropertyInfo domainPropertyInfo)
		{
			if (domainPropertyInfo == null)
			{
				throw new ArgumentNullException("domainPropertyInfo");
			}
			if (element != null)
			{
				ICustomTypeDescriptor typeDescriptor = TypeDescriptor.GetProvider(element).GetTypeDescriptor(element.GetType(), element);
				Type propertyType = domainPropertyInfo.PropertyType;
				string propertyName = domainPropertyInfo.Name;
				PropertyDescriptorCollection propertyDescriptors = typeDescriptor.GetProperties();
				foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
				{
					if (propertyDescriptor.PropertyType == propertyType && propertyDescriptor.Name == propertyName)
					{
						return propertyDescriptor;
					}
				}
			}
			return new ElementPropertyDescriptor(element, domainPropertyInfo, GetMemberAttributes(domainPropertyInfo.PropertyInfo));
		}
		#endregion // CreatePropertyDescriptor methods

		#region GetDisplayName methods
		/// <summary>
		/// Returns the <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="component"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="Object"/> for which the <see cref="DisplayNameAttribute.DisplayName"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="component"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="component"/> is <see langword="null"/> or does not have a <see cref="DisplayNameAttribute"/>,
		/// the <see cref="DisplayNameAttribute.DisplayName"/> for <see cref="DisplayNameAttribute.Default"/> is returned.
		/// </remarks>
		public static string GetDisplayName(object component)
		{
			return ((DisplayNameAttribute)TypeDescriptor.GetAttributes(component)[typeof(DisplayNameAttribute)]).DisplayName;
		}
		/// <summary>
		/// Returns the <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">
		/// The <see cref="Type"/> for which the <see cref="DisplayNameAttribute.DisplayName"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DisplayNameAttribute.DisplayName"/> for <paramref name="componentType"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="componentType"/> is <see langword="null"/> or does not have a <see cref="DisplayNameAttribute"/>,
		/// the <see cref="DisplayNameAttribute.DisplayName"/> for <see cref="DisplayNameAttribute.Default"/> is returned.
		/// </remarks>
		public static string GetDisplayName(Type componentType)
		{
			return ((DisplayNameAttribute)TypeDescriptor.GetAttributes(componentType)[typeof(DisplayNameAttribute)]).DisplayName;
		}
		#endregion // GetDisplayName methods

		#region GetDescription methods
		/// <summary>
		/// Returns the <see cref="DescriptionAttribute.Description"/> for <paramref name="component"/>.
		/// </summary>
		/// <param name="component">
		/// The <see cref="Object"/> for which the <see cref="DescriptionAttribute.Description"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DescriptionAttribute.Description"/> for <paramref name="component"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="component"/> is <see langword="null"/> or does not have a <see cref="DescriptionAttribute"/>,
		/// <see cref="DescriptionAttribute.Description"/> for <see cref="DescriptionAttribute.Default"/> will be returned.
		/// </remarks>
		public static string GetDescription(object component)
		{
			return ((DescriptionAttribute)TypeDescriptor.GetAttributes(component)[typeof(DescriptionAttribute)]).Description;
		}
		/// <summary>
		/// Returns the <see cref="DescriptionAttribute.Description"/> for <paramref name="componentType"/>.
		/// </summary>
		/// <param name="componentType">
		/// The <see cref="Type"/> for which the <see cref="DescriptionAttribute.Description"/> is desired.
		/// </param>
		/// <returns>
		/// The <see cref="DescriptionAttribute.Description"/> for <paramref name="componentType"/>.
		/// </returns>
		/// <remarks>
		/// If <paramref name="componentType"/> is <see langword="null"/> or does not have a <see cref="DescriptionAttribute"/>,
		/// <see cref="DescriptionAttribute.Description"/> for <see cref="DescriptionAttribute.Default"/> will be returned.
		/// </remarks>
		public static string GetDescription(Type componentType)
		{
			return ((DescriptionAttribute)TypeDescriptor.GetAttributes(componentType)[typeof(DescriptionAttribute)]).Description;
		}
		#endregion // GetDescription methods
	}
	#endregion // ORMTypeDescriptor utility class


	#region TypeDescriptionProvider classes

	#region ORMTypeDescriptionProvider class
	/// <summary>
	/// <see cref="ElementTypeDescriptionProvider"/> for <typeparamref name="TModelElement"/>s.
	/// </summary>
	/// <remarks>
	/// <typeparamref name="TTypeDescriptor"/> must have a constructor that takes a single parameter
	/// of type <typeparamref name="TModelElement"/>.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ORMTypeDescriptionProvider<TModelElement, TTypeDescriptor> : ElementTypeDescriptionProvider
		where TModelElement : ModelElement
		where TTypeDescriptor : ElementTypeDescriptor
	{
		private static readonly RuntimeTypeHandle TypeDescriptorTypeHandle = typeof(TTypeDescriptor).TypeHandle;
		private static readonly RuntimeMethodHandle TypeDescriptorConstructorHandle = Type.GetTypeFromHandle(TypeDescriptorTypeHandle).GetConstructor(
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
			null, new Type[] { typeof(TModelElement) }, null).MethodHandle;

		/// <summary>See <see cref="ElementTypeDescriptionProvider.CreateTypeDescriptor"/>.</summary>
		protected sealed override ElementTypeDescriptor CreateTypeDescriptor(ModelElement element)
		{
			return (ElementTypeDescriptor)((ConstructorInfo)ConstructorInfo.GetMethodFromHandle(TypeDescriptorConstructorHandle, TypeDescriptorTypeHandle)).Invoke(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
				null, new object[] { element as TModelElement }, null);
		}
	}
	#endregion // ORMTypeDescriptionProvider class

	#region ORMPresentationTypeDescriptionProvider class
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptionProvider"/> for <typeparamref name="TPresentationElement"/>s.
	/// </summary>
	/// <remarks>
	/// <typeparamref name="TTypeDescriptor"/> must have a constructor that takes two parameters of the
	/// types specified by <typeparamref name="TPresentationElement"/> and <typeparamref name="TModelElement"/>.
	/// </remarks>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ORMPresentationTypeDescriptionProvider<TPresentationElement, TModelElement, TTypeDescriptor> : PresentationElementTypeDescriptionProvider
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
		where TTypeDescriptor : PresentationElementTypeDescriptor
	{
		private static readonly RuntimeTypeHandle TypeDescriptorTypeHandle = typeof(TTypeDescriptor).TypeHandle;
		private static readonly RuntimeMethodHandle TypeDescriptorConstructorHandle = Type.GetTypeFromHandle(TypeDescriptorTypeHandle).GetConstructor(
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
			null, new Type[] { typeof(TPresentationElement), typeof(TModelElement) }, null).MethodHandle;
		/// <summary>See <see cref="PresentationElementTypeDescriptionProvider.CreatePresentationElementTypeDescriptor"/>.</summary>
		protected sealed override PresentationElementTypeDescriptor CreatePresentationElementTypeDescriptor(PresentationElement presentationElement, ModelElement selectedElement)
		{
			return (PresentationElementTypeDescriptor)((ConstructorInfo)ConstructorInfo.GetMethodFromHandle(TypeDescriptorConstructorHandle, TypeDescriptorTypeHandle)).Invoke(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding,
				null, new object[] { presentationElement as TPresentationElement, selectedElement as TModelElement }, null);
		}
	}
	#endregion // ORMPresentationTypeDescriptionProvider class

	#endregion // TypeDescriptionProvider classes


	#region ORM ModelElement TypeDescriptor classes

	#region ORMModelElementTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ORMModelElement"/>s of type <typeparamref name="TModelElement"/>.
	/// </summary>
	/// <typeparam name="TModelElement">
	/// The type of the <see cref="ORMModelElement"/> that this <see cref="ORMModelElementTypeDescriptor{TModelElement}"/> is for.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMModelElementTypeDescriptor<TModelElement> : ElementTypeDescriptor
		where TModelElement : ORMModelElement
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMModelElementTypeDescriptor{TModelElement}"/> for
		/// the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		public ORMModelElementTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
			// The ElementTypeDescriptor constructor already checked selectedElement for null.
			myElement = selectedElement;
		}

		private readonly TModelElement myElement;
		/// <summary>
		/// The <see cref="ModelElement"/> of type <typeparamref name="TModelElement"/> that
		/// this <see cref="ORMModelElementTypeDescriptor{TModelElement}"/> is for.
		/// </summary>
		protected TModelElement ORMElement
		{
			get
			{
				return myElement;
			}
		}

		/// <summary>
		/// Returns the <see cref="DomainObjectInfo.DisplayName"/> for the <see cref="DomainClassInfo"/>
		/// of the <typeparamref name="TModelElement"/> that this <see cref="ORMModelElementTypeDescriptor{TModelElement}"/>
		/// is for.
		/// </summary>
		public override string GetClassName()
		{
			return ORMElement.GetDomainClass().DisplayName;
		}

		/// <summary>
		/// Calls <see cref="GetProperties(Attribute[])"/>, passing <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}
		/// <summary>
		/// Adds extension properties to the <see cref="PropertyDescriptorCollection"/> before returning it.
		/// </summary>
		/// <seealso cref="ElementTypeDescriptor.GetProperties(Attribute[])"/>.
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(attributes);
			ExtendableElementUtility.GetExtensionProperties(myElement, properties);
			return properties;
		}

		/// <summary>
		/// Not used, don't look for them
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}

		/// <summary>
		/// Let our *Display properties handle these
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			// UNDONE: We may want to lose the *Display properties. Need a way to filter
			// the contents of a RolePlayerPropertyDescriptor dropdown list
			// UNDONE: MSBUG RolePlayerPropertyDescriptor should respect the System.ComponentModel.EditorAttribute on the
			// generated property.
			return false;
		}
	}
	#endregion // ORMModelElementTypeDescriptor class

	#region UniquenessConstraintTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="UniquenessConstraint"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class UniquenessConstraintTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : UniquenessConstraint
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="UniquenessConstraintTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public UniquenessConstraintTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>
		/// Display different class names for internal and external <see cref="UniquenessConstraint"/>s.
		/// </summary>
		public override string GetClassName()
		{
			return ORMElement.IsInternal ? ResourceStrings.InternalUniquenessConstraint : ResourceStrings.ExternalUniquenessConstraint;
		}

		/// <summary>
		/// Ensure that the <see cref="UniquenessConstraint.IsPreferred"/> property is read-only
		/// when the <see cref="FactType.InternalUniquenessConstraintChangeRule"/> is
		/// unable to make it <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == UniquenessConstraint.IsPreferredDomainPropertyId)
			{
				UniquenessConstraint uniquenessConstraint = ORMElement;
				return uniquenessConstraint.IsPreferred ? false : !uniquenessConstraint.TestAllowPreferred(null, false);
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
	#endregion // UniquenessConstraintTypeDescriptor class

	#region MandatoryConstraintTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="MandatoryConstraint"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MandatoryConstraintTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : MandatoryConstraint
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="UniquenessConstraintTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public MandatoryConstraintTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>
		/// Distinguish between disjunctive and simple <see cref="MandatoryConstraint"/>s in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			return ORMElement.IsSimple ? ResourceStrings.SimpleMandatoryConstraint : ResourceStrings.DisjunctiveMandatoryConstraint;
		}
	}
	#endregion // MandatoryConstraintTypeDescriptor class

	#region ReadingTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Reading"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReadingTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : Reading
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ReadingTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ReadingTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}
		/// <summary>
		/// Ensure that the <see cref="Reading.IsPrimaryForReadingOrder"/> and <see cref="Reading.IsPrimaryForFactType"/>
		/// properties are read-only when they are <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (propertyId.Equals(Reading.IsPrimaryForReadingOrderDomainPropertyId))
			{
				return ORMElement.IsPrimaryForReadingOrder;
			}
			else if (propertyId.Equals(Reading.IsPrimaryForFactTypeDomainPropertyId))
			{
				return ORMElement.IsPrimaryForFactType;
			}
			else
			{
				return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
			}
		}
		/// <summary>See <see cref="ElementTypeDescriptor.GetComponentName"/>.</summary>
		public override string GetComponentName()
		{
			Reading reading = ORMElement;
			ReadingOrder readingOrder = reading.ReadingOrder;
			if (readingOrder != null)
			{
				// UNDONE: Localize the format string
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}{2}", readingOrder.FactType.Name, ResourceStrings.ReadingType, readingOrder.ReadingCollection.IndexOf(reading) + 1);
			}
			return base.GetComponentName();
		}
	}
	#endregion // ReadingTypeDescriptor class

	#region ObjectTypeTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="ObjectType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ObjectTypeTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : ObjectType
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ObjectTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public ObjectTypeTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			ObjectType objectType = ORMElement;
			Guid propertyId = domainProperty.Id;
			if (propertyId.Equals(ObjectType.DataTypeDisplayDomainPropertyId) ||
				propertyId.Equals(ObjectType.ScaleDomainPropertyId) ||
				propertyId.Equals(ObjectType.LengthDomainPropertyId) ||
				propertyId.Equals(ObjectType.ValueRangeTextDomainPropertyId))
			{
				return objectType.IsValueType || objectType.HasReferenceMode;
			}
			else if (propertyId.Equals(ObjectType.NestedFactTypeDisplayDomainPropertyId) ||
				propertyId.Equals(ObjectType.ReferenceModeDisplayDomainPropertyId))
			{
				return !objectType.IsValueType;
			}
			else
			{
				return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
			}
		}

		/// <summary>
		/// Distinguish between a value type and an entity type in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			return ORMElement.IsValueType ? ResourceStrings.ValueType : ResourceStrings.EntityType;
		}

		/// <summary>
		/// Ensure that the <see cref="ObjectType.IsValueType"/> property is read-only when
		/// <see cref="ObjectType.NestedFactType"/> is not <see langword="null"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			ObjectType objectType = ORMElement;
			Guid propertyId = propertyDescriptor.DomainPropertyInfo.Id;
			if (propertyId.Equals(ObjectType.IsValueTypeDomainPropertyId))
			{
				return objectType.NestedFactType != null || objectType.PreferredIdentifier != null || objectType.IsSubtypeOrSupertype;
			}
			else if (propertyId.Equals(ObjectType.ValueRangeTextDomainPropertyId))
			{
				return !(objectType.IsValueType || objectType.HasReferenceMode);
			}
			else
			{
				return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
			}
		}
	}
	#endregion // ObjectTypeTypeDescriptor class

	#region DataTypeTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="DataType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DataTypeTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : DataType
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="DataTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public DataTypeTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>
		/// Always returns <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			return true;
		}
	}
	#endregion // DataTypeTypeDescriptor class

	#region RoleTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="Role"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RoleTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : Role
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="RoleTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public RoleTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>See <see cref="ElementTypeDescriptor.ShouldCreatePropertyDescriptor"/>.</summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			Guid propertyId = domainProperty.Id;
			if (propertyId.Equals(Role.MultiplicityDomainPropertyId))
			{
				FactType fact = ORMElement.FactType;
				// Display for binary fact types
				return fact != null && fact.RoleCollection.Count == 2;
			}
			else if (propertyId.Equals(Role.MandatoryConstraintNameDomainPropertyId) ||
				propertyId.Equals(Role.MandatoryConstraintModalityDomainPropertyId))
			{
				return ORMElement.SimpleMandatoryConstraint != null;
			}
			else if (propertyId.Equals(Role.ObjectificationOppositeRoleNameDomainPropertyId))
			{
				FactType fact = ORMElement.FactType;
				return fact != null && fact.Objectification != null;
			}
			else
			{
				return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
			}
		}
		/// <summary>
		/// Ensure that the <see cref="Role.ValueRangeText"/> property is read-only when the
		/// <see cref="Role.RolePlayer"/> is an entity type without a <see cref="ReferenceMode"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id == Role.ValueRangeTextDomainPropertyId)
			{
				Role role = ORMElement;
				FactType fact = role.FactType;
				ObjectType rolePlayer = role.RolePlayer;
				return fact != null && rolePlayer != null && !(rolePlayer.IsValueType || rolePlayer.HasReferenceMode);
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
	#endregion // RoleTypeDescriptor class

	#region FactTypeTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="FactType"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FactTypeTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : FactType
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="FactTypeTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public FactTypeTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>
		/// Ensure that the <see cref="FactType.Name"/> property is read-only when
		/// <see cref="FactType.Objectification"/> is <see langword="null"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id.Equals(FactType.NameDomainPropertyId))
			{
				return ORMElement.Objectification == null;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}

		/// <summary>
		/// Stop the <see cref="FactType.DerivationStorageDisplay"/> property from displaying if
		/// no <see cref="FactType.DerivationRule"/> is specified.
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			if (domainProperty.Id.Equals(FactType.DerivationStorageDisplayDomainPropertyId))
			{
				return ORMElement.DerivationRule != null;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Distinguish between objectified and non-objectified <see cref="FactType"/>s in the property grid display.
		/// </summary>
		public override string GetClassName()
		{
			Objectification objectification = ORMElement.Objectification;
			return (objectification == null || objectification.IsImplied) ? ResourceStrings.FactType : ResourceStrings.ObjectifiedFactType;
		}
	}
	#endregion // FactTypeTypeDescriptor class

	#region SubtypeFactTypeDescriptor class
	/// <summary>
	/// <see cref="ElementTypeDescriptor"/> for <see cref="SubtypeFact"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class SubtypeFactTypeDescriptor<TModelElement> : ORMModelElementTypeDescriptor<TModelElement>
		where TModelElement : SubtypeFact
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="SubtypeFactTypeDescriptor{TModelElement}"/>
		/// for <paramref name="selectedElement"/>.
		/// </summary>
		public SubtypeFactTypeDescriptor(TModelElement selectedElement)
			: base(selectedElement)
		{
		}

		/// <summary>
		/// Display this type with a different name than a <see cref="FactType"/>.
		/// </summary>
		public override string GetClassName()
		{
			return ResourceStrings.SubtypeFact;
		}

		/// <summary>
		/// Display a formatted string defining the relationship
		/// for the component name.
		/// </summary>
		public override string GetComponentName()
		{
			SubtypeFact subtypeFact = ORMElement;
			ObjectType subtype;
			ObjectType supertype;
			if ((subtype = subtypeFact.Subtype) != null && (supertype = subtypeFact.Supertype) != null)
			{
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.SubtypeFactComponentNameFormat, TypeDescriptor.GetComponentName(subtype), TypeDescriptor.GetComponentName(supertype));
			}
			return base.GetComponentName();
		}

		/// <summary>
		/// Hide the <see cref="FactType.NestingTypeDisplay"/> property.
		/// </summary>
		protected override bool ShouldCreatePropertyDescriptor(ModelElement requestor, DomainPropertyInfo domainProperty)
		{
			if (domainProperty.Id.Equals(FactType.NestingTypeDisplayDomainPropertyId))
			{
				return false;
			}
			return base.ShouldCreatePropertyDescriptor(requestor, domainProperty);
		}

		/// <summary>
		/// Ensure that the <see cref="SubtypeFact.IsPrimary"/> property is read-only when
		/// it is <see langword="true"/>.
		/// </summary>
		protected override bool IsPropertyDescriptorReadOnly(ElementPropertyDescriptor propertyDescriptor)
		{
			if (propertyDescriptor.DomainPropertyInfo.Id.Equals(SubtypeFact.IsPrimaryDomainPropertyId))
			{
				return ORMElement.IsPrimary;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
	}
	#endregion // SubtypeFactTypeDescriptor class

	#endregion // ORM ModelElement TypeDescriptor classes


	#region ORM PresentationElement TypeDescriptor classes

	#region ORMPresentationElementTypeDescriptor class
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for ORM <see cref="PresentationElement"/>s of type <typeparamref name="TPresentationElement"/>.
	/// </summary>
	/// <typeparam name="TPresentationElement">
	/// The type of the ORM <see cref="PresentationElement"/> that this <see cref="ORMPresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
	/// </typeparam>
	/// <typeparam name="TModelElement">
	/// The type of the ORM <see cref="ModelElement"/> that <typeparamref name="TPresentationElement"/> is associated with.
	/// </typeparam>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ORMPresentationElementTypeDescriptor<TPresentationElement, TModelElement> : PresentationElementTypeDescriptor
		where TPresentationElement : PresentationElement
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMPresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> for
		/// the instance of <typeparamref name="TPresentationElement"/> specified by <paramref name="presentationElement"/>
		/// that is associated with the instance of <typeparamref name="TModelElement"/> specified by <paramref name="selectedElement"/>.
		/// </summary>
		protected ORMPresentationElementTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
		{
			// The PresentationElementTypeDescriptor constructor already checked presentationElement for null.
			myPresentationElement = presentationElement;
			// The ElementTypeDescriptor constructor already checked selectedElement for null.
			myElement = selectedElement;
		}

		private readonly TPresentationElement myPresentationElement;
		/// <summary>
		/// The <see cref="PresentationElement"/> of type <typeparamref name="TPresentationElement"/> that
		/// this <see cref="ORMPresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		protected TPresentationElement ORMPresentationElement
		{
			get
			{
				return myPresentationElement;
			}
		}

		private readonly TModelElement myElement;
		/// <summary>
		/// The <see cref="ModelElement"/> of type <typeparamref name="TModelElement"/> that
		/// this <see cref="ORMPresentationElementTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		protected TModelElement ORMElement
		{
			get
			{
				return myElement;
			}
		}

		/// <summary>
		/// Calls <see cref="PresentationElementTypeDescriptor.GetProperties(Attribute[])"/>, passing
		/// <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		/// <summary>
		/// Returns the class name of the associated <typeparamref name="TModelElement"/>.
		/// </summary>
		public override string GetClassName()
		{
			return TypeDescriptor.GetClassName(ORMElement);
		}

		/// <summary>
		/// Returns the component name of the associated <typeparamref name="TModelElement"/>.
		/// </summary>
		public override string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(ORMElement);
		}

		/// <summary>
		/// Not used, don't look for them
		/// </summary>
		protected override bool IncludeEmbeddingRelationshipProperties(ModelElement requestor)
		{
			return false;
		}

		/// <summary>
		/// Let our *Display properties handle these
		/// </summary>
		protected override bool IncludeOppositeRolePlayerProperties(ModelElement requestor)
		{
			// UNDONE: We may want to lose the *Display properties. Need a way to filter
			// the contents of a RolePlayerPropertyDescriptor dropdown list
			// UNDONE: MSBUG RolePlayerPropertyDescriptor should respect the System.ComponentModel.EditorAttribute on the
			// generated property.
			return false;
		}
	}
	#endregion // ORMPresentationElementTypeDescriptor class

	#region ORMDiagramTypeDescriptor class
	/// <summary>
	/// <see cref="DiagramTypeDescriptor"/> for <see cref="ORMDiagram"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMDiagramTypeDescriptor<TPresentationElement, TModelElement> : DiagramTypeDescriptor
		where TPresentationElement : ORMDiagram
		where TModelElement : ORMModel
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMDiagramTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ORMDiagramTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
		{
			// The PresentationElementTypeDescriptor constructor already checked presentationElement for null.
			myPresentationElement = presentationElement;
			// The ElementTypeDescriptor constructor already checked selectedElement for null.
			myElement = selectedElement;
		}

		private readonly TPresentationElement myPresentationElement;
		/// <summary>
		/// The <see cref="PresentationElement"/> of type <typeparamref name="TPresentationElement"/> that
		/// this <see cref="ORMDiagramTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		protected TPresentationElement ORMPresentationElement
		{
			get
			{
				return myPresentationElement;
			}
		}

		private readonly TModelElement myElement;
		/// <summary>
		/// The <see cref="ModelElement"/> of type <typeparamref name="TModelElement"/> that
		/// this <see cref="ORMDiagramTypeDescriptor{TPresentationElement,TModelElement}"/> is for.
		/// </summary>
		protected TModelElement ORMElement
		{
			get
			{
				return myElement;
			}
		}

		/// <summary>
		/// Calls <see cref="PresentationElementTypeDescriptor.GetProperties(Attribute[])"/>, passing
		/// <see langword="null"/> as the parameter.
		/// </summary>
		public sealed override PropertyDescriptorCollection GetProperties()
		{
			return GetProperties(null);
		}

		/// <summary>
		/// Returns the class name of the associated <typeparamref name="TModelElement"/>.
		/// </summary>
		public override string GetClassName()
		{
			return TypeDescriptor.GetClassName(ORMElement);
		}

		/// <summary>
		/// Returns the component name of the associated <typeparamref name="TModelElement"/>.
		/// </summary>
		public override string GetComponentName()
		{
			return TypeDescriptor.GetComponentName(ORMElement);
		}
	}
	#endregion // ORMDiagramTypeDescriptor class

	#region ORMBaseShapeTypeDescriptor class
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="ORMBaseShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMBaseShapeTypeDescriptor<TPresentationElement, TModelElement> : ORMPresentationElementTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : ORMBaseShape
		where TModelElement : ORMModelElement
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMBaseShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ORMBaseShapeTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
		{
		}

		// This class does not have anything customized in it at the present time, but may in the future
		// (e.g. for supporting shape extensions, etc.).
	}
	#endregion // ORMBaseShapeTypeDescriptor class
	#region ORMBaseBinaryLinkShapeTypeDescriptor class
	/// <summary>
	/// <see cref="PresentationElementTypeDescriptor"/> for <see cref="ORMBaseBinaryLinkShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ORMBaseBinaryLinkShapeTypeDescriptor<TPresentationElement, TModelElement> : ORMPresentationElementTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : ORMBaseBinaryLinkShape
		where TModelElement : ModelElement
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMBaseShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public ORMBaseBinaryLinkShapeTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
		{
		}

		// This class does not have anything customized in it at the present time, but may in the future
		// (e.g. for supporting shape extensions, etc.).
	}
	#endregion // ORMBaseBinaryLinkShapeTypeDescriptor class
	#region FactTypeShapeTypeDescriptor class
	/// <summary>
	/// <see cref="ORMBaseShapeTypeDescriptor{TPresentationElement,TModelElement}"/> for <see cref="FactTypeShape"/>s.
	/// </summary>
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class FactTypeShapeTypeDescriptor<TPresentationElement, TModelElement> : ORMBaseShapeTypeDescriptor<TPresentationElement, TModelElement>
		where TPresentationElement : FactTypeShape
		where TModelElement : FactType
	{
		/// <summary>
		/// Instantiates a new instance of <see cref="FactTypeShapeTypeDescriptor{TPresentationElement,TModelElement}"/>
		/// for <paramref name="presentationElement"/>.
		/// </summary>
		public FactTypeShapeTypeDescriptor(TPresentationElement presentationElement, TModelElement selectedElement)
			: base(presentationElement, selectedElement)
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
			FactType factType = ORMElement;
			if (FactTypeShape.ShouldDrawObjectification(factType))
			{
				FactTypeShape factTypeShape = ORMPresentationElement;
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
	#endregion // FactTypeShapeTypeDescriptor class

	#endregion // ORM PresentationElement TypeDescriptor classes
}
