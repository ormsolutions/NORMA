using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Framework;
using System.Globalization;

namespace ExtensionExample
{
	/// <summary>
	/// This is a ModelError added to the ORM Tool using extensions.
	/// This error informs the user of the ORM Tool that A FactType must have a meaningful name.
	/// i.e. The name must be something other than "FactType1" (or other variations).
	/// </summary>
	public partial class FactTypeRequiresMeaningfulNameError : ModelError
	{
		#region Member Variables
		private static FactType myFactType = null;
		#endregion // Member Variables
		#region Validation Methods
		/// <summary>
		/// This method performs Validation of a FactType name.
		/// </summary>
		/// <param name="element">The ModelElement you are trying to validate.</param>
		private static void DelayValidateFactTypeHasMeaningfulNameError(ModelElement element)
		{
			FactType factType = element as FactType;
			ValidateFactTypeName(factType, null);
		}
		/// <summary>
		/// This method Validates a FactType name.
		/// If the FactType name has "FactType" of "FactType"
		/// followed by any digits it will create a new error.
		/// </summary>
		/// <param name="factType">The FactType you wish to be validated.</param>
		private static void ValidateFactTypeName(FactType factType, INotifyElementAdded notifyAdded)
		{
			if (!factType.IsRemoved)
			{
				myFactType = factType;
				Regex factTypeRegex = new Regex(@"FactType[\d+]{0,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				Match regexMatch = factTypeRegex.Match(factType.Name);

				ModelErrorMoveableCollection extensions = factType.ExtensionModelErrorCollection;
				FactTypeRequiresMeaningfulNameError nameError = null;
				int extensionCount = extensions.Count;
				int i;
				for (i = 0; i < extensionCount; ++i)
				{
					if (extensions[i] is FactTypeRequiresMeaningfulNameError)
					{
						nameError = extensions[i] as FactTypeRequiresMeaningfulNameError;
						break;
					}
				}

				if (regexMatch.Success)
				{
					if (nameError == null)
					{
						nameError = FactTypeRequiresMeaningfulNameError.CreateFactTypeRequiresMeaningfulNameError(factType.Store);
						nameError.Model = factType.Model;
						nameError.GenerateErrorText();
						ExtensionElementUtility.AddExtensionModelError(factType, nameError);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(nameError, true);
						}
					}
					else
					{
						nameError.GenerateErrorText();
					}
				}
				else
				{
					if (nameError != null)
					{
						nameError.Remove();
					}
				}
				if (myFactType != null)
				{
					myFactType = null;
				}
			}
		}
		#endregion // Validation Methods
		#region ModelError Overrides
		/// <summary>
		/// This method recreated the Error Text that will be displayed in the Error List.
		/// </summary>
		public override void GenerateErrorText()
		{
			if (myFactType != null)
			{
				string newText = string.Format(CultureInfo.CurrentCulture,
					"Fact '{0}' in model '{1}' needs a meaningful name",
					myFactType.Name,
					myFactType.Model.Name);
				if (!Name.Equals(newText))
				{
					Name = newText;
				}
			}
		}
		/// <summary>
		/// This methods Return when the Events should be Regenerated.
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion // ModelError Overrides
		#region ExtensionErrorRules
		/// <summary>
		/// This Rule calls the DelayValidateElement method when a FactType is added to the Diagram.
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))]
		public class ExtensionFactTypeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as ModelHasFactType).FactTypeCollection, DelayValidateFactTypeHasMeaningfulNameError);
			}
		}
		/// <summary>
		/// This method calls the DelayValidateElement method when a FactType has been changed.
		/// </summary>
		[RuleOn(typeof(FactType))]
		public class ExtensionFactTypeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as FactType), DelayValidateFactTypeHasMeaningfulNameError);
			}
		}
		#endregion // ExtensionErrorRules
		#region CustomExtensionError Deserialization Fixup Classes
		/// <summary>
		/// A fixup listener for adding/removing the FactTypeRequiresMeaningfulError class
		/// </summary>
		public static IDeserializationFixupListener FactTypeNameErrorFixupListener
		{
			get
			{
				return new FactTypeNameFixupListener();
			}
		}
		/// <summary>
		/// Private class to Validate a FactType when the .orm file is Deserialization.
		/// </summary>
		private class FactTypeNameFixupListener : DeserializationFixupListener<FactType>
		{
			public FactTypeNameFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			protected override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				ValidateFactTypeName(element, notifyAdded);
			}
		}
		#endregion // CustomExtensionError Deserialization Fixup Classes
	}
}
