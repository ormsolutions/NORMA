#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Shell;
using VsShell = Microsoft.VisualStudio.Shell.Interop;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// <see cref="ModelingEditorFactory"/> for the ORM Designer.
	/// </summary>
	[CLSCompliant(false)]
	[Guid(ORMDesignerEditorFactory.GuidString)]
	public class ORMDesignerEditorFactory : ModelingEditorFactory
	{
		// Keep in sync with PkgCmd.vsct
		/// <summary>
		/// The <see cref="String"/> form of the <see cref="Guid"/> for <see cref="ORMDesignerEditorFactory"/>.
		/// </summary>
		public const string GuidString = "EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B";
		/// <summary>
		/// The <see cref="Guid"/> for <see cref="ORMDesignerEditorFactory"/>.
		/// </summary>
		public static readonly Guid Id = new Guid(GuidString);

		#region Construction/destruction
		/// <summary>
		/// Initializes a new instance of <see cref="ORMDesignerEditorFactory"/>.
		/// </summary>
		public ORMDesignerEditorFactory(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
		#endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// This method is called before the <see cref="ModelingEditorFactory.CreateEditorInstance"/> method to allow us
		/// to map LOGICAL views to PHYSICAL ones.
		/// </summary>
		/// <remarks>
		/// Physical views are identified by a string of our choice with the one constraint that the default/primary
		/// physical view for an editor MUST use an empty string (<see cref="String.Empty"/>) or <see langword="null"/>
		/// as its physical view name.
		/// </remarks>
		/// <returns>The physical view name.</returns>
		/// <seealso cref="ModelingEditorFactory.MapLogicalView(Guid,Object)"/>
		protected override string MapLogicalView(Guid logicalView, object viewContext)
		{
			// We only support the Primary and Designer logical views. Others, like the Code logical view,
			// are handled by other editors (hence the NotImplementedException).
			if (logicalView == VSConstants.LOGVIEWID_Primary || logicalView == VSConstants.LOGVIEWID_Designer)
			{
				return null;
			}
			throw new NotImplementedException();
		}
		/// <summary>
		/// Creates an <see cref="ORMDesignerDocData"/>.
		/// See <see cref="ModelingEditorFactory.CreateDocData(String,VsShell.IVsHierarchy,UInt32)"/>.
		/// </summary>
		/// <param name="fileName">The document file.</param>
		/// <param name="hierarchy">The project/solution <see cref="VsShell.IVsHierarchy"/> to include the file in.</param>
		/// <param name="itemId">The identifier for the new item.</param>
		/// <returns>A new instance of <see cref="ORMDesignerDocData"/>.</returns>
		protected override ModelingDocData CreateDocData(string fileName, VsShell.IVsHierarchy hierarchy, uint itemId)
		{
			return new ORMDesignerDocData(this.ServiceProvider, ORMDesignerEditorFactory.Id);
		}
		/// <summary>
		/// Creates an <see cref="ORMDesignerDocView"/>. See <see cref="ModelingEditorFactory.CreateDocView"/>.
		/// </summary>
		/// <param name="docData">The document, created by <see cref="CreateDocData"/>.</param>
		/// <param name="physicalView">The name of the view to created.</param>
		/// <param name="editorCaption">The editor caption.</param>
		/// <returns>A new instance of <see cref="ORMDesignerDocView"/>.</returns>
		protected override ModelingDocView CreateDocView(ModelingDocData docData, string physicalView, out string editorCaption)
		{
			editorCaption = null;
			return new ORMDesignerDocView(docData, this.ServiceProvider);
		}
		#endregion // Base overrides
	}
}
