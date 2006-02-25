#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object Role Modeling Architect for Visual Studio                 *
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
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace ExtensionExample
{
	#region TestElementPicker Class
	/// <summary>
	/// Provides the content for a drop down list for <see cref="MyCustomExtensionElement.TestProperty"/>.
	/// </summary>
	/// <remarks>
	/// The user is allowed to specify a value that is not in the drop down list by typing it in.
	/// If the current value is not in the list of predefined values, it is added to the drop down list.
	/// </remarks>
	public class TestElementPicker : Neumont.Tools.ORM.ObjectModel.Editors.ElementPicker
	{
		private static readonly string[] predefinedValues =
			new string[] { "Default value", "Not the default value", "Another value" };

		protected override System.Collections.IList GetContentList(System.ComponentModel.ITypeDescriptorContext context, object value)
		{
			// UNDONE: If this UITypeEditor is being used for the top-level value of an expandable property,
			// the value parameter will be the value of the extension element rather than the property.
			// For now, we handle this just by checking if the value is a string or not, but we need a more general
			// solution that will (hopefully) be transparent to extensions.

			string valueString = value as string ?? (value as MyCustomExtensionElement).TestProperty;

			if (Array.IndexOf(predefinedValues, valueString) != -1)
			{
				return Array.AsReadOnly(predefinedValues);
			}
			else
			{
				List<string> valuesList = new List<string>(predefinedValues);
				valuesList.Add(valueString);
				return valuesList;
			}
		}
	}
	#endregion // TestElementPicker Class
	#region TestEnumeration Enum
	/// <summary>
	/// Provides test values for our enum sample dropdown
	/// </summary>
	public enum TestEnumeration
	{
		None = 0,
		One = 1,
		Two = 2,
		Three = 3
	}
	#endregion // TestEnumeration Enum
	#region MyCustomExtensionElement class
	public partial class MyCustomExtensionElement : IORMPropertyExtension
	{
		private static readonly Random random = new Random();
		private Guid myExtensionExpandableTopLevelAttributeGuid;

		/// <summary>
		/// Test only code to randomly pick how our extension properties are displayed.
		/// OnCreated generally does not need to be overriden. If you do override it, however,
		/// note that OnCreated is called whenever an element is created, whereas OnInitialized
		/// is not called during deserialization. Therefore, if you make any changes to the model
		/// in this routine you should make those changes in OnInitialized instead.
		/// </summary>
		public override void OnCreated()
		{
			int randomNumber = -1;
			lock (random)
			{
				randomNumber = random.Next(0, 4);
			}
			System.Diagnostics.Debug.Assert(randomNumber != -1);
			switch (randomNumber)
			{
				case 0:
					myExtensionExpandableTopLevelAttributeGuid = Guid.Empty;
					break;
				case 1:
					// MetaClassGuid is not a valid Guid for ExtensionExpandableTopLevelAttributeGuid
					// since a MetaAttributeInfo cannot be retrieved for it. We are intentionally
					// including it as a possible return value in order to test the handling
					// of invalid Guids. The result in this case should be the same as if we specified
					// Guid.Empty.
					myExtensionExpandableTopLevelAttributeGuid = MyCustomExtensionElement.MetaClassGuid;
					break;
				case 2:
					myExtensionExpandableTopLevelAttributeGuid = MyCustomExtensionElement.TestPropertyMetaAttributeGuid;
					break;
				case 3:
					myExtensionExpandableTopLevelAttributeGuid = MyCustomExtensionElement.CustomEnumMetaAttributeGuid;
					break;
			}
		}

		#region IORMPropertyExtension Members

		/// <summary>
		/// Implements <see cref="IORMPropertyExtension.ExtensionPropertySettings"/>/
		/// </summary>
		/// <value>
		/// Show this extension's properties both as a single expandable property and as individual
		/// top-level properties.
		/// </value>
		public ORMExtensionPropertySettings ExtensionPropertySettings
		{
			get
			{
				return ORMExtensionPropertySettings.MergeAsExpandableProperty | ORMExtensionPropertySettings.MergeAsTopLevelProperty;
			}
		}

		/// <summary>
		/// Implements <see cref="IORMPropertyExtension.ExtensionExpandableTopLevelAttributeGuid"/>.
		/// </summary>
		/// <value>
		/// When this extension element is created, we randomly pick which property to show.
		/// </value>
		public Guid ExtensionExpandableTopLevelAttributeGuid
		{
			get
			{
				return myExtensionExpandableTopLevelAttributeGuid;
			}
		}

		#endregion

		/// <summary>
		/// This is used as the top-level value when we're shown as an expandable property if we specify
		/// <see cref="Guid.Empty"/> or an invalid <see cref="Guid"/> for
		/// <see cref="IORMPropertyExtension.ExtensionExpandableTopLevelAttributeGuid"/>.
		/// </summary>
		public override string ToString()
		{
			return this.GetType().Name;
		}
	}
	#endregion // MyCustomExtensionElement class
	#region ExtensionAddRule class
	/// <summary>
	/// Rule classes are defined to respond to changes in the object
	/// model that occur during user editing. In this case, we're adding
	/// our custom extension element to a Role object when it is added to
	/// a FactType via the FactTypeHasRole relationship.
	/// </summary>
	[RuleOn(typeof(FactTypeHasRole))]
	public sealed class ExtensionAddRule : AddRule
	{
		/// <summary>
		/// Add our custom extension properties to a role when it is added to a fact type
		/// </summary>
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			IORMExtendableElement extendableElement = link.RoleCollection as IORMExtendableElement;
			if (extendableElement != null)
			{
				MyCustomExtensionElement customElement = MyCustomExtensionElement.CreateMyCustomExtensionElement(e.ModelElement.Store);
				ExtensionElementUtility.AddExtensionElement(extendableElement, customElement);
			}
		}
	}
	#endregion // ExtensionAddRule class
}