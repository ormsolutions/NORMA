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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.ORM.Framework
{
	partial class MultiDiagramDocView
	{
		private sealed class MultiDiagramDocViewControl : TabControl
		{
			#region ParentControl
			// Windows sends the WM_NOTIFY for TCN_SELCHANGING and TCN_SELCHANGE to the parent window,
			// which means we need to have one...
			private sealed class ParentControl : Control
			{
				private readonly MultiDiagramDocViewControl myDocViewControl;
				public ParentControl(MultiDiagramDocViewControl docViewControl)
				{
					myDocViewControl = docViewControl;
					base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ContainerControl | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
					base.SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint, false);
					base.TabStop = false;
					base.BackColor = Color.Transparent;
				}
				protected sealed override void OnGotFocus(EventArgs e)
				{
					base.Controls[0].Focus();
				}
				protected sealed override void OnMouseClick(MouseEventArgs e)
				{
					// Show context menu on single right click if it is not in the child's display area
					Control docViewControl;
					if (e.Button == MouseButtons.Right && e.Clicks == 1 && !(docViewControl = myDocViewControl).DisplayRectangle.Contains(e.Location))
					{
						docViewControl.ContextMenuStrip.Show(docViewControl, e.Location);
					}
					base.OnMouseClick(e);
				}
				protected sealed override Padding DefaultMargin
				{
					get
					{
						return Padding.Empty;
					}
				}
				protected sealed override void Dispose(bool disposing)
				{
					try
					{
						if (disposing)
						{
							ControlCollection controls = base.Controls;
							if (controls.Count > 0)
							{
								foreach (Control childControl in controls)
								{
									if (childControl != null)
									{
										childControl.Dispose();
									}
								}
								base.Controls.Clear();
							}
						}
					}
					finally
					{
						base.Dispose(disposing);
					}
				}
			}
			#endregion // ParentControl
			#region Constructor
			public readonly MultiDiagramDocView DocView;
			public MultiDiagramDocViewControl(MultiDiagramDocView docView)
			{
				DocView = docView;

				Control parentControl = new ParentControl(this);
				parentControl.SuspendLayout();
				base.SuspendLayout();
				parentControl.Controls.Add(this);

				base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
				base.SetStyle(ControlStyles.Selectable, false);

				base.Dock = DockStyle.Fill;
				base.Alignment = TabAlignment.Bottom;
				base.DrawMode = TabDrawMode.OwnerDrawFixed;
				base.Padding = new Point(4, 4);
				base.HotTrack = false;
				base.TabStop = false;

				Microsoft.Win32.SystemEvents.UserPreferenceChanged += SetFonts;
				SetFonts(null, null);

				base.ResumeLayout(false);
				parentControl.ResumeLayout(false);
			}
			#endregion // Constructor
			#region Properties
			#region DefaultMargin property
			protected sealed override Padding DefaultMargin
			{
				get
				{
					return System.Windows.Forms.Padding.Empty;
				}
			}
			#endregion // DefaultMargin property
			#region DisplayRectangle property
			public sealed override Rectangle DisplayRectangle
			{
				get
				{
					Rectangle displayRectangle = base.DisplayRectangle;
					// HACK: Resize the display area to get rid of the border that Windows wants to add
					displayRectangle.Inflate(displayRectangle.X, displayRectangle.Y);
					// HACK: Reduce the size of the display area, since Windows seems to make it overlap the tabs a bit
					displayRectangle.Height -= 4;
					return displayRectangle;
				}
			}
			#endregion // DisplayRectangle property
			#region SelectedDiagramTab property
			public DiagramTabPage SelectedDiagramTab
			{
				[DebuggerStepThrough]
				get
				{
					return (DiagramTabPage)base.SelectedTab;
				}
			}
			#endregion // SelectedDiagramTab property
			#region Font property
			public sealed override Font Font
			{
				get
				{
					Control parent = base.Parent;
					return (parent != null) ? parent.Font : base.Font;
				}
				set
				{
					Control parent = base.Parent;
					if (parent != null)
					{
						parent.Font = value;
					}
				}
			}
			#endregion // Font property
			#endregion // Properties
			#region SetFonts method
			private void SetFonts(object sender, EventArgs e)
			{
				IUIService uiService = DocView.GetService(typeof(IUIService)) as IUIService;
				if (uiService != null)
				{
					Font dialogFont = uiService.Styles["DialogFont"] as Font;
					if (dialogFont != null)
					{
						base.Font = dialogFont;
					}
				}
			}
			#endregion // SetFonts method
			#region Dispose method
			protected sealed override void Dispose(bool disposing)
			{
				try
				{
					if (disposing)
					{
						Microsoft.Win32.SystemEvents.UserPreferenceChanged -= SetFonts;

						ContextMenuStrip contextMenu = base.ContextMenuStrip;
						if (contextMenu != null)
						{
							contextMenu.Dispose();
						}

						TabPageCollection tabPages = base.TabPages;
						int index;
						while ((index = base.TabCount - 1) >= 0)
						{
							TabPage tabPage = tabPages[index];
							tabPages.RemoveAt(index);
							tabPage.Dispose();
						}
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
			#endregion // Dispose method
			#region OnGotFocus method
			protected sealed override void OnGotFocus(EventArgs e)
			{
				TabPage tabPage = base.SelectedTab;
				if (tabPage != null)
				{
					tabPage.Focus();
				}
			}
			#endregion // OnGotFocus method
			#region OnDoubleClick method
			protected sealed override void OnMouseDoubleClick(MouseEventArgs e)
			{
				Point point = e.Location;
				if (e.Button == MouseButtons.Left && !DisplayRectangle.Contains(point))
				{
					RenameTab(GetTabAtPoint(point));
				}
				base.OnMouseDoubleClick(e);
			}
			#endregion // OnDoubleClick method
			#region RenameTab method
			private void RenameTab(DiagramTabPage tabPage)
			{
				if (tabPage != null)
				{
					InlineTabRenameTextBox renamingTextBox = myRenamingTextBox;
					
					if (renamingTextBox != null)
					{
						if (renamingTextBox.RenamingTabPage == tabPage)
						{
							// If we already have an InlineTabRenameTextBox for this tab page, do nothing
							return;
						}
						else
						{
							// If we already have an InlineTabRenameTextBox for a different tab page, save the changes and close it
							renamingTextBox.Close(true);
						}
					}
					myRenamingTextBox = new InlineTabRenameTextBox(this, tabPage);
					base.Invalidate(false);
				}
			}
			#endregion // RenameTab method
			#region RenameTabAtPoint method
			public void RenameTabAtPoint(Point point)
			{
				RenameTab(GetTabAtPoint(base.PointToClient(point)));
			}
			#endregion // RenameTabAtPoint method
			#region OnSelectedIndexChanged method
			protected sealed override void OnSelectedIndexChanged(EventArgs e)
			{
				base.OnSelectedIndexChanged(e);
				DiagramTabPage tabPage = SelectedDiagramTab;
				if (tabPage != null)
				{
					DiagramView designer = tabPage.Designer;
					DocView.SetSelectedComponents(designer.Selection.RepresentedElements);
					designer.DiagramClientView.Focus();
				}
				else
				{
					DocView.SetSelectedComponents(null);
				}
			}
			#endregion // OnSelectedIndexChanged method
			#region IndexOf methods
			public int IndexOf(DiagramView designer, int startingIndex)
			{
				TabPageCollection tabPages = base.TabPages;
				int tabPagesCount = base.TabCount;
				for (int i = startingIndex; i < tabPagesCount; i++)
				{
					if (((DiagramTabPage)tabPages[i]).Designer == designer)
					{
						return i;
					}
				}
				return -1;
			}
			public int IndexOf(Diagram diagram, int startingIndex)
			{
				TabPageCollection tabPages = base.TabPages;
				int tabPagesCount = base.TabCount;
				for (int i = startingIndex; i < tabPagesCount; i++)
				{
					if (((DiagramTabPage)tabPages[i]).Diagram == diagram)
					{
						return i;
					}
				}
				return -1;
			}
			#endregion // IndexOf methods
			#region GetTabAtPoint method
			[System.Security.SuppressUnmanagedCodeSecurity]
			private static class UnsafeNativeMethods
			{
				#region TCHITTESTINFO struct
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
				public struct TCHITTESTINFO
				{
					// This should technically be a POINT structure, but inlining it saves us from having to create another
					// struct, and the resulting memory layout is the same.
					public int ptX;
					public int ptY;
					public uint flags;
					public TCHITTESTINFO(Point pt)
					{
						this.ptX = pt.X;
						this.ptY = pt.Y;
						this.flags = 0;
					}
				}
				#endregion // TCHITTESTINFO struct

				// Although the TCHITTESTINFO for lParam is used to return some information to us, we don't need that information.
				// Therefore, lParam is marked as [In] rather than [In, Out] so that it is not marshalled back to us.
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SendMessage([In] HandleRef hWnd, [In] uint msg, [In] IntPtr wParam, [In] ref TCHITTESTINFO lParam);
			}
			private DiagramTabPage GetTabAtPoint(Point point)
			{
				const uint TCM_HITTEST = 0x130D;
				UnsafeNativeMethods.TCHITTESTINFO hitTestInfo = new UnsafeNativeMethods.TCHITTESTINFO(point);
				int index = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), TCM_HITTEST, IntPtr.Zero, ref hitTestInfo).ToInt32();
				if (index >= 0 && index < base.TabCount)
				{
					// Make sure that our tab rectangle still contains the point after we've deflated it
					Rectangle tabRect = base.GetTabRect(index);
					tabRect.Inflate(TabOutsideBorderInflate, TabOutsideBorderInflate);
					if (tabRect.Contains(point))
					{
						return ((DiagramTabPage)base.TabPages[index]);
					}
				}
				return null;
			}
			#endregion // GetTabAtPoint method
			#region GetDesignerFromTabAtPoint method
			public DiagramView GetDesignerFromTabAtPoint(Point point)
			{
				DiagramTabPage tabPage = GetTabAtPoint(base.PointToClient(point));
				return (tabPage != null) ? tabPage.Designer : null;
			}
			#endregion // GetDesignerFromTabAtPoint method
			#region OnPaint method
			protected sealed override void OnPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;
				TabPageCollection tabPages = base.TabPages;
				int selectedIndex = base.SelectedIndex;
				int tabCount = base.TabCount;
				if (tabCount > 0)
				{
					// Draw the background behind the tabs
					Rectangle tabChannel = base.GetTabRect(0);
					tabChannel.X = 0;
					tabChannel.Width = base.Width;
					g.FillRectangle(SystemBrushes.Control, tabChannel);

					// Draw each tab
					InlineTabRenameTextBox renamingTextBox = myRenamingTextBox;
					TabPage renamingTabPage = (renamingTextBox != null) ? renamingTextBox.RenamingTabPage : null;
					for (int i = 0; i < tabCount; i++)
					{
						DrawTab(g, base.GetTabRect(i), tabPages[i], i == selectedIndex, renamingTabPage);
					}
				}
				base.OnPaint(e);
			}
			#endregion // OnPaint method
			#region DrawTab method
			private const int TabOutsideBorderInflate = -2;
			private const int TabInsideBorderInflate = -3;
			private void DrawTab(Graphics g, Rectangle bounds, TabPage tabPage, bool selected, TabPage renamingTabPage)
			{
				bounds.Inflate(TabOutsideBorderInflate, TabOutsideBorderInflate);

				// Draw the background (if selected) and border
				if (selected)
				{
					g.FillRectangle(SystemBrushes.Window, bounds);
					g.DrawRectangle(SystemPens.WindowFrame, bounds);
				}
				else
				{
					g.DrawRectangle(SystemPens.ControlDark, bounds);
				}

				bounds.Inflate(TabInsideBorderInflate, TabInsideBorderInflate);

				// Draw the image, if any
				ImageList imageList = base.ImageList;
				if (imageList != null)
				{
					int imageIndex = imageList.Images.IndexOfKey(tabPage.ImageKey);
					if (imageIndex >= 0)
					{
						int imageY = bounds.Y + ((bounds.Height - DiagramImageHeight) / 2);
						imageList.Draw(g, bounds.X, imageY, imageIndex);
						bounds.Width -= DiagramImageWidth;
						bounds.X += DiagramImageWidth;
					}
				}

				// Draw the text box for renaming or the tab name
				if (tabPage == renamingTabPage)
				{
					InlineTabRenameTextBox renamingTextBox = myRenamingTextBox;
					renamingTextBox.Bounds = bounds;
					renamingTextBox.Location = bounds.Location;
					renamingTextBox.MinimumSize = renamingTextBox.MaximumSize = renamingTextBox.Size = bounds.Size;
					if (!renamingTextBox.Visible)
					{
						renamingTextBox.Visible = true;
						renamingTextBox.Focus();
					}
					renamingTextBox.Invalidate();
				}
				else
				{
					const TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.LeftAndRightPadding;
					TextRenderer.DrawText(g, tabPage.Name, base.Font, bounds, SystemColors.WindowText, textFormatFlags);
				}
			}
			#endregion // DrawTab method
			#region Inline rename support
			private InlineTabRenameTextBox myRenamingTextBox;
			
			private sealed class InlineTabRenameTextBox : TextBox
			{
				private readonly MultiDiagramDocViewControl myDocViewControl;
				public readonly DiagramTabPage RenamingTabPage;
				
				public InlineTabRenameTextBox(MultiDiagramDocViewControl docViewControl, DiagramTabPage renamingTabPage)
				{
					myDocViewControl = docViewControl;
					RenamingTabPage = renamingTabPage;
					base.Parent = docViewControl.Parent;
					base.Visible = false;
					base.TabStop = false;
					base.Font = docViewControl.Font;
					base.BorderStyle = BorderStyle.None;
					base.SelectedText = renamingTabPage.Name;
					base.TextAlign = HorizontalAlignment.Center;
					base.BringToFront();
					base.Focus();
				}

				protected sealed override void Dispose(bool disposing)
				{
					if (disposing)
					{
						myDocViewControl.myRenamingTextBox = null;
						base.Parent = null;
					}
					base.Dispose(disposing);
				}

				private bool mySavedChanges;
				public void Close(bool saveChanges)
				{
					base.Visible = false;
					if (!base.Disposing && !base.IsDisposed)
					{
						if (!mySavedChanges && saveChanges && base.Modified)
						{
							RenamingTabPage.Text = base.Text;
							mySavedChanges = true;
						}
						if (base.Focused)
						{
							// Give focus back to the DocViewControl, which will give it back to the active designer
							myDocViewControl.Focus();
						}
						base.Dispose();
					}
				}

				protected sealed override void OnLostFocus(EventArgs e)
				{
					base.OnLostFocus(e);
					Close(true);
				}
				protected sealed override bool ProcessDialogKey(Keys keyData)
				{
					if (keyData == Keys.Enter || keyData == Keys.Escape)
					{
						Close(keyData == Keys.Enter);
						return true;
					}
					else
					{
						return base.ProcessDialogKey(keyData);
					}
				}

				protected sealed override Padding DefaultMargin
				{
					get
					{
						return Padding.Empty;
					}
				}
				public sealed override Font Font
				{
					get
					{
						MultiDiagramDocViewControl docViewControl = myDocViewControl;
						return (docViewControl != null) ? docViewControl.Font : base.Font;
					}
					set
					{
						base.Font = myDocViewControl.Font = value;
					}
				}
			}
			#endregion // Inline rename support
		}
	}
}
