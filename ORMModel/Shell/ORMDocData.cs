using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.ArtifactMapper;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Framework;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Xml;

#if ATTACHELEMENTPROVIDERS
using Neumont.Tools.ORM.DocumentSynchronization;
#endif // ATTACHELEMENTPROVIDERS
namespace Neumont.Tools.ORM.Shell
{
	#region ORMDesignerDocData class
	/// <summary>
	/// DocData object for the ORM Designer editor
	/// </summary>
	public partial class ORMDesignerDocData : ModelingDocData, IExtensibleObject
	{
		#region Private flags
		[Flags]
		private enum PrivateFlags
		{
			None = 0,
			AddedPostLoadEvents = 1,
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
		private IDictionary<string, Type> myExtensionSubStores;
		private static readonly IDictionary<string, Type> myStandardSubStores;
		#endregion // Member variables
		#region Construction/destruction
		/// <summary>
		/// Standard DocData constructor, called by the editor factory
		/// </summary>
		/// <param name="serviceProvider">IServiceProvider</param>
		/// <param name="editorFactory">EditorFactory</param>
		public ORMDesignerDocData(IServiceProvider serviceProvider, EditorFactory editorFactory) : base(serviceProvider, editorFactory)
		{
		}
		/// <summary>
		/// DocData constructor used to create the standard substores needed for the tool.
		/// </summary>
		static ORMDesignerDocData()
		{
			Dictionary<string, Type> standardSubStores = new Dictionary<string, Type>();
			standardSubStores.Add(ORMMetaModel.XmlNamespace, typeof(ORMMetaModel));
			standardSubStores.Add(ORMShapeModel.XmlNamespace, typeof(ORMShapeModel));
			myStandardSubStores = standardSubStores;
		}
		#endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// Return array of types of the substores used by the designer
		/// </summary>
		/// <returns></returns>
		protected override System.Type[] GetSubStores(object storeKey)
		{
			if (storeKey == PrimaryStoreKey)
			{
				// Always have 1 for the CoreDesignSurface. Note that the framework automatically
				// loads the core model (ModelElement, ElementLink, etc).
				int knownCount = 1 + myStandardSubStores.Count;
				int count = knownCount;
				IDictionary<string, Type> extensionSubstores = myExtensionSubStores;
				if (extensionSubstores != null)
				{
					count += extensionSubstores.Count;
				}
				Type[] retVal = new Type[count];
				retVal[0] = typeof(CoreDesignSurface);
				myStandardSubStores.Values.CopyTo(retVal, 1);
				if (extensionSubstores != null)
				{
					extensionSubstores.Values.CopyTo(retVal, knownCount);
				}
				return retVal;
			}						
			return null;			
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
			// Convert early so we can accurately check extension elements
			int retVal = 0;
			bool dontSave = false;
			using(FileStream fileStream = File.OpenRead(fileName))
			{
				ORMDesignerSettings settings = ORMDesignerPackage.DesignerSettings;
				using(Stream convertedStream = settings.ConvertStream(fileStream))
				{
					dontSave = convertedStream != null;
					Stream stream = dontSave ? convertedStream : fileStream;
					myFileStream = stream;
					try
					{
						XmlReaderSettings readerSettings = new XmlReaderSettings();
						readerSettings.CloseInput = false;
						Dictionary<string, Type> documentExtensions = null;
						using(XmlReader reader = XmlReader.Create(stream, readerSettings))
						{
							reader.MoveToContent();
							if(reader.NodeType == XmlNodeType.Element)
							{
								if(reader.MoveToFirstAttribute())
								{
									do
									{
										if(reader.Prefix == "xmlns")
										{
											string URI = reader.Value;
											if (!string.Equals(URI, ORMMetaModel.XmlNamespace, StringComparison.Ordinal) &&
												!string.Equals(URI, ORMShapeModel.XmlNamespace, StringComparison.Ordinal) &&
												!string.Equals(URI, ORMSerializer.RootXmlNamespace, StringComparison.Ordinal))
											{
												Type extensionType = ORMDesignerPackage.GetExtensionSubStore(URI);
												if (extensionType != null)
												{
													if (documentExtensions == null)
													{
														documentExtensions = new Dictionary<string, Type>();
													}
													documentExtensions.Add(URI, extensionType);
												}
											}
										}
									} while(reader.MoveToNextAttribute());
								}
							}
						}
						myExtensionSubStores = documentExtensions;
						stream.Position = 0;
						retVal = base.LoadDocData(fileName, isReload);
					}
					finally
					{
						myFileStream = null;
					}
				}
			}
			if (dontSave)
			{
				IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
				docTable.ModifyDocumentFlags(Cookie, (uint)_VSRDTFLAGS.RDT_DontSave, 1);
			}
			return retVal;
		}
		/// <summary>
		/// Load a file
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="isReload"></param>
		protected override void Load(string fileName, bool isReload)
		{
			if(fileName == null)
			{
				return;
			}

			Debug.Assert(myFileStream != null);
			Stream stream = myFileStream;
			Store store = this.Store;

			if(stream.Length > 1)
			{
				DeserializationFixupManager fixupManager = new DeserializationFixupManager(DeserializationFixupPhaseType, store);
				foreach(IDeserializationFixupListener listener in DeserializationFixupListeners)
				{
					fixupManager.AddListener(listener);
				}
				(new ORMSerializer(store)).Load(stream, fixupManager);
			}

			foreach (ORMDiagram diagram in store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid, true))
			{
				if (diagram.AutoPopulateShapes)
				{
					diagram.AutoPopulateShapes = false;
					// TODO: We could perform an auto-layout here as well...
				}
			}
		}
		/// <summary>
		/// Saves the model in Store format
		/// </summary>
		/// <param name="fileName"></param>
		protected override void Save(string fileName)
		{
			// sync the model to any artifacts.
			Synchronize();

			// Save it out.
#if OLDSERIALIZE
			using (FileStream fileStream = File.Create(fileName + '1'))
			{
				(new ORMSerializer(Store)).Save1(fileStream);
			}
#endif // OLDSERIALIZE
			using (FileStream fileStream = File.Create(fileName))
			{
				(new ORMSerializer(Store)).Save(fileStream);
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
				string formatList = "ORM Diagram (*.orm)|*.orm|";
				return formatList.Replace("|", "\n");
			}
		}
#if ATTACHELEMENTPROVIDERS
		/// <summary>
		/// UNDONE: Attach element providers
		/// </summary>
		/// <param name="store">The store being loaded</param>
		/// <param name="storeKey">The key for the store in the docdata. Handles PrimaryStoreKey.</param>
		/// <returns></returns>
		protected override ElementProvider[] GetElementProviders(Store store, object storeKey)
		{
			if (storeKey == PrimaryStoreKey)
			{
				//return new ElementProvider[] { new ORMElementProvider(store) };
			}
			return base.GetElementProviders(store, storeKey);
		}
		/// <summary>
		/// Continually synchronize the primary store with the element provider
		/// </summary>
		/// <param name="storeKey">The store key in the docdata. Handles PrimaryStoreKey.</param>
		public override bool GetContinuousSynchronization(object storeKey)
		{
			if (storeKey == PrimaryStoreKey)
			{
				return true;
			}
			return base.GetContinuousSynchronization(storeKey);
		}
#endif // ATTACHELEMENTPROVIDERS
		/// <summary>
		/// Set the document scope to ProjectScope for the element provider mechanism
		/// </summary>
		protected override IArtifactScope DocumentScope
		{
			get { return this.ProjectScope; }
		}
		/// <summary>
		/// Attach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected override void AddPostLoadModelingEventHandlers()
		{
			Store store = Store;
			AddErrorReportingEvents();
			AddTabRestoreEvents();
			NamedElementDictionary.AttachEventHandlers(store);
			ORMDiagram.AttachEventHandlers(store);
			SetFlag(PrivateFlags.AddedPostLoadEvents, true);
		}
		/// <summary>
		/// Detach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected override void RemoveModelingEventHandlers()
		{
			if (!GetFlag(PrivateFlags.AddedPostLoadEvents))
			{
				return;
			}
			SetFlag(PrivateFlags.AddedPostLoadEvents, false);
			Store store = Store;
			NamedElementDictionary.DetachEventHandlers(store);
			ORMDiagram.DetachEventHandlers(store);
			RemoveTabRestoreEvents();
			RemoveErrorReportingEvents();
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
		private void AddErrorReportingEvents()
		{
			Store store = Store;
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ModelHasError.MetaRelationshipGuid);

			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ErrorAddedEvent));

