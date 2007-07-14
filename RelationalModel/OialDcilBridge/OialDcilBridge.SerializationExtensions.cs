using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Shell;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	#region ORMAbstractionToConceptualDatabaseBridgeDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase")]
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMAbstractionToConceptualDatabaseBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oialtocdb";
			}
		}
		string ICustomSerializedDomainModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetCustomElementNamespaces</summary>
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "oialtocdb";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase";
			ret[0, 2] = "ORMAbstractionToConceptualDatabase.xsd";
			return ret;
		}
		string[,] ICustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		private static CustomSerializedRootRelationshipContainer[] myRootRelationshipContainers;
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		bool ICustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[0];
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static CustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			CustomSerializedRootRelationshipContainer[] retVal = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new CustomSerializedRootRelationshipContainer[]{
					new CustomSerializedRootRelationshipContainer("oialtocdb", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase", new CustomSerializedStandaloneRelationship[]{
						new CustomSerializedStandaloneRelationship(SchemaIsForAbstractionModel.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Schema", SchemaIsForAbstractionModel.SchemaDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("AbstractionModel", SchemaIsForAbstractionModel.AbstractionModelDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ColumnHasConceptTypeChild.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Column", ColumnHasConceptTypeChild.ColumnDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", ColumnHasConceptTypeChild.ConceptTypeChildDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(DomainIsForInformationTypeFormat.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Domain", DomainIsForInformationTypeFormat.DomainDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("InformationTypeFormat", DomainIsForInformationTypeFormat.InformationTypeFormatDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(TableIsPrimarilyForConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Table", TableIsPrimarilyForConceptType.TableDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", TableIsPrimarilyForConceptType.ConceptTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(TableIsAlsoForConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Table", TableIsAlsoForConceptType.TableDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", TableIsAlsoForConceptType.ConceptTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(UniquenessConstraintIsForUniqueness.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("UniquenessConstraint", UniquenessConstraintIsForUniqueness.UniquenessConstraintDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", UniquenessConstraintIsForUniqueness.ConceptTypeChildDomainRoleId)}, null, null, null)})};
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myRootRelationshipContainers = retVal;
			}
			return retVal;
		}
		CustomSerializedRootRelationshipContainer[] ICustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			return default(Guid);
		}
		Guid ICustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapClassName</summary>
		protected static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase");
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myClassNameMap = classNameMap;
			}
			if (validNamespaces.Contains(xmlNamespace) && classNameMap.ContainsKey(elementName))
			{
				return classNameMap[elementName];
			}
			return default(Guid);
		}
		Guid ICustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // ORMAbstractionToConceptualDatabaseBridgeDomainModel model serialization
}
