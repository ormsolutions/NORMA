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
	public abstract partial class ReferenceMode
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
				string retVal = this.Kind.FormatString;
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
			return string.Format(CultureInfo.InvariantCulture, this.FormatString, entityName, this.Name);
		}
		#endregion // ReferenceMode specific
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
						CreateIntrinsicReferenceMode(store, model, unitBasedKind, "CE");;
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
		public static ReferenceMode FindReferenceModeFromEnitityNameAndValueName(string valueTypeName, string entityTypeName, ORMModel model)
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
		/// Given the Entity and the ReferenceMode name return the ValueTypeCollection
		/// </summary>
		/// <param name="referenceModeName">The name of the reference mode(s) to locate</param>
		/// <param name="model">The containing model</param>
		/// <returns>Always returns a collection, does not return null</returns>
		[CLSCompliant(false)]
		public static ICollection<ReferenceMode> FindReferenceModesByName(string referenceModeName, ORMModel model)
		{
			// Choice of implementation: We need to either make two passes to allocate the
			// right size array, always create a Collection, or delay create the collection
			// for the (relatively uncommon) case when we have multiple reference modes of the
			// same name. Implement the third option.
			Collection<ReferenceMode> multiples = null;
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
						multiples = new Collection<ReferenceMode>();
						multiples.Add(single);
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
	}
	#endregion // ReferenceMode class
	#region CustomReferenceMode class
	public partial class CustomReferenceMode
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
	}
	#endregion // CustomReferenceMode class
}
