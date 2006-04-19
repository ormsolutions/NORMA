using System;
using System.Collections.Generic;
using Neumont.Tools.ORM.Framework;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using System.Collections;

namespace ExtensionExample
{
	public partial class ExtensionDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new MyCustomExtensionElementFixupListener();
				yield return FactTypeRequiresMeaningfulNameError.FactTypeNameErrorFixupListener;
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
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
		private class MyCustomExtensionElementFixupListener : DeserializationFixupListener<Role>
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
			protected override void ProcessElement(Role element, Store store, INotifyElementAdded notifyAdded)
			{
				IORMExtendableElement extendableElement = element as IORMExtendableElement;
				ModelElementMoveableCollection extensions = extendableElement.ExtensionCollection;
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
					MyCustomExtensionElement customElement = MyCustomExtensionElement.CreateMyCustomExtensionElement(store);
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
