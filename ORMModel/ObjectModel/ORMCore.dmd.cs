// ------------------------------------------------------------------------------
// Copyright (c) Northface University. All Rights Reserved.
// Information Contained Herein is Proprietary and Confidential.
// ------------------------------------------------------------------------------
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[Microsoft.VisualStudio.Modeling.InheritBaseModel("2b131234-7959-458d-834f-2dc0769ce683")]
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ORMMetaModel.MetaModelGuidString, "Northface.Tools.ORM.ObjectModel.ORMMetaModel")]
	public  partial class ORMMetaModel : Microsoft.VisualStudio.Modeling.SubStore
	{
		#region ORMMetaModel's Generated MetaClass Code
		/// <summary>
		/// MetaModel Guid String
		/// </summary>
		public const System.String MetaModelGuidString = "83ad9e12-0e90-47cd-8e2f-a79f8d9c7288";
		/// <summary>
		/// MetaModel Guid
		/// </summary>
		public static readonly System.Guid MetaModelGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ORMMetaModel.MetaModelGuidString);
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMMetaModel(Microsoft.VisualStudio.Modeling.Store store) : base(store, Northface.Tools.ORM.ObjectModel.ORMMetaModel.MetaModelGuid)
		{
		}
		#endregion

	}
	#region ORMMetaModel's ResourceManager Code
	public  partial class ORMMetaModel
	{
		private static System.Resources.ResourceManager resourceManager = null;
		/// <summary>
		/// The base name of this models resources.
		/// </summary>
		public const string ResourceBaseName = "Northface.Tools.ORM.ObjectModel.ORMMetaModel";
		/// <summary>
		/// Returns the SubStore's ResourceManager. If the ResourceManager does not already exist, then it is created.
		/// </summary>
		public override System.Resources.ResourceManager ResourceManager
		{
			get
			{
				return Northface.Tools.ORM.ObjectModel.ORMMetaModel.SingletonResourceManager;
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
				if (Northface.Tools.ORM.ObjectModel.ORMMetaModel.resourceManager == null)
				{
					lock (Northface.Tools.ORM.ObjectModel.ORMMetaModel.InternalSyncObject)
					{
						if (Northface.Tools.ORM.ObjectModel.ORMMetaModel.resourceManager == null)
						{
							Northface.Tools.ORM.ObjectModel.ORMMetaModel.resourceManager = new System.Resources.ResourceManager(ResourceBaseName, typeof(Northface.Tools.ORM.ObjectModel.ORMMetaModel).Assembly);
						}
					}
				}
				return Northface.Tools.ORM.ObjectModel.ORMMetaModel.resourceManager;
			}
		}
	}
	#endregion
	/// <summary>
	/// Copy closure visitor filter
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	public sealed class ORMMetaModelCopyClosure : Microsoft.VisualStudio.Modeling.IElementVisitorFilter
	{
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> metaRolesMember;
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMMetaModelCopyClosure()
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
	public sealed class ORMMetaModelRemoveClosure : Microsoft.VisualStudio.Modeling.IElementVisitorFilter
	{
		/// <summary>
		/// MetaRoles
		/// </summary>
		private System.Collections.Generic.Dictionary<System.Guid, System.Guid> metaRolesMember;
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMMetaModelRemoveClosure()
		{
			#region Initialize MetaData Table
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
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
	#region ORMMetaModel's Generated Closure Code
	public  partial class ORMMetaModel
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
					return ORMMetaModel.CopyClosure;
				case Microsoft.VisualStudio.Modeling.ClosureType.RemoveClosure:
					return ORMMetaModel.RemoveClosure;
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
				if (ORMMetaModel.copyClosureMember == null)
				{
					ORMMetaModel.copyClosureMember = new ORMMetaModelCopyClosure();
				}
				return ORMMetaModel.copyClosureMember;
			}
		}
		/// <summary>
		/// RemoveClosure cache
		/// </summary>
		private static Microsoft.VisualStudio.Modeling.IElementVisitorFilter RemoveClosure
		{
			get
			{
				if (ORMMetaModel.removeClosureMember == null)
				{
					ORMMetaModel.removeClosureMember = new ORMMetaModelRemoveClosure();
				}
				return ORMMetaModel.removeClosureMember;
			}
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ORMModel.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ORMModel")]
	public  partial class ORMModel : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ORMModel's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "3f4ba8fa-355b-4a49-a42c-9c56f6e5f242";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ORMModel.MetaClassGuidString);
		#endregion

		#region ObjectTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection ObjectTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region FactTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection FactTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasFactType.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region ErrorCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ModelErrorMoveableCollection ErrorCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ModelErrorMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasError.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid); }
		}
		#endregion
		#region ReferenceModeKindCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReferenceModeKindMoveableCollection ReferenceModeKindCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReferenceModeKindMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid); }
		}
		#endregion
		#region ReferenceModeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReferenceModeMoveableCollection ReferenceModeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReferenceModeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid); }
		}
		#endregion
		#region SingleColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection SingleColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
		#region MultiColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection MultiColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region ORMModel's Generated Constructor Code
	public  partial class ORMModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMModel(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ORMModel CreateORMModel(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ORMModel)store.ElementFactory.CreateElement(typeof(ORMModel));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ORMModel CreateAndInitializeORMModel(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ORMModel)store.ElementFactory.CreateElement(typeof(ORMModel), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ORMModel
	/// <summary>
	/// ORMModel Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ORMModel))]
	public sealed class ORMModelElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ORMModelElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ORMModel(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.RootType.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.RootType")]
	public abstract partial class RootType : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region RootType's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c6bdf8b0-ef3d-406a-afb2-8bfeb1f528c3";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.RootType.MetaClassGuidString);
		#endregion

		#region IsExternal's Generated  Field Code
		#region IsExternal's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsExternalMetaAttributeGuidString = "b62dffb1-d93c-4b77-901a-ad7f0920141a";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsExternalMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.RootType.IsExternalMetaAttributeGuidString);
		#endregion

		#region IsExternal's Generated Property Code

		private System.Boolean isExternalPropertyStorage = false;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(RootTypeIsExternalFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.RootType.IsExternalMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.RootType.IsExternal")]
		public  System.Boolean IsExternal
		{
			get
			{
				return isExternalPropertyStorage;
			}
		
			set
			{
				rootTypeIsExternalFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region RootTypeIsExternalFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for RootType.IsExternal field
		/// </summary>
		private static RootTypeIsExternalFieldHandler	rootTypeIsExternalFieldHandler	= RootTypeIsExternalFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for RootType.IsExternal
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class RootTypeIsExternalFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.RootType,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private RootTypeIsExternalFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the RootType.IsExternal field handler
			/// </summary>
			/// <value>RootTypeIsExternalFieldHandler</value>
			public static RootTypeIsExternalFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.RootType.rootTypeIsExternalFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.RootType.rootTypeIsExternalFieldHandler;
					}
					else
					{
						// The static constructor in RootType will assign this value to
						// Northface.Tools.ORM.ObjectModel.RootType.rootTypeIsExternalFieldHandler, so just instantiate one and return it
						return new RootTypeIsExternalFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the RootType.IsExternal field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.RootType.IsExternalMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the RootType</param>
			protected sealed override System.Boolean GetValue(Northface.Tools.ORM.ObjectModel.RootType element)
			{
				return element.isExternalPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.RootType element, System.Boolean value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Boolean oldValue)
			{
				oldValue = element.isExternalPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.isExternalPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region SuperTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RootTypeMoveableCollection SuperTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RootTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.TypeHasSubType.SubTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.TypeHasSubType.SuperTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region SubTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RootTypeMoveableCollection SubTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RootTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.TypeHasSubType.SuperTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.TypeHasSubType.SubTypeCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for RootType
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.RootType Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class RootTypeMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public RootTypeMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.RootType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.RootType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.RootType[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.RootType at that index</returns>
		public Northface.Tools.ORM.ObjectModel.RootType this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.RootType)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.RootType to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.RootType value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.RootType to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.RootType value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.RootType to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.RootType value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.RootType to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.RootType value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.RootType to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.RootType value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.RootType rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.RootType rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region RootType's Generated Constructor Code
	public abstract partial class RootType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected RootType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType")]
	public  partial class ObjectType : Northface.Tools.ORM.ObjectModel.RootType
	{
		#region ObjectType's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "910a08ef-e1a5-461a-bebd-150932f12aad";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.MetaClassGuidString);
		#endregion

		#region IsIndependent's Generated  Field Code
		#region IsIndependent's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsIndependentMetaAttributeGuidString = "9d618c71-7721-41bd-a1f3-dc321a7960ec";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsIndependentMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.IsIndependentMetaAttributeGuidString);
		#endregion

		#region IsIndependent's Generated Property Code

		private System.Boolean isIndependentPropertyStorage = false;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ObjectTypeIsIndependentFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.IsIndependentMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.IsIndependent")]
		public  System.Boolean IsIndependent
		{
			get
			{
				return isIndependentPropertyStorage;
			}
		
			set
			{
				objectTypeIsIndependentFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeIsIndependentFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.IsIndependent field
		/// </summary>
		private static ObjectTypeIsIndependentFieldHandler	objectTypeIsIndependentFieldHandler	= ObjectTypeIsIndependentFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.IsIndependent
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeIsIndependentFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeIsIndependentFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.IsIndependent field handler
			/// </summary>
			/// <value>ObjectTypeIsIndependentFieldHandler</value>
			public static ObjectTypeIsIndependentFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsIndependentFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsIndependentFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsIndependentFieldHandler, so just instantiate one and return it
						return new ObjectTypeIsIndependentFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.IsIndependent field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.IsIndependentMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ObjectType</param>
			protected sealed override System.Boolean GetValue(Northface.Tools.ORM.ObjectModel.ObjectType element)
			{
				return element.isIndependentPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ObjectType element, System.Boolean value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Boolean oldValue)
			{
				oldValue = element.isIndependentPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.isIndependentPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region IsValueType's Generated  Field Code
		#region IsValueType's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsValueTypeMetaAttributeGuidString = "08503b2b-de15-4682-8ac5-972708b73d8a";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsValueTypeMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.IsValueTypeMetaAttributeGuidString);
		#endregion

		#region IsValueType's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.ReadOnly(State=Microsoft.VisualStudio.Modeling.ReadOnlyAttributeValue.SometimesUIReadOnlyPreferFalse)]
		[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ObjectTypeIsValueTypeFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.IsValueTypeMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.IsValueType")]
		public  System.Boolean IsValueType
		{
			get
			{
				return objectTypeIsValueTypeFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeIsValueTypeFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeIsValueTypeFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.IsValueType field
		/// </summary>
		private static ObjectTypeIsValueTypeFieldHandler	objectTypeIsValueTypeFieldHandler	= ObjectTypeIsValueTypeFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.IsValueType
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeIsValueTypeFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeIsValueTypeFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.IsValueType field handler
			/// </summary>
			/// <value>ObjectTypeIsValueTypeFieldHandler</value>
			public static ObjectTypeIsValueTypeFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsValueTypeFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsValueTypeFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeIsValueTypeFieldHandler, so just instantiate one and return it
						return new ObjectTypeIsValueTypeFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.IsValueType field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.IsValueTypeMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region Scale's Generated  Field Code
		#region Scale's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ScaleMetaAttributeGuidString = "5d7a6975-eff0-4832-bc3c-bc6fb3b0e9cf";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ScaleMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.ScaleMetaAttributeGuidString);
		#endregion

		#region Scale's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.IntegerDomainAttribute(MinValue=0)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ObjectTypeScaleFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.ScaleMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.Scale")]
		public  System.Int32 Scale
		{
			get
			{
				return objectTypeScaleFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeScaleFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeScaleFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.Scale field
		/// </summary>
		private static ObjectTypeScaleFieldHandler	objectTypeScaleFieldHandler	= ObjectTypeScaleFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.Scale
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeScaleFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.Int32>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeScaleFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.Scale field handler
			/// </summary>
			/// <value>ObjectTypeScaleFieldHandler</value>
			public static ObjectTypeScaleFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeScaleFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeScaleFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeScaleFieldHandler, so just instantiate one and return it
						return new ObjectTypeScaleFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.Scale field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.ScaleMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region Length's Generated  Field Code
		#region Length's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String LengthMetaAttributeGuidString = "f3e0995d-b18e-4c42-aedd-28de749b6abd";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid LengthMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.LengthMetaAttributeGuidString);
		#endregion

		#region Length's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.IntegerDomainAttribute(MinValue=0)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ObjectTypeLengthFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.LengthMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.Length")]
		public  System.Int32 Length
		{
			get
			{
				return objectTypeLengthFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeLengthFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeLengthFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.Length field
		/// </summary>
		private static ObjectTypeLengthFieldHandler	objectTypeLengthFieldHandler	= ObjectTypeLengthFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.Length
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeLengthFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.Int32>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeLengthFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.Length field handler
			/// </summary>
			/// <value>ObjectTypeLengthFieldHandler</value>
			public static ObjectTypeLengthFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeLengthFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeLengthFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeLengthFieldHandler, so just instantiate one and return it
						return new ObjectTypeLengthFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.Length field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.LengthMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region TypeName's Generated  Field Code
		#region TypeName's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String TypeNameMetaAttributeGuidString = "e73bc8c8-8a49-41dc-84cf-e13521ee476a";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid TypeNameMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.TypeNameMetaAttributeGuidString);
		#endregion

		#region TypeName's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(XPathExpression="/ObjectType/ValueTypeHasDataType.DataType/DataType", RealAttributeName="Name", ProxyAttributeName="Name", FieldHandlerType=typeof(ObjectTypeTypeNameFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.TypeNameMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.TypeName")]
		public  System.String TypeName
		{
			get
			{
				return objectTypeTypeNameFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeTypeNameFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeTypeNameFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.TypeName field
		/// </summary>
		private static ObjectTypeTypeNameFieldHandler	objectTypeTypeNameFieldHandler	= ObjectTypeTypeNameFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.TypeName
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeTypeNameFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementPassThroughFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeTypeNameFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.TypeName field handler
			/// </summary>
			/// <value>ObjectTypeTypeNameFieldHandler</value>
			public static ObjectTypeTypeNameFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeTypeNameFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeTypeNameFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeTypeNameFieldHandler, so just instantiate one and return it
						return new ObjectTypeTypeNameFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.TypeName field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.TypeNameMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region NestedFactTypeDisplay's Generated  Field Code
		#region NestedFactTypeDisplay's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String NestedFactTypeDisplayMetaAttributeGuidString = "d94fc8f5-22f2-4864-9be1-7eb74ba9193b";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid NestedFactTypeDisplayMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.NestedFactTypeDisplayMetaAttributeGuidString);
		#endregion

		#region NestedFactTypeDisplay's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Editor(typeof(Northface.Tools.ORM.ObjectModel.Editors.NestedFactTypePicker), typeof(System.Drawing.Design.UITypeEditor))]
		[System.ComponentModel.MergableProperty(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, AllowNulls=true, FieldHandlerType=typeof(ObjectTypeNestedFactTypeDisplayFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.NestedFactTypeDisplayMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.NestedFactTypeDisplay")]
		public  Northface.Tools.ORM.ObjectModel.FactType NestedFactTypeDisplay
		{
			get
			{
				return objectTypeNestedFactTypeDisplayFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeNestedFactTypeDisplayFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeNestedFactTypeDisplayFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.NestedFactTypeDisplay field
		/// </summary>
		private static ObjectTypeNestedFactTypeDisplayFieldHandler	objectTypeNestedFactTypeDisplayFieldHandler	= ObjectTypeNestedFactTypeDisplayFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.NestedFactTypeDisplay
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeNestedFactTypeDisplayFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,Northface.Tools.ORM.ObjectModel.FactType>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeNestedFactTypeDisplayFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.NestedFactTypeDisplay field handler
			/// </summary>
			/// <value>ObjectTypeNestedFactTypeDisplayFieldHandler</value>
			public static ObjectTypeNestedFactTypeDisplayFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeNestedFactTypeDisplayFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeNestedFactTypeDisplayFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeNestedFactTypeDisplayFieldHandler, so just instantiate one and return it
						return new ObjectTypeNestedFactTypeDisplayFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.NestedFactTypeDisplay field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.NestedFactTypeDisplayMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region ReferenceModeDisplay's Generated  Field Code
		#region ReferenceModeDisplay's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ReferenceModeDisplayMetaAttributeGuidString = "4273a21b-afa2-4ba8-bdba-179b578a11b5";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeDisplayMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectType.ReferenceModeDisplayMetaAttributeGuidString);
		#endregion

		#region ReferenceModeDisplay's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ObjectTypeReferenceModeDisplayFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectType.ReferenceModeDisplayMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ObjectType.ReferenceModeDisplay")]
		public  System.String ReferenceModeDisplay
		{
			get
			{
				return objectTypeReferenceModeDisplayFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				objectTypeReferenceModeDisplayFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ObjectTypeReferenceModeDisplayFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ObjectType.ReferenceModeDisplay field
		/// </summary>
		private static ObjectTypeReferenceModeDisplayFieldHandler	objectTypeReferenceModeDisplayFieldHandler	= ObjectTypeReferenceModeDisplayFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ObjectType.ReferenceModeDisplay
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ObjectTypeReferenceModeDisplayFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ObjectType,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ObjectTypeReferenceModeDisplayFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ObjectType.ReferenceModeDisplay field handler
			/// </summary>
			/// <value>ObjectTypeReferenceModeDisplayFieldHandler</value>
			public static ObjectTypeReferenceModeDisplayFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeReferenceModeDisplayFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeReferenceModeDisplayFieldHandler;
					}
					else
					{
						// The static constructor in ObjectType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ObjectType.objectTypeReferenceModeDisplayFieldHandler, so just instantiate one and return it
						return new ObjectTypeReferenceModeDisplayFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ObjectType.ReferenceModeDisplay field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ObjectType.ReferenceModeDisplayMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region DataType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.DataType DataType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.DataType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType), newRoles);
				}
			}
		}
		#endregion
		#region NestedFactType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactType NestedFactType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType), newRoles);
				}
			}
		}
		#endregion
		#region PlayedRoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection PlayedRoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayerMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid); }
		}
		#endregion
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasObjectType), newRoles);
				}
			}
		}
		#endregion
		#region DuplicateNameError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError DuplicateNameError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError), newRoles);
				}
			}
		}
		#endregion
		#region PreferredIdentifier's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence PreferredIdentifier
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ObjectType
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ObjectType Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ObjectTypeMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ObjectTypeMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ObjectType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ObjectType[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ObjectType at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ObjectType this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ObjectType to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ObjectType value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ObjectType to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ObjectType value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ObjectType to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ObjectType value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ObjectType to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ObjectType value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ObjectType to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ObjectType value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ObjectType rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ObjectType rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ObjectType's Generated Constructor Code
	public  partial class ObjectType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectType CreateObjectType(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ObjectType)store.ElementFactory.CreateElement(typeof(ObjectType));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectType CreateAndInitializeObjectType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectType)store.ElementFactory.CreateElement(typeof(ObjectType), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectType
	/// <summary>
	/// ObjectType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ObjectType))]
	public sealed class ObjectTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ObjectType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactType.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.FactType")]
	public  partial class FactType : Northface.Tools.ORM.ObjectModel.RootType
	{
		#region FactType's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "a3acc35a-ce71-4cb8-8770-b87fbfa462d8";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactType.MetaClassGuidString);
		#endregion

		#region NestingTypeDisplay's Generated  Field Code
		#region NestingTypeDisplay's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String NestingTypeDisplayMetaAttributeGuidString = "b4a12078-45aa-4cab-a6d9-da2317ddc64a";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid NestingTypeDisplayMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactType.NestingTypeDisplayMetaAttributeGuidString);
		#endregion

		#region NestingTypeDisplay's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Editor(typeof(Northface.Tools.ORM.ObjectModel.Editors.NestingTypePicker), typeof(System.Drawing.Design.UITypeEditor))]
		[System.ComponentModel.MergableProperty(false)]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, AllowNulls=true, FieldHandlerType=typeof(FactTypeNestingTypeDisplayFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactType.NestingTypeDisplayMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.FactType.NestingTypeDisplay")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType NestingTypeDisplay
		{
			get
			{
				return factTypeNestingTypeDisplayFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				factTypeNestingTypeDisplayFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region FactTypeNestingTypeDisplayFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for FactType.NestingTypeDisplay field
		/// </summary>
		private static FactTypeNestingTypeDisplayFieldHandler	factTypeNestingTypeDisplayFieldHandler	= FactTypeNestingTypeDisplayFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for FactType.NestingTypeDisplay
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class FactTypeNestingTypeDisplayFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.FactType,Northface.Tools.ORM.ObjectModel.ObjectType>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private FactTypeNestingTypeDisplayFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the FactType.NestingTypeDisplay field handler
			/// </summary>
			/// <value>FactTypeNestingTypeDisplayFieldHandler</value>
			public static FactTypeNestingTypeDisplayFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.FactType.factTypeNestingTypeDisplayFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.FactType.factTypeNestingTypeDisplayFieldHandler;
					}
					else
					{
						// The static constructor in FactType will assign this value to
						// Northface.Tools.ORM.ObjectModel.FactType.factTypeNestingTypeDisplayFieldHandler, so just instantiate one and return it
						return new FactTypeNestingTypeDisplayFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the FactType.NestingTypeDisplay field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.FactType.NestingTypeDisplayMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region DerivationRule's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.Expression DerivationRule
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.Expression)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactTypeMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeDerivation), newRoles);
				}
			}
		}
		#endregion
		#region NestingType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectType NestingType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ObjectType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType), newRoles);
				}
			}
		}
		#endregion
		#region RoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection RoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid); }
		}
		#endregion
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasFactType.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasFactType.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasFactType), newRoles);
				}
			}
		}
		#endregion
		#region MultiColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection MultiColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
		#region SingleColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection SingleColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
		#region DuplicateNameError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError DuplicateNameError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError), newRoles);
				}
			}
		}
		#endregion
		#region ReadingOrderCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReadingOrderMoveableCollection ReadingOrderCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReadingOrderMoveableCollection(this, Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid); }
		}
		#endregion
		#region InternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.InternalConstraintMoveableCollection InternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.InternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for FactType
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.FactType Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class FactTypeMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public FactTypeMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.FactType) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.FactType))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.FactType[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.FactType at that index</returns>
		public Northface.Tools.ORM.ObjectModel.FactType this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.FactType to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.FactType value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.FactType to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.FactType value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.FactType to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.FactType value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.FactType to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.FactType value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.FactType to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.FactType value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.FactType rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.FactType rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region FactType's Generated Constructor Code
	public  partial class FactType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactType CreateFactType(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (FactType)store.ElementFactory.CreateElement(typeof(FactType));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactType CreateAndInitializeFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactType)store.ElementFactory.CreateElement(typeof(FactType), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactType
	/// <summary>
	/// FactType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactType))]
	public sealed class FactTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint")]
	public abstract partial class MultiColumnExternalConstraint : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region MultiColumnExternalConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "0d75242d-1462-4936-ad30-e1ce61a6ba3f";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint.MetaClassGuidString);
		#endregion

		#region FactTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection FactTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region RoleSequenceCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequenceMoveableCollection RoleSequenceCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequenceMoveableCollection(this, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuid, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid); }
		}
		#endregion
		#region TooFewRoleSequencesError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError TooFewRoleSequencesError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError), newRoles);
				}
			}
		}
		#endregion
		#region TooManyRoleSequencesError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError TooManyRoleSequencesError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError), newRoles);
				}
			}
		}
		#endregion
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint), newRoles);
				}
			}
		}
		#endregion
		#region DuplicateNameError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError DuplicateNameError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for MultiColumnExternalConstraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class MultiColumnExternalConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public MultiColumnExternalConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region MultiColumnExternalConstraint's Generated Constructor Code
	public abstract partial class MultiColumnExternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected MultiColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EqualityConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.EqualityConstraint")]
	public  partial class EqualityConstraint : Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint
	{
		#region EqualityConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "1bee2436-cd79-45be-b484-9c1dc5cb81ab";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EqualityConstraint.MetaClassGuidString);
		#endregion

	}
	#region EqualityConstraint's Generated Constructor Code
	public  partial class EqualityConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public EqualityConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static EqualityConstraint CreateEqualityConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (EqualityConstraint)store.ElementFactory.CreateElement(typeof(EqualityConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static EqualityConstraint CreateAndInitializeEqualityConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (EqualityConstraint)store.ElementFactory.CreateElement(typeof(EqualityConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for EqualityConstraint
	/// <summary>
	/// EqualityConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.EqualityConstraint))]
	public sealed class EqualityConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public EqualityConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.EqualityConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExclusionConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ExclusionConstraint")]
	public  partial class ExclusionConstraint : Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint
	{
		#region ExclusionConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "5373783f-9ddd-4ffb-95d8-8e28cd73b98a";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExclusionConstraint.MetaClassGuidString);
		#endregion

	}
	#region ExclusionConstraint's Generated Constructor Code
	public  partial class ExclusionConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExclusionConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExclusionConstraint CreateExclusionConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ExclusionConstraint)store.ElementFactory.CreateElement(typeof(ExclusionConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExclusionConstraint CreateAndInitializeExclusionConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExclusionConstraint)store.ElementFactory.CreateElement(typeof(ExclusionConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExclusionConstraint
	/// <summary>
	/// ExclusionConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExclusionConstraint))]
	public sealed class ExclusionConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExclusionConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExclusionConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SubsetConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.SubsetConstraint")]
	public  partial class SubsetConstraint : Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint
	{
		#region SubsetConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "7ac282bf-98d2-4ad1-9bf0-5ec0cdb17d95";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SubsetConstraint.MetaClassGuidString);
		#endregion

	}
	#region SubsetConstraint's Generated Constructor Code
	public  partial class SubsetConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SubsetConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SubsetConstraint CreateSubsetConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (SubsetConstraint)store.ElementFactory.CreateElement(typeof(SubsetConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SubsetConstraint CreateAndInitializeSubsetConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (SubsetConstraint)store.ElementFactory.CreateElement(typeof(SubsetConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for SubsetConstraint
	/// <summary>
	/// SubsetConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.SubsetConstraint))]
	public sealed class SubsetConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SubsetConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.SubsetConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.DataType.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.DataType")]
	public  partial class DataType : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region DataType's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "547d17bb-3dc9-4869-b280-ff6be0f7dca9";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.DataType.MetaClassGuidString);
		#endregion

		#region ValueTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection ValueTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region DataType's Generated Constructor Code
	public  partial class DataType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DataType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static DataType CreateDataType(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (DataType)store.ElementFactory.CreateElement(typeof(DataType));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static DataType CreateAndInitializeDataType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (DataType)store.ElementFactory.CreateElement(typeof(DataType), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for DataType
	/// <summary>
	/// DataType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.DataType))]
	public sealed class DataTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DataTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.DataType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Expression.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.Expression")]
	public  partial class Expression : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region Expression's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "ddc5ef24-70bb-48aa-8481-2a76021581a3";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Expression.MetaClassGuidString);
		#endregion

		#region Body's Generated  Field Code
		#region Body's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String BodyMetaAttributeGuidString = "75d72ae2-bc07-493b-a4d1-9b9b1f2e1dd1";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid BodyMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Expression.BodyMetaAttributeGuidString);
		#endregion

		#region Body's Generated Property Code

		private System.String bodyPropertyStorage = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ExpressionBodyFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Expression.BodyMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Expression.Body")]
		public  System.String Body
		{
			get
			{
				return bodyPropertyStorage;
			}
		
			set
			{
				expressionBodyFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ExpressionBodyFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Expression.Body field
		/// </summary>
		private static ExpressionBodyFieldHandler	expressionBodyFieldHandler	= ExpressionBodyFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Expression.Body
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ExpressionBodyFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.Expression,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ExpressionBodyFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Expression.Body field handler
			/// </summary>
			/// <value>ExpressionBodyFieldHandler</value>
			public static ExpressionBodyFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Expression.expressionBodyFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Expression.expressionBodyFieldHandler;
					}
					else
					{
						// The static constructor in Expression will assign this value to
						// Northface.Tools.ORM.ObjectModel.Expression.expressionBodyFieldHandler, so just instantiate one and return it
						return new ExpressionBodyFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Expression.Body field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Expression.BodyMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the Expression</param>
			protected sealed override System.String GetValue(Northface.Tools.ORM.ObjectModel.Expression element)
			{
				return element.bodyPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.Expression element, System.String value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.String oldValue)
			{
				oldValue = element.bodyPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.bodyPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region Language's Generated  Field Code
		#region Language's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String LanguageMetaAttributeGuidString = "a1dc9629-53d7-48fe-84e2-98423f1d292e";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid LanguageMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Expression.LanguageMetaAttributeGuidString);
		#endregion

		#region Language's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ExpressionLanguageFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Expression.LanguageMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Expression.Language")]
		public  System.String Language
		{
			get
			{
				return expressionLanguageFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				expressionLanguageFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ExpressionLanguageFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Expression.Language field
		/// </summary>
		private static ExpressionLanguageFieldHandler	expressionLanguageFieldHandler	= ExpressionLanguageFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Expression.Language
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ExpressionLanguageFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.Expression,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ExpressionLanguageFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Expression.Language field handler
			/// </summary>
			/// <value>ExpressionLanguageFieldHandler</value>
			public static ExpressionLanguageFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Expression.expressionLanguageFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Expression.expressionLanguageFieldHandler;
					}
					else
					{
						// The static constructor in Expression will assign this value to
						// Northface.Tools.ORM.ObjectModel.Expression.expressionLanguageFieldHandler, so just instantiate one and return it
						return new ExpressionLanguageFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Expression.Language field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Expression.LanguageMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
	}
	#region Expression's Generated Constructor Code
	public  partial class Expression
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Expression(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Expression CreateExpression(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (Expression)store.ElementFactory.CreateElement(typeof(Expression));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Expression CreateAndInitializeExpression(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (Expression)store.ElementFactory.CreateElement(typeof(Expression), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for Expression
	/// <summary>
	/// Expression Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.Expression))]
	public sealed class ExpressionElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExpressionElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.Expression(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Role.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.Role")]
	public  partial class Role : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region Role's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "393d854f-34c5-4dc5-86c6-56816581b957";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Role.MetaClassGuidString);
		#endregion

		#region RolePlayerDisplay's Generated  Field Code
		#region RolePlayerDisplay's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String RolePlayerDisplayMetaAttributeGuidString = "b892ba33-3d6d-41e0-98a1-54ef51a7ed57";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid RolePlayerDisplayMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Role.RolePlayerDisplayMetaAttributeGuidString);
		#endregion

		#region RolePlayerDisplay's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Editor(typeof(Northface.Tools.ORM.ObjectModel.Editors.RolePlayerPicker), typeof(System.Drawing.Design.UITypeEditor))]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, AllowNulls=true, FieldHandlerType=typeof(RoleRolePlayerDisplayFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Role.RolePlayerDisplayMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Role.RolePlayerDisplay")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType RolePlayerDisplay
		{
			get
			{
				return roleRolePlayerDisplayFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				roleRolePlayerDisplayFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region RoleRolePlayerDisplayFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Role.RolePlayerDisplay field
		/// </summary>
		private static RoleRolePlayerDisplayFieldHandler	roleRolePlayerDisplayFieldHandler	= RoleRolePlayerDisplayFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Role.RolePlayerDisplay
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class RoleRolePlayerDisplayFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.Role,Northface.Tools.ORM.ObjectModel.ObjectType>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private RoleRolePlayerDisplayFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Role.RolePlayerDisplay field handler
			/// </summary>
			/// <value>RoleRolePlayerDisplayFieldHandler</value>
			public static RoleRolePlayerDisplayFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Role.roleRolePlayerDisplayFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Role.roleRolePlayerDisplayFieldHandler;
					}
					else
					{
						// The static constructor in Role will assign this value to
						// Northface.Tools.ORM.ObjectModel.Role.roleRolePlayerDisplayFieldHandler, so just instantiate one and return it
						return new RoleRolePlayerDisplayFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Role.RolePlayerDisplay field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Role.RolePlayerDisplayMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region IsMandatory's Generated  Field Code
		#region IsMandatory's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsMandatoryMetaAttributeGuidString = "d2d267cb-547f-441e-94ab-730d1138672c";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsMandatoryMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Role.IsMandatoryMetaAttributeGuidString);
		#endregion

		#region IsMandatory's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(RoleIsMandatoryFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Role.IsMandatoryMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Role.IsMandatory")]
		public  System.Boolean IsMandatory
		{
			get
			{
				return roleIsMandatoryFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				roleIsMandatoryFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region RoleIsMandatoryFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Role.IsMandatory field
		/// </summary>
		private static RoleIsMandatoryFieldHandler	roleIsMandatoryFieldHandler	= RoleIsMandatoryFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Role.IsMandatory
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class RoleIsMandatoryFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.Role,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private RoleIsMandatoryFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Role.IsMandatory field handler
			/// </summary>
			/// <value>RoleIsMandatoryFieldHandler</value>
			public static RoleIsMandatoryFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Role.roleIsMandatoryFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Role.roleIsMandatoryFieldHandler;
					}
					else
					{
						// The static constructor in Role will assign this value to
						// Northface.Tools.ORM.ObjectModel.Role.roleIsMandatoryFieldHandler, so just instantiate one and return it
						return new RoleIsMandatoryFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Role.IsMandatory field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Role.IsMandatoryMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region Multiplicity's Generated  Field Code
		#region Multiplicity's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String MultiplicityMetaAttributeGuidString = "91159767-f6ce-4591-aeb2-ff0ab25f1b44";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid MultiplicityMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Role.MultiplicityMetaAttributeGuidString);
		#endregion

		#region Multiplicity's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.MergableProperty(false)]
		[Microsoft.VisualStudio.Modeling.EnumerationDomainAttribute(EnumerationType=typeof(Northface.Tools.ORM.ObjectModel.RoleMultiplicity),DefaultEnumerationValueName="Unspecified")]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(RoleMultiplicityFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Role.MultiplicityMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Role.Multiplicity")]
		public  Northface.Tools.ORM.ObjectModel.RoleMultiplicity Multiplicity
		{
			get
			{
				return roleMultiplicityFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				roleMultiplicityFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region RoleMultiplicityFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Role.Multiplicity field
		/// </summary>
		private static RoleMultiplicityFieldHandler	roleMultiplicityFieldHandler	= RoleMultiplicityFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Role.Multiplicity
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class RoleMultiplicityFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.Role,Northface.Tools.ORM.ObjectModel.RoleMultiplicity>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private RoleMultiplicityFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Role.Multiplicity field handler
			/// </summary>
			/// <value>RoleMultiplicityFieldHandler</value>
			public static RoleMultiplicityFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Role.roleMultiplicityFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Role.roleMultiplicityFieldHandler;
					}
					else
					{
						// The static constructor in Role will assign this value to
						// Northface.Tools.ORM.ObjectModel.Role.roleMultiplicityFieldHandler, so just instantiate one and return it
						return new RoleMultiplicityFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Role.Multiplicity field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Role.MultiplicityMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region FactType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasRole), newRoles);
				}
			}
		}
		#endregion
		#region RolePlayer's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectType RolePlayer
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayerMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ObjectType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayerMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole), newRoles);
				}
			}
		}
		#endregion
		#region ConstraintRoleSequenceCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceMoveableCollection ConstraintRoleSequenceCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for Role
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.Role Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class RoleMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public RoleMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Role) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Role))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.Role[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.Role at that index</returns>
		public Northface.Tools.ORM.ObjectModel.Role this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Role to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.Role value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Role to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.Role value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Role to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.Role value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Role to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.Role value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Role to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.Role value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.Role rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.Role rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region Role's Generated Constructor Code
	public  partial class Role
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Role(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Role CreateRole(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (Role)store.ElementFactory.CreateElement(typeof(Role));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Role CreateAndInitializeRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (Role)store.ElementFactory.CreateElement(typeof(Role), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for Role
	/// <summary>
	/// Role Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.Role))]
	public sealed class RoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.Role(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence")]
	public abstract partial class ConstraintRoleSequence : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ConstraintRoleSequence's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "d9ab8420-8604-4016-8827-0d59c77b47f8";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence.MetaClassGuidString);
		#endregion

		#region RoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection RoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid); }
		}
		#endregion
		#region PreferredIdentifierFor's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectType PreferredIdentifierFor
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ObjectType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ConstraintRoleSequence
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ConstraintRoleSequenceMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ConstraintRoleSequenceMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ConstraintRoleSequence's Generated Constructor Code
	public abstract partial class ConstraintRoleSequence
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ConstraintRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.InternalConstraint")]
	public abstract partial class InternalConstraint : Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence
	{
		#region InternalConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "e17dcd50-eefe-4960-b425-b87f246c6ea0";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalConstraint.MetaClassGuidString);
		#endregion

		#region FactType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for InternalConstraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.InternalConstraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class InternalConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public InternalConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.InternalConstraint[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.InternalConstraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.InternalConstraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.InternalConstraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.InternalConstraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.InternalConstraint value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.InternalConstraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.InternalConstraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.InternalConstraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.InternalConstraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.InternalConstraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.InternalConstraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.InternalConstraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.InternalConstraint value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.InternalConstraint rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.InternalConstraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region InternalConstraint's Generated Constructor Code
	public abstract partial class InternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected InternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SimpleMandatoryConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.SimpleMandatoryConstraint")]
	public  partial class SimpleMandatoryConstraint : Northface.Tools.ORM.ObjectModel.InternalConstraint
	{
		#region SimpleMandatoryConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "b09e7a1f-184e-4e7f-88e3-6ca763ded9eb";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SimpleMandatoryConstraint.MetaClassGuidString);
		#endregion

	}
	#region SimpleMandatoryConstraint's Generated Constructor Code
	public  partial class SimpleMandatoryConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SimpleMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SimpleMandatoryConstraint CreateSimpleMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (SimpleMandatoryConstraint)store.ElementFactory.CreateElement(typeof(SimpleMandatoryConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SimpleMandatoryConstraint CreateAndInitializeSimpleMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (SimpleMandatoryConstraint)store.ElementFactory.CreateElement(typeof(SimpleMandatoryConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for SimpleMandatoryConstraint
	/// <summary>
	/// SimpleMandatoryConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.SimpleMandatoryConstraint))]
	public sealed class SimpleMandatoryConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SimpleMandatoryConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.SimpleMandatoryConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint")]
	public  partial class InternalUniquenessConstraint : Northface.Tools.ORM.ObjectModel.InternalConstraint
	{
		#region InternalUniquenessConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "9480cf96-adfd-45b6-967e-018dddf27b94";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.MetaClassGuidString);
		#endregion

		#region IsPreferred's Generated  Field Code
		#region IsPreferred's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsPreferredMetaAttributeGuidString = "c1ba5d02-a7e4-4cd5-ac9f-89ee1d811ee8";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsPreferredMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.IsPreferredMetaAttributeGuidString);
		#endregion

		#region IsPreferred's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.ReadOnly(State=Microsoft.VisualStudio.Modeling.ReadOnlyAttributeValue.SometimesUIReadOnlyPreferTrue)]
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(InternalUniquenessConstraintIsPreferredFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.IsPreferredMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.IsPreferred")]
		public  System.Boolean IsPreferred
		{
			get
			{
				return internalUniquenessConstraintIsPreferredFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				internalUniquenessConstraintIsPreferredFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region InternalUniquenessConstraintIsPreferredFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for InternalUniquenessConstraint.IsPreferred field
		/// </summary>
		private static InternalUniquenessConstraintIsPreferredFieldHandler	internalUniquenessConstraintIsPreferredFieldHandler	= InternalUniquenessConstraintIsPreferredFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for InternalUniquenessConstraint.IsPreferred
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class InternalUniquenessConstraintIsPreferredFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private InternalUniquenessConstraintIsPreferredFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the InternalUniquenessConstraint.IsPreferred field handler
			/// </summary>
			/// <value>InternalUniquenessConstraintIsPreferredFieldHandler</value>
			public static InternalUniquenessConstraintIsPreferredFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.internalUniquenessConstraintIsPreferredFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.internalUniquenessConstraintIsPreferredFieldHandler;
					}
					else
					{
						// The static constructor in InternalUniquenessConstraint will assign this value to
						// Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.internalUniquenessConstraintIsPreferredFieldHandler, so just instantiate one and return it
						return new InternalUniquenessConstraintIsPreferredFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the InternalUniquenessConstraint.IsPreferred field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint.IsPreferredMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
	}
	#region InternalUniquenessConstraint's Generated Constructor Code
	public  partial class InternalUniquenessConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalUniquenessConstraint CreateInternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (InternalUniquenessConstraint)store.ElementFactory.CreateElement(typeof(InternalUniquenessConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalUniquenessConstraint CreateAndInitializeInternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (InternalUniquenessConstraint)store.ElementFactory.CreateElement(typeof(InternalUniquenessConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for InternalUniquenessConstraint
	/// <summary>
	/// InternalUniquenessConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint))]
	public sealed class InternalUniquenessConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalUniquenessConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.InternalUniquenessConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FrequencyConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.FrequencyConstraint")]
	public  partial class FrequencyConstraint : Northface.Tools.ORM.ObjectModel.InternalConstraint
	{
		#region FrequencyConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "cee1eaf8-155f-4646-acb3-bf2e87be032a";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FrequencyConstraint.MetaClassGuidString);
		#endregion

	}
	#region FrequencyConstraint's Generated Constructor Code
	public  partial class FrequencyConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FrequencyConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FrequencyConstraint CreateFrequencyConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (FrequencyConstraint)store.ElementFactory.CreateElement(typeof(FrequencyConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FrequencyConstraint CreateAndInitializeFrequencyConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FrequencyConstraint)store.ElementFactory.CreateElement(typeof(FrequencyConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FrequencyConstraint
	/// <summary>
	/// FrequencyConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FrequencyConstraint))]
	public sealed class FrequencyConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FrequencyConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FrequencyConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence")]
	public  partial class MultiColumnExternalConstraintRoleSequence : Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence
	{
		#region MultiColumnExternalConstraintRoleSequence's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "34a3c709-1021-4afe-80db-6302f4c681ea";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence.MetaClassGuidString);
		#endregion

		#region ExternalConstraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint ExternalConstraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for MultiColumnExternalConstraintRoleSequence
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class MultiColumnExternalConstraintRoleSequenceMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public MultiColumnExternalConstraintRoleSequenceMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence at that index</returns>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region MultiColumnExternalConstraintRoleSequence's Generated Constructor Code
	public  partial class MultiColumnExternalConstraintRoleSequence
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintRoleSequence CreateMultiColumnExternalConstraintRoleSequence(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (MultiColumnExternalConstraintRoleSequence)store.ElementFactory.CreateElement(typeof(MultiColumnExternalConstraintRoleSequence));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintRoleSequence CreateAndInitializeMultiColumnExternalConstraintRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (MultiColumnExternalConstraintRoleSequence)store.ElementFactory.CreateElement(typeof(MultiColumnExternalConstraintRoleSequence), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for MultiColumnExternalConstraintRoleSequence
	/// <summary>
	/// MultiColumnExternalConstraintRoleSequence Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence))]
	public sealed class MultiColumnExternalConstraintRoleSequenceElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintRoleSequenceElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint")]
	public abstract partial class SingleColumnExternalConstraint : Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence
	{
		#region SingleColumnExternalConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "5b304d59-8d34-45c5-80fc-4a0fe122aa0a";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint.MetaClassGuidString);
		#endregion

		#region FactTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection FactTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint), newRoles);
				}
			}
		}
		#endregion
		#region DuplicateNameError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError DuplicateNameError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for SingleColumnExternalConstraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class SingleColumnExternalConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public SingleColumnExternalConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region SingleColumnExternalConstraint's Generated Constructor Code
	public abstract partial class SingleColumnExternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected SingleColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.RingConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.RingConstraint")]
	public  partial class RingConstraint : Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint
	{
		#region RingConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c0f29d7a-f057-49a6-87df-7abcbb55d471";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.RingConstraint.MetaClassGuidString);
		#endregion

	}
	#region RingConstraint's Generated Constructor Code
	public  partial class RingConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RingConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static RingConstraint CreateRingConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (RingConstraint)store.ElementFactory.CreateElement(typeof(RingConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static RingConstraint CreateAndInitializeRingConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (RingConstraint)store.ElementFactory.CreateElement(typeof(RingConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for RingConstraint
	/// <summary>
	/// RingConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.RingConstraint))]
	public sealed class RingConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RingConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.RingConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint")]
	public  partial class ExternalUniquenessConstraint : Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint
	{
		#region ExternalUniquenessConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c50782de-5fd3-4076-a062-dd8983367363";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.MetaClassGuidString);
		#endregion

		#region IsPreferred's Generated  Field Code
		#region IsPreferred's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsPreferredMetaAttributeGuidString = "81ceefc5-e227-4a8e-be95-1a63066bc9b7";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsPreferredMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.IsPreferredMetaAttributeGuidString);
		#endregion

		#region IsPreferred's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.ReadOnly(State=Microsoft.VisualStudio.Modeling.ReadOnlyAttributeValue.SometimesUIReadOnlyPreferTrue)]
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ExternalUniquenessConstraintIsPreferredFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.IsPreferredMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.IsPreferred")]
		public  System.Boolean IsPreferred
		{
			get
			{
				return externalUniquenessConstraintIsPreferredFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				externalUniquenessConstraintIsPreferredFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ExternalUniquenessConstraintIsPreferredFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ExternalUniquenessConstraint.IsPreferred field
		/// </summary>
		private static ExternalUniquenessConstraintIsPreferredFieldHandler	externalUniquenessConstraintIsPreferredFieldHandler	= ExternalUniquenessConstraintIsPreferredFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ExternalUniquenessConstraint.IsPreferred
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ExternalUniquenessConstraintIsPreferredFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ExternalUniquenessConstraintIsPreferredFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ExternalUniquenessConstraint.IsPreferred field handler
			/// </summary>
			/// <value>ExternalUniquenessConstraintIsPreferredFieldHandler</value>
			public static ExternalUniquenessConstraintIsPreferredFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.externalUniquenessConstraintIsPreferredFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.externalUniquenessConstraintIsPreferredFieldHandler;
					}
					else
					{
						// The static constructor in ExternalUniquenessConstraint will assign this value to
						// Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.externalUniquenessConstraintIsPreferredFieldHandler, so just instantiate one and return it
						return new ExternalUniquenessConstraintIsPreferredFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ExternalUniquenessConstraint.IsPreferred field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.IsPreferredMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
	}
	#region ExternalUniquenessConstraint's Generated Constructor Code
	public  partial class ExternalUniquenessConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalUniquenessConstraint CreateExternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ExternalUniquenessConstraint)store.ElementFactory.CreateElement(typeof(ExternalUniquenessConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalUniquenessConstraint CreateAndInitializeExternalUniquenessConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalUniquenessConstraint)store.ElementFactory.CreateElement(typeof(ExternalUniquenessConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalUniquenessConstraint
	/// <summary>
	/// ExternalUniquenessConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint))]
	public sealed class ExternalUniquenessConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalUniquenessConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.DisjunctiveMandatoryConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.DisjunctiveMandatoryConstraint")]
	public  partial class DisjunctiveMandatoryConstraint : Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint
	{
		#region DisjunctiveMandatoryConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "749901ac-66bb-4d86-a42c-127456eb5451";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.DisjunctiveMandatoryConstraint.MetaClassGuidString);
		#endregion

	}
	#region DisjunctiveMandatoryConstraint's Generated Constructor Code
	public  partial class DisjunctiveMandatoryConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DisjunctiveMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static DisjunctiveMandatoryConstraint CreateDisjunctiveMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (DisjunctiveMandatoryConstraint)store.ElementFactory.CreateElement(typeof(DisjunctiveMandatoryConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static DisjunctiveMandatoryConstraint CreateAndInitializeDisjunctiveMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (DisjunctiveMandatoryConstraint)store.ElementFactory.CreateElement(typeof(DisjunctiveMandatoryConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for DisjunctiveMandatoryConstraint
	/// <summary>
	/// DisjunctiveMandatoryConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.DisjunctiveMandatoryConstraint))]
	public sealed class DisjunctiveMandatoryConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DisjunctiveMandatoryConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.DisjunctiveMandatoryConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Reading.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.Reading")]
	public  partial class Reading : Microsoft.VisualStudio.Modeling.ModelElement
	{
		#region Reading's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "cef616d5-3fe8-489e-bd60-3333921b675e";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Reading.MetaClassGuidString);
		#endregion

		#region Text's Generated  Field Code
		#region Text's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String TextMetaAttributeGuidString = "deb9d5ff-64f8-4887-b41a-8ea3689efa4d";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid TextMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Reading.TextMetaAttributeGuidString);
		#endregion

		#region Text's Generated Property Code

		private System.String textPropertyStorage = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ReadingTextFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Reading.TextMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Reading.Text")]
		public  System.String Text
		{
			get
			{
				return textPropertyStorage;
			}
		
			set
			{
				readingTextFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReadingTextFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Reading.Text field
		/// </summary>
		private static ReadingTextFieldHandler	readingTextFieldHandler	= ReadingTextFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Reading.Text
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReadingTextFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.Reading,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReadingTextFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Reading.Text field handler
			/// </summary>
			/// <value>ReadingTextFieldHandler</value>
			public static ReadingTextFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Reading.readingTextFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Reading.readingTextFieldHandler;
					}
					else
					{
						// The static constructor in Reading will assign this value to
						// Northface.Tools.ORM.ObjectModel.Reading.readingTextFieldHandler, so just instantiate one and return it
						return new ReadingTextFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Reading.Text field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Reading.TextMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the Reading</param>
			protected sealed override System.String GetValue(Northface.Tools.ORM.ObjectModel.Reading element)
			{
				return element.textPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.Reading element, System.String value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.String oldValue)
			{
				oldValue = element.textPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.textPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region IsPrimary's Generated  Field Code
		#region IsPrimary's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsPrimaryMetaAttributeGuidString = "fb40e877-7171-473d-88b5-bea06782e468";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsPrimaryMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Reading.IsPrimaryMetaAttributeGuidString);
		#endregion

		#region IsPrimary's Generated Property Code

		private System.Boolean isPrimaryPropertyStorage = false;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.ReadOnly(State=Microsoft.VisualStudio.Modeling.ReadOnlyAttributeValue.SometimesUIReadOnlyPreferFalse)]
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ReadingIsPrimaryFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Reading.IsPrimaryMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Reading.IsPrimary")]
		public  System.Boolean IsPrimary
		{
			get
			{
				return isPrimaryPropertyStorage;
			}
		
			set
			{
				readingIsPrimaryFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReadingIsPrimaryFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Reading.IsPrimary field
		/// </summary>
		private static ReadingIsPrimaryFieldHandler	readingIsPrimaryFieldHandler	= ReadingIsPrimaryFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Reading.IsPrimary
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReadingIsPrimaryFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.Reading,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReadingIsPrimaryFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Reading.IsPrimary field handler
			/// </summary>
			/// <value>ReadingIsPrimaryFieldHandler</value>
			public static ReadingIsPrimaryFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Reading.readingIsPrimaryFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Reading.readingIsPrimaryFieldHandler;
					}
					else
					{
						// The static constructor in Reading will assign this value to
						// Northface.Tools.ORM.ObjectModel.Reading.readingIsPrimaryFieldHandler, so just instantiate one and return it
						return new ReadingIsPrimaryFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Reading.IsPrimary field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Reading.IsPrimaryMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the Reading</param>
			protected sealed override System.Boolean GetValue(Northface.Tools.ORM.ObjectModel.Reading element)
			{
				return element.isPrimaryPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.Reading element, System.Boolean value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Boolean oldValue)
			{
				oldValue = element.isPrimaryPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.isPrimaryPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region Language's Generated  Field Code
		#region Language's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String LanguageMetaAttributeGuidString = "28f89850-a91e-4b64-a917-43a97cfb192e";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid LanguageMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Reading.LanguageMetaAttributeGuidString);
		#endregion

		#region Language's Generated Property Code

		private System.String languagePropertyStorage = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ReadingLanguageFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Reading.LanguageMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.Reading.Language")]
		public  System.String Language
		{
			get
			{
				return languagePropertyStorage;
			}
		
			set
			{
				readingLanguageFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReadingLanguageFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for Reading.Language field
		/// </summary>
		private static ReadingLanguageFieldHandler	readingLanguageFieldHandler	= ReadingLanguageFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for Reading.Language
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReadingLanguageFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.Reading,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReadingLanguageFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the Reading.Language field handler
			/// </summary>
			/// <value>ReadingLanguageFieldHandler</value>
			public static ReadingLanguageFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.Reading.readingLanguageFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.Reading.readingLanguageFieldHandler;
					}
					else
					{
						// The static constructor in Reading will assign this value to
						// Northface.Tools.ORM.ObjectModel.Reading.readingLanguageFieldHandler, so just instantiate one and return it
						return new ReadingLanguageFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the Reading.Language field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.Reading.LanguageMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the Reading</param>
			protected sealed override System.String GetValue(Northface.Tools.ORM.ObjectModel.Reading element)
			{
				return element.languagePropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.Reading element, System.String value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.String oldValue)
			{
				oldValue = element.languagePropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.languagePropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region RoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection RoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ReadingHasRole.ReadingCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReadingHasRole.RoleCollectionMetaRoleGuid); }
		}
		#endregion
		#region ReadingOrder's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReadingOrder ReadingOrder
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrderMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ReadingOrder)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrderMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for Reading
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.Reading Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ReadingMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ReadingMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Reading) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Reading))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.Reading[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.Reading at that index</returns>
		public Northface.Tools.ORM.ObjectModel.Reading this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.Reading)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Reading to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.Reading value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Reading to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.Reading value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Reading to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.Reading value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Reading to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.Reading value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Reading to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.Reading value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.Reading rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.Reading rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region Reading's Generated Constructor Code
	public  partial class Reading
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Reading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Reading CreateReading(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (Reading)store.ElementFactory.CreateElement(typeof(Reading));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static Reading CreateAndInitializeReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (Reading)store.ElementFactory.CreateElement(typeof(Reading), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for Reading
	/// <summary>
	/// Reading Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.Reading))]
	public sealed class ReadingElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.Reading(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ModelError")]
	public abstract partial class ModelError : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ModelError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "50b3feea-fa3a-4c87-bb8c-74b26c12b330";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelError.MetaClassGuidString);
		#endregion

		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasError.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasError.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasError), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ModelError
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ModelError Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ModelErrorMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ModelErrorMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ModelError) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ModelError))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ModelError[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ModelError at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ModelError this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ModelError)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ModelError to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ModelError value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ModelError to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ModelError value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ModelError to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ModelError value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ModelError to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ModelError value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ModelError to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ModelError value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ModelError rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ModelError rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ModelError's Generated Constructor Code
	public abstract partial class ModelError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ModelError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError")]
	public  partial class TooFewRoleSequencesError : Northface.Tools.ORM.ObjectModel.ModelError
	{
		#region TooFewRoleSequencesError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "a2f137b4-5973-4e4f-90aa-6bc936bf8e79";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError.MetaClassGuidString);
		#endregion

		#region Constraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint Constraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError), newRoles);
				}
			}
		}
		#endregion
	}
	#region TooFewRoleSequencesError's Generated Constructor Code
	public  partial class TooFewRoleSequencesError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooFewRoleSequencesError CreateTooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (TooFewRoleSequencesError)store.ElementFactory.CreateElement(typeof(TooFewRoleSequencesError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooFewRoleSequencesError CreateAndInitializeTooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (TooFewRoleSequencesError)store.ElementFactory.CreateElement(typeof(TooFewRoleSequencesError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for TooFewRoleSequencesError
	/// <summary>
	/// TooFewRoleSequencesError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError))]
	public sealed class TooFewRoleSequencesErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooFewRoleSequencesErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError")]
	public  partial class TooManyRoleSequencesError : Northface.Tools.ORM.ObjectModel.ModelError
	{
		#region TooManyRoleSequencesError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c0e9f4b2-c8e8-4126-bde2-ec5de62d2b58";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError.MetaClassGuidString);
		#endregion

		#region Constraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint Constraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError), newRoles);
				}
			}
		}
		#endregion
	}
	#region TooManyRoleSequencesError's Generated Constructor Code
	public  partial class TooManyRoleSequencesError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooManyRoleSequencesError CreateTooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (TooManyRoleSequencesError)store.ElementFactory.CreateElement(typeof(TooManyRoleSequencesError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooManyRoleSequencesError CreateAndInitializeTooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (TooManyRoleSequencesError)store.ElementFactory.CreateElement(typeof(TooManyRoleSequencesError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for TooManyRoleSequencesError
	/// <summary>
	/// TooManyRoleSequencesError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError))]
	public sealed class TooManyRoleSequencesErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooManyRoleSequencesErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.DuplicateNameError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.DuplicateNameError")]
	public abstract partial class DuplicateNameError : Northface.Tools.ORM.ObjectModel.ModelError
	{
		#region DuplicateNameError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "cbd2724e-6199-4f12-a3aa-7522187bea20";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.DuplicateNameError.MetaClassGuidString);
		#endregion

	}
	#region DuplicateNameError's Generated Constructor Code
	public abstract partial class DuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected DuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError")]
	public  partial class ObjectTypeDuplicateNameError : Northface.Tools.ORM.ObjectModel.DuplicateNameError
	{
		#region ObjectTypeDuplicateNameError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "29be74c2-24cc-4b4b-ab61-7750ca7ec339";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError.MetaClassGuidString);
		#endregion

		#region ObjectTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection ObjectTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ObjectTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region ObjectTypeDuplicateNameError's Generated Constructor Code
	public  partial class ObjectTypeDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeDuplicateNameError CreateObjectTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ObjectTypeDuplicateNameError)store.ElementFactory.CreateElement(typeof(ObjectTypeDuplicateNameError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeDuplicateNameError CreateAndInitializeObjectTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectTypeDuplicateNameError)store.ElementFactory.CreateElement(typeof(ObjectTypeDuplicateNameError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectTypeDuplicateNameError
	/// <summary>
	/// ObjectTypeDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError))]
	public sealed class ObjectTypeDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError")]
	public  partial class FactTypeDuplicateNameError : Northface.Tools.ORM.ObjectModel.DuplicateNameError
	{
		#region FactTypeDuplicateNameError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "51040183-4ecf-4128-9e5f-89369fcf152b";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError.MetaClassGuidString);
		#endregion

		#region FactTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection FactTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region FactTypeDuplicateNameError's Generated Constructor Code
	public  partial class FactTypeDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeDuplicateNameError CreateFactTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (FactTypeDuplicateNameError)store.ElementFactory.CreateElement(typeof(FactTypeDuplicateNameError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeDuplicateNameError CreateAndInitializeFactTypeDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeDuplicateNameError)store.ElementFactory.CreateElement(typeof(FactTypeDuplicateNameError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeDuplicateNameError
	/// <summary>
	/// FactTypeDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError))]
	public sealed class FactTypeDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError")]
	public  partial class ConstraintDuplicateNameError : Northface.Tools.ORM.ObjectModel.DuplicateNameError
	{
		#region ConstraintDuplicateNameError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "0ecf0d2d-b3a3-4652-9e77-48fa7765d08f";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError.MetaClassGuidString);
		#endregion

		#region SingleColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection SingleColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
		#region MultiColumnExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection MultiColumnExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region ConstraintDuplicateNameError's Generated Constructor Code
	public  partial class ConstraintDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintDuplicateNameError CreateConstraintDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ConstraintDuplicateNameError)store.ElementFactory.CreateElement(typeof(ConstraintDuplicateNameError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintDuplicateNameError CreateAndInitializeConstraintDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ConstraintDuplicateNameError)store.ElementFactory.CreateElement(typeof(ConstraintDuplicateNameError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ConstraintDuplicateNameError
	/// <summary>
	/// ConstraintDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError))]
	public sealed class ConstraintDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrder.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrder")]
	public  partial class ReadingOrder : Microsoft.VisualStudio.Modeling.ModelElement
	{
		#region ReadingOrder's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "624a498e-0659-4c34-ab74-7fe43b4c8fa1";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrder.MetaClassGuidString);
		#endregion

		#region ReadingText's Generated  Field Code
		#region ReadingText's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ReadingTextMetaAttributeGuidString = "b426931f-9232-4e42-b384-61c60d331aae";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ReadingTextMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrder.ReadingTextMetaAttributeGuidString);
		#endregion

		#region ReadingText's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Editor(typeof(Northface.Tools.ORM.ObjectModel.Editors.ReadingTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, FieldHandlerType=typeof(ReadingOrderReadingTextFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrder.ReadingTextMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrder.ReadingText")]
		public  System.String ReadingText
		{
			get
			{
				return readingOrderReadingTextFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				readingOrderReadingTextFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReadingOrderReadingTextFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ReadingOrder.ReadingText field
		/// </summary>
		private static ReadingOrderReadingTextFieldHandler	readingOrderReadingTextFieldHandler	= ReadingOrderReadingTextFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ReadingOrder.ReadingText
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReadingOrderReadingTextFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ReadingOrder,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReadingOrderReadingTextFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ReadingOrder.ReadingText field handler
			/// </summary>
			/// <value>ReadingOrderReadingTextFieldHandler</value>
			public static ReadingOrderReadingTextFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ReadingOrder.readingOrderReadingTextFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ReadingOrder.readingOrderReadingTextFieldHandler;
					}
					else
					{
						// The static constructor in ReadingOrder will assign this value to
						// Northface.Tools.ORM.ObjectModel.ReadingOrder.readingOrderReadingTextFieldHandler, so just instantiate one and return it
						return new ReadingOrderReadingTextFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ReadingOrder.ReadingText field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ReadingOrder.ReadingTextMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region ReadingCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReadingMoveableCollection ReadingCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReadingMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrderMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuid); }
		}
		#endregion
		#region RoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection RoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.ReadingOrderMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.RoleCollectionMetaRoleGuid); }
		}
		#endregion
		#region FactType's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ReadingOrder
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ReadingOrder Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ReadingOrderMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ReadingOrderMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ReadingOrder[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ReadingOrder at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ReadingOrder this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReadingOrder)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReadingOrder to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ReadingOrder value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReadingOrder to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ReadingOrder value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReadingOrder to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ReadingOrder value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReadingOrder to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ReadingOrder value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReadingOrder to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ReadingOrder value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ReadingOrder rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ReadingOrder rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ReadingOrder's Generated Constructor Code
	public  partial class ReadingOrder
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrder(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrder CreateReadingOrder(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ReadingOrder)store.ElementFactory.CreateElement(typeof(ReadingOrder));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrder CreateAndInitializeReadingOrder(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReadingOrder)store.ElementFactory.CreateElement(typeof(ReadingOrder), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReadingOrder
	/// <summary>
	/// ReadingOrder Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrder))]
	public sealed class ReadingOrderElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrderElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReadingOrder(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeKind")]
	public  partial class ReferenceModeKind : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ReferenceModeKind's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "3fc5ec28-7c78-49e7-955c-4c54536c8d21";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.MetaClassGuidString);
		#endregion

		#region FormatString's Generated  Field Code
		#region FormatString's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String FormatStringMetaAttributeGuidString = "76827916-449c-4e9a-ab21-01ac01cc5817";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid FormatStringMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.FormatStringMetaAttributeGuidString);
		#endregion

		#region FormatString's Generated Property Code

		private System.String formatStringPropertyStorage = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ReferenceModeKindFormatStringFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.FormatStringMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeKind.FormatString")]
		public  System.String FormatString
		{
			get
			{
				return formatStringPropertyStorage;
			}
		
			set
			{
				referenceModeKindFormatStringFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReferenceModeKindFormatStringFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ReferenceModeKind.FormatString field
		/// </summary>
		private static ReferenceModeKindFormatStringFieldHandler	referenceModeKindFormatStringFieldHandler	= ReferenceModeKindFormatStringFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ReferenceModeKind.FormatString
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReferenceModeKindFormatStringFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ReferenceModeKind,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReferenceModeKindFormatStringFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ReferenceModeKind.FormatString field handler
			/// </summary>
			/// <value>ReferenceModeKindFormatStringFieldHandler</value>
			public static ReferenceModeKindFormatStringFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindFormatStringFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindFormatStringFieldHandler;
					}
					else
					{
						// The static constructor in ReferenceModeKind will assign this value to
						// Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindFormatStringFieldHandler, so just instantiate one and return it
						return new ReferenceModeKindFormatStringFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ReferenceModeKind.FormatString field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ReferenceModeKind.FormatStringMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ReferenceModeKind</param>
			protected sealed override System.String GetValue(Northface.Tools.ORM.ObjectModel.ReferenceModeKind element)
			{
				return element.formatStringPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ReferenceModeKind element, System.String value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.String oldValue)
			{
				oldValue = element.formatStringPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.formatStringPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region ReferenceModeType's Generated  Field Code
		#region ReferenceModeType's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ReferenceModeTypeMetaAttributeGuidString = "ccb8e858-ddf2-4810-8231-99ccabc73142";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeTypeMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.ReferenceModeTypeMetaAttributeGuidString);
		#endregion

		#region ReferenceModeType's Generated Property Code

		private Northface.Tools.ORM.ObjectModel.ReferenceModeType referenceModeTypePropertyStorage = Northface.Tools.ORM.ObjectModel.ReferenceModeType.General;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.EnumerationDomainAttribute(EnumerationType=typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeType),DefaultEnumerationValueName="General")]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ReferenceModeKindReferenceModeTypeFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeKind.ReferenceModeTypeMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeKind.ReferenceModeType")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceModeType ReferenceModeType
		{
			get
			{
				return referenceModeTypePropertyStorage;
			}
		
			set
			{
				referenceModeKindReferenceModeTypeFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReferenceModeKindReferenceModeTypeFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ReferenceModeKind.ReferenceModeType field
		/// </summary>
		private static ReferenceModeKindReferenceModeTypeFieldHandler	referenceModeKindReferenceModeTypeFieldHandler	= ReferenceModeKindReferenceModeTypeFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ReferenceModeKind.ReferenceModeType
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReferenceModeKindReferenceModeTypeFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ReferenceModeKind,Northface.Tools.ORM.ObjectModel.ReferenceModeType>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReferenceModeKindReferenceModeTypeFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ReferenceModeKind.ReferenceModeType field handler
			/// </summary>
			/// <value>ReferenceModeKindReferenceModeTypeFieldHandler</value>
			public static ReferenceModeKindReferenceModeTypeFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindReferenceModeTypeFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindReferenceModeTypeFieldHandler;
					}
					else
					{
						// The static constructor in ReferenceModeKind will assign this value to
						// Northface.Tools.ORM.ObjectModel.ReferenceModeKind.referenceModeKindReferenceModeTypeFieldHandler, so just instantiate one and return it
						return new ReferenceModeKindReferenceModeTypeFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ReferenceModeKind.ReferenceModeType field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ReferenceModeKind.ReferenceModeTypeMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ReferenceModeKind</param>
			protected sealed override Northface.Tools.ORM.ObjectModel.ReferenceModeType GetValue(Northface.Tools.ORM.ObjectModel.ReferenceModeKind element)
			{
				return element.referenceModeTypePropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ReferenceModeKind element, Northface.Tools.ORM.ObjectModel.ReferenceModeType value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref Northface.Tools.ORM.ObjectModel.ReferenceModeType oldValue)
			{
				oldValue = element.referenceModeTypePropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.referenceModeTypePropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind), newRoles);
				}
			}
		}
		#endregion
		#region ReferenceModeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReferenceModeMoveableCollection ReferenceModeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReferenceModeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.KindMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for ReferenceModeKind
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ReferenceModeKind Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ReferenceModeKindMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ReferenceModeKindMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ReferenceModeKind[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ReferenceModeKind at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ReferenceModeKind this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceModeKind)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceModeKind to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ReferenceModeKind value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceModeKind to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ReferenceModeKind value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceModeKind to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ReferenceModeKind value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceModeKind to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ReferenceModeKind value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceModeKind to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ReferenceModeKind value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ReferenceModeKind rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ReferenceModeKind rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ReferenceModeKind's Generated Constructor Code
	public  partial class ReferenceModeKind
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReferenceModeKind CreateReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ReferenceModeKind)store.ElementFactory.CreateElement(typeof(ReferenceModeKind));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReferenceModeKind CreateAndInitializeReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReferenceModeKind)store.ElementFactory.CreateElement(typeof(ReferenceModeKind), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReferenceModeKind
	/// <summary>
	/// ReferenceModeKind Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeKind))]
	public sealed class ReferenceModeKindElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReferenceModeKindElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReferenceModeKind(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceMode.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceMode")]
	public abstract partial class ReferenceMode : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ReferenceMode's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "f6388323-e223-4e06-bbc8-055339c5bedb";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceMode.MetaClassGuidString);
		#endregion

		#region KindDisplay's Generated  Field Code
		#region KindDisplay's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String KindDisplayMetaAttributeGuidString = "0212a64d-baf9-42a1-890f-92b8d6b5ee2d";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid KindDisplayMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceMode.KindDisplayMetaAttributeGuidString);
		#endregion

		#region KindDisplay's Generated Property Code

		/// <summary>
		/// 
		/// </summary>
		[System.ComponentModel.Editor(typeof(Northface.Tools.ORM.ObjectModel.Editors.ReferenceModeKindPicker), typeof(System.Drawing.Design.UITypeEditor))]
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(CustomStorage=true, AllowNulls=true, FieldHandlerType=typeof(ReferenceModeKindDisplayFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceMode.KindDisplayMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceMode.KindDisplay")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceModeKind KindDisplay
		{
			get
			{
				return referenceModeKindDisplayFieldHandler.GetFieldValue(this);
			}
		
			set
			{
				referenceModeKindDisplayFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ReferenceModeKindDisplayFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ReferenceMode.KindDisplay field
		/// </summary>
		private static ReferenceModeKindDisplayFieldHandler	referenceModeKindDisplayFieldHandler	= ReferenceModeKindDisplayFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ReferenceMode.KindDisplay
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ReferenceModeKindDisplayFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementCustomStoredFieldHandler<Northface.Tools.ORM.ObjectModel.ReferenceMode,Northface.Tools.ORM.ObjectModel.ReferenceModeKind>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ReferenceModeKindDisplayFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ReferenceMode.KindDisplay field handler
			/// </summary>
			/// <value>ReferenceModeKindDisplayFieldHandler</value>
			public static ReferenceModeKindDisplayFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ReferenceMode.referenceModeKindDisplayFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ReferenceMode.referenceModeKindDisplayFieldHandler;
					}
					else
					{
						// The static constructor in ReferenceMode will assign this value to
						// Northface.Tools.ORM.ObjectModel.ReferenceMode.referenceModeKindDisplayFieldHandler, so just instantiate one and return it
						return new ReferenceModeKindDisplayFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ReferenceMode.KindDisplay field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ReferenceMode.KindDisplayMetaAttributeGuid;
				}
			}
		}
		#endregion
		#endregion
		
		#region Model's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						link.Remove();
						break;
					}
				}
				if (value != null)
				{
					Microsoft.VisualStudio.Modeling.RoleAssignment[] newRoles = new Microsoft.VisualStudio.Modeling.RoleAssignment[2];
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ReferenceMode
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ReferenceMode Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ReferenceModeMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ReferenceModeMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ReferenceMode))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ReferenceMode[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ReferenceMode at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ReferenceMode this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceMode)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceMode to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ReferenceMode value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceMode to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ReferenceMode value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceMode to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ReferenceMode value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceMode to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ReferenceMode value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ReferenceMode to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ReferenceMode value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ReferenceMode rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ReferenceMode rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ReferenceMode's Generated Constructor Code
	public abstract partial class ReferenceMode
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.IntrinsicReferenceMode.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.IntrinsicReferenceMode")]
	public  partial class IntrinsicReferenceMode : Northface.Tools.ORM.ObjectModel.ReferenceMode
	{
		#region IntrinsicReferenceMode's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "3e08027c-e2c5-4237-a640-2a22a6f534af";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.IntrinsicReferenceMode.MetaClassGuidString);
		#endregion

	}
	#region IntrinsicReferenceMode's Generated Constructor Code
	public  partial class IntrinsicReferenceMode
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public IntrinsicReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static IntrinsicReferenceMode CreateIntrinsicReferenceMode(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (IntrinsicReferenceMode)store.ElementFactory.CreateElement(typeof(IntrinsicReferenceMode));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static IntrinsicReferenceMode CreateAndInitializeIntrinsicReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (IntrinsicReferenceMode)store.ElementFactory.CreateElement(typeof(IntrinsicReferenceMode), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for IntrinsicReferenceMode
	/// <summary>
	/// IntrinsicReferenceMode Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.IntrinsicReferenceMode))]
	public sealed class IntrinsicReferenceModeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public IntrinsicReferenceModeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.IntrinsicReferenceMode(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaClass("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.CustomReferenceMode.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.CustomReferenceMode")]
	public  partial class CustomReferenceMode : Northface.Tools.ORM.ObjectModel.ReferenceMode
	{
		#region CustomReferenceMode's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "6c7bae68-81a4-4bfa-b75b-c231cc77a3b3";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.CustomReferenceMode.MetaClassGuidString);
		#endregion

		#region CustomFormatString's Generated  Field Code
		#region CustomFormatString's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String CustomFormatStringMetaAttributeGuidString = "92e785d7-3457-4e91-b99b-17a8bb46e65b";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid CustomFormatStringMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.CustomReferenceMode.CustomFormatStringMetaAttributeGuidString);
		#endregion

		#region CustomFormatString's Generated Property Code

		private System.String customFormatStringPropertyStorage = string.Empty;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.StringDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(CustomReferenceModeCustomFormatStringFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.CustomReferenceMode.CustomFormatStringMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.CustomReferenceMode.CustomFormatString")]
		public  System.String CustomFormatString
		{
			get
			{
				return customFormatStringPropertyStorage;
			}
		
			set
			{
				customReferenceModeCustomFormatStringFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region CustomReferenceModeCustomFormatStringFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for CustomReferenceMode.CustomFormatString field
		/// </summary>
		private static CustomReferenceModeCustomFormatStringFieldHandler	customReferenceModeCustomFormatStringFieldHandler	= CustomReferenceModeCustomFormatStringFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for CustomReferenceMode.CustomFormatString
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class CustomReferenceModeCustomFormatStringFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.CustomReferenceMode,System.String>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private CustomReferenceModeCustomFormatStringFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the CustomReferenceMode.CustomFormatString field handler
			/// </summary>
			/// <value>CustomReferenceModeCustomFormatStringFieldHandler</value>
			public static CustomReferenceModeCustomFormatStringFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.CustomReferenceMode.customReferenceModeCustomFormatStringFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.CustomReferenceMode.customReferenceModeCustomFormatStringFieldHandler;
					}
					else
					{
						// The static constructor in CustomReferenceMode will assign this value to
						// Northface.Tools.ORM.ObjectModel.CustomReferenceMode.customReferenceModeCustomFormatStringFieldHandler, so just instantiate one and return it
						return new CustomReferenceModeCustomFormatStringFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the CustomReferenceMode.CustomFormatString field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.CustomReferenceMode.CustomFormatStringMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the CustomReferenceMode</param>
			protected sealed override System.String GetValue(Northface.Tools.ORM.ObjectModel.CustomReferenceMode element)
			{
				return element.customFormatStringPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.CustomReferenceMode element, System.String value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.String oldValue)
			{
				oldValue = element.customFormatStringPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.customFormatStringPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
	}
	#region CustomReferenceMode's Generated Constructor Code
	public  partial class CustomReferenceMode
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static CustomReferenceMode CreateCustomReferenceMode(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (CustomReferenceMode)store.ElementFactory.CreateElement(typeof(CustomReferenceMode));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static CustomReferenceMode CreateAndInitializeCustomReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (CustomReferenceMode)store.ElementFactory.CreateElement(typeof(CustomReferenceMode), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for CustomReferenceMode
	/// <summary>
	/// CustomReferenceMode Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.CustomReferenceMode))]
	public sealed class CustomReferenceModeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomReferenceModeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.CustomReferenceMode(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType")]
	public  partial class ValueTypeHasDataType : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ValueTypeHasDataType's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "eaf3a24b-8adc-4836-a901-6eaa560946d1";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.MetaRelationshipGuidString);
		#endregion

		#region Scale's Generated  Field Code
		#region Scale's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ScaleMetaAttributeGuidString = "927b45e4-e831-43f5-b1af-99e18df739ce";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ScaleMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ScaleMetaAttributeGuidString);
		#endregion

		#region Scale's Generated Property Code

		private System.Int32 scalePropertyStorage = 0;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.IntegerDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ValueTypeHasDataTypeScaleFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ScaleMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.Scale")]
		public  System.Int32 Scale
		{
			get
			{
				return scalePropertyStorage;
			}
		
			set
			{
				valueTypeHasDataTypeScaleFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ValueTypeHasDataTypeScaleFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ValueTypeHasDataType.Scale field
		/// </summary>
		private static ValueTypeHasDataTypeScaleFieldHandler	valueTypeHasDataTypeScaleFieldHandler	= ValueTypeHasDataTypeScaleFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ValueTypeHasDataType.Scale
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ValueTypeHasDataTypeScaleFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType,System.Int32>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ValueTypeHasDataTypeScaleFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ValueTypeHasDataType.Scale field handler
			/// </summary>
			/// <value>ValueTypeHasDataTypeScaleFieldHandler</value>
			public static ValueTypeHasDataTypeScaleFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeScaleFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeScaleFieldHandler;
					}
					else
					{
						// The static constructor in ValueTypeHasDataType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeScaleFieldHandler, so just instantiate one and return it
						return new ValueTypeHasDataTypeScaleFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ValueTypeHasDataType.Scale field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ScaleMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ValueTypeHasDataType</param>
			protected sealed override System.Int32 GetValue(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType element)
			{
				return element.scalePropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType element, System.Int32 value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Int32 oldValue)
			{
				oldValue = element.scalePropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.scalePropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region Length's Generated  Field Code
		#region Length's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String LengthMetaAttributeGuidString = "1a220465-c1bc-4736-93e4-e675eb1d1196";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid LengthMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.LengthMetaAttributeGuidString);
		#endregion

		#region Length's Generated Property Code

		private System.Int32 lengthPropertyStorage = 0;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.IntegerDomainAttribute]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ValueTypeHasDataTypeLengthFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.LengthMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.Length")]
		public  System.Int32 Length
		{
			get
			{
				return lengthPropertyStorage;
			}
		
			set
			{
				valueTypeHasDataTypeLengthFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ValueTypeHasDataTypeLengthFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ValueTypeHasDataType.Length field
		/// </summary>
		private static ValueTypeHasDataTypeLengthFieldHandler	valueTypeHasDataTypeLengthFieldHandler	= ValueTypeHasDataTypeLengthFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ValueTypeHasDataType.Length
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ValueTypeHasDataTypeLengthFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType,System.Int32>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ValueTypeHasDataTypeLengthFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ValueTypeHasDataType.Length field handler
			/// </summary>
			/// <value>ValueTypeHasDataTypeLengthFieldHandler</value>
			public static ValueTypeHasDataTypeLengthFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeLengthFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeLengthFieldHandler;
					}
					else
					{
						// The static constructor in ValueTypeHasDataType will assign this value to
						// Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.valueTypeHasDataTypeLengthFieldHandler, so just instantiate one and return it
						return new ValueTypeHasDataTypeLengthFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ValueTypeHasDataType.Length field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.LengthMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ValueTypeHasDataType</param>
			protected sealed override System.Int32 GetValue(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType element)
			{
				return element.lengthPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType element, System.Int32 value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Int32 oldValue)
			{
				oldValue = element.lengthPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.lengthPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region DataType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DataTypeMetaRoleGuidString = "f428f909-8e22-4608-bef3-1f730fd1077f";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DataTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.DataType")]
		public  Northface.Tools.ORM.ObjectModel.DataType DataType
		{
			get { return (Northface.Tools.ORM.ObjectModel.DataType)this.GetRolePlayer(DataTypeMetaRoleGuid); }
			set { this.SetRolePlayer(DataTypeMetaRoleGuid, value); }
		}
		#endregion
		#region ValueTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ValueTypeCollectionMetaRoleGuidString = "72598bd8-40da-4a90-8005-d7ef2acaf536";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ValueTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType.ValueTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType ValueTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(ValueTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ValueTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ValueTypeHasDataType's Generated Constructor Code
	public  partial class ValueTypeHasDataType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ValueTypeHasDataType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ValueTypeHasDataType CreateValueTypeHasDataType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ValueTypeHasDataType)store.ElementFactory.CreateElementLink(typeof(ValueTypeHasDataType), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ValueTypeHasDataType CreateAndInitializeValueTypeHasDataType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ValueTypeHasDataType)store.ElementFactory.CreateElementLink(typeof(ValueTypeHasDataType), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ValueTypeHasDataType
	/// <summary>
	/// ValueTypeHasDataType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType))]
	public sealed class ValueTypeHasDataTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ValueTypeHasDataTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ValueTypeHasDataType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeDerivation")]
	public  partial class FactTypeDerivation : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeDerivation's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "20e02e70-ade6-4882-9db8-8792e880e19c";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.MetaRelationshipGuidString);
		#endregion

		#region IsStored's Generated  Field Code
		#region IsStored's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String IsStoredMetaAttributeGuidString = "f5540418-e472-4b4c-af64-27aa6b41be29";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid IsStoredMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.IsStoredMetaAttributeGuidString);
		#endregion

		#region IsStored's Generated Property Code

		private System.Boolean isStoredPropertyStorage = false;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.BooleanDomainAttribute(DefaultBoolean=false)]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(FactTypeDerivationIsStoredFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.IsStoredMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeDerivation.IsStored")]
		public  System.Boolean IsStored
		{
			get
			{
				return isStoredPropertyStorage;
			}
		
			set
			{
				factTypeDerivationIsStoredFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region FactTypeDerivationIsStoredFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for FactTypeDerivation.IsStored field
		/// </summary>
		private static FactTypeDerivationIsStoredFieldHandler	factTypeDerivationIsStoredFieldHandler	= FactTypeDerivationIsStoredFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for FactTypeDerivation.IsStored
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class FactTypeDerivationIsStoredFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.FactTypeDerivation,System.Boolean>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private FactTypeDerivationIsStoredFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the FactTypeDerivation.IsStored field handler
			/// </summary>
			/// <value>FactTypeDerivationIsStoredFieldHandler</value>
			public static FactTypeDerivationIsStoredFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.FactTypeDerivation.factTypeDerivationIsStoredFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.FactTypeDerivation.factTypeDerivationIsStoredFieldHandler;
					}
					else
					{
						// The static constructor in FactTypeDerivation will assign this value to
						// Northface.Tools.ORM.ObjectModel.FactTypeDerivation.factTypeDerivationIsStoredFieldHandler, so just instantiate one and return it
						return new FactTypeDerivationIsStoredFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the FactTypeDerivation.IsStored field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.FactTypeDerivation.IsStoredMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the FactTypeDerivation</param>
			protected sealed override System.Boolean GetValue(Northface.Tools.ORM.ObjectModel.FactTypeDerivation element)
			{
				return element.isStoredPropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.FactTypeDerivation element, System.Boolean value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref System.Boolean oldValue)
			{
				oldValue = element.isStoredPropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.isStoredPropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
		#endregion
		
		#region AssociatedFactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String AssociatedFactTypeMetaRoleGuidString = "cb2af830-7d0d-42b7-ae61-c4edb6bd5250";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid AssociatedFactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeDerivation.AssociatedFactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType AssociatedFactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(AssociatedFactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(AssociatedFactTypeMetaRoleGuid, value); }
		}
		#endregion
		#region DerivationRule's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DerivationRuleMetaRoleGuidString = "8fb08a4b-f809-4e37-a62b-28c082a28a54";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DerivationRuleMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRuleMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeDerivation.DerivationRule")]
		public  Northface.Tools.ORM.ObjectModel.Expression DerivationRule
		{
			get { return (Northface.Tools.ORM.ObjectModel.Expression)this.GetRolePlayer(DerivationRuleMetaRoleGuid); }
			set { this.SetRolePlayer(DerivationRuleMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeDerivation's Generated Constructor Code
	public  partial class FactTypeDerivation
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeDerivation(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeDerivation CreateFactTypeDerivation(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeDerivation)store.ElementFactory.CreateElementLink(typeof(FactTypeDerivation), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeDerivation CreateAndInitializeFactTypeDerivation(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeDerivation)store.ElementFactory.CreateElementLink(typeof(FactTypeDerivation), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeDerivation
	/// <summary>
	/// FactTypeDerivation Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeDerivation))]
	public sealed class FactTypeDerivationElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeDerivationElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeDerivation(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType")]
	public  partial class NestingEntityTypeHasFactType : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region NestingEntityTypeHasFactType's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "542e780f-0bd0-4617-9715-935beb73cee9";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.MetaRelationshipGuidString);
		#endregion

		#region NestedFactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String NestedFactTypeMetaRoleGuidString = "6743ca8b-70dc-4c77-85ff-acf2f197ef72";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid NestedFactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestedFactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType NestedFactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(NestedFactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(NestedFactTypeMetaRoleGuid, value); }
		}
		#endregion
		#region NestingType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String NestingTypeMetaRoleGuidString = "7a57a47b-772d-4362-898a-572e9e5a7728";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid NestingTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType.NestingType")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType NestingType
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(NestingTypeMetaRoleGuid); }
			set { this.SetRolePlayer(NestingTypeMetaRoleGuid, value); }
		}
		#endregion
	}
	#region NestingEntityTypeHasFactType's Generated Constructor Code
	public  partial class NestingEntityTypeHasFactType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public NestingEntityTypeHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static NestingEntityTypeHasFactType CreateNestingEntityTypeHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (NestingEntityTypeHasFactType)store.ElementFactory.CreateElementLink(typeof(NestingEntityTypeHasFactType), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static NestingEntityTypeHasFactType CreateAndInitializeNestingEntityTypeHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (NestingEntityTypeHasFactType)store.ElementFactory.CreateElementLink(typeof(NestingEntityTypeHasFactType), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for NestingEntityTypeHasFactType
	/// <summary>
	/// NestingEntityTypeHasFactType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType))]
	public sealed class NestingEntityTypeHasFactTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public NestingEntityTypeHasFactTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.NestingEntityTypeHasFactType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasRole")]
	public  partial class FactTypeHasRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeHasRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "af40ed09-483b-47ea-bfd6-12d7e4cb3dde";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.MetaRelationshipGuidString);
		#endregion

		#region FactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeMetaRoleGuidString = "67f51c5d-90a1-4719-8454-9da18c1d1a5c";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasRole.FactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeMetaRoleGuid, value); }
		}
		#endregion
		#region RoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleCollectionMetaRoleGuidString = "dd378220-7c82-4522-979b-0d7b23be7cab";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasRole.RoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role RoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(RoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeHasRole's Generated Constructor Code
	public  partial class FactTypeHasRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasRole CreateFactTypeHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeHasRole)store.ElementFactory.CreateElementLink(typeof(FactTypeHasRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasRole CreateAndInitializeFactTypeHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeHasRole)store.ElementFactory.CreateElementLink(typeof(FactTypeHasRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeHasRole
	/// <summary>
	/// FactTypeHasRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasRole))]
	public sealed class FactTypeHasRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeHasRole(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole")]
	public  partial class ObjectTypePlaysRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ObjectTypePlaysRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "cd3fd3e2-9822-47b3-a158-8bc59b7ef0fc";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.MetaRelationshipGuidString);
		#endregion

		#region RolePlayer's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RolePlayerMetaRoleGuidString = "8cabe0dc-33de-4332-93fe-c9f4b092004c";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RolePlayerMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayerMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayerMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.RolePlayer")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType RolePlayer
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(RolePlayerMetaRoleGuid); }
			set { this.SetRolePlayer(RolePlayerMetaRoleGuid, value); }
		}
		#endregion
		#region PlayedRoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String PlayedRoleCollectionMetaRoleGuidString = "0f45f22d-a7a7-49a8-9e09-17cf362639d0";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid PlayedRoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole.PlayedRoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role PlayedRoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(PlayedRoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(PlayedRoleCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ObjectTypePlaysRole's Generated Constructor Code
	public  partial class ObjectTypePlaysRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypePlaysRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypePlaysRole CreateObjectTypePlaysRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ObjectTypePlaysRole)store.ElementFactory.CreateElementLink(typeof(ObjectTypePlaysRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypePlaysRole CreateAndInitializeObjectTypePlaysRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectTypePlaysRole)store.ElementFactory.CreateElementLink(typeof(ObjectTypePlaysRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectTypePlaysRole
	/// <summary>
	/// ObjectTypePlaysRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
	public sealed class ObjectTypePlaysRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypePlaysRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ObjectTypePlaysRole(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TypeHasSubType.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.TypeHasSubType")]
	public  partial class TypeHasSubType : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region TypeHasSubType's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "eaac28ac-6841-462d-85e5-1fd352e36714";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TypeHasSubType.MetaRelationshipGuidString);
		#endregion

		#region SubTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String SubTypeCollectionMetaRoleGuidString = "f691e77b-54b7-4862-a8b4-354b014996af";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid SubTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TypeHasSubType.SubTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TypeHasSubType.SubTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.TypeHasSubType.SubTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.RootType SubTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.RootType)this.GetRolePlayer(SubTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(SubTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region SuperTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String SuperTypeCollectionMetaRoleGuidString = "b0846558-ccfd-4278-9d7b-9214d8bd1f39";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid SuperTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TypeHasSubType.SuperTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TypeHasSubType.SuperTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.TypeHasSubType.SuperTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.RootType SuperTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.RootType)this.GetRolePlayer(SuperTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(SuperTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region TypeHasSubType's Generated Constructor Code
	public  partial class TypeHasSubType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TypeHasSubType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TypeHasSubType CreateTypeHasSubType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (TypeHasSubType)store.ElementFactory.CreateElementLink(typeof(TypeHasSubType), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TypeHasSubType CreateAndInitializeTypeHasSubType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (TypeHasSubType)store.ElementFactory.CreateElementLink(typeof(TypeHasSubType), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for TypeHasSubType
	/// <summary>
	/// TypeHasSubType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.TypeHasSubType))]
	public sealed class TypeHasSubTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TypeHasSubTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.TypeHasSubType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasObjectType")]
	public  partial class ModelHasObjectType : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasObjectType's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "7f3a3597-e6e1-487b-a762-378fc79aedba";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.MetaRelationshipGuidString);
		#endregion

		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "f2e6e473-0a96-4199-9a63-67ce66a8783d";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasObjectType.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
		#region ObjectTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ObjectTypeCollectionMetaRoleGuidString = "d4e2ef09-1187-4d73-b0ce-c3c539e8a708";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ObjectTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasObjectType.ObjectTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType ObjectTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(ObjectTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ObjectTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasObjectType's Generated Constructor Code
	public  partial class ModelHasObjectType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasObjectType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasObjectType CreateModelHasObjectType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasObjectType)store.ElementFactory.CreateElementLink(typeof(ModelHasObjectType), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasObjectType CreateAndInitializeModelHasObjectType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasObjectType)store.ElementFactory.CreateElementLink(typeof(ModelHasObjectType), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasObjectType
	/// <summary>
	/// ModelHasObjectType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasObjectType))]
	public sealed class ModelHasObjectTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasObjectTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasObjectType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasFactType.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasFactType")]
	public  partial class ModelHasFactType : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasFactType's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4f0bfd84-658b-4bf4-acda-5db1355b1a33";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasFactType.MetaRelationshipGuidString);
		#endregion

		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "c7885cbd-6e5c-48f3-bb08-80335d82096a";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasFactType.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasFactType.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasFactType.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
		#region FactTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeCollectionMetaRoleGuidString = "ba041047-0d0f-45dc-aac3-29d5111ed1b0";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasFactType.FactTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasFactType's Generated Constructor Code
	public  partial class ModelHasFactType
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasFactType CreateModelHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasFactType)store.ElementFactory.CreateElementLink(typeof(ModelHasFactType), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasFactType CreateAndInitializeModelHasFactType(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasFactType)store.ElementFactory.CreateElementLink(typeof(ModelHasFactType), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasFactType
	/// <summary>
	/// ModelHasFactType Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasFactType))]
	public sealed class ModelHasFactTypeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasFactTypeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasFactType(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalFactConstraint")]
	public abstract partial class ExternalFactConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalFactConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "2ccd6a09-46be-4d7c-9d8b-b77679074f17";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.MetaRelationshipGuidString);
		#endregion

		#region ConstrainedRoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRoleMoveableCollection ConstrainedRoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for ExternalFactConstraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ExternalFactConstraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ExternalFactConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ExternalFactConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ExternalFactConstraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ExternalFactConstraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalFactConstraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalFactConstraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalFactConstraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalFactConstraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalFactConstraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalFactConstraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ExternalFactConstraint's Generated Constructor Code
	public abstract partial class ExternalFactConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
	}
	#endregion
}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint")]
	public  partial class MultiColumnExternalFactConstraint : Northface.Tools.ORM.ObjectModel.ExternalFactConstraint
	{
		#region MultiColumnExternalFactConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "6de34bee-23ca-4503-b451-0bf4f9623e88";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MetaRelationshipGuidString);
		#endregion

		#region MultiColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String MultiColumnExternalConstraintCollectionMetaRoleGuidString = "00ab3368-d142-4d99-8457-afd56e639f5a";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid MultiColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint MultiColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeCollectionMetaRoleGuidString = "6352e486-6683-4334-8c5c-9601fa3c07d7";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint.FactTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region MultiColumnExternalFactConstraint's Generated Constructor Code
	public  partial class MultiColumnExternalFactConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalFactConstraint CreateMultiColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (MultiColumnExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalFactConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalFactConstraint CreateAndInitializeMultiColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (MultiColumnExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalFactConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for MultiColumnExternalFactConstraint
	/// <summary>
	/// MultiColumnExternalFactConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint))]
	public sealed class MultiColumnExternalFactConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalFactConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalFactConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint")]
	public  partial class SingleColumnExternalFactConstraint : Northface.Tools.ORM.ObjectModel.ExternalFactConstraint
	{
		#region SingleColumnExternalFactConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "786eebce-120b-43c0-9622-e799943f4021";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.MetaRelationshipGuidString);
		#endregion

		#region SingleColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String SingleColumnExternalConstraintCollectionMetaRoleGuidString = "6045259d-a85e-40f6-bac3-744f03cef430";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid SingleColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint SingleColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint)this.GetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeCollectionMetaRoleGuidString = "3330f860-0dd6-4178-b7e8-e89cec558389";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint.FactTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region SingleColumnExternalFactConstraint's Generated Constructor Code
	public  partial class SingleColumnExternalFactConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SingleColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SingleColumnExternalFactConstraint CreateSingleColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (SingleColumnExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(SingleColumnExternalFactConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SingleColumnExternalFactConstraint CreateAndInitializeSingleColumnExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (SingleColumnExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(SingleColumnExternalFactConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for SingleColumnExternalFactConstraint
	/// <summary>
	/// SingleColumnExternalFactConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint))]
	public sealed class SingleColumnExternalFactConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SingleColumnExternalFactConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.SingleColumnExternalFactConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint")]
	public  partial class ExternalRoleConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalRoleConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "a075a8fd-feb9-4dec-9852-85b7272a009f";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.MetaRelationshipGuidString);
		#endregion

		#region FactConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactConstraintCollectionMetaRoleGuidString = "3b2ebee1-afe8-42cb-a7db-8ad4dc7d8be9";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.ExternalFactConstraint FactConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalFactConstraint)this.GetRolePlayer(FactConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ConstrainedRoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstrainedRoleCollectionMetaRoleGuidString = "c419f56d-c63f-4972-bdeb-86d9264e2de7";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstrainedRoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole ConstrainedRoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)this.GetRolePlayer(ConstrainedRoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ConstrainedRoleCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalRoleConstraint's Generated Constructor Code
	public  partial class ExternalRoleConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalRoleConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalRoleConstraint CreateExternalRoleConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalRoleConstraint)store.ElementFactory.CreateElementLink(typeof(ExternalRoleConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalRoleConstraint CreateAndInitializeExternalRoleConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalRoleConstraint)store.ElementFactory.CreateElementLink(typeof(ExternalRoleConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalRoleConstraint
	/// <summary>
	/// ExternalRoleConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint))]
	public sealed class ExternalRoleConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalRoleConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence")]
	public  partial class MultiColumnExternalConstraintHasRoleSequence : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region MultiColumnExternalConstraintHasRoleSequence's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4d952086-d820-4023-9207-14302e77b703";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.MetaRelationshipGuidString);
		#endregion

		#region RoleSequenceCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleSequenceCollectionMetaRoleGuidString = "152a01ce-3754-4b69-9f9e-036762f25a58";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleSequenceCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollection")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence RoleSequenceCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintRoleSequence)this.GetRolePlayer(RoleSequenceCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleSequenceCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ExternalConstraint's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ExternalConstraintMetaRoleGuidString = "efd54a41-2206-4daa-969a-b85b6a662ca9";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ExternalConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence.ExternalConstraint")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint ExternalConstraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(ExternalConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ExternalConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region MultiColumnExternalConstraintHasRoleSequence's Generated Constructor Code
	public  partial class MultiColumnExternalConstraintHasRoleSequence
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintHasRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintHasRoleSequence CreateMultiColumnExternalConstraintHasRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (MultiColumnExternalConstraintHasRoleSequence)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalConstraintHasRoleSequence), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintHasRoleSequence CreateAndInitializeMultiColumnExternalConstraintHasRoleSequence(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (MultiColumnExternalConstraintHasRoleSequence)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalConstraintHasRoleSequence), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for MultiColumnExternalConstraintHasRoleSequence
	/// <summary>
	/// MultiColumnExternalConstraintHasRoleSequence Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence))]
	public sealed class MultiColumnExternalConstraintHasRoleSequenceElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintHasRoleSequenceElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasRoleSequence(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole")]
	public  partial class ConstraintRoleSequenceHasRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ConstraintRoleSequenceHasRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "b66ae3b4-c404-486c-933a-fd23eea3c3d7";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.MetaRelationshipGuidString);
		#endregion

		#region ConstraintRoleSequenceCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintRoleSequenceCollectionMetaRoleGuidString = "dea36a6c-6706-41ee-bb34-61bc9a5416d2";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintRoleSequenceCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollection")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence ConstraintRoleSequenceCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence)this.GetRolePlayer(ConstraintRoleSequenceCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintRoleSequenceCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region RoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleCollectionMetaRoleGuidString = "43f43911-662e-480d-b566-8025ccb1f673";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole.RoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role RoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(RoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalFactConstraintMoveableCollection FactConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ExternalFactConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for ConstraintRoleSequenceHasRole
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ConstraintRoleSequenceHasRoleMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
	{
		private Microsoft.VisualStudio.Modeling.ModelElement counterpartMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo sourceRoleMember;
		private Microsoft.VisualStudio.Modeling.MetaRoleInfo targetRoleMember;
		/// <summary>
		/// Counterpart
		/// </summary>
		public Microsoft.VisualStudio.Modeling.ModelElement Counterpart
		{
			get { return this.counterpartMember; }
		}
		/// <summary>
		/// Source Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo SourceRole
		{
			get { return this.sourceRoleMember; }
		}
		/// <summary>
		/// Target Role
		/// </summary>
		public Microsoft.VisualStudio.Modeling.MetaRoleInfo TargetRole
		{
			get { return this.targetRoleMember; }
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="counterpart">Counterpart to create relationship with</param>
		/// <param name="sourceMetaRoleGuid">Source's meta role in this relationship</param>
		/// <param name="targetMetaRoleGuid">Target's meta role in this relationship</param>
		public ConstraintRoleSequenceHasRoleMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
		{
			this.counterpartMember = counterpart;
			this.sourceRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(sourceMetaRoleGuid);
			this.targetRoleMember = counterpart.Store.MetaDataDirectory.FindMetaRole(targetMetaRoleGuid);
		}
		/// <summary>
		/// Returns an enumerator that can iterate through a collection
		/// </summary>
		/// <returns>Enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).GetEnumerator();
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		void System.Collections.ICollection.CopyTo(System.Array array, System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).CopyTo(array, index);
		}
		/// <summary>
		/// When implemented by a class, gets the number of elements contained in the System.Collections.ICollection
		/// </summary>
		System.Int32 System.Collections.ICollection.Count
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Count; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether access to the System.Collections.ICollection is synchronized (thread-safe)
		/// </summary>
		System.Boolean System.Collections.ICollection.IsSynchronized
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsSynchronized; }
		}
		/// <summary>
		/// When implemented by a class, gets an object that can be used to synchronize access to the System.Collections.ICollection
		/// </summary>
		System.Object System.Collections.ICollection.SyncRoot
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).SyncRoot; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList has a fixed size
		/// </summary>
		System.Boolean System.Collections.IList.IsFixedSize
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsFixedSize; }
		}
		/// <summary>
		/// When implemented by a class, gets a value indicating whether the System.Collections.IList is read-only
		/// </summary>
		System.Boolean System.Collections.IList.IsReadOnly
		{
			get { return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>object at that index</returns>
		System.Object System.Collections.IList.this[System.Int32 index]
		{
			get
			{
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				return list[index];
			}
			set
			{
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
				{
					throw new System.InvalidCastException();
				}
				Microsoft.VisualStudio.Modeling.IMoveableCollection list = this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole);
				list[index] = value;
			}
		}
		/// <summary>
		/// When implemented by a class, adds an item to the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to add to the System.Collections.IList</param>
		/// <returns>index where object was added</returns>
		System.Int32 System.Collections.IList.Add(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Add(value);
		}
		/// <summary>
		/// When implemented by a class, removes all items from the System.Collections.IList
		/// </summary>
		void System.Collections.IList.Clear()
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Clear();
		}
		/// <summary>
		/// When implemented by a class, determines whether the System.Collections.IList has a specific value
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>true if object is contained, false otherwise</returns>
		System.Boolean System.Collections.IList.Contains(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Contains(value);
		}
		/// <summary>
		/// When implemented by a class, determines the index of a specific item in the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to locate in the System.Collections.IList</param>
		/// <returns>index of object</returns>
		System.Int32 System.Collections.IList.IndexOf(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			return this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).IndexOf(value);
		}
		/// <summary>
		/// When implemented by a class, inserts an item to the System.Collections.IList at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The System.Object to insert into the System.Collections.IList</param>
		void System.Collections.IList.Insert(System.Int32 index, System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Insert(index, value);
		}
		/// <summary>
		/// When implemented by a class, removes the first occurrence of a specific object from the System.Collections.IList
		/// </summary>
		/// <param name="value">The System.Object to remove from the System.Collections.IList</param>
		void System.Collections.IList.Remove(System.Object value)
		{
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Remove(value);
		}
		/// <summary>
		/// When implemented by a class, removes the System.Collections.IList item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		void System.Collections.IList.RemoveAt(System.Int32 index)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(Microsoft.VisualStudio.Modeling.ModelElement rolePlayer, System.Int32 newPosition)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(rolePlayer, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		void Microsoft.VisualStudio.Modeling.IMoveableCollection.ReplaceAt(System.Int32 position, Microsoft.VisualStudio.Modeling.ModelElement rolePlayer)
		{
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))))
			{
				throw new System.InvalidCastException();
			}
			this.Counterpart.GetMoveableRolePlayers(this.SourceRole, this.TargetRole).ReplaceAt(position, rolePlayer);
		}
		/// <summary>
		/// When implemented by a class, copies the elements of the System.Collection.ICollections to an System.Array, starting at a particular System.Array index
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from System.Collections.ICollection.  The System.Array must have zero-based indexing</param>
		/// <param name="index">The zero-based index in array at which copying begins</param>
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole[] array, System.Int32 index)
		{
			((System.Collections.ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Gets the number of elements contained in the collection
		/// </summary>
		public System.Int32 Count
		{
			get { return ((System.Collections.ICollection)this).Count; }
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only
		/// </summary>
		public System.Boolean IsReadOnly
		{
			get { return ((System.Collections.IList)this).IsReadOnly; }
		}
		/// <summary>
		/// Indexed accessor
		/// </summary>
		/// <param name="index">Index to access</param>
		/// <returns>Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole value)
		{
			return ((System.Collections.IList)this).Add(value as System.Object);
		}
		/// <summary>
		/// Removes all items from the list
		/// </summary>
		public void Clear()
		{
			((System.Collections.IList)this).Clear();
		}
		/// <summary>
		/// Determines whether the list has a specific value
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole value)
		{
			((System.Collections.IList)this).Remove(value as System.Object);
		}
		/// <summary>
		/// Removes the list item at the specified index
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove</param>
		public void RemoveAt(System.Int32 index)
		{
			((System.Collections.IList)this).RemoveAt(index);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="rolePlayer">The role player to move</param>
		/// <param name="newPosition">The position to move to</param>
		public void Move(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole rolePlayer, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement, newPosition);
		}
		/// <summary>
		/// Move the roleplayer to the new position in the collection
		/// </summary>
		/// <param name="oldPosition">The position of the role player to move from</param>
		/// <param name="newPosition">The position of the role player to move to</param>
		public void Move(System.Int32 oldPosition, System.Int32 newPosition)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).Move(oldPosition, newPosition);
		}
		/// <summary>
		/// Insert a roleplayer in the specified location
		/// </summary>
		/// <param name="position">The index of the roleplayer that needs to be replaced</param>
		/// <param name="rolePlayer">The role player that will be inserted</param>
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ConstraintRoleSequenceHasRole's Generated Constructor Code
	public  partial class ConstraintRoleSequenceHasRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintRoleSequenceHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintRoleSequenceHasRole CreateConstraintRoleSequenceHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ConstraintRoleSequenceHasRole)store.ElementFactory.CreateElementLink(typeof(ConstraintRoleSequenceHasRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintRoleSequenceHasRole CreateAndInitializeConstraintRoleSequenceHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ConstraintRoleSequenceHasRole)store.ElementFactory.CreateElementLink(typeof(ConstraintRoleSequenceHasRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ConstraintRoleSequenceHasRole
	/// <summary>
	/// ConstraintRoleSequenceHasRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))]
	public sealed class ConstraintRoleSequenceHasRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintRoleSequenceHasRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingHasRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ReadingHasRole")]
	public  partial class ReadingHasRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ReadingHasRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "e81b771c-faba-4f9a-9361-92d2a9f5498c";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingHasRole.MetaRelationshipGuidString);
		#endregion

		#region RoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleCollectionMetaRoleGuidString = "54350d49-2d00-46f9-85d0-68fa9771ca47";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingHasRole.RoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingHasRole.RoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingHasRole.RoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role RoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(RoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ReadingCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingCollectionMetaRoleGuidString = "da79a849-c301-40ff-b5a8-69c2c41045e5";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingHasRole.ReadingCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingHasRole.ReadingCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingHasRole.ReadingCollection")]
		public  Northface.Tools.ORM.ObjectModel.Reading ReadingCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Reading)this.GetRolePlayer(ReadingCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ReadingHasRole's Generated Constructor Code
	public  partial class ReadingHasRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingHasRole CreateReadingHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ReadingHasRole)store.ElementFactory.CreateElementLink(typeof(ReadingHasRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingHasRole CreateAndInitializeReadingHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReadingHasRole)store.ElementFactory.CreateElementLink(typeof(ReadingHasRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReadingHasRole
	/// <summary>
	/// ReadingHasRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReadingHasRole))]
	public sealed class ReadingHasRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingHasRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReadingHasRole(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasError")]
	public  partial class ModelHasError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4389da95-bd9e-4615-ac5f-5a466414c9b0";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasError.MetaRelationshipGuidString);
		#endregion

		#region ErrorCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ErrorCollectionMetaRoleGuidString = "0cc3a152-c6c3-49f3-97fd-7c97a6fd9657";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ErrorCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollection")]
		public  Northface.Tools.ORM.ObjectModel.ModelError ErrorCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ModelError)this.GetRolePlayer(ErrorCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ErrorCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "a407c3f0-23dc-468a-bc22-568bfcf8a827";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasError.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasError.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasError.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasError's Generated Constructor Code
	public  partial class ModelHasError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasError CreateModelHasError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasError)store.ElementFactory.CreateElementLink(typeof(ModelHasError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasError CreateAndInitializeModelHasError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasError)store.ElementFactory.CreateElementLink(typeof(ModelHasError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasError
	/// <summary>
	/// ModelHasError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasError))]
	public sealed class ModelHasErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError")]
	public  partial class ExternalConstraintHasTooFewRoleSequencesError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalConstraintHasTooFewRoleSequencesError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "dcf38e2c-fe6e-4f89-8cb2-44bb1e512856";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.MetaRelationshipGuidString);
		#endregion

		#region TooFewRoleSequencesError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String TooFewRoleSequencesErrorMetaRoleGuidString = "438baa58-0cff-42b2-93f5-38dffa0544a1";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid TooFewRoleSequencesErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesError")]
		public  Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError TooFewRoleSequencesError
		{
			get { return (Northface.Tools.ORM.ObjectModel.TooFewRoleSequencesError)this.GetRolePlayer(TooFewRoleSequencesErrorMetaRoleGuid); }
			set { this.SetRolePlayer(TooFewRoleSequencesErrorMetaRoleGuid, value); }
		}
		#endregion
		#region Constraint's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintMetaRoleGuidString = "fcb1aaae-a4fb-4eac-943b-af02468061d3";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.ConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError.Constraint")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint Constraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(ConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalConstraintHasTooFewRoleSequencesError's Generated Constructor Code
	public  partial class ExternalConstraintHasTooFewRoleSequencesError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooFewRoleSequencesError CreateExternalConstraintHasTooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalConstraintHasTooFewRoleSequencesError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooFewRoleSequencesError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooFewRoleSequencesError CreateAndInitializeExternalConstraintHasTooFewRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintHasTooFewRoleSequencesError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooFewRoleSequencesError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintHasTooFewRoleSequencesError
	/// <summary>
	/// ExternalConstraintHasTooFewRoleSequencesError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError))]
	public sealed class ExternalConstraintHasTooFewRoleSequencesErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooFewRoleSequencesErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSequencesError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError")]
	public  partial class ExternalConstraintHasTooManyRoleSequencesError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalConstraintHasTooManyRoleSequencesError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "5aec6871-993f-4373-8627-fb5eba073edd";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.MetaRelationshipGuidString);
		#endregion

		#region TooManyRoleSequencesError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String TooManyRoleSequencesErrorMetaRoleGuidString = "b65ff045-247b-4890-aa03-44d53c1832ad";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid TooManyRoleSequencesErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesError")]
		public  Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError TooManyRoleSequencesError
		{
			get { return (Northface.Tools.ORM.ObjectModel.TooManyRoleSequencesError)this.GetRolePlayer(TooManyRoleSequencesErrorMetaRoleGuid); }
			set { this.SetRolePlayer(TooManyRoleSequencesErrorMetaRoleGuid, value); }
		}
		#endregion
		#region Constraint's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintMetaRoleGuidString = "943eb43b-956e-4d97-9917-773386b53025";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.ConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError.Constraint")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint Constraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(ConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalConstraintHasTooManyRoleSequencesError's Generated Constructor Code
	public  partial class ExternalConstraintHasTooManyRoleSequencesError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooManyRoleSequencesError CreateExternalConstraintHasTooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalConstraintHasTooManyRoleSequencesError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooManyRoleSequencesError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooManyRoleSequencesError CreateAndInitializeExternalConstraintHasTooManyRoleSequencesError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintHasTooManyRoleSequencesError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooManyRoleSequencesError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintHasTooManyRoleSequencesError
	/// <summary>
	/// ExternalConstraintHasTooManyRoleSequencesError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError))]
	public sealed class ExternalConstraintHasTooManyRoleSequencesErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooManyRoleSequencesErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSequencesError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError")]
	public  partial class ObjectTypeHasDuplicateNameError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ObjectTypeHasDuplicateNameError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "3743ce4a-674e-4aeb-8e1c-a195fa76a063";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.MetaRelationshipGuidString);
		#endregion

		#region DuplicateNameError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DuplicateNameErrorMetaRoleGuidString = "250dd899-b1d7-40f2-88dc-b0e75c7d94c7";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DuplicateNameErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.DuplicateNameError")]
		public  Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError DuplicateNameError
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectTypeDuplicateNameError)this.GetRolePlayer(DuplicateNameErrorMetaRoleGuid); }
			set { this.SetRolePlayer(DuplicateNameErrorMetaRoleGuid, value); }
		}
		#endregion
		#region ObjectTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ObjectTypeCollectionMetaRoleGuidString = "532ed9c4-f43f-434f-8fab-f4dd2bad747e";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ObjectTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError.ObjectTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType ObjectTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(ObjectTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ObjectTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ObjectTypeHasDuplicateNameError's Generated Constructor Code
	public  partial class ObjectTypeHasDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeHasDuplicateNameError CreateObjectTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ObjectTypeHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(ObjectTypeHasDuplicateNameError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ObjectTypeHasDuplicateNameError CreateAndInitializeObjectTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ObjectTypeHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(ObjectTypeHasDuplicateNameError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ObjectTypeHasDuplicateNameError
	/// <summary>
	/// ObjectTypeHasDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError))]
	public sealed class ObjectTypeHasDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectTypeHasDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ObjectTypeHasDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError")]
	public  partial class FactTypeHasDuplicateNameError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeHasDuplicateNameError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "688413fc-399c-4ebb-b1e0-cc6e302b0e08";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.MetaRelationshipGuidString);
		#endregion

		#region DuplicateNameError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DuplicateNameErrorMetaRoleGuidString = "1c6dcf27-af15-4850-a00d-dfe60594cbfc";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DuplicateNameErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.DuplicateNameError")]
		public  Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError DuplicateNameError
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactTypeDuplicateNameError)this.GetRolePlayer(DuplicateNameErrorMetaRoleGuid); }
			set { this.SetRolePlayer(DuplicateNameErrorMetaRoleGuid, value); }
		}
		#endregion
		#region FactTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeCollectionMetaRoleGuidString = "66f806d4-d65d-4ef6-9a22-72c4044dff83";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError.FactTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeHasDuplicateNameError's Generated Constructor Code
	public  partial class FactTypeHasDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasDuplicateNameError CreateFactTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(FactTypeHasDuplicateNameError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasDuplicateNameError CreateAndInitializeFactTypeHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(FactTypeHasDuplicateNameError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeHasDuplicateNameError
	/// <summary>
	/// FactTypeHasDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError))]
	public sealed class FactTypeHasDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeHasDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading")]
	public  partial class ReadingOrderHasReading : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ReadingOrderHasReading's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "929ef898-db8d-44cf-b2d2-f1b030752b08";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.MetaRelationshipGuidString);
		#endregion

		#region ReadingCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingCollectionMetaRoleGuidString = "f09f87e5-bb49-4cc7-be1d-26104769e721";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingCollection")]
		public  Northface.Tools.ORM.ObjectModel.Reading ReadingCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Reading)this.GetRolePlayer(ReadingCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ReadingOrder's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingOrderMetaRoleGuidString = "6d039c34-74fb-47dd-9dbd-19d7754ad67f";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingOrderMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrderMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrderMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading.ReadingOrder")]
		public  Northface.Tools.ORM.ObjectModel.ReadingOrder ReadingOrder
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReadingOrder)this.GetRolePlayer(ReadingOrderMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingOrderMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ReadingOrderHasReading's Generated Constructor Code
	public  partial class ReadingOrderHasReading
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrderHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrderHasReading CreateReadingOrderHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ReadingOrderHasReading)store.ElementFactory.CreateElementLink(typeof(ReadingOrderHasReading), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrderHasReading CreateAndInitializeReadingOrderHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReadingOrderHasReading)store.ElementFactory.CreateElementLink(typeof(ReadingOrderHasReading), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReadingOrderHasReading
	/// <summary>
	/// ReadingOrderHasReading Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading))]
	public sealed class ReadingOrderHasReadingElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrderHasReadingElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReadingOrderHasReading(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole")]
	public  partial class ReadingOrderHasRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ReadingOrderHasRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "5f244cf8-a0e0-48cc-9e74-ed2ee3c853b0";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.MetaRelationshipGuidString);
		#endregion

		#region RoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleCollectionMetaRoleGuidString = "fb522e84-ba2d-49a0-bd18-37cf1f00ae11";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.RoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.RoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.RoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role RoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(RoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ReadingOrder's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingOrderMetaRoleGuidString = "71a07cfd-e938-414e-a390-1cd6dacd690e";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingOrderMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.ReadingOrderMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.ReadingOrderMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole.ReadingOrder")]
		public  Northface.Tools.ORM.ObjectModel.ReadingOrder ReadingOrder
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReadingOrder)this.GetRolePlayer(ReadingOrderMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingOrderMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ReadingOrderHasRole's Generated Constructor Code
	public  partial class ReadingOrderHasRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrderHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrderHasRole CreateReadingOrderHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ReadingOrderHasRole)store.ElementFactory.CreateElementLink(typeof(ReadingOrderHasRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReadingOrderHasRole CreateAndInitializeReadingOrderHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReadingOrderHasRole)store.ElementFactory.CreateElementLink(typeof(ReadingOrderHasRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReadingOrderHasRole
	/// <summary>
	/// ReadingOrderHasRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole))]
	public sealed class ReadingOrderHasRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReadingOrderHasRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReadingOrderHasRole(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder")]
	public  partial class FactTypeHasReadingOrder : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeHasReadingOrder's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "1170285d-5118-4945-81a9-c6ea63863c39";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.MetaRelationshipGuidString);
		#endregion

		#region ReadingOrderCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingOrderCollectionMetaRoleGuidString = "ac5ae124-5cf2-4c0f-9071-11d57cdf6668";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingOrderCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.ReadingOrderCollection")]
		public  Northface.Tools.ORM.ObjectModel.ReadingOrder ReadingOrderCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReadingOrder)this.GetRolePlayer(ReadingOrderCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingOrderCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeMetaRoleGuidString = "b852113d-a2b6-44ff-82a5-5295e69faedb";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeHasReadingOrder's Generated Constructor Code
	public  partial class FactTypeHasReadingOrder
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasReadingOrder(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasReadingOrder CreateFactTypeHasReadingOrder(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeHasReadingOrder)store.ElementFactory.CreateElementLink(typeof(FactTypeHasReadingOrder), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasReadingOrder CreateAndInitializeFactTypeHasReadingOrder(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeHasReadingOrder)store.ElementFactory.CreateElementLink(typeof(FactTypeHasReadingOrder), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeHasReadingOrder
	/// <summary>
	/// FactTypeHasReadingOrder Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder))]
	public sealed class FactTypeHasReadingOrderElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasReadingOrderElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeHasReadingOrder(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind")]
	public  partial class ModelHasReferenceModeKind : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasReferenceModeKind's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "ecdad33e-fb6f-49a8-84da-abdb9f6d936c";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.MetaRelationshipGuidString);
		#endregion

		#region ReferenceModeKindCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReferenceModeKindCollectionMetaRoleGuidString = "45bfc8c9-bf29-4e6d-81af-89f82a92e126";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeKindCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ReferenceModeKindCollection")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceModeKind ReferenceModeKindCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceModeKind)this.GetRolePlayer(ReferenceModeKindCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReferenceModeKindCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "e7a1e999-7f55-4420-b318-d3be76e3809d";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasReferenceModeKind's Generated Constructor Code
	public  partial class ModelHasReferenceModeKind
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasReferenceModeKind CreateModelHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasReferenceModeKind)store.ElementFactory.CreateElementLink(typeof(ModelHasReferenceModeKind), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasReferenceModeKind CreateAndInitializeModelHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasReferenceModeKind)store.ElementFactory.CreateElementLink(typeof(ModelHasReferenceModeKind), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasReferenceModeKind
	/// <summary>
	/// ModelHasReferenceModeKind Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind))]
	public sealed class ModelHasReferenceModeKindElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasReferenceModeKindElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasReferenceModeKind(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode")]
	public  partial class ModelHasReferenceMode : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasReferenceMode's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "3a05bf70-175f-4dac-9692-ca158806acb2";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.MetaRelationshipGuidString);
		#endregion

		#region ReferenceModeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReferenceModeCollectionMetaRoleGuidString = "47a57da0-d5e9-4291-a418-1fc328d668b3";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ReferenceModeCollection")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceMode ReferenceModeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceMode)this.GetRolePlayer(ReferenceModeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReferenceModeCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "213e68bd-1cdf-4e91-be12-be0a74d334fb";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasReferenceMode's Generated Constructor Code
	public  partial class ModelHasReferenceMode
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasReferenceMode CreateModelHasReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasReferenceMode)store.ElementFactory.CreateElementLink(typeof(ModelHasReferenceMode), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasReferenceMode CreateAndInitializeModelHasReferenceMode(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasReferenceMode)store.ElementFactory.CreateElementLink(typeof(ModelHasReferenceMode), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasReferenceMode
	/// <summary>
	/// ModelHasReferenceMode Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode))]
	public sealed class ModelHasReferenceModeElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasReferenceModeElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasReferenceMode(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind")]
	public  partial class ReferenceModeHasReferenceModeKind : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ReferenceModeHasReferenceModeKind's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "b82e579e-4d0c-4411-9135-8c8b15979355";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.MetaRelationshipGuidString);
		#endregion

		#region Kind's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String KindMetaRoleGuidString = "bfb270b2-dcd7-4637-83af-956b28b9cd83";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid KindMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.KindMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.KindMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.Kind")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceModeKind Kind
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceModeKind)this.GetRolePlayer(KindMetaRoleGuid); }
			set { this.SetRolePlayer(KindMetaRoleGuid, value); }
		}
		#endregion
		#region ReferenceModeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReferenceModeCollectionMetaRoleGuidString = "babae7a3-71e7-426b-a5c3-d8ec45d12df6";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReferenceModeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeCollection")]
		public  Northface.Tools.ORM.ObjectModel.ReferenceMode ReferenceModeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ReferenceMode)this.GetRolePlayer(ReferenceModeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReferenceModeCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ReferenceModeHasReferenceModeKind's Generated Constructor Code
	public  partial class ReferenceModeHasReferenceModeKind
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReferenceModeHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReferenceModeHasReferenceModeKind CreateReferenceModeHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ReferenceModeHasReferenceModeKind)store.ElementFactory.CreateElementLink(typeof(ReferenceModeHasReferenceModeKind), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ReferenceModeHasReferenceModeKind CreateAndInitializeReferenceModeHasReferenceModeKind(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ReferenceModeHasReferenceModeKind)store.ElementFactory.CreateElementLink(typeof(ReferenceModeHasReferenceModeKind), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ReferenceModeHasReferenceModeKind
	/// <summary>
	/// ReferenceModeHasReferenceModeKind Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind))]
	public sealed class ReferenceModeHasReferenceModeKindElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReferenceModeHasReferenceModeKindElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint")]
	public  partial class ModelHasSingleColumnExternalConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasSingleColumnExternalConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "c39ab93e-10fd-4629-a27a-b5560039f36c";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.MetaRelationshipGuidString);
		#endregion

		#region SingleColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String SingleColumnExternalConstraintCollectionMetaRoleGuidString = "e5a1060b-13ab-4ab1-8a12-c6aeeef6f8bf";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid SingleColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint SingleColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint)this.GetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "1769af74-7b45-4b04-b049-ec0b7fded9f0";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasSingleColumnExternalConstraint's Generated Constructor Code
	public  partial class ModelHasSingleColumnExternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasSingleColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasSingleColumnExternalConstraint CreateModelHasSingleColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasSingleColumnExternalConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasSingleColumnExternalConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasSingleColumnExternalConstraint CreateAndInitializeModelHasSingleColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasSingleColumnExternalConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasSingleColumnExternalConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasSingleColumnExternalConstraint
	/// <summary>
	/// ModelHasSingleColumnExternalConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint))]
	public sealed class ModelHasSingleColumnExternalConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasSingleColumnExternalConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasSingleColumnExternalConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint")]
	public  partial class ModelHasMultiColumnExternalConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasMultiColumnExternalConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "b4dc9ac8-f3f3-4280-bf70-32bd74695ea3";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MetaRelationshipGuidString);
		#endregion

		#region MultiColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String MultiColumnExternalConstraintCollectionMetaRoleGuidString = "d1aac147-a429-4d57-a8c7-e658130e1e74";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid MultiColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint MultiColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "8e41a69c-0797-4882-ac2a-2fd39cef5a18";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasMultiColumnExternalConstraint's Generated Constructor Code
	public  partial class ModelHasMultiColumnExternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasMultiColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasMultiColumnExternalConstraint CreateModelHasMultiColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasMultiColumnExternalConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasMultiColumnExternalConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasMultiColumnExternalConstraint CreateAndInitializeModelHasMultiColumnExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasMultiColumnExternalConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasMultiColumnExternalConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasMultiColumnExternalConstraint
	/// <summary>
	/// ModelHasMultiColumnExternalConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint))]
	public sealed class ModelHasMultiColumnExternalConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasMultiColumnExternalConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasMultiColumnExternalConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint")]
	public  partial class FactTypeHasInternalConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeHasInternalConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "c2f9b082-f991-4fbd-9945-d55032e7ce27";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.MetaRelationshipGuidString);
		#endregion

		#region InternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String InternalConstraintCollectionMetaRoleGuidString = "bee9d250-53f3-48b5-b63b-337f545bf988";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid InternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.InternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.InternalConstraint InternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.InternalConstraint)this.GetRolePlayer(InternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(InternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeMetaRoleGuidString = "95904280-42fc-47ce-8ed3-40f58b2deec9";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint.FactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeHasInternalConstraint's Generated Constructor Code
	public  partial class FactTypeHasInternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasInternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasInternalConstraint CreateFactTypeHasInternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeHasInternalConstraint)store.ElementFactory.CreateElementLink(typeof(FactTypeHasInternalConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasInternalConstraint CreateAndInitializeFactTypeHasInternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeHasInternalConstraint)store.ElementFactory.CreateElementLink(typeof(FactTypeHasInternalConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeHasInternalConstraint
	/// <summary>
	/// FactTypeHasInternalConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint))]
	public sealed class FactTypeHasInternalConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasInternalConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeHasInternalConstraint(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError")]
	public  partial class SingleColumnExternalConstraintHasDuplicateNameError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region SingleColumnExternalConstraintHasDuplicateNameError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "36e3a440-8e08-4b01-91cb-9182ec03f621";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.MetaRelationshipGuidString);
		#endregion

		#region DuplicateNameError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DuplicateNameErrorMetaRoleGuidString = "f16cdde1-b246-4528-a9c2-49ba4be7c288";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DuplicateNameErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.DuplicateNameError")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError DuplicateNameError
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)this.GetRolePlayer(DuplicateNameErrorMetaRoleGuid); }
			set { this.SetRolePlayer(DuplicateNameErrorMetaRoleGuid, value); }
		}
		#endregion
		#region SingleColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String SingleColumnExternalConstraintCollectionMetaRoleGuidString = "2c19092d-87b2-4b01-90c3-ffac3a8b4828";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid SingleColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint SingleColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraint)this.GetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(SingleColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region SingleColumnExternalConstraintHasDuplicateNameError's Generated Constructor Code
	public  partial class SingleColumnExternalConstraintHasDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SingleColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SingleColumnExternalConstraintHasDuplicateNameError CreateSingleColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (SingleColumnExternalConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(SingleColumnExternalConstraintHasDuplicateNameError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static SingleColumnExternalConstraintHasDuplicateNameError CreateAndInitializeSingleColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (SingleColumnExternalConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(SingleColumnExternalConstraintHasDuplicateNameError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for SingleColumnExternalConstraintHasDuplicateNameError
	/// <summary>
	/// SingleColumnExternalConstraintHasDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError))]
	public sealed class SingleColumnExternalConstraintHasDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SingleColumnExternalConstraintHasDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.SingleColumnExternalConstraintHasDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError")]
	public  partial class MultiColumnExternalConstraintHasDuplicateNameError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region MultiColumnExternalConstraintHasDuplicateNameError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "ed53d70d-b151-45bb-a0dd-13f50dc475c1";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MetaRelationshipGuidString);
		#endregion

		#region DuplicateNameError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DuplicateNameErrorMetaRoleGuidString = "83d48411-c26c-4755-a726-76ef2716998a";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DuplicateNameErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.DuplicateNameError")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError DuplicateNameError
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)this.GetRolePlayer(DuplicateNameErrorMetaRoleGuid); }
			set { this.SetRolePlayer(DuplicateNameErrorMetaRoleGuid, value); }
		}
		#endregion
		#region MultiColumnExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String MultiColumnExternalConstraintCollectionMetaRoleGuidString = "f2ee7a71-9202-4dc3-9843-ea3a7c1585e3";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid MultiColumnExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint MultiColumnExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraint)this.GetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(MultiColumnExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region MultiColumnExternalConstraintHasDuplicateNameError's Generated Constructor Code
	public  partial class MultiColumnExternalConstraintHasDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintHasDuplicateNameError CreateMultiColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (MultiColumnExternalConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalConstraintHasDuplicateNameError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MultiColumnExternalConstraintHasDuplicateNameError CreateAndInitializeMultiColumnExternalConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (MultiColumnExternalConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(MultiColumnExternalConstraintHasDuplicateNameError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for MultiColumnExternalConstraintHasDuplicateNameError
	/// <summary>
	/// MultiColumnExternalConstraintHasDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError))]
	public sealed class MultiColumnExternalConstraintHasDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultiColumnExternalConstraintHasDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.MultiColumnExternalConstraintHasDuplicateNameError(store, bag);
		}
	}
	#endregion

}
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier")]
	public  partial class EntityTypeHasPreferredIdentifier : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region EntityTypeHasPreferredIdentifier's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4e07ae25-5acd-47b4-b3cd-f92fef621975";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.MetaRelationshipGuidString);
		#endregion

		#region PreferredIdentifier's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String PreferredIdentifierMetaRoleGuidString = "6d26f6c2-85d7-47e5-8bba-54824fbd5ee2";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid PreferredIdentifierMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifier")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence PreferredIdentifier
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSequence)this.GetRolePlayer(PreferredIdentifierMetaRoleGuid); }
			set { this.SetRolePlayer(PreferredIdentifierMetaRoleGuid, value); }
		}
		#endregion
		#region PreferredIdentifierFor's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String PreferredIdentifierForMetaRoleGuidString = "2f929a15-e1d5-4486-81de-d43e7df45405";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid PreferredIdentifierForMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierFor")]
		public  Northface.Tools.ORM.ObjectModel.ObjectType PreferredIdentifierFor
		{
			get { return (Northface.Tools.ORM.ObjectModel.ObjectType)this.GetRolePlayer(PreferredIdentifierForMetaRoleGuid); }
			set { this.SetRolePlayer(PreferredIdentifierForMetaRoleGuid, value); }
		}
		#endregion
	}
	#region EntityTypeHasPreferredIdentifier's Generated Constructor Code
	public  partial class EntityTypeHasPreferredIdentifier
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public EntityTypeHasPreferredIdentifier(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static EntityTypeHasPreferredIdentifier CreateEntityTypeHasPreferredIdentifier(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (EntityTypeHasPreferredIdentifier)store.ElementFactory.CreateElementLink(typeof(EntityTypeHasPreferredIdentifier), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static EntityTypeHasPreferredIdentifier CreateAndInitializeEntityTypeHasPreferredIdentifier(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (EntityTypeHasPreferredIdentifier)store.ElementFactory.CreateElementLink(typeof(EntityTypeHasPreferredIdentifier), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for EntityTypeHasPreferredIdentifier
	/// <summary>
	/// EntityTypeHasPreferredIdentifier Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier))]
	public sealed class EntityTypeHasPreferredIdentifierElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public EntityTypeHasPreferredIdentifierElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier(store, bag);
		}
	}
	#endregion

}



