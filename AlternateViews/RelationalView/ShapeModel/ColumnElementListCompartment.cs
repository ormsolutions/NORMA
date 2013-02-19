#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using UniquenessConstraint = ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.UniquenessConstraint;

namespace ORMSolutions.ORMArchitect.Views.RelationalView
{
	/// <summary>
	/// A custom compartment element used to customize how individual compartment items are drawn.
	/// </summary>
	[DomainObjectId("A31CDD05-E36A-4CBC-A1B3-86927BA21E08")]
	partial class ColumnElementListCompartment : ElementListCompartment
	{
		/// <summary>
		/// The string that denotes a primary key.
		/// </summary>
		private const string PrimaryKeyString = "PK";
		/// <summary>
		/// The string that prepends an alternate key.
		/// </summary>
		private const string AlternateKeyString = "U{0}";
		/// <summary>
		/// The string that prepends a foreign key.
		/// </summary>
		private const string ForeignKeyString = "FK{0}";
		/// <summary>
		/// A font used to format the name of mandatory columns.
		/// </summary>
		private static readonly StyleSetResourceId MandatoryFont = new StyleSetResourceId("ORMSolutions", "MandatoryColumnFont");
		/// <summary>
		/// The <see cref="T:System.Drawing.StringFormat"/> used to write text for the columns and table name.
		/// </summary>
		private static readonly StringFormat DefaultStringFormat = new StringFormat(StringFormatFlags.NoClip);
		/// <summary>
		/// Specifies a comma string for use with delimiting constraints.
		/// </summary>
		private const string CommaString = ", ";
		/// <summary>
		/// Specifies a colon string for delimiting a constraint list with the column name.
		/// </summary>
		private const string ColonString = " : ";
		/// <summary>
		/// Specifies the offset used for measuring a string in updating the size of this
		/// <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ColumnElementListCompartment"/> based on the table name.
		/// </summary>	
		private const double TableExtraWidth = .14d;
		/// <summary>
		/// Specifies the offset used for measuring a string in updating the size of this
		/// <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ColumnElementListCompartment"/> based on the column names.
		/// </summary>	
		private const double ColumnExtraWidth = .33d;

		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnElementListCompartment" /> class.	
		/// </summary>
		/// <param name="partition"></param>
		/// <param name="propertyAssignments"></param>
		public ColumnElementListCompartment(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ColumnElementListCompartment" /> class.
		/// </summary>
		/// <param name="store">The <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> which will contain this
		/// <see cref="ColumnElementListCompartment" />.</param>
		/// <param name="propertyAssignments">An array of <see cref="T:Microsoft.VisualStudio.Modeling.PropertyAssignment"/> that
		/// works with </param>
		public ColumnElementListCompartment(Store store, params PropertyAssignment[] propertyAssignments)
			: base(store, propertyAssignments)
		{
		}
		/// <summary>
		/// Initialize a font for formatting the mandatory column contents.
		/// </summary>
		/// <param name="classStyleSet">The <see cref="StyleSet"/> being initialized.</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			FontSettings fontSettings = new FontSettings();
			fontSettings.Bold = true;
			classStyleSet.AddFont(MandatoryFont, DiagramFonts.ShapeText, fontSettings);
			this.ListField.AlternateFontId = MandatoryFont;
		}

