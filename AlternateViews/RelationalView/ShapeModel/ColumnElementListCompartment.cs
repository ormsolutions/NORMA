using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using System.Drawing;
using Neumont.Tools.ORM.OIALModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

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
		private static readonly StringFormat defaultStringFormat = new StringFormat(StringFormatFlags.NoClip);


		public ColumnElementListCompartment(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
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

			StringBuilder constraints = new StringBuilder();
			LinkedElementCollection<Constraint> columnConstraints = currentColumn.ConstraintCollection;
			int constraintCount = columnConstraints.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				Constraint constraint = columnConstraints[i];
				constraints.Append(constraint.Name);
				if (i != constraintCount - 1)
				{
					constraints.Append(", ");
				}
				else
				{
					constraints.Append(" : ");
				}
			}
			string constraintColumnText = string.Concat(constraints.ToString(), currentColumn.Name);
			if (currentColumn.Table.RelationalModel.DisplayDataTypes)
			{
				constraintColumnText = string.Concat(constraintColumnText, " : ", currentColumn.DataType);
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
			shapeFields.RemoveAt(2);
			shapeFields.RemoveAt(3);
			ColumnListField listField = new ColumnListField("MainListField");
			listField.AnchoringBehavior.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0);
			listField.AnchoringBehavior.SetTopAnchor(base.HeaderBackgroundField, AnchoringBehavior.Edge.Bottom, 0);
			listField.AnchoringBehavior.SetRightAnchor(AnchoringBehavior.Edge.Right, 0);
			AssociatedPropertyInfo info1 = new AssociatedPropertyInfo(NodeShape.IsExpandedDomainPropertyId);
			info1.IsShapeProperty = true;
			listField.AssociateVisibilityWith(base.Store, info1);

			shapeFields.Add(listField);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Initializes the default <see cref="T:System.Drawing.StringFormat"/> associated with this 
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/>.
		/// </summary>
		static ColumnElementListCompartment()
		{
			defaultStringFormat.Alignment = StringAlignment.Center;
			defaultStringFormat.LineAlignment = StringAlignment.Center;
			defaultStringFormat.Trimming = StringTrimming.EllipsisCharacter;
		}
		/// <summary>
		/// Updates the size of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.ColumnElementListCompartment"/> and
		/// its associated <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> to size according to the widest column or
		/// table name.
		/// </summary>
		public override void UpdateSize()
		{
			base.UpdateSize();
			ListField listField = ListField;
			int count = this.GetItemCount(listField);
			double height = Size.Height;
			TableShape tableShape = ParentShape as TableShape;

			string tableName = tableShape.AccessibleName;
			StyleSet styleSet = StyleSet;
			Font defaultFont = styleSet.GetFont(listField.NormalFontId);
			Font alternateFont = styleSet.GetFont(listField.AlternateFontId);
			Font tableNameFont = tableShape.StyleSet.GetFont(new StyleSetResourceId(string.Empty, "ShapeTextBold10"));

			using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
			{
				float tableNameWidth = g.MeasureString(tableName, tableNameFont, int.MaxValue, defaultStringFormat).Width;
				if (base.Size.Width < tableNameWidth)
				{
					base.AbsoluteBounds = new RectangleD(AbsoluteBounds.Location, new SizeD(tableNameWidth, height));
					tableShape.AbsoluteBounds = new RectangleD(tableShape.AbsoluteBounds.Location, new SizeD(tableNameWidth, tableShape.Size.Height));
				}
				for (int i = 0; i < count; ++i)
				{
					ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
					GetItemDrawInfo(listField, i, itemDrawInfo);
					bool isMandatory = (this.Items[i] as Column).IsMandatory;
					string text = itemDrawInfo.Text;

					SizeF stringSize = g.MeasureString(text + "____", isMandatory ? alternateFont : defaultFont, int.MaxValue, defaultStringFormat);
					double width = Convert.ToDouble(stringSize.Width);

					if (base.Size.Width < width)
					{
						double offset = (tableShape.AbsoluteBounds.Width - AbsoluteBounds.Size.Width);
						base.AbsoluteBounds = new RectangleD(AbsoluteBounds.Location, new SizeD(width, height));
						tableShape.AbsoluteBounds = new RectangleD(tableShape.AbsoluteBounds.Location, new SizeD(width + offset, tableShape.Size.Height));
					}
				}
			}
		}
		/// <summary>
		/// Disallows expanding and collapsing of the compartment.
		/// </summary>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
		}
		public override bool CanSelect
		{
			get
			{
				return false;
			}
		}
	}
	#region Temporary ColumnListField Hack to Redraw Surfaces Ourselves
	internal partial class ColumnListField : ListField
	{
		/// <summary>
		/// Disallows selecting the columns
		/// </summary>
		/// <returns><see langword="false"/></returns>
		public override bool GetItemSelectable(ShapeElement parentShape, ListItemSubField listItem)
		{
			return false;
		}
		/// <summary>
		/// Disallows setting focus on the columns
		/// </summary>
		/// <returns><see langword="false" /></returns>
		public override bool GetItemFocusable(ShapeElement parentShape, ListItemSubField listItem)
		{
			return false;
		}

	    private string moreText;
	    private bool snakedList;
	    private string watermarkText;

	    public ColumnListField(string fieldName)
	        : base(fieldName)
	    {
	    }

	    public ColumnListField(string fieldName, bool snakedList, string moreText, string watermarkText)
	        : base(fieldName)
	    {
	        this.snakedList = snakedList;
	        this.moreText = moreText;
	        this.watermarkText = watermarkText;
	    }

		//private bool IsWatermarkNeeded(ShapeElement parentShape)
		//{
		//    if (((this.GetItemCount(parentShape) == 0) && (this.watermarkText != null)) && (this.watermarkText.Length > 0))
		//    {
		//        return true;
		//    }
		//    return false;
		//}

		//private float CalculateMaximumItemWidth(ShapeElement parentShape, Graphics graphics)
		//{
		//    IListFieldContainer container1 = parentShape as IListFieldContainer;
		//    if (container1 == null)
		//    {
		//        return 0f;
		//    }
		//    ItemDrawInfo info1 = new ItemDrawInfo();
		//    int num1 = container1.GetItemCount(this);
		//    float single1 = (float)this.GetItemHeight(parentShape);
		//    SizeF ef1 = SizeF.Empty;
		//    float single2 = 0f;
		//    using (Font font1 = parentShape.StyleSet.GetFont(this.NormalFontId))
		//    {
		//        SizeF ef3 = graphics.MeasureString(this.moreText, font1);
		//        float single3 = ef3.Width;
		//        for (int num2 = 0; num2 < num1; num2++)
		//        {
		//            info1.Clear();
		//            info1.StringFormat = this.DefaultStringFormat;
		//            info1.Indent = this.DefaultItemIndent;
		//            container1.GetItemDrawInfo(this, num2, info1);
		//            if ((ef1 == SizeF.Empty) && (info1.Image != null))
		//            {
		//                ef1 = ImageHelper.GetImageSize(info1.Image);
		//            }
		//            float single4 = (ef1.Height == 0f) ? 1f : (single1 / ef1.Height);
		//            float single5 = ef1.Width * single4;
		//            string text1 = info1.Text;
		//            float single6 = single3;
		//            if ((text1 != null) && (text1.Length != 0))
		//            {
		//                PointF tf1 = new PointF(0f, 0f);
		//                SizeF ef2 = graphics.MeasureString(text1, font1, tf1, info1.StringFormat);
		//                single6 = Math.Max(ef2.Width, single3);
		//            }
		//            float single7 = ((((float)info1.Indent) + single5) + ((float)info1.ImageMargin)) + single6;
		//            single2 = Math.Max(single2, single7);
		//        }
		//    }
		//    return single2;
		//}

		//private static void SafeDrawRectangle(Graphics g, Pen pen, float x, float y, float width, float height)
		//{
		//    try
		//    {
		//        if ((g != null) && (pen != null))
		//        {
		//            g.DrawRectangle(pen, x, y, width, height);
		//        }
		//    }
		//    catch (OutOfMemoryException)
		//    {
		//        if (pen.DashStyle == DashStyle.Solid)
		//        {
		//            throw;
		//        }
		//    }
		//    catch (OverflowException)
		//    {
		//    }
		//}


		//public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
		//{
		//    IListFieldContainer container1 = parentShape as ColumnElementListCompartment;
		//    if (container1 != null)
		//    {
		//        int num1 = container1.GetItemCount(this);
		//        bool flag1 = (e.View != null) ? e.View.Focused : false;
		//        RectangleF ef1 = RectangleD.ToRectangleF(this.GetBounds(parentShape));
		//        RectangleF ef2 = RectangleD.ToRectangleF(base.GetBounds(parentShape));
		//        float single1 = (float)this.GetItemHeight(parentShape);
		//        float single2 = (float)this.GetItemTextHeight(parentShape);
		//        SizeF ef3 = SizeF.Empty;
		//        RectangleF ef4 = new RectangleF();
		//        RectangleF ef6 = new RectangleF();
		//        RectangleF ef7 = new RectangleF();
		//        RectangleF ef8 = new RectangleF();
		//        int num2 = num1;
		//        int num3 = 1;
		//        float single3 = ef1.Width;
		//        StyleSet set1 = parentShape.StyleSet;
		//        using (Font font1 = set1.GetFont(this.NormalFontId))
		//        {
		//            using (Font font2 = set1.GetFont(this.AlternateFontId))
		//            {
		//                Brush brush1 = set1.GetBrush(this.NormalTextBrushId);
		//                Brush brush2 = set1.GetBrush(this.NormalBackgroundBrushId);
		//                Brush brush3 = set1.GetBrush(this.DisabledTextBrushId);
		//                Brush brush4 = set1.GetBrush(flag1 ? this.SelectedTextBrushId : this.InactiveSelectedTextBrushId);
		//                Brush brush5 = set1.GetBrush(flag1 ? this.SelectedBackgroundBrushId : this.InactiveSelectedBackgroundBrushId);
		//                Brush brush6 = set1.GetBrush(flag1 ? this.SelectedDisabledTextBrushId : this.InactiveSelectedDisabledTextBrushId);
		//                Pen pen1 = set1.GetPen(this.FocusPenId);
		//                Pen pen2 = set1.GetPen(this.FocusBackgroundPenId);
		//                e.Graphics.FillRectangle(brush2, ef1);
		//                if (num1 == 0)
		//                {
		//                    if (this.IsWatermarkNeeded(parentShape))
		//                    {
		//                        Brush brush7 = SystemBrushes.FromSystemColor(SystemColors.GrayText);
		//                        e.Graphics.DrawString(this.watermarkText, font1, brush7, ef2, this.WatermarkFormat);
		//                    }
		//                }
		//                else
		//                {
		//                    ItemDrawInfo info1 = new ItemDrawInfo();
		//                    if (this.SnakedList)
		//                    {
		//                        single3 = this.CalculateMaximumItemWidth(parentShape, e.Graphics);
		//                        num2 = (int)Math.Max(1, Math.Floor((double)(ef2.Height / single1)));
		//                        num3 = (int)Math.Max(1, Math.Floor((double)(ef2.Width / single3)));
		//                        container1.SnakingInfo.MaxItemWidth = single3;
		//                        container1.SnakingInfo.VisibleColumns = num3;
		//                        container1.SnakingInfo.VisibleRows = num2;
		//                    }
		//                    DiagramItem item1 = new DiagramItem(parentShape, this, new ColumnListItemSubField(0));
		//                    DiagramItem item2 = (e.View != null) ? e.View.Selection.FocusedItem : null;
		//                    bool flag2 = (e.View != null) && e.View.Selection.HasPendingEdit(e.View);
		//                    for (int num4 = 0; num4 < num1; num4++)
		//                    {
		//                        RectangleF ef5;
		//                        if (this.SnakedList && (num4 >= (num2 * num3)))
		//                        {
		//                            return;
		//                        }
		//                        info1.Clear();
		//                        info1.StringFormat = this.DefaultStringFormat;
		//                        info1.Indent = this.DefaultItemIndent;
		//                        container1.GetItemDrawInfo(this, num4, info1);
		//                        string text1 = info1.Text;
		//                        Image image1 = info1.Image;
		//                        Font font3 = info1.AlternateFont ? font2 : font1;
		//                        if ((this.SnakedList && (num4 == ((num2 * num3) - 1))) && (num4 != (num1 - 1)))
		//                        {
		//                            text1 = this.moreText;
		//                            image1 = null;
		//                        }
		//                        if ((ef3 == SizeF.Empty) && (image1 != null))
		//                        {
		//                            ef3 = ImageHelper.GetImageSize(image1);
		//                        }
		//                        int num5 = num4 % num2;
		//                        int num6 = num4 / num2;
		//                        ef4.X = ef1.X + (num6 * single3);
		//                        ef4.Y = ef1.Y + (num5 * single1);
		//                        ef4.Width = Math.Max(0f, Math.Min(single3, ef1.Right - ef4.X));
		//                        ef4.Height = single1;
		//                        ef6.X = ef4.X + ((float)info1.Indent);
		//                        ef6.Y = ef4.Y;
		//                        ef6.Height = ef4.Height;
		//                        float single4 = (ef3.Height == 0f) ? 1f : (ef6.Height / ef3.Height);
		//                        ef6.Width = ef3.Width * single4;
		//                        ef7.X = ef6.X + Math.Max((float)0f, (float)((ef6.Width - ef3.Width) / 2f));
		//                        ef7.Y = ef6.Y + Math.Max((float)0f, (float)((ef6.Height - ef3.Height) / 2f));
		//                        ef7.Width = Math.Min(ef6.Width, ef3.Width);
		//                        ef7.Height = Math.Min(ef6.Height, ef3.Height);
		//                        ef8.X = ef6.Right + ((float)info1.ImageMargin);
		//                        ef8.Y = ef4.Y + ((float)this.GetPaddingAboveItemText(parentShape));
		//                        ef8.Width = Math.Max((float)0f, (float)(ef4.Right - ef8.X));
		//                        ef8.Height = Math.Min(ef4.Bottom, ef8.Y + single2) - ef8.Y;
		//                        if (this.JaggedSelectionRectangle || this.SnakedList)
		//                        {
		//                            float single5 = 0f;
		//                            if ((text1 != null) && (text1.Length != 0))
		//                            {
		//                                SizeF ef9 = e.Graphics.MeasureString(text1, font3, ef8.Location, info1.StringFormat);
		//                                single5 = ef9.Width;
		//                            }
		//                            ef8.Width = Math.Min(ef8.Width, single5);
		//                            ef8.Width += (float)VGConstants.FuzzDistance;
		//                            ef5 = new RectangleF();
		//                            ef5.X = ef4.X;
		//                            ef5.Y = ef4.Y;
		//                            ef5.Height = ef4.Height;
		//                            ef5.Width = (((float)info1.Indent) + ef6.Width) + ef8.Width;
		//                        }
		//                        else
		//                        {
		//                            ef5 = ef4;
		//                        }
		//                        ((ColumnListItemSubField)item1.SubField).Row = num5;
		//                        ((ColumnListItemSubField)item1.SubField).Column = num6;
		//                        bool flag3 = (e.View != null) ? e.View.Selection.Contains(item1) : false;
		//                        bool flag4 = (item2 != null) ? item1.Equals(item2) : false;
		//                        if (flag3 && !flag2)
		//                        {
		//                            e.Graphics.FillRectangle(brush5, ef5);
		//                        }
		//                        if ((flag1 && flag4) && !flag2)
		//                        {
		//                            SafeDrawRectangle(e.Graphics, pen2, ef5.X, ef5.Y, ef5.Width, ef5.Height);
		//                            SafeDrawRectangle(e.Graphics, pen1, ef5.X, ef5.Y, ef5.Width, ef5.Height);
		//                        }
		//                        if ((ef6.Width > 0) && (image1 != null))
		//                        {
		//                            Bitmap bitmap1 = image1 as Bitmap;
		//                            if (bitmap1 != null)
		//                            {
		//                                bitmap1.MakeTransparent(Color.Magenta);
		//                            }
		//                            //if ((e.Graphics.PageScale == 1) && this.CanSnapToPixel())
		//                            //{
		//                            //    PointF tf1 = ef7.Location;
		//                            //    PointF tf2 = base.SnapToPixel(e.Graphics, tf1);
		//                            //    e.Graphics.DrawImage(image1, tf2);
		//                            //}
		//                            //else
		//                            //{
		//                            e.Graphics.DrawImage(image1, ef7);
		//                            //}
		//                        }
		//                        if ((text1 != null) && (text1.Length > 0))
		//                        {
		//                            Brush brush8 = null;
		//                            if (info1.Disabled)
		//                            {
		//                                brush8 = flag3 ? brush6 : brush3;
		//                            }
		//                            else
		//                            {
		//                                brush8 = flag3 ? brush4 : brush1;
		//                            }
		//                            e.Graphics.DrawString(text1, font3, brush8, ef8, info1.StringFormat);
		//                        }
		//                    }
		//                }
		//            }
		//        }
		//    }
		//}
	}
	#endregion
	/// <summary>
	/// A custom element list compartment description from which a <see cref="ColumnElementListCompartment"/> is created.
	/// </summary>
	internal partial class ColumnElementListCompartmentDescription : ElementListCompartmentDescription
	{
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
		private static CompartmentDescription[] myCompartmentDescriptions;
		/// <summary>
		/// Gets the <see cref="CompartmentDescription"/>s which will create the compartments in this <see cref="TableShape"/>.
		/// </summary>
		/// <returns>CompartmentDescription[]</returns>
		public override CompartmentDescription[] GetCompartmentDescriptions()
		{
			if (myCompartmentDescriptions == null)
			{
				myCompartmentDescriptions = new CompartmentDescription[1];
				string title = RelationalShapeDomainModel.SingletonResourceManager.GetString("TableShapeColumnsCompartmentTitle");
				myCompartmentDescriptions[0] = new ColumnElementListCompartmentDescription("ColumnsCompartment", title,
					Color.FromKnownColor(KnownColor.LightGray), false,
					Color.FromKnownColor(KnownColor.White), false,
					null, null,
					false);
			}

			return myCompartmentDescriptions;
		}
	}
}