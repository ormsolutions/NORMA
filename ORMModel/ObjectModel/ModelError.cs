using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
{
	/// <summary>
	/// Specify which events should automatically trigger
	/// a GenerateErrorText call.
	/// </summary>
	[Flags]
	[CLSCompliant(true)]
	public enum RegenerateErrorTextEvents
	{
		/// <summary>
		/// Error text is not regenerated
		/// </summary>
		None,
		/// <summary>
		/// Regenerate the error text when the model
		/// name changes
		/// </summary>
		ModelNameChange,
		/// <summary>
		/// Regenerate the error text when the parent
		/// object changes. The parent object is identified
		/// via the IModelErrorOwner interface.
		/// </summary>
		OwnerNameChange,
	}
	/// <summary>
	/// Identify an object as an error owner. Used
	/// to identify errors associated with this object,
	/// and to automatically update the error text on
	/// a RegenerateErrorTextEvents.OwnerNameChange.
	/// </summary>
	[CLSCompliant(false)]
	public interface IModelErrorOwner
	{
		/// <summary>
		/// Get the enumeration of errors associated
		/// with this object.
		/// </summary>
		IEnumerable<ModelError> ErrorCollection { get;}
	}
	public abstract partial class ModelError
	{
		private object myTaskData;
		/// <summary>
		/// Non-IMS managed slot for storing error reporting
		/// data. Allows items to be easily removed from the task list.
		/// </summary>
		public object TaskData
		{
			get { return myTaskData; }
			set { myTaskData = value; }
		}
		/// <summary>
		/// Called to set the name of the error.
		/// </summary>
		public abstract void GenerateErrorText();
		/// <summary>
		/// Determines which name change events will
		/// automatically regenerate the error text.
		/// </summary>
		public abstract RegenerateErrorTextEvents RegenerateEvents { get;}
		#region Rule to update error text on model name change
		[RuleOn(typeof(ORMModel))]
		private class SynchronizeErrorTextForModelRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == NamedElement.NameMetaAttributeGuid)
				{
					ORMModel model = e.ModelElement as ORMModel;
					foreach (ModelError error in model.ErrorCollection)
					{
						if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.ModelNameChange))
						{
							error.GenerateErrorText();
						}
					}
				}
			}
		}
		#endregion // Rule to update error text on model name change
		#region Rule to update error text on model name change
		[RuleOn(typeof(NamedElement))]
		private class SynchronizeErrorForOwnerRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				IModelErrorOwner owner = e.ModelElement as IModelErrorOwner;
				if (owner != null)
				{
					Guid attributeGuid = e.MetaAttribute.Id;
					if (attributeGuid == NamedElement.NameMetaAttributeGuid)
					{
						foreach (ModelError error in owner.ErrorCollection)
						{
							if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
							{
								error.GenerateErrorText();
							}
						}
					}
				}
			}
		}
		#endregion // Rule to update error text on model name change
	}
}