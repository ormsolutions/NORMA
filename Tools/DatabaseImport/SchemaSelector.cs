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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ORMSolutions.ORMArchitect.DatabaseImport
{
	/// <summary>
	/// Dialog used to allow users to select the schema the want to use.
	/// </summary>
	public partial class SchemaSelector : Form
	{
		private IList<string> mySchemas;
		private string mySelectedSchema;

		/// <summary>
		/// Static Method used to show the dialog and return the selected result.
		/// </summary>		
		/// <param name="provider">The Provider to get the service from</param>	
		/// <param name="schemaList">The list of schema names to select from</param>
		public static string SelectSchema(IServiceProvider provider, IList<string> schemaList)
		{
			string retVal = null;
            string messageResourceId = null;
			IUIService uiService = ORMDatabaseImportWizard.GetService<IUIService>(provider);
			if (uiService != null)
			{
                if (null != schemaList && schemaList.Count != 0)
                {
                    SchemaSelector fss = new SchemaSelector(schemaList);
                    if (uiService.ShowDialog(fss) == DialogResult.OK)
                    {
                        retVal = fss.mySelectedSchema;
                    }
                    else
                    {
                        messageResourceId = "SchemaNotSelectedMessage";
                    }
                }
                else
                {
                    messageResourceId = "SchemaNotAvailableMessage";
                }
                if (messageResourceId != null)
                {
                    uiService.ShowMessage(new System.ComponentModel.ComponentResourceManager(typeof(SchemaSelector)).GetString(messageResourceId));
                }
            }
			return retVal;
		}

		private SchemaSelector(IList<string> schemaList)
		{
			InitializeComponent();
			mySchemas = schemaList;
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{			
			mySelectedSchema = cbxSelectSchema.Items[cbxSelectSchema.SelectedIndex].ToString();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			foreach (string s in mySchemas)
			{
				cbxSelectSchema.Items.Add(s);
			}
			cbxSelectSchema.SelectedIndex = 0;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			mySelectedSchema = cbxSelectSchema.Items[cbxSelectSchema.SelectedIndex].ToString();
			this.DialogResult = DialogResult.Cancel;
		}
	}
}