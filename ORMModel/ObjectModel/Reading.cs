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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using System.Globalization;
using Neumont.Tools.ORM.Framework;
namespace Neumont.Tools.ORM.ObjectModel
{
	#region Reading class
	public partial class Reading : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region Base overrides
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
				RoleBaseMoveableCollection roles = ReadingOrder.RoleCollection;
				int roleCount = roles.Count;
				string[] roleNames = new string[roleCount];
				for (int i = 0; i < roleCount; ++i)
				{
					roleNames[i] = roles[i].Role.Name;
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
			if (elemDesc != null)
			{
				Guid attributeId = elemDesc.MetaAttributeInfo.Id;
				if (attributeId == IsPrimaryForReadingOrderMetaAttributeGuid)
				{
					return IsPrimaryForReadingOrder;
				}
				else if (attributeId == IsPrimaryForFactTypeMetaAttributeGuid)
				{
					return IsPrimaryForFactType;
				}
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
		#endregion // Base overrides
		#region IsValidReadingText methods
		private static readonly Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}");
		/// <summary>
		/// Does some testing to see if the reading text is appropriate for the current
		/// set of roles assigned to it. Would eventually be more useful if it provided
		/// some information as to why it was not valid.
		/// </summary>
		/// <returns>True if the reading is deemed valid, false otherwise.</returns>
		public bool IsValidReadingText()
		{
			ReadingOrder readOrd = ReadingOrder;
			RoleBaseMoveableCollection roles = readOrd.RoleCollection;
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
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeId = attribute.Id;
			bool checkFact = false;
			if (attributeId == IsPrimaryForReadingOrderMetaAttributeGuid ||
				(checkFact = (attributeId == IsPrimaryForFactTypeMetaAttributeGuid)))
			{
				ReadingOrder order;
				if (!IsRemoved &&
					null != (order = ReadingOrder) &&
					object.ReferenceEquals(this, order.ReadingCollection[0]))
				{
					if (checkFact)
					{
						FactType factType = order.FactType;
						if (factType != null &&
							object.ReferenceEquals(order, factType.ReadingOrderCollection[0]))
						{
							return true;
						}
					}
					else
					{
						return true;
					}
				}
				return false;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in ReadingPropertiesChanged rule class.
		/// </summary>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == IsPrimaryForReadingOrderMetaAttributeGuid ||
				attributeGuid == IsPrimaryForFactTypeMetaAttributeGuid)
			{
				// Handled by ReadingPropertiesChanged
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		#endregion // CustomStorage handlers
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
						tooFewError.Reading = this;
						tooFewError.Model = theModel;
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
						tooManyError.Reading = this;
						tooManyError.Model = theModel;
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
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				Reading changedReading = e.ModelElement as Reading;
				if (changedReading.IsRemoved)
				{
					return;
				}
				bool moveReadingOrder = false;
				if (attributeGuid == Reading.IsPrimaryForReadingOrderMetaAttributeGuid ||
					(moveReadingOrder = (attributeGuid == Reading.IsPrimaryForFactTypeMetaAttributeGuid)))
				{
					if (!((bool)e.NewValue))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReadingIsPrimaryToFalse);
					}
					ReadingOrder order;
					if (null != (order = changedReading.ReadingOrder))
					{
						ReadingMoveableCollection readings = order.ReadingCollection;
						if (readings.Count > 1 && !object.ReferenceEquals(readings[0], changedReading))
						{
							readings.Move(changedReading, 0);
						}
						if (moveReadingOrder)
						{
							FactType factType;
							if (null != (factType = order.FactType))
							{
								ReadingOrderMoveableCollection readingOrders = factType.ReadingOrderCollection;
								if (readingOrders.Count > 1 && !object.ReferenceEquals(readingOrders[0], order))
								{
									readingOrders.Move(order, 0);
								}
							}
						}
					}
				}
				else if (attributeGuid == Reading.TextMetaAttributeGuid)
				{
					string newValue = (string)e.NewValue;
					if (newValue.Length == 0)
					{
						changedReading.Remove();
					}
					else
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
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)))
			{
				TooFewReadingRolesError tooFew;
				if (null != (tooFew = TooFewRolesError))
				{
					yield return new ModelErrorUsage(tooFew, ModelErrorUses.BlockVerbalization);
				}
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				TooManyReadingRolesError tooMany;
				if (null != (tooMany = TooManyRolesError))
				{
					yield return new ModelErrorUsage(tooMany, ModelErrorUses.Verbalize);
				}
			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateRoleCountError(notifyAdded);
		}

		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new static void DelayValidateErrors()
		{
			// UNDONE: DelayedValidation (Reading)
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion
		#region IHasIndirectModelErrorOwner Implementation
		private static readonly Guid[] myIndirectModelErrorOwnerLinkRoles = new Guid[] { ReadingOrderHasReading.ReadingCollectionMetaRoleGuid };
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			return myIndirectModelErrorOwnerLinkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // Reading class
	#region TooFewReadingRolesError class
	public partial class TooFewReadingRolesError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading = Reading;
			ReadingOrder order = reading.ReadingOrder;
			FactType fact = order.FactType;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooFewRolesMessage, fact.Name, Model.Name, Reading.Text);
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
	#endregion // TooFewReadingRolesError class
	#region TooManyReadingRolesError class
	public partial class TooManyReadingRolesError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading = Reading;
			ReadingOrder order = reading.ReadingOrder;
			FactType fact = order.FactType;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorReadingTooManyRolesMessage, fact.Name, Model.Name, Reading.Text);
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
	#endregion // TooManyReadingRolesError class
}
