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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	public sealed partial class ExtensionManager : Form
	{
		private readonly Store myStore;

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
			IWin32Window dialogOwner = Utility.GetDialogOwnerWindow(serviceProvider);
			if (extensionManager.ShowDialog(dialogOwner) == DialogResult.OK)
			{
				// TODO: Prompt the user to make sure they really want us to start deleting stuff...

				ListView.CheckedListViewItemCollection checkedItems = extensionManager.lvExtensions.CheckedItems;
				ExtensionLoader extensionLoader = ORMDesignerPackage.ExtensionLoader;
				IDictionary<string, ExtensionModelBinding> availableExtensions = extensionLoader.AvailableCustomExtensions;
				Dictionary<string, ExtensionModelBinding> checkedTypes = new Dictionary<string, ExtensionModelBinding>(availableExtensions.Count);
				foreach (ListViewItem listViewItem in checkedItems)
				{
					string extensionNamespace = (string)listViewItem.Tag;
					checkedTypes.Add(extensionNamespace, availableExtensions[extensionNamespace]);
				}

				// Make sure all required extensions are turned on. This will turn previously ignored
				// secondary extensions back on.
				extensionLoader.VerifyRequiredExtensions(ref checkedTypes);

				Stream currentStream = null;
				Stream newStream = null;
				Stream modifiedStream = null;
				try
				{
					Object streamObj;
					EnvDTE.IExtensibleObject docDataExtender = (EnvDTE.IExtensibleObject)docData;
					docDataExtender.GetAutomationObject("ORMXmlStream", null, out streamObj);
					currentStream = streamObj as Stream;

					Debug.Assert(currentStream != null);

					// Allow each domain model that is being removed to run custom code immediately before the
					// unload process.
					Transaction customUnloadTransaction = null;
					Store store = docData.Store;
					try
					{
						IDomainModelUnloading[] unloadingModels = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDomainModelUnloading>();
						if (unloadingModels != null)
						{
							for (int i = 0; i < unloadingModels.Length; ++i)
							{
								IDomainModelUnloading unloadingModel = unloadingModels[i];
								ICustomSerializedDomainModel serializedModel;
								if (null != (serializedModel = unloadingModel as ICustomSerializedDomainModel))
								{
									string[,] namespaceInfo = serializedModel.GetCustomElementNamespaces();
									int namespaceCount = namespaceInfo.GetLength(0);
									int j = 0;
									for (; j < namespaceCount; ++j)
									{
										if (checkedTypes.ContainsKey(namespaceInfo[j, 1]))
										{
											break;
										}
									}
									if (j == namespaceCount)
									{
										// Extension domain model is not in the pending set, go ahead and run
										// the custom code to unload it cleanly.
										if (customUnloadTransaction == null)
										{
											customUnloadTransaction = store.TransactionManager.BeginTransaction("Domain Models Unloading"); // String not localized, won't be displayed on either success or failure
										}
										unloadingModel.DomainModelUnloading(store);
									}
								}
							}
						}
					}
					finally
					{
						if (customUnloadTransaction != null)
						{
							if (customUnloadTransaction.HasPendingChanges)
							{
								customUnloadTransaction.Commit();

								// Get the modified stream
								docDataExtender.GetAutomationObject("ORMXmlStream", null, out streamObj);
								modifiedStream = streamObj as Stream;
							}
							customUnloadTransaction.Dispose();
						}
					}

					newStream = ExtensionLoader.CleanupStream(modifiedStream ?? currentStream, extensionLoader.StandardDomainModels, checkedTypes.Values, null);
					docData.ReloadFromStream(newStream, currentStream);
				}
				finally
				{
					if (currentStream != null)
					{
						currentStream.Dispose();
					}
					if (modifiedStream != null)
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
		/// <summary>
		/// The onload method.  first we gather a list of available custom
		/// extension then we populate the <see cref="ListView"/> with the list.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected sealed override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lvExtensions.Items.Clear();
			foreach (ExtensionModelBinding type in ORMDesignerPackage.ExtensionLoader.AvailableCustomExtensions.Values)
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
		/// This method adds the passed in ExtensionModelBinding to the ListView on the ExtensionManager dialogue.
		/// </summary>
		/// <param name="extensionBinding">The extension you want to add.</param>
		private void AddItemToListView(ExtensionModelBinding extensionBinding)
		{
			Type type = extensionBinding.Type;
			ListViewItem lvi = new ListViewItem();
			lvi.Tag = extensionBinding.NamespaceUri;
			if (null != myStore.FindDomainModel(extensionBinding.DomainModelId))
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
		/// Check items required by a newly checked item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void lvExtensions_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			ListViewItem item = e.Item;
			if (item.Checked)
			{
				ExtensionLoader extensionLoader = ORMDesignerPackage.ExtensionLoader;
				ExtensionModelBinding checkedType = extensionLoader.AvailableCustomExtensions[(string)item.Tag];
				ListView.ListViewItemCollection items = item.ListView.Items;
				foreach (Guid extendsModelId in checkedType.ExtendsDomainModelIds)
				{
					string extensionName = extensionLoader.MapExtensionDomainModelToName(extendsModelId);
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
