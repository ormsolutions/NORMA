using System.Security.Permissions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace unibz.ORMInferenceEngine
{
    /// <summary>
    /// Summary description for MyControl.
    /// </summary>
    public partial class ORMInferenceTreeWindow : UserControl
    {
        public Dictionary<string, string> dicConstraintImages;
        public ORMInferenceTreeWindow()
        {
            dicConstraintImages = new Dictionary<string, string>();
            InitializeComponent();
        }

        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
              // If we're the top-level form or control, we need to do the mnemonic handling
              if (charCode != ' ' && ProcessMnemonic(charCode))
              {
                    return true;
              }
              return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// Enable the IME status handling for this control.
        /// </summary>
        protected override bool CanEnableIme
        {
            get
            {
                return true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show(this,
                            string.Format(System.Globalization.CultureInfo.CurrentUICulture, "We are inside {0}.button1_Click()", this.ToString()),
                            "ORM2 Inference Engine");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void inferenceTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void inferenceTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            // Show image only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {
		        // Point where the mouse is clicked.
		        Point p = new Point(e.X, e.Y);

		        // Get the node that the user has clicked.
                TreeNode node = this.inferenceTreeView.GetNodeAt(p);
                if (node != null && node.Name != "" && node.Name != "InternalConstraints" &&
                    node.Name != "InferredFactTypes" && node.Name != "InferredExternalConstraints" &&
                    node.Name != "UnsatisfiableEntityTypes")
                {
                    string imgname = this.dicConstraintImages[node.Name];
                    if (imgname != "")
                    {
                        Image img = Image.FromFile(imgname);
                        InferredImageForm form = new InferredImageForm();
                        Size mysize = img.Size;
                        mysize.Height += 40;
                        mysize.Width += 30;
                        form.Size = mysize;
                        form.BackgroundImage = img;
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.Text = "Inferred constraint";
                        form.Show();
                    }
                }
            }
        }
    }

}
