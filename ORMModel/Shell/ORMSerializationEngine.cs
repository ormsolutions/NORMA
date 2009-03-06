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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region ORMSerializationEngine class
	/// <summary>
	/// Serialize with ORMRoot element semantics
	/// </summary>
	public class ORMSerializationEngine : SerializationEngine
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
		#region Constructor
		/// <summary>
		/// Create a serializer on the given store
		/// </summary>
		/// <param name="store">Store instance</param>
		public ORMSerializationEngine(Store store)
			: base(store)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Overrides <see cref="SerializationEngine.GetRootXmlPrefix"/>
		/// </summary>
		protected override string GetRootXmlPrefix()
		{
			return RootXmlPrefix;
		}
		/// <summary>
		/// Overrides <see cref="SerializationEngine.GetRootXmlElementName"/>
		/// </summary>
		protected override string GetRootXmlElementName()
		{
			return RootXmlElementName;
		}
		/// <summary>
		/// Overrides <see cref="SerializationEngine.GetRootXmlNamespace"/>
		/// </summary>
		protected override string GetRootXmlNamespace()
		{
			return RootXmlNamespace;
		}
		/// <summary>
		/// Overrides <see cref="SerializationEngine.GetRootSchemaFileName"/>
		/// </summary>
		/// <returns></returns>
		protected override string GetRootSchemaFileName()
		{
			return "ORM2Root.xsd";
		}
		#endregion // Base overrides
	}
	#endregion // ORMSerializationEngine class
}
