using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Diagrams.Design;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	internal partial class TableShape
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public TableShape(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public TableShape(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			// HACK: At this point in the transaction, the commit time rules have been fired, which
			// includes the Microsoft.VisualStudio.Modeling.Diagrams.ShapeSizeChangeRule. This rule
			// checks whether the Diagram of the current shape support ports. However, the diagram
			// for this shape does not get initialized until after the size has been set. We temporarily
			// disable this rule until a better solution can be found.
			RuleManager ruleManager = partition.Store.RuleManager;
			Type crappyRuleType = typeof(ShapeElement).Assembly.GetType("Microsoft.VisualStudio.Modeling.Diagrams.ShapeSizeChangeRule", true, false);
			ruleManager.DisableRule(crappyRuleType);
			AbsoluteBounds = new RectangleD(PointD.Empty, DefaultSize);
			ruleManager.EnableRule(crappyRuleType);
		}

		public override bool CanExpandAndCollapse
		{
			get
			{
				return false;
			}
		}

		public override bool HasShadow
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Overridden to allow a read-only <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.TextField"/> to be added
		/// to the collection of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects.
		/// </summary>
		/// <param name="shapeFields">A list of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects
		/// which belong to the current shape.</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);
			shapeFields.RemoveAt(1);
			TableTextField field1 = new TableTextField("TableNameDecorator");
			field1.DefaultText = global::Neumont.Tools.ORM.Views.RelationalView.RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeTableNameDecoratorDefaultText");
			field1.DefaultFocusable = true;
			field1.DefaultAutoSize = true;
			field1.AnchoringBehavior.MinimumHeightInLines = 1;
			field1.AnchoringBehavior.MinimumWidthInCharacters = 1;
			field1.DefaultFontId = new StyleSetResourceId(string.Empty, "ShapeTextBold10");
			shapeFields.Add(field1);
		}
		public override NodeSides ResizableSides
		{
			get
			{
				return NodeSides.None;
			}
		}
	}

	internal class TableTextField : TextField
	{
		public override bool GetSelectable(ShapeElement parentShape)
		{
			return false;
		}

		public override bool GetFocusable(ShapeElement parentShape)
		{
			return false;
		}

		//override 

		public TableTextField(string fieldName) : base(fieldName)
		{

		}
	}
}
