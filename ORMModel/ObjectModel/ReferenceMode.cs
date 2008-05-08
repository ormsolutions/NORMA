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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Neumont.Tools.Modeling;
using System.Text.RegularExpressions;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IReferenceModePattern interface
	/// <summary>
	/// A read-only interface representing a reference mode pattern.
	/// </summary>
	public interface IReferenceModePattern
	{
		/// <summary>
		/// The name of the reference mode
		/// </summary>
		string Name { get;}
		/// <summary>
		/// The FormatString used to combine the name of an EntityType
		/// and the name of the ReferenceMode into a ValueType name.
		/// Field {0} is the name of the EntityType and field {1} is the
		/// name of the reference mode.
		/// </summary>
		string FormatString { get;}
		/// <summary>
		/// The <see cref="T:ReferenceModeType"/>
		/// </summary>
		ReferenceModeType ReferenceModeType { get;}
	}
	#endregion // IReferenceModePattern interface
	#region ReferenceMode class
	partial class ReferenceMode : IComparable<ReferenceMode>, IReferenceModePattern
	{
		#region ReferenceMode specific
		/// <summary>
		/// The FormatString used for reference modes. All
		/// format strings must be unique across the model.
		/// FormatString should use the replacement field {0}
		/// for the entity name and {1} for refmode name
		/// </summary>
		public virtual string FormatString
		{
			get
			{
				string retVal = null;
				ReferenceModeKind kind = this.Kind;
				if (kind != null)
				{
					retVal = kind.FormatString;
				}
				return (retVal == null || retVal.Length == 0) ? Name : retVal;
			}
		}
		/// <summary>
		/// Given an entity name, generate the value type
		/// name that would correspond to the entity name for
		/// this reference type.
		/// </summary>
		/// <param name="entityName">The name of the associated entity type</param>
		/// <returns>Formatted string</returns>
		public string GenerateValueTypeName(string entityName)
		{
			return GenerateValueTypeName(entityName, this.FormatString, this.Name);
		}

		/// <summary>
		/// Given an entity name, generate the value type
		/// name that would correspond to the entity name for
		/// this reference type.
		/// </summary>
		/// <param name="entityName">The name of the associated entity type</param>
		/// <param name="formatString">The format string to use to generate the name.</param>
		/// <param name="referenceModeName">The reference Mode Name to use to generate the name.</param>
		/// <returns>Formatted string</returns>
		public string GenerateValueTypeName(string entityName, string formatString, string referenceModeName)
		{
			return string.Format(CultureInfo.InvariantCulture, formatString, entityName, referenceModeName);
		}

		/// <summary>
		/// Overridden ToString for Reference Modes
		/// </summary>
		/// <returns>Returns Reference Mode name</returns>
		public override string ToString()
		{
			return this.Name;
		}
		private PortableDataType dataType = PortableDataType.Unspecified;
		/// <summary>
		/// The intial <see cref="PortableDataType"/> to used when a new ValueType
		/// is created for this reference mode
		/// </summary>
		public PortableDataType Type
		{
			get
			{
				return dataType;
			}
			set
			{
				dataType = value;
			}
		}
		#endregion // ReferenceMode specific
		#region IReferenceModePattern implementation
		/// <summary>
		/// Implements <see cref="IReferenceModePattern.ReferenceModeType"/>
		/// </summary>
		protected ReferenceModeType ReferenceModeType
		{
			get
			{
				ReferenceModeKind kind = this.Kind;
				return (kind != null) ? kind.ReferenceModeType : ReferenceModeType.General;
			}
		}
		ReferenceModeType IReferenceModePattern.ReferenceModeType
		{
			get
			{
				return ReferenceModeType;
			}
		}
		#endregion // IReferenceModePattern implementation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the intrinsic reference modes and upgrades old reference
		/// mode patterns to the current set.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ReferenceModeFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds intrinsic reference
		/// mode implementations
		/// </summary>
		private sealed class ReferenceModeFixupListener : DeserializationFixupListener<ORMModel>
		{
			/// <summary>
			/// ReferenceModeFixupListener constructor
			/// </summary>
			public ReferenceModeFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddIntrinsicElements)
			{
			}
			protected override void  ProcessElement(ORMModel model, Store store, INotifyElementAdded notifyAdded)
			{
				// Ensure that each model has a reference mode kind for each
				// reference mode type
				ReferenceModeKind generalKind = null;
				ReferenceModeKind popularKind = null;
				ReferenceModeKind unitBasedKind = null;
				LinkedElementCollection<ReferenceModeKind> kinds = ModelHasReferenceModeKind.GetReferenceModeKindCollection(model);
				int nextKind = 0;
				int kindCount = kinds.Count;
				for (; nextKind < kindCount;) // Weird loop to allow deletion of overlapping kinds
				{
					ReferenceModeKind kind = kinds[nextKind];
					ReferenceModeKind upgradeToKind = null;
					switch (kind.ReferenceModeType)
					{
						case ReferenceModeType.General:
							if (generalKind == null)
							{
								generalKind = kind;
								generalKind.FormatString = "{1}"; // Enforce
							}
							else
							{
								// There is nothing special to say here, just delete the extra
								kind.Delete();
							}
							break;
						case ReferenceModeType.Popular:
							if (popularKind == null)
							{
								popularKind = kind;
							}
							else
							{
								upgradeToKind = popularKind;
							}
							break;
						case ReferenceModeType.UnitBased:
							if (unitBasedKind == null)
							{
								unitBasedKind = kind;
							}
							else
							{
								upgradeToKind = unitBasedKind;
							}
							break;
					}
					if (upgradeToKind != null)
					{
						string upgradeToFormatString = upgradeToKind.FormatString;
						bool upgradeAssociates = !kind.FormatString.Equals(upgradeToFormatString);
						foreach (ReferenceMode mode in kind.ReferenceModeCollection)
						{
							if (upgradeAssociates)
							{
								if (mode.FormatString.Equals(upgradeToFormatString))
								{
									CustomReferenceMode customMode = mode as CustomReferenceMode;
									if (customMode != null)
									{
										customMode.CustomFormatString = "";
									}
								}
								else
								{
									foreach (ObjectType entity in mode.AssociatedEntityTypeCollection(null, null))
									{
										entity.RenameReferenceMode(mode.GenerateValueTypeName(entity.Name, upgradeToFormatString, mode.Name));
									}
								}
							}
							mode.Kind = popularKind;
						}
						kind.Delete();
						--kindCount;
					}
					else
					{
						++nextKind;
					}
				}
				if (generalKind == null)
				{
					new ReferenceModeKind(
						store,
						new PropertyAssignment(ReferenceModeKind.ReferenceModeTypeDomainPropertyId, ReferenceModeType.General),
						new PropertyAssignment(ReferenceModeKind.FormatStringDomainPropertyId, "{1}")).Model = model;
				}
				if (popularKind == null)
				{
					(popularKind = new ReferenceModeKind(
						store,
						new PropertyAssignment(ReferenceModeKind.ReferenceModeTypeDomainPropertyId, ReferenceModeType.Popular),
						new PropertyAssignment(ReferenceModeKind.FormatStringDomainPropertyId, "{0}_{1}"))).Model = model;
				}
				if (unitBasedKind == null)
				{
					(unitBasedKind = new ReferenceModeKind(
						store,
						new PropertyAssignment(ReferenceModeKind.ReferenceModeTypeDomainPropertyId, ReferenceModeType.UnitBased),
						new PropertyAssignment(ReferenceModeKind.FormatStringDomainPropertyId, "{1}Value"))).Model = model;
				}

				// Now populate all intrinsic reference modes, tracking names as we go.
				// We used to allow duplicate names as long as the format strings were
				// unique, but we no longer allow this. The reference mode name is unique
				// in the model.
				LinkedElementCollection<ReferenceMode> modes = model.ReferenceModeCollection;
				int customCount = modes.Count;
				Action<ReferenceMode> newIntrinsicAction;
				// The values in this dictionary indicate:
				// 0 = intrinsic reference mode
				// < 0 = bitwise complement of custom reference mode that has not yet been renumbered
				// > 0 = last positive unique number for a reference mode with this name
				Dictionary<string, int> duplicateReferenceModeTracker;
				if (customCount == 0)
				{
					newIntrinsicAction = delegate(ReferenceMode newMode){};
					duplicateReferenceModeTracker = null;
				}
				else
				{
					duplicateReferenceModeTracker = new Dictionary<string, int>();
					newIntrinsicAction = delegate(ReferenceMode newMode)
					{
						duplicateReferenceModeTracker.Add(newMode.Name, 0);
					};
				}
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "Id", PortableDataType.NumericAutoCounter));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "id", PortableDataType.NumericAutoCounter));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "ID", PortableDataType.NumericAutoCounter));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "Name", PortableDataType.TextVariableLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "name", PortableDataType.TextVariableLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "Code", PortableDataType.TextFixedLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "code", PortableDataType.TextFixedLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "Title", PortableDataType.TextVariableLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "title", PortableDataType.TextVariableLength));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "Nr", PortableDataType.NumericSignedInteger));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "nr", PortableDataType.NumericSignedInteger));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, popularKind, "#", PortableDataType.NumericSignedInteger));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "kg", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mm", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "cm", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "km", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mile", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "Celsius", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "Fahrenheit", PortableDataType.NumericDecimal));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "USD", PortableDataType.NumericMoney));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "AUD", PortableDataType.NumericMoney));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "EUR", PortableDataType.NumericMoney));
				newIntrinsicAction(CreateIntrinsicReferenceMode(store, model, unitBasedKind, "CE", PortableDataType.TemporalDate));

				// UNDONE: The following loop assumes the format strings are all unique. Handle this case (requires hand edit or merge in the .orm file)
				for (int i = 0; i < customCount; ++i)
				{
					ReferenceMode mode = modes[i];
					string modeName = mode.Name;
					int existingIndex;
					if (duplicateReferenceModeTracker.TryGetValue(modeName, out existingIndex))
					{
						// We need to rename
						if (existingIndex < 0)
						{
							// Rename the old one
							ReferenceMode previousMode = modes[~existingIndex];
							existingIndex = 1;
							DecorateModeName(duplicateReferenceModeTracker, modeName, modes[~existingIndex], ref existingIndex);
						}
						++existingIndex;
						DecorateModeName(duplicateReferenceModeTracker, modeName, modes[i], ref existingIndex);
						duplicateReferenceModeTracker[modeName] = existingIndex;
					}
					else
					{
						duplicateReferenceModeTracker.Add(modeName, ~i);
					}
				}
				
				// UNDONE: Get rid of the notion of intrinsic reference mode by creating a settings files that is
				// used as the source of reference modes that are not current used in the model
			}
			private static void DecorateModeName(Dictionary<string, int> duplicateReferenceModeTracker, string modeName, ReferenceMode mode, ref int decoratorIndex)
			{
				string newModeName = modeName + decoratorIndex.ToString();
				while (duplicateReferenceModeTracker.ContainsKey(newModeName))
				{
					++decoratorIndex;
					newModeName = modeName + decoratorIndex.ToString();
				}
				foreach (ObjectType entity in mode.AssociatedEntityTypeCollection(null, null))
				{
					entity.RenameReferenceMode(mode.GenerateValueTypeName(entity.Name, mode.FormatString, newModeName));
				}
				// Treat the new name as an intrinsic, add in case it conflicts with a later
				// mode with the same name.
				duplicateReferenceModeTracker.Add(newModeName, 0);
				mode.Name = newModeName;
			}
			private static ReferenceMode CreateIntrinsicReferenceMode(Store store, ORMModel model, ReferenceModeKind kind, string referenceModeName, PortableDataType dataType)
			{
				IntrinsicReferenceMode refMode = new IntrinsicReferenceMode(store);
				refMode.Name = referenceModeName;
				refMode.Kind = kind;
				refMode.Model = model;
				refMode.Type = dataType;
				return refMode;
			}
		}
		#endregion // Deserialization Fixup
		#region Reference Mode Name Generation
		/// <summary>
		/// Get the reference mode name combined with standard
		/// decorators indicating a popular or unitbased reference mode.
		/// </summary>
		public string DecoratedName
		{
			get
			{
				string decoratedName = Name;
				ReferenceModeKind kind = Kind;
				if (kind != null)
				{
					switch (kind.ReferenceModeType)
					{
						case ReferenceModeType.Popular:
							decoratedName = "." + decoratedName;
							break;
						case ReferenceModeType.UnitBased:
							decoratedName = decoratedName + ":";
							break;
					}
				}
				return decoratedName;
			}
		}
		/// <summary>
		/// Given the Entity and the the value type created the ref mode name
		/// </summary>
		/// <param name="valueTypeName">The name of the valuetype attached
		/// to the preferred identifier role.</param>
		/// <param name="entityTypeName">The name of the entity type.</param>
		/// <param name="model">The model that owns the reference modes</param>
		/// <returns>A ReferenceMode instance, or null</returns>
		public static ReferenceMode FindReferenceModeFromEntityNameAndValueName(string valueTypeName, string entityTypeName, ORMModel model)
		{
			ReferenceMode retVal = null;
			foreach (ReferenceMode mode in model.ReferenceModeCollection)
			{
				if (valueTypeName == mode.GenerateValueTypeName(entityTypeName))
				{
					retVal = mode;
					break;
				}
			}

			return retVal;
		}
		/// <summary>
		/// Finds all entity types using this reference mode
		/// </summary>
		/// <param name="alternateFormatString">A format string to use that is not the current format string. Defaults to the current FormatString if null.</param>
		/// <param name="alternateReferenceModeName">A reference mode name to use that is not the current reference mode name. Defaults to the Name if null.</param>
		/// <returns>IEnumerable&lt;ObjectType&gt;</returns>
		public IEnumerable<ObjectType> AssociatedEntityTypeCollection(string alternateFormatString, string alternateReferenceModeName)
		{
			ORMModel model = Model;
			if (model == null)
			{
				yield break;
			}
			if (string.IsNullOrEmpty(alternateFormatString))
			{
				alternateFormatString = FormatString;
			}
			if (string.IsNullOrEmpty(alternateReferenceModeName))
			{
				alternateReferenceModeName = Name;
			}
			foreach (EntityTypeHasPreferredIdentifier link in model.Store.ElementDirectory.FindElements<EntityTypeHasPreferredIdentifier>())
			{
				ObjectType entity = link.PreferredIdentifierFor;
				if (model == entity.Model)
				{
					UniquenessConstraint constraint = link.PreferredIdentifier;
					LinkedElementCollection<Role> roles;
					ObjectType oppositeValueType;
					if (constraint.IsInternal &&
						1 == (roles = constraint.RoleCollection).Count &&
						null != (oppositeValueType = roles[0].RolePlayer) &&
						oppositeValueType.IsValueType &&
						oppositeValueType.Name == GenerateValueTypeName(entity.Name, alternateFormatString, alternateReferenceModeName))
					{
						yield return entity;
					}
				}
			}
		}

		/// <summary>
		/// Given a reference mode name and an <see cref="ORMModel"/>, return all reference modes with that
		/// name in the model.
		/// </summary>
		/// <param name="referenceModeName">The name of the reference mode(s) to locate</param>
		/// <param name="model">The containing model</param>
		/// <returns>Always returns a collection, does not return null</returns>
		public static IList<ReferenceMode> FindReferenceModesByName(string referenceModeName, ORMModel model)
		{
			// Choice of implementation: We need to either make two passes to allocate the
			// right size array, always create a Collection, or delay create the collection
			// for the (relatively uncommon) case when we have multiple reference modes of the
			// same name. Implement the third option.
			List<ReferenceMode> multiples = null;
			ReferenceMode single = null;

			foreach (ReferenceMode referenceMode in model.ReferenceModeCollection)
			{
				if (referenceMode.Name == referenceModeName)
				{
					if (multiples != null)
					{
						multiples.Add(referenceMode);
					}
					else if (single != null)
					{
						multiples = new List<ReferenceMode>();
						multiples.Add(single);
						multiples.Add(referenceMode);
						single = null;
					}
					else
					{
						single = referenceMode;
					}
				}
			}
			if (multiples != null)
			{
				return multiples;
			}
			else if (single != null)
			{
				return new ReferenceMode[] { single };
			}
			else
			{
				return new ReferenceMode[0];
			}
		}
		/// <summary>
		/// Returns a single <see cref="ReferenceMode"/> matching the optionally decorated
		/// reference mode name. If the name is decorated and the requested reference mode
		/// is not defined in the model, then create one with the <see cref="ReferenceModeType"/>
		/// indicated by the decorated name.
		/// </summary>
		/// <param name="decoratedName">The name of the reference mode(s) to locate</param>
		/// <param name="model">The containing model</param>
		/// <param name="forceCreate">Should a new <see cref="ReferenceMode"/> be created if a match
		/// is not currently in the model?</param>
		/// <returns>An existing or new <see cref="ReferenceMode"/>, or <see langword="null"/> if
		/// the name is not decorated and a mode by the requested name cannot be found.</returns>
		/// <exception cref="InvalidOperationException">If the requested name is not unique.</exception>
		public static ReferenceMode GetReferenceModeForDecoratedName(string decoratedName, ORMModel model, bool forceCreate)
		{
			ReferenceMode retVal = null;
			ReferenceModeType newModeType = ReferenceModeType.General;
			int decoratedNameLength = decoratedName.Length;
			if (decoratedNameLength > 1)
			{
				if (decoratedName[0] == '.')
				{
					newModeType = ReferenceModeType.Popular;
					decoratedName = decoratedName.Substring(1, decoratedNameLength - 1);
				}
				else if (decoratedName[decoratedNameLength - 1] == ':')
				{
					newModeType = ReferenceModeType.UnitBased;
					decoratedName = decoratedName.Substring(0, decoratedNameLength - 1);
				}
			}

			// Find the unique reference mode for this object type and reference mode string
			IList<ReferenceMode> referenceModes = ReferenceMode.FindReferenceModesByName(decoratedName, model);
			int modeCount = referenceModes.Count;
			if (modeCount == 1)
			{
				retVal = referenceModes[0];
			}
			else if (modeCount == 0 && newModeType != ReferenceModeType.General)
			{
				if (forceCreate)
				{
					retVal = new CustomReferenceMode(model.Store, new PropertyAssignment(ReferenceMode.NameDomainPropertyId, decoratedName));
					foreach (ReferenceModeKind kind in model.ReferenceModeKindCollection)
					{
						if (kind.ReferenceModeType == newModeType)
						{
							retVal.Kind = kind;
							break;
						}
					}
					retVal.Model = model;
				}
			}
			else if (modeCount > 1)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeAmbiguousName);
			}
			return retVal;
		}
		#endregion //Reference Mode Name Generation
		#region CustomStorage handlers
		private ReferenceModeKind GetKindDisplayValue()
		{
			return Kind;
		}
		private void SetKindDisplayValue(ReferenceModeKind newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Kind = newValue;
			}
		}
		#endregion // CustomStorage handlers
		#region FixedValueTypeName cache
		private string myFixedValueTypeName;
		/// <summary>
		/// Return the name of a ValueType that would always be
		/// associated with the current structure of this <see cref="ReferenceMode"/>,
		/// or <see langword="null"/> if this is not a General or UnitBased <see cref="ReferenceMode"/>
		/// </summary>
		public string FixedValueTypeName
		{
			get
			{
				string retVal = myFixedValueTypeName;
				if (retVal == null)
				{
					retVal = "";
					ReferenceModeKind kind;
					if (null != (kind = this.Kind) &&
						kind.ReferenceModeType != ReferenceModeType.Popular)
					{
						retVal = GenerateValueTypeName("\x1", this.FormatString, this.Name);
						if (retVal.Contains("\x1"))
						{
							// Sanity test, violates the expected patterns
							retVal = "";
						}
					}
					myFixedValueTypeName = retVal;
				}
				return (retVal.Length != 0) ? retVal : null;
			}
		}
		/// <summary>
		/// Internal accessor for integration with <see cref="ORMModel.GetReferenceModeForValueType"/>
		/// </summary>
		internal string ResetFixedValueTypeName()
		{
			// Internal justification: the FixedValueTypeName property is used in conjunction with
			// the ORMModel.GetReferenceModeForValueType method. This internal property is used to
			// get internal access to the field so that the per-model lookup and the internals
			// of the backing field can be synchronized without placing a second lookup dictionary
			// in the model.
			string retVal = myFixedValueTypeName;
			myFixedValueTypeName = null;
			return retVal;
		}
		/// <summary>
		/// Get the single ValueType in this model associated with this <see cref="ReferenceMode"/>
		/// There is a one-to-one mapping between UnitBased and General reference modes and
		/// the ValueType in the model.
		/// </summary>
		public ObjectType FixedValueType
		{
			get
			{
				string valueTypeName;
				ORMModel model;
				ObjectType matchedObjectType;
				if (null != (valueTypeName = FixedValueTypeName) &&
					null != (model = this.Model) &&
					null != (matchedObjectType = model.ObjectTypesDictionary.GetElement(valueTypeName).SingleElement as ObjectType) &&
					matchedObjectType.IsValueType)
				{
					return matchedObjectType;
				}
				return null;
			}
		}
		#endregion // FixedValueTypeName cache
		#region IComparable<ReferenceMode> Members

		/// <summary>
		/// Defines how to compare to ReferenceMode.  Used in sorting reference modes by name.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public int CompareTo(ReferenceMode other)
		{
			return this.Name.CompareTo(other.Name);
		}
		#endregion
		#region IndexMapRegex helper
		/// <summary>
		/// A regex pattern to extract the replacement index from a reading replacement field
		/// </summary>
		private static Regex myIndexMapRegex;
		private static Regex IndexMapRegex
		{
			get
			{
				Regex regexIndexMap = myIndexMapRegex;
				if (regexIndexMap == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myIndexMapRegex,
						new Regex(
							@"(?n)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					regexIndexMap = myIndexMapRegex;
				}
				return regexIndexMap;
			}
		}
		#endregion // IndexMapRegex helper
		#region ReferenceModeKindChangeRule
		/// <summary>
		/// ChangeRule: typeof(ReferenceModeKind)
		/// Update value types in corresponding reference mode patterns when format string changes
		/// </summary>
		private static void ReferenceModeKindChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == ReferenceModeKind.FormatStringDomainPropertyId)
			{
				ReferenceModeKind kind = e.ModelElement as ReferenceModeKind;
				string newFormatString = kind.FormatString;
				switch (kind.ReferenceModeType)
				{
					case ReferenceModeType.General:
						if (newFormatString != "{1}")
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceGeneralPattern);
						}
						break;
					case ReferenceModeType.Popular:
						{
							bool seenRefModeName = false;
							bool seenEntityName = false;
							Match match = IndexMapRegex.Match(newFormatString);
							while (match.Success)
							{
								string stringReplaceIndex = match.Groups["ReplaceIndex"].Value;
								int replaceIndex;
								if (int.TryParse(stringReplaceIndex, out replaceIndex))
								{
									if (replaceIndex == 0)
									{
										seenEntityName = true;
									}
									else if (replaceIndex == 1)
									{
										seenRefModeName = true;
									}
									else
									{
										seenRefModeName = seenEntityName = false;
										break;
									}
								}
								else
								{
									seenRefModeName = seenEntityName = false;
									break;
								}
								match = match.NextMatch();
							}
							if (!(seenEntityName && seenRefModeName))
							{
								throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeKindEnforcePopularPattern);
							}
						}
						break;
					case ReferenceModeType.UnitBased:
						{
							bool seenRefModeName = false;
							bool seenInvalidIndex = false;
							Match match = IndexMapRegex.Match(newFormatString);
							while (match.Success)
							{
								string stringReplaceIndex = match.Groups["ReplaceIndex"].Value;
								int replaceIndex;
								if (int.TryParse(stringReplaceIndex, out replaceIndex))
								{
									if (replaceIndex == 1)
									{
										seenRefModeName = true;
									}
									else
									{
										seenInvalidIndex = true;
										break;
									}
								}
								else
								{
									seenInvalidIndex = true;
									break;
								}
								match = match.NextMatch();
							}
							if (!seenRefModeName || seenInvalidIndex)
							{
								throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeKindEnforceUnitBasedPattern);
							}
						}
						break;
				}
				string oldFormatString = (string)e.OldValue;
				ORMModel model = kind.Model;
				EnsureUnique(kind, model);

				foreach (ReferenceMode mode in kind.ReferenceModeCollection)
				{
					CustomReferenceMode customReferenceMode = mode as CustomReferenceMode;
					if (customReferenceMode != null)
					{
						string customFormatString = customReferenceMode.CustomFormatString;
						if (customFormatString.Length != 0)
						{
							if (customFormatString == newFormatString)
							{
								customReferenceMode.CustomFormatString = "";
							}
							continue;
						}
					}
					foreach (ObjectType entity in mode.AssociatedEntityTypeCollection(oldFormatString, null))
					{
						string newName = mode.GenerateValueTypeName(entity.Name);
						entity.RenameReferenceMode(newName);
					}
				}
			}
		}
		/// <summary>
		/// Throws an exception if the format strings are not unique
		/// </summary>
		private static void EnsureUnique(ReferenceModeKind newKind, ORMModel model)
		{
			string newFormatString = newKind.FormatString;
			foreach (ReferenceModeKind kind in model.ReferenceModeKindCollection)
			{
				if (newKind != kind && newFormatString == kind.FormatString)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeKindEnforceUniqueFormatString);
				}
			}
		}
		#endregion // ReferenceModeKindChangeRule
		#region CustomReferenceModeChangeRule
		/// <summary>
		/// ChangeRule: typeof(CustomReferenceMode)
		/// Update value types when format string or reference mode name changes
		/// </summary>
		private static void CustomReferenceModeChangeRule(ElementPropertyChangedEventArgs e)
		{
			CustomReferenceMode mode = e.ModelElement as CustomReferenceMode;
			ORMModel model = mode.Model;
			EnsureUnique(mode, model);

			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == CustomReferenceMode.CustomFormatStringDomainPropertyId)
			{
				string customFormatString = (string)e.NewValue;
				ReferenceModeKind kind = mode.Kind;
				if (!string.IsNullOrEmpty(customFormatString))
				{
					switch (kind.ReferenceModeType)
					{
						case ReferenceModeType.General:
							if (customFormatString != "{1}")
							{
								throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceGeneralPattern);
							}
							break;
						case ReferenceModeType.Popular:
							{
								bool seenInvalidIndex = false;
								bool seenEntityName = false;
								Match match = IndexMapRegex.Match(customFormatString);
								while (match.Success)
								{
									string stringReplaceIndex = match.Groups["ReplaceIndex"].Value;
									int replaceIndex;
									if (int.TryParse(stringReplaceIndex, out replaceIndex))
									{
										if (replaceIndex == 0)
										{
											seenEntityName = true;
										}
										else if (replaceIndex != 1)
										{
											seenInvalidIndex = true;
											break;
										}
									}
									else
									{
										seenInvalidIndex = true;
										break;
									}
									match = match.NextMatch();
								}
								if (!seenEntityName || seenInvalidIndex || (seenEntityName && customFormatString == "{0}"))
								{
									throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforcePopularPattern);
								}
							}
							break;
						case ReferenceModeType.UnitBased:
							{
								bool seenInvalidIndex = false;
								Match match = IndexMapRegex.Match(customFormatString);
								while (match.Success)
								{
									string stringReplaceIndex = match.Groups["ReplaceIndex"].Value;
									int replaceIndex;
									if (int.TryParse(stringReplaceIndex, out replaceIndex))
									{
										if (replaceIndex != 1)
										{
											seenInvalidIndex = true;
											break;
										}
									}
									else
									{
										seenInvalidIndex = true;
										break;
									}
									match = match.NextMatch();
								}
								if (seenInvalidIndex)
								{
									throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceUnitBasedPattern);
								}
							}
							break;
					}
				}
				string oldFormatString = (string)e.OldValue;
				string defaultFormatString = kind.FormatString;
				if (string.IsNullOrEmpty(customFormatString))
				{
					if (oldFormatString.Equals(defaultFormatString))
					{
						return;
					}
					customFormatString = defaultFormatString;
				}
				else if (customFormatString.Equals(defaultFormatString))
				{
					// Run this rule a second time, let it return from the above branch
					mode.CustomFormatString = "";
				}
				if (string.IsNullOrEmpty(oldFormatString))
				{
					oldFormatString = mode.Kind.FormatString;
				}

				foreach (ObjectType entity in mode.AssociatedEntityTypeCollection(oldFormatString, null))
				{
					string newName = mode.GenerateValueTypeName(entity.Name);
					entity.RenameReferenceMode(newName);
				}
			}
			else if (attributeId == NameDomainPropertyId)
			{
				string newRefModeName = (string)e.NewValue;
				if (newRefModeName.Length > 0)
				{
					string oldRefModeName = (string)e.OldValue;

					foreach (ObjectType entity in mode.AssociatedEntityTypeCollection(null, oldRefModeName))
					{
						string newName = mode.GenerateValueTypeName(entity.Name);
						entity.RenameReferenceMode(newName);
					}
				}
			}
		}
		#endregion // CustomReferenceModeChangeRule
		#region ReferenceModeAddedRule
		/// <summary>
		/// AddRule: typeof(ModelHasReferenceMode), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Make sure that every added reference mode has a valid
		/// reference mode kind. Default to general.
		/// </summary>
		private static void ReferenceModeAddedRule(ElementAddedEventArgs e)
		{
			ModelHasReferenceMode link = e.ModelElement as ModelHasReferenceMode;
			ReferenceMode mode = link.ReferenceMode;
			// Make sure we have a reference kind
			if (mode.Kind == null)
			{
				foreach (ReferenceModeKind kind in link.Model.ReferenceModeKindCollection)
				{
					if (kind.ReferenceModeType == ReferenceModeType.General)
					{
						mode.Kind = kind;
						break;
					}
				}
			}

			// Test that the format string is compliant
			EnsureUnique(mode, link.Model);
		}
		/// <summary>
		/// Throws an exception if the format strings are not unique
		/// </summary>
		protected static void EnsureUnique(ReferenceMode newMode, ORMModel model)
		{
			if (model != null)
			{
				const string entityName = "\x1";
				string newFormatString = newMode.GenerateValueTypeName(entityName);
				string newModeName = newMode.Name;
				foreach (ReferenceMode mode in model.ReferenceModeCollection)
				{
					if (newMode != mode)
					{
						if (newModeName == mode.Name)
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceUniqueModeName);
						}
						if (newFormatString == mode.GenerateValueTypeName(entityName))
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceUniqueFormatString);
						}
					}
				}
			}
		}
		#endregion // ReferenceModeAddedRule
	}
	#endregion // ReferenceMode class
	#region CustomReferenceMode class
	public partial class CustomReferenceMode : IComparable<CustomReferenceMode>
	{
		/// <summary>
		/// Allow an override of the virtual FormatString
		/// property by setting the CustomFormatString property.
		/// </summary>
		public override string FormatString
		{
			get
			{
				string custString = CustomFormatString;
				if (custString != null && custString.Length > 0)
				{
					return custString;
				}
				else
				{
					return base.FormatString;
				}
			}
		}
		#region IComparable<CustomReferenceMode> Members
		int IComparable<CustomReferenceMode>.CompareTo(CustomReferenceMode other)
		{
			return base.CompareTo(other);
		}
		#endregion
	}
	#endregion // CustomReferenceMode class
	#region IntrinsicReferenceMode class
	public partial class IntrinsicReferenceMode : IComparable<IntrinsicReferenceMode>
	{
		#region IComparable<IntrinsicReferenceMode> Members

		/// <summary>
		/// Implements IComparable&lt;IntrinsicReferenceMode&gt;.CompareTo
		/// </summary>
		protected int CompareTo(IntrinsicReferenceMode other)
		{
			return base.CompareTo(other);
		}
		int IComparable<IntrinsicReferenceMode>.CompareTo(IntrinsicReferenceMode other)
		{
			return CompareTo(other);
		}
		#endregion
	}
	#endregion // CustomReferenceMode class
	#region ReferenceModeKind class
	/// <summary>
	/// Reference Mode Kind's ToString implementation
	/// </summary>
	public partial class ReferenceModeKind : IComparable<ReferenceModeKind>
	{
		#region Properties
		/// <summary>
		/// Overriding ToString to return the Reference Mode Type name
		/// </summary>
		/// <returns>Reference Mode Type name as the string</returns>
		public override string ToString()
		{
			return Utility.GetLocalizedEnumName<ReferenceModeType>(ReferenceModeType);
		}
		#endregion //properties
		#region IComparable<ReferenceModeKind> Members
		/// <summary>
		/// Defines how to compare to ReferenceModeKind.  Used in sorting reference mode kinds by name.
		/// </summary>
		public int CompareTo(ReferenceModeKind other)
		{
			return ToString().CompareTo(other.ToString());
		}
		#endregion
	}
	#endregion //ReferenceModeKind class
	#region ReferenceModeHasReferenceModeKind
	public partial class ReferenceModeHasReferenceModeKind
	{
		#region ReferenceModeHasReferenceModeKindRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(ReferenceModeHasReferenceModeKind)
		/// Update value types when reference mode kind changes
		/// </summary>
		private static void ReferenceModeHasReferenceModeKindRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id.Equals(ReferenceModeHasReferenceModeKind.KindDomainRoleId))
			{
				ReferenceModeHasReferenceModeKind link = e.ElementLink as ReferenceModeHasReferenceModeKind;
				ReferenceModeKind newKind = (ReferenceModeKind)e.NewRolePlayer;
				Debug.Assert(newKind == link.Kind);
				ReferenceModeKind oldKind = (ReferenceModeKind)e.OldRolePlayer;

				ReferenceMode mode = link.ReferenceMode;
				CustomReferenceMode customReferenceMode = mode as CustomReferenceMode;
				if (customReferenceMode != null)
				{
					string oldFormatString = customReferenceMode.CustomFormatString;
					if (oldFormatString == null || oldFormatString.Length == 0)
					{
						oldFormatString = oldKind.FormatString;
					}
					customReferenceMode.CustomFormatString = "";

					foreach (ObjectType entity in customReferenceMode.AssociatedEntityTypeCollection(oldFormatString, null))
					{
						string newName = customReferenceMode.GenerateValueTypeName(entity.Name);
						entity.RenameReferenceMode(newName);
					}
				}
				else if (mode is IntrinsicReferenceMode)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeIntrinsicRefModesDontChange);
				}
			}
		}
		#endregion // ReferenceModeHasReferenceModeKindRolePlayerChangeRule
		#region ReferenceModeHasReferenceModeKindDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ReferenceModeHasReferenceModeKind)
		/// Disallow removal of kind
		/// </summary>
		private static void ReferenceModeHasReferenceModeKindDeletingRule(ElementDeletingEventArgs e)
		{
			ReferenceModeHasReferenceModeKind link = e.ModelElement as ReferenceModeHasReferenceModeKind;

			ReferenceMode mode = link.ReferenceMode as ReferenceMode;
			if (mode != null && !mode.IsDeleting)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeReferenceModesKindNotEmpty);
			}
		}
		#endregion // ReferenceModeHasReferenceModeKindDeletingRule
	}
	#endregion
	#region ORMModel ReferenceMode/ValueType mapping cache
	partial class ORMModel
	{
		private Dictionary<string, ReferenceMode> myReferenceModeByFixedValueTypeDictionary;
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="ReferenceMode"/>s.
		/// Used to maintain cached state for the <see cref="ReferenceMode.FixedValueTypeName"/> and <see cref="GetReferenceModeForValueType"/> methods.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		private static void ManageReferenceModeModelStateEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			DomainClassInfo classInfo;
			classInfo = dataDir.FindDomainRelationship(ModelHasReferenceMode.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReferenceModeAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReferenceModeDeletedEvent), action);
			classInfo = dataDir.FindDomainRelationship(ReferenceModeHasReferenceModeKind.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(ReferenceModeKindChangedEvent), action);
			classInfo = dataDir.FindDomainClass(ReferenceModeKind.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReferenceModeKindPropertyChangedEvent), action);
			classInfo = dataDir.FindDomainClass(CustomReferenceMode.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(CustomReferenceModePropertyChangedEvent), action);
		}
		private static void ReferenceModeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ModelHasReferenceMode link = (ModelHasReferenceMode)e.ModelElement;
			if (!link.IsDeleted)
			{
				link.Model.SynchronizeFixedReferenceModeMap(link.ReferenceMode);
			}
		}
		private static void ReferenceModeDeletedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelHasReferenceMode link = (ModelHasReferenceMode)e.ModelElement;
			ORMModel model = link.Model;
			if (model.IsDeleted)
			{
				// Delete the dictionary, easier than synchronizing a deleted state
				model.myReferenceModeByFixedValueTypeDictionary = null;
			}
			else
			{
				string oldName = link.ReferenceMode.ResetFixedValueTypeName();
				Dictionary<string, ReferenceMode> dictionary;
				if (!string.IsNullOrEmpty(oldName) &&
					null != (dictionary = link.Model.myReferenceModeByFixedValueTypeDictionary))
				{
					dictionary.Remove(oldName);
				}
			}
		}
		private static void ReferenceModeKindChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			ReferenceModeHasReferenceModeKind link = (ReferenceModeHasReferenceModeKind)e.ElementLink;
			ORMModel model;
			ReferenceMode mode;
			if (!link.IsDeleted &&
				null != (model = (mode = link.ReferenceMode).Model))
			{
				model.SynchronizeFixedReferenceModeMap(mode);
			}
		}
		private static void ReferenceModeKindPropertyChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ReferenceModeKind modeKind = (ReferenceModeKind)e.ModelElement;
			ORMModel model;
			if (!modeKind.IsDeleted &&
				modeKind.ReferenceModeType != ReferenceModeType.Popular &&
				null != (model = modeKind.Model) &&
				null != model.myReferenceModeByFixedValueTypeDictionary)
			{
				foreach (ReferenceMode mode in modeKind.ReferenceModeCollection)
				{
					model.SynchronizeFixedReferenceModeMap(mode);
				}
			}
		}
		private static void CustomReferenceModePropertyChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ReferenceMode mode = (ReferenceMode)e.ModelElement;
			ORMModel model;
			if (!mode.IsDeleted &&
				null != (model = mode.Model))
			{
				model.SynchronizeFixedReferenceModeMap(mode);
			}
		}
		private void SynchronizeFixedReferenceModeMap(ReferenceMode referenceMode)
		{
			Dictionary<string, ReferenceMode> dictionary = myReferenceModeByFixedValueTypeDictionary;
			if (dictionary != null)
			{
				string oldName = referenceMode.ResetFixedValueTypeName();
				string newName = referenceMode.FixedValueTypeName;
				bool haveNewName = newName != null;
				if (!string.IsNullOrEmpty(oldName))
				{
					if (haveNewName && 0 == string.CompareOrdinal(oldName, newName))
					{
						ReferenceMode oldMode;
						if (dictionary.TryGetValue(oldName, out oldMode) && oldMode == referenceMode)
						{
							haveNewName = false;
						}
					}
					else
					{
						dictionary.Remove(oldName);
					}
				}
				if (haveNewName)
				{
					dictionary[newName] = referenceMode;
				}
			}
			else
			{
				referenceMode.ResetFixedValueTypeName();
			}
		}
		/// <summary>
		/// Given <see cref="ObjectType"/> that is a ValueType, return the
		/// associated <see cref="ReferenceMode"/> object that matches the
		/// name of the ValueType. Returns non-null values for UnitBased and
		/// explicitly specific General reference mode patterns.
		/// </summary>
		public ReferenceMode GetReferenceModeForValueType(ObjectType valueType)
		{
			if (IsDeleted || !valueType.IsValueType)
			{
				return null;
			}
			Dictionary<string, ReferenceMode> dictionary = myReferenceModeByFixedValueTypeDictionary;
			if (dictionary == null)
			{
				myReferenceModeByFixedValueTypeDictionary = dictionary = new Dictionary<string, ReferenceMode>();
				foreach (ReferenceMode referenceMode in ReferenceModeCollection)
				{
					string valueTypeName = referenceMode.FixedValueTypeName;
					if (valueTypeName != null)
					{
						dictionary[valueTypeName] = referenceMode;
					}
				}
			}
			ReferenceMode retVal;
			if (dictionary.TryGetValue(valueType.Name, out retVal))
			{
				return retVal;
			}
			return null;
		}
	}
	#endregion // ORMModel ReferenceMode/ValueType mapping cache
}
