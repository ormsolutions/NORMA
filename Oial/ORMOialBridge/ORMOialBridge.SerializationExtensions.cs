using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.ObjectModel;

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
	partial class ORMToORMAbstractionBridgeDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMToORMAbstractionBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
		/// <summary>Implements IORMCustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormtooil";
			}
		}
		string IORMCustomSerializedDomainModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetCustomElementNamespaces</summary>
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "ormtooil";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
			ret[0, 2] = "ORMToORMAbstraction.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		private static ORMCustomSerializedRootRelationshipContainer[] myRootRelationshipContainers;
		/// <summary>Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		bool IORMCustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[0];
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static ORMCustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			ORMCustomSerializedRootRelationshipContainer[] retVal = ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new ORMCustomSerializedRootRelationshipContainer[]{
					new ORMCustomSerializedRootRelationshipContainer("ormtooil", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction", new ORMCustomSerializedStandaloneRelationship[]{
						new ORMCustomSerializedStandaloneRelationship(AbstractionModelIsForORMModel.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("AbstractionModel", AbstractionModelIsForORMModel.AbstractionModelDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("ORMModel", AbstractionModelIsForORMModel.ORMModelDomainRoleId)}, null, null, null),
						new ORMCustomSerializedStandaloneRelationship(FactTypeMapsTowardsRole.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("FactType", FactTypeMapsTowardsRole.FactTypeDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("TowardsRole", FactTypeMapsTowardsRole.TowardsRoleDomainRoleId)}, null, null, null),
						new ORMCustomSerializedStandaloneRelationship(ConceptTypeIsForObjectType.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("ConceptType", ConceptTypeIsForObjectType.ConceptTypeDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("ObjectType", ConceptTypeIsForObjectType.ObjectTypeDomainRoleId)}, null, null, null),
						new ORMCustomSerializedStandaloneRelationship(ConceptTypeChildHasPathFactType.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("ConceptTypeChild", ConceptTypeChildHasPathFactType.ConceptTypeChildDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("PathFactType", ConceptTypeChildHasPathFactType.PathFactTypeDomainRoleId)}, null, null, null),
						new ORMCustomSerializedStandaloneRelationship(InformationTypeFormatIsForValueType.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("InformationTypeFormat", InformationTypeFormatIsForValueType.InformationTypeFormatDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("ValueType", InformationTypeFormatIsForValueType.ValueTypeDomainRoleId)}, null, null, null),
						new ORMCustomSerializedStandaloneRelationship(UniquenessIsForUniquenessConstraint.DomainClassId, new ORMCustomSerializedStandaloneRelationshipRole[]{
							new ORMCustomSerializedStandaloneRelationshipRole("AbstractionUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessDomainRoleId),
							new ORMCustomSerializedStandaloneRelationshipRole("ORMUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessConstraintDomainRoleId)}, null, null, null)})};
				ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers = retVal;
			}
			return retVal;
		}
		ORMCustomSerializedRootRelationshipContainer[] IORMCustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapClassName</summary>
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
		Guid IORMCustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // ORMToORMAbstractionBridgeDomainModel model serialization
}
