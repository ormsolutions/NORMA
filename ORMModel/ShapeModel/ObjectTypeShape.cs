#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Core.Shell;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	public partial class ObjectTypeShape : IModelErrorActivation, IDynamicColorGeometryHost, IConfigureableLinkEndpoint
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField;
		private static AutoSizeTextField myReferenceModeShapeField;
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
				return ORMBaseShape.ElementHasMultiplePresentations(this);
			}
		}
		/// <summary>
		/// Support automatic appearance updating when multiple presentations are present.
		/// </summary>
		public override bool DisplaysMultiplePresentations
		{
			get
			{
				return true;
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
			if ((penId == DashedShapeOutlinePen || penId == DiagramPens.ShapeOutline) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>>()))
			{
				ObjectType element = (ObjectType)ModelElement;
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
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>[] providers;
			bool isBackgroundBrush;
			if (((isBackgroundBrush = brushId == DiagramBrushes.DiagramBackground) ||
				brushId == DiagramBrushes.ShapeTitleText) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, ObjectTypeShape, ObjectType>>()))
			{
				ObjectType element = (ObjectType)ModelElement;
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
			AutoSizeTextField field = CreateObjectNameTextField("ObjectNameTextField");
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
			field.AssociateValueWith(Store, ORMNamedElement.NameDomainPropertyId);

			// Initialize reference mode field
			AutoSizeTextField referenceModeField = CreateReferenceModeTextField("RefModeTextField");
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
			//RoleValueConstraints added should be for a role opposite of
			//this object's role player. The test below allows a RoleValueConstraint
			//to be added on the opposite role even if that opposite role is played
			//by the same ObjectType as the RoleValueConstraint's role (i.e. allows
			//for ring constraints).
			bool isRoleValueRangeDefn = false;
			RoleValueConstraint roleDefn;
			if (null != (roleDefn = element as RoleValueConstraint))
			{
				Role roleInDefn = roleDefn.Role;
				FactType factType = roleInDefn.FactType;
				foreach (RoleBase rBase in factType.RoleCollection)
				{
					Role r = rBase.Role;
					if (roleInDefn != r)
					{
						if (r.RolePlayer == AssociatedObjectType)
						{
							isRoleValueRangeDefn = true;
							break;
						}
					}
				}
			}
			Debug.Assert(
				(element is ValueTypeValueConstraint && ((ValueTypeValueConstraint)element).ValueType == AssociatedObjectType) ||
				isRoleValueRangeDefn
			);
#endif // DEBUG
			return true;
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
			// Show for all shapes and fields in item
			retVal = AssociatedObjectType.DefinitionText;
			if (retVal.Length == 0)
			{
				retVal = null;
			}
			return retVal;
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
			Debug.Assert(childShape is ValueConstraintShape);
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
					DomainTypeDescriptor.CreatePropertyDescriptor(valueType, ObjectType.DataTypeDisplayDomainPropertyId),
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
				ORMDesignerPackage.SamplePopulationEditorWindow.AutoCorrectMandatoryError(populationMandatory);
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
		#region ObjectTypeShapeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Keep relative child elements a fixed distance away from the object type shape
		/// when the shape changes.
		/// </summary>
		private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			MaintainRelativeShapeOffsetsForBoundsChange(e);
		}
		#endregion // ObjectTypeShapeChangeRule
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
			//Get the object that represents the item with the preferred identifier. 
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(preferredIdentifierFor))
			{
				ObjectTypeShape objectShape;
				ObjectifiedFactTypeNameShape objectifiedShape;
				if (null != (objectShape = pel as ObjectTypeShape))
				{
					//If there is a fact shape and it is visible then we need to 
					//set ExpandRefMode to true, otherwise set it to false.
					FactTypeShape factShape = (objectShape.Diagram as ORMDiagram).FindShapeForElement<FactTypeShape>(constraint.FactTypeCollection[0]);
					bool newValue = factShape != null && factShape.IsVisible;
					if (objectShape.ExpandRefMode != newValue)
					{
						objectShape.ExpandRefMode = newValue;
					}
					else
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
					FactTypeShape factShape = (objectifiedShape.Diagram as ORMDiagram).FindShapeForElement<FactTypeShape>(constraint.FactTypeCollection[0]);
					bool newValue = factShape != null && factShape.IsVisible;
					if (objectifiedShape.ExpandRefMode != newValue)
					{
						objectifiedShape.ExpandRefMode = newValue;
					}
					else
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
					UniquenessConstraint iuc = sequences[j] as UniquenessConstraint;
					if (iuc != null && iuc.IsInternal)
					{
						ObjectType preferredFor = iuc.PreferredIdentifierFor;
						if (preferredFor != null)
						{
							EnsureRefModeExpanded(iuc, preferredFor);
						}
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
					if (iuc != null && iuc.IsInternal)
					{
						ObjectType preferredFor = iuc.PreferredIdentifierFor;
						if (preferredFor != null)
						{
							EnsureRefModeExpanded(iuc, preferredFor);
						}
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
				bool newPlayerIsValueType = link.RolePlayer.IsValueType;
				bool oldPlayerIsValueType = ((ObjectType)e.OldRolePlayer).IsValueType;
				if ((newPlayerIsValueType || oldPlayerIsValueType) &&
					(newPlayerIsValueType != oldPlayerIsValueType))
				{
					LinkedElementCollection<ConstraintRoleSequence> sequences = link.PlayedRole.ConstraintRoleSequenceCollection;
					int constraintsCount = sequences.Count;
					for (int i = 0; i < constraintsCount; ++i)
					{
						UniquenessConstraint iuc = sequences[i] as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType preferredFor = iuc.PreferredIdentifierFor;
							if (preferredFor != null)
							{
								if (newPlayerIsValueType)
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
						UniquenessConstraint iuc = sequences[i] as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType preferredFor = iuc.PreferredIdentifierFor;
							if (preferredFor != null)
							{
								EnsureRefModeExpanded(iuc, preferredFor);
							}
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
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationExpression), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// Resize associated shapes when a derivation rule is added
		/// </summary>
		private static void SubtypeDerivationAddedRule(ElementAddedEventArgs e)
		{
			ResizeAssociatedShapes(((SubtypeHasDerivationExpression)e.ModelElement).Subtype);
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationExpression), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
		/// </summary>
		private static void SubtypeDerivationDeletedRule(ElementDeletedEventArgs e)
		{
			ResizeAssociatedShapes(((SubtypeHasDerivationExpression)e.ModelElement).Subtype);
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
			}			/// <summary>
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
						retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeDerivedSubtypeFormatString, retVal);
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
				if (GetDisplayFlag(DisplayFlags.HideSubtypes) && element is SubtypeFact)
				{
					return AttachLinkResult.Defer;
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
