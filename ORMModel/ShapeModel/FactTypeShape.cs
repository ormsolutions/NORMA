using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
namespace Northface.Tools.ORM.ShapeModel
{
	#region ConstraintDisplayPosition enum
	/// <summary>
	/// Determines where internal constraints are drawn
	/// on a facttype
	/// </summary>
	[CLSCompliant(true)]
	public enum ConstraintDisplayPosition
	{
		/// <summary>
		/// Draw the constraints above the role boxes
		/// </summary>
		Top,
		/// <summary>
		/// Draw the constraints below the role boxes
		/// </summary>
		Bottom
	}
	#endregion ConstraintDisplayPosition enum
	#region FactTypeShape class
	public partial class FactTypeShape
	{
		#region ConstraintBoxRoleActivity enum
		/// <summary>
		/// The activity of a role in a ConstraintBox
		/// </summary>
		protected enum ConstraintBoxRoleActivity
		{
			/// <summary>
			/// The role is inactive
			/// </summary>
			Inactive,
			/// <summary>
			/// The role is active
			/// </summary>
			Active,
			/// <summary>
			/// The role is, technically speaking, not supposed to be in this box.  Only used for binary fact internal constraint compression.
			/// </summary>
			NotInBox
		}
		#endregion // ConstraintBoxRoleActivity enum
		#region ConstraintBox struct
		/// <summary>
		/// Defines a box to contain the constraint.
		/// </summary>
		protected struct ConstraintBox
		{
			#region Member Variables
			/// <summary>
			/// The bounding box to use.
			/// </summary>
			private RectangleD myBounds;
			/// <summary>
			/// The type of constraint contained is this box.
			/// </summary>
			private ConstraintType myConstraintType;
			/// <summary>
			/// Roles relative to the current order of the roles
			/// on the facr for which this constraint applies.
			/// </summary>
			private ConstraintBoxRoleActivity[] myActiveRoles;
			/// <summary>
			/// The constraint object this box is for.
			/// </summary>
			private IFactConstraint myFactConstraint;
			/// <summary>
			/// The cached role collection
			/// </summary>
			private IList<Role> myRoleCollection;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="factConstraint">A reference to the original constraint that this ConstraintBox is based on.</param>
			/// <param name="factRoleCount">The number of roles for the context fact.</param>
			[CLSCompliant(false)]
			public ConstraintBox(IFactConstraint factConstraint, int factRoleCount)
			{
				Debug.Assert(factConstraint != null);
				Debug.Assert(factRoleCount > 0 && factRoleCount >= factConstraint.RoleCollection.Count);
				myBounds = new RectangleD();
				IConstraint constraint = factConstraint.Constraint;
				myConstraintType = constraint.ConstraintType;
				myActiveRoles = new ConstraintBoxRoleActivity[factRoleCount];
				myFactConstraint = factConstraint;
				myRoleCollection = null;
			}
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="factConstraint">A reference to the original constraint that this ConstraintBox is based on.</param>
			/// <param name="roleActivity">A representation of the factConstraint's role activity within the fact.</param>
			[CLSCompliant(false)]
			public ConstraintBox(IFactConstraint factConstraint, ConstraintBoxRoleActivity[] roleActivity)
			{
				if (!roleActivity.Equals(PreDefinedConstraintBoxRoleActivities_FullySpanning))
				{
					int roleActivityCount = roleActivity.Length;
					Debug.Assert(factConstraint != null);
					Debug.Assert(roleActivityCount > 0 && roleActivityCount >= factConstraint.RoleCollection.Count);
					myBounds = new RectangleD();
					IConstraint constraint = factConstraint.Constraint;
					myConstraintType = constraint.ConstraintType;
					myActiveRoles = roleActivity;
					myFactConstraint = factConstraint;
					myRoleCollection = null;
				}
				else
				{
					myBounds = new RectangleD();
					IConstraint constraint = factConstraint.Constraint;
					myConstraintType = constraint.ConstraintType;
					myActiveRoles = roleActivity;
					myFactConstraint = factConstraint;
					myRoleCollection = null;
				}
			}
			#endregion // Constructors
			#region Accessor Properties
			/// <summary>
			/// The bounding box to use.
			/// </summary>
			public RectangleD Bounds
			{
				get
				{
					return myBounds;
				}
				set
				{
					myBounds = value;
				}
			}
			/// <summary>
			/// The type of constraint contained is this box.
			/// </summary>
			public ConstraintType ConstraintType
			{
				get
				{
					return myConstraintType;
				}
			}
			/// <summary>
			/// Roles relative to the current order of the roles
			/// on the facr for which this constraint applies.
			/// </summary>
			public ConstraintBoxRoleActivity[] ActiveRoles
			{
				get
				{
					return myActiveRoles;
				}
			}
			/// <summary>
			/// The constraint object this box is for.
			/// </summary>
			[CLSCompliant(false)]
			public IFactConstraint FactConstraint
			{
				get
				{
					return myFactConstraint;
				}
			}
			/// <summary>
			/// A (cached) reference to the fact constraint's role collection
			/// </summary>
			[CLSCompliant(false)]
			public IList<Role> RoleCollection
			{
				get
				{
					IList<Role> roles = myRoleCollection;
					if (roles == null)
					{
						myRoleCollection = roles = myFactConstraint.RoleCollection;
					}
					return roles;
				}
			}
			/// <summary>
			/// Tests if this constraint is a fully spanning constraint.
			/// </summary>
			/// <value>True if the constraint is fully spanning.</value>
			public bool IsSpanning
			{
				get
				{
					return myActiveRoles.Equals(PreDefinedConstraintBoxRoleActivities_FullySpanning);
				}
			}
			#endregion // Accessor Properties
			#region Array sorting code
			/// <summary>
			/// Sort the constraint boxes and place non-displayed constraints
			/// at the end of the array. Return the number of boxes that
			/// actually need displaying.
			/// </summary>
			/// <param name="boxes">An existing array of constraint boxes
			/// created with the parametrized constructor</param>
			/// <returns>The number of significant boxes</returns>
			public static int OrderConstraintBoxes(ConstraintBox[] boxes)
			{
				Array.Sort(boxes, Compare);
				int fullBoxCount = boxes.Length;
				int significantBoxCount = 0;
				int i;
				for (i = 0; i < fullBoxCount; ++i)
				{
					if (IsConstraintTypeVisible(boxes[i].ConstraintType))
					{
						// UNDONE: Possibly add more code here, needs to
						// match algorithm in Compare
					}
					else
					{
						// All insignificant ones are sorted to the end
						significantBoxCount = i;
						break;
					}
				}
				if (i == fullBoxCount)
				{
					significantBoxCount = fullBoxCount;
				}
				return significantBoxCount;
			}
			/// <summary>
			/// Compares two ConstraintBoxes
			/// </summary>
			/// <param name="c1">First ConstraintBox to compare.</param>
			/// <param name="c2">Second ConstraintBox to compare.</param>
			/// <returns>Value indicating the relative order of the ConstraintBoxes. -1 if c1 &lt; c2, 0 if c1 == c2, 1 if c1 &gt; c2</returns>
			private static int Compare(ConstraintBox c1, ConstraintBox c2)
			{
				// Order the constraints, bringing preferred uniqueness constraints to the top of 
				// internal uniqueness constraint.  Internal constraints will be on the bottom of
				// external constraints.
				ConstraintType ct1 = c1.ConstraintType;
				ConstraintType ct2 = c2.ConstraintType;
				int retVal = 0;

				if (ct1 != ct2)
				{
					int ctOrder1 = RelativeSortPosition(ct1);
					int ctOrder2 = RelativeSortPosition(ct2);
					if (ctOrder1 < ctOrder2)
					{
						retVal = -1;
					}
					else if (ctOrder1 > ctOrder2)
					{
						retVal = 1;
					}
				}
				else if (IsConstraintTypeVisible(ct1))
				{
					// If one of them is the preferred identifier, it rises to the top.
					//if (c1.Constraint.PreferredIdentifierFor.
					// else
					//	{
					// Constraints with more roles sink to the bottom.
					int c1RoleCount = c1.RoleCollection.Count;
					int c2RoleCount = c2.RoleCollection.Count;
					if (c1RoleCount < c2RoleCount)
					{
						retVal = -1;
					}
					else if (c1RoleCount > c2RoleCount)
					{
						retVal = 1;
					}
				}

				return retVal;
			}
			/// <summary>
			/// Helper function for Compare to determine
			/// the relative order of different constraint types.
			/// </summary>
			/// <param name="constraintType">ConstraintType value</param>
			/// <returns>Relative numbers (the exact values should not matter).</returns>
			private static int RelativeSortPosition(ConstraintType constraintType)
			{
				int retVal = 0;
				switch (constraintType)
				{
					case ConstraintType.InternalUniqueness:
						retVal = 0;
						break;
					case ConstraintType.ExternalUniqueness:
					case ConstraintType.DisjunctiveMandatory:
					case ConstraintType.Ring:
					case ConstraintType.Equality:
					case ConstraintType.Exclusion:
					case ConstraintType.Frequency:
					case ConstraintType.SimpleMandatory:
					case ConstraintType.Subset:
					default:
						retVal = 1;
						break;
				}
				return retVal;
			}
			/// <summary>
			/// Is the constraint type ever visible to the ConstraintBox walking
			/// algorithm? A true return here does not guarantee that a specific constraint
			/// instance of this type is visible, only that constraints of this type can
			/// be visible.
			/// </summary>
			/// <param name="constraintType">ConstraintType value</param>
			/// <returns>true if the constraint can be drawn visibly</returns>
			private static bool IsConstraintTypeVisible(ConstraintType constraintType)
			{
				switch (constraintType)
				{
					case ConstraintType.InternalUniqueness:
						return true;
				}
				return false;
			}
			#endregion // Array sorting code
		}
		#endregion // ConstraintBox struct
		#region Pre-defined ConstraintBoxRoleActivity arrays
		// Used for the WalkConstraints method.  Having these static arrays is very
		// useful for saving time allocating arrays every time something is hit tested.
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for a fully-spanning uniqueness constraint.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_FullySpanning = new ConstraintBoxRoleActivity[0] {};
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the first role active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryLeft = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.NotInBox };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the second role active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryRight = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.NotInBox, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the first and second roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryLeft = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.NotInBox, ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the first and third roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryCenter = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the second and third roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryRight = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive };
		#endregion //Pre-defined ConstraintBoxRoleActivity arrays
		#region WalkConstraintBoxes implementation
		/// <summary>
		/// Do something within the bounds you're given.  This may include
		/// painting, hit testing, highlighting, etc.
		/// </summary>
		/// <param name="constraintBox">The constraint that is being described</param>
		/// <returns>bool</returns>
		protected delegate bool VisitConstraintBox(ref ConstraintBox constraintBox);

		/// <summary>
		/// Determines the bounding boxes of all the constraints associated with the FactType,
		/// then passes those bounding boxes into the delegate.  Specifically, it will pass in
		/// the bouding box, the number of roles in the box, a boolean[] telling the method
		/// which roles are active for the constraint, and the constraint type.
		/// </summary>
		/// <param name="parentShape">The FactTypeShape that the ConstraintShape is associated with.</param>
		/// <param name="shapeField">The ShapeField whose bounds define the space that the ConstraintBoxes will be built in.</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes produced by WalkConstraintBoxes.</param>
		protected static void WalkConstraintBoxes(ShapeElement parentShape, ShapeField shapeField, VisitConstraintBox boxUser)
		{
			WalkConstraintBoxes(parentShape, shapeField.GetBounds(parentShape), boxUser);
		}

		/// <summary>
		/// Determines the bounding boxes of all the constraints associated with the FactType,
		/// then passes those bounding boxes into the delegate.  Specifically, it will pass in
		/// the bouding box, the number of roles in the box, a boolean[] telling the method
		/// which roles are active for the constraint, and the constraint type.
		/// </summary>
		/// <param name="parentShape">The FactTypeShape that the ConstraintShape is associated with.</param>
		/// <param name="fullBounds">The bounds the rectangles need to fit in.  Pass RectangleD.Empty if unknown.</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes 
		/// produced by WalkConstraintBoxes.</param>
		protected static void WalkConstraintBoxes(ShapeElement parentShape, RectangleD fullBounds, VisitConstraintBox boxUser)
		{
			if (fullBounds.IsEmpty)
			{
				fullBounds = new RectangleD(0, 0, RoleBoxWidth, 0);
			}
			FactTypeShape parentFactTypeShape = parentShape as FactTypeShape;
			FactType parentFact = parentFactTypeShape.AssociatedFactType;
			RoleMoveableCollection factRoles = parentFact.RoleCollection;

			int factRoleCount = factRoles.Count;
			//RectangleD fullBounds = shapeField.GetBounds(parentShape);

			// First, gather the various constraints that are associated with the parent FactTypeShape.
			//
			ICollection<IFactConstraint> factConstraints = parentFact.FactConstraintCollection;
			int fullConstraintCount = factConstraints.Count;
			ConstraintBox[] constraintBoxes = new ConstraintBox[fullConstraintCount];

			if (fullConstraintCount != 0)
			{
				// Constraints hasn't been filled before it's used later in the code.
				int currentConstraintIndex = 0;
				foreach (IFactConstraint factConstraint in factConstraints)
				{
					IList<Role> constraintRoles = factConstraint.RoleCollection;
					int constraintRoleCount = constraintRoles.Count;
					#region Optimized ConstraintRoleBox assignments
					// Optimization time: If we're dealing with binary or ternary constraints,
					// use the pre-defined ConstraintBoxRoleActivity collections.  This saves
					// on allocating tons of arrays every time the constraints are drawn or hit tested.
					ConstraintBoxRoleActivity[] predefinedActivityRoles = null;
					if (constraintRoleCount == factRoleCount)
					{
						predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_FullySpanning;
					}
					else
					{
						switch (factRoleCount)
						{
							#region Binary fact type
							case 2:
								switch (constraintRoleCount)
								{
									case 1:
										int roleIndex = factRoles.IndexOf(constraintRoles[0]);
										Debug.Assert(roleIndex != -1); // This violates the IFactConstraint contract
										if (roleIndex == 0)
										{
											predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_BinaryLeft;
										}
										else if (roleIndex == 1)
										{
											predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_BinaryRight;
										}
										break;
								}
								break;
							#endregion // Binary fact type
							#region Ternary fact type
							case 3:
								switch (constraintRoleCount)
								{
									case 2:
										int roleIndex0 = factRoles.IndexOf(constraintRoles[0]);
										int roleIndex1 = factRoles.IndexOf(constraintRoles[1]);
										Debug.Assert(roleIndex0 != -1); // This violates the IFactConstraint contract
										Debug.Assert(roleIndex1 != -1); // This violates the IFactConstraint contract
										switch (roleIndex0)
										{
											case 0:
												if (roleIndex1 == 1)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryLeft;
												}
												else if (roleIndex1 == 2)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryCenter;
												}
												break;
											case 1:
												if (roleIndex1 == 0)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_BinaryLeft;
												}
												else if (roleIndex1 == 2)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryRight;
												}
												break;
											case 2:
												if (roleIndex1 == 0)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryCenter;
												}
												else if (roleIndex1 == 1)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryRight;
												}
												break;
										}
										break;
								}
								break;
							#endregion // Ternary fact type
						}
					}
					#endregion // Optimized ConstraintRoleBox assignments
					#region Manual ConstraintRoleBox assignment
					if (predefinedActivityRoles != null)
					{
						constraintBoxes[currentConstraintIndex] = new ConstraintBox(factConstraint, predefinedActivityRoles);
					}
					else
					{
						// The original code, now used for handling fact types with 4 or more roles
						// or fact types that are irregular. 
						ConstraintBox currentBox = new ConstraintBox(factConstraint, factRoleCount);

						// The constraint is not a fully-spanning constraint.  We must now
						// determine if the hole is between active roles.  This is important
						// mainly for drawing, to determine if a dashed line needs to be drawn
						// to connect the solid lines over the active roles of the constraint.
						Debug.Assert(constraintRoleCount < factRoleCount); // Should be predefined otherwise
						ConstraintBoxRoleActivity[] activeRoles = currentBox.ActiveRoles;
						Debug.Assert(activeRoles.Length == factRoleCount);
						// Walk the fact's roles, and for each role that is found in this constraint
						// mark the role as active in the constraintBox.roleActive array.  
						for (int i = 0; i < constraintRoleCount; ++i)
						{
							int roleIndex = factRoles.IndexOf(constraintRoles[i]);
							Debug.Assert(roleIndex != -1); // This violates the IFactConstraint contract
							activeRoles[roleIndex] = ConstraintBoxRoleActivity.Active;
						}
						if (factRoleCount == 2)
						{
							for (int i = 0; i < factRoleCount; ++i)
							{
								if (activeRoles[i] != ConstraintBoxRoleActivity.Active)
								{
									activeRoles[i] = ConstraintBoxRoleActivity.NotInBox;
								}
							}
						}
						constraintBoxes[currentConstraintIndex] = currentBox;
					}
					#endregion // Manual ConstraintRoleBox assignment
					++currentConstraintIndex;
				}
				int significantConstraintCount = ConstraintBox.OrderConstraintBoxes(constraintBoxes);

				double constraintHeight = ConstraintHeight;
				double constraintWidth = fullBounds.Width / (double)factRoleCount;

				// Walk the constraintBoxes array and assign a physical location to each constraint box,
				fullBounds.Height = constraintHeight;
				int heightLeft = 0;
				int heightRight = 0;
				int lastUncompressedConstraint = 0;
				double yPosition = fullBounds.Y;
				double xPosition = fullBounds.X;
				double initialBottom = fullBounds.Bottom;
				double offsetHeight = (parentFactTypeShape.ConstraintDisplayPosition == ConstraintDisplayPosition.Top) ? constraintHeight : -constraintHeight;
				#region Compressing the ConstraintRoleBoxes of binary fact types.
				if (factRoleCount == 2)
				{
					for (int i = significantConstraintCount - 1; i >= 0; --i)
					{
						ConstraintBox box = constraintBoxes[i];
						box.Bounds = fullBounds;
						RectangleD bounds = box.Bounds;

						ConstraintBoxRoleActivity[] activeRoles = box.ActiveRoles;
						if (activeRoles.Length == 2)
						{
							if (activeRoles[0] == ConstraintBoxRoleActivity.NotInBox)
							{
								bounds.X = bounds.X + constraintWidth;
								bounds.Width = bounds.Width - constraintWidth;

								if (heightLeft > 0)
								{
									bounds.Y = initialBottom - ((double)lastUncompressedConstraint * constraintHeight);
									--heightLeft;
								}
								else if (heightRight++ == 0)
								{
									lastUncompressedConstraint = i;
								}
							}
							else if (activeRoles[1] == ConstraintBoxRoleActivity.NotInBox)
							{
								bounds.Width = bounds.Width - constraintWidth;

								if (heightRight > 0)
								{
									bounds.Y = initialBottom - ((double)lastUncompressedConstraint * constraintHeight);
									--heightRight;
								}
								else if (heightLeft++ == 0)
								{
									lastUncompressedConstraint = i;
								}
							}
						}
						box.Bounds = bounds;
						if (!boxUser(ref box))
						{
							break;
						}
						fullBounds.Offset(0, offsetHeight);
					}
				}
				#endregion // Compressing the ConstraintRoleBoxes of binary fact types.
				// Unaries, ternaries and n-aries do not need to have 
				// their internal uniqueness constraints compressed.
				else
				{
					for (int i = significantConstraintCount - 1; i >= 0; --i)
					{
						ConstraintBox box = constraintBoxes[i];
						box.Bounds = fullBounds;
						if (!boxUser(ref box))
						{
							break;
						}
						fullBounds.Offset(0, offsetHeight);
					}
				}
			}
		}
		#endregion // WalkConstraintBoxes implementation
		#region Size Constants
		private const double RoleBoxHeight = 0.11;
		private const double RoleBoxWidth = 0.16;
		private const double NestedFactHorizontalMargin = 0.2;
		private const double NestedFactVerticalMargin = 0.075;
		private const double ConstraintHeight = 0.07;
		#endregion // Size Constants
		#region SpacerShapeField : ShapeField
		/// <summary>
		/// Creates a shape to properly align the other shapefields within the FactTypeShape.
		/// </summary>
		private class SpacerShapeField : ShapeField
		{
			/// <summary>
			/// Construct a default SpacerShapeField
			/// </summary>
			public SpacerShapeField()
			{
				DefaultFocusable = false;
				DefaultSelectable = false;
				DefaultVisibility = false;
			}

			/// <summary>
			/// Width is that of NestedFactHorizontalMargin if parentShape is objectified; otherwise, zero.
			/// </summary>
			/// <returns>NestedFactHorizontalMargin if objectified; otherwise, 0.</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				FactTypeShape factShape = parentShape as FactTypeShape;
				if (factShape.IsObjectified)
					return NestedFactHorizontalMargin;
				else
					return 0;
			}

			/// <summary>
			/// Width is that of NestedFactVerticalMargin if parentShape is objectified; otherwise, zero.
			/// </summary>
			/// <returns>NestedFactVerticalMargin if objectified; otherwise, 0.</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				FactTypeShape factShape = parentShape as FactTypeShape;
				if (factShape.IsObjectified)
				{
					return NestedFactVerticalMargin;
				}
				else
				{
					// UNDONE: At the moment the pen width is a constant
					// value, so this should just return a constant.
					StyleSet styleSet = parentShape.StyleSet;
					Pen pen = styleSet.GetPen(InternalFactConstraintPen);
					return (Double)(pen.Width / 2);
				}
			}

			// Nothing to paint for the spacer. So, no DoPaint override needed.

		}
		#endregion // SpacerShapeField class
		#region SpacerSubField class
		private class SpacerSubField : ShapeSubField
		{
			#region Member Variables
			private Role myAssociatedRole;
			#endregion // Member Variables
			#region Construction
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="associatedRole">The role that this SpacerSubField is associated with.</param>
			public SpacerSubField(Role associatedRole)
			{
				Debug.Assert(associatedRole != null);
				myAssociatedRole = associatedRole;
			}
			#endregion // Construction
			#region Required ShapeSubField overrides
			/// <summary>
			/// Returns true if the fields have the same associated role
			/// </summary>
			public override bool SubFieldEquals(object obj)
			{
				SpacerSubField compareTo;
				if (null != (compareTo = obj as SpacerSubField))
				{
					return myAssociatedRole == compareTo.myAssociatedRole;
				}
				return false;
			}
			/// <summary>
			/// Returns the hash code for the associated role
			/// </summary>
			public override int SubFieldHashCode
			{
				get
				{
					return myAssociatedRole.GetHashCode();
				}
			}
			/// <summary>
			/// A spacer sub field is never selectable, return false regardless of parameters
			/// </summary>
			/// <returns>false</returns>
			public override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return false;
			}
			/// <summary>
			/// A spacer sub field is never focusable, return false regardless of parameters
			/// </summary>
			/// <returns>false</returns>
			public override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
			{
				return false;
			}
			/// <summary>
			/// Returns bounds based on the size of the parent shape
			/// and the NestedFactVerticalMargin
			/// </summary>
			/// <param name="parentShape">The containing FactTypeShape</param>
			/// <param name="parentField">The containing shape field</param>
			/// <returns>The vertical slice for this role</returns>
			public override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				RectangleD retVal = parentField.GetBounds(parentShape);
				retVal.Height = NestedFactVerticalMargin;
				return retVal;
			}
			#endregion // Required ShapeSubField overrides
		}
		#endregion // SpacerSubField class
		#region ConstraintShapeField : ShapeField
		private class ConstraintShapeField : ShapeField
		{
			private ConstraintDisplayPosition myConstraintPosition;

			/// <summary>
			/// Construct a default ConstraintShapeField
			/// </summary>
			/// <param name="constraintPosition">Describes the position of this constraint field in relation to the role box(es).</param>
			public ConstraintShapeField(ConstraintDisplayPosition constraintPosition)
			{
				DefaultFocusable = true;
				DefaultSelectable = true;
				DefaultVisibility = true;
				myConstraintPosition = constraintPosition;
			}

			/// <summary>
			/// Checks if constraint field is visible.
			/// </summary>
			/// <param name="parentShape">The parent FactTypeShape.</param>
			/// <returns>True if the constraint position of this ConstraintShapeField matches the selected constraint position of the FactTypeShape.</returns>
			public override bool GetVisible(ShapeElement parentShape)
			{
				FactTypeShape factTypeShape = parentShape as FactTypeShape;
				return factTypeShape.ConstraintDisplayPosition == myConstraintPosition;
			}


			/// <summary>
			/// Find the constraint sub shape at this location
			/// </summary>
			/// <param name="point">The point being hit-tested.</param>
			/// <param name="parentShape">The current ShapeField that the mouse is over.</param>
			/// <param name="diagramHitTestInfo">The DiagramHitTestInfo to which the ConstraintSubShapField
			/// will be added if the mouse is over it.</param>
			public override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				ForHitTest hitTest = new ForHitTest(point, parentShape, this, diagramHitTestInfo);
				FactTypeShape.WalkConstraintBoxes(parentShape, this, hitTest.TestForHit);
			}

			/// <summary>
			/// Handles hit test of the constraint
			/// </summary>
			private class ForHitTest
			{
				private PointD myPoint;
				private ShapeElement myShapeElement;
				private ConstraintShapeField myConstraintShapeField;
				private DiagramHitTestInfo myDiagramHitTestInfo;

				public ForHitTest(PointD point, ShapeElement parentShape, ConstraintShapeField shapeField, DiagramHitTestInfo diagramHitTestInfo)
				{
					myPoint = point;
					myShapeElement = parentShape;
					myConstraintShapeField = shapeField;
					myDiagramHitTestInfo = diagramHitTestInfo;
				}

				/// <summary>
				/// Tests if a specific constraint is at this location.
				/// </summary>
				/// <param name="constraintBox">The constraint to look for</param>
				/// <returns>true</returns>
				public bool TestForHit(ref ConstraintBox constraintBox)
				{
					RectangleD fullBounds = constraintBox.Bounds;
					if (fullBounds.Contains(myPoint))
					{
						IFactConstraint factConstraint = constraintBox.FactConstraint;
						myDiagramHitTestInfo.HitDiagramItem = new DiagramItem(myShapeElement, myConstraintShapeField, new ConstraintSubField(factConstraint.Constraint));
						return false; // Don't continue, we got our item
					}
					return true;
				}
			}

			/// <summary>
			/// Get the minimum width of the ConstraintShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape that this ConstraintShapeField is associated with.</param>
			/// <returns>The width of the ConstraintShapeField.</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				FactTypeShape parent = parentShape as FactTypeShape;
				return parent.RolesShape.GetMinimumWidth(parentShape);
			}

			/// <summary>
			/// Get the minimum height of the ConstraintShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape that this ConstraintShapeField is associated with.</param>
			/// <returns>The height of the ConstraintShapeField.</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				FactTypeShape parent = parentShape as FactTypeShape;
				if (parent.ConstraintDisplayPosition != myConstraintPosition)
				{
					return 0;
				}

				return ForMinimumHeight.CalculateMinimumHeight(parentShape);
			}

			/// <summary>
			/// Helper class for GetMinimumHeight.
			/// </summary>
			private class ForMinimumHeight
			{
				private double minY = double.MaxValue;
				private double maxY = double.MinValue;
				private bool wasVisited = false;

				private ForMinimumHeight() { }
				public static double CalculateMinimumHeight(ShapeElement parentShape)
				{
					ForMinimumHeight fmh = new ForMinimumHeight();
					WalkConstraintBoxes(parentShape, RectangleD.Empty, fmh.VisitBox);
					return fmh.wasVisited ? fmh.maxY - fmh.minY : 0;
				}
				private bool VisitBox(ref ConstraintBox constraintBox)
				{
					wasVisited = true;
					RectangleD bounds = constraintBox.Bounds;
					minY = Math.Min(minY, bounds.Top);
					maxY = Math.Max(maxY, bounds.Bottom);
					return true;
				}
			}

			/// <summary>
			/// Paints the contstraints.
			/// </summary>
			/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
			/// <param name="parentShape">ConstraintShapeField to draw to.</param>
			public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				ForDrawing draw = new ForDrawing(e, parentShape as FactTypeShape);
				FactTypeShape.WalkConstraintBoxes(parentShape, this, draw.DrawConstraint);

