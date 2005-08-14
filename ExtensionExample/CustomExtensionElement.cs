using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace ExtensionExample
{
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
	public enum TestEnumeration
	{
		None = 0,
		One = 1,
		Two = 2,
		Three = 3
	}
	public partial class MyCustomExtensionElement : IORMPropertyExtension
	{
		private static readonly Random random = new Random();
		private Guid myExtensionExpandableTopLevelAttributeGuid;

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

	[RuleOn(typeof(Role))]
	[RuleOn(typeof(FactType))]
	[RuleOn(typeof(RootType))]
	public sealed class ExtensionAddRule : AddRule
	{
		/// <summary>
		/// Whenever we are notified that an element is added to the model, we check if it is an
		/// IORMExtendableElement, and if so, we add ourselves to it.
		/// </summary>
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			IORMExtendableElement extendableElement = e.ModelElement as IORMExtendableElement;
			if (extendableElement != null)
			{
				MyCustomExtensionElement customElement = MyCustomExtensionElement.CreateMyCustomExtensionElement(e.ModelElement.Store);
				ExtensionElementUtility.AddExtensionElement(extendableElement, customElement);
			}
		}
	}
}