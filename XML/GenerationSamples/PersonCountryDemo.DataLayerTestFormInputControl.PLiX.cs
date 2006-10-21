using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
namespace PersonCountryDemo
{
	#region CountryCore_InputControl
	public partial class CountryCore_InputControl : UserControl
	{
		public CountryCore_InputControl()
		{
			this.InitializeComponent();
		}
	}
	#endregion // CountryCore_InputControl
	#region Create_CountryCore_InputControl
	public partial class Create_CountryCore_InputControl : UserControl
	{
		public Create_CountryCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Create on CountryCore");
				return null;
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.abstractTypeVar = this.testVar.CreateCountry(this.dgvCreate.Rows[0].Cells["Country_name"].Value.ToString());
				if (this.abstractTypeVar != null)
				{
					if ((this.dgvCreate.Rows[0].Cells["Country_name"].Value != null) && (this.dgvCreate.Rows[0].Cells["Country_name"].Value.ToString() != this.abstractTypeVar.Country_name))
					{
						this.abstractTypeVar.Country_name = this.dgvCreate.Rows[0].Cells["Country_name"].Value.ToString();
					}
					if ((this.dgvCreate.Rows[0].Cells["Region_Region_code"].Value != null) && (this.dgvCreate.Rows[0].Cells["Region_Region_code"].Value.ToString() != this.abstractTypeVar.Region_Region_code))
					{
						this.abstractTypeVar.Region_Region_code = this.dgvCreate.Rows[0].Cells["Region_Region_code"].Value.ToString();
					}
					this.dgvCreate.Rows.Clear();
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Create Country");
			}
		}
	}
	#endregion // Create_CountryCore_InputControl
	#region Collection_CountryCore_InputControl
	public partial class Collection_CountryCore_InputControl : UserControl
	{
		public Collection_CountryCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Collection on CountryCore");
				return null;
			}
		}
		private void btnCollection_Click(object sender, System.EventArgs e)
		{
			try
			{
				int index = 0;
				this.dgvCollection.Rows.Clear();
				using (IEnumerator<Country> iterator = this.testVar.CountryCollection.GetEnumerator())
				{
					while (iterator.MoveNext())
					{
						this.dgvCollection.Rows.Add();
						this.dgvCollection.Rows[index].Cells["Country_name"].Value = iterator.Current.Country_name;
						this.dgvCollection.Rows[index].Cells["Region_Region_code"].Value = iterator.Current.Region_Region_code;
						++index;
					}
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Collection of Country");
			}
		}
	}
	#endregion // Collection_CountryCore_InputControl
	#region Select_CountryCore_InputControl
	public partial class Select_CountryCore_InputControl : UserControl
	{
		public Select_CountryCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Select on CountryCore");
				return null;
			}
		}
		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			try
			{
				switch (this.cbxSelectionMode.SelectedItem.ToString())
				{
					case "Country_name":
						this.abstractTypeVar = this.testVar.GetCountryByCountry_name(this.dgvSelect.Rows[0].Cells["Country_name"].Value.ToString());
						break;
					default:
						break;
				}
				this.btnSave.Enabled = false;
				this.btnCancel.Enabled = false;
				this.DisplaySelection();
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Select Country");
			}
		}
		private void DisplaySelection()
		{
			try
			{
				if (this.abstractTypeVar == null)
				{
					this.editMode = false;
					this.pnlSave.Visible = false;
					this.dgvCurrentObject.Visible = false;
					this.lblCurrentObject.Text = "There is no selected Country.";
				}
				else
				{
					this.editMode = true;
					this.pnlSave.Visible = true;
					this.dgvCurrentObject.Visible = true;
					this.lblCurrentObject.Text = "The current selected Country:";
					this.dgvCurrentObject.Rows[0].Cells["Country_name"].Value = this.abstractTypeVar.Country_name;
					this.dgvCurrentObject.Columns["Country_name"].Visible = true;
					this.dgvCurrentObject.Rows[0].Cells["Region_Region_code"].Value = this.abstractTypeVar.Region_Region_code;
					this.dgvCurrentObject.Columns["Region_Region_code"].Visible = true;
				}
				this.lblNeedToSave.Text = "";
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Display Country");
			}
		}
		private void cbxSelectionMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.dgvSelect.Columns["Country_name"].Visible = false;
				this.dgvSelect.Columns["Region_Region_code"].Visible = false;
				switch (this.cbxSelectionMode.SelectedItem.ToString())
				{
					case "Country_name":
						this.dgvSelect.Columns["Country_name"].Visible = true;
						break;
					default:
						break;
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Change Selection Mode for Country");
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.abstractTypeVar.Country_name = this.dgvCurrentObject.Rows[0].Cells["Country_name"].Value.ToString();
				this.abstractTypeVar.Region_Region_code = this.dgvCurrentObject.Rows[0].Cells["Region_Region_code"].Value.ToString();
				this.lblNeedToSave.Text = "";
				this.btnSave.Enabled = false;
				this.btnCancel.Enabled = false;
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Save Country");
			}
		}
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.DisplaySelection();
				this.lblNeedToSave.Text = string.Empty;
				this.btnCancel.Enabled = false;
				this.btnSave.Enabled = false;
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Cancel Edit Country");
			}
		}
		private void dgvCurrentObject_CellBeginEdit(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
		{
			try
			{
				if (this.editMode)
				{
					this.lblNeedToSave.Text = "The changed data has not been saved.";
					this.btnCancel.Enabled = true;
					this.btnSave.Enabled = true;
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Begin Edit Country");
			}
		}
	}
	#endregion // Select_CountryCore_InputControl
	#region PersonCore_InputControl
	public partial class PersonCore_InputControl : UserControl
	{
		public PersonCore_InputControl()
		{
			this.InitializeComponent();
		}
	}
	#endregion // PersonCore_InputControl
	#region Create_PersonCore_InputControl
	public partial class Create_PersonCore_InputControl : UserControl
	{
		public Create_PersonCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Create on PersonCore");
				return null;
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.abstractTypeVar = this.testVar.CreatePerson(this.dgvCreate.Rows[0].Cells["LastName"].Value.ToString(), this.dgvCreate.Rows[0].Cells["FirstName"].Value.ToString());
				if (this.abstractTypeVar != null)
				{
					if ((this.dgvCreate.Rows[0].Cells["LastName"].Value != null) && (this.dgvCreate.Rows[0].Cells["LastName"].Value.ToString() != this.abstractTypeVar.LastName))
					{
						this.abstractTypeVar.LastName = this.dgvCreate.Rows[0].Cells["LastName"].Value.ToString();
					}
					if ((this.dgvCreate.Rows[0].Cells["FirstName"].Value != null) && (this.dgvCreate.Rows[0].Cells["FirstName"].Value.ToString() != this.abstractTypeVar.FirstName))
					{
						this.abstractTypeVar.FirstName = this.dgvCreate.Rows[0].Cells["FirstName"].Value.ToString();
					}
					if ((this.dgvCreate.Rows[0].Cells["Title"].Value != null) && (this.dgvCreate.Rows[0].Cells["Title"].Value.ToString() != this.abstractTypeVar.Title))
					{
						this.abstractTypeVar.Title = this.dgvCreate.Rows[0].Cells["Title"].Value.ToString();
					}
					if ((this.dgvCreate.Rows[0].Cells["Country"].Value != null) && (this.testVar.GetCountryByCountry_name(this.dgvCreate.Rows[0].Cells["Country"].Value.ToString()).Country_name != this.abstractTypeVar.Country.Country_name))
					{
						this.abstractTypeVar.Country = this.testVar.GetCountryByCountry_name(this.dgvCreate.Rows[0].Cells["Country"].Value.ToString());
					}
					this.dgvCreate.Rows.Clear();
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Create Person");
			}
		}
	}
	#endregion // Create_PersonCore_InputControl
	#region Collection_PersonCore_InputControl
	public partial class Collection_PersonCore_InputControl : UserControl
	{
		public Collection_PersonCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Collection on PersonCore");
				return null;
			}
		}
		private void btnCollection_Click(object sender, System.EventArgs e)
		{
			try
			{
				int index = 0;
				this.dgvCollection.Rows.Clear();
				using (IEnumerator<Person> iterator = this.testVar.PersonCollection.GetEnumerator())
				{
					while (iterator.MoveNext())
					{
						this.dgvCollection.Rows.Add();
						this.dgvCollection.Rows[index].Cells["Person_id"].Value = iterator.Current.Person_id;
						this.dgvCollection.Rows[index].Cells["LastName"].Value = iterator.Current.LastName;
						this.dgvCollection.Rows[index].Cells["FirstName"].Value = iterator.Current.FirstName;
						this.dgvCollection.Rows[index].Cells["Title"].Value = iterator.Current.Title;
						this.dgvCollection.Rows[index].Cells["Country"].Value = iterator.Current.Country.Country_name;
						++index;
					}
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Collection of Person");
			}
		}
	}
	#endregion // Collection_PersonCore_InputControl
	#region Select_PersonCore_InputControl
	public partial class Select_PersonCore_InputControl : UserControl
	{
		public Select_PersonCore_InputControl()
		{
			this.InitializeComponent();
		}
		public IDbConnection GetConnection()
		{
			try
			{
				string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
				return new SqlConnection(connectionString);
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Get Database Connection for Select on PersonCore");
				return null;
			}
		}
		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			try
			{
				switch (this.cbxSelectionMode.SelectedItem.ToString())
				{
					case "Person_id":
						this.abstractTypeVar = this.testVar.GetPersonByPerson_id(this.dgvSelect.Rows[0].Cells["Person_id"].Value);
						break;
					default:
						break;
				}
				this.btnSave.Enabled = false;
				this.btnCancel.Enabled = false;
				this.DisplaySelection();
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Select Person");
			}
		}
		private void DisplaySelection()
		{
			try
			{
				if (this.abstractTypeVar == null)
				{
					this.editMode = false;
					this.pnlSave.Visible = false;
					this.dgvCurrentObject.Visible = false;
					this.lblCurrentObject.Text = "There is no selected Person.";
				}
				else
				{
					this.editMode = true;
					this.pnlSave.Visible = true;
					this.dgvCurrentObject.Visible = true;
					this.lblCurrentObject.Text = "The current selected Person:";
					this.dgvCurrentObject.Rows[0].Cells["Person_id"].Value = this.abstractTypeVar.Person_id;
					this.dgvCurrentObject.Columns["Person_id"].Visible = true;
					this.dgvCurrentObject.Rows[0].Cells["LastName"].Value = this.abstractTypeVar.LastName;
					this.dgvCurrentObject.Columns["LastName"].Visible = true;
					this.dgvCurrentObject.Rows[0].Cells["FirstName"].Value = this.abstractTypeVar.FirstName;
					this.dgvCurrentObject.Columns["FirstName"].Visible = true;
					this.dgvCurrentObject.Rows[0].Cells["Title"].Value = this.abstractTypeVar.Title;
					this.dgvCurrentObject.Columns["Title"].Visible = true;
					this.dgvCurrentObject.Rows[0].Cells["Country"].Value = this.abstractTypeVar.Country.Country_name;
					this.dgvCurrentObject.Columns["Country"].Visible = true;
				}
				this.lblNeedToSave.Text = "";
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Display Person");
			}
		}
		private void cbxSelectionMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.dgvSelect.Columns["Person_id"].Visible = false;
				this.dgvSelect.Columns["LastName"].Visible = false;
				this.dgvSelect.Columns["FirstName"].Visible = false;
				this.dgvSelect.Columns["Title"].Visible = false;
				this.dgvSelect.Columns["Country"].Visible = false;
				switch (this.cbxSelectionMode.SelectedItem.ToString())
				{
					case "Person_id":
						this.dgvSelect.Columns["Person_id"].Visible = true;
						break;
					default:
						break;
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Change Selection Mode for Person");
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.abstractTypeVar.LastName = this.dgvCurrentObject.Rows[0].Cells["LastName"].Value.ToString();
				this.abstractTypeVar.FirstName = this.dgvCurrentObject.Rows[0].Cells["FirstName"].Value.ToString();
				this.abstractTypeVar.Title = this.dgvCurrentObject.Rows[0].Cells["Title"].Value.ToString();
				this.abstractTypeVar.Country = this.testVar.GetCountryByCountry_name(this.dgvCurrentObject.Rows[0].Cells["Country"].Value.ToString());
				this.lblNeedToSave.Text = "";
				this.btnSave.Enabled = false;
				this.btnCancel.Enabled = false;
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Save Person");
			}
		}
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.DisplaySelection();
				this.lblNeedToSave.Text = string.Empty;
				this.btnCancel.Enabled = false;
				this.btnSave.Enabled = false;
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Cancel Edit Person");
			}
		}
		private void dgvCurrentObject_CellBeginEdit(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
		{
			try
			{
				if (this.editMode)
				{
					this.lblNeedToSave.Text = "The changed data has not been saved.";
					this.btnCancel.Enabled = true;
					this.btnSave.Enabled = true;
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message, "Begin Edit Person");
			}
		}
	}
	#endregion // Select_PersonCore_InputControl
}
