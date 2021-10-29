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
	/// <summary>
	/// Flag enum for user-settable properties defined on the <see cref="NameGenerator"/> class.
	/// This allows bitfields to be used in virtual methods instead of lists of domain
	/// property id guids.
	/// </summary>
	[Flags]
	public enum StandardNameGeneratorProperty
	{
		/// <summary>
		/// No attributes
		/// </summary>
		None = 0,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.CasingOption"/> domain property
		/// </summary>
		CasingOption = 1,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.SpacingFormat"/> domain property
		/// </summary>
		SpacingFormat = 2,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.SpacingReplacement"/> domain property
		/// </summary>
		SpacingReplacement = 4,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.AutomaticallyShortenNames"/> domain property
		/// </summary>
		AutomaticallyShortenNames = 8,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.UserDefinedMaximum"/> domain property
		/// </summary>
		UserDefinedMaximum = 0x10,
		/// <summary>
		/// Corresponds to the <see cref="NameGenerator.UseTargetDefaultMaximum"/> domain property
		/// </summary>
		UseTargetDefaultMaximum = 0x20
		// If this is changed, also change myStandardPropertyIds below
	}
	partial class NameGenerator
	{
		#region CustomStorage handlers
		[Flags]
		private enum NameGeneratorFlags
		{
			CasingOptionInit = 1, // If these bits change adjust the InitializedProperties getter. These obviously line up with StandardNameGeneratorProperty values
			SpacingFormatInit = 2,
			SpacingReplacementInit = 4,
			AutomaticallyShortenNamesInit = 8,
			UserDefinedMaximumInit = 0x10,
			UseTargetDefaultMaximumInit = 0x20,
			CasingOptionBit1 = 0x40, // If these bits change adjust bit shift of 6 in GetCasingOption and SetCasingOption
			CasingOptionBit2 = 0x80,
			CasingOptionBit3 = 0x100,
			SpacingFormatBit1 = 0x200, // If these bits change adjust bit shift of 9 in GetSpacingFormat and SetSpacingFormat
			SpacingFormatBit2 = 0x400,
			AutomaticallyShortenNames = 0x800,
			UseTargetDefaultMaximum = 0x1000,
		}
		private NameGeneratorFlags myFlags;
		private bool GetFlag(NameGeneratorFlags flags)
		{
			return 0 != (myFlags & flags);
		}
		private void SetFlag(NameGeneratorFlags flags, bool value)
		{
			if (value)
			{
				myFlags |= flags;
			}
			else
			{
				myFlags &= ~flags;
			}
		}
		/// <summary>
		/// Return a <see cref="StandardNameGeneratorProperty"/> indicating which values have been explicitly set.
		/// </summary>
		/// <remarks>This is virtual so that subtypes can add additional properties. The recommendation is to do this
		/// in an enum with bit fields that do no conflict with the standard properties, then downcast the combination.
		/// If additional properties are not used then no override is required for this property.</remarks>
		protected virtual StandardNameGeneratorProperty InitializedProperties
		{
			get
			{
				return (StandardNameGeneratorProperty)((int)(myFlags & (NameGeneratorFlags.CasingOptionInit | NameGeneratorFlags.SpacingFormatInit | NameGeneratorFlags.SpacingReplacementInit | NameGeneratorFlags.AutomaticallyShortenNamesInit | NameGeneratorFlags.UserDefinedMaximumInit | NameGeneratorFlags.UseTargetDefaultMaximumInit)) /*>> 0*/);
			}
		}
		private NameGeneratorCasingOption GetCasingOptionValue()
		{
			return GetFlag(NameGeneratorFlags.CasingOptionInit) ?
				(NameGeneratorCasingOption)((int)(myFlags & (NameGeneratorFlags.CasingOptionBit1 | NameGeneratorFlags.CasingOptionBit2 | NameGeneratorFlags.CasingOptionBit3)) >> 6) :
				NameGeneratorCasingOption.Uninitialized;
		}
		private void SetCasingOptionValue(NameGeneratorCasingOption value)
		{
			Debug.Assert(value != NameGeneratorCasingOption.Uninitialized, "Do not set CasingOption to Uninitialized");
			myFlags = (NameGeneratorFlags)(((int)myFlags & ~(int)(NameGeneratorFlags.CasingOptionInit | NameGeneratorFlags.CasingOptionBit1 | NameGeneratorFlags.CasingOptionBit2 | NameGeneratorFlags.CasingOptionBit3)) | ((int)value << 6) | (int)NameGeneratorFlags.CasingOptionInit);
		}
		private NameGeneratorSpacingFormat GetSpacingFormatValue()
		{
			return GetFlag(NameGeneratorFlags.SpacingFormatInit) ?
				(NameGeneratorSpacingFormat)((int)(myFlags & (NameGeneratorFlags.SpacingFormatBit1 | NameGeneratorFlags.SpacingFormatBit2)) >> 9) :
				NameGeneratorSpacingFormat.Uninitialized;
		}
		private void SetSpacingFormatValue(NameGeneratorSpacingFormat value)
		{
			Debug.Assert(value != NameGeneratorSpacingFormat.Uninitialized, "Do not set SpacingFormat to Uninitialized");
			myFlags = (NameGeneratorFlags)(((int)myFlags & ~(int)(NameGeneratorFlags.SpacingFormatInit | NameGeneratorFlags.SpacingFormatBit1 | NameGeneratorFlags.SpacingFormatBit2)) | ((int)value << 9) | (int)NameGeneratorFlags.SpacingFormatInit);
		}
		private NameGeneratorUninitializedBoolean GetAutomaticallyShortenNamesInitializerValue()
		{
			return GetFlag(NameGeneratorFlags.AutomaticallyShortenNamesInit) ?
				GetFlag(NameGeneratorFlags.AutomaticallyShortenNames) ? NameGeneratorUninitializedBoolean.@true : NameGeneratorUninitializedBoolean.@false :
				NameGeneratorUninitializedBoolean.Uninitialized;
		}
		private void SetAutomaticallyShortenNamesInitializerValue(NameGeneratorUninitializedBoolean value)
		{
			Debug.Assert(value != NameGeneratorUninitializedBoolean.Uninitialized, "Do not set AutomaticallyShortenNamesInitializer to Uninitialized");
			if (value == NameGeneratorUninitializedBoolean.@true)
			{
				SetFlag(NameGeneratorFlags.AutomaticallyShortenNames | NameGeneratorFlags.AutomaticallyShortenNamesInit, true);
			}
			else
			{
				SetFlag(NameGeneratorFlags.AutomaticallyShortenNamesInit, true);
				SetFlag(NameGeneratorFlags.AutomaticallyShortenNames, false);
			}
		}
		private bool GetAutomaticallyShortenNamesValue()
		{
			return GetFlag(NameGeneratorFlags.AutomaticallyShortenNames);
		}
		private void SetAutomaticallyShortenNamesValue(bool value)
		{
			Debug.Assert(GetFlag(NameGeneratorFlags.AutomaticallyShortenNamesInit));
			SetFlag(NameGeneratorFlags.AutomaticallyShortenNames, value);
		}
		private NameGeneratorUninitializedBoolean GetUseTargetDefaultMaximumInitializerValue()
		{
			return GetFlag(NameGeneratorFlags.UseTargetDefaultMaximumInit) ?
				GetFlag(NameGeneratorFlags.UseTargetDefaultMaximum) ? NameGeneratorUninitializedBoolean.@true : NameGeneratorUninitializedBoolean.@false :
				NameGeneratorUninitializedBoolean.Uninitialized;
		}
		private void SetUseTargetDefaultMaximumInitializerValue(NameGeneratorUninitializedBoolean value)
		{
			Debug.Assert(value != NameGeneratorUninitializedBoolean.Uninitialized, "Do not set UseTargetDefaultMaximumInitializer to Uninitialized");
			if (value == NameGeneratorUninitializedBoolean.@true)
			{
				SetFlag(NameGeneratorFlags.UseTargetDefaultMaximum | NameGeneratorFlags.UseTargetDefaultMaximumInit, true);
			}
			else
			{
				SetFlag(NameGeneratorFlags.UseTargetDefaultMaximumInit, true);
				SetFlag(NameGeneratorFlags.UseTargetDefaultMaximum, false);
			}
		}
		private bool GetUseTargetDefaultMaximumValue()
		{
			return GetFlag(NameGeneratorFlags.UseTargetDefaultMaximum);
		}
		private void SetUseTargetDefaultMaximumValue(bool value)
		{
			Debug.Assert(GetFlag(NameGeneratorFlags.UseTargetDefaultMaximumInit));
			SetFlag(NameGeneratorFlags.UseTargetDefaultMaximum, value);
		}

		private int myUserDefinedMaximum = int.MinValue;
		private int GetUserDefinedMaximumValue()
		{
			return myUserDefinedMaximum;
		}
		private void SetUserDefinedMaximumValue(int value)
		{
			if (myUserDefinedMaximum == int.MinValue)
			{
				SetFlag(NameGeneratorFlags.UserDefinedMaximumInit, true);
			}
			myUserDefinedMaximum = value;
		}

		private string mySpacingReplacement = null;
		private string GetSpacingReplacementValue()
		{
			return mySpacingReplacement;
		}
		private void SetSpacingReplacementValue(string value)
		{
			if (mySpacingReplacement == null)
			{
				SetFlag(NameGeneratorFlags.SpacingReplacementInit, true);
			}
			mySpacingReplacement = value ?? string.Empty;
		}

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
		/// is top-level, or the first of its type in the hierarchy, or has settings
		/// that differ from its parent.
		/// </summary>
		/// <remarks>This is virtual, but only needs to be overridden if subtypes introduce
		/// additional properties to the standard name generator properties.</remarks>
		protected virtual bool RequiresSerialization()
		{
			// We always serialize the first of the typed generators. The top level does not exist without these subtypes,
			// so it is always serialized regardless of default attribute settings (see comment in FullyPopulateRefinements
			// for reasoning on why the highest of the subtypes is always serialized).
			// Lower levels are serialized only if they have settings that differ from the parents, or if they have children
			// that require serialization. Note that this means 'alternate defaults' are serialized if they differ from the
			// parent settings, but not otherwise.
			NameGenerator refinedGenerator = RefinesGenerator; // No refined generator means the top-level NameGenerator
			bool allPropertiesDefault;
			if (refinedGenerator == null || (myUsageDomainClass == null && RefinedInstance == null))
			{
				return true;
				// All generators except the top level one (this class itself with no subtype) refine
				// another generator. We know this level has no ignored properties or alternate defaults,
				// so use the basic check.
			}

			// Save is not needed for an individual property if:
			// 1) The the property is ignored (including both the fixed ignored properties and the dynamic additions)
			// 2) The property matches the nearest refining instance that does not ignore it. Note that dynamic ignorable
			//    property values are initialized and propagated, just not displayed. This means that the nearest refined
			//    generator will have the same value for a dynamically ignored property as the nearest generator that does
			//    not ignore the setting. Downstream generators should always ignore a superset of the parent's ignored properties.
			StandardNameGeneratorProperty ignoredProperties = CurrentIgnoredStandardProperties;
			allPropertiesDefault = (0 != (ignoredProperties & StandardNameGeneratorProperty.CasingOption) || CasingOption == refinedGenerator.CasingOption) &&
				(0 != (ignoredProperties & StandardNameGeneratorProperty.SpacingFormat) || SpacingFormat == refinedGenerator.SpacingFormat) &&
				(0 != (ignoredProperties & StandardNameGeneratorProperty.SpacingReplacement) || SpacingReplacement == refinedGenerator.SpacingReplacement) &&
				(0 != (ignoredProperties & StandardNameGeneratorProperty.AutomaticallyShortenNames) || AutomaticallyShortenNames == refinedGenerator.AutomaticallyShortenNames) &&
				(0 != (ignoredProperties & StandardNameGeneratorProperty.UserDefinedMaximum) || UserDefinedMaximum == refinedGenerator.UserDefinedMaximum) &&
				(0 != (ignoredProperties & StandardNameGeneratorProperty.UseTargetDefaultMaximum) || UseTargetDefaultMaximum == refinedGenerator.UseTargetDefaultMaximum);

			if (allPropertiesDefault)
			{
				foreach (NameGenerator refinement in RefinedByGeneratorCollection)
				{
					if (refinement.RequiresSerialization())
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
		/// <summary>
		/// Allow a derived name generator to ignore one or more stock properties. Returns <see cref="StandardNameGeneratorProperty.None"/> by default.
		/// Ignored properties are not displayed, serialized or automatically modified in response to parent changes.
		/// </summary>
		/// <remarks>No runtime effort is made to reconcile ignored properties between parents and children.
		/// A child item should include all ignored items on the refined instance.</remarks>
		public virtual StandardNameGeneratorProperty IgnoredStandardProperties
		{
			get
			{
				return StandardNameGeneratorProperty.None;
			}
		}
		/// <summary>
		/// Combines <see cref="IgnoredStandardProperties"/> with other properties that are hidden because
		/// other properties must be set to see them.
		/// </summary>
		/// <remarks>Temporarily ignored properties are as follows:
		/// -If SpacingFormat is ignored or not Replace, then SpacingReplacement is hidden.</remarks>
		/// -If AutomaticallyShortenNames is ignored or false, then UseTargetDefaultMaximum and UserDefinedMaximum are ignored.
		/// -If UseTargetDefaultMaximum not ignored and true then UserDefinedMaximum is hidden.
		private StandardNameGeneratorProperty CurrentIgnoredStandardProperties
		{
			get
			{
				StandardNameGeneratorProperty ignoredProperties = IgnoredStandardProperties;
				if (0 == (ignoredProperties & StandardNameGeneratorProperty.SpacingReplacement) &&
					(0 != (ignoredProperties & StandardNameGeneratorProperty.SpacingFormat) || GetSpacingFormatValue() != NameGeneratorSpacingFormat.ReplaceWith))
				{
					ignoredProperties |= StandardNameGeneratorProperty.SpacingReplacement;
				}

				if (0 != (ignoredProperties & StandardNameGeneratorProperty.AutomaticallyShortenNames) ||
					!GetAutomaticallyShortenNamesValue())
				{
					ignoredProperties |= StandardNameGeneratorProperty.UseTargetDefaultMaximum | StandardNameGeneratorProperty.UserDefinedMaximum;
				}
				else if (0 == (ignoredProperties & StandardNameGeneratorProperty.UseTargetDefaultMaximum) || GetUseTargetDefaultMaximumValue())
				{
					ignoredProperties |= StandardNameGeneratorProperty.UserDefinedMaximum;
				}
				return ignoredProperties;
			}
		}
		/// <summary>
		/// Specify the properties that are given alternative default settings when
		/// the NameGenerator subtype is initialized. Individual property fields
		/// will be sent to <see cref="GetAlternateDefaultStandardPropertyValue"/> to
		/// resolve the alternate value.
		/// </summary>
		protected virtual StandardNameGeneratorProperty AlternateDefaultStandardProperties
		{
			get
			{
				return StandardNameGeneratorProperty.None;
			}
		}
		/// <summary>
		/// Companion function to <see cref="AlternateDefaultStandardProperties"/> used to get
		/// the alternate default value for a property listed by the other function.
		/// </summary>
		/// <param name="property">A standard property value representing the property to return.</param>
		/// <returns>Alternate default value</returns>
		/// <remarks>Alternates for <see cref="AutomaticallyShortenNames"/> and <see cref="UseTargetDefaultMaximum"/> should return
		/// values of type <see cref="NameGeneratorUninitializedBoolean"/> instead of <see cref="System.Boolean"/>.</remarks>
		protected virtual object GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty property)
		{
			return null;
		}
		/// <summary>
		/// Is a stock property id always ignored for this type of name generator?
		/// </summary>
		public bool IsIgnoredStandardPropertyId(Guid propertyId, bool includeCurrent)
		{
			StandardNameGeneratorProperty ignoredProperties = includeCurrent ? CurrentIgnoredStandardProperties : IgnoredStandardProperties;
			if (0 == ignoredProperties)
			{
				return false;
			}

			return IsMatchedPropertyId(ignoredProperties, propertyId);
		}
		private static Guid[] myStandardPropertyIds;
		private static Guid[] StandardPropertyIds
		{
			get
			{
				Guid[] standardPropertyIds = myStandardPropertyIds;
				if (standardPropertyIds == null)
				{
					standardPropertyIds = new Guid[] { CasingOptionDomainPropertyId, SpacingFormatDomainPropertyId, SpacingReplacementDomainPropertyId, AutomaticallyShortenNamesDomainPropertyId, UserDefinedMaximumDomainPropertyId, UseTargetDefaultMaximumDomainPropertyId };
					if (null != System.Threading.Interlocked.CompareExchange<Guid[]>(ref myStandardPropertyIds, standardPropertyIds, null))
					{
						// Some other thread beat us, use the prior values
						standardPropertyIds = myStandardPropertyIds;
					}
				}
				return standardPropertyIds;
			}
		}
		private static bool IsMatchedPropertyId(StandardNameGeneratorProperty standardProperties, Guid domainPropertyId)
		{
			Guid[] standardPropertyIds = StandardPropertyIds;
			int i = 0;
			int remainingProps = (int)standardProperties;
			do
			{
				if (0 != (remainingProps & 1))
				{
					if (domainPropertyId == standardPropertyIds[i])
					{
						return true;
					}
				}
				remainingProps = remainingProps >> 1;
				++i;
			} while (remainingProps != 0 && i < 6);
			return false;
		}
		/// <summary>
		/// Get the top-level default value for a standard property.
		/// </summary>
		/// <returns>An instance, or null if the requested property id is not a standard value.</returns>
		public static object GetStandardPropertyDefaultValue(Guid domainPropertyId)
		{
			switch (Array.IndexOf<Guid>(StandardPropertyIds, domainPropertyId))
			{
				case 0: // CasingOptionDomainPropertyId
					return NameGeneratorCasingOption.None;
				case 1: // SpacingFormatDomainPropertyId
					return NameGeneratorSpacingFormat.Retain;
				case 2: // SpacingReplacementDomainPropertyId
					return string.Empty;
				case 3: // AutomaticallyShortenNamesDomainPropertyId
					return true;
				case 4: // UserDefinedMaximumDomainPropertyId
					return 128;
				case 5: // UseTargetDefaultMaximumDomainPropertyId
					return true;
				default:
					return null;
			}
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

					if (refinement.IsIgnoredStandardPropertyId(propertyInfo.Id, false)) // The current skipped settings are not reliable until the current settings are propagated
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
				bool hasDescendants = store.DomainDataDirectory.GetDomainClass(NameGenerator.DomainClassId).LocalDescendants.Count != 0;
				if (topGenerators.Count == 0)
				{
					if (hasDescendants)
					{
						topGenerator = new NameGenerator(store);
					}
				}
				else
				{
					foreach (NameGenerator generator in topGenerators)
					{
						if (hasDescendants)
						{
							topGenerator = generator;
						}
						else
						{
							// Removing extensions with generators will remove elements with
							// those xml namespaces but not the root element, which will load
							// but not be used.
							generator.Delete();
						}
						break;
					}
				}
				if (topGenerator != null)
				{
					StandardNameGeneratorProperty preInitializedProperties = topGenerator.InitializedProperties;
					topGenerator.FinishPropertyInitialization(null, StandardNameGeneratorProperty.None); // Second argument not used with root generator, which has no parent
					topGenerator.FullyPopulateRefinements(true, false, preInitializedProperties);
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
		private delegate void LinkRefinedInstance(NameGenerator owner, ModelElement refinedInstance);
		/// <summary>
		/// This guarantees that we have an instance for every possible Name Generator type and that properties that were not
		/// deserialized (and hence not initialized) have the correct default values filled in.
		/// </summary>
		private void FullyPopulateRefinements(bool topLevel, bool newInstance, StandardNameGeneratorProperty originalInitialized)
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
			LinkRefinedInstance linkRefinedInstanceCallback = null;
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
						linkRefinedInstanceCallback = delegate (NameGenerator refinementParent, ModelElement refinedInstance)
						{
							foreach (ElementLink link in refinedInstanceRole.GetElementLinks(refinedInstance))
							{
								NameGenerator linkedGenerator = ((NameGeneratorRefinesInstance)link).NameGenerator;
								if (linkedGenerator.RefinesGenerator == refinementParent)
								{
									linkedGenerator.FinishPropertyInitialization(refinementParent, (StandardNameGeneratorProperty)(-1));
									return;
								}
							}

							NameGenerator refinedGenerator = refinementParent.CreateRefinement(contextDomainClass, refinementParent.NameUsageType);
							NameGeneratorRefinesInstance typedLink = (NameGeneratorRefinesInstance)store.ElementFactory.CreateElementLink(refiningRelationship, new RoleAssignment(refinedGeneratorRole.Id, refinedGenerator), new RoleAssignment(refinedInstanceRole.Id, refinedInstance));
							refinedGenerator.FinishPropertyInitialization(refinementParent, (StandardNameGeneratorProperty)(-1));
						};
					}
				}
			}

			if (linkRefinedInstanceCallback != null && (requiredUsageCount == 0 || NameUsageType != null))
			{
				for (int j = 0; j < refinedInstancesCount; ++j)
				{
					linkRefinedInstanceCallback(this, refinedInstances[j]);
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
						StandardNameGeneratorProperty preInitializedProperties = currentChild.InitializedProperties;
						currentChild.FinishPropertyInitialization(this, originalInitialized);
						currentChild.FullyPopulateRefinements(false, false, preInitializedProperties);
					}
					else
					{
						currentChild.FinishPropertyInitialization(this, originalInitialized);
						if (linkRefinedInstanceCallback != null)
						{
							for (int j = 0; j < refinedInstancesCount; ++j)
							{
								linkRefinedInstanceCallback(currentChild, refinedInstances[j]);
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
							NameGenerator newGenerator = CreateRefinement(classInfo, null);
							// This is a new creation of the highest instance of a given type of
							// generator. Other refinements for name usage and refined instances
							// use the earlier codepaths. If the top-level generator was added because
							// the first extension with a NameGenerator subtype was added then the
							// originalInitialized settings here will be empty. However, if the root
							// generator was already in use for a different extension then we want to
							// see the new defaults for this new type, even if they differ from the
							// established root. After this point when the user resets a property value
							// the defaults (default or alternate) are ignored in favor of the refined
							// NamedGenerator, so this is the only chance the user has to see the defaults
							// intended for this type.
							// To accomodate this design, we do 3 things:
							// 1) Default properties at the root are not serialized.
							// 2) The type transition (this element) is always saved even if there are no
							//    non-default properties. This prevents this reinitialization of original
							//    defaults for the type.
							// 3) This instance (no usage or refined instances) is saved even if none of the
							//    generation defaults differ.
							// 4) Properties are serialized at top-level if they are not default and at
							//    every other level if they differ from the parent.
							// 5) At this point (initial creation of the general case for this generator with
							//    no name usage or refined instance) we reset the original initialized properties
							//    to not initialized to allow the initial creation to use its own defaults.
							newGenerator.FinishPropertyInitialization(this, StandardNameGeneratorProperty.None);
							newGenerator.FullyPopulateRefinements(false, true, StandardNameGeneratorProperty.None);
							if (linkRefinedInstanceCallback != null)
							{
								for (int j = 0; j < refinedInstancesCount; ++j)
								{
									linkRefinedInstanceCallback(newGenerator, refinedInstances[j]);
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
							NameGenerator newGenerator = CreateRefinement(contextDomainClass, usageType);
							newGenerator.FinishPropertyInitialization(this, originalInitialized);
							if (linkRefinedInstanceCallback != null)
							{
								for (int j = 0; j < refinedInstancesCount; ++j)
								{
									linkRefinedInstanceCallback(newGenerator, refinedInstances[j]);
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
					link.Delete(); // Delete propagates to refining generator

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
		/// Create a refinement of the specified type
		/// </summary>
		/// <param name="childType">A <see cref="DomainClassInfo"/> of the current type if <paramref name="nameUsageType"/> is set.
		/// Otherwise, the <see cref="DomainClassInfo.BaseDomainClass"/> must equal the current <see cref="ModelElement.GetDomainClass">DomainClass</see>.</param>
		/// <param name="nameUsageType">The type to associate with the <see cref="NameUsage"/> property. Can be <see langword="null"/></param>
		/// <returns>A new <see cref="NameGenerator"/> of the specified type</returns>
		private NameGenerator CreateRefinement(DomainClassInfo childType, Type nameUsageType)
		{
			NameGenerator retVal = (NameGenerator)Store.GetDomainModel(childType.DomainModel.Id).CreateElement(Partition, childType.ImplementationClass, nameUsageType != null ? new PropertyAssignment[] { new PropertyAssignment(NameUsageDomainPropertyId, ObjectModel.NameUsage.TranslateToNameUsageIdentifier(Partition.DomainDataDirectory.FindDomainClass(nameUsageType))) } : new PropertyAssignment[0]);
			retVal.RefinesGenerator = this;
			return retVal;
		}
		/// <summary>
		/// Update any uninitialized properties. This is called after the element is otherwise constructed and attached,
		/// so the refining generator, refining instance and NameUsage properties are all set.
		/// </summary>
		/// <param name="parent">The refining generator (known to caller, saves a call)</param>
		/// <param name="parentOriginalInitialized">The original initialized state of the parent generator. If the
		/// parent property was not originally initialized, then the same property here will use an alternate default
		/// if available. If an alternate default is not available the local uninitialized properties will be copied
		/// from the parent. Pass in (StandardNameGeneratorProperty)(-1) to indicate all properties are initialized.</param>
		/// <remarks>This will only be overridden if the subtype adds additional properties. In this case, the recommendation is
		/// to represent these properties as flags with higher bits than the standard properties and downcast the resulting
		/// enum to the standard. This will also need to be done with <see cref="InitializedProperties"/>, which is
		/// where the initialized setting is retrieved from.</remarks>
		protected virtual void FinishPropertyInitialization(NameGenerator parent, StandardNameGeneratorProperty parentOriginalInitialized)
		{
			// We'll assume that refined instances will have stronger Ignored settings. This is a reasonable assumption since
			// in all expected cases the descendant refinements all of the same type and differ by NameUsage and RefiningInstance.

			// Treat ignored as initialized
			StandardNameGeneratorProperty initialized = InitializedProperties | IgnoredStandardProperties;

			// Do not use the alternate default if the parent value was initialized before this routine was applied to it
			StandardNameGeneratorProperty alternateDefaults = AlternateDefaultStandardProperties & ~parentOriginalInitialized;

			// Note that we set uninitialized properties unless they are always ignored. Dynamically
			// ignored properties may be turned on later and must be initialized at that time.
			if (0 == (initialized & StandardNameGeneratorProperty.CasingOption))
			{
				CasingOption = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.CasingOption) ?
						(NameGeneratorCasingOption)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.CasingOption) :
						parent.CasingOption) :
					NameGeneratorCasingOption.None;
			}

			if (0 == (initialized & StandardNameGeneratorProperty.SpacingFormat))
			{
				SpacingFormat = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.SpacingFormat) ?
						(NameGeneratorSpacingFormat)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.SpacingFormat) :
						parent.SpacingFormat) :
					NameGeneratorSpacingFormat.Retain;
			}

			if (0 == (initialized & StandardNameGeneratorProperty.SpacingReplacement))
			{
				SpacingReplacement = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.SpacingReplacement) ?
						(string)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.SpacingReplacement) :
						parent.SpacingReplacement) :
					string.Empty;
			}

			if (0 == (initialized & StandardNameGeneratorProperty.AutomaticallyShortenNames))
			{
				AutomaticallyShortenNamesInitializer = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.AutomaticallyShortenNames) ?
						(NameGeneratorUninitializedBoolean)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.AutomaticallyShortenNames) :
						parent.AutomaticallyShortenNamesInitializer) :
					NameGeneratorUninitializedBoolean.@true;
			}

			if (0 == (initialized & StandardNameGeneratorProperty.UseTargetDefaultMaximum))
			{
				UseTargetDefaultMaximumInitializer = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.UseTargetDefaultMaximum) ?
						(NameGeneratorUninitializedBoolean)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.UseTargetDefaultMaximum) :
						parent.UseTargetDefaultMaximumInitializer) :
					NameGeneratorUninitializedBoolean.@true;
			}

			if (0 == (initialized & StandardNameGeneratorProperty.UserDefinedMaximum))
			{
				UserDefinedMaximum = parent != null ?
					(0 != (alternateDefaults & StandardNameGeneratorProperty.UserDefinedMaximum) ?
						(int)GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty.UserDefinedMaximum) :
						parent.UserDefinedMaximum) :
					128;
			}
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
				NameGenerator refinement;
				if (primaryGenerator.NameUsageTypes.Length == 0)
				{
					onCreated(refinement = primaryGenerator.CreateRefinement(domainClass, null));
					refinement.FinishPropertyInitialization(primaryGenerator, (StandardNameGeneratorProperty)(-1));
				}
				else
				{
					foreach (NameGenerator generatorWithUsageType in primaryGenerator.RefinedByGeneratorCollection)
					{
						onCreated(refinement = generatorWithUsageType.CreateRefinement(domainClass, generatorWithUsageType.NameUsageType));
						refinement.FinishPropertyInitialization(primaryGenerator, (StandardNameGeneratorProperty)(-1));
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
