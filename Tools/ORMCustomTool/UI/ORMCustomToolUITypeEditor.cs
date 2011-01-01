#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.VirtualTreeGrid;
using VSLangProj;
using Microsoft.VisualStudio;
using EnvDTE;
using EnvDTE80;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	public sealed partial class Extender
	{
		private sealed partial class ORMCustomToolPropertyDescriptor : PropertyDescriptor
		{
			public sealed class ORMCustomToolUITypeEditor : UITypeEditor
			{
				public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
				{
					return UITypeEditorEditStyle.Modal;
				}

				[DllImport("ole32.dll")]
				private static extern int GetRunningObjectTable(
					int reserved,
					out IRunningObjectTable prot);
				[DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
				private static extern int CreateItemMoniker(
					string lpszDelim,
					string lpszItem,
					out IMoniker ppmk);
				public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
				{
					IWindowsFormsEditorService windowsFormsEditorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
					if (windowsFormsEditorService != null)
					{
						FileProperties fileProperties = context.Instance as FileProperties;
						if (fileProperties != null)
						{
							IRunningObjectTable rot;
							int procId = System.Diagnostics.Process.GetCurrentProcess().Id;
							ErrorHandler.ThrowOnFailure(GetRunningObjectTable(0, out rot));
							IMoniker moniker;
							ErrorHandler.ThrowOnFailure(CreateItemMoniker(
								"!",
#if VISUALSTUDIO_10_0
								"VisualStudio.DTE.10.0:" +
#elif VISUALSTUDIO_9_0
								"VisualStudio.DTE.9.0:" +
#else
								"VisualStudio.DTE.8.0:" + 
#endif
								procId.ToString(),
								out moniker));
							object objDTE;
							ErrorHandler.ThrowOnFailure(rot.GetObject(moniker, out objDTE));
							DTE2 dte = (DTE2)objDTE;
							SelectedItems items = dte.SelectedItems;
							if (items.Count == 1)
							{
								ProjectItem projectItem = items.Item(1).ProjectItem;
								if (projectItem != null)
								{
									windowsFormsEditorService.ShowDialog(new ORMGeneratorSelectionControl(projectItem, provider));
								}
							}
						}
					}
					return null;
				}
				public override bool GetPaintValueSupported(ITypeDescriptorContext context)
				{
					return false;
				}
				public override void PaintValue(PaintValueEventArgs e)
				{
					// Don't do anything, since we don't have a visual representation.
				}
			}
		}
	}
}
