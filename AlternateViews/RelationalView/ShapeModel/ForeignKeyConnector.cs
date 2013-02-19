#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using System.Text;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
namespace ORMSolutions.ORMArchitect.Views.RelationalView
{
	partial class ForeignKeyConnector
	{
		#region Customize appearance
		/// <summary>
		/// Overridden to disallow selection of this <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Overridden to disallow manual routing of this <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ForeignKeyConnector"/>.
		/// </summary>
		/// <remarks>
		/// If this returns <see langword="true"/> while the <see cref="P:ORMSolutions.ORMArchitect.Views.RelationalView.ForeignKeyConnector.CanSelect"/>
		/// property returns <see langword="false"/>, the application will crash while trying to manually route the connector.
		/// </remarks>
		public override bool CanManuallyRoute
		{
			get
			{
				return false;
			}
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Stop the user from manually moving link end points
		/// </summary>
		public override bool CanMoveAnchorPoints
		{
			get
			{
				return false;
			}
		}
#endif // VISUALSTUDIO_10_0
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
			string sourceTableName = constraint.SourceTable.Name;
			string targetTableName = constraint.TargetTable.Name;
			foreach (ColumnReference columRef in constraint.ColumnReferenceCollection)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(sourceTableName);
				sb.Append(".");
				sb.Append(columRef.SourceColumn.Name);
				sb.Append(" -> ");
				sb.Append(targetTableName);
				sb.Append(".");
				sb.Append(columRef.TargetColumn.Name);
			}
			return sb.ToString();
		}
		/// <summary>
		/// Amp up the z-order when a connector is highlighted
		/// so that covered lines jump to the top and are easily
		/// visible.
		/// </summary>
		public override double ZOrder
		{
			get
			{
				Diagram diagram;
				DiagramView diagramView;
				DiagramClientView clientView;
				HighlightedShapesCollection highlightedShapes;
				if (null != (diagram = Diagram) &&
					null != (diagramView = diagram.ActiveDiagramView) &&
					null != (clientView = diagramView.DiagramClientView) &&
					null != (highlightedShapes = clientView.HighlightedShapes))
				{
					foreach (DiagramItem item in highlightedShapes)
					{
						if (this == item.Shape)
						{
							return base.ZOrder + 100000d;
						}
					}
				}
				return base.ZOrder;
			}
			set
			{
				base.ZOrder = value;
			}
		}
		#endregion // Customize appearance
	}
}
