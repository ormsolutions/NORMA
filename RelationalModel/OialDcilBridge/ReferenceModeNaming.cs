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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region ReferenceModeNamingUse enum
	/// <summary>
	/// Determine the target usage for <see cref="ReferenceModeNaming"/> settings
	/// </summary>
	public enum ReferenceModeNamingUse
	{
		/// <summary>
		/// The naming pattern is used for references to the EntityType
		/// with the given reference mode.
		/// </summary>
		ReferenceToEntityType,
		/// <summary>
		/// The naming pattern is used when a ValueType matching the
		/// reference mode pattern is used as a simple primary identifier column.
		/// </summary>
		PrimaryIdentifier,
	}
	#endregion // ReferenceModeNamingUse enum
	partial class ReferenceModeNaming
	{
		#region Extension property provider callbacks
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="ObjectType"/>
		/// </summary>
		public static void PopulateReferenceModeNamingExtensionProperties(object extendableElement, PropertyDescriptorCollection properties)
		{
			ObjectType objectType;
			IReferenceModePattern referenceMode;
			if (null != (objectType = extendableElement as ObjectType) &&
				!objectType.IsDeleted &&
				null != (referenceMode = objectType.ReferenceModePattern))
			{
				ReferenceModeType referenceModeType = referenceMode.ReferenceModeType;
				properties.Add(ReferenceToEntityTypeNamingChoicePropertyDescriptor.Instance);
				ReferenceModeNamingChoice currentChoice = GetNamingChoiceFromObjectType(objectType, ReferenceModeNamingUse.ReferenceToEntityType);
				if (currentChoice == ReferenceModeNamingChoice.CustomFormat ||
					(currentChoice == ReferenceModeNamingChoice.ModelDefault && GetNamingChoiceFromORMModel(objectType.Model, referenceModeType, ReferenceModeNamingUse.ReferenceToEntityType) == EffectiveReferenceModeNamingChoice.CustomFormat))
				{
					properties.Add(ReferenceToEntityTypeCustomFormatPropertyDescriptor.Instance);
				}
				properties.Add(PrimaryIdentifierNamingChoicePropertyDescriptor.Instance);
				currentChoice = GetNamingChoiceFromObjectType(objectType, ReferenceModeNamingUse.PrimaryIdentifier);
				if (currentChoice == ReferenceModeNamingChoice.CustomFormat ||
					(currentChoice == ReferenceModeNamingChoice.ModelDefault && GetNamingChoiceFromORMModel(objectType.Model, referenceModeType, ReferenceModeNamingUse.PrimaryIdentifier) == EffectiveReferenceModeNamingChoice.CustomFormat))
				{
					properties.Add(PrimaryIdentifierCustomFormatPropertyDescriptor.Instance);
				}
			}
		}
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="ORMModel"/>
		/// </summary>
		public static void PopulateDefaultReferenceModeNamingExtensionPropertiesOnORMModel(object extendableElement, PropertyDescriptorCollection properties)
		{
			ORMModel model;
			if (null != (model = extendableElement as ORMModel) &&
				!model.IsDeleted)
			{
				properties.Add(GeneralGroupingPropertyDescriptor.Instance);
				properties.Add(PopularGroupingPropertyDescriptor.Instance);
				properties.Add(UnitBasedGroupingPropertyDescriptor.Instance);
			}
		}
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="RelationalNameGenerator"/>
		/// </summary>
		public static void PopulateDefaultReferenceModeNamingExtensionPropertiesOnColumnNameGenerator(object extendableElement, PropertyDescriptorCollection properties)
		{
			RelationalNameGenerator generator;
			if (null != (generator = extendableElement as RelationalNameGenerator) &&
				!generator.IsDeleted &&
				generator.NameUsageType == typeof(ColumnNameUsage))
			{
				properties.Add(GeneralGroupingPropertyDescriptor.Instance);
				properties.Add(PopularGroupingPropertyDescriptor.Instance);
				properties.Add(UnitBasedGroupingPropertyDescriptor.Instance);
			}
		}
		#endregion // Extension property provider callbacks
		#region ReferenceModeNamingPropertyDescriptor class
		private sealed class ReferenceToEntityTypeNamingChoicePropertyDescriptor : ReferenceModeNamingPropertyDescriptor
		{
			public static readonly ReferenceToEntityTypeNamingChoicePropertyDescriptor Instance = new ReferenceToEntityTypeNamingChoicePropertyDescriptor();
			private ReferenceToEntityTypeNamingChoicePropertyDescriptor()
				: base("ReferenceToEntityTypeNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
			public override object GetEditor(Type editorBaseType)
			{
				return (editorBaseType == typeof(UITypeEditor)) ? ReferenceModeNamingEditor.Instance : base.GetEditor(editorBaseType);
			}
			private sealed class ReferenceModeNamingEditor : ReferenceModeNamingEditorBase
			{
				public static readonly ReferenceModeNamingEditor Instance = new ReferenceModeNamingEditor();
				private ReferenceModeNamingEditor() { }
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.ReferenceToEntityType; }
				}
			}
		}
		private sealed class PrimaryIdentifierNamingChoicePropertyDescriptor : ReferenceModeNamingPropertyDescriptor
		{
			public static readonly PrimaryIdentifierNamingChoicePropertyDescriptor Instance = new PrimaryIdentifierNamingChoicePropertyDescriptor();
			private PrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("PrimaryIdentifierNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
			public override object GetEditor(Type editorBaseType)
			{
				return (editorBaseType == typeof(UITypeEditor)) ? ReferenceModeNamingEditor.Instance : base.GetEditor(editorBaseType);
			}
			private sealed class ReferenceModeNamingEditor : ReferenceModeNamingEditorBase
			{
				public static readonly ReferenceModeNamingEditor Instance = new ReferenceModeNamingEditor();
				private ReferenceModeNamingEditor() { }
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.PrimaryIdentifier; }
				}
			}
		}
		private abstract class ReferenceModeNamingPropertyDescriptor : PropertyDescriptor
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
			protected abstract class ReferenceModeNamingEditorBase : ElementPicker<ReferenceModeNamingEditorBase>
			{
				/// <summary>
				/// Get the target use for this editor
				/// </summary>
				protected abstract ReferenceModeNamingUse TargetUse { get;}
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
					IReferenceModePattern referenceMode;
					if (null != (objectType = EditorUtility.ResolveContextInstance(context.Instance, true) as ObjectType) &&
						null != (referenceMode = objectType.ReferenceModePattern))
					{
						ReferenceModeType referenceModeType = referenceMode.ReferenceModeType;
						string currentModelDefault = null;
						switch (GetNamingChoiceFromORMModel(objectType.Model, referenceModeType, TargetUse))
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
							string.Format(culture, ResourceStrings.ReferenceModeNamingCurrentFormatStringCustomFormat, ResolveObjectTypeName(objectType, ReferenceModeNamingChoice.CustomFormat, TargetUse)),
						};
					}
					return null;
				}
			}
			#endregion // ReferenceModeNamingEditor class
			protected ReferenceModeNamingPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
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
				return GetNamingChoiceFromObjectType(GetObjectTypeFromComponent(component), TargetUse) != ReferenceModeNamingChoice.ModelDefault;
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingNamingChoicePropertyDisplayName :
						ResourceStrings.ReferenceModeNamingPrimaryIdentifierNamingChoicePropertyDisplayName;
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
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingNamingChoicePropertyDescription :
						ResourceStrings.ReferenceModeNamingPrimaryIdentifierNamingChoicePropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetNamingChoiceFromObjectType(GetObjectTypeFromComponent(component), TargetUse);
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
			private void SetValue(ObjectType objectType, ReferenceModeNamingChoice value)
			{
				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ReferenceModeNamingNamingChoicePropertyTransactionName))
				{
					ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
					if (naming != null)
					{
						if (TargetUse == ReferenceModeNamingUse.PrimaryIdentifier)
						{
							naming.PrimaryIdentifierNamingChoice = value;
						}
						else
						{
							naming.NamingChoice = value;
						}
					}
					else if (value != ReferenceModeNamingChoice.ModelDefault)
					{
						naming = new ReferenceModeNaming(store, new PropertyAssignment((TargetUse == ReferenceModeNamingUse.PrimaryIdentifier) ? ReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId : ReferenceModeNaming.NamingChoiceDomainPropertyId, value));
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
		private abstract class CustomFormatTypeConverter : StringConverter
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
			protected CustomFormatTypeConverter()
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
		private sealed class ReferenceToEntityTypeCustomFormatPropertyDescriptor : ReferenceModeNamingCustomFormatPropertyDescriptor
		{
			public static readonly ReferenceToEntityTypeCustomFormatPropertyDescriptor Instance = new ReferenceToEntityTypeCustomFormatPropertyDescriptor();
			private ReferenceToEntityTypeCustomFormatPropertyDescriptor()
				: base("ReferenceToEntityTypeCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
			public override TypeConverter Converter
			{
				get
				{
					return DefaultAwareReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			private sealed class DefaultAwareReferenceModeNamingCustomFormatTypeConverter : DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase
			{
				public static readonly DefaultAwareReferenceModeNamingCustomFormatTypeConverter Instance = new DefaultAwareReferenceModeNamingCustomFormatTypeConverter();
				private DefaultAwareReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.ReferenceToEntityType; }
				}
			}
		}
		private sealed class PrimaryIdentifierCustomFormatPropertyDescriptor : ReferenceModeNamingCustomFormatPropertyDescriptor
		{
			public static readonly PrimaryIdentifierCustomFormatPropertyDescriptor Instance = new PrimaryIdentifierCustomFormatPropertyDescriptor();
			private PrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("PrimaryIdentifierCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
			public override TypeConverter Converter
			{
				get
				{
					return DefaultAwareReferenceModeNamingCustomFormatTypeConverter.Instance;
				}
			}
			private sealed class DefaultAwareReferenceModeNamingCustomFormatTypeConverter : DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase
			{
				public static readonly DefaultAwareReferenceModeNamingCustomFormatTypeConverter Instance = new DefaultAwareReferenceModeNamingCustomFormatTypeConverter();
				private DefaultAwareReferenceModeNamingCustomFormatTypeConverter()
				{
				}
				protected override ReferenceModeNamingUse TargetUse
				{
					get { return ReferenceModeNamingUse.PrimaryIdentifier; }
				}
			}
		}
		private abstract class ReferenceModeNamingCustomFormatPropertyDescriptor : PropertyDescriptor
		{
			#region DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			protected abstract class DefaultAwareReferenceModeNamingCustomFormatTypeConverterBase : CustomFormatTypeConverter
			{
				/// <summary>
				/// Get the target use for this property descriptor
				/// </summary>
				protected abstract ReferenceModeNamingUse TargetUse { get;}
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						string stringValue = value as string;
						if (string.IsNullOrEmpty(stringValue))
						{
							// Fill in the backing default value if no data is currently shown
							value = GetDefaultCustomFormatForObjectType(GetObjectTypeFromComponent(context.Instance), TargetUse);
						}
					}
					return base.ConvertTo(context, culture, value, destinationType);
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					object newValue = base.ConvertFrom(context, culture, value);
					ObjectType objectType = GetObjectTypeFromComponent(context.Instance);
					string oldFormat = GetCustomFormatFromObjectType(objectType, TargetUse);
					if (string.IsNullOrEmpty(oldFormat) &&
						GetDefaultCustomFormatForObjectType(objectType, TargetUse) == newValue.ToString())
					{
						return "";
					}
					return newValue;
				}
			}
			#endregion // DefaultAwareReferenceModeNamingCustomFormatTypeConverter class
			protected ReferenceModeNamingCustomFormatPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
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
				string objectFormat = GetCustomFormatFromObjectType(objectType, TargetUse);
				return !(objectFormat.Length == 0 || objectFormat == GetDefaultCustomFormatForObjectType(objectType, TargetUse));
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingCustomFormatPropertyDisplayName :
						ResourceStrings.ReferenceModeNamingPrimaryIdentifierCustomFormatPropertyDisplayName;
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
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingCustomFormatPropertyDescription :
						ResourceStrings.ReferenceModeNamingPrimaryIdentifierCustomFormatPropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetCustomFormatFromObjectType(GetObjectTypeFromComponent(component), TargetUse);
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
			private void SetValue(ObjectType objectType, string value)
			{
				Store store = objectType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ReferenceModeNamingCustomFormatPropertyTransactionName))
				{
					ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
					if (naming != null)
					{
						if (TargetUse == ReferenceModeNamingUse.PrimaryIdentifier)
						{
							naming.PrimaryIdentifierCustomFormat = value;
						}
						else
						{
							naming.CustomFormat = value;
						}
					}
					else if (!string.IsNullOrEmpty(value))
					{
						// The only way to not have one of these already is with a ModelDefault NamingChoice, which is the default, so we
						// just need to assign the CustomFormat property.
						naming = new ReferenceModeNaming(store, new PropertyAssignment((TargetUse == ReferenceModeNamingUse.PrimaryIdentifier) ? ReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId : ReferenceModeNaming.CustomFormatDomainPropertyId, value));
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
		#region DefaultNamingChoicePropertyDescriptor class
		private sealed class UnitBasedDefaultNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly UnitBasedDefaultNamingChoicePropertyDescriptor Instance = new UnitBasedDefaultNamingChoicePropertyDescriptor();
			private UnitBasedDefaultNamingChoicePropertyDescriptor()
				: base("UnitBasedDefaultNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor Instance = new UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor();
			private UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class PopularDefaultNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly PopularDefaultNamingChoicePropertyDescriptor Instance = new PopularDefaultNamingChoicePropertyDescriptor();
			private PopularDefaultNamingChoicePropertyDescriptor()
				: base("PopularDefaultNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor Instance = new PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor();
			private PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class GeneralDefaultNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly GeneralDefaultNamingChoicePropertyDescriptor Instance = new GeneralDefaultNamingChoicePropertyDescriptor();
			private GeneralDefaultNamingChoicePropertyDescriptor()
				: base("GeneralDefaultNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor : DefaultNamingChoicePropertyDescriptor
		{
			public static readonly GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor Instance = new GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor();
			private GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor()
				: base("GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private abstract class DefaultNamingChoicePropertyDescriptor : PropertyDescriptor
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
			protected DefaultNamingChoicePropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the default naming choice for this type of object
			/// </summary>
			private EffectiveReferenceModeNamingChoice DefaultNamingChoice
			{
				get
				{
					return GetDefaultNamingChoice(TargetKind, TargetUse);
				}
			}
			private string TransactionName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultNamingChoicePropertyTransactionName :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyTransactionName;
				}
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultNamingChoicePropertyDisplayName :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultNamingChoicePropertyDescription :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierNamingChoicePropertyDescription;
				}
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract ReferenceModeType TargetKind { get;}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
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
				return GetNamingChoiceFromORMModel(GetORMModelFromComponent(component), TargetKind, TargetUse) != DefaultNamingChoice;
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
				return GetNamingChoiceFromORMModel(GetORMModelFromComponent(component), TargetKind, TargetUse);
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
			private void SetValue(ORMModel model, EffectiveReferenceModeNamingChoice value)
			{
				Store store = model.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(TransactionName))
				{
					ReferenceModeType kind = TargetKind;
					DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, kind);
					bool setPrimaryIdentifier = TargetUse == ReferenceModeNamingUse.PrimaryIdentifier;
					if (naming != null)
					{
						if (setPrimaryIdentifier)
						{
							naming.PrimaryIdentifierNamingChoice = value;
						}
						else
						{
							naming.NamingChoice = value;
						}
					}
					else if (value != DefaultNamingChoice)
					{
						// The only way to not have one of these already is with a default NamingChoice. However, DefaultReferenceModeNaming does
						// not know the default for this reference mode kind, so we set all three properties.
						naming = new DefaultReferenceModeNaming(
							store,
							new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, kind),
							new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, setPrimaryIdentifier ? GetDefaultNamingChoice(kind, ReferenceModeNamingUse.ReferenceToEntityType) : value),
							new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, setPrimaryIdentifier ? value : GetDefaultNamingChoice(kind, ReferenceModeNamingUse.PrimaryIdentifier)),
							new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, GetDefaultCustomFormat(kind, ReferenceModeNamingUse.ReferenceToEntityType)),
							new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId, GetDefaultCustomFormat(kind, ReferenceModeNamingUse.PrimaryIdentifier)));
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
		#endregion // DefaultNamingChoicePropertyDescriptor class
		#region DefaultCustomFormatPropertyDescriptor class
		private sealed class UnitBasedDefaultCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly UnitBasedDefaultCustomFormatPropertyDescriptor Instance = new UnitBasedDefaultCustomFormatPropertyDescriptor();
			private UnitBasedDefaultCustomFormatPropertyDescriptor()
				: base("UnitBasedDefaultCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor Instance = new UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor();
			private UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.UnitBased; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class PopularDefaultCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly PopularDefaultCustomFormatPropertyDescriptor Instance = new PopularDefaultCustomFormatPropertyDescriptor();
			private PopularDefaultCustomFormatPropertyDescriptor()
				: base("PopularDefaultCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor Instance = new PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor();
			private PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.Popular; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private sealed class GeneralDefaultCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly GeneralDefaultCustomFormatPropertyDescriptor Instance = new GeneralDefaultCustomFormatPropertyDescriptor();
			private GeneralDefaultCustomFormatPropertyDescriptor()
				: base("GeneralDefaultCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.ReferenceToEntityType; }
			}
		}
		private sealed class GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor : DefaultCustomFormatPropertyDescriptor
		{
			public static readonly GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor Instance = new GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor();
			private GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor()
				: base("GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor")
			{
			}
			protected override ReferenceModeType TargetKind
			{
				get { return ReferenceModeType.General; }
			}
			protected override ReferenceModeNamingUse TargetUse
			{
				get { return ReferenceModeNamingUse.PrimaryIdentifier; }
			}
		}
		private abstract class DefaultCustomFormatPropertyDescriptor : PropertyDescriptor
		{
			#region ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter class
			private class ReplacementRequiredReferenceModeNamingCustomFormatTypeConverter : CustomFormatTypeConverter
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
			protected DefaultCustomFormatPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Get the default custom format for this type of object
			/// </summary>
			private string DefaultCustomFormat
			{
				get
				{
					return GetDefaultCustomFormat(TargetKind, TargetUse);
				}
			}
			private string TransactionName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultCustomFormatPropertyTransactionName :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyTransactionName;
				}
			}
			public sealed override string DisplayName
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultCustomFormatPropertyDisplayName :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return (TargetUse == ReferenceModeNamingUse.ReferenceToEntityType) ?
						ResourceStrings.ReferenceModeNamingDefaultCustomFormatPropertyDescription :
						ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPropertyDescription;
				}
			}
			/// <summary>
			/// Get the reference mode kind for this property desciptor
			/// </summary>
			protected abstract ReferenceModeType TargetKind { get;}
			/// <summary>
			/// Get the target use for this property descriptor
			/// </summary>
			protected abstract ReferenceModeNamingUse TargetUse { get;}
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
				return GetCustomFormatFromORMModel(GetORMModelFromComponent(component), TargetKind, TargetUse) != DefaultCustomFormat;
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
				return GetCustomFormatFromORMModel(GetORMModelFromComponent(component), TargetKind, TargetUse);
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
					ReferenceModeType kind = TargetKind;
					DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, kind);
					bool setPrimaryIdentifier = TargetUse == ReferenceModeNamingUse.PrimaryIdentifier;
					if (naming != null)
					{
						if (setPrimaryIdentifier)
						{
							naming.PrimaryIdentifierCustomFormat = value;
						}
						else
						{
							naming.CustomFormat = value;
						}
					}
					else if (!string.IsNullOrEmpty(value))
					{
						// The only way to not have one of these already is with a default NamingChoice. However, DefaultReferenceModeNaming does
						// not know the default for this reference mode kind, so we set all three properties.
						naming = new DefaultReferenceModeNaming(
							store,
							new PropertyAssignment(DefaultReferenceModeNaming.ReferenceModeTargetKindDomainPropertyId, kind),
							new PropertyAssignment(DefaultReferenceModeNaming.NamingChoiceDomainPropertyId, GetDefaultNamingChoice(kind, ReferenceModeNamingUse.ReferenceToEntityType)),
							new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierNamingChoiceDomainPropertyId, GetDefaultNamingChoice(kind, ReferenceModeNamingUse.PrimaryIdentifier)),
							new PropertyAssignment(DefaultReferenceModeNaming.CustomFormatDomainPropertyId, setPrimaryIdentifier ? GetDefaultCustomFormat(kind, ReferenceModeNamingUse.ReferenceToEntityType) : value),
							new PropertyAssignment(DefaultReferenceModeNaming.PrimaryIdentifierCustomFormatDomainPropertyId, setPrimaryIdentifier ? value : GetDefaultCustomFormat(kind, ReferenceModeNamingUse.PrimaryIdentifier)));
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
		#endregion // DefaultCustomFormatPropertyDescriptor class
		#region DefaultGroupingPropertyDescriptor class
		private class UnitBasedGroupingPropertyDescriptor : DefaultGroupingPropertyDescriptor
		{
			public static readonly UnitBasedGroupingPropertyDescriptor Instance = new UnitBasedGroupingPropertyDescriptor();
			private UnitBasedGroupingPropertyDescriptor()
				: base("UnitBasedGroupingPropertyDescriptor")
			{
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupUnitBasedDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupUnitBasedDescription;
				}
			}
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						UnitBasedDefaultNamingChoicePropertyDescriptor.Instance,
						UnitBasedDefaultCustomFormatPropertyDescriptor.Instance,
						UnitBasedDefaultPrimaryIdentifierNamingChoicePropertyDescriptor.Instance,
						UnitBasedDefaultPrimaryIdentifierCustomFormatPropertyDescriptor.Instance};
				}
			}
		}
		private class PopularGroupingPropertyDescriptor : DefaultGroupingPropertyDescriptor
		{
			public static readonly PopularGroupingPropertyDescriptor Instance = new PopularGroupingPropertyDescriptor();
			private PopularGroupingPropertyDescriptor()
				: base("PopularGroupingPropertyDescriptor")
			{
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupPopularDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupPopularDescription;
				}
			}
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						PopularDefaultNamingChoicePropertyDescriptor.Instance,
						PopularDefaultCustomFormatPropertyDescriptor.Instance,
						PopularDefaultPrimaryIdentifierNamingChoicePropertyDescriptor.Instance,
						PopularDefaultPrimaryIdentifierCustomFormatPropertyDescriptor.Instance};
				}
			}
		}
		private class GeneralGroupingPropertyDescriptor : DefaultGroupingPropertyDescriptor
		{
			public static readonly GeneralGroupingPropertyDescriptor Instance = new GeneralGroupingPropertyDescriptor();
			private GeneralGroupingPropertyDescriptor()
				: base("GeneralGroupingPropertyDescriptor")
			{
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupGeneralDisplayName;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ReferenceModeNamingDefaultGroupGeneralDescription;
				}
			}
			protected override PropertyDescriptor[] PropertyDescriptors
			{
				get
				{
					return new PropertyDescriptor[]{
						GeneralDefaultNamingChoicePropertyDescriptor.Instance,
						GeneralDefaultCustomFormatPropertyDescriptor.Instance,
						GeneralDefaultPrimaryIdentifierNamingChoicePropertyDescriptor.Instance,
						GeneralDefaultPrimaryIdentifierCustomFormatPropertyDescriptor.Instance};
				}
			}
		}
		private abstract class DefaultGroupingPropertyDescriptor : PropertyDescriptor
		{
			#region ExpandableTypeConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			private class ExpandableTypeConverter : TypeConverter
			{
				public static readonly ExpandableTypeConverter Instance = new ExpandableTypeConverter();
				private ExpandableTypeConverter()
				{
				}
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return false;
				}
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return destinationType == typeof(string);
				}
				public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
				{
					return null;
				}
				public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
				{
					if (destinationType == typeof(string))
					{
						return context.PropertyDescriptor.ShouldSerializeValue(context.Instance) ? ResourceStrings.ReferenceModeNamingDefaultGroupDisplayValueCustom : ResourceStrings.ReferenceModeNamingDefaultGroupDisplayValueDefault;
					}
					return null;
				}
				public override bool GetPropertiesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
				public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
				{
					return context.PropertyDescriptor.GetChildProperties(context.Instance, attributes);
				}
			}
			#endregion // ExpandableTypeConverter
			protected DefaultGroupingPropertyDescriptor(string name)
				: base(name, null)
			{
			}
			/// <summary>
			/// Return an array of <see cref="PropertyDescriptor"/>s to use in the default expansion
			/// </summary>
			protected abstract PropertyDescriptor[] PropertyDescriptors { get;}
			private PropertyDescriptorCollection myProperties;
			public sealed override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
			{
				PropertyDescriptorCollection retVal = myProperties;
				if (retVal == null)
				{
					myProperties = retVal = new PropertyDescriptorCollection(PropertyDescriptors, true);
				}
				return retVal;
			}
			public override TypeConverter Converter
			{
				get
				{
					return ExpandableTypeConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return true;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				foreach (PropertyDescriptor descriptor in GetChildProperties(null, null))
				{
					if (descriptor.ShouldSerializeValue(component))
					{
						return true;
					}
				}
				return false;
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
				return component;
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
					return true;
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
				foreach (PropertyDescriptor descriptor in GetChildProperties(null, null))
				{
					descriptor.ResetValue(component);
				}
			}
			public sealed override void SetValue(object component, object value)
			{
			}
		}
		#endregion // DefaultGroupingPropertyDescriptor class
		#region Static helper functions
		private static Regex myFormatStringParser;
		private static Regex FormatStringParser
		{
			get
			{
				Regex retVal = myFormatStringParser;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFormatStringParser,
						new Regex(
							@"(?n)\G(?<Before>.*?)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					retVal = myFormatStringParser;
				}
				return retVal;
			}
		}
		private static ORMModel GetORMModelFromComponent(object component)
		{
			// UNDONE: We currently support showing properties on both the ORMModel
			// and the column-specific RelationalNameGenerator. The default relational
			// mapping settings predate the name generator mechanism and are associated with
			// an ORMModel, not a name generator. Changing this to store the settings with
			// the name generator requires a breaking file format change. We should look at
			// doing this the next time we upgrade the file format.
			ORMModel retVal = null;
			ModelElement element;
			if (null != (element = EditorUtility.ResolveContextInstance(component, false) as ModelElement))
			{
				if (null == (retVal = element as ORMModel))
				{
					ReadOnlyCollection<ORMModel> models = element.Store.ElementDirectory.FindElements<ORMModel>(false);
					if (0 != models.Count)
					{
						retVal = models[0];
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Retrieve the name generated for a reference to the <paramref name="entityType"/> for the
		/// given <paramref name="namingChoice"/>
		/// </summary>
		/// <param name="entityType">An entity type to generate the name for</param>
		/// <param name="namingChoice">The <see cref="ReferenceModeNamingChoice"/> to get the name for.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <returns>A name, or <see langword="null"/> if the ference mode pattern does not resolve.</returns>
		public static string ResolveObjectTypeName(ObjectType entityType, ReferenceModeNamingChoice namingChoice, ReferenceModeNamingUse targetUse)
		{
			bool consumedValueTypeDummy;
			return ResolveObjectTypeName(entityType, null, null, true, targetUse, namingChoice, null, null, out consumedValueTypeDummy);
		}
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="alternateEntityType">An <see cref="ObjectType"/> that is a subtype of <paramref name="possibleEntityType"/>. <paramref name="possibleEntityType"/> is used to resolve any reference mode relationship, but the name for this type is generated.</param>
		/// <param name="preferEntityType">If true and a reference mode pattern is not found, then use the name of the <paramref name="possibleEntityType"/> by default.
		/// Otherwise, use the <paramref name="possibleValueType"/> name as the default when a reference mode pattern is not found.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <param name="nameGenerator">An optional <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name.</param>
		/// <returns>True if the valuetype was used as part of the generated name.</returns>
		public static bool ResolveObjectTypeName(ObjectType possibleEntityType, ObjectType possibleValueType, ObjectType alternateEntityType, bool preferEntityType, ReferenceModeNamingUse targetUse, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			bool retVal;
			ResolveObjectTypeName(possibleEntityType, possibleValueType, alternateEntityType, preferEntityType, targetUse, null, nameGenerator, addNamePartCallback, out retVal);
			return retVal;
		}
		/// <summary>
		/// Given two <see cref="ObjectType"/> instances, determine if the <paramref name="possibleEntityType"/>
		/// is related to <paramref name="possibleValueType"/> via a reference mode pattern. If so, use the
		/// reference mode naming settings associated with the entity type to determine an appropriate name.
		/// </summary>
		/// <param name="possibleEntityType">An <see cref="ObjectType"/> that may be an EntityType with a <see cref="ReferenceMode"/></param>
		/// <param name="possibleValueType">An <see cref="ValueType"/> that may be the reference mode value type associated with <paramref name="possibleEntityType"/>. Set to <see langword="null"/> to automatically retrieve the available value type.</param>
		/// <param name="alternateEntityType">An <see cref="ObjectType"/> that is a subtype of <paramref name="possibleEntityType"/>. <paramref name="possibleEntityType"/> is used to resolve any reference mode relationship, but the name for this type is generated.</param>
		/// <param name="preferEntityType">If true and a reference mode pattern is not found, then use the name of the <paramref name="possibleEntityType"/> by default.
		/// Otherwise, use the <paramref name="possibleValueType"/> name as the default when a reference mode pattern is not found.</param>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> for the name</param>
		/// <param name="forceNamingChoice">Use this naming choice (if specified) instead of the current setting on <paramref name="possibleEntityType"/></param>
		/// <param name="nameGenerator">An optional <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">A <see cref="AddNamePart"/> delegate used to add a name. If this parameter is set,
		/// then we attempt to split the ValueTypeName into pieces and add through the callback instead of using the return value.</param>
		/// <param name="consumedValueType">Output set to true if the valuetype was used as part of the generated name.</param>
		/// <returns>An appropriate name, or <see langword="null"/> if the expected relationship does not pan out and <paramref name="addNamePartCallback"/> is <see langword="null"/>.</returns>
		private static string ResolveObjectTypeName(ObjectType possibleEntityType, ObjectType possibleValueType, ObjectType alternateEntityType, bool preferEntityType, ReferenceModeNamingUse targetUse, ReferenceModeNamingChoice? forceNamingChoice, NameGenerator nameGenerator, AddNamePart addNamePartCallback, out bool consumedValueType)
		{
			consumedValueType = false;
			IReferenceModePattern referenceMode;
			if (possibleEntityType != null &&
				null != (referenceMode = possibleEntityType.ReferenceModePattern))
			{
				ReferenceModeType referenceModeType = referenceMode.ReferenceModeType;
				ObjectType actualValueType = possibleEntityType.PreferredIdentifier.RoleCollection[0].RolePlayer;
				ObjectType resolveReferenceModeEntityType = possibleEntityType;
				if (alternateEntityType != null)
				{
					// Use the alternate for all naming purposes
					possibleEntityType = alternateEntityType;
				}
				if (possibleValueType != null && actualValueType != possibleValueType)
				{
					consumedValueType = !preferEntityType;
					if (addNamePartCallback != null)
					{
						SeparateObjectTypeParts(preferEntityType ? possibleEntityType : possibleValueType, nameGenerator, addNamePartCallback);
					}
					return null;
				}

				ReferenceModeNamingChoice choice = forceNamingChoice.HasValue ? forceNamingChoice.Value : GetNamingChoiceFromObjectType(resolveReferenceModeEntityType, targetUse);
				consumedValueType = true;
				switch (choice)
				{
					case ReferenceModeNamingChoice.ModelDefault:
						switch (GetNamingChoiceFromORMModel(possibleEntityType.Model, referenceModeType, targetUse))
						{
							case EffectiveReferenceModeNamingChoice.EntityTypeName:
								if (addNamePartCallback != null)
								{
									SeparateObjectTypeParts(possibleEntityType, nameGenerator, addNamePartCallback);
									return null;
								}
								else
								{
									return possibleEntityType.Name;
								}
							case EffectiveReferenceModeNamingChoice.ValueTypeName:
								if (addNamePartCallback != null)
								{
									string abbreviatedName = actualValueType.GetAbbreviatedName(nameGenerator, false);
									if (abbreviatedName != null)
									{
										addNamePartCallback(abbreviatedName, null);
									}
									else
									{
										SeparateReferenceModeParts(referenceMode, referenceModeType, possibleEntityType, nameGenerator, addNamePartCallback);
									}
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
							SeparateObjectTypeParts(possibleEntityType, nameGenerator, addNamePartCallback);
							return null;
						}
						else
						{
							return possibleEntityType.Name;
						}
					case ReferenceModeNamingChoice.ValueTypeName:
						if (addNamePartCallback != null)
						{
							string abbreviatedName = actualValueType.GetAbbreviatedName(nameGenerator, false);
							if (abbreviatedName != null)
							{
								addNamePartCallback(abbreviatedName, null);
							}
							else
							{
								SeparateReferenceModeParts(referenceMode, referenceModeType, possibleEntityType, nameGenerator, addNamePartCallback);
							}
							return null;
						}
						else
						{
							return actualValueType.Name;
						}
					case ReferenceModeNamingChoice.ReferenceModeName:
						if (addNamePartCallback != null)
						{
							// Use the alternate for all naming purposes
							addNamePartCallback(referenceMode.Name, null);
							return null;
						}
						else
						{
							return referenceMode.Name;
						}
				}

				// All that's left is custom format
				string customFormat = GetCustomFormatFromObjectType(possibleEntityType, targetUse);
				if (string.IsNullOrEmpty(customFormat))
				{
					customFormat = GetCustomFormatFromORMModel(possibleEntityType.Model, referenceModeType, targetUse);
				}
				if (!string.IsNullOrEmpty(customFormat))
				{
					if (addNamePartCallback != null)
					{
						SeparateCustomFormatParts(customFormat, referenceMode, referenceModeType, possibleEntityType, actualValueType, nameGenerator, addNamePartCallback);
						return null;
					}
					else
					{
						return string.Format(CultureInfo.CurrentCulture, customFormat, actualValueType.Name, possibleEntityType.Name, referenceMode.Name);
					}
				}
				consumedValueType = false;
			}
			if (addNamePartCallback != null)
			{
				consumedValueType = preferEntityType ? possibleEntityType == null : possibleValueType != null;
				if (possibleEntityType != null && alternateEntityType != null)
				{
					possibleEntityType = alternateEntityType;
				}
				SeparateObjectTypeParts(preferEntityType ? (possibleEntityType ?? possibleValueType) : (possibleValueType ?? possibleEntityType), nameGenerator, addNamePartCallback);
			}
			return null;
		}

		/// <summary>
		/// Used to separate value types into their constituent parts and and specify whether to explicitly case the word or not
		/// </summary>
		/// <param name="referenceMode">Used to get the name of the <see cref="IReferenceModePattern"/></param>
		/// <param name="referenceModeType">Used to determine if the mode is UnitBased and should therefore be explicitly cased</param>
		/// <param name="entityType">The EntityType that has this <paramref name="referenceMode"/></param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		private static void SeparateReferenceModeParts(IReferenceModePattern referenceMode, ReferenceModeType referenceModeType, ObjectType entityType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			string referenceModeFormatString = referenceMode.FormatString;
			Match match = FormatStringParser.Match(referenceModeFormatString);
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
						SeparateObjectTypeParts(entityType, nameGenerator, addNamePartCallback);
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
		/// Used to separate value types into their constituent parts and and specify whether to explicitly case the word or not
		/// </summary>
		/// <param name="customFormat">The custom format string</param>
		/// <param name="referenceMode">Used to get the name of the <see cref="IReferenceModePattern"/></param>
		/// <param name="referenceModeType">Used to determine if the mode is UnitBased and should therefore be explicitly cased</param>
		/// <param name="entityType">The EntityType that has this <paramref name="referenceMode"/></param>
		/// <param name="valueType">The ValueType that satisfies the <paramref name="referenceMode"/> with the <paramref name="entityType"/></param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		private static void SeparateCustomFormatParts(string customFormat, IReferenceModePattern referenceMode, ReferenceModeType referenceModeType, ObjectType entityType, ObjectType valueType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			Match match = FormatStringParser.Match(customFormat);
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
					case 0: // ValueType
						string abbreviatedName = valueType.GetAbbreviatedName(nameGenerator, false);
						if (abbreviatedName != null)
						{
							addNamePartCallback(abbreviatedName, null);
						}
						else
						{
							SeparateReferenceModeParts(referenceMode, referenceModeType, entityType, nameGenerator, addNamePartCallback);
						}
						break;
					case 1: // EntityType
						SeparateObjectTypeParts(entityType, nameGenerator, addNamePartCallback);
						break;
					case 2: // ReferenceMode
						addNamePartCallback(new NamePart(referenceMode.Name, referenceModeType == ReferenceModeType.UnitBased ? NamePartOptions.ExplicitCasing : NamePartOptions.None), null);
						break;
				}
				trailingTextIndex += match.Length;
				match = match.NextMatch();
			}
			if (trailingTextIndex < customFormat.Length)
			{
				addNamePartCallback(customFormat.Substring(trailingTextIndex), null);
			}
		}
		/// <summary>
		/// Used to separate <see cref="ObjectType"/> names into constituent parts if the names are
		/// generated, such as with an objectification or an ValueType associated with a <see cref="ReferenceMode"/>.
		/// </summary>
		/// <param name="objectType">The EntityType to test</param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/>, used to retrieve abbreviations for <see cref="ObjectType"/> names</param>
		/// <param name="addNamePartCallback">Used to add the names to the name collection</param>
		public static void SeparateObjectTypeParts(ObjectType objectType, NameGenerator nameGenerator, AddNamePart addNamePartCallback)
		{
			string abbreviatedName = objectType.GetAbbreviatedName(nameGenerator, false);
			if (abbreviatedName != null)
			{
				addNamePartCallback(abbreviatedName, null);
				return;
			}
			string nativeName = objectType.Name;
			FactType objectifiedFactType;
			IReferenceModePattern valueTypeReferenceMode;
			if (null != (objectifiedFactType = objectType.NestedFactType))
			{
				IReading defaultReading;
				string readingText;
				if (nativeName == objectifiedFactType.DefaultName &&
					null != (defaultReading = objectifiedFactType.GetDefaultReading()) &&
					!string.IsNullOrEmpty(readingText = defaultReading.Text))
				{
					Match match = FormatStringParser.Match(readingText);
					int trailingTextIndex = 0;
					IList<RoleBase> roles = defaultReading.RoleCollection;
					while (match.Success)
					{
						GroupCollection groups = match.Groups;
						string before = groups["Before"].Value;
						if (before.Length != 0)
						{
							addNamePartCallback(before.Replace("- ", " ").Replace(" -", " "), null);
						}
						SeparateObjectTypeParts(roles[int.Parse(groups["ReplaceIndex"].Value)].Role.RolePlayer, nameGenerator, addNamePartCallback);
						trailingTextIndex += match.Length;
						match = match.NextMatch();
					}
					if (trailingTextIndex < readingText.Length)
					{
						addNamePartCallback(readingText.Substring(trailingTextIndex).Replace("- ", " ").Replace(" -", " "), null);
					}
					return;
				}
			}
			else if (null != (valueTypeReferenceMode = objectType.Model.GetReferenceModeForValueType(objectType)))
			{
				SeparateReferenceModeParts(valueTypeReferenceMode, valueTypeReferenceMode.ReferenceModeType, objectType, nameGenerator, addNamePartCallback);
				return;
			}
			addNamePartCallback(nativeName, null);
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the stored <see cref="ReferenceModeNamingChoice"/> for
		/// the specified <see cref="ReferenceModeNamingUse"/>.
		/// </summary>
		private static ReferenceModeNamingChoice GetNamingChoiceFromObjectType(ObjectType objectType, ReferenceModeNamingUse targetUse)
		{
			ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
			return (null != naming) ?
				((targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.NamingChoice : naming.PrimaryIdentifierNamingChoice) :
				ReferenceModeNamingChoice.ModelDefault;
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the stored <see cref="CustomFormat"/> string for
		/// the specified <see cref="ReferenceModeNamingUse"/>.
		/// </summary>
		private static string GetCustomFormatFromObjectType(ObjectType objectType, ReferenceModeNamingUse targetUse)
		{
			ReferenceModeNaming naming = ReferenceModeNamingCustomizesObjectType.GetReferenceModeNaming(objectType);
			return (null != naming) ?
				((targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.CustomFormat : naming.PrimaryIdentifierCustomFormat) :
				"";
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored <see cref="EffectiveReferenceModeNamingChoice"/> for
		/// the specified <see cref="ReferenceModeNamingUse"/>.
		/// </summary>
		private static EffectiveReferenceModeNamingChoice GetNamingChoiceFromORMModel(ORMModel model, ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, referenceModeType);
			if (naming != null)
			{
				return (targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.NamingChoice : naming.PrimaryIdentifierNamingChoice;
			}
			return GetDefaultNamingChoice(referenceModeType, targetUse);
		}
		/// <summary>
		/// Get the default <see cref="EffectiveReferenceModeNamingChoice"/> for a given <see cref="ReferenceModeType"/>
		/// and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		private static EffectiveReferenceModeNamingChoice GetDefaultNamingChoice(ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			return referenceModeType == ReferenceModeType.Popular ?
				EffectiveReferenceModeNamingChoice.CustomFormat :
				(targetUse == ReferenceModeNamingUse.PrimaryIdentifier ? EffectiveReferenceModeNamingChoice.ValueTypeName : EffectiveReferenceModeNamingChoice.EntityTypeName);
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored <see cref="DefaultReferenceModeNaming"/> for the specified <see cref="ReferenceModeType"/>.
		/// </summary>
		private static DefaultReferenceModeNaming GetDefaultReferenceModeNamingFromORMModel(ORMModel model, ReferenceModeType referenceModeType)
		{
			if (model != null)
			{
				foreach (DefaultReferenceModeNaming naming in DefaultReferenceModeNamingCustomizesORMModel.GetDefaultReferenceModeNamingCollection(model))
				{
					if (naming.ReferenceModeTargetKind == referenceModeType)
					{
						return naming;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Given an <see cref="ObjectType"/>, determine the default custom format for the specified
		/// <see cref="ReferenceModeType"/> and <see cref="ReferenceModeNamingUse"/> in the associated <see cref="ORMModel"/>
		/// </summary>
		private static string GetDefaultCustomFormatForObjectType(ObjectType objectType, ReferenceModeNamingUse targetUse)
		{
			ORMModel model;
			IReferenceModePattern referenceMode;
			if (null != objectType &&
				null != (model = objectType.Model) &&
				null != (referenceMode = objectType.ReferenceModePattern))
			{
				return GetCustomFormatFromORMModel(model, referenceMode.ReferenceModeType, targetUse);
			}
			return "";
		}
		/// <summary>
		/// Given an <see cref="ORMModel"/>, determine the stored custom format used for reference mode naming
		/// for the given <see cref="ReferenceModeType"/> and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		private static string GetCustomFormatFromORMModel(ORMModel model, ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			DefaultReferenceModeNaming naming = GetDefaultReferenceModeNamingFromORMModel(model, referenceModeType);
			if (naming != null)
			{
				return (targetUse == ReferenceModeNamingUse.ReferenceToEntityType) ? naming.CustomFormat : naming.PrimaryIdentifierCustomFormat;
			}
			switch (referenceModeType)
			{
				case ReferenceModeType.UnitBased:
					return ResourceStrings.ReferenceModeNamingDefaultCustomFormatUnitBased;
				case ReferenceModeType.Popular:
					return ResourceStrings.ReferenceModeNamingDefaultCustomFormatPopular;
				// case ReferenceModeType.General:
				default:
					return ResourceStrings.ReferenceModeNamingDefaultCustomFormatGeneral;

			}
		}
		/// <summary>
		/// Get the default custom format used for reference mode naming of the given <see cref="ReferenceModeType"/>
		/// and <see cref="ReferenceModeNamingUse"/>
		/// </summary>
		private static string GetDefaultCustomFormat(ReferenceModeType referenceModeType, ReferenceModeNamingUse targetUse)
		{
			if (targetUse == ReferenceModeNamingUse.ReferenceToEntityType)
			{
				switch (referenceModeType)
				{
					case ReferenceModeType.UnitBased:
						return ResourceStrings.ReferenceModeNamingDefaultCustomFormatUnitBased;
					case ReferenceModeType.Popular:
						return ResourceStrings.ReferenceModeNamingDefaultCustomFormatPopular;
					// case ReferenceModeType.General:
					default:
						return ResourceStrings.ReferenceModeNamingDefaultCustomFormatGeneral;
				}
			}
			else
			{
				switch (referenceModeType)
				{
					case ReferenceModeType.UnitBased:
						return ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatUnitBased;
					case ReferenceModeType.Popular:
						return ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatPopular;
					// case ReferenceModeType.General:
					default:
						return ResourceStrings.ReferenceModeNamingDefaultPrimaryIdentifierCustomFormatGeneral;
				}
			}
		}
		#endregion // Static helper functions
		#region Instance helper functions
		/// <summary>
		/// Determine if the <see cref="CustomFormat"/> string is currently used
		/// </summary>
		/// <param name="targetUse">The <see cref="ReferenceModeNamingUse"/> to test</param>
		/// <param name="requireFormatString">Set to <see langword="true"/> if a current format string is required.</param>
		/// <returns><see langword="true"/> if the CustomFormat field is currently in use.</returns>
		private bool UsesCustomFormat(ReferenceModeNamingUse targetUse, bool requireFormatString)
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
						IReferenceModePattern referenceMode;
						if (null != (objectType = this.ObjectType) &&
							null != (referenceMode = objectType.ReferenceModePattern))
						{
							return GetNamingChoiceFromORMModel(objectType.Model, referenceMode.ReferenceModeType, targetUse) == EffectiveReferenceModeNamingChoice.CustomFormat;
						}
					}
					break;
			}
			return false;
		}
		#endregion // Instance helper functions
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DefaultReferenceModeNamingFixupListener();
			}
		}
		/// <summary>
		/// Validation DefaultReferenceModeNaming options
		/// </summary>
		private sealed class DefaultReferenceModeNamingFixupListener : DeserializationFixupListener<DefaultReferenceModeNaming>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public DefaultReferenceModeNamingFixupListener()
				: base((int)ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase.ValidateCustomizationOptions)
			{
			}
			/// <summary>
			/// Validate AssimilationMapping options
			/// </summary>
			protected sealed override void ProcessElement(DefaultReferenceModeNaming element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					if (string.IsNullOrEmpty(element.CustomFormat))
					{
						element.CustomFormat = GetDefaultCustomFormat(element.ReferenceModeTargetKind, ReferenceModeNamingUse.ReferenceToEntityType);
					}
					if (string.IsNullOrEmpty(element.PrimaryIdentifierCustomFormat))
					{
						element.PrimaryIdentifierCustomFormat = GetDefaultCustomFormat(element.ReferenceModeTargetKind, ReferenceModeNamingUse.PrimaryIdentifier);
					}
				}
			}
		}
		#endregion // Deserialization Fixup
	}
}
