#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling.Diagnostics;

namespace Neumont.Tools.ORM.ObjectModel
{
	[VerbalizationTargetProvider("VerbalizationTargets")]
	[VerbalizationSnippetsProvider("VerbalizationSnippets")]
	public partial class ORMCoreDomainModel : IModelingEventSubscriber, ISurveyNodeProvider
	{
		private static Type[] SurveyErrorQuestionTypes = new Type[] { typeof(SurveyErrorState) };
		private static Type[] SurveyGlyphQuestionTypes = new Type[] { typeof(SurveyQuestionGlyph) };
		/// <summary>
		/// The unique name the VerbalizationBrowser target. Used in the Xml files and in code to identify the core target provider.
		/// </summary>
		public const string VerbalizationTargetName = "VerbalizationBrowser";
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>.
		/// </summary>
		protected void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			Store store = Store;
			if (action == EventHandlerAction.Add &&
				(EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.DocumentLoading) == (reasons & (EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.DocumentLoading)))
			{
				store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[NamedElementDictionary.DefaultAllowDuplicateNamesKey] = null;
			}
			if (0 != (reasons & EventSubscriberReasons.DocumentLoaded))
			{
				if (0 != (reasons & EventSubscriberReasons.ModelStateEvents))
				{
					NamedElementDictionary.ManageModelStateEventHandlers(store, eventManager, action);
					ORMModel.ManageModelStateEventHandlers(store, eventManager, action);
				}
				if (action == EventHandlerAction.Add &&
					0 == (reasons & EventSubscriberReasons.DocumentReloading) &&
					0 != (reasons & EventSubscriberReasons.UserInterfaceEvents))
				{
					Design.ORMEditorUtility.RegisterModelErrorActivators(store);
				}
			}
			if (0 != (reasons & EventSubscriberReasons.SurveyQuestionEvents))
			{
				DomainDataDirectory directory = store.DomainDataDirectory;
				EventHandler<ElementDeletedEventArgs> standardDeleteHandler = new EventHandler<ElementDeletedEventArgs>(ModelElementRemovedEvent);
				EventHandler<ElementPropertyChangedEventArgs> standardGlyphChangeHandler = new EventHandler<ElementPropertyChangedEventArgs>(SurveyGlyphChangedEvent);
				EventHandler<ElementDeletedEventArgs> standardErrorPathDeletedHandler = new EventHandler<ElementDeletedEventArgs>(ModelElementErrorStateChangedEvent);
				//Object Type
				DomainClassInfo classInfo = directory.FindDomainClass(ObjectType.DomainClassId);
				DomainPropertyInfo propertyInfo = directory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChangedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectTypeAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Fact Type
				classInfo = directory.FindDomainClass(FactType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Set Constraint
				classInfo = directory.FindDomainClass(SetConstraint.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetConstraintAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Set Comparison
				classInfo = directory.FindDomainClass(SetComparisonConstraint.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetComparisonConstraintAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				// External constraint expansion
				classInfo = directory.FindDomainRelationship(FactConstraint.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactConstraintAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactConstraintDeletedEvent), action);

				//Track name change
				EventHandler<ElementPropertyChangedEventArgs> standardNameChangedHandler = new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChangedEvent);
				propertyInfo = directory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardNameChangedHandler , action);
				propertyInfo = directory.FindDomainProperty(FactType.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardNameChangedHandler, action);

				//FactTypeHasRole
				classInfo = directory.FindDomainClass(FactTypeHasRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleDeletedEvent), action);

