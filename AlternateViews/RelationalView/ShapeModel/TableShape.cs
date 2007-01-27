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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

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
			Type shapeSizeChangeRuleType = typeof(ShapeElement).Assembly.GetType("Microsoft.VisualStudio.Modeling.Diagrams.ShapeSizeChangeRule", true, false);
			ruleManager.DisableRule(shapeSizeChangeRuleType);
			AbsoluteBounds = new RectangleD(PointD.Empty, DefaultSize);
			ruleManager.EnableRule(shapeSizeChangeRuleType);
		}
		/// <summary>
		/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> can be expanded or collapsed.
		/// </summary>
		public override bool CanExpandAndCollapse
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> has a shadow.
		/// </summary>
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
			// Removes the text field from the shape field list.
			shapeFields.RemoveAt(1);
			// 
			TableTextField textField = new TableTextField("TableNameDecorator");
			textField.DefaultText = RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeTableNameDecoratorDefaultText");
			textField.DefaultFocusable = true;
			textField.DefaultAutoSize = true;
			textField.AnchoringBehavior.MinimumHeightInLines = 1;
			textField.AnchoringBehavior.MinimumWidthInCharacters = 1;
			textField.DefaultFontId = new StyleSetResourceId(string.Empty, "ShapeTextBold10");
			shapeFields.Add(textField);
		}

		/// <summary>
		/// Gets the resizable sides on this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/>.
		/// </summary>
		public override NodeSides ResizableSides
		{
			get
			{
				return NodeSides.None;
			}
		}
		/// <summary>
		/// A custom <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.TextField"/> that disallows selection and focus of the element.
		/// </summary>
		private class TableTextField : TextField
		{
			/// <summary>
			/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> is selectable.
			/// </summary>
			/// <param name="parentShape">parentShape</param>
			/// <returns><see langword="false" />.</returns>
			public override bool GetSelectable(ShapeElement parentShape)
			{
				return false;
			}
			/// <summary>
			/// Gets whether the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> is focusable.
			/// </summary>
			/// <param name="parentShape">parentShape</param>
			/// <returns><see langword="false" />.</returns>
			public override bool GetFocusable(ShapeElement parentShape)
			{
				return false;
			}
			/// <summary>
			/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableTextField" /> class.	
			/// </summary>
			/// <param name="fieldName">The name of the field.</param>
			public TableTextField(string fieldName)
				: base(fieldName)
			{

			}
		}
	}
}
