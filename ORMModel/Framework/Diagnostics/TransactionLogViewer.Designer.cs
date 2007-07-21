namespace Neumont.Tools.Modeling.Diagnostics
{
	partial class TransactionLogViewer
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
			System.Windows.Forms.Label FilterLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionLogViewer));
			this.ImageList = new System.Windows.Forms.ImageList(this.components);
			this.TreeControl = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.CloseButton = new System.Windows.Forms.Button();
			this.UndoItemsCombo = new System.Windows.Forms.ComboBox();
			this.RedoItemsCombo = new System.Windows.Forms.ComboBox();
			this.UndoLabel = new System.Windows.Forms.Label();
			this.RedoLabel = new System.Windows.Forms.Label();
			this.ClearFilterButton = new System.Windows.Forms.Button();
			this.Tooltip = new System.Windows.Forms.ToolTip(this.components);
			FilterLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// FilterLabel
			// 
			resources.ApplyResources(FilterLabel, "FilterLabel");
			FilterLabel.ImageList = this.ImageList;
			FilterLabel.Name = "FilterLabel";
			this.Tooltip.SetToolTip(FilterLabel, resources.GetString("FilterLabel.ToolTip"));
			FilterLabel.UseMnemonic = false;
			// 
			// ImageList
			// 
			this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
			this.ImageList.TransparentColor = System.Drawing.Color.Magenta;
			this.ImageList.Images.SetKeyName(0, "");
			this.ImageList.Images.SetKeyName(1, "");
			this.ImageList.Images.SetKeyName(2, "");
			this.ImageList.Images.SetKeyName(3, "");
			this.ImageList.Images.SetKeyName(4, "");
			this.ImageList.Images.SetKeyName(5, "");
			this.ImageList.Images.SetKeyName(6, "");
			this.ImageList.Images.SetKeyName(7, "");
			this.ImageList.Images.SetKeyName(8, "");
			this.ImageList.Images.SetKeyName(9, "");
			this.ImageList.Images.SetKeyName(10, "");
			this.ImageList.Images.SetKeyName(11, "");
			this.ImageList.Images.SetKeyName(12, "");
			this.ImageList.Images.SetKeyName(13, "");
			// 
			// TreeControl
			// 
			resources.ApplyResources(this.TreeControl, "TreeControl");
			this.TreeControl.HasGridLines = true;
			this.TreeControl.HasHorizontalGridLines = true;
			this.TreeControl.HasLines = false;
			this.TreeControl.HasRootLines = false;
			this.TreeControl.HasVerticalGridLines = true;
			this.TreeControl.ImageList = this.ImageList;
			this.TreeControl.IsDragSource = false;
			this.TreeControl.LabelEditSupport = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.None;
			this.TreeControl.Name = "TreeControl";
			this.TreeControl.DoubleClick += new Microsoft.VisualStudio.VirtualTreeGrid.DoubleClickEventHandler(this.TreeControl_DoubleClick);
			// 
			// CloseButton
			// 
			resources.ApplyResources(this.CloseButton, "CloseButton");
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// UndoItemsCombo
			// 
			resources.ApplyResources(this.UndoItemsCombo, "UndoItemsCombo");
			this.UndoItemsCombo.CausesValidation = false;
			this.UndoItemsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.UndoItemsCombo.FormattingEnabled = true;
			this.UndoItemsCombo.Name = "UndoItemsCombo";
			this.UndoItemsCombo.SelectedIndexChanged += new System.EventHandler(this.UndoItemsCombo_SelectedIndexChanged);
			// 
			// RedoItemsCombo
			// 
			resources.ApplyResources(this.RedoItemsCombo, "RedoItemsCombo");
			this.RedoItemsCombo.CausesValidation = false;
			this.RedoItemsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RedoItemsCombo.FormattingEnabled = true;
			this.RedoItemsCombo.Name = "RedoItemsCombo";
			this.RedoItemsCombo.SelectedIndexChanged += new System.EventHandler(this.RedoItemsCombo_SelectedIndexChanged);
			// 
			// UndoLabel
			// 
			resources.ApplyResources(this.UndoLabel, "UndoLabel");
			this.UndoLabel.CausesValidation = false;
			this.UndoLabel.Name = "UndoLabel";
			// 
			// RedoLabel
			// 
			resources.ApplyResources(this.RedoLabel, "RedoLabel");
			this.RedoLabel.CausesValidation = false;
			this.RedoLabel.Name = "RedoLabel";
			// 
			// ClearFilterButton
			// 
			resources.ApplyResources(this.ClearFilterButton, "ClearFilterButton");
			this.ClearFilterButton.Name = "ClearFilterButton";
			this.ClearFilterButton.UseVisualStyleBackColor = true;
			this.ClearFilterButton.Click += new System.EventHandler(this.ClearFilterButton_Click);
			// 
			// Tooltip
			// 
			resources.ApplyResources(this.Tooltip, "Tooltip");
			// 
			// TransactionLogViewer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CloseButton;
			this.CausesValidation = false;
			this.Controls.Add(this.RedoLabel);
			this.Controls.Add(this.UndoLabel);
			this.Controls.Add(this.RedoItemsCombo);
			this.Controls.Add(this.UndoItemsCombo);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.TreeControl);
			this.Controls.Add(this.ClearFilterButton);
			this.Controls.Add(FilterLabel);
			this.MinimizeBox = false;
			this.Name = "TransactionLogViewer";
			this.ShowIcon = false;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransactionLogViewer_FormClosed);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransactionLogViewer_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

}

		#endregion

		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.ComboBox UndoItemsCombo;
		private System.Windows.Forms.ComboBox RedoItemsCombo;
		private System.Windows.Forms.Label UndoLabel;
		private System.Windows.Forms.Label RedoLabel;
		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl TreeControl;
		private System.Windows.Forms.ImageList ImageList;
		private System.Windows.Forms.Button ClearFilterButton;
		private System.Windows.Forms.ToolTip Tooltip;
	}
}