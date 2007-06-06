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
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.CustomProperties
{
	[VerbalizationSnippetsProvider("VerbalizationSnippets")]
	sealed partial class CustomPropertiesDomainModel : IORMModelEventSubscriber
	{
		#region CustomPropertyProviders class
		private static class CustomPropertyProviders
		{
			private static void GetProvidedProperties(ORMTypes selectedTypes, IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				foreach (CustomPropertyDefinition customPropertyDefinition in extendableElement.Store.ElementDirectory.FindElements<CustomPropertyDefinition>())
				{
					if ((customPropertyDefinition.ORMTypes & selectedTypes) == 0)
					{
						continue;
					}
					properties.Add(CustomPropertyDescriptor.GetDescriptorForCustomPropertyDefinition(customPropertyDefinition));
				}
			}

			public static readonly ORMPropertyProvisioning ObjectType = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(((ObjectType)extendableElement).IsValueType ? ORMTypes.ValueType : ORMTypes.EntityType, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning FactType = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.FactType, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning SubtypeFact = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.SubtypeFact, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning Role = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.Role, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning FrequencyConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.FrequencyConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning MandatoryConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.MandatoryConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning RingConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.RingConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning UniquenessConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.UniquenessConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning EqualityConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.EqualityConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning ExclusionConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.ExclusionConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning SubsetConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.SubsetConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning ValueConstraint = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				GetProvidedProperties(ORMTypes.ValueConstraint, extendableElement, properties);
			};
			public static readonly ORMPropertyProvisioning CustomPropertiesEditor = delegate(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
			{
				properties.Add(CustomPropertiesEditorPropertyDescriptor.Instance);
			};
		}
		#endregion // CustomPropertyProviders class

		#region IORMModelEventSubscriber Members
		void IORMModelEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			IORMPropertyProviderService propertyProvisioningService = ((IORMToolServices)this.Store).PropertyProviderService;
			if (action == EventHandlerAction.Add)
			{
				propertyProvisioningService.RegisterPropertyProvider<ORMModel>(CustomPropertyProviders.CustomPropertiesEditor, true);
				propertyProvisioningService.RegisterPropertyProvider<ObjectType>(CustomPropertyProviders.ObjectType, true);
				propertyProvisioningService.RegisterPropertyProvider<SubtypeFact>(CustomPropertyProviders.SubtypeFact, true);
				propertyProvisioningService.RegisterPropertyProvider<FactType>(CustomPropertyProviders.FactType, false);
				propertyProvisioningService.RegisterPropertyProvider<Role>(CustomPropertyProviders.Role, true);
				propertyProvisioningService.RegisterPropertyProvider<FrequencyConstraint>(CustomPropertyProviders.FrequencyConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<MandatoryConstraint>(CustomPropertyProviders.MandatoryConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<RingConstraint>(CustomPropertyProviders.RingConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<UniquenessConstraint>(CustomPropertyProviders.UniquenessConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<EqualityConstraint>(CustomPropertyProviders.EqualityConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<ExclusionConstraint>(CustomPropertyProviders.ExclusionConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<SubsetConstraint>(CustomPropertyProviders.SubsetConstraint, true);
				propertyProvisioningService.RegisterPropertyProvider<ValueConstraint>(CustomPropertyProviders.ValueConstraint, true);
			}
			else
			{
				propertyProvisioningService.UnregisterPropertyProvider<ORMModel>(CustomPropertyProviders.CustomPropertiesEditor, true);
				propertyProvisioningService.UnregisterPropertyProvider<ObjectType>(CustomPropertyProviders.ObjectType, true);
				propertyProvisioningService.UnregisterPropertyProvider<SubtypeFact>(CustomPropertyProviders.SubtypeFact, true);
				propertyProvisioningService.UnregisterPropertyProvider<FactType>(CustomPropertyProviders.FactType, false);
				propertyProvisioningService.UnregisterPropertyProvider<Role>(CustomPropertyProviders.Role, true);
				propertyProvisioningService.UnregisterPropertyProvider<FrequencyConstraint>(CustomPropertyProviders.FrequencyConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<MandatoryConstraint>(CustomPropertyProviders.MandatoryConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<RingConstraint>(CustomPropertyProviders.RingConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<UniquenessConstraint>(CustomPropertyProviders.UniquenessConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<EqualityConstraint>(CustomPropertyProviders.EqualityConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<ExclusionConstraint>(CustomPropertyProviders.ExclusionConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<SubsetConstraint>(CustomPropertyProviders.SubsetConstraint, true);
				propertyProvisioningService.UnregisterPropertyProvider<ValueConstraint>(CustomPropertyProviders.ValueConstraint, true);
			}
		}
		void IORMModelEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Do nothing
		}
		void IORMModelEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Do nothing
		}
		#endregion

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
					return CustomPropertyProvidersResources.DisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return CustomPropertyProvidersResources.Category;
				}
			}
			public sealed override string Description
			{
				get
				{
					return CustomPropertyProvidersResources.Description;
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
	}
}
