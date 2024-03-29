#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
		None = 0,
		/// <summary>
		/// Regenerate the error text when the model
		/// name changes
		/// </summary>
		ModelNameChange = 1 << 0,
		/// <summary>
		/// Regenerate the error text when the parent
		/// object changes. The parent object is identified
		/// via the IModelErrorOwner interface.
		/// </summary>
		OwnerNameChange = 1 << 1,
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
		public ModelErrorUsage(ModelError error) : this(error, ModelErrorUses.Verbalize) { }
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
		/// elements. The implementation should use the <see cref="FrameworkDomainModel.DelayValidateElement"/> priority
		/// to register callbacks for element validation when the transaction is committed.
		/// </summary>
		void DelayValidateErrors();
	}
	#endregion // IModelErrorOwner interface
	#region IModelErrorDisplayContext interface
	/// <summary>
	/// Provides a flexible mechanism for using the same
	/// error information with multiple display contexts.
	/// The error using this context is responsible for
	/// determining the appropriate element for retrieving
	/// context from.
	/// </summary>
	public interface IModelErrorDisplayContext
	{
		/// <summary>
		/// Provide the owner information to use as the context in
		/// an error report.
		/// </summary>
		/// <remarks>The returned string should not be capitalized so
		/// that it can be used anywhere in the context error message.</remarks>
		string ErrorDisplayContext { get;}
	}
	#endregion // IModelErrorDisplayContext interface
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
	#endregion // IHasIndirectModelErrorOwner interface
	#region IElementLinkRoleHasIndirectModelErrorOwner interface
	/// <summary>
	/// The IElementLinkRoleHasIndirectModelErrorOwner interface is used to
	/// indicate that model errors directly attached to the link
	/// object have an indirect model error owner. This is very
	/// similar to <see cref="IHasIndirectModelErrorOwner"/>, but this assumes
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
	#endregion // IElementLinkRoleHasIndirectModelErrorOwner interface
	#region IResolveCustomErrorOwner interface
	/// <summary>
	/// Implement on an extension domain model to allow for display of model
	/// errors on elements that are related via extension to an error owner.
	/// These relationships are not known to the error object in advance, so
	/// cannot otherwise be notified with the <see cref="ModelError.WalkAssociatedElements(AssociatedErrorElementCallback)"/> and
	/// <see cref="ModelError.WalkAssociatedElements(ModelElement, AssociatedErrorElementCallback)"/> methods.
	/// </summary>
	public interface IResolveCustomErrorOwner
	{
		/// <summary>
		/// Resolve on an element in an error owner path to an alternate error owner.
		/// </summary>
		/// <param name="errorPathElement">The current node in the path, including
		/// the primary owner.</param>
		/// <returns>One or more error nodes. If the node implements <see cref="IModelErrorOwner"/> (generally
		/// by deferring to the remotely viewed object, but errors can also be removed or added) then
		/// the <see cref="AssociatedErrorElementCallback"/> will be invoked for this element.</returns>
		IEnumerable<ModelElement> ResolveCustomErrorOwner(ModelElement errorPathElement);
	}
	#endregion // IResolveCustomErrorOwner interface
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
		/// <param name="element">The element to find a proxy for. If a <see cref="Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement"/>
		/// is returned here then it will generally be attached to the implementing shape and will
		/// me used in its place.</param>
		/// <param name="forError">The <see cref="ModelError"/> that is being displayed.
		/// If the displayed as element does not display this error, then it should not
		/// be identified as a proxy display.</param>
		/// <returns>The proxy display element. Return the element itself or null
		/// if there is no proxy. To redirect to a different known shape, return
		/// a <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement"/> or a
		/// <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.DiagramItem"/></returns>
		object ElementDisplayedAs(ModelElement element, ModelError forError);
	}
	#endregion // IProxyDisplayProvider
	#region ProxyDisplayProviderDirective class
	/// <summary>
	/// Directives providing objects to return from <see cref="IProxyDisplayProvider.ElementDisplayedAs"/>
	/// to provide special directions to the selection engine.
	/// </summary>
	public static class ProxyDisplayProviderDirective
	{
		/// <summary>
		/// Return from <see cref="IProxyDisplayProvider.ElementDisplayedAs"/> to
		/// instruction the selection choose that the shape cannot be used to select
		/// the desired object at this time.
		/// </summary>
		public static readonly object IgnoreShape = "IgnoreShapeDirective";
	}
	#endregion // ProxyDisplayProviderDirective class
	#region IIndirectModelErrorOwnerPath
	/// <summary>
	/// An interface to implement on an <see cref="ElementLink"/> to
	/// indicate that one of the role players is a remote <see cref="IModelErrorOwner"/>
	/// for the other endpoint. Generally, deriving the owning relationship to
	/// a <see cref="ModelError"/> from the <see cref="ElementAssociatedWithModelError"/>
	/// is sufficient for updating the error display. However, in cases where model errors
	/// are displayed remotely and events are used to synchronize error state for deleted
	/// objects, the path from an owner to the error may be broken, so the owner cannot
	/// be notified that the error state needs to be updated. This is a helper interface
	/// that is already implemented by ElementAssociatedWithModelError that supports easy
	/// integration with custom events designed to monitor error state. The interface
	/// is not automatically monitored, so events must be explicitly attached.
	/// </summary>
	public interface IModelErrorOwnerPath
	{
		/// <summary>
		/// The role player element that is or leads to the
		/// remoted <see cref="IModelErrorOwner"/> implementation.
		/// </summary>
		ModelElement ErrorOwnerRolePlayer { get;}
	}
	#endregion // 
	#region AssociatedErrorElementCallback delegate
	/// <summary>
	/// Used with the <see cref="ModelError.WalkAssociatedElements(AssociatedErrorElementCallback)"/> and
	/// <see cref="ModelError.WalkAssociatedElements(ModelElement, AssociatedErrorElementCallback)"/> methods.
	/// </summary>
	/// <param name="associatedElement"></param>
	public delegate void AssociatedErrorElementCallback(ModelElement associatedElement);
	#endregion // AssociatedErrorElementCallback
	#region ModelError class
	public abstract partial class ModelError : IRepresentModelElements
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
		/// Test if an error should be displayed to the use based on the
		/// current type-based filter state and the <see cref="ErrorState"/>
		/// of the error itself.
		/// </summary>
		/// <param name="error">The error to test.</param>
		/// <param name="filter">The filter to apply. Can be <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the error exists in a non-ignored state
		/// and the error type is allowed by the current filter.</returns>
		public static bool IsDisplayed(ModelError error, ModelErrorDisplayFilter filter)
		{
			return error != null &&
				error.ErrorState != ModelErrorState.Ignored &&
				(filter == null || !filter.IsErrorExcluded(error.GetType()));
		}
		/// <summary>
		/// Test if an error would normally be displayed but is currently hidden
		/// based on the current type-based filter state and the <see cref="ErrorState"/>
		/// of the error itself.
		/// </summary>
		/// <param name="error">The error to test.</param>
		/// <param name="filter">The filter to apply. Can be <see langword="null"/>.</param>
		/// <returns><see langword="true"/> if the error exists in a non-ignored state
		/// and the error type is allowed by the current filter.</returns>
		public static bool IsDisplayFiltered(ModelError error, ModelErrorDisplayFilter filter)
		{
			return error != null &&
				error.ErrorState != ModelErrorState.Ignored &&
				(filter != null && filter.IsErrorExcluded(error.GetType()));
		}
		/// <summary>
		/// Helper function to add an error to the task provider
		/// when the error is attached to the model.
		/// </summary>
		/// <param name="errorLink"></param>
		public static void AddToTaskProvider(ModelHasError errorLink)
		{
			AddToTaskProvider(errorLink.Error);
		}
		/// <summary>
		/// Helper function to add an error to the task provider
		/// when the error display filter changes.
		/// </summary>
		/// <param name="error"></param>
		public static void AddToTaskProvider(ModelError error)
		{
			if (error.IsDeleted)
			{
				return;
			}
			IORMToolTaskProvider taskProvider;
			if (null != (taskProvider = ((IORMToolServices)error.Store).TaskProvider) &&
				ModelError.IsDisplayed(error, error.Model.ModelErrorDisplayFilter))
			{
				taskProvider = (error.Store as IORMToolServices).TaskProvider;
				IORMToolTaskItem newTask = taskProvider.CreateTask();
				newTask.ElementLocator = error as IRepresentModelElements;
				newTask.Text = error.ErrorText;
				Debug.Assert(error.TaskData == null);
				error.TaskData = newTask;
				taskProvider.AddTask(newTask);
			}
		}
		/// <summary>
		/// Called to set the name of the error.
		/// </summary>
		public abstract void GenerateErrorText();
		/// <summary>
		/// Get a compact version of the error text that does not include
		/// any context information. This text should be short enough
		/// to fit in space-limited spaces such as a context menu.
		/// </summary>
		public abstract string CompactErrorText { get;}
		/// <summary>
		/// Determines which name change events will
		/// automatically regenerate the error text.
		/// </summary>
		public abstract RegenerateErrorTextEvents RegenerateEvents { get;}
		/// <summary>
		/// Called at the end of deserialization fixup to enable an error
		/// to validate its <see cref="ErrorState"/> settings before an
		/// attempt is made to display the error.
		/// </summary>
		protected virtual void FixupErrorState()
		{
			// Intentionally empty
		}
		#endregion // ModelError specific
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. Return an error fixup listener
		/// for errors in the specified domain model. The fixup listener validates
		/// all model errors and adds errors to the task provider.
		/// </summary>
		/// <param name="fixupPhase">The phase for this listener</param>
		/// <param name="errorDomainModel">The domain model that owns the errors</param>
		public static IDeserializationFixupListener GetFixupListener(int fixupPhase, DomainModelInfo errorDomainModel)
		{
			return new ModelErrorFixupListener(fixupPhase, errorDomainModel);
		}
		/// <summary>
		/// A listener class to validate and/or populate the ModelError
		/// collection on load, as well as populating the task list.
		/// </summary>
		private sealed class ModelErrorFixupListener : DeserializationFixupListener<IModelErrorOwner>
		{
			private DomainModelInfo myDomainModelFilter;
			/// <summary>
			/// Create a new ModelErrorFixupListener
			/// </summary>
			/// <param name="fixupPhase">The phase for this listener</param>
			/// <param name="errorDomainModel">The domain model that owns the errors</param>
			public ModelErrorFixupListener(int fixupPhase, DomainModelInfo errorDomainModel)
				: base(fixupPhase)
			{
				myDomainModelFilter = errorDomainModel;
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
			/// Verify that the element belongs to the correct domain model
			/// </summary>
			protected override bool VerifyElementType(ModelElement element)
			{
				DomainModelInfo modelFilter = myDomainModelFilter;
				return (modelFilter != null) ? element.GetDomainClass().DomainModel == modelFilter : true;
			}
			/// <summary>
			/// Add all model errors in the specific store that match the specified domain
			/// model to the task provider.
			/// </summary>
			protected sealed override void PhaseCompleted(Store store)
			{
				DomainModelInfo modelFilter = myDomainModelFilter;
				IList<ModelHasError> errorLinks = store.ElementDirectory.FindElements<ModelHasError>();
				int linkCount = errorLinks.Count;
				for (int i = 0; i < linkCount; ++i)
				{
					ModelHasError errorLink = errorLinks[i];
					ModelError error = errorLink.Error;
					if (!errorLink.IsDeleted &&
						!error.IsDeleted &&
						(modelFilter == null || error.GetDomainClass().DomainModel == modelFilter))
					{
						// Make sure the error state is correct based on the full error state
						error.FixupErrorState();
						// Make sure the text is up to date
						error.GenerateErrorText();
						ModelError.AddToTaskProvider(errorLink);
					}
				}
			}
		}
		#endregion Deserialization Fixup
		#region Rule to update error text on model name change
		/// <summary>
		/// ChangeRule: typeof(ORMModel)
		/// </summary>
		private static void SynchronizeErrorTextForModelRule(ElementPropertyChangedEventArgs e)
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
		#endregion // Rule to update error text on model name change
		#region Rule to update error text on owner name change
		/// <summary>
		/// ChangeRule: typeof(ORMNamedElement)
		/// </summary>
		private static void SynchronizeErrorTextForOwnerRule(ElementPropertyChangedEventArgs e)
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
		#endregion // Rule to update error text on owner name change
		#region WalkAssociatedElements method
		/// <summary>
		/// Walk all elements directly or indirectly associated with this model error
		/// </summary>
		/// <param name="callback"><see cref="AssociatedErrorElementCallback"/> delegate</param>
		public void WalkAssociatedElements(AssociatedErrorElementCallback callback)
		{
			IFrameworkServices services = this.Store as IORMToolServices;
			IResolveCustomErrorOwner[] customResolvers = null;
			if (services != null)
			{
				customResolvers = services.GetTypedDomainModelProviders<IResolveCustomErrorOwner>();
			}
			foreach (ModelElement element in this.AssociatedElementCollection)
			{
				WalkAssociatedElements(element, callback);
			}
		}
		/// <summary>
		/// Walk all elements directly or indirectly associated with a model error,
		/// starting with the specified associated element
		/// </summary>
		/// <param name="associatedElement">The associated <see cref="ModelElement"/></param>
		/// <param name="callback"><see cref="AssociatedErrorElementCallback"/> delegate</param>
		public static void WalkAssociatedElements(ModelElement associatedElement, AssociatedErrorElementCallback callback)
		{
			IFrameworkServices services = associatedElement.Store as IFrameworkServices;
			IResolveCustomErrorOwner[] customResolvers = null;
			if (services != null)
			{
				customResolvers = services.GetTypedDomainModelProviders<IResolveCustomErrorOwner>();
			}
			WalkAssociatedElements(associatedElement, callback, customResolvers);
		}
		/// <summary>
		/// Walk all elements directly or indirectly associated with a model error,
		/// starting with the specified associated element
		/// </summary>
		/// <param name="associatedElement">The associated <see cref="ModelElement"/></param>
		/// <param name="callback"><see cref="AssociatedErrorElementCallback"/> delegate</param>
		/// <param name="customResolvers">Array of custom resolves to navigation an owner path in extension models. Can be null.</param>
		private static void WalkAssociatedElements(ModelElement associatedElement, AssociatedErrorElementCallback callback, IResolveCustomErrorOwner[] customResolvers)
		{
			WalkAssociatedElementsHelper(associatedElement, callback, customResolvers, null);

			ElementLink elementLink;
			IElementLinkRoleHasIndirectModelErrorOwner indirectOwnerLink;
			if (null != (elementLink = associatedElement as ElementLink) &&
				null != (indirectOwnerLink = elementLink as IElementLinkRoleHasIndirectModelErrorOwner))
			{
				Guid[] guids;
				int roleCount;
				if (null != (guids = indirectOwnerLink.GetIndirectModelErrorOwnerElementLinkRoles()) &&
					0 != (roleCount = guids.Length))
				{
					for (int i = 0; i < roleCount; ++i)
					{
						WalkAssociatedElementsHelper(DomainRoleInfo.GetRolePlayer(elementLink, guids[i]), callback, customResolvers, null);
					}
				}
			}
		}
		private static void WalkAssociatedElementsHelper(ModelElement element, AssociatedErrorElementCallback callback, IResolveCustomErrorOwner[] customResolvers, Predicate<ModelElement> filter)
		{
			if (element is IModelErrorOwner)
			{
				callback(element);
			}
			IHasIndirectModelErrorOwner indirectOwner;
			DomainDataDirectory domainDataDirectory = null;
			if (null != (indirectOwner = element as IHasIndirectModelErrorOwner))
			{
				Guid[] indirectRoles;
				int indirectRoleCount;
				if (null != (indirectRoles = indirectOwner.GetIndirectModelErrorOwnerLinkRoles()) &&
					0 != (indirectRoleCount = indirectRoles.Length))
				{
					domainDataDirectory = element.Store.DomainDataDirectory;
					for (int i = 0; i < indirectRoleCount; ++i)
					{
						foreach (ModelElement linkedElement in domainDataDirectory.FindDomainRole(indirectRoles[i]).GetLinkedElements(element))
						{
							if (filter != null && filter(linkedElement))
							{
								continue;
							}
							WalkAssociatedElementsHelper(
								linkedElement,
								callback,
								customResolvers,
								delegate(ModelElement testElement)
								{
									return testElement == element ||
										(filter != null && filter(testElement));
								});
						}
					}
				}
			}

			ElementLink elementLink;
			IElementLinkRoleHasIndirectModelErrorOwner indirectLinkRoleOwner;
			if (null != (indirectLinkRoleOwner = element as IElementLinkRoleHasIndirectModelErrorOwner) &&
				null != (elementLink = element as ElementLink))
			{
				Guid[] metaRoles = indirectLinkRoleOwner.GetIndirectModelErrorOwnerElementLinkRoles();
				int roleCount;
				if (metaRoles != null &&
					0 != (roleCount = metaRoles.Length))
				{
					if (domainDataDirectory == null)
					{
						domainDataDirectory = element.Store.DomainDataDirectory;
					}
					for (int i = 0; i < roleCount; ++i)
					{
						DomainRoleInfo metaRole = domainDataDirectory.FindDomainRole(metaRoles[i]);
						if (metaRole != null)
						{
							ModelElement rolePlayer = metaRole.GetRolePlayer(elementLink);
							if (filter != null && filter(rolePlayer))
							{
								continue;
							}
							WalkAssociatedElementsHelper(
								rolePlayer,
								callback,
								customResolvers,
								delegate(ModelElement testElement)
								{
									return testElement == element ||
										(filter != null && filter(testElement));
								});
						}
					}
				}
			}

			if (customResolvers != null)
			{
				for (int i = 0; i < customResolvers.Length; ++i)
				{
					IEnumerable<ModelElement> customElements = customResolvers[i].ResolveCustomErrorOwner(element);
					if (customElements != null)
					{
						foreach (ModelElement customElement in customElements)
						{
							if (filter != null && filter(customElement))
							{
								continue;
							}
							WalkAssociatedElementsHelper(
								customElement,
								callback,
								customResolvers,
								delegate (ModelElement testElement)
								{
									return customElement == element ||
										(filter != null && filter(customElement));
								});
						}
					}
				}
			}
		}
		#endregion // WalkAssociatedElements method
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
				foreach (ModelErrorUsage usage in errorOwner.GetErrorCollection(useFilter))
				{
					if (ModelError.IsDisplayed(usage.Error, displayFilter))
					{
						hasError = true;
						break;
					}
				}
			}
			return hasError;
		}
		#endregion //Has Errors Static Function
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Default implementation of <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// based on the <see cref="ElementAssociatedWithModelError"/> relationship.
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return ElementAssociatedWithModelError.GetAssociatedElementCollection(this).ToArray();
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // ModelError class
	#region ElementAssociatedWithModelError class
	partial class ElementAssociatedWithModelError : IModelErrorOwnerPath
	{
		#region IModelErrorOwnerPath Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwnerPath.ErrorOwnerRolePlayer"/>
		/// </summary>
		protected ModelElement ErrorOwnerRolePlayer
		{
			get
			{
				return AssociatedElement;
			}
		}
		ModelElement IModelErrorOwnerPath.ErrorOwnerRolePlayer
		{
			get
			{
				return ErrorOwnerRolePlayer;
			}
		}
		#endregion // IModelErrorOwnerPath Implementation
	}
	#endregion // ElementAssociatedWithModelError class
}
