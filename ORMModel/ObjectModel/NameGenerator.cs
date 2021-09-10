#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;
using System.Text;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region NameGenerator class
	partial class NameGenerator
	{
		#region CustomStorage handlers
		private DomainClassInfo myUsageDomainClass;
		private string GetNameUsageValue()
		{
			return ObjectModel.NameUsage.TranslateToNameUsageIdentifier(myUsageDomainClass);
		}

		private void SetNameUsageValue(string value)
		{
			myUsageDomainClass = ObjectModel.NameUsage.TranslateFromNameUsageIdentifier(Store, value);
		}
		/// <summary>
		/// Return the <see cref="Type"/> associated with the <see cref="NameUsage"/> property.
		/// Returns <see langword="null"/> if NameUsage is not set.
		/// </summary>
		public Type NameUsageType
		{
			get
			{
				DomainClassInfo classInfo = myUsageDomainClass;
				return (classInfo != null) ? classInfo.ImplementationClass : null;
			}
		}
		/// <summary>
		/// Return the <see cref="DomainClassInfo"/> associated with the <see cref="NameUsage"/> property.
		/// Returns <see langword="null"/> if NameUsage is not set.
		/// </summary>
		public DomainClassInfo NameUsageDomainClass
		{
			get
			{
				return myUsageDomainClass;
			}
		}
		#endregion // CustomStorage handlers
		#region Serialization support
		/// <summary>
		/// Return <see langword="true"/> if the <see cref="NameGenerator"/>
		/// contains non-default attributes, or refinements that should serialize.
		/// </summary>
		protected bool RequiresSerialization()
		{
			Guid[] ignoredIds = IgnoredAttributeIds;
			bool retVal = !HasDefaultAttributeValues(ignoredIds);
			NameGenerator refinesGenerator;
			if (retVal &&
				null != (refinesGenerator = RefinesGenerator))
			{
				retVal = ignoredIds == null ?
					(refinesGenerator.CasingOption != CasingOption ||
					refinesGenerator.SpacingFormat != SpacingFormat ||
					refinesGenerator.SpacingReplacement != SpacingReplacement ||
					refinesGenerator.AutomaticallyShortenNames != AutomaticallyShortenNames ||
					refinesGenerator.UserDefinedMaximum != UserDefinedMaximum ||
					refinesGenerator.UseTargetDefaultMaximum != UseTargetDefaultMaximum) :
					((refinesGenerator.CasingOption != CasingOption && Array.IndexOf<Guid>(ignoredIds, CasingOptionDomainPropertyId) == -1) ||
					(refinesGenerator.SpacingFormat != SpacingFormat && Array.IndexOf<Guid>(ignoredIds, SpacingFormatDomainPropertyId) == -1) ||
					(refinesGenerator.SpacingReplacement != SpacingReplacement && Array.IndexOf<Guid>(ignoredIds, SpacingReplacementDomainPropertyId) == -1) ||
					(refinesGenerator.AutomaticallyShortenNames != AutomaticallyShortenNames && Array.IndexOf<Guid>(ignoredIds, AutomaticallyShortenNamesDomainPropertyId) == -1) ||
					(refinesGenerator.UserDefinedMaximum != UserDefinedMaximum && Array.IndexOf<Guid>(ignoredIds, UserDefinedMaximumDomainPropertyId) == -1) ||
					(refinesGenerator.UseTargetDefaultMaximum != UseTargetDefaultMaximum && Array.IndexOf<Guid>(ignoredIds, UseTargetDefaultMaximumDomainPropertyId) == -1));
			}
			if (!retVal)
			{
				foreach (NameGenerator refinement in RefinedByGeneratorCollection)
				{
					if (refinement.RequiresSerialization())
					{
						retVal = true;
						break;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Allow a derived name generator to ignore one or more stock attributes. Returns null by default.
		/// Ignored ids are not displayed, serialized or automatically modified in response to parent changes.
		/// </summary>
		/// <remarks>No runtime effort is made to reconcile ignored ids between parents and children.
		/// A derived item should include all ignored items on the base.</remarks>
		protected virtual Guid[] IgnoredAttributeIds
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Is a stock attribute id ignored for this type of name generator?
		/// </summary>
		public bool IsIgnoredAttributeId(Guid attributeId)
		{
			Guid[] ignoredIds = IgnoredAttributeIds;
			return ignoredIds != null &&
				Array.IndexOf<Guid>(ignoredIds, attributeId) != -1;
		}
		/// <summary>
		/// Verify if this instance has fully default values
		/// </summary>
		/// <param name="ignorePropertyIds">Domain property identifiers that should not be checked. Designed
		/// to be called by a derived class that overrides specific properties.</param>
		/// <returns><see langword="true"/> if all values are default</returns>
		protected virtual bool HasDefaultAttributeValues(Guid[] ignorePropertyIds)
		{
			if (ignorePropertyIds == null || ignorePropertyIds.Length == 0)
			{
				return CasingOption == NameGeneratorCasingOption.None &&
					SpacingFormat == NameGeneratorSpacingFormat.Retain &&
					SpacingReplacement.Length == 0 &&
					AutomaticallyShortenNames &&
					UserDefinedMaximum == 128 &&
					UseTargetDefaultMaximum;
			}
			return (CasingOption == NameGeneratorCasingOption.None || Array.IndexOf<Guid>(ignorePropertyIds, CasingOptionDomainPropertyId) != -1) &&
				(SpacingFormat == NameGeneratorSpacingFormat.Retain || Array.IndexOf<Guid>(ignorePropertyIds, SpacingFormatDomainPropertyId) != -1) &&
				(SpacingReplacement.Length == 0 || Array.IndexOf<Guid>(ignorePropertyIds, SpacingReplacementDomainPropertyId) != -1) &&
				(AutomaticallyShortenNames || Array.IndexOf<Guid>(ignorePropertyIds, AutomaticallyShortenNamesDomainPropertyId) != -1) &&
				(UserDefinedMaximum == 128 || Array.IndexOf<Guid>(ignorePropertyIds, UserDefinedMaximumDomainPropertyId) != -1) &&
				(UseTargetDefaultMaximum || Array.IndexOf<Guid>(ignorePropertyIds, UseTargetDefaultMaximumDomainPropertyId) != -1);
		}
		partial class SynchronizedRefinementsPropertyChangedRuleClass
		{
			private bool myIsDisabled;
			/// <summary>
			/// ChangeRule: typeof(NameGenerator)
			/// If the current value of the same property on any refinement
			/// is equal to the old value, then change the refinement value to the new value.
			/// </summary>
			private void SynchronizedRefinementsPropertyChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (myIsDisabled)
				{
					return;
				}
				DomainPropertyInfo propertyInfo = e.DomainProperty;
				if (propertyInfo.Id == NameGenerator.NameUsageDomainPropertyId)
				{
					throw new InvalidOperationException("NameUsage should be initialized by CreateRefinement and not changed.");
				}
				NameGenerator changedGenerator = (NameGenerator)e.ModelElement;
				try
				{
					myIsDisabled = true;
					PropagateChange(changedGenerator, propertyInfo, e.OldValue, e.NewValue, changedGenerator.NameUsageType);
				}
				finally
				{
					myIsDisabled = false;
				}
			}
			private static void PropagateChange(NameGenerator parentGenerator, DomainPropertyInfo propertyInfo, object oldValue, object newValue, Type nameUsageType)
			{
				foreach (NameGenerator refinement in parentGenerator.RefinedByGeneratorCollection)
				{
					if (nameUsageType != null)
					{
						Type refinementUsage = refinement.NameUsageType;
						if (refinementUsage == null)
						{
							// Skip a level
							PropagateChange(refinement, propertyInfo, oldValue, newValue, nameUsageType);
							continue;
						}
						else if (refinementUsage != nameUsageType)
						{
							continue;
						}
					}

					Guid[] ignoredAttributeIds = refinement.IgnoredAttributeIds;
					if (ignoredAttributeIds != null &&
						Array.IndexOf<Guid>(ignoredAttributeIds, propertyInfo.Id) != -1)
					{
						continue;
					}

					if (propertyInfo.GetValue(refinement).Equals(oldValue))
					{
						propertyInfo.SetValue(refinement, newValue);
						PropagateChange(refinement, propertyInfo, oldValue, newValue, nameUsageType);
					}
				}
			}
		}
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener automatically
		/// creates instances for each of the name generator types loaded in the store.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new NameGeneratorFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit FactConstraint relationships
		/// </summary>
		private sealed class NameGeneratorFixupListener : DeserializationFixupListener<NameGenerator>, INotifyElementAdded
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public NameGeneratorFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Empty implementation
			/// </summary>
			protected sealed override void ProcessElement(NameGenerator element, Store store, INotifyElementAdded notifyAdded)
			{
				// Everything happens in PhaseCompleted
			}
			/// <summary>
			/// Recursive load NameGenerator instances
			/// </summary>
			protected override void PhaseCompleted(Store store)
			{
				ReadOnlyCollection<NameGenerator> topGenerators = store.ElementDirectory.FindElements<NameGenerator>(false);
				NameGenerator topGenerator = null;
				if (topGenerators.Count == 0 &&
					store.DomainDataDirectory.GetDomainClass(NameGenerator.DomainClassId).LocalDescendants.Count != 0)
				{
					topGenerator = new NameGenerator(store);
				}
				else
				{
					foreach (NameGenerator generator in topGenerators)
					{
						topGenerator = generator;
						break;
					}
				}
				if (topGenerator != null)
				{
					topGenerator.FullyPopulateRefinements(true, false);
				}
			}
			#region INotifyElementAdded Implementation
			void INotifyElementAdded.ElementAdded(ModelElement element)
			{
				// Just block the base from getting this, we're not recording the elements
			}
			#endregion // INotifyElementAdded Implementation
		}
		#endregion // Deserialization Fixup
		// Helpers for FullyPopulateRefinements
		private delegate NameGeneratorRefinesInstance GetRefinedInstance(NameGenerator owner, ModelElement refinedInstance, ref PropertyAssignment[] propertyAssignments, ref int nameUsagePropertyIndex);
		/// <summary>
		/// This guarantees that we have an instance for every possible Name Generator type.
		/// </summary>
		private void FullyPopulateRefinements(bool topLevel, bool newInstance)
		{
			LinkedElementCollection<NameGenerator> currentChildren = RefinedByGeneratorCollection;
			DomainClassInfo contextDomainClass = GetDomainClass();
			ReadOnlyCollection<DomainClassInfo> requiredDescendantsCollection = contextDomainClass.LocalDescendants;
			int requiredDescendantsCount = requiredDescendantsCollection.Count;
			Type[] requiredUsageTypes = GetSupportedNameUsageTypes();
			int requiredUsageCount = requiredUsageTypes.Length;
			Store store = Store;
			int refinedInstancesCount = 0;
			IList<ModelElement> refinedInstances = null;
			GetRefinedInstance getRefinedInstanceCallback = null;
			if (!topLevel)
			{
				DomainRoleInfo refinedGeneratorRole = RefiningAutoCreateRelationshipRole;
				if (refinedGeneratorRole != null)
				{
					if (!newInstance && requiredUsageCount != 0 && this.NameUsageType == null)
					{
						// Name usage was added to the model. Refinements are below the usage types.
						ClearRefiningInstances(this);
					}

					DomainRoleInfo refinedInstanceRole = refinedGeneratorRole.OppositeDomainRole;
					DomainRelationshipInfo refiningRelationship = refinedGeneratorRole.DomainRelationship;
					refinedInstances = store.DefaultPartition.ElementDirectory.FindElements(refinedInstanceRole.RolePlayer, true);
					refinedInstancesCount = refinedInstances.Count;
					if (refinedInstances.Count != 0)
					{
						getRefinedInstanceCallback = delegate (NameGenerator refinementParent, ModelElement refinedInstance, ref PropertyAssignment[] refinementPropertyAssignments, ref int nameUsagePropertyIndex)
						{
							NameGeneratorRefinesInstance typedLink;
							foreach (ElementLink link in refinedInstanceRole.GetElementLinks(refinedInstance))
							{
								typedLink = (NameGeneratorRefinesInstance)link;
								if (typedLink.NameGenerator.RefinesGenerator == refinementParent)
								{
									return typedLink;
								}
							}

							NameGenerator refinedGenerator = refinementParent.CreateRefinement(contextDomainClass, refinementParent.NameUsageType, ref refinementPropertyAssignments, ref nameUsagePropertyIndex);
							typedLink = (NameGeneratorRefinesInstance)store.ElementFactory.CreateElementLink(refiningRelationship, new RoleAssignment(refinedGeneratorRole.Id, refinedGenerator), new RoleAssignment(refinedInstanceRole.Id, refinedInstance));
							return typedLink;
						};
					}
				}
			}

			if (getRefinedInstanceCallback != null && (requiredUsageCount == 0 || NameUsageType != null))
			{
				PropertyAssignment[] propertyAssignments = null;
				int nameUsagePropertyIndex = -1;
				for (int j = 0; j < refinedInstancesCount; ++j)
				{
					getRefinedInstanceCallback(this, refinedInstances[j], ref propertyAssignments, ref nameUsagePropertyIndex);
				}
			}

			if (requiredDescendantsCount != 0 || requiredUsageCount != 0)
			{
				DomainClassInfo[] requiredDescendants = new DomainClassInfo[requiredDescendantsCount];
				requiredDescendantsCollection.CopyTo(requiredDescendants, 0);
				int missingDescendantsCount = requiredDescendantsCount;
				int missingUsageCount = requiredUsageCount;
				foreach (NameGenerator currentChild in currentChildren)
				{
					if (missingDescendantsCount != 0)
					{
						int index = Array.IndexOf<DomainClassInfo>(requiredDescendants, currentChild.GetDomainClass());
						if (index != -1)
						{
							requiredDescendants[index] = null;
						}
					}
					Type nameUsage = currentChild.NameUsageType;
					if (nameUsage == null)
					{
						currentChild.FullyPopulateRefinements(false, false);
					}
					else
					{
						if (getRefinedInstanceCallback != null)
						{
							PropertyAssignment[] propertyAssignments = null;
							int nameUsagePropertyIndex = -1;
							for (int j = 0; j < refinedInstancesCount; ++j)
							{
								getRefinedInstanceCallback(currentChild, refinedInstances[j], ref propertyAssignments, ref nameUsagePropertyIndex);
							}
						}

						if (missingUsageCount != 0)
						{
							int index = Array.IndexOf<Type>(requiredUsageTypes, nameUsage);
							if (index != -1)
							{
								requiredUsageTypes[index] = null;
							}
						}
					}
				}
				if (missingDescendantsCount != 0)
				{
					for (int i = 0; i < requiredDescendantsCount; ++i)
					{
						DomainClassInfo classInfo = requiredDescendants[i];
						if (classInfo != null)
						{
							PropertyAssignment[] propertyAssignments = null;
							int nameUsagePropertyIndex = -1;
							NameGenerator newGenerator = CreateRefinement(classInfo, null, ref propertyAssignments, ref nameUsagePropertyIndex);
							newGenerator.FullyPopulateRefinements(false, true);
							if (getRefinedInstanceCallback != null)
							{
								for (int j = 0; j < refinedInstancesCount; ++j)
								{
									getRefinedInstanceCallback(newGenerator, refinedInstances[j], ref propertyAssignments, ref nameUsagePropertyIndex);
								}
							}
						}
					}
				}
				if (missingUsageCount != 0)
				{
					for (int i = 0; i < requiredUsageCount; ++i)
					{
						Type usageType = requiredUsageTypes[i];
						if (usageType != null)
						{
							PropertyAssignment[] propertyAssignments = null;
							int nameUsagePropertyIndex = -1;
							NameGenerator newGenerator = CreateRefinement(contextDomainClass, usageType, ref propertyAssignments, ref nameUsagePropertyIndex);
							if (getRefinedInstanceCallback != null)
							{
								for (int j = 0; j < refinedInstancesCount; ++j)
								{
									getRefinedInstanceCallback(newGenerator, refinedInstances[j], ref propertyAssignments, ref nameUsagePropertyIndex);
								}
							}
						}
					}
				}
			}
		}
		private static void ClearRefiningInstances(NameGenerator owner)
		{
			ReadOnlyCollection<NameGeneratorRefinesInstance> links = NameGeneratorRefinesInstance.GetLinksToRefiningNameGenerators(owner);
			for (int i = links.Count - 1; i >= 0; --i)
			{
				NameGeneratorRefinesInstance link = links[i];
				if (link.NameGenerator.RefinedInstance != null)
				{
					link.Delete(); // Deletes propagate to refining generator
					// UNDONE: NOW Not sure this is a good idea. Just get the name consumer for this generator and get rid of these dangling aliases on load.

					// Note that matching instances (if any) are kept alive, but will not bind to anything or save.
					// This is a cleanup path that should only trigger if the NameUsage settings changed.
				}
			}
		}
		#endregion // Serialization support
		#region NameUsage and RefinedNameGeneratorInstance Attribute Caching
		// With refined instances there are multiple instances of these elements, so it makes sense to cache all of the attribute-derived data
		// on a static, per-type basis. Note that we cache ids here, not domain roles, because these instances will differ between stores.
		private sealed class ResolvedAttributeData
		{
			public readonly Type[] NameUsageTypes;
			public readonly Guid? RefinedAutoCreateRelationshipRoleId;
			public ResolvedAttributeData(Type implementingType)
			{
				// Get NameUsage information
				NameUsageAttribute[] usageAttributes = (NameUsageAttribute[])implementingType.GetCustomAttributes(typeof(NameUsageAttribute), true);
				Type[] usageTypes;
				int attrCount = usageAttributes.Length;
				if (attrCount == 0)
				{
					usageTypes = Type.EmptyTypes;
				}
				else
				{
					usageTypes = new Type[attrCount];
					for (int i = 0; i < attrCount; ++i)
					{
						usageTypes[i] = usageAttributes[i].Type;
					}
				}
				NameUsageTypes = usageTypes;

				// Figure out if this name generator supports per-instance refinement
				RefinedNameGeneratorInstanceAttribute[] instanceRefinementAttributes = (RefinedNameGeneratorInstanceAttribute[])implementingType.GetCustomAttributes(typeof(RefinedNameGeneratorInstanceAttribute), false);
				attrCount = instanceRefinementAttributes.Length;
				if (attrCount == 0)
				{
					RefinedAutoCreateRelationshipRoleId = null;
				}
				else
				{
					RefinedNameGeneratorInstanceAttribute attr = instanceRefinementAttributes[0];
					RefinedAutoCreateRelationshipRoleId = attr.AutoCreateRelationshipRole;
				}
			}
		}
		private static Dictionary<Type, ResolvedAttributeData> myResolvedAttributeData = null;
		/// <summary>
		/// Helper function to get attribute-based static data for the current implementing type.
		/// </summary>
		/// <returns>New or cached data for this type</returns>
		private ResolvedAttributeData GetResolvedAttributeData()
		{
			Type implementingType = GetType();
			Dictionary<Type, ResolvedAttributeData> dict = myResolvedAttributeData;
			if (dict == null)
			{
				dict = new Dictionary<Type, ResolvedAttributeData>();
				Dictionary<Type, ResolvedAttributeData> existing = System.Threading.Interlocked.CompareExchange(ref myResolvedAttributeData, dict, null);
				if (existing != null)
				{
					dict = existing;
				}
			}

			ResolvedAttributeData retVal;
			if (dict.TryGetValue(implementingType, out retVal))
			{
				return retVal;
			}
			retVal = new ResolvedAttributeData(implementingType);
			lock (dict)
			{
				ResolvedAttributeData existingData;
				if (dict.TryGetValue(implementingType, out existingData))
				{
					retVal = existingData;
				}
				else
				{
					dict[implementingType] = retVal;
				}
			}
			return retVal;
		}
		private Type[] NameUsageTypes
		{
			get
			{
				return GetResolvedAttributeData().NameUsageTypes;
			}
		}
		/// <summary>
		/// Get all <see cref="Type"/>s supported by the <see cref="NameUsageType"/> property.
		/// Based on the <see cref="NameUsageAttribute"/>s associated with this <see cref="NameGenerator"/>.
		/// The contents of the returned array may be safely modified.
		/// </summary>
		public Type[] GetSupportedNameUsageTypes()
		{
			Type[] types = NameUsageTypes;
			return (types.Length == 0) ? types : (Type[])types.Clone();
		}
		/// <summary>
		/// Does this name generator allow instance refinements?
		/// </summary>
		public bool AllowsRefinedInstances
		{
			get
			{
				return GetResolvedAttributeData().RefinedAutoCreateRelationshipRoleId.HasValue;
			}
		}
		/// <summary>
		/// The <see cref="DomainRoleInfo"/> played by this name generator class links to the
		/// refined instance that uses the generated names.
		/// </summary>
		public DomainRoleInfo RefiningAutoCreateRelationshipRole
		{
			get
			{
				Guid? domainRoleId = GetResolvedAttributeData().RefinedAutoCreateRelationshipRoleId;
				return domainRoleId.HasValue ? Store.DomainDataDirectory.GetDomainRole(domainRoleId.Value) : null;
			}
		}
		#endregion // NameUsage and RefinedNameGeneratorInstance Attribute Caching
		#region Refinement management
		/// <summary>
		/// Get the property assignments for this name generator
		/// </summary>
		/// <param name="nameUsageType">The type to associate with the <see cref="NameUsage"/> property. Can be <see langword="null"/></param>
		/// <param name="nameGeneratorClass">The class information to use. This can be null and can be populated automaticall.</param>
		/// <param name="nameUsagePropertyIndex">Return the index for the name usage property so it can be updated later with <see cref="UpdateNameUsageTypeProperty"/></param>
		/// <returns>Current property assignments, suitable for initializing a new child instance with the parent property settings.</returns>
		private PropertyAssignment[] ExtractPropertyAssignments(Type nameUsageType, DomainClassInfo nameGeneratorClass, out int nameUsagePropertyIndex)
		{
			if (nameGeneratorClass == null)
			{
				nameGeneratorClass = GetDomainClass();
			}
			ReadOnlyCollection<DomainPropertyInfo> properties = nameGeneratorClass.AllDomainProperties;
			PropertyAssignment[] propertyAssignments = new PropertyAssignment[properties.Count + (nameUsageType == null ? -1 : 0)];
			Partition partition = Partition;
			int i = 0;
			nameUsagePropertyIndex = -1;
			foreach (DomainPropertyInfo property in properties)
			{
				if (property.Id != NameUsageDomainPropertyId)
				{
					propertyAssignments[i] = new PropertyAssignment(property.Id, property.GetValue(this));
					++i;
				}
				else if (nameUsageType != null)
				{
					nameUsagePropertyIndex = i;
					propertyAssignments[i] = new PropertyAssignment(property.Id, ObjectModel.NameUsage.TranslateToNameUsageIdentifier(partition.DomainDataDirectory.FindDomainClass(nameUsageType)));
					++i;
				}
			}
			return propertyAssignments;
		}
		private void UpdateNameUsageTypeProperty(PropertyAssignment[] propertyAssignments, int nameUsageIndex, Type nameUsageType)
		{
			if (nameUsageIndex == -1)
			{
				return;
			}
			PropertyAssignment existingAssignment = propertyAssignments[nameUsageIndex];
			propertyAssignments[nameUsageIndex] = new PropertyAssignment(existingAssignment.PropertyId, ObjectModel.NameUsage.TranslateToNameUsageIdentifier(Partition.DomainDataDirectory.FindDomainClass(nameUsageType)));
		}
		/// <summary>
		/// Create a refinement of the specified type
		/// </summary>
		/// <param name="childType">A <see cref="DomainClassInfo"/> of the current type if <paramref name="nameUsageType"/> is set.
		/// Otherwise, the <see cref="DomainClassInfo.BaseDomainClass"/> must equal the current <see cref="ModelElement.GetDomainClass">DomainClass</see>.</param>
		/// <param name="nameUsageType">The type to associate with the <see cref="NameUsage"/> property. Can be <see langword="null"/></param>
		/// <param name="propertyAssignments">The initial property assignments for a new instance.</param>
		/// <param name="nameUsagePropertyIndex">If properties were created during a previous call, the index of the name usage property in the array.
		/// This property needs to be updated on each call instead of using the cached value.</param>
		/// <returns>A new <see cref="NameGenerator"/> of the specified type</returns>
		private NameGenerator CreateRefinement(DomainClassInfo childType, Type nameUsageType, ref PropertyAssignment[] propertyAssignments, ref int nameUsagePropertyIndex)
		{
			if (propertyAssignments == null)
			{
				DomainClassInfo nameGeneratorClass = GetDomainClass();
				Debug.Assert(childType == nameGeneratorClass || childType.BaseDomainClass == nameGeneratorClass);
				propertyAssignments = ExtractPropertyAssignments(nameUsageType, nameGeneratorClass, out nameUsagePropertyIndex);
			}
			else
			{
				if (nameUsagePropertyIndex != -1)
				{
					UpdateNameUsageTypeProperty(propertyAssignments, nameUsagePropertyIndex, nameUsageType);
				}
			}
			NameGenerator retVal = (NameGenerator)Store.GetDomainModel(childType.DomainModel.Id).CreateElement(Partition, childType.ImplementationClass, propertyAssignments);
			retVal.RefinesGenerator = this;
			return retVal;
		}
		/// <summary>
		/// Create new generators for a refined instance.
		/// </summary>
		/// <param name="store">The context <see cref="Store"/> object.</param>
		/// <param name="generatorType">The type of generator to create instance refinements for.</param>
		/// <param name="onCreated">A callback for attaching the refined instance to the newly create typed generator.</param>
		public static void CreateGeneratorsForRefinedInstance(Store store, Type generatorType, Action<NameGenerator> onCreated)
		{
			NameGenerator primaryGenerator = GetGenerator(store, generatorType, null, null);
			if (primaryGenerator != null)
			{
				DomainClassInfo domainClass = store.DomainDataDirectory.GetDomainClass(generatorType);
				PropertyAssignment[] propertyAssignments = null;
				int nameUsagePropertyIndex = -1;
				if (primaryGenerator.NameUsageTypes.Length == 0)
				{
					onCreated(primaryGenerator.CreateRefinement(domainClass, null, ref propertyAssignments, ref nameUsagePropertyIndex));
				}
				else
				{
					foreach (NameGenerator generatorWithUsageType in primaryGenerator.RefinedByGeneratorCollection)
					{
						onCreated(generatorWithUsageType.CreateRefinement(domainClass, generatorWithUsageType.NameUsageType, ref propertyAssignments, ref nameUsagePropertyIndex));
					}
				}
			}
		}
		#endregion // Refinement management
		#region GetGeneratorSettings
		/// <summary>
		/// Gets the settings for specific <see cref="NameGenerator"/>
		/// </summary>
		/// <param name="store">The <see cref="Store"/></param>
		/// <param name="generatorType">The type of the <see cref="NameGenerator"/></param>
		/// <param name="usageType">The <see cref="NameUsage"/> of the NameGenerator</param>
		/// <param name="refiningInstance">Set to a retrieve a refined generator for a specific refined instance.</param>
		/// <returns>Matching generator.</returns>
		/// <remarks>The <paramref name="refiningInstance"/> is ignored if the generator type does not support refining instances.</remarks>
		public static NameGenerator GetGenerator(Store store, Type generatorType, Type usageType, ModelElement refiningInstance)
		{
			bool init = false;
			bool matchInstance = false;
			foreach (NameGenerator generator in store.ElementDirectory.FindElements(store.DomainDataDirectory.GetDomainClass(generatorType), false))
			{
				if (!init)
				{
					init = true;
					matchInstance = generator.AllowsRefinedInstances;
				}
				if (generator.NameUsageType == usageType && (!matchInstance || refiningInstance == generator.RefinedInstance))
				{
					return generator;
				}
			}
			Debug.Fail("Generator not loaded with domain model");
			return null;
		}
		/// <summary>
		/// Retrieve the best matching alias for the provided set of aliases
		/// </summary>
		/// <remarks>If an exact type/usage match is not available, then this will
		/// return the closest usage match over the closest type match. The closest
		/// matches are determined by walking up the parent hierarchy of name generators.</remarks>
		/// <param name="aliases">A set of alias elements. The best match is returned.</param>
		/// <returns>A <see cref="NameAlias"/>, or <see langword="null"/> if none is available.</returns>
		public NameAlias FindMatchingAlias(IEnumerable<NameAlias> aliases)
		{
			NameAlias bestUsageMatch = null;
			NameAlias bestTypeMatch = null;
			NameAlias bestInstanceMatch = null;
			Type usageType = NameUsageType;
			bool checkInstance = AllowsRefinedInstances;
			ModelElement refinedInstance = checkInstance ? RefinedInstance : null;
			DomainClassInfo thisClassInfo = GetDomainClass();
			int closestTypeDistance = int.MaxValue;
			int closestUsageDistance = int.MaxValue;
			foreach (NameAlias alias in aliases)
			{
				DomainClassInfo testClassInfo = alias.NameConsumerDomainClass;
				Type testUsageType = alias.NameUsageType;
				ModelElement testInstance = null;
				bool instanceMatch = !checkInstance; // Degenerate case, nothing to check
				if (checkInstance)
				{
					testInstance = alias.RefinedInstance;
					if (refinedInstance == null)
					{
						if (testInstance != null)
						{
							// Do not ever match anything with an instance if the generator itself does not
							// have a refined instance.
							continue;
						}
						instanceMatch = true;
					}
					else if (testInstance != null)
					{
						if (testInstance != refinedInstance)
						{
							// If both the generator and alias are refined the two instances must match
							continue;
						}
						instanceMatch = true;
					}
					// Otherwise continue. There is no instance match, but we may still want the unrefined alias
					// if we don't find an exact usage.
				}
				if (testClassInfo == thisClassInfo)
				{
					if (usageType == testUsageType) // intentionally handles two null values
					{
						bestUsageMatch = alias;
						if (instanceMatch)
						{
							bestInstanceMatch = alias;
							break;
						}
						// Otherwise keep going to see if we get an exact instance match
					}
					else if (usageType != null && testUsageType == null)
					{
						closestTypeDistance = 0; // Matched self, can't get any closer
						bestTypeMatch = alias;
						// Keep going to see if we get a higher priority usage match
						// If there is a usage type then the refined instance matches will always be on the
						// the instances with a usage specied, so we do not set the refined instance here.
					}
				}
				else
				{
					DomainClassInfo iterateClassInfo = thisClassInfo.BaseDomainClass;
					int testDistance = 0;
					do
					{
						++testDistance;
						if (iterateClassInfo == testClassInfo)
						{
							if (usageType == testUsageType) // intentionally handles two null values
							{
								if (testDistance < closestUsageDistance)
								{
									closestUsageDistance = testDistance;
									bestUsageMatch = alias;
									if (instanceMatch)
									{
										bestInstanceMatch = alias;
									}
								}
							}
							else if (usageType != null && testUsageType == null)
							{
								if (testDistance < closestTypeDistance)
								{
									closestTypeDistance = testDistance;
									bestTypeMatch = alias;
								}
							}
							break;
						}
						iterateClassInfo = iterateClassInfo.BaseDomainClass;
					} while (iterateClassInfo != null);
				}
			}
			return bestInstanceMatch ?? bestUsageMatch ?? bestTypeMatch;
		}
		#endregion // GetGeneratorSettings
	}
	#endregion // NameGenerator class
}
