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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ObjectModel.Design
{
	/// <summary>
	/// Type editor for use on ReadingText properties in the property grid.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ReadingTextEditor : UITypeEditor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReadingTextEditor()
		{
		}

		/// <summary>
		/// Changes the style of the editor.
		/// </summary>
		public sealed override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		/// <summary>
		/// Called when a value using this editor is modified.
		/// </summary>
		public sealed override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider.GetService(typeof(IWindowsFormsEditorService)) != null)
			{
				FactType fact = ORMEditorUtility.ResolveContextFactType(context.Instance);
				if (fact != null)
				{
					ORMDesignerPackage.ReadingEditorWindow.Show();
				}
			}
			return value;
		}
	}
}
