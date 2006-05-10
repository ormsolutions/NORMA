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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.Shell
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
		/// Return true if no values are set in the structure
		/// </summary>
		public virtual bool IsDefault
		{
			get
			{
				return myCustomPrefix == null && myCustomName == null && myCustomNamespace == null && myDoubleTagName == null;
			}
		}

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
		public ORMCustomSerializedElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName)
			: base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteStyle = writeStyle;
		}

		private ORMCustomSerializedElementWriteStyle myWriteStyle;

		/// <summary>
		/// Default ORMCustomSerializedElementInfo
		/// </summary>
		public static readonly ORMCustomSerializedElementInfo Default = new ORMCustomSerializedElementInfo();

		/// <summary>
		/// Return true if no values are set in the structure
		/// </summary>
		public override bool IsDefault
		{
			get
			{
				return myWriteStyle == 0 && base.IsDefault;
			}
		}

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
		public ORMCustomSerializedAttributeInfo(string customPrefix, string customName, string customNamespace, bool writeCustomStorage, ORMCustomSerializedAttributeWriteStyle writeStyle, string doubleTagName)
			: base(customPrefix, customName, customNamespace, doubleTagName)
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
		public ORMCustomSerializedChildElementInfo(string customName, params Guid[] childGuids)
			: base(null, customName, null, ORMCustomSerializedElementWriteStyle.Element, null)
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
		public ORMCustomSerializedChildElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName, params Guid[] childGuids)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
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
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="oppositeMetaRoleGuid">The opposite meta role guids</param>
		public void InitializeRoles(Guid oppositeMetaRoleGuid)
		{
			mySingleGuid = oppositeMetaRoleGuid;
			myMultiGuids = null;
			myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeMetaRole;
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
		/// the prefix, [*, 1] contains the associated xml namespace, and [*, 2] contains
		/// the name of the schema file with no path. The schema file must be built into the
		/// model's assembly as an embedded resource (set via the Build Action property when the
		/// file is selected in the solution explorer) with the same namespace as the metamodel.
		/// This is most easily done by placing the schema file in the same directory as the
		/// model file, and making the namespace for the model file correspond to the default
		/// namespace for the directory (the project default namespace with the directory path appended)</returns>
		string[,] GetCustomElementNamespaces();
		/// <summary>
		/// Return the default element prefix for elements where the
		/// prefix is not specified
		/// </summary>
		string DefaultElementPrefix { get;}
		/// <summary>
		/// Return the meta class guids for all root elements.
		/// </summary>
		Guid[] GetRootElementClasses();
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
		/// <param name="elementLink">The link instance</param>
		/// <returns>Custom serialization information for links.</returns>
		ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink);
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
		/// <summary>
		/// Check the current state of the object to determine
		/// if it should be serialized or not.
		/// </summary>
		/// <returns>false to block serialization</returns>
		bool ShouldSerialize();
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
		public const string RootXmlNamespace = "http://schemas.neumont.edu/ORM/2006-04/ORMRoot";

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
				TypeConverter converter = GetTypeConverter(value.GetType());
				if (converter != null)
				{
					return converter.ConvertToInvariantString(value);
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
							return System.Xml.XmlConvert.ToString((System.DateTime)value, XmlDateTimeSerializationMode.Utc);
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
		/// Determine if an element should be serialized
		/// </summary>
		/// <param name="modelElement">Element to test</param>
		/// <returns>true unless the element is custom serialized and ShouldSerialize returns false.</returns>
		private static bool ShouldSerializeElement(ModelElement modelElement)
		{
			IORMCustomSerializedElement customElement = modelElement as IORMCustomSerializedElement;
			return (customElement != null) ? customElement.ShouldSerialize() : true;
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
		/// <param name="oppositeRolePlayer">The opposite role player.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		private void SerializeLink(System.Xml.XmlWriter file, ElementLink link, ModelElement rolePlayer, ModelElement oppositeRolePlayer, MetaRoleInfo rolePlayedInfo)
		{
			ORMCustomSerializedElementSupportedOperations supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
			ORMCustomSerializedElementInfo customInfo = ORMCustomSerializedElementInfo.Default;
			IList attributes = null;
			string defaultPrefix;
			bool hasCustomAttributes = false;

			if (!ShouldSerializeElement(link) || !ShouldSerializeElement(rolePlayer))
			{
				return;
			}
			IORMCustomSerializedElement rolePlayerCustomElement = rolePlayer as IORMCustomSerializedElement;
			IORMCustomSerializedElement customElement = rolePlayerCustomElement;
			bool writeContents = customElement != null &&
				0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) &&
				customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole, link).WriteStyle == ORMCustomSerializedElementWriteStyle.PrimaryLinkElement;

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
							customInfo = tagCustomElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole, link);
						}
					}
				}
				else if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) != 0)
				{
					customInfo = customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole, link);
					if (customInfo.IsDefault && !object.ReferenceEquals(GetParentModel(rolePlayer), GetParentModel(oppositeRolePlayer)))
					{
						return;
					}
				}
			}

			if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, string.Concat(rolePlayedInfo.MetaRelationship.Name, ".", rolePlayedInfo.OppositeMetaRole.Name)))
			{
				return;
			}

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
			MetaClassInfo lastChildClass = null;
			IORMCustomSerializedMetaModel parentModel = null;
			// If there class derived from the role player, then the class-level serialization settings may be
			// different than they were on the class specified on the role player, we need to check explicitly,
			// despite the earlier call to ShouldSerializeMetaRole
			bool checkSerializeClass = oppositeRoleInfo.RolePlayer.Descendants.Count != 0;
			Store store = myStore;

			if (!rolePlayedInfo.IsAggregate && !oppositeRoleInfo.IsAggregate) //write link
			{
				IList links = childElement.GetElementLinks(rolePlayedInfo);
				int linksCount = links.Count;
				if (links.Count != 0)
				{
					bool checkSerializeLinkClass = rolePlayedInfo.MetaRelationship.Descendants.Count != 0;
					MetaRelationshipInfo lastLinkClass = null;
					IORMCustomSerializedMetaModel linkParentModel = null;
					for (int i = 0; i < linksCount; ++i)
					{
						// Verify that the link itself should be serialized
						ElementLink link = (ElementLink)links[i];
						if (checkSerializeLinkClass)
						{
							MetaRelationshipInfo linkClass = link.MetaRelationship;
							if (linkClass != lastLinkClass)
							{
								lastLinkClass = linkClass;
								linkParentModel = GetParentModel(link);
							}
							if (linkParentModel != null && !linkParentModel.ShouldSerializeMetaClass(store, linkClass))
							{
								continue;
							}
						}

						// Verify that the opposite role player class should be serialized
						ModelElement oppositeRolePlayer = link.GetRolePlayer(oppositeRoleInfo);
						if (checkSerializeClass)
						{
							MetaClassInfo childClass = oppositeRolePlayer.MetaClass;
							if (childClass != lastChildClass)
							{
								lastChildClass = childClass;
								parentModel = GetParentModel(oppositeRolePlayer);
							}
							if (parentModel != null && !parentModel.ShouldSerializeMetaClass(store, childClass))
							{
								continue;
							}
						}

						if (writeBeginElement && !ret && customInfo != null)
						{
							if (!WriteCustomizedStartElement(file, customInfo, defaultPrefix, customInfo.CustomName))
							{
								return false;
							}
							ret = true;
						}
						SerializeLink(file, link, childElement, oppositeRolePlayer, rolePlayedInfo);
					}
				}
			}
			else if (rolePlayedInfo.IsAggregate) //write child
			{
				IList children = childElement.GetCounterpartRolePlayers(rolePlayedInfo, oppositeRoleInfo);
				int childCount = children.Count;

				if (childCount != 0)
				{
					string containerName = null;
					bool initializedContainerName = !writeBeginElement;

					for (int iChild = 0; iChild < childCount; ++iChild)
					{
						ModelElement child = (ModelElement)children[iChild];

						if (checkSerializeClass)
						{
							MetaClassInfo childClass = child.MetaClass;
							if (childClass != lastChildClass)
							{
								lastChildClass = childClass;
								parentModel = GetParentModel(child);
							}
							if (parentModel != null && !parentModel.ShouldSerializeMetaClass(store, childClass))
							{
								continue;
							}
						}

						if (ShouldSerializeElement(child))
						{
							if (customInfo == null)
							{
								defaultPrefix = DefaultElementPrefix(child);
							}
							if (!initializedContainerName)
							{
								containerName = string.Concat(rolePlayedInfo.MetaRelationship.Name, ".", rolePlayedInfo.OppositeMetaRole.Name);
								initializedContainerName = true;
							}
							if (!SerializeElement(file, child, customInfo, defaultPrefix, ref containerName))
							{
								return false;
							}
						}
					}
					ret = writeBeginElement && initializedContainerName && containerName == null;
				}
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
			if (!ShouldSerializeElement(element)) return true;
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
		public void Save(Stream stream)
		{
			System.Xml.XmlWriterSettings xmlSettings = new XmlWriterSettings();
			System.Xml.XmlWriter file;
			Store store = myStore;
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
			ElementDirectory elementDir = myStore.ElementDirectory;
			foreach (object value in values)
			{
				IORMCustomSerializedMetaModel ns = value as IORMCustomSerializedMetaModel;

				if (ns != null)
				{
					Guid[] metaClasses = ns.GetRootElementClasses();
					if (metaClasses != null)
					{
						int classCount = metaClasses.Length;
						for (int i = 0; i < classCount; ++i)
						{
							IList elements = elementDir.GetElements(metaClasses[i]);
							int elementCount = elements.Count;
							for (int j = 0; j < elementCount; ++j)
							{
								SerializeElement(file, (ModelElement)elements[j]);
							}
						}
					}
				}
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
		/// <summary>
		/// Structure used to map a guid to one or multiple
		/// placeholder elements. Because the type to create
		/// is not known until the placeholder is no longer
		/// needed, it is possible to choose different opposite
		/// placeholder types for the same Guid, so we need
		/// to be prepared to have more than one placeholder
		/// in the store for a single element.
		/// </summary>
		private struct PlaceholderElement
		{
			private ModelElement mySingleElement;
			private Collection<ModelElement> myMultipleElements;
			/// <summary>
			/// Call to create a placeholder element of the given type
			/// in the store. A placeholder is always created with a new
			/// Guid.
			/// </summary>
			/// <param name="store">Context store</param>
			/// <param name="classInfo">The classInfo of the role player. Note
			/// that it is very possible that the classInfo will be abstract. The
			/// descendants are searched to find the first non-abstract class</param>
			/// <returns>A new model element, or an existing placeholder.</returns>
			public ModelElement CreatePlaceholderElement(Store store, MetaClassInfo classInfo)
			{
				ModelElement retVal = FindElementOfType(classInfo);
				if (retVal == null)
				{
					Type implClass = classInfo.ImplementationClass;
					if (implClass.IsAbstract || implClass == typeof(ModelElement)) // The class factory won't create a raw model element
					{
						MetaClassInfo descendantInfo = FindCreatableClass(classInfo.Descendants); // Try the cheap search first
						if (descendantInfo != null)
						{
							descendantInfo = FindCreatableClass(classInfo.AllDescendants);
						}
						Debug.Assert(descendantInfo != null); // Some descendant should always be creatable, otherwise there could not be a valid link
						classInfo = descendantInfo;
					}
					retVal = store.ElementFactory.CreateElement(false, classInfo.ImplementationClass);
					if (myMultipleElements != null)
					{
						myMultipleElements.Add(retVal);
					}
					else if (mySingleElement != null)
					{
						myMultipleElements = new Collection<ModelElement>();
						myMultipleElements.Add(mySingleElement);
						myMultipleElements.Add(retVal);
						mySingleElement = null;
					}
					else
					{
						mySingleElement = retVal;
					}
				}
				return retVal;
			}
			private static MetaClassInfo FindCreatableClass(IList classInfos)
			{
				MetaClassInfo retVal = null;
				int count = classInfos.Count;
				if (count != 0)
				{
					for (int i = 0; i < count; ++i)
					{
						MetaClassInfo testInfo = (MetaClassInfo)classInfos[i];
						if (!testInfo.ImplementationClass.IsAbstract)
						{
							retVal = testInfo;
							break;
						}
					}
				}
				return retVal;
			}
			/// <summary>
			/// Find a placeholder instance with the specified meta type
			/// </summary>
			/// <param name="classInfo">The type to search for</param>
			/// <returns>The matching element, or null</returns>
			private ModelElement FindElementOfType(MetaClassInfo classInfo)
			{
				MetaRelationshipInfo relInfo = classInfo as MetaRelationshipInfo;
				if (relInfo != null)
				{
					ElementLink link;
					if (mySingleElement != null)
					{
						if (null != (link = mySingleElement as ElementLink) &&
							link.MetaRelationship == relInfo)
						{
							return mySingleElement;
						}
					}
					else if (myMultipleElements != null)
					{
						foreach (ModelElement mel in myMultipleElements)
						{
							if (null != (link = mel as ElementLink) &&
								link.MetaRelationship == relInfo)
							{
								return mel;
							}
						}
					}
				}
				else
				{
					if (mySingleElement != null)
					{
						if (mySingleElement.MetaClass == classInfo)
						{
							return mySingleElement;
						}
					}
					else if (myMultipleElements != null)
					{
						foreach (ModelElement mel in myMultipleElements)
						{
							if (mel.MetaClass == classInfo)
							{
								return mel;
							}
						}
					}
				}
				return null;
			}
			/// <summary>
			/// Take all placeholder elements, transfer all roles to the
			/// realElement, and then remove the placeholder element from the
			/// store.
			/// </summary>
			/// <param name="realElement">An element created with the real
			/// identity and type.</param>
			public void FulfilPlaceholderRoles(ModelElement realElement)
			{
				if (mySingleElement != null)
				{
					FulfilPlaceholderRoles(realElement, mySingleElement);
				}
				else
				{
					Debug.Assert(myMultipleElements != null);
					foreach (ModelElement placeholder in myMultipleElements)
					{
						FulfilPlaceholderRoles(realElement, placeholder);
					}
				}
			}
			private static void FulfilPlaceholderRoles(ModelElement realElement, ModelElement placeholder)
			{
				IList links = placeholder.GetElementLinks();
				int linkCount = links.Count;
				for (int i = linkCount - 1; i >= 0; --i) // Walk backwards, we're messing with the list contents
				{
					ElementLink link = (ElementLink)links[i];
					foreach (MetaRoleInfo metaRole in link.MetaRelationship.MetaRoles)
					{
						if (object.ReferenceEquals(link.GetRolePlayer(metaRole), placeholder))
						{
							link.SetRolePlayer(metaRole, realElement);
						}
					}
				}
				placeholder.Remove();
			}
		}
		private Dictionary<string, Guid> myCustomIdToGuidMap;
		private INotifyElementAdded myNotifyAdded;
		private Dictionary<Guid, PlaceholderElement> myPlaceholderElementMap;
		private Dictionary<string, IORMCustomSerializedMetaModel> myXmlNamespaceToModelMap;
		private static Dictionary<Type, TypeConverter> myTypeConverterCache;
#if DEBUG
		// Used for skipping schema validation on load if the shift key is down
		[System.Runtime.InteropServices.DllImport("user32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);
#endif
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		/// <param name="fixupManager">Class used to perfom fixup operations
		/// after the load is complete.</param>
		public void Load(Stream stream, DeserializationFixupManager fixupManager)
		{
			// Leave rules on so all of the links reconnect. Links are not saved.
			RulesSuspended = true;
			try
			{
				myNotifyAdded = fixupManager as INotifyElementAdded;
				XmlReaderSettings settings = new XmlReaderSettings();
				XmlSchemaSet schemas = settings.Schemas;
				Type schemaResourcePathType = GetType();
				schemas.Add(RootXmlNamespace, new XmlTextReader(schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, "ORM2Root.xsd")));

				// Extract namespace and schema information from the different meta models
				ICollection substores = myStore.SubStores.Values;
				Dictionary<string, IORMCustomSerializedMetaModel> namespaceToModelMap = new Dictionary<string, IORMCustomSerializedMetaModel>();
				foreach (object substore in substores)
				{
					IORMCustomSerializedMetaModel metaModel = substore as IORMCustomSerializedMetaModel;
					if (metaModel != null)
					{
						string[,] namespaces = metaModel.GetCustomElementNamespaces();
						int namespaceCount = namespaces.GetLength(0);
						for (int i = 0; i < namespaceCount; ++i)
						{
							string namespaceURI = namespaces[i, 1];
							namespaceToModelMap.Add(namespaceURI, metaModel);
							string schemaFile = namespaces[i, 2];
							if (schemaFile != null && schemaFile.Length != 0)
							{
								schemaResourcePathType = substore.GetType();
								schemas.Add(namespaceURI, new XmlTextReader(schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, schemaFile)));
							}
						}
					}
				}
				myXmlNamespaceToModelMap = namespaceToModelMap;
				NameTable nameTable = new NameTable();
				settings.NameTable = nameTable;
#if DEBUG
				// Skip validation when the shift key is down in debug mode
				if (0 == (0xff00 & GetKeyState(System.Windows.Forms.Keys.ShiftKey)))
				{
#endif // DEBUG
					settings.ValidationType = ValidationType.Schema;
#if DEBUG
				}
#endif // DEBUG
				// UNDONE: MSBUG Figure out why this transaction is needed. If it is ommitted then the EdgePointCollection
				// for each of the lines on the diagram is not initialized during Diagram.HandleLineRouting and none of the lines
				// are drawn. This behavior appears to be related to the diagram.GraphWrapper.IsLoading setting, which changes with
				// an extra transaction. However, the interactions between deserialization and diagram initialization are extremely
				// complex, so I'm not sure exactly what is happening here.
				using (Transaction t = myStore.TransactionManager.BeginTransaction())
				{
					using (XmlTextReader xmlReader = new XmlTextReader(new StreamReader(stream), nameTable))
					{
						using (XmlReader reader = XmlReader.Create(xmlReader, settings))
						{
							while (reader.Read())
							{
								if (reader.NodeType == XmlNodeType.Element)
								{
									if (!reader.IsEmptyElement && reader.NamespaceURI == RootXmlNamespace && reader.LocalName == RootXmlElementName)
									{
										while (reader.Read())
										{
											XmlNodeType nodeType = reader.NodeType;
											if (nodeType == XmlNodeType.Element)
											{
												bool processedRootElement = false;
												IORMCustomSerializedMetaModel metaModel;
												if (namespaceToModelMap.TryGetValue(reader.NamespaceURI, out metaModel))
												{
													Guid classGuid = metaModel.MapRootElement(reader.NamespaceURI, reader.LocalName);
													if (!classGuid.Equals(Guid.Empty))
													{
														processedRootElement = true;
														ProcessClassElement(reader, metaModel, CreateElement(reader.GetAttribute("id"), null, classGuid));
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
					if (fixupManager != null)
					{
						fixupManager.DeserializationComplete();
					}
					t.Commit();
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
					//derived objects are prefixed by an underscore and do not need to be read in
					if (!(namespaceName.Length == 0 && (attributeName == "id" || attributeName == "ref" || attributeName[0] == '_')))
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
			reader.MoveToElement();
			#endregion // Attribute processing
			ProcessChildElements(reader, customModel, element, customElement);
		}
		private void ProcessChildElements(XmlReader reader, IORMCustomSerializedMetaModel customModel, ModelElement element, IORMCustomSerializedElement customElement)
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			MetaDataDirectory dataDir = myStore.MetaDataDirectory;
			string elementName;
			string namespaceName;
			string containerName = null;
			string containerNamespace = null;
			IORMCustomSerializedMetaModel containerRestoreCustomModel = null;
			MetaRoleInfo containerOppositeMetaRole = null;
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
					bool oppositeMetaClassFullyDeterministic = false;
					bool resolveOppositeMetaClass = false;
					IList<Guid> oppositeMetaRoleGuids = null;
					IORMCustomSerializedMetaModel restoreCustomModel = null;
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
							Type namespaceType = (customModel != null) ? customModel.GetType() : element.GetType();
							oppositeMetaClass = dataDir.FindMetaClass(string.Concat(namespaceType.Namespace, ".", elementName));
						}
						if (oppositeMetaClass != null)
						{
							// Find the aggregating role that maps to this class
							IList aggregatedRoles = element.MetaClass.AggregatedRoles;
							int rolesCount = aggregatedRoles.Count;
							for (int i = 0; i < rolesCount; ++i)
							{
								MetaRoleInfo testRole = (MetaRoleInfo)aggregatedRoles[i];
								if (testRole.RolePlayer == oppositeMetaClass)
								{
									oppositeMetaRole = testRole;
									break;
								}
							}
						}
					}
					else
					{
						bool handledByCustomElement = false;
						if (customElement != null)
						{
							handledByCustomElement = true;
							ORMCustomSerializedElementMatch match = default(ORMCustomSerializedElementMatch);
							if (aggregatedClass)
							{
								// The meta role information should be available from the container name only
								match = customElement.MapChildElement(null, null, containerNamespace, containerName);
							}
							else
							{
								match = customElement.MapChildElement(namespaceName, elementName, containerNamespace, containerName);
							}
							switch (match.MatchStyle)
							{
								case ORMCustomSerializedElementMatchStyle.SingleOppositeMetaRole:
									oppositeMetaRole = dataDir.FindMetaRole(match.SingleOppositeMetaRoleGuid);
									resolveOppositeMetaClass = true;
									break;
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
											SetAttributeValue(element, attributeInfo, reader.ReadString());
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
														SetAttributeValue(element, attributeInfo, reader.ReadString());
														nodeProcessed = true;
													}
													else
													{
														PassEndElement(reader);
													}
												}
												else if (innerType == XmlNodeType.EndElement)
												{
													break;
												}
											}
										}
										break;
									}
								case ORMCustomSerializedElementMatchStyle.None:
									handledByCustomElement = false;
									break;
							}
						}
						if (!handledByCustomElement)
						{
							if (aggregatedClass)
							{
								if (containerOppositeMetaRole != null)
								{
									oppositeMetaRole = containerOppositeMetaRole;
									resolveOppositeMetaClass = true;
								}
							}
							else if (refValue != null)
							{
								IORMCustomSerializedMetaModel childModel;
								if (elementName.IndexOf('.') > 0 && myXmlNamespaceToModelMap.TryGetValue(namespaceName, out childModel))
								{
									if (childModel != customModel && customModel != null)
									{
										restoreCustomModel = customModel;
										customModel = childModel;
									}
									MetaRoleInfo metaRole = dataDir.FindMetaRole(string.Concat(childModel.GetType().Namespace, ".", elementName));
									// Fallback on the two standard meta models
									if (metaRole == null)
									{
										metaRole = dataDir.FindMetaRole(string.Concat(typeof(ModelElement).Namespace, ".", elementName));
										if (metaRole == null)
										{
											metaRole = dataDir.FindMetaRole(string.Concat(typeof(Diagram).Namespace, ".", elementName));
										}
									}
									if (metaRole != null)
									{
										oppositeMetaRole = metaRole;
										resolveOppositeMetaClass = true;
									}
								}
							}
							else if (containerName == null)
							{
								// The tag name should have the format Rel.Role. Assume the relationship
								// is in the same namespace as the model associated with the xml namespace.
								// Models can nest elements inside base models, so we can't assume the node
								// is in the same code namespace as the parent. Also, if the implementation
								// class of the parent element has been upgraded (with MetaClassInfo.UpgradeImplementationClass)
								// then the ImplemtationClass of the parent node will be in the wrong namespace.
								// The model elements themselves are more stable, use them.
								IORMCustomSerializedMetaModel childModel;
								if (elementName.IndexOf('.') > 0 && myXmlNamespaceToModelMap.TryGetValue(namespaceName, out childModel))
								{
									MetaRoleInfo metaRole = dataDir.FindMetaRole(string.Concat(childModel.GetType().Namespace, ".", elementName));
									// Fallback on the two standard meta models
									if (metaRole == null)
									{
										metaRole = dataDir.FindMetaRole(string.Concat(typeof(ModelElement).Namespace, ".", elementName));
										if (metaRole == null)
										{
											metaRole = dataDir.FindMetaRole(string.Concat(typeof(Diagram).Namespace, ".", elementName));
										}
									}
									if (metaRole != null)
									{
										containerOppositeMetaRole = metaRole;
										containerRestoreCustomModel = customModel;
										customModel = childModel;
									}
								}
								// If this is an unrecognized node without an id or ref then push
								// the container node (we only allow container depth of 1)
								// and continue to loop.
								if (!reader.IsEmptyElement)
								{
									containerName = elementName;
									containerNamespace = namespaceName;
								}
								nodeProcessed = true;
							}
						}
					}
					if (!nodeProcessed)
					{
						if (oppositeMetaRole != null)
						{
							if (resolveOppositeMetaClass)
							{
								oppositeMetaClass = oppositeMetaRole.RolePlayer;
								// If the opposite role player does not have any derived class in
								// the model then we know what type of element to create. Otherwise,
								// we need to create the element as a pending element if it doesn't exist
								// already.
								oppositeMetaClassFullyDeterministic = oppositeMetaClass.Descendants.Count == 0;
								if (aggregatedClass && !oppositeMetaClassFullyDeterministic)
								{
									MetaClassInfo testMetaClass = null;
									IORMCustomSerializedMetaModel elementModel = myXmlNamespaceToModelMap[namespaceName];
									if (elementModel == null)
									{
										elementModel = customModel;
									}
									if (elementModel != null)
									{
										Guid mappedGuid = elementModel.MapClassName(namespaceName, elementName);
										if (!mappedGuid.Equals(Guid.Empty))
										{
											testMetaClass = dataDir.FindMetaClass(mappedGuid);
										}
									}
									if (testMetaClass == null)
									{
										Type namespaceType = (elementModel != null) ? elementModel.GetType() : element.GetType();
										testMetaClass = dataDir.FindMetaClass(string.Concat(namespaceType.Namespace, ".", elementName));
									}
									oppositeMetaClass = testMetaClass;
									oppositeMetaClassFullyDeterministic = true;
								}
							}
						}
						else if (oppositeMetaRoleGuids != null)
						{
							// In this case we have multiple opposite meta role guids, so we
							// always have to rely on the aggregated element to find the data
							Debug.Assert(aggregatedClass);
							if (customModel != null)
							{
								Guid mappedGuid = customModel.MapClassName(namespaceName, elementName);
								if (!mappedGuid.Equals(Guid.Empty))
								{
									oppositeMetaClass = dataDir.FindMetaClass(mappedGuid);
								}
							}
							if (oppositeMetaClass == null)
							{
								Type namespaceType = (customModel != null) ? customModel.GetType() : element.GetType();
								oppositeMetaClass = dataDir.FindMetaClass(string.Concat(namespaceType.Namespace, ".", elementName));
							}
							if (oppositeMetaClass != null)
							{
								oppositeMetaClassFullyDeterministic = true;
								int roleGuidCount = oppositeMetaRoleGuids.Count;
								for (int i = 0; i < roleGuidCount; ++i)
								{
									MetaRoleInfo testRoleInfo = dataDir.FindMetaRole(oppositeMetaRoleGuids[i]);
									if (oppositeMetaClass.IsDerivedFrom(testRoleInfo.RolePlayer.Id))
									{
										oppositeMetaRole = testRoleInfo;
#if DEBUG
										for (int j = i + 1; j < roleGuidCount; ++j)
										{
											testRoleInfo = dataDir.FindMetaRole(oppositeMetaRoleGuids[j]);
											Debug.Assert(testRoleInfo == null || !oppositeMetaClass.IsDerivedFrom(testRoleInfo.RolePlayer.Id), "Custom serialization data does not provide a unique deserialization map for a combined element.");
										}
#endif // DEBUG
										break;
									}
								}
							}
						}
						if (oppositeMetaClass != null)
						{
							Debug.Assert(oppositeMetaRole != null);
							// Create a new element and make sure the relationship
							// to this element does not already exist. This obviously requires one
							// relationship of each type between any two objects, which is a reasonable assumption
							// for a well-formed model.
							bool isNewElement;
							string elementId = aggregatedClass ? idValue : refValue;
							ModelElement oppositeElement = CreateElement(elementId, oppositeMetaClass, Guid.Empty, !oppositeMetaClassFullyDeterministic, out isNewElement);
							bool createLink = true;
							if (aggregatedClass)
							{
								ProcessClassElement(reader, customModel, oppositeElement);
							}
							if (!isNewElement)
							{
								IList oppositeRolePlayers = oppositeElement.GetCounterpartRolePlayers(oppositeMetaRole, oppositeMetaRole.OppositeMetaRole);
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
								ElementLink newLink = CreateElementLink(aggregatedClass ? null : idValue, element, oppositeElement, oppositeMetaRole);
								if (!aggregatedClass && idValue != null)
								{
									ProcessClassElement(reader, customModel, newLink);
								}
							}
						}
						else
						{
							PassEndElement(reader);
						}
					}
					if (restoreCustomModel != null)
					{
						customModel = restoreCustomModel;
					}
				}
				else if (outerNodeType == XmlNodeType.EndElement)
				{
					if (containerName != null)
					{
						// Pop the container node
						containerName = null;
						containerNamespace = null;
						containerOppositeMetaRole = null;
						if (containerRestoreCustomModel != null)
						{
							customModel = containerRestoreCustomModel;
							containerRestoreCustomModel = null;
						}
					}
					else
					{
						break;
					}
				}
			}
		}
		/// <summary>
		/// Retrieve a type converter for the specified type
		/// </summary>
		/// <param name="propertyType">The type of the property to convert</param>
		/// <returns>TypeConverter, or null</returns>
		private static TypeConverter GetTypeConverter(Type propertyType)
		{
			TypeConverter retVal = null;
			if (myTypeConverterCache == null)
			{
				myTypeConverterCache = new Dictionary<Type, TypeConverter>();
			}
			else if (myTypeConverterCache.TryGetValue(propertyType, out retVal))
			{
				return retVal;
			}
			object[] typeConverters = propertyType.GetCustomAttributes(typeof(TypeConverterAttribute), false);

			if (typeConverters != null && typeConverters.Length != 0)
			{
				Type converterType = Type.GetType(((TypeConverterAttribute)typeConverters[0]).ConverterTypeName, false, false);
				if (converterType != null)
				{
					retVal = (TypeConverter)Activator.CreateInstance(converterType);
				}
			}
			myTypeConverterCache[propertyType] = retVal;
			return retVal;
		}
		/// <summary>
		/// Set the value of the specified attribute on the model element
		/// </summary>
		/// <param name="element">The element to modify</param>
		/// <param name="attributeInfo">The meta attribute to set</param>
		/// <param name="stringValue">The new value of the attribute</param>
		private static void SetAttributeValue(ModelElement element, MetaAttributeInfo attributeInfo, string stringValue)
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
						objectValue = XmlConvert.ToDateTime(stringValue, XmlDateTimeSerializationMode.Utc);
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
								TypeConverter converter = GetTypeConverter(propertyType);
								if (converter != null)
								{
									objectValue = converter.ConvertFromInvariantString(stringValue);
								}
							}
							break;
						}
				}
			}
			if (objectValue != null)
			{
				element.SetAttributeValue(attributeInfo, objectValue);
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
				id,
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
			return CreateElement(idValue, metaClassInfo, metaClassId, false, out isNewElement);
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="metaClassInfo">The meta class info of the element to create. If null,
		/// the metaClassId is used to find the class info</param>
		/// <param name="metaClassId">The identifier for the class</param>
		/// <param name="createAsPlaceholder">The provided meta class information is not unique.
		/// If this element is not already created then add it with a separate tracked id so it can
		/// be replaced later by the fully resolved type. All role players will be automatically
		/// moved from the pending placeholder when the real element is created</param>
		/// <param name="isNewElement">true if the element is actually created, as opposed
		/// to being identified as an existing element</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, MetaClassInfo metaClassInfo, Guid metaClassId, bool createAsPlaceholder, out bool isNewElement)
		{
			isNewElement = false;

			// Get a valid guid identifier for the element
			Guid id = GetElementId(idValue);

			// See if we've already created this element as the opposite role player in a link
			ModelElement retVal = myStore.ElementDirectory.FindElement(id);
			if (retVal == null)
			{
				PlaceholderElement placeholder = default(PlaceholderElement);
				bool existingPlaceholder = myPlaceholderElementMap != null && myPlaceholderElementMap.TryGetValue(id, out placeholder);
				// The false parameter indicates that OnInitialize should not be called, which
				// is standard fare for deserialization routines.
				if (metaClassInfo == null)
				{
					metaClassInfo = myStore.MetaDataDirectory.FindMetaClass(metaClassId);
				}
				Type implClass = metaClassInfo.ImplementationClass;
				if (createAsPlaceholder || implClass.IsAbstract)
				{
					if (!existingPlaceholder && myPlaceholderElementMap == null)
					{
						myPlaceholderElementMap = new Dictionary<Guid, PlaceholderElement>();
					}
					retVal = placeholder.CreatePlaceholderElement(myStore, metaClassInfo);
					myPlaceholderElementMap[id] = placeholder;
				}
				else
				{
					retVal = myStore.ElementFactory.CreateElement(false, implClass, id);
					isNewElement = true;
					if (myNotifyAdded != null)
					{
						myNotifyAdded.ElementAdded(retVal);
					}
					if (existingPlaceholder)
					{
						placeholder.FulfilPlaceholderRoles(retVal);
						myPlaceholderElementMap.Remove(id);
					}
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
	#region MetaModelAttributes
	#region MetaModelAttributesUtility
	/// <summary>
	/// Grabs a Singleton ResourceManager.
	/// </summary>
	public static class MetaModelAttributesUtility
	{
		/// <summary>
		/// The class that actually returns a ResourceManager.
		/// </summary>
		/// <param name="metaModelType">The type of MetaModel.</param>
		/// <returns>The ResourceManager.</returns>
		public static ResourceManager GetSingletonResourceManager(Type metaModelType)
		{
			PropertyInfo propertyInfo = metaModelType.GetProperty("SingletonResourceManager", BindingFlags.Static | BindingFlags.Public);
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(null, null) as ResourceManager;
			}
			return null;
		}
	}
	#endregion MetaModelAttributesUtility
	#region MetaModelDescriptionAttribute
	/// <summary>
	/// This is a description attribute for the MetaModel.
	/// </summary>s
	public class MetaModelDescriptionAttribute : System.ComponentModel.DescriptionAttribute
	{
		private string _description;
		private readonly Type _ownerType;
		/// <summary>
		/// MetaModelDescriptionAttribute constructor.
		/// </summary>
		/// <param name="ownerType">The owner</param>
		/// <param name="resourceId">the resource id you wish to be returned.</param>
		public MetaModelDescriptionAttribute(Type ownerType, string resourceId)
			: base(resourceId)
		{
			this._ownerType = ownerType;
		}
		/// <summary>
		/// Gets the Description from the Resource manager.
		/// </summary>
		public override string Description
		{
			get
			{
				if (this._description == null)
				{
					this._description = MetaModelAttributesUtility.GetSingletonResourceManager(this._ownerType).GetString(base.DescriptionValue, CultureInfo.CurrentUICulture);
				}
				return this._description;
			}
		}
	}
	#endregion // MetaModelDescriptionAttribute
	#region MetaModelDisplayNameAttribute
	/// <summary>
	/// This class defines a MetaModelDisplayNameAttribute.
	/// </summary>
	public class MetaModelDisplayNameAttribute : DisplayNameAttribute
	{
		private string _displayName;
		private readonly Type _ownerType;
		/// <summary>
		/// Constructor for a MetaModelDisplayNameAttribute.
		/// </summary>
		/// <param name="ownerType">The owner of the Attribute.</param>
		/// <param name="resourceId">The resource id of the attribute you are trying to retrieve.</param>
		public MetaModelDisplayNameAttribute(Type ownerType, string resourceId)
			: base(resourceId)
		{
			this._ownerType = ownerType;
		}
		/// <summary>
		/// Getter for the DisplayName of the MetaModelDisplayName attribute.
		/// </summary>
		public override string DisplayName
		{
			get
			{
				if (this._displayName == null)
				{
					this._displayName = MetaModelAttributesUtility.GetSingletonResourceManager(this._ownerType).GetString(base.DisplayNameValue, CultureInfo.CurrentUICulture);
				}
				return this._displayName;
			}
		}
	}
	#endregion // MetaModelDisplayNameAttribute
	#endregion // MetaModelAttributes
}