		/// <summary>
		/// Gets the drawing information for a particular element in the compartment.
		/// </summary>
		/// <param name="listField">The <see cref="ShapeField"/> of this <see cref="ColumnElementListCompartment"/> which
		/// paints the items in this compartment.</param>
		/// <param name="row">The index of the array of items whose item drawing information is of interest.</param>
		/// <param name="itemDrawInfo">A previously initialized <see cref="ItemDrawInfo"/> object whose properties will be modified.</param>
		public override void GetItemDrawInfo(ListField listField, int row, ItemDrawInfo itemDrawInfo)
		{
			base.GetItemDrawInfo(listField, row, itemDrawInfo);
			Column currentColumn = this.Items[row] as Column;
			Debug.Assert(currentColumn != null, "An item in the ColumnElementListCompartment is not a column.");

			if (!currentColumn.IsNullable)
			{
				itemDrawInfo.AlternateFont = true;
			}

			StringBuilder columnText = new StringBuilder();
			bool seenConstraint = false;
			LinkedElementCollection<UniquenessConstraint> tableUniquenessConstraints = null;
			Table currentTable = null;
			foreach (UniquenessConstraint constraint in UniquenessConstraintIncludesColumn.GetUniquenessConstraints(currentColumn))
			{
				if (seenConstraint)
				{
					columnText.Append(CommaString);
				}
				if (constraint.IsPrimary)
				{
					columnText.Append(PrimaryKeyString);
				}
				else
				{
					if (tableUniquenessConstraints == null)
					{
						currentTable = currentColumn.Table;
						tableUniquenessConstraints = currentTable.UniquenessConstraintCollection;
					}
					int constraintNumber = 0;
					foreach (UniquenessConstraint tableConstraint in tableUniquenessConstraints)
					{
						if (!tableConstraint.IsPrimary)
						{
							++constraintNumber;
							if (tableConstraint == constraint)
							{
								break;
							}
						}
					}
					columnText.AppendFormat(AlternateKeyString, constraintNumber);
				}
				seenConstraint = true;
			}
			LinkedElementCollection<ReferenceConstraint> tableReferenceConstraints = null;
			foreach (ColumnReference columnReference in ColumnReference.GetLinksToTargetColumnCollection(currentColumn))
			{
				if (seenConstraint)
				{
					columnText.Append(CommaString);
				}
				if (tableReferenceConstraints == null)
				{
					if (currentTable == null)
					{
						currentTable = currentColumn.Table;
					}
					tableReferenceConstraints = currentTable.ReferenceConstraintCollection;
				}
				columnText.AppendFormat(ForeignKeyString, tableReferenceConstraints.IndexOf(columnReference.ReferenceConstraint) + 1);
				seenConstraint = true;
			}
			if (seenConstraint)
			{
				columnText.Append(ColonString);
			}
			columnText.Append(currentColumn.Name);
			if (((RelationalDiagram)this.Diagram).DisplayDataTypes)
			{
				columnText.Append(ColonString);
				columnText.Append(GetDataType(currentColumn.AssociatedValueType));
			}
			itemDrawInfo.Text = columnText.ToString();
		}
		#region DataType Helper Methods
		/// <summary>
		/// Gets the SQL-Server compliant data type for the <see cref="T:ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="valueType">The <see cref="T:ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType"/> whose data type is of interest.</param>
		/// <returns>The data type, a <see cref="T:System.String"/>.</returns>
		private static string GetDataType(ObjectType valueType)
		{
			return GetDataTypeInternal(valueType).ToLowerInvariant();
		}
		/// <summary>
		/// Gets the SQL-Server compliant data type for the <see cref="T:ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="valueType">The <see cref="T:ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType"/> whose data type is of interest.</param>
		/// <returns>The data type, a <see cref="T:System.String"/>.</returns>
		private static string GetDataTypeInternal(ObjectType valueType)
		{
			// A boolean data type for unaries will not have an associated value type
			if (valueType == null)
			{
				return "BOOLEAN";
			}
			DataType dataType = valueType.DataType;
			int precision = Math.Max(valueType.DataTypeLength, 0);
			int scale = Math.Max(valueType.DataTypeScale, 0);
			if (dataType is NumericDataType || dataType is OtherDataType)
			{
				if (dataType is AutoCounterNumericDataType || dataType is SignedIntegerNumericDataType || dataType is UnsignedIntegerNumericDataType)
				{
					return "INT";
				}
				else if (dataType is SignedSmallIntegerNumericDataType || dataType is UnsignedSmallIntegerNumericDataType)
				{
					return "SMALLINT";
				}
				else if (dataType is OtherDataType || dataType is SignedLargeIntegerNumericDataType || dataType is UnsignedLargeIntegerNumericDataType)
				{
					return "BIGINT";
				}
				else if (dataType is DoublePrecisionFloatingPointNumericDataType)
				{
					return "DOUBLE PRECISION";
				}
				else if (dataType is SinglePrecisionFloatingPointNumericDataType)
				{
					return "REAL";
				}
				else if (dataType is FloatingPointNumericDataType)
				{
					return "FLOAT" + (precision > 0 ? "(" + precision + ")" : string.Empty);
				}
				else if (dataType is UnsignedTinyIntegerNumericDataType)
				{
					return "TINYINT";
				}
				else if (dataType is MoneyNumericDataType)
				{
					if ((precision + scale) > 0)
					{
						return "DECIMAL(" + ((precision == 0) ? 19 : precision) + ", " + Math.Min((scale == 0) ? 4 : scale, 4) + ")";
					}
					return "DECIMAL(19, 4)";
				}
				else
				{
					if ((precision + scale) > 0)
					{
						return "DECIMAL(" + ((precision == 0) ? null : precision.ToString()) + ", " + scale + ")";
					}
					return "DECIMAL";
				}
			}
			else if (dataType is LogicalDataType)
			{
				return "BOOLEAN";
			}
			else if (dataType is VariableLengthTextDataType)
			{
				return "VARCHAR" + (precision > 0 ? "(" + precision.ToString() + ")" : null);
			}
			else if (dataType is LargeLengthTextDataType)
			{
				return "CLOB" + (precision > 0 ? "(" + precision.ToString() + ")" : null);
			}
			else if (dataType is FixedLengthTextDataType)
			{
				return "CHAR" + (precision > 0 ? "(" + precision.ToString() + ")" : null);
			}
			else if (dataType is RawDataDataType)
			{
				return ((dataType is FixedLengthRawDataDataType) ? "BINARY" : ((dataType is LargeLengthRawDataDataType) ? "BLOB" : "VARBINARY")) + ((precision > 0 && dataType.LengthName != null) ? "(" + precision.ToString() + ")" : null);
			}
			else if (dataType is TemporalDataType)
			{
				if (dataType is DateTemporalDataType)
				{
					return "DATE";
				}
				else if (dataType is TimeTemporalDataType)
				{
					return "TIME";
				}
				return "TIMESTAMP";
			}
			else
			{
				return "UNSPECIFIED";
			}
		}
		#endregion // DataType Helper Methods
		/// <summary>
		/// Overridden to allow a read-only <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ListField"/> to be added
		/// to the collection of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects.
		/// </summary>
		/// <param name="shapeFields">A list of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects
		/// which belong to the current shape.</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);
			// Removes the PlusMinusButtonField because we don't want the compartment expanded and collapsed.
			shapeFields.RemoveAt(2);

