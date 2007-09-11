using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication2
{
	public partial class schemaForm : Form
	{
		public schemaForm(DataSet schema)
		{
			InitializeComponent();

			System.Text.StringBuilder rtf = new StringBuilder();
			rtf.Append("{\\rtf1");

			foreach (DataTable table in schema.Tables)
			{
				rtf.Append("\\i ");
				rtf.Append(table.TableName);
				rtf.Append("\\i0  ( ");
				DataColumnCollection columns = table.Columns;

				for (int i = 0; i < columns.Count; ++i)
				{
					if (i != 0)
						rtf.Append(", ");
					rtf.Append(columns[i].ColumnName);
				}
				rtf.Append(" ) \\line ");
			}

			rtf.Append("}");

			this.richTextBox1.Rtf = rtf.ToString();
		}
	}
}