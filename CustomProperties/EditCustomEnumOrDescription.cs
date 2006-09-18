using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.CustomProperties
{
	public partial class EditCustomEnumOrDescription : Form
	{
		#region Fields
		private bool _allowNewLineCharacters;
		#endregion
		#region Constructors
		public EditCustomEnumOrDescription()
		{
			InitializeComponent();
		}
		#endregion
		#region Event Handlers
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void tbxValues_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!_allowNewLineCharacters && e.KeyChar == '\r')
			{
				e.Handled = true;
			}
		}
		private void tbxValues_TextChanged(object sender, EventArgs e)
		{
			if (!_allowNewLineCharacters)
			{
				tbxValues.Text = tbxValues.Text.Replace(Environment.NewLine, string.Empty);
			}
		} 
		private void tbxValues_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return && e.Control)
			{
				btnOK.PerformClick();
			}
		}
		#endregion
		#region Methods
		/// <summary>
		/// Brings up a UI for the user to edit a cusotm enum.
		/// </summary>
		/// <param name="enumValues">The existing collection of Enum values to edit.</param>
		/// <returns>True if the user accepted changes to the Enum values.</returns>
		public static bool EditEnum(List<string> enumValues)
		{
			EditCustomEnumOrDescription frm = new EditCustomEnumOrDescription();
			frm.Text = "Edit Custom Enum";
			frm._allowNewLineCharacters = true;
			foreach (string enm in enumValues)
			{
				frm.tbxValues.Text += enm + Environment.NewLine;
			}
			if (frm.tbxValues.TextLength > 0)
			{
				frm.tbxValues.Text = frm.tbxValues.Text.Substring(0, frm.tbxValues.Text.LastIndexOf(Environment.NewLine));
			}
			DialogResult rslt = frm.ShowDialog();
			if (rslt == DialogResult.OK)
			{
				enumValues.Clear();
				foreach (string line in frm.tbxValues.Lines)
				{
					enumValues.Add(line);
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Brings up a UI for the user to edit a description.
		/// </summary>
		/// <param name="desc">The existing description to edit.</param>
		/// <returns>True if the user accepted changes to the description.</returns>
		public static bool EditDescription(ref string desc)
		{
			EditCustomEnumOrDescription frm = new EditCustomEnumOrDescription();
			frm.Text = "Edit Description";
			frm._allowNewLineCharacters = false;
			frm.tbxValues.Text = desc;
			if (frm.ShowDialog() == DialogResult.OK)
			{
				desc = frm.tbxValues.Text;
				return true;
			}
			return false;
		}
		#endregion
	}
}