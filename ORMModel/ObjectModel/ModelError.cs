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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region RegenerateErrorTextEvents enum
	/// <summary>
	/// Specify which events should automatically trigger
	/// a GenerateErrorText call.
	/// </summary>
	[Flags]
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
	#region ModelErrorUses enum
	/// <summary>
	/// A model error can be interpreted differently depending
	/// on which element is returning it. For example, some errors
	/// may block verbalization of a given element and others
	/// are listed with a specific element so they will not
	/// </summary>
	[Flags]
	public enum ModelErrorUses
	{
		/// <summary>
		/// Use is not specified
		/// </summary>
		None = 0,
		/// <summary>
		/// Verbalization of the element can be completed and
		/// will display errors matching this filter when verbalization completes.
		/// Should not be combined with BlockVerbalization.
		/// </summary>
		Verbalize = 1,
		/// <summary>
		/// Verbalization of the element cannot be completed until
		/// this error is fixed.
		/// </summary>
		BlockVerbalization = 2,
		/// <summary>
		/// If the model element can have child elements with errors this 
		/// will let us filter out the child elements.
		/// </summary>
		DisplayPrimary = 4, 
	}
	#endregion // ModelErrorUses enum
	#region ModelErrorUsage struct
	/// <summary>
	/// A structure defining how a model error is being used.
	/// </summary>
	public struct ModelErrorUsage : IEquatable<ModelErrorUsage>
	{
		private readonly ModelError myError;
		private readonly ModelErrorUses myUses;
		/// <summary>
		/// Create a ModelErrorUsage structure
		/// </summary>
		/// <param name="error">The error. Cannot be null.</param>
		/// <param name="uses">Specifies how the error is used</param>
		public ModelErrorUsage(ModelError error, ModelErrorUses uses)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			myError = error;
			myUses = uses;
		}
		/// <summary>
		/// Create a ModelErrorUsage with default usage
		/// </summary>
		/// <param name="error">The error. Cannot be null.</param>
		public ModelErrorUsage(ModelError error) : this(error, ModelErrorUses.Verbalize){}
		/// <summary>
		/// The ModelError
		/// </summary>
		public ModelError Error
		{
			get
			{
				return myError;
			}
		}
		/// <summary>
		/// How the ModelError should be used
		/// </summary>
		public ModelErrorUses UseFor
		{
			get
			{
				return myUses;
			}
		}
		#region Equality and casting routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is ModelErrorUsage)
			{
				return Equals((ModelErrorUsage)obj);
			}
			return false;
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			ModelError error = myError;
			if (error != null)
			{
				return error.GetHashCode() ^ (((int)myUses) << 2);
			}
			return 0;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(ModelErrorUsage other)
		{
			return myError == other.myError && myUses == other.myUses;
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(ModelErrorUsage left, ModelErrorUsage right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(ModelErrorUsage left, ModelErrorUsage right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Automatically cast this structure to a ModelError
		/// </summary>
		public static implicit operator ModelError(ModelErrorUsage usage)
		{
			return usage.Error;
		}
		/// <summary>
		/// Automatically cast a ModelError to this structure
		/// </summary>
		public static implicit operator ModelErrorUsage(ModelError error)
		{
			return (error == null) ? default(ModelErrorUsage) : new ModelErrorUsage(error);
		}
		#endregion // Equality and casting routines
	}
	#endregion // ModelErrorUsage struct
	#region IModelErrorOwner interface
	/// <summary>
	/// Identify an object as an error owner. Used
	/// to identify errors associated with this object,
	/// and to automatically update the error text on
	/// a RegenerateErrorTextEvents.OwnerNameChange.
	/// </summary>
	public interface IModelErrorOwner
	{
		/// <summary>
		/// Get the enumeration of errors associated
		/// with this object.
		/// </summary>
		IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter);
		/// <summary>
		/// Called after deserialization to validate errors. Rules
		/// are not enabled when this is called.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		void ValidateErrors(INotifyElementAdded notifyAdded);
		/// <summary>
		/// Called to add delayed validate callbacks to model
		/// elements. The implementation should use the ORMCoreDomainModel.DelayValidateElement
		/// to register callbacks for element validation when the transaction is committed.
		/// </summary>
		void DelayValidateErrors();
	}
	#endregion // IModelErrorOwner interface
	#region IHasIndirectModelErrorOwner interface
	/// <summary>
	/// The IHasIndirectModelErrorOwner interface is used to indicate
	/// that the model errors directly attached to one object are
	/// listed as part of the ErrorCollection for another object.
	/// The other object is often the direct or indirect aggregate,
	/// but does not have to be. IHasIndirectModelErrorOwner can also
	/// be combined with IModelErrorOwner to show the error in multiple
	/// places. If the error is shown more than one step away then
	/// each element in the chain must implement this interface.
	/// Implementing IHasIndirectModelErrorOwner allows shapes to
	/// update automatically when an error is added/removed from
	/// an indirect owner.
	/// </summary>
	public interface IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Return an array of MetaRole guids. Each MetaRole
		/// represents a role on this object that can be followed
		/// to get an IModelErrorOwner implementation that represents
		/// this object.
		/// </summary>
		Guid[] GetIndirectModelErrorOwnerLinkRoles();
	}
	/// <summary>
	/// The IElementLinkRoleHasIndirectModelErrorOwner interface is used to
	/// indicate that model errors directly attached to the link
	/// object have an indirect model error owner. This is very
	/// similar to IHasIndirectModelErrorOwner, but this assumes
	/// the element coming in is an ElementLink, and the retrieved
	/// roles are roles on that link.
	/// </summary>
	public interface IElementLinkRoleHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Return an array of MetaRole guids. Each MetaRole
		/// represents a role that can be used to leave the link
		/// to get to an object that that can be followed to get
		/// an IModelErrorOwner implementation that represents
		/// this object.
		/// </summary>
		Guid[] GetIndirectModelErrorOwnerElementLinkRoles();
	}
	#endregion // IHasIndirectModelErrorOwner interface
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
		/// <returns>true if additional work was done to activate the error</returns>
		bool ActivateModelError(ModelError error);
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
			ModelError error = errorLink.Error;
			if (error.IsDeleted)
			{
				return;
			}
			IORMToolTaskProvider provider = (error.Store as IORMToolServices).TaskProvider;
			IORMToolTaskItem newTask = provider.CreateTask();
			newTask.ElementLocator = error as IRepresentModelElements;
			newTask.Text = error.ErrorText;
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
		private sealed class ModelErrorFixupListener : DeserializationFixupListener<IModelErrorOwner>
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
			protected sealed override void ProcessElement(IModelErrorOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				element.ValidateErrors(notifyAdded);
			}
			/// <summary>
			/// Add all model errors in the specific store to
			/// the task list.
			/// </summary>
			/// <param name="store">The context store</param>
			protected sealed override void PhaseCompleted(Store store)
			{
				IList<ModelHasError> errorLinks = store.ElementDirectory.FindElements<ModelHasError>();
				int linkCount = errorLinks.Count;
				for (int i = 0; i < linkCount; ++i)
				{
					ModelHasError errorLink = errorLinks[i];
					ModelError error = errorLink.Error;
					if (!errorLink.IsDeleted && !error.IsDeleted)
					{
						// Make sure the text is up to date
						error.GenerateErrorText();
						ModelError.AddToTaskProvider(errorLink);
					}
				}
			}
		}
		#endregion Deserialization Fixup
		#region Rule to update error text on model name change
		[RuleOn(typeof(ORMModel))] // ChangeRule
		private sealed partial class SynchronizeErrorTextForModelRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id.Equals(ORMModel.NameDomainPropertyId))
				{
					foreach (ModelError error in ((ORMModel)e.ModelElement).ErrorCollection)
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
		[RuleOn(typeof(ORMNamedElement))] // ChangeRule
		private sealed partial class SynchronizeErrorForOwnerRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id.Equals(ORMNamedElement.NameDomainPropertyId))
				{
					foreach (ModelError error in ((IModelErrorOwner)e.ModelElement).GetErrorCollection(ModelErrorUses.None))
					{
						if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
						{
							error.GenerateErrorText();
						}
					}
				}
			}
		}
		#endregion // Rule to update error text on owner name change
		#region HasErrors Static Function
		/// <summary>
		/// Checks to see if the Model Element contains errors
		/// </summary>
		/// <param name="modelElement">Any <see cref="ModelElement"/>. Errors may be reported if the element implements <see cref="IModelErrorOwner"/></param>
		/// <param name="useFilter">The filter for the error being displayed. See <see cref="ModelErrorUses"/> for more information.</param>
		/// <returns>Returns <see langword="true"/> if the errors are present for the provided filters</returns>
		public static bool HasErrors(ModelElement modelElement, ModelErrorUses useFilter)
		{
			return HasErrors(modelElement, useFilter, null);
		}
		/// <summary>
		/// Checks to see if the Model Element contains errors
		/// </summary>
		/// <param name="modelElement">Any <see cref="ModelElement"/>. Errors may be reported if the element implements <see cref="IModelErrorOwner"/></param>
		/// <param name="useFilter">The filter for the error being displayed. See <see cref="ModelErrorUses"/> for more information.</param>
		/// <param name="displayFilter">The <see cref="ModelErrorDisplayFilter"/> filter to determine if errors should be displayed or not.</param>
		/// <returns>Returns <see langword="true"/> if the errors are present for the provided filters</returns>
		public static bool HasErrors(ModelElement modelElement, ModelErrorUses useFilter, ModelErrorDisplayFilter displayFilter)
		{
			bool hasError = false;
			IModelErrorOwner errorOwner = modelElement as IModelErrorOwner;
			if (errorOwner != null)
			{
				if (displayFilter == null)
				{
					using (IEnumerator<ModelErrorUsage> enumerator = errorOwner.GetErrorCollection(useFilter).GetEnumerator())
					{
						hasError = enumerator.MoveNext();
					}
				}
				else
				{
					foreach (ModelErrorUsage usage in errorOwner.GetErrorCollection(useFilter))
					{
						if (displayFilter.ShouldDisplay(usage.Error))
						{
							hasError = true;
							break;
						}
					}
				}
			}
			return hasError;
		}
		#endregion //Has Errors Static Function
	}
	#endregion // ModelError class
} 
