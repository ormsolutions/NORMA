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
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch : BranchBase
		{
			public MainBranch(ORMGeneratorSelectionControl parent)
			{
				this._parent = parent;
				SortedList<string, OutputFormatBranch> branches = this._branches = new SortedList<string, OutputFormatBranch>(StringComparer.OrdinalIgnoreCase);
				foreach (IORMGenerator ormGenerator in ORMCustomTool.ORMGenerators.Values)
				{
					string outputFormatName = ormGenerator.ProvidesOutputFormat;
					OutputFormatBranch outputFormatBranch;
					if (!branches.TryGetValue(outputFormatName, out outputFormatBranch))
					{
						outputFormatBranch = new OutputFormatBranch(this);
						branches.Add(outputFormatName, outputFormatBranch);
					}
					outputFormatBranch.ORMGenerators.Add(ormGenerator);
				}
			}

			private readonly ORMGeneratorSelectionControl _parent;
			private ORMGeneratorSelectionControl Parent
			{
				get
				{
					return this._parent;
				}
			}

			private readonly SortedList<string, OutputFormatBranch> _branches;
			public IDictionary<string, OutputFormatBranch> Branches
			{
				get
				{
					return this._branches;
				}
			}

			public override object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (style == ObjectStyle.ExpandedBranch)
				{
					System.Diagnostics.Debug.Assert(row >= 0 && row < this._branches.Count);
					return this._branches.Values[row];
				}
				return base.GetObject(row, column, style, ref options);
			}

			public override string GetText(int row, int column)
			{
				System.Diagnostics.Debug.Assert(row >= 0 && row < this._branches.Count);
				return this._branches.Keys[row];
			}

			public override VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData displayData = new VirtualTreeDisplayData();
				displayData.BackColor = System.Drawing.SystemColors.ControlLight;
				displayData.State = VirtualTreeDisplayStates.Expanded;
				return displayData;
			}

			public override int VisibleItemCount
			{
				get
				{
					return this.Branches.Count;
				}
			}

			public override BranchFeatures Features
			{
				get
				{
					return BranchFeatures.Expansions | BranchFeatures.Realigns;
				}
			}

			public override bool IsExpandable(int row, int column)
			{
				return true;
			}
		}
	}
}