//				//TODO: remove the following code after testing
//				Graphics g = e.Graphics;
//				System.Drawing.Brush myBrush = new System.Drawing.SolidBrush(Color.FromArgb(23, 255, 0, 0));
//				g.FillRectangle(myBrush, RectangleD.ToRectangleF(this.GetBounds(parentShape)));
			}

			/// <summary>
			/// Helper class for DoPaint().  Handles drawing of the constraint.
			/// </summary>
			private class ForDrawing
			{
				private Graphics myGraphics;
				private HighlightedShapesCollection highlightedShapes;
				private FactTypeShape myShapeElement;
				private float myGap;
				private Pen myConstraintPen;
				private Pen myDashedConstraintPen;
				private Brush myHighlightBrush;

				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
				/// <param name="parentShape">ConstraintShapeField to draw to.</param>
				public ForDrawing(DiagramPaintEventArgs e, FactTypeShape parentShape)
				{
					myGraphics = e.Graphics;
					highlightedShapes = e.View.HighlightedShapes;
					myShapeElement = parentShape;

					StyleSet styleSet = myShapeElement.StyleSet;
					myConstraintPen = styleSet.GetPen(InternalFactConstraintPen);
					myDashedConstraintPen = styleSet.GetPen(InternalFactConstraintSpacerPen);
					myHighlightBrush = styleSet.GetBrush(DiagramBrushes.ShapeBackgroundSelectedInactive);
					myGap = myConstraintPen.Width;
				}

				/// <summary>
				/// Does the actual drawing of a constraint.
				/// </summary>
				/// <param name="constraintBox">The constraint to draw.</param>
				/// <returns>False if constraint is not an internal uniqueness constraint; otherwise, true.</returns>
				public bool DrawConstraint(ref ConstraintBox constraintBox)
				{
					if (constraintBox.ConstraintType != ConstraintType.InternalUniqueness)
					{
						return false;
					}

					//variables
					RectangleF boundsF = RectangleD.ToRectangleF(constraintBox.Bounds);
					float verticalPos = boundsF.Top + (float)(ConstraintHeight / 2);
					ConstraintBoxRoleActivity[] rolePosToDraw = constraintBox.ActiveRoles;
					int numRoles = rolePosToDraw.Length;
					float roleWidth = (float)FactTypeShape.RoleBoxWidth;

					// test for and draw highlights
					foreach (DiagramItem item in highlightedShapes)
					{
						if (object.ReferenceEquals(myShapeElement, item.Shape))
						{
							ConstraintSubField highlightedSubField = item.SubField as ConstraintSubField;
							IFactConstraint factConstraint = constraintBox.FactConstraint;
							if (highlightedSubField != null && (highlightedSubField.AssociatedConstraint == factConstraint.Constraint))
							{
								// draw highlight
								myGraphics.FillRectangle(myHighlightBrush, boundsF);
								break;
							}
						}
					}

					float startPos = boundsF.Left, endPos = startPos;
					if (constraintBox.IsSpanning)
					{
						endPos = boundsF.Right;
						//draw fully spanning constraint
						if (myShapeElement.ShouldDrawConstraintPreferred(constraintBox.FactConstraint.Constraint))
						{
							//draw constraint as preferred
							DrawPreferredConstraintLine(myGraphics, myConstraintPen, startPos, endPos, verticalPos);
						}
						else
						{
							//draw constraint as regular
							DrawConstraintLine(myGraphics, myConstraintPen, startPos, endPos, verticalPos);
						}
					}
					else
					{

						for (int i = 0; i < numRoles; ++i)
						{
							ConstraintBoxRoleActivity currentBoxActivity = rolePosToDraw[i];
							if (currentBoxActivity != ConstraintBoxRoleActivity.NotInBox)
							{
								endPos += roleWidth;
								IConstraint currentConstraint = constraintBox.FactConstraint.Constraint;
								if (currentBoxActivity != ConstraintBoxRoleActivity.Active)
								{
									if (!(i == 0 || i == numRoles - 1))
									{
										// position that is not first or last is being skipped,
										// draw dashed line
										if (myShapeElement.ShouldDrawConstraintPreferred(currentConstraint))
										{
											//draw constraint as preferred
											DrawPreferredConstraintLine(myGraphics, myDashedConstraintPen, startPos, endPos, verticalPos);
										}
										else
										{
											//draw constraint as regular
											DrawConstraintLine(myGraphics, myDashedConstraintPen, startPos, endPos, verticalPos);
										}
									}
									startPos = endPos;
								}
								if (startPos != endPos && !(i < numRoles - 1 && (rolePosToDraw[i + 1] == ConstraintBoxRoleActivity.Active)))
								{
									//draw constraint
									if (myShapeElement.ShouldDrawConstraintPreferred(currentConstraint))
									{
										//draw constraint as preferred
										DrawPreferredConstraintLine(myGraphics, myConstraintPen, startPos, endPos, verticalPos);
									}
									else
									{
										//draw constraint as regular
										DrawConstraintLine(myGraphics, myConstraintPen, startPos, endPos, verticalPos);
									}
									startPos = endPos;
								}
							}
						}
					}
					return true;
				}

				/// <summary>
				/// Draws a preferred constraint line
				/// </summary>
				/// <param name="g">The graphics object to draw to</param>
				/// <param name="pen">The pen to use</param>
				/// <param name="startPos">The x-coordinate of the left edge to draw at.</param>
				/// <param name="endPos">The x-coordinate of the right edge to draw at.</param>
				/// <param name="verticalPos">The y-coordinate to draw at.</param>
				private void DrawPreferredConstraintLine(Graphics g, Pen pen, float startPos, float endPos, float verticalPos)
				{
					float gap = myGap;
					float vAdjust = gap * .75f;
					g.DrawLine(pen, startPos + gap, verticalPos - vAdjust, endPos - gap, verticalPos - vAdjust);
					g.DrawLine(pen, startPos + gap, verticalPos + vAdjust, endPos - gap, verticalPos + vAdjust);
				}

				/// <summary>
				/// Draws a regular constraint line
				/// </summary>
				/// <param name="g">The graphics object to draw to</param>
				/// <param name="pen">The pen to use</param>
				/// <param name="startPos">The x-coordinate of the left edge to draw at.</param>
				/// <param name="endPos">The x-coordinate of the right edge to draw at.</param>
				/// <param name="verticalPos">The y-coordinate to draw at.</param>
				private void DrawConstraintLine(Graphics g, Pen pen, float startPos, float endPos, float verticalPos)
				{
					float gap = myGap;
					g.DrawLine(pen, startPos + gap, verticalPos, endPos - gap, verticalPos);
				}
			}
		}
		#endregion // ConstraintShapeField class
		#region ConstraintSubField class
		private class ConstraintSubField : ShapeSubField
		{
			#region Member variables
			private IConstraint myAssociatedConstraint;
			#endregion // Member variables
			#region Construction
			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="associatedConstraint">The Constraint that this ConstraintSubfield will represent.</param>
			public ConstraintSubField(IConstraint associatedConstraint)
			{
				Debug.Assert(associatedConstraint != null);
				myAssociatedConstraint = associatedConstraint;
			}
			#endregion // Construction
			#region Required ShapeSubField overrides
			/// <summary>
			/// Returns true if the fields have the same associated role
			/// </summary>
			public override bool SubFieldEquals(object obj)
			{
				ConstraintSubField compareTo;
				if (null != (compareTo = obj as ConstraintSubField))
				{
					return myAssociatedConstraint == compareTo.myAssociatedConstraint;
				}
				return false;
			}
			/// <summary>
			/// Returns the hash code for the associated role
			/// </summary>
			public override int SubFieldHashCode
			{
				get
				{
					return myAssociatedConstraint.GetHashCode();
				}
			}
			/// <summary>
			/// A role sub field is always selectable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// A role sub field is always focusable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// Returns bounds based on the size of the parent shape
			/// and the RoleIndex of this shape
			/// </summary>
			/// <param name="parentShape">The containing FactTypeShape</param>
			/// <param name="parentField">The containing shape field</param>
			/// <returns>The vertical slice for this role</returns>
			public override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				return parentField.GetBounds(parentShape);
			}
			#endregion // Required ShapeSubField
			#region Accessor functions
			/// <summary>
			/// Get the Constraint element associated with this sub field
			/// </summary>
			public IConstraint AssociatedConstraint
			{
				get
				{
					return myAssociatedConstraint;
				}
			}
			#endregion // Accessor functions
		}
		#endregion // ConstraintSubField class
		#region RolesShapeField class
		private class RolesShapeField : ShapeField
		{
			/// <summary>
			/// Construct a default RolesShapeField (Visible, but not selectable or focusable)
			/// </summary>
			public RolesShapeField()
			{
				DefaultFocusable = false;
				DefaultSelectable = false;
				DefaultVisibility = true;
			}
			/// <summary>
			/// Find the role sub shape at this location
			/// </summary>
			/// <param name="point"></param>
			/// <param name="parentShape"></param>
			/// <param name="diagramHitTestInfo"></param>
			public override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				RectangleD fullBounds = GetBounds(parentShape);
				if (fullBounds.Contains(point))
				{
					FactType factType = (parentShape as FactTypeShape).AssociatedFactType;
					RoleMoveableCollection roles = factType.RoleCollection;
					int roleCount = roles.Count;
					if (roleCount != 0)
					{
						int roleIndex = Math.Min((int)((point.X - fullBounds.Left) * roleCount / fullBounds.Width), roleCount - 1);
						diagramHitTestInfo.HitDiagramItem = new DiagramItem(parentShape, this, new RoleSubField(roles[roleIndex]));
					}
				}
			}
			/// <summary>
			/// Get the minimum width of this RolesShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape associated with this RolesShapeField.</param>
			/// <returns>The width of this RolesShapeField.</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				return FactTypeShape.RoleBoxWidth * Math.Max(1, (parentShape as FactTypeShape).AssociatedFactType.RoleCollection.Count);
			}
			/// <summary>
			/// Get the minimum height of this RolesShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape associated with this RolesShapeField.</param>
			/// <returns>The height of this RolesShapeField.</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				return FactTypeShape.RoleBoxHeight;
			}
			/// <summary>
			/// Paint the RolesShapeField
			/// </summary>
			/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
			/// <param name="parentShape">FactTypeShape to draw to.</param>
			public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				FactTypeShape parentFactShape = parentShape as FactTypeShape;
				FactType factType = parentFactShape.AssociatedFactType;
				RoleMoveableCollection roles = factType.RoleCollection;
				int roleCount = roles.Count;
				bool objectified = factType.NestingType != null;
				if (roleCount > 0 || objectified)
				{
					int highlightRoleBox = -1;
					foreach (DiagramItem item in e.View.HighlightedShapes)
					{
						if (object.ReferenceEquals(parentShape, item.Shape))
						{
							RoleSubField roleField = item.SubField as RoleSubField;
							if (roleField != null)
							{
								highlightRoleBox = roleField.RoleIndex;
								break;
							}
						}
					}
					RectangleD bounds = GetBounds(parentShape);
					Graphics g = e.Graphics;
					double offsetBy = bounds.Width / roleCount;
					float offsetByF = (float)offsetBy;
					double lastX = bounds.Left;
					StyleSet styleSet = parentShape.StyleSet;
					Pen pen = styleSet.GetPen(FactTypeShape.RoleBoxOutlinePen);
					int activeRoleIndex;
					float top = (float)bounds.Top;
					float bottom = (float)bounds.Bottom;
					float height = (float)bounds.Height;
					ExternalConstraintConnectAction activeAction = ActiveExternalConstraintConnectAction;
					StringFormat stringFormat = null;
					try
					{
						for (int i = 0; i < roleCount; ++i)
						{
							float lastXF = (float)lastX;
							if ((activeAction != null) &&
								(-1 != (activeRoleIndex = activeAction.GetActiveRoleIndex(roles[i]))))
							{
								g.FillRectangle(styleSet.GetBrush((i == highlightRoleBox) ? SelectedConstraintRoleHighlightedBackgroundBrush : SelectedConstraintRoleBackgroundBrush), lastXF, top, offsetByF, height);
								if (stringFormat == null)
								{
									stringFormat = new StringFormat();
									stringFormat.LineAlignment = StringAlignment.Center;
									stringFormat.Alignment = StringAlignment.Center;
								}
								g.DrawString((activeRoleIndex + 1).ToString(), styleSet.GetFont(DiagramFonts.CommentText), styleSet.GetBrush(DiagramBrushes.CommentText), new RectangleF(lastXF, top, offsetByF, height), stringFormat);
							}
							else if (i == highlightRoleBox)
							{
								// UNDONE: The highlighted background for a full shape is drawn by adjusting
								// the luminosity. MDF modifies luminosity automatically when a color is in
								// place by directly editing the pen/brush color, then restoring it. However,
								// there is no way to get to this facility when HasHighlighting is turned off,
								// so matching the color is difficult. Turning HasHightlighting on would mean
								// the entire shape would draw highlighted, and we would have to explicitly
								// un-highlight n-1 role boxes, which would be extremely flashy. We should
								// also use this facility to adjust the color for the selected constraint so we
								// would not need to use a separate brush for the normal/highlight colors.
								// Could use a brush with an alpha channel:
								// System.Drawing.Brush myBrush = new System.Drawing.SolidBrush(Color.FromArgb(23, 0, 0, 0));
								g.FillRectangle(styleSet.GetBrush(DiagramBrushes.ShapeBackgroundSelectedInactive), lastXF, top, offsetByF, height);
							}

							// Draw the line between the role boxes
							if (i != 0)
							{
								g.DrawLine(pen, lastXF, top, lastXF, bottom);
							}
							lastX += offsetBy;
						}
					}
					finally
					{
						if (stringFormat != null)
						{
							stringFormat.Dispose();
						}
					}
					RectangleF boundsF = RectangleD.ToRectangleF(bounds);
					g.DrawRectangle(pen, boundsF.Left, boundsF.Top, boundsF.Width, boundsF.Height);
				}
			}
		}
		#endregion // RolesShapeField class
		#region RoleSubField class
		private class RoleSubField : ShapeSubField
		{
			#region Member variables
			private Role myAssociatedRole;
			#endregion // Member variables
			#region Construction
			public RoleSubField(Role associatedRole)
			{
				Debug.Assert(associatedRole != null);
				myAssociatedRole = associatedRole;
			}
			#endregion // Construction
			#region Required ShapeSubField overrides
			/// <summary>
			/// Returns true if the fields have the same associated role
			/// </summary>
			public override bool SubFieldEquals(object obj)
			{
				RoleSubField compareTo;
				if (null != (compareTo = obj as RoleSubField))
				{
					return myAssociatedRole == compareTo.myAssociatedRole;
				}
				return false;
			}
			/// <summary>
			/// Returns the hash code for the associated role
			/// </summary>
			public override int SubFieldHashCode
			{
				get
				{
					return myAssociatedRole.GetHashCode();
				}
			}
			/// <summary>
			/// A role sub field is always selectable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// A role sub field is always focusable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// Returns bounds based on the size of the parent shape
			/// and the RoleIndex of this shape
			/// </summary>
			/// <param name="parentShape">The containing FactTypeShape</param>
			/// <param name="parentField">The containing shape field</param>
			/// <returns>The vertical slice for this role</returns>
			public override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				RectangleD retVal = parentField.GetBounds(parentShape);
				RoleMoveableCollection roles = myAssociatedRole.FactType.RoleCollection;
				retVal.Width /= roles.Count;
				int roleIndex = roles.IndexOf(myAssociatedRole);
				if (roleIndex > 0)
				{
					retVal.Offset(roleIndex * retVal.Width, 0);
				}
				return retVal;
			}
			#endregion // Required ShapeSubField
			#region DragDrop support
			public override MouseAction GetPotentialMouseAction(MouseButtons mouseButtons, PointD point, DiagramHitTestInfo hitTestInfo)
			{
				if (mouseButtons == MouseButtons.Left)
				{
					return ((ORMDiagram)hitTestInfo.DiagramClientView.Diagram).RoleDragPendingAction;
				}
				return base.GetPotentialMouseAction(mouseButtons, point, hitTestInfo);
			}
			#endregion // DragDrop support
			#region Accessor functions
			/// <summary>
			/// Get the Role element associated with this sub field
			/// </summary>
			public Role AssociatedRole
			{
				get
				{
					return myAssociatedRole;
				}
			}
			/// <summary>
			/// Returns the index of the associated Role element in its
			/// containing collection.
			/// </summary>
			public int RoleIndex
			{
				get
				{
					Debug.Assert(myAssociatedRole != null && !myAssociatedRole.IsRemoved);
					return myAssociatedRole.FactType.RoleCollection.IndexOf(myAssociatedRole);
				}
			}
			#endregion // Accessor functions
		}
		#endregion // RoleSubField class
		#region Member Variables
		private static RolesShapeField myRolesShapeField = null;
		private static ConstraintShapeField myTopConstraintShapeField = null;
		private static ConstraintShapeField myBottomConstraintShapeField = null;
		private static readonly StyleSetResourceId RoleBoxOutlinePen = new StyleSetResourceId("Northface", "RoleBoxOutlinePen");
		private static readonly StyleSetResourceId SelectedConstraintRoleBackgroundBrush = new StyleSetResourceId("Northface", "SelectedConstraintRoleBackgroundBrush");
		private static readonly StyleSetResourceId SelectedConstraintRoleHighlightedBackgroundBrush = new StyleSetResourceId("Northface", "SelectedConstraintRoleHighlightedBackgroundBrush");
		private static readonly StyleSetResourceId InternalFactConstraintPen = new StyleSetResourceId("Northface", "InternalFactConstraintPen");
		private static readonly StyleSetResourceId InternalFactConstraintSpacerPen = new StyleSetResourceId("Northface", "InternalFactConstraintSpacerPen");
		private static ExternalConstraintConnectAction myActiveExternalConstraintConnectAction;
		#endregion // Member Variables
		#region RoleSubField integration
		/// <summary>
		/// Get the role corresponding to the given subField
		/// </summary>
		/// <param name="shapeField">The containing shape field (will always be the RolesShapeField)</param>
		/// <param name="subField">A RoleSubField</param>
		/// <returns>A Role element</returns>
		public override ICollection GetSubFieldRepresentedElements(ShapeField shapeField, ShapeSubField subField)
		{
			RoleSubField roleField;
			if (null != (roleField = subField as RoleSubField))
			{
				return new ModelElement[] { roleField.AssociatedRole };
			}
			ConstraintSubField constraintSubField;
			if (null != (constraintSubField = subField as ConstraintSubField))
			{
				return new ModelElement[] { (ModelElement)constraintSubField.AssociatedConstraint };
			}
			return null;
		}
		/// <summary>
		/// The roles shape field is the default and only shape field inside
		/// a FactType shape.
		/// </summary>
		public override ShapeField DefaultShapeField
		{
			get
			{
				return myRolesShapeField;
			}
		}
		#endregion // RoleSubField integration
		#region Customize appearance
		/// <summary>
		/// Set to true. Enables role highlighting
		/// </summary>
		public override bool HasSubFieldHighlighting
		{
			get
			{
				return true;
			}
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
		/// Change the outline pen to a thin black line for all instances
		/// of this shape.
		/// </summary>
		/// <param name="classStyleSet">The style set to modify</param>
		protected override void InitializeResources(StyleSet classStyleSet)
		{
			PenSettings penSettings = new PenSettings();
			penSettings.Color = SystemColors.WindowText;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.AddPen(RoleBoxOutlinePen, DiagramPens.ShapeOutline, penSettings);

			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = Color.Yellow;
			classStyleSet.AddBrush(SelectedConstraintRoleBackgroundBrush, DiagramBrushes.DiagramBackground, brushSettings);
			brushSettings.Color = Color.Gold;
			classStyleSet.AddBrush(SelectedConstraintRoleHighlightedBackgroundBrush, DiagramBrushes.DiagramBackground, brushSettings);

			penSettings.Color = Color.Violet;
			classStyleSet.AddPen(InternalFactConstraintPen, DiagramPens.ShapeOutline, penSettings);

			penSettings.DashStyle = DashStyle.Dash;
			classStyleSet.AddPen(InternalFactConstraintSpacerPen, DiagramPens.ShapeOutline, penSettings);

		}
		/// <summary>
		/// Use the rolebox outline pen unless we're objectified
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				return IsObjectified ? DiagramPens.ShapeOutline : RoleBoxOutlinePen;
			}
		}
		/// <summary>
		/// Create our one placeholder shape field, which fills the whole shape
		/// and contains our role boxes.
		/// </summary>
		/// <param name="shapeFields">Per-class collection of shape fields</param>
		protected override void InitializeShapeFields(ShapeFieldCollection shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize fields
			RolesShapeField field = new RolesShapeField();
			ConstraintShapeField topConstraintField = new ConstraintShapeField(ConstraintDisplayPosition.Top);
			ConstraintShapeField bottomConstraintField = new ConstraintShapeField(ConstraintDisplayPosition.Bottom);
			SpacerShapeField spacer = new SpacerShapeField();

			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(spacer);
			shapeFields.Add(topConstraintField);
			shapeFields.Add(bottomConstraintField);
			shapeFields.Add(field);

			// Modify anchoring behavior
			AnchoringBehavior bottomConstraintAnchor = bottomConstraintField.AnchoringBehavior;
			bottomConstraintAnchor.CenterHorizontally();
			bottomConstraintAnchor.SetTopAnchor(field, 1);

			AnchoringBehavior anchor = field.AnchoringBehavior;
			anchor.CenterHorizontally();
			anchor.SetTopAnchor(topConstraintField, 1);

			AnchoringBehavior topConstraintAnchor = topConstraintField.AnchoringBehavior;
			topConstraintAnchor.CenterHorizontally();
			topConstraintAnchor.SetTopAnchor(spacer, 1);

			AnchoringBehavior spacerAnchor = spacer.AnchoringBehavior;
			spacerAnchor.CenterHorizontally();

			// Do not modify set edge anchors in this case. Edge anchors
			// force the bounds of the text field to the size of the parent,
			// we want it the other way around.

			Debug.Assert(myRolesShapeField == null); // Only called once
			myRolesShapeField = field;

			Debug.Assert(myTopConstraintShapeField == null); // Only called once
			myTopConstraintShapeField = topConstraintField;

			Debug.Assert(myBottomConstraintShapeField == null); // Only called once
			myBottomConstraintShapeField = bottomConstraintField;
		}
		/// <summary>
		/// The shape field used to display roles
		/// </summary>
		protected ShapeField RolesShape
		{
			get
			{
				return myRolesShapeField;
			}
		}
		/// <summary>
		/// Highlight region surrounding the roles box if
		/// it is objectified
		/// </summary>
		/// <value>True if the fact type is nested</value>
		public override bool HasHighlighting
		{
			get
			{
				return IsObjectified;
			}
		}
		/// <summary>
		/// Show an outline around the fact type only
		/// if it is objectified.
		/// </summary>
		/// <value>True if the fact type is nested</value>
		public override bool HasOutline
		{
			get
			{
				return IsObjectified;
			}
		}
		/// <summary>
		/// Set the content size of the FactTypeShape
		/// </summary>
		protected override SizeD ContentSize
		{
			get
			{
				// Margin is used to adjust the width and height of the content to incorporate the
				// width of the pen being used and prevent the pen from being cropped at the edges
				// of the content.
				double margin = this.StyleSet.GetPen(FactTypeShape.RoleBoxOutlinePen).Width;
				SizeD retVal = SizeD.Empty;
				ShapeField rolesShape = RolesShape;
				if (rolesShape != null)
				{
					double width, height;
					width = rolesShape.GetMinimumWidth(this) + margin;
					height = rolesShape.GetMinimumHeight(this) + margin;
					if (IsObjectified)
					{
						height += myTopConstraintShapeField.GetMinimumHeight(this) + myBottomConstraintShapeField.GetMinimumHeight(this);
					}
					else
					{
						if (this.ConstraintDisplayPosition == ConstraintDisplayPosition.Top)
						{
							height += myTopConstraintShapeField.GetMinimumHeight(this);
						}
						else
						{
							height += myBottomConstraintShapeField.GetMinimumHeight(this);
						}
					}
					retVal = new SizeD(width, height);
				}
				return retVal;
			}
		}
		/// <summary>
		/// Size to ContentSize plus some margin padding if we're a nested fact type.
		/// </summary>
		public override void AutoResize()
		{
			SizeD contentSize = ContentSize;
			if (!contentSize.IsEmpty)
			{
				if (IsObjectified)
				{
					contentSize.Width += NestedFactHorizontalMargin + NestedFactHorizontalMargin;
					contentSize.Height += NestedFactVerticalMargin + NestedFactVerticalMargin;
				}
			}
			Size = contentSize;
		}
		/// <summary>
		/// Called during a transaction when a new constraint
		/// is added or removed that is associated with this fact.
		/// </summary>
		/// <param name="constraint">The newly added or removed constraint</param>
		public void ConstraintSetChanged(IConstraint constraint)
		{
			Debug.Assert(Store.TransactionManager.InTransaction);
			bool resize = false;
			switch (constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
					resize = true;
					break;
			}
			if (resize)
			{
				AutoResize();
				Invalidate(true);
			}
		}
		/// <summary>
		/// Return different shapes for objectified versus non-objectified fact types.
		/// The actual shape is controlled by the tools options page.
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				// If the fact is objectified, get the current setting from the options
				// page for how to draw the shape
				if (IsObjectified)
				{
					ShapeGeometry useShape;
					switch (Shell.OptionsPage.CurrentObjectifiedFactShape)
					{
						case Shell.ObjectifiedFactShape.HardRectangle:
							useShape = ShapeGeometries.Rectangle;
							break;
						case Shell.ObjectifiedFactShape.SoftRectangle:
						default:
							useShape = ShapeGeometries.RoundedRectangle;
							break;
					}
					return useShape;
				}
				else
				{
					// Just draw a rectangle if the fact IS NOT objectified
					return ShapeGeometries.Rectangle;
				}
			}
		}
		/// <summary>
		/// Add a shape element linked to this parent to display the name
		/// of the objectifying type
		/// </summary>
		/// <param name="element">ModelElement of type ObjectType</param>
		/// <returns>true</returns>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
			Debug.Assert(
					(element is ObjectType && ((ObjectType)element).NestedFactType == AssociatedFactType)
					|| (element is ReadingOrder && ((ReadingOrder)element).FactType == AssociatedFactType)
				);
			return true;
		}
		/// <summary>
		/// An object type is displayed as an ObjectTypeShape unless it is
		/// objectified, in which case we display it as an ObjectifiedFactTypeNameShape
		/// </summary>
		/// <param name="element">The element to test. Expecting an ObjectType.</param>
		/// <param name="shapeTypes">The choice of shape types</param>
		/// <returns></returns>
		protected override MetaClassInfo ChooseShape(ModelElement element, IList shapeTypes)
		{
			Guid classId = element.MetaClassId;
			if (classId == ObjectType.MetaClassGuid)
			{
				return ORMDiagram.ChooseShapeTypeForObjectType((ObjectType)element, shapeTypes);
			}
			Debug.Assert(false); // We're only expecting an ObjectType here
			return base.ChooseShape(element, shapeTypes);
		}
		/// <summary>
		/// Make an ObjectifiedFactTypeNameShape a relative child element
		/// </summary>
		/// <param name="childShape"></param>
		/// <returns></returns>
		protected override RelationshipType ChooseRelationship(ShapeElement childShape)
		{
			Debug.Assert(childShape is ObjectifiedFactTypeNameShape || childShape is ReadingShape);
			return RelationshipType.Relative;
		}
		#endregion // Customize appearance
		#region Customize property display
		#region Reusable helper class for custom property descriptor creation
		/// <summary>
		/// A helper class to enable an object to be displayed as expandable,
		/// and have one string attribute specified as an editable string.
		/// </summary>
		private abstract class ExpandableStringConverter : ExpandableObjectConverter
		{
			/// <summary>
			/// Allow conversion from a string
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="sourceType">Type</param>
			/// <returns>true for a string type</returns>
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string);
			}
			/// <summary>
			/// Allow conversion to a string. Note that the base class
			/// handles the ConvertTo function for us.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="destinationType">Type</param>
			/// <returns>true for a stirng type</returns>
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(string);
			}
			/// <summary>
			/// Convert from a string to the specified string
			/// meta attribute on the context element.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <param name="culture">CultureInfo</param>
			/// <param name="value">New value for the attribute</param>
			/// <returns>context.Instance for a string value, defers to base otherwise</returns>
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string stringValue = value as string;
				if (stringValue != null)
				{
					object instance = context.Instance;
					ModelElement element = ConvertContextToElement(context);
					if (element != null)
					{
						MetaAttributeInfo attrInfo = element.Store.MetaDataDirectory.FindMetaAttribute(PrimaryStringAttributeId);
						// This will recurse when the property descriptor is changed because the
						// transaction close will refresh the property browser. Make sure we don't
						// fire a second SetValue here so we only get one item on the undo stack.
						if (stringValue != (string)element.GetAttributeValue(attrInfo))
						{
							// We want exactly the same result as achieved by setting
							// the property directly in the property grid, so create a property
							// descriptor to do the actual work of setting the property inside
							// a transaction.
							element.CreatePropertyDescriptor(attrInfo, element).SetValue(element, stringValue);
						}
					}
					return instance;
				}
				else
				{
					return base.ConvertFrom(context, culture, value);
				}
			}
			/// <summary>
			/// Override to retrieve the ModelElement to modify from the context
			/// information.
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns>ModelElement</returns>
			protected abstract ModelElement ConvertContextToElement(ITypeDescriptorContext context);
			/// <summary>
			/// Override to specify the string property to represent
			/// as the string value for the object. Defaults to
			/// NamedElement.NameMetaAttributeGuid.
			/// </summary>
			/// <value></value>
			protected virtual Guid PrimaryStringAttributeId
			{
				get
				{
					return NamedElement.NameMetaAttributeGuid;
				}
			}
		}
		/// <summary>
		/// A property descriptor implementation to
		/// use a ModelElement as an attribute
		/// in the property grid. Use with a realized
		/// ExpandableStringConverter instance to create
		/// an expandable property with an editable text field.
		/// </summary>
		private class HeaderDescriptor : PropertyDescriptor
		{
			private ModelElement myWrappedElement;
			private TypeConverter myConverter;
			/// <summary>
			/// Create a descriptor for the specified element and
			/// type converter.
			/// </summary>
			/// <param name="wrapElement">ModelElement</param>
			/// <param name="converter">TypeConverter (can be null)</param>
			public HeaderDescriptor(ModelElement wrapElement, TypeConverter converter) : base(wrapElement.GetComponentName(), new Attribute[]{})
			{
				myWrappedElement = wrapElement;
				myConverter = converter;
			}
			/// <summary>
			/// Return the converter specified in the constructor
			/// </summary>
			public override TypeConverter Converter
			{
				get
				{
					return myConverter;
				}
			}
			/// <summary>
			/// Use the underlying class name as the display name
			/// </summary>
			public override string DisplayName
			{
				get { return myWrappedElement.GetClassName(); }
			}
			/// <summary>
			/// Return this object as the component type
			/// </summary>
			public override Type ComponentType
			{
				get { return typeof(HeaderDescriptor); }
			}
			/// <summary>
			/// Returns false
			/// </summary>
			public override bool IsReadOnly
			{
				get { return false; }
			}
			/// <summary>
			/// Specify the type of the wrapped element
			/// as the PropertyType
			/// </summary>
			public override Type PropertyType
			{
				get { return myWrappedElement.GetType(); }
			}
			/// <summary>
			/// Disallow resetting the value
			/// </summary>
			/// <param name="component">object</param>
			/// <returns>false</returns>
			public override bool CanResetValue(object component)
			{
				return false;
			}
			/// <summary>
			/// Return the wrapped element as the property value
			/// </summary>
			/// <param name="component">object (ignored)</param>
			/// <returns>wrapElement value specified in constructor</returns>
			public override object GetValue(object component)
			{
				return myWrappedElement;
			}
			/// <summary>
			/// Do not serialize
			/// </summary>
			/// <param name="component"></param>
			/// <returns></returns>
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}
			/// <summary>
			/// Do not reset
			/// </summary>
			/// <param name="component"></param>
			public override void ResetValue(object component)
			{
			}
			/// <summary>
			/// Do nothing. All value setting in this case
			/// is done by the type converter.
			/// </summary>
			/// <param name="component">object</param>
			/// <param name="value">object</param>
			public override void SetValue(object component, object value)
			{
			}
		}
		#endregion //Reusable helper class for custom property descriptor creation
		#region Nested FactType-specific type converters
		/// <summary>
		/// A type converter for showing the raw fact type
		/// as an expandable property in a nested fact type.
		/// </summary>
		private class ObjectifiedFactPropertyConverter : ExpandableStringConverter
		{
			public static readonly TypeConverter Converter = new ObjectifiedFactPropertyConverter();
			private ObjectifiedFactPropertyConverter() { }
			/// <summary>
			/// Convert from a FactTypeShape to a FactType
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns></returns>
			protected override ModelElement ConvertContextToElement(ITypeDescriptorContext context)
			{
				FactTypeShape shape = context.Instance as FactTypeShape;
				FactType factType;
				if (null != (shape = context.Instance as FactTypeShape) &&
					null != (factType = shape.AssociatedFactType))
				{
					return factType;
				}
				return null;
			}
		}
		/// <summary>
		/// A type converter for showing the nesting type
		/// as an expandable property in a nested fact type.
		/// </summary>
		private class ObjectifyingEntityTypePropertyConverter : ExpandableStringConverter
		{
			public static readonly TypeConverter Converter = new ObjectifyingEntityTypePropertyConverter();
			private ObjectifyingEntityTypePropertyConverter() { }
			/// <summary>
			/// Convert from a FactTypeShape to the nesting EntityType
			/// </summary>
			/// <param name="context">ITypeDescriptorContext</param>
			/// <returns></returns>
			protected override ModelElement ConvertContextToElement(ITypeDescriptorContext context)
			{
				FactTypeShape shape = context.Instance as FactTypeShape;
				FactType factType;
				if (null != (shape = context.Instance as FactTypeShape) &&
					null != (factType = shape.AssociatedFactType))
				{
					return factType.NestingType;
				}
				return null;
			}
		}
		#endregion // Nested FactType-specific type converters
		/// <summary>
		/// Show selected properties from the nesting type and the
		/// fact type for an objectified type, as well as expandable
		/// nodes for each of the underlying instances.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			FactType factType = AssociatedFactType;
			ObjectType nestingType = (factType == null) ? null : factType.NestingType;
			if (nestingType != null)
			{
				MetaDataDirectory metaDir = factType.Store.MetaDataDirectory;
				return new PropertyDescriptorCollection(new PropertyDescriptor[]{
					this.CreatePropertyDescriptor(metaDir.FindMetaAttribute(FactTypeShape.ConstraintDisplayPositionMetaAttributeGuid), this),
					nestingType.CreatePropertyDescriptor(metaDir.FindMetaAttribute(NamedElement.NameMetaAttributeGuid), nestingType),
					nestingType.CreatePropertyDescriptor(metaDir.FindMetaAttribute(ObjectType.IsIndependentMetaAttributeGuid), nestingType),
					new HeaderDescriptor(factType, ObjectifiedFactPropertyConverter.Converter),
					new HeaderDescriptor(nestingType, ObjectifyingEntityTypePropertyConverter.Converter),
					});
			}
			else
			{
				return base.GetProperties(attributes);
			}
		}
		#endregion // Customize property display
		#region Customize connection points
		/// <summary>
		/// Enable custom connection points
		/// </summary>
		/// <value>true</value>
		public override bool HasConnectionPoints
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Determine the best connection point for a link
		/// attached to a role in this fact type.
		/// </summary>
		/// <param name="link"></param>
		public override void CreateConnectionPoint(LinkShape link)
		{
			RolePlayerLink roleLink;
			if (null != (roleLink = link as RolePlayerLink))
			{
				// Extract basic information from the shape
				NodeShape objShape = roleLink.ToShape;
				if (objShape is FactTypeShape)
				{
					// Objectified end of relationship
					base.CreateConnectionPoint(link);
					return;
				}
				Debug.Assert((FactTypeShape)roleLink.FromShape == this);
				ObjectTypePlaysRole rolePlayerLink = roleLink.AssociatedRolePlayerLink;
				Role role = rolePlayerLink.PlayedRoleCollection;
				RoleMoveableCollection roles = AssociatedFactType.RoleCollection;
				int roleCount = roles.Count;
				int roleIndex = roles.IndexOf(role);

				PointD objCenter = objShape.AbsoluteCenter;
				RectangleD factBox = myRolesShapeField.GetBounds(this); // This finds the role box for both objectified and simple fact types
				factBox.Offset(AbsoluteBoundingBox.Location);

				// Decide whether top or bottom works best
				double finalY;
				if (Math.Abs(objCenter.Y - factBox.Top) <= Math.Abs(objCenter.Y - factBox.Bottom))
				{
					finalY = factBox.Top;
				}
				else
				{
					finalY = factBox.Bottom;
				}

				// If we're the first or last (or both) role, then
				// prefer an edge attach point.

				double finalX = factBox.Left + (factBox.Width / roleCount) * (roleIndex + .5);
				// UNDONE: Finish this code when connection points are more reliable
//				if (roleCount == 1)
//				{
//				}
//				else if (roleIndex == 0)
//				{
//				}
//				else if (roleIndex == roleCount - 1)
//				{
//				}
				base.CreateConnectionPoint(new PointD(finalX, finalY), link);
				return;
			}
			base.CreateConnectionPoint(link);
		}
		/// <summary>
		/// Set the connection point to the middle of the object
		/// for when we're objectified. This is consistent with the
		/// ObjectTypeShape implementation.
		/// </summary>
		/// <value></value>
		protected override PointD ConnectionPoint
		{
			get
			{
				RectangleD bounds = AbsoluteBounds;
				return new PointD(bounds.X + bounds.Width / 2, bounds.Top + bounds.Height / 2);
			}
		}
		#endregion // Customize connection points
		#region FactTypeShape specific
		/// <summary>
		/// Get the FactType associated with this shape
		/// </summary>
		public FactType AssociatedFactType
		{
			get
			{
				return ModelElement as FactType;
			}
		}
		/// <summary>
		/// Return true if the associated fact type is an objectified fact
		/// </summary>
		public bool IsObjectified
		{
			get
			{
				FactType factType = AssociatedFactType;
				return (factType == null) ? false : (factType.NestingType != null);
			}
		}
		/// <summary>
		/// Static property set when an external constraint is being created. The active
		/// connection is used to track which roles are highlighted.
		/// </summary>
		public static ExternalConstraintConnectAction ActiveExternalConstraintConnectAction
		{
			get
			{
				return myActiveExternalConstraintConnectAction;
			}
			set
			{
				myActiveExternalConstraintConnectAction = value;
			}
		}
		/// <summary>
		/// The core shape model only draws preferred constraints
		/// for the conceptual preferred identifier concept. This does
		/// not include concepts such as the relational multi-column primary
		/// key, so (for example), there is no way to make a spanning constraint
		/// primary in the core model. Override this function in a derived model
		/// to represented a primary identifier as a preferred constraint.
		/// </summary>
		/// <param name="constraint">Any constraint. In the core model, only uniqueness
		/// constraints will be preferred</param>
		/// <returns>true if the PreferredIdentifierFor property on the role is not null.</returns>
		protected virtual bool ShouldDrawConstraintPreferred(IConstraint constraint)
		{
			ConstraintRoleSequence sequence = constraint as ConstraintRoleSequence;
			return (sequence != null) ? (sequence.PreferredIdentifierFor != null) : false;
		}
		#endregion // FactTypeShape specific
		#region Shape display update rules
		[RuleOn(typeof(NestingEntityTypeHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchToNestedFact : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				NestingEntityTypeHasFactType link = e.ModelElement as NestingEntityTypeHasFactType;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				// Part1: Make sure the fact shape is visible on any diagram where the
				// corresponding nestingType is displayed
				foreach (object obj in nestingType.AssociatedPresentationElements)
				{
					ObjectTypeShape objectShape = obj as ObjectTypeShape;
					if (objectShape != null)
					{
						ORMDiagram currentDiagram = objectShape.Diagram as ORMDiagram;
						NodeShape factShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						if (factShape == null)
						{
							Diagram.FixUpDiagram(currentDiagram.ModelElement, nestedFactType);
							factShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						}
						if (factShape != null)
						{
							factShape.Location = objectShape.Location;
						}
					}
				}

				// Part2: Move any links from the object type to the fact type
				foreach (ObjectTypePlaysRole modelLink in nestingType.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid))
				{
					foreach (object obj in modelLink.PresentationRolePlayers)
					{
						RolePlayerLink rolePlayer = obj as RolePlayerLink;
						if (rolePlayer != null)
						{
							ORMDiagram currentDiagram = rolePlayer.Diagram as ORMDiagram;
							NodeShape factShape = currentDiagram.FindShapeForElement(nestedFactType) as NodeShape;
							if (factShape != null)
							{
								rolePlayer.ToShape = factShape;
							}
							else
							{
								// Backup. Should only happen if the FixupDiagram call in part 1
								// did not add the fact type.
								rolePlayer.Remove();
							}
						}
					}
				}

				// Part3: Remove object type shapes from the diagram. Do this before
				// adding the labels to the objectified fact types so clearing the role
				// players doesn't blow the labels away. Also, FixUpDiagram will attempt
				// to fix up the existing shapes instead of creating new ones if the existing
				// ones are not cleared away.
				nestingType.PresentationRolePlayers.Clear();

				// Part4: Resize the fact type wherever it is displayed and add the
				// labels for the fact type display.
				foreach (object obj in nestedFactType.AssociatedPresentationElements)
				{
					FactTypeShape shape = obj as FactTypeShape;
					if (shape != null)
					{
						shape.AutoResize();
						Diagram.FixUpDiagram(nestedFactType, nestingType);
					}
				}
			}
		}
		[RuleOn(typeof(NestingEntityTypeHasFactType), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchFromNestedFact : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				NestingEntityTypeHasFactType link = e.ModelElement as NestingEntityTypeHasFactType;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				// Part1: Remove any existing presentation elements for the object type.
				// This removes all of the ObjectifiedTypeNameShape objects
				nestingType.PresentationRolePlayers.Clear();

				// Part2: Resize the fact type wherever it is displayed, and make sure
				// the object type is made visible in the same location.
				foreach (object obj in nestedFactType.AssociatedPresentationElements)
				{
					FactTypeShape factShape = obj as FactTypeShape;
					if (factShape != null)
					{
						factShape.AutoResize();
						ORMDiagram currentDiagram = factShape.Diagram as ORMDiagram;
						NodeShape objectShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						if (objectShape == null)
						{
							Diagram.FixUpDiagram(nestingType.Model, nestingType);
							objectShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
						}
						if (objectShape != null)
						{
							PointD location = factShape.Location;
							location.Offset(0.0, 2 * factShape.Size.Height);
							objectShape.Location = location;
						}
					}
				}

				// Part3: Move any links from the fact type to the object type
				foreach (ObjectTypePlaysRole modelLink in nestingType.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid))
				{
					foreach (RolePlayerLink rolePlayer in modelLink.PresentationRolePlayers)
					{
						NodeShape objShape = (rolePlayer.Diagram as ORMDiagram).FindShapeForElement(nestingType) as NodeShape;
						if (objShape != null)
						{
							rolePlayer.ToShape = objShape;
						}
						else
						{
							rolePlayer.Remove();
						}
					}
				}
			}
		}
		#region ConstraintDisplayPositionChangeRule class
		[RuleOn(typeof(FactTypeShape))]
		private class ConstraintDisplayPositionChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == ConstraintDisplayPositionMetaAttributeGuid) // InternalUniquenessConstraint.IsPreferredMetaAttributeGuid)
				{
					FactTypeShape factTypeShape = e.ModelElement as FactTypeShape; //InternalUniquenessConstraint;
					if (!factTypeShape.IsRemoved)
					{
						factTypeShape.AutoResize();
						factTypeShape.Invalidate(true);
					}
				}
			}
		}
		#endregion // ConstraintDisplayPositionChangeRule class
		#endregion // Shape display update rules
	}
	#endregion // FactTypeShape class
	#region ObjectifiedFactTypeNameShape class
	/// <summary>
	/// A specialized display of the nesting type as a relative
	/// child element of an objectified fact type
	/// </summary>
	public partial class ObjectifiedFactTypeNameShape
	{
		private static AutoSizeTextField myTextShapeField;
		/// <summary>
		/// Associate the text box with the object type name
		/// </summary>
		protected override Guid AssociatedShapeMetaAttributeGuid
		{
			get { return ObjectTypeNameMetaAttributeGuid; }
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
		public ObjectType AssociatedObjectType
		{
			get
			{
				return ModelElement as ObjectType;
			}
		}
		/// <summary>
		/// Move the name label above the parent fact type shape
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
	}
	#endregion // ObjectifiedFactTypeNameShape class
}