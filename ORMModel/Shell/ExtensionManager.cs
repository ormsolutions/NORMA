using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ExtensionManager : Form
	{
		private Store _store;
		private List<Type> _loadedSubStoreTypes;
		/// <summary>
		/// Initialize the ExtensionManager form
		/// </summary>
		private ExtensionManager(Store store)
		{
			InitializeComponent();
			this._store = store;
			System.Collections.ICollection subStores = store.SubStores.Values;
			this._loadedSubStoreTypes = new List<Type>(subStores.Count);
			foreach (SubStore subStore in subStores)
			{
				this._loadedSubStoreTypes.Add(subStore.GetType());
			}
		}
		/// <summary>
		/// This method shows the ExtensionManager form.
		/// </summary>
		/// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
		/// <param name="docData">The docData retrieved from the current document.</param>
		public static void ShowDialog(IServiceProvider serviceProvider, ORMDesignerDocData docData)
		{
			ExtensionManager extensionManager = new ExtensionManager(docData.Store);
			if (extensionManager.ShowDialog(new OwnerWindow(serviceProvider)) == DialogResult.OK)
			{
				// TODO: Prompt the user to make sure they really want us to start deleting stuff...

				ListView.CheckedListViewItemCollection checkedItems = extensionManager.lvExtensions.CheckedItems;
				List<ORMExtensionType> checkedTypes = new List<ORMExtensionType>(checkedItems.Count);
				foreach (ListViewItem listViewItem in checkedItems)
				{
					checkedTypes.Add((ORMExtensionType)listViewItem.Tag);
				}

				Stream stream = null;
				try
				{
					Object streamObj;
					(docData as EnvDTE.IExtensibleObject).GetAutomationObject("ORMXmlStream", null, out streamObj);
					stream = streamObj as Stream;

					System.Diagnostics.Debug.Assert(stream != null);

					stream = CleanupStream(stream, checkedTypes);

					docData.LoadDocDataFromStream(docData.FileName, true, stream);
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
					}
				}
			}
		}
		/// <summary>
		/// This is a custom callback class for the XSLT file that is
		/// responsible for adding or removing the custom extension namespaces to the ORM document.
		/// </summary>
		private class NamespaceUtility
		{
			private readonly List<string> _namespaces;
			private readonly List<string> _addedNamespaces;
			private IEnumerator<string> _enumerator;
			private bool _hasCurrent;
			private static readonly Random random = new Random();
			/// <summary>
			/// Default Constructor for the NameSpaceUtility.
			/// </summary>
			/// <param name="namespaces">a <see cref="List"/> of available namespaces.</param>
			public NamespaceUtility(List<string> namespaces)
			{
				this._namespaces = namespaces;
				namespaces.Sort();
				this._addedNamespaces = new List<string>(namespaces.Count);
			}
			/// <summary>
			/// This method checks to see if the namespace was added via the ExtensionManager Dialogue
			/// </summary>
			/// <param name="namespaceUri">The namespace to check if it was added,</param>
			/// <returns>true if the namespace was added false if it was not.</returns>
			public bool wasNamespaceAdded(string namespaceUri)
			{
				return this._addedNamespaces.BinarySearch(namespaceUri) >= 0;
			}
			/// <summary>
			/// This method adds the namespace to the selected list. for future reference.
			/// </summary>
			/// <param name="namespaceUri">the namespace you want to add.</param>
			public void addedNamespace(string namespaceUri)
			{
				this._addedNamespaces.Add(namespaceUri);
			}
			/// <summary>
			/// This is an Iterator helper class to move accross the available namespaces
			/// you wish to add.
			/// </summary>
			/// <returns>The current namespace position if there is a next one. an empty string if there is not.</returns>
			public string getNextSelectedNamespace()
			{
				if (this._enumerator == null)
				{
					this._enumerator = this._namespaces.GetEnumerator();
				}
				this._hasCurrent = this._enumerator.MoveNext();
				if (this._hasCurrent)
				{
					return this._enumerator.Current;
				}
				else
				{
					return string.Empty;
				}
			}
			/// <summary>
			/// This method checks if the namespace was selected in the ExtensionManager Dialogue.
			/// </summary>
			/// <param name="namespaceUri">The namespace you wish to check for checked status.</param>
			/// <returns>true if the namespace was selected false if it was not.</returns>
			public bool isNamespaceSelected(string namespaceUri)
			{
				return this._namespaces.BinarySearch(namespaceUri) >= 0;
			}
			/// <summary>
			/// This is a Randomizer to get around the fact that we do not have unique identifiers
			/// for the different namespaces.
			/// </summary>
			/// <returns>the a psuedo random namespace extension to be used as the prefix.</returns>
			public static string getRandomPrefix()
			{
				return "ormExtension" + random.Next();
			}
		}
		/// <summary>
		/// This method is responsible for cleaning the streamed ORM file.
		/// </summary>
		/// <param name="stream">The file stream that contains the ORM file.</param>
		/// <param name="extensionTypes">A list of extension types.</param>
		/// <returns>The cleaned stream.</returns>
		private static Stream CleanupStream(Stream stream, IList<ORMExtensionType> extensionTypes)
		{
			MemoryStream outputStream = new MemoryStream((int)stream.Length);
			XsltArgumentList argList = new XsltArgumentList();

			List<string> namespaces = new List<string>(extensionTypes.Count);
			foreach (ORMExtensionType ormExtensionType in extensionTypes)
			{
				namespaces.Add(ormExtensionType.NamespaceUri);
			}
			argList.AddExtensionObject("urn:schemas-neumont-edu:ORM:NamespacesUtility", new NamespaceUtility(namespaces));
			XslCompiledTransform transform = GetExtensionStripperTransform();

			stream.Position = 0;
			using (XmlReader reader = XmlReader.Create(stream))
			{
				transform.Transform(reader, argList, outputStream);
			}
			outputStream.Position = 0;
			return outputStream;
		}
		/// <summary>
		/// The onload method.  first we gather a list of available custom
		/// extension then we populate the <see cref="ListView"/> with the list.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lvExtensions.Items.Clear();
			IList<ORMExtensionType> extensions = Shell.ORMDesignerPackage.GetAvailableCustomExtensions();
			foreach (ORMExtensionType type in extensions)
			{
				AddItemToListView(type);
			}

		}
		/// <summary>
		/// This method adds the passed in ORMExtensionType to the ListView on the ExtensionManager dialogue.
		/// </summary>
		/// <param name="ormExtensionType">The extension you want to add.</param>
		private void AddItemToListView(ORMExtensionType ormExtensionType)
		{
			Type type = ormExtensionType.Type;
			ListViewItem lvi = new ListViewItem();
			lvi.Tag = ormExtensionType;
			if (this._loadedSubStoreTypes.Contains(type))
			{
				lvi.Checked = true;
			}

			//Add the Display Name
			object[] attributes = type.GetCustomAttributes(typeof(MetaModelDisplayNameAttribute), true);
			if (attributes.Length > 0)
			{
				MetaModelDisplayNameAttribute displayNameAttribute = attributes[0] as MetaModelDisplayNameAttribute;
				lvi.SubItems.Add(displayNameAttribute.DisplayName);
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "Custom extension does not have MetaModelDisplayNameAttribute");
			}
			//Add the description
			attributes = type.GetCustomAttributes(typeof(MetaModelDescriptionAttribute), true);
			if (attributes.Length > 0)
			{
				MetaModelDescriptionAttribute descriptionAttribute = attributes[0] as MetaModelDescriptionAttribute;
				lvi.SubItems.Add(descriptionAttribute.Description);
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "Custom extension does not have MetaModelDescriptionAttribute");
			}
			lvExtensions.Items.Add(lvi);
		}
		private class OwnerWindow : IWin32Window
		{
			private IntPtr myHandle;
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
			private static extern IntPtr GetDesktopWindow();
			public OwnerWindow(IServiceProvider serviceProvider)
			{
				IVsUIShell shell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell));
				if (shell == null || ErrorHandler.Failed(shell.GetDialogOwnerHwnd(out myHandle)) || myHandle == IntPtr.Zero)
				{
					myHandle = GetDesktopWindow();
				}
			}
			#region IWin32Window Members
			IntPtr IWin32Window.Handle
			{
				get
				{
					return myHandle;
				}
			}
			#endregion // IWin32Window Members
		}
		/// <summary>
		/// This method grabs and compiles the XSLT transform that strips or adds custom extension to the ORM file.
		/// </summary>
		/// <returns>The compiled XSLT tranform.</returns>
		private static XslCompiledTransform GetExtensionStripperTransform()
		{
			XslCompiledTransform retVal = new XslCompiledTransform(false);
			Type resourceType = typeof(ExtensionManager);
			using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "ExtensionStripper.xslt"))
			{
				using (StreamReader reader = new StreamReader(transformStream))
				{
					using (XmlReader xmlReader = new XmlTextReader(reader))
					{
						retVal.Load(xmlReader, null, null);
					}
				}
			}
			return retVal;
		}
	}
}