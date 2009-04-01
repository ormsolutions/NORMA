using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.ExtensionExample
{
	/// <summary>
	/// A custom fixup phase for the extension example
	/// </summary>
	public enum ExtensionExampleFixupPhase
	{
		/// <summary>
		/// Validate meaningful names after the ORM model has completed its name validation.
		/// This enables any names that require regeneration to be automatically generated
		/// before we determine if the names are meaningful.
		/// </summary>
		ValidateMeaningfulNames = ORMDeserializationFixupPhase.ValidateElementNames + 1,
	}
	public partial class ExtensionDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new MyCustomExtensionElementFixupListener();
				yield return ObjectTypeRequiresMeaningfulNameError.ObjectTypeNameErrorFixupListener;
				// The errors in this model will be ignore by the ORM error validator because they are
				// in the wrong domain model. Register our own handler to deal with these errors.
				// Note that this could also be handled by explicitly add/removing validating errors
				// from the task provider when we create them, but this general facility is easier to
				// use because it does this for us.
				yield return ModelError.GetFixupListener((int)ORMDeserializationFixupPhase.ValidateErrors, DomainModelInfo);
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
			}
		}
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupPhaseType"/>
		/// The shape model uses the same fixup phases as the core domain model, so this returns null.
		/// </summary>
		protected static Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ExtensionExampleFixupPhase);
			}
		}
		Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return DeserializationFixupPhaseType;
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
		#region Deserialization Fixup Classes
		/// <summary>
		/// Fixup listener implementation. Adds implicit MyCustomExtensionElement objects to roles
		/// that don't have them when the file is deserialized. This allows extension elements to
		/// be added to existing files, as well as extensions with default values (which don't serialize
		/// because of the settings in the ExtensionDomainModel.SerializationExtensions.xml file) to
		/// be readded when the file loads.
		/// </summary>
		private sealed class MyCustomExtensionElementFixupListener : DeserializationFixupListener<Role>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public MyCustomExtensionElementFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Process elements for each FactTypeHasRole definition added
			/// during deserialization. If one of our extension elements is not
			/// included in the set of extension elements then create a new one
			/// and add it.
			/// </summary>
			/// <param name="element">A Role element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(Role element, Store store, INotifyElementAdded notifyAdded)
			{
				IORMExtendableElement extendableElement = element as IORMExtendableElement;
				LinkedElementCollection<ModelElement> extensions = extendableElement.ExtensionCollection;
				int extensionCount = extensions.Count;
				int i;
				for (i = 0; i < extensionCount; ++i)
				{
					// Look for any of our extension elements
					if (extensions[i] is MyCustomExtensionElement)
					{
						break;
					}
				}
				if (i == extensionCount)
				{
					MyCustomExtensionElement customElement = new MyCustomExtensionElement(store);
					ExtensionElementUtility.AddExtensionElement(extendableElement, customElement);
					// Always notify during deserialization. All rules are turned off, so
					// any additions need to be explicitly notified so that other deserialization
					// fixup listeners can respond appropriately.
					notifyAdded.ElementAdded(customElement, true);
				}
			}
		}
		#endregion // Deserialization Fixup Classes
	}
}
