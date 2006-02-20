using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region RegenerateErrorTextEvents enum
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
	#endregion // RegenerateErrorTextEvents enum
	#region IModelErrorOwner interface
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
		/// <summary>
		/// Called after deserialization to validate errors. Rules
		/// are not enabled when this is called.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		void ValidateErrors(INotifyElementAdded notifyAdded);
		/// <summary>
		/// Called to add delayed validate callbacks to model
		/// elements. The implementation should use the ORMMetaModel.DelayValidateElement
		/// to register callbacks for element validation when the transaction is committed.
		/// </summary>
		void DelayValidateErrors();
	}
	#endregion // IModelErrorOwner interface
	#region IModelErrorActivation interface
	/// <summary>
	/// Interface to implement on a shape element
	/// to support custom actions when an element is
	/// double-clicked in the error list.
	/// </summary>
	public interface IModelErrorActivation
	{
		/// <summary>
		/// Method called after a shape has been selected
		/// in the diagram
		/// </summary>
		/// <param name="error">The error being activated</param>
		void ActivateModelError(ModelError error);
	}
	#endregion // IModelErrorActivation interface
	#region IProxyDisplayProvider
	/// <summary>
	/// Interface to map the display of one element to
	/// the display of a second element. It is very possible
	/// to have errors on objects that are not directly displayed
	/// on the model. Generally, this would force the error
	/// activation mechanism to simply select the diagram
	/// displaying the model, as this is the first element in
	/// the parent chain with a displayable shape. If a display
	/// proxy is encountered in the parent chain, then the
	/// selection loop reverts to the starting element and
	/// an attempt is made at each level to map to a proxy
	/// element and select the proxy instead.
	/// </summary>
	public interface IProxyDisplayProvider
	{
		/// <summary>
		/// Return the element that is used to display the
		/// passed in element.
		/// </summary>
		/// <param name="element">The element to find a proxy for</param>
		/// <returns>The proxy display element. Return the element itself or null
		/// if there is no proxy.</returns>
		ModelElement ElementDisplayedAs(ModelElement element);
	}
	#endregion // IProxyDisplayProvider
	#region ModelError class
	public abstract partial class ModelError
	{
		#region Member Variables
		private object myTaskData;
		#endregion // Member Variables
		#region ModelError specific
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
		/// Helper function to add an error to the task provider
		/// when the error is attached to the model.
		/// </summary>
		/// <param name="errorLink"></param>
		public static void AddToTaskProvider(ModelHasError errorLink)
		{
			ModelError error = errorLink.ErrorCollection;
			if (error.IsRemoved)
			{
				return;
			}
			IORMToolTaskProvider provider = (error.Store as IORMToolServices).TaskProvider;
			IORMToolTaskItem newTask = provider.CreateTask();
			newTask.ElementLocator = error as IRepresentModelElements;
			newTask.Text = error.Name;
			Debug.Assert(error.TaskData == null);
			error.TaskData = newTask;
			provider.AddTask(newTask);
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
		#endregion // ModelError specific
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// validates all model errors and adds errors to the task provider.
		/// </summary>
		[CLSCompliant(false)]
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ModelErrorFixupListener();
			}
		}
		/// <summary>
		/// A listener class to validate and/or populate the ModelError
		/// collection on load, as well as populating the task list.
		/// </summary>
		private class ModelErrorFixupListener : DeserializationFixupListener<IModelErrorOwner>
		{
			/// <summary>
			/// Create a new ModelErrorFixupListener
			/// </summary>
			public ModelErrorFixupListener() : base((int)ORMDeserializationFixupPhase.ValidateErrors)
			{
			}
			/// <summary>
			/// Defer to the IModelErrorOwner.ValidateErrors method
			/// to fixup error problems.
			/// </summary>
			/// <param name="element">An IModelErrorOwner instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(IModelErrorOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				element.ValidateErrors(notifyAdded);
			}
			/// <summary>
			/// Add all model errors in the specific store to
			/// the task list.
			/// </summary>
			/// <param name="store">The context store</param>
			protected override void PhaseCompleted(Store store)
			{
				IList errorLinks = store.ElementDirectory.GetElements(ModelHasError.MetaClassGuid);
				int linkCount = errorLinks.Count;
				for (int i = 0; i < linkCount; ++i)
				{
					ModelHasError error = (ModelHasError)errorLinks[i];
					if (!error.IsRemoved)
					{
						ModelError.AddToTaskProvider(error);
					}
				}
			}
		}
		#endregion Deserialization Fixup
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
		#region Rule to update error text on owner name change
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
		#endregion // Rule to update error text on owner name change
	}
	#endregion // ModelError class
} 