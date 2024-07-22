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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	partial class RolePlayerLink : IReconfigureableLink, IConfigureAsChildShape, IDynamicColorGeometryHost
	{
		#region Member Variables
		/// <summary>
		/// Pen to draw a role box outline for a link fact type
		/// </summary>
		protected static readonly StyleSetResourceId UnarySignResource = new StyleSetResourceId("ORMArchitect", "RolePlayerLinkUnarySignResource");
		#endregion // Member Variables
		#region MandatoryDotStyle enum
		/// <summary>
		/// Specify the kind of mandatory dot to draw. This breaks down the
		/// graphics, not the underlying pen and brush styles.
		/// </summary>
		protected enum MandatoryDotStyle
		{
			/// <summary>
			/// No mandatory dot is drawn
			/// </summary>
			None,
			/// <summary>
			/// Draw a simple mandatory dot
			/// </summary>
			SimpleMandatory,
			/// <summary>
			/// Draw a dot indicate the positive fact type of a negatable unary pair.
			/// The mandatory state is incorporated via the style set.
			/// </summary>
			PositiveUnary,
			/// <summary>
			/// Draw a dot indicate the positive fact type of a negatable unary pair.
			/// Add dots to distinguish deontic form.
			/// </summary>
			DeonticPositiveUnary,
			/// <summary>
			/// Draw a dot indicate the negative fact type of a negatable unary pair.
			/// The mandatory state is incorporated via the style set.
			/// </summary>
			NegativeUnary,
			/// <summary>
			/// Draw a dot indicate the negative fact type of a negatable unary pair.
			/// Add dots to distinguish deontic form.
			/// </summary>
			DeonticNegativeUnary,
		}
		#endregion // MandatoryDotStyle enum
		#region MandatoryDotDecorator class
		/// <summary>
		/// The link decorator used to draw the mandatory
		/// constraint dot on a link.
		/// </summary>
		protected class MandatoryDotDecorator : DynamicColorLinkDecorator, ILinkDecoratorSettings
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static readonly LinkDecorator Decorator = new MandatoryDotDecorator();
			/// <summary>
			/// Create a mandatory dot decorator
			/// </summary>
			protected MandatoryDotDecorator()
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
		#region UnarySignDecorator class
		/// <summary>
		/// Base class for drawing positive and negative unary signs
		/// </summary>
		protected abstract class UnarySignDecorator : MandatoryDotDecorator, ILinkDecoratorRenderingState
		{
			private float myRenderingRotation;
			/// <summary>
			/// Create the default decorator instance
			/// </summary>
			protected UnarySignDecorator() { }

			/// <summary>
			/// Add the unary positive or minus sign and deontic pattern over the filled background.
			/// </summary>
			public override void DoPaintShape(RectangleD bounds, IGeometryHost shape, DiagramPaintEventArgs e)
			{
				// This +/- signs always need to be square with the page, not the link line. It
				// doesn't really matter where the solid background is rendered, so we rotate for
				// the full draw process.

				// Note that simply telling the base to not rotate is not sufficient here because
				// the base rotation occurs at the center right edge of the rendering box whereas
				// this rotation is needed in the true center of the bounding box, so this is not
				// simply an inverse of the base rotation.
				Graphics g = e.Graphics;
				Matrix rotationMatrix = g.Transform;
				Matrix restoreMatrix = rotationMatrix.Clone();
				rotationMatrix.RotateAt(-myRenderingRotation, PointD.ToPointF(bounds.Center));
				g.Transform = rotationMatrix;
				base.DoPaintShape(bounds, shape, e);

				Pen pen;
				StyleSetResourceId penId = RolePlayerLink.UnarySignResource;
				if (null != (pen = shape.GeometryStyleSet.GetPen(penId)))
				{
					IDynamicColorGeometryHost dynamicColors;
					Color restoreColor;
					if (null == (dynamicColors = shape as IDynamicColorGeometryHost) ||
						(restoreColor = dynamicColors.UpdateDynamicColor(penId, pen)).IsEmpty)
					{
						restoreColor = shape.UpdateGeometryLuminosity(e.View, pen);
					}
					else
					{
						shape.UpdateGeometryLuminosity(e.View, pen);
					}
					this.DrawSign(g, bounds, pen);
					pen.Color = restoreColor;

					// Dots are draw with the default pen background color. Draw after
					// the dynamic color is restored.
					if (this.ShouldDrawDeonticDots)
					{
						this.DrawDeonticDots(g, bounds, pen);
					}
				}

				// Put our graphics object back to where it was.
				g.Transform = restoreMatrix;
			}

			float ILinkDecoratorRenderingState.RotationRadians
			{
				set
				{
					myRenderingRotation = value;
				}
			}

			/// <summary>
			/// Draw the unary sign
			/// </summary>
			/// <param name="g">The graphics object.</param>
			/// <param name="bounds">Shape bounds</param>
			/// <param name="pen">The pen to draw with.</param>
			protected abstract void DrawSign(Graphics g, RectangleD bounds, Pen pen);

			/// <summary>
			/// Should the deontic dots be draw for this decorator
			/// </summary>
			protected virtual bool ShouldDrawDeonticDots
			{
				get
				{
					return false;
				}
			}

			/// <summary>
			/// Draw the deontic dots around the edge of the circle to form a dash pattern
			/// </summary>
			protected virtual void DrawDeonticDots(Graphics g, RectangleD bounds, Pen pen)
			{
				PointD center = bounds.Center;
				double centerX = center.X;
				double centerY = center.Y;
				double root2 = Math.Sqrt(2);
				double halfWidth = bounds.Width / 2;
				double dashOffsetOuter = (halfWidth + .2d / 72d) / root2;
				double dashOffsetInner = (halfWidth - .6d / 72d) / root2;

				g.DrawLine(pen, (float)(centerX + dashOffsetOuter), (float)(centerY + dashOffsetOuter), (float)(centerX + dashOffsetInner), (float)(centerY + dashOffsetInner));
				g.DrawLine(pen, (float)(centerX + dashOffsetOuter), (float)(centerY - dashOffsetOuter), (float)(centerX + dashOffsetInner), (float)(centerY - dashOffsetInner));
				g.DrawLine(pen, (float)(centerX - dashOffsetOuter), (float)(centerY + dashOffsetOuter), (float)(centerX - dashOffsetInner), (float)(centerY + dashOffsetInner));
				g.DrawLine(pen, (float)(centerX - dashOffsetOuter), (float)(centerY - dashOffsetOuter), (float)(centerX - dashOffsetInner), (float)(centerY - dashOffsetInner));
			}
		}
		#endregion // UnarySignDecorator class
		#region PositiveUnaryDecorator class
		/// <summary>
		/// A decorator displaying the sign and mandatory state for the
		/// positive fact type of a negatable unary pair.
		/// </summary>
		protected class PositiveUnaryDecorator : UnarySignDecorator
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static new readonly LinkDecorator Decorator = new PositiveUnaryDecorator();

			/// <summary>
			/// Single instance of this decorator for a deontic instance
			/// </summary>
			public static readonly LinkDecorator DeonticDecorator = new DeonticPositiveUnaryDecorator();

			private PositiveUnaryDecorator() { }
			/// <summary>
			/// Draw the plus sign in the provided graphics object.
			/// </summary>
			protected override void DrawSign(Graphics g, RectangleD bounds, Pen pen)
			{
				double signWidth = bounds.Width / 2 - 1d / 72d;

				PointD center = bounds.Center;
				double centerX = center.X;
				double centerY = center.Y;

				g.DrawLine(pen, (float)(centerX - signWidth), (float)centerY, (float)(centerX + signWidth), (float)centerY);
				g.DrawLine(pen, (float)centerX, (float)(centerY - signWidth), (float)centerX, (float)(centerY + signWidth));
			}

			private class DeonticPositiveUnaryDecorator : PositiveUnaryDecorator
			{
				protected override bool ShouldDrawDeonticDots
				{
					get
					{
						return true;
					}
				}
			}
		}
		#endregion // PositiveUnaryDecorator class
		#region NegativeUnaryDecorator class
		/// <summary>
		/// A decorator displaying the sign and mandatory state for the
		/// positive fact type of a negatable unary pair.
		/// </summary>
		protected class NegativeUnaryDecorator : UnarySignDecorator
		{
			/// <summary>
			/// Singleton instance of this decorator
			/// </summary>
			public static new readonly LinkDecorator Decorator = new NegativeUnaryDecorator();

			/// <summary>
			/// Single instance of this decorator for a deontic instance
			/// </summary>
			public static readonly LinkDecorator DeonticDecorator = new DeonticNegativeUnaryDecorator();

			private NegativeUnaryDecorator() { }
			/// <summary>
			/// Draw the minus sign in the provided graphics object.
			/// </summary>
			protected override void DrawSign(Graphics g, RectangleD bounds, Pen pen)
			{
				PointD center = bounds.Center;
				double width = bounds.Width / 2 - 1d / 72d; // Take off 1 point
				g.DrawLine(pen, (float)(center.X - width), (float)center.Y, (float)(center.X + width), (float)center.Y);
			}

			private class DeonticNegativeUnaryDecorator : NegativeUnaryDecorator
			{
				protected override bool ShouldDrawDeonticDots
				{
					get
					{
						return true;
					}
				}
			}
		}
		#endregion // NegativeUnaryDecorator class
		#region Customize appearance
		/// <summary>
		/// Draw the mandatory dot on the role box end, depending
		/// on the options settings
		/// </summary>
		public override LinkDecorator DecoratorFrom
		{
			get
			{
				if (OptionsPage.CurrentMandatoryDotPlacement != MandatoryDotPlacement.ObjectShapeEnd ||
					OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay != EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					switch (DrawMandatoryDot)
					{
						case MandatoryDotStyle.SimpleMandatory:
							return MandatoryDotDecorator.Decorator;
						case MandatoryDotStyle.PositiveUnary:
							return PositiveUnaryDecorator.Decorator;
						case MandatoryDotStyle.DeonticPositiveUnary:
							return PositiveUnaryDecorator.DeonticDecorator;
						case MandatoryDotStyle.NegativeUnary:
							return NegativeUnaryDecorator.Decorator;
						case MandatoryDotStyle.DeonticNegativeUnary:
							return NegativeUnaryDecorator.DeonticDecorator;
					}
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
					OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay == EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					switch (DrawMandatoryDot)
					{
						case MandatoryDotStyle.SimpleMandatory:
							return MandatoryDotDecorator.Decorator;
						case MandatoryDotStyle.PositiveUnary:
							return PositiveUnaryDecorator.Decorator;
						case MandatoryDotStyle.DeonticPositiveUnary:
							return PositiveUnaryDecorator.DeonticDecorator;
						case MandatoryDotStyle.NegativeUnary:
							return NegativeUnaryDecorator.Decorator;
						case MandatoryDotStyle.DeonticNegativeUnary:
							return NegativeUnaryDecorator.DeonticDecorator;
					}
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
		protected MandatoryDotStyle DrawMandatoryDot
		{
			get
			{
				MandatoryDotStyle retVal = MandatoryDotStyle.None;
				ObjectTypePlaysRole link;
				Role role;
				FactType factType;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRole)))
				{
					if (role.IsMandatory)
					{
						retVal = MandatoryDotStyle.SimpleMandatory;
					}
					else if (null != (factType = role.FactType))
					{
						switch (factType.UnaryPattern)
						{
							case UnaryValuePattern.NotUnary:
							case UnaryValuePattern.OptionalWithoutNegation:
							case UnaryValuePattern.OptionalWithoutNegationDefaultTrue:
								// Skip these, all others have a form of +/- sign
								break;
							case UnaryValuePattern.Negation:
								switch (factType.PositiveUnaryFactType.UnaryPattern)
								{
									case UnaryValuePattern.DeonticRequiredWithNegation:
									case UnaryValuePattern.DeonticRequiredWithNegationDefaultTrue:
									case UnaryValuePattern.DeonticRequiredWithNegationDefaultFalse:
										retVal = MandatoryDotStyle.DeonticNegativeUnary;
										break;
									default:
										retVal = MandatoryDotStyle.NegativeUnary;
										break;
								}
								break;
							case UnaryValuePattern.DeonticRequiredWithNegation:
							case UnaryValuePattern.DeonticRequiredWithNegationDefaultTrue:
							case UnaryValuePattern.DeonticRequiredWithNegationDefaultFalse:
								retVal = MandatoryDotStyle.DeonticPositiveUnary;
								break;
							default:
								retVal = MandatoryDotStyle.PositiveUnary;
								break;
						}
					}
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
				FactType factType;
				ObjectTypePlaysRole link;
				if ((null != (link = AssociatedRolePlayerLink)) &&
					(null != (role = link.PlayedRole)) &&
					(null != (factType = role.FactType)))
				{
					UnaryValuePattern unaryPattern = factType.UnaryPattern;
					if (unaryPattern == UnaryValuePattern.NotUnary)
					{
						if (role.IsMandatory && role.MandatoryConstraintModality == ConstraintModality.Deontic)
						{
							return DeonticStyleSet;
						}
					}
					else
					{
						if (unaryPattern == UnaryValuePattern.Negation)
						{
							unaryPattern = factType.PositiveUnaryFactType.UnaryPattern;
						}

						switch (unaryPattern)
						{
							case UnaryValuePattern.OptionalWithNegation:
							case UnaryValuePattern.OptionalWithNegationDefaultTrue:
							case UnaryValuePattern.OptionalWithNegationDefaultFalse:
								return OptionalUnaryStyleSet;
							case UnaryValuePattern.RequiredWithNegation:
							case UnaryValuePattern.RequiredWithNegationDefaultTrue:
							case UnaryValuePattern.RequiredWithNegationDefaultFalse:
								return MandatoryUnaryStyleSet;
							case UnaryValuePattern.DeonticRequiredWithNegation:
							case UnaryValuePattern.DeonticRequiredWithNegationDefaultTrue:
							case UnaryValuePattern.DeonticRequiredWithNegationDefaultFalse:
								return DeonticUnaryStyleSet;
						}
					}
				}
				return base.StyleSet;
			}
		}

		/// <summary>
		/// Pen to draw dotted line on optional ER roles
		/// </summary>
		private static readonly StyleSetResourceId BarkerEROptionalPen = new StyleSetResourceId("ORMArchitect", "BarkerEROptionalPen");
		/// <summary>
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			base.InitializeResources(classStyleSet);

			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.2F / 72.0F; // 1.2 Point. 0 Means 1 pixel, but should only be used for non-printed items
			classStyleSet.OverridePen(DiagramPens.ConnectionLine, penSettings);
			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(BarkerEROptionalPen, DiagramPens.ConnectionLine, penSettings);

			SolidBrush backgroundBrush = classStyleSet.GetBrush(DiagramBrushes.DiagramBackground) as SolidBrush;
			penSettings.Color = backgroundBrush != null ? backgroundBrush.Color : Color.White;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.DashStyle = DashStyle.Solid;
			penSettings.StartCap = LineCap.Flat;
			penSettings.EndCap = LineCap.Flat;
			classStyleSet.AddPen(UnarySignResource, DiagramPens.ShapeOutline, penSettings);

			IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
			Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.Constraint);
			penSettings = new PenSettings();
			penSettings.Color = constraintForeColor;
			classStyleSet.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);
			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = constraintForeColor;
			classStyleSet.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
		}
		private static StyleSet myDeonticClassStyleSet;
		/// <summary>
		/// The style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet DeonticStyleSet
		{
			get
			{
				// Note that we don't do anything with fonts with these style sets, so the
				// static sets are sufficient. Instance style sets also go through a font initiation
				// step inside the framework
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
		private static StyleSet myOptionalUnaryClassStyleSet;
		/// <summary>
		/// The style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet OptionalUnaryStyleSet
		{
			get
			{
				StyleSet retVal = myOptionalUnaryClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(ClassStyleSet);
					PenSettings penSettings = new PenSettings();
					penSettings.Color = SystemColors.WindowText;
					retVal.OverridePen(RolePlayerLink.UnarySignResource, penSettings);

					penSettings.Width = 1.0F / 72.0F; // 1.0 Point. 0 Means 1 pixel, but should only be used for non-printed items
					penSettings.Color = Color.LightGray;
					retVal.OverridePen(DiagramPens.ConnectionLineDecorator, penSettings);

					SolidBrush backgroundBrush = retVal.GetBrush(DiagramBrushes.DiagramBackground) as SolidBrush;
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = backgroundBrush != null ? backgroundBrush.Color : Color.White;
					retVal.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);
					myOptionalUnaryClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// The style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet MandatoryUnaryStyleSet
		{
			get
			{
				// The sign pen is in the base set, no customizations are needed.
				return base.StyleSet;
			}
		}
		private static StyleSet myDeonticUnaryClassStyleSet;
		/// <summary>
		/// Create an alternate style set for drawing deontic mandatory constraint decorators
		/// </summary>
		protected virtual StyleSet DeonticUnaryStyleSet
		{
			get
			{
				StyleSet retVal = myDeonticUnaryClassStyleSet;
				if (retVal == null)
				{
					retVal = new StyleSet(DeonticStyleSet);

					IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
					BrushSettings brushSettings = new BrushSettings();
					brushSettings.Color = fontsAndColors.GetForeColor(ORMDesignerColor.DeonticConstraint);
					retVal.OverrideBrush(DiagramBrushes.ConnectionLineDecorator, brushSettings);

					myDeonticUnaryClassStyleSet = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Set ZOrder layer
		/// </summary>
		public override double ZOrder
		{
			get
			{
				return base.ZOrder + ZOrderLayer.RolePlayerConnectors;
			}
		}
		#endregion // Customize appearance
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			ObjectTypePlaysRole link;
			Store store;
			bool forLink = penId == DiagramPens.ConnectionLine;
			bool forBorder = !forLink && penId == DiagramPens.ConnectionLineDecorator;
			if ((forLink || forBorder || (penId == RolePlayerLink.UnarySignResource)) &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (link = ModelElement as ObjectTypePlaysRole))
			{
				if (forLink)
				{
					IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, ObjectTypePlaysRole>[] providers;
					if (null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, ObjectTypePlaysRole>>(true)))
					{
						for (int i = 0; i < providers.Length; ++i)
						{
							Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.RolePlayerConnector, this, link);
							if (alternateColor != Color.Empty)
							{
								retVal = pen.Color;
								pen.Color = alternateColor;
								break;
							}
						}
					}
				}
				else if (forBorder)
				{
					IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, MandatoryConstraint>[] providers;
					Role playedRole;
					if (null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, MandatoryConstraint>>(true)) &&
						null != (playedRole = link.PlayedRole))
					{
						MandatoryConstraint mandatory =  null;
						UnaryValuePattern unaryPattern;
						FactType factType;
						ORMDiagramDynamicColor requestColor = ORMDiagramDynamicColor.Constraint;
						if (UnaryValuePattern.NotUnary == (unaryPattern = (factType = playedRole.FactType).UnaryPattern))
						{
							mandatory = playedRole.SimpleMandatoryConstraint;
							if (mandatory.Modality == ConstraintModality.Deontic)
							{
								requestColor = ORMDiagramDynamicColor.DeonticConstraint;
							}
						}
						else
						{
							if (unaryPattern == UnaryValuePattern.Negation)
							{
								factType = factType.PositiveUnaryFactType;
								unaryPattern = factType.UnaryPattern;
							}

							switch (ReduceUnaryPattern(unaryPattern))
							{
								case UnaryValuePattern.RequiredWithNegation:
									mandatory = factType.NegationMandatoryConstraint;
									break;
								case UnaryValuePattern.DeonticRequiredWithNegation:
									mandatory = factType.NegationMandatoryConstraint;
									requestColor = ORMDiagramDynamicColor.DeonticConstraint;
									break;
							}
						}

						if (mandatory != null)
						{
							for (int i = 0; i < providers.Length; ++i)
							{
								Color alternateColor = providers[i].GetDynamicColor(requestColor, this, mandatory);
								if (alternateColor != Color.Empty)
								{
									retVal = pen.Color;
									pen.Color = alternateColor;
									break;
								}
							}
						}
					}
				}
				else
				{
					UnaryValuePattern unaryPattern;
					FactType factType = link.PlayedRole.FactType;
					IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>[] providers;
					if (UnaryValuePattern.Negation == (unaryPattern = factType.UnaryPattern))
					{
						unaryPattern = factType.PositiveUnaryFactType.UnaryPattern;
					}

					switch (unaryPattern)
					{
						case UnaryValuePattern.OptionalWithNegation:
						case UnaryValuePattern.OptionalWithNegationDefaultFalse:
						case UnaryValuePattern.OptionalWithNegationDefaultTrue:
							if (null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>>(true)))
							{
								FactTypeShape fromShape = this.FromShape as FactTypeShape;
								if (fromShape != null)
								{
									for (int i = 0; i < providers.Length; ++i)
									{
										Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.ForegroundGraphics, fromShape, factType);
										if (alternateColor != Color.Empty)
										{
											retVal = pen.Color;
											pen.Color = alternateColor;
											break;
										}
									}
								}
							}
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
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, MandatoryConstraint>[] providers;
			ObjectTypePlaysRole link;
			Role playedRole;
			Store store;
			if (brushId == DiagramBrushes.ConnectionLineDecorator &&
				null != (store = Utility.ValidateStore(Store)) &&
				null != (providers = ((IFrameworkServices)store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, RolePlayerLink, MandatoryConstraint>>(true)) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (link = ModelElement as ObjectTypePlaysRole) &&
				null != (playedRole = link.PlayedRole))
			{
				MandatoryConstraint mandatory = null;
				UnaryValuePattern unaryPattern;
				FactType factType;
				ORMDiagramDynamicColor dynamicColor = ORMDiagramDynamicColor.Constraint;
				if (UnaryValuePattern.NotUnary == (unaryPattern = (factType = playedRole.FactType).UnaryPattern))
				{
					if (null != (mandatory = playedRole.SimpleMandatoryConstraint) && mandatory.Modality == ConstraintModality.Deontic) // The brush draws the middle without the background color, which we don't change
					{
						mandatory = null;
					}
					else if (mandatory.Modality == ConstraintModality.Deontic)
					{
						dynamicColor = ORMDiagramDynamicColor.DeonticConstraint;
					}
				}
				else
				{
					if (unaryPattern == UnaryValuePattern.Negation)
					{
						factType = factType.PositiveUnaryFactType;
						unaryPattern = factType.UnaryPattern;
					}

					switch (ReduceUnaryPattern(unaryPattern))
					{
						case UnaryValuePattern.RequiredWithNegation:
							mandatory = factType.NegationMandatoryConstraint;
							break;
						case UnaryValuePattern.DeonticRequiredWithNegation:
							mandatory = factType.NegationMandatoryConstraint;
							dynamicColor = ORMDiagramDynamicColor.DeonticConstraint;
							break;
					}
				}

				if (mandatory != null)
				{
					for (int i = 0; i < providers.Length; ++i)
					{
						Color alternateColor = providers[i].GetDynamicColor(dynamicColor, this, mandatory);
						if (alternateColor != Color.Empty)
						{
							retVal = solidBrush.Color;
							solidBrush.Color = alternateColor;
							break;
						}
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
		#region EntityRelationship learning mode support
		private const double CrowsFootHeight = .12;
		private const double CrowsFootHalfWidth = .05;
		private const double InfEngOuterOneMarkOffset = .03;
		private const double InfEngInnerOneMarkOffset = .06;
		private const double InfEngMarkerHalfWidth = .04;
		private const bool CrowsFootParallelMode = true;
		#region EntityRelationshipShapeGeometry class
		private sealed class EntityRelationshipShapeGeometry : ClickThroughObliqueBinaryLinkShapeGeometry
		{
			#region Constructor and singleton
			/// <summary>
			/// Singleton EntityRelationshipShapeGeometry instance
			/// </summary>
			public static new readonly ShapeGeometry ShapeGeometry = new EntityRelationshipShapeGeometry();
			/// <summary>
			/// Protected default constructor. The class should be used
			/// as a singleton instead of being publicly constructed.
			/// </summary>
			private EntityRelationshipShapeGeometry()
			{
			}
			#endregion // Constructor and singleton
			/// <summary>
			/// Paint the solid crowsfoot on the end of an optional line
			/// </summary>
			protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				RolePlayerLink connector;
				EdgePointCollection edgePoints;
				int edgePointCount;
				Pen pen;
				ShapeElement shapeHost;
				IOffsetBorderPoint offsetPointProvider;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker &&
					RoleMultiplicity.ZeroToMany == (connector = (RolePlayerLink)geometryHost).GetDisplayRoleMultiplicity(displaySetting) &&
					null != (edgePoints = connector.EdgePoints) &&
					1 < (edgePointCount = edgePoints.Count) &&
					null != (shapeHost = connector.ToShape) &&
					null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint) &&
					null != (pen = geometryHost.GeometryStyleSet.GetPen(DiagramPens.ConnectionLine)))
				{
					Color restoreColor = pen.Color;
					pen.Color = geometryHost.UpdateGeometryLuminosity(e.View, pen);
					PointD pointOnBorder = edgePoints[edgePointCount - 1].Point;
					double angle = GeometryUtility.CalculateRadiansRotationAngle(edgePoints[0].Point, pointOnBorder);
					PointD vertexPoint = pointOnBorder;
					vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
					e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(pointOnBorder));
					PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
					if (offsetBorderPoint.HasValue)
					{
						e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
					}
					offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
					if (offsetBorderPoint.HasValue)
					{
						e.Graphics.DrawLine(pen, PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
					}
					pen.Color = restoreColor;
				}
				base.DoPaintGeometry(e, geometryHost);
			}
			/// <summary>
			/// Return a path modified to include any ER multiplicity decorators
			/// </summary>
			public override GraphicsPath GetPath(IGeometryHost geometryHost)
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Off)
				{
					return base.GetPath(geometryHost);
				}
				RolePlayerLink connector = (RolePlayerLink)geometryHost;
				EdgePointCollection edgePoints;
				int edgePointCount;
				RoleMultiplicity multiplicity;
				if (RoleMultiplicity.Unspecified != (multiplicity = connector.GetDisplayRoleMultiplicity(displaySetting)) &&
					1 < (edgePointCount = (edgePoints = connector.EdgePoints).Count))
				{
					switch (displaySetting)
					{
						case EntityRelationshipBinaryMultiplicityDisplay.CrowsFootOnly:
						CrowsFootOnly:
							switch (multiplicity)
							{
								case RoleMultiplicity.OneToMany:
								case RoleMultiplicity.ZeroToMany:
									{
										PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
										PointD roleBoxEndPoint = edgePoints[0].Point;
										double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
										GraphicsPath path = base.UninitializedPath;
										path.Reset();
										path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
										PointD vertexPoint = objectTypeEndPoint;
										vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
										ShapeElement shapeHost;
										IOffsetBorderPoint offsetPointProvider;
										if (null != (shapeHost = connector.ToShape) &&
											null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
										{
											PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
											if (offsetBorderPoint.HasValue)
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
											}
											offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
											if (offsetBorderPoint.HasValue)
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
											}
										}
										return path;
									}
							}
							break;
						case EntityRelationshipBinaryMultiplicityDisplay.Barker:
							// Stop short of the crows foot if optional, draw the broken line under the solid crows foot
							switch (multiplicity)
							{
								case RoleMultiplicity.OneToMany:
									// Single pen only, include the crowsfoot as part of the path
									goto CrowsFootOnly;
								case RoleMultiplicity.ZeroToMany:
									// Stop the path at the vertex point, use a different pen for the crowsfoot in DoPaintGeometry
									{
										PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
										PointD roleBoxEndPoint = edgePoints[0].Point;
										double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
										GraphicsPath path = base.UninitializedPath;
										path.Reset();
										objectTypeEndPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
										path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
										return path;
									}
							}
							break;
						case EntityRelationshipBinaryMultiplicityDisplay.InformationEngineering:
							{
								PointD objectTypeEndPoint = edgePoints[edgePointCount - 1].Point;
								PointD roleBoxEndPoint = edgePoints[0].Point;
								double angle = GeometryUtility.CalculateRadiansRotationAngle(roleBoxEndPoint, objectTypeEndPoint);
								double cosAngle = Math.Cos(angle);
								double sinAngle = Math.Sin(angle);
								GraphicsPath path = base.UninitializedPath;
								path.Reset();
								switch (multiplicity)
								{
									case RoleMultiplicity.ExactlyOne:
										{
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
											path.StartFigure();
											PointD oneMarkLeft = objectTypeEndPoint;
											oneMarkLeft.Offset(InfEngInnerOneMarkOffset * cosAngle, InfEngInnerOneMarkOffset * sinAngle);
											PointD oneMarkRight = oneMarkLeft;
											oneMarkLeft.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));
											oneMarkLeft.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											oneMarkRight.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));
											break;
										}
									case RoleMultiplicity.ZeroToOne:
										{
											PointD circleStart = objectTypeEndPoint;
											circleStart.Offset((InfEngInnerOneMarkOffset + InfEngOuterOneMarkOffset + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * cosAngle, (InfEngInnerOneMarkOffset + InfEngOuterOneMarkOffset + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * sinAngle);
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(circleStart));
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddArc(
												(float)(circleStart.X - InfEngMarkerHalfWidth),
												(float)(circleStart.Y - InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												0,
												360);
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));
											circleStart.Offset(-InfEngOuterOneMarkOffset * cosAngle, -InfEngOuterOneMarkOffset * sinAngle);
											PointD oneMarkRight = circleStart;
											circleStart.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(oneMarkRight));
											break;
										}
									case RoleMultiplicity.ZeroToMany:
										{
											PointD circleStart = objectTypeEndPoint;
											circleStart.Offset((CrowsFootHeight + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * cosAngle, (CrowsFootHeight + InfEngMarkerHalfWidth + InfEngMarkerHalfWidth) * sinAngle);
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(circleStart));
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddArc(
												(float)(circleStart.X - InfEngMarkerHalfWidth),
												(float)(circleStart.Y - InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												(float)(InfEngMarkerHalfWidth + InfEngMarkerHalfWidth),
												0,
												360);
											circleStart.Offset(-InfEngMarkerHalfWidth * cosAngle, -InfEngMarkerHalfWidth * sinAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));

											ShapeElement shapeHost;
											IOffsetBorderPoint offsetPointProvider;
											if (null != (shapeHost = connector.ToShape) &&
												null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
											{
												path.StartFigure();
												path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(objectTypeEndPoint));
												PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, circleStart, CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(offsetBorderPoint.Value));
												}
												offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, circleStart, -CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(circleStart), PointD.ToPointF(offsetBorderPoint.Value));
												}
											}
											break;
										}
									case RoleMultiplicity.OneToMany:
										{
											path.AddLine(PointD.ToPointF(roleBoxEndPoint), PointD.ToPointF(objectTypeEndPoint));
											PointD vertexPoint = objectTypeEndPoint;
											vertexPoint.Offset(CrowsFootHeight * cosAngle, CrowsFootHeight * sinAngle);

											// Add the one mark
											PointD oneMarkLeft = vertexPoint;
											oneMarkLeft.Offset(InfEngOuterOneMarkOffset * cosAngle, InfEngOuterOneMarkOffset * sinAngle);
											PointD oneMarkRight = oneMarkLeft;
											oneMarkLeft.Offset(-InfEngMarkerHalfWidth * sinAngle, InfEngMarkerHalfWidth * cosAngle);
											oneMarkRight.Offset(InfEngMarkerHalfWidth * sinAngle, -InfEngMarkerHalfWidth * cosAngle);
											path.StartFigure();
											path.AddLine(PointD.ToPointF(oneMarkLeft), PointD.ToPointF(oneMarkRight));

											ShapeElement shapeHost;
											IOffsetBorderPoint offsetPointProvider;
											if (null != (shapeHost = connector.ToShape) &&
												null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
											{
												PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
												}
												offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, objectTypeEndPoint, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
												if (offsetBorderPoint.HasValue)
												{
													path.StartFigure();
													path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
												}
											}
											break;
										}
								}
								return path;
							}
					}
				}
				return base.GetPath(geometryHost);
			}
		}
		#endregion // EntityRelationshipShapeGeometry class
		/// <summary>
		/// Add crowsfoot area to excluded clipregion if it is not included in the path. This
		/// is synchronized with EntityRelationshipShapeGeometry.DoPaintGeometry
		/// </summary>
		public override void ExcludeFromClipRegion(Graphics g, Matrix matrix, GraphicsPath perimeter)
		{
			base.ExcludeFromClipRegion(g, matrix, perimeter);
			EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
			EdgePointCollection edgePoints;
			int edgePointCount;
			ShapeElement shapeHost;
			IOffsetBorderPoint offsetPointProvider;
			if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker &&
				RoleMultiplicity.ZeroToMany == GetDisplayRoleMultiplicity(displaySetting) &&
				null != (edgePoints = EdgePoints) &&
				1 < (edgePointCount = edgePoints.Count) &&
				null != (shapeHost = ToShape) &&
				null != (offsetPointProvider = shapeHost.ShapeGeometry as IOffsetBorderPoint))
			{
				PointD pointOnBorder = edgePoints[edgePointCount - 1].Point;
				double angle = GeometryUtility.CalculateRadiansRotationAngle(edgePoints[0].Point, pointOnBorder);
				PointD vertexPoint = pointOnBorder;
				vertexPoint.Offset(CrowsFootHeight * Math.Cos(angle), CrowsFootHeight * Math.Sin(angle));
				GraphicsPath path = ExcludePath;
				PointD? offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, CrowsFootHalfWidth, CrowsFootParallelMode);
				if (offsetBorderPoint.HasValue)
				{
					path.AddLine(PointD.ToPointF(pointOnBorder), PointD.ToPointF(offsetBorderPoint.Value));
					path.AddLine(PointD.ToPointF(offsetBorderPoint.Value), PointD.ToPointF(vertexPoint));
				}
				else
				{
					path.AddLine(PointD.ToPointF(pointOnBorder), PointD.ToPointF(vertexPoint));
				}
				offsetBorderPoint = offsetPointProvider.OffsetBorderPoint(shapeHost, pointOnBorder, vertexPoint, -CrowsFootHalfWidth, CrowsFootParallelMode);
				if (offsetBorderPoint.HasValue)
				{
					path.AddLine(PointD.ToPointF(vertexPoint), PointD.ToPointF(offsetBorderPoint.Value));
				}
				path.CloseFigure();
				g.SetClip(path, CombineMode.Exclude);
			}
		}
		/// <summary>
		/// Change the geometry if we're drawing any Entity Relationship options
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				RoleMultiplicity multiplicity = GetDisplayRoleMultiplicity(displaySetting);
				if (multiplicity != RoleMultiplicity.Unspecified)
				{
					if (displaySetting != EntityRelationshipBinaryMultiplicityDisplay.CrowsFootOnly ||
						multiplicity == RoleMultiplicity.OneToMany ||
						multiplicity == RoleMultiplicity.ZeroToMany)
					{
						return EntityRelationshipShapeGeometry.ShapeGeometry;
					}
				}
				return base.ShapeGeometry;
			}
		}
		/// <summary>
		/// Return the display role multiplicity for this link. If the options
		/// page settings do not display multiplicity then we always return Unspecified.
		/// Note that Information Engineering and UML fold multiplicity and cardinality the
		/// the same, but the Barker solid line (mandatory) is on the same side as the ORM
		/// mandatory dot. This will monkey with the multiplicity for Barker mode to reflect
		/// the mandatory on the near side.
		/// </summary>
		private RoleMultiplicity GetDisplayRoleMultiplicity(EntityRelationshipBinaryMultiplicityDisplay displaySetting)
		{
			ObjectTypePlaysRole link;
			Role role;
			RoleMultiplicity multiplicity;
			if (displaySetting != EntityRelationshipBinaryMultiplicityDisplay.Off &&
				null != (link = AssociatedRolePlayerLink) &&
				null != (role = link.PlayedRole) &&
				RoleMultiplicity.Indeterminate != (multiplicity = role.Multiplicity))
			{
				if (this is RolePlayerProxyLink)
				{
					multiplicity = RoleMultiplicity.ExactlyOne;
				}
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker)
				{
					MandatoryConstraint constraint = role.SimpleMandatoryConstraint;
					bool mandatory = constraint != null && constraint.Modality == ConstraintModality.Alethic;
					switch (multiplicity)
					{
						case RoleMultiplicity.ZeroToMany:
						case RoleMultiplicity.OneToMany:
							multiplicity = mandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
							break;
						case RoleMultiplicity.ZeroToOne:
						case RoleMultiplicity.ExactlyOne:
							multiplicity = mandatory ? RoleMultiplicity.ExactlyOne : RoleMultiplicity.ZeroToOne;
							break;
					}
				}
				return multiplicity;
			}
			return RoleMultiplicity.Unspecified;
		}
		/// <summary>
		/// Return a dashed pen if we're optional and doing BarkerER compatibility
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				EntityRelationshipBinaryMultiplicityDisplay displaySetting = OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay;
				if (displaySetting == EntityRelationshipBinaryMultiplicityDisplay.Barker)
				{
					switch (GetDisplayRoleMultiplicity(displaySetting))
					{
						case RoleMultiplicity.ZeroToMany:
						case RoleMultiplicity.ZeroToOne:
							return BarkerEROptionalPen;
					}
				}
				return base.OutlinePenId;
			}
		}
		#endregion // EntityRelationship learning mode support
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
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected new void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			base.ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
			(this as IReconfigureableLink).Reconfigure(null);
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
		}
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected void Reconfigure(ShapeElement discludedShape)
		{
			ObjectTypePlaysRole modelLink = ModelElement as ObjectTypePlaysRole;
			ObjectType rolePlayer = modelLink.RolePlayer;
			FactType nestedFact = rolePlayer.NestedFactType;

			MultiShapeUtility.ReconfigureLink(this, modelLink.PlayedRole.FactType, (nestedFact == null) ? rolePlayer as ModelElement : nestedFact, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
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
				Role role = link.PlayedRole;
				FactType fact = role.FactType;
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.RolePlayerLinkAccessibleFromValueFormat, fact.Name, role.Name, (fact.RoleCollection.IndexOf(role) + 1).ToString(CultureInfo.CurrentCulture));
			}
		}
		#endregion // Accessibility Properties
		#region Update Rules
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType), FireTime=LocalCommit
		/// </summary>
		private static void FactTypeChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == FactType.UnaryPatternDomainPropertyId)
			{
				ModelElement element = e.ModelElement;
				if (!element.IsDeleted && !element.IsDeleting)
				{
					FactType factType = (FactType)element;
					UnaryValuePattern oldValue = ReduceUnaryPattern((UnaryValuePattern)e.OldValue);
					UnaryValuePattern newValue = ReduceUnaryPattern(factType.UnaryPattern);
					if (oldValue != newValue)
					{
						bool refreshRequired = false;
						bool refreshNegation = true;
						switch (newValue)
						{
							case UnaryValuePattern.NotUnary:
								refreshRequired = oldValue != UnaryValuePattern.OptionalWithoutNegation;
								refreshNegation = false;
								break;
							case UnaryValuePattern.Negation:
								// This will result in an add or deletion, no need to look at it here.
								refreshNegation = false;
								break;
							case UnaryValuePattern.OptionalWithoutNegation:
								refreshNegation = false;
								refreshRequired = true;
								break;
							case UnaryValuePattern.OptionalWithNegation:
							case UnaryValuePattern.RequiredWithNegation:
							case UnaryValuePattern.DeonticRequiredWithNegation:
								refreshRequired = true;
								break;
						}

						if (refreshRequired)
						{
							UpdateUnaryLinkLines(factType);
							if (refreshNegation && null != (factType = factType.NegationUnaryFactType))
							{
								UpdateUnaryLinkLines(factType);
							}
						}
					}
				}
			}
		}
		private static void UpdateUnaryLinkLines(FactType factType)
		{
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
			{
				FactTypeShape shape = pel as FactTypeShape;
				if (shape != null)
				{
					foreach (RolePlayerLink linkShape in MultiShapeUtility.GetEffectiveAttachedLinkShapesFrom<RolePlayerLink>(shape))
					{
						linkShape.InvalidateRequired(true);
					}

				}
			}
		}
		/// <summary>
		/// Strip default portions of a unary value pattern so we
		/// can compare the parts that affect link display only.
		/// </summary>
		private static UnaryValuePattern ReduceUnaryPattern(UnaryValuePattern pattern)
		{
			switch (pattern)
			{
				case UnaryValuePattern.OptionalWithoutNegationDefaultTrue:
					pattern = UnaryValuePattern.OptionalWithoutNegation;
					break;
				case UnaryValuePattern.OptionalWithNegationDefaultTrue:
				case UnaryValuePattern.OptionalWithNegationDefaultFalse:
					pattern = UnaryValuePattern.OptionalWithNegation;
					break;
				case UnaryValuePattern.RequiredWithNegationDefaultTrue:
				case UnaryValuePattern.RequiredWithNegationDefaultFalse:
					pattern = UnaryValuePattern.RequiredWithNegation;
					break;
				case UnaryValuePattern.DeonticRequiredWithNegationDefaultTrue:
				case UnaryValuePattern.DeonticRequiredWithNegationDefaultFalse:
					pattern = UnaryValuePattern.DeonticRequiredWithNegation;
					break;
			}
			return pattern;
		}
		#endregion // Update Rules
		#region Store Event Handlers
		/// <summary>
		///  Helper function to update the mandatory dot in response to events
		/// </summary>
		private static void UpdateDotDisplayOnMandatoryConstraintChange(Role role)
		{
			InvalidateRolePlayerLinks(role);
			if (OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay == EntityRelationshipBinaryMultiplicityDisplay.InformationEngineering)
			{
				// The opposite links also need updating
				RoleBase oppositeRole = role.OppositeOrUnaryRole;
				if (oppositeRole != null)
				{
					InvalidateRolePlayerLinks(oppositeRole.Role);
				}
			}
		}
		/// <summary>
		/// Helper function to invalidate roles
		/// </summary>
		private static void InvalidateRolePlayerLinks(Role role)
		{
			foreach (ObjectTypePlaysRole objectTypePlaysRole in DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(objectTypePlaysRole))
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
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="RolePlayerLink"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(MandatoryConstraint.ModalityDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(InternalConstraintModalityChangedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRelationship(FactSetConstraint.DomainClassId), new EventHandler<ElementAddedEventArgs>(InternalConstraintRoleSequenceAddedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainRelationship(ConstraintRoleSequenceHasRole.DomainClassId), new EventHandler<ElementDeletedEventArgs>(InternalConstraintRoleSequenceRoleRemovedEvent), action);
		}
		/// <summary>
		/// Update the link displays when the modality of a simple mandatory constraint changes
		/// </summary>
		private static void InternalConstraintModalityChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ModelElement mel = e.ModelElement;
			if (!mel.IsDeleted)
			{
				switch (((IConstraint)mel).ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
						LinkedElementCollection<Role> roles = ((SetConstraint)mel).RoleCollection;
						if (roles.Count != 0)
						{
							UpdateDotDisplayOnMandatoryConstraintChange(roles[0]);
						}
						break;
					case ConstraintType.InternalUniqueness:
						if (OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay != EntityRelationshipBinaryMultiplicityDisplay.Off)
						{
							foreach (FactType factType in ((SetConstraint)mel).FactTypeCollection)
							{
								LinkedElementCollection<RoleBase> roleBases = factType.RoleCollection;
								if (roleBases.Count == 2)
								{
									foreach (RoleBase roleBase in roleBases)
									{
										InvalidateRolePlayerLinks(roleBase.Role);
									}
								}
							}
						}
						break;
				}
			}
		}
		/// <summary>
		/// Update the link displays when a role sequence for a mandatory constraint is added
		/// </summary>
		private static void InternalConstraintRoleSequenceAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactSetConstraint link = e.ModelElement as FactSetConstraint;
			MandatoryConstraint constraint = link.SetConstraint as MandatoryConstraint;
			if (constraint != null && !constraint.IsDeleted && constraint.IsSimple)
			{
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
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
		private static void InternalConstraintRoleSequenceRoleRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			Role role;
			MandatoryConstraint constraint;
			if (null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				constraint.IsSimple &&
				(null != (role = link.Role)) &&
				!role.IsDeleted)
			{
				UpdateDotDisplayOnMandatoryConstraintChange(role);
			}
		}
		#endregion // Store Event Handlers
	}
	partial class RolePlayerProxyLink : IReconfigureableLink
	{
		/// <summary>
		/// Implements <see cref="IReconfigureableLink.Reconfigure"/>
		/// </summary>
		protected new void Reconfigure(ShapeElement discludedShape)
		{
			ObjectTypePlaysRole modelLink = (ObjectTypePlaysRole)ModelElement;
			ObjectType rolePlayer = modelLink.RolePlayer;
			FactType nestedFact = rolePlayer.NestedFactType;

			MultiShapeUtility.ReconfigureLink(this, modelLink.PlayedRole.Proxy.FactType, (nestedFact == null) ? rolePlayer as ModelElement : nestedFact, discludedShape);
		}
		void IReconfigureableLink.Reconfigure(ShapeElement discludedShape)
		{
			Reconfigure(discludedShape);
		}
	}
}
