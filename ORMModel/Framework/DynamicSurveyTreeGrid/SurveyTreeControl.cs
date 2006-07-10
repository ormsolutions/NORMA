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
