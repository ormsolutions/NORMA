using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	/// <summary>
	/// Dialog for importing options. Realized by <see cref="ORMDesignerSettings"/>
	/// </summary>
	public abstract partial class ImportStepOptionsBase : Form
	{
		/// <summary>
		/// Create a new <see cref="ImportStepOptionsBase"/>
		/// </summary>
		protected ImportStepOptionsBase()
		{
			InitializeComponent();
		}
		/// <summary>
		/// The combo box displaying the transformations
		/// </summary>
		protected ComboBox TransformsCombo
		{
			get
			{
				return ctlTransformsCombo;
			}
		}
		/// <summary>
		/// The property grid displaying the transform options
		/// </summary>
		protected PropertyGrid TransformOptionsPropertyGrid
		{
			get
			{
				return ctlTransformOptionsPropertyGrid;
			}
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
	}
	partial class ORMDesignerSettings
	{
		private class ImportStepOptions : ImportStepOptionsBase
		{
			private ImportStepOptions()
			{
			}
			private static ImportStepOptions myForm;
			public static TransformNode GetTransformOptions(IUIService uiService, LinkedList<TransformNode> nodes)
			{
				ImportStepOptions form = myForm;
				if (form == null)
				{
					myForm = form = new ImportStepOptions();
					form.TransformsCombo.SelectedValueChanged += new EventHandler(
						delegate(object sender, EventArgs e)
						{
							ComboBox senderCombo = (ComboBox)sender;
							PropertyGrid gridControl = ((ImportStepOptions)senderCombo.Parent).TransformOptionsPropertyGrid;
							TransformNode selectedNode = (TransformNode)senderCombo.SelectedItem;
							gridControl.SelectedObject = (selectedNode != null) ? selectedNode.CreateDynamicParametersTypeDescriptor() : null;
						});
				}
				ComboBox combo = form.TransformsCombo;
				PropertyGrid grid = form.TransformOptionsPropertyGrid;
				ComboBox.ObjectCollection items = combo.Items;
				foreach (TransformNode node in nodes)
				{
					items.Add(node);
				}
				combo.SelectedIndex = 0;
				if (nodes.Count == 1)
				{
					grid.Select();
				}
				TransformNode retVal = null;
				if (DialogResult.OK == uiService.ShowDialog(form))
				{
					TransformNode node = (TransformNode)combo.SelectedItem;
					if (node != null)
					{
						node.SynchronizeArguments();
						retVal = node;
					}
				}
				items.Clear();
				grid.SelectedObject = null;
				return retVal;
			}
		}
	}
}