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
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ReferenceMode class
	[TypeConverter(typeof(Design.ReferenceModeConverter))]
	public abstract partial class ReferenceMode : IComparable<ReferenceMode>
	{
		private PortableDataType dataType = PortableDataType.Unspecified;

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
		/// <param name="formatString"></param>
		/// <returns>Formatted string</returns>
		public string GenerateValueTypeName(string entityName, string formatString)
		{
			return GenerateValueTypeName(entityName, formatString, this.Name);
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


		#endregion // ReferenceMode specific
		#region Kind's Generated Accessor Code
		/// <summary>
		/// Property to change the reference mode kind. Modification of
		/// standard code spit to generate a role player change instead of
		/// a delete/add when the property is changed.
		/// </summary>
		public ReferenceModeKind Kind
		{
			// UNDONE: 2006-06 DSL Tools port: Does this property even need to exist?
			get
			{
				ReferenceModeHasReferenceModeKind goodLink = null;
				ReadOnlyCollection<ReferenceModeHasReferenceModeKind> links = DomainRoleInfo.GetElementLinks<ReferenceModeHasReferenceModeKind>(this, ReferenceModeHasReferenceModeKind.ReferenceModeDomainRoleId);
				foreach (ReferenceModeHasReferenceModeKind link in links)
				{
					if (!link.IsDeleted)
					{
						goodLink = link;
						break;
					}
				}
				return (goodLink != null) ? goodLink.Kind : null;
			}
			set
			{
				ReadOnlyCollection<ReferenceModeHasReferenceModeKind> links = DomainRoleInfo.GetElementLinks<ReferenceModeHasReferenceModeKind>(this, ReferenceModeHasReferenceModeKind.ReferenceModeDomainRoleId);
				foreach (ReferenceModeHasReferenceModeKind link in links)
				{
					if (!link.IsDeleted)
					{
						if (value == null)
						{
							link.Delete();
						}
						else if (value != link.Kind)
						{
							// Trigger a role player change instead of an add/remove
							link.Kind = value;
						}
						return;
					}
				}
				if (value != null)
				{
					new ReferenceModeHasReferenceModeKind(this, value);
				}
			}
		}
		/// <summary/>
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
		#endregion
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit InternalFactConstraint elements.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new AddIntrinsicsFixupListener((int)ORMDeserializationFixupPhase.AddIntrinsicElements);
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds intrinsic reference
		/// mode implementations
		/// </summary>
		private sealed class AddIntrinsicsFixupListener : IDeserializationFixupListener
		{
			private readonly int myPhase;
			/// <summary>
			/// AddIntrinsicsFixupListener constructor
			/// </summary>
			/// <param name="intrinsicPhase">A phase constant to watch</param>
			public AddIntrinsicsFixupListener(int intrinsicPhase)
			{
				myPhase = intrinsicPhase;
			}
			#region IDeserializationFixupListener Implementation
			bool IDeserializationFixupListener.HasElements(int phase, Store store)
			{
				return false;
			}
			void IDeserializationFixupListener.ProcessElements(int phase, Store store, INotifyElementAdded notifyAdded)
			{
				Debug.Assert(false); // Shouldn't be called if HasElements returns false
			}
			void IDeserializationFixupListener.PhaseCompleted(int phase, Store store)
			{
				if (phase == myPhase)
				{
					foreach (ORMModel model in store.ElementDirectory.FindElements<ORMModel>())
					{
						// Ensure that each model has a reference mode kind for each
						// reference mode type
						ReferenceModeKind generalKind = null;
						ReferenceModeKind popularKind = null;
						ReferenceModeKind unitBasedKind = null;
						foreach (ReferenceModeKind kind in ModelHasReferenceModeKind.GetReferenceModeKindCollection(model))
						{
							// UNDONE: Decide what to do with multiple kinds. Recoverable IF the
							// format strings are the same, otherwise this is a bogus model.
							switch (kind.ReferenceModeType)
							{
								case ReferenceModeType.General:
									generalKind = kind;
									break;
								case ReferenceModeType.Popular:
									popularKind = kind;
									break;
								case ReferenceModeType.UnitBased:
									unitBasedKind = kind;
									break;
							}
						}
						if (generalKind == null)
						{
							generalKind = new ReferenceModeKind(store);
							generalKind.ReferenceModeType = ReferenceModeType.General;
							generalKind.Model = model;
							generalKind.FormatString = "{1}";
						}
						if (popularKind == null)
						{
							popularKind = new ReferenceModeKind(store);
							popularKind.ReferenceModeType = ReferenceModeType.Popular;
							popularKind.Model = model;
							popularKind.FormatString = "{0}_{1}";
						}
						if (unitBasedKind == null)
						{
							unitBasedKind = new ReferenceModeKind(store);
							unitBasedKind.ReferenceModeType = ReferenceModeType.UnitBased;
							unitBasedKind.Model = model;
							unitBasedKind.FormatString = "{1}Value";
						}

						// Now populate all intrinsic reference modes
						CreateIntrinsicReferenceMode(store, model, popularKind, "Id", PortableDataType.NumericAutoCounter);
						CreateIntrinsicReferenceMode(store, model, popularKind, "id", PortableDataType.NumericAutoCounter);
						CreateIntrinsicReferenceMode(store, model, popularKind, "ID", PortableDataType.NumericAutoCounter);
						CreateIntrinsicReferenceMode(store, model, popularKind, "Name", PortableDataType.TextVariableLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "name", PortableDataType.TextVariableLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "Code", PortableDataType.TextFixedLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "code", PortableDataType.TextFixedLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "Title", PortableDataType.TextVariableLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "title", PortableDataType.TextVariableLength);
						CreateIntrinsicReferenceMode(store, model, popularKind, "Nr", PortableDataType.NumericSignedInteger);
						CreateIntrinsicReferenceMode(store, model, popularKind, "nr", PortableDataType.NumericSignedInteger);
						CreateIntrinsicReferenceMode(store, model, popularKind, "#", PortableDataType.NumericSignedInteger);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "kg", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mm", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "cm", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "km", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mile", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "Celsius", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "Fahrenheit", PortableDataType.NumericDecimal);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "USD", PortableDataType.NumericMoney);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "AUD", PortableDataType.NumericMoney);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "XEU", PortableDataType.NumericMoney);
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "CE", PortableDataType.TemporalDate);
						// UNDONE: Strongly consider extending this list
					}
				}
			}
			private static void CreateIntrinsicReferenceMode(Store store, ORMModel model, ReferenceModeKind kind, string referenceModeName, PortableDataType dataType)
			{
				IntrinsicReferenceMode refMode = new IntrinsicReferenceMode(store);
				refMode.Name = referenceModeName;
				refMode.Kind = kind;
				refMode.Model = model;
				refMode.Type = dataType;
			}
			#endregion // IDeserializationFixupListener Implementation
			#region INotifyElementAdded Implementation
			void INotifyElementAdded.ElementAdded(ModelElement element)
			{
				// Nothing to do
			}
			void INotifyElementAdded.ElementAdded(ModelElement element, bool addLinks)
			{
				Debug.Assert(false); // Not used on the listeners
			}
			#endregion // INotifyElementAdded Implementation
		}
		#endregion // Deserialization Fixup
		#region Reference Mode Name Generation
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
		/// Given the Entity and the the value type created the ref mode name
		/// </summary>
		/// <param name="valueTypeName">The name of the valuetype attached
		/// to the preferred identifier role.</param>
		/// <param name="entityTypeName">The name of the entity type.</param>
		/// <param name="formatString">Use this format string when finding the name.</param>
		/// <param name="model">The model that owns the reference modes</param>
		/// <returns>A ReferenceMode instance, or null</returns>
		public static ReferenceMode FindReferenceModeFromEntityNameAndValueName(string valueTypeName, string entityTypeName, string formatString, ORMModel model)
		{
			ReferenceMode retVal = null;
			foreach (ReferenceMode mode in model.ReferenceModeCollection)
			{
				if (valueTypeName == mode.GenerateValueTypeName(entityTypeName, formatString))
				{
					retVal = mode;
					break;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Given the Entity and the the value type created the ref mode name
		/// </summary>
		/// <param name="valueTypeName">The name of the valuetype attached
		/// to the preferred identifier role.</param>
		/// <param name="entityTypeName">The name of the entity type.</param>
		/// <param name="formatString">Use this format string when finding the name.</param>
		/// <param name="referenceModeName">Use this name when finding the name.</param>
		/// <param name="oldReferenceModeName">Use this name when the name of the object
		/// has changed, but you need to locate other elements with the old name</param>
		/// <param name="model">The model that owns the reference modes</param>
		/// <returns>A ReferenceMode instance, or null</returns>
		public static ReferenceMode FindReferenceModeFromEntityNameAndValueName(string valueTypeName, string entityTypeName, string formatString, string referenceModeName, string oldReferenceModeName, ORMModel model)
		{
			ReferenceMode retVal = null;
			foreach (ReferenceMode mode in model.ReferenceModeCollection)
			{
				if (mode.Name == referenceModeName && valueTypeName == mode.GenerateValueTypeName(entityTypeName, formatString, oldReferenceModeName))
				{
					retVal = mode;
					break;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Looks at all Reference modes in the model and returns the one with the gien format string
		/// </summary>
		/// <param name="formatString"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ReferenceMode FindReferenceModesByFormatString(string formatString, ORMModel model)
		{
			foreach (ReferenceMode mode in model.ReferenceModeCollection)
			{
				if (mode.FormatString.Equals(formatString))
				{
					return mode;
				}
			}
			return null;
		}


		/// <summary>
		/// Finds all enity types using a given reference Mode
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="formatString"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IEnumerable<ObjectType> FindObjectUsingReferenceModes(ReferenceMode mode, string formatString, ORMModel model)
		{
			foreach (EntityTypeHasPreferredIdentifier link in model.Store.ElementDirectory.FindElements<EntityTypeHasPreferredIdentifier>())
			{
				ObjectType entity = link.PreferredIdentifierFor;
				if (model == entity.Model)
				{
					if (entity.GetReferenceMode(formatString) == mode)
					{
						yield return entity;
					}
				}
			}
		}

		/// <summary>
		/// Finds all enity types using a given reference Mode
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="formatString"></param>
		/// <param name="oldReferenceModeName"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IEnumerable<ObjectType> FindObjectUsingReferenceModes(ReferenceMode mode, string formatString, string oldReferenceModeName, ORMModel model)
		{
			foreach (EntityTypeHasPreferredIdentifier link in model.Store.ElementDirectory.FindElements<EntityTypeHasPreferredIdentifier>())
			{
				ObjectType entity = link.PreferredIdentifierFor;
				if (model == entity.Model)
				{
					if (entity.GetReferenceMode(formatString, mode.Name, oldReferenceModeName) == mode)
					{
						yield return entity;
					}
				}
			}
		}

		/// <summary>
		/// Given the Entity and the ReferenceMode name return the ValueTypeCollection
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
		#endregion //Reference Mode Name Generation
		#region CustomStorage handlers
		private ReferenceModeKind GetKindDisplayValue()
		{
			return Kind;
		}
		private void SetKindDisplayValue(ReferenceModeKind newValue)
		{
			// Handled by ReferenceModeChangeRule
		}
		#endregion // CustomStorage handlers
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
		#region ReferenceModeChangeRule class
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(ReferenceMode))]
		protected class ReferenceModeChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward KindDisplay change value to Kind
			/// </summary>
			/// <param name="e">ElementPropertyChangedEventArgs</param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == KindDisplayDomainPropertyId)
				{
					(e.ModelElement as ReferenceMode).Kind = (ReferenceModeKind)e.NewValue;
				}
			}
		}
		#endregion // ReferenceModeChangeRule class
		#region ReferenceModeAddedRule class
		/// <summary>
		/// Make sure that every added reference mode has a valid
		/// reference mode kind. Default to general.
		/// </summary>
		[RuleOn(typeof(ModelHasReferenceMode), FireTime = TimeToFire.LocalCommit)]
		protected class ReferenceModeAddedRule : AddRule
		{
			/// <summary>
			/// Verify the Kind relationship is set on a newly
			/// added reference mode
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
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
		}
		/// <summary>
		/// Throws an exception if the format strings are not unique
		/// </summary>
		/// <param name="newMode"></param>
		/// <param name="model"></param>
		protected static void EnsureUnique(ReferenceMode newMode, ORMModel model)
		{
			if (model != null)
			{
				const string entityName = "\x1";
				string newFormatString = newMode.GenerateValueTypeName(entityName);
				foreach (ReferenceMode mode in model.ReferenceModeCollection)
				{
					string formatString = mode.GenerateValueTypeName(entityName);
					if (newMode != mode && newFormatString == formatString)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeEnforceUniqueFormatString);
					}
				}
			}
		}
		#endregion // ReferenceModeAddedRule class
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

		#region CustomReferenceModeChangeRule class
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(CustomReferenceMode))]
		protected class CustomReferenceModeChangeRule : ChangeRule
		{
			/// <summary>
			/// Update value types when format string changes
			/// </summary>
			/// <param name="e">ElementPropertyChangedEventArgs</param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				CustomReferenceMode mode = e.ModelElement as CustomReferenceMode;
				ORMModel model = mode.Model;
				EnsureUnique(mode,model);

				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == CustomFormatStringDomainPropertyId)
				{
					string customFormatString = (string)e.NewValue;
					if (customFormatString.Length > 0)
					{
						string oldFormatString = (string)e.OldValue;
						if (oldFormatString == null || oldFormatString.Length == 0)
						{
							oldFormatString = mode.Kind.FormatString;
						}

						IEnumerable<ObjectType> objects = FindObjectUsingReferenceModes(mode, oldFormatString, model);
						foreach (ObjectType entity in objects)
						{
							string newName = mode.GenerateValueTypeName(entity.Name);
							entity.RenameReferenceMode(newName);
						}

						if (customFormatString.Equals(mode.Kind.FormatString))
						{
							mode.CustomFormatString = "";
						}
					}
				}
				else if (attributeId == NameDomainPropertyId)
				{
					string newRefModeName = (string)e.NewValue;
					if (newRefModeName.Length > 0)
					{
						string oldRefModeName = (string)e.OldValue;

						IEnumerable<ObjectType> objects = FindObjectUsingReferenceModes(mode, mode.FormatString, oldRefModeName, model);
						foreach (ObjectType entity in objects)
						{
							string newName = mode.GenerateValueTypeName(entity.Name, mode.FormatString);
							entity.RenameReferenceMode(newName);
						}
					}
				}
			}
		}
		#endregion // CustomReferenceModeChangeRule class

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
	public partial class ReferenceModeKind
	{
		#region Properties
		/// <summary>
		/// Overriding ToString to return the Reference Mode Type name
		/// </summary>
		/// <returns>Reference Mode Type name as the string</returns>
		public override string ToString()
		{
			return this.ReferenceModeType.ToString();
		}
		#endregion //properties
		#region CustomReferenceModeChangeRule class
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(ReferenceModeKind))]
		protected class ReferenceModeKindChangeRule : ChangeRule
		{
			/// <summary>
			/// Update value types when format string changes
			/// </summary>
			/// <param name="e">ElementPropertyChangedEventArgs</param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == FormatStringDomainPropertyId)
				{
					string oldFormatString = (string)e.OldValue;
					ReferenceModeKind kind = e.ModelElement as ReferenceModeKind;
					ORMModel model = kind.Model;
					EnsureUnique(kind,model);

					foreach (ReferenceMode mode in kind.ReferenceModeCollection)
					{
						IEnumerable<ObjectType> objects = ReferenceMode.FindObjectUsingReferenceModes(mode, oldFormatString, mode.Name, model);
						foreach (ObjectType entity in objects)
						{
							string newName = mode.GenerateValueTypeName(entity.Name);
							entity.RenameReferenceMode(newName);
						}

						CustomReferenceMode customReferenceMode = mode as CustomReferenceMode;

						if (customReferenceMode != null && customReferenceMode.CustomFormatString.Equals(customReferenceMode.Kind.FormatString))
						{
							customReferenceMode.CustomFormatString = "";
						}
					}
				}
			}

			/// <summary>
			/// Throws an exception if the format strings are not unique
			/// </summary>
			/// <param name="newKind"></param>
			/// <param name="model"></param>
			private static void EnsureUnique(ReferenceModeKind newKind, ORMModel model)
			{
				string newFormatString = newKind.FormatString;
				foreach (ReferenceModeKind kind in model.ReferenceModeKindCollection)
				{
					string formatString = kind.FormatString;
					if (newKind != kind && newFormatString == formatString)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeKindEnforceUniqueFormatString); 
					}
				}
			}
		}
		#endregion // ReferenceModeKindChangeRule class
	}
	#endregion //ReferenceModeKind class
	#region ReferenceModeHasReferenceModeKind
	public partial class ReferenceModeHasReferenceModeKind
	{
		#region ReferenceModeHasReferenceModeKindChangeRule class
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(ReferenceModeHasReferenceModeKind))]
		protected class ReferenceModeHasReferenceModeKindChangeRule : RolePlayerChangeRule
		{
			/// <summary>
			/// Update value types when reference mode kind changes
			/// </summary>
			/// <param name="e"></param>
			public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
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
						ORMModel model = customReferenceMode.Model;

						string oldFormatString = customReferenceMode.CustomFormatString;
						if (oldFormatString == null || oldFormatString.Length == 0)
						{
							oldFormatString = oldKind.FormatString;
						}
						customReferenceMode.CustomFormatString = "";

						IEnumerable<ObjectType> objects = ReferenceMode.FindObjectUsingReferenceModes(customReferenceMode, oldFormatString, customReferenceMode.Name, model);
						foreach (ObjectType entity in objects)
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
		}
		#endregion // ReferenceModeHasReferenceModeKindChangeRule class
		#region ReferenceModeHasReferenceModeKindDeletingRule class
		// UNDONE: If we make this a Remove instead of a Removing, then
		// mode.IsDeleted is false even if a mode.Remove triggered this.
		// This looks alot like an IMS bug.
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(ReferenceModeHasReferenceModeKind))]
		protected class ReferenceModeHasReferenceModeKindDeletingRule : DeletingRule
		{
			/// <summary>
			/// Disallow removal of kind
			/// </summary>
			/// <param name="e">ElementPropertyChangedEventArgs</param>
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ReferenceModeHasReferenceModeKind link = e.ModelElement as ReferenceModeHasReferenceModeKind;

				ReferenceMode mode = link.ReferenceMode as ReferenceMode;
				if (mode != null && !mode.IsDeleting)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeReferenceModesKindNotEmpty);
				}
			}
		}
		#endregion // ReferenceModeHasReferenceModeKindDeletingRule class
	}
	#endregion
}
