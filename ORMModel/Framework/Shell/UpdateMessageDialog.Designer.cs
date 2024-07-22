namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	partial class UpgradeMessageDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpgradeMessageDialog));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnLeaveUnread = new System.Windows.Forms.Button();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.browser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnLeaveUnread
			// 
			resources.ApplyResources(this.btnLeaveUnread, "btnLeaveUnread");
			this.btnLeaveUnread.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnLeaveUnread.Name = "btnLeaveUnread";
			this.btnLeaveUnread.UseVisualStyleBackColor = true;
			this.btnLeaveUnread.Click += new System.EventHandler(this.btnLeaveUnread_Click);
			// 
			// browser
			// 
			resources.ApplyResources(this.browser, "browser");
			this.browser.CausesValidation = false;
			this.browser.IsWebBrowserContextMenuEnabled = false;
			this.browser.Name = "browser";
			this.browser.ScriptErrorsSuppressed = true;
			this.browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.browser_Navigating);
			// 
			// UpgradeMessageDialog
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.CancelButton = this.btnLeaveUnread;
			this.Controls.Add(this.browser);
			this.Controls.Add(this.btnLeaveUnread);
			this.Controls.Add(this.btnOK);
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "UpgradeMessageDialog";
			this.ShowIcon = false;
			this.Load += new System.EventHandler(this.UpgradeMessageDialog_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnLeaveUnread;
		private System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.WebBrowser browser;
	}
}