using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	/// <summary>
	/// container for VirtualTreeControl
	/// </summary>
	public partial class SurveyTreeControl : UserControl
	{
		/// <summary>
		/// public constructor
		/// </summary>
		public SurveyTreeControl()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Tree control of VirtualTreeControl
		/// </summary>
		public ITree Tree
		{
			get
			{
				if (myTreeControl == null)
				{
					myTreeControl = new VirtualTreeControl();
				}
				return myTreeControl.Tree;
			}
			set
			{
				myTreeControl.Tree = value;
			}
		}
	}
}
