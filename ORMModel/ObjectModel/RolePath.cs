#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Globalization;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region PathNodeVisitor delegate
	/// <summary>
	/// Callback for <see cref="RolePathNode.VisitPathNodes"/>
	/// </summary>
	/// <param name="currentPathNode">The current path node</param>
	/// <param name="previousPathNode">The previous pathed role</param>
	/// <param name="unwinding">The stack is unwinding.</param>
	/// <returns>Return <see langword="true"/> to continue iteration.</returns>
	public delegate bool PathNodeVisitor(RolePathNode currentPathNode, RolePathNode previousPathNode, bool unwinding);
	#endregion // PathNodeVisitor delegate
	#region RolePathNode struct
	/// <summary>
	/// A structure representing either the <see cref="RolePathObjectTypeRoot"/> of a
	/// <see cref="RolePath"/> with an associated <see cref="P:RolePath.RootObjectType"/>
	/// or a <see cref="PathedRole"/>.
	/// </summary>
	public struct RolePathNode
	{
		#region Constructor and Fields
		private readonly object myPathObject;
		private readonly object myContext;
		/// <summary>
		/// An empty <see cref="RolePathNode"/>
		/// </summary>
		public static readonly RolePathNode Empty = default(RolePathNode);
		/// <summary>
		/// Create a <see cref="RolePathNode"/> for a <see cref="PathedRole"/>
		/// </summary>
		public RolePathNode(PathedRole pathedRole)
		{
			myPathObject = pathedRole;
			myContext = null;
		}
		/// <summary>
		/// Create a <see cref="RolePathNode"/> for a <see cref="RolePathObjectTypeRoot"/>
		/// </summary>
		public RolePathNode(RolePathObjectTypeRoot pathRoot)
		{
			myPathObject = pathRoot;
			myContext = null;
		}
		/// <summary>
		/// Create a <see cref="RolePathNode"/> for a <see cref="PathedRole"/>
		/// and a context object, which allows for tracking multiple uses of a single path.
		/// </summary>
		public RolePathNode(PathedRole pathedRole, object context)
		{
			myPathObject = pathedRole;
			myContext = context;
		}
		/// <summary>
		/// Create a <see cref="RolePathNode"/> for a <see cref="RolePathObjectTypeRoot"/>
		/// and a context object, which allows for tracking multiple uses of a single path.
		/// </summary>
		public RolePathNode(RolePathObjectTypeRoot pathRoot, object context)
		{
			myPathObject = pathRoot;
			myContext = context;
		}
		/// <summary>
		/// Create a new <see cref="RolePathNode"/> from an existing node and
		/// and a context object, which allows for tracking multiple uses of a single path.
		/// </summary>
		public RolePathNode(RolePathNode existingNode, object context)
		{
			myPathObject = existingNode.myPathObject;
			myContext = context;
		}
		#endregion // Constructor and Fields
		#region Accessor Properties
		/// <summary>
		/// Test if this is an empty node
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myPathObject == null && myContext == null;
			}
		}
		/// <summary>
		/// Get the <see cref="ObjectType"/> associated with this node.
		/// </summary>
		public ObjectType ObjectType
		{
			get
			{
				object pathObject = myPathObject;
				if (null != pathObject)
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					if (null != (pathedRole = pathObject as PathedRole))
					{
						return pathedRole.Role.RolePlayer;
					}
					else if (null != (pathRoot = pathObject as RolePathObjectTypeRoot))
					{
						return pathRoot.RootObjectType;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Get the <see cref="RolePath"/> associated with this node.
		/// </summary>
		public RolePath RolePath
		{
			get
			{
				object pathObject = myPathObject;
				if (null != pathObject)
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					if (null != (pathedRole = pathObject as PathedRole))
					{
						return pathedRole.RolePath;
					}
					else if (null != (pathRoot = pathObject as RolePathObjectTypeRoot))
					{
						return pathRoot.RolePath;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Get the <see cref="PathObjectUnifier"/> associated with this node.
		/// </summary>
		/// <remarks>This retrieves the direct object unifier. It does not attempt
		/// to resolve a <see cref="P:PathedRole"/> to a same fact type role or
		/// path root that can be unified. So, if the pathed role is not a same fact
		/// type role, then it will never be unified.</remarks>
		public PathObjectUnifier ObjectUnifier
		{
			get
			{
				object pathObject = myPathObject;
				if (null != pathObject)
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					if (null != (pathedRole = pathObject as PathedRole))
					{
						return pathedRole.ObjectUnifier;
					}
					else if (null != (pathRoot = pathObject as RolePathObjectTypeRoot))
					{
						return pathRoot.ObjectUnifier;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Retrieve the <see cref="PathedRole"/>, if any, associated with this <see cref="RolePathNode"/>
		/// </summary>
		public PathedRole PathedRole
		{
			get
			{
				return myPathObject as PathedRole;
			}
		}
		/// <summary>
		/// Retrieve the <see cref="RolePathObjectTypeRoot"/>, if any, associated with this <see cref="RolePathNode"/>
		/// </summary>
		public RolePathObjectTypeRoot PathRoot
		{
			get
			{
				return myPathObject as RolePathObjectTypeRoot;
			}
		}
		/// <summary>
		/// Retrieve the context object specified with the constructor.
		/// </summary>
		public object Context
		{
			get
			{
				return myContext;
			}
		}
		#endregion // Accessor Properties
		#region Equality and casting routines
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is RolePathNode)
			{
				return ((RolePathNode)obj).Equals(this);
			}
			return obj == myPathObject && myContext == null;
		}
		/// <summary>
		/// Standard GetHashCode override
		/// </summary>
		public override int GetHashCode()
		{
			object obj = myPathObject;
			if (obj != null)
			{
				object context = myContext;
				return (context == null) ? obj.GetHashCode() : Utility.GetCombinedHashCode(obj.GetHashCode(), context.GetHashCode());
			}
			return 0;
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(RolePathNode other)
		{
			object context = myContext;
			return myPathObject == other.myPathObject &&
				((context == null) ? other.myContext == null : context.Equals(other.myContext));
		}
		/// <summary>
		/// Equality operator
		/// </summary>
		public static bool operator ==(RolePathNode left, RolePathNode right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Inequality operator
		/// </summary>
		public static bool operator !=(RolePathNode left, RolePathNode right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Automatically cast this structure to a <see cref="PathedRole"/>
		/// </summary>
		public static implicit operator PathedRole(RolePathNode pathNode)
		{
			return pathNode.myPathObject as PathedRole;
		}
		/// <summary>
		/// Automatically cast a <see cref="PathedRole"/> to this structure
		/// </summary>
		public static implicit operator RolePathNode(PathedRole pathedRole)
		{
			return (pathedRole == null) ? default(RolePathNode) : new RolePathNode(pathedRole);
		}
		/// <summary>
		/// Automatically cast this structure to a <see cref="RolePathObjectTypeRoot"/>
		/// </summary>
		public static implicit operator RolePathObjectTypeRoot(RolePathNode pathNode)
		{
			return pathNode.myPathObject as RolePathObjectTypeRoot;
		}
		/// <summary>
		/// Automatically cast a <see cref="RolePathObjectTypeRoot"/> to this structure
		/// </summary>
		public static implicit operator RolePathNode(RolePathObjectTypeRoot pathRoot)
		{
			return (pathRoot == null) ? default(RolePathNode) : new RolePathNode(pathRoot);
		}
		#endregion // Equality and casting routines
		#region Helper Methods
		/// <summary>
		/// Iterate rooted paths and pathed roles for validation
		/// </summary>
		/// <param name="rolePath">The <see cref="RolePath"/> to get nodes for.</param>
		/// <param name="contextPathNode">The <see cref="RolePathNode"/> above <paramref name="rolePath"/> in the path structure.</param>
		/// <param name="unwind">Should <paramref name="visitor"/> be invoked for both winding and unwinding the stack?</param>
		/// <param name="visitor">A <see cref="PathNodeVisitor"/> callback.</param>
		/// <returns>Returns <see langword="true"/> if a visitor did not cancel iteration.</returns>
		public static bool VisitPathNodes(RolePath rolePath, RolePathNode contextPathNode, bool unwind, PathNodeVisitor visitor)
		{
			RolePathNode currentContext = contextPathNode;
			RolePathNode rootContext = currentContext;
			RolePathObjectTypeRoot pathRoot = rolePath.PathRoot;
			if (pathRoot != null)
			{
				rootContext = pathRoot;
				if (!visitor(rootContext, currentContext, false))
				{
					return false;
				}
				currentContext = rootContext;
			}
			ReadOnlyCollection<PathedRole> pathedRoles = rolePath.PathedRoleCollection;
			int pathedRoleCount = pathedRoles.Count;
			for (int i = 0; i < pathedRoleCount; ++i)
			{
				RolePathNode node = pathedRoles[i];
				if (!visitor(node, currentContext, false))
				{
					return false;
				}
				currentContext = node;
			}
			foreach (RoleSubPath subPath in rolePath.SubPathCollection)
			{
				if (!VisitPathNodes(subPath, currentContext, unwind, visitor))
				{
					return false;
				}
			}
			if (unwind)
			{
				for (int i = pathedRoleCount - 1; i >= 0; --i)
				{
					if (!visitor(pathedRoles[i], i == 0 ? (pathRoot != null ? rootContext : contextPathNode) : (RolePathNode)pathedRoles[i - 1], true))
					{
						return false;
					}
				}
				if (pathRoot != null)
				{
					if (!visitor(pathRoot, contextPathNode, true))
					{
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Test if this node can be projected (or otherwise assigned to) a
		/// given type.
		/// </summary>
		/// <param name="targetType">The type to test.</param>
		public bool CanProjectOn(ObjectType targetType)
		{
			ObjectType currentType = this.ObjectType;
			if (currentType == targetType ||
					targetType == null ||
					currentType == null)
			{
				// This seems like an odd response for null types, but if we don't know
				// about one or both types, then there are validation errors
				// in the model that we assume will be cleaned up in
				// a consistent manner in the future. Until we have sufficient
				// data to definitively produce a negative response we
				// respond in the affirmative.
				return true;
			}
			if (currentType.IsValueType)
			{
				if (targetType.IsValueType)
				{
					return DataType.IsAssignableValueType(targetType, currentType);
				}

				// See if we can find a single identifying type that matches the current value type
				bool hasSingleMatchingIdentifier = false;
				ObjectType.WalkSupertypes(
					targetType,
					delegate(ObjectType type, int depth, bool isPrimary)
					{
						ObjectType identifyingValueType;
						if (null != (identifyingValueType = GetIdentifyingValueType(type)) &&
							identifyingValueType == currentType)
						{
							if (hasSingleMatchingIdentifier)
							{
								hasSingleMatchingIdentifier = false;
								return ObjectTypeVisitorResult.Stop;
							}
							else
							{
								hasSingleMatchingIdentifier = true;
							}
						}
						return ObjectTypeVisitorResult.Continue;
					});
				return hasSingleMatchingIdentifier;
			}
			else
			{
				// Current type is an entity type
				if (targetType.IsValueType)
				{
					// Find a single identifying value type and see if it is compatible with the
					// data type of the target value type.
					// Theoretically we could break down multiple context-dependent single-valued
					// identifiers, but that is very fragile as the data types change. Therefore,
					// we require explicit identifier assignment for this case and only allow
					// value type projection on an entity with a single identifying value type.
					ObjectType currentIdentifyingValueType = null;
					ObjectType.WalkSupertypes(
						currentType,
						delegate(ObjectType type, int depth, bool isPrimary)
						{
							ObjectType identifyingValueType;
							if (null != (identifyingValueType = GetIdentifyingValueType(type)))
							{
								if (currentIdentifyingValueType == null)
								{
									currentIdentifyingValueType = identifyingValueType;
								}
								else
								{
									currentIdentifyingValueType = null;
									return ObjectTypeVisitorResult.Stop;
								}
							}
							return ObjectTypeVisitorResult.Continue;
						});
					return currentIdentifyingValueType != null &&
						DataType.IsAssignableValueType(targetType, currentIdentifyingValueType);
				}

				// Both types are entity types, use subtype compatibility matching
				return ObjectType.GetNearestCompatibleTypes(new ObjectType[]{targetType, currentType}, true).Length != 0;
			}
		}
		/// <summary>
		/// Helper method for CanProjectOn. Find the identifying value type for an
		/// entity type without crossing a subtype, or null.
		/// </summary>
		private static ObjectType GetIdentifyingValueType(ObjectType entityType)
		{
			UniquenessConstraint pid;
			LinkedElementCollection<Role> pidRoles;
			ObjectType identifyingRolePlayer = null;
			if (null != (pid = entityType.PreferredIdentifier) && // Verify a local identification scheme
				1 == (pidRoles = pid.RoleCollection).Count)
			{
				Role identifyingRole = pidRoles[0];
				identifyingRolePlayer = identifyingRole.RolePlayer;
				if (!identifyingRolePlayer.IsValueType)
				{
					Role[] valueRoles = identifyingRole.GetValueRoles();
					identifyingRolePlayer = (valueRoles != null && valueRoles.Length != 0) ? valueRoles[0].RolePlayer : null;
				}
			}
			return identifyingRolePlayer;
		}
		#endregion // Helper Methods
	}
	#endregion // RolePathNode struct
	#region RolePath class
	partial class RolePath
	{
		#region Abstract Properties
		/// <summary>
		/// Return the root <see cref="LeadRolePath"/> associated
		/// with this path.
		/// </summary>
		public abstract LeadRolePath RootRolePath { get;}
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path component.
		/// </summary>
		public ORMModel Model
		{
			get
			{
				RolePathOwner owner = RootOwner;
				return (owner != null) ? owner.Model : null;
			}
		}
		/// <summary>
		/// Get the resolved <see cref="RolePathOwner"/> for this <see cref="RolePath"/>
		/// </summary>
		public RolePathOwner RootOwner
		{
			get
			{
				LeadRolePath leadRolePath = RootRolePath;
				return leadRolePath != null ? leadRolePath.PathOwner : null;
			}
		}
		#endregion // Abstract Properties
		#region Accessors Properties
		/// <summary>
		/// The root of a path with an explicit <see cref="RootObjectType"/>,
		/// represented by the relationship between the path and the root
		/// <see cref="ObjectType"/>
		/// /// </summary>
		public RolePathObjectTypeRoot PathRoot
		{
			get
			{
				return RolePathObjectTypeRoot.GetLinkToRootObjectType(this);
			}
		}
		/// <summary>
		/// Return the <see cref="PathedRole"/> relationships attached
		/// to this <see cref="RolePath"/>
		/// </summary>
		public ReadOnlyCollection<PathedRole> PathedRoleCollection
		{
			get
			{
				return PathedRole.GetLinksToRoleCollection(this);
			}
		}
		/// <summary>
		/// Get the <see cref="RolePathNode"/> that occurs immediately before
		/// this path. ContextPathNode is equivalent to <see cref="ContinuationContextPathNode"/>
		/// of the parent path.
		/// </summary>
		public RolePathNode ContextPathNode
		{
			get
			{
				RolePath currentPath = this;
				RoleSubPath subPath;
				while (currentPath != null &&
					null != (subPath = currentPath as RoleSubPath))
				{
					RolePath parentPath = subPath.ParentRolePath;
					ReadOnlyCollection<PathedRole> pathedRoles = parentPath.PathedRoleCollection;
					int pathRoleCount = pathedRoles.Count;
					if (pathRoleCount == 0)
					{
						RolePathObjectTypeRoot parentRoot = parentPath.PathRoot;
						if (parentRoot != null)
						{
							return parentRoot;
						}
						currentPath = parentPath;
					}
					else
					{
						return pathedRoles[pathRoleCount - 1];
					}
				}
				return RolePathNode.Empty;
			}
		}
		/// <summary>
		/// Get the <see cref="RolePathNode"/> for the root of this path
		/// or the <see cref="ContextPathNode"/> for the path if there is no
		/// root.
		/// </summary>
		public RolePathNode LeadPathedRoleContextPathNode
		{
			get
			{
				RolePath currentPath = this;
				RolePathObjectTypeRoot pathRoot = currentPath.PathRoot;
				if (pathRoot != null)
				{
					return pathRoot;
				}
				RoleSubPath subPath;
				while (currentPath != null &&
					null != (subPath = currentPath as RoleSubPath))
				{
					RolePath parentPath = subPath.ParentRolePath;
					ReadOnlyCollection<PathedRole> pathedRoles = parentPath.PathedRoleCollection;
					int pathRoleCount = pathedRoles.Count;
					if (pathRoleCount == 0)
					{
						RolePathObjectTypeRoot parentRoot = parentPath.PathRoot;
						if (parentRoot != null)
						{
							return parentRoot;
						}
						currentPath = parentPath;
					}
					else
					{
						return pathedRoles[pathRoleCount - 1];
					}
				}
				return RolePathNode.Empty;
			}
		}
		/// <summary>
		/// Get the <see cref="RolePathNode"/> that represents the end of this
		/// path. ContinuationContextPathNode is equivalent to <see cref="ContextPathNode"/>
		/// of any path that is a continuation of this path.
		/// </summary>
		public RolePathNode ContinuationContextPathNode
		{
			get
			{
				ReadOnlyCollection<PathedRole> pathedRoles = PathedRoleCollection;
				int pathedRoleCount = pathedRoles.Count;
				if (pathedRoleCount != 0)
				{
					return pathedRoles[pathedRoleCount - 1];
				}
				RolePathObjectTypeRoot pathRoot = PathRoot;
				if (pathRoot != null)
				{
					return pathRoot;
				}
				// This path offers no additional context, so continuations
				// have the same context as this path.
				return ContextPathNode;
			}
		}
		#endregion // Accessors Properties
		#region Delayed Validation
		/// <summary>
		/// Called when a subbranch is removed. If there is one remaining subbranch,
		/// then move the remaining elements in a single branch to the end of the current branch.
		/// </summary>
		/// <param name="element">A <see cref="RoleSubPath"/></param>
		private static void DelayValidatePathCollapse(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			RolePath parentPath = (RolePath)element;
			LinkedElementCollection<RoleSubPath> subPaths = parentPath.SubPathCollection;
			RolePath collapsePath;
			RolePathObjectTypeRoot collapsePathRoot;
			if (subPaths.Count == 1 &&
				(null == (collapsePathRoot = (collapsePath = subPaths[0]).PathRoot) ||
				(null == parentPath.PathRoot && parentPath.RoleCollection.Count == 0)))
			{
				// Remove the tail split by moving all elements up one level

				// Move the root if the child has a root and the parent does not
				if (collapsePathRoot != null)
				{
					collapsePathRoot.RolePath = parentPath;
				}

				// Move pathed roles
				foreach (PathedRole pathedRole in collapsePath.PathedRoleCollection)
				{
					pathedRole.RolePath = parentPath;
				}

				// Move sub paths
				foreach (RoleSubPathIsContinuationOfRolePath subPathLink in RoleSubPathIsContinuationOfRolePath.GetLinksToSubPathCollection(collapsePath))
				{
					subPathLink.ParentRolePath = parentPath;
				}

				// Change the parent split settings to the path we just collapsed
				parentPath.SplitIsNegated = collapsePath.SplitIsNegated;
				parentPath.SplitCombinationOperator = collapsePath.SplitCombinationOperator;

				// We're done with it. Note that this can trigger other rules which will
				// reenter this routine for the grandparent branch
				collapsePath.Delete();
			}
		}
		#endregion // Delayed Validation
		#region Rule Methods
		/// <summary>
		/// DeleteRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRole)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted &&
				rolePath.PathedRoleCollection.Count == 0 &&
				rolePath.SubPathCollection.Count == 0 &&
				rolePath.RootObjectType == null)
			{
				// UNDONE: RolePathSplitConditions
				rolePath.Delete();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches.
		/// </summary>
		private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == PathedRole.RolePathDomainRoleId)
			{
				RolePath rolePath = (RolePath)e.OldRolePlayer;
				if (!rolePath.IsDeleted &&
					rolePath.PathedRoleCollection.Count == 0 &&
					rolePath.SubPathCollection.Count == 0 &&
					rolePath.RootObjectType == null)
				{
					// UNDONE: RolePathSplitConditions
					rolePath.Delete();
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathObjectTypeRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate empty branches
		/// </summary>
		private static void RootObjectTypeDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((RolePathObjectTypeRoot)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted)
			{
				if (rolePath.RootObjectType == null)
				{
					RoleSubPath subPath;
					RolePath parentPath;
					int splitCount;
					if ((splitCount = rolePath.SubPathCollection.Count) == 0 &&
						rolePath.PathedRoleCollection.Count == 0)
					{
						// UNDONE: RolePathSplitConditions
						rolePath.Delete();
					}
					else
					{
						if (null != (subPath = rolePath as RoleSubPath) &&
							null != (parentPath = subPath.ParentRolePath))
						{
							// This child path might collapse into the parent.
							FrameworkDomainModel.DelayValidateElement(parentPath, DelayValidatePathCollapse);
						}

						// The remaining child for this path might collapse into this one if the root was removed.
						if (splitCount == 1)
						{
							FrameworkDomainModel.DelayValidateElement(rolePath, DelayValidatePathCollapse);
						}
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// If a subbranch is deleted, then delay validate if the parent branch should
		/// attempt to collapse a remaining branch.
		/// </summary>
		private static void SubPathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath parentRolePath = ((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath;
			if (!parentRolePath.IsDeleted)
			{
				if (parentRolePath.PathedRoleCollection.Count == 0 &&
					parentRolePath.SubPathCollection.Count == 0 &&
					parentRolePath.RootObjectType == null)
				{
					// UNDONE: RolePathSplitConditions
					parentRolePath.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Check branch collapsing for role player changes.
		/// </summary>
		private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId)
			{
				// The parent has lost a branch, validate if needed
				RolePath parentRolePath = (RolePath)e.OldRolePlayer;
				if (!parentRolePath.IsDeleted)
				{
					if (parentRolePath.PathedRoleCollection.Count == 0 &&
						parentRolePath.SubPathCollection.Count == 0 &&
						parentRolePath.RootObjectType == null)
					{
						// UNDONE: RolePathSplitConditions
						parentRolePath.Delete();
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(parentRolePath, DelayValidatePathCollapse);
					}
				}
			}
		}
		#endregion // Rule Methods
	}
	#endregion // RolePath class
	#region LeadRolePath class
	partial class LeadRolePath : IHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Base overrides
		/// <summary>
		/// This path is the root of the path
		/// </summary>
		public override LeadRolePath RootRolePath
		{
			get
			{
				return this;
			}
		}
		#endregion // Base overrides
		#region CustomStorage Handlers
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		private void SetNoteTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Note note = Note;
				if (note != null)
				{
					note.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Note = new Note(Partition, new PropertyAssignment(Note.TextDomainPropertyId, newValue));
				}
			}
		}
		#endregion // CustomStorage Handlers
		#region Helper Methods
		/// <summary>
		/// Helper method to find a single <see cref="RolePathNode"/> of a
		/// given type.
		/// </summary>
		/// <param name="type">The node type to search for.</param>
		/// <returns>A populated <see cref="RolePathNode"/> if one node is
		/// found, or an empty node if no node is found, or multiple non-correlated
		/// nodes of the type are found.</returns>
		public RolePathNode FindSingleNodeOfType(ObjectType type)
		{
			// The algorithm here should mirror RoleProjectedDerivationRule.ValidateAutomaticProjections
			// and ConstraintRoleSequenceJoinPath.ValidateAutomaticProjections
			RolePathNode matchedNode = RolePathNode.Empty;
			if (type != null)
			{
				PathObjectUnifier matchedUnifier = null;
				RolePathNode.VisitPathNodes(
					this,
					RolePathNode.Empty,
					false,
					delegate(RolePathNode pathNode, RolePathNode previousPathNode, bool unwinding)
					{
						PathedRole pathedRole;
						ObjectType startingType;
						if ((null != (pathedRole = pathNode.PathedRole) &&
							pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType) ||
							null == (startingType = pathNode.ObjectType) ||
							(matchedUnifier != null && pathNode.ObjectUnifier == matchedUnifier))
						{
							return true;
						}
						return ObjectType.WalkSupertypes(
									startingType,
									delegate(ObjectType testType, int depth, bool isPrimary)
									{
										if (testType == type)
										{
											if (matchedNode.IsEmpty)
											{
												matchedUnifier = (matchedNode = pathNode).ObjectUnifier;
											}
											else
											{
												// Duplicate match of a non-unified role, no single-typed node
												matchedNode = RolePathNode.Empty;
											}
											return ObjectTypeVisitorResult.Stop;
										}
										return ObjectTypeVisitorResult.Continue;
									}) ||
								!matchedNode.IsEmpty;
					});
			}
			return matchedNode;
		}
		#endregion // Helper Methods
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the component level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = PathOwner as IModelErrorDisplayContext;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // LeadRolePath class
	#region PathObjectUnifier class
	partial class PathObjectUnifier : IHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				IModelErrorDisplayContext deferTo = LeadRolePath as IModelErrorDisplayContext;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // PathObjectUnifier class
	#region RolePathOwner class
	partial class RolePathOwner : IModelErrorOwner
	{
		#region Abstract members
		/// <summary>
		/// Get the containing <see cref="ORMModel"/> for this path owner.
		/// </summary>
		public abstract ORMModel Model { get;}
		/// <summary>
		/// Test if <see cref="LeadRolePath"/> elements owned by this
		/// <see cref="RolePathOwner"/> can be shared with other owners.
		/// </summary>
		public virtual bool AllowOwnedPathSharing
		{
			get
			{
				return true;
			}
		}
		#endregion // Abstract members
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			List<ModelErrorUsage> errors = null;
			ModelError error;
			foreach (LeadRolePath rolePath in OwnedLeadRolePathCollection)
			{
				if (null != (error = rolePath.RootObjectTypeRequiredError))
				{
					yield return error;
				}
				// UNDONE: PathedRole and LeadRolePath should probably be their own model error owners,
				// although all validation (except for value constraints) will come through the owner.
				bool pathHasNodes = false;
				RolePathNode.VisitPathNodes(
					rolePath,
					RolePathNode.Empty,
					false,
					delegate(RolePathNode currentPathNode, RolePathNode previousPathNode, bool unwinding)
					{
						pathHasNodes = true;
						PathedRole pathedRole = currentPathNode;
						if (null != pathedRole)
						{
							if (previousPathNode.IsEmpty && // Root object types are only required at the root
								null != (error = pathedRole.RolePath.RootObjectTypeRequiredError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.SameFactTypeRoleWithoutJoinError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.JoinCompatibilityError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							if (null != (error = pathedRole.MandatoryOuterJoinError))
							{
								(errors ?? (errors = new List<ModelErrorUsage>())).Add(error);
							}
							ValueConstraint valueConstraint = pathedRole.ValueConstraint;
							if (valueConstraint != null)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}
						}
						else
						{
							RolePathObjectTypeRoot pathRoot = currentPathNode;
							ValueConstraint valueConstraint = pathRoot.ValueConstraint;
							if (valueConstraint != null)
							{
								foreach (ModelErrorUsage valueConstraintErrorUsage in ((IModelErrorOwner)valueConstraint).GetErrorCollection(startFilter))
								{
									(errors ?? (errors = new List<ModelErrorUsage>())).Add(valueConstraintErrorUsage);
								}
							}
							// UNDONE: IntraPathRoot We'll need additional errors for path roots, including compatibility errors for set comparators
						}
						return true; // Continue iteration
					});
				if (errors != null && errors.Count != 0)
				{
					foreach (ModelErrorUsage errorUsage in errors)
					{
						yield return errorUsage;
					}
					errors.Clear();
				}
				else if (!pathHasNodes &&
					null != (error = rolePath.RootObjectTypeRequiredError))
				{
					yield return error;
				}
				foreach (PathObjectUnifier objectUnifier in rolePath.ObjectUnifierCollection)
				{
					if (null != (error = objectUnifier.CompatibilityError))
					{
						yield return error;
					}
				}
				foreach (CalculatedPathValue calculation in rolePath.CalculatedValueCollection)
				{
					if (null != (error = calculation.FunctionRequiredError))
					{
						yield return error;
					}
					foreach (CalculatedPathValueParameterBindingError bindingError in calculation.ParameterBindingErrorCollection)
					{
						yield return bindingError;
					}
					if (null != (error = calculation.AggregationContextRequiredError))
					{
						yield return error;
					}
					if (null != (error = calculation.ConsumptionRequiredError))
					{
						yield return error;
					}
				}
			}
			foreach (ModelErrorUsage errorUsage in base.GetErrorCollection(startFilter))
			{
				yield return errorUsage;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.ValidateErrors"/>
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateRolePaths(true, notifyAdded);
			ValidateDerivedRolePathOwner(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.DelayValidateErrors"/>
		/// </summary>
		protected new void DelayValidateErrors()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateLeadRolePathsWithCalculations);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Deserialization Fixup
		#region Deprecated Element Removal
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// replaces deprecated role path elements.
		/// </summary>
		public static IDeserializationFixupListener UpdateRolePathFixupListener
		{
			get
			{
				return new ReplaceDeprecatedElementsFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Replaces deprecated role path representations.
		/// </summary>
		private sealed class ReplaceDeprecatedElementsFixupListener : DeserializationFixupListener<RolePathOwner>
		{
			/// <summary>
			/// ReplaceDeprecatedElementsFixupListener constructor
			/// </summary>
			public ReplaceDeprecatedElementsFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Check that this was created on load, not programmatically during
			/// fixup. The only path owners that are automatically created are
			/// automatic join paths, which will be parented before this is called.
			/// </summary>
			protected override bool VerifyElementType(ModelElement element)
			{
				ConstraintRoleSequenceJoinPath joinPath = element as ConstraintRoleSequenceJoinPath;
				return joinPath == null || joinPath.RoleSequence == null;
			}
			/// <summary>
			/// Replace deprecated role path structures
			/// </summary>
			protected sealed override void ProcessElement(RolePathOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					// The deprecated patterns still supported in the schema and loader are:
					// 1) OLD: The role path owner had a single role path.
					//    NEW: The role path owner can specify multiple paths.
					// 2) OLD: CalculatedPathValue is stored at the level or the RolePathOwner.
					//    NEW: CalculatedPathValue is stored with each PathComponent it applies to.
					// 3) OLD: FactType and constraint role derivation is stored as a many-to-one
					//    relationship on the role or constraint role.
					//    NEW: Each top-level path or path combination can specify a projection.
					// 4) OLD: Function inputs and projections directly referenced pathed roles with
					//         a PathedRolePurpose of StartRole.
					//    NEW: StartRole is deprecated, and function inputs and projections reference
					//         role path roots instead of start roles.
					// 5) OLD: Pathed roles were directly correlated with a single pathed role parent,
					//         forming a hierarchy.
					//    NEW: Pathed roles and path roots are correlated with a parent PathObjectUnifier.
					// 6) OLD: CalculatedPathValue Scope referenced a single pathed role. Default scope indicated
					//         a scope at the path root.
					//    NEW: AggregationContext can reference multiple path nodes, UniversalAggregationContext
					//         property used otherwise. No context for an aggregate calculation is an error.
					//         Scope is no longer used for scalar functions.
					// 7) OLD: A subpath could lead with a non-join pathed role. This 'intra-fact type split'
					//         indicated branching within the roles of a single fact type.
					//    NEW: The intra-fact type split approach is difficult to use because it requires looking
					//         down sub paths to determine variables for a fact type entry, and it doesn't display
					//         well. More importantly, it does not actually solve the general problem of adding
					//         conditions to an inscope variable because variables can be in scope for both the
					//         most recent fact type and variables declared earlier in the path. The new approach
					//         deprecates the intra-fact type split, moves all pathed roles to the path with the
					//         initial entry role, and uses path roots and correlation in sub paths to state the
					//         conditions originally covered by intra-fact type splits. This pattern also allows
					//         additional conditions on all in-scope variables, which was not included in the
					//         intra-fact type split pattern.

					// Move from the single contained element to the many containment
					RolePathOwnerHasPathComponent_Deprecated rootContainmentLink = RolePathOwnerHasPathComponent_Deprecated.GetLinkToPathComponent(element);
					if (rootContainmentLink != null)
					{
						LeadRolePath leadRolePath = rootContainmentLink.RolePath;
						rootContainmentLink.Delete(); // Does not propagate, the path is now unparented
						notifyAdded.ElementAdded(new RolePathOwnerOwnsLeadRolePath(element, leadRolePath));
					}

					// Note that we shouldn't get these if we have no single root container, but the
					// XML still officially supports them. Given that functions for path combinations
					// came in after this change, we will reasonably expect calculated values to use
					// roles from a single LeadRolePath and delete them otherwise.
					ReadOnlyCollection<RolePathOwnerCalculatesCalculatedPathValue_Deprecated> calculatedValueLinks = RolePathOwnerCalculatesCalculatedPathValue_Deprecated.GetLinksToCalculatedValueCollection(element);
					bool checkedSingleRolePath = false;
					LeadRolePath singleLeadRolePath = null;
					bool noLeadRolePath = false;
					if (calculatedValueLinks.Count != 0)
					{
						FindSingleOwnedLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						checkedSingleRolePath = true;
						foreach (RolePathOwnerCalculatesCalculatedPathValue_Deprecated calculatedValueLink in calculatedValueLinks)
						{
							CalculatedPathValue calculatedValue = calculatedValueLink.CalculatedValue;
							LeadRolePathCalculatesCalculatedPathValue newCalculatedValueLink = null;
							calculatedValueLink.Delete();
							if (noLeadRolePath)
							{
								calculatedValue.Delete();
							}
							else if (singleLeadRolePath != null)
							{
								newCalculatedValueLink = new LeadRolePathCalculatesCalculatedPathValue(singleLeadRolePath, calculatedValue);
							}
							else
							{
								LeadRolePath parentRolePath = ResolveCalculatedValueRolePath(calculatedValue);
								if (parentRolePath != null)
								{
									newCalculatedValueLink = new LeadRolePathCalculatesCalculatedPathValue(parentRolePath, calculatedValue);
								}
								else
								{
									calculatedValue.Delete();
								}
							}
							if (newCalculatedValueLink != null)
							{
								notifyAdded.ElementAdded(newCalculatedValueLink);
							}
						}
					}

					// Port old projections on fact type derivation rules and join paths
					FactTypeDerivationRule factTypeDerivation;
					ConstraintRoleSequenceJoinPath joinPath = null;
					if (null != (factTypeDerivation = element as FactTypeDerivationRule))
					{
						if (!checkedSingleRolePath)
						{
							FindSingleOwnedLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						}
						LinkedElementCollection<RoleBase> factRoles = factTypeDerivation.FactType.RoleCollection;
						int factRoleCount = factRoles.Count;
						LeadRolePath resolvedLeadRolePath = null;
						RoleSetDerivationProjection derivationProjection = null;
						bool repeatLoop = true;
						while (repeatLoop)
						{
							repeatLoop = false;
							foreach (RoleBase roleBase in factRoles)
							{
								Role role = roleBase as Role;
								if (role != null)
								{
									RoleDerivesFromPathedRole_Deprecated sourcePathedRoleLink;
									RoleDerivesFromCalculatedPathValue_Deprecated sourceCalculatedValueLink;
									PathConstant pathConstant;
									RoleDerivesFromPathConstant_Deprecated sourceConstantLink;
									if (null != (sourcePathedRoleLink = RoleDerivesFromPathedRole_Deprecated.GetLinkToDerivedFromPathedRole(role)))
									{
										if (noLeadRolePath)
										{
											sourcePathedRoleLink.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											DerivedRoleProjection roleProjection = EnsureRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											roleProjection.ProjectedFromPathedRole = null;
											DerivedRoleProjectedFromPathedRole pathedRoleLink = new DerivedRoleProjectedFromPathedRole(roleProjection, sourcePathedRoleLink.Source);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(pathedRoleLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromCalculatedValue = null;
											roleProjection.ProjectedFromPathRoot = null;
											if (null != (pathConstant = roleProjection.ProjectedFromConstant))
											{
												pathConstant.Delete();
											}
											sourcePathedRoleLink.Delete();
										}
										else if (resolvedLeadRolePath == null)
										{
											resolvedLeadRolePath = sourcePathedRoleLink.Source.RolePath.RootRolePath;
											if (resolvedLeadRolePath == null)
											{
												noLeadRolePath = true;
												repeatLoop = true;
												break;
											}
										}
										else if (resolvedLeadRolePath != sourcePathedRoleLink.Source.RolePath.RootRolePath)
										{
											// Can't resolve the binding to a single path, treat the same as no lead role path
											resolvedLeadRolePath = null;
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (null != (sourceCalculatedValueLink = RoleDerivesFromCalculatedPathValue_Deprecated.GetLinkToDerivedFromCalculatedValue(role)))
									{
										if (noLeadRolePath)
										{
											sourceCalculatedValueLink.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											DerivedRoleProjection roleProjection = EnsureRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											roleProjection.ProjectedFromCalculatedValue = null;
											DerivedRoleProjectedFromCalculatedPathValue calculatedValueLink = new DerivedRoleProjectedFromCalculatedPathValue(roleProjection, sourceCalculatedValueLink.Source);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(calculatedValueLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromPathedRole = null;
											roleProjection.ProjectedFromPathRoot = null;
											if (null != (pathConstant = roleProjection.ProjectedFromConstant))
											{
												pathConstant.Delete();
											}
											sourceCalculatedValueLink.Delete();
										}
										else if (resolvedLeadRolePath == null)
										{
											resolvedLeadRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source);
											if (resolvedLeadRolePath == null)
											{
												noLeadRolePath = true;
												repeatLoop = true;
												break;
											}
										}
										else if (resolvedLeadRolePath != ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source))
										{
											// Can't resolve the binding to a single path, treat the same as no lead role path
											resolvedLeadRolePath = null;
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (null != (sourceConstantLink = RoleDerivesFromPathConstant_Deprecated.GetLinkToDerivedFromConstant(role)))
									{
										if (noLeadRolePath)
										{
											sourceConstantLink.Source.Delete();
										}
										else if (singleLeadRolePath != null)
										{
											DerivedRoleProjection roleProjection = EnsureRoleProjection(ref derivationProjection, factTypeDerivation, singleLeadRolePath, role, notifyAdded);
											pathConstant = sourceConstantLink.Source;
											sourceConstantLink.Delete(); // Introducing a second aggregate, make sure we only have one live at a time.
											roleProjection.ProjectedFromConstant = null;
											DerivedRoleProjectedFromPathConstant constantLink = new DerivedRoleProjectedFromPathConstant(roleProjection, pathConstant);
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(constantLink);
											}
											// Make sure these are clear, rules are off.
											roleProjection.ProjectedFromPathedRole = null;
											roleProjection.ProjectedFromPathRoot = null;
											roleProjection.ProjectedFromCalculatedValue = null;
										}
										// Constants can't be used to determine a role path
									}
								}
							}
							if (!repeatLoop && !noLeadRolePath && singleLeadRolePath == null && resolvedLeadRolePath != null)
							{
								singleLeadRolePath = resolvedLeadRolePath;
								repeatLoop = true;
							}
						}
					}
					else if (null != (joinPath = element as ConstraintRoleSequenceJoinPath))
					{
						if (!checkedSingleRolePath)
						{
							FindSingleOwnedLeadRolePath(element, out singleLeadRolePath, out noLeadRolePath);
						}
						ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(joinPath.RoleSequence);
						int constraintRoleCount = constraintRoles.Count;
						LeadRolePath resolvedLeadRolePath = null;
						ConstraintRoleSequenceJoinPathProjection sequenceProjection = null;
						bool repeatLoop = true;
						while (repeatLoop)
						{
							repeatLoop = false;
							foreach (ConstraintRoleSequenceHasRole constraintRole in constraintRoles)
							{
								ConstraintRoleProjectedFromPathedRole_Deprecated sourcePathedRoleLink;
								ConstraintRoleProjectedFromCalculatedPathValue_Deprecated sourceCalculatedValueLink;
								PathConstant pathConstant;
								ConstraintRoleProjectedFromPathConstant_Deprecated sourceConstantLink;
								if (null != (sourcePathedRoleLink = ConstraintRoleProjectedFromPathedRole_Deprecated.GetLinkToProjectedFromPathedRole(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourcePathedRoleLink.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										constraintRoleProjection.ProjectedFromPathedRole = null;
										notifyAdded.ElementAdded(new ConstraintRoleProjectedFromPathedRole(constraintRoleProjection, sourcePathedRoleLink.Source));
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
										constraintRoleProjection.ProjectedFromPathRoot = null;
										if (null != (pathConstant = constraintRoleProjection.ProjectedFromConstant))
										{
											pathConstant.Delete();
										}
										sourcePathedRoleLink.Delete();
									}
									else if (resolvedLeadRolePath == null)
									{
										resolvedLeadRolePath = sourcePathedRoleLink.Source.RolePath.RootRolePath;
										if (resolvedLeadRolePath == null)
										{
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (resolvedLeadRolePath != sourcePathedRoleLink.Source.RolePath.RootRolePath)
									{
										// Can't resolve the binding to a single path, treat the same as no lead role path
										resolvedLeadRolePath = null;
										noLeadRolePath = true;
										repeatLoop = true;
										break;
									}
								}
								else if (null != (sourceCalculatedValueLink = ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.GetLinkToProjectedFromCalculatedValue(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourceCalculatedValueLink.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
										notifyAdded.ElementAdded(new ConstraintRoleProjectedFromCalculatedPathValue(constraintRoleProjection, sourceCalculatedValueLink.Source));
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromPathedRole = null;
										constraintRoleProjection.ProjectedFromPathRoot = null;
										if (null != (pathConstant = constraintRoleProjection.ProjectedFromConstant))
										{
											pathConstant.Delete();
										}
										sourceCalculatedValueLink.Delete();
									}
									else if (resolvedLeadRolePath == null)
									{
										resolvedLeadRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source);
										if (resolvedLeadRolePath == null)
										{
											noLeadRolePath = true;
											repeatLoop = true;
											break;
										}
									}
									else if (resolvedLeadRolePath != ResolveCalculatedValueRolePath(sourceCalculatedValueLink.Source))
									{
										// Can't resolve the binding to a single path, treat the same as no lead role path
										resolvedLeadRolePath = null;
										noLeadRolePath = true;
										repeatLoop = true;
										break;
									}
								}
								else if (null != (sourceConstantLink = ConstraintRoleProjectedFromPathConstant_Deprecated.GetLinkToProjectedFromConstant(constraintRole)))
								{
									if (noLeadRolePath)
									{
										sourceConstantLink.Source.Delete();
									}
									else if (singleLeadRolePath != null)
									{
										ConstraintRoleProjection constraintRoleProjection = EnsureConstraintRoleProjection(ref sequenceProjection, joinPath, singleLeadRolePath, constraintRole, notifyAdded);
										pathConstant = sourceConstantLink.Source;
										sourceConstantLink.Delete(); // Introducing a second aggregate, make sure we only have one live at a time.
										constraintRoleProjection.ProjectedFromConstant = null;
										notifyAdded.ElementAdded(new ConstraintRoleProjectedFromPathConstant(constraintRoleProjection, pathConstant));
										// Make sure these are clear, rules are off.
										constraintRoleProjection.ProjectedFromPathedRole = null;
										constraintRoleProjection.ProjectedFromPathRoot = null;
										constraintRoleProjection.ProjectedFromCalculatedValue = null;
									}
									// Constants can't be used to determine a role path
								}
							}
							if (!repeatLoop && !noLeadRolePath && singleLeadRolePath == null && resolvedLeadRolePath != null)
							{
								singleLeadRolePath = resolvedLeadRolePath;
								repeatLoop = true;
							}
						}
					}

					foreach (LeadRolePath leadRolePath in element.OwnedLeadRolePathCollection)
					{
						// Replace deprecated StartRole pathed role purpose with PostInnerJoin pathed roles
						// and modify projections.
						RolePathNode.VisitPathNodes(
							leadRolePath,
							RolePathNode.Empty,
							false,
							delegate(RolePathNode currentPathNode, RolePathNode previousPathNode, bool unwinding)
							{
								PathedRole pathedRole;
								if (null != (pathedRole = currentPathNode.PathedRole))
								{
									// Deprecate direct PathedRole correlation in favor of PathObjectUnifier
									PathedRole correlatingParentPathedRole = PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.GetCorrelatingParent(pathedRole);
									if (correlatingParentPathedRole != null)
									{
										UpgradeRemoteCorrelationToObjectUnifier(leadRolePath, correlatingParentPathedRole, notifyAdded);
									}
									
									// Deprecate the StartRole purpose, move projections to the path root
									int obsoletePurpose = (int)pathedRole.PathedRolePurpose;
									if (0 > obsoletePurpose)
									{
										// Encoding for obsolete values is based on bitwise inversion, move to new
										// values without 'obsolete' warnings in a case statement.
										pathedRole.PathedRolePurpose = (PathedRolePurpose)(~obsoletePurpose);

										// If this path role is used in a fact type role projection or join path
										// projection then the projection needs to be moved to the context node.
										if (previousPathNode.IsEmpty)
										{
											// Error condition, but needs to be checked. Note that empty
											// projections are removed in later validators.
											if (factTypeDerivation != null)
											{
												DerivedRoleProjectedFromPathedRole.GetDerivedRoleProjections(pathedRole).Clear();
											}
											else if (joinPath != null)
											{
												ConstraintRoleProjectedFromPathedRole.GetConstraintRoleProjections(pathedRole).Clear();
											}
										}
										else if (null != factTypeDerivation)
										{
											PathedRole previousPathedRole = previousPathNode.PathedRole;
											RolePathObjectTypeRoot previousPathRoot = previousPathNode.PathRoot;
											LinkedElementCollection<DerivedRoleProjection> roleProjections = DerivedRoleProjectedFromPathedRole.GetDerivedRoleProjections(pathedRole);
											for (int i = roleProjections.Count - 1; i >= 0; --i) // Walk backwards to allow deletion from set
											{
												DerivedRoleProjection roleProjection = roleProjections[i];
												roleProjection.ProjectedFromPathedRole = previousPathedRole; // Removes roleProjection from iteration set if previousPathedRole is null 
												if (previousPathRoot != null)
												{
													notifyAdded.ElementAdded(new DerivedRoleProjectedFromRolePathRoot(roleProjection, previousPathRoot));
												}
											}
										}
										else if (null != joinPath)
										{
											PathedRole previousPathedRole = previousPathNode.PathedRole;
											RolePathObjectTypeRoot previousPathRoot = previousPathNode.PathRoot;
											LinkedElementCollection<ConstraintRoleProjection> constraintRoleProjections = ConstraintRoleProjectedFromPathedRole.GetConstraintRoleProjections(pathedRole);
											for (int i = constraintRoleProjections.Count - 1; i >= 0; --i) // Walk backwards to allow deletion from set
											{
												ConstraintRoleProjection constraintRoleProjection = constraintRoleProjections[i];
												constraintRoleProjection.ProjectedFromPathedRole = previousPathedRole; // Removes constraintRoleProjection from iteration set if previousPathedRole is null
												if (previousPathRoot != null)
												{
													notifyAdded.ElementAdded(new ConstraintRoleProjectedFromRolePathRoot(constraintRoleProjection, previousPathRoot));
												}
											}
										}
									}
								}
								return true; // Continue iteration
							});

						// Replace scope with aggregation context
						foreach (CalculatedPathValue calculation in leadRolePath.CalculatedValueCollection)
						{
							CalculatedPathValueScopedWithPathedRole_Deprecated scopeLink = CalculatedPathValueScopedWithPathedRole_Deprecated.GetLinkToScope(calculation);
							Function function = calculation.Function;
							if (scopeLink != null)
							{
								if (null == function || !function.IsAggregate)
								{
									scopeLink.Delete();
								}
								else
								{
									notifyAdded.ElementAdded(new CalculatedPathValueAggregationContextIncludesPathedRole(calculation, scopeLink.Scope));
									scopeLink.Delete();
								}
							}
							else if (null != function &&
								function.IsAggregate &&
								!calculation.UniversalAggregationContext &&
								calculation.AggregationContextRequiredError == null &&
								calculation.AggregationContextPathedRoleCollection.Count == 0 &&
								calculation.AggregationContextPathRootCollection.Count == 0)
							{
								// Switch the old default of no scope meaning a path root context
								// to an explicit root context if one can be determined, or a universal
								// context if aggregating over a path root. This is not a full implementation
								// because it does not consider the aggregation context of nested aggregate
								// calculations, but is a reasonable conversion for most existing cases.
								// UNDONE: This is a very expensive load check, consider pulling it out in the near future.
								foreach (CalculatedPathValueInput input in calculation.InputCollection)
								{
									FunctionParameter parameter;
									if (null != (parameter = input.Parameter) &&
										parameter.BagInput)
									{
										RolePathObjectTypeRoot bagPathRoot;
										if (null != (bagPathRoot = input.SourcePathRoot) &&
											bagPathRoot.PreviousPathNode.IsEmpty)
										{
											calculation.UniversalAggregationContext = true;
										}
										else
										{
											PathedRole bagPathedRole;
											CalculatedPathValue bagCalculation;
											IEnumerable<RolePathNode> satisfyPathNodes = null;
											if (null != (bagPathedRole = input.SourcePathedRole))
											{
												satisfyPathNodes = new RolePathNode[] { bagPathedRole };
											}
											else if (null != (bagCalculation = input.SourceCalculatedValue))
											{
												satisfyPathNodes = ResolveCalculationPathNodes(bagCalculation);
											}
											if (satisfyPathNodes != null)
											{
												RolePathObjectTypeRoot commonRoot = null;
												foreach (RolePathNode nestedNode in satisfyPathNodes)
												{
													bool seenBagNode = false;
													RolePathObjectTypeRoot resolvedRoot = null;
													RolePathNode.VisitPathNodes(
														leadRolePath,
														RolePathNode.Empty,
														true,
														delegate(RolePathNode currentPathNode, RolePathNode previousPathNode, bool unwinding)
														{
															if (unwinding)
															{
																if (seenBagNode)
																{
																	RolePathObjectTypeRoot pathRoot;
																	if (null != (pathRoot = currentPathNode))
																	{
																		resolvedRoot = pathRoot;
																		return false; // Stop iteration
																	}
																}
																else if (currentPathNode == nestedNode)
																{
																	seenBagNode = true;
																}
															}
															return true; // Continue iteration
														});
													if (resolvedRoot == null)
													{
														commonRoot = null;
														break;
													}
													else if (commonRoot == null)
													{
														commonRoot = resolvedRoot;
													}
													else if (commonRoot != resolvedRoot)
													{
														commonRoot = null;
														break;
													}
												}
												if (commonRoot != null)
												{
													notifyAdded.ElementAdded(new CalculatedPathValueAggregationContextIncludesRolePathRoot(calculation, commonRoot));
												}
											}
										}
										break;
									}
								}
							}
						}

						// Eliminate intra-fact type splits. The basic approach is to add new pathed role declarations
						// immediately after the entry role for the fact type, replace the ift split roles
						// with new path roots, and move all projections, calculations, and value ranges to the
						// new path root node. While most IFT roles will be single, it is also possible to have
						// multiple roles at the start of the path. In this case, we need to create a new subpath
						// for each additional role.
						ReadOnlyCollection<PathedRole> entryPathedRoles = null;
						RemoveIntraFactTypeSplits(leadRolePath, ref entryPathedRoles, -1, leadRolePath.SplitCombinationOperator == LogicalCombinationOperator.And && !leadRolePath.SplitIsNegated, notifyAdded);
					}
				}
			}
			/// <summary>
			/// Recursive helper function for ProcessElement
			/// </summary>
			/// <param name="rolePath">The role path to check</param>
			/// <param name="entryPathedRoles">The pathed roles for the path that contains the last
			/// entry role into a fact type use. Passed by reference so the readonly collection can
			/// be updated when it changes.</param>
			/// <param name="entryRoleIndex">The index of the entry role in <paramref name="entryPathedRoles"/></param>
			/// <param name="inPositiveConjunction">True if all of the splits passed since the entry pathed role
			/// are 'and' splits and not negated.</param>
			/// <param name="notifyAdded">Add notification callback.</param>
			private static void RemoveIntraFactTypeSplits(RolePath rolePath, ref ReadOnlyCollection<PathedRole> entryPathedRoles, int entryRoleIndex, bool inPositiveConjunction, INotifyElementAdded notifyAdded)
			{
				LinkedElementCollection<RoleSubPath> subpaths = rolePath.SubPathCollection;
				int subpathCount = subpaths.Count;
				ReadOnlyCollection<PathedRole> currentPathedRoles = null;
				if (subpathCount == 0 &&
					entryRoleIndex == -1)
				{
					// There is no bound ift role at the beginning without a context,
					// and there are no trailing ones without subpaths.
					return;
				}
				bool hasRoot = rolePath.PathRoot != null;
				bool scrubbedSplit;
				if (scrubbedSplit = (entryRoleIndex != -1 &&
					!hasRoot &&
					(currentPathedRoles = rolePath.PathedRoleCollection).Count != 0 &&
					currentPathedRoles[0].PathedRolePurpose == PathedRolePurpose.SameFactType))
				{
					// Clear the ift split in this branch. Do the work in a non-recursive routine
					// to keep the stack reasonable for this function.
					ScrubIntraFactTypeSplit(rolePath, currentPathedRoles, ref entryPathedRoles, entryRoleIndex, inPositiveConjunction, notifyAdded);
				}

				if (subpathCount != 0)
				{
					// Find an entry pathed role in the current roles. If there is none, then we may have
					// a nested ift split in a following branch, so we leave the entry role the same as with
					// this call.
					// Note that the count can change with the ift cleanup, don't cache
					int i = (currentPathedRoles ?? (currentPathedRoles = rolePath.PathedRoleCollection)).Count - 1;
					for (; i >= 0; --i)
					{
						if (currentPathedRoles[i].PathedRolePurpose != PathedRolePurpose.SameFactType)
						{
							entryPathedRoles = currentPathedRoles;
							entryRoleIndex = i;
							break;
						}
					}
					if (i == -1 && hasRoot)
					{
						// The root is reset, so there is no connection to a previous entry role
						entryPathedRoles = null;
						entryRoleIndex = -1;
					}
					RoleSubPath[] subpathSnap = subpaths.ToArray();
					RoleSubPath subpath;
					for (i = 0; i < subpathCount; ++i)
					{
						subpath = subpathSnap[i];
						RemoveIntraFactTypeSplits(
							subpath,
							ref entryPathedRoles,
							entryRoleIndex,
							entryPathedRoles == null || (inPositiveConjunction && subpath.SplitCombinationOperator == LogicalCombinationOperator.And && !subpath.SplitIsNegated),
							notifyAdded);
					}
					if (subpaths.Count == 1 &&
						(subpath = subpaths[0]).PathRoot == null)
					{
						// We've eliminated one or more subpaths and no longer need the single path.
						// Move pathed roles and subpaths up one level.
						foreach (PathedRole nestedPathedRole in subpath.PathedRoleCollection)
						{
							nestedPathedRole.RolePath = rolePath;
						}
						foreach (RoleSubPathIsContinuationOfRolePath nestedSubPathLink in RoleSubPathIsContinuationOfRolePath.GetLinksToSubPathCollection(rolePath))
						{
							nestedSubPathLink.ParentRolePath = rolePath;
						}
						subpath.Delete();
					}
				}
				else if (scrubbedSplit && rolePath.RoleCollection.Count == 0)
				{
					// This branch is empty, we don't need it anymore
					rolePath.Delete();
				}
			}
			private static void ScrubIntraFactTypeSplit(RolePath rolePath, ReadOnlyCollection<PathedRole> currentPathedRoles, ref ReadOnlyCollection<PathedRole> entryPathedRoles, int entryRoleIndex, bool inPositiveConjunction, INotifyElementAdded notifyAdded)
			{
				// Find the deepest role to scrub and work backwards
				int pathedRoleCount = currentPathedRoles.Count;

				// We already know 0 is an ift role and there is no path root, or we wouldn't be here.

				// The initial ift role can be transformed directly into the path root of this path,
				// but any subsequent roles need to split off their own path. Do this part first.
				PathedRole replacePathedRole;
				PathedRole correlatedRole;
				RolePathObjectTypeRoot replacementPathRoot;
				bool injectedRoleAtEnd;
				for (int i = 1; i < pathedRoleCount; ++i)
				{
					if (currentPathedRoles[i].PathedRolePurpose != PathedRolePurpose.SameFactType)
					{
						// Work backwards. Injecting these roles into the entry fact type will
						// at least give some semblance of order.
						RoleSubPath lastSubpath = null;
						Partition partition = rolePath.Partition;
						for (int j = i - 1; j >= 1; --j)
						{
							replacePathedRole = currentPathedRoles[j];
							correlatedRole = InjectPathedRole(replacePathedRole, ref entryPathedRoles, entryRoleIndex, false, notifyAdded, out injectedRoleAtEnd);
							RoleSubPath subpath;
							notifyAdded.ElementAdded(subpath = new RoleSubPath(partition));
							if (null != (replacementPathRoot = CreateCorrelatedRoot(subpath, correlatedRole, notifyAdded)))
							{
								TransferAdherents(replacePathedRole, replacementPathRoot, null, notifyAdded);
							}
							replacePathedRole.Delete();
							if (lastSubpath == null)
							{
								// Move all trailing pathed roles down to the new subpath. Note that
								// we've deleted j, but the indices don't move because we're analyzing
								// a read-only collection.
								for (int k = j + 1; j < pathedRoleCount; ++k)
								{
									PathedRole reparentPathedRole = currentPathedRoles[k];
									reparentPathedRole.RolePath = subpath;
								}
								foreach (RoleSubPathIsContinuationOfRolePath subPathLink in RoleSubPathIsContinuationOfRolePath.GetLinksToSubPathCollection(rolePath))
								{
									subPathLink.ParentRolePath = subpath;
								}
							}
							else
							{
								notifyAdded.ElementAdded(new RoleSubPathIsContinuationOfRolePath(subpath, lastSubpath), false);
							}
							lastSubpath = subpath;
						}
						if (lastSubpath != null)
						{
							notifyAdded.ElementAdded(new RoleSubPathIsContinuationOfRolePath(rolePath, lastSubpath));
						}
						break;
					}
				}

				// Handle the lead pathed role
				replacePathedRole = currentPathedRoles[0];
				correlatedRole = InjectPathedRole(replacePathedRole, ref entryPathedRoles, entryRoleIndex, inPositiveConjunction, notifyAdded, out injectedRoleAtEnd);
				if (correlatedRole == replacePathedRole)
				{
					// The role pathed role was moved to the end of the entry fact type
					// and can be joined to directly to any following elements. Anything
					// left in this subpath can be moved to the parent path (this only
					// happens under positive conjunction, so the split settings no longer
					// need to be checked or moved).
					RoleSubPath subpath;
					if (null != (subpath = rolePath as RoleSubPath))
					{
						RolePath parentPath = subpath.ParentRolePath;
						bool movedPathedRole = false;
						foreach (PathedRole remainingPathedRole in PathedRole.GetLinksToRoleCollection(rolePath))
						{
							remainingPathedRole.RolePath = parentPath;
							movedPathedRole = true;
						}
						if (movedPathedRole)
						{
							entryPathedRoles = entryPathedRoles[0].RolePath.PathedRoleCollection;
						}
						ReadOnlyCollection<RoleSubPathIsContinuationOfRolePath> nestedSubpathLinks = RoleSubPathIsContinuationOfRolePath.GetLinksToSubPathCollection(rolePath);
						int nestedSubpathCount = nestedSubpathLinks.Count;
						if (nestedSubpathCount != 0)
						{
							// Find a context entry role in the new extended pathed roles
							ReadOnlyCollection<PathedRole> newPathedRoles = parentPath.PathedRoleCollection;
							int newEntryRoleIndex = -1;
							for (int i = newPathedRoles.Count - 1; i >= 0; --i)
							{
								if (newPathedRoles[i].PathedRolePurpose != PathedRolePurpose.SameFactType)
								{
									newEntryRoleIndex = i;
									break;
								}
							}
							if (newEntryRoleIndex == -1)
							{
								newPathedRoles = null;
							}

							// Move all of the nested subpaths to this location
							LinkedElementCollection<RoleSubPath> parentSubpaths = parentPath.SubPathCollection;
							int moveTo = parentSubpaths.IndexOf(subpath) + 1;
							bool atEnd = moveTo == parentSubpaths.Count;
							DomainRoleInfo roleInfo = atEnd ? null : parentPath.Store.DomainDataDirectory.FindDomainRole(RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId);
							for (int i = 0; i < nestedSubpathCount; ++i)
							{
								RoleSubPathIsContinuationOfRolePath nestedSubpathLink = nestedSubpathLinks[i];
								nestedSubpathLink.ParentRolePath = parentPath;
								if (!atEnd)
								{
									nestedSubpathLink.MoveToIndex(roleInfo, moveTo);
									++moveTo;
								}
							}

							// Delete the current subpath before recursing
							subpath.Delete();

							// With these subpaths now attached to the parent, we can do their
							// ift processing. These won't be picked up by the calling code, which snapshots
							// the roles before we get this far.
							for (int i = 0; i < nestedSubpathCount; ++i)
							{
								RemoveIntraFactTypeSplits(nestedSubpathLinks[i].SubPath, ref newPathedRoles, newEntryRoleIndex, rolePath.SplitCombinationOperator == LogicalCombinationOperator.And && !rolePath.SplitIsNegated, notifyAdded);
							}
						}
						else
						{
							// This subpath has been fully absorbed into the parent path, delete it.
							subpath.Delete();
						}
					}
				}
				else
				{
					// There is nothing needed. The injected role was placed the end of the list, which
					// means that the natural join is for the next role.
					if (injectedRoleAtEnd)
					{
						replacementPathRoot = null;
						TransferAdherents(
							replacePathedRole,
							null,
							delegate()
							{
								return replacementPathRoot ?? (replacementPathRoot = CreateCorrelatedRoot(rolePath, correlatedRole, notifyAdded));
							},
							notifyAdded);
					}
					else
					{
						if (null != (replacementPathRoot = CreateCorrelatedRoot(rolePath, correlatedRole, notifyAdded)))
						{
							TransferAdherents(replacePathedRole, replacementPathRoot, null, notifyAdded);
						}
					}
					replacePathedRole.Delete();
				}
			}
			private delegate RolePathObjectTypeRoot GetPathRoot();
			/// <summary>
			/// Helper for RemoveIntraFactTypeSplits. Move projections, calculations, and value
			/// range information from a pending removed role to a path root.
			/// </summary>
			private static void TransferAdherents(PathedRole fromRole, RolePathObjectTypeRoot toRoot, GetPathRoot delayCreateRoot, INotifyElementAdded notifyAdded)
			{
				// Move value constraint ranges
				Store store = fromRole.Store;
				PathConditionRoleValueConstraint roleValueConstraint;
				if (null != (roleValueConstraint = fromRole.ValueConstraint))
				{
					PathConditionRootValueConstraint rootConstraint;
					notifyAdded.ElementAdded(rootConstraint = new PathConditionRootValueConstraint(roleValueConstraint.Partition));
					foreach (ValueConstraintHasValueRange rangeLink in ValueConstraintHasValueRange.GetLinksToValueRangeCollection(roleValueConstraint))
					{
						rangeLink.ValueConstraint = rootConstraint;
					}
					if (null == (toRoot ?? (toRoot = delayCreateRoot())))
					{
						return;
					}
					notifyAdded.ElementAdded(new RolePathRootHasValueConstraint(toRoot, rootConstraint));
					roleValueConstraint.Delete();
				}

				// Note that correlations have already been synchronized during InjectPathedRole and CreateCorrelatedRoot,
				// there is no need to check them here.

				// Check remaining known link types. Note that this could be optimized with
				// a lookup table by link type, but speed is not paramount while eliminating
				// deprecated elements.
				CalculatedPathValueInputBindsToPathedRole calculationInputLink;
				DerivedRoleProjectedFromPathedRole roleProjectionLink;
				ConstraintRoleProjectedFromPathedRole constraintRoleProjectionLink;
				CalculatedPathValueAggregationContextIncludesPathedRole aggContextLink;
				QueryParameterBoundToPathedRole queryParameterBindingLink;
				SubqueryParameterInputFromPathedRole subqueryParameterInputLink;
				foreach (ElementLink link in DomainRoleInfo.GetAllElementLinks(fromRole))
				{
					if (null != (calculationInputLink = link as CalculatedPathValueInputBindsToPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new CalculatedPathValueInputBindsToRolePathRoot(calculationInputLink.Input, toRoot));
					}
					else if (null != (roleProjectionLink = link as DerivedRoleProjectedFromPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new DerivedRoleProjectedFromRolePathRoot(roleProjectionLink.RoleProjection, toRoot));
					}
					else if (null != (constraintRoleProjectionLink = link as ConstraintRoleProjectedFromPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new ConstraintRoleProjectedFromRolePathRoot(constraintRoleProjectionLink.ConstraintRoleProjection, toRoot));
					}
					else if (null != (aggContextLink = link as CalculatedPathValueAggregationContextIncludesPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new CalculatedPathValueAggregationContextIncludesRolePathRoot(aggContextLink.CalculatedValue, toRoot));
					}
					else if (null != (queryParameterBindingLink = link as QueryParameterBoundToPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new QueryParameterBoundToRolePathRoot(queryParameterBindingLink.ParameterBinding, toRoot));
					}
					else if (null != (subqueryParameterInputLink = link as SubqueryParameterInputFromPathedRole))
					{
						if (null == (toRoot ?? (toRoot = delayCreateRoot())))
						{
							return;
						}
						notifyAdded.ElementAdded(new SubqueryParameterInputFromRolePathRoot(subqueryParameterInputLink.ParameterInput, toRoot));
					}
					else
					{
						continue;
					}
					link.Delete();
				}
			}
			private static PathedRole InjectPathedRole(PathedRole replacePathedRole, ref ReadOnlyCollection<PathedRole> entryPathedRoles, int entryRoleIndex, bool canStealReplaceRole, INotifyElementAdded notifyAdded, out bool injectedRoleAtEnd)
			{
				// Inject a role immediately after the entry role. Theoretically each role during
				// an ift was used one time only, but this was weakly enforced and there was no
				// underlying theoretical reason for it, so we verify this condition.
				// The harder case here is if there is no trailing same fact type role,
				// which means that the following joined roles will now need a new correlation
				// root to clean this up.
				Role role = replacePathedRole.Role;

				PathedRole factTypeEntry = entryPathedRoles[entryRoleIndex];
				int roleCount = entryPathedRoles.Count;
				if (factTypeEntry.Role == role)
				{
					// Unlikely, but sufficient
					SynchronizePathedRoleUnifiers(factTypeEntry, replacePathedRole);
					injectedRoleAtEnd = entryRoleIndex == (roleCount - 1);
					return factTypeEntry;
				}
				int i = entryRoleIndex + 1;
				for (; i < roleCount; ++i)
				{
					PathedRole testPathedRole = entryPathedRoles[i];
					if (testPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
					{
						break;
					}
					else if (testPathedRole.Role == role)
					{
						SynchronizePathedRoleUnifiers(testPathedRole, replacePathedRole);
						injectedRoleAtEnd = i == (roleCount - 1);
						return testPathedRole;
					}
				}
				RolePath rolePath = factTypeEntry.RolePath;
				if (i == (entryRoleIndex + 1))
				{
					// There are no trailing same fact type roles, so we need to make sure that
					// following subpaths get a correlated root to the entry role.
					// Note that anything that already has a path root can be safely ignored.
					// All of these variables are in the same fact use, so it doesn't matter
					// which of the variables we coexist with. Only worry about the subpaths
					// with no root and a lead join role. The path to modify may be nested
					// because subpaths can exist for splits only.
					ReanchorSubpaths(rolePath.SubPathCollection, factTypeEntry, notifyAdded);
				}
				// Create and return a new same fact type pathed role at this location.
				PathedRole newPathedRole;
				if (canStealReplaceRole &&
					(entryRoleIndex + 1) == roleCount)
				{
					newPathedRole = replacePathedRole;
					newPathedRole.RolePath = rolePath;
				}
				else
				{
					notifyAdded.ElementAdded(newPathedRole = new PathedRole(rolePath, role), false);
					if ((entryRoleIndex + 1) != roleCount)
					{
						newPathedRole.MoveToIndex(newPathedRole.Store.DomainDataDirectory.FindDomainRole(PathedRole.RolePathDomainRoleId), entryRoleIndex + 1);
					}

				}
				// The readonly collection has changed, update it for future reference
				entryPathedRoles = rolePath.PathedRoleCollection;
				SynchronizePathedRoleUnifiers(newPathedRole, replacePathedRole);
				injectedRoleAtEnd = entryRoleIndex == (roleCount - 1);
				return newPathedRole;
			}
			/// <summary>
			/// Synchronize correlation settings prior to deleting a role.
			/// Helper for RemoveIntraFactTypeSplits.
			/// </summary>
			private static void SynchronizePathedRoleUnifiers(PathedRole pathedRole, PathedRole replacePathedRole)
			{
				PathObjectUnifier unifier;
				if (pathedRole != replacePathedRole && // Sanity check
					null != (unifier = replacePathedRole.ObjectUnifier))
				{
					PathObjectUnifier otherUnifier;
					if (null != (otherUnifier = pathedRole.ObjectUnifier))
					{
						foreach (PathObjectUnifierUnifiesPathedRole pathedRoleLink in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(unifier))
						{
							if (pathedRoleLink.PathedRole != replacePathedRole)
							{
								pathedRoleLink.ObjectUnifier = otherUnifier;
							}
						}
						foreach (PathObjectUnifierUnifiesRolePathRoot pathRootLink in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(unifier))
						{
							pathRootLink.ObjectUnifier = otherUnifier;
						}
						unifier.Delete();
					}
					else
					{
						replacePathedRole.ObjectUnifier = null;
						pathedRole.ObjectUnifier = unifier;
					}
				}
			}
			/// <summary>
			/// Helper for RemoveIntraFactTypeSplits. For a following subpath that begins with an entry role and
			/// has no path root, set the path root and correlated with the provided pathed role.
			/// </summary>
			private static void ReanchorSubpaths(LinkedElementCollection<RoleSubPath> subpaths, PathedRole unifyWithPathedRole, INotifyElementAdded notifyAdded)
			{
				foreach (RoleSubPath subpath in subpaths)
				{
					if (subpath.PathRoot == null)
					{
						ReadOnlyCollection<PathedRole> pathedRoles = subpath.PathedRoleCollection;
						if (pathedRoles.Count == 0)
						{
							// This path splits before it does anything, recurse.
							ReanchorSubpaths(subpath.SubPathCollection, unifyWithPathedRole, notifyAdded);
						}
						else if (pathedRoles[0].PathedRolePurpose != PathedRolePurpose.SameFactType)
						{
							CreateCorrelatedRoot(subpath, unifyWithPathedRole, notifyAdded);
						}
					}
				}
			}
			/// <summary>
			/// Helper for RemoveIntraFactTypeSplits. Correlates a new path root with an existing
			/// pathed role. The pathed role is guaranteed to be in the path (so we can get
			/// back to the root to create unifiers), but the provided subpath may not be rooted yet.
			/// Note that this will return null if the role does not have a role player.
			/// </summary>
			private static RolePathObjectTypeRoot CreateCorrelatedRoot(RolePath rolePath, PathedRole unifyWithPathedRole, INotifyElementAdded notifyAdded)
			{
				ObjectType rootType = unifyWithPathedRole.Role.RolePlayer;
				if (rootType == null)
				{
					// This will result in correlation errors in the path, but this case is rare
					// and the only way to avoid the issue is to stick with the intra-fact type splits,
					// which do not require role players.
					return null;
				}
				RolePathObjectTypeRoot pathRoot;
				PathObjectUnifier unifier;
				notifyAdded.ElementAdded(pathRoot = new RolePathObjectTypeRoot(rolePath, unifyWithPathedRole.Role.RolePlayer), false);
				if (null == (unifier = unifyWithPathedRole.ObjectUnifier))
				{
					notifyAdded.ElementAdded(unifier = new PathObjectUnifier(rolePath.Partition), false);
					notifyAdded.ElementAdded(new LeadRolePathHasObjectUnifier(unifyWithPathedRole.RolePath.RootRolePath, unifier));
					notifyAdded.ElementAdded(new PathObjectUnifierUnifiesPathedRole(unifier, unifyWithPathedRole));
				}
				notifyAdded.ElementAdded(new PathObjectUnifierUnifiesRolePathRoot(unifier, pathRoot));
				return pathRoot;
			}
			/// <summary>
			/// Upgrade a pathed role correlation hierarchy to the replacement <see cref="PathObjectUnifier"/> element.
			/// </summary>
			private static void UpgradeRemoteCorrelationToObjectUnifier(LeadRolePath leadRolePath, PathedRole correlationParent, INotifyElementAdded notifyAdded)
			{
				PathedRole topParent = correlationParent;
				while (null != (correlationParent = PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.GetCorrelatingParent(correlationParent)))
				{
					topParent = correlationParent;
				}
				RolePathNode firstNode = RolePathNode.Empty; // Delay creation until we have successfully found two distinct elements.
				PathObjectUnifier objectUnifier = null;
				RecurseAddUnifiedElements(leadRolePath, topParent, notifyAdded, ref firstNode, ref objectUnifier);
			}
			/// <summary>
			/// Helper methods for <see cref="UpgradeRemoteCorrelationToObjectUnifier"/>
			/// </summary>
			private static void RecurseAddUnifiedElements(LeadRolePath leadRolePath, PathedRole correlationParent, INotifyElementAdded notifyAdded, ref RolePathNode firstPathNode, ref PathObjectUnifier objectUnifier)
			{
				// We haven't fully removed start roles at this point, so we need
				// to watch out for both start and entry roles and normalize them
				// up by looking up the parent path.
				RolePathNode resolvedNode = correlationParent;
				PathedRole resolvedPathedRole = correlationParent;
				while (resolvedPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
				{
					resolvedNode = correlationParent.PreviousPathNode;
					resolvedPathedRole = resolvedNode;
					if (resolvedPathedRole == null)
					{
						break;
					}
				}
				if (!resolvedNode.IsEmpty)
				{
					// In the unlikely case that there is another existing unifier, then adopt or merge
					// the unified elements.
					resolvedPathedRole = resolvedNode;
					RolePathObjectTypeRoot resolvedRootLink = null;
					PathObjectUnifier existingUnifier;
					if (resolvedPathedRole == null)
					{
						resolvedRootLink = resolvedNode.PathRoot;
						existingUnifier = resolvedRootLink.ObjectUnifier;
					}
					else
					{
						existingUnifier = resolvedPathedRole.ObjectUnifier;
					}
					if (existingUnifier != null)
					{
						if (objectUnifier == null)
						{
							objectUnifier = existingUnifier;
							if (!firstPathNode.IsEmpty)
							{
								PathedRole firstPathedRole = firstPathNode;
								if (firstPathedRole != null)
								{
									notifyAdded.ElementAdded(new PathObjectUnifierUnifiesPathedRole(objectUnifier, firstPathedRole));
								}
								else
								{
									notifyAdded.ElementAdded(new PathObjectUnifierUnifiesRolePathRoot(objectUnifier, firstPathNode));
								}
								firstPathNode = RolePathNode.Empty;
							}
						}
						else if (objectUnifier != existingUnifier)
						{
							// Move existing relationships to the existing unifier (note that collections are read only and can be modified)
							foreach (PathObjectUnifierUnifiesRolePathRoot pathRootUnifier in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(objectUnifier))
							{
								pathRootUnifier.ObjectUnifier = existingUnifier;
							}
							foreach (PathObjectUnifierUnifiesPathedRole pathedRoleUnifier in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(objectUnifier))
							{
								pathedRoleUnifier.ObjectUnifier = existingUnifier;
							}
							objectUnifier.Delete();
							objectUnifier = existingUnifier;
							// Note that firstPathNode is empty if objectUnifier is set.
						}
					}
					else if (objectUnifier != null)
					{
						if (resolvedPathedRole != null)
						{
							notifyAdded.ElementAdded(new PathObjectUnifierUnifiesPathedRole(objectUnifier, resolvedPathedRole));
						}
						else
						{
							notifyAdded.ElementAdded(new PathObjectUnifierUnifiesRolePathRoot(objectUnifier, resolvedRootLink));
						}
					}
					else if (firstPathNode.IsEmpty)
					{
						firstPathNode = resolvedNode;
					}
					else
					{
						PathObjectUnifier newUnifier;
						notifyAdded.ElementAdded(newUnifier = new PathObjectUnifier(leadRolePath.Partition));
						notifyAdded.ElementAdded(new LeadRolePathHasObjectUnifier(leadRolePath, newUnifier));
						if (!firstPathNode.IsEmpty)
						{
							PathedRole firstPathedRole = firstPathNode;
							if (firstPathedRole != null)
							{
								notifyAdded.ElementAdded(new PathObjectUnifierUnifiesPathedRole(newUnifier, firstPathedRole));
							}
							else
							{
								notifyAdded.ElementAdded(new PathObjectUnifierUnifiesRolePathRoot(newUnifier, firstPathNode));
							}
							firstPathNode = RolePathNode.Empty;
						}
						if (resolvedPathedRole != null)
						{
							notifyAdded.ElementAdded(new PathObjectUnifierUnifiesPathedRole(newUnifier, resolvedPathedRole));
						}
						else
						{
							notifyAdded.ElementAdded(new PathObjectUnifierUnifiesRolePathRoot(newUnifier, resolvedRootLink));
						}
						objectUnifier = newUnifier;
					}
				}
				foreach (PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated childLink in PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.GetLinksToCorrelatedChildCollection(correlationParent))
				{
					RecurseAddUnifiedElements(leadRolePath, childLink.CorrelatedChild, notifyAdded, ref firstPathNode, ref objectUnifier);
					childLink.Delete();
				}
			}
			private static DerivedRoleProjection EnsureRoleProjection(ref RoleSetDerivationProjection derivationProjection, RoleProjectedDerivationRule derivationRule, LeadRolePath projectedFromRolePath, Role projectedOnRole, INotifyElementAdded notifyAdded)
			{
				DerivedRoleProjection roleProjection = null;
				if (null == derivationProjection &&
					null == (derivationProjection = RoleSetDerivationProjection.GetLink(derivationRule, projectedFromRolePath)))
				{
					derivationProjection = new RoleSetDerivationProjection(derivationRule, projectedFromRolePath);
					notifyAdded.ElementAdded(derivationProjection);
				}
				else
				{
					roleProjection = DerivedRoleProjection.GetLink(derivationProjection, projectedOnRole);
				}
				if (roleProjection == null)
				{
					roleProjection = new DerivedRoleProjection(derivationProjection, projectedOnRole);
					notifyAdded.ElementAdded(roleProjection);
				}
				return roleProjection;
			}
			private static ConstraintRoleProjection EnsureConstraintRoleProjection(ref ConstraintRoleSequenceJoinPathProjection joinPathProjection, ConstraintRoleSequenceJoinPath joinPath, LeadRolePath projectedFromRolePath, ConstraintRoleSequenceHasRole projectedOnConstraintRole, INotifyElementAdded notifyAdded)
			{
				ConstraintRoleProjection constraintRoleProjection = null;
				if (null == joinPathProjection &&
					null == (joinPathProjection = ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, projectedFromRolePath)))
				{
					joinPathProjection = new ConstraintRoleSequenceJoinPathProjection(joinPath, projectedFromRolePath);
					notifyAdded.ElementAdded(joinPathProjection);
				}
				else
				{
					constraintRoleProjection = ConstraintRoleProjection.GetLink(joinPathProjection, projectedOnConstraintRole);
				}
				if (constraintRoleProjection == null)
				{
					constraintRoleProjection = new ConstraintRoleProjection(joinPathProjection, projectedOnConstraintRole);
					notifyAdded.ElementAdded(constraintRoleProjection);
				}
				return constraintRoleProjection;
			}
			private static void FindSingleOwnedLeadRolePath(RolePathOwner pathOwner, out LeadRolePath singleLeadRolePath, out bool noLeadRolePath)
			{
				LinkedElementCollection<LeadRolePath> rolePaths = pathOwner.OwnedLeadRolePathCollection;
				singleLeadRolePath = null;
				noLeadRolePath = false;
				switch (rolePaths.Count)
				{
					case 0:
						// Any functions cannot be reattached
						noLeadRolePath = true;
						break;
					case 1:
						singleLeadRolePath = rolePaths[0];
						break;
				}
			}
			private static LeadRolePath ResolveCalculatedValueRolePath(CalculatedPathValue calculatedValue)
			{
				LeadRolePath retVal = null;
				foreach (CalculatedPathValueInput input in calculatedValue.InputCollection)
				{
					LeadRolePath inputRolePath = null;
					PathedRole sourcePathedRole;
					CalculatedPathValue sourceCalculatedValue;
					if (null != (sourcePathedRole = input.SourcePathedRole))
					{
						inputRolePath = sourcePathedRole.RolePath.RootRolePath;
					}
					else if (null != (sourceCalculatedValue = input.SourceCalculatedValue))
					{
						inputRolePath = ResolveCalculatedValueRolePath(sourceCalculatedValue);
					}
					// No data available from a constant.
					if (inputRolePath != null)
					{
						if (retVal == null)
						{
							retVal = inputRolePath;
						}
						else if (inputRolePath != retVal)
						{
							retVal = null; // Disagreement, can't get a reliable answer
							break;
						}
					}
				}
				return retVal;
			}
			/// <summary>
			/// Get all path nodes used by a calculation 
			/// </summary>
			private static IEnumerable<RolePathNode> ResolveCalculationPathNodes(CalculatedPathValue calculation)
			{
				foreach (CalculatedPathValueInput input in calculation.InputCollection)
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					CalculatedPathValue nestedCalculation;
					if (null != (pathedRole = input.SourcePathedRole))
					{
						yield return pathedRole;
					}
					else if (null != (pathRoot = input.SourcePathRoot))
					{
						yield return pathRoot;
					}
					else if (null != (nestedCalculation = input.SourceCalculatedValue))
					{
						foreach (RolePathNode nestedPathNode in ResolveCalculationPathNodes(nestedCalculation))
						{
							yield return nestedPathNode;
						}
					}
				}
			}
		}
		#endregion // Deprecated Element Removal
		#region Implicit Element Creation
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// populates
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new AddImplicitElementsFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Replaces deprecated role path representations.
		/// </summary>
		private sealed class AddImplicitElementsFixupListener : DeserializationFixupListener<RolePathOwner>
		{
			/// <summary>
			/// ReplaceDeprecatedElementsFixupListener constructor
			/// </summary>
			public AddImplicitElementsFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Make sure implicit elements are added
			/// </summary>
			protected sealed override void ProcessElement(RolePathOwner element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					ReadOnlyCollection<RolePathOwnerHasLeadRolePath> rolePaths = RolePathOwnerHasLeadRolePath.GetLinksToLeadRolePathCollection(element);
					if (rolePaths.Count == 1)
					{
						RolePathOwnerHasLeadRolePath link = rolePaths[0];
						LeadRolePath singlePath = link.RolePath;
						notifyAdded.ElementAdded(new RolePathOwnerHasSingleLeadRolePath(element, singlePath));
						if (link is RolePathOwnerOwnsLeadRolePath)
						{
							notifyAdded.ElementAdded(new RolePathOwnerHasSingleOwnedLeadRolePath(element, singlePath));
						}
					}
				}
			}
		}
		#endregion // Implicit Element Creation
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// Attach rule notifications for subtype hierarchy changes
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public static void EnableRuleNotifications(Store store)
		{
			ObjectType.AddSubtypeHierarchyChangeRuleNotification(store, ValidatePathCompatibility);
		}
		/// <summary>
		/// Check compatibility on subtype hierarchy changes
		/// </summary>
		private static void ValidatePathCompatibility(ObjectType type)
		{
			LinkedElementCollection<Role> playedRoles = type.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				if (!playedRole.IsDeleting)
				{
					foreach (RolePath rolePath in playedRole.RolePathCollection)
					{
						AddDelayedPathValidation(rolePath);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((PathedRole)e.ModelElement).RolePath);
		}
		/// <summary>
		/// ChangeRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid propertyId = e.DomainProperty.Id;
			if (propertyId == PathedRole.PathedRolePurposeDomainPropertyId || propertyId == PathedRole.IsNegatedDomainPropertyId)
			{
				AddDelayedPathValidation(((PathedRole)e.ModelElement).RolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((PathedRole)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathedRole)
		/// </summary>
		private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == PathedRole.RoleDomainRoleId)
			{
				AddDelayedPathValidation(((PathedRole)e.ElementLink).RolePath);
			}
			else
			{
				AddDelayedPathValidation((RolePath)e.OldRolePlayer);
				AddDelayedPathValidation((RolePath)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(LeadRolePathHasObjectUnifier)
		/// </summary>
		private static void ObjectUnifierAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((LeadRolePathHasObjectUnifier)e.ModelElement).LeadRolePath);
		}
		/// <summary>
		/// DeleteRule: typeof(LeadRolePathHasObjectUnifier)
		/// </summary>
		private static void ObjectUnifierDeletedRule(ElementDeletedEventArgs e)
		{
			LeadRolePath rolePath = ((LeadRolePathHasObjectUnifier)e.ModelElement).LeadRolePath;
			if (!rolePath.IsDeleted)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// AddRule: typeof(PathObjectUnifierUnifiesPathedRole)
		/// </summary>
		private static void PathedRoleUnificationAddedRule(ElementAddedEventArgs e)
		{
			LeadRolePath rolePath = ((PathObjectUnifierUnifiesPathedRole)e.ModelElement).ObjectUnifier.LeadRolePath;
			if (rolePath != null)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(PathObjectUnifierUnifiesPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void PathedRoleUnificationDeletedRule(ElementDeletedEventArgs e)
		{
			PathObjectUnifier objectUnifier = ((PathObjectUnifierUnifiesPathedRole)e.ModelElement).ObjectUnifier;
			if (!objectUnifier.IsDeleted)
			{
				LeadRolePath rolePath;
				if (1 >= (objectUnifier.PathedRoleCollection.Count + objectUnifier.PathRootCollection.Count))
				{
					objectUnifier.Delete();
				}
				else if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathObjectUnifierUnifiesPathedRole)
		/// </summary>
		private static void PathedRoleUnificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			LeadRolePath rolePath;
			PathObjectUnifier objectUnifier;
			if (e.DomainRole.Id == PathObjectUnifierUnifiesPathedRole.PathedRoleDomainRoleId)
			{
				if (null != (rolePath = ((PathObjectUnifierUnifiesPathedRole)e.ElementLink).ObjectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
			else
			{
				objectUnifier = (PathObjectUnifier)e.OldRolePlayer;
				if (1 >= (objectUnifier.PathedRoleCollection.Count + objectUnifier.PathRootCollection.Count))
				{
					objectUnifier.Delete();
				}
				else if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
				objectUnifier = (PathObjectUnifier)e.NewRolePlayer;
				if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(PathObjectUnifierUnifiesRolePathRoot)
		/// </summary>
		private static void RolePathRootUnificationAddedRule(ElementAddedEventArgs e)
		{
			LeadRolePath rolePath = ((PathObjectUnifierUnifiesRolePathRoot)e.ModelElement).ObjectUnifier.LeadRolePath;
			if (rolePath != null)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(PathObjectUnifierUnifiesRolePathRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void RolePathRootUnificationDeletedRule(ElementDeletedEventArgs e)
		{
			PathObjectUnifier objectUnifier = ((PathObjectUnifierUnifiesRolePathRoot)e.ModelElement).ObjectUnifier;
			if (!objectUnifier.IsDeleted)
			{
				LeadRolePath rolePath;
				if (1 >= (objectUnifier.PathedRoleCollection.Count + objectUnifier.PathRootCollection.Count))
				{
					objectUnifier.Delete();
				}
				else if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(PathObjectUnifierUnifiesRolePathRoot)
		/// </summary>
		private static void RolePathRootUnificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			LeadRolePath rolePath;
			PathObjectUnifier objectUnifier;
			if (e.DomainRole.Id == PathObjectUnifierUnifiesRolePathRoot.PathRootDomainRoleId)
			{
				if (null != (rolePath = ((PathObjectUnifierUnifiesRolePathRoot)e.ElementLink).ObjectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
			else
			{
				objectUnifier = (PathObjectUnifier)e.OldRolePlayer;
				if (1 >= (objectUnifier.PathedRoleCollection.Count + objectUnifier.PathRootCollection.Count))
				{
					objectUnifier.Delete();
				}
				else if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
				objectUnifier = (PathObjectUnifier)e.NewRolePlayer;
				if (null != (rolePath = objectUnifier.LeadRolePath))
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwner)
		/// </summary>
		private static void RolePathOwnerAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwner owner = (RolePathOwner)e.ModelElement;
			FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
			owner.NewlyCreated();
		}
		/// <summary>
		/// ChangeRule: typeof(RolePath)
		/// </summary>
		private static void RolePathChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == RolePath.SplitIsNegatedDomainPropertyId) // The validation does not currently check the split type.
			{
				AddDelayedPathValidation((RolePath)e.ModelElement);
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasLeadRolePath)
		/// See if the SingleLeadRolePath has changed
		/// </summary>
		private static void LeadRolePathAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			RolePathOwner owner = link.PathOwner;
			FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
			if (link is RolePathOwnerOwnsLeadRolePath)
			{
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
				foreach (CalculatedPathValue calculation in link.RolePath.CalculatedValueCollection)
				{
					// This would be unusual, but we need to make sure that any pre-attached calculations are validated
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDerivedRolePathOwner);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasLeadRolePath)
		/// Validate single role path and other owner changes when the
		/// path link is deleted. If the path itself is not deleted, transfer
		/// ownership of the path to a <see cref="RolePathOwner"/> that
		/// shares this path, or delete the path otherwise.
		/// </summary>
		private static void LeadRolePathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			bool isOwningLink = link is RolePathOwnerOwnsLeadRolePath;
			RolePathOwner owner = link.PathOwner;
			if (!owner.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
				if (isOwningLink)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDerivedRolePathOwner);
				}
			}
			LeadRolePath rolePath;
			if (isOwningLink &&
				!(rolePath = link.RolePath).IsDeleted)
			{
				foreach (RolePathOwnerUsesSharedLeadRolePath sharedLink in RolePathOwnerUsesSharedLeadRolePath.GetLinksToSharedWithPathOwnerCollection(rolePath))
				{
					RolePathOwner newOwner = sharedLink.PathOwner;
					sharedLink.Delete();
					new RolePathOwnerOwnsLeadRolePath(newOwner, rolePath);
					return;
				}
				rolePath.Delete();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasLeadRolePath)
		/// </summary>
		private static void LeadRolePathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			RolePathOwner owner;
			if (e.DomainRole.Id == RolePathOwnerHasLeadRolePath.PathOwnerDomainRoleId)
			{
				// The owner changed, delay validate old and new owners. Note that this
				// is not an initial add, so existing calculations will already be validated.
				owner = (RolePathOwner)e.OldRolePlayer;
				bool isOwnerLink = e.ElementLink is RolePathOwnerOwnsLeadRolePath;
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
				if (isOwnerLink)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDerivedRolePathOwner);
				}

				owner = (RolePathOwner)e.NewRolePlayer;
				FrameworkDomainModel.DelayValidateElement(owner, DelayValidateSingleLeadRolePath);
				if (isOwnerLink)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDerivedRolePathOwner);
				}
			}
			else
			{
				RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ElementLink;
				owner = link.PathOwner;
				if (link is RolePathOwnerOwnsLeadRolePath)
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateLeadRolePaths);
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(owner, DelayValidateDerivedRolePathOwner);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasSubquery)
		/// Validate state on subquery owner link deletion. If the subquery
		/// itself is not deleted, transfer ownership of the subquery to a
		/// <see cref="RolePathOwner"/> that shares this subquery, or delete
		/// the subquery otherwise.
		/// </summary>
		private static void SubqueryDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathOwnerHasSubquery link = (RolePathOwnerHasSubquery)e.ModelElement;
			bool isOwningLink = link is RolePathOwnerOwnsSubquery;
			RolePathOwner owner = link.PathOwner;
			Subquery subquery;
			if (isOwningLink &&
				!(subquery = link.Subquery).IsDeleted)
			{
				foreach (RolePathOwnerUsesSharedSubquery sharedLink in RolePathOwnerUsesSharedSubquery.GetLinksToSharedWithPathOwnerCollection(subquery))
				{
					RolePathOwner newOwner = sharedLink.PathOwner;
					sharedLink.Delete();
					new RolePathOwnerOwnsSubquery(newOwner, subquery);
					return;
				}
				subquery.Delete();
			}
		}
		/// <summary>
		/// If path component changes result in a single <see cref="LeadRolePath"/>,
		/// then populate <see cref="RolePathOwnerHasSingleLeadRolePath"/> relationship.
		/// </summary>
		private static void DelayValidateSingleLeadRolePath(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				RolePathOwner owner = (RolePathOwner)element;
				ReadOnlyCollection<RolePathOwnerHasLeadRolePath> pathLinks = RolePathOwnerHasLeadRolePath.GetLinksToLeadRolePathCollection(owner);
				LeadRolePath singleRolePath = null;
				LeadRolePath singleOwnedRolePath = null;
				if (pathLinks.Count == 1)
				{
					RolePathOwnerHasLeadRolePath pathLink = pathLinks[0];
					singleRolePath = pathLink.RolePath;
					if (pathLink is RolePathOwnerOwnsLeadRolePath)
					{
						singleOwnedRolePath = singleRolePath;
					}
				}
				if (owner.SingleLeadRolePath != singleRolePath)
				{
					owner.SingleLeadRolePath = singleRolePath;
				}
				if (owner.SingleOwnedLeadRolePath != singleOwnedRolePath)
				{
					owner.SingleOwnedLeadRolePath = singleOwnedRolePath;
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(LeadRolePathCalculatesCalculatedPathValue)
		/// </summary>
		private static void CalculationAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((LeadRolePathCalculatesCalculatedPathValue)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
			// Note that we don't have a delete rule: all of the error deletion propagates
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueAggregationContextIncludesPathedRole)
		/// </summary>
		private static void CalculationAggregationContextPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueAggregationContextIncludesPathedRole)e.ModelElement).CalculatedValue;
			calculation.UniversalAggregationContext = false;
			FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueAggregationContextIncludesPathedRole)
		/// </summary>
		private static void CalculationAggregationContextPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueAggregationContextIncludesPathedRole)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueAggregationContextIncludesRolePathRoot)
		/// </summary>
		private static void CalculationAggregationContextPathRootAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueAggregationContextIncludesRolePathRoot)e.ModelElement).CalculatedValue;
			calculation.UniversalAggregationContext = false;
			FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueAggregationContextIncludesRolePathRoot)
		/// </summary>
		private static void CalculationAggregationContextPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueAggregationContextIncludesRolePathRoot)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(CalculatedPathValue)
		/// </summary>
		private static void CalculationChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == CalculatedPathValue.UniversalAggregationContextDomainPropertyId)
			{
				CalculatedPathValue calculation = (CalculatedPathValue)e.ModelElement;
				if ((bool)e.NewValue)
				{
					calculation.AggregationContextPathedRoleCollection.Clear();
					calculation.AggregationContextPathRootCollection.Clear();
				}
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(LeadRolePathSatisfiesCalculatedCondition)
		/// </summary>
		private static void CalculationAsConditionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((LeadRolePathSatisfiesCalculatedCondition)e.ModelElement).CalculatedCondition, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(LeadRolePathSatisfiesCalculatedCondition)
		/// </summary>
		private static void CalculationAsConditionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((LeadRolePathSatisfiesCalculatedCondition)e.ModelElement).CalculatedCondition;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				// None of the old parameter errors will be valid, help the error validation by clearing these now.
				calculation.ParameterBindingErrorCollection.Clear();
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void CalculationFunctionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueIsCalculatedWithFunction)e.ElementLink).CalculatedValue;

				// None of the old parameter errors will be valid, help the error validation by clearing these now.
				calculation.ParameterBindingErrorCollection.Clear();
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueHasInput)
		/// </summary>
		private static void CalculationInputAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((CalculatedPathValueHasInput)e.ModelElement).CalculatedValue, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueHasInput)
		/// </summary>
		private static void CalculationInputDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueHasInput)e.ModelElement).CalculatedValue;
			if (!calculation.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInputBindsToCalculatedPathValue link = (CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement;
			CalculatedPathValue calculation = link.Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
			FrameworkDomainModel.DelayValidateElement(link.Source, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInputBindsToCalculatedPathValue link = (CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement;
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = link.Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
			if (!(calculation = link.Source).IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// </summary>
		private static void CalculationInputBindingToCalculatedValueRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToCalculatedPathValue)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
				FrameworkDomainModel.DelayValidateElement((CalculatedPathValue)e.OldRolePlayer, DelayValidateCalculatedPathValue);
				FrameworkDomainModel.DelayValidateElement((CalculatedPathValue)e.NewRolePlayer, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// </summary>
		private static void CalculationInputBindingToConstantRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathConstant)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// </summary>
		private static void CalculationInputBindingToPathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToPathedRole)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToRolePathRoot)
		/// </summary>
		private static void CalculationInputBindingToPathRootAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToRolePathRoot)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputBindsToRolePathRoot)
		/// </summary>
		private static void CalculationInputBindingToPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputBindsToRolePathRoot)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputBindsToRolePathRoot)
		/// </summary>
		private static void CalculationInputBindingToPathRootRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputBindsToRolePathRoot)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterAddedRule(ElementAddedEventArgs e)
		{
			CalculatedPathValue calculation = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ModelElement).Input.CalculatedValue;
			if (calculation != null)
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValueInput input;
			CalculatedPathValue calculation;
			if (!(input = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ModelElement).Input).IsDeleted &&
				null != (calculation = input.CalculatedValue))
			{
				FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueInputCorrespondsToFunctionParameter)
		/// </summary>
		private static void CalculationInputTargetsParameterRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId)
			{
				CalculatedPathValue calculation = ((CalculatedPathValueInputCorrespondsToFunctionParameter)e.ElementLink).Input.CalculatedValue;
				if (calculation != null)
				{
					FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			foreach (RolePath rolePath in ((ObjectTypePlaysRole)e.ModelElement).PlayedRole.RolePathCollection)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			Role playedRole = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole;
			if (!playedRole.IsDeleted)
			{
				foreach (RolePath rolePath in playedRole.RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				foreach (RolePath rolePath in ((ObjectTypePlaysRole)e.ElementLink).PlayedRole.RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
			else
			{
				foreach (RolePath rolePath in ((Role)e.OldRolePlayer).RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
				foreach (RolePath rolePath in ((Role)e.NewRolePlayer).RolePathCollection)
				{
					AddDelayedPathValidation(rolePath);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathObjectTypeRoot)
		/// </summary>
		private static void RootObjectTypeAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((RolePathObjectTypeRoot)e.ModelElement).RolePath);
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathObjectTypeRoot)
		/// </summary>
		private static void RootObjectTypeDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath rolePath = ((RolePathObjectTypeRoot)e.ModelElement).RolePath;
			if (!rolePath.IsDeleted)
			{
				AddDelayedPathValidation(rolePath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathObjectTypeRoot)
		/// </summary>
		private static void RootObjectTypeRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathObjectTypeRoot.RootObjectTypeDomainRoleId)
			{
				AddDelayedPathValidation(((RolePathObjectTypeRoot)e.ElementLink).RolePath);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			MandatoryConstraint constraint;
			if (null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				(constraint.IsSimple || constraint.IsImplied)) // Note that implied single role mandatories can disqualify outer joins
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(link.Role))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintChangedRule(ElementPropertyChangedEventArgs e)
		{
			MandatoryConstraint constraint;
			LinkedElementCollection<Role> constraintRoles;
			if (e.DomainProperty.Id == MandatoryConstraint.ModalityDomainPropertyId &&
				((constraint = (MandatoryConstraint)e.ModelElement).IsSimple || constraint.IsImplied) &&
				1 == (constraintRoles = constraint.RoleCollection).Count)
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(constraintRoles[0]))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Track outer join coupled with mandatory mismatch
		/// </summary>
		private static void SimpleMandatoryConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			Role role = link.Role;
			MandatoryConstraint constraint;
			if (!role.IsDeleted &&
				null != (constraint = link.ConstraintRoleSequence as MandatoryConstraint) &&
				(constraint.IsSimple || constraint.IsImplied)) // Note that implied single role mandatories can disqualify outer joins
			{
				foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(role))
				{
					if (pathedRole.PathedRolePurpose == PathedRolePurpose.PostOuterJoin)
					{
						AddDelayedPathValidation(pathedRole.RolePath);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathAddedRule(ElementAddedEventArgs e)
		{
			AddDelayedPathValidation(((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath);
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePath parentPath = ((RoleSubPathIsContinuationOfRolePath)e.ModelElement).ParentRolePath;
			if (!parentPath.IsDeleted)
			{
				 AddDelayedPathValidation(parentPath);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath)
		/// </summary>
		private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId)
			{
				AddDelayedPathValidation(((RoleSubPathIsContinuationOfRolePath)e.ElementLink).ParentRolePath);
			}
			else
			{
				AddDelayedPathValidation((RolePath)e.OldRolePlayer);
				AddDelayedPathValidation((RolePath)e.NewRolePlayer);
			}
		}
		#endregion // Rule Methods
		#region DataType Change Tracking (rules and callback registration)
		/// <summary>
		/// Callback delegate used as an extension point for notifications
		/// when data type changes are made that can affect role path projections.
		/// Notification delegates can be added with the <see cref="RolePathOwner.AddRolePathDataTypeChangeRuleNotification"/> method.
		/// </summary>
		/// <param name="valueType">The value type that has a modified data type.</param>
		/// <param name="playedRoles">Roles played by this value type</param>
		/// <param name="pathNode">A path node that uses this data type. If the path node is
		/// empty, then the callback should find additional nodes that can be affected.</param>
		protected delegate void RolePathDataTypeChange(ObjectType valueType, IList<Role> playedRoles, RolePathNode pathNode);
		/// <summary>
		/// Retrieve the delegates registered with <see cref="AddRolePathDataTypeChangeRuleNotification"/>
		/// </summary>
		private static RolePathDataTypeChange GetRolePathDataTypeChangeRuleNotifications(Store store)
		{
			object callback;
			return store.PropertyBag.TryGetValue(typeof(RolePathDataTypeChange), out callback) ? callback as RolePathDataTypeChange : null;
		}
		/// <summary>
		/// Add a callback delegate that should be notified when a change is made
		/// to the use of a data type that may affect the path validation.
		/// This is meant to avoid rewriting redundant rules for data type tracking
		/// from each path type. Callbacks should be added in the <see cref="Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization"/>
		/// method for each of the derived types.
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		/// <param name="dataTypeChangeCallback">A validation method that should run when the store changes.
		/// This method will generally use <see cref="FrameworkDomainModel.DelayValidateElement"/> to
		/// perform delayed validation of affected parts of the role path.</param>
		protected static void AddRolePathDataTypeChangeRuleNotification(Store store, RolePathDataTypeChange dataTypeChangeCallback)
		{
			object key = typeof(RolePathDataTypeChange);
			Dictionary<object, object> bag = store.PropertyBag;
			RolePathDataTypeChange newDataTypeChange;
			object value;
			if (bag.TryGetValue(key, out value) &&
				null != (newDataTypeChange = value as RolePathDataTypeChange))
			{
				newDataTypeChange += dataTypeChangeCallback;
			}
			else
			{
				newDataTypeChange = dataTypeChangeCallback;
			}
			bag[key] = newDataTypeChange;
		}
		/// <summary>
		/// AddRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void DataTypeUseAddedRule(ElementAddedEventArgs e)
		{
			DataTypeChangedForValueType(((ValueTypeHasDataType)e.ModelElement).ValueType);
		}
		/// <summary>
		/// DeleteRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void DataTypeUseDeletedRule(ElementDeletedEventArgs e)
		{
			ObjectType valueType = ((ValueTypeHasDataType)e.ModelElement).ValueType;
			if (!valueType.IsDeleted)
			{
				DataTypeChangedForValueType(valueType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void DataTypeUseFacetChangedRule(ElementPropertyChangedEventArgs e)
		{
			ValueTypeHasDataType link = (ValueTypeHasDataType)e.ModelElement;
			DataType dataType = link.DataType;
			Guid propertyId = e.DomainProperty.Id;
			if ((dataType.LengthName != null && propertyId == ValueTypeHasDataType.LengthDomainPropertyId) ||
				(dataType.ScaleName != null && propertyId == ValueTypeHasDataType.ScaleDomainPropertyId))
			{
				DataTypeChangedForValueType(link.ValueType);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
		/// </summary>
		private static void DataTypeUseRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				DataTypeChangedForValueType(((ValueTypeHasDataType)e.ElementLink).ValueType);
			}
		}
		private static void DataTypeChangedForValueType(ObjectType valueType)
		{
			Store store = valueType.Store;
			RolePathDataTypeChange changeCallback;
			if (null != (changeCallback = GetRolePathDataTypeChangeRuleNotifications(store))) // Defensive, we register callbacks, so these should always be set.
			{
				LinkedElementCollection<Role> playedRoles = valueType.PlayedRoleCollection;
				foreach (RolePathObjectTypeRoot pathRoot in RolePathObjectTypeRoot.GetLinksToRolePathCollection(valueType))
				{
					changeCallback(valueType, playedRoles, pathRoot);
				}
				foreach (Role role in playedRoles)
				{
					foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(role))
					{
						changeCallback(valueType, playedRoles, pathedRole);
					}
				}
				// Let the callbacks handle other elements that aren't managed by the default owner.
				changeCallback(valueType, playedRoles, RolePathNode.Empty);
			}
		}
		#endregion // DataType Change Tracking (rules and registration)
		#region Path Validation
		/// <summary>
		/// Helper to choose between root path and subpath validators
		/// </summary>
		private static void AddDelayedPathValidation(RolePath path)
		{
			if (path is LeadRolePath)
			{
				FrameworkDomainModel.DelayValidateElement(path, DelayValidateLeadRolePath);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(path, DelayValidateSubpath);
			}
		}
		/// <summary>
		/// Intermediate validator to allow rules to delay validate
		/// the closest element in the path hierarchy.
		/// </summary>
		/// <param name="element">A <see cref="RoleSubPath"/></param>
		[DelayValidatePriority(-2)] // Before component validation
		private static void DelayValidateSubpath(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(((RoleSubPath)element).RootRolePath, DelayValidateLeadRolePath);
			}
		}
		/// <summary>
		/// Intermediate validator to allow rules to delay validate the closest
		/// element in the path hierarchy. Defers indirectly to the owner validator.
		/// </summary>
		/// <param name="element">A <see cref="LeadRolePath"/></param>
		[DelayValidatePriority(-1)] // Run before the owner validation
		private static void DelayValidateLeadRolePath(ModelElement element)
		{
			RolePathOwner pathOwner;
			if (!element.IsDeleted &&
				null != (pathOwner = ((LeadRolePath)element).PathOwner))
			{
				FrameworkDomainModel.DelayValidateElement(pathOwner, DelayValidateLeadRolePaths);
			}
		}
		/// <summary>
		/// Validate all role path components except calculations
		/// </summary>
		/// <param name="element">A <see cref="RolePathOwner"/></param>
		private static void DelayValidateLeadRolePaths(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RolePathOwner)element).ValidateRolePaths(false, null);
				FrameworkDomainModel.DelayValidateElement(element, DelayValidateDerivedRolePathOwner);
			}
		}
		/// <summary>
		/// Call to the <see cref="ValidateDerivedRolePathOwner"/> after core path
		/// validation is complete.
		/// </summary>
		[DelayValidatePriority(1)] // Run after DelayValidateLeadRolePaths
		protected static void DelayValidateDerivedRolePathOwner(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RolePathOwner)element).ValidateDerivedRolePathOwner(null);
			}
		}
		/// <summary>
		/// Validate all role path components including calculations
		/// </summary>
		/// <param name="element">A <see cref="RolePathOwner"/></param>
		private static void DelayValidateLeadRolePathsWithCalculations(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RolePathOwner)element).ValidateRolePaths(true, null);
				FrameworkDomainModel.DelayValidateElement(element, DelayValidateDerivedRolePathOwner);
			}
		}
		/// <summary>
		/// Validate all path components
		/// </summary>
		/// <param name="validateCalculations">Validate calculated values with the path components.</param>
		/// <param name="notifyAdded">Notification callback for added errors.</param>
		private void ValidateRolePaths(bool validateCalculations, INotifyElementAdded notifyAdded)
		{
			Partition partition = Partition;
			ORMModel model = null;
			BitTracker roleUseTracker = new BitTracker(0);
			Stack<LinkedElementCollection<RoleBase>> factTypeRolesStack = new Stack<LinkedElementCollection<RoleBase>>();
			ObjectType[] compatibilityTester = null;
			foreach (LeadRolePath leadRolePath in OwnedLeadRolePathCollection)
			{
				// Walk all pathed roles to check path structure and join compatibility. Errors
				// are specified on the pathed roles.
				bool havePathNodes = false;
				RolePathNode.VisitPathNodes(
					leadRolePath,
					RolePathNode.Empty,
					true,
					delegate(RolePathNode pathNode, RolePathNode previousPathNode, bool unwinding)
					{
						RolePath currentPath;
						RolePathObjectTypeRoot pathRoot = pathNode;
						PathedRole currentPathedRole = null;
						if (pathRoot != null)
						{
							currentPath = pathRoot.RolePath;
						}
						else
						{
							currentPathedRole = pathNode;
							currentPath = currentPathedRole.RolePath;
						}
						if (unwinding)
						{
							#region FactType Role Stack Maintenance
							// All errors are checked moving forward in the path. All we need to do
							// is keep the fact type roles stack in order so that we don't lose
							// track of the current fact type an in intra-fact type split. This is a
							// skeleton version of the corresponding conditions in the forward part
							// of the callback.
							bool popFactType = false;
							PathedRole previousPathedRole = previousPathNode;
							if (previousPathNode.IsEmpty)
							{
								popFactType = currentPathedRole != null;
							}
							else if (currentPathedRole != null)
							{
								if (previousPathedRole == null)
								{
									popFactType = true;
								}
								else
								{
									switch (currentPathedRole.PathedRolePurpose)
									{
										case PathedRolePurpose.PostInnerJoin:
										case PathedRolePurpose.PostOuterJoin:
											popFactType = true;
											break;
										case PathedRolePurpose.SameFactType:
											{
												// See comments in the !unwinding branch for discussion
												// of conditions here.
												Role currentRole = currentPathedRole.Role;
												FactType currentFactType = currentRole.FactType;
												Role previousRole = previousPathedRole.Role;
												FactType previousFactType = previousRole.FactType;
												if (previousFactType != currentFactType)
												{
													popFactType = true;
													Objectification objectification;
													RoleProxy proxy;
													if (previousPathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType &&
														((null != (objectification = currentFactType.ImpliedByObjectification) &&
														null != (proxy = previousRole.Proxy) &&
														proxy.FactType == currentFactType) ||
														(null != (proxy = currentRole.Proxy) &&
														previousFactType == proxy.FactType)))
													{
														// Do nothing. No fact type was pushed in the forward iteration
														popFactType = false;
													}
												}
											}
											break;
									}
								}
							}
							if (popFactType)
							{
								roleUseTracker.Resize(roleUseTracker.Count - factTypeRolesStack.Pop().Count);
							}
							#endregion // FactType Role Stack Maintenance
						}
						else
						{
							havePathNodes = true;
							#region Tracked errors
							bool hasRootObjectTypeRequiredError = false;
							bool removeRootObjectTypeRequiredError = false;
							bool hasSameFactTypeWithoutJoinError = false;
							bool hasOuterJoinError = false;
							bool hasJoinCompatibilityError = false;
							#endregion // Tracked errors
							#region Validate PathedRole Combinations
							// State information
							Role currentRole;
							FactType currentFactType;
							FactType pushFactType = null;
							ObjectType testCompatibilityWith = null;
							ObjectType currentRolePlayer = null;
							LinkedElementCollection<RoleBase> factTypeRoles;
							int resolvedRoleIndex;

							if (previousPathNode.IsEmpty)
							{
								if (currentPathedRole != null)
								{
									// We got here without passing a root node, so we do
									// not have a root object type specified above us and
									// are in error.
									hasRootObjectTypeRequiredError = true;
									currentRole = currentPathedRole.Role;
									currentFactType = pushFactType = currentRole.FactType;
									switch (currentPathedRole.PathedRolePurpose)
									{
										case PathedRolePurpose.SameFactType:
											hasSameFactTypeWithoutJoinError = true;
											break;
										case PathedRolePurpose.PostOuterJoin:
											hasOuterJoinError = currentRole.SingleRoleAlethicMandatoryConstraint != null;
											break;
									}
								}
								else
								{
									removeRootObjectTypeRequiredError = true;
									currentRole = null;
									currentFactType = null;
								}
							}
							else
							{
								RolePathObjectTypeRoot previousPathRoot = previousPathNode;
								RolePath previousPath;
								PathedRole previousPathedRole = null;
								if (previousPathRoot != null)
								{
									previousPath = previousPathRoot.RolePath;
								}
								else
								{
									previousPathedRole = previousPathNode;
									previousPath = previousPathedRole.RolePath;
								}
								if (previousPath != currentPath)
								{
									// We're moving into a subpath, which never requires a root object type.
									// Remove any existing error on entry.
									removeRootObjectTypeRequiredError = true;
								}
								if (currentPathedRole != null)
								{
									currentRole = currentPathedRole.Role;
									currentFactType = currentRole.FactType;
									switch (currentPathedRole.PathedRolePurpose)
									{
										case PathedRolePurpose.PostInnerJoin:
											testCompatibilityWith = previousPathNode.ObjectType;
											pushFactType = currentFactType;
											break;
										case PathedRolePurpose.PostOuterJoin:
											hasOuterJoinError = currentRole.SingleRoleAlethicMandatoryConstraint != null;
											testCompatibilityWith = previousPathNode.ObjectType;
											pushFactType = currentFactType;
											break;
										case PathedRolePurpose.SameFactType:
											{
												if (previousPathedRole == null)
												{
													pushFactType = currentFactType;
													hasSameFactTypeWithoutJoinError = true;
												}
												else
												{
													// Push a new fact type if this role is not part of the previous fact type.
													// This is complicated by the ability of any role in an objectification
													// to be associated either with the objectified fact type or the link fact type.
													Role previousRole = previousPathedRole.Role;
													FactType previousFactType = previousRole.FactType;
													if (previousFactType != currentFactType)
													{
														pushFactType = currentFactType;
														if (previousPathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType)
														{
															hasSameFactTypeWithoutJoinError = true;
														}
														else
														{
															// See if we're encountering a link fact type situation here.
															Objectification objectification;
															RoleProxy proxy;
															if (null != (objectification = currentFactType.ImpliedByObjectification) &&
																null != (proxy = previousRole.Proxy) &&
																proxy.FactType == currentFactType)
															{
																// We're using an objectified role as an entry role
																// into the link fact type. We guessed wrong and pushed
																// the wrong fact type on the previous step, so update
																// the information to correspond to this fact type.
																factTypeRoles = factTypeRolesStack.Pop();
																int roleUseBaseIndex = roleUseTracker.Count - factTypeRoles.Count;
																roleUseTracker.Resize(roleUseBaseIndex);
																factTypeRoles = currentFactType.RoleCollection;
																roleUseTracker.Resize(roleUseBaseIndex + factTypeRoles.Count);
																roleUseTracker[roleUseBaseIndex + factTypeRoles.IndexOf(ResolveRoleBaseInFactType(previousRole, currentFactType))] = true;
																factTypeRolesStack.Push(factTypeRoles);
																pushFactType = null; // Set the use for the remaining role with continued processing
															}
															else if (null != (proxy = currentRole.Proxy) &&
																previousFactType == proxy.FactType)
															{
																// We're using the role opposite the proxy for the entry role.
																// We have the right fact type on the stack, so there is nothing more to do.
																currentFactType = previousFactType; // We need the link fact type to resolve the role index below
																pushFactType = null;
															}
															else
															{
																hasSameFactTypeWithoutJoinError = true;
															}
														}
													}
													else if ((currentRole == previousRole &&
														previousPathedRole.PathedRolePurpose == PathedRolePurpose.SameFactType &&
														null != currentRole.Proxy) ||
														(-1 != (resolvedRoleIndex = (factTypeRoles = factTypeRolesStack.Peek()).IndexOf(ResolveRoleBaseInFactType(currentRole, currentFactType))) &&
														roleUseTracker[roleUseTracker.Count - factTypeRoles.Count + resolvedRoleIndex]))
													{
														// Make sure the current role is not already pathed in this fact type entry. The
														// first condition is a special test for a transition from a link to the same role
														// in an absorbed fact type. In this case, it isn't worth the trouble to see if
														// we have the right set of fact type roles (link or normal) because we can test
														hasSameFactTypeWithoutJoinError = true;
														pushFactType = currentFactType; // Push another fact type use, we should have a join here
													}
												}
											}
											break;
									}
								}
								else
								{
									currentRole = null;
									currentFactType = null;
								}
							}
							if (currentFactType != null)
							{
								if (pushFactType != null)
								{
									factTypeRoles = pushFactType.RoleCollection;
									roleUseTracker.Resize(roleUseTracker.Count + factTypeRoles.Count);
									factTypeRolesStack.Push(factTypeRoles);
								}
								else
								{
									factTypeRoles = factTypeRolesStack.Peek();
								}
								resolvedRoleIndex = factTypeRoles.IndexOf(ResolveRoleBaseInFactType(currentRole, currentFactType));
								if (resolvedRoleIndex != -1) // Defensive, should always be set at this point
								{
									roleUseTracker[resolvedRoleIndex + roleUseTracker.Count - factTypeRoles.Count] = true;
								}
							}
							#endregion // Validate PathedRole Combinations
							#region Root Object Type Validation
							if (hasRootObjectTypeRequiredError)
							{
								PathRequiresRootObjectTypeError rootError = currentPath.RootObjectTypeRequiredError;
								if (rootError == null)
								{
									rootError = new PathRequiresRootObjectTypeError(partition);
									rootError.RolePath = currentPath;
									rootError.Model = model ?? (model = this.Model);
									rootError.GenerateErrorText();
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(rootError, true);
									}
								}
							}
							else if (removeRootObjectTypeRequiredError)
							{
								PathRequiresRootObjectTypeError rootError = currentPath.RootObjectTypeRequiredError;
								if (rootError != null)
								{
									rootError.Delete();
								}
							}
							#endregion // Root Object Type Validation
							#region PathedRole Errors
							if (currentPathedRole != null)
							{
								#region Join Compatibility Verification
								if (testCompatibilityWith != null)
								{
									currentRolePlayer = (currentRolePlayer ?? (currentRolePlayer = currentRole.RolePlayer));
									if (currentRolePlayer != null &&
										currentRolePlayer != testCompatibilityWith)
									{
										(compatibilityTester ?? (compatibilityTester = new ObjectType[2]))[0] = currentRolePlayer;
										compatibilityTester[1] = testCompatibilityWith;
										if (ObjectType.GetNearestCompatibleTypes(compatibilityTester, true).Length == 0)
										{
											hasJoinCompatibilityError = true;
										}
									}
								}
								#endregion // Join Compatibility Verification
								#region Attach or clear pathedRole errors
								PathSameFactTypeRoleFollowsJoinError sameFactTypeWithoutJoinError = currentPathedRole.SameFactTypeRoleWithoutJoinError;
								if (hasSameFactTypeWithoutJoinError)
								{
									if (sameFactTypeWithoutJoinError == null)
									{
										sameFactTypeWithoutJoinError = new PathSameFactTypeRoleFollowsJoinError(partition);
										sameFactTypeWithoutJoinError.PathedRole = currentPathedRole;
										sameFactTypeWithoutJoinError.Model = model ?? (model = this.Model);
										sameFactTypeWithoutJoinError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(sameFactTypeWithoutJoinError, true);
										}
									}
								}
								else if (sameFactTypeWithoutJoinError != null)
								{
									sameFactTypeWithoutJoinError.Delete();
								}

								PathOuterJoinRequiresOptionalRoleError outerJoinError = currentPathedRole.MandatoryOuterJoinError;
								if (hasOuterJoinError)
								{
									if (outerJoinError == null)
									{
										outerJoinError = new PathOuterJoinRequiresOptionalRoleError(partition);
										outerJoinError.PathedRole = currentPathedRole;
										outerJoinError.Model = model ?? (model = this.Model);
										outerJoinError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(outerJoinError, true);
										}
									}
								}
								else if (outerJoinError != null)
								{
									outerJoinError.Delete();
								}

								JoinedPathRoleRequiresCompatibleRolePlayerError joinCompatibilityError = currentPathedRole.JoinCompatibilityError;
								if (hasJoinCompatibilityError)
								{
									if (joinCompatibilityError == null)
									{
										joinCompatibilityError = new JoinedPathRoleRequiresCompatibleRolePlayerError(partition);
										joinCompatibilityError.PathedRole = currentPathedRole;
										joinCompatibilityError.Model = model ?? (model = this.Model);
										joinCompatibilityError.GenerateErrorText();
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(joinCompatibilityError, true);
										}
									}
								}
								else if (joinCompatibilityError != null)
								{
									joinCompatibilityError.Delete();
								}
								#endregion // Attach or clear pathedRole errors
							}
							#endregion // PathedRole Errors
						}
						return true; // Continue iteration
					});

				#region Empty Path Root Object Type Error
				if (!havePathNodes)
				{
					// If the path is completely empty, then we did not enter
					// the validation callbacks, so we do not have a root
					// object type. Specify this as a requirement on the lead path.
					PathRequiresRootObjectTypeError rootError = leadRolePath.RootObjectTypeRequiredError;
					if (rootError == null)
					{
						rootError = new PathRequiresRootObjectTypeError(partition);
						rootError.RolePath = leadRolePath;
						rootError.Model = model ?? (model = this.Model);
						rootError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(rootError, true);
						}
					}
				}
				#endregion // Empty Path Root Object Type Error
				#region ObjectUnifier Compatibility
				LinkedElementCollection<PathObjectUnifier> objectUnifiers = leadRolePath.ObjectUnifierCollection;
				if (objectUnifiers.Count != 0)
				{
					ObjectType currentObjectType = null;
					Predicate<ObjectType> testCompatibility = delegate(ObjectType testObjectType)
					{
						if (currentObjectType == null)
						{
							currentObjectType = testObjectType;
							return true;
						}
						else if (testObjectType == currentObjectType ||
							testObjectType == null) // Null types are handled by other errors, no point in checking compatibility.
						{
							return true;
						}
						(compatibilityTester ?? (compatibilityTester = new ObjectType[2]))[0] = currentObjectType;
						compatibilityTester[1] = testObjectType;
						if (ObjectType.GetNearestCompatibleTypes(compatibilityTester, true).Length == 0)
						{
							return false;
						}
						return true;
					};
					foreach (PathObjectUnifier objectUnifier in leadRolePath.ObjectUnifierCollection)
					{
						bool hasUnificationCompatibilityError = false;
						currentObjectType = null;
						foreach (RolePathObjectTypeRoot pathRoot in objectUnifier.PathRootCollection)
						{
							if (!testCompatibility(pathRoot.RootObjectType))
							{
								hasUnificationCompatibilityError = true;
								break;
							}
						}
						if (!hasUnificationCompatibilityError)
						{
							foreach (PathedRole pathedRole in objectUnifier.PathedRoleCollection)
							{
								if (!testCompatibility(pathedRole.Role.RolePlayer))
								{
									hasUnificationCompatibilityError = true;
									break;
								}
							}
						}
						PathObjectUnifierRequiresCompatibleObjectTypesError unifierCompatibilityError = objectUnifier.CompatibilityError;
						if (hasUnificationCompatibilityError)
						{
							if (unifierCompatibilityError == null)
							{
								unifierCompatibilityError = new PathObjectUnifierRequiresCompatibleObjectTypesError(partition);
								unifierCompatibilityError.ObjectUnifier = objectUnifier;
								unifierCompatibilityError.Model = model ?? (model = this.Model);
								unifierCompatibilityError.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(unifierCompatibilityError, true);
								}
							}
						}
						else if (unifierCompatibilityError != null)
						{
							unifierCompatibilityError.Delete();
						}
					}
				}
				#endregion // ObjectUnifier Compatibility

				#region Validate calculation completeness
				if (validateCalculations)
				{
					foreach (CalculatedPathValue calculation in leadRolePath.CalculatedValueCollection)
					{
						ValidateCalculatedPathValue(calculation, this, notifyAdded, ref model);
					}
				}
				#endregion // Validate calculation completeness
			}
		}
		/// <summary>
		/// A callback point used during path validation to enable extensions
		/// to be validated along with the base.
		/// </summary>
		/// <remarks>This should be called after <see cref="ValidateRolePaths"/>
		/// is completed, but is kept separate to enable finer-grained control
		/// over path-specific and owner-specific settings.
		/// </remarks>
		/// <param name="notifyAdded">Notification callback for added elements and errors.</param>
		protected virtual void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			// Intentionally empty
		}
		/// <summary>
		/// A helper callback for derived classes that consume calculations. Called during rule execution.
		/// </summary>
		/// <param name="calculation">A <see cref="CalculatedPathValue"/> to verify errors for.</param>
		protected static void CalculatedValueUseChangedInRule(CalculatedPathValue calculation)
		{
			FrameworkDomainModel.DelayValidateElement(calculation, DelayValidateCalculatedPathValue);
		}
		/// <summary>
		/// Called when a new <see cref="RolePathOwner"/> is created. Allows
		/// derived classes to add their own validators on initial creation.
		/// This is called before any parent relationships are established,
		/// so it is intended for registering delayed validation routines only.
		/// </summary>
		protected virtual void NewlyCreated()
		{
			// Intentionally empty
		}
		/// <summary>
		/// Determine if a <see cref="CalculatedPathValue"/> is consumed by the path owner.
		/// Derived classes can override this method to add additional consumption patterns
		/// for calculated values.
		/// </summary>
		protected virtual bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return calculation.BoundInputCollection.Count != 0 || calculation.RequiredForLeadRolePath != null;
		}
		/// <summary>
		/// Delayed validator for changes in a <see cref="CalculatedPathValue"/>
		/// </summary>
		private static void DelayValidateCalculatedPathValue(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				ORMModel model = null;
				ValidateCalculatedPathValue((CalculatedPathValue)element, null, null, ref model);
			}
		}
		/// <summary>
		/// Validate calculation structural requirements. The function must be specified,
		/// all parameters must be bound and have some input source, and the calculation
		/// result must be consumed in the model.
		/// </summary>
		/// <param name="calculation">The <see cref="CalculatedPathValue"/> to validate.</param>
		/// <param name="rolePathOwner">The containing <see cref="RolePathOwner"/> for this calculation.</param>
		/// <param name="notifyAdded">Callback notification used during deserialization.</param>
		/// <param name="contextModel">The context <see cref="ORMModel"/>. Calculated automatically if <see langword="null"/></param>
		private static void ValidateCalculatedPathValue(CalculatedPathValue calculation, RolePathOwner rolePathOwner, INotifyElementAdded notifyAdded, ref ORMModel contextModel)
		{
			Function function = calculation.Function;
			Partition partition = calculation.Partition;
			CalculatedPathValueRequiresFunctionError functionRequiredError = calculation.FunctionRequiredError;
			CalculatedPathValueRequiresAggregationContextError aggregationContextRequiredError = calculation.AggregationContextRequiredError;
			if (function == null)
			{
				if (notifyAdded != null)
				{
					// Initial check only. These are enforced in rules.
					calculation.ParameterBindingErrorCollection.Clear();
					calculation.InputCollection.Clear();
					calculation.RequiredForLeadRolePath = null;
				}
				if (functionRequiredError == null &&
					(null != contextModel || null != (contextModel = calculation.Model)))
				{
					functionRequiredError = new CalculatedPathValueRequiresFunctionError(partition);
					functionRequiredError.CalculatedPathValue = calculation;
					functionRequiredError.Model = contextModel;
					functionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(functionRequiredError, true);
					}
				}
				if (aggregationContextRequiredError != null)
				{
					aggregationContextRequiredError.Delete();
				}
			}
			else
			{
				if (functionRequiredError != null)
				{
					functionRequiredError.Delete();
				}

				ReadOnlyCollection<CalculatedPathValueHasUnboundParameterError> bindingErrorLinks = CalculatedPathValueHasUnboundParameterError.GetLinksToParameterBindingErrorCollection(calculation);
				int originalBindingErrorCount = bindingErrorLinks.Count;
				if (notifyAdded != null)
				{
					// Make sure the existing binding errors target the correct function. Load check only,
					// this collection is cleared in calculated value rules on function deletion and change.
					foreach (CalculatedPathValueHasUnboundParameterError bindingErrorLink in bindingErrorLinks)
					{
						if (bindingErrorLink.Parameter.Function != function)
						{
							--originalBindingErrorCount;
							bindingErrorLink.Delete();
						}
					}
				}
				LinkedElementCollection<FunctionParameter> parameters = function.ParameterCollection;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculation.InputCollection;
				int inputCount = inputs.Count;
				if (parameters.Count == 0)
				{
					inputs.Clear(); // No parameters means no inputs
				}
				else
				{
					// There are several options here for matching parameters and inputs.
					// Jumping from the parameters to all inputs is easier code, but the
					// sets involved are not bounded in any way. Nesting the parameters
					// and defined inputs will be the cleanest approach.
					foreach (FunctionParameter parameter in parameters)
					{
						bool hasError = true;
						if (inputCount != 0)
						{
							foreach (CalculatedPathValueInput input in inputs)
							{
								if (input.Parameter == parameter)
								{
									// We've matched the parameter. Now, make sure that there
									// is some data on the input.
									if (input.SourcePathedRole != null ||
										input.SourcePathRoot != null ||
										input.SourceCalculatedValue != null ||
										input.SourceConstant != null)
									{
										hasError = false;
									}
									else
									{
										// Do not keep an empty input
										input.Delete();
										--inputCount;
									}
									break;
								}
							}
						}
						if (originalBindingErrorCount != 0)
						{
							foreach (CalculatedPathValueHasUnboundParameterError bindingErrorLink in bindingErrorLinks)
							{
								if (!bindingErrorLink.IsDeleted && bindingErrorLink.Parameter == parameter)
								{
									if (!hasError)
									{
										--originalBindingErrorCount;
										bindingErrorLink.Delete();
									}
									else
									{
										hasError = false; // No more processing needed, we have an intact error object
									}
									break;
								}
							}
						}
						if (hasError &&
							(null != contextModel || null != (contextModel = calculation.Model)))
						{
							CalculatedPathValueParameterBindingError bindingError = new CalculatedPathValueParameterBindingError(partition);
							CalculatedPathValueHasUnboundParameterError bindingErrorLink = new CalculatedPathValueHasUnboundParameterError(calculation, bindingError);
							bindingErrorLink.Parameter = parameter;
							bindingError.Model = contextModel;
							bindingError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(bindingError, false);
								notifyAdded.ElementAdded(bindingErrorLink, true);
							}
						}
					}
					if (inputCount != 0)
					{
						for (int i = inputCount - 1; i >= 0; --i) // Walk backwards to support deletion
						{
							CalculatedPathValueInput input = inputs[i];
							FunctionParameter parameter;
							if (null == (parameter = input.Parameter) || // Defensive, highly unlikely
								parameter.Function != function)
							{
								input.Delete();
							}
						}
					}
				}

				if (!function.IsAggregate ||
					calculation.UniversalAggregationContext ||
					calculation.AggregationContextPathRootCollection.Count != 0 ||
					calculation.AggregationContextPathedRoleCollection.Count != 0)
				{
					if (aggregationContextRequiredError != null)
					{
						aggregationContextRequiredError.Delete();
					}
				}
				else if (null == aggregationContextRequiredError &&
					(null != contextModel || null != (contextModel = calculation.Model)))
				{
					aggregationContextRequiredError = new CalculatedPathValueRequiresAggregationContextError(partition);
					aggregationContextRequiredError.CalculatedPathValue = calculation;
					aggregationContextRequiredError.Model = contextModel;
					aggregationContextRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(aggregationContextRequiredError, true);
					}
				}
			}

			// Finally, test if anything is using this calculation
			CalculatedPathValueMustBeConsumedError consumptionError = calculation.ConsumptionRequiredError;
			LeadRolePath rolePath;
			if ((null != rolePathOwner ||
				(null != (rolePath = calculation.LeadRolePath) &&
				null != (rolePathOwner = rolePath.PathOwner))) &&
				!rolePathOwner.IsCalculatedPathValueConsumed(calculation))
			{
				if (consumptionError == null &&
					(null != contextModel || null != (contextModel = rolePathOwner.Model)))
				{
					consumptionError = new CalculatedPathValueMustBeConsumedError(partition);
					consumptionError.CalculatedPathValue = calculation;
					consumptionError.Model = contextModel;
					consumptionError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(consumptionError, true);
					}
				}
			}
			else if (consumptionError != null)
			{
				consumptionError.Delete();
			}
		}
		/// <summary>
		/// Given a <see cref="Role"/> and <see cref="FactType"/>, determine
		/// the corresponding <see cref="RoleBase"/> that is either in the
		/// normal or implied fact type.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> to resolve.</param>
		/// <param name="factType">The <see cref="FactType"/> to get the returned <see cref="RoleBase"/> in.</param>
		/// <returns>The resolved <see cref="RoleBase"/></returns>
		private static RoleBase ResolveRoleBaseInFactType(Role role, FactType factType)
		{
			if (role.FactType != factType)
			{
				RoleProxy proxy = role.Proxy;
				if (proxy != null && proxy.FactType == factType)
				{
					return proxy;
				}
			}
			return role;
		}
		#endregion // Path Validation
	}
	#endregion // RolePathOwner class
	#region RoleProjectedDerivationRule class
	partial class RoleProjectedDerivationRule : IModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessor Properties
		/// <summary>
		/// Property used to block projection errors if an external derivation is defined.
		/// </summary>
		protected virtual bool HasExternalDerivation
		{
			get
			{
				return false;
			}
		}
		#endregion // Accessor Properties
		#region Base overrides
		/// <summary>
		/// Add role projections to the set of base elements than can consume a <see cref="CalculatedPathValue"/>
		/// </summary>
		protected override bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return base.IsCalculatedPathValueConsumed(calculation) || DerivedRoleProjectedFromCalculatedPathValue.GetDerivedRoleProjections(calculation).Count != 0;
		}
		/// <summary>
		/// Register validation for a new <see cref="RoleProjectedDerivationRule"/>
		/// </summary>
		protected override void NewlyCreated()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateProjectionExistence);
		}
		/// <summary>
		/// Check derivation projections
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback.</param>
		protected override void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			ValidateAutomaticProjections(notifyAdded);
			if (notifyAdded != null)
			{
				// We do all projection analysis independently after the deserialization request
				ValidateProjectionExistence(notifyAdded);
			}
			ValidateProjectionTypes(notifyAdded);
		}
		/// <summary>
		/// Helper struct for ValidateAutomaticProjections
		/// </summary>
		private struct AutoProjectedRoleInfo
		{
			public readonly Role Role;
			public readonly ObjectType RolePlayer;
			public DerivedRoleProjection RoleProjection;
			public RolePathNode MatchingPathNode;
			public bool HasMultipleMatches;
			public bool ExplicitProjection;
			public AutoProjectedRoleInfo(Role role, ObjectType rolePlayer)
			{
				Role = role;
				RolePlayer = rolePlayer;
				MatchingPathNode = RolePathNode.Empty;
				HasMultipleMatches = false;
				RoleProjection = null;
				ExplicitProjection = false;
			}
			/// <summary>
			/// Record if a path node matches any of the data items.
			/// </summary>
			/// <param name="data">Data previously initialized with the <see cref="Reset"/> method.</param>
			/// <param name="pathNode">The node to test for type matches.</param>
			/// <returns>The number of nodes eliminated from automatic projection consideration. When the
			/// cumulative total of return values from this method matches the return value from the Reset
			/// method, all automatic projection possibilities have been eliminated.</returns>
			public static int RecordMatches(AutoProjectedRoleInfo[] data, RolePathNode pathNode)
			{
				ObjectType startingType;
				int eliminatedItems = 0;
				if (null != (startingType = pathNode.ObjectType))
				{
					ObjectType.WalkSupertypes(
						startingType,
						delegate(ObjectType type, int depth, bool isPrimary)
						{
							for (int i = 0; i < data.Length; ++i)
							{
								AutoProjectedRoleInfo info = data[i];
								if (!info.ExplicitProjection &&
									!info.HasMultipleMatches &&
									info.RolePlayer == type)
								{
									RolePathNode previousNode = info.MatchingPathNode;
									bool previousNodeIsEmpty = previousNode.IsEmpty;
									PathObjectUnifier unifier;
									if (previousNodeIsEmpty)
									{
										// Note that we're walking from the top down, so
										// this will automatically get the first correlated
										// object.
										info.MatchingPathNode = pathNode;
									}
									else if (null == (unifier = previousNode.ObjectUnifier) ||
										unifier != pathNode.ObjectUnifier)
									{
										++eliminatedItems;
										info.HasMultipleMatches = true;
									}
									else
									{
										continue;
									}
									data[i] = info;
								}
							}
							return ObjectTypeVisitorResult.Continue;
						});
				}
				return eliminatedItems;
			}
			/// <summary>
			/// Reset projected role information for a projection set before iterating the role path.
			/// </summary>
			/// <returns>Number of possible automatic projections.</returns>
			public static int Reset(AutoProjectedRoleInfo[] data, RoleSetDerivationProjection projectionSet)
			{
				int retVal = 0;
				for (int i = 0; i < data.Length; ++i)
				{
					AutoProjectedRoleInfo info = data[i];
					DerivedRoleProjection projection;
					if (projectionSet != null &&
						null != (projection = DerivedRoleProjection.GetLink(projectionSet, info.Role)))
					{
						info.RoleProjection = projection;
						bool canBeAuto = projection.IsAutomatic;
						info.ExplicitProjection = !canBeAuto;
						if (canBeAuto)
						{
							++retVal;
						}
					}
					else
					{
						info.RoleProjection = null;
						info.ExplicitProjection = false;
						++retVal;
					}
					info.MatchingPathNode = RolePathNode.Empty;
					info.HasMultipleMatches = false;
					data[i] = info;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Populate automatic projections if there is exactly one source node
		/// that matches the possible role projection.
		/// </summary>
		private void ValidateAutomaticProjections(INotifyElementAdded notifyAdded)
		{
			// The algorithm here should mirror LeadRolePath.FindSingleNodeOfType
			int factTypeRoleCount = 0;
			AutoProjectedRoleInfo[] roleInfo = null;
			foreach (LeadRolePath leadRolePath in LeadRolePathCollection)
			{
				if (factTypeRoleCount == 0)
				{
					LinkedElementCollection<RoleBase> factTypeRoles = FactType.RoleCollection;
					int? unaryRoleIndex;
					factTypeRoleCount = factTypeRoles.Count;
					Role role;
					if (factTypeRoleCount == 2 &&
						(unaryRoleIndex = FactType.GetUnaryRoleIndex(factTypeRoles)).HasValue)
					{
						factTypeRoleCount = 1;
						role = factTypeRoles[unaryRoleIndex.Value].Role;
						roleInfo = new AutoProjectedRoleInfo[] { new AutoProjectedRoleInfo(role, role.RolePlayer) };
					}
					else
					{
						roleInfo = new AutoProjectedRoleInfo[factTypeRoleCount];
						for (int i = 0; i < factTypeRoleCount; ++i)
						{
							role = factTypeRoles[i].Role;
							roleInfo[i] =  new AutoProjectedRoleInfo(role, role.RolePlayer);
						}
					}
				}
				RoleSetDerivationProjection projectionSet = RoleSetDerivationProjection.GetLink(this, leadRolePath);
				int possibleAutoCount = AutoProjectedRoleInfo.Reset(roleInfo, projectionSet);
				if (possibleAutoCount == 0)
				{
					continue;
				}
				RolePathNode.VisitPathNodes(
					leadRolePath,
					RolePathNode.Empty,
					false,
					delegate(RolePathNode pathNode, RolePathNode previousPathNode, bool unwinding)
					{
						PathedRole pathedRole;
						if (null != (pathedRole = pathNode.PathedRole) &&
							pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
						{
							return true;
						}
						possibleAutoCount -= AutoProjectedRoleInfo.RecordMatches(roleInfo, pathNode);
						return possibleAutoCount != 0;
					});
				Store store = this.Store;
				bool setChangingFlag = notifyAdded != null; // Flag not needed if rules not on, pretend it is set in this case
				try
				{
					for (int i = 0; i < factTypeRoleCount; ++i)
					{
						AutoProjectedRoleInfo info = roleInfo[i];
						if (!info.ExplicitProjection)
						{
							RolePathNode autoNode = info.MatchingPathNode;
							DerivedRoleProjection projection = info.RoleProjection;
							PathedRole autoPathedRole;
							RolePathObjectTypeRoot autoPathRoot;
							ModelElement notifyLink;
							if (info.HasMultipleMatches || autoNode.IsEmpty)
							{
								if (projection != null)
								{
									// This is run before ValidateProjectionExistence, which will clean up empty
									// sets and add associated errors, so we don't have to do it here.
									projection.Delete();
								}
							}
							else
							{
								if (projection != null)
								{
									PathedRole existingPathedRole;
									RolePathObjectTypeRoot existingPathRoot;
									if (null != (existingPathedRole = projection.ProjectedFromPathedRole))
									{
										if (null != (autoPathedRole = autoNode.PathedRole))
										{
											if (autoPathedRole == existingPathedRole)
											{
												continue;
											}
											if (!setChangingFlag)
											{
												DerivedRoleProjection.ChangingAutomaticProjection(store, true);
												setChangingFlag = true;
											}
											projection.ProjectedFromPathedRole = autoPathedRole;
										}
										else
										{
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(new DerivedRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot), false);
												projection.ProjectedFromPathedRole = null;
											}
											else
											{
												if (!setChangingFlag)
												{
													DerivedRoleProjection.ChangingAutomaticProjection(store, true);
													setChangingFlag = true;
												}
												// Rules will clear out the other element
												projection.ProjectedFromPathRoot = autoNode.PathRoot;
											}
										}
									}
									else if (null != (existingPathRoot = projection.ProjectedFromPathRoot))
									{
										if (null != (autoPathRoot = autoNode.PathRoot))
										{
											if (autoPathRoot == existingPathRoot)
											{
												continue;
											}
											if (!setChangingFlag)
											{
												DerivedRoleProjection.ChangingAutomaticProjection(store, true);
												setChangingFlag = true;
											}
											projection.ProjectedFromPathRoot = autoPathRoot;
										}
										else
										{
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(new DerivedRoleProjectedFromPathedRole(projection, autoNode.PathedRole), false);
												projection.ProjectedFromPathRoot = null;
											}
											else
											{
												if (!setChangingFlag)
												{
													DerivedRoleProjection.ChangingAutomaticProjection(store, true);
													setChangingFlag = true;
												}
												// Rules will clear out the other element
												projection.ProjectedFromPathedRole = autoNode.PathedRole;
											}
										}
									}
									else
									{
										// Invalid state that we shouldn't hit during normal process (we don't auto project constants or calculations),
										// but can happen on load.
										if (!setChangingFlag)
										{
											DerivedRoleProjection.ChangingAutomaticProjection(store, true);
											setChangingFlag = true;
										}
										notifyLink = (null != (autoPathedRole = autoNode.PathedRole)) ?
											(ModelElement)new DerivedRoleProjectedFromPathedRole(projection, autoPathedRole) :
											new DerivedRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(notifyLink, false);
											projection.ProjectedFromConstant = null;
											projection.ProjectedFromCalculatedValue = null;
										}
									}
								}
								else
								{
									if (projectionSet == null)
									{
										projectionSet = new RoleSetDerivationProjection(this, leadRolePath);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(projectionSet, false);
										}
									}
									projection = new DerivedRoleProjection(Partition,
										new RoleAssignment[]{
											new RoleAssignment(DerivedRoleProjection.DerivationProjectionDomainRoleId, projectionSet),
											new RoleAssignment(DerivedRoleProjection.ProjectedRoleDomainRoleId, info.Role)},
										new PropertyAssignment[]{
											new PropertyAssignment(DerivedRoleProjection.IsAutomaticDomainPropertyId, true)});
									if (!setChangingFlag)
									{
										DerivedRoleProjection.ChangingAutomaticProjection(store, true);
										setChangingFlag = true;
									}
									notifyLink = (null != (autoPathedRole = autoNode.PathedRole)) ?
										(ModelElement)new DerivedRoleProjectedFromPathedRole(projection, autoPathedRole) :
										new DerivedRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(projection, false);
										notifyAdded.ElementAdded(notifyLink, false);
									}
								}
							}
						}
					}
				}
				finally
				{
					if (setChangingFlag && notifyAdded == null)
					{
						DerivedRoleProjection.ChangingAutomaticProjection(store, false);
					}
				}
			}
		}
		private void ValidateProjectionTypes(INotifyElementAdded notifyAdded)
		{
			ORMModel contextModel = null;
			foreach (RoleSetDerivationProjection projectionSet in RoleSetDerivationProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				foreach (DerivedRoleProjection projection in DerivedRoleProjection.GetLinksToProjectedRoleCollection(projectionSet))
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					bool hasError = false;
					if (null != (pathedRole = projection.ProjectedFromPathedRole))
					{
						hasError = !new RolePathNode(pathedRole).CanProjectOn(projection.ProjectedRole.RolePlayer);
					}
					else if (null != (pathRoot = projection.ProjectedFromPathRoot))
					{
						hasError = !new RolePathNode(pathRoot).CanProjectOn(projection.ProjectedRole.RolePlayer);
					}
					DerivedRoleRequiresCompatibleProjectionError error = projection.IncompatibleProjectionError;
					if (hasError)
					{
						if (error == null &&
							null != (contextModel ?? (contextModel = this.Model)))
						{
							error = new DerivedRoleRequiresCompatibleProjectionError(Partition);
							error.Projection = projection;
							error.Model = contextModel;
							error.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(error, true);
							}
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
			}
		}
		/// <summary>
		/// Verify all projection errors
		/// </summary>
		/// <param name="element">A <see cref="RoleProjectedDerivationRule"/></param>
		protected static void DelayValidateProjectionExistence(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((RoleProjectedDerivationRule)element).ValidateProjectionExistence(null);
			}
		}
		/// <summary>
		/// Validate projections for existence and completeness
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback. Perform extra
		/// structural checks if this is set.</param>
		private void ValidateProjectionExistence(INotifyElementAdded notifyAdded)
		{
			bool seenProjection = false;
			int factTypeRoleCount = 0;
			Partition partition = Partition;
			ORMModel model = null;
			foreach (RoleSetDerivationProjection projection in RoleSetDerivationProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				if (factTypeRoleCount == 0)
				{
					LinkedElementCollection<RoleBase> factTypeRoles = FactType.RoleCollection;
					factTypeRoleCount = factTypeRoles.Count;
					if (factTypeRoleCount == 2 &&
						FactType.GetUnaryRoleIndex(factTypeRoles).HasValue)
					{
						factTypeRoleCount = 1;
					}
				}
				int projectedRoleCount;
				if (notifyAdded != null)
				{
					// Clean up projections with no source information on load. Empty
					// projections are automatically removed after deserialization, so
					// verify that we have a consistent state.
					ReadOnlyCollection<DerivedRoleProjection> projectionLinks = DerivedRoleProjection.GetLinksToProjectedRoleCollection(projection);
					projectedRoleCount = projectionLinks.Count;
					foreach (DerivedRoleProjection roleProjection in projectionLinks)
					{
						if (null == roleProjection.ProjectedFromPathedRole &&
							null == roleProjection.ProjectedFromPathRoot &&
							null == roleProjection.ProjectedFromCalculatedValue &&
							null == roleProjection.ProjectedFromConstant)
						{
							--projectedRoleCount;
							roleProjection.Delete();
						}
					}
				}
				else
				{
					projectedRoleCount = projection.ProjectedRoleCollection.Count;
				}
				if (projectedRoleCount == 0)
				{
					projection.Delete();
					continue;
				}
				seenProjection = true;
				PartialRoleSetDerivationProjectionError partialProjectionError = projection.PartialProjectionError;
				if (projectedRoleCount < factTypeRoleCount)
				{
					if (partialProjectionError == null)
					{
						partialProjectionError = new PartialRoleSetDerivationProjectionError(partition);
						partialProjectionError.DerivationProjection = projection;
						partialProjectionError.Model = model ?? (model = Model);
						partialProjectionError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(partialProjectionError, true);
						}
					}
				}
				else if (partialProjectionError != null)
				{
					partialProjectionError.Delete();
				}
			}
			RoleProjectedDerivationRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
			if (!seenProjection && !HasExternalDerivation)
			{
				if (projectionRequiredError == null)
				{
					projectionRequiredError = new RoleProjectedDerivationRequiresProjectionError(partition);
					projectionRequiredError.DerivationRule = this;
					projectionRequiredError.Model = model ?? (model = Model);
					projectionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(projectionRequiredError, true);
					}
				}
			}
			else if (projectionRequiredError != null)
			{
				projectionRequiredError.Delete();
			}
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				RoleProjectedDerivationRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
				if (projectionRequiredError != null)
				{
					yield return projectionRequiredError;
				}
				else
				{
					foreach (RoleSetDerivationProjection projectionSet in RoleSetDerivationProjection.GetLinksToProjectedPathComponentCollection(this))
					{
						foreach (DerivedRoleProjection projection in DerivedRoleProjection.GetLinksToProjectedRoleCollection(projectionSet))
						{
							DerivedRoleRequiresCompatibleProjectionError compatibilityError = projection.IncompatibleProjectionError;
							if (compatibilityError != null)
							{
								yield return compatibilityError;
							}
						}
						PartialRoleSetDerivationProjectionError partialProjectionError = projectionSet.PartialProjectionError;
						if (partialProjectionError != null)
						{
							yield return partialProjectionError;
						}
					}
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				IModelErrorDisplayContext context = (IModelErrorDisplayContext)FactType;
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextRoleProjectedDerivationRule, context != null ? (context.ErrorDisplayContext ?? "") : "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region Validation Rule Methods
		/// <summary>
		/// Attach rule notifications for data type changes
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public new static void EnableRuleNotifications(Store store)
		{
			RolePathOwner.AddRolePathDataTypeChangeRuleNotification(store, ProcessDataTypeChange);
		}
		private static void ProcessDataTypeChange(ObjectType valueType, IList<Role> playedRoles, RolePathNode processNode)
		{
			PathedRole pathedRole;
			if (processNode.IsEmpty)
			{
				foreach (Role playedRole in playedRoles)
				{
					FactType factType;
					RoleProjectedDerivationRule derivationRule;
					if (null != (factType = playedRole.FactType) &&
						null != (derivationRule = factType.DerivationRule))
					{
						FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateDerivedRolePathOwner);
					}
				}
			}
			else if (null != (pathedRole = processNode.PathedRole))
			{
				foreach (DerivedRoleProjection projection in DerivedRoleProjectedFromPathedRole.GetDerivedRoleProjections(pathedRole))
				{
					FrameworkDomainModel.DelayValidateElement(projection.DerivationProjection.DerivationRule, DelayValidateDerivedRolePathOwner);
				}
			}
			else
			{
				foreach (DerivedRoleProjection projection in DerivedRoleProjectedFromRolePathRoot.GetDerivedRoleProjections(processNode.PathRoot))
				{
					FrameworkDomainModel.DelayValidateElement(projection.DerivationProjection.DerivationRule, DelayValidateDerivedRolePathOwner);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void DerivedRoleProjectionOnCalculatedPathValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedValueUseChangedInRule(((DerivedRoleProjectedFromCalculatedPathValue)e.ModelElement).Source);
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void DerivedRoleProjectionOnCalculatedPathValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((DerivedRoleProjectedFromCalculatedPathValue)e.ModelElement).Source;
			if (!calculation.IsDeleted)
			{
				CalculatedValueUseChangedInRule(calculation);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(DerivedRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void DerivedRoleProjectionOnCalculatedPathValueRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == DerivedRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.OldRolePlayer);
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void FactTypeRoleAddedRule(ElementAddedEventArgs e)
		{
			RoleProjectedDerivationRule derivationRule;
			if (null != (derivationRule = ((FactTypeHasRole)e.ModelElement).FactType.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void FactTypeRoleDeletedRule(ElementDeletedEventArgs e)
		{
			FactType factType = ((FactTypeHasRole)e.ModelElement).FactType;
			RoleProjectedDerivationRule derivationRule;
			if (!factType.IsDeleted &&
				null != (derivationRule = factType.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			ValidateAttachedProjections(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			ValidateAttachedProjections(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ElementLink;
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				ValidateAttachedProjections(link.PlayedRole);
			}
			else
			{
				ValidateAttachedProjections((Role)e.OldRolePlayer);
				ValidateAttachedProjections((Role)e.NewRolePlayer);
			}
		}
		private static void ValidateAttachedProjections(Role role)
		{
			if (role != null &&
				!role.IsDeleted &&
				!role.IsDeleting)
			{
				foreach (DerivedRoleProjection roleProjection in DerivedRoleProjection.GetLinksToDerivationProjectionCollection(role))
				{
					RoleProjectedDerivationRule derivationRule;
					if (null != (derivationRule = roleProjection.DerivationProjection.DerivationRule))
					{
						FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateDerivedRolePathOwner);
					}
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(PathObjectUnifierUnifiesPathedRole)
		/// Preserve projection with other previously unified elements if
		/// a unified pathed role is deleted.
		/// </summary>
		private static void PathedRoleUnificationDeletingRule(ElementDeletingEventArgs e)
		{
			PathObjectUnifierUnifiesPathedRole link = (PathObjectUnifierUnifiesPathedRole)e.ModelElement;
			ReadOnlyCollection<DerivedRoleProjectedFromPathedRole> roleProjections = DerivedRoleProjectedFromPathedRole.GetLinksToDerivedRoleProjections(link.PathedRole);
			if (roleProjections.Count != 0)
			{
				PathObjectUnifier objectUnifier = link.ObjectUnifier;
				foreach (PathObjectUnifierUnifiesRolePathRoot unifiedRootLink in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(objectUnifier))
				{
					if (!unifiedRootLink.IsDeleting)
					{
						RolePathObjectTypeRoot replaceWithPathRoot = unifiedRootLink.PathRoot;
						foreach (DerivedRoleProjectedFromPathedRole roleProjection in roleProjections)
						{
							if (!roleProjection.IsDeleting)
							{
								roleProjection.RoleProjection.ProjectedFromPathRoot = replaceWithPathRoot;
							}
						}
						return;
					}
				}
				foreach (PathObjectUnifierUnifiesPathedRole unifiedPathedRoleLink in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(objectUnifier))
				{
					if (!unifiedPathedRoleLink.IsDeleting)
					{
						PathedRole replaceWithPathedRole = unifiedPathedRoleLink.PathedRole;
						foreach (DerivedRoleProjectedFromPathedRole roleProjection in roleProjections)
						{
							if (!roleProjection.IsDeleting)
							{
								roleProjection.RoleProjection.ProjectedFromPathedRole = replaceWithPathedRole;
							}
						}
						return;
					}
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(PathObjectUnifierUnifiesRolePathRoot)
		/// Preserve projection with other previously unified elements if
		/// a unified path root is deleted.
		/// </summary>
		private static void PathRootUnificationDeletingRule(ElementDeletingEventArgs e)
		{
			PathObjectUnifierUnifiesRolePathRoot link = (PathObjectUnifierUnifiesRolePathRoot)e.ModelElement;
			ReadOnlyCollection<DerivedRoleProjectedFromRolePathRoot> roleProjections = DerivedRoleProjectedFromRolePathRoot.GetLinksToDerivedRoleProjections(link.PathRoot);
			if (roleProjections.Count != 0)
			{
				PathObjectUnifier objectUnifier = link.ObjectUnifier;
				foreach (PathObjectUnifierUnifiesRolePathRoot unifiedRootLink in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(objectUnifier))
				{
					if (!unifiedRootLink.IsDeleting)
					{
						RolePathObjectTypeRoot replaceWithPathRoot = unifiedRootLink.PathRoot;
						foreach (DerivedRoleProjectedFromRolePathRoot roleProjection in roleProjections)
						{
							if (!roleProjection.IsDeleting)
							{
								roleProjection.RoleProjection.ProjectedFromPathRoot = replaceWithPathRoot;
							}
						}
						return;
					}
				}
				foreach (PathObjectUnifierUnifiesPathedRole unifiedPathedRoleLink in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(objectUnifier))
				{
					if (!unifiedPathedRoleLink.IsDeleting)
					{
						PathedRole replaceWithPathedRole = unifiedPathedRoleLink.PathedRole;
						foreach (DerivedRoleProjectedFromRolePathRoot roleProjection in roleProjections)
						{
							if (!roleProjection.IsDeleting)
							{
								roleProjection.RoleProjection.ProjectedFromPathedRole = replaceWithPathedRole;
							}
						}
						return;
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RoleSetDerivationProjection)
		/// </summary>
		private static void ProjectionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((RoleSetDerivationProjection)e.ModelElement).DerivationRule, DelayValidateProjectionExistence);
		}
		/// <summary>
		/// DeleteRule: typeof(RoleSetDerivationProjection)
		/// </summary>
		private static void ProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			RoleProjectedDerivationRule derivationRule = ((RoleSetDerivationProjection)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RoleSetDerivationProjection)
		/// </summary>
		private static void ProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RoleSetDerivationProjection.DerivationRuleDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateProjectionExistence);
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateProjectionExistence);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(((RoleSetDerivationProjection)e.ElementLink).DerivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void LeadRolePathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			DeleteProjectionForDetachedPath(link.PathOwner, link.RolePath);
		}
		/// <summary>
		/// If a role path has been detached from the derivation rule but not deleted, then clear
		/// the associated projections and parameter bindings if the path has not been reattached
		/// through a different (owns vs sharing) relationship.
		/// </summary>
		private static void DeleteProjectionForDetachedPath(RolePathOwner owner, LeadRolePath rolePath)
		{
			RoleProjectedDerivationRule derivationRule;
			if (!rolePath.IsDeleted &&
				null != (derivationRule = owner as RoleProjectedDerivationRule) &&
				!derivationRule.IsDeleted &&
				null == RolePathOwnerHasLeadRolePath.GetLink(derivationRule, rolePath))
			{
				RoleSetDerivationProjection projection;
				if (null != (projection = RoleSetDerivationProjection.GetLink(derivationRule, rolePath)))
				{
					projection.Delete();
				}
				foreach (QueryParameterBinding parameterBinding in QueryParameterBinding.GetLinksToParameterBindings(rolePath))
				{
					QueryBase query;
					QueryDerivationRule queryRule;
					if (null == (query = parameterBinding.QueryParameter.Query) ||
						null == (queryRule = query.DerivationRule as QueryDerivationRule) ||
						queryRule == owner)
					{
						parameterBinding.Delete();
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when paths are added to the derivation rule and
		/// eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void LeadRolePathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathOwnerHasLeadRolePath.PathOwnerDomainRoleId)
			{
				DeleteProjectionForDetachedPath((RolePathOwner)e.OldRolePlayer, ((RolePathOwnerHasLeadRolePath)e.ElementLink).RolePath);
			}
			else
			{
				DeleteProjectionForDetachedPath(((RolePathOwnerHasLeadRolePath)e.ElementLink).PathOwner, (LeadRolePath)e.OldRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjection)
		/// </summary>
		private static void RoleProjectionAddedRule(ElementAddedEventArgs e)
		{
			RoleProjectedDerivationRule derivationRule = ((DerivedRoleProjection)e.ModelElement).DerivationProjection.DerivationRule;
			if (derivationRule != null)
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjection)
		/// </summary>
		private static void RoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			RoleSetDerivationProjection projection = ((DerivedRoleProjection)e.ModelElement).DerivationProjection;
			RoleProjectedDerivationRule derivationRule;
			if (!projection.IsDeleted &&
				null != (derivationRule = projection.DerivationRule))
			{
				FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(DerivedRoleProjection)
		/// </summary>
		private static void RoleProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == DerivedRoleProjection.DerivationProjectionDomainRoleId)
			{
				RoleProjectedDerivationRule derivationRule = ((RoleSetDerivationProjection)e.OldRolePlayer).DerivationRule;
				if (derivationRule != null)
				{
					FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
				}
				derivationRule = ((RoleSetDerivationProjection)e.NewRolePlayer).DerivationRule;
				if (derivationRule != null)
				{
					FrameworkDomainModel.DelayValidateElement(derivationRule, DelayValidateProjectionExistence);
				}
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // RoleProjectedDerivationRule class
	#region FactTypeDerivationRule class
	partial class FactTypeDerivationRule : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="FactType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				FactType factType = FactType;
				return factType != null ? factType.ResolvedModel : null;
			}
		}
		/// <summary>
		/// Block projection validation if the derivation rules is external
		/// </summary>
		protected override bool HasExternalDerivation
		{
			get
			{
				return ExternalDerivation;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasDerivationRule.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// DeleteRule: typeof(FactTypeDerivationRuleHasDerivationNote), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void DerivationNoteDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeDerivationRule derivationRule = ((FactTypeDerivationRuleHasDerivationNote)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted &&
				derivationRule.ExternalDerivation &&
				derivationRule.LeadRolePathCollection.Count == 0 &&
				derivationRule.SubqueryCollection.Count == 0)
			{
				derivationRule.Delete();
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationRule)
		/// </summary>
		private static void FactTypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ExternalDerivationDomainPropertyId)
			{
				// Projection errors will come and go based on this setting
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when paths are added to the derivation rule.
		/// </summary>
		private static void LeadRolePathAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			FactTypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as FactTypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when paths are added to the derivation rule and
		/// eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void LeadRolePathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathOwnerHasLeadRolePath.PathOwnerDomainRoleId)
			{
				FactTypeDerivationRule derivationRule;
				if (null != (derivationRule = e.NewRolePlayer as FactTypeDerivationRule) &&
					!derivationRule.IsDeleted)
				{
					derivationRule.ExternalDerivation = false;
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasSubquery), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when subqueries are added to the derivation rule.
		/// </summary>
		private static void SubqueryAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasSubquery link = (RolePathOwnerHasSubquery)e.ModelElement;
			FactTypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as FactTypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasSubquery), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when subqueries are added to the derivation rule and
		/// eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void SubqueryRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathOwnerHasSubquery.PathOwnerDomainRoleId)
			{
				FactTypeDerivationRule derivationRule;
				if (null != (derivationRule = e.NewRolePlayer as FactTypeDerivationRule) &&
					!derivationRule.IsDeleted)
				{
					derivationRule.ExternalDerivation = false;
				}
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // FactTypeDerivationRule class
	#region QueryBase class
	partial class QueryBase
	{
		/// <summary>
		/// Each query type has a different path to the <see cref="ORMModel"/>
		/// than the base <see cref="FactType"/>. Replace the Model property.
		/// </summary>
		public override abstract ORMModel Model
		{
			get;
			set;
		}
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(QueryBase)
		/// Queries are always derived. Make sure a query always has an associated QueryDerivationRule
		/// </summary>
		private static void QueryAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateQueryHasValidationRule);
		}
		/// <summary>
		/// Ensure that a newly created query has an associated validation rule.
		/// </summary>
		[DelayValidatePriority(-1)]
		private static void DelayValidateQueryHasValidationRule(ModelElement element)
		{
			QueryBase query = (QueryBase)element;
			if (!query.IsDeleted && null == query.DerivationRule)
			{
				query.DerivationRule = new QueryDerivationRule(query.Partition);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasDerivationRule), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a query when its derivation rule is deleted.
		/// </summary>
		private static void QueryDerivationRuleDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeHasDerivationRule link = (FactTypeHasDerivationRule)e.ModelElement;
			QueryBase query;
			if (null != (query = link.FactType as QueryBase) &&
				!query.IsDeleted &&
				query.DerivationRule == null)
			{
				// Note that the opposite direction (deleting the derivation rule with the query)
				// is already covered by delete propagation, but the reverse delete propagation
				// applies only to queries, not general fact types.
				query.Delete();
			}
		}
		#endregion // Rule Methods
	}
	#endregion // QueryBase class
	#region Subquery class
	partial class Subquery : IModelErrorDisplayContext, IHasIndirectModelErrorOwner
	{
		#region Base Overrides
		/// <summary>
		/// Retrieve the <see cref="ORMModel"/> through path owner.
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				RolePathOwner owner = this.PathOwner;
				return owner != null ? owner.Model : null;
			}
			set
			{
				Debug.Fail("Subquery model set indirectly");
			}
		}
		#endregion // Base Overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected new string ErrorDisplayContext
		{
			get
			{
				IModelErrorDisplayContext ownerContext = this.PathOwner as IModelErrorDisplayContext;
				// Name for a subquery returns the signature
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSubquery, Name, ownerContext != null ? ownerContext.ErrorDisplayContext : "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected new static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathOwnerOwnsSubquery.SubqueryDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // Subquery class
	#region SubqueryDerivationRule class
	partial class QueryDerivationRule : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="Subquery"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				QueryBase query;
				return null != (query = this.FactType as QueryBase) ? query.ResolvedModel : null;
			}
		}
		/// <summary>
		/// Block path sharing for subquery derivation paths
		/// </summary>
		public override bool AllowOwnedPathSharing
		{
			get
			{
				// We could reconsider this in the future, but a shared subquery path generally
				// indicates that the construct would be better defined as a reusable derived
				// fact type. The ownership here can also be recursively defined, which complicates
				// ownership transfer. For now, do not allow these paths to be shared.
				return false;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasDerivationRule.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // SubqueryDerivationRule class
	#region RoleSetDerivationProjection class
	partial class RoleSetDerivationProjection : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RoleSetDerivationProjection.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the projection level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = DerivationRule as IModelErrorDisplayContext;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // RoleSetDerivationProjection class
	#region DerivedRoleProjection class
	partial class DerivedRoleProjection : IElementLinkRoleHasIndirectModelErrorOwner
	{
		#region Custom Storage Handlers
		private bool myIsAutomatic;
		private bool GetIsAutomaticValue()
		{
			return myIsAutomatic;
		}
		private void SetIsAutomaticValue(bool value)
		{
			if (value &&
				!Store.InUndoRedoOrRollback)
			{
				PathedRole currentPathedRole;
				RolePathObjectTypeRoot currentPathRoot = null;
				// If all values are null, then this is newly constructed. Do not
				// attempt to validate an incoming value. The incoming value is coming
				// from initialization code, not the user.
				if (!(null == (currentPathedRole = ProjectedFromPathedRole) &&
					null == (currentPathRoot = ProjectedFromPathRoot) &&
					null == ProjectedFromCalculatedValue &&
					null == ProjectedFromConstant))
				{
					RolePathNode singleNode = DerivationProjection.RolePath.FindSingleNodeOfType(ProjectedRole.RolePlayer);
					if (singleNode.IsEmpty)
					{
						throw new InvalidOperationException(ResourceStrings.ModelErrorDerivedRoleProjectionProjectionAutomaticProjectionUnavailable);
					}
					else
					{
						// Note that we do not need to set ChangingAutomaticProjection here because
						// the getter for this property will still return false, so no attempt will
						// be made to reset it.
						PathedRole newPathedRole;
						if (null != (newPathedRole = singleNode.PathedRole))
						{
							if (newPathedRole != currentPathedRole)
							{
								ProjectedFromPathedRole = newPathedRole;
							}
						}
						else
						{
							RolePathObjectTypeRoot newPathRoot = singleNode.PathRoot;
							if (newPathRoot != currentPathRoot)
							{
								ProjectedFromPathRoot = newPathRoot;
							}
						}
					}
				}
			}
			myIsAutomatic = value;
		}
		#endregion // Custom Storage Handlers
		#region Role derivation validation rules
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjection), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a fact type derivation projection if there are no more contained role projections.
		/// </summary>
		private static void DerivedRoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			RoleSetDerivationProjection projection = ((DerivedRoleProjection)e.ModelElement).DerivationProjection;
			if (!projection.IsDeleted &&
				projection.ProjectedRoleCollection.Count == 0)
			{
				projection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ProjectedFromCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			DerivedRoleProjection roleProjection = ((DerivedRoleProjectedFromCalculatedPathValue)e.ModelElement).RoleProjection;
			roleProjection.IsAutomatic = false;
			roleProjection.ProjectedFromConstant = null;
			roleProjection.ProjectedFromPathRoot = null;
			roleProjection.ProjectedFromPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromCalculatedPathValue), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((DerivedRoleProjectedFromCalculatedPathValue)e.ModelElement).RoleProjection);
		}
		private static void DeleteIfEmpty(DerivedRoleProjection derivedRoleProjection)
		{
			if (!derivedRoleProjection.IsDeleted &&
				null == derivedRoleProjection.ProjectedFromPathedRole &&
				null == derivedRoleProjection.ProjectedFromPathRoot &&
				null == derivedRoleProjection.ProjectedFromCalculatedValue &&
				null == derivedRoleProjection.ProjectedFromConstant)
			{
				derivedRoleProjection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjectedFromPathConstant)
		/// </summary>
		private static void ProjectedFromConstantAddedRule(ElementAddedEventArgs e)
		{
			DerivedRoleProjection roleProjection = ((DerivedRoleProjectedFromPathConstant)e.ModelElement).RoleProjection;
			roleProjection.IsAutomatic = false;
			roleProjection.ProjectedFromPathRoot = null;
			roleProjection.ProjectedFromPathedRole = null;
			roleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromPathConstant), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromConstantDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((DerivedRoleProjectedFromPathConstant)e.ModelElement).RoleProjection);
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjectedFromPathedRole)
		/// </summary>
		private static void ProjectedFromPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			DerivedRoleProjection roleProjection = ((DerivedRoleProjectedFromPathedRole)e.ModelElement).RoleProjection;
			DerivedRoleProjectedFromRolePathRoot rootLink = DerivedRoleProjectedFromRolePathRoot.GetLinkToProjectedFromPathRoot(roleProjection);
			if (rootLink != null)
			{
				if (roleProjection.IsAutomatic && !ChangingAutomaticProjection(rootLink.Store, null))
				{
					roleProjection.IsAutomatic = false;
				}
				rootLink.Delete();
			}
			roleProjection.ProjectedFromConstant = null;
			roleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(DerivedRoleProjectedFromPathedRole)
		/// </summary>
		private static void ProjectedFromPathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == DerivedRoleProjectedFromPathedRole.SourceDomainRoleId)
			{
				DerivedRoleProjection projection = ((DerivedRoleProjectedFromPathedRole)e.ElementLink).RoleProjection;
				if (projection.IsAutomatic && !ChangingAutomaticProjection(projection.Store, null))
				{
					projection.IsAutomatic = false;
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((DerivedRoleProjectedFromPathedRole)e.ModelElement).RoleProjection);
		}
		/// <summary>
		/// AddRule: typeof(DerivedRoleProjectedFromRolePathRoot)
		/// </summary>
		private static void ProjectedFromPathRootAddedRule(ElementAddedEventArgs e)
		{
			DerivedRoleProjection roleProjection = ((DerivedRoleProjectedFromRolePathRoot)e.ModelElement).RoleProjection;
			DerivedRoleProjectedFromPathedRole pathedRoleLink = DerivedRoleProjectedFromPathedRole.GetLinkToProjectedFromPathedRole(roleProjection);
			if (pathedRoleLink != null)
			{
				if (roleProjection.IsAutomatic && !ChangingAutomaticProjection(pathedRoleLink.Store, null))
				{
					roleProjection.IsAutomatic = false;
				}
				pathedRoleLink.Delete();
			}
			roleProjection.ProjectedFromConstant = null;
			roleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromRolePathRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ProjectedFromPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((DerivedRoleProjectedFromRolePathRoot)e.ModelElement).RoleProjection);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(DerivedRoleProjectedFromRolePathRoot)
		/// </summary>
		private static void ProjectedFromPathRootRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == DerivedRoleProjectedFromRolePathRoot.SourceDomainRoleId)
			{
				DerivedRoleProjection projection = ((DerivedRoleProjectedFromRolePathRoot)e.ElementLink).RoleProjection;
				if (projection.IsAutomatic && !ChangingAutomaticProjection(projection.Store, null))
				{
					projection.IsAutomatic = false;
				}
			}
		}
		/// <summary>
		/// Add a flag to the store that changes to existing automatic projections
		/// should not result in the <see cref="IsAutomatic"/> property being cleared.
		/// </summary>
		/// <param name="store">The Store object being modified.</param>
		/// <param name="value">The boolean value to set, or null to retrieve the current value.</param>
		public static bool ChangingAutomaticProjection(Store store, bool? value)
		{
			Transaction transaction;
			if (null != (transaction = store.TransactionManager.CurrentTransaction))
			{
				transaction = transaction.TopLevelTransaction;
				Dictionary<object, object> dict = transaction.Context.ContextInfo;
				object key = typeof(DerivedRoleProjection);
				object counterObj;
				bool dictHasValue;
				dictHasValue = dict.TryGetValue(key, out counterObj) && counterObj is int;
				if (value.HasValue)
				{
					if (value.Value)
					{
						dict[key] = dictHasValue ? ((int)counterObj) + 1 : 1;
					}
					else if (dictHasValue)
					{
						int counter = (int)counterObj;
						if (counter == 1)
						{
							dict.Remove(key);
						}
						else
						{
							dict[key] = counter + 1;
						}
					}
				}
				else
				{
					return dictHasValue;
				}
			}
			return false;
		}
		#endregion // Role derivation validation rules
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { DerivedRoleProjection.DerivationProjectionDomainRoleId, DerivedRoleProjection.ProjectedRoleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
	}
	#endregion // DerivedRoleProjection class
	#region QueryParameterBinding class
	partial class QueryParameterBinding
	{
		#region Binding validation rules
		/// <summary>
		/// AddRule: typeof(QueryParameterBoundToPathedRole)
		/// </summary>
		private static void BoundToPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			QueryParameterBinding binding = ((QueryParameterBoundToPathedRole)e.ModelElement).ParameterBinding;
			binding.BoundToPathRoot = null;
		}
		/// <summary>
		/// DeleteRule: typeof(QueryParameterBoundToPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void BoundToPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((QueryParameterBoundToPathedRole)e.ModelElement).ParameterBinding);
		}
		/// <summary>
		/// AddRule: typeof(QueryParameterBoundToRolePathRoot)
		/// </summary>
		private static void BoundToPathRootAddedRule(ElementAddedEventArgs e)
		{
			QueryParameterBinding binding = ((QueryParameterBoundToRolePathRoot)e.ModelElement).ParameterBinding;
			binding.BoundToPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(QueryParameterBoundToRolePathRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void BoundToPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((QueryParameterBoundToRolePathRoot)e.ModelElement).ParameterBinding);
		}
		private static void DeleteIfEmpty(QueryParameterBinding binding)
		{
			if (!binding.IsDeleted &&
				null == binding.BoundToPathedRole &&
				null == binding.BoundToPathRoot)
			{
				binding.Delete();
			}
		}
		#endregion // Binding validation rules
	}
	#endregion // QueryParameterBinding class
	#region SubqueryParameterInput class
	partial class SubqueryParameterInput
	{
		#region Parameter input validation rules
		/// <summary>
		/// DeleteRule: typeof(SubqueryParameterInput), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a fact type derivation projection if there are no more contained role projections.
		/// </summary>
		private static void InputDeletedRule(ElementDeletedEventArgs e)
		{
			SubqueryParameterInputs inputs = ((SubqueryParameterInput)e.ModelElement).Inputs;
			if (!inputs.IsDeleted &&
				inputs.InputCollection.Count == 0)
			{
				inputs.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(SubqueryParameterInputFromCalculatedPathValue)
		/// </summary>
		private static void InputFromCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			SubqueryParameterInput input = ((SubqueryParameterInputFromCalculatedPathValue)e.ModelElement).ParameterInput;
			input.InputFromConstant = null;
			input.InputFromPathRoot = null;
			input.InputFromPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(DerivedRoleProjectedFromCalculatedPathValue), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void InputFromCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((SubqueryParameterInputFromCalculatedPathValue)e.ModelElement).ParameterInput);
		}
		private static void DeleteIfEmpty(SubqueryParameterInput input)
		{
			if (!input.IsDeleted &&
				null == input.InputFromPathedRole &&
				null == input.InputFromPathRoot &&
				null == input.InputFromCalculatedValue &&
				null == input.InputFromConstant)
			{
				input.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(SubqueryParameterInputFromPathConstant)
		/// </summary>
		private static void InputFromConstantAddedRule(ElementAddedEventArgs e)
		{
			SubqueryParameterInput input = ((SubqueryParameterInputFromPathConstant)e.ModelElement).ParameterInput;
			input.InputFromPathRoot = null;
			input.InputFromPathedRole = null;
			input.InputFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(SubqueryParameterInputFromPathConstant), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void InputFromConstantDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((SubqueryParameterInputFromPathConstant)e.ModelElement).ParameterInput);
		}
		/// <summary>
		/// AddRule: typeof(SubqueryParameterInputFromPathedRole)
		/// </summary>
		private static void InputFromPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			SubqueryParameterInput input = ((SubqueryParameterInputFromPathedRole)e.ModelElement).ParameterInput;
			input.InputFromPathRoot = null;
			input.InputFromConstant = null;
			input.InputFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(SubqueryParameterInputFromPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void InputFromPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((SubqueryParameterInputFromPathedRole)e.ModelElement).ParameterInput);
		}
		/// <summary>
		/// AddRule: typeof(SubqueryParameterInputFromRolePathRoot)
		/// </summary>
		private static void InputFromPathRootAddedRule(ElementAddedEventArgs e)
		{
			SubqueryParameterInput input = ((SubqueryParameterInputFromRolePathRoot)e.ModelElement).ParameterInput;
			input.InputFromPathedRole = null;
			input.InputFromConstant = null;
			input.InputFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(SubqueryParameterInputFromRolePathRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void InputFromPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((SubqueryParameterInputFromRolePathRoot)e.ModelElement).ParameterInput);
		}
		#endregion // Parameter input validation rules
	}
	#endregion // SubqueryParameterInput class
	#region SubtypeDerivationRule class
	partial class SubtypeDerivationRule : IModelErrorDisplayContext, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="ObjectType"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ObjectType objectType = Subtype;
				return objectType != null ? objectType.ResolvedModel : null;
			}
		}
		#endregion // Base overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				string subTypeName = null;
				string modelName = null;
				ObjectType subType = Subtype;
				if (subType != null)
				{
					subTypeName = subType.Name;
					ORMModel model = subType.ResolvedModel;
					if (model != null)
					{
						modelName = model.Name;
					}
				}
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSubtypeDerivationRule, subTypeName ?? "", modelName ?? "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { SubtypeHasDerivationRule.DerivationRuleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// DeleteRule: typeof(SubtypeDerivationRuleHasDerivationNote), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void DerivationNoteDeletedRule(ElementDeletedEventArgs e)
		{
			SubtypeDerivationRule derivationRule = ((SubtypeDerivationRuleHasDerivationNote)e.ModelElement).DerivationRule;
			if (!derivationRule.IsDeleted &&
				derivationRule.ExternalDerivation &&
				derivationRule.LeadRolePathCollection.Count == 0 &&
				derivationRule.SubqueryCollection.Count == 0)
			{
				derivationRule.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when paths are added to the derivation rule.
		/// </summary>
		private static void LeadRolePathAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			SubtypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as SubtypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when paths are added to the derivation rule.
		/// </summary>
		private static void LeadRolePathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			SubtypeDerivationRule derivationRule;
			if (e.DomainRole.Id == RolePathOwnerHasLeadRolePath.PathOwnerDomainRoleId &&
				null != (derivationRule = e.NewRolePlayer as SubtypeDerivationRule) &&
				!derivationRule.IsDeleted)
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// AddRule: typeof(RolePathOwnerHasSubquery), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when subqueries are added to the derivation rule.
		/// </summary>
		private static void SubqueryAddedRule(ElementAddedEventArgs e)
		{
			RolePathOwnerHasSubquery link = (RolePathOwnerHasSubquery)e.ModelElement;
			SubtypeDerivationRule derivationRule;
			if (!link.IsDeleted &&
				null != (derivationRule = link.PathOwner as SubtypeDerivationRule))
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasSubquery), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Clear the ExternalDefinition setting when subqueries are added to the derivation rule.
		/// </summary>
		private static void SubqueryRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			SubtypeDerivationRule derivationRule;
			if (e.DomainRole.Id == RolePathOwnerHasSubquery.PathOwnerDomainRoleId &&
				null != (derivationRule = e.NewRolePlayer as SubtypeDerivationRule) &&
				!derivationRule.IsDeleted)
			{
				derivationRule.ExternalDerivation = false;
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // SubtypeDerivationRule class
	#region ConstraintRoleSequenceJoinPath class
	partial class ConstraintRoleSequenceJoinPath : IModelErrorDisplayContext, IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Get the <see cref="ORMModel"/> from the associated <see cref="ConstraintRoleSequence"/>
		/// </summary>
		public override ORMModel Model
		{
			get
			{
				ConstraintRoleSequence roleSequence;
				IConstraint constraint;
				return (null != (roleSequence = RoleSequence) && null != (constraint = roleSequence.Constraint)) ? constraint.Model : null;
			}
		}
		/// <summary>
		/// Automatic join paths cannot be shared. Changing a shared role path to automatic
		/// will result in existing shared paths being detached from this owner.
		/// </summary>
		public override bool AllowOwnedPathSharing
		{
			get
			{
				return !IsAutomatic;
			}
		}
		/// <summary>
		/// Add role projections to the set of base elements than can consume a <see cref="CalculatedPathValue"/>
		/// </summary>
		protected override bool IsCalculatedPathValueConsumed(CalculatedPathValue calculation)
		{
			return base.IsCalculatedPathValueConsumed(calculation) || ConstraintRoleProjectedFromCalculatedPathValue.GetConstraintRoleProjections(calculation).Count != 0;
		}
		/// <summary>
		/// Register validation for a new <see cref="FactTypeDerivationRule"/>
		/// </summary>
		protected override void NewlyCreated()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateProjectionExistence);
		}
		/// <summary>
		/// Check projections
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback.</param>
		protected override void ValidateDerivedRolePathOwner(INotifyElementAdded notifyAdded)
		{
			if (!IsAutomatic)
			{
				ValidateAutomaticProjections(notifyAdded);
			}
			if (notifyAdded != null)
			{
				// We do all projection analysis independently after the deserialization request
				ValidateProjectionExistence(notifyAdded);
			}
			if (!IsAutomatic)
			{
				ValidateProjectionTypes(notifyAdded);
			}
		}
		private void ValidateProjectionTypes(INotifyElementAdded notifyAdded)
		{
			ORMModel contextModel = null;
			foreach (ConstraintRoleSequenceJoinPathProjection projectionSet in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				foreach (ConstraintRoleProjection projection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projectionSet))
				{
					PathedRole pathedRole;
					RolePathObjectTypeRoot pathRoot;
					bool hasError = false;
					if (null != (pathedRole = projection.ProjectedFromPathedRole))
					{
						hasError = !new RolePathNode(pathedRole).CanProjectOn(projection.ProjectedConstraintRole.Role.RolePlayer);
					}
					else if (null != (pathRoot = projection.ProjectedFromPathRoot))
					{
						hasError = !new RolePathNode(pathRoot).CanProjectOn(projection.ProjectedConstraintRole.Role.RolePlayer);
					}
					ConstraintRoleRequiresCompatibleJoinPathProjectionError error = projection.IncompatibleProjectionError;
					if (hasError)
					{
						if (error == null &&
							null != (contextModel ?? (contextModel = this.Model)))
						{
							error = new ConstraintRoleRequiresCompatibleJoinPathProjectionError(Partition);
							error.Projection = projection;
							error.Model = contextModel;
							error.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(error, true);
							}
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
			}
		}
		/// <summary>
		/// Helper struct for ValidateAutomaticProjections
		/// </summary>
		private struct AutoProjectedConstraintRoleInfo
		{
			public readonly ConstraintRoleSequenceHasRole ConstraintRole;
			public readonly ObjectType RolePlayer;
			public ConstraintRoleProjection ConstraintRoleProjection;
			public RolePathNode MatchingPathNode;
			public bool HasMultipleMatches;
			public bool ExplicitProjection;
			public AutoProjectedConstraintRoleInfo(ConstraintRoleSequenceHasRole constraintRole, ObjectType rolePlayer)
			{
				ConstraintRole = constraintRole;
				RolePlayer = rolePlayer;
				MatchingPathNode = RolePathNode.Empty;
				HasMultipleMatches = false;
				ConstraintRoleProjection = null;
				ExplicitProjection = false;
			}
			/// <summary>
			/// Record if a path node matches any of the data items.
			/// </summary>
			/// <param name="data">Data previously initialized with the <see cref="Reset"/> method.</param>
			/// <param name="pathNode">The node to test for type matches.</param>
			/// <returns>The number of nodes eliminated from automatic projection consideration. When the
			/// cumulative total of return values from this method matches the return value from the Reset
			/// method, all automatic projection possibilities have been eliminated.</returns>
			public static int RecordMatches(AutoProjectedConstraintRoleInfo[] data, RolePathNode pathNode)
			{
				ObjectType startingType;
				int eliminatedItems = 0;
				if (null != (startingType = pathNode.ObjectType))
				{
					ObjectType.WalkSupertypes(
						startingType,
						delegate(ObjectType type, int depth, bool isPrimary)
						{
							for (int i = 0; i < data.Length; ++i)
							{
								AutoProjectedConstraintRoleInfo info = data[i];
								if (!info.ExplicitProjection &&
									!info.HasMultipleMatches &&
									info.RolePlayer == type)
								{
									RolePathNode previousNode = info.MatchingPathNode;
									bool previousNodeIsEmpty = previousNode.IsEmpty;
									PathObjectUnifier unifier;
									if (previousNodeIsEmpty)
									{
										// Note that we're walking from the top down, so
										// this will automatically get the first correlated
										// object.
										info.MatchingPathNode = pathNode;
									}
									else if (null == (unifier = previousNode.ObjectUnifier) ||
										unifier != pathNode.ObjectUnifier)
									{
										++eliminatedItems;
										info.HasMultipleMatches = true;
									}
									else
									{
										continue;
									}
									data[i] = info;
								}
							}
							return ObjectTypeVisitorResult.Continue;
						});
				}
				return eliminatedItems;
			}
			/// <summary>
			/// Reset projected role information for a projection set before iterating the role path.
			/// </summary>
			/// <returns>Number of possible automatic projections.</returns>
			public static int Reset(AutoProjectedConstraintRoleInfo[] data, ConstraintRoleSequenceJoinPathProjection projectionSet)
			{
				int retVal = 0;
				for (int i = 0; i < data.Length; ++i)
				{
					AutoProjectedConstraintRoleInfo info = data[i];
					ConstraintRoleProjection projection;
					if (projectionSet != null &&
						null != (projection = ConstraintRoleProjection.GetLink(projectionSet, info.ConstraintRole)))
					{
						info.ConstraintRoleProjection = projection;
						bool canBeAuto = projection.IsAutomatic;
						info.ExplicitProjection = !canBeAuto;
						if (canBeAuto)
						{
							++retVal;
						}
					}
					else
					{
						info.ConstraintRoleProjection = null;
						info.ExplicitProjection = false;
						++retVal;
					}
					info.MatchingPathNode = RolePathNode.Empty;
					info.HasMultipleMatches = false;
					data[i] = info;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Populate automatic projections if there is exactly one source node
		/// that matches the possible role projection.
		/// </summary>
		private void ValidateAutomaticProjections(INotifyElementAdded notifyAdded)
		{
			// The algorithm here should mirror LeadRolePath.FindSingleNodeOfType
			int constrainedRoleCount = 0;
			AutoProjectedConstraintRoleInfo[] constraintRoleInfo = null;
			foreach (LeadRolePath leadRolePath in LeadRolePathCollection)
			{
				if (constrainedRoleCount == 0)
				{
					ReadOnlyCollection<ConstraintRoleSequenceHasRole> constrainedRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(RoleSequence);
					constrainedRoleCount = constrainedRoles.Count;
					constraintRoleInfo = new AutoProjectedConstraintRoleInfo[constrainedRoleCount];
					for (int i = 0; i < constrainedRoleCount; ++i)
					{
						ConstraintRoleSequenceHasRole constraintRole = constrainedRoles[i];
						constraintRoleInfo[i] = new AutoProjectedConstraintRoleInfo(constraintRole, constraintRole.Role.RolePlayer);
					}
				}
				ConstraintRoleSequenceJoinPathProjection projectionSet = ConstraintRoleSequenceJoinPathProjection.GetLink(this, leadRolePath);
				int possibleAutoCount = AutoProjectedConstraintRoleInfo.Reset(constraintRoleInfo, projectionSet);
				if (possibleAutoCount == 0)
				{
					continue;
				}
				RolePathNode.VisitPathNodes(
					leadRolePath,
					RolePathNode.Empty,
					false,
					delegate(RolePathNode pathNode, RolePathNode previousPathNode, bool unwinding)
					{
						PathedRole pathedRole;
						if (null != (pathedRole = pathNode.PathedRole) &&
							pathedRole.PathedRolePurpose != PathedRolePurpose.SameFactType)
						{
							return true;
						}
						possibleAutoCount -= AutoProjectedConstraintRoleInfo.RecordMatches(constraintRoleInfo, pathNode);
						return possibleAutoCount != 0;
					});
				Store store = this.Store;
				bool setChangingFlag = notifyAdded != null; // Flag not needed if rules not on, pretend it is set in this case
				try
				{
					for (int i = 0; i < constrainedRoleCount; ++i)
					{
						AutoProjectedConstraintRoleInfo info = constraintRoleInfo[i];
						if (!info.ExplicitProjection)
						{
							RolePathNode autoNode = info.MatchingPathNode;
							ConstraintRoleProjection projection = info.ConstraintRoleProjection;
							PathedRole autoPathedRole;
							RolePathObjectTypeRoot autoPathRoot;
							ModelElement notifyLink;
							if (info.HasMultipleMatches || autoNode.IsEmpty)
							{
								if (projection != null)
								{
									// This is run before ValidateProjectionExistence, which will clean up empty
									// sets and add associated errors, so we don't have to do it here.
									projection.Delete();
								}
							}
							else
							{
								if (projection != null)
								{
									PathedRole existingPathedRole;
									RolePathObjectTypeRoot existingPathRoot;
									if (null != (existingPathedRole = projection.ProjectedFromPathedRole))
									{
										if (null != (autoPathedRole = autoNode.PathedRole))
										{
											if (autoPathedRole == existingPathedRole)
											{
												continue;
											}
											if (!setChangingFlag)
											{
												ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
												setChangingFlag = true;
											}
											projection.ProjectedFromPathedRole = autoPathedRole;
										}
										else
										{
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(new ConstraintRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot), false);
												projection.ProjectedFromPathedRole = null;
											}
											else
											{
												if (!setChangingFlag)
												{
													ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
													setChangingFlag = true;
												}
												// Rules will clear out the other element
												projection.ProjectedFromPathRoot = autoNode.PathRoot;
											}
										}
									}
									else if (null != (existingPathRoot = projection.ProjectedFromPathRoot))
									{
										if (null != (autoPathRoot = autoNode.PathRoot))
										{
											if (autoPathRoot == existingPathRoot)
											{
												continue;
											}
											if (!setChangingFlag)
											{
												ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
												setChangingFlag = true;
											}
											projection.ProjectedFromPathRoot = autoPathRoot;
										}
										else
										{
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(new ConstraintRoleProjectedFromPathedRole(projection, autoNode.PathedRole), false);
												projection.ProjectedFromPathRoot = null;
											}
											else
											{
												if (!setChangingFlag)
												{
													ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
													setChangingFlag = true;
												}
												// Rules will clear out the other element
												projection.ProjectedFromPathedRole = autoNode.PathedRole;
											}
										}
									}
									else
									{
										// Invalid state that we shouldn't hit during normal process (we don't auto project constants or calculations),
										// but can happen on load.
										if (!setChangingFlag)
										{
											ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
											setChangingFlag = true;
										}
										notifyLink = (null != (autoPathedRole = autoNode.PathedRole)) ?
											(ModelElement)new ConstraintRoleProjectedFromPathedRole(projection, autoPathedRole) :
											new ConstraintRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(notifyLink, false);
											projection.ProjectedFromConstant = null;
											projection.ProjectedFromCalculatedValue = null;
										}
									}
								}
								else
								{
									if (projectionSet == null)
									{
										projectionSet = new ConstraintRoleSequenceJoinPathProjection(this, leadRolePath);
										if (notifyAdded != null)
										{
											notifyAdded.ElementAdded(projectionSet, false);
										}
									}
									projection = new ConstraintRoleProjection(Partition,
										new RoleAssignment[]{
											new RoleAssignment(ConstraintRoleProjection.JoinPathProjectionDomainRoleId, projectionSet),
											new RoleAssignment(ConstraintRoleProjection.ProjectedConstraintRoleDomainRoleId, info.ConstraintRole)},
										new PropertyAssignment[]{
											new PropertyAssignment(ConstraintRoleProjection.IsAutomaticDomainPropertyId, true)});
									if (!setChangingFlag)
									{
										ConstraintRoleProjection.ChangingAutomaticProjection(store, true);
										setChangingFlag = true;
									}
									notifyLink = (null != (autoPathedRole = autoNode.PathedRole)) ?
										(ModelElement)new ConstraintRoleProjectedFromPathedRole(projection, autoPathedRole) :
										new ConstraintRoleProjectedFromRolePathRoot(projection, autoNode.PathRoot);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(projection, false);
										notifyAdded.ElementAdded(notifyLink, false);
									}
								}
							}
						}
					}
				}
				finally
				{
					if (setChangingFlag && notifyAdded == null)
					{
						ConstraintRoleProjection.ChangingAutomaticProjection(store, false);
					}
				}
			}
		}
		/// <summary>
		/// Verify all projection errors
		/// </summary>
		/// <param name="element">A <see cref="ConstraintRoleSequenceJoinPath"/></param>
		private static void DelayValidateProjectionExistence(ModelElement element)
		{
			if (!element.IsDeleted)
			{
				((ConstraintRoleSequenceJoinPath)element).ValidateProjectionExistence(null);
			}
		}
		/// <summary>
		/// Validate projections for existence and completeness
		/// </summary>
		/// <param name="notifyAdded">Standard deserialization callback. Perform extra
		/// structural checks if this is set.</param>
		private void ValidateProjectionExistence(INotifyElementAdded notifyAdded)
		{
			bool seenProjection = false;
			int constraintRoleCount = 0;
			Partition partition = Partition;
			ORMModel model = null;
			foreach (ConstraintRoleSequenceJoinPathProjection projection in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(this))
			{
				if (constraintRoleCount == 0)
				{
					constraintRoleCount = RoleSequence.RoleCollection.Count;
				}
				int projectedRoleCount;
				if (notifyAdded != null)
				{
					// Clean up projections with no source information on load. Empty
					// projections are automatically removed after deserialization, so
					// verify that we have a consistent state.
					ReadOnlyCollection<ConstraintRoleProjection> projectionLinks = ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projection);
					projectedRoleCount = projectionLinks.Count;
					foreach (ConstraintRoleProjection roleProjection in projectionLinks)
					{
						if (null == roleProjection.ProjectedFromPathedRole &&
							null == roleProjection.ProjectedFromPathRoot &&
							null == roleProjection.ProjectedFromCalculatedValue &&
							null == roleProjection.ProjectedFromConstant)
						{
							--projectedRoleCount;
							roleProjection.Delete();
						}
					}
				}
				else
				{
					projectedRoleCount = projection.ProjectedRoleCollection.Count;
				}
				if (projectedRoleCount == 0)
				{
					projection.Delete();
					continue;
				}
				seenProjection = true;
				PartialConstraintRoleSequenceJoinPathProjectionError partialProjectionError = projection.PartialProjectionError;
				if (projectedRoleCount < constraintRoleCount)
				{
					if (partialProjectionError == null)
					{
						partialProjectionError = new PartialConstraintRoleSequenceJoinPathProjectionError(partition);
						partialProjectionError.JoinPathProjection = projection;
						partialProjectionError.Model = model ?? (model = Model);
						partialProjectionError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(partialProjectionError, true);
						}
					}
				}
				else if (partialProjectionError != null)
				{
					partialProjectionError.Delete();
				}
			}
			ConstraintRoleSequenceJoinPathRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
			if (!seenProjection)
			{
				if (projectionRequiredError == null)
				{
					projectionRequiredError = new ConstraintRoleSequenceJoinPathRequiresProjectionError(partition);
					projectionRequiredError.JoinPath = this;
					projectionRequiredError.Model = model ?? (model = Model);
					projectionRequiredError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(projectionRequiredError, true);
					}
				}
			}
			else if (projectionRequiredError != null)
			{
				projectionRequiredError.Delete();
			}
		}
		#endregion // Base overrides
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				ConstraintRoleSequence roleSequence = RoleSequence;
				SetConstraint setConstraint;
				SetComparisonConstraintRoleSequence setComparisonSequence;
				if (null != (setConstraint = roleSequence as SetConstraint))
				{
					ORMModel model = Model;
					return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetConstraintJoinPath, setConstraint.Name, model != null ? model.Name : "");
				}
				else if (null != (setComparisonSequence = roleSequence as SetComparisonConstraintRoleSequence))
				{
					string modelName = null;
					int sequenceNumber = 0;
					string constraintName = null;
					SetComparisonConstraint constraint = setComparisonSequence.ExternalConstraint;
					if (constraint != null)
					{
						constraintName = constraint.Name;
						ORMModel model = constraint.ResolvedModel;
						if (model != null)
						{
							modelName = model.Name;
						}
						sequenceNumber = constraint.RoleSequenceCollection.IndexOf(setComparisonSequence) + 1;
					}
					return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetComparisonConstraintSequenceJoinPath, constraintName ?? "", modelName ?? "", sequenceNumber);
				}
				return "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ConstraintRoleSequenceJoinPathRequiresProjectionError projectionRequiredError = ProjectionRequiredError;
				if (projectionRequiredError != null)
				{
					yield return projectionRequiredError;
				}
				else
				{
					foreach (ConstraintRoleSequenceJoinPathProjection projectionSet in ConstraintRoleSequenceJoinPathProjection.GetLinksToProjectedPathComponentCollection(this))
					{
						foreach (ConstraintRoleProjection projection in ConstraintRoleProjection.GetLinksToProjectedRoleCollection(projectionSet))
						{
							ConstraintRoleRequiresCompatibleJoinPathProjectionError compatibilityError = projection.IncompatibleProjectionError;
							if (compatibilityError != null)
							{
								yield return compatibilityError;
							}
						}
						PartialConstraintRoleSequenceJoinPathProjectionError partialProjectionError = projectionSet.PartialProjectionError;
						if (partialProjectionError != null)
						{
							yield return partialProjectionError;
						}
					}
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
		#region Validation Rule Methods
		/// <summary>
		/// Attach rule notifications for data type changes
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public new static void EnableRuleNotifications(Store store)
		{
			RolePathOwner.AddRolePathDataTypeChangeRuleNotification(store, ProcessDataTypeChange);
		}
		private static void ProcessDataTypeChange(ObjectType valueType, IList<Role> playedRoles, RolePathNode processNode)
		{
			PathedRole pathedRole;
			if (processNode.IsEmpty)
			{
				foreach (Role playedRole in playedRoles)
				{
					foreach (ConstraintRoleSequence sequence in ConstraintRoleSequenceHasRole.GetConstraintRoleSequenceCollection(playedRole))
					{
						ConstraintRoleSequenceJoinPath joinPath;
						if (null != (joinPath = sequence.JoinPath))
						{
							FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateDerivedRolePathOwner);
						}
					}
				}
			}
			else if (null != (pathedRole = processNode.PathedRole))
			{
				foreach (ConstraintRoleProjection projection in ConstraintRoleProjectedFromPathedRole.GetConstraintRoleProjections(pathedRole))
				{
					FrameworkDomainModel.DelayValidateElement(projection.JoinPathProjection.JoinPath, DelayValidateDerivedRolePathOwner);
				}
			}
			else
			{
				foreach (ConstraintRoleProjection projection in ConstraintRoleProjectedFromRolePathRoot.GetConstraintRoleProjections(processNode.PathRoot))
				{
					FrameworkDomainModel.DelayValidateElement(projection.JoinPathProjection.JoinPath, DelayValidateDerivedRolePathOwner);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath;
			if (null != (joinPath = ((ConstraintRoleSequenceHasRole)e.ModelElement).ConstraintRoleSequence.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequence roleSequence = ((ConstraintRoleSequenceHasRole)e.ModelElement).ConstraintRoleSequence;
			ConstraintRoleSequenceJoinPath joinPath;
			if (!roleSequence.IsDeleted &&
				null != (joinPath = roleSequence.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueAddedRule(ElementAddedEventArgs e)
		{
			CalculatedValueUseChangedInRule(((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).Source);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculation = ((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).Source;
			if (!calculation.IsDeleted)
			{
				CalculatedValueUseChangedInRule(calculation);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectionOnCalculatedPathValueRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.OldRolePlayer);
				CalculatedValueUseChangedInRule((CalculatedPathValue)e.NewRolePlayer);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(PathObjectUnifierUnifiesPathedRole)
		/// Preserve projection with other previously unified elements if
		/// a unified pathed role is deleted.
		/// </summary>
		private static void PathedRoleUnificationDeletingRule(ElementDeletingEventArgs e)
		{
			PathObjectUnifierUnifiesPathedRole link = (PathObjectUnifierUnifiesPathedRole)e.ModelElement;
			ReadOnlyCollection<ConstraintRoleProjectedFromPathedRole> constraintRoleProjections = ConstraintRoleProjectedFromPathedRole.GetLinksToConstraintRoleProjections(link.PathedRole);
			if (constraintRoleProjections.Count != 0)
			{
				PathObjectUnifier objectUnifier = link.ObjectUnifier;
				foreach (PathObjectUnifierUnifiesRolePathRoot unifiedRootLink in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(objectUnifier))
				{
					if (!unifiedRootLink.IsDeleting)
					{
						RolePathObjectTypeRoot replaceWithPathRoot = unifiedRootLink.PathRoot;
						foreach (ConstraintRoleProjectedFromPathedRole constraintRoleProjection in constraintRoleProjections)
						{
							if (!constraintRoleProjection.IsDeleting)
							{
								constraintRoleProjection.ConstraintRoleProjection.ProjectedFromPathRoot = replaceWithPathRoot;
							}
						}
						return;
					}
				}
				foreach (PathObjectUnifierUnifiesPathedRole unifiedPathedRoleLink in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(objectUnifier))
				{
					if (!unifiedPathedRoleLink.IsDeleting)
					{
						PathedRole replaceWithPathedRole = unifiedPathedRoleLink.PathedRole;
						foreach (ConstraintRoleProjectedFromPathedRole constraintRoleProjection in constraintRoleProjections)
						{
							if (!constraintRoleProjection.IsDeleting)
							{
								constraintRoleProjection.ConstraintRoleProjection.ProjectedFromPathedRole = replaceWithPathedRole;
							}
						}
						return;
					}
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(PathObjectUnifierUnifiesRolePathRoot)
		/// Preserve projection with other previously unified elements if
		/// a unified path root is deleted.
		/// </summary>
		private static void PathRootUnificationDeletingRule(ElementDeletingEventArgs e)
		{
			PathObjectUnifierUnifiesRolePathRoot link = (PathObjectUnifierUnifiesRolePathRoot)e.ModelElement;
			ReadOnlyCollection<ConstraintRoleProjectedFromRolePathRoot> constraintRoleProjections = ConstraintRoleProjectedFromRolePathRoot.GetLinksToConstraintRoleProjections(link.PathRoot);
			if (constraintRoleProjections.Count != 0)
			{
				PathObjectUnifier objectUnifier = link.ObjectUnifier;
				foreach (PathObjectUnifierUnifiesRolePathRoot unifiedRootLink in PathObjectUnifierUnifiesRolePathRoot.GetLinksToPathRootCollection(objectUnifier))
				{
					if (!unifiedRootLink.IsDeleting)
					{
						RolePathObjectTypeRoot replaceWithPathRoot = unifiedRootLink.PathRoot;
						foreach (ConstraintRoleProjectedFromRolePathRoot constraintRoleProjection in constraintRoleProjections)
						{
							if (!constraintRoleProjection.IsDeleting)
							{
								constraintRoleProjection.ConstraintRoleProjection.ProjectedFromPathRoot = replaceWithPathRoot;
							}
						}
						return;
					}
				}
				foreach (PathObjectUnifierUnifiesPathedRole unifiedPathedRoleLink in PathObjectUnifierUnifiesPathedRole.GetLinksToPathedRoleCollection(objectUnifier))
				{
					if (!unifiedPathedRoleLink.IsDeleting)
					{
						PathedRole replaceWithPathedRole = unifiedPathedRoleLink.PathedRole;
						foreach (ConstraintRoleProjectedFromRolePathRoot constraintRoleProjection in constraintRoleProjections)
						{
							if (!constraintRoleProjection.IsDeleting)
							{
								constraintRoleProjection.ConstraintRoleProjection.ProjectedFromPathedRole = replaceWithPathedRole;
							}
						}
						return;
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ConstraintRoleSequenceJoinPathProjection)e.ModelElement).JoinPath, DelayValidateProjectionExistence);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.ModelElement).JoinPath;
			if (!joinPath.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleSequenceJoinPathProjection)
		/// </summary>
		private static void ProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(e.OldRolePlayer, DelayValidateProjectionExistence);
				FrameworkDomainModel.DelayValidateElement(e.NewRolePlayer, DelayValidateProjectionExistence);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(((ConstraintRoleSequenceJoinPathProjection)e.ElementLink).JoinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleProjection)e.ModelElement).JoinPathProjection.JoinPath;
			if (joinPath != null)
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceJoinPathProjection projection = ((ConstraintRoleProjection)e.ModelElement).JoinPathProjection;
			ConstraintRoleSequenceJoinPath joinPath;
			if (!projection.IsDeleted &&
				null != (joinPath = projection.JoinPath))
			{
				FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleProjection)
		/// </summary>
		private static void RoleProjectionRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ConstraintRoleProjection.JoinPathProjectionDomainRoleId)
			{
				ConstraintRoleSequenceJoinPath joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.OldRolePlayer).JoinPath;
				if (joinPath != null)
				{
					FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
				}
				joinPath = ((ConstraintRoleSequenceJoinPathProjection)e.NewRolePlayer).JoinPath;
				if (joinPath != null)
				{
					FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateProjectionExistence);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void LeadRolePathDeletedRule(ElementDeletedEventArgs e)
		{
			RolePathOwnerHasLeadRolePath link = (RolePathOwnerHasLeadRolePath)e.ModelElement;
			DeleteProjectionForDetachedPath(link.PathOwner, link.RolePath);
		}
		/// <summary>
		/// If a role path has been detached from the join path but not deleted, then clear
		/// the associated projection if the path has not been reattached through a different
		/// (owns vs sharing) relationship.
		/// </summary>
		private static void DeleteProjectionForDetachedPath(RolePathOwner owner, LeadRolePath rolePath)
		{
			ConstraintRoleSequenceJoinPath joinPath;
			ConstraintRoleSequenceJoinPathProjection projection;
			if (!rolePath.IsDeleted &&
				null != (joinPath = owner as ConstraintRoleSequenceJoinPath) &&
				!joinPath.IsDeleted &&
				null == RolePathOwnerHasLeadRolePath.GetLink(joinPath, rolePath) &&
				null != (projection = ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, rolePath)))
			{
				projection.Delete();
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(RolePathOwnerHasLeadRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Eliminate projections for detached but undeleted paths.
		/// </summary>
		private static void LeadRolePathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == RolePathOwnerHasLeadRolePath.PathOwnerDomainRoleId)
			{
				DeleteProjectionForDetachedPath((RolePathOwner)e.OldRolePlayer, ((RolePathOwnerHasLeadRolePath)e.ElementLink).RolePath);
			}
			else
			{
				DeleteProjectionForDetachedPath(((RolePathOwnerHasLeadRolePath)e.ElementLink).PathOwner, (LeadRolePath)e.OldRolePlayer);
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			ValidateAttachedJoinPathProjections(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			ValidateAttachedJoinPathProjections(((ObjectTypePlaysRole)e.ModelElement).PlayedRole);
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Verify projections when a role player changes
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ElementLink;
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				ValidateAttachedJoinPathProjections(link.PlayedRole);
			}
			else
			{
				ValidateAttachedJoinPathProjections((Role)e.OldRolePlayer);
				ValidateAttachedJoinPathProjections((Role)e.NewRolePlayer);
			}
		}
		private static void ValidateAttachedJoinPathProjections(Role role)
		{
			if (role != null &&
				!role.IsDeleted &&
				!role.IsDeleting)
			{
				foreach (ConstraintRoleSequence constrainedSequence in role.ConstraintRoleSequenceCollection)
				{
					ConstraintRoleSequenceJoinPath joinPath;
					if (null != (joinPath = constrainedSequence.JoinPath))
					{
						FrameworkDomainModel.DelayValidateElement(joinPath, DelayValidateDerivedRolePathOwner);
					}
				}
			}
		}
		#endregion // Validation Rule Methods
	}
	#endregion // ConstraintRoleSequenceJoinPath class
	#region ConstraintRoleSequenceJoinPathProjection class
	partial class ConstraintRoleSequenceJoinPathProjection : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the projection level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = JoinPath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // ConstraintRoleSequenceJoinPathProjection class
	#region ConstraintRoleProjection class
	partial class ConstraintRoleProjection : IElementLinkRoleHasIndirectModelErrorOwner
	{
		#region Custom Storage Handlers
		private bool myIsAutomatic;
		private bool GetIsAutomaticValue()
		{
			return myIsAutomatic;
		}
		private void SetIsAutomaticValue(bool value)
		{
			if (value &&
				!Store.InUndoRedoOrRollback)
			{
				ConstraintRoleSequenceJoinPathProjection projection = JoinPathProjection;
				if (projection.JoinPath.IsAutomatic)
				{
					throw new InvalidOperationException(ResourceStrings.ModelErrorConstraintRoleProjectionAutomaticProjectionUnavailableForAutomaticJoinPath);
				}
				PathedRole currentPathedRole;
				RolePathObjectTypeRoot currentPathRoot = null;
				// If all values are null, then this is newly constructed. Do not
				// attempt to validate an incoming value. The incoming value is coming
				// from initialization code, not the user.
				if (!(null == (currentPathedRole = ProjectedFromPathedRole) &&
					null == (currentPathRoot = ProjectedFromPathRoot) &&
					null == ProjectedFromCalculatedValue &&
					null == ProjectedFromConstant))
				{
					RolePathNode singleNode = projection.RolePath.FindSingleNodeOfType(ProjectedConstraintRole.Role.RolePlayer);
					if (singleNode.IsEmpty)
					{
						throw new InvalidOperationException(ResourceStrings.ModelErrorConstraintRoleProjectionAutomaticProjectionUnavailable);
					}
					else
					{
						// Note that we do not need to set ChangingAutomaticProjection here because
						// the getter for this property will still return false, so no attempt will
						// be made to reset it.
						PathedRole newPathedRole;
						if (null != (newPathedRole = singleNode.PathedRole))
						{
							if (newPathedRole != currentPathedRole)
							{
								ProjectedFromPathedRole = newPathedRole;
							}
						}
						else
						{
							RolePathObjectTypeRoot newPathRoot = singleNode.PathRoot;
							if (newPathRoot != currentPathRoot)
							{
								ProjectedFromPathRoot = newPathRoot;
							}
						}
					}
				}
			}
			myIsAutomatic = value;
		}
		#endregion // Custom Storage Handlers
		#region Rule Helpers
		/// <summary>
		/// Add a flag to the store that changes to existing automatic projections
		/// should not result in the <see cref="IsAutomatic"/> property being cleared.
		/// </summary>
		/// <param name="store">The Store object being modified.</param>
		/// <param name="value">The boolean value to set, or null to retrieve the current value.</param>
		public static bool ChangingAutomaticProjection(Store store, bool? value)
		{
			Transaction transaction;
			if (null != (transaction = store.TransactionManager.CurrentTransaction))
			{
				transaction = transaction.TopLevelTransaction;
				Dictionary<object, object> dict = transaction.Context.ContextInfo;
				object key = typeof(ConstraintRoleProjection);
				object counterObj;
				bool dictHasValue;
				dictHasValue = dict.TryGetValue(key, out counterObj) && counterObj is int;
				if (value.HasValue)
				{
					if (value.Value)
					{
						dict[key] = dictHasValue ? ((int)counterObj) + 1 : 1;
					}
					else if (dictHasValue)
					{
						int counter = (int)counterObj;
						if (counter == 1)
						{
							dict.Remove(key);
						}
						else
						{
							dict[key] = counter + 1;
						}
					}
				}
				else
				{
					return dictHasValue;
				}
			}
			return false;
		}
		#endregion // Rule Helpers
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleProjection.JoinPathProjectionDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
	}
	#endregion // ConstraintRoleProjection class
	#region RoleSubPath class
	partial class RoleSubPath : IHasIndirectModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Recursively find the path root
		/// </summary>
		public override LeadRolePath RootRolePath
		{
			get
			{
				RoleSubPath subPath = this;
				RolePath parentPath = null;
				while (subPath != null)
				{
					parentPath = subPath.ParentRolePath;
					subPath = parentPath as RoleSubPath;
				}
				return parentPath as LeadRolePath;
			}
		}
		#endregion // Base overrides
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // RoleSubPath class
	#region PathedRole class
	partial class PathedRole : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessor Properties
		/// <summary>
		/// Get the previous path node in either this or the
		/// containing role path.
		/// </summary>
		public RolePathNode PreviousPathNode
		{
			get
			{
				RolePath rolePath = RolePath;
				ReadOnlyCollection<PathedRole> pathedRoles = rolePath.PathedRoleCollection;
				int index = pathedRoles.IndexOf(this);
				switch (index)
				{
					case -1:
						break;
					case 0:
						RolePathObjectTypeRoot pathRoot = rolePath.PathRoot;
						if (pathRoot != null)
						{
							return pathRoot;
						}
						RoleSubPath subPath;
						while (null != (subPath = rolePath as RoleSubPath) &&
							null != (rolePath = subPath.ParentRolePath))
						{
							pathedRoles = rolePath.PathedRoleCollection;
							int pathedRoleCount = pathedRoles.Count;
							if (pathedRoleCount != 0)
							{
								return pathedRoles[pathedRoleCount - 1];
							}
							else if (null != (pathRoot = rolePath.PathRoot))
							{
								return pathRoot;
							}
						}
						break;
					default:
						return pathedRoles[index - 1];
				}
				return RolePathNode.Empty;
			}
		}
		#endregion // Accessor Properties
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { PathedRole.RolePathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the pathed role level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = RolePath.RootRolePath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // PathedRole class
	#region RolePathObjectTypeRoot class
	partial class RolePathObjectTypeRoot : IElementLinkRoleHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessor Properties
		/// <summary>
		/// Get the previous path node in the containing role path.
		/// </summary>
		public RolePathNode PreviousPathNode
		{
			get
			{
				RolePath rolePath = RolePath;
				RoleSubPath subPath;
				while (null != (subPath = rolePath as RoleSubPath) &&
					null != (rolePath = subPath.ParentRolePath))
				{
					ReadOnlyCollection<PathedRole> pathedRoles = rolePath.PathedRoleCollection;
					int pathedRoleCount = pathedRoles.Count;
					RolePathObjectTypeRoot pathRoot;
					if (pathedRoleCount != 0)
					{
						return pathedRoles[pathedRoleCount - 1];
					}
					else if (null != (pathRoot = rolePath.PathRoot))
					{
						return pathRoot;
					}
				}
				return RolePathNode.Empty;
			}
		}
		#endregion // Accessor Properties
		#region IElementLinkRoleHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerElementLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RolePathObjectTypeRoot.RolePathDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IElementLinkRoleHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerElementLinkRoles()
		{
			return GetIndirectModelErrorOwnerElementLinkRoles();
		}
		#endregion // IElementLinkRoleHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the path root level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = RolePath.RootRolePath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // RolePathObjectTypeRoot class
	#region Function class
	partial class Function : IModelErrorOwner
	{
		#region Base overrides
		/// <summary>
		/// Use the function name for string display
		/// </summary>
		public override string ToString()
		{
			string symbol = OperatorSymbol;
			return string.IsNullOrEmpty(symbol) ? Name : symbol;
		}
		#endregion // Base overrides
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorOwner.GetErrorCollection"/>
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (filter == ModelErrorUses.None)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				FunctionDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion // IModelErrorOwner Implementation
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(ModelDefinesFunction)
		/// </summary>
		private static void FunctionAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ModelDefinesFunction)e.ModelElement).Function, DelayValidateFunction);
		}
		private static void DelayValidateFunction(ModelElement element)
		{
			// We can eventually add more validation here and add additional callers
			// as function become editable. For now, we assume that function definitions
			// immutable once they are loaded. All we do for now is determine the IsAggregate
			// setting so that we do not need to touch it more than once.
			if (!element.IsDeleted)
			{
				Function function = (Function)element;
				bool isAggregate = false;
				foreach (FunctionParameter parameter in function.ParameterCollection)
				{
					if (parameter.BagInput)
					{
						isAggregate = true;
						break;
					}
				}
				function.IsAggregate = isAggregate;
			}
		}
		#endregion // Rule Methods
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new FunctionFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class FunctionFixupListener : DeserializationFixupListener<Function>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public FunctionFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddIntrinsicElements)
			{
				// Note that we run this fixup unusually early so that it comes
				// before ReplaceDeprecatedStoredElements and we can rely on the
				// function definitions during role path validation.
			}
			/// <summary>
			/// Process Function elements
			/// </summary>
			/// <param name="element">A Function element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(Function element, Store store, INotifyElementAdded notifyAdded)
			{
				DelayValidateFunction(element);
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // Function class
	#region CalculatedPathValue class
	partial class CalculatedPathValue : IHasIndirectModelErrorOwner, IModelErrorDisplayContext
	{
		#region Accessors
		/// <summary>
		/// Get the context <see cref="ORMModel"/> for this calculation
		/// </summary>
		public ORMModel Model
		{
			get
			{
				LeadRolePath parent = LeadRolePath;
				return (parent != null) ? parent.Model : null;
			}
		}
		#endregion // Accessors
		#region Rule Methods
		/// <summary>
		/// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// When an assigned function changes, map old inputs to new inputs based on
		/// parameter position. This preserves as much data as possible for the most
		/// common cases.
		/// </summary>
		private static void FunctionChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				CalculatedPathValueIsCalculatedWithFunction link = (CalculatedPathValueIsCalculatedWithFunction)e.ElementLink;
				CalculatedPathValue calculatedValue = link.CalculatedValue;
				LinkedElementCollection<FunctionParameter> oldParameters = ((Function)e.OldRolePlayer).ParameterCollection;
				Function newFunction = link.Function;
				LinkedElementCollection<FunctionParameter> newParameters = newFunction.ParameterCollection;
				int newParameterCount = newParameters.Count;
				LinkedElementCollection<CalculatedPathValueInput> inputs = calculatedValue.InputCollection;
				for (int i = inputs.Count - 1; i >= 0; --i)
				{
					CalculatedPathValueInput input = inputs[i];
					int oldParameterIndex = oldParameters.IndexOf(input.Parameter);
					FunctionParameter newParameter;
					if (oldParameterIndex != -1 &&
						oldParameterIndex < newParameterCount &&
						(!(newParameter = newParameters[oldParameterIndex]).BagInput ||
						input.SourceConstant == null))
					{
						input.Parameter = newParameters[oldParameterIndex];
					}
					else
					{
						input.Delete();
					}
				}
				if (!newFunction.IsBoolean)
				{
					// A non-boolean function cannot be a path condition
					calculatedValue.RequiredForLeadRolePath = null;
				}
			}
			else
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEnforceInitialCalculatedValue);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
		/// </summary>
		private static void FunctionDeletedRule(ElementDeletedEventArgs e)
		{
			CalculatedPathValue calculatedValue = ((CalculatedPathValueIsCalculatedWithFunction)e.ModelElement).CalculatedValue;
			if (!calculatedValue.IsDeleted)
			{
				calculatedValue.InputCollection.Clear();

				// If there is no function, then we cannot assume that it is a boolean function
				// that is eligible to satisfy a condition
				calculatedValue.RequiredForLeadRolePath = null;
			}
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
		/// Make the four source types (path root, role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToCalculatedValueRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToCalculatedPathValue)e.ModelElement).Input;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourcePathRoot = null;
			calculatedInput.SourcePathedRole = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
		/// Make the four source types (path root, role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToConstantRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathConstant)e.ModelElement).Input;
			FunctionParameter parameter;
			if (null != (parameter = calculatedInput.Parameter) &&
				parameter.BagInput)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionPathConstantInconsistentWithBagInput);
			}
			calculatedInput.SourcePathRoot = null;
			calculatedInput.SourcePathedRole = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
		/// Make the four source types (path root, role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToPathedRoleRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToPathedRole)e.ModelElement).Input;
			calculatedInput.SourcePathRoot = null;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		/// <summary>
		/// AddRule: typeof(CalculatedPathValueInputBindsToRolePathRoot)
		/// Make the four source types (path root, role, calculated value, constant) mutually exclusive.
		/// </summary>
		private static void InputBoundToPathRootRule(ElementAddedEventArgs e)
		{
			CalculatedPathValueInput calculatedInput = ((CalculatedPathValueInputBindsToRolePathRoot)e.ModelElement).Input;
			calculatedInput.SourcePathedRole = null;
			calculatedInput.SourceConstant = null;
			calculatedInput.SourceCalculatedValue = null;
		}
		#endregion // Rule Methods
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				// UNDONE: Add more specific display context information at the calculation level
				// instead of deferring back up the parent hierarchy.
				IModelErrorDisplayContext deferTo = LeadRolePath;
				return deferTo != null ? deferTo.ErrorDisplayContext : "";
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	#endregion // CalculatedPathValue class
	#region Path Errors
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathRequiresRootObjectTypeError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = RolePath.RootRolePath;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathRequiresRootObjectType, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePathRequiresRootObjectTypeCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathSameFactTypeRoleFollowsJoinError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathSameFactTypeRoleFollowsJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePathSameFactTypeRoleFollowsJoinCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class JoinedPathRoleRequiresCompatibleRolePlayerError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathIncompatibleJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePathIncompatibleJoinCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathObjectUnifierRequiresCompatibleObjectTypesError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = ObjectUnifier;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePathObjectUnifierIncompatibleCorrelation, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePathObjectUnifierIncompatibleCorrelationCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PathOuterJoinRequiresOptionalRoleError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = PathedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture,ResourceStrings.ModelErrorRolePathMandatoryOuterJoin, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePathMandatoryOuterJoinCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueRequiresFunctionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = CalculatedPathValue;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueRequiresFunction, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorCalculatedPathValueRequiresFunctionCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueRequiresAggregationContextError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = CalculatedPathValue;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueRequiresAggregationContext, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorCalculatedPathValueRequiresAggregationContextCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueParameterBindingError
	{
		/// <summary>
		/// Get the associated <see cref="FunctionParameter"/>
		/// </summary>
		public FunctionParameter Parameter
		{
			get
			{
				// The function parameter is on the owning link to
				// assist with event handling.
				CalculatedPathValueHasUnboundParameterError errorLink = CalculatedPathValueHasUnboundParameterError.GetLinkToCalculatedPathValue(this);
				return errorLink != null ? errorLink.Parameter : null;
			}
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			CalculatedPathValueHasUnboundParameterError errorLink = CalculatedPathValueHasUnboundParameterError.GetLinkToCalculatedPathValue(this);
			IModelErrorDisplayContext displayContext = errorLink.CalculatedPathValue;
			FunctionParameter parameter = errorLink.Parameter;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueParameterBinding, displayContext != null ? displayContext.ErrorDisplayContext : "", parameter != null ? parameter.Name : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				CalculatedPathValueHasUnboundParameterError errorLink = CalculatedPathValueHasUnboundParameterError.GetLinkToCalculatedPathValue(this);
				FunctionParameter parameter = errorLink.Parameter;
				return string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueParameterBindingCompact, parameter != null ? parameter.Name : "");
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class CalculatedPathValueMustBeConsumedError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = CalculatedPathValue;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorCalculatedPathValueMustBeConsumed, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorCalculatedPathValueMustBeConsumedCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class RoleProjectedDerivationRequiresProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = (IModelErrorDisplayContext)DerivationRule;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRoleProjectedDerivationRuleProjectionRequired, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRoleProjectedDerivationRuleProjectionRequiredCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PartialRoleSetDerivationProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = DerivationProjection;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRoleProjectedDerivationRulePartialProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRoleProjectedDerivationRulePartialProjectionCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class DerivedRoleRequiresCompatibleProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = Projection.ProjectedRole;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorDerivedRoleProjectionIncompatibleProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorDerivedRoleProjectionIncompatibleProjectionCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class ConstraintRoleSequenceJoinPathRequiresProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = JoinPath;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorJoinPathProjectionRequired, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorJoinPathProjectionRequiredCompact;
			}
		}
	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class PartialConstraintRoleSequenceJoinPathProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = JoinPathProjection;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorJoinPathPartialProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorJoinPathPartialProjectionCompact;
			}
		}

	}
	[ModelErrorDisplayFilter(typeof(RolePathErrorCategory))]
	partial class ConstraintRoleRequiresCompatibleJoinPathProjectionError
	{
		/// <summary>
		/// Standard override
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		/// <summary>
		/// Generate the error text
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = Projection.JoinPathProjection;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintRoleProjectionIncompatibleProjection, displayContext != null ? displayContext.ErrorDisplayContext : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorConstraintRoleProjectionIncompatibleProjectionCompact;
			}
		}
	}
	#endregion // Path Errors
	#region FactTypeDerivationExpression class (transitional)
	// Transitional code to move all information stored in the
	// old derivation expression elements into the new derivation
	// path elements. The old expression elements are removed when
	// the file is loaded.
	partial class FactTypeDerivationExpression
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DerivationRuleFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class DerivationRuleFixupListener : DeserializationFixupListener<FactTypeHasDerivationExpression>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public DerivationRuleFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Process FactTypeHasDerivationExpression elements
			/// </summary>
			/// <param name="element">An FactTypeHasDerivationExpression element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactTypeHasDerivationExpression element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					FactType factType = element.FactType;
					Partition partition = factType.Partition;
					FactTypeDerivationRule derivationRule = factType.DerivationRule as FactTypeDerivationRule;
					FactTypeDerivationExpression derivationExpression = element.DerivationRule;
					string expressionBody = derivationExpression.Body;
					DerivationNote derivationNote;
					if (derivationRule != null)
					{
						derivationNote = derivationRule.DerivationNote;
					}
					else
					{
						notifyAdded.ElementAdded(derivationRule = new FactTypeDerivationRule(
							partition,
							new PropertyAssignment(FactTypeDerivationRule.ExternalDerivationDomainPropertyId, true)));
						notifyAdded.ElementAdded(new FactTypeHasDerivationRule(factType, derivationRule));
						derivationNote = null;
					}

					// Expressions were around before rules, so we synchronize the two part rule values
					// with the expression storage settings.
					switch (derivationExpression.DerivationStorage)
					{
						case DerivationExpressionStorageType.Derived:
							derivationRule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
							break;
						case DerivationExpressionStorageType.DerivedAndStored:
							derivationRule.DerivationCompleteness = DerivationCompleteness.FullyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
							break;
						case DerivationExpressionStorageType.PartiallyDerived:
							derivationRule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.NotStored;
							break;
						case DerivationExpressionStorageType.PartiallyDerivedAndStored:
							derivationRule.DerivationCompleteness = DerivationCompleteness.PartiallyDerived;
							derivationRule.DerivationStorage = ObjectModel.DerivationStorage.Stored;
							break;
					}

					// Migrate settings from the expression body
					if (!string.IsNullOrEmpty(expressionBody))
					{
						if (derivationNote == null)
						{
							notifyAdded.ElementAdded(derivationNote = new DerivationNote(
								partition,
								new PropertyAssignment(DerivationNote.BodyDomainPropertyId, expressionBody)));
							notifyAdded.ElementAdded(new FactTypeDerivationRuleHasDerivationNote(derivationRule, derivationNote));
						}
						else
						{
							string existingBody = derivationNote.Body;
							derivationNote.Body = string.IsNullOrEmpty(existingBody) ? expressionBody : existingBody + "\r\n" + expressionBody;
						}
					}

					// Remove the deprecated expression
					element.Delete();
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(FactTypeDerivationExpression)
		/// </summary>
		private static void DeprecateFactTypeDerivationExpression(ElementAddedEventArgs e)
		{
			// There is no need to localize this, we'll pull it altogether on the next file format upgrade.
			throw new InvalidOperationException("FactTypeDerivationExpression is deprecated. Use FactTypeDerivationRule instead.");
		}
		#endregion // Rule Methods
	}
	#endregion // FactTypeDerivationExpression class
	#region SubtypeDerivationExpression class (transitional)
	// Transitional code to move all information stored in the
	// old derivation expression elements into the new derivation
	// path elements. The old expression elements are removed when
	// the file is loaded.
	partial class SubtypeDerivationExpression
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that the two derivation storage types are in sync.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new DerivationRuleFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation.
		/// </summary>
		private sealed class DerivationRuleFixupListener : DeserializationFixupListener<SubtypeHasDerivationExpression>
		{
			/// <summary>
			/// DerivationRuleFixupListener constructor
			/// </summary>
			public DerivationRuleFixupListener()
				: base((int)ORMDeserializationFixupPhase.ReplaceDeprecatedStoredElements)
			{
			}
			/// <summary>
			/// Process SubtypeHasDerivationExpression elements
			/// </summary>
			/// <param name="element">An SubtypeHasDerivationExpression element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(SubtypeHasDerivationExpression element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					ObjectType subType = element.Subtype;
					SubtypeDerivationExpression derivationExpression = element.DerivationRule;
					string expressionBody = derivationExpression.Body;
					if (!string.IsNullOrEmpty(expressionBody))
					{
						SubtypeDerivationRule derivationRule = subType.DerivationRule;
						DerivationNote derivationNote;
						Partition partition = subType.Partition;
						if (derivationRule != null)
						{
							derivationNote = derivationRule.DerivationNote;
						}
						else
						{
							notifyAdded.ElementAdded(derivationRule = new SubtypeDerivationRule(
								partition,
								new PropertyAssignment(SubtypeDerivationRule.ExternalDerivationDomainPropertyId, true)));
							notifyAdded.ElementAdded(new SubtypeHasDerivationRule(subType, derivationRule));
							derivationNote = null;
						}
						if (derivationNote == null)
						{
							notifyAdded.ElementAdded(derivationNote = new DerivationNote(
								partition,
								new PropertyAssignment(DerivationNote.BodyDomainPropertyId, expressionBody)));
							notifyAdded.ElementAdded(new SubtypeDerivationRuleHasDerivationNote(derivationRule, derivationNote));
						}
						else
						{
							string existingBody = derivationNote.Body;
							derivationNote.Body = string.IsNullOrEmpty(existingBody) ? expressionBody : existingBody + "\r\n" + expressionBody;
						}
					}

					// Remove the deprecated expression
					element.Delete();
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(SubtypeDerivationExpression)
		/// </summary>
		private static void DeprecateSubtypeDerivationExpression(ElementAddedEventArgs e)
		{
			// There is no need to localize this, we'll pull it altogether on the next file format upgrade.
			throw new InvalidOperationException("SubtypeDerivationExpression is deprecated. Use SubtypeDerivationRule instead.");
		}
		#endregion // Rule Methods
	}
	#endregion // SubtypeDerivationExpression class
	#region DerivationNote class
	partial class DerivationNote
	{
		#region Rule Methods
		/// <summary>
		/// ChangeRule: typeof(DerivationNote)
		/// Delete a <see cref="DerivationNote"/> if the <see cref="Expression.Body"/> property is empty.
		/// </summary>
		private static void DerivationNoteChangedRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == DerivationNote.BodyDomainPropertyId &&
				string.IsNullOrEmpty((string)e.NewValue))
			{
				e.ModelElement.Delete();
			}
		}
		#endregion // Rule Methods
	}
	#endregion // DerivationNote class
}
