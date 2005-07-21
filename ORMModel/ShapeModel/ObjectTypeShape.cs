using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Northface.Tools.ORM;
using Northface.Tools.ORM.ObjectModel;
using System.Runtime.InteropServices;
namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ObjectTypeShape : IModelErrorActivation
	{

		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError for DataTypeNotSpecifiedError
		/// </summary>
		protected void ActivateModelError(ModelError error)
		{
			if (error is DataTypeNotSpecifiedError)
			{
				IServiceProvider provider;
				IVsUIShell shell;
				if (null != (provider = (Store as IORMToolServices).ServiceProvider) &&
					null != (shell = (IVsUIShell)provider.GetService(typeof(IVsUIShell))))
				{
					Guid windowGuid = new Guid(ToolWindowGuids.PropertyBrowser);
					IVsWindowFrame frame;
					ErrorHandler.ThrowOnFailure(shell.FindToolWindow((uint)(__VSFINDTOOLWIN.FTW_fForceCreate), ref windowGuid, out frame));
					ErrorHandler.ThrowOnFailure(frame.Show());
					// UNDONE: Keep going, select the DataType property, and open its dropdown. I've pinged MS on this one, I can't
					// see any good way to get at the PropertyGrid reference, or to open the dropdown when I find it.
				}
			}
		}
		void IModelErrorActivation.ActivateModelError(ModelError error)
		{
			ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
	public partial class ObjectTypeShape
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField;
		private static AutoSizeTextField myReferenceModeShapeField;
		private const double HorizontalMargin = 0.2;
		private const double VerticalMargin = 0.075;
		private static readonly StyleSetResourceId DashedShapeOutlinePen = new StyleSetResourceId("Northface", "DashedShapeOutlinePen");
		#endregion // Member Variables
		#region Customize appearance
		/// <summary>
		/// Switch between the standard solid pen and
		/// a dashed pen depending on the objectification settings
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				ObjectType associatedObjectType = ModelElement as ObjectType;
				return (associatedObjectType != null && associatedObjectType.IsValueType) ? DashedShapeOutlinePen : DiagramPens.ShapeOutline;
			}
		}
		/// <summary>
		/// Add a dashed pen to the class resource set
		/// </summary>
		/// <param name="classStyleSet">Shared class styleset instance</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings settings = new PenSettings();
			settings.Width = 1.2f / 72f; // 1.2 point
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, settings);
			settings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(DashedShapeOutlinePen, DiagramPens.ShapeOutline, settings);
		}
		/// <summary>
		/// Set the default size for this object. This value is basically
		/// ignored because the size is ultimately based on the contained
		/// text, but it needs to be set.
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return new SizeD(.7, .35);
			}
		}
		/// <summary>
		/// Get the shape of an object type. Controllable via the ORM Designer
		/// tab on the options page.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				ShapeGeometry useShape;
				switch (Shell.OptionsPage.CurrentObjectTypeShape)
				{
					case Shell.ObjectTypeShape.Ellipse:
						useShape = CustomFoldEllipseShapeGeometry.ShapeGeometry;
						break;
					case Shell.ObjectTypeShape.HardRectangle:
						useShape = CustomFoldRectangleShapeGeometry.ShapeGeometry;
						break;
					case Shell.ObjectTypeShape.SoftRectangle:
					default:
						useShape = CustomFoldRoundedRectangleShapeGeometry.ShapeGeometry;
						break;
				}
				return useShape;
			}
		}
		/// <summary>
		/// Size to ContentSize plus some margin padding.
		/// </summary>
		public override void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty)
			{
				contentSize.Width += HorizontalMargin + HorizontalMargin;
				contentSize.Height += VerticalMargin + VerticalMargin;
				Size = contentSize;
			}
		}
		/// <summary>
		/// Set the content size to the text size
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				SizeD retVal = SizeD.Empty;
				TextField textShape = TextShapeField;
				TextField referenceShape = ReferenceModeShapeField;
				if (textShape != null)
				{
					SizeD textSize = textShape.GetBounds(this).Size;
					SizeD referenceSize = referenceShape.GetBounds(this).Size;
					retVal.Width = (textSize.Width > referenceSize.Width) ? textSize.Width : referenceSize.Width;
					retVal.Height = textSize.Height + referenceSize.Height;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Retrieve the (singleton) shape field for the text
		/// </summary>
		protected static TextField TextShapeField
		{
			get
			{
				return myTextShapeField;
			}
		}
		/// <summary>
		/// Retrieve the (singleton) shape field for the reference mode text
		/// </summary>
		protected static TextField ReferenceModeShapeField
		{
			get
			{
				return myReferenceModeShapeField;
			}
		}
		/// <summary>
		/// Creates and adds shape fields to this shape type. Called once per type.
		/// </summary>
		/// <param name="shapeFields">The shape fields collection for this shape type.</param>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize field
			AutoSizeTextField field = new AutoSizeTextField();
			field.DrawBorder = false;
			field.FillBackground = false;
			field.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			field.DefaultPenId = DiagramPens.ShapeOutline;
			field.DefaultFontId = DiagramFonts.ShapeTitle;
			field.DefaultFocusable = true;
			field.DefaultText = "Object";

			StringFormat fieldFormat = new StringFormat(StringFormatFlags.NoClip);
			fieldFormat.Alignment = StringAlignment.Center;
			field.DefaultStringFormat = fieldFormat;
			field.AssociateValueWith(Store, ObjectTypeShape.ShapeNameMetaAttributeGuid, NamedElement.NameMetaAttributeGuid);

			// Initialize reference mode field
			AutoSizeTextField referenceModeField = new ReferenceModeAutoSizeTextField();
			referenceModeField.DrawBorder = false;
			referenceModeField.FillBackground = false;
			referenceModeField.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			referenceModeField.DefaultPenId = DiagramPens.ShapeOutline;
			referenceModeField.DefaultFontId = DiagramFonts.ShapeTitle;
			referenceModeField.DefaultFocusable = true;
			referenceModeField.DefaultText = "";
			referenceModeField.DefaultStringFormat = fieldFormat;
			referenceModeField.AssociateValueWith(Store, ObjectTypeShape.ReferenceModeNameMetaAttributeGuid, ObjectType.ReferenceModeDisplayMetaAttributeGuid);

			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(field);
			shapeFields.Add(referenceModeField);

			// Modify field anchoring behavior
			AnchoringBehavior anchor = field.AnchoringBehavior;
			anchor.SetTopAnchor(AnchoringBehavior.Edge.Top, VerticalMargin);
			anchor.CenterHorizontally();

			// Modify reference mode field anchoring behavior
			AnchoringBehavior referenceModeAnchor = referenceModeField.AnchoringBehavior;
			referenceModeAnchor.CenterHorizontally();
			referenceModeAnchor.SetTopAnchor(field, AnchoringBehavior.Edge.Bottom, 0);

			Debug.Assert(myTextShapeField == null); // Only called once
			myTextShapeField = field;
			Debug.Assert(myReferenceModeShapeField == null); // Only called once
			myReferenceModeShapeField = referenceModeField;
		}

		/// <summary>
		/// Add a shape element linked to this parent to display the value range
		/// </summary>
		/// <param name="element">ModelElement of type ObjectType</param>
		/// <returns>true</returns>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
