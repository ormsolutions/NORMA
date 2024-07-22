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
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Collections;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// Create a property descriptor that can wrap stock <see cref="ObjectType.DefaultValue"/>
	/// or <see cref="Role.DefaultValue"/> property descriptors.
	/// </summary>
	public sealed class DefaultValuePropertyDescriptor : ElementPropertyDescriptor
	{
		private static object DefaultInstance = new object();
		private static object NoInstance = new object();
		#region DefaultValueConverter class
		[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
		private sealed class DefaultValueConverter : TypeConverter
		{
			public static TypeConverter Instance = new DefaultValueConverter();
			private DefaultValueConverter() { }
			/// <summary>
			/// Standard override. Allow string conversion.
			/// </summary>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}
			/// <summary>
			/// Standard override. Map non-positive values to 0, meaning unbounded.
			/// </summary>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string stringValue = value == null ? string.Empty : value.ToString();
				if (stringValue == ResourceStrings.DefaultValuePickerNoDefaultText)
				{
					return NoInstance;
				}

				// Leave the default instance alone. Generally, the user will not edit this, so we will not be called here and nothing will change.
				// If the user really wants to set the current value the same as the default, the easiest way is to first do 'no default value', then type in the same value.
				return value;
			}
			/// <summary>
			/// Standard override. Convert the NoInstance value to text.
			/// </summary>
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					if (object.ReferenceEquals(value, NoInstance))
					{
						return ResourceStrings.DefaultValuePickerNoDefaultText;
					}
					else if (object.ReferenceEquals(value, DefaultInstance))
					{
						// Note that this is not a possible code path if we're targeting a value type
						ModelElement element = EditorUtility.ResolveContextInstance(context.Instance, false) as ModelElement;
						if (element != null)
						{
							ObjectType objectType;
							Role role = null;
							if (null != (objectType = element as ObjectType))
							{
								if (objectType.HasReferenceMode) // Sanity check, we should never have a default value for a value type
								{
									role = objectType.PreferredIdentifier.RoleCollection[0];
								}
							}
							else if (null != (role = element as Role))
							{
								role = element as Role;
							}

							if (role != null && null != (objectType = role.RolePlayer))
							{
								return objectType.ResolvedDefaultValue ?? ResourceStrings.DefaultValuePickerNoDefaultText;
							}
						}
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
		#endregion // DefaultValueConverter class
		#region DefaultValueEditor class
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		private sealed class DefaultValueEditor : ElementPicker<DefaultValueEditor>
		{
			private Role myRole;
			private ObjectType myValueType;
			private bool myFirstIsDirectValue;
			private string myContextDefaultValueText;
			private string myNoInstanceText;
			public DefaultValueEditor(Role role)
			{
				myRole = role;

				// Note that these resources include a non-breaking space to make it ridiculously unlikely that
				// anyone would want to enter this text as an actual default value.
				myNoInstanceText = ResourceStrings.DefaultValuePickerNoDefaultText;
				ObjectType rolePlayer = role.RolePlayer;
				string contextDefault = rolePlayer != null ? rolePlayer.ResolvedDefaultValue : null;
				if (contextDefault != null)
				{
					myContextDefaultValueText = string.Format(CultureInfo.CurrentCulture, ResourceStrings.DefaultValuePickerContextDefaultFormat, contextDefault);
				}
			}
			public DefaultValueEditor(ObjectType valueType)
			{
				myValueType = valueType;
				myNoInstanceText = ResourceStrings.DefaultValuePickerNoDefaultText;
			}
			/// <summary>
			/// Returns the Unbounded value, as well as the current value if it is not unbounded
			/// </summary>
			protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
			{
				// The content list consists of between 1 and 3 items.
				// If this is for a value type, then there either is or is not an
				// instance. So, if the ResolvedDirectDefaultValue is null then there is
				// one value <No Default>, with the default value (possibly empty).
				ObjectType valueType = myValueType;
				string currentDefault;
				if (valueType != null)
				{
					currentDefault = valueType.ResolvedDirectDefaultValue;
					if (currentDefault != null)
					{
						myFirstIsDirectValue = true;
						return new string[] { currentDefault, myNoInstanceText };
					}
				}
				else
				{
					Role role = myRole;
					currentDefault = role.ResolvedDirectDefaultValue;
					string contextDefaultText = myContextDefaultValueText;

					if (currentDefault != null)
					{
						myFirstIsDirectValue = true;
						return contextDefaultText != null ?
							new string[] { currentDefault, myNoInstanceText, contextDefaultText } :
							new string[] { currentDefault, myNoInstanceText };
					}
					else if (contextDefaultText != null)
					{
						return new string[] { myNoInstanceText, contextDefaultText };
					}
				}

				return new string[] { myNoInstanceText };
			}
			/// <summary>
			/// Translate a displayed value back to what we want to return as the real value.
			/// </summary>
			protected override object TranslateFromDisplayObject(int newIndex, object newObject)
			{
				switch (newIndex)
				{
					case 0:
						return myFirstIsDirectValue ? newObject : NoInstance;
					case 1:
						return myFirstIsDirectValue ? NoInstance : DefaultInstance;
					//case 2:
					default:
						return DefaultInstance;
				}
			}
			/// <summary>
			/// Get the display object for the current value
			/// </summary>
			protected override object TranslateToDisplayObject(object initialObject, IList contentList)
			{
				if (object.ReferenceEquals(initialObject, NoInstance))
				{
					return myNoInstanceText;
				}
				else if (object.ReferenceEquals(initialObject, DefaultInstance))
				{
					return myContextDefaultValueText;
				}
				return initialObject; // This will be the specified default value.
			}
		}
		#endregion // DefaultValueEditor class
		/// <summary>
		/// The resolved role. This can be passed in directly as the ModelElement,
		/// or deduced from an entity type with a reference scheme.
		/// </summary>
		private Role myRole;
		/// <summary>
		/// Create a new property descriptor. Parameters are forwarded to the base.
		/// </summary>
		public DefaultValuePropertyDescriptor(ElementTypeDescriptor owner, ModelElement modelElement, DomainPropertyInfo domainProperty, Attribute[] attributes, bool resolveValueType)
			: base(owner, ResolveValueType(modelElement, resolveValueType), domainProperty, attributes)
		{
			ObjectType objectType = modelElement as ObjectType;
			if (objectType != null)
			{
				if (!resolveValueType)
				{
					if (objectType.DataType == null)
					{
						// objectType.HasReferenceMode must be true to create this property descriptor for an object type.
						// This pattern is always available if HasReferenceMode is true.
						myRole = objectType.PreferredIdentifier.RoleCollection[0];
					}
				}
			}
			else
			{
				myRole = (Role)modelElement;
			}
		}
		/// <summary>
		/// Static helper to analyze the provided modelElement before passing it to the base constructor.
		/// </summary>
		private static ModelElement ResolveValueType(ModelElement modelElement, bool resolveValueType)
		{
			if (resolveValueType)
			{
				ObjectType objectType;
				if (null != (objectType = modelElement as ObjectType) &&
					null != (objectType = objectType.EntityTypeIdentifyingValueType))
				{
					return objectType;
				}
			}
			return modelElement;
		}
		/// <summary>
		/// Get the property value. This may return internal values used to communicate different
		/// states and should not be relied on for external use.
		/// </summary>
		public override object GetValue(object component)
		{
			Role role = myRole;
			if (role != null)
			{
				switch (role.DefaultState)
				{
					case DefaultValueState.EmptyValue:
						return string.Empty;
					case DefaultValueState.IgnoreContext:
						return NoInstance;
					//case DefaultValueState.UseValue:
					default:
						{
							string currentDefault = role.DefaultValue;
							if (!string.IsNullOrEmpty(currentDefault))
							{
								return currentDefault;
							}

							ObjectType rolePlayer;
							if (null != (rolePlayer = role.RolePlayer) && null != rolePlayer.ResolvedDefaultValue)
							{
								return DefaultInstance;
							}
							return NoInstance;
						}
				}
			}
			return ((ObjectType)ModelElement).ResolvedDirectDefaultValue ?? NoInstance;
		}
		/// <summary>
		/// Set a new default value. This uses internal values to accurately communicate default and empty states.
		/// </summary>
		public override void SetValue(object component, object value)
		{
			ModelElement element = this.ModelElement;
			Store store;
			if (null != (element = ModelElement) && null != (store = Utility.ValidateStore(element.Store)))
			{
				using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(DisplayName)))
				{
					DataType dataType;
					ObjectType objectType;
					Role role;
					string stringValue;
					if ((role = myRole) != null)
					{
						bool clearDirectDefault = true;
						if (object.ReferenceEquals(value, NoInstance))
						{
							if (null != (objectType = role.RolePlayer) && objectType.ResolvedDefaultValue != null)
							{
								role.DefaultState = DefaultValueState.IgnoreContext;
								clearDirectDefault = false;
							}
						}
						else if (!object.ReferenceEquals(value, DefaultInstance) && null != (stringValue = value as string))
						{
							if (stringValue.Length != 0)
							{
								role.DefaultValue = stringValue;
								clearDirectDefault = false;
							}
							else if (null != (objectType = role.ValueRoleValueType) && null != (dataType = objectType.DataType) && dataType.CanParseAnyValue)
							{
								role.DefaultState = DefaultValueState.EmptyValue;
								clearDirectDefault = false;
							}
						}

						if (clearDirectDefault)
						{
							role.DefaultState = DefaultValueState.UseValue;
							role.DefaultValue = string.Empty;
						}
					}
					else
					{
						objectType = (ObjectType)ModelElement;
						bool clearDirectDefault = true;
						if (null != (dataType = objectType.DataType))
						{
							if (!object.ReferenceEquals(value, NoInstance) && null != (stringValue = value as string))
							{
								if (stringValue.Length != 0)
								{
									objectType.DefaultValue = stringValue;
									clearDirectDefault = false;
								}
								else if (dataType.CanParseAnyValue)
								{
									objectType.DefaultState = DefaultValueState.EmptyValue; // A rule clears the default value if previously set
									clearDirectDefault = false;
								}
							}
						}

						if (clearDirectDefault)
						{
							objectType.DefaultState = DefaultValueState.UseValue;
							objectType.DefaultValue = string.Empty;
						}
					}

					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Reset the default value for the context element.
		/// </summary>
		public override void ResetValue(object component)
		{
			ModelElement element;
			Store store;
			if (null != (element = ModelElement) && null != (store = Utility.ValidateStore(element.Store)))
			{
				using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(DisplayName)))
				{
					Role role = myRole;
					if (role != null)
					{
						role.DefaultState = DefaultValueState.UseValue;
						role.DefaultValue = string.Empty;
					}
					else
					{
						ObjectType objectType = (ObjectType)element;
						objectType.DefaultState = DefaultValueState.UseValue;
						objectType.DefaultValue = string.Empty;
					}

					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Determine if the default value has a non-default state.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			Role role = myRole;
			if (role != null)
			{
				return role.DefaultState != DefaultValueState.UseValue || (role.DefaultValue ?? string.Empty).Length != 0;
			}
			else
			{
				ObjectType objectType = (ObjectType)ModelElement;
				return objectType.DefaultState != DefaultValueState.UseValue || (objectType.DefaultValue ?? string.Empty).Length != 0;
			}
		}
		/// <summary>
		/// Get a dropdown editor to use with this property.
		/// </summary>
		public override object GetEditor(Type editorBaseType)
		{
			if (editorBaseType == typeof(System.Drawing.Design.UITypeEditor))
			{
				Role role = myRole;
				return role != null ? new DefaultValueEditor(role) : new DefaultValueEditor((ObjectType)ModelElement);
			}
			return base.GetEditor(editorBaseType);
		}
		/// <summary>
		/// Get a type converter to manager property text.
		/// </summary>
		public override TypeConverter Converter
		{
			get
			{
				return DefaultValueConverter.Instance;
			}
		}
	}
}
