#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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

// Turning this on will result in the ability to debug the extension stripper
// transform and related callback code. A message showing with debugging when
// the transform is first loading, giving you the opportunity to format the
// transform, insert breakpoints, etc. Obviously, this is for debug purposes
// only and should never be turned on.
//#define DEBUG_EXTENSIONSTRIPPER_TRANSFORM
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Shell;
using System.Collections;
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	public sealed partial class ExtensionManager : Form
	{
		private readonly Store myStore;
		private static XslCompiledTransform myExtensionStripperTransform;
#if DEBUG_EXTENSIONSTRIPPER_TRANSFORM
		private static System.CodeDom.Compiler.TempFileCollection myDebugExtensionStripperTempFile;
#endif // DEBUG_EXTENSIONSTRIPPER_TRANSFORM
		private static readonly object LockObject = new object();

		/// <summary>
		/// Initialize the ExtensionManager form
		/// </summary>
		private ExtensionManager(Store store)
		{
			InitializeComponent();
			this.myStore = store;
		}
		/// <summary>
		/// This method shows the ExtensionManager form.
		/// </summary>
		/// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
		/// <param name="docData">The docData retrieved from the current document.</param>
		[CLSCompliant(false)]
		public static void ShowDialog(IServiceProvider serviceProvider, ORMDesignerDocData docData)
		{
			ExtensionManager extensionManager = new ExtensionManager(docData.Store);
			if (extensionManager.ShowDialog(Utility.GetDialogOwnerWindow(serviceProvider)) == DialogResult.OK)
			{
				// TODO: Prompt the user to make sure they really want us to start deleting stuff...

				ListView.CheckedListViewItemCollection checkedItems = extensionManager.lvExtensions.CheckedItems;
				IDictionary<string, ORMExtensionType> availableExtensions = ORMDesignerPackage.GetAvailableCustomExtensions();
				Dictionary<string, ORMExtensionType> checkedTypes = new Dictionary<string, ORMExtensionType>(availableExtensions.Count);
				foreach (ListViewItem listViewItem in checkedItems)
				{
					string extensionNamespace = (string)listViewItem.Tag;
					checkedTypes.Add(extensionNamespace, availableExtensions[extensionNamespace]);
				}

				// Make sure all required extensions are turned on. This will turn previously ignored
				// secondary extensions back on.
				ORMDesignerPackage.VerifyRequiredExtensions(checkedTypes);

				Stream stream = null;
				try
				{
					Object streamObj;
					(docData as EnvDTE.IExtensibleObject).GetAutomationObject("ORMXmlStream", null, out streamObj);
					stream = streamObj as Stream;

					Debug.Assert(stream != null);

					stream = CleanupStream(stream, ORMDesignerPackage.StandardDomainModels, checkedTypes.Values);
					docData.ReloadFromStream(stream);
				}
				finally
				{
					if (stream != null)
					{
						stream.Dispose();
					}
				}
			}
		}
		/// <summary>
		/// This is a custom callback class for the XSLT file that is
		/// responsible for adding or removing the custom extension namespaces to the ORM document.
		/// </summary>
		private sealed class ExtensionManagerUtility
		{
			private readonly string[] myNamespaces;
			private Dictionary<string, string> myAddedNamespaces;
			private IEnumerator<string> myEnumerator;
			private int myLastIdRemovalPhase;
			private int myCurrentIdRemovalPhase;
			private Dictionary<string, string> myRemovedIds;
			private static readonly Random myRandom = new Random();
			/// <summary>
			/// Default Constructor for the <see cref="ExtensionManagerUtility"/>.
			/// </summary>
			/// <param name="namespaces">An array of available namespaces.</param>
			public ExtensionManagerUtility(string[] namespaces)
			{
				myNamespaces = namespaces;
				Array.Sort<string>(namespaces);
				myLastIdRemovalPhase = -1;
			}
			/// <summary>
			/// This method checks to see if the namespace was added via the ExtensionManager Dialogue
			/// </summary>
			/// <param name="namespaceUri">The namespace to check if it was added,</param>
			/// <returns>true if the namespace was added false if it was not.</returns>
			public bool wasNamespaceAdded(string namespaceUri)
			{
				Dictionary<string, string> addedNamespaces = myAddedNamespaces;
				return addedNamespaces != null && myAddedNamespaces.ContainsKey(namespaceUri);
			}
			/// <summary>
			/// This method adds the namespace to the selected list. for future reference.
			/// </summary>
			/// <param name="namespaceUri">the namespace you want to add.</param>
			public void addNamespace(string namespaceUri)
			{
				Dictionary<string, string> addedNamespaces = myAddedNamespaces;
				if (addedNamespaces == null)
				{
					myAddedNamespaces = addedNamespaces = new Dictionary<string, string>();
					addedNamespaces.Add(namespaceUri, namespaceUri);
				}
				else
				{
					addedNamespaces[namespaceUri] = namespaceUri;
				}
			}
			/// <summary>
			/// This is an Iterator helper class to move accross the available namespaces
			/// you wish to add.
			/// </summary>
			/// <returns>The current namespace position if there is a next one. an empty string if there is not.</returns>
			public string getNextSelectedNamespace()
			{
				IEnumerator<string> enumerator = myEnumerator ?? (myEnumerator = ((IEnumerable<string>)myNamespaces).GetEnumerator());
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
				else
				{
					myEnumerator = null;
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
				return Array.BinarySearch<string>(myNamespaces, namespaceUri) >= 0;
			}
			/// <summary>
			/// This is a Randomizer to get around the fact that we do not have unique identifiers
			/// for the different namespaces.
			/// </summary>
			/// <returns>the a psuedo random namespace extension to be used as the prefix.</returns>
			public static string getRandomPrefix()
			{
				return "ormExtension" + myRandom.Next();
			}
			/// <summary>
			/// Remember id values so we can look them up quickly on a later pass
			/// </summary>
			public bool removeId(string idValue)
			{
				Dictionary<string, string> removedIds = myRemovedIds;
				if (removedIds == null)
				{
					myRemovedIds = removedIds = new Dictionary<string, string>();
				}
				else if (myRemovedIds.ContainsKey(idValue))
				{
					return false;
				}
				myLastIdRemovalPhase = myCurrentIdRemovalPhase;
				removedIds.Add(idValue, idValue);
				return true;
			}
			/// <summary>
			/// Begin a new id removal phase. Returns true if any ids
			/// have been removed since the last time this method was called.
			/// </summary>
			public bool beginIdRemovalPhase()
			{
				if (myLastIdRemovalPhase == myCurrentIdRemovalPhase)
				{
					++myCurrentIdRemovalPhase;
					return true;
				}
				return false;
			}
			/// <summary>
			/// See if the attribute value corresponds to a removed identifier.
			/// </summary>
			/// <param name="attributeValue">Verify if the attribute value is a reference to
			/// a removed identifier.</param>
			/// <returns>true value is a removed identifier</returns>
			public bool isRemovedId(string attributeValue)
			{
				Dictionary<string, string> removedIds = myRemovedIds;
				return removedIds != null && removedIds.ContainsKey(attributeValue);
			}
		}
		/// <summary>
		/// This method is responsible for cleaning the streamed ORM file.
		/// </summary>
		/// <param name="stream">The file stream that contains the ORM file.</param>
		/// <param name="standardTypes">The standard models that are not loaded as extensions</param>
		/// <param name="extensionTypes">A collection of extension types.</param>
		/// <returns>The cleaned stream.</returns>
		public static Stream CleanupStream(Stream stream, ICollection<Type> standardTypes, ICollection<ORMExtensionType> extensionTypes)
		{
			MemoryStream outputStream = new MemoryStream((int)stream.Length);
			XsltArgumentList argList = new XsltArgumentList();

			// Get all of the custom serialization attributes for the standard and
			// extension types. The serialization engine does not serialize elements
			// for types without a serialization attribute, so there is no need to
			// look at standard or extension types without this attribute.
			CustomSerializedXmlNamespacesAttribute[] namespaceAttributes = new CustomSerializedXmlNamespacesAttribute[extensionTypes.Count + standardTypes.Count];
			int totalNamespaceCount = 0;
			int serializedAttributeCount = 0;
			foreach (Type standardType in standardTypes)
			{
				object[] attributes = standardType.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				int currentNamespaceCount;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentNamespaceCount = (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count))
				{
					totalNamespaceCount += currentNamespaceCount;
					namespaceAttributes[serializedAttributeCount] = currentAttribute;
					++serializedAttributeCount;
				}
			}
			foreach (ORMExtensionType extensionType in extensionTypes)
			{
				object[] attributes = extensionType.Type.GetCustomAttributes(typeof(CustomSerializedXmlNamespacesAttribute), false);
				CustomSerializedXmlNamespacesAttribute currentAttribute;
				int currentNamespaceCount;
				if (attributes != null &&
					attributes.Length != 0 &&
					0 != (currentNamespaceCount = (currentAttribute = (CustomSerializedXmlNamespacesAttribute)attributes[0]).Count))
				{
					totalNamespaceCount += currentNamespaceCount;
					namespaceAttributes[serializedAttributeCount] = currentAttribute;
					++serializedAttributeCount;
				}
			}
			string[] namespaces = new string[totalNamespaceCount];
			int namespaceIndex = -1;
			for (int i = 0; i < serializedAttributeCount; ++i)
			{
				CustomSerializedXmlNamespacesAttribute currentAttribute = namespaceAttributes[i];
				int attributeCount = currentAttribute.Count;
				for (int j = 0; j < attributeCount; ++j)
				{
					namespaces[++namespaceIndex] = currentAttribute[j];
				}
			}
			argList.AddExtensionObject("urn:schemas-neumont-edu:ORM:ExtensionManagerUtility", new ExtensionManagerUtility(namespaces));
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
		protected sealed override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lvExtensions.Items.Clear();
			foreach (ORMExtensionType type in ORMDesignerPackage.GetAvailableCustomExtensions().Values)
			{
				if (!type.IsSecondary)
				{
					AddItemToListView(type);
				}
			}
			lvExtensions.ListViewItemSorter = ItemComparer.Instance;
			lvExtensions.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lvExtensions_ItemChecked);
		}

		private sealed class ItemComparer : IComparer
		{
			public static readonly IComparer Instance = new ItemComparer();
			private ItemComparer()
			{
			}

			public int Compare(object obj1, object obj2)
			{
				ListViewItem item1 = (ListViewItem) obj1;
				ListViewItem item2 = (ListViewItem) obj2;
				return string.Compare(item1.SubItems[1].Text, item2.SubItems[1].Text, false, CultureInfo.CurrentCulture);
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
			lvi.Tag = ormExtensionType.NamespaceUri;
			if (null != myStore.FindDomainModel(ormExtensionType.DomainModelId))
			{
				lvi.Checked = true;
			}

			//Add the DisplayName
			lvi.SubItems.Add(DomainTypeDescriptor.GetDisplayName(type));
			
			//Add the Description
			lvi.SubItems.Add(DomainTypeDescriptor.GetDescription(type));

			lvExtensions.Items.Add(lvi);
		}
		/// <summary>
		/// This method grabs and compiles the XSLT transform that strips or adds custom extensions to the ORM file.
		/// </summary>
		/// <returns>The compiled XSLT tranform.</returns>
		private static XslCompiledTransform GetExtensionStripperTransform()
		{
			XslCompiledTransform retVal = myExtensionStripperTransform;
			if (retVal == null)
			{
				lock (LockObject)
				{
					retVal = myExtensionStripperTransform;
					if (retVal == null)
					{
						Type resourceType = typeof(ExtensionManager);
						using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "ExtensionStripper.xslt"))
						{
#if DEBUG_EXTENSIONSTRIPPER_TRANSFORM 
							retVal = new XslCompiledTransform(true);
							System.CodeDom.Compiler.TempFileCollection tempFiles = new System.CodeDom.Compiler.TempFileCollection();
							string fileName = tempFiles.AddExtension("xslt");
							myDebugExtensionStripperTempFile = tempFiles;
							using (FileStream tempFile = new FileStream(fileName, FileMode.Create))
							{
								byte[] buffer = new byte[1024];
								int totalRead;
								do
								{
									totalRead = transformStream.Read(buffer, 0, 1024);
									tempFile.Write(buffer, 0, totalRead);
								} while (totalRead == 1024);
								tempFile.Flush();
							}
							IWin32Window ownerWindow = Utility.GetDialogOwnerWindow(ORMDesignerPackage.Singleton);
							if (DialogResult.Yes == MessageBox.Show(ownerWindow, "Debug extension stripper transform saved to temporary file:\r\n\r\n" + fileName + "\r\n\r\nChoose 'Yes' to place this file name on the clipboard and wait while you prepare to debug it by\r\n   -Opening the file in the debugger\r\n   -Formatting the file with the Format Document command (on the Edit/Advanced menu)\r\n   -Resaving the temporary file\r\n   -Adding breakpoints to the transform", "Extension Stripper Debugger", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
							{
								Clipboard.Clear();
								Clipboard.SetText(fileName);
								MessageBox.Show(ownerWindow, "Press OK when you are ready to load the transform and continue debugging.", "Extension Stripper Debugger", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							retVal.Load(fileName, XsltSettings.TrustedXslt, new XmlUrlResolver());
#else
							retVal = new XslCompiledTransform();
							using (XmlReader reader = XmlReader.Create(transformStream))
							{
								retVal.Load(reader, XsltSettings.TrustedXslt, new XmlUrlResolver());
							}
#endif
						}
						myExtensionStripperTransform = retVal;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Check items required by a newly checked item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void lvExtensions_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			ListViewItem item = e.Item;
			if (item.Checked)
			{
				ORMExtensionType checkedType = ORMDesignerPackage.GetAvailableCustomExtensions()[(string)item.Tag];
				ListView.ListViewItemCollection items = item.ListView.Items;
				foreach (Guid extendsModelId in checkedType.ExtendsDomainModelIds)
				{
					string extensionName = ORMDesignerPackage.MapExtensionDomainModelToName(extendsModelId);
					if (extensionName != null)
					{
						foreach (ListViewItem requiresItem in items)
						{
							if ((string)requiresItem.Tag == extensionName)
							{
								if (!requiresItem.Checked)
								{
									requiresItem.Checked = true;
								}
								break;
							}
						}
					}
				}
			}
		}
	}
}
