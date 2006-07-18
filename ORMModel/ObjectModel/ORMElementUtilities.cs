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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ExtensionElementUtility
	/// <summary>
	/// Provides static helper functions for extension <see cref="ModelElement"/>s.
	/// </summary>
	public static class ExtensionElementUtility
	{
		/// <summary>
		/// Adds the extension <see cref="ModelElement"/> specified by <paramref name="extensionElement"/> to the
		/// <see cref="IORMExtendableElement"/> specified by <paramref name="extendedElement"/>.
		/// </summary>
		public static void AddExtensionElement(IORMExtendableElement extendedElement, ModelElement extensionElement)
		{
			if (extendedElement == null)
			{
				throw new ArgumentNullException("extendedElement");
			}
			if (extensionElement == null)
			{
				throw new ArgumentNullException("extensionElement");
			}
			extendedElement.ExtensionCollection.Add(extensionElement);
		}
		/// <summary>
		/// Adds the extension <see cref="ModelError"/> specified by <paramref name="extensionError"/> to the
		/// <see cref="IORMExtendableElement"/> specified by <paramref name="extendedElement"/>.
		/// </summary>
		public static void AddExtensionModelError(IORMExtendableElement extendedElement, ModelError extensionError)
		{
			if (extendedElement == null)
			{
				throw new ArgumentNullException("extendedElement");
			}
			if (extensionError == null)
			{
				throw new ArgumentNullException("extensionError");
			}
			extendedElement.ExtensionModelErrorCollection.Add(extensionError);
		}
		/// <summary>
		/// Gets the <see cref="IORMExtendableElement"/> that the extension <see cref="ModelElement"/> specified
		/// by <paramref name="extensionElement"/> is attached to.
		/// </summary>
		public static IORMExtendableElement GetExtendedElement(ModelElement extensionElement)
		{
			if (extensionElement == null)
			{
				throw new ArgumentNullException("extensionElement");
			}
			return (IORMExtendableElement)DomainRoleInfo.GetLinkedElement(extensionElement, ORMModelElementHasExtensionElement.ExtensionDomainRoleId);
		}
		/// <summary>
		/// Gets the <see cref="IORMExtendableElement"/> that the extension <see cref="ModelError"/> specified
		/// by <paramref name="extensionError"/> is attached to.
		/// </summary>
		public static IORMExtendableElement GetExtendedErrorOwnerElement(ModelError extensionError)
		{
			if (extensionError == null)
			{
				throw new ArgumentNullException("extensionError");
			}
			return (IORMExtendableElement)DomainRoleInfo.GetLinkedElement(extensionError, ORMModelElementHasExtensionModelError.ExtensionModelErrorDomainRoleId);
		}
	}
	#endregion // ExtensionElementUtility

	#region ExtendableElementUtility
	/// <summary>
	/// Provides static helper functions for <see cref="IORMExtendableElement"/>s.
	/// </summary>
	public static class ExtendableElementUtility
	{
		/// <summary>
		/// Adds the properties from the extension <see cref="ModelElement"/>s of the
		/// <see cref="IORMExtendableElement"/> specified by <paramref name="extendableElement"/>
		/// to the <see cref="PropertyDescriptorCollection"/> sepcified by <paramref name="properties"/>.
		/// </summary>
		/// <param name="extendableElement">
		/// The <see cref="IORMExtendableElement"/> from which the extension properties should be retrieved.
		/// </param>
		/// <param name="properties">
		/// The <see cref="PropertyDescriptorCollection"/> to which the extension properties should be added.
		/// </param>
		public static void GetExtensionProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
		{
			if (extendableElement == null)
			{
				throw new ArgumentNullException("extendableElement");
			}
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			foreach (ModelElement extension in extendableElement.ExtensionCollection)
			{
				IORMPropertyExtension customPropertyExtension = extension as IORMPropertyExtension;
				ORMExtensionPropertySettings settings = (customPropertyExtension != null) ? customPropertyExtension.ExtensionPropertySettings : ORMExtensionPropertySettings.MergeAsTopLevelProperty;
				if (0 != (settings & ORMExtensionPropertySettings.MergeAsExpandableProperty))
				{
					properties.Add(ExpandableExtensionPropertyDescriptor.CreateExtensionDescriptor(customPropertyExtension));
				}
				if (0 != (settings & ORMExtensionPropertySettings.MergeAsTopLevelProperty))
				{
					foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(extension))
					{
						properties.Add(descriptor);
					}
				}
			}
		}

		#region ExpandableExtensionPropertyDescriptor
		private sealed class ExpandableExtensionPropertyDescriptor : PropertyDescriptor
		{
			#region ExpandableExtensionConverter
			private sealed class ExpandableExtensionConverter : ExpandableObjectConverter
			{
				#region ContextWrapper
				private sealed class ContextWrapper : ITypeDescriptorContext
				{
					private ITypeDescriptorContext myInnerContext;
					private readonly PropertyDescriptor myOriginalDescriptor;
					private readonly IORMPropertyExtension myExtensionElement;
					public ContextWrapper(PropertyDescriptor descriptor, IORMPropertyExtension extensionElement)
					{
						myOriginalDescriptor = descriptor;
						myExtensionElement = extensionElement;
					}

					public ContextWrapper GetUpdatedContextWrapper(ITypeDescriptorContext context)
					{
						this.myInnerContext = context;
						return this;
					}

					IContainer ITypeDescriptorContext.Container
					{
						get
						{
							return myInnerContext.Container;
						}
					}

					object ITypeDescriptorContext.Instance
					{
						get
						{
							return myExtensionElement;
						}
					}

					void ITypeDescriptorContext.OnComponentChanged()
					{
						myInnerContext.OnComponentChanged();
					}

					bool ITypeDescriptorContext.OnComponentChanging()
					{
						return myInnerContext.OnComponentChanging();
					}

					PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
					{
						get
						{
							return myOriginalDescriptor;
						}
					}

					object IServiceProvider.GetService(Type serviceType)
					{
						return myInnerContext.GetService(serviceType);
					}
				}
				#endregion // ContextWrapper

				private readonly IORMPropertyExtension myExtensionElement;
				private readonly Type myExtensionElementType;
				private readonly PropertyDescriptor myOriginalDescriptor;
				private readonly ContextWrapper myContextWrapper;

				public ExpandableExtensionConverter(IORMPropertyExtension extension, PropertyDescriptor descriptor)
				{
					this.myExtensionElement = extension;
					this.myExtensionElementType = extension.GetType();
					this.myOriginalDescriptor = descriptor;
					this.myContextWrapper = new ContextWrapper(descriptor, myExtensionElement);
				}

				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return myOriginalDescriptor.Converter.CanConvertFrom(myContextWrapper.GetUpdatedContextWrapper(context), sourceType);
				}

				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return myOriginalDescriptor.Converter.CanConvertTo(myContextWrapper.GetUpdatedContextWrapper(context), destinationType);
				}

				public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
				{
					return myOriginalDescriptor.Converter.ConvertFrom(myContextWrapper.GetUpdatedContextWrapper(context), culture, value);
				}

				public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
				{
					if (value.GetType() == myExtensionElementType)
					{
						value = myOriginalDescriptor.GetValue(myExtensionElement);
					}
					return myOriginalDescriptor.Converter.ConvertTo(myContextWrapper.GetUpdatedContextWrapper(context), culture, value, destinationType);
				}

				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return myOriginalDescriptor.Converter.GetStandardValues(myContextWrapper.GetUpdatedContextWrapper(context));
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return myOriginalDescriptor.Converter.GetStandardValuesExclusive(myContextWrapper.GetUpdatedContextWrapper(context));
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return myOriginalDescriptor.Converter.GetStandardValuesSupported(myContextWrapper.GetUpdatedContextWrapper(context));
				}

				public override bool IsValid(ITypeDescriptorContext context, object value)
				{
					return myOriginalDescriptor.Converter.IsValid(myContextWrapper.GetUpdatedContextWrapper(context), value);
				}
			}
			#endregion // ExpandableExtensionConverter

			public static ExpandableExtensionPropertyDescriptor CreateExtensionDescriptor(IORMPropertyExtension extensionElement)
			{
				ModelElement element = extensionElement as ModelElement;
				DomainPropertyInfo extensionExpandableTopLevelPropertyInfo = element.Store.DomainDataDirectory.FindDomainProperty(extensionElement.ExtensionExpandableTopLevelPropertyId);

				// If ExtensionExpandableTopLevelPropertyGuid is invalid or Guid.Empty, FindDomainProperty returns null.
				bool readOnly = (extensionExpandableTopLevelPropertyInfo == null);

				PropertyDescriptor descriptor;
				ExpandableObjectConverter expandableConverter;

				if (!readOnly)
				{
					descriptor = Design.ORMTypeDescriptor.CreatePropertyDescriptor(element, extensionExpandableTopLevelPropertyInfo);
					expandableConverter = new ExpandableExtensionConverter(extensionElement, descriptor);
				}
				else
				{
					descriptor = null;
					expandableConverter = new ExpandableObjectConverter();
				}

				return new ExpandableExtensionPropertyDescriptor(extensionElement, expandableConverter, readOnly, descriptor);
			}

			private IORMPropertyExtension myExtensionElement;
			private PropertyDescriptor myOriginalDescriptor;
			private TypeConverter myConverter;
			private bool myReadOnly;

			private ExpandableExtensionPropertyDescriptor(IORMPropertyExtension extensionElement, TypeConverter converter, bool readOnly, PropertyDescriptor descriptor)
				: base(extensionElement.ToString(), null)
			{
				this.myExtensionElement = extensionElement;
				this.myConverter = converter;
				this.myReadOnly = readOnly;
				this.myOriginalDescriptor = descriptor;
			}

			public override TypeConverter Converter
			{
				get
				{
					return myConverter;
				}
			}

			public override Type ComponentType
			{
				get
				{
					return typeof(ExpandableExtensionPropertyDescriptor);
				}
			}

			public override object GetValue(object component)
			{
				return myExtensionElement;
			}

			public override object GetEditor(Type editorBaseType)
			{
				if (myReadOnly)
				{
					// The PropertyGrid calls this method even when we return true for IsReadOnly,
					// and it doesn't handle Exceptions well, so we're returning null instead of throwing
					return null;
				}
				return myOriginalDescriptor.GetEditor(editorBaseType);
			}

			public override bool IsReadOnly
			{
				get
				{
					return myReadOnly;
				}
			}

			public override Type PropertyType
			{
				get
				{
					return myExtensionElement.GetType();
				}
			}

			public override bool CanResetValue(object component)
			{
				return false;
			}

			public override void ResetValue(object component)
			{
				throw new NotSupportedException();
			}

			public override void SetValue(object component, object value)
			{
				if (myReadOnly)
				{
					throw new NotSupportedException();
				}
				myOriginalDescriptor.SetValue(myExtensionElement, value);
			}

			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
		}
		#endregion // ExpandableExtensionPropertyDescriptor
	}
	#endregion // ExtendableElementUtility
}
