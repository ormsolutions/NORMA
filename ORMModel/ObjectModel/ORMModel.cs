using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class ORMModel
	{
		#region Entity- and ValueType specific collections
		/// <summary>
		/// All of the entity types in the object types collection.
		/// </summary>
		[CLSCompliant(false)]
		public IEnumerable<ObjectType> EntityTypeCollection
		{
			get
			{
				return RestrictedObjectTypeCollection(false);
			}
		}
		/// <summary>
		/// All of the value types in the object types collection.
		/// </summary>
		[CLSCompliant(false)]
		public IEnumerable<ObjectType> ValueTypeCollection
		{
			get
			{
				return RestrictedObjectTypeCollection(true);
			}
		}
		private IEnumerable<ObjectType> RestrictedObjectTypeCollection(bool valueType)
		{
			foreach (ObjectType obj in ObjectTypeCollection)
			{
				if (obj.IsValueType == valueType)
				{
					yield return obj;
				}
			}
		}
		#endregion // Entity- and ValueType specific collections
		#region MergeContext functions
		/// <summary>
		/// Support adding root elements and constraints directly to the design surface
		/// </summary>
		/// <param name="elementGroupPrototype"></param>
		/// <param name="protoElement"></param>
		/// <returns></returns>
		protected override bool CanAddChildElement(ElementGroupPrototype elementGroupPrototype, ProtoElementBase protoElement)
		{
			if (protoElement == null)
			{
				return false;
			}
			MetaClassInfo classInfo = Store.MetaDataDirectory.FindMetaClass(protoElement.MetaClassId);
			return classInfo.IsDerivedFrom(RootType.MetaClassGuid) || classInfo.IsDerivedFrom(ExternalConstraint.MetaClassGuid);
		}
		/// <summary>
		/// Attach a deserialized ObjectType, FactType, or Constraint to the model.
		/// Called after prototypes for these items are dropped onto the diagram
		/// from the toolbox.
		/// </summary>
		/// <param name="sourceElement">The element being added</param>
		/// <param name="elementGroup">The element describing all of the created elements</param>
		public override void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			base.MergeRelate(sourceElement, elementGroup);
			ObjectType objectType;
			FactType factType;
			Constraint constraint;
			if (null != (objectType = sourceElement as ObjectType))
			{
				objectType.Model = this;
			}
			else if (null != (factType = sourceElement as FactType))
			{
				factType.Model = this;
			}
			else if (null != (constraint = sourceElement as Constraint))
			{
				constraint.Model = this;
			}
		}
		#endregion // MergeContext functions
	}
	#region NamedElementDictionary integration
	public partial class ORMModel : INamedElementDictionaryParent
	{
		#region INamedElementDictionaryParent implementation
		private NamedElementDictionary myObjectTypesDictionary = null;
		private NamedElementDictionary myFactTypesDictionary = null;
		private NamedElementDictionary myConstraintsDictionary = null;
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns>Dictionaries for object types, fact types, and constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			if (parentMetaRoleGuid == ModelHasObjectType.ModelMetaRoleGuid)
			{
				if (myObjectTypesDictionary == null)
				{
					myObjectTypesDictionary = new NamedElementDictionary();
				}
				return myObjectTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasFactType.ModelMetaRoleGuid)
			{
				if (myFactTypesDictionary == null)
				{
					myFactTypesDictionary = new NamedElementDictionary();
				}
				return myFactTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasConstraint.ModelMetaRoleGuid)
			{
				if (myConstraintsDictionary == null)
				{
					myConstraintsDictionary = new NamedElementDictionary();
				}
				return myConstraintsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetAllowDuplicateNamesContextKey(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns></returns>
		protected object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		#endregion // INamedElementDictionaryParent implementation
	}
	public partial class ModelHasObjectType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns ObjectTypeCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return ObjectTypeCollection; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasFactType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns ObjectTypeCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return FactTypeCollection; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasConstraint : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns ObjectTypeCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return ConstraintCollection as ExternalConstraint; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	#endregion // NamedElementDictionary integration
}   