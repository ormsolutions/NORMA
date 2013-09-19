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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	public partial class ReadingOrder : IRedirectVerbalization, IHasIndirectModelErrorOwner, INamedElementDictionaryParent, INamedElementDictionaryRemoteChild
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

			Reading retVal = new Reading(Partition, new PropertyAssignment(Reading.TextDomainPropertyId, readingText));
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
				Reading reading;
				if (readings.Count > 0)
				{
					reading = readings[0];
				}
				else
				{
					reading = new Reading(Partition);
					readings.Add(reading);
				}
				reading.Text = newValue;
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
		#region EnforceNoEmptyReadingOrderDeleteRule
		/// <summary>
		/// DeleteRule: typeof(ReadingOrderHasReading), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// If the ReadingOrder.ReadingCollection is empty then remove the ReadingOrder
		/// </summary>
		private static void EnforceNoEmptyReadingOrderDeleteRule(ElementDeletedEventArgs e)
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
		#endregion // EnforceNoEmptyReadingOrderDeleteRule
		#region EnforceNoEmptyReadingOrderRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(ReadingOrderHasReading), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void EnforceNoEmptyReadingOrderRolePlayerChangeRule(RolePlayerChangedEventArgs e)
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
		#endregion // EnforceNoEmptyReadingOrderRolePlayerChangeRule
		#region ReadingOrderHasRoleDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ReadingOrderHasRole)
		/// Handles the clean up of the readings that the role is involved in by replacing
		/// the place holder with the text DELETED
		/// </summary>
		private static void ReadingOrderHasRoleDeletingRule(ElementDeletingEventArgs e)
		{
			ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
			RoleBase linkRole = link.Role;
			ReadingOrder linkReadingOrder = link.ReadingOrder;

			if (linkReadingOrder.IsDeleting || linkReadingOrder.IsDeleted)
			{
				// Don't validate if we're removing the reading order
				return;
			}
			FactType factType = linkReadingOrder.FactType;
			if (factType != null)
			{
				FrameworkDomainModel.DelayValidateElement(factType, DelayValidateReadingOrderCollation);
			}

			int pos = linkReadingOrder.RoleCollection.IndexOf(linkRole);
			if (pos >= 0)
			{
				bool isUnaryFactType = factType.UnaryRole != null;
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
						linkReading.SetAutoText(Reading.ReplaceFields(
							linkReading.Text,
							delegate(int replaceIndex)
							{
								if (replaceIndex == pos)
								{
									return isUnaryFactType ? "" : ResourceStrings.ModelReadingRoleDeletedRoleText;
								}
								else if (replaceIndex > pos)
								{
									return "{" + (replaceIndex - 1).ToString(formatProvider) + "}";
								}
								return null;
							}
							));
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
		#endregion // ReadingOrderHasRoleDeletingRule
		#region FactTypeHasRoleAddedRule
		/// <summary>
		/// Common place for code to deal with roles that exist in a fact
		/// but do not exist in the ReadingOrder objects that it contains.
		/// This allows it to be used by both the rule and to be called
		/// during post load model fixup.
		/// </summary>
		private static void ValidateReadingOrdersRoleCollection(FactType factType, RoleBase addedRole)
		{
			LinkedElementCollection<ReadingOrder> readingOrders;
			int orderCount;
			if (null == factType.UnaryRole &&
				0 != (orderCount = (readingOrders = factType.ReadingOrderCollection).Count))
			{
				bool checkedContext = false;
				bool insertAfter = false;
				RoleBase insertBeside = null;
				IFormatProvider formatProvider = CultureInfo.InvariantCulture;
				for (int i = 0; i < orderCount; ++i)
				{
					ReadingOrder ord = readingOrders[i];
					LinkedElementCollection<RoleBase> roles = ord.RoleCollection;
					if (!roles.Contains(addedRole))
					{
						if (!checkedContext)
						{
							checkedContext = true;
							Dictionary<object, object> contextInfo = factType.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
							object contextRole;
							if (contextInfo.TryGetValue(FactType.InsertAfterRoleKey, out contextRole))
							{
								insertBeside = contextRole as RoleBase;
								insertAfter = true;
							}
							else if (contextInfo.TryGetValue(FactType.InsertBeforeRoleKey, out contextRole))
							{
								insertBeside = contextRole as RoleBase;
							}
						}
						int insertIndex = -1;
						if (insertBeside != null)
						{
							insertIndex = roles.IndexOf(insertBeside);
						}

						if (insertIndex != -1)
						{
							roles.Insert(insertIndex + (insertAfter ? 1 : 0), addedRole);
						}
						else
						{
							roles.Add(addedRole);
						}
						LinkedElementCollection<Reading> readings = ord.ReadingCollection;
						int readingCount = readings.Count;
						if (readingCount != 0)
						{
							if (insertIndex == -1)
							{
								string appendText = string.Concat("  {", (roles.Count - 1).ToString(CultureInfo.InvariantCulture), "}");
								for (int j = 0; j < readingCount; ++j)
								{
									Reading reading = readings[j];
									reading.SetAutoText(reading.Text + appendText);
								}
							}
							else
							{
								for (int j = 0; j < readingCount; ++j)
								{
									Reading reading = readings[j];
									reading.SetAutoText(Reading.ReplaceFields(
										reading.Text,
										delegate(int replaceIndex)
										{
											// UNDONE: Respect leading/trailing hyphen binding and keep them associated
											// with the corresponding role. Will require work well beyond the scope of this
											// routine.
											if (replaceIndex == insertIndex)
											{
												return string.Concat("{", insertIndex.ToString(formatProvider), "} {", (insertIndex + 1).ToString(formatProvider), "}");
											}
											else if (replaceIndex > insertIndex)
											{
												return string.Concat("{", (replaceIndex + 1).ToString(formatProvider), "}");
											}
											return null; // Leave as is
										}
										));
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// Rule to detect when a Role is added to the FactType so that it
		/// can also be added to the ReadingOrders and their Readings.
		/// </summary>
		private static void FactTypeHasRoleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = (FactTypeHasRole)e.ModelElement;
			if (CopyMergeUtility.GetIntegrationPhase(link.Store) != CopyClosureIntegrationPhase.Integrating)
			{
				// UNDONE: COPYMERGE Do we need to run a similar rule on integration complete
				// to handle merge cases where roles are added to a model with additional
				// readings on the merged fact type.
				ValidateReadingOrdersRoleCollection(link.FactType, link.Role);
			}
		}
		#endregion // FactTypeHasRoleAddedRule
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
		#region INamedElementDictionaryParent implementation
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		/// <returns>Model-owned dictionary for duplicate readings</returns>
		protected INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			INamedElementDictionary dictionary = null;
			if (parentDomainRoleId == ReadingOrderHasReading.ReadingOrderDomainRoleId)
			{
				FactType factType;
				if (null != (factType = FactType))
				{
					// If the object type has an alternate owner with a dictionary, then see if that
					// owner has a dictionary that supports this relationship. Otherwise just use
					// dictionary from the model.
					IHasAlternateOwner<FactType> toAlternateOwner;
					INamedElementDictionaryParent dictionaryParent;
					ORMModel model;
					if ((null == (toAlternateOwner = factType as IHasAlternateOwner<FactType>) ||
						null == (dictionaryParent = toAlternateOwner.AlternateOwner as INamedElementDictionaryParent) ||
						null == (dictionary = dictionaryParent.GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId))) &&
						null != (model = factType.Model))
					{
						dictionary = ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
					}
				}
				if (dictionary == null)
				{
					dictionary = NamedElementDictionary.GetRemoteDictionaryToken(typeof(Reading));
				}
			}
			return dictionary;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected object GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			object retVal = null;
			Dictionary<object, object> contextInfo = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			if (!contextInfo.ContainsKey(NamedElementDictionary.DefaultAllowDuplicateNamesKey) &&
				contextInfo.ContainsKey(ORMModel.AllowDuplicateNamesKey))
			{
				// Use their value so they don't have to look up ours again
				retVal = NamedElementDictionary.AllowDuplicateNamesKey;
			}
			return retVal;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetAllowDuplicateNamesContextKey(parentDomainRoleId, childDomainRoleId);
		}
		#endregion // INamedElementDictionaryParent implementation
		#region INamedElementDictionaryRemoteChild implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryChildRoles = new Guid[] { ReadingOrderHasReading.ReadingOrderDomainRoleId };
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryRemoteChild.GetNamedElementDictionaryChildRoles"/>.
		/// </summary>
		/// <returns>Guid for the ReadingOrderHasReading.ReadingOrder role</returns>
		protected static Guid[] GetNamedElementDictionaryChildRoles()
		{
			return myRemoteNamedElementDictionaryChildRoles;
		}
		Guid[] INamedElementDictionaryRemoteChild.GetNamedElementDictionaryChildRoles()
		{
			return GetNamedElementDictionaryChildRoles();
		}
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryRemoteChild.NamedElementDictionaryParentRole"/>
		/// </summary>
		protected static Guid NamedElementDictionaryParentRole
		{
			get
			{
				return FactTypeHasReadingOrder.ReadingOrderDomainRoleId;
			}
		}
		Guid INamedElementDictionaryRemoteChild.NamedElementDictionaryParentRole
		{
			get
			{
				return NamedElementDictionaryParentRole;
			}
		}
		#endregion // INamedElementDictionaryRemoteChild implementation
	}
}
