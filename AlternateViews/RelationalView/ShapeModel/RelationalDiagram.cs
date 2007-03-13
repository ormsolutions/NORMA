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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.OIALModel;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	[DiagramMenuDisplay(DiagramMenuDisplayOptions.BlockRename, typeof(RelationalDiagram), RelationalDiagram.NameResourceName, "Diagram.TabImage")]
	internal partial class RelationalDiagram
	{
		private const string NameResourceName = "Diagram.MenuDisplayName";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public RelationalDiagram(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public RelationalDiagram(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			this.Name = Neumont.Tools.Modeling.Design.ResourceAccessor<RelationalDiagram>.ResourceManager.GetString(NameResourceName);
		}
		public override void OnInitialize()
		{
			base.OnInitialize();
			if (this.Subject == null)
			{
				ReadOnlyCollection<RelationalModel> modelElements = this.Store.DefaultPartition.ElementDirectory.FindElements<RelationalModel>();
				if (modelElements.Count > 0)
				{
					this.Associate(modelElements[0]);
				}
			}
		}
		/// <summary>
		/// Stop all auto shape selection on transaction commit except when
		/// the item is being dropped.
		/// </summary>
		public override IList FixUpDiagramSelection(ShapeElement newChildShape)
		{
			if (DropTargetContext.HasDropTargetContext(Store.TransactionManager.CurrentTransaction))
			{
				return base.FixUpDiagramSelection(newChildShape);
			}
			return null;
		}
		/// <summary>
		/// Disallows changing the name of the Relational Diagram
		/// </summary>
		[RuleOn(typeof(RelationalDiagram))]
		private sealed partial class NameChangeRule : ChangeRule
		{
			/// <summary>
			/// Changes the name of the <see cref="T:Neumont.Tools.ORM.Views.RelationalDiagram"/> to
			/// its default name if changed by a user.
			/// </summary>
			/// <param name="e"><see cref="Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs"/>.</param>
			public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == Diagram.NameDomainPropertyId)
				{
					RelationalDiagram diagram = e.ModelElement as RelationalDiagram;
					string name = Neumont.Tools.Modeling.Design.ResourceAccessor<RelationalDiagram>.ResourceManager.GetString(NameResourceName);
					if (diagram != null && diagram.Name != name)
					{
						diagram.Name = name;
					}
				}
			}
		}
	}
}
