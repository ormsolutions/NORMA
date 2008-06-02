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
using System.Collections.ObjectModel;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms;
using b = Neumont.Tools.EntityRelationshipModels.Barker;
using Neumont.Tools.EntityRelationshipModels.Barker;

namespace Neumont.Tools.ORM.Views.BarkerERView
{
	/// <summary>
	/// A custom compartment element used to customize how individual compartment items are drawn.
	/// </summary>
	[DomainObjectId("DC28FECA-04DC-4B0D-A59D-E920CCC340B6")]
	partial class AttributeElementListCompartment : ElementListCompartment
	{
		/// <summary>
		/// The string that denotes a primary identifier.
		/// </summary>
		private const string PrimaryIdentifierString = "#";
		private const string BlankString = " ";
		/// <summary>
		/// The string that denotes a mandatory attribute.
		/// </summary>
		private const string MandatoryString = "*";
		/// <summary>
		/// The string that denotes a optional attribute.
		/// </summary>
		private const string OptionalString = "°";
		/// <summary>
		/// The <see cref="T:System.Drawing.StringFormat"/> used to write text for the attribute and entity name.
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
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.AttributeElementListCompartment"/> based on the entity name.
		/// </summary>	
		private const double BarkerEntityExtraWidth = .14d;
		/// <summary>
		/// Specifies the offset used for measuring a string in updating the size of this
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.AttributeElementListCompartment"/> based on the attribute names.
		/// </summary>	
		private const double AttributeExtraWidth = .33d;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.RelationalView.AttributeElementListCompartment" /> class.	
		/// </summary>
		/// <param name="partition"></param>
		/// <param name="propertyAssignments"></param>
		public AttributeElementListCompartment(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.RelationalView.AttributeElementListCompartment" /> class.
		/// </summary>
		/// <param name="store">The <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> which will contain this
		/// <see cref="T:Neumont.Tools.ORM.RelationalView.AttributeElementListCompartment" />.</param>
		/// <param name="propertyAssignments">An array of <see cref="T:Microsoft.VisualStudio.Modeling.PropertyAssignment"/> that
		/// works with </param>
		public AttributeElementListCompartment(Store store, params PropertyAssignment[] propertyAssignments)
			: base(store, propertyAssignments)
		{
		}
		/// <summary>
		/// Initialize a font for formatting the mandatory attribute contents.
		/// </summary>
		/// <param name="classStyleSet">The <see cref="StyleSet"/> being initialized.</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
		}

		/// <summary>
		/// Gets the drawing information for a particular element in the compartment.
		/// </summary>
		/// <param name="listField">The <see cref="ShapeField"/> of this <see cref="AttributeElementListCompartment"/> which
		/// paints the items in this compartment.</param>
		/// <param name="row">The index of the array of items whose item drawing information is of interest.</param>
		/// <param name="itemDrawInfo">A previously initialized <see cref="ItemDrawInfo"/> object whose properties will be modified.</param>
		public override void GetItemDrawInfo(ListField listField, int row, ItemDrawInfo itemDrawInfo)
		{
			base.GetItemDrawInfo(listField, row, itemDrawInfo);
			b.Attribute currentAttribute = this.Items[row] as b.Attribute;
			Debug.Assert(currentAttribute != null, "An item in the AttributeElementListCompartment is not an attribute.");

			if (currentAttribute.IsMandatory)
			{
				itemDrawInfo.AlternateFont = true;
			}

			StringBuilder attributeText = new StringBuilder();

			if (currentAttribute.IsPrimaryIdComponent)
			{
				attributeText.Append(PrimaryIdentifierString);
			}
			else
			{
				attributeText.Append(BlankString);
			}
			attributeText.Append(BlankString);

			if (currentAttribute.IsMandatory)
			{
				attributeText.Append(MandatoryString);
			}
			else
			{
				attributeText.Append(OptionalString);
			}
			attributeText.Append(BlankString);
			attributeText.Append(currentAttribute.Name);
			itemDrawInfo.Text = attributeText.ToString();
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
			// Removes the PlusMinusButtonField because we don't want the compartment expanded and collapsed.
			shapeFields.RemoveAt(2);
		}
		/// <summary>
		/// Gets a handle to the desktop window.
		/// </summary>
		/// <returns>A <see cref="T:System.IntPtr"/> that points to the desktop window.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetDesktopWindow();

