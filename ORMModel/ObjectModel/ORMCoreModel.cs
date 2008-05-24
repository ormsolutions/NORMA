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
				EventHandler<ElementDeletedEventArgs> standardDeleteHandler = new EventHandler<ElementDeletedEventArgs>(ModelElementRemoved);
				EventHandler<ElementPropertyChangedEventArgs> standardGlyphChangeHandler = new EventHandler<ElementPropertyChangedEventArgs>(SurveyGlyphChanged);
				//Object Type
				DomainClassInfo classInfo = directory.FindDomainClass(ObjectType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectTypeAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Fact Type
				classInfo = directory.FindDomainClass(FactType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Set Constraint
				classInfo = directory.FindDomainClass(SetConstraint.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetConstraintAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Set Comparison
				classInfo = directory.FindDomainClass(SetComparisonConstraint.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetComparisonConstraintAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, standardDeleteHandler, action);

				//Track name change
				EventHandler<ElementPropertyChangedEventArgs> standardNameChangedHandler = new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged);
				DomainPropertyInfo propertyInfo = directory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardNameChangedHandler , action);
				propertyInfo = directory.FindDomainProperty(FactType.NameChangedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, standardNameChangedHandler, action);

				//FactTypeHasRole
				classInfo = directory.FindDomainClass(FactTypeHasRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleDeleted), action);

				//Role
				propertyInfo = directory.FindDomainProperty(Role.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(RoleNameChanged), action);

				//ValueTypeHasDataType
				classInfo = directory.FindDomainClass(ValueTypeHasDataType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ValueTypeHasDataTypeAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ValueTypeHasDataTypeDeleted), action);

				//Objectification
				classInfo = directory.FindDomainClass(Objectification.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectificationAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectificationDeleted), action);
				propertyInfo = directory.FindDomainProperty(Objectification.IsImpliedDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectificationChanged), action);

				//Unary binarization
				classInfo = directory.FindDomainClass(ObjectTypePlaysRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ImplicitUnaryBooleanValueAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ImplicitUnaryBooleanValueDeleted), action);

				//RolePlayerChanged
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationRolePlayerChanged), action);

				//Error state changed
				classInfo = directory.FindDomainRelationship(ElementAssociatedWithModelError.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementErrorStateChanged), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ModelElementErrorStateChanged), action);

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

				//ExclusiveOr added deleted 
				classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ExclusiveOrAdded), action);
				classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ExclusiveOrDeleted), action);

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
		}
		#endregion // ISurveyNodeProvider Implementation
		#region SurveyEventHandling
		/// <summary>
		/// Standard handler when an element needs to be removed from the model browser
		/// </summary>
		protected void ModelElementRemoved(object sender, ElementDeletedEventArgs e)
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
		protected void SurveyGlyphChanged(object sender, ElementPropertyChangedEventArgs e)
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
		protected void ModelElementNameChanged(object sender, ElementPropertyChangedEventArgs e)
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
		/// Survey event handler for addition of an <see cref="ObjectType"/>
		/// </summary>
		protected void ObjectTypeAdded(object sender, ElementAddedEventArgs e)
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
		protected void RoleNameChanged(object sender, ElementPropertyChangedEventArgs e)
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
		protected void FactTypeAdded(object sender, ElementAddedEventArgs e)
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
		protected void SetConstraintAdded(object sender, ElementAddedEventArgs e)
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
		protected void SetComparisonConstraintAdded(object sender, ElementAddedEventArgs e)
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
		/// Survey event handler for adding a <see cref="Role"/> to a <see cref="FactType"/>
		/// </summary>
		protected void FactTypeHasRoleAdded(object sender, ElementAddedEventArgs e)
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
		protected void FactTypeHasRoleDeleted(object sender, ElementDeletedEventArgs e)
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
		protected void ObjectificationAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Objectification objectification = (Objectification)element;
				if (!objectification.IsImplied)
				{
					eventNotify.ElementChanged(objectification.NestingType, SurveyGlyphQuestionTypes);
					eventNotify.ElementChanged(objectification.NestedFactType, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for deletion of an <see cref="Objectification"/> relationship
		/// </summary>
		protected void ObjectificationDeleted(object sender, ElementDeletedEventArgs e)
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
		/// Survey event handler for conversion of a non-unary <see cref="FactType"/> to a unary FactType
		/// </summary>
		protected void ImplicitUnaryBooleanValueAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)element;
				Role role;
				if (link.RolePlayer.IsImplicitBooleanValue && !(role = link.PlayedRole).IsDeleted)
				{
					eventNotify.ElementDeleted(role);
				}
			}
		}
		/// <summary>
		/// Survey event handler for conversion of a unary <see cref="FactType"/> to a non-unary FactType
		/// </summary>
		protected void ImplicitUnaryBooleanValueDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)element;
				Role role;
				if (link.RolePlayer.IsImplicitBooleanValue && !(role = link.PlayedRole).IsDeleted)
				{
					eventNotify.ElementAdded(role, role.FactType);
				}
			}
		}
		/// <summary>
		/// Survey event handler for a change in the <see cref="Objectification.IsImplied"/> property
		/// </summary>
		protected void ObjectificationChanged(object sender, ElementPropertyChangedEventArgs e)
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
		protected void ObjectificationRolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
		{
			ModelElement rolePlayer = e.NewRolePlayer;
			INotifySurveyElementChanged eventNotify;
			if (null != (eventNotify = (rolePlayer.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				// We notify the same for both the NestedFactType and NestingType roles
				if (!rolePlayer.IsDeleted)
				{
					eventNotify.ElementChanged(rolePlayer, SurveyGlyphQuestionTypes);
				}
				if (!(rolePlayer = e.OldRolePlayer).IsDeleted)
				{
					eventNotify.ElementChanged(rolePlayer, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for adding a datatype
		/// </summary>
		protected void ValueTypeHasDataTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectType objectType = ((ValueTypeHasDataType)element).ValueType;
				if (!objectType.IsDeleted)
				{
					eventNotify.ElementChanged(objectType, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for deleting a datatype
		/// </summary>
		protected void ValueTypeHasDataTypeDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ObjectType objectType = ((ValueTypeHasDataType)e.ModelElement).ValueType;
				if (!objectType.IsDeleted)
				{
					eventNotify.ElementChanged(objectType, SurveyGlyphQuestionTypes);
				}
			}
		}
		/// <summary>
		/// Survey event handler for adding an <see cref="ExclusiveOrConstraintCoupler"/>
		/// </summary>
		protected void ExclusiveOrAdded(object sender, ElementAddedEventArgs e)
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
		protected void ExclusiveOrDeleted(object sender, ElementDeletedEventArgs e)
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
		protected static void ModelElementErrorStateChanged(object sender, ElementEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element;
			if (null != (eventNotify = ((element = e.ModelElement).Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelError.WalkAssociatedElements((element as ElementAssociatedWithModelError).AssociatedElement,
					delegate(ModelElement associatedElement)
					{
						eventNotify.ElementChanged(associatedElement, SurveyErrorQuestionTypes);
					});
			}
		}
		#endregion //SurveyEventHandling
	}
}
