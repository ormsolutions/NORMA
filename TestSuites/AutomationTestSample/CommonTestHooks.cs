using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using EnvDTE;
using EnvDTE80;
using IServiceProvider = System.IServiceProvider;
using IServiceProvider_COM = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace ORMRegressionTestAddin
{
	/// <summary>
	/// Used with CommonTestHooks.FindAccessibleObject as a node
	/// in the search path
	/// </summary>
	public struct AccessiblePathNode
	{
		private string myName;
		private string myValue;
		private int myIndex;
		/// <summary>
		/// Match the first child object with the given name
		/// </summary>
		/// <param name="name">The accessible name</param>
		public AccessiblePathNode(string name)
		{
			myName = name;
			myValue = null;
			myIndex = -1;
		}
		/// <summary>
		/// Match the first child object with  the given name and value
		/// </summary>
		/// <param name="name">The accessible name</param>
		/// <param name="value">The accessible value (can be null)</param>
		public AccessiblePathNode(string name, string value)
		{
			myName = name;
			myValue = value;
			myIndex = -1;
		}
		/// <summary>
		/// Match a numbered occurrence of a child object with the given name
		/// </summary>
		/// <param name="name">The accessible name</param>
		/// <param name="index">The 0-based index of the child to match. Ignored if &lt; 0</param>
		public AccessiblePathNode(string name, int index)
		{
			myName = name;
			myValue = null;
			myIndex = index;
		}
		/// <summary>
		/// Match a numbered occurrence of a child object with the given name and value
		/// </summary>
		/// <param name="name">The accessible name</param>
		/// <param name="value">The accessible value (can be null)</param>
		/// <param name="index">The 0-based index of the child to match. Ignored if &lt; 0</param>
		public AccessiblePathNode(string name, string value, int index)
		{
			myName = name;
			myValue = value;
			myIndex = index;
		}
		/// <summary>
		/// The accessible name of the node to match
		/// </summary>
		public string Name
		{
			get
			{
				return myName;
			}
		}
		/// <summary>
		/// The accessible value of the node to match
		/// </summary>
		public string Value
		{
			get
			{
				return myValue;
			}
		}
		/// <summary>
		/// The accessible index of the node to match
		/// </summary>
		public int Index
		{
			get
			{
				return myIndex;
			}
		}
	}
	/// <summary>
	/// A library of common testing functionality
	/// </summary>
	[CLSCompliant(false)]
	public static class CommonTestHooks
	{
		/// <summary>
		/// Find the accessible object matching the given path
		/// </summary>
		/// <param name="parent">The starting accessible object</param>
		/// <param name="nodePath">A list of AccessiblePathNode structures. Note that the
		/// easiest way to populate this is with an array of path nodes</param>
		/// <returns>Matching AccessibleObject, or null</returns>
		public static AccessibleObject FindAccessibleObject(AccessibleObject parent, IList<AccessiblePathNode> nodePath)
		{
			int pathCount = nodePath.Count;
			if (pathCount == 0)
			{
				return null;
			}
			AccessibleObject nextParent = parent;
			for (int currentNodeIndex = 0; currentNodeIndex < pathCount && nextParent != null; ++currentNodeIndex)
			{
				parent = nextParent;
				nextParent = null;
				int childCount = parent.GetChildCount();
				if (childCount != 0)
				{
					AccessiblePathNode currentNode = nodePath[currentNodeIndex];
					int testIndex = currentNode.Index;
					for (int i = 0; i < childCount; ++i)
					{
						AccessibleObject child = parent.GetChild(i);
						if (child != null &&
							child.Name == currentNode.Name &&
							(currentNode.Value == null || currentNode.Value == child.Value))
						{
							if (testIndex > 0)
							{
								--testIndex;
							}
							else
							{
								nextParent = child;
								break;
							}
						}
					}
				}
			}
			return nextParent;
		}
		/// <summary>
		/// Dump a hierarchical accessibility report to the debug output window
		/// </summary>
		/// <param name="parent">The top element to report on</param>
		public static void DumpAccessibilityReport(AccessibleObject parent)
		{
			DumpAccessibilityReport(parent, "");
		}
		/// <summary>
		/// Activate an item in the toolbox
		/// </summary>
		/// <param name="DTE">DTE object</param>
		/// <param name="tabName">The toolbox tab name</param>
		/// <param name="itemName">The item name in that tab</param>
		/// <returns>true if the item has been activated</returns>
		public static bool ActivateToolboxItem(DTE2 DTE, string tabName, string itemName)
		{
			DTE.ExecuteCommand("View.Toolbox", "");
			ToolBox toolbox = DTE.ToolWindows.ToolBox;
			ToolBoxTab tab = toolbox.ToolBoxTabs.Item(tabName);
			if (tab != null)
			{
				tab.Activate();
				ToolBoxItem foundItem = null;
				int foundItemIndex = -1;
				int selectedItemIndex = -1;
				ToolBoxItems items = tab.ToolBoxItems;
				ToolBoxItem selectedItem = items.SelectedItem;
				foreach (ToolBoxItem currentItem in items)
				{
					if (foundItem == null)
					{
						++foundItemIndex;
						if (currentItem.Name == itemName)
						{
							foundItem = currentItem;
							if (selectedItem == null)
							{
								break;
							}
						}
					}
					if (selectedItem != null)
					{
						++selectedItemIndex;
						if (selectedItem == currentItem)
						{
							selectedItem = null;
							if (foundItem != null)
							{
								break;
							}
						}
					}
				}
				if (foundItem != null)
				{
					int distance = foundItemIndex - selectedItemIndex;
					if (distance != 0)
					{
						SendKeys.Flush();
						if (distance > 0)
						{
							SendKeys.SendWait(string.Format("{{DOWN {0}}}", distance));
						}
						else
						{
							SendKeys.SendWait(string.Format("{{UP {0}}}", -distance));
						}
					}
					return true;
				}
			}
			return false;
		}
		private static void DumpAccessibilityReport(AccessibleObject parent, string indent)
		{
			Debug.WriteLine(string.Format("{0}Name: {1}, Value: {2}, Role: {3}, Description: {4}", indent, parent.Name, parent.Value, parent.Role.ToString(), parent.Description));
			int childCount = parent.GetChildCount();
			if (childCount != 0)
			{
				string childIndent = indent + "\t";
				for (int i = 0; i < childCount; ++i)
				{
					DumpAccessibilityReport(parent.GetChild(i), childIndent);
				}
			}
		}
		/// <summary>
		/// Helper class to turn the COM IServiceProvider into the .NET IServiceProvider
		/// </summary>
		private sealed class WrapSP : IServiceProvider
		{
			private IServiceProvider_COM myInnerSP;
			public WrapSP(IServiceProvider_COM inner)
			{
				myInnerSP = inner;
			}
			#region IServiceProvider Implementation
			object IServiceProvider.GetService(Type serviceType)
			{
				object retVal = null;
				IntPtr ptr;
				Guid g = serviceType.GUID;
				ErrorHandler.ThrowOnFailure(myInnerSP.QueryService(ref g, ref g, out ptr));
				if (ptr != IntPtr.Zero)
				{
					try
					{
						retVal = Marshal.GetObjectForIUnknown(ptr);
					}
					finally
					{
						Marshal.Release(ptr);
					}
				}
				return retVal;
			}
			#endregion // IServiceProvider Implementation
		}
		/// <summary>
		/// Create a managed IServiceProvider from the application object
		/// provided to the add-in
		/// </summary>
		/// <param name="applicationObject">_DTE or DTE2 instance</param>
		/// <returns>IServiceProvider</returns>
		public static IServiceProvider CreateServiceProvider(_DTE applicationObject)
		{
			return new WrapSP((IServiceProvider_COM)applicationObject);
		}

		private static DTE2 applicationObject;

		public static DTE2 DTE
		{
			get
			{
				return applicationObject;
			}
			set
			{
				applicationObject = value;
			}
		}
	}
}
