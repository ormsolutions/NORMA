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

// It is much easier to write custom serialization xml if the
// serializer is allowed to spit the default names for the links.
// However, if there is no custom information on a link, we always
// block cross-model link serialization, so any extension models
// with links back into the core model are not written by default, making
// it more difficult to custom-serialize extensions than it needs to be.
// To temporary disable this, uncomment the following line.
//#define WRITE_ALL_DEFAULT_LINKS
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
	#region Public Enumerations
	#region ORMCustomSerializedElementSupportedOperations enum
	/// <summary>
	/// Supported operations for element custom serialization.
	/// </summary>
	[Flags]
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
		/// Custom property information is supported.
		/// </summary>
		PropertyInfo = 0x04,
		/// <summary>
		/// Custom link information is supported.
		/// </summary>
		LinkInfo = 0x08,
		/// <summary>
		/// The CustomSerializedChildRoleComparer method is supported
		/// </summary>
		CustomSortChildRoles = 0x10,
		/// <summary>
		/// Set if some of the properties are written as elements and others are written as properties.
		/// </summary>
		MixedTypedAttributes = 0x20,
		/// <summary>
		/// A child LinkInfo is actually the back link to the aggregating object. These
		/// elements have an id, but no ref.
		/// </summary>
		EmbeddingLinkInfo = 0x40,
	}
	#endregion // ORMCustomSerializedElementSupportedOperations enum
	#region ORMCustomSerializedElementWriteStyle enum
	/// <summary>
	/// Write style for element custom serialization.
	/// </summary>
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
		/// id, properties, and referencing child elements at this location.
		/// </summary>
		PrimaryLinkElement = 0x02,
		/// <summary>
		/// Used for embedding links. Write as a child element of the
		/// embedded object. Writes the link id. Any properties on the link
		/// and referencing child elements are written at this location.
		/// </summary>
		EmbeddingLinkElement = 0x03,
		/// <summary>
		/// Used for links where both roleplayers are specified with the link.
		/// This is generally required only for embedded link scenarios, which
		/// are not supported directly by the DSL framework.
		/// </summary>
		StandaloneLinkElement = 0x04,
		/// <summary>
		/// Used for links where both roleplayers are specified with the link.
		/// This is generally required only for embedded link scenarios, which
		/// are not supported directly by the DSL framework. When a standalone
		/// link is primary, then the element name is based on the link class,
		/// not the local link information, and id is written, and all other attributes
		/// and child elements of the link are written at this location. A link
		/// should be primary in at most one location in the file to avoid duplicate identifiers.
		/// </summary>
		PrimaryStandaloneLinkElement = 0x05,
	}
	#endregion // ORMCustomSerializedElementWriteStyle enum
	#region ORMCustomSerializedAttributeWriteStyle enum
	/// <summary>
	/// Write style for property custom serialization.
	/// </summary>
	public enum ORMCustomSerializedAttributeWriteStyle
	{
		/// <summary>
		/// Dont write.
		/// </summary>
		NotWritten = 0xFF,
		/// <summary>
		/// Write as an property.
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
	#endregion // ORMCustomSerializedAttributeWriteStyle enum
	#region ORMCustomSerializedElementMatchStyle enum
	/// <summary>
	/// An enum used for deserialization to determine if
	/// an element name and namespace is recognized by a
	/// custom serialized element.
	/// </summary>
	public enum ORMCustomSerializedElementMatchStyle
	{
		/// <summary>
		/// The element is not recognized, don't process it
		/// </summary>
		None,
		/// <summary>
		/// The element matched an property written out as an element.
		/// The DoubleTageName property (if it is not null) specifies the
		/// double tag name (the tag inside this element where the property
		/// data is stored). The guid identifying the DomainPropertyInfo is
		/// returned in the Guid property.
		/// </summary>
		Property,
		/// <summary>
		/// The element matches a single contained role. The guid identifying
		/// the DomainPropertyInfo is returned in the SingleOppositeDomainRoleGuid property.
		/// </summary>
		SingleOppositeDomainRole,
		/// <summary>
		/// The element matches a single contained role and the link must
		/// be created as an explicit subtype of the relationship specified by the
		/// domain role. The guid identifying the DomainPropertyInfo is returned in the
		/// SingleOppositeDomainRoleGuid property and the guid identifying the DomainRelationshipInfo
		/// is returned by the ExplicitRelationshipGuid property.
		/// </summary>
		SingleOppositeDomainRoleExplicitRelationshipType,
		/// <summary>
		/// The element matches more than one contained role. The guids identifying
		/// the roles are returned in the OppositeDomainRoleGuidCollection property
		/// </summary>
		MultipleOppositeDomainRoles,
		/// <summary>
		/// The element matches more than one contained role and the link must
		/// be created as an explicit subtype of the relationship specified by the
		/// meta role. The guids identifying the roles are returned in the
		/// OppositeDomainRoleGuidCollection property. The guid identifying the
		/// DomainRelationshipInfo is returned by the ExplicitRelationshipGuid property.
		/// </summary>
		MultipleOppositeMetaRolesExplicitRelationshipType,
	}
	#endregion // ORMCustomSerializedElementMatchStyle enum
	#endregion // Public Enumerations
	#region Public Classes
	#region ORMCustomSerializedInfo class
	/// <summary>
	/// Custom serialization information.
	/// </summary>
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
	#endregion // ORMCustomSerializedInfo class
	#region ORMCustomSerializedElementInfo class
	/// <summary>
	/// Custom serialization information for elements.
	/// </summary>
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
	#endregion // ORMCustomSerializedElementInfo class
	#region ORMCustomSerializedPropertyInfo class
	/// <summary>
	/// Custom serialization information for properties.
	/// </summary>
	public class ORMCustomSerializedPropertyInfo : ORMCustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedPropertyInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writting.</param>
		public ORMCustomSerializedPropertyInfo(ORMCustomSerializedAttributeWriteStyle writeStyle)
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
		public ORMCustomSerializedPropertyInfo(string customPrefix, string customName, string customNamespace, bool writeCustomStorage, ORMCustomSerializedAttributeWriteStyle writeStyle, string doubleTagName)
			: base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteCustomStorage = writeCustomStorage;
			myWriteStyle = writeStyle;
		}

		private bool myWriteCustomStorage;
		private ORMCustomSerializedAttributeWriteStyle myWriteStyle;

		/// <summary>
		/// Default ORMCustomSerializedPropertyInfo
		/// </summary>
		public static readonly ORMCustomSerializedPropertyInfo Default = new ORMCustomSerializedPropertyInfo();

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
	#endregion // ORMCustomSerializedPropertyInfo class
	#region ORMCustomSerializedContainerElementInfo class
	/// <summary>
	/// Custom serialization information for container elements.
	/// </summary>
	public class ORMCustomSerializedContainerElementInfo : ORMCustomSerializedElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedContainerElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public ORMCustomSerializedContainerElementInfo(string customName, params Guid[] childRoleIds)
			: base(null, customName, null, ORMCustomSerializedElementWriteStyle.Element, null)
		{
			myGuidList = childRoleIds;
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public ORMCustomSerializedContainerElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName, params Guid[] childRoleIds)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
		{
			myGuidList = childRoleIds;
		}

		private IList<Guid> myGuidList;

		/// <summary>
		/// Default ORMCustomSerializedContainerElementInfo
		/// </summary>
		public new static readonly ORMCustomSerializedContainerElementInfo Default = new ORMCustomSerializedContainerElementInfo();

		/// <summary>
		/// Test if the list of child elements contains the specified guid
		/// </summary>
		public bool ContainsGuid(Guid guid)
		{
			return myGuidList != null && myGuidList.Contains(guid);
		}
		/// <summary>
		/// Get the outer container of this element. Will be null unless this
		/// is a ORMCustomSerializedInnerContainerElementInfo instance
		/// </summary>
		public virtual ORMCustomSerializedContainerElementInfo OuterContainer
		{
			get
			{
				return null;
			}
		}
	}
	#endregion // ORMCustomSerializedContainerElementInfo class
	#region ORMCustomSerializedStandaloneLinkInfo class
	/// <summary>
	/// A class with information for serializing a standalone link element
	/// </summary>
	public class ORMCustomSerializedStandaloneLinkElementInfo : ORMCustomSerializedElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedStandaloneLinkElementInfo()
		{
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="standaloneRelationship">The domain role ideas of opposite child elements.</param>
		public ORMCustomSerializedStandaloneLinkElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName, ORMCustomSerializedStandaloneRelationship standaloneRelationship)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
		{
			myRelationship = standaloneRelationship;
		}
		private ORMCustomSerializedStandaloneRelationship myRelationship;
		/// <summary>
		/// Get the <see cref="ORMCustomSerializedStandaloneRelationship"/> information associated with this link.
		/// </summary>
		public ORMCustomSerializedStandaloneRelationship StandaloneRelationship
		{
			get
			{
				return myRelationship;
			}
		}
		/// <summary>
		/// Base override indicating when this object is in its default state (no data available)
		/// </summary>
		public override bool IsDefault
		{
			get
			{
				return myRelationship.DomainClassId == Guid.Empty && base.IsDefault;
			}
		}
	}
	#endregion // ORMCustomSerializedStandaloneLinkElementInfo class
	#region ORMCustomSerializedInnerContainerElementInfo class
	/// <summary>
	/// Custom serialization information for nested container elements.
	/// </summary>
	public class ORMCustomSerializedInnerContainerElementInfo : ORMCustomSerializedContainerElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected ORMCustomSerializedInnerContainerElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="outerContainer">The outer container</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public ORMCustomSerializedInnerContainerElementInfo(string customName, ORMCustomSerializedContainerElementInfo outerContainer, params Guid[] childRoleIds)
			: base(null, customName, null, ORMCustomSerializedElementWriteStyle.Element, null, childRoleIds)
		{
			myOuterContainer = outerContainer;
		}
		/// <summary>
		/// Main Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="outerContainer">The outer container</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public ORMCustomSerializedInnerContainerElementInfo(string customPrefix, string customName, string customNamespace, ORMCustomSerializedElementWriteStyle writeStyle, string doubleTagName, ORMCustomSerializedContainerElementInfo outerContainer, params Guid[] childRoleIds)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName, childRoleIds)
		{
			myOuterContainer = outerContainer;
		}
		private ORMCustomSerializedContainerElementInfo myOuterContainer;
		/// <summary>
		/// Get the container element for this item
		/// </summary>
		public override ORMCustomSerializedContainerElementInfo OuterContainer
		{
			get
			{
				return myOuterContainer;
			}
		}
	}
	#endregion // ORMCustomSerializedInnerContainerElementInfo class
	#region ORMCustomSerializedElementMatch struct
	/// <summary>
	/// Data returned by IORMCustomSerializedElement.MapElementName.
	/// </summary>
	public struct ORMCustomSerializedElementMatch
	{
		/// <summary>
		/// Holds a single role guid, or the explicit relationship guid
		/// </summary>
		private Guid mySingleGuid;
		/// <summary>
		/// Holds multiple guids, or the single role guid if an explicit
		/// relationship guid is provided
		/// </summary>
		private Guid[] myMultiGuids;
		/// <summary>
		/// Hold the standalone relationship information for this match.
		/// Stored as an array to minimize storage impact when not used.
		/// </summary>
		private ORMCustomSerializedStandaloneRelationship[] myStandaloneRelationship;
		private ORMCustomSerializedElementMatchStyle myMatchStyle;
		private const ORMCustomSerializedElementMatchStyle StyleMask = (ORMCustomSerializedElementMatchStyle)0xFFFF;
		private const ORMCustomSerializedElementMatchStyle AllowDuplicatesBit = (ORMCustomSerializedElementMatchStyle)((int)StyleMask + 1);
		private string myDoubleTagName;
		/// <summary>
		/// The element was recognized as a meta property.
		/// </summary>
		/// <param name="metaAttributeGuid">The guid identifying the meta property</param>
		/// <param name="doubleTagName">the name of the double tag, if any</param>
		public void InitializeAttribute(Guid metaAttributeGuid, string doubleTagName)
		{
			mySingleGuid = metaAttributeGuid;
			myMatchStyle = ORMCustomSerializedElementMatchStyle.Property;
			myDoubleTagName = (doubleTagName != null && doubleTagName.Length != 0) ? doubleTagName : null;
			myStandaloneRelationship = null;
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(ORMCustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(bool allowDuplicates, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(bool allowDuplicates, ORMCustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
		}
		private void InitializeRoles(bool allowDuplicates, ORMCustomSerializedStandaloneRelationship[] standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			Debug.Assert(oppositeDomainRoleIds != null && oppositeDomainRoleIds.Length != 0);
			if (oppositeDomainRoleIds.Length == 1)
			{
				mySingleGuid = oppositeDomainRoleIds[0];
				myMultiGuids = null;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRole;
			}
			else
			{
				mySingleGuid = Guid.Empty;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.MultipleOppositeDomainRoles;
			}
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myDoubleTagName = null;
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(ORMCustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(bool allowDuplicates, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(bool allowDuplicates, ORMCustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
		}
		private void InitializeRoles(bool allowDuplicates, ORMCustomSerializedStandaloneRelationship[] standaloneRelationship, Guid oppositeDomainRoleId)
		{
			mySingleGuid = oppositeDomainRoleId;
			myMultiGuids = null;
			myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRole;
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myStandaloneRelationship = standaloneRelationship;
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type
		/// </summary>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type
		/// </summary>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
		}
		private void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship[] standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			Debug.Assert(oppositeDomainRoleIds != null && oppositeDomainRoleIds.Length != 0);
			if (oppositeDomainRoleIds.Length == 1)
			{
				mySingleGuid = explicitRelationshipGuid;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType;
			}
			else
			{
				mySingleGuid = explicitRelationshipGuid;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType;
			}
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myDoubleTagName = null;
			myStandaloneRelationship = standaloneRelationship;
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type.
		/// Optimized overload for 1 element.
		/// </summary>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type.
		/// Optimized overload for 1 element.
		/// </summary>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type.
		/// Optimized overload for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player of an explicit link type.
		/// Optimized overload for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitRelationshipGuid">The guid of the meta relationship to create</param>
		/// <param name="standaloneRelationship">A <see cref="ORMCustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, new ORMCustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
		}
		private void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, ORMCustomSerializedStandaloneRelationship[] standaloneRelationship, Guid oppositeDomainRoleId)
		{
			mySingleGuid = explicitRelationshipGuid;
			myMultiGuids = new Guid[] { oppositeDomainRoleId };
			myMatchStyle = ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType;
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myStandaloneRelationship = standaloneRelationship;
		}
		/// <summary>
		/// The guid identifying the meta property. Valid for a match
		/// style of Property.
		/// </summary>
		public Guid DomainPropertyId
		{
			get
			{
				return ((myMatchStyle & StyleMask) == ORMCustomSerializedElementMatchStyle.Property) ? mySingleGuid : Guid.Empty;
			}
		}
		/// <summary>
		/// The guid identifying the opposite meta role if there is only
		/// one matching meta role. Valid for a match style of SingleOppositeDomainRole.
		/// </summary>
		public Guid SingleOppositeDomainRoleId
		{
			get
			{
				switch (myMatchStyle & StyleMask)
				{
					case ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRole:
						return mySingleGuid;
					case ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
						return myMultiGuids[0];
					default:
						return Guid.Empty;
				}
			}
		}
		/// <summary>
		/// The guids identifying multiple opposite meta roles. Valid for a match
		/// style of MultipleOppositeDomainRoles.
		/// </summary>
		public IList<Guid> OppositeDomainRoleIdCollection
		{
			get
			{
				switch (myMatchStyle & StyleMask)
				{
					case ORMCustomSerializedElementMatchStyle.MultipleOppositeDomainRoles:
					case ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
						return myMultiGuids;
					default:
						return null;
				}
			}
		}
		/// <summary>
		/// The guid identifying the meta relationship of the explicit relationship
		/// type to create. Validate for match styles of SingleOppositeDomainRoleExplicitRelationshipType
		/// and MultipleOppositeMetaRolesExplicitRelationshipType.
		/// </summary>
		public Guid ExplicitRelationshipGuid
		{
			get
			{
				switch (myMatchStyle & StyleMask)
				{
					case ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
					case ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
						return mySingleGuid;
					default:
						return Guid.Empty;
				}
			}
		}
		/// <summary>
		/// Return the standalone relationship info used to create this link
		/// </summary>
		public ORMCustomSerializedStandaloneRelationship? StandaloneRelationship
		{
			get
			{
				ORMCustomSerializedStandaloneRelationship[] relationshipInfo = myStandaloneRelationship;
				return (relationshipInfo != null) ? relationshipInfo[0] : (ORMCustomSerializedStandaloneRelationship?)null;
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
				return myMatchStyle & StyleMask;
			}
		}
		/// <summary>
		/// The double tag name for an property. null if the MatchStyle
		/// is not Property or if there is no double tag for this element.
		/// </summary>
		public string DoubleTagName
		{
			get
			{
				return myDoubleTagName;
			}
		}
		/// <summary>
		/// The relationship between the two specified elements can
		/// have duplicate elements
		/// </summary>
		public bool AllowDuplicates
		{
			get
			{
				return 0 != (myMatchStyle & AllowDuplicatesBit);
			}
		}
	}
	#endregion // ORMCustomSerializedElementMatch struct
	#endregion // Public Classes
	#region Public Structures
	/// <summary>
	/// Used with <see cref="IORMCustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent relationships that are not serialized under any model.
	/// </summary>
	public struct ORMCustomSerializedRootRelationshipContainer
	{
		private string myContainerName;
		private string myContainerPrefix;
		private string myContainerNamespace;
		private ORMCustomSerializedStandaloneRelationship[] myRelationships;
		/// <summary>
		/// The name of the container XML element
		/// </summary>
		public string ContainerName
		{
			get { return myContainerName; }
		}
		/// <summary>
		/// The name of a non-default prefix for the container prefix
		/// </summary>
		public string ContainerPrefix
		{
			get { return myContainerPrefix; }
		}
		/// <summary>
		/// The name of a non-default prefix for the container prefix
		/// </summary>
		public string ContainerNamespace
		{
			get { return myContainerNamespace; }
		}
		/// <summary>
		/// The relationship classes serialized within the container. Represented
		/// by an array of <see cref="ORMCustomSerializedStandaloneRelationship"/> elements
		/// </summary>
		public ORMCustomSerializedStandaloneRelationship[] GetRelationshipClasses()
		{
			return myRelationships;
		}
		/// <summary>
		/// Create a new <see cref="ORMCustomSerializedRootRelationshipContainer"/> 
		/// </summary>
		/// <param name="containerPrefix">Value returned by the <see cref="ContainerPrefix"/> property</param>
		/// <param name="containerName">Value returned by the <see cref="ContainerName"/> property</param>
		/// <param name="containerNamespace">Value returned by the <see cref="ContainerNamespace"/> property</param>
		/// <param name="relationships">Array of relationships returned by the <see cref="GetRelationshipClasses"/> method</param>
		public ORMCustomSerializedRootRelationshipContainer(string containerPrefix, string containerName, string containerNamespace, ORMCustomSerializedStandaloneRelationship[] relationships)
		{
			myContainerName = containerName;
			myContainerPrefix = containerPrefix;
			myContainerNamespace = containerNamespace;
			myRelationships = relationships;
		}
		/// <summary>
		/// Find a container based on name and namespace
		/// </summary>
		/// <param name="containers">An array of containers</param>
		/// <param name="containerName">The xml element name for the container element</param>
		/// <param name="containerNamespace">The xml namespace for the container element</param>
		/// <returns><see cref="Nullable{ORMCustomSerializedRootRelationshipContainer}"/></returns>
		public static ORMCustomSerializedRootRelationshipContainer? Find(ORMCustomSerializedRootRelationshipContainer[] containers, string containerName, string containerNamespace)
		{
			if (containers != null)
			{
				for (int i = 0; i < containers.Length; ++i)
				{
					ORMCustomSerializedRootRelationshipContainer currentContainer = containers[i];
					if (currentContainer.ContainerName == containerName && currentContainer.ContainerNamespace == containerNamespace)
					{
						return currentContainer;
					}
				}
			}
			return null;
		}
	}
	/// <summary>
	/// Used with the <see cref="IORMCustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent a single role to be written to a root link element as represented by the <see cref="ORMCustomSerializedStandaloneRelationship"/> structure.
	/// </summary>
	public struct ORMCustomSerializedStandaloneRelationshipRole
	{
		private string myAttributeName;
		private Guid myDomainRoleId;
		/// <summary>
		/// The name of the serialized attribute
		/// </summary>
		public string AttributeName
		{
			get { return myAttributeName; }
		}
		/// <summary>
		/// The identifier for the <see cref="DomainRoleInfo"/> describing this role
		/// </summary>
		public Guid DomainRoleId
		{
			get { return myDomainRoleId; }
		}
		/// <summary>
		/// Create a new <see cref="ORMCustomSerializedStandaloneRelationshipRole"/>
		/// </summary>
		/// <param name="attributeRoleName">Value corresponding to the <see cref="AttributeName"/> property</param>
		/// <param name="domainRoleId">Value corresponding to the <see cref="DomainRoleId"/> property</param>
		public ORMCustomSerializedStandaloneRelationshipRole(string attributeRoleName, Guid domainRoleId)
		{
			myAttributeName = attributeRoleName;
			myDomainRoleId = domainRoleId;
		}
	}
	/// <summary>
	/// Used with the <see cref="IORMCustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent a single relationship serialized inside a <see cref="ORMCustomSerializedRootRelationshipContainer"/>.
	/// </summary>
	public struct ORMCustomSerializedStandaloneRelationship
	{
		private string myElementName;
		private string myElementPrefix;
		private string myElementNamespace;
		private Guid myDomainClassId;
		private ORMCustomSerializedStandaloneRelationshipRole[] myRoles;
		/// <summary>
		/// The name of the link element
		/// </summary>
		public string ElementName
		{
			get { return myElementName; }
		}
		/// <summary>
		/// The xml namespace of the link element
		/// </summary>
		public string ElementNamespace
		{
			get { return myElementNamespace; }
		}
		/// <summary>
		/// The xml prefix of the link element
		/// </summary>
		public string ElementPrefix
		{
			get { return myElementPrefix; }
		}
		/// <summary>
		/// The domain class for the link being created
		/// </summary>
		public Guid DomainClassId
		{
			get { return myDomainClassId; }
		}
		/// <summary>
		/// Whether or not this representation of the link forms the
		/// primary definition for the link. If this is true, then the
		/// element name is based on the underlying link class (<see cref="ElementName"/>
		/// is ignored), and all attributes and child elements are written with the element.
		/// </summary>
		public bool IsPrimaryLinkElement
		{
			get { return myElementName == null; }
		}
		/// <summary>
		/// Get the <see cref="ORMCustomSerializedStandaloneRelationshipRole"/> representing
		/// how the source and target references are to be written for this element.
		/// </summary>
		public ORMCustomSerializedStandaloneRelationshipRole[] GetRoles()
		{
			return myRoles;
		}
		/// <summary>
		/// Create a new <see cref="ORMCustomSerializedStandaloneRelationship"/>
		/// </summary>
		/// <param name="domainClassId">Value for the <see cref="DomainClassId"/> property</param>
		/// <param name="roles">Value for the <see cref="GetRoles"/> method</param>
		/// <param name="elementPrefix">Value for the <see cref="ElementPrefix"/> property</param>
		/// <param name="elementName">Value for the <see cref="ElementName"/> property. If the name is null then this is the primary link element and all name information is retrieved from the class.</param>
		/// <param name="elementNamespace">Value for the <see cref="ElementNamespace"/> property</param>
		public ORMCustomSerializedStandaloneRelationship(Guid domainClassId, ORMCustomSerializedStandaloneRelationshipRole[] roles, string elementPrefix, string elementName, string elementNamespace)
		{
			myDomainClassId = domainClassId;
			myRoles = roles;
			myElementPrefix = elementPrefix;
			myElementName = elementName;
			myElementNamespace = elementNamespace;
		}
		/// <summary>
		/// Find a relationship based on name and namespace
		/// </summary>
		/// <param name="relationships">An array of relationship elements</param>
		/// <param name="elementName">The xml element name for the relationship element</param>
		/// <param name="xmlNamespace">The xml namespace for the relationship element</param>
		/// <returns><see cref="Nullable{ORMCustomSerializedStandaloneRelationship}"/></returns>
		public static ORMCustomSerializedStandaloneRelationship? Find(ORMCustomSerializedStandaloneRelationship[] relationships, string elementName, string xmlNamespace)
		{
			if (relationships != null)
			{
				for (int i = 0; i < relationships.Length; ++i)
				{
					ORMCustomSerializedStandaloneRelationship currentRelationship = relationships[i];
					if (currentRelationship.ElementName == elementName && currentRelationship.ElementNamespace == xmlNamespace)
					{
						return currentRelationship;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Find a relationship based on name and namespace
		/// </summary>
		/// <param name="relationships">An array of relationship elements</param>
		/// <param name="relationshipDomainClassId">The value corresponding to the class of this relationship.</param>
		/// <returns><see cref="Nullable{ORMCustomSerializedStandaloneRelationship}"/></returns>
		public static ORMCustomSerializedStandaloneRelationship? Find(ORMCustomSerializedStandaloneRelationship[] relationships, Guid relationshipDomainClassId)
		{
			if (relationships != null)
			{
				for (int i = 0; i < relationships.Length; ++i)
				{
					ORMCustomSerializedStandaloneRelationship currentRelationship = relationships[i];
					if (currentRelationship.DomainClassId == relationshipDomainClassId)
					{
						return currentRelationship;
					}
				}
			}
			return null;
		}
	}
	#endregion // Public Structures
	#region Public Interfaces
	#region IORMCustomSerializedDomainModel interface
	/// <summary>
	/// The interface for getting custom element namespaces.
	/// </summary>
	public interface IORMCustomSerializedDomainModel
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
		/// Return the relationship containers for all root links.
		/// </summary>
		ORMCustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers();
		/// <summary>
		/// Determine if a class or relationship should be serialized. This allows
		/// the serialization engine to do a meta-level sanity check before retrieving
		/// elements, and reduces the number of 'NotWritten' elements in the
		/// serialization specification file.
		/// </summary>
		/// <param name="store">The store to check</param>
		/// <param name="classInfo">The class or relationship to test</param>
		/// <returns>true if the element should be serialized</returns>
		bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo);
		/// <summary>
		/// Map an xml namespace name and element name to a meta class guid
		/// </summary>
		/// <param name="xmlNamespace">The namespace of a top-level element (directly
		/// inside the ORM2 tag)</param>
		/// <param name="elementName">The name of the element to match</param>
		/// <returns>The guid of a DomainClassInfo, or Guid.Empty if not recognized</returns>
		Guid MapRootElement(string xmlNamespace, string elementName);
		/// <summary>
		/// Map an xml namespace name and element name to a meta class guid
		/// </summary>
		/// <param name="xmlNamespace">The namespace of the xml element</param>
		/// <param name="elementName">The name of the element to match</param>
		/// <returns>A meta class guid, or Guid.Empty if the name is not recognized</returns>
		Guid MapClassName(string xmlNamespace, string elementName);
	}
	#endregion // IORMCustomSerializedDomainModel interface
	#region IORMCustomSerializedElement interface
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
		ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo();
		/// <summary>
		/// Returns custom serialization information for elements.
		/// </summary>
		ORMCustomSerializedElementInfo CustomSerializedElementInfo { get;}
		/// <summary>
		/// Returns custom serialization information for properties.
		/// </summary>
		/// <param name="domainPropertyInfo">The property info.</param>
		/// <param name="rolePlayedInfo">If this is implemented on a ElementLink-derived class, then the
		/// played role is the role player containing the reference to the opposite role. Always null for a
		/// class element.</param>
		/// <returns>Custom serialization information for properties.</returns>
		ORMCustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo);
		/// <summary>
		/// Returns custom serialization information for links.
		/// </summary>
		/// <param name="rolePlayedInfo">The role played.</param>
		/// <param name="elementLink">The link instance</param>
		/// <returns>Custom serialization information for links.</returns>
		ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink);
		/// <summary>
		/// Get a comparer to sort custom role elements. Affects the element order
		/// for nested child (aggregated) and link (referenced) elements
		/// </summary>
		IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer { get;}
		/// <summary>
		/// Attempt to map an element name to a custom serialized child element.
		/// </summary>
		/// <param name="elementNamespace">The full xml namespace of the element to match. Note
		/// that using prefixes is not robust, so the full namespace needs to be specified.</param>
		/// <param name="elementName">The local name of the element</param>
		/// <param name="containerNamespace">The full xml namespace of the container to match. A
		/// container element is an element with no id or ref parameter.</param>
		/// <param name="containerName">The local name of the container</param>
		/// <param name="outerContainerNamespace">The full xml namespace of the outer container to match. An
		/// outer container element is an element with no id or ref parameter that contains the container.</param>
		/// <param name="outerContainerName">The local name of the outer container</param>
		/// <returns>ORMCustomSerializedElementMatch. Use the MatchStyle property to determine levels of success.</returns>
		ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName);
		/// <summary>
		/// Attempt to map an property name to a custom serialized property
		/// for this element.
		/// </summary>
		/// <param name="xmlNamespace">The full xml namespace of the element to match. Note
		/// that using prefixes is not robust, so the full namespace needs to be specified.</param>
		/// <param name="attributeName">The local name of the property</param>
		/// <returns>A DomainPropertyId, or Guid.Empty. Use Guid.IsEmpty to test.</returns>
		Guid MapAttribute(string xmlNamespace, string attributeName);
		/// <summary>
		/// Check the current state of the object to determine
		/// if it should be serialized or not.
		/// </summary>
		/// <returns>false to block serialization</returns>
		bool ShouldSerialize();
	}
	#endregion // IORMCustomSerializedElement interface
	#endregion Public Interfaces
	#region Serialization Routines
	/// <summary>
	/// Serialization routines
	/// </summary>
	public partial class ORMSerializer
	{
		#region Constants
		// These need to be "static readonly" rather than "const" so that other assemblies compiled against us
		// can detect the latest version at runtime.

		/// <summary>
		/// The standard prefix for the prefix used on the root node of the ORM document
		/// </summary>
		public static readonly string RootXmlPrefix = "ormRoot";
		/// <summary>
		/// The tag name for the element used as the root node of the ORM document
		/// </summary>
		public static readonly string RootXmlElementName = "ORM2";
		/// <summary>
		/// The namespace for the root node of the ORM document
		/// </summary>
		public static readonly string RootXmlNamespace = "http://schemas.neumont.edu/ORM/2006-04/ORMRoot";
		#endregion // Constants

		/// <summary>
		/// Used for sorting.
		/// </summary>
		/// <param name="writeStyle">An property write style.</param>
		/// <returns>A number to sort with.</returns>
		private static int PropertyWriteStylePriority(ORMCustomSerializedAttributeWriteStyle writeStyle)
		{
			switch (writeStyle)
			{
				case ORMCustomSerializedAttributeWriteStyle.Attribute:
					return 0;
				case ORMCustomSerializedAttributeWriteStyle.Element:
				case ORMCustomSerializedAttributeWriteStyle.DoubleTaggedElement:
					return 1;
			}
			return 2;
		}
		/// <summary>
		/// Used for serializing properties.
		/// </summary>
		/// <param name="guid">The GUID to convert.</param>
		/// <returns>An XML encoded string.</returns>
		private static string ToXml(Guid guid)
		{
			return '_' + XmlConvert.ToString(guid).ToUpperInvariant();
		}
		/// <summary>
		/// Serializes a property value to XML.
		/// </summary>
		/// <param name="element">The <see cref="ModelElement"/> containing the property to be serialized.</param>
		/// <param name="property">The <see cref="DomainPropertyInfo"/> to be serialized.</param>
		/// <returns>An XSD-appropriate <see cref="String"/> that represents the serialized value of the property.</returns>
		private static string ToXml(ModelElement element, DomainPropertyInfo property)
		{
			object value = property.GetValue(element);
			if (value == null)
			{
				return null;
			}
			Type type = property.PropertyType;
			if (!type.IsEnum)
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Empty:
					case TypeCode.DBNull:
						return null;
					case TypeCode.String:
						return (string)value;
					case TypeCode.DateTime:
						return XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc);
					case TypeCode.UInt64:
						return XmlConvert.ToString((ulong)value);
					case TypeCode.Int64:
						return XmlConvert.ToString((long)value);
					case TypeCode.UInt32:
						return XmlConvert.ToString((uint)value);
					case TypeCode.Int32:
						return XmlConvert.ToString((int)value);
					case TypeCode.UInt16:
						return XmlConvert.ToString((ushort)value);
					case TypeCode.Int16:
						return XmlConvert.ToString((short)value);
					case TypeCode.Byte:
						return XmlConvert.ToString((byte)value);
					case TypeCode.SByte:
						return XmlConvert.ToString((sbyte)value);
					case TypeCode.Char:
						return XmlConvert.ToString((char)value);
					case TypeCode.Boolean:
						return XmlConvert.ToString((bool)value);
					case TypeCode.Decimal:
						return XmlConvert.ToString((decimal)value);
					case TypeCode.Double:
						return XmlConvert.ToString((double)value);
					case TypeCode.Single:
						return XmlConvert.ToString((float)value);
				}
				if (type == typeof(Guid))
				{
					return ToXml((Guid)value);
				}
				else if (type == typeof(TimeSpan))
				{
					return XmlConvert.ToString((TimeSpan)value);
				}
			}
			DomainTypeDescriptor.TypeDescriptorContext context = DomainTypeDescriptor.CreateTypeDescriptorContext(element, property);
			return context.PropertyDescriptor.Converter.ConvertToInvariantString(context, value);
		}
		/// <summary>
		/// Used for serializing child elements.
		/// </summary>
		/// <param name="childElementInfo">The child element info to search.</param>
		/// <param name="guid">The GUID to find.</param>
		/// <returns>An index or -1.</returns>
		private static int FindGuid(ORMCustomSerializedContainerElementInfo[] childElementInfo, Guid guid)
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
		/// Sorts mixed typed properties.
		/// </summary>
		/// <param name="customElement">The element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="properties">The element's properties.</param>
		private static void SortProperties(IORMCustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, ref IList<DomainPropertyInfo> properties)
		{
			int propertyCount = properties.Count;
			if (propertyCount > 1)
			{
				ORMCustomSerializedPropertyInfo[] customInfo = new ORMCustomSerializedPropertyInfo[propertyCount];
				int[] indices = new int[propertyCount];
				for (int i = 0; i < propertyCount; ++i)
				{
					indices[i] = i;
					customInfo[i] = customElement.GetCustomSerializedPropertyInfo(properties[i], rolePlayedInfo);
				}
				Array.Sort<int>(indices, delegate(int index1, int index2)
				{
					ORMCustomSerializedPropertyInfo customInfo1 = customInfo[index1];
					ORMCustomSerializedPropertyInfo customInfo2 = customInfo[index2];
					int ws0 = PropertyWriteStylePriority(customInfo1.WriteStyle);
					int ws1 = PropertyWriteStylePriority(customInfo2.WriteStyle);

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
				for (int i = 0; i < propertyCount; ++i)
				{
					if (indices[i] != i)
					{
						DomainPropertyInfo[] reorderedList = new DomainPropertyInfo[propertyCount];
						for (int j = 0; j < propertyCount; ++j)
						{
							reorderedList[indices[j]] = properties[j];
						}
						properties = reorderedList;
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
		/// <param name="containerInfo">The customized tag info for a container element.</param>
		/// <param name="defaultPrefix">The default prefix.</param>
		/// <param name="defaultName">The default tag name.</param>
		/// <returns>true if the begin element tag was written.</returns>
		private static bool WriteCustomizedStartElement(XmlWriter file, ORMCustomSerializedElementInfo customInfo, ORMCustomSerializedElementInfo containerInfo, string defaultPrefix, string defaultName)
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

							if (containerInfo != null)
							{
								WriteCustomizedStartElement(file, containerInfo, null, defaultPrefix, "");
							}
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

				if (containerInfo != null)
				{
					WriteCustomizedStartElement(file, containerInfo, null, defaultPrefix, "");
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
				Debug.Assert(containerInfo == null, "Should not have an outer container if an inner container is not supplied");
				file.WriteStartElement(defaultPrefix, defaultName, null);
			}
			return true;
		}
		/// <summary>
		/// Writes a customized end element tag.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="customInfo">The customized tag info.</param>
		private static void WriteCustomizedEndElement(XmlWriter file, ORMCustomSerializedElementInfo customInfo)
		{
			if (customInfo != null)
			{
				switch (customInfo.WriteStyle)
				{
#if DEBUG
					case ORMCustomSerializedElementWriteStyle.NotWritten:
						{
							Debug.Fail("WriteCustomizedEndElement - ORMCustomSerializedElementWriteStyle.DontWrite");
							throw new InvalidOperationException();
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
		/// <returns>IORMCustomSerializedDomainModel, or null</returns>
		private static IORMCustomSerializedDomainModel GetParentModel(ModelElement element)
		{
			return element.Store.GetDomainModel(element.GetDomainClass().DomainModel.Id) as IORMCustomSerializedDomainModel;
		}
		/// <summary>
		/// Determine based on the type of role and opposite role player if any elements of
		/// the given type should be serialized.
		/// </summary>
		/// <param name="parentModel">The parent model of an element</param>
		/// <param name="role">The role played</param>
		/// <returns>true if serialization should continue</returns>
		private static bool ShouldSerializeDomainRole(IORMCustomSerializedDomainModel parentModel, DomainRoleInfo role)
		{
			if (parentModel == null)
			{
				return true;
			}
			Store store = ((DomainModel)parentModel).Store;
			return parentModel.ShouldSerializeDomainClass(store, role.DomainRelationship) && parentModel.ShouldSerializeDomainClass(store, role.OppositeDomainRole.RolePlayer);
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
			IORMCustomSerializedDomainModel parentModel = GetParentModel(element);
			if (parentModel != null)
			{
				retVal = parentModel.DefaultElementPrefix;
			}
			return retVal;
		}
		/// <summary>
		/// Serializes a property.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="customElement">The element as a custom element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="property">The element's property to write.</param>
		/// <param name="isCustomProperty">true if the property has custom info.</param>
		private static void SerializeProperties(XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, DomainPropertyInfo property, bool isCustomProperty)
		{
			if (!isCustomProperty)
			{
				if (property.Kind == DomainPropertyKind.Normal)
				{
					file.WriteAttributeString(property.Name, ToXml(element, property));
				}
				return;
			}

			ORMCustomSerializedPropertyInfo customInfo = customElement.GetCustomSerializedPropertyInfo(property, rolePlayedInfo);

			if (property.Kind == DomainPropertyKind.Normal || customInfo.WriteCustomStorage)
			{
				if (customInfo.WriteStyle != ORMCustomSerializedAttributeWriteStyle.Attribute || file.WriteState != WriteState.Element)
				{
					switch (customInfo.WriteStyle)
					{
						default:
							{
								file.WriteElementString
								(
									customInfo.CustomPrefix != null ? customInfo.CustomPrefix : DefaultElementPrefix(element),
									customInfo.CustomName != null ? customInfo.CustomName : property.Name,
									customInfo.CustomNamespace,
									ToXml(element, property)
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
								string name = (customInfo.CustomName != null ? customInfo.CustomName : property.Name);

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
									ToXml(element, property)
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
						customInfo.CustomName != null ? customInfo.CustomName : property.Name,
						customInfo.CustomNamespace,
						ToXml(element, property)
					);
				}
			}

			return;
		}
		/// <summary>
		/// Serializes all properties of an element.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="customElement">The element as a custom element.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		/// <param name="properties">The element's properties.</param>
		/// <param name="hasCustomAttributes">true if the element has properties with custom info.</param>
		private static void SerializeProperties(XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, IList<DomainPropertyInfo> properties, bool hasCustomAttributes)
		{
			for (int index = 0, count = properties.Count; index < count; ++index)
			{
				DomainPropertyInfo property = properties[index];

				SerializeProperties
				(
					file,
					element,
					customElement,
					rolePlayedInfo,
					property,
					hasCustomAttributes
				);
			}
			return;
		}
		/// <summary>
		/// Serializes a link. Helper function for <see cref="SerializeChildElement"/>
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="link">The link. Should be verified with <see cref="ShouldSerializeElement"/> before this call.</param>
		/// <param name="rolePlayer">The role player. Should be verified with <see cref="ShouldSerializeElement"/> before this call.</param>
		/// <param name="oppositeRolePlayer">The opposite role player.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		private void SerializeLink(XmlWriter file, ElementLink link, ModelElement rolePlayer, ModelElement oppositeRolePlayer, DomainRoleInfo rolePlayedInfo)
		{
			ORMCustomSerializedElementSupportedOperations supportedOperations = ORMCustomSerializedElementSupportedOperations.None;
			ORMCustomSerializedElementInfo customInfo = ORMCustomSerializedElementInfo.Default;
			IList<DomainPropertyInfo> properties = null;
			string defaultPrefix;
			bool hasCustomAttributes = false;

			IORMCustomSerializedElement rolePlayerCustomElement = rolePlayer as IORMCustomSerializedElement;
			IORMCustomSerializedElement customElement = rolePlayerCustomElement;
			ORMCustomSerializedElementWriteStyle writeStyle = ORMCustomSerializedElementWriteStyle.Element;
			ORMCustomSerializedElementInfo oppositeRolePlayerElementInfo = null;
			bool aggregatingLink = false;
			bool standaloneLink = false;
			bool writeContents = customElement != null &&
				0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) &&
				((writeStyle = (oppositeRolePlayerElementInfo = customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeDomainRole, link)).WriteStyle) == ORMCustomSerializedElementWriteStyle.PrimaryLinkElement ||
				(standaloneLink = writeStyle == ORMCustomSerializedElementWriteStyle.PrimaryStandaloneLinkElement) ||
				(aggregatingLink = writeStyle == ORMCustomSerializedElementWriteStyle.EmbeddingLinkElement));

			if (!standaloneLink)
			{
				standaloneLink = writeStyle == ORMCustomSerializedElementWriteStyle.StandaloneLinkElement;
			}

			ORMCustomSerializedStandaloneRelationshipRole[] roles = null;
			if (standaloneLink)
			{
				roles = ((ORMCustomSerializedStandaloneLinkElementInfo)oppositeRolePlayerElementInfo).StandaloneRelationship.GetRoles();
				// UNDONE: This precludes using EmbeddingLinkElement inside a StandaloneLinkfs
				link = (ElementLink)oppositeRolePlayer;
			}
			
			if (writeContents)
			{
				customElement = link as IORMCustomSerializedElement;
				properties = link.GetDomainClass().AllDomainProperties;
				defaultPrefix = DefaultElementPrefix(link);
			}
			else
			{
				defaultPrefix = DefaultElementPrefix(rolePlayer);
			}

			if (customElement != null)
			{
				supportedOperations = customElement.SupportedCustomSerializedOperations;

				if (0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes) && properties != null)
				{
					SortProperties(customElement, rolePlayedInfo, ref properties);
				}
				hasCustomAttributes = (supportedOperations & ORMCustomSerializedElementSupportedOperations.PropertyInfo) != 0;

				if (writeContents)
				{
					if (oppositeRolePlayerElementInfo != null)
					{
						customInfo = oppositeRolePlayerElementInfo;
					}
				}
				else if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.LinkInfo) != 0)
				{
					customInfo = oppositeRolePlayerElementInfo;
					IORMCustomSerializedDomainModel rolePlayerParentModel;
					if (customInfo.IsDefault &&
						((rolePlayerParentModel = GetParentModel(rolePlayer)) != GetParentModel(oppositeRolePlayer) ||
						rolePlayerParentModel != GetParentModel(link)))
					{
						return;
					}
				}
#if !WRITE_ALL_DEFAULT_LINKS
				else
				{
					IORMCustomSerializedDomainModel rolePlayerParentModel = GetParentModel(rolePlayer);
					if (rolePlayerParentModel != GetParentModel(oppositeRolePlayer) ||
						rolePlayerParentModel != GetParentModel(link))
					return;
				}
#endif // WRITE_ALL_DEFAULT_LINKS
			}
			// UNDONE: NOW Write start element off roleplayer, not link, for standalone primary link element
			if (!WriteCustomizedStartElement(file, customInfo, null, defaultPrefix, standaloneLink ? link.GetDomainClass().Name : string.Concat(rolePlayedInfo.DomainRelationship.Name, ".", rolePlayedInfo.OppositeDomainRole.Name)))
			{
				return;
			}

			if (standaloneLink)
			{
				for (int i = 0; i < roles.Length; ++i)
				{
					ORMCustomSerializedStandaloneRelationshipRole role = roles[i];
					file.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
				}
			}

			Guid keyId = writeContents ? link.Id : oppositeRolePlayer.Id;
			if (writeContents)
			{
				// UNDONE: NOW Write content of the oppositeRolePlayer for the standalone link
				ReadOnlyCollection<DomainRoleInfo> rolesPlayed = link.GetDomainClass().AllDomainRolesPlayed;
				bool writeChildren = aggregatingLink || rolesPlayed.Count != 0;

				if (writeChildren)
				{
					// UNDONE: Be smarter here. If none of the relationships for the played
					// roles are actually serialized, then we don't need this at all.
					file.WriteAttributeString("id", ToXml(keyId));
				}
				if (!aggregatingLink && !standaloneLink)
				{
					file.WriteAttributeString("ref", ToXml(oppositeRolePlayer.Id));
				}

				SerializeProperties(file, link, customElement, rolePlayedInfo, properties, hasCustomAttributes);

				if (writeChildren)
				{
					ORMCustomSerializedContainerElementInfo[] childElementInfo;
					bool groupRoles;

					childElementInfo = ((groupRoles = (0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.ChildElementInfo))) ? customElement.GetCustomSerializedChildElementInfo() : null);

					//write children
					SerializeChildElements(file, link, customElement, childElementInfo, rolesPlayed, 0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles), groupRoles, defaultPrefix);
				}
			}
			else if (!standaloneLink)
			{
				file.WriteAttributeString("ref", ToXml(keyId));
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
		/// <param name="containerInfo">The container information to write when customInfo is written.</param>
		/// <param name="defaultPrefix">The default prefix.</param>
		/// <param name="writeBeginElement">true to write the begin element tag.</param>
		/// <returns>true if the begin element tag was written.</returns>
		private bool SerializeChildElement(XmlWriter file, ModelElement childElement, DomainRoleInfo rolePlayedInfo, DomainRoleInfo oppositeRoleInfo, ORMCustomSerializedElementInfo customInfo, ORMCustomSerializedContainerElementInfo containerInfo, string defaultPrefix, bool writeBeginElement)
		{
			bool ret = false;
			DomainClassInfo lastChildClass = null;
			IORMCustomSerializedDomainModel parentModel = null;
			// If there class derived from the role player, then the class-level serialization settings may be
			// different than they were on the class specified on the role player, we need to check explicitly,
			// despite the earlier call to ShouldSerializeDomainRole
			bool checkSerializeClass = oppositeRoleInfo.RolePlayer.AllDescendants.Count != 0;
			Store store = myStore;
			bool isAggregate = rolePlayedInfo.IsEmbedding;
			bool oppositeIsAggregate = oppositeRoleInfo.IsEmbedding;
			IORMCustomSerializedElement testChildInfo;

			if (!isAggregate &&
				(!oppositeIsAggregate ||
				(oppositeIsAggregate &&
				null != (testChildInfo = childElement as IORMCustomSerializedElement) &&
				0 != (testChildInfo.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.EmbeddingLinkInfo) &&
				testChildInfo.GetCustomSerializedLinkInfo(oppositeRoleInfo, null).WriteStyle == ORMCustomSerializedElementWriteStyle.EmbeddingLinkElement))) //write link
			{
				ReadOnlyCollection<ElementLink> links = rolePlayedInfo.GetElementLinks<ElementLink>(childElement);
				int linksCount = links.Count;
				if (links.Count != 0)
				{
					bool checkSerializeLinkClass = rolePlayedInfo.DomainRelationship.AllDescendants.Count != 0;
					DomainRelationshipInfo lastLinkClass = null;
					IORMCustomSerializedDomainModel linkParentModel = null;
					for (int i = 0; i < linksCount; ++i)
					{
						// Verify that the link itself should be serialized
						ElementLink link = links[i];
						if (checkSerializeLinkClass)
						{
							DomainRelationshipInfo linkClass = link.GetDomainRelationship();
							if (linkClass != lastLinkClass)
							{
								lastLinkClass = linkClass;
								linkParentModel = GetParentModel(link);
							}
							if (linkParentModel != null && !linkParentModel.ShouldSerializeDomainClass(store, linkClass))
							{
								continue;
							}
						}

						// Verify that the opposite role player class should be serialized
						ModelElement oppositeRolePlayer = oppositeRoleInfo.GetRolePlayer(link);
						if (checkSerializeClass)
						{
							DomainClassInfo childClass = oppositeRolePlayer.GetDomainClass();
							if (childClass != lastChildClass)
							{
								lastChildClass = childClass;
								parentModel = GetParentModel(oppositeRolePlayer);
							}
							if (parentModel != null && !parentModel.ShouldSerializeDomainClass(store, childClass))
							{
								continue;
							}
						}

						if (ShouldSerializeElement(link) && ShouldSerializeElement(childElement))
						{
							if (writeBeginElement && !ret && customInfo != null)
							{
								if (!WriteCustomizedStartElement(file, customInfo, containerInfo, defaultPrefix, customInfo.CustomName))
								{
									return false;
								}
								ret = true;
							}
							SerializeLink(file, link, childElement, oppositeRolePlayer, rolePlayedInfo);
						}
					}
				}
			}
			else if (isAggregate) //write child
			{
				LinkedElementCollection<ModelElement> children = rolePlayedInfo.GetLinkedElements(childElement);
				int childCount = children.Count;

				if (childCount != 0)
				{
					string containerName = null;
					bool initializedContainerName = !writeBeginElement;

					for (int iChild = 0; iChild < childCount; ++iChild)
					{
						ModelElement child = children[iChild];

						if (checkSerializeClass)
						{
							DomainClassInfo childClass = child.GetDomainClass();
							if (childClass != lastChildClass)
							{
								lastChildClass = childClass;
								parentModel = GetParentModel(child);
							}
							if (parentModel != null && !parentModel.ShouldSerializeDomainClass(store, childClass))
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
								containerName = string.Concat(rolePlayedInfo.DomainRelationship.Name, ".", rolePlayedInfo.OppositeDomainRole.Name);
								initializedContainerName = true;
							}
							if (!SerializeElement(file, child, customInfo, defaultPrefix, null, ref containerName))
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
		private void SerializeChildElements(XmlWriter file, ModelElement element, IORMCustomSerializedElement customElement, ORMCustomSerializedContainerElementInfo[] childElementInfo, IList<DomainRoleInfo> rolesPlayed, bool sortRoles, bool groupRoles, string defaultPrefix)
		{
			int rolesPlayedCount = rolesPlayed.Count;
			IORMCustomSerializedDomainModel parentModel = GetParentModel(element);

			//sort played roles
			if (sortRoles && rolesPlayedCount != 0)
			{
				IComparer<DomainRoleInfo> comparer = customElement.CustomSerializedChildRoleComparer;
				if (comparer != null)
				{
					((List<DomainRoleInfo>)(rolesPlayed = new List<DomainRoleInfo>(rolesPlayed))).Sort(comparer);
				}
			}

			//write children
			if (groupRoles)
			{
				const byte PENDING = 0;
				const byte PROCESSED = 1;
				const byte CONTAINERWRITTEN = 2;
				byte[] written = new byte[rolesPlayedCount];
				int writeEndElementCount = 0;
				ORMCustomSerializedContainerElementInfo lastCustomChildInfo = null;

				for (int index0 = 0; index0 < rolesPlayedCount; ++index0)
				{
					if (written[index0] == PENDING)
					{
						DomainRoleInfo rolePlayedInfo = rolesPlayed[index0];
						if (!ShouldSerializeDomainRole(parentModel, rolePlayedInfo))
						{
							written[index0] = PROCESSED;
							continue;
						}
						DomainRoleInfo oppositeRoleInfo = rolePlayedInfo.OppositeDomainRole;
						int childIndex = FindGuid(childElementInfo, oppositeRoleInfo.Id);
						ORMCustomSerializedContainerElementInfo customChildInfo = (childIndex >= 0) ? childElementInfo[childIndex] : null;
						string defaultChildPrefix = (customChildInfo != null) ? defaultPrefix : null;
						ORMCustomSerializedContainerElementInfo outerCustomChildInfo = (customChildInfo != null) ? customChildInfo.OuterContainer : null;
						int outerIndex = (outerCustomChildInfo == null) ? -1 : ((IList<ORMCustomSerializedContainerElementInfo>)childElementInfo).IndexOf(outerCustomChildInfo);

						bool containerAlreadyOpen = false;
						if (writeEndElementCount != 0 && outerCustomChildInfo == null)
						{
							while (writeEndElementCount != 0)
							{
								if (customChildInfo != null && lastCustomChildInfo == customChildInfo)
								{
									containerAlreadyOpen = true;
									break;
								}
								WriteCustomizedEndElement(file, lastCustomChildInfo);
								lastCustomChildInfo = (lastCustomChildInfo != null) ? lastCustomChildInfo.OuterContainer : null;
								--writeEndElementCount;
							}
						}
						lastCustomChildInfo = customChildInfo;
						int writeEndElementCount0 = writeEndElementCount;

						written[index0] = PROCESSED;
						bool writeOuter = outerIndex != -1 && written[outerIndex] < CONTAINERWRITTEN;
						if (SerializeChildElement(file, element, rolePlayedInfo, oppositeRoleInfo, customChildInfo, writeOuter ? outerCustomChildInfo : null, defaultChildPrefix, !containerAlreadyOpen))
						{
							if (writeOuter)
							{
								written[outerIndex] = CONTAINERWRITTEN;
								++writeEndElementCount;
							}
							written[index0] = CONTAINERWRITTEN;
							++writeEndElementCount;
						}

						if (customChildInfo != null)
						{
							for (int index1 = index0 + 1; index1 < rolesPlayedCount; ++index1)
							{
								if (written[index1] == PENDING)
								{
									rolePlayedInfo = rolesPlayed[index1];
									if (!ShouldSerializeDomainRole(parentModel, rolePlayedInfo))
									{
										written[index1] = PROCESSED;
										continue;
									}
									oppositeRoleInfo = rolePlayedInfo.OppositeDomainRole;

									if (customChildInfo.ContainsGuid(oppositeRoleInfo.Id))
									{
										written[index1] = PROCESSED;
										writeOuter = outerIndex != -1 && written[outerIndex] < CONTAINERWRITTEN;
										if (SerializeChildElement(file, element, rolePlayedInfo, oppositeRoleInfo, customChildInfo, writeOuter ? outerCustomChildInfo : null, defaultChildPrefix, !containerAlreadyOpen && (writeEndElementCount == writeEndElementCount0)))
										{
											if (writeOuter)
											{
												written[outerIndex] = CONTAINERWRITTEN;
												++writeEndElementCount;
											}
											written[index1] = CONTAINERWRITTEN;
											++writeEndElementCount;
										}
									}
								}
							}
						}
					}
				}

				// Close out remaining blocks
				while (writeEndElementCount != 0)
				{
					WriteCustomizedEndElement(file, lastCustomChildInfo);
					lastCustomChildInfo = (lastCustomChildInfo != null) ? lastCustomChildInfo.OuterContainer : null;
					--writeEndElementCount;
				}
			}
			else
			{
				for (int index = 0; index < rolesPlayedCount; ++index)
				{
					DomainRoleInfo rolePlayedInfo = rolesPlayed[index];
					if (!ShouldSerializeDomainRole(parentModel, rolePlayedInfo))
					{
						continue;
					}
					if (SerializeChildElement(file, element, rolePlayedInfo, rolePlayedInfo.OppositeDomainRole, null, null, null, true))
					{
						WriteCustomizedEndElement(file, null);
					}
				}
			}

			return;
		}
		/// <summary>
		/// A callback delegate for SerializeElement to allow extra attributes to write at the appropriate point in the Xml/>
		/// </summary>
		private delegate void SerializeExtraAttributesCallback(XmlWriter file);
		/// <summary>
		/// Recursively serializes elements.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="containerCustomInfo">The container element's custom serialization information.</param>
		/// <param name="containerPrefix">The container element's prefix.</param>
		/// <param name="serializeExtraAttributes">A callback delegate used to write extra attributes</param>
		/// <param name="containerName">The container element's name.</param>
		/// <returns>false if the container element was not written.</returns>
		private bool SerializeElement(XmlWriter file, ModelElement element, ORMCustomSerializedElementInfo containerCustomInfo, string containerPrefix, SerializeExtraAttributesCallback serializeExtraAttributes, ref string containerName)
		{
			if (!ShouldSerializeElement(element))
			{
				return true;
			}
			ORMCustomSerializedElementSupportedOperations supportedOperations;
			ORMCustomSerializedContainerElementInfo[] childElementInfo = null;
			DomainClassInfo classInfo = element.GetDomainClass();
			ORMCustomSerializedElementInfo customInfo;
			IORMCustomSerializedElement customElement = element as IORMCustomSerializedElement;
			IList<DomainPropertyInfo> properties = classInfo.AllDomainProperties;
			ReadOnlyCollection<DomainRoleInfo> rolesPlayed = classInfo.AllDomainRolesPlayed;
			string defaultPrefix = DefaultElementPrefix(element);
			bool roleGrouping = false;
			bool isCustom = (customElement != null);

			//load custom information
			if (isCustom)
			{
				supportedOperations = customElement.SupportedCustomSerializedOperations;

				if (0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes))
				{
					SortProperties(customElement, null, ref properties);
				}
				if (roleGrouping = (0 != (supportedOperations & ORMCustomSerializedElementSupportedOperations.ChildElementInfo)))
				{
					childElementInfo = customElement.GetCustomSerializedChildElementInfo();
				}
				if ((supportedOperations & ORMCustomSerializedElementSupportedOperations.ElementInfo) != 0)
				{
					customInfo = customElement.CustomSerializedElementInfo;
					if (customInfo.WriteStyle == ORMCustomSerializedElementWriteStyle.NotWritten)
					{
						return true;
					}
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
				if (!WriteCustomizedStartElement(file, containerCustomInfo, null, containerPrefix, containerName))
				{
					return false;
				}
				containerName = null;
			}

			//write begin element tag
			if (!WriteCustomizedStartElement(file, customInfo, null, defaultPrefix, classInfo.Name))
			{
				return true;
			}
			file.WriteAttributeString("id", ToXml(element.Id));
			if (serializeExtraAttributes != null)
			{
				serializeExtraAttributes(file);
			}

			//write properties
			SerializeProperties
			(
				file,
				element,
				customElement,
				null,
				properties,
				(supportedOperations & ORMCustomSerializedElementSupportedOperations.PropertyInfo) != 0
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
		private void SerializeElement(XmlWriter file, ModelElement element)
		{
			string containerName = null;
			SerializeElement(file, element, null, null, null, ref containerName);
			return;
		}
		/// <summary>
		/// Recursivly serializes elements.
		/// </summary>
		/// <param name="file">The file to write to.</param>
		/// <param name="element">The element.</param>
		/// <param name="serializeExtraAttributes">A callback delegate used to write extra attributes</param>
		private void SerializeElement(XmlWriter file, ModelElement element, SerializeExtraAttributesCallback serializeExtraAttributes)
		{
			string containerName = null;
			SerializeElement(file, element, null, null, serializeExtraAttributes, ref containerName);
			return;
		}
		/// <summary>
		/// New XML Serialization
		/// </summary>
		public void Save(Stream stream)
		{
			XmlWriterSettings xmlSettings = new XmlWriterSettings();
			XmlWriter file;
			Store store = myStore;
			ICollection<DomainModel> values = store.DomainModels;

			xmlSettings.IndentChars = "\t";
			xmlSettings.Indent = true;

			file = XmlWriter.Create(stream, xmlSettings);
			file.WriteStartElement(RootXmlPrefix, RootXmlElementName, RootXmlNamespace);

			//serialize namespaces
			foreach (IORMCustomSerializedDomainModel ns in Utility.EnumerateDomainModels<IORMCustomSerializedDomainModel>(values))
			{
				string[,] namespaces = ns.GetCustomElementNamespaces();

				for (int index = 0, count = namespaces.GetLength(0); index < count; ++index)
				{
					//if (/*namespaces[index].Length==2 && */namespaces[index,0] != null && namespaces[index,1] != null)
					file.WriteAttributeString("xmlns", namespaces[index, 0], null, namespaces[index, 1]);
				}
			}

			//serialize all root elements
			IElementDirectory elementDir = myStore.ElementDirectory;
			DomainDataDirectory dataDir = myStore.DomainDataDirectory;
			foreach (IORMCustomSerializedDomainModel ns in Utility.EnumerateDomainModels<IORMCustomSerializedDomainModel>(values))
			{
				Guid[] metaClasses = ns.GetRootElementClasses();
				if (metaClasses != null)
				{
					for (int i = 0; i < metaClasses.Length; ++i)
					{
						DomainClassInfo classInfo = dataDir.GetDomainClass(metaClasses[i]);
						ReadOnlyCollection<ModelElement> elements = elementDir.FindElements(classInfo);
						int elementCount = elements.Count;
						for (int j = 0; j < elementCount; ++j)
						{
							ModelElement element = elements[j];
							System.Xml.Serialization.IXmlSerializable serializableElement = element as System.Xml.Serialization.IXmlSerializable;
							if (serializableElement != null)
							{
								serializableElement.WriteXml(file);
							}
							else
							{
								SerializeElement(file, element);
							}
						}
					}
				}
				ORMCustomSerializedRootRelationshipContainer[] rootContainers = ns.GetRootRelationshipContainers();
				if (rootContainers != null)
				{
					for (int i = 0; i < rootContainers.Length; ++i)
					{
						ORMCustomSerializedRootRelationshipContainer container = rootContainers[i];
						file.WriteStartElement(container.ContainerPrefix, container.ContainerName, container.ContainerNamespace);

						ORMCustomSerializedStandaloneRelationship[] relationships = container.GetRelationshipClasses();
						for (int j = 0; j < relationships.Length; ++j)
						{
							ORMCustomSerializedStandaloneRelationship relationship = relationships[j];
							DomainRelationshipInfo relationshipInfo = dataDir.GetDomainClass(relationship.DomainClassId) as DomainRelationshipInfo;
							ReadOnlyCollection<ModelElement> relationshipElements = elementDir.FindElements(relationshipInfo);
							int elementCount = relationshipElements.Count;
							for (int k = 0; k < elementCount; ++k)
							{
								ElementLink link = relationshipElements[k] as ElementLink;
								ORMCustomSerializedStandaloneRelationshipRole[] roles = relationship.GetRoles();
								if (relationship.IsPrimaryLinkElement)
								{
									SerializeElement(file, link, new SerializeExtraAttributesCallback(delegate(XmlWriter xmlFile)
									{
										for (int l = 0; l < roles.Length; ++l)
										{
											ORMCustomSerializedStandaloneRelationshipRole role = roles[l];
											xmlFile.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
										}
									}));
								}
								else
								{
									file.WriteStartElement(relationship.ElementPrefix, relationship.ElementName, relationship.ElementNamespace);
									for (int l = 0; l < roles.Length; ++l)
									{
										ORMCustomSerializedStandaloneRelationshipRole role = roles[l];
										file.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
									}
									file.WriteEndElement();
								}
							}
						}
						file.WriteEndElement();
					}
				}
			}

			file.WriteEndElement();
			file.Close();

			return;
		}
	}
	#endregion // Serialization Routines
	#region Deserialization Routines
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
			public ModelElement CreatePlaceholderElement(Store store, DomainClassInfo classInfo)
			{
				ModelElement retVal = FindElementOfType(classInfo);
				if (retVal == null)
				{
					retVal = RealizeClassInfo(store.ElementFactory, classInfo);
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
			/// <summary>
			/// Create a ModelElement of a type compatible with the specified classInfo
			/// </summary>
			/// <param name="elementFactory">The ElementFactory from the context store</param>
			/// <param name="classInfo">The meta information for the class to create</param>
			/// <returns>ModelElement</returns>
			private ModelElement RealizeClassInfo(ElementFactory elementFactory, DomainClassInfo classInfo)
			{
				Type implClass = classInfo.ImplementationClass;
				if (implClass.IsAbstract || implClass == typeof(ModelElement)) // The class factory won't create a raw model element
				{
					DomainClassInfo descendantInfo = FindCreatableClass(classInfo.LocalDescendants); // Try the cheap search first
					if (descendantInfo != null)
					{
						descendantInfo = FindCreatableClass(classInfo.AllDescendants);
					}
					Debug.Assert(descendantInfo != null); // Some descendant should always be creatable, otherwise there could not be a valid link
					classInfo = descendantInfo;
				}
				DomainRelationshipInfo relationshipInfo;
				ModelElement retVal = null;
				int domainRoleCount;
				ReadOnlyCollection<DomainRoleInfo> domainRoles;
				if (null != (relationshipInfo = classInfo as DomainRelationshipInfo) &&
					0 != (domainRoleCount = (domainRoles = relationshipInfo.DomainRoles).Count))
				{
					// If this is a link element, then we need to create it with dummy role players
					// to go along with the dummy element. The framework will allow the create of
					// an ElementLink using CreateElement, but there is no way to remove that element
					// unless it has the correct roles players attached to it.
					RoleAssignment[] assignments = new RoleAssignment[domainRoleCount];
					for (int i = 0; i < domainRoleCount; ++i)
					{
						DomainRoleInfo roleInfo = domainRoles[i];
						assignments[i] = new RoleAssignment(roleInfo.Id, RealizeClassInfo(elementFactory, roleInfo.RolePlayer));
					}
					retVal = elementFactory.CreateElementLink(relationshipInfo, assignments);
				}
				else
				{
					retVal = elementFactory.CreateElement(classInfo);
				}
				return retVal;
			}
			private static DomainClassInfo FindCreatableClass(IList<DomainClassInfo> classInfos)
			{
				DomainClassInfo retVal = null;
				int count = classInfos.Count;
				if (count != 0)
				{
					for (int i = 0; i < count; ++i)
					{
						DomainClassInfo testInfo = classInfos[i];
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
			private ModelElement FindElementOfType(DomainClassInfo classInfo)
			{
				DomainRelationshipInfo relInfo = classInfo as DomainRelationshipInfo;
				if (relInfo != null)
				{
					ElementLink link;
					if (mySingleElement != null)
					{
						if (null != (link = mySingleElement as ElementLink) &&
							link.GetDomainRelationship() == relInfo)
						{
							return mySingleElement;
						}
					}
					else if (myMultipleElements != null)
					{
						foreach (ModelElement mel in myMultipleElements)
						{
							if (null != (link = mel as ElementLink) &&
								link.GetDomainRelationship() == relInfo)
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
						if (mySingleElement.GetDomainClass() == classInfo)
						{
							return mySingleElement;
						}
					}
					else if (myMultipleElements != null)
					{
						foreach (ModelElement mel in myMultipleElements)
						{
							if (mel.GetDomainClass() == classInfo)
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
				ReadOnlyCollection<ElementLink> links = DomainRoleInfo.GetAllElementLinks(placeholder);
				int linkCount = links.Count;
				for (int i = linkCount - 1; i >= 0; --i) // Walk backwards, we're messing with the list contents
				{
					ElementLink link = links[i];
					ReadOnlyCollection<DomainRoleInfo> domainRoles = link.GetDomainRelationship().DomainRoles;
					int domainRoleCount = domainRoles.Count;
					for (int j = 0; j < domainRoleCount; ++j)
					{
						DomainRoleInfo roleInfo = domainRoles[j];
						if (roleInfo.GetRolePlayer(link) == placeholder)
						{
							roleInfo.SetRolePlayer(link, realElement);
						}
					}
				}
				RemoveDetachedPlaceholder(placeholder);
			}
			private static void RemoveDetachedPlaceholder(ModelElement placeholder)
			{
				// If the placeholder is a dummy link, then make sure that all of its
				// children are removed
				ElementLink linkPlaceholder = placeholder as ElementLink;
				if (linkPlaceholder != null)
				{
					RemoveDetachedLinkPlaceholder(linkPlaceholder);
				}
				else
				{
					placeholder.Delete();
				}
			}
			private static void RemoveDetachedLinkPlaceholder(ElementLink linkPlaceholder)
			{
				DomainRelationshipInfo relationshipInfo = linkPlaceholder.GetDomainRelationship();
				ReadOnlyCollection<DomainRoleInfo> domainRoles = relationshipInfo.DomainRoles;
				int domainRoleCount = domainRoles.Count;
				if (domainRoleCount != 0)
				{
					// Cache the role players up front so we can recursively delete them
					// Note that deleting a role player before deleting the link is likely to
					// delete the link as well because most role players are non-optional.
					// Delete propagation may also apply to the opposite role player, which
					// we don't want in case there are elements attached to that link that
					// do not delete automatically. To handle this case, we cache the role
					// players, delete the link without propagating deletion to the
					// roles, then delete the role players explicitly.
					ModelElement[] rolePlayers = new ModelElement[domainRoleCount];
					for (int i = 0; i < domainRoleCount; ++i)
					{
						rolePlayers[i] = domainRoles[i].GetRolePlayer(linkPlaceholder);
					}

					// Remove the link without removing the role players
					Guid[] domainRoleGuids = new Guid[domainRoles.Count];
					for (int i = 0; i < domainRoleGuids.Length; ++i)
					{
						domainRoleGuids[i] = domainRoles[i].Id;
					}
					linkPlaceholder.Delete(domainRoleGuids);

					// Remove the role players
					for (int i = 0; i < domainRoleCount; ++i)
					{
						Debug.Assert(!rolePlayers[i].IsDeleted);
						RemoveDetachedPlaceholder(rolePlayers[i]);
					}
				}
				else
				{
					linkPlaceholder.Delete();
				}

			}
		}
		private Dictionary<string, Guid> myCustomIdToGuidMap;
		private INotifyElementAdded myNotifyAdded;
		private Dictionary<Guid, PlaceholderElement> myPlaceholderElementMap;
		private Dictionary<string, IORMCustomSerializedDomainModel> myXmlNamespaceToModelMap;

		#region Member Variables
		/// <summary>
		/// Current store object. Set in constructor
		/// </summary>
		private readonly Store myStore;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a serializer on the given store
		/// </summary>
		/// <param name="store">Store instance</param>
		public ORMSerializer(Store store)
		{
			myStore = store;
		}
		#endregion // Constructor
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		/// <param name="fixupManager">Class used to perfom fixup operations
		/// after the load is complete.</param>
		public void Load(Stream stream, DeserializationFixupManager fixupManager)
		{
			myNotifyAdded = fixupManager as INotifyElementAdded;
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSchemaSet schemas = settings.Schemas;
			Type schemaResourcePathType = GetType();
			schemas.Add(RootXmlNamespace, new XmlTextReader(schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, "ORM2Root.xsd")));

			// Extract namespace and schema information from the different meta models
			ICollection<DomainModel> domainModels = myStore.DomainModels;
			Dictionary<string, IORMCustomSerializedDomainModel> namespaceToModelMap = new Dictionary<string, IORMCustomSerializedDomainModel>();
			foreach (IORMCustomSerializedDomainModel customSerializedDomainModel in Utility.EnumerateDomainModels<IORMCustomSerializedDomainModel>(domainModels))
			{
				string[,] namespaces = customSerializedDomainModel.GetCustomElementNamespaces();
				int namespaceCount = namespaces.GetLength(0);
				for (int i = 0; i < namespaceCount; ++i)
				{
					string namespaceURI = namespaces[i, 1];
					namespaceToModelMap.Add(namespaceURI, customSerializedDomainModel);
					string schemaFile = namespaces[i, 2];
					if (schemaFile != null && schemaFile.Length != 0)
					{
						schemaResourcePathType = customSerializedDomainModel.GetType();
						schemas.Add(namespaceURI, new XmlTextReader(schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, schemaFile)));
					}
				}
			}
			myXmlNamespaceToModelMap = namespaceToModelMap;
			NameTable nameTable = new NameTable();
			settings.NameTable = nameTable;


#if DEBUG
			// Skip validation when the shift key is down in debug mode
			if ((System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Shift) == 0)
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
			Store store = myStore;
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			using (Transaction t = store.TransactionManager.BeginTransaction())
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
											IORMCustomSerializedDomainModel metaModel;
											if (namespaceToModelMap.TryGetValue(reader.NamespaceURI, out metaModel))
											{
												string id = reader.GetAttribute("id");
												if (id != null)
												{
													Guid classGuid = metaModel.MapRootElement(reader.NamespaceURI, reader.LocalName);
													if (!classGuid.Equals(Guid.Empty))
													{
														processedRootElement = true;
														ModelElement rootElement = CreateElement(id, null, classGuid);
														System.Xml.Serialization.IXmlSerializable serializableRootElement = rootElement as System.Xml.Serialization.IXmlSerializable;
														if (serializableRootElement != null)
														{
															using (XmlReader subtreeReader = reader.ReadSubtree())
															{
																serializableRootElement.ReadXml(subtreeReader);
															}
														}
														else
														{
															ProcessClassElement(reader, metaModel, rootElement, null, null);
														}
													}
												}
												else if (!reader.IsEmptyElement)
												{
													ORMCustomSerializedRootRelationshipContainer[] containers = metaModel.GetRootRelationshipContainers();
													if (containers != null)
													{
														ORMCustomSerializedRootRelationshipContainer? container = ORMCustomSerializedRootRelationshipContainer.Find(containers, reader.LocalName, reader.NamespaceURI);
														if (container.HasValue)
														{
															processedRootElement = true;
															// this is a relationsip container element
															while (reader.Read())
															{
																if (reader.NodeType == XmlNodeType.Element)
																{
																	bool processedLinkElement = false;
																	string linkNamespace = reader.NamespaceURI;
																	string linkName = reader.LocalName;

																	ORMCustomSerializedStandaloneRelationship[] relationships = container.Value.GetRelationshipClasses();
																	DomainRelationshipInfo relationshipInfo = null;
																	ORMCustomSerializedStandaloneRelationship? relationship = ORMCustomSerializedStandaloneRelationship.Find(relationships, linkName, linkNamespace);
																	if (relationship.HasValue)
																	{
																		relationshipInfo = dataDirectory.FindDomainRelationship(relationship.Value.DomainClassId);
																	}
																	else
																	{
																		Guid domainClassId = metaModel.MapClassName(linkNamespace, linkName);
																		if (!domainClassId.Equals(Guid.Empty))
																		{
																			relationshipInfo = dataDirectory.FindDomainRelationship(domainClassId);
																		}
																		if (relationshipInfo == null)
																		{
																			relationshipInfo = dataDirectory.FindDomainRelationship(string.Concat(metaModel.GetType().Namespace, ".", linkName));
																		}
																	}
																	if (relationshipInfo != null &&
																		(relationship = ORMCustomSerializedStandaloneRelationship.Find(relationships, relationshipInfo.Id)).HasValue)
																	{
																		ORMCustomSerializedStandaloneRelationshipRole[] roles = relationship.Value.GetRoles();
																		if (roles.Length == 2)
																		{
																			string[] rolePlayerIds = new string[2];
																			DomainClassInfo[] rolePlayerDomainClasses = new DomainClassInfo[2];
																			DomainRoleInfo oppositeRoleInfo = null;
																			for (int i = 0; i < 2; ++i)
																			{
																				ORMCustomSerializedStandaloneRelationshipRole currentRole = roles[i];
																				string rolePlayerId;
																				DomainRoleInfo domainRoleInfo;
																				DomainClassInfo domainClassInfo;
																				if (null == (rolePlayerId = reader.GetAttribute(currentRole.AttributeName)) ||
																					null == (domainRoleInfo = dataDirectory.FindDomainRole(roles[i].DomainRoleId)) ||
																					null == (domainClassInfo = domainRoleInfo.RolePlayer))
																				{
																					break;
																				}
																				rolePlayerIds[i] = rolePlayerId;
																				rolePlayerDomainClasses[i] = domainClassInfo;
																				if (i == 1)
																				{
																					oppositeRoleInfo = domainRoleInfo;
																				}
																			}
																			if (oppositeRoleInfo != null)
																			{
																				bool isNewElementDummy;
																				ElementLink newElementLink = CreateElementLink(
																						reader.GetAttribute("id"),
																						CreateElement(
																							rolePlayerIds[0],
																							rolePlayerDomainClasses[0],
																							Guid.Empty,
																							rolePlayerDomainClasses[0].AllDescendants.Count == 0,
																							out isNewElementDummy),
																						CreateElement(
																							rolePlayerIds[1],
																							rolePlayerDomainClasses[1],
																							Guid.Empty,
																							rolePlayerDomainClasses[1].AllDescendants.Count == 0,
																							out isNewElementDummy),
																						oppositeRoleInfo,
																						relationshipInfo);
																				if (relationship.Value.IsPrimaryLinkElement)
																				{
																					processedLinkElement = true;
																					int blockedAttributeCount = 0;
																					ProcessClassElement(
																						reader,
																						metaModel,
																						newElementLink,
																						null,
																						delegate(string attributeNamespace, string attributeName)
																						{
																							if (blockedAttributeCount < 2)
																							{
																								if (string.IsNullOrEmpty(attributeNamespace))
																								{
																									if (attributeName == roles[0].AttributeName ||
																										attributeName == roles[1].AttributeName)
																									{
																										++blockedAttributeCount;
																										return false;
																									}
																								}
																							}
																							return true;
																						});
																				}

																			}
																		}
																	}

																	if (!processedLinkElement)
																	{
																		PassEndElement(reader);
																	}
																}
																else if (reader.NodeType == XmlNodeType.EndElement)
																{
																	break;
																}
															}
														}
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
				if (fixupManager != null)
				{
					fixupManager.DeserializationComplete();
				}
				t.Commit();
			}
			foreach (DomainModel loadedModel in domainModels)
			{
				IDomainModelEnablesRulesAfterDeserialization enableRules = loadedModel as IDomainModelEnablesRulesAfterDeserialization;
				if (enableRules != null)
				{
					enableRules.EnableRulesAfterDeserialization(myStore);
				}
			}
		}
		private delegate ElementLink CreateAggregatingLink(string idValue);
		/// <summary>
		/// Provide a callback filter to skip attribute processing
		/// </summary>
		/// <param name="attributeNamespace">The xml namespace of attribute</param>
		/// <param name="attributeName">The name of the attribute</param>
		/// <returns><see langword="false"/> to block attribute processing</returns>
		private delegate bool ShouldProcessAttribute(string attributeNamespace, string attributeName);
		/// <summary>
		/// Process a newly created element. The element will have an
		/// Id set only. The id and ref properties should be ignored.
		/// </summary>
		/// <param name="reader">Reader set to the root node</param>
		/// <param name="customModel">The custom serialized meta model</param>
		/// <param name="element">Newly created element</param>
		/// <param name="createAggregatingLinkCallback">A callback to pre-create the aggregating link before the aggregated element has finished processing</param>
		/// <param name="shouldProcessAttributeCallback">A callback to determine which attribute values should be processed</param>
		private void ProcessClassElement(XmlReader reader, IORMCustomSerializedDomainModel customModel, ModelElement element, CreateAggregatingLink createAggregatingLinkCallback, ShouldProcessAttribute shouldProcessAttributeCallback)
		{
			IORMCustomSerializedElement customElement = element as IORMCustomSerializedElement;
			DomainDataDirectory dataDir = myStore.DomainDataDirectory;
			#region Property processing
			// Process all properties first
			if (reader.MoveToFirstAttribute())
			{
				do
				{
					string attributeName = reader.LocalName;
					string namespaceName = reader.NamespaceURI;
					//derived objects are prefixed by an underscore and do not need to be read in
					if (!(namespaceName.Length == 0 && (attributeName == "id" || attributeName == "ref" || attributeName[0] == '_')) &&
						(shouldProcessAttributeCallback == null || shouldProcessAttributeCallback(namespaceName, attributeName)))
					{
						Guid attributeGuid = new Guid();
						DomainPropertyInfo attributeInfo = null;
						if (customElement != null)
						{
							attributeGuid = customElement.MapAttribute(namespaceName, attributeName);
							if (!attributeGuid.Equals(Guid.Empty))
							{
								attributeInfo = dataDir.FindDomainProperty(attributeGuid);
							}
						}
						if (attributeInfo == null && namespaceName.Length == 0)
						{
							attributeInfo = element.GetDomainClass().FindDomainProperty(attributeName, true);
						}
						if (attributeInfo != null)
						{
							SetPropertyValue(element, attributeInfo, reader.Value);
						}
					}
				} while (reader.MoveToNextAttribute());
			}
			reader.MoveToElement();
			#endregion // Property processing
			ProcessChildElements(reader, customModel, element, customElement, createAggregatingLinkCallback);
		}
		/// <summary>
		/// Retrieve DomainRoleInfo for the provided model, relationship name, and role name
		/// </summary>
		/// <param name="dataDir">The <see cref="DomainDataDirectory"/> to search</param>
		/// <param name="modelNamespace">The namespace of the model</param>
		/// <param name="relationshipName">The name of the relationship</param>
		/// <param name="roleName">The name of the role</param>
		/// <returns><see cref="DomainRoleInfo"/>, or null if not found</returns>
		private static DomainRoleInfo FindDomainRole(DomainDataDirectory dataDir, string modelNamespace, string relationshipName, string roleName)
		{
			DomainRelationshipInfo relationshipInfo = dataDir.FindDomainRelationship(modelNamespace + "." + relationshipName);
			return (relationshipInfo != null) ? relationshipInfo.FindDomainRole(roleName) : null;
		}
		private void ProcessChildElements(XmlReader reader, IORMCustomSerializedDomainModel customModel, ModelElement element, IORMCustomSerializedElement customElement, CreateAggregatingLink createAggregatingLinkCallback)
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			DomainDataDirectory dataDir = myStore.DomainDataDirectory;
			string elementName;
			string namespaceName;
			string containerName = null;
			string containerNamespace = null;
			string outerContainerName = null;
			string outerContainerNamespace = null;
			bool testForAggregatingLink = createAggregatingLinkCallback != null && customElement != null && 0 != (customElement.SupportedCustomSerializedOperations & ORMCustomSerializedElementSupportedOperations.EmbeddingLinkInfo);
			IORMCustomSerializedDomainModel containerRestoreCustomModel = null;
			DomainRoleInfo containerOppositeDomainRole = null;
			IORMCustomSerializedDomainModel outerContainerRestoreCustomModel = null;
			DomainRoleInfo outerContainerOppositeDomainRole = null;
			while (reader.Read())
			{
				XmlNodeType outerNodeType = reader.NodeType;
				if (outerNodeType == XmlNodeType.Element)
				{
					elementName = reader.LocalName;
					namespaceName = reader.NamespaceURI;
					string idValue = reader.GetAttribute("id");
					string refValue = reader.GetAttribute("ref");
					bool aggregatedClass = (object)idValue != null && (object)refValue == null;
					DomainRoleInfo oppositeDomainRole = null;
					DomainClassInfo oppositeDomainClass = null;
					DomainRelationshipInfo explicitRelationshipType = null;
					bool oppositeDomainClassFullyDeterministic = false;
					bool resolveOppositeDomainClass = false;
					bool allowDuplicates = false;
					IList<Guid> oppositeDomainRoleIds = null;
					IORMCustomSerializedDomainModel restoreCustomModel = null;
					bool nodeProcessed = false;
					ORMCustomSerializedElementMatch aggregatingLinkMatch;
					ORMCustomSerializedStandaloneRelationship? standaloneRelationship = null;
					DomainRoleInfo testAggregatingRole;
					if (aggregatedClass &&
						testForAggregatingLink &&
						(aggregatingLinkMatch = customElement.MapChildElement(namespaceName, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName)).MatchStyle == ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRole &&
						null != (testAggregatingRole = dataDir.FindDomainRole(aggregatingLinkMatch.SingleOppositeDomainRoleId)) &&
						testAggregatingRole.IsEmbedding)
					{
						testForAggregatingLink = false;
						ElementLink aggregatingLink = createAggregatingLinkCallback(idValue);
						if (aggregatingLink != null)
						{
							ProcessClassElement(reader, customModel, aggregatingLink, null, null);
						}
						nodeProcessed = true;
					}
					else if (aggregatedClass && containerName == null)
					{
						// All we have is the class name, go look for an appropriate aggregate
						if (customModel != null)
						{
							Guid domainClassId = customModel.MapClassName(namespaceName, elementName);
							if (!domainClassId.Equals(Guid.Empty))
							{
								oppositeDomainClass = dataDir.FindDomainClass(domainClassId);
							}
						}
						if (oppositeDomainClass == null)
						{
							Type namespaceType = (customModel != null) ? customModel.GetType() : element.GetType();
							oppositeDomainClass = dataDir.FindDomainClass(string.Concat(namespaceType.Namespace, ".", elementName));
						}
						if (oppositeDomainClass != null)
						{
							// Find the aggregating role that maps to this class
							ReadOnlyCollection<DomainRoleInfo> aggregatingRoles = element.GetDomainClass().AllDomainRolesPlayed;
							int rolesCount = aggregatingRoles.Count;
							for (int i = 0; i < rolesCount; ++i)
							{
								DomainRoleInfo aggregatingRole = aggregatingRoles[i];
								if (aggregatingRole.IsEmbedding)
								{
									DomainRoleInfo testRole = aggregatingRole.OppositeDomainRole;
									if (testRole.RolePlayer == oppositeDomainClass)
									{
										oppositeDomainRole = testRole;
										break;
									}
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
								match = customElement.MapChildElement(null, null, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
							}
							else
							{
								match = customElement.MapChildElement(namespaceName, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
							}
							allowDuplicates = match.AllowDuplicates;
							standaloneRelationship = match.StandaloneRelationship;
							switch (match.MatchStyle)
							{
								case ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRole:
									oppositeDomainRole = dataDir.FindDomainRole(match.SingleOppositeDomainRoleId);
									resolveOppositeDomainClass = true;
									break;
								case ORMCustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
									explicitRelationshipType = dataDir.FindDomainRelationship(match.ExplicitRelationshipGuid);
									oppositeDomainRole = dataDir.FindDomainRole(match.SingleOppositeDomainRoleId);
									resolveOppositeDomainClass = true;
									break;
								case ORMCustomSerializedElementMatchStyle.MultipleOppositeDomainRoles:
									oppositeDomainRoleIds = match.OppositeDomainRoleIdCollection;
									break;
								case ORMCustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
									explicitRelationshipType = dataDir.FindDomainRelationship(match.ExplicitRelationshipGuid);
									oppositeDomainRoleIds = match.OppositeDomainRoleIdCollection;
									break;
								case ORMCustomSerializedElementMatchStyle.Property:
									{
										DomainPropertyInfo attributeInfo = dataDir.FindDomainProperty(match.DomainPropertyId);
										if (match.DoubleTagName == null)
										{
											// Reader the value off directly
											SetPropertyValue(element, attributeInfo, reader.ReadString());
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
														SetPropertyValue(element, attributeInfo, reader.ReadString());
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
								if (containerOppositeDomainRole != null)
								{
									oppositeDomainRole = containerOppositeDomainRole;
									resolveOppositeDomainClass = true;
								}
							}
							else if (refValue != null)
							{
								IORMCustomSerializedDomainModel childModel;
								int separatorIndex;
								if ((separatorIndex = elementName.IndexOf('.')) > 0 && myXmlNamespaceToModelMap.TryGetValue(namespaceName, out childModel))
								{
									if (childModel != customModel && customModel != null)
									{
										restoreCustomModel = customModel;
										customModel = childModel;
									}
									string relationshipName = elementName.Substring(0, separatorIndex);
									string roleName = elementName.Substring(separatorIndex + 1);
									DomainRoleInfo domainRole = FindDomainRole(dataDir, childModel.GetType().Namespace, relationshipName, roleName);
									// Fallback on the two standard meta models
									if (domainRole == null)
									{
										domainRole = FindDomainRole(dataDir, typeof(ModelElement).Namespace, relationshipName, roleName);
										if (domainRole == null)
										{
											domainRole = FindDomainRole(dataDir, typeof(Diagram).Namespace, relationshipName, roleName);
										}
									}
									if (domainRole != null)
									{
										oppositeDomainRole = domainRole;
										resolveOppositeDomainClass = true;
									}
								}
							}
							else if (containerName == null)
							{
								// The tag name should have the format Rel.Role. Assume the relationship
								// is in the same namespace as the model associated with the xml namespace.
								// Models can nest elements inside base models, so we can't assume the node
								// is in the same code namespace as the parent. Also, if the implementation
								// class of the parent element has been upgraded (with DomainClassInfo.UpgradeImplementationClass)
								// then the ImplemtationClass of the parent node will be in the wrong namespace.
								// The model elements themselves are more stable, use them.
								IORMCustomSerializedDomainModel childModel;
								int separatorIndex;
								if ((separatorIndex = elementName.IndexOf('.')) > 0 && myXmlNamespaceToModelMap.TryGetValue(namespaceName, out childModel))
								{
									string relationshipName = elementName.Substring(0, separatorIndex);
									string roleName = elementName.Substring(separatorIndex + 1);
									DomainRoleInfo domainRole = FindDomainRole(dataDir, childModel.GetType().Namespace, relationshipName, roleName);
									// Fallback on the two standard meta models
									if (domainRole == null)
									{
										domainRole = FindDomainRole(dataDir, typeof(ModelElement).Namespace, relationshipName, roleName);
										if (domainRole == null)
										{
											domainRole = FindDomainRole(dataDir, typeof(Diagram).Namespace, relationshipName, roleName);
										}
									}
									if (domainRole != null)
									{
										containerOppositeDomainRole = domainRole;
										containerRestoreCustomModel = customModel;
										customModel = childModel;
									}
								}
								// If this is an unrecognized node without an id or ref then push
								// the container node (we only allow container depth of 2, handled by
								// outerContainerName blocks) and continue to loop.
								if (!reader.IsEmptyElement)
								{
									containerName = elementName;
									containerNamespace = namespaceName;
								}
								nodeProcessed = true;
							}
							else if (outerContainerName == null)
							{
								// Push a container level. We handle up to two levels.
								// Note that nested containers will not happen without explicit
								// custom serialization, so we don't need to look for the rel.role
								// element pattern here.
								if (!reader.IsEmptyElement)
								{
									outerContainerName = containerName;
									outerContainerNamespace = containerNamespace;
									outerContainerOppositeDomainRole = containerOppositeDomainRole;
									outerContainerRestoreCustomModel = containerRestoreCustomModel;
									containerName = elementName;
									containerNamespace = namespaceName;
									containerOppositeDomainRole = null;
									containerRestoreCustomModel = null;
								}
								nodeProcessed = true;
							}
						}
					}
					if (!nodeProcessed)
					{
						if (oppositeDomainRole != null)
						{
							if (resolveOppositeDomainClass)
							{
								oppositeDomainClass = oppositeDomainRole.RolePlayer;
								// If the opposite role player does not have any derived class in
								// the model then we know what type of element to create. Otherwise,
								// we need to create the element as a pending element if it doesn't exist
								// already.
								oppositeDomainClassFullyDeterministic = oppositeDomainClass.AllDescendants.Count == 0;
								if (aggregatedClass && !oppositeDomainClassFullyDeterministic)
								{
									DomainClassInfo testMetaClass = null;
									IORMCustomSerializedDomainModel elementModel = myXmlNamespaceToModelMap[namespaceName];
									if (elementModel == null)
									{
										elementModel = customModel;
									}
									if (elementModel != null)
									{
										Guid mappedGuid = elementModel.MapClassName(namespaceName, elementName);
										if (!mappedGuid.Equals(Guid.Empty))
										{
											testMetaClass = dataDir.FindDomainClass(mappedGuid);
										}
									}
									if (testMetaClass == null)
									{
										Type namespaceType = (elementModel != null) ? elementModel.GetType() : element.GetType();
										testMetaClass = dataDir.FindDomainClass(string.Concat(namespaceType.Namespace, ".", elementName));
									}
									oppositeDomainClass = testMetaClass;
									oppositeDomainClassFullyDeterministic = true;
								}
							}
						}
						else if (oppositeDomainRoleIds != null)
						{
							// In this case we have multiple opposite meta role guids, so we
							// always have to rely on the aggregated element to find the data
							Debug.Assert(aggregatedClass);
							if (customModel != null)
							{
								Guid mappedGuid = customModel.MapClassName(namespaceName, elementName);
								if (!mappedGuid.Equals(Guid.Empty))
								{
									oppositeDomainClass = dataDir.FindDomainClass(mappedGuid);
								}
							}
							if (oppositeDomainClass == null)
							{
								Type namespaceType = (customModel != null) ? customModel.GetType() : element.GetType();
								oppositeDomainClass = dataDir.FindDomainClass(string.Concat(namespaceType.Namespace, ".", elementName));
							}
							if (oppositeDomainClass != null)
							{
								oppositeDomainClassFullyDeterministic = true;
								int roleGuidCount = oppositeDomainRoleIds.Count;
								for (int i = 0; i < roleGuidCount; ++i)
								{
									DomainRoleInfo testRoleInfo = dataDir.FindDomainRole(oppositeDomainRoleIds[i]);
									if (oppositeDomainClass.IsDerivedFrom(testRoleInfo.RolePlayer.Id))
									{
										oppositeDomainRole = testRoleInfo;
#if DEBUG
										for (int j = i + 1; j < roleGuidCount; ++j)
										{
											testRoleInfo = dataDir.FindDomainRole(oppositeDomainRoleIds[j]);
											Debug.Assert(testRoleInfo == null || !oppositeDomainClass.IsDerivedFrom(testRoleInfo.RolePlayer.Id), "Custom serialization data does not provide a unique deserialization map for a combined element.");
										}
#endif // DEBUG
										break;
									}
								}
							}
						}
						if (oppositeDomainClass != null)
						{
							Debug.Assert(oppositeDomainRole != null);
							// Create a new element and make sure the relationship
							// to this element does not already exist. This obviously requires one
							// relationship of each type between any two objects, which is a reasonable assumption
							// for a well-formed model.
							bool isNewElement = false;
							string elementId = aggregatedClass ? idValue : refValue;
							ModelElement oppositeElement = null;
							bool createLink = true;
							if (!standaloneRelationship.HasValue)
							{
								oppositeElement = CreateElement(elementId, oppositeDomainClass, Guid.Empty, !oppositeDomainClassFullyDeterministic, out isNewElement);
							}
							else
							{
								ORMCustomSerializedStandaloneRelationship relationship = standaloneRelationship.Value;
								DomainRelationshipInfo standaloneRelationshipInfo = dataDir.FindDomainRelationship(standaloneRelationship.Value.DomainClassId);
								ORMCustomSerializedStandaloneRelationshipRole[] roles = relationship.GetRoles();
								if (roles.Length == 2)
								{
									string[] rolePlayerIds = new string[2];
									DomainClassInfo[] rolePlayerDomainClasses = new DomainClassInfo[2];
									DomainRoleInfo oppositeRoleInfo = null;
									for (int i = 0; i < 2; ++i)
									{
										ORMCustomSerializedStandaloneRelationshipRole currentRole = roles[i];
										string rolePlayerId;
										DomainRoleInfo domainRoleInfo;
										DomainClassInfo domainClassInfo;
										if (null == (rolePlayerId = reader.GetAttribute(currentRole.AttributeName)) ||
											null == (domainRoleInfo = dataDir.FindDomainRole(roles[i].DomainRoleId)) ||
											null == (domainClassInfo = domainRoleInfo.RolePlayer))
										{
											break;
										}
										rolePlayerIds[i] = rolePlayerId;
										rolePlayerDomainClasses[i] = domainClassInfo;
										if (i == 1)
										{
											oppositeRoleInfo = domainRoleInfo;
										}
									}
									if (oppositeRoleInfo != null)
									{
										isNewElement = true;
										bool isNewElementDummy;
										oppositeElement = CreateElementLink(
												idValue,
												CreateElement(
													rolePlayerIds[0],
													rolePlayerDomainClasses[0],
													Guid.Empty,
													rolePlayerDomainClasses[0].AllDescendants.Count == 0,
													out isNewElementDummy),
												CreateElement(
													rolePlayerIds[1],
													rolePlayerDomainClasses[1],
													Guid.Empty,
													rolePlayerDomainClasses[1].AllDescendants.Count == 0,
													out isNewElementDummy),
												oppositeRoleInfo,
												standaloneRelationshipInfo);
										if (relationship.IsPrimaryLinkElement)
										{
											int blockedAttributeCount = 0;
											ProcessClassElement(
												reader,
												customModel,
												oppositeElement,
												delegate(string aggregateIdValue)
												{
													ElementLink retVal = null;
													if (createLink)
													{
														createLink = false;
														retVal = CreateElementLink(aggregateIdValue, element, oppositeElement, oppositeDomainRole, explicitRelationshipType);
													}
													return retVal;
												},
												delegate(string attributeNamespace, string attributeName)
												{
													if (blockedAttributeCount < 2)
													{
														if (string.IsNullOrEmpty(attributeNamespace))
														{
															if (attributeName == roles[0].AttributeName ||
																attributeName == roles[1].AttributeName)
															{
																++blockedAttributeCount;
																return false;
															}
														}
													}
													return true;
												});
										}
									}
								}
								// If this is set, then we already used it to create the standalone line
								// Any embedding relationship id would have been set with the EmbeddingLinkElement callback
								idValue = null;
								aggregatedClass = false;
							}
							if (oppositeElement != null)
							{
								if (aggregatedClass)
								{
									ProcessClassElement(
										reader,
										customModel,
										oppositeElement,
										delegate(string aggregateIdValue)
										{
											ElementLink retVal = null;
											if (createLink)
											{
												createLink = false;
												retVal = CreateElementLink(aggregateIdValue, element, oppositeElement, oppositeDomainRole, explicitRelationshipType);
											}
											return retVal;
										},
										null);
								}
								if (!allowDuplicates && !isNewElement)
								{
									LinkedElementCollection<ModelElement> oppositeRolePlayers = oppositeDomainRole.GetLinkedElements(oppositeElement);
									int oppositeCount = oppositeRolePlayers.Count;
									for (int i = 0; i < oppositeCount; ++i)
									{
										if (element == oppositeRolePlayers[i])
										{
											createLink = false;
											break;
										}
									}
								}
								if (createLink)
								{
									ElementLink newLink = CreateElementLink(aggregatedClass ? null : idValue, element, oppositeElement, oppositeDomainRole, explicitRelationshipType);
									if (!aggregatedClass && idValue != null)
									{
										ProcessClassElement(reader, customModel, newLink, null, null);
									}
								}
							}
							else
							{
								PassEndElement(reader);
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
					allowDuplicates = false;
				}
				else if (outerNodeType == XmlNodeType.EndElement)
				{
					if (containerName != null)
					{
						// Pop the container node
						if (outerContainerName != null)
						{
							containerName = outerContainerName;
							outerContainerName = null;
							containerNamespace = outerContainerNamespace;
							outerContainerNamespace = null;
							containerOppositeDomainRole = outerContainerOppositeDomainRole;
							outerContainerOppositeDomainRole = null;
							containerRestoreCustomModel = outerContainerRestoreCustomModel;
							outerContainerRestoreCustomModel = null;
						}
						else
						{
							containerName = null;
							containerNamespace = null;
							containerOppositeDomainRole = null;
							if (containerRestoreCustomModel != null)
							{
								customModel = containerRestoreCustomModel;
								containerRestoreCustomModel = null;
							}
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
		/// Set the value of the specified property on the model element
		/// </summary>
		/// <param name="element">The element to modify</param>
		/// <param name="domainPropertyInfo">The meta property to set</param>
		/// <param name="stringValue">The new value of the property</param>
		private static void SetPropertyValue(ModelElement element, DomainPropertyInfo domainPropertyInfo, string stringValue)
		{
			PropertyInfo propertyInfo = domainPropertyInfo.PropertyInfo;
			Type propertyType = domainPropertyInfo.PropertyType;
			object objectValue = null;
			if (!propertyType.IsEnum)
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
							break;
						}
				}
			}
			if (objectValue == null)
			{
				DomainTypeDescriptor.TypeDescriptorContext context = DomainTypeDescriptor.CreateTypeDescriptorContext(element, domainPropertyInfo);
				TypeConverter converter = context.PropertyDescriptor.Converter;
				Type stringType = typeof(string);
				if (converter.CanConvertFrom(context, stringType))
				{
					objectValue = converter.ConvertFromInvariantString(context, stringValue);
				}
				else if (propertyType.IsAssignableFrom(stringType))
				{
					objectValue = stringValue;
				}
				else
				{
					Debug.Fail("Could not deserialize property!");
				}
			}
			if (objectValue != null)
			{
				domainPropertyInfo.SetValue(element, objectValue);
			}
		}
		/// <summary>
		/// Create an element link after verifying that the link needs to be created
		/// </summary>
		/// <param name="idValue">The value of the id for the link, or null</param>
		/// <param name="rolePlayer">The near role player</param>
		/// <param name="oppositeRolePlayer">The opposite role player</param>
		/// <param name="oppositeMetaRoleInfo">The opposite meta role</param>
		/// <param name="explicitMetaRelationshipInfo">The relationship type to create.
		/// Derived from oppositeMetaRoleInfo if not specified.</param>
		/// <returns>The newly created element link</returns>
		private ElementLink CreateElementLink(string idValue, ModelElement rolePlayer, ModelElement oppositeRolePlayer, DomainRoleInfo oppositeMetaRoleInfo, DomainRelationshipInfo explicitMetaRelationshipInfo)
		{
			// Create an element link. There is no attempt here to determine if the link already
			// exists in the store;
			Guid id = (idValue == null) ? Guid.NewGuid() : GetElementId(idValue);

			// An element link is always created as itself or as a placeholder, so the guid
			// should never be in use. Placeholders are created if a forward reference is made
			// to a class that is a link.
			Debug.Assert(null == myStore.ElementDirectory.FindElement(id));
			PlaceholderElement placeholder = default(PlaceholderElement);
			Dictionary<Guid, PlaceholderElement> placeholderMap = null;
			bool existingPlaceholder = false;
			if (idValue != null)
			{
				// See if this has been created before as a placeholder
				placeholderMap = myPlaceholderElementMap;
				if (placeholderMap != null)
				{
					existingPlaceholder = placeholderMap.TryGetValue(id, out placeholder);
				}
			}
			ElementLink retVal = myStore.ElementFactory.CreateElementLink(
				explicitMetaRelationshipInfo ?? oppositeMetaRoleInfo.DomainRelationship,
				new PropertyAssignment[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) },
				new RoleAssignment(oppositeMetaRoleInfo.OppositeDomainRole.Id, rolePlayer),
				new RoleAssignment(oppositeMetaRoleInfo.Id, oppositeRolePlayer));
			if (myNotifyAdded != null)
			{
				myNotifyAdded.ElementAdded(retVal);
			}
			if (existingPlaceholder)
			{
				placeholder.FulfilPlaceholderRoles(retVal);
				placeholderMap.Remove(id);
			}
			return retVal;
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="domainClassInfo">The meta class info of the element to create. If null,
		/// the domainClassId is used to find the class info</param>
		/// <param name="domainClassId">The identifier for the class</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, DomainClassInfo domainClassInfo, Guid domainClassId)
		{
			bool isNewElement;
			return CreateElement(idValue, domainClassInfo, domainClassId, false, out isNewElement);
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="domainClassInfo">The meta class info of the element to create. If null,
		/// the domainClassId is used to find the class info</param>
		/// <param name="domainClassId">The identifier for the class</param>
		/// <param name="createAsPlaceholder">The provided meta class information is not unique.
		/// If this element is not already created then add it with a separate tracked id so it can
		/// be replaced later by the fully resolved type. All role players will be automatically
		/// moved from the pending placeholder when the real element is created</param>
		/// <param name="isNewElement">true if the element is actually created, as opposed
		/// to being identified as an existing element</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, DomainClassInfo domainClassInfo, Guid domainClassId, bool createAsPlaceholder, out bool isNewElement)
		{
			isNewElement = false;

			// Get a valid guid identifier for the element
			Guid id = GetElementId(idValue);

			// See if we've already created this element as the opposite role player in a link
			ModelElement retVal = myStore.ElementDirectory.FindElement(id);
			if (retVal == null)
			{
				PlaceholderElement placeholder = default(PlaceholderElement);
				Dictionary<Guid, PlaceholderElement> placeholderMap = myPlaceholderElementMap;
				bool existingPlaceholder = placeholderMap != null && placeholderMap.TryGetValue(id, out placeholder);
				// The false parameter indicates that OnInitialize should not be called, which
				// is standard fare for deserialization routines.
				if (domainClassInfo == null)
				{
					domainClassInfo = myStore.DomainDataDirectory.GetDomainClass(domainClassId);
				}
				Type implClass = domainClassInfo.ImplementationClass;
				// Any request to create a DomainRelationshipInfo as an element instead of
				// an element link means a forward reference to a link object. Always create
				// this as a placeholder, given that we will eventually realize this as
				// a real link.
				if (createAsPlaceholder || implClass.IsAbstract || domainClassInfo is DomainRelationshipInfo)
				{
					if (placeholderMap == null)
					{
						myPlaceholderElementMap = placeholderMap = new Dictionary<Guid, PlaceholderElement>();
					}
					retVal = placeholder.CreatePlaceholderElement(myStore, domainClassInfo);
					placeholderMap[id] = placeholder;
				}
				else
				{
					retVal = myStore.ElementFactory.CreateElement(domainClassInfo, new PropertyAssignment(ElementFactory.IdPropertyAssignment, id));
					isNewElement = true;
					if (myNotifyAdded != null)
					{
						myNotifyAdded.ElementAdded(retVal);
					}
					if (existingPlaceholder)
					{
						placeholder.FulfilPlaceholderRoles(retVal);
						placeholderMap.Remove(id);
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
					myCustomIdToGuidMap = new Dictionary<string, Guid>(StringComparer.Ordinal);
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
	#endregion // Deserialization Routines
}
