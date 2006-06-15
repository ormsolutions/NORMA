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
// Uncomment the following line to view element links in a debug: view store window during every change
// in the model. (One per "user" change i.e. adding an internal uniqueness constraint fires multiple rules
// but really makes only one change in the user's perspective.)
// #define VIEW_ELEMENT_LINKS
#region Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Framework;
using System.Globalization;
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
			OIALModel oial = (OIALModel)model.GetCounterpartRolePlayer(
				OIALModelHasORMModel.ORMModelMetaRoleGuid,
				OIALModelHasORMModel.OIALModelMetaRoleGuid, false);
			if (oial == null)
			{
				oial = OIALModel.CreateOIALModel(model.Store);
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
		private static class CheckConceptTypeParentExclusiveMandatory
		{
			/// <summary>
			/// Checks if a ConcepType is its own parent.
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
						//throw new InvalidOperationException("A ConceptType must have exactly one parent (either the OIALModel, or a ConceptType that absorbed it).");
					}
				}
			}
			/// <summary>
			/// This rule fires when the OIALModel has a conceptType added to it.
			/// </summary>
			[RuleOn(typeof(OIALModelHasConceptType))]
			private sealed class OIALModelHasConceptTypeAddRule : AddRule
			{
				/// <summary>
				/// When the ConceptType is added we need to process it.
				/// </summary>
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as OIALModelHasConceptType).ConceptTypeCollection);
				}
			}
			/// <summary>
			/// This rule fires when the OIALModel Has a ConceptType Removed.
			/// </summary>
			[RuleOn(typeof(OIALModelHasConceptType))]
			private sealed class OIALModelHasConceptTypeRemoveRule : RemoveRule
			{
				/// <summary>
				/// When the ConcepType is removed we process it.
				/// </summary>
				public override void ElementRemoved(ElementRemovedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as OIALModelHasConceptType).ConceptTypeCollection);
				}
			}
			/// <summary>
			/// This rule fires wen the OIALModel absorbs a ConceptType.
			/// </summary>
			[RuleOn(typeof(ConceptTypeAbsorbedConceptType))]
			private sealed class ConceptTypeAbsorbedConceptTypeAddRule : AddRule
			{
				/// <summary>
				/// When a ConceptType absorbed another ConceptType we process it.
				/// </summary>
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as ConceptTypeAbsorbedConceptType).AbsorbedConceptTypeCollection);
				}
			}
			/// <summary>
			/// This rule fires when a ConceptType that has been absorded is removed from its parent.
			/// </summary>
			[RuleOn(typeof(ConceptTypeAbsorbedConceptType))]
			private sealed class ConceptTypeAbsorbedConceptTypeRemoveRule : RemoveRule
			{
				/// <summary>
				/// When an concepttype is removed we need to process it.
				/// </summary>
				public override void ElementRemoved(ElementRemovedEventArgs e)
				{
					ProcessConceptType((e.ModelElement as ConceptTypeAbsorbedConceptType).AbsorbedConceptTypeCollection);
				}
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType))]
		private sealed class ModelHasObjectTypeAddRule : AddRule
		{
			/// <summary>
			/// When an ObjectType is added we DelayValidate the Model.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as ModelHasObjectType).Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasObjectType))]
		private sealed class ModelHasObjectTypeRemovingRule : RemovingRule
		{
			/// <summary>
			/// When an ObjectType is removed from a model we DelayValidate the Model.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectType objectType = (e.ModelElement as ModelHasObjectType).ObjectTypeCollection;
				ORMModel model = objectType.Model;
				ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for any changes being made to an ObjectType.
		/// </summary>
		[RuleOn(typeof(ObjectType))]
		private sealed class ObjectTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// When an ObjectType is changes we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ORMModel model = (e.ModelElement as ObjectType).Model;
				if (model != null)
				{
					ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This rule listens for when a FactType is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))]
		private sealed class ModelHasFactTypeAddRule : AddRule
		{
			/// <summary>
			/// When a FactType is added to the Model we DelayValidate the Model.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as ModelHasFactType).Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when a FactType is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))]
		private sealed class ModelHasFactTypeRemovingRule : RemovingRule
		{
			/// <summary>
			/// When a FactType is removed we DelayValidate the Model.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				FactType fact = (e.ModelElement as ModelHasFactType).FactTypeCollection;
				ORMModel model = fact.Model;
				ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when a FactType is changed.
		/// </summary>
		[RuleOn(typeof(FactType))]
		private sealed class FactTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// When a FactType is changed we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ORMModel model = (e.ModelElement as FactType).Model;
				if (model != null)
				{
					ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This model listens for when A SetConstraint is added to the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))]
		private sealed class ModelHasSetConstraintAddRule : AddRule
		{
			/// <summary>
			/// When a SetConstraint is added we DelayValidate the Model.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				ORMMetaModel.DelayValidateElement(setConstraint.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when A SetConstraint is changed.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))]
		private sealed class ModelHasSetConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a SetConstraint is changed we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				if (model != null)
				{
					ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
		}
		/// <summary>
		/// This rule listens for when a SetConstraint is removed from the Model.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))]
		private sealed class ModelHasSetConstraintRemovingRule : RemovingRule
		{
			/// <summary>
			/// When a SetConstraint is removed we DelayValidate the Model.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ModelHasSetConstraint setConstraint = e.ModelElement as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule listens for when an ObjectType plays a role.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private sealed class ObjectTypePlaysRoleAddRule : AddRule
		{
			/// <summary>
			/// When an ObjectType plays a role we DelayValidate the Model.
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole objectRole = e.ModelElement as ObjectTypePlaysRole;
				ObjectType objectType = objectRole.RolePlayer;
				ORMMetaModel.DelayValidateElement(objectType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when An ObjectType no longer plays a role.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private sealed class ObjectTypePlaysRoleRemovingRule : RemovingRule
		{
			/// <summary>
			/// When an ObjectType plays role is removed we DelayValidate the Model.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectTypePlaysRole objectRole = e.ModelElement as ObjectTypePlaysRole;
				ObjectType objectType = objectRole.RolePlayer;
				ORMMetaModel.DelayValidateElement(objectType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when a ConstraintRoleSequence is added to a Role.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private sealed class ConstraintRoleSequenceHasRoleAddRule : AddRule
		{
			/// <summary>
			/// When a ConstraintRoleSequence is added to a Role we DelayValidate the Model.
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole constraintSequence = e.ModelElement as ConstraintRoleSequenceHasRole;
				RoleBase rolebase = constraintSequence.RoleCollection;
				FactType factType = rolebase.FactType;
				ORMMetaModel.DelayValidateElement(factType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when a ConstraintRoleSequence is removed from a Role.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private sealed class ConstraintRoleSequenceHasRoleRemovingRule : RemovingRule
		{
			/// <summary>
			/// When a ConstraintRoleSeqeunce is removed from a Role we DelayValidate the Model.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ConstraintRoleSequenceHasRole constraintSequence = e.ModelElement as ConstraintRoleSequenceHasRole;
				RoleBase rolebase = constraintSequence.RoleCollection;
				FactType factType = rolebase.FactType;
				ORMMetaModel.DelayValidateElement(factType.Model, DelayValidateModel);
			}
		}
		/// <summary>
		/// This rule fires when a UniquenessContraint is changed.
		/// </summary>
		[RuleOn(typeof(UniquenessConstraint))]
		private sealed class UniquenessConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a UniquenessConstraint is changed we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				UniquenessConstraint constraint = e.ModelElement as UniquenessConstraint;
				RoleMoveableCollection roles = constraint.RoleCollection;
				int rolesCount = roles.Count;
				int i;
				for (i = 0; i < rolesCount; ++i)
				{
					RoleBase role = roles[i];
					ORMModel model = role.FactType.Model;
					if (model != null)
					{
						ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
					}
				}
			}
		}
		/// <summary>
		/// This rule fires when a MandatoryConstraint is changed.
		/// </summary>
		[RuleOn(typeof(MandatoryConstraint))]
		private sealed class MandatoryConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// When a MandatoryConstraint is changed we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				MandatoryConstraint constraint = e.ModelElement as MandatoryConstraint;
				RoleMoveableCollection roles = constraint.RoleCollection;
				int rolesCount = roles.Count;
				int i;
				for (i = 0; i < rolesCount; ++i)
				{
					RoleBase role = roles[i];
					ORMModel model = role.FactType.Model;
					if (model != null)
					{
						ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
					}
				}
			}
		}
		/// <summary>
		/// This rule fires when a RoleBase is changed.
		/// </summary>
		[RuleOn(typeof(RoleBase))]
		private sealed class RoleBaseChangeRule : ChangeRule
		{
			/// <summary>
			/// When a RoleBase is changed we DelayValidate the Model.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				RoleBase role = e.ModelElement as RoleBase;
				FactType fact = role.FactType;
				ORMModel model = fact.Model;
				if (model != null)
				{
					ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
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
		private class OialObjectTypeFixupListener : DeserializationFixupListener<ObjectType>
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
			protected override void ProcessElement(ObjectType element, Store store, INotifyElementAdded notifyAdded)
			{
				ObjectType objectType = element as ObjectType;
				ORMModel model = objectType.Model;
				OIALModel oil = (OIALModel)model.GetCounterpartRolePlayer(
					OIALModelHasORMModel.ORMModelMetaRoleGuid,
					OIALModelHasORMModel.OIALModelMetaRoleGuid,
					false);
				if (oil == null)
				{
					oil = OIALModel.CreateOIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				ORMMetaModel.DelayValidateElement(objectType.Model, DelayValidateModel);
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
		private class OialFactTypeFixupListener : DeserializationFixupListener<FactType>
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
			protected override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				FactType fact = element as FactType;
				ORMModel model = fact.Model;
				OIALModel oil = (OIALModel)model.GetCounterpartRolePlayer(
					OIALModelHasORMModel.ORMModelMetaRoleGuid,
					OIALModelHasORMModel.OIALModelMetaRoleGuid,
					false);
				if (oil == null)
				{
					oil = OIALModel.CreateOIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
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
		private class OialModelHasSetConstraintFixupListener : DeserializationFixupListener<ModelHasSetConstraint>
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
			protected override void ProcessElement(ModelHasSetConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				ModelHasSetConstraint setConstraint = element as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				OIALModel oil = (OIALModel)model.GetCounterpartRolePlayer(
					OIALModelHasORMModel.ORMModelMetaRoleGuid,
					OIALModelHasORMModel.OIALModelMetaRoleGuid,
					false);
				if (oil == null)
				{
					oil = OIALModel.CreateOIALModel(store);
					oil.ORMModel = model;
					notifyAdded.ElementAdded(oil, true);
				}
				ORMMetaModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		#endregion // Deserialization FixupListeners
	}
	#endregion // OIALModel Rules and Validation
	#region Live OIAL Implementation
	#region MandatoryConstraintModality Enumeration
	/// <summary>
	/// A list of constraint modalities for simple mandatory role constraints used in <see cref="ConceptTypeHasChild"/>
	/// relationships.
	/// </summary>
	public enum MandatoryConstraintModality
	{
		/// <summary>
		/// The child contained by a <see cref="ConceptType"/> is not mandatory.
		/// </summary>
		NotMandatory,
		/// <summary>
		/// The child contained by a <see cref="ConceptType"/> is alethicly mandatory.
		/// </summary>
		Alethic,
		/// <summary>
		/// The child contained by a <see cref="ConceptType"/> is deonticly mandatory.
		/// </summary>
		Deontic
	}
	#endregion // MandatoryConstraintModality Enumeration
	#region OIALModel Algorithms
	public partial class OIALModel
	{
		#region Core OIAL Implementation
		#region Initializers and Debugging
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
		/// <summary>
		/// This <see cref="List&lt;Guid&gt;"/> contains the IDs of all the top-level types of the diagrams. Top-level
		/// types are <see cref="ObjectType"/>s which map to <see cref="ConceptType"/> which are direct children of
		/// this <see cref="OIALModel"/> objects.
		/// </summary>
		private List<Guid> myTopLevelTypes = null;
		/// <summary>
		/// Processes the current ORM Model by determining which facts and objects
		/// should be noted by this <see cref="PrimaryElementTracker"/>
		/// </summary>
		/// <param name="model">The current <see cref="ORMModel"/> represented by the diagram</param>
		private void ProcessModelForTopLevelTypes()
		{
			// Clears the OIALModel previous to re-processing.
			ConceptTypeMoveableCollection thisConceptTypeCollection = ConceptTypeCollection;
			thisConceptTypeCollection.Clear();
			InformationTypeFormatCollection.Clear();
			ChildSequenceConstraintCollection.Clear();
			ORMModel model = this.ORMModel;
			FactTypeMoveableCollection modelFactTypes = model.FactTypeCollection;
			ObjectTypeMoveableCollection modelObjectTypes = model.ObjectTypeCollection;
			Store store = model.Store;

			// TODO: Monitor this area for bugs in fact set constraint collection.
			FactTypeAbsorption(modelFactTypes);
			ObjectTypeAbsorption(modelObjectTypes);
			TopLevelObjectTypes(modelObjectTypes);
			InformationTypeFormats(store, modelObjectTypes);

			// TODO: Easier way to do this? We must get the object type for each top-level type's ID
			// and pass this to the ConceptTypes() method, which generates our concept types.
			ElementDirectory elementDirectory = store.ElementDirectory;
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
			GetExternalConstraints(store, model);
#if DEBUG && VIEW_ELEMENT_LINKS

			ViewElementLinks(store, thisConceptTypeCollection, conceptTypeCount);
#endif // DEBUG && VIEW_ELEMENT_LINKS
		}
#if DEBUG && VIEW_ELEMENT_LINKS
		/// <summary>
		/// Shows the Element Links in a Debug.Assert()-style window
		/// </summary>
		/// <param name="store">The store currently attached to the model.</param>
		/// <param name="conceptTypes">A collection of <see cref="ConceptType"/> objects whose
		/// links are of interest.</param>
		/// <param name="conceptTypeCount">The number of <see cref="ConceptType"/>s in the collection</param>
		private void ViewElementLinks(Store store, ConceptTypeMoveableCollection conceptTypes, int conceptTypeCount)
		{
			// UNDONE: To be able to view ElementLinks in the debugger window, we have to add the
			// ElementLinks of each ConceptType to a ModelElement array. Then we use an overload
			// of Debug.Assert() to view this information. The Search.FindTreesInForest() does the
			// same thing for the ModelElements (not ElementLinks) of the store.
			
			ArrayList elementLinkList = new ArrayList();
			for (int i = 0; i < conceptTypeCount; ++i)
			{
				elementLinkList.AddRange(conceptTypes[i].GetElementLinks());
			}
			elementLinkList.AddRange(Search.FindTreesInForest(store));
			ModelElement[] elementLinkArray = new ModelElement[elementLinkList.Count];
			elementLinkList.CopyTo(elementLinkArray);
			Debug.Assert(store.DefaultPartition, elementLinkArray);
		}
#endif // DEBUG && VIEW_ELEMENT_LINKS
		#endregion // Initializers and Debugging
		#region ORMToOIAL Algorithms
		/// <summary>
		/// Determines to which object types all fact types in the diagram (after being
		/// co-referenced) are absorbed.
		/// </summary>
		/// <param name="modelFactTypes">
		/// A <see cref="FactTypeMoveableCollection"/> which represents
		/// all of the <see cref="FactType"/> objects on the current diagram.
		/// </param>
		private void FactTypeAbsorption(FactTypeMoveableCollection modelFactTypes)
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
				if (factType.RoleCollection.Count == 2)
				{
					bool hasSpanningUniqueness = false;
					foreach (SetConstraint scc in factType.SetConstraintCollection)
					{
						if (scc.RoleCollection.Count == 2)
						{
							hasSpanningUniqueness = true;
							break;
						}
					}
					if (hasSpanningUniqueness)
					{
						continue;
					}
					RoleBaseMoveableCollection factTypeRoles = factType.RoleCollection;
					firstRole = factTypeRoles[0].Role;	// CHANGE: From Role to RoleBase
					secondRole = factTypeRoles[1].Role;	// CHANGE: From Role to RoleBase
					RoleMultiplicity firstMultiplicity = firstRole.Multiplicity;
					RoleMultiplicity secondMultiplicity = secondRole.Multiplicity;

					// If the fact type's multiplicity is one-to-one
					if (factType.GetInternalConstraintsCount(ConstraintType.InternalUniqueness) == 2)
					{
						int mandatoryRoleCount = factType.GetInternalConstraintsCount(ConstraintType.SimpleMandatory);
						// If only one role is mandatory
						if (mandatoryRoleCount == 1)
						{
							Role nonMandatoryRole = null;
							// Find the RolePlayer which does not have the mandatory role constraint,
							// because it usually absorbs the object type that has the mandatory
							// role constraint
							nonMandatoryRole = firstRole.IsMandatory ? secondRole : firstRole;

							// If the nonMandatoryRole's RolePlayer has other functional roles
							// that are not part of the primary identifier, absorb toward that
							// ObjectType
							ObjectType rolePlayer = nonMandatoryRole.RolePlayer;
							ObjectType oppositePlayer = nonMandatoryRole.OppositeRole.Role.RolePlayer;
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
							int firstFunctionalRoleCount = GetFunctionalRoleCount(firstObject.PlayedRoleCollection, firstRole);
							int secondFunctionalRoleCount = GetFunctionalRoleCount(secondObject.PlayedRoleCollection, secondRole);

							// Compares the number of the functional roles played by the first
							// object type to the number of functional roles played by the second
							// object type. If the first is greater than or equal to the second,
							// absorb into the first.
							if (firstFunctionalRoleCount >= secondFunctionalRoleCount)
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
					//else if (factType.SetConstraintCollection.Count == 0)
					//{
					//    stopProcessing = true;
					//}
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
		private void ObjectTypeAbsorption(ObjectTypeMoveableCollection modelObjectTypes)
		{
			// Check if the Dictionary of absorbed ObjectTypes is null; if so, instantiate it.
			if (myAbsorbedObjectTypes == null)
			{
				myAbsorbedObjectTypes = new SortedDictionary<Guid, Guid>();
			}
			else
			{
				myAbsorbedObjectTypes.Clear();
			}
			// This Guid is used to store what ObjectType is absorbing the current ObjectType
			// being checked in the foreach loop
			Guid absorberGuid = Guid.Empty;
			foreach (ObjectType objectType in modelObjectTypes)
			{
				absorberGuid = Guid.Empty;

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
						List<FactType> mandatoryDirectFactTypes = new List<FactType>();
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
							List<AbsorbedFactType> currentAbsorbedFactTypes = new List<AbsorbedFactType>();
							foreach (FactType factType in mandatoryDirectFactTypes)
							{
								Guid factTypeId = factType.Id;
								if (myAbsorbedFactTypes.ContainsKey(factTypeId))
								{
									currentAbsorbedFactTypes.Add(myAbsorbedFactTypes[factTypeId]);
								}
							}
							foreach (AbsorbedFactType currentAbsorbedFactType in currentAbsorbedFactTypes)
							{
								// If the factAbsorptionType is fully and the current object type is not
								// absorbing itself, record the absorberGuid as the id of the ObjectType
								// recorded in the AbsorbedFactType object.
								Guid absorberId = currentAbsorbedFactType.AbsorberId;
								if (currentAbsorbedFactType.TypeOfAbsorption == FactAbsorptionType.Fully &&
									absorberId != objectType.Id)
								{
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
				}
			}
		}
		/// <summary>
		/// Determines what <see cref="ObjectType"/>s in the model are actually top-level object types,
		/// or object types that stand alone as separate entities in an attribute-based model.
		/// </summary>
		/// <param name="modelObjectTypes">The <see cref="ORMModel"/>'s collection of <see cref="ObjectType"/>s</param>
		private void TopLevelObjectTypes(ObjectTypeMoveableCollection modelObjectTypes)
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
						bool isAbsorbed = false;

						// If the id of any of those fact types in the factTypeAbsorptionsAwayFromThisObjectType List
						// match the id of any of the fact types in this object's functional direct fact types,
						// then it is not a top-level object type.
						foreach (FactType factType in functionalRoleFactTypes)
						{
							if (factTypeAbsorptionsAwayFromThisObjectType.Contains(factType.Id))
							{
								isAbsorbed = true;
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
		private void InformationTypeFormats(Store store, ObjectTypeMoveableCollection modelObjectTypes)
		{
			int count = modelObjectTypes.Count;
			InformationTypeFormatMoveableCollection thisInformationTypeFormats = InformationTypeFormatCollection;
			for (int i = 0; i < count; ++i)
			{
				ObjectType currentObject = modelObjectTypes[i];
				if (currentObject.IsValueType)
				{
					// No data type can be held for an UnspecifiedDataType
					if (!(currentObject.DataType is UnspecifiedDataType))
					{
						InformationTypeFormat itf = InformationTypeFormat.CreateAndInitializeInformationTypeFormat(store,
							new AttributeAssignment[]
								{
									new AttributeAssignment(InformationTypeFormat.NameMetaAttributeGuid, currentObject.Name, store)
								});
						itf.ValueType = currentObject;
						thisInformationTypeFormats.Add(itf);
					}
				}
			}
		}
		/// <summary>
		/// Finds the Concept Types in the model and adds them to its collection of <see cref="ConceptType"/> objects
		/// </summary>
		/// <param name="store">The current store of the <see cref="OIALMetaModel"/></param>
		/// <param name="objectType">The <see cref="ObjectType"/> which will be changed into a <see cref="ConceptType"/></param>
		/// <param name="parentConcept">The parent <see cref="ConceptType"/>, if any, which will absorb the <see cref="ConceptType"/>
		/// generated from this <see cref="ObjectType"/></param>
		private void ConceptTypes(Store store, ObjectType objectType, ConceptType parentConcept)
		{
			// TODO: Constraints need to added to ConceptTypeAbsorbedConceptType once OIAL constraints
			// have been further refined.
			// The name of the concept type is the name of the object type that represents that concept type.
			ConceptType conceptType = ConceptType.CreateAndInitializeConceptType(store,
				new AttributeAssignment[]
				{
					new AttributeAssignment(ConceptType.NameMetaAttributeGuid, objectType.Name, store)
				});

			conceptType.ObjectType = objectType;
			Guid objectGuid = objectType.Id;

			// We loop through the dictionary of absorbed object types to look for any more concept types that will be children
			// of the current concept type
			foreach (KeyValuePair<Guid, Guid> absorbedObjectTypeGuidPair in myAbsorbedObjectTypes)
			{
				if (absorbedObjectTypeGuidPair.Value.Equals(objectGuid))
				{
					ConceptTypes(store, store.ElementDirectory.GetElement(absorbedObjectTypeGuidPair.Key) as ObjectType, conceptType);
				}
			}
			// Either add the concept type to the root (OIALModel) or to another ConceptType
			if (parentConcept == null)
			{
				ConceptTypeCollection.Add(conceptType);
			}
			else
			{
				parentConcept.AbsorbedConceptTypeCollection.Add(conceptType);
				IList conceptTypeAbsorbedConceptTypeCollection = parentConcept.GetElementLinks(ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeMetaRoleGuid, false);
				int count = conceptTypeAbsorbedConceptTypeCollection.Count;
				ConceptTypeAbsorbedConceptType conceptTypeAbsorbedConceptType = conceptTypeAbsorbedConceptTypeCollection[count - 1] as ConceptTypeAbsorbedConceptType;
				RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
				ObjectType parentObject = parentConcept.ObjectType;
				foreach (Role role in playedRoles)
				{
					RoleBase oppositeRoleBase = role.OppositeRole;
					if (oppositeRoleBase != null)
					{
						if (object.ReferenceEquals(parentObject, oppositeRoleBase.Role.RolePlayer))
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
		/// <param name="store">The current store of the <see cref="OIALMetaModel"/></param>
		private void InformationTypesAndConceptTypeRefs(Store store, ConceptType conceptType)
		{
			ObjectType conceptObjectType = conceptType.ObjectType;
			Guid objectId = conceptObjectType.Id;

			// If the concept type's object type is a value type, we must record an information type for that concept type.
			if (conceptObjectType.IsValueType)
			{
				// Create the Information Type that represents this value type.
				InformationTypeFormat informationTypeFormat = GetInformationTypeFormat(conceptObjectType);
				InformationType newInformationType = InformationType.CreateAndInitializeInformationType(store,
					new AttributeAssignment[]
						{
							new AttributeAssignment(InformationType.NameMetaAttributeGuid, informationTypeFormat.Name + "Value", store)
						});
				newInformationType.InformationTypeFormat = informationTypeFormat;
				conceptType.InformationTypeCollection.Add(newInformationType);

				// Create the Mandatory constraint and Single Child Constraint for this link.
				IList absorbedInformationTypes = conceptType.GetElementLinks(ConceptTypeHasInformationType.ConceptTypeMetaRoleGuid, false);
				int index = absorbedInformationTypes.Count - 1;
				ConceptTypeHasInformationType absorbedInfoType = absorbedInformationTypes[index] as ConceptTypeHasInformationType;
				absorbedInfoType.Mandatory = MandatoryConstraintModality.Alethic;
				SingleChildUniquenessConstraint uConstraint = SingleChildUniquenessConstraint.CreateAndInitializeSingleChildUniquenessConstraint(store,
					new AttributeAssignment[]
						{
							new AttributeAssignment(SingleChildUniquenessConstraint.IsPreferredMetaAttributeGuid, true, store),
							new AttributeAssignment(SingleChildUniquenessConstraint.ModalityMetaAttributeGuid, ConstraintModality.Alethic, store)
						});

				absorbedInfoType.SingleChildConstraintCollection.Add(uConstraint);
			}
			// Get the facts that this object type plays which are functionally determined by this object type
			// which are not absorbed away from this object type
			List<FactType> factList = new List<FactType>();
			RoleMoveableCollection roleCollection = conceptObjectType.PlayedRoleCollection;
			int roleCollectionCount = roleCollection.Count;
			for (int j = 0; j < roleCollectionCount; ++j)
			{
				// Need to check for absorbed subtype meta facts and functional direct facts not absorbed away
				// from this object type: Lines 591 - 594 of ORMtoOIAL. However, exclude any fact types that
				// could result in nested concept types, as we have already dealt with these.
				Role role = roleCollection[j];
				if (role is SubtypeMetaRole)
				{
					continue;
				}
				FactType factType = role.FactType;
				// We have already accounted for nested concept types
				if (factType.RoleCollection.Count != 2)
				{
					continue;
				}
				ConstraintRoleSequenceMoveableCollection constraints = role.ConstraintRoleSequenceCollection;
				int count = constraints.Count;
				// Check the constraints collection to make sure that this is a functional direct fact
				for (int k = 0; k < count; ++k)
				{
					UniquenessConstraint uniquenessConstraint = constraints[k] as UniquenessConstraint;
					if (uniquenessConstraint != null && uniquenessConstraint.IsInternal && uniquenessConstraint.RoleCollection.Count == 1)
					{
						AbsorbedFactType absorbedFactType;
						myAbsorbedFactTypes.TryGetValue(factType.Id, out absorbedFactType);
						Guid id = absorbedFactType.AbsorberId;
						// if (id == objectType.Id) no longer accounted for to dismiss nested concept types
						if ((id == Guid.Empty || id == objectId) && role.OppositeRole.Role.RolePlayer != null)
						{
							factList.Add(factType);
							break;
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
				RoleBaseMoveableCollection factRoles = currentFact.RoleCollection;
				Role firstRole = factRoles[0].Role;
				Role secondRole = factRoles[1].Role;
				if (object.ReferenceEquals(firstRole.RolePlayer, conceptObjectType))
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
					if (baseName.Equals(string.Empty))
					{
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
					GetInformationTypes(store, conceptType, oppositeRole, baseName, constraintModality, constraints);
				}
				else if (oppositeRolePlayerDesiredParentOrTopLevelTypeId.Equals(objectId))
				{
					// CHANGE: Look at the immediate absorbed concept types of the current concept type. If the concept type is not present,
					// we do not do any processing, because we know that later on another concept type will account for the information types
					// of the concept type we are looking for.
					ConceptType absorbedConceptType = null;
					ConceptTypeMoveableCollection absorbedConceptTypes = conceptType.AbsorbedConceptTypeCollection;
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
					if (absorbedConceptType == null)
					{
						return;
					}
					InformationTypesAndConceptTypeRefs(store, absorbedConceptType);
				}
				else if (myTopLevelTypes.Contains(oppositeRolePlayerId) || myAbsorbedObjectTypes.ContainsKey(oppositeRolePlayerId))
				{
					ConceptType referencedConceptType = GetConceptType(ConceptTypeCollection, oppositeRolePlayer, store);
					conceptType.ReferencedConceptTypeCollection.Add(referencedConceptType);
					IList conceptTypeRefs = conceptType.GetElementLinks(ConceptTypeRef.ReferencingConceptTypeMetaRoleGuid, false);
					int count = conceptTypeRefs.Count;
					ConceptTypeRef conceptTypeRef = conceptTypeRefs[count - 1] as ConceptTypeRef;
					conceptTypeRef.PathRoleCollection.Add(oppositeRole);
					conceptTypeRef.OppositeName = referencedConceptType.ObjectType.Name;
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
					foreach (SingleChildConstraint singleChildConstraint in constraints)
					{
						conceptTypeRef.SingleChildConstraintCollection.Add(singleChildConstraint);
					}
				}
				else
				{
					continue;
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
		private void GetInformationTypes(Store store, ConceptType conceptType, Role oppositeRole, string baseName, MandatoryConstraintModality mandatory, IEnumerable<SingleChildConstraint> constraints)
		{
			GetInformationTypesInternal(store, conceptType, oppositeRole, baseName, true, new LinkedList<Role>(), mandatory, constraints);
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
		private void GetInformationTypesInternal(Store store, ConceptType conceptType, Role oppositeRole, string baseName, bool isFirst, LinkedList<Role> pathNodes, MandatoryConstraintModality mandatory, IEnumerable<SingleChildConstraint> constraints)
		{
			// We will most likely add on to the baseName parameter passed to this method.
			string newBaseName = null;
			ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
			// We have found the value type which corresponds to this information type, so we will create the information type
			// and add our Roles (Path Roles) to the ElementLink created (ConceptTypeHasInformationType).
			if (oppositeRolePlayer.IsValueType)
			{
				if (isFirst)
				{
					newBaseName = baseName;
				}
				else
				{
					string concatName = oppositeRole.Name;
					if (concatName.Equals(string.Empty))
					{
						concatName = oppositeRolePlayer.Name;
					}
					newBaseName = string.Concat(baseName, "_", concatName);
				}
				InformationType informationType = InformationType.CreateAndInitializeInformationType(store,
					new AttributeAssignment[]
					{
						new AttributeAssignment(InformationType.NameMetaAttributeGuid, newBaseName, store),
					});
				InformationTypeFormat itf = GetInformationTypeFormat(oppositeRolePlayer);
				informationType.InformationTypeFormat = itf;
				conceptType.InformationTypeCollection.Add(informationType);

				// Get the link we just created
				IList conceptTypeInformationTypeCollection = conceptType.GetElementLinks(ConceptTypeHasInformationType.ConceptTypeMetaRoleGuid, false);
				int count = conceptTypeInformationTypeCollection.Count;
				ConceptTypeHasInformationType conceptTypeInformationType = conceptTypeInformationTypeCollection[count - 1] as ConceptTypeHasInformationType;

				foreach (Role pathRole in pathNodes)
				{
					conceptTypeInformationType.PathRoleCollection.Add(pathRole);
				}
				conceptTypeInformationType.PathRoleCollection.Add(oppositeRole);
					conceptTypeInformationType.Mandatory = mandatory;
				foreach (SingleChildConstraint singleChildConstraint in constraints)
				{
					conceptTypeInformationType.SingleChildConstraintCollection.Add(singleChildConstraint);
				}
			}
			else
			{
				pathNodes.AddLast(oppositeRole);
				RoleMoveableCollection playedRoles = oppositeRolePlayer.PlayedRoleCollection;
				int count = playedRoles.Count;
				for (int i = 0; i < count; ++i)
				{
					RoleBase oppositeRoleBase = playedRoles[i].OppositeRole;
					if (oppositeRoleBase == null)
					{
						continue;
					}
					Role oppRole = oppositeRoleBase.Role;
					ConstraintRoleSequenceMoveableCollection roleSequenceConstraints = oppRole.ConstraintRoleSequenceCollection;
					int constraintsCount = roleSequenceConstraints.Count;
					for (int j = 0; j < constraintsCount; ++j)
					{
						UniquenessConstraint uConstraint = roleSequenceConstraints[j] as UniquenessConstraint;

						if (uConstraint != null && uConstraint.IsPreferred)
						{
							if (isFirst)
							{
								newBaseName = baseName;
							}
							else
							{
								string concatName = oppRole.Name;
								if (concatName.Equals(string.Empty))
								{
									concatName = oppositeRolePlayer.Name;
								}
								newBaseName = string.Concat(baseName, "_", concatName);
							}
							GetInformationTypesInternal(store, conceptType, oppRole, newBaseName, false, pathNodes, mandatory, constraints);
						}
					}
				}
				pathNodes.RemoveLast();
			}
		}
		/// <summary>
		/// Gets the internal child constraints based off the opposite role in a <see cref="ConceptTypeHasChild"/> relationship,
		/// the opposite role being the role on the same side as the child.
		/// </summary>
		/// <param name="role">The opposite role in the <see cref="ConceptTypeHasChild"/> relationship.</param>
		/// <param name="store">The store the current <see cref="OIALModel"/> is attached to.</param>
		/// <returns>An <see cref="IEnumerable&lt;SingleChildConstraint&gt;"/> to add to the <see cref="ConceptTypeHasChild"/>.</returns>
		private IEnumerable<SingleChildConstraint> GetSingleChildConstraints(Role role, Store store)
		{
			ConstraintRoleSequenceMoveableCollection constraintCollection = role.ConstraintRoleSequenceCollection;
			int constraintCount = constraintCollection.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				UniquenessConstraint uConstraint = constraintCollection[i] as UniquenessConstraint;
				if (uConstraint != null && uConstraint.IsInternal)
				{
					SingleChildUniquenessConstraint childUniquenessConstraint = SingleChildUniquenessConstraint.CreateAndInitializeSingleChildUniquenessConstraint(store,
						new AttributeAssignment[]
						{
							new AttributeAssignment(SingleChildUniquenessConstraint.IsPreferredMetaAttributeGuid, uConstraint.IsPreferred, store),
							new AttributeAssignment(SingleChildUniquenessConstraint.ModalityMetaAttributeGuid, uConstraint.Modality, store)
						});
					yield return childUniquenessConstraint;
				}
			}
		}
		/// <summary>
		/// Gets the external constraints associated with the attached <see cref="ORMModel"/>.
		/// </summary>
		/// <param name="store">The store the <paramref name="model"/> (ORMModel) is attached to.</param>
		/// <param name="model">The <see cref="ORMModel"/> currently attached to this <see cref="OIALModel"/>.</param>
		private void GetExternalConstraints(Store store, ORMModel model)
		{
			IList uniquenessConstraints = store.ElementDirectory.GetElements(UniquenessConstraint.MetaClassGuid);
			foreach (UniquenessConstraint uConstraint in uniquenessConstraints)
			{
				RoleMoveableCollection roleCollection = uConstraint.RoleCollection;
				// Checking the role collection count is not equal to zero ensures that we have only ExternalUniquenessConstraints
				if (!uConstraint.IsInternal && roleCollection.Count != 1)
				{
					ChildSequenceUniquenessConstraint childSequenceUniquenessConstraint = ChildSequenceUniquenessConstraint.CreateAndInitializeChildSequenceUniquenessConstraint(store,
						new AttributeAssignment[]
						{
							new AttributeAssignment(ChildSequenceUniquenessConstraint.NameMetaAttributeGuid, uConstraint.Name, store),
							new AttributeAssignment(ChildSequenceUniquenessConstraint.IsPreferredMetaAttributeGuid, uConstraint.IsPreferred, store),
							new AttributeAssignment(ChildSequenceUniquenessConstraint.ModalityMetaAttributeGuid, uConstraint.Modality, store)
						});
					MinTwoChildrenChildSequence minTwoChildrenChildSequence = MinTwoChildrenChildSequence.CreateMinTwoChildrenChildSequence(store);
					foreach (Role role in roleCollection)
					{
						// Converting to common base for the null coalescing operator to work.
						RoleBase thisRole = role.Proxy as RoleBase ?? role;
						IList roleConceptTypeHasChildren = thisRole.GetElementLinks(ConceptTypeHasChildHasPathRole.PathRoleCollectionMetaRoleGuid, false);
						foreach (ConceptTypeHasChildHasPathRole conceptTypeHasChildHasPathRole in roleConceptTypeHasChildren)
						{
							minTwoChildrenChildSequence.ConceptTypeHasChildCollection.Add(conceptTypeHasChildHasPathRole.ConceptTypeHasChild);
						}
					}
					childSequenceUniquenessConstraint.ChildSequence = minTwoChildrenChildSequence;
					childSequenceUniquenessConstraint.OIALModel = this;
				}
			}
		}
		#endregion // ORMToOIAL Algorithms
		#region Helper Methods
		/// <summary>
		/// Gets a collection which can iterated over of <see cref="FactType"/> objects for
		/// a specific <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectTypeRoleCollection">The PlayedRoleCollection of the <see cref="ObjectType"/> of interest</param>
		/// <param name="startingRole">If the <see cref="ObjectType"/> of interest has a specific <see cref="Role"/> whose
		/// <see cref="FactType"/> should not be checked, pass it here. Otherwise, pass null.</param>
		/// <returns>IEnumerable of <see cref="FactType"/> objects</returns>
		private IEnumerable<FactType> GetFunctionalRoles(RoleMoveableCollection objectTypeRoleCollection, Role startingRole)
		{
			foreach (Role role in objectTypeRoleCollection)
			{
				// If null is passed for the starting role, then although this check will be executed, it will
				// never reach the continue statement. Also we do not want to interpret any fact types that are
				// not binarized.
				FactType roleFactType = role.FactType;
				if (role.Equals(startingRole) || roleFactType.Objectification != null || roleFactType.RoleCollection.Count != 2)
				{
					continue;
				}
				// If it is a functional role
				Role oppositeRole = role.OppositeRole.Role;					// CHANGE: Role to RoleBase
				RoleMultiplicity roleMultiplicity = oppositeRole.Multiplicity;
				// TODO: Can this really be zero to one?
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
								break;
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
		/// other than the current <see cref="Role"/> passed to the function. Functional roles
		/// that are part of the primary identifier for an <see cref="ObjectType"/>, i.e.
		/// those that have uniqueness constraints (whether internal or external) that
		/// help identify the <see cref="ObjectType"/>
		/// </summary>
		/// <remarks>
		/// Intended to be a helper method for FactTypeAbsorption algorithm
		/// </remarks>
		/// <param name="objectType">
		/// The <see cref="ObjectType"/> object whose functional role count is of interest
		/// </param>
		/// <param name="startingRole">
		/// The <see cref="Role"/> object whose Role Player is of interest. Pass null if no
		/// role needs to be checked.
		/// </param>
		/// <returns>
		/// An integer with the number of functional roles not part of the primary identifier
		/// </returns>
		private int GetFunctionalRoleCount(RoleMoveableCollection objectTypeRoleCollection, Role startingRole)
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
		private bool IsSubtype(ObjectType objectType)
		{
			RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
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
		private ObjectType GetSupertype(ObjectType objectType)
		{
			RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role role = playedRoles[i];
				SubtypeFact roleFactType = role.FactType as SubtypeFact;
				if (role is SubtypeMetaRole && roleFactType.IsPrimary)
				{
					return role.OppositeRole.Role.RolePlayer;
				}
			}
			return null;
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
		/// <param name="objectType">The <see cref="ObjectType"/> of <see cref="ConceptType"/> which will be referenced.</param>
		/// <param name="store">The store this current <see cref="OIALModel"/> is using.</param>
		/// <returns>The <see cref="ConceptType"/> which is referenced.</returns>
		private ConceptType GetConceptType(ConceptTypeMoveableCollection conceptTypes, ObjectType objectType, Store store)
		{
			int count = conceptTypes.Count;
			for (int i = 0; i < count; ++i)
			{
				ConceptType currentConceptType = conceptTypes[i];
				if (object.ReferenceEquals(currentConceptType.ObjectType, objectType))
				{
					return currentConceptType;
				}
				ConceptTypeMoveableCollection absorbedConceptTypes = currentConceptType.AbsorbedConceptTypeCollection;
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
			InformationTypeFormatMoveableCollection thisInformationTypeFormats = InformationTypeFormatCollection;
			int informationTypeCount = thisInformationTypeFormats.Count;
			for (int i = 0; i < informationTypeCount; ++i)
			{
				InformationTypeFormat currentInformationTypeFormat = thisInformationTypeFormats[i];
				if (object.ReferenceEquals(valueType, currentInformationTypeFormat.ValueType))
				{
					return currentInformationTypeFormat;
				}
			}
			return null;
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
			public FactAbsorptionType TypeOfAbsorption
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
			SetConstraintMoveableCollection factTypeSetConstraints = mandatoryConstraint.RoleCollection[0].FactType.SetConstraintCollection;
			int setConstraintCount = factTypeSetConstraints.Count;
			int uConstraintCount = 0;
			int mConstraintCount = 0;
			Role uniqueRole = null, mandatoryRole = null;
			for (int i = 0; i < setConstraintCount; ++i)
			{
				SetConstraint setConstraint = factTypeSetConstraints[i];
				RoleMoveableCollection setConstraintRoles = setConstraint.RoleCollection;
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
				// Get ConceptTypeHasChild and change the Mandatory property.
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
				int nonMandatoryFunctionalRoleCount = GetFunctionalRoleCount(nonMandatoryRolePlayer.PlayedRoleCollection, null);
				int mandatoryFunctionalRoleCount = GetFunctionalRoleCount(mandatoryRolePlayer.PlayedRoleCollection, null);
				if (nonMandatoryFunctionalRoleCount == 0)
				{
					// Nothing should be changed. The mandatory role player should have a concept type ref to the non-mandatory role player
					return;
				}
				ConceptTypeMoveableCollection conceptTypes = ConceptTypeCollection;
				int conceptTypeCount = conceptTypes.Count;
				ConceptType absorbingConceptType = null, absorberConceptType = null;
				for (int i = 0; i < conceptTypeCount; ++i)
				{
					ConceptType conceptType = conceptTypes[i];
					ObjectType conceptObjectType = conceptType.ObjectType;
					if (object.ReferenceEquals(conceptObjectType, nonMandatoryRolePlayer))
					{
						absorberConceptType = conceptType;
						continue;
					}
					if (object.ReferenceEquals(conceptObjectType, mandatoryRolePlayer))
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
	#endregion // OIALModel Algorithms
	#region Constraint Partial Class
	public partial class Constraint
	{
		/// <summary>
		/// Custom-stored attribute for the <see cref="Constraint"/> class which represents the modality of the constraint.
		/// </summary>
		private ConstraintModality myModality;

		/// <summary>
		/// Standard override for classes with custom stored attributes.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			if (attribute.Id == SingleChildConstraint.ModalityMetaAttributeGuid)
			{
				return myModality;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override for classes with custom stored attributes.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">New value of the attribute.</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			if (attribute.Id == SingleChildConstraint.ModalityMetaAttributeGuid)
			{
				myModality = (ConstraintModality)newValue;
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
	}
	#endregion // Constraint Partial Class
	#region ConceptTypeHasChild Partial Class
	public partial class ConceptTypeHasChild
	{
		/// <summary>
		/// Custom-stored attribute for the <see cref="ConceptTypeHasChild"/> class which represents the modality of the
		/// mandatory constraint on this link. There is a possiblity that this modality may in fact represent a "Not Mandatory" state.
		/// </summary>
		private MandatoryConstraintModality myMandatory;

		/// <summary>
		/// Standard override for classes with custom stored attributes.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			if (attribute.Id == ConceptTypeHasChild.MandatoryMetaAttributeGuid)
			{
				return myMandatory;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override for classes with custom stored attributes.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">New value of the attribute.</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			if (attribute.Id == ConceptTypeHasChild.MandatoryMetaAttributeGuid)
			{
				myMandatory = (MandatoryConstraintModality)newValue;
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
	}
	#endregion // ConceptTypeHasChild Partial Class
	#endregion // Live OIAL Implementation
}