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

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using System.Collections.ObjectModel;

#endregion

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ReadingOrder : IRedirectVerbalization, IHasIndirectModelErrorOwner
	{
		#region Reading facade method
		/// <summary>
		/// Adds a reading to the fact.
		/// </summary>
		/// <param name="readingText">The text of the reading to add.</param>
		/// <returns>The reading that was added.</returns>
		public Reading AddReading(string readingText)
		{
			LinkedElementCollection<RoleBase> factRoles = RoleCollection;
			int roleCount = factRoles.Count;
			if (!Reading.IsValidReadingText(readingText, roleCount))
			{
				throw new ArgumentException(ResourceStrings.ModelExceptionFactAddReadingInvalidReadingText, "readingText");
			}

			Store store = Store;
			Reading retVal = new Reading(store, new PropertyAssignment(Reading.TextDomainPropertyId, readingText));
			retVal.ReadingOrder = this;
			return retVal;
		}
		#endregion // Reading facade method
		#region CustomStoredAttribute handling
		private string GetReadingTextValue()
		{
			LinkedElementCollection<Reading> readings = ReadingCollection;
			return (readings.Count == 0) ? String.Empty : readings[0].Text;
		}
		private void SetReadingTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				LinkedElementCollection<Reading> readings = ReadingCollection;
				if (readings.Count > 0)
				{
					readings[0].Text = newValue;
				}
			}
		}
		#endregion
		#region PrimaryReading property and helpers
		/// <summary>
		/// An alternate means of setting and retrieving which reading is primary.
		/// </summary>
		/// <value>The primary Reading.</value>
		public Reading PrimaryReading
		{
			get
			{
				LinkedElementCollection<Reading> readings;
				if (!IsDeleted &&
					0 != (readings = ReadingCollection).Count)
				{
					return readings[0];
				}
				return null;
			}
		}
		/// <summary>
		/// Return the string of the <see cref="PrimaryReading"/> if available.
		/// </summary>
		public override string ToString()
		{
			Reading primary = PrimaryReading;
			return (primary != null) ? primary.ToString() : base.ToString();
		}
		#endregion
		#region EnforceNoEmptyReadingOrderDeleteRule rule class
		[RuleOn(typeof(ReadingOrderHasReading), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // DeleteRule
		private sealed partial class EnforceNoEmptyReadingOrderDeleteRule : DeleteRule
		{
			/// <summary>
			/// If the ReadingOrder.ReadingCollection is empty then remove the ReadingOrder
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
				ReadingOrder readOrd = link.ReadingOrder;
				if (!readOrd.IsDeleted)
				{
					if (readOrd.ReadingCollection.Count == 0)
					{
						readOrd.Delete();
					}
				}
			}
		}
		#endregion // EnforceNoEmptyReadingOrderDeleteRule rule class
		#region EnforceNoEmptyReadingOrderRolePlayerChange rule class
		[RuleOn(typeof(ReadingOrderHasReading), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // RolePlayerChangeRule
		private sealed partial class EnforceNoEmptyReadingOrderRolePlayerChange : RolePlayerChangeRule
		{
			public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				ReadingOrderHasReading link = e.ElementLink as ReadingOrderHasReading;
				if (e.DomainRole.Id == ReadingOrderHasReading.ReadingOrderDomainRoleId)
				{
					ReadingOrder order = (ReadingOrder)e.OldRolePlayer;
					if (!order.IsDeleted && order.ReadingCollection.Count == 0)
					{
						order.Delete();
					}
				}
			}
		}
		#endregion // EnforceNoEmptyReadingOrderRolePlayerChange rule class
		#region ReadingOrderHasRoleRemoving rule class
		private static Regex myIndexMapRegex;
		private static Regex IndexMapRegex
		{
			get
			{
				Regex regexIndexMap = myIndexMapRegex;
				if (regexIndexMap == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myIndexMapRegex,
						new Regex(
							@"(?n)((?<!\{)\{)(?<ReplaceIndex>\d+)(\}(?!\}))",
							RegexOptions.Compiled),
						null);
					regexIndexMap = myIndexMapRegex;
				}
				return regexIndexMap;
			}
		}
		/// <summary>
		/// Handles the clean up of the readings that the role is involved in by replacing
		/// the place holder with the text {{deleted}}
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasRole))] // DeletingRule
		private sealed partial class ReadingOrderHasRoleDeleting : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
				RoleBase linkRole = link.Role;
				ReadingOrder linkReadingOrder = link.ReadingOrder;

				if (linkReadingOrder.IsDeleting)
				{
					// Don't validate if we're removing the reading order
					return;
				}
				Debug.Assert(!linkReadingOrder.IsDeleted);
				FactType factType = linkReadingOrder.FactType;
				if (factType != null)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateReadingOrderCollation);
				}

				int pos = linkReadingOrder.RoleCollection.IndexOf(linkRole);
				if (pos >= 0)
				{
					LinkedElementCollection<Reading> readings = linkReadingOrder.ReadingCollection;
					int numReadings = readings.Count;
					int roleCount = linkReadingOrder.RoleCollection.Count;
					for (int iReading = 0; iReading < numReadings; ++iReading)
					{
						Reading linkReading = readings[iReading];

						if (!linkReading.IsDeleting)
						{
							Debug.Assert(!linkReading.IsDeleted);
							string text = linkReading.Text;
							IFormatProvider formatProvider = CultureInfo.InvariantCulture;
							linkReading.Text = IndexMapRegex.Replace(
								linkReading.Text,
								delegate(Match match)
								{
									int replaceIndex = int.Parse(match.Groups["ReplaceIndex"].Value, formatProvider);
									if (replaceIndex == pos)
									{
										return ResourceStrings.ModelReadingRoleDeletedRoleText;
									}
									else if (replaceIndex > pos)
									{
										return "{" + (replaceIndex - 1).ToString(formatProvider) + "}";
									}
									return match.Value;
								});
							//UNDONE:add entry to task list service to let user know reading text might need some fixup
						}
					}
				}
			}
		}
		/// <summary>
		/// Verify that all <see cref="ReadingOrder"/>s have unique <see cref="ReadingOrder.RoleCollection">role collections</see>
		/// </summary>
		/// <param name="element">A <see cref="FactType"/></param>
		private static void DelayValidateReadingOrderCollation(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			FactType factType = (FactType)element;
			LinkedElementCollection<ReadingOrder> ordersCollection = factType.ReadingOrderCollection;
			int orderCount = ordersCollection.Count;
			if (orderCount > 1)
			{
				// Get all orders in a collatable form, starting by caching information locally
				// so it is easily accessed. Note that this will also change the collection we're
				// iterating so we need to be careful about changes.
				ReadingOrder[] orders = new ReadingOrder[orderCount];
				ordersCollection.CopyTo(orders, 0);
				LinkedElementCollection<RoleBase>[] roleCollections = new LinkedElementCollection<RoleBase>[orderCount];
				for (int i = 0; i < orderCount; ++i)
				{
					roleCollections[i] = orders[i].RoleCollection;
				}

				// Priority is top down, so we move later readings into a higher reading order
				for (int i = 0; i < orderCount; ++i)
				{
					ReadingOrder currentOrder = orders[i];
					for (int j = i + 1; j < orderCount; ++j)
					{
						ReadingOrder compareToOrder = orders[j];
						if (compareToOrder != null) // Will be null if it has already been recognized as a duplicate
						{
							// These should all have the same count, but it doesn't hurt to be defensive
							LinkedElementCollection<RoleBase> currentRoles = roleCollections[i];
							LinkedElementCollection<RoleBase> compareToRoles = roleCollections[j];
							int roleCount = currentRoles.Count;
							if (roleCount == compareToRoles.Count)
							{
								int k = 0;
								for (; k < roleCount; ++k)
								{
									if (currentRoles[k] != compareToRoles[k])
									{
										break;
									}
								}
								if (k == roleCount)
								{
									// Order is the same, collate the later readings up to the current order
									ReadOnlyCollection<ReadingOrderHasReading> readingLinks = ReadingOrderHasReading.GetLinksToReadingCollection(compareToOrder);
									int readingCount = readingLinks.Count;
									for (int l = 0; l < readingCount; ++l)
									{
										readingLinks[l].ReadingOrder = currentOrder;
									}
									orders[j] = null;
								}
							}
						}
					}
				}
			}
		}
		#endregion // ReadingOrderHasRoleRemoving rule class
		#region FactTypeHasRoleAddedRule
		/// <summary>
		/// Common place for code to deal with roles that exist in a fact
		/// but do not exist in the ReadingOrder objects that it contains.
		/// This allows it to be used by both the rule and to be called
		/// during post load model fixup.
		/// </summary>
		private static void ValidateReadingOrdersRoleCollection(FactType theFact, RoleBase addedRole)
		{
			Debug.Assert(theFact.Store.TransactionManager.InTransaction);

			LinkedElementCollection<ReadingOrder> readingOrders = theFact.ReadingOrderCollection;
			foreach (ReadingOrder ord in readingOrders)
			{
				LinkedElementCollection<RoleBase> roles = ord.RoleCollection;
				if (!roles.Contains(addedRole))
				{
					ord.RoleCollection.Add(addedRole);
					LinkedElementCollection<Reading> readings = ord.ReadingCollection;
					int readingCount = readings.Count;
					if (readingCount != 0)
					{
						string appendText = String.Concat("  {", (roles.Count - 1).ToString(CultureInfo.InvariantCulture), "}");
						for (int i = 0; i < readingCount; ++i)
						{
							Reading reading = readings[i];
							reading.Text = reading.Text + appendText;
						}
					}
				}
			}
		}

		/// <summary>
		/// Rule to detect when a Role is added to the FactType so that it
		/// can also be added to the ReadingOrders and their Readings.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // AddRule
		private sealed partial class FactTypeHasRoleAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				ValidateReadingOrdersRoleCollection(link.FactType, link.Role);
			}
		}
		#endregion
		#region IRedirectVerbalization Implementation
		/// <summary>
		/// Implements IRedirectVerbalization.SurrogateVerbalizer by deferring to the parent fact
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				return FactType as IVerbalize;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { FactTypeHasReadingOrder.ReadingOrderDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
}
