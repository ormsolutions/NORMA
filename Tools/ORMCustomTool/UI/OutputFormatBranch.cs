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
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Drawing;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		private sealed partial class MainBranch
		{
			public sealed class OutputFormatBranch : BranchBase
			{
				public OutputFormatBranch(MainBranch mainBranch)
				{
					this._mainBranch = mainBranch;
					// MainBranch populates this list for us...
					this._ormGenerators = new List<IORMGenerator>();
				}

				private IORMGenerator _selectedORMGenerator;
				private readonly List<IORMGenerator> _ormGenerators;
				private readonly MainBranch _mainBranch;
				
				public IORMGenerator SelectedORMGenerator
				{
					get
					{
						return this._selectedORMGenerator;
					}
					set
					{
						this._selectedORMGenerator = value;
					}
				}

				public IList<IORMGenerator> ORMGenerators
				{
					get
					{
						return this._ormGenerators;
					}
				}

				private MainBranch MainBranch
				{
					get
					{
						return this._mainBranch;
					}
				}

				public override string GetText(int row, int column)
				{
					return this.ORMGenerators[row].DisplayName;
				}

				public override string GetTipText(int row, int column, ToolTipType tipType)
				{
					if (tipType == ToolTipType.StateIcon)
					{
						return _ormGenerators[row].DisplayDescription;
					}
					return base.GetTipText(row, column, tipType);
				}
				public override VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData displayData = VirtualTreeDisplayData.Empty;
					displayData.BackColor = SystemColors.ControlLight;
					bool currentGeneratorSelected = object.ReferenceEquals(_selectedORMGenerator, _ormGenerators[row]);
					if (currentGeneratorSelected)
					{
						displayData.Bold = true;
					}
					if (0 != (requiredData.Mask & VirtualTreeDisplayMasks.StateImage))
					{
						if (currentGeneratorSelected)
						{
							displayData.StateImageIndex = _mainBranch.CanRemoveGenerator(this) ? (short)StandardCheckBoxImage.Checked : (short)StandardCheckBoxImage.CheckedDisabled;
						}
						else
						{
							displayData.StateImageIndex = (short)StandardCheckBoxImage.Unchecked;
						}
					}
					return displayData;
				}
				public override StateRefreshChanges ToggleState(int row, int column)
				{
					StateRefreshChanges retVal = StateRefreshChanges.None;
					IORMGenerator selectedGenerator = _selectedORMGenerator;
					if (selectedGenerator == null)
					{
						retVal = _mainBranch.ToggleOnRequiredBranches(this, row);
					}
					else
					{
						IORMGenerator newGenerator = _ormGenerators[row];
						if (object.ReferenceEquals(newGenerator, selectedGenerator))
						{
							retVal = _mainBranch.ToggleOnRequiredBranches(this, row, true);
						}
						else
						{
							_mainBranch.RemoveGenerator(this);
							retVal = _mainBranch.ToggleOnRequiredBranches(this, row, false);
						}
					}
					if (retVal != StateRefreshChanges.None && retVal != StateRefreshChanges.Entire)
					{
						retVal = StateRefreshChanges.ParentsChildren;
					}
					return retVal;
				}
				public override int VisibleItemCount
				{
					get
					{
						return this.ORMGenerators.Count;
					}
				}
				public override BranchFeatures Features
				{
					get
					{
						return BranchFeatures.StateChanges;
					}
				}
			}
		}
	}
}
