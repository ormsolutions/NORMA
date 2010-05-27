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

// Turn this on to always draw objectifications (even if they are implied)
//#define ALWAYS_DRAW_OBJECTIFICATIONS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region FactTypeShape class
	public partial class FactTypeShape : ICustomShapeFolding, IModelErrorActivation, IProvideConnectorShape, IProxyDisplayProvider, IConfigureAsChildShape, IDynamicColorGeometryHost, IDynamicColorAlsoUsedBy, IConfigureableLinkEndpoint
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
				if (roleActivity != PreDefinedConstraintBoxRoleActivities_FullySpanning && roleActivity != PreDefinedConstraintBoxRoleActivities_AntiSpanning)
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
					return myActiveRoles == PreDefinedConstraintBoxRoleActivities_FullySpanning;
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
					return myActiveRoles == PreDefinedConstraintBoxRoleActivities_AntiSpanning;
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
							IConstraint constraint = myFactConstraint.Constraint;
							ORMModel model = constraint.Model;
							retVal = !ModelError.HasErrors(constraint as ModelElement, ModelErrorUses.DisplayPrimary, (null != model) ? model.ModelErrorDisplayFilter : null);
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
			private sealed class ConstraintBoxComparer : Comparer<ConstraintBox>
			{
				private ConstraintBoxComparer()
				{
				}
				public sealed override int Compare(ConstraintBox c1, ConstraintBox c2)
				{
					if (c1.FactConstraint == c2.FactConstraint)
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
				}

				public static readonly ConstraintBoxComparer Instance = new ConstraintBoxComparer();
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
				Array.Sort(boxes, 0, fullCount, ConstraintBoxComparer.Instance);
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
				LinkedElementCollection<RoleBase> roles = EnsureDisplayOrderCollection();
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
		/// <summary>
		/// Reverse the displayed role order
		/// </summary>
		public void ReverseDisplayedRoleOrder()
		{
			using (Transaction t = Store.TransactionManager.BeginTransaction(ResourceStrings.ReverseRoleOrderTransactionName))
			{
				// Earlier code explicitly populated the collection in reverse, but we don't
				// have any rules to handle adds to this collection that are not related to corresponding
				// changes in the RoleCollection of the AssociatedFactType. Therefore, the reverse add
				// triggered nothing to force a rebind or redraw.
				// UNDONE: This collection is auto-managed. We should add a rule to make sure that noone
				// else messes with it
				LinkedElementCollection<RoleBase> displayRoles = EnsureDisplayOrderCollection();
				int moveFrom = displayRoles.Count - 1;
				for (int i = 0; i < moveFrom; ++i)
				{
					displayRoles.Move(moveFrom, i);
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		private LinkedElementCollection<RoleBase> EnsureDisplayOrderCollection()
		{
			LinkedElementCollection<RoleBase> displayRoles = RoleDisplayOrderCollection;
			if (displayRoles.Count == 0)
			{
				FactType fact = AssociatedFactType;
				if (fact != null)
				{
					LinkedElementCollection<RoleBase> nativeRoles = fact.RoleCollection;
					int nativeRoleCount = nativeRoles.Count;
					for (int i = 0; i < nativeRoleCount; ++i)
					{
						displayRoles.Add(nativeRoles[i].Role);
					}
				}
			}
			return displayRoles;
		}
		/// <summary>
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			// Make sure the factType shape is prepared to display as a unary
			FactType factType;
			Role unaryRole;
			if (null != (factType = AssociatedFactType) &&
				null != (unaryRole = factType.UnaryRole))
			{
				// Create a RoleDisplayOrder for Unary FactTypes.
				LinkedElementCollection<RoleBase> displayRoles = RoleDisplayOrderCollection;
				switch (displayRoles.Count)
				{
					case 0:
						// The RoleDisplayOrder is empty, so we don't need to do anything.
						break;
					case 1:
						// We already have only one role in the RoleDisplayOrder, so all
						// we have to do is make sure it is the right one.
						if (displayRoles[0] != unaryRole)
						{
							displayRoles[0] = unaryRole;
						}
						return;
					default:
						// We have more than one role in the RoleDisplayOrder, so we
						// have to clear it.
						displayRoles.Clear();
						break;
				}
				displayRoles.Add(unaryRole);
			}
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
		}
		/// <summary>
		/// Retrieve an editable version of the <see cref="DisplayedRoleOrder"/> property. Editing
		/// DisplayedRoleOrder directly can change the role order in the associated FactType. Using
		/// this method to retrieve the collection ensures it will only be modified on the shape.
		/// Do not call this until you are sure you need to modify the collection.
		/// </summary>
		public LinkedElementCollection<RoleBase> GetEditableDisplayRoleOrder()
		{
			return EnsureDisplayOrderCollection();
		}
		/// <summary>
		/// Gets the currently displayed order of the roles in the fact type.
		/// If there is not a custom display order then it will return the default
		/// role collection.
		/// </summary>
		public LinkedElementCollection<RoleBase> DisplayedRoleOrder
		{
			get
			{
				LinkedElementCollection<RoleBase> alternateOrder = RoleDisplayOrderCollection;
				return (alternateOrder.Count == 0) ? AssociatedFactType.RoleCollection : alternateOrder;
			}
		}
		/// <summary>
		/// Gets the reading order that matches the currently displayed order of the
		/// fact that is passed in.
		/// </summary>
		/// <returns>The matching ReadingOrder or null if one does not exist.</returns>
		public ReadingOrder FindMatchingReadingOrder()
		{
			return FindMatchingReadingOrder(false);
		}
		/// <summary>
		/// Gets the reading order that matches the currently displayed order of the
		/// fact that is passed in.
		/// </summary>
		/// <param name="reverseOrder">Find the reverse reading order</param>
		/// <returns>The matching ReadingOrder or null if one does not exist.</returns>
		public ReadingOrder FindMatchingReadingOrder(bool reverseOrder)
		{
			IList<RoleBase> roleOrder = DisplayedRoleOrder;
			if (reverseOrder ^ (DisplayOrientation == DisplayOrientation.VerticalRotatedLeft))
			{
				RoleBase[] reverseRoleOrder = new RoleBase[roleOrder.Count];
				roleOrder.CopyTo(reverseRoleOrder, 0);
				Array.Reverse(reverseRoleOrder);
				roleOrder = reverseRoleOrder;
			}
			return AssociatedFactType.FindMatchingReadingOrder(roleOrder);
		}
		#region RoleDisplayOrderChangedRule
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// </summary>
		private static void RoleDisplayOrderChangedRule(RolePlayerOrderChangedEventArgs e)
		{
			Role role;
			if (null != (role = e.CounterpartRolePlayer as Role))
			{
				Partition sourcePartition = e.SourceElement.Partition;
				foreach (PresentationElement pElem in PresentationViewsSubject.GetPresentation(role.FactType))
				{
					FactTypeShape factShape;
					if (null != (factShape = pElem as FactTypeShape) && factShape.Partition == sourcePartition)
					{
						foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(factShape))
						{
							binaryLink.RecalculateRoute();
						}
						SizeD oldSize = factShape.Size;
						factShape.AutoResize();
						if (oldSize == factShape.Size)
						{
							factShape.InvalidateRequired(true);
						}
					}
				}
			}
		}
		#endregion // RoleDisplayOrderChangedRule
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
		/// <param name="attachPosition">The position the constraints are attached to the role</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes produced by WalkConstraintBoxes.</param>
		protected void WalkConstraintBoxes(ShapeField shapeField, ConstraintAttachPosition attachPosition, VisitConstraintBox boxUser)
		{
			WalkConstraintBoxes(shapeField.GetBounds(this), attachPosition, boxUser);
		}

		/// <summary>
		/// Determines the bounding boxes of all the constraints associated with the FactType,
		/// then passes those bounding boxes into the delegate.  Specifically, it will pass in
		/// the bouding box, the number of roles in the box, a boolean[] telling the method
		/// which roles are active for the constraint, and the constraint type.
		/// </summary>
		/// <param name="fullBounds">The bounds the rectangles need to fit in.  Pass RectangleD.Empty if unknown.</param>
		/// <param name="attachPosition">The position the constraints are attached to the role</param>
		/// <param name="boxUser">The VisitConstraintBox delegate that will use the ConstraintBoxes 
		/// produced by WalkConstraintBoxes.</param>
		protected void WalkConstraintBoxes(RectangleD fullBounds, ConstraintAttachPosition attachPosition, VisitConstraintBox boxUser)
		{
			ConstraintDisplayPosition displayPosition = ConstraintDisplayPosition.Top;
			DisplayOrientation orientation = DisplayOrientation;
			bool isVertical = orientation != DisplayOrientation.Horizontal;
			switch (attachPosition)
			{
				case ConstraintAttachPosition.Left:
					if (!isVertical)
					{
						return;
					}
					if (orientation == DisplayOrientation.VerticalRotatedRight)
					{
						displayPosition = ConstraintDisplayPosition.Bottom;
					}
					break;
				case ConstraintAttachPosition.Right:
					if (!isVertical)
					{
						return;
					}
					if (orientation == DisplayOrientation.VerticalRotatedLeft)
					{
						displayPosition = ConstraintDisplayPosition.Bottom;
					}
					break;
				case ConstraintAttachPosition.Top:
					if (isVertical)
					{
						return;
					}
					break;
				case ConstraintAttachPosition.Bottom:
					if (isVertical)
					{
						return;
					}
					displayPosition = ConstraintDisplayPosition.Bottom;
					break;
			}
			// initialize variables
			bool reverseVertical = orientation == DisplayOrientation.VerticalRotatedLeft;
			FactType parentFact = AssociatedFactType;
			LinkedElementCollection<RoleBase> factRoles = DisplayedRoleOrder;
			int factRoleCount = factRoles.Count;
			if (fullBounds.IsEmpty)
			{
				fullBounds = isVertical ?
					new RectangleD(0, 0, 0, RoleBoxWidth) :
					new RectangleD(0, 0, RoleBoxWidth, 0);
			}
			else
			{
				float adjustBy = StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width / -2;
				if (isVertical)
				{
					fullBounds.Inflate(0d, adjustBy);
				}
				else
				{
					fullBounds.Inflate(adjustBy, 0d);
				}
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
					if (factConstraint.Constraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint)
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
								if (constraintRoles[j] == testRole)
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
										if (constraintRoles[j] == testRole)
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


				// Hide the internal if we're walking a unary
				if (parentFact.UnaryRole != null)
				{
					constraintBoxes[0].IsHidden = true;
					haveInternals = false;
				}

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
					//    use the center point that we would have if there were no external
					//    bars and call this sufficient.
					int externalsCount = significantConstraintCount - internalsCount;
					double testCenter = isVertical ?
						Location.X + myLeftSpacerShapeField.GetMinimumSize(this).Width + RoleBoxHeight / 2 :
						Location.Y + myTopSpacerShapeField.GetMinimumSize(this).Height + RoleBoxHeight / 2;
					ORMDiagram diagram = Diagram as ORMDiagram;
					if (currentDisplayPosition == ConstraintDisplayPosition.Top)
					{
						testCenter += internalsCount * ConstraintHeight;
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
							ShapeElement constraintShape = diagram.FindShapeForElement((ModelElement)constraintBoxes[i].FactConstraint.Constraint, true);
							if (constraintShape == null)
							{
								// This can happen if the constraint is implied. Implied constraints are not displayed.
								showConstraint = false;
							}
							else
							{
								double constraintCenter = isVertical ? constraintShape.AbsoluteCenter.X : constraintShape.AbsoluteCenter.Y;
								showConstraint = (displayPosition == ConstraintDisplayPosition.Top) ?
									(orientation == DisplayOrientation.VerticalRotatedRight) ? constraintCenter >= testCenter : constraintCenter < testCenter :
									(orientation == DisplayOrientation.VerticalRotatedRight) ? constraintCenter < testCenter : constraintCenter >= testCenter;
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
				double constraintWidth;
				if (isVertical)
				{
					constraintWidth = fullBounds.Height / (double)factRoleCount;
					fullBounds.Width = constraintHeight;
				}
				else
				{
					constraintWidth = fullBounds.Width / (double)factRoleCount;
					fullBounds.Height = constraintHeight;
				}
				int iBox;
				int incr;
				if (currentDisplayPosition == ((orientation == DisplayOrientation.VerticalRotatedRight) ? ConstraintDisplayPosition.Top : ConstraintDisplayPosition.Bottom))
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
						RectangleD bounds = fullBounds;

						ConstraintBoxRoleActivity[] activeRoles = box.ActiveRoles;
						if (activeRoles.Length == 2) // Weed out fully spanning and antispanning
						{
							if (nextCompressedConstraint == iBox)
							{
								nextCompressedConstraint = -1;
								int inactiveSide = (activeRoles[0] == ConstraintBoxRoleActivity.Inactive) ? 0 : 1;
								if (reverseVertical)
								{
									if (inactiveSide == 1)
									{
										bounds.Y += constraintWidth;
									}
								}
								else if (inactiveSide == 0)
								{
									if (isVertical)
									{
										bounds.Y += constraintWidth;
									}
									else
									{
										bounds.X += constraintWidth;
									}
								}
								box.CompressBinaryActiveRoles();
								if (isVertical)
								{
									bounds.Height -= constraintWidth;
									bounds.X = lastCompressedBottom;
								}
								else
								{
									bounds.Width -= constraintWidth;
									bounds.Y = lastCompressedBottom;
								}
								skippedRow = true;
							}
							else
							{
								int inactiveSide = (activeRoles[0] == ConstraintBoxRoleActivity.Inactive) ? 0 : 1;
								for (int j = iBox + incr; j >= 0 && j < significantConstraintCount; j += incr)
								{
									ConstraintBoxRoleActivity[] testActiveRoles = constraintBoxes[j].ActiveRoles;
									if (testActiveRoles.Length == 2 && testActiveRoles[inactiveSide] == ConstraintBoxRoleActivity.Active)
									{
										nextCompressedConstraint = j;
										break;
									}
								}
								if (nextCompressedConstraint != -1)
								{
									lastCompressedBottom = isVertical ? bounds.X : bounds.Y;
									if (reverseVertical)
									{
										if (inactiveSide == 1)
										{
											bounds.Y += constraintWidth;
										}
									}
									else if (inactiveSide == 0)
									{
										if (isVertical)
										{
											bounds.Y += constraintWidth;
										}
										else
										{
											bounds.X += constraintWidth;
										}
									}
									box.CompressBinaryActiveRoles();
									if (isVertical)
									{
										bounds.Height -= constraintWidth;
									}
									else
									{
										bounds.Width -= constraintWidth;
									}
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
						else if (isVertical)
						{
							fullBounds.Offset(constraintHeight, 0);
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
						if (isVertical)
						{
							fullBounds.Offset(constraintHeight, 0);
						}
						else
						{
							fullBounds.Offset(0, constraintHeight);
						}
					}
				}
			}
		}
		/// <summary>
		/// Convert a <see cref="ConstraintAttachPosition"/> into a <see cref="ConstraintDisplayPosition"/> depending
		/// on the current <see cref="DisplayOrientation"/> value.
		/// </summary>
		protected ConstraintDisplayPosition DisplayPositionFromAttachPosition(ConstraintAttachPosition attachPosition)
		{
			bool isBottom = false;
			switch (attachPosition)
			{
				//case ConstraintAttachPosition.Top:
				case ConstraintAttachPosition.Bottom:
					isBottom = true;
					break;
				case ConstraintAttachPosition.Left:
					isBottom = DisplayOrientation == DisplayOrientation.VerticalRotatedRight;
					break;
				case ConstraintAttachPosition.Right:
					isBottom = DisplayOrientation == DisplayOrientation.VerticalRotatedLeft;
					break;
			}
			return isBottom ? ConstraintDisplayPosition.Bottom : ConstraintDisplayPosition.Top;
		}
		/// <summary>
		/// Get the constraint shape field that corresponds to the internal constraints
		/// </summary>
		private ConstraintShapeField InternalConstraintShapeField
		{
			get
			{
				bool useTop = ConstraintDisplayPosition == ConstraintDisplayPosition.Top;
				switch (DisplayOrientation)
				{
					case DisplayOrientation.Horizontal:
						return useTop ? myTopConstraintShapeField : myBottomConstraintShapeField;
					case DisplayOrientation.VerticalRotatedRight:
						return useTop ? myRightConstraintShapeField : myLeftConstraintShapeField;
					//case DisplayOrientation.VerticalRotatedLeft:
					default:
						Debug.Assert(DisplayOrientation == DisplayOrientation.VerticalRotatedLeft);
						return useTop ? myLeftConstraintShapeField : myRightConstraintShapeField;
				}
			}
		}
		#endregion // WalkConstraintBoxes implementation
		#region Size Constants
		// Note: These values should NOT agree with the InitialSize/InitialWidth specified in the model file.
		// Changing the height to match or the width such that a muliple matches the default may adversely affect
		// the ORMBaseShape.MaintainRelativeShapeOffsetsForBoundsChange function, where changing from the default
		// size is ignored wrt/repositioning relative shapes.
		private const double RoleBoxHeight = 0.11;
		private const double RoleBoxWidth = 0.16;
		private const double NestedFactHorizontalMargin = 0.09;
		private const double NestedFactVerticalMargin = 0.056;
		private static readonly SizeD NestedFactHorizontalMarginSize = new SizeD(NestedFactHorizontalMargin, NestedFactVerticalMargin);
		private static readonly SizeD NestedFactVerticalMarginSize = new SizeD(NestedFactVerticalMargin, NestedFactHorizontalMargin);
		private const double ConstraintHeight = 0.07;
		private const double ExternalConstraintBarCenterAdjust = ConstraintHeight / 5;
		private const double BorderMargin = 0.025;
		private static readonly SizeD BorderHorizontalMarginSize = new SizeD(0, BorderMargin);
		private static readonly SizeD BorderVerticalMarginSize = new SizeD(BorderMargin, 0);
		private const double FocusIndicatorInsideMargin = .019;
		#endregion // Size Constants
		#region SpacerShapeField : ShapeField
		/// <summary>
		/// Creates a shape to properly align the other shapefields within the FactTypeShape.
		/// </summary>
		private sealed class SpacerShapeField : ShapeField
		{
			private readonly bool myIsVertical;

			/// <summary>
			/// A shape field to add differing amounts of padding depending on
			/// whether a fact type shape is objectified or not.
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field</param>
			/// <param name="isVertical">Spacer is used when the fact type shape is displayed with a vertical orientation</param>
			public SpacerShapeField(string fieldName, bool isVertical)
				: base(fieldName)
			{
				DefaultFocusable = false;
				DefaultSelectable = false;
				DefaultVisibility = false;
				myIsVertical = isVertical;
			}

			/// <summary>
			/// Returns <see cref="NestedFactHorizontalMarginSize"/> if <see cref="FactTypeShape.ShouldDrawObjectified"/> is
			/// <see langword="true"/> for <paramref name="parentShape"/>, otherwise <see cref="BorderHorizontalMarginSize"/>.
			/// </summary>
			public sealed override SizeD GetMinimumSize(ShapeElement parentShape)
			{
				FactTypeShape factTypeShape = (FactTypeShape)parentShape;
				bool isVertical = myIsVertical;
				bool isObjectified = factTypeShape.ShouldDrawObjectified;
				double offset = ((factTypeShape.DisplayOrientation == DisplayOrientation.Horizontal) ^ isVertical) ? // verticalMatchesShape
					(isObjectified ? NestedFactVerticalMargin : BorderMargin) :
					(isObjectified ? NestedFactHorizontalMargin : BorderMargin);
				return isVertical ?
					new SizeD(offset, 0d) :
					new SizeD(0d, offset);
			}

			// Nothing to paint for the spacer. So, no DoPaint override needed.
		}
		#endregion // SpacerShapeField class
		#region ConstraintShapeField : ShapeField
		/// <summary>
		/// Indicate which side of a <see cref="FactTypeShape"/> a <see cref="ConstraintShapeField"/> is attached to
		/// </summary>
		protected enum ConstraintAttachPosition
		{
			/// <summary>
			/// Attach to the top of the role boxes
			/// </summary>
			Top,
			/// <summary>
			/// Attach to the right of the role boxes
			/// </summary>
			Right,
			/// <summary>
			/// Attach to the bottom of the role boxes
			/// </summary>
			Bottom,
			/// <summary>
			/// Attach to the left of the role boxes
			/// </summary>
			Left,
		}
		private sealed class ConstraintShapeField : ShapeField
		{
			private readonly ConstraintAttachPosition myAttachPosition;

			/// <summary>
			/// Create a new constraint shape field
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field, forwarded to base class.</param>
			/// <param name="attachPosition">The position for attaching this shape field, relative to the role boxes</param>
			public ConstraintShapeField(string fieldName, ConstraintAttachPosition attachPosition)
				: base(fieldName)
			{
				DefaultFocusable = true;
				DefaultSelectable = true;
				DefaultVisibility = true;
				myAttachPosition = attachPosition;
			}
			/// <summary>
			/// Accessor property for position provided to constructor
			/// </summary>
			public ConstraintAttachPosition AttachPosition
			{
				get
				{
					return myAttachPosition;
				}
			}
			/// <summary>
			/// Find the constraint sub shape at this location
			/// </summary>
			/// <param name="point">The point being hit-tested.</param>
			/// <param name="parentShape">The current FactTypeShape that the mouse is over.</param>
			/// <param name="diagramHitTestInfo">The DiagramHitTestInfo to which the ConstraintSubShapField
			/// will be added if the mouse is over it.</param>
			public sealed override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					AttachPosition,
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
			public sealed override int GetAccessibleChildCount(ShapeElement parentShape)
			{
				int total = 0;
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					AttachPosition,
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
			public sealed override ShapeSubField GetAccessibleChild(ShapeElement parentShape, int index)
			{
				ShapeSubField retVal = null;
				((FactTypeShape)parentShape).WalkConstraintBoxes(
					this,
					AttachPosition,
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
			/// Gets the minimum <see cref="SizeD"/> of this <see cref="ConstraintShapeField"/>.
			/// </summary>
			/// <param name="parentShape">The <see cref="FactTypeShape"/> that this <see cref="ConstraintShapeField"/> is associated with.</param>
			/// <returns>The minimum <see cref="SizeD"/> of this <see cref="ConstraintShapeField"/>.</returns>
			public sealed override SizeD GetMinimumSize(ShapeElement parentShape)
			{
				FactTypeShape factTypeShape = (FactTypeShape)parentShape;
				bool isVertical = IsVertical;
				if ((factTypeShape.DisplayOrientation == DisplayOrientation.Horizontal) ^ !isVertical)
				{
					return SizeD.Empty;
				}
				double minY = double.MaxValue;
				double maxY = double.MinValue;
				bool wasVisited = false;
				factTypeShape.WalkConstraintBoxes(
					RectangleD.Empty,
					AttachPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						wasVisited = true;
						RectangleD bounds = constraintBox.Bounds;
						if (isVertical)
						{
							minY = Math.Min(minY, bounds.Left);
							maxY = Math.Max(maxY, bounds.Right);
						}
						else
						{
							minY = Math.Min(minY, bounds.Top);
							maxY = Math.Max(maxY, bounds.Bottom);
						}
						return true;
					});
				return isVertical ?
					new SizeD(wasVisited ? maxY - minY : 0, RolesShape.GetMinimumSize(parentShape).Height) :
					new SizeD(RolesShape.GetMinimumSize(parentShape).Width, wasVisited ? maxY - minY : 0);
			}
			/// <summary>
			/// Return true if the constraint box is displayed vertically
			/// </summary>
			private bool IsVertical
			{
				get
				{
					ConstraintAttachPosition attachPosition = myAttachPosition;
					switch (attachPosition)
					{
						case ConstraintAttachPosition.Left:
						case ConstraintAttachPosition.Right:
							return true;
					}
					return false;
				}
			}
			/// <summary>
			/// Constraint fields are only visible if the orientation matches the parent shape
			/// </summary>
			/// <param name="parentShape">The <see cref="FactTypeShape"/> that this <see cref="ConstraintShapeField"/> is associated with.</param>
			/// <returns>True if the orientation of the shape matches the orientation of the constraint field</returns>
			public override bool GetVisible(ShapeElement parentShape)
			{
				return ((parentShape as FactTypeShape).DisplayOrientation == DisplayOrientation.Horizontal) ^ IsVertical;
			}
			/// <summary>
			/// Paints the constraints.
			/// </summary>
			/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
			/// <param name="parentShape">ConstraintShapeField to draw to.</param>
			public sealed override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				if (null == Utility.ValidateStore(parentShape.Store))
				{
					return;
				}
				FactTypeShape factShape = (FactTypeShape)parentShape;
				DisplayOrientation orientation = factShape.DisplayOrientation;
				bool isVertical = IsVertical;
				if ((orientation == DisplayOrientation.Horizontal) ^ !isVertical)
				{
					return;
				}
				bool reverseVertical = orientation == DisplayOrientation.VerticalRotatedLeft;
				Graphics g = e.Graphics;
				HighlightedShapesCollection highlightedShapes = null;
				SelectedShapesCollection selection = null;
				ConstraintSubField testSubField = null;
				DiagramItem testSelect = null;
				DiagramClientView view = e.View;
				bool factShapeHighlighted = false;
				if (view != null)
				{
					highlightedShapes = view.HighlightedShapes;
					selection = view.Selection;
					if (highlightedShapes != null)
					{
						foreach (DiagramItem item in highlightedShapes)
						{
							if (factShape == item.Shape)
							{
								if (item.SubField == null)
								{
									factShapeHighlighted = true;
									break;
								}
							}
						}
					}
				}
				StyleSet styleSet = factShape.StyleSet;
				Pen alethicConstraintPen = styleSet.GetPen(InternalFactConstraintPen);
				Pen deonticConstraintPen = styleSet.GetPen(DeonticInternalFactConstraintPen);
				IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, IConstraint>[] dynamicColorProviders = ((IFrameworkServices)parentShape.Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, IConstraint>>();
				float gap = alethicConstraintPen.Width;
				ConstraintAttachPosition attachPosition = AttachPosition;
				ConstraintDisplayPosition position = factShape.DisplayPositionFromAttachPosition(attachPosition);
				ORMDiagram diagram = (ORMDiagram)factShape.Diagram;
				StyleSet diagramStyleSet = diagram.StyleSet;

				factShape.WalkConstraintBoxes(
					this,
					attachPosition,
					delegate(ref ConstraintBox constraintBox)
					{
						bool isInternalConstraint = constraintBox.ConstraintType == ConstraintType.InternalUniqueness;

						//default variables
						IFactConstraint factConstraint = constraintBox.FactConstraint;
						IConstraint currentConstraint = factConstraint.Constraint;
						RectangleF boundsF = RectangleD.ToRectangleF(constraintBox.Bounds);
						float verticalPos = (isVertical ? boundsF.Left : boundsF.Top) + (float)(ConstraintHeight / 2);
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
								constraintPen.DashStyle = DashStyle.Dot;
							}
						}

						// Draw active constraint highlight
						bool isHighlighted = false;
						bool isSticky = false;
						bool checkDynamicColor = dynamicColorProviders != null;
						if (isInternalConstraint)
						{
							InternalUniquenessConstraintConnectAction activeInternalAction = ActiveInternalUniquenessConstraintConnectAction;
							if (activeInternalAction != null)
							{
								UniquenessConstraint activeInternalConstraint = activeInternalAction.ActiveConstraint;
								UniquenessConstraint targetConstraint = currentConstraint as UniquenessConstraint;
								if (activeInternalConstraint == targetConstraint)
								{
									isSticky = true;
									checkDynamicColor = false;
									constraintPen.Color = diagramStyleSet.GetPen(ORMDiagram.StickyForegroundResource).Color;
								}
							}
						}
						else
						{
							ExternalConstraintShape externalConstraintShape = diagram.StickyObject as ExternalConstraintShape;
							if (externalConstraintShape != null &&
								externalConstraintShape.AssociatedConstraint == currentConstraint)
							{
								checkDynamicColor = false;
								constraintPen.Color = diagramStyleSet.GetPen(ORMDiagram.StickyBackgroundResource).Color;
							}
						}

						if (checkDynamicColor)
						{
							ORMDiagramDynamicColor requestColor = isDeontic ? ORMDiagramDynamicColor.DeonticConstraint : ORMDiagramDynamicColor.Constraint;
							for (int i = 0; i < dynamicColorProviders.Length; ++i)
							{
								Color alternateColor = dynamicColorProviders[i].GetDynamicColor(requestColor, factShape, currentConstraint);
								if (alternateColor != Color.Empty)
								{
									constraintPen.Color = alternateColor;
									break;
								}
							}
						}

						// test for and draw highlights
						if (highlightedShapes != null)
						{
							foreach (DiagramItem item in highlightedShapes)
							{
								if (factShape == item.Shape)
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
						IConstraint constraint = factConstraint.Constraint;
						ORMModel model;
						if (ModelError.HasErrors(constraint as ModelElement, ModelErrorUses.DisplayPrimary, (null != (model = constraint.Model)) ? model.ModelErrorDisplayFilter : null) && isInternalConstraint && !isSticky)
						{
							Brush backBrush;
							if (factShapeHighlighted || isHighlighted || isSticky)
							{
								backBrush = styleSet.GetBrush(ORMDiagram.HighlightedErrorBackgroundResource);
							}
							else
							{
								backBrush = styleSet.GetBrush(ORMDiagram.ErrorBackgroundResource);
							}
							g.FillRectangle(backBrush, boundsF);
						}
						else
						{
							FactType factType;
							if (isHighlighted || isSticky || ModelError.HasErrors(factType = factShape.AssociatedFactType, ModelErrorUses.DisplayPrimary, (null != (model = factType.Model)) ? model.ModelErrorDisplayFilter : null))
							{
								factShape.DrawHighlight(g, boundsF, isSticky ? null : factShape.BackgroundBrushId, null, isSticky, factShapeHighlighted || isHighlighted);
							}
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
							float startPos = isVertical ? (reverseVertical ? boundsF.Bottom : boundsF.Top) : boundsF.Left;
							float endPos = startPos;
							bool drawConstraintPreffered = factShape.ShouldDrawConstraintPreferred(currentConstraint);
							if (constraintBox.IsSpanning || constraintBox.IsAntiSpanning)
							{
								endPos = isVertical ? (reverseVertical ? boundsF.Top : boundsF.Bottom) : boundsF.Right;
								//draw fully spanning constraint
								DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered && constraintPen.DashStyle == startDashStyle, isDeontic && constraintBox.IsSpanning, isVertical, reverseVertical);
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
												constraintPen.DashStyle = DashStyle.Dot;
											}
											//draw constraint
											if (constraintHasDrawn)
											{
												DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered && constraintPen.DashStyle == startDashStyle, isDeontic && !drawCalled, isVertical, reverseVertical);
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
										endPos += reverseVertical ? -roleWidth : roleWidth;
										positionChanged = true;
									}
									else if ((isVertical ? boundsF.Height : boundsF.Width) > roleWidth)
									{
										// this covers BinaryRights when not compressing constraints
										startPos += reverseVertical ? -roleWidth : roleWidth;
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
								if (currentActivity == ConstraintBoxRoleActivity.Active &&
									(reverseVertical ? endPos < startPos : endPos > startPos))
								{
									DrawInternalConstraintLine(g, constraintPen, startPos, endPos, verticalPos, gap, drawConstraintPreffered && constraintPen.DashStyle == startDashStyle, isDeontic && !drawCalled, isVertical, reverseVertical);
								}
							}
						}
						else
						{
							int firstActive = -1;
							int lastActive = -1;
							float targetVertical;
							if (position == ((orientation == DisplayOrientation.VerticalRotatedRight) ? ConstraintDisplayPosition.Top : ConstraintDisplayPosition.Bottom))
							{
								verticalPos += (float)ExternalConstraintBarCenterAdjust;
								targetVertical = isVertical ? boundsF.Left : boundsF.Top;
							}
							else
							{
								verticalPos -= (float)ExternalConstraintBarCenterAdjust;
								targetVertical = isVertical ? boundsF.Right : boundsF.Bottom;
							}
							bool fullySpanning = rolePosToDraw == PreDefinedConstraintBoxRoleActivities_FullySpanning;
							if (fullySpanning)
							{
								numRoles = factConstraint.FactType.RoleCollection.Count;
							}
							float startPos = isVertical ? (reverseVertical ? boundsF.Bottom : boundsF.Top) : boundsF.Left;
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
										float xChange = (firstActive + .5f) * roleWidth;
										if (isVertical)
										{
											float x = startPos + (reverseVertical ? -xChange : xChange);
											g.DrawLine(constraintPen, verticalPos, x, targetVertical, x);
											x += (reverseVertical ? (firstActive - i) : (i - firstActive)) * roleWidth;
											g.DrawLine(constraintPen, verticalPos, x, targetVertical, x);
										}
										else
										{
											float x = startPos + xChange;
											g.DrawLine(constraintPen, x, verticalPos, x, targetVertical);
											x += (i - firstActive) * roleWidth;
											g.DrawLine(constraintPen, x, verticalPos, x, targetVertical);
										}
									}
									lastActive = i;
								}
								else if (firstActive == -1 && currentActivity == ConstraintBoxRoleActivity.NotInBox)
								{
									startPos += reverseVertical ? roleWidth : -roleWidth;
								}
							}
							if (lastActive == firstActive)
							{
								// Draw a box on a single role. This is used only for accessibility
								// cases when the ExternalConstraintRoleBarDisplay is set to AnyRole.
								// This is designed to provide a selectable accessibility object for
								// all constraints associated with a fact.
								float x1;
								float x2;
								if (reverseVertical)
								{
									x1 = startPos - (firstActive + .3f) * roleWidth;
									x2 = x1 - .4f * roleWidth;
								}
								else
								{
									x1 = startPos + (firstActive + .3f) * roleWidth;
									x2 = x1 + .4f * roleWidth;
								}
								if (isVertical)
								{
									g.DrawLine(constraintPen, verticalPos, x1, targetVertical, x1);
									g.DrawLine(constraintPen, verticalPos, x1, verticalPos, x2);
									g.DrawLine(constraintPen, verticalPos, x2, targetVertical, x2);
								}
								else
								{
									g.DrawLine(constraintPen, x1, verticalPos, x1, targetVertical);
									g.DrawLine(constraintPen, x1, verticalPos, x2, verticalPos);
									g.DrawLine(constraintPen, x2, verticalPos, x2, targetVertical);
								}
							}
							else
							{
								if (isVertical)
								{
									if (reverseVertical)
									{
										g.DrawLine(constraintPen, verticalPos, startPos - (firstActive + .5f) * roleWidth, verticalPos, boundsF.Top + (numRoles - lastActive - .5f) * roleWidth);
									}
									else
									{
										g.DrawLine(constraintPen, verticalPos, startPos + (firstActive + .5f) * roleWidth, verticalPos, boundsF.Bottom - (numRoles - lastActive - .5f) * roleWidth);
									}
								}
								else
								{
									g.DrawLine(constraintPen, startPos + (firstActive + .5f) * roleWidth, verticalPos, boundsF.Right - (numRoles - lastActive - .5f) * roleWidth, verticalPos);
								}
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
			/// <param name="vertical">Whether or not to draw the constraint vertical. Inverts x/y meanings of other parameters</param>
			/// <param name="reverseVertical">True if drawing from bottom to top instead of top to bottom</param>
			private static void DrawInternalConstraintLine(Graphics g, Pen pen, float startPos, float endPos, float verticalPos, float gap, bool preferred, bool deontic, bool vertical, bool reverseVertical)
			{
				if (deontic)
				{
					float deonticRadius = (float)(FactTypeShape.ConstraintHeight / 2) - gap;
					float deonticDiameter = deonticRadius + deonticRadius;
					if (vertical)
					{
						if (reverseVertical)
						{
							gap = -gap;
							g.DrawArc(pen, verticalPos - deonticRadius, startPos - deonticDiameter + gap, deonticDiameter, deonticDiameter, 0, 360);
							startPos -= deonticDiameter;
						}
						else
						{
							g.DrawArc(pen, verticalPos - deonticRadius, startPos + gap, deonticDiameter, deonticDiameter, 0, 360);
							startPos += deonticDiameter;
						}
					}
					else
					{
						g.DrawArc(pen, startPos + gap, verticalPos - deonticRadius, deonticDiameter, deonticDiameter, 0, 360);
						startPos += deonticDiameter;
					}
				}
				else if (reverseVertical)
				{
					gap = -gap;
				}
				if (preferred)
				{
					float vAdjust = gap * .75f;
					if (vertical)
					{
						g.DrawLine(pen, verticalPos - vAdjust, startPos + gap, verticalPos - vAdjust, endPos - gap);
						g.DrawLine(pen, verticalPos + vAdjust, startPos + gap, verticalPos + vAdjust, endPos - gap);
					}
					else
					{
						g.DrawLine(pen, startPos + gap, verticalPos - vAdjust, endPos - gap, verticalPos - vAdjust);
						g.DrawLine(pen, startPos + gap, verticalPos + vAdjust, endPos - gap, verticalPos + vAdjust);
					}
				}
				else
				{
					if (vertical)
					{
						g.DrawLine(pen, verticalPos, startPos + gap, verticalPos, endPos - gap);
					}
					else
					{
						g.DrawLine(pen, startPos + gap, verticalPos, endPos - gap, verticalPos);
					}
				}
			}
		}
		#endregion // ConstraintShapeField class
		#region ConstraintSubField class
		private sealed class ConstraintSubField : ORMBaseShapeSubField
		{
			#region Mouse handling
			public sealed override void OnDoubleClick(DiagramPointEventArgs e)
			{
				if (ORMBaseShape.AttemptErrorActivation(e))
				{
					base.OnDoubleClick(e);
					return;
				}
				DiagramClientView clientView = e.DiagramClientView;
				ORMDiagram diagram = clientView.Diagram as ORMDiagram;
				UniquenessConstraint iuc = AssociatedConstraint as UniquenessConstraint;
				if (iuc != null && iuc.IsInternal)
				{
					// Move on to the selection action
					InternalUniquenessConstraintConnectAction iucca = diagram.InternalUniquenessConstraintConnectAction;
					ActiveInternalUniquenessConstraintConnectAction = iucca;
					LinkedElementCollection<Role> roleColl = iuc.RoleCollection;
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
			public sealed override bool SubFieldEquals(object obj)
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
			public sealed override int SubFieldHashCode
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
			public sealed override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// A role sub field is always focusable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public sealed override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
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
			public sealed override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				RectangleD retVal = RectangleD.Empty;
				IConstraint testConstraint = myAssociatedConstraint;
				if (testConstraint != null)
				{
					((FactTypeShape)parentShape).WalkConstraintBoxes(
						parentField,
						((ConstraintShapeField)parentField).AttachPosition,
						delegate(ref ConstraintBox constraintBox)
						{
							if (constraintBox.FactConstraint.Constraint == testConstraint)
							{
								retVal = constraintBox.Bounds;
								return false; // Don't continue, we got our item
							}
							return true;
						});
				}
				if (retVal.IsEmpty)
				{
					retVal = parentField.GetBounds(parentShape);
				}
				return retVal;
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
			#region Accessibility Overrides
			/// <summary>
			/// Defer to the associated constraint for the accessible name
			/// </summary>
			public override string GetAccessibleName(ShapeElement parentShape, ShapeField parentField)
			{
				return TypeDescriptor.GetClassName(myAssociatedConstraint);
			}
			/// <summary>
			/// Defer to the associated constraint for the accessible value
			/// </summary>
			public override string GetAccessibleValue(ShapeElement parentShape, ShapeField parentField)
			{
				return TypeDescriptor.GetComponentName(myAssociatedConstraint);
			}
			#endregion // Accessibility Overrides
		}
		#endregion // ConstraintSubField class
		#region RolesShapeField class
		private sealed class RolesShapeField : ShapeField
		{
			/// <summary>
			/// Construct a default RolesShapeField (Visible, but not selectable or focusable)
			/// </summary>
			/// <param name="fieldName">Non-localized name for the field, forwarded to base class.</param>
			public RolesShapeField(string fieldName)
				: base(fieldName)
			{
				DefaultFocusable = false;
				DefaultSelectable = false;
				DefaultVisibility = true;
			}
			/// <summary>
			/// Find the role sub shape at this location
			/// </summary>
			public sealed override void DoHitTest(PointD point, ShapeElement parentShape, DiagramHitTestInfo diagramHitTestInfo)
			{
				RectangleD fullBounds = GetBounds(parentShape);
				if (fullBounds.Contains(point))
				{
					FactTypeShape parentFactShape = parentShape as FactTypeShape;
					LinkedElementCollection<RoleBase> roles = parentFactShape.DisplayedRoleOrder;
					int roleCount = roles.Count;
					if (roleCount != 0)
					{
						int roleIndex;
						switch (parentFactShape.DisplayOrientation)
						{
							case DisplayOrientation.Horizontal:
								roleIndex = (int)((point.X - fullBounds.Left) * roleCount / fullBounds.Width);
								break;
							case DisplayOrientation.VerticalRotatedRight:
								roleIndex = (int)((point.Y - fullBounds.Top) * roleCount / fullBounds.Height);
								break;
							//case DisplayOrientation.VerticalRotatedLeft:
							default:
								Debug.Assert(parentFactShape.DisplayOrientation == DisplayOrientation.VerticalRotatedLeft);
								roleIndex = (int)((fullBounds.Bottom - point.Y) * roleCount / fullBounds.Height);
								break;

						}
						roleIndex = Math.Min(roleIndex, roleCount - 1); // Deal with potential roundoff error on last role
						diagramHitTestInfo.HitDiagramItem = new DiagramItem(parentShape, this, new RoleSubField(roles[roleIndex]));
					}
				}
			}
			/// <summary>
			/// Return the number of children in this shape field.
			/// Maps to the number of roles on the FactTypeShape
			/// </summary>
			public sealed override int GetAccessibleChildCount(ShapeElement parentShape)
			{
				// Use the DisplayedRoleOrder here to account for unary binarization
				return (parentShape as FactTypeShape).DisplayedRoleOrder.Count;
			}
			/// <summary>
			/// Return the RoleSubField corresponding to the role at the requested index
			/// </summary>
			public sealed override ShapeSubField GetAccessibleChild(ShapeElement parentShape, int index)
			{
				return new RoleSubField((parentShape as FactTypeShape).DisplayedRoleOrder[index]);
			}
			/// <summary>
			/// Gets the minimum <see cref="SizeD"/> of this <see cref="RolesShapeField"/>.
			/// </summary>
			/// <param name="parentShape">The <see cref="FactTypeShape"/> that this <see cref="RolesShapeField"/> is associated with.</param>
			/// <returns>The minimum <see cref="SizeD"/> of this <see cref="RolesShapeField"/>.</returns>
			public sealed override SizeD GetMinimumSize(ShapeElement parentShape)
			{
				FactTypeShape factTypeShape = (parentShape as FactTypeShape);
				double margin = factTypeShape.StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width;
				double width = FactTypeShape.RoleBoxWidth * Math.Max(1, factTypeShape.DisplayedRoleOrder.Count) + margin;
				double height = FactTypeShape.RoleBoxHeight + margin;
				return (factTypeShape.DisplayOrientation == DisplayOrientation.Horizontal) ? new SizeD(width, height) : new SizeD(height, width);
			}
			/// <summary>
			/// Paint the RolesShapeField
			/// </summary>
			/// <param name="e">DiagramPaintEventArgs with the Graphics object to draw to.</param>
			/// <param name="parentShape">FactTypeShape to draw to.</param>
			public sealed override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
			{
				if (null == Utility.ValidateStore(parentShape.Store))
				{
					return;
				}
				FactTypeShape parentFactShape = (FactTypeShape)parentShape;
				FactType factType = parentFactShape.AssociatedFactType;
				LinkedElementCollection<RoleBase> roles = parentFactShape.DisplayedRoleOrder;
				int roleCount = roles.Count;
				bool objectified = factType.NestingType != null;
				if (roleCount > 0 || objectified)
				{
					int highlightRoleBox = -1;
					DisplayOrientation orientation = parentFactShape.DisplayOrientation;
					bool drawHorizontal = orientation == DisplayOrientation.Horizontal;
					RoleSubField testSubField = new RoleSubField();
					DiagramItem testSelect = new DiagramItem(parentShape, this, testSubField);
					DiagramClientView clientView = e.View;
					SelectedShapesCollection selection = null;
					UniquenessConstraint internalIndexedUniqueness = null;
					LinkedElementCollection<Role> internalIndexedRoles = null;
					bool factShapeHighlighted = false;
					if (clientView != null)
					{
						selection = clientView.Selection;
						if (selection.Count == 1)
						{
							DiagramItem selectedItem = selection.PrimaryItem;
							if (selectedItem.Shape == parentShape &&
								selectedItem.SubField is ConstraintSubField)
							{
								UniquenessConstraint internalUniqueness;
								foreach (object selectedElement in selectedItem.RepresentedElements)
								{
									internalUniqueness = selectedElement as UniquenessConstraint;
									if (internalUniqueness != null && internalUniqueness.IsInternal)
									{
										internalIndexedRoles = internalUniqueness.RoleCollection;
										if (internalIndexedRoles.Count < 1)
										{
											internalIndexedRoles = null;
										}
										else
										{
											internalIndexedUniqueness = internalUniqueness;
										}
									}
									break;
								}
							}
						}
						foreach (DiagramItem item in clientView.HighlightedShapes)
						{
							if (parentFactShape == item.Shape)
							{
								RoleSubField roleField = item.SubField as RoleSubField;
								if (roleField != null)
								{
									highlightRoleBox = roles.IndexOf(roleField.AssociatedRole);
								}
								else if (item.SubField == null)
								{
									factShapeHighlighted = true;
								}
								break;
							}
						}
					}
					RectangleD bounds = this.GetBounds(parentShape);
					double margin = parentShape.StyleSet.GetPen(FactTypeShape.RoleBoxResource).Width / 2;
					bounds.Inflate(-margin, -margin);

					Graphics g = e.Graphics;
					double offsetBy;
					double lastX;
					float top;
					float verticalLineBottom;
					float height;
					if (drawHorizontal)
					{
						offsetBy = bounds.Width / roleCount;
						lastX = bounds.Left;
						top = (float)bounds.Top;
						verticalLineBottom = (float)bounds.Bottom - (float)margin;
						height = (float)bounds.Height;
					}
					else
					{
						offsetBy = bounds.Height / roleCount;
						lastX = bounds.Top;
						top = (float)bounds.Left;
						verticalLineBottom = (float)bounds.Right - (float)margin;
						height = (float)bounds.Width;
					}
					float verticalLineTop = top + (float)margin;
					StyleSet styleSet = parentShape.StyleSet;
					int activeRoleIndex;
					ExternalConstraintConnectAction activeExternalAction = ActiveExternalConstraintConnectAction;
					UniquenessConstraint activeInternalUniqueness = null;
					LinkedElementCollection<Role> activeInternalUniquenessRoles = null;
					InternalUniquenessConstraintConnectAction activeInternalAction = ActiveInternalUniquenessConstraintConnectAction;
					if (activeInternalAction != null)
					{
						if (activeInternalAction.SourceShape != parentFactShape)
						{
							activeInternalAction = null;
						}
						else
						{
							activeInternalUniqueness = activeInternalAction.ActiveConstraint;
						}
					}
					ORMDiagram currentDiagram = parentFactShape.Diagram as ORMDiagram;
					StringFormat stringFormat = null;
					Font connectActionFont = null;
					Brush connectActionBrush = null;
					Font constraintSequenceFont = null;
					Brush constraintSequenceBrush = null;
					bool highlightThisRole = false;
					bool reverseRoleOrder = orientation == DisplayOrientation.VerticalRotatedLeft;
					StyleSetResourceId roleBoxPenId = FactTypeShape.RoleBoxResource;
					Pen roleBoxPen = styleSet.GetPen(roleBoxPenId);
					Color restoreRoleBoxPenColor = Color.Empty;
					try
					{
						restoreRoleBoxPenColor = parentFactShape.UpdateDynamicColor(roleBoxPenId, roleBoxPen);
						Role impliedUnaryRole = (roleCount == 1) ? impliedUnaryRole = roles[0].OppositeRole.Role : null;
						for (int i = 0; i < roleCount; ++i)
						{
							StyleSetResourceId backgroundFillResourceId = null;
							bool fillBackground = true;
							bool backgroundSticky = false;
							bool backgroundHighlighted = false;
							RectangleF roleBounds = drawHorizontal ?
								new RectangleF((float)lastX, top, (float)offsetBy, height) :
								new RectangleF(top, (float)lastX, height, (float)offsetBy);
							int iRole = reverseRoleOrder ? roleCount - i - 1 : i;
							RoleBase currentRoleBase = roles[iRole];
							Role currentRole = currentRoleBase.Role;
							highlightThisRole = factShapeHighlighted || iRole == highlightRoleBox;
							bool internalIndex = false;

							// There is an active ExternalConstraintConnectAction, and this role is currently in the action's role set.
							if ((activeExternalAction != null &&
								-1 != (activeRoleIndex = activeExternalAction.GetActiveRoleIndex(currentRole))) ||
								(activeInternalAction != null &&
								-1 != (activeRoleIndex = activeInternalAction.GetActiveRoleIndex(currentRole))) ||
								((internalIndex = internalIndexedUniqueness != null && (activeInternalAction == null || activeInternalAction.ActiveConstraint != internalIndexedUniqueness)) &&
								-1 != (activeRoleIndex = internalIndexedRoles.IndexOf(currentRole))))
							{
								// There is an active ExternalConstraintConnectAction, and this role is currently in the action's role set,
								// or an internal uniqueness is currently highlighted.
								parentFactShape.DrawHighlight(g, roleBounds, internalIndex ? null : RoleBoxResource, currentRole, false, highlightThisRole);
								fillBackground = false;
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
								if (drawHorizontal)
								{
									g.DrawString((activeRoleIndex + 1).ToString(CultureInfo.InvariantCulture), connectActionFont, connectActionBrush, roleBounds, stringFormat);
								}
								else
								{
									GraphicsState state = g.Save();
									if (orientation == DisplayOrientation.VerticalRotatedRight)
									{
										g.TranslateTransform(roleBounds.Right, roleBounds.Top);
										g.RotateTransform(90, MatrixOrder.Prepend);
									}
									else
									{
										g.TranslateTransform(roleBounds.Left, roleBounds.Bottom);
										g.RotateTransform(-90, MatrixOrder.Prepend);
									}
									g.DrawString((activeRoleIndex + 1).ToString(CultureInfo.InvariantCulture), connectActionFont, connectActionBrush, new RectangleF(0, 0, roleBounds.Height, roleBounds.Width), stringFormat);
									g.Restore(state);
								}
							}
							// There is an active InternalUniquenessConstraintConnectAction, and this role is removed from an existing constraint.
							else if (activeInternalUniqueness != null &&
								(activeInternalUniquenessRoles ?? (activeInternalUniquenessRoles = activeInternalUniqueness.RoleCollection)).Contains(currentRole))
							{
								backgroundSticky = true;
								backgroundHighlighted = highlightThisRole;
							}
							else
							{
								#region Handling StickyObject highlighting and selection
								ExternalConstraintShape stickyConstraintShape;
								IStickyObject stickyObject;
								IConstraint stickyConstraint;
								if (currentDiagram != null &&
									null != (stickyConstraintShape = (stickyObject = currentDiagram.StickyObject) as ExternalConstraintShape)
									&& null != (stickyConstraint = stickyConstraintShape.AssociatedConstraint))
								{
									// The active StickyObject for the diagram is an ExternalConstraintShape
									// If the role is selectedable as part of the sticky object then it is part
									// of the currently selected constraint.
									if (stickyObject.StickySelectable(currentRole))
									{
										Role constraintRole = (impliedUnaryRole != null && stickyConstraint.ConstraintType == ConstraintType.ExternalUniqueness) ?
											impliedUnaryRole :
											currentRole;
										// We need to find out if this role is in one of the role sequences being edited, or if it's just selected.
										backgroundHighlighted = highlightThisRole;
										SetComparisonConstraint mcec;
										SetConstraint scec;
										string indexString = null;

										if (activeExternalAction == null || !activeExternalAction.InitialRoles.Contains(currentRole))
										{
											backgroundSticky = true;
											if (null != (mcec = stickyConstraint as SetComparisonConstraint))
											{
												LinkedElementCollection<SetComparisonConstraintRoleSequence> sequenceCollection = mcec.RoleSequenceCollection;
												int sequenceCollectionCount = sequenceCollection.Count;
												for (int sequenceIndex = 0; sequenceIndex < sequenceCollectionCount; ++sequenceIndex)
												{
													int roleIndex = sequenceCollection[sequenceIndex].RoleCollection.IndexOf(constraintRole);
													if (roleIndex >= 0)
													{
														for (int j = sequenceIndex + 1; j < sequenceCollectionCount; ++j)
														{
															if (sequenceCollection[j].RoleCollection.IndexOf(constraintRole) >= 0)
															{
																// Indicate overlapping role sequences
																indexString = ResourceStrings.SetConstraintStickyRoleOverlapping;
															}
														}
														if ((object)indexString == null)
														{
															// Show 1-based position of the role in the MCEC.
															indexString = string.Format(CultureInfo.InvariantCulture, ResourceStrings.SetConstraintStickyRoleFormatString, sequenceIndex + 1, roleIndex + 1);
														}
														break;
													}
												}
											}
											else if (null != (scec = stickyConstraint as SetConstraint))
											{
												indexString = (scec.RoleCollection.IndexOf(constraintRole) + 1).ToString();
											}

											if ((object)indexString != null)
											{
												parentFactShape.DrawHighlight(g, roleBounds, null, currentRole, true, highlightThisRole);
												fillBackground = false;
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
												if (drawHorizontal)
												{
													g.DrawString(indexString, constraintSequenceFont, constraintSequenceBrush, roleBounds, stringFormat);
												}
												else
												{
													GraphicsState state = g.Save();
													if (orientation == DisplayOrientation.VerticalRotatedRight)
													{
														g.TranslateTransform(roleBounds.Right, roleBounds.Top);
														g.RotateTransform(90, MatrixOrder.Prepend);
													}
													else
													{
														g.TranslateTransform(roleBounds.Left, roleBounds.Bottom);
														g.RotateTransform(-90, MatrixOrder.Prepend);
													}
													g.DrawString(indexString, constraintSequenceFont, constraintSequenceBrush, new RectangleF(0, 0, roleBounds.Height, roleBounds.Width), stringFormat);
													g.Restore(state);
												}
											}
										}
									}
								}
								#endregion // Handling StickyObject highlighting and selection
								if (fillBackground)
								{
									ORMModel model = factType.Model;
									if (highlightThisRole)
									{
										if (ModelError.HasErrors(currentRoleBase, ModelErrorUses.DisplayPrimary, (model != null) ? model.ModelErrorDisplayFilter : null))
										{
											backgroundFillResourceId = ORMDiagram.HighlightedErrorBackgroundResource;
										}
										else
										{
											backgroundHighlighted = true;
										}
									}
									else if (ModelError.HasErrors(currentRoleBase, ModelErrorUses.DisplayPrimary, (model != null) ? model.ModelErrorDisplayFilter : null))
									{
										backgroundFillResourceId = ORMDiagram.ErrorBackgroundResource;
									}
								}
							}
							if (fillBackground)
							{
								parentFactShape.DrawHighlight(g, roleBounds, backgroundFillResourceId, currentRole, backgroundSticky, backgroundHighlighted);
							}

							// Draw a selection rectangle if needed
							testSubField.AssociatedRole = currentRoleBase;
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
								if (drawHorizontal)
								{
									g.DrawLine(roleBoxPen, (float)lastX, verticalLineTop, (float)lastX, verticalLineBottom);
								}
								else
								{
									g.DrawLine(roleBoxPen, verticalLineTop, (float)lastX, verticalLineBottom, (float)lastX);
								}
							}
							lastX += offsetBy;
						}
						// Draw the outside border of the role boxes
						g.DrawRectangle(roleBoxPen, (float)bounds.Left, (float)bounds.Top, (float)bounds.Width, (float)bounds.Height);
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
						if (!restoreRoleBoxPenColor.IsEmpty)
						{
							roleBoxPen.Color = restoreRoleBoxPenColor;
						}
					}
				}
			}
		}
		#endregion // RolesShapeField class
		#region RoleSubField class
		private sealed class RoleSubField : ORMBaseShapeSubField
		{
			#region Member variables
			private RoleBase myAssociatedRole;
			#endregion // Member variables
			#region Construction
			public RoleSubField()
			{
			}
			public RoleSubField(RoleBase associatedRole)
			{
				Debug.Assert(associatedRole != null);
				myAssociatedRole = associatedRole;
			}
			#endregion // Construction
			#region Required ShapeSubField overrides
			/// <summary>
			/// Returns true if the fields have the same associated role
			/// </summary>
			public sealed override bool SubFieldEquals(object obj)
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
			public sealed override int SubFieldHashCode
			{
				get
				{
					RoleBase associatedRole = myAssociatedRole;
					return (associatedRole != null) ? associatedRole.GetHashCode() : 0;
				}
			}
			/// <summary>
			/// A role sub field is always selectable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public sealed override bool GetSelectable(ShapeElement parentShape, ShapeField parentField)
			{
				return true;
			}
			/// <summary>
			/// A role sub field is always focusable, return true regardless of parameters
			/// </summary>
			/// <returns>true</returns>
			public sealed override bool GetFocusable(ShapeElement parentShape, ShapeField parentField)
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
			public sealed override RectangleD GetBounds(ShapeElement parentShape, ShapeField parentField)
			{
				RectangleD retVal = parentField.GetBounds(parentShape);
				FactTypeShape parentFactShape = parentShape as FactTypeShape;
				LinkedElementCollection<RoleBase> roles = parentFactShape.DisplayedRoleOrder;
				int roleCount = roles.Count;
				int roleIndex = roles.IndexOf(myAssociatedRole);
				DisplayOrientation orientation = parentFactShape.DisplayOrientation;
				switch (orientation)
				{
					case DisplayOrientation.Horizontal:
						retVal.Width /= roleCount;
						if (roleIndex > 0)
						{
							retVal.Offset(roleIndex * retVal.Width, 0);
						}
						break;
					case DisplayOrientation.VerticalRotatedLeft:
					case DisplayOrientation.VerticalRotatedRight:
						retVal.Height /= roleCount;
						if (orientation == DisplayOrientation.VerticalRotatedLeft)
						{
							roleIndex = roleCount - roleIndex - 1;
						}
						if (roleIndex > 0)
						{
							retVal.Offset(0, roleIndex * retVal.Height);
						}
						break;
				}
				return retVal;
			}
			#endregion // Required ShapeSubField
			#region Mouse handling
			public sealed override void OnDoubleClick(DiagramPointEventArgs e)
			{
				ORMBaseShape.AttemptErrorActivation(e);
				base.OnDoubleClick(e);
			}
			#endregion // Mouse handling
			#region DragDrop support
			public sealed override MouseAction GetPotentialMouseAction(MouseButtons mouseButtons, PointD point, DiagramHitTestInfo hitTestInfo)
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
			public RoleBase AssociatedRole
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
			#region Accessibility Overrides
			/// <summary>
			/// Defer to the associated role for the accessible name
			/// </summary>
			public override string GetAccessibleName(ShapeElement parentShape, ShapeField parentField)
			{
				return TypeDescriptor.GetClassName(myAssociatedRole);
			}
			/// <summary>
			/// Defer to the associated role for the accessible value
			/// </summary>
			public override string GetAccessibleValue(ShapeElement parentShape, ShapeField parentField)
			{
				return TypeDescriptor.GetComponentName(myAssociatedRole);
			}
			#endregion // Accessibility Overrides
		}
		#endregion // RoleSubField class
		#region Member Variables
		private static RolesShapeField myRolesShapeField;
		private static ConstraintShapeField myTopConstraintShapeField;
		private static ConstraintShapeField myBottomConstraintShapeField;
		private static SpacerShapeField myTopSpacerShapeField;
		private static ConstraintShapeField myLeftConstraintShapeField;
		private static ConstraintShapeField myRightConstraintShapeField;
		private static SpacerShapeField myLeftSpacerShapeField;
		/// <summary>
		/// Pen to draw a role box outline
		/// </summary>
		protected static readonly StyleSetResourceId RoleBoxResource = new StyleSetResourceId("ORMArchitect", "RoleBoxResource");
		/// <summary>
		/// Brush to draw the foreground text for a role picker  
		/// </summary>
		protected static readonly StyleSetResourceId RolePickerForeground = new StyleSetResourceId("ORMArchitect", "RolePickerForeground");
		/// <summary>
		/// Pen to draw the active part of an internal uniqueness constraint.
		/// </summary>
		protected static readonly StyleSetResourceId InternalFactConstraintPen = new StyleSetResourceId("ORMArchitect", "InternalFactConstraintPen");
		/// <summary>
		/// Pen to draw the active part of a deontic internal uniqueness constraint.
		/// </summary>
		protected static readonly StyleSetResourceId DeonticInternalFactConstraintPen = new StyleSetResourceId("ORMArchitect", "DeonticInternalFactConstraintPen");
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
		/// <param name="brushResourceId">The <see cref="StyleSetResourceId"/> for the brush.
		/// Defaults are used if this is null.</param>
		/// <param name="drawingRole">The role being drawn. If this is set and dynamic colors are
		/// available for the role, then the role dynamic colors are used instead of the shapes background
		/// dynamic colors.</param>
		/// <param name="isStuck">Bool indicating if the object to draw the highlight on
		/// is currently the "sticky" object.</param>
		/// <param name="isHighlighted">Bool indicating if object should be drawn highlighted.</param>
		protected void DrawHighlight(Graphics g, RectangleF bounds, StyleSetResourceId brushResourceId, RoleBase drawingRole, bool isStuck, bool isHighlighted)
		{
			Brush brush = null;
			bool testRoleDynamicColor = false;
			if (brushResourceId == null && !isStuck)
			{
				testRoleDynamicColor = drawingRole != null;
				brushResourceId = DiagramBrushes.ShapeBackground;
			}
			else if (isStuck)
			{
				brush = Diagram.StyleSet.GetBrush(brushResourceId = ORMDiagram.StickyBackgroundResource);
			}
			if (brush == null)
			{
				brush = StyleSet.GetBrush(brushResourceId);
			}
			SolidBrush coloredBrush = brush as SolidBrush;
			Color restoreColor = Color.Empty;
			if (testRoleDynamicColor)
			{
				if (coloredBrush != null)
				{
					IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, RoleBase>[] roleDynamicColorProviders = ((IFrameworkServices)drawingRole.Store).GetTypedDomainModelProviders <IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, RoleBase>>();
					if (roleDynamicColorProviders != null)
					{
						for (int i = 0; i < roleDynamicColorProviders.Length; ++i)
						{
							Color dynamicColor = roleDynamicColorProviders[i].GetDynamicColor(ORMDiagramDynamicColor.Background, this, drawingRole);
							if (!dynamicColor.IsEmpty)
							{
								restoreColor = coloredBrush.Color;
								coloredBrush.Color = dynamicColor;
								break;
							}
						}
					}
				}
			}
			if (restoreColor.IsEmpty)
			{
				restoreColor = UpdateDynamicColor(brushResourceId, brush);
			}
			if (!SystemInformation.HighContrast &&
				isHighlighted &&
				coloredBrush != null)
			{
				if (restoreColor.IsEmpty)
				{
					restoreColor = coloredBrush.Color;
				}
				coloredBrush.Color = ORMDiagram.ModifyLuminosity(coloredBrush.Color);
			}
			g.FillRectangle(brush, bounds);
			if (!restoreColor.IsEmpty && coloredBrush != null)
			{
				coloredBrush.Color = restoreColor;
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
			penSettings.Color = Color.DarkBlue;
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
		/// We show extra space around the fact when we're not objectified, but this
		/// space needs to be transparent if it is not highlighted and has no errors.
		/// Allow changing the background brush dynamically to handle this situation.
		/// </summary>
		public override StyleSetResourceId BackgroundBrushId
		{
			get
			{
				Objectification objectification;
				FactType associatedFact = AssociatedFactType;
				ORMModel model;
				if (null == associatedFact ||
					(null != (objectification = associatedFact.Objectification) && !objectification.IsImplied) ||
					ModelError.HasErrors(associatedFact, ModelErrorUses.DisplayPrimary, (null != (model = associatedFact.Model)) ? model.ModelErrorDisplayFilter : null))
				{
					return base.BackgroundBrushId;
				}
				else
				{
					return ORMDiagram.TransparentBrushResource;
				}
			}
		}
		/// <summary>
		/// Use the rolebox outline pen unless we're objectified
		/// </summary>
		public override StyleSetResourceId OutlinePenId
		{
			get
			{
				return ShouldDrawObjectified ? DiagramPens.ShapeOutline : RoleBoxResource;
			}
		}
		/// <summary>
		/// Create our one placeholder shape field, which fills the whole shape
		/// and contains our role boxes.
		/// </summary>
		/// <param name="shapeFields">Per-class collection of shape fields</param>
		protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
		{
			base.InitializeShapeFields(shapeFields);

			// Initialize fields
			// UNDONE: Localize these values. They are used only for accessibility display
			RolesShapeField rolesField = new RolesShapeField("Roles");
			ConstraintShapeField topConstraintField = new ConstraintShapeField("TopConstraints", ConstraintAttachPosition.Top);
			ConstraintShapeField bottomConstraintField = new ConstraintShapeField("BottomConstraints", ConstraintAttachPosition.Bottom);
			ConstraintShapeField leftConstraintField = new ConstraintShapeField("LeftConstraints", ConstraintAttachPosition.Left);
			ConstraintShapeField rightConstraintField = new ConstraintShapeField("RightConstraints", ConstraintAttachPosition.Right);
			SpacerShapeField topSpacer = new SpacerShapeField("TopSpacer", false);
			SpacerShapeField leftSpacer = new SpacerShapeField("LeftSpacer", true);

			// Add all shapes before modifying anchoring behavior
			shapeFields.Add(topSpacer);
			shapeFields.Add(topConstraintField);
			shapeFields.Add(bottomConstraintField);
			shapeFields.Add(leftSpacer);
			shapeFields.Add(leftConstraintField);
			shapeFields.Add(rightConstraintField);
			shapeFields.Add(rolesField);

			// Modify anchoring behavior
			AnchoringBehavior bottomConstraintAnchor = bottomConstraintField.AnchoringBehavior;
			bottomConstraintAnchor.CenterHorizontally();
			bottomConstraintAnchor.SetTopAnchor(rolesField, 1);
			bottomConstraintAnchor.InvisibleCollapseFlags = InvisibleCollapseFlags.VerticallyToTop;

			AnchoringBehavior anchor = rolesField.AnchoringBehavior;
			//anchor.CenterHorizontally();
			anchor.SetLeftAnchor(leftConstraintField, 1);
			anchor.SetTopAnchor(topConstraintField, 1);

			AnchoringBehavior topConstraintAnchor = topConstraintField.AnchoringBehavior;
			topConstraintAnchor.CenterHorizontally();
			topConstraintAnchor.SetTopAnchor(topSpacer, 1);
			topConstraintAnchor.InvisibleCollapseFlags = InvisibleCollapseFlags.VerticallyToTop;

			AnchoringBehavior topSpacerAnchor = topSpacer.AnchoringBehavior;
			topSpacerAnchor.CenterHorizontally();

			AnchoringBehavior rightConstraintAnchor = rightConstraintField.AnchoringBehavior;
			rightConstraintAnchor.CenterVertically();
			rightConstraintAnchor.SetLeftAnchor(rolesField, 1);
			rightConstraintAnchor.InvisibleCollapseFlags = InvisibleCollapseFlags.HorizontallyToLeft;

			AnchoringBehavior leftConstraintAnchor = leftConstraintField.AnchoringBehavior;
			leftConstraintAnchor.CenterVertically();
			leftConstraintAnchor.SetLeftAnchor(leftSpacer, 1);
			leftConstraintAnchor.InvisibleCollapseFlags = InvisibleCollapseFlags.HorizontallyToLeft;

			AnchoringBehavior leftSpacerAnchor = leftSpacer.AnchoringBehavior;
			//leftSpacerAnchor.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0);
			leftSpacerAnchor.CenterVertically();

			Debug.Assert(myRolesShapeField == null); // Only called once
			myRolesShapeField = rolesField;

			Debug.Assert(myTopConstraintShapeField == null); // Only called once
			myTopConstraintShapeField = topConstraintField;

			Debug.Assert(myBottomConstraintShapeField == null); // Only called once
			myBottomConstraintShapeField = bottomConstraintField;

			Debug.Assert(myTopSpacerShapeField == null); // Only called once
			myTopSpacerShapeField = topSpacer;

			Debug.Assert(myLeftConstraintShapeField == null); // Only called once
			myLeftConstraintShapeField = leftConstraintField;

			Debug.Assert(myRightConstraintShapeField == null); // Only called once
			myRightConstraintShapeField = rightConstraintField;

			Debug.Assert(myLeftSpacerShapeField == null); // Only called once
			myLeftSpacerShapeField = leftSpacer;
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
				return ShouldDrawObjectified;
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
					SizeD rolesShapeSize = rolesShape.GetMinimumSize(this);
					width = rolesShapeSize.Width;
					width += myLeftConstraintShapeField.GetMinimumSize(this).Width;
					width += myRightConstraintShapeField.GetMinimumSize(this).Width;
					height = rolesShapeSize.Height;
					height += myTopConstraintShapeField.GetMinimumSize(this).Height;
					height += myBottomConstraintShapeField.GetMinimumSize(this).Height;
					if (!ShouldDrawObjectified)
					{
						width += BorderMargin + BorderMargin;
						height += BorderMargin + BorderMargin;
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
				if (ShouldDrawObjectified)
				{
					if (DisplayOrientation == DisplayOrientation.Horizontal)
					{
						contentSize.Width += NestedFactHorizontalMargin + NestedFactHorizontalMargin;
						contentSize.Height += NestedFactVerticalMargin + NestedFactVerticalMargin;
					}
					else
					{
						contentSize.Width += NestedFactVerticalMargin + NestedFactVerticalMargin;
						contentSize.Height += NestedFactHorizontalMargin + NestedFactHorizontalMargin;
					}
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
		/// a single action, resulting in a single transaction log entry for AbsoluteBounds property</param>
		/// <returns>true if the bounds were changed</returns>
		private bool UpdateRolesPosition(SizeD newSize)
		{
			bool retVal = false;
			double oldRolesPosition = RolesPosition;
			PointD centerPoint = myRolesShapeField.GetBounds(this).Center;
			bool isVertical = DisplayOrientation != DisplayOrientation.Horizontal;
			double newRolesPosition = isVertical ? centerPoint.X : centerPoint.Y;
			if (!VGConstants.FuzzEqual(oldRolesPosition, newRolesPosition, VGConstants.FuzzDistance))
			{
				RolesPosition = newRolesPosition;
				if (oldRolesPosition != 0)
				{
					PointD newLocation = Location;
					if (isVertical)
					{
						newLocation.Offset(oldRolesPosition - newRolesPosition, 0);
					}
					else
					{
						newLocation.Offset(0, oldRolesPosition - newRolesPosition);
					}
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
		/// <param name="rolePositionChangeOnly">A role was repositioned</param>
		public static void ConstraintSetChanged(FactType factType, IConstraint constraint, bool roleChangeOnly, bool rolePositionChangeOnly)
		{
			Debug.Assert(factType.Store.TransactionManager.InTransaction);
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
			{
				FactTypeShape factShape = pel as FactTypeShape;
				if (factShape != null)
				{
					factShape.ConstraintShapeSetChanged(constraint, roleChangeOnly, rolePositionChangeOnly);
				}
			}
		}
		/// <summary>
		/// The constraint shapes associated with this fact type have changed. Call directly
		/// if the change happens due to a shape change instead of an underlying model change.
		/// </summary>
		/// <param name="constraint">The newly added or removed constraint</param>
		public void ConstraintShapeSetChanged(IConstraint constraint)
		{
			ConstraintShapeSetChanged(constraint, false, false);
		}
		/// <summary>
		/// The constraint shapes associated with this fact type have changed. Call directly
		/// if the change happens due to a shape change instead of an underlying model change.
		/// </summary>
		/// <param name="constraint">The newly added or removed constraint</param>
		/// <param name="roleChangeOnly">A role was added or removed</param>
		/// <param name="rolePositionChangeOnly">A role was repositioned</param>
		private void ConstraintShapeSetChanged(IConstraint constraint, bool roleChangeOnly, bool rolePositionChangeOnly)
		{
			bool resize = false;
			bool redraw = false;
			FactType factType;
			switch (constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
					if (rolePositionChangeOnly)
					{
						// Nothing to do, role numbers not displayed for internal uniqueness constraints
					}
					else if (roleChangeOnly)
					{
						resize = AssociatedFactType.RoleCollection.Count == 2;
						redraw = !resize;
					}
					else
					{
						resize = true;
					}
					break;
				case ConstraintType.SimpleMandatory:
					switch (OptionsPage.CurrentEntityRelationshipBinaryMultiplicityDisplay)
					{
						case EntityRelationshipBinaryMultiplicityDisplay.Barker:
						case EntityRelationshipBinaryMultiplicityDisplay.InformationEngineering:
							if ((factType = AssociatedFactType).RoleCollection.Count == 2 &&
								factType.UnaryRole == null)
							{
								foreach (RolePlayerLink rolePlayerLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<RolePlayerLink>(this))
								{
									((IInvalidateDisplay)rolePlayerLink).InvalidateRequired(true);
								}
							}
							break;
					}
					break;
				case ConstraintType.ExternalUniqueness:
				case ConstraintType.DisjunctiveMandatory:
				case ConstraintType.Ring:
				case ConstraintType.Exclusion:
				case ConstraintType.Subset:
				case ConstraintType.Equality:
				case ConstraintType.Frequency:
					if (rolePositionChangeOnly)
					{
						redraw = true;
					}
					else
					{
						resize = true;
					}
					break;
			}
			if (resize)
			{
				foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(this))
				{
					binaryLink.RecalculateRoute();
				}
				SizeD oldSize = Size;
				AutoResize();
				if (oldSize == Size)
				{
					redraw = true;
				}
			}
			if (redraw)
			{
				InvalidateRequired(true);
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
		/// external constraint roles, or (optionally) the definition text.
		/// </summary>
		public override string GetToolTipText(DiagramItem item)
		{
			string retVal = null;
			ShapeSubField subField = item.SubField;
			RoleSubField roleField;
			if (null != (roleField = subField as RoleSubField))
			{
				IStickyObject stickyObject;
				ExternalConstraintShape shape;
				SetComparisonConstraint mcec;
				RoleBase role;
				if ((null != (stickyObject = ((ORMDiagram)Diagram).StickyObject)) &&
					(null != (shape = stickyObject as ExternalConstraintShape)) &&
					(null != (mcec = shape.AssociatedConstraint as SetComparisonConstraint)) &&
					stickyObject.StickySelectable(role = roleField.AssociatedRole))
				{
					ExternalConstraintConnectAction activeExternalAction = ActiveExternalConstraintConnectAction;
					if ((activeExternalAction == null) ||
						(-1 == activeExternalAction.GetActiveRoleIndex(role.Role)))
					{
						LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = (mcec as SetComparisonConstraint).RoleSequenceCollection;
						int sequenceCount = sequences.Count;
						string formatString = ResourceStrings.SetConstraintStickyRoleTooltipFormatString;
						for (int i = 0; i < sequenceCount; ++i)
						{
							SetComparisonConstraintRoleSequence sequence = sequences[i];
							int roleIndex = sequence.RoleCollection.IndexOf(role.Role);
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
			if (retVal == null &&
				OptionsPage.CurrentDisplayDefinitionTooltips &&
				(subField == null || roleField != null))
			{
				retVal = AssociatedFactType.DefinitionText;
				if (retVal.Length == 0)
				{
					retVal = null;
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
				// If the fact should draw as objectified, get the current setting from the options
				// page for how to draw the shape
				if (ShouldDrawObjectified)
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
			Role role;
			if (element is ReadingOrder)
			{
				foreach (ShapeElement shape in this.RelativeChildShapes)
				{
					ReadingShape readingShape = shape as ReadingShape;
					if (readingShape != null && !readingShape.IsDeleted)
					{
						return false;
					}
				}
			}
			else if ((role = element as Role) != null)
			{
				if (this.DisplayRoleNames == DisplayRoleNames.Off)
				{
					return false;
				}
				return !string.IsNullOrEmpty(role.Name) && (role != AssociatedFactType.ImplicitBooleanRole);
			}
			return true;
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
		/// <summary>
		/// Highlight a visible role name shape along with the corresponding role box
		/// </summary>
		public override void OnSubFieldMouseEnter(ShapeField field, ShapeSubField subField, DiagramPointEventArgs e)
		{
			DiagramClientView view = e.DiagramClientView;
			if (view != null)
			{
				RoleSubField roleField = subField as RoleSubField;
				if (roleField != null)
				{
					RoleBase findRole = roleField.AssociatedRole;
					foreach (ShapeElement pel in RelativeChildShapes)
					{
						RoleNameShape nameShape;
						if (null != (nameShape = pel as RoleNameShape) &&
							nameShape.ModelElement == findRole)
						{
							DiagramItemCollection items = new DiagramItemCollection();
							items.Add(new DiagramItem(this, field, subField));
							items.Add(new DiagramItem(nameShape));
							view.HighlightedShapes.Set(items);
							return;
						}
					}
				}
				view.HighlightedShapes.Set(new DiagramItem(this, field, subField));
			}
		}
		/// <summary>
		/// Highlight a visible role name shape along with the corresponding role box
		/// </summary>
		public override void OnSubFieldMouseLeave(ShapeField field, ShapeSubField subField, DiagramPointEventArgs e)
		{
			DiagramClientView view = e.DiagramClientView;
			if (view != null)
			{
				RoleSubField roleField = subField as RoleSubField;
				if (roleField != null)
				{
					RoleBase findRole = roleField.AssociatedRole;
					foreach (ShapeElement pel in RelativeChildShapes)
					{
						RoleNameShape nameShape;
						if (null != (nameShape = pel as RoleNameShape) &&
							nameShape.ModelElement == findRole)
						{
							view.HighlightedShapes.Remove(new DiagramItem[]{new DiagramItem(this, field, subField), new DiagramItem(nameShape)});
							return;
						}
					}
				}
				view.HighlightedShapes.Remove(new DiagramItem(this, field, subField));
			}
		}
		#endregion // Customize appearance
		#region DisplayFlags
		/// <summary>
		/// Bitfield for display settings. All flags are assumed to default to false. 
		/// </summary>
		[Flags]
		private enum DisplayFlags
		{
			/// <summary>
			/// Corresponds to the ConstraintDisplayPosition property
			/// </summary>
			ConstraintsOnBottom = 1,
			/// <summary>
			/// Corresponds to the On value of the DisplayRoleNames property
			/// </summary>
			RoleNamesOn = 2,
			/// <summary>
			/// Corresponds to the Off value of the DisplayRoleNames property
			/// </summary>
			RoleNamesOff = 4,
			/// <summary>
			/// Corresponds to the VerticalRotatedLeft property of DisplayOrientation
			/// </summary>
			VerticalLeft = 8,
			/// <summary>
			/// Corresponds to the VerticalRotatedRight property of DisplayOrientation
			/// </summary>
			VerticalRight = 0x10,
			/// <summary>
			/// Corresponds to the suptypes part of the DisplayRelatedTypes property
			/// </summary>
			HideSubtypes = 0x20,
			/// <summary>
			/// Corresponds to the supertypes part of the DisplayRelatedTypes property
			/// </summary>
			HideSupertypes = 0x40,
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
		#region Custom Stored Display Settings
		private ConstraintDisplayPosition GetConstraintDisplayPositionValue()
		{
			return GetDisplayFlag(DisplayFlags.ConstraintsOnBottom) ? ConstraintDisplayPosition.Bottom : ConstraintDisplayPosition.Top;
		}
		private void SetConstraintDisplayPositionValue(ConstraintDisplayPosition value)
		{
			SetDisplayFlag(DisplayFlags.ConstraintsOnBottom, value == ConstraintDisplayPosition.Bottom);
		}
		private DisplayRoleNames GetDisplayRoleNamesValue()
		{
			return GetDisplayFlag(DisplayFlags.RoleNamesOn) ?
				DisplayRoleNames.On :
				(GetDisplayFlag(DisplayFlags.RoleNamesOff) ? DisplayRoleNames.Off : DisplayRoleNames.UserDefault);
		}
		private void SetDisplayRoleNamesValue(DisplayRoleNames value)
		{
			switch (value)
			{
				case DisplayRoleNames.UserDefault:
					SetDisplayFlag(DisplayFlags.RoleNamesOn | DisplayFlags.RoleNamesOff, false);
					break;
				case DisplayRoleNames.On:
					SetDisplayFlag(DisplayFlags.RoleNamesOn, true);
					SetDisplayFlag(DisplayFlags.RoleNamesOff, false);
					break;
				case DisplayRoleNames.Off:
					SetDisplayFlag(DisplayFlags.RoleNamesOff, true);
					SetDisplayFlag(DisplayFlags.RoleNamesOn, false);
					break;
			}
		}
		private DisplayOrientation GetDisplayOrientationValue()
		{
			return GetDisplayFlag(DisplayFlags.VerticalLeft) ?
				DisplayOrientation.VerticalRotatedLeft :
				(GetDisplayFlag(DisplayFlags.VerticalRight) ? DisplayOrientation.VerticalRotatedRight : DisplayOrientation.Horizontal);
		}
		private void SetDisplayOrientationValue(DisplayOrientation value)
		{
			switch (value)
			{
				case DisplayOrientation.Horizontal:
					SetDisplayFlag(DisplayFlags.VerticalLeft | DisplayFlags.VerticalRight, false);
					break;
				case DisplayOrientation.VerticalRotatedLeft:
					SetDisplayFlag(DisplayFlags.VerticalLeft, true);
					SetDisplayFlag(DisplayFlags.VerticalRight, false);
					break;
				case DisplayOrientation.VerticalRotatedRight:
					SetDisplayFlag(DisplayFlags.VerticalRight, true);
					SetDisplayFlag(DisplayFlags.VerticalLeft, false);
					break;
			}
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
		#endregion // Custom Stored Display Settings
		#region Accessibility Settings
		/// <summary>
		/// Get the accessible name for a shape field
		/// </summary>
		public override string GetFieldAccessibleName(ShapeField field)
		{
			return field.Name;
		}
		/// <summary>
		/// Get the accessible value for a shape field
		/// </summary>
		public override string GetFieldAccessibleValue(ShapeField field)
		{
			ConstraintShapeField constraintField;
			if (null != (constraintField = field as ConstraintShapeField))
			{
				ConstraintDisplayPosition position = DisplayPositionFromAttachPosition(constraintField.AttachPosition);
				return ORMShapeDomainModel.SingletonResourceManager.GetString("ConstraintDisplayPosition." + position.ToString());
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
			return CalculateConnectionPoint(oppositeShape, null);
		}
		/// <summary>
		/// Calculate the connection point for the oppositeShape attached to this
		/// shape. If the oppositeShape is semantially attached more than once, then
		/// a connectorShape will be passed in, and the link will be connected to the
		/// connector shape instead of the primary shape. If custom shape folding is
		/// required for a link, then secondary links between the same two shapes need
		/// to use an alternate connector shape.
		/// </summary>
		/// <param name="oppositeShape">The opposite shape we're connecting to</param>
		/// <param name="connectorShape">A connector shape used to disambiguate multiply connected lines</param>
		/// <returns>The point to connect to. May be internal to the object, or on the boundary.</returns>
		public PointD CalculateConnectionPoint(NodeShape oppositeShape, FactTypeLinkConnectorShape connectorShape)
		{
			Debug.Assert(connectorShape == null || connectorShape.ParentShape == this);

			// First figure out the link we're attempting to connect
			ModelElement linkElement = null;
			LinkedElementCollection<LinkShape> linkedToThisShape = LinkConnectsToNode.GetLink((connectorShape != null) ? connectorShape as NodeShape : this);
			LinkedElementCollection<LinkShape> linkedToOppositeShape = LinkConnectsToNode.GetLink(oppositeShape);
			int linkedToThisShapeCount = linkedToThisShape.Count;
			for (int i = 0; i < linkedToThisShapeCount; ++i)
			{
				LinkShape testLinkShape = linkedToThisShape[i];
				if (linkedToOppositeShape.Contains(testLinkShape))
				{
					linkElement = testLinkShape.ModelElement;
					break;
				}
			}

			ExternalConstraintShape constraintShape;
			ValueConstraintShape rangeShape;
			FactType factType = null;
			SubtypeFact subtypeFact = null;
			ObjectTypePlaysRole rolePlayerLink = null;
			FactTypeLinkConnectorShape proxyConnector = oppositeShape as FactTypeLinkConnectorShape;
			if (proxyConnector != null)
			{
				oppositeShape = oppositeShape.ParentShape as NodeShape;
			}
			int factRoleCount = 0;
			int roleIndex = -1;
			bool attachBeforeRole = false; // If true, attach before roleIndex, not in the middle of it
			if (null != (subtypeFact = linkElement as SubtypeFact))
			{
				// Return empty to ignore the calculated connection point and defer to the geometry
				return PointD.Empty;
			}
			else if (null != (rolePlayerLink = linkElement as ObjectTypePlaysRole))
			{
				Role role = rolePlayerLink.PlayedRole;
				factType = role.FactType;
				LinkedElementCollection<RoleBase> factRoles = DisplayedRoleOrder;
				factRoleCount = factRoles.Count;
				roleIndex = factRoles.IndexOf(role);
			}
			else if (null != (rangeShape = oppositeShape as ValueConstraintShape))
			{
				factType = AssociatedFactType;
				LinkedElementCollection<RoleBase> factRoles = DisplayedRoleOrder;
				factRoleCount = factRoles.Count;
				roleIndex = factRoles.IndexOf(((RoleValueConstraint)rangeShape.AssociatedValueConstraint).Role);
			}
			else if (null != (constraintShape = oppositeShape as ExternalConstraintShape))
			{
				IConstraint constraint = constraintShape.AssociatedConstraint;
				factType = AssociatedFactType;
				if (factType != null)
				{
					SetConstraint scec;
					SetComparisonConstraint mcec = null;
					ReadOnlyCollection<ElementLink> factConstraints = null;
					IList<Role> roles = null;
					if (null != (scec = constraint as SetConstraint))
					{
						factConstraints = DomainRoleInfo.GetElementLinks<ElementLink>(scec, FactSetConstraint.SetConstraintDomainRoleId);
					}
					else if (null != (mcec = constraint as SetComparisonConstraint))
					{
						factConstraints = DomainRoleInfo.GetElementLinks<ElementLink>(mcec, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
					}
					if (factConstraints != null)
					{
						int factConstraintCount = factConstraints.Count;
						for (int i = 0; i < factConstraintCount; ++i)
						{
							IFactConstraint factConstraint = (IFactConstraint)factConstraints[i];
							if (factConstraint.FactType == factType)
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
								LinkedElementCollection<RoleBase> factRoles;
								int roleCount = roles.Count;
								int role1Index = 1;
								switch (roleCount)
								{
									case 1:
										factRoles = DisplayedRoleOrder;
										factRoleCount = factRoles.Count;
										// The index here will be -1 for some constraints
										// that connect to the implicit boolean role on a
										// binarized unary fact type.
										roleIndex = (roleIndex = factRoles.IndexOf(roles[0])) == -1 ? 0 : roleIndex;
										break;
									case 2:
										{
											Role role0 = roles[0];
											Role role1 = roles[role1Index];
											if (mcec != null && role0 == role1)
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
													if (testRole != role0)
													{
														role1Index = i;
														role1 = testRole;
													}
												}
												else if (testRole != role0 && testRole != role1)
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
			if (roleIndex != -1)
			{
				PointD objCenter = oppositeShape.AbsoluteCenter;
				RectangleD factBox = myRolesShapeField.GetBounds(this); // This finds the role box for both objectified and simple fact types
				factBox.Offset(AbsoluteBounds.Location);
				DisplayOrientation orientation = DisplayOrientation;

				double finalY;
				double roleWidth;
				double finalX;
				switch (orientation)
				{
					case DisplayOrientation.Horizontal:
						// Decide whether top or bottom works best
						finalY = (Math.Abs(objCenter.Y - factBox.Top) <= Math.Abs(objCenter.Y - factBox.Bottom)) ? factBox.Top : factBox.Bottom;
						// Find the left/right position
						roleWidth = factBox.Width / factRoleCount;
						finalX = factBox.Left + roleWidth * (roleIndex + (attachBeforeRole ? 0 : .5));
						break;
					default:
						// Decide whether the left or right works best
						finalX = (Math.Abs(objCenter.X - factBox.Left) <= Math.Abs(objCenter.X - factBox.Right)) ? factBox.Left : factBox.Right;
						// Find the top/bottom position
						roleWidth = factBox.Height / factRoleCount;
						finalY = roleWidth * (roleIndex + (attachBeforeRole ? 0 : .5));
						if (orientation == DisplayOrientation.VerticalRotatedLeft)
						{
							finalY = factBox.Bottom - finalY;
						}
						else
						{
							finalY += factBox.Top;
						}
						break;
				}

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
						switch (orientation)
						{
							case DisplayOrientation.Horizontal:
								if (objCenter.X < factBox.Left)
								{
									testCenter = new PointD(factBox.Left + roleWidth * .5, factBox.Center.Y);
								}
								break;
							case DisplayOrientation.VerticalRotatedRight:
								if (objCenter.Y < factBox.Top)
								{
									testCenter = new PointD(factBox.Center.X, factBox.Top + roleWidth * .5);
								}
								break;
							case DisplayOrientation.VerticalRotatedLeft:
								if (objCenter.Y > factBox.Bottom)
								{
									testCenter = new PointD(factBox.Center.X, factBox.Bottom - roleWidth * .5);
								}
								break;
						}
					}
					else if (roleIndex == (factRoleCount - 1))
					{
						switch (orientation)
						{
							case DisplayOrientation.Horizontal:
								if (objCenter.X > factBox.Right)
								{
									testCenter = new PointD(factBox.Right - roleWidth * .5, factBox.Center.Y);
								}
								break;
							case DisplayOrientation.VerticalRotatedRight:
								if (objCenter.Y > factBox.Top)
								{
									testCenter = new PointD(factBox.Center.X, factBox.Bottom - roleWidth * .5);
								}
								break;
							case DisplayOrientation.VerticalRotatedLeft:
								if (objCenter.Y < factBox.Top)
								{
									testCenter = new PointD(factBox.Center.X, factBox.Top + roleWidth * .5);
								}
								break;
						}
					}
					if (!testCenter.IsEmpty)
					{
						// Compare the slope to a single role box height/width to see
						// if we should connect to the edge or the top/bottom
						if (orientation == DisplayOrientation.Horizontal)
						{
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
						else
						{
							double rise = objCenter.Y - testCenter.Y;
							if (!VGConstants.FuzzZero(rise, VGConstants.FuzzDistance))
							{
								double inverseSlope = (objCenter.X - testCenter.X) / rise;
								if (Math.Abs(inverseSlope) < (factBox.Width / roleWidth))
								{
									finalX = testCenter.X;
									// The line coming in is flatter than the line
									// across opposite corners of the role box,
									// connect to the left/right edge
									if (factRoleCount == 1)
									{
										finalY = (objCenter.Y < factBox.Top) ? factBox.Top : factBox.Bottom;
									}
									else if (roleIndex == 0)
									{
										finalY = (orientation == DisplayOrientation.VerticalRotatedRight) ? factBox.Top : factBox.Bottom;
									}
									else if (roleIndex == (factRoleCount - 1))
									{
										finalY = (orientation == DisplayOrientation.VerticalRotatedRight) ? factBox.Bottom : factBox.Top;
									}
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
		/// <summary>
		/// Helper function to get a shape that is not currently
		/// attached to the oppositeShape via any other LinkShape
		/// objects. This allows multiple links between the same two
		/// objects to be calculated without ambiguity.
		/// Implements <see cref="IProvideConnectorShape.GetUniqueConnectorShape"/>
		/// </summary>
		/// <returns>NodeShape</returns>
		protected NodeShape GetUniqueConnectorShape(ShapeElement oppositeShape, ModelElement ignoreLinkShapesFor)
		{
			NodeShape fromShape = this;
			LinkedElementCollection<LinkShape> linkedToFromShape = LinkConnectsToNode.GetLink(fromShape);
			int linkedToFromShapeCount = linkedToFromShape.Count;
			if (linkedToFromShapeCount != 0)
			{
				IEnumerable<BinaryLinkShape> linkedToToShape = MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(oppositeShape);
				for (int i = 0; i < linkedToFromShapeCount; ++i)
				{
					BinaryLinkShape testLinkShape = linkedToFromShape[i] as BinaryLinkShape;
					if (testLinkShape != null &&
						(null == ignoreLinkShapesFor || ignoreLinkShapesFor != testLinkShape.ModelElement) &&
						Utility.EnumerableContains(linkedToToShape, testLinkShape))
					{
						FactTypeLinkConnectorShape alternateFromShape = null;
						LinkedElementCollection<ShapeElement> childShapes = fromShape.RelativeChildShapes;
						int nextConnectShapePosition = 0;
						foreach (ShapeElement shape in childShapes)
						{
							alternateFromShape = shape as FactTypeLinkConnectorShape;
							if (alternateFromShape != null)
							{
								// Note that a single FactTypeLinkConnectorShape can be attached to multiple
								// opposite shapes. We just can't have two opposite shapes attached to the
								// same shape.
								LinkedElementCollection<LinkShape> linkedToAlternateCandidateShape = LinkConnectsToNode.GetLink(alternateFromShape);
								int linkedToAlternateCandidateShapeCount = linkedToAlternateCandidateShape.Count;
								for (int j = 0; j < linkedToAlternateCandidateShapeCount; ++j)
								{
									testLinkShape = linkedToAlternateCandidateShape[j] as BinaryLinkShape;
									if (testLinkShape != null &&
										(null == ignoreLinkShapesFor || ignoreLinkShapesFor != testLinkShape.ModelElement) &&
										Utility.EnumerableContains(linkedToToShape, testLinkShape))
									{
										alternateFromShape = null;
										++nextConnectShapePosition;
									}
								}
								if (alternateFromShape != null)
								{
									// This one will work just fine
									break;
								}
							}
						}
						if (alternateFromShape == null)
						{
							alternateFromShape = new FactTypeLinkConnectorShape(Partition);
							childShapes.Add(alternateFromShape);
							alternateFromShape.Location = new PointD(.001 * nextConnectShapePosition, 0);
						}
						fromShape = alternateFromShape;
						break;
					}
				}
			}
			return fromShape;
		}
		NodeShape IProvideConnectorShape.GetUniqueConnectorShape(ShapeElement oppositeShape, ModelElement ignoreLinkShapesFor)
		{
			return GetUniqueConnectorShape(oppositeShape, ignoreLinkShapesFor);
		}
		#endregion // ICustomShapeFolding implementation
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			PopulationMandatoryError populationMandatory;
			TooFewReadingRolesError tooFew;
			TooManyReadingRolesError tooMany;
			ReadingRequiresUserModificationError userModification;
			FactTypeRequiresReadingError noReading;
			FactTypeRequiresInternalUniquenessConstraintError noUniqueness;
			NMinusOneError nMinusOne;
			RolePlayerRequiredError requireRolePlayer;
			FactType factType;
			ConstraintDuplicateNameError constraintNameError;
			ImpliedInternalUniquenessConstraintError implConstraint;
			Reading reading = null;
			UniquenessConstraint activateConstraint = null;
			ValueConstraintError valueConstraintError;
			RoleValueConstraint errorValueConstraint = null;
			bool addActiveRoles = false;
			bool retVal = true;
			if (null != (populationMandatory = error as PopulationMandatoryError))
			{
				FactType autoCorrectFactType = AssociatedFactType;
				Role autoCorrectRole = null;
				DiagramView diagramView;
				SelectedShapesCollection selection;
				DiagramItem currentItem;
				RoleSubField roleSubField;
				if (null != (diagramView = Diagram.ActiveDiagramView) &&
					null != (selection = diagramView.Selection) &&
					1 == selection.Count &&
					null != (currentItem = selection.PrimaryItem) &&
					currentItem.Shape == this &&
					null != (roleSubField = currentItem.SubField as RoleSubField))
				{
					autoCorrectRole = roleSubField.AssociatedRole.Role;
				}
				retVal = ORMDesignerPackage.SamplePopulationEditorWindow.AutoCorrectMandatoryError(populationMandatory, autoCorrectRole, autoCorrectFactType);
			}
			else if (null != (tooFew = error as TooFewReadingRolesError))
			{
				reading = tooFew.Reading;
			}
			else if (null != (tooMany = error as TooManyReadingRolesError))
			{
				reading = tooMany.Reading;
			}
			else if (null != (noReading = error as FactTypeRequiresReadingError))
			{
				factType = noReading.FactType;
				Debug.Assert(factType != null);
				ORMReadingEditorToolWindow newWindow = ORMDesignerPackage.ReadingEditorWindow;
				newWindow.Show();
				newWindow.ActivateReading(factType);
			}
			else if (null != (userModification = error as ReadingRequiresUserModificationError))
			{
				reading = userModification.Reading;
			}
			else if (null != (noUniqueness = error as FactTypeRequiresInternalUniquenessConstraintError))
			{
				factType = noUniqueness.FactType;
				// This error ignores any existing zero-role internal uniqueness constraints, find an existing one if possible
				foreach (UniquenessConstraint testConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (testConstraint.Modality == ConstraintModality.Alethic && testConstraint.RoleCollection.Count == 0)
					{
						activateConstraint = testConstraint;
						break;
					}
				}
				if (activateConstraint == null)
				{
					using (Transaction tran = factType.Store.TransactionManager.BeginTransaction(ResourceStrings.AddInternalConstraintTransactionName))
					{
						activateConstraint = UniquenessConstraint.CreateInternalUniquenessConstraint(factType);
						tran.Commit();
					}
				}
			}
			else if (null != (nMinusOne = error as NMinusOneError))
			{
				activateConstraint = nMinusOne.Constraint;
				addActiveRoles = true;
			}
			else if (null != (requireRolePlayer = error as RolePlayerRequiredError))
			{
				// The role will be selected by the IProxyDisplayProvider implementation
				ORMDiagram ormDiagram = Diagram as ORMDiagram;
				ormDiagram.RoleConnectAction.ChainMouseAction(
					GetAbsoluteRoleAttachPoint(requireRolePlayer.Role),
					ormDiagram.ActiveDiagramView.DiagramClientView,
					false);
			}
			else if (null != (constraintNameError = error as ConstraintDuplicateNameError))
			{
				// We'll get here if one of the constraints we own has a duplicate name
				IList setConstraints = constraintNameError.SetConstraintCollection;
				int setConstraintsCount = setConstraints.Count;
				factType = AssociatedFactType;
				for (int i = 0; i < setConstraintsCount; ++i)
				{
					SetConstraint setConstraint = (SetConstraint)setConstraints[i];
					IConstraint ic = setConstraint.Constraint;
					if (ic.ConstraintIsInternal &&
						setConstraint.FactTypeCollection.Contains(factType))
					{
						switch (ic.ConstraintType)
						{
							case ConstraintType.InternalUniqueness:
							case ConstraintType.SimpleMandatory:
								// The appropriate role (for a mandatory constraint) or
								// an internal uniqueness constraint will be selected by
								// the proxy display provider
								ActivateNameProperty(setConstraint);
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
			else if (null != (valueConstraintError = error as ValueConstraintError))
			{
				retVal = null != (errorValueConstraint = valueConstraintError.ContextValueConstraint as RoleValueConstraint);
			}
			else if (error is TooFewFactTypeRoleInstancesError || error is ObjectifyingInstanceRequiredError)
			{
				retVal = ORMDesignerPackage.SamplePopulationEditorWindow.ActivateModelError(error);
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
				ORMDiagram ormDiagram = Diagram as ORMDiagram;
				InternalUniquenessConstraintConnectAction connectAction = ormDiagram.InternalUniquenessConstraintConnectAction;
				ActiveInternalUniquenessConstraintConnectAction = connectAction;
				if (addActiveRoles)
				{
					LinkedElementCollection<Role> roleColl = activateConstraint.RoleCollection;
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
				connectAction.ChainMouseAction(this, activateConstraint, ormDiagram.ActiveDiagramView.DiagramClientView);
			}
			else if (errorValueConstraint != null)
			{
				// The role is activated by the proxy display provider
				EditorUtility.ActivatePropertyEditor(
					(Store as IORMToolServices).ServiceProvider,
					DomainTypeDescriptor.CreatePropertyDescriptor(errorValueConstraint.Role, Role.ValueRangeTextDomainPropertyId),
					false);
			}
			return retVal;
		}
		bool IModelErrorActivation.ActivateModelError(ModelError error)
		{
			return ActivateModelError(error);
		}
		#endregion // IModelErrorActivation Implementation
		#region IProxyDisplayProvider Implementation
		/// <summary>
		/// Implements <see cref="IProxyDisplayProvider.ElementDisplayedAs"/> by
		/// returning a <see cref="DiagramItem"/> for a <see cref="Role"/>
		/// </summary>
		protected object ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			Role role;
			UniquenessConstraint uniquenessConstraint;
			MandatoryConstraint mandatoryConstraint;
			FactType factType;
			if (null != (role = element as Role))
			{
				if (role.FactType == AssociatedFactType)
				{
					return GetDiagramItem(role);
				}
			}
			else if (null != (uniquenessConstraint = element as UniquenessConstraint))
			{
				LinkedElementCollection<FactType> constraintFactTypes;
				if (uniquenessConstraint.IsInternal &&
					1 == (constraintFactTypes = uniquenessConstraint.FactTypeCollection).Count &&
					constraintFactTypes[0] == AssociatedFactType)
				{
					return GetDiagramItem(uniquenessConstraint);
				}
			}
			else if (null != (mandatoryConstraint = element as MandatoryConstraint))
			{
				LinkedElementCollection<Role> constraintRoles;
				if (mandatoryConstraint.IsSimple &&
					1 == (constraintRoles = mandatoryConstraint.RoleCollection).Count &&
					(role = constraintRoles[0]).FactType == AssociatedFactType)
				{
					return GetDiagramItem(role);
				}
			}
			else if (null != (factType = element as FactType))
			{
				Objectification objectification;
				if (null != (objectification = factType.ImpliedByObjectification) &&
					objectification.NestedFactType == AssociatedFactType)
				{
					Role targetRole = null;
					foreach (RoleBase roleBase in factType.RoleCollection)
					{
						RoleProxy proxy;
						ObjectifiedUnaryRole objectifiedUnaryRole;
						if (null != (proxy = roleBase as RoleProxy))
						{
							targetRole = proxy.TargetRole;
							break;
						}
						else if (null != (objectifiedUnaryRole = roleBase as ObjectifiedUnaryRole))
						{
							targetRole = objectifiedUnaryRole.TargetRole;
							break;
						}
					}
					if (targetRole != null)
					{
						return GetDiagramItem(targetRole);
					}
				}
			}
			return null;
		}
		object IProxyDisplayProvider.ElementDisplayedAs(ModelElement element, ModelError forError)
		{
			return ElementDisplayedAs(element, forError);
		}
		#endregion // IProxyDisplayProvider Implementation
		#region IDynamicColorGeometryHost Implementation
		/// <summary>
		/// Implements <see cref="IDynamicColorGeometryHost.UpdateDynamicColor(StyleSetResourceId,Pen)"/>
		/// </summary>
		protected Color UpdateDynamicColor(StyleSetResourceId penId, Pen pen)
		{
			Color retVal = Color.Empty;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>[] providers;
			bool isRoleBox;
			FactType element;
			if (((isRoleBox = penId == RoleBoxResource) ||
				penId == DiagramPens.ShapeOutline) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>>()) &&
				null != (element = (FactType)ModelElement))
			{
				ORMDiagramDynamicColor requestColor = isRoleBox ? ORMDiagramDynamicColor.ForegroundGraphics : ORMDiagramDynamicColor.Outline;
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(requestColor, this, element);
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
			FactType element;
			IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>[] providers;
			if ((brushId == DiagramBrushes.DiagramBackground || brushId == DiagramBrushes.ShapeBackground) &&
				null != (solidBrush = brush as SolidBrush) &&
				null != (providers = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IDynamicShapeColorProvider<ORMDiagramDynamicColor, FactTypeShape, FactType>>()) &&
				null != (element = (FactType)ModelElement))
			{
				for (int i = 0; i < providers.Length; ++i)
				{
					Color alternateColor = providers[i].GetDynamicColor(ORMDiagramDynamicColor.Background, this, element);
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
				foreach (ShapeElement childShape in RelativeChildShapes)
				{
					if (childShape is ReadingShape)
					{
						yield return childShape;
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
			FactType factType;
			ObjectType objectType;
			ORMDiagram ormDiagram;
			if (null != (factType = AssociatedFactType) && 
				null != (objectType = factType.NestingType) &&
				null != (ormDiagram = diagram as ORMDiagram))
			{
				foreach (Role role in objectType.PlayedRoleCollection)
				{
					FactType subtypeFact = null;
					if ((role is SubtypeMetaRole || role is SupertypeMetaRole) &&
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
		/// Return true if the specified fact type should be drawn as an objectified fact type.
		/// </summary>
		public static bool ShouldDrawObjectification(FactType factType)
		{
			return factType != null && ShouldDrawObjectification(factType.Objectification);
		}
		/// <summary>
		/// Return true if the specified objectification should be drawn as such.
		/// </summary>
		public static bool ShouldDrawObjectification(Objectification objectification)
		{
			return ShouldDrawObjectification(objectification, null);
		}
		/// <summary>
		/// Return true if the specified objectification should be drawn as such.
		/// </summary>
		private static bool ShouldDrawObjectification(Objectification objectification, ObjectType nestingType)
		{
			if (objectification == null)
			{
				return false;
			}
			if (nestingType == null)
			{
				nestingType = objectification.NestingType;
			}
			if (!nestingType.IsDeleted && nestingType.Model == null)
			{
				return false;
			}
#if ALWAYS_DRAW_OBJECTIFICATIONS
			return true;
#else
			// Return true only if the specified objectification is explicit
			return !objectification.IsImplied;
#endif
		}
		/// <summary>
		/// Return true if the associated fact type should be drawn as an objectified fact type.
		/// </summary>
		public bool ShouldDrawObjectified
		{
			get
			{
				return ShouldDrawObjectification(AssociatedFactType);
			}
		}
		/// <summary>
		/// Get a diagram item for an internal uniqueness constraint on the associated fact.
		/// A diagram item is used to represent selection in a DiagramClientView.
		/// </summary>
		public DiagramItem GetDiagramItem(UniquenessConstraint constraint)
		{
			Debug.Assert(constraint.IsInternal);
			return new DiagramItem(this, InternalConstraintShapeField, new ConstraintSubField(constraint));
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
			if (constraint.ConstraintIsInternal)
			{
				// We know the attach location of an internal constraint
				ConstraintShapeField constraintShapeField = InternalConstraintShapeField;
				WalkConstraintBoxes(
					constraintShapeField.GetBounds(this),
					constraintShapeField.AttachPosition,
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
				DisplayOrientation orientation = DisplayOrientation;
				ConstraintShapeField constraintShapeField;
				ConstraintShapeField pendingConstraintShapeField;
				switch (orientation)
				{
					case DisplayOrientation.Horizontal:
						constraintShapeField = myTopConstraintShapeField;
						pendingConstraintShapeField = myBottomConstraintShapeField;
						break;
					case DisplayOrientation.VerticalRotatedRight:
						constraintShapeField = myRightConstraintShapeField;
						pendingConstraintShapeField = myLeftConstraintShapeField;
						break;
					//case DisplayOrientation.VerticalRotatedLeft:
					default:
						Debug.Assert(orientation == DisplayOrientation.VerticalRotatedLeft);
						constraintShapeField = myLeftConstraintShapeField;
						pendingConstraintShapeField = myRightConstraintShapeField;
						break;
				}
				for (int iShape = 0; iShape < 2; ++iShape)
				{
					WalkConstraintBoxes(
						constraintShapeField.GetBounds(this),
						constraintShapeField.AttachPosition,
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
								int trailingCount = roleCount - trailingNotInBox - lastActive - 1;
								if (firstActive > 0 || trailingCount > 0)
								{
									bool isVertical = orientation != DisplayOrientation.Horizontal;
									bool reverseVertical = orientation == DisplayOrientation.VerticalRotatedLeft;
									int widthAdjust = 0;
									if (firstActive > 0)
									{
										if (!reverseVertical)
										{
											double adjust = (firstActive - leadingNotInBox) * RoleBoxWidth;
											if (isVertical)
											{
												rect.Y += adjust;
											}
											else
											{
												rect.X += adjust;
											}
										}
										if (leadingNotInBox == 0)
										{
											widthAdjust = firstActive;
										}
									}
									if (trailingCount > 0)
									{
										widthAdjust += trailingCount;
										if (reverseVertical)
										{
											rect.Offset(0, trailingCount * RoleBoxWidth);
										}
									}
									if (widthAdjust != 0)
									{
										double adjust = widthAdjust * RoleBoxWidth;
										if (isVertical)
										{
											rect.Height -= adjust;
										}
										else
										{
											rect.Width -= adjust;
										}
									}
								}
								return false;
							}
							return true;
						});
					if (!rect.IsEmpty)
					{
						rect.Offset(Bounds.Location);
						retVal = rect.Center;
						double constraintBarAdjust = (position == ConstraintDisplayPosition.Bottom) ? ExternalConstraintBarCenterAdjust : -ExternalConstraintBarCenterAdjust;
						switch (orientation)
						{
							case DisplayOrientation.Horizontal:
								retVal.Y += constraintBarAdjust;
								break;
							case DisplayOrientation.VerticalRotatedLeft:
								retVal.X += constraintBarAdjust;
								break;
							case DisplayOrientation.VerticalRotatedRight:
								retVal.X -= constraintBarAdjust;
								break;
						}
						break;
					}
					position = ConstraintDisplayPosition.Bottom;
					constraintShapeField = pendingConstraintShapeField;
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
			LinkedElementCollection<RoleBase> roles = DisplayedRoleOrder;
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
				return (DisplayOrientation == DisplayOrientation.Horizontal) ?
					new PointD(Size.Width / 2, RolesPosition) :
					new PointD(RolesPosition, Size.Height / 2);
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
			bool retVal = false;
			if (constraint.ConstraintType == ConstraintType.InternalUniqueness)
			{
				PreferredInternalUniquenessConstraintDisplay currentDisplayOption = OptionsPage.CurrentPreferredInternalUniquenessConstraintDisplay;
				if (currentDisplayOption != PreferredInternalUniquenessConstraintDisplay.Never)
				{
					UniquenessConstraint currentConstraint = (UniquenessConstraint)constraint;
					ObjectType forType = currentConstraint.PreferredIdentifierFor;
					if (forType != null)
					{
						Objectification objectification = forType.Objectification;
						if (objectification != null)
						{
							bool requireMultiple = false;
							if (objectification.NestedFactType != currentConstraint.FactTypeCollection[0])
							{
								retVal = true;
							}
							else
							{
								switch (currentDisplayOption)
								{
									case PreferredInternalUniquenessConstraintDisplay.MultipleObjectifiedInternalConstraints:
										if (!objectification.IsImplied)
										{
											requireMultiple = true;
										}
										break;
									case PreferredInternalUniquenessConstraintDisplay.SingleObjectifiedInternalConstraint:
										retVal = !objectification.IsImplied;
										break;
									case PreferredInternalUniquenessConstraintDisplay.MultipleImpliedObjectifiedInternalConstraints:
										requireMultiple = true;
										break;
									case PreferredInternalUniquenessConstraintDisplay.SingleImpliedObjectifiedInternalConstraint:
										retVal = true;
										break;
								}
								if (requireMultiple)
								{
									foreach (UniquenessConstraint testConstraint in currentConstraint.FactTypeCollection[0].GetInternalConstraints<UniquenessConstraint>())
									{
										if (currentConstraint != testConstraint)
										{
											retVal = true;
											break;
										}
									}
								}
							}
						}
						else
						{
							retVal = ((int)currentDisplayOption) >= (int)PreferredInternalUniquenessConstraintDisplay.UnobjectifiedInternalConstraint;
						}
					}
				}
			}
			return retVal;
		}
		#endregion // FactTypeShape specific
		#region Shape display update rules
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SwitchToNestedFactTypeRule(ElementAddedEventArgs e)
		{
			ProcessObjectificationAdd(e.ModelElement as Objectification, null, null);
		}
		/// <summary>
		/// Switch to displaying a nested fact
		/// </summary>
		/// <param name="link">The Objectification relationship to process</param>
		/// <param name="nestedFactType">The nestedFactType to change. If this is null, then use the NestedFactType from the link argument.</param>
		/// <param name="nestingType">The nestingType to change. If this is null, then use the NestingType from the link argument.</param>
		private static void ProcessObjectificationAdd(Objectification link, FactType nestedFactType, ObjectType nestingType)
		{
			if (nestedFactType == null)
			{
				nestedFactType = link.NestedFactType;
			}
			if (nestingType == null)
			{
				nestingType = link.NestingType;
			}

			LinkedElementCollection<PresentationElement> pels;
			// If the objectification should not be drawn, we only need to make sure that the nesting ObjectType has no shapes
			if (!ShouldDrawObjectification(link, nestingType))
			{
				pels = PresentationViewsSubject.GetPresentation(nestingType);
				for (int i = pels.Count - 1; i >= 0; --i)
				{
					ObjectTypeShape pel = pels[i] as ObjectTypeShape;
					if (pel != null)
					{
						pel.Delete();
					}
				}
				return;
			}

			// Part 1: Resize the existing fact shapes
			bool missingNameShapes = false;
			bool hasShape = false;
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(nestedFactType))
			{
				FactTypeShape factShape = pel as FactTypeShape;
				if (factShape != null)
				{
					hasShape = true;
					factShape.AutoResize();
					if (!missingNameShapes)
					{
						bool foundNameShape = false;
						foreach (PresentationElement testNameShape in factShape.RelativeChildShapes)
						{
							if (testNameShape is ObjectifiedFactTypeNameShape)
							{
								foundNameShape = true;
								break;
							}
						}
						if (!foundNameShape)
						{
							missingNameShapes = true;
						}
					}
				}
			}

			// Part2: Create additional shapes for object shapes corresponding to the new type
			pels = PresentationViewsSubject.GetPresentation(nestingType);
#if TRACKNEWSHAPES
			List<FactTypeShape> newShapes = null;
#endif // TRACKNEWSHAPES
			for (int i = pels.Count - 1; i >= 0; --i)
			{
				ObjectTypeShape objectShape = pels[i] as ObjectTypeShape;
				if (objectShape != null)
				{
					hasShape = true;
					ORMDiagram currentDiagram = (ORMDiagram)objectShape.Diagram;

					// Search the current diagram and see if we have a shape for the FactType
					// in the general vicinity. If we do, then we just delete the object shape
					// and let role players bind directly to the FactType.
					bool createNewShape = true;
					RectangleD objectShapeVicinity = objectShape.AbsoluteBoundingBox;
					objectShapeVicinity.Inflate(objectShapeVicinity.Height * 1.5, objectShapeVicinity.Width);
					foreach (FactTypeShape factShape in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(currentDiagram, nestedFactType, true))
					{
						if (factShape.AbsoluteBoundingBox.IntersectsWith(objectShapeVicinity))
						{
							createNewShape = false;
							break;
						}
					}

					if (createNewShape)
					{
						currentDiagram.PlaceORMElementOnDiagram(
							null,
							nestedFactType,
							objectShape.Location,
							ORMPlacementOption.AllowMultipleShapes,
							delegate(ModelElement fixupElement, ShapeElement newShape)
							{
								FactTypeShape factShape = (FactTypeShape)newShape;
#if TRACKNEWSHAPES
								(newShapes ?? (newShapes = new List<FactTypeShape>())).Add(factShape);
#endif // TRACKNEWSHAPES
								factShape.DisplayRelatedTypes = objectShape.DisplayRelatedTypes;
							},
							delegate(ModelElement fixupElement, ShapeElement newShape)
							{
								foreach (PresentationElement relativePel in newShape.RelativeChildShapes)
								{
									ObjectifiedFactTypeNameShape nameShape = relativePel as ObjectifiedFactTypeNameShape;
									if (nameShape != null)
									{
										nameShape.ExpandRefMode = objectShape.ExpandRefMode;
										break;
									}
								}
								MultiShapeUtility.DetachLinks(objectShape);
								objectShape.Delete();
							});
					}
					else
					{
						MultiShapeUtility.DetachLinks(objectShape);
						objectShape.Delete();
					}
				}
			}
#if TRACKNEWSHAPES
			if (newShapes != null)
			{
				// Having replaced ObjectTypeShapes with a bunch of FactTypeShapes,
				// we now check if we actually need the shapes. Any new shape that
				// has no links attached to it and is not the last shape on the diagram
				// is removed.
				// UNDONE: The code here is finding the correct shapes, but the framework
				// Microsoft.VisualStudio.Modeling.Diagrams.ShapeSizeChangeRule is crashing
				// on commit if we remove shapes added during the same transaction.
				foreach (FactTypeShape newShape in newShapes)
				{
					if (!newShape.IsDeleted)
					{
						bool hasLink = false;
						foreach (BinaryLinkShape attachedLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(newShape))
						{
							hasLink = true;
							break;
						}
						if (!hasLink)
						{
							foreach (FactTypeShape otherShape in MultiShapeUtility.FindAllShapesForElement<FactTypeShape>(newShape.Diagram, nestedFactType))
							{
								if (otherShape != newShape)
								{
									newShape.Delete();
									break;
								}
							}
						}
					}
				}
			}
#endif // TRACKNEWSHAPES

			// Make sure we have name shapes for all fact type shapes
			if (!hasShape)
			{
				Diagram.FixUpDiagram(nestedFactType.Model, nestedFactType);
				missingNameShapes = true;
			}
			if (missingNameShapes)
			{
				Diagram.FixUpDiagram(nestedFactType, nestingType);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void SwitchFromNestedFactTypeRule(ElementDeletedEventArgs e)
		{
			Objectification link = e.ModelElement as Objectification;
			// If the objectification was not being drawn, we don't need to do anything
			if (!ShouldDrawObjectification(link))
			{
				return;
			}
			ProcessObjectificationDelete(link, null, null, false);
		}
		/// <summary>
		/// Switch to displaying a nested fact
		/// </summary>
		/// <param name="link">The Objectification relationship to process</param>
		/// <param name="nestedFactType">The nestedFactType to change. If this is null, then use the NestedFactType from the link argument.</param>
		/// <param name="nestingType">The nestingType to change. If this is null, then use the NestingType from the link argument.</param>
		/// <param name="switchingToImplied">Change the objectification to implied from explicit.</param>
		private static void ProcessObjectificationDelete(Objectification link, FactType nestedFactType, ObjectType nestingType, bool switchingToImplied)
		{
			if (nestedFactType == null)
			{
				nestedFactType = link.NestedFactType;
			}
			if (nestingType == null)
			{
				nestingType = link.NestingType;
			}

			bool nestingTypeRemoved = nestingType.IsDeleted;
			bool nestedFactTypeRemoved = nestedFactType.IsDeleted;

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
					// This is a borderline scenario that can't be done through the UI.
					// With delete propagation on the shapes associated with the FactType,
					// if the FactType is removed, then so are all of its shapes and
					// their subshapes, so we have no information to create any replacement
					// ObjectTypeShape elements in this condition.
					return;
				}
				else if (switchingToImplied)
				{
					LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(nestingType);
					for (int i = presentationElements.Count - 1; i >= 0; --i)
					{
						// Grab name shapes specifically in case the work has already been done to
						// transform into the unobjectified state, in which case just clearing the collection
						// would blow away the ObjectTypeShape elements as well as the name shapes.
						ObjectifiedFactTypeNameShape objectifiedNameShape = presentationElements[i] as ObjectifiedFactTypeNameShape;
						if (objectifiedNameShape != null)
						{
							objectifiedNameShape.Delete();
						}
					}
				}
			}

			// Part2: Decide whether to keep the FactTypeShape, replace
			// it with an ObjectTypeShape, or have both shapes. Move
			// links as appropriate if an ObjectTypeShape is created.
			ORMModel nestingTypeModel = nestingTypeRemoved ? null : nestingType.Model;
			if (!nestedFactTypeRemoved)
			{
				LinkedElementCollection<PresentationElement> presentationElements = PresentationViewsSubject.GetPresentation(nestedFactType);
				for (int i = presentationElements.Count - 1; i >= 0; --i)
				{
					FactTypeShape factShape = presentationElements[i] as FactTypeShape;
					if (factShape != null)
					{
						if (!nestingTypeRemoved && !switchingToImplied)
						{
							// See if any links coming in apply to the ObjectType
							bool needObjectShape = false;
							bool needFactShape = false;
							// The contents of the enumerator change over the course of this function, get a snapshot enumerator
							IEnumerable<BinaryLinkShape> factShapeAttachedLinkShapes = MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(factShape, true);
							foreach (BinaryLinkShape linkShape in factShapeAttachedLinkShapes)
							{
								RolePlayerLink rolePlayerLink;
								if (linkShape is SubtypeLink ||
									(null != (rolePlayerLink = linkShape as RolePlayerLink) &&
									factShape == MultiShapeUtility.ResolvePrimaryShape(rolePlayerLink.ToShape)))
								{
									needObjectShape = true;
									if (needFactShape)
									{
										break;
									}
								}
								else
								{
									needFactShape = true;
									if (needObjectShape)
									{
										break;
									}
								}
							}
							ObjectifiedFactTypeNameShape objectifiedNameShape = null;
							ReadOnlyCollection<LinkConnectsToNode> nameShapeLinks = null; // Handle note connectors and anything an extension element model might add
							foreach (ShapeElement relativeShape in factShape.RelativeChildShapes)
							{
								if (null != (objectifiedNameShape = relativeShape as ObjectifiedFactTypeNameShape))
								{
									nameShapeLinks = LinkConnectsToNode.GetLinksToLink(objectifiedNameShape);
									if (nameShapeLinks.Count != 0)
									{
										needObjectShape = true;
									}
									else
									{
										nameShapeLinks = null;
									}
									break;
								}
							}
							if (!needObjectShape)
							{
								if (!needFactShape && !needObjectShape)
								{
									// We need to keep something at this spot on the screen
									needFactShape = true;
								}
							}
							ORMDiagram currentDiagram = factShape.Diagram as ORMDiagram;
							if (needObjectShape)
							{
								RectangleD bounds = factShape.AbsoluteBounds;
								if (needFactShape)
								{
									factShape.AutoResize();
									bounds.Offset(0, factShape.Size.Height * 2);
								}
								currentDiagram.PlaceElementOnDiagram(
									nestingType,
									bounds.Location,
									ORMPlacementOption.AllowMultipleShapes,
									delegate(ModelElement element, ShapeElement newShape)
									{
										ObjectTypeShape newObjectShape = (ObjectTypeShape)newShape;
										newObjectShape.DisplayRelatedTypes = factShape.DisplayRelatedTypes;
										if (objectifiedNameShape != null)
										{
											newObjectShape.ExpandRefMode = objectifiedNameShape.ExpandRefMode;
										}

										foreach (BinaryLinkShape linkShape in factShapeAttachedLinkShapes)
										{
											SubtypeLink subtypeLink;
											RolePlayerLink rolePlayerLink;
											if (null != (subtypeLink = linkShape as SubtypeLink))
											{
												if (subtypeLink.ToShape == factShape)
												{
													subtypeLink.ToShape = newObjectShape;
												}
												else
												{
													subtypeLink.FromShape = newObjectShape;
												}
											}
											else if (null != (rolePlayerLink = linkShape as RolePlayerLink) &&
												factShape == MultiShapeUtility.ResolvePrimaryShape(rolePlayerLink.ToShape))
											{
												rolePlayerLink.ToShape = newObjectShape;
											}
										}
										// Move links from the name shape
										if (nameShapeLinks != null)
										{
											foreach (LinkConnectsToNode linkConnector in nameShapeLinks)
											{
												BinaryLinkShape connector = linkConnector.Link as BinaryLinkShape;
												if (connector != null)
												{
													if (connector.ToShape == objectifiedNameShape)
													{
														connector.ToShape = newObjectShape;
													}
													else
													{
														connector.FromShape = newObjectShape;
													}
												}
											}
										}
									});
							}
							else if (needFactShape)
							{
								factShape.AutoResize();
							}
							if (!needFactShape)
							{
								factShape.Delete();
							}
							else if (objectifiedNameShape != null)
							{
								objectifiedNameShape.Delete();
							}
						}
						else
						{
							factShape.AutoResize();
						}
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectificationIsImpliedChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == Objectification.IsImpliedDomainPropertyId)
			{
				if ((bool)e.OldValue)
				{
					// It was previously implied
					if (!(bool)e.NewValue)
					{
						// It is now explicit
						ProcessObjectificationAdd(e.ModelElement as Objectification, null, null);
					}
				}
				else
				{
					// It was previously explicit
					if ((bool)e.NewValue)
					{
						// It is now implied
						ProcessObjectificationDelete(e.ModelElement as Objectification, null, null, true);
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// </summary>
		private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			Objectification link = e.ElementLink as Objectification;
			if (link.IsDeleted)
			{
				return;
			}
			Guid changedRoleGuid = e.DomainRole.Id;
			ObjectType oldObjectType = null;
			FactType oldFactType = null;
			if (changedRoleGuid == Objectification.NestingTypeDomainRoleId)
			{
				oldObjectType = (ObjectType)e.OldRolePlayer;
			}
			else
			{
				oldFactType = (FactType)e.OldRolePlayer;
			}
			ProcessObjectificationDelete(link, oldFactType, oldObjectType, false);
			if (!link.IsImplied)
			{
				ProcessObjectificationAdd(link, null, null);
			}
		}
		#region ConnectionPropertyChangeRule
		/// <summary>
		/// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// </summary>
		private static void ConnectionPropertyChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			bool orientationChange = false;
			if (attributeId == ConstraintDisplayPositionDomainPropertyId ||
				(orientationChange = (attributeId == DisplayOrientationDomainPropertyId)))
			{
				FactTypeShape factTypeShape = e.ModelElement as FactTypeShape;
				if (!factTypeShape.IsDeleted)
				{
					foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(factTypeShape))
					{
						binaryLink.RecalculateRoute();
					}
					if (orientationChange)
					{
						// Seed the RolesPosition value and adjust the location so that
						// any AutoSize changes make the fact type shape look like it
						// rotated around the center of the role box
						switch ((DisplayOrientation)e.OldValue)
						{
							case DisplayOrientation.Horizontal:
								{
									RectangleD bounds = factTypeShape.AbsoluteBounds;
									SizeD size = bounds.Size;
									PointD location = bounds.Location;
									double halfWidth = size.Width / 2;
									location.Offset(0, factTypeShape.RolesPosition - halfWidth);
									factTypeShape.AbsoluteBounds = new RectangleD(location, size);
									factTypeShape.RolesPosition = halfWidth;
									break;
								}
							case DisplayOrientation.VerticalRotatedRight:
							case DisplayOrientation.VerticalRotatedLeft:
								if ((DisplayOrientation)e.NewValue == DisplayOrientation.Horizontal)
								{
									RectangleD bounds = factTypeShape.AbsoluteBounds;
									SizeD size = bounds.Size;
									PointD location = bounds.Location;
									double halfHeight = size.Height / 2;
									location.Offset(factTypeShape.RolesPosition - halfHeight, 0);
									factTypeShape.AbsoluteBounds = new RectangleD(location, size);
									factTypeShape.RolesPosition = halfHeight;
								}
								break;
						}
					}
					SizeD oldSize = factTypeShape.Size;
					factTypeShape.AutoResize();
					if (oldSize == factTypeShape.Size)
					{
						factTypeShape.InvalidateRequired(true);
					}
				}
			}
			else if (attributeId == DisplayRelatedTypesDomainPropertyId)
			{
				FactTypeShape factTypeShape = e.ModelElement as FactTypeShape;
				if (!factTypeShape.IsDeleted)
				{
					MultiShapeUtility.AttachLinkConfigurationChanged(factTypeShape);
				}
			}
		}
		#endregion // ConnectionPropertyChangeRule
		#region ExternalConstraintShapeChangeRule
		/// <summary>
		/// ChangeRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// Rule to force the external constraint link bars to redraw and/or reposition
		/// when an external constraint shape is moved.
		/// </summary>
		private static void ExternalConstraintShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == ExternalConstraintShape.AbsoluteBoundsDomainPropertyId)
			{
				ExternalConstraintShape externalConstraintShape = e.ModelElement as ExternalConstraintShape;
				if (!externalConstraintShape.IsDeleted)
				{
					foreach (BinaryLinkShape binaryLink in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(externalConstraintShape))
					{
						Debug.Assert(binaryLink.ToShape == externalConstraintShape);
						FactTypeShape factShape = MultiShapeUtility.ResolvePrimaryShape(binaryLink.FromShape) as FactTypeShape;
						if (factShape != null)
						{
							IFactConstraint factConstraint = binaryLink.ModelElement as IFactConstraint;
							IList<Role> roles;
							if (null != (factConstraint = binaryLink.ModelElement as IFactConstraint) &&
								null != (roles = factConstraint.RoleCollection))
							{
								ExternalConstraintRoleBarDisplay displayOption = OptionsPage.CurrentExternalConstraintRoleBarDisplay;
								bool constraintBarVisible;
								LinkedElementCollection<RoleBase> factRoles = null;
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
											if (role0 == role1)
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
									foreach (BinaryLinkShape binaryLinkToFact in MultiShapeUtility.GetEffectiveAttachedLinkShapes<BinaryLinkShape>(factShape))
									{
										if (binaryLink != binaryLinkToFact)
										{
											binaryLinkToFact.RecalculateRoute();
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
		#endregion // ExternalConstraintShapeChangeRule
		#region FactTypeShapeChangeRule
		/// <summary>
		/// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Keep relative child elements a fixed distance away from the fact
		/// when the shape changes.
		/// </summary>
		private static void FactTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
		{
			MaintainRelativeShapeOffsetsForBoundsChange(e);
		}
		#endregion // FactTypeShapeChangeRule
		#region RoleName fixup
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
		/// Add shape elements for role names. Used during deserialization fixup
		/// and rules.
		/// </summary>
		private static void RoleChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == Role.NameDomainPropertyId)
			{
				Role role = (Role)e.ModelElement;
				if (!role.IsDeleted)
				{
					FixupRoleNameShapes(role);
				}
			}
		}
		private static void FixupRoleNameShapes(Role role)
		{
			if (string.IsNullOrEmpty(role.Name))
			{
				RoleNameShape.RemoveRoleNameShapeFromRole(role);
			}
			else
			{
				Diagram.FixUpDiagram(role.FactType, role);
				if (OptionsPage.CurrentRoleNameDisplay == RoleNameDisplay.Off)
				{
					foreach (PresentationElement element in PresentationViewsSubject.GetPresentation(role.FactType))
					{
						FactTypeShape fts = element as FactTypeShape;
						if (fts != null
							&& fts.DisplayRoleNames == DisplayRoleNames.UserDefault)
						{
							RoleNameShape.SetRoleNameDisplay(role.FactType);
						}
					}
				}
			}
		}
		#endregion // RoleName fixup
		#region ImplicitBooleanValueTypeDeletedRule
		/// <summary>
		/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
		/// Deletion of an implicit boolean value type means that the FactType is no longer unary.
		/// </summary>
		private static void ImplicitBooleanValueTypeDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ModelElement;
			ObjectType impliedBoolean = link.RolePlayer;
			Role role;
			if (impliedBoolean.IsImplicitBooleanValue &&
				!(role = link.PlayedRole).IsDeleted)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(role.FactType))
				{
					FactTypeShape shape = pel as FactTypeShape;
					if (shape != null)
					{
						// When a binarized Unary Fact is no longer a unary, clear the displayed role orders
						shape.RoleDisplayOrderCollection.Clear();
						shape.AutoResize();
					}
				}
				if (!string.IsNullOrEmpty(role.Name))
				{
					FixupRoleNameShapes(role);
				}
			}
		}
		#endregion // ImplicitBooleanValueTypeDeletedRule
		#endregion // Shape display update rules
		#region Store Event Handlers
		/// <summary>
		///  Helper function to update the mandatory dot in response to events
		/// </summary>
		private static void UpdateDotDisplayOnMandatoryConstraintChange(Role role)
		{
			foreach (ModelElement mel in DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(role, ObjectTypePlaysRole.PlayedRoleDomainRoleId))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(mel))
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
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="FactTypeShape"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static new void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(UniquenessConstraint.ModalityDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(InternalConstraintChangedEvent), action);

			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(EntityTypeHasPreferredIdentifier.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(PreferredIdentifierAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(PreferredIdentifierRemovedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(PreferredIdentifierRolePlayerChangedEvent), action);
		}
		/// <summary>
		/// Update the link displays when the modality of an internal uniqueness constraint changes
		/// </summary>
		private static void InternalConstraintChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			UniquenessConstraint iuc = e.ModelElement as UniquenessConstraint;
			LinkedElementCollection<FactType> factTypes;
			if (iuc != null &&
				!iuc.IsDeleted &&
				iuc.IsInternal &&
				1 == (factTypes = iuc.FactTypeCollection).Count)
			{
				FactType factType = factTypes[0];
				if (factType != null && !factType.IsDeleted)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
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
		/// Helper function for internal preferred identifier invalidation
		/// </summary>
		private static void InvalidateForPreferredIdentifier(UniquenessConstraint preferredIdentifier)
		{
			LinkedElementCollection<FactType> factTypes;
			FactType factType;
			if (!preferredIdentifier.IsDeleted &&
				preferredIdentifier.IsInternal &&
				1 == (factTypes = preferredIdentifier.FactTypeCollection).Count &&
				null != (factType = factTypes[0]))
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(factType))
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
		/// Event handler that listens for preferred identifiers being added.
		/// </summary>
		public static void PreferredIdentifierAddedEvent(object sender, ElementAddedEventArgs e)
		{
			InvalidateForPreferredIdentifier(((EntityTypeHasPreferredIdentifier)e.ModelElement).PreferredIdentifier);
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers being removed.
		/// </summary>
		public static void PreferredIdentifierRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			InvalidateForPreferredIdentifier(((EntityTypeHasPreferredIdentifier)e.ModelElement).PreferredIdentifier);
		}
		/// <summary>
		/// Event handler that listens for preferred identifiers role player changes.
		/// </summary>
		public static void PreferredIdentifierRolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId)
			{
				InvalidateForPreferredIdentifier((UniquenessConstraint)e.OldRolePlayer);
				InvalidateForPreferredIdentifier((UniquenessConstraint)e.NewRolePlayer);
			}
		}
		#endregion // Store Event Handlers
		#region CustomFactTypeShapeGeometry
		/// <summary>
		/// We're using a custom shape geometry to move the focus line in. The border
		/// width has been adjusted to make it easier to select the fact shape, but we
		/// don't want to add extra width for the normal selection outline.
		/// </summary>
		private sealed class CustomFactTypeShapeGeometry : CustomFoldRectangleShapeGeometry
		{
			public new static readonly ShapeGeometry ShapeGeometry = new CustomFactTypeShapeGeometry();
			protected sealed override double GetFocusIndicatorInsideMargin(IGeometryHost geometryHost)
			{
				return FocusIndicatorInsideMargin;
			}
			/// <summary>
			/// Override GetPerimeterBoundingBox to ignore outline pen when the outline is not displayed
			/// UNDONE: MSBUG The framework should check HasOutline before using the outline pen
			/// </summary>
			protected sealed override RectangleD GetPerimeterBoundingBox(IGeometryHost geometryHost)
			{
				if (geometryHost.GeometryHasOutline)
				{
					return base.GetPerimeterBoundingBox(geometryHost);
				}
				return geometryHost.GeometryBoundingBox;
			}
		}
		#endregion // CustomFactTypeShapeGeometry
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// ensures that non-serialized information on a FactTypeShape is properly restored
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new FactTypeShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset non-serialized FactTypeShape properties.
		/// </summary>
		private sealed class FactTypeShapeFixupListener : DeserializationFixupListener<FactTypeShape>
		{
			/// <summary>
			/// Create a new FactTypeShapeFixupListener
			/// </summary>
			public FactTypeShapeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateStoredPresentationElements)
			{
			}
			/// <summary>
			/// Update the non-serialized RolesPosition property, used to keep a FactTypeShape
			/// anchored on the center of the roles box
			/// </summary>
			protected sealed override void ProcessElement(FactTypeShape element, Store store, INotifyElementAdded notifyAdded)
			{
				PointD centerPoint = RolesShape.GetBounds(element).Center;
				element.RolesPosition = (element.DisplayOrientation != DisplayOrientation.Horizontal) ? centerPoint.X : centerPoint.Y;
			}
		}
		#endregion Deserialization Fixup
	}
	#endregion // FactTypeShape class
	#region ObjectifiedFactTypeNameShape class
	/// <summary>
	/// A specialized display of the nesting type as a relative
	/// child element of an objectified fact type
	/// </summary>
	public partial class ObjectifiedFactTypeNameShape : IModelErrorActivation, ISelectionContainerFilter, IConfigureAsChildShape
	{
		#region ObjectifiedFactTypeNameShape specific
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
		#endregion // ObjectifiedFactTypeNameShape specific
		#region Shape initialize overrides
		/// <summary>
		/// Move a new name label above the parent fact type shape
		/// </summary>
		public override void PlaceAsChildOf(NodeShape parent, bool createdDuringViewFixup)
		{
			AutoResize();
			SizeD size = Size;
			if (createdDuringViewFixup)
			{
				Location = new PointD(0, -1.5 * size.Height);
			}
			foreach (ShapeElement childShape in RelativeChildShapes)
			{
				ORMBaseShape shape = childShape as ORMBaseShape;
				if (shape != null)
				{
					// Child configuration is the most reliable spot
					// for creating any value shape on this shape. However,
					// it has the side effect of placing the value element
					// before this one is placed. Repeat the call.
					shape.PlaceAsChildOf(this, createdDuringViewFixup);
				}
			}
		}
		/// <summary>
		/// Allow a role value constraint to attach to this object shape.
		/// Caters for reference modes on objectified facts.
		/// </summary>
		protected override bool ShouldAddShapeForElement(ModelElement element)
		{
			if (element is RoleValueConstraint)
			{
				return true;
			}
			return base.ShouldAddShapeForElement(element);
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
		#endregion // Shape initialize overrides
		#region IConfigureAsChildShape Members
		/// <summary>
		/// Implements <see cref="IConfigureAsChildShape.ConfiguringAsChildOf"/>
		/// </summary>
		protected void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ObjectType objectType;
			ValueConstraint valueConstraint;
			ORMDiagram diagram;
			if (createdDuringViewFixup &&
				null != (objectType = AssociatedObjectType) &&
				null != (valueConstraint = objectType.FindValueConstraint(false)) &&
				null != (diagram = parentShape.Diagram as ORMDiagram))
			{
				diagram.FixUpLocalDiagram(this, valueConstraint);
			}
		}
		void IConfigureAsChildShape.ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup)
		{
			ConfiguringAsChildOf(parentShape, createdDuringViewFixup);
		}

		#endregion
		#region Customize appearance
		/// <summary>
		/// Connect lines to the edge of the rectangular shape
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				return CustomFoldRectangleShapeGeometry.ShapeGeometry;
			}
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
		/// Show a tooltip containing the <see cref="ObjectType"/> definition text.
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
		#region IModelErrorActivation Implementation
		/// <summary>
		/// Implements IModelErrorActivation.ActivateModelError for activatable displayed errors
		/// </summary>
		protected bool ActivateModelError(ModelError error)
		{
			ObjectTypeDuplicateNameError duplicateName;
			EntityTypeRequiresReferenceSchemeError requiresReferenceScheme;
			PopulationMandatoryError populationMandatory;
			bool retVal = true;
			if (null != (duplicateName = error as ObjectTypeDuplicateNameError))
			{
				ActivateNameProperty(duplicateName.ObjectTypeCollection[0]);
			}
			else if (null != (requiresReferenceScheme = error as EntityTypeRequiresReferenceSchemeError))
			{
				Store store = Store;
				EditorUtility.ActivatePropertyEditor(
					(store as IORMToolServices).ServiceProvider,
					DomainTypeDescriptor.CreatePropertyDescriptor(requiresReferenceScheme.ObjectType, ObjectType.ReferenceModeDisplayDomainPropertyId),
					true);
			}
			else if (error is ObjectifiedInstanceRequiredError || error is TooFewEntityTypeRoleInstancesError)
			{
				ORMDesignerPackage.SamplePopulationEditorWindow.ActivateModelError(error);
			}
			else if (null != (populationMandatory = error as PopulationMandatoryError))
			{
				retVal = ORMDesignerPackage.SamplePopulationEditorWindow.AutoCorrectMandatoryError(populationMandatory, AssociatedObjectType);
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
		private static AutoSizeTextField myTextShapeField;
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
		/// Create a text field that will correctly display objectified type names
		/// </summary>
		/// <param name="fieldName">Non-localized name for the field</param>
		protected override AutoSizeTextField CreateAutoSizeTextField(string fieldName)
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
				string baseText = base.GetDisplayText(parentShape);
				ObjectType objectType = parentShape.ModelElement as ObjectType;
				string formatString = null;
				string refModeString = "";
				if (objectType != null)
				{
					bool independent = objectType.IsIndependent;
					bool derived = !independent && objectType.DerivationRule != null && objectType.IsSubtype;
					refModeString = objectType.ReferenceModeDecoratedString;
					if (refModeString.Length != 0)
					{
						formatString = independent ?
							ResourceStrings.ObjectifiedFactTypeNameShapeRefModeIndependentFormatString :
							derived ?
								ResourceStrings.ObjectifiedFactTypeNameShapeRefModeDerivedSubtypeFormatString :
								ResourceStrings.ObjectifiedFactTypeNameShapeRefModeFormatString;
					}
					else if (independent)
					{
						formatString = ResourceStrings.ObjectifiedFactTypeNameShapeIndependentFormatString;
					}
					else if (derived)
					{
						formatString = ResourceStrings.ObjectifiedFactTypeNameShapeDerivedSubtypeFormatString;
					}
					else
					{
						formatString = ResourceStrings.ObjectifiedFactTypeNameShapeStandardFormatString;
					}
				}
				return (formatString == null) ? baseText : string.Format(CultureInfo.InvariantCulture, formatString, baseText, refModeString);
			}
		}
		#endregion // ObjectNameTextField class
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener ensures
		/// the correct size for an <see cref="ObjectifiedFactTypeNameShape"/>.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ObjectifiedFactTypeNameShapeFixupListener();
			}
		}
		/// <summary>
		/// A listener to reset the size of an ObjectifiedFactTypeNameShape.
		/// </summary>
		private sealed class ObjectifiedFactTypeNameShapeFixupListener : DeserializationFixupListener<ObjectifiedFactTypeNameShape>
		{
			/// <summary>
			/// Create a new ObjectifiedFactTypeNameShapeFixupListener
			/// </summary>
			public ObjectifiedFactTypeNameShapeFixupListener()
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
			protected sealed override void ProcessElement(ObjectifiedFactTypeNameShape element, Store store, INotifyElementAdded notifyAdded)
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
	}
	#endregion // ObjectifiedFactTypeNameShape class
	#region FactTypeLinkConnectorShape class
	public partial class FactTypeLinkConnectorShape : ICustomShapeFolding
	{
		#region ICustomShapeFolding Implementation
		/// <summary>
		/// Implements ICustomShapeFolding.CalculateConnectionPoint
		/// </summary>
		protected PointD CalculateConnectionPoint(NodeShape oppositeShape)
		{
			PointD retVal = PointD.Empty;
			FactTypeShape parentShape = ParentShape as FactTypeShape;
			if (parentShape != null)
			{
				retVal = parentShape.CalculateConnectionPoint(oppositeShape, this);
			}
			return retVal;
		}
		PointD ICustomShapeFolding.CalculateConnectionPoint(NodeShape oppositeShape)
		{
			return CalculateConnectionPoint(oppositeShape);
		}
		#endregion // ICustomShapeFolding Implementation
		#region Modified shape geometry
		/// <summary>
		/// Return a shape geometry that supports shape folding
		/// </summary>
		public override ShapeGeometry ShapeGeometry
		{
			get
			{
				// Note that it doesn't much matter which shape we return
				// because connector shapes have zero size. We just need
				// one that will call into our CalculateConnectionPoint routine.
				return CustomFoldRectangleShapeGeometry.ShapeGeometry;
			}
		}
		#endregion // Modified shape geometry
	}
	#endregion // FactTypeLinkConnectorShape class
}
