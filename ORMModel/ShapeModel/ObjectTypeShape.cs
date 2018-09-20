#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Design;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ObjectTypeShape : IModelErrorActivation, IDynamicColorGeometryHost, IDynamicColorAlsoUsedBy, IConfigureableLinkEndpoint, IDisplayMultiplePresentations
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField;
		private static AutoSizeTextField myReferenceModeShapeField;
		// Margin constants duplicated in FactTypeShape for 'DisplayAsObjectType' option. Keep in sync.
		private const double HorizontalMargin = 0.060;
		private const double VerticalMargin = 0.050;
		private static readonly StyleSetResourceId DashedShapeOutlinePen = new StyleSetResourceId("ORMArchitect", "DashedShapeOutlinePen");
		#endregion // Member Variables
		#region DisplayFlags
		/// <summary>
		/// Bitfield for display settings. All flags are assumed to default to false. 
		/// </summary>
		[Flags]
		private enum DisplayFlags
		{
			/// <summary>
			/// Corresponds to the ExpandRefMode property
			/// </summary>
			ExpandRefMode = 1,
			/// <summary>
			/// Corresponds to the suptypes part of the DisplayRelatedTypes property
			/// </summary>
			HideSubtypes = 2,
			/// <summary>
			/// Corresponds to the supertypes part of the DisplayRelatedTypes property
			/// </summary>
			HideSupertypes = 4,
		}
		private DisplayFlags myDisplayFlags;
		/// <summary>
		/// Test if a display flag is set
		/// </summary>
		private bool GetDisplayFlag(DisplayFlags flag)
		{
			return 0 != (myDisplayFlags & flag);
		}
		/// <summary>
		/// Set a value for a display flag. Returns true if the flag value changed.
		/// </summary>
		private bool SetDisplayFlag(DisplayFlags flag, bool value)
		{
			if (value)
			{
				if ((myDisplayFlags & flag) != flag)
				{
					myDisplayFlags |= flag;
					return true;
				}
			}
			else if (0 != (myDisplayFlags & flag))
			{
				myDisplayFlags &= ~flag;
				return true;
			}
			return false;
		}
		#endregion // DisplayFlags
		#region Customize appearance
		/// <summary>
		/// Show a shadow if this <see cref="ObjectTypeShape"/> represents an <see cref="ObjectType"/> that appears
		/// in more than one location.
		/// </summary>
		public override bool HasShadow
		{
			get
			{
				return OptionsPage.CurrentDisplayShadows && MultiShapeUtility.ElementHasMultiplePresentations(this);
			}
		}
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
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>[] providers;
			ObjectType element;
			Store store;
			if ((penId == DashedShapeOutlinePen || penId == DiagramPens.ShapeOutline) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>>(true)) &&
				null != (element = (ObjectType)ModelElement))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.Outline, this, element);
					if (alternateColor != Color.Empty)
					{
						retVal = pen.Color;
						pen.Color = alternateColor;
						break;
					}
				}
			}
			return retVal;
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
			ObjectType element;
			Store store;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>[] providers;
			bool isBackgroundBrush;
			if (((isBackgroundBrush = brushId == DiagramBrushes.DiagramBackground) ||
				brushId == DiagramBrushes.ShapeTitleText) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>>(true)) &&
				null != (element = (ObjectType)ModelElement))
			{
				ORMDiagramDynamicColor requestColor = isBackgroundBrush ? ORMDiagramDynamicColor.Background : ORMDiagramDynamicColor.ForegroundText;
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
				foreach (RolePlayerLink linkShape in MultiShapeUtility.GetEffectiveAttachedLinkShapesTo<RolePlayerLink>(this))
				{
					yield return linkShape;
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
		/// <summary>
		/// Add a dashed pen to the class resource set
		/// </summary>
		/// <param name="classStyleSet">Shared class styleset instance</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);
			PenSettings settings = new PenSettings();
			settings.Color = Color.DarkBlue;
			settings.Width = 1.2f / 72f; // 1.2 point
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, settings);
			settings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(DashedShapeOutlinePen, DiagramPens.ShapeOutline, settings);
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
				switch (Shell.OptionsPage.CurrentObjectTypeDisplayShape)
				{
					case Shell.ObjectTypeDisplayShape.Ellipse:
						useShape = CustomFoldEllipseShapeGeometry.ShapeGeometry;
						break;
					case Shell.ObjectTypeDisplayShape.HardRectangle:
						useShape = CustomFoldRectangleShapeGeometry.ShapeGeometry;
						break;
					case Shell.ObjectTypeDisplayShape.SoftRectangle:
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
				RectangleD oldBounds = (RectangleD)AbsoluteBounds;
				if (!(oldBounds.IsEmpty ||
					oldBounds.Size == DefaultSize ||
					Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(ORMBaseShape.PlaceAllChildShapes)))
				{
					SizeD oldSize = oldBounds.Size;
					double xDelta = contentSize.Width - oldSize.Width;
					double yDelta = contentSize.Height - oldSize.Height;
					bool xChanged = !VGConstants.FuzzZero(xDelta, VGConstants.FuzzDistance);
					bool yChanged = !VGConstants.FuzzZero(yDelta, VGConstants.FuzzDistance);
					if (xChanged || yChanged)
					{
						PointD location = oldBounds.Location;
						location.Offset(xChanged ? -xDelta / 2 : 0d, yChanged ? -yDelta / 2 : 0d);
						AbsoluteBounds = new RectangleD(location, contentSize);
						return;
					}
				}
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
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize field
			AutoSizeTextField nameField = CreateObjectNameTextField("ObjectTypeName");
			nameField.DrawBorder = false;
			nameField.FillBackground = false;
			nameField.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			nameField.DefaultPenId = DiagramPens.ShapeOutline;
			nameField.DefaultFontId = DiagramFonts.ShapeTitle;
			nameField.DefaultFocusable = true;
			nameField.DefaultText = "Object";

			StringFormat fieldFormat = new StringFormat(StringFormatFlags.NoClip);
			fieldFormat.Alignment = StringAlignment.Center;
			nameField.DefaultStringFormat = fieldFormat;
			nameField.AssociateValueWith(Store, ORMNamedElement.NameDomainPropertyId);

			// Initialize reference mode field
			AutoSizeTextField referenceModeField = CreateReferenceModeTextField("ObjectTypeRefMode");
			referenceModeField.DrawBorder = false;
			referenceModeField.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			referenceModeField.DefaultPenId = DiagramPens.ShapeOutline;
			referenceModeField.DefaultFontId = DiagramFonts.ShapeTitle;
			referenceModeField.DefaultText = string.Empty;
			referenceModeField.DefaultStringFormat = fieldFormat;
			// Note that the reference mode field is associated with the ReferenceModeDecoratedString
			// property, not ReferenceModeDisplay. The field will only activate for editing
			// if it is a string property.
			referenceModeField.AssociateValueWith(Store, ObjectType.ReferenceModeDecoratedStringDomainPropertyId);

			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(nameField);
			shapeFields.Add(referenceModeField);

			// Modify field anchoring behavior
			AnchoringBehavior anchor = nameField.AnchoringBehavior;
			anchor.SetTopAnchor(AnchoringBehavior.Edge.Top, VerticalMargin);
			anchor.CenterHorizontally();

			// Modify reference mode field anchoring behavior
			anchor = referenceModeField.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.SetTopAnchor(nameField, AnchoringBehavior.Edge.Bottom, 0);

			Debug.Assert(myTextShapeField == null); // Only called once
			myTextShapeField = nameField;
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
			ObjectType objectType;
			if (null != (objectType = AssociatedObjectType))
			{
				RoleValueConstraint roleValueConstraint;
				ValueTypeValueConstraint valueTypeValueConstraint;
				ObjectTypeCardinalityConstraint cardinalityConstraint;
				if (null != (valueTypeValueConstraint = element as ValueTypeValueConstraint))
				{
					return valueTypeValueConstraint.ValueType == objectType;
				}
				else if (null != (roleValueConstraint = element as RoleValueConstraint))
				{
					FactType refModeFactType;
					return !ExpandRefMode &&
						null != (refModeFactType = objectType.ReferenceModeFactType) &&
						refModeFactType == roleValueConstraint.Role.FactType;
				}
				else if (null != (cardinalityConstraint = element as ObjectTypeCardinalityConstraint))
				{
					return cardinalityConstraint.ObjectType == objectType;
				}
			}
			IShapeExtender<ObjectTypeShape>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IShapeExtender<ObjectTypeShape>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].ShouldAddShapeForElement(this, element))
					{
						return true;
					}
				}
			}
			return base.ShouldAddShapeForElement(element);
		}
		/// <summary>
		/// Indicate that we support tool tips. Used for showing
		/// definition information.
		/// </summary>
		public override bool HasToolTip
		{
			get
			{
				return OptionsPage.CurrentDisplayDefinitionTooltips;
			}
		}
		/// <summary>
		/// Show a tooltip containing the element definition text.
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			string retVal = null;
			ObjectType objectType;
			// Show for all shapes and fields in item.
			// Make sure that the shape has not been destroyed between the tooltip text
			// timer starting and firing.
			if (!IsDeleted &&
				null != (objectType = AssociatedObjectType) &&
				(retVal = objectType.DefinitionText).Length == 0)
			{
				retVal = null;
			}
			return retVal;
		}
		/// <summary>
		/// Set ZOrder layer
		/// </summary>
		public override double ZOrder
		{
			get
			{
				return base.ZOrder + ZOrderLayer.ObjectTypeShapes;
			}
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
			//Debug.Assert(childShape is ValueConstraintShape || childShape is CardinalityConstraintShape);
			return RelationshipType.Relative;
		}

		#endregion // ObjectTypeShape specific
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
		/// Implements IModelErrorActivation.ActivateModelError for DataTypeNotSpecifiedError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			DataTypeNotSpecifiedError dataTypeError;
			EntityTypeRequiresReferenceSchemeError requiresReferenceSchemeError;
			ObjectTypeDuplicateNameError duplicateName;
			PopulationMandatoryError populationMandatory;
			bool retVal = true;
			if (null != (dataTypeError = error as DataTypeNotSpecifiedError))
			{
				Store store = Store;
				ObjectType valueType = dataTypeError.ValueTypeHasDataType.ValueType;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					ObjectTypeTypeDescriptor.DataTypeDisplayPropertyDescriptor,
					true);
			}
			else if (null != (requiresReferenceSchemeError = error as EntityTypeRequiresReferenceSchemeError))
			{
				Store store = Store;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					DomainTypeDescriptor.CreatePropertyDescriptor(requiresReferenceSchemeError.ObjectType, ObjectType.ReferenceModeDisplayDomainPropertyId),
					true);
			}
			else if (null != (duplicateName = error as ObjectTypeDuplicateNameError))
			{
				ActivateNameProperty(duplicateName.ObjectTypeCollection[0]);
			}
			else if (null != (populationMandatory = error as PopulationMandatoryError))
			{
				retVal = ORMDesignerPackage.SamplePopulationEditorWindow.AutoCorrectMandatoryError(populationMandatory, AssociatedObjectType);
			}
			else if (error is TooFewEntityTypeRoleInstancesError)
			{
				retVal = ORMDesignerPackage.SamplePopulationEditorWindow.ActivateModelError(error);
			}
			else
			{
				retVal = false;
			}
			return retVal;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region Shape display update rules
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Iif primaryidentifier for a Entity type is 
		/// removed and ref mode is not expanded, then resize the entity type
		/// </summary>
		private static void ObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			ObjectType changeType = null;
			if (attributeGuid == ORMNamedElement.NameDomainPropertyId)
			{
				// Figure out if we need to resize related object shapes. This happens
				// when we've renamed a value type that is bound to the reference scheme
				// of an entity type.
				changeType = e.ModelElement as ObjectType;

				if (changeType.IsValueType)
				{
					LinkedElementCollection<Role> playedRoles = changeType.PlayedRoleCollection;
					int roleCount = playedRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						Role currentRole = playedRoles[i];
						LinkedElementCollection<ConstraintRoleSequence> roleConstraints = currentRole.ConstraintRoleSequenceCollection;
						int constraintCount = roleConstraints.Count;
						for (int j = 0; j < constraintCount; ++j)
						{
							ConstraintRoleSequence currentConstraintRoleSequence = roleConstraints[j];
							IConstraint associatedConstraint = currentConstraintRoleSequence.Constraint;
							if (associatedConstraint.ConstraintType == ConstraintType.InternalUniqueness)
							{
								ResizeAssociatedShapes(associatedConstraint.PreferredIdentifierFor);
							}
						}
					}
				}
			}
			else if (attributeGuid == ObjectType.IsIndependentDomainPropertyId)
			{
				changeType = e.ModelElement as ObjectType;
			}
			if (changeType != null)
			{
				ResizeAssociatedShapes(changeType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeKind), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// A reference mode kind format string can be changed in such a way
		/// that it matches preexisting reference scheme patterns, requiring
		/// possible resizing of the associated entity types.
		/// </summary>
		private static void ReferenceModeKindChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ReferenceModeKind.FormatStringDomainPropertyId)
			{
				ReferenceModeKind kind;
				if (!(kind = (ReferenceModeKind)e.ModelElement).IsDeleted)
				{
					foreach (ReferenceMode mode in kind.ReferenceModeCollection)
					{
						foreach (ObjectType objectType in mode.AssociatedEntityTypeCollection(null, null))
						{
							ResizeAssociatedShapes(objectType);
						}
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.CustomReferenceMode), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// The reference custom format string can be changed in such a way
		/// that it matches preexisting reference scheme patterns, requiring
		/// possible resizing of the associated entity types.
		/// </summary>
		private static void CustomReferenceModeChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == CustomReferenceMode.CustomFormatStringDomainPropertyId)
			{
				ReferenceMode mode;
				if (!(mode = (CustomReferenceMode)e.ModelElement).IsDeleted)
				{
					foreach (ObjectType objectType in mode.AssociatedEntityTypeCollection(null, null))
					{
						ResizeAssociatedShapes(objectType);
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Changing a reference mode type can force it to match the pattern for
		/// an existing entity type reference scheme.
		/// </summary>
		private static void ReferenceModeKindRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ReferenceMode mode;
			if (e.DomainRole.Id == ReferenceModeHasReferenceModeKind.KindDomainRoleId &&
				!(mode = ((ReferenceModeHasReferenceModeKind)e.ElementLink).ReferenceMode).IsDeleted)
			{
				foreach (ObjectType objectType in mode.AssociatedEntityTypeCollection(null, null))
				{
					ResizeAssociatedShapes(objectType);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Keep relative child elements a fixed distance away from the object type shape
		/// when the shape changes.
		/// </summary>
		private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			MaintainRelativeShapeOffsetsForBoundsChange(e);
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
		{
			ProcessPreferredIdentifierDelete(e.ModelElement as EntityTypeHasPreferredIdentifier, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessPreferredIdentifierDelete(EntityTypeHasPreferredIdentifier link, ObjectType objectType)
		{
			if (objectType == null)
			{
				objectType = link.PreferredIdentifierFor;
			}
			if (!objectType.IsDeleted)
			{
				ResizeAssociatedShapes(objectType);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangeRuleForResizeRule(RolePlayerChangedEventArgs e)
		{
			ObjectType oldObjectType = null;
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			ProcessPreferredIdentifierDelete(e.ElementLink as EntityTypeHasPreferredIdentifier, oldObjectType);
		}
		/// <summary>
		/// Resize shapes for the provided object type
		/// </summary>
		/// <param name="objectType">The associated model element</param>
		private static void ResizeAssociatedShapes(ObjectType objectType)
		{
			if (objectType != null && !objectType.IsDeleted)
			{
				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(objectType);
				int pelCount = pels.Count;
				for (int i = 0; i < pelCount; ++i)
				{
					// ORMBaseShape Picks up ObjectTypeShape, ObjectifiedFactTypeNameShape
					ORMBaseShape objectShape = pels[i] as ORMBaseShape;
					if (null != objectShape)
					{
						SizeD oldSize = objectShape.Size;
						objectShape.AutoResize();
						if (oldSize == objectShape.Size)
						{
							((IInvalidateDisplay)objectShape).InvalidateRequired(true);
						}
					}
				}
				FactType factType = objectType.NestedFactType;
				if (factType != null)
				{
					// Pick up objectified fact type shapes displayed as object type shapes
					pels = PresentationViewsSubject.GetPresentation(factType);
					pelCount = pels.Count;
					for (int i = 0; i < pelCount; ++i)
					{
						FactTypeShape factTypeShape;
						if (null != (factTypeShape = pels[i] as FactTypeShape) &&
							factTypeShape.DisplayAsObjectType)
						{
							SizeD oldSize = factTypeShape.Size;
							factTypeShape.AutoResize();
							if (oldSize == factTypeShape.Size)
							{
								((IInvalidateDisplay)factTypeShape).InvalidateRequired(true);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Ensure that the ExpandRefMode property is set. Called when a
		/// reference mode pattern is added to the model manually on
		/// shapes that are already expanded.
		/// </summary>
		/// <param name="constraint">The constraint providing a preferred identifier</param>
		/// <param name="preferredIdentifierFor">The ObjectType from the core model</param>
		private static void EnsureRefModeExpanded(UniquenessConstraint constraint, ObjectType preferredIdentifierFor)
		{
			Debug.Assert(constraint != null); // Check before call
			Debug.Assert(preferredIdentifierFor != null); // Check before call
			FactType identifyingFactType = constraint.FactTypeCollection[0];
			ObjectTypePlaysRole rolePlayerLink = null;
			//Get the object that represents the item with the preferred identifier. 
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(preferredIdentifierFor))
			{
				ObjectTypeShape objectShape;
				ObjectifiedFactTypeNameShape objectifiedShape;
				if (rolePlayerLink == null)
				{
					Role oppositeRole;
					if (null == (oppositeRole = constraint.RoleCollection[0].OppositeRole as Role) ||
						null == (rolePlayerLink = ObjectTypePlaysRole.GetLink(oppositeRole, preferredIdentifierFor)))
					{
						return;
					}
				}
				if (null != (objectShape = pel as ObjectTypeShape))
				{
					//If there is a fact shape and it is visible then we need to 
					//set ExpandRefMode to true, otherwise set it to false.
					//preferredIdentifierFor.
					FactTypeShape factShape = MultiShapeUtility.FindNearestShapeForElement(null, objectShape, identifyingFactType, rolePlayerLink) as FactTypeShape;
					if (factShape != null &&
						!objectShape.ExpandRefMode)
					{
						objectShape.ExpandRefMode = true;
					}
					else if (!objectShape.ExpandRefMode)
					{
						SizeD oldSize = objectShape.Size;
						objectShape.AutoResize();
						if (oldSize == objectShape.Size)
						{
							objectShape.InvalidateRequired(true);
						}
					}
				}
				else if (null != (objectifiedShape = pel as ObjectifiedFactTypeNameShape))
				{
					FactTypeShape factShape = MultiShapeUtility.FindNearestShapeForElement(null, objectifiedShape, identifyingFactType, rolePlayerLink) as FactTypeShape;
					if (factShape != null &&
						!objectifiedShape.ExpandRefMode)
					{
						objectifiedShape.ExpandRefMode = true;
					}
					else if (!objectifiedShape.ExpandRefMode)
					{
						SizeD oldSize = objectifiedShape.Size;
						objectifiedShape.AutoResize();
						if (oldSize == objectifiedShape.Size)
						{
							((IInvalidateDisplay)objectifiedShape).InvalidateRequired(true);
						}
					}
				}
			}
			// Do the same operation for objectified fact types displayed as an object type.
			FactType objectifiedFactType = preferredIdentifierFor.NestedFactType;
			if (objectifiedFactType != null)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(objectifiedFactType))
				{
					FactTypeShape factTypeShape = pel as FactTypeShape;
					if (factTypeShape != null &&
						factTypeShape.DisplayAsObjectType)
					{
						FactTypeShape identifyingFactTypeShape = MultiShapeUtility.FindNearestShapeForElement(null, factTypeShape, identifyingFactType, rolePlayerLink) as FactTypeShape;
						if (identifyingFactTypeShape != null &&
							!factTypeShape.ExpandRefMode)
						{
							factTypeShape.ExpandRefMode = true;
						}
						else if (!factTypeShape.ExpandRefMode)
						{
							SizeD oldSize = factTypeShape.Size;
							factTypeShape.AutoResize();
							if (oldSize == factTypeShape.Size)
							{
								((IInvalidateDisplay)factTypeShape).InvalidateRequired(true);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
		/// </summary>
		private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
		{
			ProcessPreferredIdentifierAdded(e.ModelElement as EntityTypeHasPreferredIdentifier);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessPreferredIdentifierAdded(EntityTypeHasPreferredIdentifier link)
		{
			ObjectType rolePlayer;
			LinkedElementCollection<Role> roles;
			UniquenessConstraint constraint = link.PreferredIdentifier;
			if (constraint.IsInternal &&
				1 == (roles = constraint.RoleCollection).Count &&
				null != (rolePlayer = roles[0].RolePlayer) &&
				rolePlayer.IsValueType)
			{
				EnsureRefModeExpanded(constraint, link.PreferredIdentifierFor);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
		/// Verify that all preconditions hold for adding a primary
		/// identifier and extend modifiable conditions as needed.
		/// Defers to <see cref="ProcessPreferredIdentifierAdded"/>.
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ProcessPreferredIdentifierAdded(e.ElementLink as EntityTypeHasPreferredIdentifier);
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void PreferredIdentifierLengthenedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			UniquenessConstraint constraint;
			ObjectType preferredIdentifierFor;
			LinkedElementCollection<Role> constraintRoles;
			if (null != (constraint = link.ConstraintRoleSequence as UniquenessConstraint) &&
				!constraint.IsDeleted &&
				null != (preferredIdentifierFor = constraint.PreferredIdentifierFor) &&
				null != preferredIdentifierFor.Objectification &&
				(constraintRoles = constraint.RoleCollection).Count != 1)
			{
				ResizeAssociatedShapes(preferredIdentifierFor);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void PreferredIdentifierShortenedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			UniquenessConstraint constraint;
			ObjectType preferredIdentifierFor;
			LinkedElementCollection<Role> constraintRoles;
			if (null != (constraint = link.ConstraintRoleSequence as UniquenessConstraint) &&
				!constraint.IsDeleted &&
				null != (preferredIdentifierFor = constraint.PreferredIdentifierFor) &&
				null != preferredIdentifierFor.Objectification &&
				(constraintRoles = constraint.RoleCollection).Count == 1)
			{
				ResizeAssociatedShapes(preferredIdentifierFor);
			}
		}

		// Note that we do not need a RolePlayerChangeRule for ValueTypeHasDataType as
		// this will not change whether a given type is used as a refmode or not

		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
		/// An object type can be a preferred identifier. Changing it to a value
		/// type makes it a refmode. Make sure that the ExpandRefMode property is in sync.
		/// </summary>
		private static void DataTypeAddedRule(ElementAddedEventArgs e)
		{
			ValueTypeHasDataType dataTypeLink = e.ModelElement as ValueTypeHasDataType;
			LinkedElementCollection<Role> playedRoles = dataTypeLink.ValueType.PlayedRoleCollection;
			int playedRolesCount = playedRoles.Count;
			for (int i = 0; i < playedRolesCount; ++i)
			{
				LinkedElementCollection<ConstraintRoleSequence> sequences = playedRoles[i].ConstraintRoleSequenceCollection;
				int constraintsCount = sequences.Count;
				for (int j = 0; j < constraintsCount; ++j)
				{
					UniquenessConstraint iuc;
					ObjectType preferredFor;
					if (null != (iuc = sequences[j] as UniquenessConstraint) &&
						iuc.IsInternal &&
						iuc.Modality == ConstraintModality.Alethic &&
						null != (preferredFor = iuc.PreferredIdentifierFor) &&
						iuc.RoleCollection.Count == 1)
					{
						EnsureRefModeExpanded(iuc, preferredFor);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// An object type can be the role player for a single-role preferred identifier. Changing it to a value
		/// type makes it a refmode. Make sure that the ExpandRefMode property is in sync.
		/// </summary>
		private static void DataTypeDeleteRule(ElementDeletedEventArgs e)
		{
			ValueTypeHasDataType dataTypeLink = e.ModelElement as ValueTypeHasDataType;
			ObjectType valueType = dataTypeLink.ValueType;
			if (!valueType.IsDeleted)
			{
				LinkedElementCollection<Role> playedRoles = valueType.PlayedRoleCollection;
				int playedRolesCount = playedRoles.Count;
				for (int i = 0; i < playedRolesCount; ++i)
				{
					LinkedElementCollection<ConstraintRoleSequence> sequences = playedRoles[i].ConstraintRoleSequenceCollection;
					int constraintsCount = sequences.Count;
					for (int j = 0; j < constraintsCount; ++j)
					{
						UniquenessConstraint iuc = sequences[j] as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType preferredFor = iuc.PreferredIdentifierFor;
							if (preferredFor != null)
							{
								ResizeAssociatedShapes(preferredFor);
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
		/// A preferred identifier internal uniqueness constraint can be attached to a role with no
		/// role player. Attaching a role player will match the reference mode pattern, which then needs
		/// to ensure that the ExpandRefMode property is correct. Also, resize shapes if the first subtype role
		/// is added where this is an existing derivation rule.
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			ObjectType rolePlayer = link.RolePlayer;
			if (rolePlayer.IsValueType)
			{
				LinkedElementCollection<ConstraintRoleSequence> sequences = link.PlayedRole.ConstraintRoleSequenceCollection;
				int constraintsCount = sequences.Count;
				for (int i = 0; i < constraintsCount; ++i)
				{
					UniquenessConstraint iuc = sequences[i] as UniquenessConstraint;
					ObjectType preferredFor;
					if (null != (iuc = sequences[i] as UniquenessConstraint) &&
						iuc.IsInternal &&
						iuc.Modality == ConstraintModality.Alethic &&
						null != (preferredFor = iuc.PreferredIdentifierFor) &&
						iuc.RoleCollection.Count == 1)
					{
						EnsureRefModeExpanded(iuc, preferredFor);
					}
				}
			}
			if (link.PlayedRole is SubtypeMetaRole &&
				null != rolePlayer.DerivationRule)
			{
				int subtypeCount = 0;
				ObjectType.WalkSupertypeRelationships(
					rolePlayer,
					delegate(SubtypeFact subtypeFact, ObjectType type, int depth)
					{
						++subtypeCount;
						return (subtypeCount == 1) ? ObjectTypeVisitorResult.SkipChildren : ObjectTypeVisitorResult.Stop;
					});
				if (subtypeCount == 1)
				{
					ResizeAssociatedShapes(rolePlayer);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Deleting a value type that participates in a refmode pattern does not remove the
		/// preferred identifier, so there is no notification to the shape that the refmode is gone.
		/// This forces the opposite ObjectTypeShape to resize in case it lost its refmode.
		/// Also, remove the derivation decorator if the last Subtype is removed.
		/// </summary>
		private static void RolePlayerDeleteRule(ElementDeletedEventArgs e)
		{
			ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ModelElement;
			Role role = link.PlayedRole;
			if (!role.IsDeleted)
			{
				foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
				{
					UniquenessConstraint iuc = sequence as UniquenessConstraint;
					if (iuc != null && iuc.IsInternal)
					{
						ResizeAssociatedShapes(iuc.PreferredIdentifierFor);
					}
				}
			}
			ObjectType rolePlayer = link.RolePlayer;
			if (!rolePlayer.IsDeleted &&
				role is SubtypeMetaRole &&
				rolePlayer.DerivationRule != null &&
				!rolePlayer.IsSubtype)
			{
				ResizeAssociatedShapes(rolePlayer);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
		/// Changing the identifying role player of a FactType that participates in the refmode
		/// pattern needs to appropriately update the ExpandRefMode property and to resize
		/// shapes for the identified object types.
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				ObjectType oldPlayer = (ObjectType)e.OldRolePlayer;
				ObjectType newPlayer = (ObjectType)e.NewRolePlayer;
				if (oldPlayer != newPlayer)
				{
					bool newPlayerIsValueType = newPlayer.IsValueType;
					bool oldPlayerIsValueType = oldPlayer.IsValueType;
					if (newPlayerIsValueType || oldPlayerIsValueType)
					{
						bool switchToValueType = newPlayerIsValueType && !oldPlayerIsValueType;
						LinkedElementCollection<ConstraintRoleSequence> sequences = link.PlayedRole.ConstraintRoleSequenceCollection;
						int constraintsCount = sequences.Count;
						for (int i = 0; i < constraintsCount; ++i)
						{
							UniquenessConstraint iuc;
							ObjectType preferredFor;
							if (null != (iuc = sequences[i] as UniquenessConstraint) &&
								iuc.IsInternal &&
								iuc.Modality == ConstraintModality.Alethic &&
								null != (preferredFor = iuc.PreferredIdentifierFor) &&
								iuc.RoleCollection.Count == 1)
							{
								if (switchToValueType)
								{
									EnsureRefModeExpanded(iuc, preferredFor);
								}
								else
								{
									ResizeAssociatedShapes(preferredFor);
								}
							}
						}
					}
				}
			}
			else
			{
				// This can add an expanded reference mode for an object type associated with the new
				// role player and remove the reference mode for the old player
				if (link.RolePlayer.IsValueType)
				{
					LinkedElementCollection<ConstraintRoleSequence> sequences = link.PlayedRole.ConstraintRoleSequenceCollection;
					int constraintsCount = sequences.Count;
					for (int i = 0; i < constraintsCount; ++i)
					{
						UniquenessConstraint iuc;
						ObjectType preferredFor;
						if (null != (iuc = sequences[i] as UniquenessConstraint) &&
							iuc.IsInternal &&
							iuc.Modality == ConstraintModality.Alethic &&
							null != (preferredFor = iuc.PreferredIdentifierFor) &&
							iuc.RoleCollection.Count == 1)
						{
							EnsureRefModeExpanded(iuc, preferredFor);
						}
					}
					foreach (ConstraintRoleSequence sequence in ((Role)e.OldRolePlayer).ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint iuc = sequence as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ResizeAssociatedShapes(iuc.PreferredIdentifierFor);
						}
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// Resize associated shapes when a derivation rule is added
		/// </summary>
		private static void SubtypeDerivationRuleAddedRule(ElementAddedEventArgs e)
		{
			ResizeAssociatedShapes(((SubtypeHasDerivationRule)e.ModelElement).Subtype);
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// </summary>
		private static void SubtypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectType subtype = ((SubtypeHasDerivationRule)e.ModelElement).Subtype;
			if (!subtype.IsDeleted)
			{
				ResizeAssociatedShapes(subtype);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// </summary>
		private static void SubtypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == SubtypeDerivationRule.DerivationCompletenessDomainPropertyId ||
				propertyId == SubtypeDerivationRule.DerivationStorageDomainPropertyId)
			{
				SubtypeDerivationRule derivationRule = (SubtypeDerivationRule)e.ModelElement;
				ObjectType subtype;
				if (!derivationRule.IsDeleted &&
					null != (subtype = derivationRule.Subtype))
				{
					ResizeAssociatedShapes(subtype);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ConnectionPropertyChangeRule(ElementPropertyChangedEventArgs e)
		{
			ObjectTypeShape objectTypeShape;
			if (e.DomainProperty.Id == DisplayRelatedTypesDomainPropertyId &&
				!(objectTypeShape = (ObjectTypeShape)e.ModelElement).IsDeleted)
			{
				MultiShapeUtility.AttachLinkConfigurationChanged(objectTypeShape);
			}
		}
		#endregion // Shape display update rules
		#region Store Event Handlers
		/// <summary>
		///  Helper function to update the mandatory dot in response to events
		/// </summary>
		private static void UpdateObjectTypeDisplay(ObjectType objectType)
		{
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(objectType))
			{
				ShapeElement shape = pel as ShapeElement;
				if (shape != null)
				{
					shape.Invalidate();
				}
			}
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="ObjectTypeShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			DomainRelationshipInfo relInfo = dataDirectory.FindDomainRelationship(ValueTypeHasDataType.DomainClassId);
			eventManager.AddOrRemoveHandler(relInfo, new EventHandler<ElementAddedEventArgs>(DataTypeAddedEvent), action);
			eventManager.AddOrRemoveHandler(relInfo, new EventHandler<ElementDeletedEventArgs>(DataTypeRemovedEvent), action);
		}
		/// <summary>
		/// Update the shape when a data type is added from the ObjecType.
		/// </summary>
		private static void DataTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
			ObjectType ot = link.ValueType;
			if (!ot.IsDeleted)
			{
				UpdateObjectTypeDisplay(ot);
			}
		}
		/// <summary>
		/// Update the shape when a data type is removed from the ObjecType.
		/// </summary>
		private static void DataTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
			ObjectType ot = link.ValueType;
			if (!ot.IsDeleted)
			{
				UpdateObjectTypeDisplay(ot);
			}
		}
		#endregion // Store Event Handlers
		#region ReferenceModeTextField class
		/// <summary>
		/// Create the text field used for displaying the reference mode
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		/// <returns>ReferenceModeTextField</returns>
		protected virtual ReferenceModeTextField CreateReferenceModeTextField(string fieldName)
		{
			return new ReferenceModeTextField(fieldName);
		}
		/// <summary>
		/// Class to show reference mode
		/// </summary>
		protected class ReferenceModeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public ReferenceModeTextField(string fieldName)
				: base(fieldName)
			{
				DefaultFocusable = true;
			}

			/// <summary>
			/// Gets the minimum <see cref="SizeD"/> of this <see cref="ReferenceModeTextField"/>.
			/// </summary>
			/// <param name="parentShape">
			/// The <see cref="ObjectTypeShape"/> that this <see cref="ReferenceModeTextField"/> is associated with.
			/// </param>
			/// <returns>The minimum <see cref="SizeD"/> of this <see cref="ReferenceModeTextField"/>.</returns>
			public override SizeD GetMinimumSize(ShapeElement parentShape)
			{
				if (GetVisible(parentShape))
				{
					return base.GetMinimumSize(parentShape);
				}
				return SizeD.Empty;
			}

			/// <summary>
			/// Returns whether or not the text field is visible.
			/// </summary>
			public override bool GetVisible(ShapeElement parentShape)
			{
				ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				return (objectType != null && objectTypeShape != null && !objectTypeShape.ExpandRefMode && !objectType.IsValueType);
			}

			/// <summary>
			/// Overrides the display text to add parenthesis
			/// </summary>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null && objectType.HasReferenceMode)
				{
					return string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeReferenceModeFormatString, base.GetDisplayText(parentShape));
				}
				return base.GetDisplayText(parentShape);
			}
		}
		#endregion // ReferenceModeTextField class
		#region ObjectNameTextField class
		/// <summary>
		/// Create the text field used for displaying the object name
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		/// <returns>ObjectNameTextField</returns>
		protected virtual ObjectNameTextField CreateObjectNameTextField(string fieldName)
		{
			return new ObjectNameTextField(fieldName);
		}
		/// <summary>
		/// Class to show a decorated object name
		/// </summary>
		protected class ObjectNameTextField : AutoSizeTextField
		{
			/// <summary>
			/// Create a new ObjectNameTextField
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			public ObjectNameTextField(string fieldName)
				: base(fieldName)
			{
			}
			/// <summary>
			/// Modify the display text for independent object types.
			/// </summary>
			/// <param name="parentShape">The ShapeElement to get the display text for.</param>
			/// <returns>The text to display.</returns>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				string retVal = base.GetDisplayText(parentShape);
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null)
				{
					if (objectType.IsIndependent)
					{
						retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeIndependentFormatString, retVal);
					}
					else if (objectType.DerivationRule != null && objectType.IsSubtype) // Note that subtypes are never independent
					{
						string derivationDecorator = null;
						switch (objectType.DerivationStorageDisplay)
						{
							case DerivationExpressionStorageType.Derived:
								derivationDecorator = ResourceStrings.ObjectTypeShapeDerivationDecoratorFullyDerived;
								break;
							case DerivationExpressionStorageType.DerivedAndStored:
								derivationDecorator = ResourceStrings.ObjectTypeShapeDerivationDecoratorFullyDerivedAndStored;
								break;
							case DerivationExpressionStorageType.PartiallyDerived:
								derivationDecorator = ResourceStrings.ObjectTypeShapeDerivationDecoratorPartiallyDerived;
								break;
							case DerivationExpressionStorageType.PartiallyDerivedAndStored:
								derivationDecorator = ResourceStrings.ObjectTypeShapeDerivationDecoratorPartiallyDerivedAndStored;
								break;
						}
						if (derivationDecorator != null)
						{
							retVal = string.Format(CultureInfo.InvariantCulture, derivationDecorator, retVal);
						}
					}
				}
				return retVal;
			}
		}
		#endregion // ObjectNameTextField class
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener ensures
		/// the correct size for an <see cref="ObjectTypeShape"/>.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ObjectTypeShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset the size of an ObjectTypeShape.
		/// </summary>
		private sealed class ObjectTypeShapeFixupListener : DeserializationFixupListener<ObjectTypeShape>
		{
			/// <summary>
			/// Create a new ObjectTypeShapeFixupListener
			/// </summary>
			public ObjectTypeShapeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Update the shape size if it is independent or if the derivation status is set.
			/// The size for these strings depends on localized values, which may change when
			/// loading the file on a different machine.
			/// Also, the SubtypeFact fixup rule may dynamically create a derivation rule if
			/// a rule is specified on the SubtypeFact instead of on the Subtype itself.
			/// </summary>
			protected sealed override void ProcessElement(ObjectTypeShape element, Store store, INotifyElementAdded notifyAdded)
			{
				ObjectType objectType;
				if (!element.IsDeleted &&
					null != (objectType = element.AssociatedObjectType) &&
					(objectType.IsIndependent ||
					(objectType.DerivationRule != null && objectType.IsSubtype)))
				{
					// Note that technically the size may be wrong if a derivation rule or an independent
					// setting has been removed. However, in this case, the length will be a little too long,
					// which is harmless. This fixes the situation where it is too short.
					element.AutoResize();
				}
			}
		}
		#endregion // Deserialization Fixup
		#region CustomStorage Properties
		private bool GetExpandRefModeValue()
		{
			return GetDisplayFlag(DisplayFlags.ExpandRefMode);
		}
		private void SetExpandRefModeValue(bool value)
		{
			SetDisplayFlag(DisplayFlags.ExpandRefMode, value);
		}
		private RelatedTypesDisplay GetDisplayRelatedTypesValue()
		{
			if (GetDisplayFlag(DisplayFlags.HideSubtypes))
			{
				return GetDisplayFlag(DisplayFlags.HideSupertypes) ? RelatedTypesDisplay.AttachNoTypes : RelatedTypesDisplay.AttachSupertypes;
			}
			return GetDisplayFlag(DisplayFlags.HideSupertypes) ? RelatedTypesDisplay.AttachSubtypes : RelatedTypesDisplay.AttachAllTypes;
		}
		private void SetDisplayRelatedTypesValue(RelatedTypesDisplay value)
		{
			switch (value)
			{
				case RelatedTypesDisplay.AttachAllTypes:
					SetDisplayFlag(DisplayFlags.HideSubtypes | DisplayFlags.HideSupertypes, false);
					break;
				case RelatedTypesDisplay.AttachSubtypes:
					SetDisplayFlag(DisplayFlags.HideSubtypes, false);
					SetDisplayFlag(DisplayFlags.HideSupertypes, true);
					break;
				case RelatedTypesDisplay.AttachSupertypes:
					SetDisplayFlag(DisplayFlags.HideSubtypes, true);
					SetDisplayFlag(DisplayFlags.HideSupertypes, false);
					break;
				case RelatedTypesDisplay.AttachNoTypes:
					SetDisplayFlag(DisplayFlags.HideSubtypes | DisplayFlags.HideSupertypes, true);
					break;
			}
		}
		#endregion // CustomStorage Properties
		#region IConfigureableLinkEndpoint Implementation
		/// <summary>
		/// Implements <see cref="IConfigureableLinkEndpoint.CanAttachLink"/>
		/// </summary>
		protected AttachLinkResult CanAttachLink(ModelElement element, bool toRole)
		{
			if (toRole)
			{
				ObjectTypePlaysRole rolePlayerLink;
				if (element is SubtypeFact)
				{
					if (GetDisplayFlag(DisplayFlags.HideSubtypes))
					{
						return AttachLinkResult.Defer;
					}
				}
				else if (null != (rolePlayerLink = element as ObjectTypePlaysRole))
				{
					FactType refModeFactType;
					if (!ExpandRefMode &&
						null != (refModeFactType = AssociatedObjectType.ReferenceModeFactType) &&
						rolePlayerLink.PlayedRole.FactType == refModeFactType)
					{
						return AttachLinkResult.Defer;
					}
				}
			}
			else if (GetDisplayFlag(DisplayFlags.HideSupertypes) &&
				element is SubtypeFact)
			{
				return AttachLinkResult.Defer;
			}
			return AttachLinkResult.Attach;
		}
		AttachLinkResult IConfigureableLinkEndpoint.CanAttachLink(ModelElement element, bool toRole)
		{
			return CanAttachLink(element, toRole);
		}
		/// <summary>
		/// Implements <see cref="IConfigureableLinkEndpoint.FixupUnattachedLinkElements"/>
		/// </summary>
		protected void FixupUnattachedLinkElements(Diagram diagram)
		{
			ObjectType objectType;
			ORMDiagram ormDiagram;
			if (null != (objectType = AssociatedObjectType) &&
				null != (ormDiagram = diagram as ORMDiagram))
			{
				foreach (Role role in objectType.PlayedRoleCollection)
				{
					FactType subtypeFact = null;
					if ((role is SubtypeMetaRole ||	role is SupertypeMetaRole) &&
						null != (subtypeFact = role.FactType) &&
						null == ormDiagram.FindShapeForElement<SubtypeLink>(subtypeFact))
					{
						ormDiagram.FixUpLocalDiagram(subtypeFact);
					}
				}
			}
		}
		void IConfigureableLinkEndpoint.FixupUnattachedLinkElements(Diagram diagram)
		{
			FixupUnattachedLinkElements(diagram);
		}
		#endregion // IConfigureableLinkEndpoint Implementation
	}
}
