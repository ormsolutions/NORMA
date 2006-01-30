//#define IMPLIEDJOINPATH
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ShapeModel
{
	#region (Temporary) CompositeLinkDecorator test class
	/// <summary>
	/// UNDONE: This is just a test to see if we can draw a filled circle contained inside
	/// a nested circle
	/// </summary>
	public class CircleInCircleLinkDecorator : CompositeLinkDecorator
	{
		/// <summary>
		/// Singleton instance of this decorator
		/// </summary>
		public static readonly LinkDecorator Decorator = new CircleInCircleLinkDecorator();
		private CircleInCircleLinkDecorator()
		{
		}
		/// <summary>
		/// Provides multiple paths for the composite decorator that are drawn in
		/// to produce the decorator.
		/// </summary>
		/// <value></value>
		protected override System.Collections.Generic.IList<LinkDecorator> DecoratorCollection
		{
			get
			{
				return new LinkDecorator[] { new InnerCircle(), new OuterCircle() };
			}
		}
		private class OuterCircle : LinkDecorator
		{
			public OuterCircle()
			{
				FillDecorator = true;
			}
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				path.AddArc(RectangleD.ToRectangleF(bounds), 0, 360);
				return path;
			}
			public override StyleSetResourceId BrushId
			{
				get
				{
					return DiagramBrushes.DiagramBackground;
				}
			}
		}
		private class InnerCircle : LinkDecorator
		{
			public InnerCircle()
			{
				FillDecorator = true;
			}
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				float inflateBy = -(float)(bounds.Width / 4);
				RectangleF boundsF = RectangleD.ToRectangleF(bounds);
				boundsF.Inflate(inflateBy, inflateBy);
				//path.AddRectangle(boundsF);
				path.AddArc(boundsF, 0, 360);
				return path;
			}
		}
	}
	#endregion // (Temporary) CompositeLinkDecorator test
	public partial class RolePlayerLink
	{
#if IMPLIEDJOINPATH
		#region ImpliedFactJoinPathDecorator class
		/// <summary>
		/// A decorator used to display the join path role boxes for
		/// implied facts when we're in join path editing mode. We only
		/// decorate one end of a role player link, so we are able to use
		/// the opposite decorator for nefarious purposes such as this.
		/// </summary>
		protected class ImpliedFactJoinPathDecorator : LinkDecorator, ILinkDecoratorSettings
		{
			// UNDONE: Constants stolen from FactTypeShape, do something about it
			private const double RoleBoxHeight = 0.11;
			private const double RoleBoxWidth = 0.16;
			private RolePlayerLink myLinkShape;
			/// <summary>
			/// Create an ImpliedFactJoinPathDecorator
			/// </summary>
			/// <param name="linkShape">The associated link shape</param>
			public ImpliedFactJoinPathDecorator(RolePlayerLink linkShape)
			{
				myLinkShape = linkShape;
			}
			/// <summary>
			/// Return a circle slightly smaller than the standard decorator
			/// as the path
			/// </summary>
			/// <param name="bounds">A bounding rectangle for the decorator</param>
			/// <returns>A circle path</returns>
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				bounds.Height /= 2;
				if (IsFlipped)
				{
					bounds.Y += bounds.Height;
				}
				path.AddRectangle(RectangleD.ToRectangleF(bounds));
				return path;
			}
			private bool IsFlipped
			{
				get
				{
					PointD fromPoint = myLinkShape.FromEndPoint;
					PointD toPoint = myLinkShape.ToEndPoint;
					return toPoint.X < fromPoint.X;
				}
			}
			#region ILinkDecoratorSettings Implementation
			/// <summary>
			/// Implements ILinkDecoratorSettings.DecoratorSize.
			/// </summary>
			protected static SizeD DecoratorSize
			{
				get
				{
					return new SizeD(2 * RoleBoxWidth, 2 * RoleBoxHeight);
				}
			}
			SizeD ILinkDecoratorSettings.DecoratorSize
			{
				get
				{
					return DecoratorSize;
				}
			}
			/// <summary>
			/// Implements ILinkDecoratorSettings.OffsetBy
			/// </summary>
			protected double OffsetBy
			{
				get
				{
					// Note: FromShape == FactTypeShape, so the endpoint
					// is inside the shape. The ToShape will always have the
					// endpoint attaching at the shape, so we do not have to
					// recalculate our attach point
					PointD fromPoint = myLinkShape.FromEndPoint;
					PointD toPoint = myLinkShape.ToEndPoint;
					double xDif = toPoint.X - fromPoint.X;
					double yDif = toPoint.Y - fromPoint.Y;
					return -Math.Sqrt(xDif * xDif + yDif * yDif) / 2 + RoleBoxWidth ;
				}
			}
			double ILinkDecoratorSettings.OffsetBy
			{
				get
				{
					return OffsetBy;
				}
			}
			#endregion // ILinkDecoratorSettings Implementation
		}
		#endregion // ImpliedFactJoinPathDecorator class
