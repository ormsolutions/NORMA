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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
using System.Runtime.InteropServices;
using System.Collections;
namespace Neumont.Tools.ORM.ShapeModel
{
	public partial class ObjectTypeShape : IModelErrorActivation
	{
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError for DataTypeNotSpecifiedError
		/// </summary>
		protected void ActivateModelError(ModelError error)
		{
			if (error is DataTypeNotSpecifiedError)
			{
				IServiceProvider provider;
				IVsUIShell shell;
				if (null != (provider = (Store as IORMToolServices).ServiceProvider) &&
					null != (shell = (IVsUIShell)provider.GetService(typeof(IVsUIShell))))
				{
					Guid windowGuid = new Guid(ToolWindowGuids.PropertyBrowser);
					IVsWindowFrame frame;
					ErrorHandler.ThrowOnFailure(shell.FindToolWindow((uint)(__VSFINDTOOLWIN.FTW_fForceCreate), ref windowGuid, out frame));
					ErrorHandler.ThrowOnFailure(frame.Show());
					// UNDONE: Keep going, select the DataType property, and open its dropdown. I've pinged MS on this one, I can't
					// see any good way to get at the PropertyGrid reference, or to open the dropdown when I find it.
				}
			}
		}
		void IModelErrorActivation.ActivateModelError(ModelError error)
		{
			ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
	}
	public partial class ObjectTypeShape
	{
		#region Member Variables
		private static AutoSizeTextField myTextShapeField;
		private static AutoSizeTextField myReferenceModeShapeField;
		private const double HorizontalMargin = 0.12;
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
			settings.Width = 1.2f / 72f; // 1.2 point
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, settings);
			settings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(DashedShapeOutlinePen, DiagramPens.ShapeOutline, settings);
		}
		/// <summary>
		/// Set the default size for this object. This value is basically
		/// ignored because the size is ultimately based on the contained
		/// text, but it needs to be set.
		/// </summary>
		public override SizeD DefaultSize
		{
			get
			{
				return new SizeD(.7, .35);
			}
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
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize field
			AutoSizeTextField field = CreateObjectNameTextField();
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
			field.AssociateValueWith(Store, ObjectTypeShape.ShapeNameMetaAttributeGuid, NamedElement.NameMetaAttributeGuid);

			// Initialize reference mode field
			AutoSizeTextField referenceModeField = CreateReferenceModeTextField();
			referenceModeField.DrawBorder = false;
			referenceModeField.FillBackground = false;
			referenceModeField.DefaultTextBrushId = DiagramBrushes.ShapeTitleText;
			referenceModeField.DefaultPenId = DiagramPens.ShapeOutline;
			referenceModeField.DefaultFontId = DiagramFonts.ShapeTitle;
			referenceModeField.DefaultFocusable = true;
			referenceModeField.DefaultText = "";
			referenceModeField.DefaultStringFormat = fieldFormat;
			referenceModeField.AssociateValueWith(Store, ObjectTypeShape.ReferenceModeNameMetaAttributeGuid, ObjectType.ReferenceModeDisplayMetaAttributeGuid);

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
				foreach (Role r in factType.RoleCollection)
				{
					if (!object.ReferenceEquals(roleInDefn, r))
					{
						if (object.ReferenceEquals(r.RolePlayer, AssociatedObjectType))
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
		#region Shape display update rules
		[RuleOn(typeof(ObjectType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class ShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				//if primaryidentifyer for a Entity type is 
				//removed and ref mode is not expanded...AutoResize() the entity type
				Guid attributeGuid = e.MetaAttribute.Id;
				bool resizeShapes = false;
				if (attributeGuid == NamedElement.NameMetaAttributeGuid)
				{
					// Figure out if we need to resize related object shapes. This happens
					// when we've renamed a value type that is bound to the reference scheme
					// of an entity type.
					ObjectType changeType = e.ModelElement as ObjectType;
		
					if (changeType.IsValueType)
					{
						RoleMoveableCollection playedRoles = changeType.PlayedRoleCollection;
						int roleCount = playedRoles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							Role currentRole = playedRoles[i];
							ConstraintRoleSequenceMoveableCollection roleConstraints = currentRole.ConstraintRoleSequenceCollection;
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
										PresentationElementMoveableCollection pels = identifierFor.PresentationRolePlayers;
										int pelCount = pels.Count;
										ObjectTypeShape ots;
										for (int k = 0; k < pelCount; ++k)
										{
											if (null != (ots = pels[k] as ObjectTypeShape))
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
				else if (attributeGuid == ObjectType.IsIndependentMetaAttributeGuid)
				{
					resizeShapes = true;
				}
				if (resizeShapes)
				{
					foreach (PresentationElement pel in e.ModelElement.PresentationRolePlayers)
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
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class PreferredIdentifierRemovedRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
				ObjectType entityType = link.PreferredIdentifierFor;
				if (!entityType.IsRemoved)
				{
					PresentationElementMoveableCollection pels = entityType.PresentationRolePlayers;
					int pelCount = pels.Count;
					ObjectTypeShape ots;
					for (int i = 0; i < pelCount; ++i)
					{
						if (null != (ots = pels[i] as ObjectTypeShape))
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
		private static void EnsureRefModeExpanded(InternalUniquenessConstraint constraint, ObjectType preferredIdentifierFor)
		{
			Debug.Assert(constraint != null); // Check before call
			Debug.Assert(preferredIdentifierFor != null); // Check before call
			//Get the object that represents the item with the preferred identifier. 
			foreach (PresentationElement pel in preferredIdentifierFor.PresentationRolePlayers)
			{
				ObjectTypeShape objectShape = pel as ObjectTypeShape;
				if (objectShape != null)
				{
					//If there is a fact shape and it is visible then we need to 
					//set ExpandRefMode to true, otherwise set it to false.
					FactTypeShape factShape = (objectShape.Diagram as ORMDiagram).FindShapeForElement<FactTypeShape>(constraint.FactType);
					objectShape.ExpandRefMode = factShape != null && factShape.IsVisible;
				}
			}
		}
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
		private class PreferredIdentifierAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectType rolePlayer;
				RoleMoveableCollection roles;
				EntityTypeHasPreferredIdentifier link;
				InternalUniquenessConstraint constraint;
				if (null != (link = e.ModelElement as EntityTypeHasPreferredIdentifier) &&
					null != (constraint = link.PreferredIdentifier as InternalUniquenessConstraint) &&
					0 != (roles = constraint.RoleCollection).Count &&
					null != (rolePlayer = roles[0].RolePlayer) &&
					rolePlayer.IsValueType)
				{
					EnsureRefModeExpanded(constraint, link.PreferredIdentifierFor);
				}
			} //method
		} //class
		/// <summary>
		/// An object type can be a preferred identifier. Changing it to a value
		/// type makes it a refmode. Make sure that the ExpandRefMode property is in sync.
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))]
		private class DataTypeAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				EntityTypeHasPreferredIdentifier link;
				if (null == (link = element as EntityTypeHasPreferredIdentifier))
				{
					ValueTypeHasDataType dataTypeLink = element as ValueTypeHasDataType;
					RoleMoveableCollection playedRoles = dataTypeLink.ValueTypeCollection.PlayedRoleCollection;
					int playedRolesCount = playedRoles.Count;
					for (int i = 0; i < playedRolesCount; ++i)
					{
						ConstraintRoleSequenceMoveableCollection sequences = playedRoles[i].ConstraintRoleSequenceCollection;
						int constraintsCount = sequences.Count;
						for (int j = 0; j < constraintsCount; ++j)
						{
							InternalUniquenessConstraint iuc = sequences[j] as InternalUniquenessConstraint;
							if (iuc != null)
							{
								ObjectType preferredFor = (iuc as IConstraint).PreferredIdentifierFor;
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
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class RolePlayerAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				if (link.RolePlayer.IsValueType)
				{
					ConstraintRoleSequenceMoveableCollection sequences = link.PlayedRoleCollection.ConstraintRoleSequenceCollection;
					int constraintsCount = sequences.Count;
					for (int i = 0; i < constraintsCount; ++i)
					{
						InternalUniquenessConstraint iuc = sequences[i] as InternalUniquenessConstraint;
						if (iuc != null)
						{
							ObjectType preferredFor = (iuc as IConstraint).PreferredIdentifierFor;
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
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime=TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class RolePlayerRemovedRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				if (!role.IsRemoved)
				{
					foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
					{
						InternalUniquenessConstraint iuc = sequence as InternalUniquenessConstraint;
						if (iuc != null)
						{
							ObjectType objectType = (iuc as IConstraint).PreferredIdentifierFor;
							if (objectType != null)
							{
								foreach (PresentationElement pel in objectType.PresentationRolePlayers)
								{
									ObjectTypeShape objectShape = pel as ObjectTypeShape;
									if (objectShape != null)
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
			foreach (PresentationElement pel in objectType.PresentationRolePlayers)
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
		public static void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaRelationshipInfo relInfo = dataDirectory.FindMetaRelationship(ValueTypeHasDataType.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(relInfo, new ElementAddedEventHandler(DataTypeAddedEvent));
			eventDirectory.ElementRemoved.Add(relInfo, new ElementRemovedEventHandler(DataTypeRemovedEvent));
		}
		/// <summary>
		/// Detach event handlers from the store
		/// </summary>
		public static void DetachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaRelationshipInfo relInfo = dataDirectory.FindMetaRelationship(ValueTypeHasDataType.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(relInfo, new ElementAddedEventHandler(DataTypeAddedEvent));
			eventDirectory.ElementRemoved.Remove(relInfo, new ElementRemovedEventHandler(DataTypeRemovedEvent));
		}
		/// <summary>
		/// Update the shape when a data type is added from the ObjecType.
		/// </summary>
		private static void DataTypeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
			ObjectType ot = link.ValueTypeCollection;
			if (!ot.IsRemoved)
			{
				UpdateObjectTypeDisplay(ot);
			}
		}
		/// <summary>
		/// Update the shape when a data type is removed from the ObjecType.
		/// </summary>
		private static void DataTypeRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
			ObjectType ot = link.ValueTypeCollection;
			if (!ot.IsRemoved)
			{
				UpdateObjectTypeDisplay(ot);
			}
		}
		#endregion // Store Event Handlers
		#region ReferenceModeTextField class
		/// <summary>
		/// Create the text field used for displaying the reference mode
		/// </summary>
		/// <returns>ReferenceModeTextField</returns>
		protected virtual ReferenceModeTextField CreateReferenceModeTextField()
		{
			return new ReferenceModeTextField();
		}
		/// <summary>
		/// Class to show reference mode
		/// </summary>
		protected class ReferenceModeTextField : AutoSizeTextField
		{
			/// <summary>
			/// Default constructor
			/// </summary>
			public ReferenceModeTextField()
			{
				DefaultFocusable = true;
				DefaultSelectable = true;
			}

			/// <summary>
			/// Get the minimum width of the shape field for the current text.
			/// </summary>
			/// <param name="parentShape">ShapeElement</param>
			/// <returns>Width of current text</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null)
				{
					if (!objectType.IsValueType && !objectTypeShape.ExpandRefMode)
					{

						return base.GetMinimumWidth(parentShape);
					}
				}
				return 0;
			}
			/// <summary>
			/// Get the minimum height of the shape field for the current text.
			/// </summary>
			/// <param name="parentShape">ShapeElement</param>
			/// <returns>Width of current text</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null)
				{
					if (!objectType.IsValueType && !objectTypeShape.ExpandRefMode)
					{

						return base.GetMinimumHeight(parentShape);
					}
				}
				return 0;
			}

			/// <summary>
			/// Returns whether or not the text field is visible
			/// </summary>
			/// <param name="parentShape"></param>
			/// <returns></returns>
			public override bool GetVisible(ShapeElement parentShape)
			{
				ObjectTypeShape objectTypeShape = parentShape as ObjectTypeShape;
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null && !objectTypeShape.ExpandRefMode)
				{
					if (!objectType.IsValueType)
					{
						return true;
					}
				}
				return false;
			}

			/// <summary>
			/// Overrides the display text to add parenthesis
			/// </summary>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				if (objectType != null)
				{
					if (objectType.HasReferenceMode)
					{
						return string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeReferenceModeFormatString, base.GetDisplayText(parentShape));
					}
				}
				return base.GetDisplayText(parentShape);
			}
		}
		#endregion // ReferenceModeTextField class
		#region ObjectNameTextField class
		/// <summary>
		/// Create the text field used for displaying the object name
		/// </summary>
		/// <returns>ObjectNameTextField</returns>
		protected virtual ObjectNameTextField CreateObjectNameTextField()
		{
			return new ObjectNameTextField();
		}
		/// <summary>
		/// Class to show a decorated object name
		/// </summary>
		protected class ObjectNameTextField : AutoSizeTextField
		{
			/// <summary>
			/// Modify the display text for independent object types.
			/// </summary>
			/// <param name="parentShape">The ShapeElement to get the display text for.</param>
			/// <returns>The text to display.</returns>
			public override string GetDisplayText(ShapeElement parentShape)
			{
				string retVal = base.GetDisplayText(parentShape);
				ObjectType objectType;
				if (null != (objectType = parentShape.ModelElement as ObjectType) &&
					objectType.IsIndependent)
				{
					retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ObjectTypeShapeIndependentFormatString, retVal);
				}
				return retVal;
			}
		}
		#endregion // ObjectNameTextField class
	}
}
