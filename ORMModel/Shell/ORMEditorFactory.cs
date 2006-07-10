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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Shell;
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
		// Keep in sync with PkgCmd.ctc
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
			return String.Empty;
		}
		/// <summary>
		/// Standard override. Create an ORMDesignerDocData
		/// </summary>
		/// <param name="fileName">The document file</param>
		/// <param name="hierarchy">The project/solution hierarchy to include the file in</param>
		/// <param name="itemId">The identifier for the new item</param>
		/// <returns>ORMDesignerDocData</returns>
		protected override ModelingDocData CreateDocData(string fileName, VsShell.IVsHierarchy hierarchy, uint itemId)
		{
			return new ORMDesignerDocData(this.ServiceProvider, Id);
		}
		/// <summary>
		/// Create a view on an ORMDesignerDocData
		/// </summary>
		/// <param name="docData">The document, created by CreateDocData</param>
		/// <param name="physicalView">The name of the view to created</param>
		/// <param name="editorCaption">The editor caption</param>
		/// <returns>ORMDesignerDocView</returns>
		protected override ModelingDocView CreateDocView(ModelingDocData docData, string physicalView, out string editorCaption)
		{
			editorCaption = String.Empty;
			return new ORMDesignerDocView(docData, this.ServiceProvider);
		}
		#endregion // Base overrides
	}
}
