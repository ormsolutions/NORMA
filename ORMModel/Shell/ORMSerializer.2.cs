#if NEWSERIALIZE
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using SysDiag = System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Query;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;

namespace Northface.Tools.ORM.Shell
{
	#region Public Enumerations
	/// <summary>
	/// Supported operations for element custom serialization.
	/// </summary>
	[Flags]
	[CLSCompliant(true)]
	public enum ORMCustomSerializedElementSupportedOperations
	{
		/// <summary>
		/// No operations are supported.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Child element information is supported.
		/// </summary>
		ChildElementInfo = 0x01,
		/// <summary>
		/// Custom element information is supported.
		/// </summary>
		ElementInfo = 0x02,
		/// <summary>
		/// Custom attribute information is supported.
		/// </summary>
		AttributeInfo = 0x04,
		/// <summary>
		/// Custom link information is supported.
		/// </summary>
		LinkInfo = 0x08,
		/// <summary>
		/// The CustomSerializedChildRoleComparer method is supported
		/// </summary>
		CustomSortChildRoles = 0x10,
		/// <summary>
		/// Set if some of the attributes are written as elements and others are written as attributes.
		/// </summary>
		MixedTypedAttributes = 0x20,
	}
	/// <summary>
	/// Write style for element custom serialization.
	/// </summary>
	[CLSCompliant(true)]
	public enum ORMCustomSerializedElementWriteStyle
	{
		/// <summary>
		/// Dont write.
		/// </summary>
		NotWritten = 0xFF,
		/// <summary>
		/// Write as an element.
		/// </summary>
		Element = 0x00,
		/// <summary>
		/// Write as a double tagged element.
		/// </summary>
		DoubleTaggedElement = 0x01
	}
	/// <summary>
	/// Write style for attribute custom serialization.
	/// </summary>
	[CLSCompliant(true)]
	public enum ORMCustomSerializedAttributeWriteStyle
	{
		/// <summary>
		/// Dont write.
		/// </summary>
		NotWritten = 0xFF,
		/// <summary>
		/// Write as an attribute.
		/// </summary>
		Attribute = 0x00,
		/// <summary>
		/// Write as an element.
		/// </summary>
		Element = 0x01,
		/// <summary>
		/// Write as a double tagged element.
		/// </summary>
		DoubleTaggedElement = 0x02
	}
	#endregion Public Enumerations

	#region Public Classes
	/// <summary>
	/// Custom serialization information.
	/// </summary>
	[CLSCompliant(true)]
	public abstract class ORMCustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedInfo()
		{
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		protected ORMCustomSerializedInfo(string customPrefix, string customName, string customNamespace, string doubleTagName)
		{
			myCustomPrefix = customPrefix;
			myCustomName = customName;
			myCustomNamespace = customNamespace;
			myDoubleTagName = doubleTagName;
		}

		private string myCustomPrefix;
		private string myCustomName;
		private string myCustomNamespace;
		private string myDoubleTagName;

