﻿#region Common Public License Copyright Notice
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
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
namespace ORMSolutions.ORMArchitect.ExtensionExample
{
	/// <summary>
	/// DomainModel ExtensionDomainModel
	/// The extension created for testing purposes
	/// </summary>
	[DslModeling::ExtendsDomainModel("3EAE649F-E654-4D04-8289-C25D2C0322D8"/*ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel*/)]
	[DslDesign::DisplayNameResource("ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel.DisplayName", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel.Description", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainObjectId("9f620b5a-9a99-45a4-a022-c9ed95ce85d6")]
	public partial class ExtensionDomainModel : DslModeling::DomainModel
	{
		#region Constructor, domain model Id
	
		/// <summary>
		/// ExtensionDomainModel domain model Id.
		/// </summary>
		public static readonly global::System.Guid DomainModelId = new global::System.Guid(0x9f620b5a, 0x9a99, 0x45a4, 0xa0, 0x22, 0xc9, 0xed, 0x95, 0xce, 0x85, 0xd6);
	
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="store">Store containing the domain model.</param>
		public ExtensionDomainModel(DslModeling::Store store)
			: base(store, DomainModelId)
		{
		}
		
		#endregion
		#region Domain model reflection
			
		/// <summary>
		/// Gets the list of generated domain model types (classes, rules, relationships).
		/// </summary>
		/// <returns>List of types.</returns>
		protected sealed override global::System.Type[] GetGeneratedDomainModelTypes()
		{
			return new global::System.Type[]
			{
				typeof(MyCustomExtensionElement),
				typeof(ObjectTypeRequiresMeaningfulNameError),
			};
		}
		/// <summary>
		/// Gets the list of generated domain properties.
		/// </summary>
		/// <returns>List of property data.</returns>
		protected sealed override DomainMemberInfo[] GetGeneratedDomainProperties()
		{
			return new DomainMemberInfo[]
			{
				new DomainMemberInfo(typeof(MyCustomExtensionElement), "TestProperty", MyCustomExtensionElement.TestPropertyDomainPropertyId, typeof(MyCustomExtensionElement.TestPropertyPropertyHandler)),
				new DomainMemberInfo(typeof(MyCustomExtensionElement), "CustomEnum", MyCustomExtensionElement.CustomEnumDomainPropertyId, typeof(MyCustomExtensionElement.CustomEnumPropertyHandler)),
			};
		}
		#endregion
		#region Factory methods
		private static global::System.Collections.Generic.Dictionary<global::System.Type, int> createElementMap;
	
		/// <summary>
		/// Creates an element of specified type.
		/// </summary>
		/// <param name="partition">Partition where element is to be created.</param>
		/// <param name="elementType">Element type which belongs to this domain model.</param>
		/// <param name="propertyAssignments">New element property assignments.</param>
		/// <returns>Created element.</returns>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public sealed override DslModeling::ModelElement CreateElement(DslModeling::Partition partition, global::System.Type elementType, DslModeling::PropertyAssignment[] propertyAssignments)
		{
			if (elementType == null) throw new global::System.ArgumentNullException("elementType");
	
			if (createElementMap == null)
			{
				createElementMap = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(2);
				createElementMap.Add(typeof(MyCustomExtensionElement), 0);
				createElementMap.Add(typeof(ObjectTypeRequiresMeaningfulNameError), 1);
			}
			int index;
			if (!createElementMap.TryGetValue(elementType, out index))
			{
				throw new global::System.ArgumentException("elementType is not recognized as a type of domain class which belongs to this domain model.");
			}
			switch (index)
			{
				// A constructor was not generated for MyCustomExtensionElement because it had HasCustomConstructor
				// set to true. Please provide the constructor below.
				case 0: return new MyCustomExtensionElement(partition, propertyAssignments);
				case 1: return new ObjectTypeRequiresMeaningfulNameError(partition, propertyAssignments);
				default: return null;
			}
		}
	
		private static global::System.Collections.Generic.Dictionary<global::System.Type, int> createElementLinkMap;
	
		/// <summary>
		/// Creates an element link of specified type.
		/// </summary>
		/// <param name="partition">Partition where element is to be created.</param>
		/// <param name="elementLinkType">Element link type which belongs to this domain model.</param>
		/// <param name="roleAssignments">List of relationship role assignments for the new link.</param>
		/// <param name="propertyAssignments">New element property assignments.</param>
		/// <returns>Created element link.</returns>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public sealed override DslModeling::ElementLink CreateElementLink(DslModeling::Partition partition, global::System.Type elementLinkType, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
		{
			if (elementLinkType == null) throw new global::System.ArgumentNullException("elementType");
			if (roleAssignments == null) throw new global::System.ArgumentNullException("roleAssignments");
	
			if (createElementLinkMap == null)
			{
				createElementLinkMap = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(0);
			}
			int index;
			if (!createElementLinkMap.TryGetValue(elementLinkType, out index))
			{
				throw new global::System.ArgumentException("elementLinkType is not recognized as a type of domain relationship which belongs to this domain model.");
			}
			switch (index)
			{
				default: return null;
			}
		}
		#endregion
		#region Resource manager
		
		private static global::System.Resources.ResourceManager resourceManager;
		
		/// <summary>
		/// The base name of this model's resources.
		/// </summary>
		public const string ResourceBaseName = "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx";
		
		/// <summary>
		/// Gets the DomainModel's ResourceManager. If the ResourceManager does not already exist, then it is created.
		/// </summary>
		public override global::System.Resources.ResourceManager ResourceManager
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return ExtensionDomainModel.SingletonResourceManager;
			}
		}
	
		/// <summary>
		/// Gets the Singleton ResourceManager for this domain model.
		/// </summary>
		public static global::System.Resources.ResourceManager SingletonResourceManager
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				if (ExtensionDomainModel.resourceManager == null)
				{
					ExtensionDomainModel.resourceManager = new global::System.Resources.ResourceManager(ResourceBaseName, typeof(ExtensionDomainModel).Assembly);
				}
				return ExtensionDomainModel.resourceManager;
			}
		}
		#endregion
		#region Copy/Remove closures
		/// <summary>
		/// CopyClosure cache
		/// </summary>
		private static DslModeling::IElementVisitorFilter copyClosure;
		/// <summary>
		/// DeleteClosure cache
		/// </summary>
		private static DslModeling::IElementVisitorFilter removeClosure;
		/// <summary>
		/// Returns an IElementVisitorFilter that corresponds to the ClosureType.
		/// </summary>
		/// <param name="type">closure type</param>
		/// <param name="rootElements">collection of root elements</param>
		/// <returns>IElementVisitorFilter or null</returns>
		public override DslModeling::IElementVisitorFilter GetClosureFilter(DslModeling::ClosureType type, global::System.Collections.Generic.ICollection<DslModeling::ModelElement> rootElements)
		{
			switch (type)
			{
				case DslModeling::ClosureType.CopyClosure:
					return ExtensionDomainModel.CopyClosure;
				case DslModeling::ClosureType.DeleteClosure:
					return ExtensionDomainModel.DeleteClosure;
			}
			return base.GetClosureFilter(type, rootElements);
		}
		/// <summary>
		/// CopyClosure cache
		/// </summary>
		private static DslModeling::IElementVisitorFilter CopyClosure
		{
			get
			{
				// Incorporate all of the closures from the models we extend
				if (ExtensionDomainModel.copyClosure == null)
				{
					DslModeling::ChainingElementVisitorFilter copyFilter = new DslModeling::ChainingElementVisitorFilter();
					copyFilter.AddFilter(new ExtensionCopyClosure());
					
					ExtensionDomainModel.copyClosure = copyFilter;
				}
				return ExtensionDomainModel.copyClosure;
			}
		}
		/// <summary>
		/// DeleteClosure cache
		/// </summary>
		private static DslModeling::IElementVisitorFilter DeleteClosure
		{
			get
			{
				// Incorporate all of the closures from the models we extend
				if (ExtensionDomainModel.removeClosure == null)
				{
					DslModeling::ChainingElementVisitorFilter removeFilter = new DslModeling::ChainingElementVisitorFilter();
					removeFilter.AddFilter(new ExtensionDeleteClosure());
		
					ExtensionDomainModel.removeClosure = removeFilter;
				}
				return ExtensionDomainModel.removeClosure;
			}
		}
		#endregion
	}
		
	#region Copy/Remove closure classes
	/// <summary>
	/// Remove closure visitor filter
	/// </summary>
	[global::System.CLSCompliant(true)]
	public partial class ExtensionDeleteClosure : ExtensionDeleteClosureBase, DslModeling::IElementVisitorFilter
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtensionDeleteClosure() : base()
		{
		}
	}
	
	/// <summary>
	/// Base class for remove closure visitor filter
	/// </summary>
	public partial class ExtensionDeleteClosureBase : DslModeling::IElementVisitorFilter
	{
		/// <summary>
		/// DomainRoles
		/// </summary>
		private global::System.Collections.Generic.Dictionary<global::System.Guid, bool> domainRoles;
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtensionDeleteClosureBase()
		{
			#region Initialize DomainData Table
			#endregion
		}
		/// <summary>
		/// Called to ask the filter if a particular relationship from a source element should be included in the traversal
		/// </summary>
		/// <param name="walker">ElementWalker that is traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="sourceRoleInfo">DomainRoleInfo of the role that the source element is playing in the relationship</param>
		/// <param name="domainRelationshipInfo">DomainRelationshipInfo for the ElementLink in question</param>
		/// <param name="targetRelationship">Relationship in question</param>
		/// <returns>Yes if the relationship should be traversed</returns>
		public virtual DslModeling::VisitorFilterResult ShouldVisitRelationship(DslModeling::ElementWalker walker, DslModeling::ModelElement sourceElement, DslModeling::DomainRoleInfo sourceRoleInfo, DslModeling::DomainRelationshipInfo domainRelationshipInfo, DslModeling::ElementLink targetRelationship)
		{
			return DslModeling::VisitorFilterResult.Yes;
		}
		/// <summary>
		/// Called to ask the filter if a particular role player should be Visited during traversal
		/// </summary>
		/// <param name="walker">ElementWalker that is traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="elementLink">Element Link that forms the relationship to the role player in question</param>
		/// <param name="targetDomainRole">DomainRoleInfo of the target role</param>
		/// <param name="targetRolePlayer">Model Element that plays the target role in the relationship</param>
		/// <returns></returns>
		public virtual DslModeling::VisitorFilterResult ShouldVisitRolePlayer(DslModeling::ElementWalker walker, DslModeling::ModelElement sourceElement, DslModeling::ElementLink elementLink, DslModeling::DomainRoleInfo targetDomainRole, DslModeling::ModelElement targetRolePlayer)
		{
			return this.DomainRoles.ContainsKey(targetDomainRole.Id) ? DslModeling::VisitorFilterResult.Yes : DslModeling::VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// DomainRoles
		/// </summary>
		private global::System.Collections.Generic.Dictionary<global::System.Guid, bool> DomainRoles
		{
			get
			{
				if (this.domainRoles == null)
				{
					this.domainRoles = new global::System.Collections.Generic.Dictionary<global::System.Guid, bool>();
				}
				return this.domainRoles;
			}
		}
	
	}
	/// <summary>
	/// Copy closure visitor filter
	/// </summary>
	[global::System.CLSCompliant(true)]
	public partial class ExtensionCopyClosure : ExtensionCopyClosureBase, DslModeling::IElementVisitorFilter
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtensionCopyClosure() : base()
		{
		}
	}
	/// <summary>
	/// Base class for copy closure visitor filter
	/// </summary>
	public partial class ExtensionCopyClosureBase : DslModeling::IElementVisitorFilter
	{
		/// <summary>
		/// DomainRoles
		/// </summary>
		private global::System.Collections.Generic.Dictionary<global::System.Guid, bool> domainRoles;
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtensionCopyClosureBase()
		{
			#region Initialize DomainData Table
			#endregion
		}
		/// <summary>
		/// Called to ask the filter if a particular relationship from a source element should be included in the traversal
		/// </summary>
		/// <param name="walker">ElementWalker traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="sourceRoleInfo">DomainRoleInfo of the role that the source element is playing in the relationship</param>
		/// <param name="domainRelationshipInfo">DomainRelationshipInfo for the ElementLink in question</param>
		/// <param name="targetRelationship">Relationship in question</param>
		/// <returns>Yes if the relationship should be traversed</returns>
		public virtual DslModeling::VisitorFilterResult ShouldVisitRelationship(DslModeling::ElementWalker walker, DslModeling::ModelElement sourceElement, DslModeling::DomainRoleInfo sourceRoleInfo, DslModeling::DomainRelationshipInfo domainRelationshipInfo, DslModeling::ElementLink targetRelationship)
		{
			return this.DomainRoles.ContainsKey(sourceRoleInfo.Id) ? DslModeling::VisitorFilterResult.Yes : DslModeling::VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// Called to ask the filter if a particular role player should be Visited during traversal
		/// </summary>
		/// <param name="walker">ElementWalker traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="elementLink">Element Link that forms the relationship to the role player in question</param>
		/// <param name="targetDomainRole">DomainRoleInfo of the target role</param>
		/// <param name="targetRolePlayer">Model Element that plays the target role in the relationship</param>
		/// <returns></returns>
		public virtual DslModeling::VisitorFilterResult ShouldVisitRolePlayer(DslModeling::ElementWalker walker, DslModeling::ModelElement sourceElement, DslModeling::ElementLink elementLink, DslModeling::DomainRoleInfo targetDomainRole, DslModeling::ModelElement targetRolePlayer)
		{
			return this.DomainRoles.ContainsKey(targetDomainRole.Id) ? DslModeling::VisitorFilterResult.Yes : DslModeling::VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// DomainRoles
		/// </summary>
		private global::System.Collections.Generic.Dictionary<global::System.Guid, bool> DomainRoles
		{
			get
			{
				if (this.domainRoles == null)
				{
					this.domainRoles = new global::System.Collections.Generic.Dictionary<global::System.Guid, bool>();
				}
				return this.domainRoles;
			}
		}
	
	}
	#endregion
		
}
namespace ORMSolutions.ORMArchitect.ExtensionExample
{
	/// <summary>
	/// DomainEnumeration: TestEnumeration
	/// Provides test values for our enum sample dropdown.
	/// </summary>
	[global::System.ComponentModel.TypeConverter(typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter<TestEnumeration, global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel>))]
	[global::System.CLSCompliant(true)]
	public enum TestEnumeration
	{
		/// <summary>
		/// Zero
		/// Description for ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration.Zero
		/// </summary>
		[DslDesign::DescriptionResource("ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration/Zero.Description", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
		Zero = 0,
		/// <summary>
		/// One
		/// Description for ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration.One
		/// </summary>
		[DslDesign::DescriptionResource("ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration/One.Description", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
		One = 1,
		/// <summary>
		/// Two
		/// Description for ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration.Two
		/// </summary>
		[DslDesign::DescriptionResource("ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration/Two.Description", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
		Two = 2,
		/// <summary>
		/// Three
		/// Description for ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration.Three
		/// </summary>
		[DslDesign::DescriptionResource("ORMSolutions.ORMArchitect.ExtensionExample.TestEnumeration/Three.Description", typeof(global::ORMSolutions.ORMArchitect.ExtensionExample.ExtensionDomainModel), "ORMSolutions.ORMArchitect.ExtensionExample.GeneratedCode.DomainModelResx")]
		Three = 3,
	}
}