			//// Removes the HdrText fields because we don't need to see 'Columns' on every shape.
			shapeFields.RemoveAt(2);
			AreaField headerField = (AreaField)shapeFields[1];
			AnchoringBehavior anchor = headerField.AnchoringBehavior;
			anchor.ClearBottomAnchor();
			anchor.SetTopAnchor(AnchoringBehavior.Edge.Top, 0d);
			headerField.DefaultHeight = 1.5 / 72d;

			// The main list field is anchored off the header, which no longer has a bottom anchor.
			// Anchor directly to the top at the same size as the header field
			anchor = shapeFields[2].AnchoringBehavior;
			anchor.SetTopAnchor(AnchoringBehavior.Edge.Top, 1.5 / 72d);
		}
		/// <summary>
		/// Gets a handle to the desktop window.
		/// </summary>
		/// <returns>A <see cref="T:System.IntPtr"/> that points to the desktop window.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Initializes the default <see cref="T:System.Drawing.StringFormat"/> associated with this 
		/// <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ColumnElementListCompartment"/>.
		/// </summary>
		static ColumnElementListCompartment()
		{
			DefaultStringFormat.Alignment = StringAlignment.Center;
			DefaultStringFormat.LineAlignment = StringAlignment.Center;
			DefaultStringFormat.Trimming = StringTrimming.EllipsisCharacter;
		}
		/// <summary>
		/// Updates the size of this <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ColumnElementListCompartment"/> and
		/// its associated <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.TableShape"/> to size according to the widest column or
		/// table name.
		/// </summary>
		public override void UpdateSize()
		{
			UpdateSize(CalculateSize());
		}
		/// <summary>
		/// Change the size of the shape if the size has changed,
		/// or force invalidation if the size has not changed.
		/// </summary>
		public void InvalidateOrUpdateSize()
		{
			SizeD oldSize = Size;
			SizeD newSize = CalculateSize();
			// The .03 adjustment here accounts for the .015 left and right margins applied in
			// the CompartmentShape.AnchorAllCompartments
			if (oldSize != new SizeD(newSize.Width - .03, newSize.Height))
			{
				UpdateSize(newSize);
			}
			else
			{
				((TableShape)ParentShape).InvalidateRequired(true);
			}
		}
		/// <summary>
		/// The default implementation of CollapsedSize requires the
		/// header text field, which we remove.
		/// </summary>
		protected override SizeD CollapsedSize
		{
			get
			{
				return new SizeD(0d, 0d);
			}
		}
		private void UpdateSize(SizeD newSize)
		{
			Size = newSize;
			TableShape parent = (TableShape)ParentShape;
			parent.Size = new SizeD(newSize.Width, parent.Size.Height + newSize.Height);
		}
		private SizeD CalculateSize()
		{
			ListField listField = ListField;
			int count = this.GetItemCount(listField);
			double height = HeaderBounds.Height + listField.GetItemHeight(this) * count;
			double width = 0;
			TableShape tableShape = ParentShape as TableShape;

			string tableName = tableShape.AccessibleName;
			StyleSet styleSet = StyleSet;
			Font defaultFont = styleSet.GetFont(listField.NormalFontId);
			Font alternateFont = styleSet.GetFont(listField.AlternateFontId);
			Font tableNameFont = tableShape.StyleSet.GetFont(new StyleSetResourceId(string.Empty, "ShapeTextBold10"));

			using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
			{
				double tableNameWidth = (double)g.MeasureString(tableName, tableNameFont, int.MaxValue, DefaultStringFormat).Width + TableExtraWidth;

				// Changes the width if the current width is less than the width of the table name.
				if (width < tableNameWidth)
				{
					width = tableNameWidth;
				}
				// Iterates through the column list to check the widths of the column names.
				for (int i = 0; i < count; ++i)
				{
					ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
					GetItemDrawInfo(listField, i, itemDrawInfo);
					bool isMandatory = !(this.Items[i] as Column).IsNullable;
					string text = itemDrawInfo.Text;

					// Gets the size of the column name in the context of the compartment
					double stringWidth = (double)g.MeasureString(text, isMandatory ? alternateFont : defaultFont, int.MaxValue, DefaultStringFormat).Width + ColumnExtraWidth;

					// Changes the width if the current width is less than the width of the column name.
					if (width < stringWidth)
					{
						width = stringWidth;
					}
				}
			}
			return new SizeD(width, height);
		}
		/// <summary>
		/// Disallows expanding and collapsing of the compartment.
		/// </summary>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
		}
		/// <summary>
		/// Disallows collapsing of the compartment with a left key
		/// </summary>
		public override void OnKeyDown(DiagramKeyEventArgs e)
		{
			if (e.KeyData == Keys.Left && !e.Handled)
			{
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}
		/// <summary>
		/// Disallows expanding and collapsing of the compartment. Unfortunately,
		/// noone seems to respect this, or we wouldn't need to OnDoubleClick and OnKeyDown
		/// overrides.
		/// </summary>
		public override bool CanExpandAndCollapse
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Disallows selection of the compartment.
		/// </summary>
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
		#region Column Reordering Gestures
		/// <summary>
		/// Get the <see cref="Column"/> represented by a given <see cref="DiagramItem"/>
		/// </summary>
		private static Column ResolveColumn(DiagramItem diagramItem)
		{
			ICollection targetElements;
			if (diagramItem != null &&
				null != diagramItem.SubField &&
				null != (targetElements = diagramItem.RepresentedElements) &&
				targetElements.Count == 1)
			{
				foreach (object targetElement in targetElements)
				{
					Column retVal = targetElement as Column;
					if (retVal != null)
					{
						return retVal;
					}
					break;
				}
			}
			return null;
		}
		/// <summary>
		/// Check for column reordering
		/// </summary>
		public override void OnDragOver(DiagramDragEventArgs e)
		{
			// I want to handle this in subfield-specific events using
			// enter events only. However, the list compartment doesn't
			// support enter/leave/hover events (these extensions were
			// added to the core elements, but I can't get access to the
			// correct base types here), so we have to reprocess on every
			// mouse move.
			IDataObject data;
			Column targetColumn;
			Column sourceColumn;
			Table table;
			if (null != (data = e.Data) &&
				data.GetDataPresent(typeof(Column)) &&
				null != (targetColumn = ResolveColumn(e.HitDiagramItem)) &&
				null != (sourceColumn = data.GetData(typeof(Column)) as Column) &&
				targetColumn != sourceColumn &&
				(table = targetColumn.Table) == sourceColumn.Table)
			{
				e.Effect = DragDropEffects.Move;
				e.Handled = true;
			}
			base.OnDragOver(e);
		}
		/// <summary>
		/// Reorder columns if a column is dropped on a different column
		/// in the same table.
		/// </summary>
		public override void OnDragDrop(DiagramDragEventArgs e)
		{
			IDataObject data;
			Column targetColumn;
			Column sourceColumn;
			if (null != (data = e.Data) &&
				data.GetDataPresent(typeof(Column)) &&
				null != (targetColumn = ResolveColumn(e.HitDiagramItem)) &&
				null != (sourceColumn = data.GetData(typeof(Column)) as Column))
			{
				e.Handled = Table.CustomReorderColumns(new Column[]{sourceColumn}, targetColumn);
			}
			base.OnDragDrop(e);
		}
		[NonSerialized]
		private static ColumnElementListCompartment myDragSourceShape;
		[NonSerialized]
		private static Column[] myDragSourceColumns;
		public override void OnMouseDown(DiagramMouseEventArgs e)
		{
			base.OnMouseDown(e);
			DiagramItem diagramItem = e.HitDiagramItem;
			Column referenceColumn = ResolveColumn(diagramItem);
			if (referenceColumn != null)
			{
				// See if we're dragging other columns
				DiagramItemCollection selection = e.DiagramClientView.Selection.TopLevelItems;
				Table table = referenceColumn.Table;
				int count = selection.Count;
				Column[] selectedColumns = new Column[count];
				for (int i = 0; i < count; ++i)
				{
					Column column = ResolveColumn(selection[i]);
					if (null == column ||
						(column != referenceColumn && column.Table != table))
					{
						return;
					}
					selectedColumns[i] = column;
				}
				myDragSourceColumns = selectedColumns;
				myDragSourceShape = (ColumnElementListCompartment)diagramItem.Shape;
			}
			else
			{
				myDragSourceColumns = null;
				myDragSourceShape = null;
			}
		}
		public override void OnMouseUp(DiagramMouseEventArgs e)
		{
			myDragSourceShape = null;
			myDragSourceColumns = null;
			base.OnMouseUp(e);
		}
		public override void OnMouseMove(DiagramMouseEventArgs e)
		{
			Column[] sourceColumns;
			if (null != (sourceColumns = myDragSourceColumns))
			{
				DiagramItem currentItem = e.HitDiagramItem;
				Column targetColumn;
				ColumnElementListCompartment sourceShape = myDragSourceShape;
				if (currentItem.Shape != sourceShape)
				{
					myDragSourceColumns = null;
					myDragSourceShape = null;
				}
				else if (null != (targetColumn = ResolveColumn(currentItem)) &&
					-1 == (Array.IndexOf<Column>(sourceColumns, targetColumn)))
				{
					myDragSourceColumns = null;
					myDragSourceShape = null;
					e.DiagramClientView.ActiveMouseAction = new ColumnDragMouseAction(sourceShape, sourceColumns, targetColumn);
				}

			}
			base.OnMouseMove(e);
		}
		private sealed class ColumnDragMouseAction : MouseAction
		{
			private ColumnElementListCompartment myShape;
			private Column[] mySourceColumns;
			private Column myLastTargetColumn;
			public ColumnDragMouseAction(ColumnElementListCompartment compartment, Column[] sourceColumns, Column targetColumn)
				: base(compartment.Diagram)
			{
				myShape = compartment;
				mySourceColumns = sourceColumns;
				myLastTargetColumn = targetColumn;
			}
			/// <summary>
			/// Call back to the source shape to drop the dragged item.
			/// </summary>
			/// <param name="e"></param>
			protected override void OnMouseUp(DiagramMouseEventArgs e)
			{
				base.OnMouseUp(e);
				this.Cancel(e.DiagramClientView);
				e.Handled = true;
				Column column = ResolveColumn(e.HitDiagramItem);
				if (null != column)
				{
					Table.CustomReorderColumns(mySourceColumns, column);
				}
			}
			protected override void OnMouseMove(DiagramMouseEventArgs e)
			{
				base.OnMouseMove(e);
				myLastTargetColumn = ResolveColumn(e.HitDiagramItem);
			}
			/// <summary>
			/// Sanity check. This action may still be active outside the
			/// source shape with rapid clicks.
			/// </summary>
			protected override void OnMouseDown(DiagramMouseEventArgs e)
			{
				base.OnMouseDown(e);
				this.Cancel(e.DiagramClientView);
				e.Handled = false;
			}
			/// <summary>
			/// Display a move cursor while dragging a column
			/// </summary>
			public override Cursor GetCursor(Cursor currentCursor, DiagramClientView diagramClientView, PointD mousePosition)
			{
				Column[] sourceColumns;
				Column targetColumn;
				return (null != (targetColumn = myLastTargetColumn) &&
					-1 == Array.IndexOf<Column>(sourceColumns = mySourceColumns, targetColumn) &&
					sourceColumns[0].Table == targetColumn.Table) ? Cursors.SizeAll : Cursors.No;
			}
		}
		#endregion // Column reordering gestures
	}
	/// <summary>
	/// A custom element list compartment description from which a <see cref="ColumnElementListCompartment"/> is created.
	/// </summary>
	partial class ColumnElementListCompartmentDescription : ElementListCompartmentDescription
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ORMSolutions.ORMArchitect.Views.RelationalView.ColumnElementListCompartmentDescription" /> class.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		/// <param name="title">The title of the element.</param>
		/// <param name="titleFill">The <see cref="T:System.Drawing.Color"/> that fills the title.</param>
		/// <param name="allowCustomTitleFillColor">Specifies whether a title's fill color can be customized.</param>
		/// <param name="compartmentFill">The <see cref="T:System.Drawing.Color"/> that fills the compartment.</param>
		/// <param name="allowCustomCompartmentFillColor">Specifies whether a compartment's fill color can be customized.</param>
		/// <param name="titleFontSettings">The <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.FontSettings"/> used for
		/// the title.</param>
		/// <param name="itemFontSettings">The <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.FontSettings"/> used for
		/// the items in the compartment.</param>
		/// <param name="isDefaultCollapsed">Specifies whether the compartment will be collapsed by default.</param>
		public ColumnElementListCompartmentDescription(string name, string title, Color titleFill, bool allowCustomTitleFillColor, Color compartmentFill, bool allowCustomCompartmentFillColor, FontSettings titleFontSettings, FontSettings itemFontSettings, bool isDefaultCollapsed)
			: base(name, title, titleFill, allowCustomTitleFillColor, compartmentFill, allowCustomCompartmentFillColor, titleFontSettings, itemFontSettings, isDefaultCollapsed)
		{
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Creates a new <see cref="ColumnElementListCompartment"/>.
		/// </summary>
		/// <param name="partition">The current <see cref="Partition"/> of the diagram.</param>
		/// <returns>ColumnElementListCompartment</returns>
		public override Compartment CreateCompartment(Microsoft.VisualStudio.Modeling.Partition partition)
		{
			ColumnElementListCompartment columnElementListCompartment = new ColumnElementListCompartment(partition, null);
			if (base.IsDefaultCollapsed)
			{
				columnElementListCompartment.IsExpanded = false;
			}
			return columnElementListCompartment;
		}
	}
