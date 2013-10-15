#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ReferenceMode class
	partial class ReferenceMode : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Matches reference mode pattern by kind and name.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ReferenceModeKind kind = Kind;
			ReferenceModeKind otherKind;
			if (null != (otherKind = CopyMergeUtility.GetEquivalentElement(kind, foreignStore, elementTracker)))
			{
				string testName = Name;
				foreach (ReferenceMode otherMode in otherKind.ReferenceModeCollection)
				{
					if (otherMode.Name == testName)
					{
						elementTracker.AddEquivalentElement(this, otherMode);
						return true;
					}
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ReferenceMode class
	#region ReferenceModeKind class
	partial class ReferenceModeKind : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match reference mode kind by <see cref="ReferenceModeType"/>
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ReferenceModeType testType = ReferenceModeType;
			foreach (ReferenceModeKind otherKind in foreignStore.ElementDirectory.FindElements<ReferenceModeKind>(false))
			{
				if (otherKind.ReferenceModeType == testType)
				{
					elementTracker.AddEquivalentElement(this, otherKind);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ReferenceModeKind class
	#region DataType class
	partial class DataType : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Each instantiated data type is a singleton, find the instance based
		/// on its type.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			foreach (DataType otherDataType in foreignStore.ElementDirectory.FindElements(GetDomainClass().Id, false))
			{
				elementTracker.AddEquivalentElement(this, otherDataType);
				return true;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // DataType class
	#region ObjectType class
	partial class ObjectType : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match object type by name.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			// These are generally matched by name unless a ValueType is used in a simple reference scheme
			// pattern where this value type is used as the identifying value for one fact type only.
			ObjectType otherObjectType = null;
			MandatoryConstraint impliedMandatory;
			LinkedElementCollection<Role> impliedMandatoryRoles;
			UniquenessConstraint singleRoleUniqueness;
			ObjectType simpleIdentifierFor = null;
			FactType objectifiedFactType = null;
			DataType dataType = null;
			bool isValueType = false;
			if (IsImplicitBooleanValue)
			{
				// Match through the unary fact type, the name is of minimal use here
				foreach (Role playedRole in PlayedRoleCollection)
				{
					if (null != CopyMergeUtility.GetEquivalentElement(playedRole.FactType, foreignStore, elementTracker))
					{
						otherObjectType = (ObjectType)elementTracker.GetEquivalentElement(this);
						dataType = DataType;
						isValueType = true;
					}
					break;
				}
			}
			else if (null != (objectifiedFactType = NestedFactType))
			{
				// Match through the fact type. Note that this is exclusion with the next branch (value types are not objectified)
				FactType otherFactType = CopyMergeUtility.GetEquivalentElement(objectifiedFactType, foreignStore, elementTracker);
				if (otherFactType != null)
				{
					otherObjectType = otherFactType.NestingType;
				}
			}
			else if ((isValueType = null != (dataType = DataType)) &&
				null != (impliedMandatory = ImpliedMandatoryConstraint) &&
				1 == (impliedMandatoryRoles = impliedMandatory.RoleCollection).Count &&
				null != (singleRoleUniqueness = impliedMandatoryRoles[0].SingleRoleAlethicUniquenessConstraint) &&
				null != (simpleIdentifierFor = singleRoleUniqueness.PreferredIdentifierFor))
			// UNDONE: COPYMERGE Do we need to check for an objectification pattern here for an objectified many-to-one?
			{
				ObjectType otherIdentifiedElement;
				UniquenessConstraint otherIdentifier;
				LinkedElementCollection<Role> otherIdentifierRoles;
				ObjectType otherSimpleIdentifier;
				Role otherIdentifierRole;
				if (null != (otherIdentifiedElement = CopyMergeUtility.GetEquivalentElement(simpleIdentifierFor, foreignStore, elementTracker)) &&
					null != (otherIdentifier = otherIdentifiedElement.PreferredIdentifier) &&
					1 == (otherIdentifierRoles = otherIdentifier.RoleCollection).Count &&
					(otherSimpleIdentifier = (otherIdentifierRole = otherIdentifierRoles[0]).RolePlayer).IsValueType &&
					null != (impliedMandatory = otherSimpleIdentifier.ImpliedMandatoryConstraint) &&
					1 == (impliedMandatoryRoles = impliedMandatory.RoleCollection).Count &&
					impliedMandatoryRoles[0] == otherIdentifierRole)
				{
					otherObjectType = otherSimpleIdentifier;
				}
			}
			if (otherObjectType == null && objectifiedFactType == null)
			{
				foreach (ORMModel otherModel in foreignStore.ElementDirectory.FindElements<ORMModel>(false))
				{
					LocatedElement otherCandidates = otherModel.ObjectTypesDictionary.GetElement(Name);
					IEnumerator enumerator = null;
					if (!otherCandidates.IsEmpty)
					{
						if (null == (otherObjectType = otherCandidates.SingleElement as ObjectType))
						{
							enumerator = otherCandidates.MultipleElements.GetEnumerator();
							if (enumerator.MoveNext())
							{
								otherObjectType = enumerator.Current as ObjectType;
							}
						}
						while (otherObjectType != null)
						{
							// UNDONE: COPYMERGE We can't recover from Entity/Value mismatch without deletion semantics
							// We may also need to adjust ObjectType.ProcessCheckForIncompatibleRelationship if
							// this changes.
							if (otherObjectType.IsValueType == isValueType)
							{
								break;
							}
							if (enumerator != null && enumerator.MoveNext())
							{
								otherObjectType = enumerator.Current as ObjectType;
							}
							else
							{
								otherObjectType = null;
							}
						}
						if (otherObjectType.IsValueType != isValueType) 
						{
							otherObjectType = null;
						}
					}
					break;
				}
			}
			if (otherObjectType != null)
			{
				elementTracker.AddEquivalentElement(this, otherObjectType);
				if (dataType != null)
				{
					// Compare all instances with one pass
					LinkedElementCollection<ValueTypeInstance> instances;
					int instanceCount;
					LinkedElementCollection<ValueTypeInstance> otherInstances;
					int otherInstanceCount;
					if (0 != (instanceCount = (instances = ValueTypeInstanceCollection).Count) &&
						0 != (otherInstanceCount = (otherInstances = otherObjectType.ValueTypeInstanceCollection).Count))
					{
						// See comments on data type in ValueConstraint.MatchValueRanges
						// Although it is a model error, there is not guarantee that there
						// will be no duplicate values in this model. If the duplicate is in
						// both places, this algorithm will match earliest-to-earliest.

						// Determine the smallest collection to optimize processing
						bool reverseProcessing = otherInstanceCount > instanceCount;
						if (reverseProcessing)
						{
							int swapCount = instanceCount;
							instanceCount = otherInstanceCount;
							otherInstanceCount = swapCount;
							LinkedElementCollection<ValueTypeInstance> swapInstances = instances;
							instances = otherInstances;
							otherInstances = swapInstances;
						}
						ValueTypeInstance instance;
						string normalizedValue;
						if (otherInstanceCount == 1)
						{
							ValueTypeInstance otherInstance = otherInstances[0];
							string otherNormalizedValue;
							if (dataType.ParseNormalizeValue(otherInstance.Value, otherInstance.InvariantValue, out otherNormalizedValue))
							{
								int i = 0;
								for (; i < instanceCount; ++i)
								{
									instance = instances[i];
									if (dataType.ParseNormalizeValue(instance.Value, instance.InvariantValue, out normalizedValue) &&
										normalizedValue == otherNormalizedValue)
									{
										if (reverseProcessing)
										{
											elementTracker.AddEquivalentElement(otherInstance, instance);
										}
										else
										{
											elementTracker.AddEquivalentElement(instance, otherInstance);
											for (int j = i + 1; j < instanceCount; ++j)
											{
												elementTracker.AddFailedEquivalentElement(instances[j]);
											}
										}
										break;
									}
									else if (!reverseProcessing)
									{
										elementTracker.AddFailedEquivalentElement(instance);
									}
								}
								if (reverseProcessing && i == instanceCount)
								{
									elementTracker.AddFailedEquivalentElement(otherInstance);
								}
							}
						}
						else
						{
							Dictionary<string, int> otherInstanceDictionary = new Dictionary<string, int>(otherInstanceCount);
							int[] duplicatedAt = null;
							int existingIndex;
							for (int i = 0; i < otherInstanceCount; ++i)
							{
								instance = otherInstances[i];
								if (dataType.ParseNormalizeValue(instance.Value, instance.InvariantValue, out normalizedValue))
								{
									if (otherInstanceDictionary.TryGetValue(normalizedValue, out existingIndex))
									{
										// Track duplicates. The duplicated at value for existing index
										// points to the duplicate index, which points to the next duplicate, etc.
										// Note that 0 is the default value and means 'not duplicated'.
										if (duplicatedAt == null)
										{
											duplicatedAt = new int[otherInstanceCount];
										}
										else
										{
											int duplicateIndex;
											while (0 != (duplicateIndex = duplicatedAt[existingIndex]))
											{
												existingIndex = duplicateIndex;
											}
										}
										duplicatedAt[existingIndex] = i;
									}
									else
									{
										otherInstanceDictionary[normalizedValue] = i;
									}
								}
							}
							for (int i = 0; i < instanceCount; ++i)
							{
								instance = instances[i];
								if (dataType.ParseNormalizeValue(instance.Value, instance.InvariantValue, out normalizedValue) &&
									otherInstanceDictionary.TryGetValue(normalizedValue, out existingIndex) &&
									existingIndex != -1)
								{
									if (reverseProcessing)
									{
										elementTracker.AddEquivalentElement(otherInstances[existingIndex], instance);
									}
									else
									{
										elementTracker.AddEquivalentElement(instance, otherInstances[existingIndex]);
									}
									if (null == duplicatedAt ||
										0 == (existingIndex = duplicatedAt[existingIndex]))
									{
										existingIndex = -1;
									}
									otherInstanceDictionary[normalizedValue] = existingIndex;
								}
								else if (!reverseProcessing)
								{
									elementTracker.AddFailedEquivalentElement(instance);
								}
							}
							if (reverseProcessing)
							{
								// Mark remaining items as not processed
								foreach (int index in otherInstanceDictionary.Values)
								{
									if (index != -1)
									{
										elementTracker.AddFailedEquivalentElement(otherInstances[index]);
										if (duplicatedAt != null)
										{
											int duplicateIndex = index;
											while (0 != (duplicateIndex = duplicatedAt[duplicateIndex]))
											{
												elementTracker.AddFailedEquivalentElement(otherInstances[duplicateIndex]);
											}
										}
									}
								}
							}
						}
					}
				}
				return true;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ObjectType class
	#region ValueTypeInstance class
	partial class ValueTypeInstance : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match an instance based on the parent <see cref="ValueTypeInstance"/>.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return CopyMergeUtility.GetEquivalentElement(ObjectType, foreignStore, elementTracker) != null &&
				null != elementTracker.GetEquivalentElement(this);
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ValueTypeInstance class
	#region EntityTypeInstance class
	partial class EntityTypeInstance : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			// UNDONE: EntityTypeInstance merge is more complicated than ValueTypeInstance or FactTypeInstance
			// because it is possibly cyclic on the instance type, so we cannot always map these in a block during
			// ObjectType merge. In practice, this condition is rare, and mapping these indivdually is less efficient
			// than mapping them altogether, so we should be able to detect this condition and apply a block
			// processing algorithm in ObjectType.
			ObjectType entityType = EntityType;
			ObjectType otherEntityType;
			LinkedElementCollection<Role> roles;
			LinkedElementCollection<Role> otherRoles;
			int roleCount;
			UniquenessConstraint pid;
			UniquenessConstraint otherPid;
			if (null != (otherEntityType = CopyMergeUtility.GetEquivalentElement(entityType, foreignStore, elementTracker)) &&
				null != (pid = entityType.PreferredIdentifier) &&
				null != (otherPid = CopyMergeUtility.GetEquivalentElement(pid, foreignStore, elementTracker)) &&
				(roleCount = (roles = pid.RoleCollection).Count) == (otherRoles = otherPid.RoleCollection).Count)
			{
				Role[] sortedOtherRoles = new Role[roleCount];
				for (int i = 0; i < roleCount; ++i)
				{
					sortedOtherRoles[i] = (Role)elementTracker.GetEquivalentElement(roles[i]); // This has to succeed or the constraints would not have matched
				}
				LinkedElementCollection<EntityTypeInstance> allInstances = null;
				LinkedElementCollection<EntityTypeRoleInstance> roleInstances = RoleInstanceCollection;
				int roleInstanceCount = roleInstances.Count;
				foreach (EntityTypeInstance testOtherInstance in otherEntityType.EntityTypeInstanceCollection)
				{
					LinkedElementCollection<EntityTypeRoleInstance> otherRoleInstances = testOtherInstance.RoleInstanceCollection;
					if (otherRoleInstances.Count == roleInstanceCount)
					{
						int i = 0;
						for (; i < roleInstanceCount; ++i)
						{
							EntityTypeRoleInstance roleInstance = roleInstances[i];
							bool matchedRoleInstance = false;
							ObjectTypeInstance otherTargetInstance;
							if (null != (otherTargetInstance = CopyMergeUtility.GetEquivalentElement(roleInstance.ObjectTypeInstance, foreignStore, elementTracker)))
							{
								Role findOtherRole = sortedOtherRoles[roles.IndexOf(roleInstance.Role)];
								foreach (EntityTypeRoleInstance otherRoleInstance in otherRoleInstances)
								{
									if (otherRoleInstance.Role == findOtherRole)
									{
										matchedRoleInstance = otherRoleInstance.ObjectTypeInstance == otherTargetInstance;
										break;
									}
								}
							}
							if (!matchedRoleInstance)
							{
								break;
							}
						}
						if (i == roleInstanceCount)
						{
							// We matched on all populated role instances. Match them up
							// The mapping framework does not store reverse mappings, so
							// we cannot easily check if the other item has already been
							// matched. Perform a previously matched check as a final
							// check before stating equivalence.
							EntityTypeInstance otherInstance = testOtherInstance;
							foreach (EntityTypeInstance siblingInstance in (allInstances ?? (allInstances = entityType.EntityTypeInstanceCollection)))
							{
								if (siblingInstance != this &&
									elementTracker.GetEquivalentElement(siblingInstance) == otherInstance)
								{
									otherInstance = null;
									break;
								}
							}
							if (otherInstance != null)
							{
								elementTracker.AddEquivalentElement(this, otherInstance);
								// Role instances allow duplicates and will not map automatically,
								// add them explicitly for this instance.
								for (i = 0; i < roleInstanceCount; ++i)
								{
									EntityTypeRoleInstance roleInstance = roleInstances[i];
									Role findOtherRole = sortedOtherRoles[roles.IndexOf(roleInstance.Role)];
									foreach (EntityTypeRoleInstance otherRoleInstance in otherRoleInstances)
									{
										if (otherRoleInstance.Role == findOtherRole)
										{
											elementTracker.AddEquivalentElement(roleInstance, otherRoleInstance);
											break;
										}
									}
								}
								return true;
							}
						}
					}
				}
			}
			elementTracker.AddFailedEquivalentElement(this);
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // EntityTypeInstance class
	#region EntityTypeSubtypeInstance class
	partial class EntityTypeSubtypeInstance : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ObjectType entityType = EntityTypeSubtype;
			ObjectType otherEntityType;
			EntityTypeSubtypeInstanceHasSupertypeInstance supertypeInstanceLink;
			EntityTypeInstance otherSupertypeInstance;
			if (null != (otherEntityType = CopyMergeUtility.GetEquivalentElement(entityType, foreignStore, elementTracker)) &&
				null != (supertypeInstanceLink = EntityTypeSubtypeInstanceHasSupertypeInstance.GetLinkToSupertypeInstance(this)) &&
				null != (otherSupertypeInstance = CopyMergeUtility.GetEquivalentElement(supertypeInstanceLink.SupertypeInstance, foreignStore, elementTracker)))
			{
				// The supertype instance collection is limited in size to the total number of
				// potential subtypes, whereas the full instance collections are unlimited, so
				// we key off this collection.
				foreach (EntityTypeSubtypeInstanceHasSupertypeInstance otherSupertypeInstanceLink in EntityTypeSubtypeInstanceHasSupertypeInstance.GetLinksToEntityTypeSubtypeInstanceCollection(otherSupertypeInstance))
				{
					EntityTypeSubtypeInstance otherSubtypeInstance = otherSupertypeInstanceLink.EntityTypeSubtypeInstance;
					if (otherEntityType == otherSubtypeInstance.EntityTypeSubtype)
					{
						elementTracker.AddEquivalentElement(this, otherSubtypeInstance);
						// This will link automatically, but we already have the information, so we might as well use it.
						elementTracker.AddEquivalentElement(supertypeInstanceLink, otherSupertypeInstanceLink);
						return true;
					}
				}
			}
			elementTracker.AddFailedEquivalentElement(this);
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // EntityTypeSubtypeInstance class
	#region Function class
	partial class Function : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match object type by name.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			foreach (ORMModel otherModel in foreignStore.ElementDirectory.FindElements<ORMModel>(false))
			{
				LocatedElement otherFunctionCandidates = otherModel.FunctionsDictionary.GetElement(Name);
				Function otherFunction = null;
				IEnumerator enumerator = null;
				if (otherFunctionCandidates.IsEmpty)
				{
					return false;
				}
				else if (null == (otherFunction = otherFunctionCandidates.SingleElement as Function))
				{
					enumerator = otherFunctionCandidates.MultipleElements.GetEnumerator();
					if (enumerator.MoveNext())
					{
						otherFunction = enumerator.Current as Function;
					}
				}
				while (otherFunction != null)
				{
					LinkedElementCollection<FunctionParameter> parameters = ParameterCollection;
					int parameterCount = parameters.Count;
					LinkedElementCollection<FunctionParameter> otherParameters = otherFunction.ParameterCollection;
					int otherParameterCount = otherParameters.Count;
					if (parameterCount == otherParameterCount)
					{
						// Match by position and bag status to ensure match
						int i = 0;
						for (; i < parameterCount; ++i)
						{
							if (parameters[i].BagInput != otherParameters[i].BagInput)
							{
								break;
							}
						}
						if (i == parameterCount)
						{
							elementTracker.AddEquivalentElement(this, otherFunction);
							for (i = 0; i < parameterCount; ++i)
							{
								elementTracker.AddEquivalentElement(parameters[i], otherParameters[i]);
							}
							return true;
						}
					}
					else if (otherParameterCount < parameterCount)
					{
						// Match by parameter name and bag status, allowing a match on parameter additions
						int i = 0;
						for (; i < otherParameterCount; ++i)
						{
							FunctionParameter otherParameter = otherParameters[i];
							string matchOtherName = otherParameter.Name;
							bool matchOtherBag = otherParameter.BagInput;
							int j = 0;
							for (; j < parameterCount; ++j)
							{
								FunctionParameter testParameter = parameters[j];
								if (testParameter.BagInput == matchOtherBag &&
									testParameter.Name == matchOtherName)
								{
									break;
								}
							}
							if (j == parameterCount)
							{
								break;
							}
						}
						if (i == otherParameterCount)
						{
							// We have a match for all 'other' parameters
							elementTracker.AddEquivalentElement(this, otherFunction);
							for (i = 0; i < otherParameterCount; ++i)
							{
								FunctionParameter otherParameter = otherParameters[i];
								string matchOtherName = otherParameter.Name;
								bool matchOtherBag = otherParameter.BagInput;
								int j = 0;
								for (; j < parameterCount; ++j)
								{
									FunctionParameter testParameter = parameters[j];
									if (testParameter.BagInput == matchOtherBag &&
										testParameter.Name == matchOtherName)
									{
										elementTracker.AddEquivalentElement(testParameter, otherParameter);
										break;
									}
								}
							}
							return true;
						}
					}

					// Handle both single and multiple elements
					if (enumerator != null &&
						enumerator.MoveNext())
					{
						otherFunction = enumerator.Current as Function;
					}
					else
					{
						otherFunction = null;
					}
				}
				break;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // Function class
	#region FactType class
	partial class FactType : IElementEquivalence
	{
		#region FactTypeAndRole struct
		/// <summary>
		/// Helper struct for <see cref="MapEquivalentElements"/>
		/// Tracks a role and its parent fact type.
		/// </summary>
		private struct FactTypeAndRole
		{
			public readonly FactType FactType;
			public readonly Role Role;
			public FactTypeAndRole(FactType factType, Role role)
			{
				FactType = factType;
				Role = role;
			}
		}
		#endregion // FactTypeAndRole struct
		#region PopulatedReadingTextWithRoleOrder struct
		/// <summary>
		/// Helper struct for <see cref="MapEquivalentElements"/>.
		/// Tracks a populated reading text with its role order.
		/// </summary>
		private struct PopulatedReadingTextWithRoleOrder
		{
			public IList<RoleBase> ReadingRoles;
			public readonly string PopulatedReadingText;
			public PopulatedReadingTextWithRoleOrder(IList<RoleBase> readingRoles, string populatedReadingText)
			{
				ReadingRoles = readingRoles;
				PopulatedReadingText = populatedReadingText;
			}
			public static readonly IComparer<PopulatedReadingTextWithRoleOrder> Comparer = new ComparerImpl();
			private sealed class ComparerImpl : IComparer<PopulatedReadingTextWithRoleOrder>
			{
				int IComparer<PopulatedReadingTextWithRoleOrder>.Compare(PopulatedReadingTextWithRoleOrder x, PopulatedReadingTextWithRoleOrder y)
				{
					return string.Compare(x.PopulatedReadingText, y.PopulatedReadingText, StringComparison.CurrentCulture);
				}
			}
		}
		#endregion // PopulatedReadingTextWithRoleOrder struct
		#region ObjectTypeInstanceArrayEqualityComparer class
		private sealed class ObjectTypeInstanceArrayEqualityComparer : IEqualityComparer<ObjectTypeInstance[]>
		{
			#region Singleton and Constructor
			public static readonly IEqualityComparer<ObjectTypeInstance[]> Instance = new ObjectTypeInstanceArrayEqualityComparer();
			private ObjectTypeInstanceArrayEqualityComparer()
			{
			}
			#endregion // Singleton and Constructor
			#region IEqualityComparer<ObjectTypeInstance[]> Implementation
			bool IEqualityComparer<ObjectTypeInstance[]>.Equals(ObjectTypeInstance[] x, ObjectTypeInstance[] y)
			{
				for (int i = 0; i < x.Length; ++i)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
				return true;
			}
			int IEqualityComparer<ObjectTypeInstance[]>.GetHashCode(ObjectTypeInstance[] obj)
			{
				unchecked
				{
					uint hashCode = 0;
					bool seenValue = false;
					for (int i = 0; i < obj.Length; ++i)
					{
						ObjectTypeInstance instance = obj[i];
						if (instance != null)
						{
							if (seenValue)
							{
								hashCode ^= (uint)Utility.RotateRight(instance.GetHashCode(), i);
							}
							else
							{
								seenValue = true;
								hashCode = i == 0 ? (uint)instance.GetHashCode() : (uint)Utility.RotateRight(instance.GetHashCode(), i);
							}
						}
					}
					return (int)hashCode;
				}
			}
			#endregion // IEqualityComparer<ObjectTypeInstance[]> Implementation
		}
		#endregion // ObjectTypeInstanceArrayEqualityComparer class
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match a fact type based on entity and predicate names.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			FactType otherFactType = null;
			IList<RoleBase> roleOrder = null;
			IList<RoleBase> otherRoleOrder = null;
			IList<RoleBase> roles = RoleCollection;
			int roleCount = roles.Count;
			LinkedElementCollection<ReadingOrder> readingOrders = null;
			LinkedElementCollection<ReadingOrder> otherReadingOrders = null;
			RoleBase implicitBooleanRole = null;
			RoleBase otherImplicitBooleanRole = null;
			bool impliedPopulation = false;
			if (roleCount == 2 &&
				null != ImpliedByObjectification)
			{
				// This is a link fact type and is effectively identified by the target
				// of its RoleProxy or ObjectifiedUnaryRole. Pragmatically, readings for
				// these fact types also tend to be somewhat unreliable because they are
				// auto generated and it is easy to have duplication errors. Therefore,
				// we match this fact type based on the roles in the primary fact type.
				Role matchFactTypeForRole = null;
				RoleProxy roleProxy;
				ObjectifiedUnaryRole objectifiedUnaryRole;
				RoleBase objectifyingEntityRole = null;
				RoleBase mirrorRole = null;
				foreach (RoleBase roleBase in roles)
				{
					if (null != (roleProxy = roleBase as RoleProxy))
					{
						mirrorRole = roleProxy;
						matchFactTypeForRole = roleProxy.TargetRole;
					}
					else if (null != (objectifiedUnaryRole = roleBase as ObjectifiedUnaryRole))
					{
						mirrorRole = objectifiedUnaryRole;
						matchFactTypeForRole = objectifiedUnaryRole.TargetRole;
					}
					else
					{
						objectifyingEntityRole = roleBase;
					}
				}
				if (matchFactTypeForRole != null &&
					objectifyingEntityRole != null)
				{
					// We match at the role level, not the fact level
					Role otherMatchingRole;
					if (null != (otherMatchingRole = CopyMergeUtility.GetEquivalentElement(matchFactTypeForRole, foreignStore, elementTracker)))
					{
						RoleBase otherMirrorRole = null;
						if (null != (roleProxy = otherMatchingRole.Proxy))
						{
							otherMirrorRole = roleProxy;
						}
						else if (null != (objectifiedUnaryRole = otherMatchingRole.ObjectifiedUnaryRole))
						{
							otherMirrorRole = objectifiedUnaryRole;
						}
						RoleBase otherObjectifyingEntityRole;
						if (otherMirrorRole != null &&
							null != (otherObjectifyingEntityRole = otherMirrorRole.OppositeRole))
						{
							roleOrder = new RoleBase[] { objectifyingEntityRole, mirrorRole };
							otherFactType = otherMirrorRole.FactType;
							otherRoleOrder = new RoleBase[] { otherObjectifyingEntityRole, otherMirrorRole };
						}
					}
				}
				impliedPopulation = true;
			}
			else
			{
				// Handle other fact type patterns based on predicate readings
				// UNDONE: COPYMERGE This would be a quick lookup if we kept a dictionary based on normalized predicate names.
				// Note that there are no proxies here, these were all handled in the previous branch
				int unaryRoleIndex = roleCount == 2 ? FactType.GetUnaryRoleIndex(roles).GetValueOrDefault(-1) : -1;
				if (unaryRoleIndex != -1)
				{
					implicitBooleanRole = roles[1 - unaryRoleIndex];
					roles = new RoleBase[] { roles[unaryRoleIndex] };
					roleCount = 1;
				}
				// UNDONE: COPYMERGE Should a role player match for a refmode scheme add the value type?
				// If so, then we need a way here to share code adding other elements.
				ObjectType[] matchingRolePlayers = new ObjectType[roleCount];
				ObjectType matchingRolePlayer;
				for (int i = 0; i < roleCount; ++i)
				{
					if (null == (matchingRolePlayer = roles[i].Role.RolePlayer) ||
						null == (matchingRolePlayer = CopyMergeUtility.GetEquivalentElement(matchingRolePlayer, foreignStore, elementTracker)))
					{
						return false; // All role players need a match or we're sunk. Note that the match may not be the same name.
					}
					matchingRolePlayers[i] = matchingRolePlayer;
				}
				// Find all played roles for each matching role player. If there is
				// a duplicate role player, then leave that slot blank. After getting
				// all role players, we will combined the sets and sort by fact type.
				// The fact types with a full role count will be the potential candidates
				// that can then be further verified by reading text.
				LinkedElementCollection<Role>[] allPlayedRoles = new LinkedElementCollection<Role>[roleCount];
				int totalPlayedRoles = 0;
				for (int i = 0; i < roleCount; ++i)
				{
					matchingRolePlayer = matchingRolePlayers[i];
					int j = i - 1;
					for (; j >= 0; --j)
					{
						if (matchingRolePlayers[j] == matchingRolePlayer)
						{
							break;
						}
					}
					if (j != -1)
					{
						// We've already process this set of played roles.
						continue;
					}
					totalPlayedRoles += (allPlayedRoles[i] = matchingRolePlayer.PlayedRoleCollection).Count;
				}
				if (totalPlayedRoles == 0)
				{
					return false;
				}
				FactTypeAndRole[] tracker = new FactTypeAndRole[totalPlayedRoles];
				int nextTrackedIndex = 0;
				for (int i = 0; i < roleCount; ++i)
				{
					LinkedElementCollection<Role> playedRoles = allPlayedRoles[i];
					if (playedRoles != null)
					{
						foreach (Role role in playedRoles)
						{
							FactType factType = role.FactType;
							tracker[nextTrackedIndex] = new FactTypeAndRole(role.FactType, role);
							++nextTrackedIndex;
						}
					}
				}

				Array.Sort<FactTypeAndRole>(
					tracker,
					delegate(FactTypeAndRole left, FactTypeAndRole right)
					{
						int retVal = left.FactType.Id.CompareTo(right.FactType.Id);
						if (retVal == 0)
						{
							retVal = left.Role.Id.CompareTo(right.Role.Id);
						}
						return retVal;
					});
				int nextFactTypeStart = 0;

				List<PopulatedReadingTextWithRoleOrder> orderedReplacementReadings = null;
				while (nextFactTypeStart < totalPlayedRoles)
				{
					int factTypeStart = nextFactTypeStart;
					FactType testFactType = tracker[factTypeStart].FactType;
					++nextFactTypeStart;
					for (int i = factTypeStart + 1; i < totalPlayedRoles; ++i)
					{
						if (tracker[i].FactType != testFactType)
						{
							break;
						}
						++nextFactTypeStart;
					}
					if ((nextFactTypeStart - factTypeStart) == roleCount)
					{
						// We have at least as many roles as we need. Make sure we
						// have the correct kind of fact type and no extra roles.
						if (testFactType.ImpliedByObjectification == null &&
							!(testFactType is SubtypeFact))
						{
							LinkedElementCollection<RoleBase> otherRoles = testFactType.RoleCollection;
							int otherRoleCount = otherRoles.Count;
							int otherUnaryRoleIndex = -1;
							if (otherRoleCount == roleCount ||
								(roleCount == 1 && otherRoleCount == 2 && -1 != (otherUnaryRoleIndex = FactType.GetUnaryRoleIndex(otherRoles).GetValueOrDefault(-1))))
							{
								// We have matching fact types, so we can turn to testing predicates for a match.
								if (orderedReplacementReadings == null)
								{
									orderedReplacementReadings = new List<PopulatedReadingTextWithRoleOrder>();
									foreach (ReadingOrder order in readingOrders = ReadingOrderCollection)
									{
										LinkedElementCollection<RoleBase> readingRoles = order.RoleCollection;
										foreach (Reading reading in order.ReadingCollection)
										{
											orderedReplacementReadings.Add(new PopulatedReadingTextWithRoleOrder(
												readingRoles,
												Reading.ReplaceFields(
													reading.Text,
													delegate(int replacementIndex)
													{
														if (replacementIndex < roleCount &&
															-1 != (replacementIndex = roles.IndexOf(readingRoles[replacementIndex])))
														{
															// Use the matched names, which may or may not be the current names
															return "{" + matchingRolePlayers[replacementIndex].Name + "}";
														}
														return null;
													})));
										}
									}
									if (orderedReplacementReadings.Count > 1)
									{
										orderedReplacementReadings.Sort(PopulatedReadingTextWithRoleOrder.Comparer);
									}
								}
								LinkedElementCollection<ReadingOrder> testReadingOrders;
								foreach (ReadingOrder order in testReadingOrders = testFactType.ReadingOrderCollection)
								{
									LinkedElementCollection<RoleBase> readingRoles = order.RoleCollection;
									foreach (Reading reading in order.ReadingCollection)
									{
										int matchIndex;
										if (0 <= (matchIndex = orderedReplacementReadings.BinarySearch(
											new PopulatedReadingTextWithRoleOrder(
												null,
												Reading.ReplaceFields(
													reading.Text,
													delegate(int replacementIndex)
													{
														if (replacementIndex < roleCount)
														{
															return "{" + readingRoles[replacementIndex].Role.RolePlayer.Name + "}";
														}
														return null;
													})),
											PopulatedReadingTextWithRoleOrder.Comparer)))
										{
											roleOrder = orderedReplacementReadings[matchIndex].ReadingRoles;
											otherFactType = testFactType;
											otherRoleOrder = readingRoles;
											otherReadingOrders = testReadingOrders;
											if (otherUnaryRoleIndex != -1)
											{
												otherImplicitBooleanRole = otherRoles[1 - otherUnaryRoleIndex];
											}
											nextFactTypeStart = totalPlayedRoles; // Break outer loop
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			if (otherFactType != null)
			{
				// Add primary fact type elements
				elementTracker.AddEquivalentElement(this, otherFactType);

				// Add all roles
				for (int i = 0; i < roleCount; ++i)
				{
					elementTracker.AddEquivalentElement(roleOrder[i], otherRoleOrder[i]);
					// UNDONE: COPYMERGE Map internal constraints, including adding deletion semantics for
					// constraints weaker than the new constraint
				}
				if (implicitBooleanRole != null)
				{
					elementTracker.AddEquivalentElement(implicitBooleanRole, otherImplicitBooleanRole);
					elementTracker.AddEquivalentElement(implicitBooleanRole.Role.RolePlayer, otherImplicitBooleanRole.Role.RolePlayer);
				}

				// Add reading orders and readings
				if ((readingOrders ?? (readingOrders = ReadingOrderCollection)).Count != 0)
				{
					int otherReadingOrderCount = (otherReadingOrders ?? (otherReadingOrders = otherFactType.ReadingOrderCollection)).Count;
					if (otherReadingOrderCount != 0)
					{
						BitTracker matchedOtherOrder = new BitTracker(otherReadingOrderCount);
						LinkedElementCollection<RoleBase>[] allOtherReadingRoles = null;
						int unmatchedOtherReadingOrderCount = otherReadingOrderCount;
						foreach (ReadingOrder readingOrder in readingOrders)
						{
							LinkedElementCollection<RoleBase> readingRoles = readingOrder.RoleCollection;
							for (int i = 0; i < otherReadingOrderCount; ++i)
							{
								if (!matchedOtherOrder[i])
								{
									ReadingOrder otherReadingOrder = otherReadingOrders[i];
									LinkedElementCollection<RoleBase> otherReadingRoles = (allOtherReadingRoles ?? (allOtherReadingRoles = new LinkedElementCollection<RoleBase>[otherReadingOrderCount]))[i] ?? (allOtherReadingRoles[i] = otherReadingOrder.RoleCollection);
									int j = 0;
									for (; j < roleCount; ++j)
									{
										if (otherReadingRoles[j] != elementTracker.GetEquivalentElement(readingRoles[j]))
										{
											break;
										}
									}
									if (j == roleCount)
									{
										// We have a match, record readings with an exact text match
										elementTracker.AddEquivalentElement(readingOrder, otherReadingOrder);
										LinkedElementCollection<Reading> otherReadings = otherReadingOrder.ReadingCollection;
										int otherReadingCount = otherReadings.Count;
										if (otherReadingCount != 0)
										{
											BitTracker matchedOtherReading = new BitTracker(otherReadingCount);
											int unmatchedOtherReadingCount = otherReadingCount;
											foreach (Reading reading in readingOrder.ReadingCollection)
											{
												string matchText = reading.Text;
												for (int k = 0; k < otherReadingCount; ++k)
												{
													if (!matchedOtherReading[k])
													{
														Reading otherReading = otherReadings[k];
														if (matchText == otherReading.Text)
														{
															elementTracker.AddEquivalentElement(reading, otherReading);
															matchedOtherReading[k] = true;
															--unmatchedOtherReadingCount;
															break;
														}
													}
												}
												if (unmatchedOtherReadingCount == 0)
												{
													break;
												}
											}
										}
										matchedOtherOrder[i] = true;
										--unmatchedOtherReadingOrderCount;
										break;
									}
								}
							}
							if (unmatchedOtherReadingOrderCount == 0)
							{
								break;
							}
						}
					}
				}

				// Add fact type instances
				if (!impliedPopulation)
				{
					LinkedElementCollection<FactTypeInstance> instances;
					int instanceCount;
					LinkedElementCollection<FactTypeInstance> otherInstances;
					int otherInstanceCount;
					if (0 != (instanceCount = (instances = FactTypeInstanceCollection).Count) &&
						0 != (otherInstanceCount = (otherInstances = otherFactType.FactTypeInstanceCollection).Count))
					{
						ObjectTypeInstance[] mappedInstances = null;
						Dictionary<ObjectTypeInstance[], int> instanceMap = null;
						int[] duplicatedAt = null;
						for (int i = 0; i < instanceCount; ++i)
						{
							FactTypeInstance instance = instances[i];
							foreach (FactTypeRoleInstance roleInstance in instance.RoleInstanceCollection)
							{
								ObjectTypeInstance objectTypeInstance = roleInstance.ObjectTypeInstance;
								ObjectTypeInstance otherObjectTypeInstance;
								int roleIndex;
								if (null != (otherObjectTypeInstance = CopyMergeUtility.GetEquivalentElement(objectTypeInstance, foreignStore, elementTracker)) &&
									-1 != (roleIndex = roleOrder.IndexOf(roleInstance.Role)))
								{
									(mappedInstances ?? (mappedInstances = new ObjectTypeInstance[roleCount]))[roleIndex] = otherObjectTypeInstance;
								}
								else
								{
									mappedInstances = null;
									break;
								}
							}
							if (mappedInstances != null)
							{
								int existingIndex;
								if (instanceMap == null)
								{
									instanceMap = new Dictionary<ObjectTypeInstance[], int>(ObjectTypeInstanceArrayEqualityComparer.Instance);
									instanceMap.Add(mappedInstances, i);
								}
								else if (instanceMap.TryGetValue(mappedInstances, out existingIndex))
								{
									// Track duplicates. The duplicated at value for existing index
									// points to the duplicate index, which points to the next duplicate, etc.
									// Note that 0 is the default value and means 'not duplicated'.
									if (duplicatedAt == null)
									{
										duplicatedAt = new int[instanceCount];
									}
									else
									{
										int duplicateIndex;
										while (0 != (duplicateIndex = duplicatedAt[existingIndex]))
										{
											existingIndex = duplicateIndex;
										}
									}
									duplicatedAt[existingIndex] = i;
								}
								else
								{
									instanceMap.Add(mappedInstances, i);
								}
								mappedInstances = null;
							}
							else
							{
								elementTracker.AddFailedEquivalentElement(instance);
							}
						}
						if (instanceMap != null)
						{
							// UNDONE: CLOSEDWORLDUNARY Closed world unary population will need
							// to store a value for the implied boolean role, which will mean that
							// the trivial unary role order here will fail. Currently, only the
							// unary role is populated, so this is sufficient.
							mappedInstances = new ObjectTypeInstance[roleCount];
							for (int i = 0; i < otherInstanceCount; ++i)
							{
								FactTypeInstance otherInstance = otherInstances[i];
								Array.Clear(mappedInstances, 0, roleCount);
								bool matchedAll = true;
								LinkedElementCollection<FactTypeRoleInstance> otherRoleInstances = otherInstance.RoleInstanceCollection;
								foreach (FactTypeRoleInstance otherRoleInstance in otherRoleInstances)
								{
									ObjectTypeInstance objectTypeInstance = otherRoleInstance.ObjectTypeInstance;
									int roleIndex;
									if (-1 == (roleIndex = otherRoleOrder.IndexOf(otherRoleInstance.Role)))
									{
										matchedAll = false;
										break;
									}
									mappedInstances[roleIndex] = otherRoleInstance.ObjectTypeInstance;
								}
								int existingIndex;
								if (matchedAll &&
									instanceMap.TryGetValue(mappedInstances, out existingIndex) &&
									existingIndex != -1)
								{
									FactTypeInstance instance = instances[existingIndex];
									elementTracker.AddEquivalentElement(instance, otherInstance);
									// Role instances allow duplicates and will not bind.
									foreach (FactTypeRoleInstance roleInstance in instance.RoleInstanceCollection)
									{
										Role findRole = (Role)otherRoleOrder[roleOrder.IndexOf(roleInstance.Role)];
										foreach (FactTypeRoleInstance otherRoleInstance in otherRoleInstances)
										{
											if (otherRoleInstance.Role == findRole)
											{
												elementTracker.AddEquivalentElement(roleInstance, otherRoleInstance);
												break;
											}
										}
									}
									if (null == duplicatedAt ||
										0 == (existingIndex = duplicatedAt[existingIndex]))
									{
										existingIndex = -1;
									}
									instanceMap[mappedInstances] = existingIndex;
								}
							}
							// Mark remaining items as unmatched
							foreach (int index in instanceMap.Values)
							{
								if (index != -1)
								{
									elementTracker.AddFailedEquivalentElement(instances[index]);
									if (duplicatedAt != null)
									{
										int duplicateIndex = index;
										while (0 != (duplicateIndex = duplicatedAt[duplicateIndex]))
										{
											elementTracker.AddFailedEquivalentElement(instances[duplicateIndex]);
										}
									}
								}
							}
						}
					}
				}
				return true;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // FactType class
	#region Role class
	partial class Role : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match a role based on its parent fact type, which requires full
		/// matching roles for equivalence.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return CopyMergeUtility.GetEquivalentElement(FactType, foreignStore, elementTracker) != null;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // Role class
	#region SubtypeFact class
	partial class SubtypeFact : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match a fact type based on a match of the supertype and subtype
		/// </summary>
		protected new bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ObjectType supertype;
			ObjectType otherSupertype;
			ObjectType subtype;
			ObjectType otherSubtype;
			if (null == (otherSupertype = CopyMergeUtility.GetEquivalentElement(supertype = Supertype, foreignStore, elementTracker)) ||
				null == (otherSubtype = CopyMergeUtility.GetEquivalentElement(subtype = Subtype, foreignStore, elementTracker)))
			{
				return false;
			}

			// Large numbers of subtypes are more likely that large numbers of supertypes.
			// Walk from the subtype to find a matching supertype.
			SubtypeFact otherSubtypeFact = null;
			ObjectType.WalkSupertypeRelationships(
				otherSubtype,
				delegate(SubtypeFact subtypeFact, ObjectType testSupertype, int depth)
				{
					if (testSupertype == otherSupertype)
					{
						otherSubtypeFact = subtypeFact;
						return ObjectTypeVisitorResult.Stop;
					}
					// Only look at direct supertypes
					return ObjectTypeVisitorResult.SkipChildren;
				});
			if (otherSubtypeFact != null)
			{
				elementTracker.AddEquivalentElement(this, otherSubtypeFact);
				// Also map roles and internal uniqueness constraints
				LinkedElementCollection<RoleBase> roles = RoleCollection;
				// NORMA puts subtype first by default, but this is officially unordered,
				// so support both orders.
				Role firstRole = roles[0].Role;
				SubtypeMetaRole subtypeRole = firstRole as SubtypeMetaRole;
				SupertypeMetaRole supertypeRole;
				if (subtypeRole != null)
				{
					supertypeRole = (SupertypeMetaRole)roles[1].Role;
				}
				else
				{
					subtypeRole = (SubtypeMetaRole)roles[1].Role;
					supertypeRole = (SupertypeMetaRole)firstRole;
				}
				roles = otherSubtypeFact.RoleCollection;
				firstRole = roles[0].Role;
				SubtypeMetaRole otherSubtypeRole = firstRole as SubtypeMetaRole;
				SupertypeMetaRole otherSupertypeRole;
				if (otherSubtypeRole != null)
				{
					otherSupertypeRole = (SupertypeMetaRole)roles[1].Role;
				}
				else
				{
					otherSubtypeRole = (SubtypeMetaRole)roles[1].Role;
					otherSupertypeRole = (SupertypeMetaRole)firstRole;
				}
				elementTracker.AddEquivalentElement(subtypeRole, otherSubtypeRole);
				elementTracker.AddEquivalentElement(supertypeRole, otherSupertypeRole);
				elementTracker.AddEquivalentElement(subtypeRole.SingleRoleAlethicMandatoryConstraint, otherSubtypeRole.SingleRoleAlethicMandatoryConstraint);
				elementTracker.AddEquivalentElement(subtypeRole.SingleRoleAlethicUniquenessConstraint, otherSubtypeRole.SingleRoleAlethicUniquenessConstraint);
				elementTracker.AddEquivalentElement(supertypeRole.SingleRoleAlethicUniquenessConstraint, otherSupertypeRole.SingleRoleAlethicUniquenessConstraint);
				return true;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // SubtypeFact class
	#region FactTypeInstance class
	partial class FactTypeInstance : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match an instance based on the parent <see cref="FactType"/>.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return CopyMergeUtility.GetEquivalentElement(FactType, foreignStore, elementTracker) != null &&
				null != elementTracker.GetEquivalentElement(this);
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // FactTypeInstance class
	#region SetConstraint class
	partial class SetConstraint : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match an external set constraint based on resolved roles. This defers
		/// to internal constraints for fact type matching.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			IConstraint constraint = (IConstraint)this;
			// UNDONE: COPYMERGE Handle internal constraints during fact type matching
			//if (constraint.ConstraintIsInternal)
			//{
			//    LinkedElementCollection<FactType> factTypes;
			//    if (1 == (factTypes = FactTypeCollection).Count &&
			//        null != CopyMergeUtility.GetEquivalentElement(factTypes[0], foreignStore, elementTracker) &&
			//        null != elementTracker.GetEquivalentElement(this))
			//    {
			//        return true;
			//    }
			//}
			//else
			{
				SetConstraint otherSetConstraint = null;
				SetConstraint otherSetConstraintMismatchedModality = null;
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> roleLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
				int roleCount = roleLinks.Count;
				if (roleCount != 0)
				{
					// Verify that we have matches for all roles first
					Role[] otherMatchingRoles = null;
					for (int i = 0; i < roleCount; ++i)
					{
						Role otherRole = CopyMergeUtility.GetEquivalentElement(roleLinks[i].Role, foreignStore, elementTracker);
						if (otherRole == null)
						{
							return false;
						}
						(otherMatchingRoles ?? (otherMatchingRoles = new Role[roleCount]))[i] = otherRole;
					}

					// Now go through connected role sequences and match role order
					ConstraintType matchConstraintType = constraint.ConstraintType;
					ConstraintModality matchConstraintModality = Modality;
					Role firstMatchingRole = otherMatchingRoles[0];
					foreach (ConstraintRoleSequence sequence in firstMatchingRole.ConstraintRoleSequenceCollection)
					{
						SetConstraint testOtherSetConstraint;
						LinkedElementCollection<Role> otherRoles;
						if (null != (testOtherSetConstraint = sequence as SetConstraint) &&
							((IConstraint)testOtherSetConstraint).ConstraintType == matchConstraintType &&
							roleCount == (otherRoles = sequence.RoleCollection).Count)
						{
							int i = 1;
							for (; i < roleCount; ++i)
							{
								if (otherRoles.IndexOf(otherMatchingRoles[i]) == -1)
								{
									break;
								}
							}
							if (i == roleCount)
							{
								// We have a match
								if (testOtherSetConstraint.Modality == matchConstraintModality)
								{
									otherSetConstraint = testOtherSetConstraint;
									break;
								}
								else if (otherSetConstraintMismatchedModality == null)
								{
									otherSetConstraintMismatchedModality = testOtherSetConstraint;
								}
							}
						}
					}
					if (null != (otherSetConstraint ?? (otherSetConstraint = otherSetConstraintMismatchedModality)))
					{
						elementTracker.AddEquivalentElement(this, otherSetConstraint);
						// Prepopulate constraint role equivalence so that join path projection
						// can rely on these if this constraint is mapped. This will be done
						// automatically (and less efficiently) at a later closure integration
						// stage if we do not do it here.
						ReadOnlyCollection<ConstraintRoleSequenceHasRole> otherConstraintRoleLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(otherSetConstraint);
						BitTracker matchedOtherConstraintRole = new BitTracker(roleCount);
						for (int i = 0; i < roleCount; ++i)
						{
							Role otherMatchingRole = otherMatchingRoles[i];
							for (int j = 0; j < roleCount; ++j)
							{
								if (!matchedOtherConstraintRole[j] &&
									otherConstraintRoleLinks[j].Role == otherMatchingRole)
								{
									matchedOtherConstraintRole[j] = true;
									elementTracker.AddEquivalentElement(roleLinks[i], otherConstraintRoleLinks[j]);
									break;
								}
							}
						}
						return true;
					}
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // SetConstraint class
	#region SetComparisonConstraint class
	partial class SetComparisonConstraint : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match an external set constraint based on resolved roles. This defers
		/// to internal constraints for fact type matching.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = RoleSequenceCollection;
			int sequenceCount = sequences.Count;
			int roleCount; // Note that we allow jagged (arity mismatch) sequences, roleCount is retrieved per row
			if (sequenceCount != 0)
			{
				// State tracking, allowing a match on all but modality followed
				// by a stronger match (including modality) later.
				SetComparisonConstraint otherSetComparisonConstraint = null;
				LinkedElementCollection<SetComparisonConstraintRoleSequence> otherSetComparisonSequences = null;
				// An array of matching original sequence numbers indexed by other sequence numbers. Matches are +1 so 0 means unmatched.
				int[] matchedOtherSequences = null;
				SetComparisonConstraint otherSetComparisonConstraintMismatchedModality = null;
				LinkedElementCollection<SetComparisonConstraintRoleSequence> otherSetComparisonSequencesMismatchedModality = null;
				int[] matchedOtherSequencesMismatchedModality = null; // Copy of matchedOtherSequences for modality mismatch.

				// First verify that we have matching roles for roles in all sequences
				Role[][] allOtherMatchingRoles = null;
				IConstraint constraint = (IConstraint)this;
				ConstraintType matchConstraintType = constraint.ConstraintType;
				ConstraintModality matchConstraintModality = Modality;
				Role[] otherMatchingRoles;
				for (int i = 0; i < sequenceCount; ++i)
				{
					LinkedElementCollection<Role> roles = sequences[i].RoleCollection;
					roleCount = roles.Count;
					if (roleCount == 0)
					{
						return false; // Highly unlikely, editors don't do this, and the sequences are deleted if it happens
					}
					otherMatchingRoles = null;
					for (int j = 0; j < roleCount; ++j)
					{
						Role otherRole = CopyMergeUtility.GetEquivalentElement(roles[j], foreignStore, elementTracker);
						if (otherRole == null)
						{
							return false;
						}
						(otherMatchingRoles ?? (otherMatchingRoles = new Role[roleCount]))[j] = otherRole;
					}
					(allOtherMatchingRoles ?? (allOtherMatchingRoles = new Role[sequenceCount][]))[i] = otherMatchingRoles;
				}

				// Now find matching sequences for each role
				bool matchSequenceOrder = 0 != (constraint.RoleSequenceStyles & RoleSequenceStyles.OrderedRoleSequences);
				otherMatchingRoles = allOtherMatchingRoles[0]; // Match the first sequence first
				roleCount = otherMatchingRoles.Length;
				Role firstMatchingRole = otherMatchingRoles[0];
				foreach (ConstraintRoleSequence sequence in firstMatchingRole.ConstraintRoleSequenceCollection)
				{
					SetComparisonConstraintRoleSequence testOtherSequence;
					SetComparisonConstraint testOtherConstraint;
					LinkedElementCollection<SetComparisonConstraintRoleSequence> testOtherSequences;
					LinkedElementCollection<Role> otherRoles;
					if (null != (testOtherSequence = sequence as SetComparisonConstraintRoleSequence) &&
						null != (testOtherConstraint = testOtherSequence.ExternalConstraint) &&
						((IConstraint)testOtherConstraint).ConstraintType == matchConstraintType &&
						sequenceCount == (testOtherSequences = testOtherConstraint.RoleSequenceCollection).Count &&
						roleCount == (otherRoles = sequence.RoleCollection).Count)
					{
						int otherSequenceIndex = testOtherSequences.IndexOf(testOtherSequence);
						if (matchSequenceOrder && 0 != otherSequenceIndex)
						{
							continue;
						}
						int i = 1;
						for (; i < roleCount; ++i)
						{
							if (otherRoles.IndexOf(otherMatchingRoles[i]) == -1)
							{
								break;
							}
						}
						if (i == roleCount)
						{
							// We've matched one sequence, match additional sequences as well.
							if (matchedOtherSequences == null)
							{
								matchedOtherSequences = new int[sequenceCount];
							}
							else
							{
								Array.Clear(matchedOtherSequences, 0, sequenceCount);
							}
							matchedOtherSequences[otherSequenceIndex] = 1; // Add 1 so zero is not matched
							LinkedElementCollection<Role>[] allOtherRoles = new LinkedElementCollection<Role>[sequenceCount];
							int trailingSequenceIndex = 1;
							for (; trailingSequenceIndex < sequenceCount; ++trailingSequenceIndex)
							{
								int testSequenceIndex;
								int testSequenceBound;
								if (matchSequenceOrder)
								{
									testSequenceIndex = trailingSequenceIndex;
									testSequenceBound = trailingSequenceIndex + 1;
								}
								else
								{
									testSequenceIndex = 0;
									testSequenceBound = sequenceCount;
								}
								otherMatchingRoles = allOtherMatchingRoles[trailingSequenceIndex];
								roleCount = otherMatchingRoles.Length;
								for (; testSequenceIndex < testSequenceBound; ++testSequenceIndex)
								{
									if (0 == matchedOtherSequences[testSequenceIndex])
									{
										otherRoles = allOtherRoles[testSequenceIndex] ?? (allOtherRoles[testSequenceIndex] = testOtherSequences[testSequenceIndex].RoleCollection);
										if (otherRoles.Count == roleCount)
										{
											for (i = 0; i < roleCount; ++i)
											{
												// UNDONE: COPYMERGE Be stricter on this match and require that non-jagged constraints
												// have all sequences match in the same order.
												if (otherRoles.IndexOf(otherMatchingRoles[i]) == -1)
												{
													break;
												}
											}
											if (i == roleCount)
											{
												// We have a match for this sequence
												matchedOtherSequences[testSequenceIndex] = trailingSequenceIndex + 1; // Index is adjusted when it is retrieved
												break;
											}
										}
									}
								}
								if (testSequenceIndex == testSequenceBound)
								{
									// We could not find a matching sequence, this is not a matching constraint
									break;
								}
							}

							if (trailingSequenceIndex == sequenceCount)
							{
								// We have a full constraint match on all sequences.
								if (testOtherConstraint.Modality == matchConstraintModality)
								{
									otherSetComparisonConstraint = testOtherConstraint;
									otherSetComparisonSequences = testOtherSequences;
									break; // We won't find a better match
								}
								else if (otherSetComparisonConstraintMismatchedModality == null)
								{
									otherSetComparisonConstraintMismatchedModality = testOtherConstraint;
									otherSetComparisonSequencesMismatchedModality = testOtherSequences;
									matchedOtherSequencesMismatchedModality = matchedOtherSequences;
									matchedOtherSequences = null;
									// Do not break, try to find a full modality match
								}
							}
						}
					}
				}
				if (otherSetComparisonConstraint == null &&
					otherSetComparisonConstraintMismatchedModality != null)
				{
					otherSetComparisonConstraint = otherSetComparisonConstraintMismatchedModality;
					otherSetComparisonSequences = otherSetComparisonSequencesMismatchedModality;
					matchedOtherSequences = matchedOtherSequencesMismatchedModality;
				}
				if (otherSetComparisonConstraint != null)
				{
					elementTracker.AddEquivalentElement(this, otherSetComparisonConstraint);
					for (int i = 0; i < sequenceCount; ++i)
					{
						int sequenceIndex = matchedOtherSequences[i] - 1;
						ConstraintRoleSequence sequence = sequences[sequenceIndex];
						ConstraintRoleSequence otherSequence = otherSetComparisonSequences[i];
						elementTracker.AddEquivalentElement(sequence, otherSequence);

						// Prepopulate constraint role equivalence so that join path projection
						// can rely on these if this constraint is mapped. This will be done
						// automatically (and less efficiently) at a later closure integration
						// stage if we do not do it here.
						ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoleLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(sequence);
						ReadOnlyCollection<ConstraintRoleSequenceHasRole> otherConstraintRoleLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(otherSequence);
						roleCount = constraintRoleLinks.Count;
						BitTracker matchedOtherConstraintRole = new BitTracker(roleCount);
						otherMatchingRoles = allOtherMatchingRoles[sequenceIndex];
						for (int j = 0; j < roleCount; ++j)
						{
							Role otherMatchingRole = otherMatchingRoles[j];
							for (int k = 0; k < roleCount; ++k)
							{
								if (!matchedOtherConstraintRole[k] &&
									otherConstraintRoleLinks[k].Role == otherMatchingRole)
								{
									matchedOtherConstraintRole[k] = true;
									elementTracker.AddEquivalentElement(constraintRoleLinks[j], otherConstraintRoleLinks[k]);
									break;
								}
							}
						}
					}
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // SetComparisonConstraint class
	#region ElementGrouping class
	partial class ElementGrouping : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match groups by name.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			foreach (ElementGroupingSet otherGroupingSet in foreignStore.ElementDirectory.FindElements<ElementGroupingSet>(false))
			{
				ElementGrouping otherGrouping = otherGroupingSet.GroupsDictionary.GetElement(Name).FirstElement as ElementGrouping;
				if (otherGrouping != null)
				{
					elementTracker.AddEquivalentElement(this, otherGrouping);
					LinkedElementCollection<ElementGroupingType> groupTypes;
					LinkedElementCollection<ElementGroupingType> otherGroupTypes;
					if (0 != (groupTypes = GroupingTypeCollection).Count &&
						0 != (otherGroupTypes = otherGrouping.GroupingTypeCollection).Count)
					{
						foreach (ElementGroupingType groupType in groupTypes)
						{
							Type testType = groupType.GetType();
							foreach (ElementGroupingType otherGroupType in otherGroupTypes)
							{
								if (testType == otherGroupType.GetType())
								{
									elementTracker.AddEquivalentElement(groupType, otherGroupType);
									break;
								}
							}
						}
					}
					return true;
				}
				break;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ElementGrouping class
	#region LeadRolePath class
	partial class LeadRolePath : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match paths by roots, pathed roles, and conditional calculations.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			RolePathOwner pathOwner = PathOwner;
			RolePathOwner otherPathOwner;
			if (null != (otherPathOwner = CopyMergeUtility.GetEquivalentElement(pathOwner, foreignStore, elementTracker)))
			{
				LinkedElementCollection<PathObjectUnifier> unifiers = null;
				LinkedElementCollection<CalculatedPathValue> conditions = null;
				LinkedElementCollection<CalculatedPathValue> calculations = null;
				int unifierCount = 0;
				int conditionCount = 0;
				int calculationCount = 0;
				int[] matchingOtherCalculations = null; // Array indexed into other calculations (index +1, zero means empty)
				int[] matchingOtherUnifiers = null; // Array indexed into other unifiers (index +1, zero means empty)
				BitTracker matchedOthers = default(BitTracker);
				Dictionary<ModelElement, ModelElement> preMatchedElements = null;
				LinkedElementCollection<LeadRolePath> allOwnedPaths = pathOwner.OwnedLeadRolePathCollection;
				if (allOwnedPaths.Count == 1)
				{
					allOwnedPaths = null;
				}
				foreach (LeadRolePath otherLeadRolePath in otherPathOwner.OwnedLeadRolePathCollection)
				{
					if (allOwnedPaths != null)
					{
						bool previouslyMatched = false;
						foreach (LeadRolePath ownedPath in allOwnedPaths)
						{
							if (ownedPath != this &&
								elementTracker.GetEquivalentElement(ownedPath) == otherLeadRolePath)
							{
								// Do a sanity check before we continue. It is possible for one owner
								// to have two equivalent paths (possibly with alternate projections, etc).
								// The merge framework does not track reverse elements, so we need to make
								// sure that we don't match with two different things. Without this check,
								// two projections can end up mapping the same lead role path. For the more
								// common case of a relatively small number of different paths, this also
								// avoids repetitions of the matching code below.
								previouslyMatched = true;
								break;
							}
						}
						if (previouslyMatched)
						{
							continue;
						}
					}
					if (preMatchedElements == null)
					{
						preMatchedElements = new Dictionary<ModelElement,ModelElement>();
					}
					else
					{
						preMatchedElements.Clear();
					}
					if (IsEquivalentPath(this, otherLeadRolePath, preMatchedElements, foreignStore, elementTracker))
					{
						// Verify all object unifiers
						if (unifiers == null)
						{
							unifiers = ObjectUnifierCollection;
							unifierCount = unifiers.Count;
						}
						LinkedElementCollection<PathObjectUnifier> otherUnifiers = otherLeadRolePath.ObjectUnifierCollection;
						if (unifierCount != otherUnifiers.Count)
						{
							continue;
						}
						else if (unifierCount != 0)
						{
							if (matchingOtherUnifiers == null)
							{
								matchingOtherUnifiers = new int[unifierCount];
							}
							else
							{
								Array.Clear(matchingOtherUnifiers, 0, unifierCount);
							}
							matchedOthers.Reset(unifierCount);
							int i = 0;
							for (; i < unifierCount; ++i)
							{
								PathObjectUnifier unifier = unifiers[i];
								LinkedElementCollection<PathedRole> unifiedRoles = unifier.PathedRoleCollection;
								LinkedElementCollection<RolePathObjectTypeRoot> unifiedRoots = unifier.PathRootCollection;
								int unifiedRoleCount = unifiedRoles.Count;
								int unifiedRootCount = unifiedRoots.Count;
								int j = 0;
								for (; j < unifierCount; ++j)
								{
									if (!matchedOthers[j])
									{
										PathObjectUnifier otherUnifier = otherUnifiers[j];
										LinkedElementCollection<PathedRole> otherUnifiedRoles = otherUnifier.PathedRoleCollection;
										if (otherUnifiedRoles.Count == unifiedRoleCount)
										{
											int k = 0;
											for (; k < unifiedRoleCount; ++k)
											{
												if (!otherUnifiedRoles.Contains((PathedRole)preMatchedElements[unifiedRoles[k]]))
												{
													break;
												}
											}
											if (k < unifiedRoleCount)
											{
												continue;
											}
										}
										else
										{
											continue;
										}
										LinkedElementCollection<RolePathObjectTypeRoot> otherUnifiedRoots = otherUnifier.PathRootCollection;
										if (otherUnifiedRoots.Count == unifiedRootCount)
										{
											int k = 0;
											for (; k < unifiedRootCount; ++k)
											{
												if (!otherUnifiedRoots.Contains((RolePathObjectTypeRoot)preMatchedElements[unifiedRoots[k]]))
												{
													break;
												}
											}
											if (k < unifiedRootCount)
											{
												continue;
											}
										}
										else
										{
											continue;
										}
										matchingOtherUnifiers[i] = j + 1;
										matchedOthers[j] = true;
										break;
									}
								}
								if (j == unifierCount)
								{
									break; // No match found
								}
							}
							if (i != unifierCount)
							{
								// Not a full match
								continue;
							}
						}
						
						// Verify all conditional functions. Additional functions are used as projections,
						// but do not change the path equality. So, condition functions are matched first
						// to test path equality, then remaining functions are filled in after path equality
						// has been established.
						if (conditions == null)
						{
							conditions = CalculatedConditionCollection;
							conditionCount = conditions.Count;
						}
						LinkedElementCollection<CalculatedPathValue> otherConditions = otherLeadRolePath.CalculatedConditionCollection;
						LinkedElementCollection<CalculatedPathValue> otherCalculations = null;
						if (conditionCount == otherConditions.Count)
						{
							if (conditionCount != 0)
							{
								if (calculations == null)
								{
									calculations = CalculatedValueCollection;
									calculationCount = calculations.Count;
								}
								otherCalculations = otherLeadRolePath.CalculatedValueCollection;
								if (matchingOtherCalculations == null)
								{
									matchingOtherCalculations = new int[calculationCount];
								}
								else
								{
									Array.Clear(matchingOtherCalculations, 0, calculationCount);
								}
								matchedOthers.Reset(conditionCount);
								int i = 0;
								for (; i < conditionCount; ++i)
								{
									CalculatedPathValue condition = conditions[i];
									int calculationIndex = calculations.IndexOf(condition);
									int j = 0;
									for (; j < conditionCount; ++j)
									{
										if (!matchedOthers[j])
										{
											if ((matchingOtherCalculations[calculationIndex] - 1) == j ||
												IsEquivalentCalculation(calculations, otherCalculations, i, j, matchingOtherCalculations, preMatchedElements, foreignStore, elementTracker))
											{
												// This condition was picked as as the input to another calculation
												matchedOthers[j] = true;
												break;
											}
										}
									}
									if (j == conditionCount)
									{
										break;
									}
								}
								if (i < conditionCount)
								{
									continue;
								}
							}

							// All conditions match, the paths are equivalent.
							// Now try top pair up any remaining unmapped functions.
							if (calculations == null)
							{
								calculations = CalculatedValueCollection;
								calculationCount = calculations.Count;
							}
							if (calculationCount != 0)
							{
								int otherCalculationCount;
								if (otherCalculations != null)
								{
									otherCalculationCount = otherCalculations.Count;
								}
								else
								{
									otherCalculations = otherLeadRolePath.CalculatedValueCollection;
									otherCalculationCount = otherCalculations.Count;
								}
								if (otherCalculationCount != 0)
								{
									matchedOthers.Reset(otherCalculationCount);
									int alreadyMatchedCount = 0;
									if (matchingOtherCalculations == null)
									{
										matchingOtherCalculations = new int[calculationCount];
									}
									else
									{
										for (int i = 0; i < calculationCount; ++i)
										{
											int alreadyMatchedIndex = matchingOtherCalculations[i];
											if (alreadyMatchedIndex != 0)
											{
												matchedOthers[alreadyMatchedIndex - 1] = true;
												++alreadyMatchedCount;
											}
										}
									}
									if (alreadyMatchedCount < otherCalculationCount &&
										alreadyMatchedCount < calculationCount)
									{
										for (int i = 0; i < calculationCount; ++i)
										{
											if (0 == matchingOtherCalculations[i])
											{
												CalculatedPathValue calculation = calculations[i];
												for (int j = 0; j < otherCalculationCount; ++j)
												{
													if (!matchedOthers[j] &&
														IsEquivalentCalculation(calculations, otherCalculations, i, j, matchingOtherCalculations, preMatchedElements, foreignStore, elementTracker))
													{
														matchedOthers[j] = true;
														++alreadyMatchedCount;
													}
												}
											}
										}
									}
								}
							}

							// Register prematched paths, path roots, pathed roles
							foreach (KeyValuePair<ModelElement, ModelElement> pair in preMatchedElements)
							{
								elementTracker.AddEquivalentElement(pair.Key, pair.Value);
							}

							// Register unifiers
							if (matchingOtherUnifiers != null)
							{
								for (int i = 0; i < unifierCount; ++i)
								{
									elementTracker.AddEquivalentElement(unifiers[i], otherUnifiers[matchingOtherUnifiers[i] - 1]);
								}
							}
							
							// Register matched functions and inputs
							if (matchingOtherCalculations != null)
							{
								for (int i = 0; i < calculationCount; ++i)
								{
									int otherCalculationIndex = matchingOtherCalculations[i];
									if (otherCalculationIndex != 0)
									{
										CalculatedPathValue calculation = calculations[i];
										CalculatedPathValue otherCalculation = otherCalculations[otherCalculationIndex - 1];
										elementTracker.AddEquivalentElement(calculation, otherCalculation);
										LinkedElementCollection<CalculatedPathValueInput> otherInputs = null;
										foreach (CalculatedPathValueInput input in calculation.InputCollection)
										{
											FunctionParameter otherParameter = elementTracker.GetEquivalentElement(input.Parameter) as FunctionParameter;
											if (otherParameter != null)
											{
												foreach (CalculatedPathValueInput otherInput in (otherInputs ?? (otherInputs = otherCalculation.InputCollection)))
												{
													if (otherInput.Parameter == otherParameter)
													{
														elementTracker.AddEquivalentElement(input, otherInput);
														break;
													}
												}
											}
										}
									}
								}
							}
							return true;
						}
					}
				}
			}
			return false;
		}
		/// <summary>
		/// Helper for <see cref="MapEquivalentElements"/>
		/// </summary>
		/// <param name="allCalculations">All available calculations</param>
		/// <param name="allOtherCalculations">All available calculations to match</param>
		/// <param name="calculationIndex">The index in <paramref name="allCalculations"/> to compare.</param>
		/// <param name="otherCalculationIndex">The index in <paramref name="allOtherCalculations"/> to compare to.</param>
		/// <param name="matchedCalculations">An array of currently matched other calculations. Values are indices in the allOtherCalculations
		/// list (+1 so 0 is empty), indexed by the allCalculations index.</param>
		/// <param name="matchedPathElements">A dictionary of pathed role, path root, and path elements matched during subpath iteration</param>
		/// <param name="foreignStore">The foreign store to match</param>
		/// <param name="elementTracker">The element tracker</param>
		/// <returns><see langword="true"/> if the calculations are equivalent. Also files in the <paramref name="matchedCalculations"/>
		/// indices as needed.</returns>
		private static bool IsEquivalentCalculation(
			LinkedElementCollection<CalculatedPathValue> allCalculations,
			LinkedElementCollection<CalculatedPathValue> allOtherCalculations,
			int calculationIndex,
			int otherCalculationIndex,
			int[] matchedCalculations,
			Dictionary<ModelElement, ModelElement> matchedPathElements,
			Store foreignStore,
			IEquivalentElementTracker elementTracker)
		{
			int existingMatch = matchedCalculations[calculationIndex];
			if (existingMatch != 0)
			{
				return (existingMatch - 1) == otherCalculationIndex;
			}
			CalculatedPathValue calculation = allCalculations[calculationIndex];
			CalculatedPathValue otherCalculation = allOtherCalculations[otherCalculationIndex];
			Function calculationFunction;
			Function otherCalculationFunction;
			if (null != (calculationFunction = calculation.Function) &&
				null != (otherCalculationFunction = otherCalculation.Function) &&
				null != (otherCalculationFunction = CopyMergeUtility.GetEquivalentElement(calculationFunction, foreignStore, elementTracker)))
			{
				LinkedElementCollection<CalculatedPathValueInput> inputs = null;
				LinkedElementCollection<CalculatedPathValueInput> otherInputs = null;
				bool mismatchedParameter = false;
				foreach (FunctionParameter parameter in calculationFunction.ParameterCollection)
				{
					// Note that in some cases a function with fewer 'other' parameters can match,
					// so we ignore a a current parameter that has no existing parameter in the
					// foreign store. We require equivalent input sources for all mapped parameters.
					FunctionParameter otherParameter = elementTracker.GetEquivalentElement(parameter) as FunctionParameter;
					if (otherParameter != null)
					{
						bool sawBothParameters = false;
						foreach (CalculatedPathValueInput input in (inputs ?? (inputs = calculation.InputCollection)))
						{
							if (input.Parameter == parameter)
							{
								foreach (CalculatedPathValueInput otherInput in (otherInputs ?? (otherInputs = otherCalculation.InputCollection)))
								{
									if (otherInput.Parameter == otherParameter)
									{
										sawBothParameters = true;

										// Test set/bag options on aggregate inputs
										if (parameter.BagInput &&
											input.DistinctValues != otherInput.DistinctValues)
										{
											mismatchedParameter = true;
											break;
										}

										// Test the inputs
										PathedRole sourcePathedRole;
										RolePathObjectTypeRoot sourcePathRoot;
										CalculatedPathValue sourceCalculation;
										PathConstant sourcePathConstant;
										if (null != (sourcePathedRole = input.SourcePathedRole))
										{
											PathedRole otherSourcePathedRole = otherInput.SourcePathedRole;
											if (otherSourcePathedRole == null ||
												matchedPathElements[sourcePathedRole] != otherSourcePathedRole)
											{
												mismatchedParameter = true;
											}
										}
										else if (null != (sourcePathRoot = input.SourcePathRoot))
										{
											RolePathObjectTypeRoot otherSourcePathRoot = otherInput.SourcePathRoot;
											if (otherSourcePathRoot == null ||
												matchedPathElements[sourcePathRoot] != otherSourcePathRoot)
											{
												mismatchedParameter = true;
											}
										}
										else if (null != (sourceCalculation = input.SourceCalculatedValue))
										{
											CalculatedPathValue otherSourceCalculation = otherInput.SourceCalculatedValue;
											if (otherSourceCalculation == null ||
												!IsEquivalentCalculation(allCalculations, allOtherCalculations, allCalculations.IndexOf(sourceCalculation), allOtherCalculations.IndexOf(otherSourceCalculation), matchedCalculations, matchedPathElements, foreignStore, elementTracker))
											{
												mismatchedParameter = true;
											}
										}
										else if (null != (sourcePathConstant = input.SourceConstant))
										{
											PathConstant otherSourcePathConstant = otherInput.SourceConstant;
											if (otherSourcePathConstant == null ||
												sourcePathConstant.LexicalValue != otherSourcePathConstant.LexicalValue)
											{
												mismatchedParameter = true;
											}
										}
										else
										{
											mismatchedParameter = true; // Do not treat parameters with no inputs as equal
										}
										break;
									}
								}
								break;
							}
						}
						if (!sawBothParameters)
						{
							mismatchedParameter = true;
							break;
						}
						else if (mismatchedParameter)
						{
							break;
						}
					}
				}
				if (!mismatchedParameter)
				{
					matchedCalculations[calculationIndex] = otherCalculationIndex + 1; // Adjusted so 0 means empty
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Helper for <see cref="MapEquivalentElements"/>
		/// </summary>
		/// <param name="path">A <see cref="RolePath"/> to match.</param>
		/// <param name="otherPath">The foreign <see cref="RolePath"/> to compare it to.</param>
		/// <param name="matchedPathElements">A dictionary to store matches, abandon if this returns false. On return, contains mappings for path roots, path roles, and the path being tested.</param>
		/// <param name="foreignStore">The foreign <see cref="Store"/>.</param>
		/// <param name="elementTracker">The tracker used to determine element matches.</param>
		/// <returns></returns>
		private static bool IsEquivalentPath(RolePath path, RolePath otherPath, Dictionary<ModelElement, ModelElement> matchedPathElements, Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			// Check easily accessible properties first
			if (path.SplitCombinationOperator != otherPath.SplitCombinationOperator ||
				path.SplitIsNegated != otherPath.SplitIsNegated)
			{
				return false;
			}

			// Check the path root
			RolePathObjectTypeRoot pathRoot;
			if (null != (pathRoot = path.PathRoot))
			{
				RolePathObjectTypeRoot otherPathRoot;
				if (null == (otherPathRoot = otherPath.PathRoot) ||
					otherPathRoot.RootObjectType != CopyMergeUtility.GetEquivalentElement(pathRoot.RootObjectType, foreignStore, elementTracker))
				{
					return false;
				}
				matchedPathElements[pathRoot] = otherPathRoot;
			}
			else if (null != otherPath.PathRoot)
			{
				return false;
			}

			// Check pathed roles on this path
			ReadOnlyCollection<PathedRole> pathedRoles = path.PathedRoleCollection;
			ReadOnlyCollection<PathedRole> otherPathedRoles = otherPath.PathedRoleCollection;
			int childElementCount = pathedRoles.Count;
			if (childElementCount != otherPathedRoles.Count)
			{
				return false;
			}
			for (int i = 0; i < childElementCount; ++i)
			{
				PathedRole pathedRole = pathedRoles[i];
				PathedRole otherPathedRole = otherPathedRoles[i];
				if (pathedRole.PathedRolePurpose != otherPathedRole.PathedRolePurpose ||
					pathedRole.IsNegated != otherPathedRole.IsNegated ||
					null == CopyMergeUtility.GetEquivalentElement(pathedRole.Role, foreignStore, elementTracker))
				{
					return false;
				}
				matchedPathElements[pathedRole] = otherPathedRole;
			}

			// Check subpaths. Note that these are ordered for display and file consistency
			// purposes, but the order is not logically significant, so we attempt to match
			// different ordered paths.
			LinkedElementCollection<RoleSubPath> subPaths = path.SubPathCollection;
			LinkedElementCollection<RoleSubPath> otherSubPaths = otherPath.SubPathCollection;
			childElementCount = subPaths.Count;
			if (childElementCount != otherSubPaths.Count)
			{
				return false;
			}
			if (childElementCount != 0)
			{
				BitTracker matchedOtherPaths = new BitTracker(childElementCount);
				for (int i = 0; i < childElementCount; ++i)
				{
					RolePath childPath = subPaths[i];
					int j = 0;
					for (; j < childElementCount; ++j)
					{
						if (!matchedOtherPaths[j])
						{
							if (IsEquivalentPath(childPath, otherSubPaths[j], matchedPathElements, foreignStore, elementTracker))
							{
								matchedOtherPaths[j] = true;
								break;
							}
						}
					}
					if (j == childElementCount)
					{
						return false;
					}
				}
			}
			matchedPathElements[path] = otherPath;
			return true;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // LeadRolePath class
	#region PathedRole class
	partial class PathedRole : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match the pathed role based on the associated lead role path.
		/// IElementEquivalence is implemented on path components that are referenced from
		/// outside the path structure.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			LeadRolePath leadRolePath;
			return null != (leadRolePath = RolePath.RootRolePath) &&
				CopyMergeUtility.GetEquivalentElement(leadRolePath, foreignStore, elementTracker) != null &&
				elementTracker.GetEquivalentElement(this) != null;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // PathedRole class
	#region RolePathObjectTypeRoot class
	partial class RolePathObjectTypeRoot : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match the path root based on the associated lead role path.
		/// IElementEquivalence is implemented on path components that are referenced from
		/// outside the path structure.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			LeadRolePath leadRolePath;
			return null != (leadRolePath = RolePath.RootRolePath) &&
				CopyMergeUtility.GetEquivalentElement(leadRolePath, foreignStore, elementTracker) != null &&
				elementTracker.GetEquivalentElement(this) != null;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // RolePathObjectTypeRoot class
	#region CalculatedPathValue class
	partial class CalculatedPathValue : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match the calculation based on the associated lead role path.
		/// IElementEquivalence is implemented on path components that are referenced from
		/// outside the path structure.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return CopyMergeUtility.GetEquivalentElement(LeadRolePath, foreignStore, elementTracker) != null &&
				elementTracker.GetEquivalentElement(this) != null;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // CalculatedPathValue class
	#region CalculatedPathValueInput class
	partial class CalculatedPathValueInput : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match the calculation input based on the associated lead role path.
		/// IElementEquivalence is implemented on path components that are referenced from
		/// outside the path structure or nest 1-1 mapped elements.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CalculatedPathValue calculation;
			LeadRolePath rolePath;
			if (null != (calculation = CalculatedValue) &&
				null != (rolePath = calculation.LeadRolePath) &&
				CopyMergeUtility.GetEquivalentElement(rolePath, foreignStore, elementTracker) != null)
			{
				return elementTracker.GetEquivalentElement(this) != null;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // CalculatedPathValueInput class
	#region RecognizedPhrase class
	partial class RecognizedPhrase : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// Match phrases by name.
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			foreach (ORMModel otherModel in foreignStore.ElementDirectory.FindElements<ORMModel>(false))
			{
				RecognizedPhrase otherPhrase = otherModel.RecognizedPhrasesDictionary.GetElement(Name).FirstElement as RecognizedPhrase;
				if (otherPhrase != null)
				{
					elementTracker.AddEquivalentElement(this, otherPhrase);
					return true;
				}
				break;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // RecognizedPhrase class
	#region NameAlias class
	partial class NameAlias : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// </summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ModelElement aliasOwner;
			ModelElement otherAliasOwner;
			if (null != (aliasOwner = Element) &&
				null != (otherAliasOwner = CopyMergeUtility.GetEquivalentElement(aliasOwner, foreignStore, elementTracker)))
			{
				// Match by consumer and usage classes. Use the internal class information
				// instead of the serialized string forms. If we end up copying one of these
				// with no matching class information then we'll delete it at the end of
				// the merge integration phase. Note that we match by id because the meta
				// information is in a different store.
				// UNDONE: COPYMERGE We need a way for an element to add domain model requirements
				// without referencing elements in that model.
				DomainClassInfo classInfo = myConsumerDomainClass;
				bool hasConsumer = classInfo != null;
				Guid consumerId = hasConsumer ? classInfo.Id : Guid.Empty;
				classInfo = myUsageDomainClass;
				bool hasUsage = classInfo != null;
				Guid usageId = hasUsage ? classInfo.Id : Guid.Empty;
				foreach (NameAlias otherAlias in ElementHasAlias.GetAliasCollection(otherAliasOwner))
				{
					if (consumerId == (null != (classInfo = otherAlias.myConsumerDomainClass) ? classInfo.Id : Guid.Empty) &&
						usageId == (null != (classInfo = otherAlias.myUsageDomainClass) ? classInfo.Id : Guid.Empty))
					{
						elementTracker.AddEquivalentElement(this, otherAlias);
						return true;
					}
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // NameAlias class
	#region ModelNote class
	partial class ModelNote : IElementEquivalence
	{
		/// <summary>
		/// Implements <see cref="IElementEquivalence.MapEquivalentElements"/>
		/// </summary>
		protected new bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			foreach (ORMModel otherModel in foreignStore.ElementDirectory.FindElements<ORMModel>(false))
			{
				string matchText = Text;
				foreach (ModelNote otherNote in otherModel.NoteCollection)
				{
					if (otherNote.Text == matchText)
					{
						elementTracker.AddEquivalentElement(this, otherNote);
						return true;
					}
				}
				break;
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ModelNote class
}
