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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.Modeling;
using EnvDTE;

namespace Neumont.Tools.ORM.Shell
{
	#region ORMDesignerDocData class
	/// <summary>
	/// DocData object for the ORM Designer editor
	/// </summary>
	[CLSCompliant(false)]
	public partial class ORMDesignerDocData : ModelingDocData, IExtensibleObject
	{
		#region Private flags
		[Flags]
		private enum PrivateFlags
		{
			None = 0,
			AddedPostLoadEvents = 1,
			AddedPreLoadEvents = 2,
			AddedSurveyQuestionEvents = 4,
			SaveDisabled = 8,
			// Other flags here, add instead of lots of bool variables
		}
		private PrivateFlags myFlags;
		private bool GetFlag(PrivateFlags flags)
		{
			return 0 != (myFlags & flags);
		}
		private void SetFlag(PrivateFlags flags, bool value)
		{
			if (value)
			{
				myFlags |= flags;
			}
			else
			{
				myFlags &= ~flags;
			}
		}

		#endregion // Private flags
		#region Member variables
		private Stream myFileStream;
		private IDictionary<string, Type> myExtensionDomainModels;
		private static readonly Dictionary<string, Type> myStandardDomainModels = InitializeStandardDomainModels();
		private delegate void StoreDiagramMappingDataClearChangesDelegate();
		private static readonly StoreDiagramMappingDataClearChangesDelegate myStoreDiagramMappingDataClearChanges = InitializeStoreDiagramMappingDataClearChanges();
		#endregion // Member variables
		#region Construction/destruction
		/// <summary>
		/// Standard <see cref="DocData"/> constructor, called by <see cref="Microsoft.VisualStudio.Package.EditorFactory"/>.
		/// </summary>
		public ORMDesignerDocData(IServiceProvider serviceProvider, Guid editorId)
			: base(serviceProvider, editorId)
		{
		}
		/// <summary>
		/// Initialize the dictionary of standard <see cref="DomainModel"/>s needed for the tool.
		/// </summary>
		private static Dictionary<string, Type> InitializeStandardDomainModels()
		{
			Dictionary<string, Type> standardDomainModels = new Dictionary<string, Type>();
			standardDomainModels.Add(ORMCoreDomainModel.XmlNamespace, typeof(ORMCoreDomainModel));
			standardDomainModels.Add(ORMShapeDomainModel.XmlNamespace, typeof(ORMShapeDomainModel));
			return standardDomainModels;
		}
		private static StoreDiagramMappingDataClearChangesDelegate InitializeStoreDiagramMappingDataClearChanges()
		{
			Type storeDiagramMappingDataType = typeof(Diagram).Assembly.GetType("Microsoft.VisualStudio.Modeling.Diagrams.StoreDiagramMappingData", false, false);
			if (storeDiagramMappingDataType != null)
			{
				PropertyInfo instanceProperty = storeDiagramMappingDataType.GetProperty("Instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, storeDiagramMappingDataType, Type.EmptyTypes, null);
				if (instanceProperty != null)
				{
					MethodInfo getInstanceMethod = instanceProperty.GetGetMethod(true);
					if (getInstanceMethod != null)
					{
						object instance = getInstanceMethod.Invoke(null, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, null, CultureInfo.InvariantCulture);
						if (instance != null)
						{
							MethodInfo clearChangesMethod = storeDiagramMappingDataType.GetMethod("ClearChanges", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.ExactBinding | BindingFlags.DeclaredOnly, null, CallingConventions.HasThis, Type.EmptyTypes, null);
							if (clearChangesMethod != null)
							{
								return (StoreDiagramMappingDataClearChangesDelegate)Delegate.CreateDelegate(typeof(StoreDiagramMappingDataClearChangesDelegate), instance, clearChangesMethod, false);
							}
						}
					}
				}
			}
			return null;
		}
		#endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// Retrieves an <see cref="IList{Type}"/> of the <see cref="Type"/>s of the <see cref="DomainModel"/>s used by the designer.
		/// </summary>
		protected override IList<Type> GetDomainModels()
		{
			Dictionary<string, Type> standardDomainModels = myStandardDomainModels;
			// Always have 1 for the CoreDesignSurface. Note that the framework automatically
			// loads CoreDomainModel (which contains ModelElement, ElementLink, and ElementDeserializedRule).
			int count = standardDomainModels.Count + 1;
			IDictionary<string, Type> extensionDomainModels = myExtensionDomainModels;
			if (extensionDomainModels != null)
			{
				count += extensionDomainModels.Count;
			}
			List<Type> retVal = new List<Type>(count);
			retVal.Add(typeof(CoreDesignSurfaceDomainModel));
			retVal.AddRange(standardDomainModels.Values);
			if (extensionDomainModels != null)
			{
				retVal.AddRange(extensionDomainModels.Values);
			}
			return retVal;
		}
		/// <summary>
		/// Reload this document from a <see cref="Stream"/> instead of from a file.
		/// </summary>
		/// <param name="stream">The <see cref="Stream"/> to load</param>
		public void ReloadFromStream(Stream stream)
		{
			myFileStream = stream;
			// This calls into LoadDocData(string, bool) after doing necessary cleanup
			ReloadDocData((uint)_VSRELOADDOCDATA.RDD_RemoveUndoStack);
		}
		/// <summary>
		/// See the <see cref="LoadDocData"/> method.
		/// </summary>
		/// <param name="fileName">The file name that we pass to <see cref="ModelingDocData.LoadDocData"/>.</param>
		/// <param name="inputStream">The stream from which we are trying to load.</param>
		/// <param name="isReload">Tells us if the file is being reloaded or not.</param>
		private int LoadDocDataFromStream(string fileName, bool isReload, Stream inputStream)
		{
			// HACK: MSBUG: StoreDiagramMappingData can end up containing information from old, disposed Stores if any
			// TransactionCommittingRule makes model changes after the DiagramCommittingRule has already run. Since the
			// FireTime, Priority, and IsEnabled properties of TransactionCommittingRules are completely ignored, we have
			// no way to prevent this from occurring. The only full solution is to not use TransactionCommittingRules at
			// all, but we have no way to stop others from doing so. In case they do, by clearing StoreDiagramMappingData
			// here, we at least avoid a crash during load. We need to do this on both regular loads and reloads, since
			// StoreDiagramMappingData is a static singleton.
			StoreDiagramMappingDataClearChangesDelegate clearChanges = myStoreDiagramMappingDataClearChanges;
			if ((object)clearChanges != null)
			{
				clearChanges();
			}

			if (isReload)
			{
				this.RemoveModelingEventHandlers();

				IDisposable propertyProviderService = this.myPropertyProviderService as IDisposable;
				if (propertyProviderService != null)
				{
					propertyProviderService.Dispose();
				}
				// Null out the myPropertyProviderService field so that a new instance will be created
				// with the new Store next time it is needed
				this.myPropertyProviderService = null;

				foreach (ModelingDocView view in DocViews)
				{
					MultiDiagramDocView multiDiagramDocView = view as MultiDiagramDocView;
					if (multiDiagramDocView != null)
					{
						multiDiagramDocView.RemoveAllDiagrams();
					}
				}

				// Remove items from the ErrorList (TaskList) when isReload is true.
				// The Tasks in the ErrorList (TaskList) are not removed when isReload is true.
				// So, we have duplicates when a file is reloaded
				// (after a custom extension is removed or added)!
				IORMToolTaskProvider taskProvider = this.myTaskProvider;
				if (taskProvider != null)
				{
					taskProvider.RemoveAllTasks();
				}
			}
			// Convert early so we can accurately check extension elements
			int retVal = 0;
			bool dontSave = false;
			ORMDesignerSettings settings = ORMDesignerPackage.DesignerSettings;
			using (Stream convertedStream = settings.ConvertStream(inputStream))
			{
				dontSave = convertedStream != null;
				Stream stream = dontSave ? convertedStream : inputStream;
				myFileStream = stream;
				try
				{
					XmlReaderSettings readerSettings = new XmlReaderSettings();
					readerSettings.CloseInput = false;
					IDictionary<string, Type> documentExtensions = ORMDesignerPackage.GetAutoLoadExtensions();
					using (XmlReader reader = XmlReader.Create(stream, readerSettings))
					{
						reader.MoveToContent();
						if (reader.NodeType == XmlNodeType.Element)
						{
							if (reader.MoveToFirstAttribute())
							{
								do
								{
									if (reader.Prefix == "xmlns")
									{
										string URI = reader.Value;
										if (!string.Equals(URI, ORMCoreDomainModel.XmlNamespace, StringComparison.Ordinal) &&
											!string.Equals(URI, ORMShapeDomainModel.XmlNamespace, StringComparison.Ordinal) &&
											!string.Equals(URI, ORMSerializer.RootXmlNamespace, StringComparison.Ordinal))
										{
											Type extensionType = ORMDesignerPackage.GetExtensionDomainModel(URI);
											if (extensionType != null)
											{
												if (documentExtensions == null)
												{
													documentExtensions = new Dictionary<string, Type>();
												}
												documentExtensions[URI] = extensionType;
											}
										}
									}
								} while (reader.MoveToNextAttribute());
							}
						}
					}
					myExtensionDomainModels = documentExtensions;
					stream.Position = 0;

					retVal = base.LoadDocData(fileName, isReload);

					// HACK: After the file is loaded and the load transaction has committed, commit a new transaction.
					// For some reason this seems to fix various line routing issues (including the lines not showing up).
					TransactionManager transactionManager = this.Store.TransactionManager;
					if (!transactionManager.InTransaction)
					{
						using (Transaction t = transactionManager.BeginTransaction())
						{
							t.Commit();
						}
						this.FlushUndoManager();
					}
				}
				finally
				{
					myFileStream = null;
				}
			}
			if (dontSave)
			{
				SetFlag(PrivateFlags.SaveDisabled, true);
				IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
				docTable.ModifyDocumentFlags(Cookie, (uint)_VSRDTFLAGS.RDT_DontSave, 1);
			}
			this.AddPostLoadModelingEventHandlers();
			return retVal;
		}
		/// <summary>
		/// Override the LoadDocData method.
		/// We override here to do a peek ahead and load the namespaces in the ORM file.
		/// </summary>
		/// <param name="fileName">The name of the file we are trying to load.</param>
		/// <param name="isReload">Tells us if the file is being reloaded or not.</param>
		/// <returns></returns>
		protected override int LoadDocData(string fileName, bool isReload)
		{
			Stream surrogateStream = myFileStream;
			if (surrogateStream != null)
			{
				myFileStream = null;
				return LoadDocDataFromStream(fileName, isReload, surrogateStream);
			}
			else
			{
				using (FileStream fileStream = File.OpenRead(fileName))
				{
					return LoadDocDataFromStream(fileName, isReload, fileStream);
				}
			}
		}
		/// <summary>
		/// Load a file
		/// </summary>
		protected override void Load(string fileName, bool isReload)
		{
			if ((object)fileName == null)
			{
				return;
			}

			this.AddPreLoadModelingEventHandlers();

			Debug.Assert(myFileStream != null);
			Stream stream = myFileStream;
			Store store = this.Store;
			
			Debug.Assert(base.InLoad);
			store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[NamedElementDictionary.DefaultAllowDuplicateNamesKey] = null;

			if (stream.Length > 1)
			{
				DeserializationFixupManager fixupManager = new DeserializationFixupManager(DeserializationFixupPhaseType, store);
				foreach (IDeserializationFixupListener listener in DeserializationFixupListeners)
				{
					fixupManager.AddListener(listener);
				}
				(new ORMSerializer(store)).Load(stream, fixupManager);
			}

			foreach (ORMDiagram diagram in store.ElementDirectory.FindElements<ORMDiagram>(true))
			{
				if (diagram.AutoPopulateShapes)
				{
					diagram.AutoPopulateShapes = false;
					ORMDesignerDocView.AutoLayoutDiagram(diagram, diagram.NestedChildShapes);
				}
			}
		}
		/// <summary>
		/// Saves the model in Store format
		/// </summary>
		/// <param name="fileName"></param>
		protected override void Save(string fileName)
		{
			// UNDONE: 2006-06 DSL Tools port: Synchronize() method doesn't appear to exist any more.
			// sync the model to any artifacts.
			//Synchronize();

			// Save it out.
			using (FileStream fileStream = File.Create(fileName))
			{
				(new ORMSerializer(Store)).Save(fileStream);
			}

			if (GetFlag(PrivateFlags.SaveDisabled))
			{
				SetFlag(PrivateFlags.SaveDisabled, false);
				// An imported file does not have the Save menu enabled. We should
				// turn it on after a successful SaveAs
				IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
				docTable.ModifyDocumentFlags(Cookie, (uint)_VSRDTFLAGS.RDT_DontSave, 0);
			}
		}
		/// <summary>
		/// Override the default implementation, which attempts
		/// to set the fileName to a path, which doesn't exist.
		/// UNDONE: MSBUG, FDBK32824, we shouldn't need to do this
		/// </summary>
		/// <param name="pszDocDataPath">Ignored per SDK directions</param>
		/// <returns>S_OK</returns>
		public override int SetUntitledDocPath(string pszDocDataPath)
		{
			return VSConstants.S_OK;
		}
		/// <summary>
		/// Called to populate the Filter field in the Save As... dialog.
		/// </summary>
		protected override string FormatList
		{
			get
			{
				// UNDONE: Localize this.
				return "ORM Diagram (*.orm)\n*.orm\n";
			}
		}
		/// <summary>
		/// Defer event handling to the loaded models
		/// </summary>
		protected virtual void AddPreLoadModelingEventHandlers()
		{
			Store store = Store;
			SafeEventManager eventManager = ((ISafeEventManagerProvider)store).SafeEventManager;
			foreach (DomainModel domainModel in store.DomainModels)
			{
				IORMModelEventSubscriber subscriber = domainModel as IORMModelEventSubscriber;
				if (subscriber != null)
				{
					subscriber.ManagePreLoadModelingEventHandlers(eventManager, true);
				}
			}
			SetFlag(PrivateFlags.AddedPreLoadEvents, true);
		}
		/// <summary>
		/// Attach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected virtual void AddPostLoadModelingEventHandlers()
		{
			Store store = Store;
			SafeEventManager eventManager = ((ISafeEventManagerProvider)store).SafeEventManager;
			foreach (DomainModel domainModel in store.DomainModels)
			{
				IORMModelEventSubscriber subscriber = domainModel as IORMModelEventSubscriber;
				if (subscriber != null)
				{
					subscriber.ManagePostLoadModelingEventHandlers(eventManager, true);
				}
			}
			ManageErrorReportingEvents(eventManager, true);
			ManageTabRestoreEvents(eventManager, true);
			SetFlag(PrivateFlags.AddedPostLoadEvents, true);
		}
		/// <summary>
		/// Detach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected virtual void RemoveModelingEventHandlers()
		{
			bool addedPreLoad = GetFlag(PrivateFlags.AddedPreLoadEvents);
			bool addedPostLoad = GetFlag(PrivateFlags.AddedPostLoadEvents);
			bool addedSurveyQuestion = GetFlag(PrivateFlags.AddedSurveyQuestionEvents);
			SetFlag(PrivateFlags.AddedPreLoadEvents | PrivateFlags.AddedPostLoadEvents | PrivateFlags.AddedSurveyQuestionEvents, false);
			if (!addedPreLoad && !addedPostLoad && !addedSurveyQuestion)
			{
				return;
			}
			Store store = Store;
			SafeEventManager eventManager = ((ISafeEventManagerProvider)store).SafeEventManager;
			foreach (DomainModel domainModel in store.DomainModels)
			{
				IORMModelEventSubscriber subscriber = domainModel as IORMModelEventSubscriber;
				if (subscriber != null)
				{
					if (addedPreLoad)
					{
						subscriber.ManagePreLoadModelingEventHandlers(eventManager, false);
					}
					if (addedPostLoad)
					{
						subscriber.ManagePostLoadModelingEventHandlers(eventManager, false);
					}
					if (addedSurveyQuestion)
					{
						subscriber.ManageSurveyQuestionModelingEventHandlers(eventManager, false);
					}
				}
			}
			if (addedPostLoad)
			{
				ManageTabRestoreEvents(eventManager, false);
				ManageErrorReportingEvents(eventManager, false);
			}
		}
		/// <summary>
		/// Clear out the task provider
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (myTaskProvider != null)
				{
					myTaskProvider.RemoveAllTasks();
					myTaskProvider = null;
				}
				this.RemoveModelingEventHandlers();
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		/// <summary>
		/// Support the default/only (GUID_NULL) view
		/// </summary>
		/// <param name="logicalView">A view guid to test</param>
		/// <returns>true for an empty guid</returns>
		public override bool SupportsLogicalView(Guid logicalView)
		{
			if (logicalView == Guid.Empty)
			{
				return true;
			}
			return base.SupportsLogicalView(logicalView);
		}
		#endregion // Base overrides
		#region Error reporting
		private void ManageErrorReportingEvents(SafeEventManager eventManager, bool addHandlers)
		{
			Store store = Store;
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ModelHasError.DomainClassId);

			eventManager.AddOrRemove(classInfo, new EventHandler<ElementAddedEventArgs>(ErrorAddedEvent), addHandlers);

			classInfo = dataDirectory.FindDomainClass(ModelError.DomainClassId);
			eventManager.AddOrRemove(classInfo, new EventHandler<ElementDeletedEventArgs>(ErrorRemovedEvent), addHandlers);
			eventManager.AddOrRemove(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorChangedEvent), addHandlers);
		}
		private void ErrorAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ModelError.AddToTaskProvider(e.ModelElement as ModelHasError);
		}
		private void ErrorRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelError error = e.ModelElement as ModelError;
			IORMToolTaskItem taskData = error.TaskData as IORMToolTaskItem;
			if (taskData != null)
			{
				error.TaskData = null;
				(this as IORMToolServices).TaskProvider.RemoveTask(taskData);
			}
		}
		private void ErrorChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ModelError error = e.ModelElement as ModelError;
			IORMToolTaskItem taskData = error.TaskData as IORMToolTaskItem;
			if (taskData != null)
			{
				taskData.Text = error.Name;
			}
		}
		#endregion // Error reporting
		#region Tab Restoration Hack
		private void ManageTabRestoreEvents(SafeEventManager eventManager, bool addHandlers)
		{
			// Add event handlers to cater for undo/redo and initial
			// add when multiple windows are open
			Store store = Store;
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainClass(Diagram.DomainClassId);
			eventManager.AddOrRemove(classInfo, new EventHandler<ElementAddedEventArgs>(AddOrRemoveTabForEvent), addHandlers);
		}
		private void AddOrRemoveTabForEvent(object sender, ElementAddedEventArgs e)
		{
			Diagram diagram = e.ModelElement as Diagram;
			Store store = diagram.Store;
			if (diagram.Partition != store.DefaultPartition)
			{
				return;
			}
			IMonitorSelectionService monitorSelection = (IMonitorSelectionService)ServiceProvider.GetService(typeof(IMonitorSelectionService));
			MultiDiagramDocView activeView = (monitorSelection != null) ? (monitorSelection.CurrentDocumentView as MultiDiagramDocView) : null;
			foreach (ModelingDocView view in DocViews)
			{
				MultiDiagramDocView multiDocView = view as MultiDiagramDocView;
				if (multiDocView != null)
				{
					// Activate the tab only if this is the active window.
					multiDocView.AddDiagram(diagram, multiDocView == activeView);
				}
			}
		}
		#endregion // Tab Restoration Hack
		#region ORMDesignerDocData specific
		/// <summary>
		/// Retrieve the phase enum to use with the
		/// deserialization manager.
		/// </summary>
		protected virtual Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ORMDeserializationFixupPhase);
			}
		}
		/// <summary>
		/// Return a set of listeners for deserialization fixup
		/// </summary>
		protected virtual IEnumerable<IDeserializationFixupListener> DeserializationFixupListeners
		{
			get
			{
				foreach (DomainModel domainModel in Store.DomainModels)
				{
					IDeserializationFixupListenerProvider provider = domainModel as IDeserializationFixupListenerProvider;
					if (provider != null)
					{
						foreach (IDeserializationFixupListener listener in provider.DeserializationFixupListenerCollection)
						{
							yield return listener;
						}
					}
				}
			}
		}
		#endregion // ORMDesignerDocData specific
		#region Automation support
		/// <summary>
		/// Implements IExtensibleObject.GetAutomationObject. Returns the ORM XML stream for
		/// the "ORMXmlStream" object name and the this object for everything else.
		/// </summary>
		protected void GetAutomationObject(string name, IExtensibleObjectSite parent, out object result)
		{
			if ("ORMXmlStream" == name)
			{
				MemoryStream stream = new MemoryStream();
				(new ORMSerializer(Store)).Save(stream);
				stream.Position = 0;
				result = stream;
				return;
			}
			result = this;
		}
		void IExtensibleObject.GetAutomationObject(string Name, IExtensibleObjectSite pParent, out object ppDisp)
		{
			GetAutomationObject(Name, pParent, out ppDisp);
		}
		#endregion // Automation support
	}
	#endregion // ORMDesignerDocData class
}
