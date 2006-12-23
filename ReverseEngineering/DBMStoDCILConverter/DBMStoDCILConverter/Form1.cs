using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DBMStoDCILConverter
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			int itemCount = listView1.Items.Count;
			for (int i = 0; i < itemCount; ++i)
			{
				listView1.Items[i].Checked = true;
			}
		}

		private void btnSelectNone_Click(object sender, EventArgs e)
		{
			int itemCount = listView1.Items.Count;
			for (int i = 0; i < itemCount; ++i)
			{
				listView1.Items[i].Checked = false;
			}
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			FileStream output = File.Open("test.xml", FileMode.Create, FileAccess.Write);
			XmlWriter writer = XmlWriter.Create(output, settings);
			SQLServer2005_DCILSchemaProvider sql = new SQLServer2005_DCILSchemaProvider(new System.Data.SqlClient.SqlConnection(""));
			DCILSchema schema = new DCILSchema(sql, "dbo");
			DCILSchema.Serialize(schema, writer);
			writer.Flush();
			Application.Exit();
		}
	}
}