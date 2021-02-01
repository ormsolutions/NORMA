#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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
using System.Collections;
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
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	#region Public Enumerations
	#region CustomSerializedElementSupportedOperations enum
	/// <summary>
	/// Supported operations for element custom serialization.
	/// </summary>
	[Flags]
	public enum CustomSerializedElementSupportedOperations
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
	#endregion // CustomSerializedElementSupportedOperations enum
	#region CustomSerializedElementWriteStyle enum
	/// <summary>
	/// Write style for element custom serialization.
	/// </summary>
	public enum CustomSerializedElementWriteStyle
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
	#endregion // CustomSerializedElementWriteStyle enum
	#region CustomSerializedAttributeWriteStyle enum
	/// <summary>
	/// Write style for property custom serialization.
	/// </summary>
	public enum CustomSerializedAttributeWriteStyle
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
	#endregion // CustomSerializedAttributeWriteStyle enum
	#region CustomSerializedElementMatchStyle enum
	/// <summary>
	/// An enum used for deserialization to determine if
	/// an element name and namespace is recognized by a
	/// custom serialized element.
	/// </summary>
	public enum CustomSerializedElementMatchStyle
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
	#endregion // CustomSerializedElementMatchStyle enum
	#region SerializationEngineLoadOptions enum
	/// <summary>
	/// Options modifying the behavior of <see cref="SerializationEngine.Load(System.IO.Stream,SerializationEngineLoadOptions)"/>.
	/// </summary>
	[Flags]
	public enum SerializationEngineLoadOptions
	{
		/// <summary>
		/// No special options
		/// </summary>
		None = 0,
		/// <summary>
		/// Do not do schema validation on the file load.
		/// </summary>
		SkipSchemaValidation = 1,
		/// <summary>
		/// Check the store for the <see cref="ISkipExtensions"/> interface
		/// to load schemas for extensions skipped in this load.
		/// </summary>
		ResolveSkippedExtensions = 2,
	}
	#endregion // SerializationEngineLoadOptions enum
	#endregion // Public Enumerations
	#region Public Classes
	#region CustomSerializedInfo class
	/// <summary>
	/// Custom serialization information.
	/// </summary>
	public abstract class CustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedInfo()
		{
		}
		/// <summary>
		/// Create customized serialization info
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		protected CustomSerializedInfo(string customPrefix, string customName, string customNamespace, string doubleTagName)
		{
			myCustomPrefix = customPrefix;
			myCustomName = customName;
			myCustomNamespace = customNamespace;
			myDoubleTagName = doubleTagName;
		}

		private readonly string myCustomPrefix;
		private readonly string myCustomName;
		private readonly string myCustomNamespace;
		private readonly string myDoubleTagName;

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
		}
		/// <summary>
		/// The custom name to use.
		/// </summary>
		public string CustomName
		{
			get { return myCustomName; }
		}
		/// <summary>
		/// The custom namespace to use.
		/// </summary>
		/// <value>The custom namespace to use.</value>
		public string CustomNamespace
		{
			get { return myCustomNamespace; }
		}
		/// <summary>
		/// The name of the double tag.
		/// </summary>
		/// <value>The name of the double tag.</value>
		public string DoubleTagName
		{
			get { return myDoubleTagName; }
		}
	}
	#endregion // CustomSerializedInfo class
	#region CustomSerializedElementInfo class
	/// <summary>
	/// Custom serialization information for elements.
	/// </summary>
	public class CustomSerializedElementInfo : CustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedElementInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writing.</param>
		public CustomSerializedElementInfo(CustomSerializedElementWriteStyle writeStyle)
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
		public CustomSerializedElementInfo(string customPrefix, string customName, string customNamespace, CustomSerializedElementWriteStyle writeStyle, string doubleTagName)
			: base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteStyle = writeStyle;
		}

		private CustomSerializedElementWriteStyle myWriteStyle;

		/// <summary>
		/// Default CustomSerializedElementInfo
		/// </summary>
		public static readonly CustomSerializedElementInfo Default = new CustomSerializedElementInfo();
		/// <summary>
		/// Commonly used 'not written' element info
		/// </summary>
		public static readonly CustomSerializedElementInfo NotWritten = new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);

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
		public CustomSerializedElementWriteStyle WriteStyle
		{
			get { return myWriteStyle; }
			set { myWriteStyle = value; }
		}
	}
	#endregion // CustomSerializedElementInfo class
	#region CustomSerializedPropertyInfo class
	/// <summary>
	/// Custom serialization information for properties.
	/// </summary>
	public class CustomSerializedPropertyInfo : CustomSerializedInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedPropertyInfo()
		{
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writeStyle">The style to use when writting.</param>
		public CustomSerializedPropertyInfo(CustomSerializedAttributeWriteStyle writeStyle)
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
		public CustomSerializedPropertyInfo(string customPrefix, string customName, string customNamespace, bool writeCustomStorage, CustomSerializedAttributeWriteStyle writeStyle, string doubleTagName)
			: base(customPrefix, customName, customNamespace, doubleTagName)
		{
			myWriteCustomStorage = writeCustomStorage;
			myWriteStyle = writeStyle;
		}

		private bool myWriteCustomStorage;
		private CustomSerializedAttributeWriteStyle myWriteStyle;

		/// <summary>
		/// Default CustomSerializedPropertyInfo
		/// </summary>
		public static readonly CustomSerializedPropertyInfo Default = new CustomSerializedPropertyInfo();

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
		public CustomSerializedAttributeWriteStyle WriteStyle
		{
			get { return myWriteStyle; }
			set { myWriteStyle = value; }
		}
	}
	#endregion // CustomSerializedPropertyInfo class
	#region CustomSerializedContainerElementInfo class
	/// <summary>
	/// Custom serialization information for container elements.
	/// </summary>
	public class CustomSerializedContainerElementInfo : CustomSerializedElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedContainerElementInfo()
		{
		}
		/// <summary>
		/// Basic element constructor
		/// </summary>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public CustomSerializedContainerElementInfo(string customName, params Guid[] childRoleIds)
			: base(null, customName, null, CustomSerializedElementWriteStyle.Element, null)
		{
			myGuidList = childRoleIds;
		}
		/// <summary>
		/// Full constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public CustomSerializedContainerElementInfo(string customPrefix, string customName, string customNamespace, CustomSerializedElementWriteStyle writeStyle, string doubleTagName, params Guid[] childRoleIds)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
		{
			myGuidList = childRoleIds;
		}

		private IList<Guid> myGuidList;

		/// <summary>
		/// Default CustomSerializedContainerElementInfo
		/// </summary>
		public new static readonly CustomSerializedContainerElementInfo Default = new CustomSerializedContainerElementInfo();

		/// <summary>
		/// Test if the list of child elements contains the specified guid
		/// </summary>
		public bool ContainsGuid(Guid guid)
		{
			return myGuidList != null && myGuidList.Contains(guid);
		}
		/// <summary>
		/// Get the outer container of this element. Will be null unless this
		/// is a CustomSerializedInnerContainerElementInfo instance
		/// </summary>
		public virtual CustomSerializedContainerElementInfo OuterContainer
		{
			get
			{
				return null;
			}
		}
	}
	#endregion // CustomSerializedContainerElementInfo class
	#region CustomSerializedStandaloneLinkInfo class
	/// <summary>
	/// A class with information for serializing a standalone link element
	/// </summary>
	public class CustomSerializedStandaloneLinkElementInfo : CustomSerializedElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedStandaloneLinkElementInfo()
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
		public CustomSerializedStandaloneLinkElementInfo(string customPrefix, string customName, string customNamespace, CustomSerializedElementWriteStyle writeStyle, string doubleTagName, CustomSerializedStandaloneRelationship standaloneRelationship)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName)
		{
			myRelationship = standaloneRelationship;
		}
		private CustomSerializedStandaloneRelationship myRelationship;
		/// <summary>
		/// Get the <see cref="CustomSerializedStandaloneRelationship"/> information associated with this link.
		/// </summary>
		public CustomSerializedStandaloneRelationship StandaloneRelationship
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
	#endregion // CustomSerializedStandaloneLinkElementInfo class
	#region CustomSerializedInnerContainerElementInfo class
	/// <summary>
	/// Custom serialization information for nested container elements.
	/// </summary>
	public class CustomSerializedInnerContainerElementInfo : CustomSerializedContainerElementInfo
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected CustomSerializedInnerContainerElementInfo()
		{
		}
		/// <summary>
		/// Basic constructor
		/// </summary>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="outerContainer">The outer container</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public CustomSerializedInnerContainerElementInfo(string customName, CustomSerializedContainerElementInfo outerContainer, params Guid[] childRoleIds)
			: base(null, customName, null, CustomSerializedElementWriteStyle.Element, null, childRoleIds)
		{
			myOuterContainer = outerContainer;
		}
		/// <summary>
		/// Full Constructor
		/// </summary>
		/// <param name="customPrefix">The custom prefix to use.</param>
		/// <param name="customName">The custom name to use.</param>
		/// <param name="customNamespace">The custom namespace to use.</param>
		/// <param name="writeStyle">The style to use when writting.</param>
		/// <param name="doubleTagName">The name of the double tag.</param>
		/// <param name="outerContainer">The outer container</param>
		/// <param name="childRoleIds">The domain role ideas of opposite child elements.</param>
		public CustomSerializedInnerContainerElementInfo(string customPrefix, string customName, string customNamespace, CustomSerializedElementWriteStyle writeStyle, string doubleTagName, CustomSerializedContainerElementInfo outerContainer, params Guid[] childRoleIds)
			: base(customPrefix, customName, customNamespace, writeStyle, doubleTagName, childRoleIds)
		{
			myOuterContainer = outerContainer;
		}
		private CustomSerializedContainerElementInfo myOuterContainer;
		/// <summary>
		/// Get the container element for this item
		/// </summary>
		public override CustomSerializedContainerElementInfo OuterContainer
		{
			get
			{
				return myOuterContainer;
			}
		}
	}
	#endregion // CustomSerializedInnerContainerElementInfo class
	#region CustomSerializedElementMatch struct
	/// <summary>
	/// Data returned by ICustomSerializedElement.MapElementName.
	/// </summary>
	public struct CustomSerializedElementMatch
	{
		#region Member Variables
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
		private CustomSerializedStandaloneRelationship[] myStandaloneRelationship;
		/// <summary>
		/// Hold the explicit forward reference information for this match.
		/// Stored as an array to minimized storage impact when not used.
		/// </summary>
		private Guid[] myExplicitForwardReferenceGuids;
		private CustomSerializedElementMatchStyle myMatchStyle;
		private const CustomSerializedElementMatchStyle StyleMask = (CustomSerializedElementMatchStyle)0xFFFF;
		private const CustomSerializedElementMatchStyle AllowDuplicatesBit = (CustomSerializedElementMatchStyle)((int)StyleMask + 1);
		private string myDoubleTagName;
		#endregion // Member Variables
		#region InitializeAttribute method
		/// <summary>
		/// The element was recognized as a meta property.
		/// </summary>
		/// <param name="metaAttributeGuid">The guid identifying the meta property</param>
		/// <param name="doubleTagName">the name of the double tag, if any</param>
		public void InitializeAttribute(Guid metaAttributeGuid, string doubleTagName)
		{
			mySingleGuid = metaAttributeGuid;
			myMatchStyle = CustomSerializedElementMatchStyle.Property;
			myDoubleTagName = (doubleTagName != null && doubleTagName.Length != 0) ? doubleTagName : null;
			myStandaloneRelationship = null;
		}
		#endregion // InitializeAttribute method
		#region InitializeRoles methods
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, null, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(CustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(bool allowDuplicates, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, null, null, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRoles(bool allowDuplicates, CustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, null, oppositeDomainRoleIds);
		}
		private void InitializeRoles(bool allowDuplicates, CustomSerializedStandaloneRelationship[] standaloneRelationship, Guid[] explicitForwardReferenceIds, params Guid[] oppositeDomainRoleIds)
		{
			Debug.Assert(oppositeDomainRoleIds != null && oppositeDomainRoleIds.Length != 0);
			if (oppositeDomainRoleIds.Length == 1)
			{
				mySingleGuid = oppositeDomainRoleIds[0];
				myMultiGuids = null;
				myMatchStyle = CustomSerializedElementMatchStyle.SingleOppositeDomainRole;
			}
			else
			{
				mySingleGuid = Guid.Empty;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = CustomSerializedElementMatchStyle.MultipleOppositeDomainRoles;
			}
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myDoubleTagName = null;
			myExplicitForwardReferenceGuids = explicitForwardReferenceIds;
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, null, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(CustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(bool allowDuplicates, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, null, null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRoles(bool allowDuplicates, CustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, null, oppositeDomainRoleId);
		}
		private void InitializeRoles(bool allowDuplicates, CustomSerializedStandaloneRelationship[] standaloneRelationship, Guid[] explicitForwardReferenceIds, Guid oppositeDomainRoleId)
		{
			mySingleGuid = oppositeDomainRoleId;
			myMultiGuids = null;
			myMatchStyle = CustomSerializedElementMatchStyle.SingleOppositeDomainRole;
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myStandaloneRelationship = standaloneRelationship;
			myExplicitForwardReferenceGuids = explicitForwardReferenceIds;
		}
		#endregion // InitializeRoles methods
		#region InitializeRolesWithExplicitForwardReference methods
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="explicitForwardReferenceIds">Specify an explicit type per opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitForwardReference(Guid[] explicitForwardReferenceIds, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, null, explicitForwardReferenceIds, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="explicitForwardReferenceIds">Specify an explicit type per opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitForwardReference(CustomSerializedStandaloneRelationship standaloneRelationship, Guid[] explicitForwardReferenceIds, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(false, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, explicitForwardReferenceIds, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitForwardReferenceIds">Specify an explicit type per opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitForwardReference(bool allowDuplicates, Guid[] explicitForwardReferenceIds, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, null, explicitForwardReferenceIds, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="explicitForwardReferenceIds">Specify an explicit type per opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitForwardReference(bool allowDuplicates, CustomSerializedStandaloneRelationship standaloneRelationship, Guid[] explicitForwardReferenceIds, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRoles(allowDuplicates, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, explicitForwardReferenceIds, oppositeDomainRoleIds);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="explicitForwardReferenceId">Specify an explicit type for the opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitForwardReference(Guid explicitForwardReferenceId, Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, null, (explicitForwardReferenceId != Guid.Empty) ? new Guid[] { explicitForwardReferenceId } : null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="explicitForwardReferenceId">Specify an explicit type for the opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitForwardReference(CustomSerializedStandaloneRelationship standaloneRelationship, Guid explicitForwardReferenceId, Guid oppositeDomainRoleId)
		{
			InitializeRoles(false, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, (explicitForwardReferenceId != Guid.Empty) ? new Guid[] { explicitForwardReferenceId } : null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="explicitForwardReferenceId">Specify an explicit type for the opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitForwardReference(bool allowDuplicates, Guid explicitForwardReferenceId, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, null, (explicitForwardReferenceId != Guid.Empty) ? new Guid[] { explicitForwardReferenceId } : null, oppositeDomainRoleId);
		}
		/// <summary>
		/// The element was recognized as an opposite role player. Optimized overload
		/// for 1 element.
		/// </summary>
		/// <param name="allowDuplicates">Allow duplicates for this link</param>
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="explicitForwardReferenceId">Specify an explicit type for the opposite domain role
		/// to create in a forward reference situation.</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitForwardReference(bool allowDuplicates, CustomSerializedStandaloneRelationship standaloneRelationship, Guid explicitForwardReferenceId, Guid oppositeDomainRoleId)
		{
			InitializeRoles(allowDuplicates, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, (explicitForwardReferenceId != Guid.Empty) ? new Guid[] { explicitForwardReferenceId } : null, oppositeDomainRoleId);
		}
		#endregion // InitializeRolesWithExplicitForwardReference methods
		#region InitializeRolesWithExplicitRelationship methods
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
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
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
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleIds">1 or more opposite meta role guids</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleIds);
		}
		private void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship[] standaloneRelationship, params Guid[] oppositeDomainRoleIds)
		{
			Debug.Assert(oppositeDomainRoleIds != null && oppositeDomainRoleIds.Length != 0);
			if (oppositeDomainRoleIds.Length == 1)
			{
				mySingleGuid = explicitRelationshipGuid;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType;
			}
			else
			{
				mySingleGuid = explicitRelationshipGuid;
				myMultiGuids = oppositeDomainRoleIds;
				myMatchStyle = CustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType;
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
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(false, explicitRelationshipGuid, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
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
		/// <param name="standaloneRelationship">A <see cref="CustomSerializedStandaloneRelationship"/> structure</param>
		/// <param name="oppositeDomainRoleId">The opposite meta role guid</param>
		public void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship standaloneRelationship, Guid oppositeDomainRoleId)
		{
			InitializeRolesWithExplicitRelationship(allowDuplicates, explicitRelationshipGuid, new CustomSerializedStandaloneRelationship[] { standaloneRelationship }, oppositeDomainRoleId);
		}
		private void InitializeRolesWithExplicitRelationship(bool allowDuplicates, Guid explicitRelationshipGuid, CustomSerializedStandaloneRelationship[] standaloneRelationship, Guid oppositeDomainRoleId)
		{
			mySingleGuid = explicitRelationshipGuid;
			myMultiGuids = new Guid[] { oppositeDomainRoleId };
			myMatchStyle = CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType;
			if (allowDuplicates)
			{
				myMatchStyle |= AllowDuplicatesBit;
			}
			myStandaloneRelationship = standaloneRelationship;
		}
		#endregion // InitializeRolesWithExplicitRelationship methods
		#region Accessor Properties
		/// <summary>
		/// The guid identifying the meta property. Valid for a match
		/// style of Property.
		/// </summary>
		public Guid DomainPropertyId
		{
			get
			{
				return ((myMatchStyle & StyleMask) == CustomSerializedElementMatchStyle.Property) ? mySingleGuid : Guid.Empty;
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
					case CustomSerializedElementMatchStyle.SingleOppositeDomainRole:
						return mySingleGuid;
					case CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
						return myMultiGuids[0];
					default:
						return Guid.Empty;
				}
			}
		}
		/// <summary>
		/// Return the identifier for a type that satisfies the opposite role
		/// that should be created for a forward reference.
		/// </summary>
		public Guid SingleForwardReferenceTypeId
		{
			get
			{
				Guid[] forwardReferenceIds = myExplicitForwardReferenceGuids;
				if (forwardReferenceIds != null)
				{
					switch (myMatchStyle & StyleMask)
					{
						case CustomSerializedElementMatchStyle.SingleOppositeDomainRole:
						case CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
							return forwardReferenceIds[0];
					}
				}
				return Guid.Empty;
			}
		}
		/// <summary>
		/// Return the identifiers for types that satisfy the opposite role
		/// that should be created for forward references needed to create
		/// the relationships.
		/// </summary>
		public IList<Guid> ForwardReferenceTypeIdCollection
		{
			get
			{
				Guid[] forwardReferenceIds = myExplicitForwardReferenceGuids;
				if (forwardReferenceIds != null)
				{
					switch (myMatchStyle & StyleMask)
					{
						case CustomSerializedElementMatchStyle.SingleOppositeDomainRole:
						case CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
							return Array.AsReadOnly<Guid>(forwardReferenceIds);
					}
				}
				return null;
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
					case CustomSerializedElementMatchStyle.MultipleOppositeDomainRoles:
					case CustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
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
					case CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
					case CustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
						return mySingleGuid;
					default:
						return Guid.Empty;
				}
			}
		}
		/// <summary>
		/// Return the standalone relationship info used to create this link
		/// </summary>
		public CustomSerializedStandaloneRelationship? StandaloneRelationship
		{
			get
			{
				CustomSerializedStandaloneRelationship[] relationshipInfo = myStandaloneRelationship;
				return (relationshipInfo != null) ? relationshipInfo[0] : (CustomSerializedStandaloneRelationship?)null;
			}
		}
		/// <summary>
		/// The type of element match
		/// </summary>
		/// <value>CustomSerializedElementMatchStyle</value>
		public CustomSerializedElementMatchStyle MatchStyle
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
		#endregion // Accessor Properties
	}
	#endregion // CustomSerializedElementMatch struct
	#endregion // Public Classes
	#region Public Structures
	#region CustomSerializedRootRelationshipContainer struct
	/// <summary>
	/// Used with <see cref="ICustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent relationships that are not serialized under any model.
	/// </summary>
	public struct CustomSerializedRootRelationshipContainer
	{
		private string myContainerName;
		private string myContainerPrefix;
		private string myContainerNamespace;
		private CustomSerializedStandaloneRelationship[] myRelationships;
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
		/// by an array of <see cref="CustomSerializedStandaloneRelationship"/> elements
		/// </summary>
		public CustomSerializedStandaloneRelationship[] GetRelationshipClasses()
		{
			return myRelationships;
		}
		/// <summary>
		/// Create a new <see cref="CustomSerializedRootRelationshipContainer"/> 
		/// </summary>
		/// <param name="containerPrefix">Value returned by the <see cref="ContainerPrefix"/> property</param>
		/// <param name="containerName">Value returned by the <see cref="ContainerName"/> property</param>
		/// <param name="containerNamespace">Value returned by the <see cref="ContainerNamespace"/> property</param>
		/// <param name="relationships">Array of relationships returned by the <see cref="GetRelationshipClasses"/> method</param>
		public CustomSerializedRootRelationshipContainer(string containerPrefix, string containerName, string containerNamespace, CustomSerializedStandaloneRelationship[] relationships)
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
		/// <returns><see cref="Nullable{CustomSerializedRootRelationshipContainer}"/></returns>
		public static CustomSerializedRootRelationshipContainer? Find(CustomSerializedRootRelationshipContainer[] containers, string containerName, string containerNamespace)
		{
			if (containers != null)
			{
				for (int i = 0; i < containers.Length; ++i)
				{
					CustomSerializedRootRelationshipContainer currentContainer = containers[i];
					if (currentContainer.ContainerName == containerName && currentContainer.ContainerNamespace == containerNamespace)
					{
						return currentContainer;
					}
				}
			}
			return null;
		}
	}
	#endregion // CustomSerializedRootRelationshipContainer struct
	#region CustomSerializedStandaloneRelationshipRole struct
	/// <summary>
	/// Used with the <see cref="ICustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent a single role to be written to a root link element as represented by the <see cref="CustomSerializedStandaloneRelationship"/> structure.
	/// </summary>
	public struct CustomSerializedStandaloneRelationshipRole
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
		/// Create a new <see cref="CustomSerializedStandaloneRelationshipRole"/>
		/// </summary>
		/// <param name="attributeRoleName">Value corresponding to the <see cref="AttributeName"/> property</param>
		/// <param name="domainRoleId">Value corresponding to the <see cref="DomainRoleId"/> property</param>
		public CustomSerializedStandaloneRelationshipRole(string attributeRoleName, Guid domainRoleId)
		{
			myAttributeName = attributeRoleName;
			myDomainRoleId = domainRoleId;
		}
	}
	#endregion // CustomSerializedStandaloneRelationshipRole struct
	#region CustomSerializedStandaloneRelationship struct
	/// <summary>
	/// Used with the <see cref="ICustomSerializedDomainModel.GetRootRelationshipContainers"/> method
	/// to represent a single relationship serialized inside a <see cref="CustomSerializedRootRelationshipContainer"/>.
	/// </summary>
	public struct CustomSerializedStandaloneRelationship
	{
		private string myElementName;
		private string myElementPrefix;
		private string myElementNamespace;
		private Guid myDomainClassId;
		private CustomSerializedStandaloneRelationshipRole[] myRoles;
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
		/// Get the <see cref="CustomSerializedStandaloneRelationshipRole"/> representing
		/// how the source and target references are to be written for this element.
		/// </summary>
		public CustomSerializedStandaloneRelationshipRole[] GetRoles()
		{
			return myRoles;
		}
		/// <summary>
		/// Create a new <see cref="CustomSerializedStandaloneRelationship"/>
		/// </summary>
		/// <param name="domainClassId">Value for the <see cref="DomainClassId"/> property</param>
		/// <param name="roles">Value for the <see cref="GetRoles"/> method</param>
		/// <param name="elementPrefix">Value for the <see cref="ElementPrefix"/> property</param>
		/// <param name="elementName">Value for the <see cref="ElementName"/> property. If the name is null then this is the primary link element and all name information is retrieved from the class.</param>
		/// <param name="elementNamespace">Value for the <see cref="ElementNamespace"/> property</param>
		public CustomSerializedStandaloneRelationship(Guid domainClassId, CustomSerializedStandaloneRelationshipRole[] roles, string elementPrefix, string elementName, string elementNamespace)
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
		/// <returns><see cref="Nullable{CustomSerializedStandaloneRelationship}"/></returns>
		public static CustomSerializedStandaloneRelationship? Find(CustomSerializedStandaloneRelationship[] relationships, string elementName, string xmlNamespace)
		{
			if (relationships != null)
			{
				for (int i = 0; i < relationships.Length; ++i)
				{
					CustomSerializedStandaloneRelationship currentRelationship = relationships[i];
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
		/// <returns><see cref="Nullable{CustomSerializedStandaloneRelationship}"/></returns>
		public static CustomSerializedStandaloneRelationship? Find(CustomSerializedStandaloneRelationship[] relationships, Guid relationshipDomainClassId)
		{
			if (relationships != null)
			{
				for (int i = 0; i < relationships.Length; ++i)
				{
					CustomSerializedStandaloneRelationship currentRelationship = relationships[i];
					if (currentRelationship.DomainClassId == relationshipDomainClassId)
					{
						return currentRelationship;
					}
				}
			}
			return null;
		}
	}
	#endregion // CustomSerializedStandaloneRelationship struct
	#endregion // Public Structures
	#region Public Interfaces
	#region ISerializationContext interface
	/// <summary>
	/// A service interface used to enable custom implementations
	/// of <see cref="IXmlSerializable"/> to cleanly participate in order-independent
	/// single-pass loading where the status (created or not yet created)
	/// of referenced elements is not known. The context store for this serializer
	/// must implement <see cref="ISerializationContextHost"/> to make this
	/// information available.
	/// </summary>
	public interface ISerializationContext
	{
		/// <summary>
		/// Get a normalized string form of the provided <see cref="Guid"/> identifier.
		/// May be used during serialization.
		/// </summary>
		string GetIdentifierString(Guid id);
		/// <summary>
		/// Get the <see cref="Guid"/> identifier that corresponds to the
		/// provided <paramref name="idValue"/>. Used during deserialization.
		/// </summary>
		/// <param name="idValue">The string identifier. This may or may not
		/// be a Guid value. Any identifier unique across the file will do.</param>
		/// <returns>The same <see cref="Guid"/> for all requests of this <paramref name="idValue"/></returns>
		Guid ResolveElementIdentifier(string idValue);
		/// <summary>
		/// Get an element of the specified type and identifier. If the element
		/// is created as a reference, then there is no guarantee that the returned
		/// element will be the final form of this element or that the <see cref="ModelElement.Id"/>
		/// of the returned element matches the requested <paramref name="idValue"/>.
		/// May be used during deserialization.
		/// </summary>
		/// <param name="idValue">An identifier value unique across the file.</param>
		/// <param name="domainClassIdentifier">The identifier of the <see cref="DomainClassInfo"/> for
		/// the element to create.</param>
		/// <param name="isReference">Determine if the element is being primarily created or referenced.</param>
		/// <returns>A corresponding <see cref="ModelElement"/>. If <paramref name="isReference"/> is <see langword="true"/>,
		/// then the returned element should never be cached. A subsequent call with the same parameter values
		/// can be made at any time to efficiently retrieve the current element for this identifier.</returns>
		ModelElement RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference);
		/// <summary>
		/// Get an element of the specified type and identifier. If the element
		/// is created as a reference, then there is no guarantee that the returned
		/// element will be the final form of this element or that the <see cref="ModelElement.Id"/>
		/// of the returned element matches the requested <paramref name="idValue"/>.
		/// May be used during deserialization.
		/// </summary>
		/// <param name="idValue">An identifier value unique across the file.</param>
		/// <param name="domainClassIdentifier">The identifier of the <see cref="DomainClassInfo"/> for
		/// the element to create.</param>
		/// <param name="isReference">Determine if the element is being primarily created or referenced.</param>
		/// <param name="explicitForwardReferenceDomainClassIdentifier">If a reference is being created and the
		/// target element is serialized as a top-level element, but has not yet been created, then the true type
		/// of the target elements is not known. However, if a 'lucky' guess is made and the true type of the
		/// element is created, then it can change the top-level element order, resulting in a large change in
		/// the serialized file. This specifies a class to create as a temporary placeholder for the forward
		/// reference. The temporary class will be deleted before serialization completes</param>
		/// <returns>A corresponding <see cref="ModelElement"/>. If <paramref name="isReference"/> is <see langword="true"/>,
		/// then the returned element should never be cached. A subsequent call with the same parameter values
		/// can be made at any time to efficiently retrieve the current element for this identifier.</returns>
		ModelElement RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference, Guid explicitForwardReferenceDomainClassIdentifier);
		/// <summary>
		/// Create an element link after verifying that the link needs to be created. May be
		/// used during deserialization.
		/// </summary>
		/// <param name="idValue">The value of the id for the link, or null</param>
		/// <param name="rolePlayer">The near role player</param>
		/// <param name="oppositeRolePlayer">The opposite role player</param>
		/// <param name="oppositeDomainRoleInfoId">The identifier for the opposite <see cref="DomainRoleInfo"/></param>
		/// <param name="explicitDomainRelationshipInfoId">The relationship type to create.
		/// Derived from <paramref name="oppositeDomainRoleInfoId"/> if not specified.</param>
		/// <returns>The newly created element link</returns>
		ElementLink RealizeElementLink(string idValue, ModelElement rolePlayer, ModelElement oppositeRolePlayer, Guid oppositeDomainRoleInfoId, Guid? explicitDomainRelationshipInfoId);
	}
	#endregion // ISerializationContext interface
	#region ISerializationContextHost interface
	/// <summary>
	/// Implement on the context <see cref="Store"/> passed to the <see cref="SerializationEngine"/>
	/// constructor to enable <see cref="IXmlSerializable"/> elements to participate in order-independent
	/// single-pass loading.
	/// </summary>
	public interface ISerializationContextHost
	{
		/// <summary>
		/// The current serialization context
		/// </summary>
		ISerializationContext SerializationContext { get; set;}
	}
	#endregion // ISerializationContextHost interface
	#region IDomainModelEnablesRulesAfterDeserialization interface
	/// <summary>
	/// Interface implemented on a DomainModel to enable
	/// initially disabled rules after deserialization.
	/// </summary>
	public interface IDomainModelEnablesRulesAfterDeserialization
	{
		/// <summary>
		/// Called after successful deserialization to enable
		/// rules that were initially disabled.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> that loaded the rules. Enable rules using the <see cref="Store.RuleManager">RuleManager</see> from the store.</param>
		void EnableRulesAfterDeserialization(Store store);
	}
	#endregion // IDomainModelEnablesRulesAfterDeserialization interface
	#region ICustomSerializedDomainModel interface
	/// <summary>
	/// The interface for retrieving custom serialization information about a namespace
	/// </summary>
	public interface ICustomSerializedDomainModel
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
		CustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers();
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
		/// Filter root element instances.
		/// </summary>
		/// <param name="rootElement">A element of a type specified by <see cref="GetRootElementClasses"/></param>
		/// <returns>Returns <see langword="true"/> if the element should serialize.</returns>
		bool ShouldSerializeRootElement(ModelElement rootElement);
		/// <summary>
		/// Map an xml namespace name and element name to a meta class guid
		/// </summary>
		/// <param name="xmlNamespace">The namespace of a top-level element (directly
		/// inside the root tag)</param>
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
	#endregion // ICustomSerializedDomainModel interface
	#region ICustomSerializedElement interface
	/// <summary>
	/// The interface for getting element custom serialization information.
	/// </summary>
	public interface ICustomSerializedElement
	{
		/// <summary>
		/// Returns the supported operations.
		/// </summary>
		CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations { get;}
		/// <summary>
		/// Returns custom serialization information for child elements.
		/// </summary>
		/// <returns>Custom serialization information for child elements.</returns>
		CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo();
		/// <summary>
		/// Returns custom serialization information for elements.
		/// </summary>
		CustomSerializedElementInfo CustomSerializedElementInfo { get;}
		/// <summary>
		/// Returns custom serialization information for properties.
		/// </summary>
		/// <param name="domainPropertyInfo">The property info.</param>
		/// <param name="rolePlayedInfo">If this is implemented on a ElementLink-derived class, then the
		/// played role is the role player containing the reference to the opposite role. Always null for a
		/// class element.</param>
		/// <returns>Custom serialization information for properties.</returns>
		CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo);
		/// <summary>
		/// Returns custom serialization information for links.
		/// </summary>
		/// <param name="rolePlayedInfo">The role played.</param>
		/// <param name="elementLink">The link instance</param>
		/// <returns>Custom serialization information for links.</returns>
		CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink);
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
		/// <returns>CustomSerializedElementMatch. Use the MatchStyle property to determine levels of success.</returns>
		CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName);
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
	#endregion // ICustomSerializedElement interface
	#region ISkipExtension interface
	/// <summary>
	/// Implement on a store when extensions are available but not loaded. This
	/// allows non-displayed documents to be loaded without the cost of loading
	/// non-generative extensions such as models used for display or editing of
	/// models and diagrams but not for code generation. This will be checked if
	/// the <see cref="SerializationEngineLoadOptions.ResolveSkippedExtensions"/>
	/// flag is set on the call to <see cref="SerializationEngine.Load(System.IO.Stream,SerializationEngineLoadOptions)"/>
	/// </summary>
	public interface ISkipExtensions
	{
		/// <summary>
		/// Get or set a list of skipped extension types. Can be null.
		/// </summary>
		IList<Type> SkippedExtensionTypes { get; set; }
	}
	#endregion // ISkipExtension interface
	#endregion Public Interfaces
	#region Public Attributes
	#region CustomSerializedXmlNamespacesAttribute class
	/// <summary>
	/// An attribute associated with a <see cref="DomainModel"/> that indicates all of the
	/// xml namespaces serialized by that model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	[Obsolete("Please use CustomSerializedXmlSchemaAttribute instead of CustomSerializedXmlNamespacesAttribute.")]
	public sealed class CustomSerializedXmlNamespacesAttribute : Attribute, IEnumerable<string>
	{
		#region Member variables
		private string[] myNamespaces;
		#endregion // Member variables
		#region Constructors (lots of overrides to eliminate CLSCompliant attribute array warnings)
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with a single xml namespace.
		/// </summary>
		/// <param name="xmlNamespace">The namespace to associate with this domain model.</param>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5, string xmlNamespace6)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5), EnsureNotNull(xmlNamespace6) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5, string xmlNamespace6, string xmlNamespace7)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5), EnsureNotNull(xmlNamespace6), EnsureNotNull(xmlNamespace7) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5, string xmlNamespace6, string xmlNamespace7, string xmlNamespace8)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5), EnsureNotNull(xmlNamespace6), EnsureNotNull(xmlNamespace7), EnsureNotNull(xmlNamespace8) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5, string xmlNamespace6, string xmlNamespace7, string xmlNamespace8, string xmlNamespace9)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5), EnsureNotNull(xmlNamespace6), EnsureNotNull(xmlNamespace7), EnsureNotNull(xmlNamespace8), EnsureNotNull(xmlNamespace9) };
		}
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlNamespacesAttribute"/> with multiple namespaces.
		/// </summary>
		public CustomSerializedXmlNamespacesAttribute(string xmlNamespace1, string xmlNamespace2, string xmlNamespace3, string xmlNamespace4, string xmlNamespace5, string xmlNamespace6, string xmlNamespace7, string xmlNamespace8, string xmlNamespace9, string xmlNamespace10)
		{
			myNamespaces = new string[] { EnsureNotNull(xmlNamespace1), EnsureNotNull(xmlNamespace2), EnsureNotNull(xmlNamespace3), EnsureNotNull(xmlNamespace4), EnsureNotNull(xmlNamespace5), EnsureNotNull(xmlNamespace6), EnsureNotNull(xmlNamespace7), EnsureNotNull(xmlNamespace8), EnsureNotNull(xmlNamespace9), EnsureNotNull(xmlNamespace10) };
		}
		private static string EnsureNotNull(string s)
		{
			return s ?? "";
		}
		#endregion // Constructors
		#region Public accessor properties
		/// <summary>
		/// Return the total number of namespaces defined on this DomainModel
		/// </summary>
		public int Count
		{
			get
			{
				return (myNamespaces as ICollection).Count;
			}
		}
		/// <summary>
		/// Return the namespace at this index
		/// </summary>
		/// <param name="index">An index, bounded by <see cref="Count"/></param>
		/// <returns>An xml namespace string</returns>
		public string this[int index]
		{
			get
			{
				return myNamespaces[index];
			}
		}
		#endregion // Public accessor properties
		#region IEnumerable<string> Implementation
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return (myNamespaces as IEnumerable<string>).GetEnumerator();
		}
		#endregion // IEnumerable<string> implementation
		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator()
		{
			return myNamespaces.GetEnumerator();
		}
		#endregion // IEnumerable implementation
	}
	#endregion // CustomSerializedXmlNamespacesAttribute class
	#region CustomSerializedXmlSchemaAttribute class
	/// <summary>
	/// An attribute associated with a <see cref="DomainModel"/> that indicates the
	/// namespace and corresponding resource name for an xml namespace serialized by
	/// that model. Use multiple attributes for multiple namespaces.
	/// </summary>
	/// <remarks><see cref="ICustomSerializedDomainModel"/> for a discussion on how
	/// to build the schema file into the model assembly as a resource.</remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class CustomSerializedXmlSchemaAttribute : Attribute
	{
		#region Member variables
		private string myNamespace;
		private string mySchemaFile;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create an <see cref="CustomSerializedXmlSchemaAttribute"/> with a namespace and schema file.
		/// </summary>
		/// <param name="xmlNamespace">An xml namespace to associate with this domain model.</param>
		/// <param name="schemaFile">The name of a resoure in the model assembly that contains
		/// the schema file. See comments in <see cref="ICustomSerializedDomainModel"/> for information on
		/// how to integrate this file as part of the build process</param>
		public CustomSerializedXmlSchemaAttribute(string xmlNamespace, string schemaFile)
		{
			myNamespace = xmlNamespace;
			mySchemaFile = schemaFile;
		}
		#endregion // Constructors
		#region Public accessor properties
		/// <summary>
		/// An XML namespace associated with the domain model
		/// </summary>
		public string XmlNamespace
		{
			get
			{
				return myNamespace;
			}
			set
			{
				myNamespace = value;
			}
		}
		/// <summary>
		/// The XML schema file for this namespace.
		/// Used to resolve the schema file from a resource in this assembly.
		/// </summary>
		public string SchemaFile
		{
			get
			{
				return mySchemaFile;
			}
			set
			{
				mySchemaFile = value;
			}
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override bool IsDefaultAttribute()
		{
			return myNamespace == null && mySchemaFile == null;
		}
		#endregion // Public accessor properties
	}
	#endregion // CustomSerializedXmlSchemaAttribute class
	#endregion // Public Attributes
	#region Serialization Routines
	/// <summary>
	/// Serialization routines
	/// </summary>
	public abstract partial class SerializationEngine
	{
		/// <summary>
		/// Provide the serialized prefix for the root element
		/// </summary>
		protected abstract string GetRootXmlPrefix();
		/// <summary>
		/// Provide the element name for the root XML element
		/// </summary>
		protected abstract string GetRootXmlElementName();
		/// <summary>
		/// Provide the namespace name for the root XML element
		/// </summary>
		protected abstract string GetRootXmlNamespace();
		/// <summary>
		/// The name of an embedded schema resource in the same location as the class concrete serialization class
		/// </summary>
		protected abstract string GetRootSchemaFileName();

		/// <summary>
		/// Used for sorting.
		/// </summary>
		/// <param name="writeStyle">An property write style.</param>
		/// <returns>A number to sort with.</returns>
		private static int PropertyWriteStylePriority(CustomSerializedAttributeWriteStyle writeStyle)
		{
			switch (writeStyle)
			{
				case CustomSerializedAttributeWriteStyle.Attribute:
					return 0;
				case CustomSerializedAttributeWriteStyle.Element:
				case CustomSerializedAttributeWriteStyle.DoubleTaggedElement:
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
		private static int FindGuid(CustomSerializedContainerElementInfo[] childElementInfo, Guid guid)
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
		private static void SortProperties(ICustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, ref IList<DomainPropertyInfo> properties)
		{
			int propertyCount = properties.Count;
			if (propertyCount > 1)
			{
				CustomSerializedPropertyInfo[] customInfo = new CustomSerializedPropertyInfo[propertyCount];
				int[] indices = new int[propertyCount];
				for (int i = 0; i < propertyCount; ++i)
				{
					indices[i] = i;
					customInfo[i] = customElement.GetCustomSerializedPropertyInfo(properties[i], rolePlayedInfo);
				}
				Array.Sort<int>(indices, delegate(int index1, int index2)
				{
					CustomSerializedPropertyInfo customInfo1 = customInfo[index1];
					CustomSerializedPropertyInfo customInfo2 = customInfo[index2];
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
					if (ws0 == 1)
					{
						// Sort attributes rendered as elements in model definition order.
						return index1.CompareTo(index2);
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
		private static bool WriteCustomizedStartElement(XmlWriter file, CustomSerializedElementInfo customInfo, CustomSerializedElementInfo containerInfo, string defaultPrefix, string defaultName)
		{
			if (customInfo != null)
			{
				switch (customInfo.WriteStyle)
				{
					case CustomSerializedElementWriteStyle.NotWritten:
						{
							return false;
						}
					case CustomSerializedElementWriteStyle.DoubleTaggedElement:
						{
							string prefix = customInfo.CustomPrefix ?? defaultPrefix;
							string name = customInfo.CustomName ?? defaultName;

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
								customInfo.DoubleTagName ?? name,
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
					customInfo.CustomPrefix ?? defaultPrefix,
					customInfo.CustomName ?? defaultName,
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
		private static void WriteCustomizedEndElement(XmlWriter file, CustomSerializedElementInfo customInfo)
		{
			if (customInfo != null)
			{
				switch (customInfo.WriteStyle)
				{
#if DEBUG
					case CustomSerializedElementWriteStyle.NotWritten:
						{
							Debug.Fail("WriteCustomizedEndElement - CustomSerializedElementWriteStyle.DontWrite");
							throw new InvalidOperationException();
						}
#endif
					case CustomSerializedElementWriteStyle.DoubleTaggedElement:
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
		/// <returns>ICustomSerializedDomainModel, or null</returns>
		private static ICustomSerializedDomainModel GetParentModel(ModelElement element)
		{
			return element.Store.GetDomainModel(element.GetDomainClass().DomainModel.Id) as ICustomSerializedDomainModel;
		}
		/// <summary>
		/// Determine based on the type of role and opposite role player if any elements of
		/// the given type should be serialized.
		/// </summary>
		/// <param name="parentModel">The parent model of an element</param>
		/// <param name="role">The role played</param>
		/// <returns>true if serialization should continue</returns>
		private static bool ShouldSerializeDomainRole(ICustomSerializedDomainModel parentModel, DomainRoleInfo role)
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
			ICustomSerializedElement customElement = modelElement as ICustomSerializedElement;
			return (customElement != null) ? customElement.ShouldSerialize() : true;
		}
		/// <summary>
		/// Determine if an element should be serialized
		/// </summary>
		/// <param name="modelElement">Element to test</param>
		/// <param name="verifyDomainModelSerialization">If true, verify that a serializable element is also in a serializable domain model.</param>
		/// <returns>true unless the element is custom serialized and ShouldSerialize returns false, or the element is not in a serialized domain model.</returns>
		private static bool ShouldSerializeElement(ModelElement modelElement, bool verifyDomainModelSerialization)
		{
			ICustomSerializedElement customElement = modelElement as ICustomSerializedElement;
			if (customElement == null || customElement.ShouldSerialize())
			{
				return verifyDomainModelSerialization ? typeof(ICustomSerializedDomainModel).IsAssignableFrom(modelElement.GetDomainClass().DomainModel.ImplementationType) : true;
			}
			return false;
		}
		/// <summary>
		/// Get the default prefix for an element from the meta model containing the element
		/// </summary>
		private static string DefaultElementPrefix(ModelElement element)
		{
			string retVal = null;
			ICustomSerializedDomainModel parentModel = GetParentModel(element);
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
		private static void SerializeProperties(XmlWriter file, ModelElement element, ICustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, DomainPropertyInfo property, bool isCustomProperty)
		{
			if (!isCustomProperty)
			{
				if (property.Kind == DomainPropertyKind.Normal)
				{
					file.WriteAttributeString(property.Name, ToXml(element, property));
				}
				return;
			}

			CustomSerializedPropertyInfo customInfo = customElement.GetCustomSerializedPropertyInfo(property, rolePlayedInfo);

			if (property.Kind == DomainPropertyKind.Normal || customInfo.WriteCustomStorage)
			{
				if (customInfo.WriteStyle != CustomSerializedAttributeWriteStyle.Attribute || file.WriteState != WriteState.Element)
				{
					switch (customInfo.WriteStyle)
					{
						default:
							{
								file.WriteElementString
								(
									customInfo.CustomPrefix ?? DefaultElementPrefix(element),
									customInfo.CustomName ?? property.Name,
									customInfo.CustomNamespace,
									ToXml(element, property)
								);
								break;
							}
						case CustomSerializedAttributeWriteStyle.NotWritten:
							{
								break;
							}
						case CustomSerializedAttributeWriteStyle.DoubleTaggedElement:
							{
								string prefix = customInfo.CustomPrefix ?? DefaultElementPrefix(element);
								string name = customInfo.CustomName ?? property.Name;

								file.WriteStartElement
								(
									prefix,
									name,
									customInfo.CustomNamespace
								);
								file.WriteElementString
								(
									prefix,
									customInfo.DoubleTagName ?? name,
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
						customInfo.CustomName ?? property.Name,
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
		private static void SerializeProperties(XmlWriter file, ModelElement element, ICustomSerializedElement customElement, DomainRoleInfo rolePlayedInfo, IList<DomainPropertyInfo> properties, bool hasCustomAttributes)
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
		/// <param name="link">The link. Should be verified with <see cref="ShouldSerializeElement(ModelElement)"/> before this call.</param>
		/// <param name="rolePlayer">The role player. Should be verified with <see cref="ShouldSerializeElement(ModelElement)"/> before this call.</param>
		/// <param name="oppositeRolePlayer">The opposite role player.</param>
		/// <param name="rolePlayedInfo">The role being played.</param>
		private void SerializeLink(XmlWriter file, ElementLink link, ModelElement rolePlayer, ModelElement oppositeRolePlayer, DomainRoleInfo rolePlayedInfo)
		{
			CustomSerializedElementSupportedOperations supportedOperations = CustomSerializedElementSupportedOperations.None;
			CustomSerializedElementInfo customInfo = CustomSerializedElementInfo.Default;
			IList<DomainPropertyInfo> properties = null;
			string defaultPrefix;
			bool hasCustomAttributes = false;

			ICustomSerializedElement rolePlayerCustomElement = rolePlayer as ICustomSerializedElement;
			ICustomSerializedElement customElement = rolePlayerCustomElement;
			CustomSerializedElementWriteStyle writeStyle = CustomSerializedElementWriteStyle.Element;
			CustomSerializedElementInfo oppositeRolePlayerElementInfo = null;
			bool aggregatingLink = false;
			bool standaloneLink = false;
			bool writeContents = customElement != null &&
				0 != (customElement.SupportedCustomSerializedOperations & CustomSerializedElementSupportedOperations.LinkInfo) &&
				((writeStyle = (oppositeRolePlayerElementInfo = customElement.GetCustomSerializedLinkInfo(rolePlayedInfo.OppositeDomainRole, link)).WriteStyle) == CustomSerializedElementWriteStyle.PrimaryLinkElement ||
				(standaloneLink = writeStyle == CustomSerializedElementWriteStyle.PrimaryStandaloneLinkElement) ||
				(aggregatingLink = writeStyle == CustomSerializedElementWriteStyle.EmbeddingLinkElement));

			if (!standaloneLink)
			{
				standaloneLink = writeStyle == CustomSerializedElementWriteStyle.StandaloneLinkElement;
			}

			CustomSerializedStandaloneRelationshipRole[] roles = null;
			if (standaloneLink)
			{
				roles = ((CustomSerializedStandaloneLinkElementInfo)oppositeRolePlayerElementInfo).StandaloneRelationship.GetRoles();
				// UNDONE: This precludes using EmbeddingLinkElement inside a StandaloneLinkfs
				link = (ElementLink)oppositeRolePlayer;
			}
			
			if (writeContents)
			{
				customElement = link as ICustomSerializedElement;
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

				if (0 != (supportedOperations & CustomSerializedElementSupportedOperations.MixedTypedAttributes) && properties != null)
				{
					SortProperties(customElement, rolePlayedInfo, ref properties);
				}
				hasCustomAttributes = (supportedOperations & CustomSerializedElementSupportedOperations.PropertyInfo) != 0;

				if (writeContents)
				{
					if (oppositeRolePlayerElementInfo != null)
					{
						customInfo = oppositeRolePlayerElementInfo;
					}
				}
				else if ((supportedOperations & CustomSerializedElementSupportedOperations.LinkInfo) != 0)
				{
					customInfo = oppositeRolePlayerElementInfo;
					ICustomSerializedDomainModel rolePlayerParentModel;
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
					ICustomSerializedDomainModel rolePlayerParentModel = GetParentModel(rolePlayer);
					if (rolePlayerParentModel != GetParentModel(oppositeRolePlayer) ||
						rolePlayerParentModel != GetParentModel(link))
					return;
				}
#endif // WRITE_ALL_DEFAULT_LINKS
			}
			// UNDONE: Write start element off roleplayer, not link, for standalone primary link element
			if (!WriteCustomizedStartElement(file, customInfo, null, defaultPrefix, standaloneLink ? link.GetDomainClass().Name : string.Concat(rolePlayedInfo.DomainRelationship.Name, ".", rolePlayedInfo.OppositeDomainRole.Name)))
			{
				return;
			}

			if (standaloneLink)
			{
				for (int i = 0; i < roles.Length; ++i)
				{
					CustomSerializedStandaloneRelationshipRole role = roles[i];
					file.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
				}
			}

			Guid keyId = writeContents ? link.Id : oppositeRolePlayer.Id;
			if (writeContents)
			{
				// UNDONE: Write content of the oppositeRolePlayer for the standalone link
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
					CustomSerializedContainerElementInfo[] childElementInfo;
					bool groupRoles;

					childElementInfo = ((groupRoles = (0 != (supportedOperations & CustomSerializedElementSupportedOperations.ChildElementInfo))) ? customElement.GetCustomSerializedChildElementInfo() : null);

					//write children
					SerializeChildElements(file, link, customElement, childElementInfo, rolesPlayed, 0 != (supportedOperations & CustomSerializedElementSupportedOperations.CustomSortChildRoles), groupRoles, defaultPrefix);
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
		private bool SerializeChildElement(XmlWriter file, ModelElement childElement, DomainRoleInfo rolePlayedInfo, DomainRoleInfo oppositeRoleInfo, CustomSerializedElementInfo customInfo, CustomSerializedContainerElementInfo containerInfo, string defaultPrefix, bool writeBeginElement)
		{
			bool ret = false;
			DomainClassInfo lastChildClass = null;
			ICustomSerializedDomainModel parentModel = null;
			// If there class derived from the role player, then the class-level serialization settings may be
			// different than they were on the class specified on the role player, we need to check explicitly,
			// despite the earlier call to ShouldSerializeDomainRole
			bool checkSerializeClass = oppositeRoleInfo.RolePlayer.AllDescendants.Count != 0;
			Store store = myStore;
			bool isAggregate = rolePlayedInfo.IsEmbedding;
			bool oppositeIsAggregate = oppositeRoleInfo.IsEmbedding;
			ICustomSerializedElement testChildInfo;

			if (!isAggregate &&
				(!oppositeIsAggregate ||
				(oppositeIsAggregate &&
				null != (testChildInfo = childElement as ICustomSerializedElement) &&
				0 != (testChildInfo.SupportedCustomSerializedOperations & CustomSerializedElementSupportedOperations.EmbeddingLinkInfo) &&
				testChildInfo.GetCustomSerializedLinkInfo(oppositeRoleInfo, null).WriteStyle == CustomSerializedElementWriteStyle.EmbeddingLinkElement))) //write link
			{
				ReadOnlyCollection<ElementLink> links = rolePlayedInfo.GetElementLinks<ElementLink>(childElement);
				int linksCount = links.Count;
				if (links.Count != 0)
				{
					bool checkSerializeLinkClass = rolePlayedInfo.DomainRelationship.AllDescendants.Count != 0;
					DomainRelationshipInfo lastLinkClass = null;
					ICustomSerializedDomainModel linkParentModel = null;
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

						if (ShouldSerializeElement(link) && ShouldSerializeElement(oppositeRolePlayer))
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

						if (ShouldSerializeElement(child, true))
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
		private void SerializeChildElements(XmlWriter file, ModelElement element, ICustomSerializedElement customElement, CustomSerializedContainerElementInfo[] childElementInfo, IList<DomainRoleInfo> rolesPlayed, bool sortRoles, bool groupRoles, string defaultPrefix)
		{
			int rolesPlayedCount = rolesPlayed.Count;
			ICustomSerializedDomainModel parentModel = GetParentModel(element);

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
				CustomSerializedContainerElementInfo lastCustomChildInfo = null;

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
						CustomSerializedContainerElementInfo customChildInfo = (childIndex >= 0) ? childElementInfo[childIndex] : null;
						string defaultChildPrefix = (customChildInfo != null) ? defaultPrefix : null;
						CustomSerializedContainerElementInfo outerCustomChildInfo = (customChildInfo != null) ? customChildInfo.OuterContainer : null;
						int outerIndex = (outerCustomChildInfo == null) ? -1 : ((IList<CustomSerializedContainerElementInfo>)childElementInfo).IndexOf(outerCustomChildInfo);

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
		private bool SerializeElement(XmlWriter file, ModelElement element, CustomSerializedElementInfo containerCustomInfo, string containerPrefix, SerializeExtraAttributesCallback serializeExtraAttributes, ref string containerName)
		{
			if (!ShouldSerializeElement(element))
			{
				return true;
			}

			// Support fully customized write of element
			IXmlSerializable fullyCustomElement = element as IXmlSerializable;
			if (fullyCustomElement != null)
			{
				//write container begin element
				if (containerName != null)
				{
					if (!WriteCustomizedStartElement(file, containerCustomInfo, null, containerPrefix, containerName))
					{
						return false;
					}
					containerName = null;
				}
				fullyCustomElement.WriteXml(file);
				return true;
			}

			CustomSerializedElementSupportedOperations supportedOperations;
			CustomSerializedContainerElementInfo[] childElementInfo = null;
			DomainClassInfo classInfo = element.GetDomainClass();
			CustomSerializedElementInfo customInfo;
			ICustomSerializedElement customElement = element as ICustomSerializedElement;
			IList<DomainPropertyInfo> properties = classInfo.AllDomainProperties;
			ReadOnlyCollection<DomainRoleInfo> rolesPlayed = classInfo.AllDomainRolesPlayed;
			string defaultPrefix = DefaultElementPrefix(element);
			bool roleGrouping = false;
			bool isCustom = (customElement != null);

			//load custom information
			if (isCustom)
			{
				supportedOperations = customElement.SupportedCustomSerializedOperations;

				if (0 != (supportedOperations & CustomSerializedElementSupportedOperations.MixedTypedAttributes))
				{
					SortProperties(customElement, null, ref properties);
				}
				if (roleGrouping = (0 != (supportedOperations & CustomSerializedElementSupportedOperations.ChildElementInfo)))
				{
					childElementInfo = customElement.GetCustomSerializedChildElementInfo();
				}
				if ((supportedOperations & CustomSerializedElementSupportedOperations.ElementInfo) != 0)
				{
					customInfo = customElement.CustomSerializedElementInfo;
					if (customInfo.WriteStyle == CustomSerializedElementWriteStyle.NotWritten)
					{
						return true;
					}
				}
				else
				{
					customInfo = CustomSerializedElementInfo.Default;
				}
			}
			else
			{
				supportedOperations = CustomSerializedElementSupportedOperations.None;
				customInfo = CustomSerializedElementInfo.Default;
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
				(supportedOperations & CustomSerializedElementSupportedOperations.PropertyInfo) != 0
			);

			//write children
			SerializeChildElements(file, element, customElement, childElementInfo, rolesPlayed, 0 != (supportedOperations & CustomSerializedElementSupportedOperations.CustomSortChildRoles), roleGrouping, defaultPrefix);

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
		#region SaveSerializationContext class
		private sealed class SaveSerializationContext : ISerializationContext, IDisposable
		{
			#region Public initialization
			/// <summary>
			/// Create a serialization context if it is supported by the current store
			/// </summary>
			/// <param name="engine">The parent <see cref="SerializationEngine"/></param>
			/// <returns>A <see cref="ISerializationContext"/> implementation if the host <see cref="Store"/>
			/// supports <see cref="ISerializationContextHost"/>. Otherwise <see langword="null"/></returns>
			public static SaveSerializationContext InitializeContext(SerializationEngine engine)
			{
				ISerializationContextHost host = engine.myStore as ISerializationContextHost;
				return (host != null) ? new SaveSerializationContext(engine, host) : null;
			}
			#endregion // Public initialization
			#region Member variables and constructors
			private SerializationEngine myEngine;
			private ISerializationContextHost myHost;
			private SaveSerializationContext(SerializationEngine engine, ISerializationContextHost host)
			{
				myEngine = engine;
				myHost = host;
				host.SerializationContext = this;
			}
			#endregion // Member variables and constructors
			#region IDisposable Implementation
			void IDisposable.Dispose()
			{
				myHost.SerializationContext = null;
			}
			#endregion // IDisposable Implementation
			#region ISerializationContext Implementation
			string ISerializationContext.GetIdentifierString(Guid id)
			{
				return ToXml(id);
			}
			Guid ISerializationContext.ResolveElementIdentifier(string idValue)
			{
				// Only support during load, not save
				throw new InvalidOperationException();
			}
			ModelElement ISerializationContext.RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference)
			{
				// Only support during load, not save
				throw new InvalidOperationException();
			}
			ModelElement ISerializationContext.RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference, Guid explicitForwardReferenceDomainClassIdentifier)
			{
				// Only support during load, not save
				throw new InvalidOperationException();
			}
			ElementLink ISerializationContext.RealizeElementLink(string idValue, ModelElement rolePlayer, ModelElement oppositeRolePlayer, Guid oppositeDomainRoleInfoId, Guid? explicitDomainRelationshipInfoId)
			{
				// Only support during load, not save
				throw new InvalidOperationException();
			}
			#endregion // ISerializationContext Implementation
		}
		#endregion // LoadSerializationContext class
		/// <summary>
		/// Save the store contents to the provided <see cref="Stream"/>
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
			file.WriteStartElement(GetRootXmlPrefix(), GetRootXmlElementName(), GetRootXmlNamespace());

			//serialize namespaces
			foreach (ICustomSerializedDomainModel ns in Utility.EnumerateDomainModels<ICustomSerializedDomainModel>(values))
			{
				string[,] namespaces = ns.GetCustomElementNamespaces();

				for (int index = 0, count = namespaces.GetLength(0); index < count; ++index)
				{
					//if (/*namespaces[index].Length==2 && */namespaces[index,0] != null && namespaces[index,1] != null)
					file.WriteAttributeString("xmlns", namespaces[index, 0], null, namespaces[index, 1]);
				}
			}

			using (SaveSerializationContext serializationContext = SaveSerializationContext.InitializeContext(this))
			{
				//serialize all root elements
				IElementDirectory elementDir = myStore.ElementDirectory;
				DomainDataDirectory dataDir = myStore.DomainDataDirectory;
				foreach (ICustomSerializedDomainModel ns in Utility.EnumerateDomainModels<ICustomSerializedDomainModel>(values))
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
								if (ns.ShouldSerializeRootElement(element))
								{
									SerializeElement(file, element);
								}
							}
						}
					}
					CustomSerializedRootRelationshipContainer[] rootContainers = ns.GetRootRelationshipContainers();
					if (rootContainers != null)
					{
						Dictionary<ModelElement, ModelElement> linksBySource = null;
						for (int i = 0; i < rootContainers.Length; ++i)
						{
							CustomSerializedRootRelationshipContainer container = rootContainers[i];
							file.WriteStartElement(container.ContainerPrefix, container.ContainerName, container.ContainerNamespace);

							CustomSerializedStandaloneRelationship[] relationships = container.GetRelationshipClasses();
							for (int j = 0; j < relationships.Length; ++j)
							{
								CustomSerializedStandaloneRelationship relationship = relationships[j];
								DomainRelationshipInfo relationshipInfo = dataDir.GetDomainClass(relationship.DomainClassId) as DomainRelationshipInfo;
								ReadOnlyCollection<ModelElement> allRelationshipElements = elementDir.FindElements(relationshipInfo);

								// This collection is very randomly order. To maintain order within the relationship, find
								// all of the unique source elements and requery to get the ordered targets for that element.
								// UNDONE: Do we want to specify if we should key off the source, target, or not at all in
								// the serialization specs?
								int allElementCount = allRelationshipElements.Count;
								if (allElementCount == 0)
								{
									continue;
								}
								if (linksBySource == null)
								{
									linksBySource = new Dictionary<ModelElement, ModelElement>();
								}
								else
								{
									linksBySource.Clear();
								}
								for (int k = 0; k < allElementCount; ++k)
								{
									ModelElement sourceElement = DomainRoleInfo.GetSourceRolePlayer((ElementLink)allRelationshipElements[k]);
									if (!linksBySource.ContainsKey(sourceElement))
									{
										linksBySource[sourceElement] = sourceElement;
									}
								}
								IList<DomainRoleInfo> roleInfos = relationshipInfo.DomainRoles;
								DomainRoleInfo sourceRoleInfo = roleInfos[0];
								if (!sourceRoleInfo.IsSource)
								{
									sourceRoleInfo = roleInfos[1];
									Debug.Assert(sourceRoleInfo.IsSource);
								}
								CustomSerializedStandaloneRelationshipRole[] customRelationshipRoles = relationship.GetRoles();

								// The values collection is officially randomly ordered and may change over time.
								// Sort the elements by name to get a more stable XML file over time.
								// Note that doing a guid comparison here is also theoretically stable,
								// but wreaks havoc with unit test baselines, so we sort by name instead.
								ICollection<ModelElement> sourceValues = linksBySource.Values;
								int sourceCount = sourceValues.Count;
								DomainPropertyInfo nameDomainProperty;
								if (sourceCount > 1 &&
									null != (nameDomainProperty = sourceRoleInfo.RolePlayer.NameDomainProperty))
								{
									ModelElement[] sortedSources = new ModelElement[sourceCount];
									sourceValues.CopyTo(sortedSources, 0);
									Array.Sort<ModelElement>(
										sortedSources,
										delegate(ModelElement x, ModelElement y)
										{
											if ((object)x == (object)y)
											{
												return 0;
											}
											// If the string comparison is equal then we could do a guid
											// comparison, but this ends up with different element orders
											// when attempting to write baselined unit tests. Always choosing
											// the current order (return -1) works until VS2012, which appears
											// to have modified the sort algorithm and makes this call with the same
											// two elements in different orders, resulting in a failed sort. So, we
											// just let the zero return and leave it to the sort algorithm to produce
											// a consistent order instead of doing a secondary sort.
											return string.Compare(nameDomainProperty.GetValue(x) as string, nameDomainProperty.GetValue(y) as string, StringComparison.Ordinal);
										});
									sourceValues = sortedSources;
								}
								foreach (ModelElement element in sourceValues)
								{
									ReadOnlyCollection<ElementLink> relationshipElements = sourceRoleInfo.GetElementLinks(element);
									int linkCount = relationshipElements.Count;
									for (int k = 0; k < linkCount; ++k)
									{
										ElementLink link = relationshipElements[k] as ElementLink;
										if (ns.ShouldSerializeRootElement(link))
										{
											int testSerialize = 0;
											for (; testSerialize < customRelationshipRoles.Length; ++testSerialize)
											{
												if (!ShouldSerializeElement(DomainRoleInfo.GetRolePlayer(link, customRelationshipRoles[testSerialize].DomainRoleId)))
												{
													break;
												}
											}
											if (testSerialize != customRelationshipRoles.Length)
											{
												continue;
											}
											if (relationship.IsPrimaryLinkElement)
											{
												SerializeElement(file, link, new SerializeExtraAttributesCallback(delegate(XmlWriter xmlFile)
												{
													for (int l = 0; l < customRelationshipRoles.Length; ++l)
													{
														CustomSerializedStandaloneRelationshipRole role = customRelationshipRoles[l];
														xmlFile.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
													}
												}));
											}
											else
											{
												file.WriteStartElement(relationship.ElementPrefix, relationship.ElementName, relationship.ElementNamespace);
												for (int l = 0; l < customRelationshipRoles.Length; ++l)
												{
													CustomSerializedStandaloneRelationshipRole role = customRelationshipRoles[l];
													file.WriteAttributeString(role.AttributeName, ToXml(DomainRoleInfo.GetRolePlayer(link, role.DomainRoleId).Id));
												}
												file.WriteEndElement();
											}
										}
									}
								}
							}
							file.WriteEndElement();
						}
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
	public partial class SerializationEngine
	{
		#region DeserializationFixupManager class
		/// <summary>
		/// A class to manage post-deserialization model fixup
		/// </summary>
		private class DeserializationFixupManager : INotifyElementAdded
		{
			#region Member Variables
			private Store myStore;
			private List<IDeserializationFixupListener> myListeners;
			private int[] myPhases;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// A class to manage the complex process of post-deserialization
			/// model fixup. Generally, rules are suspended during loading as
			/// it is difficult to enforce any complex rules when only portions
			/// off the model are in place. This means that there is no guarantee
			/// after a load sequence that the model is in workable valid state.
			/// The invalid state can occur because certain model elements are not
			/// serialized, or because of the Notepad factor (edits to the model file
			/// outside a sanctioned editor). Post-deserialization fixups allow different
			/// elements in the model to bring the model up to a consistent state
			/// so that all subsequent edits run against a model in a known state.
			/// </summary>
			/// <param name="store">The store being deserialized to.</param>
			public DeserializationFixupManager(Store store)
			{
				myStore = store;
				List<IDeserializationFixupListener> listeners = new List<IDeserializationFixupListener>();
				Dictionary<int, int> allPhases = new Dictionary<int, int>();
				myListeners = listeners;
				int[] phases;
				foreach (IDeserializationFixupListenerProvider provider in Utility.EnumerateDomainModels<IDeserializationFixupListenerProvider>(store.DomainModels))
				{
					foreach (IDeserializationFixupListener listener in provider.DeserializationFixupListenerCollection)
					{
						listeners.Add(listener);
					}
					Type phaseType = provider.DeserializationFixupPhaseType;
					if (phaseType != null)
					{
						phases = (int[])Enum.GetValues(phaseType);
						for (int i = 0; i < phases.Length; ++i)
						{
							// The keyed insertion keeps the values unique
							allPhases[phases[i]] = phases[i];
						}
					}
				}
				int phaseCount = allPhases.Count;
				phases = new int[phaseCount];
				allPhases.Values.CopyTo(phases, 0);
				Array.Sort<int>(phases);
				myPhases = phases;
			}
			#endregion // Constructors
			#region INotifyElementAdded Implementation
			/// <summary>
			/// Note that this matches the signature for the
			/// System.VisualStudio.Modeling.Diagnostics.XmlSerialization.Deserialized
			/// delegate, so can be used as the callback point for the
			/// Microsoft-provided IMS deserialization engine.
			/// </summary>
			/// <param name="element">The newly added element</param>
			void INotifyElementAdded.ElementAdded(ModelElement element)
			{
				ElementAdded(element);
			}
			/// <summary>
			/// Implements INotifyElementAdded.ElementAdded(ModelElement).
			/// </summary>
			/// <param name="element">The newly added element</param>
			protected void ElementAdded(ModelElement element)
			{
				List<IDeserializationFixupListener> listeners = myListeners;
				int listenerCount = listeners.Count;
				for (int i = 0; i < listenerCount; ++i)
				{
					listeners[i].ElementAdded(element);
				}
			}
			/// <summary>
			/// Implements INotifyElementAdded.ElementAdded(ModelElement, bool)
			/// </summary>
			/// <param name="element">The newly added element</param>
			/// <param name="addLinks">true if all links attached directly to the
			/// element should also be added. Defaults to false.</param>
			protected void ElementAdded(ModelElement element, bool addLinks)
			{
				// Call through the interface to support overrides
				INotifyElementAdded notify = this;
				notify.ElementAdded(element);
				if (addLinks)
				{
					ReadOnlyCollection<ElementLink> links = DomainRoleInfo.GetAllElementLinks(element);
					int linkCount = links.Count;
					for (int i = 0; i < linkCount; ++i)
					{
						notify.ElementAdded(links[i]);
					}
				}
			}
			void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
			{
				ElementAdded(element, addLinks);
			}
			#endregion // INotifyElementAdded Implementation
			#region DeserializationFixupManager specific
			/// <summary>
			/// Deserialization has been completed. Proceed with the
			/// fixup process.
			/// </summary>
			public virtual void DeserializationComplete()
			{
				int[] phases = myPhases;
				int phaseCount = phases.Length;
				List<IDeserializationFixupListener> listeners = myListeners;
				int listenerCount = listeners.Count;
				Store store = myStore;
				for (int phaseIndex = 0; phaseIndex < phaseCount; ++phaseIndex)
				{
					int phase = phases[phaseIndex];
					bool phaseComplete = false;
					// Process elements on the current phase until HasElements
					// returns false for all listeners in the phase.
					while (!phaseComplete)
					{
						phaseComplete = true;
						for (int i = 0; i < listenerCount; ++i)
						{
							IDeserializationFixupListener listener = listeners[i];
							if (listener.HasElements(phase, store))
							{
								phaseComplete = false;
								listener.ProcessElements(phase, store, this);
							}
						}
					}
					for (int i = 0; i < listenerCount; ++i)
					{
						listeners[i].PhaseCompleted(phase, store);
					}
				}
#if DEBUG
				// Walk through one more time and assert if elements
				// were added to the listener for any phase after
				// it was completed.
				for (int phaseIndex = 0; phaseIndex < phaseCount; ++phaseIndex)
				{
					int phase = phases[phaseIndex];
					for (int i = 0; i < listenerCount; ++i)
					{
						IDeserializationFixupListener listener = listeners[i];
						if (listener.HasElements(phase, store))
						{
							Debug.Fail(string.Format(CultureInfo.InvariantCulture, "A fixup phase after phase {0} added elements to an IDeserializationFixupListener of type {1}.", phase, listener.GetType().FullName));
						}
					}
				}
#endif // DEBUG
			}
			#endregion // DeserializationFixupManager specific
		}
		#endregion // DeserializationFixupManager class
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
		private Dictionary<string, ICustomSerializedDomainModel> myXmlNamespaceToModelMap;

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
		public SerializationEngine(Store store)
		{
			myStore = store;
		}
		#endregion // Constructor
		#region LoadSerializationContext class
		private sealed class LoadSerializationContext : ISerializationContext, IDisposable
		{
			#region Public initialization
			/// <summary>
			/// Create a serialization context if it is supported by the current store
			/// </summary>
			/// <param name="engine">The parent <see cref="SerializationEngine"/></param>
			/// <returns>A <see cref="ISerializationContext"/> implementation if the host <see cref="Store"/>
			/// supports <see cref="ISerializationContextHost"/>. Otherwise <see langword="null"/></returns>
			public static LoadSerializationContext InitializeContext(SerializationEngine engine)
			{
				ISerializationContextHost host = engine.myStore as ISerializationContextHost;
				return (host != null) ? new LoadSerializationContext(engine, host) : null;
			}
			#endregion // Public initialization
			#region Member variables and constructors
			private SerializationEngine myEngine;
			private ISerializationContextHost myHost;
			private LoadSerializationContext(SerializationEngine engine, ISerializationContextHost host)
			{
				myEngine = engine;
				myHost = host;
				host.SerializationContext = this;
			}
			#endregion // Member variables and constructors
			#region IDisposable Implementation
			void IDisposable.Dispose()
			{
				myHost.SerializationContext = null;
			}
			#endregion // IDisposable Implementation
			#region ISerializationContext Implementation
			string ISerializationContext.GetIdentifierString(Guid id)
			{
				return ToXml(id);
			}
			Guid ISerializationContext.ResolveElementIdentifier(string idValue)
			{
				return myEngine.GetElementId(idValue);
			}
			ModelElement ISerializationContext.RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference)
			{
				return ((ISerializationContext)this).RealizeElement(idValue, domainClassIdentifier, isReference, Guid.Empty);
			}
			ModelElement ISerializationContext.RealizeElement(string idValue, Guid domainClassIdentifier, bool isReference, Guid explicitForwardReferenceDomainClassIdentifier)
			{
				SerializationEngine engine = myEngine;
				Store store = engine.myStore;
				bool createAsPlaceHolder = false;
				DomainClassInfo classInfo = null;
				if (isReference)
				{
					classInfo = store.DomainDataDirectory.GetDomainClass(domainClassIdentifier);
					if (classInfo.LocalDescendants.Count != 0)
					{
						createAsPlaceHolder = true;
					}
				}
				bool isNewElementDummy;
				return myEngine.CreateElement(idValue, classInfo, domainClassIdentifier, explicitForwardReferenceDomainClassIdentifier, createAsPlaceHolder, out isNewElementDummy);
			}
			ElementLink ISerializationContext.RealizeElementLink(string idValue, ModelElement rolePlayer, ModelElement oppositeRolePlayer, Guid oppositeDomainRoleInfoId, Guid? explicitDomainRelationshipInfoId)
			{
				SerializationEngine engine = myEngine;
				DomainDataDirectory dataDirectory = engine.myStore.DomainDataDirectory;
				return engine.CreateElementLink(
					idValue,
					rolePlayer,
					oppositeRolePlayer,
					dataDirectory.FindDomainRole(oppositeDomainRoleInfoId),
					explicitDomainRelationshipInfoId.HasValue ? dataDirectory.FindDomainRelationship(explicitDomainRelationshipInfoId.Value) : null);

			}
			#endregion // ISerializationContext Implementation
		}
		#endregion // LoadSerializationContext class
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		public void Load(Stream stream)
		{
			Load(stream, SerializationEngineLoadOptions.None);
		}
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		/// <param name="options">Options to modify load behavior. Defaults to <see cref="SerializationEngineLoadOptions.None"/></param>
		public void Load(Stream stream, SerializationEngineLoadOptions options)
		{
			DeserializationFixupManager fixupManager = new DeserializationFixupManager(myStore);
			myNotifyAdded = fixupManager as INotifyElementAdded;
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSchemaSet schemas = settings.Schemas;
			Type schemaResourcePathType = GetType();
			using (Stream schemaStream = schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, GetRootSchemaFileName()))
			{
				using (XmlReader reader = new XmlTextReader(schemaStream))
				{
					schemas.Add(GetRootXmlNamespace(), reader);
				}
			}

			// Extract namespace and schema information from the different meta models
			ICollection<DomainModel> domainModels = myStore.DomainModels;
			Dictionary<string, ICustomSerializedDomainModel> namespaceToModelMap = new Dictionary<string, ICustomSerializedDomainModel>();
			foreach (ICustomSerializedDomainModel customSerializedDomainModel in Utility.EnumerateDomainModels<ICustomSerializedDomainModel>(domainModels))
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
						using (Stream schemaStream = schemaResourcePathType.Assembly.GetManifestResourceStream(schemaResourcePathType, schemaFile))
						{
							using (XmlReader reader = new XmlTextReader(schemaStream))
							{
								schemas.Add(namespaceURI, reader);
							}
						}
					}
				}
			}

			ISkipExtensions skippingStore;
			IList<Type> skippedExtensionTypes;
			int skippedExtensionCount;
			if (0 != (options & SerializationEngineLoadOptions.ResolveSkippedExtensions) &&
				null != (skippingStore = myStore as ISkipExtensions) &&
				null != (skippedExtensionTypes = skippingStore.SkippedExtensionTypes) &&
				0 != (skippedExtensionCount = skippedExtensionTypes.Count))
			{
				for (int i = 0; i < skippedExtensionCount; ++i)
				{
					Type skippedType = skippedExtensionTypes[i];
					object[] serializedSchemas;
					int schemaCount;
					if (typeof(ICustomSerializedDomainModel).IsAssignableFrom(skippedType) &&
						null != (serializedSchemas = skippedType.GetCustomAttributes(typeof(CustomSerializedXmlSchemaAttribute), false)) &&
						0 != (schemaCount = serializedSchemas.Length))
					{
						for (int iSchema = 0; iSchema < schemaCount; ++iSchema)
						{
							CustomSerializedXmlSchemaAttribute schemaAttribute = (CustomSerializedXmlSchemaAttribute)serializedSchemas[iSchema];
							using (Stream schemaStream = skippedType.Assembly.GetManifestResourceStream(skippedType, schemaAttribute.SchemaFile))
							{
								using (XmlReader reader = new XmlTextReader(schemaStream))
								{
									schemas.Add(schemaAttribute.XmlNamespace, reader);
								}
							}
						}
					}
				}
			}

			myXmlNamespaceToModelMap = namespaceToModelMap;
			NameTable nameTable = new NameTable();
			settings.NameTable = nameTable;

			if (0 == (options & SerializationEngineLoadOptions.SkipSchemaValidation))
			{
				settings.ValidationType = ValidationType.Schema;
			}

			using (LoadSerializationContext context = LoadSerializationContext.InitializeContext(this))
			{
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
									if (!reader.IsEmptyElement && reader.NamespaceURI == GetRootXmlNamespace() && reader.LocalName == GetRootXmlElementName())
									{
										while (reader.Read())
										{
											XmlNodeType nodeType = reader.NodeType;
											if (nodeType == XmlNodeType.Element)
											{
												bool processedRootElement = false;
												ICustomSerializedDomainModel metaModel;
												if (namespaceToModelMap.TryGetValue(reader.NamespaceURI, out metaModel))
												{
													string id = reader.GetAttribute("id");
													if (id != null)
													{
														Guid classGuid = metaModel.MapRootElement(reader.NamespaceURI, reader.LocalName);
														if (!classGuid.Equals(Guid.Empty))
														{
															processedRootElement = true;
															ProcessClassElement(reader, metaModel, CreateElement(id, null, classGuid), null, null);
														}
													}
													else if (!reader.IsEmptyElement)
													{
														CustomSerializedRootRelationshipContainer[] containers = metaModel.GetRootRelationshipContainers();
														if (containers != null)
														{
															CustomSerializedRootRelationshipContainer? container = CustomSerializedRootRelationshipContainer.Find(containers, reader.LocalName, reader.NamespaceURI);
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

																		CustomSerializedStandaloneRelationship[] relationships = container.Value.GetRelationshipClasses();
																		DomainRelationshipInfo relationshipInfo = null;
																		CustomSerializedStandaloneRelationship? relationship = CustomSerializedStandaloneRelationship.Find(relationships, linkName, linkNamespace);
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
																			(relationship = CustomSerializedStandaloneRelationship.Find(relationships, relationshipInfo.Id)).HasValue)
																		{
																			CustomSerializedStandaloneRelationshipRole[] roles = relationship.Value.GetRoles();
																			if (roles.Length == 2)
																			{
																				string[] rolePlayerIds = new string[2];
																				DomainClassInfo[] rolePlayerDomainClasses = new DomainClassInfo[2];
																				DomainRoleInfo oppositeRoleInfo = null;
																				for (int i = 0; i < 2; ++i)
																				{
																					CustomSerializedStandaloneRelationshipRole currentRole = roles[i];
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
																								Guid.Empty,
																								rolePlayerDomainClasses[0].AllDescendants.Count == 0,
																								out isNewElementDummy),
																							CreateElement(
																								rolePlayerIds[1],
																								rolePlayerDomainClasses[1],
																								Guid.Empty,
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
		private void ProcessClassElement(XmlReader reader, ICustomSerializedDomainModel customModel, ModelElement element, CreateAggregatingLink createAggregatingLinkCallback, ShouldProcessAttribute shouldProcessAttributeCallback)
		{
			#region IXmlSerializable processing
			IXmlSerializable fullyCustomElement = element as IXmlSerializable;
			if (fullyCustomElement != null)
			{
				using (XmlReader subtreeReader = reader.ReadSubtree())
				{
					fullyCustomElement.ReadXml(subtreeReader);
				}
				return;
			}
			#endregion // IXmlSerializable processing
			ICustomSerializedElement customElement = element as ICustomSerializedElement;
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
		private void ProcessChildElements(XmlReader reader, ICustomSerializedDomainModel customModel, ModelElement element, ICustomSerializedElement customElement, CreateAggregatingLink createAggregatingLinkCallback)
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
			bool testForAggregatingLink = createAggregatingLinkCallback != null && customElement != null && 0 != (customElement.SupportedCustomSerializedOperations & CustomSerializedElementSupportedOperations.EmbeddingLinkInfo);
			ICustomSerializedDomainModel containerRestoreCustomModel = null;
			DomainRoleInfo containerOppositeDomainRole = null;
			ICustomSerializedDomainModel outerContainerRestoreCustomModel = null;
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
					Guid explicitForwardReferenceTypeId = Guid.Empty;
					bool oppositeDomainClassFullyDeterministic = false;
					bool resolveOppositeDomainClass = false;
					bool allowDuplicates = false;
					IList<Guid> oppositeDomainRoleIds = null;
					ICustomSerializedDomainModel restoreCustomModel = null;
					bool nodeProcessed = false;
					CustomSerializedElementMatch aggregatingLinkMatch;
					CustomSerializedStandaloneRelationship? standaloneRelationship = null;
					DomainRoleInfo testAggregatingRole;
					if (aggregatedClass &&
						testForAggregatingLink &&
						(aggregatingLinkMatch = customElement.MapChildElement(namespaceName, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName)).MatchStyle == CustomSerializedElementMatchStyle.SingleOppositeDomainRole &&
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
							CustomSerializedElementMatch match = default(CustomSerializedElementMatch);
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
								case CustomSerializedElementMatchStyle.SingleOppositeDomainRole:
									oppositeDomainRole = dataDir.FindDomainRole(match.SingleOppositeDomainRoleId);
									explicitForwardReferenceTypeId = match.SingleForwardReferenceTypeId;
									resolveOppositeDomainClass = true;
									break;
								case CustomSerializedElementMatchStyle.SingleOppositeDomainRoleExplicitRelationshipType:
									explicitRelationshipType = dataDir.FindDomainRelationship(match.ExplicitRelationshipGuid);
									oppositeDomainRole = dataDir.FindDomainRole(match.SingleOppositeDomainRoleId);
									resolveOppositeDomainClass = true;
									break;
								case CustomSerializedElementMatchStyle.MultipleOppositeDomainRoles:
									oppositeDomainRoleIds = match.OppositeDomainRoleIdCollection;
									break;
								case CustomSerializedElementMatchStyle.MultipleOppositeMetaRolesExplicitRelationshipType:
									explicitRelationshipType = dataDir.FindDomainRelationship(match.ExplicitRelationshipGuid);
									oppositeDomainRoleIds = match.OppositeDomainRoleIdCollection;
									break;
								case CustomSerializedElementMatchStyle.Property:
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
								case CustomSerializedElementMatchStyle.None:
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
								ICustomSerializedDomainModel childModel;
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
								ICustomSerializedDomainModel childModel;
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
									ICustomSerializedDomainModel elementModel = myXmlNamespaceToModelMap[namespaceName];
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
								oppositeElement = CreateElement(elementId, oppositeDomainClass, Guid.Empty, explicitForwardReferenceTypeId, !oppositeDomainClassFullyDeterministic, out isNewElement);
							}
							else
							{
								CustomSerializedStandaloneRelationship relationship = standaloneRelationship.Value;
								DomainRelationshipInfo standaloneRelationshipInfo = dataDir.FindDomainRelationship(standaloneRelationship.Value.DomainClassId);
								CustomSerializedStandaloneRelationshipRole[] roles = relationship.GetRoles();
								if (roles.Length == 2)
								{
									string[] rolePlayerIds = new string[2];
									DomainClassInfo[] rolePlayerDomainClasses = new DomainClassInfo[2];
									DomainRoleInfo oppositeRoleInfo = null;
									for (int i = 0; i < 2; ++i)
									{
										CustomSerializedStandaloneRelationshipRole currentRole = roles[i];
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
													explicitForwardReferenceTypeId,
													rolePlayerDomainClasses[0].AllDescendants.Count == 0,
													out isNewElementDummy),
												CreateElement(
													rolePlayerIds[1],
													rolePlayerDomainClasses[1],
													Guid.Empty,
													explicitForwardReferenceTypeId,
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
											PassEndElement(reader);
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
					Debug.Fail(string.Format(
						"Could not deserialize property.\r\nProperty Name: {0}\r\nSerialized Value: {1}\r\nProperty Type: {2} (in {3})\r\nConverter Type: {4} (in {5})",
						domainPropertyInfo.Name,
						stringValue,
						domainPropertyInfo.PropertyType.AssemblyQualifiedName,
						domainPropertyInfo.PropertyType.Assembly.Location,
						converter.GetType().AssemblyQualifiedName,
						converter.GetType().Assembly.Location));
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
			return CreateElement(idValue, domainClassInfo, domainClassId, Guid.Empty, false, out isNewElement);
		}
		/// <summary>
		/// Create a class element with the id specified in the reader
		/// </summary>
		/// <param name="idValue">The id for this element in the xml file</param>
		/// <param name="domainClassInfo">The meta class info of the element to create. If null,
		/// the domainClassId is used to find the class info</param>
		/// <param name="domainClassId">The identifier for the class</param>
		/// <param name="explicitForwardReferenceDomainClassId">The identifier for a class derived from
		/// the specified domain class that should be created for a placeholder instead of the specified
		/// domain class. This allowed forward referencing without adverse effects on the load order of
		/// top-level elements that are forward referenced.</param>
		/// <param name="createAsPlaceholder">The provided meta class information is not unique.
		/// If this element is not already created then add it with a separate tracked id so it can
		/// be replaced later by the fully resolved type. All role players will be automatically
		/// moved from the pending placeholder when the real element is created</param>
		/// <param name="isNewElement">true if the element is actually created, as opposed
		/// to being identified as an existing element</param>
		/// <returns>A new ModelElement</returns>
		private ModelElement CreateElement(string idValue, DomainClassInfo domainClassInfo, Guid domainClassId, Guid explicitForwardReferenceDomainClassId, bool createAsPlaceholder, out bool isNewElement)
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
					if (explicitForwardReferenceDomainClassId != Guid.Empty)
					{
						domainClassInfo = myStore.DomainDataDirectory.GetDomainClass(explicitForwardReferenceDomainClassId);
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
