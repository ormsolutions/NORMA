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
				return ORMDesignerPackage.GetGlobalSubStores();
			}
			return null;
		}
		/// <summary>
		/// Load a file
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="isReload"></param>
		protected override void Load(string fileName, bool isReload)
		{
			if (fileName == null)
				return;

			Store store = this.Store;
			bool dontSave = false;
			if (fileName.EndsWith(@"\default.orm", true, CultureInfo.CurrentCulture))
			{
				#region Generate Test Object Model
				DeserializationFixupManager fixupManager = new DeserializationFixupManager(DeserializationFixupPhaseType, store);
				foreach (IDeserializationFixupListener listener in DeserializationFixupListeners)
				{
					fixupManager.AddListener(listener);
				}
				ORMModel model = ORMModel.CreateORMModel(store);
				model.Name = "Model1";
				fixupManager.DeserializationComplete();

				// Create a ValueType
				ObjectType valType = ObjectType.CreateObjectType(store);
				valType.Name = "ValueType1";
				valType.Model = model;
				valType.DataType = model.DefaultDataType;

				// Create an EntityType
				ObjectType entType = ObjectType.CreateObjectType(store);
				entType.Name = "EntityType1";
				entType.Model = model;

				// Create a FactType
				FactType fact = FactType.CreateFactType(store);
				fact.Name = "Fact1";
				fact.Model = model; // Also do after roles are added to test shape generation
				RoleMoveableCollection roles = fact.RoleCollection;
				Role role = Role.CreateRole(store);
				role.Name = "Role1";
				role.RolePlayer = valType;
				roles.Add(role);

				role = Role.CreateRole(store);
				role.Name = "Role2";
				role.RolePlayer = entType;
				roles.Add(role);

				ReadingOrder readOrd = ReadingOrder.CreateReadingOrder(store);
				readOrd.RoleCollection.Add(fact.RoleCollection[0]);
				readOrd.RoleCollection.Add(fact.RoleCollection[1]);

				fact.ReadingOrderCollection.Add(readOrd);

				Reading read = Reading.CreateReading(store);
				readOrd.ReadingCollection.Add(read);
				read.Text = "{0} has {1}";

				read = Reading.CreateReading(store);
				readOrd.ReadingCollection.Add(read);
				read.Text = "{0} owns {1}";

				read = Reading.CreateReading(store);
				readOrd.ReadingCollection.Add(read);
				read.Text = "{0} possesses {1}";

				readOrd = ReadingOrder.CreateReadingOrder(store);
				readOrd.RoleCollection.Add(fact.RoleCollection[1]);
				readOrd.RoleCollection.Add(fact.RoleCollection[0]);
				fact.ReadingOrderCollection.Add(readOrd);

				read = Reading.CreateReading(store);
				readOrd.ReadingCollection.Add(read);
				read.Text = "{0} is of {1}";

				InternalConstraint ic = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
				ic.RoleCollection.Add(fact.RoleCollection[0]); // Automatically sets FactType, setting it again will remove and delete the new constraint

				// Create an objectified fact type with one role
				FactType nestedFact = FactType.CreateFactType(store);
				nestedFact.Name = "ObjectifiedFact1";
				nestedFact.Model = model; // Also do after roles are added to test shape generation

				ObjectType nestingType = ObjectType.CreateObjectType(store);
				nestingType.Name = "ObjectifyFact1";
				nestingType.Model = model;
				roles = nestedFact.RoleCollection;
				role = Role.CreateRole(store);
				role.Name = "Role1";
				roles.Add(role);

				nestingType.NestedFactType = nestedFact;
				role.RolePlayer = entType;

				//ExternalUniquenessConstraint euc = ExternalUniquenessConstraint.CreateExternalUniquenessConstraint(store);
				ExclusionConstraint euc = ExclusionConstraint.CreateExclusionConstraint(store);
				euc.Name = "Constraint1";
				euc.Model = model;
				MultiColumnExternalConstraintRoleSequenceMoveableCollection roleSequences = euc.RoleSequenceCollection;
				MultiColumnExternalConstraintRoleSequence roleSequence = MultiColumnExternalConstraintRoleSequence.CreateMultiColumnExternalConstraintRoleSequence(store);
				roleSequence.RoleCollection.Add(fact.RoleCollection[1]);
				roleSequences.Add(roleSequence);
				roleSequence = MultiColumnExternalConstraintRoleSequence.CreateMultiColumnExternalConstraintRoleSequence(store);
				roleSequences.Add(roleSequence);
				roleSequence.RoleCollection.Add(nestedFact.RoleCollection[0]);

				// UNDONE: Need to verify that this ordering is handled as well
				//fact.Model = model; // Done earlier to test shape generation
				#endregion // Generate Test Object Model
			}
			else
			{
				Synchronize();
				using (FileStream fileStream = File.OpenRead(fileName))
				{
					ORMDesignerSettings settings = ORMDesignerPackage.DesignerSettings;
					using (Stream convertedStream = settings.ConvertStream(fileStream))
					{
						Stream stream = (convertedStream != null) ? convertedStream : fileStream;
						dontSave = convertedStream != null;
						if (stream.Length > 1)
						{
							DeserializationFixupManager fixupManager = new DeserializationFixupManager(DeserializationFixupPhaseType, store);
							foreach (IDeserializationFixupListener listener in DeserializationFixupListeners)
							{
								fixupManager.AddListener(listener);
							}
							(new ORMSerializer(store)).Load(stream, fixupManager);
						}
					}
				}
			}

			this.SetFileName(fileName);

			if (dontSave)
			{
				IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
				docTable.ModifyDocumentFlags(Cookie, (uint)_VSRDTFLAGS.RDT_DontSave, 1);
			}


			foreach (ORMDiagram diagram in this.Store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid, true))
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
			if (fileName == null || fileName.EndsWith(@"\default.orm", true, CultureInfo.CurrentCulture))
				return;

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
			NamedElementDictionary.AttachEventHandlers(store);
			ORMDiagram.AttachEventHandlers(store);
			SetFlag(PrivateFlags.AddedPostLoadEvents, true);
		}
		/// <summary>s
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
