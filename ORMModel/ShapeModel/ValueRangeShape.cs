#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
	public partial class ValueConstraintShape : IModelErrorActivation, ISelectionContainerFilter, IDynamicColorGeometryHost, IDynamicColorAlsoUsedBy
	{
		private static AutoSizeTextField myTextShapeField;
		private string myDisplayText;

		#region Customize appearance
		/// <summary>
		/// A brush used to draw the value range text
		/// </summary>
		protected static readonly StyleSetResourceId ValueRangeTextBrush = new StyleSetResourceId("ORMArchitect", "ValueRangeTextBrush");
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
		/// <summary>
		/// Switch between alethic and deontic style sets
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				ValueConstraint constraint = AssociatedValueConstraint;
				return (constraint != null && constraint.Modality == ConstraintModality.Deontic && myDeonticStyleSet != InitializingDeonticSignal) ?
					DeonticStyleSet :
					base.StyleSet;
			}
		}
		/// <summary>
		/// A style set used for drawing deontic constraints. This is an instance
		/// style set (not a static class style set) because rendering this constraint
		/// uses fonts that are defined at the diagram instance level which need to be
		/// reflected in the deontic style set used for this instance.
		/// </summary>
		private StyleSet myDeonticStyleSet;

		/// <summary>
		/// MSBUG: During initialization, base.StyleSet ensures the class style set is initialized
		/// then calls CreateInstanceStyleSet, which calls InitializeInstanceStyleSetFonts, which
		/// calls the virtual StyleSet property to retrieve the style set it just created (instead
		/// of simply passing it down the call chain). This signal instance lets us ignore the
		/// nested request and enable initialization without using additional instance data.
		/// </summary>
		private static StyleSet InitializingDeonticSignal = new StyleSet(null);

		/// <summary>
		/// Create an alternate style set for deontic constraints
		/// </summary>
		protected virtual StyleSet DeonticStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticStyleSet;
				if (retVal == null)
				{
					// Set up an alternate style set for drawing deontic constraints
					myDeonticStyleSet = InitializingDeonticSignal;
					retVal = new StyleSet(base.StyleSet);
					InitializeDeonticStyleSet(retVal);
					myDeonticStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Initialize a <see cref="StyleSet"/> for rendering deontic constraints.
		/// The style set is created in <see cref="DeonticStyleSet"/> and
		/// initialized here.
		/// </summary>
		/// <remarks>(Currently there are no derived classes, so this is informational
		/// if a derived class is added for some reason).
		/// If a derived class does not modify additional resources in the
		/// default style set, then this method is not required and any derived deontic
		/// style set can be based on the deontic style set for this base class. However,
		/// if new resources are introduced, then the derived class should base a
		/// deontic style set on the derived class style set and reinitialize the
		/// deontic settings in that style set.</remarks>
		protected virtual void InitializeDeonticStyleSet(StyleSet styleSet)
		{
			IORMFontAndColorService colorService = (Store as IORMToolServices).FontAndColorService;
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = colorService.GetForeColor(ORMDesignerColor.DeonticConstraint);
			styleSet.OverrideBrush(ValueRangeTextBrush, brushSettings);
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
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ValueConstraintShape, ValueConstraint>[] providers;
			ValueConstraint element;
			Store store;
			// See notes in corresponding method on ExternalConstraintShape
			// regarding not using the dynamic background color.
			if (brushId == ValueRangeTextBrush &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ValueConstraintShape, ValueConstraint>>(true)) &&
				null != (element = (ValueConstraint)ModelElement))
			{
				ORMDiagramDynamicColor requestColor = element.Modality == ConstraintModality.Deontic ? ORMDiagramDynamicColor.DeonticConstraint : ORMDiagramDynamicColor.Constraint;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(requestColor, this, element);
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
		#region IDynamicColorAlsoUsedBy Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorAlsoUsedBy.RelatedDynamicallyColoredShapes"/>
		/// </summary>
		protected IEnumerable<ShapeElement> RelatedDynamicallyColoredShapes
		{
			get
			{
				if (AssociatedValueConstraint is RoleValueConstraint)
				{
					foreach (LinkConnectsToNode link in LinkConnectsToNode.GetLinksToLink(this))
					{
						ValueRangeLink constraintLink = link.Link as ValueRangeLink;
						if (constraintLink != null)
						{
							yield return constraintLink;
							// UNDONE: FactTypeShape should support ExternalConstraintRoleBarDisplay.AnyRole
							// for role value constraints. This needs to be updated when FactType
						}
					}
				}
			}
		}
		IEnumerable<ShapeElement> IDynamicColorAlsoUsedBy.RelatedDynamicallyColoredShapes
		{
			get
			{
				return RelatedDynamicallyColoredShapes;
			}
		}
		#endregion // IDynamicColorAlsoUsedBy Implementation
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
		/// </summary>
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
			if (createdDuringViewFixup)
			{
				SizeD size = Size;
				RectangleD parentBounds = ParentShape.AbsoluteBoundingBox;
				// Place slightly to the right. This will cause the label to
				// track in this position due to a horizontal resize of the
				// shape because of rename, etc
				Location = new PointD(parentBounds.Width + .06, -1 * size.Height);
			}
		}
		/// <summary>
		/// Overrides default implemenation to instantiate an value constraint specific one.
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
		{
			return new ValueRangeAutoSizeTextField(fieldName);
		}
		/// <summary>
		/// Indicate that tooltips are supported if the displayed values are truncated.
		/// </summary>
		public override bool HasToolTip
		{
			get
			{
				ValueConstraint constraint;
				int displayedValues;
				return (displayedValues = MaximumDisplayedValues) > 0 &&
					null != (constraint = AssociatedValueConstraint) &&
					constraint.ValueRangeCollection.Count > displayedValues;
			}
		}
		/// <summary>
		/// Show a tooltip containing the text for all values.
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			string retVal = null;
			ValueConstraint constraint;
			if (!IsDeleted &&
				null != (constraint = AssociatedValueConstraint))
			{
				retVal = constraint.GetDisplayText(MaximumDisplayedColumns, 0, false);
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = null;
				}
			}
			return retVal;
		}
		#endregion
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
		#region properties
		/// <summary>
		/// Constructs how the value range text should be displayed.
		/// </summary>
		public string DisplayText
		{
			get
			{
				string retVal = myDisplayText;
				if (retVal == null)
				{
					ValueConstraint defn = this.ModelElement as ValueConstraint;
					Debug.Assert(defn != null);
					retVal = defn.GetDisplayText(MaximumDisplayedColumns, MaximumDisplayedValues, true);
					myDisplayText = retVal;
				}
				return retVal;
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
			Guid domainPropertyId = e.DomainProperty.Id;
			if (domainPropertyId == ValueConstraint.TextChangedDomainPropertyId ||
				domainPropertyId == ValueConstraint.ModalityDomainPropertyId)
			{
				ValueConstraint constraint = (ValueConstraint)e.ModelElement;
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
		#region ValueConstraintShapeDisplayChangedRule
		/// <summary>
		/// ChangeRule: typeof(ValueConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void ValueConstraintShapeDisplayChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == ValueConstraintShape.MaximumDisplayedValuesDomainPropertyId || propertyId == ValueConstraintShape.MaximumDisplayedColumnsDomainPropertyId)
			{
				ValueConstraintShape valueConstraintShape = (ValueConstraintShape)e.ModelElement;
				if (!valueConstraintShape.IsDeleted)
				{
					valueConstraintShape.InvalidateDisplayText();
				}
			}
		}
		#endregion // ValueConstraintShapeDisplayChangedRule
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
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener ensures
		/// the correct size for value constraint shapes.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ValueConstraintShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset the size of value constraint shapes on load. Shapes are dependent
		/// on user settings not stored with the model.
		/// </summary>
		private sealed class ValueConstraintShapeFixupListener : DeserializationFixupListener<ValueConstraintShape>
		{
			/// <summary>
			/// Create a new ValueConstraintShapeFixupListener
			/// </summary>
			public ValueConstraintShapeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Update the shape size.
			/// </summary>
			protected sealed override void ProcessElement(ValueConstraintShape element, Store store, INotifyElementAdded notifyAdded)
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
				DefaultMultipleLine = true;
				StringFormat fieldFormat = new StringFormat(StringFormatFlags.NoClip);
				fieldFormat.Alignment = StringAlignment.Near;
				DefaultStringFormat = fieldFormat;
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
							break;
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
		}
		#endregion // ValueRangeAutoSizeTextField class
	}
}
