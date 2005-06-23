#if NEWSERIALIZE
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Query;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
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
		DoubleTaggedElement = 0x01,
		/// <summary>
		/// Used for links. Write as an element, but write the link
		/// id, attributes, and referencing child elements at this location.
		/// </summary>
		PrimaryLinkElement = 0x02,
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
	/// <summary>
	/// An enum used for deserialization to determine if
	/// an element name and namespace is recognized by a
	/// custom serialized element.
	/// </summary>
	[CLSCompliant(true)]
	public enum ORMCustomSerializedElementMatchStyle
	{
		/// <summary>
		/// The element is not recognized, don't process it
		/// </summary>
		None,
		/// <summary>
		/// The element matched an attribute written out as an element.
		/// The DoubleTageName property (if it is not null) specifies the
		/// double tag name (the tag inside this element where the attribute
		/// data is stored). The guid identifying the MetaAttributeInfo is
		/// returned in the Guid property.
		/// </summary>
		Attribute,
		/// <summary>
		/// The element matches a singled contained role. The guid identifying
		/// the MetaRoleInfo is returned in the SingleOppositeMetaRoleGuid property.
		/// </summary>
		SingleOppositeMetaRole,
		/// <summary>
		/// The element matches more than one contained role. The guids identifying
		/// the roles are returned in the OppositeMetaRoleGuidCollection property
		/// </summary>
		MultipleOppositeMetaRoles,
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
	/// <summary>
	/// Data returned by IORMCustomSerializedElement.MapElementName.
	/// </summary>
	[CLSCompliant(true)]
	public struct ORMCustomSerializedElementMatch
	{
		private Guid mySingleGuid;
		private Guid[] myMultiGuids;
		private ORMCustomSerializedElementMatchStyle myMatchStyle;
		private string myDoubleTagName;
		/// <summary>
		/// The element was recognized as a meta attribute.
		/// </summary>
		/// <param name="metaAttributeGuid">The guid identifying the meta attribute</param>
		/// <param name="doubleTagName">the name of the double tag, if any</param>
		public void InitializeAttribute(Guid metaAttributeGuid, string doubleTagName)
		{
			mySingleGuid = metaAttributeGuid;
			myMatchStyle = ORMCustomSerializedElementMatchStyle.Attribute;
			myDoubleTagName = (doubleTagName != null && doubleTagName.Length != 0) ? doubleTagName : null;
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="oppositeMetaRoleGuids">1 or more opposite meta role guids</param>
		public void InitializeRoles(params Guid[] oppositeMetaRoleGuids)
		{
			Debug.Assert(oppositeMetaRoleGuids != null && oppositeMetaRoleGuids.Length != 0);
			if (oppositeMetaRoleGuids.Length == 1)
			{
				mySingleGuid = oppositeMetaRoleGuids[0];
				myMultiGuids = null;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeMetaRole;
			}
			else
			{
				mySingleGuid = Guid.Empty;
				myMultiGuids = oppositeMetaRoleGuids;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRoles;
			}
			myDoubleTagName = null;
		}
		/// <summary>
		/// The guid identifying the meta attribute. Valid for a match
		/// style of Attribute.
		/// </summary>
		public Guid MetaAttributeGuid
		{
			get
			{
				return (myMatchStyle == ORMCustomSerializedElementMatchStyle.Attribute) ? mySingleGuid : Guid.Empty;
			}
		}
		/// <summary>
		/// The guid identifying the opposite meta role if there is only
		/// one matching meta role. Valid for a match style of SingleOppositeMetaRole.
		/// </summary>
		public Guid SingleOppositeMetaRoleGuid
		{
			get
			{
				return (myMatchStyle == ORMCustomSerializedElementMatchStyle.SingleOppositeMetaRole) ? mySingleGuid : Guid.Empty;
			}
		}
		/// <summary>
		/// The guids identifying multiple opposite meta roles. Valid for a match
		/// style of MultipleOppositeMetaRoles.
		/// </summary>
		[CLSCompliant(false)]
		public IList<Guid> OppositeMetaRoleGuidCollection
		{
			get
			{
				return (myMatchStyle == ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRoles) ? myMultiGuids : null;
			}
		}
		/// <summary>
		/// The type of element match
		/// </summary>
		/// <value>ORMCustomSerializedElementMatchStyle</value>
		public ORMCustomSerializedElementMatchStyle MatchStyle
		{
			get
			{
				return myMatchStyle;
			}
		}
		/// <summary>
		/// The double tag name for an attribute. null if the MatchStyle
		/// is not Attribute or if there is no double tag for this element.
		/// </summary>
		public string DoubleTagName
		{
			get
			{
				return myDoubleTagName;
			}
		}
	}
	#endregion Public Classes

	#region Public Interfaces
	/// <summary>
	/// The interface for getting custom element namespaces.
	/// </summary>
	public interface IORMCustomSerializedMetaModel
	{
		/// <summary>
		/// Return all namespaces used by custom elements in this model.
		/// </summary>
		/// <returns>Custom element namespaces. return value [*, 0] contains
		/// the prefix and [*, 1] contains the associated xml namespace</returns>
		string[,] GetCustomElementNamespaces();
		/// <summary>
		/// Return the default element prefix for elements where the
		/// prefix is not specified
		/// </summary>
		string DefaultElementPrefix { get;}
		/// <summary>
		/// Determine if a class or relationship should be serialized. This allows
		/// the serialization engine to do a meta-level sanity check before retrieving
		/// elements, and reduces the number of 'NotWritten' elements in the
		/// serialization specification file.
		/// </summary>
		/// <param name="store">The store to check</param>
		/// <param name="classInfo">The class or relationship to test</param>
		/// <returns>true if the element should be serialized</returns>
		bool ShouldSerializeMetaClass(Store store, MetaClassInfo classInfo);
		/// <summary>
		/// Map an xml namespace name and element name to a meta class guid
		/// </summary>
		/// <param name="xmlNamespace">The namespace of a top-level element (directly
		/// inside the ORM2 tag)</param>
		/// <param name="elementName">The name of the element to match</param>
		/// <returns>The guid of a MetaClassInfo, or Guid.Empty if not recognized</returns>
		Guid MapRootElement(string xmlNamespace, string elementName);
		/// <summary>
		/// Map an xml namespace name and element name to a meta class guid
		/// </summary>
		/// <param name="xmlNamespace">The namespace of the xml element</param>
		/// <param name="elementName">The name of the element to match</param>
		/// <returns>A meta class guid, or Guid.Empty if the name is not recognized</returns>
		Guid MapClassName(string xmlNamespace, string elementName);
	}
	/// <summary>
	/// The interface for getting element custom serialization information.
	/// </summary>
	public interface IORMCustomSerializedElement
	{
		/// <summary>
		/// Returns the supported operations.
		/// </summary>
		ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations { get;}
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
		IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer { get;}
		/// <summary>
		/// Attempt to map an element name to a custom serialized child element.
		/// </summary>
		/// <param name="elementNamespace">The full xml namespace of the element to match. Note
		/// that using prefixes is not robust, so the full namespace needs to be specified.</param>
		/// <param name="elementName">The local name of the element</param>
		/// <param name="containerNamespace">The full xml namespace of the container to match. A
		/// container element is an element with no id or ref parameter.</param>
		/// <param name="containerName">The local name of the container</param>
		/// <returns>ORMCustomSerializedElementMatch. Use the MatchStyle property to determine levels of success.</returns>
		ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName);
		/// <summary>
		/// Attempt to map an attribute name to a custom serialized attribute
		/// for this element.
		/// </summary>
		/// <param name="xmlNamespace">The full xml namespace of the element to match. Note
		/// that using prefixes is not robust, so the full namespace needs to be specified.</param>
		/// <param name="attributeName">The local name of the attribute</param>
		/// <returns>A MetaAttributeGuid, or Guid.Empty. Use Guid.IsEmpty to test.</returns>
		Guid MapAttribute(string xmlNamespace, string attributeName);
	}
	#endregion Public Interfaces
	#region New Serialization
	/// <summary>
	///New Serialization
	/// </summary>
	public partial class ORMSerializer
	{
		/// <summary>
		/// The standard prefix for the prefix used on the root node of the ORM document
		/// </summary>
		public const string RootXmlPrefix = "ormRoot";
		/// <summary>
		/// The tag name for the element used as the root node of the ORM document
		/// </summary>
		public const string RootXmlElementName = "ORM2";
		/// <summary>
		/// The namespace for the root node of the ORM document
		/// </summary>
		public const string RootXmlNamespace = "http://Schemas.Northface.edu/ORM/ORMRoot";

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
			return '_' + System.Xml.XmlConvert.ToString(guid).ToUpper(CultureInfo.InvariantCulture);
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

				if (type == typeof(System.Guid))
				{
					return ToXML((System.Guid)value);
				}
				else if (type == typeof(System.TimeSpan))
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
			if (customInfo != null)
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
			if (customInfo != null)
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
		/// Find the parent model for this element.
		/// </summary>
		/// <param name="element">A ModelElement being serialized</param>
		/// <returns>IORMCustomSerializedMetaModel, or null</returns>
		private static IORMCustomSerializedMetaModel GetParentModel(ModelElement element)
		{
			return element.Store.SubStores[element.MetaClass.MetaModel.Id] as IORMCustomSerializedMetaModel;
		}
		/// <summary>
		/// Determine based on the type of role and opposite role player if any elements of
		/// the given type should be serialized.
		/// </summary>
		/// <param name="parentModel">The parent model of an element</param>
		/// <param name="role">The role played</param>
		/// <returns>true if serialization should continue</returns>
		private static bool ShouldSerializeMetaRole(IORMCustomSerializedMetaModel parentModel, MetaRoleInfo role)
		{
			if (parentModel == null)
			{
				return true;
			}
			Store store = ((SubStore)parentModel).Store;
			return parentModel.ShouldSerializeMetaClass(store, role.MetaRelationship) && parentModel.ShouldSerializeMetaClass(store, role.OppositeMetaRole.RolePlayer);
		}
		/// <summary>
		/// Get the default prefix for an element from the meta model containing the element
		/// </summary>
		private static string DefaultElementPrefix(ModelElement element)
		{
			string retVal = null;
			IORMCustomSerializedMetaModel parentModel = GetParentModel(element);
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
				if (customInfo.WriteStyle != ORMCustomSerializedAttributeWriteStyle.Attribute || file.WriteState != System.Xml.WriteState.Element)
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
								string prefix = (customInfo.CustomPrefix != null ? customInfo.CustomPrefix : DefaultElementPrefix(element));
								string name = (customInfo.CustomName != null ? customInfo.CustomName : attribute.Name);

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
		/// <param name="rolePlayedInfo">The role being played.</param>
		private void SerializeLink(System.Xml.XmlWriter file, ElementLink link, ModelElement rolePlayer, MetaRoleInfo rolePlayedInfo)
		{
			ModelElement oppositeRolePlayer = link.GetRolePlayer(rolePlayedInfo.OppositeMetaRole);
			ORMCustomSerializedElementSupportedOperations supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
			ORMCustomSerializedElementInfo customInfo = ORMCustomSerializedElementInfo.Default;
			IList attributes = null;
			string defaultPrefix;
			bool hasCustomAttributes = false;

			if (!ShouldSerialize(link) || !ShouldSerialize(rolePlayer))
			{
				return;
			}
			IORMCustomSerializedElement rolePlayerCustomElement = rolePlayer as IORMCustomSerializedElement;
			IORMCustomSerializedElement customElement = rolePlayerCustomElement;
			bool writeContents = customElement != null &&
				0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) &&
				customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole).WriteStyle == ORMCustomSerializedElementWriteStyle.PrimaryLinkElement;

			if (writeContents)
			{
				customElement = link as IORMCustomSerializedElement;
				attributes = link.MetaClass.MetaAttributes;
				defaultPrefix = DefaultElementPrefix(link);
			}
			else
			{
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
				if (writeContents)
				{
					tagCustomElement = rolePlayerCustomElement;
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

			if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, string.Concat(rolePlayedInfo.MetaRelationship.Name, ".", rolePlayedInfo.OppositeMetaRole.Name))) return;

			Guid keyId = writeContents ? link.Id : oppositeRolePlayer.Id;
			if (writeContents)
			{
				IList rolesPlayed = link.MetaClass.MetaRolesPlayed;
				bool writeChildren = rolesPlayed.Count != 0;

				if (writeChildren)
				{
					// UNDONE: Be smarter here. If none of the relationships for the played
					// roles are actually serialized, then we don't need this at all.
					file.WriteAttributeString("id", ToXML(keyId));
				}
				file.WriteAttributeString("ref", ToXML(oppositeRolePlayer.Id));

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

			return (writeStartElement && defaultName == null);
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
				IList links = childElement.GetElementLinks(rolePlayedInfo);

				foreach (ElementLink link in links)
				{
					if (writeBeginElement && !ret && customInfo != null)
					{
						if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, customInfo.CustomName))
						{
							return false;
						}
						ret = true;
					}
					SerializeLink(file, link, childElement, rolePlayedInfo);
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
			IORMCustomSerializedMetaModel parentModel = GetParentModel(element);

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
						if (!ShouldSerializeMetaRole(parentModel, rolePlayedInfo))
						{
							written[index0] = true;
							continue;
						}
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
									if (!ShouldSerializeMetaRole(parentModel, rolePlayedInfo))
									{
										written[index1] = true;
										continue;
									}
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
					if (!ShouldSerializeMetaRole(parentModel, rolePlayedInfo))
					{
						continue;
					}
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

			xmlSettings.IndentChars = "\t";
			xmlSettings.Indent = true;

			file = System.Xml.XmlWriter.Create(stream, xmlSettings);
			file.WriteStartElement(RootXmlPrefix, RootXmlElementName, RootXmlNamespace);

			//serialize namespaces
			foreach (object value in values)
			{
				IORMCustomSerializedMetaModel ns = value as IORMCustomSerializedMetaModel;

				if (ns != null)
				{
					string[,] namespaces = ns.GetCustomElementNamespaces();

					for (int index = 0, count = namespaces.GetLength(0); index < count; ++index)
					{
						//if (/*namespaces[index].Length==2 && */namespaces[index,0] != null && namespaces[index,1] != null)
						file.WriteAttributeString("xmlns", namespaces[index, 0], null, namespaces[index, 1]);
					}
				}
			}

			//serialize all root elements
			currentElements = RootElements;
			for (int i = 0, count = currentElements.Length; i < count; ++i)
			{
				SerializeElement(file, currentElements[i]);
			}

			file.WriteEndElement();
			file.Close();

			return;
		}
	}
	#endregion New Serialization
	#region New Deserialization
	public partial class ORMSerializer
	{
		private Dictionary<string, Guid> myCustomIdToGuidMap;
		private INotifyElementAdded myNotifyAdded;
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		/// <param name="fixupManager">Class used to perfom fixup operations
		/// after the load is complete.</param>
		public void Load2(Stream stream, DeserializationFixupManager fixupManager)
		{
			// Leave rules on so all of the links reconnect. Links are not saved.
			RulesSuspended = true;
			try
			{
				myNotifyAdded = fixupManager as INotifyElementAdded;
				ICollection substores = myStore.SubStores.Values;
				NameTable nameTable = new NameTable();
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.NameTable = nameTable;
				// UNDONE: Set XsdValidate and add schemas here
				using (XmlTextReader xmlReader = new XmlTextReader(new StreamReader(stream), nameTable))
				{
					using (XmlReader reader = XmlReader.Create(xmlReader, settings))
					{
						while (reader.Read())
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								// UNDONE: The name check will go away if a validating reader is loaded
								if (!reader.IsEmptyElement && reader.NamespaceURI == RootXmlNamespace && reader.LocalName == RootXmlElementName)
								{
									while (reader.Read())
									{
										XmlNodeType nodeType = reader.NodeType;
										if (nodeType == XmlNodeType.Element)
										{
											bool processedRootElement = false;
											foreach (object substore in substores)
											{
												IORMCustomSerializedMetaModel metaModel = substore as IORMCustomSerializedMetaModel;
												if (metaModel != null)
												{
													Guid classGuid = metaModel.MapRootElement(reader.NamespaceURI, reader.LocalName);
													if (!classGuid.Equals(Guid.Empty))
													{
														processedRootElement = true;
														ProcessClassElement(reader, metaModel, CreateElement(reader.GetAttribute("id"), null, classGuid));
														break;
													}
												}
											}
											if (!processedRootElement)
											{
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
//				XmlSerialization.DeserializeStore(
//					myStore,
//					stream,
//					MajorVersion,
//					MinorVersion,
//					new XmlSerialization.UpgradeFileFormat(UpgradeFileFormat),
//					(fixupManager == null) ? null : new XmlSerialization.Deserialized((fixupManager as INotifyElementAdded).ElementAdded));
				if (fixupManager != null)
				{
					fixupManager.DeserializationComplete();
				}
			}
			finally
			{
				RulesSuspended = false;
			}
		}
		/// <summary>
		/// Process a newly created element. The element will have an
		/// Id set only. The id and ref attributes should be ignored.
		/// </summary>
		/// <param name="reader">Reader set to the root node</param>
		/// <param name="customModel">The custom serialized meta model</param>
		/// <param name="element">Newly created element</param>
		private void ProcessClassElement(XmlReader reader, IORMCustomSerializedMetaModel customModel, ModelElement element)
		{
			IORMCustomSerializedElement customElement = element as IORMCustomSerializedElement;
			MetaDataDirectory dataDir = myStore.MetaDataDirectory;

			#region Attribute processing
			// Process all attributes first
			if (reader.MoveToFirstAttribute())
			{
				do
				{
					string attributeName = reader.LocalName;
					string namespaceName = reader.NamespaceURI;
					if (!(namespaceName.Length == 0 && (attributeName == "id" || attributeName == "ref")))
					{
						Guid attributeGuid = new Guid();
						MetaAttributeInfo attributeInfo = null;
						if (customElement != null)
						{
							attributeGuid = customElement.MapAttribute(namespaceName, attributeName);
							if (!attributeGuid.Equals(Guid.Empty))
							{
								attributeInfo = dataDir.FindMetaAttribute(attributeGuid);
							}
						}
						if (attributeInfo == null && namespaceName.Length == 0)
						{
							attributeInfo = element.MetaClass.FindMetaAttribute(attributeName);
						}
						if (attributeInfo != null)
						{
							SetAttributeValue(element, attributeInfo, reader.Value);
						}
					}
				} while (reader.MoveToNextAttribute());
			}
			#endregion // Attribute processing
			//PassEndElement(reader); // UNDONE: Now temporary, rip this
			ProcessChildElements(reader, customModel, element, customElement, true);
		}
		private void ProcessChildElements(XmlReader reader, IORMCustomSerializedMetaModel customModel, ModelElement element, IORMCustomSerializedElement customElement, bool allowIgnoreContainer)
		{
			MetaDataDirectory dataDir = myStore.MetaDataDirectory;
			string elementName;
			string namespaceName;
			string containerName = null;
			string containerNamespace = null;
			while (reader.Read())
			{
				XmlNodeType outerNodeType = reader.NodeType;
				if (outerNodeType == XmlNodeType.Element)
				{
					elementName = reader.LocalName;
					namespaceName = reader.NamespaceURI;
					string idValue = reader.GetAttribute("id");
					string refValue = reader.GetAttribute("ref");
					bool aggregatedClass = idValue != null && refValue == null;
					MetaRoleInfo oppositeMetaRole = null;
					MetaClassInfo oppositeMetaClass = null;
					IList<Guid> oppositeMetaRoleGuids = null;
					bool nodeProcessed = false;
					if (aggregatedClass && containerName == null)
					{
						// All we have is the class name, go look for an appropriate aggregate
						if (customModel != null)
						{
							Guid metaClassGuid = customModel.MapClassName(namespaceName, elementName);
							if (!metaClassGuid.Equals(Guid.Empty))
							{
								oppositeMetaClass = dataDir.FindMetaClass(metaClassGuid);
							}
						}
						if (oppositeMetaClass == null)
						{
							oppositeMetaClass = dataDir.FindMetaClass(elementName);
						}
						if (oppositeMetaClass != null)
						{
							// Find the aggregating role that maps to this class
							IList aggregatedRoles = element.MetaClass.AggregatedRoles;
							MetaRoleInfo aggregatedRole = null;
							int rolesCount = aggregatedRoles.Count;
							for (int i = 0; i < rolesCount; ++i)
							{
								MetaRoleInfo testRole = (MetaRoleInfo)aggregatedRoles[i];
								if (testRole.RolePlayer == oppositeMetaClass)
								{
									aggregatedRole = testRole;
									break;
								}
							}
						}
					}
					else if (customElement != null)
					{
						ORMCustomSerializedElementMatch match = default(ORMCustomSerializedElementMatch);
						if (aggregatedClass)
						{
							// The meta role information should be available from the container name only
							match = customElement.MapChildElement(null, null, containerNamespace, containerName);
						}
						else if (refValue != null)
						{
							// Only go for a match if we have an id or a ref. Otherwise, this must be a container node,
							// so there is no point in looking it up.
							match = customElement.MapChildElement(namespaceName, elementName, containerNamespace, containerName);
						}
						switch (match.MatchStyle)
						{
							case ORMCustomSerializedElementMatchStyle.SingleOppositeMetaRole:
								{
									oppositeMetaRole = dataDir.FindMetaRole(match.SingleOppositeMetaRoleGuid);
									if (oppositeMetaRole.RolePlayer.Descendants.Count == 0)
									{
										// The opposite element can only be of one type, which means
										// we can create the relationship with no additional information
										oppositeMetaClass = oppositeMetaRole.RolePlayer;
									}
									else
									{
										// It is possible that there are multiple types associated with the opposite
										// end point, we don't know what type to create at this point
									}
									break;
								}
							case ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRoles:
								{
									oppositeMetaRoleGuids = match.OppositeMetaRoleGuidCollection;
									break;
								}
							case ORMCustomSerializedElementMatchStyle.Attribute:
								{
									MetaAttributeInfo attributeInfo = dataDir.FindMetaAttribute(match.MetaAttributeGuid);
									if (match.DoubleTagName == null)
									{
										// Reader the value off directly
										SetAttributeValue(element, attributeInfo, reader.Value);
										nodeProcessed = true;
									}
									else
									{
										// Look for the inner tag
										string matchName = match.DoubleTagName;
										while (reader.Read())
										{
											XmlNodeType innerType = reader.NodeType;
											if (innerType == XmlNodeType.Element)
											{
												if (reader.LocalName == matchName && reader.NamespaceURI == namespaceName)
												{
													SetAttributeValue(element, attributeInfo, reader.Value);
													nodeProcessed = true;
												}
											}
											else if (innerType == XmlNodeType.EndElement)
											{
												break;
											}
										}
									}
								}
								break;
							case ORMCustomSerializedElementMatchStyle.None:
								// If this is an unrecognized node without an id or ref then push
								// the container node (we only allow container depth of 1)
								// and continue to loop.
								if (containerName == null && idValue == null && refValue == null)
								{
									containerName = elementName;
									containerNamespace = namespaceName;
									nodeProcessed = true;
								}
								break;
						}
					}
					else if (aggregatedClass)
					{
						// UNDONE: Now This may be very similar to the first case (aggregate with no container)
						// The default spit adds the opposite role player name
					}
					else if (refValue != null)
					{
					}
					if (!nodeProcessed)
					{
						if (oppositeMetaRole != null)
						{
							if (oppositeMetaClass != null)
							{
								// Create a new element and make sure the relationship
								// to this element does not already exist. This obviously requires one
								// relationship of each type between any two objects, which is a reasonable assumption
								// for a well-formed model.
								bool isNewElement;
								string elementId = aggregatedClass ? idValue : refValue;
								ModelElement oppositeElement = CreateElement(elementId, oppositeMetaClass, Guid.Empty, out isNewElement);
								bool createLink = true;
								if (isNewElement)
								{
									if (aggregatedClass)
									{
										ProcessChildElements(reader, customModel, oppositeElement, oppositeElement as IORMCustomSerializedElement, true);
									}
									else
									{
										// UNDONE: Now handle PrimaryDefinition links here
									}
								}
								else
								{
									IList oppositeRolePlayers = oppositeElement.GetElementLinks(oppositeMetaRole);
									int oppositeCount = oppositeRolePlayers.Count;
									for (int i = 0; i < oppositeCount; ++i)
									{
										if (object.ReferenceEquals(element, oppositeRolePlayers[i]))
										{
											createLink = false;
											break;
										}
									}
								}
								if (createLink)
								{
									ElementLink newLink = CreateElementLink(idValue, element, oppositeElement, oppositeMetaRole);
									if (aggregatedClass)
									{
										ProcessChildElements(reader, customModel, oppositeElement, oppositeElement as IORMCustomSerializedElement, true);
									}
									else if (idValue != null)
									{
										ProcessChildElements(reader, customModel, newLink, newLink as IORMCustomSerializedElement, true);
									}
								}
							}
						}
						else if (oppositeMetaRoleGuids != null)
						{
							PassEndElement(reader);
						}
						else
						{
							PassEndElement(reader);
						}
					}
				}
				else if (outerNodeType == XmlNodeType.EndElement && containerName != null)
				{
					// Pop the container node
					containerName = null;
					containerNamespace = null;
				}
			}
		}
		/// <summary>
		/// Set the value of the specified attribute on the model element
		/// </summary>
		/// <param name="element">The element to modify</param>
		/// <param name="attributeInfo">The meta attribute to set</param>
		/// <param name="stringValue">The new value of the attribute</param>
		private void SetAttributeValue(ModelElement element, MetaAttributeInfo attributeInfo, string stringValue)
		{
			PropertyInfo propertyInfo = attributeInfo.PropertyInfo;
			Type propertyType = propertyInfo.PropertyType;
			object objectValue = null;
			if (propertyType.IsEnum)
			{
				objectValue = Enum.Parse(propertyType, stringValue);
			}
			else
			{
				switch (Type.GetTypeCode(propertyType))
				{
					case TypeCode.DateTime:
						objectValue = XmlConvert.ToDateTime(stringValue);
						break;
					case TypeCode.UInt64:
						objectValue = XmlConvert.ToUInt64(stringValue);
						break;
					case TypeCode.Int64:
						objectValue = XmlConvert.ToInt64(stringValue);
						break;
					case TypeCode.UInt32:
						objectValue = XmlConvert.ToUInt32(stringValue);
						break;
					case TypeCode.Int32:
						objectValue = XmlConvert.ToInt32(stringValue);
						break;
					case TypeCode.UInt16:
						objectValue = XmlConvert.ToUInt16(stringValue);
						break;
					case TypeCode.Int16:
						objectValue = XmlConvert.ToInt16(stringValue);
						break;
					case TypeCode.Byte:
						objectValue = XmlConvert.ToByte(stringValue);
						break;
					case TypeCode.SByte:
						objectValue = XmlConvert.ToSByte(stringValue);
						break;
					case TypeCode.Char:
						objectValue = XmlConvert.ToChar(stringValue);
						break;
					case TypeCode.Boolean:
						objectValue = XmlConvert.ToBoolean(stringValue);
						break;
					case TypeCode.Decimal:
						objectValue = XmlConvert.ToDecimal(stringValue);
						break;
					case TypeCode.Double:
						objectValue = XmlConvert.ToDouble(stringValue);
						break;
					case TypeCode.Single:
						objectValue = XmlConvert.ToSingle(stringValue);
						break;
					case TypeCode.String:
						objectValue = stringValue;
						break;
					case TypeCode.Object:
						{
							if (propertyType == typeof(Guid))
							{
								objectValue = XmlConvert.ToGuid(stringValue);
							}
							else if (propertyType == typeof(TimeSpan))
							{
								objectValue = XmlConvert.ToTimeSpan(stringValue);
							}
							else
							{
								Debug.Assert(false); // UNDONE: Now Use TypeConverter
							}
							break;
						}
				}
				if (objectValue != null)
				{
					element.SetAttributeValue(attributeInfo, objectValue);
				}
			}
		}
		/// <summary>
		/// Create an element link after verifying that the link needs to be created
		/// </summary>
		/// <param name="idValue">The value of the id for the link, or null</param>
		/// <param name="rolePlayer">The near role player</param>
		/// <param name="oppositeRolePlayer">The opposite role player</param>
		/// <param name="oppositeMetaRoleInfo">The opposite meta role</param>
		/// <returns>The newly created element link</returns>
		private ElementLink CreateElementLink(string idValue, ModelElement rolePlayer, ModelElement oppositeRolePlayer, MetaRoleInfo oppositeMetaRoleInfo)
		{
			// Create an element link. There is no attempt here to determine if the link already
			// exists in the store;
			Guid id = (idValue == null) ? Guid.NewGuid() : GetElementId(idValue);
			Debug.Assert(null == myStore.ElementDirectory.FindElement(id));
			ElementLink retVal = myStore.ElementFactory.CreateElementLink(
				false,
				oppositeMetaRoleInfo.MetaRelationship.ImplementationClass,
				new RoleAssignment[]{
					new RoleAssignment(oppositeMetaRoleInfo.OppositeMetaRole, rolePlayer),
					new RoleAssignment(oppositeMetaRoleInfo, oppositeRolePlayer)});
			if (myNotifyAdded != null)
			{
				myNotifyAdded.ElementAdded(retVal);
			}
			return retVal;
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="metaClassInfo">The meta class info of the element to create. If null,
		/// the metaClassId is used to find the class info</param>
		/// <param name="metaClassId">The identifier for the class</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, MetaClassInfo metaClassInfo, Guid metaClassId)
		{
			bool isNewElement;
			return CreateElement(idValue, metaClassInfo, metaClassId, out isNewElement);
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="metaClassInfo">The meta class info of the element to create. If null,
		/// the metaClassId is used to find the class info</param>
		/// <param name="metaClassId">The identifier for the class</param>
		/// <param name="isNewElement">true if the element is actually created, as opposed
		/// to being identified as an existing element</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, MetaClassInfo metaClassInfo, Guid metaClassId, out bool isNewElement)
		{
			isNewElement = false;

			// Get a valid guid identifier for the element
			Guid id = GetElementId(idValue);

			// See if we've already created this element as the opposite role player in a link
			ModelElement retVal = myStore.ElementDirectory.FindElement(id);
			if (retVal == null)
			{
				// The false parameter indicates that OnInitialize should not be called, which
				// is standard fare for deserialization routines.
				if (metaClassInfo == null)
				{
					metaClassInfo = myStore.MetaDataDirectory.FindMetaClass(metaClassId);
				}
				retVal = myStore.ElementFactory.CreateElement(false, metaClassInfo.ImplementationClass, id);
				isNewElement = true;
				if (myNotifyAdded != null)
				{
					myNotifyAdded.ElementAdded(retVal);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Parse or generate a guid from the passed in identifier
		/// </summary>
		/// <param name="id">A string taken from an id or ref tag in the xml file. If the
		/// value cannot be interpreted as a guid then generated a new guid and keep a map
		/// from this name to the generated guid</param>
		/// <returns>A non-empty Guid</returns>
		private Guid GetElementId(string id)
		{
			Guid retVal = new Guid();
			bool haveGuid = false;
			try
			{
				if (id[0] == '_')
				{
					retVal = new Guid(id.Substring(1));
					haveGuid = true;
				}
			}
			catch (FormatException)
			{
				// Swallow
			}
			if (!haveGuid)
			{
				if (myCustomIdToGuidMap == null)
				{
					myCustomIdToGuidMap = new Dictionary<string, Guid>();
				}
				else
				{
					haveGuid = myCustomIdToGuidMap.TryGetValue(id, out retVal);
				}
				if (!haveGuid)
				{
					retVal = Guid.NewGuid();
					myCustomIdToGuidMap[id] = retVal;
				}
			}
			return retVal;
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
	}
	#endregion // New Deserialization
}
#endif // NEWSERIALIZE
