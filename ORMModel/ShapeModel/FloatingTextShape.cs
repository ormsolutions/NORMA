using System;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
namespace Neumont.Tools.ORM.ShapeModel
{
	public abstract partial class FloatingTextShape
	{
		/// <summary>
		/// Retrieve or set the (singleton) shape field for the text
		/// </summary>
		[CLSCompliant(false)]
		protected virtual AutoSizeTextField TextShapeField
		{
			get
			{
				Debug.Assert(false); // Must override
				return null;
			}
			set
			{
				Debug.Assert(false); // Must override
			}
		}

		/// <summary>
		/// Associate the value of the text field with a property
		/// specified by the derived class. Called one per class type.
		/// </summary>
		/// <param name="shapeFields">ShapeFieldCollection to initialized</param>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			AutoSizeTextField textField = CreateAutoSizeTextField();
			textField.AssociateValueWith(Store, AssociatedShapeMetaAttributeGuid, AssociatedModelMetaAttributeGuid);
			textField.DefaultFocusable = true;
			shapeFields.Add(textField);

			// Adjust anchoring after all shape fields are added
			AnchoringBehavior anchor = textField.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.CenterVertically();

			Debug.Assert(TextShapeField == null); // This should only be called once per type
			TextShapeField = textField;
		}
		/// <summary>
		/// Size the label shape to the size of the content label
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				SizeD retVal = SizeD.Empty;
				TextField textShape = TextShapeField;
				if (textShape != null)
				{
					retVal = textShape.GetBounds(this).Size;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Don't outline the text
		/// </summary>
		/// <value></value>
		public override bool HasOutline
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Changed to make the background transparent
		/// </summary>
		public override bool HasFilledBackground
		{
			get { return false; }
		}



		/// <summary>
		/// Retrieve the attribute on the shape field to be associated with the
		/// text field. This attribute should be an xpath-bound property to
		/// the attribute specified in the AssociatedModelMetaAttributeGuid override
		/// </summary>
		protected abstract Guid AssociatedShapeMetaAttributeGuid { get;}
		/// <summary>
		/// Retrieve the attribute on the associated model element to be bound to
		/// the text field. This attribute should be xpath-bound to a property on
		/// the derived shape class. The opposite property is specified with the
		/// AssociatedShapeMetaAttributeGuid override. Defaults to NamedElement.NameMetaAttributeGuid
		/// </summary>
		protected virtual Guid AssociatedModelMetaAttributeGuid
		{
			get
			{
				return NamedElement.NameMetaAttributeGuid;
			}
		}

		/// <summary>
		/// Method to allow inheritors to provide custom implementations of the AutoSizeTextField.
		/// </summary>
		/// <returns>The AutoSizeTextField to use.</returns>
		[CLSCompliant(false)]
		protected virtual AutoSizeTextField CreateAutoSizeTextField()
		{
			// Note: Don't add any other code here. The override
			// is here specifically to enable creation of a more
			// derived text field.
			return new AutoSizeTextField();
		}
	}
}
