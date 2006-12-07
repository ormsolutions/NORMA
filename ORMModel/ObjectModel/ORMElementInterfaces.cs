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
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IORMExtendableElement
	/// <summary>
	/// An ORM <see cref="ModelElement"/> that can be extended.
	/// </summary>
	/// <remarks>
	/// In order to support <see cref="IORMPropertyExtension"/>s and <see cref="ORMPropertyProvisioning"/>s,
	/// implementions must ensure that their <see cref="ICustomTypeDescriptor.GetProperties(Attribute[])"/>
	/// method calls <see cref="ExtendableElementUtility.GetExtensionProperties"/>.
	/// </remarks>
	public interface IORMExtendableElement
	{
		/// <summary>
		/// See <see cref="ModelElement.Store"/>.
		/// </summary>
		Store Store { get;}

		/// <summary>
		/// The <see cref="IORMToolServices"/> for this <see cref="IORMExtendableElement"/>.
		/// </summary>
		IORMToolServices ORMToolServices { get;}

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
		Guid ExtensionExpandableTopLevelPropertyId { get; }
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
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called before the document is loaded and when <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action);
		/// <summary>
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called after the document is loaded and when <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action);
		/// <summary>
		/// Manages modeling <see cref="EventHandler{TEventArgs}"/>s for the primary <see cref="Store"/>.
		/// Called when survey questions (used to load the model browser) are required and when
		/// <see cref="EventHandler{TEventArgs}"/>s are removed.
		/// </summary>
		/// <param name="eventManager">
		/// The <see cref="ModelingEventManager"/> used to add or remove <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		/// <param name="action">
		/// The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.
		/// </param>
		void ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, EventHandlerAction action);
	}
	#endregion // IORMModelEventSubscriber
	#region IDomainModelEnablesRulesAfterDeserialization interface
	/// <summary>
	/// Interface implemented on a DomainModel to enable
	/// initially disabled rules after deserialization.
	/// </summary>
	public interface IDomainModelEnablesRulesAfterDeserialization
	{
		/// <summary>
		/// Called after successful deserialization to enable
		/// rules that were initially disabled.
		/// </summary>
		/// <param name="ruleManager">Rule manager from the owning store.</param>
		void EnableRulesAfterDeserialization(RuleManager ruleManager);
	}
	#endregion // IDomainModelEnablesRulesAfterDeserialization interface
}
