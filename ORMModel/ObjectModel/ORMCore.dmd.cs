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
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid, Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasError.ErrorCollectionMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid);
			MetaRoles.Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid);
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
		#region ConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintMoveableCollection ConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ModelMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid); }
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
		public Northface.Tools.ORM.ObjectModel.Constraint PreferredIdentifier
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
				return (Northface.Tools.ORM.ObjectModel.Constraint)o;
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
		#region ExternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraintMoveableCollection ExternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ExternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.FactTypeCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuid); }
		}
		#endregion
		#region ReadingCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReadingMoveableCollection ReadingCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ReadingMoveableCollection(this, Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid); }
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
		#region InternalConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.InternalConstraintMoveableCollection InternalConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.InternalConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactTypeMetaRoleGuid, Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuid); }
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.Constraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.Constraint")]
	public abstract partial class Constraint : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region Constraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "31230436-a925-46d9-b64c-24e028aa5d60";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.Constraint.MetaClassGuidString);
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
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ModelMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ORMModel)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ModelMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ModelHasConstraint), newRoles);
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
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError), newRoles);
				}
			}
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
	#region Collection Classes for Constraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.Constraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
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
		public ConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
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
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.Constraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.Constraint))))
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
		public void CopyTo(Northface.Tools.ORM.ObjectModel.Constraint[] array, System.Int32 index)
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
		/// <returns>Northface.Tools.ORM.ObjectModel.Constraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.Constraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.Constraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Constraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.Constraint value)
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
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Constraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.Constraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Constraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.Constraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Constraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.Constraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.Constraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.Constraint value)
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
		public void Move(Northface.Tools.ORM.ObjectModel.Constraint rolePlayer, System.Int32 newPosition)
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
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.Constraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region Constraint's Generated Constructor Code
	public abstract partial class Constraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected Constraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
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
	public abstract partial class InternalConstraint : Northface.Tools.ORM.ObjectModel.Constraint
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

		#region RoleSet's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet RoleSet
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet), newRoles);
				}
			}
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
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.InternalFactConstraint), newRoles);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.MandatoryConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.MandatoryConstraint")]
	public  partial class MandatoryConstraint : Northface.Tools.ORM.ObjectModel.InternalConstraint
	{
		#region MandatoryConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "b09e7a1f-184e-4e7f-88e3-6ca763ded9eb";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.MandatoryConstraint.MetaClassGuidString);
		#endregion

	}
	#region MandatoryConstraint's Generated Constructor Code
	public  partial class MandatoryConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MandatoryConstraint CreateMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (MandatoryConstraint)store.ElementFactory.CreateElement(typeof(MandatoryConstraint));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static MandatoryConstraint CreateAndInitializeMandatoryConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (MandatoryConstraint)store.ElementFactory.CreateElement(typeof(MandatoryConstraint), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for MandatoryConstraint
	/// <summary>
	/// MandatoryConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.MandatoryConstraint))]
	public sealed class MandatoryConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MandatoryConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.MandatoryConstraint(store, bag);
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
	public  partial class RingConstraint : Northface.Tools.ORM.ObjectModel.InternalConstraint
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraint")]
	public abstract partial class ExternalConstraint : Northface.Tools.ORM.ObjectModel.Constraint
	{
		#region ExternalConstraint's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "0d75242d-1462-4936-ad30-e1ce61a6ba3f";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraint.MetaClassGuidString);
		#endregion

		#region FactTypeCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection FactTypeCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.FactTypeMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.FactTypeCollectionMetaRoleGuid); }
		}
		#endregion
		#region RoleSetCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSetMoveableCollection RoleSetCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSetMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid); }
		}
		#endregion
		#region TooFewRoleSetsError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError TooFewRoleSetsError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError), newRoles);
				}
			}
		}
		#endregion
		#region TooManyRoleSetsError's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError TooManyRoleSetsError
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ExternalConstraint
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ExternalConstraint Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ExternalConstraintMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
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
		public ExternalConstraintMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
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
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraint))))
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
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ExternalConstraint[] array, System.Int32 index)
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
		/// <returns>Northface.Tools.ORM.ObjectModel.ExternalConstraint at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraint this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraint to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ExternalConstraint value)
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
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraint to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ExternalConstraint value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraint to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ExternalConstraint value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraint to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ExternalConstraint value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraint to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ExternalConstraint value)
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
		public void Move(Northface.Tools.ORM.ObjectModel.ExternalConstraint rolePlayer, System.Int32 newPosition)
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
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ExternalConstraint rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ExternalConstraint's Generated Constructor Code
	public abstract partial class ExternalConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ExternalConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ExternalUniquenessConstraint")]
	public  partial class ExternalUniquenessConstraint : Northface.Tools.ORM.ObjectModel.ExternalConstraint
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EqualityConstraint.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.EqualityConstraint")]
	public  partial class EqualityConstraint : Northface.Tools.ORM.ObjectModel.ExternalConstraint
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
	public  partial class ExclusionConstraint : Northface.Tools.ORM.ObjectModel.ExternalConstraint
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

		#region ExclusionType's Generated  Field Code
		#region ExclusionType's Generated  MetaAttribute Code
		/// <summary>
		/// MetaAttribute Guid String
		/// </summary>
		public const System.String ExclusionTypeMetaAttributeGuidString = "0018bab8-a32d-4cc1-a12a-3a18626dc942";

		/// <summary>
		/// MetaAttribute Guid
		/// </summary>
		public static readonly System.Guid ExclusionTypeMetaAttributeGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExclusionConstraint.ExclusionTypeMetaAttributeGuidString);
		#endregion

		#region ExclusionType's Generated Property Code

		private Northface.Tools.ORM.ObjectModel.ExclusionType exclusionTypePropertyStorage = Northface.Tools.ORM.ObjectModel.ExclusionType.Exclusion;
		
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.EnumerationDomainAttribute(EnumerationType=typeof(Northface.Tools.ORM.ObjectModel.ExclusionType),DefaultEnumerationValueName="Exclusion")]
		[Microsoft.VisualStudio.Modeling.MetaAttributeAttribute(FieldHandlerType=typeof(ExclusionConstraintExclusionTypeFieldHandler))]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExclusionConstraint.ExclusionTypeMetaAttributeGuidString, "Northface.Tools.ORM.ObjectModel.ExclusionConstraint.ExclusionType")]
		public  Northface.Tools.ORM.ObjectModel.ExclusionType ExclusionType
		{
			get
			{
				return exclusionTypePropertyStorage;
			}
		
			set
			{
				exclusionConstraintExclusionTypeFieldHandler.SetFieldValue(this, value, false, Microsoft.VisualStudio.Modeling.TransactionManager.CommandFactory);
			}
		}
		#endregion

		#region ExclusionConstraintExclusionTypeFieldHandler Generated Code
		/// <summary>
		/// FieldHandler for ExclusionConstraint.ExclusionType field
		/// </summary>
		private static ExclusionConstraintExclusionTypeFieldHandler	exclusionConstraintExclusionTypeFieldHandler	= ExclusionConstraintExclusionTypeFieldHandler.Instance;

		/// <summary>
		/// Implement the field handler for ExclusionConstraint.ExclusionType
		/// </summary>
		[System.CLSCompliant(false)]
		public sealed partial class ExclusionConstraintExclusionTypeFieldHandler : Microsoft.VisualStudio.Modeling.TypedModelElementInlineFieldHandler<Northface.Tools.ORM.ObjectModel.ExclusionConstraint,Northface.Tools.ORM.ObjectModel.ExclusionType>
		{
			/// <summary>
			/// Constructor
			/// </summary>
			private ExclusionConstraintExclusionTypeFieldHandler() { }

			/// <summary>
			/// Returns the singleton instance of the ExclusionConstraint.ExclusionType field handler
			/// </summary>
			/// <value>ExclusionConstraintExclusionTypeFieldHandler</value>
			public static ExclusionConstraintExclusionTypeFieldHandler Instance
			{
				get
				{
					if (Northface.Tools.ORM.ObjectModel.ExclusionConstraint.exclusionConstraintExclusionTypeFieldHandler != null)
					{
						return Northface.Tools.ORM.ObjectModel.ExclusionConstraint.exclusionConstraintExclusionTypeFieldHandler;
					}
					else
					{
						// The static constructor in ExclusionConstraint will assign this value to
						// Northface.Tools.ORM.ObjectModel.ExclusionConstraint.exclusionConstraintExclusionTypeFieldHandler, so just instantiate one and return it
						return new ExclusionConstraintExclusionTypeFieldHandler();
					}
				}
			}

			/// <summary>
			/// Returns the meta attribute id for the ExclusionConstraint.ExclusionType field handler
			/// </summary>
			/// <value>Guid</value>
			public sealed override System.Guid Id
			{
				get
				{
					return Northface.Tools.ORM.ObjectModel.ExclusionConstraint.ExclusionTypeMetaAttributeGuid;
				}
			}
			/// <summary>
			/// Gets the value of the attribute as it exists in the element
			/// </summary>
			/// <param name="element">the ExclusionConstraint</param>
			protected sealed override Northface.Tools.ORM.ObjectModel.ExclusionType GetValue(Northface.Tools.ORM.ObjectModel.ExclusionConstraint element)
			{
				return element.exclusionTypePropertyStorage;
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
			protected sealed override bool SetValue(Northface.Tools.ORM.ObjectModel.ExclusionConstraint element, Northface.Tools.ORM.ObjectModel.ExclusionType value, Microsoft.VisualStudio.Modeling.CommandFactory commandFactory, bool allowDuplicates, ref Northface.Tools.ORM.ObjectModel.ExclusionType oldValue)
			{
				oldValue = element.exclusionTypePropertyStorage;
				if (allowDuplicates || oldValue != value)
				{
					OnValueChanging(element, oldValue, value);
					element.exclusionTypePropertyStorage = value;
					OnValueChanged(element, oldValue, value);
					return true;
				}
				return false;
			}
		
		}
		#endregion
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
	public  partial class SubsetConstraint : Northface.Tools.ORM.ObjectModel.ExternalConstraint
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
		#region ConstraintRoleSetCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSetMoveableCollection ConstraintRoleSetCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSetMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.RoleCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.ConstraintRoleSetCollectionMetaRoleGuid); }
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSet")]
	public abstract partial class ConstraintRoleSet : Microsoft.VisualStudio.Modeling.NamedElement
	{
		#region ConstraintRoleSet's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "d9ab8420-8604-4016-8827-0d59c77b47f8";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet.MetaClassGuidString);
		#endregion

		#region RoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.RoleMoveableCollection RoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.ConstraintRoleSetCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.RoleCollectionMetaRoleGuid); }
		}
		#endregion
	}
	#region Collection Classes for ConstraintRoleSet
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ConstraintRoleSet Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ConstraintRoleSetMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
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
		public ConstraintRoleSetMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
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
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet))))
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
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet[] array, System.Int32 index)
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
		/// <returns>Northface.Tools.ORM.ObjectModel.ConstraintRoleSet at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSet this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSet)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSet to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet value)
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
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSet to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSet to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSet to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ConstraintRoleSet value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSet to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet value)
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
		public void Move(Northface.Tools.ORM.ObjectModel.ConstraintRoleSet rolePlayer, System.Int32 newPosition)
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
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ConstraintRoleSet rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ConstraintRoleSet's Generated Constructor Code
	public abstract partial class ConstraintRoleSet
	{
		/// <summary>
		/// Constructor
		/// </summary>
		protected ConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet")]
	public  partial class ExternalConstraintRoleSet : Northface.Tools.ORM.ObjectModel.ConstraintRoleSet
	{
		#region ExternalConstraintRoleSet's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "34a3c709-1021-4afe-80db-6302f4c681ea";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet.MetaClassGuidString);
		#endregion

		#region ExternalConstraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraint ExternalConstraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet), newRoles);
				}
			}
		}
		#endregion
	}
	#region Collection Classes for ExternalConstraintRoleSet
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ExternalConstraintRoleSetMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
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
		public ExternalConstraintRoleSetMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
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
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))))
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
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet[] array, System.Int32 index)
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
		/// <returns>Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet value)
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
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet value)
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
		public void Move(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet rolePlayer, System.Int32 newPosition)
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
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ExternalConstraintRoleSet's Generated Constructor Code
	public  partial class ExternalConstraintRoleSet
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintRoleSet CreateExternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (ExternalConstraintRoleSet)store.ElementFactory.CreateElement(typeof(ExternalConstraintRoleSet));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintRoleSet CreateAndInitializeExternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintRoleSet)store.ElementFactory.CreateElement(typeof(ExternalConstraintRoleSet), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintRoleSet
	/// <summary>
	/// ExternalConstraintRoleSet Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet))]
	public sealed class ExternalConstraintRoleSetElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintRoleSetElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet")]
	public  partial class InternalConstraintRoleSet : Northface.Tools.ORM.ObjectModel.ConstraintRoleSet
	{
		#region InternalConstraintRoleSet's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "8264311c-af73-4e80-ae8a-b312d6861a50";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet.MetaClassGuidString);
		#endregion

		#region InternalConstraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.InternalConstraint InternalConstraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.InternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet), newRoles);
				}
			}
		}
		#endregion
	}
	#region InternalConstraintRoleSet's Generated Constructor Code
	public  partial class InternalConstraintRoleSet
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalConstraintRoleSet CreateInternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (InternalConstraintRoleSet)store.ElementFactory.CreateElement(typeof(InternalConstraintRoleSet));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalConstraintRoleSet CreateAndInitializeInternalConstraintRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (InternalConstraintRoleSet)store.ElementFactory.CreateElement(typeof(InternalConstraintRoleSet), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for InternalConstraintRoleSet
	/// <summary>
	/// InternalConstraintRoleSet Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet))]
	public sealed class InternalConstraintRoleSetElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalConstraintRoleSetElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet(store, bag);
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
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactTypeMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.FactType)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactTypeMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasReading), newRoles);
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
			get { return new Northface.Tools.ORM.ObjectModel.RoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ReadingHasRole.ReadingCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ReadingHasRole.RoleCollectionMetaRoleGuid); }
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError")]
	public  partial class TooFewRoleSetsError : Northface.Tools.ORM.ObjectModel.ModelError
	{
		#region TooFewRoleSetsError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "a2f137b4-5973-4e4f-90aa-6bc936bf8e79";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError.MetaClassGuidString);
		#endregion

		#region Constraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraint Constraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError), newRoles);
				}
			}
		}
		#endregion
	}
	#region TooFewRoleSetsError's Generated Constructor Code
	public  partial class TooFewRoleSetsError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooFewRoleSetsError CreateTooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (TooFewRoleSetsError)store.ElementFactory.CreateElement(typeof(TooFewRoleSetsError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooFewRoleSetsError CreateAndInitializeTooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (TooFewRoleSetsError)store.ElementFactory.CreateElement(typeof(TooFewRoleSetsError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for TooFewRoleSetsError
	/// <summary>
	/// TooFewRoleSetsError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError))]
	public sealed class TooFewRoleSetsErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooFewRoleSetsErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError.MetaClassGuidString, "Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError")]
	public  partial class TooManyRoleSetsError : Northface.Tools.ORM.ObjectModel.ModelError
	{
		#region TooManyRoleSetsError's Generated MetaClass Code
		/// <summary>
		/// MetaClass Guid String
		/// </summary>
		public new const System.String MetaClassGuidString = "c0e9f4b2-c8e8-4126-bde2-ec5de62d2b58";
		/// <summary>
		/// MetaClass Guid
		/// </summary>
		public static readonly new System.Guid MetaClassGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError.MetaClassGuidString);
		#endregion

		#region Constraint's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ExternalConstraint Constraint
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid);
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
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)o;
			}
			set
			{
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid);
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
					newRoles[0] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuid, value);
					newRoles[1] = new Microsoft.VisualStudio.Modeling.RoleAssignment(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError), newRoles);
				}
			}
		}
		#endregion
	}
	#region TooManyRoleSetsError's Generated Constructor Code
	public  partial class TooManyRoleSetsError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooManyRoleSetsError CreateTooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store)
		{
			return (TooManyRoleSetsError)store.ElementFactory.CreateElement(typeof(TooManyRoleSetsError));
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static TooManyRoleSetsError CreateAndInitializeTooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (TooManyRoleSetsError)store.ElementFactory.CreateElement(typeof(TooManyRoleSetsError), assignments);
		}
	}
	#endregion
	#region Class Factory Creator for TooManyRoleSetsError
	/// <summary>
	/// TooManyRoleSetsError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError))]
	public sealed class TooManyRoleSetsErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public TooManyRoleSetsErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError(store, bag);
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

		#region ConstraintCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintMoveableCollection ConstraintCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuid); }
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasConstraint")]
	public  partial class ModelHasConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ModelHasConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "de71244e-4628-49b5-a566-8cfcfd71a3ba";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.MetaRelationshipGuidString);
		#endregion

		#region Model's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ModelMetaRoleGuidString = "96a03455-4e3d-4e68-9042-485a0ee2dd0d";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ModelMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ModelMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ModelMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasConstraint.Model")]
		public  Northface.Tools.ORM.ObjectModel.ORMModel Model
		{
			get { return (Northface.Tools.ORM.ObjectModel.ORMModel)this.GetRolePlayer(ModelMetaRoleGuid); }
			set { this.SetRolePlayer(ModelMetaRoleGuid, value); }
		}
		#endregion
		#region ConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintCollectionMetaRoleGuidString = "3467caef-671f-42da-b72a-f47675ecbfc5";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ModelHasConstraint.ConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.Constraint ConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Constraint)this.GetRolePlayer(ConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ModelHasConstraint's Generated Constructor Code
	public  partial class ModelHasConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasConstraint CreateModelHasConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ModelHasConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ModelHasConstraint CreateAndInitializeModelHasConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ModelHasConstraint)store.ElementFactory.CreateElementLink(typeof(ModelHasConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ModelHasConstraint
	/// <summary>
	/// ModelHasConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ModelHasConstraint))]
	public sealed class ModelHasConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelHasConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ModelHasConstraint(store, bag);
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
	public  partial class ExternalFactConstraint : Microsoft.VisualStudio.Modeling.ElementLink
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

		#region ExternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ExternalConstraintCollectionMetaRoleGuidString = "b8e2c375-a1c8-408b-90bc-b4ce0ba65b6f";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ExternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.ExternalConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.ExternalConstraint ExternalConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)this.GetRolePlayer(ExternalConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ExternalConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactTypeCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeCollectionMetaRoleGuidString = "8eb5f466-0534-4d78-882f-b7e11d84d63e";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.FactTypeCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.FactTypeCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalFactConstraint.FactTypeCollection")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactTypeCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ConstrainedRoleCollection's Generated Accessor Code
		/// <summary>
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRoleMoveableCollection ConstrainedRoleCollection
		{
			get { return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRoleMoveableCollection(this, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.FactConstraintCollectionMetaRoleGuid, Northface.Tools.ORM.ObjectModel.ExternalRoleConstraint.ConstrainedRoleCollectionMetaRoleGuid); }
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
	public  partial class ExternalFactConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalFactConstraint CreateExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(ExternalFactConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalFactConstraint CreateAndInitializeExternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(ExternalFactConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalFactConstraint
	/// <summary>
	/// ExternalFactConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalFactConstraint))]
	public sealed class ExternalFactConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalFactConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalFactConstraint(store, bag);
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
		public  Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole ConstrainedRoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole)this.GetRolePlayer(ConstrainedRoleCollectionMetaRoleGuid); }
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet")]
	public  partial class InternalConstraintHasRoleSet : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region InternalConstraintHasRoleSet's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4f723a25-7972-4248-8893-a57a71f6d86c";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.MetaRelationshipGuidString);
		#endregion

		#region RoleSet's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleSetMetaRoleGuidString = "465d8a69-2608-4fa7-8fa2-68a310fb82a1";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleSetMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSetMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.RoleSet")]
		public  Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet RoleSet
		{
			get { return (Northface.Tools.ORM.ObjectModel.InternalConstraintRoleSet)this.GetRolePlayer(RoleSetMetaRoleGuid); }
			set { this.SetRolePlayer(RoleSetMetaRoleGuid, value); }
		}
		#endregion
		#region InternalConstraint's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String InternalConstraintMetaRoleGuidString = "7ed4a1e7-75bd-46bc-a81c-66bd8184cc51";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid InternalConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet.InternalConstraint")]
		public  Northface.Tools.ORM.ObjectModel.InternalConstraint InternalConstraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.InternalConstraint)this.GetRolePlayer(InternalConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(InternalConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region InternalConstraintHasRoleSet's Generated Constructor Code
	public  partial class InternalConstraintHasRoleSet
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalConstraintHasRoleSet CreateInternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (InternalConstraintHasRoleSet)store.ElementFactory.CreateElementLink(typeof(InternalConstraintHasRoleSet), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalConstraintHasRoleSet CreateAndInitializeInternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (InternalConstraintHasRoleSet)store.ElementFactory.CreateElementLink(typeof(InternalConstraintHasRoleSet), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for InternalConstraintHasRoleSet
	/// <summary>
	/// InternalConstraintHasRoleSet Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet))]
	public sealed class InternalConstraintHasRoleSetElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalConstraintHasRoleSetElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.InternalConstraintHasRoleSet(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet")]
	public  partial class ExternalConstraintHasRoleSet : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalConstraintHasRoleSet's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "4d952086-d820-4023-9207-14302e77b703";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.MetaRelationshipGuidString);
		#endregion

		#region RoleSetCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleSetCollectionMetaRoleGuidString = "152a01ce-3754-4b69-9f9e-036762f25a58";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleSetCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.RoleSetCollection")]
		public  Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet RoleSetCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraintRoleSet)this.GetRolePlayer(RoleSetCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleSetCollectionMetaRoleGuid, value); }
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
		public static readonly System.Guid ExternalConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet.ExternalConstraint")]
		public  Northface.Tools.ORM.ObjectModel.ExternalConstraint ExternalConstraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)this.GetRolePlayer(ExternalConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ExternalConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalConstraintHasRoleSet's Generated Constructor Code
	public  partial class ExternalConstraintHasRoleSet
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasRoleSet CreateExternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalConstraintHasRoleSet)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasRoleSet), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasRoleSet CreateAndInitializeExternalConstraintHasRoleSet(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintHasRoleSet)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasRoleSet), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintHasRoleSet
	/// <summary>
	/// ExternalConstraintHasRoleSet Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet))]
	public sealed class ExternalConstraintHasRoleSetElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasRoleSetElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintHasRoleSet(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole")]
	public  partial class ConstraintRoleSetHasRole : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ConstraintRoleSetHasRole's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "b66ae3b4-c404-486c-933a-fd23eea3c3d7";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.MetaRelationshipGuidString);
		#endregion

		#region RoleCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String RoleCollectionMetaRoleGuidString = "43f43911-662e-480d-b566-8025ccb1f673";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid RoleCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.RoleCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.RoleCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.RoleCollection")]
		public  Northface.Tools.ORM.ObjectModel.Role RoleCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Role)this.GetRolePlayer(RoleCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(RoleCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region ConstraintRoleSetCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintRoleSetCollectionMetaRoleGuidString = "dea36a6c-6706-41ee-bb34-61bc9a5416d2";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintRoleSetCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.ConstraintRoleSetCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.ConstraintRoleSetCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole.ConstraintRoleSetCollection")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintRoleSet ConstraintRoleSetCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSet)this.GetRolePlayer(ConstraintRoleSetCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintRoleSetCollectionMetaRoleGuid, value); }
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
	#region Collection Classes for ConstraintRoleSetHasRole
	/// <summary>
	/// Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole Collection class, strongly-typed collection
	/// </summary>
	[System.CLSCompliant(true)]
	public sealed partial class ConstraintRoleSetHasRoleMoveableCollection : Microsoft.VisualStudio.Modeling.IMoveableCollection
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
		public ConstraintRoleSetHasRoleMoveableCollection(Microsoft.VisualStudio.Modeling.ModelElement counterpart, System.Guid sourceMetaRoleGuid, System.Guid targetMetaRoleGuid)
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
				if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (value == null || (value.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !value.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
			if (rolePlayer == null || (rolePlayer.GetType() != typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole) && !rolePlayer.GetType().IsSubclassOf(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))))
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
		public void CopyTo(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole[] array, System.Int32 index)
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
		/// <returns>Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole at that index</returns>
		public Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole this[System.Int32 index]
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole)(((System.Collections.IList)this)[index]); }
			set { ((System.Collections.IList)this)[index] = value as System.Object; }
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole to add to the list</param>
		/// <returns>index where object was added</returns>
		public System.Int32 Add(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole value)
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
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole to locate in the list</param>
		/// <returns>true if object is contained, false otherwise</returns>
		public System.Boolean Contains(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole value)
		{
			return ((System.Collections.IList)this).Contains(value as System.Object);
		}
		/// <summary>
		/// Determines the index of a specific item in the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole to locate in the list</param>
		/// <returns>index of object</returns>
		public System.Int32 IndexOf(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole value)
		{
			return ((System.Collections.IList)this).IndexOf(value as System.Object);
		}
		/// <summary>
		/// Inserts an item to the list at the specified position
		/// </summary>
		/// <param name="index">The zero-based index at which the value should be inserted</param>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole to insert into the list</param>
		public void Insert(System.Int32 index, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole value)
		{
			((System.Collections.IList)this).Insert(index, value as System.Object);
		}
		/// <summary>
		/// Removes the first occurrence of a specific object from the list
		/// </summary>
		/// <param name="value">The Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole to remove from the list</param>
		public void Remove(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole value)
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
		public void Move(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole rolePlayer, System.Int32 newPosition)
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
		public void ReplaceAt(System.Int32 position, Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole rolePlayer)
		{
			((Microsoft.VisualStudio.Modeling.IMoveableCollection)this).ReplaceAt(position, rolePlayer as Microsoft.VisualStudio.Modeling.ModelElement);
		}

	}
	#endregion

	#region ConstraintRoleSetHasRole's Generated Constructor Code
	public  partial class ConstraintRoleSetHasRole
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintRoleSetHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintRoleSetHasRole CreateConstraintRoleSetHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ConstraintRoleSetHasRole)store.ElementFactory.CreateElementLink(typeof(ConstraintRoleSetHasRole), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintRoleSetHasRole CreateAndInitializeConstraintRoleSetHasRole(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ConstraintRoleSetHasRole)store.ElementFactory.CreateElementLink(typeof(ConstraintRoleSetHasRole), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ConstraintRoleSetHasRole
	/// <summary>
	/// ConstraintRoleSetHasRole Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole))]
	public sealed class ConstraintRoleSetHasRoleElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintRoleSetHasRoleElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ConstraintRoleSetHasRole(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReading")]
	public  partial class FactTypeHasReading : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region FactTypeHasReading's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "43d4abf4-8bef-41da-9597-049a6630f300";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.MetaRelationshipGuidString);
		#endregion

		#region ReadingCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ReadingCollectionMetaRoleGuidString = "e8edc6a4-0e36-4db9-8d52-5ed08e06d963";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ReadingCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReading.ReadingCollection")]
		public  Northface.Tools.ORM.ObjectModel.Reading ReadingCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Reading)this.GetRolePlayer(ReadingCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ReadingCollectionMetaRoleGuid, value); }
		}
		#endregion
		#region FactType's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String FactTypeMetaRoleGuidString = "892d6985-b064-4452-a17a-dec195324576";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=true, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.FactTypeHasReading.FactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeMetaRoleGuid, value); }
		}
		#endregion
	}
	#region FactTypeHasReading's Generated Constructor Code
	public  partial class FactTypeHasReading
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasReading CreateFactTypeHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (FactTypeHasReading)store.ElementFactory.CreateElementLink(typeof(FactTypeHasReading), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static FactTypeHasReading CreateAndInitializeFactTypeHasReading(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (FactTypeHasReading)store.ElementFactory.CreateElementLink(typeof(FactTypeHasReading), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for FactTypeHasReading
	/// <summary>
	/// FactTypeHasReading Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.FactTypeHasReading))]
	public sealed class FactTypeHasReadingElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FactTypeHasReadingElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.FactTypeHasReading(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError")]
	public  partial class ExternalConstraintHasTooFewRoleSetsError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalConstraintHasTooFewRoleSetsError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "dcf38e2c-fe6e-4f89-8cb2-44bb1e512856";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.MetaRelationshipGuidString);
		#endregion

		#region TooFewRoleSetsError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String TooFewRoleSetsErrorMetaRoleGuidString = "438baa58-0cff-42b2-93f5-38dffa0544a1";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid TooFewRoleSetsErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.TooFewRoleSetsError")]
		public  Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError TooFewRoleSetsError
		{
			get { return (Northface.Tools.ORM.ObjectModel.TooFewRoleSetsError)this.GetRolePlayer(TooFewRoleSetsErrorMetaRoleGuid); }
			set { this.SetRolePlayer(TooFewRoleSetsErrorMetaRoleGuid, value); }
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
		public static readonly System.Guid ConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.ConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError.Constraint")]
		public  Northface.Tools.ORM.ObjectModel.ExternalConstraint Constraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)this.GetRolePlayer(ConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalConstraintHasTooFewRoleSetsError's Generated Constructor Code
	public  partial class ExternalConstraintHasTooFewRoleSetsError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooFewRoleSetsError CreateExternalConstraintHasTooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalConstraintHasTooFewRoleSetsError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooFewRoleSetsError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooFewRoleSetsError CreateAndInitializeExternalConstraintHasTooFewRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintHasTooFewRoleSetsError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooFewRoleSetsError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintHasTooFewRoleSetsError
	/// <summary>
	/// ExternalConstraintHasTooFewRoleSetsError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError))]
	public sealed class ExternalConstraintHasTooFewRoleSetsErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooFewRoleSetsErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooFewRoleSetsError(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError")]
	public  partial class ExternalConstraintHasTooManyRoleSetsError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ExternalConstraintHasTooManyRoleSetsError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "5aec6871-993f-4373-8627-fb5eba073edd";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.MetaRelationshipGuidString);
		#endregion

		#region TooManyRoleSetsError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String TooManyRoleSetsErrorMetaRoleGuidString = "b65ff045-247b-4890-aa03-44d53c1832ad";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid TooManyRoleSetsErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=true, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.TooManyRoleSetsError")]
		public  Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError TooManyRoleSetsError
		{
			get { return (Northface.Tools.ORM.ObjectModel.TooManyRoleSetsError)this.GetRolePlayer(TooManyRoleSetsErrorMetaRoleGuid); }
			set { this.SetRolePlayer(TooManyRoleSetsErrorMetaRoleGuid, value); }
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
		public static readonly System.Guid ConstraintMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=false, IsAggregate=false, IsNavigableFrom=true, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.ConstraintMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError.Constraint")]
		public  Northface.Tools.ORM.ObjectModel.ExternalConstraint Constraint
		{
			get { return (Northface.Tools.ORM.ObjectModel.ExternalConstraint)this.GetRolePlayer(ConstraintMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ExternalConstraintHasTooManyRoleSetsError's Generated Constructor Code
	public  partial class ExternalConstraintHasTooManyRoleSetsError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooManyRoleSetsError CreateExternalConstraintHasTooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ExternalConstraintHasTooManyRoleSetsError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooManyRoleSetsError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ExternalConstraintHasTooManyRoleSetsError CreateAndInitializeExternalConstraintHasTooManyRoleSetsError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ExternalConstraintHasTooManyRoleSetsError)store.ElementFactory.CreateElementLink(typeof(ExternalConstraintHasTooManyRoleSetsError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ExternalConstraintHasTooManyRoleSetsError
	/// <summary>
	/// ExternalConstraintHasTooManyRoleSetsError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError))]
	public sealed class ExternalConstraintHasTooManyRoleSetsErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExternalConstraintHasTooManyRoleSetsErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ExternalConstraintHasTooManyRoleSetsError(store, bag);
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
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError")]
	public  partial class ConstraintHasDuplicateNameError : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region ConstraintHasDuplicateNameError's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "462e847e-840d-4766-b251-d896a18fa599";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.MetaRelationshipGuidString);
		#endregion

		#region DuplicateNameError's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String DuplicateNameErrorMetaRoleGuidString = "66386508-14ce-48b5-aa0c-ac971dca74a8";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid DuplicateNameErrorMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.DuplicateNameError")]
		public  Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError DuplicateNameError
		{
			get { return (Northface.Tools.ORM.ObjectModel.ConstraintDuplicateNameError)this.GetRolePlayer(DuplicateNameErrorMetaRoleGuid); }
			set { this.SetRolePlayer(DuplicateNameErrorMetaRoleGuid, value); }
		}
		#endregion
		#region ConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String ConstraintCollectionMetaRoleGuidString = "9d5edc77-403f-47ef-a003-b56d7ad92bdb";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid ConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError.ConstraintCollection")]
		public  Northface.Tools.ORM.ObjectModel.Constraint ConstraintCollection
		{
			get { return (Northface.Tools.ORM.ObjectModel.Constraint)this.GetRolePlayer(ConstraintCollectionMetaRoleGuid); }
			set { this.SetRolePlayer(ConstraintCollectionMetaRoleGuid, value); }
		}
		#endregion
	}
	#region ConstraintHasDuplicateNameError's Generated Constructor Code
	public  partial class ConstraintHasDuplicateNameError
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintHasDuplicateNameError CreateConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (ConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(ConstraintHasDuplicateNameError), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static ConstraintHasDuplicateNameError CreateAndInitializeConstraintHasDuplicateNameError(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (ConstraintHasDuplicateNameError)store.ElementFactory.CreateElementLink(typeof(ConstraintHasDuplicateNameError), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for ConstraintHasDuplicateNameError
	/// <summary>
	/// ConstraintHasDuplicateNameError Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError))]
	public sealed class ConstraintHasDuplicateNameErrorElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ConstraintHasDuplicateNameErrorElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.ConstraintHasDuplicateNameError(store, bag);
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
		public new const System.String MetaRelationshipGuidString = "515b27c0-e9c6-4c0d-a852-8edcf3a794d5";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.MetaRelationshipGuidString);
		#endregion

		#region PreferredIdentifier's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String PreferredIdentifierMetaRoleGuidString = "ccfab332-362c-4ab4-bb0b-bfcb577bcaad";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid PreferredIdentifierMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifier")]
		public  Northface.Tools.ORM.ObjectModel.Constraint PreferredIdentifier
		{
			get { return (Northface.Tools.ORM.ObjectModel.Constraint)this.GetRolePlayer(PreferredIdentifierMetaRoleGuid); }
			set { this.SetRolePlayer(PreferredIdentifierMetaRoleGuid, value); }
		}
		#endregion
		#region PreferredIdentifierFor's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String PreferredIdentifierForMetaRoleGuidString = "37f4cab9-3ba7-44cb-a366-94ba8b2e020c";
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
namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// 
	/// </summary>
	[System.CLSCompliant(true)]
	[System.Serializable]
	[Microsoft.VisualStudio.Modeling.MetaRelationship("83ad9e12-0e90-47cd-8e2f-a79f8d9c7288")]
	[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.MetaRelationshipGuidString, "Northface.Tools.ORM.ObjectModel.InternalFactConstraint")]
	public  partial class InternalFactConstraint : Microsoft.VisualStudio.Modeling.ElementLink
	{
		#region InternalFactConstraint's Generated MetaRelationship Code
		/// <summary>
		/// MetaRelationship Guid String
		/// </summary>
		public new const System.String MetaRelationshipGuidString = "e3b478ea-fe89-4367-a178-cce609e667b2";
		/// <summary>
		/// MetaRelationship Guid
		/// </summary>
		public static readonly new System.Guid MetaRelationshipGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.MetaRelationshipGuidString);
		#endregion

		#region InternalConstraintCollection's Generated MetaRole Code
		/// <summary>
		/// MetaRole Guid String
		/// </summary>
		public const System.String InternalConstraintCollectionMetaRoleGuidString = "47c8046a-b913-4730-bd83-c30b188672a2";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid InternalConstraintCollectionMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.Many)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollectionMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.InternalFactConstraint.InternalConstraintCollection")]
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
		public const System.String FactTypeMetaRoleGuidString = "d0f8626a-095d-4a76-bbf1-389d9a62b1bc";
		/// <summary>
		/// MetaRole Guid
		/// </summary>
		public static readonly System.Guid FactTypeMetaRoleGuid = new System.Guid(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactTypeMetaRoleGuidString);
		/// <summary>
		/// 
		/// </summary>
		[Microsoft.VisualStudio.Modeling.MetaRole(IsOptional=false, IsOrdered=true, IsAggregate=false, IsNavigableFrom=false, PropagateRemove=false, PropagateCopy=false, Cardinality=Microsoft.VisualStudio.Modeling.Cardinality.One)]
		[Microsoft.VisualStudio.Modeling.MetaObject(Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactTypeMetaRoleGuidString, "Northface.Tools.ORM.ObjectModel.InternalFactConstraint.FactType")]
		public  Northface.Tools.ORM.ObjectModel.FactType FactType
		{
			get { return (Northface.Tools.ORM.ObjectModel.FactType)this.GetRolePlayer(FactTypeMetaRoleGuid); }
			set { this.SetRolePlayer(FactTypeMetaRoleGuid, value); }
		}
		#endregion
	}
	#region InternalFactConstraint's Generated Constructor Code
	public  partial class InternalFactConstraint
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag) : base(store, bag)
		{
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalFactConstraint CreateInternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers)
		{
			return (InternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(InternalFactConstraint), rolePlayers);
		}
		/// <summary>
		/// Class Factory
		/// </summary>
		public static InternalFactConstraint CreateAndInitializeInternalFactConstraint(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.RoleAssignment[] rolePlayers, Microsoft.VisualStudio.Modeling.AttributeAssignment[] assignments)
		{
			return (InternalFactConstraint)store.ElementFactory.CreateElementLink(typeof(InternalFactConstraint), rolePlayers, assignments);
		}
	}
	#endregion
	#region Class Factory Creator for InternalFactConstraint
	/// <summary>
	/// InternalFactConstraint Class Factory Creator
	/// </summary>
	[Microsoft.VisualStudio.Modeling.ElementFactoryCreatorFor(typeof(Northface.Tools.ORM.ObjectModel.InternalFactConstraint))]
	public sealed class InternalFactConstraintElementFactoryCreator : Microsoft.VisualStudio.Modeling.ElementFactoryCreator
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalFactConstraintElementFactoryCreator()
		{
		}
		/// <summary>
		/// Class Factory Create Method
		/// </summary>
		public override Microsoft.VisualStudio.Modeling.ModelElement Create(Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.ModelDataBag bag)
		{
			return new Northface.Tools.ORM.ObjectModel.InternalFactConstraint(store, bag);
		}
	}
	#endregion

}