				//Role
				propertyInfo = directory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChangedEvent), action);

				//ValueTypeHasDataType
				classInfo = directory.FindDomainClass(ValueTypeHasDataType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasDataTypeAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasDataTypeDeletedEvent), action);

				//Objectification
				classInfo = directory.FindDomainClass(Objectification.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationDeletedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationRolePlayerChangedEvent), action);
				propertyInfo = directory.FindDomainProperty(Objectification.IsImpliedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectificationChangedEvent), action);

				//Role player changes
				classInfo = directory.FindDomainClass(ObjectTypePlaysRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RolePlayerAddedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RolePlayerDeletedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(RolePlayerRolePlayerChangedEvent), action);

				//Error state changed
				classInfo = directory.FindDomainRelationship(ElementAssociatedWithModelError.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementErrorStateChangedEvent), action);
				eventManager.AddOrRemoveHandler(classInfo, standardErrorPathDeletedHandler, action);

				classInfo = directory.FindDomainRelationship(FactTypeHasFactTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, standardErrorPathDeletedHandler, action);

				classInfo = directory.FindDomainRelationship(ObjectTypeHasObjectTypeInstance.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, standardErrorPathDeletedHandler, action);

				//ModalityChanged
				propertyInfo = directory.FindDomainProperty(SetConstraint.ModalityDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardGlyphChangeHandler, action);
				propertyInfo = directory.FindDomainProperty(SetComparisonConstraint.ModalityDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardGlyphChangeHandler, action);

				//RingType changed
				propertyInfo = directory.FindDomainProperty(RingConstraint.RingTypeDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardGlyphChangeHandler, action);

				//Preferred Identifier Changed
				propertyInfo = directory.FindDomainProperty(UniquenessConstraint.IsPreferredDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardGlyphChangeHandler, action);

				classInfo = directory.FindDomainClass(EntityTypeHasPreferredIdentifier.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, standardErrorPathDeletedHandler, action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(PreferredIdentifierRolePlayerChangedEvent), action);

				//ExclusiveOr added deleted 
				classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ExclusiveOrAddedEvent), action);
				classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ExclusiveOrDeletedEvent), action);

				//SubType
				propertyInfo = directory.FindDomainProperty(SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardGlyphChangeHandler, action);
			}
		}
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			this.ManageModelingEventHandlers(eventManager, reasons, action);
		}
		#endregion // IModelingEventSubscriber Implementation
		#region IVerbalizationTargetProvider implementation
		private sealed class VerbalizationTargets : IVerbalizationTargetProvider
		{
			#region IVerbalizationTargetProvider implementation
			VerbalizationTargetData[] IVerbalizationTargetProvider.ProvideVerbalizationTargets()
			{
				return new VerbalizationTargetData[] { new VerbalizationTargetData("VerbalizationBrowser", ResourceStrings.VerbalizationTargetVerbalizationBrowserDisplayName) };
			}
			#endregion // IVerbalizationTargetProvider implementation
		}
		#endregion // IVerbalizationTargetProvider implementation
		#region IVerbalizationSnippetsProvider Implementation
		private class VerbalizationSnippets : IVerbalizationSnippetsProvider
		{
			/// <summary>
			/// IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets
			/// </summary>
			protected VerbalizationSnippetsData[] ProvideVerbalizationSnippets()
			{
				return new VerbalizationSnippetsData[]
				{
					new VerbalizationSnippetsData(
						typeof(CoreVerbalizationSnippetType),
						CoreVerbalizationSets.Default,
						"Core",
						ResourceStrings.CoreVerbalizationSnippetsTypeDescription,
						ResourceStrings.CoreVerbalizationSnippetsDefaultDescription
					),
					new VerbalizationSnippetsData(
						typeof(ReportVerbalizationSnippetType),
						ReportVerbalizationSets.Default,
						"HtmlReport",
						ResourceStrings.VerbalizationReportSnippetsTypeDescription,
						ResourceStrings.VerbalizationReportSnippetsDefaultDescription
					)
				};
			}
			VerbalizationSnippetsData[] IVerbalizationSnippetsProvider.ProvideVerbalizationSnippets()
			{
				return ProvideVerbalizationSnippets();
			}
		}
		#endregion // IVerbalizationSnippetsProvider Implementation
		#region ISurveyNodeProvider Implementation
		IEnumerable<object> ISurveyNodeProvider.GetSurveyNodes(object context, object expansionKey)
		{
			return this.GetSurveyNodes(context, expansionKey);
		}
		/// <summary>
		/// Provides an <see cref="IEnumerable{SampleDataElementNode}"/> for the <see cref="SurveyTreeContainer"/>.
		/// </summary>
		protected IEnumerable<object> GetSurveyNodes(object context, object expansionKey)
		{
			if (expansionKey == null)
			{
				IElementDirectory elementDirectory = Store.ElementDirectory;
				foreach (FactType element in elementDirectory.FindElements<FactType>(true))
				{
					if (null == element.ImpliedByObjectification)
					{
						yield return element;
					}
				}
				foreach (ObjectType element in elementDirectory.FindElements<ObjectType>(true))
				{
					Objectification objectification;
					if (!element.IsImplicitBooleanValue && (null == (objectification = element.Objectification) || !objectification.IsImplied))
					{
						yield return element;
					}
				}

				foreach (SetConstraint element in elementDirectory.FindElements<SetConstraint>(true))
				{
					IConstraint constraint = (IConstraint)element;
					if (!constraint.ConstraintIsInternal && constraint.ConstraintType != ConstraintType.ImpliedMandatory)
					{
						yield return element;
					}
				}

				foreach (SetComparisonConstraint element in elementDirectory.FindElements<SetComparisonConstraint>(true))
				{
					yield return element;
				}

				foreach (NameGenerator element in elementDirectory.FindElements<NameGenerator>(false))
				{
					if (element.RefinesGenerator == null)
					{
						yield return element;
					}
				}
			}
			else if (expansionKey == FactType.SurveyExpansionKey)
			{
				FactType factType = context as FactType;
				if (factType != null)
				{
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						Role role;
						ObjectType player;
						if (null != (role = roleBase as Role) &&
							null != (player = role.RolePlayer) &&
							player.IsImplicitBooleanValue)
						{
							continue;
						}
						yield return roleBase;
					}
					foreach (SetConstraint element in factType.GetInternalConstraints<SetConstraint>())
					{
						yield return element;
					}
					Objectification objectification = factType.Objectification;
					if (objectification != null)
					{
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							yield return impliedFactType;
						}
					}
				}
			}
			else if (expansionKey == NameGenerator.SurveyExpansionKey)
			{
				foreach (NameGenerator refinement in ((NameGenerator)context).RefinedByGeneratorCollection)
				{
					yield return refinement;
				}
			}
			else if (expansionKey == FactConstraint.SurveyConstraintExpansionKey)
			{
				foreach (FactConstraint constraint in FactConstraint.GetLinksToFactTypeCollection((ORMNamedElement)context))
				{
					yield return constraint;
				}
			}
		}
		#endregion // ISurveyNodeProvider Implementation
		#region SurveyEventHandling
		/// <summary>
		/// Standard handler when an element needs to be removed from the model browser
		/// </summary>
		private static void ModelElementRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element);
			}
		}
		/// <summary>
		/// Standard handling for survey glyph changes on an element
		/// </summary>
		private static void SurveyGlyphChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementChanged(element, SurveyGlyphQuestionTypes);
			}
		}
		/// <summary>
		/// Standard survey handler for element rename
		/// </summary>
		private static void ModelElementNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
			}
		}
		/// <summary>
		/// Handler to update role name display when an <see cref="ObjectType"/> name changes
		/// </summary>
		private static void ObjectTypeNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				// Note that the primary change notification is handled by ModelElementNameChanged
				foreach (Role role in ((ObjectType)element).PlayedRoleCollection)
				{
					eventNotify.ElementRenamed(role);
				}
			}
		}
		/// <summary>
		/// Survey event handler for addition of an <see cref="ObjectType"/>
		/// </summary>
		private static void ObjectTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectType objectType = (ObjectType)element;
				Objectification objectification;
				if (!objectType.IsImplicitBooleanValue && (null == (objectification = objectType.Objectification) || !objectification.IsImplied))
				{
					eventNotify.ElementAdded(objectType, null);
				}
			}
		}
		/// <summary>
		/// Survey event handler for changes to a <see cref="Role"/>
		/// </summary>
		private static void RoleNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Role role = (Role)element;
				eventNotify.ElementRenamed(role);
				RoleProxy proxy = role.Proxy;
				if (proxy != null)
				{
					eventNotify.ElementRenamed(proxy);
				}
			}
		}
		/// <summary>
		/// Survey event handler for addition of a <see cref="FactType"/>
		/// </summary>
		private static void FactTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = (FactType)element;
				Objectification objectification = factType.ImpliedByObjectification;
				eventNotify.ElementAdded(factType, (objectification != null) ? objectification.NestedFactType : null);
			}
		}
		/// <summary>
		/// Survey event handler for addition of a <see cref="SetConstraint"/>
		/// </summary>
		private static void SetConstraintAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				SetConstraint constraint = (SetConstraint)element;
				//do not add mandatory constraint if it's part of ExclusiveOr
				switch (((IConstraint)constraint).ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
					case ConstraintType.InternalUniqueness:
						LinkedElementCollection<FactType> factTypes = constraint.FactTypeCollection;
						if (factTypes.Count == 1)
						{
							// Add as a detail on the FactType, not the main list
							eventNotify.ElementAdded(constraint, factTypes[0]);
						}
						return;
					case ConstraintType.DisjunctiveMandatory:
						if ((constraint as MandatoryConstraint).ExclusiveOrExclusionConstraint != null)
						{
							return;
						}
						break;
					case ConstraintType.ImpliedMandatory:
						return;
				}
				eventNotify.ElementAdded(constraint, null);
			}
		}
		/// <summary>
		/// Survey event handler for addition of a <see cref="SetComparisonConstraint"/>
		/// </summary>
		private static void SetComparisonConstraintAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusionConstraint exclusion;
				//do not add the exclusion constraint if its part of ExclusiveOr. 
				if (null != (exclusion = element as ExclusionConstraint) && null != exclusion.ExclusiveOrMandatoryConstraint)
				{
					return;
				}
				eventNotify.ElementAdded(element, null);
			}
		}
		/// <summary>
		/// Survey event handler for addition of a <see cref="FactConstraint"/>
		/// </summary>
		private static void FactConstraintAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactConstraint link = (FactConstraint)element;
				IConstraint constraint = link.Constraint as IConstraint;
				if (constraint != null && !constraint.ConstraintIsInternal && !constraint.ConstraintIsImplied)
				{
					eventNotify.ElementAdded(link, constraint);
				}
			}
		}
		/// <summary>
		/// Survey event handler for deletion of a <see cref="FactConstraint"/>
		/// </summary>
		private static void FactConstraintDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactConstraint link = (FactConstraint)element;
				IConstraint constraint = link.Constraint as IConstraint;
				FactType factType = link.FactType;
				if (!factType.IsDeleted &&
					null != (constraint = link.Constraint as IConstraint) &&
					!constraint.ConstraintIsInternal &&
					!constraint.ConstraintIsImplied)
				{
					eventNotify.ElementReferenceDeleted(factType, link, constraint);
				}
			}
		}
		/// <summary>
		/// Survey event handler for adding a <see cref="Role"/> to a <see cref="FactType"/>
		/// </summary>
		private static void FactTypeHasRoleAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactTypeHasRole link = (FactTypeHasRole)element;
				FactType factType = link.FactType;
				eventNotify.ElementChanged(factType, typeof(SurveyQuestionGlyph));
				Role role = link.Role as Role;
				if (role != null) // ProxyRole is only added as part of an implicit fact type, don't notify separately
				{
					eventNotify.ElementAdded(role, factType);
				}
				foreach (RoleBase displayedRole in factType.RoleCollection)
				{
					if (displayedRole != role)
					{
						eventNotify.ElementCustomSortChanged(displayedRole);
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for deleting a <see cref="Role"/> from a <see cref="FactType"/>
		/// </summary>
		private static void FactTypeHasRoleDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactTypeHasRole link = (FactTypeHasRole)element;
				FactType factType = link.FactType;
				if (!factType.IsDeleted)
				{
					eventNotify.ElementChanged(factType, SurveyGlyphQuestionTypes);
				}
				RoleBase role = link.Role;
				if (role != null)
				{
					eventNotify.ElementDeleted(role);
				}
				foreach (RoleBase displayedRole in factType.RoleCollection)
				{
					if (displayedRole != role)
					{
						eventNotify.ElementCustomSortChanged(displayedRole);
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for addition of an <see cref="Objectification"/> relationship
		/// </summary>
		private static void ObjectificationAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Objectification objectification = (Objectification)element;
				if (!objectification.IsImplied)
				{
					ObjectType nestingType = objectification.NestingType;
					eventNotify.ElementChanged(nestingType, SurveyGlyphQuestionTypes);
					foreach (Role role in nestingType.PlayedRoleCollection)
					{
						eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
					}
					eventNotify.ElementChanged(objectification.NestedFactType, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for deletion of an <see cref="Objectification"/> relationship
		/// </summary>
		private static void ObjectificationDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Objectification objectification = (Objectification)element;
				ObjectType nestingType = objectification.NestingType;
				if (!objectification.IsImplied)
				{
					FactType nestedFactType = objectification.NestedFactType;
					if (!nestingType.IsDeleted)
					{
						eventNotify.ElementChanged(nestingType, SurveyGlyphQuestionTypes);
						foreach (Role role in nestingType.PlayedRoleCollection)
						{
							eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						}
					}
					if (!nestedFactType.IsDeleted)
					{
						eventNotify.ElementChanged(nestedFactType, SurveyGlyphQuestionTypes);
					}
				}
				else if (!nestingType.IsDeleted)
				{
					eventNotify.ElementAdded(nestingType, null);
				}
			}
		}
		/// <summary>
		/// Survey event handler when a role player is added. Handles conversion
		/// of a non-unary <see cref="FactType"/> to a unary FactType and role updates.
		/// </summary>
		private static void RolePlayerAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)element;
				Role role = link.PlayedRole;
				if (!role.IsDeleted)
				{
					if (link.RolePlayer.IsImplicitBooleanValue)
					{
						eventNotify.ElementDeleted(role);
					}
					else
					{
						eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						eventNotify.ElementRenamed(role);
					}
				}

				if (link.RolePlayer.IsImplicitBooleanValue && !(role = link.PlayedRole).IsDeleted)
				{
					eventNotify.ElementDeleted(role);
				}
			}
		}
		/// <summary>
		/// Survey event handler when a role player is deleted. Handles conversion
		/// of a unary <see cref="FactType"/> to a non-unary FactType and role display updates.
		/// </summary>
		private static void RolePlayerDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)element;
				Role role = link.PlayedRole;
				if (!role.IsDeleted)
				{
					if (link.RolePlayer.IsImplicitBooleanValue)
					{
						eventNotify.ElementAdded(role, role.FactType);
					}
					else
					{
						eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						eventNotify.ElementRenamed(role);
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for a role player change
		/// </summary>
		private static void RolePlayerRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			ModelElement element = e.ElementLink;
			INotifySurveyElementChanged eventNotify;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)element;
				Role role = link.PlayedRole;
				if (!role.IsDeleted)
				{
					NotifyRoleChanged(eventNotify, role);
				}
				if (e.DomainRole.Id == ObjectTypePlaysRole.PlayedRoleDomainRoleId &&
					!(role = (Role)e.OldRolePlayer).IsDeleted)
				{
					NotifyRoleChanged(eventNotify, role);
				}
			}
		}
		private static void NotifyRoleChanged(INotifySurveyElementChanged eventNotify, Role role)
		{
			foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
			{
				UniquenessConstraint uniquenessConstraint;
				EntityTypeHasPreferredIdentifier identifierLink;
				if (null != (uniquenessConstraint = sequence as UniquenessConstraint) &&
					null != (identifierLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifierFor(uniquenessConstraint)))
				{
					NotifyErrorStateChanged(eventNotify, identifierLink);
				}
			}
			eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
			eventNotify.ElementRenamed(role);
		}
		/// <summary>
		/// Verify that error glyphs for the remote entity type update on role player changes
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element;
			if (null != (eventNotify = ((element = e.ElementLink).Store as IORMToolServices).NotifySurveyElementChanged))
			{
				NotifyErrorStateChanged(eventNotify, element as IModelErrorOwnerPath);
				ObjectType objectType;
				if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId &&
					!(objectType = (ObjectType)e.OldRolePlayer).IsDeleted)
				{
					eventNotify.ElementChanged(objectType, SurveyErrorQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for a change in the <see cref="Objectification.IsImplied"/> property
		/// </summary>
		private static void ObjectificationChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Objectification objectification = (Objectification)element;
				ObjectType nestingType = objectification.NestingType;
				FactType nestedFactType = objectification.NestedFactType;
				if (!nestingType.IsDeleted)
				{
					if (objectification.IsImplied)
					{
						eventNotify.ElementDeleted(nestingType);
					}
					else
					{
						eventNotify.ElementAdded(nestingType, null);
						foreach (Role role in nestingType.PlayedRoleCollection)
						{
							eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						}
					}
				}
				if (!nestedFactType.IsDeleted)
				{
					eventNotify.ElementChanged(nestedFactType, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for a change in the <see cref="Objectification"/> <see cref="ObjectType"/>
		/// </summary>
		private static void ObjectificationRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			ModelElement rolePlayer = e.NewRolePlayer;
			INotifySurveyElementChanged eventNotify;
			if (null != (eventNotify = (rolePlayer.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				bool nestingTypeChanged = e.DomainRole.Id == Objectification.NestingTypeDomainRoleId;
				// We notify the same for both the NestedFactType and NestingType roles
				if (!rolePlayer.IsDeleted)
				{
					eventNotify.ElementChanged(rolePlayer, SurveyGlyphQuestionTypes);
					if (nestingTypeChanged)
					{
						foreach (Role role in ((ObjectType)rolePlayer).PlayedRoleCollection)
						{
							eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						}
					}
				}
				if (!(rolePlayer = e.OldRolePlayer).IsDeleted)
				{
					eventNotify.ElementChanged(rolePlayer, SurveyGlyphQuestionTypes);
					if (nestingTypeChanged)
					{
						foreach (Role role in ((ObjectType)rolePlayer).PlayedRoleCollection)
						{
							eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
						}
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for adding a datatype
		/// </summary>
		private static void ValueTypeHasDataTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectType objectType = ((ValueTypeHasDataType)element).ValueType;
				if (!objectType.IsDeleted)
				{
					eventNotify.ElementChanged(objectType, SurveyGlyphQuestionTypes);
					foreach (Role role in objectType.PlayedRoleCollection)
					{
						eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for deleting a datatype
		/// </summary>
		private static void ValueTypeHasDataTypeDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectType objectType = ((ValueTypeHasDataType)e.ModelElement).ValueType;
				if (!objectType.IsDeleted)
				{
					eventNotify.ElementChanged(objectType, SurveyGlyphQuestionTypes);
					foreach (Role role in objectType.PlayedRoleCollection)
					{
						eventNotify.ElementChanged(role, SurveyGlyphQuestionTypes);
					}
				}
			}
		}
		/// <summary>
		/// Survey event handler for adding an <see cref="ExclusiveOrConstraintCoupler"/>
		/// </summary>
		private static void ExclusiveOrAddedEvent(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement as ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusiveOrConstraintCoupler coupler = element as ExclusiveOrConstraintCoupler;
				eventNotify.ElementAdded(coupler.ExclusionConstraint, null);
				eventNotify.ElementDeleted(coupler.MandatoryConstraint);
				eventNotify.ElementChanged(coupler.ExclusionConstraint, SurveyGlyphQuestionTypes);
			}
		}
		/// <summary>
		/// Survey event handler for deleting a <see cref="ExclusiveOrConstraintCoupler"/>
		/// </summary>
		private static void ExclusiveOrDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement as ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusiveOrConstraintCoupler coupler = element as ExclusiveOrConstraintCoupler;
				if (!coupler.ExclusionConstraint.IsDeleted)
				{
					eventNotify.ElementAdded(coupler.MandatoryConstraint, null);
					eventNotify.ElementChanged(coupler.ExclusionConstraint, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for changes to a <see cref="ModelElement">ModelElement</see>'s error state
		/// </summary>
		private static void ModelElementErrorStateChangedEvent(object sender, ElementEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element;
			if (null != (eventNotify = ((element = e.ModelElement).Store as IORMToolServices).NotifySurveyElementChanged))
			{
				NotifyErrorStateChanged(eventNotify, element as IModelErrorOwnerPath);
			}
		}
		private static void NotifyErrorStateChanged(INotifySurveyElementChanged eventNotify, IModelErrorOwnerPath errorPath)
		{
			if (errorPath != null)
			{
				ModelError.WalkAssociatedElements(errorPath.ErrorOwnerRolePlayer,
					delegate(ModelElement associatedElement)
					{
						eventNotify.ElementChanged(associatedElement, SurveyErrorQuestionTypes);
					});
			}
		}
		#endregion //SurveyEventHandling
	}
	#region IModelErrorOwner Implementations
	partial class FactTypeHasFactTypeInstance : IModelErrorOwnerPath
	{
		#region IModelErrorOwnerPath Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwnerPath.ErrorOwnerRolePlayer"/>
		/// </summary>
		protected ModelElement ErrorOwnerRolePlayer
		{
			get
			{
				return FactType;
			}
		}
		ModelElement IModelErrorOwnerPath.ErrorOwnerRolePlayer
		{
			get
			{
				return ErrorOwnerRolePlayer;
			}
		}
		#endregion // IModelErrorOwnerPath Implementation
	}
	partial class ObjectTypeHasObjectTypeInstance : IModelErrorOwnerPath
	{
		#region IModelErrorOwnerPath Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwnerPath.ErrorOwnerRolePlayer"/>
		/// </summary>
		protected ModelElement ErrorOwnerRolePlayer
		{
			get
			{
				return ObjectType;
			}
		}
		ModelElement IModelErrorOwnerPath.ErrorOwnerRolePlayer
		{
			get
			{
				return ErrorOwnerRolePlayer;
			}
		}
		#endregion // IModelErrorOwnerPath Implementation
	}
	partial class EntityTypeHasPreferredIdentifier : IModelErrorOwnerPath
	{
		#region IModelErrorOwnerPath Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwnerPath.ErrorOwnerRolePlayer"/>
		/// </summary>
		protected ModelElement ErrorOwnerRolePlayer
		{
			get
			{
				return PreferredIdentifierFor;
			}
		}
		ModelElement IModelErrorOwnerPath.ErrorOwnerRolePlayer
		{
			get
			{
				return ErrorOwnerRolePlayer;
			}
		}
		#endregion // IModelErrorOwnerPath Implementation
	}
	#endregion // IModelErrorOwner Implementations
}
