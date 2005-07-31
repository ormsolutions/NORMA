using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// This is the concrete tool window class that the Object Model Browser uses
	/// </summary>
	[Guid("DD2334C3-AFDB-4FC5-9E8A-17D19A8CC97A")]
	[CLSCompliant(false)]
	public sealed class ORMBrowserToolWindow : ModelExplorerToolWindow
	{
		#region TreeContainer class
		/// <summary>
		/// Summary description for ORMModelExplorerTreeContainer.
		/// </summary>
		private sealed class ORMModelExplorerTreeContainer : ModelExplorerTreeContainer
		{
			#region Constructor
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="serviceProvider">Service</param>
			public ORMModelExplorerTreeContainer(IServiceProvider serviceProvider) : base(serviceProvider)
			{
			}
			#endregion
			#region ModelExplorerTreeContainer Overrides
			/// <summary>
			/// Create IElementVisitor
			/// </summary>
			/// <returns>IElementVisitor</returns>
			protected override IElementVisitor CreateElementVisitor()
			{
				return new ExplorerElementVisitor(this.ObjectModelBrowser, ServiceProvider);
			}
			protected override IList FindRootElements(Store store)
			{
				return store.ElementDirectory.GetElements(ORMModel.MetaClassGuid);
			}
			#endregion
			#region CustomVisitor
			/*
			private class CustomVisitor : ExplorerElementVisitor
			{
				public CustomVisitor(TreeView treeView, IServiceProvider serviceProvider) : base(treeView, serviceProvider)
				{
				}
				protected override void PruneTree()
				{
					PruneTree(TreeView.Nodes);
				}
				private void PruneTree(TreeNodeCollection nodes)
				{
					IList<ModelElementTreeNode> deadNodes = null;
					foreach (TreeNode childNode in nodes)
					{
						ModelElementTreeNode node;
						if (null != (node = childNode as ModelElementTreeNode))
						{
							if (node.KeepNode)
							{
								node.KeepNode = false;
								PruneTree(node.Nodes);
							}
							else
							{
								if (deadNodes == null)
								{
									deadNodes = new List<ModelElementTreeNode>();
								}
								deadNodes.Add(node);
							}
						}
					}
					if (deadNodes != null)
					{
						foreach (TreeNode killNode in deadNodes)
						{
							killNode.Remove();
						}
					}
				}
			}
			*/
			#endregion // CustomVisitor
		}
		#endregion // TreeContainer class
		#region Construction/Destruction
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serviceProvider">Service Provider</param>
		public ORMBrowserToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
		#endregion
		#region ModelExplorerToolWindow Overrides
		/// <summary>
		/// Retrieve the localized text for the model browser tool window
		/// </summary>
		public override String WindowTitle
		{
			get
			{
				return ResourceStrings.ModelBrowserWindowTitle;
			}
		}

		/// <summary>
		/// Create TreeContainer
		/// </summary>
		/// <returns>ModelExplorerTreeContainer</returns>
		protected override ModelExplorerTreeContainer CreateTreeContainer()
		{
			return new ORMModelExplorerTreeContainer(this);
		}
		#endregion
	}
}