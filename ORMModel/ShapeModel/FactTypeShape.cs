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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.ShapeModel
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
	#region DisplayRoleNames enum
	/// <summary>
	/// Determines whether RoleNameShapes will be
	/// drawn for this fact, overrides global settings
	/// </summary>
	[CLSCompliant(true)]
	public enum DisplayRoleNames
	{
		/// <summary>
		/// Use the global setting
		/// </summary>
		UserDefault,
		/// <summary>
		/// Draw the RoleNameShape for the fact 
		/// </summary>
		On,
		/// <summary>
		/// Do not draw RoleNameShapes for the fact
		/// </summary>
		Off,
	}
	#endregion // DisplayRoleNames
	#region FactTypeShape class
	public partial class FactTypeShape : ICustomShapeFolding, IModelErrorActivation
	{
		#region Public token values
		/// <summary>
		/// A key to set in the top-level transaction context to indicate the role that
		/// a newly added role should be added after.
		/// </summary>
		public static readonly object InsertAfterRoleKey = new object();
		/// <summary>
		/// A key to set in the top-level transaction context to indicate the role that
		/// a newly added role should be added before.
		/// </summary>
		public static readonly object InsertBeforeRoleKey = new object();
		#endregion // Public token values
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
			/// True if the box is explicitly hidden 
			/// </summary>
			private bool myIsHidden;
			/// <summary>
			/// The cached role collection
			/// </summary>
			private IList<Role> myRoleCollection;
			private bool? myIsValid;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="factConstraint">A reference to the original constraint that this ConstraintBox is based on.</param>
			/// <param name="uniqueConstraintRoles">Unique constraint roles. factConstraint.RoleCollection can contain duplicate roles
			/// and must be used with extreme caution.</param>
			/// <param name="factRoleCount">The number of roles for the context fact.</param>
			public ConstraintBox(IFactConstraint factConstraint, IList<Role> uniqueConstraintRoles, int factRoleCount)
			{
				Debug.Assert(factConstraint != null);
				Debug.Assert(uniqueConstraintRoles != null);
				Debug.Assert(factRoleCount > 0 && factRoleCount >= uniqueConstraintRoles.Count);
				myBounds = new RectangleD();
				IConstraint constraint = factConstraint.Constraint;
				myConstraintType = constraint.ConstraintType;
				myActiveRoles = new ConstraintBoxRoleActivity[factRoleCount];
				myFactConstraint = factConstraint;
				myRoleCollection = uniqueConstraintRoles;
				myIsHidden = false;
				myIsValid = null;
			}
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="factConstraint">A reference to the original constraint that this ConstraintBox is based on.</param>
			/// <param name="uniqueConstraintRoles">Unique constraint roles. factConstraint.RoleCollection can contain duplicate roles
			/// and must be used with extreme caution.</param>
			/// <param name="roleActivity">A representation of the factConstraint's role activity within the fact.</param>
			public ConstraintBox(IFactConstraint factConstraint, IList<Role> uniqueConstraintRoles, ConstraintBoxRoleActivity[] roleActivity)
			{
				Debug.Assert(factConstraint != null);
				Debug.Assert(uniqueConstraintRoles != null);
				Debug.Assert(roleActivity != null);
				if (!object.ReferenceEquals(roleActivity, PreDefinedConstraintBoxRoleActivities_FullySpanning) && !object.ReferenceEquals(roleActivity, PreDefinedConstraintBoxRoleActivities_AntiSpanning))
				{
					int roleActivityCount = roleActivity.Length;
					Debug.Assert(roleActivityCount > 0 && roleActivityCount >= uniqueConstraintRoles.Count);
					myBounds = new RectangleD();
				}
				else
				{
					myBounds = new RectangleD();
				}
				IConstraint constraint = factConstraint.Constraint;
				myConstraintType = constraint.ConstraintType;
				myActiveRoles = roleActivity;
				myFactConstraint = factConstraint;
				myRoleCollection = uniqueConstraintRoles;
				myIsHidden = false;
				myIsValid = null;
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
			/// on the fact for which this constraint applies.
			/// </summary>
			public ConstraintBoxRoleActivity[] ActiveRoles
			{
				get
				{
					return myActiveRoles;
				}
			}
			/// <summary>
			/// If a binary ActiveRoles set is marked as Active/Inactive or
			/// Inactive/Active, then change the Inactive to NotInBox
			/// </summary>
			public void CompressBinaryActiveRoles()
			{
				ConstraintBoxRoleActivity[] roles = myActiveRoles;
				if (roles.Length == 2)
				{
					if (roles[0] == ConstraintBoxRoleActivity.Inactive)
					{
						myActiveRoles = PreDefinedConstraintBoxRoleActivities_BinaryRightCompressed;
					}
					else
					{
						Debug.Assert(roles[1] == ConstraintBoxRoleActivity.Inactive); // Spanning or anti-spanning otherwise
						myActiveRoles = PreDefinedConstraintBoxRoleActivities_BinaryLeftCompressed;
					}
				}
			}
			/// <summary>
			/// The constraint object this box is for.
			/// </summary>
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
			public IList<Role> RoleCollection
			{
				get
				{
					return myRoleCollection;
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
					return object.ReferenceEquals(myActiveRoles, PreDefinedConstraintBoxRoleActivities_FullySpanning);
				}
			}
			/// <summary>
			/// Tests if this constraint is undefined (AntiSpanning).
			/// </summary>
			/// <value>True if the constraint is undefined.</value>
			public bool IsAntiSpanning
			{
				get
				{
					return object.ReferenceEquals(myActiveRoles, PreDefinedConstraintBoxRoleActivities_AntiSpanning);
				}
			}
			/// <summary>
			/// True if the box is explicitly hidden 
			/// </summary>
			public bool IsHidden
			{
				get
				{
					return myIsHidden;
				}
				set
				{
					myIsHidden = value;
				}
			}
			/// <summary>
			/// Tests if this constraint is valid in combination with the other existing constraints on the fact type.
			/// </summary>
			/// <value>True if the constraint is valid.</value>
			public bool IsValid
			{
				get
				{
					if (!myIsValid.HasValue)
					{
						bool retVal = true;
						if (IsAntiSpanning)
						{
							retVal = false;
						}
						else
						{
							IModelErrorOwner errorOwner = myFactConstraint.Constraint as IModelErrorOwner;
							if (errorOwner != null)
							{
								// UNDONE: ModelErrorUses filter
								using (IEnumerator<ModelErrorUsage> errors = errorOwner.GetErrorCollection(ModelErrorUses.None).GetEnumerator())
								{
									retVal = !errors.MoveNext();
								}
							}
						}
						myIsValid = retVal;
						return retVal;
					}
					return myIsValid.Value;
				}
			}
			#endregion // Accessor Properties
			#region Array sorting code
			/// <summary>
			/// Order the constraint boxes into their respective groups,
			/// with internal followed by external. This allows for group
			/// analysis and hiding of individual boxes prior to a full sort.
			/// </summary>
			/// <param name="boxes"></param>
			/// <returns></returns>
			public static int GroupConstraintBoxes(ConstraintBox[] boxes)
			{
				Array.Sort(
					boxes,
					delegate(ConstraintBox c1, ConstraintBox c2)
					{
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
						return retVal;
					});
				return CalculateSignificantCount(boxes, boxes.Length);
			}
			private static int CalculateSignificantCount(ConstraintBox[] boxes, int fullCount)
			{
				int significantBoxCount = 0;
				int i;
				for (i = 0; i < fullCount; ++i)
				{
					if (!IsConstraintTypeVisible(boxes[i].ConstraintType) || boxes[i].IsHidden)
					{
						// All insignificant ones are sorted to the end
						significantBoxCount = i;
						break;
					}
				}
				if (i == fullCount)
				{
					significantBoxCount = fullCount;
				}
				return significantBoxCount;
			}
			/// <summary>
			/// Sort the constraint boxes and place non-displayed constraints
			/// at the end of the array. Return the number of boxes that
			/// actually need displaying.
			/// </summary>
			/// <param name="boxes">An existing array of constraint boxes
			/// created with the parametrized constructor</param>
			/// <param name="fullCount">The number of boxes to use. Elements with index >= fullCount
			/// are ignored.</param>
			/// <returns>The number of significant boxes</returns>
			public static int OrderConstraintBoxes(ConstraintBox[] boxes, int fullCount)
			{
				Array.Sort(
					boxes,
					//0,
					//fullCount, // UNDONE: This is dumb. Sort should have an overload that takes a Comparer<T> with index, count. Check in Beta2
					delegate(ConstraintBox c1, ConstraintBox c2)
					{
						if (object.ReferenceEquals(c1.FactConstraint, c2.FactConstraint))
						{
							// Same object
							return 0;
						}

						// Order the constraints by IsHidden, ConstraintType, RoleCount
						int retVal = 0;

						if (c1.IsHidden)
						{
							if (!c2.IsHidden)
							{
								retVal = 1;
							}
						}
						else if (c2.IsHidden)
						{
							retVal = -1;
						}
						else
						{
							ConstraintType ct1 = c1.ConstraintType;
							ConstraintType ct2 = c2.ConstraintType;
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
								// Constraints with less roles sink to the bottom.
								int c1RoleCount = c1.RoleCollection.Count;
								int c2RoleCount = c2.RoleCollection.Count;
								if (c1RoleCount < c2RoleCount)
								{
									retVal = 1;
								}
								else if (c1RoleCount > c2RoleCount)
								{
									retVal = -1;
								}
							}
						}

						return retVal;
					});
				return CalculateSignificantCount(boxes, fullCount);
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
					case ConstraintType.Exclusion:
					case ConstraintType.Equality:
					case ConstraintType.Subset:
						retVal = 1;
						break;
					case ConstraintType.Frequency:
					case ConstraintType.SimpleMandatory:
					default:
						retVal = 2;
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
					case ConstraintType.ExternalUniqueness:
					case ConstraintType.Equality:
					case ConstraintType.DisjunctiveMandatory:
					case ConstraintType.Ring:
					case ConstraintType.Exclusion:
					case ConstraintType.Subset:
					case ConstraintType.Frequency:
						return true;
				}
				return false;
			}
			#endregion // Array sorting code
		}
			#region Move Roles
			/// <summary>
			/// Moves the display order of a role to the left in a fact type shape.
			/// </summary>
			/// <param name="roleToMove">The role to move to the left.</param>
			/// <returns>True if it actually moved the role.</returns>
			public bool MoveRoleLeft(Role roleToMove)
			{
				return MoveRole(true, roleToMove);
			}
			/// <summary>
			/// Moves the display order of a role to the right in a fact type shape.
			/// </summary>
			/// <param name="roleToMove">The role to move to the right.</param>
			/// <returns>True if it actually moved the role.</returns>
			public bool MoveRoleRight(Role roleToMove)
			{
				return MoveRole(false, roleToMove);
			}
			private bool MoveRole(bool movingLeft, Role roleToMove)
			{
				bool moveOccured = false;
				using (Transaction t = Store.TransactionManager.BeginTransaction(ResourceStrings.MoveRoleOrderTransactionName))
				{
					RoleMoveableCollection roles = EnsureDisplayOrderCollection();
					int index = roles.IndexOf(roleToMove);
					if (index != 0 && movingLeft)
					{
						roles.Move(index, index - 1);
					}
					else if (index < roles.Count - 1 && !movingLeft)
					{
						roles.Move(index, index + 1);
					}

					if (t.HasPendingChanges)
					{
						t.Commit();
						moveOccured = true;
					}
				}
				return moveOccured;
			}
			private RoleMoveableCollection EnsureDisplayOrderCollection()
			{
				RoleMoveableCollection displayRoles = RoleDisplayOrderCollection;
				if (displayRoles.Count == 0)
				{
					FactType fact = AssociatedFactType;
					if (fact != null)
					{
						RoleMoveableCollection nativeRoles = fact.RoleCollection;
						int nativeRoleCount = nativeRoles.Count;
						for (int i = 0; i < nativeRoleCount; ++i)
						{
							displayRoles.Add(nativeRoles[i]);
						}
					}
				}
				return displayRoles;
			}
			/// <summary>
			/// Gets the currently displayed order of the roles in the fact type.
			/// If there is not a custom display order then it will return the default
			/// role collection.
			/// </summary>
			public RoleMoveableCollection DisplayedRoleOrder
			{
				get
				{
					RoleMoveableCollection alternateOrder = RoleDisplayOrderCollection;
					return (alternateOrder.Count == 0) ? AssociatedFactType.RoleCollection : alternateOrder;
				}
			}
			/// <summary>
			/// Gets the reading order that matches the currently displayed order of the
			/// fact that is passed in.
			/// </summary>
			/// <returns>The matching ReadingOrder or null if one does not exist.</returns>
			public static ReadingOrder FindMatchingReadingOrder(FactTypeShape theFact)
			{
				RoleMoveableCollection factRoles = theFact.DisplayedRoleOrder;
				Role[] roleOrder = new Role[factRoles.Count];
				factRoles.CopyTo(roleOrder, 0);
				return FactType.FindMatchingReadingOrder(theFact.AssociatedFactType, roleOrder);
			}
			#region RoleDisplayOrderChanged class
			[RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
			private class RoleDisplayOrderChanged : RolePlayerPositionChangeRule
			{
				public override void RolePlayerPositionChanged(RolePlayerOrderChangedEventArgs e)
				{
					Role role;
					if (null != (role = e.CounterpartRolePlayer as Role))
					{
						foreach (PresentationElement pElem in role.FactType.PresentationRolePlayers)
						{
							FactTypeShape factShape;
							if (null != (factShape = pElem as FactTypeShape))
							{
								foreach (LinkConnectsToNode connection in factShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
								{
									BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
									if (binaryLink != null)
									{
										binaryLink.RipUp();
									}
								}

								factShape.InvalidateRequired(true);
							}
						}
					}
				}
			}
			#endregion // RoleDisplayOrderChanged class
			#endregion // Move Roles
		#endregion // ConstraintBox struct
		#region Pre-defined ConstraintBoxRoleActivity arrays
		// Used for the WalkConstraints method.  Having these static arrays is very
		// useful for saving time allocating arrays every time something is hit tested.
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for a fully-spanning uniqueness constraint.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_FullySpanning = new ConstraintBoxRoleActivity[0] { };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an undefined uniqueness constraint.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_AntiSpanning = new ConstraintBoxRoleActivity[0] { };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the first role active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryLeft = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the first role active, compressed alongside a BinaryRightCompressed.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryLeftCompressed = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.NotInBox };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the second role active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryRight = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 binary fact with the second role active, compressed alongside a BinaryLeftCompressed.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_BinaryRightCompressed = new ConstraintBoxRoleActivity[2] { ConstraintBoxRoleActivity.NotInBox, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the first and second roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryLeftCenter = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the first and third roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryLeftRight = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an n-1 ternary fact with the second and third roles active.
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_TernaryCenterRight = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Active };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an ternary facts with first role only
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_Left = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Inactive };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an ternary facts with second role only
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_Center = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active, ConstraintBoxRoleActivity.Inactive };
		/// <summary>
		/// A ConstraintBoxRoleActivity[] for an ternary facts with third role only
		/// </summary>
		private static readonly ConstraintBoxRoleActivity[] PreDefinedConstraintBoxRoleActivities_Right = new ConstraintBoxRoleActivity[3] { ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Inactive, ConstraintBoxRoleActivity.Active };
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
		/// the bounding box, the number of roles in the box, a ConstraintRoleBoxActivity[] telling the method
		/// which roles are active for the constraint, and the constraint type.
		/// </summary>
		/// <param name="shapeField">The ShapeField whose bounds define the space that the ConstraintBoxes will be built in.</param>
		/// <param name="displayPosition">The position the constraint will be displayed in</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes produced by WalkConstraintBoxes.</param>
		protected void WalkConstraintBoxes(ShapeField shapeField, ConstraintDisplayPosition displayPosition, VisitConstraintBox boxUser)
		{
			WalkConstraintBoxes(shapeField.GetBounds(this), displayPosition, boxUser);
		}

		/// <summary>
		/// Determines the bounding boxes of all the constraints associated with the FactType,
		/// then passes those bounding boxes into the delegate.  Specifically, it will pass in
		/// the bouding box, the number of roles in the box, a boolean[] telling the method
		/// which roles are active for the constraint, and the constraint type.
		/// </summary>
		/// <param name="fullBounds">The bounds the rectangles need to fit in.  Pass RectangleD.Empty if unknown.</param>
		/// <param name="displayPosition">The position the constraint will be displayed in</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes 
		/// produced by WalkConstraintBoxes.</param>
		protected void WalkConstraintBoxes(RectangleD fullBounds, ConstraintDisplayPosition displayPosition, VisitConstraintBox boxUser)
		{
			// initialize variables
			FactType parentFact = AssociatedFactType;
			RoleMoveableCollection factRoles = DisplayedRoleOrder;
			int factRoleCount = factRoles.Count;
			if (fullBounds.IsEmpty)
			{
				fullBounds = new RectangleD(0, 0, RoleBoxWidth, 0);
			}
			else
			{
				fullBounds.Inflate(StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width / -2, 0d);
			}

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
					if (factConstraint.Constraint.ConstraintStorageStyle == ConstraintStorageStyle.MultiColumnExternalConstraint)
					{
						// The constraint can have multiple role sequences, and there is nothing stopping
						// them from overlapping. Although this is a pathological state, it is a valid model
						// and needs to draw without crashing.
						int duplicateCount = 0;
						for (int i = constraintRoleCount - 1; i > 0; --i)
						{
							Role testRole = constraintRoles[i];
							for (int j = 0; j < i; ++j)
							{
								if (object.ReferenceEquals(constraintRoles[j], testRole))
								{
									++duplicateCount;
									break;
								}
							}
						}
						if (duplicateCount != 0)
						{
							int lastItem = constraintRoleCount - duplicateCount;
							Role[] uniqueConstraintRoles = new Role[lastItem];
							uniqueConstraintRoles[0] = constraintRoles[0];
							if (lastItem > 1)
							{
								for (int i = constraintRoleCount - 1; i > 0; --i)
								{
									Role testRole = constraintRoles[i];
									int j = 0;
									for (; j < i; ++j)
									{
										if (object.ReferenceEquals(constraintRoles[j], testRole))
										{
											break;
										}
									}
									if (j == i)
									{
										uniqueConstraintRoles[--lastItem] = testRole;
										if (lastItem == 1)
										{
											break;
										}
									}
								}
								Debug.Assert(!((ICollection<Role>)uniqueConstraintRoles).Contains(null)); // Algorithm gone bad
							}
							constraintRoles = uniqueConstraintRoles;
							constraintRoleCount -= duplicateCount;
						}
					}
					#region Optimized ConstraintRoleBox assignments
					// Optimization time: If we're dealing with binary or ternary constraints,
					// use the pre-defined ConstraintBoxRoleActivity collections.  This saves
					// on allocating tons of arrays every time the constraints are drawn or hit tested.
					ConstraintBoxRoleActivity[] predefinedActivityRoles = null;
					if (constraintRoleCount == factRoleCount)
					{
						predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_FullySpanning;
					}
					else if (constraintRoleCount == 0)
					{
						predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_AntiSpanning;
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
									case 1:
										int roleIndex = factRoles.IndexOf(constraintRoles[0]);
										Debug.Assert(roleIndex != -1); // This violates the IFactConstraint contract
										switch (roleIndex)
										{
											case 0:
												predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_Left;
												break;
											case 1:
												predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_Center;
												break;
											case 2:
												predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_Right;
												break;
										}
										break;
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
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryLeftCenter;
												}
												else if (roleIndex1 == 2)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryLeftRight;
												}
												break;
											case 1:
												if (roleIndex1 == 0)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryLeftCenter;
												}
												else if (roleIndex1 == 2)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryCenterRight;
												}
												break;
											case 2:
												if (roleIndex1 == 0)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryLeftRight;
												}
												else if (roleIndex1 == 1)
												{
													predefinedActivityRoles = PreDefinedConstraintBoxRoleActivities_TernaryCenterRight;
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
						constraintBoxes[currentConstraintIndex] = new ConstraintBox(factConstraint, constraintRoles, predefinedActivityRoles);
					}
					else
					{
						Debug.Assert(factRoleCount >= 4);
						// The original code, now used for handling fact types with 4 or more roles
						// or fact types that are irregular. 
						ConstraintBox currentBox = new ConstraintBox(factConstraint, constraintRoles, factRoleCount);

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
						constraintBoxes[currentConstraintIndex] = currentBox;
					}
					#endregion // Manual ConstraintRoleBox assignment
					++currentConstraintIndex;
				}

				// Get an initial grouping
				int significantConstraintCount = ConstraintBox.GroupConstraintBoxes(constraintBoxes);

				// Determine which boxes should be hidden
				int internalsCount = 0;
				for (int i = 0; i < significantConstraintCount; ++i)
				{
					if (constraintBoxes[i].ConstraintType != ConstraintType.InternalUniqueness)
					{
						break;
					}
					++internalsCount;
				}
				bool haveExternals = internalsCount < significantConstraintCount;
				bool haveInternals = internalsCount > 0;

				// Hide all the internals if we're walking the wrong box
				ConstraintDisplayPosition currentDisplayPosition = ConstraintDisplayPosition;
				if (displayPosition != currentDisplayPosition)
				{
					if (!haveExternals)
					{
						return; // There's nothing left to do, just get out
					}
					for (int i = 0; i < internalsCount; ++i)
					{
						constraintBoxes[i].IsHidden = true;
					}
					haveInternals = false;
				}

				// Figure out where externals should be displayed
				if (haveExternals)
				{
					// Be very careful here so as to not make this routine recursive.
					// What we want:
					//    If the center of the external constraint is above the center
					//    of the role box and the constraint needs a spanning bar
					//    displayed, then the bar should be on the same side of the
					//    role box as the constraint center point.
					// What we can do:
					//    In practice, displaying the bar moves the center point. Finding
					//    the center point requires a size for all of the constraint boxes,
					//    which requires this routine. To avoid blowing the stack, we just
					//    use the center point that we would have if there were no extenal
					//    bars and call this sufficient.
					int externalsCount = significantConstraintCount - internalsCount;
					double testVerticalPoint = Location.Y + mySpacerShapeField.GetMinimumHeight(this) + RoleBoxHeight / 2;
					ORMDiagram diagram = Diagram as ORMDiagram;
					if (currentDisplayPosition == ConstraintDisplayPosition.Top)
					{
						// Offset by the internals. These display above the externals for
						// the top box, and below them for the bottom box.
						testVerticalPoint += internalsCount * ConstraintHeight;
					}
					for (int i = internalsCount; i < significantConstraintCount; ++i)
					{
						bool showConstraint;
						ExternalConstraintRoleBarDisplay displayOption = OptionsPage.CurrentExternalConstraintRoleBarDisplay;
						if (displayOption == ExternalConstraintRoleBarDisplay.AnyRole)
						{
							showConstraint = true;
						}
						else
						{
							IList<Role> roles = constraintBoxes[i].RoleCollection;
							switch (roles.Count)
							{
								case 1:
									showConstraint = false;
									break;
								case 2:
									if (displayOption == ExternalConstraintRoleBarDisplay.AdjacentRoles)
									{
										showConstraint = true;
									}
									else
									{
										int index1 = factRoles.IndexOf(roles[0]);
										int index2 = factRoles.IndexOf(roles[1]);
										showConstraint = Math.Abs(index1 - index2) > 1;
									}
									break;
								default:
									showConstraint = true;
									break;
							}
						}
						if (showConstraint)
						{
							ShapeElement constraintShape = diagram.FindShapeForElement((ModelElement)constraintBoxes[i].FactConstraint.Constraint);
							if (constraintShape == null)
							{
								// This can happen if the constraint is implied. Implied constraints are not displayed.
								showConstraint = false;
							}
							else
							{
								double constraintVerticalCenter = constraintShape.AbsoluteCenter.Y;
								showConstraint = (displayPosition == ConstraintDisplayPosition.Top) ?
									constraintVerticalCenter < testVerticalPoint :
									constraintVerticalCenter >= testVerticalPoint;
							}
						}
						if (!showConstraint)
						{
							--externalsCount;
							constraintBoxes[i].IsHidden = true;
						}
					}
					if (externalsCount == 0 && !haveInternals)
					{
						return; // Nothing left to process
					}
				}

				// Further refine the set
				significantConstraintCount = ConstraintBox.OrderConstraintBoxes(constraintBoxes, significantConstraintCount);

				// Walk the constraintBoxes array and assign a physical location to each constraint box,
				double constraintHeight = ConstraintHeight;
				double constraintWidth = fullBounds.Width / (double)factRoleCount;
				fullBounds.Height = constraintHeight;
				int iBox;
				int incr;
				if (ConstraintDisplayPosition == ConstraintDisplayPosition.Bottom)
				{
					// walk the constraints from top to bottom
					iBox = significantConstraintCount - 1;
					incr = -1;
				}
				else
				{
					// walk the constraints from bottom to top
					iBox = 0;
					incr = 1;
				}
				#region Compressing the ConstraintRoleBoxes of binary fact types.
				if (factRoleCount == 2)
				{
					bool skippedRow = false;
					int nextCompressedConstraint = -1;
					double lastCompressedBottom = 0;
					for (; iBox >= 0 && iBox < significantConstraintCount; iBox += incr)
					{
						ConstraintBox box = constraintBoxes[iBox];
						box.Bounds = fullBounds;
						RectangleD bounds = box.Bounds;

						ConstraintBoxRoleActivity[] activeRoles = box.ActiveRoles;
						if (activeRoles.Length == 2) // Weed out fully spanning and antispanning
						{
							if (nextCompressedConstraint == iBox)
							{
								nextCompressedConstraint = -1;
								if (activeRoles[0] == ConstraintBoxRoleActivity.Inactive)
								{
									bounds.X += constraintWidth;
								}
								box.CompressBinaryActiveRoles();
								bounds.Width -= constraintWidth;
								bounds.Y = lastCompressedBottom;
								skippedRow = true;
							}
							else
							{
								int checkSide = (activeRoles[0] == ConstraintBoxRoleActivity.Inactive) ? 0 : 1;
								for (int j = iBox + incr; j >= 0 && j < significantConstraintCount; j += incr)
								{
									ConstraintBoxRoleActivity[] testActiveRoles = constraintBoxes[j].ActiveRoles;
									if (testActiveRoles.Length == 2 && testActiveRoles[checkSide] == ConstraintBoxRoleActivity.Active)
									{
										nextCompressedConstraint = j;
										break;
									}
								}
								if (nextCompressedConstraint != -1)
								{
									lastCompressedBottom = bounds.Y;
									if (checkSide == 0)
									{
										bounds.X += constraintWidth;
									}
									box.CompressBinaryActiveRoles();
									bounds.Width -= constraintWidth;
								}
							}
						}
						box.Bounds = bounds;
						if (!boxUser(ref box))
						{
							break;
						}
						if (skippedRow)
						{
							skippedRow = false;
						}
						else
						{
							fullBounds.Offset(0, constraintHeight);
						}
					}
				}
				#endregion // Compressing the ConstraintRoleBoxes of binary fact types.
				// Unaries, ternaries and n-aries do not need to have 
				// their internal uniqueness constraints compressed.
				// This will also run if a binary has too many constraints.
				else
				{
					for (; iBox >= 0 && iBox < significantConstraintCount; iBox += incr)
					{
						ConstraintBox box = constraintBoxes[iBox];
						box.Bounds = fullBounds;
						if (!boxUser(ref box))
						{
							break;
						}
						fullBounds.Offset(0, constraintHeight);
					}
				}
			}
		}
		#endregion // WalkConstraintBoxes implementation
		#region Size Constants
		private const double RoleBoxHeight = 0.11;
		private const double RoleBoxWidth = 0.16;
		private const double NestedFactHorizontalMargin = 0.09;
		private const double NestedFactVerticalMargin = 0.056;
		private const double ConstraintHeight = 0.07;
		private const double ExternalConstraintBarCenterAdjust = ConstraintHeight / 5;
		private const double BorderMargin = 0.05;
		private const double FocusIndicatorInsideMargin = .019;
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
					return BorderMargin / 2;
				}
			}

			// Nothing to paint for the spacer. So, no DoPaint override needed.

		}
		#endregion // SpacerShapeField class
		#region ConstraintShapeField : ShapeField
		private class ConstraintShapeField : ShapeField
		{
			private ConstraintDisplayPosition myDisplayPosition;

			public ConstraintShapeField(ConstraintDisplayPosition displayPosition)
			{
				DefaultFocusable = true;
				DefaultSelectable = true;
				DefaultVisibility = true;
				myDisplayPosition = displayPosition;
			}
			/// <summary>
			/// Accessor property for display position provided to constructor
			/// </summary>
			public ConstraintDisplayPosition DisplayPosition
			{
				get
				{
					return myDisplayPosition;
				}
			}
			/// <summary>
			/// Find the constraint sub shape at this location
			/// </summary>
			/// <param name="point">The point being hit-tested.</param>
			/// <param name="parentShape">The current FactTypeShape that the mouse is over.</param>
			/// <param name="diagramHitTestInfo">The DiagramHitTestInfo to which the ConstraintSubShapField
			/// will be added if the mouse is over it.</param>
			public override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					myDisplayPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						RectangleD fullBounds = constraintBox.Bounds;
						if (fullBounds.Contains(point))
						{
							IFactConstraint factConstraint = constraintBox.FactConstraint;
							diagramHitTestInfo.HitDiagramItem = new DiagramItem(parentShape, this, new ConstraintSubField(factConstraint.Constraint));
							return false; // Don't continue, we got our item
						}
						return true;
					});
			}
			/// <summary>
			/// Return the number of child constraints displayed in this shape
			/// </summary>
			/// <param name="parentShape">The current FactTypeShape to retrieve information for</param>
			/// <returns>Total number of child items</returns>
			public override int GetAccessibleChildCount(ShapeElement parentShape)
			{
				int total = 0;
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					myDisplayPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						++total;
						return true; // Keep going to get the full total
					});
				return total;
			}
			/// <summary>
			/// Return the child shape field corresponding to the specified index
			/// </summary>
			/// <param name="parentShape">The current FactTypeShape to retrieve information for</param>
			/// <param name="index">The 0-based index of the child to retrieve</param>
			/// <returns>ShapeSubField for the specified constraint</returns>
			public override ShapeSubField GetAccessibleChild(ShapeElement parentShape, int index)
			{
				ShapeSubField retVal = null;
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					myDisplayPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						if (index == 0)
						{
							retVal = new ConstraintSubField(constraintBox.FactConstraint.Constraint);
							return false; // Don't continue, we got our item
						}
						--index;
						return true; // Keep going
					});
				return retVal;
			}
			/// <summary>
			/// Get the minimum width of the ConstraintShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape that this ConstraintShapeField is associated with.</param>
			/// <returns>The width of the ConstraintShapeField.</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				return RolesShape.GetMinimumWidth(parentShape);
			}

			/// <summary>
			/// Get the minimum height of the ConstraintShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape that this ConstraintShapeField is associated with.</param>
			/// <returns>The height of the ConstraintShapeField.</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				double minY = double.MaxValue;
				double maxY = double.MinValue;
				bool wasVisited = false;
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					RectangleD.Empty,
					myDisplayPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						wasVisited = true;
						RectangleD bounds = constraintBox.Bounds;
						minY = Math.Min(minY, bounds.Top);
						maxY = Math.Max(maxY, bounds.Bottom);
						return true;
					});
				return wasVisited ? maxY - minY : 0;
			}

			/// <summary>
			/// Paints the contstraints.
			/// </summary>
			/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
			/// <param name="parentShape">ConstraintShapeField to draw to.</param>
			public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				FactTypeShape factShape = parentShape as FactTypeShape;
				Graphics g = e.Graphics;
				HighlightedShapesCollection highlightedShapes = null;
				SelectedShapesCollection selection = null;
				ConstraintSubField testSubField = null;
				DiagramItem testSelect = null;
				DiagramClientView view = e.View;
				if (view != null)
				{
					highlightedShapes = view.HighlightedShapes;
					selection = view.Selection;
				}
				StyleSet styleSet = factShape.StyleSet;
				Pen alethicConstraintPen = styleSet.GetPen(InternalFactConstraintPen);
				Pen deonticConstraintPen = styleSet.GetPen(DeonticInternalFactConstraintPen);
				float gap = alethicConstraintPen.Width;
				ConstraintDisplayPosition position = myDisplayPosition;
				ORMDiagram diagram = (ORMDiagram)factShape.Diagram;
				StyleSet diagramStyleSet = diagram.StyleSet;

				factShape.WalkConstraintBoxes(
					this,
					position,
					delegate(ref ConstraintBox constraintBox)
					{
						bool isInternalConstraint = constraintBox.ConstraintType == ConstraintType.InternalUniqueness;

						//default variables
						IFactConstraint factConstraint = constraintBox.FactConstraint;
						IConstraint currentConstraint = factConstraint.Constraint;
						RectangleF boundsF = RectangleD.ToRectangleF(constraintBox.Bounds);
						float verticalPos = boundsF.Top + (float)(ConstraintHeight / 2);
						ConstraintBoxRoleActivity[] rolePosToDraw = constraintBox.ActiveRoles;
						int numRoles = rolePosToDraw.Length;
						float roleWidth = (float)FactTypeShape.RoleBoxWidth;
						bool isDeontic = currentConstraint.Modality == ConstraintModality.Deontic;
						Pen constraintPen = isDeontic ? deonticConstraintPen : alethicConstraintPen;
						Color startColor = constraintPen.Color;
						DashStyle startDashStyle = constraintPen.DashStyle;

						if (isInternalConstraint)
						{
							//test if constraint is valid and apply appropriate pen
							if (!constraintBox.IsValid)
							{
								constraintPen.Color = factShape.ConstraintErrorForeColor;
							}
							if (constraintBox.IsAntiSpanning)
							{
								constraintPen.DashStyle = DashStyle.Dash;
							}
						}

						// Draw active constraint highlight
						bool isHighlighted = false;
						bool isSticky = false;
						if (isInternalConstraint)
						{
							InternalUniquenessConstraintConnectAction activeInternalAction = ActiveInternalUniquenessConstraintConnectAction;
							if (activeInternalAction != null)
							{
								InternalUniquenessConstraint activeInternalConstraint = activeInternalAction.ActiveConstraint;
								InternalUniquenessConstraint targetConstraint = currentConstraint as InternalUniquenessConstraint;
								if (object.ReferenceEquals(activeInternalConstraint, targetConstraint))
								{
									isSticky = true;
									constraintPen.Color = diagramStyleSet.GetPen(ORMDiagram.StickyForegroundResource).Color;
								}
							}
						}
						else
						{
							ExternalConstraintShape externalConstraintShape = diagram.StickyObject as ExternalConstraintShape;
							if (externalConstraintShape != null &&
								object.ReferenceEquals(externalConstraintShape.AssociatedConstraint, currentConstraint))
							{
								constraintPen.Color = diagramStyleSet.GetPen(ORMDiagram.StickyBackgroundResource).Color;
							}
						}

						// test for and draw highlights
						if (highlightedShapes != null)
						{
							foreach (DiagramItem item in highlightedShapes)
							{
								if (object.ReferenceEquals(factShape, item.Shape))
								{
									ConstraintSubField highlightedSubField = item.SubField as ConstraintSubField;
									if (highlightedSubField != null && highlightedSubField.AssociatedConstraint == currentConstraint)
									{
										isHighlighted = true;
										constraintPen.Color = ORMDiagram.ModifyLuminosity(constraintPen.Color);
										break;
									}
								}
							}
						}
						if (isHighlighted || isSticky)
						{
							factShape.DrawHighlight(g, boundsF, isSticky, isHighlighted);
						}

						if (isInternalConstraint)
						{
							if (selection != null)
							{
								if (testSubField == null)
								{
									testSubField = new ConstraintSubField(currentConstraint);
									testSelect = new DiagramItem(parentShape, this, testSubField);
								}
								else
								{
									testSubField.AssociatedConstraint = currentConstraint;
								}
								if (selection.Contains(testSelect))
								{
									RectangleF constraintBounds = boundsF;
									StyleSetResourceId pen1Id;
									StyleSetResourceId pen2Id;
									if (testSelect.Equals(selection.FocusedItem))
									{
										pen1Id = DiagramPens.FocusIndicatorBackground;
										pen2Id = DiagramPens.FocusIndicator;
									}
									else
									{
										pen1Id = DiagramPens.SelectionBackground;
										pen2Id = testSelect.Equals(selection.PrimaryItem) ? DiagramPens.SelectionPrimaryOutline : DiagramPens.SelectionNonPrimaryOutline;
									}
									Pen pen = styleSet.GetPen(pen1Id);
									if (pen.Alignment == PenAlignment.Center)
									{
										float adjust = -pen.Width / 2;
										constraintBounds.Inflate(adjust, adjust);
									}
									g.DrawRectangle(pen, constraintBounds.Left, constraintBounds.Top, constraintBounds.Width, constraintBounds.Height);
									g.DrawRectangle(styleSet.GetPen(pen2Id), constraintBounds.Left, constraintBounds.Top, constraintBounds.Width, constraintBounds.Height);
								}
							}
							float startPos = boundsF.Left, endPos = startPos;
							bool drawConstraintPreffered = factShape.ShouldDrawConstraintPreferred(currentConstraint);
							if (constraintBox.IsSpanning || constraintBox.IsAntiSpanning)
							{
								endPos = boundsF.Right;
								//draw fully spanning constraint
								DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered, isDeontic && constraintBox.IsSpanning);
							}
							else
							{
								bool positionChanged = false;
								bool constraintHasDrawn = false;
								int i = 0;
								ConstraintBoxRoleActivity currentActivity = rolePosToDraw[i];
								bool drawCalled = false;
								for (; i < numRoles; ++i)
								{
									ConstraintBoxRoleActivity currentBoxActivity = rolePosToDraw[i];
									if (currentActivity != currentBoxActivity)
									{
										//activity has changed; draw previous activity
										if (positionChanged && currentActivity != ConstraintBoxRoleActivity.NotInBox)
										{
											if (currentActivity == ConstraintBoxRoleActivity.Active)
											{
												constraintPen.DashStyle = startDashStyle;
												constraintHasDrawn = true;
											}
											else
											{
												Debug.Assert(currentActivity == ConstraintBoxRoleActivity.Inactive); // enforces if statement above
												constraintPen.DashStyle = DashStyle.Dash;
											}
											//draw constraint
											if (constraintHasDrawn)
											{
												DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered, isDeontic && !drawCalled);
												drawCalled = true;
											}
											startPos = endPos;
											positionChanged = false;
										}
										currentActivity = currentBoxActivity;
									}
									// move to next position
									if (currentActivity != ConstraintBoxRoleActivity.NotInBox)
									{
										endPos += roleWidth;
										positionChanged = true;
									}
									else if (boundsF.Width > roleWidth)
									{
										// this covers BinaryRights when not compressing constraints
										startPos += roleWidth;
										endPos = startPos;
										positionChanged = false;
									}
								}
								// set DashStyle to original setting (solid)
								if (constraintPen.DashStyle != startDashStyle)
								{
									constraintPen.DashStyle = startDashStyle;
								}
								//We've reached the end. Draw out any right constraints that may exist.
								if (endPos > startPos && currentActivity == ConstraintBoxRoleActivity.Active)
								{
									DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered, isDeontic && !drawCalled);
								}
							}
						}
						else
						{
							int firstActive = -1;
							int lastActive = -1;
							float targetVertical;
							if (position == ConstraintDisplayPosition.Bottom)
							{
								verticalPos += (float)ExternalConstraintBarCenterAdjust;
								targetVertical = boundsF.Top;
							}
							else
							{
								verticalPos -= (float)ExternalConstraintBarCenterAdjust;
								targetVertical = boundsF.Bottom;
							}
							bool fullySpanning = rolePosToDraw == PreDefinedConstraintBoxRoleActivities_FullySpanning;
							if (fullySpanning)
							{
								numRoles = factConstraint.FactType.RoleCollection.Count;
							}
							float startPos = boundsF.Left;
							for (int i = 0; i < numRoles; ++i)
							{
								ConstraintBoxRoleActivity currentActivity = fullySpanning ? ConstraintBoxRoleActivity.Active : rolePosToDraw[i];
								if (currentActivity == ConstraintBoxRoleActivity.Active)
								{
									if (firstActive == -1)
									{
										firstActive = i;
									}
									else
									{
										float x = startPos + (firstActive + .5f) * roleWidth;
										g.DrawLine(constraintPen, x, verticalPos, x, targetVertical);
										x += (i - firstActive) * roleWidth;
										g.DrawLine(constraintPen, x, verticalPos, x, targetVertical);
									}
									lastActive = i;
								}
								else if (firstActive == -1 && currentActivity == ConstraintBoxRoleActivity.NotInBox)
								{
									startPos -= roleWidth;
								}
							}
							if (lastActive == firstActive)
							{
								// Draw a box on a single role. This is used only for accessibility
								// cases when the ExternalConstraintRoleBarDisplay is set to AnyRole.
								// This is designed to provide a selectable accessibility object for
								// all constraints associated with a fact.
								float x1 = startPos + (firstActive + .3f) * roleWidth;
								float x2 = x1 + .4f * roleWidth;
								g.DrawLine(constraintPen, x1, verticalPos, x1, targetVertical);
								g.DrawLine(constraintPen, x1, verticalPos, x2, verticalPos);
								g.DrawLine(constraintPen, x2, verticalPos, x2, targetVertical);
							}
							else
							{
								g.DrawLine(constraintPen, startPos + (firstActive + .5f) * roleWidth, verticalPos, boundsF.Right - (numRoles - lastActive - .5f) * roleWidth, verticalPos);
							}
						}

						// set colors back to normal if they changed
						if (constraintPen.Color != startColor)
						{
							constraintPen.Color = startColor;
						}
						// set DashStyle to original setting (solid)
						if (constraintPen.DashStyle != startDashStyle)
						{
							constraintPen.DashStyle = startDashStyle;
						}

						return true;
					});
			}

			/// <summary>
			/// Draws a regular constraint line
			/// </summary>
			/// <param name="g">The graphics object to draw to</param>
			/// <param name="pen">The pen to use</param>
			/// <param name="startPos">The x-coordinate of the left edge to draw at.</param>
			/// <param name="endPos">The x-coordinate of the right edge to draw at.</param>
			/// <param name="verticalPos">The y-coordinate to draw at.</param>
			/// <param name="gap">The gap to leave at the ends of the constraint line</param>
			/// <param name="preferred">Whether or not to draw the constraint as preffered.</param>
			/// <param name="deontic">Whether or not to draw this portion of the constraint as deontic.</param>
			private static void DrawInternalConstraintLine(Graphics g, Pen pen, float startPos, float endPos, float verticalPos, float gap, bool preferred, bool deontic)
			{
				if (deontic)
				{
					float deonticRadius = (float)(FactTypeShape.ConstraintHeight / 2) - gap;
					float deonticDiameter = deonticRadius + deonticRadius;
					g.DrawArc(pen, startPos + gap, verticalPos - deonticRadius, deonticDiameter, deonticDiameter, 0, 360);
					startPos += deonticDiameter;
				}
				if (preferred)
				{
					float vAdjust = gap * .75f;
					g.DrawLine(pen, startPos + gap, verticalPos - vAdjust, endPos - gap, verticalPos - vAdjust);
					g.DrawLine(pen, startPos + gap, verticalPos + vAdjust, endPos - gap, verticalPos + vAdjust);
				}
				else
				{
					g.DrawLine(pen, startPos + gap, verticalPos, endPos - gap, verticalPos);
				}
			}
		}
		#endregion // ConstraintShapeField class
		#region ConstraintSubField class
		private class ConstraintSubField : ShapeSubField
		{
			#region Mouse handling
			public override void OnDoubleClick(DiagramPointEventArgs e)
			{
				if (ORMBaseShape.AttemptErrorActivation(e))
				{
					base.OnDoubleClick(e);
					return;
				}
				DiagramClientView clientView = e.DiagramClientView;
				ORMDiagram diagram = clientView.Diagram as ORMDiagram;
				InternalUniquenessConstraint iuc = AssociatedConstraint as InternalUniquenessConstraint;
				if (iuc != null)
				{
					// Move on to the selection action
					InternalUniquenessConstraintConnectAction iucca = diagram.InternalUniquenessConstraintConnectAction;
					ActiveInternalUniquenessConstraintConnectAction = iucca;
					RoleMoveableCollection roleColl = iuc.RoleCollection;
					FactTypeShape factShape = e.DiagramHitTestInfo.HitDiagramItem.Shape as FactTypeShape;
					if (roleColl.Count != 0)
					{
						IList<Role> iuccaRoles = iucca.SelectedRoleCollection;
						foreach (Role r in roleColl)
						{
							iuccaRoles.Add(r);
						}
					}
					factShape.Invalidate(true);
					iucca.ChainMouseAction(factShape, iuc, clientView);
				}
				// UNDONE: MSBUG Report Microsoft bug DiagramClientView.OnDoubleClick is checking
				// for an active mouse action after the double click and clearing it if it is set.
				// This may be appropriate if the mouse action was set before the subfield double
				// click and did not change during the callback, but is definitely not appropriate
				// if the double click activated the mouse action.
				//e.Handled = true;
				base.OnDoubleClick(e);
			}
			#endregion // Mouse handling
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
				set
				{
					myAssociatedConstraint = value;
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
			public override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				RectangleD fullBounds = GetBounds(parentShape);
				if (fullBounds.Contains(point))
				{
					FactTypeShape parentFactShape = parentShape as FactTypeShape;
					RoleMoveableCollection roles = parentFactShape.DisplayedRoleOrder;
					int roleCount = roles.Count;
					if (roleCount != 0)
					{
						int roleIndex = Math.Min((int)((point.X - fullBounds.Left) * roleCount / fullBounds.Width), roleCount - 1);
						diagramHitTestInfo.HitDiagramItem = new DiagramItem(parentShape, this, new RoleSubField(roles[roleIndex]));
					}
				}
			}
			/// <summary>
			/// Return the number of children in this shape field.
			/// Maps to the number of roles on the FactTypeShape
			/// </summary>
			public override int GetAccessibleChildCount(ShapeElement parentShape)
			{
				return (parentShape as FactTypeShape).AssociatedFactType.RoleCollection.Count;
			}
			/// <summary>
			/// Return the RoleSubField corresponding to the role at the requested index
			/// </summary>
			public override ShapeSubField GetAccessibleChild(ShapeElement parentShape, int index)
			{
				return new RoleSubField((parentShape as FactTypeShape).AssociatedFactType.RoleCollection[index]);
			}
			/// <summary>
			/// Get the minimum width of this RolesShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape associated with this RolesShapeField.</param>
			/// <returns>The width of this RolesShapeField.</returns>
			public override double GetMinimumWidth(ShapeElement parentShape)
			{
				double margin = parentShape.StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width;
				return FactTypeShape.RoleBoxWidth * Math.Max(1, (parentShape as FactTypeShape).AssociatedFactType.RoleCollection.Count) + margin;
			}
			/// <summary>
			/// Get the minimum height of this RolesShapeField.
			/// </summary>
			/// <param name="parentShape">The FactTypeShape associated with this RolesShapeField.</param>
			/// <returns>The height of this RolesShapeField.</returns>
			public override double GetMinimumHeight(ShapeElement parentShape)
			{
				double margin = parentShape.StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width;
				return FactTypeShape.RoleBoxHeight + margin;
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
				RoleMoveableCollection roles = parentFactShape.DisplayedRoleOrder;
				int roleCount = roles.Count;
				bool objectified = factType.NestingType != null;
				if (roleCount > 0 || objectified)
				{
					int highlightRoleBox = -1;
					RoleSubField testSubField = new RoleSubField();
					DiagramItem testSelect = new DiagramItem(parentShape, this, testSubField);
					DiagramClientView clientView = e.View;
					SelectedShapesCollection selection = null;
					if (clientView != null)
					{
						selection = clientView.Selection;
						foreach (DiagramItem item in clientView.HighlightedShapes)
						{
							if (object.ReferenceEquals(parentShape, item.Shape))
							{
								RoleSubField roleField = item.SubField as RoleSubField;
								if (roleField != null)
								{
									highlightRoleBox = roles.IndexOf(roleField.AssociatedRole);
									break;
								}
							}
						}
					}
					RectangleD bounds = this.GetBounds(parentShape);
					double margin = parentShape.StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width / 2;
					bounds.Inflate(-margin, -margin);

					Graphics g = e.Graphics;
					double offsetBy = bounds.Width / roleCount;
					float offsetByF = (float)offsetBy;
					double lastX = bounds.Left;
					StyleSet styleSet = parentShape.StyleSet;
					Pen pen = styleSet.GetPen(FactTypeShape.RoleBoxResource);
					int activeRoleIndex;
					float top = (float)bounds.Top;
					float bottom = (float)bounds.Bottom;
					float height = (float)bounds.Height;
					ExternalConstraintConnectAction activeExternalAction = ActiveExternalConstraintConnectAction;
					InternalUniquenessConstraintConnectAction activeInternalAction = ActiveInternalUniquenessConstraintConnectAction;
					ORMDiagram currentDiagram = parentFactShape.Diagram as ORMDiagram;
					StringFormat stringFormat = null;
					Font connectActionFont = null;
					Brush connectActionBrush = null;
					Font constraintSequenceFont = null;
					Brush constraintSequenceBrush = null;
					bool highlightThisRole = false;
					try
					{
						for (int i = 0; i < roleCount; ++i)
						{
							float lastXF = (float)lastX;
							RectangleF roleBounds = new RectangleF(lastXF, top, offsetByF, height);
							highlightThisRole = (i == highlightRoleBox);
							Role currentRole = roles[i];

							// There is an active ExternalConstraintConnectAction, and this role is currently in the action's role set.
							if ((activeExternalAction != null) &&
								(-1 != (activeRoleIndex = activeExternalAction.GetActiveRoleIndex(currentRole))))
							{
								// There is an active ExternalConstraintConnectAction, and this role is currently in the action's role set.
								DrawHighlight(g, styleSet, roleBounds, highlightThisRole);
								if (stringFormat == null)
								{
									stringFormat = new StringFormat();
									stringFormat.LineAlignment = StringAlignment.Center;
									stringFormat.Alignment = StringAlignment.Center;
								}
								if (connectActionFont == null)
								{
									connectActionFont = styleSet.GetFont(DiagramFonts.CommentText);
								}
								if (connectActionBrush == null)
								{
									connectActionBrush = styleSet.GetBrush(RolePickerForeground);
								}
								g.DrawString((activeRoleIndex + 1).ToString(CultureInfo.InvariantCulture), connectActionFont, connectActionBrush, roleBounds, stringFormat);
							}
							// There is an active InternalUniquenessConstraintConnectAction, and this role is currently in the action's role set.
							else if (activeInternalAction != null && -1 != (activeRoleIndex = activeInternalAction.GetActiveRoleIndex(currentRole)))
							{
								// There is an active InternalUniquenessConstraintConnectAction, and this role is currently in the action's role set.
								DrawHighlight(g, styleSet, roleBounds, highlightThisRole);
							}
							else if (null != currentDiagram)
							{
								// Current diagram is an ORMDiagram.
								#region Handling StickyObject highlighting and selection
								ExternalConstraintShape stickyConstraintShape;
								IConstraint stickyConstraint;
								// The active StickyObject for the diagram is an ExternalConstraintShape
								if (null != (stickyConstraintShape = currentDiagram.StickyObject as ExternalConstraintShape)
									&& null != (stickyConstraint = stickyConstraintShape.AssociatedConstraint))
								{
									ConstraintRoleSequence sequence = null;
									bool roleIsInStickyObject = false;

									// Test to see if the diagram's StickyObject (which is an IConstraint) contains a reference to this role.
									foreach (ConstraintRoleSequence c in currentRole.ConstraintRoleSequenceCollection)
									{
										if (object.ReferenceEquals(c.Constraint, stickyConstraint))
										{
											sequence = c;
											roleIsInStickyObject = true;
											break;
										}
									}

									// This role is in the diagram's StickyObject.
									if (roleIsInStickyObject)
									{
										// We need to find out if this role is in one of the role sequences being edited, or if it's just selected.
										parentFactShape.DrawHighlight(g, roleBounds, true, highlightThisRole);
										Debug.Assert(sequence != null);
										MultiColumnExternalConstraint mcec;
										SingleColumnExternalConstraint scec;
										bool drawIndexNumbers = false;
										string indexString = "";

										if (activeExternalAction == null)
										{
											drawIndexNumbers = true;
										}
										else
										{
											if (activeExternalAction.InitialRoles.IndexOf(currentRole) < 0)
											{
												drawIndexNumbers = true;
											}
										}
										if (drawIndexNumbers)
										{
											if (null != (mcec = stickyConstraint as MultiColumnExternalConstraint))
											{
												MultiColumnExternalConstraintRoleSequenceMoveableCollection sequenceCollection = mcec.RoleSequenceCollection;
												int sequenceCollectionCount = sequenceCollection.Count;
												for (int sequenceIndex = 0; sequenceIndex < sequenceCollectionCount; ++sequenceIndex)
												{
													int roleIndex = sequenceCollection[sequenceIndex].RoleCollection.IndexOf(currentRole);
													if (roleIndex >= 0)
													{
														for (int j = sequenceIndex + 1; j < sequenceCollectionCount; ++j)
														{
															if (sequenceCollection[j].RoleCollection.IndexOf(currentRole) >= 0)
															{
																// Indicate overlapping role sequences
																indexString = ResourceStrings.SetConstraintStickyRoleOverlapping;
															}
														}
														if (indexString.Length == 0)
														{
															// Show 1-based position of the role in the MCEC.
															indexString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.SetConstraintStickyRoleFormatString, sequenceIndex + 1, roleIndex + 1);
														}
														break;
													}
												}
											}
											else if (null != (scec = stickyConstraint as SingleColumnExternalConstraint))
											{
												indexString = (scec.RoleCollection.IndexOf(currentRole) + 1).ToString();
											}

											if (stringFormat == null)
											{
												stringFormat = new StringFormat();
												stringFormat.LineAlignment = StringAlignment.Center;
												stringFormat.Alignment = StringAlignment.Center;
											}
											if (constraintSequenceFont == null)
											{
												constraintSequenceFont = styleSet.GetFont(RoleBoxResource);
											}
											if (constraintSequenceBrush == null)
											{
												constraintSequenceBrush = currentDiagram.StyleSet.GetBrush(ORMDiagram.StickyForegroundResource);
											}
											g.DrawString(indexString, constraintSequenceFont, constraintSequenceBrush, roleBounds, stringFormat);
										}
									}
									else if (highlightThisRole)
									{
										parentFactShape.DrawHighlight(g, roleBounds, false, true);
									}
								}
								#endregion // Handling StickyObject highlighting and selection
								else if (highlightThisRole)
								{
									parentFactShape.DrawHighlight(g, roleBounds, false, true);
								}
							}

							// Draw a selection rectangle if needed
							testSubField.AssociatedRole = currentRole;
							if (selection != null && selection.Contains(testSelect))
							{
								roleBounds.Inflate(-.02f, -.02f);
								StyleSetResourceId pen1Id;
								StyleSetResourceId pen2Id;
								if (testSelect.Equals(selection.FocusedItem))
								{
									pen1Id = DiagramPens.FocusIndicatorBackground;
									pen2Id = DiagramPens.FocusIndicator;
								}
								else
								{
									pen1Id = DiagramPens.SelectionBackground;
									pen2Id = testSelect.Equals(selection.PrimaryItem) ? DiagramPens.SelectionPrimaryOutline : DiagramPens.SelectionNonPrimaryOutline;
								}
								g.DrawRectangle(styleSet.GetPen(pen1Id), roleBounds.Left, roleBounds.Top, roleBounds.Width, roleBounds.Height);
								g.DrawRectangle(styleSet.GetPen(pen2Id), roleBounds.Left, roleBounds.Top, roleBounds.Width, roleBounds.Height);
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
						if (constraintSequenceFont != null)
						{
							constraintSequenceFont.Dispose();
						}
						if (connectActionFont != null)
						{
							connectActionFont.Dispose();
						}
					}
					// Draw the outside border of the role boxes
					RectangleF boundsF = RectangleD.ToRectangleF(bounds);
					g.DrawRectangle(pen, boundsF.Left, boundsF.Top, boundsF.Width, boundsF.Height);
				}
			}
			/// <summary>
			/// Draws a role highlight.
			/// </summary>
			/// <param name="g">The Graphics object to draw to.</param>
			/// <param name="styleSet">The StyleSet of the shape we are drawing to.</param>
			/// <param name="bounds">The bounds to draw as the highlight.</param>
			/// <param name="active">Boolean indicating whether or not to draw highlight as active (ex: the mouse is currently over this highlight).</param>
			protected static void DrawHighlight(Graphics g, StyleSet styleSet, RectangleF bounds, bool active)
			{
				Brush brush = styleSet.GetBrush(RoleBoxResource);
				Color startColor = default(Color);
				SolidBrush coloredBrush = null;
				if (!SystemInformation.HighContrast && active)
				{
					coloredBrush = brush as SolidBrush;
					if (coloredBrush != null)
					{
						startColor = coloredBrush.Color;
						coloredBrush.Color = ORMDiagram.ModifyLuminosity(coloredBrush.Color);
					}
				}
				g.FillRectangle(brush, bounds);
				if (coloredBrush != null)
				{
					coloredBrush.Color = startColor;
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
			public RoleSubField()
			{
			}
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
				FactTypeShape parentFactShape = parentShape as FactTypeShape;
				RoleMoveableCollection roles = parentFactShape.DisplayedRoleOrder;
				retVal.Width /= roles.Count;
				int roleIndex = roles.IndexOf(myAssociatedRole);
				if (roleIndex > 0)
				{
					retVal.Offset(roleIndex * retVal.Width, 0);
				}
				return retVal;
			}
			#endregion // Required ShapeSubField
			#region Mouse handling
			public override void OnDoubleClick(DiagramPointEventArgs e)
			{
				ORMBaseShape.AttemptErrorActivation(e);
				base.OnDoubleClick(e);
			}
			#endregion // Mouse handling
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
			/// Get or set the Role element associated with this sub field
			/// </summary>
			public Role AssociatedRole
			{
				get
				{
					return myAssociatedRole;
				}
				set
				{
					myAssociatedRole = value;
				}
			}
			#endregion // Accessor functions
		}
		#endregion // RoleSubField class
		#region Member Variables
		private static RolesShapeField myRolesShapeField;
		private static ConstraintShapeField myTopConstraintShapeField;
		private static ConstraintShapeField myBottomConstraintShapeField;
		private static SpacerShapeField mySpacerShapeField;
		/// <summary>
		/// Pen to draw a role box outline
		/// </summary>
		protected static readonly StyleSetResourceId RoleBoxResource = new StyleSetResourceId("Neumont", "RoleBoxResource");
		/// <summary>
		/// Brush to draw the foreground text for a role picker  
		/// </summary>
		protected static readonly StyleSetResourceId RolePickerForeground = new StyleSetResourceId("Neumont", "RolePickerForeground");
		/// <summary>
		/// Pen to draw the active part of an internal uniqueness constraint.
		/// </summary>
		protected static readonly StyleSetResourceId InternalFactConstraintPen = new StyleSetResourceId("Neumont", "InternalFactConstraintPen");
		/// <summary>
		/// Pen to draw the active part of a deontic internal uniqueness constraint.
		/// </summary>
		protected static readonly StyleSetResourceId DeonticInternalFactConstraintPen = new StyleSetResourceId("Neumont", "DeonticInternalFactConstraintPen");
		private static ExternalConstraintConnectAction myActiveExternalConstraintConnectAction;
		private static InternalUniquenessConstraintConnectAction myActiveInternalUniquenessConstraintConnectAction;
		#endregion // Member Variables
		#region RoleSubField integration
		/// <summary>
		/// Get the role corresponding to the given subField
		/// </summary>
		/// <param name="field">The containing shape field (will always be the RolesShapeField)</param>
		/// <param name="subField">A RoleSubField</param>
		/// <returns>A Role element</returns>
		public override ICollection GetSubFieldRepresentedElements(ShapeField field, ShapeSubField subField)
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
		/// Show a shadow if this <see cref="FactTypeShape"/> represents a <see cref="FactType"/> that appears
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
		/// Always highlight
		/// </summary>
		public override bool HasHighlighting
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// The color to use when drawing constraint errors.
		/// </summary>
		protected Color ConstraintErrorForeColor
		{
			get
			{
				return (Store as IORMToolServices).FontAndColorService.GetForeColor(ORMDesignerColor.ConstraintError);
			}
		}
		/// <summary>
		/// Standard method to draw a consistent highlight within the FactTypeShape.
		/// </summary>
		/// <param name="g">The Graphics object to draw to.</param>
		/// <param name="bounds">The bounds of the highlight to draw.</param>
		/// <param name="isStuck">Bool indicating if the object to draw the highlight on
		/// is currently the "sticky" object.</param>
		/// <param name="isHighlighted">Bool indicating if object should be drawn highlighted.</param>
		protected void DrawHighlight(Graphics g, RectangleF bounds, bool isStuck, bool isHighlighted)
		{
			Brush brush;
			if (isStuck)
			{
				brush = Diagram.StyleSet.GetBrush(ORMDiagram.StickyBackgroundResource);
			}
			else
			{
				brush = StyleSet.GetBrush(DiagramBrushes.ShapeBackground);
			}
			Color startColor = default(Color);
			SolidBrush coloredBrush = null;
			if (!SystemInformation.HighContrast && isHighlighted)
			{
				coloredBrush = brush as SolidBrush;
				if (coloredBrush != null)
				{
					startColor = coloredBrush.Color;
					coloredBrush.Color = ORMDiagram.ModifyLuminosity(coloredBrush.Color);
				}
			}
			g.FillRectangle(brush, bounds);
			if (coloredBrush != null)
			{
				coloredBrush.Color = startColor;
			}
		}
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
			base.InitializeResources(classStyleSet);
			IORMFontAndColorService fontsAndColors = (Store as IORMToolServices).FontAndColorService;
			Color constraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.Constraint);
			Color deonticConstraintForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.DeonticConstraint);
			Color rolePickerForeColor = fontsAndColors.GetForeColor(ORMDesignerColor.RolePicker);
			Color rolePickerBackColor = fontsAndColors.GetBackColor(ORMDesignerColor.RolePicker);

			BrushSettings brushSettings = new BrushSettings();
			brushSettings.Color = rolePickerForeColor;
			classStyleSet.AddBrush(RolePickerForeground, DiagramBrushes.DiagramBackground, brushSettings);

			brushSettings.Color = rolePickerBackColor;
			classStyleSet.AddBrush(RoleBoxResource, DiagramBrushes.DiagramBackground, brushSettings);

			PenSettings penSettings = new PenSettings();
			penSettings.Width = 1.2f / 72f; // 1.2 point
			classStyleSet.OverridePen(DiagramPens.ShapeOutline, penSettings);

			penSettings.Color = SystemColors.WindowText;
			penSettings.Width = 1.0F / 72.0F; // 1 Point. 0 Means 1 pixel, but should only be used for non-printed items
			penSettings.Alignment = PenAlignment.Center;
			classStyleSet.AddPen(RoleBoxResource, DiagramPens.ShapeOutline, penSettings);

			penSettings.Color = constraintForeColor;
			classStyleSet.AddPen(InternalFactConstraintPen, DiagramPens.ShapeOutline, penSettings);
			penSettings.Color = deonticConstraintForeColor;
			classStyleSet.AddPen(DeonticInternalFactConstraintPen, InternalFactConstraintPen, penSettings);

			FontSettings fontSettings = new FontSettings();
			fontSettings.Size = 5f / 72f; // 5 Point.
			classStyleSet.AddFont(RoleBoxResource, DiagramFonts.CommentText, fontSettings);
		}
		/// <summary>
		/// Use the rolebox outline pen unless we're objectified
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				return IsObjectified ? DiagramPens.ShapeOutline : RoleBoxResource;
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

			Debug.Assert(myRolesShapeField == null); // Only called once
			myRolesShapeField = field;

			Debug.Assert(myTopConstraintShapeField == null); // Only called once
			myTopConstraintShapeField = topConstraintField;

			Debug.Assert(myBottomConstraintShapeField == null); // Only called once
			myBottomConstraintShapeField = bottomConstraintField;

			Debug.Assert(mySpacerShapeField == null); // Only called once
			mySpacerShapeField = spacer;
		}
		/// <summary>
		/// The shape field used to display roles
		/// </summary>
		protected static ShapeField RolesShape
		{
			get
			{
				return myRolesShapeField;
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
				SizeD retVal = SizeD.Empty;
				ShapeField rolesShape = RolesShape;
				if (rolesShape != null)
				{
					double width, height;
					width = rolesShape.GetMinimumWidth(this);
					height = rolesShape.GetMinimumHeight(this);
					height += myTopConstraintShapeField.GetMinimumHeight(this);
					height += myBottomConstraintShapeField.GetMinimumHeight(this);
					if (!IsObjectified)
					{
						width += BorderMargin;
						height += BorderMargin;
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
				if (!UpdateRolesPosition(contentSize))
				{
					Size = contentSize;
				}
			}
		}
		/// <summary>
		/// Update the cached center position of the role box
		/// </summary>
		/// <param name="newSize">The new size of the fact, or SizeD.Empty. Allows a move/size with
		/// a single action, resulting in a single transaction log entry for AbsoluteBounds attribute</param>
		/// <returns>true if the bounds were changed</returns>
		private bool UpdateRolesPosition(SizeD newSize)
		{
			bool retVal = false;
			double oldRolesPosition = RolesPosition;
			double newRolesPosition = myRolesShapeField.GetBounds(this).Center.Y;
			if (!VGConstants.FuzzEqual(oldRolesPosition, newRolesPosition, VGConstants.FuzzDistance))
			{
				RolesPosition = newRolesPosition;
				if (oldRolesPosition != 0)
				{
					PointD newLocation = Location;
					newLocation.Offset(0, oldRolesPosition - newRolesPosition);
					if (newSize.IsEmpty)
					{
						newSize = Size;
					}
					AbsoluteBounds = new RectangleD(newLocation, newSize);
					retVal = true;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Called during a transaction when a new constraint
		/// is added or removed that is associated with a fact.
		/// </summary>
		/// <param name="factType">The associated FactType</param>
		/// <param name="constraint">The newly added or removed constraint</param>
		/// <param name="roleChangeOnly">A role was added or removed</param>
		public static void ConstraintSetChanged(FactType factType, IConstraint constraint, bool roleChangeOnly)
		{
			Debug.Assert(factType.Store.TransactionManager.InTransaction);
			foreach (PresentationElement pel in factType.PresentationRolePlayers)
			{
				FactTypeShape factShape = pel as FactTypeShape;
				if (factShape != null)
				{
					bool resize = false;
					switch (constraint.ConstraintType)
					{
						case ConstraintType.InternalUniqueness:
							if (roleChangeOnly)
							{
								resize = factType.RoleCollection.Count == 2;
							}
							else
							{
								resize = true;
							}
							break;
						case ConstraintType.ExternalUniqueness:
						case ConstraintType.DisjunctiveMandatory:
						case ConstraintType.Ring:
						case ConstraintType.Exclusion:
						case ConstraintType.Subset:
						case ConstraintType.Equality:
						case ConstraintType.Frequency:
							resize = true;
							break;
					}
					if (resize)
					{
						foreach (LinkConnectsToNode connection in factShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
						{
							BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
							if (binaryLink != null)
							{
								binaryLink.RipUp();
							}
						}
						factShape.AutoResize();
						factShape.InvalidateRequired(true);
					}
				}
			}
		}
		/// <summary>
		/// Indicate that we support tool tips. Used for showing
		/// detailed information about sticky roles.
		/// </summary>
		public override bool HasToolTip
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Show a tooltip explaining the numbering for sticky multi-column
		/// external constraint roles.
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			string retVal = null;
			ShapeSubField subField = item.SubField;
			RoleSubField roleField;
			if ((null != subField) &&
				(null != (roleField = subField as RoleSubField)))
			{
				IStickyObject stickyObject;
				ExternalConstraintShape shape;
				MultiColumnExternalConstraint mcec;
				Role role;
				if ((null != (stickyObject = ((ORMDiagram)Diagram).StickyObject)) &&
					(null != (shape = stickyObject as ExternalConstraintShape)) &&
					(null != (mcec = shape.AssociatedConstraint as MultiColumnExternalConstraint)) &&
					stickyObject.StickySelectable(role = roleField.AssociatedRole))
				{
					ExternalConstraintConnectAction activeExternalAction = ActiveExternalConstraintConnectAction;
					if ((activeExternalAction == null) ||
						(-1 == activeExternalAction.GetActiveRoleIndex(role)))
					{
						MultiColumnExternalConstraintRoleSequenceMoveableCollection sequences = (mcec as MultiColumnExternalConstraint).RoleSequenceCollection;
						int sequenceCount = sequences.Count;
						string formatString = ResourceStrings.SetConstraintStickyRoleTooltipFormatString;
						for (int i = 0; i < sequenceCount; ++i)
						{
							MultiColumnExternalConstraintRoleSequence sequence = sequences[i];
							int roleIndex = sequence.RoleCollection.IndexOf(role);
							if (roleIndex != -1)
							{
								string current = string.Format(CultureInfo.InvariantCulture, formatString, i + 1, roleIndex + 1);
								if (retVal == null)
								{
									retVal = current;
								}
								else
								{
									retVal = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}", retVal, current);
								}
							}
						}
					}
				}
			}
			return retVal;
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
					switch (Shell.OptionsPage.CurrentObjectifiedFactDisplayShape)
					{
						case Shell.ObjectifiedFactDisplayShape.HardRectangle:
							useShape = CustomFoldRectangleShapeGeometry.ShapeGeometry;
							break;
						case Shell.ObjectifiedFactDisplayShape.SoftRectangle:
						default:
							useShape = CustomFoldRoundedRectangleShapeGeometry.ShapeGeometry;
							break;
					}
					return useShape;
				}
				else
				{
					// Just draw a rectangle if the fact IS NOT objectified
					return CustomFactTypeShapeGeometry.ShapeGeometry;
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
			Debug.Assert(element is Role
					|| (element is ObjectType && ((ObjectType)element).NestedFactType == AssociatedFactType)
					|| (element is ReadingOrder && ((ReadingOrder)element).FactType == AssociatedFactType)
					|| (element is RoleValueConstraint && ((RoleValueConstraint)element).Role.FactType == AssociatedFactType)
				);
			if (element is ReadingOrder)
			{
				foreach (ShapeElement shape in this.RelativeChildShapes)
				{
					ReadingShape readingShape = shape as ReadingShape;
					if (readingShape != null && !readingShape.IsRemoved)
					{
						return false;
					}
				}
			}
			else if (element is Role)
			{
				Role role = element as Role;
				foreach (PresentationElement pElement in role.FactType.PresentationRolePlayers)
				{
					FactTypeShape fts = pElement as FactTypeShape;
					if (fts != null)
					{
						if (fts.DisplayRoleNames == DisplayRoleNames.Off)
						{
							return false;
						}
					}
				}
				return !string.IsNullOrEmpty(role.Name);
			}
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
		/// Makes an ObjectifiedFactTypeNameShape, ReadingShape, RoleNameShape, or ValueConstraintShape a
		/// relative child element.
		/// </summary>
		/// <param name="childShape">The ShapeElement to get the ReleationshipType for.</param>
		/// <returns>RelationshipType.Relative</returns>
		protected override RelationshipType ChooseRelationship(ShapeElement childShape)
		{
			Debug.Assert(childShape is ObjectifiedFactTypeNameShape || childShape is ReadingShape || childShape is ValueConstraintShape || childShape is RoleNameShape);
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
			public HeaderDescriptor(ModelElement wrapElement, TypeConverter converter)
				: base(wrapElement.GetComponentName(), new Attribute[] { })
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
					this.CreatePropertyDescriptor(metaDir.FindMetaAttribute(FactTypeShape.DisplayRoleNamesMetaAttributeGuid), this), 
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
		#region Accessibility Settings
		/// <summary>
		/// Get the accessible name for a shape field
		/// </summary>
		public override string GetFieldAccessibleName(ShapeField field)
		{
			// UNDONE: Localize these values
			ConstraintShapeField constraintField;
			if (null != (constraintField = field as ConstraintShapeField))
			{
				return "Constraints";
			}
			else if (field is RolesShapeField)
			{
				return "Roles";
			}
			return base.GetFieldAccessibleName(field);
		}
		/// <summary>
		/// Get the accessible value for a shape field
		/// </summary>
		public override string GetFieldAccessibleValue(ShapeField field)
		{
			ConstraintShapeField constraintField;
			if (null != (constraintField = field as ConstraintShapeField))
			{
				ConstraintDisplayPosition position = constraintField.DisplayPosition;
				return ORMShapeModel.SingletonResourceManager.GetString("ConstraintDisplayPosition." + position.ToString());
			}
			return base.GetFieldAccessibleValue(field);
		}
		#endregion // Accessibility Settings
		#region ICustomShapeFolding implementation
		/// <summary>
		/// Implements ICustomShapeFolding.CalculateConnectionPoint
		/// </summary>
		/// <param name="oppositeShape">The opposite shape we're connecting to</param>
		/// <returns>The point to connect to. May be internal to the object, or on the boundary.</returns>
		protected PointD CalculateConnectionPoint(NodeShape oppositeShape)
		{
			ObjectTypeShape objectShape;
			FactTypeShape factShape;
			ExternalConstraintShape constraintShape;
			ValueConstraintShape rangeShape;
			FactType factType = null;
			ObjectType objectType = null;
			int factRoleCount = 0;
			int roleIndex = -1;
			bool attachBeforeRole = false; // If true, attach before roleIndex, not in the middle of it
			if (null != (factShape = oppositeShape as FactTypeShape))
			{
				FactType oppositeFactType = factShape.AssociatedFactType;
				if (oppositeFactType != null)
				{
					factType = AssociatedFactType;
					objectType = oppositeFactType.NestingType;
				}
			}
			else if (null != (objectShape = oppositeShape as ObjectTypeShape))
			{
				factType = AssociatedFactType;
				objectType = objectShape.AssociatedObjectType;
			}
			else if (null != (rangeShape = oppositeShape as ValueConstraintShape))
			{
				factType = AssociatedFactType;
				RoleMoveableCollection factRoles = DisplayedRoleOrder;
				factRoleCount = factRoles.Count;
				roleIndex = factRoles.IndexOf(((RoleValueConstraint)rangeShape.AssociatedValueConstraint).Role);
			}
			else if (null != (constraintShape = oppositeShape as ExternalConstraintShape))
			{
				IConstraint constraint = constraintShape.AssociatedConstraint;
				factType = AssociatedFactType;
				if (factType != null)
				{
					SingleColumnExternalConstraint scec;
					MultiColumnExternalConstraint mcec = null;
					IList factConstraints = null;
					IList<Role> roles = null;
					if (null != (scec = constraint as SingleColumnExternalConstraint))
					{
						factConstraints = scec.GetElementLinks(SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
					}
					else if (null != (mcec = constraint as MultiColumnExternalConstraint))
					{
						factConstraints = mcec.GetElementLinks(MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
					}
					if (factConstraints != null)
					{
						int factConstraintCount = factConstraints.Count;
						for (int i = 0; i < factConstraintCount; ++i)
						{
							IFactConstraint factConstraint = (IFactConstraint)factConstraints[i];
							if (object.ReferenceEquals(factConstraint.FactType, factType))
							{
								roles = factConstraint.RoleCollection;
								break;
							}
						}
						if (roles != null)
						{
							ExternalConstraintRoleBarDisplay displayOption = OptionsPage.CurrentExternalConstraintRoleBarDisplay;
							if (displayOption == ExternalConstraintRoleBarDisplay.AnyRole)
							{
								return GetAbsoluteConstraintAttachPoint(constraint);
							}
							else
							{
								RoleMoveableCollection factRoles;
								int roleCount = roles.Count;
								int role1Index = 1;
								switch (roleCount)
								{
									case 1:
										factRoles = DisplayedRoleOrder;
										factRoleCount = factRoles.Count;
										roleIndex = factRoles.IndexOf(roles[0]);
										break;
									case 2:
										{
											Role role0 = roles[0];
											Role role1 = roles[role1Index];
											if (mcec != null && object.ReferenceEquals(role0, role1))
											{
												// Handle overlapping role sequences
												goto case 1;
											}
											else if (displayOption == ExternalConstraintRoleBarDisplay.AdjacentRoles)
											{
												mcec = null;
												goto default;
											}
											else
											{
												factRoles = DisplayedRoleOrder;
												factRoleCount = factRoles.Count;
												int index1 = factRoles.IndexOf(role0);
												int index2 = factRoles.IndexOf(role1);
												if (Math.Abs(index1 - index2) > 1)
												{
													mcec = null;
													goto default;
												}
												roleIndex = (index1 + index2 + 1) / 2;
												attachBeforeRole = true;
											}
										}
										break;
									default:
										if (mcec != null)
										{
											// Handle overlapping role sequences. If the correct mode is
											// not on, the GetAbsoluteConstraintAttachPoint will not find
											// the correct point because the role bar is not visible.
											Role role0 = roles[0];
											Role role1 = null;
											bool haveThirdRole = false;
											for (int i = 1; i < roleCount; ++i)
											{
												Role testRole = roles[i];
												if (role1 == null)
												{
													if (!object.ReferenceEquals(testRole, role0))
													{
														role1Index = i;
														role1 = testRole;
													}
												}
												else if (!object.ReferenceEquals(testRole, role0) && !object.ReferenceEquals(testRole, role1))
												{
													haveThirdRole = true;
													break;
												}
											}
											if (!haveThirdRole)
											{
												mcec = null;
												if (role1 == null)
												{
													goto case 1;
												}
												else
												{
													goto case 2;
												}
											}
										}
										return GetAbsoluteConstraintAttachPoint(constraint);
								}
							}
						}
					}
				}
			}
			if (factType != null && objectType != null)
			{
				RoleMoveableCollection roles = DisplayedRoleOrder;
				factRoleCount = roles.Count;
				Role role = null;
				ORMDiagram parentDiagram = (ORMDiagram)Diagram;
				int firstIndex = -1;
				int bestIndex = -1;
				RolePlayerLink firstLinkShape = null;
				RolePlayerLink linkShape = null;
				bool recordMatch = false;
				Dictionary<FactTypeShape, Collection<RolePlayerLink>> linksDictionary = ConnectedLinksContextDictionary;
				Collection<RolePlayerLink> processedLinks = null;
				for (int i = 0; i < factRoleCount; ++i)
				{
					// UNDONE: MSBUG Note that this where the data passed to DoFoldToShape
					// is insufficient. Unless we're given the specific link object
					// we're dealing with, there is no way to tell which role we're
					// on when the role player is shared by multiple roles. The hack
					// solution is to simply guess, but this is also not completely accurate
					// because we can end up position the lines in the wrong place. This is
					// OK for display, but does not work if we ever attach anything to the lines
					// or make them selectable. The code here includes the RolePlayerLink.RolePlayerRemoving
					// and the RolePlayeLink.HasBeenConnected properties.
					role = roles[i];
					IList rolePlayerLinks = role.GetElementLinks(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
					if (rolePlayerLinks.Count != 0)
					{
						ObjectTypePlaysRole rolePlayerLink = (ObjectTypePlaysRole)rolePlayerLinks[0];
						if (object.ReferenceEquals(rolePlayerLink.RolePlayer, objectType))
						{
							if (firstIndex == -1)
							{
								firstIndex = i;
								firstLinkShape = linkShape = parentDiagram.FindShapeForElement<RolePlayerLink>(rolePlayerLink);
							}
							else
							{
								// Now we need to decide which link to take. First priority is any link that
								// has not yet been connected. Second priority is the ones that have not been connected
								// in this transaction. If there are multiple links, then store them in the
								// transaction context for this shape.
								recordMatch = true;
								if (linkShape.HasBeenConnected)
								{
									RolePlayerLink testLinkShape = parentDiagram.FindShapeForElement<RolePlayerLink>(rolePlayerLink);
									if (!testLinkShape.HasBeenConnected)
									{
										bestIndex = i;
										linkShape = testLinkShape;
										break;
									}
									if (bestIndex == -1)
									{
										EnsureLinksDictionary(ref linksDictionary, ref processedLinks);
										if (processedLinks.Contains(linkShape))
										{
											bestIndex = i;
											linkShape = testLinkShape;
										}
										else
										{
											bestIndex = firstIndex;
										}
									}
									else if (processedLinks.Contains(linkShape))
									{
										bestIndex = i;
										linkShape = testLinkShape;
									}
								}
								else
								{
									if (bestIndex == -1)
									{
										bestIndex = firstIndex;
									}
									break;
								}
							}
						}
					}
				}
				if (bestIndex == -1)
				{
					if (linkShape == null)
					{
						// UNDONE: This is here to stop a subtypelink from an objectified fact
						// from crashing. However, if there is also a role-player link to the
						// supertype then the subtype link will overdraw the other line. This
						// all comes back to the same problem: the framework needs to provide
						// the link instance we're trying to connect.
						return PointD.Empty; // Signals a standard attach to the edge of the shape
					}
					// There was only one match, we don't have to record anything
					bestIndex = firstIndex;
					linkShape.HasBeenConnected = true;
				}
				else if (!linkShape.HasBeenConnected)
				{
					linkShape.HasBeenConnected = true;
					if (recordMatch)
					{
						EnsureLinksDictionary(ref linksDictionary, ref processedLinks);
						processedLinks.Add(linkShape);
					}
				}
				else if (processedLinks != null)
				{
					if (processedLinks.Contains(linkShape))
					{
						// They've all been recorded, empty the collection and revert to the first
						processedLinks.Clear();
						bestIndex = firstIndex;
						processedLinks.Add(firstLinkShape);
					}
					else if (recordMatch)
					{
						processedLinks.Add(linkShape);
					}
				}
				roleIndex = bestIndex;
			}
			if (roleIndex != -1)
			{
				PointD objCenter = oppositeShape.AbsoluteCenter;
				RectangleD factBox = myRolesShapeField.GetBounds(this); // This finds the role box for both objectified and simple fact types
				factBox.Offset(AbsoluteBoundingBox.Location);

				// Decide whether top or bottom works best
				double finalY = (Math.Abs(objCenter.Y - factBox.Top) <= Math.Abs(objCenter.Y - factBox.Bottom)) ? factBox.Top : factBox.Bottom;

				// Find the left/right position
				double roleWidth = factBox.Width / factRoleCount;
				double finalX = factBox.Left + roleWidth * (roleIndex + (attachBeforeRole ? 0 : .5));

				if (!attachBeforeRole)
				{
					// If we're the first or last (or both) role, then
					// prefer an edge attach point.
					PointD testCenter = PointD.Empty;
					if (factRoleCount == 1)
					{
						testCenter = factBox.Center;
					}
					else if (roleIndex == 0)
					{
						if (objCenter.X < factBox.Left)
						{
							testCenter = new PointD(factBox.Left + roleWidth * .5, factBox.Center.Y);
						}
					}
					else if (roleIndex == (factRoleCount - 1))
					{
						if (objCenter.X > factBox.Right)
						{
							testCenter = new PointD(factBox.Right - roleWidth * .5, factBox.Center.Y);
						}
					}
					if (!testCenter.IsEmpty)
					{
						// Compare the slope to a single role box height/width to see
						// if we should connect to the edge or the top/bottom
						double run = objCenter.X - testCenter.X;
						if (!VGConstants.FuzzZero(run, VGConstants.FuzzDistance))
						{
							double slope = (objCenter.Y - testCenter.Y) / run;
							if (Math.Abs(slope) < (factBox.Height / roleWidth))
							{
								finalY = testCenter.Y;
								// The line coming in is flatter than the line
								// across opposite corners of the role box,
								// connect to the left/right edge
								if (factRoleCount == 1)
								{
									finalX = (objCenter.X < factBox.Left) ? factBox.Left : factBox.Right;
								}
								else if (roleIndex == 0)
								{
									finalX = factBox.Left;
								}
								else if (roleIndex == (factRoleCount - 1))
								{
									finalX = factBox.Right;
								}
							}
						}

					}
				}
				return new PointD(finalX, finalY);
			}
			return AbsoluteCenter;
		}
		PointD ICustomShapeFolding.CalculateConnectionPoint(NodeShape oppositeShape)
		{
			return CalculateConnectionPoint(oppositeShape);
		}
		// Code to track which links have already been returned during this walk
		private static object ConnectedLinksContextDictionaryKey = new object();
		private Dictionary<FactTypeShape, Collection<RolePlayerLink>> ConnectedLinksContextDictionary
		{
			get
			{
				Store store = Store;
				return store.TransactionActive ? (Dictionary<FactTypeShape, Collection<RolePlayerLink>>)store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ConnectedLinksContextDictionaryKey] : null;
			}
			set
			{
				Debug.Assert(Store.TransactionActive, "Link connections require context dictionary");
				Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[ConnectedLinksContextDictionaryKey] = value;
			}
		}
		private void EnsureLinksDictionary(ref Dictionary<FactTypeShape, Collection<RolePlayerLink>> linksDictionary, ref Collection<RolePlayerLink> processedLinks)
		{
			if (processedLinks == null)
			{
				if (linksDictionary == null)
				{
					Debug.Assert(ConnectedLinksContextDictionary == null, "Attempt to retrieve the dictionary  before creating it");
					linksDictionary = new Dictionary<FactTypeShape, Collection<RolePlayerLink>>();
					linksDictionary[this] = processedLinks = new Collection<RolePlayerLink>();
					ConnectedLinksContextDictionary = linksDictionary;
				}
				else
				{
					if (!linksDictionary.TryGetValue(this, out processedLinks))
					{
						linksDictionary[this] = processedLinks = new Collection<RolePlayerLink>();
					}
				}
			}
		}
		#endregion // ICustomShapeFolding implementation
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			TooFewReadingRolesError tooFew;
			TooManyReadingRolesError tooMany;
			FactTypeRequiresReadingError noReading;
			FactTypeRequiresInternalUniquenessConstraintError noUniqueness;
			NMinusOneError nMinusOne;
			RolePlayerRequiredError requireRolePlayer;
			FactType fact;
			ConstraintDuplicateNameError constraintNameError;
			FactTypeDuplicateNameError factTypeNameError;
			ImpliedInternalUniquenessConstraintError implConstraint;
			Reading reading = null;
			InternalUniquenessConstraint activateConstraint = null;
			bool selectConstraintOnly = false;
			bool activateNamePropertyAfterSelect = false;
			bool addActiveRoles = false;
			bool retVal = true;
			if (null != (tooFew = error as TooFewReadingRolesError))
			{
				reading = tooFew.Reading;
			}
			else if (null != (tooMany = error as TooManyReadingRolesError))
			{
				reading = tooMany.Reading;
			}
			else if (null != (noReading = error as FactTypeRequiresReadingError))
			{
				fact = noReading.FactType;
				Debug.Assert(fact != null);
				ORMReadingEditorToolWindow window = ORMDesignerPackage.ReadingEditorWindow;
				window.Show();
				window.ActivateReading(fact);
			}
			else if (null != (noUniqueness = error as FactTypeRequiresInternalUniquenessConstraintError))
			{
				fact = noUniqueness.FactType;
				Store theStore = fact.Store;
				using (Transaction tran = theStore.TransactionManager.BeginTransaction(ResourceStrings.AddInternalConstraintTransactionName))
				{
					InternalUniquenessConstraint theConstraint = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(theStore);
					activateConstraint = theConstraint;
					theConstraint.FactType = fact;
					tran.Commit();
				}
			}
			else if (null != (nMinusOne = error as NMinusOneError))
			{
				activateConstraint = nMinusOne.Constraint;
				addActiveRoles = true;
			}
			else if (null != (requireRolePlayer = error as RolePlayerRequiredError))
			{
				ORMDiagram ormDiagram = Diagram as ORMDiagram;
				Role role = requireRolePlayer.Role;
				DiagramClientView clientView = ormDiagram.ActiveDiagramView.DiagramClientView;
				DiagramItem diagramItem = new DiagramItem(this, RolesShape, new RoleSubField(role));
				clientView.Selection.Set(diagramItem);
				RoleConnectAction connectAction = ormDiagram.RoleConnectAction;
				connectAction.ChainMouseAction(GetAbsoluteRoleAttachPoint(role), clientView, false);
			}
			else if (null != (factTypeNameError = error as FactTypeDuplicateNameError))
			{
				ActivateNameProperty(factTypeNameError.FactTypeCollection[0]);
			}
			else if (null != (constraintNameError = error as ConstraintDuplicateNameError))
			{
				// We'll get here if one of the constraints we own has a duplicate name
				IList internalConstraints = constraintNameError.InternalConstraintCollection;
				int internalConstraintsCount = internalConstraints.Count;
				fact = AssociatedFactType;
				for (int i = 0; i < internalConstraintsCount; ++i)
				{
					InternalConstraint ic = (InternalConstraint)internalConstraints[i];
					if (ic.FactType == fact)
					{
						switch (ic.Constraint.ConstraintType)
						{
							case ConstraintType.InternalUniqueness:
								activateConstraint = (InternalUniquenessConstraint)ic;
								selectConstraintOnly = true;
								activateNamePropertyAfterSelect = true;
								break;
							case ConstraintType.SimpleMandatory:
								Diagram.ActiveDiagramView.DiagramClientView.Selection.Set(new DiagramItem(this, RolesShape, new RoleSubField(ic.RoleCollection[0])));
								ActivateNameProperty(ic);
								break;
						}
						break;
					}
				}
			}
			else if (null != (implConstraint = error as ImpliedInternalUniquenessConstraintError))
			{
				IServiceProvider provider;
				IVsUIShell shell;
				if (null != (provider = (Store as IORMToolServices).ServiceProvider) &&
					null != (shell = (IVsUIShell)provider.GetService(typeof(IVsUIShell))))
				{
					Guid g = new Guid();
					int pnResult;
					shell.ShowMessageBox(0, ref g, ResourceStrings.PackageOfficialName,
						ResourceStrings.ImpliedInternalConstraintFixMessage,
						"", 0, OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
						OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_QUERY, 0, out pnResult);
					if (pnResult == (int)DialogResult.Yes)
					{
						implConstraint.FactType.RemoveImpliedInternalUniquenessConstraints();
					}
				}
			}
			else
			{
				retVal = false;
			}

			if (reading != null)
			{
				// Open the reading editor window and activate the reading  
				ORMReadingEditorToolWindow window = ORMDesignerPackage.ReadingEditorWindow;
				window.Show();
				window.ActivateReading(reading);

			}
			else if (activateConstraint != null)
			{
				ConstraintShapeField targetSubfield = null;
				switch (ConstraintDisplayPosition)
				{
					case ConstraintDisplayPosition.Top:
						targetSubfield = myTopConstraintShapeField;
						break;
					case ConstraintDisplayPosition.Bottom:
						targetSubfield = myBottomConstraintShapeField;
						break;
				}
				Debug.Assert(targetSubfield != null);
				if (targetSubfield != null)
				{
					ORMDiagram ormDiagram = Diagram as ORMDiagram;
					DiagramClientView clientView = ormDiagram.ActiveDiagramView.DiagramClientView;
					DiagramItem diagramItem = new DiagramItem(this, targetSubfield, new ConstraintSubField(activateConstraint));
					clientView.Selection.Set(diagramItem);
					if (!selectConstraintOnly)
					{
						InternalUniquenessConstraintConnectAction connectAction = ormDiagram.InternalUniquenessConstraintConnectAction;
						ActiveInternalUniquenessConstraintConnectAction = connectAction;
						if (addActiveRoles)
						{
							RoleMoveableCollection roleColl = activateConstraint.RoleCollection;
							if (roleColl.Count != 0)
							{
								IList<Role> selectedRoles = connectAction.SelectedRoleCollection;
								foreach (Role r in roleColl)
								{
									selectedRoles.Add(r);
								}
							}
						}
						this.Invalidate(true);
						connectAction.ChainMouseAction(this, activateConstraint, clientView);
					}
					else if (activateNamePropertyAfterSelect)
					{
						ActivateNameProperty(activateConstraint);
					}
				}
			}
			return retVal;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
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
		/// Get a diagram item for an internal uniqueness constraint on the associated fact.
		/// A diagram item is used to represent selection in a DiagramClientView.
		/// </summary>
		public DiagramItem GetDiagramItem(InternalUniquenessConstraint constraint)
		{
			return new DiagramItem(this, (ConstraintDisplayPosition == ConstraintDisplayPosition.Top) ? myTopConstraintShapeField : myBottomConstraintShapeField, new ConstraintSubField(constraint));
		}
		/// <summary>
		/// Get a diagram item for a role on the associated fact.
		/// A diagram item is used to represent selection in a DiagramClientView.
		/// </summary>
		public DiagramItem GetDiagramItem(Role role)
		{
			return new DiagramItem(this, myRolesShapeField, new RoleSubField(role));
		}
		/// <summary>
		/// Gets the attach point of the specific constraint within this shape.
		/// </summary>
		/// <param name="constraint">The constraint to find the location of.</param>
		/// <returns></returns>
		public PointD GetAbsoluteConstraintAttachPoint(IConstraint constraint)
		{
			RectangleD rect = RectangleD.Empty;
			PointD retVal = PointD.Empty;
			if (constraint.ConstraintStorageStyle == ConstraintStorageStyle.InternalConstraint)
			{
				// We know the attach location of an internal constraint
				ConstraintDisplayPosition position = ConstraintDisplayPosition;
				WalkConstraintBoxes(
					(position == ConstraintDisplayPosition.Top) ? myTopConstraintShapeField.GetBounds(this) : myBottomConstraintShapeField.GetBounds(this),
					position,
					delegate(ref ConstraintBox constraintBox)
					{
						if (constraintBox.FactConstraint.Constraint == constraint)
						{
							rect = constraintBox.Bounds;
							return false;
						}
						return true;
					});
				rect.Offset(Bounds.Location);
				retVal = rect.Center;
			}
			else
			{
				// We have to work a little harder for an external because we
				// don't know where it is located. Also, we want the attach point
				// to be in the middle of the first/last active roles, not just
				// in the raw middle.
				ConstraintDisplayPosition position = ConstraintDisplayPosition.Top;
				ConstraintShapeField constraintShapeField = myTopConstraintShapeField;
				for (int iShape = 0; iShape < 2; ++iShape)
				{
					WalkConstraintBoxes(
						constraintShapeField.GetBounds(this),
						position,
						delegate(ref ConstraintBox constraintBox)
						{
							if (constraintBox.FactConstraint.Constraint == constraint)
							{
								rect = constraintBox.Bounds;
								ConstraintBoxRoleActivity[] roles = constraintBox.ActiveRoles;
								int roleCount = roles.Length;
								int firstActive = -1;
								int lastActive = -1;
								int leadingNotInBox = 0;
								int trailingNotInBox = 0;
								if (roleCount == 0)
								{
									if (roles == PreDefinedConstraintBoxRoleActivities_FullySpanning)
									{
										roleCount = AssociatedFactType.RoleCollection.Count;
										firstActive = 0;
										lastActive = roleCount - 1;
									}
								}
								else if (roleCount > 0)
								{
									for (int i = 0; i < roleCount; ++i)
									{
										switch (roles[i])
										{
											case ConstraintBoxRoleActivity.Active:
												if (firstActive == -1)
												{
													firstActive = i;
												}
												lastActive = i;
												break;
											case ConstraintBoxRoleActivity.NotInBox:
												if (firstActive == -1)
												{
													++leadingNotInBox;
												}
												else
												{
													++trailingNotInBox;
												}
												break;
										}
									}
								}
								Debug.Assert(firstActive != -1 && lastActive != -1);
								if (firstActive > 0)
								{
									rect.X += (firstActive - leadingNotInBox) * RoleBoxWidth;
									if (leadingNotInBox == 0 && firstActive != 0)
									{
										rect.Width -= firstActive * RoleBoxWidth;
									}
								}
								int trailingCount = roleCount - trailingNotInBox - lastActive - 1;
								if (trailingCount > 0)
								{
									rect.Width -= trailingCount * RoleBoxWidth;
								}
								return false;
							}
							return true;
						});
					if (!rect.IsEmpty)
					{
						rect.Offset(Bounds.Location);
						retVal = rect.Center;
						retVal.Y += (position == ConstraintDisplayPosition.Bottom) ? ExternalConstraintBarCenterAdjust : -ExternalConstraintBarCenterAdjust;
						break;
					}
					position = ConstraintDisplayPosition.Bottom;
					constraintShapeField = myBottomConstraintShapeField;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Gets the attach point of the specific role within this shape.
		/// </summary>
		/// <param name="role">The role to locate</param>
		/// <returns>An absolute point</returns>
		public PointD GetAbsoluteRoleAttachPoint(Role role)
		{
			Debug.Assert(role.FactType == AssociatedFactType);
			FactType factType = role.FactType;
			RoleMoveableCollection roles = DisplayedRoleOrder;
			int roleCount = roles.Count;
			if (roleCount != 0)
			{
				RectangleD roleBounds = myRolesShapeField.GetBounds(this);
				roleBounds.Offset(Bounds.Location);
				int roleIndex = roles.IndexOf(role);
				return new PointD((roleBounds.Width / roleCount) * (roleIndex + .5f) + roleBounds.X, roleBounds.Height / 2 + roleBounds.Y);
			}
			return default(PointD);
		}
		/// <summary>
		/// The center of the roles box
		/// </summary>
		public PointD RolesCenter
		{
			get
			{
				return new PointD(Size.Width / 2, RolesPosition);
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
		/// Static property set when an internal constraint is being created. The active
		/// connection is used to track which roles are highlighted.
		/// </summary>
		public static InternalUniquenessConstraintConnectAction ActiveInternalUniquenessConstraintConnectAction
		{
			get
			{
				return myActiveInternalUniquenessConstraintConnectAction;
			}
			set
			{
				myActiveInternalUniquenessConstraintConnectAction = value;
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
			return constraint.PreferredIdentifierFor != null;
		}
		#endregion // FactTypeShape specific
		#region Shape display update rules
		[RuleOn(typeof(Objectification), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchToNestedFact : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				// Part1: Make sure the fact shape is visible on any diagram where the
				// corresponding nestingType is displayed
				foreach (PresentationElement pel in nestingType.PresentationRolePlayers)
				{
					ObjectTypeShape objectShape = pel as ObjectTypeShape;
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
					Role playedRole = modelLink.PlayedRoleCollection;
					SubtypeFact subType = playedRole.FactType as SubtypeFact;
					if (subType != null)
					{
						foreach (object obj in subType.PresentationRolePlayers)
						{
							SubtypeLink subtypeLink = obj as SubtypeLink;
							if (subtypeLink != null)
							{
								ORMDiagram currentDiagram = subtypeLink.Diagram as ORMDiagram;
								NodeShape factShape = currentDiagram.FindShapeForElement(nestedFactType) as NodeShape;
								if (factShape != null)
								{
									if (object.ReferenceEquals(playedRole, subType.SupertypeRole))
									{
										subtypeLink.ToShape = factShape;
									}
									else
									{
										Debug.Assert(object.ReferenceEquals(playedRole, subType.SubtypeRole));
										subtypeLink.FromShape = factShape;
									}
								}
								else
								{
									// Backup. Should only happen if the FixupDiagram call in part 1
									// did not add the fact type.
									subtypeLink.Remove();
								}
							}
						}
					}
					else
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
				}

				// Part3: Remove object type shapes from the diagram. Do this before
				// adding the labels to the objectified fact types so clearing the role
				// players doesn't blow the labels away. Also, FixUpDiagram will attempt
				// to fix up the existing shapes instead of creating new ones if the existing
				// ones are not cleared away.
				nestingType.PresentationRolePlayers.Clear();

				// Part4: Resize the fact type wherever it is displayed and add the
				// labels for the fact type display.
				foreach (PresentationElement pel in nestedFactType.PresentationRolePlayers)
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						shape.AutoResize();
						Diagram.FixUpDiagram(nestedFactType, nestingType);
					}
				}
			}
		}
		[RuleOn(typeof(Objectification), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddShapeRulePriority)]
		private class SwitchFromNestedFact : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				Objectification link = e.ModelElement as Objectification;
				FactType nestedFactType = link.NestedFactType;
				ObjectType nestingType = link.NestingType;

				bool nestingTypeRemoved = nestingType.IsRemoved;
				bool nestedFactTypeRemoved = nestedFactType.IsRemoved;

				if (nestingTypeRemoved && nestedFactTypeRemoved)
				{
					return;
				}

				// Part1: Remove any existing presentation elements for the object type.
				// This removes all of the ObjectifiedTypeNameShape objects
				if (!nestingTypeRemoved)
				{
					if (nestedFactTypeRemoved)
					{
						// If we're just removing the fact, then we need to readd the normal object shape
						Store store = nestingType.Store;
						IList pels = nestingType.PresentationRolePlayers;
						int pelCount = pels.Count;
						for (int i = pelCount - 1; i >= 0; --i)
						{
							ObjectifiedFactTypeNameShape oldShape;
							ORMDiagram shapeDiagram;
							if (null != (oldShape = pels[i] as ObjectifiedFactTypeNameShape) &&
								!oldShape.IsRemoved &&
								null != (shapeDiagram = oldShape.Diagram as ORMDiagram))
							{
								ObjectTypeShape newShape = ObjectTypeShape.CreateObjectTypeShape(store);
								shapeDiagram.NestedChildShapes.Add(newShape);
								newShape.AbsoluteBounds = oldShape.AbsoluteBoundingBox;
								oldShape.Remove();
								newShape.Associate(nestingType);
								newShape.AutoResize();
							}
						}
					}
					else
					{
						nestingType.PresentationRolePlayers.Clear();
					}
				}

				// Part2: Resize the fact type wherever it is displayed, and make sure
				// the object type is made visible in the same location.
				ORMModel nestingTypeModel = nestingTypeRemoved ? null : nestingType.Model;
				if (!nestedFactTypeRemoved)
				{
					foreach (PresentationElement pel in nestedFactType.PresentationRolePlayers)
					{
						FactTypeShape factShape = pel as FactTypeShape;
						if (factShape != null)
						{
							factShape.AutoResize();
							if (!nestingTypeRemoved)
							{
								ORMDiagram currentDiagram = factShape.Diagram as ORMDiagram;
								NodeShape objectShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
								if (objectShape == null)
								{
									Diagram.FixUpDiagram(nestingTypeModel, nestingType);
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
					}
				}

				// Part3: Move any links from the fact type to the object type
				if (!nestingTypeRemoved)
				{
					foreach (ObjectTypePlaysRole modelLink in nestingType.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid))
					{
						Role playedRole = modelLink.PlayedRoleCollection;
						SubtypeFact subType = playedRole.FactType as SubtypeFact;
						if (subType != null)
						{
							if (nestedFactTypeRemoved)
							{
								Diagram.FixUpDiagram(nestingTypeModel, subType);
							}
							else
							{
								foreach (object obj in subType.PresentationRolePlayers)
								{
									SubtypeLink subtypeLink = obj as SubtypeLink;
									if (subtypeLink != null)
									{
										ORMDiagram currentDiagram = subtypeLink.Diagram as ORMDiagram;
										NodeShape objShape = currentDiagram.FindShapeForElement(nestingType) as NodeShape;
										if (objShape != null)
										{
											if (object.ReferenceEquals(playedRole, subType.SupertypeRole))
											{
												subtypeLink.ToShape = objShape;
											}
											else
											{
												Debug.Assert(object.ReferenceEquals(playedRole, subType.SubtypeRole));
												subtypeLink.FromShape = objShape;
											}
										}
										else
										{
											// Backup. Should only happen if the FixupDiagram call in part 1
											// did not add the fact type.
											subtypeLink.Remove();
										}
									}
								}
							}
						}
						else
						{
							if (nestedFactTypeRemoved)
							{
								Diagram.FixUpDiagram(nestingTypeModel, modelLink);
							}
							else
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
				if (attributeId == ConstraintDisplayPositionMetaAttributeGuid)
				{
					FactTypeShape factTypeShape = e.ModelElement as FactTypeShape;
					if (!factTypeShape.IsRemoved)
					{
						foreach (LinkConnectsToNode connection in factTypeShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
						{
							BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
							if (binaryLink != null)
							{
								binaryLink.RipUp();
							}
						}
						factTypeShape.AutoResize();
						factTypeShape.InvalidateRequired(true);
					}
				}
			}
		}
		#endregion // ConstraintDisplayPositionChangeRule class
		#region ExternalConstraintShapeChangeRule class
		/// <summary>
		/// Class to force the external constraint link bars to redraw and/or reposition
		/// when an external constraint shape is moved.
		/// </summary>
		[RuleOn(typeof(ExternalConstraintShape), FireTime = TimeToFire.LocalCommit)]
		private class ExternalConstraintShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == ExternalConstraintShape.AbsoluteBoundsMetaAttributeGuid)
				{
					ExternalConstraintShape externalConstraintShape = e.ModelElement as ExternalConstraintShape;
					if (!externalConstraintShape.IsRemoved)
					{
						foreach (LinkConnectsToNode connection in externalConstraintShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
						{
							BinaryLinkShape binaryLink = connection.Link as BinaryLinkShape;
							if (binaryLink != null && !binaryLink.IsRemoved)
							{
								Debug.Assert(binaryLink.ToShape == externalConstraintShape);
								FactTypeShape factShape = binaryLink.FromShape as FactTypeShape;
								if (factShape != null)
								{
									IFactConstraint factConstraint = binaryLink.ModelElement as IFactConstraint;
									IList<Role> roles;
									if (null != (factConstraint = binaryLink.ModelElement as IFactConstraint) &&
										null != (roles = factConstraint.RoleCollection))
									{
										ExternalConstraintRoleBarDisplay displayOption = OptionsPage.CurrentExternalConstraintRoleBarDisplay;
										bool constraintBarVisible;
										RoleMoveableCollection factRoles = null;
										switch (roles.Count)
										{
											case 0:
												constraintBarVisible = false;
												break;
											case 1:
												constraintBarVisible = displayOption == ExternalConstraintRoleBarDisplay.AnyRole;
												break;
											case 2:
												{
													// Handle possible duplicates in IFactConstraint.RoleCollection
													Role role0 = roles[0];
													Role role1 = roles[1];
													if (object.ReferenceEquals(role0, role1))
													{
														goto case 1;
													}
													else if (displayOption == ExternalConstraintRoleBarDisplay.SplitRoles)
													{
														factRoles = factShape.DisplayedRoleOrder;
														constraintBarVisible = Math.Abs(factRoles.IndexOf(role0) - factRoles.IndexOf(role1)) > 1;
													}
													else
													{
														constraintBarVisible = true;
													}
												}
												break;
											default:
												constraintBarVisible = true;
												break;
										}
										if (constraintBarVisible)
										{
											bool resized = false;
											if (displayOption == ExternalConstraintRoleBarDisplay.AnyRole)
											{
												if (factRoles == null)
												{
													factRoles = factConstraint.FactType.RoleCollection;
												}
												if (factRoles.Count == 2)
												{
													// If AnyRole is on, then a binary fact can compress the display
													// of an external constraint role. Moving the connection point from
													// the top to the bottom will require more space and change the
													// size of the fact shape.
													factShape.AutoResize();
													resized = true;
												}
											}
											// All links going into the FactTypeShape are
											// suspect, get rid of all of them.
											foreach (LinkConnectsToNode factConnection in factShape.GetElementLinks(LinkConnectsToNode.NodesMetaRoleGuid))
											{
												BinaryLinkShape binaryLinkToFact = factConnection.Link as BinaryLinkShape;
												if (binaryLinkToFact != null && !object.ReferenceEquals(binaryLink, binaryLinkToFact))
												{
													binaryLinkToFact.RipUp();
												}
											}
											if (!resized)
											{
												factShape.UpdateRolesPosition(SizeD.Empty);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // ExternalConstraintShapeChangeRule class
		#region FactTypeShapeChangeRule class
		/// <summary>
		/// Keep relative child elements a fixed distance away from the fact
		/// when the shape changes.
		/// </summary>
		[RuleOn(typeof(FactTypeShape), FireTime = TimeToFire.LocalCommit, Priority = DiagramFixupConstants.ResizeParentRulePriority)]
		private class FactTypeShapeChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == FactTypeShape.AbsoluteBoundsMetaAttributeGuid)
				{
					FactTypeShape parentShape = e.ModelElement as FactTypeShape;
					RectangleD oldBounds = (RectangleD)e.OldValue;
					RectangleD newBounds = (RectangleD)e.NewValue;
					SizeD oldSize = oldBounds.Size;
					SizeD newSize = newBounds.Size;
					double xChange = newSize.Width - oldSize.Width;
					double yChange = newSize.Height - oldSize.Height;
					bool checkX = !VGConstants.FuzzZero(xChange, VGConstants.FuzzDistance);
					bool checkY = !VGConstants.FuzzZero(yChange, VGConstants.FuzzDistance);
					if (checkX || checkY)
					{
						ShapeElementMoveableCollection childShapes = parentShape.RelativeChildShapes;
						int childCount = childShapes.Count;
						if (childCount != 0)
						{
							for (int i = 0; i < childCount; ++i)
							{
								bool changeBounds = false;
								PointD change = default(PointD);
								NodeShape childShape = childShapes[i] as NodeShape;
								if (childShape != null)
								{
									RectangleD childBounds = childShape.AbsoluteBoundingBox;
									if (checkX && (childBounds.Left > (newBounds.Right - xChange)))
									{
										change.X = xChange;
										changeBounds = true;
									}
									if (checkY && (childBounds.Top > (newBounds.Bottom - yChange)))
									{
										change.Y = yChange;
										changeBounds = true;
									}
									if (changeBounds)
									{
										childBounds.Offset(change);
										childShape.AbsoluteBounds = childBounds;
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion // FactTypeShapeChangeRule class
		#endregion // Shape display update rules
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
		public static new void AttachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaAttributeInfo attributeInfo = dataDirectory.FindMetaAttribute(InternalUniquenessConstraint.ModalityMetaAttributeGuid);
			eventDirectory.ElementAttributeChanged.Add(attributeInfo, new ElementAttributeChangedEventHandler(InternalConstraintChangedEvent));

			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Add(classInfo, new ElementAddedEventHandler(PreferredIdentifierAddedEvent));
			eventDirectory.ElementRemoved.Add(classInfo, new ElementRemovedEventHandler(PreferredIdentifierRemovedEvent));
		}
		/// <summary>
		/// Detach event handlers from the store
		/// </summary>
		public static new void DetachEventHandlers(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventDirectory = store.EventManagerDirectory;

			MetaAttributeInfo attributeInfo = dataDirectory.FindMetaAttribute(InternalUniquenessConstraint.ModalityMetaAttributeGuid);
			eventDirectory.ElementAttributeChanged.Remove(attributeInfo, new ElementAttributeChangedEventHandler(InternalConstraintChangedEvent));

			MetaClassInfo classInfo = dataDirectory.FindMetaRelationship(EntityTypeHasPreferredIdentifier.MetaRelationshipGuid);
			eventDirectory.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(PreferredIdentifierAddedEvent));
			eventDirectory.ElementRemoved.Remove(classInfo, new ElementRemovedEventHandler(PreferredIdentifierRemovedEvent));
		}
		/// <summary>
		/// Update the link displays when the modality of an internal uniqueness constraint changes
		/// </summary>
		private static void InternalConstraintChangedEvent(object sender, ElementAttributeChangedEventArgs e)
		{
			InternalUniquenessConstraint iuc = e.ModelElement as InternalUniquenessConstraint;
			if (iuc != null && !iuc.IsRemoved)
			{
				FactType factType = iuc.FactType;
				if (factType != null && !factType.IsRemoved)
				{
					foreach (PresentationElement pel in factType.PresentationRolePlayers)
					{
						ShapeElement shape = pel as ShapeElement;
						if (shape != null)
						{
							shape.Invalidate(true);
						}
					}
				}
			}
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers being added.
		/// </summary>
		public static void PreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			InternalUniquenessConstraint constraint;
			FactType factType;
			if ((null != (constraint = link.PreferredIdentifier as InternalUniquenessConstraint)) &&
				(null != (factType = constraint.FactType)))
			{
				foreach (PresentationElement pel in factType.PresentationRolePlayers)
				{
					FactTypeShape factShape = pel as FactTypeShape;
					if (factShape != null)
					{
						factShape.Invalidate(true);
					}
				}
			}
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers being removed.
		/// </summary>
		public static void PreferredIdentifierRemovedEvent(object sender, ElementRemovedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;
			InternalUniquenessConstraint constraint;
			FactType factType;
			if ((null != (constraint = link.PreferredIdentifier as InternalUniquenessConstraint)) &&
				!constraint.IsRemoved &&
				(null != (factType = constraint.FactType)))
			{
				foreach (PresentationElement pel in factType.PresentationRolePlayers)
				{
					FactTypeShape factShape = pel as FactTypeShape;
					if (factShape != null)
					{
						factShape.Invalidate(true);
					}
				}
			}
		}
		#endregion // Store Event Handlers
		#region CustomFactTypeShapeGeometry
		/// <summary>
		/// We're using a custom shape geometry to move the focus line in. The border
		/// width has been adjusted to make it easier to select the fact shape, but we
		/// don't want to add extra width for the normal selection outline.
		/// </summary>
		private class CustomFactTypeShapeGeometry : CustomFoldRectangleShapeGeometry
		{
			public new static readonly ShapeGeometry ShapeGeometry = new CustomFactTypeShapeGeometry();
			protected override double GetFocusIndicatorInsideMargin(IGeometryHost geometryHost)
			{
				return FocusIndicatorInsideMargin;
			}
			/// <summary>
			/// Override GetPerimeterBoundingBox to ignore outline pen when the outline is not displayed
			/// UNDONE: MSBUG The framework should check HasOutline before using the outline pen
			/// </summary>
			protected override RectangleD GetPerimeterBoundingBox(IGeometryHost geometryHost)
			{
				if (geometryHost.GeometryHasOutline)
				{
					return base.GetPerimeterBoundingBox(geometryHost);
				}
				return geometryHost.GeometryBoundingBox;
			}
		}
		#endregion // CustomFactTypeShapeGeometry
	}
	#endregion // FactTypeShape class
	#region ObjectifiedFactTypeNameShape class
	/// <summary>
	/// A specialized display of the nesting type as a relative
	/// child element of an objectified fact type
	/// </summary>
	public partial class ObjectifiedFactTypeNameShape : IModelErrorActivation
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
		/// Move a new name label above the parent fact type shape
		/// </summary>
		public override void PlaceAsChildOf(NodeShape parent)
		{
			AutoResize();
			SizeD size = Size;
			Location = new PointD(0, -1.5 * size.Height);
		}
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError for DataTypeNotSpecifiedError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			ObjectTypeDuplicateNameError duplicateName;
			bool retVal = true;
			if (null != (duplicateName = error as ObjectTypeDuplicateNameError))
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
		#region ObjectNameTextField class
		/// <summary>
		/// Create a text field that will correctly display objectified type names
		/// </summary>
		/// <returns></returns>
		protected override AutoSizeTextField CreateAutoSizeTextField()
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
				string baseText = base.GetDisplayText(parentShape);
				ObjectType objectType;
				string formatString;
				if (null != (objectType = parentShape.ModelElement as ObjectType) &&
					objectType.IsIndependent)
				{
					formatString = ResourceStrings.ObjectifiedFactTypeNameShapeIndependentFormatString;
				}
				else
				{
					formatString = ResourceStrings.ObjectifiedFactTypeNameShapeStandardFormatString;
				}
				return string.Format(CultureInfo.InvariantCulture, formatString, baseText);
			}
		}
		#endregion // ObjectNameTextField class
	}
	#endregion // ObjectifiedFactTypeNameShape class
}
