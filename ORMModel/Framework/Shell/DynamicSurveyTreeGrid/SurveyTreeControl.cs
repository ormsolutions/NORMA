#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	/// <summary>
	/// Container for <see cref="VirtualTreeControl"/>.
	/// </summary>
	public class SurveyTreeContainer : UserControl
	{
		private readonly VirtualTreeControl myTreeControl;

		/// <summary>
		/// Initializes a new instance of <see cref="SurveyTreeContainer"/>.
		/// </summary>
		public SurveyTreeContainer()
		{
			this.myTreeControl = new StandardVirtualTreeControl();
			this.SuspendLayout();
			// 
			// myTreeControl
			// 
			this.myTreeControl.Dock = DockStyle.Fill;
			this.myTreeControl.Name = "myTreeControl";
			this.myTreeControl.TabIndex = 0;
#if VISUALSTUDIO_9_0 // MSBUG: Hack workaround crashing bug in VirtualTreeControl.OnToggleExpansion
			this.myTreeControl.ColumnPermutation = new ColumnPermutation(1, new int[]{0}, false);
#endif
			// 
			// SurveyTreeControl
			// 
			this.Controls.Add(this.myTreeControl);
			this.Name = "SurveyTreeContainer";
			this.ResumeLayout(false);
		}

		/// <summary>
		/// Retrieve the <see cref="VirtualTreeControl"/> contained by this control./>
		/// </summary>
		public VirtualTreeControl TreeControl
		{
			get
			{
				return myTreeControl;
			}
		}

		/// <summary>
		/// <see cref="VirtualTreeControl.Tree"/> of <see cref="VirtualTreeControl"/>.
		/// </summary>
		public ITree Tree
		{
			get
			{
				return myTreeControl.Tree;
			}
			set
			{
				myTreeControl.Tree = value;
				myTreeControl.AnchorIndex = -1;
			}
		}
	}
}
