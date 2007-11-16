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

using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
namespace Neumont.Tools.ORM.Views.RelationalView
{
	partial class ForeignKeyConnector
	{
		#region Customize appearance
		/// <summary>
		/// Overridden to disallow selection of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Overridden to disallow manual routing of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		/// <remarks>
		/// If this returns <see langword="true"/> while the <see cref="P:Neumont.Tools.ORM.Views.RelationalView.ForeignKeyConnector.CanSelect"/>
		/// property returns <see langword="false"/>, the application will crash while trying to manually route the connector.
		/// </remarks>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Turn on tooltips to show column relationships
		/// </summary>
		public override bool HasToolTip
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Indicate the source/target columns for the foreign key connector
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			ReferenceConstraint constraint = ((ReferenceConstraintTargetsTable)ModelElement).ReferenceConstraint;
			StringBuilder sb = new StringBuilder();
			foreach (ColumnReference columRef in constraint.ColumnReferenceCollection)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(columRef.SourceColumn.Name);
				sb.Append(" -> ");
				sb.Append(columRef.TargetColumn.Name);
			}
			return sb.ToString();
		}
		#endregion // Customize appearance
	}
}
