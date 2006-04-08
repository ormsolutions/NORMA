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
using Neumont.Tools.ORM.ObjectModel.Editors;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ValueConstraintShape : IModelErrorActivation
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
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.Constraint);
			classStyleSet.AddBrush(ValueRangeTextBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		#endregion // Customize appearance
		#region overrides
		/// <summary>
		/// Associate the value range text with this shape
		/// </summary>
		protected override Guid AssociatedShapeMetaAttributeGuid
		{
			get { return ValueRangeTextMetaAttributeGuid; }
		}
		/// <summary>
		/// Associate to the value range's text attribute
		/// </summary>
		protected override Guid AssociatedModelMetaAttributeGuid
		{
			get { return ValueRange.TextMetaAttributeGuid; }
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
		public override void PlaceAsChildOf(NodeShape parent)
		{
			AutoResize();
			SizeD size = Size;
			RectangleD parentBounds = ParentShape.AbsoluteBoundingBox;
			Location = new PointD(parentBounds.Width, -1 * size.Height);
		}
		/// <summary>
		/// Changed to allow resizing of the label
		/// </summary>
		public override NodeSides ResizableSides
		{
			get { return NodeSides.All; }
		}
		/// <summary>
		/// Overrides default implemenation to instantiate an Reading specific one.
		/// </summary>
		protected override AutoSizeTextField CreateAutoSizeTextField()
		{
			return new ValueRangeAutoSizeTextField();
		}
		#endregion
		#region Helper methods
		/// <summary>
		/// Notifies the shape that the currently cached display text may no longer
		/// be accurate, so it needs to be recreated.
		/// </summary>
		private void InvalidateDisplayText()
		{
			Debug.Assert(TransactionManager.InTransaction);
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
		/// <summary>
		/// Invalidate the display text on all presentation role players associated
		/// with the given ValueConstraint.
		/// </summary>
		/// <param name="e">The ValueConstraint to update.</param>
		protected static void UpdatePresentationRolePlayers(ModelElement e)
		{
			if (e != null && !e.IsRemoved)
			{
				foreach (ShapeElement pel in e.PresentationRolePlayers)
				{
					ValueConstraintShape valueConstraintShape = pel as ValueConstraintShape;
					if (valueConstraintShape != null)
					{
						valueConstraintShape.InvalidateDisplayText();
					}
				}
			}
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
		#region change rules
		/// <summary>
		/// Rule to update an associated ValueConstraintShape when a DataType is added (or changed).
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ValueTypeHasDataTypeAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				base.ElementAdded(e);
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueTypeCollection;
				ValueConstraint defn = objectType.ValueConstraint;
				//Update the display on the objectType
				UpdatePresentationRolePlayers(defn);
				//Update the display on any attached roles
				if (objectType != null)
				{
					foreach (Role r in objectType.PlayedRoleCollection)
					{
						defn = r.ValueConstraint;
						UpdatePresentationRolePlayers(defn);
					}
				}
			}
		}
		/// <summary>
		/// Rule to notice changes to ValueRange properties so that the
		/// value range shapes can have their display text invalidated.
		/// </summary>
		[RuleOn(typeof(ValueRange), FireTime = TimeToFire.TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority)]
		private class ValueRangeChanged : ChangeRule
		{
			/// <summary>
			/// Notice when the Min or Max attributes are changed and invalidate
			/// display text of the ValueConstraintShapes.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attrId = e.MetaAttribute.Id;
				ValueRange valueRange = e.ModelElement as ValueRange;
				Debug.Assert(valueRange != null);
				if (attrId == ValueRange.MaxValueMetaAttributeGuid ||
					attrId == ValueRange.MinValueMetaAttributeGuid)
				{
					Debug.Assert(valueRange.ValueConstraint != null);
					ValueConstraint defn = valueRange.ValueConstraint;
					UpdatePresentationRolePlayers(defn);
				}
			}
		}
		/// <summary>
		/// Rule to notice the addition of ValueConstraintHasValueRange links so that the
		/// value range shapes can have their display text invalidated.
		/// </summary>
		[RuleOn(typeof(ValueConstraintHasValueRange), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ValueConstraintAdded : AddRule
		{
			/// <summary>
			/// Notice when the ValueConstraintHasValueRange link is added
			/// and invalidate display text of the ValueConstraintShapes.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueConstraintHasValueRange link = e.ModelElement as ValueConstraintHasValueRange;
				ValueConstraint defn = link.ValueConstraint;
				UpdatePresentationRolePlayers(defn);
			}
		}
		#endregion // change rules
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError. Forwards errors to
		/// associated fact type
		/// </summary>
		/// <param name="error">Activated model error</param>
		protected void ActivateModelError(ModelError error)
		{
			MaxValueMismatchError maxValueMismatchError;
			MinValueMismatchError minValueMismatchError;
			ValueRangeOverlapError overlapError;
			ConstraintDuplicateNameError duplicateName;
			ValueConstraint errorValueConstraint = null;
			if (null != (maxValueMismatchError = error as MaxValueMismatchError))
			{
				errorValueConstraint = maxValueMismatchError.ValueRange.ValueConstraint;
			}
			else if (null != (minValueMismatchError = error as MinValueMismatchError))
			{
				errorValueConstraint = minValueMismatchError.ValueRange.ValueConstraint;
			}
			else if (null != (overlapError = error as ValueRangeOverlapError))
			{
				errorValueConstraint = overlapError.ValueConstraint;
			}
			else if (null != (duplicateName = error as ConstraintDuplicateNameError))
			{
				ActivateNameProperty((NamedElement)duplicateName.ConstraintCollection[0]);
			}
			if (errorValueConstraint != null)
			{
				Store store = Store;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					errorValueConstraint.CreatePropertyDescriptor(store.MetaDataDirectory.FindMetaAttribute(ValueConstraint.TextMetaAttributeGuid), this),
					false);
			}
		}
		void IModelErrorActivation.ActivateModelError(ModelError error)
		{
			ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
	#region ValueRangeAutoSizeTextField class
	/// <summary>
	/// Contains code to create a value range text field.
	/// </summary>
	public class ValueRangeAutoSizeTextField : AutoSizeTextField
	{
		/// <summary>
		/// Code that handles retrieval of the text to display in ValueConstraintShape.
		/// </summary>
		public override string GetDisplayText(ShapeElement parentShape)
		{
			string retval = null;
			ValueConstraintShape parentValueConstraintShape = parentShape as ValueConstraintShape;
			if (parentShape is ObjectTypeShape)
			{
				PresentationElementMoveableCollection pelList = parentShape.PresentationRolePlayers;
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
		public override bool GetMultipleLine(ShapeElement parentShape)
		{
			return true;
		}
	}
	#endregion // ValueRangeAutoSizeTextField class
}
