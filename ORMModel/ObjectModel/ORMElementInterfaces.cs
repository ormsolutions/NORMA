#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region IORMExtendableElement
	/// <summary>
	/// An ORM <see cref="ModelElement"/> that can be extended.
	/// </summary>
	/// <remarks>
	/// In order to support <see cref="IORMPropertyExtension"/>s and <see cref="PropertyProvider"/>s,
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
}
