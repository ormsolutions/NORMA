#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	/// <summary>
	/// Display an upgrade message dialog
	/// </summary>
	[CLSCompliant(false)]
	public sealed partial class UpgradeMessageDialog : Form
	{
		private IList<Tuple<string, Action>> myMessages;
		private int myCurrentMessage;
		private UpgradeMessageDialog(IList<Tuple<string, Action>> messages)
		{
			myMessages = messages;
			InitializeComponent();
			myCurrentMessage = 0;
		}

		/// <summary>
		/// Display the diagram order dialog with the specified diagram order and update
		/// the diagram order as specified by the user.
		/// </summary>
		/// <param name="serviceProvider">A <see cref="IServiceProvider"/> used to parent the dialog</param>
		/// <param name="messages">A list of message strings (as html) and actions to take when the message is cleared.</param>
		public static void ShowDialog(IServiceProvider serviceProvider, IList<Tuple<string, Action>> messages)
		{
			UpgradeMessageDialog messageDialog = new UpgradeMessageDialog(messages);
			messageDialog.ShowDialog(Utility.GetDialogOwnerWindow(serviceProvider));
		}

		private void UpgradeMessageDialog_Load(object sender, EventArgs e)
		{
			browser.DocumentText = myMessages[myCurrentMessage].Item1;
		}

		private void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			Uri uri = e.Url;
			string scheme = uri.Scheme.ToLower();
			if (scheme == "http" || scheme == "https")
			{
				Process.Start(uri.OriginalString);
				e.Cancel = true;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			myMessages[myCurrentMessage].Item2();
			++myCurrentMessage;
			if (myCurrentMessage >= myMessages.Count)
			{
				this.Close();
			}
			else
			{
				browser.DocumentText = myMessages[myCurrentMessage].Item1;
			}
		}

		private void btnLeaveUnread_Click(object sender, EventArgs e)
		{
			++myCurrentMessage;
			if (myCurrentMessage >= myMessages.Count)
			{
				this.Close();
			}
			else
			{
				browser.DocumentText = myMessages[myCurrentMessage].Item1;
			}
		}
	}
}