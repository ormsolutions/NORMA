namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Filters which model errors are displayed for an ORM Model.
	/// </summary>
	partial class ModelErrorDisplayFilterForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ImageList imageList;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelErrorDisplayFilterForm));
			this.virtualTreeControl = new Neumont.Tools.Modeling.Shell.StandardVirtualTreeControl();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageList
			// 
			imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			resources.ApplyResources(imageList, "imageList");
			imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// virtualTreeControl
			// 
			resources.ApplyResources(this.virtualTreeControl, "virtualTreeControl");
			this.virtualTreeControl.ImageList = imageList;
			this.virtualTreeControl.Name = "virtualTreeControl";
			this.virtualTreeControl.StandardCheckBoxes = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// ModelErrorDisplayFilterForm
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.virtualTreeControl);
			this.MinimizeBox = false;
			this.Name = "ModelErrorDisplayFilterForm";
			this.ShowIcon = false;
			this.ResumeLayout(false);

		}

		#endregion

		private Neumont.Tools.Modeling.Shell.StandardVirtualTreeControl virtualTreeControl;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}