#endif // IMPLIEDJOINPATH
		#region MandatoryDotDecorator class
		/// <summary>
		/// The link decorator used to draw the mandatory
		/// constraint dot on a link.
		/// </summary>
		protected class MandatoryDotDecorator : LinkDecorator, ILinkDecoratorSettings
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static readonly LinkDecorator Decorator = new MandatoryDotDecorator();
			private MandatoryDotDecorator()
			{
				FillDecorator = true;
			}
			/// <summary>
			/// Return a circle slightly smaller than the standard decorator
			/// as the path
			/// </summary>
			/// <param name="bounds">A bounding rectangle for the decorator</param>
			/// <returns>A circle path</returns>
			protected override GraphicsPath GetPath(RectangleD bounds)
			{
				GraphicsPath path = new GraphicsPath();
				path.AddArc(RectangleD.ToRectangleF(bounds), 0, 360);
				return path;
			}

			#region ILinkDecoratorSettings Implementation
			/// <summary>
			/// Implements ILinkDecoratorSettings.DecoratorSize.
			/// </summary>
			protected static SizeD DecoratorSize
			{
				get
				{
					return new SizeD(.075d, .075d);
				}
			}
			SizeD ILinkDecoratorSettings.DecoratorSize
			{
				get
				{
					return DecoratorSize;
				}
			}
			/// <summary>
			/// Implements ILinkDecoratorSettings.OffsetBy
			/// </summary>
			protected static double OffsetBy
			{
				get
				{
					return .0375d;
				}
			}
			double ILinkDecoratorSettings.OffsetBy
			{
				get
				{
					return OffsetBy;
				}
			}
			#endregion // ILinkDecoratorSettings Implementation
		}
		#endregion // MandatoryDotDecorator class
		#region Customize appearance
		/// <summary>
		/// Draw the mandatory dot on the role box end, depending
		/// on the options settings
		/// </summary>
		public override LinkDecorator DecoratorFrom
		{
			get
			{
				if (OptionsPage.CurrentMandatoryDotPlacement != MandatoryDotPlacement.ObjectShapeEnd &&
					DrawMandatoryDot)
				{
					return MandatoryDotDecorator.Decorator;
				}
#if IMPLIEDJOINPATH
				else if (OptionsPage.CurrentMandatoryDotPlacement == MandatoryDotPlacement.ObjectShapeEnd)
				{
					return new ImpliedFactJoinPathDecorator(this);
				}
#endif // IMPLIEDJOINPATH
				return base.DecoratorFrom;
			}
			set
			{
			}
		}
		/// <summary>
		/// Draw the mandatory dot on the object type end, depending
		/// on the options settings
		/// </summary>
		public override LinkDecorator DecoratorTo
		{
			get
			{
				if (OptionsPage.CurrentMandatoryDotPlacement != MandatoryDotPlacement.RoleBoxEnd &&
					DrawMandatoryDot)
				{
					return MandatoryDotDecorator.Decorator;
				}
#if IMPLIEDJOINPATH
				else if (OptionsPage.CurrentMandatoryDotPlacement == MandatoryDotPlacement.RoleBoxEnd)
				{
					return new ImpliedFactJoinPathDecorator(this);
				}
#endif // IMPLIEDJOINPATH
				return base.DecoratorTo;
			}
			set
			{
			}
		}
		/// <summary>
		/// Helper function to determine if we should draw a mandatory dot
		/// </summary>
		protected bool DrawMandatoryDot
		{
			get
			{
				bool retVal = false;
				ObjectTypePlaysRole link;
				Role role;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRoleCollection)) &&
					role.IsMandatory)
				{
					retVal = true;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Switch between alethic and deontic style sets to draw
		/// the mandatory dot correctly
		/// </summary>
		public override StyleSet StyleSet
		{
			get
			{
				Role role;
				ObjectTypePlaysRole link;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRoleCollection)) &&
					role.IsMandatory &&
					role.MandatoryConstraintModality == ConstraintModality.Deontic)
				{
					// Note that we don't do anything with fonts with this style set, so the
					// static one is sufficient. Instance style sets also go through a font initiation
					// step inside the framework
					return DeonticClassStyleSet;
				}
				return base.StyleSet;
			}
		}
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.2F / 72.0F; // 1.2 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
			Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.Constraint);
			penSettings = new PenSettings();
			penSettings.Color = constraintForeColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = constraintForeColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		/// <summary>
		/// A style set used for drawing deontic mandatory decorators
		/// </summary>
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// Create an alternate style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet DeonticClassStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(ClassStyleSet);
					IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
					Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.DeonticConstraint);
					PenSettings penSettings = new PenSettings();
					penSettings.Color = constraintForeColor;
					retVal.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
					SolidBrush backgroundBrush = retVal.GetBrush(DiagramBrushes.DiagramBackground) as SolidBrush;
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = (backgroundBrush == null) ? constraintForeColor : backgroundBrush.Color;
					retVal.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
					myDeonticClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		#endregion // Customize appearance
		#region RolePlayerLink specific
		/// <summary>
		/// Get the ObjectTypePlaysRole link associated with this link shape
		/// </summary>
		public ObjectTypePlaysRole AssociatedRolePlayerLink
		{
			get
			{
				return ModelElement as ObjectTypePlaysRole;
			}
		}
		/// <summary>
		/// Configuring this link after it has been added to the diagram
		/// </summary>
		/// <param name="diagram">The parent diagram</param>
		public override void ConfiguringAsChildOf(ORMDiagram diagram)
		{
			// If we're already connected then walk away
			if (FromShape == null && ToShape == null)
			{
				ObjectTypePlaysRole modelLink = ModelElement as ObjectTypePlaysRole;
				ObjectType rolePlayer = modelLink.RolePlayer;
				FactType nestedFact = rolePlayer.NestedFactType;
				NodeShape fromShape;
				NodeShape toShape;
				if (null != (fromShape = diagram.FindShapeForElement(modelLink.PlayedRoleCollection.FactType) as NodeShape) &&
					null != (toShape = diagram.FindShapeForElement((nestedFact == null) ? rolePlayer as ModelElement : nestedFact) as NodeShape))
				{
					Connect(fromShape, toShape);
				}
			}
		}
		#endregion // RolePlayerLink specific
		#region Accessibility Properties
		/// <summary>
		/// Return the localized accessible name for the link
		/// </summary>
		public override string AccessibleName
		{
			get
			{
				return ResourceStrings.RolePlayerLinkAccessibleName;
			}
		}
		/// <summary>
		/// Return the localized accessible description
		/// </summary>
		public override string AccessibleDescription
		{
			get
			{
				return ResourceStrings.RolePlayerLinkAccessibleDescription;
			}
		}
		/// <summary>
		/// Describe the from role in terms of FactName.RoleName(RolePosition)
		/// </summary>
		protected override string FromAccessibleValue
		{
			get
			{
				ObjectTypePlaysRole link = ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				FactType fact = role.FactType;
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.RolePlayerLinkAccessibleFromValueFormat, fact.Name, role.Name, (fact.RoleCollection.IndexOf(role) + 1).ToString(CultureInfo.CurrentCulture));
			}
		}
		#endregion // Accessibility Properties
		#region Store Event Handlers
		/// <summary>
		///  Helper function to update the mandatory dot in response to events
		/// </summary>
		private static void UpdateDotDisplayOnMandatoryConstraintChange(Role role)
		{
			foreach (ModelElement mel in role.GetElementLinks(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid))
			{
				foreach (PresentationElement pel in mel.PresentationRolePlayers)
				{
					ShapeElement shape = pel as ShapeElement;
					if (shape != null)
					{
						shape.Invalidate(true);
					}
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

			MetaAttributeInfo attributeInfo = dataDirectory.FindMetaAttribute(SimpleMandatoryConstraint.ModalityMetaAttributeGuid);
			eventDirectory.ElementAttributeChanged.Add(attributeInfo, new ElementAttributeChangedEventHandler(InternalConstraintChangedEvent));
			MetaRelationshipInfo relInfo = dataDirectory.FindMetaRelationship(FactTypeHasInternalConstraint.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(relInfo, new ElementAddedEventHandler(InternalConstraintRoleSequenceAddedEvent));
			relInfo = dataDirectory.FindMetaRelationship(ConstraintRoleSequenceHasRole.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Add(relInfo, new ElementRemovedEventHandler(InternalConstraintRoleSequenceRoleRemovedEvent));
		}
		/// <summary>
		/// Detach event handlers from the store
		/// </summary>
		public static void DetachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaAttributeInfo attributeInfo = dataDirectory.FindMetaAttribute(SimpleMandatoryConstraint.ModalityMetaAttributeGuid);
			eventDirectory.ElementAttributeChanged.Remove(attributeInfo, new ElementAttributeChangedEventHandler(InternalConstraintChangedEvent));
			MetaRelationshipInfo relInfo = dataDirectory.FindMetaRelationship(FactTypeHasInternalConstraint.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(relInfo, new ElementAddedEventHandler(InternalConstraintRoleSequenceAddedEvent));
			relInfo = dataDirectory.FindMetaRelationship(ConstraintRoleSequenceHasRole.MetaRelationshipGuid);
			eventDirectory.ElementRemoved.Remove(relInfo, new ElementRemovedEventHandler(InternalConstraintRoleSequenceRoleRemovedEvent));
		}
		/// <summary>
		/// Update the link displays when the modality of a simple mandatory constraint changes
		/// </summary>
		private static void InternalConstraintChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			SimpleMandatoryConstraint smc = e.ModelElement as SimpleMandatoryConstraint;
			if (smc != null && !smc.IsRemoved)
			{
				RoleMoveableCollection roles = smc.RoleCollection;
				if (roles.Count != 0)
				{
					UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
				}
			}
		}
		/// <summary>
		/// Update the link displays when a role sequence for a mandatory constraint is added
		/// </summary>
		private static void InternalConstraintRoleSequenceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
			SimpleMandatoryConstraint constraint = link.InternalConstraintCollection as SimpleMandatoryConstraint;
			if (constraint != null && !constraint.IsRemoved)
			{
				RoleMoveableCollection roles = constraint.RoleCollection;
				if (roles.Count > 0)
				{
					Debug.Assert(roles.Count == 1); // Mandatory constraints have a single role only
					UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
				}
			}
		}
		/// <summary>
		/// Update the link display when a mandatory constraint role is removed
		/// </summary>
		private static void InternalConstraintRoleSequenceRoleRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			Role role;
			if (link.ConstraintRoleSequenceCollection is SimpleMandatoryConstraint &&
				(null != (role = link.RoleCollection)) &&
				!role.IsRemoved)
			{
				UpdateDotDisplayOnMandatoryConstraintChange(role);
			}
		}
		#endregion // Store Event Handlers
		#region Hack code to enable multiple links between the same fact/object pair
		private bool myHasBeenConnected;
		/// <summary>
		/// True if the link has ever been connected
		/// </summary>
		public bool HasBeenConnected
		{
			get
			{
				return myHasBeenConnected;
			}
			set
			{
				Debug.Assert(value, "HasBeenConnected should never be set to false");
				myHasBeenConnected = value;
			}
		}
		/// <summary>
		/// Due to the hackish nature of the way we're connecting
		/// players for rings, we need to reconnect when any link of
		/// the same type is deleted.
		/// UNDONE: MSBUG This is a huge hack that will go away if
		/// Microsoft passes link information into DoFoldToShape
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class RolePlayerRemoving : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;

				Role role = link.PlayedRoleCollection;
				FactType fact = role.FactType;
				if (fact != null && !fact.IsRemoving)
				{
					IList roles = fact.RoleCollection;
					int rolesCount = roles.Count;
					ObjectType rolePlayer = link.RolePlayer;
					for (int i = 0; i < rolesCount; ++i)
					{
						Role currentRole = (Role)roles[i];
						if (!currentRole.IsRemoving)
						{
							IList rolePlayerLinks = currentRole.GetElementLinks(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
							if (rolePlayerLinks.Count != 0)
							{
								ObjectTypePlaysRole playerLink = (ObjectTypePlaysRole)rolePlayerLinks[0];
								if (!playerLink.IsRemoving && object.ReferenceEquals(playerLink.RolePlayer, rolePlayer))
								{
									foreach (PresentationElement pel in playerLink.PresentationRolePlayers)
									{
										RolePlayerLink displayLink = pel as RolePlayerLink;
										if (displayLink != null)
										{
											displayLink.RipUp();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // Hack code to enable multiple links between the same fact/object pair
	}
}
