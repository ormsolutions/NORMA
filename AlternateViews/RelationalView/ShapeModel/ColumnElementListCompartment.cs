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
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	/// <summary>
	/// A custom compartment element used to customize how individual compartment items are drawn.
	/// </summary>
	[DomainObjectId("A31CDD05-E36A-4CBC-A1B3-86927BA21E08")]
	internal partial class ColumnElementListCompartment : ElementListCompartment
	{
		/// <summary>
		/// A font used to format the name of mandatory columns.
		/// </summary>
		private static readonly StyleSetResourceId MandatoryFont = new StyleSetResourceId("Neumont", "MandatoryFont");
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
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/> based on the table name.
		/// </summary>	
		private const string TableOffsetString = "_";
		/// <summary>
		/// Specifies the offset used for measuring a string in updating the size of this
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/> based on the column names.
		/// </summary>	
		private const string ColumnOffsetString = "____";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.RelationalView.ColumnElementListCompartment" /> class.	
		/// </summary>
		/// <param name="partition"></param>
		/// <param name="propertyAssignments"></param>
		public ColumnElementListCompartment(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.RelationalView.ColumnElementListCompartment" /> class.
		/// </summary>
		/// <param name="store">The <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> which will contain this
		/// <see cref="T:Neumont.Tools.ORM.RelationalView.ColumnElementListCompartment" />.</param>
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

			if (currentColumn.IsMandatory)
			{
				itemDrawInfo.AlternateFont = true;
			}

			StringBuilder constraintsString = new StringBuilder();
			LinkedElementCollection<Constraint> columnConstraints = currentColumn.ConstraintCollection;
			int constraintCount = columnConstraints.Count;
			int colonIndex = constraintCount - 1;
			for (int i = 0; i < constraintCount; ++i)
			{
				Constraint constraint = columnConstraints[i];
				constraintsString.Append(constraint.Name);
				if (i != colonIndex)
				{
					constraintsString.Append(CommaString);
				}
				else
				{
					constraintsString.Append(ColonString);
				}
			}
			string constraintColumnText = string.Concat(constraintsString.ToString(), currentColumn.Name);
			if (currentColumn.Table.RelationalModel.DisplayDataTypes)
			{
				constraintColumnText = string.Concat(constraintColumnText, ColonString, currentColumn.DataType);
			}
			itemDrawInfo.Text = constraintColumnText;
		}
		/// <summary>
		/// Overridden to allow a read-only <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ListField"/> to be added
		/// to the collection of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects.
		/// </summary>
		/// <param name="shapeFields">A list of <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ShapeField"/> objects
		/// which belong to the current shape.</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);
			// Removes the PlusMinusButtonField and the ListCompartmentListField.
			// The first is removed because we don't want the compartment expanded and collapsed.
			// The second is replaced by our custom list field, the ColumnListField.
			shapeFields.RemoveAt(2);
			shapeFields.RemoveAt(3);
			ColumnListField listField = new ColumnListField("MainListField");
			AnchoringBehavior listFieldAnchoringBehavior = listField.AnchoringBehavior;
			listFieldAnchoringBehavior.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0);
			listFieldAnchoringBehavior.SetTopAnchor(base.HeaderBackgroundField, AnchoringBehavior.Edge.Bottom, 0);
			listFieldAnchoringBehavior.SetRightAnchor(AnchoringBehavior.Edge.Right, 0);
			AssociatedPropertyInfo propertyInfo = new AssociatedPropertyInfo(NodeShape.IsExpandedDomainPropertyId);
			propertyInfo.IsShapeProperty = true;
			listField.AssociateVisibilityWith(base.Store, propertyInfo);

			shapeFields.Add(listField);
		}
		/// <summary>
		/// Gets a handle to the desktop window.
		/// </summary>
		/// <returns>A <see cref="T:System.IntPtr"/> that points to the desktop window.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Initializes the default <see cref="T:System.Drawing.StringFormat"/> associated with this 
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/>.
		/// </summary>
		static ColumnElementListCompartment()
		{
			DefaultStringFormat.Alignment = StringAlignment.Center;
			DefaultStringFormat.LineAlignment = StringAlignment.Center;
			DefaultStringFormat.Trimming = StringTrimming.EllipsisCharacter;
		}
		/// <summary>
		/// Updates the size of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/> and
		/// its associated <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> to size according to the widest column or
		/// table name.
		/// </summary>
		public override void UpdateSize()
		{
			base.UpdateSize();
			SizeD baseSize = Size;
			double height = baseSize.Height;
			double width = baseSize.Width;
			ListField listField = ListField;
			int count = this.GetItemCount(listField);
			TableShape tableShape = ParentShape as TableShape;

			string tableName = tableShape.AccessibleName;
			StyleSet styleSet = StyleSet;
			Font defaultFont = styleSet.GetFont(listField.NormalFontId);
			Font alternateFont = styleSet.GetFont(listField.AlternateFontId);
			Font tableNameFont = tableShape.StyleSet.GetFont(new StyleSetResourceId(string.Empty, "ShapeTextBold10"));
			bool increasedWidth = false;

			using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
			{
				double tableNameWidth = (double)g.MeasureString(tableName + TableOffsetString, tableNameFont, int.MaxValue, DefaultStringFormat).Width;

				// Changes the width if the current width is less than the width of the table name.
				if (width < tableNameWidth)
				{
					width = tableNameWidth;
					increasedWidth = true;
				}
				// Iterates through the column list to check the widths of the column names.
				for (int i = 0; i < count; ++i)
				{
					ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
					GetItemDrawInfo(listField, i, itemDrawInfo);
					bool isMandatory = (this.Items[i] as Column).IsMandatory;
					string text = itemDrawInfo.Text;

					// Gets the size of the column name in the context of the compartment
					double stringWidth = (double)g.MeasureString(text + ColumnOffsetString, isMandatory ? alternateFont : defaultFont, int.MaxValue, DefaultStringFormat).Width;

					// Changes the width if the current width is less than the width of the column name.
					if (width < stringWidth)
					{
						width = stringWidth;
						increasedWidth = true;
					}
				}
			}
			SizeD newSize = new SizeD();
			newSize.Width = width;
			newSize.Height = height;
			if (newSize != baseSize)
			{
				Size = newSize;
			}
			newSize = tableShape.Size;
			if (increasedWidth)
			{
				newSize.Width = width;
			}
			newSize.Height += height;
			tableShape.Size = newSize;
		}
		/// <summary>
		/// Disallows expanding and collapsing of the compartment.
		/// </summary>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
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
	}
	/// <summary>
	/// A custom <see cref="T:Microsoft.VisualStudio.Modeling.Diagrams.ListField"/> that disallows selection of each
	/// element in the list.
	/// </summary>
	internal partial class ColumnListField : ListField
	{
		/// <summary>
		/// Disallows selecting the columns
		/// </summary>
		/// <returns><see langword="false"/>.</returns>
		public override bool GetItemSelectable(ShapeElement parentShape, ListItemSubField listItem)
		{
			return false;
		}
		/// <summary>
		/// Disallows setting focus on the columns
		/// </summary>
		/// <returns><see langword="false" />.</returns>
		public override bool GetItemFocusable(ShapeElement parentShape, ListItemSubField listItem)
		{
			return false;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnListField" /> class.	
		/// </summary>
		/// <param name="fieldName">The name of the field.</param>
	    public ColumnListField(string fieldName)
	        : base(fieldName)
	    {
	    }
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnListField" /> class.	
		/// </summary>
		/// <param name="fieldName">The name of the field.</param>
		/// <param name="snakedList">snakedList</param>
		/// <param name="moreText">moreText</param>
		/// <param name="watermarkText">watermarkText</param>
		public ColumnListField(string fieldName, bool snakedList, string moreText, string watermarkText)
			: base(fieldName, snakedList, moreText, watermarkText)
		{
		}
	}
	/// <summary>
	/// A custom element list compartment description from which a <see cref="ColumnElementListCompartment"/> is created.
	/// </summary>
	internal partial class ColumnElementListCompartmentDescription : ElementListCompartmentDescription
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartmentDescription" /> class.
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

	internal partial class TableShape
	{
		/// <summary>
		/// Holds the <see cref="CompartmentDescriptions"/>s for this <see cref="TableShape"/>.
		/// </summary>
		private static CompartmentDescription[] CompartmentDescriptions;
		/// <summary>
		/// Gets the <see cref="CompartmentDescription"/>s which will create the compartments in this <see cref="TableShape"/>.
		/// </summary>
		/// <returns>CompartmentDescription[]</returns>
		public override CompartmentDescription[] GetCompartmentDescriptions()
		{
			if (CompartmentDescriptions == null)
			{
				CompartmentDescriptions = new CompartmentDescription[1];
				string title = RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeColumnsCompartmentTitle");
				CompartmentDescriptions[0] = new ColumnElementListCompartmentDescription("ColumnsCompartment", title,
					Color.FromKnownColor(KnownColor.LightGray), false,
					Color.FromKnownColor(KnownColor.White), false,
					null, null,
					false);
			}

			return CompartmentDescriptions;
		}
	}
}
