using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;

namespace Neumont.Tools.ORM.ObjectModel
{
	/// <summary>
	/// An ORM element that can be extended.
	/// </summary>
	public interface IORMExtendableElement
	{
		/// <summary>
		/// The collection of IORMExtensionElements
		/// </summary>
		ModelElementMoveableCollection ExtensionCollection { get;}
	}
	/// <summary>
	/// An IORMExtension element that provides custom properties
	/// </summary>
	public interface IORMPropertyExtension
	{
		/// <summary>
		/// Controls how custom properties are displayed
		/// </summary>
		ORMExtensionPropertySettings ExtensionPropertySettings { get;}
		/// <summary>
		/// If the extension is being shown as a direct property, then
		/// provide the meta attribute to display as the default value,
		/// or Guid.Empty to show a read-only value returned by ToString.
		/// </summary>
		Guid ExtensionDefaultAttribute { get;}
	}
	/// <summary>
	/// Controls how custom properties are displayed
	/// </summary>
	[Flags, CLSCompliant(true)]
	public enum ORMExtensionPropertySettings
	{
		/// <summary>
		/// Properties are not displayed
		/// </summary>
		NotDisplayed = 0,
		/// <summary>
		/// Displays as an expandable tree in the property grid
		/// </summary>
		MergeAsChildProperties = 1,
		/// <summary>
		/// Displays as a top level property in the property grid
		/// </summary>
		MergeAsDirectProperty = 2,
	}
	/// <summary>
	/// Provides static helper utility functions for IORMExtensionElement
	/// </summary>
	public static partial class ExtensionUtility
	{		
		/// <summary>
		/// Adds an IORMExtensionElement to an IORMExtendableElement
		/// </summary>
		public static void AddExtensionElement(ModelElement extensionElement, IORMExtendableElement extendedElement)
		{
			extendedElement.ExtensionCollection.Add(extensionElement);
		}
		/// <summary>
		/// Gets the IORMExtendableElement that extensionElement is attached to. 
		/// </summary>
		public static T GetExtendedElement<T>(ModelElement extensionElement) where T : ModelElement, IORMExtendableElement
		{
			return (T)(extensionElement.GetCounterpartRolePlayer(ORMNamedElementHasExtensionElement.ExtensionCollectionMetaRoleGuid, ORMNamedElementHasExtensionElement.ExtendedElementMetaRoleGuid, false)
			??
			extensionElement.GetCounterpartRolePlayer(ORMModelElementHasExtensionElement.ExtensionCollectionMetaRoleGuid, ORMModelElementHasExtensionElement.ExtendedElementMetaRoleGuid, false));
		}
	}
}
