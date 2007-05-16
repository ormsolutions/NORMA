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
using Neumont.Tools.Modeling;
namespace Neumont.Tools.ORM.ObjectModel
{
	#region IReading interface
	/// <summary>
	/// An abstract form of a <see cref="Reading"/> element. Designed
	/// to allow pseudo-readings that are not stored with the model, such
	/// as the "is" reading used by a <see cref="SubtypeFact"/>
	/// </summary>
	public interface IReading
	{
		/// <summary>
		/// The reading text with replacement fields corresponding
		/// to the order in the <see cref="RoleCollection"/>
		/// </summary>
		string Text { get;}
		/// <summary>
		/// An order list of <see cref="RoleBase"/> elements associated
		/// with the reading.
		/// </summary>
		IList<RoleBase> RoleCollection { get;}
		/// <summary>
		/// Return true if the reading text is editable
		/// </summary>
		bool IsEditable { get;}
	}
	#endregion // IReading interface
	#region Reading text utility delegates
	/// <summary>
	/// A callback function for use with the <see cref="Reading.ReplaceFields(string,ReadingTextReplace)"/> method. Provides
	/// a replacement string for the provided <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range.</param>
	/// <returns>A string to replace the placeholder for the given index. Return <see langword="null"/> to leave the replacement field.</returns>
	public delegate string ReadingTextReplace(int index);
	/// <summary>
	/// A callback function for use with the <see cref="Reading.ReplaceFields(string,ReadingTextReplaceWithFieldGroup)"/> method. Provides
	/// a replacement string for the provided <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range.</param>
	/// <param name="match">The <see cref="Match"/> information for this replacement. Contains information about the replacement location in the provided reading text.</param>
	/// <returns>A string to replace the placeholder for the given index. Return <see langword="null"/> to leave the replacement field.</returns>
	public delegate string ReadingTextReplaceWithFieldGroup(int index, Match match);
	/// <summary>
	/// A callback function for use with the <see cref="Reading.VisitFields"/> method. Called once for
	/// each integer replacement field.
	/// </summary>
	/// <param name="index">The zero-based index of the replacement field. The index is relative to the <see cref="ReadingOrder.RoleCollection"/>
	/// for the <see cref="ReadingOrder"/> associated with the <see cref="Reading"/> that owns this text. The index is not guaranteed to be in range, and
	/// the same index may be returned multiple times.</param>
	/// <returns><see langword="true"/> to continue, or <see langword="false"/> to stop visiting.</returns>
	public delegate bool ReadingTextVisit(int index);
	#endregion // Reading text utility delegates
	#region Reading class
	public partial class Reading : IModelErrorOwner, IHasIndirectModelErrorOwner, IReading
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
				LinkedElementCollection<RoleBase> roles = ReadingOrder.RoleCollection;
				int roleCount = roles.Count;
				string[] roleNames = new string[roleCount];
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = roles[i].Role;
					string name = role.Name;
					if (name.Length == 0)
					{
						ObjectType rolePlayer = role.RolePlayer;
						if (rolePlayer != null)
						{
							name = rolePlayer.Name;
						}
					}
					roleNames[i] = (name.Length != 0) ? name : ("{" + i.ToString(CultureInfo.InvariantCulture) + "}");
				}
				return string.Format(CultureInfo.InvariantCulture, this.Text, roleNames);
			}
			else
			{
				return this.Text;
			}
		}
		#endregion // Base overrides
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
			LinkedElementCollection<RoleBase> roles = readOrd.RoleCollection;
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

			MatchCollection matches = FieldRegex.Matches(testText);

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
				nrList[i] = int.Parse(matches[i].Groups[ReplaceFieldsMatchIndexGroupName].Value, CultureInfo.InvariantCulture);
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
			return FieldRegex.Matches(textText).Count;
		}
		#endregion
		#region CustomStorage handlers
		private bool GetIsPrimaryForReadingOrderValue()
		{
			ReadingOrder order;
			return !IsDeleted && (order = ReadingOrder) != null && order.ReadingCollection[0] == this;
		}
		private bool GetIsPrimaryForFactTypeValue()
		{
			ReadingOrder order;
			FactType factType;
			return GetIsPrimaryForReadingOrderValue() && (factType = (order = ReadingOrder).FactType) != null && factType.ReadingOrderCollection[0] == order;
		}
		private void SetIsPrimaryForReadingOrderValue(bool newValue)
		{
			// Handled by ReadingPropertiesChanged
		}
		private void SetIsPrimaryForFactTypeValue(bool newValue)
		{
			// Handled by ReadingPropertiesChanged
		}
		#endregion // CustomStorage handlers
		#region Reading Specific Methods
		/// <summary>
		/// The Text property has been automatically modified by another rule
		/// </summary>
		public void SetAutoText(string value)
		{
			ReadingOrder order;
			FactType factType;
			ORMModel model;
			if (!string.IsNullOrEmpty(value) &&
				null != (order = ReadingOrder) &&
				null != (factType = order.FactType) &&
				null != (model = factType.Model))
			{
				Store store = Store;
				RuleManager ruleManager = store.RuleManager;
				Type ruleType = typeof(ReadingPropertiesChanged);
				ruleManager.DisableRule(ruleType);
				try
				{
					// Explicitly call helper that is called by the change rule
					ReadingTextChanged(value);

					// Set text property here. We do this after the modifications
					// were made that would normally trigger the ReadingTextChanged
					// so that this change is made early
					Text = value;

					// Create the error if we don't already have one. GenerateErrorText
					// will be called automatically because changing any reading text triggers
					// a FactTypeNamePartChanged and this error includes the fact type name.
					if (RequiresUserModificationError == null)
					{
						ReadingRequiresUserModificationError newError = new ReadingRequiresUserModificationError(store);
						newError.Reading = this;
						newError.Model = model;
						newError.GenerateErrorText();
					}
				}
				finally
				{
					ruleManager.EnableRule(ruleType);
				}
			}
			else
			{
				Text = value;
			}
		}
		#endregion // Reading Specific Methods
		#region rule classes and helpers
		/// <summary>
		/// Delay validation callback for <see cref="ValidateRoleCountError"/> method
		/// </summary>
		private static void DelayValidateRoleCountError(ModelElement element)
		{
			((Reading)element).ValidateRoleCountError(null);
		}
		/// <summary>
		/// Compares number of roles in ReadingOrder ot the place holders in
		/// the reading and then creates or removes errors as needed.
		/// </summary>
		private void ValidateRoleCountError(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool removeTooFew = false;
				bool removeTooMany = false;
				TooFewReadingRolesError tooFewError;
				TooManyReadingRolesError tooManyError;
				ReadingOrder readingOrder = ReadingOrder;
				ORMModel theModel = readingOrder.FactType.Model;
				Store store = Store;
				LinkedElementCollection<RoleBase> orderRoles = readingOrder.RoleCollection;
				int numRoles = orderRoles.Count;
				int deletingCount = 0;
				for (int i = 0; i < numRoles; ++i)
				{
					if (orderRoles[i].IsDeleting)
					{
						++deletingCount;
					}
				}
				numRoles -= deletingCount;
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
					if (null == (tooFewError = TooFewRolesError))
					{
						tooFewError = new TooFewReadingRolesError(store);
						tooFewError.Reading = this;
						tooFewError.Model = theModel;
						tooFewError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooFewError, true);
						}
					}
					else
					{
						tooFewError.GenerateErrorText();
					}
				}
				//too many roles
				else
				{
					removeTooFew = true;
					if (null == (tooManyError = TooManyRolesError))
					{
						tooManyError = new TooManyReadingRolesError(store);
						tooManyError.Reading = this;
						tooManyError.Model = theModel;
						tooManyError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(tooManyError, true);
						}
					}
					else
					{
						tooManyError.GenerateErrorText();
					}
				}

				if (removeTooFew && null != (tooFewError = TooFewRolesError))
				{
					tooFewError.Delete();
				}
				if (removeTooMany && null != (tooManyError = TooManyRolesError))
				{
					tooManyError.Delete();
				}
			}
		}

		#region ReadingPropertiesChanged rule class
		/// <summary>
		/// Handles the resetting the current primary reading when a new one is selected.
		/// Also rejects trying to change the current primary reading's IsPrimary to false.
		/// Validates that the reading text has the necessary number of placeholders.
		/// </summary>
		[RuleOn(typeof(Reading))] // ChangeRule
		private sealed partial class ReadingPropertiesChanged : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				Reading changedReading = e.ModelElement as Reading;
				if (changedReading.IsDeleted)
				{
					return;
				}
				bool moveReadingOrder = false;
				if (attributeGuid == Reading.IsPrimaryForReadingOrderDomainPropertyId ||
					(moveReadingOrder = (attributeGuid == Reading.IsPrimaryForFactTypeDomainPropertyId)))
				{
					if (!((bool)e.NewValue))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionReadingIsPrimaryToFalse);
					}
					ReadingOrder order;
					if (null != (order = changedReading.ReadingOrder))
					{
						LinkedElementCollection<Reading> readings = order.ReadingCollection;
						if (readings.Count > 1 && readings[0] != changedReading)
						{
							readings.Move(changedReading, 0);
						}
						if (moveReadingOrder)
						{
							FactType factType;
							if (null != (factType = order.FactType))
							{
								LinkedElementCollection<ReadingOrder> readingOrders = factType.ReadingOrderCollection;
								if (readingOrders.Count > 1 && readingOrders[0] != order)
								{
									readingOrders.Move(order, 0);
								}
							}
						}
					}
				}
				else if (attributeGuid == Reading.TextDomainPropertyId)
				{
					changedReading.ReadingTextChanged((string)e.NewValue);
					if (!changedReading.IsDeleted)
					{
						ReadingRequiresUserModificationError modificationError;
						if (null != (modificationError = changedReading.RequiresUserModificationError))
						{
							modificationError.Delete();
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes empty readings or calls delay validate.
		/// </summary>
		/// <param name="newValue">The new string value. This may be called before the
		/// Text property is changed, so the value needs to be provided separately.</param>
		private void ReadingTextChanged(string newValue)
		{
			if (string.IsNullOrEmpty(newValue))
			{
				Delete();
			}
			else
			{
				ORMCoreDomainModel.DelayValidateElement(this, DelayValidateRoleCountError);
			}
		}

		#endregion // ReadingPropertiesChanged rule class
		#region ReadingOrderHasRoleRemoved rule class
		[RuleOn(typeof(ReadingOrderHasRole))] // DeleteRule
		private sealed partial class ReadingOrderHasRoleDeleted : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ReadingOrderHasRole link = e.ModelElement as ReadingOrderHasRole;
				ReadingOrder ord = link.ReadingOrder;
				if (!ord.IsDeleted)
				{
					LinkedElementCollection<Reading> readings = ord.ReadingCollection;
					foreach (Reading read in readings)
					{
						ORMCoreDomainModel.DelayValidateElement(read, DelayValidateRoleCountError);
					}
				}
			}
		}
		#endregion // ReadingOrderHasRoleRemoved rule class
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
					yield return new ModelErrorUsage(tooFew, ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary);
				}
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				TooManyReadingRolesError tooMany;
				if (null != (tooMany = TooManyRolesError))
				{
					yield return new ModelErrorUsage(tooMany, ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary);
				}
				ReadingRequiresUserModificationError userModificationRequired;
				if (null != (userModificationRequired = RequiresUserModificationError))
				{
					yield return new ModelErrorUsage(userModificationRequired, ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary);
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
		protected new void DelayValidateErrors()
		{
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateRoleCountError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ReadingOrderHasReading.ReadingDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IReading Implementation
		/// <summary>
		/// Implements <see cref="IReading.RoleCollection"/>
		/// </summary>
		protected IList<RoleBase> RoleCollection
		{
			get
			{
				ReadingOrder order = ReadingOrder;
				return (order != null) ? order.RoleCollection : null;
			}
		}
		IList<RoleBase> IReading.RoleCollection
		{
			get
			{
				return RoleCollection;
			}
		}
		/// <summary>
		/// Implements <see cref="IReading.IsEditable"/>. Always returns <see langword="true"/>
		/// </summary>
		public static bool IsEditable
		{
			get
			{
				return true;
			}
		}
		bool IReading.IsEditable
		{
			get
			{
				return IsEditable;
			}
		}
		#endregion // IReading Implementation
		#region Reading text utility fields and methods
		/// <summary>
		/// Named field for the <see cref="Match"/> passed to the <see cref="ReadingTextReplaceWithFieldGroup"/> delegate.
		/// Represents the entire replacement field, including the curly braces.
		/// </summary>
		public const string ReplaceFieldsMatchFieldGroupName = "Field";
		/// <summary>
		/// Named field for the <see cref="Match"/> passed to the <see cref="ReadingTextReplaceWithFieldGroup"/> delegate.
		/// Represents the index portion of the  replacement field, not including the curly braces.
		/// </summary>
		public const string ReplaceFieldsMatchIndexGroupName = "Index";
		private static Regex myFieldRegex;

		/// <summary>
		/// Visits the fields in the reading to verify the text can be correctly modified.
		/// </summary>
		/// <param name="readingText">The reading text.<see cref="Reading"/></param>
		/// <param name="visitCallback">The visit callback.</param>
		/// <returns><see langword="true"/> to continue, or <see langword="false"/> to stop visiting.</returns>
		public static bool VisitFields(string readingText, ReadingTextVisit visitCallback)
		{
			Match visitMatch = FieldRegex.Match(readingText);
			while (visitMatch.Success)
			{
				int index;
				if (int.TryParse(visitMatch.Groups[ReplaceFieldsMatchIndexGroupName].Value, NumberStyles.None, CultureInfo.InvariantCulture, out index))
				{
					if (!visitCallback(index))
					{
						return false;
					}
				}
				visitMatch = visitMatch.NextMatch();
			}
			return true;
		}
		/// <summary>
		/// Replaces the fields in the reading text.
		/// </summary>
		/// <param name="readingText">The reading text.</param>
		/// <param name="replacementText">Text to replace the fields with.</param>
		/// <returns>Reading text with modified replacement fields.</returns>
		public static string ReplaceFields(string readingText, string replacementText)
		{
			return FieldRegex.Replace(readingText, replacementText);
		}
		/// <summary>
		/// Replaces the fields in the reading text.
		/// </summary>
		/// <param name="readingText">The reading text.</param>
		/// <param name="replaceCallback">The replace callback. See <see cref="ReadingTextReplace"/>.</param>
		/// <returns>Reading text with modified replacement fields.</returns>
		public static string ReplaceFields(string readingText, ReadingTextReplace replaceCallback)
		{
			return FieldRegex.Replace(
				readingText,
				delegate(Match match)
				{
					int index;
					string retVal = null;
					if (int.TryParse(match.Groups[ReplaceFieldsMatchIndexGroupName].Value, NumberStyles.None, CultureInfo.InvariantCulture, out index))
					{
						//Index param to the delegate
						retVal = replaceCallback(index);
					}
					return (retVal != null) ? retVal : match.Value;
				});
		}
		/// <summary>
		/// Replaces the fields in the reading text.
		/// </summary>
		/// <param name="readingText">The reading text. See <see cref="ReadingTextReplaceWithFieldGroup"/>.</param>
		/// <param name="replaceCallback">The replace callback.</param>
		/// <returns>Reading text with modified replacement fields.</returns>
		public static string ReplaceFields(string readingText, ReadingTextReplaceWithFieldGroup replaceCallback)
		{
			return FieldRegex.Replace(
				readingText,
				delegate(Match match)
				{
					int index;
					string retVal = null;
					if (int.TryParse(match.Groups[ReplaceFieldsMatchIndexGroupName].Value, NumberStyles.None, CultureInfo.InvariantCulture, out index))
					{
						//Index param to the delegate
						retVal = replaceCallback(index, match);
					}
					return (retVal != null) ? retVal : match.Value;
				});
		}
		/// <summary>
		/// The regular expression used to find fields and indices in
		/// a reading format string. Captures named groups corresponding
		/// to the <see cref="ReplaceFieldsMatchFieldGroupName"/> and <see cref="ReplaceFieldsMatchIndexGroupName"/>
		/// constants.
		/// </summary>
		private static Regex FieldRegex
		{
			get
			{
				Regex retVal = myFieldRegex;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myFieldRegex,
						new Regex(
							@"(?n)\.*?(?<Field>((?<!\{)\{)(?<Index>[0-9]+)(\}(?!\})))",
							RegexOptions.Compiled),
						null);
					retVal = myFieldRegex;
				}
				return retVal;
			}
		}
		#endregion // Reading text utility fields and methods
	}
	#endregion // Reading class
	#region TooFewReadingRolesError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
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
			if (ErrorText != newText)
			{
				ErrorText = newText;
			}
		}

		/// <summary>
		/// Sets regenerate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion // overrides
		#region IRepresentModelElements Members
		/// <summary>
		/// The reading the error belongs to
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
	#endregion // TooFewReadingRolesError class
	#region TooManyReadingRolesError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
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
			if (ErrorText != newText)
			{
				ErrorText = newText;
			}
		}

		/// <summary>
		/// Sets regenerate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion // overrides
		#region IRepresentModelElements Implementation
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
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // TooManyReadingRolesError class
	#region ReadingRequiresUserModificationError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	public partial class ReadingRequiresUserModificationError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Creates the error text.
		/// </summary>
		public override void GenerateErrorText()
		{
			Reading reading = Reading;
			if (reading != null)
			{
				ReadingOrder order = reading.ReadingOrder;
				string newText = string.Format(
					CultureInfo.InvariantCulture,
					ResourceStrings.ModelErrorReadingRequiresUserModificationMessage,
					order.FactType.Name,
					Model.Name,
					GenerateReadingText(reading.Text, order));
				if (ErrorText != newText)
				{
					ErrorText = newText;
				}
			}
		}

		/// <summary>
		/// Sets regenerate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}

		private static string GenerateReadingText(string readingText, ReadingOrder order)
		{
			LinkedElementCollection<RoleBase> roles = order.RoleCollection;
			int roleCount = roles.Count;
			return Reading.ReplaceFields(
				readingText,
				delegate(int index)
				{
					if (index < roleCount)
					{
						ObjectType rolePlayer = roles[index].Role.RolePlayer;
						return (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText;
					}
					return null;
				});
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { Reading };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // ReadingHasReadingRequiresUserModificationError class
}
