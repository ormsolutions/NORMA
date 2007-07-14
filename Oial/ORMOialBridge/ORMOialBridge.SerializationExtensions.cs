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

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	#region ORMToORMAbstractionBridgeDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction")]
	partial class ORMToORMAbstractionBridgeDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMToORMAbstractionBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormtooial";
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
			ret[0, 0] = "ormtooial";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
			ret[0, 2] = "ORMToORMAbstraction.xsd";
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
			CustomSerializedRootRelationshipContainer[] retVal = ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new CustomSerializedRootRelationshipContainer[]{
					new CustomSerializedRootRelationshipContainer("ormtooial", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction", new CustomSerializedStandaloneRelationship[]{
						new CustomSerializedStandaloneRelationship(AbstractionModelIsForORMModel.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("AbstractionModel", AbstractionModelIsForORMModel.AbstractionModelDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ORMModel", AbstractionModelIsForORMModel.ORMModelDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(FactTypeMapsTowardsRole.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("FactType", FactTypeMapsTowardsRole.FactTypeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("TowardsRole", FactTypeMapsTowardsRole.TowardsRoleDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ConceptTypeIsForObjectType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("ConceptType", ConceptTypeIsForObjectType.ConceptTypeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ObjectType", ConceptTypeIsForObjectType.ObjectTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ConceptTypeChildHasPathFactType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", ConceptTypeChildHasPathFactType.ConceptTypeChildDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("PathFactType", ConceptTypeChildHasPathFactType.PathFactTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(InformationTypeFormatIsForValueType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("InformationTypeFormat", InformationTypeFormatIsForValueType.InformationTypeFormatDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ValueType", InformationTypeFormatIsForValueType.ValueTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(UniquenessIsForUniquenessConstraint.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("AbstractionUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ORMUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessConstraintDomainRoleId)}, null, null, null)})};
				ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers = retVal;
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
			Collection<string> validNamespaces = ORMToORMAbstractionBridgeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMToORMAbstractionBridgeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction");
				ORMToORMAbstractionBridgeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				ORMToORMAbstractionBridgeDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ORMToORMAbstractionBridgeDomainModel model serialization
}
