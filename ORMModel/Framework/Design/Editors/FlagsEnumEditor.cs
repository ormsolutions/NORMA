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
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	/// <summary>
	/// A <see cref="UITypeEditor"/> for editing <see cref="Enum"/> values that have the <see cref="FlagsAttribute"/>
	/// applied to them.
	/// </summary>
	/// <remarks>
	/// This <see cref="UITypeEditor"/> displays an instance of <see cref="FlagsEnumListBox"/> as a drop-down dialog box.
	/// </remarks>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class FlagsEnumEditor : SizePreservingEditor<FlagsEnumEditor>
	{
		#region FlagsEnumUI class
		private sealed class FlagsEnumUI : FlagsEnumListBox
		{
			private readonly object _originalValue;
			public FlagsEnumUI(object enumValue)
			{
				base.IntegralHeight = false;
				base.CheckOnClick = true;
				this.Value = this._originalValue = enumValue;
			}

			protected sealed override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					this.Value = this._originalValue;
				}
				return base.ProcessDialogKey(keyData);
			}
		}
		#endregion // FlagsEnumUI class

		/// <summary>
		/// Initializes a new instance of <see cref="FlagsEnumEditor"/>.
		/// </summary>
		public FlagsEnumEditor()
			: base()
		{
		}

		/// <summary>See <see cref="UITypeEditor.EditValue(ITypeDescriptorContext,IServiceProvider,Object)"/>.</summary>
		public sealed override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null && value != null)
			{
				Type enumType = value.GetType();
				if (enumType.IsEnum && enumType.IsDefined(typeof(FlagsAttribute), false))
				{
					IWindowsFormsEditorService editor = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
					if (editor != null)
					{
						using (FlagsEnumUI flagsEnumUI = new FlagsEnumUI(value))
						{
							editor.DropDownControl(flagsEnumUI);
							value = flagsEnumUI.Value;
						}
					}
				}
			}
			return value;
		}
	}
}