#if DEBUG
			//RoleValueRangeDefinitions added should be for a role opposite of
			//this object's role player. The test below allows a RoleValueRangeDefinition
			//to be added on the opposite role even if that opposite role is played
			//by the same ObjectType as the RoleValueRangeDefinition's role (i.e. allows
			//for ring constraints).
			bool isRoleValueRangeDefn = false;
			RoleValueRangeDefinition roleDefn;
			if (null != (roleDefn = element as RoleValueRangeDefinition))
			{
				Role roleInDefn = roleDefn.Role;
				FactType factType = roleInDefn.FactType;
				foreach (Role r in factType.RoleCollection)
				{
					if (!object.ReferenceEquals(roleInDefn, r))
					{
						if (object.ReferenceEquals(r.RolePlayer, AssociatedObjectType))
						{
							isRoleValueRangeDefn = true;
							break;
						}
					}
				}
			}
			Debug.Assert(
				(element is ValueTypeValueRangeDefinition && ((ValueTypeValueRangeDefinition)element).ValueType == AssociatedObjectType) ||
				isRoleValueRangeDefn
			);
#endif // DEBUG
			return true;
		}
		#endregion // Customize appearance
		#region ObjectTypeShape specific
		/// <summary>
		/// Get the ObjectTypeType associated with this shape
		/// </summary>
		public ObjectType AssociatedObjectType
		{
			get
			{
				return ModelElement as ObjectType;
			}
		}
		/// <summary>
		/// Makes a shape a relative child element.
		/// </summary>
		/// <param name="childShape">The ShapeElement to get the ReleationshipType for.</param>
		/// <returns>RelationshipType.Relative</returns>
		protected override RelationshipType ChooseRelationship(ShapeElement childShape)
		{
			Debug.Assert(childShape is ValueRangeShape);
			return RelationshipType.Relative;
		}

		#endregion // ObjectTypeShape specific
		#region Shape display update rules
		[RuleOn(typeof(ObjectType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == NamedElement.NameMetaAttributeGuid)
				{
					foreach (object obj in e.ModelElement.AssociatedPresentationElements)
					{
						ORMBaseShape shape = obj as ORMBaseShape;
						if (shape != null)
						{
							shape.AutoResize();
						}
					}
				}
				else if (attributeGuid == ObjectType.IsValueTypeMetaAttributeGuid)
				{
					foreach (object obj in e.ModelElement.AssociatedPresentationElements)
					{
						ShapeElement shape = obj as ShapeElement;
						if (shape != null)
						{
							shape.Invalidate();
						}
					}
				}
			}
		}
		#endregion // Shape display update rules
	}

	/// <summary>
	/// Temporary class to fer refernce mode to show up.
	/// </summary>
	public class ReferenceModeAutoSizeTextField : AutoSizeTextField
	{
		//TODO:Remove
		/// <summary>
		/// Default constructor
		/// </summary>
		public ReferenceModeAutoSizeTextField()
		{
			DefaultFocusable = true;
			DefaultSelectable = true;
			DefaultVisibility = true;			
		}

		/// <summary>
		/// Get the minimum width of the shape field for the current text.
		/// </summary>
		/// <param name="parentShape">ShapeElement</param>
		/// <returns>Width of current text</returns>
		public override double GetMinimumWidth(ShapeElement parentShape)
		{
			ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
			ObjectType objectType = parentShape.ModelElement as ObjectType;
			if (objectType != null)
			{
				if (!objectType.IsValueType && !objectTypeShape.ExpandRefMode)
				{

					return base.GetMinimumWidth(parentShape);
				}
			}
			return 0;
		}
		/// <summary>
		/// Get the minimum height of the shape field for the current text.
		/// </summary>
		/// <param name="parentShape">ShapeElement</param>
		/// <returns>Width of current text</returns>
		public override double GetMinimumHeight(ShapeElement parentShape)
		{
			ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
			ObjectType objectType = parentShape.ModelElement as ObjectType;
			if (objectType != null)
			{
				if (!objectType.IsValueType && !objectTypeShape.ExpandRefMode)
				{

					return base.GetMinimumHeight(parentShape);
				}
			}
			return 0;
		}

		/// <summary>
		/// Returns whether or not the text field is visible
		/// </summary>
		/// <param name="parentShape"></param>
		/// <returns></returns>
		public override bool GetVisible(ShapeElement parentShape)
		{
			ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
			ObjectType objectType = parentShape.ModelElement as ObjectType;
			if (objectType != null && !objectTypeShape.ExpandRefMode)
			{
				if (!objectType.IsValueType)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Overrides the display text to add parenthesis
		/// </summary>
		/// <param name="parentShape"></param>
		/// <returns></returns>
		public override string GetDisplayText(ShapeElement parentShape)
		{
			
			ObjectType objectType = parentShape.ModelElement as ObjectType;
			if (objectType != null)
			{
				if (objectType.HasReferenceMode)
				{
					return string.Format(CultureInfo.InvariantCulture, "({0})", base.GetDisplayText(parentShape));
				}
			}
			return base.GetDisplayText(parentShape);
		}
	}
}
