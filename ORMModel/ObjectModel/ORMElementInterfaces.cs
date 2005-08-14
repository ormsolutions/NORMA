using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IORMExtendableElement
	/// <summary>
	/// An <see cref="ModelElement">ORM element</see> that can be extended.
	/// </summary>
	[CLSCompliant(true)]
	public interface IORMExtendableElement
	{
		/// <summary>
		/// The collection of extension <see cref="ModelElement"/>s.
		/// </summary>
		ModelElementMoveableCollection ExtensionCollection { get;}

		/// <summary>
		/// In order to support <see cref="IORMPropertyExtension"/>s, this method must call
		/// <see cref="ExtendableElementUtility.MergeExtensionProperties"/>. See example.
		/// </summary>
		/// <example>
		/// public override PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor)
		/// {
		///		return Neumont.Tools.ORM.ObjectModel.ExtendableElementUtility.MergeExtensionProperties(this, base.GetDisplayProperties(requestor, ref defaultPropertyDescriptor));
		/// }
		/// </example>
		PropertyDescriptorCollection GetDisplayProperties(ModelElement requestor, ref PropertyDescriptor defaultPropertyDescriptor);
	}
	#endregion

	#region IORMPropertyExtension
	/// <summary>
	/// An extension <see cref="ModelElement"/> that provides custom properties for the
	/// <see cref="System.Windows.Forms.PropertyGrid"/> of the <see cref="IORMExtendableElement"/>
	/// that it is extending.
	/// </summary>
	[CLSCompliant(true)]
	public interface IORMPropertyExtension
	{
		/// <summary>
		/// Controls how custom properties are displayed.
		/// </summary>
		ORMExtensionPropertySettings ExtensionPropertySettings { get;}
		/// <summary>
		/// If the extension is being shown as an
		/// <see cref="ORMExtensionPropertySettings.MergeAsExpandableProperty">expandable property</see>,
		/// this determines the meta attribute to display as the value at the root of the expandable tree.
		/// </summary>
		/// <remarks>
		/// If <see cref="ORMExtensionPropertySettings.MergeAsExpandableProperty"/> is not set, the value of this
		/// property is not used.
		/// If <see cref="Guid.Empty"/> or a <see cref="Guid"/> for which a <see cref="MetaAttributeInfo"/>
		/// cannot be retrieved is specified, the value returned by <see cref="Object.ToString"/> is used.
		/// </remarks>
		Guid ExtensionExpandableTopLevelAttributeGuid { get; }
		/// <summary>
		/// Returns a <see cref="PropertyDescriptorCollection"/> containing the <see cref="PropertyDescriptor"/>s
		/// that should be merged with the <see cref="IORMExtendableElement"/>'s <see cref="PropertyDescriptor"/>s.
		/// </summary>
		PropertyDescriptorCollection GetProperties();
	}

	/// <summary>
	/// Controls how custom properties are displayed in the <see cref="System.Windows.Forms.PropertyGrid"/>.
	/// </summary>
	[Flags, CLSCompliant(true)]
	public enum ORMExtensionPropertySettings
	{
		/// <summary>
		/// Properties are not displayed
		/// </summary>
		NotDisplayed = 0,
		/// <summary>
		/// Properties are displayed as an expandable tree
		/// </summary>
		MergeAsExpandableProperty = 1,
		/// <summary>
		/// Properties are displayed as top-level entries
		/// </summary>
		MergeAsTopLevelProperty = 2,
	}
	#endregion


}