#else // VISUALSTUDIO_10_0
		/// <summary>
		/// Creates a new <see cref="ColumnElementListCompartment"/>.
		/// </summary>
		/// <param name="store">The current <see cref="Store"/> of the model.</param>
		/// <returns>ColumnElementListCompartment</returns>
		public override Compartment CreateCompartment(Microsoft.VisualStudio.Modeling.Store store)
		{
			ColumnElementListCompartment columnElementListCompartment = new ColumnElementListCompartment(store, null);
			if (base.IsDefaultCollapsed)
			{
				columnElementListCompartment.IsExpanded = false;
			}
			return columnElementListCompartment;
		}
	}
#endif // VISUALSTUDIO_10_0

	partial class TableShape
	{
		/// <summary>
		/// Holds the <see cref="CompartmentDescription"/>s for this <see cref="TableShape"/>.
		/// </summary>
		private static CompartmentDescription[] myCompartmentDescriptions;
		/// <summary>
		/// Gets the <see cref="CompartmentDescription"/>s which will create the compartments in this <see cref="TableShape"/>.
		/// </summary>
		/// <returns>CompartmentDescription[]</returns>
		public override CompartmentDescription[] GetCompartmentDescriptions()
		{
			CompartmentDescription[] retVal = myCompartmentDescriptions;
			if (retVal == null)
			{
				myCompartmentDescriptions = retVal = new CompartmentDescription[1];
				string title = RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeColumnsCompartmentTitle");
				retVal[0] = new ColumnElementListCompartmentDescription("ColumnsCompartment", title,
					Color.FromKnownColor(KnownColor.LightGray), false,
					Color.FromKnownColor(KnownColor.White), false,
					null, null,
					false);
			}

			return myCompartmentDescriptions;
		}
	}
}
