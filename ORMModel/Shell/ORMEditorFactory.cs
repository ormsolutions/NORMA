using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using VsShell = Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Editor factory for ORM Designer. 
	/// </summary>
	[Guid(ORMDesignerEditorFactory.GuidString)]
	[CLSCompliant(false)]
	public class ORMDesignerEditorFactory : ModelingEditorFactory
	{
		/// <summary>
		/// The <see cref="String"/> form of the <see cref="Guid"/> for <see cref="ORMDesignerEditorFactory"/>.
		/// </summary>
		public const string GuidString = "EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B";

        #region Construction/destruction
		/// <summary>
		/// Public constructor for our editor factory.
		/// </summary>
		public ORMDesignerEditorFactory(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
        #endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// This method is called before the EditorFactory.CreateEditorInstance method to allow us to map LOGICAL views to PHYSICAL ones.  Our Editor Factory supports unlimited physical views.
		/// NOTE: Physical views are identified by a string of our choice with the one constraint that the default/primary physical view for an editor *MUST* use an empty ("") string as its physical view name (return "").
		/// </summary>
		/// <param name="logicalView">Guid</param>
		/// <param name="viewContext">Context</param>
		/// <returns>The physical view name</returns>
		protected override string MapLogicalView(Guid logicalView, object viewContext)
		{
			return "";
		}
		/// <summary>
		/// Standard override. Create an ORMDesignerDocData
		/// </summary>
		/// <param name="fileName">The document file</param>
		/// <param name="hierarchy">The project/solution hierarchy to include the file in</param>
		/// <param name="itemId">The identifier for the new item</param>
		/// <returns>ORMDesignerDocData</returns>
		protected override DocData CreateDocData(string fileName, VsShell.IVsHierarchy hierarchy, uint itemId)
		{
			return new ORMDesignerDocData(this.ServiceProvider, this);
		}
		/// <summary>
		/// Create a view on an ORMDesignerDocData
		/// </summary>
		/// <param name="docData">The document, created by CreateDocData</param>
		/// <param name="physicalView">The name of the view to created</param>
		/// <param name="editorCaption">The editor caption</param>
		/// <returns>ORMDesignerDocView</returns>
		protected override DocView CreateDocView(DocData docData, string physicalView, out string editorCaption)
		{
			editorCaption = "";
			return new ORMDesignerDocView(docData, this.ServiceProvider);
		}
		/// <summary>
		/// Retrieve toolbox items. Called during devenv /setup or
		/// toolbox refresh. Uses standard prototype settings (mostly
		/// created in ORMDiagram.InitializeToolboxItem) and adds additional
		/// filter strings as required.
		/// </summary>
		/// <returns>ModelingToolboxItem[]</returns>
		public override ModelingToolboxItem[] GetToolboxItems()
		{
			ModelingToolboxItem[] items = null;
			ORMMetaModel.InitializingToolboxItems = false;
			try
			{
				items = base.GetToolboxItems();
			}
			finally
			{
				ORMMetaModel.InitializingToolboxItems = true;
			}

			// Build up a dictionary of items so we can add filter strings. This is
			// much easier than trying to maintain all of the filter strings at the ims level,
			// which would require elements with different filter string sets to be placed on different
			// ims elements.
			int itemCount = items.Length;
			Dictionary<string, int> itemIndexDictionary = new Dictionary<string, int>(itemCount);
			for (int i = 0; i < itemCount; ++i)
			{
				itemIndexDictionary[items[i].Id] = i;
			}

			ToolboxItemFilterAttribute attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectInternalUniquenessConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxInternalUniquenessConstraintConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectExternalConstraintFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxExternalConstraintConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramConnectRoleFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxRoleConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramCreateSubtypeFilterString, ToolboxItemFilterType.Allow);
			AddFilterAttribute(items, itemIndexDictionary, ResourceStrings.ToolboxSubtypeConnectorItemId, attribute);

			attribute = new ToolboxItemFilterAttribute(ORMDiagram.ORMDiagramExternalConstraintFilterString, ToolboxItemFilterType.Allow);
			string[] itemIds = {
				ResourceStrings.ToolboxEqualityConstraintItemId,
				ResourceStrings.ToolboxExclusionConstraintItemId,
				ResourceStrings.ToolboxExclusiveOrConstraintItemId,
				ResourceStrings.ToolboxExternalUniquenessConstraintItemId,
				ResourceStrings.ToolboxInclusiveOrConstraintItemId,
				ResourceStrings.ToolboxRingConstraintItemId,
				ResourceStrings.ToolboxSubsetConstraintItemId,
				ResourceStrings.ToolboxFrequencyConstraintItemId
			};
			int idCount = itemIds.Length;
			for (int i = 0; i < idCount; ++i)
			{
				AddFilterAttribute(items, itemIndexDictionary, itemIds[i], attribute);
			}
			return items;
		}
		/// <summary>
		/// Add a filter string to the specified ModelingToolboxItem
		/// </summary>
		/// <param name="items">An array of existing items</param>
		/// <param name="itemIndexDictionary">A dictionary mapping from the item name
		/// to an index in the items array</param>
		/// <param name="itemId">The name of the item to modify</param>
		/// <param name="attribute">The filter attribute to add</param>
		private static void AddFilterAttribute(ModelingToolboxItem[] items, Dictionary<string, int> itemIndexDictionary, string itemId, ToolboxItemFilterAttribute attribute)
		{
			int itemIndex;
			if (itemIndexDictionary.TryGetValue(itemId, out itemIndex))
			{
				ModelingToolboxItem itemBase = items[itemIndex];
				ICollection baseFilters = itemBase.Filter;
				int baseFilterCount = baseFilters.Count;
				ToolboxItemFilterAttribute[] newFilters = new ToolboxItemFilterAttribute[baseFilterCount + 1];
				baseFilters.CopyTo(newFilters, 0);
				newFilters[baseFilterCount] = attribute;
				items[itemIndex] = new ModelingToolboxItem(
					itemBase.Id,
					itemBase.Position,
					StringNotNull(itemBase.DisplayName),
					itemBase.Bitmap,
					StringNotNull(itemBase.TabNameId),
					StringNotNull(itemBase.TabName),
					StringNotNull(itemBase.ContextSensitiveHelpKeyword),
					StringNotNull(itemBase.Description),
					itemBase.Prototype,
					newFilters);
			}
		}
		private static string StringNotNull(string testString)
		{
				return (testString != null) ? testString : "";
		}
		#endregion // Base overrides
	}
} 