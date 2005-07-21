using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.Shell;
namespace Northface.Tools.ORM.ShapeModel
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
		public void ConfiguringAsChildOf(ORMDiagram diagram)
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
		#region Shape display update rules
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
		/// Update the link displays when a role sequence for a mandatory constraint is added
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.TopLevelCommit)]
		private class InternalConstraintRoleSequenceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				SimpleMandatoryConstraint constraint = link.InternalConstraintCollection as SimpleMandatoryConstraint;
				if (constraint != null)
				{
					RoleMoveableCollection roles = constraint.RoleCollection;
					if (roles.Count > 0)
					{
						Debug.Assert(roles.Count == 1); // Mandatory constraints have a single role only
						UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
					}
				}
			}
		}
		/// <summary>
		/// Update the link display when a mandatory constraint role is removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.TopLevelCommit)]
		private class InternalConstraintRoleSequenceRoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				Role role;
				if ((null != (link.ConstraintRoleSequenceCollection as SimpleMandatoryConstraint)) &&
				    (null != (role = link.RoleCollection)))
				{
					UpdateDotDisplayOnMandatoryConstraintChange(role);
				}
			}
		}
		#endregion // Shape display update rules
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
