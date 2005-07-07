#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
using Northface.Tools.ORM;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Reflection;
using System.Runtime.Serialization;
using Northface.Tools.ORM.Shell;
using System.Xml.Query;
using System.Xml;

#endregion

namespace ORM2CommandLineTest
{
	public class ORMDocServices : IORMToolServices, IORMFontAndColorService
	{
		public ORMDocServices()
		{

		}

		#region IORMToolServices Members
		protected IORMFontAndColorService FontAndColorService
		{
			get
			{
				return this;
			}
		}
		IORMFontAndColorService IORMToolServices.FontAndColorService
		{
			get
			{
				return FontAndColorService;
			}
		}

		private IORMToolTaskProvider myTaskProvider;
		protected IORMToolTaskProvider TaskProvider
		{
			get
			{
				IORMToolTaskProvider provider = myTaskProvider;
				if (provider == null)
				{
					myTaskProvider = provider = CreateTaskProvider();
				}
				return provider;
			}
		}
		IORMToolTaskProvider IORMToolServices.TaskProvider
		{
			get
			{
				return TaskProvider;
			}
		}
		/// <summary>
		/// Create a new task provider. Called once the first time the TaskProvider
		/// property is accessed.
		/// </summary>
		protected virtual IORMToolTaskProvider CreateTaskProvider()
		{
			Debug.Assert(myTaskProvider == null);
			return new ORMTaskProvider();
		}
		#endregion

		#region IORMFontAndColorService Members
		protected Color GetBackColor(ORMDesignerColor colorindex)
		{
			return Color.Black;
		}
		Color IORMFontAndColorService.GetBackColor(ORMDesignerColor colorIndex)
		{
			return GetBackColor(colorIndex);
		}

		protected Font GetFont()
		{
			Font font = new Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
			return font;
		}
		Font IORMFontAndColorService.GetFont()
		{
			return GetFont();
		}

		protected FONTFLAGS GetFontFlags(ORMDesignerColor colorindex)
		{
			return FONTFLAGS.FF_BOLD;
		}
		FONTFLAGS IORMFontAndColorService.GetFontFlags(ORMDesignerColor colorIndex)
		{
			return GetFontFlags(colorIndex);
		}

		protected Color GetForeColor(ORMDesignerColor colorindex)
		{
			return Color.White;
		}
		Color IORMFontAndColorService.GetForeColor(ORMDesignerColor colorIndex)
		{
			return GetForeColor(colorIndex);
		}
		#endregion

		public ORMStore LoadFile(string fileName)
		{
			ORMStore store = new ORMStore(this);
			store.UndoManager.UndoState = UndoState.Disabled;
			Type[] subStores = new Type[4] { typeof(Core), typeof(CoreDesignSurface), typeof(ORMMetaModel), typeof(ORMShapeModel) };
			store.LoadMetaModels(subStores);
			AddErrorReportingEvents(store);
			using (Transaction t = store.TransactionManager.BeginTransaction("Initialization"))
			{
				foreach (Type subStoreType in subStores)
				{
					if (subStoreType == null)
						continue;

					object[] createArgs = new Object[1] { store };
					SubStore subStoreInstance = (SubStore)subStoreType.Assembly.CreateInstance(subStoreType.FullName, false, BindingFlags.Public | BindingFlags.Instance, null, createArgs, null, null);
				}
				t.Commit();
			}

			using (Transaction t = store.TransactionManager.BeginTransaction("File load and fixup"))
			{
				using (FileStream stream = File.OpenRead(fileName))
				{
					if (fileName == null)
						return null;

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
				t.Commit();
			}
			return store;
		}

		/// <summary>
		/// Saves the model in Store format
		/// </summary>
		/// <param name="fileName"></param>
		public void SaveFile(Store store, string fileName)
		{
			using (FileStream fileStream = File.Create(fileName))
			{
				(new ORMSerializer(store)).Save(fileStream);
			}
		}

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
		private IEnumerable<IDeserializationFixupListener> DeserializationFixupListeners
		{
			get
			{
				foreach (IDeserializationFixupListener listener in ORMModel.DeserializationFixupListeners)
				{
					yield return listener;
				}
				foreach (IDeserializationFixupListener listener in ORMDiagram.DeserializationFixupListeners)
				{
					yield return listener;
				}
			}
		}
		#region Model Event Manipulation
		private void AddErrorReportingEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;
			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(ModelHasError.MetaRelationshipGuid);

			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ErrorAddedEvent));

			classInfo = dataDirectory.FindMetaClass(ModelError.MetaClassGuid);
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(ErrorRemovedEvent));
			eventDirectory.ElementAttributeChanged.Add(classInfo, new ElementAttributeChangedEventHandler(ErrorChangedEvent));
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
		#endregion //Model Event Manipulation
	}
}
