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
using System.Diagnostics;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.ComponentModel;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Neumont.Tools.Modeling.Shell
{
	/// <summary>
	/// A version of <see cref="VirtualTreeControl"/> with overrides
	/// for common bug fixes.
	/// </summary>
	public class StandardVirtualTreeControl : VirtualTreeControl
	{
		#region NativeMethods class
		[System.Security.SuppressUnmanagedCodeSecurity]
		private sealed class NativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
			public const int LB_GETITEMHEIGHT = 0x1A1;
		}
		#endregion // NativeMethods class
		#region Properties
		/// <summary>
		/// Get the item height
		/// </summary>
		protected int ItemHeight
		{
			get
			{
				return NativeMethods.SendMessage(new HandleRef(this, Handle), NativeMethods.LB_GETITEMHEIGHT, 0, 0);
			}
		}
		#endregion // Properties
		#region Base Overrides
		/// <summary>
		/// Make sure a newly created control has no selection.
		/// Overrides <see cref="VirtualTreeControl.OnHandleCreated"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			int setIndex = InitialIndex;
			if (setIndex < 0)
			{
				AnchorIndex = -1;
			}
			else
			{
				CurrentIndex = setIndex;
			}
		}
		/// <summary>
		/// Override to provide an initial selection index
		/// </summary>
		protected virtual int InitialIndex
		{
			get
			{
				return -1;
			}
		}
		/// <summary>
		/// Handle PageUp/PageDown/Home/End for a single column tree control.
		/// Extension of <see cref="VirtualTreeControl.OnKeyDown"/>
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			// MSBUG: VirtualTreeControl does not handle PageUp/PageDown/Home/End for a single-column tree
			ITree tree;
			int visibleCount;
			if (MultiColumnTree == null &&
				null != (tree = Tree) &&
				0 < (visibleCount = tree.VisibleItemCount))
			{
				Keys keyCode = e.KeyCode;
				int newIndex = -1;
				int newTopIndex = -1;
				int currentIndex;
				int fullyVisible;
				int offsetBy;
				switch (keyCode)
				{
					case Keys.PageDown:
					case Keys.End:
						currentIndex = CurrentIndexCheckAnchor;
						fullyVisible = ClientSize.Height / ItemHeight;
						offsetBy = (keyCode == Keys.PageDown) ? fullyVisible : visibleCount;
						if (offsetBy <= 1)
						{
							offsetBy = 1;
							newIndex = currentIndex + 1;
						}
						else
						{
							newIndex = currentIndex + offsetBy - 1;
						}
						if (newIndex >= visibleCount)
						{
							newIndex = visibleCount - 1;
						}
						if (newIndex == currentIndex)
						{
							if (currentIndex == -1)
							{
								newIndex = 0;
							}
							e.Handled = true;
						}
						if (newIndex != -1)
						{
							newTopIndex = Math.Max(0, (newIndex - fullyVisible) + 1);
						}
						break;
					case Keys.PageUp:
					case Keys.Home:
						currentIndex = CurrentIndexCheckAnchor;
						fullyVisible = ClientSize.Height / ItemHeight;
						offsetBy = (keyCode == Keys.PageUp) ? fullyVisible : visibleCount;
						if (offsetBy <= 1)
						{
							offsetBy = 1;
							newIndex = currentIndex - 1;
						}
						else
						{
							newIndex = currentIndex - offsetBy + 1;
						}
						if (newIndex < 0)
						{
							newIndex = 0;
						}
						if (newIndex == currentIndex)
						{
							if (currentIndex == -1)
							{
								newIndex = 0;
							}
							e.Handled = true;
						}
						else
						{
							newTopIndex = newIndex;
						}
						break;
				}
				if (newIndex != -1)
				{
					if (SelectionMode == SelectionMode.MultiExtended)
					{
						SetCurrentExtendedMultiSelectIndex(newIndex, e.Shift, e.Control, ModifySelectionAction.None);
					}
					else
					{
						CurrentIndex = newIndex;
					}
					if (newTopIndex != -1)
					{
						TopIndex = newTopIndex;
					}
					e.Handled = true;
				}
				if (e.Handled)
				{
					return;
				}
			}
			base.OnKeyDown(e);
		}
		/// <summary>
		/// Override the setter to <see cref="VirtualTreeControl.TopIndex"/> to stop crash in <see cref="VirtualTreeControl.OnKeyDown"/>
		/// </summary>
		public override int TopIndex
		{
			get
			{
				return base.TopIndex;
			}
			set
			{
				// MSBUG: The OnKeyDown implementation of PageDown has a -1
				// instead of a +1 when the TopIndex is adjusted. Account for this issue.
				// Note that this far from perfect, but it does fix the bug.
				base.TopIndex = value < 0 ? value + 2 : value;
			}
		}
		#endregion // Base Overrides
	}
}