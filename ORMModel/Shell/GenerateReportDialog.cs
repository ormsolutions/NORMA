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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Verbalization;
using ORMSolutions.ORMArchitect.Core.ShapeModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// Provides a configuration for the Verbalization Report Generator
	/// </summary>
	public partial class GenerateReportDialog : Form
	{
		private ORMModel myModel;
		/// <summary>
		/// Initializes a new instance of GenerateReportDialog
		/// </summary>
		public GenerateReportDialog(ORMModel model)
		{
			InitializeComponent();
			myModel = model;
			chkLbOptions.Sorted = true;
			chkLbOptions.Value = VerbalizationReportContent.All;
		}
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				txtOutputDirectory.Text = folderBrowserDialog1.SelectedPath;
			}
		}
		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (Directory.Exists(txtOutputDirectory.Text))
			{
				VerbalizationReportContent reportContent = 0;
				CheckedListBox.CheckedItemCollection checkedItems = chkLbOptions.CheckedItems;
				int itemCount = checkedItems.Count;
				for (int i = 0; i < itemCount; ++i)
				{
					reportContent |= (VerbalizationReportContent)checkedItems[i];
				}
				ObjectModel.Verbalization.VerbalizationReportGenerator.GenerateReport(myModel, reportContent, txtOutputDirectory.Text);
				System.Diagnostics.Process.Start(txtOutputDirectory.Text);
				DialogResult = DialogResult.OK;
			}
			else DialogResult = DialogResult.None;
		}
		private void btnCancel_Click(object sender, EventArgs e) { }
		private void txtOutputDirectory_TextChanged(object sender, EventArgs e)
		{
			btnGenerate.Enabled = Directory.Exists(txtOutputDirectory.Text);
		}
		private void lblOutput_Click(object sender, EventArgs e) { }
	}
}
