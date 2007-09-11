using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication2
{
	public partial class NORMA_Name_Generation : Form
	{
		const int targetMax = 128;

		public NORMA_Name_Generation()
		{
			columnAbbreviations = new Dictionary<string, string>();
			tableAbbreviations = new Dictionary<string, string>();

			columnSpace = "";
			tableSpace = "";
			tableCase = Generator.Case.Pascal;
			columnCase = Generator.Case.Pascal;
			shortenColumn = false;
			shortenTable = false;
			columnMaxLength = targetMax;
			tableMaxLength = targetMax;

			InitializeComponent();
			RestoreDefaults();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			StoreCurrent(cbxTargetNameType.SelectedIndex);
			DataSet schema = Generator.GenerateNames(this);
			new schemaForm(schema).Show();
		}
		private void btnDefaults_Click(object sender, EventArgs e)
		{
			RestoreDefaults();
		}
		private void RestoreDefaults()
		{
			//General
			this.lbxOmitWords.Items.Clear();
			this.lbxOmitWords.Items.AddRange(new string[] { "has", "a", "an", "the" });
			this.txtOmitWord.Clear();
			this.txtSubjectArea.Clear();
			this.rbPrefixNone.Checked = true;
			this.rbSubjectAreaNone.Checked = true;
			this.txtConstraintNames.Text = "{s}_FK{n}";

			//Abbreviations
			this.dgvGlobalAbbreviations.Rows.Clear();
			this.dgvGlobalAbbreviations.Rows.Add("Vehicle Identification Number", "VIN");
			this.dgvGlobalAbbreviations.Rows.Add("International Standard Book Number", "ISBN");

			//Targets
			this.rbLongWarn.Checked = true;
			this.rbLongTargetMax.Checked = true;
			this.txtLongMax.Text = targetMax.ToString();
			this.rbCasePascal.Checked = true;
			this.rbSpaceRemove.Checked = true;
			this.txtSpaceChar.Text = "_";
			this.cbxTargetBaseType.Items.Clear();
			this.cbxTargetBaseType.Items.AddRange(new string[] { "Relational" });
			this.cbxTargetBaseType.SelectedIndex = 0;
			this.cbxTargetNameType.Items.Clear();
			this.cbxTargetNameType.Items.AddRange(new string[] { "Table", "Column" });
			this.cbxTargetNameType.SelectedIndex = 1;
			this.cbxTargetSpecificType.Items.Clear();
			this.cbxTargetSpecificType.Items.AddRange(new string[] { "SQL Server 2005" });
			this.cbxTargetSpecificType.SelectedIndex = 0;

			//Supertypes
			this.lbxSupertypesIncluded.Items.Clear();
			this.lbxSupertypesIncluded.Items.AddRange(new string[] { });
			this.lbxSupertypesExcluded.Items.Clear();
			this.lbxSupertypesExcluded.Items.AddRange(new string[] { "Employee" });

			//Composite IDs
			this.lbxCompositeObjects.Items.Clear();
			this.lbxCompositeObjects.Items.AddRange(new string[] { "Room", "Building" });
			this.lbxCompositeIncluded.Items.Clear();
			this.lbxCompositeExcluded.Items.Clear();
		}

		#region Composite IDs
		private void btnCompositeExclude_Click(object sender, EventArgs e)
		{
			MoveItem(lbxCompositeIncluded, lbxCompositeExcluded);
			CompositeMoved();
		}
		private void btnCompositeInclude_Click(object sender, EventArgs e)
		{
			MoveItem(lbxCompositeExcluded, lbxCompositeIncluded);
			CompositeMoved();
		}
		private void CompositeMoved()
		{
			if (lbxCompositeObjects.SelectedIndex == 0)
			{
				StoreRoom();
			}
			else
			{
				StoreBuilding();
			}

			RefreshCompositeExamples();
		}
		private void RefreshCompositeExamples()
		{
			lbxCompositeExamples.Items.Clear();

			string[] s = new string[] { "CampusId", "BuildingNr", "RoomNr" };
			string start = "";
			if (lbxCompositeIncluded.Items.Contains("Room"))
				start = "office";
			if (lbxCompositeIncluded.Items.Contains("Building"))
				s[0] = "Building" + s[0];

			for (int i = 0; i < 3; i++)
			{
				lbxCompositeExamples.Items.Add(start + s[i]);
			}
		}
		#endregion //Composite IDs

		#region General
		private bool editOmitWord = false;
		private void btnOmitWordsEdit_Click(object sender, EventArgs e)
		{
			string editText = lbxOmitWords.SelectedItem as string;
			if (editText != null)
			{
				editOmitWord = false;
				txtOmitWord.Text = editText;
				editOmitWord = true;
			}
		}
		private void btnOmitWordsRemove_Click(object sender, EventArgs e)
		{
			int index = lbxOmitWords.SelectedIndex;
			if (index > -1)
			{
				lbxOmitWords.Items.RemoveAt(index);
			}
		}
		private void btnOmitWordsAdd_Click(object sender, EventArgs e)
		{
			string newItem = txtOmitWord.Text;
			lbxOmitWords.Items.Add(newItem);
			lbxOmitWords.SelectedItem = newItem;
			editOmitWord = true;
		}
		private void txtOmitWord_TextChanged(object sender, EventArgs e)
		{
			if (editOmitWord)
			{
				int index = lbxOmitWords.SelectedIndex;
				if (index > -1)
				{
					lbxOmitWords.Items[index] = txtOmitWord.Text;
					editOmitWord = true;
				}
				else
				{
					editOmitWord = false;
				}
			}
		}
		private void lbxOmitWords_SelectedIndexChanged(object sender, EventArgs e)
		{
			editOmitWord = false;
		}
		private void rbSubjectAreaNone_CheckedChanged(object sender, EventArgs e)
		{
			this.txtSubjectArea.ReadOnly = this.rbSubjectAreaNone.Checked;
		}
		#endregion //General

		#region Targets
		private void rbSpaceReplace_CheckedChanged(object sender, EventArgs e)
		{
			this.txtSpaceChar.ReadOnly = !(this.rbSpaceReplace.Checked);
		}
		private void rbLongTargetMax_CheckedChanged(object sender, EventArgs e)
		{
			this.txtLongMax.ReadOnly = this.rbLongTargetMax.Checked;
		}
		#endregion //Targets

		#region Supertypes
		private void btnSupertypeExclude_Click(object sender, EventArgs e)
		{
			MoveItem(lbxSupertypesIncluded, lbxSupertypesExcluded);
			RefreshSupertypeExamples();
		}
		private void btnSupertypeInclude_Click(object sender, EventArgs e)
		{
			MoveItem(lbxSupertypesExcluded, lbxSupertypesIncluded);
			RefreshSupertypeExamples();
		}
		private void RefreshSupertypeExamples()
		{
		}
		#endregion //Supertypes

		private void MoveItem(ListBox fromBox, ListBox toBox)
		{
			int index = fromBox.SelectedIndex;
			object item = fromBox.SelectedItem;
			if (item == null)
			{
				return;
			}

			fromBox.Items.Remove(item);
			toBox.Items.Add(item);
			toBox.SelectedItem = item;

			if (index == fromBox.Items.Count)
			{
				--index;
			}
			fromBox.SelectedIndex = index;
		}

		private void btnGlobalPopulate_Click(object sender, EventArgs e)
		{
			populate(this.dgvGlobalAbbreviations);
		}

		private void btnTargetPopulate_Click(object sender, EventArgs e)
		{
			populate(this.dgvTargetAbbreviations);
		}

		private void populate(DataGridView dgv)
		{
			string[] objects = new string[] { "Building", "Building Number", "Campus", "Campus_Address", 
				"Employee", "Employee_Nr", "Manager", "Program", "Program_Code", "Room", "Room Number" };

			foreach (string objectType in objects)
			{
				bool add = true;
				foreach (DataGridViewRow row in dgv.Rows)
				{
					if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == objectType)
					{
						add = false;
						break;
					}
				}
				if (add)
				{
					dgv.Rows.Add(objectType, objectType);
				}
			}
		}

		private void lbxCompositeObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbxCompositeObjects.SelectedIndex == 0)
			{
				LoadRoom();
			}
			else
			{
				LoadBuilding();
			}
		}

		public bool RoomCampus, RoomRoom, RoomBuilding;
		public bool BuildingCampus, BuildingBuilding;

		private void StoreBuilding()
		{
			BuildingCampus = this.lbxCompositeIncluded.Items.Contains("Campus");
			BuildingBuilding = this.lbxCompositeIncluded.Items.Contains("Building");
		}
		private void StoreRoom()
		{
			RoomCampus = this.lbxCompositeIncluded.Items.Contains("Campus");
			RoomRoom = this.lbxCompositeIncluded.Items.Contains("Room");
			RoomBuilding = this.lbxCompositeIncluded.Items.Contains("Building");
		}
		private void LoadBuilding()
		{
			this.lbxCompositeIncluded.Items.Clear();
			this.lbxCompositeExcluded.Items.Clear();
			if (BuildingCampus)
				this.lbxCompositeIncluded.Items.Add("Campus");
			else
				this.lbxCompositeExcluded.Items.Add("Campus");
			if (BuildingBuilding)
				this.lbxCompositeIncluded.Items.Add("Building");
			else
				this.lbxCompositeExcluded.Items.Add("Building");
		}
		private void LoadRoom()
		{
			this.lbxCompositeIncluded.Items.Clear();
			this.lbxCompositeExcluded.Items.Clear();
			if (RoomCampus)
				this.lbxCompositeIncluded.Items.Add("Campus");
			else
				this.lbxCompositeExcluded.Items.Add("Campus");
			if (RoomRoom)
				this.lbxCompositeIncluded.Items.Add("Room");
			else
				this.lbxCompositeExcluded.Items.Add("Room");
			if (RoomBuilding)
				this.lbxCompositeIncluded.Items.Add("Building");
			else
				this.lbxCompositeExcluded.Items.Add("Building");
		}

		public string columnSpace, tableSpace;
		public Generator.Case tableCase, columnCase;
		public bool shortenColumn, shortenTable;
		public int columnMaxLength, tableMaxLength;
		public Dictionary<string, string> columnAbbreviations, tableAbbreviations;

		int lastIndex = 1;
		private void cbxTargetNameType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = cbxTargetNameType.SelectedIndex;
			if (index != lastIndex)
			{
				StoreCurrent(1 - index);
				LoadIndex(index);
				lastIndex = index;
			}
		}

		private void StoreCurrent(int index)
		{
			if (index == 0)
				storeTarget(ref tableSpace, ref tableCase, ref shortenTable, ref tableMaxLength, tableAbbreviations);
			else
				storeTarget(ref columnSpace, ref columnCase, ref shortenColumn, ref columnMaxLength, columnAbbreviations);
		}
		private void LoadIndex(int index)
		{
			if (index == 0)
				loadTarget(tableSpace, tableCase, shortenTable, tableMaxLength, tableAbbreviations);
			else
				loadTarget(columnSpace, columnCase, shortenColumn, columnMaxLength, columnAbbreviations);
		}

		void storeTarget(ref string space, ref Generator.Case casing, ref bool shorten, ref int maxLength, Dictionary<string, string> abbreviations)
		{
			if (rbSpaceReplace.Checked)
				space = txtSpaceChar.Text;
			else if (rbSpaceRemove.Checked)
				space = "";
			else
				space = " ";

			if (rbCaseCamel.Checked)
				casing = Generator.Case.Camel;
			else if (rbCaseLower.Checked)
				casing = Generator.Case.Lower;
			else if (rbCasePascal.Checked)
				casing = Generator.Case.Pascal;
			else
				casing = Generator.Case.Upper;

			shorten = rbLongShorten.Checked;

			maxLength = rbLongTargetMax.Checked ? targetMax : int.Parse(txtLongMax.Text);

			abbreviations.Clear();
			foreach (System.Windows.Forms.DataGridViewRow row in dgvTargetAbbreviations.Rows)
				Generator.AddRow(abbreviations, row);
		}
		void loadTarget(string space, Generator.Case casing, bool shorten, int maxLength, Dictionary<string, string> abbreviations)
		{
			txtSpaceChar.Text = "";
			if (space == "")
				rbSpaceRemove.Checked = true;
			else if (space == " ")
				rbSpaceRetain.Checked = true;
			else
			{
				rbSpaceReplace.Checked = true;
				txtSpaceChar.Text = space;
			}

			switch (casing)
			{
				case Generator.Case.Camel:
					rbCaseCamel.Checked = true;
					break;
				case Generator.Case.Lower:
					rbCaseLower.Checked = true;
					break;
				case Generator.Case.Pascal:
					rbCasePascal.Checked = true;
					break;
				case Generator.Case.Upper:
					rbCaseUpper.Checked = true;
					break;
			}

			if (shorten)
				rbLongShorten.Checked = true;
			else
				rbLongWarn.Checked = true;

			if (maxLength == targetMax)
				rbLongTargetMax.Checked = true;
			else
			{
				rbLongUserMax.Checked = true;
				txtLongMax.Text = maxLength.ToString();
			}

			dgvTargetAbbreviations.Rows.Clear();
			foreach (string key in abbreviations.Keys)
			{
				dgvTargetAbbreviations.Rows.Add(key, abbreviations[key]);
			}
		}

		private void dgvGlobalAbbreviations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			CheckUnique(dgvGlobalAbbreviations, e.RowIndex, e.ColumnIndex);
		}

		private void CheckUnique(DataGridView dgv, int rowI, int columnI)
		{
			DataGridViewCell cell = dgv.Rows[rowI].Cells[columnI];
			string check;
			if (cell.Value == null)
			{
				check = "";
			}
			else
			{
				check = cell.Value.ToString();
			}
			foreach (DataGridViewRow row in dgv.Rows)
			{
				if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == check)
				{
					cell.Value = "";
					return;
				}
			}
		}
	}
}