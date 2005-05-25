#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Northface.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

#endregion

namespace Northface.Tools.ORM.ShapeModel
{
	public partial class ValueRangeShape : IModelErrorActivation
	{
		private static AutoSizeTextField myTextShapeField;
		private string myDisplayText;

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
		[CLSCompliant(false)]
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
			}
		}
		/// <summary>
		/// Get the ObjectType associated with this shape
		/// </summary>s
		public ValueRangeDefinition AssociatedRangeDefinition
		{
			get
			{
				return ModelElement as ValueRangeDefinition;
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
		/// <param name="fixupState">BoundsFixupState</param>
		/// <param name="iteration">int</param>
		public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration)
		{
			base.OnBoundsFixup(fixupState, iteration);
			if (fixupState != BoundsFixupState.Invalid)
			{
				SizeD size = Size;
				Location = new PointD(0, -1.5 * size.Height);
			}
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
		[CLSCompliant(false)]
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
			myDisplayText = null;
			this.Invalidate();
			//this is triggering code that needs a transaction
			if (Store.TransactionManager.InTransaction)
			{
				this.AutoResize();
			}
		}
		/// <summary>
		/// Invalidate the display text on all presentation role players associated
		/// with the given ValueRangeDefinition.
		/// </summary>
		/// <param name="e">The ValueRangeDefinition to update.</param>
		protected static void UpdatePresentationRolePlayers(ModelElement e)
		{
			if (e != null && !e.IsRemoved)
			{
				foreach (ShapeElement pel in e.PresentationRolePlayers)
				{
					ValueRangeShape valueRangeShape = pel as ValueRangeShape;
					if (valueRangeShape != null)
					{
						valueRangeShape.InvalidateDisplayText();
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
					ValueRangeDefinition defn = this.ModelElement as ValueRangeDefinition;
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
		/// Rule to update an associated ValueRangeShape when a DataType is added (or changed).
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ValueTypeHasDataTypeAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				base.ElementAdded(e);
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueTypeCollection;
				ValueRangeDefinition defn = objectType.ValueRangeDefinition;
				//Update the display on the objectType
				UpdatePresentationRolePlayers(defn);
				//Update the display on any attached roles
				if (objectType != null)
				{
					foreach (Role r in objectType.PlayedRoleCollection)
					{
						defn = r.ValueRangeDefinition;
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
			/// display text of the ValueRangeShapes.
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attrId = e.MetaAttribute.Id;
				ValueRange valueRange = e.ModelElement as ValueRange;
				Debug.Assert(valueRange != null);
				if (attrId == ValueRange.MaxValueMetaAttributeGuid ||
					attrId == ValueRange.MinValueMetaAttributeGuid)
				{
					Debug.Assert(valueRange.ValueRangeDefinition != null);
					ValueRangeDefinition defn = valueRange.ValueRangeDefinition;
					UpdatePresentationRolePlayers(defn);
				}
			}
		}
		/// <summary>
		/// Rule to notice the addition of ValueRangeDefinitionHasValueRange links so that the
		/// value range shapes can have their display text invalidated.
		/// </summary>
		[RuleOn(typeof(ValueRangeDefinitionHasValueRange), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ValueRangeDefinitionAdded : AddRule
		{
			/// <summary>
			/// Notice when the ValueRangeDefinitionHasValueRange link is added
			/// and invalidate display text of the ValueRangeShapes.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueRangeDefinitionHasValueRange link = e.ModelElement as ValueRangeDefinitionHasValueRange;
				ValueRangeDefinition defn = link.ValueRangeDefinition;
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
			IModelErrorActivation parent = ParentShape as IModelErrorActivation;
			if (parent != null)
			{
				parent.ActivateModelError(error);
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
	/// Contains code to replace RolePlayer place holders with an ellipsis.
	/// </summary>
	public class ValueRangeAutoSizeTextField : AutoSizeTextField
	{
		/// <summary>
		/// Code that handles retrieval of the text to display in ValueRangeShape.
		/// </summary>
		public override string GetDisplayText(ShapeElement parentShape)
		{
			string retval = null;
			ValueRangeShape parentValueRangeShape = parentShape as ValueRangeShape;
			if (parentShape is ObjectTypeShape)
			{
				PresentationElementMoveableCollection pelList = parentShape.PresentationRolePlayers;
				foreach (ShapeElement pel in pelList)
				{
					ValueRangeShape valueRangeShape = pel as ValueRangeShape;
					if (valueRangeShape != null)
					{
						parentValueRangeShape = valueRangeShape;
					}
				}
			}
			if (parentValueRangeShape == null)
			{
				retval = base.GetDisplayText(parentShape);
			}
			else
			{
				retval = parentValueRangeShape.DisplayText;
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
