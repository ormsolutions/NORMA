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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// A base class for <see cref="UITypeEditor"/>s that want to preserve
	/// their <see cref="Control.Size"/> across multiple invocations.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class SizePreservingEditor<T> : UITypeEditor
	{
		/// <summary>
		/// Defaults to <see cref="UITypeEditorEditStyle.DropDown"/>.
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// Defaults to allowing resizing.
		/// </summary>
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Derived classes can use this field to store their <see cref="LastControlSize"/>.
		/// Each closed constructed type will have a separate copy of this field, so two derived
		/// classes can maintain their own <see cref="LastControlSize"/> by specifying different
		/// types for <typeparamref name="T"/>. See ECMA-334 §25.5.2 for further information.
		/// </summary>
		protected static Size LastControlSizeStorage;
		/// <summary>
		/// Controls the <see cref="Size"/> of the <see cref="Control"/> for a given type as it
		/// opens and closes. Can be overridden for further customization.
		/// </summary>
		protected virtual Size LastControlSize
		{
			get
			{
				return LastControlSizeStorage;
			}
			set
			{
				LastControlSizeStorage = value;
			}
		}
	}
}
