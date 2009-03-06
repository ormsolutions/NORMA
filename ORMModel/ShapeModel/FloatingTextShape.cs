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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Drawing;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public abstract partial class FloatingTextShape
	{
		/// <summary>
		/// Retrieve or set the (singleton) shape field for the text
		/// </summary>
		protected virtual AutoSizeTextField TextShapeField
		{
			get
			{
				Debug.Fail("Must override");
				return null;
			}
			set
			{
				Debug.Fail("Must override");
			}
		}
		/// <summary>
		/// Set up our background pen to be transparent. Combining this with
		/// HasFilledBackground of true enables errors to draw on floating text shapes.
		/// </summary>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			if (HasTransparentBackground)
			{
				BrushSettings brushSettings = new BrushSettings();
				// ORMBaseShape sets the background to DiagramBackground, not ShapeBackground
				SolidBrush baseOnBrush = classStyleSet.GetBrush(DiagramBrushes.DiagramBackground) as SolidBrush;
				brushSettings.ForeColor = (baseOnBrush != null) ? Color.FromArgb(0, baseOnBrush.Color) : Color.Transparent;
				classStyleSet.OverrideBrush(DiagramBrushes.DiagramBackground, brushSettings);
			}
		}
		/// <summary>
		/// Allow the floating shape to both display errors
		/// and appear transparent when there are no associated errors.
		/// An element can be transparent all the time if HasFilledBackground
		/// returns false.
		/// </summary>
		protected virtual bool HasTransparentBackground
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Associate the value of the text field with a property
		/// specified by the derived class. Called one per class type.
		/// </summary>
		/// <param name="shapeFields">ShapeFieldCollection to initialized</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			Store store = Store;
			AutoSizeTextField textField = CreateAutoSizeTextField("TextField");
			textField.AssociateValueWith(Store, AssociatedModelDomainPropertyId);
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
		public override bool HasOutline
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Retrieve the property on the associated model element to be bound to
		/// the text field. This property should be xpath-bound to a property on
		/// the derived shape class. The opposite property is specified with the
		/// AssociatedShapeDomainPropertyId override. Defaults to NamedElement.NameDomainPropertyId
		/// </summary>
		protected virtual Guid AssociatedModelDomainPropertyId
		{
			get
			{
				return ORMNamedElement.NameDomainPropertyId;
			}
		}

		/// <summary>
		/// Method to allow inheritors to provide custom implementations of the AutoSizeTextField.
		/// </summary>
		/// <returns>The AutoSizeTextField to use.</returns>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected virtual AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			// Note: Don't add any other code here. The override
			// is here specifically to enable creation of a more
			// derived text field.
			return new AutoSizeTextField(fieldName);
		}
	}
}
