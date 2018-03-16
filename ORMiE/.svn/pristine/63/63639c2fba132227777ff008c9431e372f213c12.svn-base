namespace unibz.ORMInferenceEngine
{
    using System.Windows.Forms;

    partial class ORMInferenceTreeWindow
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORMInferenceTreeWindow));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Inferred Fact Types");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Inferred External Constraints");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Unsatisfiable Components");
            this.myImageList = new System.Windows.Forms.ImageList(this.components);
            this.inferenceTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // myImageList
            // 
            this.myImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("myImageList.ImageStream")));
            this.myImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.myImageList.Images.SetKeyName(0, "entity_type.png");
            this.myImageList.Images.SetKeyName(1, "exclusion.png");
            this.myImageList.Images.SetKeyName(2, "isa.png");
            this.myImageList.Images.SetKeyName(3, "isa_for_exclusion.png");
            this.myImageList.Images.SetKeyName(4, "isa_sub.png");
            this.myImageList.Images.SetKeyName(5, "isa_super.png");
            this.myImageList.Images.SetKeyName(6, "mandatory.png");
            this.myImageList.Images.SetKeyName(7, "role.png");
            this.myImageList.Images.SetKeyName(8, "role_for_subset.png");
            this.myImageList.Images.SetKeyName(9, "subset.png");
            this.myImageList.Images.SetKeyName(10, "uniqueness.png");
            this.myImageList.Images.SetKeyName(11, "value_type.png");
            this.myImageList.Images.SetKeyName(12, "empty.png");
            this.myImageList.Images.SetKeyName(13, "constraint.png");
            this.myImageList.Images.SetKeyName(14, "rolelist.png");
            // 
            // inferenceTreeView
            // 
            this.inferenceTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inferenceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inferenceTreeView.ImageIndex = 12;
            this.inferenceTreeView.ImageList = this.myImageList;
            this.inferenceTreeView.Location = new System.Drawing.Point(0, 0);
            this.inferenceTreeView.Name = "inferenceTreeView";
            treeNode1.ImageIndex = 12;
            treeNode1.Name = "InferredFactTypes";
            treeNode1.Text = "Inferred Fact Types";
            treeNode2.ImageKey = "empty.png";
            treeNode2.Name = "InferredExternalConstraints";
            treeNode2.SelectedImageKey = "empty.png";
            treeNode2.Text = "Inferred External Constraints";
            treeNode3.ImageKey = "empty.png";
            treeNode3.Name = "UnsatisfiableEntityTypes";
            treeNode3.SelectedImageKey = "empty.png";
            treeNode3.Text = "Unsatisfiable Components";
            this.inferenceTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.inferenceTreeView.SelectedImageIndex = 12;
            this.inferenceTreeView.Size = new System.Drawing.Size(195, 208);
            this.inferenceTreeView.TabIndex = 0;
            this.inferenceTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.inferenceTreeView_MouseUp);
            this.inferenceTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // ORMInferenceTreeWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.inferenceTreeView);
            this.Name = "ORMInferenceTreeWindow";
            this.Size = new System.Drawing.Size(195, 208);
            this.ResumeLayout(false);

        }
        #endregion

        public System.Windows.Forms.TreeView inferenceTreeView;
        public ImageList myImageList;

    }
}
