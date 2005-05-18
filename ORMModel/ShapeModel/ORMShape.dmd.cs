// ------------------------------------------------------------------------------
// Copyright (c) Northface University. All Rights Reserved.
// Information Contained Herein is Proprietary and Confidential.
// ------------------------------------------------------------------------------
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.InheritBaseModel("2b131234-7959-458d-834f-2dc0769ce683")]
	[Microsoft.VisualStudio.Modeling.InheritBaseModel("91d59b16-e488-4a28-8d51-59273ad5bf2e")]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ORMShapeModel.MetaModelGuidString, "Northface.Tools.ORM.ShapeModel.ORMShapeModel")]
	public  partial class ORMShapeModel : Microsoft.VisualStudio.Modeling.SubStore
	{
		#region ORMShapeModel's Generated MetaClass Code
		/// <summary>
		/// MetaModel Guid String
		/// </summary>
		public const System.String MetaModelGuidString = "c52fb9a5-6bf4-4267-8716-71d74c7aa89c";
		/// <summary>
		/// MetaModel Guid
		/// </summary>
		public static readonly System.Guid MetaModelGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ORMShapeModel.MetaModelGuidString);
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMShapeModel(Microsoft.VisualStudio.Modeling.Store store) : base(store, Northface.Tools.ORM.ShapeModel.ORMShapeModel.MetaModelGuid)
		{
		}
		#endregion

	}
	#region ORMShapeModel's ResourceManager Code
	public  partial class ORMShapeModel
	{
		private static System.Resources.ResourceManager resourceManager = null;
		/// <summary>
		/// The base name of this models resources.
		/// </summary>
		public const string ResourceBaseName = "Northface.Tools.ORM.ShapeModel.ORMShapeModel";
		/// <summary>
		/// Returns the SubStore's ResourceManager. If the ResourceManager does not already exist, then it is created.
		/// </summary>
		public override System.Resources.ResourceManager ResourceManager
		{
			get
			{
				return Northface.Tools.ORM.ShapeModel.ORMShapeModel.SingletonResourceManager;
			}
		}
		/// <summary>
		/// A internal object used for synchronization.
		/// </summary>
		private static object internalSyncObject;
		/// <summary>
		/// Gets the internal object used for synchronization.
		/// </summary>
		private static object InternalSyncObject 
		{
			get 
			{
				if (internalSyncObject == null) 
				{
					object o = new object();
					System.Threading.Interlocked.CompareExchange(ref internalSyncObject, o, null);
				}
				return internalSyncObject;
			}
		}
		/// <summary>
		/// Gets the Singleton ResourceManager for this SubStore
		/// </summary>
		public static System.Resources.ResourceManager SingletonResourceManager
		{
			get
			{
				if (Northface.Tools.ORM.ShapeModel.ORMShapeModel.resourceManager == null)
				{
					lock (Northface.Tools.ORM.ShapeModel.ORMShapeModel.InternalSyncObject)
					{
						if (Northface.Tools.ORM.ShapeModel.ORMShapeModel.resourceManager == null)
						{
							Northface.Tools.ORM.ShapeModel.ORMShapeModel.resourceManager = new System.Resources.ResourceManager(ResourceBaseName, typeof(Northface.Tools.ORM.ShapeModel.ORMShapeModel).Assembly);
						}
					}
				}
				return Northface.Tools.ORM.ShapeModel.ORMShapeModel.resourceManager;
			}
		}
	}
	#endregion
	/// <summary>
	/// Copy closure visitor filter
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	public sealed class ORMShapeModelCopyClosure : Microsoft.VisualStudio.Modeling.IElementVisitorFilter
	{
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> metaRolesMember;
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMShapeModelCopyClosure()
		{
			#region Initialize MetaData Table
			#endregion
		}
		/// <summary>
		/// Called to ask the filter if a particular relationship from a source element should be included in the traversal
		/// </summary>
		/// <param name="walker">ElementWalker traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="sourceRoleInfo">MetaRoleInfo of the role that the source element is playing in the relationship</param>
		/// <param name="metaRelationshipInfo">MetaRelationshipInfo for the ElementLink in question</param>
		/// <param name="targetRelationship">Relationship in question</param>
		/// <returns>Yes if the relationship should be traversed</returns>
		public  Microsoft.VisualStudio.Modeling.VisitorFilterResult ShouldVisitRelationship(Microsoft.VisualStudio.Modeling.ElementWalker walker, Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleInfo, Microsoft.VisualStudio.Modeling.MetaRelationshipInfo metaRelationshipInfo, Microsoft.VisualStudio.Modeling.ElementLink targetRelationship)
		{
			return this.MetaRoles.ContainsKey(sourceRoleInfo.Id) ? Microsoft.VisualStudio.Modeling.VisitorFilterResult.Yes : Microsoft.VisualStudio.Modeling.VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// Called to ask the filter if a particular role player should be Visited during traversal
		/// </summary>
		/// <param name="walker">ElementWalker traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="elementLink">Element Link that forms the relationship to the role player in question</param>
		/// <param name="targetRoleInfo">MetaRoleInfo of the target role</param>
		/// <param name="targetRolePlayer">Model Element that plays the target role in the relationship</param>
		/// <returns></returns>
		public  Microsoft.VisualStudio.Modeling.VisitorFilterResult ShouldVisitRolePlayer(Microsoft.VisualStudio.Modeling.ElementWalker walker, Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ElementLink elementLink, Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleInfo, Microsoft.VisualStudio.Modeling.ModelElement targetRolePlayer)
		{
			foreach (Microsoft.VisualStudio.Modeling.MetaRoleInfo metaRoleInfo in elementLink.MetaRelationship.MetaRoles)
			{
				if (metaRoleInfo != targetRoleInfo && this.MetaRoles.ContainsKey(metaRoleInfo.Id))
				{
					return Microsoft.VisualStudio.Modeling.VisitorFilterResult.Yes;
				}
			}
			return Microsoft.VisualStudio.Modeling.VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> MetaRoles
		{
			get
			{
				if (this.metaRolesMember == null)
				{
					this.metaRolesMember = new System.Collections.Generic.Dictionary<System.Guid, System.Guid>();
				}
				return this.metaRolesMember;
			}
		}

	}
	/// <summary>
	/// Remove closure visitor filter
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	public sealed class ORMShapeModelRemoveClosure : Microsoft.VisualStudio.Modeling.IElementVisitorFilter
	{
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> metaRolesMember;
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMShapeModelRemoveClosure()
		{
			#region Initialize MetaData Table
			#endregion
		}
		/// <summary>
		/// Called to ask the filter if a particular relationship from a source element should be included in the traversal
		/// </summary>
		/// <param name="walker">ElementWalker that is traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="sourceRoleInfo">MetaRoleInfo of the role that the source element is playing in the relationship</param>
		/// <param name="metaRelationshipInfo">MetaRelationshipInfo for the ElementLink in question</param>
		/// <param name="targetRelationship">Relationship in question</param>
		/// <returns>Yes if the relationship should be traversed</returns>
		public  Microsoft.VisualStudio.Modeling.VisitorFilterResult ShouldVisitRelationship(Microsoft.VisualStudio.Modeling.ElementWalker walker, Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleInfo, Microsoft.VisualStudio.Modeling.MetaRelationshipInfo metaRelationshipInfo, Microsoft.VisualStudio.Modeling.ElementLink targetRelationship)
		{
			return Microsoft.VisualStudio.Modeling.VisitorFilterResult.Yes;
		}
		/// <summary>
		/// Called to ask the filter if a particular role player should be Visited during traversal
		/// </summary>
		/// <param name="walker">ElementWalker that is traversing the model</param>
		/// <param name="sourceElement">Model Element playing the source role</param>
		/// <param name="elementLink">Element Link that forms the relationship to the role player in question</param>
		/// <param name="targetRoleInfo">MetaRoleInfo of the target role</param>
		/// <param name="targetRolePlayer">Model Element that plays the target role in the relationship</param>
		/// <returns></returns>
		public  Microsoft.VisualStudio.Modeling.VisitorFilterResult ShouldVisitRolePlayer(Microsoft.VisualStudio.Modeling.ElementWalker walker, Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ElementLink elementLink, Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleInfo, Microsoft.VisualStudio.Modeling.ModelElement targetRolePlayer)
		{
			return this.MetaRoles.ContainsKey(targetRoleInfo.Id) ? Microsoft.VisualStudio.Modeling.VisitorFilterResult.Yes : Microsoft.VisualStudio.Modeling.VisitorFilterResult.DoNotCare;
		}
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> MetaRoles
		{
			get
			{
				if (this.metaRolesMember == null)
				{
					this.metaRolesMember = new System.Collections.Generic.Dictionary<System.Guid, System.Guid>();
				}
				return this.metaRolesMember;
			}
		}

	}
	#region ORMShapeModel's Generated Closure Code
	public  partial class ORMShapeModel
	{
		/// <summary>
		/// CopyClosure cache
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.IElementVisitorFilter copyClosureMember;
		/// <summary>
		/// RemoveClosure cache
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.IElementVisitorFilter removeClosureMember;
		/// <summary>
		/// Returns an IElementVisitorFilter that corresponds to the ClosureType.
		/// </summary>
		/// <param name="type">closure type</param>
		/// <param name="rootElements">collection of root elements</param>
		/// <returns>IElementVisitorFilter or null</returns>
		public override Microsoft.VisualStudio.Modeling.IElementVisitorFilter GetClosureFilter(Microsoft.VisualStudio.Modeling.ClosureType type, System.Collections.ICollection rootElements)
		{
			switch (type)
			{
				case Microsoft.VisualStudio.Modeling.ClosureType.CopyClosure:
					return ORMShapeModel.CopyClosure;
				case Microsoft.VisualStudio.Modeling.ClosureType.RemoveClosure:
					return ORMShapeModel.RemoveClosure;
			}
			return base.GetClosureFilter(type, rootElements);
		}
		/// <summary>
		/// CopyClosure cache
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.IElementVisitorFilter CopyClosure
		{
			get
			{
				if (ORMShapeModel.copyClosureMember == null)
				{
					ORMShapeModel.copyClosureMember = new ORMShapeModelCopyClosure();
				}
				return ORMShapeModel.copyClosureMember;
			}
		}
		/// <summary>
		/// RemoveClosure cache
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.IElementVisitorFilter RemoveClosure
		{
			get
			{
				if (ORMShapeModel.removeClosureMember == null)
				{
					ORMShapeModel.removeClosureMember = new ORMShapeModelRemoveClosure();
				}
				return ORMShapeModel.removeClosureMember;
			}
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ORMBaseShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ORMBaseShape")]
	public abstract partial class ORMBaseShape : Microsoft.VisualStudio.Modeling.Diagrams.NodeShape
	{
		#region ORMBaseShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "55131f4b-0f9a-408d-bed0-79451ba7f4f0";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ORMBaseShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region ORMBaseShape's Generated Constructor Code
	public abstract partial class ORMBaseShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ORMBaseShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ObjectTypeShape")]
	public  partial class ObjectTypeShape : Northface.Tools.ORM.ShapeModel.ORMBaseShape
	{
		#region ObjectTypeShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "00c1f246-d8f1-4eea-ac88-39ba238143a8";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

		#region ShapeName's Generated  Field Code
		#region ShapeName's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ShapeNameMetaAttributeGuidString = "b70a72b8-282e-403a-aad6-50d75a2f071c";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ShapeNameMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ShapeNameMetaAttributeGuidString);
		#endregion

		#region ShapeName's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(XPathExpression="/ObjectTypeShape/SubjectHasPresentation.Subject/Subject", ReverseXPathExpression="/NamedElement/SubjectHasPresentation.Presentation/Presentation", RealAttributeName="Name", ProxyAttributeName="Name", FieldHandlerType=typeof(ObjectTypeShapeShapeNameFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ShapeNameMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ShapeName")]
		public  System.String ShapeName
		{
			get
			{
				return objectTypeShapeShapeNameFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeShapeShapeNameFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeShapeShapeNameFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectTypeShape.ShapeName field
		/// </summary>
		private static ObjectTypeShapeShapeNameFieldHandler	objectTypeShapeShapeNameFieldHandler	= ObjectTypeShapeShapeNameFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectTypeShape.ShapeName
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeShapeShapeNameFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementPassThroughFieldHandler<Northface.Tools.ORM.ShapeModel.ObjectTypeShape,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeShapeShapeNameFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectTypeShape.ShapeName field handler
			/// </summary>
			/// <value>ObjectTypeShapeShapeNameFieldHandler</value>
			public static ObjectTypeShapeShapeNameFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeShapeNameFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeShapeNameFieldHandler;
					}
					else
					{
						// The static constructor in ObjectTypeShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeShapeNameFieldHandler, so just instantiate one and return it
						return new ObjectTypeShapeShapeNameFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectTypeShape.ShapeName field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ShapeNameMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region ReferenceModeName's Generated  Field Code
		#region ReferenceModeName's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ReferenceModeNameMetaAttributeGuidString = "55d073e6-a77b-4440-a881-b2a8222b6c8b";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeNameMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ReferenceModeNameMetaAttributeGuidString);
		#endregion

		#region ReferenceModeName's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(XPathExpression="/ObjectTypeShape/SubjectHasPresentation.Subject/Subject", ReverseXPathExpression="/ObjectType/SubjectHasPresentation.Presentation/Presentation", RealAttributeName="ReferenceModeString", ProxyAttributeName="ReferenceModeString", FieldHandlerType=typeof(ObjectTypeShapeReferenceModeNameFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ReferenceModeNameMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ReferenceModeName")]
		public  System.String ReferenceModeName
		{
			get
			{
				return objectTypeShapeReferenceModeNameFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeShapeReferenceModeNameFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeShapeReferenceModeNameFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectTypeShape.ReferenceModeName field
		/// </summary>
		private static ObjectTypeShapeReferenceModeNameFieldHandler	objectTypeShapeReferenceModeNameFieldHandler	= ObjectTypeShapeReferenceModeNameFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectTypeShape.ReferenceModeName
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeShapeReferenceModeNameFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementPassThroughFieldHandler<Northface.Tools.ORM.ShapeModel.ObjectTypeShape,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeShapeReferenceModeNameFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectTypeShape.ReferenceModeName field handler
			/// </summary>
			/// <value>ObjectTypeShapeReferenceModeNameFieldHandler</value>
			public static ObjectTypeShapeReferenceModeNameFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeReferenceModeNameFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeReferenceModeNameFieldHandler;
					}
					else
					{
						// The static constructor in ObjectTypeShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeReferenceModeNameFieldHandler, so just instantiate one and return it
						return new ObjectTypeShapeReferenceModeNameFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectTypeShape.ReferenceModeName field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ReferenceModeNameMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region ExpandRefMode's Generated  Field Code
		#region ExpandRefMode's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ExpandRefModeMetaAttributeGuidString = "b2415bb1-1c83-4f0b-b2c3-58b67bc620dd";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ExpandRefModeMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ExpandRefModeMetaAttributeGuidString);
		#endregion

		#region ExpandRefMode's Generated Property Code

		private System.Boolean expandRefModePropertyStorage = false;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ObjectTypeShapeExpandRefModeFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ExpandRefModeMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ExpandRefMode")]
		public  System.Boolean ExpandRefMode
		{
			get
			{
				return expandRefModePropertyStorage;
			}
		
			set
			{
				objectTypeShapeExpandRefModeFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeShapeExpandRefModeFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectTypeShape.ExpandRefMode field
		/// </summary>
		private static ObjectTypeShapeExpandRefModeFieldHandler	objectTypeShapeExpandRefModeFieldHandler	= ObjectTypeShapeExpandRefModeFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectTypeShape.ExpandRefMode
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeShapeExpandRefModeFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ShapeModel.ObjectTypeShape,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeShapeExpandRefModeFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectTypeShape.ExpandRefMode field handler
			/// </summary>
			/// <value>ObjectTypeShapeExpandRefModeFieldHandler</value>
			public static ObjectTypeShapeExpandRefModeFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeExpandRefModeFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeExpandRefModeFieldHandler;
					}
					else
					{
						// The static constructor in ObjectTypeShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.ObjectTypeShape.objectTypeShapeExpandRefModeFieldHandler, so just instantiate one and return it
						return new ObjectTypeShapeExpandRefModeFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectTypeShape.ExpandRefMode field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.ObjectTypeShape.ExpandRefModeMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ObjectTypeShape</param>
			protected sealed override System.Boolean GetValue(Northface.Tools.ORM.ShapeModel.ObjectTypeShape element)
			{
				return element.expandRefModePropertyStorage;
			}

			/// <summary>
			/// Sets the value into the element
			/// </summary>
			/// <param name="element">the element</param>
			/// <param name="value">new value</param>
			/// <param name="commandFactory">the command factory for this change</param>
			/// <param name="allowDuplicates">allow duplicate value to continue to fire rules and events</param>
			/// <param name="oldValue">the old value before the change</param>
			/// <returns>true if the value actually changed</returns>
			protected sealed override bool SetValue(Northface.Tools.ORM.ShapeModel.ObjectTypeShape element, System.Boolean value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Boolean oldValue)
			{
				oldValue = element.expandRefModePropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.expandRefModePropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
	}
	#region ObjectTypeShape's Generated Constructor Code
	public  partial class ObjectTypeShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeShape CreateObjectTypeShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ObjectTypeShape)store.ElementFactory.CreateElement(typeof(ObjectTypeShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeShape CreateAndInitializeObjectTypeShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectTypeShape)store.ElementFactory.CreateElement(typeof(ObjectTypeShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectTypeShape
	/// <summary>
	/// ObjectTypeShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ObjectTypeShape))]
	public sealed class ObjectTypeShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ObjectTypeShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.FactType))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.FactTypeShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.FactTypeShape")]
	public  partial class FactTypeShape : Northface.Tools.ORM.ShapeModel.ORMBaseShape
	{
		#region FactTypeShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "8e440a3b-275e-42f7-868b-d5d473158acd";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.FactTypeShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

		#region ConstraintDisplayPosition's Generated  Field Code
		#region ConstraintDisplayPosition's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ConstraintDisplayPositionMetaAttributeGuidString = "802767fd-de7d-4541-b42b-90b613dfe22d";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ConstraintDisplayPositionMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPositionMetaAttributeGuidString);
		#endregion

		#region ConstraintDisplayPosition's Generated Property Code

		private Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition constraintDisplayPositionPropertyStorage = Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition.Top;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.EnumerationDomainAttribute(EnumerationType=typeof(Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition),DefaultEnumerationValueName="Top")]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(FactTypeShapeConstraintDisplayPositionFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPositionMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPosition")]
		public  Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition ConstraintDisplayPosition
		{
			get
			{
				return constraintDisplayPositionPropertyStorage;
			}
		
			set
			{
				factTypeShapeConstraintDisplayPositionFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region FactTypeShapeConstraintDisplayPositionFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for FactTypeShape.ConstraintDisplayPosition field
		/// </summary>
		private static FactTypeShapeConstraintDisplayPositionFieldHandler	factTypeShapeConstraintDisplayPositionFieldHandler	= FactTypeShapeConstraintDisplayPositionFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for FactTypeShape.ConstraintDisplayPosition
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class FactTypeShapeConstraintDisplayPositionFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ShapeModel.FactTypeShape,Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private FactTypeShapeConstraintDisplayPositionFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the FactTypeShape.ConstraintDisplayPosition field handler
			/// </summary>
			/// <value>FactTypeShapeConstraintDisplayPositionFieldHandler</value>
			public static FactTypeShapeConstraintDisplayPositionFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.FactTypeShape.factTypeShapeConstraintDisplayPositionFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.FactTypeShape.factTypeShapeConstraintDisplayPositionFieldHandler;
					}
					else
					{
						// The static constructor in FactTypeShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.FactTypeShape.factTypeShapeConstraintDisplayPositionFieldHandler, so just instantiate one and return it
						return new FactTypeShapeConstraintDisplayPositionFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the FactTypeShape.ConstraintDisplayPosition field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPositionMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the FactTypeShape</param>
			protected sealed override Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition GetValue(Northface.Tools.ORM.ShapeModel.FactTypeShape element)
			{
				return element.constraintDisplayPositionPropertyStorage;
			}

			/// <summary>
			/// Sets the value into the element
			/// </summary>
			/// <param name="element">the element</param>
			/// <param name="value">new value</param>
			/// <param name="commandFactory">the command factory for this change</param>
			/// <param name="allowDuplicates">allow duplicate value to continue to fire rules and events</param>
			/// <param name="oldValue">the old value before the change</param>
			/// <returns>true if the value actually changed</returns>
			protected sealed override bool SetValue(Northface.Tools.ORM.ShapeModel.FactTypeShape element, Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref Northface.Tools.ORM.ShapeModel.ConstraintDisplayPosition oldValue)
			{
				oldValue = element.constraintDisplayPositionPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.constraintDisplayPositionPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
	}
	#region FactTypeShape's Generated Constructor Code
	public  partial class FactTypeShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeShape CreateFactTypeShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (FactTypeShape)store.ElementFactory.CreateElement(typeof(FactTypeShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeShape CreateAndInitializeFactTypeShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeShape)store.ElementFactory.CreateElement(typeof(FactTypeShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeShape
	/// <summary>
	/// FactTypeShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.FactTypeShape))]
	public sealed class FactTypeShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.FactTypeShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))]
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ExternalConstraintShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ExternalConstraintShape")]
	public  partial class ExternalConstraintShape : Northface.Tools.ORM.ShapeModel.ORMBaseShape
	{
		#region ExternalConstraintShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "00a08f56-73ba-4c8f-8fa1-ae61b8fc1cae";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ExternalConstraintShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region ExternalConstraintShape's Generated Constructor Code
	public  partial class ExternalConstraintShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintShape CreateExternalConstraintShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ExternalConstraintShape)store.ElementFactory.CreateElement(typeof(ExternalConstraintShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintShape CreateAndInitializeExternalConstraintShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintShape)store.ElementFactory.CreateElement(typeof(ExternalConstraintShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintShape
	/// <summary>
	/// ExternalConstraintShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ExternalConstraintShape))]
	public sealed class ExternalConstraintShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ExternalConstraintShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.FrequencyConstraint))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.FrequencyConstraintShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.FrequencyConstraintShape")]
	public  partial class FrequencyConstraintShape : Northface.Tools.ORM.ShapeModel.ExternalConstraintShape
	{
		#region FrequencyConstraintShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "ec47cd7d-023b-4971-8b5b-1242dbc7356f";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.FrequencyConstraintShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region FrequencyConstraintShape's Generated Constructor Code
	public  partial class FrequencyConstraintShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FrequencyConstraintShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FrequencyConstraintShape CreateFrequencyConstraintShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (FrequencyConstraintShape)store.ElementFactory.CreateElement(typeof(FrequencyConstraintShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FrequencyConstraintShape CreateAndInitializeFrequencyConstraintShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FrequencyConstraintShape)store.ElementFactory.CreateElement(typeof(FrequencyConstraintShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FrequencyConstraintShape
	/// <summary>
	/// FrequencyConstraintShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.FrequencyConstraintShape))]
	public sealed class FrequencyConstraintShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FrequencyConstraintShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.FrequencyConstraintShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.FloatingTextShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.FloatingTextShape")]
	public abstract partial class FloatingTextShape : Northface.Tools.ORM.ShapeModel.ORMBaseShape
	{
		#region FloatingTextShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "0904999f-d9c5-4c4e-a08f-f8dd4b2f29a3";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.FloatingTextShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region FloatingTextShape's Generated Constructor Code
	public abstract partial class FloatingTextShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected FloatingTextShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape")]
	public  partial class ObjectifiedFactTypeNameShape : Northface.Tools.ORM.ShapeModel.FloatingTextShape
	{
		#region ObjectifiedFactTypeNameShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "7fd5183a-8bc2-43bb-8474-a0a2d558d90a";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

		#region ObjectTypeName's Generated  Field Code
		#region ObjectTypeName's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ObjectTypeNameMetaAttributeGuidString = "4c9bb78d-6290-455e-b844-e7bcc01676f8";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ObjectTypeNameMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.ObjectTypeNameMetaAttributeGuidString);
		#endregion

		#region ObjectTypeName's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(XPathExpression="/ObjectifiedFactTypeNameShape/SubjectHasPresentation.Subject/Subject", ReverseXPathExpression="/NamedElement/SubjectHasPresentation.Presentation/Presentation", RealAttributeName="Name", ProxyAttributeName="Name", FieldHandlerType=typeof(ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.ObjectTypeNameMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.ObjectTypeName")]
		public  System.String ObjectTypeName
		{
			get
			{
				return objectifiedFactTypeNameShapeObjectTypeNameFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectifiedFactTypeNameShapeObjectTypeNameFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectifiedFactTypeNameShape.ObjectTypeName field
		/// </summary>
		private static ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler	objectifiedFactTypeNameShapeObjectTypeNameFieldHandler	= ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectifiedFactTypeNameShape.ObjectTypeName
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementPassThroughFieldHandler<Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectifiedFactTypeNameShape.ObjectTypeName field handler
			/// </summary>
			/// <value>ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler</value>
			public static ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.objectifiedFactTypeNameShapeObjectTypeNameFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.objectifiedFactTypeNameShapeObjectTypeNameFieldHandler;
					}
					else
					{
						// The static constructor in ObjectifiedFactTypeNameShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.objectifiedFactTypeNameShapeObjectTypeNameFieldHandler, so just instantiate one and return it
						return new ObjectifiedFactTypeNameShapeObjectTypeNameFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectifiedFactTypeNameShape.ObjectTypeName field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape.ObjectTypeNameMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
	}
	#region ObjectifiedFactTypeNameShape's Generated Constructor Code
	public  partial class ObjectifiedFactTypeNameShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectifiedFactTypeNameShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectifiedFactTypeNameShape CreateObjectifiedFactTypeNameShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ObjectifiedFactTypeNameShape)store.ElementFactory.CreateElement(typeof(ObjectifiedFactTypeNameShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectifiedFactTypeNameShape CreateAndInitializeObjectifiedFactTypeNameShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectifiedFactTypeNameShape)store.ElementFactory.CreateElement(typeof(ObjectifiedFactTypeNameShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectifiedFactTypeNameShape
	/// <summary>
	/// ObjectifiedFactTypeNameShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape))]
	public sealed class ObjectifiedFactTypeNameShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectifiedFactTypeNameShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ObjectifiedFactTypeNameShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ReadingShape.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ReadingShape")]
	public  partial class ReadingShape : Northface.Tools.ORM.ShapeModel.FloatingTextShape
	{
		#region ReadingShape's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c567ed6d-d0a6-4fd8-a974-c567aa309d5e";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ReadingShape.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

		#region ReadingText's Generated  Field Code
		#region ReadingText's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ReadingTextMetaAttributeGuidString = "22bf9cf7-715c-4159-9ef0-1bdf98647f9c";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ReadingTextMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ReadingShape.ReadingTextMetaAttributeGuidString);
		#endregion

		#region ReadingText's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(XPathExpression="/ReadingShape/SubjectHasPresentation.Subject/Subject", ReverseXPathExpression="/Reading/SubjectHasPresentation.Presentation/Presentation", RealAttributeName="ReadingText", ProxyAttributeName="ReadingText", FieldHandlerType=typeof(ReadingShapeReadingTextFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ReadingShape.ReadingTextMetaAttributeGuidString, "Northface.Tools.ORM.ShapeModel.ReadingShape.ReadingText")]
		public  System.String ReadingText
		{
			get
			{
				return readingShapeReadingTextFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				readingShapeReadingTextFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReadingShapeReadingTextFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ReadingShape.ReadingText field
		/// </summary>
		private static ReadingShapeReadingTextFieldHandler	readingShapeReadingTextFieldHandler	= ReadingShapeReadingTextFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ReadingShape.ReadingText
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReadingShapeReadingTextFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementPassThroughFieldHandler<Northface.Tools.ORM.ShapeModel.ReadingShape,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReadingShapeReadingTextFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ReadingShape.ReadingText field handler
			/// </summary>
			/// <value>ReadingShapeReadingTextFieldHandler</value>
			public static ReadingShapeReadingTextFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ShapeModel.ReadingShape.readingShapeReadingTextFieldHandler != null)
					{
						return Northface.Tools.ORM.ShapeModel.ReadingShape.readingShapeReadingTextFieldHandler;
					}
					else
					{
						// The static constructor in ReadingShape will assign this value to
						// Northface.Tools.ORM.ShapeModel.ReadingShape.readingShapeReadingTextFieldHandler, so just instantiate one and return it
						return new ReadingShapeReadingTextFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ReadingShape.ReadingText field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ShapeModel.ReadingShape.ReadingTextMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
	}
	#region ReadingShape's Generated Constructor Code
	public  partial class ReadingShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingShape CreateReadingShape(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ReadingShape)store.ElementFactory.CreateElement(typeof(ReadingShape));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingShape CreateAndInitializeReadingShape(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReadingShape)store.ElementFactory.CreateElement(typeof(ReadingShape), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReadingShape
	/// <summary>
	/// ReadingShape Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ReadingShape))]
	public sealed class ReadingShapeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingShapeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ReadingShape(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.RolePlayerLink.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.RolePlayerLink")]
	public  partial class RolePlayerLink : Microsoft.VisualStudio.Modeling.Diagrams.BinaryLinkShape
	{
		#region RolePlayerLink's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "2b3f0aae-b1b1-4727-8862-5c34b494b499";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.RolePlayerLink.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region RolePlayerLink's Generated Constructor Code
	public  partial class RolePlayerLink
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RolePlayerLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static RolePlayerLink CreateRolePlayerLink(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (RolePlayerLink)store.ElementFactory.CreateElement(typeof(RolePlayerLink));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static RolePlayerLink CreateAndInitializeRolePlayerLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (RolePlayerLink)store.ElementFactory.CreateElement(typeof(RolePlayerLink), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for RolePlayerLink
	/// <summary>
	/// RolePlayerLink Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.RolePlayerLink))]
	public sealed class RolePlayerLinkElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RolePlayerLinkElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.RolePlayerLink(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.SubtypeFact))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.SubtypeLink.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.SubtypeLink")]
	public  partial class SubtypeLink : Microsoft.VisualStudio.Modeling.Diagrams.BinaryLinkShape
	{
		#region SubtypeLink's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "87ddaeda-1fd8-4433-bb1e-7482c7f471a7";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.SubtypeLink.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region SubtypeLink's Generated Constructor Code
	public  partial class SubtypeLink
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SubtypeLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SubtypeLink CreateSubtypeLink(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (SubtypeLink)store.ElementFactory.CreateElement(typeof(SubtypeLink));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SubtypeLink CreateAndInitializeSubtypeLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (SubtypeLink)store.ElementFactory.CreateElement(typeof(SubtypeLink), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for SubtypeLink
	/// <summary>
	/// SubtypeLink Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.SubtypeLink))]
	public sealed class SubtypeLinkElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SubtypeLinkElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.SubtypeLink(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ShapeForAttribute(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ExternalConstraintLink.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ExternalConstraintLink")]
	public  partial class ExternalConstraintLink : Microsoft.VisualStudio.Modeling.Diagrams.BinaryLinkShape
	{
		#region ExternalConstraintLink's Generated Shape Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "8815e6d8-238b-422c-a4b3-29fdc8de9ea5";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ExternalConstraintLink.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region ExternalConstraintLink's Generated Constructor Code
	public  partial class ExternalConstraintLink
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintLink CreateExternalConstraintLink(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ExternalConstraintLink)store.ElementFactory.CreateElement(typeof(ExternalConstraintLink));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintLink CreateAndInitializeExternalConstraintLink(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintLink)store.ElementFactory.CreateElement(typeof(ExternalConstraintLink), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintLink
	/// <summary>
	/// ExternalConstraintLink Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ExternalConstraintLink))]
	public sealed class ExternalConstraintLinkElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintLinkElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ExternalConstraintLink(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.ComponentModel.ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramDefaultFilterString, System.ComponentModel.ToolboxItemFilterType.Require)]
	
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.EntityType.Item.Id", 0, "Toolbox.EntityType.Bitmap.Id", "Toolbox.EntityType.Caption.Id", "Toolbox.EntityType.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.ValueType.Item.Id", 5, "Toolbox.ValueType.Bitmap.Id", "Toolbox.ValueType.Caption.Id", "Toolbox.ValueType.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.UnaryFactType.Item.Id", 10, "Toolbox.UnaryFactType.Bitmap.Id", "Toolbox.UnaryFactType.Caption.Id", "Toolbox.UnaryFactType.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.BinaryFactType.Item.Id", 15, "Toolbox.BinaryFactType.Bitmap.Id", "Toolbox.BinaryFactType.Caption.Id", "Toolbox.BinaryFactType.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.TernaryFactType.Item.Id", 20, "Toolbox.TernaryFactType.Bitmap.Id", "Toolbox.TernaryFactType.Caption.Id", "Toolbox.TernaryFactType.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.RoleConnector.Item.Id", 21, "Toolbox.RoleConnector.Bitmap.Id", "Toolbox.RoleConnector.Caption.Id", "Toolbox.RoleConnector.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.SubtypeConnector.Item.Id", 22, "Toolbox.SubtypeConnector.Bitmap.Id", "Toolbox.SubtypeConnector.Caption.Id", "Toolbox.SubtypeConnector.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.InternalUniquenessConstraint.Item.Id", 23, "Toolbox.InternalUniquenessConstraint.Bitmap.Id", "Toolbox.InternalUniquenessConstraint.Caption.Id", "Toolbox.InternalUniquenessConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.ExternalUniquenessConstraint.Item.Id", 25, "Toolbox.ExternalUniquenessConstraint.Bitmap.Id", "Toolbox.ExternalUniquenessConstraint.Caption.Id", "Toolbox.ExternalUniquenessConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.EqualityConstraint.Item.Id", 35, "Toolbox.EqualityConstraint.Bitmap.Id", "Toolbox.EqualityConstraint.Caption.Id", "Toolbox.EqualityConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.ExclusionConstraint.Item.Id", 40, "Toolbox.ExclusionConstraint.Bitmap.Id", "Toolbox.ExclusionConstraint.Caption.Id", "Toolbox.ExclusionConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.InclusiveOrConstraint.Item.Id", 45, "Toolbox.InclusiveOrConstraint.Bitmap.Id", "Toolbox.InclusiveOrConstraint.Caption.Id", "Toolbox.InclusiveOrConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.ExclusiveOrConstraint.Item.Id", 50, "Toolbox.ExclusiveOrConstraint.Bitmap.Id", "Toolbox.ExclusiveOrConstraint.Caption.Id", "Toolbox.ExclusiveOrConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.SubsetConstraint.Item.Id", 55, "Toolbox.SubsetConstraint.Bitmap.Id", "Toolbox.SubsetConstraint.Caption.Id", "Toolbox.SubsetConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.FrequencyConstraint.Item.Id", 57, "Toolbox.FrequencyConstraint.Bitmap.Id", "Toolbox.FrequencyConstraint.Caption.Id", "Toolbox.FrequencyConstraint.Tooltip.Id", "Toolbox.DefaultTabName")]
	[Microsoft.VisualStudio.Modeling.MetaToolboxItem("Toolbox.ExternalConstraintConnector.Item.Id", 60, "Toolbox.ExternalConstraintConnector.Bitmap.Id", "Toolbox.ExternalConstraintConnector.Caption.Id", "Toolbox.ExternalConstraintConnector.Tooltip.Id", "Toolbox.DefaultTabName")][System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("c52fb9a5-6bf4-4267-8716-71d74c7aa89c")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ShapeModel.ORMDiagram.MetaClassGuidString, "Northface.Tools.ORM.ShapeModel.ORMDiagram")]
	public  partial class ORMDiagram : Microsoft.VisualStudio.Modeling.Diagrams.Diagram
	{
		#region ORMDiagram's Generated Diagram Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "948f992d-c9b8-46f9-be3c-b48347f8ab0b";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ShapeModel.ORMDiagram.MetaClassGuidString);
		#region Boilerplate code for every Shape-derived class
		/// <summary>
		/// Style Set
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.StyleSet classStyleSet = null;
		/// <summary>
		/// Shape Fields
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection shapeFields = null;
		/// <summary>
		/// Style Set
		/// </summary>
		protected override Microsoft.VisualStudio.Modeling.Diagrams.StyleSet ClassStyleSet
		{
			get
			{
				if (classStyleSet == null)
				{
					classStyleSet = CreateClassStyleSet();
				}
				return classStyleSet;
			}
		}
		/// <summary>
		/// Shape Fields
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.Diagrams.ShapeFieldCollection ShapeFields
		{
			get
			{
				if (shapeFields == null)
				{
					shapeFields = CreateShapeFields();
				}
				return shapeFields;
			}
		}
		#endregion
		#endregion

	}
	#region ORMDiagram's Generated Constructor Code
	public  partial class ORMDiagram
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMDiagram(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ORMDiagram CreateORMDiagram(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ORMDiagram)store.ElementFactory.CreateElement(typeof(ORMDiagram));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ORMDiagram CreateAndInitializeORMDiagram(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ORMDiagram)store.ElementFactory.CreateElement(typeof(ORMDiagram), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ORMDiagram
	/// <summary>
	/// ORMDiagram Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ShapeModel.ORMDiagram))]
	public sealed class ORMDiagramElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMDiagramElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ShapeModel.ORMDiagram(store, bag);
		}
	}
	#endregion

}