			classInfo = dataDirectory.FindMetaClass(ModelError.MetaClassGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ErrorRemovedEvent));
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ErrorChangedEvent));
		}
		private void RemoveErrorReportingEvents()
		{
			Store store = Store;
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ModelHasError.MetaRelationshipGuid);

			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ErrorAddedEvent));

			classInfo = dataDirectory.FindMetaClass(ModelError.MetaClassGuid);
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(ErrorRemovedEvent));
			eventDirectory.ElementAttributeChanged.Remove(classInfo, new ElementAttributeChangedEventHandler(ErrorChangedEvent));
		}
		private void ErrorAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ModelError.AddToTaskProvider(e.ModelElement as ModelHasError);
		}
		private void ErrorRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ModelError error = e.ModelElement as ModelError;
			IORMToolTaskItem taskData = error.TaskData as IORMToolTaskItem;
			if (taskData != null)
			{
				error.TaskData = null;
				(this as IORMToolServices).TaskProvider.RemoveTask(taskData);
			}
		}
		private void ErrorChangedEvent(object sender, ElementAttributeChangedEventArgs e)
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
		private void AddTabRestoreEvents()
		{
			//Added events to cater for undo/redo
			Store store = Store;
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaClass(Diagram.MetaClassGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(DiagramAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(DiagramRemovedEvent));
		}
		private void RemoveTabRestoreEvents()
		{
			//Added events to cater for undo/redo
			Store store = Store;
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaClass(Diagram.MetaClassGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(DiagramAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(DiagramRemovedEvent));
		}
		private void DiagramAddedEvent(object sender, ElementAddedEventArgs e)
		{
			AddOrRemoveTabForEvent(e.ModelElement as Diagram, true);
		}
		private void DiagramRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			AddOrRemoveTabForEvent(e.ModelElement as Diagram, false);
		}
		private void AddOrRemoveTabForEvent(Diagram diagram, bool add)
		{
			Store store = diagram.Store;
			if (store.InUndo || store.InRedo)
			{
				foreach (DocView view in DocViews)
				{
					TabbedDiagramDocView tabbedDocView = view as TabbedDiagramDocView;
					if (tabbedDocView != null)
					{
						if (add)
						{
							tabbedDocView.Diagrams.Add(diagram, true);
						}
						else
						{
							tabbedDocView.Diagrams.Remove(diagram);
						}
					}
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
				foreach (object subStore in Store.SubStores.Values)
				{
					IDeserializationFixupListenerProvider provider = subStore as IDeserializationFixupListenerProvider;
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
		/// Implements IExtensibleObject.GetAutomationObject. Returns the ORM2 stream for
		/// the "ORM2Stream" object name and the this object for everything else.
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
