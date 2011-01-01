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

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	// UNDONE: We should take a look at the built-in MultilineStringEditor to see if it does what we need.
	/// <summary>
	/// A base class used to display text in a multi-line text box control.
	/// This editor and derived classes can be directly applied to any string
	/// property. Overriding the LastControlSize property allows dropdown
	/// size to be control for each editor implementation.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class MultilineTextEditor<T> : SizePreservingEditor<T>
	{
		#region TextBoxControl class. Handles Escape key for TextBox
		private sealed class TextBoxControl : TextBox, INotifyEscapeKeyPressed
		{
			private EventHandler myEscapePressed;
			/// <summary>
			/// Set the appropriate TextBox styles
			/// </summary>
			public TextBoxControl()
				: base()
			{
				Multiline = true;
				ScrollBars = ScrollBars.Vertical;
				BorderStyle = BorderStyle.None;
			}
			protected sealed override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					EventHandler escapePressed;
					if (null != (escapePressed = myEscapePressed))
					{
						escapePressed(this, EventArgs.Empty);
					}
				}
				return base.IsInputKey(keyData);
			}
			event EventHandler INotifyEscapeKeyPressed.EscapePressed
			{
				add { myEscapePressed += value; }
				remove { myEscapePressed -= value; }
			}
		}
		#endregion // TextBoxControl class. Handles Escape key for TextBox
		#region UITypeEditor overrides
		/// <summary>
		/// The default <see cref="Size.Width"/> used for the <see cref="SizePreservingEditor{T}.LastControlSize"/>
		/// property the first time the control is shown.
		/// </summary>
		protected const int DefaultInitialControlWidth = 272;
		/// <summary>
		/// The default <see cref="Size.Height"/> used for the <see cref="SizePreservingEditor{T}.LastControlSize"/>
		/// property the first time the control is shown.
		/// </summary>
		protected const int DefaultInitialControlHeight = 128;
		/// <summary>
		/// Set the default <see cref="Size"/> used for the <see cref="SizePreservingEditor{T}.LastControlSize"/>
		/// property the first time the control is shown.
		/// </summary>
		static MultilineTextEditor()
		{
			LastControlSizeStorage = new Size(DefaultInitialControlWidth, DefaultInitialControlHeight);
		}
		/// <summary>
		/// Required UITypeEditor override. Opens dropdown modally
		/// and waits for user input.
		/// </summary>
		/// <param name="context">The descriptor context. Used to retrieve
		/// the live instance and other data.</param>
		/// <param name="provider">The service provider for the given context.</param>
		/// <param name="value">The current property value</param>
		/// <returns>The updated property value, or the orignal value to effect a cancel</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editor = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			if (editor != null)
			{
				string newText = value as string ?? string.Empty;

				// Create a textbox with its events
				using (TextBoxControl textControl = new TextBoxControl())
				{
					// Manage the size of the control
					Size lastSize = LastControlSize;
					if (!lastSize.IsEmpty)
					{
						textControl.Size = lastSize;
					}

					textControl.Text = newText;
					textControl.Select(0, 0);

					bool escapePressed = false;
					EditorUtility.AttachEscapeKeyPressedEventHandler(
						textControl,
						delegate(object sender, EventArgs e)
						{
							escapePressed = true;
						});
					editor.DropDownControl(textControl);

					// Record the final size, we'll use it next time for this type of control
					LastControlSize = textControl.Size;

					// Make sure the user didn't cancel
					if (!escapePressed)
					{
						newText = textControl.Text;
					}
				}
				return newText;
			}
			return value;
		}
		#endregion // UITypeEditor overrides
	}
}
