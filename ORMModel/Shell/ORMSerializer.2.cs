#if NEWSERIALIZE
using System;
using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;
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
	/// <summary>
	/// Supported operations for element custom serialization.
	/// </summary>
	[Flags]
	[CLSCompliant(true)]
	public enum ORMCustomSerializedElementSupportedOperations : byte
	{
		/// <summary>
		/// No operations are supported.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Combined element information is supported.
		/// </summary>
		CustomSerializedCombinedElementInfo = 0x01,
		/// <summary>
		/// Custom element information is supported.
		/// </summary>
		CustomSerializedElementInfo = 0x02,
		/// <summary>
		/// Custom attribute information is supported.
		/// </summary>
		CustomSerializedAttributeInfo = 0x04,
		/// <summary>
		/// Custom link information is supported.
		/// </summary>
		CustomSerializedLinkInfo = 0x08
	}
	/// <summary>
	/// Write style for element custom serialization.
	/// </summary>
	[CLSCompliant(true)]
	public enum ORMCustomSerializedElementWriteStyle : byte
	{
		/// <summary>
		/// Dont write.
		/// </summary>
		DontWrite = 0xFF,
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
	public enum ORMCustomSerializedAttributeWriteStyle : byte
	{
		/// <summary>
		/// Dont write.
		/// </summary>
		DontWrite = 0xFF,
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
	/// Custom serialization information for combined elements.
	/// </summary>
	[CLSCompliant(true)]
	public struct ORMCustomSerializedCombinedElementInfo
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The combined element name.</param>
		/// <param name="GUIDs">The combined element GUIDs.</param>
		public ORMCustomSerializedCombinedElementInfo(string name, Guid[] GUIDs)
		{
			myName = name;
			myGUIDs = GUIDs;
			myCachedElements = new System.Collections.Generic.Stack<ModelElement>();
			myCachedRoleInfo = new System.Collections.Generic.Stack<MetaRoleInfo>();
		}

		private string myName;
		private Guid[] myGUIDs;
		private System.Collections.Generic.Stack<ModelElement> myCachedElements;
		private System.Collections.Generic.Stack<MetaRoleInfo> myCachedRoleInfo;

		/// <summary>
		/// The combined element name.
		/// </summary>
		/// <value>The combined element name.</value>
		public string Name
		{
			get { return myName; }
			set { myName = value; }
		}
		/// <summary>
		/// The combined element GUIDs.
		/// </summary>
		/// <value>The combined element GUIDs.</value>
		public Guid[] GUIDs
		{
			get { return myGUIDs; }
			set { myGUIDs = value; }
		}

		/// <summary>
		/// Checks to see if the GUID is part of the combined element.
		/// </summary>
		/// <param name="guid">The GUID to find.</param>
		/// <returns>true if the GUID is part of the combined element.</returns>
		public bool ContainsGUID(Guid guid)
		{
			return (myGUIDs as IList<Guid>).Contains(guid);
		}
		/// <summary>
		/// Stores the element and returns true when the cached elements should be written.
		/// </summary>
		/// <returns>true when the cached elements should be written.</returns>
		public bool PushElement(ModelElement element, MetaRoleInfo roleInfo)
		{
			myCachedElements.Push(element);
			myCachedRoleInfo.Push(roleInfo);
			return myCachedElements.Count >= myGUIDs.Length;
		}
		/// <summary>
		/// Retrieves a stored element and returns true when the cache is empty.
		/// </summary>
		/// <param name="element">Returns the element that was retrieved.</param>
		/// <param name="roleInfo">Returns the role information that was retrieved.</param>
		/// <returns>true when the cache is empty.</returns>
		public bool PopElement(out ModelElement element, out MetaRoleInfo roleInfo)
		{
			if (myCachedElements.Count < 1)
			{
				element = null;
				roleInfo = null;
				return false;
			}

			element = myCachedElements.Pop();
			roleInfo = myCachedRoleInfo.Pop();

			return true;
		}
	}

	/// <summary>
	/// Custom serialization information.
	/// </summary>
	[CLSCompliant(true)]
	public abstract class ORMCustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ORMCustomSerializedInfo()
		{
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		public ORMCustomSerializedInfo(string customPrefix, string customName, string customNamespace, string doubleTagName)
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
		private ORMCustomSerializedElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writting.</param>
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
		private ORMCustomSerializedAttributeInfo()
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
	/// The interface for getting custom element namespaces.
	/// </summary>
	public interface IORMCustomElementNamespace
	{
		/// <summary>
		/// Returns custom element namespaces.
		/// </summary>
		/// <returns>Custom element namespaces.</returns>
		string[,] GetCustomElementNamespaces();
	}
	/// <summary>
	/// The interface for getting element custom serialization information.
	/// </summary>
	public interface IORMCustomSerializedElement
	{
		/// <summary>
		/// Returns the supported operations.
		/// </summary>
		/// <returns>The supported operations.</returns>
		ORMCustomSerializedElementSupportedOperations GetSupportedOperations();
		/// <summary>
		/// Returns true if some of the attributes are written as elements and others are written as attributes.
		/// </summary>
		/// <returns>true if some of the attributes are written as elements and others are written as attributes.</returns>
		bool HasMixedTypedAttributes();
		/// <summary>
		/// Returns custom serialization information for combined elements.
		/// </summary>
		/// <returns>Custom serialization information for combined elements.</returns>
		ORMCustomSerializedCombinedElementInfo[] GetCustomSerializedCombinedElementInfo();
		/// <summary>
		/// Returns custom serialization information for elements.
		/// </summary>
		/// <returns>Custom serialization information for elements.</returns>
		ORMCustomSerializedElementInfo GetCustomSerializedElementInfo();
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
	}

	/// <summary>
	/// Read/write .orm files leveraging the default IMS serializer
	/// </summary>
	public partial class ORMSerializer
	{
		#region New Serialization
		private System.Collections.Generic.List<Guid> myLinkGUIDs = new System.Collections.Generic.List<Guid>();

		private int FindGUID(ORMCustomSerializedCombinedElementInfo[] combinedElementInfo, Guid guid)
		{
			int count = combinedElementInfo.Length;

			for (int index = 0; index < count; ++index)
			{
				if (combinedElementInfo[index].ContainsGUID(guid))
					return index;
			}

			return -1;
		}
		private void SortAttributes(IORMCustomSerializedElement customElement, MetaRoleInfo rolePlayedInfo, ref IList attributes)
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
				IList attrs = attributes;
				Array.Sort<int>(indices, delegate(int index1, int index2)
				{
					ORMCustomSerializedAttributeInfo customInfo1 = customInfo[index1];
					ORMCustomSerializedAttributeInfo customInfo2 = customInfo[index2];

					if (customInfo1.WriteStyle==customInfo2.WriteStyle)
						return 0;
					if (customInfo1.WriteStyle == ORMCustomSerializedAttributeWriteStyle.Attribute)
						return -1;

					return 1;
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
		private bool WriteCustomizedStartElement(System.Xml.XmlWriter file, ORMCustomSerializedElementInfo customInfo, string defaultName)
		{
			switch (customInfo.WriteStyle)
			{
				default:
				{
					file.WriteStartElement
					(
						customInfo.CustomPrefix,
						customInfo.CustomName != null ? customInfo.CustomName : defaultName,
						customInfo.CustomNamespace
					);
					return true;
				}
				case ORMCustomSerializedElementWriteStyle.DontWrite:
				{
					return false;
				}
				case ORMCustomSerializedElementWriteStyle.DoubleTaggedElement:
				{
					string name = (customInfo.CustomName != null ? customInfo.CustomName : defaultName);

					file.WriteStartElement
					(
						customInfo.CustomPrefix,
						name,
						customInfo.CustomNamespace
					);
					file.WriteStartElement
					(
						customInfo.CustomPrefix,
						customInfo.DoubleTagName != null ? customInfo.DoubleTagName : name,
						customInfo.CustomNamespace
					);

					return true;
				}
			}
		}
		private void WriteCustomizedEndElement(System.Xml.XmlWriter file, ORMCustomSerializedElementInfo customInfo)
		{
			switch (customInfo.WriteStyle)
			{
				default:
				{
					file.WriteEndElement();
					return;
				}
#if DEBUG
				case ORMCustomSerializedElementWriteStyle.DontWrite:
				{
					System.Diagnostics.Debug.Fail("WriteCustomizedEndElement - ORMCustomSerializedElementWriteStyle.DontWrite");
					throw new System.InvalidOperationException();
				}
#endif
				case ORMCustomSerializedElementWriteStyle.DoubleTaggedElement:
				{
					file.WriteEndElement();
					file.WriteEndElement();
					return;
				}
			}
		}
		private void SerializeAttribute(System.Xml.XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, Microsoft.VisualStudio.Modeling.MetaAttributeInfo attribute, bool isCustomAttribute, MetaRoleInfo rolePlayedInfo)
		{
			ORMCustomSerializedAttributeInfo customInfo;

			if (isCustomAttribute)
				customInfo = customElement.GetCustomSerializedAttributeInfo(attribute, rolePlayedInfo);
			else
				customInfo = ORMCustomSerializedAttributeInfo.Default;

			if (!attribute.CustomStorage || customInfo.WriteCustomStorage)
			{
				object value = element.GetAttributeValue(attribute);

				if (customInfo.WriteStyle!=ORMCustomSerializedAttributeWriteStyle.Attribute || file.WriteState != System.Xml.WriteState.Element)
				{
					switch (customInfo.WriteStyle)
					{
						default:
						{
							file.WriteElementString
							(
								customInfo.CustomPrefix,
								customInfo.CustomName != null ? customInfo.CustomName : attribute.Name,
								customInfo.CustomNamespace,
								value != null ? value.ToString() : null
							);
							break;
						}
						case ORMCustomSerializedAttributeWriteStyle.DontWrite:
						{
							break;
						}
						case ORMCustomSerializedAttributeWriteStyle.DoubleTaggedElement:
						{
							string name = (customInfo.CustomName != null ? customInfo.CustomName : attribute.Name);

							file.WriteStartElement
							(
								customInfo.CustomPrefix,
								name,
								customInfo.CustomNamespace
							);
							file.WriteElementString
							(
								customInfo.CustomPrefix,
								customInfo.DoubleTagName != null ? customInfo.DoubleTagName : name,
								customInfo.CustomNamespace,
								value != null ? value.ToString() : null
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
						value != null ? value.ToString() : null
					);
				}
			}

			return;
		}
		private void SerializeChildElement(System.Xml.XmlWriter file, ModelElement element, MetaRoleInfo roleInfo, string name) //<*Collection/>
		{
			IList children = element.GetCounterpartRolePlayers(roleInfo.OppositeMetaRole, roleInfo);
			int childCount = children.Count;

			if (childCount < 1)
				return;
			if (name != null)
				if (!WriteCustomizedStartElement(file, ORMCustomSerializedElementInfo.Default, name)) return; //TODO change ORMCustomSerializedElementInfo.Default to a variable

			for (int iChild = 0; iChild < childCount; ++iChild)
			{
				SerializeElement(file, (ModelElement)children[iChild]);
			}

			if (name != null)
				WriteCustomizedEndElement(file, ORMCustomSerializedElementInfo.Default); //TODO change ORMCustomSerializedElementInfo.Default to a variable

			return;
		}
		private void SerializeLink(System.Xml.XmlWriter file, ModelElement link, ModelElement rolePlayer, ModelElement oppositeRolePlayer, MetaRoleInfo rolePlayedInfo, bool directLink)
		{
			ORMCustomSerializedElementSupportedOperations supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
			ORMCustomSerializedElementInfo customInfo = ORMCustomSerializedElementInfo.Default;
			IORMCustomSerializedElement customElement;
			IList attributes = null;

			if (link != null)
			{
				if (!ShouldSerialize(link)) return;
				customElement = link as IORMCustomSerializedElement;
				attributes = link.MetaClass.MetaAttributes;
			}
			else
			{
				if (!ShouldSerialize(rolePlayer)) return;
				customElement = rolePlayer as IORMCustomSerializedElement;
			}

			bool isCustomAttribute = false;

			if (customElement != null)
			{
				supportedOperations = customElement.GetSupportedOperations();

				if (customElement.HasMixedTypedAttributes() && attributes != null)
				{
					SortAttributes(customElement, rolePlayedInfo, ref attributes);
				}
				isCustomAttribute = (supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSerializedAttributeInfo) != 0;

				IORMCustomSerializedElement tagCustomElement = customElement;
				if (link != null)
				{
					tagCustomElement = rolePlayer as IORMCustomSerializedElement;
					if (tagCustomElement != null)
					{
						if (0 != (tagCustomElement.GetSupportedOperations() & ORMCustomSerializedElementSupportedOperations.CustomSerializedLinkInfo))
						{
							customInfo = tagCustomElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole);
						}
					}
				}
				else if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSerializedLinkInfo) != 0)
				{
					customInfo = customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeMetaRole);
				}
			}

			System.Text.StringBuilder name = new System.Text.StringBuilder();
			name.Append(rolePlayedInfo.MetaRelationship.Name);
			name.Append('.');
			name.Append(rolePlayedInfo.OppositeMetaRole.Name);
			if (!WriteCustomizedStartElement(file, customInfo, name.ToString())) return;

			Guid keyId = (link != null) ? link.Id : oppositeRolePlayer.Id;
			if (!directLink && !myLinkGUIDs.Contains(keyId))
			{
				myLinkGUIDs.Add(keyId);
				file.WriteAttributeString("id", keyId.ToString().ToUpper());

				for (int count = attributes.Count, index = 0; index < count; ++index)
				{
					Microsoft.VisualStudio.Modeling.MetaAttributeInfo attribute = (Microsoft.VisualStudio.Modeling.MetaAttributeInfo)attributes[index];

					SerializeAttribute
					(
						file,
						link,
						customElement,
						attribute,
						isCustomAttribute,
						rolePlayedInfo
					);
				}
			}
			else
			{
				file.WriteAttributeString("ref", keyId.ToString().ToUpper());
			}

			WriteCustomizedEndElement(file, customInfo);

			return;
		}
		private void SerializeElement(System.Xml.XmlWriter file, ModelElement element)
		{
			if (!ShouldSerialize(element)) return;
			ORMCustomSerializedElementSupportedOperations supportedOperations;
			ORMCustomSerializedCombinedElementInfo[] combinedElementInfo = null;
			MetaClassInfo classInfo = element.MetaClass;
			ORMCustomSerializedElementInfo customInfo;
			IORMCustomSerializedElement customElement = element as IORMCustomSerializedElement;
			IList attributes = classInfo.AllMetaAttributes;
			IList rolesPlayed = classInfo.AllMetaRolesPlayed;
			bool roleGrouping = false, isCustom = (customElement!=null);

			//load custom information
			if (isCustom)
			{
				supportedOperations = customElement.GetSupportedOperations();

				if (customElement.HasMixedTypedAttributes())
				{
					SortAttributes(customElement, null, ref attributes);
				}

				if (roleGrouping = (0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSerializedCombinedElementInfo)))
					combinedElementInfo = customElement.GetCustomSerializedCombinedElementInfo();

				if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSerializedElementInfo) != 0)
					customInfo = customElement.GetCustomSerializedElementInfo();
				else
					customInfo = ORMCustomSerializedElementInfo.Default;
			}
			else
			{
				supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
				customInfo = ORMCustomSerializedElementInfo.Default;
			}

			//start new element
			if (!WriteCustomizedStartElement(file, customInfo, classInfo.Name)) return;
			file.WriteAttributeString("id", element.Id.ToString().ToUpper());

			//write attributes
			for (int index = 0, count = attributes.Count; index < count; ++index)
			{
				Microsoft.VisualStudio.Modeling.MetaAttributeInfo attribute = (Microsoft.VisualStudio.Modeling.MetaAttributeInfo)attributes[index];

				if (!isCustom)
				{
					if (!attribute.CustomStorage)
					{
						object value = element.GetAttributeValue(attribute);
						file.WriteAttributeString(attribute.Name, value != null ? value.ToString() : null);
					}
				}
				else
				{
					SerializeAttribute
					(
						file,
						element,
						customElement,
						attribute,
						(supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSerializedAttributeInfo) != 0,
						null
					);
				}
			}

			for (int index = 0, count = rolesPlayed.Count; index < count; ++index)
			{
				MetaRoleInfo roleInfo = (MetaRoleInfo)rolesPlayed[index];

				if (!roleInfo.IsAggregate && !roleInfo.OppositeMetaRole.IsAggregate) //write link
				{
					if (roleInfo.MetaRelationship.MetaAttributesCount > 0) //link has attributes
					{
						IList links = element.GetElementLinks(roleInfo);

						foreach (ElementLink link in links)
						{
							if (link.MetaClass.Id == roleInfo.MetaRelationship.Id)
							{
								SerializeLink(file, link, element, null, roleInfo, false); //write indirect link
								break;
							}
						}
					}
					else //link does not have attributes
					{
						IList oppositeElements = element.GetCounterpartRolePlayers(roleInfo, roleInfo.OppositeMetaRole);

						//write direct links
						foreach (ModelElement oppositeElement in oppositeElements)
						{
							SerializeLink(file, null, element, oppositeElement, roleInfo, true);
						}
					}
				}
				else //write child
				{
					int combinedIndex;

					if (!roleGrouping || (combinedIndex = FindGUID(combinedElementInfo, roleInfo.Id)) < 0)
					{
						//write child element
						SerializeChildElement(file, element, roleInfo, roleInfo.Name);
					}
					else if (combinedElementInfo[combinedIndex].PushElement(element, roleInfo)) //push until full
					{
						ModelElement poppedElement;
						MetaRoleInfo poppedRoleInfo;
						string name = combinedElementInfo[combinedIndex].Name;

						//write combined child element
						while (combinedElementInfo[combinedIndex].PopElement(out poppedElement, out poppedRoleInfo))
						{
							SerializeChildElement(file, poppedElement, poppedRoleInfo, name);
							name = null;
						}
					}
				}
			}

			WriteCustomizedEndElement(file, customInfo);

			return;
		}
		/// <summary>
		/// New XML Serialization
		/// </summary>
		public void NewSerialize(Stream stream)
		{
			System.Xml.XmlWriterSettings xmlSettings = new XmlWriterSettings();
			System.Xml.XmlWriter file;
			Store store = myStore;
			ModelElement[] currentElements;
			ICollection values = store.SubStores.Values;
			int count;

			xmlSettings.IndentChars = "\t";
			xmlSettings.Indent = true;

			file = System.Xml.XmlWriter.Create("C:\\orm2.xml", xmlSettings);
			file.WriteStartElement("orm", "ORM2", "http://Schemas.Northface.edu/ORM/ORMCore");
			file.WriteAttributeString("xmlns", "http://Schemas.Northface.edu/ORM/ORMCore");

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
				SerializeElement(file, currentElements[i]);

			//serialize ORM diagram
			currentElements = GenerateElementArray(new IList[1] { store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid) });
			count = currentElements.Length;
			for (int i = 0; i < count; ++i)
				SerializeElement(file, currentElements[i]);

			//serialize data types
			currentElements = GenerateElementArray(new IList[1] { store.ElementDirectory.GetElements(DataType.MetaClassGuid) });
			count = currentElements.Length;
			for (int i = 0; i < count; ++i)
				SerializeElement(file, currentElements[i]);

			file.WriteEndElement();
			file.Close();

			myLinkGUIDs.Clear();

			return;
		}
		#endregion //New Serialization
	}
}
#endif // NEWSERIALIZE