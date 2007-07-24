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
		private static Type[] errorQuestionTypes = new Type[] { typeof(SurveyErrorState) };
		private static Type[] questionTypes = new Type[] { typeof(SurveyQuestionGlyph) };
		/// <summary>
		/// The unique name the VerbalizationBrowser target. Used in the Xml files and in code to identify the core target provider.
		/// </summary>
		public const string VerbalizationTargetName = "VerbalizationBrowser";
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManagePreLoadModelingEventHandlers"/>.
		/// This implementation does nothing and does not need to be called.
		/// </summary>
		void IModelingEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
		}
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManagePostLoadModelingEventHandlers"/>.
		/// </summary>
		protected void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			NamedElementDictionary.ManageEventHandlers(Store, eventManager, action);
		}
		void IModelingEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			this.ManagePostLoadModelingEventHandlers(eventManager, action);
		}
		/// <summary>
		/// Implementes <see cref="IModelingEventSubscriber.ManageSurveyQuestionModelingEventHandlers"/>.
		/// </summary>
		protected void ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory directory = this.Store.DomainDataDirectory;
			//Object Type
			DomainClassInfo classInfo = directory.FindDomainRelationship(ModelHasObjectType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ModelElementRemoved), action);

			//Fact Type
			classInfo = directory.FindDomainClass(ModelHasFactType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemoved), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeAdded), action);

			//Set Constraint
			classInfo = directory.FindDomainClass(ModelHasSetConstraint.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetConstraintAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(SetConstraintDeleted), action);

			//Set Comparison
			classInfo = directory.FindDomainClass(ModelHasSetComparisonConstraint.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(SetComparisonConstraintAdded), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(SetComparisonConstraintDeleted), action);

			//Track name change
			DomainPropertyInfo propertyInfo = directory.FindDomainProperty(ORMNamedElement.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged), action);
			propertyInfo = directory.FindDomainProperty(FactType.NameChangedDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ModelElementNameChanged), action);

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

			//RolePlayerChanged
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ObjectificationRolePlayerChanged), action);

			//Error state changed
			//classInfo = directory.FindDomainClass(ElementLink.DomainClassId);
			//eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(PotentialIndirectModelErrorLinkDeleted), action);
			classInfo = directory.FindDomainRelationship(ElementAssociatedWithModelError.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ModelElementErrorStateChanged), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ModelElementErrorStateChanged), action);

			//ModalityChanged
			DomainPropertyInfo info = directory.FindDomainProperty(SetConstraint.ModalityDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(ModalityChanged), action);
			info = directory.FindDomainProperty(SetComparisonConstraint.ModalityDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(ModalityChanged), action);

			//RingType changed
			info = directory.FindDomainProperty(RingConstraint.RingTypeDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(RingTypeChanged), action);

			//RingType changed
			info = directory.FindDomainProperty(UniquenessConstraint.IsPreferredDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(IsPreferredChanged), action);
			//ExclusiveOr added deleted 
			classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ExclusiveOrAdded), action);
			classInfo = directory.FindDomainClass(ExclusiveOrConstraintCoupler.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ExclusiveOrDeleted), action);

			//SubType
			info = directory.FindDomainProperty(SubtypeFact.IsPrimaryDomainPropertyId);
			eventManager.AddOrRemoveHandler(info, new EventHandler<ElementPropertyChangedEventArgs>(SubtypeFactIsPrimaryChanged), action);
		}

		void IModelingEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action)
		{
			this.ManageSurveyQuestionModelingEventHandlers(eventManager, action);
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
					yield return element;
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
			}
			else if (expansionKey == FactType.SurveyExpansionKey)
			{
				FactType factType = context as FactType;
				if (factType != null)
				{
					foreach (RoleBase role in factType.RoleCollection)
					{
						yield return role;
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
		}
		#endregion // ISurveyNodeProvider Implementation
		#region SurveyEventHandling
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementAdded events
		/// </summary>
		protected void ModelElementAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				eventNotify.ElementAdded(link.ObjectType, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementDeleted events
		/// </summary>
		protected void ModelElementRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				eventNotify.ElementDeleted(link.ObjectType);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for ElementPropertyChanged events
		/// </summary>
		protected void ModelElementNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted && null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementRenamed(element);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for changes to a Role
		/// </summary>
		protected void RoleNameChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (!element.IsDeleted && null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
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
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void FactTypeRemoved(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasFactType element = e.ModelElement as ModelHasFactType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.FactType);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void FactTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasFactType element = e.ModelElement as ModelHasFactType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				Objectification objectification = factType.ImpliedByObjectification;
				eventNotify.ElementAdded(factType, (objectification != null) ? objectification.NestedFactType : null);
			}
		}
		/// <summary>
		/// Set Constraint added
		/// </summary>
		protected void SetConstraintAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			SetConstraint element = (e.ModelElement as ModelHasSetConstraint).SetConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				//do not add mandatory constraint if it's part of ExclusiveOr
				switch (((IConstraint)element).ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
					case ConstraintType.InternalUniqueness:
						LinkedElementCollection<FactType> factTypes = element.FactTypeCollection;
						if (factTypes.Count == 1)
						{
							// Add as a detail on the FactType, not the main list
							eventNotify.ElementAdded(element, factTypes[0]);
						}
						return;
					case ConstraintType.DisjunctiveMandatory:
						if ((element as MandatoryConstraint).ExclusiveOrExclusionConstraint != null)
						{
							return;
						}
						break;
					case ConstraintType.ImpliedMandatory:
						return;
				}
				eventNotify.ElementAdded(element, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void SetConstraintDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasSetConstraint element = e.ModelElement as ModelHasSetConstraint;
			MandatoryConstraint mandatoryConstraint;
			if ((null == (mandatoryConstraint = element.SetConstraint as MandatoryConstraint) ||
				!mandatoryConstraint.IsImplied) &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.SetConstraint);
			}
		}
		/// <summary>
		/// Set Comparison Constraint added
		/// </summary>
		protected void SetComparisonConstraintAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			SetComparisonConstraint element = (e.ModelElement as ModelHasSetComparisonConstraint).SetComparisonConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				//do not add the exclusion constraint if its part of ExclusiveOr. 
				if (null != (element as ExclusionConstraint) && null != ((element as ExclusionConstraint).ExclusiveOrMandatoryConstraint))
				{
					return;
				}
				eventNotify.ElementAdded(element, null);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for FactType Name change events (custom events)
		/// </summary>
		protected void SetComparisonConstraintDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelHasSetComparisonConstraint element = e.ModelElement as ModelHasSetComparisonConstraint;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementDeleted(element.SetComparisonConstraint);
			}
		}
		/// <summary>
		/// Fact Type has role added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void FactTypeHasRoleAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			FactTypeHasRole element = e.ModelElement as FactTypeHasRole;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				eventNotify.ElementChanged(factType, typeof(SurveyQuestionGlyph));
				Role role = element.Role as Role;
				if (role != null) // ProxyRole is only added as part of an implicit fact type, don't notify separately
				{
					eventNotify.ElementAdded(role, factType);
				}
			}
		}
		/// <summary>
		/// Fact Type has role deleted
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void FactTypeHasRoleDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			FactTypeHasRole element = e.ModelElement as FactTypeHasRole;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				FactType factType = element.FactType;
				if (!factType.IsDeleted)
				{
					eventNotify.ElementChanged(factType, questionTypes);
				}
				RoleBase role = element.Role as RoleBase;
				if (role != null)
				{
					eventNotify.ElementDeleted(role);
				}
			}
		}
		/// <summary>
		/// Objectification added.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged)
				&& !element.IsImplied)
			{
				eventNotify.ElementChanged(element.NestingType, questionTypes);
				eventNotify.ElementChanged(element.NestedFactType, questionTypes);
			}
		}
		/// <summary>
		/// Objectification deleted.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged)
				&& !element.IsImplied)
			{
				ObjectType nestingType = element.NestingType;
				FactType nestedFactType = element.NestedFactType;
				if (!nestingType.IsDeleted)
				{
					eventNotify.ElementChanged(nestingType, questionTypes);
				}
				if (!nestedFactType.IsDeleted)
				{
					eventNotify.ElementChanged(nestedFactType, questionTypes);
				}
			}
		}
		/// <summary>
		/// Objectification property change.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			Objectification element = e.ModelElement as Objectification;
			if (!element.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == Objectification.IsImpliedDomainPropertyId)
				{
					ObjectType nestingType = element.NestingType;
					FactType nestedFactType = element.NestedFactType;
					if (!nestingType.IsDeleted)
					{
						eventNotify.ElementChanged(nestingType, questionTypes);
					}
					if (!nestedFactType.IsDeleted)
					{
						eventNotify.ElementChanged(nestedFactType, questionTypes);
					}
				}
			}
		}
		/// <summary>
		/// Objectifications role player changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RolePlayerChangedEventArgs"/> instance containing the event data.</param>
		protected void ObjectificationRolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
		{

			ObjectType newObjectType;
			ObjectType oldObjectType;
			ModelElement element = e.NewRolePlayer;
			INotifySurveyElementChanged eventNotify;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				if (null != (newObjectType = element as ObjectType))
				{
					oldObjectType = e.OldRolePlayer as ObjectType;
					eventNotify.ElementChanged(newObjectType, questionTypes);
					eventNotify.ElementChanged(oldObjectType, questionTypes);
				}
			}
		}
		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ValueTypeHasDataTypeAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ValueTypeHasDataType element = e.ModelElement as ValueTypeHasDataType;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementChanged(element.ValueType, questionTypes);
			}
		}
		/// <summary>
		/// ValueTypeHasDataType Deleted
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ValueTypeHasDataTypeDeleted(object sender, ElementDeletedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ValueTypeHasDataType element = e.ModelElement as ValueTypeHasDataType;
			ObjectType objectType = (e.ModelElement as ValueTypeHasDataType).ValueType;
			if (!objectType.IsDeleted &&
				null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				eventNotify.ElementChanged(objectType, questionTypes);
			}
		}
		/// <summary>
		/// Modality changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void ModalityChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				IConstraint constraint = element as IConstraint;
				eventNotify.ElementChanged(constraint, questionTypes);
			}
		}
		/// <summary>
		/// Ring type changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void RingTypeChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				RingConstraint ringConstraint = element as RingConstraint;
				eventNotify.ElementChanged(ringConstraint, questionTypes);
			}
		}
		/// <summary>
		/// External Uniqueness constraint IsPreferred property Changed
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void IsPreferredChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				UniquenessConstraint constraint = element as UniquenessConstraint;
				eventNotify.ElementChanged(constraint, questionTypes);
			}
		}
		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
		protected void ExclusiveOrAdded(object sender, ElementAddedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement as ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				ExclusiveOrConstraintCoupler coupler = element as ExclusiveOrConstraintCoupler;
				eventNotify.ElementAdded(coupler.ExclusionConstraint, null);
				eventNotify.ElementDeleted(coupler.MandatoryConstraint);
				eventNotify.ElementChanged(coupler.ExclusionConstraint, questionTypes);
			}
		}
		/// <summary>
		/// ValueTypeHasDataType added
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementAddedEventArgs"/> instance containing the event data.</param>
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
					eventNotify.ElementChanged(coupler.ExclusionConstraint, questionTypes);
				}
			}
		}
		/// <summary>
		/// SubType Fact IsPrimary property changed
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected void SubtypeFactIsPrimaryChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			INotifySurveyElementChanged eventNotify;
			ModelElement element = e.ModelElement;
			if (null != (eventNotify = (element.Store as IORMToolServices).NotifySurveyElementChanged))
			{
				SubtypeFact subTypeFact = element as SubtypeFact;
				eventNotify.ElementChanged(subTypeFact, questionTypes);
			}
		}
		/// <summary>
		/// wired on SurveyQuestionLoad as event handler for changes to a <see cref="ModelElement">ModelElement</see>'s error state
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
						eventNotify.ElementChanged(associatedElement, errorQuestionTypes);
					});
			}
		}
		#endregion //SurveyEventHandling
	}
}
