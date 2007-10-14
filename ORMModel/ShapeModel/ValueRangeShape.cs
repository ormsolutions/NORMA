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
using System.Diagnostics;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Diagrams;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ValueConstraintShape : IModelErrorActivation, ISelectionContainerFilter
	{
		private static AutoSizeTextField myTextShapeField;
		private string myDisplayText;

		#region Customize appearance
		/// <summary>
		/// A brush used to draw the value range text
		/// </summary>
		protected static readonly StyleSetResourceId ValueRangeTextBrush = new StyleSetResourceId("Neumont", "ValueRangeTextBrush");
		/// <summary>
		/// Initialize a pen and a brush for drawing the constraint
		/// outlines and contents.
		/// </summary>
		/// <param name="classStyleSet">StyleSet</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			classStyleSet.AddBrush(ValueRangeTextBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		#endregion // Customize appearance
		#region overrides
		/// <summary>
		/// Associate to the value range's text property
		/// </summary>
		protected override Guid AssociatedModelDomainPropertyId
		{
			get { return ValueConstraint.TextDomainPropertyId; }
		}
		/// <summary>
		/// Store per-type value for the base class
		/// </summary>
		protected override AutoSizeTextField TextShapeField
		{
			get
			{
				return myTextShapeField;
			}
			set
			{
				Debug.Assert(myTextShapeField == null); // This should only be called once per type
				myTextShapeField = value;
				myTextShapeField.DefaultTextBrushId = ValueRangeTextBrush;
			}
		}
		/// <summary>
		/// Get the ValueConstraint associated with this shape
		/// </summary>s
		public ValueConstraint AssociatedValueConstraint
		{
			get
			{
				return ModelElement as ValueConstraint;
			}
		}
		/// <summary>
		/// Return a shape geometry that handles center-to-center routing
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get { return CustomFoldRectangleShapeGeometry.ShapeGeometry; }
		}
		/// <summary>
		/// Move the name label above the parent shape
		/// </summary>
		public override void PlaceAsChildOf(NodeShape parent, bool createdDuringViewFixup)
		{
			AutoResize();
			SizeD size = Size;
			RectangleD parentBounds = ParentShape.AbsoluteBoundingBox;
			// Place slightly to the right. This will cause the label to
			// track in this position due to a horizontal resize of the
			// shape because of rename, etc
			Location = new PointD(parentBounds.Width + .06, -1 * size.Height);
		}
		/// <summary>
		/// Overrides default implemenation to instantiate an Reading specific one.
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			return new ValueRangeAutoSizeTextField(fieldName);
		}
		#endregion
		#region Helper methods
		/// <summary>
		/// Notifies the shape that the currently cached display text may no longer
		/// be accurate, so it needs to be recreated.
		/// </summary>
		private void InvalidateDisplayText()
		{
			Debug.Assert(Store.TransactionManager.InTransaction);
			myDisplayText = null;
			InvalidateRequired();
			this.AutoResize();
		}
		/// <summary>
		/// The constraint shape text needs to be refreshed before it is invalidated
		/// </summary>
		protected override void BeforeInvalidate()
		{
			myDisplayText = null;
		}
		#endregion // Helper methods
		#region properties
		/// <summary>
		/// Constructs how the value range text should be displayed.
		/// </summary>
		public String DisplayText
		{
			get
			{
				String retval = null;
				if (myDisplayText == null)
				{
					ValueConstraint defn = this.ModelElement as ValueConstraint;
					Debug.Assert(defn != null);
					retval = defn.Text;
					myDisplayText = retval;
				}
				else
				{
					retval = myDisplayText;
				}
				return retval;
			}
		}
		#endregion // properties
		#region Shape fixup rules
		#region ValueConstraintTextChangedRule
		/// <summary>
		/// ChangeRule: typeof(ValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Rule to watch the ValueConstraint.TextChanged property so that we can
		/// update text as needed. TextChanged is triggered when the ValueConstraint is
		/// first added, so this is sufficient for updating.
		/// </summary>
		private static void ValueConstraintTextChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ValueConstraint.TextChangedDomainPropertyId)
			{
				ValueConstraint constraint = e.ModelElement as ValueConstraint;
				if (!constraint.IsDeleted)
				{
					foreach (ShapeElement pel in PresentationViewsSubject.GetPresentation(constraint))
					{
						ValueConstraintShape valueConstraintShape = pel as ValueConstraintShape;
						if (valueConstraintShape != null)
						{
							valueConstraintShape.InvalidateDisplayText();
						}
					}
				}
			}
		}
		#endregion // ValueConstraintTextChangedRule
		#endregion // Shape fixup rules
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError.
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			// UNDONE: Automatically defer IModelErrorActivation to ModelErrorActivationService without explicit implementation
			IORMModelErrorActivationService activationService = (Store as IORMToolServices).ModelErrorActivationService;
			return activationService != null && activationService.ActivateError(ModelElement, error);
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region ISelectionContainerFilter Implementation
		/// <summary>
		/// Implements ISelectionContainerFilter.IncludeInSelectionContainer
		/// </summary>
		protected static bool IncludeInSelectionContainer
		{
			get
			{
				return false;
			}
		}
		bool ISelectionContainerFilter.IncludeInSelectionContainer
		{
			get
			{
				return IncludeInSelectionContainer;
			}
		}
		#endregion // ISelectionContainerFilter Implementation
		#region Mouse handling
		/// <summary>
		/// Attempt model error activation
		/// </summary>
		public override void OnDoubleClick(DiagramPointEventArgs e)
		{
			ORMBaseShape.AttemptErrorActivation(e);
			base.OnDoubleClick(e);
		}
		#endregion // Mouse handling
		#region ValueRangeAutoSizeTextField class
		/// <summary>
		/// Contains code to create a value range text field.
		/// </summary>
		private sealed class ValueRangeAutoSizeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Create a new ValueRangeAutoSizeTextField
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public ValueRangeAutoSizeTextField(string fieldName)
				: base(fieldName)
			{
			}
			/// <summary>
			/// Code that handles retrieval of the text to display in ValueConstraintShape.
			/// </summary>
			public sealed override string GetDisplayText(ShapeElement parentShape)
			{
				string retval = null;
				ValueConstraintShape parentValueConstraintShape = parentShape as ValueConstraintShape;
				if (parentShape is ObjectTypeShape)
				{
					LinkedElementCollection<PresentationElement> pelList = PresentationViewsSubject.GetPresentation(parentShape);
					foreach (ShapeElement pel in pelList)
					{
						ValueConstraintShape valueConstraintShape = pel as ValueConstraintShape;
						if (valueConstraintShape != null)
						{
							parentValueConstraintShape = valueConstraintShape;
						}
					}
				}
				if (parentValueConstraintShape == null)
				{
					retval = base.GetDisplayText(parentShape);
				}
				else
				{
					retval = parentValueConstraintShape.DisplayText;
				}
				return retval;
			}
			/// <summary>
			/// Changed to return true to get multiple line support.
			/// </summary>
			public sealed override bool GetMultipleLine(ShapeElement parentShape)
			{
				return true;
			}
		}
		#endregion // ValueRangeAutoSizeTextField class
	}
}
