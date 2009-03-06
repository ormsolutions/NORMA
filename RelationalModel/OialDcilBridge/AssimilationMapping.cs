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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Diagnostics;
using System.Security.Permissions;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region ObjectTypeAbsorptionChoice enum
	/// <summary>
	/// Derives options for <see cref="ObjectType"/> absorption based on current
	/// settings of properties on related assimilations.
	/// </summary>
	[TypeConverter(typeof(EnumConverter<ObjectTypeAbsorptionChoice, MappingCustomizationModel>))]
	public enum ObjectTypeAbsorptionChoice
	{
		/// <summary>
		/// The associated <see cref="ObjectType"/> is absorbed through all paths
		/// </summary>
		Absorbed = 0,
		/// <summary>
		/// Each subtype is given its own table with duplicated data from this <see cref="ObjectType"/>
		/// </summary>
		Partitioned = 1,
		/// <summary>
		/// This <see cref="ObjectType"/> receives its own table.
		/// </summary>
		Separate = 2,
		/// <summary>
		/// This <see cref="ObjectType"/> is absorbed, but the absorption
		/// path is customized on the individual supertype relationships
		/// </summary>
		CustomAbsorbed = 3,
		/// <summary>
		/// Displayed as an option for removing a partitioned state. Setting
		/// to this value acts as a command to remove the partition from
		/// the <see cref="ObjectType"/> and leaving all paritioned subtypes
		/// that are not themselves partitioned in a separated state.
		/// </summary>
		UnpartitionAbsorb = 4,
		/// <summary>
		/// Displayed as an option for removing a partitioned state. Setting
		/// to this value acts as a command to remove the partition from
		/// the <see cref="ObjectType"/> and leaving all paritioned subtypes
		/// in a separated state.
		/// </summary>
		UnpartitionSeparate = 5,
		/// <summary>
		/// This <see cref="ObjectType"/> is a child of a partitioned parent
		/// </summary>
		PartitionedChild = 6,
		/// <summary>
		/// There are no subtyping relationship on the current <see cref="ObjectType"/>
		/// that make an absorption choice relevant
		/// </summary>
		NotApplicable = 7,
	}
	#endregion // ObjectTypeAbsorptionChoice enum
	partial class AssimilationMapping
	{
		#region Extension property provider callbacks
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to a <see cref="FactType"/>
		/// </summary>
		public static void PopulateAssimilationMappingExtensionProperties(ModelElement extendableElement, PropertyDescriptorCollection properties)
		{
			FactType factType;
			if (null != (factType = extendableElement as FactType) &&
				!factType.IsDeleted &&
				IsFactTypeAssociatedWithDeepAssimilationsOnly(factType))
			{
				properties.Add(AssimilationMappingPropertyDescriptor.Instance);
			}
		}
		/// <summary>
		/// An <see cref="PropertyProvider"/> callback for adding extender properties to an <see cref="ObjectType"/>
		/// </summary>
		public static void PopulateObjectTypeAbsorptionExtensionProperties(ModelElement extendableElement, PropertyDescriptorCollection properties)
		{
			ObjectType objectType;
			ConceptType conceptType;
			if (null != (objectType = extendableElement as ObjectType) &&
				!objectType.IsDeleted &&
				null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)))
			{
				bool shouldAddProperty = false;
				if (ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType).Count != 0)
				{
					shouldAddProperty = true;
				}
				else
				{
					ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType);
					if (assimilations.Count != 0)
					{
						LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilations[0]);
						if (factTypes.Count == 1)
						{
							FactType factType = factTypes[0];
							if (GetAbsorptionChoiceFromFactType(factType) == AssimilationAbsorptionChoice.Partition)
							{
								shouldAddProperty = true;
							}
							else
							{
								Role towardsRoleDummy;
								LinkedElementCollection<Role> disjunctiveMandatoryRoles = GetDisjunctiveMandatoryRoles(factType, out towardsRoleDummy);
								if (disjunctiveMandatoryRoles != null)
								{
									shouldAddProperty = CanPartitionAssimilations(assimilations, disjunctiveMandatoryRoles);
								}
							}
						}
					}
				}
				if (shouldAddProperty)
				{
					properties.Add(ObjectTypeAbsorptionPropertyDescriptor.Instance);
				}
			}
		}
		#endregion // Extension property provider callbacks
		#region Static helper functions
		/// <summary>
		/// Given a <see cref="ConceptTypeAssimilatesConceptType"/> relationship, determine the current
		/// <see cref="AssimilationAbsorptionChoice"/> for that assimilation
		/// </summary>
		public static AssimilationAbsorptionChoice GetAbsorptionChoiceFromAssimilation(ConceptTypeAssimilatesConceptType assimilation)
		{
			LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilation);
			return (factTypes.Count == 1) ?
				GetAbsorptionChoiceFromFactType(factTypes[0]) :
				AssimilationAbsorptionChoice.Absorb;
		}
		/// <summary>
		/// Given a <see cref="ConceptTypeAssimilatesConceptType"/> relationship, determine the current
		/// <see cref="AssimilationMapping"/> for that assimilation
		/// </summary>
		public static AssimilationMapping GetAssimilationMappingFromAssimilation(ConceptTypeAssimilatesConceptType assimilation)
		{
			LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilation);
			return (factTypes.Count == 1) ?
				AssimilationMappingCustomizesFactType.GetAssimilationMapping(factTypes[0]) :
				null;
		}
		/// <summary>
		/// Given a <see cref="FactType"/>, determine the stored <see cref="AssimilationAbsorptionChoice"/>.
		/// Note that this does not check if this is an appropriate value for the current assimilation state
		/// of the FactType.
		/// </summary>
		private static AssimilationAbsorptionChoice GetAbsorptionChoiceFromFactType(FactType factType)
		{
			AssimilationMapping mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(factType);
			return (null != mapping) ?
				mapping.AbsorptionChoice :
				AssimilationMapping.GetDefaultAbsorptionChoice(factType);
		}
		/// <summary>
		/// Given a <see cref="FactType"/>, return an associated <see cref="ConceptTypeAssimilatesConceptType"/>
		/// </summary>
		private static ConceptTypeAssimilatesConceptType GetAssimilationFromFactType(FactType factType)
		{
			if (factType != null)
			{
				foreach (ConceptTypeChildHasPathFactType pathLink in ConceptTypeChildHasPathFactType.GetLinksToConceptTypeChild(factType))
				{
					// UNDONE: Is there any guarantee that there is only one of these?
					ConceptTypeAssimilatesConceptType assimilation = pathLink.ConceptTypeChild as ConceptTypeAssimilatesConceptType;
					if (assimilation != null)
					{
						return assimilation;
					}
				}
			}
			return null;
		}
		/// <summary>
		/// Get the default absorption choice for a <paramref name="factType"/>
		/// </summary>
		/// <param name="factType">A <see cref="FactType"/> or <see cref="SubtypeFact"/></param>
		/// <returns><see cref="AssimilationAbsorptionChoice.Absorb"/> for a <see cref="SubtypeFact"/>, <see cref="AssimilationAbsorptionChoice.Separate"/> otherwise.</returns>
		public static AssimilationAbsorptionChoice GetDefaultAbsorptionChoice(FactType factType)
		{
			return (factType is SubtypeFact || factType.ImpliedByObjectification != null) ? AssimilationAbsorptionChoice.Absorb : AssimilationAbsorptionChoice.Separate;
		}
		/// <summary>
		/// Determine if a <see cref="FactType"/> is associated only with deeply mapped assimilations
		/// </summary>
		private static bool IsFactTypeAssociatedWithDeepAssimilationsOnly(FactType factType)
		{
			FactTypeMapsTowardsRole factTypeMapLink;
			bool retVal = false;
			if (null != factType &&
				null != (factTypeMapLink = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType)) &&
				factTypeMapLink.Depth == MappingDepth.Deep)
			{
				foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
				{
					if (!(child is ConceptTypeAssimilatesConceptType))
					{
						retVal = false;
						break;
					}
					retVal = true;
				}
			}
			return retVal;
		}
		#endregion // Static helper functions
		#region AssimilationMappingPropertyDescriptor class
		private sealed class AssimilationMappingPropertyDescriptor : PropertyDescriptor
		{
			#region AssimilationMappingEnumConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			public class AssimilationMappingEnumConverter : EnumConverter<AssimilationAbsorptionChoice, MappingCustomizationModel>
			{
				public static readonly AssimilationMappingEnumConverter Instance = new AssimilationMappingEnumConverter();
				private static readonly StandardValuesCollection AllValues = new StandardValuesCollection(new object[] { AssimilationAbsorptionChoice.Absorb, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Partition });
				private static readonly StandardValuesCollection AbsorbAndSeparateValues = new StandardValuesCollection(new object[] { AssimilationAbsorptionChoice.Absorb, AssimilationAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection SeparateValues = new StandardValuesCollection(new object[] { AssimilationAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection PartitionAndSeparateValues = new StandardValuesCollection(new object[] { AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Partition });
				private AssimilationMappingEnumConverter()
				{
				}
				/// <summary>
				/// Limit the items that are actually displayed
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					// Note that we'll only get here if the property descriptor is currently
					// being displayed. Therefore, we already know we're in a reasonable state
					// and do not need to ask prerequisite questions
					object instance;
					FactType factType;
					if (null != (instance = context.Instance) &&
						null != (factType = GetFactTypeFromComponent(instance)))
					{
						bool canPartition = VerifyCanPartitionFactType(factType, false);
						bool canAbsorb = VerifyCanAbsorbAssimilation(GetAssimilationFromFactType(factType), false);
						return canPartition ?
							(canAbsorb ? AllValues : PartitionAndSeparateValues) :
							canAbsorb ? AbsorbAndSeparateValues : SeparateValues;
					}
					return base.GetStandardValues(context);
				}
			}
			#endregion // AssimilationMappingEnumConverter
			public static readonly AssimilationMappingPropertyDescriptor Instance = new AssimilationMappingPropertyDescriptor();
			private AssimilationMappingPropertyDescriptor()
				: base("AssmilationMappingPropertyEditor", null)
			{
			}
			private static FactType GetFactTypeFromComponent(object component)
			{
				return ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ORMEditorUtility.ResolveContextFactType(component);
			}
			public override AttributeCollection Attributes
			{
				get
				{
					// Absorption properties are a poor mergeable candidate. There are too
					// many places where the same setting on multiple assimilations will cause
					// a contradiction within the same transaction
					return new AttributeCollection(new MergablePropertyAttribute(false));
				}
			}
			public override TypeConverter Converter
			{
				get
				{
					return AssimilationMappingEnumConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return null != GetFactTypeFromComponent(component);
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				FactType factType = GetFactTypeFromComponent(component);
				if (factType != null)
				{
					return GetAbsorptionChoiceFromFactType(factType) != GetDefaultAbsorptionChoice(factType);
				}
				return false;
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.AbsorptionChoicePropertyDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.AbsorptionChoicePropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				return GetAbsorptionChoiceFromFactType(GetFactTypeFromComponent(component));
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(FactType);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(AssimilationAbsorptionChoice);
				}
			}
			public sealed override void ResetValue(object component)
			{
				FactType factType = GetFactTypeFromComponent(component);
				if (factType != null)
				{
					SetValue(factType, GetDefaultAbsorptionChoice(factType));
				}
			}
			public sealed override void SetValue(object component, object value)
			{
				FactType factType = GetFactTypeFromComponent(component);
				if (factType != null)
				{
					SetValue(factType, (AssimilationAbsorptionChoice)value);
				}
			}
			private static void SetValue(FactType factType, AssimilationAbsorptionChoice newChoice)
			{
				Store store = factType.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.AbsorptionChoicePropertyTransactionName))
				{
					SetPrimaryAbsorptionChoice(factType, newChoice);
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // AssimilationMappingPropertyDescriptor class
		#region ObjectTypeAbsorptionPropertyDescriptor class
		private sealed class ObjectTypeAbsorptionPropertyDescriptor : PropertyDescriptor
		{
			#region ObjectTypeAbsorptionEnumConverter
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			public class ObjectTypeAbsorptionEnumConverter : EnumConverter<ObjectTypeAbsorptionChoice, MappingCustomizationModel>
			{
				public static readonly ObjectTypeAbsorptionEnumConverter Instance = new ObjectTypeAbsorptionEnumConverter();

				// Note that the values are explicitly ordered with special displayed
				// values (values we return from a get, but cannot set) first, then
				// in the absorb/separate/partition order. The special command values
				// for unpartitioning display after the partition.
				private static readonly StandardValuesCollection NotApplicableValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.NotApplicable });
				private static readonly StandardValuesCollection PartitionedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.Partitioned, ObjectTypeAbsorptionChoice.UnpartitionSeparate, ObjectTypeAbsorptionChoice.UnpartitionAbsorb });
				private static readonly StandardValuesCollection PartitionableNotApplicableValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.NotApplicable, ObjectTypeAbsorptionChoice.Partitioned });
				private static readonly StandardValuesCollection PartitionedChildValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.PartitionedChild });
				private static readonly StandardValuesCollection PartitionablePartitionedChildValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.PartitionedChild, ObjectTypeAbsorptionChoice.Partitioned });
				private static readonly StandardValuesCollection SeparateValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection SeparateAbsorbedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.Absorbed, ObjectTypeAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection SeparatePartitionedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.Separate, ObjectTypeAbsorptionChoice.Partitioned });
				private static readonly StandardValuesCollection SeparateAbsorbedPartitionedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.Absorbed, ObjectTypeAbsorptionChoice.Separate, ObjectTypeAbsorptionChoice.Partitioned });
				private static readonly StandardValuesCollection SeparateCustomAbsorbedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.CustomAbsorbed, ObjectTypeAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection SeparateCustomAbsorbedPartitionedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.CustomAbsorbed, ObjectTypeAbsorptionChoice.Separate, ObjectTypeAbsorptionChoice.Partitioned });
				private static readonly StandardValuesCollection SeparateCustomAbsorbedAbsorbedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.CustomAbsorbed, ObjectTypeAbsorptionChoice.Absorbed, ObjectTypeAbsorptionChoice.Separate });
				private static readonly StandardValuesCollection SeparateCustomAbsorbedAbsorbedPartitionedValues = new StandardValuesCollection(new object[] { ObjectTypeAbsorptionChoice.CustomAbsorbed, ObjectTypeAbsorptionChoice.Absorbed, ObjectTypeAbsorptionChoice.Separate, ObjectTypeAbsorptionChoice.Partitioned });
				private ObjectTypeAbsorptionEnumConverter()
				{
				}
				/// <summary>
				/// Limit the items that are actually displayed
				/// </summary>
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					object instance;
					ObjectType objectType;
					ConceptType conceptType;
					if (null != (instance = context.Instance) &&
						null != (objectType = GetObjectTypeFromComponent(instance)) &&
						null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)))
					{
						bool canPartition = false;
						// Figure out the down side (are we partitioned/partitionable)
						ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType);
						if (assimilations.Count != 0)
						{
							LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilations[0]);
							if (factTypes.Count == 1)
							{
								FactType factType = factTypes[0];
								if (GetAbsorptionChoiceFromFactType(factType) == AssimilationAbsorptionChoice.Partition)
								{
									// If one assimilation is partitioned, it will be set on all of them.
									// The only thing we can reasonably do when we're partitioned is unpartition.
									return PartitionedValues;
								}
								else
								{
									Role towardsRoleDummy;
									LinkedElementCollection<Role> disjunctiveMandatoryRoles = GetDisjunctiveMandatoryRoles(factType, out towardsRoleDummy);
									if (disjunctiveMandatoryRoles != null)
									{
										canPartition = CanPartitionAssimilations(assimilations, disjunctiveMandatoryRoles);
									}
								}
							}
						}

						// Figure out the rest of the current state by looking at things that assimilate us
						assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType);
						bool seenAbsorb = false;
						bool seenSeparation = false;
						bool customAbsorb = false;
						bool fullAbsorbAvailable = false;
						bool seenPartition = false;
						int assimilatorCount = assimilations.Count;
						if (assimilatorCount != 0)
						{
							foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
							{
								switch (GetAbsorptionChoiceFromAssimilation(assimilation))
								{
									case AssimilationAbsorptionChoice.Absorb:
										seenAbsorb = true;
										customAbsorb = seenSeparation;
										break;
									case AssimilationAbsorptionChoice.Separate:
										seenSeparation = true;
										customAbsorb = seenAbsorb;
										break;
									case AssimilationAbsorptionChoice.Partition:
										seenPartition = true;
										break;
								}
								if (customAbsorb)
								{
									break;
								}
							}
							if (seenPartition && !(seenAbsorb || seenSeparation))
							{
								return canPartition ? PartitionablePartitionedChildValues : PartitionedChildValues;
							}
							if (customAbsorb || !seenAbsorb)
							{
								fullAbsorbAvailable = null != AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilations, true, null);
							}
							else if (seenAbsorb)
							{
								fullAbsorbAvailable = true;
							}
							// We always offer separate, it's 0. Get a reasonable list by or'ing other values together
							switch ((fullAbsorbAvailable ? 1 : 0) | (canPartition ? 2 : 0) | (customAbsorb ? 4 : 0))
							{
								case 0: // separate
									return SeparateValues;
								case 1: // separate|absorbed
									return SeparateAbsorbedValues;
								case 2: // separate|partition
									return SeparatePartitionedValues;
								case 3: // separate|partition|absorb
									return SeparateAbsorbedPartitionedValues;
								case 4: // separate|customAbsorb
									return SeparateCustomAbsorbedValues;
								case 5: // separate|customAbsorb|absorb
									return SeparateCustomAbsorbedAbsorbedValues;
								case 6: // separate|customAbsorb|partition
									return SeparateCustomAbsorbedPartitionedValues;
								case 7: // separate|customAbsorb|absorb|partition
									return SeparateCustomAbsorbedAbsorbedPartitionedValues;
							}
						}
						else if (canPartition)
						{
							return PartitionableNotApplicableValues;
						}
						return NotApplicableValues;
					}
					return base.GetStandardValues(context);
				}
			}
			#endregion // ObjectTypeAbsorptionEnumConverter
			public static readonly ObjectTypeAbsorptionPropertyDescriptor Instance = new ObjectTypeAbsorptionPropertyDescriptor();
			private ObjectTypeAbsorptionPropertyDescriptor()
				: base("ObjectTypeAbsorptionPropertyEditor", null)
			{
			}
			private static ObjectType GetObjectTypeFromComponent(object component)
			{
				return EditorUtility.ResolveContextInstance(component, false) as ObjectType;
			}
			public override AttributeCollection Attributes
			{
				get
				{
					// Absorption properties are a poor mergeable candidate. There are too
					// many places where the same setting on multiple assimilations will cause
					// a contradiction within the same transaction
					return new AttributeCollection(new MergablePropertyAttribute(false));
				}
			}
			public override TypeConverter Converter
			{
				get
				{
					return ObjectTypeAbsorptionEnumConverter.Instance;
				}
			}
			public sealed override bool CanResetValue(object component)
			{
				return false;
			}
			public sealed override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			public sealed override string DisplayName
			{
				get
				{
					return ResourceStrings.ObjectTypeAbsorptionChoicePropertyDisplayName;
				}
			}
			public sealed override string Category
			{
				get
				{
					return ResourceStrings.MappingCustomizationPropertyCategory;
				}
			}
			public sealed override string Description
			{
				get
				{
					return ResourceStrings.ObjectTypeAbsorptionChoicePropertyDescription;
				}
			}
			public sealed override object GetValue(object component)
			{
				ObjectType objectType;
				ConceptType conceptType;
				if (null != (objectType = GetObjectTypeFromComponent(component)) &&
					!objectType.IsDeleted &&
					null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)))
				{
					// First see if we're partitioned. Partitioned downstream does not check
					// anthing else upstream
					ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType);
					if (assimilations.Count != 0 &&
						GetAbsorptionChoiceFromAssimilation(assimilations[0]) == AssimilationAbsorptionChoice.Partition)
					{
						return ObjectTypeAbsorptionChoice.Partitioned;
					}
					assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType);
					if (assimilations.Count != 0)
					{
						bool seenAbsorb = false;
						bool seenSeparation = false;
						bool customAbsorb = false;
						bool seenPartition = false;
						int assimilatorCount = assimilations.Count;
						if (assimilatorCount != 0)
						{
							foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
							{
								switch (GetAbsorptionChoiceFromAssimilation(assimilation))
								{
									case AssimilationAbsorptionChoice.Absorb:
										seenAbsorb = true;
										customAbsorb = seenSeparation;
										break;
									case AssimilationAbsorptionChoice.Separate:
										seenSeparation = true;
										customAbsorb = seenAbsorb;
										break;
									case AssimilationAbsorptionChoice.Partition:
										seenPartition = true;
										break;
								}
								if (customAbsorb)
								{
									return ObjectTypeAbsorptionChoice.CustomAbsorbed;
								}
							}
							return seenAbsorb ?
								ObjectTypeAbsorptionChoice.Absorbed :
								(seenPartition && !seenSeparation) ?
									ObjectTypeAbsorptionChoice.PartitionedChild :
									ObjectTypeAbsorptionChoice.Separate;
						}
					}
				}
				return ObjectTypeAbsorptionChoice.NotApplicable;
			}
			public sealed override Type ComponentType
			{
				get
				{
					return typeof(ObjectType);
				}
			}
			public sealed override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			public sealed override Type PropertyType
			{
				get
				{
					return typeof(ObjectTypeAbsorptionChoice);
				}
			}
			public sealed override void ResetValue(object component)
			{
				// CanResetValue returns false, nothing to do
			}
			public sealed override void SetValue(object component, object value)
			{
				ObjectType objectType = GetObjectTypeFromComponent(component);
				if (objectType != null)
				{
					SetValue(objectType, (ObjectTypeAbsorptionChoice)value);
				}
			}
			private static void SetValue(ObjectType objectType, ObjectTypeAbsorptionChoice newChoice)
			{
				using (Transaction t = objectType.Store.TransactionManager.BeginTransaction(ResourceStrings.AbsorptionChoicePropertyTransactionName))
				{
					switch (newChoice)
					{
						case ObjectTypeAbsorptionChoice.Absorbed:
							AbsorbObjectType(objectType);
							break;
						case ObjectTypeAbsorptionChoice.Separate:
							SeparateObjectType(objectType);
							break;
						case ObjectTypeAbsorptionChoice.Partitioned:
							PartitionObjectType(objectType);
							break;
						case ObjectTypeAbsorptionChoice.UnpartitionSeparate:
						case ObjectTypeAbsorptionChoice.UnpartitionAbsorb:
							UnpartitionObjectType(objectType, newChoice == ObjectTypeAbsorptionChoice.UnpartitionAbsorb);
							break;
						// Other choices are just for show, they are not actionable
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
			private static void PartitionObjectType(ObjectType objectType)
			{
				// Partition any child assimilation for automatic synchronization
				ConceptType conceptType;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations;
				LinkedElementCollection<FactType> factTypes;
				if (null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)) &&
					(assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType)).Count != 0 &&
					(factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilations[0])).Count == 1)
				{
					SetPrimaryAbsorptionChoice(factTypes[0], AssimilationAbsorptionChoice.Partition);
				}
			}
			private static void UnpartitionObjectType(ObjectType objectType, bool attemptAbsorption)
			{
				ConceptType conceptType;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations;
				if (null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)) &&
					(assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType)).Count != 0)
				{
					ConceptTypeAssimilatesConceptType candidateAssimilation = null;
					bool doAbsorb = false;
					foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
					{
						if (attemptAbsorption)
						{
							// Verify there is at least one assimilation we can absorb.
							// Otherwise, we just separate.
							if (VerifyCanAbsorbAssimilation(assimilation, false))
							{
								doAbsorb = true;
								candidateAssimilation = assimilation;
								break;
							}
							if (candidateAssimilation == null)
							{
								candidateAssimilation = assimilation;
							}
						}
						else
						{
							candidateAssimilation = assimilation;
							break;
						}
					}
					LinkedElementCollection<FactType> factTypes;
					if (candidateAssimilation != null &&
						(factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(candidateAssimilation)).Count == 1)
					{
						SetPrimaryAbsorptionChoice(factTypes[0], doAbsorb ? AssimilationAbsorptionChoice.Absorb : AssimilationAbsorptionChoice.Separate);
					}
				}
			}
			private static void SeparateObjectType(ObjectType objectType)
			{
				// Find assimilators that are not separated and separate them. Note
				// that we only need to do a primary absorption on one of them. Setting
				// the primary absorption verifies that downstream assimilations are
				// separated to ensure that multiple assimilations rejoin, but this
				// condition will only change when the last absorbed assimilator
				// is removed, so we only need to fire the expensive primary check once.

				// Note that we don't touched partitioned assimilators here. These are handled
				// by settings on the parent.
				ConceptType conceptType;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations;
				if (null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)) &&
					(assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType)).Count != 0)
				{
					ConceptTypeAssimilatesConceptType firstAbsorbedAssimilation = null;
					MappingCustomizationModel customizationModel = null;
					Type disabledChangeRuleType = null;
					try
					{
						foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
						{
							if (firstAbsorbedAssimilation == null)
							{
								if (GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
								{
									firstAbsorbedAssimilation = assimilation;
								}
								continue;
							}
							SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Absorb, null, ref customizationModel, ref disabledChangeRuleType);
						}
					}
					finally
					{
						if (disabledChangeRuleType != null)
						{
							objectType.Store.RuleManager.EnableRule(disabledChangeRuleType);
						}
					}
					LinkedElementCollection<FactType> factTypes;
					if (firstAbsorbedAssimilation != null &&
						(factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(firstAbsorbedAssimilation)).Count == 1)
					{
						SetPrimaryAbsorptionChoice(factTypes[0], AssimilationAbsorptionChoice.Separate);
					}
				}
			}
			private static void AbsorbObjectType(ObjectType objectType)
			{
				ConceptType conceptType;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations;
				if (null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)) &&
					(assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType)).Count != 0)
				{
					// We're only using secondary absorption choices here, so there is no backup condition
					// checking in the rules. Do some validation checking here.
					if (null == AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilations, true, null))
					{
						throw new InvalidOperationException(ResourceStrings.AssimilationMappingInvalidSeparationPatternForAbsorbException);
					}
					MappingCustomizationModel customizationModel = null;
					Type disabledChangeRuleType = null;
					try
					{
						foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
						{
							SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Absorb, AssimilationAbsorptionChoice.Separate, null, ref customizationModel, ref disabledChangeRuleType);
						}
					}
					finally
					{
						if (disabledChangeRuleType != null)
						{
							objectType.Store.RuleManager.EnableRule(disabledChangeRuleType);
						}
					}
				}
			}
		}
		#endregion // AssimilationMappingPropertyDescriptor class
		#region Assimilation pattern checking
		private struct AssimilatorTracker
		{
			#region Settings fields
			private bool myIgnoreAllAbsorptionChoices;
			private bool myIgnoreInitialAbsorptionChoices;
			private ConceptTypeAssimilatesConceptType myAlwaysAbsorbAssimilation;
			#endregion // Settings fields
			#region Constructors
			private AssimilatorTracker(
				bool ignoreAllAbsorptionChoices,
				bool ignoreInitialAbsorptionChoices,
				ConceptTypeAssimilatesConceptType alwaysAbsorbAssimilation)
			{
				myIgnoreAllAbsorptionChoices = ignoreAllAbsorptionChoices;
				myIgnoreInitialAbsorptionChoices = ignoreInitialAbsorptionChoices || ignoreAllAbsorptionChoices;
				myAlwaysAbsorbAssimilation = alwaysAbsorbAssimilation;
			}
			#endregion // Constructors
			#region Settings helper methods
			/// <summary>
			/// Return true if one of the initial assimilations should be treated as absorbed
			/// </summary>
			private bool TreatInitialAssimilationAsAbsorbed(ConceptTypeAssimilatesConceptType assimilation)
			{
				if (myAlwaysAbsorbAssimilation == assimilation)
				{
					return true;
				}
				AssimilationAbsorptionChoice choice = GetAbsorptionChoiceFromAssimilation(assimilation);
				return myIgnoreInitialAbsorptionChoices ?
					choice != AssimilationAbsorptionChoice.Partition :
					choice == AssimilationAbsorptionChoice.Absorb;
			}
			/// <summary>
			/// Return true if one of the non-initial assimilations should be treated as absorbed
			/// </summary>
			private bool TreatUpstreamAssimilationAsAbsorbed(ConceptTypeAssimilatesConceptType assimilation)
			{
				if (myAlwaysAbsorbAssimilation == assimilation)
				{
					return true;
				}
				AssimilationAbsorptionChoice choice = GetAbsorptionChoiceFromAssimilation(assimilation);
				return myIgnoreAllAbsorptionChoices ?
					choice != AssimilationAbsorptionChoice.Partition :
					choice == AssimilationAbsorptionChoice.Absorb;
			}
			#endregion // Settings helper methods
			#region Public accessor methods
			/// <summary>
			/// Unconditionally retrieve the nearest assimilating <see cref="ConceptType"/>
			/// </summary>
			/// <param name="conceptType">The <see cref="ConceptType"/> to start with</param>
			/// <returns>The <see cref="ConceptType"/> that is the nearest shared assimilator for
			/// all assimilators of the provided <paramref name="conceptType"/>. Can return <see langword="null"/></returns>
			public static ConceptType GetNearestAssimilatorConceptType(ConceptType conceptType)
			{
				return (new AssimilatorTracker(true, true, null)).GetAbsorbingAssimilatorConceptType(ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType));
			}
			/// <summary>
			/// Get the nearest absorbing <see cref="ConceptType"/> based on the current absorption settings
			/// of the assimilators.
			/// </summary>
			/// <param name="assimilationCollection">Initial assimilation relationships to walk. All assimilators should assimilate the same <see cref="ConceptType"/></param>
			/// <returns>The absorbing <see cref="ConceptType"/> that is the nearest shared assimilator for
			/// all assimilators in <paramref name="assimilationCollection"/>. Can return <see langword="null"/></returns>
			public static ConceptType GetNearestAbsorbingAssimilatorConceptType(IEnumerable<ConceptTypeAssimilatesConceptType> assimilationCollection)
			{
				return GetNearestAbsorbingAssimilatorConceptType(assimilationCollection, false, null);
			}
			/// <summary>
			/// Get the nearest absorbing <see cref="ConceptType"/> based on the current absorption settings
			/// of the assimilators.
			/// </summary>
			/// <param name="assimilationCollection">Initial assimilation relationships to walk. All assimilators should assimilate the same <see cref="ConceptType"/></param>
			/// <param name="ignoreInitialAbsorptionChoices">Treat separated assimilations as absorbed for the assimilators in <paramref name="assimilationCollection"/>. Note that partitioned assimilators are note treated as absorbed.</param>
			/// <param name="alwaysAbsorbAssimilation">Assume the provided <see cref="ConceptTypeAssimilatesConceptType">assimilation</see> absorbs, regardless of its current setting.</param>
			/// <returns>The absorbing <see cref="ConceptType"/> that is the nearest shared assimilator for
			/// all assimilators in <paramref name="assimilationCollection"/>. Can return <see langword="null"/></returns>
			public static ConceptType GetNearestAbsorbingAssimilatorConceptType(IEnumerable<ConceptTypeAssimilatesConceptType> assimilationCollection, bool ignoreInitialAbsorptionChoices, ConceptTypeAssimilatesConceptType alwaysAbsorbAssimilation)
			{
				return (new AssimilatorTracker(false, ignoreInitialAbsorptionChoices, alwaysAbsorbAssimilation)).GetAbsorbingAssimilatorConceptType(assimilationCollection);
			}
			#endregion // Public accessor methods
			#region Helper class for GetAbsorbingAssimilatorConceptType
			/// <summary>
			/// A small helper class for the <see cref="GetAbsorbingAssimilatorConceptType"/> function.
			/// Tracks how many times a given object has been visited while
			/// walking assimilators of a given object.
			/// </summary>
			private sealed class NearestAssimilatorConceptTypeNode
			{
				public NearestAssimilatorConceptTypeNode(ConceptType conceptType, int lastVisitedDuring)
				{
					ConceptType = conceptType;
					VisitCount = 1;
					LastVisitedDuring = lastVisitedDuring;
				}
				/// <summary>
				/// Recursively increment all VisitCount fields for this
				/// node and its assimilators
				/// </summary>
				/// <param name="dictionary">Dictionary containing assimilator nodes</param>
				/// <param name="currentVisitIndex">current visit index</param>
				public void IncrementVisitCounts(Dictionary<ConceptType, NearestAssimilatorConceptTypeNode> dictionary, int currentVisitIndex)
				{
					if (LastVisitedDuring != currentVisitIndex)
					{
						LastVisitedDuring = currentVisitIndex;
						++VisitCount;
						LinkedList<ConceptType> assmilators = AssimilatorNodes;
						if (assmilators != null)
						{
							foreach (ConceptType assimilator in assmilators)
							{
								dictionary[assimilator].IncrementVisitCounts(dictionary, currentVisitIndex);
							}
						}
					}
				}
				public delegate ObjectTypeVisitorResult NodeVisitor(NearestAssimilatorConceptTypeNode node);
				/// <summary>
				/// Walk all descendants of this object type
				/// </summary>
				/// <param name="dictionary">Dictionary containing other types</param>
				/// <param name="visitor"><see cref="NodeVisitor"/> callback</param>
				/// <returns>true if walk completed</returns>
				public bool WalkAssimilators(Dictionary<ConceptType, NearestAssimilatorConceptTypeNode> dictionary, NodeVisitor visitor)
				{
					ObjectTypeVisitorResult result = visitor(this);
					switch (result)
					{
						//case ObjectTypeVisitorResult.Continue:
						//    break;
						case ObjectTypeVisitorResult.SkipChildren:
							return true;
						case ObjectTypeVisitorResult.Stop:
							return false;
					}
					LinkedList<ConceptType> assimilators = AssimilatorNodes;
					if (assimilators != null)
					{
						foreach (ConceptType assimilator in assimilators)
						{
							if (!dictionary[assimilator].WalkAssimilators(dictionary, visitor))
							{
								return false;
							}
						}
					}
					return true;
				}
				/// <summary>
				/// The concept type being tracked
				/// </summary>
				public readonly ConceptType ConceptType;
				/// <summary>
				/// The number of times this node has been visited
				/// </summary>
				public int VisitCount;
				/// <summary>
				/// A linked list of assimilator nodes
				/// </summary>
				public LinkedList<ConceptType> AssimilatorNodes;
				/// <summary>
				/// An index specifying the last visit so we don't
				/// increment the VisitCount twice on one pass, or
				/// count a node reachable through two paths twice.
				/// </summary>
				public int LastVisitedDuring;
			}
			#endregion // Helper class for GetAbsorbingAssimilatorConceptType
			/// <summary>
			/// Get the <see cref="ConceptType"/> that assimilates the passed in set of
			/// assimilations, or <see langword="null"/> if there is no absorbing container.
			/// </summary>
			/// <param name="assimilationCollection">initial assimilation relationships to walk</param>
			/// <returns>A <see cref="ConceptType"/></returns>
			private ConceptType GetAbsorbingAssimilatorConceptType(IEnumerable<ConceptTypeAssimilatesConceptType> assimilationCollection)
			{
				// Notes and assumptions:
				// 1) All assimilations are deeply mapped towards the assimilator
				// 2) If more than one assimilator is specified, then they will
				// all be subtypes, which are guaranteed by the ORMElementGateway
				// in the ORMAbsorption bridge to rejoin somewhere up the chain.

				// Note: The code for this is based on ObjectType.GetNearestCompatibleTypes. However,
				// given the preconditions, we know we will only find one closest node.
				int currentAssimilationIndex = 0;
				int expectedVisitCount = 0;
				ConceptType firstConceptType = null;
				ConceptTypeAssimilatesConceptType firstAssimilation = null;
				Dictionary<ConceptType, NearestAssimilatorConceptTypeNode> dictionary = null;
				foreach (ConceptTypeAssimilatesConceptType currentAssimilation in assimilationCollection)
				{
					if (!TreatInitialAssimilationAsAbsorbed(currentAssimilation))
					{
						continue;
					}
					// Increment first so we can use with the LastVisitedDuring field. Otherwise,
					// this is not used
					++currentAssimilationIndex;
					ConceptType currentConceptType = currentAssimilation.AssimilatorConceptType;
					if (firstConceptType == null)
					{
						firstAssimilation = currentAssimilation;
						firstConceptType = currentConceptType;
					}
					else
					{
						if (expectedVisitCount == 0)
						{
							// Delay add the initial data to the set
							dictionary = new Dictionary<ConceptType, NearestAssimilatorConceptTypeNode>();
							WalkAssimilatorsForNearestAssimilatorConceptType(dictionary, firstConceptType, 1);
							expectedVisitCount = 1;
						}

						// Process the current element
						WalkAssimilatorsForNearestAssimilatorConceptType(dictionary, currentConceptType, currentAssimilationIndex);
						++expectedVisitCount;
					}
				}

				if (dictionary == null)
				{
					// We only found one, no need to look further
					return TreatInitialAssimilationAsAbsorbed(firstAssimilation) ? firstConceptType : null;
				}
				// Now we have our total visit counts, walk the elements to determine
				// how many candidate nodes we have.
				ConceptType retVal = null;
				foreach (ConceptTypeAssimilatesConceptType currentAssimilation in assimilationCollection)
				{
					NearestAssimilatorConceptTypeNode currentNode;
					if (dictionary.TryGetValue(currentAssimilation.AssimilatorConceptType, out currentNode))
					{
						currentNode.WalkAssimilators(
							dictionary,
							delegate(NearestAssimilatorConceptTypeNode node)
							{
								if (node.VisitCount == expectedVisitCount)
								{
									if (node.LastVisitedDuring > 0)
									{
										node.LastVisitedDuring = 0;
										node.WalkAssimilators(
											dictionary,
											delegate(NearestAssimilatorConceptTypeNode assimilatorNode)
											{
												if (assimilatorNode == node)
												{
													return ObjectTypeVisitorResult.Continue;
												}
												switch (assimilatorNode.LastVisitedDuring)
												{
													case 0:
														// We've been here before and may have used it as
														// the retVal, but we were wrong because it is a
														// supertype of a node at the same depth. There
														// is no need to walk the children because it has
														// already been done. retVal does not need
														// to be cleared here even if it is set.
														assimilatorNode.LastVisitedDuring = -1;
														return ObjectTypeVisitorResult.SkipChildren;
													case -1:
														// No additional modifications needed
														return ObjectTypeVisitorResult.SkipChildren;
													default:
														if (node.VisitCount == expectedVisitCount)
														{
															assimilatorNode.LastVisitedDuring = -1;
														}
														return ObjectTypeVisitorResult.Continue;
												}
											});
										retVal = node.ConceptType;
									}
									return ObjectTypeVisitorResult.SkipChildren;
								}
								return ObjectTypeVisitorResult.Continue;
							});
					}
				}
				return retVal;
			}
			/// <summary>
			/// Helper method for GetAbsorbingAssimilatorConceptType
			/// </summary>
			private void WalkAssimilatorsForNearestAssimilatorConceptType(Dictionary<ConceptType, NearestAssimilatorConceptTypeNode> dictionary, ConceptType conceptType, int currentVisitIndex)
			{
				NearestAssimilatorConceptTypeNode currentNode;
				if (dictionary.TryGetValue(conceptType, out currentNode))
				{
					currentNode.IncrementVisitCounts(dictionary, currentVisitIndex);
				}
				else
				{
					currentNode = new NearestAssimilatorConceptTypeNode(conceptType, currentVisitIndex);
					dictionary[conceptType] = currentNode;
					foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
					{
						if (!TreatUpstreamAssimilationAsAbsorbed(assimilation))
						{
							continue;
						}
						ConceptType assimilatorType = assimilation.AssimilatorConceptType;
						LinkedList<ConceptType> currentAssimilators = currentNode.AssimilatorNodes;
						if (currentAssimilators == null)
						{
							currentNode.AssimilatorNodes = currentAssimilators = new LinkedList<ConceptType>();
						}
						currentAssimilators.AddLast(new LinkedListNode<ConceptType>(assimilatorType));
						WalkAssimilatorsForNearestAssimilatorConceptType(dictionary, assimilatorType, currentVisitIndex);
					}
				}
			}
		}
		/// <summary>
		/// Helper method for <see cref="VerifyCanPartitionFactType"/> and <see cref="DelayValidatePartitionedAssimilator"/>
		/// If the <paramref name="factType"/> maps towards a role, then get the disjunctive mandatory
		/// roles associated with that role.
		/// </summary>
		/// <param name="factType">An assimilated <see cref="FactType"/></param>
		/// <param name="towardsRole">The <see cref="Role"/> the <paramref name="factType"/> maps towards</param>
		/// <returns>The roles from a disjunctive mandatory constraint, or <see langword="null"/></returns>
		private static LinkedElementCollection<Role> GetDisjunctiveMandatoryRoles(FactType factType, out Role towardsRole)
		{
			LinkedElementCollection<Role> disjunctiveMandatoryRoles = null;
			FactTypeMapsTowardsRole towardsRoleLink = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType);

			towardsRole = null;
			if (towardsRoleLink != null &&
				towardsRoleLink.Depth == MappingDepth.Deep)
			{
				towardsRole = towardsRoleLink.TowardsRole.Role;
				foreach (ConstraintRoleSequence constraintSequence in towardsRole.ConstraintRoleSequenceCollection)
				{
					MandatoryConstraint testMandatory = constraintSequence as MandatoryConstraint;
					if (testMandatory != null &&
						testMandatory.Modality == ConstraintModality.Alethic &&
						testMandatory.ExclusiveOrExclusionConstraint != null)
					{
						if (!ModelError.HasErrors(testMandatory, ModelErrorUses.None))
						{
							disjunctiveMandatoryRoles = testMandatory.RoleCollection;
						}
						break;
					}
				}
			}
			return disjunctiveMandatoryRoles;
		}
		/// <summary>
		/// Helper method for <see cref="VerifyCanPartitionFactType"/> and <see cref="DelayValidatePartitionedAssimilator"/>
		/// </summary>
		private static bool CanPartitionAssimilations(ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations, LinkedElementCollection<Role> disjunctiveMandatoryRoles)
		{
			bool canPartition = false;
			if (disjunctiveMandatoryRoles != null)
			{
				int rolePassedCount = 0;
				canPartition = true; // Prove otherwise
				foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
				{
					foreach (FactType factType in ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilation))
					{
						FactTypeMapsTowardsRole link = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType);
						if (link.Depth == MappingDepth.Deep) // UNDONE: Can the mapping depth just be asserted for an assimilated FactType?
						{
							if (!disjunctiveMandatoryRoles.Contains(link.TowardsRole.Role))
							{
								canPartition = false;
								break;
							}
							++rolePassedCount;
						}
					}
				}
				if (canPartition && rolePassedCount != disjunctiveMandatoryRoles.Count)
				{
					canPartition = false;
				}
			}
			return canPartition;
		}
		/// <summary>
		/// Is it possible to partition this <see cref="FactType"/> with the current
		/// assimilation pattern? Only the possibility of partition is verified. No
		/// attempt is made to determine if other related FactTypes have the correct
		/// settings (such as not absorbing away from the partitioned ConceptType) to
		/// allow partitioning.
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to validate</param>
		/// <param name="throwOnFailure">Throw an exception if this fails instead of returning <see langword="false"/></param>
		/// <returns><see langword="true"/> if the <paramref name="factType"/> can be partitioned</returns>
		private static bool VerifyCanPartitionFactType(FactType factType, bool throwOnFailure)
		{
			bool canPartition = false;
			// We only allow the partition if all of the deeply mapped assimilations
			// have target roles pointing to the same target as this one and all of
			// them are part of the same disjunctive mandatory constraint
			LinkedElementCollection<Role> disjunctiveMandatoryRoles;
			ObjectType objectType;
			ConceptType conceptType;
			Role towardsRole;
			if (null != (disjunctiveMandatoryRoles = GetDisjunctiveMandatoryRoles(factType, out towardsRole)) &&
				null != (objectType = towardsRole.RolePlayer) &&
				null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)))
			{
				canPartition = CanPartitionAssimilations(ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType), disjunctiveMandatoryRoles);
			}
			if (throwOnFailure && !canPartition)
			{
				throw new InvalidOperationException(ResourceStrings.AssimilationMappingInvalidPatternForPartitionException);
			}
			return canPartition;
		}
		private static bool VerifyCanAbsorbAssimilation(ConceptTypeAssimilatesConceptType assimilation, bool throwOnFailure)
		{
			if (assimilation == null)
			{
				return false;
			}
			ConceptType assimilatedConceptType = assimilation.AssimilatedConceptType;
			foreach (ConceptTypeAssimilatesConceptType childAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(assimilatedConceptType))
			{
				if (GetAbsorptionChoiceFromAssimilation(childAssimilation) == AssimilationAbsorptionChoice.Partition)
				{
					if (!throwOnFailure)
					{
						return false;
					}
					throw new InvalidOperationException(ResourceStrings.AssimilationMappingInvalidPartitionPatternForAbsorbException);
				}
				break;
			}
			ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilatedAssimilators = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(assimilatedConceptType);
			if (1 < assimilatedAssimilators.Count)
			{
				if (null == AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilatedAssimilators, false, assimilation))
				{
					if (!throwOnFailure)
					{
						return false;
					}
					// UNDONE: We can give a better error message here (get all possible (not just nearest) potential shared assimilators for the assimilator)
					throw new InvalidOperationException(ResourceStrings.AssimilationMappingInvalidSeparationPatternForAbsorbException);
				}
			}
			return true;
		}
		#endregion // Assimilation pattern checking
		#region AssimilationMapping Rule Methods
		/// <summary>
		/// AddRule: typeof(AssimilationMappingCustomizesFactType)
		/// Mapping options have been modified for a <see cref="FactType"/>
		/// </summary>
		private static void AssimilationMappingAddedRule(ElementAddedEventArgs e)
		{
			AssimilationMappingCustomizesFactType link = (AssimilationMappingCustomizesFactType)e.ModelElement;
			AssimilationMapping mapping = link.AssimilationMapping;
			// Adding the link to the MappingCustomizationModel before the link to the FactType
			// is a signal used by creation routines to indicate whether or not the add should be processed
			// or ignored.
			if (mapping.Model != null)
			{
				FactType factType = link.FactType;
				AssimilationAbsorptionChoice oldChoice = GetDefaultAbsorptionChoice(factType);
				if (oldChoice != mapping.AbsorptionChoice)
				{
					ProcessModifiedAbsorptionChoice(factType, mapping, oldChoice);
				}
			}
		}
		private static ConceptType GetAssimilatorConceptType(FactType factType)
		{
			FactTypeMapsTowardsRole towardsRoleLink;
			Role towardsRole;
			ObjectType rolePlayer;
			return (null != (towardsRoleLink = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType)) &&
				null != (towardsRole = towardsRoleLink.TowardsRole.Role) &&
				null != (rolePlayer = towardsRole.RolePlayer)) ?
				ConceptTypeIsForObjectType.GetConceptType(rolePlayer) :
				null;
		}
		/// <summary>
		/// Set the <see cref="AssimilationAbsorptionChoice"/> for the provided <see cref="FactType"/>
		/// and trigger all associated rules.
		/// </summary>
		private static void SetPrimaryAbsorptionChoice(FactType factType, AssimilationAbsorptionChoice newChoice)
		{
			AssimilationMapping mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(factType);
			if (mapping != null)
			{
				// Note that we don't delete these once they're created so that we
				// only have to deal with property changes, not deletions, when deciding
				// our course of acton.
				mapping.AbsorptionChoice = newChoice;
			}
			else if (newChoice != GetDefaultAbsorptionChoice(factType))
			{
				Store store = factType.Store;
				mapping = new AssimilationMapping(store, new PropertyAssignment(AssimilationMapping.AbsorptionChoiceDomainPropertyId, newChoice));
				// Add the model first tells the AssimilationMappingAddedRule method that we want it to process this element
				mapping.Model = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
				if (factType.IsDeleting)
				{
					// UNDONE: Helper to temporary hack workaround in FactTypeDeletingRule. We create these elements
					// while the FactType is deleting, so they are not automatically deleted at the appropriate time.
					FrameworkDomainModel.DelayValidateElement(new AssimilationMappingCustomizesFactType(mapping, factType), DelayValidateDeleteTemporaryAssimilationMapping);
				}
				else
				{
					mapping.FactType = factType;
				}
			}
		}
		private static void DelayValidateDeleteTemporaryAssimilationMapping(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				AssimilationMappingCustomizesFactType link = (AssimilationMappingCustomizesFactType)element;
				if (link.FactType.IsDeleted)
				{
					link.Delete();
				}
			}
		}
		/// <summary>
		/// Helper function for <see cref="ProcessModifiedAbsorptionChoice"/>. Does not fire additional
		/// rules for modified absorption choices.
		/// </summary>
		private static bool SetSecondaryAbsorptionChoice(ConceptTypeAssimilatesConceptType assimilation, AssimilationAbsorptionChoice newAbsorptionChoice, AssimilationAbsorptionChoice? requireOldAbsorptionChoice, AssimilationMapping skipMapping, ref MappingCustomizationModel customizationModel, ref Type disabledChangeRuleType)
		{
			bool retVal = false;
			Store store = assimilation.Store;
			foreach (FactType assimilatedFactType in ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilation))
			{
				AssimilationMapping mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(assimilatedFactType);
				if (skipMapping == null || mapping != skipMapping)
				{
					if (mapping != null)
					{
						if (!requireOldAbsorptionChoice.HasValue || (retVal = mapping.AbsorptionChoice == requireOldAbsorptionChoice.Value))
						{
							if (!retVal)
							{
								retVal = mapping.AbsorptionChoice != newAbsorptionChoice;
							}
							if (retVal)
							{
								if (disabledChangeRuleType == null)
								{
									store.RuleManager.DisableRule(disabledChangeRuleType = typeof(AssimilationMappingChangedRuleClass));
								}
								mapping.AbsorptionChoice = newAbsorptionChoice;
							}
						}
						if (customizationModel == null)
						{
							customizationModel = mapping.Model;
						}
					}
					else if (GetDefaultAbsorptionChoice(assimilatedFactType) != newAbsorptionChoice)
					{
						mapping = new AssimilationMapping(store, new PropertyAssignment(AssimilationMapping.AbsorptionChoiceDomainPropertyId, newAbsorptionChoice));
						mapping.FactType = assimilatedFactType;
						// Add the model last so that this does not interact with the AssimilationMappingAddedRule
						if (customizationModel == null)
						{
							customizationModel = MappingCustomizationModel.GetMappingCustomizationModel(store, true);
						}
						mapping.Model = customizationModel;
						retVal = true;
					}
				}
			}
			return retVal;
		}
		private static void ProcessModifiedAbsorptionChoice(FactType factType, AssimilationMapping currentMapping, AssimilationAbsorptionChoice oldChoice)
		{
			MappingCustomizationModel customizationModel = currentMapping.Model;
			if (customizationModel == null)
			{
				return;
			}
			Store store = customizationModel.Store;
			RuleManager ruleManager = store.RuleManager;
			Type disabledChangeRuleType = null;
			ConceptTypeAssimilatesConceptType modifiedAssimilation = null;
			bool handleRemainingPartitions = false;
			bool attemptRemainingPartitionAbsorption = false;
			try
			{
				AssimilationAbsorptionChoice newChoice = currentMapping.AbsorptionChoice;
				ConceptType conceptType;
				switch (newChoice)
				{
					case AssimilationAbsorptionChoice.Partition:
						// This will throw, control does not return if it fails
						VerifyCanPartitionFactType(factType, true);
						if (null != (conceptType = GetAssimilatorConceptType(factType)))
						{
							foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
							{
								SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Partition, null, currentMapping, ref customizationModel, ref disabledChangeRuleType);
							}
							// Any assimilation away cannot be absorbed for a partitioned concept type
							foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
							{
								SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Absorb, currentMapping, ref customizationModel, ref disabledChangeRuleType);
								// Note that any case that would cause the separation here to have side effects is
								// excluded by the partition pattern. The only time we would worry changing something
								// to separate is when the downstream types potentially come back together, but
								// this cannot happen with the exclusion in place that is required for partition
								// because this will lock out any downstream subtype that violates the constraint.
							}
						}
						break;
					case AssimilationAbsorptionChoice.Separate:
						if (null != (modifiedAssimilation = GetAssimilationFromFactType(factType)))
						{
							if (oldChoice == AssimilationAbsorptionChoice.Absorb)
							{
								// Design note:
								// We considered a design where changing from absorb to separate could also modify
								// the absorption choice for other assimilators of the assimilated concept type.
								// The reason for doing this is that changing to separate for one assimilator in a set
								// has no real meaning unless the other paths to the shared assimilator node are also
								// set to separate. The design was well intentioned (don't let the user do something
								// that has no net effect), but clumsy in principle because changing one assimilator to
								// not absorb could have a ripple effect through a large subtype graph, possible changing
								// a large percentage of the nodes.
								//
								// The implemented design only separates assimilated nodes if not separating would produce
								// a situation where a shared assimilator ConceptType cannot be reached.

								ConceptType modifiedAssimilatorConceptType = modifiedAssimilation.AssimilatorConceptType;
								Dictionary<ConceptTypeAssimilatesConceptType, object> downstreamAssimilationsDictionary = null;
								PopulateDownstreamAssimilations(modifiedAssimilation, true, ref downstreamAssimilationsDictionary);
								if (downstreamAssimilationsDictionary != null)
								{
									Dictionary<ConceptType, ConceptType> upstreamConceptTypesDictionary = null;
									PopulateUpstreamAbsorbingConceptTypes(modifiedAssimilatorConceptType, ref upstreamConceptTypesDictionary);
									foreach (ConceptTypeAssimilatesConceptType currentAssimilation in downstreamAssimilationsDictionary.Keys)
									{
										ConceptType assimilatedConceptType = currentAssimilation.AssimilatedConceptType;
										ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilatorLinks = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(assimilatedConceptType);
										ConceptType nearestAbsorbingConceptType = AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilatorLinks, false, modifiedAssimilation);
										if (((modifiedAssimilatorConceptType == nearestAbsorbingConceptType) ||
											(upstreamConceptTypesDictionary != null && upstreamConceptTypesDictionary.ContainsKey(nearestAbsorbingConceptType))) &&
											// Only separate automatically if the ConceptType is left with nowhere to go without
											// the modified assimilation. Note that we don't care if the assimilator concept type changes
											// as long as there is one.
											null == AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilatorLinks, false, null))
										{
											SetSecondaryAbsorptionChoice(currentAssimilation, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Absorb, currentMapping, ref customizationModel, ref disabledChangeRuleType);
											// Although it is not obvious, there is no need to recurse here. If there are additional assimilations
											// lower down the chain then they will simply absorb through a remaining assimilator. There is no guarantee
											// that they will absorb to the same shared concept type, but they will always continue to absorb with the
											// current concept type to a shared parent.
										}
									}
								}
							}
							else
							{
								handleRemainingPartitions = true;
							}
						}
						break;
					case AssimilationAbsorptionChoice.Absorb:
						if (null != (modifiedAssimilation = GetAssimilationFromFactType(factType)))
						{
							// This throws, will not return on failure
							VerifyCanAbsorbAssimilation(modifiedAssimilation, true);
							attemptRemainingPartitionAbsorption = handleRemainingPartitions = oldChoice == AssimilationAbsorptionChoice.Partition;
						}
						break;
				}
				if (handleRemainingPartitions)
				{
					foreach (ConceptTypeAssimilatesConceptType partitionedAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(modifiedAssimilation.AssimilatorConceptType))
					{
						if (partitionedAssimilation != modifiedAssimilation)
						{
							AssimilationAbsorptionChoice verifiedChoice = attemptRemainingPartitionAbsorption && VerifyCanAbsorbAssimilation(partitionedAssimilation, false) ?
								AssimilationAbsorptionChoice.Absorb :
								AssimilationAbsorptionChoice.Separate;
							SetSecondaryAbsorptionChoice(partitionedAssimilation, verifiedChoice, AssimilationAbsorptionChoice.Partition, currentMapping, ref customizationModel, ref disabledChangeRuleType);
						}
					}
				}
			}
			finally
			{
				if (disabledChangeRuleType != null)
				{
					ruleManager.EnableRule(disabledChangeRuleType);
				}
			}
		}
		/// <summary>
		/// Recursively walk any assimilated <see cref="ConceptType"/>s
		/// </summary>
		/// <param name="assimilation">The current assimilation to start from</param>
		/// <param name="topLevel">The <paramref name="assimilation"/> is the one of the ones we're testing, do not place in the dictionary, and assume it is always absorbed</param>
		/// <param name="assimilationsDictionary">A dictionary of the currently assimilated <see cref="ConceptType"/>s as the
		/// key and a flag indicating if we're on the primary branch as the value.</param>
		private static void PopulateDownstreamAssimilations(ConceptTypeAssimilatesConceptType assimilation, bool topLevel, ref Dictionary<ConceptTypeAssimilatesConceptType, object> assimilationsDictionary)
		{
			if (assimilationsDictionary == null || !assimilationsDictionary.ContainsKey(assimilation))
			{
				if (topLevel || GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					ConceptType assimilatedConceptType = assimilation.AssimilatedConceptType;
					if (!topLevel)
					{
						if (assimilationsDictionary == null)
						{
							assimilationsDictionary = new Dictionary<ConceptTypeAssimilatesConceptType, object>();
						}
						assimilationsDictionary.Add(assimilation, null);
					}
					foreach (ConceptTypeAssimilatesConceptType childAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(assimilatedConceptType))
					{
						PopulateDownstreamAssimilations(childAssimilation, false, ref assimilationsDictionary);
					}
				}
			}
		}
		private static void PopulateUpstreamAbsorbingConceptTypes(ConceptType conceptType, ref Dictionary<ConceptType, ConceptType> upstreamConceptTypesDictionary)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilator in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (GetAbsorptionChoiceFromAssimilation(assimilator) == AssimilationAbsorptionChoice.Absorb)
				{
					if (upstreamConceptTypesDictionary == null)
					{
						upstreamConceptTypesDictionary = new Dictionary<ConceptType, ConceptType>();
						upstreamConceptTypesDictionary[conceptType] = conceptType;
					}
					ConceptType assimilatorConceptType = assimilator.AssimilatorConceptType;
					if (!upstreamConceptTypesDictionary.ContainsKey(assimilatorConceptType))
					{
						upstreamConceptTypesDictionary[assimilatorConceptType] = assimilatorConceptType;
						PopulateUpstreamAbsorbingConceptTypes(assimilatorConceptType, ref upstreamConceptTypesDictionary);
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(AssimilationMapping)
		/// An assimilation mapping choice has been changed. Verify consistency with other
		/// mapping choices.
		/// </summary>
		private static void AssimilationMappingChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == AssimilationMapping.AbsorptionChoiceDomainPropertyId)
			{
				AssimilationMapping currentMapping = (AssimilationMapping)e.ModelElement;
				FactType factType = currentMapping.FactType;
				if (factType != null)
				{
					ProcessModifiedAbsorptionChoice(factType, currentMapping, (AssimilationAbsorptionChoice)e.OldValue);
				}
			}
		}
		#endregion // AssimilationMapping Rule Methods
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new AssimilationMappingFixupListener();
			}
		}
		/// <summary>
		/// Validation AssimilationMapping options
		/// </summary>
		private sealed class AssimilationMappingFixupListener : DeserializationFixupListener<AssimilationMappingCustomizesFactType>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public AssimilationMappingFixupListener()
				: base((int)ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase.ValidateCustomizationOptions)
			{
			}
			/// <summary>
			/// Validate AssimilationMapping options
			/// </summary>
			protected sealed override void ProcessElement(AssimilationMappingCustomizesFactType element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
				}
			}
		}
		#endregion // Deserialization Fixup
		#region ORM/ORMAbstraction Bridge modification tracker
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeChildHasPathFactType)
		/// Validation absorption choices when a fact type is added to a path
		/// </summary>
		private static void PathFactTypeAddedRule(ElementAddedEventArgs e)
		{
			ConceptTypeChildHasPathFactType link = (ConceptTypeChildHasPathFactType)e.ModelElement;
			ConceptTypeAssimilatesConceptType assimilation;
			FactType factType;
			AssimilationAbsorptionChoice choice;
			AssimilationMapping mapping;
			if (null != (assimilation = link.ConceptTypeChild as ConceptTypeAssimilatesConceptType) &&
				null != (mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(factType = link.PathFactType)) &&
				(choice = mapping.AbsorptionChoice) != GetDefaultAbsorptionChoice(factType))
			{
				switch (choice)
				{
					case AssimilationAbsorptionChoice.Partition:
						FrameworkDomainModel.DelayValidateElement(assimilation.AssimilatorConceptType, DelayValidatePartitionedAssimilator);
						break;
					case AssimilationAbsorptionChoice.Separate:
						FrameworkDomainModel.DelayValidateElement(assimilation, DelayValidateSeparatedAssimilation);
						break;
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler)
		/// Validate partition patterns when a disjunctive mandatory constraint is decoupled or
		/// being deleted
		/// </summary>
		private static void DisjunctiveMandatoryCouplerDeletingRule(ElementDeletingEventArgs e)
		{
			ExclusiveOrConstraintCoupler coupler = (ExclusiveOrConstraintCoupler)e.ModelElement;
			foreach (Role role in coupler.MandatoryConstraint.RoleCollection)
			{
				FactType factType;
				AssimilationMapping mapping;
				ObjectType rolePlayer;
				ConceptType conceptType;
				if (null != (factType = role.BinarizedFactType) &&
					null != (mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(factType)) &&
					mapping.AbsorptionChoice == AssimilationAbsorptionChoice.Partition &&
					null != (rolePlayer = role.RolePlayer) &&
					!rolePlayer.IsDeleting &&
					null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(rolePlayer)) &&
					!conceptType.IsDeleting)
				{
					FrameworkDomainModel.DelayValidateElement(conceptType, DelayValidatePartitionedAssimilator);
				}
				// Just look at the first role, partitions are on all or none
				break;
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
		/// Validate partition patterns when a role is removed from a disjunctive mandatory constraint
		/// </summary>
		private static void DisjunctiveMandatoryRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			Role role = link.Role;
			ConstraintRoleSequence sequence;
			MandatoryConstraint mandatoryConstraint;
			FactType factType;
			AssimilationMapping mapping;
			ObjectType rolePlayer;
			ConceptType conceptType;
			if (!(role = link.Role).IsDeleted &&
				!(sequence = link.ConstraintRoleSequence).IsDeleted &&
				null != (mandatoryConstraint = sequence as MandatoryConstraint) &&
				null != mandatoryConstraint.ExclusiveOrExclusionConstraint &&
				null != (factType = role.BinarizedFactType) &&
				null != (mapping = AssimilationMappingCustomizesFactType.GetAssimilationMapping(factType)) &&
				mapping.AbsorptionChoice == AssimilationAbsorptionChoice.Partition &&
				null != (rolePlayer = role.RolePlayer) &&
				null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(rolePlayer)))
			{
				// Note that the case where the full constraint is deleted is handled in the
				// DisjunctiveMandatoryCouplerDeletingRule
				FrameworkDomainModel.DelayValidateElement(conceptType, DelayValidatePartitionedAssimilator);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType)
		/// UNDONE: Very expensive rule that needs to be revisited when
		/// incremental ORMAbstraction is in place. This takes any assimilated
		/// <see cref="FactType"/> that is being deleted and separates it.
		/// </summary>
		private static void FactTypeDeletingRule(ElementDeletingEventArgs e)
		{
			FactType factType = (FactType)e.ModelElement;
			ConceptTypeAssimilatesConceptType assimilation;
			MappingCustomizationModel customizationModel;
			if (null != (assimilation = GetAssimilationFromFactType(factType)) &&
				null != (customizationModel = MappingCustomizationModel.GetMappingCustomizationModel(factType.Store, false)))
			{
				if (GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					SetPrimaryAbsorptionChoice(factType, AssimilationAbsorptionChoice.Separate);
				}
			}
		}
		/// <summary>
		/// Delay validator to verify that partitioning is still valid for a <see cref="ConceptType"/>
		/// with partitioned assimilations. Runs before separation validation.
		/// </summary>
		[DelayValidatePriority(-20, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
		private static void DelayValidatePartitionedAssimilator(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ConceptType assimilator = (ConceptType)element;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(assimilator);
				if (assimilations.Count != 0) // Very unlikely, but need to check
				{
					bool partitionInvalid = true; // Prove otherwise
					// Choose any assimilation, they all need to follow the same pattern
					LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(assimilations[0]);
					if (factTypes.Count == 1)
					{
						Role towardsRoleDummy;
						LinkedElementCollection<Role> disjunctiveMandatoryRoles = GetDisjunctiveMandatoryRoles(factTypes[0], out towardsRoleDummy);
						if (disjunctiveMandatoryRoles != null)
						{
							partitionInvalid = !CanPartitionAssimilations(assimilations, disjunctiveMandatoryRoles);
						}
					}
					if (partitionInvalid)
					{
						Type disabledChangeRuleType = null;
						try
						{
							MappingCustomizationModel customizationModel = null;
							foreach (ConceptTypeAssimilatesConceptType assimilation in assimilations)
							{
								// Change from partition to separate. Separate is closest conceptually: the user
								// will still get separate table for each of the concept types that were previously partitioned.
								if (SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Partition, null, ref customizationModel, ref disabledChangeRuleType))
								{
									FrameworkDomainModel.DelayValidateElement(assimilation, DelayValidateSeparatedAssimilation);
								}
							}
						}
						finally
						{
							if (disabledChangeRuleType != null)
							{
								element.Store.RuleManager.EnableRule(disabledChangeRuleType);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Delay validator to verify that separation does not break downstream absorption choices. This
		/// routine identifies potentially influenced <see cref="ConceptType"/>s, which are then validated
		/// at a later phase by <see cref="DelayValidateDownstreamSeparatedConceptType"/>
		/// </summary>
		[DelayValidatePriority(-15, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
		private static void DelayValidateSeparatedAssimilation(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ConceptTypeAssimilatesConceptType assimilation = (ConceptTypeAssimilatesConceptType)element;
				// Goal of this routine is to find all direct and indirect assimilated concept types
				// so we can verify that any absorption pattern on them is still valid
				ValidateDownstreamSeparatedConceptType(assimilation.AssimilatedConceptType);
			}
		}
		private static void ValidateDownstreamSeparatedConceptType(ConceptType conceptType)
		{
			FrameworkDomainModel.DelayValidateElement(conceptType, DelayValidateDownstreamSeparatedConceptType);
			foreach (ConceptType assimilatedConceptType in ConceptTypeAssimilatesConceptType.GetAssimilatedConceptTypeCollection(conceptType))
			{
				ValidateDownstreamSeparatedConceptType(assimilatedConceptType);
			}
		}
		/// <summary>
		/// Make sure that any <see cref="ConceptType"/> that is absorbed in more than
		/// one direction has a shared node.
		/// </summary>
		[DelayValidatePriority(-10, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
		private static void DelayValidateDownstreamSeparatedConceptType(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ConceptType conceptType = (ConceptType)element;
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> assimilatorLinks = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType);
				if (assimilatorLinks.Count > 1)
				{
					if (null == (AssimilatorTracker.GetNearestAbsorbingAssimilatorConceptType(assimilatorLinks)))
					{
						Type disabledChangeRuleType = null;
						try
						{
							MappingCustomizationModel customizationModel = null;
							bool separationModified = false;
							foreach (ConceptTypeAssimilatesConceptType assimilation in assimilatorLinks)
							{
								// At this point, making an intelligence choice on which
								// assimilators to separate and which to absorb is difficult,
								// expensive, and arbitrary. We just separate all of them for now.
								separationModified |= SetSecondaryAbsorptionChoice(assimilation, AssimilationAbsorptionChoice.Separate, AssimilationAbsorptionChoice.Absorb, null, ref customizationModel, ref disabledChangeRuleType);
							}
							if (separationModified)
							{
								// Make sure any downstream concept types that have not yet been validated
								// or have already been validated once are correctly validated.
								foreach (ConceptType assimilatedConceptType in ConceptTypeAssimilatesConceptType.GetAssimilatedConceptTypeCollection(conceptType))
								{
									ValidateDownstreamSeparatedConceptType(assimilatedConceptType);
								}
							}
						}
						finally
						{
							if (disabledChangeRuleType != null)
							{
								element.Store.RuleManager.EnableRule(disabledChangeRuleType);
							}
						}
					}
				}
			}
		}
		#endregion // ORM/ORMAbstraction Bridge modification tracker
	}
}
