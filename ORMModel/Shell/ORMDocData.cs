#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Win32;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.Shell
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
			ErrorDisplayModified = 0x10,
			UndoStackRemoved = 0x20,
			RethrowLoadDocDataException = 0x40,
			IgnoreDocumentReloading = 0x80,
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
		private IDictionary<string, ExtensionModelBinding> myExtensionDomainModels;
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
			ICollection<Type> standardDomainModels = ORMDesignerPackage.ExtensionLoader.StandardDomainModels;
			int standardCount = standardDomainModels.Count;
			int count = standardCount;
			IDictionary<string, ExtensionModelBinding> extensionDomainModels = myExtensionDomainModels;
			if (extensionDomainModels != null)
			{
				count += extensionDomainModels.Count;
			}
			Type[] retVal = new Type[count];
			standardDomainModels.CopyTo(retVal, 0);
			if (extensionDomainModels != null)
			{
				int i = standardCount - 1;
				foreach (ExtensionModelBinding extensionType in extensionDomainModels.Values)
				{
					retVal[++i] = extensionType.Type;
				}
			}
			// Add a fixed sort order for all machines. The order here is based on
			// attribute sets, which are unordered, and other factors. The result is
			// that you get different orders here on different machines and builds,
			// which causes large differences in the serialized file, breaks unit
			// test baselines, etc.
			Array.Sort<Type>(
				retVal,
				delegate(Type x, Type y)
				{
					return x.FullName.CompareTo(y.FullName);
				});
			return retVal;
		}
		/// <summary>
		/// Reload this document from a <see cref="Stream"/> instead of from a file.
		/// </summary>
		/// <param name="newStream">The <see cref="Stream"/> to load</param>
		/// <param name="fallbackStream">If <paramref name="newStream"/> fails to load, then
		/// reload this stream instead.</param>
		public void ReloadFromStream(Stream newStream, Stream fallbackStream)
		{
			myFileStream = newStream;
			// This calls into LoadDocData(string, bool) after doing necessary cleanup
			IServiceProvider serviceProvider;
			if (fallbackStream == null)
			{
				ReloadDocData((uint)_VSRELOADDOCDATA.RDD_RemoveUndoStack);
			}
			else
			{
				SetFlag(PrivateFlags.RethrowLoadDocDataException, true);
				try
				{
					ReloadDocData((uint)_VSRELOADDOCDATA.RDD_RemoveUndoStack);
				}
				catch (Exception ex)
				{
					SetFlag(PrivateFlags.RethrowLoadDocDataException, false);
					SetFlag(PrivateFlags.IgnoreDocumentReloading, true);
					fallbackStream.Position = 0;
					myFileStream = fallbackStream;
					ReloadDocData((uint)_VSRELOADDOCDATA.RDD_RemoveUndoStack);
					if (null != (serviceProvider = ServiceProvider))
					{
						StringBuilder builder = new StringBuilder(ResourceStrings.RevertExtensionsMessage);
						const string offset = "\r\n";
						Exception messageException = ex;
						while (messageException != null)
						{
							string message = messageException.Message;
							if (!string.IsNullOrEmpty(message))
							{
								builder.Append(offset);
								builder.Append(message);
							}
							messageException = messageException.InnerException;
						}

						VsShellUtilities.ShowMessageBox(
							serviceProvider,
							builder.ToString(),
							ResourceStrings.PackageOfficialName,
							OLEMSGICON.OLEMSGICON_INFO,
							OLEMSGBUTTON.OLEMSGBUTTON_OK,
							OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
					}
				}
				finally
				{
					SetFlag(PrivateFlags.RethrowLoadDocDataException, false);
					SetFlag(PrivateFlags.IgnoreDocumentReloading, false);
				}
			}

			// The document now has no undo stack, but we need it to display it as dirty
			SetFlag(PrivateFlags.UndoStackRemoved, true);
			uint cookie;
			IVsUIShell shell;
			if (null != (serviceProvider = base.ServiceProvider) &&
				0 != (cookie = base.Cookie) &&
				null != (shell = serviceProvider.GetService(typeof(IVsUIShell)) as IVsUIShell))
			{
				shell.UpdateDocDataIsDirtyFeedback(cookie, 1);
			}
		}
		/// <summary>
		/// Enable reloading of the original stream during a failed attempt
		/// to modify extensions.
		/// </summary>
		protected override void OnDocumentReloading(EventArgs e)
		{
			if (!GetFlag(PrivateFlags.IgnoreDocumentReloading))
			{
				base.OnDocumentReloading(e);
			}
		}
		/// <summary>
		/// Enable reloading of the original stream during a failed attempt
		/// to modify extensions.
		/// </summary>
		protected override void HandleLoadDocDataException(string fileName, Exception exception, bool isReload)
		{
			if (GetFlag(PrivateFlags.RethrowLoadDocDataException))
			{
				throw exception;
			}
			base.HandleLoadDocDataException(fileName, exception, isReload);
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
				// Null out the myPropertyProviderService and myTypedDomainModelProviderCache fields
				// so that a new instance will be created with the new Store next time it is needed
				this.myPropertyProviderService = null;
				this.myTypedDomainModelProviderCache = null;

				this.RemoveModelingEventHandlers(isReload);

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

				// Remove the cached verbalization snippets and targets, this set will change
				myTargetedVerbalizationSnippets = null;
				myVerbalizationTargets = null;
				myExtensionVerbalizerService = null;
			}
			// Convert early so we can accurately check extension elements
			int retVal = 0;
			bool dontSave = false;
			List<string> unrecognizedNamespaces = null;
			ORMDesignerSettings settings = ORMDesignerPackage.DesignerSettings;
			using (Stream convertedStream = settings.ConvertStream(inputStream, ServiceProvider))
			{
				dontSave = convertedStream != null;
				Stream stream = dontSave ? convertedStream : inputStream;
				myFileStream = stream;
				Stream namespaceStrippedStream = null;
				try
				{
					XmlReaderSettings readerSettings = new XmlReaderSettings();
					ExtensionLoader extensionLoader = ORMDesignerPackage.ExtensionLoader;
					readerSettings.CloseInput = false;
					Dictionary<string, ExtensionModelBinding> documentExtensions = null;
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
											!string.Equals(URI, ORMSerializationEngine.RootXmlNamespace, StringComparison.Ordinal))
										{
											ExtensionModelBinding? extensionType = extensionLoader.GetExtensionDomainModel(URI);
											if (extensionType.HasValue)
											{
												if (documentExtensions == null)
												{
													documentExtensions = new Dictionary<string, ExtensionModelBinding>();
												}
												documentExtensions[URI] = extensionType.Value;
											}
											else
											{
												(unrecognizedNamespaces ?? (unrecognizedNamespaces = new List<string>())).Add(URI);
											}
										}
									}
								} while (reader.MoveToNextAttribute());
							}
						}
					}
					extensionLoader.VerifyRequiredExtensions(ref documentExtensions);
					Stream unstrippedNamespaceStream = stream;
					if (unrecognizedNamespaces != null)
					{
						stream.Position = 0;
						namespaceStrippedStream = ExtensionLoader.CleanupStream(stream, extensionLoader.StandardDomainModels, documentExtensions.Values, unrecognizedNamespaces);
						if (namespaceStrippedStream != null)
						{
							dontSave = true;
							stream = namespaceStrippedStream;
							myFileStream = namespaceStrippedStream;
						}
						else
						{
							unrecognizedNamespaces = null;
						}
					}
					myExtensionDomainModels = documentExtensions;
					stream.Position = 0;

					try
					{
						 retVal = base.LoadDocData(fileName, isReload);
					}
					catch (TypeInitializationException ex)
					{
						// If the type that failed to load is an extensions, then remove it from
						// the list of available extensions and try again.
						if (documentExtensions != null)
						{
							string typeName = ex.TypeName;
							foreach (KeyValuePair<string, ExtensionModelBinding> pair in documentExtensions)
							{
								Type testType = pair.Value.Type;
								if (testType.FullName == typeName)
								{
									if (extensionLoader.CustomExtensionUnavailable(testType))
									{
										// If the unloadable type is a registered extensions, then
										// we now have an additional namespace element that will not
										// load correctly. Recurse on this function with the stream
										// before any extensions namespace were stripped so that we can
										// see all stripped namespaces in the final message. Obviously,
										// this repeats some processing we've already done, but this is
										// very much an exception case, and the additional minor performance
										// hit is minimal compared with the user annoyance at potentially
										// seeing multiple error displays.
										Exception innerException = ex.InnerException;
										string message;
										if (innerException != null &&
											!string.IsNullOrEmpty(message = innerException.Message))
										{
											VsShellUtilities.ShowMessageBox(
												ServiceProvider,
												message,
												ResourceStrings.PackageOfficialName,
												OLEMSGICON.OLEMSGICON_INFO,
												OLEMSGBUTTON.OLEMSGBUTTON_OK,
												OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
										}
										unstrippedNamespaceStream.Position = 0;
										return LoadDocDataFromStream(fileName, true, unstrippedNamespaceStream);
									}
									break;
								}
							}
						}
						throw;
					}

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
					if (namespaceStrippedStream != null)
					{
						namespaceStrippedStream.Dispose();
					}
				}
			}
			if (dontSave)
			{
				// If this is a new file then do not disable the save button
				IVsHierarchy hierarchy;
				uint itemId;
				object isNewObject;
				if (null != (hierarchy = this.Hierarchy) &&
					VSConstants.VSITEMID_NIL != (itemId = this.ItemId) &&
					ErrorHandler.Succeeded(hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_IsNewUnsavedItem, out isNewObject)) &&
					(bool)isNewObject)
				{
					dontSave = false;
				}
				if (dontSave)
				{
					string message;
					CultureInfo culture = CultureInfo.CurrentCulture;
					int unrecognizedCount;
					if (unrecognizedNamespaces != null &&
						0 != (unrecognizedCount = unrecognizedNamespaces.Count))
					{
						string namespaceReplacement = unrecognizedNamespaces[0];
						if (unrecognizedCount > 1)
						{
							string separator = culture.TextInfo.ListSeparator;
							if (!char.IsWhiteSpace(separator, separator.Length - 1))
							{
								separator += " ";
							}
							for (int i = 1; i < unrecognizedCount; ++i)
							{
								namespaceReplacement += separator + unrecognizedNamespaces[i];
							}
						}
						message = string.Format(culture, ResourceStrings.UnrecognizedExtensionsStrippedMessage, fileName, namespaceReplacement);
					}
					else
					{
						message = string.Format(culture, ResourceStrings.FileFormatUpgradeMessage, fileName);
					}
					// The disabled save is leading to data loss, prompt the user
					dontSave = (int)DialogResult.Yes == VsShellUtilities.ShowMessageBox(
						ServiceProvider,
						message,
						ResourceStrings.PackageOfficialName,
						OLEMSGICON.OLEMSGICON_QUERY,
						OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
						OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
					if (dontSave)
					{
						IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
						SetFlag(PrivateFlags.SaveDisabled, true);
						docTable.ModifyDocumentFlags(Cookie, (uint)_VSRDTFLAGS.RDT_DontSave, 1);
					}
				}
			}
			this.AddPostLoadModelingEventHandlers(isReload);
			SetFlag(PrivateFlags.UndoStackRemoved, false);
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

			this.AddPreLoadModelingEventHandlers(isReload);

			Debug.Assert(myFileStream != null);
			Stream stream = myFileStream;
			Store store = this.Store;

			Debug.Assert(base.InLoad);

			if (stream.Length > 1)
			{
				try
				{
					(new ORMSerializationEngine(store)).Load(stream);
				}
				catch (XmlSchemaValidationException ex)
				{
					throw new XmlSchemaValidationException(string.Format(CultureInfo.CurrentCulture, ResourceStrings.SchemaValidationFailureInstructions, ex.Message), ex);
				}
			}

			foreach (ORMDiagram diagram in store.ElementDirectory.FindElements<ORMDiagram>(true))
			{
				if (diagram.AutoPopulateShapes)
				{
					diagram.AutoPopulateShapes = false;
					ORMDesignerCommandManager.AutoLayoutDiagram(diagram, diagram.NestedChildShapes, true);
				}
			}

			// Go through each diagram type, look for the 'Required' setting on the DiagramMenuDisplay attribute, and
			// create a diagram if needed.
			IList<Diagram> existingDiagrams = store.ElementDirectory.FindElements<Diagram>(true);
			int existingDiagramCount = existingDiagrams.Count;
			ReadOnlyCollection<DomainClassInfo> possibleDiagramTypes = store.DomainDataDirectory.FindDomainClass(Diagram.DomainClassId).AllDescendants;
			int possibleDiagramTypeCount = possibleDiagramTypes.Count;
			for (int i = 0; i < possibleDiagramTypeCount; ++i)
			{
				DomainClassInfo diagramInfo = possibleDiagramTypes[i];
				Type testType = diagramInfo.ImplementationClass;
				if (!testType.IsAbstract)
				{
					object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), false);
					if (attributes.Length > 0)
					{
						DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
						if ((attribute.DiagramOption & DiagramMenuDisplayOptions.Required) != 0)
						{
							int j = 0;
							for (; j < existingDiagramCount; ++j)
							{
								if (existingDiagrams[j].GetType() == testType)
								{
									break;
								}
							}

							if (j == existingDiagramCount)
							{
								//A diagram does not exist for this required diagram type.
								//Create one.
								store.ElementFactory.CreateElement(diagramInfo);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Saves the model in Store format
		/// </summary>
		/// <param name="fileName"></param>
		protected override void Save(string fileName)
		{
			// Save it first to a memory stream, so that the user doesn't lose their original copy
			// if something goes wrong while serializing.
			using (MemoryStream memoryStream = new MemoryStream(1024 * 1024))
			{
				new ORMSerializationEngine(this.Store).Save(memoryStream);

				// UNDONE: We don't yet support ORM models greater than 2GB in size
				int memoryStreamLength = (int)memoryStream.Length;
				using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, memoryStreamLength, FileOptions.SequentialScan))
				{
					fileStream.SetLength(memoryStreamLength);
					fileStream.Write(memoryStream.GetBuffer(), 0, memoryStreamLength);
				}
			}

			SetFlag(PrivateFlags.UndoStackRemoved, false);
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
		protected virtual void AddPreLoadModelingEventHandlers(bool isReload)
		{
			Store store = Store;
			ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
			EventSubscriberReasons reasons = EventSubscriberReasons.DocumentLoading | EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.UserInterfaceEvents;
			if (isReload)
			{
				reasons |= EventSubscriberReasons.DocumentReloading;
			}
			foreach (IModelingEventSubscriber subscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(Store.DomainModels))
			{
				subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
			}
			foreach (ModelingDocView docView in DocViews)
			{
				IModelingEventSubscriber subscriber = docView as IModelingEventSubscriber;
				if (subscriber != null)
				{
					subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
				}
			}
			SetFlag(PrivateFlags.AddedPreLoadEvents, true);
		}
		/// <summary>
		/// Attach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected virtual void AddPostLoadModelingEventHandlers(bool isReload)
		{
			if (!isReload)
			{
				SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(CultureChangedEvent);
			}
			Store store = Store;
			ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
			EventSubscriberReasons reasons = EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.UserInterfaceEvents;
			if (isReload)
			{
				reasons |= EventSubscriberReasons.DocumentReloading;
			}
			foreach (IModelingEventSubscriber subscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(Store.DomainModels))
			{
				subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
			}
			foreach (ModelingDocView docView in DocViews)
			{
				IModelingEventSubscriber subscriber = docView as IModelingEventSubscriber;
				if (subscriber != null)
				{
					subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Add);
				}
			}
			ReloadSurveyTree(isReload);
			ManageErrorReportingEvents(eventManager, EventHandlerAction.Add);
			ManageTabRestoreEvents(eventManager, EventHandlerAction.Add);
			SetFlag(PrivateFlags.AddedPostLoadEvents, true);
		}
		/// <summary>
		/// Detach model events. Adds NamedElementDictionary handling
		/// to the document's primary store.
		/// </summary>
		protected virtual void RemoveModelingEventHandlers(bool isReload)
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
			ModelingEventManager eventManager = ModelingEventManager.GetModelingEventManager(store);
			EventSubscriberReasons reasons = EventSubscriberReasons.ModelStateEvents | EventSubscriberReasons.UserInterfaceEvents;
			if (isReload)
			{
				reasons |= EventSubscriberReasons.DocumentReloading;
			}
			if (addedPreLoad)
			{
				reasons |= EventSubscriberReasons.DocumentLoading;
			}
			if (addedPostLoad)
			{
				reasons |= EventSubscriberReasons.DocumentLoaded;
			}
			if (addedSurveyQuestion)
			{
				reasons |= EventSubscriberReasons.SurveyQuestionEvents;
			}
			foreach (IModelingEventSubscriber subscriber in Utility.EnumerateDomainModels<IModelingEventSubscriber>(Store.DomainModels))
			{
				subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Remove);
			}
			foreach (ModelingDocView docView in DocViews)
			{
				IModelingEventSubscriber subscriber = docView as IModelingEventSubscriber;
				if (subscriber != null)
				{
					subscriber.ManageModelingEventHandlers(eventManager, reasons, EventHandlerAction.Remove);
				}
			}
			UnloadSurveyTree();
			if (addedPostLoad)
			{
				ManageTabRestoreEvents(eventManager, EventHandlerAction.Remove);
				ManageErrorReportingEvents(eventManager, EventHandlerAction.Remove);
				if (!isReload)
				{
					SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(CultureChangedEvent);
				}
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
				this.RemoveModelingEventHandlers(false);
			}
			finally
			{
				if (ModelingDocStore != null)
				{
					// This crashes if the ModelingDocStore has not been established
					base.Dispose(disposing);
				}
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
		/// <summary>
		/// Mark files with extension changes as dirty
		/// </summary>
		public override int IsDocDataDirty(out int isDirty)
		{
			if (GetFlag(PrivateFlags.UndoStackRemoved))
			{
				isDirty = 1;
				return VSConstants.S_OK;
			}
			return base.IsDocDataDirty(out isDirty);
		}
		#endregion // Base overrides
		#region Error reporting
		private void ManageErrorReportingEvents(ModelingEventManager eventManager, EventHandlerAction action)
		{
			Store store = Store;
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ModelHasError.DomainClassId);

			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ErrorAddedEvent), action);

			classInfo = dataDirectory.FindDomainClass(ModelError.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ErrorRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorChangedEvent), action);

			classInfo = dataDirectory.FindDomainRelationship(ModelHasModelErrorDisplayFilter.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ErrorDisplayChangedEvent), action);

			classInfo = dataDirectory.FindDomainClass(ModelErrorDisplayFilter.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ErrorDisplayChangedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorDisplayChangedEvent), action);

			DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(ModelErrorDisplayFilter.ExcludedCategoriesDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorDisplayChangedEvent), action);
			propertyInfo = dataDirectory.FindDomainProperty(ModelErrorDisplayFilter.ExcludedErrorsDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorDisplayChangedEvent), action);
			propertyInfo = dataDirectory.FindDomainProperty(ModelErrorDisplayFilter.IncludedErrorsDomainPropertyId);
			eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ErrorDisplayChangedEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ErrorEventsEnded), action);
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
				taskData.Text = error.ErrorText;
			}
		}

		private void ErrorDisplayChangedEvent(object sender, ElementEventArgs e)
		{
			SetFlag(PrivateFlags.ErrorDisplayModified, true);
		}
		private void ErrorEventsEnded(object sender, ElementEventsEndedEventArgs e)
		{
			if (GetFlag(PrivateFlags.ErrorDisplayModified))
			{
				SetFlag(PrivateFlags.ErrorDisplayModified, false);

				IORMToolServices toolServices = this as IORMToolServices;
				IORMToolTaskProvider taskProvider = toolServices.TaskProvider;

				//refresh diagrams
				ORMDesignerDocView.InvalidateAllDiagrams(toolServices.ServiceProvider, this);

				//refresh task list and model browser
				SurveyTree<Store> surveyTree = mySurveyTree;
				foreach (ORMModel model in this.Store.ElementDirectory.FindElements<ORMModel>(true))
				{
					ModelErrorDisplayFilter filter = model.ModelErrorDisplayFilter;

					foreach (ModelError error in model.ErrorCollection)
					{
						object taskData = error.TaskData;
						if (filter == null || filter.ShouldDisplay(error))
						{
							if (taskData == null)
							{
								ModelError.AddToTaskProvider(error);
							}
						}
						else if (taskData != null)
						{
							error.TaskData = null;
							IORMToolTaskItem taskItem = taskData as IORMToolTaskItem;
							if (taskItem != null)
							{
								taskProvider.RemoveTask(taskItem);
							}
						}
						if (surveyTree != null)
						{
							error.WalkAssociatedElements(delegate(ModelElement associatedElement)
							{
								surveyTree.UpdateErrorDisplay(associatedElement);
							});
						}
					}
					break;
				}
			}
		}
		#endregion // Error reporting
		#region Tab Restoration Hack
		private void ManageTabRestoreEvents(ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Add event handlers to cater for undo/redo and initial
			// add when multiple windows are open
			Store store = Store;
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainClass(Diagram.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(AddOrRemoveTabForEvent), action);
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
		#region Automation support
		/// <summary>
		/// Class used for the document extensibility layer. Request
		/// the "ORMExtensionManager" extension object.
		/// </summary>
		[ComVisible(true)]
		[ClassInterface(ClassInterfaceType.AutoDispatch)]
		public class ORMExtensionManagerAutomationObject
		{
			private ORMDesignerDocData myDocData;
			/// <summary>
			/// Create a new <see cref="ORMExtensionManagerAutomationObject"/> for the specific <paramref name="docData"/>
			/// </summary>
			/// <param name="docData">An <see cref="ORMDesignerDocData"/> instance</param>
			public ORMExtensionManagerAutomationObject(ORMDesignerDocData docData)
			{
				myDocData = docData;
			}
			/// <summary>
			/// Retrieve a string array of loaded extension names
			/// </summary>
			public string[] GetLoadedExtensions()
			{
				ICollection<ExtensionModelBinding> availableExtensions = ORMDesignerPackage.ExtensionLoader.AvailableCustomExtensions.Values;
				List<string> extensionNames = new List<string>();
				foreach (DomainModel domainModel in myDocData.Store.DomainModels)
				{
					Type domainModelType = domainModel.GetType();
					foreach (ExtensionModelBinding extensionInfo in availableExtensions)
					{
						if (extensionInfo.Type == domainModelType)
						{
							extensionNames.Add(extensionInfo.NamespaceUri);
							break;
						}
					}
				}
				return extensionNames.ToArray();
			}
			/// <summary>
			/// Verify that the requested extensions are loaded in the current designer instance
			/// </summary>
			public void EnsureExtensions(string[] extensions)
			{
				int ensureCount;
				if (extensions != null && (ensureCount = extensions.Length) != 0)
				{
					string[] clonedExtensions = (string[])extensions.Clone();
					ExtensionLoader extensionLoader = ORMDesignerPackage.ExtensionLoader;
					IDictionary<string, ExtensionModelBinding> availableExtensions = extensionLoader.AvailableCustomExtensions;
					ICollection<ExtensionModelBinding> availableExtensionsCollection = availableExtensions.Values;
					Dictionary<string, ExtensionModelBinding> requestedExtensions = null;
					List<ExtensionModelBinding> nonRequestedLoadedExtensions = null;
					foreach (DomainModel domainModel in myDocData.Store.DomainModels)
					{
						Type domainModelType = domainModel.GetType();
						foreach (ExtensionModelBinding extensionInfo in availableExtensionsCollection)
						{
							if (extensionInfo.Type == domainModelType)
							{
								string namespaceUri = extensionInfo.NamespaceUri;
								int clonedIndex = Array.IndexOf<string>(clonedExtensions, namespaceUri);
								if (clonedIndex == -1)
								{
									(nonRequestedLoadedExtensions ?? (nonRequestedLoadedExtensions = new List<ExtensionModelBinding>())).Add(extensionInfo);
								}
								else
								{
									--ensureCount;
									if (ensureCount == 0)
									{
										return; // Nothing to do, everything we need is already loaded
									}
									(requestedExtensions ?? (requestedExtensions = new Dictionary<string,ExtensionModelBinding>())).Add(namespaceUri, extensionInfo);
									clonedExtensions[clonedIndex] = null;
								}
								break;
							}
						}
					}
					for (int i = 0; i < clonedExtensions.Length; ++i)
					{
						string newExtension = clonedExtensions[i];
						if (newExtension != null)
						{
							--ensureCount;
							ExtensionModelBinding extensionInfo;
							if (availableExtensions.TryGetValue(newExtension, out extensionInfo))
							{
								(requestedExtensions ?? (requestedExtensions = new Dictionary<string,ExtensionModelBinding>())).Add(extensionInfo.NamespaceUri, extensionInfo);
							}
							if (ensureCount == 0)
							{
								break;
							}
						}
					}
					Object streamObj;
					(myDocData as EnvDTE.IExtensibleObject).GetAutomationObject("ORMXmlStream", null, out streamObj);
					Stream currentStream = streamObj as Stream;
					Stream newStream = null;

					Debug.Assert(currentStream != null);

					try
					{
						extensionLoader.VerifyRequiredExtensions(ref requestedExtensions);
						ICollection<ExtensionModelBinding> allExtensions = requestedExtensions.Values;
						if (nonRequestedLoadedExtensions != null)
						{
							nonRequestedLoadedExtensions.AddRange(allExtensions);
							allExtensions = nonRequestedLoadedExtensions;
						}
						newStream = ExtensionLoader.CleanupStream(currentStream, extensionLoader.StandardDomainModels, allExtensions, null);
						myDocData.ReloadFromStream(newStream, currentStream);
					}
					finally
					{
						if (currentStream != null)
						{
							currentStream.Dispose();
						}
						if (newStream != null)
						{
							newStream.Dispose();
						}
					}
				}
			}
		}
		/// <summary>
		/// Implements IExtensibleObject.GetAutomationObject. Returns the ORM XML stream for
		/// the "ORMXmlStream" object name and the this object for everything else.
		/// </summary>
		protected void GetAutomationObject(string name, IExtensibleObjectSite parent, out object result)
		{
			if ("ORMXmlStream" == name)
			{
				MemoryStream stream = new MemoryStream();
				(new ORMSerializationEngine(Store)).Save(stream);
				stream.Position = 0;
				result = stream;
				return;
			}
			else if ("ORMExtensionManager" == name)
			{
				// This returns an object with two methods:
				// GetLoadedExtensions returns an array of current loaded extension objects
				// EnsureExtensions accepts an array of extensions that need to be loaded
				result = new ORMExtensionManagerAutomationObject(this);
				return;
			}
			result = this;
		}
		void IExtensibleObject.GetAutomationObject(string Name, IExtensibleObjectSite pParent, out object ppDisp)
		{
			GetAutomationObject(Name, pParent, out ppDisp);
		}
		#endregion // Automation support
		#region Locale Change
		/// <summary>
		/// Gives extensions a chance to update when the thread culture changes
		/// </summary>
		private void CultureChangedEvent(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Locale)
			{
				Store store = Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.CultureChangedTransactionName))
				{
					foreach (INotifyCultureChange changeListener in Utility.EnumerateDomainModels<INotifyCultureChange>(store.DomainModels))
					{
						changeListener.CultureChanged();
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
		}
		#endregion // Implementation
	}
	#endregion // ORMDesignerDocData class
}
