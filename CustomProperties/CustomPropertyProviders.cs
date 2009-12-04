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
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	[VerbalizationSnippetsProvider("VerbalizationSnippets")]
	sealed partial class CustomPropertiesDomainModel : IModelingEventSubscriber
	{
		#region CustomPropertyProviders class
		private static class CustomPropertyProviders
		{
			private static void GetProvidedProperties(ORMTypes selectedTypes, object extendableElement, PropertyDescriptorCollection properties)
			{
				foreach (CustomPropertyDefinition customPropertyDefinition in ((ModelElement)extendableElement).Store.ElementDirectory.FindElements<CustomPropertyDefinition>())
				{
					if ((customPropertyDefinition.ORMTypes & selectedTypes) == 0)
					{
						continue;
					}
					properties.Add(CustomPropertyDescriptor.GetDescriptorForCustomPropertyDefinition(customPropertyDefinition));
				}
			}

			public static readonly PropertyProvider Model = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.Model, extendableElement, properties);
			};
			public static readonly PropertyProvider ObjectType = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(((ObjectType)extendableElement).IsValueType ? ORMTypes.ValueType : ORMTypes.EntityType, extendableElement, properties);
			};
			public static readonly PropertyProvider FactType = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.FactType, extendableElement, properties);
			};
			public static readonly PropertyProvider SubtypeFact = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.SubtypeFact, extendableElement, properties);
			};
			public static readonly PropertyProvider Role = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.Role, extendableElement, properties);
			};
			public static readonly PropertyProvider FrequencyConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.FrequencyConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider MandatoryConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.MandatoryConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider RingConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.RingConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider UniquenessConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.UniquenessConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider EqualityConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.EqualityConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider ExclusionConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.ExclusionConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider SubsetConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.SubsetConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider ValueConstraint = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.ValueConstraint, extendableElement, properties);
			};
			public static readonly PropertyProvider CustomPropertiesEditor = delegate(object extendableElement, PropertyDescriptorCollection properties)
			{
				properties.Add(CustomPropertiesEditorPropertyDescriptor.Instance);
			};
		}
		#endregion // CustomPropertyProviders class
		#region DefaultVerbalizationProviders class
		/// <summary>
		/// Default verbalization shows verbalizations for custom property
		/// definitions that have a verbalized default value but no CustomProperty
		/// for the definition on a given context element.
		/// </summary>
		private static class DefaultVerbalizationProviders
		{
			#region PropertyDefinitionFilter delegate
			/// <summary>
			/// A delegate used to determine if a custom property definition applies to the context object.
			/// </summary>
			/// <param name="propertyOwner">The property owner</param>
			/// <param name="propertyDefinition">A custom property</param>
			/// <returns>true to allow process, false to block.</returns>
			private delegate bool PropertyDefinitionFilter(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition);
			#endregion // PropertyDefinitionFilter delegate
			#region DefaultCustomPropertyVerbalizer class
			/// <summary>
			/// A verbalization implementation to verbalizer default values for custom property definitions
			/// that do not have a defined CustomProperty instance.
			/// </summary>
			private sealed class DefaultCustomPropertyVerbalizer : IVerbalizeExtensionChildren
			{
				#region DefaultPropertyValueVerbalizer class
				private sealed class DefaultPropertyValueVerbalizer : IVerbalize, IDisposable
				{
					#region Cache management
					// Cache an instance so we only create one helper in single-threaded scenarios
					private static DefaultPropertyValueVerbalizer myCache;
					public static DefaultPropertyValueVerbalizer GetVerbalizer()
					{
						DefaultPropertyValueVerbalizer retVal = myCache;
						if (retVal != null)
						{
							retVal = System.Threading.Interlocked.CompareExchange<DefaultPropertyValueVerbalizer>(ref myCache, null as DefaultPropertyValueVerbalizer, retVal);
						}
						if (retVal == null)
						{
							retVal = new DefaultPropertyValueVerbalizer();
						}
						return retVal;
					}
					private CustomPropertyDefinition myDefinition;
					/// <summary>
					/// Attach the property to verbalize to this instance
					/// </summary>
					/// <param name="definition">A <see cref="CustomPropertyDefinition"/> with a default value.</param>
					public void Attach(CustomPropertyDefinition definition)
					{
						myDefinition = definition;
					}
					/// <summary>
					/// Called by the verbalization engine when verbalization is completed.
					/// Return this instance to the cache so that it can be reused.
					/// </summary>
					void IDisposable.Dispose()
					{
						myDefinition = null;
						if (myCache == null)
						{
							System.Threading.Interlocked.CompareExchange<DefaultPropertyValueVerbalizer>(ref myCache, this, null as DefaultPropertyValueVerbalizer);
						}
					}
					#endregion // Cache management
					#region IVerbalize Implementation
					bool IVerbalize.GetVerbalization(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
					{
						CustomPropertyDefinition propertyDefinition = myDefinition;
						object defaultValue = propertyDefinition.DefaultValue;
						verbalizationContext.BeginVerbalization(VerbalizationContent.Normal);
						writer.Write(string.Format(
							((IVerbalizationSets<CustomPropertyVerbalizationSnippetType>)snippetsDictionary[typeof(CustomPropertyVerbalizationSnippetType)]).GetSnippet(CustomPropertyVerbalizationSnippetType.CustomPropertiesVerbalization),
							propertyDefinition.Name,
							propertyDefinition.DefaultValue.ToString(),
							propertyDefinition.CustomPropertyGroup.Name,
							propertyDefinition.Description));
						return true;
					}
					#endregion // IVerbalize Implementation
				}
				#endregion // DefaultPropertyValueVerbalizer class
				#region Constructor
				private PropertyDefinitionFilter myPropertyFilter;
				/// <summary>
				/// Create a child verbalizer with the specified delegate filter.
				/// </summary>
				/// <param name="filter">A filter used to determine which properties to verbalize</param>
				public DefaultCustomPropertyVerbalizer(PropertyDefinitionFilter filter)
				{
					myPropertyFilter = filter;
				}
				#endregion // Constructor
				#region IVerbalizeExtensionChildren Implementation
				IEnumerable<CustomChildVerbalizer> IVerbalizeExtensionChildren.GetExtensionChildVerbalizations(object parentElement, IVerbalizeFilterChildren filter, VerbalizationSign sign)
				{
					// Get the instance data local and release the cached iterator
					IORMExtendableElement propertyOwner = parentElement as IORMExtendableElement;
					if (propertyOwner != null)
					{
						PropertyDefinitionFilter propertyFilter = myPropertyFilter;
						LinkedElementCollection<ModelElement> extensions = null;
						foreach (CustomPropertyDefinition customPropertyDefinition in propertyOwner.Store.ElementDirectory.FindElements<CustomPropertyDefinition>())
						{
							object defaultValue;
							if (!customPropertyDefinition.VerbalizeDefaultValue ||
								null == (defaultValue = customPropertyDefinition.DefaultValue) ||
								(propertyFilter != null && !propertyFilter(propertyOwner, customPropertyDefinition)))
							{
								continue;
							}
							bool haveMatchingProperty = false;
							foreach (ModelElement extensionElement in extensions ?? (extensions = propertyOwner.ExtensionCollection))
							{
								CustomProperty customProperty = extensionElement as CustomProperty;
								if (customProperty != null && customProperty.CustomPropertyDefinition == customPropertyDefinition)
								{
									haveMatchingProperty = true;
									break;
								}
							}
							if (!haveMatchingProperty)
							{
								DefaultPropertyValueVerbalizer verbalizer = DefaultPropertyValueVerbalizer.GetVerbalizer();
								verbalizer.Attach(customPropertyDefinition);
								yield return CustomChildVerbalizer.VerbalizeInstance(verbalizer, true);
							}
						}
					}
				}
				#endregion // IVerbalizeExtensionChildren Implementation
			}
			#endregion // DefaultCustomPropertyVerbalizer class
			#region Specific verbalizers for each element type
			public static readonly IVerbalizeExtensionChildren Model = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.Model);
			});
			public static readonly IVerbalizeExtensionChildren ObjectType = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & (((ObjectType)propertyOwner).IsValueType ? ORMTypes.ValueType : ORMTypes.EntityType));
			});
			public static readonly IVerbalizeExtensionChildren SubtypeFact = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.SubtypeFact);
			});
			public static readonly IVerbalizeExtensionChildren FactType = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.FactType);
			});
			public static readonly IVerbalizeExtensionChildren Role = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.Role);
			});
			public static readonly IVerbalizeExtensionChildren FrequencyConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.FrequencyConstraint);
			});
			public static readonly IVerbalizeExtensionChildren MandatoryConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.MandatoryConstraint);
			});
			public static readonly IVerbalizeExtensionChildren RingConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.RingConstraint);
			});
			public static readonly IVerbalizeExtensionChildren UniquenessConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.UniquenessConstraint);
			});
			public static readonly IVerbalizeExtensionChildren EqualityConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.EqualityConstraint);
			});
			public static readonly IVerbalizeExtensionChildren ExclusionConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.ExclusionConstraint);
			});
			public static readonly IVerbalizeExtensionChildren SubsetConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.SubsetConstraint);
			});
			public static readonly IVerbalizeExtensionChildren ValueConstraint = new DefaultCustomPropertyVerbalizer(delegate(IORMExtendableElement propertyOwner, CustomPropertyDefinition propertyDefinition)
			{
				return 0 != (propertyDefinition.ORMTypes & ORMTypes.ValueConstraint);
			});
			#endregion // Specific verbalizers for each element type
		}
		#endregion // DefaultVerbalizationProviders class
		#region IModelingEventSubscriber Implementation
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if (0 != (reasons & EventSubscriberReasons.DocumentLoaded))
			{
				Store store = Store;
				IPropertyProviderService propertyService = ((IFrameworkServices)store).PropertyProviderService;
				IExtensionVerbalizerService verbalizerService = ((IORMToolServices)store).ExtensionVerbalizerService;
				propertyService.AddOrRemovePropertyProvider(typeof(ORMModel), CustomPropertyProviders.CustomPropertiesEditor, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(ORMModel), CustomPropertyProviders.Model, false, action);
				propertyService.AddOrRemovePropertyProvider(typeof(ObjectType), CustomPropertyProviders.ObjectType, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(SubtypeFact), CustomPropertyProviders.SubtypeFact, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(FactType), CustomPropertyProviders.FactType, false, action);
				propertyService.AddOrRemovePropertyProvider(typeof(Role), CustomPropertyProviders.Role, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(FrequencyConstraint), CustomPropertyProviders.FrequencyConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(MandatoryConstraint), CustomPropertyProviders.MandatoryConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(RingConstraint), CustomPropertyProviders.RingConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(UniquenessConstraint), CustomPropertyProviders.UniquenessConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(EqualityConstraint), CustomPropertyProviders.EqualityConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(ExclusionConstraint), CustomPropertyProviders.ExclusionConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(SubsetConstraint), CustomPropertyProviders.SubsetConstraint, true, action);
				propertyService.AddOrRemovePropertyProvider(typeof(ValueConstraint), CustomPropertyProviders.ValueConstraint, true, action);
				if (verbalizerService != null)
				{
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(ORMModel), DefaultVerbalizationProviders.Model, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(ObjectType), DefaultVerbalizationProviders.ObjectType, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(SubtypeFact), DefaultVerbalizationProviders.SubtypeFact, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(FactType), DefaultVerbalizationProviders.FactType, false, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(Role), DefaultVerbalizationProviders.Role, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(FrequencyConstraint), DefaultVerbalizationProviders.FrequencyConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(MandatoryConstraint), DefaultVerbalizationProviders.MandatoryConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(RingConstraint), DefaultVerbalizationProviders.RingConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(UniquenessConstraint), DefaultVerbalizationProviders.UniquenessConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(EqualityConstraint), DefaultVerbalizationProviders.EqualityConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(ExclusionConstraint), DefaultVerbalizationProviders.ExclusionConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(SubsetConstraint), DefaultVerbalizationProviders.SubsetConstraint, true, action);
					verbalizerService.AddOrRemoveExtensionVerbalizer(typeof(ValueConstraint), DefaultVerbalizationProviders.ValueConstraint, true, action);
				}
			}
		}
		#endregion // IModelingEventSubscriber Implementation
		#region IVerbalizationSnippetsProvider Implementation
		private class VerbalizationSnippets : IVerbalizationSnippetsProvider
		{
			/// <summary>
			/// IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets
			/// </summary>
			VerbalizationSnippetsData[] IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets()
			{
				return new VerbalizationSnippetsData[]
				{
					new VerbalizationSnippetsData(
						typeof(CustomPropertyVerbalizationSnippetType),
						CustomPropertyVerbalizationSets.Default,
						"CustomProperties",
						ResourceStrings.CustomPropertiesSnippetsTypeDescription,
						ResourceStrings.CustomPropertiesSnippetsDefaultDescription
					),
				};
			}
		}
		#endregion // IVerbalizationSnippetsProvider Implementation
		#region CustomPropertiesEditorPropertyDescriptor class
		private sealed class CustomPropertiesEditorPropertyDescriptor : PropertyDescriptor
		{
			private sealed class CustomPropertiesEditorUITypeEditor : UITypeEditor
			{
				public static readonly CustomPropertiesEditorUITypeEditor Instance = new CustomPropertiesEditorUITypeEditor();
				private CustomPropertiesEditorUITypeEditor()
					: base()
				{
				}

				public sealed override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
				{
					return UITypeEditorEditStyle.Modal;
				}
				public sealed override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
				{
					if (context != null)
					{
						ModelElement element = EditorUtility.ResolveContextInstance(context.Instance, false) as ModelElement;
						if (element != null)
						{
							CustomPropertiesManager.ShowCustomGroups(element.Store, provider);
						}
					}
					return value;
				}
			}

			public static readonly CustomPropertiesEditorPropertyDescriptor Instance = new CustomPropertiesEditorPropertyDescriptor();
			private CustomPropertiesEditorPropertyDescriptor()
				: base("CustomPropertiesEditor", null)
			{
			}

			public sealed override bool CanResetValue(object component)
			{
				return false;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.PropertiesEditorDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.PropertiesEditorCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.PropertiesEditorDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return null;
			}
			public sealed override object GetEditor(Type editorBaseType)
			{
				if (editorBaseType == typeof(UITypeEditor))
				{
					return CustomPropertiesEditorUITypeEditor.Instance;
				}
				else
				{
					return base.GetEditor(editorBaseType);
				}
			}

			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ORMModel);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(object);
				}
			}
			public sealed override void ResetValue(object component)
			{
				// Do nothing.
			}
			public sealed override void SetValue(object component, object value)
			{
				// Do nothing.
			}
		}
		#endregion // CustomPropertiesEditorPropertyDescriptor class
	}
}
