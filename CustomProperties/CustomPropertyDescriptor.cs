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
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	internal sealed class CustomPropertyTypeDescriptor : ElementTypeDescriptor<CustomProperty>
	{
		public CustomPropertyTypeDescriptor(ICustomTypeDescriptor parent, CustomProperty selectedElement)
			: base(parent, selectedElement)
		{
		}

		public sealed override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = this.Properties;
			if (properties != null)
			{
				return properties;
			}
			else
			{
				return base.GetProperties(attributes);
			}
		}

		private PropertyDescriptorCollection _properties;
		private PropertyDescriptorCollection Properties
		{
			get
			{
				PropertyDescriptorCollection properties = this._properties;
				if (properties == null)
				{
					CustomPropertyDefinition customPropertyDefinition = base.ModelElement.CustomPropertyDefinition;
					if (customPropertyDefinition != null)
					{
						return this._properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { CustomPropertyDescriptor.GetDescriptorForCustomPropertyDefinition(customPropertyDefinition) }, true);
					}
				}
				return properties;
			}
		}
	}
	internal sealed class CustomPropertyDescriptor : PropertyDescriptor
	{
		private static readonly Dictionary<CustomPropertyDefinition, CustomPropertyDescriptor> Descriptors = new Dictionary<CustomPropertyDefinition, CustomPropertyDescriptor>();
		public static CustomPropertyDescriptor GetDescriptorForCustomPropertyDefinition(CustomPropertyDefinition customPropertyDefinition)
		{
			CustomPropertyDescriptor descriptor;
			if (!Descriptors.TryGetValue(customPropertyDefinition, out descriptor))
			{
				descriptor = new CustomPropertyDescriptor(customPropertyDefinition);
				Descriptors[customPropertyDefinition] = descriptor;
			}
			return descriptor;
		}

		private readonly CustomPropertyDefinition customPropertyDefinition;
		private CustomPropertyDescriptor(CustomPropertyDefinition customPropertyDefinition)
			: base(customPropertyDefinition.Name, null)
		{
			this.customPropertyDefinition = customPropertyDefinition;
		}

		#region Simple overrides
		public sealed override string Category
		{
			get
			{
				return this.customPropertyDefinition.Category;
			}
		}
		public sealed override string Description
		{
			get
			{
				return this.customPropertyDefinition.Description;
			}
		}
		public sealed override string Name
		{
			get
			{
				return this.customPropertyDefinition.Name;
			}
		}
		public sealed override string DisplayName
		{
			get
			{
				return this.customPropertyDefinition.Name;
			}
		}
		public sealed override bool IsBrowsable
		{
			get
			{
				return true;
			}
		}
		public sealed override bool IsLocalizable
		{
			get
			{
				return false;
			}
		}
		public sealed override bool CanResetValue(object component)
		{
			return true;
		}
		public sealed override Type ComponentType
		{
			get
			{
				return typeof(ORMModelElement);
			}
		}
		public sealed override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}
		#endregion // Simple overrides

		public sealed override Type PropertyType
		{
			get
			{
				switch (customPropertyDefinition.DataType)
				{
					case CustomPropertyDataType.Integer:
						return typeof(long?);
					case CustomPropertyDataType.Decimal:
						return typeof(decimal?);
					case CustomPropertyDataType.DateTime:
						return typeof(DateTime?);
					case CustomPropertyDataType.CustomEnumeration:
					case CustomPropertyDataType.String:
					default:
						return typeof(string);
				}
			}
		}

		public sealed override object GetEditor(Type editorBaseType)
		{
			switch (this.customPropertyDefinition.DataType)
			{
				case CustomPropertyDataType.CustomEnumeration:
					// UNDONE: Return custom enumeration editor
					return null;
				default:
					return TypeDescriptor.GetEditor(this.PropertyType, editorBaseType);
			}
		}

		public sealed override TypeConverter Converter
		{
			get
			{
				switch (this.customPropertyDefinition.DataType)
				{
					case CustomPropertyDataType.CustomEnumeration:
						// UNDONE: Return custom enumeration converter
						return null;
					case CustomPropertyDataType.String:
						return CustomPropertyStringConverter.Instance;
					default:
						return TypeDescriptor.GetConverter(this.PropertyType);
				}
			}
		}

		#region CustomProperty TypeConverters
		private sealed class CustomPropertyStringConverter : StringConverter
		{
			private CustomPropertyStringConverter()
				: base()
			{
			}
			public static readonly CustomPropertyStringConverter Instance = new CustomPropertyStringConverter();
			public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				// Turn empty string into null
				string stringValue = value as string;
				if ((object)stringValue != null && stringValue.Length == 0)
				{
					return null;
				}
				else
				{
					return base.ConvertFrom(context, culture, value);
				}
			}
		}
		#endregion // CustomProperty TypeConverters

		private CustomProperty FindCustomProperty(ORMModelElement modelElement)
		{
			if (modelElement != null)
			{
				foreach (ModelElement extensionElement in modelElement.ExtensionCollection)
				{
					CustomProperty customProperty = extensionElement as CustomProperty;
					if (customProperty != null && customProperty.CustomPropertyDefinition == this.customPropertyDefinition)
					{
						return customProperty;
					}
				}
			}
			return null;
		}
		private Transaction StartTransaction(ORMModelElement modelElement)
		{
			return modelElement.Store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(this.customPropertyDefinition.Name));
		}

		public sealed override object GetValue(object component)
		{
			CustomProperty customProperty = FindCustomProperty(EditorUtility.ResolveContextInstance(component, false) as ORMModelElement);
			if (customProperty != null)
			{
				return customProperty.Value;
			}
			return this.customPropertyDefinition.DefaultValue;
		}

		public sealed override bool ShouldSerializeValue(object component)
		{
			return FindCustomProperty(EditorUtility.ResolveContextInstance(component, false) as ORMModelElement) != null;
		}

		public sealed override void ResetValue(object component)
		{
			ORMModelElement modelElement = EditorUtility.ResolveContextInstance(component, false) as ORMModelElement;
			System.Diagnostics.Debug.Assert(modelElement != null);
			CustomProperty customProperty = FindCustomProperty(modelElement);
			if (customProperty != null)
			{
				using (Transaction transaction = this.StartTransaction(modelElement))
				{
					customProperty.Delete();
					transaction.Commit();
				}
			}
		}

		public sealed override void SetValue(object component, object value)
		{
			CustomPropertyDefinition customPropertyDefinition = this.customPropertyDefinition;
			ORMModelElement modelElement = EditorUtility.ResolveContextInstance(component, false) as ORMModelElement;
			System.Diagnostics.Debug.Assert(modelElement != null);
			CustomProperty customProperty = FindCustomProperty(modelElement);
			using (Transaction transaction = this.StartTransaction(modelElement))
			{
				if (customProperty != null)
				{
					if (!object.Equals(customProperty.Value, value))
					{
						if (value == null || object.Equals(customPropertyDefinition.DefaultValue, value))
						{
							customProperty.Delete();
						}
						else
						{
							customProperty.Value = value;
						}
					}
				}
				else if (value != null && !object.Equals(customPropertyDefinition.DefaultValue, value))
				{
					customProperty = new CustomProperty(modelElement.Partition, new PropertyAssignment(CustomProperty.ValueDomainPropertyId, value));
					customProperty.CustomPropertyDefinition = customPropertyDefinition;
					modelElement.ExtensionCollection.Add(customProperty);
				}
				if (transaction.HasPendingChanges)
				{
					transaction.Commit();
				}
			}
		}
	}
}