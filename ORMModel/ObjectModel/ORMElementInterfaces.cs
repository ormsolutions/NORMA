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
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IORMExtendableElement
	/// <summary>
	/// An <see cref="ModelElement">ORM element</see> that can be extended.
	/// </summary>
	/// <remarks>
	/// In order to support <see cref="IORMPropertyExtension"/>s, implementions
	/// must ensure that their <see cref="ICustomTypeDescriptor.GetProperties(Attribute[])"/>
	/// method calls <see cref="ExtendableElementUtility.GetExtensionProperties"/>.
	/// </remarks>
	public interface IORMExtendableElement
	{
		/// <summary>
		/// The collection of extension <see cref="ModelElement"/>s.
		/// </summary>
		LinkedElementCollection<ModelElement> ExtensionCollection { get;}

		/// <summary>
		/// The collection of extension <see cref="ModelError"/>s.
		/// </summary>
		LinkedElementCollection<ModelError> ExtensionModelErrorCollection { get;}
	}
	#endregion

	#region IORMPropertyExtension
	/// <summary>
	/// An extension <see cref="ModelElement"/> that provides custom properties for the
	/// <see cref="System.Windows.Forms.PropertyGrid"/> of the <see cref="IORMExtendableElement"/>
	/// that it is extending. The extension properties will be obtained from the
	/// <see cref="ICustomTypeDescriptor"/> provided for the <see cref="IORMPropertyExtension"/>.
	/// </summary>
	public interface IORMPropertyExtension
	{
		/// <summary>
		/// Controls how custom properties are displayed.
		/// </summary>
		ORMExtensionPropertySettings ExtensionPropertySettings { get;}
		/// <summary>
		/// If the extension is being shown as an
		/// <see cref="ORMExtensionPropertySettings.MergeAsExpandableProperty">expandable property</see>,
		/// this determines the property to display as the value at the root of the expandable tree.
		/// </summary>
		/// <remarks>
		/// If <see cref="ORMExtensionPropertySettings.MergeAsExpandableProperty"/> is not set, the value of this
		/// property is not used.
		/// If <see cref="Guid.Empty"/> or a <see cref="Guid"/> for which a <see cref="DomainPropertyInfo"/>
		/// cannot be retrieved is specified, the value returned by <see cref="Object.ToString"/> is used.
		/// </remarks>
		Guid ExtensionExpandableTopLevelPropertyGuid { get; }
		/// <summary>
		/// Returns a <see cref="PropertyDescriptorCollection"/> containing the <see cref="PropertyDescriptor"/>s
		/// that should be merged with the <see cref="IORMExtendableElement"/>'s <see cref="PropertyDescriptor"/>s.
		/// </summary>
		PropertyDescriptorCollection GetProperties();
	}

	/// <summary>
	/// Controls how custom properties are displayed in the <see cref="System.Windows.Forms.PropertyGrid"/>.
	/// </summary>
	[Flags]
	public enum ORMExtensionPropertySettings
	{
		/// <summary>
		/// Properties are not displayed.
		/// </summary>
		NotDisplayed = 0,
		/// <summary>
		/// Properties are displayed as an expandable tree.
		/// </summary>
		MergeAsExpandableProperty = 1,
		/// <summary>
		/// Properties are displayed as top-level entries.
		/// </summary>
		MergeAsTopLevelProperty = 2,
	}
	#endregion

	#region IORMModelEventSubscriber
	/// <summary>
	/// This interface provides needed methods that are required to add Events to the Object Model.
	/// </summary>
	public interface IORMModelEventSubscriber
	{
		/// <summary>
		/// This method attaches ModelEvents to the primary Store. Before the Document is Loaded.
		/// </summary>
		void AddPreLoadModelingEventHandlers();
		/// <summary>
		/// This method attaches ModelEvents to the primary Store. After the Document is Loaded.
		/// </summary>
		void AddPostLoadModelingEventHandlers();
		/// <summary>
		/// This method removes ModelEvents from the primary Store.
		/// </summary>
		/// <param name="preLoadAdded">The AddPreLoadModelingEventHandlers was called</param>
		/// <param name="postLoadAdded">The AddPostLoadModelingEventHandlers was called</param>
		/// <param name="surveyHandlerAdded">The SurveyEventHandlers were loaded</param>
		void RemoveModelingEventHandlers(bool preLoadAdded, bool postLoadAdded, bool surveyHandlerAdded);
		/// <summary>
		/// this method attaches ModelEvents to the MetaModel where they can be passed to the affected store
		/// </summary>
		void SurveyQuestionLoad();
	}
	#endregion // IORMModelEventSubscriber
}
