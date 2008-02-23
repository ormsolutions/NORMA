// Uncomment the following line for the OIALModel to binarize unaries from the ORMModel.
#define USE_UNBINARIZED_UNARIES
#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
#region Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using System.Globalization;
using System.Collections;
using System.Diagnostics;
using ObjModel = Neumont.Tools.ORM.ObjectModel;
#endregion // Using Directives

namespace Neumont.Tools.ORM.OIALModel
{
	#region OIALModel Rules and Validation
	public partial class OIALModel
	{
		#region Validation Methods
		#region Temporary Model Validation
		// These Validation methods were added to meet end of Quarter deadline.
		// They should only be considered a Hack not final implementation.

		/// <summary>
		/// This is the Delay Validation Method for ORMModel.
		/// </summary>
		/// <param name="element">The ModelElement you want validated.</param>
		public static void DelayValidateModel(ModelElement element)
		{
			ValidateModel(element, null);
		}
		/// <summary>
		/// This method validates the ORMModel.
		/// We make a call to ProcessModelForTopLevelTypes
		/// to gather a live OIAL model.
		/// </summary>
		public static void ValidateModel(ModelElement element, INotifyElementAdded notifyAdded)
		{
			ORMModel model = element as ORMModel;
			OIALModel oial = OIALModelHasORMModel.GetOIALModel(model);
			if (oial == null)
			{
				oial = new OIALModel(model.Store);
				oial.ORMModel = model;
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(oial, true);
				}
			}
			oial.ProcessModelForTopLevelTypes();
		}
		#endregion //Temporary Model Validation
		#region Removed to meet Quarter Deadline
		// These Validation methods were removed due to time contraints.
		// They are supposed to handle individual model changes

