#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;

#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	public partial class Reading
	{
		/// <summary>
		/// The filled version of the reading. Currently works on the assumption that the arity of
		/// the statement is equal to the number of roles. The text is expected to have {n} where
		/// n is the 0 based position of the predicate to substitute.
		/// 
		/// "{0} has {1}" with the role players Person and GivenName would be returned as
		/// "Person has GivenName"
		/// </summary>
		/// <returns>The filled version of the reading.</returns>
		public override string ToString()
		{
			//depending on what the members of ReadingRoleCollection get turned into
			//as strings this will likely need to change.
			if (IsValidReadingText())
			{
				RoleMoveableCollection roles = RoleCollection;
				int roleCount = roles.Count;
				string[] roleNames = new string[roleCount];
				for (int i = 0; i < roleCount; ++i)
				{
					roleNames[i] = roles[i].Name;
				}
				return string.Format(this.Text, roleNames);
			}
			else
			{
				return this.Text;
			}
		}

		/// <summary>
		/// Standard override. Determines when derived properties are read-only. Called
		/// if the ReadOnly setting on the element is one of the SometimesUIReadOnly* values.
		/// Currently, IsPrimary is read-only if true.
		/// </summary>
		/// <param name="propertyDescriptor">PropertyDescriptor</param>
		/// <returns></returns>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			//TODO:test
			ElementPropertyDescriptor elemDesc = propertyDescriptor as ElementPropertyDescriptor;
			if (elemDesc != null && elemDesc.MetaAttributeInfo.Id == IsPrimaryMetaAttributeGuid)
			{
				return IsPrimary;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		/// <summary>
		/// Identify the reading for the property grid component dropdown
		/// </summary>
		/// <returns></returns>
		public override string GetComponentName()
		{
			FactType fact = FactType;
			if (fact != null)
			{
				return string.Format("{0} Reading {1}", fact.Name, fact.ReadingCollection.IndexOf(this) + 1); // UNDONE: Localize
			}
			return base.GetComponentName();
		}
		#region IsValidReadingText methods
		/// <summary>
		/// Does some testing to see if the reading text is appropriate for the current
		/// set of roles assigned to it. Would eventually be more useful if it provided
		/// some information as to why it was not valid.
		/// </summary>
		/// <returns>True if the reading is deemed valid, false otherwise.</returns>
		public bool IsValidReadingText()
		{
			Debug.Assert(FactType.RoleCollection.Count == RoleCollection.Count);
			return IsValidReadingText(Text, RoleCollection.Count);
		}

		/// <summary>
		/// Does some testing to see if the reading text is appropriate for the current
		/// set of roles assigned to it. Would eventually be more useful if it provided
		/// some information as to why it was not valid.
		/// </summary>
		/// <param name="testText">The reading text to test</param>
		/// <param name="roleCount">The number of expected role place holders</param>
		/// <returns>True if the reading is deemed valid, false otherwise.</returns>
		public static bool IsValidReadingText(string testText, int roleCount)
		{
			bool retval = true;

			//TODO:make this regex reusable
			Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");
			MatchCollection matches = regCountPlaces.Matches(testText);

			#region testing placehold and role player counts
			if (matches.Count != roleCount)
			{
				//number placeholders not the same as number of roles
				retval = false;
			}
			#endregion

			#region test placeholder number is unique and that the min/max values are valid
			int listCount = matches.Count;
			int[] nrList = new int[listCount];
			for (int i = 0; i < listCount; ++i)
			{

				nrList[i] = int.Parse(matches[i].Groups["placeHolderNr"].Value);
			}
			Array.Sort<int>(nrList);
			int last = int.MinValue;
			int min = int.MaxValue;
			int max = int.MinValue;
			for (int i = 0; i < listCount; ++i)
			{
				int nr = nrList[i];

				if (nr < min) min = nr;
				if (nr > max) max = nr;

				if (nr == last)
				{
					//duplicated a role in the placeholders
					retval = false;
					break;
				}
				else
				{
					last = nr;
				}
			}

			if (retval &&
				(min != 0 || //need to start at 0
				max != listCount - 1)) //highest should be the number of roles -1
			{

				retval = false;
			}
			#endregion // placeholder uniqueness -- min/max values

			return retval;
		}
		#endregion

		#region ReadingHasRoleRemovint rule class
		/// <summary>
		/// Handles the clean up of the readings that the role is involved in by replacing
		/// the place holder with the text {{deleted}}
		/// </summary>
		[RuleOn(typeof(ReadingHasRole))]
		private class ReadingHasRoleRemoving : RemovingRule
		{
			//TODO:test
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ReadingHasRole link = e.ModelElement as ReadingHasRole;
				Role linkRole = link.RoleCollection;
				Reading linkReading = link.ReadingCollection;

				//might want to change to reconstructing using a StringBuilder
				int pos = linkReading.RoleCollection.IndexOf(linkRole);
				if (pos >= 0)
				{
					// UNDONE: This could be done much cleaner with RegEx.Replace and a callback
					linkReading.Text = linkReading.Text.Replace("{" + pos.ToString() + "}", "{{deleted}}");
					int roleCount = linkReading.FactType.RoleCollection.Count;
					for (int i = pos + 1; i < roleCount; ++i)
					{
						linkReading.Text = linkReading.Text.Replace(string.Concat("{", i.ToString(), "}"), string.Concat("{", (i - 1).ToString(), "}"));
					}
				}
			}
		}
		#endregion ReadingHasRoleRemoving

		#region ReadingIsPrimaryChanged rule class
		/// <summary>
		/// Handles the resetting the current primary reading when a new one is selected.
		/// Also rejects trying to change the current primary reading's IsPrimary to false.
		/// </summary>
		[RuleOn(typeof(Reading))]
		private class ReadingIsPrimaryChanged : ChangeRule
		{
			//so we know when not to run code when items are being set to false
			bool mySettingNewPrimary = false;

			//TODO:test
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				Reading changedReading = e.ModelElement as Reading;
				if (attributeGuid == Reading.IsPrimaryMetaAttributeGuid)
				{
					bool newVal = (bool)e.NewValue;
					if (newVal)
					{
						Debug.Assert(!mySettingNewPrimary);
						FactType readingFact = changedReading.FactType;
						if (readingFact != null)
						{
							mySettingNewPrimary = true;
							try
							{
								foreach (Reading r in readingFact.ReadingCollection)
								{
									if (!object.ReferenceEquals(r, changedReading))
									{
										if (r.IsPrimary) r.IsPrimary = false;
										// UNDONE: Break here? This should be the only one
									}
								}
							}
							finally
							{
								mySettingNewPrimary = false;
							}
						}
					}
					else if (!mySettingNewPrimary)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReadingIsPrimaryToFalse);
					}
				}
				else if (attributeGuid == Reading.TextMetaAttributeGuid)
				{
					string newVal = e.NewValue as string;
					int roleCount = changedReading.RoleCollection.Count;

					//if text is set before roles this code will fail
					Debug.Assert(roleCount > 0);

					if (!Reading.IsValidReadingText(newVal, roleCount))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReadingTextChangeInvalid);
					}
				}
			}
		}
		#endregion ReadingIsPrimaryChanged
	}
}
