#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class ReadingOrder
	{
		private Reading primaryReading = null;

		#region Reading facade method
		/// <summary>
		/// Adds a reading to the fact.
		/// </summary>
		/// <param name="readingText">The text of the reading to add.</param>
		/// <returns>The reading that was added.</returns>
		public Reading AddReading(string readingText)
		{
			RoleMoveableCollection factRoles = RoleCollection;
			int roleCount = factRoles.Count;
			if (!Reading.IsValidReadingText(readingText, roleCount))
			{
				throw new ArgumentException(ResourceStrings.ModelExceptionFactAddReadingInvalidReadingText, "readingText");
			}

			Store theStore = this.Store;
			//TODO:determine which reading order matches the current display order
			bool setIsPrimary = ReadingCollection.Count == 0;
			AttributeAssignment[] attrList = new AttributeAssignment[setIsPrimary ? 2 : 1];

			attrList[0] = new AttributeAssignment(Reading.TextMetaAttributeGuid, readingText, theStore);
			if (setIsPrimary)
			{
				attrList[1] = new AttributeAssignment(Reading.IsPrimaryMetaAttributeGuid, true, theStore);
			}

			Reading retval = Reading.CreateAndInitializeReading(theStore, attrList);

			return retval;
		}
		#endregion // Reading facade method

		#region CustomStoredAttribute handling
		/// <summary>
		/// Currently only handles when the ReadingText value is accessed.
		/// </summary>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			object retval = null;
			if (attribute.Id == ReadingTextMetaAttributeGuid)
			{
				ReadingMoveableCollection readings = ReadingCollection;
				if (readings.Count == 0)
				{
					retval = String.Empty;
				}
				else
				{
					retval = PrimaryReading.Text;
				}
			}
			else
			{
				retval = base.GetValueForCustomStoredAttribute(attribute);
			}
			return retval;
		}

		/// <summary>
		/// Currently only handles when the ReadingText is set.
		/// </summary>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			if (attribute.Id == ReadingTextMetaAttributeGuid)
			{
				ReadingMoveableCollection readings = ReadingCollection;
				if (readings.Count > 0)
				{
					PrimaryReading.Text = (string)newValue;
				}
			}
			else
			{
				base.SetValueForCustomStoredAttribute(attribute, newValue);
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
				Reading retval = null;
				if (primaryReading == null)
				{
					ReadingMoveableCollection readings = this.ReadingCollection;
					int numReadings = readings.Count;
					for (int i = 0; i < numReadings && retval == null; ++i)
					{
						if (readings[i].IsPrimary) retval = readings[i];
					}
					if (retval == null && numReadings > 0)
					{
						readings[0].IsPrimary = true;
						retval = readings[0];
					}
				}
				else
				{
					retval = primaryReading;
				}
				return retval;
			}
		}
		/// <summary>
		/// Invalidates the cached value of the primary reading.
		/// </summary>
		private void InvalidatePrimaryReading()
		{
			primaryReading = null;
		}
		#endregion

		#region ReadingOrderHasReading rule classes
		[RuleOn(typeof(ReadingOrderHasReading))]
		private class ReadingOrderHasReadingAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
				ReadingOrder theReadingOrder = link.ReadingOrder;
				ReadingMoveableCollection factReadings = theReadingOrder.ReadingCollection;
				int roleCount = factReadings.Count;
				ReadingMoveableCollection readings = theReadingOrder.ReadingCollection;
				if (readings.Count == 1)
				{
					Reading onlyReading = readings[0];
					if (!onlyReading.IsPrimary)
					{
						onlyReading.IsPrimary = true;
					}
				}
				else
				{
					//if more than one reading and the new one is set to be the
					//primary one then setting any others that are primary to false.
					Reading newReading = link.ReadingCollection;
					if (newReading.IsPrimary)
					{
						Reading r;
						for (int i = 0; i < roleCount; ++i)
						{
							r = factReadings[i];
							if (!object.ReferenceEquals(r, newReading))
							{
								if (r.IsPrimary)
								{
									r.IsPrimary = false;
									//UNDONE:break? should only be one.
								}
							}
						}
					}
				}
			}
		}

		[RuleOn(typeof(ReadingOrderHasReading))]
		private class ReadingOrderHasReadingRemoved : RemoveRule
		{
			/// <summary>
			/// deals with the primary reading being removed by selecting the first
			/// reading in the list if there are any left.
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				//TODO:test
				ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
				ReadingOrder readOrd = link.ReadingOrder;
				Reading read = link.ReadingCollection;
				if (read.IsPrimary)
				{
					ReadingMoveableCollection allReadings = readOrd.ReadingCollection;
					if (allReadings.Count > 0)
					{
						allReadings[0].IsPrimary = true;
					}
				}
			}
		}

		#endregion FactTypeReadingRoleRemoved rule class

		#region ReadingOrderHasRoleRemoving rule class
		/// <summary>
		/// Handles the clean up of the readings that the role is involved in by replacing
		/// the place holder with the text {{deleted}}
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasRole))]
		private class ReadingOrderHasRoleRemoving : RemovingRule
		{
			//TODO:test
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
				Role linkRole = link.RoleCollection;
				ReadingOrder linkReadingOrder = link.ReadingOrder;

				if (linkReadingOrder.IsRemoving)
				{
					// Don't validate if we're removing the reading order
					return;
				}
				Debug.Assert(!linkReadingOrder.IsRemoved);

				int pos = linkReadingOrder.RoleCollection.IndexOf(linkRole);
				if (pos >= 0)
				{
					// UNDONE: This could be done much cleaner with RegEx.Replace and a callback
					ReadingMoveableCollection readings = linkReadingOrder.ReadingCollection;
					int numReadings = readings.Count;
					for (int iReading = 0; iReading < numReadings; ++iReading)
					{
						Reading linkReading = linkReadingOrder.ReadingCollection[iReading];

						if (!linkReading.IsRemoving)
						{
							Debug.Assert(!linkReading.IsRemoved);
							string text = linkReading.Text;
							text = text.Replace("{" + pos.ToString() + "}", "{{deleted}}");
							int roleCount = linkReading.ReadingOrder.RoleCollection.Count;
							for (int i = pos + 1; i < roleCount; ++i)
							{
								text = text.Replace(string.Concat("{", i.ToString(), "}"), string.Concat("{", (i - 1).ToString(), "}"));
							}
							linkReading.Text = text;
						}
					}
				}
			}
		}
		#endregion ReadingHasRoleRemoving
	}
}
