using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using System.Globalization;
using System.Resources;

namespace ORMSolutions.ORMArchitect.ExtensionExample
{
	/// <summary>
	/// This is a ModelError added to the ORM Tool using extensions.
	/// This error informs the user of the ORM Tool that an ObjectType must have a meaningful name.
	/// i.e. The name must be something other than "ObjectType1" (or other variations).
	/// </summary>
	public partial class ObjectTypeRequiresMeaningfulNameError : ModelError
	{
		#region Validation Methods
		/// <summary>
		/// This method performs Validation of an ObjectType name.
		/// </summary>
		/// <param name="element">The ModelElement you are trying to validate.</param>
		private static void DelayValidateObjectTypeHasMeaningfulNameError(ModelElement element)
		{
			ObjectType objectType = element as ObjectType;
			ValidateObjectTypeName(objectType, null);
		}
		private static Regex objectTypeRegex;
		/// <summary>
		/// This method Validates an ObjectType name.
		/// If the ObjectType name has a name of "ObjectType"
		/// followed by any digits it will create a new error.
		/// </summary>
		/// <param name="objectType">The ObjectType you wish to be validated.</param>
		private static void ValidateObjectTypeName(ObjectType objectType, INotifyElementAdded notifyAdded)
		{
			if (!objectType.IsDeleted)
			{
				Regex regex = objectTypeRegex;
				if (regex == null)
				{
					ResourceManager resMgr = ResourceAccessor<ORMModel>.ResourceManager;
					regex = System.Threading.Interlocked.CompareExchange<Regex>(
						ref objectTypeRegex,
						new Regex(string.Format(CultureInfo.InvariantCulture, @"\A({0}|{1})\d+\z", resMgr.GetString("ObjectModel.ValueType"), resMgr.GetString("ObjectModel.EntityType")), RegexOptions.Compiled | RegexOptions.IgnoreCase),
						null);
					regex = objectTypeRegex;
				}
				string objectTypeName = objectType.Name;
				Match regexMatch = regex.Match(objectType.Name);

				LinkedElementCollection<ModelError> extensions = objectType.ExtensionModelErrorCollection;
				ObjectTypeRequiresMeaningfulNameError nameError = null;
				int extensionCount = extensions.Count;
				int i;
				for (i = 0; i < extensionCount; ++i)
				{
					if (extensions[i] is ObjectTypeRequiresMeaningfulNameError)
					{
						nameError = extensions[i] as ObjectTypeRequiresMeaningfulNameError;
						break;
					}
				}

				if (regexMatch.Success)
				{
					if (nameError == null)
					{
						nameError = new ObjectTypeRequiresMeaningfulNameError(objectType.Store);
						ExtensionElementUtility.AddExtensionModelError(objectType, nameError);
						nameError.Model = objectType.Model;
						nameError.GenerateErrorText();
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
						nameError.Delete();
					}
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
			ObjectType objectType = (ObjectType)ExtensionElementUtility.GetExtendedErrorOwnerElement(this);
			if (objectType != null)
			{
				string newText = string.Format(CultureInfo.CurrentCulture,
					"Object '{0}' in model '{1}' needs a meaningful name.",
					objectType.Name,
					objectType.Model.Name);
				ErrorText = newText;
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
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
		/// This Rule calls the DelayValidateElement method when a ObjectType is added to the Diagram.
		/// </summary>
		private static void ExtensionObjectTypeAddRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ModelHasObjectType)e.ModelElement).ObjectType, DelayValidateObjectTypeHasMeaningfulNameError);
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType)
		/// This method calls the DelayValidateElement method when the name of an ObjectType has been changed.
		/// </summary>
		private static void ExtensionObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ObjectType.NameDomainPropertyId)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateObjectTypeHasMeaningfulNameError);
			}
		}
		#endregion // ExtensionErrorRules
		#region CustomExtensionError Deserialization Fixup Classes
		/// <summary>
		/// A fixup listener for adding/removing the ObjectTypeRequiresMeaningfulError class
		/// </summary>
		public static IDeserializationFixupListener ObjectTypeNameErrorFixupListener
		{
			get
			{
				return new ObjectTypeNameFixupListener();
			}
		}
		/// <summary>
		/// Private class to Validate a ObjectType when the .orm file is Deserialization.
		/// </summary>
		private sealed class ObjectTypeNameFixupListener : DeserializationFixupListener<ObjectType>
		{
			public ObjectTypeNameFixupListener()
				: base((int)ExtensionExampleFixupPhase.ValidateMeaningfulNames)
			{
			}
			protected sealed override void ProcessElement(ObjectType element, Store store, INotifyElementAdded notifyAdded)
			{
				ValidateObjectTypeName(element, notifyAdded);
			}
		}
		#endregion // CustomExtensionError Deserialization Fixup Classes
	}
}
