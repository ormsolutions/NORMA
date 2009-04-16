#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using System.Globalization;
using System.Resources;
using System.Drawing;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Reflection;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region GroupingTypeElementSupportLevel enum
	/// <summary>
	/// Values to indicate the level of support an <see cref="ElementGroupingType"/> instance
	/// offers for this element. The support level from all <see cref="ElementGroupingType"/> instances
	/// associated with a single <see cref="ElementGroupingType"/> are used (along with the
	/// <see cref="P:ElementGrouping.TypeCompliance"/> setting for the context <see cref="ElementGrouping"/>)
	/// to determine which elements are allowed in a group.
	/// </summary>
	public enum GroupingTypeElementSupportLevel
	{
		/// <summary>
		/// The <see cref="ElementGroupingType"/> does not have an opinion on the element.
		/// </summary>
		NotApplicable,
		/// <summary>
		/// The <see cref="ElementGroupingType"/> allows this element to be included in the group.
		/// </summary>
		Allowed,
		/// <summary>
		/// The <see cref="ElementGroupingType"/> automatically includes this element to be included in the group.
		/// An automatic element can be explicitly excluded to keep it out of the group.
		/// </summary>
		Automatic,
		/// <summary>
		/// The <see cref="ElementGroupingType"/> blocks this element from being included in the group.
		/// </summary>
		Blocked,
	}
	#endregion // GroupingTypeElementSupportLevel enum
	#region GroupingTypeFeatures enum
	/// <summary>
	/// Specify features for advance element grouping
	/// </summary>
	[Flags]
	public enum GroupingTypeFeatures
	{
		/// <summary>
		/// No advanced features
		/// </summary>
		None = 0,
		/// <summary>
		/// Enable the default group option, which
		/// states that all elements not in a group
		/// end up in the default group.
		/// </summary>
		DefaultGroup = 1,
		/// <summary>
		/// The <see cref="M:ElementGroupingType.GetElementSupportLevel"/>
		/// can return <see cref="F:GroupingTypeElementSupportLevel.Automatic"/> for one
		/// or more elements.
		/// </summary>
		AutomaticMembers = 2,
	}
	#endregion // GroupTypeFeatures enum
	#region GroupingMembershipType enum
	/// <summary>
	/// Specify if an element is not included, included, explicitly excluded,
	/// or in a contradictory state within a group.
	/// </summary>
	public enum GroupingMembershipType
	{
		/// <summary>
		/// The element is not linked
		/// </summary>
		None,
		/// <summary>
		/// The element is included
		/// </summary>
		Inclusion,
		/// <summary>
		/// The element is explicitly excluded
		/// </summary>
		Exclusion,
		/// <summary>
		/// The element is in a contradictory state, meaning that
		/// it is both automatically included and blocked by different
		/// <see cref="ElementGroupingType"/> instances associated with
		/// the <see cref="ElementGrouping"/>
		/// </summary>
		Contradiction,
	}
	#endregion // GroupingMembershipType enum
	#region GroupingMembershipInclusion enum
	/// <summary>
	/// Values determining if an element may be added to a group. Returned by
	/// the <see cref="ElementGrouping.GetElementInclusion"/> method
	/// </summary>
	public enum GroupingMembershipInclusion
	{
		/// <summary>
		/// The element is already included in the <see cref="ElementGrouping"/>
		/// </summary>
		Included,
		/// <summary>
		/// The element can be added to the <see cref="ElementGrouping"/>
		/// </summary>
		AddAllowed,
		/// <summary>
		/// The element may not be added to the <see cref="ElementGrouping"/>
		/// </summary>
		AddBlocked,
	}
	#endregion // GroupingMembershipInclusion enum
	#region ElementGroupingType class
	partial class ElementGroupingType
	{
		/// <summary>
		/// Determine the level of support by this group type for a requested element
		/// </summary>
		/// <param name="element">The <see cref="ModelElement"/> to test.</param>
		/// <returns></returns>
		public abstract GroupingTypeElementSupportLevel GetElementSupportLevel(ModelElement element);
		/// <summary>
		/// Set the feature support level for this type of group
		/// </summary>
		public virtual GroupingTypeFeatures SupportedFeatures
		{
			get
			{
				return GroupingTypeFeatures.None;
			}
		}
	}
	#endregion // ElementGroupingType class
	#region ElementGroupingTypeDisplayAttribute class
	/// <summary>
	/// Specifies how an <see cref="ElementGroupingType"/> instance of this type should be displayed in the Model Browser.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ElementGroupingTypeDisplayAttribute : Attribute
	{
		private string myBrowserImageResourceId;
		private ResourceManager myResourceManager;

		/// <summary>
		/// Retrieves the Glyph for the Model Browser.
		/// </summary>
		public Image BrowserImage
		{
			get
			{
				ResourceManager resourceManager = this.myResourceManager;
				if (resourceManager == null || string.IsNullOrEmpty(myBrowserImageResourceId))
				{
					return null;
				}
				else
				{
					return resourceManager.GetObject(myBrowserImageResourceId, CultureInfo.CurrentUICulture) as Image;
				}
			}
		}

		/// <summary>
		/// Determines how an Element Grouping Type. Supplies the Display Name and Glyph for model browser display
		/// </summary>
		/// <param name="resourceManagerSource">The type of class that the resource manager will target</param>
		/// <param name="browserImageResourceId">The resource that identifies the Glyph for <see cref="ElementGroupingType"/> instances of this type in the Model Browser</param>
		public ElementGroupingTypeDisplayAttribute(Type resourceManagerSource, string browserImageResourceId)
		{
			if (resourceManagerSource == null)
			{
				throw new ArgumentNullException("ResourceManagerSource");
			}
			myBrowserImageResourceId = browserImageResourceId;

			myResourceManager = typeof(ResourceAccessor<>).MakeGenericType(resourceManagerSource).InvokeMember("ResourceManager", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, null, null, null, CultureInfo.InvariantCulture, null) as ResourceManager;
		}
	}
	#endregion // ElemengGroupTypeDisplayAttribute class
	#region ElementGrouping class
	partial class ElementGrouping : IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ElementGroupingDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
				foreach (ElementGroupingMembershipContradictionError membershipError in MembershipContradictionErrorCollection)
				{
					yield return membershipError;
				}
			}

			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.ValidateErrors"/>
		/// </summary>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			ValidateGroupElements(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.DelayValidateErrors"/>
		/// </summary>
		protected new void DelayValidateErrors()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateGroupElements);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region CustomStorage Handling
		private void SetDefinitionTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Definition definition = Definition;
				if (definition != null)
				{
					definition.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Definition = new Definition(Store, new PropertyAssignment(Definition.TextDomainPropertyId, newValue));
				}
			}
		}
		private void SetNoteTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Note note = Note;
				if (note != null)
				{
					note.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Note = new Note(Store, new PropertyAssignment(Note.TextDomainPropertyId, newValue));
				}
			}
		}
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		#endregion // CustomStorage Handling
		#region Public Helper Methods
		/// <summary>
		/// Remove a <see cref="GroupingElementInclusion"/> relationship
		/// by either fully removing it from the group or by replacing it
		/// with a <see cref="GroupingElementExclusion"/> if the element
		/// is required. A current <see cref="Transaction"/> is assumed.
		/// </summary>
		public static void RemoveElement(GroupingElementInclusion inclusion)
		{
			if (!inclusion.IsDeleted)
			{
				ElementGrouping grouping = inclusion.Grouping;
				ModelElement element = inclusion.Element;
				inclusion.Delete();
				foreach (ElementGroupingType groupingType in grouping.GroupingTypeCollection)
				{
					if (0 != (groupingType.SupportedFeatures & GroupingTypeFeatures.AutomaticMembers))
					{
						if (GroupingTypeElementSupportLevel.Automatic == groupingType.GetElementSupportLevel(element))
						{
							new GroupingElementExclusion(grouping, element);
							break;
						}
					}
				}
			}
		}
		/// <summary>
		/// Remove a contradiction link for a group by transforming it into
		/// an <see cref="GroupingElementExclusion"/>.  A current <see cref="Transaction"/> is assumed.
		/// </summary>
		public static void RemoveElement(GroupingMembershipContradictionErrorIsForElement contradiction)
		{
			if (!contradiction.IsDeleted)
			{
				// A rule will give priority to the exclusion and automatically remove the contradiction
				new GroupingElementExclusion(contradiction.GroupingMembershipContradictionErrorRelationship.Grouping, contradiction.Element);
			}
		}
		/// <summary>
		/// Perform a remove action on an element referenced by this group.
		/// If the element is automatically included or in a contradictory state
		/// then the element remains tracked as an exclusion. This fails silently
		/// if the element cannot be removed.
		/// </summary>
		/// <param name="element">The element to remove</param>
		/// <param name="groupingTypes">The <see cref="ElementGroupingType"/> collection to test.
		/// Assumed to be <see cref="GroupingTypeCollection"/> if not provided. Used as an optimization
		/// for multiple calls.</param>
		public void RemoveGroupedElement(ModelElement element, IList<ElementGroupingType> groupingTypes)
		{
			GroupingElementInclusion inclusion;
			GroupingElementExclusion exclusion;
			ElementGroupingMembershipContradictionError contradiction;
			switch (GetExistingMembershipType(element, out inclusion, out exclusion, out contradiction))
			{
				case GroupingMembershipType.Inclusion:
					inclusion.Delete();
					foreach (ElementGroupingType groupingType in groupingTypes ?? GroupingTypeCollection)
					{
						if (0 != (groupingType.SupportedFeatures & GroupingTypeFeatures.AutomaticMembers))
						{
							if (GroupingTypeElementSupportLevel.Automatic == groupingType.GetElementSupportLevel(element))
							{
								new GroupingElementExclusion(this, element);
								break;
							}
						}
					}
					break;
				case GroupingMembershipType.Contradiction:
					new GroupingElementExclusion(this, element);
					break;
			}
		}
		/// <summary>
		/// Validation helper to integrate support level changes within an
		/// <see cref="ElementGrouping"/>. A support level change request is
		/// generally triggered by a single <see cref="ElementGroupingType"/>,
		/// but a single type cannot arbitrarily change the element membership
		/// type in a grouping. The correct membership level is calculated based
		/// on the entire <see cref="GroupingTypeCollection"/> and the current
		/// <see cref="TypeCompliance"/> setting.
		/// </summary>
		/// <param name="element">The element that is changed.</param>
		/// <param name="notifyAdded">Standard notify callback, set during deserialization.</param>
		public void ValidateSupportLevelChange(ModelElement element, INotifyElementAdded notifyAdded)
		{
			GroupingElementInclusion inclusion;
			GroupingElementExclusion exclusion;
			ElementGroupingMembershipContradictionError contradiction;
			GroupingMembershipType existingLinkType = GetExistingMembershipType(element, out inclusion, out exclusion, out contradiction);
			GroupingMembershipType requiredLinkType = GroupingMembershipType.None; // Interpret none as allowed but not their
			bool notApplicable;
			bool allowed;
			bool automatic;
			bool blocked;
			GetElementSupportLevels(GroupingTypeCollection, element, out notApplicable, out allowed, out automatic, out blocked);
			if (blocked)
			{
				requiredLinkType = automatic ? GroupingMembershipType.Contradiction : (GroupingMembershipType)(-1); // -1 = extra value indicating 'not allowed'
			}
			else
			{
				switch (TypeCompliance)
				{
					case GroupingMembershipTypeCompliance.NotExcluded:
						requiredLinkType = automatic ? GroupingMembershipType.Inclusion : GroupingMembershipType.None;
						break;
					case GroupingMembershipTypeCompliance.PartiallyApproved:
						requiredLinkType = automatic ?
							GroupingMembershipType.Inclusion :
							(allowed ? GroupingMembershipType.None : (GroupingMembershipType)(-1));
						break;
					case GroupingMembershipTypeCompliance.FullyApproved:
						requiredLinkType = (notApplicable || !allowed) ?
							(GroupingMembershipType)(-1) :
							(automatic ? GroupingMembershipType.Inclusion : GroupingMembershipType.None);
						break;
				}
			}

			bool createInclusion = false;
			switch (requiredLinkType)
			{
				case (GroupingMembershipType)(-1):
					switch (existingLinkType)
					{
						case GroupingMembershipType.Inclusion:
							inclusion.Delete();
							break;
						case GroupingMembershipType.Exclusion:
							exclusion.Delete();
							break;
						case GroupingMembershipType.Contradiction:
							contradiction.Delete();
							break;
					}
					break;
				case GroupingMembershipType.None:
					switch (existingLinkType)
					{
						//case GroupingElementLinkType.None: // Don't add
						//case GroupingElementLinkType.Inclusion: // Leave alone if there
						case GroupingMembershipType.Exclusion:
							exclusion.Delete();
							break;
						case GroupingMembershipType.Contradiction:
							// Remove the error, add the element
							contradiction.Delete();
							createInclusion = true;
							break;
					}
					break;
				case GroupingMembershipType.Inclusion:
					switch (existingLinkType)
					{
						case GroupingMembershipType.None:
							createInclusion = true;
							break;
						//case GroupingElementLinkType.Inclusion: // Already have it
						case GroupingMembershipType.Exclusion:
							exclusion.Delete();
							createInclusion = true;
							break;
						case GroupingMembershipType.Contradiction:
							contradiction.Delete();
							createInclusion = true;
							break;
					}
					break;
				//case GroupingElementLinkType.Exclusion: Exclusion is never required
				case GroupingMembershipType.Contradiction:
					if (existingLinkType != GroupingMembershipType.Contradiction)
					{
						switch (existingLinkType)
						{
							//case GroupingElementLinkType.None: // Just create
							case GroupingMembershipType.Inclusion:
								inclusion.Delete();
								break;
							case GroupingMembershipType.Exclusion:
								exclusion.Delete();
								break;
						}
						contradiction = new ElementGroupingMembershipContradictionError(this, element);
						contradiction.Model = GroupingSet.Model;
						contradiction.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(contradiction, true);
						}
					}
					break;
			}
			if (createInclusion)
			{
				inclusion = new GroupingElementInclusion(this, element);
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(inclusion, false);
				}
			}
		}
		/// <summary>
		/// Validation helper to integrate support level changes for a single type
		/// with other types in the <see cref="GroupingTypeCollection"/> and the
		/// current <see cref="TypeCompliance"/> setting.
		/// </summary>
		/// <param name="element">The element that is changed.</param>
		/// <param name="notifyAdded">Standard notify callback, set during deserialization.</param>
		public static void ValidateSupportLevelChange<GroupingType>(ModelElement element, INotifyElementAdded notifyAdded) where GroupingType : ElementGroupingType
		{
			if (!element.IsDeleted)
			{
				foreach (GroupingType modifiedGroupingType in element.Store.ElementDirectory.FindElements<GroupingType>(false))
				{
					modifiedGroupingType.Grouping.ValidateSupportLevelChange(element, notifyAdded);
				}
			}
		}
		#endregion // Public Helper Methods
		#region Rule methods
		/// <summary>
		/// ChangeRule: typeof(ElementGrouping)
		/// </summary>
		private static void GroupingChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == ElementGrouping.TypeComplianceDomainPropertyId)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateGroupElements);
			}
		}
		/// <summary>
		/// AddRule: typeof(ElementGroupingIsOfElementGroupingType)
		/// </summary>
		private static void GroupingTypeAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ElementGroupingIsOfElementGroupingType)e.ModelElement).Grouping, DelayValidateGroupElements);
		}
		/// <summary>
		/// DeleteRule: typeof(ElementGroupingIsOfElementGroupingType)
		/// </summary>
		private static void GroupingTypeDeletedRule(ElementDeletedEventArgs e)
		{
			ElementGrouping grouping = ((ElementGroupingIsOfElementGroupingType)e.ModelElement).Grouping;
			if (!grouping.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(grouping, DelayValidateGroupElements);
			}
		}
		/// <summary>
		/// AddRule: typeof(GroupingElementExclusion)
		/// </summary>
		private static void GroupingExclusionAddedRule(ElementAddedEventArgs e)
		{
			// Inclusion and Exclusion relationships are mutually because they are subtypes
			// of GroupingElementRelationship, but the membership contradiction model is also
			// meant to be exclusive with the other two relationships. The mutual exclusion
			// between the three relationships is assumed in multiple places, so we do not
			// wait until delayed validation to enfore this structure.
			GroupingElementExclusion exclusion = (GroupingElementExclusion)e.ModelElement;
			ElementGrouping grouping = exclusion.Grouping;
			ReadOnlyCollection<GroupingMembershipContradictionErrorIsForElement> contradictionLinks = GroupingMembershipContradictionErrorIsForElement.GetLinksToMembershipContradictionErrorCollection(exclusion.Element);
			ElementGroupingHasMembershipContradictionError groupingLink;
			for (int i = contradictionLinks.Count - 1; i >= 0; --i)
			{
				GroupingMembershipContradictionErrorIsForElement contradictionLink = contradictionLinks[i];
				if (null != (groupingLink = contradictionLink.GroupingMembershipContradictionErrorRelationship) &&
					groupingLink.Grouping == grouping)
				{
					groupingLink.MembershipContradictionError.Delete();
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(GroupingElementExclusion)
		/// </summary>
		private static void GroupingExclusionDeletedRule(ElementDeletedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateDeletedElementExclusion);
		}
		/// <summary>
		/// ChangeRule: typeof(ORMNamedElement)
		/// </summary>
		private static void StandardNamedElementNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ORMNamedElement.NameDomainPropertyId)
			{
				UpdateMembershipContradictionErrorText(e.ModelElement);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactType)
		/// </summary>
		private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == FactType.NameChangedDomainPropertyId)
			{
				UpdateMembershipContradictionErrorText(e.ModelElement);
			}
		}
		/// <summary>
		/// Helper function to update membership contradiction errors when an element name changes.
		/// <see cref="ElementGrouping"/> child relationships attached to a <see cref="ModelElement"/>, which
		/// does not itself have a Name property, so it is the responsibility of each named element to
		/// notify when names are update. The <see cref="ORMNamedElement"/> base class is handled natively.
		/// </summary>
		public static void UpdateMembershipContradictionErrorText(ModelElement element)
		{
			foreach (GroupingMembershipContradictionErrorIsForElement link in GroupingMembershipContradictionErrorIsForElement.GetLinksToMembershipContradictionErrorCollection(element))
			{
				link.GroupingMembershipContradictionErrorRelationship.MembershipContradictionError.GenerateErrorText();
			}
		}
		#endregion // Rule methods
		#region Delay Validation methods
		private static void DelayValidateGroupElements(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((ElementGrouping)element).ValidateGroupElements(null);
			}
		}
		/// <summary>
		/// Validate that all contents of the group satisfy the current grouping types and group settings
		/// </summary>
		private void ValidateGroupElements(INotifyElementAdded notifyAdded)
		{
			GroupingMembershipTypeCompliance typeCompliance = TypeCompliance;
			LinkedElementCollection<ElementGroupingType> types = GroupingTypeCollection;
			int typeCount = types.Count;

			// If the type compliance is stronger than the number of group types,
			// then we need to move it back to a reasonable level.
			if (typeCount == 0 && typeCompliance != GroupingMembershipTypeCompliance.NotExcluded)
			{
				TypeCompliance = GroupingMembershipTypeCompliance.NotExcluded;
				// Changing the TypeCompliance will retrigger an immediate call
				// to this method.
				return;
			}

			ReadOnlyCollection<GroupingElementInclusion> elementInclusionLinks = GroupingElementInclusion.GetLinksToIncludedElementCollection(this);
			LinkedElementCollection<ElementGroupingMembershipContradictionError> contradictionsError = MembershipContradictionErrorCollection;
			int startContradictionErrorCount = contradictionsError.Count;
			ORMModel model = null;
			Store store = Store;

			// State locals, set repeatedly
			bool notApplicable;
			bool allowed;
			bool blocked;
			bool automatic;
			GroupingElementInclusion elementInclusionLink;
			ModelElement testElement;
			ElementGroupingMembershipContradictionError contradictionError;

			// Test all included elements. If the element is no longer allowed then remove it.
			// If the element is both blocked and automatic then add a new contradiction error.
			for (int i = elementInclusionLinks.Count - 1; i >= 0; --i)
			{
				elementInclusionLink = elementInclusionLinks[i];
				testElement = elementInclusionLink.IncludedElement;
				GetElementSupportLevels(types, testElement, out notApplicable, out allowed, out automatic, out blocked);
				if (blocked && automatic)
				{
					// Error situation, remove the element and add a contradiction error
					elementInclusionLink.Delete();
					contradictionError = new ElementGroupingMembershipContradictionError(this, testElement);
					contradictionError.Model = model ?? (model = GroupingSet.Model);
					contradictionError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(contradictionError, true);
					}
				}
				else
				{
					bool deleteElement = false;
					switch (typeCompliance)
					{
						case GroupingMembershipTypeCompliance.NotExcluded:
							deleteElement = blocked;
							break;
						case GroupingMembershipTypeCompliance.PartiallyApproved:
							deleteElement = blocked || !allowed;
							break;
						case GroupingMembershipTypeCompliance.FullyApproved:
							deleteElement = blocked || !allowed || notApplicable;
							break;
					}
					if (deleteElement)
					{
						elementInclusionLink.Delete();
					}
				}
			}

			// Test element exclusions using a similar criteria. An item
			// should be excluded only if it is required by one of the
			// grouping types. Otherwise, it should be removed from the group
			ReadOnlyCollection<GroupingElementExclusion> elementExclusionLinks = GroupingElementExclusion.GetLinksToExcludedElementCollection(this);
			for (int i = elementExclusionLinks.Count - 1; i >= 0; --i)
			{
				GroupingElementExclusion elementExclusionLink = elementExclusionLinks[i];
				testElement = elementExclusionLink.ExcludedElement;
				GetElementSupportLevels(types, testElement, out notApplicable, out allowed, out automatic, out blocked);
				if (blocked && automatic)
				{
					// Error situation, remove the element and add a contradiction error
					// This is the same behavior as the element inclusions
					elementExclusionLink.Delete();
					contradictionError = new ElementGroupingMembershipContradictionError(this, testElement);
					contradictionError.Model = model ?? (model = GroupingSet.Model);
					contradictionError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(contradictionError, true);
					}
				}
				else if (!automatic)
				{
					elementExclusionLink.Delete();
				}
			}


			// Test existing contradiction errors. If they are still contradiction errors then keep them.
			// If the element is no longer in a contradictory state and the type compliance filter applies
			// then add it.
			for (int i = startContradictionErrorCount - 1; i >= 0; --i)
			{
				contradictionError = contradictionsError[i];
				testElement = contradictionError.Element;
				GetElementSupportLevels(types, testElement, out notApplicable, out allowed, out automatic, out blocked);
				if (!(blocked && automatic)) // Blocked and automatic indicates the error situation still exists
				{
					bool deleteElement = false;
					switch (typeCompliance)
					{
						case GroupingMembershipTypeCompliance.NotExcluded:
							deleteElement = blocked;
							break;
						case GroupingMembershipTypeCompliance.PartiallyApproved:
							deleteElement = blocked || !allowed;
							break;
						case GroupingMembershipTypeCompliance.FullyApproved:
							deleteElement = blocked || !allowed || notApplicable;
							break;
					}
					if (deleteElement)
					{
						// Don't need the element or the error
						contradictionError.Delete();
					}
					else
					{
						// There is no error condition and this is a known element, add it into the set
						contradictionError.Delete();
						elementInclusionLink = new GroupingElementInclusion(this, testElement);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(elementInclusionLink, false);
						}
					}
				}
			}
		}
		/// <summary>
		/// Verify that a deleted <see cref="GroupingElementExclusion"/> is replaced with either
		/// a <see cref="GroupingElementInclusion"/> or an <see cref="GroupingMembershipContradictionErrorIsForElement"/>
		/// relationship, or nothing at all if the grouping types have changed.
		/// </summary>
		private static void DelayValidateDeletedElementExclusion(ModelElement element)
		{
			GroupingElementExclusion link = (GroupingElementExclusion)element;
			ElementGrouping grouping;
			ModelElement referencedElement;
			if (!(grouping = link.Grouping).IsDeleted &&
				!(referencedElement = link.Element).IsDeleted &&
				GroupingMembershipType.None == grouping.GetMembershipType(referencedElement))
			{
				bool automatic = false;
				bool blocked = false;
				foreach (ElementGroupingType groupingType in grouping.GroupingTypeCollection)
				{
					switch (groupingType.GetElementSupportLevel(referencedElement))
					{
						case GroupingTypeElementSupportLevel.Automatic:
							automatic = true;
							break;
						case GroupingTypeElementSupportLevel.Blocked:
							blocked = true;
							break;
						default:
							continue;
					}
					if (automatic && blocked)
					{
						break;
					}
				}
				if (automatic)
				{
					if (blocked)
					{
						ElementGroupingMembershipContradictionError error = new ElementGroupingMembershipContradictionError(grouping, link.Element);
						error.Model = grouping.GroupingSet.Model;
						error.GenerateErrorText();
					}
					else
					{
						new GroupingElementInclusion(grouping, referencedElement);
					}
				}
			}
		}
		/// <summary>
		/// Test whether an element can be added to this <see cref="ElementGrouping"/>
		/// </summary>
		/// <param name="element">The element to test</param>
		/// <param name="groupingTypes">The <see cref="ElementGroupingType"/> collection to test.
		/// Assumed to be <see cref="GroupingTypeCollection"/> if not provided. Used as an optimization
		/// for multiple calls.</param>
		/// <returns><see cref="GroupingMembershipInclusion"/></returns>
		public GroupingMembershipInclusion GetElementInclusion(ModelElement element, IList<ElementGroupingType> groupingTypes)
		{
			GroupingMembershipInclusion retVal = GroupingMembershipInclusion.AddAllowed;
			switch (GetMembershipType(element))
			{
				case GroupingMembershipType.Inclusion:
					retVal = GroupingMembershipInclusion.Included;
					break;
				case GroupingMembershipType.Contradiction:
					// An exclusion is a user setting indicating that the element
					// can be added back into the set. A contradictory element
					// cannot be successfully added back in.
					retVal = GroupingMembershipInclusion.AddBlocked;
					break;
				//case ElementGroupingMembershipType.Exclusion:
					// If the element is excluded, then it could not
					// be removed because it is required to be in the
					// group. These can always be readded by deleting
					// the exclusion.
					// Note that we allow the add even it can result in
					// a contradiction. This allows a contradictory element
					// to be excluded and then readded without an undo
				//	break;
				case GroupingMembershipType.None:
					bool notApplicable;
					bool allowed;
					bool automatic;
					bool blocked;
					bool canAdd = true;
					GetElementSupportLevels(groupingTypes ?? GroupingTypeCollection, element, out notApplicable, out allowed, out automatic, out blocked);
					if (blocked && !automatic)
					{
						canAdd = false;
					}
					else
					{
						switch (TypeCompliance)
						{
							case GroupingMembershipTypeCompliance.NotExcluded:
								canAdd = allowed || notApplicable || !blocked;
								break;
							case GroupingMembershipTypeCompliance.PartiallyApproved:
								canAdd = allowed;
								break;
							case GroupingMembershipTypeCompliance.FullyApproved:
								canAdd = allowed && !notApplicable;
								break;
						}
					}
					if (!canAdd)
					{
						retVal = GroupingMembershipInclusion.AddBlocked;
					}
					break;
			}
			return retVal;
		}
		private GroupingMembershipType GetExistingMembershipType(ModelElement testElement, out GroupingElementInclusion inclusion, out GroupingElementExclusion exclusion, out ElementGroupingMembershipContradictionError contradiction)
		{
			GroupingElementRelationship elementRel;
			inclusion = null;
			exclusion = null;
			contradiction = null;
			if (null != (elementRel = GroupingElementRelationship.GetLink(this, testElement)))
			{
				if (null != (inclusion = elementRel as GroupingElementInclusion))
				{
					return GroupingMembershipType.Inclusion;
				}
				else
				{
					exclusion = (GroupingElementExclusion)elementRel;
					return GroupingMembershipType.Exclusion;
				}
			}
			foreach (ElementGroupingHasMembershipContradictionError errorLink in GroupingMembershipContradictionErrorIsForElement.GetMembershipContradictionErrorCollection(testElement))
			{
				if (errorLink.Grouping == this)
				{
					contradiction = errorLink.MembershipContradictionError;
					return GroupingMembershipType.Contradiction;
				}
			}
			return GroupingMembershipType.None;
		}
		/// <summary>
		/// Determine if an element is referenced by an <see cref="ElementGrouping"/>
		/// </summary>
		/// <param name="element">The element to test</param>
		/// <returns>The <see cref="GroupingMembershipType"/> indicating the </returns>
		public GroupingMembershipType GetMembershipType(ModelElement element)
		{
			GroupingElementRelationship groupingLink;
			if (null != (groupingLink = GroupingElementRelationship.GetLink(this, element)))
			{
				return groupingLink is GroupingElementInclusion ? GroupingMembershipType.Inclusion : GroupingMembershipType.Exclusion;
			}
			foreach (ElementGroupingHasMembershipContradictionError errorLink in GroupingMembershipContradictionErrorIsForElement.GetMembershipContradictionErrorCollection(element))
			{
				if (errorLink.Grouping == this)
				{
					return GroupingMembershipType.Contradiction;
				}
			}
			return GroupingMembershipType.None;
		}
		private static void GetElementSupportLevels(IList<ElementGroupingType> types, ModelElement testElement, out bool notApplicable, out bool allowed, out bool automatic, out bool blocked)
		{
			int typeCount = types.Count;
			notApplicable = typeCount == 0;
			blocked = false;
			automatic = false;
			allowed = false;
			for (int j = 0; j < typeCount; ++j)
			{
				switch (types[j].GetElementSupportLevel(testElement))
				{
					case GroupingTypeElementSupportLevel.NotApplicable:
						notApplicable = true;
						break;
					case GroupingTypeElementSupportLevel.Automatic:
						automatic = true;
						if (blocked)
						{
							break;
						}
						break;
					case GroupingTypeElementSupportLevel.Allowed:
						allowed = true;
						break;
					case GroupingTypeElementSupportLevel.Blocked:
						blocked = true;
						if (automatic)
						{
							break;
						}
						break;
				}
			}
			allowed = allowed || automatic; // Automatic implies allowed
		}
		#endregion // Delay Validation methods
	}
	#endregion // ElementGrouping class
	#region ElementGroupingSet class
	partial class ElementGroupingSet
	{
		#region Rule Methods
		/// <summary>
		/// DeleteRule: typeof(ElementGroupingSetContainsElementGrouping), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Runs when an <see cref="ElementGrouping"/> is removed and removes the container <see cref="ElementGroupingSet"/>
		/// if it is empty.
		/// </summary>
		private static void GroupingDeletedRule(ElementDeletedEventArgs e)
		{
			ElementGroupingSetContainsElementGrouping link = (ElementGroupingSetContainsElementGrouping)e.ModelElement;
			ElementGroupingSet groupingSet = link.GroupingSet;
			if (!groupingSet.IsDeleted)
			{
				if (groupingSet.GroupingCollection.Count == 0)
				{
					groupingSet.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ElementGroupingSet)
		/// Associate a new <see cref="ElementGroupingSet"/> with the existing <see cref="ORMModel"/>
		/// </summary>
		private static void GroupingSetAddedRule(ElementAddedEventArgs e)
		{
			ElementGroupingSet groupingSet = (ElementGroupingSet)e.ModelElement;
			groupingSet.Model = groupingSet.Store.ElementDirectory.FindElements<ORMModel>()[0];
		}
		#endregion // Rule Methods
	}
	#endregion // ElementGroupingSet class
	#region GroupDuplicateNameError class
	[ModelErrorDisplayFilter(typeof(NameErrorCategory))]
	public partial class ElementGroupingDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		#region DuplicateNameError overrides
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>GroupCollection</returns>
		protected override IList<ModelElement> DuplicateElements
		{
			get
			{
				return GroupingCollection.ToArray();
			}
		}
		/// <summary>
		/// Provide an efficient name lookup
		/// </summary>
		protected override string GetElementName(ModelElement element)
		{
			return ((ElementGrouping)element).Name;
		}
		/// <summary>
		/// Get the text to display the duplicate error information. Replacement
		/// field {0} is replaced by the model name, field {1} is replaced by the
		/// element name.
		/// </summary>
		protected override string ErrorFormatText
		{
			get
			{
				return ResourceStrings.ModelErrorElementGroupingDuplicateNameError;
			}
		}
		#endregion // DuplicateNameError overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ElementGroupingHasDuplicateNameError.DuplicateNameErrorDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // GroupDuplicateNameError class
	#region GroupMembershipContradictionError class
	[ModelErrorDisplayFilter(typeof(ElementGroupingErrorCategory))]
	partial class ElementGroupingMembershipContradictionError
	{
		/// <summary>
		/// Create a new <see cref="ElementGroupingMembershipContradictionError"/>
		/// for the specified <see cref="ElementGrouping"/> and referenced <see cref="ModelElement"/>.
		/// This does not set the owning <see cref="ORMModel"/> for the new error.
		/// </summary>
		/// <param name="grouping">The context <see cref="ElementGrouping"/></param>
		/// <param name="element">The target element</param>
		public ElementGroupingMembershipContradictionError(ElementGrouping grouping, ModelElement element)
			: this(grouping.Store)
		{
			(new ElementGroupingHasMembershipContradictionError(grouping, this)).Element = element;
			Grouping = grouping;
		}
		/// <summary>
		/// Generate text for the error message
		/// </summary>
		public override void GenerateErrorText()
		{
			ElementGrouping group = Grouping;
			ModelElement element = Element;
			DomainPropertyInfo nameProperty;
			ErrorText = string.Format(
				CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorElementGroupingMembershipContradictionError,
				(group != null) ? group.Name : "",
				(element != null) ? ((null != (nameProperty = element.GetDomainClass().NameDomainProperty)) ? nameProperty.GetValue(element) as string : element.ToString()) : "");
		}
		/// <summary>
		/// Regenerate events when the owner name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Get the <see cref="ModelElement"/> that is in a contradictory state
		/// </summary>
		public ModelElement Element
		{
			get
			{
				ElementGroupingHasMembershipContradictionError groupingLink = ElementGroupingHasMembershipContradictionError.GetLinkToGrouping(this);
				if (groupingLink != null)
				{
					return groupingLink.Element;
				}
				return null;
			}
		}
	}
	#endregion // GroupMembershipContradictionError class
	#region NamedElementDictionary Integration
	partial class ElementGroupingSet : INamedElementDictionaryParent
	{
		#region INamedElementDictionaryParent implementation
		[NonSerialized]
		private NamedElementDictionary myGroupsDictionary;
		/// <summary>
		/// Returns the Groups Dictionary
		/// </summary>
		public INamedElementDictionary GroupsDictionary
		{
			get
			{
				INamedElementDictionary retVal = myGroupsDictionary;
				if (retVal == null)
				{
					retVal = myGroupsDictionary = new ElementGroupingNamedElementDictionary();
				}
				return retVal;
			}
		}
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == ElementGroupingSetContainsElementGrouping.GroupingSetDomainRoleId)
			{
				return GroupsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return null;
		}
		#endregion // INamedElementDictionaryParent implementation
		#region Rules to remove duplicate name errors
		/// <summary>
		/// DeleteRule: typeof(ElementGroupingHasDuplicateNameError)
		/// </summary>
		private static void DuplicateGroupingNameGroupingDeletedRule(ElementDeletedEventArgs e)
		{
			ElementGroupingHasDuplicateNameError link = (ElementGroupingHasDuplicateNameError)e.ModelElement;
			ElementGroupingDuplicateNameError error = link.DuplicateNameError;
			if (!error.IsDeleted)
			{
				if (error.GroupingCollection.Count < 2)
				{
					error.Delete();
				}
			}
		}
		#endregion // Rules to remove duplicate name errors
		#region GroupNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of Groups and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		private class ElementGroupingNamedElementDictionary : NamedElementDictionary
		{
			private sealed class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region TrackingList class
				private sealed class TrackingList : List<ElementGrouping>
				{
					private readonly LinkedElementCollection<ElementGrouping> myNativeCollection;
					public TrackingList(ElementGroupingDuplicateNameError error)
					{
						myNativeCollection = error.GroupingCollection;
					}
					public LinkedElementCollection<ElementGrouping> NativeCollection
					{
						get
						{
							return myNativeCollection;
						}
					}
				}
				#endregion // TrackingList class
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, ModelElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					ElementGrouping grouping = (ElementGrouping)element;
					if (afterTransaction)
					{
						if (elementCollection == null)
						{
							ElementGroupingDuplicateNameError error = grouping.DuplicateNameError;
							if (error != null)
							{
								// We're not in a transaction, but the object model will be in
								// the state we need it because we put it there during a transaction.
								// Just return the collection from the current state of the object model.
								TrackingList trackingList = new TrackingList(error);
								trackingList.Add(grouping);
								elementCollection = trackingList;
							}
						}
						else
						{
							((TrackingList)elementCollection).Add(grouping);
						}
						return elementCollection;
					}
					else
					{
						// Modify the object model to add the error.
						if (elementCollection == null)
						{
							ElementGroupingDuplicateNameError error = null;
							if (notifyAdded != null)
							{
								// During deserialization fixup, an error
								// may already be attached to the object. Track
								// it down and verify that it is a legitimate error.
								// If it is not legitimate, then generate a new one.
								error = grouping.DuplicateNameError;
								if (error != null && !error.ValidateDuplicates(grouping))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = new ElementGroupingDuplicateNameError(grouping.Store);
								grouping.DuplicateNameError = error;
								error.Model = grouping.GroupingSet.Model;
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							TrackingList trackingList = new TrackingList(error);
							trackingList.Add(grouping);
							elementCollection = trackingList;
						}
						else
						{
							TrackingList trackingList = (TrackingList)elementCollection;
							trackingList.Add(grouping);
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							LinkedElementCollection<ElementGrouping> typedCollection = trackingList.NativeCollection;
							if (notifyAdded == null || !typedCollection.Contains(grouping))
							{
								typedCollection.Add(grouping);
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, ModelElement element, bool afterTransaction)
				{
					TrackingList trackingList = (TrackingList)elementCollection;
					ElementGrouping group = (ElementGrouping)element;
					trackingList.Remove(group);
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is no longer a duplicate.
						group.DuplicateNameError = null;
					}
					return elementCollection;
				}
				#endregion // IDuplicateNameCollectionManager Implementation
			}
			#region Constructors
			/// <summary>
			/// Default constructor for GroupNamedElementDictionary
			/// </summary>
			public ElementGroupingNamedElementDictionary()
				: base(new DuplicateNameManager())
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Provide a base name for Group elements
			/// </summary>
			/// <param name="element">The element to test</param>
			/// <returns>A base name string pattern</returns>
			protected override string GetRootNamePattern(ModelElement element)
			{
				return ResourceStrings.ElementGroupingDefaultNamePattern;
			}
			/// <summary>
			/// Duplicate automatically generated group names should regenerate on load.
			/// Caters for common merging scenario.
			/// </summary>
			protected override bool ShouldResetDuplicateName(ModelElement element, string elementName)
			{
				return IsDecoratedRootName(element, elementName);
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(ModelElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // GroupNamedElementDictionary class
	}
	partial class ElementGroupingSetContainsElementGrouping : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return GroupingSet; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return Grouping; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	partial class ElementGrouping : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild Implementation
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryChild.GetRoleGuids"/>
		/// </summary>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ElementGroupingSetContainsElementGrouping.GroupingSetDomainRoleId;
			childDomainRoleId = ElementGroupingSetContainsElementGrouping.GroupingDomainRoleId;
		}
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		#endregion // Implementation
	}
	#endregion // NamedElementDictionary Integration
}
