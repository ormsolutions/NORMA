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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel.Design
{
	/// <summary>
	/// Shows the editor for the model error filter.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ModelErrorDisplayFilterEditor : UITypeEditor
	{
		//public static readonly ModelErrorDisplayFilterEditor Instance = new ModelErrorDisplayFilterEditor();
		//private ModelErrorDisplayFilterEditor()
		//    : base()
		//{
		//}

		/// <summary>
		/// Edit the property with a modal dialog.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public sealed override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
		/// <summary>
		/// Shows a modal dialog to edit the property.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public sealed override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context != null)
			{
				ORMModel element = EditorUtility.ResolveContextInstance(context.Instance, false) as ORMModel;
				if (element != null)
				{
					IUIService uiService;
					if (null != (provider = (element.Store as IORMToolServices).ServiceProvider) &&
							null != (uiService = (IUIService)provider.GetService(typeof(IUIService))))
					{
						uiService.ShowDialog(new ModelErrorDisplayFilterForm(element));
					}
				}
			}
			return value;
		}
	}
}
