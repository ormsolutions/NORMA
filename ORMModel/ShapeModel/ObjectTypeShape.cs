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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ObjectTypeShape : IModelErrorActivation
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField;
		private static AutoSizeTextField myReferenceModeShapeField;
		private const double HorizontalMargin = 0.080;
		private const double VerticalMargin = 0.060;
		private static readonly StyleSetResourceId DashedShapeOutlinePen = new StyleSetResourceId("Neumont", "DashedShapeOutlinePen");
		#endregion // Member Variables
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
			// Note that the reference mode field is associated with the ReferenceModeString
			// property, not ReferenceModeDisplay. The field will only activate for editing
			// if it is a string property.
			referenceModeField.AssociateValueWith(Store, ObjectType.ReferenceModeStringDomainPropertyId);

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
		[RuleOn(typeof(ObjectType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)] // ChangeRule
		private sealed partial class ShapeChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				//if primaryidentifyer for a Entity type is 
				//removed and ref mode is not expanded...AutoResize() the entity type
				Guid attributeGuid = e.DomainProperty.Id;
				bool resizeShapes = false;
				if (attributeGuid == ORMNamedElement.NameDomainPropertyId)
				{
					// Figure out if we need to resize related object shapes. This happens
					// when we've renamed a value type that is bound to the reference scheme
					// of an entity type.
					ObjectType changeType = e.ModelElement as ObjectType;
		
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
									ObjectType identifierFor = associatedConstraint.PreferredIdentifierFor;
									if (identifierFor != null)
									{
										LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(identifierFor);
										int pelCount = pels.Count;
										ORMBaseShape ots;
										for (int k = 0; k < pelCount; ++k)
										{
											if (null != (ots = pels[k] as ORMBaseShape))
											{
												ots.AutoResize();
											}
										}
									}
								}
							}
						}
					}
					resizeShapes = true;
				}
				else if (attributeGuid == ObjectType.IsIndependentDomainPropertyId)
				{
					resizeShapes = true;
				}
				if (resizeShapes)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(e.ModelElement))
					{
						ORMBaseShape shape = pel as ORMBaseShape;
						if (shape != null)
						{
							shape.AutoResize();
						}
					}
				}
			}
		}
		#region ObjectTypeShapeChangeRule class
		/// <summary>
		/// Keep relative child elements a fixed distance away from the object type shape
		/// when the shape changes.
		/// </summary>
		[RuleOn(typeof(ObjectTypeShape), FireTime = TimeToFire.LocalCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)] // ChangeRule
		private sealed partial class ObjectTypeShapeChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				MaintainRelativeShapeOffsetsForBoundsChange(e);
			}
		}
		#endregion // ObjectTypeShapeChangeRule class
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)] // DeleteRule
		private sealed partial class PreferredIdentifierDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType entityType = link.PreferredIdentifierFor;
				if (!entityType.IsDeleted)
				{
					LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(entityType);
					int pelCount = pels.Count;
					ORMBaseShape ots; // Picks up ObjectTypeShape, ObjectifiedFactTypeNameShape
					for (int i = 0; i < pelCount; ++i)
					{
						PresentationElement pel = pels[i];
						if (null != (ots = pel as ORMBaseShape))
						{
							ots.AutoResize();
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
					objectShape.ExpandRefMode = factShape != null && factShape.IsVisible;
				}
				else if (null != (objectifiedShape = pel as ObjectifiedFactTypeNameShape))
				{
					FactTypeShape factShape = (objectifiedShape.Diagram as ORMDiagram).FindShapeForElement<FactTypeShape>(constraint.FactTypeCollection[0]);
					objectifiedShape.ExpandRefMode = factShape != null && factShape.IsVisible;
				}
			}
		}
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))] // AddRule
		private sealed partial class PreferredIdentifierAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectType rolePlayer;
				LinkedElementCollection<Role> roles;
				EntityTypeHasPreferredIdentifier link;
				UniquenessConstraint constraint;
				if (null != (link = e.ModelElement as EntityTypeHasPreferredIdentifier) &&
					null != (constraint = link.PreferredIdentifier) &&
					constraint.IsInternal &&
					0 != (roles = constraint.RoleCollection).Count &&
					null != (rolePlayer = roles[0].RolePlayer) &&
					rolePlayer.IsValueType)
				{
					EnsureRefModeExpanded(constraint, link.PreferredIdentifierFor);
				}
			} //method
		} //class

		// Note that we do not need a RolePlayerChangeRule for ValueTypeHasDataType as
		// this will not change whether a given type is used as a refmode or not

		/// <summary>
		/// An object type can be a preferred identifier. Changing it to a value
		/// type makes it a refmode. Make sure that the ExpandRefMode property is in sync.
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))] // AddRule
		private sealed partial class DataTypeAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				EntityTypeHasPreferredIdentifier link;
				if (null == (link = element as EntityTypeHasPreferredIdentifier))
				{
					ValueTypeHasDataType dataTypeLink = element as ValueTypeHasDataType;
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
			}
		}
		/// <summary>
		/// A preferred identifier internal uniqueness constraint can be attached to a role with no
		/// role player. Attaching a role player will match the reference mode pattern, which then needs
		/// to ensure that the ExpandRefMode property is correct.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))] // AddRule
		private sealed partial class RolePlayerAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
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
				}
			}
		}
		/// <summary>
		/// Deleting a value type that participates in a refmode pattern does not remove the
		/// preferred identifier, so there is no notification to the shape that the refmode is gone.
		/// This forces the opposite ObjectTypeShape to resize in case it lost its refmode.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime=TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)] // DeleteRule
		private sealed partial class RolePlayerDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				if (!role.IsDeleted)
				{
					foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint iuc = sequence as UniquenessConstraint;
						if (iuc != null && iuc.IsInternal)
						{
							ObjectType objectType = iuc.PreferredIdentifierFor;
							if (objectType != null)
							{
								foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(objectType))
								{
									ORMBaseShape objectShape; // Picks up ObjectTypeShape, ObjectifiedFactTypeNameShape
									if (null != (objectShape = pel as ORMBaseShape))
									{
										objectShape.AutoResize();
									}
								}
							}
						}
					}
				}
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
		/// Attach event handlers to the store
		/// </summary>
		public static new void AttachEventHandlers(Store store)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			DomainRelationshipInfo relInfo = dataDirectory.FindDomainRelationship(ValueTypeHasDataType.DomainClassId);
			eventDirectory.ElementAdded.Add(relInfo, new EventHandler<ElementAddedEventArgs>(DataTypeAddedEvent));
			eventDirectory.ElementDeleted.Add(relInfo, new EventHandler<ElementDeletedEventArgs>(DataTypeRemovedEvent));
		}
		/// <summary>
		/// Detach event handlers from the store
		/// </summary>
		public static new void DetachEventHandlers(Store store)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			DomainRelationshipInfo relInfo = dataDirectory.FindDomainRelationship(ValueTypeHasDataType.DomainClassId);
			eventDirectory.ElementAdded.Remove(relInfo, new EventHandler<ElementAddedEventArgs>(DataTypeAddedEvent));
			eventDirectory.ElementDeleted.Remove(relInfo, new EventHandler<ElementDeletedEventArgs>(DataTypeRemovedEvent));
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
				if (objectType != null && objectType.IsIndependent)
				{
					retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeIndependentFormatString, retVal);
				}
				return retVal;
			}
		}
		#endregion // ObjectNameTextField class
	}
}