		/// <summary>
		/// The custom prefix to use.
		/// </summary>
		/// <value>The custom prefix to use.</value>
		public string CustomPrefix
		{
			get { return myCustomPrefix; }
			set { myCustomPrefix = value; }
		}
		/// <summary>
		/// The custom name to use.
		/// </summary>
		/// <value>The custom name to use.</value>
		public string CustomName
		{
			get { return myCustomName; }
			set { myCustomName = value; }
		}
		/// <summary>
		/// The custom namespace to use.
		/// </summary>
		/// <value>The custom namespace to use.</value>
		public string CustomNamespace
		{
			get { return myCustomNamespace; }
			set { myCustomNamespace = value; }
		}
		/// <summary>
		/// The name of the double tag.
		/// </summary>
		/// <value>The name of the double tag.</value>
		public string DoubleTagName
		{
			get { return myDoubleTagName; }
			set { myDoubleTagName = value; }
		}
	}
	/// <summary>
	/// Custom serialization information for elements.
	/// </summary>
	[CLSCompliant(true)]
	public class ORMCustomSerializedElementInfo : ORMCustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writing.</param>
		public ORMCustomSerializedElementInfo(ORMCustomSerializedElementWriteStyle writeStyle)
		{
			myWriteStyle = writeStyle;
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		public ORMCustomSerializedElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName) : base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteStyle = writeStyle;
		}

		private ORMCustomSerializedElementWriteStyle myWriteStyle;

		/// <summary>
		/// Default ORMCustomSerializedElementInfo
		/// </summary>
		public static readonly ORMCustomSerializedElementInfo Default = new ORMCustomSerializedElementInfo();

		/// <summary>
		/// The style to use when writting.
		/// </summary>
		/// <value>The style to use when writting.</value>
		public ORMCustomSerializedElementWriteStyle WriteStyle
		{
			get { return myWriteStyle; }
			set { myWriteStyle = value; }
		}
	}
	/// <summary>
	/// Custom serialization information for attributes.
	/// </summary>
	[CLSCompliant(true)]
	public class ORMCustomSerializedAttributeInfo : ORMCustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedAttributeInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writting.</param>
		public ORMCustomSerializedAttributeInfo(ORMCustomSerializedAttributeWriteStyle writeStyle)
		{
			myWriteStyle = writeStyle;
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeCustomStorage">true to write when custom storage.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		public ORMCustomSerializedAttributeInfo(string customPrefix, string customName, string customNamespace, bool writeCustomStorage, ORMCustomSerializedAttributeWriteStyle writeStyle, string doubleTagName) : base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteCustomStorage = writeCustomStorage;
			myWriteStyle = writeStyle;
		}

		private bool myWriteCustomStorage;
		private ORMCustomSerializedAttributeWriteStyle myWriteStyle;

		/// <summary>
		/// Default ORMCustomSerializedAttributeInfo
		/// </summary>
		public static readonly ORMCustomSerializedAttributeInfo Default = new ORMCustomSerializedAttributeInfo();

		/// <summary>
		/// true to write when custom storage.
		/// </summary>
		/// <value>true to write when custom storage.</value>
		public bool WriteCustomStorage
		{
			get { return myWriteCustomStorage; }
			set { myWriteCustomStorage = value; }
		}
		/// <summary>
		/// The style to use when writting.
		/// </summary>
		/// <value>The style to use when writting.</value>
		public ORMCustomSerializedAttributeWriteStyle WriteStyle
		{
			get { return myWriteStyle; }
			set { myWriteStyle = value; }
		}
	}
	/// <summary>
	/// Custom serialization information for child elements.
	/// </summary>
	[CLSCompliant(true)]
	public class ORMCustomSerializedChildElementInfo : ORMCustomSerializedElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedChildElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="childGuids">The child element guids.</param>
		public ORMCustomSerializedChildElementInfo(string customName, Guid[] childGuids) : base(null, customName, null, ORMCustomSerializedElementWriteStyle.Element, null)
		{
			myGuidList = childGuids;
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="childGuids">The child element guids.</param>
		public ORMCustomSerializedChildElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName, Guid[] childGuids) : base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
		{
			myGuidList = childGuids;
		}

		private IList<Guid> myGuidList;

		/// <summary>
		/// Default ORMCustomSerializedChildElementInfo
		/// </summary>
		public new static readonly ORMCustomSerializedChildElementInfo Default = new ORMCustomSerializedChildElementInfo();

		/// <summary>
		/// Test if the list of child elements contains the specified guid
		/// </summary>
		public bool ContainsGuid(Guid guid)
		{
			return myGuidList != null && myGuidList.Contains(guid);
		}
	}
	#endregion Public Classes

	#region Public Interfaces
	/// <summary>
	/// The interface for getting custom element namespaces.
	/// </summary>
	public interface IORMCustomElementNamespace
	{
		/// <summary>
		/// Returns custom element namespaces.
		/// </summary>
		/// <returns>Custom element namespaces.</returns>
		string[,] GetCustomElementNamespaces();
		/// <summary>
		/// Return the default element prefix for elements where the
		/// prefix is not specified
		/// </summary>
		string DefaultElementPrefix { get;}
	}
	/// <summary>
	/// The interface for getting element custom serialization information.
	/// </summary>
	public interface IORMCustomSerializedElement
	{
		/// <summary>
		/// Returns the supported operations.
		/// </summary>
		ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations{get;}
		/// <summary>
		/// Returns custom serialization information for child elements.
		/// </summary>
		/// <returns>Custom serialization information for child elements.</returns>
		ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo();
		/// <summary>
		/// Returns custom serialization information for elements.
		/// </summary>
		ORMCustomSerializedElementInfo CustomSerializedElementInfo { get;}
		/// <summary>
		/// Returns custom serialization information for attributes.
		/// </summary>
		/// <param name="attributeInfo">The attribute info.</param>
		/// <param name="rolePlayedInfo">If this is implemented on a ElementLink-derived class, then the
		/// played role is the role player containing the reference to the opposite role. Always null for a
		/// class element.</param>
		/// <returns>Custom serialization information for attributes.</returns>
		ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo);
		/// <summary>
		/// Returns custom serialization information for links.
		/// </summary>
		/// <param name="rolePlayedInfo">The role played.</param>
		/// <returns>Custom serialization information for links.</returns>
		ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo);
		/// <summary>
		/// Get a comparer to sort custom role elements. Affects the element order
		/// for nested child (aggregated) and link (referenced) elements
		/// </summary>
		IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer {get;}
	}
	#endregion Public Interfaces

	#region New Serialization
	/// <summary>
	///New Serialization
	/// </summary>
	public partial class ORMSerializer
	{
		private System.Collections.Generic.List<Guid> myLinkGUIDs = new System.Collections.Generic.List<Guid>();

		/// <summary>
		/// Used for sorting.
		/// </summary>
		/// <param name="writeStyle">An attribute write style.</param>
		/// <returns>A number to sort with.</returns>
		private static int AttributeWriteStylePriority(ORMCustomSerializedAttributeWriteStyle writeStyle)
		{
			switch (writeStyle)
			{
				case ORMCustomSerializedAttributeWriteStyle.Attribute:
					return 0;
				case ORMCustomSerializedAttributeWriteStyle.Element:
					return 1;
				case ORMCustomSerializedAttributeWriteStyle.DoubleTaggedElement:
					return 2;
			}
			return 3;
		}
		/// <summary>
		/// Used for serializing attributes.
		/// </summary>
		/// <param name="guid">The GUID to convert.</param>
		/// <returns>An XML encoded string.</returns>
		private static string ToXML(System.Guid guid)
		{
			return '_'+System.Xml.XmlConvert.ToString(guid).ToUpper(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Used for serializing attributes.
		/// </summary>
		/// <param name="value">The attribute's value.</param>
		/// <param name="typeConvert">true to type convert the value (value is an object type).</param>
		/// <returns>An XML encoded string.</returns>
		private static string ToXML(object value, bool typeConvert)
		{
			if (value == null)
			{
				return null;
			}
			else if (typeConvert)
			{
				object[] typeConverters = value.GetType().GetCustomAttributes(typeof(TypeConverterAttribute), false);

				if (typeConverters != null && typeConverters.Length != 0)
				{
					Type converterType = Type.GetType(((TypeConverterAttribute)typeConverters[0]).ConverterTypeName, false, false);

					if (converterType != null)
					{
						return ((TypeConverter)Activator.CreateInstance(converterType)).ConvertToInvariantString(value);
					}
				}
			}

			Type type = value.GetType();
			if (type.IsEnum)
			{
				return Enum.GetName(type, value);
			}
			else
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Empty:
					case TypeCode.DBNull:
					{
						return null;
					}
					case TypeCode.DateTime:
					{
						return System.Xml.XmlConvert.ToString((System.DateTime)value);
					}
					case TypeCode.UInt64:
					{
						return System.Xml.XmlConvert.ToString((ulong)value);
					}
					case TypeCode.Int64:
					{
						return System.Xml.XmlConvert.ToString((long)value);
					}
					case TypeCode.UInt32:
					{
						return System.Xml.XmlConvert.ToString((uint)value);
					}
					case TypeCode.Int32:
					{
						return System.Xml.XmlConvert.ToString((int)value);
					}
					case TypeCode.UInt16:
					{
						return System.Xml.XmlConvert.ToString((ushort)value);
					}
					case TypeCode.Int16:
					{
						return System.Xml.XmlConvert.ToString((short)value);
					}
					case TypeCode.Byte:
					{
						return System.Xml.XmlConvert.ToString((byte)value);
					}
					case TypeCode.SByte:
					{
						return System.Xml.XmlConvert.ToString((sbyte)value);
					}
					case TypeCode.Char:
					{
						return System.Xml.XmlConvert.ToString((char)value);
					}
					case TypeCode.Boolean:
					{
						return System.Xml.XmlConvert.ToString((bool)value);
					}
					case TypeCode.Decimal:
					{
						return System.Xml.XmlConvert.ToString((decimal)value);
					}
					case TypeCode.Double:
					{
						return System.Xml.XmlConvert.ToString((double)value);
					}
					case TypeCode.Single:
					{
						return System.Xml.XmlConvert.ToString((float)value);
					}
				}

				if (type==typeof(System.Guid))
				{
					return ToXML((System.Guid)value);
				}
				else if (type==typeof(System.TimeSpan))
				{
					return System.Xml.XmlConvert.ToString((System.TimeSpan)value);
				}

				return value.ToString();
			}
		}
		/// <summary>
		/// Used for serializing child elements.
		/// </summary>
		/// <param name="childElementInfo">The child element info to search.</param>
		/// <param name="guid">The GUID to find.</param>
		/// <returns>An index or -1.</returns>
		private static int FindGuid(ORMCustomSerializedChildElementInfo[] childElementInfo, Guid guid)
		{
			int count = childElementInfo.Length;

			for (int index = 0; index < count; ++index)
			{
				if (childElementInfo[index].ContainsGuid(guid))
				{
					return index;
				}
			}

			return -1;
		}
		/// <summary>
		/// Sorts mixed typed attributes.
		/// </summary>
		/// <param name="customElement">The element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="attributes">The element's attributes.</param>
		private static void SortAttributes(IORMCustomSerializedElement customElement, MetaRoleInfo rolePlayedInfo, ref IList attributes)
		{
			int attrCount = attributes.Count;
			if (attrCount != 0)
			{
				ORMCustomSerializedAttributeInfo[] customInfo = new ORMCustomSerializedAttributeInfo[attrCount];
				int[] indices = new int[attrCount];
				for (int i = 0; i < attrCount; ++i)
				{
					indices[i] = i;
					customInfo[i] = customElement.GetCustomSerializedAttributeInfo((MetaAttributeInfo)attributes[i], rolePlayedInfo);
				}
				Array.Sort<int>(indices, delegate(int index1, int index2)
				{
					ORMCustomSerializedAttributeInfo customInfo1 = customInfo[index1];
					ORMCustomSerializedAttributeInfo customInfo2 = customInfo[index2];
					int ws0 = AttributeWriteStylePriority(customInfo1.WriteStyle);
					int ws1 = AttributeWriteStylePriority(customInfo2.WriteStyle);

					if (ws0 > ws1)
					{
						return 1;
					}
					else if (ws0 < ws1)
					{
						return -1;
					}

					return 0;
				});
				for (int i = 0; i < attrCount; ++i)
				{
					if (indices[i] != i)
					{
						MetaAttributeInfo[] reorderedList = new MetaAttributeInfo[attrCount];
						for (int j = 0; j < attrCount; ++j)
						{
							reorderedList[indices[j]] = (MetaAttributeInfo)attributes[j];
						}
						attributes = reorderedList;
						break;
					}
				}
			}
			return;
		}
		/// <summary>
		/// Writes a customized begin element tag.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="customInfo">The customized tag info.</param>
		/// <param name="defaultPrefix">The default prefix.</param>
		/// <param name="defaultName">The default tag name.</param>
		/// <returns>true if the begin element tag was written.</returns>
		private static bool WriteCustomizedStartElement(System.Xml.XmlWriter file, ORMCustomSerializedElementInfo customInfo, string defaultPrefix, string defaultName)
		{
			if (customInfo!=null)
			{
				switch (customInfo.WriteStyle)
				{
					case ORMCustomSerializedElementWriteStyle.NotWritten:
					{
						return false;
					}
					case ORMCustomSerializedElementWriteStyle.DoubleTaggedElement:
					{
						string prefix = (customInfo.CustomPrefix != null ? customInfo.CustomPrefix : defaultPrefix);
						string name = (customInfo.CustomName != null ? customInfo.CustomName : defaultName);

						file.WriteStartElement
						(
							prefix,
							name,
							customInfo.CustomNamespace
						);
						file.WriteStartElement
						(
							prefix,
							customInfo.DoubleTagName != null ? customInfo.DoubleTagName : name,
							customInfo.CustomNamespace
						);

						return true;
					}
				}

				file.WriteStartElement
				(
					customInfo.CustomPrefix != null ? customInfo.CustomPrefix : defaultPrefix,
					customInfo.CustomName != null ? customInfo.CustomName : defaultName,
					customInfo.CustomNamespace
				);
			}
			else
			{
				file.WriteStartElement(defaultPrefix, defaultName, null);
			}
			return true;
		}
		/// <summary>
		/// Writes a customized end element tag.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="customInfo">The customized tag info.</param>
		private static void WriteCustomizedEndElement(System.Xml.XmlWriter file, ORMCustomSerializedElementInfo customInfo)
		{
			if (customInfo!=null)
			{
				switch (customInfo.WriteStyle)
				{
#if DEBUG
					case ORMCustomSerializedElementWriteStyle.NotWritten:
					{
						System.Diagnostics.Debug.Fail("WriteCustomizedEndElement - ORMCustomSerializedElementWriteStyle.DontWrite");
						throw new System.InvalidOperationException();
					}
#endif
					case ORMCustomSerializedElementWriteStyle.DoubleTaggedElement:
					{
						file.WriteEndElement();
						break;
					}
				}
			}

			file.WriteEndElement();

			return;
		}
		/// <summary>
		/// Get the default prefix for an element from the meta model containing the element
		/// </summary>
		private static string DefaultElementPrefix(ModelElement element)
		{
			string retVal = null;
			IORMCustomElementNamespace parentModel = element.Store.SubStores[element.MetaClass.MetaModel.Id] as IORMCustomElementNamespace;
			if (parentModel != null)
			{
				retVal = parentModel.DefaultElementPrefix;
			}
			return retVal;
		}
		/// <summary>
		/// Serializes an attribute.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="customElement">The element as a custom element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="attribute">The element's attribute to write.</param>
		/// <param name="isCustomAttribute">true if the attribute has custom info.</param>
		private static void SerializeAttribute(System.Xml.XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, MetaRoleInfo rolePlayedInfo, MetaAttributeInfo attribute, bool isCustomAttribute)
		{
			if (!isCustomAttribute)
			{
				if (!attribute.CustomStorage)
				{
					file.WriteAttributeString
					(
						attribute.Name,
						ToXML(element.GetAttributeValue(attribute), false)
					);
				}
				return;
			}

			ORMCustomSerializedAttributeInfo customInfo = customElement.GetCustomSerializedAttributeInfo(attribute, rolePlayedInfo);

			if (!attribute.CustomStorage || customInfo.WriteCustomStorage)
			{
				if (customInfo.WriteStyle!=ORMCustomSerializedAttributeWriteStyle.Attribute || file.WriteState != System.Xml.WriteState.Element)
				{
					switch (customInfo.WriteStyle)
					{
						default:
						{
							file.WriteElementString
							(
								customInfo.CustomPrefix != null ? customInfo.CustomPrefix : DefaultElementPrefix(element),
								customInfo.CustomName != null ? customInfo.CustomName : attribute.Name,
								customInfo.CustomNamespace,
								ToXML(element.GetAttributeValue(attribute), attribute.CustomStorage)
							);
							break;
						}
						case ORMCustomSerializedAttributeWriteStyle.NotWritten:
						{
							break;
						}
						case ORMCustomSerializedAttributeWriteStyle.DoubleTaggedElement:
						{
							string prefix = (customInfo.CustomPrefix!= null?customInfo.CustomPrefix:DefaultElementPrefix(element));
							string name = (customInfo.CustomName!= null?customInfo.CustomName:attribute.Name);

							file.WriteStartElement
							(
								prefix,
								name,
								customInfo.CustomNamespace
							);
							file.WriteElementString
							(
								prefix,
								customInfo.DoubleTagName != null ? customInfo.DoubleTagName : name,
								customInfo.CustomNamespace,
								ToXML(element.GetAttributeValue(attribute), attribute.CustomStorage)
							);
							file.WriteEndElement();

							break;
						}
					}
				}
				else
				{
					file.WriteAttributeString
					(
						customInfo.CustomPrefix,
						customInfo.CustomName != null ? customInfo.CustomName : attribute.Name,
						customInfo.CustomNamespace,
						ToXML(element.GetAttributeValue(attribute), attribute.CustomStorage)
					);
				}
			}

			return;
		}
		/// <summary>
		/// Serializes all attributes of an element.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="customElement">The element as a custom element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="attributes">The element's attributes.</param>
		/// <param name="hasCustomAttributes">true if the element has attributes with custom info.</param>
		private static void SerializeAttributes(System.Xml.XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, MetaRoleInfo rolePlayedInfo, IList attributes, bool hasCustomAttributes)
		{
			for (int index = 0, count = attributes.Count; index < count; ++index)
			{
				MetaAttributeInfo attribute = (MetaAttributeInfo)attributes[index];

				SerializeAttribute
				(
					file,
					element,
					customElement,
					rolePlayedInfo,
					attribute,
					hasCustomAttributes
				);
			}
			return;
		}
		/// <summary>
		/// Serializes a link.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="link">The link.</param>
		/// <param name="rolePlayer">The role player.</param>
		/// <param name="oppositeRolePlayer">The opposite role player.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="directLink">true to write as a direct link.</param>
		private void SerializeLink(System.Xml.XmlWriter file, ElementLink link, ModelElement rolePlayer, ModelElement oppositeRolePlayer, MetaRoleInfo rolePlayedInfo, bool directLink)
		{
			ORMCustomSerializedElementSupportedOperations supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
			ORMCustomSerializedElementInfo customInfo = ORMCustomSerializedElementInfo.Default;
			IORMCustomSerializedElement customElement;
			IList attributes = null;
			string defaultPrefix;
			bool hasCustomAttributes = false;

			if (link != null)
			{
				if (!ShouldSerialize(link)) return;
				customElement = link as IORMCustomSerializedElement;
				attributes = link.MetaClass.MetaAttributes;
				defaultPrefix = DefaultElementPrefix(link);
			}
			else
			{
				if (!ShouldSerialize(rolePlayer)) return;
				customElement = rolePlayer as IORMCustomSerializedElement;
				defaultPrefix = DefaultElementPrefix(rolePlayer);
			}

			if (customElement != null)
			{
				supportedOperations = customElement.SupportedCustomSerializedOperations;

				if (0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes) && attributes != null)
				{
					SortAttributes(customElement, rolePlayedInfo, ref attributes);
				}
				hasCustomAttributes = (supportedOperations & ORMCustomSerializedElementSupportedOperations.AttributeInfo) != 0;

				IORMCustomSerializedElement tagCustomElement = customElement;
				if (link != null)
				{
					tagCustomElement = rolePlayer as IORMCustomSerializedElement;
					if (tagCustomElement != null)
					{
						if (0 != (tagCustomElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo))
						{
							customInfo = tagCustomElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole);
						}
					}
				}
				else if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) != 0)
				{
					customInfo = customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole);
				}
			}

			System.Text.StringBuilder name = new System.Text.StringBuilder();
			name.Append(rolePlayedInfo.MetaRelationship.Name);
			name.Append('.');
			name.Append(rolePlayedInfo.OppositeMetaRole.Name);
			if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, name.ToString())) return;

			Guid keyId = (link != null) ? link.Id : oppositeRolePlayer.Id;
			if (!directLink && !myLinkGUIDs.Contains(keyId))
			{
				IList rolesPlayed = link.MetaClass.MetaRolesPlayed;
				bool writeChildren;

				myLinkGUIDs.Add(keyId);

				if (writeChildren=((link == null) || rolesPlayed.Count != 0))
				{
					file.WriteAttributeString("id", ToXML(keyId));
				}
				if (link != null)
				{
					if (oppositeRolePlayer == null)
					{
						oppositeRolePlayer = link.GetRolePlayer(rolePlayedInfo.OppositeMetaRole);
					}
					file.WriteAttributeString("ref", ToXML(oppositeRolePlayer.Id));
				}

				SerializeAttributes(file, link, customElement, rolePlayedInfo, attributes, hasCustomAttributes);

				if (writeChildren)
				{
					ORMCustomSerializedChildElementInfo[] childElementInfo;
					bool groupRoles;

					childElementInfo = ((groupRoles = (0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.ChildElementInfo))) ? customElement.GetCustomSerializedChildElementInfo() : null);

					//write children
					SerializeChildElements(file, link, customElement, childElementInfo, rolesPlayed, 0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles), groupRoles, defaultPrefix);
				}
			}
			else
			{
				file.WriteAttributeString("ref", ToXML(keyId));
			}

			WriteCustomizedEndElement(file, customInfo);

			return;
		}
		/// <summary>
		/// Serializes a child element. (usually a Collection element)
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="childElement">The child element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="customInfo">The custom element info.</param>
		/// <param name="defaultPrefix">The default prefix.</param>
		/// <param name="defaultName">The default element name.</param>
		/// <returns>true if the begin element tag was written.</returns>
		private bool SerializeChildElement(System.Xml.XmlWriter file, ModelElement childElement, MetaRoleInfo rolePlayedInfo, ORMCustomSerializedElementInfo customInfo, string defaultPrefix, string defaultName)
		{
			IList children = childElement.GetCounterpartRolePlayers(rolePlayedInfo.OppositeMetaRole, rolePlayedInfo);
			int childCount = children.Count;

			if (childCount < 1)
			{
				return false;
			}
			bool writeStartElement = (defaultName != null);

			for (int iChild = 0; iChild < childCount; ++iChild)
			{
				ModelElement child = (ModelElement)children[iChild];

				if (ShouldSerialize(child))
				{
					if (customInfo == null)
					{
						defaultPrefix = DefaultElementPrefix(child);
					}
					if (!SerializeElement(file, child, customInfo, defaultPrefix, ref defaultName))
					{
						return false;
					}
				}
			}

			return (writeStartElement && defaultName==null);
		}
		/// <summary>
		/// Serializes a child element.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="childElement">The child element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="oppositeRoleInfo">The opposite role being played.</param>
		/// <param name="customInfo">The custom element info.</param>
		/// <param name="defaultPrefix">The default prefix.</param>
		/// <param name="writeBeginElement">true to write the begin element tag.</param>
		/// <returns>true if the begin element tag was written.</returns>
		private bool SerializeChildElement(System.Xml.XmlWriter file, ModelElement childElement, MetaRoleInfo rolePlayedInfo, MetaRoleInfo oppositeRoleInfo, ORMCustomSerializedElementInfo customInfo, string defaultPrefix, bool writeBeginElement)
		{
			bool ret = false;

			if (!rolePlayedInfo.IsAggregate && !oppositeRoleInfo.IsAggregate) //write link
			{
				if (rolePlayedInfo.MetaRelationship.MetaAttributesCount > 0) //link has attributes
				{
					IList links = childElement.GetElementLinks(rolePlayedInfo);

					foreach (ElementLink link in links)
					{
						if (link.MetaClass.Id == rolePlayedInfo.MetaRelationship.Id)
						{
							if (writeBeginElement && customInfo != null)
							{
								if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, customInfo.CustomName))
								{
									return false;
								}
								ret = true;
							}
							SerializeLink(file, link, childElement, null, rolePlayedInfo, false); //write indirect link
							break;
						}
					}
				}
				else //link does not have attributes
				{
					IList oppositeElements = childElement.GetCounterpartRolePlayers(rolePlayedInfo, oppositeRoleInfo);

					//write direct links
					if (oppositeElements.Count != 0)
					{
						if (writeBeginElement && customInfo != null)
						{
							if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, customInfo.CustomName))
							{
								return false;
							}
							ret = true;
						}
						foreach (ModelElement oppositeElement in oppositeElements)
						{
							SerializeLink(file, null, childElement, oppositeElement, rolePlayedInfo, true);
						}
					}
				}
			}
			else if (rolePlayedInfo.IsAggregate) //write child
			{
				return SerializeChildElement(file, childElement, oppositeRoleInfo, customInfo, defaultPrefix, writeBeginElement ? oppositeRoleInfo.Name : null);
			}

			return ret;
		}
		private void SerializeChildElements(System.Xml.XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, ORMCustomSerializedChildElementInfo[] childElementInfo, IList rolesPlayed, bool sortRoles, bool groupRoles, string defaultPrefix)
		{
			int rolesPlayedCount = rolesPlayed.Count;

			//sort played roles
			if (sortRoles && rolesPlayedCount != 0)
			{
				IComparer<MetaRoleInfo> comparer = customElement.CustomSerializedChildRoleComparer;
				if (comparer != null)
				{
					MetaRoleInfo[] sortedRoles = new MetaRoleInfo[rolesPlayedCount];
					rolesPlayed.CopyTo(sortedRoles, 0);
					Array.Sort(sortedRoles, comparer);
					rolesPlayed = sortedRoles;
				}
			}

			//write children
			if (groupRoles)
			{
				bool[] written = new bool[rolesPlayedCount];

				for (int index0 = 0; index0 < rolesPlayedCount; ++index0)
				{
					if (!written[index0])
					{
						MetaRoleInfo rolePlayedInfo = (MetaRoleInfo)rolesPlayed[index0];
						MetaRoleInfo oppositeRoleInfo = rolePlayedInfo.OppositeMetaRole;
						ORMCustomSerializedChildElementInfo customChildInfo;
						bool writeEndElement = false;

						int childIndex = FindGuid(childElementInfo, oppositeRoleInfo.Id);
						customChildInfo = (childIndex >= 0) ? childElementInfo[childIndex] : null;
						string defaultChildPrefix = (customChildInfo != null) ? defaultPrefix : null;

						written[index0] = true;
						if (SerializeChildElement(file, element, rolePlayedInfo, oppositeRoleInfo, customChildInfo, defaultChildPrefix, true))
						{
							writeEndElement = true;
						}

						if (customChildInfo != null)
						{
							for (int index1 = index0 + 1; index1 < rolesPlayedCount; ++index1)
							{
								if (!written[index1])
								{
									rolePlayedInfo = (MetaRoleInfo)rolesPlayed[index1];
									oppositeRoleInfo = rolePlayedInfo.OppositeMetaRole;

									if (customChildInfo.ContainsGuid(oppositeRoleInfo.Id))
									{
										written[index1] = true;
										if (SerializeChildElement(file, element, rolePlayedInfo, oppositeRoleInfo, customChildInfo, defaultChildPrefix, !writeEndElement))
										{
											writeEndElement = true;
										}
									}
								}
							}
						}

						if (writeEndElement)
						{
							WriteCustomizedEndElement(file, customChildInfo);
						}
					}
				}
			}
			else
			{
				for (int index = 0; index < rolesPlayedCount; ++index)
				{
					MetaRoleInfo rolePlayedInfo = (MetaRoleInfo)rolesPlayed[index];
					if (SerializeChildElement(file, element, rolePlayedInfo, rolePlayedInfo.OppositeMetaRole, null, null, true))
					{
						WriteCustomizedEndElement(file, null);
					}
				}
			}

			return;
		}
		/// <summary>
		/// Recursivly serializes elements.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="containerCustomInfo">The container element's custom serialization information.</param>
		/// <param name="containerPrefix">The container element's prefix.</param>
		/// <param name="containerName">The container element's name.</param>
		/// <returns>false if the container element was not written.</returns>
		private bool SerializeElement(System.Xml.XmlWriter file, ModelElement element, ORMCustomSerializedElementInfo containerCustomInfo, string containerPrefix, ref string containerName)
		{
			if (!ShouldSerialize(element)) return true;
			ORMCustomSerializedElementSupportedOperations supportedOperations;
			ORMCustomSerializedChildElementInfo[] childElementInfo = null;
			MetaClassInfo classInfo = element.MetaClass;
			ORMCustomSerializedElementInfo customInfo;
			IORMCustomSerializedElement customElement = element as IORMCustomSerializedElement;
			IList attributes = classInfo.AllMetaAttributes;
			IList rolesPlayed = classInfo.AllMetaRolesPlayed;
			string defaultPrefix = DefaultElementPrefix(element);
			bool roleGrouping = false;
			bool isCustom = (customElement != null);

			//load custom information
			if (isCustom)
			{
				supportedOperations = customElement.SupportedCustomSerializedOperations;

				if (0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes))
				{
					SortAttributes(customElement, null, ref attributes);
				}
				if (roleGrouping = (0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.ChildElementInfo)))
				{
					childElementInfo = customElement.GetCustomSerializedChildElementInfo();
				}
				if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.ElementInfo) != 0)
				{
					customInfo = customElement.CustomSerializedElementInfo;
					if (customInfo.WriteStyle == ORMCustomSerializedElementWriteStyle.NotWritten) return true;
				}
				else
				{
					customInfo = ORMCustomSerializedElementInfo.Default;
				}
			}
			else
			{
				supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
				customInfo = ORMCustomSerializedElementInfo.Default;
			}

			//write container begin element
			if (containerName != null)
			{
				if (!WriteCustomizedStartElement(file, containerCustomInfo, containerPrefix, containerName))
				{
					return false;
				}
				containerName = null;
			}

			//write begin element tag
			if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, classInfo.Name)) return true;
			file.WriteAttributeString("id", ToXML(element.Id));

			//write attributes
			SerializeAttributes
			(
				file,
				element,
				customElement,
				null,
				attributes,
				(supportedOperations & ORMCustomSerializedElementSupportedOperations.AttributeInfo) != 0
			);

			//write children
			SerializeChildElements(file, element, customElement, childElementInfo, rolesPlayed, 0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles), roleGrouping, defaultPrefix);

			//write end element tag
			WriteCustomizedEndElement(file, customInfo);

			return true;
		}
		/// <summary>
		/// Recursivly serializes elements.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		private void SerializeElement(System.Xml.XmlWriter file, ModelElement element)
		{
			string containerName = null;
			SerializeElement(file, element, null, null, ref containerName);
			return;
		}
		/// <summary>
		/// New XML Serialization
		/// </summary>
		public void Save2(Stream stream)
		{
			System.Xml.XmlWriterSettings xmlSettings = new XmlWriterSettings();
			System.Xml.XmlWriter file;
			Store store = myStore;
			ModelElement[] currentElements;
			ICollection values = store.SubStores.Values;
			int count;

			xmlSettings.IndentChars = "\t";
			xmlSettings.Indent = true;

			file = System.Xml.XmlWriter.Create(stream, xmlSettings);
			file.WriteStartElement("ormRoot", "ORM2", "http://Schemas.Northface.edu/ORM/ORMRoot");

			//serialize namespaces
			foreach (object value in values)
			{
				IORMCustomElementNamespace ns = value as IORMCustomElementNamespace;

				if (ns!=null)
				{
					string[,] namespaces = ns.GetCustomElementNamespaces();

					count = namespaces.GetLength(0);
					for (int index = 0; index < count; ++index)
					{
						//if (/*namespaces[index].Length==2 && */namespaces[index,0] != null && namespaces[index,1] != null)
						file.WriteAttributeString("xmlns", namespaces[index,0], null, namespaces[index,1]);
					}
				}
			}

			//serialize ORM model
			currentElements = GenerateElementArray(new IList[1] { store.ElementDirectory.GetElements(ORMModel.MetaClassGuid) });
			count = currentElements.Length;
			for (int i = 0; i < count; ++i)
			{
				SerializeElement(file, currentElements[i]);
			}

			//serialize ORM diagram
			currentElements = GenerateElementArray(new IList[1] { store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid) });
			count = currentElements.Length;
			for (int i = 0; i < count; ++i)
			{
				SerializeElement(file, currentElements[i]);
			}

			file.WriteEndElement();
			file.Close();

			myLinkGUIDs.Clear();

			return;
		}
	}
	#endregion New Serialization
}
#endif // NEWSERIALIZE