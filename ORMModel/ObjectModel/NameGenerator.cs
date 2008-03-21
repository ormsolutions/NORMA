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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Neumont.Tools.Modeling;
using System.Text;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel
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
		/// Return the <see cref="Type"/> assocatiated with the <see cref="NameUsage"/> property.
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
		/// Return the <see cref="DomainClassInfo"/> assocatiated with the <see cref="NameUsage"/> property.
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
			NameGenerator refinesGenerator = RefinesGenerator;
			bool retVal = (refinesGenerator != null) ?
				(refinesGenerator.CasingOption != CasingOption ||
				refinesGenerator.SpacingFormat != SpacingFormat ||
				refinesGenerator.SpacingReplacement != SpacingReplacement ||
				refinesGenerator.AutomaticallyShortenNames != AutomaticallyShortenNames ||
				refinesGenerator.UserDefinedMaximum != UserDefinedMaximum ||
				refinesGenerator.UseTargetDefaultMaximum != UseTargetDefaultMaximum) :
				(CasingOption != NameGeneratorCasingOption.None ||
				SpacingFormat != NameGeneratorSpacingFormat.Retain ||
				SpacingReplacement.Length != 0 ||
				!AutomaticallyShortenNames ||
				UserDefinedMaximum != 128 ||
				!UseTargetDefaultMaximum);
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
				Debug.Assert(propertyInfo.Id != NameGenerator.NameUsageDomainPropertyId, "NameUsage should be initialized by CreateRefinement and not changed.");
				object oldValue = e.OldValue;
				object newValue = e.NewValue;
				NameGenerator changedGenerator = (NameGenerator)e.ModelElement;
				Type testUsage = changedGenerator.NameUsageType;
				if (testUsage != null)
				{
					changedGenerator = changedGenerator.RefinesGenerator;
					Debug.Assert(changedGenerator.NameUsageType == null, "Parents of usage refinements should not have a usage");
				}
				try
				{
					myIsDisabled = true;
					if (testUsage != null)
					{
						PropagateChange(changedGenerator, propertyInfo, oldValue, newValue, testUsage);
					}
					else
					{
						PropagateChange(changedGenerator, propertyInfo, oldValue, newValue);
					}
				}
				finally
				{
					myIsDisabled = false;
				}
			}
			private static void PropagateChange(NameGenerator parentGenerator, DomainPropertyInfo propertyInfo, object oldValue, object newValue)
			{
				foreach (NameGenerator refinement in parentGenerator.RefinedByGeneratorCollection)
				{
					if (propertyInfo.GetValue(refinement).Equals(oldValue))
					{
						propertyInfo.SetValue(refinement, newValue);
						PropagateChange(refinement, propertyInfo, oldValue, newValue);
					}
				}
			}
			private static void PropagateChange(NameGenerator parentGenerator, DomainPropertyInfo propertyInfo, object oldValue, object newValue, Type nameUsageType)
			{
				foreach (NameGenerator refinement in parentGenerator.RefinedByGeneratorCollection)
				{
					Type refinementUsage = refinement.NameUsageType;
					if (refinementUsage == null)
					{
						// Make sure we skip the level to get the corresponding usages
						PropagateChange(refinement, propertyInfo, oldValue, newValue, nameUsageType);
					}
					else if	(refinementUsage == nameUsageType &&
						propertyInfo.GetValue(refinement).Equals(oldValue))
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
					topGenerator.FullyPopulateRefinements();
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
		/// <summary>
		/// This guarantees that we have an instance for every possible Name Generator type.
		/// </summary>
		private void FullyPopulateRefinements()
		{
			LinkedElementCollection<NameGenerator> currentChildren = RefinedByGeneratorCollection;
			DomainClassInfo contextDomainClass = GetDomainClass();
			ReadOnlyCollection<DomainClassInfo> requiredDescendantsCollection = contextDomainClass.LocalDescendants;
			int requiredDescendantsCount = requiredDescendantsCollection.Count;
			Type[] requiredUsageTypes = GetSupportedNameUsageTypes();
			int requiredUsageCount = requiredUsageTypes.Length;
			Store store = Store;
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
						currentChild.FullyPopulateRefinements();
					}
					else if (missingUsageCount != 0)
					{
						int index = Array.IndexOf<Type>(requiredUsageTypes, nameUsage);
						if (index != -1)
						{
							requiredUsageTypes[index] = null;
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
							CreateRefinement(classInfo, null).FullyPopulateRefinements(); ;
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
							CreateRefinement(contextDomainClass, usageType);
						}
					}
				}
			}
		}
		#endregion // Serialization support
		#region NameUsage Attribute Caching
		private Type[] myNameUsageTypes;
		private Type[] NameUsageTypes
		{
			get
			{
				Type[] retVal = myNameUsageTypes;
				if (retVal == null)
				{
					NameUsageAttribute[] usageAttributes = (NameUsageAttribute[])GetType().GetCustomAttributes(typeof(NameUsageAttribute), true);
					int usageCount = usageAttributes.Length;
					if (usageCount == 0)
					{
						retVal = Type.EmptyTypes;
					}
					else
					{
						retVal = new Type[usageCount];
						for (int i = 0; i < usageCount; ++i)
						{
							retVal[i] = usageAttributes[i].Type;
						}
					}
					myNameUsageTypes = retVal;
				}
				return retVal;
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
		#endregion // NameUsage Attribute Caching
		#region Refinement management
		/// <summary>
		/// Create a refinement of the specified type
		/// </summary>
		/// <param name="childType">A <see cref="DomainClassInfo"/> of the current type if <paramref name="nameUsageType"/> is set.
		/// Otherwise,  the <see cref="DomainClassInfo.BaseDomainClass"/> must equal the current <see cref="ModelElement.GetDomainClass">DomainClass</see>.</param>
		/// <param name="nameUsageType">The type to associate with the <see cref="NameUsage"/> property. Can be <see langword="null"/></param>
		/// <returns>A new <see cref="NameGenerator"/> of the specified type</returns>
		private NameGenerator CreateRefinement(DomainClassInfo childType, Type nameUsageType)
		{
			DomainClassInfo nameGeneratorClass = GetDomainClass();
			Debug.Assert(childType == nameGeneratorClass || childType.BaseDomainClass == nameGeneratorClass);
			Store store = Store;
			ReadOnlyCollection<DomainPropertyInfo> properties = nameGeneratorClass.AllDomainProperties;
			PropertyAssignment[] propertyAssignments = new PropertyAssignment[properties.Count + (nameUsageType == null ? -1 : 0)];
			int i = 0;
			foreach (DomainPropertyInfo property in properties)
			{
				if (property.Id != NameUsageDomainPropertyId)
				{
					propertyAssignments[i] = new PropertyAssignment(property.Id, property.GetValue(this));
					++i;
				}
				else if (nameUsageType != null)
				{
					propertyAssignments[i] = new PropertyAssignment(property.Id, ObjectModel.NameUsage.TranslateToNameUsageIdentifier(store.DomainDataDirectory.FindDomainClass(nameUsageType)));
					++i;
				}
			}
			NameGenerator retVal = (NameGenerator)store.GetDomainModel(childType.DomainModel.Id).CreateElement(store.DefaultPartition, childType.ImplementationClass, propertyAssignments);
			retVal.RefinesGenerator = this;
			return retVal;
		}
		#endregion // Refinement management
		#region GetGeneratorSettings
		/// <summary>
		/// Gets the settings for specific <see cref="NameGenerator"/>
		/// </summary>
		/// <param name="store">The <see cref="Store"/></param>
		/// <param name="generatorType">The type of the <see cref="NameGenerator"/></param>
		/// <param name="usageType">The <see cref="NameUsage"/> of the NameGenerator</param>
		/// <returns></returns>
		public static NameGenerator GetGenerator(Store store, Type generatorType, Type usageType)
		{
			foreach (NameGenerator generator in store.ElementDirectory.FindElements(store.DomainDataDirectory.GetDomainClass(generatorType), false))
			{
				if (generator.NameUsageType == usageType)
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
		/// <param name="aliases">A set of alias elements. The best match is returned.</param>
		/// <returns>A <see cref="NameAlias"/>, or <see langword="null"/> if none is available.</returns>
		public NameAlias FindMatchingAlias(IEnumerable<NameAlias> aliases)
		{
			NameAlias bestMatch = null;
			Type usageType = NameUsageType;
			DomainClassInfo thisClassInfo = GetDomainClass();
			DomainClassInfo matchedClassInfo = null;
			bool matchedUsageType = false;
			int closestDistance = int.MaxValue;
			foreach (NameAlias alias in aliases)
			{
				DomainClassInfo testClassInfo = alias.NameConsumerDomainClass;
				Type testUsageType = alias.NameUsageType;
				if (testClassInfo == thisClassInfo)
				{
					if (usageType == testUsageType)
					{
						bestMatch = alias;
						break;
					}
					else if (usageType != null && testUsageType == null)
					{
						matchedClassInfo = testClassInfo;
						bestMatch = alias;
						// Keep going to see if we get an exact usage match
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
							if (testDistance <= closestDistance)
							{
								if (testClassInfo == matchedClassInfo)
								{
									if (!matchedUsageType)
									{
										bestMatch = alias;
										matchedUsageType = usageType == testUsageType;
									}
								}
								else if (usageType == testUsageType)
								{
									closestDistance = testDistance;
									matchedClassInfo = testClassInfo;
									matchedUsageType = true;
									bestMatch = alias;
								}
								else if (usageType != null && testUsageType == null)
								{
									closestDistance = testDistance;
									matchedClassInfo = testClassInfo;
									matchedUsageType = false;
									bestMatch = alias;
								}
							}
							break;
						}
						iterateClassInfo = iterateClassInfo.BaseDomainClass;
					} while (iterateClassInfo != null);
				}
			}
			return bestMatch;
		}
		#endregion // GetGeneratorSettings
	}
	#endregion // NameGenerator class
}
