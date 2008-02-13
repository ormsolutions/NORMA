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
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling;
namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	partial class ReferenceConstraint
	{
		#region Rule methods
		/// <summary>
		/// AddRule: typeof(ReferenceConstraintTargetsTable)
		/// Wire delayed verification to bind any reference constraint to an
		/// existing uniqueness constraint in the target table
		/// </summary>
		private static void ReferenceConstraintTargetAdded(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(e.ModelElement, DelayValidateTargetUniquenessConstraint);
		}
		private static void DelayValidateTargetUniquenessConstraint(ModelElement element)
		{
			ValidateTargetUniquenessConstraint((ReferenceConstraintTargetsTable)element, null);
		}
		private static void ValidateTargetUniquenessConstraint(ReferenceConstraintTargetsTable tableReference, INotifyElementAdded notifyAdded)
		{
			if (tableReference.IsDeleted)
			{
				return;
			}
			Table targetTable = tableReference.TargetTable;
			ReferenceConstraint referenceConstraint = tableReference.ReferenceConstraint;
			LinkedElementCollection<ColumnReference> columnRefs = referenceConstraint.ColumnReferenceCollection;
			int columnRefCount = columnRefs.Count;
			ReferenceConstraintTargetsUniquenessConstraint targetUniquenessLink = ReferenceConstraintTargetsUniquenessConstraint.GetLinkToTargetUniquenessConstraint(referenceConstraint);
			if (targetUniquenessLink != null)
			{
				// Verify the target uniqueness
				UniquenessConstraint targetUniqueness = targetUniquenessLink.TargetUniquenessConstraint;
				bool existingIsValid = targetUniqueness.Table == targetTable;
				if (existingIsValid)
				{
					LinkedElementCollection<Column> uniquenessColumns = targetUniqueness.ColumnCollection;
					existingIsValid = uniquenessColumns.Count == columnRefCount;
					if (existingIsValid)
					{
						for (int i = 0; i < columnRefCount; ++i)
						{
							if (uniquenessColumns[i] != columnRefs[i].TargetColumn)
							{
								existingIsValid = false;
								break;
							}
						}
					}
				}
				if (!existingIsValid)
				{
					// Delete the link without deleting the reference constraint we're processing
					targetUniquenessLink.Delete(ReferenceConstraintTargetsUniquenessConstraint.ReferenceConstraintDomainRoleId);
					targetUniquenessLink = null;
				}
			}
			if (targetUniquenessLink == null)
			{
				foreach (UniquenessConstraint testUniqueness in targetTable.UniquenessConstraintCollection)
				{
					LinkedElementCollection<Column> uniquenessColumns = testUniqueness.ColumnCollection;
					if (uniquenessColumns.Count == columnRefCount)
					{
						int i = 0;
						for (; i < columnRefCount; ++i)
						{
							if (uniquenessColumns[i] != columnRefs[i].TargetColumn)
							{
								break;
							}
						}
						if (i == columnRefCount)
						{
							// We have a match
							targetUniquenessLink = new ReferenceConstraintTargetsUniquenessConstraint(referenceConstraint, testUniqueness);
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(targetUniquenessLink, false);
							}
							break;
						}
					}
				}
				if (targetUniquenessLink == null)
				{
					referenceConstraint.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(UniquenessConstraintIncludesColumn)
		/// Verify any related <see cref="ReferenceConstraint"/> when a new
		/// column is added to a uniqueness constraint. The reference constraint
		/// needs to be deleted if the calling code has not made a matching modification
		/// by the end of the transaction.
		/// </summary>
		private static void UniquenessConstraintColumnAdded(ElementAddedEventArgs e)
		{
			foreach (ReferenceConstraint referenceConstraint in ReferenceConstraintTargetsUniquenessConstraint.GetReferenceConstraintCollection(((UniquenessConstraintIncludesColumn)e.ModelElement).UniquenessConstraint))
			{
				ReferenceConstraintTargetsTable tableLink = ReferenceConstraintTargetsTable.GetLinkToTargetTable(referenceConstraint);
				if (tableLink != null)
				{
					FrameworkDomainModel.DelayValidateElement(tableLink, DelayValidateTargetUniquenessConstraint);
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(UniquenessConstraintIncludesColumn)
		/// If a uniqueness constraint column is being removed, then remove the corresponding column
		/// in any reference constraints that target the columns in the uniqueness constraint.
		/// </summary>
		private static void UniquenessConstraintColumnDeleting(ElementDeletingEventArgs e)
		{
			UniquenessConstraintIncludesColumn link = (UniquenessConstraintIncludesColumn)e.ModelElement;
			UniquenessConstraint uniquenessConstraint = link.UniquenessConstraint;
			if (!uniquenessConstraint.IsDeleting)
			{
				bool haveIndex = false;
				int index = -1;
				foreach (ReferenceConstraint referenceConstraint in uniquenessConstraint.ReferenceConstraintCollection)
				{
					if (!haveIndex)
					{
						haveIndex = true;
						index = UniquenessConstraintIncludesColumn.GetLinksToColumnCollection(uniquenessConstraint).IndexOf(link);
					}
					LinkedElementCollection<ColumnReference> columnRefs = referenceConstraint.ColumnReferenceCollection;
					if (index < columnRefs.Count && columnRefs[index].TargetColumn == link.Column) // Sanity check
					{
						columnRefs.RemoveAt(index);
					}
					else
					{
						// Something is off, delay validate this later
						ReferenceConstraintTargetsTable tableLink = ReferenceConstraintTargetsTable.GetLinkToTargetTable(referenceConstraint);
						if (tableLink != null)
						{
							FrameworkDomainModel.DelayValidateElement(tableLink, DelayValidateTargetUniquenessConstraint);
						}
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(UniquenessConstraintIncludesColumn)
		/// If a uniqueness constraint column is being repositioned, then reposition the corresponding column
		/// in any reference constraints that target the columns in the uniqueness constraint.
		/// </summary>
		private static void UniquenessConstraintColumnPositionChanged(RolePlayerOrderChangedEventArgs e)
		{
			if (e.CounterpartDomainRole.Id == UniquenessConstraintIncludesColumn.ColumnDomainRoleId)
			{
				UniquenessConstraint uniquenessConstraint = (UniquenessConstraint)e.SourceElement;
				int oldIndex = e.OldOrdinal;
				int newIndex = e.NewOrdinal;
				foreach (ReferenceConstraint referenceConstraint in uniquenessConstraint.ReferenceConstraintCollection)
				{
					LinkedElementCollection<ColumnReference> columnRefs = referenceConstraint.ColumnReferenceCollection;
					int columnRefCount = columnRefs.Count;
					if (newIndex < columnRefCount && oldIndex < columnRefCount && columnRefs[oldIndex].TargetColumn == e.CounterpartRolePlayer) // Sanity check
					{
						columnRefs.Move(oldIndex, newIndex);
					}
					else
					{
						// Something is off, delay validate this later
						ReferenceConstraintTargetsTable tableLink = ReferenceConstraintTargetsTable.GetLinkToTargetTable(referenceConstraint);
						if (tableLink != null)
						{
							FrameworkDomainModel.DelayValidateElement(tableLink, DelayValidateTargetUniquenessConstraint);
						}
					}
				}
			}
		}
		#endregion // Rule methods
		#region Deserialization Fixup
		/// <summary>
		/// A <see cref="IDeserializationFixupListener"/> to associate reference constraints
		/// with uniqueness constraints in the target table.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ReferenceConstraintFixupListener();
			}
		}

		private class ReferenceConstraintFixupListener : DeserializationFixupListener<ReferenceConstraintTargetsTable>
		{
			/// <summary>
			/// Create a new fixup listener
			/// </summary>
			public ReferenceConstraintFixupListener()
				: base((int)ConceptualDatabaseDeserializationFixupPhase.ValidateConstraints)
			{
			}
			/// <summary>
			/// Verify that a reference constraint is bound to a uniqueness constraint in its target table
			/// </summary>
			protected override void ProcessElement(ReferenceConstraintTargetsTable element, Store store, INotifyElementAdded notifyAdded)
			{
				ValidateTargetUniquenessConstraint(element, notifyAdded);
			}
		}
		#endregion // Deserialization Fixup
	}
}
