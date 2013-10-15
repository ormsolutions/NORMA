#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class CardinalityConstraintShape : ISelectionContainerFilter, IDynamicColorGeometryHost, IModelErrorActivation
	{
		#region Fields
		private static AutoSizeTextField myTextShapeField;
		private string myDisplayText;
		#endregion // Fields
		#region Customize appearance
		/// <summary>
		/// A brush used to draw the cardinality range text
		/// </summary>
		protected static readonly StyleSetResourceId CardinalityTextAlethicBrush = new StyleSetResourceId("ORMArchitect", "CardinalityTextAlethicBrush");
		/// <summary>
		/// A brush used to draw the cardinality range text
		/// </summary>
		protected static readonly StyleSetResourceId CardinalityTextDeonticBrush = new StyleSetResourceId("ORMArchitect", "CardinalityTextDeonticBrush");
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
			classStyleSet.AddBrush(CardinalityTextAlethicBrush, DiagramBrushes.ShapeBackground, brushSettings);
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
			classStyleSet.AddBrush(CardinalityTextDeonticBrush, DiagramBrushes.ShapeBackground, brushSettings);
		}
		/// <summary>
		/// Set ZOrder layer
		/// </summary>
		public override double ZOrder
		{
			get
			{
				return base.ZOrder + ZOrderLayer.ValueConstraintShapes;
			}
		}
		#endregion // Customize appearance
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected static Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			return Color.Empty;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			return UpdateDynamicColor(penId, pen);
		}
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Brush)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			Color retVal = Color.Empty;
			SolidBrush solidBrush;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, CardinalityConstraintShape, CardinalityConstraint>[] providers;
			CardinalityConstraint element;
			bool isAlethic;
			Store store;
			// See notes in corresponding method on ExternalConstraintShape
			// regarding not using the dynamic background color.
			if (((isAlethic = (brushId == CardinalityTextAlethicBrush)) || brushId == CardinalityTextDeonticBrush) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, CardinalityConstraintShape, CardinalityConstraint>>()) &&
				null != (element = (CardinalityConstraint)ModelElement))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(isAlethic ? ORMDiagramDynamicColor.Constraint : ORMDiagramDynamicColor.DeonticConstraint, this, element);
					if (alternateColor != Color.Empty)
					{
						retVal = solidBrush.Color;
						solidBrush.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
		}
		Color IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId brushId, Brush brush)
		{
			return UpdateDynamicColor(brushId, brush);
		}
		#endregion // IDynamicColorGeometryHost Implementation
		#region Base overrides
		/// <summary>
		/// Associate to the value range's text property
		/// </summary>
		protected override Guid AssociatedModelDomainPropertyId
		{
			get { return CardinalityConstraint.TextDomainPropertyId; }
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
				myTextShapeField.DefaultTextBrushId = CardinalityTextAlethicBrush;
			}
		}
		/// <summary>
		/// Get the CardinalityConstraint associated with this shape
		/// </summary>
		public CardinalityConstraint AssociatedCardinalityConstraint
		{
			get
			{
				return ModelElement as CardinalityConstraint;
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
			if (createdDuringViewFixup)
			{
				SizeD size = Size;
				RectangleD parentBounds = ParentShape.AbsoluteBoundingBox;
				// Place slightly to the right and aligned with the top
				// of the shape. This will place the cardinality shape
				// below any value ranges.
				// For a fact type shape with a role cardinality, we want
				// to align with the top of the current roles shape field,
				// not the top of the shape itself.
				double top = 0d;
				FactTypeShape factTypeShape;
				ShapeField rolesField;
				if (ModelElement is UnaryRoleCardinalityConstraint &&
					null != (factTypeShape = parent as FactTypeShape) &&
					null != (rolesField = FactTypeShape.RolesField))
				{
					top = rolesField.GetBounds(factTypeShape).Top;
				}
				Location = new PointD(parentBounds.Width + .06, top);
			}
		}
		/// <summary>
		/// Overrides default implemenation to instantiate an value constraint specific one.
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			return new CardinalityAutoSizeTextField(fieldName);
		}
		#endregion // Base overrides
		#region Helper methods
		/// <summary>
		/// Notifies the shape that the currently cached display text may no longer
		/// be accurate, so it needs to be recreated.
		/// </summary>
		public void InvalidateDisplayText()
		{
			BeforeInvalidate();
			if (Store.TransactionManager.InTransaction)
			{
				AutoResize();
				InvalidateRequired(true);
			}
		}
		/// <summary>
		/// The constraint shape text needs to be refreshed before it is invalidated
		/// </summary>
		protected override void BeforeInvalidate()
		{
			myDisplayText = null;
		}
		#endregion // Helper methods
		#region Properties
		/// <summary>
		/// Constructs how the value range text should be displayed when it is not being edited.
		/// </summary>
		public string DisplayText
		{
			get
			{
				string retVal = myDisplayText;
				if (retVal == null)
				{
					CardinalityConstraint constraint = (CardinalityConstraint)this.ModelElement;
					LinkedElementCollection<CardinalityRange> ranges = constraint.RangeCollection;
					CultureInfo culture = CultureInfo.CurrentCulture;
					bool isDeontic = constraint.Modality == ConstraintModality.Deontic;
					int count = ranges.Count;
					if (count == 1)
					{
						retVal = string.Format(culture, isDeontic ? ResourceStrings.CardinalityConstraintShapeTextWrapperSingleRangeDeontic : ResourceStrings.CardinalityConstraintShapeTextWrapperSingleRangeAlethic, GetDisplayedRangeText(ranges[0], true));
					}
					else
					{
						string[] rangeFields = new string[count];
						for (int i = 0; i < count; ++i)
						{
							rangeFields[i] = GetDisplayedRangeText(ranges[i], false);
						}
						retVal = string.Format(culture, isDeontic ? ResourceStrings.CardinalityConstraintShapeTextWrapperMultiRangeDeontic : ResourceStrings.CardinalityConstraintShapeTextWrapperMultiRangeAlethic, string.Join(culture.TextInfo.ListSeparator + " ", rangeFields));
					}
					myDisplayText = retVal;
				}
				return retVal;
			}
		}
		private static string GetDisplayedRangeText(CardinalityRange range, bool singleRange)
		{
			int lower = range.LowerBound;
			int upper = range.UpperBound;
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			if (lower == upper)
			{
				return string.Format(invariantCulture, singleRange ? ResourceStrings.CardinalityConstraintShapeRangeSingleValuedForSingleRange : ResourceStrings.CardinalityConstraintShapeRangeSingleValuedForMultiRange, lower);
			}
			else if (lower == 0)
			{
				return string.Format(invariantCulture, ResourceStrings.CardinalityConstraintShapeRangeUnboundedBelow, upper);
			}
			else if (upper == -1)
			{
				return string.Format(invariantCulture, ResourceStrings.CardinalityConstraintShapeRangeUnboundedAbove, lower);
			}
			return string.Format(invariantCulture, ResourceStrings.CardinalityConstraintShapeRangeFullyBounded, lower, upper);
		}
		#endregion // Properties
		#region Shape fixup rules
		#region ValueConstraintTextChangedRule
		/// <summary>
		/// ChangeRule: typeof(CardinalityConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Rule to watch the CardinalityConstraint.TextChanged property so that we can
		/// update text as needed. TextChanged is triggered when the CardinalityConstraint is
		/// first added, so this is sufficient for updating.
		/// </summary>
		private static void CardinalityConstraintTextChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == CardinalityConstraint.TextChangedDomainPropertyId)
			{
				CardinalityConstraint constraint = (CardinalityConstraint)e.ModelElement;
				if (!constraint.IsDeleted)
				{
					foreach (ShapeElement pel in PresentationViewsSubject.GetPresentation(constraint))
					{
						CardinalityConstraintShape cardinalityConstraintShape = pel as CardinalityConstraintShape;
						if (cardinalityConstraintShape != null)
						{
							cardinalityConstraintShape.InvalidateDisplayText();
						}
					}
				}
			}
		}
		#endregion // ValueConstraintTextChangedRule
		#endregion // Shape fixup rules
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
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener ensures
		/// the correct size for value constraint shapes.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new CardinalityConstraintShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset the size of value constraint shapes on load. Shapes are dependent
		/// on user settings not stored with the model.
		/// </summary>
		private sealed class CardinalityConstraintShapeFixupListener : DeserializationFixupListener<CardinalityConstraintShape>
		{
			/// <summary>
			/// Create a new CardinalityConstraintShapeFixupListener
			/// </summary>
			public CardinalityConstraintShapeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Update the shape size.
			/// </summary>
			protected sealed override void ProcessElement(CardinalityConstraintShape element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					element.AutoResize();
				}
			}
		}
		#endregion // Deserialization Fixup
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
		#region CardinalityAutoSizeTextField class
		/// <summary>
		/// Contains code to create a value range text field.
		/// </summary>
		private sealed class CardinalityAutoSizeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Create a new CardinalityAutoSizeTextField
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public CardinalityAutoSizeTextField(string fieldName)
				: base(fieldName)
			{
			}
			/// <summary>
			/// Adjust the brush used based on the modality
			/// </summary>
			public override StyleSetResourceId GetTextBrushId(DiagramClientView view, ShapeElement parentShape)
			{
				CardinalityConstraintShape constraintShape;
				CardinalityConstraint constraint;
				return (null != (constraintShape = parentShape as CardinalityConstraintShape) &&
					null != (constraint = constraintShape.AssociatedCardinalityConstraint) &&
					constraint.Modality == ConstraintModality.Deontic) ?
						CardinalityTextDeonticBrush :
						CardinalityTextAlethicBrush;
			}
			/// <summary>
			/// Code that handles retrieval of the text to display in CardinalityConstraintShape while editing.
			/// </summary>
			public sealed override string GetDisplayText(ShapeElement parentShape)
			{
				CardinalityConstraintShape parentCardinalityConstraintShape = parentShape as CardinalityConstraintShape;
				return (parentCardinalityConstraintShape != null) ? parentCardinalityConstraintShape.DisplayText : base.GetDisplayText(parentShape);
			}
		}
		#endregion // CardinalityAutoSizeTextField class
	}
}
