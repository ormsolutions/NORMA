#region Using directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	#region ReferenceModeType enum
	/// <summary>
	/// Standard reference mode categories
	/// </summary>
	[CLSCompliant(true)]
	public enum ReferenceModeType
	{
		/// <summary>
		/// Used for custom refmode formats
		/// </summary>
		General,
		/// <summary>
		/// Standard popular reference mode
		/// </summary>
		Popular,
		/// <summary>
		/// Standard unit-based reference mode
		/// </summary>
		UnitBased,
	}
	#endregion // ReferenceModeType enum
	#region ReferenceMode class
	public abstract partial class ReferenceMode : IComparable<ReferenceMode>
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
		/// 
		/// </summary>
		public Northface.Tools.ORM.ObjectModel.ReferenceModeKind Kind
		{
			get
			{
				System.Object o = null;
				Microsoft.VisualStudio.Modeling.ElementLink goodLink = null;
				System.Collections.IList links = this.GetElementLinks(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid);
				foreach (Microsoft.VisualStudio.Modeling.ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						goodLink = link;
						break;
					}
				}
				if (goodLink != null)
				{
					o = goodLink.GetRolePlayer(Northface.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.KindMetaRoleGuid);
				}
				return (Northface.Tools.ORM.ObjectModel.ReferenceModeKind)o;
			}
			set
			{
				IList links = this.GetElementLinks(ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid);
				foreach (ElementLink link in links)
				{
					if (!link.IsRemoved)
					{
						if (value == null)
						{
							link.Remove();
						}
						else if (value != link.GetRolePlayer(ReferenceModeHasReferenceModeKind.KindMetaRoleGuid))
						{
							// Trigger a role player change instead of an add/remove
							link.SetRolePlayer(ReferenceModeHasReferenceModeKind.KindMetaRoleGuid, value);
						}
						return;
					}
				}
				if (value != null)
				{
					RoleAssignment[] newRoles = new RoleAssignment[2];
					newRoles[0] = new RoleAssignment(ReferenceModeHasReferenceModeKind.KindMetaRoleGuid, value);
					newRoles[1] = new RoleAssignment(ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid, this);
					this.Store.ElementFactory.CreateElementLink(typeof(ReferenceModeHasReferenceModeKind), newRoles);
				}
			}
		}
		#endregion
		#region Customize property display
		/// <summary>
		/// Return a custom property descriptor for the Kind Display property
		/// </summary>
		/// <param name="modelElement"></param>
		/// <param name="metaAttributeInfo"></param>
		/// <param name="requestor"></param>
		/// <param name="attributes"></param>
		/// <returns></returns>
		protected override ElementPropertyDescriptor CreatePropertyDescriptor(ModelElement modelElement, MetaAttributeInfo metaAttributeInfo, ModelElement requestor, Attribute[] attributes)
		{
			if (metaAttributeInfo.Id == KindDisplayMetaAttributeGuid)
			{
				return new CustomSetStringPropertyDescriptor(ResourceStrings.ModelReferenceModeEditorChangeReferenceModeKindTransaction, modelElement, metaAttributeInfo, requestor, attributes);
			}
			return base.CreatePropertyDescriptor(modelElement, metaAttributeInfo, requestor, attributes);
		}
		#endregion // Customize property display
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit InternalFactConstraint elements.
		/// </summary>
		[CLSCompliant(false)]
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
					foreach (ORMModel model in store.ElementDirectory.GetElements(ORMModel.MetaClassGuid))
					{
						// Ensure that each model has a reference mode kind for each
						// reference mode type
						ReferenceModeKind generalKind = null;
						ReferenceModeKind popularKind = null;
						ReferenceModeKind unitBasedKind = null;
						foreach (ReferenceModeKind kind in model.GetCounterpartRolePlayers(ModelHasReferenceModeKind.ModelMetaRoleGuid, ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid))
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
							generalKind = ReferenceModeKind.CreateReferenceModeKind(store);
							generalKind.ReferenceModeType = ReferenceModeType.General;
							generalKind.Model = model;
							generalKind.FormatString = "{1}";
						}
						if (popularKind == null)
						{
							popularKind = ReferenceModeKind.CreateReferenceModeKind(store);
							popularKind.ReferenceModeType = ReferenceModeType.Popular;
							popularKind.Model = model;
							popularKind.FormatString = "{0}_{1}";
						}
						if (unitBasedKind == null)
						{
							unitBasedKind = ReferenceModeKind.CreateReferenceModeKind(store);
							unitBasedKind.ReferenceModeType = ReferenceModeType.UnitBased;
							unitBasedKind.Model = model;
							unitBasedKind.FormatString = "{1}Value";
						}

						// Now populate all intrinsic reference modes
						CreateIntrinsicReferenceMode(store, model, popularKind, "id");
						CreateIntrinsicReferenceMode(store, model, popularKind, "name");
						CreateIntrinsicReferenceMode(store, model, popularKind, "code");
						CreateIntrinsicReferenceMode(store, model, popularKind, "title");
						CreateIntrinsicReferenceMode(store, model, popularKind, "nr");
						CreateIntrinsicReferenceMode(store, model, popularKind, "#");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mm");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "cm");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "kg");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "mile");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "Celsius");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "USD");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "AUD");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "EUR");
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "CE");
						// UNDONE: Strongly consider extending this list
					}
				}
			}
			private static void CreateIntrinsicReferenceMode(Store store, ORMModel model, ReferenceModeKind kind, string referenceModeName)
			{
				IntrinsicReferenceMode refMode = IntrinsicReferenceMode.CreateIntrinsicReferenceMode(store);
				refMode.Name = referenceModeName;
				refMode.Kind = kind;
				refMode.Model = model;
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
		[CLSCompliant(false)]
		public static IEnumerable<ObjectType> FindObjectUsingReferenceModes(ReferenceMode mode, string formatString, ORMModel model)
		{
			IList links = model.Store.ElementDirectory.GetElements(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			foreach (EntityTypeHasPreferredIdentifier link in links)
			{
				ObjectType entity = link.PreferredIdentifierFor;
				if (object.ReferenceEquals(model, entity.Model))
				{
					if (object.ReferenceEquals(entity.GetReferenceMode(formatString), mode))
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
		[CLSCompliant(false)]
		public static IEnumerable<ObjectType> FindObjectUsingReferenceModes(ReferenceMode mode, string formatString, string oldReferenceModeName, ORMModel model)
		{
			IList links = model.Store.ElementDirectory.GetElements(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			foreach (EntityTypeHasPreferredIdentifier link in links)
			{
				ObjectType entity = link.PreferredIdentifierFor;
				if (object.ReferenceEquals(model, entity.Model))
				{
					if (object.ReferenceEquals(entity.GetReferenceMode(formatString, mode.Name, oldReferenceModeName), mode))
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
		[CLSCompliant(false)]
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
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in ObjectTypeChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == KindDisplayMetaAttributeGuid)
			{
				// Handled by ReferenceModeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == KindDisplayMetaAttributeGuid)
			{
				return Kind;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}


		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
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

		/// <summary>
		/// Reteurns true if the two elements are the same element
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(ReferenceMode other)
		{
			return this == other;
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
			/// <param name="e">ElementAttributeChangedEventArgs</param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == KindDisplayMetaAttributeGuid)
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
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasReferenceMode link = e.ModelElement as ModelHasReferenceMode;
				ReferenceMode mode = link.ReferenceModeCollection;
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
			/// <param name="e">ElementAttributeChangedEventArgs</param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				CustomReferenceMode mode = e.ModelElement as CustomReferenceMode;
				ORMModel model = mode.Model;
				EnsureUnique(mode,model);

				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == CustomFormatStringMetaAttributeGuid)
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
				else if (attributeId == NameMetaAttributeGuid)
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

		bool IComparable<CustomReferenceMode>.Equals(CustomReferenceMode other)
		{
			return base.Equals(other);
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

		/// <summary>
		/// Typed Equals method. Implements IComparable&lt;IntrinsicReferenceMode&gt;.Equals.
		/// </summary>
		public bool Equals(IntrinsicReferenceMode other)
		{
			return base.Equals(other);
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
			/// <param name="e">ElementAttributeChangedEventArgs</param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == FormatStringMetaAttributeGuid)
				{
					string newFormatString = (string)e.NewValue;
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
			private void EnsureUnique(ReferenceModeKind newKind,ORMModel model)
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
				Guid roleId = e.MetaRole.Id;
				if (roleId == ReferenceModeHasReferenceModeKind.KindMetaRoleGuid)
				{
					ReferenceModeHasReferenceModeKind link = e.ElementLink as ReferenceModeHasReferenceModeKind;
					ReferenceModeKind newKind = (ReferenceModeKind)e.NewRolePlayer;
					Debug.Assert(newKind == link.Kind);
					ReferenceModeKind oldKind = (ReferenceModeKind)e.OldRolePlayer;

					ReferenceMode mode = link.ReferenceModeCollection;
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
		#region ReferenceModeHasReferenceModeKindRemovingRule class
		// UNDONE: If we make this a Remove instead of a Removing, then
		// mode.IsRemoved is false even if a mode.Remove triggered this.
		// This looks alot like an IMS bug.
		/// <summary>
		/// Rule to forward the KindDisplay property to the generated
		/// Kind property
		/// </summary>
		[RuleOn(typeof(ReferenceModeHasReferenceModeKind))]
		protected class ReferenceModeHasReferenceModeKindRemovingRule : RemovingRule
		{
			/// <summary>
			/// Disallow removal of kind
			/// </summary>
			/// <param name="e">ElementAttributeChangedEventArgs</param>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ReferenceModeHasReferenceModeKind link = e.ModelElement as ReferenceModeHasReferenceModeKind;

				ReferenceMode mode = link.ReferenceModeCollection as ReferenceMode;
				if (mode != null && !mode.IsRemoving)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionReferenceModeReferenceModesKindNotEmpty);
				}
			}
		}
		#endregion // ReferenceModeHasReferenceModeKindRemovingRule class
	}
	#endregion
}
