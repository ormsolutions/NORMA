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

//#define DEBUG_CONVERTER_TRANSFORMS
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.Win32;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// A class used to read XmlConverters section of the designer settings file
	/// and run transforms between registered converter types.
	/// </summary>
	[CLSCompliant(false)]
	public partial class ORMDesignerSettings
	{
		#region Schema definition classes
		#region ORMDesignerSchema class
		private static class ORMDesignerSchema
		{
			#region String Constants
			public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/DesignerSettings";
			public const string SettingsElement = "settings";

			public const string XmlConvertersElement = "xmlConverters";
			public const string XmlConverterElement = "xmlConverter";
			public const string XmlExtensionConverter = "xmlExtensionConverter";
			public const string TransformFileAttribute = "transformFile";
			public const string SourceElementAttribute = "sourceElement";
			public const string SourceNamespaceAttribute = "sourceNamespace";
			public const string TargetElementAttribute = "targetElement";
			public const string TargetNamespaceAttribute = "targetNamespace";
			public const string DescriptionAttribute = "description";

			public const string TransformParameterElement = "transformParameter";
			public const string NameAttribute = "name";
			public const string ValueAttribute = "value";
			public const string DynamicTypeAttribute = "dynamicType";
			public const string DynamicValuesExclusiveAttribute = "dynamicValuesExclusive";

			public const string DynamicValueElement = "dynamicValue";

			public const string ExtensionClassElement = "extensionClass";
			public const string RunsWithAttribute = "runsWithSourceNamespace";
			public const string XslNamespaceAttribute = "xslNamespace";
			public const string ClassNameAttribute = "className";
			#endregion // String Constants
			#region Static properties
			private static ORMDesignerNameTable myNames;
			public static ORMDesignerNameTable Names
			{
				get
				{
					ORMDesignerNameTable retVal = myNames;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myNames;
							if (retVal == null)
							{
								retVal = myNames = new ORMDesignerNameTable();
							}
						}
					}
					return retVal;
				}
			}
			private static XmlReaderSettings myReaderSettings;
			public static XmlReaderSettings ReaderSettings
			{
				get
				{
					XmlReaderSettings retVal = myReaderSettings;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myReaderSettings;
							if (retVal == null)
							{
								retVal = myReaderSettings = new XmlReaderSettings();
								retVal.ValidationType = ValidationType.Schema;
								retVal.Schemas.Add(SchemaNamespace, new XmlTextReader(typeof(ORMDesignerSettings).Assembly.GetManifestResourceStream(typeof(ORMDesignerSettings), "ORMDesignerSettings.xsd")));
								retVal.NameTable = Names;
							}
						}
					}
					return retVal;
				}
			}
			#endregion // Static properties
		}
		#endregion // ORMDesignerSchema class
		#region ORMDesignerNameTable class
		private sealed class ORMDesignerNameTable : NameTable
		{
			public readonly string SchemaNamespace;
			public readonly string SettingsElement;
			public readonly string XmlConvertersElement;
			public readonly string XmlConverterElement;
			public readonly string XmlExtensionConverterElement;
			public readonly string TransformFileAttribute;
			public readonly string SourceElementAttribute;
			public readonly string SourceNamespaceAttribute;
			public readonly string TargetElementAttribute;
			public readonly string TargetNamespaceAttribute;
			public readonly string DescriptionAttribute;
			public readonly string TransformParameterElement;
			public readonly string NameAttribute;
			public readonly string ValueAttribute;
			public readonly string DynamicTypeAttribute;
			public readonly string DynamicValuesExclusiveAttribute;
			public readonly string DynamicValueElement;
			public readonly string ExtensionClassElement;
			public readonly string RunsWithAttribute;
			public readonly string XslNamespaceAttribute;
			public readonly string ClassNameAttribute;

			public ORMDesignerNameTable()
				: base()
			{
				SchemaNamespace = Add(ORMDesignerSchema.SchemaNamespace);
				SettingsElement = Add(ORMDesignerSchema.SettingsElement);
				XmlConvertersElement = Add(ORMDesignerSchema.XmlConvertersElement);
				XmlConverterElement = Add(ORMDesignerSchema.XmlConverterElement);
				XmlExtensionConverterElement = Add(ORMDesignerSchema.XmlExtensionConverter);
				TransformFileAttribute = Add(ORMDesignerSchema.TransformFileAttribute);
				SourceElementAttribute = Add(ORMDesignerSchema.SourceElementAttribute);
				SourceNamespaceAttribute = Add(ORMDesignerSchema.SourceNamespaceAttribute);
				TargetElementAttribute = Add(ORMDesignerSchema.TargetElementAttribute);
				TargetNamespaceAttribute = Add(ORMDesignerSchema.TargetNamespaceAttribute);
				DescriptionAttribute = Add(ORMDesignerSchema.DescriptionAttribute);
				TransformParameterElement = Add(ORMDesignerSchema.TransformParameterElement);
				NameAttribute = Add(ORMDesignerSchema.NameAttribute);
				ValueAttribute = Add(ORMDesignerSchema.ValueAttribute);
				ExtensionClassElement = Add(ORMDesignerSchema.ExtensionClassElement);
				RunsWithAttribute = Add(ORMDesignerSchema.RunsWithAttribute);
				DynamicTypeAttribute = Add(ORMDesignerSchema.DynamicTypeAttribute);
				DynamicValuesExclusiveAttribute = Add(ORMDesignerSchema.DynamicValuesExclusiveAttribute);
				DynamicValueElement = Add(ORMDesignerSchema.DynamicValueElement);
				XslNamespaceAttribute = Add(ORMDesignerSchema.XslNamespaceAttribute);
				ClassNameAttribute = Add(ORMDesignerSchema.ClassNameAttribute);
			}
		}
		#endregion // ORMDesignerNameTable class
		#endregion // Schema definition classes
		#region Constants
		private const string REGISTRYKEY_CORESETTINGS = "Core";
		private const string REGISTRYVALUE_SETTINGSFILE = "SettingsFile";
		private const string REGISTRYVALUE_CONVERTERSDIR = "ConvertersDir";
		#endregion // Constraints
		#region Member Variables
		private Package myPackage;
		private string myRootRegistryKey; // Relative to the package application root
		private bool myIsLoaded;
		private Dictionary<XmlElementIdentifier, LinkedList<TransformNode>> myXmlConverters;
		#endregion // Member Variables
		#region Static Variables
		private static readonly object LockObject = new object();
		#endregion // Static Variables
		#region Constructors
		/// <summary>
		/// Construct new designer settings
		/// </summary>
		/// <param name="package">The context package</param>
		/// <param name="rootKey">The root key for designer settings, relative to the package registry root.</param>
		public ORMDesignerSettings(Package package, string rootKey)
		{
			myPackage = package;
			myRootRegistryKey = rootKey;
		}
		#endregion // Constructors
		#region ConvertStream method
		/// <summary>
		/// Convert the given stream to a new stream with converted contents.
		/// Regardless of the action taken, the starting stream is returned
		/// at its original position.
		/// </summary>
		/// <param name="stream">The starting stream</param>
		/// <param name="serviceProvider">The context service provider</param>
		/// <returns>A new stream with modified content. The caller is responsible for disposing the new stream.</returns>
		public Stream ConvertStream(Stream stream, IServiceProvider serviceProvider)
		{
			EnsureGlobalSettingsLoaded();
			if (myXmlConverters == null)
			{
				return null;
			}
			long startingPosition = stream.Position;
			try
			{
				XmlReaderSettings readerSettings = new XmlReaderSettings();
				readerSettings.CloseInput = false;
				LinkedList<TransformNode> nodes = null;
				using (XmlReader reader = XmlReader.Create(stream, readerSettings))
				{
					reader.MoveToContent();
					if (reader.NodeType == XmlNodeType.Element)
					{
						string namespaceURI = reader.NamespaceURI;
						string localName = reader.LocalName;
						if (namespaceURI != ORMSerializationEngine.RootXmlNamespace || localName != ORMSerializationEngine.RootXmlElementName)
						{
							myXmlConverters.TryGetValue(new XmlElementIdentifier(namespaceURI, localName), out nodes);
						}
						else
						{
							if (reader.MoveToFirstAttribute())
							{
								do
								{
									string prefix = reader.Prefix;
									if ("xmlns" == (string.IsNullOrEmpty(prefix) ? reader.Value : prefix))
									{
										//test for any extension Converters to be run
										if (myXmlConverters.TryGetValue(new XmlElementIdentifier(reader.Value), out nodes))
										{
											break;
										}
									}
								} while (reader.MoveToNextAttribute());
							}
						}
					}
				}
				if (nodes != null)
				{
					TransformNode node = nodes.First.Value;
					if (nodes.Count > 1 || node.HasDynamicParameters)
					{
						IUIService uiService;
						if (null != serviceProvider &&
							null != (uiService = (IUIService)serviceProvider.GetService(typeof(IUIService))))
						{
							node = ImportStepOptions.GetTransformOptions(uiService, nodes);
							if (node == null)
							{
								throw new OperationCanceledException();
							}
						}
					}
#if RUNSWITHSOURCEIDENTIFIER
					// UNDONE: Support RunsWithSourceIdentifier option for import
					XmlElementIdentifier runsWithSource = node.RunsWithSourceIdentifier;
					//if needs to be run with another converter then keep track of the source element.
					if (!string.IsNullOrEmpty(runsWithSource.NamespaceURI))
					{
						//logic if needs to be run with another converter
					}
#endif // RUNSWITHSOURCEIDENTIFIER
					stream.Position = startingPosition;
					using (XmlReader reader = XmlReader.Create(stream, readerSettings))
					{
						MemoryStream outputStream = new MemoryStream();
						XmlWriterSettings writerSettings = new XmlWriterSettings();
						writerSettings.CloseOutput = false;
						using (XmlWriter writer = XmlWriter.Create(outputStream, writerSettings))
						{
							node.Transform.Transform(reader, node.Arguments, writer);
						}
						outputStream.Position = 0;
						Stream nextStream = ConvertStream(outputStream, serviceProvider);
						if (nextStream != null)
						{
							outputStream.Dispose();
							return nextStream;
						}
						return outputStream;
					}
				}
			}
			finally
			{
				stream.Position = startingPosition;
			}
			return null;
		}
		#endregion // ConvertStream method
		#region Global Settings Loader
		/// <summary>
		/// Structure representing a single settings file
		/// </summary>
		private struct SettingsLocation
		{
			/// <summary>
			/// The settings file to load
			/// </summary>
			public readonly string SettingsFile;
			/// <summary>
			/// The directory for converter transforms
			/// </summary>
			public readonly string ConvertersDirectory;
			private SettingsLocation(string settingsFile, string convertersDirectory)
			{
				SettingsFile = settingsFile;
				ConvertersDirectory = convertersDirectory;
			}
			/// <summary>
			/// Enumerate all registered settings files
			/// </summary>
			/// <param name="package">The context package, provides a starting key for registry information</param>
			/// <param name="registryRoot">The root key relative to the package root</param>
			/// <returns>Enumeration of settings files</returns>
			public static IEnumerable<SettingsLocation> SettingsLocations(Package package, string registryRoot)
			{
				RegistryKey applicationRegistryRoot = null;
				RegistryKey settingsRegistryRoot = null;
				try
				{
					applicationRegistryRoot = package.ApplicationRegistryRoot;
					settingsRegistryRoot = applicationRegistryRoot.OpenSubKey(registryRoot, RegistryKeyPermissionCheck.ReadSubTree);
					if (settingsRegistryRoot != null)
					{
						string[] settingsKeyNames = settingsRegistryRoot.GetSubKeyNames();
						int settingsCount = (settingsKeyNames == null) ? 0 : settingsKeyNames.Length;
						if (settingsCount > 1)
						{
							// Treat the Core key specially to get a default converters directory
							int coreIndex = Array.IndexOf<string>(settingsKeyNames, REGISTRYKEY_CORESETTINGS);
							if (coreIndex > 0)
							{
								string swap = settingsKeyNames[0];
								settingsKeyNames[0] = settingsKeyNames[coreIndex];
								settingsKeyNames[coreIndex] = swap;
							}
						}
						if (settingsCount != 0 && settingsKeyNames[0] == REGISTRYKEY_CORESETTINGS)
						{
							string defaultConvertersDirectory = null;
							for (int i = 0; i < settingsCount; ++i)
							{
								using (RegistryKey settingsKey = settingsRegistryRoot.OpenSubKey(settingsKeyNames[i], RegistryKeyPermissionCheck.ReadSubTree))
								{
									string settingsFile = settingsKey.GetValue(REGISTRYVALUE_SETTINGSFILE, "", RegistryValueOptions.None) as string;
									if (settingsFile != null && File.Exists(settingsFile))
									{
										string convertersDirectory = settingsKey.GetValue(REGISTRYVALUE_CONVERTERSDIR, "", RegistryValueOptions.None) as string;
										if (string.IsNullOrEmpty(convertersDirectory))
										{
											if (i == 0)
											{
												break; // Invalid registration, Core information required
											}
											convertersDirectory = defaultConvertersDirectory;
										}
										if (Directory.Exists(convertersDirectory))
										{
											if (i == 0)
											{
												defaultConvertersDirectory = convertersDirectory;
											}
											yield return new SettingsLocation(settingsFile, convertersDirectory);
										}
										else if (i == 0)
										{
											break; // Invalid registration, Core information required
										}
									}
								}
							}
						}
					}
				}
				finally
				{
					if (applicationRegistryRoot != null)
					{
						applicationRegistryRoot.Close();
					}
					if (settingsRegistryRoot != null)
					{
						settingsRegistryRoot.Close();
					}
				}
			}
		}
		private void EnsureGlobalSettingsLoaded()
		{
			if (myIsLoaded)
			{
				return;
			}
			myIsLoaded = true;
			ORMDesignerNameTable names = ORMDesignerSchema.Names;
			foreach (SettingsLocation location in SettingsLocation.SettingsLocations(myPackage, myRootRegistryKey))
			{
				string convertersDirectory = location.ConvertersDirectory;
				using (FileStream designerSettingsStream = new FileStream(location.SettingsFile, FileMode.Open, FileAccess.Read))
				{
					using (XmlTextReader settingsReader = new XmlTextReader(new StreamReader(designerSettingsStream), names))
					{
						using (XmlReader reader = XmlReader.Create(settingsReader, ORMDesignerSchema.ReaderSettings))
						{
							if (XmlNodeType.Element == reader.MoveToContent())
							{
								if (TestElementName(reader.NamespaceURI, names.SchemaNamespace) && TestElementName(reader.LocalName, names.SettingsElement))
								{
									while (reader.Read())
									{
										XmlNodeType nodeType = reader.NodeType;
										if (nodeType == XmlNodeType.Element)
										{
											if (TestElementName(reader.LocalName, names.XmlConvertersElement))
											{
												ProcessXmlConverters(reader, names, convertersDirectory);
											}
											else
											{
												Debug.Fail("Validating reader should have failed");
												PassEndElement(reader);
											}
										}
										else if (nodeType == XmlNodeType.EndElement)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private void ProcessXmlConverters(XmlReader reader, ORMDesignerNameTable names, string convertersDirectory)
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (TestElementName(reader.LocalName, names.XmlConverterElement) ||
						TestElementName(reader.LocalName, names.XmlExtensionConverterElement))
					{
						ProcessXmlConverter(reader, names, convertersDirectory);
					}
					else
					{
						Debug.Fail("Validating reader should have failed");
						PassEndElement(reader);
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}
		}
		#region Helper Classes
		#region XmlElementIdentifier Structure
		private struct XmlElementIdentifier
		{
			#region Member Variables
			/// <summary>
			/// The namespace for the element. Can be an empty string.
			/// </summary>
			private string myNamespaceURI;
			/// <summary>
			/// The local name of the element. Cannot be an empty string.
			/// </summary>
			private string myLocalName;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create an element identifier based on a known namespace without an ElementName (String.Empty)
			/// </summary>
			/// <param name="namespaceURI">The namespace for the element. Can be an empty string.</param>
			public XmlElementIdentifier(string namespaceURI)
			{
				myNamespaceURI = namespaceURI;
				myLocalName = String.Empty;
			}
			/// <summary>
			/// Create an element identifier based on a known namespace and unqualified element name
			/// </summary>
			/// <param name="namespaceURI">The namespace for the element. Can be an empty string.</param>
			/// <param name="localName">The local name of the element. Cannot be an empty string.</param>
			public XmlElementIdentifier(string namespaceURI, string localName)
			{
				myNamespaceURI = namespaceURI;
				myLocalName = localName;
			}
			/// <summary>
			/// Parse the provided element name to find the namespaceURI and local name for the element
			/// </summary>
			/// <param name="elementName">A qualified xml name</param>
			/// <param name="reader">A reader used to resolve namespace prefixes</param>
			public XmlElementIdentifier(string elementName, XmlReader reader)
			{
				int colonIndex = elementName.IndexOf(':');
				if (colonIndex != -1)
				{
					myLocalName = elementName.Substring(colonIndex + 1);
					myNamespaceURI = reader.LookupNamespace(elementName.Substring(0, colonIndex));
				}
				else
				{
					myNamespaceURI = "";
					myLocalName = elementName;
				}
			}
			#endregion // Constructors
			#region Accessor Functions
			/// <summary>
			/// The namespace for the element. Can be an empty string.
			/// </summary>
			public string NamespaceURI
			{
				get
				{
					return myNamespaceURI;
				}
			}
			/// <summary>
			/// The local name of the element. Cannot be an empty string.
			/// </summary>
			public string LocalName
			{
				get
				{
					return myLocalName;
				}
			}
			#endregion // Accessor Functions
		}
		#endregion // XmlElementIdentifier Structure
		#region DynamicParameter class
		private abstract class DynamicParameter : PropertyDescriptor
		{
			#region Public methods
			/// <summary>
			/// Create a new <see cref="DynamicParameter"/> and advance the reader to the
			/// end of the parent transformParameter element.
			/// </summary>
			/// <param name="reader">The <see cref="XmlReader"/> with a selection on a transformParameter element</param>
			/// <param name="names">The nametable to reference</param>
			/// <param name="name">The parameter name (already read from the reader)</param>
			/// <param name="defaultValue">The parameter default value (already read from the reader)</param>
			/// <param name="dynamicType">The type of dynamic parameter (already read from the reader)</param>
			/// <returns>A new DynamicParameter, or <see langword="null"/> if the value cannot be established.</returns>
			public static DynamicParameter Create(XmlReader reader, ORMDesignerNameTable names, string name, string defaultValue, string dynamicType)
			{
				DynamicParameter retVal = null;
				string description = reader.GetAttribute(names.DescriptionAttribute);
				string dynamicValuesExclusiveString = reader.GetAttribute(names.DynamicValuesExclusiveAttribute);
				bool dynamicValuesExclusive = (dynamicValuesExclusiveString == null) ? true : XmlConvert.ToBoolean(dynamicValuesExclusiveString);
				switch (dynamicType)
				{
					case "string":
						retVal = new DynamicStringParameter(name, description, dynamicValuesExclusive, defaultValue);
						break;
					case "number":
						retVal = new DynamicNumericParameter(name, description, dynamicValuesExclusive, defaultValue);
						break;
					case "boolean":
						retVal = new DynamicBooleanParameter(name, description, dynamicValuesExclusive, defaultValue);
						break;
					default:
						PassEndElement(reader);
						break;
				}
				if (retVal != null && !reader.IsEmptyElement)
				{
					while (reader.Read())
					{
						XmlNodeType nodeType = reader.NodeType;
						if (nodeType == XmlNodeType.Element)
						{
							string localName = reader.LocalName;
							if (TestElementName(reader.LocalName, names.DynamicValueElement))
							{
								retVal.AddExclusiveValue(reader.GetAttribute(names.ValueAttribute));
							}
							PassEndElement(reader);
						}
						else if (nodeType == XmlNodeType.EndElement)
						{
							break;
						}
					}
				}
				return retVal;
			}
			#endregion // Public methods
			#region Member variables
			private string myName;
			private string myDescription;
			private bool myValuesExclusive;
			private object myDefaultValue;
			private object myCurrentValue;
			private IList<object> myValues;
			#endregion // Member variables
			#region Constructors
			protected DynamicParameter(string name, string description, bool valuesExclusive, object defaultValue)
				: base(name, null)
			{
				myName = name;
				myDefaultValue = defaultValue;
				myCurrentValue = defaultValue;
				myDescription = description ?? "";
				myValuesExclusive = valuesExclusive;
			}
			#endregion // Constructors
			#region Methods
			private void AddExclusiveValue(string value)
			{
				if (value == null)
				{
					return;
				}
				object typedValue = ParseValue(value);
				IList<object> values = myValues;
				if (values == null)
				{
					myValues = values = new List<object>();
				}
				values.Add(typedValue);
			}
			/// <summary>
			/// Interpret a string value as the given type
			/// </summary>
			protected abstract object ParseValue(string value);
			#endregion // Methods
			#region Typed classes
			private abstract class TypedDynamicParameter<TValue> : DynamicParameter
			{
				protected TypedDynamicParameter(string name, string description, bool valuesExclusive, TValue defaultValue)
					: base(name, description, valuesExclusive, defaultValue)
				{
				}
				public override Type PropertyType
				{
					get
					{
						return typeof(TValue);
					}
				}
			}
			private sealed class DynamicStringParameter : TypedDynamicParameter<string>
			{
				public DynamicStringParameter(string name, string description, bool valuesExclusive, string defaultValue)
					: base(name, description, valuesExclusive, defaultValue)
				{
				}
				protected override object ParseValue(string value)
				{
					return value;
				}
			}
			private sealed class DynamicBooleanParameter : TypedDynamicParameter<bool>
			{
				public DynamicBooleanParameter(string name, string description, bool valuesExclusive, string defaultValue)
					: base(name, description, valuesExclusive, XmlConvert.ToBoolean(defaultValue))
				{
				}
				protected override object ParseValue(string value)
				{
					return XmlConvert.ToBoolean(value);
				}
			}
			private sealed class DynamicNumericParameter : TypedDynamicParameter<double>
			{
				public DynamicNumericParameter(string name, string description, bool valuesExclusive, string defaultValue)
					: base(name, description, valuesExclusive, XmlConvert.ToDouble(defaultValue))
				{
				}
				protected override object ParseValue(string value)
				{
					return XmlConvert.ToDouble(value);
				}
			}
			#endregion // Typed classes
			#region PropertyDescriptor Implementation
			public override bool CanResetValue(object component)
			{
				return true;
			}
			public override Type ComponentType
			{
				get
				{
					return typeof(object);
				}
			}
			public override object GetValue(object component)
			{
				return myCurrentValue;
			}
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public override void ResetValue(object component)
			{
				myCurrentValue = myDefaultValue;
			}
			public override void SetValue(object component, object value)
			{
				myCurrentValue = value;
			}
			public override bool ShouldSerializeValue(object component)
			{
				return !myCurrentValue.Equals(myDefaultValue);
			}
			public override string Description
			{
				get
				{
					return myDescription;
				}
			}
			public override TypeConverter Converter
			{
				get
				{
					if (myValues != null)
					{
						return new AddStandardValuesConverter(TypeDescriptor.GetConverter(PropertyType), myValues, myValuesExclusive);
					}
					return base.Converter;
				}
			}
			private class AddStandardValuesConverter : TypeConverter
			{
				#region Member Variables and Constructor
				private TypeConverter myInner;
				private IList<object> myStandardValues;
				private bool myStandardValuesExclusive;
				private StandardValuesCollection myValuesCollection;
				public AddStandardValuesConverter(TypeConverter innerConverter, IList<object> standardValues, bool standardValuesExclusive)
				{
					myInner = innerConverter;
					myStandardValues = standardValues;
					myStandardValuesExclusive = standardValuesExclusive;
				}
				#endregion // Member Variables and Constructor
				#region Forward all overrides
				public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
				{
					return myInner.CanConvertFrom(context, sourceType);
				}
				public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
				{
					return myInner.CanConvertTo(context, destinationType);
				}
				public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
				{
					return myInner.ConvertFrom(context, culture, value);
				}
				public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
				{
					return myInner.ConvertTo(context, culture, value, destinationType);
				}
				public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
				{
					return myInner.CreateInstance(context, propertyValues);
				}
				public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
				{
					return myInner.GetCreateInstanceSupported(context);
				}
				public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
				{
					return myInner.GetProperties(context, value, attributes);
				}
				public override bool GetPropertiesSupported(ITypeDescriptorContext context)
				{
					return myInner.GetPropertiesSupported(context);
				}
				public override bool IsValid(ITypeDescriptorContext context, object value)
				{
					return myInner.IsValid(context, value);
				}
				public override string ToString()
				{
					return myInner.ToString();
				}
				#endregion // Forward all overrides
				#region Standard values handling
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					StandardValuesCollection retVal = myValuesCollection;
					if (retVal == null)
					{
						IList<object> values = myStandardValues;
						object[] valuesArray = new object[values.Count];
						values.CopyTo(valuesArray, 0);
						myValuesCollection = retVal = new StandardValuesCollection(valuesArray);
					}
					return retVal;
				}
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return myStandardValuesExclusive;
				}
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
				#endregion // Standard values handling
			}
			#endregion // Property Descriptor Implementation
		}
		#endregion // DynamicParameter class
		#region TransformNode Class
		private class TransformNode
		{
			#region Member Variables
			private string myDescription;
			private XmlElementIdentifier myRunsWithSourceIdentifier;
			private XslCompiledTransform myTransform;
			private string myTransformFile;
			private XsltArgumentList myArguments;
			private XmlElementIdentifier myTargetElement;
			private IList<DynamicParameter> myDynamicParameters;
			#endregion // Member Variables
			#region Constructors
			public TransformNode(XmlElementIdentifier targetElement, string description, string transformFile, XsltArgumentList arguments, IList<DynamicParameter> dynamicParameters, XmlElementIdentifier runsWithSourceIdentifier)
			{
				myTargetElement = targetElement;
				myDescription = description;
				myTransform = null;
				myTransformFile = transformFile;
				myArguments = arguments;
				myDynamicParameters = dynamicParameters;
				myRunsWithSourceIdentifier = runsWithSourceIdentifier;
			}
			#endregion // Constructors
			#region Accessor Functions
			/// <summary>
			/// Description of the transform
			/// </summary>
			public string Description
			{
				get
				{
					return myDescription;
				}
			}
			private class XmlFileResolver : XmlUrlResolver
			{
				private Uri myBaseUri;
				public XmlFileResolver(string baseFile)
				{
					myBaseUri = new Uri(baseFile, UriKind.Absolute);
				}
				public override Uri ResolveUri(Uri baseUri, string relativeUri)
				{
					return base.ResolveUri((baseUri == null) ? myBaseUri : baseUri, relativeUri);
				}
			}
			/// <summary>
			/// The transform to apply
			/// </summary>
			public XslCompiledTransform Transform
			{
				get
				{
					XslCompiledTransform retVal = myTransform;
					if (retVal == null)
					{
#if DEBUG_CONVERTER_TRANSFORMS 
						retVal = new XslCompiledTransform(true);
#else
						retVal = new XslCompiledTransform();
#endif
						retVal.Load(myTransformFile, XsltSettings.TrustedXslt, new XmlFileResolver(myTransformFile));
						myTransform = retVal;
					}
					return retVal;
				}
			}
			/// <summary>
			/// The element identifier for the expected output
			/// </summary>
			public XmlElementIdentifier TargetElement
			{
				get
				{
					return myTargetElement;
				}
			}
			/// <summary>
			/// The argument list to pass to the transform
			/// </summary>
			public XsltArgumentList Arguments
			{
				get
				{
					return myArguments;
				}
			}
			/// <summary>
			/// Return true if dynamic parameters are available for this node
			/// </summary>
			public bool HasDynamicParameters
			{
				get
				{
					return myDynamicParameters != null;
				}
			}
			/// <summary>
			/// The source namespace of the xmlExtensionConverter to include
			/// this xmlExtensionConverter in
			/// </summary>
			public XmlElementIdentifier RunsWithSourceIdentifier
			{
				get
				{
					return myRunsWithSourceIdentifier;
				}
			}
			#endregion // Accessor Functions
			#region Public methods
			/// <summary>
			/// Dynamic parameter values may have changed, synchronize the
			/// current arguments
			/// </summary>
			public void SynchronizeArguments()
			{
				IList<DynamicParameter> parameters = myDynamicParameters;
				XsltArgumentList arguments = myArguments;
				if (parameters != null)
				{
					int count = parameters.Count;
					for (int i = 0; i < count; ++i)
					{
						DynamicParameter parameter = parameters[i];
						string name = parameter.Name;
						arguments.RemoveParam(name, "");
						arguments.AddParam(name, "", parameter.GetValue(null));
					}
				}
			}
			#endregion // Public methods
			#region TypeDescriptor implementation
			public ICustomTypeDescriptor CreateDynamicParametersTypeDescriptor()
			{
				IList<DynamicParameter> parameters = myDynamicParameters;
				return (parameters != null && parameters.Count != 0) ? new DynamicParametersTypeDescriptor(this) : null;
			}
			private class DynamicParametersTypeDescriptor : ICustomTypeDescriptor
			{
				#region Member Variables and Constructors
				private PropertyDescriptorCollection myProperties;
				private TransformNode myContextNode;
				public DynamicParametersTypeDescriptor(TransformNode contextNode)
				{
					myContextNode = contextNode;
					IList<DynamicParameter> parameters = contextNode.myDynamicParameters;
					int count = parameters.Count;
					PropertyDescriptor[] descriptors = new PropertyDescriptor[count];
					for (int i = 0; i < count; ++i)
					{
						descriptors[i] = parameters[i];
					}
					myProperties = new PropertyDescriptorCollection(descriptors);
				}
				#endregion // Member Variables and Constructors
				#region ICustomTypeDescriptor Implementation
				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					return null;
				}
				string ICustomTypeDescriptor.GetClassName()
				{
					return "";
				}
				string ICustomTypeDescriptor.GetComponentName()
				{
					return "";
				}
				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					return null;
				}
				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					return null;
				}
				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					return null;
				}
				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					return null;
				}
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					return null;
				}
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					return null;
				}

				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					return myProperties;
				}

				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					return myProperties;
				}
				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					return myContextNode;
				}
				#endregion // ICustomTypeDescriptor Implementation
			}
			#endregion // TypeDescriptor implementation
		}
		#endregion // TransformNode Class
		#endregion // Helper Classes
		private void ProcessXmlConverter(XmlReader reader, ORMDesignerNameTable names, string convertersDirectory)
		{
			if (myXmlConverters == null)
			{
				myXmlConverters = new Dictionary<XmlElementIdentifier, LinkedList<TransformNode>>();
			}
			string transformFile = reader.GetAttribute(names.TransformFileAttribute);
			string description = reader.GetAttribute(names.DescriptionAttribute);

			XmlElementIdentifier sourceIdentifier;
			XmlElementIdentifier targetIdentifier;
			XmlElementIdentifier runsWithSourceIdentifier;
			XsltArgumentList arguments = null;
			IList<DynamicParameter> dynamicParameters = null;

			if (TestElementName(reader.LocalName, names.XmlConverterElement))
			{
				string sourceElement = reader.GetAttribute(names.SourceElementAttribute);
				string targetElement = reader.GetAttribute(names.TargetElementAttribute);

				sourceIdentifier = new XmlElementIdentifier(sourceElement, reader);
				targetIdentifier = new XmlElementIdentifier(targetElement, reader);
				runsWithSourceIdentifier = default(XmlElementIdentifier);

			}
			else // XmlExtensionConverterElement
			{
				string sourceNamespace = reader.GetAttribute(names.SourceNamespaceAttribute);
				string targetNamespace = reader.GetAttribute(names.TargetNamespaceAttribute);
				string runsWith = reader.GetAttribute(names.RunsWithAttribute);

				sourceIdentifier = new XmlElementIdentifier(sourceNamespace);
				targetIdentifier = new XmlElementIdentifier(targetNamespace);
				runsWithSourceIdentifier = !string.IsNullOrEmpty(runsWith) ? new XmlElementIdentifier(runsWith) : default(XmlElementIdentifier);
			}

			if (!reader.IsEmptyElement)
			{
				while (reader.Read())
				{
					XmlNodeType nodeType = reader.NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						string localName = reader.LocalName;
						if (TestElementName(localName, names.TransformParameterElement))
						{
							// Add an argument for the transform
							if (arguments == null)
							{
								arguments = new XsltArgumentList();
							}
							string paramName = reader.GetAttribute(names.NameAttribute);
							string paramValue = reader.GetAttribute(names.ValueAttribute);
							arguments.AddParam(paramName, "", paramValue);
							DynamicParameter dynamicParameter = null;
							string dynamicType = reader.GetAttribute(names.DynamicTypeAttribute);
							if (dynamicType != null)
							{
								dynamicParameter = DynamicParameter.Create(reader, names, paramName, paramValue, dynamicType);
								if (dynamicParameter != null)
								{
									if (dynamicParameters == null)
									{
										dynamicParameters = new List<DynamicParameter>();
									}
									dynamicParameters.Add(dynamicParameter);
								}
							}
							else
							{
								PassEndElement(reader);
							}
						}
						else if (TestElementName(localName, names.ExtensionClassElement))
						{
							// Load an extension class and associate it with an extension namespace
							// used by the transform
							if (arguments == null)
							{
								arguments = new XsltArgumentList();
							}
							arguments.AddExtensionObject(reader.GetAttribute(names.XslNamespaceAttribute), Type.GetType(reader.GetAttribute(names.ClassNameAttribute), true, false).GetConstructor(Type.EmptyTypes).Invoke(new object[0]));
							PassEndElement(reader);
						}
						else
						{
							Debug.Fail("Validating reader should have failed");
							PassEndElement(reader);
						}
					}
					else if (nodeType == XmlNodeType.EndElement)
					{
						break;
					}
				}
			}
			TransformNode transformNode = new TransformNode(targetIdentifier, description, Path.Combine(convertersDirectory, transformFile), arguments, dynamicParameters, runsWithSourceIdentifier);
			LinkedList<TransformNode> nodes;
			if (myXmlConverters.TryGetValue(sourceIdentifier, out nodes))
			{
				nodes.AddLast(new LinkedListNode<TransformNode>(transformNode));
			}
			else
			{
				nodes = new LinkedList<TransformNode>();
				nodes.AddFirst(new LinkedListNode<TransformNode>(transformNode));
				myXmlConverters.Add(sourceIdentifier, nodes);
			}
		}
		private static bool TestElementName(string localName, string elementName)
		{
			return (object)localName == (object)elementName;
		}
		/// <summary>
		/// Move the reader to the node immediately after the end element corresponding to the current open element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
		private static void PassEndElement(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				bool finished = false;
				while (!finished && reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							PassEndElement(reader);
							break;

						case XmlNodeType.EndElement:
							finished = true;
							break;
					}
				}
			}
		}
		#endregion // Global Settings Loader
	}
}
