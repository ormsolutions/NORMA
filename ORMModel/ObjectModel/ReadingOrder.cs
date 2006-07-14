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
			LinkedElementCollection<Reading> readings = ReadingCollection;
			if (readings.Count > 0)
			{
				readings[0].Text = newValue;
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
		#endregion
		#region EnforceNoEmptyReadingOrder rule class
		[RuleOn(typeof(ReadingOrderHasReading), FireTime = TimeToFire.LocalCommit)]
		private sealed class EnforceNoEmptyReadingOrder : DeleteRule
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
		#endregion // EnforceNoEmptyReadingOrder rule class
		#region ReadingOrderHasRoleRemoving rule class
		/// <summary>
		/// Handles the clean up of the readings that the role is involved in by replacing
		/// the place holder with the text {{deleted}}
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasRole))]
		private sealed class ReadingOrderHasRoleDeleting : DeletingRule
		{
			//UNDONE:a role being removed creates the possibility of there being two ReadingOrders with the same Role sequences, they should be merged
			
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

				int pos = linkReadingOrder.RoleCollection.IndexOf(linkRole);
				if (pos >= 0)
				{
					// UNDONE: This could be done much cleaner with RegEx.Replace and a callback
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
							text = text.Replace("{" + pos.ToString(CultureInfo.InvariantCulture) + "}", ResourceStrings.ModelReadingRoleDeletedRoleText);
							for (int i = pos + 1; i < roleCount; ++i)
							{
								text = text.Replace(string.Concat("{", i.ToString(CultureInfo.InvariantCulture), "}"), string.Concat("{", (i - 1).ToString(CultureInfo.InvariantCulture), "}"));
							}
							linkReading.Text = text;
							//UNDONE:add entry to task list service to let user know reading text might need some fixup
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

			string deletedText = ResourceStrings.ModelReadingRoleDeletedRoleText;
			// TODO: escape the deletedText for any Regex text, since it's localizable
			Regex regExDeleted = new Regex(deletedText, RegexOptions.Compiled);

			LinkedElementCollection<ReadingOrder> readingOrders = theFact.ReadingOrderCollection;
			foreach (ReadingOrder ord in readingOrders)
			{
				LinkedElementCollection<RoleBase> roles = ord.RoleCollection;
				if (!roles.Contains(addedRole))
				{
					ord.RoleCollection.Add(addedRole);
					LinkedElementCollection<Reading> readings = ord.ReadingCollection;
					foreach (Reading read in readings)
					{
						string readingText = read.Text;
						
						int pos = readingText.IndexOf(deletedText);
						string newText;
						if (pos < 0)
						{
							newText = String.Concat(readingText, "{", roles.Count - 1, "}");
						}
						else
						{
							newText = regExDeleted.Replace(readingText, string.Concat("{", roles.Count - 1, "}"), 1);
						}
						//UNDONE:add entries to the task list service to let user know the reading might need some correction

						read.Text = newText;
					}
				}
			}
		}

		/// <summary>
		/// Rule to detect when a Role is added to the FactType so that it
		/// can also be added to the ReadingOrders and their Readings.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private sealed class FactTypeHasRoleAddedRule : AddRule
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
