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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORM.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using Neumont.Tools.Modeling.Design;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	partial class ReferenceModeNaming
	{
		#region Extension property provider callbacks
		/// <summary>
		/// An <see cref="ORMPropertyProvisioning"/> callback for adding extender properties to an <see cref="ObjectType"/>
		/// </summary>
		public static void PopulateReferenceModeNamingExtensionProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
		{
			ObjectType objectType;
			ReferenceMode referenceMode;
			ReferenceModeType referenceModeType;
			if (null != (objectType = extendableElement as ObjectType) &&
				!objectType.IsDeleted &&
				null != (referenceMode = objectType.ReferenceMode) &&
				ReferenceModeType.General != (referenceModeType = referenceMode.Kind.ReferenceModeType))
			{
				properties.Add(ReferenceModeNamingPropertyDescriptor.Instance);
				ReferenceModeNamingChoice currentChoice = GetNamingChoiceFromObjectType(objectType);
				if (currentChoice == ReferenceModeNamingChoice.CustomFormat ||
					(currentChoice == ReferenceModeNamingChoice.ModelDefault && GetNamingChoiceFromORMModel(objectType.Model, referenceModeType) == EffectiveReferenceModeNamingChoice.CustomFormat))
				{
					properties.Add(ReferenceModeNamingCustomFormatPropertyDescriptor.Instance);
				}
			}
		}
		/// <summary>
		/// An <see cref="ORMPropertyProvisioning"/> callback for adding extender properties to an <see cref="ORMModel"/>
		/// </summary>
		public static void PopulateDefaultReferenceModeNamingExtensionProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties)
		{
			ORMModel model;
			if (null != (model = extendableElement as ORMModel) &&
				!model.IsDeleted)
			{
				properties.Add(PopularDefaultReferenceModeNamingPropertyDescriptor.Instance);
				properties.Add(PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor.Instance);
				properties.Add(UnitBasedDefaultReferenceModeNamingPropertyDescriptor.Instance);
				properties.Add(UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor.Instance);
			}
		}
		#endregion // Extension property provider callbacks
		#region ReferenceModeNamingPropertyDescriptor class
		private sealed class ReferenceModeNamingPropertyDescriptor : PropertyDescriptor
		{
			#region ReferenceModeNameEnumConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			public class ReferenceModeNamingEnumConverter : EnumConverter<ReferenceModeNamingChoice, MappingCustomizationModel>
			{
				public static readonly ReferenceModeNamingEnumConverter Instance = new ReferenceModeNamingEnumConverter();

				// Note that the values are explicitly ordered  and match the values listed in the ReferenceModeNamingEditor
				private static readonly StandardValuesCollection OrderedValues = new StandardValuesCollection(new object[] { ReferenceModeNamingChoice.ModelDefault, ReferenceModeNamingChoice.EntityTypeName, ReferenceModeNamingChoice.ReferenceModeName, ReferenceModeNamingChoice.ValueTypeName, ReferenceModeNamingChoice.CustomFormat});
				private ReferenceModeNamingEnumConverter()
				{
				}
				/// <summary>
				/// List the items in a consistent order
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return OrderedValues;
				}
			}
			#endregion // ReferenceModeNamingEnumConverter
			#region ReferenceModeNamingEditor class
			private sealed class ReferenceModeNamingEditor : ElementPicker<ReferenceModeNamingEditor>
			{
				public static readonly ReferenceModeNamingEditor Instance = new ReferenceModeNamingEditor();
				private ReferenceModeNamingEditor() { }
				protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
				{
					switch (newIndex)
					{
						case 1:
							return ReferenceModeNamingChoice.EntityTypeName;
						case 2:
							return ReferenceModeNamingChoice.ReferenceModeName;
						case 3:
							return ReferenceModeNamingChoice.ValueTypeName;
						case 4:
							return ReferenceModeNamingChoice.CustomFormat;
						// case 0:
						default:
							return ReferenceModeNamingChoice.ModelDefault;
					}
				}
				protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
				{
					return contentList[NamingChoiceToIndex((ReferenceModeNamingChoice)initialObject)];
				}
				private static int NamingChoiceToIndex(ReferenceModeNamingChoice namingChoice)
				{
					switch (namingChoice)
					{
						case ReferenceModeNamingChoice.ValueTypeName:
							return 3;
						case ReferenceModeNamingChoice.EntityTypeName:
							return 1;
						case ReferenceModeNamingChoice.ReferenceModeName:
							return 2;
						case ReferenceModeNamingChoice.CustomFormat:
							return 4;
						//case ReferenceModeNamingChoice.ModelDefault:
						default:
							return 0;
					}
				}
				protected sealed override IList GetContentList(ITypeDescriptorContext context, object value)
				{
					ObjectType objectType;
					ReferenceMode referenceMode;
					ReferenceModeType referenceModeType;
					if (null != (objectType = EditorUtility.ResolveContextInstance(context.Instance, true) as ObjectType) &&
						null != (referenceMode = objectType.ReferenceMode) &&
						ReferenceModeType.General != (referenceModeType = referenceMode.Kind.ReferenceModeType))
					{
						string currentModelDefault = null;
						switch (GetNamingChoiceFromORMModel(objectType.Model, referenceModeType))
						{
							case EffectiveReferenceModeNamingChoice.ValueTypeName:
								currentModelDefault = ResourceStrings.ReferenceModeNamingCurrentModelDefaultValueTypeName;
								break;
							case EffectiveReferenceModeNamingChoice.ReferenceModeName:
								currentModelDefault = ResourceStrings.ReferenceModeNamingCurrentModelDefaultReferenceModeName;
								break;
							case EffectiveReferenceModeNamingChoice.EntityTypeName:
								currentModelDefault = ResourceStrings.ReferenceModeNamingCurrentModelDefaultEntityTypeName;
								break;
							case EffectiveReferenceModeNamingChoice.CustomFormat:
								currentModelDefault = ResourceStrings.ReferenceModeNamingCurrentModelDefaultCustomFormat;
								break;
						}
						CultureInfo culture = CultureInfo.CurrentCulture;
						return new string[]
						{
							currentModelDefault,
							string.Format(culture, ResourceStrings.ReferenceModeNamingCurrentFormatStringEntityTypeName, objectType.Name),
							string.Format(culture, ResourceStrings.ReferenceModeNamingCurrentFormatStringReferenceModeName, referenceMode.Name),
							string.Format(culture, ResourceStrings.ReferenceModeNamingCurrentFormatStringValueTypeName, objectType.PreferredIdentifier.RoleCollection[0].RolePlayer.Name),
							string.Format(culture, ResourceStrings.ReferenceModeNamingCurrentFormatStringCustomFormat, ResolveObjectTypeName(objectType, null, ReferenceModeNamingChoice.CustomFormat, null)),
						};
					}
					return null;
				}
			}
			#endregion // ReferenceModeNamingEditor class
			public static readonly ReferenceModeNamingPropertyDescriptor Instance = new ReferenceModeNamingPropertyDescriptor();
			private ReferenceModeNamingPropertyDescriptor()
				: base("ReferenceModeNamingPropertyDescriptor", null)
			{
			}
			public override object GetEditor(Type editorBaseType)
			{
				return (editorBaseType == typeof(UITypeEditor)) ? ReferenceModeNamingEditor.Instance : base.GetEditor(editorBaseType);
			}
			public override TypeConverter Converter
			{
				get
				{
					return ReferenceModeNamingEnumConverter.Instance;
				}
			}
			private static ObjectType GetObjectTypeFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ObjectType;
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the model browser
				return GetNamingChoiceFromObjectType(GetObjectTypeFromComponent(component)) != ReferenceModeNamingChoice.ModelDefault;
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingNamingChoicePropertyDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingNamingChoicePropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetNamingChoiceFromObjectType(GetObjectTypeFromComponent(component));
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ObjectType);
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
					return typeof(ReferenceModeNamingChoice);
				}	
			}
			public sealed override void ResetValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, ReferenceModeNamingChoice.ModelDefault);
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, (ReferenceModeNamingChoice)value);
				}
			}
			private static void SetValue(ObjectType objectType, ReferenceModeNamingChoice newChoice)
			{
				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ReferenceModeNamingNamingChoicePropertyTransactionName))
				{
					ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
					if (naming != null)
					{
						naming.NamingChoice = newChoice;
					}
					else if (newChoice != ReferenceModeNamingChoice.ModelDefault)
					{
						naming = new ReferenceModeNaming(store, new PropertyAssignment(ReferenceModeNaming.NamingChoiceDomainPropertyId, newChoice));
						naming.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
						naming.ObjectType = objectType;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // ReferenceModeNamingPropertyDescriptor class
		#region ReferenceModeNamingCustomFormatTypeConverter class
		private abstract class ReferenceModeNamingCustomFormatTypeConverter : StringConverter
		{
			private static Regex myParseRegex;
			private static Regex myCleanDanglingBracesRegex;
			private struct CustomFormatDisplayMapping
			{
				public CustomFormatDisplayMapping(string native, string display, string shortParse)
				{
					Native = native;
					Display = display;
					ShortParse = shortParse;
				}
				public string Native;
				public string Display;
				public string ShortParse;
			}
			private static CustomFormatDisplayMapping[] myDisplayMappings;
			protected ReferenceModeNamingCustomFormatTypeConverter()
			{
			}
			private static CustomFormatDisplayMapping[] DisplayMappings
			{
				get
				{
					CustomFormatDisplayMapping[] retVal = myDisplayMappings;
					if (retVal == null)
					{
						myDisplayMappings = retVal = new CustomFormatDisplayMapping[]
						{
							new CustomFormatDisplayMapping("{0}", ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldValueTypeName, ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldShortFormValueTypeName),
							new CustomFormatDisplayMapping("{1}", ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldEntityTypeName, ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldShortFormEntityTypeName),
							new CustomFormatDisplayMapping("{2}", ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldReferenceModeName, ResourceStrings.ReferenceModeNamingDisplayedReplacementFieldShortFormReferenceModeName),
						};
					}
					return retVal;
				}
			}
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == typeof(string))
				{
					CustomFormatDisplayMapping[] mappings = DisplayMappings;
					return string.Format(culture, value.ToString(), mappings[0].Display, mappings[1].Display, mappings[2].Display);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			private static string CleanDanglingBraces(string startString, Regex cleanDanglingBracesRegex)
			{
				return cleanDanglingBracesRegex.Replace(
					startString,
					delegate(Match match)
					{
						GroupCollection groups = match.Groups;
						Group group = groups["Open"];
						if (group.Success)
						{
							return "{{";
						}
						group = groups["Close"];
						if (group.Success)
						{
							return "}}";
						}
						return match.Value;
					});
			}
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				bool seenReplacementField;
				return TestConvertFrom(culture, value, out seenReplacementField);
			}
			protected object TestConvertFrom(CultureInfo culture, object value, out bool seenReplacementField)
			{
				#region Regex expressions
				Regex parseRegex = myParseRegex;
				if (parseRegex == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myParseRegex,
						new Regex(
							@"(?n)\G(?(?=((\{\{)|[^\{])*\{.+?\})((?<Before>((\{\{)|[^\{])*)(?<Replace>\{.+?\}))|(.*))",
							RegexOptions.Compiled),
						null);
					parseRegex = myParseRegex;
				}
				Regex cleanDanglingBracesRegex = myCleanDanglingBracesRegex;
				if (cleanDanglingBracesRegex == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myCleanDanglingBracesRegex,
						new Regex(
							@"(?n)\G(?<Open>\{)|(?<Close>\})|[^\{\}]*",
							RegexOptions.Compiled),
						null);
					cleanDanglingBracesRegex = myCleanDanglingBracesRegex;
				}
				#endregion // Regex expressions
				bool localSeenKnownToken = false;
				string retVal = parseRegex.Replace(
					(string)value,
					delegate(Match match)
					{
						GroupCollection groups = match.Groups;
						Group replaceGroup = groups["Replace"];
						if (replaceGroup.Success)
						{
							string knownToken = null;
							string replacementField = replaceGroup.Value;
							CustomFormatDisplayMapping[] mappings = DisplayMappings;
							for (int i = 0; i < mappings.Length; ++i)
							{
								CustomFormatDisplayMapping mapping = mappings[i];
								if (0 == string.Compare(replacementField, mapping.Display, true, culture) ||
									0 == string.Compare(replacementField, mapping.ShortParse, true, culture))
								{
									knownToken = mapping.Native;
									break;
								}
							}
							if (knownToken != null)
							{
								localSeenKnownToken = true;
								return CleanDanglingBraces(groups["Before"].Value, cleanDanglingBracesRegex) + knownToken;
							}
						}
						return CleanDanglingBraces(match.Value, cleanDanglingBracesRegex);
					});
				seenReplacementField = localSeenKnownToken;
				return retVal;
			}
		}
		#endregion // ReferenceModeNamingCustomFormatTypeConverter class
		#region ReferenceModeNamingCustomFormatPropertyDescriptor class
		private sealed class ReferenceModeNamingCustomFormatPropertyDescriptor : PropertyDescriptor
		{
			#region DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			private class DefaultAwareReferenceModeNamingCustomFormatTypeConverter : ReferenceModeNamingCustomFormatTypeConverter
			{
				public static readonly DefaultAwareReferenceModeNamingCustomFormatTypeConverter Instance = new DefaultAwareReferenceModeNamingCustomFormatTypeConverter();
				private DefaultAwareReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						string stringValue = value as string;
						if (string.IsNullOrEmpty(stringValue))
						{
							// Fill in the backing default value if no data is currently shown
							value = GetDefaultCustomFormatForObjectType(GetObjectTypeFromComponent(context.Instance));
						}
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					object newValue = base.ConvertFrom(context, culture, value);
					ObjectType objectType = GetObjectTypeFromComponent(context.Instance);
					string oldFormat = GetCustomFormatFromObjectType(objectType);
					if (string.IsNullOrEmpty(oldFormat) &&
						GetDefaultCustomFormatForObjectType(objectType) == newValue.ToString())
					{
						return "";
					}
					return newValue;
				}
			}
			#endregion // DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			public static readonly ReferenceModeNamingCustomFormatPropertyDescriptor Instance = new ReferenceModeNamingCustomFormatPropertyDescriptor();
			private ReferenceModeNamingCustomFormatPropertyDescriptor()
				: base("ReferenceModeNamingCustomFormatPropertyDescriptor", null)
			{
			}
			private static ObjectType GetObjectTypeFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ObjectType;
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				string objectFormat = GetCustomFormatFromObjectType(objectType);
				return !(objectFormat.Length == 0 || objectFormat == GetDefaultCustomFormatForObjectType(objectType));
			}
			public override TypeConverter Converter
			{
				get
				{
					return DefaultAwareReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingCustomFormatPropertyDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingCustomFormatPropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetCustomFormatFromObjectType(GetObjectTypeFromComponent(component));
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ObjectType);
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
					return typeof(string);
				}
			}
			public sealed override void ResetValue(object component)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, "");
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, (string)value);
				}
			}
			private static void SetValue(ObjectType objectType, string value)
			{
				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ReferenceModeNamingCustomFormatPropertyTransactionName))
				{
					ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
					if (naming != null)
					{
						naming.CustomFormat = value;
					}
					else if (!string.IsNullOrEmpty(value))
					{
						// The only way to not have one of these already is with a ModelDefault NamingChoice, which is the default, so we
						// just need to assign the CustomFormat property.
						naming = new ReferenceModeNaming(store, new PropertyAssignment(ReferenceModeNaming.CustomFormatDomainPropertyId, value));
						naming.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
						naming.ObjectType = objectType;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // ReferenceModeNamingCustomFormatPropertyDescriptor class
		#region DefaultReferenceModeNamingPropertyDescriptor class
		private sealed class UnitBasedDefaultReferenceModeNamingPropertyDescriptor : DefaultReferenceModeNamingPropertyDescriptor
		{
			public static readonly UnitBasedDefaultReferenceModeNamingPropertyDescriptor Instance = new UnitBasedDefaultReferenceModeNamingPropertyDescriptor();
			private UnitBasedDefaultReferenceModeNamingPropertyDescriptor()
				: base("UnitBasedDefaultReferenceModeNamingPropertyDescriptor")
			{
			}
			protected override DefaultReferenceModeNamingTargetKind TargetKind
			{
				get { return DefaultReferenceModeNamingTargetKind.UnitBased; }
			}
			protected override EffectiveReferenceModeNamingChoice DefaultNamingChoice
			{
				get { return EffectiveReferenceModeNamingChoice.EntityTypeName; }
			}
			protected override string DefaultCustomFormat
			{
				get { return ResourceStrings.ReferenceModeNamingDefaultCustomFormatUnitBased; }
			}
			protected override string TransactionName
			{
				get { return ResourceStrings.ReferenceModeNamingUnitBasedNamingChoicePropertyTransactionName; }
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingUnitBasedNamingChoicePropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingUnitBasedNamingChoicePropertyDescription;
				}
			}
		}
		private sealed class PopularDefaultReferenceModeNamingPropertyDescriptor : DefaultReferenceModeNamingPropertyDescriptor
		{
			public static readonly PopularDefaultReferenceModeNamingPropertyDescriptor Instance = new PopularDefaultReferenceModeNamingPropertyDescriptor();
			private PopularDefaultReferenceModeNamingPropertyDescriptor()
				: base("PopularDefaultReferenceModeNamingPropertyDescriptor")
			{
			}
			protected override DefaultReferenceModeNamingTargetKind TargetKind
			{
				get { return DefaultReferenceModeNamingTargetKind.Popular; }
			}
			protected override EffectiveReferenceModeNamingChoice DefaultNamingChoice
			{
				get { return EffectiveReferenceModeNamingChoice.ValueTypeName; }
			}
			protected override string DefaultCustomFormat
			{
				get { return ResourceStrings.ReferenceModeNamingDefaultCustomFormatPopular; }
			}
			protected override string TransactionName
			{
				get { return ResourceStrings.ReferenceModeNamingPopularNamingChoicePropertyTransactionName; }
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingPopularNamingChoicePropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingPopularNamingChoicePropertyDescription;
				}
			}
		}
		private abstract class DefaultReferenceModeNamingPropertyDescriptor : PropertyDescriptor
		{
			#region DefaultReferenceModeNamingEnumConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			public class DefaultReferenceModeNamingEnumConverter : EnumConverter<EffectiveReferenceModeNamingChoice, MappingCustomizationModel>
			{
				public static readonly DefaultReferenceModeNamingEnumConverter Instance = new DefaultReferenceModeNamingEnumConverter();

				// Note that the values are explicitly ordered  and match the values listed in the ReferenceModeNamingEditor
				private static readonly StandardValuesCollection OrderedValues = new StandardValuesCollection(new object[] { EffectiveReferenceModeNamingChoice.EntityTypeName, EffectiveReferenceModeNamingChoice.ReferenceModeName, EffectiveReferenceModeNamingChoice.ValueTypeName, EffectiveReferenceModeNamingChoice.CustomFormat });
				private DefaultReferenceModeNamingEnumConverter()
				{
				}
				/// <summary>
				/// List the items in a consistent order
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return OrderedValues;
				}
			}
			#endregion // DefaultReferenceModeNamingEnumConverter
			protected DefaultReferenceModeNamingPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract DefaultReferenceModeNamingTargetKind TargetKind { get;}
			/// <summary>
			/// Get the default naming choice for this type of object
			/// </summary>
			protected abstract EffectiveReferenceModeNamingChoice DefaultNamingChoice { get;}
			/// <summary>
			/// Get the default custom format for this type of object
			/// </summary>
			protected abstract string DefaultCustomFormat { get;}
			/// <summary>
			/// The string used for the name of the transaction to set this property
			/// </summary>
			protected abstract string TransactionName { get;}
			private static ORMModel GetORMModelFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ORMModel;
			}
			public override TypeConverter Converter
			{
				get
				{
					return DefaultReferenceModeNamingEnumConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the property grid
				return GetNamingChoiceFromORMModel(GetORMModelFromComponent(component), (ReferenceModeType)TargetKind) != DefaultNamingChoice;
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetNamingChoiceFromORMModel(GetORMModelFromComponent(component), (ReferenceModeType)TargetKind);
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
					return typeof(EffectiveReferenceModeNamingChoice);
				}
			}
			public sealed override void ResetValue(object component)
			{
				ORMModel model = GetORMModelFromComponent(component);
				if (model != null)
				{
					SetValue(model, DefaultNamingChoice);
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				ORMModel model = GetORMModelFromComponent(component);
				if (model != null)
				{
					SetValue(model, (EffectiveReferenceModeNamingChoice)value);
				}
			}
			private void SetValue(ORMModel model, EffectiveReferenceModeNamingChoice newChoice)
			{
				Store store = model.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(TransactionName))
				{
					DefaultReferenceModeNamingTargetKind testKind = TargetKind;
					DefaultReferenceModeNaming naming = null;
					foreach (DefaultReferenceModeNaming testNaming in DefaultReferenceModeNamingCustomizesORMModel.GetDefaultReferenceModeNamingCollection(model))
					{
						if (testNaming.ReferenceModeTargetKind == testKind)
						{
							naming = testNaming;
							break;
						}
					}
					if (naming != null)
					{
						naming.NamingChoice = newChoice;
					}
					else if (newChoice != DefaultNamingChoice)
					{
						naming = new DefaultReferenceModeNaming(
							store,
							new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, newChoice),
							new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, testKind),
							new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, DefaultCustomFormat));
						naming.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
						naming.ORMModel = model;
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // DefaultReferenceModeNamingPropertyDescriptor class
		#region ReferenceModeNamingCustomFormatPropertyDescriptor class
		private sealed class UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor : DefaultReferenceModeNamingCustomFormatPropertyDescriptor
		{
			public static readonly UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor Instance = new UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor();
			private UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor()
				: base("UnitBasedDefaultReferenceModeNamingCustomFormatPropertyDescriptor")
			{
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingUnitBasedCustomFormatPropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingUnitBasedCustomFormatPropertyDescription;
				}
			}
			protected override DefaultReferenceModeNamingTargetKind TargetKind
			{
				get { return DefaultReferenceModeNamingTargetKind.UnitBased; }
			}
			protected override EffectiveReferenceModeNamingChoice DefaultNamingChoice
			{
				get { return EffectiveReferenceModeNamingChoice.EntityTypeName; }
			}
			protected override string TransactionName
			{
				get { return ResourceStrings.ReferenceModeNamingUnitBasedCustomFormatPropertyTransactionName; }
			}
			protected override string DefaultCustomFormat
			{
				get { return ResourceStrings.ReferenceModeNamingDefaultCustomFormatUnitBased; }
			}
		}
		private sealed class PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor : DefaultReferenceModeNamingCustomFormatPropertyDescriptor
		{
			public static readonly PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor Instance = new PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor();
			private PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor()
				: base("PopularDefaultReferenceModeNamingCustomFormatPropertyDescriptor")
			{
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingPopularCustomFormatPropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingPopularCustomFormatPropertyDescription;
				}
			}
			protected override DefaultReferenceModeNamingTargetKind TargetKind
			{
				get { return DefaultReferenceModeNamingTargetKind.Popular; }
			}
			protected override EffectiveReferenceModeNamingChoice DefaultNamingChoice
			{
				get { return EffectiveReferenceModeNamingChoice.ValueTypeName; }
			}
			protected override string TransactionName
			{
				get { return ResourceStrings.ReferenceModeNamingPopularCustomFormatPropertyTransactionName; }
			}
			protected override string DefaultCustomFormat
			{
				get { return ResourceStrings.ReferenceModeNamingDefaultCustomFormatPopular; }
			}
		}
		private abstract class DefaultReferenceModeNamingCustomFormatPropertyDescriptor : PropertyDescriptor
		{
			#region ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter class
			private class ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter : ReferenceModeNamingCustomFormatTypeConverter
			{
				public static readonly ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter Instance = new ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter();
				private ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					bool seenReplacementField;
					object retVal = TestConvertFrom(culture, value, out seenReplacementField);
					if (!seenReplacementField)
					{
						throw new InvalidOperationException(ResourceStrings.ReferenceModeNamingDefaultCustomFormatInvalidDefaultCustomFormatException);
					}
					return retVal;
				}
			}
			#endregion // ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter class
			protected DefaultReferenceModeNamingCustomFormatPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract DefaultReferenceModeNamingTargetKind TargetKind { get;}
			/// <summary>
			/// Get the default naming choice for this type of object
			/// </summary>
			protected abstract EffectiveReferenceModeNamingChoice DefaultNamingChoice { get;}
			/// <summary>
			/// Get the default naming choice for this type of object
			/// </summary>
			protected abstract string DefaultCustomFormat { get;}
			/// <summary>
			/// The string used for the name of the transaction to set this property
			/// </summary>
			protected abstract string TransactionName { get;}
			private static ORMModel GetORMModelFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ORMModel;
			}
			public override TypeConverter Converter
			{
				get
				{
					return ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				// This controls bolding in the property grid
				return GetCustomFormatFromORMModel(GetORMModelFromComponent(component), (ReferenceModeType)TargetKind) != DefaultCustomFormat;
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetCustomFormatFromORMModel(GetORMModelFromComponent(component), (ReferenceModeType)TargetKind);
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
					return typeof(string);
				}
			}
			public sealed override void ResetValue(object component)
			{
				ORMModel model = GetORMModelFromComponent(component);
				if (model != null)
				{
					SetValue(model, DefaultCustomFormat);
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				ORMModel model = GetORMModelFromComponent(component);
				if (model != null)
				{
					SetValue(model, (string)value);
				}
			}
			private void SetValue(ORMModel model, string value)
			{
				Store store = model.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(TransactionName))
				{
					DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, (ReferenceModeType)TargetKind);
					if (naming != null)
					{
						naming.CustomFormat = value;
					}
					else if (!string.IsNullOrEmpty(value))
					{
						// The only way to not have one of these already is with a default NamingChoice. However, DefaultReferenceModeNaming does
						// not now the default for this reference mode kind, so we set all three properties.
						naming = new DefaultReferenceModeNaming(
							store,
							new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, TargetKind),
							new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, DefaultNamingChoice),
							new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, value));
						naming.ORMModel = model;
						naming.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // DefaultReferenceModeNamingCustomFormatPropertyDescriptor class
		#region Static helper functions
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name</param>
		public static void ResolveObjectTypeName(ObjectType possibleEntityType, ObjectType possibleValueType, AddNamePart addNamePartCallback)
		{
			ResolveObjectTypeName(possibleEntityType, possibleValueType, null, addNamePartCallback);
		}
		private static Regex myReferenceModeFormatStringParser;
		private static Regex ReferenceModeFormatStringParser
		{
			get
			{
				Regex retVal = myReferenceModeFormatStringParser;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myReferenceModeFormatStringParser,
						new Regex(
							@"(?n)\G(?<Before>.*?)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					retVal = myReferenceModeFormatStringParser;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="forceNamingChoice">Use this naming choice (if specified) instead of the current setting on <paramref name="possibleEntityType"/></param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name. If this parameter is set,
		/// then we attempt to split the ValueTypeName into pieces and add through the callback instead of using the return value.</param>
		/// <returns>An appropriate name, or <see langword="null"/> if the expected relationship does not pan out.</returns>
		private static string ResolveObjectTypeName(ObjectType possibleEntityType, ObjectType possibleValueType, ReferenceModeNamingChoice? forceNamingChoice, AddNamePart addNamePartCallback)
		{
			ReferenceMode referenceMode;
			ReferenceModeType referenceModeType;
			if (possibleEntityType != null &&
				null != (referenceMode = possibleEntityType.ReferenceMode) &&
				ReferenceModeType.General != (referenceModeType = referenceMode.Kind.ReferenceModeType))
			{
				ObjectType actualValueType = possibleEntityType.PreferredIdentifier.RoleCollection[0].RolePlayer;
				if (possibleValueType != null && actualValueType != possibleValueType)
				{
					return null;
				}

				ReferenceModeNamingChoice choice = forceNamingChoice.HasValue ? forceNamingChoice.Value : GetNamingChoiceFromObjectType(possibleEntityType);
				switch (choice)
				{
					case ReferenceModeNamingChoice.ModelDefault:
						switch (GetNamingChoiceFromORMModel(possibleEntityType.Model, referenceModeType))
						{
							case EffectiveReferenceModeNamingChoice.EntityTypeName:
								if (addNamePartCallback != null)
								{
									addNamePartCallback(possibleEntityType.Name, null);
									return null;
								}
								else
								{
									return possibleEntityType.Name;
								}
							case EffectiveReferenceModeNamingChoice.ValueTypeName:
								if (addNamePartCallback != null)
								{
									CheckRegex(referenceMode, referenceModeType, possibleEntityType, addNamePartCallback);
									return null;
								}
								else
								{
									return actualValueType.Name;
								}
							case EffectiveReferenceModeNamingChoice.ReferenceModeName:
								if (addNamePartCallback != null)
								{
									addNamePartCallback(referenceMode.Name, null);
									return null;
								}
								else
								{
									return referenceMode.Name;
								}
						}
						break;
					case ReferenceModeNamingChoice.EntityTypeName:
						if (addNamePartCallback != null)
						{
							addNamePartCallback(possibleEntityType.Name, null);
							return null;
						}
						else
						{
							return possibleEntityType.Name;
						}
					case ReferenceModeNamingChoice.ValueTypeName:
						//UNDONE: Ask Matt if the regex should also be used here
						if (addNamePartCallback != null)
						{
							CheckRegex(referenceMode, referenceModeType, possibleEntityType, addNamePartCallback);
							return null;
						}
						else
						{
							return actualValueType.Name;
						}
					case ReferenceModeNamingChoice.ReferenceModeName:
						if (addNamePartCallback != null)
						{
							addNamePartCallback(referenceMode.Name, null);
							return null;
						}
						else
						{
							return referenceMode.Name;
						}
				}

				// All that's left is custom format
				string customFormat = GetCustomFormatFromObjectType(possibleEntityType);
				if (string.IsNullOrEmpty(customFormat))
				{
					customFormat = GetCustomFormatFromORMModel(possibleEntityType.Model, referenceModeType);
				}
				if (!string.IsNullOrEmpty(customFormat))
				{
					return string.Format(CultureInfo.CurrentCulture, customFormat, actualValueType.Name, possibleEntityType.Name, referenceMode.Name);
				}
			}
			if (addNamePartCallback != null)
			{
				addNamePartCallback(possibleEntityType.Name, null);
			}
			return null;
		}

		/// <summary>
		/// Used to separate value types into their constituant parts and and specify whether to explicitly case the word or not
		/// </summary>
		/// <param name="referenceMode">Used to get the name of the <see cref="ReferenceMode"/></param>
		/// <param name="referenceModeType">Used to determine if the mode is UnitBased and should therefore be explicitly cased</param>
		/// <param name="possibleEntityType">Used to access the name of the <see cref="ObjectType"/></param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		private static void CheckRegex(ReferenceMode referenceMode, ReferenceModeType referenceModeType, ObjectType possibleEntityType, AddNamePart addNamePartCallback)
		{
			string referenceModeFormatString = referenceMode.FormatString;
			Regex formatStringParser = ReferenceModeFormatStringParser;
			Match match = formatStringParser.Match(referenceModeFormatString);
			int trailingTextIndex = 0;
			while (match.Success)
			{
				GroupCollection groups = match.Groups;
				string before = groups["Before"].Value;
				if (before.Length != 0)
				{
					addNamePartCallback(before, null);
				}
				switch (int.Parse(groups["ReplaceIndex"].Value))
				{
					case 0:
						addNamePartCallback(possibleEntityType.Name, null);
						break;
					case 1:
						addNamePartCallback(new NamePart(referenceMode.Name, referenceModeType == ReferenceModeType.UnitBased ? NamePartOptions.ExplicitCasing : NamePartOptions.None), null);
						break;
				}
				trailingTextIndex += match.Length;
				match = match.NextMatch();
			}
			if (trailingTextIndex < referenceModeFormatString.Length)
			{
				addNamePartCallback(referenceModeFormatString.Substring(trailingTextIndex), null);
			}
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the stored <see cref="ReferenceModeNamingChoice"/>.
		/// </summary>
		private static ReferenceModeNamingChoice GetNamingChoiceFromObjectType(ObjectType objectType)
		{
			ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
			return (null != naming) ?
				naming.NamingChoice :
				ReferenceModeNamingChoice.ModelDefault;
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the stored <see cref="CustomFormat"/> string.
		/// </summary>
		private static string GetCustomFormatFromObjectType(ObjectType objectType)
		{
			ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
			return (null != naming) ?
				naming.CustomFormat :
				"";
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored <see cref="EffectiveReferenceModeNamingChoice"/>.
		/// </summary>
		private static EffectiveReferenceModeNamingChoice GetNamingChoiceFromORMModel(ORMModel model, ReferenceModeType referenceModeType)
		{
			DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, referenceModeType);
			if (naming != null)
			{
				return naming.NamingChoice;
			}
			return (referenceModeType == ReferenceModeType.UnitBased) ? EffectiveReferenceModeNamingChoice.EntityTypeName : EffectiveReferenceModeNamingChoice.ValueTypeName;
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored <see cref="DefaultReferenceModeNaming"/> for the specified ReferenceModeType.
		/// </summary>
		private static DefaultReferenceModeNaming GetDefaultReferenceModeNamingFromORMModel(ORMModel model, ReferenceModeType referenceModeType)
		{
			if (model != null)
			{
				foreach (DefaultReferenceModeNaming naming in DefaultReferenceModeNamingCustomizesORMModel.GetDefaultReferenceModeNamingCollection(model))
				{
					if (naming.ReferenceModeTargetKind == (DefaultReferenceModeNamingTargetKind)referenceModeType)
					{
						return naming;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the default custom format for the specified ReferenceModeType in the associated <see cref="ORMModel"/>
		/// </summary>
		private static string GetDefaultCustomFormatForObjectType(ObjectType objectType)
		{
			ORMModel model;
			ReferenceMode referenceMode;
			ReferenceModeType referenceModeType;
			if (null != objectType &&
				null != (model = objectType.Model) &&
				null != (referenceMode = objectType.ReferenceMode) &&
				ReferenceModeType.General != (referenceModeType = referenceMode.Kind.ReferenceModeType))
			{
				return GetCustomFormatFromORMModel(model, referenceModeType);
			}
			return "";
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored custom format used for reference mode naming
		/// </summary>
		private static string GetCustomFormatFromORMModel(ORMModel model, ReferenceModeType referenceModeType)
		{
			DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, referenceModeType);
			if (naming != null)
			{
				return naming.CustomFormat;
			}
			return (referenceModeType == ReferenceModeType.UnitBased) ? ResourceStrings.ReferenceModeNamingDefaultCustomFormatUnitBased : ResourceStrings.ReferenceModeNamingDefaultCustomFormatPopular;
		}
		#endregion // Static helper functions
		#region Instance helper functions
		/// <summary>
		/// Determine if the <see cref="CustomFormat"/> string is currently used
		/// </summary>
		/// <param name="requireFormatString">Set to <see langword="true"/> if a current format string is required.</param>
		/// <returns><see langword="true"/> if the CustomFormat field is currently in use.</returns>
		private bool UsesCustomFormat(bool requireFormatString)
		{
			if (requireFormatString && string.IsNullOrEmpty(CustomFormat))
			{
				return false;
			}
			switch (NamingChoice)
			{
				case ReferenceModeNamingChoice.CustomFormat:
					return true;
				case ReferenceModeNamingChoice.ModelDefault:
					{
						ObjectType objectType;
						ReferenceMode referenceMode;
						ReferenceModeType referenceModeType;
						if (null != (objectType = this.ObjectType) &&
							null != (referenceMode = objectType.ReferenceMode) &&
							ReferenceModeType.General != (referenceModeType = referenceMode.Kind.ReferenceModeType))
						{
							return GetNamingChoiceFromORMModel(objectType.Model, referenceModeType) == EffectiveReferenceModeNamingChoice.CustomFormat;
						}
					}
					break;
			}
			return false;
		}
		#endregion // Instance helper functions
	}
}
