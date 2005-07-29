using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Globalization;
using Northface.Tools.ORM.Framework;
namespace Northface.Tools.ORM.ObjectModel
{
	public partial class Reading : IModelErrorOwner
	{
		private static Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");

		#region overrides
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
				RoleMoveableCollection roles = ReadingOrder.RoleCollection;
				int roleCount = roles.Count;
				string[] roleNames = new string[roleCount];
				for (int i = 0; i < roleCount; ++i)
				{
					roleNames[i] = roles[i].Name;
				}
				return string.Format(CultureInfo.InvariantCulture, this.Text, roleNames);
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
			ReadingOrder readOrd = ReadingOrder;
			if (readOrd != null)
			{
				// UNDONE: Localize the format string
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}{2}", readOrd.FactType.Name, ResourceStrings.ReadingType, readOrd.ReadingCollection.IndexOf(this) + 1);
			}
			return base.GetComponentName();
		}
		#endregion

		#region IsValidReadingText methods
		/// <summary>
		/// Does some testing to see if the reading text is appropriate for the current
		/// set of roles assigned to it. Would eventually be more useful if it provided
		/// some information as to why it was not valid.
		/// </summary>
		/// <returns>True if the reading is deemed valid, false otherwise.</returns>
		public bool IsValidReadingText()
		{
			ReadingOrder readOrd = ReadingOrder;
			RoleMoveableCollection roles = readOrd.RoleCollection;
//			Debug.Assert(readOrd.FactType.RoleCollection.Count == roles.Count);
			return IsValidReadingText(Text, roles.Count);
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

				nrList[i] = int.Parse(matches[i].Groups["placeHolderNr"].Value, CultureInfo.InvariantCulture);
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

		/// <summary>
		/// Determines how many role placeholders are indicated in the indicated text
		/// and returns the count.
		/// </summary>
		public static int PlaceholderCount(string textText)
		{
			return regCountPlaces.Matches(textText).Count;
		}
		#endregion

		#region rule classes and helpers

		/// <summary>
		/// Compares number of roles in ReadingOrder ot the place holders in
		/// the reading and then creates or removes errors as needed.
		/// </summary>
		private void ValidateRoleCountError(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool removeTooFew = false;
				bool removeTooMany = false;
				TooFewReadingRolesError tooFewError;
				TooManyReadingRolesError tooManyError;
				ORMModel theModel = ReadingOrder.FactType.Model;
				Store store = Store;
				int numRoles = ReadingOrder.RoleCollection.Count;
				int numPlaces = Reading.PlaceholderCount(Text);

				if (numRoles == numPlaces)
				{
					removeTooFew = true;
					removeTooMany = true;
				}
				//too few roles
				else if (numRoles < numPlaces)
				{
					removeTooMany = true;
					if (null == TooFewRolesError)
					{
						tooFewError = TooFewReadingRolesError.CreateTooFewReadingRolesError(store);
						tooFewError.Model = theModel;
						tooFewError.Reading = this;
						tooFewError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooFewError, true);
						}
					}
				}
				//too many roles
				else
				{
					removeTooFew = true;
					if (null == TooManyRolesError)
					{
						tooManyError = TooManyReadingRolesError.CreateTooManyReadingRolesError(store);
						tooManyError.Model = theModel;
						tooManyError.Reading = this;
						tooManyError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooManyError, true);
						}
					}
				}

				if (removeTooFew && null != (tooFewError = TooFewRolesError))
				{
					tooFewError.Remove();
				}
				if (removeTooMany && null != (tooManyError = TooManyRolesError))
				{
					tooManyError.Remove();
				}
			}
		}

		#region ReadingPropertiesChanged rule class
		/// <summary>
		/// Handles the resetting the current primary reading when a new one is selected.
		/// Also rejects trying to change the current primary reading's IsPrimary to false.
		/// Validates that the reading text has the necessary number of placeholders.
		/// </summary>
		[RuleOn(typeof(Reading))]
		private class ReadingPropertiesChanged : ChangeRule
		{
			//so we know when not to run code when items are being set to false
			bool mySettingNewPrimary;

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
						ReadingOrder readingOrder = changedReading.ReadingOrder;
						if (readingOrder != null)
						{
							mySettingNewPrimary = true;
							try
							{
								foreach (Reading r in readingOrder.ReadingCollection)
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
					if (!changedReading.IsRemoved)
					{
						changedReading.ValidateRoleCountError(null);

						TooFewReadingRolesError tooFew;
						TooManyReadingRolesError tooMany;
						if (null != (tooFew = changedReading.TooFewRolesError))
						{
							tooFew.GenerateErrorText();
						}
						if (null != (tooMany = changedReading.TooManyRolesError))
						{
							tooMany.GenerateErrorText();
						}
					}
				}
			}
		}
		#endregion

		#region ReadingOrderHasRoleRemoved rule class
		[RuleOn(typeof(ReadingOrderHasRole))]
		private class ReadingOrderHasRoleRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
				if (!link.IsRemoved)
				{
					ReadingOrder ord = link.ReadingOrder;
					if (!ord.IsRemoved)
					{
						ReadingMoveableCollection readings = ord.ReadingCollection;
						foreach (Reading read in readings)
						{
							read.ValidateRoleCountError(null);
						}
					}
				}
			}
		}
		#endregion

		#endregion // rule classes and helpers

		#region IModelErrorOwner implementation

		/// <summary>
		/// Returns the errors associated with the Reading.
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				TooFewReadingRolesError tooFew;
				TooManyReadingRolesError tooMany;
				if (null != (tooFew = TooFewRolesError))
				{
					yield return tooFew;
				}
				if (null != (tooMany = TooManyRolesError))
				{
					yield return tooMany;
				}
			}
		}

		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}

		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateRoleCountError(notifyAdded);
		}

		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}

		#endregion
	}

	#region class TooFewRolesError
	public partial class TooFewReadingRolesError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooFewRolesMessage, Reading.Text, Model.Name);
			if(Name != newText)
			{
				Name = newText;
			}
		}

		/// <summary>
		/// Sets regernate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get 
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion
		#region IRepresentModelElements Members
		/// <summary>
		/// The reading the error belongs to
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.Reading };
		}

		ModelElement[]  IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}

		#endregion

	}
	#endregion

	#region class TooManyRolesError
	public partial class TooManyReadingRolesError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooManyRolesMessage, Reading.Text, Model.Name);
			if (Name != newText)
			{
				Name = newText;
			}
		}

		/// <summary>
		/// Sets regernate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion

		#region IRepresentModelElements Members
		/// <summary>
		/// The Reading the error belongs too.
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.Reading };
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}

		#endregion
	}
	#endregion
}