		/// <summary>
		/// Initializes the default <see cref="T:System.Drawing.StringFormat"/> associated with this 
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.AttributeElementListCompartment"/>.
		/// </summary>
		static AttributeElementListCompartment()
		{
			DefaultStringFormat.Alignment = StringAlignment.Center;
			DefaultStringFormat.LineAlignment = StringAlignment.Center;
			DefaultStringFormat.Trimming = StringTrimming.EllipsisCharacter;
		}
		/// <summary>
		/// Updates the size of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.AttributeElementListCompartment"/> and
		/// its associated <see cref="T:Neumont.Tools.ORM.Views.RelationalView.TableShape"/> to size according to the widest column or
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
				((BarkerEntityShape)ParentShape).InvalidateRequired(true);
			}
		}
		private void UpdateSize(SizeD newSize)
		{
			Size = newSize;
			BarkerEntityShape parent = (BarkerEntityShape)ParentShape;
			parent.Size = new SizeD(newSize.Width, parent.Size.Height + newSize.Height);
		}
		private SizeD CalculateSize()
		{
			ListField listField = ListField;
			int count = this.GetItemCount(listField);
			double height = HeaderBounds.Height + listField.GetItemHeight(this) * count;
			double width = 0;
			BarkerEntityShape entityShape = ParentShape as BarkerEntityShape;

			string entityName = entityShape.AccessibleName;
			StyleSet styleSet = StyleSet;
			Font defaultFont = styleSet.GetFont(listField.NormalFontId);
			Font alternateFont = styleSet.GetFont(listField.AlternateFontId);
			Font entityNameFont = entityShape.StyleSet.GetFont(new StyleSetResourceId(string.Empty, "ShapeTextBold10"));

			using (Graphics g = Graphics.FromHwnd(GetDesktopWindow()))
			{
				double entityNameWidth = (double)g.MeasureString(entityName, entityNameFont, int.MaxValue, DefaultStringFormat).Width + BarkerEntityExtraWidth;

				// Changes the width if the current width is less than the width of the table name.
				if (width < entityNameWidth)
				{
					width = entityNameWidth;
				}
				// Iterates through the column list to check the widths of the column names.
				for (int i = 0; i < count; ++i)
				{
					ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
					GetItemDrawInfo(listField, i, itemDrawInfo);
					bool isMandatory = (this.Items[i] as b.Attribute).IsMandatory;
					string text = itemDrawInfo.Text;

					// Gets the size of the column name in the context of the compartment
					double stringWidth = (double)g.MeasureString(text, isMandatory ? alternateFont : defaultFont, int.MaxValue, DefaultStringFormat).Width + AttributeExtraWidth;

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
	}
	/// <summary>
	/// A custom element list compartment description from which a <see cref="AttributeElementListCompartment"/> is created.
	/// </summary>
	partial class AttributeElementListCompartmentDescription : ElementListCompartmentDescription
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.AttributeElementListCompartmentDescription" /> class.
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
		public AttributeElementListCompartmentDescription(string name, string title, Color titleFill, bool allowCustomTitleFillColor, Color compartmentFill, bool allowCustomCompartmentFillColor, FontSettings titleFontSettings, FontSettings itemFontSettings, bool isDefaultCollapsed)
			: base(name, title, titleFill, allowCustomTitleFillColor, compartmentFill, allowCustomCompartmentFillColor, titleFontSettings, itemFontSettings, isDefaultCollapsed)
		{
		}
		/// <summary>
		/// Creates a new <see cref="AttributeElementListCompartment"/>.
		/// </summary>
		/// <param name="store">The current <see cref="Store"/> of the model.</param>
		/// <returns>AttributeElementListCompartment</returns>
		public override Compartment CreateCompartment(Microsoft.VisualStudio.Modeling.Store store)
		{
			AttributeElementListCompartment AttributeElementListCompartment = new AttributeElementListCompartment(store, null);
			if (base.IsDefaultCollapsed)
			{
				AttributeElementListCompartment.IsExpanded = false;
			}
			return AttributeElementListCompartment;
		}
	}

	partial class BarkerEntityShape
	{
		/// <summary>
		/// Holds the <see cref="CompartmentDescription"/>s for this <see cref="BarkerEntityShape"/>.
		/// </summary>
		private static CompartmentDescription[] myCompartmentDescriptions;
		/// <summary>
		/// Gets the <see cref="CompartmentDescription"/>s which will create the compartments in this <see cref="BarkerEntityShape"/>.
		/// </summary>
		/// <returns>CompartmentDescription[]</returns>
		public override CompartmentDescription[] GetCompartmentDescriptions()
		{
			CompartmentDescription[] retVal = myCompartmentDescriptions;
			if (retVal == null)
			{
				myCompartmentDescriptions = retVal = new CompartmentDescription[1];
				string title = BarkerERShapeDomainModel.SingletonResourceManager.GetString("BarkerEntityShapeAttributesCompartmentTitle");
				retVal[0] = new AttributeElementListCompartmentDescription("AttributesCompartment", title,
					Color.FromKnownColor(KnownColor.LightGray), false,
					Color.FromKnownColor(KnownColor.White), false,
					null, null,
					false);
			}

			return myCompartmentDescriptions;
		}
	}
}