		//public static void DelayValidateObjectTypeAdd(ModelElement element)
		//{
		//    ValidateObjectTypeAdd(element, null);
		//}
		//public static void ValidateObjectTypeAdd(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    ObjectType objectType = element as ObjectType;
		//    ORMModel model = objectType.Model;
		//    OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//        OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//        OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//    if (oial == null)
		//    {
		//        oial = OIALModel.CreateOIALModel(model.Store);
		//        oial.ORMModel = model;
		//        if (notifyAdded != null)
		//        {
		//            notifyAdded.ElementAdded(oial, true);
		//        }
		//    }
		//    oial.ProcessModelForTopLevelTypes();
		//}
		//public static void DelayValidateObjectTypeChange(ModelElement element)
		//{
		//    ValidateObjectTypeChange(element, null);
		//}
		//public static void ValidateObjectTypeChange(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    ObjectType objectType = element as ObjectType;
		//    if (!objectType.IsRemoved)
		//    {
		//        ORMModel model = objectType.Model;
		//        OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//            OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//            OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//        if (oial == null)
		//        {
		//            oial = OIALModel.CreateOIALModel(model.Store);
		//            oial.ORMModel = model;
		//            if (notifyAdded != null)
		//            {
		//                notifyAdded.ElementAdded(oial, true);
		//            }
		//        }
		//        oial.ProcessModelForTopLevelTypes();
		//    }
		//}
		//public static void DelayValidateFactTypeAdd(ModelElement element)
		//{
		//    ValidateFactTypeAdd(element, null);
		//}
		//public static void ValidateFactTypeAdd(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    FactType factType = element as FactType;
		//    ORMModel model = factType.Model;
		//    OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//        OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//        OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//    if (oial == null)
		//    {
		//        oial = OIALModel.CreateOIALModel(model.Store);
		//        oial.ORMModel = model;
		//        if (notifyAdded != null)
		//        {
		//            notifyAdded.ElementAdded(oial, true);
		//        }
		//    }
		//    oial.ProcessModelForTopLevelTypes();
		//}
		//public static void DelayValidateFactTypeChange(ModelElement element)
		//{
		//    ValidateFactTypeChange(element, null);
		//}
		//public static void ValidateFactTypeChange(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    FactType factType = element as FactType;
		//    if (!factType.IsRemoved)
		//    {
		//        ORMModel model = factType.Model;
		//        OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//            OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//            OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//        if (oial == null)
		//        {
		//            oial = OIALModel.CreateOIALModel(model.Store);
		//            oial.ORMModel = model;
		//            if (notifyAdded != null)
		//            {
		//                notifyAdded.ElementAdded(oial, true);
		//            }
		//        }
		//        oial.ProcessModelForTopLevelTypes();
		//    }
		//}
		//public static void DelayValidateModelHasSetConstraintAdd(ModelElement element)
		//{
		//    ValidateModelHasSetConstraintAdd(element, null);
		//}
		//public static void ValidateModelHasSetConstraintAdd(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    ModelHasSetConstraint setConstraint = element as ModelHasSetConstraint;
		//    ORMModel model = setConstraint.Model;
		//    OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//        OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//        OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//    if (oial == null)
		//    {
		//        oial = OIALModel.CreateOIALModel(model.Store);
		//        oial.ORMModel = model;
		//        if (notifyAdded != null)
		//        {
		//            notifyAdded.ElementAdded(oial, true);
		//        }
		//    }
		//    oial.ProcessModelForTopLevelTypes();
		//}
		//public static void DelayValidateModelHasSetConstraintChange(ModelElement element)
		//{
		//    ValidateModelHasSetConstraintChange(element, null);
		//}
		//public static void ValidateModelHasSetConstraintChange(ModelElement element, INotifyElementAdded notifyAdded)
		//{
		//    ModelHasSetConstraint setConstraint = element as ModelHasSetConstraint;
		//    if (!setConstraint.IsRemoved)
		//    {
		//        ORMModel model = setConstraint.Model;
		//        OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
		//            OIALModelHasORMModel.ORMModelMetaRoleGuid,
		//            OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
		//        if (oial == null)
		//        {
		//            oial = OIALModel.CreateOIALModel(model.Store);
		//            oial.ORMModel = model;
		//            if (notifyAdded != null)
		//            {
		//                notifyAdded.ElementAdded(oial, true);
		//            }
		//        }
		//        oial.ProcessModelForTopLevelTypes();
		//    }
		//}
		#endregion // Removed to meet Quarter Deadline
		#endregion // Validation Methods
		#region OIALModel Rules
		/// <summary>
		/// This class checks for ConceptTypeParents are not parents of themselves.
		/// </summary>
		private static partial class CheckConceptTypeParentExclusiveMandatory
		{
			/// <summary>
			/// Checks if a ConceptType is its own parent.
			/// </summary>
			/// <param name="childConceptType"></param>
			private static void ProcessConceptType(ConceptType childConceptType)
			{
				if (childConceptType != null)
				{
					bool hasParentModel = childConceptType.Model != null;
					bool hasParentConceptType = childConceptType.AbsorbingConceptType != null;
					if ((hasParentModel && hasParentConceptType) || (!hasParentModel && !hasParentConceptType))
					{
						// UNDONE: Localize this
						// throw new InvalidOperationException("A ConceptType must have exactly one parent (either the OIALModel, or a ConceptType that absorbed it).");
					}
				}
			}
			/// <summary>
			/// This rule fires when the OIALModel has a conceptType added to it.
			/// </summary>
			[RuleOn(typeof(OIALModelHasConceptType))] // AddRule
			private sealed partial class OIALModelHasConceptTypeAddRule : AddRule
			{
				/// <summary>
				/// When the ConceptType is added we need to process it.
				/// </summary>
				public sealed override void ElementAdded(ElementAddedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as OIALModelHasConceptType).ConceptType);
				}
			}
			/// <summary>
			/// This rule fires when the OIALModel Has a ConceptType Removed.
			/// </summary>
			[RuleOn(typeof(OIALModelHasConceptType))] // DeleteRule
			private sealed partial class OIALModelHasConceptTypeDeleteRule : DeleteRule
			{
				/// <summary>
				/// When the ConcepType is removed we process it.
				/// </summary>
				public sealed override void ElementDeleted(ElementDeletedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as OIALModelHasConceptType).ConceptType);
				}
			}
			/// <summary>
			/// This rule fires wen the OIALModel absorbs a ConceptType.
			/// </summary>
			[RuleOn(typeof(ConceptTypeAbsorbedConceptType))] // AddRule
			private sealed partial class ConceptTypeAbsorbedConceptTypeAddRule : AddRule
			{
				/// <summary>
				/// When a ConceptType absorbed another ConceptType we process it.
				/// </summary>
				public sealed override void ElementAdded(ElementAddedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as ConceptTypeAbsorbedConceptType).AbsorbedConceptType);
				}
			}
			/// <summary>
			/// This rule fires when a ConceptType that has been absorded is removed from its parent.
			/// </summary>
			[RuleOn(typeof(ConceptTypeAbsorbedConceptType))] // DeleteRule
			private sealed partial class ConceptTypeAbsorbedConceptTypeDeleteRule : DeleteRule
			{
				/// <summary>
				/// When an concepttype is removed we need to process it.
				/// </summary>
				public sealed override void ElementDeleted(ElementDeletedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as ConceptTypeAbsorbedConceptType).AbsorbedConceptType);
				}
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType))] // AddRule
		private sealed partial class ModelHasObjectTypeAddRule : AddRule
		{
			/// <summary>
			/// When an ObjectType is added we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement((e.ModelElement as ModelHasObjectType).Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType))] // DeletingRule
		private sealed partial class ModelHasObjectTypeDeletingRule : DeletingRule
		{
			/// <summary>
			/// When an ObjectType is removed from a model we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ObjectType objectType = (e.ModelElement as ModelHasObjectType).ObjectType;
				ORMModel model = objectType.Model;
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for any changes being made to an ObjectType.
		/// </summary>
		[RuleOn(typeof(ObjectType))] // ChangeRule
		private sealed partial class ObjectTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// When an ObjectType is changes we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				ORMModel model = (e.ModelElement as ObjectType).Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This rule listens for when a FactType is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))] // AddRule
		private sealed partial class ModelHasFactTypeAddRule : AddRule
		{
			/// <summary>
			/// When a FactType is added to the Model we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement((e.ModelElement as ModelHasFactType).Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when a FactType is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))] // DeletingRule
		private sealed partial class ModelHasFactTypeDeletingRule : DeletingRule
		{
			/// <summary>
			/// When a FactType is removed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				FactType fact = (e.ModelElement as ModelHasFactType).FactType;
				ORMModel model = fact.Model;
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This model listens for when A SetConstraint is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))] // AddRule
		private sealed partial class ModelHasSetConstraintAddRule : AddRule
		{
			/// <summary>
			/// When a SetConstraint is added we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				FrameworkDomainModel.DelayValidateElement(setConstraint.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when A SetConstraint is changed.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))] // ChangeRule
		private sealed partial class ModelHasSetConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a SetConstraint is changed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This rule listens for when a SetConstraint is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))] // DeletingRule
		private sealed partial class ModelHasSetConstraintDeletingRule : DeletingRule
		{
			/// <summary>
			/// When a SetConstraint is removed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType plays a role.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))] // AddRule
		private sealed partial class ObjectTypePlaysRoleAddRule : AddRule
		{
			/// <summary>
			/// When an ObjectType plays a role we DelayValidate the Model.
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole objectRole = e.ModelElement as ObjectTypePlaysRole;
				ObjectType objectType = objectRole.RolePlayer;
				ORMModel model = objectType.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This rule fires when An ObjectType no longer plays a role.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))] // DeletingRule
		private sealed partial class ObjectTypePlaysRoleDeletingRule : DeletingRule
		{
			/// <summary>
			/// When an ObjectType plays role is removed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ObjectTypePlaysRole objectRole = e.ModelElement as ObjectTypePlaysRole;
				ObjectType objectType = objectRole.RolePlayer;
				FrameworkDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when a ConstraintRoleSequence is added to a Role.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed partial class ConstraintRoleSequenceHasRoleAddRule : AddRule
		{
			/// <summary>
			/// When a ConstraintRoleSequence is added to a Role we DelayValidate the Model.
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole constraintSequence = e.ModelElement as ConstraintRoleSequenceHasRole;
				RoleBase rolebase = constraintSequence.Role;
				FactType factType = rolebase.FactType;
				ORMModel model = factType.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}

			}
		}
		/// <summary>
		/// This rule fires when a ConstraintRoleSequence is removed from a Role.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeletingRule
		private sealed partial class ConstraintRoleSequenceHasRoleDeletingRule : DeletingRule
		{
			/// <summary>
			/// When a ConstraintRoleSeqeunce is removed from a Role we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ConstraintRoleSequenceHasRole constraintSequence = e.ModelElement as ConstraintRoleSequenceHasRole;
				RoleBase rolebase = constraintSequence.Role;
				FactType factType = rolebase.FactType;
				FrameworkDomainModel.DelayValidateElement(factType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when a UniquenessContraint is changed.
		/// </summary>
		[RuleOn(typeof(UniquenessConstraint))] // ChangeRule
		private sealed partial class UniquenessConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a UniquenessConstraint is changed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				UniquenessConstraint constraint = e.ModelElement as UniquenessConstraint;
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
				int rolesCount = roles.Count;
				int i;
				for (i = 0; i < rolesCount; ++i)
				{
					RoleBase role = roles[i];
					ORMModel model = role.FactType.Model;
					if (model != null)
					{
						FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
					}
				}
			}
		}
		/// <summary>
		/// This rule fires when a MandatoryConstraint is changed.
		/// </summary>
		[RuleOn(typeof(MandatoryConstraint))] // ChangeRule
		private sealed partial class MandatoryConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a MandatoryConstraint is changed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				MandatoryConstraint constraint = e.ModelElement as MandatoryConstraint;
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
				int rolesCount = roles.Count;
				int i;
				for (i = 0; i < rolesCount; ++i)
				{
					RoleBase role = roles[i];
					ORMModel model = role.FactType.Model;
					if (model != null)
					{
						FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
					}
				}
			}
		}
		/// <summary>
		/// This rule fires when a RoleBase is changed.
		/// </summary>
		[RuleOn(typeof(RoleBase))] // ChangeRule
		private sealed partial class RoleBaseChangeRule : ChangeRule
		{
			/// <summary>
			/// When a RoleBase is changed we DelayValidate the Model.
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				RoleBase role = e.ModelElement as RoleBase;
				FactType fact = role.FactType;
				ORMModel model = fact.Model;
				if (model != null)
				{
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		#endregion // OIALModel Rules
		#region Deserialization FixupListeners
		/// <summary>
		/// This accessor returns an OialObjectTypeFixupListener.
		/// </summary>
		public static IDeserializationFixupListener ORMModelHasObjectTypeFixupListener
		{
			get { return new OialObjectTypeFixupListener(); }
		}
		/// <summary>
		/// This class is the ObjectType fixuplistener.
		/// We listen for ObjectTypes as they deserialize and DelayValidate the Model.
		/// </summary>
		private sealed class OialObjectTypeFixupListener : DeserializationFixupListener<ObjectType>
		{
			/// <summary>
			/// OialObjectTypeFixupListener Constructor.
			/// </summary>
			public OialObjectTypeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// When an ObjectType is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(ObjectType element, Store store, INotifyElementAdded notifyAdded)
			{
				ObjectType objectType = element as ObjectType;
				ORMModel model = objectType.Model;
				OIALModel oil = OIALModelHasORMModel.GetOIALModel(model);
				if (oil == null)
				{
					oil = new OIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				FrameworkDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This accessor returns an OialFactTypeFixupListener.
		/// </summary>
		public static IDeserializationFixupListener ORMModelHasFactTypeFixupListener
		{
			get { return new OialFactTypeFixupListener(); }
		}
		/// <summary>
		/// This FixupListener listens for FactTypes that are being Deserialized.
		/// </summary>
		private sealed class OialFactTypeFixupListener : DeserializationFixupListener<FactType>
		{
			/// <summary>
			/// OialFactTypeFixupListener Constructor.
			/// </summary>
			public OialFactTypeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// When a FactType is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				FactType fact = element as FactType;
				ORMModel model = fact.Model;
				OIALModel oil = OIALModelHasORMModel.GetOIALModel(model);
				if (oil == null)
				{
					oil = new OIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This accessor returns an OialModelHasSetConstraintFixupListener.
		/// </summary>
		public static IDeserializationFixupListener ORMModelModelHasSetConstraintFixupListener
		{
			get { return new OialModelHasSetConstraintFixupListener(); }
		}
		/// <summary>
		/// This FixupListener listens for SetConstraints during deserialization.
		/// </summary>
		private sealed class OialModelHasSetConstraintFixupListener : DeserializationFixupListener<ModelHasSetConstraint>
		{
			/// <summary>
			/// OialModelHasSetConstraintFixupListener Constructor.
			/// </summary>
			public OialModelHasSetConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// When a SetConstraint is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(ModelHasSetConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				ModelHasSetConstraint setConstraint = element as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				OIALModel oil = OIALModelHasORMModel.GetOIALModel(model);
				if (oil == null)
				{
					oil = new OIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		#endregion // Deserialization FixupListeners
	}
	#endregion // OIALModel Rules and Validation
	#region Live OIAL Implementation
	public partial class OIALModel
	{
		#region Core OIAL Implementation
		#region Initializers
		/// <summary>
		/// A <see cref="SortedDictionary"/> which keeps track of the Guids of <see cref="FactType"/> objects which
		/// are absorbed into <see cref="ObjectType"/> objects and their corresponding details, encapsulated
		/// in an <see cref="AbsorbedFactType"/> structure.
		/// </summary>
		private SortedDictionary<Guid, AbsorbedFactType> myAbsorbedFactTypes = null;
		/// <summary>
		/// A <see cref="SortedDictionary&lt;Guid, Guid&gt;"/> which keeps track of the Guids of <see cref="ObjectType"/> objects and
		/// the Guids of other <see cref="ObjectType"/> objects into which they are absorbed. The first Guid
		/// is the ObjectType being absorbed; the second Guid is the absorbing object.
		/// </summary>
		private SortedDictionary<Guid, Guid> myAbsorbedObjectTypes = null;
		private SortedDictionary<Guid, MandatoryConstraintModality> myAbsorbedObjectTypeMandatories = null;
		/// <summary>
		/// This <see cref="List&lt;Guid&gt;"/> contains the IDs of all the top-level types of the diagrams. Top-level
		/// types are <see cref="ObjectType"/>s which map to <see cref="ConceptType"/> which are direct children of
		/// this <see cref="OIALModel"/> objects.
		/// </summary>
		private List<Guid> myTopLevelTypes = null;
#if USE_UNBINARIZED_UNARIES
		private bool myUnariesExist = false;
#endif
		/// <summary>
		/// Processes the current ORM Model by determining which facts and objects
		/// should be noted by this <see cref="PrimaryElementTracker"/>
		/// </summary>
		/// <param name="model">The current <see cref="ORMModel"/> represented by the diagram</param>
		private void ProcessModelForTopLevelTypes()
		{
			Store store = this.Store;
			this.Regenerating = true;
			this.Regenerating = false;

			// Clears the OIALModel previous to re-processing.
			LinkedElementCollection<ConceptType> thisConceptTypeCollection = ConceptTypeCollection;
			thisConceptTypeCollection.Clear();
			InformationTypeFormatCollection.Clear();
			ChildSequenceConstraintCollection.Clear();
			ORMModel model = this.ORMModel;
			LinkedElementCollection<FactType> modelFactTypes = model.FactTypeCollection;
			LinkedElementCollection<ObjectType> modelObjectTypes = model.ObjectTypeCollection;

			// TODO: Monitor this area for bugs in fact set constraint collection.
			FactTypeAbsorption(modelFactTypes);
			ObjectTypeAbsorption(modelObjectTypes);
			TopLevelObjectTypes(modelObjectTypes);
			InformationTypeFormats(store, modelObjectTypes);

			// TODO: Easier way to do this? We must get the object type for each top-level type's ID
			// and pass this to the ConceptTypes() method, which generates our concept types.
			IElementDirectory elementDirectory = store.ElementDirectory;
			for (int i = 0, topLevelCount = myTopLevelTypes.Count; i < topLevelCount; ++i)
			{
				ConceptTypes(store, (elementDirectory.GetElement(myTopLevelTypes[i]) as ObjectType), null);
			}
			// Generate the information types and concept type refs for the Concept Types we have just created.
			int conceptTypeCount = thisConceptTypeCollection.Count;
			for (int i = 0; i < conceptTypeCount; ++i)
			{
				InformationTypesAndConceptTypeRefs(store, thisConceptTypeCollection[i]);
			}
			GenerateExternalConstraints(store, model);
		}
		#endregion // Initializers
		#region ORMToOIAL Algorithms
		/// <summary>
		/// Determines to which object types all one-to-one fact types in the diagram (after being
		/// co-referenced) are absorbed.
		/// </summary>
		/// <param name="modelFactTypes">
		/// A <see cref="FactTypeMoveableCollection"/> which represents
		/// all of the <see cref="FactType"/> objects on the current diagram.
		/// </param>
		private void FactTypeAbsorption(LinkedElementCollection<FactType> modelFactTypes)
		{
			// Check if the Dictionary of factTypes is null; if so, instantiate it.
			if (myAbsorbedFactTypes == null)
			{
				myAbsorbedFactTypes = new SortedDictionary<Guid, AbsorbedFactType>();
			}
			else
			{
				myAbsorbedFactTypes.Clear();
			}

			// These two Roles are used to get the first and second role of the
			// current FactType
			Role firstRole, secondRole = null;
			// The two Guids are used to store to which ObjectType (towardsGuid)
			// a FactType (refGuid) should be absorbed.
			Guid absorberGuid = Guid.Empty;
			// The default FactAbsorptionType for a fact absorption
			FactAbsorptionType type = FactAbsorptionType.FactOnly;
			// Loop through each FactType in the model's FactTypeCollection
			// These should already be CoReferenced.
			foreach (FactType factType in modelFactTypes)
			{
				// We are not interested in meta-FactTypes represented by subtype relationships
				if (factType is SubtypeFact)
				{
					continue;
				}
				// When you add a reference mode for an object, a new object and fact type are created. Although
				// we can see this fact type in the model's fact type collection, it does not have any roles
				// associated with it. Therefore we must do this check that the count of the rolecollection is equal to 2.
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				int roleCount = roles.Count;
#if USE_UNBINARIZED_UNARIES
				if (roleCount == 1)
				{
					//Guid absorberId = roles[0].Role.RolePlayer.Id;
					//myAbsorbedFactTypes.Add(factType.Id, new AbsorbedFactType(absorberId, FactAbsorptionType.FactOnly));
					myUnariesExist = true;
				}
#endif
				if (roleCount == 2)
				{
					bool hasSpanningUniqueness = false;
					foreach (SetConstraint scc in factType.SetConstraintCollection)
					{
						UniquenessConstraint uConstraint = scc as UniquenessConstraint;
						if (uConstraint != null && uConstraint.Modality == ConstraintModality.Alethic && uConstraint.RoleCollection.Count > 2)
						{
							hasSpanningUniqueness = true;
							break;
						}
					}
					if (hasSpanningUniqueness)
					{
						continue;
					}

					firstRole = roles[0].Role;	// CHANGE: From Role to RoleBase
					secondRole = roles[1].Role;	// CHANGE: From Role to RoleBase
					RoleMultiplicity firstMultiplicity = firstRole.Multiplicity;
					RoleMultiplicity secondMultiplicity = secondRole.Multiplicity;

					// If the fact type's multiplicity is one-to-one
					if (GetAlethicInternalConstraintsCount(factType, ConstraintType.InternalUniqueness) == 2)
					{
						int mandatoryRoleCount = GetAlethicInternalConstraintsCount(factType, ConstraintType.SimpleMandatory);
						// If only one role is mandatory
						if (mandatoryRoleCount == 1)
						{
							Role nonMandatoryRole = null;
							// Find the RolePlayer which does not have the mandatory role constraint,
							// because it usually absorbs the object type that has the mandatory
							// role constraint
							nonMandatoryRole = firstRole.IsMandatory && firstRole.MandatoryConstraintModality == ConstraintModality.Alethic ? secondRole : firstRole;
							Role mandatoryRole = nonMandatoryRole.OppositeRole.Role;

							// If the nonMandatoryRole's RolePlayer has other functional roles
							// that are not part of the primary identifier, absorb toward that
							// ObjectType
							ObjectType rolePlayer = nonMandatoryRole.RolePlayer;
							ObjectType oppositePlayer = mandatoryRole.RolePlayer;
							// If a fact type is detached from its Object Types we do not want to process it.
							if (rolePlayer == null || oppositePlayer == null)
							{
								continue;
							}
							int functionalRoleCount = GetFunctionalRoleCount(rolePlayer.PlayedRoleCollection, nonMandatoryRole);

							if (functionalRoleCount > 0)
							{
								absorberGuid = rolePlayer.Id;
								type = FactAbsorptionType.Fully;
							}
							// Otherwise, absorb toward the ObjectType that has the mandatory
							// role constraint
							else
							{
								absorberGuid = oppositePlayer.Id;
								type = FactAbsorptionType.FactOnly;
							}
						}
						// If there are zero or two mandatory role constraints
						else
						{
							ObjectType firstObject = firstRole.RolePlayer;
							ObjectType secondObject = secondRole.RolePlayer;

							UniquenessConstraint firstUniquenessConstraint = firstObject.PreferredIdentifier;
							bool isFirstRolePlayerPreferredIdentifierFact = false;
							if (firstUniquenessConstraint != null)
							{
								LinkedElementCollection<FactType> firstUniquenessConstraintFactTypeCollection =
	firstObject.PreferredIdentifier.FactTypeCollection;
								Debug.Assert(firstUniquenessConstraintFactTypeCollection.Count > 0);
								isFirstRolePlayerPreferredIdentifierFact = firstUniquenessConstraintFactTypeCollection[0] == factType;
							}

							bool isSecondRolePlayerPreferredIdentifierFact = false;

							if (!isFirstRolePlayerPreferredIdentifierFact)
							{
								UniquenessConstraint secondUniquenessConstraint =
									secondObject.PreferredIdentifier;
								if (secondUniquenessConstraint != null)
								{
									LinkedElementCollection<FactType> secondUniquenessConstraintFactTypeCollection =
										secondUniquenessConstraint.FactTypeCollection;
									Debug.Assert(secondUniquenessConstraintFactTypeCollection.Count > 0);
									isSecondRolePlayerPreferredIdentifierFact = secondUniquenessConstraintFactTypeCollection[0] == factType;
								}
							}

							int firstFunctionalRoleCount = GetFunctionalNonDependentRoleCount(firstObject.PlayedRoleCollection, firstRole);
							int secondFunctionalRoleCount = GetFunctionalNonDependentRoleCount(secondObject.PlayedRoleCollection, secondRole);

							// Compares the number of the functional roles played by the first
							// object type to the number of functional roles played by the second
							// object type. If the first is greater than or equal to the second,
							// absorb into the first.
							if ((firstFunctionalRoleCount >= secondFunctionalRoleCount || isSecondRolePlayerPreferredIdentifierFact) &&
								!isFirstRolePlayerPreferredIdentifierFact)
							{
								absorberGuid = firstObject.Id;
							}
							// Otherwise, absorb into the second.
							else
							{
								absorberGuid = secondObject.Id;
							}
							type = mandatoryRoleCount == 0 ? FactAbsorptionType.FactOnly : FactAbsorptionType.Fully;
						}
						myAbsorbedFactTypes.Add(factType.Id, new AbsorbedFactType(absorberGuid, type));
					}
				}
			}
		}
		/// <summary>
		/// Determines to which object types all object types in the diagram (after being
		/// co-referenced) are absorbed.
		/// </summary>
		/// <param name="modelObjectTypes">
		/// A <see cref="ObjectTypeMoveableCollection"/> which represents
		/// all of the <see cref="ObjectType"/> objects on the current diagram.
		/// </param>
		private void ObjectTypeAbsorption(LinkedElementCollection<ObjectType> modelObjectTypes)
		{
			// Check if the Dictionary of absorbed ObjectTypes is null; if so, instantiate it.
			if (myAbsorbedObjectTypes == null || myAbsorbedObjectTypeMandatories == null)
			{
				myAbsorbedObjectTypes = new SortedDictionary<Guid, Guid>();
				myAbsorbedObjectTypeMandatories = new SortedDictionary<Guid, MandatoryConstraintModality>();
			}
			else
			{
				myAbsorbedObjectTypes.Clear();
				myAbsorbedObjectTypeMandatories.Clear();
			}
			// This Guid is used to store what ObjectType is absorbing the current ObjectType
			// being checked in the foreach loop
			Guid absorberGuid = Guid.Empty;
			foreach (ObjectType objectType in modelObjectTypes)
			{
				absorberGuid = Guid.Empty;
				MandatoryConstraintModality modality = MandatoryConstraintModality.NotMandatory;

				if (!objectType.IsIndependent)
				{
					// If the ObjectType is not independent and is a Subtype, absorb the ObjectType
					// into its supertype
					ObjectType superType;
					if ((superType = GetSupertype(objectType)) != null)
					{
						// Absorb into the first immediate supertype of this object type
						absorberGuid = superType.Id;
					}
					else
					{
						// Create a list of all the FactTypes that this current ObjectType plays Roles
						// in that that ObjectType is functionally dependent on and
						// must play that role (i.e. there is a mandatory role constraint)
						IList<FactType> mandatoryDirectFactTypes = new List<FactType>();
						//IEnumerable<FactType> mandatoryDirectFactTypes = GetFunctionalRoles(objectType.PlayedRoleCollection, null);
						foreach (Role role in objectType.PlayedRoleCollection)
						{
							FactType roleFactType = role.FactType;
							if (roleFactType.RoleCollection.Count == 2)
							{
								RoleMultiplicity roleMultiplicity = role.OppositeRole.Role.Multiplicity;
								if (roleMultiplicity == RoleMultiplicity.ExactlyOne ||
									roleMultiplicity == RoleMultiplicity.OneToMany)
								{
									mandatoryDirectFactTypes.Add(roleFactType);
								}
							}
						}
						// If there were any fact types in the previous list that was just created
						if (mandatoryDirectFactTypes.Count != 0)
						{
							// If any FactType in that list of mandatory direct facts is also a member
							// of the absorbedFactTypes Dictionary, add that AbsorbedFactType to a new
							// List of AbsorbedFactTypes
							Dictionary<FactType, AbsorbedFactType> currentAbsorbedFactTypes = new Dictionary<FactType, AbsorbedFactType>();
							foreach (FactType factType in mandatoryDirectFactTypes)
							{
								Guid factTypeId = factType.Id;
								if (myAbsorbedFactTypes.ContainsKey(factTypeId))
								{
									currentAbsorbedFactTypes.Add(factType, myAbsorbedFactTypes[factTypeId]);
								}
							}
							foreach (KeyValuePair<FactType, AbsorbedFactType> kvp in currentAbsorbedFactTypes)
							{
								// If the factAbsorptionType is fully and the current object type is not
								// absorbing itself, record the absorberGuid as the id of the ObjectType
								// recorded in the AbsorbedFactType object.
								AbsorbedFactType currentAbsorbedFactType = kvp.Value;
								Guid absorberId = currentAbsorbedFactType.AbsorberId;
								if (currentAbsorbedFactType.AbsorptionType == FactAbsorptionType.Fully &&
									absorberId != objectType.Id)
								{
									FactType factType = kvp.Key;
									Role firstRole = factType.RoleCollection[0].Role;
									Role secondRole = factType.RoleCollection[1].Role;
									if (secondRole.RolePlayer.Id == absorberId)
									{
										modality = secondRole.IsMandatory ? secondRole.MandatoryConstraintModality == ConstraintModality.Alethic ? MandatoryConstraintModality.Alethic : MandatoryConstraintModality.Deontic : MandatoryConstraintModality.NotMandatory;
									}
									else if (firstRole.RolePlayer.Id == absorberId)
									{
										modality = firstRole.IsMandatory ? firstRole.MandatoryConstraintModality == ConstraintModality.Alethic ? MandatoryConstraintModality.Alethic : MandatoryConstraintModality.Deontic : MandatoryConstraintModality.NotMandatory;
									}
									else
									{
										Debug.Fail("Role player didn't exist.");
									}
									absorberGuid = absorberId;
									break;
								}
							}
						}
					}
				}
				// Add the ObjectType to the Dictionary if a Guid exists for its absorber
				if (absorberGuid != Guid.Empty)
				{
					myAbsorbedObjectTypes.Add(objectType.Id, absorberGuid);
					myAbsorbedObjectTypeMandatories.Add(objectType.Id, modality);
				}
			}
		}
		/// <summary>
		/// Determines what <see cref="ObjectType"/>s in the model are actually top-level object types,
		/// or object types that stand alone as separate entities in an attribute-based model.
		/// </summary>
		/// <param name="modelObjectTypes">The <see cref="ORMModel"/>'s collection of <see cref="ObjectType"/>s</param>
		private void TopLevelObjectTypes(LinkedElementCollection<ObjectType> modelObjectTypes)
		{
			List<Guid> factTypeAbsorptionsAwayFromThisObjectType = new List<Guid>();
			if (myTopLevelTypes == null)
			{
				myTopLevelTypes = new List<Guid>();
			}
			else
			{
				myTopLevelTypes.Clear();
			}
			foreach (ObjectType objectType in modelObjectTypes)
			{
				Guid objectTypeId = objectType.Id;
				//ObjectTypeIsRelatedToObjectType.CreateObjectTypeIsRelatedToObjectType(PrimaryElementTrackerHasORMModel.
				// If an object is independent, then it is a top level type.
				if (objectType.IsIndependent)
				{
					myTopLevelTypes.Add(objectTypeId);
					continue;
				}
				List<FactType> functionalRoleFactTypes = new List<FactType>(GetFunctionalRoles(objectType.PlayedRoleCollection, null));

				// If the object type plays at least one functional role, is not independent, is not a subtype
				if (functionalRoleFactTypes.Count >= 1 && !IsSubtype(objectType))
				{
					// If the object type is not an absorbed object type
					if (!myAbsorbedObjectTypes.ContainsKey(objectTypeId))
					{
						factTypeAbsorptionsAwayFromThisObjectType.Clear();
						// Use a temporary list of Guids to store the id of FactTypes which are not absorbed into
						// the current object type
						foreach (KeyValuePair<Guid, AbsorbedFactType> absorbedFactType in myAbsorbedFactTypes)
						{
							if (absorbedFactType.Value.AbsorberId != objectTypeId)
							{
								factTypeAbsorptionsAwayFromThisObjectType.Add(absorbedFactType.Key);
							}
						}
						bool isAbsorbed = true;

						// If the id of any of those fact types in the factTypeAbsorptionsAwayFromThisObjectType List
						// match the id of any of the fact types in this object's functional direct fact types,
						// then it is not a top-level object type.
						foreach (FactType factType in functionalRoleFactTypes)
						{
							if (!factTypeAbsorptionsAwayFromThisObjectType.Contains(factType.Id))
							{
								isAbsorbed = false;
								break;
							}
						}
						if (!isAbsorbed)
						{
							myTopLevelTypes.Add(objectTypeId);
						}
					}
				}
			}
		}
		/// <summary>
		/// Finds the Value Types on the diagram and adds their information to the InformationTypeFormat Collection
		/// of this <see cref="OIALModel"/>.
		/// </summary>
		/// <param name="modelObjectTypes">The <see cref="ORMModel"/>'s collection of <see cref="ObjectType"/>s</param>
		private void InformationTypeFormats(Store store, LinkedElementCollection<ObjectType> modelObjectTypes)
		{
			int count = modelObjectTypes.Count;
			LinkedElementCollection<InformationTypeFormat> thisInformationTypeFormats = InformationTypeFormatCollection;
			for (int i = 0; i < count; ++i)
			{
				ObjectType currentObject = modelObjectTypes[i];
				if (currentObject.IsValueType)
				{
					// No data type can be held for an UnspecifiedDataType
					//if (!(currentObject.DataType is UnspecifiedDataType))
					//{
					InformationTypeFormat itf = new InformationTypeFormat(store,
						new PropertyAssignment(InformationTypeFormat.NameDomainPropertyId, currentObject.Name));
					itf.ValueType = currentObject;
					thisInformationTypeFormats.Add(itf);
					//}
				}
			}
#if USE_UNBINARIZED_UNARIES
			if (myUnariesExist)
			{
				InformationTypeFormat booleanInformationTypeFormat = new InformationTypeFormat(store,
					new PropertyAssignment(InformationTypeFormat.NameDomainPropertyId, "Boolean"));
				thisInformationTypeFormats.Add(booleanInformationTypeFormat);
			}
#endif
		}
		/// <summary>
		/// Finds the Concept Types in the model and adds them to its collection of <see cref="ConceptType"/> objects
		/// </summary>
		/// <param name="store">The current store of the <see cref="OIALDomainModel"/></param>
		/// <param name="objectType">The <see cref="ObjectType"/> which will be changed into a <see cref="ConceptType"/></param>
		/// <param name="parentConcept">The parent <see cref="ConceptType"/>, if any, which will absorb the <see cref="ConceptType"/>
		/// generated from this <see cref="ObjectType"/></param>
		private void ConceptTypes(Store store, ObjectType objectType, ConceptType parentConcept)
		{
			// TODO: Constraints need to added to ConceptTypeAbsorbedConceptType once OIAL constraints
			// have been further refined.
			// The name of the concept type is the name of the object type that represents that concept type.
			ConceptType conceptType = new ConceptType(store, new PropertyAssignment(ConceptType.NameDomainPropertyId, objectType.Name));
			conceptType.ObjectType = objectType;
			Guid objectGuid = objectType.Id;

			// We loop through the dictionary of absorbed object types to look for any more concept types that will be children
			// of the current concept type
			IElementDirectory elementDirectory = store.ElementDirectory;
			foreach (KeyValuePair<Guid, Guid> absorbedObjectTypeGuidPair in myAbsorbedObjectTypes)
			{
				if (absorbedObjectTypeGuidPair.Value.Equals(objectGuid))
				{
					ConceptTypes(store, elementDirectory.GetElement(absorbedObjectTypeGuidPair.Key) as ObjectType, conceptType);
				}
			}
			// Either add the concept type to the root (OIALModel) or to another ConceptType
			if (parentConcept == null)
			{
				ConceptTypeCollection.Add(conceptType);
				conceptType.Model = this;
			}
			else
			{
				parentConcept.AbsorbedConceptTypeCollection.Add(conceptType);
				ReadOnlyCollection<ConceptTypeAbsorbedConceptType> conceptTypeAbsorbedConceptTypeCollection = ConceptTypeAbsorbedConceptType.GetLinksToAbsorbedConceptTypeCollection(parentConcept);
				int count = conceptTypeAbsorbedConceptTypeCollection.Count;
				ConceptTypeAbsorbedConceptType conceptTypeAbsorbedConceptType = conceptTypeAbsorbedConceptTypeCollection[count - 1];
				conceptTypeAbsorbedConceptType.Name = objectType.Name;
				conceptTypeAbsorbedConceptType.Mandatory = myAbsorbedObjectTypeMandatories[conceptType.ObjectType.Id];
				LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
				ObjectType parentObject = parentConcept.ObjectType;
				foreach (Role role in playedRoles)
				{
					RoleBase oppositeRoleBase = role.OppositeRole;
					if (oppositeRoleBase != null)
					{
						if (parentObject == oppositeRoleBase.Role.RolePlayer)
						{
							conceptTypeAbsorbedConceptType.PathRoleCollection.Add(role);
							break;
						}
					}
				}
			}
		}
		/// <summary>
		/// Adds the low-level details, like <see cref="InformationType"/> and <see cref="ConceptTypeRef"/> elements,
		/// for each <see cref="ConceptType"/>.
		/// </summary>
		/// <param name="store">The current store of the <see cref="OIALDomainModel"/></param>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> for which information types
		/// and concept type refs should be generated.</param>
		private void InformationTypesAndConceptTypeRefs(Store store, ConceptType conceptType)
		{
			ObjectType conceptObjectType = conceptType.ObjectType;
			Guid objectId = conceptObjectType.Id;

			// If the concept type's object type is a value type, we must record an information type for that concept type.
			if (conceptObjectType.IsValueType)
			{
				// Create the Information Type that represents this value type.
				InformationTypeFormat informationTypeFormat = GetInformationTypeFormat(conceptObjectType);
				if (informationTypeFormat == null)
				{
					return;
				}
				//InformationType newInformationType = new InformationType(store, new PropertyAssignment(InformationType.NameDomainPropertyId, informationTypeFormat.Name + "Value"));
				//newInformationType.InformationTypeFormat = informationTypeFormat;
				conceptType.InformationTypeFormatCollection.Add(informationTypeFormat);

				// Create the Mandatory constraint and Single Child Constraint for this link.
				ReadOnlyCollection<InformationType> absorbedInformationTypes = InformationType.GetLinksToInformationTypeFormatCollection(conceptType);
				int index = absorbedInformationTypes.Count - 1;
				InformationType absorbedInfoType = absorbedInformationTypes[index];
				absorbedInfoType.Mandatory = MandatoryConstraintModality.Alethic;
				SingleChildUniquenessConstraint uConstraint = new SingleChildUniquenessConstraint(store,
					new PropertyAssignment(SingleChildUniquenessConstraint.IsPreferredDomainPropertyId, true),
					new PropertyAssignment(SingleChildUniquenessConstraint.ModalityDomainPropertyId, ConstraintModality.Alethic));
				absorbedInfoType.SingleChildConstraintCollection.Add(uConstraint);
				absorbedInfoType.Name = conceptObjectType.Name + "Value";
			}
			// Get the facts that this object type plays which are functionally determined by this object type
			// which are not absorbed away from this object type
			List<FactType> factList = new List<FactType>();
			LinkedElementCollection<Role> roleCollection = conceptObjectType.PlayedRoleCollection;
			int roleCollectionCount = roleCollection.Count;
			for (int j = 0; j < roleCollectionCount; ++j)
			{
				// Need to check for absorbed subtype meta facts and functional direct facts not absorbed away
				// from this object type: Lines 591 - 594 of ORMtoOIAL. However, exclude any fact types that
				// could result in nested concept types, as we have already dealt with these.
				Role role = roleCollection[j];
				if (role is SubtypeMetaRole && !conceptObjectType.IsIndependent)
				{
					continue;
				}
				FactType factType = role.FactType;
				// We have already accounted for nested concept types
				int roleCount = factType.RoleCollection.Count;
				if (roleCount > 2 || (factType.DerivationStorageDisplay == DerivationStorageType.Derived && !string.IsNullOrEmpty(factType.DerivationRuleDisplay)))
				{
					continue;
				}
#if USE_UNBINARIZED_UNARIES
				if (roleCount == 1)
				{
					LinkedElementCollection<InformationTypeFormat> informationTypeFormats = InformationTypeFormatCollection;
					int count = informationTypeFormats.Count;
					InformationTypeFormat booleanInformationTypeFormat = informationTypeFormats[count - 1];
					Debug.Assert(booleanInformationTypeFormat.Name == "Boolean");
					conceptType.InformationTypeFormatCollection.Add(booleanInformationTypeFormat);
					ReadOnlyCollection<InformationType> absorbedInformationTypes = InformationType.GetLinksToInformationTypeFormatCollection(conceptType);
					int index = absorbedInformationTypes.Count - 1;
					InformationType absorbedInfoType = absorbedInformationTypes[index];
					string roleName = role.Name;
					absorbedInfoType.Name = string.IsNullOrEmpty(roleName) ? GetUnaryReading(factType) : roleName;
					absorbedInfoType.Mandatory = MandatoryConstraintModality.Alethic;
				}
#else
				if (roleCount == 1)
				{
					continue;
				}
#endif
				else if (roleCount == 2)
				{
					LinkedElementCollection<ConstraintRoleSequence> constraints = role.ConstraintRoleSequenceCollection;
					int count = constraints.Count;
					// Check the constraints collection to make sure that this is a functional direct fact
					for (int k = 0; k < count; ++k)
					{
						UniquenessConstraint uniquenessConstraint = constraints[k] as UniquenessConstraint;
						if (uniquenessConstraint != null && uniquenessConstraint.IsInternal && uniquenessConstraint.RoleCollection.Count == 1 && uniquenessConstraint.Modality == ConstraintModality.Alethic)
						{
							AbsorbedFactType absorbedFactType;
							myAbsorbedFactTypes.TryGetValue(factType.Id, out absorbedFactType);
							Guid id = absorbedFactType.AbsorberId;
							// UNDONE: We need to check that the factList collection does not already contain the fact type. However,
							// this condition will only be true in very rare cases, i.e. when a concept type plays a role with itself
							// and we need to generate only one ConceptTypeRef instead of two. Is there a better way to do this without
							// the check
							int factListCount = factList.Count;
							ObjectType oppositeRolePlayer = role.OppositeRole.Role.RolePlayer;
							if ((id == Guid.Empty || id == objectId) && oppositeRolePlayer != null &&
								(factListCount == 0 || factList[factListCount - 1] != factType))
							{
								factList.Add(factType);
								break;
							}
						}
					}
				}
			}
			int factCount = factList.Count;
			for (int j = 0; j < factCount; ++j)
			{
				Role oppositeRole = null;
				Role thisRole = null;
				FactType currentFact = factList[j];
				LinkedElementCollection<RoleBase> factRoles = currentFact.RoleCollection;
				Role firstRole = factRoles[0].Role;
				Role secondRole = factRoles[1].Role;
				if (firstRole.RolePlayer == conceptObjectType)
				{
					oppositeRole = secondRole;
					thisRole = firstRole;
				}
				else
				{
					oppositeRole = firstRole;
					thisRole = secondRole;
				}
				ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
				Guid oppositeRolePlayerId = oppositeRolePlayer.Id;
				Guid oppositeRolePlayerDesiredParentOrTopLevelTypeId = GetTopLevelId(oppositeRolePlayerId, objectId);
				IEnumerable<SingleChildConstraint> constraints = GetSingleChildConstraints(oppositeRole, store);
				if (oppositeRolePlayerDesiredParentOrTopLevelTypeId.Equals(Guid.Empty))
				{
					// Account for role names (if they exist) instead of object type names.
					string baseName = oppositeRole.Name;
					bool useRoleName = true;
					if (string.IsNullOrEmpty(baseName))
					{
						useRoleName = false;
						baseName = oppositeRolePlayer.Name;
					}
					MandatoryConstraintModality constraintModality = MandatoryConstraintModality.NotMandatory;
					if (thisRole.IsMandatory)
					{
						switch (thisRole.MandatoryConstraintModality)
						{
							case ConstraintModality.Alethic:
								constraintModality = MandatoryConstraintModality.Alethic;
								break;
							case ConstraintModality.Deontic:
								constraintModality = MandatoryConstraintModality.Deontic;
								break;
						}
					}
					GetInformationTypes(store, conceptType, oppositeRole, baseName, constraintModality, constraints, useRoleName);
				}
				else if (oppositeRolePlayerDesiredParentOrTopLevelTypeId.Equals(objectId) && oppositeRolePlayer != conceptObjectType)
				{
					// CHANGE: Look at the immediate absorbed concept types of the current concept type. If the concept type is not present,
					// we do not do any processing, because we know that later on another concept type will account for the information types
					// of the concept type we are looking for.
					ConceptType absorbedConceptType = null;
					LinkedElementCollection<ConceptType> absorbedConceptTypes = conceptType.AbsorbedConceptTypeCollection;
					int absorbedConceptTypeCount = absorbedConceptTypes.Count;
					for (int k = 0; k < absorbedConceptTypeCount; ++k)
					{
						ConceptType current = absorbedConceptTypes[k];
						if (current.ObjectType == oppositeRolePlayer)
						{
							absorbedConceptType = current;
							break;
						}
					}
					Debug.Assert(absorbedConceptType != null, "AbsorbedConceptType cannot be null.");

					InformationTypesAndConceptTypeRefs(store, absorbedConceptType);
				}
				else if (myTopLevelTypes.Contains(oppositeRolePlayerId) || myAbsorbedObjectTypes.ContainsKey(oppositeRolePlayerId))
				{
					ConceptType referencedConceptType = GetConceptType(ConceptTypeCollection, oppositeRolePlayer, store);
					Debug.Assert(referencedConceptType != null, "Referenced concept type should not be null.");
					conceptType.ReferencedConceptTypeCollection.Add(referencedConceptType);
					ReadOnlyCollection<ConceptTypeRef> conceptTypeRefCollection = ConceptTypeRef.GetLinksToReferencingConceptType(referencedConceptType);
					Debug.Assert(conceptTypeRefCollection.Count != 0, "ConceptTypeRefCollection should not be empty.");
					ConceptTypeRef conceptTypeRef = conceptTypeRefCollection[conceptTypeRefCollection.Count - 1];
					conceptTypeRef.PathRoleCollection.Add(oppositeRole);
					string name = oppositeRole.Name;
					string oppositeName = thisRole.Name;
					if (string.IsNullOrEmpty(name))
					{
						name = oppositeRolePlayer.Name;
					}
					if (string.IsNullOrEmpty(oppositeName))
					{
						oppositeName = conceptObjectType.Name;
					}
					conceptTypeRef.Name = name;
					conceptTypeRef.OppositeName = oppositeName;

					if (thisRole.IsMandatory)
					{
						switch (thisRole.MandatoryConstraintModality)
						{
							case ConstraintModality.Alethic:
								conceptTypeRef.Mandatory = MandatoryConstraintModality.Alethic;
								break;
							case ConstraintModality.Deontic:
								conceptTypeRef.Mandatory = MandatoryConstraintModality.Deontic;
								break;
						}
					}
					conceptTypeRef.SingleChildConstraintCollection.AddRange(constraints);
				}
			}
		}
		/// <summary>
		/// Gets an <see cref="InformationType"/> for the particular <see cref="ConceptType"/> passed to it, based on the <see cref="Role"/>
		/// object passed to it whose role player is an <see cref="ObjectType"/> which will be absorbed into this <see cref="ConceptType"/>
		/// </summary>
		/// <param name="store">The store that this <see cref="OIALModel"/> uses to store its elements.</param>
		/// <param name="conceptType">The current <see cref="ConceptType"/> for which an <see cref="InformationType"/> will be added.</param>
		/// <param name="oppositeRole">The <see cref="Role"/> object whose opposite role's role player is the <see cref="ObjectType"/> of
		/// the <see cref="ConceptType"/> being passed.</param>
		/// <param name="baseName">The initial name for this <see cref="InformationType"/>. This will be the name of <see cref="Role"/> object
		/// passed to this method, or if that is an empty string, the name of that <see cref="Role"/>'s role player.</param>
		private void GetInformationTypes(Store store, ConceptType conceptType, Role oppositeRole, string baseName, MandatoryConstraintModality mandatory, IEnumerable<SingleChildConstraint> constraints, bool useRoleName)
		{
			GetInformationTypesInternal(store, conceptType, oppositeRole, baseName, true, new LinkedList<RoleBase>(), mandatory, constraints, useRoleName, !useRoleName);
			// Later on there will be more processing here to handle nullable equality constraints, etc.
		}
		/// <summary>
		/// Gets an <see cref="InformationType"/> for the particular <see cref="ConceptType"/> passed to it, based on the <see cref="Role"/>	
		/// object passed to it whose role player is an <see cref="ObjectType"/> which will be absorbed into this <see cref="ConceptType"/>
		/// </summary>
		/// <param name="store">The store that this <see cref="OIALModel"/> uses to store its elements.</param>
		/// <param name="conceptType">The current <see cref="ConceptType"/> for which an <see cref="InformationType"/> will be added.</param>
		/// <param name="oppositeRole">The <see cref="Role"/> object whose opposite role's role player is the <see cref="ObjectType"/> of
		/// the <see cref="ConceptType"/> being passed.</param>
		/// <param name="baseName">The initial name for this <see cref="InformationType"/>. This will be the name of <see cref="Role"/> object
		/// passed to this method, or if that is an empty string, the name of that <see cref="Role"/>'s role player. AS more calls to this
		/// method are made recursively, baseName will be added onto.</param>
		/// <param name="isFirst">If this is the first time through this method, true. Otherwise, false.</param>
		private void GetInformationTypesInternal(Store store, ConceptType conceptType, Role oppositeRole, string baseName, bool isFirst, LinkedList<RoleBase> pathNodes, MandatoryConstraintModality mandatory, IEnumerable<SingleChildConstraint> constraints, bool isPartOfCompositeIdentifier, bool forceRoleNames)
		{
			// We will most likely add on to the baseName parameter passed to this method.
			string newBaseName = baseName;
			ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
			Guid oppositeRolePlayerId = oppositeRolePlayer.Id;
			// We have found the value type which corresponds to this information type, so we will create the information type
			// and add our Roles (Path Roles) to the ElementLink created (ConceptTypeHasInformationType).
			if (oppositeRolePlayer.IsValueType)
			{
				// If it is first, we don't do anything to change the name. Also, if it isn't part of a preferred identifier,
				// then we are not interested in changing the name either.
				if (!isFirst && isPartOfCompositeIdentifier)
				{
					string concatName = oppositeRole.Name;

					// If the opposite role has a name, then we use it as the name of the information type.
					// Otherwise, we concatenate the previous name with the role player name.
					if (!string.IsNullOrEmpty(concatName))
					{
						if (!string.IsNullOrEmpty(concatName.TrimEnd(' ')))
						{
							newBaseName = forceRoleNames ? concatName : string.Concat(newBaseName, "_", concatName);
						}
					}
					else
					{
						newBaseName = string.Concat(newBaseName, "_", oppositeRolePlayer.Name);
					}
				}
				InformationTypeFormat informationTypeFormat = GetInformationTypeFormat(oppositeRolePlayer);
				conceptType.InformationTypeFormatCollection.Add(informationTypeFormat);

				// Get the link we just created
				ReadOnlyCollection<InformationType> conceptTypeInformationTypeFormatCollection = InformationType.GetLinksToInformationTypeFormatCollection(conceptType);
				int count = conceptTypeInformationTypeFormatCollection.Count;
				InformationType informationType = conceptTypeInformationTypeFormatCollection[count - 1];

				informationType.PathRoleCollection.AddRange(pathNodes);
				informationType.PathRoleCollection.Add(oppositeRole);
				informationType.Mandatory = mandatory;
				informationType.Name = newBaseName.TrimEnd(' ');
				informationType.SingleChildConstraintCollection.AddRange(constraints);
			}
			else if (GetTopLevelId(oppositeRolePlayerId, conceptType.ObjectType.Id) != Guid.Empty)
			{
				if (!isFirst && isPartOfCompositeIdentifier)
				{
					string concatName = oppositeRole.Name;

					// If the opposite role has a name, then we use it as the name of the information type.
					// Otherwise, we concatenate the previous name with the role player name.
					if (!string.IsNullOrEmpty(concatName))
					{
						if (!string.IsNullOrEmpty(concatName.TrimEnd(' ')))
						{
							newBaseName = forceRoleNames ? concatName : string.Concat(newBaseName, "_", concatName);
						}
					}
					else
					{
						newBaseName = string.Concat(newBaseName, "_", oppositeRolePlayer.Name);
					}
				}
				ConceptType referencedConceptType = GetConceptType(ConceptTypeCollection, oppositeRolePlayer, store);
				conceptType.ReferencedConceptTypeCollection.Add(referencedConceptType);
				ReadOnlyCollection<ConceptTypeRef> conceptTypeRefs = ConceptTypeRef.GetLinksToReferencedConceptTypeCollection(conceptType);
				int count = conceptTypeRefs.Count;
				ConceptTypeRef conceptTypeRef = conceptTypeRefs[count - 1] as ConceptTypeRef;
				conceptTypeRef.PathRoleCollection.AddRange(pathNodes);
				conceptTypeRef.PathRoleCollection.Add(oppositeRole);

				string[] nameComponents = newBaseName.Split('_');
				Array.Reverse(nameComponents);
				string oppositeName = string.Join("_", nameComponents);
				conceptTypeRef.OppositeName = oppositeName;
				conceptTypeRef.Mandatory = mandatory;
				conceptTypeRef.SingleChildConstraintCollection.AddRange(constraints);
			}
			else
			{
				pathNodes.AddLast(oppositeRole);
				UniquenessConstraint preferredIdentifier = oppositeRolePlayer.PreferredIdentifier;
				if (preferredIdentifier != null)
				{
					LinkedElementCollection<Role> preferredIdentifierRoles = preferredIdentifier.RoleCollection;
					int count = preferredIdentifierRoles.Count;
					bool hasCompositeIdentifier = count > 1;
					for (int i = 0; i < count; ++i)
					{
						Role playedRole = preferredIdentifierRoles[i];
						//If it is part of a composite identifier, then we try to use the role name. If there is no role name,
						// then we concatenate the previous name with the current object type name.
						if (!isFirst && isPartOfCompositeIdentifier)
						{
							string concatName = oppositeRole.Name;
							if (!string.IsNullOrEmpty(concatName))
							{
								if (!string.IsNullOrEmpty(concatName.TrimEnd(' ')))
								{
									newBaseName = forceRoleNames ? concatName : string.Concat(newBaseName, "_", concatName);
									forceRoleNames = true;
								}
							}
							else
							{
								newBaseName = string.Concat(newBaseName, "_", oppositeRolePlayer.Name);
							}
						}
						GetInformationTypesInternal(store, conceptType, playedRole, newBaseName, false, pathNodes, mandatory, constraints, hasCompositeIdentifier, forceRoleNames);
					}
				}
				//LinkedElementCollection<Role> playedRoles = oppositeRolePlayer.PlayedRoleCollection;
				//int count = playedRoles.Count;
				//for (int i = 0; i < count; ++i)
				//{
				//    Role playedRole = playedRoles[i];
				//    if (playedRole.FactType.RoleCollection.Count != 2)
				//    {
				//        continue;
				//    }
				//    Role oppRole = playedRole.OppositeRole.Role;
				//    LinkedElementCollection<ConstraintRoleSequence> roleSequenceConstraints = oppRole.ConstraintRoleSequenceCollection;
				//    int constraintsCount = roleSequenceConstraints.Count;
				//    for (int j = 0; j < constraintsCount; ++j)
				//    {
				//        ConstraintRoleSequence constraintRoleSequence = roleSequenceConstraints[j];
				//        UniquenessConstraint uConstraint = constraintRoleSequence as UniquenessConstraint;

				//        // We cannot be sure that constraints that we receive span only one role,
				//        // so we must check for this.
				//        bool hasCompositeIdentifier = constraintRoleSequence.RoleCollection.Count != 1;
				//        if (uConstraint != null && uConstraint.Modality == ConstraintModality.Alethic && uConstraint.IsPreferred && ((!hasCompositeIdentifier && uConstraint.IsInternal) || !uConstraint.IsInternal) )
				//        {
				//            // If it is part of a composite identifier, then we try to use the role name. If there is no role name,
				//            // then we concatenate the previous name with the current object type name.
				//            if (!isFirst && isPartOfCompositeIdentifier)
				//            {
				//                string concatName = oppositeRole.Name;
				//                if (!string.IsNullOrEmpty(concatName))
				//                {
				//                    if (!string.IsNullOrEmpty(concatName.TrimEnd(' ')))
				//                    {
				//                        newBaseName = forceRoleNames ? concatName : string.Concat(newBaseName, "_", concatName);
				//                        forceRoleNames = true;
				//                    }
				//                }
				//                else
				//                {
				//                    newBaseName = string.Concat(newBaseName, "_", oppositeRolePlayer.Name);
				//                }
				//            }
				//GetInformationTypesInternal(store, conceptType, oppRole, newBaseName, false, pathNodes, mandatory, constraints, hasCompositeIdentifier, forceRoleNames);
				//        }
				//    }
				//}
				pathNodes.RemoveLast();
			}
		}
		/// <summary>
		/// Gets the internal child constraints based off the opposite role in a <see cref="ConceptTypeChild"/> relationship,
		/// the opposite role being the role on the same side as the child.
		/// </summary>
		/// <param name="role">The opposite role in the <see cref="ConceptTypeChild"/> relationship.</param>
		/// <param name="store">The store the current <see cref="OIALModel"/> is attached to.</param>
		/// <returns>An <see cref="IEnumerable&lt;SingleChildConstraint&gt;"/> to add to the <see cref="ConceptTypeChild"/>.</returns>
		private IEnumerable<SingleChildConstraint> GetSingleChildConstraints(Role role, Store store)
		{
			LinkedElementCollection<ConstraintRoleSequence> constraintCollection = role.ConstraintRoleSequenceCollection;
			int constraintCount = constraintCollection.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				ConstraintRoleSequence constraintRoleSequence = constraintCollection[i];

				UniquenessConstraint uConstraint = constraintRoleSequence as UniquenessConstraint;
				if (uConstraint != null && uConstraint.RoleCollection.Count == 1 && uConstraint.IsInternal)
				{
					//Debug.Assert(uConstraint.IsInternal, "Uniqueness Constraint with role collection count of 1 must be internal.");
					yield return new SingleChildUniquenessConstraint(store,
						new PropertyAssignment(SingleChildUniquenessConstraint.IsPreferredDomainPropertyId, uConstraint.IsPreferred),
						new PropertyAssignment(SingleChildUniquenessConstraint.ModalityDomainPropertyId, uConstraint.Modality));
					continue;
				}

				FrequencyConstraint fConstraint = constraintRoleSequence as FrequencyConstraint;
				if (fConstraint != null && fConstraint.RoleCollection.Count == 1)
				{
					yield return new SingleChildFrequencyConstraint(store,
						new PropertyAssignment(SingleChildFrequencyConstraint.NameDomainPropertyId, fConstraint.Name),
						new PropertyAssignment(SingleChildFrequencyConstraint.ModalityDomainPropertyId, fConstraint.Modality));
					continue;
				}
			}

			//RoleValueConstraint vConstraint = role.ValueConstraint;
			//if (vConstraint != null)
			//{
			//    yield return new ValueConstraint(store,
			//        new PropertyAssignment(ValueConstraint.NameDomainPropertyId, vConstraint.Name));
			//}

			//ObjectType rolePlayer = role.RolePlayer;
			//ObjModel.ValueConstraint valueConstraint = rolePlayer.ValueConstraint;
			//if (valueConstraint != null)
			//{
			//    yield return new ValueConstraint(store,
			//        new PropertyAssignment(ValueConstraint.NameDomainPropertyId, valueConstraint.Name));
			//}
		}
		/// <summary>
		/// Gets the external constraints associated with the attached <see cref="ORMModel"/>.
		/// </summary>
		/// <param name="store">The store the <paramref name="model"/> (ORMModel) is attached to.</param>
		/// <param name="model">The <see cref="ORMModel"/> currently attached to this <see cref="OIALModel"/>.</param>
		private void GenerateExternalConstraints(Store store, ORMModel model)
		{
			foreach (SetConstraint constraint in model.SetConstraintCollection)
			{
				if (constraint.RoleCollection.Count == 1)
				{
					continue;
				}
				string constraintName = constraint.Name;
				ConstraintModality constraintModality = constraint.Modality;
				bool isDisjunctiveMandatoryConstraint = constraint is MandatoryConstraint;
				LinkedElementCollection<Role> roleCollection = constraint.RoleCollection;
				List<ConceptTypeChild> conceptTypeHasChildCollection = GetConceptTypeChildRelationshipsForSetConstraints(roleCollection, isDisjunctiveMandatoryConstraint);
				if (conceptTypeHasChildCollection == null)
				{
					continue;
				}
				int collectionCount = roleCollection.Count;
				if (conceptTypeHasChildCollection.Count != 0)
				{
					MinTwoChildrenChildSequence minTwoChildrenChildSequence = new MinTwoChildrenChildSequence(store);
					minTwoChildrenChildSequence.ConceptTypeChildCollection.AddRange(conceptTypeHasChildCollection);
					SingleChildSequenceConstraint childSequenceConstraint = null;

					UniquenessConstraint uConstraint = constraint as UniquenessConstraint;
					if (uConstraint != null)
					{
						bool isPreferred = uConstraint.IsPreferred;
						bool ignoreIfPrimary = !uConstraint.IsInternal;
						bool shouldBeUnique = false;
						if (ignoreIfPrimary)
						{
							for (int i = 0; i < conceptTypeHasChildCollection.Count; ++i)
							{
								ConceptTypeChild conceptTypeChild = conceptTypeHasChildCollection[i];
								LinkedElementCollection<RoleBase> pathRoles = conceptTypeChild.PathRoleCollection;
								int pathRoleCount = pathRoles.Count;
								if (pathRoleCount > 1)
								{
									Role secondPathRole = pathRoles[pathRoleCount - 2].Role;
									LinkedElementCollection<ConstraintRoleSequence> constraints = secondPathRole.ConstraintRoleSequenceCollection;
									for (int j = 0; j < constraints.Count; ++j)
									{
										UniquenessConstraint uniquenessConstraint = constraints[j].Constraint as UniquenessConstraint;
										if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred && uConstraint != uniquenessConstraint)
										{
											int uConstraintRoleCount = uniquenessConstraint.RoleCollection.Count;
											if (uConstraintRoleCount > 1)
											{
												ignoreIfPrimary = true;
												shouldBeUnique = false;
												break;
											}
											else
											{
												ignoreIfPrimary = false;
												shouldBeUnique = false;
												break;
											}
										}
										else
										{
											ignoreIfPrimary = false;
											shouldBeUnique = true;
										}
									}
								}
								//else if (conceptTypeChild.Parent.AbsorbingConceptType != null)
								//{
								//    shouldBeUnique = true;
								//}
							}
						}
						//if (isPreferred && ignoreIfPrimary && !shouldBeUnique)
						//{
						//    for (int i = 0; i < collectionCount; ++i)
						//    {
						//        RoleBase oppositeRole = roleCollection[i].OppositeRole;
						//        if (oppositeRole != null)
						//        {
						//            if (myTopLevelTypes.Contains(oppositeRole.Role.RolePlayer.Id))
						//            {
						//                ignoreIfPrimary = false;
						//                break;
						//            }
						//        }
						//    }
						//}
						if (shouldBeUnique && isPreferred)
						{
							isPreferred = false;
						}
						childSequenceConstraint = new ChildSequenceUniquenessConstraint(store,
							new PropertyAssignment(ChildSequenceUniquenessConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(ChildSequenceUniquenessConstraint.IsPreferredDomainPropertyId, isPreferred),
							new PropertyAssignment(ChildSequenceUniquenessConstraint.ModalityDomainPropertyId, constraintModality),
							new PropertyAssignment(ChildSequenceUniquenessConstraint.ShouldIgnoreDomainPropertyId, ignoreIfPrimary));
					}
					else if (isDisjunctiveMandatoryConstraint)
					{
						childSequenceConstraint = new DisjunctiveMandatoryConstraint(store,
							new PropertyAssignment(DisjunctiveMandatoryConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(DisjunctiveMandatoryConstraint.ModalityDomainPropertyId, constraintModality));
					}
					else if (constraint is FrequencyConstraint)
					{
						childSequenceConstraint = new ChildSequenceFrequencyConstraint(store,
							new PropertyAssignment(ChildSequenceFrequencyConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(ChildSequenceFrequencyConstraint.ModalityDomainPropertyId, constraintModality));
					}
					else if (constraint is ObjModel.RingConstraint)
					{
						childSequenceConstraint = new RingConstraint(store,
							new PropertyAssignment(RingConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(RingConstraint.ModalityDomainPropertyId, constraintModality));
					}
					Debug.Assert(childSequenceConstraint != null);
					childSequenceConstraint.ChildSequence = minTwoChildrenChildSequence;
					childSequenceConstraint.OIALModel = this;
				}
			}

			//foreach (SetComparisonConstraint setComparisonConstraint in model.SetComparisonConstraintCollection)
			//{
			//    string constraintName = setComparisonConstraint.Name;
			//    ConstraintModality constraintModality = setComparisonConstraint.Modality;
			//    LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = setComparisonConstraint.RoleSequenceCollection;
			//    int roleSequenceCount = roleSequences.Count;

			//    if (roleSequenceCount > 1)
			//    {
			//        if (setComparisonConstraint is ObjModel.SubsetConstraint)
			//        {
			//            SubsetConstraint subsetConstraint = new SubsetConstraint(store,
			//                new PropertyAssignment(SubsetConstraint.NameDomainPropertyId, constraintName),
			//                new PropertyAssignment(SubsetConstraint.ModalityDomainPropertyId, constraintModality));
			//            List<ConceptTypeChild> list = GetConceptTypeChildRelationshipsForSetComparisonConstraints(roleSequences[0], true);
			//            List<ConceptTypeChild> list2 = GetConceptTypeChildRelationshipsForSetComparisonConstraints(roleSequences[1], true);
			//            if (list == null || list2 == null)
			//            {
			//                continue;
			//            }
			//            ChildSequence subChildSequence = new ChildSequence(store);
			//            subChildSequence.ConceptTypeChildCollection.AddRange(list);

			//            ChildSequence superChildSequence = new ChildSequence(store);
			//            superChildSequence.ConceptTypeChildCollection.AddRange(list2);

			//            subsetConstraint.SubChildSequence = subChildSequence;
			//            subsetConstraint.SuperChildSequence = superChildSequence;
			//            subsetConstraint.OIALModel = this;
			//        }
			//        else if (setComparisonConstraint is ObjModel.EqualityConstraint)
			//        {
			//            EqualityConstraint equalityConstraint = new EqualityConstraint(store,
			//                new PropertyAssignment(EqualityConstraint.NameDomainPropertyId, constraintName),
			//                new PropertyAssignment(EqualityConstraint.ModalityDomainPropertyId, constraintModality));

			//            for (int i = 0; i < roleSequenceCount; ++i)
			//            {
			//                List<ConceptTypeChild> list = GetConceptTypeChildRelationshipsForSetComparisonConstraints(roleSequences[i], true);
			//                if (list == null)
			//                {
			//                    continue;
			//                }
			//                ChildSequence childSequence = new ChildSequence(store);
			//                childSequence.ConceptTypeChildCollection.AddRange(list);
			//                equalityConstraint.ChildSequence.Add(childSequence);
			//            }
			//            equalityConstraint.OIALModel = this;
			//        }
			//        else if (setComparisonConstraint is ObjModel.ExclusionConstraint)
			//        {
			//            ExclusionConstraint exclusionConstraint = new ExclusionConstraint(store,
			//                new PropertyAssignment(ExclusionConstraint.NameDomainPropertyId, constraintName),
			//                new PropertyAssignment(ExclusionConstraint.ModalityDomainPropertyId, constraintModality));

			//            for (int i = 0; i < roleSequenceCount; ++i)
			//            {
			//                List<ConceptTypeChild> list = GetConceptTypeChildRelationshipsForSetComparisonConstraints(roleSequences[i], true);
			//                if (list == null)
			//                {
			//                    continue;
			//                }
			//                ChildSequence childSequence = new ChildSequence(store);
			//                childSequence.ConceptTypeChildCollection.AddRange(list);
			//                exclusionConstraint.ChildSequence.Add(childSequence);
			//            }
			//            exclusionConstraint.OIALModel = this;
			//        }
			//    }
			//}
		}
		#endregion // ORMToOIAL Algorithms
		#region Helper Methods
#if USE_UNBINARIZED_UNARIES
		/// <summary>
		/// Gets the reading for a unary fact type, which can interpreted to be its name at a logical level (e.g. Relational)
		/// </summary>
		/// <param name="unaryFactType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.FactType"/> whose reading order is of interest.</param>
		/// <returns>Column name</returns>
		private static string GetUnaryReading(FactType unaryFactType)
		{
			if (unaryFactType.RoleCollection.Count != 1)
			{
				// UNDONE: Localize this.
				throw new ArgumentException("Passed fact type does not have one role.");
			}
			LinkedElementCollection<ReadingOrder> readingOrderCollection = unaryFactType.ReadingOrderCollection;
			int readingCount = readingOrderCollection.Count;
			if (readingCount > 0)
			{
				ReadingOrder unaryReadingOrder = readingOrderCollection[0];
				string reading = unaryReadingOrder.ReadingText;
				reading = reading.Remove(reading.IndexOf("{0}"), 3);
				string[] splitReading = reading.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				int splitReadingCount = splitReading.Length;
				for (int i = 0; i < splitReadingCount; ++i)
				{
					string currentReadingWord = splitReading[i];
					char firstChar;
					if (i == 0)
					{
						firstChar = char.ToLower(currentReadingWord[0], CultureInfo.InvariantCulture);
					}
					else
					{
						firstChar = char.ToUpper(currentReadingWord[0], CultureInfo.InvariantCulture);

					}
					splitReading[i] = string.Concat(firstChar, currentReadingWord.Remove(0, 1));
				}
				return string.Concat(splitReading);
			}
			return "thisUnaryFactTypeNeedsAName";
		}
#endif
		private static int GetAlethicInternalConstraintsCount(FactType factType, ConstraintType constraintType)
		{
			LinkedElementCollection<SetConstraint> factSetConstraints = factType.SetConstraintCollection;
			int factSetConstraintCount = factSetConstraints.Count;
			int retVal = 0;

			for (int i = 0; i < factSetConstraintCount; ++i)
			{
				IConstraint constraint = (IConstraint)factSetConstraints[i];
				if (constraint.ConstraintType == constraintType && constraint.Modality == ConstraintModality.Alethic && constraint.ConstraintIsInternal)
				{
					++retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Gets a collection which can iterated over of <see cref="FactType"/> objects for
		/// a specific <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectTypeRoleCollection">The PlayedRoleCollection of the <see cref="ObjectType"/> of interest</param>
		/// <param name="startingRole">If the <see cref="ObjectType"/> of interest has a specific <see cref="Role"/> whose
		/// <see cref="FactType"/> should not be checked, pass it here. Otherwise, pass null.</param>
		/// <returns>IEnumerable of <see cref="FactType"/> objects</returns>
		private static IEnumerable<FactType> GetFunctionalNonDependentRoles(LinkedElementCollection<Role> objectTypeRoleCollection, Role startingRole)
		{
			foreach (Role role in objectTypeRoleCollection)
			{
				// If null is passed for the starting role, then although this check will be executed, it will
				// never reach the continue statement. Also we do not want to interpret any fact types that are
				// not binarized.
				FactType roleFactType = role.FactType;
				if (role.Equals(startingRole) || roleFactType.Objectification != null || roleFactType.RoleCollection.Count != 2 || (roleFactType.DerivationStorageDisplay == DerivationStorageType.Derived && !string.IsNullOrEmpty(roleFactType.DerivationRuleDisplay)))
				{
					continue;
				}
				// If it is a functional role
				Role oppositeRole = role.OppositeRole.Role;					// CHANGE: Role to RoleBase
				RoleMultiplicity roleMultiplicity = oppositeRole.Multiplicity;

				if (roleMultiplicity == RoleMultiplicity.ZeroToOne ||
					roleMultiplicity == RoleMultiplicity.ExactlyOne)
				{
					// If there are no constraints in the role-sequence collection, then it is a
					// functional role. If there are constraints but no uniqueness constraints, it
					// is a functional role. If there are constraints and one is an internal uniqueness
					// constraint and it is not primary, then it is a functional role. If there are
					// constraints and one is an external uniqueness constraint and it is not primary,
					// then it is a functional role. If there are constraints and one is an internal
					// uniqueness constraint and another is an external uniqueness constraint and either
					// one is primary, then it is not a functional role.
					bool isDependentOrPreferredIdentifier = false;
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						IConstraint constraint = constraintRoleSequence.Constraint;
						ConstraintType constraintType = constraint.ConstraintType;
						if (constraintType == ConstraintType.InternalUniqueness ||
							constraintType == ConstraintType.ExternalUniqueness)
						{
							isDependentOrPreferredIdentifier = true;
							break;
						}
					}
					if (!isDependentOrPreferredIdentifier)
					{
						yield return roleFactType;
					}
				}
			}
		}
		/// <summary>
		/// Determines how many functional roles an <see cref="ObjectType"/> participates in
		/// other than the current <see cref="Role"/> passed to the function.
		/// </summary>
		/// <remarks>
		/// Intended to be a helper method for FactTypeAbsorption algorithm
		/// </remarks>
		/// <param name="objectTypeRoleCollection">
		/// Roles that the object type plays.
		/// </param>
		/// <param name="startingRole">
		/// The <see cref="Role"/> object whose Role Player is of interest. Pass null if no
		/// role needs to be checked.
		/// </param>
		/// <returns>
		/// An integer with the number of functional roles not part of the primary identifier
		/// </returns>
		private static int GetFunctionalNonDependentRoleCount(LinkedElementCollection<Role> objectTypeRoleCollection, Role startingRole)
		{
			int retVal = 0;
			IEnumerator<FactType> iEnumerator = GetFunctionalNonDependentRoles(objectTypeRoleCollection, startingRole).GetEnumerator();
			// Iterates over the enumerator to count the collection.
			while (iEnumerator.MoveNext())
			{
				++retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Gets a collection which can iterated over of <see cref="FactType"/> objects for
		/// a specific <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectTypeRoleCollection">The PlayedRoleCollection of the <see cref="ObjectType"/> of interest</param>
		/// <param name="startingRole">If the <see cref="ObjectType"/> of interest has a specific <see cref="Role"/> whose
		/// <see cref="FactType"/> should not be checked, pass it here. Otherwise, pass null.</param>
		/// <returns>IEnumerable of <see cref="FactType"/> objects</returns>
		private static IEnumerable<FactType> GetFunctionalRoles(LinkedElementCollection<Role> objectTypeRoleCollection, Role startingRole)
		{
			foreach (Role role in objectTypeRoleCollection)
			{
				// If null is passed for the starting role, then although this check will be executed, it will
				// never reach the continue statement. Also we do not want to interpret any fact types that are
				// not binarized.
				FactType roleFactType = role.FactType;
				if (role.Equals(startingRole) || roleFactType.Objectification != null || roleFactType.RoleCollection.Count != 2 || (roleFactType.DerivationStorageDisplay == DerivationStorageType.Derived && !string.IsNullOrEmpty(roleFactType.DerivationRuleDisplay)))
				{
					continue;
				}
				// If it is a functional role
				Role oppositeRole = role.OppositeRole.Role;					// CHANGE: Role to RoleBase
				RoleMultiplicity roleMultiplicity = oppositeRole.Multiplicity;

				if (roleMultiplicity == RoleMultiplicity.ZeroToOne ||
					roleMultiplicity == RoleMultiplicity.ExactlyOne)
				{
					// If there are no constraints in the role-sequence collection, then it is a
					// functional role. If there are constraints but no uniqueness constraints, it
					// is a functional role. If there are constraints and one is an internal uniqueness
					// constraint and it is not primary, then it is a functional role. If there are
					// constraints and one is an external uniqueness constraint and it is not primary,
					// then it is a functional role. If there are constraints and one is an internal
					// uniqueness constraint and another is an external uniqueness constraint and either
					// one is primary, then it is not a functional role.
					bool isPreferredIdentifier = false;
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						IConstraint constraint = constraintRoleSequence.Constraint;
						ConstraintType constraintType = constraint.ConstraintType;
						if (constraintType == ConstraintType.InternalUniqueness ||
							constraintType == ConstraintType.ExternalUniqueness)
						{
							if (constraint.PreferredIdentifierFor != null)
							{
								isPreferredIdentifier = true;
							}
						}
					}
					if (!isPreferredIdentifier)
					{
						yield return roleFactType;
					}
				}
			}
		}
		/// <summary>
		/// Determines how many functional roles an <see cref="ObjectType"/> participates in
		/// other than the current <see cref="Role"/> passed to the function.
		/// </summary>
		/// <remarks>
		/// Intended to be a helper method for FactTypeAbsorption algorithm
		/// </remarks>
		/// <param name="objectTypeRoleCollection">
		/// Roles that the object type plays.
		/// </param>
		/// <param name="startingRole">
		/// The <see cref="Role"/> object whose Role Player is of interest. Pass null if no
		/// role needs to be checked.
		/// </param>
		/// <returns>
		/// An integer with the number of functional roles not part of the primary identifier
		/// </returns>
		private static int GetFunctionalRoleCount(LinkedElementCollection<Role> objectTypeRoleCollection, Role startingRole)
		{
			int retVal = 0;
			IEnumerator<FactType> iEnumerator = GetFunctionalRoles(objectTypeRoleCollection, startingRole).GetEnumerator();
			// Iterates over the enumerator to count the collection.
			while (iEnumerator.MoveNext())
			{
				++retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Determines if an <see cref="ObjectType"/> is a Subtype of another <see cref="ObjectType"/>
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> whose Subtype-status is of interest</param>
		/// <returns>True if the passed <see cref="ObjectType"/> is a Subtype. Otherwise, false.</returns>
		private static bool IsSubtype(ObjectType objectType)
		{
			LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role role = playedRoles[i];
				if (role is SubtypeMetaRole)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Gets the first primary super type for an <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> whose supertype we are interested in.</param>
		/// <returns>An <see cref="ObjectType"/> of the first primary supertype if it exist. If not, null.</returns>
		private static ObjectType GetSupertype(ObjectType objectType)
		{
			ObjectType retVal = null;
			ObjectType.WalkSupertypes(
				objectType,
				delegate(ObjectType supertype, int depth, bool isPrimary)
				{
					if (isPrimary)
					{
						retVal = supertype;
						return ObjectTypeVisitorResult.Stop;
					}
					return ObjectTypeVisitorResult.SkipChildren;
				});
			return retVal;
		}
		/// <summary>
		/// Gets the ID of the top-level concept type which absorbs the element with the ID of <paramref name="targetId"/>.
		/// </summary>
		/// <param name="targetId">The ID of the <see cref="ObjectType"/> whose absorber is of interest.</param>
		/// <param name="desiredParentId">The ID of the <see cref="ObjectType"/> which is the desired parent.</param>
		/// <returns>The <see cref="Guid"/> of the <see cref="ObjectType"/>, if any, which absorbs this <see cref="ObjectType"/></returns>
		private Guid GetTopLevelId(Guid targetId, Guid desiredParentId)
		{
			Guid towardsId;
			myAbsorbedObjectTypes.TryGetValue(targetId, out towardsId);
			if (targetId.Equals(Guid.Empty))
			{
				return Guid.Empty;
			}
			else if (myTopLevelTypes.Contains(targetId))
			{
				return targetId;
			}
			else if (!towardsId.Equals(Guid.Empty) && towardsId.Equals(desiredParentId))
			{
				return desiredParentId;
			}
			else
			{
				return GetTopLevelId(towardsId, desiredParentId);
			}
		}
		/// <summary>
		/// Gets the <see cref="ConceptType"/> which will be referenced for a <see cref="ConceptTypeRef"/> relationship.
		/// </summary>
		/// <param name="conceptTypes">The <see cref="ConceptTypeMoveableCollection"/> of <see cref="ConceptType"/> objects
		/// to look through.</param>
		/// <param name="objectType">The <see cref="T:ObjectType"/> of <see cref="ConceptType"/> which will be referenced.</param>
		/// <param name="store">The store this current <see cref="OIALModel"/> is using.</param>
		/// <returns>The <see cref="ConceptType"/> which is referenced.</returns>
		private ConceptType GetConceptType(LinkedElementCollection<ConceptType> conceptTypes, ObjectType objectType, Store store)
		{
			int count = conceptTypes.Count;
			for (int i = 0; i < count; ++i)
			{
				ConceptType currentConceptType = conceptTypes[i];
				if (currentConceptType.ObjectType == objectType)
				{
					return currentConceptType;
				}
				LinkedElementCollection<ConceptType> absorbedConceptTypes = currentConceptType.AbsorbedConceptTypeCollection;
				int absorbedConceptTypeCount = absorbedConceptTypes.Count;
				if (absorbedConceptTypeCount != 0)
				{
					ConceptType conceptType = GetConceptType(absorbedConceptTypes, objectType, store);
					if (conceptType != null)
					{
						return conceptType;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Gets the <see cref="InformationTypeFormat"/> which will be referenced for an <see cref="InformationType"/>.
		/// </summary>
		/// <param name="valueType">The <see cref="ObjectType"/> (a Value Type) which corresponds to the <see cref="InformationTypeFormat"/>
		/// we are looking for.</param>
		/// <returns>The <see cref="InformationTypeFormat"/> that corresponds to the <see cref="ObjectType"/> passed to this method.</returns>
		private InformationTypeFormat GetInformationTypeFormat(ObjectType valueType)
		{
			LinkedElementCollection<InformationTypeFormat> thisInformationTypeFormats = InformationTypeFormatCollection;
			int informationTypeCount = thisInformationTypeFormats.Count;
			for (int i = 0; i < informationTypeCount; ++i)
			{
				InformationTypeFormat currentInformationTypeFormat = thisInformationTypeFormats[i];
				if (valueType == currentInformationTypeFormat.ValueType)
				{
					return currentInformationTypeFormat;
				}
			}
			return null;
		}
		/// <summary>
		/// Gets a <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/> that contains all the
		/// <see cref="Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> relationships that have <see cref="Neumont.Tools.ORM.ObjectModel.Role"/>
		/// objects in their path role collection that are in the passed role collection.
		/// </summary>
		/// <param name="constraintRoleCollection">The <see cref="Microsoft.VisualStudio.Modeling.LinkedElementCollection&lt;Role&gt;"/> to generate a
		/// <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/> for.</param>
		/// <param name="checkOppositeRole">True if the opposite roles of <paramref name="constraint"/>'s role collection should be checked
		/// instead of the normal roles. Otherwise, false. An example would be a <example>DisjunctiveMandatoryConstraint</example>.</param>
		/// <returns>A <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/>.</returns>
		private static List<ConceptTypeChild> GetConceptTypeChildRelationshipsForSetConstraints(LinkedElementCollection<Role> constraintRoleCollection, bool checkOppositeRole)
		{
			List<ConceptTypeChild> conceptTypeHasChildCollection = new List<ConceptTypeChild>();
			// Checking the role collection count is not equal to zero ensures that we have only external constraints (or constraint that
			// will map to external constraint in the co-referenced version of the model). We have checked for this in the calling method.
			foreach (Role role in constraintRoleCollection)
			{
				Role newRole = role;
				if (checkOppositeRole)
				{
					RoleBase oppositeRoleBase = role.OppositeRole;
					if (oppositeRoleBase == null)
					{
						return null;
					}
					//Debug.Assert(oppositeRoleBase != null, "Opposite Role Base was null when generating an Child Sequence Constraint.");
					newRole = oppositeRoleBase.Role;
				}
				ReadOnlyCollection<ConceptTypeChildHasPathRole> roleConceptTypeChildren = ConceptTypeChildHasPathRole.GetLinksToConceptTypeChild(newRole);
				foreach (ConceptTypeChildHasPathRole conceptTypeHasChildHasPathRole in roleConceptTypeChildren)
				{
					conceptTypeHasChildCollection.Add(conceptTypeHasChildHasPathRole.ConceptTypeChild);
				}
			}
			return conceptTypeHasChildCollection;
		}
		/// <summary>
		/// Gets a <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/> that contains all the
		/// <see cref="Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> relationships that have <see cref="Neumont.Tools.ORM.ObjectModel.Role"/>
		/// objects in their path role collection that are in the passed constraint's role collection.
		/// </summary>
		/// <param name="constraintRoleSequence">A <see cref="Neumont.Tools.ORM.ObjectModel.SetComparisonConstraintRoleSequence"/> to generate a 
		/// <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/> for.</param>
		/// <param name="checkOppositeRole">True if the opposite roles of <paramref name="constraint"/>'s role collection should be checked
		/// instead of the normal roles. Otherwise, false. An example would be a <example>DisjunctiveMandatoryConstraint</example>.</param>
		/// <returns>A <see cref="System.Collections.Generic.List&lt;ConceptTypeChild&gt;"/>.</returns>
		private List<ConceptTypeChild> GetConceptTypeChildRelationshipsForSetComparisonConstraints(SetComparisonConstraintRoleSequence constraintRoleSequence, bool checkOppositeRole)
		{
			return GetConceptTypeChildRelationshipsForSetConstraints(constraintRoleSequence.RoleCollection, checkOppositeRole);
			//List<ConceptTypeChild> conceptTypeHasChildCollection = new List<ConceptTypeChild>();
			//LinkedElementCollection<Role> roleSequenceCollection = constraint.RoleSequenceCollection;
			//// Checking the role collection count is not equal to zero ensures that we have only external constraints (or constraint that
			//// will map to external constraint in the co-referenced version of the model). We have checked for this in the calling method.
			//foreach (SetComparisonConstraintRoleSequence roleSequence in roleSequenceCollection)
			//{

			//    Role newRole = role;
			//    if (checkOppositeRole)
			//    {
			//        Neumont.Tools.ORM.OIALModel.SubsetConstraint n = new SubsetConstraint();
			//        Neumont.Tools.ORM.ObjectModel.SubsetConstraint s = new Neumont.Tools.ORM.ObjectModel.SubsetConstraint();
			//        RoleBase oppositeRoleBase = role.OppositeRole;
			//        Debug.Assert(oppositeRoleBase != null, "Opposite Role Base was null when generating an Child Sequence Constraint.");
			//        newRole = oppositeRoleBase.Role;
			//    }
			//    ReadOnlyCollection<ConceptTypeChildHasPathRole> roleConceptTypeChildren = ConceptTypeChildHasPathRole.GetLinksToConceptTypeChild(newRole);
			//    foreach (ConceptTypeChildHasPathRole conceptTypeHasChildHasPathRole in roleConceptTypeChildren)
			//    {
			//        conceptTypeHasChildCollection.Add(conceptTypeHasChildHasPathRole.ConceptTypeChild);
			//    }
			//}
			//return conceptTypeHasChildCollection;
		}
		#endregion // Helper Methods
		#region Corresponding Structures For OIAL Implementation
		/// <summary>
		/// For the absorption of <see cref="FactType"/>s, this determines whether how
		/// the absorption algorithm of <see cref="FactType"/>s will be implemented
		/// </summary>
		private enum FactAbsorptionType
		{
			/// <summary>
			/// Indicates that only the <see cref="FactType"/> itself will be absorbed
			/// into the <see cref="ObjectType"/>
			/// </summary>
			FactOnly,
			/// <summary>
			/// Indicates that the <see cref="FactType"/> and everything it connects
			/// to (except the current <see cref="ObjectType"/>) will be absorbed into
			/// the current <see cref="ObjectType"/>
			/// </summary>
			Fully
		}
		/// <remarks>
		/// A structure that keeps track of where and how <see cref="FactType"/>s will be absorbed.
		/// </remarks>
		private struct AbsorbedFactType
		{
			/// <summary>
			/// The <see cref="Guid"/> of the absorber <see cref="ObjectType"/>.
			/// </summary>
			private Guid absorberId;
			/// <summary>
			/// The type of absorption in the relationship.
			/// </summary>
			private FactAbsorptionType typeOfAbsorption;
			/// <summary>
			/// Creates a new instance of this <see cref="AbsorbedFactType"/>
			/// </summary>
			/// <param name="absorberId">The id of the <see cref="ObjectType"/> to which a <see cref="FactType"/> is absorbed</param>
			/// <param name="typeOfAbsorption">The <see cref="FactAbsorptionType"/> of a specific absorption from a <see cref="FactType"/>
			/// to an <see cref="ObjectType"/></param>
			public AbsorbedFactType(Guid absorberId, FactAbsorptionType typeOfAbsorption)
			{
				this.absorberId = absorberId;
				this.typeOfAbsorption = typeOfAbsorption;
			}
			/// <summary>
			/// Gets the <see cref="Guid"/> of the absorber <see cref="ObjectType"/>.
			/// </summary>
			public Guid AbsorberId
			{
				get { return absorberId; }
			}
			/// <summary>
			/// Gets the <see cref="FactAbsorptionType"/> of the absorption.
			/// </summary>
			public FactAbsorptionType AbsorptionType
			{
				get { return typeOfAbsorption; }
			}
		}
		#endregion // Corresponding Structures For OIAL Implementation
		#endregion // Core OIAL Implementation
		#region Peripheral OIAL Implementation
		/// <summary>
		/// Changes this <see cref="OIALModel"/> to accommodate an added mandatory role constraint
		/// </summary>
		/// <param name="mandatoryConstraint">The <see cref="MandatoryConstraint"/> which has just been added to the diagram.</param>
		public void ProcessSimpleMandatoryRoleConstraintAdded(MandatoryConstraint mandatoryConstraint)
		{
			// UNDONE: Is there ever a chance that we won't get a binary fact type?
			bool manyToManyOrNoConstraints = true;
			//bool manyToOne = false;
			//bool oneToOne = false;
			LinkedElementCollection<SetConstraint> factTypeSetConstraints = mandatoryConstraint.RoleCollection[0].FactType.SetConstraintCollection;
			int setConstraintCount = factTypeSetConstraints.Count;
			int uConstraintCount = 0;
			int mConstraintCount = 0;
			Role uniqueRole = null, mandatoryRole = null;
			for (int i = 0; i < setConstraintCount; ++i)
			{
				SetConstraint setConstraint = factTypeSetConstraints[i];
				LinkedElementCollection<Role> setConstraintRoles = setConstraint.RoleCollection;
				UniquenessConstraint uConstraint = setConstraint as UniquenessConstraint;
				// If the fact type is many-to-many or has no uniqueness constraints, do not do anything.
				if (uConstraint != null && uConstraint.IsInternal && setConstraintRoles.Count != 2)
				{
					manyToManyOrNoConstraints = false;
					++uConstraintCount;
					// TODO: Check if it will always be the first role.
					uniqueRole = setConstraintRoles[0];
					if (uniqueRole.IsMandatory)
					{
						++mConstraintCount;
						mandatoryRole = uniqueRole;
					}
				}
			}
			if (manyToManyOrNoConstraints)
			{
				return;
			}
			if (uConstraintCount == 1)
			{
				// Get ConceptTypeChild and change the Mandatory property.
				return;
			}
			// We are left with one-to-one fact types.

			// 1.	Absorption usually goes toward the non-mandatory role player. However, if the non-mandatory role player
			// doesn’t play any functional roles aside from those that supply its preferred identifier, then it goes toward
			// the mandatory role player. The mandatory role player will have a Concept Type Ref to the non-mandatory role player.
			// 2.	But if the non-mandatory role player does play other functional roles, then the mandatory role player will be absorbed into the
			// non-mandatory role player. There will be no ConceptTypeRefs, just an AbsorbedConceptType.
			if (mConstraintCount == 1)
			{
				ObjectType nonMandatoryRolePlayer = mandatoryRole.OppositeRole.Role.RolePlayer;
				ObjectType mandatoryRolePlayer = mandatoryRole.RolePlayer;
				int nonMandatoryFunctionalRoleCount = GetFunctionalNonDependentRoleCount(nonMandatoryRolePlayer.PlayedRoleCollection, null);
				int mandatoryFunctionalRoleCount = GetFunctionalNonDependentRoleCount(mandatoryRolePlayer.PlayedRoleCollection, null);
				if (nonMandatoryFunctionalRoleCount == 0)
				{
					// Nothing should be changed. The mandatory role player should have a concept type ref to the non-mandatory role player
					return;
				}
				LinkedElementCollection<ConceptType> conceptTypes = ConceptTypeCollection;
				int conceptTypeCount = conceptTypes.Count;
				ConceptType absorbingConceptType = null, absorberConceptType = null;
				for (int i = 0; i < conceptTypeCount; ++i)
				{
					ConceptType conceptType = conceptTypes[i];
					ObjectType conceptObjectType = conceptType.ObjectType;
					if (conceptObjectType == nonMandatoryRolePlayer)
					{
						absorberConceptType = conceptType;
						continue;
					}
					if (conceptObjectType == mandatoryRolePlayer)
					{
						absorbingConceptType = conceptType;
					}
				}
				absorberConceptType.AbsorbedConceptTypeCollection.Add(absorbingConceptType);
			}
			//RoleMoveableCollection factTypeRoles = factType.RoleCollection;
			//int roleCount = factTypeRoles.Count;
			//for (int i = 0; i < roleCount; ++i)
			//{
			//    Role role = factTypeRoles[i];
			//    if (role.IsMandatory)
			//    {

			//    }
			//    //ConstraintRoleSequenceMoveableCollection roleConstraints = role.ConstraintRoleSequenceCollection;
			//    //for (int j = 0; j < roleConstraints.Count; ++j)
			//    //{
			//    //    UniquenessConstraint uConstraint = roleConstraints[j] as UniquenessConstraint;
			//    //    if (uConstraint != null && uConstraint.IsInternal && 
			//    //}
			//}
		}
		#endregion // Peripheral OIAL Implementation
	}
	#endregion // Live OIAL Implementation
}